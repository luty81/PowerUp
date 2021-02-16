using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using static System.Reflection.BindingFlags;

namespace PowerUp
{
    public static class ReflectionExtensions
    {
        public static bool HasAttribute<TAttribute>(this MemberInfo self) 
            where TAttribute : Attribute =>
                self.GetCustomAttributes<TAttribute>().Any();
        public static bool HasNotAttribute<TAttribute>(this MemberInfo self)
            where TAttribute : Attribute =>
                false == self.HasAttribute<TAttribute>();

        public static IEnumerable<PropertyInfo> GetPropertiesWith<T>(this Type self) 
            where T : Attribute =>
                self.GetProperties().Where(p => p.HasAttribute<T>());

        public static IEnumerable<PropertyInfo> Getters(this Type self) => self.GetProperties(OnlyGettersFilter);

        public static BindingFlags OnlyGettersFilter => Public | Instance | GetProperty;



        public static object GetPropertyValue(this object self, string propertyName) =>
            self.GetType().GetProperty(propertyName).GetValue(self, null);

        public static TValue GetPropertyValue<TValue>(this object self, string propertyName) =>
            (TValue)GetPropertyValue(self, propertyName);
        
        public static string GetExpressionTargetMemberName(this Expression self)
        {
            if (self is LambdaExpression lambda)
            {
            
                if (lambda.Body is MemberExpression member) 
                    return member.Member.Name;
                
                throw Expected<MemberExpression>(lambda.Body);
            }
            
            throw Expected<LambdaExpression>(self);

            static Exception Expected<TExpected>(Expression exp) where TExpected : Expression =>
                new InvalidOperationException(
                    $"An {exp.GetType().Name} expression type was provided when a {typeof(TExpected).Name} was expected.");
        }
    }
}
