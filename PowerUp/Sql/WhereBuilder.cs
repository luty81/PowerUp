using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PowerUp.SQL
{
    public class WhereBuilder<T> where T : class
    {
        public WhereBuilder<T> AddClause(Expression<Func<T, object>> columnSelector, ExpressionType? @op = null)
        {
            if (op.HasValue)
            {
                _builder.AppendLine()
                    .Append($"\t {@op}".ToUpper());
            }

            _builder.Append(NewClause(columnSelector));
            return this;
        }

        public WhereBuilder<T> And(Expression<Func<T, object>> columnSelector) => 
            AddClause(columnSelector, ExpressionType.And);
            
        public WhereBuilder<T> Or(Expression<Func<T, object>> columnSelector) =>
            AddClause(columnSelector, ExpressionType.Or);
            
        public string Done()
        {
            _builder.Insert(0, "WHERE");
            return _sql.Done();
        }

        public override string ToString() =>
            _builder.ToString();
        
        public WhereBuilder(SqlBuilder<T> sql)
        {
            _sql = sql;
        }

        private string NewClause(Expression<Func<T, object>> propertySelector)
        {
            var prop = "";
            if (propertySelector.Body is UnaryExpression unary)
                prop = ((MemberExpression)unary.Operand).Member.Name;
            else if (propertySelector.Body is MemberExpression expression)
                prop = expression.ToString().Split('.').LastOrDefault();
            
            
            
            var alias = _sql.TableAlias;
            var column = alias.IsEmpty() ? prop : $"{alias}.{prop}";
            return $" {column} = @{prop} ";
        }

        private readonly SqlBuilder<T> _sql;
        private readonly StringBuilder _builder = new StringBuilder();
    }
}