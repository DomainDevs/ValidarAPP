
/*-------------------------------------------------------------------------------------------------------------------------------------------*/
/*														DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                              */
/*-------------------------------------------------------------------------------------------------------------------------------------------*/

var BranchUserDefault = $("#ViewBagBranchUserDefaultPayment").val();

var paymentRow = null;
var SearchBranchId = 0;
var PaymentOrdersId = 0;

var oTransferPaymentOrderModel = {
    Id: 0,
    DeliveryDate: null,
    StatusId: 0,
    UserId: 0,
    PaymentOrdersItems: []
};

var oPaymentOrderTransferModel = {
    PaymentOrderId: 0,
    AccountBankId: 0,
    BankId: 0,
    AccountBankNumber: ""
};

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
/*																  ACCIONES / EVENTOS				 												*/
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

// Oculta alerts ante perdida/ganacia de foco
$("#BranchPaymentOrderSelect").on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
});

//////////////////////////////////////////////////////
//Botones                                           //
//////////////////////////////////////////////////////
/**
**CleanPaymentOrder
** botón que encera los campos
*/
$("#CancelPaymentOrderAuthorization").click(function () {
    $("#alert").UifAlert('hide');
    setSearchPaymentOrdersAuthorizationDataFieldsEmpty();
});

/**
** SearchPaymentOrderProcess
** botón ejecuta proceso de búsqueda
*/
$("#SearchPaymentOrderAutorization").click(function () {
    $("#alert").UifAlert('hide');

    $("#PaymentOrdersAuthorization").validate();
    if ($("#PaymentOrdersAuthorization").valid()) {
        $("#SearchPaymentOrdersAutorizationTable").UifDataTable();

        SearchBranchId = $("#BranchPaymentOrderAuthorization").val();
        TypePaymentMethod = $("#PaymentMethodAuthorization").val();
        PaymentOrderEstimatedDateAuthorization = $("#PaymentOrderEstimatedDateAuthorization").val();

        var controller = ACC_ROOT + "PaymentOrders/GetPaymentOrderAuthorization?branchId=" + SearchBranchId +
            "&paymentMethodId=" + TypePaymentMethod +
            "&estimatedPaymentDate=" + PaymentOrderEstimatedDateAuthorization;

        $("#SearchPaymentOrdersAutorizationTable").UifDataTable({ source: controller });


        $("#AutorizePaymentOrderAuthorization").show();

    }
});

$('#SearchPaymentOrdersAutorizationTable').on('rowSelected', function (event, selectedRow) {
    paymentRow = selectedRow;
    PaymentOrdersId = selectedRow.PaymentOrderCode;
});

/////////////////////////////////////////
//  Botón  Autorizar                   //
/////////////////////////////////////////

$("#AutorizePaymentOrderAuthorization").click(function () {
    $("#alert").UifAlert('hide');
    $("#PaymentOrdersAuthorization").validate();
    if ($("#PaymentOrdersAuthorization").valid()) {
        var rowid = $("#SearchPaymentOrdersAutorizationTable").UifDataTable("getSelected");
        if (rowid != null) {
  
         showConfirmAuthorization();  
                       
        } else {
            $("#alert").UifAlert('show', Resources.WarningSelectOneRecord, "warning");
        }
    }
});

$("#PaymentOrderEstimatedDateAuthorization").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#PaymentOrderEstimatedDateAuthorization").val() != '') {
        if (!IsDate($("#PaymentOrderEstimatedDateAuthorization").val())) {
            $("#alert").UifAlert('show', Resources.InvalidDates, "warning");
            $("#PaymentOrderEstimatedDateAuthorization").val("");
        }
    }
});
/*------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*															DEFINICIÓN DE FUNCIONES GLOBALES															*/
/*------------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#AutorizePaymentOrderAuthorization").hide();

///////////////////////////////////////////////////
//  Limpia campos del formulario                //
//////////////////////////////////////////////////
function setSearchPaymentOrdersAuthorizationDataFieldsEmpty() {
    $("#BranchPaymentOrderAuthorization").val("");
    $("#PaymentMethodAuthorization").val("");
    $("#PaymentOrderEstimatedDateAuthorization").val("");
    $("#SearchPaymentOrdersAutorizationTable").dataTable().fnClearTable();
}

//////////////////////////////////////////////////
//  Arma Objeto   oAuthorizationPaymentOrderModel   //
//////////////////////////////////////////////////
function SetDataAuthorization() {
    oTransferPaymentOrderModel.Id = 0; //autonumerico
    oTransferPaymentOrderModel.UserId = 0;
    oTransferPaymentOrderModel.StatusId = 0;
    oTransferPaymentOrderModel.PaymentOrdersItems = [];

    var rowid = $("#SearchPaymentOrdersAutorizationTable").UifDataTable("getSelected");
    if (rowid != null) {
        for (var i in rowid) {
            oPaymentOrderTransferModel = {
                PaymentOrderId: 0,
                UserId: 0,
                BankId: 0,
                AccountBankNumber: null
            };

            oPaymentOrderTransferModel.PaymentOrderId = rowid[i].PaymentOrderCode;            
            oTransferPaymentOrderModel.PaymentOrdersItems.push(oPaymentOrderTransferModel);
        }
    }
    return oTransferPaymentOrderModel;
}

///////////////////////////////////////////////////////
//  Limpia Objeto   oAuthorizationPaymentOrderModel //
/////////////////////////////////////////////////////
function SetDataAuthorizationEmpty() {
    oTransferPaymentOrderModel = {
        Id: 0,
        AccountBankId: 0,
        DeliveryDate: null,
        CancellationDate: null,
        StatusId: 0,
        UserId: 0,
        PaymentOrdersItems: []
    };

    oPaymentOrderTransferModel = {
        PaymentOrderId: 0,
        AccountBankId: 0,
        BankId: 0,
        AccountBankNumber: ""
    };
}

function showConfirmAuthorization() {
    $.UifDialog('confirm', { 'message': Resources.AutorizerPaymentOrder, 'title': Resources.Authorize },
        function (result) {

        if (result) {
            lockScreen();

            setTimeout(function () {

                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "PaymentOrders/SavePaymentOrderAuthorization",
                    data: JSON.stringify({ "transferPaymentOrderModel": SetDataAuthorization() }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        //EXCEPCION ROLLBACK
                        if (data.success == true) {
                            $("#alert").UifAlert('show', Resources.SuccessfulAuthorization, 'success');
                            refreshPaymentOrdersAuthorization();                        
                        }
                        else {
                        
                            $("#alert").UifAlert('show', data.result, 'danger');
                        }

                        unlockScreen();
                        SetDataAuthorizationEmpty();
                 
                    }
                });
            }, 1000);       
        }        
    });
}



function refreshPaymentOrdersAuthorization() {
    
    $("#PaymentOrdersAuthorization").validate();
    if ($("#PaymentOrdersAuthorization").valid()) {
        $("#SearchPaymentOrdersAutorizationTable").UifDataTable();
        if ($("#BranchPaymentOrderAuthorization").val() != "") {
            SearchBranchId = $("#BranchPaymentOrderAuthorization").val();
        }
        else {
            SearchBranchId = ""; //-3;
        }

        if ($("#PaymentMethodAuthorization").val() != "") {
            TypePaymentMethod = $("#PaymentMethodAuthorization").val();
        }
        else {
            TypePaymentMethod = ""; //-1;
        }

        if ($("#PaymentMethodAuthorization").val() != null) {
            PaymentOrderEstimatedDateAuthorization = $("#PaymentOrderEstimatedDateAuthorization").val();
        }
        else {
            PaymentOrderEstimatedDateAuthorization = ""; //*;
        }

        var controller = ACC_ROOT + "PaymentOrders/GetPaymentOrderAuthorization?branchId=" + SearchBranchId +
            "&paymentMethodId=" + TypePaymentMethod +
            "&estimatedPaymentDate=" + PaymentOrderEstimatedDateAuthorization;

        $("#SearchPaymentOrdersAutorizationTable").UifDataTable({ source: controller });

    }
}

