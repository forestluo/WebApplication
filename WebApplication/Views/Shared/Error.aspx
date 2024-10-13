<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Error
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3>An Error Occurred:</h3>
    <p><%: Model.Exception %></p>
</asp:Content>
