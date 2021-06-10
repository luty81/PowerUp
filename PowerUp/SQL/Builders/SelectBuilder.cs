using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using static PowerUp.SQL.ColumnsMode;

namespace PowerUp.SQL
{
    public class SelectBuilder<T> where T : class
    {
        public SelectBuilder<T> Select => this;

        public string SelectAll => Done();

        public WhereBuilder<T> Where(Expression<Func<T, object>> columnSelector) =>
            _where.AddClause(columnSelector);

        public WhereBuilder<T> WhereLike(Expression<Func<T, object>> columnSelector) =>
            _where.AddLikeClause(columnSelector);

        public override string ToString() => Done();

        public string Done() => _builder
            .Append(_where.ToString())
            .Append(';')
            .ToString();

        public string TableAlias =>
            _mode != ResolveAliasesAndNames ? string.Empty :
                TableName.SplitByPascalCase()
                    .Select(x => x.FirstCharacter())
                    .Join();


        public SelectBuilder(ColumnsMode mode = Star)
        {
            _mode = mode;
            _where = new WhereBuilder<T>(this);
            _builder = new StringBuilder()
                .AppendLine($@"SELECT {ColumnList().Join(", ")} ")
                .AppendLine($@"FROM {TableName} {TableAlias} ");
        }

        private static string TableName => typeof(T).Name;
        private static IEnumerable<string> ColumnNames => typeof(T).GetProperties().Select(p => p.Name);
        private IEnumerable<string> ColumnList() => _mode switch
        {
            Star => new[] { "*" },
            ResolveNames => ColumnNames,
            ResolveAliasesAndNames => ColumnNames.Select(c => $"{TableAlias}.{c}"),
            _ => null,
        };

        private readonly ColumnsMode _mode;
        private readonly WhereBuilder<T> _where;
        private readonly StringBuilder _builder;
    }
}
