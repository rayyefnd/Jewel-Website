<%@ Page Title="Transaction Details" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransactionDetailView.aspx.cs" Inherits="PSDLab_Project.Views.TransactionDetailView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-width: 800px; margin: 20px auto;">
        <h2 style="text-align: center;">Transaction Details</h2>
        <hr />

        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" EnableViewState="false" CssClass="d-block mb-3 text-center"></asp:Label>

        <div class="mb-3" style="margin-top: 20px">
            <strong>
                <asp:Label ID="lblTransactionIDStatic" runat="server" Text="Transaction ID:" />
            </strong>
            <asp:Label ID="lblTransactionIDValue" runat="server" Font-Bold="true" />
        </div>

        <h4 class="mt-4">Transaction Items</h4>
        <asp:GridView ID="gvTransactionItems" runat="server" AutoGenerateColumns="False"
            CssClass="table table-hover table-bordered"
            EmptyDataText="No items found for this transaction.">
            <Columns>
                <asp:BoundField DataField="JewelName" HeaderText="Jewel Name" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            </Columns>
        </asp:GridView>

        <div class="text-center mt-4" style="margin-top: 15px">
            <asp:HyperLink ID="hlBackToOrders" runat="server" NavigateUrl="~/Views/MyOrder.aspx"
                Text="&laquo; Back to order history" CssClass="btn btn-secondary" />
        </div>
    </div>
</asp:Content>
