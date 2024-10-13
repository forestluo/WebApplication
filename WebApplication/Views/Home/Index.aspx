<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Inherits="System.Web.Mvc.ViewPage"%>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    首页
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        欢迎来到NLDB（数据库管理网站）!
    </h2>
    <p>
        NLDB（数据库管理网站）提供通过浏览器网页管理NLDB。
    </p>
    <p>
        它让用户可以观察NLDB中相关的数据库实例。
    </p>
    <b>NLDB具有以下功能：</b>
    <ul>
        <li>运行SQL语句。</li>
        <li>显示SQL语句历史，可查找最耗资源的查询语句。</li>
        <li>显示存储索引碎片。</li>
    </ul>
    NLDB允许用户中止正在执行的SQL语句或者运行自己的SQL语句。
    <p>
    <%= ViewData["Message"] %>
    </p>
</asp:Content>


