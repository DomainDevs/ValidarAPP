/**
    * @file   Posting.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var currentMonth;
var currentDay;
var currentYear;
var modalChoose;
var validateClosurePromise;
var saveProcessRequestPromise;
var validateClosureMonthPromise;

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainPosting();
});

class MainPosting extends Uif2.Page {
    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        MainPosting.GetPostingCurrentDate();
        MainPosting.LoadProcessDate(Resources.BalanceClousingModuleDateId);
        $("#progressBarPanel").hide();

        $('#processMonth').attr("disabled", "disabled");
        $('#processDay').attr("disabled", "disabled");

        setTimeout(function () {
            $('#processMonth').attr("disabled", "disabled");
        }, 700);
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#process").on('click', this.ProcessMajorization);
        $("#closureModalAccept").on('click', this.SaveAutomaticLedgerEntry);
        $("#clear").on('click', this.CleanParameters);
        $("#module").on("itemSelected", this.SelectModuleClosingDate);
    }

    /**
        * Valida la fecha de ciere del módulo seleccioando antes de procesar la mayorización.
        *
        */
    ProcessMajorization() {
        $("#postingForm").validate();
        if ($("#postingForm").valid()) {
            MainPosting.ValidateClosureDate();
            validateClosurePromise.then(function (result) {
                if (result) {
                    $('#closureModal').UifModal('showLocal', Resources.Warning);
                }
                else {
                    MainPosting.SaveProcess(0);
                }
            });
        }
    }

    /**
        * Procesa la mayorización.
        *
        */
    SaveAutomaticLedgerEntry() {
        MainPosting.SaveProcess(1);
        $('#closureModal').UifModal('hide');
    }

    /**
        * Limpia los parámetros de generación de la mayorización.
        *
        */
    CleanParameters() {
        //MainPosting.LoadProcessDate(Resources.BalanceClousingModuleDateId);
        $("#progressBarPanel").hide();
        $("#alert").UifAlert('hide');
        $('#processMonth').val("");
        $('#processYear').val("");
        $('#processDay').val("");
        $('#module').val("");
    }

    /**
        * Obtiene la fecha del módulo seleccionado.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del módulo seleccionado.
        */
    SelectModuleClosingDate(event, selectedItem) {
        if (selectedItem.Id > 0 && selectedItem.Id != null) {
            $("#alert").UifAlert('hide');
            $("#process").attr("disabled", false);

            MainPosting.ValidateClosureMonth(selectedItem.Id);
            validateClosureMonthPromise.then(function (validationData) {
                var controller = ACL_ROOT + "Base/GetModuleDays?moduleId=" + selectedItem.Id;
                $("#processDay").UifSelect({ source: controller, selectedId: String(validationData.day).trim() });

                $('#processMonth').val(validationData.month);
                $('#processYear').val(validationData.year);
                setTimeout(function () {
                    $('#processDay').attr("disabled", "disabled");
                }, 1000);
            });
        }
        else {
            $('#processDay').val("");
            $('#processMonth').val("");
            $('#processYear').val("");
        }
    }


    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Ejecuta el cierre de mes que realiza validaciones y envia los datos.
        *
        */
    static SaveProcess(isClosure) {
        $("#alert").UifAlert('hide');
        $("#progressBarPanel").show();

        $("#postingForm").validate();
        if ($("#postingForm").valid()) {

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

            var month = $("#processMonth").val();
            var year = $("#processYear").val();

            MainPosting.SaveProcessRequest(year, month, isClosure);
            saveProcessRequestPromise.then(function (data) {
                if (data.success) {
                    if (data.result > 0) {
                        $("#alert").UifAlert('show', Resources.EntrySuccessfullySaved + " " + data.result, "success");
                        MainPosting.LoadProcessDate(Resources.BalanceClousingModuleDateId);
                        $("#progressBarPanel").hide();
                        MainPosting.CleanFilters();
                        $("#module").trigger('change');
                    } else {
                        $("#alert").UifAlert('show', Resources.PostingProcessNoRecords, "danger");
                        $("#progressBarPanel").hide();
                    }
                } else {
                    $("#alert").UifAlert('show', Resources.PostingProcessError, "danger");
                    $("#progressBarPanel").hide();
                }
                $.unblockUI();
            });
        }
    }

    /**
        * Obtiene la fecha que aún no fue procesada en combos.
        *
        * @param {Number} moduleDateId - identificador de módulo.
        */
    static LoadProcessDate(moduleDateId) {

        var processDatePromise = new Promise(function (resolve, reject) {
            $.ajax({
                url: GL_ROOT + "Process/LoadProcessDate",
                data: { "moduleDateId": moduleDateId },
                success: function (data) {
                    resolve(data);
                }
            });
        });

        processDatePromise.then(function (data) {
            var dateString = data.result.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = day + "/" + month + "/" + year;

            $("#processYear").val(year);
            $("#processMonth").val(month);
            $("#processMonth").attr('disabled', true);
        });
    }

    /**
        * Obtiene la fecha actual desde el servidor.
        *
        */
    static GetPostingCurrentDate() {
        $.ajax({
            url: GL_ROOT + "Base/GetDate",
            success: function (data) {
                currentMonth = data.split("/")[1];
                currentDay = data.split("/")[0];
                currentYear = data.split("/")[2];
            }
        });
    }

    /**
        * Valida la fecha de cierre.
        *
        */
    static ValidateClosureDate() {
        return validateClosurePromise = new Promise(function (resolve, reject) {
            $.ajax({
                url: GL_ROOT + "Process/ValidateClosureDate",
                data: { "year": $("#processYear").val(), "month": $("#processMonth").val() },
                success: function (result) {
                    resolve(result);
                }
            });
        });
    }

    /**
        * Ejecuta la mayorización de asientos de diario.
        *
        * @param {Number} processYear      - Año de proceso.
        * @param {Number} processMonth     - Mes de proceso.
        * @param {Number} processIsClosure - Si el cierre es mensaul o diario.
        */
    static SaveProcessRequest(processYear, processMonth, processIsClosure) {
        return saveProcessRequestPromise = new Promise(function (resolve, reject) {
            $.ajax({
                url: GL_ROOT + "Process/SaveAutomaticLedgerEntry",
                data: {
                    "moduleId": $("#module").val(),
                    "moduleDescription": $("#module option:selected").text(),
                    "year": processYear,
                    "month": processMonth,
                    "day": $("#processDay").val(),
                    "isClosure": processIsClosure
                },
                success: function (data) {
                    resolve(data);
                }
            });
        });
    }

    /**
        * Valida la fecha de cierre del módulo seleccioando.
        *
        * @param {Number} moduleId - Identificador de módulo.
        */
    static ValidateClosureMonth(moduleId) {
        return validateClosureMonthPromise = new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: GL_ROOT + "Base/GetClosureMonth",
                data: { "moduleId": moduleId }
            }).done(function (validationData) {
                resolve(validationData);
                //MainPosting.GetClosingProcess(moduleId)
            });
        });
    }

    /**
        * Limpia los parámetros de generación de la mayorización.
        *
        */
    static CleanFilters() {
        $("#progressBarPanel").hide();
        $("#alert").UifAlert('hide');
        $('#processMonth').val("");
        $('#processYear').val("");
        $('#processDay').val("");
        $('#module').val("");
    }
}
