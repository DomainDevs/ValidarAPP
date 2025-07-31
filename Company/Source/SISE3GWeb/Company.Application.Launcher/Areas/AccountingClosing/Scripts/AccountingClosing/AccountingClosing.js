/**
    * @file   AccountingClosing.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var entryClosing = 0;
//var timerAccountingClosing = window.setInterval(refreshGridAccountingClosing, 3000);
var timerAccountingClosing;
var isRunningProcessIss = false;
var isRunningProcessClm = false;
var isRunningProcessReins = false;
var isRunningProcessRsv = false;
var isRunningProcessIBNR = false;
var validateClosureMonthPromise;


/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainAccountingClosing();
});

class MainAccountingClosing extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        $('#month').attr("disabled", "disabled");
        $('#day').attr("disabled", "disabled");

        setTimeout(function () {
            $('#month').attr("disabled", "disabled");
            $('#excelAccountingClosing').attr("disabled", "disabled");
        }, 700);
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#module").on("itemSelected", this.SelectClosingModule);
        $("#module1").on("itemSelected", this.SelectClosingModule1);
        $("#processAccountingClosing").on('click', this.GenerateMonthlyClosing);
        $("#excelAccountingClosing").on('click', this.ExportMonthlyClosing);
        $("#accountingAccountingClosing").on('click', this.AccountingMonthlyClosing);
        $("#printAccountingClosing").on('click', this.PrintMonthlyClosing);
        $("#modalPrint").find("#btnPrintModal").on('click', this.PrintAccountingMonthlyClosing);
        $('#tblReports11').on('rowSelected', this.SelectTableRow);
        $('#tblReports11').on('selectAll', this.SelectTableAll);
        $('body').delegate('#tblReports tbody tr', "click", this.SelectTableTBodyRow);
        $('body').delegate('#tblReports thead tr', "click", this.SelectTableTHeadRow);
    }



    /**
        * Obtiene la fecha del módulo seleccionado.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del módulo seleccionado.
        */
    SelectClosingModule1() {

        $("#alertAccountingClosing").UifAlert('hide');

        //Habilita / deshabilita el botón de Generar según sea el caso
        switch (parseInt($("#module").val())) {

            case 3:
                MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessIss);
                break;
            case 4:
                MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessClm);
                break;
            case 7:
                MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessReins);
                break;
            case 11:
                MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessRsv);
                break;
            case 14:
                MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessIBNR);
                break;

            default:
        }

        MainAccountingClosing.RefreshGridAccountingClosing();

        if ($("#module").val() != "") {
        
            $("#alertAccountingClosing").UifAlert('hide');
            $("#tblReports").dataTable().fnClearTable();
            $("#excelAccountingClosing").attr("href", "#");
            $("#excelAccountingClosing").attr("disabled", "disabled");
            $("#printAccountingClosing").attr("disabled", "disabled");
            $("#processAccountingClosing").attr("disabled", false);
            $("#accountingAccountingClosing").attr("disabled", "disabled");

            if ($('#module').val() == $("#ViewBagIncomeAndExpensesModule").val()) // Validación solo se debe habilitar el botón Contabilizar para el cierre de ingresos y egresos
            {
                $("#excelAccountingClosing").attr("disabled", "disabled");
                $("#printAccountingClosing").attr("disabled", "disabled");
                $("#processAccountingClosing").attr("disabled", "disabled");
                $("#accountingAccountingClosing").attr("disabled", false);
            }

            if ($('#module').val() != "" && $('#module').val() != null) {
                var controller = ACL_ROOT + "Base/GetModuleDays?moduleId=" + $("#module").val();
                $("#day").UifSelect({
                    source: controller
                });

                $.ajax({
                    type: "POST",
                    url: ACL_ROOT + "AccountingClosing/GetClosureMonth",
                    data: JSON.stringify({
                        "moduleId": $("#module").val()
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $('#month').val(data.month);
                        $('#YearAccountingClosing').val(data.year);

                        setTimeout(function () {
                            $('#month').attr("disabled", "disabled");
                            $('#day').val(data.day);
                            $('#day').attr("disabled", "disabled");
                        }, 700);
                    }
                });
            } else {
                $('#month').val("");
            }
        } else {

            $("#processAccountingClosing").attr("disabled", true);
        }
    }

    /**
        * Obtiene la fecha del módulo seleccionado.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del módulo seleccionado.
        */
    SelectClosingModule(event, selectedItem) {
        if (selectedItem.Id > 0 && selectedItem.Id != null) {
            $("#alertAccountingClosing").UifAlert('hide');
            $("#tblReports").dataTable().fnClearTable();
            $("#excelAccountingClosing").attr("href", "#");
            $("#excelAccountingClosing").attr("disabled", "disabled");
            $("#printAccountingClosing").attr("disabled", "disabled");
            $("#processAccountingClosing").attr("disabled", false);
            $("#accountingAccountingClosing").attr("disabled", "disabled");

            // Validación solo se debe habilitar el botón Contabilizar para el cierre de ingresos y egresos
            if (selectedItem.Id == $("#ViewBagIncomeAndExpensesModule").val()) 
            {
                $("#excelAccountingClosing").attr("disabled", "disabled");
                $("#printAccountingClosing").attr("disabled", "disabled");
                $("#processAccountingClosing").attr("disabled", "disabled");
                $("#accountingAccountingClosing").attr("disabled", false);
            }

            MainAccountingClosing.ValidateClosureMonth(selectedItem.Id);
            validateClosureMonthPromise.then(function (validationData) {
                var controller = ACL_ROOT + "Base/GetModuleDays?moduleId=" + selectedItem.Id;
                $("#day").UifSelect({ source: controller, selectedId: String(validationData.day).trim() });

                $('#month').val(validationData.month);
                $('#YearAccountingClosing').val(validationData.year);
                setTimeout(function () {
                    $('#day').attr("disabled", "disabled");
                }, 1000);

                //MainAccountingClosing.GetClosingProcess(selectedItem.Id);
                timerAccountingClosing = window.setInterval(MainAccountingClosing.RefreshClosingProcess, 3000);
            });

        }
        else {
            $('#day').val("");
            $('#month').val("");
            $('#YearAccountingClosing').val("");
            window.clearInterval(timerAccountingClosing);
            $("#tblReports").UifDataTable('clear');
        }
    }

    /**
        * Genera el proceso de cierre mensual.
        *
        */
    GenerateMonthlyClosing() {

        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "<h1>" + Resources.MessageWaiting + "</h1>"
        });

        $("#alertAccountingClosing").UifAlert('show', Resources.MessageWaiting, "info");
        $("#processAccountingClosing").attr("disabled", "disabled");

        const REINSURANCE_MODULE_ID = 7;

        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/MonthlyClosure",
            data: JSON.stringify({ "module": $("#module").val() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    if ($("#module").val() == REINSURANCE_MODULE_ID) {
                        if (data.result > 0) {
                            var url = ACL_ROOT + "AccountingClosing/ReportExcel?module=" + $("#module").val();
                            $("#excelAccountingClosing").attr("href", url);
                            $("#alertAccountingClosing").UifAlert('show', Resources.MessageFinalizedProcess, "success");

                            $("#excelAccountingClosing").attr("disabled", false);
                            $("#printAccountingClosing").attr("disabled", false);
                        }
                        if (data.result == 0) {
                            $("#alertAccountingClosing").UifAlert('show', Resources.MessageNotAvailable, "warning");
                            $("#tblReports").UifDataTable({
                                source: null
                            });
                        }
                    } else {
                        if (data.result == 0) {
                            $("#alertAccountingClosing").UifAlert('show', Resources.MessageNotAvailable, "warning");
                            $("#tblReports").UifDataTable({
                                source: null
                            });
                        } else if (data.result == 1) {
                            var url = ACL_ROOT + "AccountingClosing/ReportExcel?module=" + $("#module").val();
                            $("#excelAccountingClosing").attr("href", url);
                            $("#alertAccountingClosing").UifAlert('show', Resources.MessageFinalizedProcess, "success");

                            $("#excelAccountingClosing").attr("disabled", false);
                            $("#printAccountingClosing").attr("disabled", false);
                        }
                        else {
                            $("#alertAccountingClosing").UifAlert('show', Resources.MessageInternalError, "danger");
                            $("#tblReports").UifDataTable({ source: null });
                        }
                    }
                    $("#processAccountingClosing").attr("disabled", false);
                }
                else {
                    $("#alertAccountingClosing").UifAlert('show', data.result, "danger");
                    $("#tblReports").UifDataTable({ source: null });
                }
                MainAccountingClosing.GetClosingProcess($("#module").val());
                timerAccountingClosing = window.setInterval(MainAccountingClosing.RefreshClosingProcess, 3000);
                $.unblockUI();
            }
        });
    }

    /**
        * Genera el reporte en formato pdf.
        *
        */
    PrintMonthlyClosing() {
        $("#alertAccountingClosing").UifAlert('hide');
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "<h1>" + Resources.MessageWaiting + "</h1>"
        });

        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/LoadMonthlyProcessReport",
            data: JSON.stringify({
                "module": $("#module").val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                MainAccountingClosing.ShowReportAccountingClosing();
                $("#accountingAccountingClosing").attr("disabled", false);
                $.unblockUI();
            }
        });
    }

    /**
        * Genera el reporte en formato excel y lo descarga.
        *
        */
    ExportMonthlyClosing() {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "<h1>" + Resources.MessageWaiting + "</h1>"
        });

        $("#alertAccountingClosing").UifAlert('hide');
        if ($(this).attr("href") != "#") {
            $("#accountingAccountingClosing").attr("disabled", false);
            $.unblockUI();
        }
        $.unblockUI();
    }

    /**
        * Genera la contabilización del cierre mensual.
        *
        */
    AccountingMonthlyClosing() {

        $.blockUI({
            //$('div.main-container').block({             
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: "<h1>" + Resources.MessageWaiting + "</h1>"
        });

        $("#processAccountingClosing").attr("disabled", "disabled");
        $("#printAccountingClosing").attr("disabled", "disabled");
        $("#excelAccountingClosing").attr("disabled", "disabled");
        $("#module").attr("disabled", "disabled");
        $("#accountingAccountingClosing").attr("disabled", "disabled");

        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/AccountClosure",
            data: JSON.stringify({
                "module": $("#module").val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {

                    if (data.result.length > 0) {

                        var description = "";

                        for (var i = 0; i < data.result.length; i++) {
                            description = description + data.result[i] + ". ";
                        }

                        if ($("#module").val() == $("#ViewBagIncomeAndExpensesModule").val()) {
                            entryClosing = description.replace(". ", "");
                        }

                        $('#modalPrint').find("#printMessageModal").html("<p class= 'text-info'>" + Resources.MessageAccountingEntry + '(s): ' + description + "</p>" + "<br/>" + Resources.MessagePrintAccountingEntry);

                        $('#modalPrint').modal({
                            backdrop: 'static'
                        });

                        $('#modalPrint').detach().appendTo("body").modal('show');

                        $("#processAccountingClosing").attr("disabled", false);
                        $("#module").attr("disabled", false);
                    } else if (data.result.length == 0) {
                        $("#alertAccountingClosing").UifAlert('show', Resources.MessageNotDataAnnualClosure, "warning");
                        $("#processAccountingClosing").attr("disabled", false);
                        $("#module").attr("disabled", false);
                    } else {
                        $("#alertAccountingClosing").UifAlert('show', Resources.ProcessExecutionError, "danger");
                        $("#processAccountingClosing").attr("disabled", false);
                        $("#module").attr("disabled", false);
                    }
                } else {
                    $("#alertAccountingClosing").UifAlert('show', Resources.MessageInternalError + ' ' + Resources.Details + ': ' + data.result, "danger");
                    $("#processAccountingClosing").attr("disabled", false);
                    $("#module").attr("disabled", false);
                }
                $.unblockUI();
                //$('div.main-container').unblock(); 
            }
        });
    }

    /**
        * Imprime la contabilización del cierre mensual en formato pdf.
        *
        */
    PrintAccountingMonthlyClosing() {
        $("#alertAccountingClosing").UifAlert('hide');
        $('#modalPrint').modal('hide');
        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/LoadPrintEntryReport",
            data: JSON.stringify({
                "entry": entryClosing,
                "module": $('#module').val()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                MainAccountingClosing.ShowEntryReport();
            }
        });
        MainAccountingClosing.ShowEntryReport();
    }

    /**
        * Selecciona un proceso de la tabla y habilita los botones.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Información del registro seleccionado.
        * @param {Number} position - Posición del registro seleccionado.
        */
    SelectTableRow(event, data, position) {
        if (data.InProgress == Resources.Finalized) {
            var url = ACL_ROOT + "AccountingClosing/ReportExcel?module=" + $("#module").val();
            $("#excelAccountingClosing").attr("href", url);
            $("#excelAccountingClosing").attr("disabled", false);
            $("#printAccountingClosing").attr("disabled", false);
            $("#processAccountingClosing").attr("disabled", true);
        }
    }

    /**
        * Selecciona un proceso de la tabla y habilita los botones.
        *
        * @param {String} event - Seleccionar todos.
        */
    SelectTableAll(event) {
        $("#excelAccountingClosing").attr("disabled", true);
        $("#printAccountingClosing").attr("disabled", true);

        var data = $("#tblReports").UifDataTable('getData');

        for (var i in data) {
            if (data[i].InProgress == Resources.Finalized) {
                var url = ACL_ROOT + "AccountingClosing/ReportExcel?module=" + $("#module").val();
                $("#excelAccountingClosing").attr("href", url);
                $("#excelAccountingClosing").attr("disabled", false);
                $("#printAccountingClosing").attr("disabled", false);
                $("#processAccountingClosing").attr("disabled", true);
            }
        }
    }

    /**
        * Deselecciona un proceso de la tabla y deshabilita los botones.
        *
        */
    SelectTableTBodyRow() {
    
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#excelAccountingClosing").attr("disabled", true);
            $("#printAccountingClosing").attr("disabled", true);
            $("#accountingAccountingClosing").attr("disabled", true);
            $("#processAccountingClosing").attr("disabled", false);
        }
        else {
            $(this).siblings('.selected').removeClass('selected');
            $(this).addClass('selected');
        
            var data = $("#tblReports").UifDataTable('getData');

            for (var i in data) {
                if (data[i].InProgress == Resources.Finalized) {
                    var url = ACL_ROOT + "AccountingClosing/ReportExcel?module=" + $("#module").val();
                    $("#excelAccountingClosing").attr("href", url);
                    $("#excelAccountingClosing").attr("disabled", false);
                    $("#printAccountingClosing").attr("disabled", false);
                    $("#accountingAccountingClosing").attr("disabled", false);
                    $("#processAccountingClosing").attr("disabled", true);
                }
            }
        }
    }

    /**
        * Deselecciona un proceso de la tabla y deshabilita los botones.
        *
        */
    SelectTableTHeadRow() {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#excelAccountingClosing").attr("disabled", true);
            $("#printAccountingClosing").attr("disabled", true);
            $("#accountingAccountingClosing").attr("disabled", true);
            $("#processAccountingClosing").attr("disabled", false);
        }
        else {
            $(this).siblings('.selected').removeClass('selected');
            $(this).addClass('selected');

            var data = $("#tblReports").UifDataTable('getData');

            for (var i in data) {
                if (data[i].InProgress == Resources.Finalized) {
                    var url = ACL_ROOT + "AccountingClosing/ReportExcel?module=" + $("#module").val();
                    $("#excelAccountingClosing").attr("href", url);
                    $("#excelAccountingClosing").attr("disabled", false);
                    $("#printAccountingClosing").attr("disabled", false);
                    $("#accountingAccountingClosing").attr("disabled", false);
                    $("#processAccountingClosing").attr("disabled", true);
                }
            }
        }
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Visualiza el reporte en formato pdf.
        *
        */
    static ShowReportAccountingClosing() {
        window.open(ACL_ROOT + "AccountingClosing/ShowMonthlyProcessReport", 'Windows',
            'fullscreen=yes, scrollbars=auto');
    }

    /**
        * Visualiza el reporte en formato excel.
        *
        */
    static ShowEntryReport() {
        window.open(ACL_ROOT + "AccountingClosing/ShowMonthlyProcessReport", 'Windows',
            'fullscreen=yes, scrollbars=auto');
    }

    /**
        * Refresca la grilla de proceso.
        *
        */
    static RefreshGridAccountingClosing() {

        var controller = "";
        var result = false;

        if ($("#module").val() != "") {

            controller = ACL_ROOT + "AccountingClosing/GetStatus?module=" + $("#module").val();
            $("#tblReports").UifDataTable({
                source: controller
            });

            setTimeout(function () {
                result = MainAccountingClosing.ValidateProcessIsRunning($('#tblReports').UifDataTable('getData'));
            }, 1500);

            setTimeout(function () {

                if (result == false) {

                    if (!isRunningProcessIss && !isRunningProcessClm && !isRunningProcessReins && !isRunningProcessRsv && !isRunningProcessIBNR) {
                        window.clearInterval(timerAccountingClosing);
                    }
                } else {

                    switch (parseInt($("#module").val())) {

                        case 3:
                            isRunningProcessIss = true;
                            break;
                        case 4:
                            isRunningProcessClm = true;
                            break;
                        case 7:
                            isRunningProcessReins = true;
                            break;

                        case 11:
                            isRunningProcessRsv = true;
                            break;

                        case 14:
                            isRunningProcessIBNR = true;
                            break;

                        default:
                    }
                }


                switch (parseInt($("#module").val())) {

                    case 3:
                        MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessIss);
                        break;
                    case 4:
                        MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessClm);
                        break;
                    case 7:
                        MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessReins);
                        break;
                    case 11:
                        MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessRsv);
                        break;
                    case 14:
                        MainAccountingClosing.SwitchEnabledGenerate(isRunningProcessIBNR);
                        break;

                    default:
                }
            }, 3000);
        }
    }

    /**
        * Habilita/deshabilita botón de Generar.
        *
        * @param {Boolean} status - Estado del proceso.
        */
    static SwitchEnabledGenerate(status) {

        if (status) {
            $('#processAccountingClosing').attr("disabled", true);
        } else {
            $('#processAccountingClosing').attr("disabled", false);
        }
    }

    /**
        * Valida si el proceso está corriendo o no.
        *
        * @param {Number} processTable - Identificador de tabla de proceso.
        */
    static ValidateProcessIsRunning(processTable) {

        if (processTable.length > 0) {

            var state = processTable[0].InProgress;

            if (state == Resources.Finalized || state == "") {


                switch (parseInt($("#module").val())) {

                    case 3:
                        isRunningProcessIss = false;
                        break;
                    case 4:
                        isRunningProcessClm = false;
                        break;
                    case 7:
                        isRunningProcessReins = false;
                        break;

                    case 11:
                        isRunningProcessRsv = false;
                        break;

                    case 14:
                        isRunningProcessIBNR = false;
                        break;

                    default:
                }

                return false;
            } else {

                return true;
            }
        } else {
            return false;
        }
    }

    /**
        * Obtiene los procesos de cierre pendientes del módulo seleccioando.
        *
        * @param {Number} moduleId - Identificador de módulo.
        */
    static GetClosingProcess(moduleId) {
        var controller = ACL_ROOT + "AccountingClosing/GetStatus?module=" + moduleId;
        $("#tblReports").UifDataTable({ source: controller });
    }

    /**
        * Refresca la tabla de cierres masivos.
        *
        */
    static RefreshClosingProcess() {
        var validateProcess = true;
        var process = $('#tblReports').UifDataTable('getData');

        // Se valida que no se esten ejecutando procesos
        if (process.length == 0) {
            MainAccountingClosing.GetClosingProcess($("#module").val());
        }
        else {
            if (MainAccountingClosing.ValidateClosingProcess() == true) {
                window.clearInterval(timerAccountingClosing);
                $('#processAccountingClosing').attr("disabled", false);
            }
            else {
                if (process.length == 1 && process[0].Module == "") {
                    if ($("#module").val() == $("#ViewBagIncomeAndExpensesModule").val()) {
                        $("#excelAccountingClosing").attr("disabled", "disabled");
                        $("#printAccountingClosing").attr("disabled", "disabled");
                        $("#processAccountingClosing").attr("disabled", "disabled");
                        $("#accountingAccountingClosing").attr("disabled", false);
                    }
                    else {
                        $('#processAccountingClosing').attr("disabled", false);
                        window.clearInterval(timerAccountingClosing);
                    }
                }
                else {
                    $('#processAccountingClosing').attr("disabled", true);
                    MainAccountingClosing.GetClosingProcess($("#module").val());
                }
            }
        }
    }

    /**
        * Valida si el proceso ha finalizado.
        *
        */
    static ValidateClosingProcess() {
        var process = $('#tblReports').UifDataTable('getData');
        var count = 0;

        for (var i in process) {
            if (process[i].InProgress == Resources.Finalized) {
                count = 0
            }
            else {
                count++;
                return;
            }
        }

        if (count > 0) {
            return false;
        }
        else {
            return true;
        }
    }

    /**
        * Valida si el proceso ha finalizado.
        *
        * @param {Number} moduleId - Identificador de módulo.
        */
    static ValidateClosureMonth(moduleId) {
        return validateClosureMonthPromise = new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: ACL_ROOT + "AccountingClosing/GetClosureMonth",
                data: { "moduleId": moduleId }
            }).done(function (validationData) {
                resolve(validationData);
                MainAccountingClosing.GetClosingProcess(moduleId)
            });
        });
    }

}
