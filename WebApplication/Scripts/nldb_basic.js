////////////////////////////////////
//
// Web Pagers
//
////////////////////////////////////

var NLDBWebPager1 = Object.create(NLDBWebPager);

NLDBWebPager1.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    //add a column in each row for killing the process
    for (var i in data) {
        var eid = data[i]["标识"];
        var op = '';
        if (data[i]["消息"].indexOf("(tid=") > 0 ||
            data[i]["消息"].indexOf("(eid=") > 0) {
            op += '<a href="#" onclick="NLDBWebPager1.ResetExceptionLog(' + eid + ')" class="killbutton">重置</a>&nbsp;';
        }
        data[i]["操作"] = op + '<a href="#" onclick="NLDBWebPager1.DeleteExceptionLog(' + eid + ')" class="killbutton">删除</a>';
    }

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager1.DeleteExceptionLog = function (eid) {
    if (confirm("确认删除日志（eid： " + eid + "）？"))
    {
        NLDB.QueryBasicController("DeleteExceptionLog",
            { 'eid': eid }, function (data) {
                RefreshCurrentlySelectedTab();
            });
    }
};

NLDBWebPager1.ResetExceptionLog = function (eid) {
    if (confirm("确认重置日志（eid： " + eid + "）？")) {
        NLDB.QueryBasicController("ResetExceptionLog",
            { 'eid': eid }, function (data) {
                RefreshCurrentlySelectedTab();
            });
    }
};

GetStatisticInfo = function () {
    NLDB.ShowLoadingSpinner('#divStatisticInfo');

    NLDB.QueryBasicController("GetStatisticInfo",
        null, function (data) {
            NLDB.RenderTable('#divStatisticInfo', data);
        });
};

GetTextPoolInfo = function () {
    NLDB.ShowLoadingSpinner('#divTextPoolInfo');

    NLDB.QueryBasicController("GetTextPoolInfo",
        null, function (data) {
            NLDB.RenderTable('#divTextPoolInfo', data);
    });

    NLDB.QueryBasicController("GetTextPoolParsedInfo",
        null, function (data) {
            NLDB.RenderTable('#divTextPoolParsedInfo', data);
        });

    NLDB.QueryBasicController("GetTextPoolLengthInfo",
        null, function (data) {
            NLDB.RenderTable('#divTextPoolLengthInfo', data);
        });
};

GetDictionaryInfo = function () {
    NLDB.ShowLoadingSpinner('#divDictionaryInfo');

    NLDB.QueryBasicController("GetDictionaryInfo",
        null, function (data) {
            NLDB.RenderTable('#divDictionaryInfo', data);
        });

    NLDB.QueryBasicController("GetDictionaryEnabledInfo",
        null, function (data) {
            NLDB.RenderTable('#divDictionaryEnabledInfo', data);
        });

    NLDB.QueryBasicController("GetDictionaryCountInfo",
        null, function (data) {
            NLDB.RenderTable('#divDictionaryCountInfo', data);
        });

    NLDB.QueryBasicController("GetDictionaryLengthInfo",
        null, function (data) {
            NLDB.RenderTable('#divDictionaryLengthInfo', data);
        });

    NLDB.QueryBasicController("GetDictionaryClassificationInfo",
        null, function (data) {
            NLDB.RenderTable('#divDictionaryClassificationInfo', data);
        });
};

GetRuleInfo = function () {
    NLDB.ShowLoadingSpinner('#divRuleInfo');

    NLDB.QueryBasicController("GetFilterRuleInfo",
        null, function (data) {
            NLDB.RenderTable('#divFilterRuleInfo', data);
        });

    NLDB.QueryBasicController("GetRegularRuleInfo",
        null, function (data) {
            NLDB.RenderTable('#divRegularRuleInfo', data);
        });

    NLDB.QueryBasicController("GetNumericalRuleInfo",
        null, function (data) {
            NLDB.RenderTable('#divNumericalRuleInfo', data);
        });

    NLDB.QueryBasicController("GetAttributeRuleInfo",
        null, function (data) {
            NLDB.RenderTable('#divAttributeRuleInfo', data);
        });

    NLDB.QueryBasicController("GetPhraseRuleInfo",
        null, function (data) {
            NLDB.RenderTable('#divPhraseRuleInfo', data);
        });

    NLDB.QueryBasicController("GetParseRuleInfo",
        null, function (data) {
            NLDB.RenderTable('#divParseRuleInfo', data);
        });
}

GetContentInfo = function () {
    NLDB.ShowLoadingSpinner('#divContentInfo');

    NLDB.QueryBasicController("GetInnerContentInfo",
        null, function (data) {
            NLDB.RenderTable('#divInnerContentInfo', data);
        });

    NLDB.QueryBasicController("GetOuterContentInfo",
        null, function (data) {
            NLDB.RenderTable('#divOuterContentInfo', data);
        });

    NLDB.QueryBasicController("GetExternalContentInfo",
        null, function (data) {
            NLDB.RenderTable('#divExternalContentInfo', data);
        });
}

RefreshCurrentlySelectedTab = function (selectedIndex) {
    if (typeof selectedIndex == "undefined")
        selectedIndex = $("#tabs").tabs('option', 'selected');
    switch (selectedIndex) {
        case 0:
            NLDBWebPager1.GetDataResult();
            break;
        case 1:
            GetStatisticInfo();
            break;
        case 2:
            GetTextPoolInfo();
            break;
        case 3:
            GetDictionaryInfo();
            break;
        case 4:
            GetRuleInfo();
            break;
        case 5:
            GetContentInfo();
            break;
        default:
            alert("unknown tab index!");
            break;
    }
};

$(function () {

    //Controller
    NLDB.pathName = "Basic";

    NLDBWebPager1.DataCount = 0;
    NLDBWebPager1.CurrPageNum = 1;
    NLDBWebPager1.TotalPageNum = 1;
    NLDBWebPager1.PageSize = 20;
    NLDBWebPager1.id = "1";
    NLDBWebPager1.divDataName = 'divExceptionLog';
    NLDBWebPager1.divPagerName = "divWebPager1";
    NLDBWebPager1.selectAction = 'SelectExceptionLog';
    NLDBWebPager1.countAction = 'GetExceptionLogCount';

    NLDBWebPager1.RefreshPagerStatus();

    $("#tabs").tabs({
        'select': function (event, ui) { RefreshCurrentlySelectedTab(ui.index); }
    });

    NLDBWebPager1.GetDataResult();
    //GetStatisticInfo();
    //GetTextPoolInfo();
    //GetDictionaryInfo();
    //GetRuleInfo();
    //GetContentInfo();

    NLDB.errorDialog = $("#errorDialog").dialog({ autoOpen: false, title: '遇到一个错误', width: 400, height: 300 });
});
