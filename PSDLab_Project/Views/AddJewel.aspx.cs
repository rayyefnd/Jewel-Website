using PSDLab_Project.Models;
using PSDLab_Project.Handlers;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSDLab_Project.Views
{
    public partial class AddJewel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = Session["user"] as User;
            if (user == null || user.Role != "Admin")
            {
                Response.Redirect("~/Views/HomePage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDropdowns();
            }
        }

        private void LoadDropdowns()
        {
            ddlCategory.DataSource = JewelHandler.GetAllCategories();
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));

            ddlBrand.DataSource = JewelHandler.GetAllBrands();
            ddlBrand.DataBind();
            ddlBrand.Items.Insert(0, new ListItem("-- Select Brand --", ""));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/HomePage.aspx");
        }

        protected void btnAddJewel_Click(object sender, EventArgs e)
        {
            string name = txtJewelName.Text.Trim();
            string categoryIdStr = ddlCategory.SelectedValue;
            string brandIdStr = ddlBrand.SelectedValue;
            string priceStr = txtPrice.Text.Trim();
            string yearStr = txtReleaseYear.Text.Trim();

            int categoryId, brandId, releaseYear;
            decimal price;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(categoryIdStr) ||
                string.IsNullOrEmpty(brandIdStr) || string.IsNullOrEmpty(priceStr) ||
                string.IsNullOrEmpty(yearStr))
            {
                lblError.Text = "All fields must be filled.";
                return;
            }

            if (!int.TryParse(categoryIdStr, out categoryId))
            {
                lblError.Text = "Please select a valid category.";
                return;
            }

            if (!int.TryParse(brandIdStr, out brandId))
            {
                lblError.Text = "Please select a valid brand.";
                return;
            }

            if (!decimal.TryParse(priceStr, out price))
            {
                lblError.Text = "Price must be a valid number.";
                return;
            }

            if (!int.TryParse(yearStr, out releaseYear))
            {
                lblError.Text = "Release year must be a valid number.";
                return;
            }

            string result = JewelHandler.AddJewel(name, categoryId, brandId, price, releaseYear);

            if (result == null)
            {
                Response.Redirect("~/Views/HomePage.aspx?status=add_success");
            }
            else
            {
                lblError.Text = result;
            }
        }
    }
}