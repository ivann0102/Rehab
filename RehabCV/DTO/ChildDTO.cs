using System;
using System.ComponentModel.DataAnnotations;

namespace RehabCV.DTO
{
    public class ChildDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть ім'я.")]
        [StringLength(100, ErrorMessage = "В полі ім'я має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Ім'я дитини")]
        public string FirstNameOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть по батькові.")]
        [StringLength(100, ErrorMessage = "В полі по батькові має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "По батькові дитини")]
        public string MiddleNameOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть прізвище.")]
        [StringLength(100, ErrorMessage = "В полі прізвище має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Прізвище дитини")]
        public string LastNameOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть дату народження.")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата народження дитини")]
        public DateTime BirthdayOfChild { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Виберіть діагноз.")]
        [Display(Name = "Виберіть діагноз")]
        public string Diagnosis { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Виберіть пріоритет.")]
        [Display(Name = "Виберіть пріоритет")]
        public string Priority { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть домашню адресу.")]
        [StringLength(100, ErrorMessage = "В полі домашня адреса має бути мінімум 1 символ і максимум 100", MinimumLength = 1)]
        [Display(Name = "Домашня адреса")]
        public string HomeAddress { get; set; }
    }
}
