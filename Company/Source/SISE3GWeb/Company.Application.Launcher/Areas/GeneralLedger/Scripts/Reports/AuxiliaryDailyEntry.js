/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
$(document).ready(function () {

    if ($("#ViewBagOptionId").val() == "AuxiliaryDailyEntry") {
        window.onload = function () {
            $("#reportsTrack").click();
            setTimeout(function () {
                $("#allBranches").trigger("click");
            }, 1000);
        };
    }
});


//Valida que no ingresen una fecha invalida.
$("#dailyDateFrom").blur(function () {

    $("#alert").UifAlert('hide');
    if ($("#dailyDateFrom").val() != '') {

        if (IsDate($("#dailyDateFrom").val()) == true) {

            if (CompareDates($("#dailyDateFrom").val(), getCurrentDate()) == true) {
                $("#dailyDateFrom").val(getCurrentDate);
            }
            else {
                if ($("#dailyDateTo").val() != "") {
                    if (compare_dates($('#dailyDateFrom').val(), $("#dailyDateTo").val())) {
                        $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
                        $("#dailyDateFrom").val('');
                    }
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#dailyDateFrom").val("");
        }
    }
});

//Valida que no ingresen una fecha invalida.
$("#dailyDateTo").blur(function () {

    $("#alert").UifAlert('hide');
    if ($("#dailyDateTo").val() != '') {

        if (IsDate($("#dailyDateTo").val()) == true) {

            if (CompareDates($("#dailyDateTo").val(), getCurrentDate()) == true) {
                $("#dailyDateTo").val(getCurrentDate);
            }
            else {
                if ($("#dailyDateFrom").val() != "") {
                    if (compare_dates($("#dailyDateFrom").val(), $('#dailyDateTo').val())) {

                        $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");

                        $("#dailyDateTo").val('');
                    }
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#dailyDateTo").val("");
        }
    }
});


//Controla que la fecha final sea mayor a la inicial
$('#dailyDateFrom').on('datepicker.change', function (event, date) {

    $("#alert").UifAlert('hide');
    if ($("#dailyDateTo").val() != "") {

        if (compare_dates($('#dailyDateFrom').val(), $("#dailyDateTo").val())) {

            $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");

            $("#dailyDateFrom").val('');
        } else {
            $("#dailyDateFrom").val($('#dailyDateFrom').val());
            $("#alert").UifAlert('hide');
        }
    }
});


//Controla que la fecha final sea mayor a la inicial
$('#dailyDateTo').on('datepicker.change', function (event, date) {

    $("#alert").UifAlert('hide');
    if ($("#dailyDateFrom").val() != "") {
        if (compare_dates($("#dailyDateFrom").val(), $('#dailyDateTo').val())) {

            $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");

            $("#dailyDateTo").val('');
        } else {
            $("#dailyDateTo").val($('#dailyDateTo').val());
            $("#alert").UifAlert('hide');
        }
    }
});



//sucursales DailyEntry
$("#allBranches").on('click', function (event, selectedItem) {

    $("#alert").UifAlert('hide');
    if ($("#allBranches").is(':checked')) {

        $("#ddlBranchesAuxiliaryDailyEntry").prop('disabled', true);
        $("#ddlBranchesAuxiliaryDailyEntry").val(0);
        $("#divBranches").addClass('hidden');
    } else {
        $("#ddlBranchesAuxiliaryDailyEntry").prop('disabled', false);
        $("#ddlBranchesAuxiliaryDailyEntry").val("");

        var controller = GL_ROOT + "Base/GetBranches";
        $("#ddlBranchesAuxiliaryDailyEntry").UifSelect({ source: controller });
        $("#divBranches").removeClass('hidden');
    }
});

//sucursales DailyEntry
$("#ddlBranchesAuxiliaryDailyEntry").on('click', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
});


//
//*********** -   Botones   -  ***********//
$("#daylyEntryReport").click(function () {
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
        //$("#daylyEntryReport").attr("disabled", true);
        //$("#daylyEntryExcel").attr("disabled", true);

        setTimeout(function () {
            loadReportAuxiliaryDailyEntry();
        }, 100);
    }
});

$("#daylyEntryExcel").click(function () {
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
            loadExel();
        }, 100);

    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

//+++++++++++++++++++++++++DailyEntry Report+++++++++++++++++++++++++//

function loadReportAuxiliaryDailyEntry() {
    var branchId = 0;
    if ($("#ddlBranchesAuxiliaryDailyEntry").val() != null) {
        branchId = $("#ddlBranchesAuxiliaryDailyEntry").val();
    }
    $.ajax({
        async: true,
        type: "POST",
        url: GL_ROOT + "Reports/LoadDailyEntryReport",
        data: { "branchId": branchId, "dateFrom": $("#dailyDateFrom").val(), "dateTo": $("#dailyDateTo").val() },
        success: function (data) {

            $("#alert").UifAlert('show', Resources.ReportGenerateSuccess, "success");
            showReportAuxiliaryDailyEntry();

        }
    });
}

function showReportAuxiliaryDailyEntry() {

    //window.open(GL_ROOT + "Reports/ShowDailyEntryReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');

    $("#framePdf").attr("src", GL_ROOT + "Reports/ShowDailyEntryReport");
    //$("#framePdf").attr("height", "300px");
    $.unblockUI();
}

//+++++++++++++++++++++++++Excel+++++++++++++++++++++++++//
//Crea excel con  datos
function loadExel() {
    $.ajax({
        async: false,
        type: "POST",
        url: GL_ROOT + "Reports/LoadDailyEntryReport",
        data: {
            "dateFrom": $("#dailyDateFrom").val(),
            "dateTo": $("#dailyDateTo").val(),
            "branchId": $("#ddlBranchesAuxiliaryDailyEntry").val() || 0
        },
        success: function (data) {

            $("#alert").UifAlert('show', Resources.ReportGenerateSuccess, "success");
            showExcel();
        }
    });
}

//Descarga archivo excel
function showExcel() {

    var newPage = window.open(GL_ROOT + "Reports/CreateExcelDailyEntry", '_self', 'width=5, height=5, scrollbars=no');
    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
    $.unblockUI();
}

