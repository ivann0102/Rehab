﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Models
{
    public class Group
    {
        public string Id { get; set; }
        public string NameOfDisease { get; set; }
        public int NumberOfChildren { get; set; }
        public int PercentOfChildren { get; set; }
        public int Priority { get; set; }
        public List<Child> Children { get; set; }
    }
}
