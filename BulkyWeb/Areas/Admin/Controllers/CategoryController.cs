using BulkyBook.DataAccess.Data;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        //private readonly ApplicationDbContext _db;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll(null).ToList();
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
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();//this executes the database update when you are ready
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u=>u.CategoryId==id);
            //Category? categoryFromDb1 = _categoryRepo.FirstOrDefault(c=>c.Name.Contains("Sci"));
            //Category? categoryFromDb2 = _categoryRepo.Where(u=>u.CategoryId==id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            //Category objCategory = _db.Categories.FirstOrDefault( c => c.CategoryId == id.Value);
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
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();//this executes the database update when you are ready
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
           Category? categoryFromDb = _unitOfWork.Category.Get(u=> u.CategoryId == id);
           if (categoryFromDb == null)
            {
                return NotFound();
            }

            //Category objCategory = _categoryRepo.FirstOrDefault( c => c.CategoryId == id.Value);
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u=>u.CategoryId == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}
