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

////////////////////////////////////
//
// NLDB Class
//
////////////////////////////////////

var NLDB = {

    siteName: 'NLDB',
    pathName: null,
    errorDialog: null,

};

////////////////////////////////////
//
// NLDB
//
////////////////////////////////////

NLDB.ShowLoadingSpinner = function (target) {
    var url = "/Images/ajax-loader.gif";
    if (NLDB.siteName != null) url = '/' + NLDB.siteName + url;
    $(target).html('<img src="' + url + '" />');
};

NLDB.QueryBasicController = function (controller, data, callback) {
    var url = '/' + NLDB.pathName + '/' + controller;
    if (NLDB.siteName != null) url = '/' + NLDB.siteName + url;
    $.ajax({
        url: url,
        type: 'get',
        dataType: 'json',
        data: data,
        async: false,
        success: callback,
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 403) {
                window.location = '/Account/Login';
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

NLDB.PostBasicController = function (controller, formData, callback) {

    var url = '/' + NLDB.pathName + '/' + controller;
    if (NLDB.siteName != null) url = '/' + NLDB.siteName + url;
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'json',
        data: formData,
        async: false,
        processData: false,   // jQuery不要去处理发送的数据
        contentType: false,   // jQuery不要去设置Content-Type请求头
        success: callback,
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 403) {
                window.location = '/Account/Login';
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

NLDB.RenderTable = function (targetElement, data) {

    $(targetElement).html(NLDB.GetTableHTML(data));
};

NLDB.GetTableHTML = function (data) {
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
