/*----------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*----------------------------------------------------------*/

var currencyId = -1;
var accountBankId = -1;
var paymentOrderCanceledBefore = 0;
var paymentOrderCancel = 0;

/*----------------------------------------------------------*/
//ACCIONES / EVENTOS
/*----------------------------------------------------------*/

$(document).ready(function ()
{
    $(document).ajaxSend(function (event, xhr, settings) {
        if (event.currentTarget.activeElement.id == "issuingAcountNumberCancellation") {
            if (settings.url.indexOf("SearchAccountBank") != -1) {
                settings.url = settings.url + "&param=1";
            }
        }
        if (event.currentTarget.activeElement.id == "issuingBankAccountNameCancellation") {
            if (settings.url.indexOf("SearchAccountBank") != -1) {
                settings.url = settings.url + "&param=2";
            }
        }
    });
});

$("#CleanCancellation").click(function () {
    paymentOrderCanceledBefore = 0;
    paymentOrderCancel = 0;

    CleanFieldsCancellation();
    ClearGridDataCancellation();
    var $el = $('#inputControlSingle').parent();
    $el.wrap('<form>').closest('form').get(0).reset();
    $el.unwrap();

});

$("#CancelCancellation").click(function () {
    showConfirmCancellation();
});

$("#Annular").click(function () {

    $("#TransferCancellationForm").validate();

    if ($("#TransferCancellationForm").valid()) {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });
        cancelTransferBank();
    } else {
        $("#alert").UifAlert('show', Resources.MassiveDataLoadValidationFields, "warning");
    }
});

////////////////////////////////////////
// Autocomplete banco emisor - cheque //
////////////////////////////////////////
$('#issuingAcountNumberCancellation').on('itemSelected', function (event, selectedItem) {
    accountBankId = selectedItem.AccountBankId;
    $("#CurrencyCancellation").val(selectedItem.CurrencyDescription);
    $("#issuingBankAccountNameCancellation").val(selectedItem.LongDescription);
});

$('#issuingBankAccountNameCancellation').on('itemSelected', function (event, selectedItem) {
    accountBankId = selectedItem.AccountBankId;
    $("#CurrencyCancellation").val(selectedItem.CurrencyDescription);
    $("#issuingAcountNumberCancellation").val(selectedItem.AccountNumber);
});

////////////////////////////////////////
// Botón de Cargar Archivo a Tabla    //
////////////////////////////////////////
$("#UpLoadFile").on('click', function () {
    var Numerrors = 0;

    $("#TransferCancellationForm").validate();

    if ($("#TransferCancellationForm").valid()) {

        if ($("#inputControlSingle").val() != "")
        {
            if (uploadAjaxCancellation()) uploadAjaxCancellation();
        }
    }
});

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/


setTimeout(function () {
    getAccountingDate();
}, 1500);

//Obtiene la fecha contable del servidor
function getAccountingDate() {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId").val() == undefined) {
            $.ajax({
                url: ACC_ROOT + "Common/GetAccountingDate",
                success: function (data) {
                    $("#CancellationDate").val(data);
                }
            });
    }
}

function ValidateFileCancellation() {
    var validFilesTypes = ["csv", "xml", "xls", "xlsx", "txt"];
    var file = document.getElementById("inputControlSingle");
    var path = file.value;
    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
    var isValidFile = false;
    for (var i = 0; i < validFilesTypes.length; i++) {
        if (ext == validFilesTypes[i]) {
            isValidFile = true;
            break;
        }
    }
    if (!isValidFile) {
        $('#FileCancellationPath').val('');
        $("#alert").UifAlert('show', Resources.IncorrectFileType, "warning");

        ClearGridDataCancellation();
    }
    return isValidFile;
}

function uploadAjaxCancellation() {

    $("#TransferCancellation").UifDataTable();
    var inputFileImage = document.getElementById("inputControlSingle");
    var file = inputFileImage.files[0];
    var data = new FormData();


    data.append('uploadedFile', file);
    data.append("accountBankId", accountBankId);
    var url = ACC_ROOT  + "Transfer/ReadFileInMemory";

    $.ajax({
        url: url, type: 'POST',
        contentType: false, data: data, processData: false, cache: false,
        success: function (result) {

            ClearGridDataCancellation();

            switch (result) {
                case "FormatException":
                    $("#alert").UifAlert('show', Resources.WrongFormatBlankRows + " / " + Resources.WrongDateFormat, "warning");
                    break;
                case "OverflowException":
                    $("#alert").UifAlert('show', Resources.WrongFormatBlankColumns, "warning");
                    break;
                case "IndexOutOfRangeException":
                    $("#alert").UifAlert('show', Resources.FileDoesNotHaveColumnsNeeded, "warning");
                    break;
                case "InvalidCastException":
                    $("#alert").UifAlert('show', Resources.EmptyFile + " / " + Resources.FileDoesNotHaveColumnsNeeded, "warning");
                    break;
                case "NegativeId":
                    $("#alert").UifAlert('show', Resources.IdsNoNegative, "warning");
                    break;
                case "NullReferenceException":
                    $("#alert").UifAlert('show', Resources.FileDoesNotHaveColumnsNeeded, "warning");
                    break;
                case "XmlException":
                    $("#alert").UifAlert('show', Resources.ErrorInXMLTags, "warning");
                    break;
                default:
                    $("#FileCancellationPath").val(result);
                    var controller = ACC_ROOT + "Transfer/LoadTransferCancellation?accountBankId=" + accountBankId + "&pathFile=" + result;
                    $("#TransferCancellation").UifDataTable({ source: controller });
                    setTimeout(function () { TotalAmountCancellation(); }, 1000);
            }
        }
    });
}

function TotalAmountCancellation() {
    var totalSum = 0.0;
    var rowids = $("#TransferCancellation").UifDataTable('getData');
    for (var i in rowids) {
        totalSum += parseFloat(ClearFormatCurrency(rowids[i].Amount.replace("", ",")));
    }
    $("#TotalAmountTransferCancellation").val("$ " + NumberFormatDecimal(totalSum, "2", ".", ","));
}

function ClearGridDataCancellation() {
    var fileupload = $('#inputControlSingle');
    fileupload.replaceWith(fileupload.clone(true));

    $('#FileCancellationPath').val('');
    $("#TransferCancellation").dataTable().fnClearTable();
}

function cancelTransferBank() {
    var rowids = $("#TransferCancellation").UifDataTable('getData');
    for (var i in rowids) {
        $.ajax({
            async: false,
            url: ACC_ROOT + "Transfer/CancelTransferBank",
            data: { "paymentOrderId": rowids[i].PaymentOrderCode, "typeOperation": 1 },
            success: function (data) {
                if (data.Id > 0) {
                    paymentOrderCancel++;
                    $("#alert").UifAlert('show', Resources.CancellationProcessSuccessful, "success");
                }
            }
        });
    }
    $("#TransferCancellation").dataTable().fnClearTable();
}

function showConfirmCancellation () {
    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': 'Anulación' }, function (result) {
        if (result) {

            paymentOrderCanceledBefore = 0;
            paymentOrderCancel = 0;

            CleanFieldsCancellation();
            //ClearGridDataTransferNull();

            var $el = $('#inputControlSingle').parent();
            $el.wrap('<form>').closest('form').get(0).reset();
            $el.unwrap();

        }
    });
}

function CleanFieldsCancellation() {
    $("#issuingAcountNumberCancellation").val('');
    $("#issuingBankAccountNameCancellation").val('');
    $("#Currency").val('');
    $("#TotalAmountTransferCancellation").val('');
}