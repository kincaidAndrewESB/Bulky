using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        //void Save(); moved to IUnitOfWork

        void UpdateStatus(int id, string orderStatus, string? paymentStaus = null);
        void updateStripePaymentId(int id, string sessionId, string paymentId);
    } 


}
