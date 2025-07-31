/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var oParameterModel = {
    Id: 0,
    Name: "",
    Description: "",
    IsObject: false,
    BranchDailyClosing: 0,
    DateFrom: null,
    DateTo: null
};


var reportNameDailyClosing = Resources.CashClosingList.toUpperCase();
var typeProcessDailyClosing = 2087;
var processTableRunning;
var isRunning = false;
var timeDailyClosing = window.setInterval(getMassiveReportDailyClosingList, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/
//refresca la grilla la primera vez cuando arranca la página
getMassiveReportDailyClosingList();

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchDailyClosing").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchDailyClosing").removeAttr("disabled");
}

// Controla que la fecha final sea mayor a la inicial
$('#StartDateDailyClosing').blur(function () {
    if ($("#EndDateDailyClosing").val() != "") {
        if (compare_dates($('#StartDateDailyClosing').val(), $("#EndDateDailyClosing").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateDailyClosing").val('');
        } else {
            $("#StartDateDailyClosing").val($('#StartDateDailyClosing').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});


// Controla que la fecha final sea mayor a la inicial
$('#EndDateDailyClosing').blur(function () {
    if ($("#StartDateDailyClosing").val() != "") {
        if (compare_dates($("#StartDateDailyClosing").val(), $('#EndDateDailyClosing').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateDailyClosing").val('');
        } else {
            $("#EndDateDailyClosing").val($('#EndDateDailyClosing').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

//BOTÓN INICIAR
$("#MassiveDailyClosing").click(function () {

    $("#alertForm").UifAlert('hide');
    $("#ReportFormDailyClosing").validate();
    if ($("#ReportFormDailyClosing").valid()) {
        if ($("#StartDateDailyClosing").val() != "" && $("#EndDateDailyClosing").val() != "") {

            if (IsDate($("#StartDateDailyClosing").val()) == false || IsDate($("#EndDateDailyClosing").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateDailyClosing").val(), $("#EndDateDailyClosing").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveDailyClosing();
                    }, 300);
                }
            }
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

//Botón (SI) del modal de confirmación
$("#btnModalMassiveDailyClosing").click(function () {

    $('#modalMassiveTotRecords').modal('hide');
    
    $.ajax({
        url: ACC_ROOT + "Reports/DailyClosingReports",
        data: {
            "dateFrom": $("#StartDateDailyClosing").val(), "dateTo": $("#EndDateDailyClosing").val(),
            "branchId": $('#BranchDailyClosing').val(), "reportType": typeProcessDailyClosing
        },
        success: function (data) {

            if (data.success && data.result == 0) {
               
                getMassiveReportDailyClosingList();

                if (!isRunning) {
                    timeDailyClosing = window.setInterval(getMassiveReportDailyClosingList, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

//BOTON LIMPIAR
$('#CancelClosingList').click(function () {
    $('#BranchDailyClosing').val("");
    $('#StartDateDailyClosing').val("");
    $('#EndDateDailyClosing').val("");
    $("#selectFileTypePartial").val("");
    $("#alertForm").UifAlert('hide');
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/
//Genera el archivo una vez terminado el proceso inicial
function generateFileReportDailyClosing(processId, records, reportDescription) {
    
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

                    getMassiveReportDailyClosingList();
                    if (!isRunning) {
                        timeDailyClosing = window.setInterval(getMassiveReportDailyClosingList, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

//Valida la fila seleccionada(lapiz edit)
function whenSelectRow(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReportDailyClosing(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkDailyClosing(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateDailyClosing").val("");
    $("#EndDateDailyClosing").val("");
}


function getTotalRecordsMassiveDailyClosing() {
    
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsDailyClosing",
        data: {
            "dateFrom": $("#StartDateDailyClosing").val(), "dateTo": $("#EndDateDailyClosing").val(),
            "branchId": $('#BranchDailyClosing').val()
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

//Refresca o detiene la grilla de avance de proceso
function getMassiveReportDailyClosingList() {
    
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameDailyClosing;
                    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeDailyClosing);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }
      
};

//Permite descargar el archivo ya generado
function SetDownloadLinkDailyClosing(fileName) {
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




