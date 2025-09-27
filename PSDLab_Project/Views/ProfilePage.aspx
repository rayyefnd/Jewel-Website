<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProfilePage.aspx.cs" Inherits="PSDLab_Project.Views.ProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="profile-container">
        <div class="profile-header" style="text-align: center">
            <h2>User Profile</h2>
        </div>

        <div class="profile-info" style="line-height: 10px">
            <p><strong>Email:</strong> <asp:Label ID="lblEmail" runat="server"></asp:Label></p>
            <p><strong>Username:</strong> <asp:Label ID="lblUsername" runat="server"></asp:Label></p>
            <p><strong>Gender:</strong> <asp:Label ID="lblGender" runat="server"></asp:Label></p>
            <p><strong>Date of Birth:</strong> <asp:Label ID="lblDOB" runat="server"></asp:Label></p>
        </div>

        <hr />

     <div class="change-password-section">
        <h3>Change Password</h3>

        <div style="display: flex; align-items: center; margin-bottom: 5px;">
            <asp:Label AssociatedControlID="txtOldPassword" runat="server" style="width: 180px;">Old Password:</asp:Label>
            <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password" style="width: 200px;" />
        </div>

        <div style="display: flex; align-items: center; margin-bottom: 5px;">
            <asp:Label AssociatedControlID="txtNewPassword" runat="server" style="width: 180px;">New Password:</asp:Label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" style="width: 200px;" />
        </div>

        <div style="display: flex; align-items: center; margin-bottom: 5px;">
            <asp:Label AssociatedControlID="txtConfirmPassword" runat="server" style="width: 180px;">Confirm New Password:</asp:Label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" style="width: 200px;" />
        </div>

        <br />

        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" OnClick="btnChangePassword_Click" CssClass="btn-submit" />
        <asp:Label ID="lblPasswordError" runat="server" CssClass="lblError" Visible="false"></asp:Label>
        <asp:Label ID="lblPasswordSuccess" runat="server" CssClass="lblSuccess" Visible="false"></asp:Label>
        <asp:Button ID="btnBackToHomepage" runat="server" Text="Back to Homepage" OnClick="btnCancel_Click" Style="margin-left: 5px" />

    </div>

    </div>
</asp:Content>