using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PowerUp.SQL
{
    public class DeleteCommand
    {
        public static string For<T>(string tableName = null)
        {
            return Build(typeof(T), tableName);
        }
        public static string For(object entityToDelete, string tableName = null)
        {
            return Build(entityToDelete.GetType(), tableName);
        }

        private static string Build(Type entityType, string tableName)
        {
            tableName ??= entityType.GetCustomAttribute<TableAttribute>()?.Name ?? entityType.Name;

            return new StringBuilder()
                .AppendLine($"DELETE FROM {tableName}")
                .AppendLine($"WHERE {entityType.KeyFilter()};")
            .ToString();
        }

    }

    public static class KeyFilterExtractor
    {
        public static string KeyFilter(this Type type) =>
            type.KeyColumnNames()
                .Select(p => $"{p} = @{p}")
                .Join(" AND ");
    }


}
