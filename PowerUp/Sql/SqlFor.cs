using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static PowerUp.Sql.ColumnsMode;

namespace PowerUp.Sql
{
    public class SqlFor<T> where T : class
    {
        //public WhereBuilder<T> Where { get; private set; }
        public WhereBuilder<T> _where;
        public string Select => Done();

        public SqlFor<T> Where(Expression<Func<T, object>> columnSelector)
        {
            _where.AddClause(columnSelector);
            return this;
        }

        public string Done()
        {
            _builder.Append(_where.Done());
            _builder.Append(";");
            return _builder.ToString();
        }

        public SqlFor(ColumnsMode mode = Star)
        {
            _mode = mode;
            _builder = new StringBuilder();
            _builder.AppendLine($@"SELECT {ColumnList().Join(", ")} ");
            _builder.AppendLine($@"FROM {TableName} {TableAlias} ");

            _where = new WhereBuilder<T>(TableAlias);
            
        }

        private readonly ColumnsMode _mode;

        private readonly StringBuilder _builder;

        private static string TableName => typeof(T).Name;
        private static IEnumerable<string> ColumnNames => typeof(T).GetProperties().Select(p => p.Name);
        private string TableAlias => _mode != ResolveAliasesAndNames ? string.Empty :
            TableName.SplitByPascalCase()
                .Select(x => x.First().ToString()).ToList()
                .Join();
        private IEnumerable<string> ColumnList() => _mode switch
        {
            Star => new[] { "*" },
            ResolveNames => ColumnNames,
            ResolveAliasesAndNames => ColumnNames.Select(c => $"{TableAlias}.{c}"),
            _ => null,
        };
    }

    public enum ColumnsMode { Star, ResolveNames, ResolveAliasesAndNames }
}