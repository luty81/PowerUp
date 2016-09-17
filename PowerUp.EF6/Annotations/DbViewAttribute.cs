using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DbViewAttribute: Attribute
    {
        public String ViewName { get; set; }

        public DbViewAttribute(string viewName)
        {
            ViewName = viewName;
        }

    }
}
