var statusBankEnabled = false;
var isEdit = false;
var bankId = 0;

$(document).ready(function () {
    // BOTON ACEPTAR DEL DIALOGO
    $("#ModalBankBranch").find("#SaveBankBranchModal").click(function () {

        $("#ModalBankBranch").find("#BankBranchForm").validate();
        if ($("#ModalBankBranch").find("#BankBranchForm").valid()) {

            isEnabled();
            if (!isEdit) {
                bankId = $("#ModalBankBranch").find("#BankBranchComboModal").val();
            }

            $.ajax({
                url: ACC_ROOT + "Parameters/SaveBankBranch",
                data: { "branchId": $('#ComboBranch').val(), "bankId": bankId, "isEnabled": statusBankEnabled, "isEdit": isEdit },
                success: function (data) {

                    if (data > 0) {

                        $("#alertBankBranch").UifAlert('show', Resources.SaveSuccessfully, "success");

                        refreshBankBranch($('#ComboBranch').val());
                        $("#ModalBankBranch").modal('hide');
                        $("#ModalBankBranch").find("#alertBankBranchModal").UifAlert('hide');

                    } else {
                        $("#ModalBankBranch").find("#alertBankBranchModal").UifAlert('show', Resources.BankExist, "warning");
                    }
                }
            });
        }
    });

    //COMBO DEL DIALOGO
    $("#ModalBankBranch").find('#BankBranchComboModal').on('itemSelected', function (event, selectedItem) {
        $("#ModalBankBranch").find("#alertBankBranchModal").UifAlert('hide');
    });
});

//COMBO DE SUCURSAL
$('#ComboBranch').on('itemSelected', function (event, selectedItem) {

    $("#alertBankBranch").UifAlert('hide');
    if ($('#ComboBranch').val() > 0) {
        refreshBankBranch(selectedItem.Id);
    } else {
        refreshBankBranch(0);
    }
});

//Levanta modal selecciona banco
$('#TableBank').on('rowAdd', function (event, data) {

    $("#alertBankBranch").UifAlert('hide');
    $("#ModalBankBranch").find("#alertBankBranchModal").UifAlert('hide');
    isEdit = false;
    $("#ModalBankBranch").find("#BankBranchComboModal").attr("disabled", false);
    $("#ModalBankBranch").find("#BankBranchComboModal").val("");

    if ($('#ComboBranch').val() > 0) {
        $("#alertBankBranch").UifAlert('hide');
        $('#ModalBankBranch').UifModal('showLocal', '');
    } else {
        $("#alertBankBranch").UifAlert('show', Resources.SelectBranch, "warning");
    }

});

//Levanta modal selecciona banco
$('#TableBank').on('rowEdit', function (event, data) {
    $("#alertBankBranch").UifAlert('hide');
    $("#ModalBankBranch").find("#alertBankBranchModal").UifAlert('hide');
    isEdit = true;
    bankId = data.Id;
    $("#ModalBankBranch").find("#BankBranchComboModal").val(data.Id);
    $("#ModalBankBranch").find("#BankBranchComboModal").attr("disabled", true);

    if (data.IsEnabled) {
        $("#CheckEnableBank").prop("checked", true);
    } else {
        $("#CheckEnableBank").prop("checked", false);
    }
    //$('#ModalBankBranch').appendTo("body").modal('show');
    $('#ModalBankBranch').UifModal('showLocal', '');
});

//Elimina selecciona banco
$('#TableBank').on('rowDelete', function (event, data) {
    $("#alertBankBranch").UifAlert('hide');
    $("#ModalBankBranch").find("#alertBankBranchModal").UifAlert('hide');

    showConfirmMainAssociate(data.Id);
});


// BOTON CANCELAR DEL DIALOGO
$("#CancelSaveBankBranchModal").click(function () {

    $("#ModalBankBranch").modal('hide');
});


//************************************************************************************************************//
//     FUNCIONES

function isEnabled() {

    if ($("#CheckEnableBank").is(":checked")) {

        statusBankEnabled = true;
    } else {
        statusBankEnabled = false;
    }
}


function refreshBankBranch(branchId) {

    var controller = ACC_ROOT + "Parameters/GetBankBranchsByBranchId?branchId=" + branchId;
    $("#TableBank").UifDataTable({ source: controller });
}

//Eliminar registro
function deleteBankBranch(bankId) {

    $.ajax({
        url: ACC_ROOT + "Parameters/DeleteBankBranch",
        data: { "branchId": $('#ComboBranch').val(), "bankId": bankId },
        success: function () {
            refreshBankBranch($('#ComboBranch').val());
            $("#alertBankBranch").UifAlert('show', Resources.DeleteSucces, "success");
        }
    });
}

//Confirmación de borrado
function showConfirmMainAssociate (bankId) {
    $.UifDialog('confirm', {
        'message': Resources.DeletedMultipleRecords,
        'title': Resources.Parameter
    }, function (result) {
        if (result) {
            deleteBankBranch(bankId);
        }
    });
};
