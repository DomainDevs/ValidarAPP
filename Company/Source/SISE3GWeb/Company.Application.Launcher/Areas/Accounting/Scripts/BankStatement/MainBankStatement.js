var currentEditIndex = 0;
var index = 0;
var bankStatementId = 0;
var bankStatementList = [];
var processDate;
var processId;
var documentFormatType = 0;

function RowModel() {
    this.BankStatementId;
    this.BankId;
    this.BankDescription;
    this.BankingMovementTypeId;
    this.BankingMovementTypeDescription;
    this.BranchId;
    this.BranchDescription;
    this.VoucherNumber;
    this.MovementDate;
    this.MovementAmount;
    this.MovementDescription;
    this.MovementThird;
    this.MovementOrigin;
}

$(document).ready(function () {
    $('div#container').hide();

    $("#modalAddBankStatement").find('#btnSaveAdd').click(function () {
        $("#addFormBankStatement").validate();

        if ($("#addFormBankStatement").valid()) {
            var rowModel = new RowModel();

            rowModel.BankStatementId = index++;
            rowModel.BankId = $('#selectAccountNumber').val();
            rowModel.BankDescription = $("#selectBankStatement option:selected").html();
            rowModel.BankingMovementTypeId = $("#addFormBankStatement").find("#BankingMovementTypeId").val();
            rowModel.BankingMovementTypeDescription = $("#addFormBankStatement").find("#BankingMovementTypeId option:selected").html();
            rowModel.BranchId = $("#addFormBankStatement").find("#BranchId").val();
            rowModel.BranchDescription = $("#addFormBankStatement").find("#BranchId option:selected").html();
            rowModel.VoucherNumber = $("#addFormBankStatement").find("#VoucherNumber").val();
            rowModel.MovementDate = $("#addFormBankStatement").find("#MovementDate").val();
            rowModel.MovementAmount = $("#addFormBankStatement").find("#MovementAmount").val();
            rowModel.MovementDescription = $("#addFormBankStatement").find("#MovementDescription").val();
            rowModel.MovementThird = $("#addFormBankStatement").find("#MovementThird").val();
            rowModel.MovementOrigin = "";
            processDate = $("#selectDate option:selected").html();
            processId = 0;//para tomar el valor del id Proceso de la base
            AddStatement(rowModel);
            //$('#tableSingle').UifDataTable('addRow', rowModel);
            $("#addFormBankStatement").formReset();
            $('#modalAddBankStatement').UifModal('hide');
        }
    });

    $("#modalEditBankStatement").find('#btnSaveEdit').click(function () {
        $("#editFormBankStatement").validate();

        if ($("#editFormBankStatement").valid()) {

            var rowModel = new RowModel();
            rowModel.BankStatementId = bankStatementId;
            rowModel.BankId = $('#selectAccountNumber').val();
            rowModel.BankDescription = $("#selectBankStatement option:selected").html();
            rowModel.BankingMovementTypeId = $("#editFormBankStatement").find("#BankingMovementTypeId").val();
            rowModel.BankingMovementTypeDescription = $("#editFormBankStatement").find("#BankingMovementTypeId option:selected").html();
            rowModel.BranchId = $("#editFormBankStatement").find("#BranchId").val();
            rowModel.BranchDescription = $("#editFormBankStatement").find("#BranchId option:selected").html();
            rowModel.VoucherNumber = $("#editFormBankStatement").find("#VoucherNumber").val();
            rowModel.MovementDate = $("#editFormBankStatement").find("#MovementDate").val();
            rowModel.MovementAmount = $("#editFormBankStatement").find("#MovementAmount").val();
            rowModel.MovementDescription = $("#editFormBankStatement").find("#MovementDescription").val();
            rowModel.MovementThird = $("#editFormBankStatement").find("#MovementThird").val();
            rowModel.MovementOrigin = "";

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "BankStatement/UpdateBankStatement",
                data: JSON.stringify({ "bankStatementModel": rowModel }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data) {
                        $("#alertStatement").UifAlert('show', Resources.SaveSuccessfully, "success");
                    }
                    else {
                        $("#alertStatement").UifAlert('show', Resources.ErrorLoadStatement, "danger");
                    }
                }
            });
            $('#tableSingle').UifDataTable();
            $('#tableSingle').UifDataTable('editRow', rowModel, currentEditIndex);
            $("#editFormBankStatement").formReset();
            $('#modalEditBankStatement').UifModal('hide');
        }
    });

    $("#modalDeleteBankStatement").find("#btnDeleteModal").on('click', function () {
        $('#modalDeleteBankStatement').modal('hide');

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankStatement/DeleteBankStatement",
            data: JSON.stringify({ "bankStatementModel": deleteRowModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data) {
                $("#alertBankStatement").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "BankStatement/GetBankStatementsByAccounBankId?accountBankId=" + $('#selectAccountNumber').val() + "&bankName=" +
                             $("#selectBankStatement option:selected").html() + "&processDate=" + $("#selectDate option:selected").html();
                $('#tableSingle').UifDataTable();
                $('#tableSingle').UifDataTable({ source: controller })
            }
        });
    });
});

/*Agregar Registro Manualmente*/
$('#tableSingle').on('rowAdd', function (event, data) {
    $('#alertStatement').UifAlert('hide');
    if ($('#selectDate').val() == "" || $('#selectDate').val() == null) {
        $("#alertStatement").UifAlert('show', Resources.SelectUploadDate, "danger");
    }
    else {
        $('#modalAddBankStatement').UifModal('showLocal', Resources.AddBankStatement);
    }
});

/*Edición de registros*/
$('#tableSingle').on('rowEdit', function (event, data, position) {
    currentEditIndex = position;
    bankStatementId = data.BankStatementId;
    $("#editFormBankStatement").find("#BranchId").val(data.BranchId);
    $("#editFormBankStatement").find("#BankingMovementTypeId").val(data.BankingMovementTypeId);
    $("#editFormBankStatement").find("#VoucherNumber").val(data.VoucherNumber);
    $("#editFormBankStatement").find("#MovementDate").val(data.MovementDate);
    $("#editFormBankStatement").find("#MovementAmount").val(data.MovementAmount);
    $("#editFormBankStatement").find("#MovementDescription").val(data.MovementDescription);
    $("#editFormBankStatement").find("#MovementThird").val(data.MovementThird);

    $('#modalEditBankStatement').UifModal('showLocal', Resources.EditBankStatement);
});

/*Eliminación registro de la tabla*/
deleteRowModel = new RowModel();
$('#tableSingle').on('rowDelete', function (event, data) {
    $('#alertBankStatement').UifAlert('hide');
    $("#selectedRecod").text(data.BankStatementId + " - " + data.MovementDate + " - " + data.VoucherNumber + " - " + data.BankingMovementTypeDescription + " - " + data.MovementThird);
    $('#modalDeleteBankStatement').UifModal('showLocal', '');
    //$('#modalDeleteBankStatement').appendTo("body").modal('show');

    deleteRowModel.BankStatementId = data.BankStatementId;
    deleteRowModel.BankId = data.BankId;
    deleteRowModel.BankDescription = data.BankDescription;
    deleteRowModel.BankingMovementTypeId = data.BankingMovementTypeId;
    deleteRowModel.BankingMovementTypeDescription = data.BankingMovementTypeDescription;
    deleteRowModel.BranchId = data.BranchId;
    deleteRowModel.BranchDescription = data.BranchDescription;
    deleteRowModel.VoucherNumber = data.VoucherNumber;
    deleteRowModel.MovementDate = data.MovementDate;
    deleteRowModel.MovementAmount = data.MovementAmount;
    deleteRowModel.MovementDescription = data.MovementDescription;
    deleteRowModel.MovementThird = data.MovementThird;
    deleteRowModel.MovementOrigin = "";
});

///////////////////////////////////////
/// Botón Cargar Archivo            ///
///////////////////////////////////////
$('#btnUploadFile').click(function () {
    $('#alertStatement').UifAlert('hide');
    $("#formBankStatement").validate();

    if ($("#formBankStatement").valid()) {

        if ($('#selectBankStatement').val() != "") {
            if ($('#selectAccountType').val() != "") {
                if ($('#selectAccountNumber').val() != "") {
                    console.log("Número de Cuenta->" + $('#selectAccountNumber').val());
                    uploadAjaxBankStatement();
                    //$('#imageform').each(function () {
                    //    this.reset();
                    //})
                }
                else {
                    $("#alertStatement").UifAlert('show', Resources.SelectAccountNumber, "danger");
                    //$('#imageform').each(function () {
                    //    this.reset();
                    //})
                }
            }
            else {
                $("#alertStatement").UifAlert('show', Resources.SelectAccountType, "danger");
                //$('#imageform').each(function () {
                //    this.reset();
                //})
            }
        }
        else {
            $("#alerSatement").UifAlert('show', Resources.SelectBank, "danger");
            //$('#imageform').each(function () {
            //    this.reset();
            //})
        }
    }
});

$('#selectAccountType').on('itemSelected', function (event, selectedItem) {
    $('#alertStatement').UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "BankStatement/GetAccountNumbers?bankId=" + $('#selectBankStatement').val() + "&accountTypeId=" + selectedItem.Id;
        $("#selectAccountNumber").UifSelect({ source: controller });
    }
    else {
        $("#selectAccountNumber").UifSelect();
    }
});

$('#selectBankStatement').on('itemSelected', function (event, selectedItem) {
    $('#alertStatement').UifAlert('hide');
    if (selectedItem.Id > 0) {
        $("#selectAccountType").val('');
        $("#selectAccountType").trigger('change');
        $("#selectAccountNumber").val('');
        $("#selectAccountNumber").trigger('change');
    }
});

$('#selectAccountNumber').on('itemSelected', function (event, selectedItem) {
    $('#alertStatement').UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "BankStatement/GetDateProcess?accountBankId=" + $('#selectAccountNumber').val();
        $('#selectDate').UifSelect({ source: controller })
        $('#tableSingleExcel').dataTable().fnClearTable();
        $('#tableSingle').dataTable().fnClearTable();

        /*consulta el tipo de formato y validar si es Excel o txt*/
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankStatement/GetDocumentFormat",
            data: JSON.stringify({ "accountBankId": +$('#selectAccountNumber').val() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.length > 0) {
                    if (data[0].Id != null) {
                        documentFormatType = data[0].Id;
                    }
                }
            }
        });
    }
    else {
        $("#selectDate").val('');
        $("#selectDate").trigger('change');
        $('#tableSingleExcel').dataTable().fnClearTable();
        $('#tableSingle').dataTable().fnClearTable();
    }
});

$("#selectDate").on("itemSelected", function (event, selectedItem) {
    $('#alertStatement').UifAlert('hide');

    if (selectedItem.Id >= 0) {
        var controller = ACC_ROOT + "BankStatement/GetBankStatementsByAccounBankId?accountBankId=" + $('#selectAccountNumber').val() + "&bankName=" +
                         $("#selectBankStatement option:selected").html() + "&processDate=" + $("#selectDate option:selected").html();
        $('#tableSingle').UifDataTable();
        $('#tableSingle').UifDataTable({ source: controller })
        $('#tableSingleExcel').UifDataTable();

    }

});

$('#cancelStatement').click(function () {
    $('#alertStatement').UifAlert('hide');
    SetDataFieldEmpty();
});

/////////////////////////////////////////////////////////////////////////////
/// Funciones                                                             ///
/////////////////////////////////////////////////////////////////////////////
function uploadAjaxBankStatement() {
    $('div#container').show();
    var inputFileImage = document.getElementById("inputControlSingleBankStatement");
    var file = inputFileImage.files[0];

    if (file == undefined) {
        $("#alertStatement").UifAlert('show', Resources.SelectFile, "danger");
    }
    else {
        lockScreen();
        var data = new FormData();
        data.append('uploadedFile', file);
        data.append('accountBankId', $('#selectAccountNumber').val());
        if (file.name.split('.').pop().toLowerCase() == "txt") {
            documentFormatType = 1;
        }
        else {
            documentFormatType = 0;
        }
        data.append('fileType', documentFormatType);

        var url = ACC_ROOT + "BankStatement/ReadFileInMemory";
        $.ajax({
            url: url,
            type: 'POST',
            contentType: false,
            data: data,
            processData: false,
            cache: false,
            success: function (data) {
                if (data.length > 0) {
                    if (data == "BadFileExtension") {
                        $("#alertStatement").UifAlert('show', Resources.WrongFormatBlankColumns, "danger");
                    }
                    else if (data == "NegativeId") {
                        $("#alertStatement").UifAlert('show', Resources.IdsNoNegative, "warning");
                    }
                    else if (data == "fieldEmpty") {
                        $("#alertStatement").UifAlert('show', Resources.BankNumberRequired, "warning");
                    }
                    else if (data == "NotExistsFormat") {
                        $("#alertStatement").UifAlert('show', Resources.WrongNotParametrizedDesignFormat, "warning");
                    }
                    else if (data == "InvalidNumberBank") {
                        $("#alertStatement").UifAlert('show', Resources.BankCdDifferentFile, "warning");
                    }
                    else if (data == "InvalidBankAccountNumber") {
                        $("#alertStatement").UifAlert('show', Resources.MessageBankAccountNumberFile, "warning");
                    }
                    else if (data == "InvalidBankName") {
                        $("#alertStatement").UifAlert('show', Resources.MessageBankNameFile, "warning");
                    }
                    else if (data.indexOf("/") != -1) {
                        var array2 = data.split("/");
                        $("#alertStatement").UifAlert('show', Resources.ValidateRowsExcel + ' ' + array2[1], "danger");
                    }
                    else if (data == "Exception") {
                        $("#alertStatement").UifAlert('show', Resources.ErrorLoadStatement, "danger");
                    }
                    else if (data == "InvalidSeparator") {
                        $("#alertStatement").UifAlert('show', Resources.InvalidSeparator, "warning");
                    }
                    else if (data == "SendDateRequired") {
                        $("#alertStatement").UifAlert('show', Resources.SendDateRequired, "warning");
                    }
                    else if (data == "InvalidFileNumber") {
                        $("#alertStatement").UifAlert('show', Resources.MessageInvalidFileNumber, "warning");
                    }
                    else {
                        $('#tableSingleExcel').dataTable().fnClearTable();
                        $("#tableSingleExcel").UifDataTable();
                        for (var index = 0; index < data.length; index++) {
                            var rowModel = new RowModel();

                            rowModel.BankStatementId = data[index].BankStatementId;
                            rowModel.BankId = data[index].BankId;
                            rowModel.BankDescription = "";
                            rowModel.BankingMovementTypeId = data[index].BankingMovementTypeId
                            rowModel.BankingMovementTypeDescription = data[index].BankingMovementTypeDescription;
                            rowModel.BranchId = data[index].BranchId;
                            rowModel.BranchDescription = data[index].BranchDescription;
                            rowModel.VoucherNumber = data[index].VoucherNumber;
                            rowModel.MovementDate = data[index].MovementDate;
                            rowModel.MovementAmount = data[index].MovementAmount;
                            rowModel.MovementDescription = data[index].MovementDescription;
                            rowModel.MovementThird = data[index].MovementThird;
                            rowModel.MovementOrigin = data[index].MovementOrigin;

                            $('#tableSingleExcel').UifDataTable('addRow', rowModel);
                        }
                        var controller = ACC_ROOT + "BankStatement/GetDateProcess?accountBankId=" + $('#selectAccountNumber').val();
                        $('#selectDate').UifSelect({ source: controller })
                        $("#alertStatement").UifAlert('show', Resources.DocumentErrors, "danger");
}
                }
                
                if (data.length == 0) {
                    var controller = ACC_ROOT + "BankStatement/GetDateProcess?accountBankId=" + $('#selectAccountNumber').val();
                    $('#selectDate').UifSelect({ source: controller })
                    $('#tableSingleExcel').dataTable().fnClearTable();
                    $("#alertStatement").UifAlert('show', Resources.SaveSuccessfully, "success");
                }
                $('div#container').hide();
            }
        });
        $.unblockUI();
    }
}

function SetBankStatemenList() {
    var rowData = null;

    var recordsNumber = $("#tableSingle").UifDataTable('getData').length;

    if (recordsNumber == 0) {
        bankStatementList = [];
    }
    else {

        for (var i = 0; i < recordsNumber; i++) {
            rowData = $("#tableSingle").UifDataTable('getData')[i];
            var rowModel = new RowModel();
            rowModel.Id = rowData.Id;
            rowModel.BankId = rowData.BankId;
            rowModel.BankDescription = rowData.BankDescription;
            rowModel.BankingMovementTypeId = rowData.BankingMovementTypeId;
            rowModel.BankingMovementTypeDescription = rowData.BankingMovementTypeDescription;
            rowModel.BranchId = rowData.BranchId;
            rowModel.BranchDescription = rowData.BranchDescription;
            rowModel.VoucherNumber = rowData.VoucherNumber;
            rowModel.MovementDate = rowData.MovementDate;
            rowModel.MovementAmount = rowData.MovementAmount;
            rowModel.MovementDescription = rowData.MovementDescription;
            rowModel.MovementThird = rowData.MovementThird;
            rowModel.MovementOrigin = "";
            bankStatementList.push(rowModel);
        }
    }
    return bankStatementList;
}

function AddStatement(rowModel) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "BankStatement/AddBankStatement",
        data: JSON.stringify({ "bankStatementModel": rowModel, "processDate": processDate, "processId": processId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == "Exception") {
                $("#alertStatement").UifAlert('show', Resources.ErrorLoadStatement, "danger");
            }
            else {
                $("#alertStatement").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = ACC_ROOT + "BankStatement/GetBankStatementsByAccounBankId?accountBankId=" + $('#selectAccountNumber').val() + "&bankName=" +
                         $("#selectBankStatement option:selected").html() + "&processDate=" + $("#selectDate option:selected").html();
                $('#tableSingle').UifDataTable({ source: controller })
            }
        }
    });
}

function SetDataFieldEmpty() {
    $("#selectBankStatement").val("");
    $("#selectAccountType").val("");
    $("#selectAccountNumber").val("");
    $("#selectDate").val("");
    $("#selectAccountNumber").UifSelect();
    $("#selectDate").UifSelect();
}

