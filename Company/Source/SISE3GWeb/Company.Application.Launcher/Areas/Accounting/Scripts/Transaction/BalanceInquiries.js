/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                          ACCIONES / EVENTOS                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var systemDate = "";
var balanceCurrentDatePromise;

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchSearchCashMovements").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchSearchCashMovements").removeAttr("disabled");
}

//setTimeout(function () {
//    GetUserByBranch();
//}, 1000);

//setea la fecha del sistema

getBalanceCurrentDate();
$("#CurrentDateSearchCashMovements").val(GetCurrentDate());

balanceCurrentDatePromise.then(function (balanceCurrentDate) {
    $("#CurrentDateSearchCashMovements").val(new Date());
    //$("#CurrentDateSearchCashMovements").val(balanceCurrentDate);
});



//Valida que no ingresen una fecha invalida.
$("#CurrentDateSearchCashMovements").blur(function () {

    $("#alert").UifAlert('hide');
    getBalanceCurrentDate();
    balanceCurrentDatePromise.then(function (balanceCurrentDate) {
        systemDate = balanceCurrentDate;

        if ($("#CurrentDateSearchCashMovements").val() != '') {
            if (IsDate($("#CurrentDateSearchCashMovements").val()) == true) {
                var date = $("#CurrentDateSearchCashMovements").val();
                if (validateBalanceDate(date, systemDate)) {
                    $("#alert").UifAlert('show', Resources.SystemdateValidation, "warning");
                    $("#CurrentDateSearchCashMovements").val(systemDate);
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.InvalidDates, "warning");
                $("#CurrentDateSearchCashMovements").val("");
            }
        }
    });

});



$('#BranchSearchCashMovements').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Transaction/GetUserInquiries?branchId=" + selectedItem.Id;
        $("#CashierSearchCashMovements").UifSelect({ source: controller });
        setDataEmpty();
    }
    //else {
    //    setDataFieldsEmpty();
    //}
});



$("#SearchCashMovements").click(function () {    
    
    
    if ($("#CurrentDateSearchCashMovements").UifDatepicker('getValue') > new Date()) {
        $("#alert").UifAlert('show', Resources.DateInvalidBalanceInquiries, "warning");
    }
    else
    {
        $("#BalanceInquiriesForm").validate();
        if ($("#BalanceInquiriesForm").valid()) {
            if ($('#CashierSearchCashMovements').val() != null) {
                var controller = ACC_ROOT + "Transaction/GetBalanceInquiries?branch=" + $('#BranchSearchCashMovements').val() +
                    "&user=" + $('#CashierSearchCashMovements').val() + "&date=" + $('#CurrentDateSearchCashMovements').val();
                $("#BalanceInquieries").UifDataTable({ source: controller });
                /*
                setTimeout(function () {
                    var total = totalCalculation();
                    $("#totalInquiries").text("$ " + NumberFormatDecimal(total, "2", ".", ","));
                }, 500);
                */
            }
        }
    }
    
});

$("#CleanCashMovements").click(function () {
    $("#BalanceInquieries").dataTable().fnClearTable();
    $("#BranchSearchCashMovements").val('');
    $("#CashierSearchCashMovements").val('');
    $("#CurrentDateSearchCashMovements").val('');
    $("#totalInquiries").text('');
    $("#alert").UifAlert('hide');
});


/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/


function setDataEmpty() {
    $("#CashierSearchCashMovements").val("");
    $("#BalanceInquieries").dataTable().fnClearTable();
}


function getBalanceCurrentDate() {

    return balanceCurrentDatePromise = new Promise(function (resolve, reject) {

        if ($("#ViewBagImputationType").val() == undefined &&
            $("#ViewBagParameterMulticompanyPayment").val() == undefined && 
            $("#ViewBagBillControlId")==undefined) {

               $.ajax({
                   type: "GET",
                   url: ACC_ROOT + "Common/GetDate",
                   dataType: "json",
                   contentType: "application/json; charset=utf-8",
                   success: function (balanceCurrentDate) {
                       resolve(balanceCurrentDate);
                   }
               });
        }

     }); 
}

function validateBalanceDate(startDate, endDate) {
    var xMonth = startDate.substring(3, 5);
    var xDay = startDate.substring(0, 2);
    var xYear = startDate.substring(6, 10);
    var yMonth = endDate.substring(3, 5);
    var yDay = endDate.substring(0, 2);
    var yYear = endDate.substring(6, 10);

    if (xYear > yYear) {
        return (true);
    }
    else {
        if (xYear == yYear) {
            if (xMonth > yMonth) {
                return (true);
            }
            if (xMonth == yMonth) {
                if (xDay > yDay) {
                    return (true);
                }
                else {
                    return (false);
                }
            }
            else {
                return (false);
            }
        }
        else {
            return (false);
        }
    }
}

function totalCalculation() {
    var total = 0;
    var balanceInquieries = $('#BalanceInquieries').UifDataTable('getData');

    if (balanceInquieries != null) {
        for (var i in balanceInquieries) {
            total += parseFloat(balanceInquieries[i].Amount.replace("$", "").replace(",", ""));
        }
    }
    return total;
}


