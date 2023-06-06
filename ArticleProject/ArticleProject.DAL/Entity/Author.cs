using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.DAL.Entity
{
    public class Author
    {
        [Required]
        public int Id { get; set; }      
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? FaceBook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }

    }
}
