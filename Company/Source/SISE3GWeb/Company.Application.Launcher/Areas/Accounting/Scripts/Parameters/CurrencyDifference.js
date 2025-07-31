var currencyDifferenceIsUpdate = 0;

var oCurrencyDifference = {
    currencyCode: 0,
    maxDifference: 0,
    currencyDifferencePercentage: 0
};

setTimeout(function () {
    currencyDifferenceRefreshGrid();
}, 5000);


//ACCION GRABAR
$("#SaveCurrencyDifference").click(function () {

    if ($("#CurrencyDifferenceCurrency").val() != "" && $("#maxDifference").val() != '' &&
    $("#currencyDifferencePercentage").val() != '') {

        if (currencyDifferenceIsUpdate == 0 && !(validePercentage())) {
            saveCurrencyDifference();
        }

        if (currencyDifferenceIsUpdate == 1 || validePercentage()) {
            updateCurrencyDifference(setDataUpdate());
            currencyDifferenceIsUpdate = 0;
        }
    }
    else {

        $("#alertCurrencyDifference").UifAlert('show', Resources.ValidationSave, "warning");
    }
});

//ACCION CANCELAR
$("#CancelCurrencyDifference").click(function () {
    $("#CurrencyDifferenceCurrency").val("");
    $("#CurrencyDifferenceCurrency").removeAttr("disabled");
    $("#maxDifference").val('');
    $("#currencyDifferencePercentage").val('');
    currencyDifferenceIsUpdate = 0;
    $("#alertCurrencyDifference").UifAlert('hide');

});

$("#CurrencyDifference").on('rowEdit', function (event, data, position) {

    $("#CurrencyDifferenceCurrency").val(data.CurrencyCode);
    $("#CurrencyDifferenceCurrency").attr("disabled", "disabled");
    $("#maxDifference").val(data.MaximumDifference);
    $("#currencyDifferencePercentage").val(data.PercentageDifference);
    currencyDifferenceIsUpdate = 1;
});

$("#maxDifference").on('blur', function (event) {
    if ($("#maxDifference").val() != "") {
        if ($("#maxDifference").val() > 100) {
            $("#alertCurrencyDifference").UifAlert('show', Resources.CannotInputValueGreatherThan + '100', "warning");
            $("#maxDifference").val("");
        }
    }
});

$("#currencyDifferencePercentage").on('blur', function (event) {
    if ($("#currencyDifferencePercentage").val() != "") {
        if ($("#currencyDifferencePercentage").val() > 100) {
            $("#alertCurrencyDifference").UifAlert('show', Resources.CannotInputValueGreatherThan + '100', "warning");
            $("#currencyDifferencePercentage").val("");
        }
    }
});

$('#CurrencyDifference').on('rowDelete', function (event, data, position) {
    oCurrencyDifference.currencyCode = data.CurrencyCode;
    $('#DeleteDialog').appendTo("body").UifModal('showLocal');
});

$("#DeleteAccountDialog").click(function () {

    $.ajax({
        url: ACC_ROOT + "Parameters/DeleteCurrencyDifference",
        data: { "currencyCode": oCurrencyDifference.currencyCode },
        success: function () {
            currencyDifferenceClearData();
            $("#alertCurrencyDifference").UifAlert('show', Resources.DeleteSucces, "success");
            currencyDifferenceRefreshGrid();
            $("#DeleteDialog").modal('hide');
        }
    });
});

function currencyDifferenceRefreshGrid() {
    var controller = ACC_ROOT + "Parameters/GetCurrencyDifferences";
    $("#CurrencyDifference").UifDataTable({ source: controller });
}

//LLENAR MODELO
function setDataUpdate() {

    oCurrencyDifference.maxDifference = $("#maxDifference").val();
    oCurrencyDifference.percentageDifference = $("#currencyDifferencePercentage").val();
    oCurrencyDifference.currencyCode = $("#CurrencyDifferenceCurrency").val();

    return oCurrencyDifference;
}

//Limpiar datos
function currencyDifferenceClearData() {
    $("#CurrencyDifference").dataTable().fnClearTable();
    $("#CurrencyDifferenceCurrency").val("");
    $("#CurrencyDifferenceCurrency").removeAttr("disabled");
    $("#maxDifference").val("");
    $("#currencyDifferencePercentage").val("");
    $("#alertCurrencyDifference").UifAlert('hide');
}

//EDITAR
function updateCurrencyDifference(oCurrencyDifference) {
    if (oCurrencyDifference != null) {
        $.ajax({
            url: ACC_ROOT + "Parameters/UpdateCurrencyDifference",
            data: {
                "currencyCode": oCurrencyDifference.currencyCode, "maxDifference": oCurrencyDifference.maxDifference,
                "percentageDifference": oCurrencyDifference.percentageDifference
            },
            success: function () {
                currencyDifferenceClearData();
                $("#alertCurrencyDifference").UifAlert('show', Resources.UpdateSucces, "success");
                currencyDifferenceRefreshGrid();
            }
        });
    }
};

//Valida si ya existe un registro en la tabla
function validePercentage() {
    var result = false;

    var idsTable = $("#CurrencyDifference").UifDataTable("getData");

    for (var i = 0; i < idsTable.length; i++) {
        //  trae el valor de una celda
        if ($("#CurrencyDifferenceCurrency").val() == idsTable[i].CurrencyCode) {
            $("#alertCurrencyDifference").UifAlert('show', Resources.Duplicate, "warning");
            result = true;
        }
    }
    return result;
}

//GUARDAR
function saveCurrencyDifference() {
    $.ajax({
        url: ACC_ROOT + "Parameters/SaveCurrencyDifference",
        data: {
            "currencyCode": $("#CurrencyDifferenceCurrency").val(), "maxDifference": $("#maxDifference").val(),
            "percentageDifference": $("#currencyDifferencePercentage").val()
        },
        success: function () {
            currencyDifferenceClearData();
            currencyDifferenceIsUpdate = 0;
            $("#alertCurrencyDifference").UifAlert('show', Resources.SaveSuccessfully, "success");
            currencyDifferenceRefreshGrid();
        }
    });
};