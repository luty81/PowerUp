using Dapper;
using PowerUp.SQL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.Database
{
    public class CommandBatching
    {
        public CommandBatching Insert(object entity) =>
            Insert(entity, null, (null, null));

        public CommandBatching Insert(object entity, string customTableName) =>
            Insert(entity, customTableName, (null, null));

        public CommandBatching Insert<T>(IEnumerable<T> entities) where T : class
        {
            entities.ForEach(e => Insert(e));
            return this;
        }

        public CommandBatching InsertUUID(object entity) =>
            InsertUUID(entity, customTableName: null);

        public CommandBatching InsertUUID(object entity, string customTableName) =>
            Insert(entity, customTableName, AutoKey(entity));

        public CommandBatching InsertUUID(IEnumerable<object> objects)
        {
            objects.ForEach(o => InsertUUID(o));
            return this;

        }


        public CommandBatching Update<T>(T obj, string customTableName = null) where T : class, new()
        {
            var commandText = $"{SqlFor<T>.GetUpdate(obj, customTableName)};";
            _batchOperations.Add(new BatchOperation(commandText, obj));
            return this;
        }
        
        
        public CommandBatching AddRawSQL(string sql, object @params)
        {
            _batchOperations.Add(new BatchOperation(sql, @params));
            return this;
        }

        public virtual async Task Commit<TConnection>(TConnection dbConnection) 
            where TConnection : DbConnection
        {
            using var transaction = await dbConnection.BeginTransactionAsync();
            var batchTasks = 
                Operations.Select(op => dbConnection.ExecuteAsync(op.CommandSQL, op.Entity)).ToArray();

            Task.WaitAll(batchTasks);
            await transaction.CommitAsync();
        }

        public IEnumerable<BatchOperation> Operations { get => _batchOperations; }

        private CommandBatching Insert(object obj, string tableName, (string column, string param) customParam)
        {
            var customParams = Enumerable.Empty<(string c, string p)>().ToList();
            if (customParam.column != null)
                customParams.Add(customParam);

            var commandText = new CommandBuilder(obj)
                .For<InsertCommand>(tableName, customParams.ToArray());

            _batchOperations.Add(new BatchOperation(commandText, obj));
            return this;
        }

        private (string keyColumn, string dbFuncName) AutoKey(object entity, string dbFunction = null)
        {
            var keyFieldName = entity.GetType().KeyColumnNames().Single();
            dbFunction ??= isBigInt() ? "UUID_SHORT()" : "UUID()";
            return (keyFieldName, dbFunction);

            bool isBigInt() => entity.GetType().GetProperty(keyFieldName).PropertyType == typeof(ulong);
        }

        private readonly IList<BatchOperation> _batchOperations = new List<BatchOperation>();
    }

    public class BatchOperation
    {
        public string CommandSQL { get; set; }
        public object Entity { get; set; }

        public BatchOperation(string sql, object entity)
        {
            CommandSQL = sql;
            Entity = entity;
        }
    }
}
