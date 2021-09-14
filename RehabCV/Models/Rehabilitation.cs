using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Models
{
    public class Rehabilitation
    {
        public int ChildId { get; set; }
        public Child Child { get; set; }
        public string Form { get; set; }
        public string Duration { get; set; }
        public DateTime DateOfRehab { get; set; }
    }
}
