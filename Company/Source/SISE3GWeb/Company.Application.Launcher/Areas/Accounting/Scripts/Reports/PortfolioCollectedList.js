/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNamePortfolioCollected = Resources.PortfolioCollected.toUpperCase();
var typeProcessPortfolioCollected = 2146;
var processTableRunningPortfolioCollected;
var isRunningPortfolioCollected = false;
var timePortfolioCollected = window.setInterval(getMassiveReportPortfolioCollected, 6000);
var insuredId = "";
var agentId = "";
var userId = "";

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

getMassiveReportPortfolioCollected();
setTimeout(function () {
    $("#InsuredPortfolioCollected").attr("disabled", true);
    $("#BrokerPortfolioCollected").attr("disabled", true);
    $("#UserPortfolioCollected").attr("disabled", true);
    $("#DocumentInsuredPortfolioCollected").attr("disabled", true);
    $("#DocumentBrokerPortfolioCollected").attr("disabled", true);
}, 1000);


$('.cPersonTypePortfolioCollected').change(function () {

    if ($("#allCompanyPortfolioCollected").is(':checked'))
    {
        $("#InsuredPortfolioCollected").attr("disabled",true);
        $("#BrokerPortfolioCollected").attr("disabled" ,true);
        $("#UserPortfolioCollected").attr("disabled", true);
        $("#DocumentInsuredPortfolioCollected").attr("disabled", true);
        $("#DocumentBrokerPortfolioCollected").attr("disabled", true);
        $("#InsuredPortfolioCollected").val("");
        $("#BrokerPortfolioCollected").val("");
        $("#UserPortfolioCollected").val("");
    }
    else
        if ($("#insuredRadioPortfolioCollected").is(':checked')) {
            $("#BrokerPortfolioCollected").attr("disabled", true);
            $("#DocumentBrokerPortfolioCollected").attr("disabled", true);
            $("#UserPortfolioCollected").attr("disabled", true);
            $("#InsuredPortfolioCollected").attr("disabled", false);
            $("#DocumentInsuredPortfolioCollected").attr("disabled", false);
            $("#BrokerPortfolioCollected").val("");
            $("#DocumentBrokerPortfolioCollected").val("");
            $("#UserPortfolioCollected").val("");
    }
        else
            if ($("#brokerRadioPortfolioCollected").is(':checked')) {
                $("#InsuredPortfolioCollected").attr("disabled", true);
                $("#DocumentInsuredPortfolioCollected").attr("disabled", true);
                $("#UserPortfolioCollected").attr("disabled", true);
                $("#BrokerPortfolioCollected").attr("disabled", false);
                $("#DocumentBrokerPortfolioCollected").attr("disabled", false);
                $("#InsuredPortfolioCollected").val("");
                $("#DocumentInsuredPortfolioCollected").val("");
                $("#UserPortfolioCollected").val("");
            } else
                if ($("#userRadioPortfolioCollected").is(':checked'))  {
                    $("#InsuredPortfolioCollected").attr("disabled", true);
                    $("#BrokerPortfolioCollected").attr("disabled", true);
                    $("#DocumentInsuredPortfolioCollected").attr("disabled", true);
                    $("#DocumentBrokerPortfolioCollected").attr("disabled", true);
                    $("#UserPortfolioCollected").attr("disabled", false);
                    $("#InsuredPortfolioCollected").val("");
                    $("#BrokerPortfolioCollected").val("");
                    $("#DocumentInsuredPortfolioCollected").val("");
                    $("#DocumentBrokerPortfolioCollected").val("");
                }
});

$('#CancelPortfolioCollected').click(function () {
    $('#BranchPortfolioCollected').val("");
    $('#PrefixPortfolioCollected').val("");
    $('#StartDatePortfolioCollected').val("");
    $('#EndDatePortfolioCollected').val("");
    $("#selectFileTypePartial").val("");
    $("#InsuredPortfolioCollected").val("");
    $("#InsuredPortfolioCollected").val("");
    $("#BrokerPortfolioCollected").val("");
    $("#alertForm").UifAlert('hide');
});

// Controla que la fecha final sea mayor a la inicial
$('#StartDatePortfolioCollected').blur(function () {
    if ($("#EndDatePortfolioCollected").val() != "") {
        if (compare_dates($('#StartDatePortfolioCollected').val(), $("#EndDatePortfolioCollected").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDatePortfolioCollected").val('');
        } else {
            $("#StartDatePortfolioCollected").val($('#StartDatePortfolioCollected').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDatePortfolioCollected').blur(function () {
    if ($("#StartDatePortfolioCollected").val() != "") {
        if (compare_dates($("#StartDatePortfolioCollected").val(), $('#EndDatePortfolioCollected').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDatePortfolioCollected").val('');
        } else {
            $("#EndDatePortfolioCollected").val($('#EndDatePortfolioCollected').val());
        }
    }
});

$("#MassiveReportPortfolioCollected").click(function () {

    $("#alertForm").UifAlert('hide');

    if ($("#insuredRadioPortfolioCollected").is(':checked')) {
        if ($("#DocumentInsuredPortfolioCollected").val() == "" || $("#InsuredPortfolioCollected").val() == "") {
            $("#spanDocumentInsured").show();
            $("#spanInsured").show();
            $("#spanUser").hide();
            $("#spanDocumentBroker").hide();
            $("#spanBroker").hide();

        }
    }

    if ($("#brokerRadioPortfolioCollected").is(':checked')) {
        if ($("#DocumentBrokerPortfolioCollected").val() == "" || $("#BrokerPortfolioCollected").val() == "") {
            $("#spanDocumentBroker").show();
            $("#spanBroker").show();
            $("#spanUser").hide();
            $("#spanDocumentInsured").hide();
            $("#spanInsured").hide();

        }
    }
    if ($("#userRadioPortfolioCollected").is(':checked')) {
        if ($("#UserPortfolioCollected").val() == "") {
            $("#spanUser").show();
            $("#spanDocumentInsured").hide();
            $("#spanInsured").hide();
            $("#spanDocumentBroker").hide();
            $("#spanBroker").hide();
        }
    }


    $("#ReportFormPortfolioCollected").validate();

    if ($("#ReportFormPortfolioCollected").valid()) {
        if ($("#StartDatePortfolioCollected").val() != "" && $("#EndDatePortfolioCollected").val() != "") {

            if (IsDate($("#StartDatePortfolioCollected").val()) == false || IsDate($("#EndDatePortfolioCollected").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDatePortfolioCollected").val(), $("#EndDatePortfolioCollected").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassivePortfolioCollected();
                    }, 300);
                    
                }
            }
        }
    }
});

//AUTOCOMPLETES ASEGURADO DOCUMENTO
$("#InsuredPortfolioCollected").on("itemSelected", function (event, selectedItem) {
    insuredId = selectedItem.Id;
    agentId = null;
    userId = null;

    $("#DocumentInsuredPortfolioCollected").val(selectedItem.DocumentNumber);
});

$("#DocumentInsuredPortfolioCollected").on("itemSelected", function (event, selectedItem) {
    insuredId = selectedItem.Id;
    agentId = null;
    userId = null;
    $("#InsuredPortfolioCollected").val(selectedItem.Name);
});


//AUTOCOMPLETES ASEGURADO DOCUMENTO
$('#BrokerPortfolioCollected').on('itemSelected', function (event, selectedItem) {

    insuredId = null;
    agentId = selectedItem.Id;
    userId = null;
    $('#DocumentBrokerPortfolioCollected').val(selectedItem.DocumentNumber);


});

$('#DocumentBrokerPortfolioCollected').on('itemSelected', function (event, selectedItem) {

    insuredId = null;
    agentId = selectedItem.Id;
    userId = null;
    $('#BrokerPortfolioCollected').val(selectedItem.Name);
});

//AUTOCOMPLETES ASEGURADO DOCUMENTO
$('#UserPortfolioCollected').on('itemSelected', function (event, selectedItem) {

    insuredId = null;
    agentId = null;
    userId = selectedItem.id;

});
//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRowPortfolioCollected(data);
});

$("#btnModalPortfolioCollected").click(function () {

    $('#modalMassiveTotRecords').modal('hide');

    if ($('#BranchPortfolioCollected').val() != "") {
        branchId = $('#BranchPortfolioCollected').val();
    }

    if ($('#PrefixPortfolioCollected').val() != "") {
        prefixId = $('#PrefixPortfolioCollected').val();
    }

    listBy = $('input:radio[name=options2]:checked').val();

    if ($("#allCompanyPortfolioCollected").is(':checked')) {
        all = "A";
    }

    $.ajax({
        url: ACC_ROOT + "Reports/PortfolioCollectedReports",
        data: {
            "branchId": branchId, "prefixId": prefixId, "dateFrom": $("#StartDatePortfolioCollected").val(),
            "dateTo": $("#EndDatePortfolioCollected").val(), "listBy": listBy,
            "selectAll": all, "insuredId": insuredId, "agentId": agentId, "userId": userId,
            "reportType": typeProcessPortfolioCollected
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportPortfolioCollected();

                if (!isRunningPortfolioCollected) {
                    timePortfolioCollected = window.setInterval(getMassiveReportPortfolioCollected, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/


function getMassiveReportPortfolioCollected() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNamePortfolioCollected;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningPortfolioCollected = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningPortfolioCollected != undefined) {
        if (validateProcessReport(processTableRunningPortfolioCollected, processTableRunningPortfolioCollected.length)) {
            clearInterval(timePortfolioCollected);
            isRunningPortfolioCollected = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningPortfolioCollected = true;
        }
    }
};


//Valida la fila seleccionada(lapiz edit)
function whenSelectRowPortfolioCollected(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReportPortfolioCollected(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkPortfolioCollected(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDatePortfolioCollected").val("");
    $("#EndDatePortfolioCollected").val("");
}


//Genera el archivo una vez terminado el proceso inicial
function generateFileReportPortfolioCollected(processId, records, reportDescription) {

    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertForm").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportPortfolioCollected();
                    if (!isRunningPortfolioCollected) {
                        timePortfolioCollected = window.setInterval(getMassiveReportPortfolioCollected, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function getTotalRecordsMassivePortfolioCollected() {

    if ($('#BranchPortfolioCollected').val() != "") {
        branchId = $('#BranchPortfolioCollected').val();
    }

    if ($('#PrefixPortfolioCollected').val() != "") {
        prefixId = $('#PrefixPortfolioCollected').val();
    }

    listBy = $('input:radio[name=options2]:checked').val();

    if ($("#allCompanyPortfolioCollected").is(':checked')) {
        all = "A";
    }
    else {
        all = "";
    }



    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsPortfolioCollected",
        data: {
            "branchId": branchId, "prefixId": prefixId, "dateFrom": $("#StartDatePortfolioCollected").val(),
            "dateTo": $("#EndDatePortfolioCollected").val(), "listBy": listBy,
            "selectAll": all, "insuredId": insuredId, "agentId": agentId, "userId": userId
        },
        success: function (data) {

            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertForm").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertForm").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

function SetDownloadLinkPortfolioCollected(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertForm", "selectFileTypePartial");
}


//Genera el archivo una vez terminado el proceso inicial
function generateFileReportPortfolioCollected(processId, records, reportDescription) {

    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertForm").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {

                    getMassiveReportPortfolioCollected();
                    if (!isRunningPortfolioCollected) {
                        timePortfolioCollected = window.setInterval(getMassiveReportPortfolioCollected, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
};

