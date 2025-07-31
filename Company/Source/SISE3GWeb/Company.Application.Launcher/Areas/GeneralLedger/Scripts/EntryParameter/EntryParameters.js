var orderPromise;
var savePromise;

$(() => {
    new EntryParameters();
});

class ParameterModel {
    constructor() {
        this.Id = 0;
        this.ModuleId = 0;
        this.Order = "";
        this.ParameterDescription = "";
        this.TypeId = 0;
        this.TypeDescription = "";
    }
};

class EntryParameters extends Uif2.Page {
    getInitialState() {
        $('.cancel-button').click(function () {
            $("#alertConcept").UifAlert('hide');
        });
    }
    bindEvents() {
        $('#Module').on('itemSelected', this.LoadList);
        $('#ParametersViewTemplate').on('rowAdd', this.ShowAdd);
        $('#ParametersViewTemplate').on('rowEdit', this.EditRow);
        $("#parametersModal").find("#addParameter").on("click", this.SaveParameter);
        $("#parametersModal").find("#cancelParameter").on("click", this.CancelParameter);
    }
    LoadList() {
        $("#parametersAlert").UifAlert('hide');
        if ($('#Module').val() > 0) {
            $("#ParametersViewTemplate").find('.cancel-button').click();
            $("#ParametersViewTemplate").UifListView({
                source: GL_ROOT + "EntryParameter/GetParametersListByModuleId?moduleId=" + $('#Module').val(),
                customDelete: false,
                customAdd: true,
                customEdit: true,
                add: true,
                edit: true,
                delete: true,
                displayTemplate: "#parametersListTemplate",
                deleteCallback: deleteParameter
            });
        } else {
            $("#ParametersViewTemplate").find('.cancel-button').click();
            $("#ParametersViewTemplate").UifListView({ source: null });
            $('.add-button').hide();
        }
    }
    ShowAdd() {
        LoadOrderNumber($('#Module').val());
        orderPromise.then(function (orderData) {
            $("#parametersModal").find("#Order").val(orderData);
        });
        $('#parametersModal').UifModal('showLocal', Resources.AddParameter);
    }
    EditRow(event, data, position) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetParameter",
            data: JSON.stringify({ "moduleId": $('#Module').val(), "parameterId": data.Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.Id > 0) {
                    $("#parametersModal").find("#Id").val(data.Id);
                    $("#parametersModal").find("#Order").val(data.Order);
                    $("#parametersModal").find("#ParameterDescription").val(data.ParameterDescription);
                    $("#parametersModal").find("#DataType").val(data.TypeId);

                    $('#parametersModal').UifModal('showLocal', Resources.EditParameter);
                } else
                    $("#parametersAlert").UifAlert('show', Resources.ErrorGettingRecord, "warning");
            }
        });
        $("#parametersModal").find("#Id").val("");
        $("#parametersModal").find("#ParameterDescription").val("");
        $("#parametersModal").find("#DataType").val("");

        $('#parametersModal').UifModal('showLocal', Resources.EditParameter);
    }
    SaveParameter() {
        var parameterModel = new ParameterModel();
        parameterModel.Id = $("#parametersModal").find("#Id").val();
        parameterModel.ModuleId = $('#Module').val();
        parameterModel.Order = $("#parametersModal").find("#Order").val();
        parameterModel.ParameterDescription = $("#parametersModal").find("#ParameterDescription").val();
        parameterModel.TypeId = $("#parametersModal").find("#DataType").val();

        if (parameterModel.Id == 0) {
            if (ValidateParameterExists(parameterModel.ParameterDescription)) {
                $("#parametersAlert").UifAlert('show', Resources.CodeAlreadyExists, "warning");
                $('#parametersModal').UifModal('hide');
            } else {
                SaveParameterRequest(parameterModel);
            }
        } else {
            SaveParameterRequest(parameterModel);
        }
    }
    CancelParameter() {
        ClearModalFields();
    }
}

var deleteParameter = function (deferred, data) {
    $.ajax({
        type: "POST",
        url: GL_ROOT + "EntryParameter/DeleteParameter",
        data: JSON.stringify({ "parameterId": data.Id }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data > 0)
                $("#parametersAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
            else
                $("#parametersAlert").UifAlert('show', Resources.ErrorDeletingRecord, "danger");

            deferred.resolve();
            $("#ParametersViewTemplate").UifListView("refresh");
        }
    });
};

function ValidateParameterExists(description) {
    var exists = false;
    var fields = $("#ParametersViewTemplate").UifListView("getData");
    if (fields.length > 0) {
        for (var j = 0; j < fields.length; j++) {
            if (description.toUpperCase() == fields[j].ParameterDescription.toUpperCase()) {
                exists = true;
                break;
            }
        }
    }
    return exists;
}

function LoadOrderNumber(moduleId) {
    return orderPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/GetOrderNumber",
            data: JSON.stringify({ "moduleId": moduleId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (orderData) {
            resolve(orderData);
        });
    });
}

function ClearModalFields() {
    $("#parametersModal").find("#Id").val("");
    $("#parametersModal").find("#ParameterDescription").val("");
    $("#parametersModal").find("#DataType").val("");
}

function SaveParameterRequest(parameterModel) {
    savePromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "EntryParameter/SaveParameter",
            data: JSON.stringify({ "parameterModel": parameterModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (saveData) {
                resolve(saveData);
            }
        });
    });

    savePromise.then(function (saveData) {
        if (saveData.Id > 0) {
            if ($("#parametersModal").find("#Id").val() == 0) {
                $("#parametersAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
            }
            else {
                $("#parametersAlert").UifAlert('show', Resources.EditSuccessfully, "success");
            }
        }
        else {
            $("#parametersAlert").UifAlert('show', Resources.SaveError, "danger");
        }
        $('#parametersModal').UifModal('hide');
        ClearModalFields();
        $("#ParametersViewTemplate").UifListView("refresh");
    });
}