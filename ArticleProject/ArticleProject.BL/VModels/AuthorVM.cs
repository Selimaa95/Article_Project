using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.VModels
{
    public class AuthorVM
    {       
        public int Id { get; set; }          
        public string UserId { get; set; }
        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Image { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? FaceBook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }

    }
}
