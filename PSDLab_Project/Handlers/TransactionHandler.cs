using PSDLab_Project.Models;
using PSDLab_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSDLab_Project.Handlers
{
    public class TransactionHandler
    {
        public static List<TransactionHeader> GetUnfinishedOrders()
        {
            return TransactionRepository.GetUnfinishedOrders();
        }

        public static bool UpdateStatus(int transactionId, string newStatus)
        {
            if (string.IsNullOrEmpty(newStatus)) return false;

            return TransactionRepository.UpdateTransactionStatus(transactionId, newStatus);
        }
        public static List<TransactionHeader> GetTransactionsByUserId(int userId)
        {
            return TransactionRepository.GetTransactionsByUserId(userId);
        }
    }
}