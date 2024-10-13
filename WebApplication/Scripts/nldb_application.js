////////////////////////////////////
//
// Dictionary Detail
//
////////////////////////////////////

updateDictionaryControls = function (id, data) {

    $("#txtDID" + id).val(data[0]["did"]);
    $("#txtLength" + id).val(data[0]["length"]);
    $("#txtContent" + id).val(data[0]["content"]);
    $("#txtEnable" + id).val(data[0]["enable"]);
    $("#txtCount" + id).val(data[0]["count"]);
    $("#ddlClassification" + id).val(data[0]["classification"]);
    $("#txtRemark" + id).val(data[0]["remark"]);
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

btnDictionarySwitchItem_Click = function (id) {

    var content = $("#txtContent" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append('content', content);

    NLDB.PostBasicController("SwitchDictionaryByContent",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('SwitchDictionaryByContent：没能获得详细信息！');
            else
                updateDictionaryControls(id, data);
        });

    NLDB.PostBasicController("GetFreqCountByContent",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('GetFreqCountByContent：没能获得详细信息！');
            else
                NLDB.RenderTable('#divQueryFreq', data);
        });
};

btnDictionaryQueryFreqItem_Click = function (id) {

    var content = $("#txtContent" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    NLDB.ShowLoadingSpinner('#divQueryFreq');

    var formData = new FormData();
    formData.append('content', content);

    NLDB.PostBasicController("GetFreqCountByContent",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('GetFreqCountByContent：没能获得详细信息！');
            else
                NLDB.RenderTable('#divQueryFreq', data);
        });

    NLDBWebPager1.GetDataResult();
};

////////////////////////////////////
//
// Phrase Rule Detail
//
////////////////////////////////////

clearPhraseRuleControls = function (id) {

    $("#txtRID" + id).val('');
    //$("#txtAttribute" + id).val('');
    $("#ddlAttribute" + id).val('结构');
    $("#ddlClassification" + id).val('短语');
    $("#txtRule" + id).val('');
    $("#txtRequirements" + id).val('');
};

updatePhraseRuleControls = function (id, data) {

    $("#txtRID" + id).val(data[0]["rid"]);
    //$("#txtAttribute" + id).val(data[0]["attribute"]);
    $("#ddlAttribute" + id).val(data[0]["attribute"]);
    $("#ddlClassification" + id).val(data[0]["classification"]);
    $("#txtRule" + id).val(data[0]["rule"]);
    $("#txtRequirements" + id).val(data[0]["requirements"]);
};

btnAddPhraseRule_Click = function (id) {

    var rule = $("#txtRule" + id).val();
    if (rule == null || typeof rule.length == "undefined" || rule.length <= 0) {
        alert('用于新增规则的规则内容（rule）为空！');
        return;
    }

    {
        var ruleExist = 0;

        var formData = new FormData();
        formData.append('rule', rule);

        NLDB.PostBasicController("SelectPhraseRuleByRule",
            formData, function (data) {
                if (data != null && data.length > 0) {
                    ruleExist = 1;
                    alert('已经存在规则（rule：' + rule + '）！');
                }
            });
        if (ruleExist == 1) return;
    }

    var classification = $("#ddlClassification" + id).val();
    if (classification == null || typeof classification.length == "undefined" || classification.length <= 0) {
        alert('新增规则的分类（classification）为空！');
        return;
    }

    var attribute = $("#ddlAttribute" + id).val();
    if (attribute == null || typeof attribute.length == "undefined" || attribute.length <= 0) {
        alert('新增规则的属性（attribute）为空！');
        return;
    }

    if (confirm("确认新增规则（rule：" + rule + "；classification：" + classification + "；attribute：" + attribute + "）？")) {

        var formData = new FormData();
        formData.append('rule', rule);
        formData.append('classification', classification);
        formData.append('attribute', attribute);

        NLDB.PostBasicController("InsertPhraseRule",
            formData, function (data) {
                if (data == null || data.length <= 0)
                    alert('InsertPhraseRule：没能获得详细信息！');
                else {
                    //Refresh
                    NLDBWebPager2.GetDataResult();
                    //Update
                    updatePhraseRuleControls(id, data);
                }
            });
    }

};

btnEditPhraseRule_Click = function (id) {

    var rid = $("#txtRID" + id).val();
    if (rid == null || typeof rid.length == "undefined" || rid.length <= 0) {
        alert('用于编辑的规则标识（rid）为空！');
        return;
    }

    var rule = $("#txtRule" + id).val();
    if (rule == null || typeof rule.length == "undefined" || rule.length <= 0) {
        alert('用于编辑的规则内容（rule）为空！');
        return;
    }

    {
        var ruleConflict = 0;

        var formData = new FormData();
        formData.append('rule', rule);

        NLDB.PostBasicController("SelectPhraseRuleByRule",
            formData, function (data) {
                if (data != null && data.length > 0) {

                    if (data[0]["rid"] != rid) {

                        ruleConflict = 1;
                        alert('与现有规则（rule：' + rule + '）相冲突！');
                    }
                }
            });
        if (ruleConflict == 1) return; 
    }

    var classification = $("#ddlClassification" + id).val();
    if (classification == null || typeof classification.length == "undefined" || classification.length <= 0)
        return;

    var attribute = $("#ddlAttribute" + id).val();
    if (attribute == null || typeof attribute.length == "undefined" || attribute.length <= 0)
        return;

    if (confirm("确认修改规则（rid：" + rid + "；rule：" + rule + "；classification：" + classification + "；attribute：" + attribute + "）？")) {

        var formData = new FormData();
        formData.append('rid', rid);
        formData.append('rule', rule);
        formData.append('classification', classification);
        formData.append('attribute', attribute);

        NLDB.PostBasicController("UpdatePhraseRule",
            formData, function (data) {
                if (data == null || data.length <= 0)
                    alert('UpdatePhraseRule：没能获得详细信息！');
                else {
                    //Refresh
                    NLDBWebPager2.GetDataResult();
                    //Update
                    updatePhraseRuleControls(id, data);
                }
            });
    }

};

btnRandomContentItem_Click = function (id) {

    NLDB.QueryBasicController("GetExternalContentRandomly",
        null, function (data) {
            if (data == null || data.length <= 0)
                alert('GetExternalContentRandomly：没能获得详细信息！');
            else {
                $('#txtInput' + id).val(data[0]["content"]);
                $("#divOutput" + id).html('<hr style="border:1px dotted #036"/>');
            }
        });
};

btnCheckPhraseRuleItem_Click = function (id) {

    var rule = $("#txtRule" + id).val();
    if (rule == null || typeof rule.length == "undefined" || rule.length <= 0) {
        alert('用于测试的规则（rule）为空！');
        return;
    }

    var content = $("#txtInput" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0) {
        alert('用于测试的内容（content）为空！');
        return; 
    }

    var formData = new FormData();
    formData.append('rule', rule);
    formData.append('content', content);

    NLDB.PostBasicController("MatchPhraseRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('MatchPhraseRule：没能获得详细信息！');
            else {
                //Refresh
                NLDB.RenderTable('#divOutput' + id, data);
            }
        });

}

btnCheckStoredPhraseRule_Click = function (id) {

    var content = $("#txtInput" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0) {
        alert('用于测试的内容（content）为空！');
        return;
    }

    var formData = new FormData();
    formData.append('content', content);

    NLDB.PostBasicController("ExtractPhrases",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ExtractPhrases：没能获得详细信息！');
            else {
                //Refresh
                NLDB.RenderTable('#divOutput' + id, data);
            }
        });

}

////////////////////////////////////
//
// Parse Rule Detail
//
////////////////////////////////////

clearParseRuleControls = function (id) {

    $("#txtRID" + id).val('');
    $("#txtPrefix" + id).val('');
    $("#txtSuffix" + id).val('');

    $("#txtRule" + id).val('');

    $("#txtCount" + id).val('');
    $("#txtLength" + id).val('');
    $("#txtPriority" + id).val('');
};

updateParseRuleControls = function (id, data) {

    $("#txtRID" + id).val(data[0]["rid"]);
    $("#txtPrefix" + id).val(data[0]["static_prefix"]);
    $("#txtSuffix" + id).val(data[0]["static_suffix"]);

    $("#txtRule" + id).val(data[0]["rule"]);

    $("#txtCount" + id).val(data[0]["parameter_count"]);
    $("#txtLength" + id).val(data[0]["minimum_length"]);
    $("#txtPriority" + id).val(data[0]["controllable_priority"]);
};

btnAddParseRule_Click = function (id) {

    var op = $("#txtRule" + id).attr('name');

    var rule = $("#txtRule" + id).val();
    if (rule == null || typeof rule.length == "undefined" || rule.length <= 0) {
        alert('用于新增规则的规则内容（rule）为空！');
        return;
    }

    //Check
    if (rule.indexOf('$') < 0) {

        if (confirm("确认新增内容（content：" + rule + "）？")) {

            var formData = new FormData();
            formData.append('content', rule);

            NLDB.PostBasicController("ContentInsertValue",
                formData, function (data) {
                    if (op != null && op.length > 0) {

                        if (op == 'fmm') btnFMMSplitAllStructRule_Click(id);
                        else if (op == 'mmm') btnMultilayerSplitAll_Click(id);
                        else if (op == 'bmm') btnBMMSplitAllStructRule_Click(id);
                    }
                });
        }
        return;
    }

    {
        var ruleExist = 0;

        var formData = new FormData();
        formData.append('rule', rule);

        NLDB.PostBasicController("SelectParseRuleByRule",
            formData, function (data) {
                if (data != null && data.length > 0) {
                    ruleExist = 1;
                    alert('已经存在规则（rule：' + rule + '）！');
                }
            });
        if (ruleExist == 1) return;
    }

    if (confirm("确认新增规则（rule：" + rule + "）？")) {

        var formData = new FormData();
        formData.append('rule', rule);

        NLDB.PostBasicController("InsertParseRule",
            formData, function (data) {
                if (data == null || data.length <= 0)
                    alert('InsertParseRule：没能获得详细信息！');
                else {
                    //Refresh
                    NLDBWebPager3.GetDataResult();
                    //Update
                    updateParseRuleControls(id, data);

                    if (op != null && op.length > 0) {
                        if (op == 'fmm') btnFMMSplitAllStructRule_Click(id);
                        else if (op == 'mmm') btnMultilayerSplitAll_Click(id);
                        else if (op == 'bmm') btnBMMSplitAllStructRule_Click(id);
                    }
                }
            });
    }

};

btnEditParseRule_Click = function (id) {

    var rid = $("#txtRID" + id).val();
    if (rid == null || typeof rid.length == "undefined" || rid.length <= 0) {
        alert('用于编辑的规则标识（rid）为空！');
        return;
    }

    var rule = $("#txtRule" + id).val();
    if (rule == null || typeof rule.length == "undefined" || rule.length <= 0) {
        alert('用于编辑的规则内容（rule）为空！');
        return;
    }

    {
        var ruleConflict = 0;

        var formData = new FormData();
        formData.append('rule', rule);

        NLDB.PostBasicController("SelectParseRuleByRule",
            formData, function (data) {
                if (data != null && data.length > 0) {

                    if (data[0]["rid"] != rid) {

                        ruleConflict = 1;
                        alert('与现有规则（rule：' + rule + '）相冲突！');
                    }
                }
            });
        if (ruleConflict == 1) return;
    }

    if (confirm("确认修改规则（rid：" + rid + "）？")) {

        var formData = new FormData();
        formData.append('rid', rid);
        formData.append('rule', rule);

        NLDB.PostBasicController("UpdateParseRule",
            formData, function (data) {
                if (data == null || data.length <= 0)
                    alert('UpdateParseRule：没能获得详细信息！');
                else {
                    //Refresh
                    NLDBWebPager3.GetDataResult();
                    //Update
                    updateParseRuleControls(id, data);

                    var op = $("#txtRule" + id).attr('name');
                    if (op != null && op.length > 0) {
                        if (op == 'fmm') btnFMMSplitAllStructRule_Click(id);
                        else if (op == 'mmm') btnMultilayerSplitAll_Click(id);
                        else if (op == 'bmm') btnBMMSplitAllStructRule_Click(id);
                    }
                }
            });
    }

};

btnCheckParseRuleItem_Click = function (id) {

    $("#txtRule" + id).attr('name', '');

    var rule = $("#txtRule" + id).val();
    if (rule == null || typeof rule.length == "undefined" || rule.length <= 0) {
        alert('用于测试的规则（rule）为空！');
        return;
    }

    var content = $("#txtInput" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0) {
        alert('用于测试的内容（content）为空！');
        return;
    }

    var formData = new FormData();
    formData.append('rule', rule);
    formData.append('content', content);

    NLDB.PostBasicController("ParseContentByRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ParseContentByRule：没能获得详细信息！');
            else {
                //Refresh
                NLDB.RenderTable('#divOutput' + id, data);
            }
        });

}

btnCheckStoredParseRule_Click = function (id) {

    $("#txtRule" + id).attr('name', '');

    var content = $("#txtInput" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0) {
        alert('用于测试的内容（content）为空！');
        return;
    }

    var formData = new FormData();
    formData.append('content', content);

    NLDB.PostBasicController("ExtractStructs",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('ExtractStructs：没能获得详细信息！');
            else {
                //Refresh
                NLDB.RenderTable('#divOutput' + id, data);
            }
        });

}

cbCombinateContent = function (op) {

    var cbContents = document.getElementsByName("cbContent");

    var content = '';
    for (var i = 0; i < cbContents.length; i++) {
        if (cbContents[i].checked != true) {
            content += '$';
        }
        else {
            content += cbContents[i].value;
        }
    }
    //Replace.
    content = content.replace(/\${2,}/g, '$');
    if (content[0] == '$') content = content.substr(1,content.length - 1);
    if (content[content.length - 1] == '$') content = content.substr(0, content.length - 1);
    content = content.trim();
    //Split
    var array = content.split('$');
    //Check result.
    if (array.length > 26) {
        alert('参数过多（content：' + content + '）！'); return;
    }

    content = '';
    for (var i = 0; i < array.length && i < 26; i++) {

        if (i == array.length - 1) {
            content = content + array[i];
        }
        else {
            content = content + array[i] + '$' + String.fromCharCode(97 + i);
        }
    }

    //Update content.
    $("#txtRule3").val(content);
    $("#txtRule3").attr('name', op);
};

btnFMMSplitAllStructRule_Click = function (id) {

    NLDB.ShowLoadingSpinner('#divOutput' + id);

    var content = $("#txtInput" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("FMMSplitAllByStructRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('FMMSplitAllByStructRule：没能获得解析结果！');
            else {

                $("#txtRule3").attr('name', 'fmm');

                for (var i in data) {

                    if (data[i]["节点类"] == "rule") {
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
                        if (data[i]["内容标识"] <= 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentInsertValue(\'fmm\', \'' + value + '\')" class="killbutton">增加</a>';
                        }
                        op += '&nbsp;<a href="#" onclick="NLDBContentDeleteValue(\'fmm\',\'' + value + '\')" class="killbutton">删除</a>';
                    }
                    data[i]["操作"] = op;
                }

                NLDB.RenderTable('#divOutput' + id, data);

                $("a.killbutton").button();
            }
        });
};

btnBMMSplitAllStructRule_Click = function (id) {

    NLDB.ShowLoadingSpinner('#divOutput' + id);

    var content = $("#txtInput" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("BMMSplitAllByStructRule",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('BMMSplitAllByStructRule：没能获得解析结果！');
            else {

                $("#txtRule3").attr('name', 'bmm');

                for (var i in data) {

                    if (data[i]["节点类"] == "rule") {
                        data[i]["选择"] = '';
                    }
                    else {
                        data[i]["选择"] = '<input name="cbContent" type="checkbox" value="' + data[i]["内容"] + '" onclick="cbCombinateContent(\'bmm\');"/>';
                    }

                    var op = '';
                    if (data[i]["节点类"] == "var") {
                        var value = data[i]["内容"];
                        if (data[i]["终结符"] == 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentSetTerminator(\'bmm\', \'' + value + '\')" class="killbutton">终结</a>';
                        }
                        if (data[i]["内容标识"] <= 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentInsertValue(\'bmm\', \'' + value + '\')" class="killbutton">增加</a>';
                        }
                        op += '&nbsp;<a href="#" onclick="NLDBContentDeleteValue(\'bmm\',\'' + value + '\')" class="killbutton">删除</a>';
                    }
                    data[i]["操作"] = op;
                }

                NLDB.RenderTable('#divOutput' + id, data);

                $("a.killbutton").button();
            }
        });
};

btnMultilayerSplitAll_Click = function (id) {

    NLDB.ShowLoadingSpinner('#divOutput' + id);

    var content = $("#txtInput" + id).val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append("content", content);

    NLDB.PostBasicController("MultilayerSplitAll",
        formData, function (data) {
            if (data == null || data.length <= 0)
                alert('MultilayerSplitAll：没能获得解析结果！');
            else {

                $("#txtRule3").attr('name', 'mmm');

                for (var i in data) {

                    data[i]["选择"] = '<input name="cbContent" type="checkbox" value="' + data[i]["内容"] + '" onclick="cbCombinateContent(\'mmm\');"/>';

                    var op = '';
                    if (data[i]["类型"] == 'BMM' || data[i]["类型"] == 'FMM') {
                        var value = data[i]["内容"];
                        if (data[i]["终结符"] == 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentSetTerminator(\'mmm\', \'' + value + '\')" class="killbutton">终结</a>';
                        }
                        if (data[i]["内容标识"] <= 0) {
                            op += '&nbsp;<a href="#" onclick="NLDBContentInsertValue(\'mmm\', \'' + value + '\')" class="killbutton">增加</a>';
                        }
                        op += '&nbsp;<a href="#" onclick="NLDBContentDeleteValue(\'mmm\',\'' + value + '\')" class="killbutton">删除</a>';
                    }
                    data[i]["操作"] = op;
                }

                NLDB.RenderTable('#divOutput' + id, data);

                $("a.killbutton").button();
            }
        });
};

NLDBContentSetTerminator = function (op, content) {

    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认终结内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentSetTerminator",
            formData, function (data) {

                if (op != null && op.length > 0) {
                    if (op == 'fmm') btnFMMSplitAllStructRule_Click('3');
                    else if (op == 'mmm') btnMultilayerSplitAll_Click('3');
                    else if (op == 'bmm') btnBMMSplitAllStructRule_Click('3');
                }
            });
    }
}

NLDBContentInsertValue = function (op, content) {

    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认增加内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentInsertValue",
            formData, function (data) {

                if (op != null && op.length > 0) {
                    if (op == 'fmm') btnFMMSplitAllStructRule_Click('3');
                    else if (op == 'mmm') btnMultilayerSplitAll_Click('3');
                    else if (op == 'bmm') btnBMMSplitAllStructRule_Click('3');
                }
            });
    }
}

NLDBContentDeleteValue = function (op, content) {

    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    if (confirm("确认删除内容（content：" + content + "）？")) {

        var formData = new FormData();
        formData.append('content', content);

        NLDB.PostBasicController("ContentDeleteValue",
            formData, function (data) {

                if (op != null && op.length > 0) {
                    if (op == 'fmm') btnFMMSplitAllStructRule_Click('3');
                    else if (op == 'mmm') btnMultilayerSplitAll_Click('3');
                    else if (op == 'bmm') btnBMMSplitAllStructRule_Click('3');
                }
            });
    }
}

////////////////////////////////////
//
// Web Pagers
//
////////////////////////////////////

var NLDBWebPager1 = Object.create(NLDBWebPager);
var NLDBWebPager2 = Object.create(NLDBWebPager);
var NLDBWebPager3 = Object.create(NLDBWebPager);

////////////////////////////////////
//
// Web Pager 1
//
////////////////////////////////////

NLDBWebPager1.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager1.RefreshCountResult = function () {

    var content = $("#txtContent1").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append('content', content);

    var instance = this;
    NLDB.PostBasicController(this.countAction,
        formData, function (data) {
            instance.DataCount = data != null ? parseInt(data[0]["总数"]) : 0;
        });
};

NLDBWebPager1.RefreshSelectResult = function () {

    var content = $("#txtContent1").val();
    if (content == null || typeof content.length == "undefined" || content.length <= 0)
        return;

    var formData = new FormData();
    formData.append('content', content);
    formData.append('pageSize', this.PageSize);
    formData.append('pageIndex', this.CurrPageNum);

    var instance = this;
    NLDB.PostBasicController(this.selectAction,
        formData, function (data) { instance.RenderResult(data); });
};

////////////////////////////////////
//
// Web Pager 2
//
////////////////////////////////////

NLDBWebPager2.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    for (var i in data) {
        var op = '';
        var id = data[i]["标识"];
        if (data[i]["其他要求"] == "User") {
            op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.DeletePhraseRule(' + id + ')" class="killbutton">删除</a>&nbsp;';
        }
        op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.ViewDataDetail(' + id + ')" class="killbutton">详情</a>';
        data[i]["操作"] = op;
    }

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager2.ViewDataDetail = function (id) {

    NLDB.QueryBasicController("GetPhraseRuleByID",
        { 'rid': id }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetPhraseRuleByID：没能获得详细信息！');
            else
                updatePhraseRuleControls(NLDBWebPager2.id, data);
        });
};

NLDBWebPager2.DeletePhraseRule = function (id) {

    if (confirm("确认删除规则（rid：" + id + "）？")) {

        NLDB.QueryBasicController("DeletePhraseRuleByID",
            { 'rid': id }, function (data) {
                    //Refresh
                    NLDBWebPager2.GetDataResult();
                    //Clear
                    clearPhraseRuleControls(NLDBWebPager2.id);
            });
    }
};

////////////////////////////////////
//
// Web Pager 3
//
////////////////////////////////////

NLDBWebPager3.RenderResult = function (data) {

    $('#' + this.divPagerName).html(this.GetPagerHTML());

    for (var i in data) {
        var id = data[i]["标识"];
        var op = '';
        op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.DeleteParseRule(' + id + ')" class="killbutton">删除</a>&nbsp;';
        op = op + '<a href="#" onclick="NLDBWebPager' + this.id + '.ViewDataDetail(' + id + ')" class="killbutton">详情</a>';
        data[i]["操作"] = op;
    }

    this.RenderTable('#' + this.divDataName, data);

    $("a.killbutton").button();

    this.CheckButtonStatus();
};

NLDBWebPager3.ViewDataDetail = function (id) {

    NLDB.QueryBasicController("GetParseRuleByID",
        { 'rid': id }, function (data) {
            if (data == null || data.length <= 0)
                alert('GetParseRuleByID：没能获得详细信息！');
            else
                updateParseRuleControls(NLDBWebPager3.id, data);
        });
};

NLDBWebPager3.DeleteParseRule = function (id) {

    if (confirm("确认删除规则（rid：" + id + "）？")) {

        NLDB.QueryBasicController("DeleteParseRuleByID",
            { 'rid': id }, function (data) {
                //Refresh
                NLDBWebPager3.GetDataResult();
                //Clear
                clearParseRuleControls(NLDBWebPager3.id);

                var op = $("#txtRule" + id).attr('name');
                if (op != null && op.length > 0) {
                    if (op == 'fmm') btnFMMSplitAllStructRule_Click(id);
                    else if (op == 'mmm') btnMultilayerSplitAll_Click(id);
                    else if (op == 'bmm') btnBMMSplitAllStructRule_Click(id);
                }
            });
    }
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
            break;
        case 1:
            NLDBWebPager2.GetDataResult();
            break;
        case 2:
            NLDBWebPager3.GetDataResult();
            break;
        default:
            alert("unknown tab index!");
            break;
    }
};

$(function () {

    //Controller
    NLDB.pathName = "Application";

    //Pager
    NLDBWebPager1.DataCount = 0;
    NLDBWebPager1.CurrPageNum = 1;
    NLDBWebPager1.TotalPageNum = 1;
    NLDBWebPager1.PageSize = 20;
    NLDBWebPager1.id = "1";
    NLDBWebPager1.divDataName = 'divQueryRelatedFreq';
    NLDBWebPager1.divPagerName = "divWebPager1";
    NLDBWebPager1.selectAction = 'SelectRelatedFreq';
    NLDBWebPager1.countAction = 'GetRelatedFreqCount';

    //NLDBWebPager1.RefreshPagerStatus();

    NLDBWebPager2.DataCount = 0;
    NLDBWebPager2.CurrPageNum = 1;
    NLDBWebPager2.TotalPageNum = 1;
    NLDBWebPager2.PageSize = 20;
    NLDBWebPager2.id = "2";
    NLDBWebPager2.divDataName = 'divPhraseRule';
    NLDBWebPager2.divPagerName = "divWebPager2";
    NLDBWebPager2.selectAction = 'SelectEditablePhraseRule';
    NLDBWebPager2.countAction = 'GetEditablePhraseRuleCount';

    //NLDBWebPager2.RefreshPagerStatus();

    NLDBWebPager3.DataCount = 0;
    NLDBWebPager3.CurrPageNum = 1;
    NLDBWebPager3.TotalPageNum = 1;
    NLDBWebPager3.PageSize = 10;
    NLDBWebPager3.id = "3";
    NLDBWebPager3.divDataName = 'divParseRule';
    NLDBWebPager3.divPagerName = "divWebPager3";
    NLDBWebPager3.selectAction = 'SelectEditableParseRule';
    NLDBWebPager3.countAction = 'GetEditableParseRuleCount';

    //NLDBWebPager3.RefreshPagerStatus();

    $("#tabs").tabs({
        'select': function (event, ui) { RefreshCurrentlySelectedTab(ui.index); }
    });

    //NLDBWebPager1.GetDataResult();
    //NLDBWebPager2.GetDataResult();
    //NLDBWebPager3.GetDataResult();

    $("a.killbutton").button();

    NLDB.errorDialog = $("#errorDialog").dialog({ autoOpen: false, title: '遇到一个错误', width: 400, height: 300 });
});
