using ArticleProject.BL.Helper;
using ArticleProject.BL.IRepository;
using ArticleProject.BL.VModels;
using ArticleProject.DAL.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ArticleProject.PL.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        #region Prop
        private readonly IMapper mapper;
        private readonly IBaseRepository<Post> repository;
        private readonly IBaseRepository<Category> category;
        private readonly IAuthorizationService authorizationService;
        private readonly UserManager<IdentityUser> userManager;
        private Task<AuthorizationResult> admin;
        private string UserId;

        #endregion

        #region Ctor
        public PostController(IBaseRepository<Post> repository,
            IBaseRepository<Category> category,
            IMapper mapper, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.category = category;
            this.authorizationService = authorizationService;
            this.userManager = userManager;            
        }
        #endregion
      
        #region Actions
        
        private void Authorization()
        {
            //Authorization
            admin = authorizationService.AuthorizeAsync(User, "Admin");
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        #region GetData

        public async Task<IActionResult> Index()
        {
            Authorization();

            if (admin.Result.Succeeded)
            {
                //Admin
                var data = await repository.FindAllAsync(new[] { "Category" });
                var result = mapper.Map<IEnumerable<PostVM>>(data);
                return View(result);

            }
            else
            {
                //User
                var data1 = await repository.FindAllAsync(new[] { "Category" }, x => x.UserId == UserId);
                var result1 = mapper.Map<IEnumerable<PostVM>>(data1);
                return View(result1);

            }

            //return View(result);
            /*var data = await repository.FindAllAsync(new[] { "Category" });
            var result = mapper.Map<IEnumerable<PostVM>>(data);
            return View(result);*/
        }

        public async Task<IActionResult> Details(int id)
        {
            //SetUser();

            var data = await repository.GetByIdAsync(id);
            var result = mapper.Map<PostVM>(data);

            return View(result);
        }
        #endregion

        #region Create

        [HttpGet]
        public async  Task<IActionResult> Create()
        {
            //SetUser();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userManager.GetUserAsync(User);
                    model.UserId = user.Result.Id;
                    model.Date = DateTime.Now;
                    if (!string.IsNullOrEmpty(model?.FileImage?.FileName))
                    {
                        model.Image = FileUploader.UploadFile(model.FileImage, "Images");
                    }

                    var data = mapper.Map<Post>(model);
                    var result = await repository.CreateAsync(data);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return View(model);
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var data = await repository.GetByIdAsync(id);
            var result = mapper.Map<PostVM>(data);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PostVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(!string.IsNullOrEmpty(model?.FileImage?.FileName))
                    {
                        FileUploader.RemoveFile("Images", model.Image);
                        model.Image = FileUploader.UploadFile(model.FileImage, "Images");
                    }
                    var data = mapper.Map<Post>(model);
                    data = await repository.UpdateAsync(data);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return View(model);
        }


        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await repository.GetByIdAsync(id);
            var result = mapper.Map<PostVM>(data);
            
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PostVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model?.Image))
                    {
                        FileUploader.RemoveFile("Images", model.Image);
                    }

                    var data = mapper.Map<Post>(model);
                    await repository.DeleteAsync(data);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }

            return View(model);
        }

        #endregion

        #endregion
    }
}
