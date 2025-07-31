var selectedItemTablePolicyNumber = null;
var selectedItemTableClaimNumber = null;
var selectedItemTableClaimCode = null;
var selectedItemTableClaimModifyCode = null;
var selectedItemTableMovementId = null;
var selectedItemTableClaimCoverageCd = null;
var active = false;
var processId = 0;
var variance = 0;

$('#btnReinsureClaim').hide();
$("#btnRecordReinsure").hide();
$("#btnLoadReinsClaims").hide();
$("#sourceAmount").hide();
$("#variance").hide();
$("#tempReinsuranceProcessId").hide();
$("#tempClaimReinsSourceId").hide();

$("#searchClaimsReinsure").click(function () {
    $("#tableClaims").UifDataTable('clear');
    $("#tableReinsuranceClaims").UifDataTable('clear');
    selectedItemTablePolicyNumber = null;
    selectedItemTableClaimNumber = null;
    selectedItemTableClaimCode = null;
    selectedItemTableClaimModifyCode = null;
    selectedItemTableMovementId = null;
    selectedItemTableClaimCoverageCd = null;
    variance = 0;
    active = false;

    $('#btnReinsureClaim').hide();
    $("#btnLoadReinsClaims").hide();
    $("#btnRecordReinsure").hide();
    $('#tabsReinsClaims').UifTabHeader('disabled', '#tabModifyClaimReinsure', true);

    $("#formSearchClaimsReinsure").validate();

    var branchCode = $('#selectBranch').val();
    var prefixCode = $('#selectPrefix').val();
    var policyNumber = $('#policyNumber').val();
    var claimNumber = $('#claimNumber').val();

    if (claimNumber == "" || claimNumber == null) {
        claimNumber = 0;
    }

    if ($("#formSearchClaimsReinsure").valid()) {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetClaims',
            data: JSON.stringify({
                branchCode: branchCode,
                prefixCode: prefixCode,
                policyNumber: policyNumber,
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
        }).always(function () {
            unlockScreen();
        });;
    }
});

$('#tableClaims').on('rowSelected', function (event, selectedRow) {
    $('#tabsReinsClaims').UifTabHeader('disabled', '#tabModifyClaimReinsure', true);
    selectedItemTablePolicyNumber = selectedRow.Endorsement.PolicyNumber;
    selectedItemTableClaimNumber = selectedRow.Number;
    selectedItemTableClaimCode = selectedRow.Id;
    selectedItemTableClaimModifyCode = selectedRow.Modifications[0].Id;
    selectedItemTableMovementId = selectedRow.Modifications[0].Coverages[0].Estimations[0].Type.Id;
    selectedItemTableClaimCoverageCd = selectedRow.Modifications[0].Coverages[0].Id;
    active = false;
    $('#tableReinsuranceClaims').UifDataTable('clear');
    $("#btnLoadReinsClaims").show();

    getReinsuranceClaimDistributionByClaimCodeClaimModifyCode(selectedItemTableClaimCode, selectedItemTableClaimModifyCode, selectedItemTableMovementId, selectedItemTableClaimCoverageCd).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#tableReinsuranceClaims").UifDataTable({ sourceData: data.result });
                $('#btnReinsureClaim').hide();
            }
            else {
                $('#btnReinsureClaim').show();
            }
        }
        else {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
            $('#tableReinsuranceClaims').UifDataTable('clear');
        }
    }).always(function () {
        unlockScreen();
    });
});

$('#btnReinsureClaim').click(function () {
    if (selectedItemTableClaimNumber == null) {
        $.UifNotify('show', { type: 'info', message: Resources.MessageNotSelectedClaim, autoclose: true });
    }
    else if ($("#tableReinsuranceClaims").UifDataTable('getData').length > 0) {
        $.UifNotify('show', { type: 'info', message: Resources.ClaimAlreadyReinsurance, autoclose: true });
    }
    else {
        $('#modalConfirmation').appendTo("body").UifModal('showLocal');
    }
});

$("#btnConfirmYes").click(function () {
    $('#modalConfirmation').modal('hide');
    lockScreen();
    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/ReinsuranceClaim",
        data: JSON.stringify({
            claimId: selectedItemTableClaimCode,
            claimModifyId: selectedItemTableClaimModifyCode
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success == false && data.result.ReinsuranceId != undefined) {
            if (data.result.Number == -1) {
                $.UifNotify('show', { type: 'danger', message: Resources.NotPossibleToReinsure, autoclose: true });
            }
            else {
                $.UifNotify('show', { type: 'danger', message: Resources.MessageReinsureClaimError + ' ' + Resources.ReinsuranceId + ': ' + data.result.ReinsuranceId, autoclose: true });
            }
        }
        else {
            if (data.result.Number == -1) {
                $.UifNotify('show', { type: 'danger', message: Resources.NotPossibleToReinsure, autoclose: true });
            }
            else {
                getReinsuranceClaimDistributionByClaimCodeClaimModifyCode(selectedItemTableClaimCode, selectedItemTableClaimModifyCode, selectedItemTableMovementId, selectedItemTableClaimCoverageCd).done(function (response) {
                    if (response.success) {
                        if (response.result.length > 0) {
                            $("#tableReinsuranceClaims").UifDataTable({ sourceData: response.result })
                            $.UifNotify('show', { type: 'success', message: Resources.MessageReinsureClaimSuccess + ' ' + Resources.ReinsuranceId + ': ' + data.result.ReinsuranceId, autoclose: true });
                            $('#btnReinsureClaim').hide();
                        }
                        else {
                            $('#btnReinsureClaim').show();
                        }
                    }
                    else {
                        $.UifNotify('show', { type: 'danger', message: response.result, autoclose: true });
                        $('#tableReinsuranceClaims').UifDataTable('clear');
                    }
                }).always(function () {
                    unlockScreen();
                });
            }
        }
    }).always(function () {
        unlockScreen();
    });
});

$("#btnLoadReinsClaims").click(function () {
    $('#modalLoadReinsClaim').appendTo("body").UifModal('showLocal');
});

$("#btnModalLoadReinsClaim").click(function () {
    $('#tabsReinsClaims').UifTabHeader('disabled', '#tabGeneralClm', true);
    $('#modalLoadReinsClaim').modal('hide');
    $("#btnLoadReinsClaims").hide();
    $('#btnReinsureClaim').hide();

    lockScreen();

    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/ManualClaimReinsurance",
        data: JSON.stringify({ claimId: selectedItemTableClaimCode, claimModifyId: selectedItemTableClaimModifyCode }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (!data.success) {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
            $("#tableModificationReinsuranceClaim").UifDataTable('clear');
        }
        else {
            if ((data.success) && data.result > 0) {
                processId = data.result;
                $('#tabsReinsClaims').UifTabHeader('disabled', '#tabModifyClaimReinsure', false);

                getClaimAllocations(processId, selectedItemTableMovementId, selectedItemTableClaimCoverageCd).done(function (response) {
                    if (response.success) {
                        if (response.result.length > 0) {
                            $("#tabModifyClaimReinsure").trigger('click');
                            $("#btnRecordReinsure").show();
                            $("#tableModificationReinsuranceClaim").UifDataTable({ sourceData: response.result });
                            $.UifNotify('show', { type: 'success', message: Resources.MessageProcessSuccessfully, autoclose: true });
                        }
                    }
                });

            } else {
                $.UifNotify('show', { type: 'info', message: Resources.PolicyIsNotReinsured, autoclose: true });
            }
        }
    }).always(function () {
        unlockScreen();
    });
});

$('#tableModificationReinsuranceClaim').on('rowEdit', function (event, selectedRow) {
    active = true;
    $("#layerNumber").val(selectedRow.LayerNumber);
    $("#lineId").val(selectedRow.LineId);
    $("#cumulusKey").val(selectedRow.CumulusKey);
    $("#contract").val(selectedRow.Contract);
    $("#levelNumber").val(selectedRow.LevelNumber);
    $("#movementSource").val(selectedRow.MovementSource);
    $("#newAmount").val(FormatMoney(parseFloat(selectedRow.Amount)));
    $("#sourceAmount").val(FormatMoney(parseFloat(selectedRow.Amount)));
    $("#variance").val(FormatMoney(parseFloat(variance)));
    $("#tempReinsuranceProcessId").val(selectedRow.TmpReinsuranceProcessId);
    $("#tempClaimReinsSourceId").val(selectedRow.TmpClaimReinsSourceId);
    $('#modalModificationReinsuranceLines').UifModal('showLocal', Resources.ModificationReinsurance);
    $("#btnReinsureClaim").hide();
    $("#btnRecordReinsure").show();
});

$("#modalModificationReinsuranceLines").find("#btnReinsClaim").click(function () {
    $("#formReinsClaim").validate();
    if ($("#formReinsClaim").valid()) {

        var tempClaimReinsSourceId = $("#tempClaimReinsSourceId").val();
        var newAmount = NotFormatMoney($("#newAmount").val());
        var sourceAmount = NotFormatMoney($("#sourceAmount").val());
        var newVariance = parseFloat(sourceAmount) - parseFloat(newAmount);
        variance = variance + newVariance;

        $.ajax({
            type: "POST",
            url: rootPath + "Reinsurance/Process/ModificationReinsuranceClaim",
            data: JSON.stringify({
                tempClaimReinsSourceId: tempClaimReinsSourceId,
                newAmount: newAmount
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        }).done(function (data) {
            if (data.success) {
                $('#modalModificationReinsuranceLines').UifModal('hide');
                if (variance == 0) {
                    $.UifNotify('show', { type: 'success', message: Resources.TotalDifferenceBetweenAmount + ': ' + FormatMoney(variance), autoclose: true });
                }
                else {
                    $.UifNotify('show', { type: 'danger', message: Resources.TotalDifferenceBetweenAmount + ': ' + FormatMoney(variance), autoclose: true });
                    $('#tabsReinsClaims').UifTabHeader('disabled', '#tabGeneralClm', true);
                }

                getClaimAllocations(processId, selectedItemTableMovementId, selectedItemTableClaimCoverageCd).done(function (response) {
                    if (response.success) {
                        if (response.result.length > 0) {
                            $("#tableModificationReinsuranceClaim").UifDataTable({ sourceData: response.result });
                        }
                    }
                });
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
            $('#tabsReinsClaims').UifTabHeader('disabled', '#tabGeneralClm', true);
        }
    }
    else {
        $.UifNotify('show', { type: 'danger', message: Resources.MessageReinsuranceNotRecord, autoclose: true });
    }
});

$("#btnConfirmRecordNo").click(function () {
    $.UifNotify('show', { type: 'warning', message: Resources.ReinsuranceNotSave, autoclose: true });
});

$("#btnConfirmRecordYes").click(function () {
    $('#modalRecordConfirmation').modal('hide');
    lockScreen();

    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/SaveClaimReinsurance",
        data: JSON.stringify({
            processId: processId,
            claimCode: selectedItemTableClaimCode,
            claimModifyCode: selectedItemTableClaimModifyCode
        }),
        dataType: "json", contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success == false) {
            $.UifNotify('show', { type: 'danger', message: data.result, autoclose: true });
        }
        else {
            processId = 0;
            getReinsuranceClaimDistributionByClaimCodeClaimModifyCode(selectedItemTableClaimCode, selectedItemTableClaimModifyCode, selectedItemTableMovementId, selectedItemTableClaimCoverageCd).done(function (response) {
                if (response.success) {
                    if (response.result.length > 0) {
                        $("#tableReinsuranceClaims").UifDataTable({ sourceData: response.result });
                        $("#btnRecordReinsure").hide();
                        $('#tabsReinsClaims').UifTabHeader('disabled', '#tabModifyClaimReinsure', true);
                        $('#tabGeneralClm').UifTabHeader('disabled', '#tabGeneralClm', false);
                        $("#tabGeneralClm").trigger('click');
                        $.UifNotify('show', { type: 'success', message: Resources.MessageReinsureClaimSuccess + ' ' + Resources.ReinsuranceId + ': ' + data.result, autoclose: true });
                    }
                }
            });
        }
    }).always(function () {
        unlockScreen();
    });
});

function getReinsuranceClaimDistributionByClaimCodeClaimModifyCode(claimId, claimModifyId, movementSourceId, claimCoverageCd) {
    return $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/GetReinsuranceClaimDistributionByClaimCodeClaimModifyCode",
        data: JSON.stringify(
            {
                claimCode: claimId,
                claimModifyCode: claimModifyId,
                movementSourceId: movementSourceId,
                claimCoverageCd: claimCoverageCd
            }
        ),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function getClaimAllocations(processId, movementSourceId, claimCoverageId) {
    return $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/GetClaimAllocations",
        data: JSON.stringify(
            {
                processId: processId,
                movementSourceId: movementSourceId,
                claimCoverageId: claimCoverageId
            }
        ),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}