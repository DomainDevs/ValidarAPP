/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var InsuredId = 0;
var PolicyDocumentNumber = 0;
var paymentStatusPageNumber = 1;

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
$("#MainPaymentStatusPreviousPage").attr("disabled", true);
$("#MainPaymentStatusNextPage").attr("disabled", true);


$("#paymentStatusListView").UifListView({
    autoHeight: true, theme: 'dark',
    customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: false,
    displayTemplate: "#payment-template"
});

$("#Clean").on('click', function () {
    ClearField();
});

$('#PaymentSearchBy').on('itemSelected', function (event, selectedItem) {
    ClearField();
    InsuredId = 0;
    PolicyDocumentNumber = 0;

    switch ($("#PaymentSearchBy").val()) {
        case "":
            $("#PaymentSearchBy").val("1");
            $("#RowPolicy").hide();
            $("#RowBranchPrefix").hide();
            $("#RowExpirationDates").hide();
            $("#RowInsured").show();
            break;
        case "1":
            $("#RowPolicy").hide();
            $("#RowBranchPrefix").hide();
            $("#RowExpirationDates").show();
            $("#RowInsured").show();
            $("#DocumentNumberPaymentStatus").attr("required", "required");
            $("#PolicyNumber").removeAttr("required");
            break;
        case "2":
            $("#RowPolicy").show();
            $("#RowBranchPrefix").show();
            $("#RowInsured").hide();
            $("#RowExpirationDates").hide();
            $("#PolicyNumber").attr("required", "required");
            $("#DocumentNumberPaymentStatus").removeAttr("required");
            break;
    }
});

$("#RowPolicy").hide();
$("#RowBranchPrefix").hide();
$("#RowInsured").show();

// Autocomplete documento asegurado
$('#InsuredName').on('itemSelected', function (event, selectedItem) {
    InsuredId = selectedItem.Id;
    $('#DocumentNumberPaymentStatus').UifAutoComplete('setValue',selectedItem.DocumentNumber);
});

$('#DocumentNumberPaymentStatus').on('itemSelected', function (event, selectedItem) {
    InsuredId = selectedItem.Id;
    $('#InsuredName').UifAutoComplete('setValue',selectedItem.Name);
});

// Botón Buscar
$('#SearchPayment').click(function () {
    paymentStatusPageNumber = 1;

    refreshPaymentStatusListView();
});

function refreshPaymentStatusListView() {
    $("#alertForm").UifAlert('hide');
    if ($("#PaymentStatusForm").valid()) {

        // Se valida que ingrese un valor en el campo código
        if ($("#PaymentSearchBy").val() == "") {
            $("#alertForm").UifAlert('show', Resources.ValidationConciliationDate, "warning");
        }

        var policyNumber = $("#PolicyNumber").val();
        var prefix = ($("#Prefix").val() == "") ? -1 : $("#Prefix").val();
        var branch = ($("#Branch").val() == "") ? -1 : $("#Branch").val();
        var expirationDateFrom = moment($("#idExpirationDateFrom").UifDatepicker('getValue')).format("DD/MM/YYYY") == "Invalid date" ? "" : moment($("#idExpirationDateFrom").UifDatepicker('getValue')).format("DD/MM/YYYY");
        var expirationDateTo = moment($("#idExpirationDateTo").UifDatepicker('getValue')).format("DD/MM/YYYY") == "Invalid date" ? "" : moment($("#idExpirationDateFrom").UifDatepicker('getValue')).format("DD/MM/YYYY");

        var _insured = (InsuredId == 0 || InsuredId == "0") ? "" : InsuredId;

        $("#paymentStatusListView").UifListView({
            source: ACC_ROOT + "PaymentStatus/PremiumReceivableStatus?insuredId=" + _insured + "&policyDocumentNumber=" + policyNumber +
                "&branchId=" + branch + "&prefixId=" + prefix + "&endorsementDocumentNumber=" +
                $("#EndorsementNumber").val() + "&policiesWithPortfolio=N" + "&pageNumber=" + paymentStatusPageNumber + "&ExpirationDateFrom=" + expirationDateFrom + "&ExpirationDateTo=" + expirationDateTo,
            customDelete: false,
            customAdd: false,
            customEdit: false,
            edit: false,
            delete: false,
            result: false,
            displayTemplate: "#payment-template"
        });

        setTimeout(function () {
            var totalAmount = GetTotalAmount();
            if (totalAmount != null) {
                $("#TotalCash").text("$ " + NumberFormatDecimal(totalAmount, "2", ".", ","));
            }
        }, 7000);

        setTimeout(function () {
            $("#MainPaymentStatusPreviousPage").attr("disabled", false);
            $("#MainPaymentStatusNextPage").attr("disabled", false);
        }, 10000);
    }
}

$("#MainPaymentStatusPreviousPage").click(function () {
    $("#MainPaymentStatusPreviousPage").attr("disabled", true);
    $("#MainPaymentStatusNextPage").attr("disabled", true);

    if (paymentStatusPageNumber == 1) {
        $("#MainPaymentStatusPreviousPage").attr("disabled", true);
        $("#MainPaymentStatusNextPage").attr("disabled", false);

    } else {
        paymentStatusPageNumber--;
        refreshPaymentStatusListView();
    }
});


$("#MainPaymentStatusNextPage").click(function () {
    $("#MainPaymentStatusPreviousPage").attr("disabled", true);
    $("#MainPaymentStatusNextPage").attr("disabled", true);
    paymentStatusPageNumber++;
    refreshPaymentStatusListView();
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function ClearField() {
    $("#PaymenSearchBy").val("");
    $("#PolicyNumber").val("");
    $("#DocumentNumberPaymentStatus").val("");
    $("#InsuredName").val("");
    $("#Branch").val("");
    $("#Prefix").val("");
    $("#EndorsementNumber").val("");
    $("#paymentStatusListView").UifListView();
    $("#TotalCash").text("$ " + NumberFormatDecimal(0, "2", ".", ","));
    $("#MainPaymentStatusPreviousPage").attr("disabled", true);
    $("#MainPaymentStatusNextPage").attr("disabled", true);
    $("#idExpirationDateFrom").UifDatepicker('clear');
    $("#idExpirationDateTo").UifDatepicker('clear');
}


/// Obtiene el total del saldo y valor cuota
function GetTotalAmount() {
    var totalAmount = 0;
    var error = "";
    const rowids = $("#paymentStatusListView").UifListView("getData");

    if (rowids != null) {
        for (var k = 0; k < rowids.length; k++) {
            var index = rowids[k].Amount.indexOf("(");
            if (index == 0) {
                var paymentAmount = String(rowids[k].Amount).replace("$", "").replace(/,/g, "").replace(" ", "").replace("(", "").replace(")", "");
                totalAmount -= parseFloat(paymentAmount);
            }
            else {
                var paymentAmount = String(rowids[k].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
                totalAmount += parseFloat(paymentAmount);

            }
        }
    }
    return totalAmount;
}