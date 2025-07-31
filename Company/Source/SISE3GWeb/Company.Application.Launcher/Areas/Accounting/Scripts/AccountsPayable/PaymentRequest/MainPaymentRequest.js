/**
    * @file   MainPaymentRequest.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var idsTax;
var paymentMethodsPromise;
var paymentMethodsTime;

var paymentOrigin = "#PaymentOrigin";
var rowModelTax = new RowModelTax();

function RowModelTax() {
    this.TaxDescription = "";
    this.TaxGroup = "";
    this.Amount = 0;
    this.Rate = 0;
    this.TaxAmount = 0;
    this.MinimumTax = 0;
}

var payToValue = "";
var changePayment = true;
var documentNumber = "";

var agentTypeId = -1;
var personCode = 0;
var paymentSourceOthers = $("#ViewBagPaymentSources").val();
var taxConditionTitle = "";
var percentageDifferencePo = 0;
var maxPo = 0;
var minPo = 0;
var rateExchangePo = 0;
var voucherType = "";
var voucherNumber = 0;
var voucherCurrency = "";
var paymentConceptTypeId = 0;
var paymentBranchId = 0;
var rate;
var index = 0;
var indexConcept = 0;
var coverageTaxId = 0;
var totalTaxValueConcept = 0;
var enableForSave = false;

var oPaymentRequestModel = {
    PaymentRequestId: 0,                   //PaymentRequestInfoId
    PaymentSourceId: 0,                    //PaymentRequestSource
    BranchId: 0,                           //PaymentRequestBranch
    PaymentEstimateDate: null,             //PaymentRequestEstimatedDate
    RegistrationDate: null,                //PaymentRequestRegistrationDate
    PaymentDate: null,                     //PaymentRequestPaymentDate
    PersonTypeId: 0,                       //PaymentRequestPersonTypeId
    IndividualId: 0,                       //PaymentRequestIndividualId
    PaymentMethodId: 0,                    //PaymentRequestPaymentMethodId
    CurrencyId: 0,                         //PaymentRequestCurrencyId
    UserId: 0,                             //PaymentRequestUserId
    IsPrinted: false,                      //PaymentRequestIsPrinted
    TotalAmount: 0,                        //PaymentRequestTotalAmount
    Description: "",                       //PaymentRequestDescription
    PaymentMovementTypeId: 0,              //PaymentRequestPaymentMovementTypeId
    PrefixId: 0,                           //PaymentRequestPrefixId
    PersonBankAccountId: 0,                //PaymentRequestAccountBankId
    PaymentRequestTypeId: 0,               //PaymentRequestType
    CompanyId: 0,                          //PaymentRequestCompanyId
    SalePointId: 0,                        //PaymentRequestSalePointId
    Vouchers: [],
    PaymentClaim: [],
    PaymentTax: [],
};


//***********************************
//TaxCategories
var pivot = 0;
var totCountTax = 0;
var pivotSet = false;
var j = 0;
var taxRateResult = 0;

var taxIds = [];
var taxGrid = false;

//Objeto para almacenar los TaxCategories
var oTax = {
    TaxCategory: []
};
var oTaxCategory = {
    TaxId: 0,
    TaxCategoryId: 0,
    TaxCategoryDescription: "", //TaxCategoryDescript
    TaxCondition: 0
};
//**********************************
//para obtener en memoria los valores de la suma

var taxModel = {
    Concept: []
};
var taxHeaderModel = {
    Index: 0,
    TaxConcept: []
};

var taxDetailModel = {
    Index: 0,
    TaxId: 0,
    TaxCategoryId: 0,
    TaxConditionId: 0,
    TaxDescription: "",
    TaxGroup: "",
    Amount: 0,
    Rate: 0,
    TaxAmount: 0,
    MinimumTax: 0
};
//***********************************

//Variable PaymentClaim.
var oPaymentClaimModel = {
    PaymentClaimId: 0,
    PaymentClaimCoverages: []
};

//Variable PaymentClaimCoverages
var oPaymentClaimCoveragesModel = {
    PaymentClaimCoveragesId: 0,
    PaymentClaimCoveragesSubClaim: 0,
    PaymentClaimCoveragesClaimAmount: []
};

//Variable para las Estimaciones
var oPaymentClaimCoveragesClaimAmountModel = {
    PaymentClaimCoveragesClaimAmountId: 0,
    PaymentClaimCoveragesClaimAmountEstimationTypeId: 0,
    PaymentClaimCoveragesClaimAmountVersion: 0,
    PaymentClaimCoveragesClaimAmountDate: null,
    PaymentClaimCoveragesClaimAmountEstimatedAmount: 0,
    PaymentClaimCoveragesClaimAmountDeductibleAmount: 0,
    PaymentClaimCoveragesClaimAmountPaymentTypeId: 0,
    PaymentClaimCoveragesClaimAmountVoucher: []
};

//Variable Voucher
var oVoucherModel = {
    Index: 0,
    VoucherId: 0,
    VoucherType: 0,
    VoucherNumber: 0,
    VoucherDate: null,
    VoucherExchangeRate: 0,
    VoucherCurrencyId: 0,
    VoucherConcepts: [] //VoucherConcept
};

//Variable VoucherConcept
var oVoucherConceptModel = {
    VoucherConceptId: 0,
    VoucherConceptPaymentConcept: 0,
    VoucherConceptValue: 0,
    VoucherConceptTaxValue: 0,
    VoucherConceptDescription: "",
    VoucherConceptRetentionValue: 0,
    VoucherConceptCostCenterId: 0,
    VoucherConceptTaxes: [],
};

//Variable PaymentTaxCategory
var oPaymentTaxCategoryModel = {
    TaxId: 0,
    TaxCategoryId: 0,
    TaxCondition: 0,
    TaxCategoryDescript: ""
};

var oVoucherConceptTax = {
    TaxId: 0,
    TaxCategoryId: 0,
    TaxConditionId: 0,
    TaxCategoryDescription: "",
    TaxRate: 0,
    TaxBase: 0,
    TaxValue: 0
};

var insuredCode = -1;

var paymentConceptId;
var totalPaymentAmount = 0;
var totalPaymentTaxes = 0;
var totalPaymentRetention = 0;

var individualId = -1;
var individualName = '';
var editIndex;
var correlativeNumber = -1;
var accountBankId = -1;

var paymentRequestId;
var exchangeRate;

var accountingAccountId = 0;
var conceptCode = 0;
var accountingAccountName = '';

var accountingPaymentConceptId = "";
var accountingConceptText = "";
var journalPayerIndividualId = 0;

function RowPaymentConceptModel() {
    this.Index;
    this.PaymentConceptId;
    this.PaymentConceptDescription;
    this.PaymentConceptIdDescription;
    this.CostCenterId;
    this.CostCenterDescription;
    this.CostCenterIdDescription;
    this.Amount;
    this.Taxes;
    this.Retentions;
    this.PercentageTaxes;
    this.PercentageRetentions;
    this.Taxid;
    this.GroupId;
    this.ConditionId;
    this.TaxSaved;
    this.TaxDontSave;
    this.treatmentId;
    this.VoucherName;

}

function RowOtherPaymentModel() {
    this.TaxId;
    this.TaxDescription;
    this.IndividualId;
    this.TaxConditionId;
    this.TaxConditionDescription;
    this.HasNationalRate;
    this.RateDescription;
}

function RowPaymentRequest() {
    this.Index;
    this.VoucherTypeId;
    this.VoucherTypeDescription;
    this.VoucherType;
    this.VoucherNumber;
    this.VoucherDate;
    this.VoucherCurrencyId;
    this.VoucherCurrency;
    this.ExchangeRate;
    this.Amount;
    this.Taxes
    this.Retentions;
    this.Id;
}

function RowTransferListView() {
    this.AccountBankId;
    this.IndividualId;
    this.BankDescription;
    this.AccountTypeDescription;
    this.Number;
    this.AccountBankEnabled;
    this.IsActive;
    this.Default;
}

function RowTaxes() {
    this.TaxId;
    this.TaxDescription;
    this.GroupCondition;
    this.GroupAmount;
    this.Rate;
    this.TaxAmount;
    this.MinimunTaxBase;
    this.MinimunTax;
    this.Branch;
}

function RowVoucher() {
    this.Index;
    this.VoucherTypeId;
    this.VoucherTypeDescription;
    this.VoucherNumber;
    this.VoucherDate;
    this.VoucherCurrencyId;
    this.VoucherCurrency;
    this.VoucherExchangeRate;
    this.PaymentConceptId;
    this.PaymentConcept;
    this.CostCenterId;
    this.CostCenter;
    this.Amount;
    this.Taxes
    this.Retentions;
    this.Id;
    this.PaymentConceptAmount;
    this.PaymentConceptTax;
    this.VoucherAmount;
}


///////////////////////////////////////////////////
//  Borra objeto al borrarlo del listView        //
///////////////////////////////////////////////////
var deletetaxesListView = function (deferred, data) {
    if (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.length > 0) {
        for (var i = 0; i <= (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.length); i++) {
            if (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher[i].Index == data.Index) {
                oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.splice(i, 1);
            }
        }
    }
    //elimino un comprobante de pago seleccionado
    deferred.resolve();
    setTimeout(function () {
        MainPaymentRequest.DelRowOtherPayments("taxesListView");
    }, 2000);

    if (taxModel.Concept.length > 0) {
        var idelete = 0;
        for (var k = 0; k <= taxModel.Concept.length; k++) {
            if (taxModel.Concept[k].Index == data.Index) {
                idelete = taxModel.Concept[k].TaxConcept.length;
                for (var j = 0; j < idelete; j++) {
                    taxModel.Concept[k].TaxConcept.splice(j, 1);
                }
                taxModel.Concept.splice(k, 1);
            }
        }
    }
};

//////////////////////////////////////////////////////*//////////
//  Borra objeto al borrarlo del listView Concepto de Pagos    //
/////////////////////////////////////////////////////////////////
var deletePaymentConcept = function (deferred, data) {
    if (taxHeaderModel.TaxConcept.length > 0) {
        for (var i = 0; i < taxHeaderModel.TaxConcept.length; i++) {
            if (taxHeaderModel.TaxConcept[i].Index == data.Index) {
                taxHeaderModel.TaxConcept.splice(i, 1);
            }
        }
    }
    deferred.resolve();
    setTimeout(function () {
        MainPaymentRequest.DelRowOtherPayments("PaymentConcetpsListView");
    }, 2000);
};



function validParmeters(conceptId, selectedItem) {

    lockScreen(); 

    setTimeout(function () {
        $.ajax({
        type: "POST",
        url: ACC_ROOT + "PaymentRequest/ValidParameterAccountConcept",
        data: JSON.stringify(
            { "conceptId": conceptId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            unlockScreen();

            if (data.success == false) {
                $("#alertForm").UifAlert('show', data.result, "warning");

                accountingPaymentConceptId = "";
                accountingConceptText = ""
                accountingAccountId = 0;
                accountingAccountName = "";
                conceptCode = "";
                $("#PaymentConcepts").find("#PaymentConceptId").text("");
                $("#PaymentConcepts").find("#PaymentConceptDescription").text("");
                $("#PaymentConcepts").find("#PaymentConceptId").val("");
                $("#PaymentConcepts").find("#PaymentConceptDescription").val("");

                return false;
            }
            else {

                $("#VoucherCurrency").val(selectedItem.CurrencyId)
                rate = MainPaymentRequest.GetCurrencyRate($("#ViewBagAccountingDate").val(), selectedItem.CurrencyId);
                setTimeout(function () {
                    $("#PaymentConcepts").find("#ExchangeRate").val(FormatCurrency(FormatDecimal(rate[0])))
                }, 500);
                $("#PaymentConcepts").find("#VoucherCurrency").attr("disabled", true);     

                accountingPaymentConceptId = selectedItem.Id.toString();
                accountingConceptText = selectedItem.Description;
                accountingAccountId = selectedItem.GeneralLedgerId;
                accountingAccountName = selectedItem.Description;
                MainPaymentRequest.LoadCostCenter();

                return true;
            }
        }
     });//fin de ajax
    }, 1000);

}

function prepareTaxCategory(type, data) {

    if (type == 0) {
        oTax = { TaxCategory: [] };
        // oPaymentRequestModel.PaymentTax = [];
        oTaxCategory = {};
    }
   
    oTaxCategory = {
        TaxId: 0,
        TaxCategoryId: 0,
        TaxCategoryDescript: "",
        TaxCondition: 0
    };

    oTaxCategory.TaxId = data.TaxCode;
    oTaxCategory.TaxCondition = data.TaxConditionCode;

    if (data.TaxCategoryCode == null) {
        oTaxCategory.TaxCategoryId = "";
    } else {
        oTaxCategory.TaxCategoryId = data.TaxCategoryCode;
    }
    if (data.TaxCategotyDescription == null) {
        oTaxCategory.TaxCategoryDescript = "";
    } else {
        oTaxCategory.TaxCategoryDescript = data.TaxCategotyDescription;
    }
    oTax.TaxCategory.push(oTaxCategory);
}

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainPaymentRequest();
});

class MainPaymentRequest extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        //MainPaymentRequest.GetSelectPaymentOrigin();

        //Se ocultan campos de busqueda de autocompletes
        MainPaymentRequest.CleanAutocompletesOtherPayment("SearchSuppliers");
        MainPaymentRequest.CleanAutocompletesOtherPayment("SearchInsured");
        MainPaymentRequest.CleanAutocompletesOtherPayment("SearchCoinsurance");
        MainPaymentRequest.CleanAutocompletesOtherPayment("SearchPerson");
        MainPaymentRequest.CleanAutocompletesOtherPayment("SearchAgent");
        MainPaymentRequest.CleanAutocompletesOtherPayment("SearchEmployee");
        MainPaymentRequest.CleanAutocompletesOtherPayment("SearchReinsurer");

        //Cargando los eventos
        MainPaymentRequest.LoadAutocompleteEventOtherPayment("SearchSuppliers");
        MainPaymentRequest.LoadAutocompleteEventOtherPayment("SearchInsured");
        MainPaymentRequest.LoadAutocompleteEventOtherPayment("SearchCoinsurance");
        MainPaymentRequest.LoadAutocompleteEventOtherPayment("SearchPerson");
        MainPaymentRequest.LoadAutocompleteEventOtherPayment("SearchAgent");
        MainPaymentRequest.LoadAutocompleteEventOtherPayment("SearchEmployee");
        MainPaymentRequest.LoadAutocompleteEventOtherPayment("SearchReinsurer");

        MainPaymentRequest.BlurAutocompleteFields("SearchSuppliers");
        MainPaymentRequest.BlurAutocompleteFields("SearchInsured");
        MainPaymentRequest.BlurAutocompleteFields("SearchCoinsurance");
        MainPaymentRequest.BlurAutocompleteFields("SearchPerson");
        MainPaymentRequest.BlurAutocompleteFields("SearchAgent");
        MainPaymentRequest.BlurAutocompleteFields("SearchEmployee");
        MainPaymentRequest.BlurAutocompleteFields("SearchReinsurer")

        var controller = ACC_ROOT + "Common/GetPaymentSources";
        $("#PaymentOrigin").UifSelect({ source: controller, selectedId: $("#ViewBagPaymentSources").val(), enabled: false  });

        controller = ACC_ROOT + "Common/GetAccountingCompanies";
        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#PaymentAccountingCompany").UifSelect({ source: controller, selectedId: $("#ViewBagAccountingCompanyDefault").val(), enabled: false });
        }
        else {
            $("#PaymentAccountingCompany").UifSelect({ source: controller, selectedId: $("#ViewBagAccountingCompanyDefault").val(), enabled: true });
        }
        
        controller = ACC_ROOT + "Common/GetListBranchesbyUserName";
        $("#PaymentBranch").UifSelect({ source: controller, /*selectedId: $("#ViewBagAccountingCompanyDefault").val(),*/ enabled: true });


        var defaultCurrencyCode = $("#DefaultCurrencyCode").val()
        $("#PaymentCurrency").UifSelect("setSelected", defaultCurrencyCode);
        $("#PaymentCurrency").trigger("change");


        $('#TransferPanel').hide();
        controller = ACC_ROOT + "PaymentRequest/GetEnabledPaymentTypes";
        $("#PaymentType").UifSelect({ source: controller });

        /*
        setTimeout(function () {
            MainPaymentRequest.GetSalePoints();
        }, 2000);
        */

        /////////////////////////////////////////
        //   listViews                        //
        /////////////////////////////////////////
        $("#taxesListView").UifListView({
            height: 200, //autoHeight: true,
            customAdd: true, customEdit: false, customDelete: false, localMode: true, add: true, edit: false, delete: true,
            displayTemplate: "#check-display", deleteCallback: deletetaxesListView
        });

        $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
            autoHeight: true,
            customDelete: false,
            localMode: true,
            customEdit: false,
            edit: false,
            delete: true,
            displayTemplate: "#check-display-template",
            deleteCallback: deletePaymentConcept
        });

        $("#TransferInformationListView").UifListView({
            height: 50, //autoHeight: true,
            customAdd: false, customEdit: true, customDelete: false, localMode: true, add: true, edit: false, delete: false,
            displayTemplate: "#check-display-transfer"
        });


        //Xhr de autocomplete de varios parametros
        $(document).ajaxSend(function (event, xhr, settings,data) {

            if (settings.url.indexOf("GetPaymentConceptById") != -1) {
                if ($("#PaymentBranch").val() == undefined) {
                    settings.url = settings.url + "&param=" + $("#AccountingBranch").val() + "/" + personCode + "/1";
                }
                else {
                    settings.url = settings.url + "&param=" + $("#PaymentBranch").val() + "/0/1";
                }
            }
            if (settings.url.indexOf("GetPaymentConceptByDescription") != -1) {
                if ($("#PaymentBranch").val() == undefined) {
                    settings.url = settings.url + "&param=" + $("#AccountingBranch").val() + "/" + personCode + "/2";
                }
                else {
                    settings.url = settings.url + "&param=" + $("#PaymentBranch").val() + "/0/2";
                }
            }
        });

        //$("#PaymentCurrency").attr("disabled", true);

    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#PaymentConcepts").find("#AddPayment").on("click", this.AddPaymentConcept);
        $("#PaymentConcepts").find("#AceptPayment").on("click", this.AcceptPaymentConcept);
        $("#modalTaxSelection").find("#btnTaxSelection").on("click", this.SelectedIndividualTax);
        $("#modalTaxSelection").find('#TableIndividualTax').on('selectAll', this.individualTaxSelectAll);
        $("#modalTaxSelection").find('#TableIndividualTax').on('desSelectAll', this.individualTaxDeselectAll);
        $("#modalTaxSelection").find('#TableIndividualTax').on('rowSelected', this.individualTaxRowSelected);
        $("#modalTaxSelection").find('#TableIndividualTax').on('rowDeselected', this.individualTaxRowDeselected);

        $("#ModalSuccessPrint").find("#PrintDetail").on("click", this.RequestPrint);
        $("#ModalSuccessPrint").find("#PrintAcept").on("click", this.ShowRequestPrint);
        $("#PaymentConcepts").find("#PaymentConceptDescription").on("blur", this.BlurAutocompletePaymentConceptDescription);
        $("#PaymentConcepts").find("#PaymentConceptDescription").on("itemSelected", this.AutocompletePaymentConceptDescription);
        $("#PaymentConcepts").find("#PaymentConceptDescription").on("keypress", this.KeyPressAutocompletePaymentConceptDescription);
        $("#PaymentConcepts").find("#PaymentConceptId").on("blur", this.BlurAutocompletePaymentConcept);
        $("#PaymentConcepts").find('#PaymentConceptId').on("itemSelected", this.AutocompletePaymentConcept);
        $("#PaymentConcepts").find("#PaymentConceptId").on("keypress", this.KeyPressAutocompletePaymentConcept);
        $("#PaymentConcepts").find('#VoucherCurrency').on('itemSelected', this.SelectedVoucherCurrency);
        $("#PaymentBranch").on("binded", this.BindedPaymentBranch);
        $("#PaymentConcepts").find('#ConceptAmount').on("blur", this.BlurConceptAmount);
        $("#CancelTicket").on("click", this.CancelPaymentRequest);
        $("#SaveTicketOtherPayments").on("click", this.SavePaymentRequest);
        $("#TransferInformationListView").on("rowEdit", this.RowEditTransferBank);
        $("#taxesListView").on("rowAdd", this.RowAddTaxListView);
        $(".uif-panel").on("Add", this.AddInvoice);
        $("#taxesListView").on("rowEdit", this.RowEditTaxListView);
        $("#PaymentTotal").on("blur", this.BlurPaymentTotal);
        $("#PaymentType").on("itemSelected", this.SelectedPaymentType);
        $("#PaymentOrigin").on("itemSelected", this.SelectedPaymentOrigin);
        $("#PaymentOrigin").on("binded", this.BindedPaymentOrigin);
        $("#PaymentBranch").on("itemSelected", this.SelectedPaymentBranch);
        $("#TaxCondition").on("click", this.ShowTaxCondition);
        $("#PaymentPayTo").on("itemSelected", this.SelectPaymentPayTo);
        $("#ShowTaxes").on("click", this.ShowTaxes);
        $("#EstimatedPaymentDate").on("datepicker.change", this.ChangeEstimatedPaymentDate);
        $("#PaymentConcepts").find("#CheckDate").on("datepicker.change", this.ChangeCheckDate);
        $("#PaymentCurrency").on("itemSelected", this.ChangePaymentCurrency);
        $("#PaymentAccountingCompany").on("binded", this.BindedPaymentAccountingCompany);
        $("#TablePaymentRequest").on("rowAdd", this.RowAddTablePaymentRequest);
        $("#TablePaymentRequest").on("rowDelete", this.RowDeleteTablePaymentRequest);
        $("#TableTransferRequest").on("rowSelected", this.RowSelectedPersonBankAccount);
    }

    /**
        * Validar al seleccionar todos los registros de la tabla.
        *
        */
    individualTaxSelectAll(event, data, position) {
        $("#alertIndividualTax").UifAlert('hide');
        var rowTemporals = $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable("getSelected");
        if (rowTemporals != null) {
            var count = 0;
            var countAll = 0;
            var localTemporalId = 0;

            for (var i = 0; i < rowTemporals.length; i++) {

               var rowselected = $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable("getSelected");

                count = 0;
                for (var j = 0; j < rowselected.length; j++) {
                    if (rowTemporals[i].TaxCode == rowselected[j].TaxCode) {
                        count++;
                        localTemporalId = rowselected[j].TemporalId;
                    }
                }

                if (count > 1) {
                    var value = {
                        label: 'TemporalId',//El campo por el cual se desea buscar
                        values: [localTemporalId] //Los valores a deseleccionar.
                    };
                    $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable('setUnselect', value)
                    countAll++;
                }
            }
            if (countAll > 0) {
                $("#alertIndividualTax").UifAlert('show', Resources.PaymentRequestValidTaxAll, "warning");
            }

        }
    }

    /**
        * Validar al seleccionar un registro de la tabla.
        *
        */
    individualTaxRowSelected(event, data, position) {

        $("#alertIndividualTax").UifAlert('hide');
        var rowselected = $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable("getSelected");
        var count = 0;
        if (rowselected != null) {
            if (rowselected.length > 1) {

                for (var i = 0; i < rowselected.length; i++) {
                    if (data.TaxCode == rowselected[i].TaxCode) {
                        count++;
                    }
                }

                if (count > 1) {
                    var value = {
                        label: 'TemporalId',//El campo por el cual se desea buscar
                        values: [data.TemporalId] //Los valores a deseleccionar.
                    };
                    $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable('setUnselect', value)
                    $("#alertIndividualTax").UifAlert('show', Resources.PaymentRequestValidTax, "warning");
                }

            }
        }
        
    }

    individualTaxRowDeselected() {
        $("#alertIndividualTax").UifAlert('hide');
    }

    individualTaxDeselectAll() {
        $("#alertIndividualTax").UifAlert('hide');
    }

    /**
        * Agrega un concepto de pago a la factura.
        *
        */
    AddPaymentConcept() {
        $("#alertMainOtherPayment").UifAlert('hide');
        $("#PaymentConceptFormulario").validate();
       
        if ($("#PaymentConceptFormulario").valid()) {

            if (($("#PaymentConcepts").find('#costCenter').val() == "") || ($("#PaymentConcepts").find('#costCenter').val() == null)) {
                MainPaymentRequest.LoadCostCenter();
            }
            else {
                if (MainPaymentRequest.DuplicateVoucherNumber($("#PaymentConcepts").find("#VoucherNumber").val()) == false) {
                    MainPaymentRequest.AddRowPaymentConcept();
                    accountingAccountName = "";
                    conceptCode = "";
                    $("#PaymentConcepts").find("#PaymentConceptId").text("");
                    $("#PaymentConcepts").find("#PaymentConceptDescription").text("");
                }
                else {
                    $("#alertForm").UifAlert('show', Resources.InternalBallotDuplicateVoucher, "warning");
                }            
            }
        }
    }

    /**
        * Cierra el modal de ingreso de conceptos de pago.
        *
        */
    AcceptPaymentConcept() {
        $("#alertMainOtherPayment").UifAlert('hide');
        index += 1;
        indexConcept = 0;
        var ids = $("#PaymentConcetpsListView").UifListView("getData");
        if (ids.length > 0) {
            MainPaymentRequest. SetDataPaymentRequest();
            var rowModel = new RowPaymentRequest();
            rowModel.Index = index;
            rowModel.VoucherTypeId = $("#PaymentConcepts").find("#VoucherType").val();
            rowModel.VoucherTypeDescription = $("#PaymentConcepts").find("#VoucherType option:selected").html();
            rowModel.VoucherType = $("#PaymentConcepts").find("#VoucherType").val();
            rowModel.VoucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
            rowModel.VoucherDate = $("#PaymentConcepts").find("#CheckDate").val();
            rowModel.VoucherCurrencyId = $("#PaymentConcepts").find("#VoucherCurrency").val();
            rowModel.VoucherCurrency = $("#PaymentConcepts").find("#VoucherCurrency option:selected").html();
            rowModel.ExchangeRate = ClearFormatCurrency($("#ExchangeRate").val());

            MainPaymentRequest.DelRowOtherPayments("PaymentConcetpsListView");
            rowModel.Amount = $("#TotalPaymentAmount").val();
            rowModel.Taxes = $("#TotalPaymentTaxes").val();
            rowModel.Retentions = $("#TotalPaymentRetention").val();
            rowModel.Id = "0";
            MainPaymentRequest.CleanDataVoucherEmpty();
            MainPaymentRequest.SetDataVoucher(rowModel);

            if (editIndex == -1) {
                //$('#taxesListView').UifListView("addItem", rowModel);
                $("#TablePaymentRequest").UifDataTable("addRow", rowModel);
            }
            else {
                //$('#taxesListView').UifListView("editItem", editIndex, rowModel);
                $("#TablePaymentRequest").UifDataTable('editRow', rowModel, editIndex)
                editIndex = -1;
            }
            /*
            totalPaymentAmount = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalAmountOtherPayments("taxesListView")));
            totalPaymentTaxes = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalTax("taxesListView")));
            totalPaymentRetention = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalRetention("taxesListView")));
            */
            totalPaymentAmount = FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalPaymentRequest()));
            totalPaymentTaxes = FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalTaxPaymentRequest()));
            totalPaymentRetention = FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalRetentionPAymentRequest()));

            $('#VouchersTotal').text(totalPaymentAmount);
            $('#VouchersTotal').val(totalPaymentAmount);

            $('#VoucherTaxesTotal').text(totalPaymentTaxes);
            $('#VoucherTaxesTotal').val(totalPaymentTaxes);

            $('#VoucherRetentionsTotal').text(totalPaymentRetention);
            $('#VoucherRetentionsTotal').val(totalPaymentRetention);

            $('#PaymentConcepts').UifModal('hide');
            MainPaymentRequest.ClearPaymentConceptsModal();

            $("#PaymentConcepts").find("#VoucherNumber").removeAttr("disabled");
            $("#PaymentConcepts").find("#CheckDate").removeAttr("disabled");
            $("#PaymentConcepts").find("#VoucherType").removeAttr("disabled");
            
            $("#PaymentConcepts").find("#VoucherCurrency").removeAttr("disabled");
            $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").removeAttr("disabled");

            //Se calula el total a pagar más impuestos
            $('#PaymentTotal').val(FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalToPay())));
        }
        else {
            $("#alertForm").UifAlert('show', Resources.RequiredConcept, "warning");
        }
    }

    /**
        * Selecciona los impuestos del individuo.
        *
        */
    SelectedIndividualTax() {
        var dataSelection = $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable("getSelected");
        if (dataSelection != null) {
            if (dataSelection.length >= 1) {
                MainPaymentRequest.ControlCheckOtherPayments();
            }
            else {
                $("#alertIndividualTax").UifAlert('show', Resources.SelectTax, "warning");
            }
        }
        else {
            $("#alertIndividualTax").UifAlert('show', Resources.SelectTax, "warning");
        }
    }

    /**
        * Permite imprimir la solicitud de pagos varios.
        *
        */
    RequestPrint() {
        MainPaymentRequest.LoadOtherPaymentsRequestReport(paymentRequestId);
        MainPaymentRequest.ClearAllOtherPayments();
        $('#ModalSuccessPrint').UifModal('hide');
    }

    /**
        * Permite visualizar el modal de impresión.
        *
        */
    ShowRequestPrint() {
        MainPaymentRequest.ClearAllOtherPayments();
        $('#ModalSuccessPrint').UifModal('hide');
    }

    /**
        * Obtiene los conceptos de pago por descripción.
        *
        * @param {String} event        - Perder el foco.
        */
    BlurAutocompletePaymentConceptDescription(event) {
        setTimeout(function () {
            $("#PaymentConcepts").find("#PaymentConceptDescription").val(accountingAccountName);
        }, 50);
    }

    /**
        * Selecciona un concepto de pago y obtiene los centros de costo asociados.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto de pago seleccionado.
        */
    AutocompletePaymentConceptDescription(event, selectedItem) {
        accountingAccountId = 0;
        $("#PaymentConcepts").find('#PaymentConceptId').val(selectedItem.Id);
        conceptCode = selectedItem.Id;
        
        validParmeters(conceptCode, selectedItem);
        
    }

    /**
        * Selecciona un concepto de pago y obtiene los centros de costo asociados.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto de pago seleccionado.
        */
    KeyPressAutocompletePaymentConceptDescription(event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        if (!event.altKey) {
            accountingPaymentConceptId = "";
            $("#PaymentConcepts").find('#PaymentConceptId').val("");
        }
    }

    /**
        * Obtiene los coneptos de pago por identificador.
        *
        * @param {String} event        - Perder el foco.
        */
    BlurAutocompletePaymentConcept(event) {
        setTimeout(function () {
            $("#PaymentConcepts").find('#PaymentConceptId').val(conceptCode);
        }, 50);
    }

    /**
        * Selecciona un concepto de pago y obtiene los centros de costo asociados.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto de pago seleccionado.
        */
    AutocompletePaymentConcept(event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        accountingAccountId = 0;
        $("#PaymentConcepts").find('#PaymentConceptDescription').val(selectedItem.Description);
        conceptCode = selectedItem.Id;
        validParmeters(conceptCode, selectedItem);

        //$("#VoucherCurrency").val(selectedItem.CurrencyId)
      
        //rate = MainPaymentRequest.GetCurrencyRate($("#ViewBagAccountingDate").val(), selectedItem.CurrencyId);        

        //setTimeout(function () {
        //    $("#PaymentConcepts").find("#ExchangeRate").val(FormatCurrency(FormatDecimal(rate[0])))
        //}, 500);

        //$("#PaymentConcepts").find("#VoucherCurrency").attr("disabled", true);        

    }

    /**
        * Selecciona un concepto de pago y obtiene los centros de costo asociados.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto de pago seleccionado.
        */
    KeyPressAutocompletePaymentConcept(event, selectedItem) {
        if (!event.altKey) {
            accountingPaymentConceptId = "";
            $("#PaymentConcepts").find('#PaymentConceptDescription').val("");
        }
    }

    /**
        * Selecciona una moneda y obtiene la tasa de cambio.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto de pago seleccionado.
        */
    SelectedVoucherCurrency(event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        if (selectedItem.Id != "") {

            $("#PaymentConcepts").find('#VoucherCurrencyPaymentConcept').val($("#PaymentConcepts").find("#VoucherCurrency option:selected").html())
            rate = MainPaymentRequest.GetCurrencyRate($("#ViewBagAccountingDate").val(), selectedItem.Id);
            
            setTimeout(function () {
                $("#PaymentConcepts").find("#ExchangeRate").val(FormatCurrency(FormatDecimal(rate[0])))
            }, 500);
        }

     }
    

    /**
        * Setea la sucursal por default después de cargar todas las sucursales.
        *
        */
    BindedPaymentBranch() {
        if ($("#ViewBagBranchDisableMainOther").val() == "1") {
            $("#PaymentBranch").attr("disabled", "disabled");
        }
        else {
            $("#PaymentBranch").removeAttr("disabled");
        }
        MainPaymentRequest.GetSalePoints();
    }

    /**
        * Da formato de número al importe.
        *
        */
    BlurConceptAmount() {
        var conceptAmount = $("#PaymentConcepts").find("#ConceptAmount").val();
        $("#PaymentConcepts").find("#ConceptAmount").val("$ " + NumberFormatDecimal(conceptAmount, "2", ".", ","));
    }

    /**
        * Cancela la grabación de la solicitud de pagos.
        *
        */
    CancelPaymentRequest() {
        MainPaymentRequest.ClearAllOtherPayments();
    }

    /**
        * Graba la solicitud de pagos.
        *
        */
    SavePaymentRequest() {
       
        if ($("#OtherPaymentsRequestForm").valid()) {
            if (MainPaymentRequest.validatePaymentPayTo() == false) {
                $("#alertMainOtherPayment").UifAlert('show', Resources.AutorizerPaymentPayTo, "warning");
                return;
            }

            if (ClearFormatCurrency($("#VouchersTotal").val()) != 0) {

                var resp = MainPaymentRequest.ValidTotal(parseFloat(ClearFormatCurrency($("#VouchersTotal").val().replace("", ",")).trim()),
                    parseFloat(ClearFormatCurrency($("#VoucherTaxesTotal").val().replace("", ",")).trim()),
                    parseFloat(ClearFormatCurrency($("#VoucherRetentionsTotal").val().replace("", ",")).trim()),
                    parseFloat(ClearFormatCurrency($("#PaymentTotal").val().replace("", ","))));

                if (resp) {
                    $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });
                    MainPaymentRequest.SetHeaderPayment();
                    if (parseInt($("#PaymentType").val()) == $("#ViewBagParamPaymentMethodTransferMainOther").val()) {
                        if (MainPaymentRequest.ValidTransferSelected()) {
                            lockScreen();
                            MainPaymentRequest.SetDataPaymentRequest();
                            MainPaymentRequest.SaveOtherPaymentRequest();
                            
                        } else {
                            $("#alertMainOtherPayment").UifAlert('show', Resources.SelectAccountainOtherPayments, "warning");
                        }
                    } else {
                        lockScreen();
                        MainPaymentRequest.SetDataPaymentRequest();
                        MainPaymentRequest.SaveOtherPaymentRequest();
                    }
                } else {
                    $("#alertMainOtherPayment").UifAlert('show', Resources.ValidTotMainOtherPaymentsRequest, "warning");
                    unlockScreen();
                }
            } else {
                $("#alertMainOtherPayment").UifAlert('show', Resources.TotalPayment, "warning");
            }
        }
       
    }

    /**
        * Selecciona una moneda y obtiene la tasa de cambio.
        *
        * @param {String} event       - Seleccionar.
        * @param {Object} selectedRow - Objeto con valores de la cuenta banacaria seleccionada.
        */
    RowEditTransferBank(event, selectedRow) {
        accountBankId = selectedRow.AccountBankId;
    }

    /**
        * Visualiza el modal de inserción de impuestos.
        *
        * @param {String} event - Agregar.
        */
    RowAddTaxListView(event) {
        $("#alertForm").UifAlert('hide');

        editIndex = -1;
        indexConcept = 0;
        taxHeaderModel = {
            Index: 0,
            TaxConcept: []
        };

        $("#alertMainOtherPayment").UifAlert('hide');
        MainPaymentRequest.ClearPaymentConceptsModal();
        $("#OtherPaymentsRequestForm").validate();

        if ($("#OtherPaymentsRequestForm").valid()) {
            if (enableForSave) {

                var ids = $("#taxesListView").UifListView("getData");
                if (ids.length == 0) {
                    MainPaymentRequest.ClearModel();
                    correlativeNumber = 1;
                }
                else {
                    correlativeNumber = ids.length + 1;
                }

                voucherType = $("#PaymentConcepts").find("#VoucherType option:selected").html();
                voucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
                voucherCurrency = $("#PaymentConcepts").find("#VoucherCurrency option:selected").html();
                if (voucherCurrency != "") {
                    $("#PaymentConcepts").find("#VoucherCurrencyPayment").val(VoucherCurrency);
                }

                paymentConceptTypeId = $("#PaymentMovementType").val();
                paymentBranchId = $("#PaymentBranch").val();

                $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
                    customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: false, delete: true,
                    displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept
                });

                MainPaymentRequest.DelRowOtherPayments("PaymentConcetpsListView");

                $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);
            }
            else {
                $("#alertMainOtherPayment").UifAlert('show', Resources.PersonTypeNotAuthorized, "warning");
            }
        }
    }

    /**
        * Visualiza el modal de inserción de conceptos de pago.
        *
        * @param {String} event - Agregar.
        */
    AddInvoice(event) {
        editIndex = -1;
        indexConcept = 0;
        taxHeaderModel = {
            Index: 0,
            TaxConcept: []
        };

        $("#alertMainOtherPayment").UifAlert('hide');
        MainPaymentRequest.ClearPaymentConceptsModal();
        $("#OtherPaymentsRequestForm").validate();

        if ($("#OtherPaymentsRequestForm").valid()) {
            ids = $("#taxesListView").UifListView("getData");
            if (ids.length == 0) {
                correlativeNumber = 1;
            }
            else {
                correlativeNumber = ids.length + 1;
            }

            voucherType = $("#PaymentConcepts").find("#VoucherType option:selected").html();
            voucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
            voucherCurrency = $("#PaymentConcepts").find("#VoucherCurrency option:selected").html();
            if (voucherCurrency != "") {
                $("#PaymentConcepts").find("#VoucherCurrencyPayment").val(VoucherCurrency);

            }
            paymentConceptTypeId = $("#PaymentMovementType").val();
            paymentBranchId = $("#PaymentBranch").val();

            $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
                customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: false, delete: true,
                displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept

            });

            MainPaymentRequest.DelRowOtherPayments("PaymentConcetpsListView");
            $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);
        }
    }

    /**
        * Edita un impuesto.
        *
        * @param {String} event - Editar.
        * @param {Object} data  - Objeto con el impuesto seleccionado.
        * @param {Number} index - Posición o índice en el ListView.
        */
    RowEditTaxListView(event, data, index) {
        ids = $("#taxesListView").UifListView("getData");
        if (ids.length == 0) {
            MainPaymentRequest.SetDataPaymentRequest();
        }
        $("#PaymentConcepts").find("#VoucherType").val(data.VoucherType);
        $("#PaymentConcepts").find("#VoucherNumber").val(data.VoucherNumber);
        $("#PaymentConcepts").find("#CheckDate").val(data.Date);
        $("#PaymentConcepts").find("#VoucherCurrency").val(data.CurrencyId);
        $("#PaymentConcepts").find("#ExchangeRate").val(data.PaymentConcept);

        editIndex = index;
        $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
            autoHeight: true,
            customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: true,
            displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept
        });
        MainPaymentRequest.DelRowOtherPayments("PaymentConcetpsListView");
        $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);
    }

    /**
        * Formato de número al importe.
        *
        */
    BlurPaymentTotal() {
        var paymentsTotal = $.trim(ClearFormatCurrency($("#PaymentTotal").val()));

        if (paymentsTotal != "" && paymentsTotal != "$") {            
            $("#PaymentTotal").val("$ " + NumberFormatDecimal(paymentsTotal, "2", ".", ","));
        } else {
            $("#PaymentTotal").val('');
        }
    }

    /**
        * Obtiene las cuentas bancarias del individuo.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con la cuenta bancaria seleccionada.
        */
    SelectedPaymentType(event, selectedItem) {
        if (selectedItem.Id == $("#ViewBagParamPaymentMethodTransferMainOther").val()) {
            $("#TransferInformationListView").UifListView(
                {
                    source: ACC_ROOT + "Common/GetAccountBanksByIndividualId?individualId=" + individualId,
                    height: 50,
                    customDelete: false,
                    customAdd: false,
                    customEdit: true,
                    edit: true,
                    displayTemplate: "#check-display-transfer"
                });
            //$('#TransferPanel').show();

            var controller = ACC_ROOT + "PaymentRequest/GetBankAccountsByIndividualId?individualId=" + individualId;
            $("#ModalTransfer").find("#TableTransferRequest").UifDataTable({ source: controller });

            $('#ModalTransfer').UifModal('showLocal', Resources.BankAccounts);
        }
        else {
            $('#TransferPanel').hide();
        }
        //se comprueba si es pago electrónico.
        if (selectedItem.Id == $("#ViewBagParamPaymentMethodElectronicPayment").val()) {
            if (individualId <= 0) {
                //mensaje de que no se a seleccionado un beneficiario
                $("#alertMainOtherPayment").UifAlert('show', Resources.InputBeneficiaryInformation, "warning");
            } else {
                MainPaymentRequest.ShowAccountInfo(individualId);
            }
        } else {
            
            $("#PaymentDescription").val("");
            $("#alertMainOtherPayment").UifAlert("hide");
        }
    }

    /**
        * Obtiene las cuentas bancarias del individuo.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Objeto con la cuenta bancaria seleccionada.
        * @param {Object} position - Indice con la cuenta bancaria seleccionada.
        */
    RowSelectedPersonBankAccount(event, data, position) {
        accountBankId = data.AccountBankId;
        $('#ModalTransfer').modal('hide');
    }


    /**
        * Obtiene los tipos de movimiento al seleccionar un origen de pago.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con el origen de pago seleccionado.
        */
    SelectedPaymentOrigin(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "PaymentRequest/GetPaymentMovementTypesByPaymentSourceIdFilter?paymentSourceId=" + selectedItem.Id;
            $("#PaymentMovementType").UifSelect({ source: controller });
        }
    }

    /**
        * Setea el origen de pago por default.
        *
        */
    BindedPaymentOrigin() {
        $("#PaymentOrigin").val(paymentSourceOthers);
        $("#PaymentOrigin").attr("disabled", "disabled");
        $("#PaymentOrigin").trigger('change');
    }

    /**
        * Obtiene los puntos de venta al seleccionar una sucursal.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con la sucursal seleccionada.
        */
    SelectedPaymentBranch(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#PaymentSalesPoint").UifSelect({ source: controller });
        }
    }

    
    /**
        * Setea la compañia por default.
        *
        */
    BindedPaymentAccountingCompany() {
        //$("#PaymentAccountingCompany").attr("disabled", "disabled");
        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#PaymentAccountingCompany").attr("disabled", true);
        }
        else {
            $("#PaymentAccountingCompany").attr("disabled", false);
        }
    }

    /**
        * Visualiza el modal de condición impositiva.
        *
        */
    ShowTaxCondition() {
        taxConditionTitle = Resources.TaxCondition + ':  ' + ' ' + individualName;
        MainPaymentRequest.LoadTaxCondition(individualId)
        $('#ModalTaxCondition').find('.ptitle').html(taxConditionTitle);
        $('#ModalTaxCondition').UifModal('showLocal', taxConditionTitle);

        var controller = ACC_ROOT + "PaymentRequest/GetIndividualTaxByIndividualId?individualId=" + individualId;
        $("#ModalTaxCondition").find("#TableTaxCondition").UifDataTable({ source: controller });
    }


    /**
        * Visualiza los autocomplete de individuos al seleccionar pagar a.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con pagar a seleccionado.
        */
    SelectPaymentPayTo(event, selectedItem) {
        individualId = 0;
        individualName = "";
        changePayment = true;
        var enableAjax = 0;
        $("#alertForm").UifAlert('hide');

        var lisTaxView = $("#taxesListView").UifListView("getData");
        if (lisTaxView.length > 0) {
            MainPaymentRequest.ShowConfirmOtherPayments(1, "", "", "", payToValue);
        }

        if (changePayment) {
            payToValue = selectedItem.Id;
            MainPaymentRequest.CleanAutocompletesOtherPayment('SearchSuppliers');
            MainPaymentRequest.CleanAutocompletesOtherPayment('SearchInsured');
            MainPaymentRequest.CleanAutocompletesOtherPayment('SearchCoinsurance');
            MainPaymentRequest.CleanAutocompletesOtherPayment('SearchPerson');
            MainPaymentRequest.CleanAutocompletesOtherPayment('SearchAgent');
            MainPaymentRequest.CleanAutocompletesOtherPayment('SearchEmployee');
            MainPaymentRequest.CleanAutocompletesOtherPayment('SearchReinsurer');

            if (selectedItem.Id != "") {
                $("#BeneficiaryDocumentNumberData").hide();
                $("#BeneficiaryNameData").hide();

                if (selectedItem.Id == $("#ViewBagSupplierCodeMainOther").val()) { // 1 Proveedor  // 10 en BE
                    $('#SearchSuppliersByDocumentNumberRequest').parent().parent().show();
                    $("#SearchSuppliersByNameRequest").parent().parent().show();
                    enableForSave = true;
                }
                else if (selectedItem.Id == $("#ViewBagProducerCodeMainOther").val()) { // 10 Productor  // agente 1 en BE
                    $("#SearchAgentByDocumentNumberRequest").parent().parent().show();
                    $("#SearchAgentByNameRequest").parent().parent().show();
                    enableForSave = true;
                }
                else if (selectedItem.Id == $("#ViewBagThirdPartyCodeMainOther").val()) { //tercero - //TODO: Por el momento queda pendiente la implementación.
                    $("#SearchPersonByDocumentNumberRequest").parent().parent().show();
                    $("#SearchPersonByNameRequest").parent().parent().show();
                    enableForSave = true;
                }
                else if (selectedItem.Id == $("#ViewBagAgentCodeMainOther").val()) { //agente
                    $("#SearchAgentByDocumentNumberRequest").parent().parent().show();
                    $("#SearchAgentByNameRequest").parent().parent().show();
                    enableForSave = true;
                }
                else if (selectedItem.Id == $("#ViewBagEmployeeCodMainOther").val()) { // 11 Empleado  // 11 en BE
                    $("#SearchEmployeeByDocumentNumberRequest").parent().parent().show();
                    enableAjax = 1;

                    $("#SearchEmployeeByNameRequest").parent().parent().show();

                    enableForSave = true;
                }
                else if (selectedItem.Id == '') { //15 Agente   //1 en BE
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchSuppliers');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchInsured');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchCoinsurance');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchPerson');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchAgent');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchEmployee');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchReinsurer');

                    enableForSave = false;
                }
                else {
                    $("#BeneficiaryDocumentNumberData").show();
                    $("#BeneficiaryNameData").show();

                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchSuppliers');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchInsured');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchCoinsurance');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchPerson');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchAgent');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchEmployee');
                    MainPaymentRequest.CleanAutocompletesOtherPayment('SearchReinsurer');

                    enableForSave = false;
                }

                if (enableAjax > 0) {
                }
            }
            else {
                $("#BeneficiaryDocumentNumberData").show();
                $("#BeneficiaryNameData").show();
                MainPaymentRequest.CleanAutocompletesOtherPayment('SearchSuppliers');
                MainPaymentRequest.CleanAutocompletesOtherPayment('SearchInsured');
                MainPaymentRequest.CleanAutocompletesOtherPayment('SearchCoinsurance');
                MainPaymentRequest.CleanAutocompletesOtherPayment('SearchPerson');
                MainPaymentRequest.CleanAutocompletesOtherPayment('SearchAgent');
                MainPaymentRequest.CleanAutocompletesOtherPayment('SearchEmployee');
                MainPaymentRequest.CleanAutocompletesOtherPayment('SearchReinsurer');

                enableForSave = false;
            }
        }
    }

    /**
        * Visualiza el modal de impuestos calculados.
        *
        */
    ShowTaxes() {
        var taxDescription = "";
        var totalTaxConcepts = [];
        /*
        if (taxModel.Concept.length > 0) {
            for (var i = 0; i < taxModel.Concept.length; i++) {
                for (var j = 0; j < taxModel.Concept[i].TaxConcept.length; j++) {
                    taxDetailModel = {
                        TaxId: 0,
                        TaxCategoryId: 0,
                        TaxConditionId: 0,
                        TaxDescription: "",
                        TaxGroup: "",
                        Amount: 0,
                        Rate: 0,
                        TaxAmount: 0,
                        MinimumTax: 0,
                    };
                    taxDescription = taxModel.Concept[i].TaxConcept[j].TaxDescription;
                    if (totalTaxConcepts.length > 0) {
                        for (var f = 0; f < totalTaxConcepts.length; f++) {
                            if (taxDescription == totalTaxConcepts[f].TaxDescription) {
                                totalTaxConcepts[f].Amount = totalTaxConcepts[f].Amount + taxModel.Concept[i].TaxConcept[j].Amount;
                                totalTaxConcepts[f].TaxAmount = totalTaxConcepts[f].TaxAmount + taxModel.Concept[i].TaxConcept[j].TaxAmount;
                                break;
                            }
                            else {
                                if (f == totalTaxConcepts.length - 1) {
                                    taxDetailModel.TaxDescription = taxModel.Concept[i].TaxConcept[j].TaxDescription;
                                    taxDetailModel.TaxGroup = taxModel.Concept[i].TaxConcept[j].TaxGroup
                                    taxDetailModel.Amount = taxModel.Concept[i].TaxConcept[j].Amount
                                    taxDetailModel.Rate = taxModel.Concept[i].TaxConcept[j].Rate;
                                    taxDetailModel.TaxAmount = taxModel.Concept[i].TaxConcept[j].TaxAmount;
                                    taxDetailModel.MinimumTax = taxModel.Concept[i].TaxConcept[j].MinimumTax;

                                    totalTaxConcepts.push(taxDetailModel)
                                    $("#TableCalculatedTaxes").UifDataTable('addRow', taxDetailModel);
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        taxDetailModel.TaxDescription = taxModel.Concept[i].TaxConcept[j].TaxDescription;
                        taxDetailModel.TaxGroup = taxModel.Concept[i].TaxConcept[j].TaxGroup
                        taxDetailModel.Amount = taxModel.Concept[i].TaxConcept[j].Amount
                        taxDetailModel.Rate = taxModel.Concept[i].TaxConcept[j].Rate;
                        taxDetailModel.TaxAmount = taxModel.Concept[i].TaxConcept[j].TaxAmount;
                        taxDetailModel.MinimumTax = taxModel.Concept[i].TaxConcept[j].MinimumTax;

                        totalTaxConcepts.push(taxDetailModel)
                        $("#TableCalculatedTaxes").UifDataTable('addRow', taxDetailModel);
                    }
                }
            }
        }
        */
        $('#taxesView').hide();

        $("#TableCalculatedTaxes").UifDataTable('clear');

        if (taxModel.Concept.length > 0) {
            for (var i = 0; i < taxModel.Concept.length; i++) {
                for (var j = 0; j < taxModel.Concept[i].TaxConcept.length; j++) {
                    taxDetailModel = {
                        TaxId: 0,
                        TaxCategoryId: 0,
                        TaxConditionId: 0,
                        TaxDescription: "",
                        TaxGroup: "",
                        Amount: 0,
                        Rate: 0,
                        TaxAmount: 0,
                        MinimumTax: 0,
                    };

                    taxDetailModel.TaxDescription = taxModel.Concept[i].TaxConcept[j].TaxDescription;
                    taxDetailModel.TaxGroup = taxModel.Concept[i].TaxConcept[j].TaxGroup
                    taxDetailModel.Amount = FormatCurrency(decimalAdjust('round', taxModel.Concept[i].TaxConcept[j].Amount, -2))
                    taxDetailModel.Rate = taxModel.Concept[i].TaxConcept[j].Rate;
                    taxDetailModel.TaxAmount = FormatCurrency(decimalAdjust('round', taxModel.Concept[i].TaxConcept[j].TaxAmount, -2));
                    taxDetailModel.MinimumTax = FormatCurrency(decimalAdjust('round', taxModel.Concept[i].TaxConcept[j].MinimumTax, -2));

                    $("#TableCalculatedTaxes").UifDataTable('addRow', taxDetailModel);
                }
            }
        }

        $('#ModalTaxes').UifModal('showLocal', Resources.CalculatedTaxes);
    }

    /**
        * Valida la fecha de estimación de pago.
        *
        * @param {String} event - Seleccionar.
        * @param {Date} date    - Fecha seleccionada.
        */
    ChangeEstimatedPaymentDate(event, date) {
        $("#alertMainOtherPayment").UifAlert('hide');
        if (IsDate($('#EstimatedPaymentDate').val())) {
            var systemDate = GetCurrentDate();
            var dateAfter = $('#EstimatedPaymentDate').val();
            if (!MainPaymentRequest.ValidateDateAfter(dateAfter)) {
                $("#alertMainOtherPayment").UifAlert('show', Resources.InvalidDates, "warning");
                $("#EstimatedPaymentDate").val("");
            }
        }
    }

    /**
        * Valida la fecha del comprobante.
        *
        * @param {String} event  - Seleccionar.
        * @param {DateTime} date - Fecha seleccionada.
        */
    ChangeCheckDate(event, date) {
        $("#alertForm").UifAlert('hide');
        if (IsDate($("#PaymentConcepts").find("#CheckDate").val())) {

        }
        else {
            $("#PaymentConcepts").find("#CheckDate").val(GetCurrentDate());
        }
    }

    /**
        * Asignación código de la moneda de acuerdo a lo que se seleccione.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con la moneda seleccionada.
        */
    ChangePaymentCurrency(event, selectedItem) {
        if (selectedItem.Id != "") {
            oPaymentRequestModel.CurrencyId = selectedItem.Id; //PaymentRequestCurrencyId
        }
    }

    /**
        * Visualiza el modal de ingreso de conceptos de la factura.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con el concepto agregado.
        */
    RowAddTablePaymentRequest(event, data) {
        editIndex = -1;
        indexConcept = 0;
        taxHeaderModel = {
            Index: 0,
            TaxConcept: []
        };

        $("#alertMainOtherPayment").UifAlert('hide');
        MainPaymentRequest.ClearPaymentConceptsModal();
        $("#OtherPaymentsRequestForm").validate();

        if (MainPaymentRequest.validatePaymentPayTo()==false) {
            $("#alertMainOtherPayment").UifAlert('show', Resources.AutorizerPaymentPayTo, "warning");
            return;
        }

        if ($("#OtherPaymentsRequestForm").valid()) {
            var ids = $("#taxesListView").UifListView("getData");
            if (ids.length == 0) {
                correlativeNumber = 1;
            }
            else {
                correlativeNumber = ids.length + 1;
            }

            voucherType = $("#PaymentConcepts").find("#VoucherType option:selected").html();
            voucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
            voucherCurrency = $("#PaymentConcepts").find("#VoucherCurrency option:selected").html();
            if (voucherCurrency != "") {
                $("#PaymentConcepts").find("#VoucherCurrencyPayment").val(voucherCurrency);
            }
            paymentConceptTypeId = $("#PaymentMovementType").val();
            paymentBranchId = $("#PaymentBranch").val();

            $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
                customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: false, delete: true,
                displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept

            });

            MainPaymentRequest.DelRowOtherPayments("PaymentConcetpsListView");
            $('#PaymentConcepts').UifModal('showLocal', $("#ViewBagTitle").val());
        }
    }

    /**
        * Visualiza el modal de confirmación de eliminación de la factura.
        *
        * @param {String} event    - Eliminar.
        * @param {Object} data     - Objeto con la factura a eliminar.
        * @param {Number} position - Número de posición de la factura a eliminar.
        */
    RowDeleteTablePaymentRequest(event, data, position) {
        /*
        if (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.length > 0) {
            for (var i = 0; i <= (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.length); i++) {
                if (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher[i].Index == data.Index) {
                    oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.splice(i, 1);
                }
            }
        }
        */
        if (oPaymentRequestModel.Vouchers.length > 0) {
            var length = 0;
            for (var i = 0; i <= oPaymentRequestModel.Vouchers.length - 1; i++) {
                if (oPaymentRequestModel.Vouchers[i].Index == data.Index) {
                    length = oPaymentRequestModel.Vouchers[i].VoucherConcepts.length;
                    for (var j = 0; j < length; j++) {
                        oPaymentRequestModel.Vouchers[i].VoucherConcepts.splice(j, 1);
                    }
                    oPaymentRequestModel.Vouchers.splice(i, 1);
                }
            }
        }
        //Se elimina el comprobante de pago seleccionado
        $("#TablePaymentRequest").UifDataTable('deleteRow', position)

        totalPaymentAmount = FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalPaymentRequest()));
        totalPaymentTaxes = FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalTaxPaymentRequest()));
        totalPaymentRetention = FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalRetentionPAymentRequest()));

        $('#VouchersTotal').text(totalPaymentAmount);
        $('#VouchersTotal').val(totalPaymentAmount);

        $('#VoucherTaxesTotal').text(totalPaymentTaxes);
        $('#VoucherTaxesTotal').val(totalPaymentTaxes);

        $('#VoucherRetentionsTotal').text(totalPaymentRetention);
        $('#VoucherRetentionsTotal').val(totalPaymentRetention);

        setTimeout(function () {
            MainPaymentRequest.DelRowOtherPayments("taxesListView");
        }, 2000);

        if (taxModel.Concept.length > 0) {
            var idelete = 0;
            for (var k = 0; k <= taxModel.Concept.length - 1; k++) {
                if (taxModel.Concept[k].Index == data.Index) {
                    idelete = taxModel.Concept[k].TaxConcept.length;
                    for (var j = 0; j < idelete; j++) {
                        taxModel.Concept[k].TaxConcept.splice(j, 1);
                    }
                    taxModel.Concept.splice(k, 1);
                }
            }
        }

        //Se calula el total a pagar más impuestos
        $('#PaymentTotal').val(FormatCurrency(FormatDecimal(MainPaymentRequest.CalculateTotalToPay())));
    }


    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Obtiene última tasa de cambio.
        *
        * @param {Number} id - Identificador de moneda.
        */
    static LoadExchangeRateDeafult(id) {

        
        var rate;

        if ($('#VoucherCurrency').val() == "") {
            rate = MainPaymentRequest.GetCurrencyRate($("#ViewBagAccountingDate").val(), 0);
        }
        else {
            rate = MainPaymentRequest.GetCurrencyRate($("#ViewBagAccountingDate").val(), $('#VoucherCurrency').val());
        }

        $('#ExchangeRate').val(rate[0]);
        exchangeRate = $("#PaymentConcepts").find("#ExchangeRate").val();
        $("#PaymentConcepts").find("#ExchangeRate").val(FormatCurrency(FormatDecimal(exchangeRate)))

        $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
            autoHeight: true,
            customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: false, delete: true,
            displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept
        });

        $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);
    }

    /**
        * Obtiene la tasa de cambio.
        *
        * @param {Date} accountingDate - Fecha contable.
        * @param {Number} currencyId   - Identificador de moneda.
        * 
        * @returns {boolean} true or false
        */
    static GetCurrencyRate(accountingDate, currencyId) {
        var resp = new Array();
        lockScreen();
        setTimeout(function () {    
            $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Common/GetCurrencyExchangeRate",
            data: JSON.stringify({ rateDate: accountingDate, currencyId: parseInt(currencyId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                    unlockScreen();
                if (data != null) {
                    resp[0] = data;
                }
            }
            });
        }, 500);
        return resp;
    }

    /**
        * Obtiene los centros de costo asociados a una cuenta contable.
        *
        */
    static LoadCostCenter() {
        //SMT-1758 Inicio
        //var controller = ACC_ROOT + "PaymentRequest/GetCostCenterByAccountingAccountId?accountingAccountId=" + accountingAccountId;
        //$("#PaymentConcepts").find("#costCenter").UifSelect({ source: controller });
        $("#PaymentConcepts").find('#costCenter').UifSelect("setSelected", null);
        $("#PaymentConcepts").find("#costCenter").attr("disabled", "disabled");

        if (accountingAccountId > 0) {
            lockScreen();
            setTimeout(function () {   
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "PaymentRequest/GetCostCenterByAccountingAccountId",
                    data: JSON.stringify({ accountingAccountId: accountingAccountId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                }).done(function (data) {
                    unlockScreen();
                    if (data.success) {
                        if (data.result.length > 0) {
                            $("#PaymentConcepts").find('#costCenter').UifSelect({ sourceData: data.result });
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.ItIsNotParameterized + " " + Resources.CostCenter, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true })
                    resultOperation = false;
                 });
            }, 500);
        }
        //SMT-1758 Fin
    }

    /**
        * Añade un concepto de pago a la factura.
        *
        */
    static AddRowPaymentConcept() {
        var amountAux;
        var conditionCode = -1;
        var branchCode = -1;
        var currencyId = -1;
        var rowModel = new RowPaymentConceptModel();
        var categories = "";
        indexConcept++;
        var totalAmount;


        //lockScreen();
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        if (MainPaymentRequest.DuplicateRecordsOtherPayments($("#PaymentConcepts").find("#PaymentConceptId").val(), $("#PaymentConcepts").find("#costCenter").val()) == false) {
            $("#alertForm").UifAlert('hide');
            rowModel = new RowPaymentConceptModel();
            rowModel.Index = indexConcept;
            rowModel.PaymentConceptId = $("#PaymentConcepts").find("#PaymentConceptId").val();
            conditionCode = $("#PaymentConcepts").find("#PaymentConceptId").val();
            currencyId = $("#PaymentConcepts").find("#VoucherCurrency").val();
            rowModel.PaymentConceptDescription = $("#PaymentConcepts").find("#PaymentConceptDescription").val();
            rowModel.PaymentConceptIdDescription = $("#PaymentConcepts").find("#PaymentConceptId").val() + " - " + $("#PaymentConcepts").find("#PaymentConceptDescription").val();
            rowModel.CostCenterId = $("#PaymentConcepts").find("#costCenter").val();
            rowModel.CostCenterDescription = $("#costCenter option:selected").html();
            rowModel.CostCenterIdDescription = $("#PaymentConcepts").find("#costCenter").val() + " - " + $("#PaymentConcepts").find("#costCenter option:selected").html();
            rowModel.Amount = parseFloat(ClearFormatCurrency($("#ConceptAmount").val()).replace(",", ".")) * parseFloat(ClearFormatCurrency($("#ExchangeRate").val()).replace(",", "."));
            $("#PaymentConcepts").val(rowModel.Amount);
            amountAux = ClearFormatCurrency($("#PaymentConcepts").val());
            branchCode = $("#PaymentBranch").val();
            insuredCode = $("#PaymentPayTo").val();
            exchangeRate = ClearFormatCurrency($("#ExchangeRate").val());
            rowModel.VoucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
            rowModel.Taxes = 0;
            rowModel.Retentions = 0;

            lockScreen();
            setTimeout(function () {
                $.ajax({
                type: "POST",
                url: ACC_ROOT + "PaymentRequest/GetIndividualTaxesByIndividualId",
                data: JSON.stringify({
                    "individualId": individualId
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    unlockScreen();
                    if (data != undefined) {
                        if (data.length > 0) {
                            if (personCode == 0 || personCode == -1) {
                                personCode = individualId;
                            }
                            if (editIndex != -1) {
                                correlativeNumber = editIndex;
                            }

                            if (oPaymentRequestModel.PaymentTax != undefined) {

                                oPaymentRequestModel.PaymentTax = oTax.TaxCategory;

                                if (oPaymentRequestModel.PaymentTax.length > 0) {
                                    for (var i = 0; i < oPaymentRequestModel.PaymentTax.length; i++) {
                                        categories = categories + oPaymentRequestModel.PaymentTax[i].TaxId + ";";
                                        categories = categories + oPaymentRequestModel.PaymentTax[i].TaxCategoryId + ";";
                                        conditionCode = oPaymentRequestModel.PaymentTax[i].TaxCondition;
                                    }
                                } else {
                                    categories = "";
                                }
                            } else {
                                categories = "";
                            }
                            lockScreen();
                            setTimeout(function () {
                               $.ajax({
                                type: "POST",
                                url: ACC_ROOT + "PaymentRequest/GetTotalTax",
                                data: JSON.stringify({
                                    "payerTypeId": insuredCode, "branchId": branchCode,
                                    "individualId": personCode, "amount": parseFloat(amountAux),
                                    "conditionId": conditionCode, "agentTypeId": agentTypeId,
                                    "correlativeNumber": correlativeNumber, "currencyId": currencyId,
                                    "exchangeRate": parseFloat(exchangeRate),
                                    "categories": categories,
                                    "coverageTaxId": coverageTaxId,
                                }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                   success: function (data) {
                                       
                                       unlockScreen();
                                       if (data == -1) {
                                       
                                           $("#alertForm").UifAlert('show', Resources.ErrorTaxCategory, "danger");
                                       }
                                       else if (data != null)
                                       {
                                        totalTaxValueConcept = parseFloat(data[0].TaxValue);
                                        rowModel.Taxes = parseFloat(data[0].TaxValue);
                                        rowModel.Retentions = parseFloat(data[0].RetentionValue);
                                        if (rowModel.Taxes >= 0) {
                                            //arma el objeto para ver los impuestos cargados(depende de la seleccion de impuestos)
                                            if (idsTax != null) {
                                                for (var i in idsTax) {
                                                    var row = idsTax[i];
                                                    taxDetailModel = {
                                                        Index: 0,
                                                        TaxId: 0,
                                                        TaxCategoryId: 0,
                                                        TaxConditionId: 0,
                                                        TaxDescription: "",
                                                        TaxGroup: "",
                                                        Amount: 0,
                                                        Rate: 0,
                                                        TaxAmount: 0,
                                                        MinimumTax: 0,
                                                    };
                                                    taxDetailModel.Index = indexConcept;
                                                    taxDetailModel.TaxId = row.TaxCode;
                                                    taxDetailModel.TaxCategoryId = row.TaxCategoryCode;
                                                    taxDetailModel.TaxConditionId = row.TaxConditionCode,
                                                    taxDetailModel.TaxDescription = row.TaxDescription;
                                                    taxDetailModel.TaxGroup = row.TaxConditionDescription;
                                                    taxDetailModel.Amount = amountAux;
                                                    taxDetailModel.Rate = row.Rate;
                                                    if (row.RateTypeCode == 1) {
                                                        taxDetailModel.TaxAmount = (amountAux * row.Rate) / 100;
                                                    }
                                                    else {
                                                        taxDetailModel.TaxAmount = 0;
                                                    }
                                                    taxDetailModel.MinimumTax = 0;

                                                    taxHeaderModel.TaxConcept.push(taxDetailModel);
                                                    //indexConcept++;
                                                }
                                            }

                                            setTimeout(function () {
                                                $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView("addItem", rowModel);

                                                totalPaymentAmount = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalAmountOtherPayments("PaymentConcetpsListView")))
                                                totalPaymentTaxes = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalTax("PaymentConcetpsListView")))
                                                totalPaymentRetention = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalRetention("PaymentConcetpsListView")))

                                                $("#PaymentConcepts").find('#TotalPaymentAmount').text(totalPaymentAmount);
                                                $("#PaymentConcepts").find('#TotalPaymentAmount').val(totalPaymentAmount);

                                                $("#PaymentConcepts").find('#TotalPaymentTaxes').text(totalPaymentTaxes);
                                                $("#PaymentConcepts").find('#TotalPaymentTaxes').val(totalPaymentTaxes);

                                                $("#PaymentConcepts").find('#TotalPaymentRetention').text(totalPaymentRetention);
                                                $("#PaymentConcepts").find('#TotalPaymentRetention').val(totalPaymentRetention);
                                            }, 3000);
                                        }
                                        else {
                                            //-1 indica que existe un error de parametrización.
                                            $("#alertForm").UifAlert('show', Resources.ReviewParameterization, "warning");
                                        }
                                    }
                                    
                                }
                                });
                            }, 500);
                        }
                        else {
                            rowModel.Taxes = 0;
                            rowModel.Retentions = 0;

                            setTimeout(function () {
                                $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView("addItem", rowModel);

                                totalPaymentAmount = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalAmountOtherPayments("PaymentConcetpsListView")))
                                totalPaymentTaxes = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalTax("PaymentConcetpsListView")))
                                totalPaymentRetention = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalRetention("PaymentConcetpsListView")))

                                $("#PaymentConcepts").find('#TotalPaymentAmount').text(totalPaymentAmount);
                                $("#PaymentConcepts").find('#TotalPaymentAmount').val(totalPaymentAmount);

                                $("#PaymentConcepts").find('#TotalPaymentTaxes').text(totalPaymentTaxes);
                                $("#PaymentConcepts").find('#TotalPaymentTaxes').val(totalPaymentTaxes);

                                $("#PaymentConcepts").find('#TotalPaymentRetention').text(totalPaymentRetention);
                                $("#PaymentConcepts").find('#TotalPaymentRetention').val(totalPaymentRetention);
                            }, 3000);
                        }
                    } else {
                        setTimeout(function () {
                            $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView("addItem", rowModel);

                            totalPaymentAmount = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalAmountOtherPayments("PaymentConcetpsListView")))
                            totalPaymentTaxes = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalTax("PaymentConcetpsListView")))
                            totalPaymentRetention = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalRetention("PaymentConcetpsListView")))

                            $("#PaymentConcepts").find('#TotalPaymentAmount').text(totalPaymentAmount);
                            $("#PaymentConcepts").find('#TotalPaymentAmount').val(totalPaymentAmount);

                            $("#PaymentConcepts").find('#TotalPaymentTaxes').text(totalPaymentTaxes);
                            $("#PaymentConcepts").find('#TotalPaymentTaxes').val(totalPaymentTaxes);

                            $("#PaymentConcepts").find('#TotalPaymentRetention').text(totalPaymentRetention);
                            $("#PaymentConcepts").find('#TotalPaymentRetention').val(totalPaymentRetention);
                        }, 3000);
                    }
                }
            });
            }, 500);
            MainPaymentRequest.ClearFieldsOtherPayments();
            $("#PaymentConcepts").find("#VoucherNumber").attr("disabled", "disabled");
            $("#PaymentConcepts").find("#CheckDate").attr("disabled", "disabled");
            $("#PaymentConcepts").find("#VoucherType").attr("disabled", "disabled");
            //$("#PaymentConcepts").find("#ExchangeRate").attr("disabled", "disabled");
            $("#PaymentConcepts").find("#VoucherCurrency").attr("disabled", "disabled");
            $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").attr("disabled", "disabled");
        }
        else {
            $("#alertForm").UifAlert('show', Resources.DuplicateRecords, "warning");
        }
    }

    /**
        * Valida que no existan registros duplicados en conceptos de pagos.
        *
        * @param {Number} paymentConceptCode - Identificador de concepto de pago.
        * @param {Number} costCenterCode     - Identificador de centro de costo.
        * 
        * @returns {boolean} true or false
        */
    static DuplicateRecordsOtherPayments(paymentConceptCode, costCenterCode) {
        var result = false;

        var ids = $("#PaymentConcetpsListView").UifListView("getData");

        if (ids != null) {
            for (var i in ids) {
                if ((paymentConceptCode == ids[i].PaymentConceptId) && (ids[i].CostCenterId == costCenterCode)) {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    /**
        * Valida que no existan # voucher duplicados.
        *
        * @param {Number} voucherNumber - Número de comprobante.
        * @returns {boolean} true or false
        */
    static DuplicateVoucherNumber(voucherNumber) {
        var result = false;

        //var ids = $("#taxesListView").UifListView("getData");
        var ids = $("#TablePaymentRequest").UifDataTable('getData')

        if (ids != null) {
            for (var i in ids) {
                if ((voucherNumber == ids[i].VoucherNumber)) {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    /**
        * Limpia campos y variables.
        *
        */
    static ClearFieldsOtherPayments() {
        $("#PaymentConcepts").find("#PaymentConceptId").val("");
        $("#PaymentConcepts").find("#PaymentConceptDescription").val("");
        $("#CostCenterId").val("");
        $("#CostCenterDescription").val("");
        $("#PaymentConceptIncomeAmount").val("");
        $("#PaymentConcepts").find("#ConceptAmount").val("");

        accountingAccountId = "-1";
        MainPaymentRequest.LoadCostCenter();
    }


    /**
        * Calcula los totales de la factura.
        *
        * @param {String} listView - Nombre del listview.
        * 
        * @returns {decimal} total
        */
    static TotalAmountOtherPayments(listView) {
        var total = 0;

        var ids = $("#" + listView).UifListView("getData");
        if (ids.length > 0) {
            for (var i in ids) {
                total += parseFloat(ids[i].Amount);
            }
        }
        return total;
    }


    /**
        * Calcula los totales de impuestos de la factura.
        *
        * @param {String} listView - Nombre del listview.
        * 
        * @returns {decimal} total
        */
    static TotalTax(listView) {
        var total = 0;

        var ids = $("#" + listView).UifListView("getData");
        if (ids.length > 0) {
            for (var i in ids) {
                total += parseFloat(ClearFormatCurrency(ids[i].Taxes.toString().replace("", ",")));
            }
        }
        return total;
    }

    /**
        * Calcula los totales de retención de la factura.
        *
        * @param {String} listView - Nombre del listview.
        * 
        * @returns {decimal} total
        */
    static TotalRetention(listView) {
        var total = 0;
        var ids = $("#" + listView).UifListView("getData");
        if (ids.length > 0) {
            for (var i in ids) {
                total += parseFloat(ClearFormatCurrency(ids[i].Retentions.toString().replace("", ",")));
            }
        }
        return total;
    }

    /**
        * Calcula los totales de la factura.
        *
        * @returns {decimal} total
        */
    static CalculateTotalPaymentRequest() {
        var total = 0;

        var ids = $("#TablePaymentRequest").UifDataTable('getData');
        if (ids.length > 0) {
            for (var i in ids) {
                total += parseFloat(ClearFormatCurrency(ids[i].Amount.replace("", ",")));
            }
        }
        return total;
    }


    /**
        * Calcula los totales de impuestos de la factura.
        *
        * @returns {decimal} total
        */
    static CalculateTotalTaxPaymentRequest() {
        var total = 0;

        var ids = $("#TablePaymentRequest").UifDataTable('getData');
        if (ids.length > 0) {
            for (var i in ids) {
                total += parseFloat(ClearFormatCurrency(ids[i].Taxes.toString().replace("", ",")));
            }
        }
        return total;
    }

    /**
        * Calcula los totales de retención de la factura.
        *
        * @returns {decimal} total
        */
    static CalculateTotalRetentionPAymentRequest() {
        var total = 0;
        var ids = $("#TablePaymentRequest").UifDataTable('getData');
        if (ids.length > 0) {
            for (var i in ids) {
                total += parseFloat(ClearFormatCurrency(ids[i].Retentions.toString().replace("", ",")));
            }
        }
        return total;
    }

    /**
        * Calcula el total a pagar de la solicitud de pagos varios.
        *
        * @returns {boolean} total
        */
    static CalculateTotalToPay() {
        var total = 0;
        var amount = 0;
        var tax = 0;
        var retention = 0;

        var ids = $("#TablePaymentRequest").UifDataTable('getData');
        if (ids.length > 0) {
            for (var i in ids) {
                amount += parseFloat(ClearFormatCurrency(ids[i].Amount.replace("", ",")));
                tax += parseFloat(ClearFormatCurrency(ids[i].Taxes.toString().replace("", ",")));
                retention += parseFloat(ClearFormatCurrency(ids[i].Retentions.toString().replace("", ",")));
            }
        }

        total = amount + tax + retention;

        return total;
    }

    /**
        * Limpia campos del formulario.
        *
        */
    static ClearPaymentConceptsModal() {
        $("#PaymentConcepts").find("#VoucherType").val("");
        $("#PaymentConcepts").find("#VoucherNumber").val("");
        $("#PaymentConcepts").find("#CheckDate").UifDatepicker('setValue', moment().format("DD/MM/YYYY"));

        var defaultCurrencyCode = $("#DefaultCurrencyCode").val()
        $("#VoucherCurrency").UifSelect("setSelected", defaultCurrencyCode);
        $("#VoucherCurrency").trigger("change");

        //$("#PaymentConcepts").find("#ExchangeRate").val("");
        $("#PaymentConcepts").find("#PaymentConceptId").val("");
        $("#PaymentConcepts").find("#PaymentConceptDescription").val("");
        $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").val("");
        $("#PaymentConcepts").find("#ConceptAmount").val("");
    }

    /**
    * Valida combos PaymentPayTo
    *
    */
    static validatePaymentPayTo() {
        return (
             ($("#PaymentPayTo").val() == $("#ViewBagSupplierCodeMainOther").val()) ||
            ($("#PaymentPayTo").val() == $("#ViewBagProducerCodeMainOther").val()) ||
            ($("#PaymentPayTo").val() == $("#ViewBagThirdPartyCodeMainOther").val()) ||
            ($("#PaymentPayTo").val() == $("#ViewBagAgentCodeMainOther").val()) ||
            ($("#PaymentPayTo").val() == $("#ViewBagEmployeeCodMainOther").val()) 
            );
    }


    /**
        * Limpia formato de Moneda.
        *
        */
    static SetHeaderPayment() {
        oPaymentRequestModel.TotalAmount = parseFloat(ClearFormatCurrency($("#PaymentTotal").val()).replace(",", ".").trim()); //PaymentRequestTotalAmount
    }

    /**
        * Setea objeto oPaymentClaimCoveragesModel.
        *
        */
    static SetDataPaymentRequest() {

        oPaymentRequestModel.PaymentRequestId = 0;                                   //PaymentRequestInfoId
        oPaymentRequestModel.PaymentSourceId = $("#PaymentOrigin").val();            //PaymentRequestSource
        oPaymentRequestModel.BranchId = $("#PaymentBranch").val();                   //PaymentRequestBranch
        oPaymentRequestModel.PaymentEstimateDate = $("#EstimatedPaymentDate").val(); //PaymentRequestEstimatedDate

        oPaymentRequestModel.PaymentDate = $("#PaymentAccountingDate").val();        //PaymentRequestPaymentDate
        oPaymentRequestModel.PersonTypeId = $("#PaymentPayTo").val();                //PaymentRequestPersonTypeId
        oPaymentRequestModel.IndividualId = individualId;                            //PaymentRequestIndividualId
        oPaymentRequestModel.PaymentMethodId = $("#PaymentType").val();              //PaymentRequestPaymentMethodId
        oPaymentRequestModel.CurrencyId = $("#PaymentCurrency").val();               //PaymentRequestCurrencyId

        oPaymentRequestModel.TotalAmount = parseFloat(ClearFormatCurrency($("#PaymentTotal").val()).replace(",", ".").trim()); //PaymentRequestTotalAmount
        oPaymentRequestModel.Description = $("#PaymentDescription").val();            //PaymentRequestDescription
        oPaymentRequestModel.PaymentMovementTypeId = $("#PaymentMovementType").val(); //PaymentRequestPaymentMovementTypeId

        oPaymentRequestModel.PersonBankAccountId = 0;                                 //PaymentRequestAccountBankId
        oPaymentRequestModel.PaymentRequestTypeId = 1;                                //PaymentRequestType
        oPaymentRequestModel.CompanyId = $("#PaymentAccountingCompany").val();        //PaymentRequestCompanyId
        oPaymentRequestModel.SalePointId = $("#PaymentSalesPoint").val();             //PaymentRequestSalePointId

        if (accountBankId == -1) {
            oPaymentRequestModel.PersonBankAccountId = 0;             //PaymentRequestAccountBankId
        }
        else {
            oPaymentRequestModel.PersonBankAccountId = accountBankId; //PaymentRequestAccountBankId
        }

        oPaymentRequestModel.PaymentTax = oTax.TaxCategory;

        /*LFR HOY
        oPaymentClaimModel.PaymentClaimId = $("#PaymentPayTo").val();

        //Solo si no hay registros ingresados se llena el objecto principal
        var taxListview = $("#taxesListView").UifListView("getData");
        if (taxListview.length == 0) {
            oPaymentClaimCoveragesModel.PaymentClaimCoveragesId = 0;
            oPaymentClaimCoveragesModel.PaymentClaimCoveragesSubClaim = 0;

            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountId = 0;
            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountEstimationTypeId = 0;
            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountVersion = 0;
            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountDate = null;
            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountEstimatedAmount = 0;
            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountDeductibleAmount = 0;
            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountPaymentTypeId = 0;
            oPaymentClaimCoveragesModel.PaymentClaimCoveragesClaimAmount.push(oPaymentClaimCoveragesClaimAmountModel);

            oPaymentClaimModel.PaymentClaimCoverages.push(oPaymentClaimCoveragesModel);
            oPaymentRequestModel.PaymentClaim.push(oPaymentClaimModel);
        }
        */
    }

    /**
        * Setea objeto oVoucherModel.
        *
        * @param {Object} rowModel - Objeto con información del voucher.
        * 
        * @returns {object} modelo
        */
    static SetDataVoucher(rowModel) {
        oVoucherModel.Index = rowModel.Index;
        oVoucherModel.VoucherId = rowModel.Index;
        oVoucherModel.VoucherType = rowModel.VoucherType;
        oVoucherModel.VoucherNumber = rowModel.VoucherNumber;
        oVoucherModel.VoucherDate = rowModel.VoucherDate;
        oVoucherModel.VoucherExchangeRate = rowModel.ExchangeRate.replace(",", ".");
        oVoucherModel.VoucherCurrencyId = rowModel.VoucherCurrencyId;

        taxHeaderModel.Index = rowModel.Index;

        var paymentConceptsIds = $("#PaymentConcetpsListView").UifListView("getData");

        if (paymentConceptsIds.length > 0) {
            if (editIndex != -1) {
                oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountVoucher.splice(editIndex, 1);
                taxModel.Concept.splice(editIndex, 1);
            }

            for (var i in paymentConceptsIds) {

                oVoucherConceptModel = {
                    VoucherConceptId: 0,
                    VoucherConceptPaymentConcept: 0,
                    VoucherConceptValue: 0,
                    VoucherConceptTaxValue: 0,
                    VoucherConceptDescription: "",
                    VoucherConceptRetentionValue: 0,
                    VoucherConceptCostCenterId: 0,
                    VoucherConceptTaxes: []
                };

                oVoucherConceptModel.VoucherConceptId = 0;
                oVoucherConceptModel.VoucherConceptPaymentConcept = paymentConceptsIds[i].PaymentConceptId;
                oVoucherConceptModel.VoucherConceptValue = parseFloat(paymentConceptsIds[i].Amount);
                oVoucherConceptModel.VoucherConceptTaxValue = paymentConceptsIds[i].Taxes.toString().replace(",", ".");
                oVoucherConceptModel.VoucherConceptDescription = paymentConceptsIds[i].PaymentConceptDescription;
                oVoucherConceptModel.VoucherConceptRetentionValue = paymentConceptsIds[i].Retentions.toString().replace(",", ".");
                oVoucherConceptModel.VoucherConceptCostCenterId = paymentConceptsIds[i].CostCenterId;

                
                /*
                for (var i = 0; i < oPaymentRequestModel.PaymentTax.length; i++) {
                    oVoucherConceptTax.TaxBase = 0;
                    oVoucherConceptTax.TaxCategoryDescription = "";
                    oVoucherConceptTax.TaxCategoryId = oPaymentRequestModel.PaymentTax[i].TaxCategoryId;
                    oVoucherConceptTax.TaxConditionId = oPaymentRequestModel.PaymentTax[i].TaxCondition;
                    oVoucherConceptTax.TaxRate = 0;
                    oVoucherConceptTax.TaxId = oPaymentRequestModel.PaymentTax[i].TaxId;
                    oVoucherConceptTax.TaxValue = 0;
                }
                */
                for (var j = 0; j < taxHeaderModel.TaxConcept.length; j++) {
                    if (paymentConceptsIds[i].Index == taxHeaderModel.TaxConcept[j].Index) {
                        oVoucherConceptTax = {
                            TaxId: 0,
                            TaxCategoryId: 0,
                            TaxConditionId: 0,
                            TaxCategoryDescription: "",
                            TaxRate: 0,
                            TaxBase: 0,
                            TaxValue: 0
                        };

                        oVoucherConceptTax.TaxBase = taxHeaderModel.TaxConcept[j].Amount;
                        oVoucherConceptTax.TaxCategoryDescription = "";
                        oVoucherConceptTax.TaxCategoryId = taxHeaderModel.TaxConcept[j].TaxCategoryId;
                        oVoucherConceptTax.TaxConditionId = taxHeaderModel.TaxConcept[j].TaxConditionId;
                        oVoucherConceptTax.TaxRate = taxHeaderModel.TaxConcept[j].Rate;
                        oVoucherConceptTax.TaxId = taxHeaderModel.TaxConcept[j].TaxId;
                        oVoucherConceptTax.TaxValue = taxHeaderModel.TaxConcept[j].TaxAmount;

                        oVoucherConceptModel.VoucherConceptTaxes.push(oVoucherConceptTax);
                    }
                }

                //oVoucherConceptModel.VoucherConceptTaxes.push(oVoucherConceptTax);

                oVoucherModel.VoucherConcepts.push(oVoucherConceptModel); //VoucherConcept
            }
            taxModel.Concept.push(taxHeaderModel);
            taxHeaderModel = {
                Index: 0,
                TaxConcept: []
            };
        }

        //oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountVoucher.push(oVoucherModel); LFR HOY
        oPaymentRequestModel.Vouchers.push(oVoucherModel);

        return oPaymentRequestModel;
    }

    /**
        * Valida totales de la solicitud.
        *
        * @param {Decimal} totComp  - Total componente.
        * @param {Decimal} totTax   - Total impuesto.
        * @param {Decimal} totToPay - Total a pagar.
        * 
        * @returns {boolean} true or false
        */
    static ValidTotal(totComp, totTax, totRetention, totToPay) {
        var totGen = parseFloat(totComp + totTax + totRetention).toFixed(2);
        return (totGen == totToPay) ? true : false;
    }

    /**
        * Valida transferencias.
        *
        * @returns {boolean} true or false
        */
    static ValidTransferSelected() {
        return (accountBankId == -1) ? false : true;
    }

    /**
        * Graba la solicitud de pagos varios.
        *
        */
    static SaveOtherPaymentRequest() {

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "PaymentRequest/SavePaymentRequest",
            data: JSON.stringify({ "paymentRequestModel": oPaymentRequestModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                //EXCEPCION ROLLBACK
                if (data.success == false) {

                    $("#alertMainOtherPayment").UifAlert('show', data.result, "danger");
                    MainPaymentRequest.ClearAllOtherPayments();

                } else {
                    if (data.PaymentRequestId != 0) {
                        $("#otherPaymentsRequestId").text(data.PaymentRequestNumber);
                        if (data.IsEnabledGeneralLedger == false) {
                            $("#OtherPaymentRequestAccountingMessageDiv").hide();
                        } else {
                            $("#OtherPaymentRequestAccountingMessageDiv").show();
                        }
                        $("#VoucherTaxesTotal").text("");
                        $("#VoucherRetentionsTotal").text("");

                        $("#OtherPaymentRequestAccountingMessage").text(data.SaveDailyEntryMessage);
                        paymentRequestId = data.PaymentRequestId;
                        $('#ModalSuccessPrint').UifModal('showLocal', $("#ViewBagSaveSuccess").val());
                    }
                    else {
                        $("#alertMainOtherPayment").UifAlert('show', Resources.PaymentRequestNumberNotParameterized, 'warninig');
                    }
                }
                $.unblockUI();
            }
        });//fin de ajax
    }

    /**
        * Limpia objeto oVoucherModel.
        *
        * @returns {object} modelo
        */
    static CleanDataVoucherEmpty() {
        oVoucherModel = {
            VoucherId: 0,
            VoucherType: 0,
            VoucherNumber: 0,
            VoucherDate: null,
            VoucherExchangeRate: 0,
            VoucherCurrencyId: 0,
            VoucherConcepts: [] //VoucherConcept
        };

        return oVoucherModel;
    }

    /**
        * Valida fecha.
        *
        * @param {String} dateTo - Fecha.
        * 
        * @returns {boolean} true or false
        */
    static ValidateDateAfter(dateTo) {
        //Fecha hasta
        var dateToDate = new Date();
        var dateToDay = dateTo.substr(0, 2);
        var dateToMonth = dateTo.substr(3, 2);
        var dateToYear = dateTo.substr(6, 4);

        dateToDate.setMonth(dateToMonth - 1);
        dateToDate.setDate(dateToDay);
        dateToDate.setYear(dateToYear);

        //Fecha Hoy
        var dateNow = new Date();
        var dateSystem = GetCurrentDate();
        var dayNow = dateSystem.substr(0, 2);
        var monthNow = dateSystem.substr(3, 2);
        var yearNow = dateSystem.substr(6, 4);

        dateNow.setMonth(monthNow - 1);
        dateNow.setDate(dayNow);
        dateNow.setYear(yearNow);

        if (dateToDate >= dateNow) {
            return true;
        }
        else {
            return false;
        }
    }


    /**
        * Borra registro de ListView genérico.
        *
        * @param {String} listviewName  - Nombre del listview.
        */
    static DelRowOtherPayments(listviewName) {
        var rowsPaymentConcepts = $("#PaymentConcetpsListView").UifListView("getData");
        if (rowsPaymentConcepts.length == 0) {
            $("#PaymentConcepts").find("#VoucherNumber").removeAttr("disabled");
            $("#PaymentConcepts").find("#CheckDate").removeAttr("disabled");
            $("#PaymentConcepts").find("#VoucherType").removeAttr("disabled");

            $("#PaymentConcepts").find("#VoucherCurrency").removeAttr("disabled");
            $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").removeAttr("disabled");
        }
        totalPaymentAmount = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalAmountOtherPayments(listviewName)))
        totalPaymentTaxes = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalTax(listviewName)))
        totalPaymentRetention = FormatCurrency(FormatDecimal(MainPaymentRequest.TotalRetention(listviewName)))

        if (listviewName == "taxesListView") {
            /*
            $('#VouchersTotal').text(totalPaymentAmount);
            $('#VoucherTaxesTotal').text(TotalPaymentTaxes);
            $('#VoucherRetentionsTotal').text(TotalPaymentRetention);
            */
        }
        else {
            $("#PaymentConcepts").find('#TotalPaymentAmount').text(totalPaymentAmount);
            $("#PaymentConcepts").find('#TotalPaymentAmount').val(totalPaymentAmount);

            $("#PaymentConcepts").find('#TotalPaymentTaxes').text(totalPaymentTaxes);
            $("#PaymentConcepts").find('#TotalPaymentTaxes').val(totalPaymentTaxes);

            $("#PaymentConcepts").find('#TotalPaymentRetention').text(totalPaymentRetention);
            $("#PaymentConcepts").find('#TotalPaymentRetention').val(totalPaymentRetention);
        }
    }

    /**
        * Carga reporte solicitud pagos.
        *
        * @param {Number} paymentRequestId  - Identificador de la solicitud de pagos.
        */
    static LoadOtherPaymentsRequestReport(paymentRequestId) {
        var controller = ACC_ROOT + "PaymentRequest/LoadPaymentRequestReport?paymentRequestId=" + paymentRequestId;
        window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }

    /**
        * Limpia campos y variables.
        *
        */
    static ClearAllOtherPayments() {
        index = 0;
        indexConcept = 0;
        $("#PaymentMovementType").val("");
        $("#PaymentBranch").val("");
        $("#PaymentSalesPoint").val("");
        $("#PaymentPayTo").val("");
        $("#OtherPaymentsDocumentNumber").val("");
        $("#OtherPaymentsFirstLastName").val("");
        $("#PaymentType").val("");
        //$("#PaymentCurrency").val(-1);
        $("#PaymentTotal").val("");
        $("#EstimatedPaymentDate").val("");
        $("#PaymentDescription").val("");

        MainPaymentRequest.CleanAutocompletesOtherPayment('SearchSuppliers');
        MainPaymentRequest.CleanAutocompletesOtherPayment('SearchInsured');
        MainPaymentRequest.CleanAutocompletesOtherPayment('SearchCoinsurance');
        MainPaymentRequest.CleanAutocompletesOtherPayment('SearchPerson');
        MainPaymentRequest.CleanAutocompletesOtherPayment('SearchAgent');
        MainPaymentRequest.CleanAutocompletesOtherPayment('SearchEmployee');
        MainPaymentRequest.CleanAutocompletesOtherPayment('SearchReinsurer');

        $("#BeneficiaryDocumentNumberData").show();
        $("#BeneficiaryNameData").show();

        $("#VouchersTotal").text("");
        $("#TotalTaxes").text("");
        $("#TotalRetention").text("");

        $("#taxesListView").UifListView({
            height: 200, 
            customAdd: true, customEdit: false, customDelete: false, localMode: true, add: true, edit: false, delete: true,
            displayTemplate: "#check-display",
            deleteCallback: deletetaxesListView
        });

        //dialogo de conceptos de pago
        paymentConceptId = "";


        setTimeout(function () {
            $("#VouchersTotal").text("");
            $("#VoucherTaxesTotal").text("");
            $("#VoucherRetentionsTotal").text("");
        }, 2000);


        individualId = 0;
        exchangeRate = 0;
        //************************
        taxModel = {
            Concept: []
        };
        taxHeaderModel = {
            Index: 0,
            TaxConcept: []
        };

        taxDetailModel = {
            TaxId: 0,
            TaxCategoryId: 0,
            TaxConditionId: 0,
            TaxDescription: "",
            TaxGroup: "",
            Amount: 0,
            Rate: 0,
            TaxAmount: 0,
            MinimumTax: 0,
        };
        //*********************

        MainPaymentRequest.ClearModel();

        //Variable PaymentTaxCategory
        oPaymentTaxCategoryModel = {
            TaxId: 0,
            TaxCategoryId: 0,
            TaxCondition: 0,
            TaxCategoryDescript: ""
        };
        $("#TablePaymentRequest").UifDataTable('clear');
    }

    /**
        * Sete alos modelos con valores iniciales.
        *
        */
    static ClearModel() {
        //Variable PaymentClaimCoverages
        oPaymentClaimCoveragesModel = {
            PaymentClaimCoveragesId: 0,
            PaymentClaimCoveragesSubClaim: 0,
            PaymentClaimCoveragesClaimAmount: []
        };
        //Variable PaymentClaim.
        oPaymentClaimModel = {
            PaymentClaimId: 0,
            PaymentClaimCoverages: []
        };

        //Variable para las Estimaciones
        oPaymentClaimCoveragesClaimAmountModel = {
            PaymentClaimCoveragesClaimAmountId: 0,
            PaymentClaimCoveragesClaimAmountEstimationTypeId: 0,
            PaymentClaimCoveragesClaimAmountVersion: 0,
            PaymentClaimCoveragesClaimAmountDate: null,
            PaymentClaimCoveragesClaimAmountEstimatedAmount: 0,
            PaymentClaimCoveragesClaimAmountDeductibleAmount: 0,
            PaymentClaimCoveragesClaimAmountPaymentTypeId: 0,
            PaymentClaimCoveragesClaimAmountVoucher: []
        };
        //Variable Voucher
        oVoucherModel = {
            VoucherId: 0,
            VoucherType: 0,
            VoucherNumber: 0,
            VoucherDate: null,
            VoucherExchangeRate: 0,
            VoucherCurrencyId: 0,
            VoucherConcepts: [] //VoucherConcept
        };
        //Variable VoucherConcept
        oVoucherConceptModel = {
            VoucherConceptId: 0,
            VoucherConceptPaymentConcept: 0,
            VoucherConceptValue: 0,
            VoucherConceptTaxValue: 0,
            VoucherConceptDescription: "",
            VoucherConceptRetentionValue: 0,
            VoucherConceptCostCenterId: 0,
            VoucherConceptTaxes: [],
        };
        oVoucherConceptTax = {
            TaxId: 0,
            TaxCategoryId: 0,
            TaxConditionId: 0,
            TaxCategoryDescription: "",
            TaxRate: 0,
            TaxBase: 0,
            TaxValue: 0
        };

        oPaymentRequestModel = {
            PaymentRequestId: 0,          //PaymentRequestInfoId
            PaymentSourceId: 0,           //PaymentRequestSource
            BranchId: 0,                  //PaymentRequestBranch
            PaymentEstimateDate: null,    //PaymentRequestEstimatedDate
            RegistrationDate: null,       //PaymentRequestRegistrationDate
            PaymentDate: null,            //PaymentRequestPaymentDate
            PersonTypeId: 0,              //PaymentRequestPersonTypeId
            IndividualId: 0,              //PaymentRequestIndividualId
            PaymentMethodId: 0,           //PaymentRequestPaymentMethodId
            CurrencyId: 0,                //PaymentRequestCurrencyId
            UserId: 0,                    //PaymentRequestUserId
            IsPrinted: false,             //PaymentRequestIsPrinted
            TotalAmount: 0,               //PaymentRequestTotalAmount
            Description: "",              //PaymentRequestDescription
            PaymentMovementTypeId: 0,     //PaymentRequestPaymentMovementTypeId
            PrefixId: 0,                  //PaymentRequestPrefixId
            PersonBankAccountId: 0,       //PaymentRequestAccountBankId
            PaymentRequestTypeId: 0,      //PaymentRequestType
            PaymentClaim: [],
            PaymentTax: [],
            CompanyId: 0,                 //PaymentRequestCompanyId
            SalePointId: 0,               //PaymentRequestSalePointId,
            Vouchers: []
        };
    }

    /**
        * Limpia autocomplete.
        *
        * @param {String} identifier  - Nombre del autocomplete.
        */
    static CleanAutocompletesOtherPayment(identifier) {

        $('#' + identifier + 'ByDocumentNumberRequest').val("");
        $('#' + identifier + 'ByNameRequest').val("");

        journalPayerIndividualId = 0;

        /*Vacía el Objeto oTax
        ----------------------*/
        oTax = { TaxCategory: [] };
        oPaymentRequestModel.PaymentTax = [];
        oTaxCategory = {};
        /*---------------------------*/

        $('#' + identifier + 'ByDocumentNumberRequest').parent().parent().hide();
        $('#' + identifier + 'ByNameRequest').parent().parent().hide();
    }

    /**
        * Carga autocomplete.
        *
        * @param {String} identifier  - Nombre del autocomplete.
        */
    static LoadAutocompleteEventOtherPayment(identifier) {
        $('#' + identifier + 'ByDocumentNumberRequest').on('itemSelected', function (event, selectedItem) {
            MainPaymentRequest.FillAutocompletesOtherPayments(identifier, selectedItem);
        });

        $('#' + identifier + 'ByNameRequest').on('itemSelected', function (event, selectedItem) {
            MainPaymentRequest.FillAutocompletesOtherPayments(identifier, selectedItem);
        });
    }

    /**
        * Visualiza mensaje de confirmación de grabación de solicitud de pagos.
        *
        * @param {Number} id             - Identificador de asegurado.
        * @param {String} identifier     - Nombre del autocomplete.
        * @param {String} individualName - Nombre del individuo.
        * @param {String} documentNumber - Número de documento del individuo.
        * @param {String} value          - Valor del mensaje.
        */
    static ShowConfirmOtherPayments(id, identifier, individualName, documentNumber, value) {
        changePayment = false;
        $.UifDialog('confirm', { 'message': Resources.ConfirmFilterMessage + '?', 'title': '' }, function (result) {
            if (result) {
                MainPaymentRequest.ClearAllOtherPayments();
                changePayment = true;
            }
            else {
                if (id == 1 && value > -1) {
                    $("#CreditCardType").val(value);
                }
                if (id == 2) {
                    $('#' + identifier + 'ByDocumentNumber').val(documentNumber);
                    $('#' + identifier + 'ByName').val(individualName);
                }
                changePayment = false;
            }
        });
    }

    /**
        * Eventos al seleccionar un individuo.
        *
        * @param {String} identifier   - Nombre del autocomplete.
        * @param {Object} selectedItem - Objeto con valores del individuo seleccionado.
        */
    static FillAutocompletesOtherPayments(identifier, selectedItem) {
        changePayment = true;
        if (changePayment) {

            if (selectedItem.Id != undefined && selectedItem.Id != -1) {

                if (selectedItem.IndividualId == undefined) {

                    individualId = selectedItem.Id;
                } else {
                    individualId = selectedItem.IndividualId;
                }

                individualName = selectedItem.Name;
                documentNumber = selectedItem.DocumentNumber;

                if (selectedItem.InsuredCode != undefined && selectedItem.InsuredCode != -1) {
                    personCode = selectedItem.InsuredCode;
                }
                if (selectedItem.SupplierCode != undefined && selectedItem.SupplierCode != -1) {
                    personCode = selectedItem.SupplierCode;
                }
            }
            else if (selectedItem.AgentId != undefined && selectedItem.AgentId != -1) {
                individualId = selectedItem.IndividualId;
                agentTypeId = selectedItem.AgentTypeId;
                personCode = selectedItem.AgentId;
            } else {
                individualId = selectedItem.CoinsuranceIndividualId;
            }

            if (selectedItem.Id != undefined && selectedItem.Id != -1) {
                //Impuestos por categoría
                MainPaymentRequest.OpenTaxSelectionOtherPayments(individualId);

                //Información de la cuenta bancaria.
                //MainPaymentRequest.ShowAccountInfo(individualId);
            }
        }

        if (individualId == undefined || individualId <= 0 || individualId == ""){
            $('#' + identifier + 'ByDocumentNumberRequest').val("");
            $('#' + identifier + 'ByNameRequest').val("");
        }else{
            $('#' + identifier + 'ByDocumentNumberRequest').val(selectedItem.DocumentNumber);
            $('#' + identifier + 'ByNameRequest').val(selectedItem.Name);
        }    
    }

    static BlurAutocompleteFields(identifier){
        $('#' + identifier + 'ByDocumentNumberRequest').on('blur', function(){            
            if(individualId == undefined || individualId <= 0 || individualId == ""){
                $('#' + identifier + 'ByDocumentNumberRequest').val("");
                $('#' + identifier + 'ByNameRequest').val("");
            }else{
                $('#' + identifier + 'ByDocumentNumberRequest').val(documentNumber);
                $('#' + identifier + 'ByNameRequest').val(individualName);
            }
        });
        $('#' + identifier + 'ByNameRequest').on('blur', function(){
            if(individualId == undefined || individualId <= 0 || individualId == ""){
                $('#' + identifier + 'ByDocumentNumberRequest').val("");
                $('#' + identifier + 'ByNameRequest').val("");
            }else{
                $('#' + identifier + 'ByDocumentNumberRequest').val(documentNumber);
                $('#' + identifier + 'ByNameRequest').val(individualName);
            }
        });
    }

    /**
        * Visualiza impuestos al seleccionar un individuo.
        *
        * @param {Number} individualId - Identificador único de individuo.
        */
    static LoadTaxCondition(individualId) {

        $("#ModalTaxCondition").find("#TaxConditionListView").UifListView(
            {
                source: ACC_ROOT + "PaymentRequest/GetIndividualTaxesByIndividualId?individualId=" + individualId,
                customDelete: false,
                customAdd: false,
                customEdit: true,
                edit: false,
                displayTemplate: "#taxCondicional-display"
            });
    }

    /**
        * Carga los impuestos al seleccionar un individuo.
        *
        * @param {Number} individualId - Identificador único de individuo.
        */
    static OpenTaxSelectionOtherPayments(individualId) {

        if (individualId != undefined && individualId > 0) {

            var controller = ACC_ROOT + "PaymentRequest/GetIndividualTaxCategoryConditionByIndividualId?individualId=" + individualId;
            $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable({ source: controller });
            $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });
            setTimeout(function () {
                MainPaymentRequest.ControlCheckOtherPayments();
                MainPaymentRequest.ShowModalTaxSelection();
            }, 1000);
        }
    }

    /**
        * Visualiza impuestos al seleccionar un individuo.
        *
        */
    static ShowModalTaxSelection() {
        $("#alertIndividualTax").UifAlert('hide');
        var count = $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable('getData');
        if (count.length > 0) {
            $('#modalTaxSelection').UifModal('showLocal', Resources.Taxes);
        }
        else {
            //Muestra la información de pago electronico.
            MainPaymentRequest.ShowAccountInfo(individualId);
        }
    }

    /**
        * Muestra modal de selección de categorias de impuestos.
        *
        */
    static ControlCheckOtherPayments() {
        /*Vacía el Objeto oTax
        ----------------------*/
        oTax = { TaxCategory: [] };
        oPaymentRequestModel.PaymentTax = [];
        oTaxCategory = {};
        /*---------------------------*/

        idsTax = $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable("getSelected");

        /**************************************/

        if (idsTax != null) {
            
            for (var i = 0; i < idsTax.length; i++) {
                var row = $("#modalTaxSelection").find("#TableIndividualTax").UifDataTable('getSelected')[i];
                oTaxCategory = {
                    TaxId: 0,
                    TaxCategoryId: 0,
                    TaxCategoryDescript: "",
                    TaxCondition: 0
                };

                oTaxCategory.TaxId = row.TaxCode;
                oTaxCategory.TaxCondition = row.TaxConditionCode;
                if (row.TaxCategoryCode == null) {
                    oTaxCategory.TaxCategoryId = "";
                } else {
                    oTaxCategory.TaxCategoryId = row.TaxCategoryCode;
                }
                if (row.TaxCategotyDescription == null) {
                    oTaxCategory.TaxCategoryDescript = "";
                } else {
                    oTaxCategory.TaxCategoryDescript = row.TaxCategotyDescription;
                }
                oTax.TaxCategory.push(oTaxCategory);
            }


            $("#ModalTaxes").find("#taxesView").UifListView({
                autoHeight: true,
                customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: false,
                displayTemplate: "#taxes-display"
            });
            //Arma el Objeto
            for (var k in idsTax) {
                var rowTax = idsTax[k];
                var rowTaxes = new RowTaxes();
                rowTaxes.TaxId = 0;
                rowTaxes.TaxDescription = rowTax.TaxDescription;
                rowTaxes.GroupCondition = rowTax.TaxConditionDescription;
                rowTaxes.GroupAmount = 0;
                rowTaxes.Rate = 0;
                rowTaxes.TaxAmount = 0;
                rowTaxes.MinimunTaxBase = 0;
                rowTaxes.MinimunTax = 0;
                rowTaxes.Branch = "";
                $('#taxesView').UifListView("addItem", rowTaxes);
            }
        }
        $('#modalTaxSelection').modal('hide');

        //Muestra la información de pago electronico.
        MainPaymentRequest.ShowAccountInfo(individualId);
    }

    /**
        * Obtiene los puntos de venta al seleccionar una sucursal.
        *
        */
    static GetSalePoints() {
        if ($('#PaymentBranch').val() != "") {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + $('#PaymentBranch').val();
            $("#PaymentSalesPoint").UifSelect({ source: controller });

            //Setea el punto de venta de default
            setTimeout(function () {
                $("#PaymentSalesPoint").val($("#ViewBagSalePointBranchUserDefault").val());
            }, 500);
        }
    }

    /**
        * Visualiza las cuentas bancarias del individuo cuando el pago es por transferencia.
        *
        * @param {Number} individualId - Identificador de individuo.
        */
    static ShowAccountInfo(individualId) {
        var accountInfoPromise = new Promise(function (resolve, reject) {
            if (individualId != undefined && individualId > 0) {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "PaymentRequest/GetPersonBankAccountsByIndividualId",
                    data: JSON.stringify({
                        "individualId": individualId
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (accountInfo) {
                        resolve(accountInfo);
                    }
                });
            }
        });

        accountInfoPromise.then(function (accountInfo) {
            if (accountInfo.length > 0) {
                $("#AccountBankInfoModal").find("#AccountType").val(accountInfo[0].BankAccountType.Description);
                $("#AccountBankInfoModal").find("#AccountNumber").val(accountInfo[0].Number);
                $("#AccountBankInfoModal").find("#BankDescription").val(accountInfo[0].Bank.Description);

                $('#AccountBankInfoModal').UifModal('showLocal', Resources.PaymentMeans);

                //Carga la información del tipo de pago.
                $("#PaymentCurrency").val(accountInfo[0].Currency.Id);
                $("#PaymentDescription").val("Dummy Voucher 123456, 654321");

                //Indica que los datos fueron cargados del pago electrónico
                $("#alertMainOtherPayment").UifAlert('show', Resources.DataLoadedFromElectronicPayment, "warning");

                var controller = ACC_ROOT + "PaymentRequest/GetEnabledPaymentTypes";
                $("#PaymentType").UifSelect({ source: controller });

                MainPaymentRequest.CheckPaymentMethodsLoaded();
                paymentMethodsPromise.then(function (isLoaded) {
                    if (isLoaded) {
                        clearTimeout(paymentMethodsTime);
                        $("#PaymentType").val($("#ViewBagParamPaymentMethodElectronicPayment").val());
                    }
                });

                //pendiente guardar la información del tipo de pago.


            } else {
                //muestra mensaje de que no existe información bancaria.
                $("#alertMainOtherPayment").UifAlert('show', Resources.BeneficiaryNotHavingAccountForElectronicPayment, "warning");

                //deshabilita la opción de "Pago electrónico"
                var newController = ACC_ROOT + "PaymentRequest/ExcludeElectronicPaymentFromEnablePaymentMethods";
                $("#PaymentType").UifSelect({ source: newController });
            }
        });
    }

    /**
        * Obtiene los orígenes de pago.
        *
        */
    static GetSelectPaymentOrigin() {
        if ($("#ViewBagImputationType").val() == undefined &&
            $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
            $("#ViewBagBillControlId").val() == undefined) {

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "Common/GetPaymentSources",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data != null) {
                        $.each(data, function (i, value) {
                            for (var i = 0; i < value.length; i++) {
                                if (value[i].Id == paymentSourceOthers) {
                                    $(paymentOrigin).append('<option value=' + value[i].Id + '>' + value[i].Description + '</option>');
                                }
                            }
                        });
                        $(paymentOrigin).trigger('change');
                    }
                }
            });
        }
    }

    /**
        * Deshabilita la compañía contable.
        *
        */
    static ValidateCompanyOtherPaymentRequest() {

        if ($("#PaymentAccountingCompany").val() != "" && $("#PaymentAccountingCompany").val() != null) {

            if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

                $("#PaymentAccountingCompany").attr("disabled", true);
            }
            else {
                $("#PaymentAccountingCompany").attr("disabled", false);
            }
            clearInterval(timeOtherPaymentRequest);
        }
    }

    /**
        * Valida si el concepto de pago esta cargado.
        *
        * @returns {boolean} true or false
        */
    static CheckPaymentMethodsLoaded() {
        var isLoaded;
        return paymentMethodsPromise = new Promise(function (resolve, reject) {
            paymentMethodsTime = setInterval(function () {
                if ($('#PaymentType').children('option').length > 0) {
                    isLoaded = true;
                    resolve(isLoaded);
                }
            }, 3);
        });
    }

}
