var tempImputationId = $("#ViewBagTempImputationId").val();
var dateAccounting = $("#ViewBagDateAccounting").val();

var InsuredId = "";
var PayerId = "";
var AgentId = "";

var groupNumberSelect = false;
var groupNameSelect = false;

var groupIdControl = "";
var groupIdLast = "";
var groupNameLast = "";

var payerIdControl = "";
var payerIdLast = "";
var payerNameLast = "";


var insuredIdControl = "";
var insuredIdLast = "";
var insuredNameLast = "";

var agentIdControl = "";
var agentIdLast = "";
var agentNameLast = "";



var rowItemApply;
var IsMainSearchBills = 1;
var DiscountedCommisson = 0;
var PayableAmount = 0;
var amount = 0;
var usedAmount = 0;
var currentEditIndex = 0;
var depositPremiumRow = "";
var validationPolicyComponentsPromise;
var echangeRatePromise;

var echangeRate = 0;
var currencyCode = 0;

var oPremiumReceivableModel = {
    ImputationId: 0,
    IsDiscountedCommisson: false,
    PremiumReceivableItems: []
};

var oPremiumReceivableItemModel = {
    PremiumReceivableItemId: 0,
    PolicyId: 0,
    EndorsementId: 0,
    PaymentNum: 0,
    PaymentAmount: 0,
    PayerId: 0,
    IncomeAmount: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    Amount: 0,
    UserId: 0,
    RegisterDate: null,
    DiscountedCommisson: 0
};

var oUsedDepositPremiumModel = {
    PremiumReceivableItemId: 0,
    UsedAmounts: []
};

var oUsedDepositPremiumAmountModel = {
    UsedDepositPremiumId: 0,
    DepositPremiumTrasactionId: 0,
    Amount: 0
};

class AdvancedSearchPolicies extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //dropDownSearchAdvPolicy = uif2.dropDown({
        //    source: ACC_ROOT + '/AdvancedSearchPolicies/AdvancedSearchPolicies',
        //    element: '#btnSearchAdvPolicy',
        //    align: 'right',
        //    width: 600,
        //    height: 500,
        //    //loadedCallback: this.AdvancedSearchEventsPerson
        //});







        /*---------------------------------------------------------------------------------------------------------------------------------*/
        /*                                                     DEFINICION DE FUNCIONES                                                     */
        /*---------------------------------------------------------------------------------------------------------------------------------*/

        if ($("#ViewBagBranchDisable").val() == "1") {
            setTimeout(function () {
                $("#BranchSearchDrop").attr("disabled", "disabled");
            }, 300);

        }
        else {
            $("#BranchSearchDrop").removeAttr("disabled");
        }
        function LoadSearchByForPolicies() {
            DialogSearchPoliciesRequest.GetLoadSearchByForPolicies().done(function (data) {
                $("#SearchBy").UifSelect({ sourceData: data.data });
            });
        }
        function LoadBranch() {
            DialogSearchPoliciesRequest.GetLoadBranch().done(function (data) {
                $("#BranchSearchDrop").UifSelect({ sourceData: data.data });
            });
        }
        function LoadPrefix() {
            DialogSearchPoliciesRequest.GetLoadPrefix().done(function (data) {
                $("#PrefixDrop").UifSelect({ sourceData: data.data });
            });
        }

        /*---------------------------------------------------------------------------------------------------------------------------------*/
        /*                                                        ACCIONES / EVENTOS                                                       */
        /*---------------------------------------------------------------------------------------------------------------------------------*/

        var deletePremiums = function (deferred, data) {

            deferred.resolve();
            $("#alertPrimeApply").UifAlert('hide');

            //Valida el orden en que se eliminan las cuotas
            var resultQuota = deleteQuotaOrderControlPremiums(data.BranchPrefixPolicyEndorsement, data.PaymentNumber);

            if (resultQuota) {

                $("#SelecEditPartial").hide();
                $("#DepositPremiumsDialog").hide();
                $("#CommissionRetainedDialog").hide();

                delRow(data.PremiumReceivableItemId);

            } else {
                $("#alertPrimeApply").UifAlert('show', Resources.QuotaSequenceDelete, "warning");
            }

            refreshApplyView();
        };


        //ELEMENTOS DEL FORMULARIO
        $('#editAction').on('Save', function () {

            $("#alertDepositPremiumsDialog").UifAlert('hide');
            var amount = parseFloat(ClearFormatCurrency(depositPremiumRow.TotalAmount.replace("", ",")));
            var usedAmount = parseFloat(ClearFormatCurrency(depositPremiumRow.UsedAmount.replace("", ",")));

            if (usedAmount > amount) {

                $("#alertDepositPremiumsDialog").UifAlert('show', Resources.PaymentExcessValidation, "warning");

            }
            else if (usedAmount > parseFloat(ClearFormatCurrency($("#DepositAmountPaymentBalance").val().replace("", ",")))) {

                $("#alertDepositPremiumsDialog").UifAlert('show', Resources.PaymentBalanceExcessValidation, "warning");
            }
            else {
                var rowModel = new RowModelSearch();

                rowModel.DepositPremiumTransactionId = depositPremiumRow.DepositPremiumTransactionId;
                rowModel.BillId = depositPremiumRow.BillId;
                rowModel.RegisterDate = depositPremiumRow.RegisterDate;
                rowModel.CurrencyId = depositPremiumRow.CurrencyId;
                rowModel.CurrencyDescription = depositPremiumRow.CurrencyDescription;
                rowModel.TotalAmount = depositPremiumRow.TotalAmount;
                rowModel.UsedAmount = FormatCurrency(FormatDecimal($("#UsedAmountForm").val()));;

                $('#DepositPremiumsTable').UifDataTable('editRow', rowModel, currentEditIndex);

                $("#editForm").formReset();
                $('#editAction').UifInline('hide');

                totalAmountSearch();
                totalUsedAmount();
            }
        });


        $('#editAction').on('Next', function () {
            $('#DepositPremiumsTable').UifDataTable("next");
        });

        $('#editAction').on('Previous', function () {
            $('#DepositPremiumsTable').UifDataTable("previous");
        });


        //HIDE ------------------------------------------------------------------------------

        $("#SearchAdvancedSection").hide();
        $("#SelecEditPartial").hide();
        $("#DepositPremiumsDialog").hide();
        $("#CommissionRetainedDialog").hide();
        $("#DepositPremiumsDiv").hide();


        //RUN FUNCTIONS ------------------------------------------------------------------------------


        if ($("#ViewBagDepositPrimes").val() == 'true') {

            //habilita para EE
            $("#DepositAmountUsePaymentBalanceCheck").removeAttr("disabled");

        } else {
            //deshabilita para BE
            $("#DepositAmountUsePaymentBalanceCheck").attr("disabled", true);
        }


        var confirmUncheckDepositPrimeUse = function () {
            $.UifDialog('confirm', {
                'message': Resources.DialogMovementPrimeDepositConfirmationMessage,
                'Resources.PremiumDepositLabel': 'Resources.DialogMovementPrimeDepositConfirmationMessage'
            },
                function (result) {
                    if (result) {

                        deleteTempUPD(parseInt(rowItemApply.PremiumReceivableItemId));
                        $("#DepositAmountPayingAmount").val(0);
                        $("#DepositPremiumsHeader").slideDown("slow");
                        $("#DepositPremiumsDiv").slideUp("slow");

                        $("#DepositAmountUsePaymentBalance").removeAttr('checked');
                    }
                });
        };



    }

    searchBy() {
        switch ($("#SearchBy").val()) {
            case "":
                AdvancedSearchPolicies.labelHide();
                ClearSearchFields();
                HideFields();
                $("#policySearch").hide();
                break;
            case "1":
                $("#ItemToPayView").UifListView();
                labelHide();
                InsuredId = "";
                PayerId = "";
                AgentId = "";
                $("#insuredSearch").show();

                $("#PayerDocumentNumber").val("");
                $("#PayerName").val("");
                $("#AgentDocumentNumberDialog").val("");
                $("#AgentNameDialog").val("");
                $("#GroupNumber").val("");
                $("#GroupName").val("");
                $("#PolicyDocumentNumber").val("");
                $("#PolicyText").val("");
                $("#SaleTicketDocumentNumber").val("");
                $("#SaleTicketName").val("");
                $("#BranchSearchDrop").val("");
                $("#PrefixDrop").val("");
                $("#Endorsement").val("");
                $("#PayExpDateQuotaFrom").val("");
                $("#PayExpDateQuotaTo").val("");

                $("#SearchAdvancedSection").show();
                $("#policySearch").hide();
                break;
            case "2":
                $("#ItemToPayView").UifListView();
                labelHide();
                InsuredId = "";
                PayerId = "";
                AgentId = "";
                $("#payerSearch").show();

                $("#InsuredDocumentNumber").val("");
                $("#InsuredName").val("");
                $("#AgentDocumentNumberDialog").val("");
                $("#AgentNameDialog").val("");
                $("#GroupNumber").val("");
                $("#GroupName").val("");
                $("#PolicyDocumentNumber").val("");
                $("#PolicyText").val("");
                $("#SaleTicketDocumentNumber").val("");
                $("#SaleTicketName").val("");
                $("#BranchSearchDrop").val("");
                $("#PrefixDrop").val("");
                $("#_Endorsement").val("");
                $("#PayExpDateQuotaFrom").val("");
                $("#PayExpDateQuotaTo").val("");

                $("#SearchAdvancedSection").show();
                $("#policySearch").hide();

                break;
            case "3":
                $("#ItemToPayView").UifListView();
                labelHide();
                InsuredId = "";
                PayerId = "";
                AgentId = "";
                $("#agentSearch").show();

                $("#InsuredDocumentNumber").val("");
                $("#InsuredName").val("");
                $("#PayerDocumentNumber").val("");
                $("#PayerName").val("");
                $("#GroupNumber").val("");
                $("#GroupName").val("");
                $("#PolicyDocumentNumber").val("");
                $("#PolicyText").val("");
                $("#SaleTicketDocumentNumber").val("");
                $("#SaleTicketName").val("");
                $("#BranchSearchDrop").val("");
                $("#PrefixDrop").val("");
                $("#Endorsement").val("");
                $("#PayExpDateQuotaFrom").val("");
                $("#PayExpDateQuotaTo").val("");

                $("#SearchAdvancedSection").show();
                $("#policySearch").hide();
                break;
            case "4":
                $("#ItemToPayView").UifListView();
                labelHide();
                InsuredId = "";
                PayerId = "";
                AgentId = "";
                $("#groupSearch").show();

                $("#InsuredDocumentNumber").val("");
                $("#InsuredName").val("");
                $("#PayerDocumentNumber").val("");
                $("#PayerName").val("");
                $("#AgentDocumentNumberDialog").val("");
                $("#AgentNameDialog").val("");
                $("#PolicyDocumentNumber").val("");
                $("#PolicyText").val("");
                $("#SaleTicketDocumentNumber").val("");
                $("#SaleTicketName").val("");
                $("#BranchSearchDrop").val("");
                $("#PrefixDrop").val("");
                $("#Endorsement").val("");
                $("#PayExpDateQuotaFrom").val("");
                $("#PayExpDateQuotaTo").val("");

                $("#SearchAdvancedSection").hide();
                $("#policySearch").hide();

                break;
            case "5":  //poliza
                $("#ItemToPayView").UifListView();
                labelHide();
                InsuredId = "";
                PayerId = "";
                AgentId = "";
                $("#policySearch").show();

                $("#InsuredDocumentNumber").val("");
                $("#InsuredName").val("");
                $("#PayerDocumentNumber").val("");
                $("#PayerName").val("");
                $("#AgentDocumentNumberDialog").val("");
                $("#AgentNameDialog").val("");
                $("#GroupNumber").val("");
                $("#GroupName").val("");
                $("#SaleTicketDocumentNumber").val("");
                $("#SaleTicketName").val("");
                $("#BranchSearchDrop").val("");
                $("#PrefixDrop").val("");
                $("#Endorsement").val("");
                $("#PayExpDateQuotaFrom").val("");
                $("#PayExpDateQuotaTo").val("");

                $("#policySearch").show();
                $("#SearchAdvancedSection").show();

                break;
            case "6":
                $("#ItemToPayView").UifListView();
                labelHide();
                InsuredId = "";
                PayerId = "";
                AgentId = "";
                $("#salesTicketSearch").show();

                $("#InsuredDocumentNumber").val("");
                $("#InsuredName").val("");
                $("#PayerDocumentNumber").val("");
                $("#PayerName").val("");
                $("#AgentDocumentNumberDialog").val("");
                $("#AgentNameDialog").val("");
                $("#GroupNumber").val("");
                $("#GroupName").val("");
                $("#PolicyDocumentNumber").val("");
                $("#PolicyText").val("");
                $("#BranchSearchDrop").val("");
                $("#PrefixDrop").val("");
                $("#Endorsement").val("");
                $("#PayExpDateQuotaFrom").val("");
                $("#PayExpDateQuotaTo").val("");
                $("#SearchAdvancedSection").hide();
                $("#policySearch").hide();
                break;
        }
    }

    bindEvents() {
        $("#SearchBy").on('itemSelected', function (event, selectedItem) {
            searchBy();
        });
        //*****************************************************************************************************
        //AUTOCOMPLETES
        //*****************************************************************************************************

        //Asegurado
        $("#ModalPremiums").find('#InsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {
            InsuredId = selectedItem.Id;
            if (InsuredId > 0) {
                $("#InsuredDocumentNumber").val(selectedItem.DocumentNumber);
                $("#InsuredName").val(selectedItem.Name);
                insuredIdControl = "S";
                insuredIdLast = $("#InsuredDocumentNumber").val();
            }
            else {
                $('#InsuredDocumentNumber').val('');
                $('#InsuredName').val('');
                insuredIdControl = "";
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#InsuredDocumentNumber').on('blur', function (event) {
            if (insuredIdLast != $("#PayerDocumentNumber").val()) {
                if (insuredIdControl == "") {
                    InsuredId = 0;
                }
            }
            else {
                insuredIdControl = "";
            }

            if ((InsuredId == 0) || ($("#InsuredDocumentNumber").val() == "")) {
                $('#InsuredDocumentNumber').val('');
                $('#InsuredName').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#InsuredName').on('itemSelected', function (event, selectedItem) {
            InsuredId = selectedItem.Id;
            if (InsuredId > 0) {
                $("#InsuredDocumentNumber").val(selectedItem.DocumentNumber);
                $("#InsuredName").val(selectedItem.Name);
                insuredIdControl = "S";
                insuredNameLast = $("#InsuredName").val();
            }
            else {
                $('#InsuredDocumentNumber').val('');
                $('#InsuredName').val('');
                insuredIdControl = "";
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#InsuredName').on('blur', function (event) {
            if (insuredNameLast != $("#InsuredName").val()) {
                if (insuredIdControl == "") {
                    InsuredId = 0;
                }
            }
            else {
                insuredIdControl = "";
            }

            if ((InsuredId == 0) || ($("#InsuredName").val() == "")) {
                $('#InsuredDocumentNumber').val('');
                $('#InsuredName').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        //Pagador
        $("#ModalPremiums").find('#PayerDocumentNumber').on('itemSelected', function (event, selectedItem) {
            PayerId = selectedItem.Id;
            if (PayerId > 0) {
                $("#PayerDocumentNumber").val(selectedItem.DocumentNumber);
                $("#PayerName").val(selectedItem.Name);
                payerIdControl = "S";
                payerIdLast = $("#PayerDocumentNumber").val();
            }
            else {
                $('#PayerDocumentNumber').val('');
                $('#PayerName').val('');
                payerIdControl = "";
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#PayerDocumentNumber').on('blur', function (event) {
            if (payerIdLast != $("#PayerDocumentNumber").val()) {
                if (payerIdControl == "") {
                    PayerId = 0;
                }
            }
            else {
                payerIdControl = "";
            }
            if ((PayerId == 0) || ($("#PayerDocumentNumber").val() == "")) {
                $('#PayerDocumentNumber').val('');
                $('#PayerName').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#PayerName').on('itemSelected', function (event, selectedItem) {
            PayerId = selectedItem.Id;
            if (PayerId > 0) {
                $("#PayerDocumentNumber").val(selectedItem.DocumentNumber);
                $("#PayerName").val(selectedItem.Name);
                payerIdControl = "S";
                payerNameLast = $("#PayerName").val();
            }
            else {
                $('#PayerDocumentNumber').val('');
                $('#PayerName').val('');
                payerIdControl = "";
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#PayerName').on('blur', function (event) {
            if (payerNameLast != $("#PayerName").val()) {
                if (payerIdControl == "") {
                    PayerId = 0;
                }
            }
            else {
                payerIdControl = "";
            }
            if ((PayerId == 0) || ($("#PayerName").val() == "")) {
                $('#PayerDocumentNumber').val('');
                $('#PayerName').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        //Agente
        $("#ModalPremiums").find('#AgentDocumentNumberDialog').on('itemSelected', function (event, selectedItem) {
            AgentId = selectedItem.AgentId;
            if (AgentId > 0) {
                $("#AgentDocumentNumberDialog").val(selectedItem.DocumentNumber);
                $("#AgentNameDialog").val(selectedItem.Name);
                agentIdControl = "S";
                agentIdLast = $("#AgentDocumentNumberDialog").val();
            }
            else {
                $('#AgentDocumentNumberDialog').val('');
                $('#AgentNameDialog').val('');
                agentIdControl = "";
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#AgentDocumentNumberDialog').on('blur', function (event) {
            if (agentIdLast != $("#AgentDocumentNumberDialog").val()) {
                if (agentIdControl == "") {
                    AgentId = 0;
                }
            }
            else {
                agentIdControl = "";
            }
            if ((AgentId == 0) || ($("#AgentDocumentNumberDialog").val() == "")) {
                $('#AgentDocumentNumberDialog').val('');
                $('#AgentNameDialog').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#AgentNameDialog').on('itemSelected', function (event, selectedItem) {
            AgentId = selectedItem.AgentId;
            if (AgentId > 0) {
                $("#AgentDocumentNumberDialog").val(selectedItem.DocumentNumber);
                $("#AgentNameDialog").val(selectedItem.Name);
                agentIdControl = "S";
                agentNameLast = $("#AgentNameDialog").val();
            }
            else {
                $('#AgentDocumentNumberDialog').val('');
                $('#AgentNameDialog').val('');
                agentIdControl = "";
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#AgentNameDialog').on('blur', function (event) {

            if (agentNameLast != $("#AgentNameDialog").val()) {
                if (agentIdControl == "") {
                    AgentId = 0;
                }
            }
            else {
                agentIdControl = "";
            }

            if ((AgentId == 0) || ($("#AgentNameDialog").val() == "")) {
                $('#AgentDocumentNumberDialog').val('');
                $('#AgentNameDialog').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        //Grupo
        $("#ModalPremiums").find('#GroupNumber').focusin(function () {
            groupNumberSelect = true;
            groupNameSelect = false;
        });

        $("#ModalPremiums").find('#GroupName').focusin(function () {
            groupNameSelect = true;
            groupNumberSelect = false;
        });

        $("#ModalPremiums").find('#GroupNumber').on('itemSelected', function (event, selectedItem) {
            $("#GroupNumber").val(selectedItem.Id);
            if ($("#GroupNumber").val() > 0) {
                $("#GroupName").val(selectedItem.Description);
                groupIdControl = "S";
                groupIdLast = $("#GroupNumber").val();
            }
            else {
                $('#GroupNumber').val('');
                $('#GroupName').val('');
                groupIdControl = "";
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#GroupNumber').on('blur', function (event) {
            if (groupIdLast != $("#GroupNumber").val()) {
                if (groupIdControl == "") {
                    $("#GroupNumber").val(0);
                }
            }
            else {
                groupIdControl = "";
            }
            if (($("#GroupNumber").val() == 0) || ($("#GroupNumber").val() == "")) {
                $('#GroupNumber').val('');
                $('#GroupName').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#GroupName').on('itemSelected', function (event, selectedItem) {
            $("#GroupNumber").val(selectedItem.Id);
            if ($("#GroupNumber").val() > 0) {
                $("#GroupName").val(selectedItem.Description);
                groupIdControl = "S";
                groupNameLast = $("#GroupName").val();
            }
            else {
                $('#GroupNumber').val('');
                $('#GroupName').val('');
                groupIdControl = "";
                $("#ItemToPayView").UifListView();
            }

        });

        $("#ModalPremiums").find('#GroupName').on('blur', function (event) {
            if (groupNameLast != $("#GroupName").val()) {
                if (groupIdControl == "") {
                    $("#GroupNumber").val(0);
                }
            }
            else {
                groupIdControl = "";
            }
            if (($("#GroupNumber").val() == 0) || ($("#GroupNumber").val() == "")) {
                $('#GroupNumber').val('');
                $('#GroupName').val('');
                $("#ItemToPayView").UifListView();
            }
        });

        $("#ModalPremiums").find('#DepositAmountPayingAmount').on('blur', function (event) {
            var amount = ClearFormatCurrency($("#DepositAmountPayingAmount").val());
            $("#DepositAmountPayingAmount").val("$ " + NumberFormatSearch(amount, "2", ".", ","));
        });


        //DATES ---------------------------------------------------------------------------------------
        $("#ModalPremiums").find("#PayExpDateQuotaFrom").blur(function () {
            $("#ModalPremiums").find("#alertPrime").UifAlert('hide');
            if ($("#ModalPremiums").find("#PayExpDateQuotaFrom").val() != '') {
                if (IsDate($("#ModalPremiums").find("#PayExpDateQuotaFrom").val()) == true) {
                    if ($("#ModalPremiums").find("#PayExpDateQuotaTo").val() != '') {
                        if (CompareDates($("#ModalPremiums").find("#PayExpDateQuotaFrom").val(), $("#ModalPremiums").find("#PayExpDateQuotaTo").val())) {
                            $("#ModalPremiums").find("#PayExpDateQuotaFrom").val(getCurrentDate);
                        }
                    }
                } else {
                    $("#ModalPremiums").find("#alertPrime").UifAlert('show', Resources.InvalidDates, "warning");
                    $("#ModalPremiums").find("#PayExpDateQuotaFrom").val("");
                }
            }
        });

        //Valida que no ingresen una fecha invalida.
        $("#ModalPremiums").find("#PayExpDateQuotaTo").blur(function () {
            $("#ModalPremiums").find("#alertPrime").UifAlert('hide');
            if ($("#ModalPremiums").find("#PayExpDateQuotaTo").val() != '') {
                if (IsDate($("#ModalPremiums").find("#PayExpDateQuotaTo").val()) == true) {
                    if ($("#ModalPremiums").find("#PayExpDateQuotaFrom").val() != '') {
                        if (CompareDates($("#ModalPremiums").find("#PayExpDateQuotaFrom").val(), $("#ModalPremiums").find("#PayExpDateQuotaTo").val())) {
                            $("#ModalPremiums").find("#PayExpDateQuotaTo").val(getCurrentDate);
                        }
                    }
                } else {
                    $("#ModalPremiums").find("#alertPrime").UifAlert('show', Resources.InvalidDates, "warning");
                    $("#ModalPremiums").find("#PayExpDateQuotaTo").val("");
                }
            }
        });

        //CLICK ------------------------------------------------------------------------------

        //BOTON BUSCAR
        $("#ModalPremiums").find("#SearchSearchPoliciesButton").click(function () {
            $("#alertItemToPayView").UifAlert('hide');
            if ($("#PoliciesSearchForm").valid()) {
                refreshGridSearch();
            }
            else {
                $("#ItemToPayView").UifListView();
            }
        });

        //BOTON LIMPIAR
        $("#ModalPremiums").find("#CleanSearchPoliciesButton").click(function () {
            ClearSearchFields();
            $("#SearchBy").trigger('itemSelected');
        });


        $("#ModalPremiums").find('#ItemToPayView').on('rowEdit', function (event, data, index) {
            var msj = "";
            ValidatePolicyComponents(data.PolicyId, data.EndorsementId);

            validationPolicyComponentsPromise.then(function (validationData) {
                if (validationData) {
                    var imputation = checkPremiumReceivable(data.PolicyId, data.EndorsementId, data.PaymentNumber, data.PayerId);
                    if (imputation[0] == 0) {

                        //Valida el orden en que se van añadiendo las cuotas
                        var resultQuota = quotaOrderControlPremiums(data.BranchPrefixPolicyEndorsement, data.PaymentNumber);

                        if (resultQuota) {

                            echangeRateCollect(data.CurrencyId);

                            echangeRatePromise.then(function (echangeRateData) {
                                echangeRate = echangeRateData;

                                $.ajax({
                                    async: false,
                                    type: "POST",
                                    url: ACC_ROOT + "PremiumsReceivable/SaveTempPremiumReceivableRequest",
                                    data: JSON.stringify({ "premiumReceivable": SetDataPremiumReceivable(data) }),
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function () {
                                        refreshApplyView();
                                        $("#alertItemToPayView").UifAlert('show', Resources.AddQuota, "success");
                                    }
                                });

                            });
                        } else {
                            $("#alertItemToPayView").UifAlert('show', Resources.QuotaSequenceAdded, "warning");
                        }
                    } else {
                        if (imputation[1] == true) {
                            msj = Resources.BranchPrefixPolicyEndorsement + ": " + data.BranchPrefixPolicyEndorsement + ", " +
                                Resources.Quota + ": " + data.PaymentNumber + ", " + Resources.AlreadyAppliedTemporalImputationQuotaWarning + " " +
                                Resources.Type + " " + imputation[4] + " No.: " + imputation[0] + " - " + Resources.Branch + ": " + imputation[3];// + " No.: " + imputation[2];

                        } else {
                            msj = Resources.BranchPrefixPolicyEndorsement + ": " + data.BranchPrefixPolicyEndorsement + ", " +
                                Resources.Quota + ": " + data.PaymentNumber + ", " +
                                Resources.AlreadyAppliedRealImputationQuotaWarning + " " + Resources.Type + " " +
                                imputation[4] + " No.: " + imputation[2] + " " + Resources.Branch + ": " + imputation[3];
                        }

                        $("#alertItemToPayView").UifAlert('show', msj, "warning");
                    }
                } else {
                    msj = Resources.BranchPrefixPolicyEndorsement + ": " + data.BranchPrefixPolicyEndorsement + ", " +
                        Resources.Quota + ": " + data.PaymentNumber + ", " + Resources.SinglePolicyComponentsValidationMessage + " ";

                    $("#alertItemToPayView").UifAlert('show', msj, "warning");
                }
            });
        });


        $("#ModalPremiums").find('#ItemToApplyView').on('rowEdit', function (event, data, index) {
            $("#SelecEditPartial").slideDown("slow");
            $("#DepositPremiumsDialog").slideDown("slow");
            showDepositPremiumEdit(data);
        });

        //selecciona radio comisión descontada
        $("#ModalPremiums").find('#CommissionCheck').on('click', function (event, selectedItem) {

            $("#DepositPremiumsDialog").hide();
            $("#CommissionRetainedDialog").slideDown("slow");
        });

        //selecciona radio importe a pagar
        $("#ModalPremiums").find('#AmountToPayCheck').on('click', function (event, selectedItem) {

            $("#CommissionRetainedDialog").hide();
            $("#DepositPremiumsDialog").slideDown("slow");

        });

        //presiona Aceptar en modificar prima
        $("#ModalPremiums").find("#AcceptDepositPrimeDialog").click(function () {

            if ($("#DepositAmountExcessPayment").val() == "") {
                $("#DepositAmountExcessPayment").val(0);
            }
            if ($("#DepositAmountUsePaymentBalanceCheck").is(":checked")) {
                if ($("#DepositAmountPayingAmount").val() != "") {
                    var usedAmount = totalUsedAmount();
                    var amount = parseFloat(ClearFormatCurrency($("#DepositAmountPaymentBalance").val().replace("", ",")));
                    var payingAmount = parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ",")));

                    if (usedAmount != false || usedAmount >= 0) {
                        if ((payingAmount) <= amount) {
                            let data = SetTempDepositPrimeData()
                            console.log(data)
                            $.ajax({
                                type: "POST",
                                url: ACC_ROOT + "PremiumsReceivable/SaveTempDepositPrime",
                                data: JSON.stringify({ "usedDepositPremiumModel": data }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function () {
                                    setDataFieldsEmptySearch();
                                    SetTempDepositPrimeDataEmpty();

                                    $("#DepositPremiumsDialog").slideDown("slow");
                                    refreshApplyView();
                                }
                            });
                        } else {

                            var mesj = Resources.AmountToPay + " " + " " + Resources.MightNotBeGreaterThan + " " + Resources.DialogDepositPremiumsFeeBalance;
                            $("#alertDepositPremiumsDialog").UifAlert('show', mesj, "warning");
                            $("#DepositAmountPayingAmount").val("");
                            return;
                        }
                    }

                } else {
                    var msj = Resource.ValidationSave + " " + Resources.AmountToPay;
                    $("#alertDepositPremiumsDialog").UifAlert('show', msj, "warning");
                }
                if (ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val().replace("", ",")) != "" ||
                    ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val().replace(",", ".")) == "0") {
                    DiscountedCommisson = parseFloat(ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val().replace("", ",")));
                }
                else {
                    DiscountedCommisson = parseFloat(ClearFormatCurrency(rowItemApply.DiscountedCommission.replace("", ",")));
                }

                PayableAmount = parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ",")));

                echangeRateCollect(currencyCode);

                echangeRatePromise.then(function (echangeRateData) {
                    echangeRate = echangeRateData;

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "PremiumsReceivable/SaveTempPremiumReceivableRequest",
                        data: JSON.stringify({ "premiumReceivable": SetData(false) }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            setDataFieldsEmptySearch();
                            SetTempDepositPrimeDataEmpty();
                            $("#SelecEditPartial").hide();
                            $("#DepositPremiumsDialog").hide();
                            $("#DepositPremiumsDialog").slideUp("slow");
                            refreshApplyView();
                        }
                    });
                });

            }
            else {
                if (ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val().replace("", ",")) != "" ||
                    ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val().replace("", ",")) == "0") {

                    DiscountedCommisson = parseFloat(ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val()).replace("", ","));
                } else {
                    DiscountedCommisson = parseFloat(ClearFormatCurrency(rowItemApply.DiscountedCommission.replace("", ",")));
                }

                PayableAmount = parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ",")));

                echangeRateCollect(currencyCode);

                echangeRatePromise.then(function (echangeRateData) {
                    echangeRate = echangeRateData;
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "PremiumsReceivable/SaveTempPremiumReceivableRequest",
                        data: JSON.stringify({ "premiumReceivable": SetData(false) }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            setDataFieldsEmptySearch();
                            $("#SelecEditPartial").hide();
                            $("#DepositPremiumsDialog").hide();
                            $("#DepositPremiumsDialog").slideUp("slow");
                            refreshApplyView();
                        }
                    });
                });
            }
        });


        //Botón Cancelar
        $("#ModalPremiums").find("#CancelDepositPrimeDialog").click(function () {
            setDataFieldsEmptySearch();
            $("#DepositPremiumsDialog").hide();
            $("#DepositPremiumsDialog").slideUp("slow");
        });

        $("#ModalPremiums").find('#DepositPremiumsTable').on('rowEdit', function (event, data, position) {

            depositPremiumRow = data;
            currentEditIndex = position;

            $("#editForm").find("#UsedAmountForm").val(data.UsedAmount);
            $('#editAction').UifInline('show');
        });

        //---------------en comisiones descontadas--------------------------------------------------
        //Botòn Aceptar en comisiones descontadas
        $("#ModalPremiums").find("#AcceptCommissionDialog").click(function () {

            if (parseFloat(ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val())) >
                parseFloat(ClearFormatCurrency($("#ChangeAmountPendantCommission").val()))) {

                var mesj = Resources.CannotInputValueGreatherThan + Resources.DialogCommissionRetainedOutstandingCommission;
                $("#alertCommissionRetained").UifAlert('show', mesj, "warning");

                $("#ChangeAmountDiscountedCommission").val(0);
            }
            else {
                PayableAmount = parseFloat(ClearFormatCurrency(rowItemApply.PayableAmount.replace("", ","))) +
                    parseFloat(ClearFormatCurrency(rowItemApply.ExcessPayment.replace("", ",")));

                DiscountedCommisson = parseFloat(ClearFormatCurrency($("#ChangeAmountDiscountedCommission").val().replace("", ",")));

                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "PremiumsReceivable/SaveTempPremiumReceivableRequest",
                    data: JSON.stringify({ "premiumReceivable": SetData(true) }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        setDataFieldsEmptySearch();
                        refreshApplyView();

                        $("#SelecEditPartial").hide();
                        $("#CommissionRetainedDialog").hide();
                        $("#CommissionRetainedDialog").slideUp("slow");
                    }
                });
            }
        });

        //Botòn Cancelar en comisiones descontadas
        $("#ModalPremiums").find("#CancelCommissionDialog").click(function () {
            setDataFieldsEmptySearch();
            $("#CommissionRetainedDialog").hide();
            $("#CommissionRetainedDialog").slideUp("slow");
        });

        //Click en Utilizar Primas
        $("#ModalPremiums").find("#DepositAmountUsePaymentBalanceCheck").click(function () {

            if ($("#DepositAmountUsePaymentBalanceCheck").is(":checked")) {

                $("#DepositAmountPayingAmount").val("0");

                $("#DepositPremiumsHeader").slideUp("slow");
                $("#DepositPremiumsDiv").slideDown("slow");

                var control = ACC_ROOT + "PremiumsReceivable/GetDepositPremiumTransactionByPayerId?payerId=" + rowItemApply.PayerId;
                $("#DepositPremiumsTable").UifDataTable({ source: control });

            } else {

                confirmUncheckDepositPrimeUse();
            }
        });

        //CHANGE ------------------------------------------------------------------------------

        labelHide();
    }


    labelHide() {
        $("#insuredSearch").hide();
        $("#payerSearch").hide();
        $("#agentSearch").hide();
        $("#groupSearch").hide();
        $("#policySearch").hide();
        $("#salesTicketSearch").hide();
        groupNumberSelect = false;
        groupNameSelect = false;

    }

    ShowAdvanced() {
        //TemporalAdvancedSearch.componentLoadedCallback();
        dropDownSearch.show();
        //TemporalAdvancedSearch.ClearAdvanced();
    }
    quotaOrderControlPremiums(branchPrefixPolicyEndorsement, quotaSelect) {
        var result = false;
        var chkErr = 0;
        var ids = $("#ItemToPayView").UifListView("getData");
        var toApplyIds = $("#ItemToApplyView").UifListView("getData");
        var isApply = false;

        for (var k in ids) {

            var rowData = ids[k];

            //Establecer control para evitar desbordamiento
            if (!(k < ids.length - 1)) {

                break;
            }

            var key = rowData.BranchPrefixPolicyEndorsement;

            if (key == branchPrefixPolicyEndorsement) {

                //validar si ya esta seleccionada ItemToApplyView
                for (var q in toApplyIds) {

                    var rowApply = toApplyIds[q];

                    if (key == rowApply.BranchPrefixPolicyEndorsement) {

                        if ((quotaSelect - rowApply.PaymentNumber) == 1) {
                            isApply = true;
                        } else {
                            isApply = false;
                        }
                    }
                }

                if (!isApply) {
                    if (rowData.PaymentNumber < quotaSelect) {
                        chkErr += 1;
                    }
                }
            }
        }
        if (chkErr > 0) {

            result = false;
        } else {
            result = true;
        }

        return result;
    }


    //Controla el orden en que se eliminan las cuotas ya agregadas
    deleteQuotaOrderControlPremiums(branchPrefixPolicyEndorsement, quotaSelect) {

        var result = false;
        var chkErr = 0;
        var ids = $("#ItemToApplyView").UifListView("getData");//pólizas ya seleccionadas

        for (var k in ids) {

            var rowData = ids[k];

            //Establecer control para evitar desbordamiento
            if (!(parseInt(k) < ids.length)) {
                break;
            }

            if (rowData.BranchPrefixPolicyEndorsement == branchPrefixPolicyEndorsement) {

                if (quotaSelect < rowData.PaymentNumber) {
                    chkErr += 1;
                }
            }
        }
        if (chkErr > 0) {

            result = false;
        } else {
            result = true;
        }

        return result;
    }



    totalUsedAmount() {
        var total = 0;

        var ids = $("#DepositPremiumsTable").UifDataTable('getData');

        for (i in ids) {

            var row = ids[i];

            if (row.UsedAmount == "")
                return false;

            total += parseFloat(ClearFormatCurrency(row.UsedAmount.replace("", ",")));
        }
        if (parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ","))) == 0 ||
            parseFloat(ClearFormatCurrency($("#DepositAmountPaymentBalance").val().replace("", ","))) == 0) {
            $("#DepositAmountPayingAmount").val(total);
        }
        if (parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ","))) >= total) {

            var amount = ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ","));
            $("#DepositAmountPayingAmount").val(amount);
        }
        if (parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ","))) <= total) {
            $("#DepositAmountPayingAmount").val(total);
        }
        return total;
    }

    setDataFieldsEmptySearch() {
        $("#ChangeAmountBranchName").val('');
        $("#ChangeAmountPrefix").val('');
        $("#ChangeAmountPolicy").val('');
        $("#ChangeAmountEndorsement").val('');
        $("#ChangeAmountPayment").val('');
        $("#ChangeAmountPaymentBalance").val('');
        $("#ChangeAmountDepositPrimes").val('');
        $("#ChangeAmountPayingAmount").val('');
        $("#ChangeAmountExcessPayment").val(0);
        $("#ChangeAmountAgent").val('');
        $("#ChangeAmountPendantCommission").val('');
        $("#ChangeAmountDiscountedCommission").val('');

        $("#DepositAmountUsePaymentBalanceCheck").attr('checked', false);
        PayableAmount = 0;
        DiscountedCommisson = 0;
    }

    setExcessPayment() {
        var difference = 0;

        if ($("#DepositAmountUsePaymentBalance").is(":checked") == false && $("#DepositAmountPayingAmount").val() != "") {
            if (parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ","))) >
                parseFloat(ClearFormatCurrency($("#DepositAmountPaymentBalance").val().replace("", ",")))) {
                difference = parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val().replace("", ","))) - parseFloat(ClearFormatCurrency($("#DepositAmountPaymentBalance").val().replace("", ",")));

                $("#DepositAmountPaymentBalance").val(ClearFormatCurrency($("#DepositAmountPaymentBalance").val().replace("", ",")));
                $("#DepositAmountExcessPayment").val(difference);
            } else {
                if (parseFloat(ClearFormatCurrency($("#DepositAmountPayingAmount").val())) < parseFloat(ClearFormatCurrency($("#DepositAmountPaymentBalance").val()))) {
                    $("#DepositAmountExcessPayment").val(0);
                }
            }
        }
    }

    totalAmountSearch() {
        var total = 0;
        var ids = $("#DepositPremiumsTable").UifDataTable('getData');

        for (i in ids) {

            var row = ids[i];
            total += parseFloat(ClearFormatCurrency(row.TotalAmount.replace("", ",")));
        }

        return total;
    }

    SetTempDepositPrimeDataEmpty() {
        oUsedDepositPremiumModel = {
            PremiumReceivableItemId: 0,
            UsedAmounts: []
        };

        oUsedDepositPremiumAmountModel = {
            UsedDepositPremiumId: 0,
            DepositPremiumTrasactionId: 0,
            Amount: 0
        };
    }

    SetTempDepositPrimeData() {

        oUsedDepositPremiumModel.PremiumReceivableItemId = rowItemApply.PremiumReceivableItemId;

        var idsDepositPrimeItems = $("#DepositPremiumsTable").UifDataTable('getData');
        var depositPrimeItem;

        for (var j in idsDepositPrimeItems) {
            depositPrimeItem = idsDepositPrimeItems[j];

            oUsedDepositPremiumAmountModel = {
                UsedDepositPremiumId: 0,
                DepositPremiumTrasactionId: 0,
                Amount: 0
            };

            oUsedDepositPremiumAmountModel.UsedDepositPremiumId = 0;
            oUsedDepositPremiumAmountModel.DepositPremiumTrasactionId = depositPrimeItem.DepositPremiumTransactionId;
            oUsedDepositPremiumAmountModel.Amount = parseFloat(ClearFormatCurrency(depositPrimeItem.UsedAmount).replace(",", ".").replace(" ", ""));

            oUsedDepositPremiumModel.UsedAmounts.push(oUsedDepositPremiumAmountModel);
        }

        return oUsedDepositPremiumModel;
    }

    SetDataPremiumReceivableEmpty() {
        oPremiumReceivableModel = {
            ImputationId: 0,
            PremiumReceivableItems: []
        };

        oPremiumReceivableItemModel = {
            PremiumReceivableItemId: 0,
            PolicyId: 0,
            EndorsementId: 0,
            PaymentNum: 0,
            PaymentAmount: 0,
            PayerId: 0,
            IncomeAmount: 0,
            CurrencyCode: 0,
            ExchangeRate: 0,
            Amount: 0,
            UserId: 0,
            RegisterDate: null
        };
    }

    SetData(isDiscountedCommisson) {

        SetDataPremiumReceivableEmpty();
        oPremiumReceivableModel.ImputationId = tempImputationId;
        oPremiumReceivableModel.IsDiscountedCommisson = isDiscountedCommisson;

        var itemToApplyRow = rowItemApply;

        oPremiumReceivableItemModel = {
            PremiumReceivableItemId: 0,
            PolicyId: 0,
            EndorsementId: 0,
            PaymentNum: 0,
            PaymentAmount: 0,
            PayerId: 0,
            IncomeAmount: 0,
            CurrencyCode: 0,
            ExchangeRate: 0,
            Amount: 0,
            UserId: 0,
            RegisterDate: null,
            DiscountedCommisson: 0

        };

        var itemPaymentAmount = 0;
        if (itemToApplyRow.PaymentAmount.indexOf("(") == 0) {
            itemPaymentAmount = parseFloat(ClearFormatCurrency(itemToApplyRow.PaymentAmount.replace("(", "").replace(")", "")));
            itemPaymentAmount = itemPaymentAmount * -1;
        }
        else {
            itemPaymentAmount = parseFloat(ClearFormatCurrency(itemToApplyRow.PaymentAmount.replace("", ",")));
            if (itemPaymentAmount == 0) {
                itemPaymentAmount = parseFloat(ClearFormatCurrency(itemToApplyRow.QuotaValue.replace("", ",")));
            }
        }

        var itemPayableAmount = 0;
        if (itemToApplyRow.PayableAmount.indexOf("(") == 0) {
            itemPayableAmount = parseFloat(ClearFormatCurrency(itemToApplyRow.PayableAmount.replace("(", "").replace(")", "")));
            itemPayableAmount = itemPayableAmount * -1;
        }
        else {
            itemPayableAmount = parseFloat(ClearFormatCurrency(itemToApplyRow.PayableAmount.replace("", ",")));
        }

        oPremiumReceivableItemModel.PremiumReceivableItemId = itemToApplyRow.PremiumReceivableItemId;
        oPremiumReceivableItemModel.PolicyId = itemToApplyRow.PolicyId;
        oPremiumReceivableItemModel.EndorsementId = itemToApplyRow.EndorsementId;
        oPremiumReceivableItemModel.PaymentNum = itemToApplyRow.PaymentNumber;
        oPremiumReceivableItemModel.PaymentAmount = itemPaymentAmount; //ANTES
        oPremiumReceivableItemModel.PayerId = itemToApplyRow.PayerId;
        oPremiumReceivableItemModel.IncomeAmount = PayableAmount;
        oPremiumReceivableItemModel.CurrencyCode = itemToApplyRow.CurrencyId;
        oPremiumReceivableItemModel.ExchangeRate = echangeRate;
        oPremiumReceivableItemModel.Amount = itemPayableAmount * parseFloat(echangeRate);
        oPremiumReceivableItemModel.RegisterDate = getCurrentDate();
        oPremiumReceivableItemModel.DiscountedCommisson = DiscountedCommisson;


        oPremiumReceivableModel.PremiumReceivableItems.push(oPremiumReceivableItemModel);

        return oPremiumReceivableModel;
    }

    showDepositPremiumEdit(data) {

        rowItemApply = data;
        currencyCode = data.CurrencyId;

        var paymentAmount = 0;

        if (data.PaymentAmount.indexOf("(") == 0) {
            paymentAmount = parseFloat(ClearFormatCurrency(data.PaymentAmount.replace("(", "").replace(")", "")));
            paymentAmount = paymentAmount * -1;
        }
        else {
            paymentAmount = parseFloat(ClearFormatCurrency(data.PaymentAmount.replace("", ",")));
        }

        var payableAmount = 0;

        if (data.PayableAmount.indexOf("(") == 0) {
            payableAmount = parseFloat(ClearFormatCurrency(data.PayableAmount.replace("(", "").replace(")", "")));
            payableAmount = payableAmount * -1;
        }
        else {
            payableAmount = parseFloat(ClearFormatCurrency(data.PayableAmount.replace("", ",")));
        }

        $("#DepositAmountBranchName").val(data.BranchDescription);
        $("#DepositAmountPrefix").val(data.PrefixDescription);
        $("#DepositAmountPolicy").val(data.PolicyDocumentNumber);
        $("#DepositAmountEndorsement").val(data.EndorsementDocumentNumber);
        $("#DepositAmountPayment").val(data.PaymentNumber);
        //$("#DepositAmountPaymentBalance").val(data.PaymentAmount);
        $("#DepositAmountPaymentBalance").val(payableAmount);//Cambio NAguilar 18/01/2017
        $("#DepositAmountPayingAmount").val(data.ValueToCollect);

        $("#DepositAmountExcessPayment").val(data.ExcessPayment);

        // Indica si se usaron primas en depósito al aplicar una prima por cobrar
        if (data.Upd == "1") {
            $("#DepositAmountUsePaymentBalanceCheck").prop('checked', true);
            $("#DepositAmountPayingAmount").val(paymentAmount);
        }

        $("#DepositAmountPaymentBalance").val("$ " + NumberFormatDecimal($("#DepositAmountPaymentBalance").val(), "2", ".", ","));

        $("#ChangeAmountBranchName").val(data.BranchDescription);
        $("#ChangeAmountPrefix").val(data.PrefixDescription);
        $("#ChangeAmountPolicy").val(data.PolicyDocumentNumber);
        $("#ChangeAmountEndorsement").val(data.EndorsementDocumentNumber);
        $("#ChangeAmountPayment").val(data.PaymentNumber);
        //$("#ChangeAmountPaymentBalance").val(data.PaymentAmount);
        $("#ChangeAmountPaymentBalance").val(payableAmount);//Cambio NAguilar 18/01/2017
        $("#ChangeAmountPayingAmount").val(payableAmount);
        $("#ChangeAmountExcessPayment").val('');
        $("#ChangeAmountAgent").val(data.PolicyAgentDocumentNumberName);
        $("#ChangeAmountPendantCommission").val(data.PendingCommission);
        $("#ChangeAmountDiscountedCommission").val(parseFloat(ClearFormatCurrency(data.DiscountedCommission.replace("", ","))));
    }

    labelHide() {
        $("#insuredSearch").hide();
        $("#payerSearch").hide();
        $("#agentSearch").hide();
        $("#groupSearch").hide();
        $("#policySearch").hide();
        $("#salesTicketSearch").hide();
        groupNumberSelect = false;
        groupNameSelect = false;

    }

    HideFields() {
        $("#salesTicketSearch").hide();
        $("#policySearch").hide();
        $("#groupSearch").hide();
        $("#agentSearch").hide();
        $("#payerSearch").hide();
        $("#insuredSearch").hide();
    }

    ClearSearchFields() {
        $("#SearchBy").val("");
        $("#InsuredDocumentNumber").val("");
        $("#InsuredName").val("");
        $("#PayerDocumentNumber").val("");
        $("#PayerName").val("");
        $("#AgentDocumentNumber").val("");
        $("#AgentName").val("");
        $("#GroupNumber").val("");
        $("#GroupName").val("");
        $("#PolicyDocumentNumber").val("");
        $("#PolicyText").val("");
        $("#SaleTicketDocumentNumber").val("");
        $("#SaleTicketName").val("");
        $("#BranchSearchDrop").val("");
        $("#PrefixDrop").val("");
        $("#Endorsement").val("");
        $("#PayExpDateQuotaFrom").val("");
        $("#PayExpDateQuotaTo").val("");

        $("#ItemToPayView").UifListView();

        $("#alertItemToPayView").UifAlert('hide');
        $("#alertPrimeApply").UifAlert('hide');
        $("#alertPrime").UifAlert('hide');
        $("#SelecEditPartial").hide();
        $("#DepositPremiumsDialog").hide();
        $("#CommissionRetainedDialog").hide();


    }

    ClearFieldsSearch() {
        $("#SearchBy").val("");

        $("#InsuredDocumentNumber").val("");
        $("#InsuredName").val("");
        $("#PayerDocumentNumber").val("");
        $("#PayerName").val("");
        $("#AgentDocumentNumberDialog").val("");
        $("#AgentNameDialog").val("");
        $("#GroupNumber").val("");
        $("#GroupName").val("");
        $("#PolicyDocumentNumber").val("");
        $("#PolicyText").val("");
        $("#SaleTicketDocumentNumber").val("");
        $("#SaleTicketName").val("");
        $("#BranchSearchDrop").val(-1);
        $("#PrefixDrop").val(-1);
        $("#Endorsement").val("");
        $("#PayExpDateQuotaFrom").val("");
        $("#PayExpDateQuotaTo").val("");

        $("#ItemToPayView").UifListView();
        $("#ItemToApplyView").UifListView();
    }

    refreshGridSearch() {

        $('#ModalPremiums').find("#ItemToPayView").UifListView({
            height: 400,
            theme: 'dark',
            source: ACC_ROOT + "PremiumsReceivable/PremiumReceivableSearchPolicy?insuredId=" + InsuredId + "&payerId=" +
                PayerId + "&agentId=" + AgentId + "&groupId=" + $("#GroupNumber").val() + "&policyDocumentNumber=" +
                $("#PolicyDocumentNumber").val() + "&salesTicket=" + $("#SaleTicketDocumentNumber").val() + "&branchId=" +
                $("#BranchSearchDrop").val() + "&prefixId=" + $("#PrefixDrop").val() + "&endorsementDocumentNumber=" +
                $("#Endorsement").val() + "&dateFrom=" + $("#PayExpDateQuotaFrom").val() + "&dateTo=" + $("#PayExpDateQuotaTo").val(),
            customDelete: true,
            customEdit: true,
            edit: true,
            delete: false,
            displayTemplate: "#ItemToPayViewTemplate"
        });
    }

    refreshApplyView() {

        $("#ItemToApplyView").UifListView({
            autoHeight: true,
            theme: 'dark',
            source: ACC_ROOT + "PremiumsReceivable/GetTempPremiumReceivableItemByTempImputationId?tempImputationId=" + tempImputationId,
            customDelete: false,
            customEdit: true,
            edit: true,
            delete: true,
            displayTemplate: "#ItemToApplyViewTemplate",
            deleteCallback: deletePremiums
        });
    }

    SetDataPremiumReceivable(data) {

        oPremiumReceivableModel = {
            ImputationId: 0,
            PremiumReceivableItems: []
        };

        oPremiumReceivableItemModel = {
            PremiumReceivableItemId: 0,
            PolicyId: 0,
            EndorsementId: 0,
            PaymentNum: 0,
            PaymentAmount: 0,
            PayerId: 0,
            IncomeAmount: 0,
            CurrencyCode: 0,
            ExchangeRate: 0,
            Amount: 0,
            UserId: 0,
            RegisterDate: null
        };

        oPremiumReceivableModel.ImputationId = tempImputationId;
        oPremiumReceivableItemModel = {
            PremiumReceivableItemId: 0,
            PolicyId: 0,
            EndorsementId: 0,
            PaymentNum: 0,
            PaymentAmount: 0,
            PayerId: 0,
            IncomeAmount: 0,
            CurrencyCode: 0,
            ExchangeRate: 0,
            Amount: 0,
            UserId: 0,
            RegisterDate: null
        };

        var paymentAmount = 0;

        if (data.PaymentAmount.indexOf("(") == 0) {
            paymentAmount = parseFloat(ClearFormatCurrency(data.PaymentAmount.replace("(", "").replace(")", "")));
            paymentAmount = paymentAmount * -1;
        }
        else {
            paymentAmount = parseFloat(ClearFormatCurrency(data.PaymentAmount.replace("", ",")));
        }

        oPremiumReceivableItemModel.PremiumReceivableItemId = 0;
        oPremiumReceivableItemModel.PolicyId = data.PolicyId;
        oPremiumReceivableItemModel.EndorsementId = data.EndorsementId;
        oPremiumReceivableItemModel.PaymentNum = data.PaymentNumber;
        oPremiumReceivableItemModel.PaymentAmount = paymentAmount;
        oPremiumReceivableItemModel.PayerId = data.PayerId;
        oPremiumReceivableItemModel.IncomeAmount = paymentAmount;
        oPremiumReceivableItemModel.CurrencyCode = data.CurrencyId;
        oPremiumReceivableItemModel.ExchangeRate = parseFloat(echangeRate);
        oPremiumReceivableItemModel.Amount = paymentAmount;
        oPremiumReceivableItemModel.RegisterDate = getCurrentDate();

        oPremiumReceivableModel.PremiumReceivableItems.push(oPremiumReceivableItemModel);

        return oPremiumReceivableModel;
    }

    //control para comprobar que la póliza ya ha sido utilizada en alguna aplicación.
    checkPremiumReceivable(policyId, endorsementId, paymentNum, payerIndividualId) {
        var imputation = new Array();
        $.ajax({
            async: false,
            url: ACC_ROOT + "PremiumsReceivable/CheckPremiumReceivable",
            data: {
                "policyId": policyId,
                "endorsementId": endorsementId,
                "paymentNum": paymentNum,
                "payerIndividualId": payerIndividualId
            },
            success: function (data) {

                imputation[0] = data.Id;
                imputation[1] = data.IsTemporal;
                imputation[2] = data.SourceId;
                imputation[3] = data.Branch;

                switch (data.ImputationTypeId) {
                    case 1:
                        imputation[4] = Resources.Recovery_Receipt;
                        break;
                    case 2:
                        imputation[4] = Resources.JournalEntry;
                        break;
                    case 3:
                        imputation[4] = Resources.Preliquidation;
                        break;
                    case 4:
                        imputation[4] = Resources.PaymentOrder;
                        break;
                }
            }
        });

        return imputation;
    }

    delRow(premiumReceivableItemId) {

        $.ajax({
            url: ACC_ROOT + "PremiumsReceivable/DeleteTempPremiumRecievableTransactionItem",
            data: { "tempImputationCode": tempImputationId, "tempPremiumReceivableCode": premiumReceivableItemId },
            success: function (data) {
                if (!data) {
                    $("#alertPrimeApply").UifAlert('show', Resources.PaymentItemPartialQuotaDeleteNotAllowed, "warning");
                }
            }
        });
    }

    ValidatePolicyComponents(policyId, endorsementId) {
        return validationPolicyComponentsPromise = new Promise(function (resolve, reject) {
            lockScreen();
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "PremiumsReceivable/ValidatePolicyComponents",
                    data: { "policyId": policyId, "endorsementId": endorsementId }
                }).done(function (validationData) {
                    unlockScreen();
                    resolve(validationData);
                });
            }, 500);
        });
    }

    deleteTempUPD(tempPremiumReceivableId) {
        $.ajax({
            async: false,
            url: ACC_ROOT + "PremiumsReceivable/DeleteTempUsedDepositPremiumRequest",
            data: { "tempPremiumReceivableId": tempPremiumReceivableId },
            success: function () {
            }
        });
    }

    RowModelSearch() {
        this.DepositPremiumTransactionId;
        this.BillId;
        this.RegisterDate;
        this.CurrencyId;
        this.CurrencyDescription;
        this.TotalAmount;
        this.UsedAmount;
    }


    echangeRateCollect(currencyId) {
        return echangeRatePromise = new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "PremiumsReceivable/GetEchangeRateByCollect",
                data: { "currencyCode": currencyId, "accountingDate": applyAccountingDate, "applicationCollectId": applyCollecId }
            }).done(function (echangeRateData) {
                resolve(echangeRateData);
            });
        });
    }

}