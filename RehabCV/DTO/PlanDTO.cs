namespace RehabCV.DTO
{
    public class PlanDTO
    {

        public string Id { get; set; }
        public string ChildId { get; set; }
        public string TherapistId { get; set; }
        public string RehabId { get; set; }

        public string Description { get; set; }
        public int NumberOfAppointments { get; set; }

    }
}