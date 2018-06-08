using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class IntSearchExpressionProvider : DefaultSearchExpressionProvider
    {
        public override Expression GetComparison(MemberExpression left, string op, ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case "gt": return Expression.GreaterThan(left, right);
                case "gte": return Expression.GreaterThanOrEqual(left, right);
                case "lt": return Expression.LessThan(left, right);
                case "lte": return Expression.LessThanOrEqual(left, right);

                // If nothing matches, fall back to base implementation
                default: return base.GetComparison(left, op, right);
            }
        }

        public override ConstantExpression GetValue(string input)
            => Expression.Constant(Convert.ToInt32(input));
    }
}
