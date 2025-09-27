<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="MyOrder.aspx.cs" Inherits="PSDLab_Project.Views.MyOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-width: 1000px; margin: 20px auto; text-align: center;">
        <h2 style="text-align: center;">My Order History</h2>
        <hr />
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" CssClass="mb-3"></asp:Label>

        <div style="display: inline-block; margin-top: 20px">
            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" DataKeyNames="TransactionID,Status"
                OnRowCommand="gvOrders_RowCommand" OnRowDataBound="gvOrders_RowDataBound"
                CssClass="table table-hover table-striped" EmptyDataText="You have no orders yet.">
                <Columns>
                    <asp:BoundField DataField="TransactionID" HeaderText="Order ID" SortExpression="TransactionID" />
                    <asp:BoundField DataField="TransactionDate" HeaderText="Order Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" SortExpression="TransactionDate" />
                    <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" SortExpression="PaymentMethod" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnViewDetails" runat="server" Text="View Details" CommandName="ViewDetails"
                                CommandArgument='<%# Eval("TransactionID") %>' CssClass="btn btn-info btn-sm" />
                            <asp:Button ID="btnConfirmPackage" runat="server" Text="Confirm Package" CommandName="ConfirmPackage"
                                CommandArgument='<%# Eval("TransactionID") %>' CssClass="btn btn-success btn-sm ms-1" Visible="False" />
                            <asp:Button ID="btnRejectPackage" runat="server" Text="Reject Package" CommandName="RejectPackage"
                                CommandArgument='<%# Eval("TransactionID") %>' CssClass="btn btn-danger btn-sm ms-1" Visible="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Button ID="btnBackToHomepage" runat="server" Text="Back to Homepage" OnClick="btnCancel_Click" Style="margin-left: 5px; display:flex; flex-direction: column;" />
    </div>
</asp:Content>
