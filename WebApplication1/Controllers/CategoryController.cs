using DataAccesss.Data;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApplication1.Controllers
{
	public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = _db.Categories.ToList();
            return View(categoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            bool containsNumber = category.Name?.Any(char.IsDigit) ?? false;
            if (containsNumber)
            {
                ModelState.AddModelError("Name", "Category name can not contain number(s)");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
				TempData["success"] = $"The category {category.Name} has been added successfuly";
				return RedirectToAction("Index");
			}
            return View();
		}
		public IActionResult Edit(int? id)
		{
            if(id is null)
                return NotFound();
            Category? categoryToEdit = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryToEdit is null)
                return NotFound();
			return View(categoryToEdit);
		}
		[HttpPost]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Update(category);
				_db.SaveChanges();
				TempData["success"] = $"The category {category.Name} has been updated successfuly";
				return RedirectToAction("Index");
			}
			return View();
		}
		public IActionResult Delete(int? id)
		{
			if (id is null)
				return NotFound();
			Category? categoryToEdit = _db.Categories.FirstOrDefault(c => c.Id == id);
			if (categoryToEdit is null)
				return NotFound();
			return View(categoryToEdit);
		}
		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Category? categoryToDelete = _db.Categories.FirstOrDefault(c =>c.Id == id);
			if(categoryToDelete is null)
				return NotFound();
			_db.Categories.Remove(categoryToDelete);
			_db.SaveChanges();
			TempData["success"] = $"The category {categoryToDelete.Name} has been deleted successfuly";
			return RedirectToAction("Index");
		}
	}
}
