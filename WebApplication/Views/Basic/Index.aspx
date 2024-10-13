<%@ Page Language="C#" MasterPageFile="~/Site.Master" Inherits="WebApplication.Views.Basic.Index"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    基础信息
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_webpager.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_basic.js") %>"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
	    <ul>
		    <li><a href="#tabExceptionLog">异常日志</a></li>
		    <li><a href="#tabStatisticInfo">数据统计</a></li>
			<li><a href="#tabTextPoolInfo">语料统计</a></li>
			<li><a href="#tabDictionaryInfo">词典统计</a></li>
			<li><a href="#tabRuleInfo">规则统计</a></li>
			<li><a href="#tabContentInfo">内容统计</a></li>
	    </ul>
	    <div id="tabExceptionLog">
            <div id="divWebPager1">
             </div>
            <div id="divExceptionLog"></div>
	    </div>
	    <div id="tabStatisticInfo">
			<h4>总体统计：</h4>
	        <div id="divStatisticInfo">
	        </div>
			<hr />
	    </div>
	    <div id="tabTextPoolInfo">
			<h4>语料统计：</h4>
	        <div id="divTextPoolInfo">
	        </div>
			<hr />
			<h4>状态统计：</h4>
	        <div id="divTextPoolParsedInfo">
	        </div>
			<hr />
			<h4>长度统计：</h4>
	        <div id="divTextPoolLengthInfo">
	        </div>
			<hr />
	    </div>
	    <div id="tabDictionaryInfo">
			<h4>语料统计：</h4>
	        <div id="divDictionaryInfo">
	        </div>
			<hr />
			<h4>状态统计：</h4>
	        <div id="divDictionaryEnabledInfo">
	        </div>
			<hr />
			<h4>词频统计：</h4>
	        <div id="divDictionaryCountInfo">
	        </div>
			<hr />
			<h4>长度统计：</h4>
	        <div id="divDictionaryLengthInfo">
	        </div>
			<hr />
			<h4>类别统计：</h4>
	        <div id="divDictionaryClassificationInfo">
	        </div>
			<hr />
		</div>
		<div id="tabRuleInfo">
			<h4>过滤规则统计：</h4>
	        <div id="divFilterRuleInfo">
	        </div>
			<hr />
			<h4>提取规则统计：</h4>
	        <div id="divRegularRuleInfo">
	        </div>
			<hr />
			<h4>数量规则统计：</h4>
	        <div id="divNumericalRuleInfo">
	        </div>
			<hr />
			<h4>词性规则统计：</h4>
	        <div id="divAttributeRuleInfo">
	        </div>
			<hr />
			<h4>短语规则统计：</h4>
	        <div id="divPhraseRuleInfo">
	        </div>
			<hr />
			<h4>断句规则统计：</h4>
	        <div id="divParseRuleInfo">
	        </div>
			<hr />
		</div>
		<div id="tabContentInfo">
			<h4>核心内容统计：</h4>
	        <div id="divInnerContentInfo">
	        </div>
			<hr />
			<h4>外延内容统计：</h4>
	        <div id="divOuterContentInfo">
	        </div>
			<hr />
			<h4>语料内容统计：</h4>
	        <div id="divExternalContentInfo">
	        </div>
			<hr />
		</div>
    </div>
    <div id="errorDialog"><div id="errorDialogText" style="width:350px;height:250px;overflow:auto"></div></div>

</asp:Content>

