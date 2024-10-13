////////////////////////////////////
//
// Web Pagers
//
////////////////////////////////////

var NLDBWebPager1 = Object.create(NLDBWebPager);
var NLDBWebPager2 = Object.create(NLDBWebPager);
var NLDBWebPager3 = Object.create(NLDBWebPager);
var NLDBWebPager4 = Object.create(NLDBWebPager);
var NLDBWebPager5 = Object.create(NLDBWebPager);
var NLDBWebPager6 = Object.create(NLDBWebPager);
var NLDBWebPager7 = Object.create(NLDBWebPager);

NLDBWebPager1.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager2.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager3.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager4.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager5.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager6.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager7.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

////////////////////////////////////
//
// Check Rule
//
////////////////////////////////////

btnCheckRule1_Click = function () {

    var content = $("#txtInput1").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("FilterContentByRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('FilterContentByRule：没能获得解析结果！');
            else
                $('#txtOutput1').val(data[0]["内容"]);
        });
};

btnCheckRule2_Click = function () {

    var content = $("#txtInput2").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("ParseContentByRegularRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ParseContentByRegularRule：没能获得解析结果！');
            else
                NLDB.RenderTable('#divOutput2', data);
        });
};

btnCheckRule3_Click = function () {

    var content = $("#txtInput3").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("ParseContentByNumericalRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ParseContentByNumericalRule：没能获得解析结果！');
            else
                NLDB.RenderTable('#divOutput3', data);
        });
};

btnCheckRule4_Click = function () {

    var content = $("#txtInput4").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("ParseContentByAttributeRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ParseContentByAttributeRule：没能获得解析结果！');
            else
                NLDB.RenderTable('#divOutput4', data);
        });
};

btnCheckRule5_Click = function () {

    var content = $("#txtInput5").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("ParseContentByPhraseRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ParseContentByPhraseRule：没能获得解析结果！');
            else
                NLDB.RenderTable('#divOutput5', data);
        });
};

btnCheckRule6_Click = function () {

    var content = $("#txtInput6").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("CutContentIntoSentences",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('CutContentIntoSentences：没能获得解析结果！');
            else
                NLDB.RenderTable('#divOutput6', data);
        });
};

btnCheckRule7_Click = function () {

    var content = $("#txtInput7").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("ExtractStructs",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ExtractStructs：没能获得解析结果！');
            else
                NLDB.RenderTable('#divOutput7', data);
        });
};

////////////////////////////////////
//
// Functions
//
////////////////////////////////////

RefreshCurrentlySelectedTab = function (selectedIndex) {
    if (typeof selectedIndex == "undefined")
        selectedIndex = $("#tabs").tabs('option', 'selected');
    switch (selectedIndex) {
        case 0:
            NLDBWebPager1.GetDataResult();
            break;
        case 1:
            NLDBWebPager2.GetDataResult();
            break;
        case 2:
            NLDBWebPager3.GetDataResult();
            break;
        case 3:
            NLDBWebPager4.GetDataResult();
            break;
        case 4:
            NLDBWebPager5.GetDataResult();
            break;
        case 5:
            NLDBWebPager6.GetDataResult();
            break;
        case 6:
            NLDBWebPager7.GetDataResult();
            break;
        default:
            alert("unknown tab index!");
            break;
    }
};

$(function () {

    //Controller
    NLDB.pathName = "Rule";

    NLDBWebPager1.DataCount = 0;
    NLDBWebPager1.CurrPageNum = 1;
    NLDBWebPager1.TotalPageNum = 1;
    NLDBWebPager1.PageSize = 100;
    NLDBWebPager1.id = "1";
    NLDBWebPager1.divDataName = 'divFilterRule';
    NLDBWebPager1.divPagerName = "divWebPager1";
    NLDBWebPager1.selectAction = 'SelectFilterRule';
    NLDBWebPager1.countAction = 'GetFilterRuleCount';

    NLDBWebPager1.RefreshPagerStatus();

    NLDBWebPager2.DataCount = 0;
    NLDBWebPager2.CurrPageNum = 1;
    NLDBWebPager2.TotalPageNum = 1;
    NLDBWebPager2.PageSize = 100;
    NLDBWebPager2.id = "2";
    NLDBWebPager2.divDataName = 'divRegularRule';
    NLDBWebPager2.divPagerName = "divWebPager2";
    NLDBWebPager2.selectAction = 'SelectRegularRule';
    NLDBWebPager2.countAction = 'GetRegularRuleCount';

    NLDBWebPager2.RefreshPagerStatus();

    NLDBWebPager3.DataCount = 0;
    NLDBWebPager3.CurrPageNum = 1;
    NLDBWebPager3.TotalPageNum = 1;
    NLDBWebPager3.PageSize = 100;
    NLDBWebPager3.id = "3";
    NLDBWebPager3.divDataName = 'divNumericalRule';
    NLDBWebPager3.divPagerName = "divWebPager3";
    NLDBWebPager3.selectAction = 'SelectNumericalRule';
    NLDBWebPager3.countAction = 'GetNumericalRuleCount';

    NLDBWebPager3.RefreshPagerStatus();

    NLDBWebPager4.DataCount = 0;
    NLDBWebPager4.CurrPageNum = 1;
    NLDBWebPager4.TotalPageNum = 1;
    NLDBWebPager4.PageSize = 100;
    NLDBWebPager4.id = "4";
    NLDBWebPager4.divDataName = 'divAttributeRule';
    NLDBWebPager4.divPagerName = "divWebPager4";
    NLDBWebPager4.selectAction = 'SelectAttributeRule';
    NLDBWebPager4.countAction = 'GetAttributeRuleCount';

    NLDBWebPager4.RefreshPagerStatus();

    NLDBWebPager5.DataCount = 0;
    NLDBWebPager5.CurrPageNum = 1;
    NLDBWebPager5.TotalPageNum = 1;
    NLDBWebPager5.PageSize = 100;
    NLDBWebPager5.id = "5";
    NLDBWebPager5.divDataName = 'divPhraseRule';
    NLDBWebPager5.divPagerName = "divWebPager5";
    NLDBWebPager5.selectAction = 'SelectPhraseRule';
    NLDBWebPager5.countAction = 'GetPhraseRuleCount';

    NLDBWebPager5.RefreshPagerStatus();

    NLDBWebPager6.DataCount = 0;
    NLDBWebPager6.CurrPageNum = 1;
    NLDBWebPager6.TotalPageNum = 1;
    NLDBWebPager6.PageSize = 100;
    NLDBWebPager6.id = "6";
    NLDBWebPager6.divDataName = 'divParseRule';
    NLDBWebPager6.divPagerName = "divWebPager6";
    NLDBWebPager6.selectAction = 'SelectParseRule';
    NLDBWebPager6.countAction = 'GetParseRuleCount';

    NLDBWebPager6.RefreshPagerStatus();

    NLDBWebPager7.DataCount = 0;
    NLDBWebPager7.CurrPageNum = 1;
    NLDBWebPager7.TotalPageNum = 1;
    NLDBWebPager7.PageSize = 100;
    NLDBWebPager7.id = "7";
    NLDBWebPager7.divDataName = 'divStructRule';
    NLDBWebPager7.divPagerName = "divWebPager7";
    NLDBWebPager7.selectAction = 'SelectStructRule';
    NLDBWebPager7.countAction = 'GetStructRuleCount';

    NLDBWebPager7.RefreshPagerStatus();

    $("#tabs").tabs({
        'select': function (event, ui) { RefreshCurrentlySelectedTab(ui.index); }
    });

    NLDBWebPager1.GetDataResult();
    //NLDBWebPager2.GetDataResult();
    //NLDBWebPager3.GetDataResult();
    //NLDBWebPager4.GetDataResult();
    //NLDBWebPager5.GetDataResult();
    //NLDBWebPager6.GetDataResult();
    //NLDBWebPager7.GetDataResult();

    NLDB.errorDialog = $("#errorDialog").dialog({ autoOpen: false, title: '遇到一个错误', width: 400, height: 300 });
});
