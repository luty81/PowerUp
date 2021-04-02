using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;


namespace PowerUp.SQL
{
    public static class KeyColumnsExtractor
    {
        public static IEnumerable<PropertyInfo> KeyColumns(this Type self) =>
            self.GetPropertiesWith<KeyAttribute>();

        public static IEnumerable<string> KeyColumnNames(this Type self) =>
            self.KeyColumns().Select(p => p.Name).DefaultIfEmpty("Id");


        public static IEnumerable<string> Names(IEnumerable<PropertyInfo> properties) =>
            properties.Where(p => p.HasAttribute<KeyAttribute>())
                .Select(p => p.Name)
                .DefaultIfEmpty("Id");

        public static string ToWhereByKey(this IEnumerable<string> columns) =>
            @$"WHERE {columns.Select(c => @$"{c} = @{c}").Join(" AND ")}";

        public static string ToWhereByKey(this IEnumerable<PropertyInfo> properties) =>
            Names(properties).ToWhereByKey();
    }


}
