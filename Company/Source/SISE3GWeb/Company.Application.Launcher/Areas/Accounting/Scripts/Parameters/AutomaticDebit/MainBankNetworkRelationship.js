var networkRelationshipCurrentEditIndex = 0;
var networkRelationshipIndex = 0;
var networkRelationBankNetworkId = 0;
var networkRelationPaymentMethodId = 0;
var networkRelationGenerated = 0;

function NetworkRelationRowModel() {
    this.Id;
    this.PaymentMethodId;
    this.AccountBankId;
    this.ToGenerate;
}


$('.glyphy').click(function (e) {
    if ($(e.target).is('input')) {
        $(this).find('.glyphicon').toggleClass('glyphicon-check glyphicon-unchecked');
        console.log($(this).find('input').is(':checked'));
        networkRelationGenerated = $(this).find('input').is(':checked');
    }
});


/// Combo red 
$('#NetworkRelationshipSelectBankNetwork').on('itemSelected', function (event, selectedItem) {
    $("#tableNetworkRelation").UifDataTable();
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Networks/GetBankNetworkRelationshipByBankNetworkId?bankNetworkId=" + selectedItem.Id;
        $('#tableNetworkRelation').UifDataTable({ source: controller })
    }
    else {
        $('#tableNetworkRelation').dataTable().fnClearTable();
    }
});

$("#networkRelationshipAddForm").find("#selectPaymentMethod").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
})


/// Tabla de relaciones de redes y conductos: Agregar nuevo registro
$('#tableNetworkRelation').on('rowAdd', function (event, data) {
    $("#alertForm").UifAlert('hide');
    $("#alert").UifAlert('hide');
    $("#networkRelationshipEditForm").find("#selectPaymentMethod").prop("disabled", false);

    if ($("#NetworkRelationshipSelectBankNetwork").val() != "") {
        $('#networkRelationshipModalAdd').UifModal('showLocal', Resources.AddBankNetworkRelationship);
    }
    else {
        $("#bankNetRelationshipForm").validate();
        $("#bankNetRelationshipForm").valid();
    }
});

$('#networkRelationshipSaveAdd').click(function () {
    $("#networkRelationshipAddForm").validate();

    if ($("#networkRelationshipAddForm").valid()) {

        if (validateExistsPaymentMethod($("#networkRelationshipAddForm").find("#selectPaymentMethod").val()) == false) {
            var networkRelationRowModel = new NetworkRelationRowModel();

            networkRelationRowModel.Id = $("#NetworkRelationshipSelectBankNetwork").val();
            networkRelationRowModel.PaymentMethodId = $("#networkRelationshipAddForm").find("#selectPaymentMethod").val();
            networkRelationRowModel.AccountBankId = $("#networkRelationshipAddForm").find("#selectAccountBank").val();
            if ($("#networkRelationshipAddForm").find('#checkGenerate').hasClass("glyphicon glyphicon-unchecked")) {
                networkRelationRowModel.ToGenerate = 0;
            }
            else if ($("#networkRelationshipAddForm").find('#checkGenerate').hasClass("glyphicon glyphicon-check")) {
                networkRelationRowModel.ToGenerate = 1;
            }

            SavePaymentMethodBankNetwork(networkRelationRowModel, "I");

            $("#networkRelationshipAddForm").formReset();
            $('#networkRelationshipModalAdd').UifModal('hide');
        }
        else {
            $("#alertForm").UifAlert('show', Resources.AlreadyExistsPaymentMethod, "warning");
        }
    }
});

$('#networkRelationshipCancelAdd').click(function () {
    $('#selectPaymentMethod').val("");
    $('#selectAccountBank').val("");
});

/// Tabla de relaciones de redes y conductos: editar un registro
$('#tableNetworkRelation').on('rowEdit', function (event, data, position) {
    networkRelationshipCurrentEditIndex = position;
    networkRelationBankNetworkId = data.Id;
    networkRelationPaymentMethodId = data.PaymentMethodId;

    // Disable #selectPaymentMethod
    $("#networkRelationshipEditForm").find("#selectPaymentMethod").prop("disabled", true);

    $("#networkRelationshipEditForm").find("#selectPaymentMethod").val(data.PaymentMethodId);
    $("#networkRelationshipEditForm").find("#selectAccountBank").val(data.AccountBankId);

    var $checkBox = $("#networkRelationshipEditForm").find("#GenerateCheck");

    if (data.Generate == true) {
        $("#networkRelationshipEditForm").find("#checkGenerate").removeClass("glyphicon glyphicon-unchecked");
        $("#networkRelationshipEditForm").find("#checkGenerate").addClass("glyphicon glyphicon-check");
        networkRelationGenerated = 1;
    }
    else {
        $("#networkRelationshipEditForm").find("#checkGenerate").removeClass("glyphicon glyphicon-check");
        $("#networkRelationshipEditForm").find("#checkGenerate").addClass("glyphicon glyphicon-unchecked");
        networkRelationGenerated = 0;
    }

    $('#networkRelationshipModalEdit').UifModal('showLocal', Resources.EditBankNetworkRelationship);
});

$('#networkRelationshipSaveEdit').click(function () {
    $("#networkRelationshipEditForm").validate();

    if ($("#networkRelationshipEditForm").valid()) {

        if (ValidateEditForm() == true) {
            var networkRelationRowModel = new NetworkRelationRowModel();

            networkRelationRowModel.Id = networkRelationBankNetworkId;
            networkRelationRowModel.PaymentMethodId = $("#networkRelationshipEditForm").find("#selectPaymentMethod").val();
            networkRelationRowModel.AccountBankId = $("#networkRelationshipEditForm").find("#selectAccountBank").val();
            if ($("#networkRelationshipEditForm").find('#checkGenerate').hasClass("glyphicon glyphicon-unchecked")) {
                networkRelationRowModel.ToGenerate = 0;
            }
            else if ($("#networkRelationshipEditForm").find('#checkGenerate').hasClass("glyphicon-check glyphicon")) {
                networkRelationRowModel.ToGenerate = 1;
            }

            SavePaymentMethodBankNetwork(networkRelationRowModel, "U");

            $("#networkRelationshipEditForm").formReset();
            $('#networkRelationshipModalEdit').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").UifAlert('hide');
            }, 3000);
        }
    }
});

/// Tabla de relacones de redes y conductos: eliminar un registro
var networkRelationDeleteRowModel = new NetworkRelationRowModel();
$('#tableNetworkRelation').on('rowDelete', function (event, data) {
    $('#alert').UifAlert('hide');
    $('#modalDeleteRelation').appendTo("body").modal('show');

    networkRelationDeleteRowModel.Id = data.Id;
    networkRelationDeleteRowModel.PaymentMethodId = data.PaymentMethodId;
    networkRelationDeleteRowModel.AccountBankId = data.AccountBankId;
    networkRelationDeleteRowModel.ToGenerate = data.Generate;
});

$("#relationDeleteModal").on('click', function () {
    $('#modalDeleteRelation').modal('hide');

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/DeletePaymentMethodBankNetwork",
        data: JSON.stringify({ "paymentMethodModel": networkRelationDeleteRowModel, "operationType": "D" }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data[0].PaymentMethodBankNetworkCode == 0) {
            $("#alert").UifAlert('show', Resources.DeleteSuccessfully, "success");
            var controller = ACC_ROOT + "Networks/GetBankNetworkRelationshipByBankNetworkId?bankNetworkId=" + networkRelationDeleteRowModel.Id;
            $('#tableNetworkRelation').UifDataTable({ source: controller })
        }
        else {
            $("#alert").UifAlert('show', data[0].MessageError, "danger");
        }
        $('#modalDeleteRelation').UifModal('hide');

        setTimeout(function () {
            $("#alert").find('.close').click();
        }, 3000);
    });
});

// CheckBox

$('span').click(function () {
    if ($("#ViewBagControlSpanBankNetwork").val() == "true") {
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

$("#networkRelationshipEditForm").find('#checkGenerate').click(function () {
    if (networkRelationGenerated == 1) {
        $("#networkRelationshipEditForm").find('#checkGenerate').removeClass("glyphicon glyphicon-check");
        $("#networkRelationshipEditForm").find('#checkGenerate').addClass("glyphicon glyphicon-unchecked");
        networkRelationGenerated = 0;
    }
    else if (networkRelationGenerated == 0) {
        $("#networkRelationshipEditForm").find('#checkGenerate').removeClass("glyphicon glyphicon-unchecked");
        $("#networkRelationshipEditForm").find('#checkGenerate').addClass("glyphicon glyphicon-check");
        networkRelationGenerated = 1;
    }
});

/// Funciones
function SavePaymentMethodBankNetwork(rowModel, operationType) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/SavePaymentMethodBankNetwork",
        data: JSON.stringify({ "paymentMethodModel": rowModel, "operationType": operationType }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data[0].PaymentMethodBankNetworkCode < 0) {
                $("#alert").UifAlert('show', data[0].MessageError, "danger");
            }
            else {
                $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
                var controller = ACC_ROOT + "Networks/GetBankNetworkRelationshipByBankNetworkId?bankNetworkId=" + rowModel.Id;
                $('#tableNetworkRelation').UifDataTable({ source: controller })
            }
            setTimeout(function () {
                $("#alert").UifAlert('hide');
            }, 3000);
        }
    });
}

function validateExistsPaymentMethod(id) {
    var exists = false;
    var fields = $("#tableNetworkRelation").UifDataTable("getData");
    if (fields.length > 0) {
        for (var j = 0; j < fields.length; j++) {
            if (id == fields[j].PaymentMethodId) {
                exists = true;
                break;
            }
        }
    }
    return exists
}

function ValidateEditForm() {
    if ($("#networkRelationshipEditForm").find('#selectPaymentMethod').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectConduit, "warning");
        return false;
    }
    if ($("#networkRelationshipEditForm").find('#selectAccountBank').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectBank, "warning");
        return false;
    }

    return true;
}