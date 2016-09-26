using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6
{
    public static class DbMigrationExtensions
    {
        public static void InsertSql<TEntity>(this DbMigration self, DbContext context, object entity)
        {
            var columns = entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var sqlInsertBuilder = new StringBuilder();
            sqlInsertBuilder.AppendFormat("INSERT INTO {0} ", typeof(TEntity).Name);
            sqlInsertBuilder.AppendFormat(" ({0}) VALUES ", string.Join(", ", columns.Select(c => c.Name)));
            sqlInsertBuilder.AppendFormat(" ({0}) ", string.Join(", ", columns.Select(c => string.Format("@{0}", c.Name))));

            var sqlInsert = sqlInsertBuilder.ToString();
            var sqlParameters = columns.Select(c => new SqlParameter("@" + c.Name, entity.GetPropertyValue(c.Name)));
            

            //context.Database.ExecuteSqlCommand(sqlInsert, sqlParameters.ToArray());
        }
    }
}
