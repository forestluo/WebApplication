////////////////////////////////////
//
// NLDB Web Pager
//
////////////////////////////////////

var NLDBWebPager =
{
    //Pager
    DataCount: 0,
    CurrPageNum: 1,
    TotalPageNum: 1,
    PageSize: 20,

    id: "1",
    divDataName: "divResult1",
    divPagerName: "divWebPager1",

    countAction: null,
    selectAction: null,
};


NLDBWebPager.RefreshCountResult = function () {

    var instance = this;
    NLDB.QueryBasicController(this.countAction,
        null, function (data) {
            instance.DataCount = data != null ? parseInt(data[0]["总数"]) : 0;
        });
}

NLDBWebPager.RefreshSelectResult = function () {

    var instance = this;
    NLDB.QueryBasicController(this.selectAction,
        { 'pageIndex': this.CurrPageNum, 'pageSize': this.PageSize },
        function (data) { instance.RenderResult(data); });
};

NLDBWebPager.RefreshPagerStatus = function () {

    this.RefreshCountResult();

    if (this.PageSize < 10) this.PageSize = 10;
    this.TotalPageNum = parseInt(this.DataCount / this.PageSize);
    if (this.DataCount % this.PageSize != 0) this.TotalPageNum ++;
    if (this.TotalPageNum <= 0) this.TotalPageNum = 1;
    if (this.CurrPageNum <= 0) this.CurrPageNum = 1;
    if (this.CurrPageNum > this.TotalPageNum) this.CurrPageNum = this.TotalPageNum;
};

NLDBWebPager.CheckButtonStatus = function () {

    if (this.DataCount <= 0 || this.TotalPageNum <= 1) {
        document.getElementById("btnFirstPage" + this.id).disabled = true;
        document.getElementById("btnPrevPage" + this.id).disabled = true;
        document.getElementById("btnLastPage" + this.id).disabled = true;
        document.getElementById("btnNextPage" + this.id).disabled = true;
    }
    else {
        if (this.CurrPageNum <= 0) {
            document.getElementById("btnFirstPage" + this.id).disabled = true;
            document.getElementById("btnPrevPage" + this.id).disabled = true;
            document.getElementById("btnLastPage" + this.id).disabled = true;
            document.getElementById("btnNextPage" + this.id).disabled = true;
        }
        else if (this.CurrPageNum == 1) {
            document.getElementById("btnFirstPage" + this.id).disabled = true;
            document.getElementById("btnPrevPage" + this.id).disabled = true;
            document.getElementById("btnLastPage" + this.id).disabled = false;
            document.getElementById("btnNextPage" + this.id).disabled = false;
        }
        else if (this.CurrPageNum == this.TotalPageNum) {
            document.getElementById("btnFirstPage" + this.id).disabled = false;
            document.getElementById("btnPrevPage" + this.id).disabled = false;
            document.getElementById("btnLastPage" + this.id).disabled = true;
            document.getElementById("btnNextPage" + this.id).disabled = true;
        }
    }
};

NLDBWebPager.btnFirstPage_Click = function () {

    NLDB.ShowLoadingSpinner('#' + this.divPagerName);

    this.CurrPageNum = 1;

    this.RefreshPagerStatus();
    this.RefreshSelectResult();
};

NLDBWebPager.btnPrevPage_Click = function () {

    NLDB.ShowLoadingSpinner('#' + this.divPagerName);

    this.CurrPageNum = this.CurrPageNum - 1;
    if (this.CurrPageNum <= 0) this.CurrPageNum = 1;

    this.RefreshPagerStatus();
    this.RefreshSelectResult();
};

NLDBWebPager.btnNextPage_Click = function () {

    NLDB.ShowLoadingSpinner('#' + this.divPagerName);

    this.CurrPageNum = this.CurrPageNum + 1;
    if (this.CurrPageNum > this.TotalPageNum) this.CurrPageNum = this.TotalPageNum;

    this.RefreshPagerStatus();
    this.RefreshSelectResult();
};

NLDBWebPager.btnLastPage_Click = function () {

    NLDB.ShowLoadingSpinner('#' + this.divPagerName);

    this.CurrPageNum = this.TotalPageNum;

    this.RefreshPagerStatus();
    this.RefreshSelectResult();
};

NLDBWebPager.btnGO_Click = function () {

    var value = $("#txtPageNum" + this.id).val();
    if (typeof value == "undefined" || value == null || value == "") return;
    var pageNum = parseInt(value);
    if (pageNum <= 1 || pageNum > this.TotalPageNum) {
        alert('无效的输入值：' + value + '！'); return;
    }

    NLDB.ShowLoadingSpinner('#' + this.divPagerName);

    this.CurrPageNum = pageNum;

    this.RefreshPagerStatus();
    this.RefreshSelectResult();
};

NLDBWebPager.ddlPageSizeChanged = function () {

    var value = $("#ddlPageSize" + this.id).val();
    if (value == '10') {
        this.PageSize = 10;
    }
    else if (value == '20') {
        this.PageSize = 20;
    }
    else if (value == '40') {
        this.PageSize = 40;
    }
    else if (value == '60') {
        this.PageSize = 60;
    }
    else if (value == '80') {
        this.PageSize = 80;
    }
    else if (value == '100') {
        this.PageSize = 100;
    }
    else if (value == '500') {
        this.PageSize = 500;
    }
    else if (value == '1000') {
        this.PageSize = 1000;
    }
    else return;

    NLDB.ShowLoadingSpinner('#' + this.divPagerName);

    this.RefreshPagerStatus();
    this.RefreshSelectResult();
};

NLDBWebPager.GetDataResult = function () {

    var instance = this;

    NLDB.ShowLoadingSpinner('#' + this.divPagerName);

    this.RefreshPagerStatus();
    this.RefreshSelectResult();
};

NLDBWebPager.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    //add a column in each row for killing the process
    for (var i in data) {
        var id = data[i]["标识"];
        data[i]["操作"] = '<a href="#" onclick="NLDBWebPager' + this.id + '.ViewDataDetail(' + id + ')" class="killbutton">详情</a>';
    }

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager.RenderTable = function (targetElement, data) {

    $(targetElement).html(this.GetTableHTML(data));
};

NLDBWebPager.GetTableHTML = function (data) {
    if (typeof data == "undefined" || data == null || data.length == 0) {
        return "No results found";
    }

    var html = "";
    html += '<table class="grid" border="0" cellspacing="0" cellpadding="0">';
    html += '<tr valign="top"><td>';
    html += '<table class="gridview" border="0" width="100%">';

    // headers
    html += '<tr valign="top" class="gridHeaderRow">';
    for (var name in data[0]) {
        html += '   <th align="center">' + name + '</th>';
    }
    html += '</tr>';

    // data rows
    for (var i in data) {
        var even = '';
        if ((parseInt(i) + 1) % 2 == 0) even = ' class="even"';
        html += '<tr valign="center" ' + even + '>';
        for (var name in data[i]) {
            var value = data[i][name];
            html += '   <td align="right">' + value + '</td>';
        }
        html += '</tr>';
    }
    html += '</table>';
    html += '</td></tr>';
    html += '</table>';

    return html;
};

NLDBWebPager.GetPagerHTML = function () {

    var html = "";
    html += '<table class="gridview"><tr><td>' +
        '总共有<label id="lblTotalCount' + this.id + '">' + this.DataCount + '<label>条记录&nbsp; &nbsp; &nbsp' +
        '每页显示<select id="ddlPageSize' + this.id + '" onchange="NLDBWebPager' + this.id + '.ddlPageSizeChanged();">' +
        '<option value="10" ' + (this.PageSize == 10 ? 'selected="true"' : '') + '>10</option>' +
        '<option value="20" ' + (this.PageSize == 20 ? 'selected="true"' : '') + '>20</option>' +
        '<option value="40" ' + (this.PageSize == 40 ? 'selected="true"' : '') + '>40</option>' +
        '<option value="60" ' + (this.PageSize == 60 ? 'selected="true"' : '') + '>60</option>' +
        '<option value="80" ' + (this.PageSize == 80 ? 'selected="true"' : '') + '>80</option>' +
        '<option value="100" ' + (this.PageSize == 100 ? 'selected="true"' : '') + '>100</option>' +
        '<option value="500" ' + (this.PageSize == 500 ? 'selected="true"' : '') + '>500</option>' +
        '<option value="1000" ' + (this.PageSize == 1000 ? 'selected="true"' : '') + '>1000</option>' +
        '</select>条记录' +
        '</td>' +
        '<td>' +
        '<label id="label7' + this.id + '">当前页码为：</label>' +
        '[' +
        '<label id="lblCurrPage' + this.id + '">' + this.CurrPageNum + '</label>' +
        ']&nbsp;' +
        '<label id="label6' + this.id + '">总页码为：</label>' +
        '[' +
        '<label id="lblTotalPage' + this.id + '">' + this.TotalPageNum + '</label>' +
        ']&nbsp;' +
        '<button id="btnFirstPage' + this.id + '" onclick="NLDBWebPager' + this.id + '.btnFirstPage_Click();">第一页</button>&nbsp;' +
        '<button id="btnPrevPage' + this.id + '" onclick="NLDBWebPager' + this.id + '.btnPrevPage_Click();">上一页</button>&nbsp;' +
        '<button id="btnNextPage' + this.id + '" onclick="NLDBWebPager' + this.id + '.btnNextPage_Click();">下一页</button>&nbsp;' +
        '<button id="btnLastPage' + this.id + '" onclick="NLDBWebPager' + this.id + '.btnLastPage_Click();">最后一页</button>' +
        '&nbsp; &nbsp; 转到第<input id="txtPageNum' + this.id + '" size="10"></input>页' +
        '<button id="btnGO" onclick="NLDBWebPager' + this.id + '.btnGO_Click();">GO</button>' +
        '</td></tr></table>';
    return html;
};
