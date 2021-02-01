using System;
using System.Linq.Expressions;
using System.Text;

namespace PowerUp.Sql
{
    public class WhereBuilder<T> where T : class
    {
        public WhereBuilder<T> And(Expression<Func<T, object>> columnSelector) => 
            AppendLogicOperator(ExpressionType.And)
                .AddClause(columnSelector);

        public WhereBuilder<T> Or(Expression<Func<T, object>> columnSelector) => 
            AppendLogicOperator(ExpressionType.Or)
                .AddClause(columnSelector);
            
        public WhereBuilder<T> AddClause(Expression<Func<T, object>> columnSelector)
        {
            _builder.Append(NewClause(columnSelector));
            return this;
        }

        public string NewClause(Expression<Func<T, object>> propertySelector)
        {
            var prop = "";
            if (propertySelector.Body is UnaryExpression unary)
                prop = ((MemberExpression)unary.Operand).Member.Name;
            
            var column = _alias.IsEmpty() ? prop : $"{_alias}.{prop}";
            var param = $"@{prop}";
            return $" {column} = {param} ";
        }

        public string Done()
        {
            _builder.Insert(0, "WHERE");
            return _builder.ToString();
        }

        public bool IsNotEmpty => _builder.Length > 0;

        public WhereBuilder(string alias)
        {
            _alias = alias;
        }

        private WhereBuilder<T> AppendLogicOperator(ExpressionType @op)
        {
            _builder.AppendLine($"\t {@op}".ToUpper());
            return this;
        }
        
        private readonly string _alias;
        private readonly StringBuilder _builder = new StringBuilder();
    }
}