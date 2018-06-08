using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchableIntAttribute : SearchableAttribute
    {
        public SearchableIntAttribute()
        {
            ExpressionProvider = new IntSearchExpressionProvider();
        }
    }
}
