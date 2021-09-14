using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Models
{
    public class Reserve
    {
        public int ChildId { get; set; }
        public Child Child { get; set; }
        public int NumberInQueue { get; set; }
    }
}
