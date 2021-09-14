using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Models
{
    public class Queue
    {
        public int ChildId { get; set; }
        public Child Child { get; set; }
        public string TypeOfRehab { get; set; }
        public int NumberInQueue { get; set; }
    }
}
