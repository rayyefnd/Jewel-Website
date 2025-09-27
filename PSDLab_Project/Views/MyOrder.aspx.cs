using PSDLab_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization; 

namespace PSDLab_Project.Views
{
    public partial class MyOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Session["user"] as User; 
            if (user == null || user.Role != "Customer")
            {
                Response.Redirect("~/Views/LoginPage.aspx"); 
                return;
            }

            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            var user = Session["user"] as User; 
            if (user == null)
            {
                return;
            }

            using (var db = new JawelsDBEntities1()) 
            {
                var orders = db.TransactionHeaders 
                                .Where(th => th.UserID == user.UserID)
                                .OrderByDescending(th => th.TransactionDate) 
                                .Select(th => new
                                {
                                    th.TransactionID,
                                    th.TransactionDate,
                                    th.PaymentMethod,
                                    th.Status
                                })
                                .ToList();

                gvOrders.DataSource = orders;
                gvOrders.DataBind();

                if (!orders.Any())
                {
                    lblMessage.Text = "You have not placed any orders yet.";
                    lblMessage.CssClass = "alert alert-info";
                }
                else
                {
                    lblMessage.Text = ""; 
                }
            }
        }

        protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string status = gvOrders.DataKeys[e.Row.RowIndex].Values["Status"]?.ToString();

                if (status == "Arrived")
                {
                    Button btnConfirm = (Button)e.Row.FindControl("btnConfirmPackage");
                    Button btnReject = (Button)e.Row.FindControl("btnRejectPackage");

                    if (btnConfirm != null) btnConfirm.Visible = true;
                    if (btnReject != null) btnReject.Visible = true;
                }
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int transactionId;
            if (!int.TryParse(e.CommandArgument?.ToString(), out transactionId))
            {
                lblMessage.Text = "Invalid Transaction ID.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            var user = Session["user"] as User; 
            if (user == null)
            {
                Response.Redirect("~/Views/LoginPage.aspx");
                return;
            }

            if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"~/Views/TransactionDetailView.aspx?TransactionID={transactionId}");
            }
            else if (e.CommandName == "ConfirmPackage" || e.CommandName == "RejectPackage")
            {
                using (var db = new JawelsDBEntities1()) 
                {
                    var transactionHeader = db.TransactionHeaders.FirstOrDefault(th => th.TransactionID == transactionId && th.UserID == user.UserID); 

                    if (transactionHeader != null)
                    {
                        if (transactionHeader.Status == "Arrived") 
                        {
                            if (e.CommandName == "ConfirmPackage")
                            {
                                transactionHeader.Status = "Done";
                                lblMessage.Text = $"Order ID {transactionId} confirmed successfully.";
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                            }
                            else 
                            {
                                transactionHeader.Status = "Rejected";
                                lblMessage.Text = $"Order ID {transactionId} has been marked as rejected.";
                                lblMessage.ForeColor = System.Drawing.Color.Orange;
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            lblMessage.Text = "This action can only be performed on orders with 'Arrived' status.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Order not found or you do not have permission to modify it.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                LoadOrders(); 
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/HomePage.aspx");
        }

    }
}