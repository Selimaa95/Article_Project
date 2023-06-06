using ArticleProject.BL.IRepository;
using ArticleProject.BL.VModels;
using ArticleProject.DAL.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic;

namespace ArticleProject.PL.Controllers
{
    [Authorize("Admin")]
    public class CategoryController : Controller
    {

        #region prop
        private readonly IBaseRepository<Category> repository;
        private readonly IMapper mapper;

        #endregion
            
        #region Ctor

        public CategoryController(IBaseRepository<Category> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        #endregion

        #region Actions

        #region GetData

        public async Task<IActionResult> Index()
        {
            var data = await repository.GetAllAsync();
            var result = mapper.Map<IEnumerable<CategoryVM>>(data);
            return View(result);
        }
        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Category>(model);
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
            var result = mapper.Map<CategoryVM>(data);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Category>(model);
                    var result = await repository.UpdateAsync(data);
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["errro"] = ex.Message;
            }

            return View(model);
        }
        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int id)
        {
            var data = await repository.GetByIdAsync(id);
            var result = mapper.Map<CategoryVM>(data);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryVM model)
        {
            try
            {
                var data = mapper.Map<Category>(model);
                var result = await repository.DeleteAsync(data);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }

            return View(model);
        }

        #endregion

        #region Search

        public async Task<IActionResult> Search(string searchItem)
        {
            if(searchItem != null)
            {
                var data = await repository.GetAllAsync(x => x.Name.Contains(searchItem));
                var result = mapper.Map<IEnumerable<CategoryVM>>(data);
                
                return View("Index",result);
            }

            var data1 = await repository.GetAllAsync();
            var result1 = mapper.Map<IEnumerable<CategoryVM>>(data1);
            return View("Index", result1);
        }

        #endregion

        #endregion
    }
}
