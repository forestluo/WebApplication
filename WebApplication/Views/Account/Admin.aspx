<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        //from: http://stackoverflow.com/questions/610406/javascript-equivalent-to-printf-string-format
        String.prototype.format = function () {
            var args = arguments;
            if (typeof args[0] == "object")
                return this.format.apply(this, args[0]);
            return this.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] != 'undefined'
                    ? args[number]
                    : match
                    ;
            });
        };

        var NLDB = { selectedDBName: null, errorDialog: null };

        NLDB.QueryDBController = function (controller, data, callback) {
            var url = '<%=Url.Action("", "DB") %>/' + controller;
            $.ajax({
                url: url,
                dataType: 'json',
                data: data,
                success: callback,
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status == 403) {
                        window.location = '<%=Url.Action("Login", "Account") %>';
                }
                else {
                    var errorData = JSON.parse(jqXHR.responseText);
                    $("#errorDialogText").text(errorData.StackTrace);
                    NLDB.errorDialog.dialog('option', 'title', errorData.Message);
                    NLDB.errorDialog.dialog('open');
                }
              }
            });

        };

        NLDB.updateSelectedDB = function () {
            NLDB.selectedDBName = $('#selectDatabase').val();
            NLDB.QueryDBController("UpdateSelectedDB", {'selectedDBName': NLDB.selectedDBName}, function (data) { NLDB.ShowSpaceUsedSummary(data); } );

            NLDB.RefreshCurrentlySelectedTab();
        };

        NLDB.ShowSpaceUsedSummary = function (spaceUsedSummary) {
            $('#SpaceUsedSummary').html("<b>数据库名称：</b>{0}；<b>占用空间：</b>{1}；<b>未分配空间：</b>{2}，<b>保留空间</b>{3}；<b>数据文件：</b>{4}；<b>索引文件：</b>{5}；<b>未使用：</b>{6}。".format(spaceUsedSummary));
        };

        NLDB.GetRunningQueries = function () {
            NLDB.ShowLoadingSpinner('#tabRunningQueries');
            NLDB.QueryDBController("GetRunningQueries", null, function (data) { NLDB.RenderRunningQueries(data); } );
        };

        NLDB.RenderRunningQueries = function (data) {

            //add a column in each row for killing the process
            for(var i in data)
            {
                var sessionId = data[i]["session_id"];
                data[i]["Kill Process"] = '<a href="#" onclick="NLDB.KillRunningQuery('+sessionId+')" class="killbutton">Kill ' + sessionId + '</a>';
            }
            NLDB.RenderTable('#tabRunningQueries', data);

            $("a.killbutton").button();
        };

        NLDB.KillRunningQuery = function(sessionId) {
            if(confirm("确认终止进程（id： " + sessionId + "）？"))
                NLDB.QueryDBController("KillRunningQuery", {'sessionId': sessionId}, function (data) { NLDB.RefreshCurrentlySelectedTab(); } );
        };

        NLDB.GetQueryHistory = function () {
            NLDB.ShowLoadingSpinner('#tabsQueryHistory');
            NLDB.QueryDBController("GetQueryHistory", null, function (data) { NLDB.RenderQueryHistory(data); } );
        };

        NLDB.RenderQueryHistory = function (data) {
            NLDB.RenderTable('#tabsQueryHistory', data);
        };

        NLDB.GetIndexFragmentation = function () {
            NLDB.ShowLoadingSpinner('#tabIndexFragmentation');
            NLDB.QueryDBController("GetIndexFragmentation", null, function (data) { NLDB.RenderIndexFragmentation(data); } );
        };

        NLDB.RenderIndexFragmentation = function (data) {
            NLDB.RenderTable('#tabIndexFragmentation', data);
        };

        NLDB.ShowLoadingSpinner = function(target) {
            $(target).html('<img src="<%=Url.Action("", "") %>Images/ajax-loader.gif" />');
        };

        NLDB.ExecuteQuery = function() {
            var query = $("#txtExecuteQuery").val();
            NLDB.QueryDBController("ExecuteQuery", {'query':query}, 
                function (data) { 
                    var html = "";
                    for(var i in data)
                    {
                        html += NLDB.GetTableHTML(data[i]);
                    }
                    $("#divExecuteQueryResults").html(html); 
                } 
            );
        };

        NLDB.RefreshCurrentlySelectedTab = function(selectedIndex) {
            if( typeof selectedIndex == "undefined" )
                selectedIndex = $( "#tabs" ).tabs('option', 'selected');
            switch(selectedIndex)
            {
                case 0:
                    NLDB.GetRunningQueries();
                    break;
                case 1:
                    NLDB.GetQueryHistory();
                    break;
                case 2:
                    NLDB.GetIndexFragmentation();
                    break;
                case 3:
                    //do nothing
                    break;
                default:
                    alert("unknown tab index!");
                    break;
            }
        };

        NLDB.RenderTable = function(targetElement, data) {
            $(targetElement).html(NLDB.GetTableHTML(data));
        };

        NLDB.GetTableHTML = function(data) {  
            if(typeof data == "undefined" || data == null || data.length == 0)
            {
                return "No results found";
            }

            var html = "";
            html += '<table class="grid" border="0" cellspacing="0" cellpadding="0">';
            html += '<tr valign="top"><td>';
            html += '<table id="" class="gridview" border="0">';

            // headers
            html += '<tr valign="top" class="gridHeaderRow">';
            for(var name in data[0])
            {
                html += '   <th align="center">' + name + '</th>';
            }
            html += '</tr>';

            // data rows
            for(var i in data)
            {
                var even = '';
                if ((parseInt(i) + 1) % 2 == 0) even = ' class="even"';
                html += '<tr valign="top"' + even + '>';
                for(var name in data[i])
                {
                    var value = data[i][name];
                    if(value !== null && typeof value.length != "undefined" && value.length > 255)
                    {
                        var tooltip = value;
                        var toShow = value.substr(0, 255);
                        //check to see if we can extract a FUNCTION or Stored Proc name out of this
                        var regex = new RegExp(/\s*CREATE (PROCEDURE|FUNCTION|VIEW)\s*\S*\s/g);
                        var match = value.match(regex);
                        if(match)
                        {
                            toShow = match[0];
                        }
                        value = '<span title="' + tooltip + '">' + toShow + '...</span>';
                    }

                    html += '   <td align="right">' + value + '</td>';
                }
                html += '</tr>';
            }
            html += '</table>';
            html += '</td></tr>';
            html += '</table>';
            
            return html;
        };

        $(function () {

            $('#selectDatabase').change(NLDB.updateSelectedDB);

            $( "#tabs" ).tabs({
               'select': function(event, ui) { NLDB.RefreshCurrentlySelectedTab(ui.index); }
            });

            NLDB.ShowSpaceUsedSummary(<%= ViewData["SpaceUsedSummary"]%>);
            NLDB.GetRunningQueries();
            $("#btnExecuteQuery").button();

            NLDB.errorDialog = $("#errorDialog").dialog({ autoOpen: false, title: '遇到一个错误', width: 400, height: 300 });
        });

    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	数据管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form action="." method="post">
        <select id="selectDatabase" name="selectDatabase">
            <%= ViewData["DBDropDown"]%>
        </select>
    </form>

    <div id="SpaceUsedSummary">
    
    </div>
    <br /><br />
    <div id="tabs">
	    <ul>
		    <li><a href="#tabRunningQueries">现行进程</a></li>
		    <li><a href="#tabsQueryHistory">历史记录</a></li>
            <li><a href="#tabIndexFragmentation">索引碎片</a></li>
            <li><a href="#tabExecuteQuery">执行SQL</a></li>
	    </ul>
	    <div id="tabRunningQueries">
		
	    </div>
	    <div id="tabsQueryHistory">
		
	    </div>
	    <div id="tabIndexFragmentation">
		
        </div>
        <div id="tabExecuteQuery">
		    <h4>SQL语句：</h4>
            <textarea id="txtExecuteQuery" rows="5" cols="60"></textarea>
            <br />
            <a href="#" id="btnExecuteQuery" onclick="NLDB.ExecuteQuery()">立即执行</a>
            <br />
            <div id="divExecuteQueryResults"></div>
        </div>
    </div>
    <div id="errorDialog"><div id="errorDialogText" style="width:350px;height:250px;overflow:auto"></div></div>
</asp:Content>
