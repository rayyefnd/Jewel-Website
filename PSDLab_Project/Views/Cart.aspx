<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="PSDLab_Project.Views.Cart" MasterPageFile="~/Views/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 800px; margin: 20px auto;">
        <div style="text-align: center">
            <h2 >My Cart</h2>
        </div>
        <hr />

        <asp:Label ID="lblError" runat="server" ForeColor="Red" EnableViewState="False" />
        <br />

        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" 
            OnRowCommand="gvCart_RowCommand" DataKeyNames="CartID,JewelID"
            OnRowEditing="gvCart_RowEditing" OnRowCancelingEdit="gvCart_RowCancelingEdit"
            OnRowUpdating="gvCart_RowUpdating" CssClass="table table-hover" ShowFooter="True">
            <Columns>
                <asp:BoundField DataField="JewelID" HeaderText="Jewel ID" ReadOnly="True" />
                <asp:BoundField DataField="JewelName" HeaderText="Jewel Name" ReadOnly="True" />
                <asp:BoundField DataField="BrandName" HeaderText="Brand" ReadOnly="True" />
                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" ReadOnly="True" />
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Bind("Quantity") %>' Width="50px" TextMode="Number"></asp:TextBox>
                        <asp:RangeValidator ID="rvQuantity" runat="server" ControlToValidate="txtQuantity"
                            ErrorMessage="Quantity must be > 0" Type="Integer" MinimumValue="1" MaximumValue="1000" ForeColor="Red" Display="Dynamic" />
                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                            ErrorMessage="Quantity is required" ForeColor="Red" Display="Dynamic" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" ReadOnly="True" />
                <asp:CommandField ShowEditButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-sm btn-primary" />
                <asp:ButtonField Text="Remove" ButtonType="Button" CommandName="Remove" ControlStyle-CssClass="btn btn-sm btn-danger" />
            </Columns>
            <EmptyDataTemplate>
                Your cart is empty.
            </EmptyDataTemplate>
        </asp:GridView>
        <br />
        <div style="text-align: right; margin-bottom: 20px;">
            <strong><asp:Label ID="lblTotalPriceText" runat="server" Text="Total Price: " /></strong>
            <asp:Label ID="lblTotalPrice" runat="server" Font-Bold="True" />
        </div>

        <div style="margin-top: 20px;">
            <asp:Button ID="btnClearCart" runat="server" Text="Clear Cart" OnClick="btnClearCart_Click" CssClass="btn btn-warning" />
        </div>

        <hr />

        <h3>Checkout</h3>
        <div>
            <asp:Label ID="lblPaymentMethod" runat="server" Text="Select Payment Method:" AssociatedControlID="ddlPaymentMethod" />
            <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control" Width="200px">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvPaymentMethod" runat="server" ControlToValidate="ddlPaymentMethod"
                ErrorMessage="Please select a payment method." InitialValue="" ForeColor="Red" Display="Dynamic" />
        </div>
        <br />
        <div>
            <asp:Button ID="btnCheckout" runat="server" Text="Proceed to Checkout" OnClick="btnCheckout_Click" CssClass="btn btn-success" />
            <asp:Button ID="btnBackToHomepage" runat="server" Text="Back to Homepage" OnClick="btnCancel_Click" Style="margin-left: 5px" />
        </div>
    </div>
</asp:Content>