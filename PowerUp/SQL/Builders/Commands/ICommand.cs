using System.Collections.Generic;
using System.Reflection;


namespace PowerUp.SQL.Commands
{
    public interface ICommand
    {
        string Build(string tableName, IEnumerable<(PropertyInfo column, string param)> fields, bool dontSetKey);
    }

}
