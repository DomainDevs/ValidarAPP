/**
 * @file   MainGenerationReclassification.js
 * @author Desarrollador
 * @version 0.1
 */

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var windowObjectReference = null; // global variable
var timerReclassification;

var counterPromise;
/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  DEFINICIÓN DE CLASE                                                     */
/*--------------------------------------------------------------------------------------------------------------------------*/

$(() => {
    new MainGenerationReclassification();
});



class MainGenerationReclassification extends Uif2.Page {
    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        var controller = GL_ROOT + "AccountReclassification/GetMonths";
        $("#GenerationReclassificationMonth").UifSelect({ source: controller });
    }

    /**
       * Enlaza los eventos de los componentes de la pantalla.
       *
       */
    bindEvents() {
        $("#GenerationReclassificationYear").on("blur", this.ValidateYear);
        $("#CancelGeneration").on("click", this.CleanFieldsGenerationReclassification);
        $("#GenerateReclassification").on("click", this.GenerateReclassification);
        $("#GenerationReclassificationMonth").on('binded', this.BindedMonthDefault);
        $('#GenerationReclassificationMonth').on('itemSelected', this.ItemSelectedMonth);
        $("#PrintReclassification").on("click", this.PrintReclassification);
        $("#ExcelReclassification").on("click", this.ExcelReclassification);
        $("#PrintEntryReclassification").on("click", this.PrintEntryReclassification);
        $("#ExcelEntryReclassification").on("click", this.ExcelEntryReclassification);

        $("#AccountingReclassification").on("click", this.AccountingReclassification);
        $("#modalPrint").find("#btnPrintModal").on('click', this.PrintAccountingReclassification);
    }

    /**
        * Valida que el año sea mayor a 1900.
        *
        */
    ValidateYear() {
        $("#alert").UifAlert('hide');
        $("#GenerateReclassification").attr("disabled", true);
        $("#PrintReclassification").attr("disabled", true);
        $("#ExcelReclassification").attr("disabled", true);
        $("#PrintEntryReclassification").attr("disabled", true);
        $("#ExcelEntryReclassification").attr("disabled", true);
        $("#AccountingReclassification").attr("disabled", true);

        if ($("#GenerationReclassificationYear").val() != "") {
            if (parseInt($("#GenerationReclassificationYear").val()) < 1900) {
                $("#alert").UifAlert('show', Resources.MessageYear, "warning");
                $("#GenerationReclassificationYear").val("");
            }
            else {
                MainGenerationReclassification.RefreshGridReclassification();

                setTimeout(function () {
                    MainGenerationReclassification.ValidateReclassificationProcess();
                }, 2000);
            }
        }
    }

    /**
        * Limpia los campos modificables para la generación de reclasificación.
        *
        */
    CleanFieldsGenerationReclassification() {
        $("#alert").UifAlert('hide');
        $("#GenerationReclassificationYear").val("");
        $("#GenerationReclassificationMonth").val("");
    }

    /**
        * Genera la reclasificación contable.
        *
        */
    GenerateReclassification() {
        $("#generationReclassificationForm").validate();

        if ($("#generationReclassificationForm").valid()) {
            MainGenerationReclassification.GenerateAccountingReclassification($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val());
        }
    }

    /**
        * Setea el mes por default una vez que esta cargado.
        *
        */
    BindedMonthDefault() {
        $("#GenerationReclassificationMonth").val($("#ViewBagMonthDefault").val());
        MainGenerationReclassification.RefreshGridReclassification();
    }

    /**
        * Obtiene el proceso de reclasificación contable por mes y año.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del mes seleccionado.
        */
    ItemSelectedMonth(event, selectedItem) {
        $("#alert").UifAlert('hide');
        $("#GenerateReclassification").attr("disabled", true);
        $("#PrintReclassification").attr("disabled", true);
        $("#ExcelReclassification").attr("disabled", true);
        $("#PrintEntryReclassification").attr("disabled", true);
        $("#ExcelEntryReclassification").attr("disabled", true);
        $("#AccountingReclassification").attr("disabled", true);

        if (selectedItem.Id > 0) {
            MainGenerationReclassification.RefreshGridReclassification();

            setTimeout(function () {
                MainGenerationReclassification.ValidateReclassificationProcess();
            }, 2000);
        }
    }

    /**
        * Exporta a pdf el listado de registro del proceso.
        *
        */
    PrintReclassification() {
        $("#alert").UifAlert('hide');
        $("#generationReclassificationForm").validate();
                
        MainGenerationReclassification.AccountingReclassificationCount($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val()).then(function (counter) {
            if (counter > 0) {
                if ($("#generationReclassificationForm").valid()) {
                    MainGenerationReclassification.GeneratePDFReclassification($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val());
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.ReclassificationNoData, "warning");
            }
        });
    }

    /**
        * Exporta a excel el listado registro de proceso.
        *
        */
    ExcelReclassification() {
        $("#alert").UifAlert('hide');
        $("#generationReclassificationForm").validate();

        if ($("#generationReclassificationForm").valid()) {

            MainGenerationReclassification.AccountingReclassificationCount($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val()).then(function (counter) {
                if (counter > 0) {
                    MainGenerationReclassification.GenerateExcelReclassification($("#GenerationReclassificationMonth").val(),
                        $("#GenerationReclassificationYear").val(), $("#GenerationReclassificationMonth option:selected").text());
                    $("#AccountingReclassification").attr("disabled", false);
                }
                else {
                    $("#alert").UifAlert('show', Resources.NoRecordsToContinue, "warning");
                }
            });
        }
    }

    /**
        * Exporta a pdf el listado de asiento del proceso.
        *
        */
    PrintEntryReclassification() {
        $("#alert").UifAlert('hide');
        $("#generationReclassificationForm").validate();
        MainGenerationReclassification.AccountingReclassificationCount($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val()).then(function (counter) {
            if (counter > 0) {
                if ($("#generationReclassificationForm").valid()) {
                    MainGenerationReclassification.GeneratePDFEntryReclassification($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val());
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.NoRecordsToContinue, "warning");
            }
        });
    }

    /**
        * Exporta a excel el listado de asiento de proceso.
        *
        */
    ExcelEntryReclassification() {
        $("#alert").UifAlert('hide');
        $("#generationReclassificationForm").validate();

        if ($("#generationReclassificationForm").valid()) {
            MainGenerationReclassification.AccountingReclassificationCount($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val()).then(function (counter) {
                if (counter > 0) {
                    MainGenerationReclassification.GenerateExcelEntryReclassification($("#GenerationReclassificationMonth").val(),
                        $("#GenerationReclassificationYear").val(), $("#GenerationReclassificationMonth option:selected").text());
                    $("#AccountingReclassification").attr("disabled", false);
                }
                else {
                    $("#alert").UifAlert('show', Resources.NoRecordsToContinue, "warning");
                }
            });
        }
    }

    /**
        * Contabiliza la reclasificación de cuentas.
        *
        */
    AccountingReclassification() {
        $("#alert").UifAlert('hide');
        $("#generationReclassificationForm").validate();

        if ($("#generationReclassificationForm").valid()) {

            $("#GenerateReclassification").attr("disabled", "disabled");
            $("#PrintReclassification").attr("disabled", "disabled");
            $("#ExcelReclassification").attr("disabled", "disabled");
            $("#PrintEntryReclassification").attr("disabled", "disabled");
            $("#ExcelEntryReclassification").attr("disabled", "disabled");
            $("#AccountingReclassification").attr("disabled", "disabled");

            $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AccountReclassification/AccountingReclassificationClosure",
                data: JSON.stringify({ "month": $("#GenerationReclassificationMonth").val(), "year": $("#GenerationReclassificationYear").val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        var entryNumber = data.result;
                        if (entryNumber.length > 0) {
                            $("#alert").UifAlert('show', Resources.MessageReclassifiationEntry + ': ' + data.result, "success");
                            $("#GenerateReclassification").attr("disabled", false);
                            $("#tblReports").UifDataTable('clear');
                            MainGenerationReclassification.RefreshGridReclassification();
                        }
                        else if (entryNumber == "") {
                            $("#alert").UifAlert('show', Resources.MessageNotDataAnnualClosure, "warning");
                            $("#GenerateReclassification").attr("disabled", false);
                        }
                        else {
                            $("#alert").UifAlert('show', Resources.ProcessExecutionError, "danger");
                            $("#GenerateReclassification").attr("disabled", false);
                        }
                    }
                    else {
                        $("#alert").UifAlert('show', Resources.MessageInternalError + ' ' + Resources.Details + ': ' + data.result, "danger");
                        //$("#GenerateReclassification").attr("disabled", false);
                    }
                }
            });
        }
    }

    /**
        * Imprime la contabilización de la reclasificación de cuentas.
        *
        */
    PrintAccountingReclassification() {
        $("#generationReclassificationForm").validate();

        if ($("#generationReclassificationForm").valid()) {
            $("#alert").UifAlert('hide');
            $('#modalPrint').modal('hide');

            MainGenerationReclassification.GeneratePDFEntryReclassification($("#GenerationReclassificationMonth").val(), $("#GenerationReclassificationYear").val());
        }
    }


    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICION DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Genera la reclasificación contable por mes y año.
        *
        * @param {Number} month - Mes de reclasificación.
        * @param {Number} year  - Año de reclasificación.
        */
    static GenerateAccountingReclassification(month, year) {
        $("#GenerateReclassification").attr("disabled", "disabled");

        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        timerReclassification = window.setInterval(MainGenerationReclassification.RefreshGridReclassification, 2000);

        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountReclassification/GenerateAccountingReclassification",
            data: JSON.stringify({ "month": month, "year": year }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    if (data.result > 0) {

                        $("#alert").UifAlert('show', Resources.MessageFinalizedProcess, "success");

                        $("#PrintReclassification").attr("disabled", false);
                        $("#ExcelReclassification").attr("disabled", false);
                        $("#PrintEntryReclassification").attr("disabled", false);
                        $("#ExcelEntryReclassification").attr("disabled", false);
                    }
                    else {
                        $("#alert").UifAlert('show', Resources.NoDataProcess, "warning");
                        $("#tblReports").UifDataTable({ source: null });
                        $("#GenerateReclassification").attr("disabled", false);
                    }
                }
                else {
                    $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger", data.result);
                    $("#tblReports").UifDataTable({ source: null });
                    $("#GenerateReclassification").removeAttr("disabled");
                }
                setTimeout(function () {
                    clearInterval(timerReclassification);
                }, 3000);
            }
        });
    }

    /**
        * Exporta a pdf el listado de registro del proceso de reclasificación contable.
        *
        * @param {Number} month     - Mes de reclasificación.
        * @param {Number} year      - Año de reclasificación.
        */
    static GeneratePDFReclassification(month, year) {
        $.msg({
            content: Resources.MessagePleaseWait, klass: 'white-on-black'
        });

        MainGenerationReclassification.ShowReportProcessLogList(month, year, $("#GenerationReclassificationMonth option:selected").text());
        $("#AccountingReclassification").attr("disabled", false);

        $.msg('unblock');
    }

    /**
    * Visualiza el reporte en una pestaña del browser.
    *
    * @param {Number} month     - Mes de reclasificación.
    * @param {Number} year      - Año de reclasificación.
    * @param {String} monthName - Nombre mes de reclasificación.
    */
    static ShowReportProcessLogList(month, year, monthName) {
        if (windowObjectReference == null || windowObjectReference.closed) {

            window.open(GL_ROOT + "AccountReclassification/ShowReclassificationReport?month="
                            +month + "&year=" +year + "&monthName=" +monthName, 'mywindow', 'fullscreen=yes, scrollbars=auto');

        }
    }

    /**
        * Exporta a excel el listado de registro del proceso de reclasificación contable.
        *
        * @param {Number} month     - Mes de reclasificación.
        * @param {Number} year      - Año de reclasificación.
        * @param {String} monthName - Nombre mes de reclasificación.
        */
    static GenerateExcelReclassification(month, year, monthName) {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountReclassification/GenerateReclassificationToExcel",
            data: { "month": month, "year": year, "monthName": monthName },
        }).done(function (data) {
            // Get the file name for download
            if (data.fileName != "") {
                // Use window.location.href for redirect to download action for download the file
                window.location.href = GL_ROOT + "AccountReclassification/Download/?file=" + data.fileName;
                $("#AccountingReclassification").attr("disabled", false);
            }
            $.msg('unblock');
        });
    }

    /**
        * Exporta a pdf el listado de asiento del proceso de reclasificación contable.
        *
        * @param {Number} month - Mes de reclasificación.
        * @param {Number} year  - Año de reclasificación.
        */
    static GeneratePDFEntryReclassification(month, year) {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        MainGenerationReclassification.ShowReportProcessEntryList(month, year, $("#GenerationReclassificationMonth option:selected").text());
        $("#AccountingReclassification").attr("disabled", false);

        $.msg('unblock');
    }

    /**
        * Exporta a excel el listado de asiento del proceso de reclasificación contable.
        *
        * @param {Number} month     - Mes de reclasificación.
        * @param {Number} year      - Año de reclasificación.
        * @param {String} monthName - Nombre mes de reclasificación.
        */
    static GenerateExcelEntryReclassification(month, year, monthName) {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountReclassification/GenerateReclassificationEntryToExcel",
            data: { "month": month, "year": year, "monthName": monthName },
        }).done(function (data) {
            // Get the file name for download
            if (data.fileName != "") {
                // Use window.location.href for redirect to download action for download the file
                window.location.href = GL_ROOT + "AccountReclassification/Download/?file=" + data.fileName;
                $("#AccountingReclassification").attr("disabled", false);
            }
            $.msg('unblock');
        });
    }

    /**
        * Visualiza el reporte en una pestaña del browser.
        *
        * @param {Number} month     - Mes de reclasificación.
        * @param {Number} year      - Año de reclasificación.
        * @param {String} monthName - Nombre mes de reclasificación.
        */
    static ShowReportProcessEntryList(month, year, monthName) {
        if (windowObjectReference == null || windowObjectReference.closed) {
            window.open(GL_ROOT + "AccountReclassification/ShowReclassificationEntryReport?month="
                           + month + "&year=" + year + "&monthName=" + monthName, 'mywindow', 'fullscreen=yes, scrollbars=auto');
        }
    }

    /**
        * Refresca la tabla de proceso.
        *
        */
    static RefreshGridReclassification() {
        var controller = GL_ROOT + "AccountReclassification/GetReclassificationStatus?month=" +
                         $("#GenerationReclassificationMonth").val() + "&year=" + $("#GenerationReclassificationYear").val();
        $("#tblReports").UifDataTable({ source: controller });
    }

    /**
        * Valida el estado del proceso de proceso.
        *
        */
    static ValidateReclassificationProcess() {
        var process = $('#tblReports').UifDataTable('getData');
        var count = 0;

        if (process.length > 0) {
            for (var p in process) {
                if (process[p].InProgress == Resources.Finalized) {
                    count = 0;
                }
                else {
                    count++;
                    //return;
                }
            }
        }
        else {
            count = -1;
        }


        if (count > 0) {
            $("#GenerateReclassification").attr("disabled", true);
        }
        if (count == 0) {
            $("#PrintReclassification").attr("disabled", false);
            $("#ExcelReclassification").attr("disabled", false);
            $("#PrintEntryReclassification").attr("disabled", false);
            $("#ExcelEntryReclassification").attr("disabled", false);
            $("#AccountingReclassification").attr("disabled", false);
            $("#GenerateReclassification").attr("disabled", true);
        }
        else if (count < 0) {
            $("#GenerateReclassification").attr("disabled", false);
            $("#PrintReclassification").attr("disabled", true);
            $("#ExcelReclassification").attr("disabled", true);
            $("#PrintEntryReclassification").attr("disabled", true);
            $("#ExcelEntryReclassification").attr("disabled", true);
            $("#AccountingReclassification").attr("disabled", true);
        }

    }

    /**
        * Validacion de cantidad de registrod e proceso
        *
        * @param {Number} month     - Mes de reclasificación.
        * @param {Number} year      - Año de reclasificación.
        */
    static AccountingReclassificationCount(month, year) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: GL_ROOT + "AccountReclassification/AccountinReclassificationCount",
                data: JSON.stringify({ "month": month, "year": year }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    resolve(data);
                }
            });
        });
    }
}