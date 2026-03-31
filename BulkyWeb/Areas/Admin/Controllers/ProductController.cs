using BulkyBook.DataAccess.Data;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        //private readonly ApplicationDbContext _db;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //Additional Input Validation options - these are server side and require a reload
            //if (obj.Title != null && obj.Title == obj.ISBN.ToString())
            //{
            //    ModelState.AddModelError("Title", "The DisplayOrder cannot exactly match the Title.");
            //}
            
            /*
             Use JavaScript to carry out client side validation is more efficient*/
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();//this executes the database update when you are ready
                TempData["Success"] = "Product created successfully!";
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
            Product? productFromDb = _unitOfWork.Product.Get(u=>u.ProductId==id);
            //Product? productFromDb1 = _productRepo.FirstOrDefault(c=>c.Title.Contains("Sci"));
            //Product? productFromDb2 = _productRepo.Where(u=>u.ProductId==id).FirstOrDefault();
            if (productFromDb == null)
            {
                return NotFound();
            }

            //Product objProduct = _db.Categories.FirstOrDefault( c => c.ProductId == id.Value);
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            //Additional Input Validation options - these are server side and require a reload
            //if (obj.Title != null && obj.Title == obj.ISBN.ToString())
            //{
            //    ModelState.AddModelError("Title", "The DisplayOrder cannot exactly match the Title.");
            //}

             //Use JavaScript to carry out client side validation is more efficient*/
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();//this executes the database update when you are ready
                TempData["Success"] = "Product updated successfully!";
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
           Product? productFromDb = _unitOfWork.Product.Get(u=> u.ProductId == id);
           if (productFromDb == null)
            {
                return NotFound();
            }

            //Product objProduct = _productRepo.FirstOrDefault( c => c.ProductId == id.Value);
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u=>u.ProductId == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}
