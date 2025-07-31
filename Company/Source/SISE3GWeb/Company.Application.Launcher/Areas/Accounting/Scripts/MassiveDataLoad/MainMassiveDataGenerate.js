var date;
var dateMassiveDataGeneratePromise;
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                          ACCIONES / EVENTOS                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#ExportExcelPoliciesButton").hide();
getDateMassiveDataGenerate();
dateMassiveDataGeneratePromise.then(function (dateMassiveDataGenerate) {
    $("#DataGenerateDate").val(dateMassiveDataGenerate);
});

$("#DataSearch").click(function () {
    if ($("#DataGenerateForm").valid()) {

        var control = ACC_ROOT + "MassiveDataLoad/MassiveDataForGenerate?branchId=" + $('#DataGenerateBranch').val()
                               + "&issuedate=" + $("#DataGenerateDate").val();

        $("#MassiveDataTable").UifDataTable({ source: control });
        $("#ExportExcelPoliciesButton").show();
    } else {
        return false;
    }
});

$("#DataCancel").click(function () {
    setDataFieldsEmptyDataGenerated();
});


$("#DataGenerateBranch").on('itemSelected', function (event, selectedItem) {
    if ($('#DataGenerateBranch').val() != "") {
        setDataEmptyDataGenerated();
    }
    else {
        setDataFieldsEmptyDataGenerated();
    }
});

$('#DataGenerateDate').on('datepicker.change', function (event, date) {

    var systemDate;
    getDateMassiveDataGenerate();
    dateMassiveDataGeneratePromise.then(function (dateMassiveDataGenerate) {
        systemDate = dateMassiveDataGenerate;

        date = $("#DataGenerateDate").val();

        if (IsDate($("#DataGenerateDate").val()) == true) {
            if ($("#DataGenerateDate").val() != "__/__/____") {

                if (CompareDates(date, systemDate) == true) {

                    $("#alertDataGeneration").UifAlert('show', Resources.SystemdateValidation, "warning");

                    $("#DataGenerateDate").val(systemDate);
                } else {
                    $("#DataGenerateDate").val(date);
                }
            }
        }
        else {
            $("#alertDataGeneration").UifAlert('show', Resources.InvalidDates, "danger");
            $("#DataGenerateDate").val("");
        }
    });
});


$("#ExportExcelPoliciesButton").on("click", function () {

    var ids = $("#MassiveDataTable").UifDataTable("getData");
    if (ids.length == 0) {

        $("#alertDataGeneration").UifAlert('show', Resources.WrongNoDataFound, "warning");

    } else {
        exportToExcel();
    }
});

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/


function exportToExcel() {
    var url = ACC_ROOT;
    url = url + "MassiveDataLoad/MassiveDataForGenerateExcel?branchId=" + $('#DataGenerateBranch').val() + "&issuedate=" + $("#DataGenerateDate").val(); //id;
    var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
}

function setDataFieldsEmptyDataGenerated() {

    $("#DataGenerateBranch").val("");
    $("#DataGenerateDate").val("");
    $("#ExportExcelPoliciesButton").hide();
    $("#MassiveDataTable").dataTable().fnClearTable();
    $("#alertDataGeneration").UifAlert('hide');
}

function setDataEmptyDataGenerated() {
    $("#MassiveDataTable").dataTable().fnClearTable();
    $("#ExportExcelPoliciesButton").hide();
}

function getDateMassiveDataGenerate() {

    return dateMassiveDataGeneratePromise = new Promise(function (resolve, reject) {

            if ($("#ViewBagImputationType").val() == undefined &&
                $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
                $("#ViewBagBillControlId").val() == undefined) {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "Billing/GetDate"
                    }).done(function(dateMassiveDataGenerate) {
                        resolve(dateMassiveDataGenerate);
                    });
                }
        });
}