using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.VModels
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="الاسم مطلوب !")]
        [MaxLength(50,ErrorMessage ="الحد الاقصى 50 حرف ")]
        [MinLength(2,ErrorMessage ="الحد الادنى حرفان ")]
        public string Name { get; set; }
    }
}
