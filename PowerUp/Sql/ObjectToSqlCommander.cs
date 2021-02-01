using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PowerUp.Sql
{
    public class ObjectToSqlCommander
    {
        public IEnumerable<string> AssignedProperties { get; private set; }
        
        
        public string UpdateCommand() => GetUpdateCommandFor(_object.GetType().Name);
        public string GetUpdateCommandFor(string tableName, bool ignoreUnassignedProperties = true)
        {
            var keyField = _properties.FirstOrDefault(prop => prop.Name == "Id");
            var fieldsToUpdate =
                _properties
                    .Except(new[] { keyField });

            if (ignoreUnassignedProperties)
                fieldsToUpdate = fieldsToUpdate.Where(HasValue);

            AssignedProperties = fieldsToUpdate.Select(f => f.Name);

            if (fieldsToUpdate.Any())
            {
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine($"UPDATE {tableName}");
                sqlBuilder.AppendLine($"SET {string.Join(", ", fieldsToUpdate.Select(x => $"{x.Name} = @{x.Name}"))}");
                sqlBuilder.AppendLine($"WHERE Id = @Id");
                return sqlBuilder.ToString();
            }

            return null;
        }
        public string GetInsertCommandFor(string tableName)
        {
            var insertBuilder = new StringBuilder($"INSERT INTO {tableName} {Environment.NewLine}")
                .AppendLine($"({ByComma(AssignedProperties)})")
                .AppendLine("VALUES")
                .AppendLine($"({ByComma(AssignedProperties.Select(Params))})");

            return insertBuilder.ToString();

            static string Params(string columnName) => $"@{columnName}";
            static string ByComma(IEnumerable<string> columns) => string.Join(", ", columns);
        }
        public ObjectToSqlCommander(object @object)
        {
            _object = @object;
            _properties = _object.GetType().GetProperties(Getters);
            AssignedProperties = _properties.Where(HasValue).Select(p => p.Name);
        }

        private BindingFlags Getters => BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
        private object _object;
        private readonly PropertyInfo[] _properties;
        private bool HasValue(PropertyInfo propertyInfo)
        {
            var @value = propertyInfo.GetValue(_object);
            if (@value == null)
                return false;

            
            if (propertyInfo.PropertyType.IsValueType)
            {
                // If the value is equal to the type default value, then we assume it is unassigned
                return ! Activator.CreateInstance(@value.GetType()).Equals(@value);
            }

            return true;
        }
    }

}
