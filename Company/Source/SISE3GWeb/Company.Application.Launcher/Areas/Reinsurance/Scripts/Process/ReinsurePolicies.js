var policyId = 0;
var endorsementId = 0;
var endorsementNumber = 0;
var policyNumber = 0;
var tempReinsuranceProcessId = 0;
var tempIssueLayerId = 0;
var tempLayerLineId = 0;
var tempFacultativeId = 0;
var selectEndorsement = false;
var titleLayer = "";
var titleLine = "";
var titleContract = "";
var titleFacultative = "";
var totSum = 0;
var totPremium = 0;
var contTot = 0;
var layerNumber = 0;
var lineId = 0;
var cumulusKey = "";
var msjConfirmFacultative = "";
var switchSave = true;
var countFacultative = 0;
var accountPolicyPromise;
var reinsCalculatedPromise;
var optionAccountPromise;
var reinsResultIdCounterpart = 0;
var reinsResultIdAdjustment = 0;
var reinsResultIdOriginal = 0;
var movementTypeCounterpart = "";
var movementTypeAdjustment = "";
var movementTypeOriginal = "";
var valuePremium = 0;
var valueAmount = 0;
var currentTo = null;
var indexFacultative = -1;
var tempFacultativeCompanyId = -1;
var quotaNumber = 0;
var dueDateLimit = null;
var sumValuePremium = 0;
var netPremiumPay = 0;
var isTotalPlanFacultative = true;
var isEdit = false;
var isCalcReinsurance = false;
var isPassed = false;
var closingDate;
var issueDate;

var monthName = [Resources.January, Resources.February, Resources.March, Resources.April, Resources.May, Resources.June, Resources.July, Resources.August, Resources.September, Resources.October, Resources.November, Resources.December];
var endorsementDateCurrentFrom;
var endorsementDateCurrentTo;

var oReinsuranceLine = {
    ReinsuranceLineId: 0,
    Line: {}
};

var oLine = {
    LineId: 0
};

var oReinsuranceFacultative = {
    TempFacultativeCompanyId: 0,
    TempFacultativeId: 0,
    BrokerReinsuranceId: 0,
    ReinsuranceCompanyId: 0,
    CommissionPercentage: 0,
    PremiumPercentage: 0,
    ParticipationPercentage: 0,
    PaymentDeadline: 0,
    DepositPercentage: 0,
    InterestOnReserve: 0,
    DepositReleaseDate: null
};

var oReinsuranceAllocation = {
    ReinsuranceAllocationId: 0,
    Sum: 0,
    PremiumAllocation: 0
};

var oReinsuranceLayerIssuance = {
    SumPercentage: 0,
    PremiumPercentage: 0,
    ReinsuranceLayerId: 0,
    ReinsSourceId: 0,
    LayerNumber: 0,
    TempReinsuranceProcessId: 0
};


$(document).ready(function () {
    disabledTabs(true);
    $("#DueDateReinsFacultative").mask("99/99/9999");
});

function searchEndorsement() {
    $('#tableEndorsement').UifDataTable('clear');
    $('#reinsurePoliciesTableReinsurances').UifDataTable('clear');
    $('#tableDistributionErrors').UifDataTable('clear');
    validateTabFacultative("N");

    if (($('#ReinsurePoliciesPolicyNumber').val() != "") && ($('#ReinsurePoliciesPolicyNumber').val() != null)) {

        var documentNumber = $('#ReinsurePoliciesPolicyNumber').val();
        var branchCode;
        var prefixCode;
        var endorsementNumber;

        if (($('#ReinsurePoliciesSelectBranch').val() != null) && ($('#ReinsurePoliciesSelectBranch').val() != "")) {
            branchCode = $('#ReinsurePoliciesSelectBranch').val();
        } else {
            branchCode = 0;
        }

        if (($('#ReinsurePoliciesSelectPrefix').val() != null) && ($('#ReinsurePoliciesSelectPrefix').val() != "")) {
            prefixCode = $('#ReinsurePoliciesSelectPrefix').val();
        } else {
            prefixCode = 0;
        }

        if (($('#ReinsurePoliciesEndorsementNumber').val() != "") && ($('#ReinsurePoliciesEndorsementNumber').val() != null)) {
            endorsementNumber = $('#ReinsurePoliciesEndorsementNumber').val();
        } else {
            endorsementNumber = -1;
        }

        if (isPassed == false) {
            cleanAll();
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
                            $("#alertReinsurePolicieSearch").UifAlert('show', Resources.PolicyNotExists, "danger");
                        }
                    } else {
                        $("#alertReinsurePolicieSearch").UifAlert('show', data.result, "danger");
                    }
                }
            });
        } else {
            $("#alertReinsurePolicieSearch").UifAlert('show', Resources.MessageSelectBranch + ' y ' + Resources.Prefix + ' ', "warning");
        }
    }
    else {
        $("#alertReinsurePolicieSearch").UifAlert('show', Resources.ErrorPolicyNumberRequired, "warning");
    }
}

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
                $("#ReinsurePoliciesSelectBranch").UifSelect("setSelected", data.result.Branch.Id);
                $("#ReinsurePoliciesSelectPrefix").UifSelect("setSelected", data.result.Prefix.Id);
            } else {

                $("#alertReinsurePolicieSearch").UifAlert('show', Resources.PolicyNotExists, "danger");
            }
        }
    }).fail(function (response) {
        $("#alertReinsurePolicieSearch").UifAlert('show', response.result, "danger");
    });
}

function reinsurancePolicy() {
    if (policyId != 0 && endorsementId != 0) {
        var processTypeId = 2;
        var controller = "";
        var dateFrom = "0";
        var dateTo = "0";

        $.ajax({
            type: 'POST',
            url: rootPath + "Reinsurance/Process/ReinsuranceProcess",
            data: JSON.stringify({
                policyId: policyId,
                endorsementId: endorsementId,
                processTypeId: processTypeId,
                dateFrom: dateFrom,
                dateTo: dateTo
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success == false) {
                    $("#alertReinsurePolicy").UifAlert('show', data.result, "danger");
                    $.unblockUI();
                }
                else {

                    if (data.success == true && data.result.ReinsuranceId > 0) {
                        $("#alertReinsurePolicy").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + data.result.ReinsuranceId, "success");
                        $("#panelReinsurances").show();
                        $("#panelDistributionErrors").hide();
                        $("#btnReinsurance").hide();

                        accountPolicyReinsurance(data.result.ReinsuranceId);
                        accountPolicyPromise.then(function (accountingData) {
                            $.unblockUI();
                            if (accountingData == Resources.UnhandledExceptionMsj || accountingData == Resources.AccountingIntegrationUnbalanceEntry
                                || accountingData == Resources.ParameterNotExist) {
                                $("#reinsurePolicyAccountingAlert").UifAlert('show', accountingData, "danger");
                                $("#ContractPolicyModificationAccountingAlert").UifAlert('show', accountingData, "danger");
                                $("#LinePolicyModificationAccountingAlert").UifAlert('show', accountingData, "danger");
                                $("#LayerPolicyModificationAccountingAlert").UifAlert('show', accountingData, "danger");

                            }
                            else {
                                $("#reinsurePolicyAccountingAlert").UifAlert('show', accountingData, "success");
                                $("#ContractPolicyModificationAccountingAlert").UifAlert('show', accountingData, "success");
                                $("#LinePolicyModificationAccountingAlert").UifAlert('show', accountingData, "success");
                                $("#LayerPolicyModificationAccountingAlert").UifAlert('show', accountingData, "success");
                            }
                        });

                        $.ajax({
                            type: 'POST',
                            url: rootPath + 'Reinsurance/Process/GetReinsuranceDistributionByEndorsementId',
                            data: JSON.stringify({ endorsementId: endorsementId }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                if (response.success) {
                                    if (response.result.length > 0) {
                                        if (issueDate >= closingDate) {
                                            $("#btnLoadReinsurance").hide();
                                        }
                                        $("#reinsurePoliciesTableReinsurances").UifDataTable({ sourceData: response.result });
                                    }
                                }
                            }
                        });
                    }
                    else if (data.result.ReinsuranceId == 0) //devuelve 0 si hay errores por el proceso
                    {
                        $.unblockUI();
                        $("#alertReinsurePolicy").UifAlert('show', Resources.ReinsuranceValidateLineCumulus, "danger");
                    }
                    else if (data.result.ReinsuranceId == -1) //devuelve -1 si hay errores de parametrización
                    {
                        $("#panelReinsurances").hide();
                        $("#panelDistributionErrors").show();
                        $("#alertReinsurePolicy").UifAlert('show', Resources.ReinsuranceNotSucess, "danger");
                        $.ajax({
                            type: 'POST',
                            url: rootPath + 'Reinsurance/Process/GetDistributionErrors',
                            data: JSON.stringify({ endorsementId: endorsementId }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                if (response.success) {
                                    if (response.result.length > 0) {
                                        $("#tableDistributionErrors").UifDataTable({ sourceData: response.result });
                                    }
                                }
                            }
                        });
                        $("#reinsurePolicyAccountingAlert").UifAlert('hide');
                        $.unblockUI();
                    }
                    else if (data.success == true && data.result.ReinsuranceId == undefined) { //error de parametrizacion
                        $.unblockUI();
                        $("#alertReinsurePolicy").UifAlert('show', data.result, "danger");
                        $("#reinsurePolicyAccountingAlert").UifAlert('hide');
                    }
                }
            }
        });
    }
    else {
        unlockScreen();
    }
}

function disabledTabs(switchParam) {
    $("#tabsIssReinsurance").UifTabHeader("disabled", "#tabTitleModificacion", switchParam);
}

function loadReinsuranceLayer(endorsementId) {

    lockScreen();

    $.ajax({
        type: 'POST',
        url: rootPath + "Reinsurance/Process/LoadReinsuranceLayer",
        data: JSON.stringify({ "endorsementId": endorsementId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success == false && data.result != null) {
                $.unblockUI();
                $("#alertReinsurePolicy").UifAlert('show', data.result, "danger");
            }
            else if (data.success == false && data.result == null) {
                $("#panelReinsurances").hide();
                $("#panelDistributionErrors").show();
                $("#alertReinsurePolicy").UifAlert('show', Resources.ReinsuranceNotSucess, "danger");
                $.ajax({
                    type: 'POST',
                    url: rootPath + 'Reinsurance/Process/GetDistributionErrors',
                    data: JSON.stringify({ endorsementId: endorsementId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response.success) {
                            if (response.result.length > 0) {
                                $("#tableDistributionErrors").UifDataTable({ sourceData: response.result });
                            }
                        }
                    }
                });
                $("#reinsurePolicyAccountingAlert").UifAlert('hide');
                $.unblockUI();

                $("#btnRecalculate").hide();
                $('#tabsIssReinsurance').UifTabHeader("disabled", "#tabTitleModificacion", true);

            }
            else if (data.success == true && data.result > 0) { //carga la vista REINS.GET_ISS_TMP_LAYER_DISTRIBUTION
                tempReinsuranceProcessId = data.result;

                recalculateReinsurance(endorsementId, policyId, tempReinsuranceProcessId);
                reinsCalculatedPromise.then(function (result) {
                    if (result == false) {
                        unlockScreen();
                    }
                    $("#btnLoadReinsurance").hide();
                    $("#btnRecalculate").show();
                    $("#btnReinsurance").hide();

                    $("#tabsIssReinsurance").UifTabHeader("disabled", "#tabTitleModificacion", false);

                    setTimeout(function () {
                        $("#tabTitleModificacion").trigger('click');
                    }, 500);
                    unlockScreen();
                });

            }
            else if (data.success == true) { //error de parametrizacion
                $.unblockUI();
                var msjErr = data.result;
                $("#alertReinsurePolicy").UifAlert('show', msjErr, "danger");
            }
        },
        error: function (xhr, status, errorThrown) {
            unlockScreen();
            $("#alertReinsurePolicy").UifAlert('show', Resources.GeneratesError + ' ' + errorThrown, "danger");
        }

    });
}

function recalculateReinsurance(endorsementId, policyId, tempReinsuranceProcessId) {
    return reinsCalculatedPromise = new Promise(function (resolve) {
        $.ajax({
            type: 'POST',
            url: rootPath + "Reinsurance/Process/RecalculateReinsurance",
            data: JSON.stringify({ "endorsementId": endorsementId, "policyId": policyId, "processId": tempReinsuranceProcessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var controller;
                if (data.success == false && data.result != null) {
                    $("#alertModification").UifAlert('show', data.result, "danger");
                    $("#alertReinsurePolicieLine").UifAlert('show', data.result, "danger");
                    $("#alertContract").UifAlert('show', data.result, "danger");
                    $("#alertFacultative").UifAlert('show', data.result, "danger");
                }
                else if (data.success == false && data.result == null) {
                    $("#panelReinsurances").hide();
                    $("#panelDistributionErrors").show();
                    $("#alertReinsurePolicy").UifAlert('show', Resources.ReinsuranceNotSucess, "danger");
                    $("#alertModification").UifAlert('show', Resources.ReinsuranceNotSucess, "danger");
                    $.ajax({
                        type: 'POST',
                        url: rootPath + 'Reinsurance/Process/GetDistributionErrors',
                        data: JSON.stringify({ endorsementId: endorsementId }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            if (response.success) {
                                if (response.result.length > 0) {
                                    $("#tableDistributionErrors").UifDataTable({ sourceData: response.result });
                                }
                            }
                        }
                    });
                    $("#reinsurePolicyAccountingAlert").UifAlert('hide');
                    $("#btnRecalculate").hide();
                    $('#tabsIssReinsurance').UifTabHeader("disabled", "#tabTitleModificacion", true);
                }
                else if (data.success == true) {

                    $("#alertModification").UifAlert('show', Resources.MessageProcessSuccessfully, "success");
                    $("#alertReinsurePolicieLine").UifAlert('show', Resources.MessageProcessSuccessfully, "success");
                    $("#alertContract").UifAlert('show', Resources.MessageProcessSuccessfully, "success");
                    $("#alertFacultative").UifAlert('show', Resources.MessageProcessSuccessfully, "success");
                    $("#alertReinsurePolicieSearch").UifAlert('show', Resources.MessageProcessSuccessfully, "success");

                    $.ajax({
                        type: 'POST',
                        url: rootPath + 'Reinsurance/Process/GetTempLayerDistribution',
                        data: JSON.stringify({ endorsementId: endorsementId }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            if (response.success) {
                                if (response.result.length > 0) {
                                    $("#tableReinsuranceLayer").UifDataTable({ sourceData: response.result });
                                }
                            }
                        }
                    });

                    if (tempIssueLayerId > 0) {
                        controller = rootPath + "Reinsurance/Process/GetTempLineCumulus?tempIssueLayerId=" + tempIssueLayerId;
                        $("#tableLineModificationReinsurance").UifDataTable({ source: controller });
                    }

                    if (tempLayerLineId > 0) {
                        controller = rootPath + "Reinsurance/Process/GetTempAllocation?tempLayerLineId=" + tempLayerLineId;
                        $("#tableContracts").UifDataTable({ source: controller });
                    }

                    $("#btnSaveReinsurance").show();
                    disabledTabs(false);
                }
                resolve(data.success);
            }
        });
    });

}

function setOptionsAccount(data) {
    return optionAccountPromise = new Promise(function (resolve) {
        var count = 0;
        var total = 0;
        isPassed = false;
        for (var i = 0; i < data.result.length; i++) {

            if (data.result[i].Movements == 2) {
                movementTypeCounterpart = Resources.MovementType + ": " + Resources.Counterpart;
                reinsResultIdCounterpart = data.result[i].ReinsuranceId;
            }
            else if (data.result[i].Movements == 3) {

                movementTypeAdjustment = Resources.MovementType + ": " + Resources.Adjustment;
                reinsResultIdAdjustment = data.result[i].ReinsuranceId;
            }
            else if (data.result[i].Movements == 1) {
                movementTypeOriginal = Resources.MovementType + ": " + Resources.Original;
                reinsResultIdOriginal = data.result[i].ReinsuranceId;
            }
            isPassed = true;
            $("#alertModification").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdOriginal + " - " + movementTypeOriginal, "success");
            $("#alertReinsurePolicieLine").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdOriginal + " - " + movementTypeOriginal, "success");
            $("#alertContract").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdOriginal + " - " + movementTypeOriginal, "success");
            $("#alertFacultative").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdOriginal + " - " + movementTypeOriginal, "success");
            count = count + 1;
        }
        if (data.result.length == count) {
            total = count;
        }
        resolve(total);

    });
}

function saveReinsurance(endorsementId) {
    $("#alertModification").UifAlert('hide');
    $("#alertReinsurePolicieLine").UifAlert('hide');
    $("#alertContract").UifAlert('hide');
    $("#alertFacultative").UifAlert('hide');
    $.ajax({
        type: 'POST',
        url: rootPath + "Reinsurance/Process/SaveReinsurance",
        data: JSON.stringify({ "endorsementId": endorsementId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success == false) {
                if (data.result == -1) {
                    setTimeout(function () {
                        $("#panelDistributionErrors").show();
                        $.ajax({
                            type: 'POST',
                            url: rootPath + 'Reinsurance/Process/GetDistributionErrors',
                            data: JSON.stringify({ endorsementId: endorsementId }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                if (response.success) {
                                    if (response.result.length > 0) {
                                        $("#tableDistributionErrors").UifDataTable({ sourceData: response.result });
                                    }
                                }
                            }
                        });
                    }, 1000);
                    $("#alertModification").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertReinsurePolicieLine").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertContract").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertFacultative").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertReinsurePolicieSearch").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");

                    $.unblockUI();
                }
                else if (data.result == null) {
                    $.unblockUI();
                    $("#alertModification").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
                    $("#alertReinsurePolicieLine").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
                    $("#alertContract").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
                    $("#alertFacultative").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
                }
                else {
                    $.unblockUI();
                    $("#alertModification").UifAlert('show', data.result, "danger");
                    $("#alertReinsurePolicieLine").UifAlert('show', data.result, "danger");
                    $("#alertContract").UifAlert('show', data.result, "danger");
                    $("#alertFacultative").UifAlert('show', data.result, "danger");
                }
            }
            else if (data.success == true) {
                if (data.result[0].ReinsuranceId == -1) {
                    setTimeout(function () {
                        $("#panelReinsurances").hide();
                        $("#reinsurePoliciesTableReinsurances").UifDataTable('clear');
                        $("#panelDistributionErrors").show();
                        $.ajax({
                            type: 'POST',
                            url: rootPath + 'Reinsurance/Process/GetDistributionErrors',
                            data: JSON.stringify({ endorsementId: endorsementId }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                if (response.success) {
                                    if (response.result.length > 0) {
                                        $("#tableDistributionErrors").UifDataTable({ sourceData: response.result });
                                    }
                                }
                            }
                        });
                    }, 1000);
                    $("#alertModification").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertReinsurePolicieLine").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertContract").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertFacultative").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");
                    $("#alertReinsurePolicieSearch").UifAlert('show', Resources.ReinsuranceNotSucess, "warning");

                    $.unblockUI();

                } else {
                    var sequentialNumber = data.result[0].Number;
                    $('#pnlLayers').text("");
                    $('#pnlLine').text("");
                    $('#pnlContract').text("");
                    $('#pnlFacultative').text("");
                    $('#pnlFacultativeContract').text("");
                    cleanAll();
                    if (sequentialNumber != 0) {
                        $("#reinsurePolicyFacultativeAlert").UifAlert('show', Resources.MessageSucessFacultativeNumber + ': ' + sequentialNumber, "success");
                    }
                    searchEndorsement();
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            url: rootPath + 'Reinsurance/Process/GetReinsuranceDistributionByEndorsementId',
                            data: JSON.stringify({ endorsementId: endorsementId }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                if (response.success) {
                                    if (response.result.length > 0) {
                                        if (issueDate >= closingDate) {
                                            $("#btnLoadReinsurance").hide();
                                        }
                                        $("#reinsurePoliciesTableReinsurances").UifDataTable({ sourceData: response.result });
                                    }
                                }
                            }
                        });
                    }, 1000);
                    var typeAlert = "";
                    setOptionsAccount(data);
                    optionAccountPromise.then(function (numberResult) {
                        if (numberResult > 0) {
                            setTimeout(function () {
                                if (reinsResultIdOriginal > 0) {
                                    var msjOriginal = Resources.ReinsuranceProcess + ": " + reinsResultIdOriginal;
                                    $.ajax({
                                        url: rootPath + "Reinsurance/Process/RecordReinsuranceEntry",
                                        data: { "processId": reinsResultIdOriginal },
                                        success: function (accountingData) {
                                            unlockScreen();

                                            if (accountingData == Resources.EntryRecordingError ||
                                                accountingData == Resources.UnhandledExceptionMsj ||
                                                accountingData == Resources.AccountingIntegrationUnbalanceEntry ||
                                                accountingData == Resources.ParameterNotExist) {
                                                typeAlert = "danger";
                                            }
                                            else {
                                                typeAlert = "success";
                                            }
                                            $("#reinsurePolicyModificationAccountingAlert").UifAlert('show', msjOriginal + " | " + accountingData, typeAlert);
                                            $("#ContractPolicyModificationAccountingAlert").UifAlert('show', msjOriginal + " | " + accountingData, typeAlert);
                                            $("#LinePolicyModificationAccountingAlert").UifAlert('show', msjOriginal + " | " + accountingData, typeAlert);
                                            $("#LayerPolicyModificationAccountingAlert").UifAlert('show', msjOriginal + " | " + accountingData, typeAlert);
                                        }
                                    });
                                }
                                else {

                                    $("#alertModification").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdCounterpart + " - " + movementTypeCounterpart + ' | ' + Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdAdjustment + " - " + movementTypeAdjustment, "success");
                                    $("#alertReinsurePolicieLine").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdCounterpart + " - " + movementTypeCounterpart + ' | ' + Resources.MessageProcessSuccessfullyReinsurance + ": " + reinsResultIdAdjustment + " - " + movementTypeAdjustment, "success");
                                    $("#alertContract").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdCounterpart + " - " + movementTypeCounterpart + ' | ' + Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdAdjustment + " - " + movementTypeAdjustment, "success");
                                    $("#alertFacultative").UifAlert('show', Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdCounterpart + " - " + movementTypeCounterpart + ' | ' + Resources.MessageProcessSuccessfullyReinsurance + ': ' + reinsResultIdAdjustment + " - " + movementTypeAdjustment, "success");

                                    if (reinsResultIdCounterpart > 0) {
                                        $.ajax({
                                            url: rootPath + "Reinsurance/Process/RecordReinsuranceEntry",
                                            data: { "processId": reinsResultIdCounterpart },
                                            success: function (accountingData) {

                                                if (accountingData == Resources.EntryRecordingError ||
                                                    accountingData == Resources.UnhandledExceptionMsj ||
                                                    accountingData == Resources.AccountingIntegrationUnbalanceEntry ||
                                                    accountingData == Resources.ParameterNotExist) {
                                                    unlockScreen();
                                                    typeAlert = "danger";
                                                    return;
                                                }
                                                else {
                                                    typeAlert = "success";
                                                }

                                                $("#reinsurePolicyModificationAccountingAlert").UifAlert('show', Resources.ReinsuranceProcess + ": " + reinsResultIdCounterpart + " " + accountingData, typeAlert);
                                                $("#ContractPolicyModificationAccountingAlert").UifAlert('show', Resources.ReinsuranceProcess + ": " + reinsResultIdCounterpart + " " + accountingData, typeAlert);
                                                $("#LinePolicyModificationAccountingAlert").UifAlert('show', Resources.ReinsuranceProcess + ": " + reinsResultIdCounterpart + " " + accountingData, typeAlert);
                                                $("#LayerPolicyModificationAccountingAlert").UifAlert('show', Resources.ReinsuranceProcess + ": " + reinsResultIdCounterpart + " " + accountingData, typeAlert);
                                                var msjCounterpart = Resources.ReinsuranceProcess + ": " + reinsResultIdCounterpart + ", " + accountingData;

                                                if (reinsResultIdAdjustment > 0) {

                                                    $.ajax({
                                                        url: rootPath + "Reinsurance/Process/RecordReinsuranceEntry",
                                                        data: { "processId": reinsResultIdAdjustment },
                                                        success: function (accountingData) {
                                                            unlockScreen();
                                                            if (accountingData == Resources.EntryRecordingError ||
                                                                accountingData == Resources.UnhandledExceptionMsj ||
                                                                accountingData == Resources.AccountingIntegrationUnbalanceEntry ||
                                                                accountingData == Resources.ParameterNotExist) {

                                                                typeAlert = "danger";
                                                            } else {

                                                                typeAlert = "success";
                                                            }
                                                            $("#reinsurePolicyModificationAccountingAlert").UifAlert('show', msjCounterpart + " | " + Resources.ReinsuranceProcess + ": " + reinsResultIdAdjustment + ", " + accountingData, typeAlert);
                                                            $("#ContractPolicyModificationAccountingAlert").UifAlert('show', msjCounterpart + " | " + Resources.ReinsuranceProcess + ": " + reinsResultIdAdjustment + ", " + accountingData, typeAlert);
                                                            $("#LinePolicyModificationAccountingAlert").UifAlert('show', msjCounterpart + " | " + Resources.ReinsuranceProcess + ": " + reinsResultIdAdjustment + ", " + accountingData, typeAlert);
                                                            $("#LayerPolicyModificationAccountingAlert").UifAlert('show', msjCounterpart + " | " + Resources.ReinsuranceProcess + ": " + reinsResultIdAdjustment + ", " + accountingData, typeAlert);
                                                        }
                                                    });
                                                }
                                                else {
                                                    unlockScreen();
                                                }

                                            }
                                        });
                                    }
                                    else {
                                        unlockScreen();
                                    }

                                }

                            }, 2000);
                        }
                    });
                    countFacultative = 0;
                }
            }
        },
        error: function (xhr, status, errorThrown) {
            unlockScreen();
            $("#alertModification").UifAlert('show', Resources.GeneratesError + ' ' + errorThrown, "danger");
        }
    });
}

function cleanAll() {
    countFacultative = 0;
    policyId = 0;
    endorsementId = 0;
    endorsementNumber = 0;
    policyNumber = 0;
    tempReinsuranceProcessId = 0;
    tempIssueLayerId = 0;
    tempLayerLineId = 0;
    tempFacultativeId = 0;
    selectEndorsement = false;
    titleLayer = "";
    titleLine = "";
    titleContract = "";
    titleFacultative = "";
    totSum = 0;
    totPremium = 0;
    contTot = 0;
    layerNumber = 0;
    lineId = 0;
    cumulusKey = "";
    msjConfirmFacultative = "";
    $("#btnReinsurance").hide();
    $("#btnRecalculate").hide();
    $("#btnLoadReinsurance").hide();
    $("#btnSaveReinsurance").hide();
    $("#panelDistributionErrors").hide();
    $("#alertReinsurePolicieSearch").UifAlert('hide');
    $("#alertReinsurePolicy").UifAlert('hide');
    $("#alertReinsuranceDate").UifAlert('hide');
    $("#alertModification").UifAlert('hide');
    $("#LayerPolicyModificationAccountingAlert").UifAlert('hide');
    $("#alertReinsurePolicieLine").UifAlert('hide');
    $("#LinePolicyModificationAccountingAlert").UifAlert('hide');
    $("#alertContract").UifAlert('hide');
    $("#ContractPolicyModificationAccountingAlert").UifAlert('hide');
    $("#alertFacultative").UifAlert('hide');
    $("#reinsurePolicyAccountingAlert").UifAlert('hide');
    $("#reinsurePolicyModificationAccountingAlert").UifAlert('hide');
    $("#reinsurePolicyFacultativeAlert").UifAlert('hide');
    //Facultativo
    $("#txtFacultativeDesciption").val("");
    $("#txtValueAmountPercentage").val("");
    $("#txtValueAmount").val("");
    $("#txtPremiumPercentage").val("");
    $("#txtValuePremium").val("");
    //limpia grilla capas
    $('#tableReinsuranceLayer').UifDataTable('clear');
    //limpiar grilla de líneas
    $('#tableLineModificationReinsurance').UifDataTable('clear');
    //limpiar grilla de contratos
    $('#tableContracts').UifDataTable('clear');
    //limpiar grilla de endosos
    $('#tableEndorsement').UifDataTable('clear');
    //limpiar grilla de reaseguros
    $('#reinsurePoliciesTableReinsurances').UifDataTable('clear');
    //limpiar grilla de reaseguros errores
    $('#tableDistributionErrors').UifDataTable('clear');
    //CARGA GRILLA COMPANIAS
    $('#tableFacultatives').UifDataTable('clear');
    //bloquea pestaña de modificación
    $('#tabsIssReinsurance').UifTabHeader("disabled", "#tabTitleModificacion", true);
}

function onAddCompleteTempIssueLayer(data) {
    if (data.success) {
        $('#modalReinsuranceLayer').UifModal('hide');
        $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetTempLayerDistribution',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    if (response.result.length > 0) {
                        $("#tableReinsuranceLayer").UifDataTable({ sourceData: response.result });
                    }
                }
            }
        });

        if (data.result[0].LayerPercentage != 100 || data.result[0].PremiumPercentage != 100) {
            $("#alertModification").UifAlert('show', Resources.MsgCompleteSumPrime, "warning");
            $('#tableLineModificationReinsurance').UifDataTable('clear');
            $('#tableContracts').UifDataTable('clear');
            switchSave = false;
        }
        else {
            switchSave = true;
        }
    }
    else if (!data.success) {
        $("#alertModificationDialog").UifAlert('show', Resources.ValidPrimeSum, "warning");
        $("#alertModification").UifAlert('show', Resources.ValidPrimeSum, "warning");
    }
}

function validateFormTempIssueLayer() {
    $("#alertModificationDialog").UifAlert('hide');
    if ($("#layerPremiumPercentage").val() == "" || $("#layerSumPercentage").val() == "") {
        $("#alertModificationDialog").UifAlert('show', Resources.MessageValidateParticipationPremium, "warning");
        return false;
    }
    if (parseFloat($("#layerSumPercentage").val()) > 100 || parseFloat($("#layerPremiumPercentage").val()) > 100) {
        $("#alertModificationDialog").UifAlert('show', Resources.MessageValidateParticipationPremium, "warning");
        return false;
    }
    if (parseFloat($("#layerPremiumPercentage").val()) == 0 && parseFloat($("#layerSumPercentage").val()) == 0) {
        $("#alertModificationDialog").UifAlert('show', Resources.ValidInvalidValue, "warning");
        return false;
    }
    return true;
}

function onAddCompleteTempLayerLine(data) {
    var json = data;
    if (json.success) {
        $("#modalLineReinsurance").UifModal('hide');
        var controller = rootPath + "Reinsurance/Process/GetTempLineCumulus?tempIssueLayerId=" + tempIssueLayerId;
        $("#tableLineModificationReinsurance").UifDataTable({ source: controller });
        $("#alertReinsurePolicieLine").UifAlert('show', Resources.MsgCompleteReinsuranceLine, "warning");
    }
    else if (json.success == false) {
        $("#alertReinsurePolicieLine").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
    }
}

function onAddCompleteTempIssueAllocation(data) {

    if (data.success) {

        $("#modalReinsuranceContract").UifModal('hide');
        var controller = rootPath + "Reinsurance/Process/GetTempAllocation?tempLayerLineId=" + tempLayerLineId;

        $("#tableContracts").UifDataTable({ source: controller });
        $("#alertContract").UifAlert('show', Resources.MsgCompleteContractAllocation, "warning");

        //compara con los totales originales
        $.ajax({
            url: rootPath + "Reinsurance/Process/GetTotSumPrimeAllocation",
            data: { "tempLayerLineId": tempLayerLineId },
            success: function (response) {
                if ((totSum != response.aaData[0].TotalSum) || totPremium != response.aaData[0].TotalPremium) {

                    var msjTotValues = Resources.ValueTotSum + ': ' + FormatCurrency(FormatDecimal(response.aaData[0].TotalSum)) + " " + Resources.AndMustTotal
                        + ': ' + FormatCurrency(FormatDecimal(totSum)) + ' / ' + Resources.ValueTotPremium + ': ' + FormatCurrency(FormatDecimal(response.aaData[0].TotalPremium))
                        + ' ' + Resources.AndMustTotal + ': ' + FormatCurrency(FormatDecimal(totPremium));

                    $("#alertContract").UifAlert('show', msjTotValues, "warning");
                    switchSave = false;
                    $("#btnSaveReinsurance").hide();
                }
                else {
                    switchSave = true;
                    $("#btnSaveReinsurance").show();
                }
            }
        });
    } else if (data.success == false) {
        $("#alertContract").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
    }
}

function accountPolicyReinsurance(reinsuranceId) {
    return accountPolicyPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: rootPath + "Reinsurance/Process/RecordReinsuranceEntry",
            data: { "processId": reinsuranceId },
            success: function (accountingData) {
                resolve(accountingData);
            }
        });
    });
}

function GetModificationReinsuranceContractDialog(tempIssueAllocationId) {
    $.ajax({
        url: REINS_ROOT + 'Process/ModificationReinsuranceContractDialog',
        data: {
            'tempIssueAllocationId': tempIssueAllocationId
        },
    }).done(function (data) {
        $("#ReinsuranceAllocationId").val(data.ReinsuranceAllocationId);
        $("#Sum").val(FormatMoney(data.TotalSum));
        $("#Premium").val(FormatMoney(data.TotalPremium));
        $('#modalReinsuranceContract').UifModal('showLocal', Resources.UpdateContractReinsurance);
    });
}

function GetModificationReinsuranceLayer(endorsementId, tempIssueLayerId, title) {
    $.ajax({
        type: 'POST',
        url: REINS_ROOT + 'Process/ModificationReinsuranceLayer',
        data: JSON.stringify({ 'endorsementId': endorsementId, 'tempIssueLayerId': tempIssueLayerId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#ReinsLayerId").val(data.result[0].ReinsuranceLayerId);
                $("#ReinsLayerNumber").val(data.result[0].LayerNumber);
                $("#ReinsSourceId").val(data.result[0].ReinsSourceId);
                $('#modalReinsuranceLayer').UifModal('showLocal', title);
            }
        }
    });
}

function GetModificationReinsuranceLineDialog(tempLayerLineId) {
    $.ajax({
        url: REINS_ROOT + 'Process/ModificationReinsuranceLineDialog',
        data: {
            'tempLayerLineId': tempLayerLineId
        },
    }).done(function (data) {
        $("#ReinsuranceLineId").val(data.ReinsuranceLineId);
        var controller = rootPath + "Reinsurance/Process/GetReinsuranceLines";
        if (data.Line.LineId > 0) {
            $("#LineId").UifSelect({ source: controller, selectedId: data.Line.LineId });
        }
        else {
            $("#LineId").UifSelect({ source: controller });
        }

        $.ajax({
            type: "POST",
            url: rootPath + "Reinsurance/Process/GetReinsuranceLines",
        }).done(function (data) {
            if (data.data.length > 0) {

                $("#LineId").UifSelect({
                    sourceData: data.data,
                    id: "LineId",
                    name: "Description",
                    enable: true,
                });

                if (lineId > 0) {
                    $("#LineId").UifSelect("setSelected", lineId);
                }

                $('#modalLineReinsurance').UifModal('showLocal', Resources.UpdateLineReinsurance);
            }
            else {
                $("#LineId").UifSelect();
                $("#LinePolicyModificationAccountingAlert").UifAlert('show', Resources.TheresNotCurrenteLine, "warning");
            }
        });
    });
}

$("#btnReinsurance").hide();
$("#btnLoadReinsurance").hide();
$("#btnRecalculate").hide();
$("#btnSaveReinsurance").hide();

$("#ReinsurePoliciesSearch").click(function () {
    searchEndorsement();
});

$("#btnReinsurance").click(function () {
    if (selectEndorsement) {
        var currentDate = new Date();
        var DateSplitFrom = endorsementDateCurrentFrom.split('/');
        var DateSplitTo = endorsementDateCurrentTo.split('/');
        var DateFrom = new Date(DateSplitFrom[1] + '/' + DateSplitFrom[0] + '/' + DateSplitFrom[2]);
        var DateTo = new Date(DateSplitTo[1] + '/' + DateSplitTo[0] + '/' + DateSplitTo[2]);

        $("#ConfirmPolicyNotcurrent").hide();
        $("#ConfirmMessageProcess").show();

        if (currentDate < DateFrom || currentDate > DateTo) {
            $("#ConfirmPolicyNotcurrent").show();
            $("#ConfirmMessageProcess").hide();
        }
        $('#modalReinsurance').appendTo("body").UifModal('showLocal');

    } else {
        $("#alertReinsurePolicy").UifAlert('show', Resources.SelectPolicy, "warning");
    }
});

$("#btnModalReinsurance").click(function () {
    $('#modalReinsurance').modal('hide');
    lockScreen();
    setTimeout(function () {
        reinsurancePolicy();
    }, 1000);

});

$('#tableEndorsement').on('rowSelected', function (event, selectedRow) {
    $("#alertReinsurePolicy").UifAlert('hide');
    $("#reinsurePolicyAccountingAlert").UifAlert('hide');
    $("#panelDistributionErrors").hide();
    $('#reinsurePoliciesTableReinsurances').UifDataTable('clear');
    closingDate = $("#ViewBagReinsuranceDate").val();
    issueDate = moment(selectedRow.IssueDate).format("DD/MM/YYYY");
    var closingDateSplit = closingDate.split('/');
    issueDate = moment(selectedRow.IssueDate).format("DD/MM/YYYY");
    var issueDateSplit = issueDate.split('/');
    closingDate = new Date(closingDateSplit[1] + '/' + closingDateSplit[0] + '/' + closingDateSplit[2]);
    issueDate = new Date(issueDateSplit[1] + '/' + issueDateSplit[0] + '/' + issueDateSplit[2]);
    if (issueDate <= closingDate) {
        $("#alertReinsuranceDate").UifAlert('show', Resources.EndorsementClosedMonth + "( " + Resources.MonthClosing + ": " + monthName[closingDate.getMonth()] + " " + closingDate.getFullYear() + " )", "danger");
        $("#btnReinsurance").attr("disabled", "disabled");
    }
    else {
        $("#alertReinsuranceDate").UifAlert('hide');
        $("#btnReinsurance").removeAttr("disabled");
    }
    selectEndorsement = true;
    endorsementDateCurrentFrom = moment(selectedRow.CurrentFrom).format("DD/MM/YYYY");
    endorsementDateCurrentTo = moment(selectedRow.CurrentTo).format("DD/MM/YYYY");
    policyId = selectedRow.PolicyId;
    endorsementId = selectedRow.EndorsementId;
    endorsementNumber = selectedRow.EndorsementNumber;
    policyNumber = $("#ReinsurePoliciesPolicyNumber").val();
    $("#DateDepositRelease").UifDatepicker('setValue', selectedRow.CurrentTo);
    $('#pnlLayers').text(Resources.PolicyNumber + ': ' + policyNumber + ' / ' + Resources.Endorsement + ': ' + endorsementNumber);

    $.ajax({
        type: 'POST',
        url: rootPath + "Reinsurance/Process/GetReinsuranceDistributionByEndorsementId",
        data: JSON.stringify({ "endorsementId": endorsementId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                endorsementNumber = "";
                policyNumber = "";
                $("#btnReinsurance").show();
                if (data.result.length > 0) {
                    if (issueDate > closingDate) {
                        $("#btnLoadReinsurance").hide();
                    }
                    $("#panelDistributionErrors").hide();
                    $("#panelReinsurances").show();
                    $("#btnReinsurance").hide();
                    $("#reinsurePoliciesTableReinsurances").UifDataTable({ sourceData: data.result });
                }
            } else {
                $("#alertReinsurePolicy").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
    $("#btnReinsurance").show();
    $("#btnLoadReinsurance").show();
});

$("#btnLoadReinsurance").click(function () {
    $('#modalLoadReinsurance').appendTo("body").modal('show');
});

$("#btnModalLoadReinsurance").click(function () {
    $('#modalLoadReinsurance').modal('hide');
    $('#alertModification').UifAlert('hide');
    $("#reinsurePolicyModificationAccountingAlert").UifAlert('hide');
    $("#ContractPolicyModificationAccountingAlert").UifAlert('hide');
    $("#LinePolicyModificationAccountingAlert").UifAlert('hide');
    $("#LayerPolicyModificationAccountingAlert").UifAlert('hide');
    loadReinsuranceLayer(endorsementId);
});

$('#tableReinsuranceLayer').on('rowAdd', function (event, data) {
    $('#alertModification').UifAlert('hide');
    if (tempReinsuranceProcessId > 0) {
        $.ajax({
            type: 'POST',
            url: rootPath + "Reinsurance/Process/GetTempLayerDistribution",
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    if (response.result.length > 0) {
                        var layerPerTop = 0;
                        var premiumPerTop = 0;
                        for (var i = 0; i < response.result.length; i++) {
                            layerPerTop = layerPerTop + response.result[i].LayerPercentage;
                            premiumPerTop = premiumPerTop + response.result[i].PremiumPercentage;
                        }
                        if (layerPerTop < 100 || premiumPerTop < 100) {
                            GetModificationReinsuranceLayer(endorsementId, 0, Resources.AddLayerReinsurance);
                        } else {
                            $("#alertModification").UifAlert('show', Resources.ValidPrimeSum, "warning");
                        }
                    } else {
                        $("#alertModification").UifAlert('show', Resources.MsgExecuteLoadLayer, "warning");
                    }
                }
            }
        });
    } else {
        $("#alertModification").UifAlert('show', Resources.MsgExecuteLoadLayer, "warning");
    }
});

$('#tableReinsuranceLayer').on('rowEdit', function (event, selectedRow) {

    $('#alertModification').UifAlert('hide');
    tempIssueLayerId = selectedRow.TempIssueLayerId;
    layerNumber = selectedRow.LayerNumber;
    $("#btnSaveReinsurance").hide();

    $("#layerSumPercentage").val(selectedRow.LayerPercentage);
    $("#layerPremiumPercentage").val(selectedRow.PremiumPercentage);

    GetModificationReinsuranceLayer(endorsementId, tempIssueLayerId, Resources.UpdateLayerReinsurance);
});

$('#tableReinsuranceLayer').on('rowDelete', function (event, data) {
    $("#alertModification").UifAlert('hide');
    $('#modalDeleteReinsuranceLayer').appendTo("body").UifModal('showLocal');
    tempIssueLayerId = data.TempIssueLayerId;
});

$("#btnModalReinsuranceDelete").click(function () {
    $('#modalDeleteReinsuranceLayer').modal('hide');
    $.ajax({
        type: 'POST',
        url: rootPath + "Reinsurance/Process/DeleteTempIssueLayer",
        data: JSON.stringify({ endorsementId: endorsementId, tempIssueLayerId: tempIssueLayerId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                $.ajax({
                    type: 'POST',
                    url: rootPath + "Reinsurance/Process/GetTempLayerDistribution",
                    data: JSON.stringify({ endorsementId: endorsementId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response.success) {
                            if (response.result.length > 0) {

                                var layerPerTop = 0;
                                var premiumPerTop = 0;

                                for (var i = 0; i < response.result.length; i++) {
                                    layerPerTop = layerPerTop + response.result[i].LayerPercentage;
                                    premiumPerTop = premiumPerTop + response.result[i].PremiumPercentage;
                                }

                                if (layerPerTop != 100 || premiumPerTop != 100) {

                                    switchSave = false;

                                } else {
                                    switchSave = true;
                                }

                                $("#tableReinsuranceLayer").UifDataTable({ sourceData: response.result });
                                setTimeout(function () {
                                    disabledTabs(false);
                                }, 600);

                            }
                        }
                    }
                });
            } else {
                $("#alertModification").UifAlert('show', Resources.ValidateNoDeleteReinsuranceLayer, "warning");
            }
        }
    });
});

$('#tableReinsuranceLayer').on('rowSelected', function (event, selectedRow) {

    $('#alertReinsurePolicieLine').UifAlert('hide');
    $("#alertModification").UifAlert('hide');
    $("#alertContract").UifAlert('hide');
    $("#alertFacultative").UifAlert('hide');
    $("#alertReinsurePolicieSearch").UifAlert('hide')
    titleLine = titleLayer + ' / ' + Resources.LayerNumber + ': ' + selectedRow.LayerNumber;
    $('#pnlLine').text(titleLine);
    tempIssueLayerId = selectedRow.TempIssueLayerId;
    layerNumber = selectedRow.LayerNumber;
    var controller = rootPath + "Reinsurance/Process/GetTempLineCumulus?tempIssueLayerId=" + tempIssueLayerId;
    $("#tableLineModificationReinsurance").UifDataTable({ source: controller });
    disabledTabs(false);
});

$("#btnRecalculate").click(function () {
    validatePaymentPlan().then(function (result) {
        if (result) {
            $('#modalRecalculateReinsurance').appendTo("body").UifModal('showLocal');
        }

    });

});

$("#btnModalRecalculateReinsurance").click(function () {
    $('#modalRecalculateReinsurance').modal('hide');
    isCalcReinsurance = true;
    lockScreen();
    var desciption = $("#txtFacultativeDesciption").val();
    var sumPercentage = $("#txtValueAmountPercentage").val() == "" ? 0 : parseFloat(NotFormatMoney($("#txtValueAmountPercentage").val()).replace(',', '.'));
    var premiumPercentage = $("#txtPremiumPercentage").val() == "" ? 0 : parseFloat(NotFormatMoney($("#txtPremiumPercentage").val()).replace(',', '.'));

    $("#formCalculateFacultative").validate();
    if (lineId > 0 && sumPercentage > 0 && premiumPercentage > 0) {
        if ($("#formCalculateFacultative").valid()) {
            loadFacultative(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, desciption, sumPercentage, premiumPercentage).then((tempFacultativeId) => {
                if (tempFacultativeId <= 0) {
                    $("#alertFacultative").UifAlert('show', Resources.MsgExecuteLoadFacultative, "warning");
                } else {
                    recalculateReinsurance(endorsementId, policyId, tempReinsuranceProcessId);
                    reinsCalculatedPromise.then(function (result) {
                        if (result == false) {
                            unlockScreen();
                        }
                        if (switchSave) {
                            $("#btnSaveReinsurance").show();
                        }
                        else {
                            $("#alertModification").UifAlert('show', Resources.ValidPercentajeLayers, "warning");
                            $("#alertReinsurePolicieLine").UifAlert('show', Resources.ValidPercentajeLayers, "warning");
                            $("#alertContract").UifAlert('show', Resources.ValidPercentajeLayers, "warning");
                            $("#alertFacultative").UifAlert('show', Resources.ValidPercentajeLayers + ' / ' + Resources.ValidFacultativePercentage, "warning");
                            $("#btnSaveReinsurance").hide();
                        }

                        disabledTabs(false);
                        unlockScreen();
                    });
                }
            }).catch((result) => {
                $("#alertFacultative").UifAlert('show', result, "warning");
            });
        }
    } else {

        recalculateReinsurance(endorsementId, policyId, tempReinsuranceProcessId);
        reinsCalculatedPromise.then(function (result) {
            if (result == false) {
                unlockScreen();
            }
            if (switchSave) {
                $("#btnSaveReinsurance").show();
            }
            else {
                $("#alertModification").UifAlert('show', Resources.ValidPercentajeLayers, "warning");
                $("#alertReinsurePolicieLine").UifAlert('show', Resources.ValidPercentajeLayers, "warning");
                $("#alertContract").UifAlert('show', Resources.ValidPercentajeLayers, "warning");
                $("#alertFacultative").UifAlert('show', Resources.ValidPercentajeLayers + ' / ' + Resources.ValidFacultativePercentage, "warning");
                $("#btnSaveReinsurance").hide();
            }

            disabledTabs(false);
            unlockScreen();
        });

    }
});

$("#btnSaveReinsurance").click(function () {

    validatePaymentPlan().then(function (result) {
        if (result) {
            if (switchSave) {
                if (isTotalPlanFacultative) {
                    $('#modalSaveReinsurance').UifModal('showLocal');
                }
                else {
                    $("#alertFacultative").UifAlert('show', Resources.PaymentPlansPleaseCheck, "warning");
                }
            }
            else {
                $("#alertFacultative").UifAlert('show', Resources.ValidFacultativePercentage, "warning");
                $("#alertModification").UifAlert('show', Resources.ValidFacultativePercentage, "warning");
            }

        }

    });

});

$("#btnModalCancelReinsurance").click(function () {
    $("#alertReinsurePolicy").UifAlert('show', Resources.ReinsuranceNotSave, "warning");
    $("#alertFacultative").UifAlert('show', Resources.ReinsuranceNotSave, "warning");
    $("#alertModification").UifAlert('show', Resources.ReinsuranceNotSave, "warning");
    $("#alertReinsurePolicieLine").UifAlert('show', Resources.ReinsuranceNotSave, "warning");
    $("#alertContract").UifAlert('show', Resources.ReinsuranceNotSave, "warning");
});

$("#btnModalNotRecalculateReinsurance").click(function () {
    $("#alertReinsurePolicy").UifAlert('show', Resources.ReinsuranceNotRecalculate, "warning");
    $("#alertFacultative").UifAlert('show', Resources.ReinsuranceNotRecalculate, "warning");
    $("#alertModification").UifAlert('show', Resources.ReinsuranceNotRecalculate, "warning");
    $("#alertReinsurePolicieLine").UifAlert('show', Resources.ReinsuranceNotRecalculate, "warning");
    $("#alertContract").UifAlert('show', Resources.ReinsuranceNotRecalculate, "warning");
});

$("#btnModalSaveReinsurance").click(function () {

    $('#modalSaveReinsurance').modal('hide');
    //Lanza recálculo
    lockScreen();

    setTimeout(function () {
        recalculateReinsurance(endorsementId, policyId, tempReinsuranceProcessId);
        reinsCalculatedPromise.then(function (result) {
            if (result == true) {
                setTimeout(function () {
                    saveReinsurance(endorsementId);
                }, 1000);
            }
            else {
                unlockScreen();
            }
        });
    }, 1000);

});

$('#tableLineModificationReinsurance').on('rowEdit', function (event, selectedRow) {
    $('#alertReinsurePolicieLine').UifAlert('hide');
    $("#alertModification").UifAlert('hide');
    $("#alertContract").UifAlert('hide');
    $("#alertFacultative").UifAlert('hide');

    GetModificationReinsuranceLineDialog(selectedRow.TempLayerLineId);
});

$('#tableLineModificationReinsurance').on('rowSelected', function (event, selectedRow) {

    tempLayerLineId = selectedRow.TempLayerLineId;
    lineId = selectedRow.LineId;
    cumulusKey = selectedRow.CumulusKey;

    titleContract = titleLine + ' / ' + Resources.Line + ': ' + selectedRow.LineDescription;
    $('#pnlContract').text(titleContract);
    $('#pnlFacultativeContract').text(titleContract);

    var controller = rootPath + "Reinsurance/Process/GetTempAllocation?tempLayerLineId=" + tempLayerLineId;
    $("#tableContracts").UifDataTable({ source: controller });

   getTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey);
    getSlips(endorsementId);
    validateTabFacultative("S");
    disabledTabs(false);

});

$('#tableContracts').on('rowEdit', function (event, selectedRow) {

    $('#alertContract').UifAlert('hide');

    if (contTot == 0) {
        totSum = selectedRow.TotalSum;
        totPremium = selectedRow.TotalPremium;
    }

    contTot = contTot + 1;
    if (selectedRow.Facultative == true) {
        $("#alertContract").UifAlert('show', Resources.WarningFacultativeContract, "warning");
    }
    else {
        GetModificationReinsuranceContractDialog(selectedRow.ReinsuranceAllocationId);
    }

});

$('#modalReinsuranceContract').on('focusout', '#Sum ', function (event, selectedItem) {
    $(this).val(FormatMoney($(this).val()));

});

$('#modalReinsuranceContract').on('focusin', '#Sum ', function (event, selectedItem) {
    $(this).val(NotFormatMoney($(this).val()));
});

$('#modalReinsuranceContract').on('focusout', '#Premium ', function (event, selectedItem) {
    $(this).val(FormatMoney($(this).val()));

});

$('#modalReinsuranceContract').on('focusin', '#Premium ', function (event, selectedItem) {
    $(this).val(NotFormatMoney($(this).val()));
});

$("#modalReinsuranceContract").on("click", "#btnReinsAllocation", function (event, selectedItem) {

    if ($("#formReinsAllocation").valid()) {
        //ARMA EL OBJETO
        oReinsuranceAllocation.ReinsuranceAllocationId = $("#ReinsuranceAllocationId").val();
        oReinsuranceAllocation.Sum = NotFormatMoney($("#Sum").val());
        oReinsuranceAllocation.PremiumAllocation = NotFormatMoney($("#Premium").val());

        $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "Reinsurance/Process/UpdateTempAllocation",
            data: JSON.stringify({ "reinsuranceAllocationDTO": oReinsuranceAllocation }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                onAddCompleteTempIssueAllocation(data);

                setTimeout(function () {
                    disabledTabs(false);
                }, 600);
            }
        });
    }
});

$("#modalReinsuranceLayer").find("#btnReinsLayer").click(function () {

    $("#tempReinsuranceProcessId").val(tempReinsuranceProcessId);
    if ($("#formReinsLayer").valid()) {
        if (validateFormTempIssueLayer()) {
            oReinsuranceLayerIssuance.SumPercentage = parseFloat(NotFormatMoney($("#layerSumPercentage").val()).replace(',', '.'));
            oReinsuranceLayerIssuance.PremiumPercentage = parseFloat(NotFormatMoney($("#layerPremiumPercentage").val()).replace(',', '.')); 
            oReinsuranceLayerIssuance.ReinsuranceLayerId = $("#ReinsLayerId").val();
            oReinsuranceLayerIssuance.ReinsSourceId = $("#ReinsSourceId").val();
            oReinsuranceLayerIssuance.LayerNumber = $("#ReinsLayerNumber").val();
            oReinsuranceLayerIssuance.TempReinsuranceProcessId = tempReinsuranceProcessId;

            $.ajax({
                type: "POST",
                url: rootPath + "Reinsurance/Process/SaveTempIssueLayer",
                data: JSON.stringify({ "reinsuranceLayerIssuanceDTO": oReinsuranceLayerIssuance, "endorsementId": endorsementId }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    onAddCompleteTempIssueLayer(data);
                    disabledTabs(false);
                }
            });
        }
    }
});

$("#modalLineReinsurance").find("#btnLayerLine").click(function () {

    $("#formLayerLine").validate();
    if ($("#formLayerLine").valid()) {

        oLine.LineId = $("#LineId").val();

        oReinsuranceLine.ReinsuranceLineId = $("#ReinsuranceLineId").val();
        oReinsuranceLine.Line = oLine;

        $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "Reinsurance/Process/UpdateTempLayerLine",
            data: JSON.stringify({ "reinsuranceLine": oReinsuranceLine }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                onAddCompleteTempLayerLine(data);

                setTimeout(function () {
                    disabledTabs(false);
                }, 600);
            }
        });
    }
});


//#region Facultativos

function calculationFacultative() {
    $("#formCalculateFacultative").validate();
    if ($("#formCalculateFacultative").valid()) {
        calculationValue(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, parseFloat($("#txtValueAmountPercentage").val()), parseFloat($("#txtPremiumPercentage").val())).done(function (data) {
            if (data.success) {
                $("#txtValueAmount").val(FormatMoney(data.result[0].TotalSum));
                $("#txtValuePremium").val(FormatMoney((data.result[0].TotalPremium)));
                valueAmount = parseFloat(data.result[0].TotalSum);
                valuePremium = parseFloat(data.result[0].TotalPremium);
            }
        });
    }
}

$("#txtValueAmountPercentage").focusout(function () {
    calculationFacultative();
});

$("#txtPremiumPercentage").focusout(function () {
    calculationFacultative();
});

$("#ModalPaymentFacultative").find('#PaymentPlanFacultativelistview').on('rowEdit', function (event, data, index) {
    $("#modalCheckAdd").find("#selectCheckCurrency").val(data.CurrencyId);
    $("#modalCheckAdd").find("#CheckAmount").val(data.Amount);
    $("#modalCheckAdd").find("#CheckExchangeRate").val(data.ExchangeRate);
    $("#modalCheckAdd").find("#CheckLocalAmount").val(data.LocalAmount);
    $("#modalCheckAdd").find("#inputCheckIssuingBank").val(data.IssuingBankName);
    $("#modalCheckAdd").find("#CheckAccountNumber").val(data.IssuingBankAccountNumber);
    $("#modalCheckAdd").find("#HiddenCheckAccountNumber").val(data.IssuingBankAccountNumber);
    $("#modalCheckAdd").find("#CheckDocumentNumber").val(data.DocumentNumber);
    $("#modalCheckAdd").find("#HiddenCheckDocumentNumber").val(data.DocumentNumber);
    $("#modalCheckAdd").find("#CheckHolderName").val(data.IssuerName);
    $("#modalCheckAdd").find("#CheckDate").val(data.Date);
    issuingBankId = data.IssuingBankId;
    $("#modalCheckAdd").find("#HiddenIssuingBankId").val(data.IssuingBankId);
    indexFacultative = index;
});

$("#ModalPaymentFacultative").find('#PaymentPlanFacultativelistview').on('rowDelete', function (event, data) {

    var dataListviewFacultative = $("#PaymentPlanFacultativelistview").UifListView("getData");
    if (dataListviewFacultative != null) {
        var i = dataListviewFacultative.length - 1;
        var facultativePaymentsIdView = dataListviewFacultative[i].FacultativePaymentsId;
        if (facultativePaymentsIdView == data.FacultativePaymentsId) {
            $.ajax({
                type: "POST",
                url: rootPath + "Reinsurance/Process/DeletePlanFacultative",
                data: {
                    "facultativePaymentsId": data.FacultativePaymentsId, "facultativeCompanyId": 0

                }
            }).done(function (data) {
                if (data.success) {
                    quotaNumber = quotaNumber - 1;
                    $("#PaymentFacultativeNumber").val(quotaNumber);
                    getTempFacultativePayment(tempFacultativeCompanyId);
                    $("#addAccountingMovementForm").formReset();
                }
            });
        }
        else {
            $("#alertPayentPlan").UifAlert('show', Resources.RemoveInstallmentMessage, "warning");
        }
    }
});

$("#ModalPaymentFacultative").find("#SavePlanFacultative").on('click', function () {
    $("#ReinsPaymentFacultativeForm").validate();
    if ($("#ReinsPaymentFacultativeForm").valid()) {
        if (validateNetPremiumPay()) {
            $.ajax({
                type: "POST",
                url: rootPath + "Reinsurance/Process/SavePaymentPlanFacultative",
                data: JSON.stringify({
                    tmpFacultativeCompanyCode: tempFacultativeCompanyId,
                    feeNumber: $("#PaymentFacultativeNumber").val(),
                    paymentDate: $("#DueDateReinsFacultative").val(),
                    paymentAmount: NotFormatMoney($("#AmountReinsFacultative").val())
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"

            }).done(function (data) {
                if (data.success) {
                    quotaNumber++;
                    $("#DueDateReinsFacultative").val("");
                    $("#AmountReinsFacultative").val("");
                    $("#addAccountingMovementForm").formReset();
                    $("#PaymentFacultativeNumber").val(quotaNumber);
                    getTempFacultativePayment(tempFacultativeCompanyId);
                }
            });
        }
        else {
            $("#alertPayentPlan").UifAlert('show', Resources.AmounExceedsTotalMessage, "warning");
        }
    }
});

$("#ModalPaymentFacultative").find("#CancelPlanFacultative").on('click', function () {
    $("#DueDateReinsFacultative").val("");
    $("#AmountReinsFacultative").val("");
    $("#alertPayentPlan").UifAlert('hide');
});

$('#tableFacultatives').on('rowEdit', function (event, selectedRow) {
    $("#ParticipationPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#PremiumPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#CommissionPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#DepositPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#InterestReservePersentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#alertFacultative").UifAlert('hide');
    isEdit = true;
    getReinsuranceFacultative(selectedRow.TempFacultativeCompanyId, selectedRow.TempFacultativeId);
});

$('#tableFacultatives').on('rowSelected', function (event, data, position) {
    $("#alertFacultative").UifAlert('hide');
    isEdit = false;
    dueDateLimit = null;
    tempFacultativeCompanyId = data.TempFacultativeCompanyId;
    sumValuePremium = parseFloat(ClearFormatCurrency(data.SumValuePremium));
    getTempFacultativePayment(data.TempFacultativeCompanyId);
    getValuesPlanFacultative(data.TempFacultativeCompanyId);
});

$('#tableFacultatives').on('rowAdd', function (event, data) {
    $("#ParticipationPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#PremiumPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#CommissionPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#DepositPercentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#InterestReservePersentage").ValidatorKey(ValidatorType.Decimal, 1, 1);
    $("#alertFacultative").UifAlert('hide');
    isEdit = false;
    isCalcReinsurance = false;

    $("#pnlFacultative").trigger('click');
    $('#pnlModalFacultative').text(msjConfirmFacultative);

    $("#CommissionPercentage").val("");
    $("#CommissionValue").val("");
    $("#DepositPercentage").val("");
    $("#Reservation").val("");
    $("#InterestReservePersentage").val("");
    $("#InterestReserve").val("");
    $("#NetPremiumPay").val("");

    var desciption = $("#txtFacultativeDesciption").val();
    var sumPercentage = $("#txtValueAmountPercentage").val() == "" ? 0 : parseFloat(NotFormatMoney($("#txtValueAmountPercentage").val()).replace(',', '.'));
    var premiumPercentage = $("#txtPremiumPercentage").val() == "" ? 0 : parseFloat(NotFormatMoney($("#txtPremiumPercentage").val()).replace(',', '.')); 

    $("#formCalculateFacultative").validate();
    if ($("#formCalculateFacultative").valid()) {
        loadFacultative(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, desciption, sumPercentage, premiumPercentage).then((tempFacultativeId) => {
            if (tempFacultativeId <= 0) {
                $("#alertFacultative").UifAlert('show', Resources.MsgExecuteLoadFacultative, "warning");
            }
        }).catch((result) => {
            $("#alertFacultative").UifAlert('show', result, "warning");
        });
    }
});

$("#modalReinsuranceFacultative").find("#btnReinsFacultative").click(function () {
   
    var tempFacultativeCompanyId = $("#TempFacultativeCompanyId").val();
    $("#formReinsFacultative").validate();
    if ($("#formReinsFacultative").valid()) {
        var depositPercentage = parseFloat(NotFormatMoney($("#DepositPercentage").val()));
        var interestOnReserve = parseFloat(NotFormatMoney($("#InterestReservePersentage").val()));
        var commissionPercentage = parseFloat(NotFormatMoney($("#CommissionPercentage").val()));
        var premiumPercentage = $("#PremiumPercentage").val();
        var participationPercentage = $("#ParticipationPercentage").val();
        var brokerReinsuranceId = $("#BrokerReinsuranceId").val();
        var reinsuranceCompanyId = $("#ReinsuranceCompanyId").val();
        var depositReleaseDate = $("#DateDepositRelease").val();
        var tempFacultativeCompanyId = $("#TempFacultativeCompanyId").val();

        oReinsuranceFacultative = {
            TempFacultativeCompanyId: tempFacultativeCompanyId,
            TempFacultativeId: tempFacultativeId,
            BrokerReinsuranceId: brokerReinsuranceId,
            ReinsuranceCompanyId: reinsuranceCompanyId,
            CommissionPercentage: commissionPercentage,
            PremiumPercentage: premiumPercentage,
            ParticipationPercentage: participationPercentage,
            DepositPercentage: depositPercentage,
            InterestOnReserve: interestOnReserve,
            DepositReleaseDate: depositReleaseDate
        };

        $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "Reinsurance/Process/SaveTempFacultativeCompany",
            data: JSON.stringify(
                {
                    reinsuranceFacultative: oReinsuranceFacultative,
                    endorsementId: endorsementId,
                    layerNumber: layerNumber,
                    lineId: lineId,
                    cumulusKey: cumulusKey
                }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (tempFacultativeCompanyId > 0) {
                    deletePlanFacultative(tempFacultativeCompanyId);
                }
                if (data.success) {
                    clearAgent();
                    clearCompany();
                    onAddCompleteTempFacultativeCompany(data);
                } else {
                    $("#alertFacultativetDialog").UifAlert('show', data.result, "danger");
                }

            }
        });
    }
});

$("#pnlFacultative").click(function () {
    titleFacultative = titleContract;
    if (layerNumber == 0) {
        msjConfirmFacultative = Resources.ConfirmMessageLoadFacultative + titleLayer + ' ?';
        $('#pnlFacultative').text(titleLayer);
    } else if (lineId == 0) {
        msjConfirmFacultative = Resources.ConfirmMessageLoadFacultative + titleLine + ' ?';
        $('#pnlFacultative').text(titleLine);
    } else {
        msjConfirmFacultative = Resources.ConfirmMessageLoadFacultative + titleFacultative + ' / ' + Resources.CumulusType + ': ' + cumulusKey + ' ?';
        $('#pnlFacultative').text(titleFacultative);
    }
});

$('#inputSearchCompany').on('itemSelected', function (event, selectedItem) {
    $("#alertFacultativetDialog").UifAlert('hide');
    if (selectedItem.IndividualId > 0) {
        $.ajax({
            url: REINS_ROOT + "Parameter/IsReinsurerActive",
            data: { "individualId": selectedItem.IndividualId },
            success: function (data) {
                if (data.success) {
                    if (!data.result) {
                        $.ajax({
                            url: REINS_ROOT + "Process/GetCompanyIdByFacultativeIdAndIndividualId",
                            data: { "facultativeId": tempFacultativeId, "individualId": selectedItem.IndividualId },
                            success: function (response) {
                                if (response.result > 0) {
                                    $("#alertFacultativetDialog").UifAlert('show', Resources.MessageValidateDuplicateReinsuranceCompany, "danger");
                                    $('#inputSearchCompany').UifAutoComplete('clean');
                                    clearCompany();
                                }
                                else {
                                    $("#ReinsuranceCompanyId").val(selectedItem.IndividualId);
                                }
                            }
                        });
                    } else {
                        $("#alertFacultativetDialog").UifAlert('show', 'La reaseguradora se encuentra inactiva', "danger");
                        $('#inputSearchCompany').UifAutoComplete('clean');
                        clearCompany();
                    }
                }
            }
        });
    }
    else {
        clearCompany()
        $("#alertFacultativetDialog").UifAlert('show', Resources.MessageReinsuranceCompanyNotFound, "warning");
        $("#inputSearchCompany").focus();
    }
});

$('#inputSearchBroker').on('itemSelected', function (event, selectedItem) {
    $("#alertFacultativetDialog").UifAlert('hide');
    if (selectedItem.IndividualId > 0) {
        $("#BrokerReinsuranceId").val(selectedItem.IndividualId);
    }
    else {
        clearAgent();
        $("#alertFacultativetDialog").UifAlert('show', Resources.MessageBrokerNotFound, "warning");
        $("#inputSearchBroker").focus();
    }
});

$("#CommissionPercentage").blur(function () {
    calculationCommissionPercentage($("#CommissionPercentage").val());
});

$("#DepositPercentage").blur(function () {
    calculationDepositPercentage($("#DepositPercentage").val());
});

$("#InterestReservePersentage").blur(function () {
    calculationInterestReservePercentage($("#InterestReservePersentage").val());
});

$("#AmountReinsFacultative").blur(function () {
    if ($("#AmountReinsFacultative").val() != "") {
        var amountReinsFacultative = $("#AmountReinsFacultative").val();
        $("#AmountReinsFacultative").val(FormatMoney(amountReinsFacultative));
    }
    $("#alertPayentPlan").UifAlert('hide');
});

$("#DueDateReinsFacultative").on("datepicker.change", function (event, date) {
    $("#alertPayentPlan").UifAlert('hide');
    if (dueDateLimit != null) {
        if (compare_dates(dueDateLimit, $("#DueDateReinsFacultative").val())) {

            $("#alertPayentPlan").UifAlert('show', Resources.DateQuota, 'warning');
            $("#DueDateReinsFacultative").val('');
        }
        else {
            $("#DueDateReinsFacultative").val($("#DueDateReinsFacultative").val());
        }
    }
});

$("#SelectSlips").on("itemSelected", function () {
    if ($("#SelectSlips").val() > 0) {
        expandFacultative($("#SelectSlips").val());
        $("#txtFacultativeDesciption").attr("disabled", "disabled");
        $("#txtValueAmount").attr("disabled", "disabled");
        $("#txtValuePremium").attr("disabled", "disabled");
    }
    else {
        $("#txtFacultativeDesciption").removeAttr("disabled");
        $("#txtValueAmount").removeAttr("disabled");
        $("#txtValuePremium").removeAttr("disabled");
    }
});

function paymentPlansReinsFacultative() {
    $("#alertFacultative").UifAlert('hide');
    var selectPlanFacultive = $("#tableFacultatives").UifDataTable('getSelected');

    if (selectPlanFacultive != null) {
        var paymentPlanFacultative = $("#ModalPaymentFacultative").find('#PaymentPlanFacultativelistview').UifListView("getData");
        quotaNumber = paymentPlanFacultative.length + 1;
        $("#PaymentFacultativeNumber").val(quotaNumber);
        $("#DueDateReinsFacultative").val("");
        $("#AmountReinsFacultative").val("");
        $('#ModalPaymentFacultative').UifModal('showLocal', Resources.TitleFacultativeContract + "/" + Resources.ReinsPaymentPlan + " Valor: " + $("#NetPremiumPay").val());
    }
    else {
        $("#alertFacultative").UifAlert('show', Resources.SelectPaymentRecordPlanIncome, "warning");
    }
}

function getTempFacultativePayment(levelCompanyId) {
    $.ajax({
        url: rootPath + "Reinsurance/Process/GetTempFacultativePayment?levelCompanyId=" + levelCompanyId,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var items = data.map(function (item) {
                item.Date = moment(item.Date).format('DD/MM/YYYY');
                return item;
            });
            $("#PaymentPlanFacultativelistview").UifListView({
                height: 400,
                sourceData: items,
                customDelete: true,
                customAdd: false,
                customEdit: true,
                add: false,
                edit: false,
                delete: true,
                displayTemplate: "#facultative-template"
            });

            setTimeout(function () {
                planFacultativeTotal();
            }, 1000);
        }
    });
}

function planFacultativeTotal() {
    isTotalPlanFacultative = true;
    var totalAmount = 0;
    var netPremiumPay = NotFormatMoney($("#NetPremiumPay").val());
    var planFacultative = $("#PaymentPlanFacultativelistview").UifListView("getData");
    if (planFacultative != null) {
        for (var j = 0; j < planFacultative.length; j++) {
            totalAmount += planFacultative[j].Amount.Value;
            dueDateLimit = planFacultative[j].DueDate;
        }

        var amount = FormatMoney(totalAmount);
        var residuePlanFacultative = parseFloat(netPremiumPay) - parseFloat(totalAmount);
        residuePlanFacultative = FormatMoney(residuePlanFacultative);

        if (totalAmount > 0 && totalAmount < netPremiumPay) {
            isTotalPlanFacultative = false;
        }
        $("#totalPaymentPlanFacultative").text(amount);
        $("#residuePaymentPlanFacultative").text(residuePlanFacultative);
    }
    else {
        $("#totalPaymentPlanFacultative").text("");
    }
}

function getValuesPlanFacultative(paramTempFacultativeCompanyId) {
    var paramFacultative = paramTempFacultativeCompanyId;
    $.ajax({
        type: "POST",
        url: REINS_ROOT + 'Process/GetReinsuranceFacultative',
        data: JSON.stringify({
            tempFacultativeCompanyId: paramTempFacultativeCompanyId,
            endorsementId: endorsementId,
            layerNumber: layerNumber,
            lineId: lineId,
            cumulusKey: cumulusKey
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            var commission = parseFloat(data.result.CommissionPercentage);
            var deposit = parseFloat(data.result.DepositPercentage);
            var reserve = parseFloat(data.result.InterestOnReserve);

            if (data.result.ParticipationPercentage != "") {
                calculationParticipationPercentage(data.result.ParticipationPercentage);
            }
            if (data.result.PremiumPercentage != "") {
                calculationPremiumPercentage(data.result.PremiumPercentage);
            }

            if (paramFacultative > 0 && paramFacultative != null) {
                if (commission > 0) {
                    calculationCommissionPercentage(data.result.CommissionPercentage);
                }
                if (deposit > 0) {
                    calculationDepositPercentage(data.result.DepositPercentage);
                }
            }
        } else {
            $("#alertFacultative").UifAlert('show', data.result, "danger");
        }
    });
}

function onAddCompleteTempFacultativeCompany(data) {
    if (data.success) {
        $('#modalReinsuranceFacultative').UifModal('hide');
        getTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey);
        if (data.result[0].Participation != 100 || data.result[0].Premium != 100) {
            $("#alertFacultative").UifAlert('show', Resources.ValidFacultativePercentage, "warning");
            $("#btnSaveReinsurance").hide();
            switchSave = false;
        }
        else {
            $("#btnSaveReinsurance").show();
            $("#alertFacultative").UifAlert('hide');
            switchSave = true;
        }
        $("#LinkPaymentPlansReinsFacultative").attr("href", "#");
    }
}

function deletePlanFacultative(tempFacultativeCompanyId) {
    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/DeletePlanFacultative",
        data: {
            "facultativePaymentsId": 0, "facultativeCompanyId": tempFacultativeCompanyId
        }
    }).done(function (data) {
        if (data.success) {
            quotaNumber = 1;
            $("#PaymentFacultativeNumber").val(quotaNumber);
        }
    });
}

function loadFacultative(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, descriptionFacultative, sumPercentage, premiumPercentage) {
    return new Promise((resolve, reject) => {
        loadFacultativeByTempReinsuranceProcessId(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, descriptionFacultative, sumPercentage, premiumPercentage).done(function (data) {
            if (data.success) {
                if (isCalcReinsurance == false) {
                    if (!data.success) {
                        $("#alertFacultative").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
                        resolve(tempFacultativeId);
                    }
                    else {
                        $("#alertModification").UifAlert('show', Resources.MsgCompleteReinsuranceFacultative, "warning");
                        $("#alertReinsurePolicieLine").UifAlert('show', Resources.MsgCompleteReinsuranceFacultative, "warning");
                        $("#alertContract").UifAlert('show', Resources.MsgCompleteReinsuranceFacultative, "warning");
                        $("#alertFacultative").UifAlert('show', Resources.MsgCompleteReinsuranceFacultative, "warning");
                        tempFacultativeId = data.result;
                        getTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey);
                        countFacultative = countFacultative + 1;
                        if (countFacultative == 1) {
                            loadFacultativeDialog(null);
                        }
                        else {
                            loadFacultativeDialog(0);
                        }
                        resolve(tempFacultativeId);
                    }
                }
                else {
                    isCalcReinsurance = false;
                    resolve(tempFacultativeId);
                }
            }
        }).fail(function (response) {
            tempFacultativeId = 0;
            $("#alertFacultative").UifAlert('show', Resources.GeneratesError + ' ' + response.result, "danger");
            reject(response.result);
        }).always(function () {
            unlockScreen();
        });
    });
}

function getTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey) {
    $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/GetTempFacultativeCompanies",
        data: JSON.stringify(
            {
                endorsementId: endorsementId,
                layerNumber: layerNumber,
                lineId: lineId,
                cumulusKey: cumulusKey
            }
        ),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#tableFacultatives").UifDataTable({ sourceData: data.result })
                    $("#txtFacultativeDesciption").val(data.result[0].Comments);
                    $("#txtValueAmountPercentage").val(data.result[0].FacultativePercentage);
                    $("#txtPremiumPercentage").val(data.result[0].FacultativePremiumPercentage);
                    var sumPercentage = $("#txtValueAmountPercentage").val() == "" ? 0 : parseFloat(NotFormatMoney($("#txtValueAmountPercentage").val()).replace(',', '.'));
                    var premiumPercentage = $("#txtPremiumPercentage").val() == "" ? 0 : parseFloat(NotFormatMoney($("#txtPremiumPercentage").val()).replace(',', '.')); 

                    calculationValue(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, sumPercentage, premiumPercentage).done(function (data) {
                        if (data.success) {
                            $("#txtValueAmount").val(FormatMoney(data.result[0].TotalSum));
                            $("#txtValuePremium").val(FormatMoney((data.result[0].TotalPremium)));
                            valueAmount = parseFloat(data.result[0].TotalSum);
                            valuePremium = parseFloat(data.result[0].TotalPremium);
                        }
                    });
                } else {
                    $("#tableFacultatives").UifDataTable('clear')
                    $("#txtFacultativeDesciption").val('');
                    $("#txtValueAmountPercentage").val('');
                    $("#txtPremiumPercentage").val('');
                    $("#txtValueAmount").val('');
                    $("#txtValuePremium").val('');
                    countFacultative = 0;
                }
            } else {
                $("#alertFacultative").UifAlert('show', data.result, "danger");
            }
        }
    });
}

function loadFacultativeDialog(tempFacultativeCompanyId) {
    getReinsuranceFacultative(tempFacultativeCompanyId)
}

function getReinsuranceFacultative(tempFacultativeCompanyId) {
    $.ajax({
        type: "POST",
        url: REINS_ROOT + 'Process/GetReinsuranceFacultative',
        data: JSON.stringify({
            tempFacultativeCompanyId: tempFacultativeCompanyId,
            endorsementId: endorsementId,
            layerNumber: layerNumber,
            lineId: lineId,
            cumulusKey: cumulusKey
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            var commission = data.result.CommissionPercentage == null ? 0 : parseFloat(data.result.CommissionPercentage);
            var deposit = parseFloat(data.result.DepositPercentage);
            var reserve = parseFloat(data.result.InterestOnReserve);
            $("#BrokerReinsuranceId").val(data.result.BrokerReinsuranceId);
            $("#ReinsuranceCompanyId").val(data.result.ReinsuranceCompanyId);
            $("#TempFacultativeCompanyId").val(data.result.TempFacultativeCompanyId);
            $("#TempFacultativeId").val(data.result.TempFacultativeId);
            $("#inputSearchBroker").val(data.result.BrokerDescription);
            $("#inputSearchCompany").val(data.result.DescriptionCompany);
            $("#ParticipationPercentage").val(data.result.ParticipationPercentage);

            if (data.result.ParticipationPercentage != "" && data.result.ParticipationPercentage != null) {
                calculationParticipationPercentage(data.result.ParticipationPercentage);
            }

            $("#PremiumPercentage").val(data.result.PremiumPercentage);
            if (data.result.PremiumPercentage != "" && data.result.PremiumPercentage != null) {
                calculationPremiumPercentage(data.result.PremiumPercentage);
            }

            if (tempFacultativeCompanyId > 0 && tempFacultativeCompanyId != null) {

                $("#CommissionPercentage").val(data.result.CommissionPercentage);
                if (commission > 0) {
                    calculationCommissionPercentage(data.result.CommissionPercentage);
                }

                $("#DepositPercentage").val(data.result.DepositPercentage);
                if (deposit > 0) {
                    calculationDepositPercentage(data.result.DepositPercentage);
                }

                $("#InterestReservePersentage").val(data.result.InterestOnReserve);
                if (reserve > 0) {
                    calculationInterestReservePercentage(data.result.InterestOnReserve);
                }

                if (data.result.DepositReleaseDate == "01/01/1900") {
                    $("#DateDepositRelease").val("");
                }
                else {
                    var dateDepositRelease = data.result.DepositReleaseDate;
                    $("#DateDepositRelease").UifDatepicker('setValue', dateDepositRelease);
                }
            }

            $('#modalReinsuranceFacultative').UifModal('showLocal', Resources.TitleFacultativeContract);
            if (isEdit) {
                $("#alertFacultativetDialog").UifAlert('show', Resources.PaymentPlansDelete, "warning");
            }

        } else {
            $("#alertFacultative").UifAlert('show', data.result, "danger");
        }
    });
}

function loadFacultativeByTempReinsuranceProcessId(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, descriptionFacultative, sumPercentage, premiumPercentage) {
    return $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/LoadFacultative",
        data: JSON.stringify({
            tempReinsuranceProcessId: tempReinsuranceProcessId,
            layerNumber: layerNumber,
            lineId: lineId,
            cumulusKey: cumulusKey,
            description: descriptionFacultative,
            sumPercentage: sumPercentage,
            premiumPercentage: premiumPercentage
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function validateTabFacultative(option) {
    if (option == "S") {
        $('#litabFacultative').removeAttr('class');
        $('#litabFacultative a').attr('role', "tab");
        $('#litabFacultative a').attr('data-toggle', "tab");
    }
    else {
        $('#litabFacultative').attr('class', "disabled"); //add
        $('#litabFacultative a').removeAttr('role');
        $('#litabFacultative a').removeAttr('data-toggle');

        // tabs-  coloca estilo normal quitando active 
        $('#tabFacultative').attr('class', "tab-pane");
        $('#tabLines').attr('class', "tab-pane");
        $('#tabAllocation').attr('class', "tab-pane");

        // tipos li -> de ul
        $('#litabLines').removeAttr('class');
        $('#litabDistribuitionContract').removeAttr('class');

        $('#litabLayer').attr('class', "active");
        $('#tabModificationLayer').attr('class', "tab-pane active");
    }
}

function calculationValue(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey, sumPercentage, premiumPercentage) {
    return $.ajax({
        type: "POST",
        url: rootPath + "Reinsurance/Process/CalculationValue",
        data: JSON.stringify({
            tempReinsuranceProcessId: tempReinsuranceProcessId,
            layerNumber: layerNumber,
            lineId: lineId,
            cumulusKey: cumulusKey,
            sumPercentage: sumPercentage,
            premiumPercentage: premiumPercentage
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function calculationParticipationPercentage(value) {
    $("#alertFacultativetDialog").UifAlert('hide');
    var participationPercentage = parseFloat(value);
    $("#lblParticipationAmount").val(FormatMoney(valueAmount * (participationPercentage / 100)));
}

function calculationPremiumPercentage(value) {
    $("#alertFacultativetDialog").UifAlert('hide');
    var premiumPercentage = parseFloat(value);
    var commission = valuePremium * (premiumPercentage / 100);
    $("#lblPremiumAmount").val(FormatMoney(commission));
    calculationNetPremiumPay();
}

function calculationCommissionPercentage(value) {
    $("#alertFacultativetDialog").UifAlert('hide');
    var premiumAmount = parseFloat(NotFormatMoney($("#lblPremiumAmount").val()));
    var commissionPercentage = parseFloat(value);
    var commission = premiumAmount * (commissionPercentage / 100);
    $("#CommissionValue").val(FormatMoney(commission));
    calculationNetPremiumPay();
}

function calculationDepositPercentage(value) {
    $("#alertFacultativetDialog").UifAlert('hide');
    var premiumAmount = parseFloat(NotFormatMoney($("#lblPremiumAmount").val()));
    var depositPersentage = parseFloat(value);
    var reserve = premiumAmount * (depositPersentage / 100);
    $("#Reservation").val(FormatMoney(reserve));
    calculationNetPremiumPay();
}

function calculationInterestReservePercentage(value) {
    $("#alertFacultativetDialog").UifAlert('hide');
    var reservation = parseFloat(NotFormatMoney($("#Reservation").val()));
    var interestReservePersentage = parseFloat(value);
    var interestReserve = Math.round(reservation * (interestReservePersentage / 100));
    $("#InterestReserve").val(FormatMoney(interestReserve));
}

function calculationNetPremiumPay() {
    $("#alertFacultativetDialog").UifAlert('hide');
    var premiumAmount = parseFloat(NotFormatMoney($("#lblPremiumAmount").val()));
    var commissionValue = parseFloat(NotFormatMoney($("#CommissionValue").val()));
    var reservation = parseFloat(NotFormatMoney($("#Reservation").val()));
    netPremiumPay = premiumAmount - commissionValue - reservation;
    $("#NetPremiumPay").val(FormatMoney(netPremiumPay));
}

function validateNetPremiumPay() {
    var totalPaymentPlanFacultative = NotFormatMoney($("#totalPaymentPlanFacultative").text());
    var netPremiumPay = NotFormatMoney($("#NetPremiumPay").val());
    var amountReinsFacultative = NotFormatMoney($("#AmountReinsFacultative").val());
    if ((parseFloat(totalPaymentPlanFacultative) + parseFloat(amountReinsFacultative)) <= parseFloat(netPremiumPay)) {
        return true;
    }
    else {
        return false;
    }
}

function clearAgent() {
    $('#inputSearchBroker').val("");
    $("#BrokerReinsuranceId").val("");
}

function clearCompany() {
    $("#inputSearchCompany").val("");
    $('#ReinsuranceCompanyId').val("");
}

function getNetPremiumPay(TempFacultativeCompanyId) {
    return new Promise((resolve, reject) => {
        var paramFacultative = TempFacultativeCompanyId;
        return $.ajax({
            type: "POST",
            url: REINS_ROOT + 'Process/GetReinsuranceFacultative',
            data: JSON.stringify({
                tempFacultativeCompanyId: TempFacultativeCompanyId,
                endorsementId: endorsementId,
                layerNumber: layerNumber,
                lineId: lineId,
                cumulusKey: cumulusKey
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                var commission = parseFloat(data.result.CommissionPercentage);
                var deposit = parseFloat(data.result.DepositPercentage);
                var reserve = parseFloat(data.result.InterestOnReserve);

                if (data.result.ParticipationPercentage != "") {
                    calculationParticipationPercentage(data.result.ParticipationPercentage);
                }
                if (data.result.PremiumPercentage != "") {
                    calculationPremiumPercentage(data.result.PremiumPercentage);
                }

                if (paramFacultative > 0 && paramFacultative != null) {
                    if (commission > 0) {
                        calculationCommissionPercentage(data.result.CommissionPercentage);
                    }
                    if (deposit > 0) {
                        calculationDepositPercentage(data.result.DepositPercentage);
                    }
                }
                resolve(parseFloat(NotFormatMoney($("#NetPremiumPay").val())));

            }
        });

    });
}

function validatePaymentPlanComplete(paymentsPlan) {
    return new Promise(function (resolve, reject) {
        var result = true;
        var count = paymentsPlan.length;
        paymentsPlan.forEach(function (item) {
            var amountPaymenPlan = 0;
            getNetPremiumPay(item[0].TempCompanyId).then((netPremiun) => {
                item.forEach(function (items) {
                    amountPaymenPlan = amountPaymenPlan + items.Amount.Value;
                });
                if ((netPremiun - amountPaymenPlan) > 0) {
                    result = false;
                    $("#alertFacultative").UifAlert('show', Resources.PaymentPlanIsNotComplete, "danger")
                }
                count = count - 1;
                if (count == 0) {
                    resolve(result);
                }
            });
        });

    });
}

function validatePaymentPlan() {
    return new Promise(function (resolve, reject) {
        var result = true;
        var requests = [];
        var paymentsPlan = [];
        var selectPlanFacultive = $("#tableFacultatives").UifDataTable('getData')
        if (selectPlanFacultive != undefined && selectPlanFacultive.length > 0) {
            selectPlanFacultive.forEach(function (item) {
                requests.push(getPaymenPlan(item.TempFacultativeCompanyId));
            });
            Promise.all(requests).then(response => {
                response.forEach(function (item) {
                    if (item.length == 0) {
                        result = false;
                    }
                });
                if (result == false) {
                    $("#alertFacultative").UifAlert('show', Resources.FacultativeThereIsNotPaymentPlan, "danger")
                    resolve(result);
                }
                else {
                    validatePaymentPlanComplete(response).then(function (result) {
                        resolve(result);
                    });
                }

            });
        }
        else {
            resolve(true);
        }
    });
}

function getPaymenPlan(levelCompanyId) {
    return $.ajax({
        url: rootPath + "Reinsurance/Process/GetTempFacultativePayment?levelCompanyId=" + levelCompanyId,
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
}

function expandFacultative(facultativeId) {
    $.ajax({
        url: REINS_ROOT + 'Process/ExpandFacultative',
        data: {
            'processId': tempReinsuranceProcessId,
            'endorsementId': endorsementId,
            'layerNumber': layerNumber,
            'facultativeId': facultativeId
        },
    }).done(function (data) {
        if (data.success) {
            if (data.result > 0) {
                tempFacultativeId = data.result;
                getTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey);
            }
        }
    });
}

function getSlips() {
    $("#divSlips").hide();
    $.ajax({
        url: REINS_ROOT + 'Process/GetSlips',
        data: {
            'processId': tempReinsuranceProcessId,
            'endorsementId': endorsementId
        },
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                $("#divSlips").show();
                $("#SelectSlips").UifSelect({ sourceData: data.result });
            }
        }
    });
}

//#endregion Facultativos
