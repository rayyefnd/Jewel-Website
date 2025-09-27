using PSDLab_Project.Models; 
using System;
using System.Linq;
using System.Web.UI;
using System.Data.Entity; 

namespace PSDLab_Project.Views
{
    public partial class TransactionDetailView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
  
            var currentUser = Session["user"] as User;
            if (currentUser == null || currentUser.Role != "Customer")
            {
                Response.Redirect("~/Views/LoginPage.aspx"); 
                return;
            }

            if (!IsPostBack)
            {
                
                if (Request.QueryString["TransactionID"] == null)
                {
                    ShowError("ID Transaksi tidak ditemukan di URL.");
                    return;
                }

                int transactionIdFromQuery;
                if (!int.TryParse(Request.QueryString["TransactionID"], out transactionIdFromQuery))
                {
                    ShowError("Format ID Transaksi tidak valid.");
                    return;
                }

                
                LoadTransactionDetails(transactionIdFromQuery, currentUser.UserID);
            }
        }

        private void LoadTransactionDetails(int transactionId, int currentUserID)
        {
            using (var db = new JawelsDBEntities1()) 
            {
                
                var transactionHeader = db.TransactionHeaders
                                          .FirstOrDefault(th => th.TransactionID == transactionId && th.UserID == currentUserID);

                if (transactionHeader == null)
                {
                    ShowError("Detail transaksi tidak ditemukan atau Anda tidak memiliki izin untuk melihatnya.");
                    lblTransactionIDValue.Text = "N/A";
                    return;
                }

                
                lblTransactionIDValue.Text = transactionHeader.TransactionID.ToString();

                
                var transactionItems = db.TransactionDetails
                                        .Include(td => td.Jewel) 
                                        .Where(td => td.TransactionID == transactionId)
                                        .Select(td => new
                                        {
                                            JewelName = td.Jewel != null ? td.Jewel.JewelName : "Data Permata Tidak Tersedia", 
                                            Quantity = td.Quantity ?? 0 
                                        })
                                        .ToList();

                gvTransactionItems.DataSource = transactionItems;
                gvTransactionItems.DataBind();

                if (!transactionItems.Any())
                {
                    lblErrorMessage.Text = "Tidak ada item detail untuk transaksi ini.";
                }
            }
        }

        private void ShowError(string message)
        {
            lblErrorMessage.Text = message;
            lblErrorMessage.Visible = true;
            gvTransactionItems.Visible = false; 
            lblTransactionIDValue.Text = "N/A";
        }
    }
}