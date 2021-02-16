using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PowerUp.SQL
{
    public interface ICommand
    {
        string Build(string tableName, IEnumerable<(PropertyInfo column, string param)> columns);
    }

    public class InsertCommand : ICommand
    {
        public string Build(string tableName, IEnumerable<(PropertyInfo column, string param)> columns) =>
            _builder
                .AppendLine($"INSERT INTO {tableName}")
                .AppendLine($"({columns.Select(c => c.column.Name).JoinByComma()})")
                .AppendLine("VALUES")
                .AppendLine($"({columns.Select(c => c.param ?? $"@{c.column.Name}").JoinByComma()})")
            .ToString();

        private readonly StringBuilder _builder = new StringBuilder();
    }

    public class UpdateCommand : ICommand
    {
        public string Build(string tableName, IEnumerable<(PropertyInfo column, string param)> fields)
        {
            var keyFields = KeyColumnsExtractor.Names(fields.Select(f => f.column));

            var columnsToSet = fields
                .Where(f => keyFields.NotContains(f.column.Name))
                .Select(f => $"{f.column.Name} = {f.param}");

            return _builder
                .AppendLine($"UPDATE {tableName}")
                .AppendLine($"SET {columnsToSet.JoinByComma()}")
                .AppendLine($"{keyFields.ToWhereByKey()}")
            .ToString();
        }

        private readonly StringBuilder _builder = new StringBuilder();
    }

    public class DeleteCommandFor<T>
    {
        public static string Build(string tableName = null)
        {
            tableName ??= typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T).Name;

            return new StringBuilder()
                .AppendLine($"DELETE FROM {tableName}")
                .AppendLine($"{ByKeys<T>.Where};")
            .ToString();
        }
    }

    public class CommandBuilder
    {
        public IEnumerable<PropertyInfo> AssignedProperties { get; set; }

        public string For<T>(string tableName = null, params (string field, string param)?[] customAssigments)
            where T : ICommand, new()
        {
            tableName ??= TableNameFromType();

            var fields =
                from PropertyInfo column in Columns
                let customAssign = customAssigments.FirstOrDefault(x => x.Value.field == column.Name)
                let @param = customAssign?.param ?? $"@{column.Name}"
                select (column, @param);

            if (fields.Any())
                return new T().Build(tableName, fields);

            return null;
        }

        public CommandBuilder(object @object, bool ignoreUnassignedProperties = true)
        {
            _object = @object;
            
            Columns = _object.GetType().Getters();
            AssignedProperties = Columns.Where(HasValue);
            if (ignoreUnassignedProperties)
                Columns = AssignedProperties;
        }

        protected readonly IEnumerable<PropertyInfo> Columns;

        private string TableNameFromType() => _object.GetType().GetCustomAttribute<TableAttribute>()?.Name ?? _object.GetType().Name;
        private bool HasValue(PropertyInfo propertyInfo)
        {
            var @value = propertyInfo.GetValue(_object);
            if (@value == null) return false;

            if (propertyInfo.PropertyType.IsValueType) // If the value is the type default, then we assume it is unassigned
                return !Activator.CreateInstance(@value.GetType()).Equals(@value);

            return true;
        }
        private readonly object _object;
    }

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

    public static class ByKeys<T>
    {
        public static string Clause => 
            typeof(T).KeyColumnNames()
                .Select(p => $"{p} = @{p}")
                .Join(" AND ");

        public static string AndClause(IEnumerable<PropertyInfo> properties) =>
            properties
                .Select(p => $"{p} = @{p}")
                .Join(" AND ");

        public static string Where => $"WHERE {Clause}";

    }

}
