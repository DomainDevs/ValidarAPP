
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

// Botones de breadcrumb
$("#accontTrack").on('click', function () {
    $('#accountingMenu').find('li').find('a').click();
    $('#accountingMenu').siblings('a').click();
});

$("#reportsTrack").on('click', function () {
    $('#accountingMenu').siblings('a').click();
    $('#reportsMenu').siblings('a').click();
});

window.onload = function () {
    $("#reportsTrack").click();
};

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
        } else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#dailyDateTo").val("");
        }
    }
});

//Controla que la fecha final sea mayor a la inicial
$('#dailyDateFrom').on('datepicker.change', function (event, date) {

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

//Boton para generar Pdf
$("#btnGenerate").click(function () {
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

        loadReportBalance();
    }
});

//Boton para generar Excel
$("#btnExcel").click(function () {
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

        ReportExcel();
    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function showReportBalance() {

    $("#framePdf").attr("src", GL_ROOT + "Reports/ShowReport");
    $.unblockUI();
}

function showReportExcel() {

    $("#framePdf").attr("src", GL_ROOT + "Reports/ReportExcel");
    $.unblockUI();
}

//Summary Entry Report
function loadReportBalance() {
    $.ajax({
        async: false,
        type: "POST",
        url: GL_ROOT + "Reports/LoadBalanceChekingReport",
        data: {
            "dateFrom": $("#dailyDateFrom").val(),
            "dateTo": $("#dailyDateTo").val(),
        },
        success: function (data) {
            showReportBalance();
        }
    });
}

function ReportExcel() {
    $.ajax({
        async: false,
        type: "POST",
        url: GL_ROOT + "Reports/LoadBalanceChekingReportExcel",
        data: {
            "dateFrom": $("#dailyDateFrom").val(),
            "dateTo": $("#dailyDateTo").val(),
        },
        success: function (data) {
            showReportExcel();
        }
    });
}

