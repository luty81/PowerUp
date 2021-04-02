using PowerUp.SQL.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace PowerUp.SQL
{
    public class InsertCommand: ICommand
    {
        public string Build(string tableName, IEnumerable<(PropertyInfo column, string param)> fields)
        {
            return new StringBuilder()
                .AppendLine($"INSERT INTO {tableName}")
                .AppendLine($"({fields.Select(c => c.column.Name).JoinByComma()})")
                .AppendLine("VALUES")
                .AppendLine($"({fields.Select(c => c.param ?? $"@{c.column.Name}").JoinByComma()})")
            .ToString();
        }
    }

}
