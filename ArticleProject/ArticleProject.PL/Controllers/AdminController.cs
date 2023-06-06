using ArticleProject.BL.IRepository;
using ArticleProject.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics;
using System.Security.Claims;

namespace ArticleProject.PL.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        #region Prop

        private readonly IBaseRepository<Post> repository;
        private readonly IAuthorizationService authorizationService;
        
        #endregion

        #region Ctor

        public AdminController(IBaseRepository<Post> repository, IAuthorizationService authorizationService )
        {
            this.repository = repository;
            this.authorizationService = authorizationService;
        }
        #endregion

        #region Actions

        public IActionResult Index()
        {
            var dataByMonth = DateTime.Now.AddMonths(-1);
            var dataByYear = DateTime.Now.AddYears(-1);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TempData["AllPosts"] = repository.GetAllAsync(x => x.UserId == userId).Result.Count();
            TempData["PostsLastYear"] = repository.GetAllAsync(x => x.UserId == userId && x.Date >= dataByYear).Result.Count();
            TempData["PostsLastMonth"] = repository.GetAllAsync(x => x.UserId == userId && x.Date >= dataByMonth).Result.Count();

            return View();
        }

        #endregion      

    }
}