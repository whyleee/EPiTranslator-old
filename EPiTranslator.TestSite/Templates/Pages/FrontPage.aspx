<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FrontPage.aspx.cs" Inherits="EPiTranslator.TestSite.Templates.Pages.FrontPage" MasterPageFile="~/Templates/MasterPages/Site.Master" %>
<%@ Import Namespace="EPiTranslator" %>

<asp:Content ContentPlaceHolderID="main" runat="server">
    <h1>EPiTranslator Test Site</h1>
    
    <%= this.L("/Test/Translation", "Hi, {0}, this is a test translation", "Paul") %>
</asp:Content>
