using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        [Obsolete("Use GetProperties(params BindingFlags[] filter) type extension method")]
        public static IEnumerable<PropertyInfo> Getters(this Type self) => 
            self.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public static IEnumerable<PropertyInfo> GetProperties(this Type self, params BindingFlags[] filter)
        {
            var result = self.GetProperties(BindingFlags.Public | BindingFlags.Instance).AsEnumerable();
            if (self.IsAnonymous())
                return result;

            if (filter.Contains(BindingFlags.GetProperty))
                result = result.Where(x => x.GetGetMethod() != null);
            if (filter.Contains(BindingFlags.SetProperty))
                result = result.Where(x => x.GetSetMethod() != null);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="aggressiveMode">
        /// If true the type must be sealed, non-public, with Object as base type, and the first custom attribute type must be the CompilerGenerated
        /// </param>
        /// <returns></returns>
        public static bool IsAnonymous(this Type self, bool aggressiveMode = false)
        {
            var result = self.Namespace == null;
            if (aggressiveMode)
            {
                result &= self.IsSealed && !self.IsPublic
                      && self.BaseType == typeof(Object)
                      && self.CustomAttributes.Any()
                      && self.CustomAttributes.ElementAt(0).GetType() == typeof(CompilerGeneratedAttribute);
            }

            return result;
        }


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
