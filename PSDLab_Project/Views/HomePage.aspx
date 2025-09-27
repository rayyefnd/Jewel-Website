<%@ Page Title="Home" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="PSDLab_Project.Views.HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 15px 5px;"> 
        <h1 style="text-align: center; margin-top: 10px; margin-bottom:20px  font-weight: bold;">Our Collection</h1>

        <div style="display: grid; grid-template-columns: repeat(auto-fill, minmax(210px, 1fr)); gap: 25px;">
            <asp:Repeater ID="RepeaterJewels" runat="server">
                <ItemTemplate>
                    <div style="border: 1px solid #eee; padding: 18px"> 
                        <h4 style="margin-top: 0; margin-bottom: 10px; font-size: 1.1em; font-weight: 600;">
                            <asp:Label ID="lblJewelName" runat="server" Text='<%# Eval("JewelName") %>'></asp:Label>
                        </h4>
                        <p style="font-size: 0.9em; margin-bottom: 5px; color: #555;">
                            ID: <asp:Label ID="lblJewelID" runat="server" Text='<%# Eval("JewelID") %>'></asp:Label>
                        </p>
                        <p style="font-size: 1em; margin-bottom: 18px; color: #333; font-weight: bold;">
                            Price: <asp:Label ID="lblJewelPrice" runat="server" Text='<%# Eval("Price", "{0:C}") %>'></asp:Label>
                        </p>
                        <asp:HyperLink ID="hlViewDetails" runat="server"
                            NavigateUrl='<%# Eval("JewelID", "~/Views/JewelDetail.aspx?id={0}") %>'
                            Text="View Details"
                            style="font-size: 0.9em; color: #007bff; text-decoration: none;"> 
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <div style="text-align: center; padding: 20px 0; width: 100%;">
                         <asp:Label ID="lblNoJewels" runat="server" Text="No jewels found." Visible='<%# RepeaterJewels.Items.Count == 0 %>' style="font-size: 1em; color: #666;"></asp:Label>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>