using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static PowerUp.SQL.ColumnsMode;

namespace PowerUp.SQL
{
    public class Sql
    {
        public static SqlBuilder<Table> For<Table>(ColumnsMode mode = ResolveAliasesAndNames) where Table : class
            => new SqlBuilder<Table>(mode);

        public static SqlBuilder<Table> SelectFor<Table>() where Table : class
            => new SqlBuilder<Table>(ResolveAliasesAndNames);
    }

    public static class SqlFor<T> where T : class, new()
    {
        public static SqlBuilder<T> Select =>
            new SqlBuilder<T>(ResolveAliasesAndNames);

        public static string GetSelectStar() =>
            new SqlBuilder<T>().SelectAll;

        public static string GetQueryWithFilter(Expression<Func<T, object>> column) =>
            new SqlBuilder<T>(ResolveAliasesAndNames)
                .Where(column)
                .Done();
    }
}