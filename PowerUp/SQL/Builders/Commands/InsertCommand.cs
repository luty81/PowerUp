using PowerUp.SQL.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PowerUp.SQL
{
    public class InsertCommand: ICommand
    {
        public string Build(string tableName, IEnumerable<(PropertyInfo column, string param)> fields, bool dontSetKeys)
        {
            var keyFields = KeyColumnsExtractor.Names(fields.Select(f => f.column));
            var fieldsToSet = dontSetKeys ? fields.Where(f => keyFields.NotContains(f.column.Name)) : fields;

            return new StringBuilder()
                .AppendLine($"INSERT INTO {tableName}")
                .AppendLine($"({fieldsToSet.Select(c => c.column.Name).JoinByComma()})")
                .AppendLine("VALUES")
                .AppendLine($"({fieldsToSet.Select(c => c.param ?? $"@{c.column.Name}").JoinByComma()})")
            .ToString();
        }
    }

}
