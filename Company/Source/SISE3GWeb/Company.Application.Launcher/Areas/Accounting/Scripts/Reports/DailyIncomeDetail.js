/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var oParameterModel = {
    Id: 0,
    Name: "",
    Description: "",
    IsObject: false,
    Branch: 0,
    DateFrom: null,
    DateTo: null
};

var reportNameDailyIncomeDetail = Resources.DailyIncomeDetail.toUpperCase();
var typeProcessDaily = 2105;
var processTableRunning;
var isRunning = false;
var timeDailyIncome = window.setInterval(getMassiveReportDailyIncomeDetail, 6000);


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/
//refresca la grilla la primera vez cuando arranca la página
getMassiveReportDailyIncomeDetail();

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchDailyIncome").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchDailyIncome").removeAttr("disabled");
}


// Controla que la fecha final sea mayor a la inicial
$('#EndDateDailyIncomeDetail').blur(function () {
    if ($("#StartDateDailyIncomeDetail").val() != "") {
        if (compare_dates($("#StartDateDailyIncomeDetail").val(), $('#EndDateDailyIncomeDetail').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateDailyIncomeDetail").val('');
        } else {
            $("#EndDateDailyIncomeDetail").val($('#EndDateDailyIncomeDetail').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#StartDateDailyIncomeDetail').blur(function () {
    if ($("#EndDateDailyIncomeDetail").val() != "") {
        if (compare_dates($('#StartDateDailyIncomeDetail').val(), $("#EndDateDailyIncomeDetail").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateDailyIncomeDetail").val('');
        } else {
            $("#StartDateDailyIncomeDetail").val($('#StartDateDailyIncomeDetail').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});


//BOTÓN INICIAR
$("#MassiveReportDayliIncome").click(function () {

    $("#alertForm").UifAlert('hide');
    $("#ReportFormDailyIncome").validate();
    if ($("#ReportFormDailyIncome").valid()) {
        if ($("#StartDateDailyIncomeDetail").val() != "" && $("#EndDateDailyIncomeDetail").val() != "") {

            if (IsDate($("#StartDateDailyIncomeDetail").val()) == false || IsDate($("#EndDateDailyIncomeDetail").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateDailyIncomeDetail").val(), $("#EndDateDailyIncomeDetail").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveDailyIncome();
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
$("#btnModalDailyIncomeDetail").click(function () {

    $('#modalMassiveTotRecords').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/DailyIncomeDetailReports",
        data: {
            "dateFrom": $("#StartDateDailyIncomeDetail").val(), "dateTo": $("#EndDateDailyIncomeDetail").val(),
            "branchId": $('#BranchDailyIncome').val(), "reportType": typeProcessDaily
        },
        success: function (data) {
   
            if (data.success && data.result == 0) {

               getMassiveReportDailyIncomeDetail();

               if (!isRunning) {
                 timeDailyIncome = window.setInterval(getMassiveReportDailyIncomeDetail, 6000);
               }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

//BOTON LIMPIAR
$('#CancelIncomeDetail').click(function () {

    $('#BranchDailyIncome').val("");
    $('#StartDateDailyIncomeDetail').val("");
    $('#EndDateDailyIncomeDetail').val("");
    $("#selectFileTypePartial").val("");
    $("#alertForm").UifAlert('hide');
});


/*-------------------------------------------------------------------------------------------------------------------------*/
/*                                          DEFINICION DE FUNCIONES                                                        */
/*-------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReport(processId,records,reportDescription) {

        $.ajax({
            url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
            data: {
                "processId": processId, "reportTypeDescription": reportDescription,
                "exportFormatType": $("#selectFileTypePartial").val(),"recordsNumber": records
            },
            success: function (data) {
                if (data[0].ErrorInfo == null) {
                    if (data[0].ExportedFileName == "-1") {
                        $("#alertForm").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                    }
                    else {
                        
                        getMassiveReportDailyIncomeDetail();
                        if (!isRunning) {
                            timeDailyIncome = window.setInterval(getMassiveReportDailyIncomeDetail, 6000);
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
                generateFileReport(data.ProcessId, data.RecordsNumber, data.Description);
            }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkDailyDetail(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateDailyIncomeDetail").val("");
    $("#EndDateDailyIncomeDetail").val("");
}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveDailyIncome() {
    
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsDailyIncome",
        data: {"dateFrom": $("#StartDateDailyIncomeDetail").val(), "dateTo": $("#EndDateDailyIncomeDetail").val(),
            "branchId": $('#BranchDailyIncome').val()
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
function getMassiveReportDailyIncomeDetail() {
    
     var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameDailyIncomeDetail;
     $("#tableMassiveProcessReport").UifDataTable({ source: controller });        

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeDailyIncome);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }
 };


//Permite descargar el archivo ya generado
function setDownloadLinkDailyDetail(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertForm", "BranchDailyIncome");
}

