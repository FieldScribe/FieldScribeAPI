﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class EHorizontalMarkEntity
    {
        [Key]
        public int EHorizontalId { get; set; }

        public int entryId { get; set; }

        public int AttemptNum { get; set; }
        
        public int Feet { get; set; }

        public float Inches { get; set; }

        public float? Wind { get; set; }
    }
}
