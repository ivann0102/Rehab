﻿using System;
using System.Collections.Generic;

namespace RehabCV.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        public string ChildId { get; set; }
        public Child Child { get; set; }
        public string TherapistId { get; set; }
        public Therapist Therapist { get; set; }
    }
}
