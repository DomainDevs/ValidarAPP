var BankResponseCodeCurrentEditIndex = 0;
var BankResponseCodeIndex = 0;
var BankResponseCodeBankNetworkId = 0;
var BankResponseCodeTableId = 0;
var BankResponseEditModalPromise;

function BankResponseCodeRowModel() {
    this.Id;
    this.BankNetworkId;
    this.TableCode;
    this.NetworkDescription;
    this.RejectedCouponStatus;
    this.AcceptedCouponStatus;
}


/// Tabla de códigos respuesta banco: Agregar nuevo registro
$('#tableBankResponseCodes').on('rowAdd', function (event, data) {
    $("#BankResponseCodeEditForm").find("#selectNetwork").prop("disabled", false);
    $("#BankResponseCodeEditForm").find("#selectTable").prop("disabled", false);

    var controller = ACC_ROOT + "Networks/GetRejectionStatusCodesByTableId?tableId=0";
    $("#BankResponseCodeAddForm").find('#selectRejection').UifSelect({ source: controller })

    controller = ACC_ROOT + "Networks/GetAcceptedStatusCodesByTableId?tableId=0";
    $("#BankResponseCodeAddForm").find('#selectAccepted').UifSelect({ source: controller })

    $('#BankResponseCodeModalAdd').UifModal('showLocal', Resources.AddBankResponseCode);
});

$('#BankResponseCodeSaveAdd').click(function () {
    $("#BankResponseCodeAddForm").validate();

    if ($("#BankResponseCodeAddForm").valid()) {

        if (ValidateAddForm() == true) {
            var bankResponseCodeRowModel = new BankResponseCodeRowModel();

            bankResponseCodeRowModel.Id = 0;
            bankResponseCodeRowModel.BankNetworkId = $("#BankResponseCodeAddForm").find("#selectNetwork").val();
            bankResponseCodeRowModel.NetworkDescription = $("#BankResponseCodeAddForm").find("#selectNetwork option:selected").html();
            bankResponseCodeRowModel.TableCode = $("#BankResponseCodeAddForm").find("#selectTable").val();
            bankResponseCodeRowModel.RejectedCouponStatus = $("#BankResponseCodeAddForm").find("#selectRejection").val();
            bankResponseCodeRowModel.AcceptedCouponStatus = $("#BankResponseCodeAddForm").find("#selectAccepted").val();

            SaveBankNetworkStatus(bankResponseCodeRowModel, "I");

            $("#BankResponseCodeAddForm").formReset();
            $('#BankResponseCodeModalAdd').UifModal('hide');
        }

    }
});

$('#BankResponseCodeCancelAdd').click(function () {
    $('#selectNetwork').val("");
    $('#selectTable').val("");
    $('#selectRejection').val("");
    $('#selectAccepted').val("");
});

/// Tabla de códigos respuesta banco: editar un registro
$('#tableBankResponseCodes').on('rowEdit', function (event, data, position) {

    BankResponseCodeCurrentEditIndex = position;

    BankResponseCodeBankNetworkId = data.Id;
    BankResponseCodeTableId = data.TableCode;

    $("#BankResponseCodeEditForm").find("#selectNetwork").prop("disabled", true);
    $("#BankResponseCodeEditForm").find("#selectTable").prop("disabled", true);
    $("#BankResponseCodeEditForm").find("#selectNetwork").val(data.Id);
    $("#BankResponseCodeEditForm").find("#selectTable").val(data.TableCode);

    var rejectionController = ACC_ROOT + "Networks/GetRejectionStatusCodesByTableId?tableId=" + data.TableCode;
    $("#BankResponseCodeEditForm").find('#selectRejection').UifSelect({ source: rejectionController, selectedId: data.RejectionDefaultCode })

    var acceptedController = ACC_ROOT + "Networks/GetAcceptedStatusCodesByTableId?tableId=" + data.TableCode;
    $("#BankResponseCodeEditForm").find('#selectAccepted').UifSelect({ source: acceptedController, selectedId: String(data.AcceptedDefaultCode).trim() })
    $('#BankResponseCodeModalEdit').UifModal('showLocal', Resources.EditBankResponseCode);

    loadBankResponseEditModal();
    BankResponseEditModalPromise.then(function () {
        $("#BankResponseCodeEditForm").find("#selectAccepted").val(data.AcceptedDefaultCode);
        $("#BankResponseCodeEditForm").find('#selectRejection').val(data.RejectionDefaultCode);
    });
});

$('#BankResponseCodeSaveEdit').click(function () {
    $("#BankResponseCodeEditForm").validate();

    if ($("#BankResponseCodeEditForm").valid()) {

        if (ValidateEditForm() == true) {
            var bankResponseCodeRowModel = new BankResponseCodeRowModel();

            bankResponseCodeRowModel.Id = BankResponseCodeBankNetworkId;
            bankResponseCodeRowModel.BankNetworkId = $("#BankResponseCodeEditForm").find("#selectNetwork").val();
            bankResponseCodeRowModel.NetworkDescription = $("#BankResponseCodeEditForm").find("#selectNetwork option:selected").html();
            bankResponseCodeRowModel.TableCode = $("#BankResponseCodeEditForm").find("#selectTable").val();
            bankResponseCodeRowModel.RejectedCouponStatus = $("#BankResponseCodeEditForm").find("#selectRejection").val();
            bankResponseCodeRowModel.AcceptedCouponStatus = $("#BankResponseCodeEditForm").find("#selectAccepted").val();

            SaveBankNetworkStatus(bankResponseCodeRowModel, "U");

            $("#BankResponseCodeEditForm").formReset();
            $('#BankResponseCodeModalEdit').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").UifAlert('hide');
            }, 3000);
        }
    }
});

/// Tabla de códigos respuesta banco: eliminar un registro
var deleteBankResponseCodeRowModel = new BankResponseCodeRowModel();
$('#tableBankResponseCodes').on('rowDelete', function (event, data) {
    $('#BankResponseCodeAlert').UifAlert('hide');
    $('#modalDeleteResponse').appendTo("body").modal('show');

    deleteBankResponseCodeRowModel.Id = data.Id;
    deleteBankResponseCodeRowModel.BankNetworkId = data.Id;
    deleteBankResponseCodeRowModel.NetworkDescription = data.Description;
    deleteBankResponseCodeRowModel.TableCode = data.TableCode;
    deleteBankResponseCodeRowModel.RejectedCouponStatus = data.RejectionDefaultCode;
    deleteBankResponseCodeRowModel.AcceptedCouponStatus = data.AcceptedDefaultCode;
});

$("#BankResponseCodeDeleteModal").on('click', function () {
    $('#modalDeleteResponse').modal('hide');

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/DeleteBankResponseCodes",
        data: JSON.stringify({ "responseStatusModel": deleteBankResponseCodeRowModel, "operationType": "D" }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data[0].BankResponseStatusCode == 0) {
            var controller = ACC_ROOT + "Networks/GetBankNetworkStatus";
            $('#tableBankResponseCodes').UifDataTable({ source: controller })
            $("#BankResponseCodeAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
        }
        else {
            $("#BankResponseCodeAlert").UifAlert('show', data[0].MessageError, "danger");
        }
        $('#modalDeleteResponse').UifModal('hide');

        setTimeout(function () {
            $("#BankResponseCodeAlert").find('.close').click();
        }, 3000);
    });
});

/// Combo tabla estado débito rechazados
$("#BankResponseCodeAddForm").find('#selectTable').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Networks/GetRejectionStatusCodesByTableId?tableId=" + $("#BankResponseCodeAddForm").find('#selectTable').val();
        $("#BankResponseCodeAddForm").find('#selectRejection').UifSelect({ source: controller })

        controller = ACC_ROOT + "Networks/GetAcceptedStatusCodesByTableId?tableId=" + $("#BankResponseCodeAddForm").find('#selectTable').val();
        $("#BankResponseCodeAddForm").find('#selectAccepted').UifSelect({ source: controller })
    }
    else {
        $("#BankResponseCodeAddForm").find('#selectRejection').UifSelect();
        $("#BankResponseCodeAddForm").find('#selectAccepted').UifSelect();
    }
});

$("#BankResponseCodeEditForm").find('#selectTable').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Networks/GetRejectionStatusCodesByTableId?tableId=" + $("#BankResponseCodeEditForm").find('#selectTable').val();
        $("#BankResponseCodeEditForm").find('#selectRejection').UifSelect({ source: controller })

        controller = ACC_ROOT + "Networks/GetAcceptedStatusCodesByTableId?tableId=" + $("#BankResponseCodeEditForm").find('#selectTable').val();
        $("#BankResponseCodeEditForm").find('#selectAccepted').UifSelect({ source: controller })
    }
    else {
        $("#BankResponseCodeEditForm").find('#selectRejection').UifSelect();
        $("#BankResponseCodeEditForm").find('#selectAccepted').UifSelect();
    }
});



/// Funciones
function SaveBankNetworkStatus(rowModel, operationType) {

    var valid = true;

    if (operationType != "U") {
        var ids = $("#tableBankResponseCodes").UifDataTable("getData");
        if (ids.length > 0) {
            for (var i in ids) {
                if (ids[i].Id == rowModel.BankNetworkId && ids[i].TableCode == rowModel.TableCode) {
                    valid = false;
                    break;
                }
            }
        }
    }   

    if (valid) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Networks/SaveBankResponseCodes",
            data: JSON.stringify({ "responseStatusModel": rowModel, "operationType": operationType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data[0].BankResponseStatusCode < 0) {
                    $("#BankResponseCodeAlert").UifAlert('show', data[0].MessageError, "danger");
                }
                else {
                    var controller = ACC_ROOT + "Networks/GetBankNetworkStatus";
                    $('#tableBankResponseCodes').UifDataTable({ source: controller })
                    $("#BankResponseCodeAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                }
                setTimeout(function () {
                    $("#BankResponseCodeAlert").UifAlert('hide');
                }, 3000);
            }
        });
    } else {
        $("#BankResponseCodeAlert").UifAlert('show', Resources.DuplicatedRecord, "danger");
    }    
}

function ValidateAddForm() {
    if ($("#BankResponseCodeAddForm").find('#selectNetwork').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectNet, "warning");
        return false;
    }
    if ($("#BankResponseCodeAddForm").find('#selectTable').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectTableCode, "warning");
        return false;
    }
    if ($("#BankResponseCodeAddForm").find('#selectRejection').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectRejectionDefaultCode, "warning");
        return false;
    }
    if ($("#BankResponseCodeAddForm").find('#selectAccepted').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectRejectionDefaultCode, "warning");
        return false;
    }

    return true;
}

function ValidateEditForm() {
    if ($("#BankResponseCodeEditForm").find('#selectNetwork').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectNet, "warning");
        return false;
    }
    if ($("#BankResponseCodeEditForm").find('#selectTable').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectTableCode, "warning");
        return false;
    }
    if ($("#BankResponseCodeEditForm").find('#selectRejection').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectRejectionDefaultCode, "warning");
        return false;
    }
    if ($("#BankResponseCodeEditForm").find('#selectAccepted').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectRejectionDefaultCode, "warning");
        return false;
    }

    return true;
}

//comprueba que el modal de edición se haya levantado.
function loadBankResponseEditModal() {
    return BankResponseEditModalPromise = new Promise(function (resolve, reject) {
        var time = setInterval(function () {
            if (($("#BankResponseCodeModalEdit").data('bs.modal') || {}).isShown) {
                window.clearInterval(time);
                resolve(true);
            }
        }, 100);
    });
}