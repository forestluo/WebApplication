<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebPager.ascx.cs" Inherits="WebApplication.WebPager" %>
<table class="gridview">
    <tr>
        <td id="pleft" stype="width: 50%">
            总共有<asp:Label ID="lbl_TotalCount" runat="server" Text="0"></asp:Label>条记录&nbsp; &nbsp; &nbsp;
            每页显示<asp:DropDownList ID="ddl_PageSize" runat="server">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="20" Selected="True">20</asp:ListItem>
                <asp:ListItem Value="40">40</asp:ListItem>
                <asp:ListItem Value="60">60</asp:ListItem>
                <asp:ListItem Value="80">80</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="500">500</asp:ListItem>
                <asp:ListItem Value="1000">1000</asp:ListItem>
            </asp:DropDownList>条记录
        </td>
        <td>
            <asp:Label ID="Label7" runat="server" Text="当前页码为："></asp:Label>
            [
            <asp:Label ID="lbl_CurrPage" runat="server" Text="0"></asp:Label>
            ]
            <asp:Label ID="Label6" runat="server" Text="总页码为："></asp:Label>
            [
            <asp:Label ID="lbl_TotalPage" runat="server" Text="0"></asp:Label>
            ]
            <asp:LinkButton ID="btn_FirstPage" runat="server" Font-Underline="false" OnClick="btn_FirstPage_Click">第一页</asp:LinkButton>
            <asp:LinkButton ID="btn_PrevPage" runat="server" Font-Underline="false" OnClick="btn_PrevPage_Click">上一页</asp:LinkButton>
            <asp:LinkButton ID="btn_NextPage" runat="server" Font-Underline="false" OnClick="btn_NextPage_Click">下一页</asp:LinkButton>&nbsp;
            <asp:LinkButton ID="btn_LastPage" runat="server" Font-Underline="false" OnClick="btn_LastPage_Click">最后一页</asp:LinkButton>&nbsp; &nbsp;
            转到第<asp:TextBox ID="txt_PageNum" runat="server" Width="60px"></asp:TextBox>页
            <asp:LinkButton ID="btn_GO" runat="server" OnClick="btn_GO_Click">GO</asp:LinkButton>
        </td>
    </tr>
</table>