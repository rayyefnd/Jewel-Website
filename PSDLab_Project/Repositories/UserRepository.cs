using PSDLab_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity; 

namespace PSDLab_Project.Repositories
{
    public class UserRepository
    {
        public static bool IsEmailUnique(string email)
        {
            using (var db = new JawelsDBEntities1())
            {
                return !db.Users.Any(u => u.Email == email);
            }
        }

        public static User GetUserByEmailAndPassword(string email, string password)
        {
            using (var db = new JawelsDBEntities1())
            {

                return db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            }
        }

        public static void AddUser(User user)
        {
            using (var db = new JawelsDBEntities1())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
        public static User GetUserById(int userId)
        {
            using (var db = new JawelsDBEntities1())
            {
                return db.Users.FirstOrDefault(u => u.UserID == userId);
            }
        }

        public static bool UpdateUserPassword(int userId, string newPassword)
        {
            using (var db = new JawelsDBEntities1())
            {
                User userToUpdate = db.Users.FirstOrDefault(u => u.UserID == userId);
                if (userToUpdate != null)
                {
                    userToUpdate.Password = newPassword; 
                    db.Entry(userToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return true; 
                }
                return false;
            }
        }
    }
}