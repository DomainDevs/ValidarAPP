var productId = null;
var massiveLoadPolicies = [];
var searchModel;
var glbEndorsements = [];
var glbEndorsementsCollectFormat = [];
var currentPolicyId;
var currentState;
var temporary;
var gblQuotation;
var pathFile = "";
var glbFilePath = "";
var glbPrintingLogs = [];
var glbPrintingId;
var glbReadyForQuotes = false;
var glbUniquePolicy = false;
var endorsementText = false;
var temporalAutho = false;
var currentFromFirst = false;

$.ajaxSetup({ async: false });
$(document).ready(function () {
    $('#inputTemporal').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputProcessCode').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputPolicyNumber').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputAdvice').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputLoad').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputLicensePlate').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    $('#inputLicensePlates').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    $('#inputRangeFrom').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputRangeTo').ValidatorKey(ValidatorType.Number, 2, 0);
    $("#inputCurrentDate").text(GetCurrentFromDate());
    $("#checkPrintSelectEndoso").attr("disabled", true);
    $("#checkPrintAllEndoso").attr("disabled", true);
    $("#checkCurrentFromFirst").attr('disabled', true);

    DisableControls();
    PrinterUniquePolicy();

    $("#checkEmail").prop("checked", false);
    $("#checkCollectFormat").prop("checked", false);

    if ($('#selectBranch').UifSelect("getSelected") > 0 && $('#selectPrefixCommercial').UifSelect("getSelected") > 0) {
        LoadEndorsements();
        LoadSummaryEndorsement($('#selectEndorsement').UifSelect("getSelected"));
    }
    if ($('#inputLoad').val() > 0) {
        GetDetailByMassiveLoadId();
    }
    if ($('#inputTemporal').val() != undefined) {
        LoadSummaryTemporal();
    }
});

//Eventos
$("#inputNumQuota").ValidatorKey(ValidatorType.Number, 5, 0);

$("#checkEmail").change(function () {
    if ($("#checkEmail").is(":checked")) {
        $("#inputEmail").prop("disabled", false);
        if (pathFile !== "") {
            $("#divSendMail").show();
        }
    }
    else {
        $("#inputEmail").prop("disabled", true);
        $("#divSendMail").hide();
    }
});

$("#checkPrintSelectEndoso").change(function () {
    if ($("#checkPrintSelectEndoso").is(":checked")) {
        $("#checkPrintAllEndoso").prop('checked', false)
        endorsementText = false;
    }
    else {
        endorsementText = true;
    }
});

$("#checkPrintAllEndoso").change(function () {
    if ($("#checkPrintAllEndoso").is(":checked")) {
        $("#checkPrintSelectEndoso").prop('checked', false)
        endorsementText = true;
    }
    else {
        endorsementText = false;
    }
});

$("#checkCurrentFromFirst").change(function () {
    if ($("#checkCurrentFromFirst").is(":checked")) {
        currentFromFirst = true;
    }
    else {
        currentFromFirst = false;
    }
});


$("#btnSendEmail").on('click', function () {
    if (pathFile != "" || glbFilePath != "") {
        SendMail(pathFile);
    }
    else {
        $.UifNotify('show', { 'type': 'danger', 'message': 'Falta url de documento generado', 'autoclose': true });
    }
});

$("#btnGenerateZip").on('click', function () {
    if ($('#inputProcessCode').val().trim().length > 0) {
        GenerateZip($('#inputProcessCode').val());
    }
});

$('#selectBranch').on('itemSelected', function (event, selectedItem) {
    $('#selectEndorsement').UifSelect();
});

$('#selectPrefixCommercial').on('itemSelected', function (event, selectedItem) {
    $('#selectEndorsement').UifSelect();
});

$('#inputPolicyNumber').on('buttonClick', function () {
    if ($('#inputPolicyNumber').val().trim().length > 0) {
        if (glbUniquePolicy == false) {
            if ($('#selectPrefixCommercial').UifSelect("getSelected") == null || $('#selectBranch').UifSelect("getSelected") == null || $('#inputPolicyNumber').val().trim().length < 0) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.BranchPrefixAndDocumentNumRequired, 'autoclose': true });
            } else {
                LoadEndorsements();
            }
        } else {
            LoadEndorsements();
        }
        
    }
});

$('#inputProcessCode').on('buttonClick', function () {
    if ($('#inputProcessCode').val().trim().length > 0) {
        ClearPanels();
        ClearAreaQuota();
        //ClearAreaPolicy();
        LoadPrintingProcess();
    }
});

$('#inputTemporal').on('buttonClick', function () {
    if ($('#inputTemporal').val().trim().length > 0) {
        ClearAreaPolicy();
        ClearAreaQuota();
        $("#inputProcessCode").val("");
        $("#inputAdvice").val("");
        LoadSummaryTemporal();
        $("#divMassiveDetail").hide();
    }
});

$('#inputAdvice').on('buttonClick', function () {
    if ($('#inputAdvice').val().trim().length > 0) {
        ClearAreaPolicy();
        ClearAreaQuota();
        $("#inputProcessCode").val("");
        $("#inputTemporal").val("");
        LoadSummaryTemporalAutho();
        $("#divMassiveDetail").hide();
    }
});

$('#inputNumQuota').on('buttonClick', function () {
    if ($('#inputNumQuota').val().trim().length > 0) {
        LoadQuotationVersion();
    }
});

$('#inputLoad').on('buttonClick', function () {
    if ($('#inputLoad').val().trim().length > 0) {
        GetDetailByMassiveLoadId();
    }
});

$('#selectVersionQuota').on('itemSelected', function () {
    $('#btnPrint').prop('disabled', false);
});

$('#selectEndorsement').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        LoadSummaryEndorsement(selectedItem.Id);
        HidePrintSelect();
        currentState = "PolicyReady";
        $('#checkCollectFormat').prop('disabled', false);
        var endorsement = glbEndorsements.find(endorsement => endorsement.Id == selectedItem.Id);
        if (endorsement.Number == 0) {
            $("#checkPrintSelectEndoso").attr("disabled", true);
            $("#checkPrintAllEndoso").attr("disabled", true);
            $("#checkPrintSelectEndoso").prop('checked', false);
            $("#checkPrintAllEndoso").prop('checked', false);
            $("#checkCurrentFromFirst").attr('disabled', true);
            $("#checkCurrentFromFirst").prop('checked', false);
        }
        else {
            $("#checkPrintSelectEndoso").attr("disabled", false);
            $("#checkPrintAllEndoso").attr("disabled", false);
            $("#checkCurrentFromFirst").attr('disabled', false);
        }
    }
});

$('input[name=rbPrintSelect]:radio').change(function () {
    var printSelect = $('input[name=rbPrintSelect]:checked').val();
    DisableControls();
    $('#btnPrint').prop('disabled', false);
    HidePrintSelect();
    switch (printSelect) {
        case '2':
            $('#inputLicensePlate').prop('disabled', false);
            $('#printerPlate').show();
            break;
        case '3':
            $('#inputRangeFrom').prop('disabled', false);
            $('#inputRangeTo').prop('disabled', false);
            $("#printerRank").show();
            break;
        default:
            break;
    }
});

$('#inputLicensePlate').on('buttonClick', function () {
    if ($('#inputLicensePlate').val().trim().length > 0) {
        AddLicensePlates();
    }
});

$('#inputRangeFrom').focusout(function () {
    if ($('#inputRangeFrom').val().trim().length > 0) {
        ValidateRange();
    }
});

$('#inputRangeTo').focusout(function () {
    if ($('#inputRangeTo').val().trim().length > 0) {
        ValidateRange();
    }
});

$('#btnPrint').on('click', function () {

    if (!glbReadyForQuotes) {
        var printMassive = false;
        var detailMassive = $('#tableDetail').UifDataTable('getData');
        if (detailMassive.length > 0) {
            printMassive = true;
        }

        PrintReport(null, null, false, printMassive, $("#checkCollectFormat").is(":checked"));
    } else {
        var quotesSelected = [];
        $.each($("#tableQuote").UifDataTable("getSelected"), function (Id, item) {
            quotesSelected.push({
                QuoteNumber: item.QuoteNumber
            });
        });

        GenerateColletFormatReport($("#selectBranch").UifSelect("getSelected"), $("#selectPrefixCommercial").UifSelect("getSelected"), $('#inputPolicyNumber').val(), glbEndorsementsCollectFormat, quotesSelected);
    }

});

$("#btnClose").on("click", function () {
    window.location = rootPath + "Home/Index";
});



//Metodos

function PrinterUniquePolicy() {
    $.ajax({
        type: "GET",
        url: rootPath + 'Printer/GetParameterUniquePolicy',
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8"
        }).done(function (data) {
        if (data.success) {
            glbUniquePolicy = data.result.BoolParameter
        }
    });
}

function SendMail(path) {
    if ($("#checkEmail").is(":checked")) {
        if ($('#inputEmail').val() != '') {
            $.ajax({
                type: 'POST',
                async: true,
                url: rootPath + 'Printing/Printer/SendToEmail',
                data: JSON.stringify({ email: $('#inputEmail').val(), filePath: path, pathCollectFormat: glbFilePath }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8'
            }).done(function (data) {
                if (data.success && data.result) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SentEmail, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSentEmail, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSentEmail + " : " + "Falta email destino", 'autoclose': true });
            $('#inputEmail').css({
                'backgroundColor': '#f77259'
            });
        }
    }
}

function GenerateZip(printingId) {
    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GenerateZip',
        data: JSON.stringify({ printingId: printingId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success && data.result) {
            DownloadFile(data.result.Url, true, (url) => url.match(/([^\\]+.zip)/)[0]);
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
        }
    });
}

function LoadEndorsements() {
    $('#selectEndorsement').UifSelect();
    $('#formPrinting').validate();

    if ($('#formPrinting').valid()) {
        $("#inputTemporal").val('');
        ClearAreaQuota();
        $('#inputLoad').val('');
        $("#checkCurrentFromFirst").attr('disabled', true);
        $("#checkCurrentFromFirst").attr('checked', false);
        DisableControls();
        ClearPanels();
        $("#inputProcessCode").val("");

        var branch = $("#selectBranch").UifSelect("getSelected")
        var prefix = $("#selectPrefixCommercial").UifSelect("getSelected")

        if (branch == null && prefix == null) {
            branch = 0
            prefix = 0
        }
        GetEndorsementsByBranchIdPrefixIdPolicyNumber(branch, prefix, $('#inputPolicyNumber').val(), true);
    }
}

function LoadPrintingProcess() {
    GetPrintingByPrintingId($("#inputProcessCode").val());
}

function LoadSummaryEndorsement(endorsementId) {
    GetPrintSummaryByEndorsementId(endorsementId, true);
}

function LoadPanelEndorsement(summary) {
    ClearPanels();
    if (summary != null) {
        summary.AmountInsured = FormatMoney(summary.AmountInsured);
        summary.Premium = FormatMoney(summary.Premium);
        summary.Expenses = FormatMoney(summary.Expenses);
        summary.Taxes = FormatMoney(summary.Taxes);
        summary.FullPremium = FormatMoney(summary.FullPremium);
        FillSummary(summary);
        ShowPanelRisk(true);
        LoadTitle(summary);
    }
}

function LoadQuotationVersion() {
    $('#selectVersionQuota').UifSelect();
    ClearPanels();
    $("#inputPolicyNumber").val("");
    $("#selectEndorsement").UifSelect();
    $("#inputTemporal").val('');
    $("#inputAdvice").val('');
    $("#inputProcessCode").val("");

    if ($('#formPrinting').valid()) {
        var versions = GetVersionsByQuotationId($('#inputNumQuota').val(), $("#selectBranch").UifSelect("getSelected"), $("#selectPrefixCommercial").UifSelect("getSelected"));

        if (versions != null) {
            $('#selectVersionQuota').UifSelect({ sourceData: versions, id: 'TempId', name: 'VersionId' });
        }
    }

    currentState = "QuotationReady";
}

function LoadSummaryTemporal() {
    ClearPanels();
    temporalAutho = false;
    temporary = GetSummaryByTemporalId($('#inputTemporal').val(), temporalAutho);
    currentState = "TemporaryReady";
}

function LoadSummaryTemporalAutho() {
    ClearPanels();
    temporalAutho = true;
    temporary = GetSummaryByTemporalId($('#inputAdvice').val(), temporalAutho);
    currentState = "TemporaryReady";
}

function LoadTitle(summary) {
    var titlePrincipal = 'Póliza: ' + $("#selectBranch").UifSelect("getSelectedText");
    titlePrincipal = titlePrincipal + ' ' + $("#selectPrefix").UifSelect("getSelectedText");
    titlePrincipal = titlePrincipal + ' ' + $('#inputPolicyNumber').val();
    titlePrincipal = titlePrincipal + ' ' + summary.ProductDescription;
    $.uif2.helpers.setGlobalTitle(titlePrincipal);
}

function GetSummaryByTemporalId(temporalId, tempAutho) {
    var summary = null;
    var temporalId = temporalId;
    if (temporalId > 0) {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Printing/Printer/GetSummaryByTemporalId',
            data: JSON.stringify({ temporalId: temporalId, temporalAutho: tempAutho }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                summary = data.result;
                $('#btnPrint').prop('disabled', false);
                $("#checkCollectFormat").prop("checked", false);
                $('#checkCollectFormat').prop('disabled', true);
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.TemporaryFoundForPrinting, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }
    return summary;
}

function GetDetailByMassiveLoadId() {
    $("#selectBranch").UifSelect("setSelected", null);
    $("#selectPrefixCommercial").UifSelect("setSelected", null);
    $('#inputPolicyNumber').val('');
    $('#inputTemporal').val('');
    $('#selectEndorsement').UifSelect();
    $('#divSummary').hide();
    $('#divDetail').hide();
    $('#divDownload').hide();
    ClearSummary();
    $('#tableDetail').UifDataTable('clear');

    $('#btnPrint').prop('disabled', false);

    $.ajax({
        type: 'POST',
        url: rootPath + 'Massive/Massive/GetMassiveLoadsByDescription',
        data: JSON.stringify({ description: $('#inputLoad').val() }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                ShowPanelRisk(false);
                $('#rbRiskAll').prop('checked', true);
                var massiveLoad = data.result[0];
                $.ajax({
                    type: 'POST',
                    url: rootPath + 'Massive/Massive/GetMassiveLoadDetailsByMassiveLoad',
                    data: JSON.stringify({ massiveLoadId: massiveLoad.Id, processType: massiveLoad.LoadType.Id, status: massiveLoad.Status }),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                }).done(function (data) {
                    if (data.success) {
                        $('#tableDetail').UifDataTable('addRow', data.result.Rows);
                        if (data.result) {
                            $("#selectBranch").UifSelect("setSelected", data.result.Branch.Id);
                            $("#selectPrefixCommercial").UifSelect("setSelected", data.result.Prefix.Id);
                            productId = data.result.Product.Id;
                        }

                        var endorsements = [];
                        $.each(data.result.Rows, function (key, value) {
                            if (value.Risk.Policy.DocumentNumber != 0) {
                                var endorsement = GetEndorsementsByBranchIdPrefixIdPolicyNumber($("#selectBranch").UifSelect("getSelected"), $("#selectPrefixCommercial").UifSelect("getSelected"), value.Risk.Policy.DocumentNumber)[0];
                                endorsements.push({ EndorsementId: endorsement.Id });
                            } else
                                endorsements.push({ TemporalId: value.Risk.Policy.Id });

                        })
                        massiveLoadPolicies = endorsements;
                    }
                })
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoLoadRecordsPrint, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    })
}

function AddLicensePlates() {
    var licensePlate = $('#inputLicensePlate').val().replace(/ /g, '');

    if (licensePlate.slice(-1) == ',') {
        licensePlate = licensePlate.slice(0, -1);
    }

    var licensePlates = licensePlate.split(',');
    var licensePlatesAdd = '';
    var summaries = GetTableSummary();
    var validateAll = true;

    if ($('#inputLicensePlates').val().length > 0) {
        licensePlatesAdd = $('#inputLicensePlates').val().split(',');

        $.each(licensePlates, function (key, value) {
            var description = this;
            var validate = true;
            $.each(licensePlatesAdd, function (key, value) {
                if (description.toString() == this.toString()) {
                    validate = false;
                }
            });

            if (validate == false) {
                validateAll = false;
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ThePlate + " " + description + ' ' + Resources.Language.IsAlreadyIncluded, 'autoclose': true });
            }
        });
    }

    if (validateAll == true) {
        $.each(licensePlates, function (key, value) {
            var description = this;
            var validate = false;
            $.each(summaries[0].Risks, function (key, value) {
                if (description.toString() == this.Description.toString()) {
                    validate = true;
                }
            });

            if (validate == false) {
                validateAll = false;
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ThePlate + ' ' + description + ' ' + Resources.Language.NotPartPolicy, 'autoclose': true });
            }
        });

        if (validateAll == true) {
            if ($('#inputLicensePlates').val().trim().length == 0) {
                licensePlatesAdd = licensePlate.replace(/,/g, ', ');
                $('#inputLicensePlates').val(licensePlatesAdd);
            }
            else {
                licensePlatesAdd = licensePlate.replace(/,/g, ', ');
                $('#inputLicensePlates').val($('#inputLicensePlates').val() + ', ' + licensePlatesAdd);
            }
        }
    }
}

function ValidateRange() {
    var summaries = GetTableSummary();
    var rangeFrom = 0;
    var rangeTo = 0;

    if ($('#inputRangeFrom').val().trim().length > 0) {
        rangeFrom = parseInt($('#inputRangeFrom').val(), 10);
        if (rangeFrom > summaries[0].RiskCount) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NumberEnteredGreaterRisks, 'autoclose': true });
        }
    }

    if ($('#inputRangeTo').val().trim().length > 0) {
        rangeTo = parseInt($('#inputRangeTo').val(), 10);
        if (rangeTo > summaries[0].RiskCount) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NumberEnteredGreaterRisks, 'autoclose': true });
        }
    }

    if ($('#inputRangeFrom').val().trim().length > 0 && $('#inputRangeTo').val().trim().length > 0) {
        if (rangeFrom > rangeTo) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.RangeMustLessThanFinal, 'autoclose': true });
        }
    }
}

function ShowPanelRisk(showSummary) {
    if (showSummary) {
        $('#divDetail').hide();
        $('#divSummary').show();
    }
    else {
        $('#divDetail').show();
        $('#divSummary').hide();
    }

    var summaries = GetTableSummary();
    var detail = $('#tableDetail').UifDataTable('getData');

    if (summaries.length > 0) {
        switch (summaries[0].CoveredRiskType) {
            case CoveredRiskType.Vehicles:
                $('#rbRiskAll').prop('checked', true);
                $('#modalVehicle').show();
                break;
            default:
                break;
        }
        DisableControls();
        $('#btnPrint').prop('disabled', false);
    }
    else if (detail.length > 0) {
        DisableControls();
        $('#btnPrint').prop('disabled', false);
    }
}

function PrintReport(printingModel, summary, outside, printMassive, printCollectFormat) {
    if (printMassive == false) {
        if (outside == false) {
            printingModel = $('#formPrinting').serializeObject();
        }

        function successFunc(data) {
            ShowPanelRisk(true);
            if (data.result.Url !== undefined) {
                if (!printCollectFormat) {

                    $('#divDownload').show();
                    $('#hrfPathName').text(data.result.Filename);
                    $('#hrfPathPdf').prop('href', data.result.Url);
                    $('#hrfPathPdf').prop('download', data.result.Filename);
                }
            }
            else if (data.result != 0) {
                glbPrintingId = data.result;
                $.UifNotify('show', { 'type': 'info', 'message': 'Se genero el proceso de impresion numero ' + data.result + ' favor consultarlo en unos segundos', 'autoclose': false });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }
        function failFunc(data) {
            if (data.result == Resources.Language.EndorsmentNotReinsured) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotPrinter + ": " + data.result, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
            }
        }
        if ($("#checkCurrentFromFirst").prop('checked')) {
            currentFromFirst = true;
        }
        else {
            currentFromFirst = false;
        }

        var x = GenerateReport(printingModel, successFunc, failFunc, endorsementText, currentFromFirst);
    }
    else {
        PrintMassiveLoad();
    }
}

function PrintMassiveLoad() {
    $.ajax({
        type: 'POST',
        url: rootPath + 'Printing/Printer/GenerateReportMassive',
        data: JSON.stringify({ companyFilterReports: massiveLoadPolicies }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            ShowPanelRisk(false);
            DownloadFile(data.result);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    });
}

function GenerateReport(printingModel, successFunc, failFunc, endorsementText, currentFromFirst) {
    
    if (currentState == "PolicyReady") {
        $.ajax({
            type: 'POST',
            async: true,
            url: rootPath + 'Printing/Printer/GenerateReportPolicy',
            data: JSON.stringify({ policyId: currentPolicyId, endorsementId: printingModel.EndorsementId, prefixId: printingModel.PrefixId, endorsementText: endorsementText, currentFromFirst: currentFromFirst }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                successFunc(data);
                pathFile = data.result.FilePathResult;
                if ($("#checkEmail").is(":checked")) {
                    SendMail(data.result.FilePathResult);
                }
                if ($("#checkCollectFormat").is(":checked")) {
                    GetDataByBranchIdPrefixIdPolicyNumber($("#selectBranch").UifSelect("getSelected"), $("#selectPrefixCommercial").UifSelect("getSelected"), $('#inputPolicyNumber').val(), parseInt($("#selectEndorsement option:selected").text()));
                }
            }
            else {
                failFunc(data);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
        });
    } else if (currentState == "TemporaryReady") {
        $.ajax({
            type: 'POST',
            async: true,
            url: rootPath + 'Printing/Printer/GenerateReportTemporary',
            data: JSON.stringify({ temporaryId: temporary.TempId, prefixId: temporary.CommonProperties.PrefixId, riskSince: temporary.CommonProperties.RiskSince, riskUntil: temporary.CommonProperties.RiskUntil, operationId: temporary.OperationId, tempAuthorization: temporalAutho }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                successFunc(data);
                pathFile = data.result.FilePathResult;
                if ($("#checkEmail").is(":checked")) {
                    SendMail(data.result.FilePathResult);
                }
            }
            else {
                failFunc(data);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
        })
    } else if (currentState == "QuotationReady") {
        $.ajax({
            type: 'POST',
            async: true,
            url: rootPath + 'Printing/Printer/GenerateReportQuotation',
            data: JSON.stringify({ temporaryId: $("#selectVersionQuota").UifSelect("getSelected"), prefixId: gblQuotation[0].CommonProperties.PrefixId, quotationId: gblQuotation[0].QuotationId, versionId: $("#selectVersionQuota").UifSelect("getSelectedText") }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                successFunc(data);
                pathFile = data.result.FilePathResult;
                if ($("#checkEmail").is(":checked")) {
                    SendMail(data.result.FilePathResult);
                }
            }
            else {
                failFunc(data);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
        });
    }
}

function DisableControls() {
    $('#inputLicensePlates').prop('disabled', true);
    $('#inputLicensePlates').val('');
    $('#inputLicensePlate').val('');
    $('#inputRangeFrom').val('');
    $('#inputRangeTo').val('');

    $('#inputLicensePlate').prop('disabled', true);
    $('#inputRangeFrom').prop('disabled', true);
    $('#inputRangeTo').prop('disabled', true);
    $('#btnPrint').prop('disabled', true);
    $("#embed").hide();
    $("#divMassiveDetail").hide();
}

function ClearPanels() {
    $('#divSummary').hide();
    $('#divDetail').hide();
    $('#divDownload').hide();
    ClearSummary();
    $('#tableDetail').UifDataTable('clear');
    $('#modalVehicle').hide();
    $("#checkEmail").prop("checked", false);
    $("#inputEmail").prop("disabled", true);
    $("#inputEmail").val('');
    $("#divSendMail").hide();
    $("#embed").hide();
}

function ClearAreaPolicy() {
    $('#selectBranch').UifSelect();
    $("#selectPrefixCommercial").UifSelect();
    $("#inputPolicyNumber").val("");
    $("#selectEndorsement").UifSelect();
}


function ClearAreaQuota() {
    $("#inputNumQuota").val("");
    $("#selectVersionQuota").UifSelect();
}

function GetEndorsementsByBranchIdPrefixIdPolicyNumber(branchId, prefixId, policyNumber, impPol) {
    var endorsements = null;

    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GetEndorsementsByFilterPolicy',
        data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            endorsements = data.result;
            $("#selectBranch").UifSelect("setSelected", data.result[0].Branch.Id);
            $("#selectPrefixCommercial").UifSelect("setSelected", data.result[0].Prefix.Id);
            if (endorsements[0] != null && !impPol) {
                var printingModel = {
                    TemporalId: endorsements[0].TemporalId,
                    EndorsementId: endorsements[0].Id,
                    PolicyId: endorsements[0].PolicyId
                };

                PrintReport(printingModel, endorsements[0], true, false, $("#checkCollectFormat").is(":checked"));
            }

            if (endorsements != null) {
                $('#selectEndorsement').UifSelect({ sourceData: endorsements, id: 'Id', name: 'Description' });
            }
            glbEndorsements = endorsements;
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
    });

}

function GetPrintingByPrintingId(printingId) {

    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GetPrintingProcessByPrintingId',
        data: JSON.stringify({ printingId: printingId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {

            if (data.result.TypePrinting !== undefined) {
                $('#divMassiveDetail').show();
                $('#inputRecordsTotal').val(data.result.TotalRisks);
                $('#inputRecordsProcessed').val(data.result.PendingRisks);
                $('#inputRecordsErrors').val(data.result.ErrorRisks);

                glbPrintingLogs = data.result.CompanyPrintingLogs;

                if (data.result.PendingRisks == 0) {
                    $('#btnGenerateZip').show();
                } else {
                    $('#btnGenerateZip').hide();
                }
            }
            else {
                $('#divDownload').show();
                $('#hrfPathName').text(data.result.Filename);
                $('#hrfPathPdf').prop('href', data.result.Url);
                $('#hrfPathPdf').prop('download', data.result.Filename);
            }

            $("#checkCollectFormat").prop("checked", false);
            $("#checkCollectFormat").prop("disabled", true);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGetPrintingPending, 'autoclose': true });
    });

}

function GetPrintSummaryByEndorsementId(endorsementId, impPol) {
    var summary = null;
    var endorsements = null;
    var endorsementNumber = endorsementId;
    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GetSummaryByEndorsementId',
        data: JSON.stringify({ endorsementId: endorsementId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            summary = data.result;
            endorsements = data.result;
            currentPolicyId = summary.PolicyId;
            if (summary != null && !impPol) {
                var printingModel = {
                    EndorsementId: endorsementNumber
                };

                PrintReport(printingModel, summary, true, false, $("#checkCollectFormat").is(":checked"));
            }
            LoadPanelEndorsement(summary);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchEndorsementInformation, 'autoclose': true });
    });
}

function GetVersionsByQuotationId(quotationId, branchId, prefixId) {
    var versions = null;
    $.ajax({
        type: 'POST',
        url: rootPath + 'Printing/Printer/GetQuotationByTemporalId',
        data: JSON.stringify({ quotationlId: quotationId, branchId: branchId, prefixId: prefixId }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            versions = data.result;
            $("#checkCollectFormat").prop("checked", false);
            $('#checkCollectFormat').prop('disabled', true);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchEndorsementInformation, 'autoclose': true });
    });
    gblQuotation = versions;
    return versions;
}

function FillSummary(summary) {
    $("#PrinterRiskCount").text(summary.RiskCount);
    $("#PrinterAmountInsured").text(summary.AmountInsured);
    $("#PrinterPremium").text(summary.Premium);
    $("#PrinterExpenses").text(summary.Expenses);
    $("#PrinterTaxes").text(summary.Taxes);
    $("#PrinterFullPremium").text(summary.FullPremium);
    $("#printerTableSummary").data("Object", summary);
}

function ClearSummary() {
    $("#PrinterRiskCount").text();
    $("#PrinterAmountInsured").text();
    $("#PrinterPremium").text();
    $("#PrinterExpenses").text();
    $("#PrinterTaxes").text();
    $("#PrinterFullPremium").text();
    $("#printerTableSummary").data("Object", null);
}

function GetTableSummary() {
    let summary = $("#printerTableSummary").data("Object");
    if (summary != null) {
        summary = [summary];
    }
    else {
        summary = "";
    }
    return summary;
}

function HidePrintSelect() {
    $("#printerRank").hide();
    $("#printerPlate").hide();
}

//function checkhierarchy() {
//    PrinterRequest.ValidationAccessAndHierarchy().done(function (data) {
//        if (!data.success) {
//            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
//        }
//    }).fail(function (jqXHR, textStatus, errorThrown) {
//        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
//    });
//}

function GetDataByBranchIdPrefixIdPolicyNumber(branchId, prefixId, policyNumber, endorsementNumber, impPol) {

    $('#hrfPathName').val("");
    $('#hrfPathPdf').val("");
    $('#hrfPathPdf').val("");
    $('#embed').val("");
    glbFilePath = "";
    $("#embed").hide();

    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GetPolicyForCollectFormat',
        data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber, endorsementNumber: endorsementNumber }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            glbEndorsementsCollectFormat = data.result.listEndorsement;
            $('#tableQuote').UifDataTable('clear');

            if (data.result.lstQuotes.length > 0) {
                $('#tableQuote').UifDataTable('addRow', data.result.lstQuotes);
                $.each(data.result.lstQuotes, function (id, item) {
                    if (item.Check == true) {
                        $('#tableQuote tbody tr:eq(' + id + ' )').removeClass('row-selected').addClass('row-selected');
                        $('#tableQuote tbody tr:eq(' + id + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                });
                $('#divQuotes').show();
                glbReadyForQuotes = true;
            } else if (data.result.validationPolicy != undefined) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result.validationPolicy, 'autoclose': true });
            }            
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
    });
}

function GenerateColletFormatReport(branchId, prefixId, policyNumber, listEndorsements, QuotesSelected) {

    var coutas = [];

    for (var i = 0; i < QuotesSelected.length; i++) {
        coutas.push(QuotesSelected[i].QuoteNumber);
    }

    if (coutas.length > 0) {
        $.ajax({
            type: 'POST',
            async: true,
            url: rootPath + 'Printing/Printer/GenerateReportCollectFormat',
            data: JSON.stringify({ endorsementList: listEndorsements, idCuotes: coutas, branchId: branchId, prefixId: prefixId, endorsementId: parseInt($("#selectEndorsement option:selected").text()), policyNumber: policyNumber }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                $('#embed').prop('src', data.result.Url);
                glbFilePath = data.result.FilePathResult;
                $("#embed").show();

                if (pathFile != undefined && glbFilePath != undefined) {
                    MergeFiles();
                }
                else if (glbPrintingId != 0 || glbPrintingId != undefined) {
                    $("#embed").hide();
                    $('#divDownload').hide();
                    UpdatePrintingCollectFormat(glbPrintingId, glbFilePath);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
        });
    } else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.QuotaSelected, 'autoclose': true });
    }
}

function UpdatePrintingCollectFormat(printingId, FilePathCollect) {
    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/UpdatePrintingProcess',
        data: JSON.stringify({ printingId: printingId, pathCollect: FilePathCollect }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {

    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
    });
}

function MergeFiles() {

    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/MergeFiles',
        data: JSON.stringify({ FilePolicy: pathFile, FileCollectFormat: glbFilePath }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            if (data.result.Url !== undefined) {
                $('#divDownload').show();
                //pathFile = data.result.Filename;
                $('#hrfPathName').text(data.result.Filename);
                $('#hrfPathPdf').prop('href', data.result.Url);
                $('#hrfPathPdf').prop('download', data.result.Filename);
            } else if (data.result.companyPrinting.Id != 0) {
                $.UifNotify('show', { 'type': 'info', 'message': 'proceso de impresion numero ' + data.result + ' se encuentra en proceso, favor consultarlo en unos segundos', 'autoclose': false });
            }
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {

    });

}