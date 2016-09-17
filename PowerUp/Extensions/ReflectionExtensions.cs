using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;

namespace PowerUp
{
    public static class ReflectionExtensions
    {
        public static bool HasAttribute<TAttribute>(this MemberInfo self) where TAttribute : Attribute
        {
            return self.GetCustomAttributes<TAttribute>().Any();
        }

        public static TValue GetPropertyValue<TValue>(this object self, string propertyName)
        {
            return (TValue)self.GetType().GetProperty(propertyName).GetValue(self, null);
        }

        public static object GetPropertyValue(this object self, string propertyName)
        {
            return self.GetType().GetProperty(propertyName).GetValue(self, null);
        }

        public static string GetExpressionTargetMemberName(this Expression self)
        {
            if (self is LambdaExpression)
            {
                var expressionBody = (self as LambdaExpression).Body;
                if (expressionBody is MemberExpression)
                {
                    return (expressionBody as MemberExpression).Member.Name;
                }

                throw new InvalidOperationException(String.Format("Only MemberExpression is supported, but a {0} body type was provided.", expressionBody.GetType().Name));
                
            }

            throw new InvalidOperationException(String.Format("Only LambdaExpression is ´supported, but a {0} type was provided.", self.GetType().Name));
        }

        //public static bool HasAttribute<TAttribute>(this PropertyInfo self) where TAttribute : Attribute
        //{
        //    return self.GetCustomAttributes<TAttribute>().Any();
        //}

    }
}
