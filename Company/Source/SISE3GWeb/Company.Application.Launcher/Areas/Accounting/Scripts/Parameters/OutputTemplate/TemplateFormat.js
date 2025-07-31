
var structureType = '#StructureType';
var templateTable = '#TemplateTable';
var editTemplateForm = '#editTemplateForm';
var addTemplateForm = '#addTemplateForm';
var idGroup = 0;
var groupDescription = "";
var templateId = 0;


/*TemplateTable///////////////////////////////////////////////////*/
$(templateTable).on('rowAdd', function (event, data) {
    $('#alert').UifAlert('hide');
    templateId = 0;
    if ($(structureType).val() == "") {
        $("#alert").UifAlert('show', Resources.WarningTemplateGroup, "warning");
    }
    else {
        $("#modalAdd").find("#divBankAccount").hide();
        $("#modalAdd").find("#divAutomaticDebit").hide();

        $('#modalAdd').UifModal('showLocal', Resources.AddTemplate + ' ' + groupDescription);
    }
});

$(templateTable).on('rowEdit', function (event, data, position) {
    $('#alert').UifAlert('hide');
    templateId = data.Id;

    $(editTemplateForm).find("#Description").val(data.Description);
    $(editTemplateForm).find("#selectFileType").val(data.FileTypeCd);
    $(editTemplateForm).find("#CurrentFrom").val(data.CurrentFrom);
    $(editTemplateForm).find("#CurrentTo").val(data.CurrentTo);

    $("#modalEdit").find("#divBankAccount").hide();
    $("#modalEdit").find("#divAutomaticDebit").hide();

    $('#modalEdit').UifModal('showLocal', Resources.EditTemplate + ' ' + groupDescription);
});

$(templateTable).on('rowDelete', function (event, data) {
    $('#alert').UifAlert('hide');
    templateId = data.Id;
    var rangeId = data.Id;
    $('#modalDeleteTemplate').appendTo("body").modal('show');
});
/*////////////////////////////////////////////////////////////////*/

/*controles*/
$(structureType).on('itemSelected', function (event, selectedItem) {
    idGroup = selectedItem.Id;
    groupDescription = selectedItem.Text;
    $('#alert').UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Parameters/GetFormatItems?idModule=" + selectedItem.Id;
        $(templateTable).UifDataTable({ source: controller });
    }
});

/*///////////////////////////////////////////////////////////////*/

/*Botones*/
$('#saveAddFieldTemplate').click(function () {
    $("#addTemplateForm").validate();
    if ($("#addTemplateForm").valid()) {
        var format = templateControlTemplate("#addTemplateForm");
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/SaveFormat",
            data: JSON.stringify({ "format": format }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "Parameters/GetFormatItems?idModule=" + $(structureType).val();
                    $(templateTable).UifDataTable({ source: controller });
                }
                else {
                    $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger");
                }
            }
        });
        $("#addTemplateForm").formReset();
        $('#modalAdd').UifModal('hide');
    }
});

$('#saveEdit').click(function () {
    $(editTemplateForm).validate();

    if ($(editTemplateForm).valid()) {
        var format = templateControlTemplate("#editTemplateForm");

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/UpdateFormat",
            data: JSON.stringify({ "format": format }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "Parameters/GetFormatItems?idModule=" + $(structureType).val();
                    $(templateTable).UifDataTable({ source: controller });
                }
                else {
                    $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger");
                }
            }
        });
        $(editTemplateForm).formReset();
        $('#modalEdit').UifModal('hide');
    }
});

$("#deleteFieldModal").on('click', function () {
    $('#modalDeleteTemplate').modal('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/DeleteFormat",
        data: JSON.stringify({ "idTemplate": templateId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                $("#alert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "Parameters/GetFormatItems?idModule=" + $(structureType).val();
                $(templateTable).UifDataTable({ source: controller });
            }
            else {
                if (data.result == 1) {
                    $("#alert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "warning");
                }
                else {
                    $("#alert").UifAlert('show', data.result, "danger");
                }
            }
        }
    });
});


function templateControlTemplate(templateForm) {
    var formatModule = { Id: $(structureType).val() }
    return {
        Id: templateId,
        FormatModule: formatModule,
        Description: $(templateForm).find("#Description").val(),
        FileType: $(templateForm).find("#selectFileType").val(),
        DateFrom: $(templateForm).find("#CurrentFrom").val(),
        DateTo: $(templateForm).find("#CurrentTo").val()
    }
};


$(addTemplateForm).find("#CurrentFrom").blur(function () {
    ValidateCurrentFrom("#addTemplateForm");
});

$(addTemplateForm).find("#CurrentFrom").on('datepicker.change', function (event, date) {
    ValidateCurrentFrom("#addTemplateForm");
});

$(editTemplateForm).find("#CurrentFrom").blur(function () {
    ValidateCurrentFrom("#editTemplateForm");
});


$(editTemplateForm).find("#CurrentFrom").on('datepicker.change', function (event, date) {
    ValidateCurrentFrom("#editTemplateForm");
});




$(addTemplateForm).find("#CurrentTo").blur(function () {
    DateValidateCurrentTo("#addTemplateForm");
});

$(addTemplateForm).find("#CurrentTo").on('datepicker.change', function (event, date) {
    DateValidateCurrentTo("#addTemplateForm");
});

$(editTemplateForm).find("#CurrentTo").blur(function () {
    DateValidateCurrentTo("#editTemplateForm");
});

$(editTemplateForm).find("#CurrentTo").on('datepicker.change', function (event, date) {
    DateValidateCurrentTo("#editTemplateForm");
});



function DateValidateCurrentTo(formatModal) {
    $(formatModal).find("#alertForm").UifAlert('hide');
    if ($(formatModal).find("#DateTo").val() != '') {
        if (IsDate($(formatModal).find("#CurrentTo").val()) == true) {
            if ($(formatModal).find("#CurrentFrom").val() != '') {
                if (CompareDates($(formatModal).find("#CurrentFrom").val(), $(formatModal).find("#CurrentTo").val())) {
                    $(formatModal).find("#alertForm").UifAlert('show', Resources.MessageValidateDateTo, "danger");
                    $(formatModal).find("#CurrentTo").val("");
                }
            }
        } else {
            $(formatModal).find("#alertForm").UifAlert('show', Resources.InvalidDates, "danger");
            $(formatModal).find("#CurrentTo").val("");
        }
    }
}

function ValidateCurrentFrom(formatModal) {
    $(formatModal).find("#alertForm").UifAlert('hide');
    if ($(formatModal).find("#CurrentFrom").val() != '') {
        if (IsDate($(formatModal).find("#CurrentFrom").val()) == true) {
            if ($(formatModal).find("#CurrentTo").val() != '') {
                if (CompareDates($(formatModal).find("#CurrentFrom").val(), $(formatModal).find("#CurrentTo").val())) {
                    $(formatModal).find("#alertForm").UifAlert('show', Resources.MessageValidateDateFrom, "danger");
                    $(formatModal).find("#CurrentFrom").val("");
                }
            }
        } else {
            $(formatModal).find("#alertForm").UifAlert('show', Resources.InvalidDates, "danger");
            $(formatModal).find("#CurrentFrom").val("");
        }
    }
}

