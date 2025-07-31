var endorsementId = 0;
var endorsementNumber = 0;
var issueDate = null;
var monthName = [Resources.January, Resources.February, Resources.March, Resources.April, Resources.May, Resources.June, Resources.July, Resources.August, Resources.September, Resources.October, Resources.November, Resources.December];
$(document).ready(function () {
    $("#btnDeleteReinsurance").hide();
});

$("#FindReinsuranceSearch").click(function () {
    $("#tableEndorsement").UifDataTable('clear');
    $("#alertSearch").UifAlert('hide');
    $("#tableReinsurances tbody").remove();
    $("#tableDistributions tbody").remove();

    if (($('#policyNumber').val() != "") && ($('#policyNumber').val() != null)) {
        var documentNumber = $('#policyNumber').val();
        var branchCode;
        var prefixCode;
        var endorsementNumber;

        if (($('#selectBranch').val() != null) && ($('#selectBranch').val() != "")) {
            branchCode = $('#selectBranch').val();
        } else {
            branchCode = 0;
        }

        if (($('#selectPrefix').val() != null) && ($('#selectPrefix').val() != "")) {
            prefixCode = $('#selectPrefix').val();
        } else {
            prefixCode = 0;
        }

        if (($('#endorsementNumber').val() != "") && ($('#endorsementNumber').val() != null)) {
            endorsementNumber = $('#endorsementNumber').val();
        } else {
            endorsementNumber = -1;
        }

        if ((parseInt(prefixCode) > 0 && parseInt(branchCode) > 0) || (parseInt(prefixCode) == 0 && parseInt(branchCode) == 0)) {
            $.ajax({
                type: 'POST',
                url: rootPath + 'Reinsurance/Process/GetEndorsementByPolicyId',
                data: JSON.stringify({ branchCode: branchCode, prefixCode: prefixCode, documentNumber: documentNumber, endorsementNumber: endorsementNumber }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        if (data.result.length > 0) {
                            getCurrentPolicyByPrefixIdBranchIdPolicyNumber(branchCode, prefixCode, documentNumber);
                            $("#tableEndorsement").UifDataTable({ sourceData: data.result });
                        } else {
                            $("#alertSearch").UifAlert('show', Resources.PolicyNotExists, "danger");
                        }
                    } else {
                        $("#alertSearch").UifAlert('show', data.result, "danger");
                    }
                }
            });
        } else {
            $("#alertSearch").UifAlert('show', Resources.MessageSelectBranch + ' y ' + Resources.Prefix + ' ', "warning");
        }
    }
    else {
        $("#alertSearch").UifAlert('show', Resources.ErrorPolicyNumberRequired, "warning");
    }
});

function getCurrentPolicyByPrefixIdBranchIdPolicyNumber(branchCode, prefixCode, documentNumber) {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Reinsurance/Process/GetCurrentPolicyByPrefixIdBranchIdPolicyNumber',
        data: JSON.stringify({ branchCode: branchCode, prefixCode: prefixCode, documentNumber: documentNumber }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.success) {
            if (data.result != null) {
                $("#selectBranch").UifSelect("setSelected", data.result.Branch.Id);
                $("#selectPrefix").UifSelect("setSelected", data.result.Prefix.Id);
            } else {

                $("#alertSearch").UifAlert('show', Resources.PolicyNotExists, "danger");
            }
        }
    }).fail(function (response) {
        $("#alertSearch").UifAlert('show', response.result, "danger");
    });
}

$('#tableEndorsement').on('rowSelected', function (event, selectedRow) {
    $("#btnDeleteReinsurance").show();
    $("#tableReinsurances").UifDataTable('clear');
    $("#tableDistributions").UifDataTable('clear');
    endorsementId = selectedRow.EndorsementId;
    endorsementNumber = selectedRow.EndorsementNumber;
    issueDate = selectedRow.IssueDate;
    $.ajax({
        type: 'POST',
        url: rootPath + 'Reinsurance/Process/GetReinsuranceByEndorsement',
        data: JSON.stringify({ endorsementId: endorsementId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableReinsurances").UifDataTable({ sourceData: data.result });
            }
        }
    }).fail(function (response) {
        $("#alertSearch").UifAlert('show', response.result, "danger");
    });
});

$('#tableReinsurances').on('rowSelected', function (event, selectedRow) {
    $("#tableDistributions").UifDataTable('clear');
    //Se identifica el elemento seleccionado.
    var layerId = selectedRow.IssueLayerId;
    $.ajax({
        type: 'POST',
        url: rootPath + 'Reinsurance/Process/GetDistributionByReinsurance',
        data: JSON.stringify({ layerId: layerId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableDistributions").UifDataTable({ sourceData: data.result });
            }
        }
    }).fail(function (response) {
        $("#alertSearch").UifAlert('show', response.result, "danger");
    });
});

$('#btnDeleteReinsurance').on('click', function () {
    var documentNumber = $('#policyNumber').val();
    var closingDate = $("#ViewBagReinsuranceDate").val();
    var closingDateSplit = closingDate.split('/');
    issueDate = moment(issueDate).format("DD/MM/YYYY");
    var issueDateSplit = issueDate.split('/');

    closingDate = new Date(closingDateSplit[1] + '/' + closingDateSplit[0] + '/' + closingDateSplit[2]);
    issueDate = new Date(issueDateSplit[1] + '/' + issueDateSplit[0] + '/' + issueDateSplit[2]);

    if (issueDate <= closingDate) {
        $.UifNotify('show', { type: 'danger', message: Resources.EndorsementClosedMonth + " (" + Resources.MonthClosing + ": " + monthName[closingDate.getMonth()] + " " + closingDate.getFullYear() + " )", autoclose: true });
    } else {
        $.UifDialog('confirm', {
            title: 'Eliminar Reaseguro',
            message: '¿Desea eliminar el reaseguro de la Póliza : ' + documentNumber + ' Endoso Nro: ' + endorsementNumber + '?',
            class: 'modal-sm'
        }, function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: rootPath + 'Reinsurance/Process/DeleteReinsurance',
                    data: JSON.stringify({ documentNumber: documentNumber, endorsementNumber: endorsementNumber }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                }).done(function (data) {
                    if (data.success) {
                        if (data.result) {
                            $("#tableReinsurances").UifDataTable('clear');
                            $("#tableDistributions").UifDataTable('clear');
                            $.UifNotify('show', { type: 'success', message: 'El reaseguro se eliminó correctamente', autoclose: true });
                        } else {
                            $.UifNotify('show', { type: 'warning', message: 'No existe reaseguro para este endoso', autoclose: true });
                        }
                    }
                }).fail(function (response) {
                    $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
                });
            }
        });
    }
});

//#region Pagos

$("#searchPaymentsReinsure").click(function () {
    clearReinsurancePayment();
    $("#formSearchPaymentsReinsure").validate();

    var branchCode = $('#paymentSelectBranch').val();
    var prefixCode = $('#paymentSelectPrefix').val();
    var policyNumber = $('#paymentPolicyNumber').val();
    var claimNumber = $('#paymentClaimNumber').val();
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

$('#tablePaymentsRequest').on('rowSelected', function (event, selectedRow, position) {
    $("#tableReinsuredPaymentsLayer").UifDataTable('clear');
    var paymentRequestId = selectedRow.PaymentRequestId;
    var voucherConceptCd = selectedRow.PaymentVoucherConceptCd;
    var claimCoverageCd = selectedRow.ClaimCoverageCd;
    $.ajax({
        type: 'POST',
        url: REINS_ROOT + 'Process/GetPaymentLayerByPaymentRequestId',
        data: JSON.stringify({ paymentRequestId: paymentRequestId, voucherConceptCd: voucherConceptCd, claimCoverageCd: claimCoverageCd }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableReinsuredPaymentsLayer").UifDataTable({ sourceData: data.result });
            }
        } else {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
        }
    }).fail(function (response) {
        $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
    });

});

$('#tableReinsuredPaymentsLayer').on('rowSelected', function (event, selectedRow, position) {
    $("#tableReinsuredPaymentsDistribution").UifDataTable('clear');
    var paymentLayerId = selectedRow.PaymentLayerId;
    $.ajax({
        type: 'POST',
        url: REINS_ROOT + 'Process/GetDistributionPaymentByPaymentLayerId',
        data: JSON.stringify({ paymentLayerId: paymentLayerId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableReinsuredPaymentsDistribution").UifDataTable({ sourceData: data.result });
            }
        } else {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
        }
    }).fail(function (response) {
        $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
    });
});

$('#paymentSelectBranch').on('itemSelected', function (event, selectedItem) {
    clearReinsurancePayment();
    $('#paymentPolicyNumber').val("");
    $('#paymentClaimNumber').val("");
    $('#paymentRequestNumber').val("");
});

$('#paymentSelectPrefix').on('itemSelected', function (event, selectedItem) {
    clearReinsurancePayment();
    $('#paymentPolicyNumber').val("");
    $('#paymentClaimNumber').val("");
    $('#paymentRequestNumber').val("");
});

function clearReinsurancePayment() {
    $("#tablePaymentsRequest").UifDataTable('clear');
    $("#tableReinsuredPaymentsLayer").UifDataTable('clear');
    $("#tableReinsuredPaymentsDistribution").UifDataTable('clear');
}

// #endregion

//#region Siniestros

$("#searchClaimsReinsure").click(function () {
    clearReinsuranceClaim();
    $("#formSearchClaimsReinsure").validate();

    var claimBranchCode = $('#claimSelectBranch').val();
    var claimPrefixCode = $('#claimSelectPrefix').val();
    var claimPolicyNumber = $('#claimPolicyNumber').val();
    var claimNumber = $('#claimNumber').val();

    if (claimNumber == "" || claimNumber == null) {
        claimNumber = 0;
    }

    if ($("#formSearchClaimsReinsure").valid()) {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetClaims',
            data: JSON.stringify({
                branchCode: claimBranchCode,
                prefixCode: claimPrefixCode,
                policyNumber: claimPolicyNumber,
                claimNumber: claimNumber
            }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#tableClaims").UifDataTable({ sourceData: data.result });
                    $('#tableClaims').UifDataTable('order', [8, 'asc']);
                    $('#tableClaims').UifDataTable('order', [7, 'asc']);
                } else {
                    $.UifNotify('show', { type: 'danger', message: Resources.PolicyClaimNotExists, autoclose: true });
                }
            } else {
                $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
            }
        }).fail(function (response) {
            $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
        });
    }
});

$('#tableClaims').on('rowSelected', function (event, selectedRow) {
    $("#tableReinsuredClaimsLayer").UifDataTable('clear');
    var claimId = selectedRow.Id;
    var claimModifyId = selectedRow.Modifications[0].Id;
    var movementSourceId = selectedRow.Modifications[0].Coverages[0].Estimations[0].Type.Id;
    var claimCoverageCd = selectedRow.Modifications[0].Coverages[0].Id;
    $.ajax({
        type: 'POST',
        url: REINS_ROOT + 'Process/GetClaimLayerByClaimIdClaimModifyId',
        data: JSON.stringify({ claimId: claimId, claimModifyId: claimModifyId, movementSourceId: movementSourceId, claimCoverageCd: claimCoverageCd }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableReinsuredClaimsLayer").UifDataTable({ sourceData: data.result });
            }
        } else {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
        }
    }).fail(function (response) {
        $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
    });
});

$('#tableReinsuredClaimsLayer').on('rowSelected', function (event, selectedRow) {
    $("#tableReinsuredClaimsDistribution").UifDataTable('clear');
    var claimLayerId = selectedRow.ClaimLayerId;
    var reinsuredSelected = $("#tableClaims").UifDataTable('getSelected');
    var movementSourceId = reinsuredSelected[0].Modifications[0].Coverages[0].Estimations[0].Type.Id;
    $.ajax({
        type: 'POST',
        url: REINS_ROOT + 'Process/GetDistributionClaimByClaimLayerId',
        data: JSON.stringify({ claimLayerId: claimLayerId, movementSourceId: movementSourceId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableReinsuredClaimsDistribution").UifDataTable({ sourceData: data.result });
            }
        } else {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
        }
    }).fail(function (response) {
        $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
    });
});

$('#claimSelectBranch').on('itemSelected', function (event, selectedItem) {
    clearReinsuranceClaim();
    $("#claimPolicyNumber").val("");
    $("#claimNumber").val("");
});


$('#claimSelectPrefix').on('itemSelected', function (event, selectedItem) {
    clearReinsuranceClaim();
    $("#claimPolicyNumber").val("");
    $("#claimNumber").val("");
});

function clearReinsuranceClaim() {
    $("#tableClaims").UifDataTable('clear');
    $("#tableReinsuredClaimsLayer").UifDataTable('clear');
    $("#tableReinsuredClaimsDistribution").UifDataTable('clear');
}

// #endregion 

