using BulkyBook.DataAccess.Data;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly ApplicationDbContext _db;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.CategoryId.ToString() });

            return View(objProductList);
        }

        //public IActionResult Create()
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.CategoryId.ToString() });
            //Can use ViewBag or View Data to add this lsit to the next view (for the duration of the next refresh) 
            //TempData is another option
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem 
                            { 
                                Text = u.Name, 
                                Value = u.CategoryId.ToString() 
                            }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //if no id it is an create i.e. blank view with jstu category for dropdown menu
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.ProductId == id);
                return View(productVM);
            }             
        }

        [HttpPost]
        //public IActionResult Create(ProductVM productVM)
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }


                    // ✅ Ensure directory exists
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }


                    productVM.Product.ImageUrl = @"\images\product\" + fileName;


                }
                if(productVM.Product.ProductId == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                    _unitOfWork.Save();//this executes the database update when you are ready
                TempData["Success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            else
            { 
                //else statement will populate the categroy dropdown if the ModelState is not valid
               productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.CategoryId.ToString()
                    });
                return View(productVM);
            }
               
            
        }

        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id.Value == 0)
        //    {
        //        return NotFound();
        //    }
        //   Product? productFromDb = _unitOfWork.Product.Get(u=> u.ProductId == id);
        //   if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    //Product objProduct = _productRepo.FirstOrDefault( c => c.ProductId == id.Value);
        //    return View(productFromDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u=>u.ProductId == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Product deleted successfully!";

        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
           return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
           var productToBeDeleted = _unitOfWork.Product.Get(u => u.ProductId == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            if( productToBeDeleted.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }           

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully!" });
        }

        #endregion
    }
}
