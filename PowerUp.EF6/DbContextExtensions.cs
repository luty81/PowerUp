using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using PowerUp;
using System.Data.Entity.Infrastructure;

namespace PowerUp.EF6
{
    public static class DbContextExtensions
    {
        public static DbRawSqlQuery<TEntity> View<TEntity>(this DbContext self)
        {
            var dbViewAttr = typeof(TEntity).GetCustomAttributes(typeof(DbViewAttribute), false).FirstOrDefault();
            if (dbViewAttr != null)
            {
                var selectViewSql = String.Format("select * from {0}", ((DbViewAttribute)dbViewAttr).ViewName);
                return self.Database.SqlQuery<TEntity>(selectViewSql);
            }

            throw new Exception(string.Format("Missing custom attribute. Check if {0} class is decorated with DbView attribute.", typeof(TEntity).Name));
        }
    }
}
