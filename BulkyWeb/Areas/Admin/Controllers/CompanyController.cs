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
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        //private readonly ApplicationDbContext _db;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            
            return View(objCompanyList);
        }

        //public IActionResult Create()
        public IActionResult Upsert(int? companyId)
        {
            if (companyId == null || companyId == 0)
            {
                //if no id it is an create i.e. blank view with just category for dropdown menu
                return View(new Company());     
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.CompanyId == companyId);
                return View(companyObj);
            }             
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if(companyObj.CompanyId == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Upsert(companyObj);
                }
                _unitOfWork.Save();//this executes the database update when you are ready
                TempData["Success"] = "Company created successfully!";
                return RedirectToAction("Index");
            }
            else
            { 
               
                return View(companyObj);
            }
               
            
        }

        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
           return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? companyId)
        {
           var companyToBeDeleted = _unitOfWork.Company.Get(u => u.CompanyId == companyId);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }            
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company deleted successfully!" });
        }

        #endregion
    }
}
