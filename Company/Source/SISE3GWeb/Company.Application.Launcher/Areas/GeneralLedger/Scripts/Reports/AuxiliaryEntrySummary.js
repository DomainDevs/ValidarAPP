
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$(document).ready(function () {

    if ($("#ViewBagOptionId").val() == "AuxiliaryEntrySummary") {

        window.onload = function () {
            $("#reportsTrack").click();
            setTimeout(function () {
                $("#allBranchesSummary").trigger("click");
            }, 1200);
        };
    };
});

//Valida que no ingresen una fecha invalida.
$("#summaryEntryDateFrom").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#summaryEntryDateFrom").val() != '') {
        if (IsDate($("#summaryEntryDateFrom").val()) == true) {
            if (CompareDates($("#summaryEntryDateFrom").val(), getCurrentDate()) == true) {
                $("#summaryEntryDateFrom").val(getCurrentDate);
            }
            else {
                if ($("#summaryEntryDateTo").val() != "") {
                    if (compare_dates($('#summaryEntryDateFrom').val(), $("#summaryEntryDateTo").val())) {
                        $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
                        $("#summaryEntryDateFrom").val('');
                    }
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#summaryEntryDateFrom").val("");
        }
    }
});

//Valida que no ingresen una fecha invalida.
$("#summaryEntryDateTo").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#summaryEntryDateTo").val() != '') {
        if (IsDate($("#summaryEntryDateTo").val()) == true) {
            if (CompareDates($("#summaryEntryDateTo").val(), getCurrentDate()) == true) {
                $("#summaryEntryDateTo").val(getCurrentDate);
            }
            else {
                if ($("#summaryEntryDateFrom").val() != "") {
                    if (compare_dates($("#summaryEntryDateFrom").val(), $('#summaryEntryDateTo').val())) {
                        $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");
                        $("#summaryEntryDateTo").val('');
                    }
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#summaryEntryDateTo").val("");
        }
    }
});

//Controla que la fecha final sea mayor a la inicial
$('#summaryEntryDateFrom').on('datepicker.change', function (event, date) {
    if ($("#summaryEntryDateTo").val() != "") {
        if (compare_dates($('#summaryEntryDateFrom').val(), $("#summaryEntryDateTo").val())) {
            $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#summaryEntryDateFrom").val('');
        } else {
            $("#summaryEntryDateFrom").val($('#summaryEntryDateFrom').val());
            $("#alert").UifAlert('hide');
        }
    }
});

//Controla que la fecha final sea mayor a la inicial
$('#summaryEntryDateTo').on('datepicker.change', function (event, date) {
    if ($("#summaryEntryDateFrom").val() != "") {
        if (compare_dates($("#summaryEntryDateFrom").val(), $('#summaryEntryDateTo').val())) {
            $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#summaryEntryDateTo").val('');
        } else {
            $("#summaryEntryDateTo").val($('#summaryEntryDateTo').val());
            $("#alert").UifAlert('hide');
        }
    }
});

//sucursales SummaryEntryBranches
$("#allBranchesSummary").on('click', function (event, selectedItem) {

    if ($("#allBranchesSummary").is(':checked')) {
        $("#ddlSummaryEntryBranches").prop('disabled', true);
        $("#ddlSummaryEntryBranches").val(0);
        $("#divEntryBranches").addClass('hidden');

    } else {
        $("#ddlSummaryEntryBranches").prop('disabled', false);
        $("#ddlSummaryEntryBranches").val("");

        var controller = GL_ROOT + "Base/GetBranches";
        $("#ddlSummaryEntryBranches").UifSelect({ source: controller });
        $("#divEntryBranches").removeClass('hidden');

    }
});

//sucursales DailyEntry
$("#allBranchesSummary").on('click', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
});

$("#summaryEntryReport").click(function () {
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
            loadSummaryEntryReport();
        }, 100);

    }

});

$("#summaryEntryExcel").click(function () {
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
            loadExelSummary();
        }, 100);

    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

//Summary Entry Report
function loadSummaryEntryReport() {
    $.ajax({
        async: false,
        type: "POST",
        url: GL_ROOT + "Reports/LoadSummaryEntryReport",
        data: {
            "dateFrom": $("#summaryEntryDateFrom").val(),
            "dateTo": $("#summaryEntryDateTo").val(),
            "branchId": $("#ddlSummaryEntryBranches").val() || 0
        },
        success: function (data) {
            $("#alert").UifAlert('show', Resources.ReportGenerateSuccess, "success");
            showSummaryEntryReport();
        }
    });
}

function showSummaryEntryReport() {
    $("#framePdf").attr("src", GL_ROOT + "Reports/ShowSummaryEntryReport");
    //$("#framePdf").attr("height", "300px");
    $.unblockUI();
}

//+++++++++++++++++++++++++Excel+++++++++++++++++++++++++//
//Crea excel con  datos
function loadExelSummary() {
    $.ajax({
        async: false,
        type: "POST",
        url: GL_ROOT + "Reports/LoadSummaryEntryReport",
        data: {
            "dateFrom": $("#summaryEntryDateFrom").val(),
            "dateTo": $("#summaryEntryDateTo").val(),
            "branchId": $("#ddlSummaryEntryBranches").val() || 0
        },
        success: function (data) {
            $("#alert").UifAlert('show', Resources.ReportGenerateSuccess, "success");
            showExcelSummary();
        }
    });
}

//Descarga archivo excel
function showExcelSummary() {
    var newPage = window.open(GL_ROOT + "Reports/CreateExcelSummaryEntry", '_self', 'width=5, height=5, scrollbars=no');
    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
    $.unblockUI();
}

