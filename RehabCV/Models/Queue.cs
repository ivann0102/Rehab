using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Models
{
    public class Queue
    {
        public string Id { get; set; }
        public string RehabilitationId { get; set; }
        public Rehabilitation Rehabilitation { get; set; }
        public string TypeOfRehab { get; set; }
        public int NumberInQueue { get; set; }
    }
}
