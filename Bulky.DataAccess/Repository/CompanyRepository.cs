using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) :base(db)
        { 
            _db = db;
        }


        //public void Save()
        //{
        //    _db.SaveChanges();
        //}

        public void Upsert(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
