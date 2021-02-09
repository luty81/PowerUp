using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Reflection.BindingFlags;

namespace PowerUp.SQL
{
    public interface ICommand
    {
        string Build(string tableName, IEnumerable<string> columns);
    }

    public class InsertCommand: ICommand
    {
        public string Build(string tableName, IEnumerable<string> columns) =>
            _builder
                .AppendLine($"INSERT INTO {tableName}")
                .AppendLine($"({columns.JoinByComma()})")
                .AppendLine("VALUES")
                .AppendLine($"({columns.Select(@P).JoinByComma()})")
            .ToString();

        private static string @P(string columnName) => $"@{columnName}";

        private readonly StringBuilder _builder = new StringBuilder();
    }

    public class UpdateCommand: ICommand
    {
        public string Build(string tableName, IEnumerable<string> columns) =>
            _builder
                .AppendLine($"UPDATE {tableName}")
                .AppendLine($"SET {columns.Where(c => c != "Id").Select(ColumnAssignment).JoinByComma()}")
                .AppendLine($"WHERE Id = @Id")
            .ToString();

        private static string ColumnAssignment(string column) => $"{column} = @{column}";

        private readonly StringBuilder _builder = new StringBuilder();
    }

    public class DeleteCommandFor<T>
    {
        public static string Build(string tableName = null) => 
            new StringBuilder()
                .AppendLine($"DELETE FROM {tableName ?? typeof(T).Name}")
                .Append("WHERE Id = @Id")
            .ToString();

   }

    public class CommandBuilder
    {
        public IEnumerable<PropertyInfo> AssignedProperties { get; set; }

        public string For<T>(string tableName) where T : ICommand, new()
        {
            var columns = Columns.Select(p => p.Name);
            if (columns.Any())
                return new T().Build(tableName, Columns.Select(p => p.Name));
                    
            return null;
        }

        public CommandBuilder(object @object, bool ignoreUnassignedProperties = true)
        {
            _object = @object;
            Columns = _object.GetType().GetProperties(_getters);

            var keyColumns = Columns.Where(c => c.GetCustomAttribute<KeyAttribute>() != null);

            AssignedProperties = Columns.Where(HasValue);
            if (ignoreUnassignedProperties)
                Columns = AssignedProperties;
        }
        
        protected readonly IEnumerable<PropertyInfo> Columns;

        private bool HasValue(PropertyInfo propertyInfo)
        {
            var @value = propertyInfo.GetValue(_object);
            if (@value == null) return false;
            
            if (propertyInfo.PropertyType.IsValueType) // If the value is the type default, then we assume it is unassigned
                return ! Activator.CreateInstance(@value.GetType()).Equals(@value);

            return true;
        }
        private static BindingFlags _getters => Public | Instance | GetProperty;
        private readonly object _object;
    }
}
