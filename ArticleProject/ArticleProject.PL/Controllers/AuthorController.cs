using ArticleProject.BL.IRepository;
using ArticleProject.BL.VModels;
using ArticleProject.BL.Helper;
using ArticleProject.DAL.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace ArticleProject.PL.Controllers
{

    public class AuthorController : Controller
    {

        #region Prop
        private readonly IBaseRepository<Author> repository;
        private readonly IMapper mapper;
        private readonly IAuthorizationService service;

        #endregion

        #region Ctor
        public AuthorController(IBaseRepository<Author> repository, IMapper mapper, IAuthorizationService service)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.service = service;
        }
        #endregion

        #region Actions

        #region GetData
        
        [Authorize("Admin")]
        public async Task<IActionResult> Index()
        {

            var data = await repository.GetAllAsync();
            var result = mapper.Map<IEnumerable<AuthorVM>>(data);
            
            return View(result);
        }
        
        [Authorize("Admin")]
        public async Task<IActionResult> Search(string searchItem)
        {
            if (searchItem != null)
            {
                var data = await repository.GetAllAsync(x => x.UserName.Contains(searchItem) || x.FullName.Contains(searchItem));
                var result = mapper.Map<IEnumerable<AuthorVM>>(data);

                return View("Index", result);
            }

            var data1 = await repository.GetAllAsync();
            var result1 = mapper.Map<IEnumerable<AuthorVM>>(data1);
            return View("Index", result1);
        }
        #endregion

        #region Update
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var data = await repository.GetByIdAsync(id);
            var result = mapper.Map<AuthorVM>(data);

            return View(result);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(AuthorVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model?.ImageFile?.FileName))
                    {
                        FileUploader.RemoveFile("Images", model.Image);
                        model.Image = FileUploader.UploadFile(model.ImageFile, "Images");
                    }
                    var data = mapper.Map<Author>(model);
                    await repository.UpdateAsync(data);

                    var admin = service.AuthorizeAsync(User, "Admin");                
                    if (admin.Result.Succeeded)
                    {
                        return RedirectToAction("Index", "Author");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
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

        [Authorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await repository.GetByIdAsync(id);
            var result = mapper.Map<AuthorVM>(data);

            return View(result); 
        }


        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> Delete(AuthorVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FileUploader.RemoveFile("Images", model.Image);
                    var data = mapper.Map<Author>(model);
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
