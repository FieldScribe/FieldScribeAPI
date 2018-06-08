using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class SearchTermsForm
    {
        [Display(Name = "searchTerms", Description = "array of search terms")]
        public string[] SearchTerms { get; set; }
    }
}
