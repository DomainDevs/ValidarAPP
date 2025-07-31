var detailType = 0;
var holderDetails = null;
var currencyObject = {
    exchangeRate: 1,
    currencyType: 0,
    local: 0
}
var UnderwritingDecimal;
var gblCurrentRiskTemporalNumber = null;
var gblCurrentRiskTemporalNumberOld = null;
var glbpolicyEndorsementTexts = null;
var IsConsortium = false;
var IssueDate;
var Consolidation = {};
var dataList = [];
var policyHolderData = null;
var changeHolder = false;
var selectBranchId = 0;

class Underwriting extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        Underwriting.LoadAdditionalData();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        $('#tableIndividualResults').HideColums({ control: '#tableIndividualResults', colums: [1] });
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
        this.GetModuleDateIssue();
        this.LoadTime('selectHour', 24);
        $("#inputHolder").ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        $("#inputAgentPrincipal").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputBillingGroup").ValidatorKey(ValidatorType.lettersandnumbersSpecial, 4, 0);
        $("#inputRequest").ValidatorKey(ValidatorType.lettersandnumbersSpecial, ValidatorType.lettersandnumbersSpecial, 0);
        $("#inputFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputChange").OnlyDecimals(UnderwritingDecimal);
        $("#inputRegistrationNumber").OnlyDecimals(UnderwritingDecimal);
        //var temporalId = Underwriting.getQueryVariable("temporalId");
        var isCollective = Underwriting.getQueryVariable("isCollective") == null ? false : Underwriting.getQueryVariable("isCollective");
        var endorsement = Underwriting.getQueryVariable("endorsement") == null ? false : Underwriting.getQueryVariable("endorsement");
        if (glbPolicy.TemporalType == TemporalType.Quotation) {
            $("#PlateFieldRequired").removeClass("field-required");
            $("#EngineFieldRequired").removeClass("field-required");
            $("#ChassisFieldRequired").removeClass("field-required");
            $("#PlateFieldRequired2").removeClass("field-required");
            $("#EngineFieldRequired2").removeClass("field-required");
            $("#ChassisFieldRequired2").removeClass("field-required");
        }
        $("#selectJustificationDiv").hide();
        $("#ConceptsDiv").hide();

        if (glbPolicy.Id > 0 && glbPolicy.TemporalType == TemporalType.Endorsement) {
            lockScreen();
            UnderwritingTemporal.GetTemporalById(glbPolicy.Id);
        }
        else {
            if (isCollective == "true" && endorsement == "false") {
                lockScreen();
                UnderwritingTemporal.GetTemporalById(temporalId);
            }
            //Underwriting.GetDefaultValues();
            if (glbPersonOnline != null) {
                if (glbPersonOnline.Rol == 1) {

                    if (glbPersonOnline.IndividualId > 0) {
                        Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.DocumentNumber, InsuredSearchType.DocumentNumber, 1);
                        if (glbPersonOnline.CustomerType == CustomerType.Individual && glbPersonOnline.RolType < 1) {
                            Underwriting.ConvertProspectToInsured(glbPersonOnline.IndividualId, glbPersonOnline.DocumentNumber);
                        }
                    }
                    else {
                        $("#inputHolder").data("Object", null);
                        $("#inputHolder").data("Detail", null);
                        if (glbPersonOnline.DocumentNumber != null && glbPersonOnline.DocumentNumber.length > 0) {
                            $("#inputHolder").val(glbPersonOnline.DocumentNumber);
                        }
                        else {
                            $("#inputHolder").val(glbPersonOnline.Name == null ? "" : glbPersonOnline.Name);
                        }
                    }
                    UnderwritingTemporal.GetTemporalById(glbPolicy.Id);
                    Underwriting.LoadViewModel(glbPersonOnline.ViewModel);
                    //UnderwritingTemporal.UpdatePolicyComponents(glbPolicy.Id);
                    glbPersonOnline = null;
                }
                else {
                    UnderwritingTemporal.GetTemporalById(glbPolicy.Id);
                    Underwriting.LoadSiteRisk();
                }
            }
            else if (glbPolicy.Id > 0) {
                Underwriting.LoadTemporal(glbPolicy);
            }
            else {
                Underwriting.GetBranches(0);
                Underwriting.LoadDates();
                //Se realiza la acción solo cuando la variable glbPolicy esta vacía dvanegas
                Underwriting.GetDefaultAgent();
                Underwriting.GetJustificationSarlaft();
            }

            if (window.location.hash === "#prtQuotation") {
                $("#btnOpenProspectNatural").show();
                $("#btnOpenProspectLegal").show();
                $('#CreatePersonOrProspectMessage').show();
                $('#CreatePersonMessage').hide();
            } else {
                $("#btnOpenProspectNatural").hide();
                $("#btnOpenProspectLegal").hide();
                $('#CreatePersonOrProspectMessage').hide();
                $('#CreatePersonMessage').show();
            }
        }
        $("#chkCurrencyIssue").prop('disabled', true);
        $("#chkCurrencyLocal").prop('disabled', true);
        $("#chkCurrencyIssue").prop("checked", false);
    }

    //Seccion Eventos
    bindEvents() {
        $("#inputText").TextTransform(ValidatorType.UpperCase);
        $("#inputTextObservations").TextTransform(ValidatorType.UpperCase);
        $("#inputChange").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputChange").focusout(Underwriting.FormatMoneyOut);
        $("#btnDetail").click(this.ShowDetail);
        $("#inputAgentPrincipal").on('buttonClick', this.SearchAgentPrincipal);
        $('#selectAgentAgency').on('itemSelected', this.ChangeAgentAgency);
        $('#selectBranch').on('itemSelected', this.ChangeBranch);
        $('#selectPrefixCommercial').on('itemSelected', this.ChangePrefixCommercial);
        $('#selectProduct').on('itemSelected', this.ChangeProduct);
        $('#selectPolicyType').on('itemSelected', this.ChangePolicyType);
        $("#inputFrom").on('datepicker.change', this.ChangeFrom);
        //$("#inputFrom").off("blur");
        $("#inputTo").on('datepicker.change', this.ChangeTo);
        $('#selectCurrency').on('itemSelected', this.ChangeCurrency);
        $('#tableResults tbody').on('click', 'tr', this.SelectResults);
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividualResults);
        $("#btnIndividualDetailAccept").click(Underwriting.SetIndividualDetail);
        $("#btnExit").click(this.Exit);
        $("#btnDeleteTemporal").click(this.Delete);
        //consulta los grupos de facturación
        $("#inputBillingGroup").on('buttonClick', this.SearchBillingGroup);
        //Realiza búsqueda de solicitud agrupadora por descripción o código
        $("#inputRequest").on('buttonClick', this.SearchRequest);
        //Redireccionar riesgos
        $('#btnInclusionRisk').click(Underwriting.InclusionRisk);
        $("#btnAcceptNewPersonOnline").click(this.AcceptNewPersonOnline);
        //$('#btnConvertProspect').click(this.ConvertProspect);
        $('#btnConvertProspect').click(this.ConvertProspect);
        $("#btnOpenSup").on("click", Underwriting.OpenPerson);
        $("#btnOpenPersonNatural").on("click", Underwriting.OpenPersonNatural);
        $("#btnOpenPersonLegal").on("click", Underwriting.OpenPersonLegal);
        $("#btnOpenProspectNatural").on("click", Underwriting.OpenProspectusNatural);
        $("#btnOpenProspectLegal").on("click", Underwriting.OpenProspectusLegal);
        $("#SearchindividualId").on('buttonClick', UnderwritingTemporal.SearchByindividualId);
        $("#SearchCodeId").on('buttonClick', UnderwritingTemporal.SearchByindividualCode);
        $('input[type=radio][name=chkCurrency]').change(this.changeCurrencyCheck);
        $("#btnRiskFromPolicy").on("click", Underwriting.ShowModalRiskFromPolicy);
        $("#chkTemporal").click(function () {
            $("#selectBranchRisk").prop("disabled", true);
            $("#selectPrefixRisk").prop("disabled", true);
            $("#inputPolicyNumber").prop("disabled", true);
            $("#selectEndorsementRisk").prop("disabled", true);
            $("#inputRiskFromTemporal").prop("disabled", false)
        });
        $("#chkPolicy").click(function () {
            $("#selectBranchRisk").prop("disabled", false);
            $("#selectPrefixRisk").prop("disabled", false);
            $("#inputPolicyNumber").prop("disabled", false);
            $("#selectEndorsementRisk").prop("disabled", false);
            $("#inputRiskFromTemporal").prop("disabled", true);
        });
        $("#inputPolicyNumber").on('buttonClick', Underwriting.SearchRiskFromPolicy);
        $('#inputRiskFromTemporal').on('buttonClick', Underwriting.SearchRiskTemporal);
    }
    //Crear metodo para recalcular en los campos de FormatMoneyCurrency
    static setInputCurrency(CurrencyType) {
        FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.AmountInsured, $("#labelSum").prop("id"));
        FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.FullPremium, $("#labelTotalPremium").prop("id"));
        FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.Premium, $("#labelPremium").prop("id"));
        FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.Expenses, $("#labelExpenses").prop("id"));
        FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.Taxes, $("#labelTaxes").prop("id"));
    }

    static getExchangeRateModel(currencyId, CurrencyType) {
        UnderwritingRequest.GetExchangeRateByCurrencyId(currencyId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    currencyObject = {
                        exchangeRate: data.result.SellAmount,
                        local: CurrencyType,
                        currencyType: currencyId//glbPolicy.ExchangeRate.Currency.Id
                    }
                    Underwriting.setInputCurrency(CurrencyType);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRateType, 'autoclose': true });
        });
    }

    changeCurrencyCheck() {
        $('#chkCurrencyIssue').prop('checked', false);
        $('#chkCurrencyLoad').prop('checked', false);
        $('#chkCurrencyLocal').prop('checked', false);

        switch (this.value) {
            case 'Issue':
                Underwriting.getExchangeRateModel(CurrencyType.COP, 1);
                $('#chkCurrencyIssue').prop('checked', true);
                break;
            case 'Load':
                $('#chkCurrencyLoad').prop('checked', true);
                break;
            case 'Local':
                //Underwriting.getExchangeRateModel(CurrencyType.USD, 0);
                Underwriting.getExchangeRateModel(parseInt($('#selectCurrency').UifSelect('getSelected')), 0);
                $('#chkCurrencyLocal').prop('checked', true);
                break;
            default:
                $('#chkCurrencyIssue').prop('checked', true);
                break;
        }
    }

    //static getExchangeRateModel(currencyId) {
    //    UnderwritingRequest.GetExchangeRateByCurrencyId(currencyId).done(function (data) {
    //        if (data.success) {
    //            if (data.result != null) {
    //                currencyObject = {
    //                    exchangeRate: data.result.SellAmount,
    //                    currencyType: data.result.Currency.Id
    //                }
    //                Underwriting.setInputCurrency(currencyObject);
    //            }
    //        }
    //        else {
    //            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
    //        }
    //    }).fail(function (jqXHR, textStatus, errorThrown) {
    //        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRateType, 'autoclose': true });
    //    });
    //}

    Upperkeyup() {
        $(this).val($(this).val().toUpperCase());
    }

    static LoadAdditionalData() {
        if (glbPolicy.CalculateMinPremium == true) {
            $("#chkCalculateMinimumPremium_").prop('checked', true);
        }
        else {
            $("#chkCalculateMinimumPremium_").prop('checked', false);
        }
        Underwriting.LoadSubTitles(8);
        individualSearchType = 1;

    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        $(this).val(FormatMoney($(this).val()));
    }

    ShowDetail() {
        if ($("#inputHolder").data("Object") != undefined) {
            Underwriting.ShowModalHolderDetail();
        }
    }

    SearchAgentPrincipal() {
        agentSearchType = 1;
        if ($("#selectPrefixCommercial").val() > 0) {
            $.UifDialog('confirm', { 'message': AppResources.ConfirmChangeAgencyMain }, function (result) {
                if (result) {
                    Underwriting.GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipal").val().trim());
                } else {
                    Underwriting.LoadAgencyPrincipal(glbPolicy.Agencies);
                }
            });
        } else {
            Underwriting.GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipal").val().trim());
        }
    }

    ChangeAgentAgency(event, selectedItem) {
        if (selectedItem.Id > 0) {
            Underwriting.ValidateAgency(selectedItem.Id);
        }
        else {
            $("#selectAgentAgency").UifSelect();
        }
    }

    ChangeBranch(event, selectedItem) {
        if (selectedItem.Id > 0) {
            Underwriting.GetSalePointsByBranchId(selectedItem.Id, 0);
        }
        else {
            $("#selectSalePoint").UifSelect();
        }
    }
    
    ChangePrefixCommercial(event, selectedItem) {
        if (selectedItem.Id > 0) {
            Underwriting.GetProductsByAgentIdPrefixId($("#inputAgentPrincipal").data("Object").Agent.IndividualId, selectedItem.Id, 0);
        }
        else {
            $("#selectProduct").UifSelect();
            $("#selectPolicyType").UifSelect();
        }
        $('#additionalData').hide();
    }

    ChangeProduct(event, selectedItem) {
        if ($("#inputHolder").val() != "") {
            if (selectedItem.Id > 0) {
                Underwriting.ExecuteRulesPre(false);
                UnderwritingAdditionalData.GetCalculateMinPremiumByProductId(selectedItem.Id);
                if (glbPolicy.Summary.RiskCount > 0) {
                    Underwriting.ValidateDeleteRisk();
                }
            }
            else {
                $("#selectCurrency").UifSelect();
                $("#selectPolicyType").UifSelect();
            }
            glbPolicy.PaymentPlan = null;
            //if (selectedItem.Id > 0) {
            //    UnderwritingRequest.LoadProductForm(selectedItem.Id).done(function (data) {
            //        if (data.success) {
            //            if (data.result.length > 0) {
            //                $("#inputFrom").UifDatepicker('setValue', FormatDate(data.result[0].CurrentFrom));
            //                $("#inputTo").UifDatepicker('setValue', AddToDate($("#inputFrom").val(), 0, 0, 1));
            //            }
            //        }
            //    });
            //}
        }
        else {
            $("#selectProduct").val('');
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.SelectHolder, 'autoclose': true });
        }
    }
    static LoadDecimal() {
        UnderwritingRequest.GetDecimalByProductIdCurrencyId(glbPolicy.Product.Id, glbPolicy.ExchangeRate.Currency.Id).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    UnderwritingDecimal = data.result;
                }
            }
        });
    }

    ChangePolicyType(event, selectedItem) {
        if ($("#inputHolder").val() != "") {
            if (selectedItem.Id > 0) {
                if (glbPolicy.Summary.RiskCount > 0) {
                    Underwriting.ValidateDeleteRiskByChangePolicyType();
                }

            }
            //else {
            //    $("#selectCurrency").UifSelect();
            //}
            glbPolicy.PaymentPlan = null;
        }
        else {
            $("#selectPolicyType").val('');
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.SelectHolder, 'autoclose': true });
        }
    }

    ChangeFrom(event, date) {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
            $("#inputTo").UifDatepicker('setValue', AddToDate($("#inputFrom").val(), 0, 0, 1));
        }
        else if (CompareDates(FormatDate(glbPolicy.Endorsement.CurrentFrom), $("#inputFrom").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidatePolicyDate, 'autoclose': true });
            $("#inputFrom").UifDatepicker('setValue', GetCurrentFromDate());
        }
        Underwriting.CalculateDays();
    }

    ChangeTo(event, date) {
        if (CompareDates($("#inputFrom").val(), $("#inputTo").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalDate, 'autoclose': true });
            $("#inputTo").UifDatepicker('setValue', AddToDate($("#inputFrom").val(), 0, 0, 1));
        }
        Underwriting.CalculateDays();
    }

    ChangeCurrency(event, selectedItem) {
        if (selectedItem.Id != '') {
            Underwriting.GetExchangeRateByCurrencyId(selectedItem.Id);
        }
        else {
            $("#inputChange").val('');
        }
    }

    SelectResults(e) {
        switch (modalListType) {
            case 1:
                Underwriting.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
                break;
            case 2:
                UnderwritingBeneficiaries.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
                break;
            case 3:
                Underwriting.GetBillingGroupByDescription($(this).children()[1].innerHTML);
                break;
            case 4:
                Underwriting.GetCoRequestByDescription($(this).children()[1].innerHTML, true);
                break;
            default:
                break;
        }
        $('#modalDefaultSearch').UifModal("hide");
    }

    SelectIndividualResults(e) {
        var individualId = $(this).children()[0].innerHTML;
        var customerType = $(this).children()[1].innerHTML;
        Underwriting.GetHolderByIndividualId(individualId, customerType);

    }

    static GetHolderByIndividualId(individualId, customerType) {
        HoldersRequest.GetHoldersByIndividualId(individualId, customerType).done(function (resp) {
            if (resp.success) {
                let holder = resp.result.holder;
                holder.details = resp.result.details;

                Underwriting.LoadHolder(resp.result.holder);
            } else {
                showErrorToast(resp.result);
            }

        });

        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    Delete() {
        glbPolicy.Id = $("#inputTemporal").val();
        Underwriting.DeleteTemporal();
    }

    SearchBillingGroup() {
        if ($("#inputBillingGroup").val().trim().length > 0) {
            Underwriting.GetBillingGroupByDescription($("#inputBillingGroup").val().trim());
        }
    }

    SearchRequest() {
        if ($("#inputRequest").val().trim().length > 0) {
            Underwriting.GetCoRequestByDescription($("#inputRequest").val().trim(), true);
        }
    }

    static InclusionRisk(e) {
        Underwriting.LoadDecimal();
        if (glbPolicy.Endorsement.QuotationId == 0 && glbPolicy.TemporalType == TemporalType.Quotation) {
            glbPolicy.TemporalType = TemporalType.TempQuotation;
        } else if ((glbPolicy.TemporalType != TemporalType.TempQuotation && glbPolicy.TemporalType != TemporalType.Quotation)) {
            window.isPolicy = true;
        }
        e.preventDefault();
        Underwriting.RedirectToRisk();
        $("#formAdvancedQuotation").parent().html("");
        $("#formAdvancedTemporal").parent().html("");
    }

    AcceptNewPersonOnline() {
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel(false)
        };
        UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputHolder").val().trim());
    }

    ConvertProspect() {
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel(false)
        };
        UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputHolder").data("Object").IndividualId, $("#inputHolder").data("Object").IndividualType, $("#inputHolder").data("Object").CustomerType, 0);
    }

    GetModuleDateIssue() {
        lockScreen();
        UnderwritingRequest.GetModuleDateIssue().done(function (data) {
            if (data.success) {
                $("#inputIssueDate").text(FormatFullDate(data.result));
                IssueDate = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            showErrorToast(AppResources.ErrorSearchDateIssue);
        });
    }

    LoadTime(selectId, amountItems) {
        $('#' + selectId).prop('disabled', false)
        var selectedTime = 0;
        for (var i = 0; i < amountItems; i++) {
            if (i < 10) {
                if (i == selectedTime) {
                    $('#' + selectId).append($('<option>', {
                        value: i < 10 ? '0' + i : '' + i,
                        text: i < 10 ? '0' + i : '' + i,
                        selected: true
                    }));
                }
                else {
                    $('#' + selectId).append($('<option>', {
                        value: i < 10 ? '0' + i : '' + i,
                        text: i < 10 ? '0' + i : '' + i
                    }));
                }
            }
            else {
                $('#' + selectId).append($('<option>', {
                    value: i,
                    text: i
                }));
            }
        }
    }

    static getQueryVariable(variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) {
                return pair[1];
            }
        }
    }

    static GetDefaultValues() {
        UnderwritingRequest.GetDefaultValues().done(function (data) {
            if (data.success) {
                Underwriting.SetDefaultValues(data.result);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showErrorToast(AppResources.ErrorSearchDefaultData);
        });
    }

    static GetDefaultAgent() {

        UnderwritingRequest.GetDefaultAgent().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    agentSearchType = 1;
                    $("#inputAgentPrincipal").data("Object", data.result);
                    $('#inputAgentPrincipal').next().prop("disabled", "true")
                    $("#inputAgentPrincipal").prop('disabled', true);
                    $("#inputAgentPrincipal").val(data.result.Agent.FullName);
                    Underwriting.GetAgenciesByAgentId(data.result.Agent.IndividualId, data.result.Id); //Carga las agencias del intermediario
                    Underwriting.GetPrefixesByAgentId(data.result.Agent.IndividualId, 0);

                }
            }
            else {
                showInfoToast(data.result);
            }
        });
    }

    static GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        var number = parseInt(description, 10);
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
        var dataList = [];
        if (description != undefined) {
            if ((!isNaN(number) || description.length > 2) && (description != 0)) {
                UnderwritingRequest.GetHolders(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 0 && individualSearchType == 2) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                        }
                        else if (data.result.length == 0) {
                            UnderwritingPersonOnline.ShowOnlinePerson();
                        }
                        else if (data.result.length == 1) {
                            Underwriting.LoadHolder(data.result[0]);
                        }
                        else if (data.result.length > 1) {
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].IndividualId,
                                    CustomerType: data.result[i].CustomerType,
                                    Code: data.result[i].InsuredId,
                                    DocumentNum: data.result[i].IdentificationDocument.Number,
                                    Description: data.result[i].Name,
                                    CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                    DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            Underwriting.ShowIndividualResults(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelSelectHolder);
                        }
                    }
                    else {

                        $('#modalNotificationPerson').UifModal('showLocal', "Crear Persona");

                        if (individualSearchType == 2) {
                            $("#inputBeneficiaryName").data("Object", null);
                            $("#inputBeneficiaryName").data("Detail", null);
                            $("#inputBeneficiaryName").val('');
                        }
                        else {
                            $("#inputHolder").data("Object", null);
                            $("#inputHolder").data("Detail", null);
                            //$("#inputHolder").val('');
                        }
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
                });
            }
        }

    }

    static GetIndividualsByIndividualId(description, insuredSearchType, customerType) {
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            UnderwritingRequest.GetHolders(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0 && individualSearchType == 2) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                    }
                    else if (data.result.length == 0) {
                        UnderwritingPersonOnline.ShowOnlinePerson();
                    }
                    else if (data.result.length >= 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].InsuredId,
                                DocumentNum: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                            });
                        }

                        Underwriting.ShowIndividualResults(dataList);
                        $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelSelectHolder);
                    }
                }
                else {

                    $('#modalNotificationPerson').UifModal('showLocal', "Crear Persona");

                    if (individualSearchType == 2) {
                        $("#inputBeneficiaryName").data("Object", null);
                        $("#inputBeneficiaryName").data("Detail", null);
                        $("#inputBeneficiaryName").val('');
                    }
                    else {
                        $("#inputHolder").data("Object", null);
                        $("#inputHolder").data("Detail", null);
                        $("#inputHolder").val('');
                    }
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
            });
        }
    }

    static GetIndividualsByIndividualCode(description, insuredSearchType, customerType) {
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            UnderwritingRequest.GetHolders(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0 && individualSearchType == 2) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                    }
                    else if (data.result.length == 0) {
                        UnderwritingPersonOnline.ShowOnlinePerson();
                    }
                    else if (data.result.length >= 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].InsuredId,
                                DocumentNum: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                            });
                        }

                        Underwriting.ShowIndividualResults(dataList);
                        $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelSelectHolder);
                    }
                }
                else {

                    $('#modalNotificationPerson').UifModal('showLocal', "Crear Persona");

                    if (individualSearchType == 2) {
                        $("#inputBeneficiaryName").data("Object", null);
                        $("#inputBeneficiaryName").data("Detail", null);
                        $("#inputBeneficiaryName").val('');
                    }
                    else {
                        $("#inputHolder").data("Object", null);
                        $("#inputHolder").data("Detail", null);
                        $("#inputHolder").val('');
                    }
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
            });
        }
    }

    static ValidateBeneficiary(individualId, documentNumber) {
        // Si es Auto --- Faltan los otros ramos
        if (glbPolicy.Id > 0) {
            RiskVehicle.GetRisksByTemporalIdQuotation(glbPolicy.Id, individualId, documentNumber);
        }
    }


    static ConvertProspectToInsured(individualId, documentNumber) {
        if (glbPolicy.Id > 0) {
            UnderwritingRequest.ConvertProspectToInsured(glbPolicy.Id, individualId, documentNumber, Underwriting.GetControllerRisk()).done(function (data) {
                if (!data.success) {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Underwriting.ClearControls();
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }

    static GetControllerRisk() {
        if (glbPolicy != null) {
            if (glbPolicy.Product == undefined)
                return;
            switch (glbPolicy.Product.CoveredRisk.SubCoveredRiskType) {
                case SubCoveredRiskType.Vehicle:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#formVehicle";
                        glbRisk.Class = RiskVehicle;
                        glbRisk.Object = "RiskVehicle";
                    }
                    return "RiskVehicle";
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#formRiskTPL";
                        glbRisk.Class = RiskThirdPartyLiability;
                        glbRisk.Object = "RiskThirdPartyLiability";
                    }
                    return "RiskThirdPartyLiability";
                    break;
                case SubCoveredRiskType.Property:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#formProperty";
                        glbRisk.Class = Property;
                        glbRisk.Object = "Property";
                    }
                    return "RiskProperty";
                    break;
                case SubCoveredRiskType.Liability:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#formLiability";
                        glbRisk.Class = RiskLiability;
                        glbRisk.Object = "RiskLiability";
                    }
                    return "RiskLiability";
                    break;
                case SubCoveredRiskType.Surety:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#mainRiskSurety";
                        glbRisk.Class = RiskSurety;
                        glbRisk.Object = "RiskSurety";
                    }
                    return "RiskSurety";
                    break;
                case SubCoveredRiskType.Lease:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#mainRiskSurety";
                        glbRisk.Class = RiskSurety;
                        glbRisk.Object = "RiskSurety";
                    }
                    return "RiskSurety";
                    break;
                case SubCoveredRiskType.JudicialSurety:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#formJudicialSurety";
                        glbRisk.Class = RiskJudicialSurety;
                        glbRisk.Object = "RiskJudicialSurety";
                    }
                    return "RiskJudicialSurety";
                    break;
                case SubCoveredRiskType.Transport:
                    if (!Isnull(glbRisk)) {
                        glbRisk.formRisk = "#formTransport";
                        glbRisk.Class = RiskTransport;
                        glbRisk.Object = "RiskTransport";
                    }
                    return "RiskTransport";
                    break;
                case SubCoveredRiskType.Fidelity:
                    return "RiskFidelity";
                    break;
                case SubCoveredRiskType.Aircraft:
                    return "RiskAircraft";
                    break;
                case SubCoveredRiskType.Marine:
                    return "RiskMarine";
                    break;
                case SubCoveredRiskType.FidelityNewVersion:
                    return "RiskFidelity";
                    break;
            }
        }
        else {
            window.location = rootPath + "Home/Index";
        }
    }

    static LoadViewModel(viewModel) {
        if (viewModel.AgentId > 0 && viewModel.AgentCode > 0) {
            agentSearchType = 1;
            Underwriting.GetAgenciesByAgentIdDescription(viewModel.AgentId, viewModel.AgentCode);
            Underwriting.GetPrefixesByAgentId(viewModel.AgentId, viewModel.PrefixId);
            if (viewModel.PrefixId > 0) {
                Underwriting.GetProductsByAgentIdPrefixId(viewModel.AgentId, viewModel.PrefixId, viewModel.ProductId);
                if (viewModel.ProductId > 0) {
                    Underwriting.GetPolicyTypesByProductId(viewModel.ProductId, viewModel.PolicyType);
                    Underwriting.GetCurrenciesByProductId(viewModel.ProductId, viewModel.Currency);
                }
            }
        }
        Underwriting.GetBranches(viewModel.BranchId);
        if (viewModel.BranchId > 0) {
            Underwriting.GetSalePointsByBranchId(viewModel.BranchId, viewModel.SalePoint);
            CommonAgent.GetSalePointsByBranchId(viewModel.BranchId, 0);
        }

        if (viewModel.CurrentFrom != "") {
            $("#inputFrom").UifDatepicker('setValue', viewModel.CurrentFrom);
        }
        if (viewModel.CurrentTo != "") {
            $("#inputTo").UifDatepicker('setValue', viewModel.CurrentTo);
        }
        if (viewModel.CurrentFrom != "" && viewModel.CurrentTo != "") {
            Underwriting.CalculateDays();
        }
    }

    static LoadSiteRisk() {
        if (glbPolicy != null) {
            var uriSPA = "prt" + Underwriting.GetControllerRisk();
            router.run(uriSPA);
        }
        else {
            window.location = rootPath + "Home/Index";
        }
    }

    static LoadTemporal(policyData) {
        UnderwritingQuotation.DisabledButtonsQuotation();
        if (policyData.TemporalType == TemporalType.Quotation) {
            $("#inputQuotation").val(policyData.Endorsement.QuotationId);
            $("#inputQuotation").val("No. cotización " + policyData.Endorsement.QuotationId);

        }
        else if (policyData.TemporalType == TemporalType.TempQuotation) {
            $("#inputQuotation").val(policyData.Id);
            $("#inputQuotation").val(" No. Temporal " + policyData.Id);
        }
        else {
            $("#inputTemporal").val(policyData.Id);
        }

        if ($("#inputHolder").data("Object") == null) {
            Underwriting.LoadHolder(policyData.Holder);
        } else {
            Underwriting.LoadHolder($("#inputHolder").data("Object"));
        }



        agentSearchType = 1;

        //-----------------------------------------------------------------
        var isCollective = Underwriting.getQueryVariable("isCollective");

        if (isCollective != "true") {
            isCollective = false;
        }
        Underwriting.LoadPolicyCombos(policyData, isCollective);
        Underwriting.LoadAgencyPrincipal(policyData.Agencies);
        Underwriting.GetBranches(policyData.Branch.Id);
        Underwriting.GetJustificationSarlaft(policyData.JustificationSarlaft);

        if (policyData.Endorsement.EndorsementType == EndorsementType.Emission || (policyData.Endorsement.EndorsementType == EndorsementType.Renewal && policyData.Endorsement.TemporalId > 0))
            Underwriting.LoadTicket(policyData.Endorsement);
        Underwriting.LoadCalculateMinimumPremium(policyData.CalculateMinPremium);
        $("#inputFrom").UifDatepicker('setValue', FormatDate(policyData.CurrentFrom));
        $("#inputTo").UifDatepicker('setValue', FormatDate(policyData.CurrentTo));
        Underwriting.CalculateDays();
        $("#selectHour").val(policyData.TimeHour);
        $("#inputChange").val(FormatMoney(policyData.ExchangeRate.SellAmount));
        if (policyData.ExchangeRate.SellAmount == 1) {
            $("#inputChange").prop('disabled', true);
        }
        else {
            $("#inputChange").prop('disabled', false);
        }
        if (policyData.BillingGroup != null && policyData.BillingGroup.Id > 0) {
            Underwriting.GetBillingGroupByDescription(policyData.BillingGroup.Id);
        }
        if (policyData.Request != null && policyData.Request.Id > 0) {
            Underwriting.GetCoRequestByDescription(policyData.Request.Id, false);
            Underwriting.DisabledControlRequest(true);
        }

        dynamicProperties = policyData.DynamicProperties;
        Underwriting.LoadSummary(policyData.Summary);

        Underwriting.LoadTitle(policyData);
        Underwriting.LoadSubTitles(0);


        if (glbPolicy.Endorsement != null) {
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Renewal) {
                $('#btnIssuePolicy').text(AppResources.PolicyRenovate);
            }
            else {
                $('#btnIssuePolicy').text(AppResources.IssuePolicy);
            }
        }
        else {
            $('#btnIssuePolicy').text(AppResources.IssuePolicy);
        }
    }
    static LoadTemporalRisk(policyData) {
        UnderwritingQuotation.DisabledButtonsQuotation();
        if (policyData.TemporalType == TemporalType.Quotation) {
            $("#inputQuotation").val(policyData.Endorsement.QuotationId);
            $("#inputQuotation").val("No. cotización " + policyData.Endorsement.QuotationId);

        }
        else if (policyData.TemporalType == TemporalType.TempQuotation) {
            $("#inputQuotation").val(policyData.Id);
            $("#inputQuotation").val(" No. Temporal " + policyData.Id);
        }
        else {
            $("#inputTemporal").val(policyData.Id);
        }

        if ($("#inputHolder").data("Object") == null) {
            Underwriting.LoadHolder(policyData.Holder);
        } else {
            Underwriting.LoadHolder($("#inputHolder").data("Object"));
        }



        agentSearchType = 1;

        //-----------------------------------------------------------------
        var isCollective = Underwriting.getQueryVariable("isCollective");

        if (isCollective != "true") {
            isCollective = false;
        }
        Underwriting.LoadPolicyCombos(policyData, isCollective);
        Underwriting.LoadAgencyPrincipal(policyData.Agencies);
        Underwriting.GetBranches(policyData.Branch.Id);
        Underwriting.GetJustificationSarlaft(policyData.JustificationSarlaft);

        if (policyData.Endorsement.EndorsementType == EndorsementType.Emission || (policyData.Endorsement.EndorsementType == EndorsementType.Renewal && policyData.Endorsement.TemporalId > 0))
            Underwriting.LoadTicket(policyData.Endorsement);
        Underwriting.LoadCalculateMinimumPremium(policyData.CalculateMinPremium);
        $("#inputFrom").UifDatepicker('setValue', FormatDate(policyData.CurrentFrom));
        $("#inputTo").UifDatepicker('setValue', FormatDate(policyData.CurrentTo));
        Underwriting.CalculateDays();
        $("#selectHour").val(policyData.TimeHour);
        $("#inputChange").val(FormatMoney(policyData.ExchangeRate.SellAmount));
        if (policyData.ExchangeRate.SellAmount == 1) {
            $("#inputChange").prop('disabled', true);
        }
        else {
            $("#inputChange").prop('disabled', false);
        }
        if (policyData.BillingGroup != null && policyData.BillingGroup.Id > 0) {
            Underwriting.GetBillingGroupByDescription(policyData.BillingGroup.Id);
        }
        if (policyData.Request != null && policyData.Request.Id > 0) {
            Underwriting.GetCoRequestByDescription(policyData.Request.Id, false);
            Underwriting.DisabledControlRequest(true);
        }

        dynamicProperties = policyData.DynamicProperties;
        Underwriting.LoadSummary(policyData.Summary);

        Underwriting.LoadTitle(policyData);
        Underwriting.LoadSubTitles(0);


        if (glbPolicy.Endorsement != null) {
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Renewal) {
                $('#btnIssuePolicy').text(AppResources.PolicyRenovate);
            }
            else {
                $('#btnIssuePolicy').text(AppResources.IssuePolicy);
            }
        }
        else {
            $('#btnIssuePolicy').text(AppResources.IssuePolicy);
        }
        Underwriting.RedirectToRisk();
    }
    static SetIndividualDetails(individualDetails) {
        var TradeNameDetails = AppResources.ErrorHolderNo;
        if (individualDetails != null && individualDetails.length > 0) {

            for (let i = 0; i < individualDetails.length; i++) {
                individualDetails[i].Detail = individualDetails[i].Address.Description;
                if (individualDetails[i].TradeName != null) {
                    individualDetails[i].Detail = '<b>' + individualDetails[i].TradeName + '</b>' + '<br/>' + individualDetails[i].Detail;
                }
                if (individualDetails[i].Phone != null) {
                    individualDetails[i].Detail += '<br/>' + individualDetails[i].Phone.Description;
                }
                if (individualDetails[i].Email != null) {
                    individualDetails[i].Detail += '<br/>' + individualDetails[i].Email.Description;
                }

                if (individualSearchType == 2) {
                    if ($("#inputBeneficiaryName").data("Object").CompanyName == null) {
                        if (individualDetails[i].IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = individualDetails[i];
                        }
                    }
                    else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && individualDetails[i].IsMain) {
                        $("#inputBeneficiaryName").data("Object").CompanyName = individualDetails[i];
                    }
                }
                else {
                    if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName == null) {
                        if (individualDetails[i].IsMain) {
                            $("#inputHolder").data("Object").CompanyName = individualDetails[i];
                        }
                    }
                    else if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.NameNum == 0 && individualDetails[i].IsMain) {
                        $("#inputHolder").data("Object").CompanyName = individualDetails[i];
                    }
                }
            };

        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorHolderCorrespondenceAddress, 'autoclose': true });
        }
        return individualDetails;
    }

    static SetIndividualDetail() {
        var details = $('#tableIndividualDetails').UifDataTable('getSelected');

        if (details == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectAddress, 'autoclose': true });
        }
        else {
            if (detailType == 1) {
                $("#inputHolder").data("Object").CompanyName = details[0];
            }
            else if (detailType == 2) {
                $("#inputBeneficiaryName").data("Object").CompanyName = details[0];
            }

            $('#modalIndividualDetail').UifModal('hide');
        }
    }

    static GetPolicyModel(redirect) {
        var policyData = $("#formUnderwriting").serializeObject();
        policyData.Id = glbPolicy.Id;
        switch (glbPolicy.Endorsement.QuotationId) {
            case 0:
                if (glbPolicy.TemporalType == 1) {
                    policyData.TemporalType = TemporalType.TempQuotation;
                }
                else {
                    policyData.TemporalType = glbPolicy.TemporalType;
                }
                break;
            case -1:
                glbPolicy.Endorsement.QuotationId = 0;
                policyData.TemporalType = glbPolicy.TemporalType;
                break;
            default:
                policyData.TemporalType = glbPolicy.TemporalType;
                break;
        }

        policyData.EndorsementType = glbPolicy.Endorsement.EndorsementType;
        policyData.QuotationId = glbPolicy.Endorsement.QuotationId;
        policyData.QuotationVersion = glbPolicy.Endorsement.QuotationVersion;
        policyData.IssueDate = FormatFullDate($("#inputIssueDate").text());

        if ($("#inputHolder").data("Object") != null) {
            policyData.HolderName = $("#inputHolder").data("Object").Name;
            policyData.HolderId = $("#inputHolder").data("Object").IndividualId;
            policyData.HolderIndividualType = $("#inputHolder").data("Object").IndividualType;
            policyData.HolderCustomerType = $("#inputHolder").data("Object").CustomerType;
            policyData.HolderBirthDate = FormatDate($("#inputHolder").data("Object").BirthDate);
            policyData.HolderGender = $("#inputHolder").data("Object").Gender;
            policyData.HolderIdentificationDocument = $("#inputHolder").data("Object").IdentificationDocument.Number;
            policyData.HolderInsuredId = $("#inputHolder").data("Object").InsuredId;
            policyData.HolderDeclinedDate = FormatDate($("#inputHolder").data("Object").DeclinedDate);
            policyData.HolderDocumentType = $("#inputHolder").data("Object").IdentificationDocument.DocumentType.Id;
            if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName != null) {
                policyData.HolderDetailId = $("#inputHolder").data("Object").CompanyName.NameNum;
                policyData.HolderAddressId = $("#inputHolder").data("Object").CompanyName.Address.Id;
                if ($("#inputHolder").data("Object").CompanyName.Address.City != null) {
                    policyData.HolderStateId = $("#inputHolder").data("Object").CompanyName.Address.City.State.Id;
                    policyData.HolderCountryId = $("#inputHolder").data("Object").CompanyName.Address.City.State.Country.Id;
                }
                if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.Phone != null) {
                    policyData.HolderPhoneId = $("#inputHolder").data("Object").CompanyName.Phone.Id;
                }
                if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.Email != null) {
                    policyData.HolderEmailId = $("#inputHolder").data("Object").CompanyName.Email.Id;
                    policyData.HolderEmailDescription = $("#inputHolder").data("Object").CompanyName.Email.Description;
                }
            }

            if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").PaymentMethod != null) {
                policyData.HolderPaymentMethodId = $("#inputHolder").data("Object").PaymentMethod.Id;

                policyData.HolderPaymentIdId = $("#inputHolder").data("Object").PaymentMethod.Id;
            }

            if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").AssociationType != null) {
                policyData.HolderAssociationType = $("#inputHolder").data("Object").AssociationType.Id;
            }
            if (policyData.HolderIdentificationDocument == null && policyData.HolderName != null) {
                policyData.HolderIdentificationDocument = policyData.HolderName;
            }
        }
        if ($("#inputAgentPrincipal").data("Object") != null) {
            policyData.AgentId = $("#inputAgentPrincipal").data("Object").Agent.IndividualId;
            policyData.AgentName = $("#inputAgentPrincipal").data("Object").Agent.FullName;
            policyData.AgencyName = $("#inputAgentPrincipal").data("Object").Agent.FullName;
            policyData.AgentCode = $("#inputAgentPrincipal").data("Object").Code;
            if ($("#inputAgentPrincipal").data("Object").Agent.AgentType != null) {
                policyData.AgentType = $("#inputAgentPrincipal").data("Object").Agent.AgentType.Id;
            }

            //if ($("#inputAgentPrincipal").data("Object").Branch != null) {
            //    policyData.AgencyBranchId = $("#inputAgentPrincipal").data("Object").Branch.Id;
            //}

            policyData.AgencyBranchId = $('#selectBranch').find('option:selected').val();
            policyData.AgencyBranchDesc = $('#selectBranch').find('option:selected').text();
            policyData.AgencyBranchSalePointId = $('#selectSalesPoint').find('option:selected').val(),
            policyData.AgencyBranchSalePointDesc = $('#selectSalesPoint').find('option:selected').text()
        }
        policyData.BusinessType = glbPolicy.BusinessType;
        policyData.ExchangeRate = NotFormatMoney($("#inputChange").val());
        policyData.Exchange = {
            Currency: {
                Id: $('#selectCurrency').UifSelect('getSelected'),
            }
        }
        policyData.BranchName = $("#selectBranch").UifSelect('getSelectedText');
        policyData.PrefixName = $("#selectPrefixCommercial").UifSelect('getSelectedText');
        policyData.ProductName = $("#selectProduct").UifSelect('getSelectedText');
        policyData.TimeHour = $("#selectHour").val();
        policyData.TimeMinutes = "00";

        if ($("#inputFrom").val() != null && $("#inputFrom").val() != undefined && $("#inputFrom").val() != '') {
            var aFecha1 = $("#inputFrom").val().toString().split('/');
            var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, parseInt(aFecha1[0]) + 1, ($("#selectHour").val() - 19));
            policyData.CurrentFrom = new Date(fFecha1)
        }

        if ($("#inputTo").val() != null && $("#inputTo").val() != undefined && $("#inputTo").val() != '') {
            var aFecha2 = $("#inputTo").val().toString().split('/');
            var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, parseInt(aFecha2[0]) + 1, ($("#selectHour").val() - 19));;
            policyData.CurrentTo = new Date(fFecha2);
        }

        if ($("#inputBillingGroup").data("Object") != null) {
            policyData.BillingGroup = $("#inputBillingGroup").data("Object").Id;
        }

        if ($("#inputRequest").data("Object") != null) {
            policyData.Request = $("#inputRequest").data("Object").Request.Id;
        }
        if (glbPolicy.Summary == null) { glbPolicy.Summary = []; glbPolicy.Summary.RiskCount = null }
        policyData.RisksQuantity = glbPolicy.Summary !== null ? glbPolicy.Summary.RiskCount : 0;
        policyData.JustificationSarlaft = $("#selectJustification").val();
        if (policyData.TemporalType == 1 || policyData.TemporalType == 4) {
            policyData.TicketDate = "1900/01/01";
            policyData.TicketNumber = "0";
        } else {
            policyData.TicketDate = $('#inputDateEstablishment').val();
            policyData.TicketNumber = $('#inputRegistrationNumber').val();
        }
        policyData.CalculateMinPremium = $("#chkCalculateMinimumPremium_").prop('checked');
        var policyType = $("#selectPolicyType").UifSelect("getSelectedSource");

        if (policyType != undefined) {
            policyData.IsFloating = policyType.IsFloating;
        }

        if (gblProspectData != null && gblProspectData.IdCarNo != null) {
            policyData.Holder = gblProspectData.IdCarNo;
        }
        if (policyData.politieshold) {
            policyData.polities = false;
        }
        else if (glbPolicy.InfringementPolicies != null && redirect) {
            policyData.InfringementPolicies = glbPolicy.InfringementPolicies;
            policyData.polities = true;
        } else {
            policyData.polities = false;
        }

        return policyData;
    }

    static SaveTemporal(showAlert) {
        $("#formUnderwriting").validate();
        if ($("#formUnderwriting").valid() && UnderwritingTemporal.validateProspect()) {
            var events = null;
            var policyData = Underwriting.GetPolicyModel(false);
            lockScreen();
            UnderwritingQuotation.EnabledButtonsSaveQuotetion(glbPolicy.Endorsement.QuotationId);
            UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities).done(function (data) {
                if (data.success) {
                    Underwriting.UpdateGlbPolicy(data.result);
                    Underwriting.LoadTitle(glbPolicy);
                    Underwriting.UpdateRisks();
                    Underwriting.LoadSubTitles(0);
                    $("#selectPrefixCommercial").prop('disabled', 'disabled');
                    $("#selectProduct").prop('disabled', 'disabled');

                    let events = TypeAuthorizationPolicies.Nothing;
                    if (glbPolicy.InfringementPolicies != null) {
                        /// se lanzan los eventos de la pantalla principal
                        events = LaunchPolicies.ValidateInfringementPolicies(glbPolicy.InfringementPolicies, true);
                    }
                    /// FIN -- se lanzan los eventos de la pantalla principal
                    if (showAlert && events !== TypeAuthorizationPolicies.Restrictive) {
                        if (glbPolicy.TemporalType == TemporalType.Quotation) {
                            $.UifDialog('confirm', { 'message': AppResources.LabelQuotation + ' ' + glbPolicy.Endorsement.QuotationId + ' ' + AppResources.LabelVersion + ' ' + glbPolicy.Endorsement.QuotationVersion + ' ' + AppResources.MessageSavedSuccessfully + '\n' + AppResources.MessagePrint }, function (result) {
                                if (result) {

                                    if (glbPolicy.TemporalType == TemporalType.Quotation) {
                                        PrinterRequest.PrintQuotationOutside(
                                            glbPolicy.Endorsement.TemporalId,
                                            glbPolicy.Prefix.Id,
                                            glbPolicy.Endorsement.QuotationId,
                                            glbPolicy.Endorsement.QuotationVersion
                                        ).done(function (data) {
                                            if (data.success) {
                                                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);

                                            }
                                            else {
                                                if (data.result == Resources.Language.EndorsmentNotReinsured) {
                                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotPrinter + ": " + data.result, 'autoclose': true });
                                                }
                                                else {
                                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                                }
                                            }
                                        }).fail(function (data) {
                                            UnderwritingQuotation.EnabledButtonsSaveQuotetion(0);
                                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                        });
                                    }
                                }
                                UnderwritingQuotation.ClearControls();
                            });
                        }

                        else if (glbPolicy.TemporalType == TemporalType.TempQuotation) {
                            UnderwritingQuotation.EnabledTempButtonsTemp(glbPolicy.Id);
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelTemporary + ' ' + glbPolicy.Id + ' ' + AppResources.MessageSavedSuccessfully, 'autoclose': true });
                        }
                        else {
                            $.UifProgress('close');
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelTemporary + ' ' + glbPolicy.Id + ' ' + AppResources.MessageSavedSuccessfully, 'autoclose': true });
                        }
                    } else {
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                $.UifProgress('close');
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
            });
        }
    }

    static SaveTemporalPartial(Menu) {
        $("#formUnderwriting").validate();
        if ($("#formUnderwriting").valid()) {
            var policyData = Underwriting.GetPolicyModel(false);
            UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities).done(function (data) {
                if (data.success) {
                    Underwriting.UpdateGlbPolicy(data.result);
                    Underwriting.LoadTitle(glbPolicy);
                    Underwriting.UpdateRisks();
                    Underwriting.LoadPartial(Menu);
                    Underwriting.LoadSubTitles(0);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
            });

        }
    }

    static LoadPartial(Menu) {
        switch (Menu) {
            case MenuType.CoInsurance:
                UnderwritingCoInsurance.LoadPartialCoinsurance();
                break;
            case MenuType.Agents:
                UnderwritingAgent.LoadPartialAgents();
                break;
            case MenuType.PaymentPlan:
                UnderwritingPaymentPlan.LoadPaymentPlan();
                break;
            case MenuType.Texts:
                UnderwritingText.LoadPartialText();
                break;
            case MenuType.Clauses:
                UnderwritingClauses.LoadPartialClauses();
                break;
            case MenuType.Beneficiaries:
                UnderwritingBeneficiaries.LoadBeneficiaries();
                break;
            case MenuType.Collective:
                Underwriting.RedirectCollective('Collective/Collective');
                break;
            case MenuType.AdditionalData:
                UnderwritingAdditionalData.LoadPartialAdditionalData();
                break;
            default:
                break;
        }
    }

    static RedirectCollective(controller) {
        var collectiveModel = $("#formUnderwriting").serializeObject();
        collectiveModel.TempId = glbPolicy.Id;
        collectiveModel.Id = glbPolicy.Id;
        if (glbPolicy.Product != undefined) {
            if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                if (glbPolicy.BusinessType == BusinessType.Direct) {
                    $.redirect(rootPath + 'Collective/' + controller, collectiveModel);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateBusinessTypeCollective, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageVadilateTemporaryCollective, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectPolicy, 'autoclose': true });
        }
    }

    static LoadTitle(policyData) {
        if (policyData.Id != 0) {
            var titlePrincipal = '';
            if ($.trim(policyData.TemporalTypeDescription) == '' || $.trim(policyData.Endorsement.EndorsementTypeDescription) == '') {
                if (glbPolicy.Id != 0) {
                    UnderwritingRequest.GetTitle(glbPolicy.Id).done(function (data) {
                        if (data.success) {
                            policyData = data.result;
                        }
                        else {
                            //$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        //$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorUpdateRecord, 'autoclose': true });
                    });
                }
            }

            if (policyData.TemporalType == TemporalType.Quotation || policyData.TemporalType == TemporalType.TempQuotation) {
                titlePrincipal = policyData.TemporalTypeDescription + ': ' + policyData.Id + ' ';
            }
            else {
                titlePrincipal = policyData.TemporalTypeDescription + ': ' + policyData.Id + ' ' + policyData.Endorsement.EndorsementTypeDescription;
            }

            $.uif2.helpers.setGlobalTitle(titlePrincipal);
            glbPolicy = policyData;
            glbPolicy.Title = titlePrincipal;
        }
    }

    static LoadAgencyPrincipal(agencies) {
        if (agencies != null) {
            for (let i = 0; i < agencies.length; i++) {
                if (agencies[i].Agent.FullName == null) {
                    agencies[i].Agent.FullName = agencies[i].FullName;
                }
                if (agencies[i].FullName == null) {
                    agencies[i].FullName = agencies[i].Agent.FullName;
                }
                if (agencies[i].IsPrincipal == true) {
                    if (agentSearchType == 1) {
                        $("#inputAgentPrincipal").data("Object", agencies[i]);
                        $("#inputAgentPrincipal").val(agencies[i].Agent.FullName);
                        Underwriting.GetAgenciesByAgentId(agencies[i].Agent.IndividualId, agencies[i].Id);
                    }
                }
                if (agencies[i].Agent.FullName == null) {
                    agencies[i].Agent.FullName = agencies[i].FullName;
                }
            }
        }
    }

    static ClearControls() {
        $.uif2.helpers.setGlobalTitle("");
        $("#inputHolder").data("Object", null);
        $("#inputHolder").val('');
        $("#inputAgentPrincipal").data("Object", null);
        $("#inputAgentPrincipal").val('');
        $("#inputBillingGroup").data("Object", null);
        $("#inputBillingGroup").val('');
        $("#inputRequest").data("Object", null);
        $("#inputRequest").val('');
        $("#selectAgentAgency").UifSelect();
        $("#selectBranch").UifSelect("setSelected", null);
        $("#selectSalesPoint").UifSelect();
        $("#selectPrefixCommercial").UifSelect();
        $("#selectPolicyType").UifSelect();
        $("#selectProduct").UifSelect();
        $("#selectCurrency").UifSelect();
        Underwriting.LoadDates();
        Underwriting.CalculateDays();
        $("#inputChange").val('');
        $("#inputChange").prop('disabled', false);
        $('#selectedCoInsurance').text("");
        $('#selectedAgents').text("");
        $('#selectedPaymentPlan').text("");
        $('#selectedInclusionRisk').text("");
        $('#selectedTexts').text("");
        $('#selectedClauses').text("");
        $('#selectedBeneficiaries').text("");
        $("#hiddenEndorsementType").val();

        $("#inputBillingGroup").UifInputSearch('disabled', false);
        $("#inputRequest").UifInputSearch('disabled', false);
        $("#inputHolder").UifInputSearch('disabled', false);
        $("#inputAgentPrincipal").UifInputSearch('disabled', false);
        $("#selectBranch").prop('disabled', false);
        $("#inputFrom").UifDatepicker('disabled', false);
        $("#inputTo").UifDatepicker('disabled', false);
        $("#inputChange").prop('disabled', false);
        $("#btnAgentsSave").prop('disabled', false);
        $("#btnBeneficiarySave").prop('disabled', false);
        $("#btnCoInsuranceSave").prop('disabled', false);
        $("#btnPaymentPlanSave").prop('disabled', false);
        $('#labelRisk').text("0");
        $('#labelSum').text("0");
        $('#labelPremium').text("0");
        $('#labelExpenses').text("0");
        $('#labelTaxes').text("0");
        $('#labelTotalPremium').text("0");

        Underwriting.GetDefaultAgent();
        glbRisk = null;
        glbCoverage = null;

        if (gblProspectData != null) {
            gblProspectData.IdCarNo = null;
        }
        $("#inputRegistrationNumber").val("");
        $("#inputDateEstablishment").val("");
        $("#selectJustification").UifSelect("setSelected", null);
        $("#btnConvertProspect").hide();
    }

    static LoadDates() {
        $("#inputFrom").UifDatepicker('setValue', GetCurrentDateFrom());
        //$("#inputTo").UifDatepicker('setValue', AddToDate(GetCurrentDateFrom(), 0, 0, 1));

        $("#inputFrom").data("dateFrom", $("#inputFrom").val());
        Underwriting.CalculateDays();
    }

    static GetParameterByDescription(description) {
        var dataResult = '';
        UnderwritingRequest.GetParameterByDescription(description).done(function (data) {
            if (data.success) {
                dataResult = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchParameter, 'autoclose': true });
        });
        return dataResult;
    }

    static GetAgenciesByAgentIdDescription(agentId, description) {
        var number = parseInt(description, 10);


        /*Valida si la catalogación de la poliza ya se realizó*/

        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            UnderwritingRequest.GetUserAgenciesByAgentIdDescription(agentId, description).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.length == 1) {
                            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
                                if (agentSearchType == 1) {
                                    $("#inputAgentPrincipal").data("Object", data.result[0]);
                                    $("#inputAgentPrincipal").val(data.result[0].FullName);
                                    Underwriting.GetPrefixesByAgentId(data.result[0].Agent.IndividualId, 0);
                                }
                                else if (agentSearchType == 2) {
                                    $("#inputAgentsAgency").data("Object", data.result[0]);
                                    $("#inputAgentsAgency").val(data.result[0].FullName);
                                    CommonAgent.GetPrefixesByAgentIdAgents(data.result[0].Agent.IndividualId, 0);
                                }
                                Underwriting.GetAgenciesByAgentId(data.result[0].Agent.IndividualId, data.result[0].Id);
                            }
                            else {

                                Underwriting.ExistProductAgentByAgency(data.result[0]);
                            }
                        }
                        else if (data.result.length > 1) {
                            modalListType = 1;
                            var dataList = [];

                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].Agent.IndividualId,
                                    Code: data.result[i].Code,
                                    Description: data.result[i].Agent.FullName
                                });
                            }
                            Underwriting.ShowDefaultResults(dataList);
                            $('#modalDefaultSearch').UifModal('showLocal', AppResources.LabelAgentPrincipal);
                        }
                        else {
                            if (agentSearchType == 1) {
                                $("#inputAgentPrincipal").data("Object", null);
                                $("#inputAgentPrincipal").val('');
                            }
                            else if (agentSearchType == 2) {
                                $("#inputAgentsAgency").data("Object", null);
                                $("#inputAgentsAgency").val('');
                            }
                            $('#selectAgentAgency').UifSelect({ source: null });
                            showInfoToast(AppResources.MessageSearchAgents);
                        }
                    }
                    else {
                        $('#selectAgentAgency').UifSelect({ source: null });
                        showInfoToast(AppResources.MessageSearchAgents);
                    }
                }
                else {
                    if (agentSearchType == 1) {
                        $("#inputAgentPrincipal").data("Object", null);
                        $("#inputAgentPrincipal").val('');
                    }
                    else if (agentSearchType == 2) {
                        $("#inputAgentsAgency").data("Object", null);
                        $("#inputAgentsAgency").val('');
                    }
                    $('#selectAgentAgency').UifSelect({ source: null });
                    showInfoToast(data.result)
                    $("#selectAgentAgency").prop('disabled', true);
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                $('#selectAgentAgency').UifSelect({ source: null });
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchAgents, 'autoclose': true });
            });
        }
    }

    static ExistProductAgentByAgency(agency) {
        var result = false;
        UnderwritingRequest.ExistProductAgentByAgentIdPrefixIdProductId(agency.Agent.IndividualId, $("#selectPrefixCommercial").val(), $("#selectProduct").val()).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (agentSearchType == 1) {
                        $("#inputAgentPrincipal").data("Object", agency);
                        $("#inputAgentPrincipal").val(agency.Agent.FullName);
                    }
                    else if (agentSearchType == 2) {
                        $("#inputAgentsAgency").data("Object", agency);
                        $("#inputAgentsAgency").val(agency.FullName);
                    }
                    Underwriting.GetAgenciesByAgentId(agency.Agent.IndividualId, agency.Id);
                }
                else if (agentSearchType == 1) {
                    $("#inputAgentPrincipal").val(agency.FullName);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageProductAgent, 'autoclose': true });
                }
            }
            else {

                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchProductAgent, 'autoclose': true });
        });
    }

    static GetAgenciesByAgentId(agentId, selectedId) {
        UnderwritingRequest.GetUserAgenciesByAgentId(agentId).done(function (data) {
            if (data.success) {
                if (agentSearchType == 1) {
                    $("#selectAgentAgency").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $("#selectAgentAgency").prop('disabled', true);
                }
                else if (agentSearchType == 2) {
                    $("#selectAgentsAgency").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetPrefixesByAgentId(agentId, selectedId) {
        UnderwritingRequest.GetPrefixesByAgentId(agentId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectPrefixCommercial").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectPrefixCommercial").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetSalePointsByBranchId(branchId, selectedId) {
        UnderwritingRequest.GetSalePointsByBranchId(branchId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    if (selectedId == 0) {
                        $("#selectSalesPoint").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectSalesPoint").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        CommonAgent.GetSalePointsByBranchId(branchId, 0);
                    }
                    ////Show first and selected default
                    //var defaults = data.result.filter(obj => obj.IsDefault == true);
                    //if (defaults.length == 1) {
                    //    //$("#selectSalesPoint").UifSelect("setSelected", defaults[0].Id);
                    //}
                }
                else {
                    $("#selectSalesPoint").UifSelect();
                    $.UifNotify('show', { 'type': 'info', 'message': 'No se hallan puntos de venta asociados', 'autoclose': true });
                }
            }
        });
    }

    static GetProductsByAgentIdPrefixId(agentId, prefixId, selectedId) {
        var isCollective = Underwriting.getQueryVariable("isCollective");
        if (isCollective != "true") {
            isCollective = false;
        }

        UnderwritingRequest.GetProductsByAgentIdPrefixId(agentId, prefixId, isCollective).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectProduct").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectProduct").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });


    }

    static GetPolicyTypesByProductId(productId, selectedId) {
        UnderwritingRequest.GetPolicyTypesByProductId(productId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    if (data.result != null) {
                        for (let i = 0; i < data.result.length; i++) {
                            if (data.result[i].IsDefault == true) {
                                selectedId = data.result.Id;
                                break;
                            }
                        }
                    }

                    if (selectedId == 0) {
                        $("#selectPolicyType").UifSelect({ sourceData: data.result });
                        if (data.result.length == 1) {
                            $("#selectPolicyType").UifSelect("setSelected", data.result[0].Id);
                        }
                    }
                    else {
                        $("#selectPolicyType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                        if (data.result.length == 1) {
                            $("#selectPolicyType").UifSelect("setSelected", data.result[0].Id);
                        }
                    }
                }
                else {
                    $("#selectPolicyType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    if (data.result.length == 1) {
                        $("#selectPolicyType").UifSelect("setSelected", data.result[0].Id);
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static CalculateDays() {

        var fFecha1 = 0;
        var fFecha2 = 0;

        if ($("#inputFrom").val() != null && $("#inputFrom").val() != undefined && $("#inputFrom").val() != '') {
            var aFecha1 = $("#inputFrom").val().toString().split('/');
            fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
        }

        if ($("#inputTo").val() != null && $("#inputTo").val() != undefined && $("#inputTo").val() != '') {
            var aFecha2 = $("#inputTo").val().toString().split('/');
            fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
        }

        var dif = fFecha2 - fFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (isNaN(dias)) {
            $("#inputDays").val(0);
        }
        else {
            $("#inputDays").val(dias);
        }
    }

    static GetCurrenciesByProductId(productId, selectedId) {
        UnderwritingRequest.GetCurrenciesByProductId(productId).done(function (data) {
            if (data.success) {
                if (selectedId === "") {
                    $("#selectCurrency").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectCurrency").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    //Se agrega validacion de 0= pesos, solo se debe consultar la tasa si es diferente
                    //if (selectedId != 0) {
                    Underwriting.GetExchangeRateByCurrencyId(selectedId);
                    //}
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetJustificationSarlaft(JustificationSarlaft) {
        UnderwritingRequest.GetJustificationSarlaft().done(function (data) {
            if (data.success) {
                if (JustificationSarlaft == null) {
                    $("#selectJustification").UifSelect({ sourceData: data.result });
                } else {

                    $("#selectJustification").UifSelect({ sourceData: data.result, selectedId: JustificationSarlaft.JustificationReasonCode });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetExchangeRateByCurrencyId(currencyId) {
        $("#inputChange").prop('disabled', true);
        if (CurrencyType.COP == currencyId) {
            $("#chkCurrencyIssue").prop('disabled', true);
            $("#chkCurrencyLocal").prop('disabled', true);
            $("#chkCurrencyLocal").prop("checked", true);
        } else {
            $("#chkCurrencyIssue").prop('disabled', false);
            $("#chkCurrencyLocal").prop('disabled', false);
            $("#chkCurrencyIssue").prop("checked", true);
        }
        if (glbPolicy.Summary.RiskCount > 0) {
            $('#selectCurrency').prop('disabled', true);
            $('#inputChange').prop('disabled', true);
        } else {
            $('#selectCurrency').prop('disabled', false);
        }

        UnderwritingRequest.GetExchangeRateByCurrencyId(currencyId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $("#inputChange").val(FormatMoney(data.result.SellAmount));
                    if (FormatDate(data.result.RateDate) != FormatDate(IssueDate) && data.result.Currency.Id != 0) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.updateexchangerate, 'autoclose': true });
                    }
                    else {
                        $("#inputChange").prop('disabled', true);
                    }
                    currencyObject = {
                        exchangeRate: data.result.SellAmount,
                        currencyType: data.result.Id,
                        local: currencyId
                        //currencyType: data.result.Currency.Id
                    }
                    Underwriting.setInputCurrency(currencyId);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.OutdatedExchangeRateExist, 'autoclose': true });
        });
    }

    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        UnderwritingRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                if (individualSearchType == 2) {
                    $("#inputBeneficiaryName").data('Detail', Underwriting.SetIndividualDetails(data.result));
                }
                else {
                    $("#inputHolder").data('Detail', Underwriting.SetIndividualDetails(data.result));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        });
    }

    static ShowModalHolderDetail() {
        var holderDetails = {};
        $('#tableIndividualDetails').UifDataTable('clear');

        if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CustomerType == CustomerType.Individual) {
            detailType = 1;

            if ($("#inputHolder").data('Detail') != null) {
                holderDetails = $("#inputHolder").data('Detail');
                $.each(holderDetails, function (index, value) {
                    $('#tableIndividualDetails').UifDataTable('addRow', value);
                });
                holderDetails = $("#tableIndividualDetails").UifDataTable("getData");
                if (holderDetails.length > 0) {
                    for (let i = 0; i < holderDetails.length; i++) {
                        if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.NameNum > 0 && $("#inputHolder").data("Object").CompanyName.NameNum == holderDetails[i].NameNum) {
                            $('#tableIndividualDetails tbody tr:eq(' + i + ')').removeClass('row-selected').addClass('row-selected');
                            $('#tableIndividualDetails tbody tr:eq(' + i + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                        else if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.Address.Id == holderDetails[i].Address.Id && holderDetails[i].IsMain == true) {
                            $('#tableIndividualDetails tbody tr:eq(' + i + ')').removeClass('row-selected').addClass('row-selected');
                            $('#tableIndividualDetails tbody tr:eq(' + i + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                        else if ($("#inputHolder").data("Object") != null && $("#inputHolder").data("Object").CompanyName.Address.Id == holderDetails[i].Address.Id) {
                            $('#tableIndividualDetails tbody tr:eq(' + i + ')').removeClass('row-selected').addClass('row-selected');
                            $('#tableIndividualDetails tbody tr:eq(' + i + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                        }
                    }


                    if ($('#tableIndividualDetails').UifDataTable('getSelected') == null) {
                        $('#tableIndividualDetails tbody tr:eq(0)').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(0) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                }
            }

            $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelHolderDetail);
        }
    }

    static ShowIndividualResults(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    //Ocultar paneles
    static HidePanelsIssuance(Menu) {
        switch (Menu) {
            case MenuType.Underwriting:
                break;
            case MenuType.CoInsurance:
                $("#modalCoInsurance").UifModal('hide');
                break;
            case MenuType.Agents:
                $("#modalAgents").UifModal('hide');
                break;
            case MenuType.PaymentPlan:
                $("#modalPaymentPlan").UifModal('hide');
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('hide');
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('hide');
                break;
            case MenuType.Beneficiaries:
                $("#modalBeneficiaries").UifModal('hide');
                break;
            case MenuType.AdditionalData:
                $("#modalAdditionalData").UifModal('hide');
                break;
            default:
                break;
        }
    }

    //Mostrar panel
    static ShowPanelsIssuance(Menu) {
        if (glbPolicy.Summary.RiskCount > 0) {
            switch (Menu) {
                case MenuType.Underwriting:
                    break;
                case MenuType.CoInsurance:
                    $("#modalCoInsurance").UifModal('showLocal', AppResources.LabelDataBusinessType);
                    break;
                case MenuType.Agents:
                    $("#modalAgents").UifModal('showLocal', AppResources.LabelDataAgents);
                    break;
                case MenuType.PaymentPlan:
                    $("#modalPaymentPlan").UifModal('showLocal', AppResources.LabelPaymentPlan);
                    break;
                case MenuType.Texts:
                    $("#modalTexts").UifModal('showLocal', AppResources.LabelTexts);
                    break;
                case MenuType.Clauses:
                    $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                    break;
                case MenuType.Beneficiaries:
                    $("#modalBeneficiaries").UifModal('showLocal', AppResources.LabelBeneficiaries);
                    break;
                case MenuType.AdditionalData:
                    $("#modalAdditionalData").UifModal('showLocal', AppResources.LabelAdditionalData);
                    break;
                case MenuType.TablesExpreses:
                    $("#modalTablesExpreses").UifModal('showLocal', 'Datos Gastos');
                    break;
                case MenuType.Surcharges:
                    $("#modalSurcharges").UifModal('showLocal', 'Datos Recargos');
                    break;
                case MenuType.Discounts:
                    $("#modalDiscounts").UifModal("showLocal", "Datos Descuentos");
                    break;
                case MenuType.Taxes:
                    $("#modalTaxes").UifModal('showLocal', 'Datos Impuestos');
                    break;
                default:
                    break;
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.RiskPolicy, 'autoclose': true });
        }
    }

    static RedirectToRisk() {
        var resultOperation = false;
        $("#formUnderwriting").validate();
        Underwriting.ValidateExchangeRateUnder().then(function (data) {
            var validateExchange = data;
            if ($("#formUnderwriting").valid() && validateExchange) {
                lockScreen();
                var policyData = Underwriting.GetPolicyModel(true);
                var polities = policyData.polities;
                UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities).done(function (data) {
                    if (data.success) {
                        resultOperation = true;
                        Underwriting.UpdateGlbPolicy(data.result, polities);
                        Underwriting.LoadTitle(glbPolicy);
                        Underwriting.LoadSubTitles(0);
                        //Caucion Judicial
                        if ($('#selectPrefixCommercial').val() == 31) {
                            if (glbPolicy.RiskTemporalOldId == undefined && glbPolicy.RiskTemporalOldId == null) {
                                glbPolicy.RiskTemporalOldId = gblCurrentRiskTemporalNumber;
                                if (gblCurrentRiskTemporalNumberOld == null) {
                                    gblCurrentRiskTemporalNumberOld = gblCurrentRiskTemporalNumber;
                                }
                            }
                        }

                        if (glbPolicy.TemporalType != TemporalType.Quotation) {
                            if (glbPolicy.Holder.InsuredId == 0) {
                                if (gblProspectData !== null) {
                                    if (gblProspectData.IdCardNo == null) {
                                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyholderWithoutRol, 'autoclose': true });
                                        resultOperation = false;
                                    }
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': 'Informacion del prospecto no obtenida', 'autoclose': true });
                                    resultOperation = false;
                                }
                            }
                            else if (!Isnull(FormatDate(glbPolicy.Holder.DeclinedDate))) {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyholderDisabled, 'autoclose': true });
                                resultOperation = false;
                            }
                        }
                        if (resultOperation !== false) {
                            let events = TypeAuthorizationPolicies.Nothing;
                            if (glbPolicy.InfringementPolicies != null) {
                                /// se lanzan los eventos de la pantalla principal
                                events = LaunchPolicies.ValidateInfringementPolicies(glbPolicy.InfringementPolicies, true);
                            }

                            if (resultOperation === true && events !== TypeAuthorizationPolicies.Restrictive) {
                                if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemporaryCollective, 'autoclose': true });
                                }
                                else {
                                    Underwriting.LoadSiteRisk();
                                }
                            }
                        }
                    }
                    else {
                        resultOperation = false;
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    resultOperation = false;
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
                });
            }
        });
    }

    static ValidateAgency(agencyId) {
        var agentId = 0;
        if (agentSearchType == 1) {
            agentId = $("#inputAgentPrincipal").data("Object").Agent.IndividualId;
        }
        else if (agentSearchType == 2) {
            agentId = $("#inputAgentsAgent").data("Object").Agent.IndividualId;
        }
        UnderwritingRequest.GetAgencyByAgentIdAgencyId(agentId, agencyId).done(function (data) {
            if (data.success) {
                $("#inputAgentPrincipal").data("Object").Code = data.result.Code;
                $("#inputAgentPrincipal").data("Object").FullName = data.result.FullName;
                $("#inputAgentPrincipal").data("Object").Branch = data.result.Branch;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                $('#selectAgentAgency').val(null);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
        });
    }

    static LoadCalculateMinimumPremium(Calculation) {
        if (Calculation != null) {
            if (Calculation) {
                $("#chkCalculateMinimumPremium_").prop('checked', true);
            } else {
                $("#chkCalculateMinimumPremium_").prop('checked', false);
            }
        }
    }

    static LoadTicket(ticket) {
        if (ticket != null) {
            $('#inputDateEstablishment').val(FormatDate(ticket.TicketDate) == "01/01/1900" ? "" : FormatDate(ticket.TicketDate));
            $('#inputRegistrationNumber').val(ticket.TicketNumber == "0" ? "" : ticket.TicketNumber);
        }
    }

    static LoadSarlaft(sarlaft) {
        if (sarlaft != null) {
            Underwriting.GetJustificationSarlaft(sarlaft.JustificationReasonCode);
        }
    }

    static LoadSummary(summary) {
        if (summary != null) {
            $('#labelRisk').text(summary.RiskCount);
            $('#labelSum').text(FormatMoney(summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(summary.Premium));
            $('#labelExpenses').text(FormatMoney(summary.Expenses));
            $('#labelTaxes').text(FormatMoney(summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(summary.FullPremium));

            if (summary.RiskCount > 0 && summary.RiskCount != null && summary.RiskCount != undefined ) {
                $("#selectPrefixCommercial").UifSelect('disabled', true);
                $("#selectProduct").UifSelect('disabled', true);
                $("#selectPolicyType").prop('disabled', true);
            }
        }
        else {
            $("#selectPrefixCommercial").UifSelect('disabled', false);
            $("#selectProduct").UifSelect('disabled', false);
            $("#selectPolicyType").prop('disabled', false);
        }
    }

    static ExecuteRulesPre(showAlert) {
        var resultOperation = false;
        var policyData = Underwriting.GetPolicyModel(false);
        UnderwritingRequest.RunRulesPolicyPre(policyData, dynamicProperties).done(function (data) {
            if (data.success) {
                resultOperation = true;
                glbPolicy = data.result;
                Underwriting.LoadTemporalRules(glbPolicy);
            }
            else {
                resultOperation = false;
                Underwriting.GetProductsByAgentIdPrefixId($("#inputAgentPrincipal").data("Object").Agent.IndividualId, $("#selectPrefixCommercial").val(), 0);
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultOperation = false;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
        });

        return resultOperation;
    }

    static LoadTemporalRules(policyData) {
        if (policyData.Holder.IdentificationDocument != null) {
            $("#inputHolder").data("Object", policyData.Holder);
            $("#inputHolder").val(policyData.Holder.Name + ' (' + policyData.Holder.IdentificationDocument.Number + ')');
            Underwriting.GetIndividualDetailsByIndividualId(policyData.Holder.IndividualId, policyData.Holder.CustomerType)
        }

        agentSearchType = 1;
        Underwriting.LoadAgencyPrincipal(policyData.Agencies);
        if (policyData.Branch.SalePoints != null && policyData.Branch.SalePoints.length > 0) {
            $("#selectBranch").val(policyData.Branch.Id);
            Underwriting.GetSalePointsByBranchId(policyData.Branch.Id, policyData.Branch.SalePoints[0].Id);
            CommonAgent.GetSalePointsByBranchId(policyData.Branch.Id, 0);
        }
        Underwriting.GetPolicyTypesByProductId(policyData.Product.Id, policyData.PolicyType.Id);
        Underwriting.GetCurrenciesByProductId(policyData.Product.Id, policyData.ExchangeRate.Currency.Id);

        $("#inputFrom").UifDatepicker('setValue', FormatDate(policyData.CurrentFrom));
        $("#inputTo").UifDatepicker('setValue', FormatDate(policyData.CurrentTo));
        Underwriting.CalculateDays();
        $("#selectHour").val(policyData.TimeHour);
        if (policyData.ExchangeRate.SellAmount > 0) {
            $("#inputChange").val(FormatMoney(policyData.ExchangeRate.SellAmount));
            if (policyData.ExchangeRate.SellAmount == 1) {
                $("#inputChange").prop('disabled', true);
            }
            else {
                $("#inputChange").prop('disabled', false);
            }
        }
        dynamicProperties = policyData.DynamicProperties;
        glbPolicy = policyData;
    }

    static LoadSubTitles(subTitle) {
        if (subTitle == 0 || subTitle == 1) {
            if (glbPolicy.BusinessType > 0) {
                lockScreen();
                CoInsuranceRequest.GetBusinessTypes().done(function (data) {
                    if (data.success) {
                        if (data.result[glbPolicy.BusinessType - 1] != null) {
                            $('#selectedCoInsurance').text(data.result[glbPolicy.BusinessType - 1].Text);
                        }
                    }
                });
            }
        }

        if (subTitle == 0 || subTitle == 2) {
            if (glbPolicy.Agencies != null) {
                for (let i = 0; i < glbPolicy.Agencies.length; i++) {
                    if (glbPolicy.Agencies[i].IsPrincipal == true) {
                        if (glbPolicy.Agencies[i].Participation == 0) {
                            glbPolicy.Agencies[i].Participation = 100;
                        }
                        if (glbPolicy.Agencies[i].Commissions != null && glbPolicy.Agencies[i].Commissions.length > 0) {
                            $('#selectedAgents').text(AppResources.LabelParticipants + ": (" + glbPolicy.Agencies.length + ")" + AppResources.LabelCommission + ": " + glbPolicy.Agencies[i].Commissions[0].Percentage + "%");
                        }
                        else {
                            $('#selectedAgents').text(AppResources.LabelParticipants + ": (" + glbPolicy.Agencies.length + ")");
                        }
                    }
                }
            }
        }

        if (subTitle == 0 || subTitle == 3) {
            if (glbPolicy.PaymentPlan != null) {
                $('#selectedPaymentPlan').text(glbPolicy.PaymentPlan.Description);
            }
        }

        if (subTitle == 0 || subTitle == 4) {
            if (glbPolicy.Text != null) {
                if (glbPolicy.Text.TextBody == null) {
                    glbPolicy.Text.TextBody = "";
                }
                if (glbPolicy.Text.Observations == null) {
                    glbPolicy.Text.Observations = "";
                }

                if (glbPolicy.Text.TextBody.length > 0 && glbPolicy.Text.Observations.length > 0) {
                    $('#selectedTexts').text("(" + AppResources.LabelWithTextAndObservations + ")");
                }
                else if (glbPolicy.Text.TextBody.length > 0) {
                    $('#selectedTexts').text("(" + AppResources.LabelWithTextWithoutObservations + ")");
                }
                else if (glbPolicy.Text.Observations.length > 0) {
                    $('#selectedTexts').text("(" + AppResources.LabelWithoutTextWithobservations + ")");
                }
                else {
                    $('#selectedTexts').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedTexts').text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 5) {
            if (glbPolicy.Clauses != null) {
                if (glbPolicy.Clauses.length > 0) {
                    $('#selectedClauses').text("(" + glbPolicy.Clauses.length + ")");
                }
                else {
                    $('#selectedClauses').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedClauses').text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 6) {
            if (glbPolicy.Summary != null) {
                if (glbPolicy.Summary.RiskCount > 0) {
                    $("#selectedInclusionRisk").text("(" + glbPolicy.Summary.RiskCount + ")");
                }
                else {
                    $("#selectedInclusionRisk").text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $("#selectedInclusionRisk").text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 7) {
            if (glbPolicy.DefaultBeneficiaries != null) {
                if (glbPolicy.DefaultBeneficiaries.length > 0) {
                    $("#selectedBeneficiaries").text("(" + AppResources.LabelWithBeneficiaries + ")");
                }
                else {
                    $("#selectedBeneficiaries").text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $("#selectedBeneficiaries").text("(" + AppResources.LabelWithoutData + ")");
            }
        }
    }

    static DisabledControlByEndorsementType(endorsementType, prefixId, disabled) {
        switch (endorsementType) {
            case EndorsementType.Renewal: {
                $("#inputHolder").prop('disabled', disabled);
                $('#inputFrom').UifDatepicker('disabled', disabled);
                $("#selectBranch").prop('disabled', disabled);
                if ($("#selectBranch").val() != null && $("#selectSalesPoint").val() != null)
                    $("#selectSalesPoint").prop('disabled', disabled);
                $("#selectCurrency").prop('disabled', disabled);
                $("#selectPrefixCommercial").prop('disabled', disabled);
                $("#selectProduct").prop('disabled', disabled);
                $("#selectPolicyType").prop('disabled', disabled);
                $("#btnNewTemporal").hide();
                $("#btnDeleteTemporal").hide();

                break;
            }
            case EndorsementType.Modification: {
                $("#inputBillingGroup").prop('disabled', disabled);
                $("#inputRequest").prop('disabled', disabled);
                $("#inputHolder").prop('disabled', disabled);
                $("#inputAgentPrincipal").prop('disabled', disabled);
                if (Prefix != null)
                    if (prefixId != PrefixCollective.Surety) {
                        $("#inputTo").UifDatepicker('disabled', disabled);
                    }
                break;
            }
            default:
                break;
        }
    }

    static UpdateRisks() {
        if (glbPolicy.Id > 0) {
            UnderwritingRequest.UpdateRisks(glbPolicy.Id, Underwriting.GetControllerRisk()).done(function (data) {
                if (data.success) {
                    if (glbPolicy.Summary.RisksInsured != null) {
                        var riskInsured = glbPolicy.Summary.RisksInsured;
                        glbPolicy = data.result;
                        glbPolicy.Summary.RisksInsured = riskInsured;
                        Underwriting.LoadTemporal(glbPolicy);
                    }
                    else {
                        glbPolicy = data.result;
                        Underwriting.LoadTemporal(glbPolicy);
                    }
                    
                }
                else {
                    Underwriting.ClearControls();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }

    static UpdateRisksInclusion() {
        if (glbPolicy.Id > 0) {
            UnderwritingRequest.UpdateRisks(glbPolicy.Id, Underwriting.GetControllerRisk()).done(function (data) {
                if (data.success) {
                    if (glbPolicy.Summary.RisksInsured != null) {
                        var riskInsured = glbPolicy.Summary.RisksInsured;
                        glbPolicy = data.result;
                        glbPolicy.Summary.RisksInsured = riskInsured;
                        Underwriting.LoadTemporalRisk(glbPolicy);
                    }
                    else {
                        glbPolicy = data.result;
                        Underwriting.LoadTemporalRisk(glbPolicy);
                    }

                }
                else {
                    Underwriting.ClearControls();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }

    static ExistRisk() {
        if (glbPolicy.Id > 0) {
            UnderwritingRequest.ExistRiskByTemporalId(glbPolicy.Id, Underwriting.GetControllerRisk()).done(function (data) {
                if (data.success) {
                    UnderwritingTemporal.CreatePolicy();
                }
                else {
                    if (data.result.startsWith("**")) {
                        //mostrar como cuadro de dialogo
                        $.UifDialog('alert',
                            { 'message': data.result },
                            function (result) {
                                //No hacer nada, sólo informativo
                            });
                        $('.modal-body.modal-body-dialog-alert p').prop('style', 'white-space: pre-line')

                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }

    static ValidateAvailableAmountByTemporalId() {
        if (glbPolicy.Id > 0) {
            UnderwritingRequest.ValidateAvailableAmountByTemporalId(glbPolicy.Id, Underwriting.GetControllerRisk()).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0) {
                        UnderwritingTemporal.CreatePolicy();
                    } else {
                        data.result.forEach(function (item, index) {
                            $.UifNotify('show', { 'type': 'danger', 'message': item, 'autoclose': true });
                        });
                    }
                }
            });
        }
    }

    //Obtiene grupo de facturación por Identificador o nombre
    static GetBillingGroupByDescription(description) {
        var number = parseInt(description, 10);
        if (!isNaN(number) || description.length > 2) {
            UnderwritingRequest.GetBillingGroupByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $("#inputBillingGroup").data("Object", data.result[0]);
                        $("#inputBillingGroup").val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                        $("#inputRequest").data("Object", null);
                        $("#inputRequest").val("");
                        $('#inputRequest').UifInputSearch('disabled', false);
                    }
                    else if (data.result.length > 1) {
                        $("#inputRequest").data("Object", null);
                        modalListType = 3;
                        var dataList = { dataObject: [] };
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.dataObject.push({
                                Id: data.result[i].Id,
                                Code: data.result[i].Id,
                                Description: data.result[i].Description
                            });
                        }
                        Underwriting.ShowDefaultResults(dataList.dataObject);
                        $('#modalDefaultSearch').UifModal('showLocal', AppResources.LabelSelectBillingGroup);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchBillingGroup });
                        $("#inputRequest").data("Object", null);
                        $("#inputRequest").val("");
                        $('#inputRequest').UifInputSearch('disabled', true);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                    $("#inputRequest").data("Object", null);
                    $("#inputRequest").val("");
                    $('#inputRequest').UifInputSearch('disabled', true);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchBillingGroup });
                $("#inputRequest").data("Object", null);
                $('#inputRequest').UifInputSearch('disabled', true);
            });
        }
    }

    //Obtiene una solicitud agrupadora por Identificador o nombre
    static GetCoRequestByDescription(description, loadRequest) {
        var billingGroupId = 0;
        if ($("#inputBillingGroup").data("Object") != null) {
            billingGroupId = $("#inputBillingGroup").data("Object").Id;
        }
        if (billingGroupId != 0) {
            var number = parseInt(description, 10);
            if (!isNaN(number) || description.length > 2) {
                UnderwritingRequest.GetCoRequestByRequestIdDescription(description, billingGroupId).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            $("#inputRequest").data("Object", data.result[0]);
                            $("#inputRequest").val(data.result[0].Request.Description + ' (' + data.result[0].Request.Id + ')');
                            if (loadRequest == true) {
                                Underwriting.ShowRequest(data.result[0]);
                            }
                        }
                        else if (data.result.length > 1) {
                            modalListType = 4;
                            var dataList = { dataObject: [] };
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.dataObject.push({
                                    Id: data.result[i].Request.Id,
                                    Code: data.result[i].Request.Id,
                                    Description: data.result[i].Request.Description
                                });
                            }
                            Underwriting.ShowDefaultResults(dataList.dataObject);
                            $('#modalDefaultSearch').UifModal('showLocal', AppResources.LabelSelectRequest);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchRequest });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchRequest });
                });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectBillingGroup });
            $('#inputRequest').UifInputSearch('disabled', true);
        }
    }


    //Deshabilita los campos de solicitud agrupadora
    static DisabledControlRequest(disabled) {
        $('#inputHolder').UifInputSearch('disabled', disabled);
        $("#inputAgentPrincipal").UifInputSearch('disabled', disabled);
        $("#selectAgentAgency").prop("disabled", disabled);
        $("#selectBranch").prop("disabled", disabled);
        $("#selectPrefixCommercial").prop("disabled", disabled);
        $("#selectProduct").prop("disabled", disabled);
        $("#selectPolicyType").prop("disabled", disabled);
        $("#btnAgentsSave").prop('disabled', disabled);
        $("#btnCoInsuranceSave").prop('disabled', disabled);
        $("#btnPaymentPlanSave").prop('disabled', disabled);
    }

    static clearValidation() {
        var validator = $("#formUnderwriting").validate();
        $('[name]', "#formUnderwriting").each(function () {
            validator.successList.push(this);//mark as error free
            validator.showErrors();//remove error messages if present
        });
        validator.resetForm();//remove error class on name elements and clear history
        validator.reset();//remove all error and success data
    }

    static UpdateGlbPolicy(data, polities = false) {
        var endorsementOriginId = glbPolicy.EndorsementOriginId;
        var InfringementPoliciesTemp = glbPolicy.InfringementPolicies;


        glbPolicy = data;
        if (polities) {
            glbPolicy.InfringementPolicies = InfringementPoliciesTemp;
        }
        glbPolicy.EndorsementOriginId = endorsementOriginId;
        glbPolicy.Title = glbPolicy.TemporalTypeDescription + ": " + glbPolicy.Id + " " + glbPolicy.Endorsement.EndorsementTypeDescription;
        glbPolicy.BeginDate = FormatDate(glbPolicy.BeginDate);
        glbPolicy.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
        glbPolicy.CurrentTo = FormatDate(glbPolicy.CurrentTo);
        glbPolicy.IssueDate = FormatFullDate(glbPolicy.IssueDate);
    }

    static validateBeneficiaryProspect(beneficiaries) {
        for (var i = 0; i < beneficiaries.length; i++) {
            if (beneficiaries[i].CustomerType == CustomerType.Prospect) {
                return true;
            }
        }
        return false;
    }

    static LoadHolder(holder) {
        if (gblProspectData == null) {
            gblProspectData = {};
        }

        glbPolicy.Holder = holder;
        gblProspectData.IdCardNo = holder.IdentificationDocument.Number;
        if (glbPolicy.Holder.CompanyName == null) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorInsuredWithoutAddress, 'autoclose': true });
            $("#inputHolder").val('');
            $("#inputHolder").data("Object", null);
            return;
        }
        let details = holder.details || [holder.CompanyName];

        if (individualSearchType == 2 || (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0
            && glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType == 2)) {
            $("#inputBeneficiaryName").data("Object", holder);
            $("#inputBeneficiaryName").val(holder.Name + ' (' + holder.IdentificationDocument.Number + ')');

            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured.length > 0 &&
                glbPolicy.Summary.RisksInsured[0].Insured != null &&
                glbPolicy.Summary.RisksInsured[0].Insured.CustomerType == 2) {

                glbPolicy.Summary.RisksInsured[0].Insured = holder;
            }

        }

        if (individualSearchType == 1) {
            $("#inputHolder").data("Object", holder);
            if (holder.IdentificationDocument != null) {
                $("#inputHolder").val(holder.Name + ' (' + holder.IdentificationDocument.Number + ')');
                if (glbPolicy.Id > 0 && (glbPolicy.TemporalType == TemporalType.TempQuotation || glbPolicy.TemporalType == TemporalType.Policy)) {

                    var policyData = Underwriting.GetPolicyModel(true);
                    UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities);
                }

            }
            if (glbPolicy.TemporalType == TemporalType.Policy && glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Holder.CustomerType != CustomerType.Prospect && glbPolicy.Summary.RisksInsured.length > 0) {
                if (glbPolicy.Summary.RisksInsured[0].Insured.CustomerType == CustomerType.Prospect || Underwriting.validateBeneficiaryProspect(glbPolicy.Summary.RisksInsured[0].Beneficiaries)) {
                    UnderwritingRequest.UpdateProspect(glbPolicy.Id, glbPolicy.Holder.IndividualId).done(function (data) {
                        if (data.success) {
                            glbPolicy.Summary = data.result;
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.ErrorConvertingProspectIntoIndividual, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConvertingProspectIntoIndividual, 'autoclose': true });
                    });
                }
            }
        }

        if (gblProspectData == null) {
            gblProspectData = {};
        }
        gblProspectData.IdCardNo = holder.IdentificationDocument.Number;

        if (holder.CustomerType == CustomerType.Individual) {
            Underwriting.GetIndividualDetailsByIndividualId(holder.IndividualId, holder.CustomerType);
            $("#btnConvertProspect").hide();
        }
        else if (glbPolicy.TemporalType != TemporalType.Quotation || glbPolicy.TemporalType != TemporalType.TempQuotation) {
            if (glbPolicy.Holder.CustomerType == CustomerType.Prospect) {
                UnderwritingRequest.GetHolders(glbPolicy.Holder.IdentificationDocument.Number, 1, CustomerType.Individual, glbPolicy.TemporalType).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 0) {
                            $("#btnConvertProspect").show();
                            return;
                        }
                        if (data.result.length == 1) {
                            Underwriting.LoadHolder(data.result[0]);
                        }
                        else if (data.result.length > 1) {
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].IndividualId,
                                    CustomerType: data.result[i].CustomerType,
                                    Code: data.result[i].InsuredId,
                                    DocumentNum: data.result[i].IdentificationDocument.Number,
                                    Description: data.result[i].Name,
                                    CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                    DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            Underwriting.ShowIndividualResults(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelSelectHolder);
                        }
                    }
                    else {
                        if (glbPolicy.TemporalType == TemporalType.TempQuotation || glbPolicy.TemporalType == TemporalType.Policy) {
                            $("#btnConvertProspect").show();
                        }
                        else {
                            $("#btnConvertProspect").hide();
                        }

                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
                });
            }

        }

    }

    static SetDefaultValues(values) {
        $.each(values, function (index, value) {
            $('#' + this.ControlName).val(this.ControlValue)
        });
    }

    static DeleteTemporal() {
        $.UifDialog('confirm', { 'message': AppResources.RemoveTemporalQuestion }, function (result) {
            if (result) {
                $("#formUnderwriting").validate();
                if ($("#formUnderwriting").valid()) {
                    UnderwritingRequest.DeleteTemporalByOperationId(glbPolicy.Id, 0, 0, 0).done(function (data) {
                        if (data.success) {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            ScrollTop();
                            UnderwritingTemporal.ClearControls();
                            //Underwriting.GetDefaultValues();
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDeleteTemporal, 'autoclose': true });
                    });
                }
            }
        });
    }

    static ValidateDeleteRisk() {
        if ($("#selectProduct").val() != glbPolicy.Product.Id.toString()) {
            var oldProduct = glbPolicy.Product.Id;
            $.UifDialog('confirm', { 'message': AppResources.ConfirmDeleteRisks }, function (result) {
                if (result) {
                    if (glbPolicy.Summary.RiskCount > 0) {
                        glbPolicy.Summary = null;
                        RiskVehicleRequest.GetCiaRiskByTemporalId(glbPolicy.Id).done(function (data) {
                            if (data.success) {
                                if (data.result != null) {
                                    for (var i = 0; i < data.result.length; i++) {
                                        UnderwritingRequest.DeleteRisk(glbPolicy.Id).done(function (data2) {
                                            if (data2.success) {
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                            }
                                        }).fail(function (jqXHR, textStatus, errorThrown) {
                                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDeleteTemporal, 'autoclose': true });
                                        });
                                    }
                                    UnderwritingRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
                                        if (data.success) {
                                            glbPolicy = data.result;
                                            $('#labelRisk').text("0");
                                            $('#labelSum').text("0");
                                            $('#labelPremium').text("0");
                                            $('#labelExpenses').text("0");
                                            $('#labelTaxes').text("0");
                                            $('#labelTotalPremium').text("0");
                                            //limpiar sumary
                                            $.UifNotify('show', { 'type': 'info', 'message': "Riesgo eliminado", 'autoclose': true });
                                        }
                                        else {
                                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                        }
                                    }).fail(function (jqXHR, textStatus, errorThrown) {
                                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
                                    });
                                    Underwriting.LoadSubTitles(6);
                                }
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                    glbRisk = null;
                } else {
                    $("#selectProduct").UifSelect("setSelected", oldProduct);
                    glbPolicy.Product.Id = oldProduct;

                }
            });
        }

    }

    static ValidateDeleteRiskByChangePolicyType() {
        if ($("#selectPolicyType").val() != glbPolicy.PolicyType.Id.toString()) {
            var oldPoliciyType = glbPolicy.PolicyType.Id;
            $.UifDialog('confirm', { 'message': AppResources.ConfirmDeleteRisks }, function (result) {
                if (result) {
                    if (glbPolicy.Summary.RiskCount > 0) {
                        glbPolicy.Summary = null;
                        RiskVehicleRequest.GetCiaRiskByTemporalId(glbPolicy.Id).done(function (data) {
                            if (data.success) {
                                if (data.result != null) {
                                    for (var i = 0; i < data.result.length; i++) {
                                        UnderwritingRequest.DeleteRisk(glbPolicy.Id).done(function (data2) {
                                            if (data2.success) {
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                            }
                                        }).fail(function (jqXHR, textStatus, errorThrown) {
                                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDeleteTemporal, 'autoclose': true });
                                        });
                                    }
                                    UnderwritingRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
                                        if (data.success) {
                                            glbPolicy = data.result;
                                            $.UifNotify('show', { 'type': 'info', 'message': "Riesgo eliminado", 'autoclose': true });
                                        }
                                        else {
                                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                        }
                                    }).fail(function (jqXHR, textStatus, errorThrown) {
                                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorRunRulesPolicyPre, 'autoclose': true });
                                    });
                                    Underwriting.LoadSubTitles(6);
                                }
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                    glbRisk = null;
                } else {
                    $("#selectPolicyType").UifSelect("setSelected", oldPoliciyType);
                    glbPolicy.Product.Id = oldPoliciyType;
                }
            });
        }
    }

    static ValidateAgentUser(agentId, description) {
        var number = parseInt(description, 10);
        var bandValidateAgentUser = false;
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            UnderwritingRequest.GetUserAgenciesByAgentIdDescription(agentId, description).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        bandValidateAgentUser = true;
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserNoHaveAgentEnabled, 'autoclose': true });
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchAgents, 'autoclose': true });
            });
        }
        return bandValidateAgentUser;
    }

    static GetBranches(selectedId) {
        UnderwritingRequest.GetBranches().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectBranch").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectBranch").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }

    /*static ConvertProspect() {
        $('#modalNotificationPerson').modal('hide');
        lockScreen();
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel()
        };

        if (glbPolicy.Holder.IndividualType = 1) {
            router.run("prtPersonNatural");
        } else {
            router.run("prtPersonLegal");
        }
        
    }*/

    static OpenPersonNatural() {
        $('#modalNotificationPerson').modal('hide');
        lockScreen();
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel(false)
        };
        router.run("prtPersonNatural");
    }

    static OpenPersonLegal() {
        $('#modalNotificationPerson').modal('hide');
        lockScreen();
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel(false)
        };
        insuredTmp = { Id: 0 };
        router.run("prtPersonLegal");
    }

    static OpenPerson() {
        $('#modalNotificationPerson').modal('hide');
        lockScreen();
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel(false)
        };
        router.run("prtPersonNatural");
    }

    static OpenProspectusNatural() {
        $('#modalNotificationPerson').modal('hide');
        lockScreen();
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel(false)
        };
        router.run("prtProspectusNatural");

    }

    static OpenProspectusLegal() {
        $('#modalNotificationPerson').modal('hide');
        lockScreen();
        glbPersonOnline = {
            Rol: 1,
            ViewModel: Underwriting.GetPolicyModel(false)
        };
        router.run("prtProspectusLegal");

    }

    static LoadPolicyCombos(policyData, isCollective) {

        UnderwritingRequest.LoadPolicyCombos(policyData, isCollective).done(function (data) {
            if (data.success) {
                //Se cargan las agencias
                for (let i = 0; i < policyData.Agencies.length; i++) {
                    if (policyData.Agencies[i].IsPrincipal == true) {
                        $("#inputAgentPrincipal").data("Object", policyData.Agencies[i]);
                        $("#inputAgentPrincipal").val(policyData.Agencies[i].Agent.FullName);
                        $("#selectAgentAgency").UifSelect({ sourceData: data.result.UserAgencies, selectedId: policyData.Agencies[i].Id });
                        $("#selectAgentAgency").prop('disabled', true);
                        break;
                    }
                }
                //Se cargan las sucursales
                if (policyData.Branch.Id == 0) {
                    $("#selectBranch").UifSelect({ sourceData: data.result.Branches });
                }
                else {
                    $("#selectBranch").UifSelect({ sourceData: data.result.Branches, selectedId: policyData.Branch.Id });
                }
                // Se carga Motivo Justificación Formulario Sarlaft
                if (policyData.JustificationSarlaft == null) {
                    $("#selectJustification").UifSelect({ sourceData: data.result.CompanyJustificationSarlafts });
                } else {

                    $("#selectJustification").UifSelect({ sourceData: data.result.CompanyJustificationSarlafts, selectedId: policyData.JustificationSarlaft.JustificationReasonCode });
                }
                // Se carga Punto de venta
                if (data.result.SalePoints != null && data.result.SalePoints.length > 0) {
                    if (policyData.Branch.SalePoints == null || policyData.Branch.SalePoints.length == 0 || policyData.Branch.SalePoints[0].Id == 0) {
                        $("#selectSalesPoint").UifSelect({ sourceData: data.result.SalePoints });
                    }
                    else {
                        $("#selectSalesPoint").UifSelect({ sourceData: data.result.SalePoints, selectedId: policyData.Branch.SalePoints[0].Id });
                        CommonAgent.GetSalePointsByBranchId(policyData.Branch.Id, 0);
                    }

                }
                else {
                    $("#selectSalesPoint").UifSelect();
                    $.UifNotify('show', { 'type': 'info', 'message': 'No se hallan puntos de venta asociados', 'autoclose': true });
                }
                // Se carga Ramo Comercial
                if (data.result.BasePrefixes != null && data.result.BasePrefixes.length > 0) {
                    if (policyData.Prefix.Id == 0) {
                        $("#selectPrefixCommercial").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectPrefixCommercial").UifSelect({ sourceData: data.result.BasePrefixes, selectedId: policyData.Prefix.Id });
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageAgentWithoutPrefix, 'autoclose': true });
                }

                // Se carga Producto Comercial
                if (policyData.Product.Id == 0) {
                    $("#selectProduct").UifSelect({ sourceData: data.result.CompanyProducts });
                }
                else {
                    $("#selectProduct").UifSelect({ sourceData: data.result.CompanyProducts, selectedId: policyData.Product.Id });

                }

                // Se carga Tipo de Póliza
                var selectedPolicyTypeId = 0;
                if (policyData.PolicyType.Id == 0 && data.result.PolicyTypes != null) {
                    for (let i = 0; i < data.result.PolicyTypes.length; i++) {
                        if (data.result.PolicyTypes[i].IsDefault == true) {
                            selectedPolicyTypeId = data.result.PolicyTypes[i].Id;
                            break;
                        }
                    }
                    if (selectedPolicyTypeId == 0) {
                        $("#selectPolicyType").UifSelect({ sourceData: data.result.PolicyTypes });
                    }
                    else {
                        $("#selectPolicyType").UifSelect({ sourceData: data.result.PolicyTypes, selectedId: selectedPolicyTypeId });
                    }
                }
                else {
                    $("#selectPolicyType").UifSelect({ sourceData: data.result.PolicyTypes, selectedId: policyData.PolicyType.Id });
                }
                // Se carga Moneda
                if (policyData.ExchangeRate.Currency.Id === "") {
                    $("#selectCurrency").UifSelect({ sourceData: data.result.Currencies });
                }
                else {
                    $("#selectCurrency").UifSelect({ sourceData: data.result.Currencies, selectedId: policyData.ExchangeRate.Currency.Id });
                    //Se agrega validacion de 0= pesos, solo se debe consultar la tasa si es diferente
                    if (policyData.ExchangeRate.Currency.Id != 0) {
                        Underwriting.GetExchangeRateByCurrencyId(policyData.ExchangeRate.Currency.Id);
                    }
                }

                Underwriting.DisabledControlByEndorsementType(policyData.Endorsement.EndorsementType, policyData.Prefix.Id, true);
                if (policyData != null && policyData.Endorsement.QuotationId != null && policyData.Endorsement.QuotationId > 0 && (policyData.TemporalType == TemporalType.Quotation || policyData.TemporalType == TemporalType.TempQuotation))
                    $("#selectBranch").prop('disabled', true);
            }

        });
    }

    static DeleteEndorsementControl(EndorsementId) {

    }

    // Riesgo Copia temporal
    static ShowModalRiskFromPolicy() {
        var resultOperation = false;
        $("#formUnderwriting").validate();
        if ($("#formUnderwriting").valid()) {
            //guardar temporal
            var policyData = Underwriting.GetPolicyModel(false);
            policyHolderData = policyData;
            UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities).done(function (data) {
                if (data.success) {
                    resultOperation = true;
                    Underwriting.UpdateGlbPolicy(data.result);
                    Underwriting.LoadTitle(glbPolicy);
                    Underwriting.LoadSubTitles(0);

                    //if (glbPolicy.TemporalType != TemporalType.Quotation) {
                    //    if (glbPolicy.Holder.InsuredId == 0) {
                    //        if (gblProspectData.IdCardNo == null) {
                    //            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyholderWithoutRol, 'autoclose': true });
                    //            resultOperation = false;
                    //        }
                    //    }
                    //    else if (FormatDate(glbPolicy.Holder.DeclinedDate) != '01/01/0001') {
                    //        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyholderDisabled, 'autoclose': true });
                    //        resultOperation = false;
                    //    }
                    //}
                    if (resultOperation !== false) {
                        let events = TypeAuthorizationPolicies.Nothing;
                        if (glbPolicy.InfringementPolicies != null) {
                            /// se lanzan los eventos de la pantalla principal
                            events = LaunchPolicies.ValidateInfringementPolicies(glbPolicy.InfringementPolicies, true);
                        }

                        UnderwritingRequest.GetBranches().done(function (data) {
                            if (data.success) {
                                $("#selectBranchRisk").UifSelect({ sourceData: data.result });
                            }
                        });
                        TemporalRequest.GetPrefixes().done(dataPrefix => {
                            if (dataPrefix.success) {
                                $('#selectPrefixRisk').UifSelect({ sourceData: dataPrefix.result });
                            }
                        });
                        $("#inputPolicyNumber").val('');
                        $('#modalRiskFromPolicy').UifModal('showLocal', "Recuperar Riesgo desde otra póliza");
                    }
                }
                else {
                    resultOperation = false;
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                resultOperation = false;
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
            });
        }
    }


    static SearchRiskTemporal() {

        if ($("#chkTemporal").prop("checked")) {
            var branch = $("#selectBranchRisk").UifSelect("getSelected");
            var prefix = $("#selectPrefixRisk").UifSelect("getSelected");
            var endorsement = null;
            var listEndorsement = [];
            if (branch == "" && prefix == "") {
                branch = 0
                prefix = 0
            }
            gblCurrentRiskTemporalNumber = $('#inputRiskFromTemporal').val().trim();
            TemporalRiskUnderwriting.GetRiskByTemporald(gblCurrentRiskTemporalNumber).done(function (dataRisk) {
                if (dataRisk.success) {
                    if (dataRisk.result != null) {
                        if ($('#selectPrefixCommercial').val() == dataRisk.result[0].Risk.Policy.Prefix.Id && $("#selectProduct").val() == dataRisk.result[0].Risk.Policy.Product.Id) {
                            var totalRisks = parseInt(glbPolicy.Summary.RiskCount, 10) + parseInt(dataRisk.result.length, 10);
                            TemporalRiskUnderwriting.GetRiskByTemporalId(gblCurrentRiskTemporalNumber, glbPolicy.Id, $("#selectPrefixCommercial").val(), glbPolicy.TemporalType, true).done(function (dataRisk) {
                                if (dataRisk.success) {
                                    var policyData = Underwriting.GetPolicyModel(false);
                                    UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities).done(function (data) {
                                        Underwriting.UpdateRisksInclusion();
                                        $("#selectedInclusionRisk").text("(" + totalRisks + ")");
                                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                                        if ($('#selectPrefixCommercial').val() == PrefixType.CUMPLIMIENTO) {
                                            glbRisk = dataRisk.result;
                                            gblCurrentRiskTemporalNumber = data.result.Id;
                                        }
                                        else {
                                            gblCurrentRiskTemporalNumber = null;
                                        }
                                        changeHolder = true;
                                        //Underwriting.RedirectToRisk();
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InformationRiskRecovery, 'autoclose': true });
                                        $("#modalRiskFromPolicy").UifModal("hide");
                                    });
                                }
                                else {
                                    gblCurrentRiskTemporalNumber = null;
                                    $.UifNotify('show', { 'type': 'danger', 'message': dataRisk.result, 'autoclose': true });
                                    unlockScreen();
                                }


                            });
                        }
                        else {
                            if ($("#selectProduct").val() != dataRisk.result[0].Risk.Policy.Product.Id) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDifferentProduct, 'autoclose': true });
                            } else {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorConsultRiskFromTemporal, 'autoclose': true });
                            }
                            gblCurrentRiskTemporalNumber = null;
                            unlockScreen();
                        }

                    }
                    else {
                        gblCurrentRiskTemporalNumber = null;
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRiskFromTemporal, 'autoclose': true });
                        unlockScreen();
                    }

                }
                else {
                    gblCurrentRiskTemporalNumber = null;
                    $.UifNotify('show', { 'type': 'danger', 'message': dataRisk.result, 'autoclose': true });
                }
            });
        }
    }


    static SearchRiskFromPolicy() {

        if ($("#chkPolicy").prop("checked")) {
            var branch = $("#selectBranchRisk").UifSelect("getSelected");
            var prefix = $("#selectPrefixRisk").UifSelect("getSelected");
            var endorsement = null;
            var listEndorsement = [];
            if (branch == "" && prefix == "") {
                branch = 0
                prefix = 0
            }

            //Traer los endosos asociados a la poliza
            TemporalRiskUnderwriting.GetCompanyEndorsementsByFilterPolicy(branch, prefix, parseInt($('#inputPolicyNumber').val()), null).done(function (data) {
                if (data.success) {
                    endorsement = data.result.endorsements[0];
                    //Traer informacion de la poliza asociada al ultimo endoso realizado
                    TemporalRiskUnderwriting.GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsement.Id, true).done(function (dataSt) {

                        if (dataSt.result != null) {
                            var date = new Date();
                            //Validar si el producto del temporal y el de la poliza a recuperar es distinto
                            if (dataSt.result.Product.Id != glbPolicy.Product.Id) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDifferentProduct, 'autoclose': true });
                            }
                            else if (FormatDate(dataSt.result.Endorsement.CurrentTo) > FormatDate(date)) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorCurrentPolicy, 'autoclose': true });
                            }
                            else {
                                //Cargar riesgos y guardarlos a la temporal que esta en glbPolicy
                                TemporalRiskUnderwriting.GetRiskByPolicyId(dataSt.result.Endorsement.PolicyId, glbPolicy.Id, $("#selectPrefixCommercial").val(), glbPolicy.TemporalType, true).done(function (dataRisk) {

                                    if (dataRisk.success) {
                                        var totalRisks = parseInt(glbPolicy.Summary.RiskCount, 10) + parseInt(dataRisk.result.length, 10);
                                        $("#selectedInclusionRisk").text("(" + totalRisks + ")");
                                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                                        $("#modalRiskFromPolicy").UifModal("hide");
                                        Underwriting.UpdateRisksInclusion();
                                        if (dataRisk.result != null && dataRisk.result.length > 0)
                                            glbRisk = dataRisk.result[0].Risk;
                                        else
                                            glbRisk = dataRisk.result;
                                        changeHolder = true;
                                        //Underwriting.RedirectToRisk();
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InformationRiskRecovery, 'autoclose': true });
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'danger', 'message': dataRisk.result, 'autoclose': true });
                                    }
                                });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
            });
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
        }
    }

    static PrintTemporal() {
        UnderwritingRequest.GenerateReportTemporary(glbPolicy.Endorsement.TemporalId, glbPolicy.Prefix.Id, glbPolicy.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }

    static ValidateExchangeRateUnder() {
        return new Promise(function (resolve, reject) {
            var result = true;
            if (glbPolicy != null && glbPolicy.ExchangeRate != null && glbPolicy.ExchangeRate.Currency != null && glbPolicy.ExchangeRate.Currency.Id != 0) {
                UnderwritingRequest.GetExchangeRateByCurrencyId(glbPolicy.ExchangeRate.Currency.Id).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            $("#inputChange").val(FormatMoney(data.result.SellAmount));
                            if (FormatDate(data.result.RateDate) != FormatDate(IssueDate) && data.result.Currency.Id != 0) {
                                $.UifNotify('show', {
                                    'type': 'danger', 'message': Resources.Language.updateexchangerate, 'autoclose': true
                                });
                                result = false;
                            }
                            else {
                                result = true;
                            }
                        }
                    }
                    resolve(result);
                });
            }
            else {
                var ExchangeRateUnder = parseInt($('#selectCurrency').UifSelect('getSelected'));
                if ($('#selectCurrency').UifSelect('getSelected') != "" && ExchangeRateUnder != null && ExchangeRateUnder != 0) {
                    UnderwritingRequest.GetExchangeRateByCurrencyId(ExchangeRateUnder).done(function (data) {
                        if (data.success) {
                            if (data.result != null) {
                                $("#inputChange").val(FormatMoney(data.result.SellAmount));
                                if (FormatDate(data.result.RateDate) != FormatDate(IssueDate) && data.result.Currency.Id != 0) {

                                    $.UifNotify('show', {
                                        'type': 'danger', 'message': Resources.Language.updateexchangerate, 'autoclose': true
                                    });
                                    result = false;
                                }
                                else {
                                    result = true;
                                }
                            }
                        }
                        resolve(result);
                    });

                }
                else
                    resolve(result);
            }
        });

    }
}