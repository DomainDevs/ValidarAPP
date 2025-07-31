
/*-------------------------------------------------------------------------------------------------------------------------------------------*/
/*														DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                              */
/*-------------------------------------------------------------------------------------------------------------------------------------------*/

var IsMulticompany = $("#ViewBagParameterMulticompanyPayment").val();
var AccountingCompanyDefault = $("#ViewBagAccountingCompanyDefaultPayment").val();
var BranchUserDefault = $("#ViewBagBranchUserDefaultPayment").val();
var SalePointBranchUserDefault = $("#ViewBagSalePointBranchUserDefaultPayment").val();
var beneficiaryIndividualId = 0;
var paymentRow = null;
var checkNumber = null;
var setMainTotalControl = 0;

var SearchBranchId = 0;
var SearchBeneficiaryId = 0;
var PaymentOrdersUserId = 0;
var PaymentOrdersId = 0;
var PaymentOrdersUserNick = "";
var Alert = 0;
var Numerrors = 0;
var searchTempImputationId = 0;
var amount = 0;
var IsMainSearchBills = 0;
var IsMainSearchPreliquidations = 1;
var IsBilling = 0;

var idControl = "";


var oRePrintPaymentOrdersMovementsModel = {
    Id: 0,
    PaymentOrderNumber: null,
    BranchId: 0,
    BranchName: null,
    PaymentBranchId: 0,
    PaymentBranchName: null,
    CompanyId: 0,
    CompanyName: null,
    EstimatedPaymentDate: null,
    PaymentDate: null,
    UserId: 0,
    UserName: null,
    PaymentTypeId: 0,
    PaymentTypeName: null,
    CurrencyId: 0,
    CurrencyName: null,
    PaymentIncomeAmount: 0,
    PaymentAmount: 0,
    BeneficiaryDocNumber: null,
    BeneficiaryName: null,
    PayToName: null,
    MovementSummaryItems: []
};


var oRePrintMovementSummaryModel = {
    DescriptionMovementSumary: null,
    Debit: 0,
    Credit: 0
};

var oPaymentOrder = {
    PaymentOrderItemId: 0,
    AccountingDate: null,
    AccountBankId: 0,
    BranchId: 0,
    BranchPayId: 0,
    CompanyId: 0,
    EstimatedPaymentDate: null,
    PaymentDate: null,
    PaymentMethodId: 0,
    PaymentSourceId: 0,
    IndividualId: 0,
    PersonTypeId: 0,
    CurrencyId: 0,
    ExchangeRate: 0,
    PaymentIncomeAmount: 0,
    PaymentAmount: 0,
    PayTo: null,
    StatusId: 0
};


$("#ExportToExcel").hide();

if ($("#ViewBagBranchDisablePayment").val() == "1") {
    setTimeout(function () {
        $("#BranchPaymentOrderSelect").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchPaymentOrderSelect").removeAttr("disabled");
}

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
/*										FORMATOS BOTONES /FECHAS/ #CARACTERES / NÚMEROS-DECIMALES / ACORDEONES										*/
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

//Fechas
$("#SearchPaymentOrderStartDate").mask("99/99/9999");
$("#SearchPaymentOrderEndDate").mask("99/99/9999");

//Control número máximo de caracteres
$("#SearchPayerDocumentNumber").attr("maxlength", 20);
$("#SearchPayerNamePaymentOrder").attr("maxlength", 60);
$("#UserPaymentOrders").attr("maxlength", 20);

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
/*																  ACCIONES / EVENTOS				 												*/
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

setAccountingCompanyPaymentOrders();

//Se ocultan campos de busqueda de autocompletes
$(window).load(function () {
    cleanAutocompletesPaymentOrder('SearchSuppliers');
    cleanAutocompletesPaymentOrder('SearchInsured');
    cleanAutocompletesPaymentOrder('SearchCoinsurance');
    cleanAutocompletesPaymentOrder('SearchPerson');
    cleanAutocompletesPaymentOrder('SearchAgent');
    cleanAutocompletesPaymentOrder('SearchEmployee');
    cleanAutocompletesPaymentOrder('SearchReinsurer');

    //Cargando los eventos
    loadAutocompleteEventPaymentOrder('SearchSuppliers');
    loadAutocompleteEventPaymentOrder('SearchInsured');
    loadAutocompleteEventPaymentOrder('SearchCoinsurance');
    loadAutocompleteEventPaymentOrder('SearchPerson');
    loadAutocompleteEventPaymentOrder('SearchAgent');
    loadAutocompleteEventPaymentOrder('SearchEmployee');
    loadAutocompleteEventPaymentOrder('SearchReinsurer');
});

hideButtons();

//Controla que la fecha final sea mayor a la inicial
$("#SearchPaymentOrderStartDate").on("datepicker.change", function (event, date) {
    $("#alert").UifAlert('hide');
    if ($("#SearchPaymentOrderEndDate").val() != "") {

        if (compare_dates($("#SearchPaymentOrderStartDate").val(), $("#SearchPaymentOrderEndDate").val())) {
            $("#alert").UifAlert('show', Resources.ValidateDateTo, 'warning');
            $("#SearchPaymentOrderStartDate").val('');
        }
    }
});


//Controla que la fecha final sea mayor a la inicial
$("#SearchPaymentOrderStartDate").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#SearchPaymentOrderStartDate").val() != '') {

        if (IsDate($("#SearchPaymentOrderStartDate").val()) == true) {
            if ($("#SearchPaymentOrderEndDate").val() != '') {
                if (CompareDates($("#SearchPaymentOrderStartDate").val(), $("#SearchPaymentOrderEndDate").val())) {
                    $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
                    $("#SearchPaymentOrderStartDate").val("");
                    return true;
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessFrom, "warning");
            $("#SearchPaymentOrderStartDate").val(date);
        }
    }
});

//Controla que la fecha final sea mayor a la inicial
$("#SearchPaymentOrderEndDate").on("datepicker.change", function (event, date) {
    $("#alert").UifAlert('hide');
    if ($("#SearchPaymentOrderStartDate").val() != "") {
        if (compare_dates($("#SearchPaymentOrderStartDate").val(), $("#SearchPaymentOrderEndDate").val())) {
            $("#alert").UifAlert('show', Resources.ValidateDateFrom, 'warning');
            $("#SearchPaymentOrderEndDate").val('');
        }
    }
});


//Control de fecha final mayor a la inicial
$("#SearchPaymentOrderEndDate").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#SearchPaymentOrderEndDate").val() != '') {

        if (IsDate($("#SearchPaymentOrderEndDate").val()) == true) {
            if ($("#SearchPaymentOrderStartDate").val() != '') {
                if (CompareDates($("#SearchPaymentOrderStartDate").val(), $("#SearchPaymentOrderEndDate").val())) {
                    $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");
                    $("#SearchPaymentOrderEndDate").val("");
                    return true;
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessTo, "warning");
            $("#SearchPaymentOrderEndDate").val("");
        }
    }
});

// Oculta alerts ante perdida/ganacia de foco
$("#BranchPaymentOrderSelect").on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');    
    $("#SearchPaymentOrdersTable").dataTable().fnClearTable();
});


$("#SearchPaymentOrderNumber").on('keypress', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
});


$("#SearchPaymentMeansSelect").on('keypress', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
});


/////////////////////////////////////
// Autocomplete nombre             //
/////////////////////////////////////
$('#inputName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();

    payerId = selectedItem.IndividualId;
    if (payerId > 0) {
        $("#inputName").val(selectedItem.Name);
        $("#inputDocumentNumber").val(selectedItem.DocumentNumber);
    }
    else {
        CleanAllFields();
    }
});

/////////////////////////////////////
// Dropdown tipo de pagador        //
/////////////////////////////////////
$("#SearchPayerTypeSelect").on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    var secondParameter = 0;
    var enableAjax = 0;
    var aditionalParameter;
    var secondAditionalParameter;
    var aditionalUrl;

    cleanAutocompletesPaymentOrder('SearchSuppliers');
    cleanAutocompletesPaymentOrder('SearchInsured');
    cleanAutocompletesPaymentOrder('SearchCoinsurance');
    cleanAutocompletesPaymentOrder('SearchPerson');
    cleanAutocompletesPaymentOrder('SearchAgent');
    cleanAutocompletesPaymentOrder('SearchEmployee');
    cleanAutocompletesPaymentOrder('SearchReinsurer');


    if (selectedItem != null) {
        $("#SearchPayerDocumentNumber").hide();
        $("#SearchPayerNamePaymentOrder").hide();

        if (selectedItem.Id == $("#ViewBagSupplierCodePayment").val()) { // 1 Proveedor  // 10 en BE

            $('#SearchSuppliersByDocumentNumber').parent().parent().show();
            $("#SearchSuppliersByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagInsuredCodePayment").val()) { // 2 Asegurado  //7 en BE  ya esta hecho

            $("#SearchInsuredByDocumentNumber").parent().parent().show();
            enableAjax = 1;

            $("#SearchInsuredByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagCoinsurerCodePayment").val()) { // 7 Coaseguradora  //Compañia 2 en BE

            $("#SearchCoinsuranceByDocumentNumber").parent().parent().show();
            $("#SearchCoinsuranceByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagThirdPartyCodePayment").val()) { //8 Tercero   // 8 en BE

            $("#SearchPersonByDocumentNumber").parent().parent().show();
            $("#SearchPersonByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagProducerCodePayment").val()) { // 10 Productor  // agente 1 en BE

            $("#SearchAgentByDocumentNumber").parent().parent().show();
            $("#SearchAgentByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagEmployeeCodePayment").val()) { // 11 Empleado  // 11 en BE

            $("#SearchEmployeeByDocumentNumber").parent().parent().show();
            enableAjax = 1;


            $("#SearchEmployeeByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagReinsurerCodePayment").val()) { //13 Reaseguradora  //Compañia 2 en BE

            $("#SearchReinsurerByDocumentNumber").parent().parent().show();
            $("#SearchReinsurerByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagTradeConsultantPayment").val()) { // 14 Asesor comercial   // 8 en BE

            $("#SearchPersonByDocumentNumber").parent().parent().show();
            $("#SearchPersonByName").parent().parent().show();

        } else if (selectedItem.Id == $("#ViewBagAgentCodePayment").val()) { //15 Agente   //1 en BE

            $("#SearchAgentByDocumentNumber").parent().parent().show();
            $("#SearchAgentByName").parent().parent().show();

        } else if (selectedItem.Id == '') {

            cleanAutocompletesPaymentOrder('SearchSuppliers');
            cleanAutocompletesPaymentOrder('SearchInsured');
            cleanAutocompletesPaymentOrder('SearchCoinsurance');
            cleanAutocompletesPaymentOrder('SearchPerson');
            cleanAutocompletesPaymentOrder('SearchAgent');
            cleanAutocompletesPaymentOrder('SearchEmployee');
            cleanAutocompletesPaymentOrder('SearchReinsurer');

        } else {

            $("#SearchPersonByDocumentNumber").parent().parent().show();
            $("#SearchPersonByName").parent().parent().show();
        }

        if (enableAjax > 0) {
        }
    } else {
        cleanAutocompletesPaymentOrder('SearchSuppliers');
        cleanAutocompletesPaymentOrder('SearchInsured');
        cleanAutocompletesPaymentOrder('SearchCoinsurance');
        cleanAutocompletesPaymentOrder('SearchPerson');
        cleanAutocompletesPaymentOrder('SearchAgent');
        cleanAutocompletesPaymentOrder('SearchEmployee');
        cleanAutocompletesPaymentOrder('SearchReinsurer');
    }

});

$('#inputSupplierName').on('itemSelected', function (event, selectedItem) {
    $("#alert").hide();
    CleanAllFields();
    selectedInsured = selectedItem;
    payerId = selectedItem.IndividualId;
    if (payerId > 0) {
        payerId = selectedItem.IndividualId;
        $("#inputSupplierName").val(selectedItem.Name);
        $("#inputSupplierDocumentNumber").val(selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
        $('#inputSupplierDocumentNumber').val("");
        $('#inputSupplierName').val("");
        selectedInsured = undefined;
        personDocumentNumber = "";
        personName = "";

    }
});

/**
**UserPaymentOrders
** autocomplete ordenes de pago
*/
$('#UserPaymentOrders').on('itemSelected', function (event, selectedItem) {
    fillPaymentAutocomplete(selectedItem);
});

/**
**UserPaymentOrders
** autocomplete ordenes de pago
*/
$('#SearchPaymentOrdersTable').on('rowSelected', function (event, selectedRow) {

  
    paymentRow = selectedRow;
    searchTempImputationId = selectedRow.TempImputationCode;
    PaymentOrdersId = selectedRow.PaymentOrderCode;
    
    var statusId = selectedRow.Status;  

    $("#alert").UifAlert('hide');

    if (statusId >= 1)    {

        //Las OPs que fueron generadas por la liberación automática de comisiones
        if (selectedRow.PaymentSourceCode == parseInt($("#ViewBagParamConceptSourceId").val())) {

            $("#ModifyPaymentOrder").hide();
            $("#CancelPaymentOrder").hide();
            $("#PrintPaymentOrder").hide();

            $("#alert").UifAlert('show', Resources.MessageConfirmProcessTitle + ' ' + Resources.PaymentOrder + ' ' + Resources.Source + ': ' + Resources.AgentCheckingAccountLabel , 'warning');

        } else {          
            
            /*Las Ordenes de Pago que no tengan asignado un número de cheque son las que se puden editar */            
            if (statusId == 1 && (selectedRow.CheckNumber == "" || selectedRow.BankAccountNumberPerson == "" )) { //Solo se puede modificar las activas
                $("#ModifyPaymentOrder").show();
                $("#CancelPaymentOrder").show();
            }
            else {
                $("#ModifyPaymentOrder").hide();
                $("#CancelPaymentOrder").hide();
            }                        
            $("#PrintPaymentOrder").show();        }
    }
    else {
        hideButtons();
    }
});

$('#SearchPaymentOrdersTable').on('rowDeselected', function (event, data, position) {
    PaymentOrdersUserId = "-1";
    PaymentOrdersId = "";
    SearchBeneficiaryId = 0;
    hideButtons();
});

//////////////////////////////////////////////////////
//Botones                                           //
//////////////////////////////////////////////////////
/**
**CleanPaymentOrder
** botón que encera los campos
*/
$("#CleanPaymentOrder").click(function () {
    $("#alert").UifAlert('hide');
    setSearchPaymentOrdersDataFieldsEmpty();
});

/**
** SearchPaymentOrderProcess
** botón ejecuta proceso de búsqueda
*/
$("#SearchPaymentOrderProcess").click(function () {

    $("#alert").UifAlert('hide');

    $("#SearchPaymentOrdersTable").dataTable().fnClearTable();
    $("#PaymentOrdersForm").validate();
    Numerrors = 0;

    if (Numerrors == 0 || Numerrors == undefined) {
        Alert = 0;
    } else {
        Alert = 1;
        Numerrors = 0;
    }

    if (Alert == 0 && $("#PaymentOrdersForm").valid()) {
        SearchBranchId = $("#BranchPaymentOrderSelect").val();
        if ($("#UserPaymentOrders").val() != "") {
            PaymentOrdersUserId = $("#_UserIdPaymentOrders").val();            
        }
        else {
            PaymentOrdersUserId = ""; //-1;
        }
        PaymentOrdersId = $("#SearchPaymentOrderNumber").val();
        SearchBeneficiaryId = beneficiaryIndividualId;

        var controller = ACC_ROOT + "PaymentOrders/GetSearchPaymentOrders?branchId=" + parseInt(SearchBranchId) +
            "&userId=" + PaymentOrdersUserId + "&paymentMethodId=" + $("#SearchPaymentMeansSelect").val() +
            "&startDate=" + $("#SearchPaymentOrderStartDate").val() + "&endDate=" +
            $("#SearchPaymentOrderEndDate").val() + "&paymentOrderNumber=" + PaymentOrdersId +
            "&personTypeId=" + $("#SearchPayerTypeSelect").val() + "&beneficiaryIndividualId=" + SearchBeneficiaryId +
            "&beneficiaryDocumentNumber=" + $("#SearchPayerDocumentNumber").val() + "&beneficiaryName=" +
            $("#SearchPayerNamePaymentOrder").val() +
            '&status=' + ($("#PaymentOrderStatus").val() == "" ? -1 : $("#PaymentOrderStatus").val()) +
            '&IsDelivered=' + ($("#SearchDeliveredSelect").val() == "" ? null : ($("#SearchDeliveredSelect").val() == "1" ? true : false)) +
            '&IsReconciled=' + ($("#SearchReconcileSelect").val() == "" ? null : ($("#SearchReconcileSelect").val() == "1" ? true : false)) +
            '&IsAccounting=' + ($("#SearchAccountingSelect").val() == "" ? null : ($("#SearchAccountingSelect").val() == "1" ? true : false));

        $("#SearchPaymentOrdersTable").UifDataTable({ source: controller });

        $("#AutorizePaymentOrder").hide();
        $("#ExportToExcel").show();
    }
});

/**
** ModifyPaymentOrder
** botón de modificación
*/
$("#ModifyPaymentOrder").click(function () {   

    if ($("#SearchPaymentOrdersTable").UifDataTable("getSelected") == null) {

        $("#alert").UifAlert('show', Resources.WarningSelectOneRecord, 'warning');

        return;

    } else {        
    
        searchTempImputationId = paymentRow.TempImputationCode;
        $("#dlgPaymentOrdersApplication").attr("title", Resources.TemporalPaymentOrder + " " + searchTempImputationId);

        lockScreen();
        setTimeout(function () {

            location.href = $("#ViewBagLoadPaymentOrdersApplicationLink").val() + "?paymentOrderId=" + searchTempImputationId +
                "&amount=" + parseFloat(ClearFormatCurrency(paymentRow.Amount)) + "&paymentOrderNumber=" + paymentRow.PaymentOrderCode +
                "&branchId=" + paymentRow.BranchCode + "&tempSearchId=" + 0;

        },500)
    }
});

/////////////////////////////////////////
//  Botón  Autorizar                   //
/////////////////////////////////////////
$("#AutorizePaymentOrder").click(function (e) { showConfirm(); });

/**
** CancelPaymentOrder
** botón que cancela orden de pago
*/
$("#CancelPaymentOrder").click(function () {
    $("#alert").UifAlert('hide');

    var rowSelect = $("#SearchPaymentOrdersTable").UifDataTable("getSelected");

    if (rowSelect == null) {
        $("#alert").UifAlert('show', Resources.WarningSelectOneRecord, 'warning');
        return;
    }
    else {      
            searchTempImputationId = rowSelect[0].TempImputationCode;
            PaymentOrdersId = rowSelect[0].PaymentOrderCode;
            confirmCancellationPaymentOrder(Resources.Cancellation, PaymentOrdersId);        
    }
});

/**
** PrintPaymentOrder
** botón que imprime orden de pago
*/
$("#PrintPaymentOrder").click(function () {
    $("#alert").UifAlert('hide');
    if (paymentRow == null) {
        $("#alert").UifAlert('show', Resources.RequiredFieldsMissing, 'warning');
        return;
    }
    if (paymentRow != null) {

        searchTempImputationId = paymentRow.TempImputationCode;
        PaymentOrdersId = paymentRow.PaymentOrderCode;
        var msg = Resources.Print + " " + " " + Resources.PaymentOrder + ": " + PaymentOrdersId + " ?";

        $.UifDialog('confirm', { 'message': msg, 'title': Resources.PaymentOrdersPrint + " " }, function (result) {
            if (result) {

                lockScreen();
                setTimeout(function () {

                 $.ajax({
                    type: "POST",
                    async: false,
                    url: ACC_ROOT + "Report/LoadRePrintPaymentOrdersReport",
                    data: { "paymentOrdersMovementsModel": setDataPrintReportPaymentOrders(paymentRow), "tempImputationId": searchTempImputationId },
                    success: function (data) {
                        unlockScreen();
                        if (parseInt(data) > 0) {
                            showRePrintPaymentOrdersReport();
                            //hideButtons();
                        }
                    }
                });
              }, 500);
            }
        });
    }
});

var isShow = true;
$("#ShowMoreParam").on('click', function () {
    if (isShow) {
        $(".filterRow").fadeOut("slow");
        isShow = false;
    }
    else {
        $(".filterRow").fadeIn("slow");
        isShow = true;
    }
});


$('#ExportToExcel').click(function () {
    var tableData = $("#SearchPaymentOrdersTable").UifDataTable('getData');

    if (tableData.length > 0) {
        var controller = ACC_ROOT + "PaymentOrders/GeneratePaymentOrdersToExcel?branchId=" + parseInt(SearchBranchId) +
            "&userId=" + PaymentOrdersUserId + "&paymentMethodId=" + $("#SearchPaymentMeansSelect").val() +
            "&startDate=" + $("#SearchPaymentOrderStartDate").val() + "&endDate=" +
            $("#SearchPaymentOrderEndDate").val() + "&paymentOrderNumber=" + PaymentOrdersId +
            "&personTypeId=" + $("#SearchPayerTypeSelect").val() + "&beneficiaryIndividualId=" + SearchBeneficiaryId +
            "&beneficiaryDocumentNumber=" + $("#SearchPayerDocumentNumber").val() + "&beneficiaryName=" +
            $("#SearchPayerNamePaymentOrder").val() +
            '&status=' + ($("#PaymentOrderStatus").val() == "" ? -1 : $("#PaymentOrderStatus").val()) +
            '&IsDelivered=' + ($("#SearchDeliveredSelect").val() == "" ? null : ($("#SearchDeliveredSelect").val() == "1" ? true : false)) +
            '&IsReconciled=' + ($("#SearchReconcileSelect").val() == "" ? null : ($("#SearchReconcileSelect").val() == "1" ? true : false)) +
            '&IsAccounting=' + ($("#SearchAccountingSelect").val() == "" ? null : ($("#SearchAccountingSelect").val() == "1" ? true : false));

        var newPage = window.open(controller, '_self', 'width=5, height=5, scrollbars=no');
        setTimeout(function () {
            newPage.open('', '_self', '');
        }, 100);
    }
    else {
        $("#alert").UifAlert('show', Resources.WrongNoDataFound, 'warning');
    }
});

/*------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*															DEFINICIÓN DE FUNCIONES GLOBALES															*/
/*------------------------------------------------------------------------------------------------------------------------------------------------------*/

function hideButtons() {
    $("#ModifyPaymentOrder").hide();
    $("#CancelPaymentOrder").hide();
    $("#PrintPaymentOrder").hide();
    $("#AutorizePaymentOrder").hide();
}

///////////////////////////////////////////////////
//  Limpia campos del formulario                //
//////////////////////////////////////////////////
function setSearchPaymentOrdersDataFieldsEmpty() {
    $("#BranchPaymentOrderSelect").val("");
    $("#SearchPaymentMeansSelect").val("");
    $("#SearchPayerTypeSelect").val("");
    $("#UserPaymentOrders").val("");
    $("#SearchPaymentOrderStartDate").val("");
    $("#SearchPaymentOrderEndDate").val("");
    $("#SearchPaymentOrderNumber").val("");
    $("#SearchPayerDocumentNumber").val("");
    $("#SearchPayerNamePaymentOrder").val("");
    $("#SearchPaymentOrdersTable").dataTable().fnClearTable();
    SearchBeneficiaryId = 0;
    SearchBranchId = 0;
    hideButtons();

    $("#SearchPayerTypeSelect").trigger('change');
}

////////////////////////////////////////////////////////////////
//  Muestra mensaje si/no cancela operación                   //
///////////////////////////////////////////////////////////////
function confirmCancellationPaymentOrder(message, paymentOrderCode) {

    $.UifDialog('confirm', { 'message': message + " " + Resources.TemporalPaymentOrder + " " + paymentOrderCode, 'title': Resources.Cancel }, function (result) {

        if (result) {

            lockScreen();
            setTimeout(function () {            
              $.ajax({
                url: ACC_ROOT + "PaymentOrders/CancellationPaymentOrder",
                data: { "paymentOrderId": paymentOrderCode, "tempImputationId": searchTempImputationId },
                success: function (data) {
                    unlockScreen();
                    if (data) {
                        $("#alert").UifAlert('show', Resources.SuccessfulCancellation, 'success');

                        SearchBranchId = $("#BranchPaymentOrderSelect").val();
                        if ($("#UserPaymentOrders").val() != "") {
                            PaymentOrdersUserId = $("#_UserIdPaymentOrders").val();
                            PaymentOrdersUserId = PaymentOrdersUserNick;
                        }
                        else {
                            PaymentOrdersUserId = ""; //-1;
                        }
                        PaymentOrdersId = $("#SearchPaymentOrderNumber").val();
                        SearchBeneficiaryId = beneficiaryIndividualId;

                        var controller = ACC_ROOT + "PaymentOrders/GetSearchPaymentOrders?branchId=" +
                            parseInt(SearchBranchId) + "&userId=" + PaymentOrdersUserId + "&paymentMethodId=" +
                            $("#SearchPaymentMeansSelect").val() + "&startDate=" + $("#SearchPaymentOrderStartDate").val() +
                            "&endDate=" + $("#SearchPaymentOrderEndDate").val() + "&paymentOrderNumber=" + PaymentOrdersId +
                            "&personTypeId=" + $("#SearchPayerTypeSelect").val() + "&beneficiaryIndividualId=" +
                            SearchBeneficiaryId + "&beneficiaryDocumentNumber=" + $("#SearchPayerDocumentNumber").val() +
                            "&beneficiaryName=" + $("#SearchPayerNamePaymentOrder").val() +
                            '&status=' + ($("#PaymentOrderStatus").val() == "" ? -1 : $("#PaymentOrderStatus").val()) +
                            '&IsDelivered=' + ($("#SearchDeliveredSelect").val() == "" ? null : ($("#SearchDeliveredSelect").val() == "1" ? true : false)) +
                            '&IsReconciled=' + ($("#SearchReconcileSelect").val() == "" ? null : ($("#SearchReconcileSelect").val() == "1" ? true : false)) +
                            '&IsAccounting=' + ($("#SearchAccountingSelect").val() == "" ? null : ($("#SearchAccountingSelect").val() == "1" ? true : false));

                        $("#SearchPaymentOrdersTable").UifDataTable({ source: controller });
                        hideButtons();
                    }
                }
            });
            }, 500);
        }
        else {
            paymentRow = null;
            $("#SearchPaymentOrdersTable").UifDataTable('unselect');
            hideButtons();
        }
    });
}

////////////////////////////////////////////////////////////////
//  Muestra mensaje si/no imprime OP                         //
//////////////////////////////////////////////////////////////
function confirmPrintPaymentOrder(message, paymentOrder) {

    SearchBranchId = $("#BranchPaymentOrderSelect").val();
    if ($("#UserPaymentOrders").val() != "") {
        PaymentOrdersUserId = $("#_UserIdPaymentOrders").val();
        PaymentOrdersUserId = PaymentOrdersUserNick;
    }
    else {
        PaymentOrdersUserId = ""; //-1;
    }
    PaymentOrdersId = $("#SearchPaymentOrderNumber").val();
    SearchBeneficiaryId = beneficiaryIndividualId;

    var controller = ACC_ROOT + "PaymentOrders/GetSearchPaymentOrders?branchId=" + parseInt(SearchBranchId) +
        "&userId=" + PaymentOrdersUserId + "&paymentMethodId=" + $("#SearchPaymentMeansSelect").val() +
        "&startDate=" + $("#SearchPaymentOrderStartDate").val() + "&endDate=" + $("#SearchPaymentOrderEndDate").val() + "&paymentOrderNumber=" + PaymentOrdersId +
        "&personTypeId=" + $("#SearchPayerTypeSelect").val() + "&beneficiaryIndividualId=" + SearchBeneficiaryId + "&beneficiaryDocumentNumber=" +
        $("#SearchPayerDocumentNumber").val() + "&beneficiaryName=" + $("#SearchPayerNamePaymentOrder").val() +
        '&status=' + ($("#PaymentOrderStatus").val() == "" ? -1 : $("#PaymentOrderStatus").val()) +
        '&IsDelivered=' + ($("#SearchDeliveredSelect").val() == "" ? null : ($("#SearchDeliveredSelect").val() == "1" ? true : false)) +
        '&IsReconciled=' + ($("#SearchReconcileSelect").val() == "" ? null : ($("#SearchReconcileSelect").val() == "1" ? true : false)) +
        '&IsAccounting=' + ($("#SearchAccountingSelect").val() == "" ? null : ($("#SearchAccountingSelect").val() == "1" ? true : false));


    $("#SearchPaymentOrdersTable").UifDataTable({ source: controller });
    hideButtons();
}

function showRePrintPaymentOrdersReport() {
    window.open(ACC_ROOT + "Report/ShowPaymentOrdersReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function setAccountingCompanyPaymentOrders() {

    if (IsMulticompany == 0) {
        $("#CompanyPaymentOrderSelect").val(AccountingCompanyDefault);
        $("#CompanyPaymentOrderSelect").attr("disabled", "disabled");
    }
    else {
        $("#CompanyPaymentOrderSelect").removeAttr("disabled");
    }
}

function reloadSearchPaymentOrders() {
    SearchBranchId = $("#BranchPaymentOrderSelect").val();
    if ($("#UserPaymentOrders").val() != "") {
        PaymentOrdersUserId = $("#_UserIdPaymentOrders").val();
        PaymentOrdersUserId = PaymentOrdersUserNick;
    }
    else {
        PaymentOrdersUserId = ""; //-1;
    }
    PaymentOrdersId = $("#SearchPaymentOrderNumber").val();
    SearchBeneficiaryId = beneficiaryIndividualId;

    var controller = ACC_ROOT + "PaymentOrders/GetSearchPaymentOrders?branchId=" + parseInt(SearchBranchId) +
        "&userId=" + PaymentOrdersUserId + "&paymentMethodId=" + $("#SearchPaymentMeansSelect").val() +
        "&startDate=" + $("#SearchPaymentOrderStartDate").val() + "&endDate=" + $("#SearchPaymentOrderEndDate").val() + "&paymentOrderNumber=" + PaymentOrdersId +
        "&personTypeId=" + $("#SearchPayerTypeSelect").val() + "&beneficiaryIndividualId=" + SearchBeneficiaryId + "&beneficiaryDocumentNumber=" +
        $("#SearchPayerDocumentNumber").val() + "&beneficiaryName=" + $("#SearchPayerNamePaymentOrder").val() +
        '&status=' + ($("#PaymentOrderStatus").val() == "" ? -1 : $("#PaymentOrderStatus").val()) +
        '&IsDelivered=' + ($("#SearchDeliveredSelect").val() == "" ? null : ($("#SearchDeliveredSelect").val() == "1" ? true : false)) +
        '&IsReconciled=' + ($("#SearchReconcileSelect").val() == "" ? null : ($("#SearchReconcileSelect").val() == "1" ? true : false)) +
        '&IsAccounting=' + ($("#SearchAccountingSelect").val() == "" ? null : ($("#SearchAccountingSelect").val() == "1" ? true : false));


    $("#SearchPaymentOrdersTable").UifDataTable({ source: controller });
    hideButtons();
}

/////////////////////////////////////////////////////////
//  Arma Objeto   oRePrintPaymentOrdersMovementsModel  //
/////////////////////////////////////////////////////////
function setDataPrintReportPaymentOrders(paymentOrder) {

   
    oRePrintPaymentOrdersMovementsModel = {
        Id: 0,
        PaymentOrderNumber: null,
        BranchId: 0,
        BranchName: null,
        PaymentBranchId: 0,
        PaymentBranchName: null,
        CompanyId: 0,
        CompanyName: null,
        EstimatedPaymentDate: null,
        PaymentDate: null,
        UserId: 0,
        UserName: null,
        PaymentTypeId: 0,
        PaymentTypeName: null,
        CurrencyId: 0,
        CurrencyName: null,
        PaymentIncomeAmount: 0,
        PaymentAmount: 0,
        BeneficiaryDocNumber: null,
        BeneficiaryName: null,
        PayToName: null,
        MovementSummaryItems: []
    };

    if (paymentOrder != null) {

        oRePrintPaymentOrdersMovementsModel.BeneficiaryDocNumber = paymentOrder.BeneficiaryDocumentNumber;
        oRePrintPaymentOrdersMovementsModel.BeneficiaryName = paymentOrder.BeneficiaryName;
        oRePrintPaymentOrdersMovementsModel.BranchId = paymentOrder.BranchCode;
        oRePrintPaymentOrdersMovementsModel.BranchName = paymentOrder.BranchName;
        oRePrintPaymentOrdersMovementsModel.CompanyId = paymentOrder.CompanyCode;
        oRePrintPaymentOrdersMovementsModel.CompanyName = paymentOrder.CompanyName;
        oRePrintPaymentOrdersMovementsModel.CurrencyId = paymentOrder.CurrencyCode;
        oRePrintPaymentOrdersMovementsModel.CurrencyName = paymentOrder.CurrencyName;
        oRePrintPaymentOrdersMovementsModel.Id = paymentOrder.PaymentOrderCode;
        oRePrintPaymentOrdersMovementsModel.PaymentAmount = ReplaceDecimalPoint(ClearFormatCurrency(paymentOrder.Amount).replace(",", "."));         
        oRePrintPaymentOrdersMovementsModel.PaymentBranchId = paymentOrder.BranchPayCode;
        oRePrintPaymentOrdersMovementsModel.PaymentBranchName = paymentOrder.BranchPayName;
        oRePrintPaymentOrdersMovementsModel.EstimatedPaymentDate = paymentOrder.EstimatedPaymentDate;
        oRePrintPaymentOrdersMovementsModel.PaymentDate = paymentOrder.EstimatedPaymentDate;
        oRePrintPaymentOrdersMovementsModel.PaymentOrderNumber = paymentOrder.PaymentOrderCode;
        oRePrintPaymentOrdersMovementsModel.PaymentTypeId = paymentOrder.PaymentMethodCode;
        oRePrintPaymentOrdersMovementsModel.PaymentTypeName = paymentOrder.PaymentMethodName;
        oRePrintPaymentOrdersMovementsModel.PayToName = paymentOrder.PayTo;

        oRePrintMovementSummaryModel = {
            DescriptionMovementSumary: null,
            Debit: 0,
            Credit: 0
        };

        oRePrintPaymentOrdersMovementsModel.MovementSummaryItems.push(oRePrintMovementSummaryModel);
    }

    return oRePrintPaymentOrdersMovementsModel;
}

function triggerSend(targetUrl, targetParameter) {
    //Xhr de autocomplete de varios parametros
    $(document).ajaxSend(function (event, xhr, settings) {
        if (settings.url.indexOf(targetUrl) != -1) {
            settings.url = settings.url + targetParameter;
        }
    });
}

/////////////////////////////////////////
// Limpia Autocomplete Beneficiario    //
/////////////////////////////////////////
function cleanAutocompletesPaymentOrder(identifier) {
    $('#' + identifier + 'ByDocumentNumber').val("");
    $('#' + identifier + 'ByName').val("");
    beneficiaryIndividualId = 0;
    $("#SearchPayerDocumentNumber").val("");
    $("#SearchPayerNamePaymentOrder").val("");
    $("#SearchBeneficiaryIndividualId").val("");


    $('#' + identifier + 'ByDocumentNumber').parent().parent().hide();
    $('#' + identifier + 'ByName').parent().parent().hide();
    $("#SearchPayerDocumentNumber").show();
    $("#SearchPayerNamePaymentOrder").show();
    $("#SearchBeneficiaryIndividualId").show();
}

/////////////////////////////////////////
// Carga Autocomplete Beneficiario     //
/////////////////////////////////////////
function fillAutocompletesPaymentOrder(identifier, selectedItem) {
    $("#alert").UifAlert('hide');
    $('#' + identifier + 'ByDocumentNumber').val(selectedItem.DocumentNumber);
    $('#' + identifier + 'ByName').val(selectedItem.Name);
    if (selectedItem.Id != undefined) {
        beneficiaryIndividualId = selectedItem.Id;

    } else if (selectedItem.AgentId != undefined) {
        beneficiaryIndividualId = selectedItem.AgentId;
    } else {
        beneficiaryIndividualId = selectedItem.CoinsuranceIndividualId;
    }

    if (selectedItem.Id > -1) {
        $("#SearchPayerNamePaymentOrder").val(selectedItem.Name);
        $("#SearchPayerDocumentNumber").val(selectedItem.DocumentNumber);
        $("#SearchBeneficiaryIndividualId").val(selectedItem.DocumentNumber);
        idControl = "S";
    } else {
        $('#' + identifier + 'ByDocumentNumber').val("");
        $('#' + identifier + 'ByName').val("");
        idControl = "";
        $("#SearchPayerNamePaymentOrder").val("");
        $("#SearchPayerDocumentNumber").val("");
        $("#SearchBeneficiaryIndividualId").val("");
    }
}

//////////////////////////////////////////////////////////////////////
// loadAutocompleteEvent..  Iniacializa eventos en autocompletes    //
/////////////////////////////////////////////////////////////////////
function loadAutocompleteEventPaymentOrder(identifier) {

    var selectedDeptor;
    $('#' + identifier + 'ByDocumentNumber').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesPaymentOrder(identifier, selectedItem);
        selectedDeptor = selectedItem;
    });

    $('#' + identifier + 'ByName').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesPaymentOrder(identifier, selectedItem);
        selectedDeptor = selectedItem;
    });


    $('#' + identifier + 'ByDocumentNumber').on('blur', function (event, selectedItem) {
        if (selectedDeptor != undefined) {

            if (selectedDeptor.DocumentNumber != $('#' + identifier + 'ByDocumentNumber').val()) {
                if ((idControl == "K") && ($('#' + identifier + 'ByDocumentNumber').val() == "")) {
                    $('#' + identifier + 'ByDocumentNumber').val("");
                    $('#' + identifier + 'ByName').val("");
                    idControl = "";
                    selectedDeptor = undefined;
                    $("#SearchPayerDocumentNumber").val("");
                    $("#SearchPayerNamePaymentOrder").val("");
                    $("#SearchBeneficiaryIndividualId").val("");

                    return;
                }
            }
            else {
                idControl = "";
            }


            selectedItem = selectedDeptor;
            $("#alert").UifAlert('hide');
            fillAutocompletesPaymentOrder(identifier, selectedItem);
        } else {
            $('#' + identifier + 'ByDocumentNumber').val("");
            $('#' + identifier + 'ByName').val("");

        }
    });


    $('#' + identifier + 'ByName').on('blur', function (event, selectedItem) {
        if (selectedDeptor != undefined) {
            if (selectedDeptor.Name != $('#' + identifier + 'ByName').val()) {
                if ((idControl == "K") && ($('#' + identifier + 'ByName').val() == "")) {
                    $('#' + identifier + 'ByDocumentNumber').val("");
                    $('#' + identifier + 'ByName').val("");
                    idControl = "";
                    selectedDeptor = undefined;
                    $("#SearchPayerDocumentNumber").val("");
                    $("#SearchPayerNamePaymentOrder").val("");
                    $("#SearchBeneficiaryIndividualId").val("");
                    return;
                }

            }
            else {
                idControl = "";
            }
            selectedItem = selectedDeptor;

            $("#alert").UifAlert('hide');
            fillAutocompletesPaymentOrder(identifier, selectedItem);
        }
        else {
            $('#' + identifier + 'ByDocumentNumber').val("");
            $('#' + identifier + 'ByName').val("");

        }
    });


    $('#' + identifier + 'ByDocumentNumber').on('keyup', function (event, selectedItem) {
        //beneficiaryIndividualId = 0;
        tecla = (document.all) ? event.keyCode : event.which;
        if ((tecla == 8) || (tecla == 46)) {
            idControl = "K";
        }

    });

    $('#' + identifier + 'ByName').on('keyup', function (event, selectedItem) {
        tecla = (document.all) ? event.keyCode : event.which;
        if ((tecla == 8) || (tecla == 46)) {
            idControl = "K";
        }
    });

}

/////////////////////////////////////////
// Autocomplete PaymentOrder           //
/////////////////////////////////////////
function fillPaymentAutocomplete(selectedItem) {
    PaymentOrdersUserId = selectedItem.id;
    PaymentOrdersUserNick = selectedItem.value;
    $("#_UserIdPaymentOrders").val(PaymentOrdersUserId);
}

///////////////////////////////////////////////////
//  Recarga listView Resumen                     //
///////////////////////////////////////////////////
function GetDebitsAndCreditsMovementTypesPayment(tempImputationId, amount, incomeAmount) {
    var totalDebits = 0;
    var totalCredits = 0;
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "ReceiptApplication/GetDebitsAndCreditsMovementTypes",
        data: JSON.stringify({ "tempImputationId": tempImputationId, "amount": amount }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            for (i in data) {
                totalDebits += data[i].Debits;
                totalCredits += data[i].Credits;
            }

            setMainTotalControl = SetMainTotalControl(incomeAmount, totalDebits, totalCredits, 1)

            if (setMainTotalControl == 0) {
                $("#AutorizePaymentOrder").show();
            }
            else {
                $("#AutorizePaymentOrder").hide();
            }
        }
    });
}

function showConfirm() {
    $.UifDialog('confirm', {
        'message': Resources.AutorizerPaymentOrder + PaymentOrdersId + ',.. ' + Resources.ContinueLabel
        , 'title': Resources.Authorize
    }, function (result) {
        if (result) {
        }
    });
}

function SetDataPaymentOrder(paymentOrderStatusId) {
    SetDataPaymentOrderSearchEmpty();

    oPaymentOrder.PaymentOrderItemId = PaymentOrdersId;

    if ($("#SearchPaymentMeansSelect").val() == $("#ViewBagParamPaymentMethodTransferPayment").val()) {
        oPaymentOrder.AccountBankId = 0;
    }
    else {
        oPaymentOrder.AccountBankId = -1;
    }

    return oPaymentOrder;
}

function SetDataPaymentOrderSearchEmpty() {
    oPaymentOrder = {
        PaymentOrderItemId: 0,
        AccountingDate: null,
        AccountBankId: 0,
        BranchId: 0,
        BranchPayId: 0,
        CompanyId: 0,
        EstimatedPaymentDate: null,
        PaymentDate: null,
        PaymentMethodId: 0,
        PaymentSourceId: 0,
        IndividualId: 0,
        PersonTypeId: 0,
        CurrencyId: 0,
        ExchangeRate: 0,
        PaymentIncomeAmount: 0,
        PaymentAmount: 0,
        PayTo: null,
        StatusId: 0
    };
}