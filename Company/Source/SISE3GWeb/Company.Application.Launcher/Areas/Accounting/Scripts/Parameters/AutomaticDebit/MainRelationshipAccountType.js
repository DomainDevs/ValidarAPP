var RelationshipAccountTypeCurrentEditIndex = 0;
var RelationshipAccountTypeIndex = 0;
var RelationshipAccountTypePaymentMethodId = 0;
var RelationshipAccountTypeAccountTypeId = 0;

function RelationshipAccountTypeRowModel() {
    this.Id;
    this.Description;
    this.AccountTypeId;
    this.AccountTypeDescription;
    this.DebitCode;
}

/////////////////////////////////////////////////////////////////////////
/// Tabla de relación conducto tipo de cuenta: Agregar nuevo registro ///
/////////////////////////////////////////////////////////////////////////
$('#tableRelationAccountType').on('rowAdd', function (event, data) {
    // Enable #selectPaymentMethod
    $("#RelationshipAccountTypeEditForm").find("#selectPaymentMethod").prop("disabled", false);
    $("#RelationshipAccountTypeEditForm").find("#selectAccountType").prop("disabled", false);

    $('#RelationshipAccountTypeModalAdd').UifModal('showLocal', Resources.AddAccountTypeRelationship);
});

$('#RelationshipAccountTypeSaveAdd').click(function () {
    $("#RelationshipAccountTypeAddForm").validate();

    if ($("#RelationshipAccountTypeAddForm").valid()) {

        if (ValidateAddForm() == true) {
            var rowModel = new RelationshipAccountTypeRowModel();

            rowModel.Id = $("#RelationshipAccountTypeAddForm").find("#selectPaymentMethod").val();
            rowModel.Description = $("#RelationshipAccountTypeAddForm").find("#selectPaymentMethod option:selected").html();
            rowModel.AccountTypeId = $("#RelationshipAccountTypeAddForm").find("#selectAccountType").val();
            rowModel.AccountTypeDescription = $("#RelationshipAccountTypeAddForm").find("#selectAccountType option:selected").html();
            rowModel.DebitCode = $("#RelationshipAccountTypeAddForm").find("#DebitCode").val();

            SavePaymentMethodAccountType(rowModel, "I");

            $("#RelationshipAccountTypeAddForm").formReset();
            $('#RelationshipAccountTypeModalAdd').UifModal('hide');
        }

    }
});

$('#RelationshipAccountTypeCancelAdd').click(function () {
    $("#RelationshipAccountTypeAddForm").find('#selectPaymentMethod').val("");
    $("#RelationshipAccountTypeAddForm").find('#selectAccountType').val("");
    $("#RelationshipAccountTypeAddForm").find('#DebitCode').val("");
});

/// Tabla de relación conducto tipo de cuenta: editar un registro
$('#tableRelationAccountType').on('rowEdit', function (event, data, position) {
    RelationshipAccountTypeCurrentEditIndex = position;

    RelationshipAccountTypePaymentMethodId = data.Id;
    RelationshipAccountTypeAccountTypeId = data.AccountTypeId;

    // Disable #selectPaymentMethod
    $("#RelationshipAccountTypeEditForm").find("#selectPaymentMethod").prop("disabled", true);
    $("#RelationshipAccountTypeEditForm").find("#selectAccountType").prop("disabled", true);
    $("#RelationshipAccountTypeEditForm").find("#selectPaymentMethod").val(data.Id);
    $("#RelationshipAccountTypeEditForm").find("#selectAccountType").val(data.AccountTypeId);
    $("#RelationshipAccountTypeEditForm").find("#DebitCode").val(data.DebitCode);

    $('#RelationshipAccountTypeModalEdit').UifModal('showLocal', Resources.EditAccountTypeRelationship);
});

$('#RelationshipAccountTypeSaveEdit').click(function () {
    $("#RelationshipAccountTypeEditForm").validate();

    if ($("#RelationshipAccountTypeEditForm").valid()) {

        if (ValidateEditForm() == true) {
            var rowModel = new RelationshipAccountTypeRowModel();

            rowModel.Id = RelationshipAccountTypePaymentMethodId;
            rowModel.Description = $("#RelationshipAccountTypeEditForm").find("#selectPaymentMethod option:selected").html();
            rowModel.AccountTypeId = RelationshipAccountTypeAccountTypeId;
            rowModel.AccountTypeDescription = $("#RelationshipAccountTypeEditForm").find("#selectAccountType option:selected").html();
            rowModel.DebitCode = $("#RelationshipAccountTypeEditForm").find("#DebitCode").val();

            SavePaymentMethodAccountType(rowModel, "U");

            //$('#tableRelationAccountType').UifDataTable('editRow', rowModel, RelationshipAccountTypeCurrentEditIndex);
            $("#RelationshipAccountTypeEditForm").formReset();
            $('#RelationshipAccountTypeModalEdit').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").UifAlert('hide');
            }, 3000);
        }
    }
});

/// Tabla de relación conducto tipo de cuenta: eliminar un registro
var relationshipAccountTypeDeleteRowModel = new RelationshipAccountTypeRowModel();
$('#tableRelationAccountType').on('rowDelete', function (event, data) {
    $('#RelationshipAccountTypeAlert').UifAlert('hide');
    $('#modalDeleteAccountType').appendTo("body").modal('show');

    relationshipAccountTypeDeleteRowModel.Id = data.Id;
    relationshipAccountTypeDeleteRowModel.Description = data.Description;
    relationshipAccountTypeDeleteRowModel.AccountTypeId = data.AccountTypeId;
    relationshipAccountTypeDeleteRowModel.AccountTypeDescription = data.AccountTypeDescription;
    relationshipAccountTypeDeleteRowModel.DebitCode = data.DebitCode;
});

$("#RelationshipAccountTypeDeleteModal").on('click', function () {
    $('#modalDeleteAccountType').modal('hide');

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/DeletePaymentMethodAccountType",
        data: JSON.stringify({ "paymentMethodModel": relationshipAccountTypeDeleteRowModel, "operationType": "D" }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data[0].PaymentMethodAccountTypeCode == 0) {
            var controller = ACC_ROOT + "Networks/GetPaymentMethodAccountTypes";
            $('#tableRelationAccountType').UifDataTable({ source: controller })
            $("#RelationshipAccountTypeAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
        }
        else {
            $("#RelationshipAccountTypeAlert").UifAlert('show', data[0].MessageError, "danger");
        }
        $('#modalDeleteAccountType').UifModal('hide');

        setTimeout(function () {
            $("#RelationshipAccountTypeAlert").find('.close').click();
        }, 3000);
    });
});


/// Funciones 
function SavePaymentMethodAccountType(rowModel, operationType) {

    var valid = true;

    if (operationType != "U") {
        var ids = $("#tableRelationAccountType").UifDataTable("getData");
        if (ids.length > 0) {
            for (var i in ids) {
                if (ids[i].Id == rowModel.Id && ids[i].AccountTypeId == rowModel.AccountTypeId) {
                    valid = false;
                    break;
                }
            }
        }
    }

    if (valid) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Networks/SavePaymentMethodAccountType",
            data: JSON.stringify({ "paymentMethodModel": rowModel, "operationType": operationType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data[0].PaymentMethodAccountTypeCode < 0) {
                    $("#RelationshipAccountTypeAlert").UifAlert('show', data[0].MessageError, "danger");
                }
                else {
                    var controller = ACC_ROOT + "Networks/GetPaymentMethodAccountTypes";
                    $('#tableRelationAccountType').UifDataTable({ source: controller })
                    $("#RelationshipAccountTypeAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                }
                setTimeout(function () {
                    $("#RelationshipAccountTypeAlert").UifAlert('hide');
                }, 3000);
            }
        });
    } else {
        $("#RelationshipAccountTypeAlert").UifAlert('show', Resources.DuplicatedRecord, "danger");
    }
    
}

function ValidateAddForm() {
    if ($("#RelationshipAccountTypeAddForm").find('#selectPaymentMethod').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectConduit, "warning");
        return false;
    }
    if ($("#RelationshipAccountTypeAddForm").find('#selectAccountType').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectAccountType, "warning");
        return false;
    }
    if ($("#RelationshipAccountTypeAddForm").find('#DebitCode').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryDebitCode, "warning");
        return false;
    }

    return true;
}

function ValidateEditForm() {
    if ($("#RelationshipAccountTypeEditForm").find('#selectPaymentMethod').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectConduit, "warning");
        return false;
    }
    if ($("#RelationshipAccountTypeEditForm").find('#selectAccountType').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectAccountType, "warning");
        return false;
    }
    if ($("#RelationshipAccountTypeEditForm").find('#DebitCode').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryDebitCode, "warning");
        return false;
    }

    return true;
}