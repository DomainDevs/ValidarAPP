var productId = null;
var massiveLoadPolicies = [];
var searchModel;
var glbEndorsements = [];
var currentPolicyId;
var currentState;
var glbPolicy;
var glbFilePath;

$.ajaxSetup({ async: false });
$(document).ready(function () {
    $('#inputPolicyNumber').ValidatorKey(ValidatorType.Number, 2, 0);
    $('#inputLoad').ValidatorKey(ValidatorType.Number, 2, 0);
    $("#checkEmail").prop("checked", false);
});


//Eventos
$("#checkEmail").change(function () {
    if ($("#checkEmail").is(":checked")) {
        $("#inputEmail").prop("disabled", false);
        $("#btnSendMail").show();
    }
    else {
        $("#inputEmail").prop("disabled", true);
        $("#btnSendMail").hide();
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
        LoadEndorsement();
    }
});

$('#btnPrintFormat').on('click', function () {

    var quotesSelected = [];
    $.each($("#tableQuote").UifDataTable("getSelected"), function (Id, item) {
        quotesSelected.push({
            QuoteNumber: item.QuoteNumber
        });
    });

    GenerateColletFormatReport($("#selectBranch").UifSelect("getSelected"), $("#selectPrefixCommercial").UifSelect("getSelected"), $('#inputPolicyNumber').val(), quotesSelected);
});

$("#btnClose").on("click", function () {
    window.location = rootPath + "Home/Index";
});

$('#btnSendMail').on('click', function () {
    if (glbFilePath != "") {
        SendMail(glbFilePath);
    }
    else {
        $.UifNotify('show', { 'type': 'danger', 'message': 'Falta url de documento generado', 'autoclose': true });
    }  
});

$('#selectEndorsement').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        LoadDataCollectFormat();
    }
});


function LoadEndorsement() {
    $('#selectEndorsement').UifSelect();
    $('#formCollectPrinting').validate();

    if ($('#formCollectPrinting').valid()) {

        var branch = $("#selectBranch").UifSelect("getSelected")
        var prefix = $("#selectPrefixCommercial").UifSelect("getSelected")

        if (branch == null && prefix == null) {
            branch = 0
            prefix = 0
        }
        GetEndorsementsByBranchIdPrefixIdPolicyNumber(branch, prefix, $('#inputPolicyNumber').val(), true);
    }
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
            if (data.result != null) {
                $("#selectBranch").UifSelect("setSelected", data.result[0].Branch.Id);
                $("#selectPrefixCommercial").UifSelect("setSelected", data.result[0].Prefix.Id);
                $('#selectEndorsement').UifSelect({ sourceData: data.result, id: 'Id', name: 'Description' });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
    });

}

//Metodos
function SendMail(path) {
    if ($("#checkEmail").is(":checked")) {
        if ($('#inputEmail').val() != '') {
            $.ajax({
                type: 'POST',
                async: true,
                url: rootPath + 'Printing/Printer/SendToEmail',
                data: JSON.stringify({ email: $('#inputEmail').val(), filePath: path }),
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

function LoadDataCollectFormat() {
    
    if ($('#formCollectPrinting').valid()) {
        var branch = $("#selectBranch").UifSelect("getSelected");
        var prefix = $("#selectPrefixCommercial").UifSelect("getSelected");

        if (branch == null && prefix == null) {
            branch = 0
            prefix = 0
        }
        GetDataByBranchIdPrefixIdPolicyNumber(branch, prefix, $('#inputPolicyNumber').val(), parseInt($("#selectEndorsement option:selected").text()));
    }
}

function LoadPanelEndorsement(summary) {
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

function LoadTitle(summary) {
    var titlePrincipal = 'Póliza: ' + $("#selectBranch").UifSelect("getSelectedText");
    titlePrincipal = titlePrincipal + ' ' + $("#selectPrefix").UifSelect("getSelectedText");
    titlePrincipal = titlePrincipal + ' ' + $('#inputPolicyNumber').val();
    titlePrincipal = titlePrincipal + ' ' + summary.ProductDescription;
    $.uif2.helpers.setGlobalTitle(titlePrincipal);
}

function ClearAreaPolicy() {
    $('#selectBranch').UifSelect();
    $("#selectPrefixCommercial").UifSelect();
    $("#inputPolicyNumber").val("");
    $("#selectEndorsement").UifSelect();
    $("#radioNumPolicy").prop("checked", true);
    $("#radioPay").prop("checked", false);
}


function GetDataByBranchIdPrefixIdPolicyNumber(branchId, prefixId, policyNumber, endorsementNumber) {
    glbFilePath = "";

    $.ajax({
        type: 'POST',
        async: true,
        url: rootPath + 'Printing/Printer/GetPolicyForCollectFormat',
        data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber, endorsementNumber: endorsementNumber }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        if (data.success) {
            if (data.result.lstQuotes != null){
                glbEndorsements = data.result.listEndorsement;
                $('#tableQuote').UifDataTable('clear');
                $('#tableQuote').UifDataTable('addRow', data.result.lstQuotes);
                $.each(data.result.lstQuotes, function (id, item) {
                    if (item.Check == true) {
                        $('#tableQuote tbody tr:eq(' + id + ' )').removeClass('row-selected').addClass('row-selected');
                        $('#tableQuote tbody tr:eq(' + id + ' ) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                });
                $('#divQuotes').show();
                $("#btnPrintFormat").prop("disabled", false);
            }
            else {
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

function GenerateColletFormatReport(branchId, prefixId, policyNumber, QuotesSelected) {

    var coutas = [];

    for (var i = 0; i < QuotesSelected.length; i++) {
        coutas.push(QuotesSelected[i].QuoteNumber);
    }

    if (coutas.length > 0) {

        $.ajax({
            type: 'POST',
            async: true,
            url: rootPath + 'Printing/Printer/GenerateReportCollectFormat',
            data: JSON.stringify({ endorsementList: glbEndorsements, idCuotes: coutas, branchId: branchId, prefixId: prefixId, endorsementId: parseInt($("#selectEndorsement option:selected").text()), policyNumber: policyNumber }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                //$('#divDownload').show();
                $('#hrfPathName').text(data.result.Filename);
                $('#hrfPathPdf').prop('href', data.result.Url);
                $('#hrfPathPdf').prop('download', data.result.Filename);
                $('#embed').prop('src', data.result.Url);
                glbFilePath = data.result.FilePathResult;
                $("#embed").show();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
        });
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': 'Por favor seleccionar por lo menos una cuota', 'autoclose': true });
    }
}
