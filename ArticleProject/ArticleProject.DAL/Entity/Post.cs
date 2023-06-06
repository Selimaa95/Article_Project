using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.DAL.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PostCategory { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public DateTime? Date { get; set; }
        
        public int? AuthorId { get; set; }    
        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }

        public int CategoryId { get; set; }      
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        

    }
}
