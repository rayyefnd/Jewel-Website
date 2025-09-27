using PSDLab_Project.Handlers;
using PSDLab_Project.Models;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace PSDLab_Project.Views
{
    public partial class ProfilePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {

                if (Request.Cookies["user_cookie"] == null)
                {
                    Response.Redirect("~/Views/LoginPage.aspx");
                    return;
                }
          
                if (Session["user"] == null)
                {
                    Response.Redirect("~/Views/LoginPage.aspx");
                    return;
                }
            }

            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        private void LoadUserProfile()
        {
            User currentUser = Session["user"] as User;
            if (currentUser != null)
            {
                lblEmail.Text = currentUser.Email;
                lblUsername.Text = currentUser.Username;
                lblGender.Text = currentUser.Gender;
                lblDOB.Text = currentUser.DateOfBirth.ToString("dd/MM/yyyy");
            }
            else
            {
               
                Response.Redirect("~/Views/LoginPage.aspx");
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            lblPasswordError.Visible = false;
            lblPasswordSuccess.Visible = false;

            User currentUser = Session["user"] as User;
            if (currentUser == null)
            {
                Response.Redirect("~/Views/LoginPage.aspx");
                return;
            }

            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowError("All password fields must be filled.");
                return;
            }

            if (newPassword.Length < 8 || newPassword.Length > 25)
            {
                ShowError("New Password must be 8 to 25 characters long.");
                return;
            }

            if (!Regex.IsMatch(newPassword, @"^[a-zA-Z0-9]+$"))
            {
                ShowError("New Password must be alphanumeric.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowError("New Password and Confirm Password do not match.");
                return;
            }

            string result = UserHandler.UpdatePassword(currentUser.UserID, oldPassword, newPassword);

            if (result == null) 
            {
                ShowSuccess("Password changed successfully!");
                txtOldPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";

                HttpCookie cookie = Request.Cookies["user_cookie"];
                if (cookie != null)
                {

                    cookie.Expires = DateTime.Now.AddDays(-1); 
                    Response.Cookies.Add(cookie);

                }

            }
            else
            {
                ShowError(result);
            }
        }

        private void ShowError(string message)
        {
            lblPasswordError.Text = message;
            lblPasswordError.Visible = true;
            lblPasswordSuccess.Visible = false;
        }

        private void ShowSuccess(string message)
        {
            lblPasswordSuccess.Text = message;
            lblPasswordSuccess.Visible = true;
            lblPasswordError.Visible = false;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/HomePage.aspx");
        }
    }
}