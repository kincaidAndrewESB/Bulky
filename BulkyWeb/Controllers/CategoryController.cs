using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;


namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepo;
        //private readonly ApplicationDbContext _db;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;

        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
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
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
            }
            //if (obj.Name != null &&obj.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "test is an invalid value.");
            //}
            /*
             Use JavaScript to carry out client side validation is more efficient*/
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();//this executes the database update when you are ready
                TempData["Success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            return View();
            
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id==null || id.Value == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.Get(u=>u.Id==id);
            //Category? categoryFromDb1 = _categoryRepo.FirstOrDefault(c=>c.Name.Contains("Sci"));
            //Category? categoryFromDb2 = _categoryRepo.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            //Category objCategory = _db.Categories.FirstOrDefault( c => c.Id == id.Value);
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //Additional Input Validation options - these are server side and require a reload
            if (obj.Name != null && obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
            }

             //Use JavaScript to carry out client side validation is more efficient*/
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();//this executes the database update when you are ready
                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id.Value == 0)
            {
                return NotFound();
            }
           Category? categoryFromDb = _categoryRepo.Get(u=> u.Id == id);
           if (categoryFromDb == null)
            {
                return NotFound();
            }

            //Category objCategory = _categoryRepo.FirstOrDefault( c => c.Id == id.Value);
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _categoryRepo.Get(u=>u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["Success"] = "Category deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}
