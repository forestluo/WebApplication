<%@ Page Language="C#" MasterPageFile="~/Site.Master" Inherits="WebApplication.Views.Content.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    内容管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_webpager.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/nldb_content.js") %>"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
	    <ul>
		    <li><a href="#tabInnerContent">核心内容</a></li>
            <li><a href="#tabOuterContent">外延内容</a></li>
            <li><a href="#tabExternalContent">语料内容</a></li>
            <li><a href="#tabSplitExternalContent">语料切分检测</a></li>
	    </ul>
	    <div id="tabInnerContent">
            <h4>详细内容：</h4>
            <p>（1）核心内容可视为终结符；<br />（2）内容标识为全局唯一标识序列；<br />（3）单字符内容均为最基础的核心内容标准；<br />（4）现代汉语词典、新华字典、成语词典和简明英汉词典为引入的内容标准；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>内容标识(cid)：</th><td><input id="txtCID1" width="100%" height="100%"/>&nbsp;<button id="btnQuery1" onclick="btnQuery1_Click();">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength1" width="100%" height="100%" disabled="true"/></td>
                    <th>统计词频(count)：</th><td><input id="txtCount1" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr>
                    <th>状态标志(type)：</th><td><input id="txtType1" width="100%" height="100%" disabled="true"/></td>
                    <th>分类(classification)：</th>
                        <td colspan="3"><select id="ddlClassification1">
                            <option value="编号序号">编号序号</option>
                            <option value="标点符号">标点符号</option>
                            <option value="成语词典">成语词典</option>
                            <option value="单位符号">单位符号</option>
                            <option value="俄语字母">俄语字母</option>
                            <option value="符号图案">符号图案</option>
                            <option value="汉语拼音">汉语拼音</option>
                            <option value="汉字">汉字</option>
                            <option value="货币符号">货币符号</option>
                            <option value="简明英汉词典">简明英汉词典</option>
                            <option value="箭头符号">箭头符号</option>
                            <option value="日文平假名片假名">日文平假名片假名</option>
                            <option value="数学符号">数学符号</option>
                            <option value="特殊符号">特殊符号</option>
                            <option value="希腊字母">希腊字母</option>
                            <option value="现代汉语词典">现代汉语词典</option>
                            <option value="新华字典">新华字典</option>
                            <option value="英文字母">英文字母</option>
                            <option value="制表符">制表符</option>
                            <option value="中文字符">中文字符</option>
                            </select>
                        </td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="5">
                    <textarea id="txtContent1" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>词性标注(attribute)：</th><td colspan="5">
                    <textarea id="txtAttribute1" rows="2" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem1" onclick="btnPrevItem1_Click();" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem1" onclick="btnNextItem1_Click();" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem1" onclick="btnRandomItem1_Click();" class="killbutton">随机一条</a>&nbsp;
            <hr />
            <div id="divWebPager1">
            </div>
            <div id="divInnerContent"></div>
	    </div>
	    <div id="tabOuterContent">
            <h4>详细内容：</h4>
            <p>（1）外延内容可视为用户拓展内容；<br />（2）同一内容具有相同的内容标识；<br />（3）外延内容可以为终结符号或者需要进一步解析；<br />（4）外延内容允许同一内容有不同的解析方式；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(oid)：</th><td><input id="txtOID2" width="100%" height="100%"/>&nbsp;<button id="btnQuery2" onclick="btnQuery2_Click();">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength2" width="100%" height="100%" disabled="true"/></td>
                    <th>统计词频(count)：</th><td><input id="txtCount2" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr>
                    <th>状态标志(type)：</th><td><input id="txtType2" width="100%" height="100%" disabled="true"/></td>
                    <th>分类(classification)：</th>
                        <td><select id="ddlClassification2">
                            <option value="文本">文本</option>
                            </select>
                        </td>
                    <th>内容标识(cid)：</th><td><input id="txtCID2" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="5">
                    <textarea id="txtContent2" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>解析规则(rule)：</th><td colspan="5">
                    <textarea id="txtRule2" rows="2" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem2" onclick="btnPrevItem2_Click();" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem2" onclick="btnNextItem2_Click();" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem2" onclick="btnRandomItem2_Click();" class="killbutton">随机一条</a>&nbsp;
            <hr />
            <div id="divWebPager2">
            </div>
            <div id="divOuterContent"></div>
        </div>
	    <div id="tabExternalContent">
            <h4>详细内容：</h4>
            <p>（1）语料内容为外部语料经整理后的内容；<br />（2）语料内容均需要进一步解析；</p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(eid)：</th><td><input id="txtEID3" width="100%" height="100%"/>&nbsp;<button id="btnQuery3" onclick="btnExternalQuery_Click('3');">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength3" width="100%" height="100%" disabled="true"/></td>
                    <th>语料标识(tid)：</th><td><input id="txtTID3" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr>
                    <th>状态标志(type)：</th><td><input id="txtType3" width="100%" height="100%" disabled="true"/></td>
                    <th>分类(classification)：</th>
                        <td colspan="3"><select id="ddlClassification3">
                            <option value="单句">单句</option>
                            <option value="错句">错句</option>
                            <option value="分句">分句</option>
                            </select>
                        </td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="5">
                    <textarea id="txtContent3" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>解析规则(rule)：</th><td colspan="5">
                    <textarea id="txtRule3" rows="2" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
            </table>
            <a href="#" id="btnPrevItem3" onclick="btnExternalPrevItem_Click('3');" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem3" onclick="btnExternalNextItem_Click('3');" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem3" onclick="btnExternalRandomItem_Click('3');" class="killbutton">随机一条</a>&nbsp;
            <hr />
            <div id="divWebPager3">
            </div>
            <div id="divExternalContent"></div>
        </div>
	    <div id="tabSplitExternalContent">
            <h4>语料切分检测：</h4>
            <p>（1）仅使用正则、数词和数量词规则；<br />
               （2）短语规则并不包含在此处切分算法内；<br />
               （3）正向切分和逆向切分算法得到的结果可能会有所不同；<br />
            </p>
            <hr />
            <table class="grid" border="0" cellspacing="2" cellpadding="2" width="100%">
                <tr><th>标识(eid)：</th><td><input id="txtEID4" width="100%" height="100%"/>&nbsp;<button id="btnQuery4" onclick="btnExternalQuery_Click('4');">查询</button></td>
                    <th>长度(length)：</th><td><input id="txtLength4" width="100%" height="100%" disabled="true"/></td>
                    <th>语料标识(tid)：</th><td><input id="txtTID4" width="100%" height="100%" disabled="true"/></td>
                </tr>
                <tr>
                    <th>状态标志(type)：</th><td><input id="txtType4" width="100%" height="100%" disabled="true"/></td>
                    <th>分类(classification)：</th>
                        <td colspan="3"><select id="ddlClassification4">
                            <option value="单句">单句</option>
                            <option value="错句">错句</option>
                            <option value="分句">分句</option>
                            </select>
                        </td>
                </tr>
                <tr><th>内容(content)：</th><td colspan="5">
                    <textarea id="txtContent4" rows="4" cols="80" disabled="true"
                        style="overflow:visible;width:100%;overflow:auto;word-break:break-all;"></textarea></td></tr>
                <tr><th>解析规则(rule)：</th><td colspan="2">
                    <input id="txtRule4" style="width: 90%; height: 100%" disabled="true"/></td>
                    <th>合并内容(combinated content)：</th><td colspan="2">
                    <input id="txtCombinated4" style="width: 80%; height: 100%"/>
                        &nbsp;<button id="btnCombinated4" onclick="btnExternalCombinate_Click('4');">添加</button></td>
                </tr>
            </table>
            <a href="#" id="btnPrevItem4" onclick="btnExternalPrevItem_Click('4');" class="killbutton">上一条</a>&nbsp;
            <a href="#" id="btnNextItem4" onclick="btnExternalNextItem_Click('4');" class="killbutton">下一条</a>&nbsp;
            <a href="#" id="btnRandomItem4" onclick="btnExternalRandomItem_Click('4');" class="killbutton">随机一条</a>&nbsp;
            <a href="#" id="btnExecuteItem4" onclick="btnSplitExternalContent_Click();" class="killbutton">简单切分</a>&nbsp;
            <a href="#" id="btnFMMExecuteItem4" onclick="btnFMMSplitExternalContent_Click();" class="killbutton">正向切分</a>&nbsp;
            <a href="#" id="btnBMMExecuteItem4" onclick="btnBMMSplitExternalContent_Click();" class="killbutton">逆向切分</a>&nbsp;
            <hr />
            <div id="divSplitExternalContent"></div>
        </div>
    </div>
    <div id="errorDialog"><div id="errorDialogText" style="width:350px;height:250px;overflow:auto"></div></div>

</asp:Content>
