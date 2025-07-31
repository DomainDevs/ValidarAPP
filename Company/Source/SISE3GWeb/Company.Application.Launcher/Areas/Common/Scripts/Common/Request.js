function request(url, data, type, msjError, functionRedirect) {
    $.ajax({
        type: type,
        url: rootPath + url,
        data: data,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success && data.result != null) {
            functionRedirect(data.result);
        }
        else {
            if (typeof (data.result) === "object") {
                $.each(data.result, function (index, item) {
                    $.UifNotify('show', { 'type': 'info', 'message': item, 'autoclose': true });
                })
            }
            else if (data.result != null) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': msjError, 'autoclose': true });
            }
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': msjError, 'autoclose': true });
    });
}