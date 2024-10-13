<%@ Page Language="C#" MasterPageFile="~/Site.Master" Inherits="WebApplication.Views.Rule.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    规则管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_webpager.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_rule.js") %>"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
	    <ul>
		    <li><a href="#tabFilterRule">过滤规则</a></li>
            <li><a href="#tabRegularRule">提取规则</a></li>
            <li><a href="#tabNumericalRule">数量规则</a></li>
            <li><a href="#tabAttributeRule">词性规则</a></li>
            <li><a href="#tabPhraseRule">短语规则</a></li>
            <li><a href="#tabParseRule">断句规则</a></li>
            <li><a href="#tabStructRule">结构规则</a></li>
	    </ul>
	    <div id="tabFilterRule">
            <h4>测试过滤规则：</h4>
            <p>（1）替代或删除文本中多余的字符；<br />（2）“b”表示空格（即：blankspace）的缩写；<br />（3）利用REPLACE函数，匹配“规则项”则替代为“替代项”；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput1" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td>
                        <textarea id="txtOutput1" rows="8" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
            </table>
            <a href="#" id="btnCheckRule1" onclick="btnCheckRule1_Click();" class="killbutton">执行</a>&nbsp;
            <hr />
            <div id="divWebPager1">
            </div>
            <div id="divFilterRule"></div>
	    </div>
	    <div id="tabRegularRule">
            <h4>测试提取规则：</h4>
            <p>（1）按照多个正则规则提取内容；<br />（2）利用dbo.RegExMatches函数，匹配正则规则；<br />（3）在去除包含和覆盖问题后，得到最终提取结果；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput2" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput2"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnCheckRule2" onclick="btnCheckRule2_Click();" class="killbutton">执行</a>&nbsp;
            <hr />
            <div id="divWebPager2">
            </div>
            <div id="divRegularRule"></div>
	    </div>
	    <div id="tabNumericalRule">
            <h4>测试数量规则：</h4>
            <p>（1）按照多个正则规则提取内容；<br />（2）利用dbo.RegExMatches函数，匹配正则规则；<br />（3）在去除包含和覆盖问题后，得到最终提取结果；<br />（4）$c表示中文数字；$d表示整数；$e表示英文字母；$f表示浮点数；$n表示数字编号；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput3" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput3"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnCheckRule3" onclick="btnCheckRule3_Click();" class="killbutton">执行</a>&nbsp;
            <hr />
            <div id="divWebPager3">
            </div>
            <div id="divNumericalRule"></div>
	    </div>
	    <div id="tabAttributeRule">
            <h4>测试词性规则：</h4>
            <p>（1）按照多个正则规则提取内容；<br />（2）利用dbo.RegExMatches函数，匹配正则规则；<br />（3）在去除包含和覆盖问题后，得到最终提取结果；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput4" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput4"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnCheckRule4" onclick="btnCheckRule4_Click();" class="killbutton">执行</a>&nbsp;
            <hr />
            <div id="divWebPager4">
            </div>
            <div id="divAttributeRule"></div>
	    </div>
	    <div id="tabPhraseRule">
            <h4>测试短语规则：</h4>
            <p>（1）按照多个正则规则提取内容；<br />（2）利用dbo.RegExMatches函数，匹配正则规则；<br />（3）在去除包含和覆盖问题后，得到最终提取结果；<br />（4）$c表示中文数字；$d表示整数；$e表示英文字母；$f表示浮点数；$n表示数字编号；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput5" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput5"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnCheckRule5" onclick="btnCheckRule5_Click();" class="killbutton">执行</a>&nbsp;
            <hr />
            <div id="divWebPager5">
            </div>
            <div id="divPhraseRule"></div>
	    </div>
	    <div id="tabParseRule">
            <h4>测试断句规则：</h4>
            <p>（1）$表示不含标点符号的字符串；<br />（2）a~z表示参数的序号，最多接受26个字符串；<br />（3）单条句子的长度不能超过450个Unicode字符；<br />
                （4）使用规则利用“消去法”逐步消去符号，并确定一个可提取的句子；<br />（5）规定以下符号为句子所用标点符号：【《》（）〈〉‘，’：“；”。？！】</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput6" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput6"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnCheckRule6" onclick="btnCheckRule6_Click();" class="killbutton">执行</a>&nbsp;
            <hr />
            <div id="divWebPager6">
            </div>
            <div id="divParseRule"></div>
	    </div>
	    <div id="tabStructRule">
            <h4>测试结构规则：</h4>
            <p>（1）$表示不含标点符号的字符串；<br />
               （2）a~z表示参数的序号，最多接受26个字符串；<br />
               （3）利用dbo.RegExMatches函数，对字符串进行参数化处理；<br />
               （4）在不影响正确参数化的情况下，允许适当使用正则规则辅助处理；
            </p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput7" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput7"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnCheckRule7" onclick="btnCheckRule7_Click();" class="killbutton">执行</a>&nbsp;
            <hr />
            <div id="divWebPager7">
            </div>
            <div id="divStructRule"></div>
	    </div>
    </div>
    <div id="errorDialog"><div id="errorDialogText" style="width:350px;height:250px;overflow:auto"></div></div>

</asp:Content>
