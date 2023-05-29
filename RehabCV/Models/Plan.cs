using System;

namespace RehabCV.Models
{
    public class Plan
    {
        public string Id { get; set; }
        public string TherapistId { get; set; }
        public Therapist Therapist { get; set; }
        public string Description { get; set; }
        public int NumberOfAppointments { get; set; }
        public string RehabId { get; set; }
        public Rehabilitation Rehab { get; set; }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
