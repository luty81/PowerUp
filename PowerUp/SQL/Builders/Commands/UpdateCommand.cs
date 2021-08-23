using PowerUp.SQL.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PowerUp.SQL
{
    public class UpdateCommand: ICommand
    {
        public string Build(string tableName, IEnumerable<(PropertyInfo column, string param)> fields, bool dontSetKeys)
        {
            var keyFields = KeyColumnsExtractor.Names(fields.Select(f => f.column));

            var columnsToSet = fields
                .Where(f => (dontSetKeys ? keyFields : Enumerable.Empty<string>()).NotContains(f.column.Name))
                .Select(f => $"{f.column.Name} = {f.param}");

            return new StringBuilder()
                .AppendLine($"UPDATE {tableName}")
                .AppendLine($"SET {columnsToSet.JoinByComma()}")
                .AppendLine($"{keyFields.ToWhereByKey()}")
            .ToString();
        }
    }

}
