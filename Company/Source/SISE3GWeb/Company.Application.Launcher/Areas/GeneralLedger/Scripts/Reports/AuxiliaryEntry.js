/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$(document).ready(function () {

    if ($("#ViewBagOptionId").val() == "AuxiliaryEntry") {

        window.onload = function () {
            $("#reportsTrack").click();
            setTimeout(function () {
                $("#allBranchesAuxiliaryEntry").trigger("click");
            }, 1000);
        };
    }
});

//Valida que no ingresen una fecha invalida.
$("#entryDateFrom").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#entryDateFrom").val() != '') {
        if (IsDate($("#entryDateFrom").val()) == true) {
            if (CompareDates($("#entryDateFrom").val(), getCurrentDate()) == true) {
                $("#entryDateFrom").val(getCurrentDate);
            }
            else {
                if ($("#entryDateTo").val() != "") {

                    if (compare_dates($('#entryDateFrom').val(), $("#entryDateTo").val())) {
                        $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
                        $("#entryDateFrom").val('');
                    }
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#entryDateFrom").val("");
        }
    }
});

//Valida que no ingresen una fecha invalida.
$("#entryDateTo").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#entryDateTo").val() != '') {
        if (IsDate($("#entryDateTo").val()) == true) {

            if (CompareDates($("#entryDateTo").val(), getCurrentDate()) == true) {
                $("#entryDateTo").val(getCurrentDate);
            }
            else {
                if ($("#entryDateFrom").val() != "") {
                    if (compare_dates($("#entryDateFrom").val(), $('#entryDateTo').val())) {
                        $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");
                        $("#entryDateTo").val('');
                    }
                }
            }
        } else {

            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#entryDateTo").val("");
        }
    }
});

//Controla que la fecha final sea mayor a la inicial
$('#entryDateFrom').on('datepicker.change', function (event, date) {
    if ($("#entryDateTo").val() != "") {
        if (compare_dates($('#entryDateFrom').val(), $("#entryDateTo").val())) {

            $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");

            $("#entryDateFrom").val('');
        } else {
            $("#entryDateFrom").val($('#entryDateFrom').val());
            $("#alert").UifAlert('hide');
        }
    }
});

//Controla que la fecha final sea mayor a la inicial
$('#entryDateTo').on('datepicker.change', function (event, date) {

    if ($("#entryDateFrom").val() != "") {
        if (compare_dates($("#entryDateFrom").val(), $('#entryDateTo').val())) {

            $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");

            $("#entryDateTo").val('');
        } else {
            $("#entryDateTo").val($('#entryDateTo').val());
            $("#alert").UifAlert('hide');
        }
    }
});


//sucursales Entry
$("#allBranchesAuxiliaryEntry").on('click', function (event, selectedItem) {

    if ($("#allBranchesAuxiliaryEntry").is(':checked')) {

        $("#ddlEntryBranchesAuxiliaryEntry").prop('disabled', true);
        $("#ddlEntryBranchesAuxiliaryEntry").val(0);
        $("#divEntryBranches").addClass('hidden');
    } else {
        $("#ddlEntryBranchesAuxiliaryEntry").prop('disabled', false);
        $("#ddlEntryBranchesAuxiliaryEntry").val("");

        var controller = GL_ROOT + "Base/GetBranches";
        $("#ddlEntryBranchesAuxiliaryEntry").UifSelect({ source: controller });
        $("#divEntryBranches").removeClass('hidden');
    }
});

//sucursales DailyEntry
$("#allBranchesAuxiliaryEntry").on('click', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
});


//   Botones
$("#entryReport").click(function () {
    $("#alert").UifAlert('hide');
    $('#formData').validate();
    if ($('#formData').valid()) {
        lockScreen();

        //limpia el contenedor
        $("#framePdf").attr("src", null);
        $("#alert").UifAlert('show', Resources.PleaseWaitInfo, "warning");

        setTimeout(function () {
            loadEntryReport();
        }, 100);

    }
});

$("#entryExcel").click(function () {
    $("#alert").UifAlert('hide');
    $('#formData').validate();
    if ($('#formData').valid()) {
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

        //limpia el contenedor
        $("#framePdf").attr("src", null);
        $("#alert").UifAlert('show', Resources.PleaseWaitInfo, "warning");

        setTimeout(function () {
            loadExelAuxiliaryEntry();
        }, 100);
    }
});


/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/



//Entry Report
function loadEntryReport() {
    $.ajax({
        async: true,
        type: "POST",
        url: GL_ROOT + "Reports/LoadEntryReport",
        data: {
            "dateFrom": $("#entryDateFrom").val(),
            "dateTo": $("#entryDateTo").val(),
            "branchId": $("#ddlEntryBranchesAuxiliaryEntry").val() || 0
        },
        success: function (data) {

            if (data.success == false) {
                unlockScreen();
                $("#alert").UifAlert('show', data.result, "danger");
                
            }
            else {

                $("#alert").UifAlert('show', Resources.ReportGenerateSuccess, "success");
                showEntryReportEntry();
            }

        }
    });
}

function showEntryReportEntry() {

    //window.open(GL_ROOT + "Reports/ShowEntryReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
    $("#framePdf").attr("src", GL_ROOT + "Reports/ShowEntryReport");
    //$("#framePdf").attr("height", "300px");
    $.unblockUI();
}

//+++++++++++++++++++++++++Excel+++++++++++++++++++++++++//
//Crea excel con  datos
function loadExelAuxiliaryEntry() {
    $.ajax({
        async: false,
        type: "POST",
        url: GL_ROOT + "Reports/LoadEntryReport",
        data: {
            "dateFrom": $("#entryDateFrom").val(),
            "dateTo": $("#entryDateTo").val(),
            "branchId": $("#ddlEntryBranchesAuxiliaryEntry").val() || 0
        },
        success: function (data) {

            if (data.success == false) {
                unlockScreen();
                $("#alert").UifAlert('show', data.result, "danger");

            }
            else {

                $("#alert").UifAlert('show', Resources.ReportGenerateSuccess, "success");
                showExcelAuxiliaryEntry();
            }

        }
    });
}

//Descarga archivo excel
function showExcelAuxiliaryEntry() {
    var newPage = window.open(GL_ROOT + "Reports/CreateExcelEntry", '_self', 'width=5, height=5, scrollbars=no');
    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
    $.unblockUI();
}

