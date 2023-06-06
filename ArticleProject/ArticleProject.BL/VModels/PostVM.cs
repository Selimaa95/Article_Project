using ArticleProject.DAL.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.VModels
{
    public class PostVM
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PostCategory { get; set; }

        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        public string Title { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Description { get; set; }
        public string? Image { get; set; }

        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        public IFormFile? FileImage { get; set; }
        public DateTime? Date { get; set; }

        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }


    }
}
