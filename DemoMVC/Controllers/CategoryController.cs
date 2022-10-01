using DemoMVC.Data;
using DemoMVC.Models;
using DemoMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers
{
    /// <summary>
    /// Information of CategoryController
    /// CreatedBy: ThiepTT(19/08/2022)
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly CategoryRepository _categoryRepositoryclass;

        public CategoryController(ApplicationDbContext applicationDbContext, IBaseRepository<Category> categoryRepository, CategoryRepository categoryRepositoryclass)
        {
            _applicationDbContext = applicationDbContext;
            _categoryRepository = categoryRepository;
            _categoryRepositoryclass = categoryRepositoryclass;
        }

        public IActionResult Index(string? valueSearch)
        {
            IEnumerable<Category> listCategory;
            if (string.IsNullOrEmpty(valueSearch))
            {
                //listCategory = _applicationDbContext.Categories;
                listCategory = _categoryRepository.GetAll();
            }
            else
            {
                //listCategory = _categoryRepository.Categories.Where(category => category.Name.ToLower().Trim().Contains(valueSearch.ToLower().Trim())
                //                                 || category.DisplayOrder.ToString().ToLower().Trim().Contains(valueSearch.ToLower().Trim()));
                listCategory = _categoryRepositoryclass.GetBySearchValue(valueSearch);
            }
            return View(listCategory);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                //_applicationDbContext.Categories.Add(category);
                //_applicationDbContext.SaveChanges();
                _categoryRepository.Create(category);
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = _applicationDbContext.Categories.Find(id);
            var category = _categoryRepository.GetById((int)id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                //_applicationDbContext.Categories.Update(category);
                //_applicationDbContext.SaveChanges();
                _categoryRepository.Update(category);
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = _applicationDbContext.Categories.Find(id);
            var category = _categoryRepository.GetById((int)id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            //var category = _applicationDbContext.Categories.Find(id);
            var category = _categoryRepository.GetById((int)id);

            if (category == null)
            {
                return NotFound();
            }

            //_applicationDbContext.Categories.Remove(category);
            //_applicationDbContext.SaveChanges();
            _categoryRepository.Delete((int)id);
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}