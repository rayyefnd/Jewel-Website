using PSDLab_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PSDLab_Project.Repositories
{
    public class TransactionRepository
    {
        public static List<TransactionHeader> GetUnfinishedOrders()
        {
            using (var db = new JawelsDBEntities1())
            {

                var unfinishedStatuses = new List<string> { "Payment Pending", "Shipment Pending", "Arrived" };

                return db.TransactionHeaders
              
                         .Where(th => unfinishedStatuses.Contains(th.Status))
                          .Include(th => th.TransactionDetails)
                         .OrderBy(th => th.TransactionDate) 
                         .ToList();
            }
        }

        public static bool UpdateTransactionStatus(int transactionId, string newStatus)
        {
            using (var db = new JawelsDBEntities1())
            {
                var transaction = db.TransactionHeaders.Find(transactionId);
                if (transaction != null)
                {
                    transaction.Status = newStatus;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public static List<TransactionHeader> GetTransactionsByUserId(int userId)
        {
            using (var db = new JawelsDBEntities1())
            {
                return db.TransactionHeaders
                         .Where(th => th.UserID == userId)
                         .OrderByDescending(th => th.TransactionDate)    
                         .ToList();
            }
        }
    }
}