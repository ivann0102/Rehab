using System;

namespace RehabCV.Models
{
    public class Card
    {
    public string Id { get; set; }
    public string UserId { get; set; }
    public Child Child { get; set; }
    public string TherapistId { get; set; }
    public Therapist Therapist { get; set; }
    public string RehabilitationResults { get; set; }
    public string Diagnosis{ get; set; }
    public List<DateTime > Lessons {get; set;}
    public List<string> Symptoms { get; set; }

    public List<Card> Cards { get;set; }

    }

}