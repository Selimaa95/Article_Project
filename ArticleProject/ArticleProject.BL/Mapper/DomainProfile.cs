using ArticleProject.BL.VModels;
using ArticleProject.DAL.Entity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryVM, Category>();

            CreateMap<Author, AuthorVM>();
            CreateMap<AuthorVM, Author>();

            CreateMap<Post, PostVM>();
            CreateMap<PostVM, Post>();

        }
    }
}
