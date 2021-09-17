using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.DTO
{
    public class RehabDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Виберіть для котрої дитини реабілітація.")]
        [Display(Name = "Реабілітація дитини")]
        public string ChildId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть форму реабілітації.")]
        [Display(Name = "Форма реабілітації")]
        public string Form { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть тривалість реабілітації.")]
        [Display(Name = "Тривалість реабілітації")]
        public string Duration { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть дату початку реабілітації.")]
        [Display(Name = "Дата початку реабілітації")]
        public DateTime DateOfRehab { get; set; }
    }
}
