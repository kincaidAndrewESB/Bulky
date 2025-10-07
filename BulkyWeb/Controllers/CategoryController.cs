using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
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
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //Additional Input Validation options - these are server side and require a reload
            if (obj.Name != null && obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            //if (obj.Name != null &&obj.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "test is an invalid value.");
            //}
            /*
             Use JavaScript to carry out client side validation is more efficient*/
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();//this executes the database update when you are ready
                return RedirectToAction("Index");
            }
            return View();
            
        }
    }
}
