/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var entryNumberClosing = 0;
var yearClosing = 0;
var closingTypeId = 0;
var time;

/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//FORMATOS BUTTONS /FECHAS/ #CARACTERES / NÚMEROS-DECIMALES
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

$("#YearClosing").on('keypress', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
window.onload = function () {
    $("#printPanel").hide();
    $("#progressBarPanel").hide();
    $("#printClosing").attr("disabled", true);
    $("#monthPanel").hide();
    CheckDate();
};

//click en el botón de búsqueda
$("#processClosing").on('click', function (event) {
    $("#alertClosing").UifAlert('hide');

    $("#closingForm").validate();
    if ($("#closingForm").valid()) {

        var closingYear = $("#YearClosing").val();
        var closingTypeId = $("#ClosingTypeId").val();
        var entryMonth = 0;

        if ($("#entryMonth").val() == "") {
            entryMonth = 0;
        } else {
            entryMonth = $("#entryMonth").val();
        }

        //barrita de proceso
        $("#progressBarPanel").show();

        //compruebo si el año está cerrado
        CheckClosedModule(closingYear).then(function (checkData) {
            if (checkData.result) {
                $("#alertClosing").UifAlert('hide');
                ExecuteClosing(closingTypeId, closingYear, entryMonth).then(function (closingData) {
                    if (closingData.success) {
                        if (closingData.result > 0) {
                            $("#alertClosing").UifAlert('show', Resources.ProcessSuccessfullyExecuted + ', ' + Resources.EntrySuccessfullySaved + ' ' + closingData.result, "success");
                            $("#progressBarPanel").hide();
                        } else {
                            $("#alertClosing").UifAlert('show', Resources.ProcessExecutionError, "warning");
                            $("#progressBarPanel").hide();
                        }
                    } else {
                        $("#alertClosing").UifAlert('show', Resources.ProcessExecutionError, "warning");
                        $("#progressBarPanel").hide();
                    }
                });
            } else {
                $("#alertClosing").UifAlert('show', Resources.Month_Not_Closed_Warning, "warning");
                $("#progressBarPanel").hide();
            }
        });
    }
});

$("#ClosingTypeId").on('itemSelected', function (event, selectedItem) {
    $("#printClosing").attr("disabled", true);
});

//click en el botón de limpiar
$("#clearClosing").on('click', function (event) {
    clearFieldsClosing();
});

$("#printClosing").on('click', function () {
    if (entryNumberClosing > -1) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/GenerateTaxYearEntryReport",
            data: JSON.stringify({
                "closingTypeId": $("#ClosingTypeId").val(),
                "entryNumber": entryNumberClosing,
                "year": yearClosing
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                showReportClosing();
                clearFields();
            }
        });
        showReportClosing();
    }
});

//Evento de Ocultar y mostrar el campo de mes
$("#ClosingTypeId").on('itemSelected', function (event, selectedItem) {
    switch ($("#ClosingTypeId").val()) {
        case "1":
            //Cierre de Ingresos y Egresos
            $("#monthPanel").hide();
            break;
        case "2":
            //Cierre de Utilidad Mensual
            $("#monthPanel").show();
            break;
        case "3":
            //Asiento de Apertura de Activos y Pasivos
            $("#monthPanel").hide();
            break;
        case "4":
            //Reversar Asiento Anual de Apertura
            $("#monthPanel").hide();
            break;
        case "5":
            //Reversar Cierre Anual de Ingresos y Egresos
            $("#monthPanel").hide();
            break;
        case "":
            //Reversar Cierre Anual de Ingresos y Egresos
            $("#monthPanel").hide();
            break;
    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function showReportClosing() {
    window.open(ACL_ROOT + "AccountingClosing/ShowMonthlyProcessReport", 'Windows',
        'fullscreen=yes, scrollbars=auto');
}

function clearFieldsClosing() {
    //campos de búsqueda
    $("#ClosingTypeId").val("");
    //reinicia el formulario de búsqueda
    $("#closingForm").formReset();
    //oculta mensajes de advertencia
    $("#alertClosing").UifAlert('hide');
    getActualDate();
    $("#printPanel").hide();
    entryNumberClosing = 0;
    //oculta dropdown de meses
    $("#monthPanel").hide();
}

//obtiene la fecha actual desde el servidor

function getActualDate() {
    if ($("#ViewBagImputationType").val() == undefined && $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId").val() == undefined) {
        $.ajax({
            url: ACL_ROOT + "Base/GetDate",
            success: function (data) {
                $("#YearClosing").val(data.substring(6, 10));
            }
        });
    }
}

function CheckDate() {
    time = setInterval(function () {
        if ($("#YearClosing").val() == "") {
            getActualDate();
        } else {
            clearTimeout(time);
        }
    }, 300);
}

function CheckClosedModule(year) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/CheckClosedModules",
            data: JSON.stringify({
                "year": year
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (checkData) {
                resolve(checkData);
            }
        });
    });
}

function ExecuteClosing(closingTypeId, year, month) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/ExecuteClosing",
            data: JSON.stringify({
                "closingTypeId": closingTypeId,
                "year": year,
                "month": month
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (closingData) {
                resolve(closingData);
            }
        });
    });
}