using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RehabCV.ViewModels
{
    public class ChildViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Ім'я дитини")]
        public string FirstNameOfChild { get; set; }

        [Display(Name = "По батькові дитини")]
        public string MiddleNameOfChild { get; set; }

        [Display(Name = "Прізвище дитини")]
        public string LastNameOfChild { get; set; }

        [Display(Name = "Дата народження дитини")]
        public DateTime BirthdayOfChild { get; set; }

        [Display(Name = "Виберіть діагноз")]
        public string Diagnosis { get; set; }

        [Display(Name = "Виберіть пріоритет")]
        public string Priority { get; set; }

        [Display(Name = "Домашня адреса")]
        public string HomeAddress { get; set; }
    }
}
