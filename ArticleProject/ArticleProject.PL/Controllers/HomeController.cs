using ArticleProject.BL.IRepository;
using ArticleProject.BL.VModels;
using ArticleProject.DAL.Entity;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArticleProject.PL.Controllers
{
    public class HomeController : Controller
    {

        #region Prop
        private readonly IBaseRepository<Post> post;
        private readonly IMapper mapper;
        private readonly IBaseRepository<Author> author;
        #endregion

        #region Ctor

        public HomeController(IBaseRepository<Post> post, IMapper mapper, IBaseRepository<Author> author)
        {
            this.post = post;
            this.mapper = mapper;
            this.author = author;
        }

        #endregion

        #region Actions

        #endregion
        public async Task<IActionResult> Index(int categoryId)
        {           
            if(categoryId is 0)
            {
                var data = await post.GetAllAsync();
                var result = mapper.Map<IEnumerable<PostVM>>(data);
                return View(result);
            }
            else
            {
                var data1 = await post.GetAllAsync(x => x.CategoryId == categoryId);
                var result1 = mapper.Map<IEnumerable<PostVM>>(data1);
                return View(result1);
            }
        }

        
        public async Task<IActionResult> GetByCategoryId(int id)
        {
            var data = await post.GetByIdAsync(x => x.CategoryId == id);
            var result = mapper.Map<PostVM>(data);
            return View(result);
        }

        public async Task<IActionResult> Article(int id)
        {
            var data = await post.GetByIdAsync(id);
            var result = mapper.Map<PostVM>(data);
            return View(result);
        }

        public async Task<IActionResult> AllUsers()
        {
            var data = await author.GetAllAsync();
            var result = mapper.Map<IEnumerable<AuthorVM>>(data);
            return View(result);
        }

       public IActionResult Test()
        {
            return View();  
        }

    }
}