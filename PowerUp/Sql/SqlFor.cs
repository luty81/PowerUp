using PowerUp.SQL.Commands;
using System;
using System.Linq.Expressions;
using static PowerUp.SQL.ColumnsMode;

namespace PowerUp.SQL
{
    public class Sql
    {
        public static SelectBuilder<Table> For<Table>(ColumnsMode mode = ResolveAliasesAndNames) where Table : class
            => new SelectBuilder<Table>(mode);

        public static SelectBuilder<Table> SelectFor<Table>() where Table : class
            => new SelectBuilder<Table>(ResolveAliasesAndNames);
    }

    public static class SqlFor<T> where T : class, new()
    {
        public static string GetUpdate(T obj, string table = null) => 
            Get<UpdateCommand>(obj, table);
        public static string GetInsert(T obj, bool dontSetKeyFields, string table = null) => 
            Get<InsertCommand>(obj, table, dontSetKeyFields);
        public static string GetDelete(T _, string table = null) => 
            DeleteCommand.For<T>(table);

        public static string GetSelectStar() => new SelectBuilder<T>().SelectAll;
        public static string GetQuery(Expression<Func<T, object>> column) =>
            new SelectBuilder<T>(ResolveAliasesAndNames)
                .Where(column)
                .Done();

        private static string Get<TCommand>(T obj, string table, bool dontSetKeyFields = true) 
            where TCommand : ICommand, new() =>
                new CommandBuilder(obj, dontSetKeyFields).For<TCommand>(table);

    }

    public static class SqlForExtensions
    {
        public static string Close(this string self) => self += ";";
    }
}