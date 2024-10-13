////////////////////////////////////
//
// Text Pool Details
//
////////////////////////////////////

updateTextPoolControls = function (id, data) {

    $("#txtTID" + id).val(data[0]["tid"]);
    $("#txtLength" + id).val(data[0]["length"]);
    $("#txtParsed" + id).val(data[0]["parsed"]);
    $("#txtResult" + id).val(data[0]["result"]);
    $("#txtContent" + id).val(data[0]["content"]);
};

updateTextPoolDetail = function (id, tid) {

    NLDB.QueryBasicController("GetTextPoolByID",
        { 'tid': tid }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetTextPoolByID：没能获得详细信息！');
            else
                updateTextPoolControls(id, data);
        });
};

btnTextPoolQuery_Click = function (id) {

    var tid = $("#txtTID" + id).val();
    if (tid == null || typeof tid.length == "undefined" || tid.length <= 0)
        return;

    tid = parseInt(tid);
    if (tid == null) return;

    updateTextPoolDetail(id, tid);
};

btnTextPoolPrevItem_Click = function (id) {

    var tid = $("#txtTID" + id).val();
    if (tid == null || typeof tid.length == "undefined" || tid.length <= 0)
        return;

    updateTextPoolDetail(id, parseInt(tid) - 1);
};

btnTextPoolNextItem_Click = function (id) {

    var tid = $("#txtTID" + id).val();
    if (tid == null || typeof tid.length == "undefined" || tid.length <= 0)
        return;

    updateTextPoolDetail(id, parseInt(tid) + 1);
};

btnTextPoolRandomItem_Click = function (id) {

    NLDB.QueryBasicController("GetTextPoolRandomly",
        null, function (data) {
            if (data == null || data.length <= 0)
                alert('GetTextPoolRandomly：没能获得详细信息！');
            else
                updateTextPoolControls(id, data);
        });
};

btnTextPoolClearItem_Click = function (id) {

    var tid = $("#txtTID" + id).val();
    if (tid == null || typeof tid.length == "undefined" || tid.length <= 0)
        return;

    NLDB.QueryBasicController("ClearTextPoolByID",
        { 'tid': tid }, function (data) {
            if (data == null || data.length <= 0)
                alert('ClearTextPoolByID：没能获得详细信息！');
            else
                updateTextPoolControls(id, data);
        });
};

btnExecuteItem3_Click = function () {

    NLDB.ShowLoadingSpinner('#divCutSentence');

    var content = $("#txtContent3").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("CutContentIntoSentences",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('CutContentIntoSentences：没能获得解析结果！');
            else
                NLDB.RenderTable('#divCutSentence', data);
        });
};

////////////////////////////////////
//
// Dictionary Detail
//
////////////////////////////////////

updateDictionaryControls = function (id, data) {

    $("#txtDID" + id).val(data[0]["did"]);
    $("#txtCID" + id).val(data[0]["cid"]);
    $("#txtLength" + id).val(data[0]["length"]);
    $("#txtContent" + id).val(data[0]["content"]);
    $("#txtEnable" + id).val(data[0]["enable"]);
    $("#txtCount" + id).val(data[0]["count"]);
    $("#ddlClassification" + id).val(data[0]["classification"]);
    $("#txtRemark" + id).val(data[0]["remark"]);

    if (data[0]["cid"] > 0) {
        document.getElementById("btnInsert" + id).disabled = true;
    }
    else {
        document.getElementById("btnInsert" + id).disabled = false;
    }
};

updateDictionaryDetail = function (id, did) {

    NLDB.QueryBasicController("GetDictionaryByID",
        { 'did': did }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetDictionaryByID：没能获得详细信息！');
            else
                updateDictionaryControls(id, data);
        });
};

btnDictionaryQuery_Click = function (id) {

    var did = $("#txtDID" + id).val();
    if (did == null || typeof did.length == "undefined" || did.length <= 0)
        return;

    did = parseInt(did);
    if (did == null) return;

    updateDictionaryDetail(id, did);
};

btnDictionaryPrevItem_Click = function (id) {

    var did = $("#txtDID" + id).val();
    if (did == null || typeof did.length == "undefined" || did.length <= 0)
        return;

    updateDictionaryDetail(id, parseInt(did) - 1);
};

btnDictionaryNextItem_Click = function (id) {

    var did = $("#txtDID" + id).val();
    if (did == null || typeof did.length == "undefined" || did.length <= 0)
        return;

    updateDictionaryDetail(id, parseInt(did) + 1);
};

btnDictionaryRandomItem_Click = function (id) {

    NLDB.QueryBasicController("GetDictionaryRandomly",
        null, function (data) {
            if (data == null || data.length <= 0)
                alert('GetDictionaryRandomly：没能获得详细信息！');
            else
                updateDictionaryControls(id, data);
        });
};

btnDictionaryInsertItem_Click = function (id) {

    var did = $("#txtDID" + id).val();
    if (did == null || typeof did.length == "undefined" || did.length <= 0)
        return;

    if (confirm("确认增加词条（did：" + did + "）？")) {

        NLDB.QueryBasicController("InsertDictionaryByID",
            { 'did': did }, function (data) {
                if (data == null || data.length <= 0)
                    alert('InsertDictionaryByID：没能获得详细信息！');
                else {
                    NLDBWebPager2.GetDataResult();
                    updateDictionaryControls(NLDBWebPager2.id, data);
                }
            });
    }
}

btnDictionarySwitch_Click = function (id) {

    var did = $("#txtDID" + id).val();
    if (did == null || typeof did.length == "undefined" || did.length <= 0)
        return;

    if (confirm("确认切换词条（did：" + did + "）？")) {

        NLDB.QueryBasicController("SwitchDictionaryByID",
            { 'did': did }, function (data) {
                if (data == null || data.length <= 0)
                    alert('SwitchDictionaryByID：没能获得详细信息！');
                else {
                    NLDBWebPager2.GetDataResult();
                    updateDictionaryControls(id, data);
                }
            });
    }
};

btnDictionaryFMMAll_Click = function (id) {

    var did = $("#txtDID" + id).val();
    if (did == null || typeof did.length == "undefined" || did.length <= 0)
        return;

    NLDB.QueryBasicController("FMMAllDictionaryByID",
        { 'did': did }, function (data) {
            if (data == null || data.length <= 0)
                alert('FMMAllDictionaryByID：没能获得详细信息！');
            else
                NLDB.RenderTable('#divParseDictionary', data);
        });
}

btnDictionaryBMMAll_Click = function (id) {

    var did = $("#txtDID" + id).val();
    if (did == null || typeof did.length == "undefined" || did.length <= 0)
        return;

    NLDB.QueryBasicController("BMMAllDictionaryByID",
        { 'did': did }, function (data) {
            if (data == null || data.length <= 0)
                alert('BMMAllDictionaryByID：没能获得详细信息！');
            else
                NLDB.RenderTable('#divParseDictionary', data);
        });
}

////////////////////////////////////
//
// Web Pagers
//
////////////////////////////////////

var NLDBWebPager1 = Object.create(NLDBWebPager);
var NLDBWebPager2 = Object.create(NLDBWebPager);
var NLDBWebPager5 = Object.create(NLDBWebPager);
var NLDBWebPager6 = Object.create(NLDBWebPager);

NLDBWebPager1.ViewDataDetail = function (id) {

    NLDB.QueryBasicController("GetTextPoolByID",
        { 'tid': id }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetTextPoolByID：没能获得详细信息！');
            else
                updateTextPoolControls(NLDBWebPager1.id,data);
        });
};

////////////////////////////////////
//
// Web Pager 2
//
////////////////////////////////////

NLDBWebPager2.ViewDataDetail = function (id) {

    NLDB.QueryBasicController("GetDictionaryByID",
        { 'did': id }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetDictionaryByID：没能获得详细信息！');
            else
                updateDictionaryControls(NLDBWebPager2.id,data);
        });
};

NLDBWebPager2.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    for (var i in data) {
        var op = '';
        var id = data[i]["标识"];
        if (data[i]["内容标识"] <= 0) {
            op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.InsertDictionaryItem(' + id + ')" class="killbutton">增加</a>&nbsp;';
        }
        op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.SwitchDictionaryItem(' + id + ')" class="killbutton">切换</a>&nbsp;';
        op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.ViewDataDetail(' + id + ')" class="killbutton">详情</a>';
        data[i]["操作"] = op;
    }

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager2.InsertDictionaryItem = function (did) {

    if (confirm("确认增加词条（did：" + did + "）？")) {

        NLDB.QueryBasicController("InsertDictionaryByID",
            { 'did': did }, function (data) {
                if (data == null || data.length <= 0)
                    alert('InsertDictionaryByID：没能获得详细信息！');
                else {
                    NLDBWebPager2.GetDataResult();
                    updateDictionaryControls(NLDBWebPager2.id, data);
                }
            });
    }
};

NLDBWebPager2.SwitchDictionaryItem = function(did) {

    if (confirm("确认切换词条（did：" + did + "）？")) {

        NLDB.QueryBasicController("SwitchDictionaryByID",
            { 'did': did }, function (data) {
                if (data == null || data.length <= 0)
                    alert('SwitchDictionaryByID：没能获得详细信息！');
                else {
                    NLDBWebPager2.GetDataResult();
                    updateDictionaryControls(NLDBWebPager2.id, data);
                }
            });
    }
};

////////////////////////////////////
//
// Web Pager 5
//
////////////////////////////////////

NLDBWebPager5.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    for (var i in data) {

        data[i]["选择"] = '<input name="cbContent5" type="checkbox" value="' + data[i]["内容"] + '" onclick="cbCombinateContent5();"/>';

        var op = '';
        var content = data[i]["内容"];
        if (data[i]["内容标识"] <= 0) {
            op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.InsertContentValue(\'' + content + '\')" class="killbutton">增加</a>&nbsp;';
        }
        op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.DeleteContentValue(\'' + content + '\')" class="killbutton">删除</a>&nbsp;';
        data[i]["操作"] = op;
    }

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager5.InsertContentValue = function (content) {

    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认增加内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentInsertValue",
            formData, function (data) {

            NLDBWebPager5.GetDataResult();
        });
    }
};

NLDBWebPager5.DeleteContentValue = function (content) {

    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认删除内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentDeleteValue",
            formData, function (data) {

                NLDBWebPager5.GetDataResult();
            });
    }
};

cbCombinateContent5 = function (op) {

    var cbContents = document.getElementsByName("cbContent5");

    var content = '';
    for (var i = 0; i < cbContents.length; i++) {
        if (cbContents[i].checked == true) {
            content += cbContents[i].value + ';';
        }
    }

    //Update content.
    $("#txtContent5").val(content);
};

btnDictionaryRemoveItems_Click = function (id) {

    var content = $("#txtContent5").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认批量删除内容（content：" + content + "）？")) {

        var values = content.split(";");
        for (var i = 0; i < values.length - 1; i++) {

            if (values[i].length > 0) {

                var formData = new FormData();
                formData.append('content', values[i]);

                NLDB.PostBasicController("ContentDeleteValue",
                    formData, function (data) { });

            }
        }
        if (values.length > 1) NLDBWebPager5.GetDataResult();
    }
};

btnDictionaryInsertItems_Click = function (id) {

    var content = $("#txtContent5").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认批量增加内容（content：" + content + "）？")) {

        var values = content.split(";");
        for (var i = 0; i < values.length - 1; i++) {

            if (values[i].length > 0) {

                var formData = new FormData();
                formData.append('content', values[i]);

                NLDB.PostBasicController("ContentInsertValue",
                    formData, function (data) { });
            }
        }
        if (values.length > 1) NLDBWebPager5.GetDataResult();
    }
};

////////////////////////////////////
//
// Web Pager 6
//
////////////////////////////////////

NLDBWebPager6.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    for (var i in data) {

        data[i]["语料原文"] = data[i]["语料原文"].replace(data[i]["内容"], '<mark>' + data[i]["内容"] + '</mark>');

        data[i]["内容"] = data[i]["内容"] + '&nbsp;<input name="cbContent6" type="checkbox" value="' + data[i]["内容"] + '" onclick="cbCombinateContent6();"/>';
        data[i]["FMM内容"] = data[i]["FMM内容"] + '&nbsp;<input name="cbContent6" type="checkbox" value="' + data[i]["FMM内容"] + '" onclick="cbCombinateContent6();"/>';
        data[i]["BMM内容"] = data[i]["BMM内容"] + '&nbsp;<input name="cbContent6" type="checkbox" value="' + data[i]["BMM内容"] + '" onclick="cbCombinateContent6();"/>';
        //data[i]["操作"] = '<a href="#" onclick="NLDBWebPager' + this.id + '.DeleteAmbiguityByID(\'' + data[i]["标识"] + '\')" class="killbutton">删除</a>';
        data[i]["操作"] = '<select id="ddlOperation' + data[i]["标识"] + '" onchange="ddlOnOperationChange6(\'' +  data[i]["标识"] + '\');">' +
            '<option value="0"' + (data[i]["操作"] == 0 ? ' selected="true"' : '') + '>待定</option>' +
            '<option value="1"' + (data[i]["操作"] == 1 ? ' selected="true"' : '') + '>合并解析</option>' +
            '<option value="2"' + (data[i]["操作"] == 2 ? ' selected="true"' : '') + '>选FMM解析</option>' +
            '<option value="3"' + (data[i]["操作"] == 3 ? ' selected="true"' : '') + '>选BMM解析</option>' +
            '<option value="4"' + (data[i]["操作"] == 4 ? ' selected="true"' : '') + '>错误的解析</option>' +
            '<option value="5">删除</option>' +
            '</select>';
    }

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

cbCombinateContent6 = function (op) {

    var cbContents = document.getElementsByName("cbContent6");

    var content = '';
    for (var i = 0; i < cbContents.length; i++) {
        if (cbContents[i].checked == true) {
            content += cbContents[i].value + ';';
        }
    }

    //Update content.
    $("#txtContent6").val(content);
};

ddlOnOperationChange6 = function (id) {

    var value = $("#ddlOperation" + id).val();
    if (value == null || typeof value.length == "undefined" || value.length <= 0)
        return;

    if (id == null || typeof id.length == "undefined" || id.length <= 0)
        return;

    if (value >= 0 && value <= 4) {

        NLDB.QueryBasicController("UpdateAmbiguityOperationByID",
            { 'id': id, 'operation' : value }, function (data) {
                NLDBWebPager6.GetDataResult();
            });
    }
    else if (value == 5) {

        NLDB.QueryBasicController("DeleteAmbiguityByID",
            { 'id': id }, function (data) {
                NLDBWebPager6.GetDataResult();
            });
    }
};

NLDBWebPager6.DeleteAmbiguityByID = function (id) {

    if (id == null || typeof id.length == "undefined" || id.length <= 0)
        return;

    NLDB.QueryBasicController("DeleteAmbiguityByID",
        { 'id': id }, function (data) {
            NLDBWebPager6.GetDataResult();
        });
};

btnAmbiguityRemoveItems_Click = function (id) {

    var content = $("#txtContent6").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认批量删除内容（content：" + content + "）？")) {

        var values = content.split(";");
        for (var i = 0; i < values.length - 1; i++) {

            if (values[i].length > 0) {

                var formData = new FormData();
                formData.append('content', values[i]);

                NLDB.PostBasicController("ContentDeleteValue",
                    formData, function (data) { });

            }
        }
        if (values.length > 1) NLDBWebPager6.GetDataResult();
    }
};

btnAmbiguityInsertItems_Click = function (id) {

    var content = $("#txtContent6").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认批量增加内容（content：" + content + "）？")) {

        var values = content.split(";");
        for (var i = 0; i < values.length - 1; i++) {

            if (values[i].length > 0) {

                var formData = new FormData();
                formData.append('content', values[i]);

                NLDB.PostBasicController("ContentInsertValue",
                    formData, function (data) { });
            }
        }
        if (values.length > 1) NLDBWebPager6.GetDataResult();
    }
};

btnAmbiguityClearItems_Click = function (id) {

    NLDB.QueryBasicController("ClearAmbiguity",
        null, function (data) {
            NLDBWebPager6.GetDataResult();
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
        case 3:
            break;
        case 4:
            NLDBWebPager5.GetDataResult();
            break;
        case 5:
            NLDBWebPager6.GetDataResult();
            break;
        default:
            alert("unknown tab index!");
            break;
    }
};

$(function () {

    //Controller
    NLDB.pathName = "Corpus";

    //Pager
    NLDBWebPager1.DataCount = 0;
    NLDBWebPager1.CurrPageNum = 1;
    NLDBWebPager1.TotalPageNum = 1;
    NLDBWebPager1.PageSize = 20;
    NLDBWebPager1.id = "1";
    NLDBWebPager1.divDataName = 'divTextPool';
    NLDBWebPager1.divPagerName = "divWebPager1";
    NLDBWebPager1.selectAction = 'SelectTextPool';
    NLDBWebPager1.countAction = 'GetTextPoolCount';

    NLDBWebPager1.RefreshPagerStatus();

    //Pager
    NLDBWebPager2.DataCount = 0;
    NLDBWebPager2.CurrPageNum = 1;
    NLDBWebPager2.TotalPageNum = 1;
    NLDBWebPager2.PageSize = 20;
    NLDBWebPager2.id = "2";
    NLDBWebPager2.divDataName = 'divDictionary';
    NLDBWebPager2.divPagerName = "divWebPager2";
    NLDBWebPager2.selectAction = 'SelectDictionary';
    NLDBWebPager2.countAction = 'GetDictionaryCount';

    NLDBWebPager2.RefreshPagerStatus();

    //Pager
    NLDBWebPager5.DataCount = 0;
    NLDBWebPager5.CurrPageNum = 1;
    NLDBWebPager5.TotalPageNum = 1;
    NLDBWebPager5.PageSize = 40;
    NLDBWebPager5.id = "5";
    NLDBWebPager5.divDataName = 'divClearDictionary';
    NLDBWebPager5.divPagerName = "divWebPager5";
    NLDBWebPager5.selectAction = 'SelectClearableDictionary';
    NLDBWebPager5.countAction = 'GetClearableDictionaryCount';

    NLDBWebPager5.RefreshPagerStatus();

    //Pager
    NLDBWebPager6.DataCount = 0;
    NLDBWebPager6.CurrPageNum = 1;
    NLDBWebPager6.TotalPageNum = 1;
    NLDBWebPager6.PageSize = 40;
    NLDBWebPager6.id = "6";
    NLDBWebPager6.divDataName = 'divClearAmbiguity';
    NLDBWebPager6.divPagerName = "divWebPager6";
    NLDBWebPager6.selectAction = 'SelectAmbiguity';
    NLDBWebPager6.countAction = 'GetAmbiguityCount';

    NLDBWebPager5.RefreshPagerStatus();

    $("#tabs").tabs({
        'select': function (event, ui) { RefreshCurrentlySelectedTab(ui.index); }
    });

    NLDBWebPager1.GetDataResult();
    //NLDBWebPager2.GetDataResult();
    //NLDBWebPager5.GetDataResult();
    //NLDBWebPager6.GetDataResult();

    NLDB.errorDialog = $("#errorDialog").dialog({ autoOpen: false, title: '遇到一个错误', width: 400, height: 300 });
});
