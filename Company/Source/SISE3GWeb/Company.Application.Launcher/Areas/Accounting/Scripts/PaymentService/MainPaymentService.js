/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES GLOBALES                                                                       */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
var typePay = 0;
var amount = 0;

var branchId = $("#ViewBagBranchPaymentService").val(); //definido en web.config
var issuingBankIdPaymentService = $("#ViewBagIssuingBankPaymentService").val(); //definido en web.config banco emisor Tarjeta

var idSelected = new Array();
var typePerson = 1;

var dateService = "";
var mydate = new Date();
var year = mydate.getYear();

//variables para apertura de caja
var isOpen = false;
var idBillControl = 0;

var insuredDocumentNumberSelect = false;
var insuredNameSelect = false;
var op = 0;

var numTransf = 105600;
var exchangeRate = 0;
var InsuredId = "";
var AgentId = "";
var AgentIdSearch = "";
var insuredName = "";
var insuredDocumentNumber = "";
var agentName = "";
var payerIndividualId = 0;

var documentNumber = '#_documentNumber';

var pageNumber = 1;
var totRegisters = 0;

var OpenBillingPromise;
var allowOpenBillPromise;
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*   PANTALLA PRINCIPAL BOTON DE PAGOS                                                                                                       */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#agentSearch").hide();
$("#AddFileButton").hide();
$("#btnExcel").hide();

//Setea si es Persona Agente o Asegurada
$('.cPersonType').change(function () {
    if ($("#_radioAgent").is(':checked')) {

        setAgentPerson();

        $("#insuredSearch").hide();
        $("#agentSearch").show();
        $("#AddFileButton").show();
        $("imageform").text("");

        $("#information").text(Resources.MainPaymentServiceInformationAgent);
    }
    else if ($("#_radioInsured").is(':checked')) {

        setInsuredPerson();

        $("#agentSearch").hide();
        $("#insuredSearch").show();
        $("#AddFileButton").hide();
        $("#btnExcel").hide();

        $("#information").text(Resources.MainPaymentServiceInformationInsured);
    };
});

//OK
function setAgentPerson() {

    typePerson = "1";

    $("#PoliciesTable").dataTable().fnClearTable();
    $("#InputExcel").val("");
    $("#_fileLocation").val("");
    $("#_totalPay").val("0");
    $("#_totalPay").trigger('blur');
    $("#_totalPayP").trigger('blur');
    $("#_documentNumber").val("");
    $("#_names").val("");
    $("#_informationData").val("");
    $("#textSelectedPolicies").val("");
    InsuredId = "";
    AgentIdSearch = "";
}

//OK
function setInsuredPerson() {

    typePerson = "2";

    $("#PoliciesTable").dataTable().fnClearTable();
    $("#InputExcel").val("");
    $("#_fileLocation").val("");
    $("#_totalPay").val("0");
    $("#_totalPay").trigger('blur');
    $("#InsuredDocumentNumber").val("");
    $("#PaymentServiceInsuredName").val("");
    $("#_emailInsured").val("");
    $("#_informationData").val("");
    $("#textSelectedPolicies").val("");
    InsuredId = "";
    AgentIdSearch = "";
}


$("#_dateprocess").val(dateService);

$("#BankDepositButton").attr("disabled", true);
$("#CreditCardsButton").attr("disabled", true);
$("#PaymentAreaButton").attr("disabled", true);
$("#PrintInvoiceButton").attr("disabled", true);
$("#_documentNumber").attr("maxlength", 13);

$("#PreviousPage").attr("disabled", true);
$("#NextPage").attr("disabled", true);


//AUTOCOMPLETES ASEGURADO DOCUMENTO
$('#InsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {

    InsuredId = selectedItem.Id;
    AgentId = "";

    $("#InsuredDocumentNumber").val(selectedItem.DocumentNumber);
    $("#PaymentServiceInsuredName").val(selectedItem.Name);
    insuredName = selectedItem.Name;

    $("#_fileLocation").val("");
    $("#_informationData").val(selectedItem.DocumentNumber + " - " + selectedItem.Name);

    pageNumber = 1;

    refreshGridInsured("");
});

//AUTOCOMPLETES ASEGURADO NOMBRE
$('#PaymentServiceInsuredName').on('itemSelected', function (event, selectedItem) {
    InsuredId = selectedItem.Id;
    AgentId = "";

    $("#InsuredDocumentNumber").val(selectedItem.DocumentNumber);
    $("#PaymentServiceInsuredName").val(selectedItem.Name);
    insuredName = selectedItem.Name;

    $("#_fileLocation").val("");

    $("#_informationData").val(selectedItem.DocumentNumber + " - " + selectedItem.Name);

    pageNumber = 1;
    refreshGridInsured("");
});

//AUTOCOMPLETE AGENTE DOCUMENTO
$('#_documentNumber').on('itemSelected', function (event, selectedItem) {

    AgentId = selectedItem.Id;
    agentName = selectedItem.Name;
    AgentIdSearch = selectedItem.Id;

    InsuredId = "";
    insuredName = "";

    $("#_fileLocation").val("");
    $("#_documentNumber").val(selectedItem.DocumentNumber);
    $("#_names").val(selectedItem.Name);
    $("#_informationData").val(selectedItem.DocumentNumber + " - " + selectedItem.Name);

    pageNumber = 1;
    refreshGridInsured("");

    $("#btnExcel").show();
});

$("#_documentNumber").blur(function (event) {
    event.preventDefault();
    if ($("#_documentNumber").val() == "") {
        agentId = 0;
        $('#_names').val("");
    }
});

//AUTOCOMPLETE AGENTE NOMBRE
$('#_names').on('itemSelected', function (event, selectedItem) {

    InsuredId = "";
    insuredName = "";

    AgentId = selectedItem.Id;
    agentName = selectedItem.Name;
    AgentIdSearch = selectedItem.Id;

    $("#_fileLocation").val("");
    $("#_names").val(selectedItem.Name);
    $("#_informationData").val(selectedItem.DocumentNumber + " - " + selectedItem.Name);

    pageNumber = 1;
    refreshGridInsured("");

    $("#btnExcel").show();

    $("#_documentNumber").val(selectedItem.DocumentNumber);
});

//OK
function refreshGridInsured(file) {    

    var control = ACC_ROOT + "PaymentService/PremiumReceivableSearchPolicy?insuredId=" + InsuredId
        + "&payerId=" + "" + "&agentId=" + AgentIdSearch + "&groupId=" + "" + "&policyDocumentNumber=" + ""
        + "&salesTicket=" + "" + "&branchId=" + "" + "&prefixId=" + "" + "&endorsementDocumentNumber=" + ""
        + "&dateFrom=" + "" + "&dateTo=" + "" + "&pathFile=" + file + "&pageNumber=" + pageNumber;

    $("#PoliciesTable").UifDataTable({ source: control });

    var invoiceTotal = 0;

    $("#_totalPay").val("0");
    $("#textSelectedPolicies").val("");

    setTimeout(function () {
        $("#PreviousPage").attr("disabled", false);
        $("#NextPage").attr("disabled", false);
        totRegisters = $("#PoliciesTable").UifDataTable('getData').length;
    }, 25000);
}


//levanta detalles del registro seleccionado
$('#PoliciesTable').on('rowEdit', function (event, data, position) {

    $("#alertDepositBank").UifAlert('hide');
    $("#alertCreditCard").UifAlert('hide');
    $("#alertPaymentZone").UifAlert('hide');
    showConsult(data);
});


$("#_totalPay").blur(function () {
    $("#_totalPay").val(FormatCurrency(FormatDecimal($("#_totalPay").val())));
});


//OK
//LEVANTA MODAL DE CONSULTA DETALLES GRILLA DE POLIZAS
function showConsult(row) {
    var commisionPercentage = 0;
    var agentParticipationPercentage = 0;

    $.ajax({
        async: false,
        url: ACC_ROOT + "PaymentService/GetPendingCommission",
        data: { "policyId": row.PolicyId, "endorsementId": row.EndorsementId },
        success: function (data) {
            if (data != null) {
                commisionPercentage = data.CommissionPercentage;
                agentParticipationPercentage = data.AgentParticipationPercentage;
            }
        }
    });

    $("#_serviceBranch").val(row.BranchDescription);
    $("#_servicePrefix").val(row.PrefixDescription);
    $("#_servicePolicies").val(row.PolicyDocumentNumber);
    $("#_serviceEndorsement").val(row.EndorsementDocumentNumber);
    $("#_servicePayExpDate").val(parseJsonDate(row.PaymentExpirationDate));
    $("#_serviceQuota").val(row.PaymentNumber);
    $("#_serviceCurrency").val(row.CurrencyDescription);
    $("#_serviceAmount").val(row.PaymentAmount);
    $("#_serviceInsured").val(row.InsuredName);
    $("#_serviceIntermediary").val(row.PolicyAgentName);
    $("#_servicePercentage").val(commisionPercentage);
    $("#_serviceCommission").val(parseFloat(ClearFormatCurrency(row.PaymentAmount).replace(",", ""))
        * (agentParticipationPercentage / 100) * (commisionPercentage / 100));

    $("#_serviceCommission").val(FormatCurrency(FormatDecimal($("#_serviceCommission").val())));

    $('#DetailedConsultationDialog').UifModal('showLocal', Resources.MovementDetails);
}

//////////////////////////////////////
// Botón Imprimir Detalle           //
//////////////////////////////////////
$("#PaymentServiceSaveBillModal").find('#PrintDetail').click(function () {
    ShowReceiptReportMainPayment(branchId, $("#PaymentServiceSaveBillModal").find("#BillingId").text(), "");
    $('#PaymentServiceSaveBillModal').UifModal('hide');
});

//////////////////////////////////////////////
// Visualiza el reporte de caja en pantalla //
//////////////////////////////////////////////
function ShowReceiptReportMainPayment(branchId, billCode, otherPayerName) {

    var controller = ACC_ROOT + "Report/ShowReceiptReport?branchId=" + branchId + "&billCode=" +
        billCode + "&reportId=3" + "&otherPayerName=" + otherPayerName;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*   BOTON DEPOSITO BANCARIO                                                                                                                                   */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#BankDepositButton").click(function () {

    $("#alertDepositBank").UifAlert('hide');
    $("#alertCreditCard").UifAlert('hide');
    $("#alertPaymentZone").UifAlert('hide');
    if (quotaOrderControl() == true) {

        var invoiceTotal = parseFloat(ClearFormatCurrency($("#_totalPay").val()).replace(",", ""));

        if (invoiceTotal != 0) {

            // Se valida que el total a pagar sea positivo
            if (invoiceTotal > 0) {
                typePay = $("#ViewBagParamPaymentMethodDepositVoucher").val(); //DEPOSITO EN CUENTA

                showBankDeposit();
            }
            else {
                $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
            }
        } else {
            $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
        }
    } else {
        $("#alertMainPaymentService").UifAlert('show', Resources.QuotaSequenceAdded, "warning");
    }
});


function showBankDeposit() {

    $("#_totalDepositPayment").val($("#_totalPay").val());
    $('#BankDepositDialog').UifModal('showLocal', Resources.MainPaymentServiceTitleBankDeposit);
}


//OK
//BOTON PAGAR DEL MODAL
$("#btnPayBank").click(function () {

    if ($("#DepositBankForm").valid()) {

        var invoiceTotal = parseFloat(ClearFormatCurrency($("#_totalPay").val()).replace(",", ""));

        if (parseFloat(invoiceTotal) != parseFloat(0)) {
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "Billing/SaveBillRequest",
                dataType: 'json',
                data: {
                    "frmBill": SetDataBillPaymentService(), "itemsToPayGridModel": SetDatatblItemToPaySummary(),
                    "branchId": branchId, "preliquidationBranch": 0
                },
                success: function (data) {

                    if (data.success == false && data.result != -2) {

                        $("#alertDepositBank").UifAlert('show', data.result, "danger");

                    } else if (data.success == false && data.result == -2) {

                        $("#alertDepositBank").UifAlert('show', Resources.PolicyComponentsValidationMessage, "danger");
                    }
                    else {

                        if (data.BillId > 0) {

                            $("#alertDepositBank").UifAlert('show', Resources.PayBankDepositMsj, "warning");

                            $('#PaymentServiceSaveBillModal').find("#ReceiptTotalAmount").text($("#_totalPay").val());
                            $('#PaymentServiceSaveBillModal').find("#ReceiptDescription").text(data.Description);
                            $('#PaymentServiceSaveBillModal').find("#BillingId").text("00000" + data.BillId);
                            $("#PaymentServiceSaveBillModal").find("#TransactionNumber").text("00000" + data.TechnicalTransaction);
                            $('#PaymentServiceSaveBillModal').find("#BillingDate").text(data.Date);
                            $('#PaymentServiceSaveBillModal').find("#ReceiptUser").text(MainPaymentServiceGetUserNick());

                            if (data.ShowMessage == "False") {

                                $("#PaymentServiceSaveBillModal").find("#accountingLabelDiv").hide();
                                $("#PaymentServiceSaveBillModal").find("#accountingMessageDiv").hide();

                            } else {
                                $("#PaymentServiceSaveBillModal").find("#accountingLabelDiv").show();
                                $("#PaymentServiceSaveBillModal").find("#accountingMessageDiv").show();
                            }

                            if (data.StatusId == 3)//se aplicó el recibo
                            {
                                // applyReceiptNumber = data.BillNumber;
                                $("#PaymentServiceSaveBillModal").find("#ApplyReceipt").hide();
                                if (data.ShowImputationMessage == "False") {

                                    $("#PaymentServiceSaveBillModal").find("#applicationIntegration").hide();
                                } else {

                                    $("#PaymentServiceSaveBillModal").find("#applicationIntegration").show();
                                }
                            }


                            if (data.ShowImputationMessage == "False") {

                                $("#PaymentServiceSaveBillModal").find("#applicationIntegration").hide();
                            } else {

                                $("#PaymentServiceSaveBillModal").find("#applicationIntegration").show();
                            }

                            $('#PaymentServiceSaveBillModal').find("#accountingMessage").val(data.Message);
                            $('#PaymentServiceSaveBillModal').find("#accountingIntegrationMessage").val(data.ImputationMessage);

                            $('#BankDepositDialog').UifModal('hide');
                            $('#PaymentServiceSaveBillModal').UifModal('showLocal', Resources.ReceiptSaveSuccess);

                            setDataBillEmpty();
                            ClearAll();
                        }
                    }
                }
            });
        } else {
            $("#alertDepositBank").UifAlert('show', Resources.TotalPayment, "warning");
        }
    }
});


//OK
//Controla el orden en que se añaden las cuotas
function quotaOrderControl() {
    var result = false;
    var chkErr = 0;
    var ids = $("#PoliciesTable").UifDataTable("getSelected");

    for (var k in ids) {

        var rowData = ids[k];

        //Establecer control para evitar desbordamiento
        if (k < ids.length - 1) {

            var j = parseInt(k) + 1;
            var rowDataNext = ids[j];

        } else {

            break;
        }

        // Ajuste Jira SMT-1669 Inicio
        var sup = rowData.EndorsementId + rowData.PolicyId + rowData.PolicyDocumentNumber + rowData.PaymentNumber;
        var inf = rowDataNext.EndorsementId + rowDataNext.PolicyId + rowDataNext.PolicyDocumentNumber + rowDataNext.PaymentNumber;
        // Ajuste Jira SMT-1669 Fin

        if (sup == inf) {
            chkErr += 1;
        }
    }
    if (chkErr > 0) {

        result = false;
    } else {
        result = true;
    }

    return result;
}

//no existe evento para deseleccionar
$('#PoliciesTable').on('selectAll', function (event) {

    //$("#PoliciesTable").UifDataTable('unselect');

    //$('#PoliciesTable tbody tr td button').click();
        //.removeClass('glyphicon-unchecked').addClass('glyphicon-check');

    //var data = $("#PoliciesTable").UifDataTable("getSelected");
    //if (data != null) {
    //    getTotalAmount(1, data);
    //}

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //var rows = $("#PoliciesTable tbody .row-selected");

    //for (var k in rows) {
    //    if (k < rows.length) {
    //        var item = $(rows[k]).find("td");
    //        var item1 = $(rows[k]).find("glyphicon-check");


    //        $(item[0]).find("button").click();

    //    }
    //}

    //se refresca la grilla ya que no funciona el evento de deschequeo
    refreshGridInsured("");

    //var rows = $("#PoliciesTable th");
    //$(rows).find("button").find("span").removeClass('glyphicon-checked').addClass('glyphicon-unchecked');

    //var data = $("#PoliciesTable").UifDataTable("getSelected");
    //getTotalAmount(1, data);
});


//OK
//resta cuando se deselecciona un registro de la tabla
$('body').delegate('#PoliciesTable tbody tr', "click", function () {

    if ($(this).hasClass('selected'))//cuando deselecciona
    {
        $(this).removeClass('selected');
        op = 0;
    }
    else {//cuando selecciona
        $(this).siblings('.selected').removeClass('selected');
        $(this).addClass('selected');
        op = 1;
    }

    var data = $("#PoliciesTable").UifDataTable("getSelected");
    getTotalAmount(op, data);

});

$('#PoliciesTable').on('rowSelected', function (event, data, position) {   
    AgentId = data.PayerId;    
});

//OK
//función que totaliza el monto de las pólizas seleccionadas
function getTotalAmount(op, data) {

    var totalSum = 0;
    var totalSelected = 0;
    var ids = data;

    for (var i in ids) {

        var row = ids[i];

        totalSum = totalSum + parseFloat(ClearFormatCurrency(row.PaymentAmount).replace(",", ""));
        totalSelected++;
    }

    $("#_totalPay").val(FormatCurrency(FormatDecimal(totalSum)));
    //$("#_totalPayP").val(FormatCurrency(FormatDecimal(totalSum)));

    $("#textSelectedPolicies").val(totalSelected + " " + Resources.GridFooterSelectedPolicies);

    // Ajuste Jira SMT-1669 Inicio
    //Validación Selección cuotas de poliza en orden
    $("#alertMainPaymentService").UifAlert('hide');

    var result = true;
    var allPolicies = $("#PoliciesTable").UifDataTable('getData');
    var selectedPolicies = $("#PoliciesTable").UifDataTable("getSelected");

    if (selectedPolicies != null) {
        for (var ap in allPolicies) {
            var rowAllPolicies = allPolicies[ap];

            for (var sp in selectedPolicies) {
                var rowSelectedPolicies = selectedPolicies[sp];

                if (rowAllPolicies.PolicyDocumentNumber == rowSelectedPolicies.PolicyDocumentNumber) {
                    var rowTr = $('#PoliciesTable tbody tr')[ap];

                    if (!$(rowTr).hasClass('row-selected'))//cuando deselecciona
                    {
                        if (rowAllPolicies.PaymentNumber < rowSelectedPolicies.PaymentNumber) {
                            result = false;
                            $("#alertMainPaymentService").UifAlert('show', Resources.QuotaSequenceAdded, "warning");
                        }
                    }
                }
            }
        }
    }
    // Ajuste Jira SMT-1669 Fin

    if ((totalSelected > 0) && (result)) {

        $("#BankDepositButton").attr("disabled", false);
        $("#CreditCardsButton").attr("disabled", false);
        $("#PaymentAreaButton").attr("disabled", false);
        $("#PrintInvoiceButton").attr("disabled", false);
    }
    else {

        $("#BankDepositButton").attr("disabled", true);
        $("#CreditCardsButton").attr("disabled", true);
        $("#PaymentAreaButton").attr("disabled", true);
        $("#PrintInvoiceButton").attr("disabled", true);
    }
}

//OK
function setDataBillEmpty() {

    oItemsToPayGrid = {
        BillItem: []
    };

    oBillItem = {
        BillItemId: 0,
        BillId: 0,
        ItemTypeId: 0,
        Description: "",
        Amount: 0,
        PaidAmount: 0,
        CurrencyId: 0,
        ExchangeRate: 0,
        Column1: "",
        Column2: "",
        Column3: "",
        Column4: "",
        Column5: "",
        Column6: "",
        Column7: ""
    };

    oBillModel = {
        BillId: 0,
        BillingConceptId: 0,
        BillControlId: 0,
        RegisterDate: null,
        Description: null,
        PaymentsTotal: 0,
        PayerId: -1,
        PaymentSummary: [],
        PayerName: ""
    };

    oPaymentSummaryModel = {
        PaymentId: 0,
        BillId: 0,
        PaymentMethodId: 0,
        Amount: 0,
        CurrencyId: 0,
        LocalAmount: 0,
        ExchangeRate: 0,
        CheckPayments: [],
        CreditPayments: [],
        TransferPayments: [],
        DepositVouchers: [],
        RetentionReceipts: []
    };

    oCheckModel = {
        DocumentNumber: null,
        IssuingBankId: -1,
        Date: null,
        Amount: 0
    };

    oCreditModel = {
        CardNumber: null,
        Voucher: null,
        AuthorizationNumber: null,
        CreditCardTypeId: 0,
        IssuingBankId: -1,
        Holder: null,
        ValidThruYear: 0,
        ValidThruMonth: 0,
        TaxBase: 0
    };

    oTransferModel = {
        DocumentNumber: null,
        IssuingBankId: -1,
        ReceivingBankId: -1,
        Date: null,
        Amount: 0
    };

    oDepositVoucherModel = {
        VoucherNumber: null,
        ReceivingAccountBankId: -1,
        ReceivingAccountNumber: null,
        Date: null,
        DepositorName: null
    };

    oRetentionReceiptModel = {
        BillNumber: null,
        AuthorizationNumber: null,
        SerialNumber: null,
        VoucherNumber: null,
        SerialVoucherNumber: null,
        Date: null
    };
}

//OK
function ClearGridData() {
    $('#_informationData').val('');
    $('#_fileLocation').val('');

    $("#PoliciesTable").dataTable().fnClearTable();
}

//OK
function ClearAll() {
    $("#_informationData").val("");
    $("#_documentNumberInsured").val("");
    $("#_namesInsured").val("");
    $("#_emailInsured").val("");
    $("#_fileIdentification").val("");
    $("#_fileLocation").val("");
    $("#_documentNumber").val("");
    $("#_names").val("");
    //$("#_email").val("");

    InsuredId = 0;
    AgentId = 0;

    $("#_totalPay").val("");
    $("#PoliciesTable").dataTable().fnClearTable();

    $("#ddlPaymentType").val("");
    $("#ConduitDrop").val("");
    $("#_numberCreditCard").val("");
    $("#_numberVoucher").val("");
    $("#_namesCreditCard").val("");
    $("#CardExpirationYear").val('');

    $("#_accountNumber").val("");
    $("#_numberBallot").val("");

    $("#CardCVV").val("");
    $("#ExpirationYear").val("");
    $("#_AuthorizationNumber").val("");
    $("#ExpirationMonthDrop").val("");

    $("#_accountNumber").val("");
    $("#_numberBallot").val("");
    $("#_pay").hide();

    $("#_totalPay").val("0");
}


/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*   BOTON PAGO CON TARJETAS                                                                                                                             */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

//BOTON PAGO TARJETAS
$("#CreditCardsButton").click(function () {
    $("#alertDepositBank").UifAlert('hide');
    $("#alertCreditCard").UifAlert('hide');
    $("#alertPaymentZone").UifAlert('hide');
    if (quotaOrderControl() == true) {

        var invoiceTotal = parseFloat(ClearFormatCurrency($("#_totalPay").val()).replace(",", ""));

        if (parseFloat(invoiceTotal) != parseFloat(0)) {
            // Se valida que el total a pagar sea positivo
            if (parseFloat(invoiceTotal) > 0) {

                typePay = $("#ViewBagParamPaymentMethodCreditableCreditCard").val();

                showCreditCardPayment();
            } else {
                $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
            }
        } else {
            $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
        }
    } else {
        $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
    }
});

//OK
function showCreditCardPayment() {
    $("#_totalCreditPayment").val($('#_totalPay').val());
    $('#CardPaymentDialog').UifModal('showLocal', Resources.Card);
}

//OK
$("#btnPayCard").click(function () {

    if ($("#CardPaymentForm").valid()) {

        var invoiceTotal = ClearFormatCurrency($("#_totalPay").val()).replace("", ",");

        if (parseFloat(invoiceTotal) != parseFloat(0)) {

            $("#_numberVoucher").val("00048");
            $("#_AuthorizationNumber").val("00032");

            var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox/') > -1;
            var dataType = "";

            if (isFirefox) {
                dataType = "json";
            }

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "Billing/SaveBillRequest",
                dataType: 'json',
                data: {
                    "frmBill": SetDataBillPaymentService(), "itemsToPayGridModel": SetDatatblItemToPaySummary(),
                    "branchId": branchId, "preliquidationBranch": 0
                },
                success: function (data) {

                    if (data.success == false && data.result != -2) {

                        $("#alertCreditCard").UifAlert('show', data.result, "danger");

                    } else if (data.success == false && data.result == -2) {

                        $("#alertCreditCard").UifAlert('show', Resources.PolicyComponentsValidationMessage, "danger");
                    }
                    else {

                        if (data.BillId > 0) {

                            $("#alertCreditCard").UifAlert('show', "Pago realizado en Depósito Bancario", "warning");

                            $('#PaymentServiceSaveBillModal').find("#ReceiptTotalAmount").text($("#_totalPay").val());
                            $('#PaymentServiceSaveBillModal').find("#ReceiptDescription").text(data.Description);
                            $('#PaymentServiceSaveBillModal').find("#BillingId").text("00000" + data.BillId);
                            $("#PaymentServiceSaveBillModal").find("#TransactionNumber").text("00000" + data.TechnicalTransaction);
                            $('#PaymentServiceSaveBillModal').find("#BillingDate").text(data.Date);
                            $('#PaymentServiceSaveBillModal').find("#ReceiptUser").text(MainPaymentServiceGetUserNick());

                            if (data.ShowMessage == "False") {

                                $("#PaymentServiceSaveBillModal").find("#accountingLabelDiv").hide();
                                $("#PaymentServiceSaveBillModal").find("#accountingMessageDiv").hide();

                            } else {
                                $("#PaymentServiceSaveBillModal").find("#accountingLabelDiv").show();
                                $("#PaymentServiceSaveBillModal").find("#accountingMessageDiv").show();
                            }

                            if (data.StatusId == 3)//se aplicó el recibo
                            {
                                // applyReceiptNumber = data.BillNumber;
                                $("#PaymentServiceSaveBillModal").find("#ApplyReceipt").hide();
                                if (data.ShowImputationMessage == "False") {

                                    $("#PaymentServiceSaveBillModal").find("#applicationIntegration").hide();
                                } else {

                                    $("#PaymentServiceSaveBillModal").find("#applicationIntegration").show();
                                }
                            }
                            $('#PaymentServiceSaveBillModal').find("#accountingMessage").val(data.Message);
                            $('#PaymentServiceSaveBillModal').find("#accountingIntegrationMessage").val(data.ImputationMessage);

                            $('#CardPaymentDialog').UifModal('hide');
                            $('#PaymentServiceSaveBillModal').UifModal('showLocal', Resources.ReceiptSaveSuccess);

                            setDataBillEmpty();
                            ClearAll();

                        }
                    }
                }
            });
        }
    }
});

//OK
$("#CardExpirationYear").blur(function () {

    $("#alertCreditCard").UifAlert('hide');
    var actualDate = getDate();
    var partDate = actualDate.split("/");
    var year = partDate[2];

    var minYearValue = 1900;
    var maxYearValue = 9999;

    if ($("#CardExpirationYear").val() != "") {
        if (maxYearValue < parseInt($("#CardExpirationYear").val()) || parseInt($("#CardExpirationYear").val()) < minYearValue) {

            //$("#alertCreditCard").UifAlert('show', Resources.MainPaymentServiceMessageExpirationYear, "warning");
            $("#alertCreditCard").UifAlert('show', Resources.MessageIncorrectExpirationYear, "warning");

            $("#CardExpirationYear").val("");
            $("#CardExpirationYear").focus();
        }
        else {
            if ($("#ExpirationMonthDrop").val() != "") {
                checkCardExpirationDate();
            }
        }
    }
});

//OK
$('#ExpirationMonthDrop').on('itemSelected', function (event, selectedItem) {
    $("#alertCreditCard").UifAlert('hide');
    checkCardExpirationDate();
});

//OK
function checkCardExpirationDate() {

    var date = getDate();
    var partDate = date.split("/");
    var day = parseInt(partDate[0], 10);
    var month = parseInt(partDate[1], 10);
    var year = partDate[2];

    if ($("#CardExpirationYear").val() < year) {

        $("#alertCreditCard").UifAlert('show', Resources.ExpiredCreditCard, "warning");

        $("#CardExpirationYear").val('');
        $("#CardExpirationYear").focus();

    }
    else if ($("#CardExpirationYear").val() == year && $("#ExpirationMonthDrop").val() < month) {

        $("#alertCreditCard").UifAlert('show', Resources.ExpiredCreditCard, "warning");
        $("#ExpirationMonthDrop").val("");

    }
    else if ($("#CardExpirationYear").val() == year && $("#ExpirationMonthDrop").val() == month && day >= 1) {

        $("#alertCreditCard").UifAlert('show', Resources.ExpiredCreditCard, "warning");
        $("#ExpirationMonthDrop").val("");

    }
}

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*   BOTON ZONAS DE PAGOS                                                                                                                           */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

//OK
$("#PaymentAreaButton").click(function () {
    $("#alertDepositBank").UifAlert('hide');
    $("#alertCreditCard").UifAlert('hide');
    $("#alertPaymentZone").UifAlert('hide');
    if (quotaOrderControl() == true) {

        var invoiceTotal = parseFloat(ClearFormatCurrency($("#_totalPay").val()).replace(",", ""));

        if (parseFloat(invoiceTotal) != parseFloat(0)) {
            // Se valida que el total a pagar sea positivo
            if (invoiceTotal > 0) {

                typePay = $("#ViewBagParamPaymentMethodPaymentArea").val();

                var data = $("#PoliciesTable").UifDataTable("getSelected");

                showPaymentArea(data);
            }
            else {
                $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
            }
        } else {
            $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
        }
    } else {
        $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
    }
});



//OK
function showPaymentArea(data) {

    $("#TransferCurrencyDrop").val(data[0].CurrencyId);
    $("#TransferCurrencyDrop").attr("disabled", true);

    $("#_TransferAmount").val($("#_totalPay").val());
    $("#_TransferDate").val(GetCurrentDate());
    $("#_TransferExchangeRate").val(FormatCurrency(FormatDecimal(data[0].ExchangeRate)));
    $("#_TransferHolderName").val(insuredName);
    $("#_TransferLocalAmount").val(data[0].PaymentAmount);

    $("#ddlTransferReceivingBank").val($("#ViewBagTransferReceivingBankPaymentService").val());

    $("#ddlTransferReceivingBank").trigger('change');

    setTimeout(function () {
        $("#ddlAccountNumberReceivingBank").val($("#ViewBagAccountNumberReceivingBankPaymentService").val());

        $("#ddlTransferReceivingBank").attr("disabled", true);
        $("#ddlAccountNumberReceivingBank").attr("disabled", true);
    }, 1000);

    // Ajuste Jira SMT-1687 Inicio
    $("#inputTransferIssuingBank").val("");
    $("#_TransferIssuingAccountNumber").val("");
    $("#_TransferDocumentNumber").val("");
    // Ajuste Jira SMT-1687 Fin

    $('#PaymentZoneDialog').UifModal('showLocal', Resources.PaymentArea);
}

/////////////////////////////////////////
// Combo banco receptor - transferencia //
/////////////////////////////////////////
$("#PaymentZoneDialog").find("#ddlTransferReceivingBank").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Billing/GetAccountByBankId?bankId=" + selectedItem.Id;
        $("#PaymentZoneDialog").find("#ddlAccountNumberReceivingBank").UifSelect({ source: controller });
    }
});


////////////////////////////////////////////////////
// Autocomplete banco emisor - Transferencia      //
////////////////////////////////////////////////////
$("#PaymentZoneDialog").find('#inputTransferIssuingBank').on('itemSelected', function (event, selectedItem) {
    issuingBankIdPaymentService = selectedItem.Id;
});

//OK
$("#btnPayZone").click(function () {

    if ($("#PaymentZoneForm").valid()) {

        var invoiceTotal = parseFloat(ClearFormatCurrency($("#_totalPay").val()).replace(",", ""));

        if (parseFloat(invoiceTotal) != parseFloat(0)) {

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "Billing/SaveBillRequest",
                dataType: 'json',
                data: {
                    "frmBill": SetDataBillPaymentService(), "itemsToPayGridModel": SetDatatblItemToPaySummary(),
                    "branchId": branchId, "preliquidationBranch": 0
                },
                success: function (data) {

                    if (data.success == false && data.result != -2) {

                        $("#alertPaymentZone").UifAlert('show', data.result, "danger");

                    } else if (data.success == false && data.result == -2) {

                        $("#alertPaymentZone").UifAlert('show', Resources.PolicyComponentsValidationMessage, "danger");
                    }
                    else {

                        if (data.BillId > 0) {

                            $('#PaymentServiceSaveBillModal').find("#ReceiptTotalAmount").text($("#_totalPay").val());
                            $('#PaymentServiceSaveBillModal').find("#ReceiptDescription").text(data.Description);
                            $('#PaymentServiceSaveBillModal').find("#BillingId").text("00000" + data.BillId);
                            $("#PaymentServiceSaveBillModal").find("#TransactionNumber").text("00000" + data.TechnicalTransaction);
                            $('#PaymentServiceSaveBillModal').find("#BillingDate").text(data.Date);
                            $('#PaymentServiceSaveBillModal').find("#ReceiptUser").text(MainPaymentServiceGetUserNick());

                            if (data.ShowMessage == "False") {

                                $("#PaymentServiceSaveBillModal").find("#accountingLabelDiv").hide();
                                $("#PaymentServiceSaveBillModal").find("#accountingMessageDiv").hide();

                            } else {
                                $("#PaymentServiceSaveBillModal").find("#accountingLabelDiv").show();
                                $("#PaymentServiceSaveBillModal").find("#accountingMessageDiv").show();
                            }

                            if (data.StatusId == 3)//se aplicó el recibo
                            {
                                // applyReceiptNumber = data.BillNumber;
                                $("#PaymentServiceSaveBillModal").find("#ApplyReceipt").hide();
                                if (data.ShowImputationMessage == "False") {

                                    $("#PaymentServiceSaveBillModal").find("#applicationIntegration").hide();
                                } else {

                                    $("#PaymentServiceSaveBillModal").find("#applicationIntegration").show();
                                }
                            }

                            $('#PaymentServiceSaveBillModal').find("#accountingMessage").val(data.Message);
                            $('#PaymentServiceSaveBillModal').find("#accountingIntegrationMessage").val(data.ImputationMessage);

                            $('#PaymentZoneDialog').UifModal('hide');
                            $('#PaymentServiceSaveBillModal').UifModal('showLocal', Resources.ReceiptSaveSuccess);

                            setDataBillEmpty();
                            ClearAll();
                        }
                    }
                }
            });
        }
    }
});

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*   BOTON IMPRESIÓN                                                                                                                         */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#PrintInvoiceButton").click(function () {

    var documentNumber = insuredDocumentNumber;
    var clientName = insuredName;

    var invoiceTotal = parseFloat(ClearFormatCurrency($("#_totalPay").val()).replace(",", ""));

    if (invoiceTotal != 0) {
        // Se valida que el total a pagar sea positivo
        if (invoiceTotal > 0) {

            var controller = ACC_ROOT + "PaymentService/LoadPrintInvoiceReport?invoiceTotal=" + invoiceTotal
                + "&invoiceClientName=" + clientName + "&invoiceClientRuc" + documentNumber;

            window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');

        }
        else {
            $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
        }
    } else {
        $("#alertMainPaymentService").UifAlert('show', Resources.MainPaymentServiceMessagePositivePay, "warning");
    }
});



//OK
function OpenBilling(branchId, datePresent, branchDescription) {
    return OpenBillingPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Billing/NeedCloseBill",
            data: { "branchId": branchId, "accountingDatePresent": datePresent },
        }).done(function (collectData) {
            resolve(collectData);
        });
    });
}


function allowOpenBill(branchId, accountingDate) {
    return allowOpenBillPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Billing/AllowOpenBill",
            data: { "branchId": branchId, "accountingDate": accountingDate },
        }).done(function (openCollectData) {
            resolve(openCollectData);
        });
    });
}



//Crea un nuevo registro en la ACC.BILL_CONTROL
function OpenBillingDialogPaymentService(branchId, date, branchDescription) {

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/SaveBillControl",
        data: {
            "branchId": branchId, "accountingDate": date
        },
        success: function (data) {

            idBillControl = data.result[0].Id;
            isOpen = true;
            $("#alertMainPaymentService").UifAlert('show', Resources.Branch + " " + branchDescription + " " + Resources.BranchOpenAutomatic, "success");

        }
    });

    setTimeout(function () {
        $("#alertMainPaymentService").UifAlert('hide');
    }, 5000);
}


//OK
function MainPaymentServiceGetUserNick() {
    var userNick;
    $.ajax({
        async: false,
        type: "GET",
        url: ACC_ROOT + "Common/GetUserNick",
        success: function (data) {
            userNick = data;
        }
    });

    return userNick;
}



function GetAccountingDate() {
    var date;
    $.ajax({
        url: ACC_ROOT + "Common/GetAccountingDate",
        success: function (data) {
            date = data;

        }
    });

    return date;
}

//Carga reporte
function loadBillingReport(branchId, billCode) {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Report/LoadBillingReport",
        data: { "branch": branchId, "billCode": billCode, "idReport": 3 },
        success: function () {

        }
    });
}


//OK
///////////////////////////////////////////////////
//  Obtiene fecha del servidor                   //
//////////////////////////////////////////////////
function getDate() {
    var systemDate;
    $.ajax({
        type: "GET",
        async: false,
        url: ACC_ROOT + "Common/GetDate",
        success: function (data) {
            systemDate = data;
        }
    });

    return systemDate;
}


//OK
function SetDataBillPaymentService() {

    var paymentsTotal = 0;

    setDataBillEmpty();

    oBillModel.BillId = 0; //autonumerico
    oBillModel.BillingConceptId = $("#ViewBagPaymentConceptPaymentService").val(); // no hay campo para esto
    oBillModel.BillControlId = idBillControl; //asigno una apertura de caja por el momento.
    var date = getDate();
    oBillModel.RegisterDate = date; //$("#BillingDate").val(); obtiene la fecha del servidor
    oBillModel.Description = "Pago realizado desde el servicio de Pago";

    paymentsTotal = ReplaceDecimalPoint(ClearFormatCurrency($("#_totalPay").val().replace(",", "")));

    oBillModel.PaymentsTotal = paymentsTotal;

    if (InsuredId != "") {

        oBillModel.PayerId = InsuredId;
    } else {

        //oBillModel.PayerId = AgentId;
        oBillModel.PayerId = GetIndividualId();
    }

    oBillModel.PayerName = "";

    var payValue = ReplaceDecimalPoint(ClearFormatCurrency($("#_totalPay").val().replace("", ",")));

    //llenar pagos
    oPaymentSummaryModel.PaymentId = 0; //autonumerico
    oPaymentSummaryModel.BillId = 0; //se lo asigna a nivel de controlador
    oPaymentSummaryModel.PaymentMethodId = typePay;
    oPaymentSummaryModel.Amount = payValue;
    oPaymentSummaryModel.LocalAmount = payValue;
    oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(getCurrencyRate($("#ViewBagAccountingDate").val(), 0)[0]);

    //llena los datos de los metodos de pago
    //cheque, transferencia, conexion directa, Recibo de Retención -- pendiente

    //tarjeta de crédito
    if (typePay == $("#ViewBagParamPaymentMethodCreditableCreditCard").val()) {

        oCreditModel.CardNumber = $('#CardPaymentDialog').find("#_numberCreditCard").val(); //$("#_numberCreditCard").val();
        oCreditModel.Voucher = $('#CardPaymentDialog').find("#_numberCreditCard").val(); //$("#_numberVoucher").val();
        oCreditModel.AuthorizationNumber = $('#CardPaymentDialog').find("#_AuthorizationNumber").val(); //$("#_AuthorizationNumber").val();
        oCreditModel.CreditCardTypeId = $('#CardPaymentDialog').find("#ConduitDrop").val(); //$("#ConduitDrop").val();
        oCreditModel.Holder = $('#CardPaymentDialog').find("#_namesCreditCard").val(); //$("#_namesCreditCard").val();
        oCreditModel.ValidThruYear = $('#CardPaymentDialog').find("#CardExpirationYear").val(); //$("#CardExpirationYear").val();
        oCreditModel.ValidThruMonth = $('#CardPaymentDialog').find("#ExpirationMonthDrop").val(); //$("#ExpirationMonthDrop").val();
        oCreditModel.IssuingBankId = issuingBankIdPaymentService;

        oPaymentSummaryModel.CreditPayments.push(oCreditModel);
    }
    //transferencias zona de pagos
    if (typePay == $("#ViewBagParamPaymentMethodPaymentArea").val()) {

        oTransferModel.DocumentNumber = $('#PaymentZoneDialog').find("#_TransferDocumentNumber").val();
        oTransferModel.IssuingBankId = issuingBankIdPaymentService; //$('#PaymentZoneDialog').find("#ddlTransferIssuingBank").val();
        oTransferModel.IssuingAccountNumber = $('#PaymentZoneDialog').find("#_TransferIssuingAccountNumber").val();
        oTransferModel.IssuerName = $('#PaymentZoneDialog').find("#_TransferHolderName").val();
        oTransferModel.ReceivingBankId = $('#PaymentZoneDialog').find("#ddlTransferReceivingBank").val();
        oTransferModel.ReceivingAccountNumber = $('#PaymentZoneDialog').find("#ddlAccountNumberReceivingBank").val();
        oTransferModel.Date = $('#PaymentZoneDialog').find("#_TransferDate").val();

        oPaymentSummaryModel.TransferPayments.push(oTransferModel);
    }

    //boleta de depósito
    if (typePay == $("#ViewBagParamPaymentMethodDepositVoucher").val()) {

        oDepositVoucherModel.VoucherNumber = $('#BankDepositDialog').find("#_numberBallot").val();
        oDepositVoucherModel.ReceivingAccountBankId = $('#BankDepositDialog').find("#ReceivingBankDrop").val();
        oDepositVoucherModel.ReceivingAccountNumber = $('#BankDepositDialog').find("#_accountNumber").val();
        oDepositVoucherModel.Date = getDate();

        oPaymentSummaryModel.DepositVouchers.push(oDepositVoucherModel);
    }

    oBillModel.PaymentSummary.push(oPaymentSummaryModel);

    return oBillModel;
}


//OK
//FUNCION OBTENER TASA DE CAMBIO
function getCurrencyRate(accountingDate, currencyId) {
    var alert = true;
    var rate=0;
    var resp = new Array();

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Common/GetCurrencyExchangeRate",
        data: JSON.stringify({ "rateDate": accountingDate, "currencyId": parseInt(currencyId) }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == 1 || data == 0) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Billing/GetLatestCurrencyExchangeRate",
                    data: JSON.stringify({ "rateDate": accountingDate, "currencyId": parseInt(currencyId) }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (dataRate) {
                        if (dataRate == 0 || dataRate == 1) {
                            rate = 1;
                            exchangeRate = rate;
                            alert = true;
                        } else {
                            rate = dataRate;
                            exchangeRate = rate;
                            alert = false;
                        }
                    }
                });

            } else {
                rate = data;
                alert = true;
            }
        }
    });

    resp[0] = rate;
    resp[1] = alert;

    return resp;
}

function GetIndividualId() {

    var ids = $("#PoliciesTable").UifDataTable("getSelected");

    if (ids.length > 0) {
        for (var i in ids) {
            var rowid = ids[i];

            payerIndividualId = rowid.PayerIndividualId;
        }
    }

    return payerIndividualId;
}

//OK
function SetDatatblItemToPaySummary() {

    //Carga de datos de la grilla tblPayToItemsSumary al Objeto respectivo
    var ids = $("#PoliciesTable").UifDataTable("getSelected");

    if (ids.length > 0) {
        for (var i in ids) {

            var rowid = ids[i];
            oBillItem = {
                BillItemId: 0,
                BillId: 0,
                ItemTypeId: 0,
                Description: "",
                Amount: 0,
                PaidAmount: 0,
                CurrencyId: 0,
                ExchangeRate: 0,
                Column1: "",
                Column2: "",
                Column3: "",
                Column4: "",
                Column5: "",
                Column6: "",
                Column7: ""
            };

            oBillItem.BillItemId = 0;
            oBillItem.BillId = 0;//rowid.BillId;
            oBillItem.Description = "No: " + rowid.PolicyDocumentNumber + " Endoso No: " + rowid.EndorsementDocumentNumber + " Cuota No: " + rowid.PaymentNumber;

            oBillItem.Amount = ReplaceDecimalPoint(ClearFormatCurrency(rowid.PaymentAmount).replace(",", ""));
            oBillItem.PaidAmount = ReplaceDecimalPoint(ClearFormatCurrency(rowid.PaymentAmount).replace(",", ""));

            oBillItem.CurrencyId = rowid.CurrencyId;

            if (rowid.ExchangeRate > 0) {
                oBillItem.ExchangeRate = rowid.ExchangeRate; //incluir en el archivo
            } else {
                oBillItem.ExchangeRate = ReplaceDecimalPoint(getCurrencyRate($("#ViewBagAccountingDate").val(), rowid.CurrencyId)[0].toString().replace(".", ","));
            }
            oBillItem.Column1 = rowid.PolicyId;
            oBillItem.Column2 = rowid.EndorsementId;
            oBillItem.Column3 = rowid.PaymentNumber;
            oBillItem.Column4 = rowid.PayerId;

            oItemsToPayGrid.BillItem.push(oBillItem);
        }
    }

    return oItemsToPayGrid;
}


//OK
//funcion para convertir una fecha de formato Json a fecha corta.
function parseJsonDate(jsonDateString) {

    var day;
    var month;
    var year;
    var isoDate;

    isoDate = new Date(parseInt(jsonDateString.replace('/Date(', '')));
    day = ("0" + isoDate.getDate()).slice(-2);
    month = ("0" + (isoDate.getMonth() + 1)).slice(-2);
    year = isoDate.getFullYear();

    return day + "/" + month + "/" + year;
}




function fileSelect(id, e) {
    var path = document.getElementById("file_upfile").value;
}

function forceDownload($archivo) {
    $mime = 'application/force-download';
    header('Pragma: public');
    header('Expires: 0');
    header('Cache-Control: must-revalidate, post-check=0, pre-check=0');
    header('Cache-Control: private', false);
    header('Content-Type: '.$mime);
    header("Content-Disposition: attachment; filename='" + basename($archivo) + "'");
    header('Content-Transfer-Encoding: binary');
    header('Connection: close');
    readfile($archivo);
    exit();
}

function UpdateForm(modelObject) {
    if ($('#hidden-form').length < 1) {
        $('<form>').attr({
            method: 'POST',
            id: 'hidden-form',
            //action: '@Url.Action("ExportFile", "PaymentService")'
            action: ACC_ROOT + '("ExportFile", "PaymentService")'
        }).appendTo('body');
    }
    $('#hidden-form').html('');
    for (var propertyName in modelObject) {
        $('<input>').attr({
            type: 'hidden',
            id: propertyName,
            name: propertyName,
            value: modelObject[propertyName]
        }).appendTo('#hidden-form');
    }
}



//OK
function ValidateFileAgentPaymentService() {

    var validFilesTypes = ["csv", "xml", "xls", "xlsx", "txt"];
    var file = document.getElementById("uploadAgentFile");
    var path = file.value;
    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
    var isValidFile = false;
    for (var i = 0; i < validFilesTypes.length; i++) {
        if (ext == validFilesTypes[i]) {
            isValidFile = true;
            break;
        }
    }
    if (!isValidFile) {
        $('#_informationData').val('');
        $('#_fileLocation').val('');

        $("#alertMainPaymentService").UifAlert('show', Resources.IncorrectFileType, "warning");

        ClearGridData();
    }
    return isValidFile;
}

//
function uploadAjaxAgentPaymentService() {

    var inputFileImage = document.getElementById("uploadAgentFile");
    var file = inputFileImage.files[0];
    var data = new FormData();

    data.append('uploadedFile', file);

    var url = "ReadFileInMemory";

    $.ajax({
        url: url,
        type: 'POST',
        contentType: false,
        data: data,
        processData: false,
        cache: false,
        success: function (data) {
            ClearGridData();

            if (data == "FormatException") {

                $("#alertMainPaymentService").UifAlert('show', Resources.WrongFormatBlankRows + " / " + Resources.WrongDateFormat, "warning");

            } else if (data == "OverflowException") {

                $("#alertMainPaymentService").UifAlert('show', Resources.WrongFormatBlankColumns, "warning");

            } else if (data == "IndexOutOfRangeException") {

                $("#alertMainPaymentService").UifAlert('show', Resources.FileDoesNotHaveColumnsNeeded, "warning");

            } else if (data == "InvalidCastException") {

                $("#alertMainPaymentService").UifAlert('show', Resources.EmptyFile + " / " + Resources.FileDoesNotHaveColumnsNeeded, "warning");

            } else if (data == "NegativeId") {

                $("#alertMainPaymentService").UifAlert('show', Resources.IdsNoNegative, "warning");

            }
            else if (data == "NullReferenceException") {

                $("#alertMainPaymentService").UifAlert('show', Resources.FileDoesNotHaveColumnsNeeded, "warning");

            }
            else if (data == "XmlException") {

                $("#alertMainPaymentService").UifAlert('show', Resources.ErrorInXMLTags, "warning");
            }

            else {

                var result = data.split("/");

                $("#_fileLocation").val(result[0]);
                $("#_informationData").val(result[1]);
                AgentId = parseInt(result[2]);

                refreshGridInsured(result[0]);

            }
        }
    });
}


/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

if (year < 1000)
    year += 1900;

var day = mydate.getDay();
var month = mydate.getMonth() + 1;
var hour = mydate.getHours();
var minutes = mydate.getMinutes();

if (month < 10)
    month = "0" + month;

var daym = mydate.getDate();

if (daym < 10)
    daym = "0" + daym;

if (hour < 10)
    hour = "0" + hour;

if (minutes < 10)
    minutes = "0" + minutes;

//dateService = Resources.ProcessDate + " " + daym + "/" + month + "/" + year + "  -  " + hour + ": " + minutes;


var oItemsToPayGrid = {
    BillItem: []
};

var oBillItem = {
    BillItemId: 0,
    BillId: 0,
    ItemTypeId: 0,
    Description: "",
    Amount: 0,
    PaidAmount: 0,
    CurrencyId: 0,
    ExchangeRate: 0,
    Column1: "",
    Column2: "",
    Column3: "",
    Column4: "",
    Column5: "",
    Column6: "",
    Column7: ""
};

var oBillModel = {
    BillId: 0,
    BillingConceptId: 0,
    BillControlId: 0,
    RegisterDate: null,
    Description: null,
    PaymentsTotal: 0,
    PayerId: -1,
    PaymentSummary: [],
    PayerName: ""
};

var oPaymentSummaryModel = {
    PaymentId: 0,
    BillId: 0,
    PaymentMethodId: 0,
    Amount: 0,
    CurrencyId: 0,
    LocalAmount: 0,
    ExchangeRate: 0,
    CheckPayments: [],
    CreditPayments: [],
    TransferPayments: [],
    DepositVouchers: [],
    RetentionReceipts: []
};

var oCheckModel = {
    DocumentNumber: null,
    IssuingBankId: -1,
    IssuingAccountNumber: null,
    IssuerName: null,
    Date: null
};

var oCreditModel = {
    CardNumber: null,
    Voucher: null,
    AuthorizationNumber: null,
    CreditCardTypeId: 0,
    IssuingBankId: -1,
    Holder: null,
    ValidThruYear: 0,
    ValidThruMonth: 0,
    TaxBase: 0
};

var oTransferModel = {
    DocumentNumber: null,
    IssuingBankId: -1,
    IssuingAccountNumber: null,
    IssuerName: null,
    ReceivingBankId: -1,
    ReceivingAccountNumber: null,
    Date: null
};

var oDepositVoucherModel = {
    VoucherNumber: null,
    ReceivingAccountBankId: -1,
    ReceivingAccountNumber: null,
    Date: null,
    DepositorName: null
};

var oRetentionReceiptModel = {
    BillNumber: null,
    AuthorizationNumber: null,
    SerialNumber: null,
    VoucherNumber: null,
    SerialVoucherNumber: null,
    Date: null
};

var agentInsuredId = $("#ViewBagAgentInsured").val();


$("#_totalPay").val("0");


$('#_informationData').val('');
$('#_fileLocation').val('');


/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

setTimeout(function () {

    if ($("#ViewBagBranchDescriptionPaymentService").val() == Resources.BranchNotExistPaymentService) {

        $("#alertMainPaymentService").UifAlert('show', $("#ViewBagBranchDescriptionPaymentService").val(), "danger");
        $("#InsuredDocumentNumber").attr("disabled", true);
        $("#PaymentServiceInsuredName").attr("disabled", true);
        $("#_documentNumber").attr("disabled", true);
        $("#_names").attr("disabled", true);

    } else {
        $("#InsuredDocumentNumber").attr("disabled", false);
        $("#PaymentServiceInsuredName").attr("disabled", false);
        $("#_documentNumber").attr("disabled", false);
        $("#_names").attr("disabled", false);

        setBranchPaymentService($("#ViewBagBranchPaymentService").val());
        
        if (branchId > 0) {

            OpenBilling(branchId, $("#ViewBagDateAccounting").val(), $("#ViewBagBranchDescriptionPaymentService").val());

            OpenBillingPromise.then(function (collectData) {
                if (collectData[0].Id == 0) {

                    allowOpenBill(branchId, $("#ViewBagDateAccounting").val());
                    allowOpenBillPromise.then(function (openCollectData) {
                        if (openCollectData[0].resp == true) {
                            OpenBillingDialogPaymentService(branchId, $("#ViewBagAccountingDate").val(), $("#ViewBagBranchDescriptionPaymentService").val());
                        } else {
                            isOpen = true;
                        }
                    });
                }
                else {
                    idBillControl = collectData[0].Id;
                }
            });
        }
        setRadio();
    }

}, 1500);




$('.btn_showpath').click(function () {
    var getpath = $('#file_upfile').val();
    $('.p_upfilepath').slideUp(function () {
        $('.p_upfilepath').text('"' + getpath + '"').slideDown();
    });

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "PaymentService/UploadFileInServer",
        data: { "fileName": "" },
        success: function (data) {

        }
    });
});


$("#ReceivingBankDrop").change(function () {
    if ($("#ReceivingBankDrop").val() != -1 && $("#_accountNumber").val() != "" && $("#_numberBallot").val() != "") {
        $("#_printReceipt").show();
    }
});


$("#btnPayCard").click(function () {
    typePay = $("#ViewBagParamPaymentMethodCreditableCreditCard").val();
    $("#_printReceipt").show();
});



//boton de generaciòn de file a Excel
$('#btnExcel').on("click", function () {

    if ($('#_documentNumber').val() == '' || $('#_names').val() == '') {

        $("#alertMainPaymentService").UifAlert('show', Resources.ValidationRequired, "warning");

    } else {
        generateFile();
    }
});


function generateFile() {

    var url = ACC_ROOT;
    url = url + "PaymentService/GeneratePremiumReceivablePolicy?insuredId=" + InsuredId + "&agentId=" +
        AgentIdSearch + "&pathFile=" + $("#_fileLocation").val();

    var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');

    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
}


/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE GRIDS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/

//OK
function setRadio() {
    //$("#cata").hide();

    if (agentInsuredId == 1) {
        $("[name=TipoPersona]").filter("[value='1']").prop("checked", true);

        //$("#cPersonType").trigger('change');
        $("#_radioAgent").trigger('change');

        if ($("#_radioAgent").is(':checked')) {
            $("#agentNumber").show();
            $("#agentName").show();
            $("#agentFileLocation").show();
            $("#agentFile").show();
            $("#insuredNumber").hide();
            $("#insuredName").hide();
            $("#information").text(Resources.MainPaymentServiceInformationAgent);
        }
    }
    else {
        $("[name=TipoPersona]").filter("[value='2']").prop("checked", true);
        //$("#cPersonType").trigger('change');
        $("#_radioInsured").trigger('change');

        if ($("#_radioInsured").is(':checked')) {
            $("#agentNumber").hide();
            $("#agentName").hide();
            $("#agentFileLocation").hide();
            $("#agentFile").hide();
            $("#insuredNumber").show();
            $("#insuredName").show();
            //$("#information").text(Resources.MainPaymentServiceInformationInsured);

        }
    }
}


$('#imageform').submit(function () {
    $(this).ajaxSubmit(options);
    // return false to prevent standard browser submit and page navigation
    return false;
});

function setBranchPaymentService(branchPaymentService) {
    branchId = parseInt(branchPaymentService);
    return branchId;
}

$("#PreviousPage").click(function () {
    $("#PreviousPage").attr("disabled", true);
    $("#NextPage").attr("disabled", true);

    if (pageNumber == 1) {
        $("#PreviousPage").attr("disabled", true);
        $("#NextPage").attr("disabled", false);

    } else {
        pageNumber--;
        refreshGridInsured("");
    }
});



$("#NextPage").click(function () {
    $("#PreviousPage").attr("disabled", true);
    $("#NextPage").attr("disabled", true);

    if (totRegisters == 0) {
        $("#PreviousPage").attr("disabled", false);
        $("#NextPage").attr("disabled", true);
    } else {
        pageNumber++;
        refreshGridInsured("");
    }
});


