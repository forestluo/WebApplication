////////////////////////////////////
//
// Inner Content Detail
//
////////////////////////////////////

updateInnerContentControls = function (data) {

    $("#txtCID1").val(data[0]["cid"]);
    $("#txtLength1").val(data[0]["length"]);
    $("#txtContent1").val(data[0]["content"]);
    $("#txtType1").val(data[0]["type"]);
    $("#txtCount1").val(data[0]["count"]);
    $("#ddlClassification1").val(data[0]["classification"]);
    $("#txtAttribute1").val(data[0]["attribute"]);
};

updateInnerContentDetail = function (cid) {

    NLDB.QueryBasicController("GetInnerContentByID",
        { 'cid': cid }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetInnerContentByID：没能获得详细信息！');
            else
                updateInnerContentControls(data);
        });
};

btnQuery1_Click = function () {

    var cid = $("#txtCID1").val();
    if (cid == null || typeof cid.length == "undefined" || cid.length <= 0)
        return;

    cid = parseInt(cid);
    if (cid == null) return;

    updateInnerContentDetail(cid);
};

btnPrevItem1_Click = function () {

    var cid = $("#txtCID1").val();
    if (cid == null || typeof cid.length == "undefined" || cid.length <= 0)
        return;

    updateInnerContentDetail(parseInt(cid) - 1);
};

btnNextItem1_Click = function () {

    var cid = $("#txtCID1").val();
    if (cid == null || typeof cid.length == "undefined" || cid.length <= 0)
        return;

    updateInnerContentDetail(parseInt(cid) + 1);
};

btnRandomItem1_Click = function () {

    NLDB.QueryBasicController("GetInnerContentRandomly",
        null, function (data) {
            if (data == null || data.length <= 0)
                alert('GetInnerContentRandomly：没能获得详细信息！');
            else
                updateInnerContentControls(data);
        });
};

////////////////////////////////////
//
// Outer Content Detail
//
////////////////////////////////////

updateOuterContentControls = function (data) {

    $("#txtOID2").val(data[0]["oid"]);
    $("#txtCID2").val(data[0]["cid"]);
    $("#txtLength2").val(data[0]["length"]);
    $("#txtContent2").val(data[0]["content"]);
    $("#txtType2").val(data[0]["type"]);
    $("#txtCount2").val(data[0]["count"]);
    $("#ddlClassification2").val(data[0]["classification"]);
    $("#txtRule2").val(data[0]["rule"]);
};

updateOuterContentDetail = function (oid) {

    NLDB.QueryBasicController("GetOuterContentByID",
        { 'oid': oid }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetOuterContentByID：没能获得详细信息！');
            else
                updateOuterContentControls(data);
        });
};

btnQuery2_Click = function () {

    var oid = $("#txtOID2").val();
    if (oid == null || typeof oid.length == "undefined" || oid.length <= 0)
        return;

    oid = parseInt(oid);
    if (oid == null) return;

    updateOuterContentDetail(oid);
};

btnPrevItem2_Click = function () {

    var oid = $("#txtOID2").val();
    if (oid == null || typeof oid.length == "undefined" || oid.length <= 0)
        return;

    updateOuterContentDetail(parseInt(oid) - 1);
};

btnNextItem2_Click = function () {

    var oid = $("#txtOID2").val();
    if (oid == null || typeof oid.length == "undefined" || oid.length <= 0)
        return;

    updateOuterContentDetail(parseInt(oid) + 1);
};

btnRandomItem2_Click = function () {

    NLDB.QueryBasicController("GetOuterContentRandomly",
        null, function (data) {
            if (data == null || data.length <= 0)
                alert('GetOuterContentRandomly：没能获得详细信息！');
            else
                updateOuterContentControls(data);
        });
};

////////////////////////////////////
//
// External Content Detail
//
////////////////////////////////////

updateExternalContentControls = function (id, data) {

    $("#txtEID" + id).val(data[0]["eid"]);
    $("#txtLength" + id).val(data[0]["length"]);
    $("#txtContent" + id).val(data[0]["content"]);
    $("#txtType" + id).val(data[0]["type"]);
    $("#txtTID" + id).val(data[0]["tid"]);
    $("#ddlClassification" + id).val(data[0]["classification"]);
    $("#txtRule" + id).val(data[0]["rule"]);
};

updateExternalContentDetail = function (id, eid) {

    NLDB.QueryBasicController("GetExternalContentByID",
        { 'eid': eid }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetExternalContentByID：没能获得详细信息！');
            else
                updateExternalContentControls(id, data);
        });
};

btnExternalQuery_Click = function (id) {

    var eid = $("#txtEID" + id).val();
    if (eid == null || typeof eid.length == "undefined" || eid.length <= 0)
        return;

    eid = parseInt(eid);
    if (eid == null) return;

    updateExternalContentDetail(id, eid);
};

btnExternalPrevItem_Click = function (id) {

    var eid = $("#txtEID" + id).val();
    if (eid == null || typeof eid.length == "undefined" || eid.length <= 0)
        return;

    updateExternalContentDetail(id, parseInt(eid) - 1);
};

btnExternalNextItem_Click = function (id) {

    var eid = $("#txtEID" + id).val();
    if (eid == null || typeof eid.length == "undefined" || eid.length <= 0)
        return;

    updateExternalContentDetail(id, parseInt(eid) + 1);
};

btnExternalRandomItem_Click = function (id) {

    NLDB.QueryBasicController("GetExternalContentRandomly",
        null, function (data) {
            if (data == null || data.length <= 0)
                alert('GetExternalContentRandomly：没能获得详细信息！');
            else
                updateExternalContentControls(id, data);
        });
};

btnSplitExternalContent_Click = function () {

    NLDB.ShowLoadingSpinner('#divSplitExternalContent');

    var eid = $("#txtEID4").val();
    if (eid == null || typeof eid.length == "undefined" || eid.length <= 0)
        return;

    NLDB.QueryBasicController("SplitExternalContentByID",
        { 'eid': eid }, function (data) {
            if (data == null || data.length <= 0)
                alert('SplitExternalContentByID：没能获得解析结果！');
            else
                NLDB.RenderTable('#divSplitExternalContent', data);
        });

}

cbCombinateContent = function (op) {

    var cbContents = document.getElementsByName("cbContent");

    var content = '';
    for (var i = 0; i < cbContents.length; i++) {
        if (cbContents[i].checked == true) {
            content += cbContents[i].value;
        }
    }

    //Update content.
    $("#txtCombinated4").val(content);
    $("#btnCombinated4").attr('name', op);
};

btnFMMSplitExternalContent_Click = function () {

    NLDB.ShowLoadingSpinner('#divSplitExternalContent');

    var eid = $("#txtEID4").val();
    if (eid == null || typeof eid.length == "undefined" || eid.length <= 0)
        return;

    NLDB.QueryBasicController("FMMSplitExternalContentByID",
        { 'eid': eid }, function (data) {
            if (data == null || data.length <= 0)
                alert('FMMSplitExternalContentByID：没能获得解析结果！');
            else {

                for (var i in data) {

                    if (data[i]["节点类"] == 'rule') {
                        data[i]["选择"] = '';
                    }
                    else {
                        data[i]["选择"] = '<input name="cbContent" type="checkbox" value="' + data[i]["内容"] + '" onclick="cbCombinateContent(\'fmm\');"/>';
                    }

                    var op = '';
                    if (data[i]["节点类"] == "var") {
                        var value = data[i]["内容"];
                        if (data[i]["终结符"] == 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentSetTerminator(\'fmm\', \'' + value + '\')" class="killbutton">终结</a>';
                        }
                        if (data[i]["内容标识"] == 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentInsertValue(\'fmm\', \'' + value + '\')" class="killbutton">增加</a>';
                        }
                        op += '&nbsp;<a href="#" onclick="NLDBContentDeleteValue(\'fmm\',\'' + value + '\')" class="killbutton">删除</a>';
                    }
                    data[i]["操作"] = op;
                }

                NLDB.RenderTable('#divSplitExternalContent', data);

                $("a.killbutton").button();
            }
        });
};

btnBMMSplitExternalContent_Click = function () {

    NLDB.ShowLoadingSpinner('#divSplitExternalContent');

    var eid = $("#txtEID4").val();
    if (eid == null || typeof eid.length == "undefined" || eid.length <= 0)
        return;

    NLDB.QueryBasicController("BMMSplitExternalContentByID",
        { 'eid': eid }, function (data) {
            if (data == null || data.length <= 0)
                alert('BMMSplitExternalContentByID：没能获得解析结果！');
            else {

                for (var i in data) {

                    data[i]["选择"] = '<input name="cbContent" type="checkbox" value="' + data[i]["内容"] + '" onclick="cbCombinateContent(\'bmm\');"/>';

                    var op = "";
                    if (data[i]["节点类"] == "var") {
                        var value = data[i]["内容"];
                        if (data[i]["终结符"] == 0) {
                            op += '<a href="#" onclick="NLDBContentSetTerminator(\'bmm\', \'' + value + '\')" class="killbutton">终结</a>';
                        }
                        if (data[i]["内容标识"] == 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentInsertValue(\'bmm\', \'' + value + '\')" class="killbutton">增加</a>';
                        }
                        op += '&nbsp;<a href="#" onclick="NLDBContentDeleteValue(\'bmm\',\'' + value + '\')" class="killbutton">删除</a>';
                    }
                    data[i]["操作"] = op;
                }

                NLDB.RenderTable('#divSplitExternalContent', data);

                $("a.killbutton").button();
            }
        });
};

NLDBContentSetTerminator = function (op, content) {

    if (op == null || typeof op.length == "undefined" || op.length <= 0)
        return;
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认终结内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentSetTerminator",
            formData, function (data) {

                if (op == 'fmm') btnFMMSplitExternalContent_Click();
                else if (op == 'bmm') btnBMMSplitExternalContent_Click();
            });
    }
}

NLDBContentInsertValue = function (op, content) {

    if (op == null || typeof op.length == "undefined" || op.length <= 0)
        return;
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认增加内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentInsertValue",
            formData, function (data) {

                if (op == 'fmm') btnFMMSplitExternalContent_Click();
                else if (op == 'bmm') btnBMMSplitExternalContent_Click();
            });
    }
}

NLDBContentDeleteValue = function (op, content) {

    if (op == null || typeof op.length == "undefined" || op.length <= 0)
        return;
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认删除内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentDeleteValue",
            formData, function (data) {

                if (op == 'fmm') btnFMMSplitExternalContent_Click();
                else if (op == 'bmm') btnBMMSplitExternalContent_Click();
            });
    }
}

btnExternalCombinate_Click = function (id) {

    var op = $("#btnCombinated" + id).attr('name');
    if (op == null || typeof op.length == "undefined" || op.length <= 0)
        return;

    var content = $("#txtCombinated" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认增加内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentInsertValue",
            formData, function (data) {

                if (op == 'fmm') btnFMMSplitExternalContent_Click();
                else if (op == 'bmm') btnBMMSplitExternalContent_Click();
            });

    }
};

////////////////////////////////////
//
// Web Pagers
//
////////////////////////////////////

var NLDBWebPager1 = Object.create(NLDBWebPager);
var NLDBWebPager2 = Object.create(NLDBWebPager);
var NLDBWebPager3 = Object.create(NLDBWebPager);

NLDBWebPager1.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager2.ViewDataDetail = function (id) {

    NLDB.QueryBasicController("GetOuterContentByID",
        { 'oid': id }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetOuterContentByID：没能获得详细信息！');
            else
                updateOuterContentControls(data);
        });
};

NLDBWebPager3.ViewDataDetail = function (id) {

    NLDB.QueryBasicController("GetExternalContentByID",
        { 'eid': id }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetExternalContentByID：没能获得详细信息！');
            else
                updateExternalContentControls('3', data);
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
            break;
        default:
            alert("unknown tab index!");
            break;
    }
};

$(function () {

    //Controller
    NLDB.pathName = "Content";

    NLDBWebPager1.DataCount = 0;
    NLDBWebPager1.CurrPageNum = 1;
    NLDBWebPager1.TotalPageNum = 1;
    NLDBWebPager1.PageSize = 20;
    NLDBWebPager1.id = "1";
    NLDBWebPager1.divDataName = 'divInnerContent';
    NLDBWebPager1.divPagerName = "divWebPager1";
    NLDBWebPager1.selectAction = 'SelectInnerContent';
    NLDBWebPager1.countAction = 'GetInnerContentCount';

    NLDBWebPager1.RefreshPagerStatus();

    NLDBWebPager2.DataCount = 0;
    NLDBWebPager2.CurrPageNum = 1;
    NLDBWebPager2.TotalPageNum = 1;
    NLDBWebPager2.PageSize = 20;
    NLDBWebPager2.id = "2";
    NLDBWebPager2.divDataName = 'divOuterContent';
    NLDBWebPager2.divPagerName = "divWebPager2";
    NLDBWebPager2.selectAction = 'SelectOuterContent';
    NLDBWebPager2.countAction = 'GetOuterContentCount';

    NLDBWebPager2.RefreshPagerStatus();

    NLDBWebPager3.DataCount = 0;
    NLDBWebPager3.CurrPageNum = 1;
    NLDBWebPager3.TotalPageNum = 1;
    NLDBWebPager3.PageSize = 20;
    NLDBWebPager3.id = "3";
    NLDBWebPager3.divDataName = 'divExternalContent';
    NLDBWebPager3.divPagerName = "divWebPager3";
    NLDBWebPager3.selectAction = 'SelectExternalContent';
    NLDBWebPager3.countAction = 'GetExternalContentCount';

    NLDBWebPager3.RefreshPagerStatus();

    $("#tabs").tabs({
        'select': function (event, ui) { RefreshCurrentlySelectedTab(ui.index); }
    });

    NLDBWebPager1.GetDataResult();
    //NLDBWebPager2.GetDataResult();
    //NLDBWebPager3.GetDataResult();

    NLDB.errorDialog = $("#errorDialog").dialog({ autoOpen: false, title: '遇到一个错误', width: 400, height: 300 });
});
