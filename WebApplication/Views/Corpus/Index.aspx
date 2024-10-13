<%@ Page Language="C#" MasterPageFile="~/Site.Master" Inherits="WebApplication.Views.Corpus.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    语料管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_webpager.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_corpus.js") %>"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
	    <ul>
		    <li><a href="#tabTextPool">原始语料</a></li>
            <li><a href="#tabDictionary">原始词典</a></li>
            <li><a href="#tabCutSentence">语料断句检测</a></li>
            <li><a href="#tabParseDictionary">词典切分检测</a></li>
            <li><a href="#tabClearDictionary">清理词典词条</a></li>
            <li><a href="#tabClearAmbiguity">处理歧义词条</a></li>
	    </ul>
	    <div id="tabTextPool">
            <h4>详细信息：</h4>
            <p>（1）原始语料由其他文件导入生成；<br />（2）即使存在明显错误，原始语料不允许被更改和删除；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(tid)：</th><td><input id="txtTID1" width="100%" height="100%"/>&nbsp;<button id="btnQuery1" onclick="btnTextPoolQuery_Click('1');">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength1" width="100%" height="100%" disabled="true"/></td>
                    <th>状态(parsed)：</th><td><input id="txtParsed1" width="100%" height="100%" disabled="true"/></td>
                    <th>结果值(result)：</th><td><input id="txtResult1" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="7">
                    <textarea id="txtContent1" rows="8" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem1" onclick="btnTextPoolPrevItem_Click('1');" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem1" onclick="btnTextPoolNextItem_Click('1');" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem1" onclick="btnTextPoolRandomItem_Click('1');" class="killbutton">随机一条</a>&nbsp;
            <a href="#" id="btnClearItem1" onclick="btnTextPoolClearItem_Click('1');" class="killbutton">清理语料</a>&nbsp;
            <hr />
            <div id="divWebPager1">
            </div>
            <div id="divTextPool"></div>
	    </div>
        <div id="tabDictionary">
            <h4>详细信息：</h4>
            <p>（1）词典语料由其他文件导入生成；<br />（2）词典语料允许被屏蔽，使其不参与各种分析运算；<br />（3）即使存在明显错误，词典语料不允许被更改和删除；</p>
            <hr />
             <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(did)：</th><td><input id="txtDID2" width="100%" height="100%"/>&nbsp;<button id="btnQuery2" onclick="btnDictionaryQuery_Click('2');">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength2" width="100%" height="100%" disabled="true"/></td>
                    <th>统计词频(count)：</th><td><input id="txtCount2" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr>
                    <th>是否可用(enable)：</th><td><input id="txtEnable2" width="100%" height="100%" disabled="true"/>&nbsp;<button id="btnSwitch2" onclick="btnDictionarySwitch_Click('2');">切换</button></td>
                    <th>分类(classification)：</th>
                        <td><select id="ddlClassification2">
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
                    <th>内容标识(cid)：</th><td><input id="txtCID2" width="100%" height="100%" disabled="true"/>&nbsp;<button id="btnInsert2" onclick="btnDictionaryInsertItem_Click('2');">增加</button></td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="5">
                    <textarea id="txtContent2" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>备注(remark)：</th><td colspan="5">
                    <textarea id="txtRemark2" rows="2" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem2" onclick="btnDictionaryPrevItem_Click('2');" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem2" onclick="btnDictionaryNextItem_Click('2');" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem2" onclick="btnDictionaryRandomItem_Click('2');" class="killbutton">随机一条</a>&nbsp;
            <hr />
            <div id="divWebPager2">
            </div>
            <div id="divDictionary"></div>
	    </div>
	    <div id="tabCutSentence">
            <h4>语料断句检测：</h4>
            <p>（1）利用解析规则，将句子解析成段；<br />（2）利用“消去法”判断是否符合句式；<br />（3）对不符合规则或者超长（超过450个Unicod）的句子不允许入库；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(tid)：</th><td><input id="txtTID3" width="100%" height="100%"/>&nbsp;<button id="btnQuery3" onclick="btnTextPoolQuery_Click('3');">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength3" width="100%" height="100%" disabled="true"/></td>
                    <th>状态(parsed)：</th><td><input id="txtParsed3" width="100%" height="100%" disabled="true"/></td>
                    <th>结果值(result)：</th><td><input id="txtResult3" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="7">
                    <textarea id="txtContent3" rows="8" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem3" onclick="btnTextPoolPrevItem_Click('3');" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem3" onclick="btnTextPoolNextItem_Click('3');" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem3" onclick="btnTextPoolRandomItem_Click('3');" class="killbutton">随机一条</a>&nbsp;
            <a href="#" id="btnExecuteItem3" onclick="btnExecuteItem3_Click();" class="killbutton">执行断句</a>&nbsp;
            <hr />
            <div id="divCutSentence"></div>
	    </div>
        <div id="tabParseDictionary">
            <h4>词典切分检测：</h4>
            <p>（1）对词典的内容进行简单切分检测；</p>
            <hr />
             <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(did)：</th><td><input id="txtDID4" width="100%" height="100%"/>&nbsp;<button id="btnQuery4" onclick="btnDictionaryQuery_Click('4');">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength4" width="100%" height="100%" disabled="true"/></td>
                    <th>统计词频(count)：</th><td><input id="txtCount4" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr>
                    <th>是否可用(enable)：</th><td><input id="txtEnable4" width="100%" height="100%" disabled="true"/>&nbsp;<button id="btnSwitch4" onclick="btnDictionarySwitch_Click('4');">切换</button></td>
                    <th>分类(classification)：</th>
                        <td colspan="3"><select id="ddlClassification4">
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
                    <textarea id="txtContent4" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>备注(remark)：</th><td colspan="5">
                    <textarea id="txtRemark4" rows="2" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem4" onclick="btnDictionaryPrevItem_Click('4');" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem4" onclick="btnDictionaryNextItem_Click('4');" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem4" onclick="btnDictionaryRandomItem_Click('4');" class="killbutton">随机一条</a>&nbsp;
            <a href="#" id="btnFMMAllItem4" onclick="btnDictionaryFMMAll_Click('4');" class="killbutton">执行正向切分</a>&nbsp;
            <a href="#" id="btnBMMAllItem4" onclick="btnDictionaryBMMAll_Click('4');" class="killbutton">执行逆向切分</a>&nbsp;
            <hr />
            <div id="divParseDictionary"></div>
	    </div>
        <div id="tabClearDictionary">
            <h4>清理词典词条：</h4>
            <p>（1）关闭词典中意义不正确、不明确或者易引起歧义的词条；</p>
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>批量选项(options)：</th><td colspan="5">
                    <textarea id="txtContent5" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnInsertItems5" onclick="btnDictionaryInsertItems_Click('5');" class="killbutton">批量增加</a>&nbsp;
            <a href="#" id="btnRemoveItems5" onclick="btnDictionaryRemoveItems_Click('5');" class="killbutton">批量删除</a>&nbsp;
            <hr />
            <div id="divWebPager5">
            </div>
            <div id="divClearDictionary"></div>
	    </div>
        <div id="tabClearAmbiguity">
            <h4>处理歧义词条：</h4>
            <p>（1）通过正向和逆向切分找出引起解析歧义的词条；<br />
                （2）关闭词典中意义不正确、不明确或者易引起歧义的词条；</p>
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>批量选项(options)：</th><td colspan="5">
                    <textarea id="txtContent6" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnInsertItems6" onclick="btnAmbiguityInsertItems_Click('6');" class="killbutton">批量增加</a>&nbsp;
            <a href="#" id="btnRemoveItems6" onclick="btnAmbiguityRemoveItems_Click('6');" class="killbutton">批量删除</a>&nbsp;
            <a href="#" id="btnClearItems6" onclick="btnAmbiguityClearItems_Click('6');" class="killbutton">快速清理</a>&nbsp;
            <hr />
            <div id="divWebPager6">
            </div>
            <div id="divClearAmbiguity"></div>
	    </div>
    </div>
    <div id="errorDialog"><div id="errorDialogText" style="width:350px;height:250px;overflow:auto"></div></div>

</asp:Content>
