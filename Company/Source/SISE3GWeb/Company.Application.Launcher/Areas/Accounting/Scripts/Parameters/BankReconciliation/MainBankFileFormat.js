var currentEditIndex = 0;
var designFormatId = 0;
var formatId = 0;
var columnNumber = 0;
var bankNetworkId = 0;
var orderId = 0;
var isRequired = 0;


function RowFieldModel() {
    this.AccountBankId;
    this.FormatId;
    this.FormatTypeId;
    this.OrderNumber;
    this.ColumnNumber;
    this.RowNumber;
    this.Description;
    this.ExternalDescription;
    this.Length;
    this.IsRequired;
    this.Separator;
}

$(document).ready(function () {
    /*Modal Add*/
    $("#modalBankAddFieldFormat").find('#saveBankAddFieldFormat').click(function () {
        $("#addBankFieldFormat").validate();

        if ($("#addBankFieldFormat").valid()) {

            if (ValidateAddFieldForm() == true) {
                var rowFieldModel = new RowFieldModel();

                rowFieldModel.AccountBankId = $('#selectBank').val();
                rowFieldModel.FormatId = designFormatId;
                rowFieldModel.FormatTypeId = $('#selectFormatType').val();
                rowFieldModel.OrderNumber = $("#modalBankAddFieldFormat").find("#OrderNumber").val();
                rowFieldModel.ColumnNumber = $("#modalBankAddFieldFormat").find("#ColumnNumber").val();
                rowFieldModel.RowNumber = $("#modalBankAddFieldFormat").find("#RowNumber").val();
                rowFieldModel.Description = $("#modalBankAddFieldFormat").find("#Description").val();
                rowFieldModel.ExternalDescription = $("#modalBankAddFieldFormat").find("#FieldName").val();
                rowFieldModel.Length = $("#modalBankAddFieldFormat").find("#FieldLength").val();
                if ($("#modalBankAddFieldFormat").find('#checkIsRequired').hasClass("glyphicon glyphicon-unchecked")) {
                    rowFieldModel.IsRequired = 0;
                }
                else if ($("#modalBankAddFieldFormat").find('#checkIsRequired').hasClass("glyphicon glyphicon-check")) {
                    rowFieldModel.IsRequired = 1;
                }
                rowFieldModel.Separator = $("#Separator").val();

                SaveFieldFormatDesign(rowFieldModel, "I");

                $("#addBankFieldFormat").formReset();
                $('#modalBankAddFieldFormat').UifModal('hide');
            }
          
        }
    });

    $("#modalBankAddFieldFormat").find('span').click(function () {
        if (isRequired == 1) {
            $("#modalBankAddFieldFormat").find('#checkIsRequired').removeClass("glyphicon glyphicon-check");
            $("#modalBankAddFieldFormat").find('#checkIsRequired').addClass("glyphicon glyphicon-unchecked");
            isRequired = 0;
        }
        else if (isRequired == 0) {
            $("#modalBankAddFieldFormat").find('#checkIsRequired').removeClass("glyphicon glyphicon-unchecked");
            $("#modalBankAddFieldFormat").find('#checkIsRequired').addClass("glyphicon glyphicon-check");
            isRequired = 1;
        }
    });

    /*Modal de Edicion*/
    $("#modalBankEditFieldFormat").find('#saveEditBankFieldFormat').click(function () {
        $("#editBankFieldFormat").validate();

        if ($("#editBankFieldFormat").valid()) {

            if (ValidateEditFieldForm() == true) {
                var rowFieldModel = new RowFieldModel();

                rowFieldModel.AccountBankId = $('#selectBank').val();
                rowFieldModel.FormatId = designFormatId;
                rowFieldModel.FormatTypeId = $('#selectFormatType').val();
                rowFieldModel.OrderNumber = $("#modalBankEditFieldFormat").find("#OrderNumber").val();
                rowFieldModel.ColumnNumber = $("#modalBankEditFieldFormat").find("#ColumnNumber").val();
                rowFieldModel.RowNumber = $("#modalBankEditFieldFormat").find("#RowNumber").val();
                rowFieldModel.Description = $("#modalBankEditFieldFormat").find("#Description").val();
                rowFieldModel.ExternalDescription = $("#modalBankEditFieldFormat").find("#FieldName").val();
                rowFieldModel.Length = $("#modalBankEditFieldFormat").find("#FieldLength").val();
                if ($("#modalBankEditFieldFormat").find('#checkIsRequired').hasClass("glyphicon glyphicon-unchecked") ||
                    $("#modalBankEditFieldFormat").find('#checkIsRequired').hasClass("glyphicon-unchecked glyphicon")) {
                    rowFieldModel.IsRequired = 0;
                }
                else if ($("#modalBankEditFieldFormat").find('#checkIsRequired').hasClass("glyphicon glyphicon-check") ||
                    $("#modalBankEditFieldFormat").find('#checkIsRequired').hasClass("glyphicon-check glyphicon")) {
                    rowFieldModel.IsRequired = 1;
                }
                rowFieldModel.Separator = $("#Separator").val();

                SaveFieldFormatDesign(rowFieldModel, "U");

                $("#editBankFieldFormat").formReset();
                $('#modalBankEditFieldFormat').UifModal('hide');
            }
            else {
                $('#modalBankEditFieldFormat').find("#alertFieldForm").hide();
            }
        }
    });

    $("#modalBankEditFieldFormat").find('span').click(function () {
        if (isRequired == 1) {
            $("#editBankFieldFormat").find('#checkIsRequired').removeClass("glyphicon glyphicon-check");
            $("#editBankFieldFormat").find('#checkIsRequired').addClass("glyphicon glyphicon-unchecked");
            isRequired = 0;
        }
        else if (isRequired == 0) {
            $("#editBankFieldFormat").find('#checkIsRequired').removeClass("glyphicon glyphicon-unchecked");
            $("#editBankFieldFormat").find('#checkIsRequired').addClass("glyphicon glyphicon-check");
            isRequired = 1;
        }
    });

    /*Modal de Eliminación*/
    $("#deleteFieldModal").on('click', function () {
        $("#alertBankFileFormat").UifAlert('hide');
        $('#modalDeleteFormat').modal('hide');

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankStatement/DeleteBankFieldFormat",
            data: JSON.stringify({ "fieldFormatModel": deleteRowFieldModel, "operationType": "D" }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data[0].FieldFormatCode == 0) {
                $("#alertBankFileFormat").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "BankStatement/GetFormatBankByBankId?accountBankId=" +
                                 $('#selectBank').val() + "&fileType=" + $('#selectFormatType').val();
                $("#formatDetails").UifDataTable({ source: controller });
            }
            $('#modalDeleteFormat').UifModal('hide');

        });
    });
});

/// Combo banco
$('#selectBank').on('itemSelected', function (event, selectedItem) {
    $("#alertBankFileFormat").UifAlert('hide');
    if (selectedItem.Id > 0) {
        $("#alertBankFileFormat").UifAlert('hide');
        if ($('#selectFormatType').val() > 0) {
            var controller = ACC_ROOT + "BankStatement/GetFormatBankByBankId?accountBankId=" + selectedItem.Id +
                             "&fileType=" + $('#selectFormatType').val();
            $('#formatDetails').UifDataTable({ source: controller })

            setTimeout(function () {
                GetFormatHeader();
            }, 1500);
        }
    }
    else {
        $('#formatDetails').dataTable().fnClearTable();
    }
});

/// Combo tipo archivo 
$('#selectFormatType').on('itemSelected', function (event, selectedItem) {
    $("#alertBankFileFormat").UifAlert('hide');
    if (selectedItem.Id > 0) {
        $("#alertBankFileFormat").UifAlert('hide');
        if ($('#selectBank').val() > 0) {
            var controller = ACC_ROOT + "BankStatement/GetFormatBankByBankId?accountBankId=" + $('#selectBank').val() +
                             "&fileType=" + selectedItem.Id;
            $('#formatDetails').UifDataTable({ source: controller })

            setTimeout(function () {
                GetFormatHeader();
            }, 1000);
        }
    }
    else {
        $('#formatDetails').dataTable().fnClearTable();
    }
});

/// Tabla de campos de formato: Agregar nuevo registro 
$('#formatDetails').on('rowAdd', function (event, data) {
    $("#alertBankFileFormat").UifAlert('hide');

    $("#MainBankFileFormatForm").validate();
    if ($("#MainBankFileFormatForm").valid()) {
    $("#addBankFieldFormat").find("#OrderNumber").prop("disabled", false);
        $("#addBankFieldFormat").find("#ColumnNumber").prop("disabled", false);

        var controlFieldExtractBank = $("#ViewBagControlFieldExtractBank").val();
        var fields = $('#formatDetails').UifDataTable('getData');
        var orderNumber = 0;
        var columnId = 0;
        var rowNumber = 0;

        if (fields.length == controlFieldExtractBank) {
            $("#alertBankFileFormat").UifAlert('show', Resources.MaxFile + controlFieldExtractBank.toString(), "warning");
        }
        else {
            for (var j = 0; j < fields.length; j++) {
                orderNumber = fields[j].Order;
                columnId = fields[j].ColumnNumber;
                rowNumber = fields[j].RowNumber;
            }

            $("#addBankFieldFormat").find("#OrderNumber").val(fields.length + 1);
            $("#addBankFieldFormat").find("#ColumnNumber").val(columnId + 1);
            $("#addBankFieldFormat").find("#RowNumber").val(rowNumber);

            $("#addBankFieldFormat").find("#OrderNumber").prop("disabled", true);
            $("#addBankFieldFormat").find("#ColumnNumber").prop("disabled", true);

            isRequired = 1;
            $("#modalBankAddFieldFormat").find('#checkIsRequired').addClass("glyphicon glyphicon-check");

            $('#modalBankAddFieldFormat').UifModal('showLocal', Resources.AddFieldFormatDesign);
        }
    }
});

/// Tabla de campos de formato: editar un registro
$('#formatDetails').on('rowEdit', function (event, data, position) {
    $("#alertBankFileFormat").UifAlert('hide');
    currentEditIndex = position;
    orderId = data.Order

    $("#editBankFieldFormat").find("#OrderNumber").prop("disabled", true);
    $("#editBankFieldFormat").find("#ColumnNumber").prop("disabled", true);

    $("#editBankFieldFormat").find("#OrderNumber").val(data.Order);
    $("#editBankFieldFormat").find("#FieldName").val(data.Field);
    $("#editBankFieldFormat").find("#Description").val(data.Description);
    $("#editBankFieldFormat").find("#ColumnNumber").val(data.ColumnNumber);
    $("#editBankFieldFormat").find("#RowNumber").val(data.RowNumber);
    $("#editBankFieldFormat").find("#FieldLength").val(data.Length);
    if (data.Required == true) {
        $("#editBankFieldFormat").find("#checkIsRequired").removeClass("glyphicon glyphicon-unchecked");
        $("#editBankFieldFormat").find("#checkIsRequired").addClass("glyphicon glyphicon-check");
        isRequired = 1;
    }
    else {
        $("#editBankFieldFormat").find("#checkIsRequired").removeClass("glyphicon glyphicon-check");
        $("#editBankFieldFormat").find("#checkIsRequired").addClass("glyphicon glyphicon-unchecked");
        isRequired = 0;
    }

    $('#modalBankEditFieldFormat').UifModal('showLocal', Resources.EditFieldFormatDesign);
});


/// Tabla de campos de formato: eliminar un registro
deleteRowFieldModel = new RowFieldModel();
$('#formatDetails').on('rowDelete', function (event, data) {
    $('#alertBankFileFormat').UifAlert('hide');
    deleteRowFieldModel.AccountBankId = $('#selectBank').val();
    deleteRowFieldModel.FormatId = designFormatId;
    deleteRowFieldModel.FormatTypeId = $('#selectFormatType').val();
    deleteRowFieldModel.OrderNumber = data.Order;
    deleteRowFieldModel.ColumnNumber = data.ColumnNumber;
    deleteRowFieldModel.RowNumber = data.RowNumber;
    deleteRowFieldModel.Description = data.Description;
    deleteRowFieldModel.ExternalDescription = data.Field;
    deleteRowFieldModel.Length = data.Length;
    deleteRowFieldModel.IsRequired = data.Required;

    $('#modalDeleteFormat').appendTo("body").modal('show');
});

/// Funciones

function GetFormatHeader() {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "BankStatement/GetFormatHeaderByAccountBankId",
        data: JSON.stringify({ "accountBankId": $('#selectBank').val(), "formatTypeId": $('#selectFormatType').val() }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data[0].FormatId > 0) {
                $('#Separator').val(data[0].Separator);
                designFormatId = data[0].FormatId;
            }
            else {
                $('#Separator').val("");
                designFormatId = -1;
            }
        }
    });
}

// Fields
function SaveFieldFormatDesign(rowFieldModel, operationType) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "BankStatement/SaveBankFieldFormat",
        data: JSON.stringify({ "fieldFormatModel": rowFieldModel, "operationType": operationType }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data[0].FieldFormatCode < 0) {
                $("#alertBankFileFormat").UifAlert('show', data[0].MessageError, "danger");
            }
            else {
                designFormatId = data[0].FieldFormatCode;
                $("#alertBankFileFormat").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = ACC_ROOT + "BankStatement/GetFormatBankByBankId?accountBankId=" +
                                 $('#selectBank').val() + "&fileType=" + $('#selectFormatType').val();
                $("#formatDetails").UifDataTable({ source: controller });
            }
        }
    });
}

function ValidateAddFieldForm() {
    if ($("#addBankFieldFormat").find('#RowNumber').val() == "") {
        $("#addBankFieldFormat").find("#alertFieldForm").UifAlert('show', Resources.EntryRowNumber, "warning");
        return false;
    }
    if ($("#addBankFieldFormat").find('#Description').val() == "") {
        $("#addBankFieldFormat").find("#alertFieldForm").UifAlert('show', Resources.EntryDescription, "warning");
        return false;
    }
    if ($("#addBankFieldFormat").find('#FieldName').val() == "") {
        $("#addBankFieldFormat").find("#alertFieldForm").UifAlert('show', Resources.EntryFieldName, "warning");
        return false;
    }
    if ($("#addBankFieldFormat").find('#FieldLength').val() == "") {
        $("#addBankFieldFormat").find("#alertFieldForm").UifAlert('show', Resources.EntryLength, "warning");
        return false;
    }

    return true;
}

function ValidateEditFieldForm() {
    if ($("#editBankFieldFormat").find('#RowNumber').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryRowNumber, "warning");
        return false;
    }
    if ($("#editBankFieldFormat").find('#Description').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryDescription, "warning");
        return false;
    }
    if ($("#editBankFieldFormat").find('#FieldName').val() == "") {
        $("#alertFieldForm").UifAlert('show', Resources.EntryFieldName, "warning");
        return false;
    }
    if ($("#editBankFieldFormat").find('#FieldLength').val() == "") {
        $("#editBankFieldFormat").UifAlert('show', Resources.EntryLength, "warning");
        return false;
    }

    return true;
}