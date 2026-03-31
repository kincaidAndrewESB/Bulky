using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Categroy or any other class we want to use
        IEnumerable<T> GetAll();

        T Get(Expression<Func<T, bool>> filter);

        void Add(T entity);
        //void Update(T entity);
        /*Update is often better to do in class as there are often specific rules depending in T*/
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
