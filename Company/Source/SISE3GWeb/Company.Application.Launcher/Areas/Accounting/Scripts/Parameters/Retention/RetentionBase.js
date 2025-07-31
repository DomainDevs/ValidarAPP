var saveCallback = function (deferred, data) {
    $("#alertRetentionBase").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + 'Retention/RetentionBase',
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve();
            $("#alertRetentionBase").UifAlert('show', Resources.SaveSuccessfully, "success");
        } else {
            deferred.reject();
            $("#alertRetentionBase").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};

var editCallback = function (deferred, data) {
    $("#alertRetentionBase").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + 'Retention/UpdateRetentionBase',
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve(data.result);
            $("#alertRetentionBase").UifAlert('show', Resources.EditSuccessfully, "success");
            RefreshRetentionBase();
        } else {
            deferred.reject();
            $("#alertRetentionBase").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};

var deleteBaseCallback = function (deferred, data) {
    $("#alertRetentionBase").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + 'Retention/DeleteRetentionBase',
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve();
            $("#alertRetentionBase").UifAlert('show', Resources.DeleteSuccessfully, "success");
        } else {
            deferred.reject();
            $("#alertRetentionBase").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
        }
    });
};

$(document).ready(function () {
    $("#retentionBaseListView").UifListView(
        {
            source: ACC_ROOT + 'Retention/GetRetentionBase',
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#listTemplateRetentionBase",
            addTemplate: '#add-template',
            addCallback: saveCallback,
            editCallback: editCallback,
            deleteCallback: deleteBaseCallback
        });
});

$("#btn-refresh").click(function () {
    $("#alertRetentionBase").UifAlert('hide');
    $("#retentionBaseListView").UifListView("refresh");
});

function RefreshRetentionBase() {
    $("#retentionBaseListView").UifListView(
        {
            source: ACC_ROOT + 'Retention/GetRetentionBase',
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#listTemplateRetentionBase",
            addTemplate: '#add-template',
            addCallback: saveCallback,
            editCallback: editCallback,
            deleteCallback: deleteBaseCallback
        });
}
