using PSDLab_Project.Dataset;
using PSDLab_Project.Handlers;
using PSDLab_Project.Models;
using PSDLab_Project.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PSDLab_Project.Views
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CrystalReport1 report = new CrystalReport1();
            CrystalReportViewer1.ReportSource = report;
            DataSet1 dataSet = getData(TransactionHandler.GetUnfinishedOrders());
            report.SetDataSource(dataSet);
        }
        private DataSet1 getData(List<TransactionHeader> transactions)
        {
            DataSet1 dataSet = new DataSet1();
            var headertable = dataSet.TransactionHeader;
            var detailtable = dataSet.TransactionDetail;

            foreach (TransactionHeader th in transactions)
            {
                var hrow = headertable.NewRow();
                hrow["TransactionID"] = th.TransactionID;
                hrow["UserID"] = th.UserID;
                hrow["TransactionDate"] = th.TransactionDate;
                hrow["PaymentMethod"] = th.PaymentMethod;
                hrow["Status"] = th.Status;
                headertable.Rows.Add(hrow);

                foreach (TransactionDetail d in th.TransactionDetails)
                {
                    var drow = detailtable.NewRow();
                    drow["TransactionDetailID"] = d.TransactionDetailID;
                    drow["TransactionID"] = d.TransactionID;
                    drow["JewelID"] = d.JewelID;
                    drow["Quantity"] = d.Quantity;
                    detailtable.Rows.Add(drow); 
                }
            }
            return dataSet;
        }
    }
}