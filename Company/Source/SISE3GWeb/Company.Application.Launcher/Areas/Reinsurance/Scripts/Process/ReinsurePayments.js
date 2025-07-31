var selectedItemTablePaymentRequestCode = null;
var selectedItemTablePaymentMovementId = null;
var selectedItemTableVoucherConceptCd = null;
var selectedItemTableClaimCoverageCd = null;
var processId = 0;
var variance = 0;
var active = false;

$("#btnReinsurePayment").hide();
$("#btnRecordReinsure").hide();
$("#btnLoadReinsPayment").hide();
$("#sourceAmount").hide();
$("#variance").hide();
$("#tempPaymentReinsSourceId").hide();

$("#searchPaymentsReinsure").click(function () {
    $("#tablePaymentsRequest").UifDataTable('clear');
    $("#tablePaymentsRequest").UifDataTable('clear');
    $("#tableReinsurancePayments").UifDataTable('clear');
    selectedItemTablePaymentRequestCode = null;
    selectedItemTablePaymentMovementId = null;
    selectedItemTableVoucherConceptCd = null;
    selectedItemTableClaimCoverageCd = null;
    processId = 0;
    variance = 0;
    active = false;
    $('#tabsReinsPayments').UifTabHeader('disabled', '#tabModifyPaymentReinsure', true);
    $("#btnReinsurePayment").hide();


    $("#formSearchPaymentsReinsure").validate();

    var branchCode = $('#selectBranch').val();
    var prefixCode = $('#selectPrefix').val();
    var policyNumber = $('#policyNumber').val();
    var claimNumber = $('#claimNumber').val();
    var paymentRequestNumber = $('#paymentRequestNumber').val();

    if (paymentRequestNumber == "" || paymentRequestNumber == null) {
        paymentRequestNumber = 0;
    }

    if ($("#formSearchPaymentsReinsure").valid()) {
        $.ajax({
            type: 'POST',
            url: REINS_ROOT + 'Process/GetPaymentsRequest',
            data: JSON.stringify({
                branchCode: branchCode,
                prefixCode: prefixCode,
                policyNumber: policyNumber,
                claimNumber: claimNumber,
                paymentRequestNumber: paymentRequestNumber
            }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#tablePaymentsRequest").UifDataTable({ sourceData: data.result });
                } else {
                    $.UifNotify('show', { type: 'danger', message: Resources.PolicyClaimPaymentNotExists, autoclose: true });
                }
            } else {
                $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
            }
        }).fail(function (response) {
            $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
        }).always(function () {
            unlockScreen();
        });;
    }
});

$('#tablePaymentsRequest').on('rowSelected', function (event, selectedRow) {
    active = false;
    $('#tabsReinsPayments').UifTabHeader('disabled', '#tabModifyPaymentReinsure', true);
    selectedItemTablePaymentRequestCode = selectedRow.PaymentRequestId;
    selectedItemTablePaymentMovementId = selectedRow.ConceptSourceId;
    selectedItemTableVoucherConceptCd = selectedRow.PaymentVoucherConceptCd;
    selectedItemTableClaimCoverageCd = selectedRow.ClaimCoverageCd;
    $("#tableReinsurancePayments").UifDataTable('clear');
    getReinsurancePaymentDistributionsByPaymentRequestId(selectedItemTablePaymentRequestCode, selectedItemTablePaymentMovementId, selectedItemTableVoucherConceptCd, selectedItemTableClaimCoverageCd).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableReinsurancePayments").UifDataTable({ sourceData: data.result });
                $('#btnReinsurePayment').hide();
                $('#btnLoadReinsPayment').show();

            } else {
                $('#btnReinsurePayment').show();
                $('#btnLoadReinsPayment').hide();
            }
        } else {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
            $("#tableReinsurancePayments").UifDataTable('clear');
        }
    }).always(function () {
        unlockScreen();
    });

});

$('#btnReinsurePayment').click(function () {
    if (selectedItemTablePaymentRequestCode == null) {
        $.UifNotify('show', { type: 'info', message: Resources.MessageNotSelectedPayment, autoclose: true });
    }
    else if ($("#tableReinsurancePayments").UifDataTable('getData').length != 0) {
        $.UifNotify('show', { type: 'info', message: Resources.PaymentAlreadyReinsurance, autoclose: true });
    }
    else {
        $('#modalConfirmation').appendTo("body").UifModal('showLocal');
    }
});

$("#ReinsurePaymentsConfirmYes").click(function () {
    $('#modalConfirmation').modal('hide');
    lockScreen();
    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/ReinsurancePayment",
        data: JSON.stringify({ paymentRequestId: selectedItemTablePaymentRequestCode }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success == false) {
            if (data.result.ReinsuranceId == 0) {
                $.UifNotify('show', { type: 'danger', message: Resources.PaymentAlreadyNoReinsurance, autoclose: true });
            }
            else {
                $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
            }
        }
        else {
            if (data.result.ReinsuranceId > 0) {
                getReinsurancePaymentDistributionsByPaymentRequestId(selectedItemTablePaymentRequestCode, selectedItemTablePaymentMovementId, selectedItemTableVoucherConceptCd, selectedItemTableClaimCoverageCd).done(function (response) {
                    if (response.success) {
                        if (response.result.length > 0) {
                            $("#tableReinsurancePayments").UifDataTable({ sourceData: response.result });
                            $("#btnReinsurePayment").hide();
                            $.UifNotify('show', { type: 'success', message: Resources.MessageReinsurePaymentSuccess + ' ' + Resources.ReinsuranceId + ': ' + data.result.ReinsuranceId, autoclose: true });
                        }
                    } else {
                        $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
                        $("#tableReinsurancePayments").UifDataTable('clear')
                    }
                })
            } else {
                if (data.result.ReinsuranceId == 0) {
                    $.UifNotify('show', { type: 'danger', message: Resources.PaymentAlreadyNoReinsurance, autoclose: true });
                }
            }
        }
    }).always(function () {
        unlockScreen();
    });
});

$("#btnLoadReinsPayment").click(function () {
    $('#modalLoadReinsPayment').appendTo("body").UifModal('showLocal');
});

$("#btnModalLoadReinsPayment").click(function () {
    $('#modalLoadReinsPayment').modal('hide');
    $('#tabsReinsPayments').UifTabHeader('disabled', '#tabGeneralPay', true);
    $("#btnLoadReinsPayment").hide();
    $('#btnReinsurePayment').hide();
    lockScreen();
    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/ManualPaymentReinsurance",
        data: JSON.stringify({
            paymentRequestId: selectedItemTablePaymentRequestCode,
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (!data.success) {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
            $("#tableModificationReinsurancePaym").UifDataTable('clear');
        }
        else {
            if ((data.success) && data.result > 0) {
                processId = data.result;
                $('#tabsReinsPayments').UifTabHeader('disabled', '#tabModifyPaymentReinsure', false);

                getPaymentAllocations(processId, selectedItemTablePaymentMovementId, selectedItemTableVoucherConceptCd, selectedItemTableClaimCoverageCd).done(function (response) {
                    if (response.success) {
                        if (response.result.length > 0) {
                            $("#tabModifyPaymentReinsure").trigger('click');
                            $("#btnRecordReinsure").show();
                            $("#tableModificationReinsurancePaym").UifDataTable({ sourceData: response.result });
                            $.UifNotify('show', { type: 'success', message: Resources.MessageProcessSuccessfully, autoclose: true });
                        }
                    }
                })

            }
        }
    }).always(function () {
        unlockScreen();
    });
});

$('#tableModificationReinsurancePaym').on('rowEdit', function (event, selectedRow) {
    active = true;
    $("#description").val(selectedRow.Description);
    $("#smallDescription").val(selectedRow.SmallDescription);
    $("#cumulusKey").val(selectedRow.CumulusKey);
    $("#lineId").val(selectedRow.CumulusKey);
    $("#layerNumber").val(selectedRow.LayerNumber);
    $("#levelNumber").val(selectedRow.LevelNumber);
    $("#newAmount").val(FormatMoney(parseFloat(selectedRow.Amount)));
    $("#sourceAmount").val(FormatMoney(parseFloat(selectedRow.Amount)));
    $("#variance").val(FormatMoney(parseFloat(variance)));
    $("#tempPaymentReinsSourceId").val(selectedRow.TmpPaymentReinsSourceId);
    $('#modalModificationReinsuranceLines').UifModal('showLocal', Resources.ModificationReinsurance);
    $("#btnRecordReinsure").show();
});

$("#modalModificationReinsuranceLines").find("#btnReinsPaym").click(function () {
    $("#formReinsPaym").validate();

    if ($("#formReinsPaym").valid()) {

        var tempPaymentReinsSourceId = $("#tempPaymentReinsSourceId").val();
        var newAmount = NotFormatMoney($("#newAmount").val());
        var sourceAmount = NotFormatMoney($("#sourceAmount").val());
        var newVariance = parseFloat(sourceAmount) - parseFloat(newAmount);
        variance = variance + newVariance;

        $.ajax({
            type: "POST",
            url: rootPath + "Reinsurance/Process/ModificationReinsurancePayment",
            data: JSON.stringify({
                tempPaymentReinsSourceId: tempPaymentReinsSourceId,
                newAmount: newAmount,
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $('#modalModificationReinsuranceLines').UifModal('hide');
                if (variance == 0) {
                    $.UifNotify('show', { type: 'success', message: Resources.TotalDifferenceBetweenAmount + ': ' + FormatMoney(variance), autoclose: true });
                } else {
                    $.UifNotify('show', { type: 'danger', message: Resources.TotalDifferenceBetweenAmount + ': ' + FormatMoney(variance), autoclose: true });
                    $('#tabsReinsPayments').UifTabHeader('disabled', '#tabGeneralPay', true);
                }

                selectedItemTableClaimCoverageCd

                getPaymentAllocations(processId, selectedItemTablePaymentMovementId, selectedItemTableVoucherConceptCd, selectedItemTableClaimCoverageCd).done(function (response) {
                    if (response.success) {
                        if (response.result.length > 0) {
                            $("#tableModificationReinsurancePaym").UifDataTable({ sourceData: response.result });
                        }
                    }
                })
            }
        });
    }
});

$('#modalModificationReinsuranceLines').on('focusout', '#newAmount ', function (event, selectedItem) {

    if (parseInt($(this).val()) > 0) {
        $(this).val(FormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$('#modalModificationReinsuranceLines').on('focusin', '#newAmount ', function (event, selectedItem) {

    if (parseInt($(this).val()) > 0) {
        $(this).val(NotFormatMoney($(this).val()));
    } else {
        $(this).val(0);
    }
});

$("#btnRecordReinsure").click(function () {
    if (processId != 0) {
        if (variance == 0) {
            $('#modalRecordConfirmation').appendTo("body").UifModal('showLocal');
        }
        else {
            $.UifNotify('show', { type: 'danger', message: Resources.TotalDifferenceBetweenAmount + ': ' + FormatMoney(variance), autoclose: true });
            $('#tabsReinsPayments').UifTabHeader('disabled', '#tabGeneralPay', true);
        }
    }
    else {
        $.UifNotify('show', { type: 'danger', message: Resources.MessageReinsuranceNotRecord, autoclose: true });
    }
});
$("#btnConfirmRecordNo").click(function () {
    $.UifNotify('show', { type: 'warning', message: Resources.ReinsuranceNotSave, autoclose: true });
});
//Implementación del botón de Confirmación del modal Grabar Reaseguro de Pago.
$("#btnConfirmRecordYes").click(function () {
    $('#modalRecordConfirmation').modal('hide');
    lockScreen();
    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/SavePaymentReinsurance",
        data: JSON.stringify({
            processId: processId,
            paymentRequestId: selectedItemTablePaymentRequestCode
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success == false) {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
        }
        else {
            processId = 0;
            getReinsurancePaymentDistributionsByPaymentRequestId(selectedItemTablePaymentRequestCode, selectedItemTablePaymentMovementId, selectedItemTableVoucherConceptCd, selectedItemTableClaimCoverageCd).done(function (response) {
                if (response.success) {
                    if (response.result.length > 0) {
                        $("#tableReinsurancePayments").UifDataTable({ sourceData: response.result });
                        $("#btnRecordReinsure").hide();
                        $('#tabsReinsPayments').UifTabHeader('disabled', '#tabGeneralPay', false);
                        $('#tabsReinsPayments').UifTabHeader('disabled', '#tabModifyPaymentReinsure', true);
                        $("#tabGeneralPay").trigger('click');
                        $.UifNotify('show', { type: 'success', message: Resources.MessageReinsurePaymentSuccess + ' ' + Resources.ReinsuranceId + ': ' + data.result, autoclose: true });
                    }
                }
            });
        }
    }).always(function () {
        unlockScreen();
    })
});

function getReinsurancePaymentDistributionsByPaymentRequestId(paymentRequestId, movementSourceId, voucherConceptCd, claimCoverageCd) {
    return $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/GetReinsurancePaymentDistributionsByPaymentRequestId",
        data: JSON.stringify({
            paymentRequestId: paymentRequestId,
            movementSourceId: movementSourceId,
            voucherConceptCd: voucherConceptCd,
            claimCoverageCd: claimCoverageCd
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function getPaymentAllocations(processId, movementSourceId, voucherConceptCd, claimCoverageCd) {
    return $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/GetPaymentAllocations",
        data: JSON.stringify({
            processId: processId,
            movementSourceId: movementSourceId,
            voucherConceptCd: voucherConceptCd,
            claimCoverageCd: claimCoverageCd
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}