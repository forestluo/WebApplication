<%@ Page Language="C#" MasterPageFile="~/Site.Master" Inherits="WebApplication.Views.Application.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    应用管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_webpager.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_application.js") %>"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
	    <ul>
		    <li><a href="#tabQueryFreq">查询词频</a></li>
            <li><a href="#tabPhraseRule">编辑正则规则</a></li>
            <li><a href="#tabParseRule">编辑解析规则</a></li>
	    </ul>
        <div id="tabQueryFreq">
            <h4>查询词频：</h4>
            <p>（1）查询内容以及相关内容的词频；<br />
               （2）内容的词频以词典统计词频为标准；<br />
               （3）相关内容的词频为包含该内容（即：“%内容%”）的相关词频统计；
            </p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(did)：</th><td><input id="txtDID1" width="100%" height="100%" disabled="true"/></td>
                    <th>长度(length)：</th><td><input id="txtLength1" width="100%" height="100%" disabled="true"/></td>
                    <th>统计词频(count)：</th><td><input id="txtCount1" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr>
                    <th>是否可用(enable)：</th><td><input id="txtEnable1" width="100%" height="100%" disabled="true"/></td>
                    <th>分类(classification)：</th>
                        <td colspan="3"><select id="ddlClassification1">
                                <option value="财经">财经</option>
                                <option value="餐饮">餐饮</option>
                                <option value="常用词">常用词</option>
                                <option value="成语">成语</option>
                                <option value="地点">地点</option>
                                <option value="电影">电影</option>
                                <option value="动漫">动漫</option>
                                <option value="动物">动物</option>
                                <option value="法律">法律</option>
                                <option value="分词21万">分词21万</option>
                                <option value="分词30万">分词30万</option>
                                <option value="分词35万">分词35万</option>
                                <option value="公司">公司</option>
                                <option value="公司缩写">公司缩写</option>
                                <option value="古名">古名</option>
                                <option value="股票">股票</option>
                                <option value="基本词汇">基本词汇</option>
                                <option value="计算机">计算机</option>
                                <option value="历史">历史</option>
                                <option value="名录">名录</option>
                                <option value="名人">名人</option>
                                <option value="农牧">农牧</option>
                                <option value="汽车">汽车</option>
                                <option value="亲近名">亲近名</option>
                                <option value="日文名">日文名</option>
                                <option value="社会科学">社会科学</option>
                                <option value="生活常识">生活常识</option>
                                <option value="诗词">诗词</option>
                                <option value="食物">食物</option>
                                <option value="搜狗">搜狗</option>
                                <option value="体育">体育</option>
                                <option value="网游">网游</option>
                                <option value="网站">网站</option>
                                <option value="文学">文学</option>
                                <option value="现代汉语词典" selected="true">现代汉语词典</option>
                                <option value="新华字典">新华字典</option>
                                <option value="行业">行业</option>
                                <option value="姓名">姓名</option>
                                <option value="姓氏">姓氏</option>
                                <option value="医学">医学</option>
                                <option value="应用工程">应用工程</option>
                                <option value="英文单词">英文单词</option>
                                <option value="英文名">英文名</option>
                                <option value="娱艺">娱艺</option>
                                <option value="职业">职业</option>
                                <option value="自然科学">自然科学</option>
                                <option value="组织机构">组织机构</option>
                            </select>
                        </td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="5">
                    <textarea id="txtContent1" rows="4" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>备注(remark)：</th><td colspan="5">
                    <textarea id="txtRemark1" rows="2" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem1" onclick="btnDictionaryPrevItem_Click('1');" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem1" onclick="btnDictionaryNextItem_Click('1');" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem1" onclick="btnDictionaryRandomItem_Click('1');" class="killbutton">随机一条</a>&nbsp;
            <a href="#" id="btnSwitchItem1" onclick="btnDictionarySwitchItem_Click('1');" class="killbutton">切换可用状态</a>&nbsp;
            <a href="#" id="btnQueryFreqItem1" onclick="btnDictionaryQueryFreqItem_Click('1');" class="killbutton">查询相关词频</a>&nbsp;
            <hr />
            <h4>查询结果：</h4>
            <div id="divQueryFreq"></div>
            <hr />
            <h4>相关词频：</h4>
            <div id="divWebPager1"></div>
            <div id="divQueryRelatedFreq"></div>
            <hr />
        </div>
        <div id="tabPhraseRule">
            <h4>编辑规则：</h4>
            <p>（1）可以编辑属性、数词、数量词和短语规则；<br />
               （2）用于过滤、断句和预处理规则不允许直接编辑；<br />
               （3）对规则的编辑处理要格外慎重，会影响到很多解析程序；<br />
               （4）$c表示中文数字；$d表示整数；$e表示英文字母；$f表示浮点数；$n表示数字编号；<br />
               （5）当有解析程序（或作业）在正常运行时，不应同时进行规则修改和编辑，否则会产生无法预期的结果；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(rid)：</th><td><input id="txtRID2" width="100%" height="100%" disabled="true"/></td>
                    <th>属性(attribute)：</th>
                        <td><select id="ddlAttribute2">
                                <option value="结构">结构</option>
                                <option value="分句">分句</option>
                                <option value="单句">单句</option>
                                <option value="并列结构">并列结构</option>
                                <option value="偏正结构">偏正结构</option>
                                <option value="动宾结构">动宾结构</option>
                                <option value="介宾结构">介宾结构</option>
                                <option value="补充结构">补充结构</option>
                                <option value="主谓结构">主谓结构</option>
                                <option value="的字结构">的字结构</option>
                                <option value="兼语结构">兼语结构</option>
                                <option value="连动结构">连动结构</option>
                                <option value="固定结构">固定结构</option>
                                <option value="单动谓语句">单动谓语句</option>
                                <option value="动宾谓语句">动宾谓语句</option>
                                <option value="动补谓语句">动补谓语句</option>
                                <option value="连动谓语句">连动谓语句</option>
                                <option value="兼语谓语句">兼语谓语句</option>
                                <option value="形谓句">形谓句</option>
                                <option value="名谓句">名谓句</option>
                                <option value="“是”字句">“是”字句</option>
                                <option value="“有”字句">“有”字句</option>
                                <option value="“把”字句">“把”字句</option>
                                <option value="“被”字句">“被”字句</option>
                                <option value="“使”字句">“使”字句</option>
                                <option value="“比”字句">“比”字句</option>
                                <option value="非主谓句">非主谓句</option>
                                <option value="存现句">存现句</option>
                                <option value="疑问句">疑问句</option>
                                <option value="祈使句">祈使句</option>
                                <option value="评议句">评议句</option>
                            </select></td>
                    <th>分类(classification)：</th>
                        <td><select id="ddlClassification2">
                                <option value="数词">数词</option>
                                <option value="数量词">数量词</option>
                                <option value="短语" selected="true">短语</option>
                                <option value="句子">句子</option>
                            </select></td>
                </tr>
                <tr><th>规则(rule)：</th><td colspan="5">
                    <textarea id="txtRule2" rows="4" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>其他要求(requirements)：</th><td colspan="5">
                    <textarea id="txtRequirements2" rows="2" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;" disabled="true"></textarea></td></tr>
            </table>
            <a href="#" id="btnAddRuleItem2" onclick="btnAddPhraseRule_Click('2');" class="killbutton">新增规则</a>&nbsp;
            <a href="#" id="btnEditRuleItem2" onclick="btnEditPhraseRule_Click('2');" class="killbutton">修改规则</a>&nbsp;
            <hr/>
            <h4>测试规则：</h4>
            <p>（1）测试当前规则仅测试当前正在处理的该条规则；<br />
               （2）正向（逆向）切分测试将使用混合规则进行完整正向（逆向）切分测试；</p>
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput2" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput2"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnRandomContentItem2" onclick="btnRandomContentItem_Click('2');" class="killbutton">随机内容</a>&nbsp;
            <a href="#" id="btnCheckRuleItem2" onclick="btnCheckPhraseRuleItem_Click('2');" class="killbutton">测试当前规则</a>&nbsp;
            <a href="#" id="btnCheckStoredRuleItem2" onclick="btnCheckStoredPhraseRule_Click('2');" class="killbutton">测试库存规则</a>
            <hr />
            <h4>正则规则：</h4>
            <div id="divWebPager2"></div>
            <div id="divPhraseRule"></div>
        </div>
        <div id="tabParseRule">
            <h4>编辑规则：</h4>
            <p>（1）可以编辑结构解析规则（非正则方法）；<br />
               （2）用于过滤、断句和预处理规则不允许直接编辑；<br />
               （3）对规则的编辑处理要格外慎重，会影响到很多解析程序；<br />
               （4）$表示不含标点符号的字符串，a~z则表示参数的顺序及名称；<br />
               （5）当有解析程序（或作业）在正常运行时，不应同时进行规则修改和编辑，否则会产生无法预期的结果；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(rid)：</th><td><input id="txtRID3" width="100%" height="100%" disabled="true"/></td>
                    <th>前缀(prefix)：</th><td><input id="txtPrefix3" width="100%" height="100%" disabled="true"/></td>
                    <th>后缀(suffix)：</th><td><input id="txtSuffix3" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr><th>规则(rule)：</th><td colspan="5">
                    <textarea id="txtRule3" rows="4" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>参数个数(count)：</th><td><input id="txtCount3" width="100%" height="100%" disabled="true"/></td>
                    <th>最小长度(length)：</th><td><input id="txtLength3" width="100%" height="100%" disabled="true"/></td>
                    <th>优先级(priority)：</th><td><input id="txtPriority3" width="100%" height="100%" disabled="true"/></td>
                </tr>
            </table>
            <a href="#" id="btnAddRuleItem3" onclick="btnAddParseRule_Click('3');" class="killbutton">新增规则</a>&nbsp;
            <a href="#" id="btnEditRuleItem3" onclick="btnEditParseRule_Click('3');" class="killbutton">修改规则</a>&nbsp;
            <hr/>
            <h4>测试规则：</h4>
            <p>（1）测试当前规则仅测试当前正在处理的该条规则；<br />
               （2）正向（逆向）切分测试将使用混合规则进行完整正向（逆向）切分测试；</p>
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>输入内容：</th>
                    <td>
                        <textarea id="txtInput3" rows="8" cols="80"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea>
                    </td></tr>
                <tr><th>输出结果：</th>
                    <td><div id="divOutput3"><hr style="border:1px dotted #036"/></div></td></tr>
            </table>
            <a href="#" id="btnRandomContentItem3" onclick="btnRandomContentItem_Click('3');" class="killbutton">随机内容</a>&nbsp;
            <a href="#" id="btnCheckRuleItem3" onclick="btnCheckParseRuleItem_Click('3');" class="killbutton">测试当前规则</a>&nbsp;
            <a href="#" id="btnCheckStoredRuleItem3" onclick="btnCheckStoredParseRule_Click('3');" class="killbutton">测试库存规则</a>&nbsp;
            <a href="#" id="btnMultilayerSplitAllItem3" onclick="btnMultilayerSplitAll_Click('3');" class="killbutton">多层切分测试</a>&nbsp;
            <a href="#" id="btnFMMSplitAllItem3" onclick="btnFMMSplitAllStructRule_Click('3');" class="killbutton">正向切分测试</a>&nbsp;
            <a href="#" id="btnBMMSplitAllItem3" onclick="btnBMMSplitAllStructRule_Click('3');" class="killbutton">逆向切分测试</a>&nbsp;
            <hr />
            <h4>解析规则：</h4>
            <div id="divWebPager3"></div>
            <div id="divParseRule"></div>
        </div>
    </div>
    <div id="errorDialog"><div id="errorDialogText" style="width:350px;height:250px;overflow:auto"></div></div>
</asp:Content>
