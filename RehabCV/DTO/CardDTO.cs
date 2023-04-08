using System;
using System.ComponentModel.DataAnnotations;

namespace RehabCV.DTO
{
    public class CardDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public ChildDTO Child { get; set; }
        public string TherapistId { get; set; }
        public TherapistDTO Therapist { get; set; }

        [Required(ErrorMessage = "Вкажіть діагноз дитини")]
        [StringLength(100, ErrorMessage = "В полі дагноз має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Діагноз дитини")]
        public string Diagnosis { get; set; }

        public List<DateTime> Lessons { get; set; }

        [Required(ErrorMessage = "Вкажіть симптоми дитини")]
        [StringLength(100, ErrorMessage = "В полі дагноз має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Системи дитини")]
        public List<string> Symptoms { get; set; }
    }
}