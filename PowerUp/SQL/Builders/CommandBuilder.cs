using PowerUp.SQL.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PowerUp.SQL
{

    public class CommandBuilder
    {
        public IEnumerable<PropertyInfo> AssignedProperties { get; set; }

        public string For<TCommand>(string tableName = null, params (string field, string param)[] customAssigments)
            where TCommand : ICommand, new()
        {
            tableName ??= TableNameFromType();

            var fields = (
                from PropertyInfo column in Columns
                let customAssign = customAssigments?.FirstOrDefault(x => x.field == column.Name)
                let @param = customAssign?.param ?? $"@{column.Name}"
                select (column, @param)).ToList();

            var fieldsToBeIncluded = customAssigments.Where(ca => ! fields.Any(x => x.column.Name == ca.field));
            if (fieldsToBeIncluded.Any())
            {
                fieldsToBeIncluded.ForEach(x => fields.Insert(0, (_object.GetType().GetProperty(x.field), x.param)));
            }

            if (fields.Any())
                return new TCommand().Build(tableName, fields, DontInsertKeyFields);

            return null;
        }

        public CommandBuilder(object @object, bool dontInsertKeyFields = true, bool ignoreUnassigned = true)
        {
            _object = @object;
            DontInsertKeyFields = dontInsertKeyFields;
            Columns = DiscoverColumns(ignoreUnassigned);
            KeyColumns = KeyColumnsExtractor.Names(Columns);
        }


        protected readonly IEnumerable<PropertyInfo> Columns;
        
        protected readonly IEnumerable<string> KeyColumns;

        protected readonly bool DontInsertKeyFields;

        protected string TableNameFromType() => 
            _object.GetType().GetCustomAttribute<TableAttribute>()?.Name ?? _object.GetType().Name;


        private bool HasValue(PropertyInfo propertyInfo)
        {
            var @value = propertyInfo.GetValue(_object);

            if (propertyInfo.PropertyType.IsEnum)
                return propertyInfo.PropertyType.IsEnumDefined(@value);

            return @value != null;
        }
        private IEnumerable<PropertyInfo> DiscoverColumns(bool ignoreUnassigned)
        {
            var onlyGettersAndSetters = new[] { BindingFlags.GetProperty, BindingFlags.SetProperty };
            var properties = _object.GetType().GetProperties(onlyGettersAndSetters);
            
            AssignedProperties = properties.Where(HasValue);
            return ignoreUnassigned ? AssignedProperties : properties;
        }


        private readonly object _object;
    }
}
