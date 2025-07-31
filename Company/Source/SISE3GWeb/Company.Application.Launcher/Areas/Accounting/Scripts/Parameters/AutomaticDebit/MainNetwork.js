var currentEditIndex = 0;
var index = 0;
var bankNetworkId = 0;
var itHasTax = 0;
var generated = 0;

function RowModel() {
    this.Id;
    this.Description;
    this.HasTax;
    this.TaxTypeId;
    this.TaxTypeDescription;
    this.CommissionValue; 
    this.RetriesNumber;
    this.RequiresNotification;
}

/// Tabla de redes: Agregar nuevo registro 
$('#tableNetwork').on('rowAdd', function (event, data) {
    $('#MainNetworkModalAdd').UifModal('showLocal', Resources.AddNetwork);
});

$('#MainNetworkSaveAdd').click(function () {
    $("#MainNetworkAddForm").validate();

    var commissionLocal = $("#MainNetworkAddForm").find("#Commission_LocalAmount").val();
    if (commissionLocal != "") {
        $("#MainNetworkAddForm").find("#Commission_LocalAmount").val(parseFloat(ClearFormatCurrency(commissionLocal)));
    }

    if ($("#MainNetworkAddForm").valid()) {

        if (ValidateAddForm() == true)
        {
            var rowModel = new RowModel();

            rowModel.Id = 0;
            rowModel.Description = $("#MainNetworkAddForm").find("#Description").val();
            rowModel.TaxTypeId = $("#MainNetworkAddForm").find("#selectTaxCategory").val();
            rowModel.TaxTypeDescription = $("#MainNetworkAddForm").find("#selectTaxCategory option:selected").html();
            rowModel.CommissionValue = ReplaceDecimalPoint($("#MainNetworkAddForm").find("#Commission_LocalAmount").val());
            rowModel.RetriesNumber = $("#MainNetworkAddForm").find("#RetriesNumber").val();
            if ($("#MainNetworkAddForm").find('#checkHasTax').hasClass("glyphicon glyphicon-unchecked")) {
                rowModel.HasTax = 0;
            }
            else if ($("#MainNetworkAddForm").find('#checkHasTax').hasClass("glyphicon glyphicon-check")) {
                rowModel.HasTax = 1;
            }
            if ($("#MainNetworkAddForm").find('#checkRequiresNotification').hasClass("glyphicon glyphicon-unchecked")) {
                rowModel.RequiresNotification = 0;
            }
            else if ($("#MainNetworkAddForm").find('#checkRequiresNotification').hasClass("glyphicon glyphicon-check")) {
                rowModel.RequiresNotification = 1;
            }

            SaveBankNetwork(rowModel, "I");

            $("#MainNetworkAddForm").formReset();
            $('#MainNetworkModalAdd').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").UifAlert('hide');
            }, 3000);
        }
    }
});

/// Tabla de redes: editar un registro
$('#tableNetwork').on('rowEdit', function (event, data, position) {
    currentEditIndex = position;

    bankNetworkId = data.Id;
    $("#MainNetworkEditForm").find("#Description").val(data.Description);
    $("#MainNetworkEditForm").find("#selectTaxCategory").val(data.TaxCategoryId);
    $("#MainNetworkEditForm").find("#Commission_LocalAmount").val(data.CommissionValue);
    $("#MainNetworkEditForm").find("#RetriesNumber").val(data.RetriesNumber);

    if (data.IsHasTax == true) {
        $("#MainNetworkEditForm").find("#checkHasTax").removeClass("glyphicon glyphicon-unchecked");
        $("#MainNetworkEditForm").find("#checkHasTax").addClass("glyphicon glyphicon-check");
        itHasTax = 1;
    }
    else {
        $("#MainNetworkEditForm").find("#checkHasTax").removeClass("glyphicon glyphicon-check");
        $("#MainNetworkEditForm").find("#checkHasTax").addClass("glyphicon glyphicon-unchecked");
        itHasTax = 0;
    }

    if (data.PreNotifications == true) {
        $("#MainNetworkEditForm").find("#checkRequiresNotification").removeClass("glyphicon glyphicon-unchecked");
        $("#MainNetworkEditForm").find("#checkRequiresNotification").addClass("glyphicon glyphicon-check");
        generated = 1;
    }
    else {
        $("#MainNetworkEditForm").find("#checkRequiresNotification").removeClass("glyphicon glyphicon-check");
        $("#MainNetworkEditForm").find("#checkRequiresNotification").addClass("glyphicon glyphicon-unchecked");
        generated = 0;
    }

    $('#MainNetworkModalEdit').UifModal('showLocal', Resources.EditNetwork);
});

$("#MainNetworkEditForm").find("#Commission_LocalAmount").on('blur', function (event) {
    $("#alertForm").UifAlert('hide');
    var localAmount = $("#MainNetworkEditForm").find("#Commission_LocalAmount").val();
    if (localAmount != "") {
        var commision = ClearFormatCurrency(localAmount);
        $("#MainNetworkEditForm").find("#Commission_LocalAmount").val("$ " + NumberFormatSearch(commision, "2", ".", ","));

    }
});


$("#MainNetworkModalAdd").find("#Commission_LocalAmount").on('blur', function (event) {
    $("#alertForm").UifAlert('hide');
    var localAmount = $("#MainNetworkModalAdd").find("#Commission_LocalAmount").val();
    if (localAmount != "") {
        var commision = ClearFormatCurrency(localAmount);
        $("#MainNetworkModalAdd").find("#Commission_LocalAmount").val("$ " + NumberFormatSearch(commision, "2", ".", ","));

    }
});


$('#MainNetworkSaveEdit').click(function () {
    $("#MainNetworkEditForm").validate();

    var commissionLocal = $("#MainNetworkEditForm").find("#Commission_LocalAmount").val();
    if (commissionLocal != "") {
        $("#MainNetworkEditForm").find("#Commission_LocalAmount").val(parseFloat(ClearFormatCurrency(commissionLocal)));
    }

    if ($("#MainNetworkEditForm").valid()) {

        if (ValidateEditForm() == true) {
           
            var rowModel = new RowModel();

            rowModel.Id = bankNetworkId;
            rowModel.Description = $("#MainNetworkEditForm").find("#Description").val();
            rowModel.TaxTypeId = $("#MainNetworkEditForm").find("#selectTaxCategory").val();
            rowModel.TaxTypeDescription = $("#MainNetworkEditForm").find("#selectTaxCategory option:selected").html();
            rowModel.CommissionValue = ReplaceDecimalPoint($("#MainNetworkEditForm").find("#Commission_LocalAmount").val());

            rowModel.RetriesNumber = $("#MainNetworkEditForm").find("#RetriesNumber").val();
            if ($("#MainNetworkEditForm").find('#checkHasTax').hasClass("glyphicon glyphicon-unchecked") ||
                $("#MainNetworkEditForm").find('#checkHasTax').hasClass("glyphicon-unchecked glyphicon")) {
                rowModel.HasTax = 0;
            }
            else if ($("#MainNetworkEditForm").find('#checkHasTax').hasClass("glyphicon glyphicon-check") ||
                $("#MainNetworkEditForm").find('#checkHasTax').hasClass("glyphicon-check glyphicon")) {
                rowModel.HasTax = 1;
            }
            if ($("#MainNetworkEditForm").find('#checkRequiresNotification').hasClass("glyphicon glyphicon-unchecked") ||
                $("#MainNetworkEditForm").find('#checkRequiresNotification').hasClass("glyphicon-unchecked glyphicon")) {
                rowModel.RequiresNotification = 0;
            }
            else if ($("#MainNetworkEditForm").find('#checkRequiresNotification').hasClass("glyphicon glyphicon-check") ||
                $("#MainNetworkEditForm").find('#checkRequiresNotification').hasClass("glyphicon-check glyphicon")) {
                rowModel.RequiresNotification = 1;
            }

            SaveBankNetwork(rowModel, "U");

            $("#MainNetworkEditForm").formReset();
            $('#MainNetworkModalEdit').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").UifAlert('hide');
            }, 3000);
        }
    }
   
});

/// Tabla de redes: eliminar un registro
deleteRowModel = new RowModel();
$('#tableNetwork').on('rowDelete', function (event, data) {
    $('#MainNetworkAlert').UifAlert('hide');
    $('#modalDeleteNetwork').appendTo("body").modal('show');

    deleteRowModel.Id = data.Id;
    deleteRowModel.Description = data.Description;
    deleteRowModel.TaxTypeId = data.TaxCategoryId;
    deleteRowModel.TaxTypeDescription = data.TaxCategoryDescription;
    deleteRowModel.CommissionValue = data.CommissionValue;
    deleteRowModel.RetriesNumber = data.RetriesNumber;
    deleteRowModel.HasTax = data.HasTax;
    deleteRowModel.RequiresNotification = data.PreNotifications;
});

$("#MainNetworkDeleteModal").on('click', function () {
    $('#modalDeleteNetwork').modal('hide');

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/DeleteBankNetwork",
        data: JSON.stringify({ "bankNetworkModel": deleteRowModel, "operationType": "D" }),
    dataType: "json",
    contentType: "application/json; charset=utf-8"
}).done(function (data) {
    if (data[0].BankNetworkCode == 0) {
        $("#MainNetworkAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
        var controller = ACC_ROOT + "Networks/GetNetworks";
        $('#tableNetwork').UifDataTable({ source: controller })
    }
    else {
        $("#MainNetworkAlert").UifAlert('show', data[0].MessageError, "danger");
    }
    $('#modalDeleteNetwork').UifModal('hide');

    setTimeout(function () {
        $("#MainNetworkAlert").find('.close').click();
    }, 3000);
});
});

// CheckBox
$('span').click(function () {
    if ($("#ViewBagControlSpanNetwork").val() == "true") {
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

$("#MainNetworkEditForm").find('#checkHasTax').click(function () {
    if (itHasTax == 1) {
        $("#MainNetworkEditForm").find('#checkHasTax').removeClass("glyphicon glyphicon-check");
        $("#MainNetworkEditForm").find('#checkHasTax').addClass("glyphicon glyphicon-unchecked");
        itHasTax = 0;
    }
    else if (itHasTax == 0) {
        $("#MainNetworkEditForm").find('#checkHasTax').removeClass("glyphicon glyphicon-unchecked");
        $("#MainNetworkEditForm").find('#checkHasTax').addClass("glyphicon glyphicon-check");
        itHasTax = 1;
    }
});

$("#MainNetworkEditForm").find('#checkRequiresNotification').click(function () {
    if (generated == 1) {
        $("#MainNetworkEditForm").find('#checkRequiresNotification').removeClass("glyphicon glyphicon-check");
        $("#MainNetworkEditForm").find('#checkRequiresNotification').addClass("glyphicon glyphicon-unchecked");
        generated = 0;
    }
    else if (generated == 0) {
        $("#MainNetworkEditForm").find('#checkRequiresNotification').removeClass("glyphicon glyphicon-unchecked");
        $("#MainNetworkEditForm").find('#checkRequiresNotification').addClass("glyphicon glyphicon-check");
        generated = 1;
    }
});

/// Funciones
function SaveBankNetwork(rowModel, operationType) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Networks/SaveBankNetwork",
        data: JSON.stringify({ "bankNetworkModel": rowModel, "operationType": operationType }),
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function (data) {
        if (data[0].BankNetworkCode < 0) {
            $("#MainNetworkAlert").UifAlert('show', data[0].MessageError, "danger");
        }
        else {
            $("#MainNetworkAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
            var controller = ACC_ROOT + "Networks/GetNetworks";
            $('#tableNetwork').UifDataTable({ source: controller })
        }
        setTimeout(function () {
            $("#MainNetworkAlert").UifAlert('hide');
        }, 3000);
    }
});
}

function ValidateAddForm() {
    if ($("#MainNetworkAddForm").find('#Description').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
        return false;
    }
    if ($("#MainNetworkAddForm").find('#selectTaxCategory').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectTypeTax, "warning");
        return false;
    }

    return true;
}

function ValidateEditForm() {
    if ($("#MainNetworkEditForm").find('#Description').val() == "") {
        $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
        return false;
    }
    if ($("#MainNetworkEditForm").find('#selectTaxCategory').val() == "") {
        $("#alertForm").UifAlert('show', Resources.SelectTypeTax, "warning");
        return false;
    }

    return true;
}