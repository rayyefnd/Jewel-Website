using PSDLab_Project.Models;
using PSDLab_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSDLab_Project.Handlers
{
    public class UserHandler
    {
        public static string Register(User user)
        {
            if (!UserRepository.IsEmailUnique(user.Email))
            {
                return "Email already registered.";
            }

            try
            {
                UserRepository.AddUser(user);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); 
                return "An error occurred during registration. Please try again.";
            }
        }

        public static User Login(string email, string password)
        {
            return UserRepository.GetUserByEmailAndPassword(email, password);
        }

        public static string UpdatePassword(int userId, string oldPassword, string newPassword)
        {
            User user = UserRepository.GetUserById(userId);
            if (user == null)
            {
                return "User not found."; 
            }

            if (user.Password != oldPassword)
            {
                return "Old password does not match.";
            }

            try
            {
                bool success = UserRepository.UpdateUserPassword(userId, newPassword);
                if (success)
                {
                    return null; 
                }
                else
                {
                    return "Could not update password. User not found or no changes made.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "An error occurred while updating the password. Please try again.";
            }
        }
    }
}