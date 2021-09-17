using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Models
{
    public class Reserve
    {
        public string Id { get; set; }
        public string ChildId { get; set; }
        public Child Child { get; set; }
        public int NumberInQueue { get; set; }
    }
}
