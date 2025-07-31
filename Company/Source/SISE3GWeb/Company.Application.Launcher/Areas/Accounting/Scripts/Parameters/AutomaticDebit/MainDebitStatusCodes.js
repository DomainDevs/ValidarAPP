var debitStatusCurrentEditIndex = 0;
var debitStatusIndex = 0;
var debitStatusCode = "0";
var debitStatusTable = 0;
var debitStatusEnabled = 0;

function DebitStatusRowModel() {
    this.Id;
    this.SmallDescription;
    this.Description;
    this.IsEnabled;
    this.IsRetry;
    this.DebitStatusType;
    this.RetryDays;
}


/// Combo tabla estado débito
$('#selectDebitStatusTable').on('itemSelected', function (event, selectedItem) {
    $("#MainDebitStatusAlert").UifAlert('hide');
    $("#tableDebitStatus").UifDataTable();
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Networks/GetDebitStatusCodesByTableId?tableId=" + $('#selectDebitStatusTable').val();
        $('#tableDebitStatus').UifDataTable({ source: controller })
    }
    else {
        $('#tableDebitStatus').dataTable().fnClearTable();
    }
});

/// Tabla de estados débito: Agregar nuevo registro
$('#tableDebitStatus').on('rowAdd', function (event, data) {
    $('#MainDebitStatusModalAdd').UifModal('showLocal', Resources.AddDebitStatusCode);
});

$('#MainDebitStatusSaveAdd').click(function () {
    $("#MainDebitStatusAddForm").validate();

    if ($("#MainDebitStatusAddForm").valid()) {

        if (ValidateAddForm() == true) {
            var rowModel = new DebitStatusRowModel();

            rowModel.Id = $("#selectDebitStatusTable").val();
            rowModel.SmallDescription = $("#MainDebitStatusAddForm").find("#SmallDescription").val();
            rowModel.Description = $("#MainDebitStatusAddForm").find("#Description").val();

            if ($("#MainDebitStatusAddForm").find('#checkStatusEnabled').hasClass("glyphicon glyphicon-unchecked")) {
                rowModel.IsEnabled = 0;
            }
            else if ($("#MainDebitStatusAddForm").find('#checkStatusEnabled').hasClass("glyphicon glyphicon-check")) {
                rowModel.IsEnabled = 1;

            }
            rowModel.IsRetry = ($("#MainDebitStatusAddForm").find('#RetriesNumber').val() == 0) ? false : true;
            var debitStatusType = $("#MainDebitStatusAddForm").find('input:radio[name=options]:checked').val();
            rowModel.DebitStatusType = debitStatusType;
            rowModel.RetryDays = $("#MainDebitStatusAddForm").find("#RetriesNumber").val();

            SaveCouponStatus(rowModel, "I");

            $("#MainDebitStatusAddForm").formReset();
            $('#MainDebitStatusModalAdd').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").UifAlert('hide');
            }, 3000);
        }
    }
});

/// Tabla de estados débito: editar un registro
$('#tableDebitStatus').on('rowEdit', function (event, data, position) {
    debitStatusCurrentEditIndex = position;
    debitStatusTable = data.Id;
    debitStatusCode = data.SmallDescription;

    $("#MainDebitStatusEditForm").find("#SmallDescription").attr("disabled", true);
    $("#MainDebitStatusEditForm").find("#SmallDescription").val(data.SmallDescription);
    $("#MainDebitStatusEditForm").find("#Description").val(data.Description);
    $("#MainDebitStatusEditForm").find("#RetriesNumber").val(data.RetryDays);
    $("#MainDebitStatusEditForm").find('#checkStatusEnabled').removeAttr("class")

    if (data.IsEnabled == true) {

        $("#MainDebitStatusEditForm").find('#checkStatusEnabled').attr("class", "glyphicon glyphicon-check")
        debitStatusEnabled = 1;
    }
    else {
        $("#MainDebitStatusEditForm").find('#checkStatusEnabled').attr("class", "glyphicon glyphicon-unchecked")
        debitStatusEnabled = 0;
    }
    
    if (data.DebitStatusType == Resources.Applied) {
        
        $("#MainDebitStatusEditForm").find("#optionApplied").prop("checked", true);
    }
    else if (data.DebitStatusType == Resources.Rejected) {
        
        $("#MainDebitStatusEditForm").find("#optionRejection").prop("checked", true);
    }

    $('#MainDebitStatusModalEdit').UifModal('showLocal', Resources.EditDebitStatusCode);
});

$('#MainDebitStatusSaveEdit').click(function () {
    $("#MainDebitStatusEditForm").validate();

    if ($("#MainDebitStatusEditForm").valid()) {

        if (ValidateEditForm() == true) {
            var rowModel = new DebitStatusRowModel();

            rowModel.Id = debitStatusTable;
            rowModel.SmallDescription = $("#MainDebitStatusEditForm").find("#SmallDescription").val();
            rowModel.Description = $("#MainDebitStatusEditForm").find("#Description").val();

            if ($("#MainDebitStatusEditForm").find('#checkStatusEnabled').hasClass("glyphicon glyphicon-unchecked")) {
                rowModel.IsEnabled = 0;
            }
            else if ($("#MainDebitStatusEditForm").find('#checkStatusEnabled').hasClass("glyphicon glyphicon-check")) {
                rowModel.IsEnabled = 1;
            }
            rowModel.IsRetry = ($("#MainDebitStatusEditForm").find('#RetriesNumber').val() == 0) ? false : true;
            var debitStatusType = $("#MainDebitStatusEditForm").find('input:radio[name=options]:checked').val();
            rowModel.DebitStatusType = debitStatusType;
            rowModel.RetryDays = $("#MainDebitStatusEditForm").find("#RetriesNumber").val();

            SaveCouponStatus(rowModel, "U");

            $("#MainDebitStatusEditForm").formReset();
            $('#MainDebitStatusModalEdit').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").UifAlert('hide');
            }, 3000);
        }
    }
});

/// Tabla de estados débito: eliminar un registro
var debitStatusDeleteRowModel = new DebitStatusRowModel();

$('#tableDebitStatus').on('rowDelete', function (event, data) {
    $('#MainDebitStatusAlert').UifAlert('hide');
    $('#modalDeleteDebit').appendTo("body").modal('show');

    debitStatusDeleteRowModel.Id = data.Id;
    debitStatusDeleteRowModel.SmallDescription = data.SmallDescription;
    debitStatusDeleteRowModel.Description = data.Description;
    debitStatusDeleteRowModel.IsEnabled = data.IsEnabled;
    debitStatusDeleteRowModel.IsRetry = data.IsRetry;
    debitStatusDeleteRowModel.DebitStatusType = data.DebitStatusType;
    debitStatusDeleteRowModel.RetryDays = data.RetryDays;
});

$("#MainDebitStatusDeleteModal").on('click', function () {
    $('#modalDeleteDebit').modal('hide');

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/DeleteDebitStatusCodes",
        data: JSON.stringify({ "debitStatusModel": debitStatusDeleteRowModel, "operationType": "D" }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data[0].DebitStatusCode == 0) {
            $('#selectDebitStatusTable').trigger('change');
            $("#MainDebitStatusAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
        }
        else {
            $("#MainDebitStatusAlert").UifAlert('show', data[0].MessageError, "danger");
        }
        $('#modalDeleteDebit').UifModal('hide');

        setTimeout(function () {
            $("#MainDebitStatusAlert").UifAlert('hide');
        }, 3000);
    });
});

// CheckBox
$('span').click(function () {
    if ($("#ViewBagControlSpan").val() == "true") {
        if ($(this).hasClass("glyphicon glyphicon-unchecked")) {
            $(this).removeClass("glyphicon glyphicon-unchecked");
            $(this).addClass("glyphicon glyphicon-check");
        }
        else if ($(this).hasClass("glyphicon glyphicon-check")) {
            $(this).removeClass("glyphicon glyphicon-check");
            $(this).addClass("glyphicon glyphicon-unchecked");
        }
    }
});


/// Funciones
function SaveCouponStatus(rowModel, operationType) {

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/SaveDebitStatusCodes",
        data: JSON.stringify({ "debitStatusModel": rowModel, "operationType": operationType }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data[0].DebitStatusCode < 0) {
                $("#MainDebitStatusAlert").UifAlert('show', data[0].MessageError, "danger");
            }
            else {
                if (rowModel.Id == null) {
                    var tables = ACC_ROOT + "Networks/GetDebitStatusTables";
                    $("#selectDebitStatusTable").UifSelect({ source: tables });
                    rowModel.Id = 0;
                }

                $("#MainDebitStatusAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = ACC_ROOT + "Networks/GetDebitStatusCodesByTableId?tableId=" + rowModel.Id; //data[0].DebitStatusCode;
                $('#tableDebitStatus').UifDataTable({ source: controller });

                controller = ACC_ROOT + "Networks/GetDebitStatusTables";
                $('#selectDebitStatusTable').UifSelect({ source: controller, selectedId: rowModel.Id });

                setTimeout(function () {
                    $('#selectDebitStatusTable').val(rowModel.Id); // data[0].DebitStatusCode
                    $('#selectDebitStatusTable').trigger('change');
                }, 1000);
            }
            setTimeout(function () {
                $("#MainDebitStatusAlert").UifAlert('hide');
            }, 3000);
        }
    });
}

function ValidateAddForm() {
    if ($("#MainDebitStatusAddForm").find('#SmallDescription').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryCode, "warning");
        return false;
    }
    if ($("#MainDebitStatusAddForm").find('#Description').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
        return false;
    }

    return true;
}

function ValidateEditForm() {
    if ($("#MainDebitStatusEditForm").find('#Description').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
        return false;
    }
    if ($("#MainDebitStatusEditForm").find('#SmallDescription').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryCode, "warning");
        return false;
    }

    return true;
}
