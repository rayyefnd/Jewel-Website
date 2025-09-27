using PSDLab_Project.Models; // Your model namespace
using PSDLab_Project.Repositories; // Assuming CartRepository is here
using System;
using System.Collections.Generic;
using System.Data.Entity; 
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization; 

namespace PSDLab_Project.Views
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
  
            var user = Session["user"] as User;
            if (user == null || user.Role != "Customer")
            {
                Response.Redirect("LoginPage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCart();
                PopulatePaymentMethods();
            }
        }

        private void LoadCart()
        {
            var user = Session["user"] as User;
            if (user == null) return; 

            using (var db = new JawelsDBEntities1()) 
            {

                var cartItems = db.Carts
                    .Include(c => c.Jewel)
                    .Include(c => c.Jewel.Brand)
                    .Where(c => c.UserID == user.UserID)
                    .ToList();

                var data = cartItems.Select(c => new
                {
                    c.CartID,
                    c.JewelID, 
                    JewelName = c.Jewel?.JewelName, 
                    BrandName = c.Jewel?.Brand?.BrandName, 
                    Price = c.Jewel?.Price ?? 0, 
                    c.Quantity,
                    Subtotal = (c.Quantity ?? 0) * (c.Jewel?.Price ?? 0) 
                }).ToList();

                gvCart.DataSource = data;
                gvCart.DataBind();

                if (data.Any())
                {
                    decimal totalPrice = data.Sum(item => item.Subtotal);
                    lblTotalPrice.Text = totalPrice.ToString("C", CultureInfo.GetCultureInfo("en-US"));
                }
                else
                {
                    lblTotalPrice.Text = (0M).ToString("C", CultureInfo.GetCultureInfo("en-US"));
                    lblError.Text = "Your cart is currently empty.";
                }
            }
        }

        private void PopulatePaymentMethods()
        {
 
            ddlPaymentMethod.Items.Clear();
            ddlPaymentMethod.Items.Add(new ListItem("-- Select Payment Method --", ""));
            ddlPaymentMethod.Items.Add(new ListItem("Credit Card", "Credit Card"));
            ddlPaymentMethod.Items.Add(new ListItem("Bank Transfer", "Bank Transfer"));
            ddlPaymentMethod.Items.Add(new ListItem("E-Wallet", "E-Wallet"));
        }

        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int cartId = Convert.ToInt32(gvCart.DataKeys[rowIndex].Values["CartID"]);

                using (var db = new JawelsDBEntities1()) //
                {
                    var cartItem = db.Carts.Find(cartId);
                    if (cartItem != null)
                    {
                        db.Carts.Remove(cartItem);
                        db.SaveChanges();
                        lblError.Text = "Item removed from cart.";
                    }
                }
                LoadCart(); 
                gvCart.EditIndex = -1; 
            }
        }

        protected void gvCart_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCart.EditIndex = e.NewEditIndex;
            LoadCart(); 
        }

        protected void gvCart_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCart.EditIndex = -1;
            LoadCart(); 
        }

        protected void gvCart_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvCart.Rows[e.RowIndex];
            int cartId = Convert.ToInt32(gvCart.DataKeys[e.RowIndex].Values["CartID"]);
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");

            if (txtQuantity != null && int.TryParse(txtQuantity.Text, out int quantity))
            {
                if (quantity <= 0)
                {
                    lblError.Text = "Quantity must be greater than 0.";
                    e.Cancel = true; 
                    return;
                }

                using (var db = new JawelsDBEntities1()) //
                {
                    var cartItem = db.Carts.Find(cartId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity = quantity;
                        db.SaveChanges();
                        lblError.Text = "Cart updated successfully.";
                    }
                }
                gvCart.EditIndex = -1; 
                LoadCart(); 
            }
            else
            {
                lblError.Text = "Invalid quantity format.";
                e.Cancel = true; 
            }
        }

        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            var user = Session["user"] as User;
            if (user == null) return;

            using (var db = new JawelsDBEntities1()) //
            {
                var userCartItems = db.Carts.Where(c => c.UserID == user.UserID);
                db.Carts.RemoveRange(userCartItems);
                db.SaveChanges();
                lblError.Text = "Cart has been cleared.";
            }
            LoadCart(); 
            gvCart.EditIndex = -1; 
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            var user = Session["user"] as User;
            if (user == null)
            {
                Response.Redirect("LoginPage.aspx"); 
                return;
            }

            if (string.IsNullOrEmpty(ddlPaymentMethod.SelectedValue))
            {
                lblError.Text = "Please select a payment method.";
                return;
            }

            using (var db = new JawelsDBEntities1()) 
            {
                var cartItems = db.Carts
                                  .Include(c => c.Jewel)
                                  .Where(c => c.UserID == user.UserID)
                                  .ToList();

                if (!cartItems.Any())
                {
                    lblError.Text = "Your cart is empty. Nothing to checkout.";
                    return;
                }

                TransactionHeader newTransactionHeader = new TransactionHeader 
                {
                    UserID = user.UserID,
                    TransactionDate = DateTime.Now,
                    PaymentMethod = ddlPaymentMethod.SelectedValue,
                    Status = "Payment Pending" 
                };

                newTransactionHeader.PaymentMethod = ddlPaymentMethod.SelectedValue;
                newTransactionHeader.Status = "Payment Pending"; 

                System.Diagnostics.Debug.WriteLine($"Attempting to save TransactionHeader with Status: {newTransactionHeader.Status}, PaymentMethod: {newTransactionHeader.PaymentMethod}");

                db.TransactionHeaders.Add(newTransactionHeader);
                db.SaveChanges(); 

                foreach (var cartItem in cartItems)
                {
                    TransactionDetail newDetail = new TransactionDetail
                    {
                        TransactionID = newTransactionHeader.TransactionID, 
                        JewelID = cartItem.JewelID,
                        Quantity = cartItem.Quantity

                    };
                    db.TransactionDetails.Add(newDetail);
                }

                db.Carts.RemoveRange(cartItems); 

                db.SaveChanges(); 

                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Text = "Checkout successful! Your order has been placed.";
                LoadCart(); 
                gvCart.EditIndex = -1; 
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/HomePage.aspx");
        }
    }
}