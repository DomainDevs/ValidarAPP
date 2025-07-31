var currentState;
var glbEndorsements = [];

$.ajaxSetup({ async: false });
$(document).ready(function () {
    $('#inputPolicyNumber').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputLoad').ValidatorKey(ValidatorType.Number, 2, 0);
});

$('#inputPolicyNumber').on('buttonClick', function () {
    if ($('#inputPolicyNumber').val().trim().length > 0) {
        LoadEndorsementsMassive();
    }
});

$('#inputTemporal').on('buttonClick', function () {
    if ($('#inputTemporal').val().trim().length > 0) {
        ClearAreaPolicy();
        LoadMassiveTemporal();
    }
});

$('input[name=radiosPrint]:radio').change(function () {
    var printSelect = $('input[name=radiosPrint]:checked').val();

    $('#btnPrint').prop('disabled', false);

    switch (printSelect) {
        case '1':
            $('#RiskArea').show();
            $('#RiskPlate').hide();
            break;
        case '2':
            $('#RiskArea').hide();
            $('#RiskPlate').show();
            break;
        default:
            break;
    }
});

$('#btnPrint').on('click', function () {

    GenerateReportMassive();

});

function LoadEndorsementsMassive() {

    GetEndorsementsByBranchIdPrefixIdPolicyNumberMassive($("#selectBranch").UifSelect("getSelected"), $("#selectPrefixCommercial").UifSelect("getSelected"), $('#inputPolicyNumber').val(), true);
}

function LoadMassiveTemporal() {

    GetMassiveByTemporalId($('#inputTemporal').val(), true);
    currentState = "TemporaryReady";
}

function ClearAreaPolicy() {
    $('#selectBranch').UifSelect();
    $("#selectPrefixCommercial").UifSelect();
    $("#inputPolicyNumber").val("");
    $("#selectEndorsement").UifSelect();
}

function GetEndorsementsByBranchIdPrefixIdPolicyNumberMassive(branchId, prefixId, policyNumber, impPol) {

    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GetEndorsementsByFilterPolicy',
        data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {

            glbEndorsements = data.result;

            if (glbEndorsements[0] != null) {
                $('#selectEndorsement').UifSelect({ sourceData: glbEndorsements, id: 'Id', name: 'EndorsementNumber' });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
    });

}

function GetMassiveByTemporalId(temporalId, impTem) {

    if (temporalId > 0) {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Printing/Printer/GetSummaryByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                summary = data.result;
                $.UifNotify('show', { 'type': 'info', 'message': "TEMPORARIO ENCONTRADO Y LISTO PARA IMPRIMIR", 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }
}

function GenerateReportMassive() {
    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GenerateReportMassive',
        data: JSON.stringify({ Massive: 0 }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            $('#divDownload').show();
            //window.open(data.result.Url, "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes,  copyhistory=yes");
            $('#hrfPathName').text(data.result.Url);
            $('#hrfPathPdf').prop('href', data.result.Url);
            $('#hrfPathPdf').prop('download', data.result.Url);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPrintingLoad, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
    });
}