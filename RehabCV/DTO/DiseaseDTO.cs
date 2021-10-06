using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.DTO
{
    public class DiseaseDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Вкажіть групу захворювання.")]
        [Display(Name = "Група захворювання")]
        public string Name { get; set; }
    }
}
