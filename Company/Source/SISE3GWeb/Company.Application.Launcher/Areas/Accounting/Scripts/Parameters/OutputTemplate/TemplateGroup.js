
var saveTemplateGroup = function (deferred, data) {
    $("#alertTemplateGroup").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/SaveFormatModule",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve();
            $("#alertTemplateGroup").UifAlert('show', Resources.SaveSuccessfully, "success");
        } else {
            deferred.reject();
            $("#alertTemplateGroup").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};

var editTemplateGroup = function (deferred, data) {
    $("#alertTemplateGroup").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/UpdateFormatModule",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve(data.result);
            $("#alertTemplateGroup").UifAlert('show', Resources.EditSuccessfully, "success");
        } else {
            deferred.reject();
            $("#alertTemplateGroup").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};

var deleteTemplateGroup = function (deferred, data) {
    $("#alertTemplateGroup").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/DeleteFormatModule",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve();
            $("#alertTemplateGroup").UifAlert('show', Resources.DeleteSuccessfully, "success");
        } else {
            if (data.result.Description == "relatedObject") {
                deferred.UifAlert;
                $("#alertTemplateGroup").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "warning");
            }
            else {
                deferred.reject();
                $("#alertTemplateGroup").UifAlert('show', Resources.ErrorTransaction, "danger");
            }
        }
    });
};

$(document).ready(function () {
    $("#alertTemplateGroupListView").UifListView(
    {
        source: ACC_ROOT + "Parameters/GetFormatModules",
        customDelete: false,
        customAdd: false,
        customEdit: false,
        add: true,
        edit: true,
        delete: true,
        displayTemplate: "#listTemplateGroup",
        addTemplate: '#add-template',
        addCallback: saveTemplateGroup,
        editCallback: editTemplateGroup,
        deleteCallback: deleteTemplateGroup
    });
});

$("#btn-refresh").click(function () {
    $("#alertTemplateGroup").UifAlert('hide');
    $("#alertTemplateGroupListView").UifListView("refresh");
});



