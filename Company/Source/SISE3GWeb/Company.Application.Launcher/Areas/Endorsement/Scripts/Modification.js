var Prefix;
var glbPolicyEndorsement = null;
var temporalModification = 0;
var policyTemporal = null;
var ValueAmount = 0;
var PreviousContractValue = 0;

class Modification extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            $("input[type=text]").keyup(function () {
                $(this).val($(this).val().toUpperCase());
            });
            $('#inputCurrentFrom').UifDatepicker("setValue", FormatDate(glbPolicy.CurrentFrom));
            $('#inputCurrentTo').UifDatepicker("setValue", FormatDate(glbPolicy.Endorsement.CurrentTo));
            $('#inputCurrentFrom').UifDatepicker("setMaxDate", FormatDate(glbPolicy.Endorsement.CurrentTo));
            $('#inputCurrentTo').UifDatepicker("setMinDate", FormatDate(glbPolicy.CurrentFrom));
            $("#inputCurrentFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $("#inputCurrentTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
            $('#inputRecord').prop('disabled', true);
            glbPolicy.TemporalType = TemporalType.Endorsement;

            $('#inputIssueDate').UifDatepicker('disabled', true);
            $('#inputDays').prop('disabled', true);
            Modification.Coinsurance();

            Modification.ValidateSummary();
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && glbPolicy.EndorsementTexts == undefined) {
                glbPolicy.EndorsementTexts = glbpolicyEndorsementTexts;
            }

            Modification.LoadRecordObservation();

            if (glbPolicy.Id > 0) {
                $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
                Modification.GetTemporalById(glbPolicy.EndorsementController, glbPolicy.Id);
            }
            else {
                Modification.GetModificationReasons(0);
                Modification.GetModificationType(0);
                Modification.DisableModificationDate(true);
                $('#inputDays').val(CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val()));
            }
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
            GetDateIssue();
            this.ValidatePolicies();
        }
        if (glbPolicy == null || glbPolicy.PolicyOrigin != PolicyOrigin.Collective) {
            $('#btnConsultationRisk').hide();
        }
        if (glbPolicy.Id > 0) {
            if (glbPolicy.Endorsement.TemporalId == undefined || glbPolicy.Endorsement.TemporalId == 0) {
                ModificationRequest.GetSummaryByTemporalId(glbPolicy.Id).done(function (data) {
                    if (data.success) {
                        glbPolicy.Endorsement.TemporalId = data.result.TempId;
                    }
                });
            }
            $('#btnPrintTemporalModification').show();
        }
        else {
            $('#btnPrintTemporalModification').hide();
        }
    }

    //Eventos
    bindEvents() {
        $("#inputCurrentFrom").on("datepicker.change", function (event, date) {
            $('#inputCurrentTo').UifDatepicker("setMinDate", date);
            Modification.ValidateCancellationDate(glbPolicy.CurrentFrom, $('#inputCurrentFrom').val(), null);
        });

        $("#inputCurrentTo").on("datepicker.change", function (event, date) {
            $('#inputCurrentFrom').UifDatepicker("setMaxDate", date);
            Modification.ValidateCancellationDate(null, $('#inputCurrentTo').val(), glbPolicy.Endorsement.CurrentTo);
        });

        $("#btnCalculate").on('click', function () {
            if ($("#selectModificationType").val() == enumModificationType.Prorogation || $("#selectModificationType").val() == enumModificationType.ModificationProrogation) {

                //if (FormatDate($("#inputCurrentTo").val()) <= FormatDate(glbPolicy.Endorsement.CurrentTo)) {
                    if (CompareDates($("#inputCurrentTo").val(), glbPolicy.Endorsement.CurrentTo) === 0 || CompareDates($("#inputCurrentTo").val(), glbPolicy.Endorsement.CurrentTo) === 2) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorModificationDateLowerOrEqualCurrentTo, 'autoclose': true });
                        return false;
                }
                else {
                    if (glbPolicy != null && glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                        if (!Modification.ValidateCollective()) {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorLicensePlate, 'autoclose': true });
                            return false;
                        }
                    }
                }
            }
            else {
                if (glbPolicy != null && glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                    if (!Modification.ValidateCollective()) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorLicensePlate, 'autoclose': true });
                        return false;
                    }
                }
            }
            Modification.ValidateCreateTemporalCoInsuredAcccepted();
        });

        $("#btnModificationCancel").on('click', function () {

            Modification.RedirectSearchController();
        });

        $("#btnModificationSave").on('click', function () {
            if (IsEdit != undefined || IsEdit != null) {
                if (IsEdit) {
                    IsEdit = false;
                }
            }
			var ValuePremium = 0;
			for (var i = 0; i < glbPolicyEndorsement.PayerComponents.length; i++) {
				ValuePremium += glbPolicyEndorsement.PayerComponents[i].Amount;
			}
            if (glbPolicyEndorsement.Summary.FullPremium.toFixed(2) != ValuePremium.toFixed(2)) {
				$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ValidatePayer, 'autoclose': true });
			} else {
				if (glbPolicyEndorsement != null && glbPolicyEndorsement.PaymentPlan != null && glbPolicyEndorsement.PaymentPlan.PremiumFinance != null) {
					$.UifDialog('confirm',
						{ 'message': Resources.Language.NotificationPaymentPlan },
						function (result) {
							if (result) {
								Modification.CreateEndorsement();
							}
						});
				} else {
					Modification.CreateEndorsement();
				}
			}
        });

        $("#inputDescriptionRisk").on("buttonClick", this.descriptionRisk);

        $('input[type=radio][name=chkCurrency]').change(this.changeCurrencyCheck);
        $('#selectModificationType').on('itemSelected', this.ModificationType);
        $("#btnPrintTemporalModification").click(Modification.PrintTemporal);
    }
    //Crear metodo para recalcular en los campos de FormatMoneyCurrency
    static setInputCurrency(CurrencyType) {

        if (policyTemporal != null) {

            FormatMoneyCurrency(CurrencyType, policyTemporal.Summary.AmountInsured, $("#labelSum").prop("id"));
            FormatMoneyCurrency(CurrencyType, policyTemporal.Summary.FullPremium, $("#labelTotalPremium").prop("id"));
            FormatMoneyCurrency(CurrencyType, policyTemporal.Summary.Premium, $("#labelPremium").prop("id"));
            FormatMoneyCurrency(CurrencyType, policyTemporal.Summary.Expenses, $("#labelExpenses").prop("id"));
            FormatMoneyCurrency(CurrencyType, policyTemporal.Summary.Taxes, $("#labelTaxes").prop("id"));
        }
        else {
            FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.AmountInsured, $("#labelSum").prop("id"));
            FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.FullPremium, $("#labelTotalPremium").prop("id"));
            FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.Premium, $("#labelPremium").prop("id"));
            FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.Expenses, $("#labelExpenses").prop("id"));
            FormatMoneyCurrency(CurrencyType, glbPolicy.Summary.Taxes, $("#labelTaxes").prop("id"));
        }
    }

    static getExchangeRateModel(currencyId, CurrencyType) {
        UnderwritingRequest.GetExchangeRateByCurrencyId(currencyId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    currencyObject = {
                        exchangeRate: data.result.SellAmount,
                        local: CurrencyType,
                        currencyType: currencyId//data.result.Currency.Id
                    }
                    Modification.setInputCurrency(CurrencyType);
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
                if (glbPolicyEndorsement.ExchangeRate.Currency.Id == EnumCurrency.Dolares) {
                    Modification.getExchangeRateModel(EnumCurrency.Pesos, glbPolicyEndorsement.ExchangeRate.Currency.Id);
                }
                else if (glbPolicyEndorsement.ExchangeRate.Currency.Id == EnumCurrency.Euros) {
                    Modification.getExchangeRateModel(EnumCurrency.Pesos, glbPolicyEndorsement.ExchangeRate.Currency.Id);
                }
                else {
                    Modification.getExchangeRateModel(EnumCurrency.Dolares, glbPolicyEndorsement.ExchangeRate.Currency.Id);
                }
                $('#chkCurrencyIssue').prop('checked', true);
                break;
            case 'Load':
                $('#chkCurrencyLoad').prop('checked', true);
                break;
            case 'Local':
                Modification.getExchangeRateModel(glbPolicyEndorsement.ExchangeRate.Currency.Id, 0);
                $('#chkCurrencyLocal').prop('checked', true);
                break;
            default:
                $('#chkCurrencyIssue').prop('checked', true);
                break;
        }
    }
    ModificationType() {
        Modification.DisableModificationDate(true);
    }
    descriptionRisk() {
        if (glbPolicy != null && glbPolicy.Product.CoveredRisk.SubCoveredRiskType == SubCoveredRiskType.Vehicle && glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
            if (Modification.ValidationPlate()) {
                setTimeout(function () { lockScreen(); });
                Modification.SearchByRiskDescription();
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': "La placa ingresada no es valida", 'autoclose': true });

            }
        }

    }
    static Coinsurance() {
        switch (glbPolicy.BusinessType) {
            case BusinessType.Accepted:
                $("#divCoInsuranceAccepted").show();
                break;
            case BusinessType.Assigned:
                $("#divCoInsuranceAccepted").hide();
                break;
            default:
                $("#divCoInsuranceAccepted").hide();
                break;
        }
    }
    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(glbPolicy.Id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Modification.RedirectSearchController();
                }
            });
        }
    }

    static ValidationPlate() {


        var regex = /(^[a-zA-Z]{3}[0-9]{3}$)/;
        var regex2 = /(^[a-zA-Z]{3}[0-9]{2}[a-zA-Z]{1}$)/
        var regex3 = /(^[rRsS]{1}[0-9]{5}$)/
        var regex4 = /(^[a-zA-z]{2}[0-9]{4}$)/
        if (!regex.test($("#inputDescriptionRisk").val()) && !regex2.test($("#inputDescriptionRisk").val()) && !regex3.test($("#inputDescriptionRisk").val()) && !regex4.test($("#inputDescriptionRisk").val())) {
            return false;
        } else {
            return true;
        }
    }

    static SearchByRiskDescription() {
        if ($('#inputDescriptionRisk').val().trim().length > 0) {
            $("#resultConsultationRisk").html("");
            $("#resultConsultationRisk").show();
            setTimeout(lockScreen(), 2000);
            ModificationRequest.SearchByRiskDescription().done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $("#resultConsultationRisk").css({ border: "1px solid #e0e2e3", "margin-left": "-4px", "margin-right": "-4px" });
                        switch (glbPolicy.Product.CoveredRisk.SubCoveredRiskType) {

                            case SubCoveredRiskType.Vehicle:
                                $("#resultConsultationRisk").html(
                                    "<strong>  " + data.result.LicensePlate + "-"
                                    + (data.result.Make.Description ? " Marca: " + data.result.Make.Description : "")
                                    + (data.result.Model.Description ? " Modelo: " + data.result.Model.Description : "")
                                    + " Año: " + data.result.Year + "</strong><br/>" +
                                    (data.result.Price ? " Valor: " + "$" + FormatMoney(data.result.Price) : "$0") +
                                    (data.result.PriceAccesories ? " Accesorios: " + data.result.PriceAccesories : "") +
                                    " <br/> " + (data.result.Version.Description ? " Tipo: " + data.result.Version.Description : "")
                                );
                                glbPolicyEndorsement.DescriptionRisk = data.result.LicensePlate;
                                //ModificationModel.DescriptionRisk = data.result.LicensePlate;
                                break;
                            case SubCoveredRiskType.Property:
                                $("#resultConsultationRisk").html("<strong>" + data.result.FullAddress + "</strong>");
                                ModificationModel.DescriptionRisk = data.result.Number;
                                break;
                        }
                    }
                }
                else {
                    //ModificationModel.DescriptionRisk = null;
                    glbPolicyEndorsement.DescriptionRisk = null;
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                resultOperation = false;
                glbPolicyEndorsement.DescriptionRisk = null;
                //ModificationModel.DescriptionRisk = null;
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryPlate, 'autoclose': true });
            });
        }
        else {
            $("#resultConsultationRisk").text("");
            $("#resultConsultationRisk").hide();
        }
    }
    static ShowSearchPlate() {
        if ($("#selectPrefix").UifSelect("getSelected") == PrefixCollective.Vehicle && policy.PolicyOrigin == PolicyOrigin.Collective) {
            $("#btnConsultationRisk").show();
            $("#resultConsultationRisk").show();
        }
    }

    static HideSearchPlate() {
        $("#resultConsultationRisk").hide();
        $("#inputDescriptionRisk").val("");
        $("#resultConsultationRisk").text("");
        $("#btnConsultationRisk").hide();
        $("#resultConsultationRisk").css("border", "none");
    }
    static ValidateSummary() {
        SearchRequest.GetCompanyEndorsementsByFilterPolicy(glbPolicy.Branch.Id, glbPolicy.Prefix.Id, glbPolicy.DocumentNumber, true).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    SearchRequest.GetCurrentPolicyByEndorsementId(data.result.Id, true).done(function (data) {
                        if (data.success) {
                            if (data.result != null) {
                                Modification.LoadCurrentSummaryEndorsement(data.result);
                            }
                            else {
                                Modification.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
                            }
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadCurrentSummaryEndorsement(policyData) {
        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(FormatMoney(policyData.Summary.AmountInsured));
            PreviousContractValue = parseFloat(NotFormatMoney(policyData.Summary.AmountInsured).replace(",", "."));
            $('#labelCurrentPremium').text(FormatMoney(policyData.Summary.Premium));
            $('#labelCurrentExpenses').text(FormatMoney(policyData.Summary.Expenses));
            $('#labelCurrentSurcharges').text(FormatMoney(policyData.Summary.Surcharges));
            $('#labelCurrentDiscounts').text(FormatMoney(policyData.Summary.Discounts));
            $('#labelCurrentTaxes').text(FormatMoney(policyData.Summary.Taxes));
            $('#labelCurrentTotalPremium').text(FormatMoney(policyData.Summary.FullPremium));
            $('#chkCurrencyIssue').prop('checked', false);
            $('#chkCurrencyLoad').prop('checked', false);
            $('#chkCurrencyLocal').prop('checked', false);
            ValueAmount = FormatMoney(policyData.Summary.AmountInsured);
            switch (policyData.ExchangeRate.Currency.Id) {
                case 1:
                    $('#chkCurrencyIssue').prop('checked', true);
                    break;
                case 0:
                    $('#chkCurrencyLocal').prop('checked', true);
                    break;
                default:
                    $('#chkCurrencyIssue').prop('checked', true);
                    break;
            }
        }
    }

    static GetTemporalById(endorsementController, temporalId) {


        ModificationRequest.GetTemporalById(endorsementController, temporalId).done(function (data) {
            if (data.success) {
                Modification.LoadEndorsementData(data.result);
                $('#inputDays').val(CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val()));
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static LoadEndorsementData(policy) {
        policyTemporal = policy;
        $('#inputIssueDate').UifDatepicker("setValue", FormatFullDate(policy.IssueDate));
        $('#inputCurrentFrom').UifDatepicker("setValue", FormatDate(policy.CurrentFrom));
        $('#inputCurrentTo').UifDatepicker("setValue", FormatDate(policy.CurrentTo));
        $('#inputCurrentFrom').UifDatepicker("setMaxDate", FormatDate(policy.CurrentTo));
        $('#inputCurrentTo').UifDatepicker("setMinDate", FormatDate(policy.CurrentFrom));
        Modification.GetModificationReasons(policy.Endorsement.EndorsementReasonId);
        Modification.GetModificationType(policy.Endorsement.ModificationTypeId);
        $('#inputText').val(policy.Endorsement.Text.TextBody);
        $('#inputObservations').val(policy.Endorsement.Text.Observations);

        $("#inputTicketNumber").val(policy.Endorsement.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(policy.Endorsement.TicketDate));
        if ($("#inputBusinessTypeDescription").val(policy.CoInsuranceCompanies != null)) {
            $("#inputBusinessTypeDescription").val(policy.CoInsuranceCompanies[0].PolicyNumber);
        }


        if (policy.Summary != null) {
            $('#labelRisk').text(policy.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(policy.Summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(policy.Summary.Premium));
            $('#labelExpenses').text(FormatMoney(policy.Summary.Expenses));
            $('#labelTaxes').text(FormatMoney(policy.Summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(policy.Summary.FullPremium));
        }
        glbPolicyEndorsement = policy;

    }

    static ValidateCancellationDate(currentFrom, modificationDate, currentTo) {
        if (enumModificationType.Modification == $("#selectModificationType").val()) {
            if (CompareDates(currentFrom, modificationDate) == 1) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorModificationDateGreater, 'autoclose': true });
                $('#inputCurrentFrom').UifDatepicker("setValue", FormatDate(glbPolicy.CurrentFrom));
            }
        }
        else if (enumModificationType.Prorogation == $("#selectModificationType").val()) {
            if (CompareDates(currentTo, modificationDate) == 1) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorModificationDateGreaterCurrentTo, 'autoclose': true });
                $('#inputCurrentTo').UifDatepicker("setValue", FormatDate(glbPolicy.Endorsement.CurrentTo));
            }
        }
        else if (enumModificationType.ModificationProrogation == $("#selectModificationType").val()) {
            if (CompareDates(currentFrom, modificationDate) == 1) {
                $('#inputCurrentFrom').UifDatepicker("setValue", FormatDate(glbPolicy.CurrentFrom));
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorModificationDateGreater, 'autoclose': true });
            }
            else if (CompareDates(currentTo, modificationDate) == 1) {
                $('#inputCurrentTo').UifDatepicker("setValue", FormatDate(glbPolicy.Endorsement.CurrentTo));
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorModificationDateGreaterCurrentTo, 'autoclose': true });
            }
        } else if ($("#selectModificationType").val() == 4) {
            if (CompareDates(currentFrom, modificationDate) == 1) {
                $('#inputCurrentFrom').UifDatepicker("setValue", FormatDate(glbPolicy.CurrentFrom));
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorModificationDateGreater, 'autoclose': true });
            }
            else if (CompareDates(modificationDate, currentTo) == 1) {
                $('#inputCurrentTo').UifDatepicker("setValue", FormatDate(glbPolicy.Endorsement.CurrentTo));
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorModificationDateLowerCurrentTo, 'autoclose': true });
            } else {
                let originalDays = CalculateDays(glbPolicy.CurrentFrom, glbPolicy.Endorsement.CurrentTo);
                let currentDays = CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val());

                if (currentDays >= originalDays) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NotDecreasePolicy, 'autoclose': true });
                }
            }
        }

        $('#inputDays').val(CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val()));
    }

    static ValidateCreateTemporalCoInsuredAcccepted() {
        if ($("#selectModificationType").val() == 4) {

            let originalDays = CalculateDays(glbPolicy.CurrentFrom, glbPolicy.Endorsement.CurrentTo);
            let currentDays = CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val());

            if (currentDays >= originalDays) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NotDecreasePolicy, 'autoclose': true });
                return;
            }
        }
        

        if ($("#selectModificationType").val() == 2 || $("#selectModificationType").val() == 3 || $("#selectModificationType").val() == 1) {

           // let originalDays = CalculateDays(glbPolicy.CurrentFrom, glbPolicy.Endorsement.CurrentTo);
            let currentDays = CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val());

            if (currentDays == 0) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NotdifferencePolicy, 'autoclose': true });
                return;
            }
        }


        if (glbPolicy.BusinessType == BusinessType.Accepted) {
            if ($("#inputBusinessTypeDescription").val() != 0) {
                Modification.CreateTemporal();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ValidateCoInsuranceAccepted, 'autoclose': true });
            }
        }
        else {
            Modification.CreateTemporal();
        }
    }
    static CreateTemporal() {
        $("#formModification").validate();
        if ($("#formModification").valid()) {
            var objectModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formModification);
            objectModel.Summary = [];
            objectModel.Summary.RiskCount = 0;
            objectModel.BusinessTypeDescription = objectModel.BusinessTypeDescription || localStorage.getItem("BusinessTypeDescription");
            objectModel.ModificationTypeId = $("#selectModificationType").val();
            glbPolicy.Endorsement.ModificationTypeId = $("#selectModificationType").val();
            lockScreen();
            ModificationRequest.CreateTemporal(glbPolicy.EndorsementController, objectModel).done(function (data) {
                if (data.success) {
                    data.result.EndorsementTexts = glbPolicy.EndorsementTexts;
                    Modification.LoadRiskView(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static LoadRiskView(policy) {
        glbPolicy.Id = policy.Id;
        glbpolicyEndorsementTexts = policy.EndorsementTexts;
        router.run("prtEndorsement");
    }

    static GetPolicyModelView(policy) {
        var policyModelsView = {
            Id: policy.Id,
            PrefixId: policy.Prefix.Id,
            ProductId: policy.Product.Id,
            PolicyType: policy.PolicyType.Id,
            HolderId: policy.Holder.IndividualId,
            HolderName: policy.Holder.Name,
            IssueDate: FormatFullDate(policy.IssueDate),
            CoveredRiskType: policy.Product.CoveredRisk.CoveredRiskType,
            EndorsementType: policy.Endorsement.EndorsementType,
            Title: policy.TemporalTypeDescription + ': ' + policy.Id + ' ' + policy.Endorsement.EndorsementTypeDescription
        };

        return policyModelsView;
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static async CreateEndorsement() {
        if (glbPolicy.Id > 0) {
            glbPolicy.Endorsement.EndorsementType = EndorsementType.Modification;
            ValueAmount = undefined;
            glbRisk = {};

            $("#formModification").validate();

            if ($("#formModification").valid()) {

                lockScreen();
                var companyModification = {
                    Text: $('#inputText').val(),
                    Observations: $('#inputObservations').val(),
                    RegistrationNumber: $("#inputTicketNumber").val(),
                    RegistrationDate: $("#inputTicketDate").val()
                };

                if ((glbPolicy.Prefix.Id == 30 || glbPolicy.Prefix.Id == 32) && glbPolicyEndorsement.Summary.AmountInsured != 0) {
                    var risk = await RiskSuretyRequest.GetCiaRiskByTemporalId(glbPolicy.Id);
                    if (risk.success) {
                        risk = risk.result;
                        if (risk.length == 1) {
                            risk = await RiskSuretyRequest.GetRiskSuretyById(glbPolicy.Id, risk[0].Id);

                            if (risk.success) {
                                risk = risk.result;

                                glbPolicy.Summary.AmountInsured = glbPolicyEndorsement.Summary.AmountInsured;

                                $("#inputTotalSumInsuredRisk").remove();
                                $("#inputAvaliableOperationQuota").remove();
                                $("#inputAvailable").remove();

                                //style = "visibility: hidden"

                                $("#formModification").append(`<input style = "visibility: hidden" name="inputAvailable" id="inputAvailable" value="${risk.Available}" />`);
                                $("#formModification").append(`<input style = "visibility: hidden" name="inputTotalSumInsuredRisk" id="inputTotalSumInsuredRisk" value="${risk.Risk.AmountInsured}" />`);
                                $("#formModification").append(`<input style = "visibility: hidden" name="inputAvaliableOperationQuota" id="inputAvaliableOperationQuota" value="${risk.Available}" />`);

                                $("#inputAvailable").text(risk.Available);
                                $("#inputAvaliableOperationQuota").text(risk.Available);
                                $("#inputTotalSumInsuredRisk").text(risk.Risk.AmountInsured);

                                if (await RiskSurety.LoadAggregate(risk.Contractor.IndividualId) == false) {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
                                    return;
                                }
                                if (ValidityParticipant1 != undefined && ValidityParticipant1 != null) {
                                    if (ValidityParticipant1.some(x => x.consortiumEventDTO.IsConsortium == true) || ValidityParticipant1.some(x => x.IndividualOperatingQuota.IndividualID == 0)) {
                                        ValidityParticipant1.forEach((x) => {
                                            if (x.consortiumEventDTO.IsConsortium == true) {
                                                $.UifNotify('show', { 'type': 'danger', 'message': `El integrante ${x.consortiumEventDTO.ConsortiumpartnersDTO.PartnerName} no tiene cupo disponible`, 'autoclose': true });
                                            }
                                        });

                                        ValidityParticipant1 = null;
                                        return;
                                    }
                                }

                                if (PreviousContractValue > risk.Risk.AmountInsured) {
                                    differenceContract = PreviousContractValue - risk.Risk.AmountInsured;
                                    differenceContract = differenceContract * -1;
                                }
                                else {
                                    differenceContract = risk.Risk.AmountInsured - PreviousContractValue;
                                }

                                if (parseInt(NotFormatMoney($('#inputTotalSumInsuredRisk').text())) <= parseFloat(NotFormatMoney($("#inputAvailable").val())) || differenceContract <= parseFloat(NotFormatMoney($("#inputAvailable").val()))) {
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
                                    return;
                                }
                            }
                            else {
                                unlockScreen();
                                $.UifNotify('show', { 'type': 'danger', 'message': 'Error generando el Endoso', 'autoclose': true });
                                return;
                            }
                        }
                    } else {
                        unlockScreen();
                        $.UifNotify('show', { 'type': 'danger', 'message': 'Error generando el Endoso', 'autoclose': true });
                        return;
                    }
                }

                ModificationRequest.CreateEndorsement(glbPolicy.EndorsementController, glbPolicy.Id, glbPolicy.DocumentNumber, companyModification).done(function (data) {
                    /// Lanza el resumen de los eventos, si no existen se genera la poliza
                    if (data.success) {
                        if (data.result != null) {
                            if (Array.isArray(data.result) && data.result.length > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(data.result, glbPolicy.Id, FunctionType.Individual);
                            } else {
                                Search.SetNewEndorsement(data.result, glbPolicy.Endorsement.Id);
                                $.UifDialog('confirm',
                                    { 'message': data.result + '\n ' + Resources.Language.MessagePrint },
                                    function (result) {
                                        if (result) {
                                            var message = data.result.split(':');
                                            var endorsementNumber = parseFloat(message[2]);
                                            PrinterRequest.PrintReportFromOutside(glbPolicy.Branch.Id,
                                                glbPolicy.Prefix.Id,
                                                glbPolicy.DocumentNumber, parseFloat(message[3])).done(function (data) {
                                                    if (data.success) {
                                                        if (data.result.Url != undefined) {
                                                            DownloadFile(data.result.Url, true, (url) => url.match(/([^\\]+.pdf)/)[0]);
                                                        }
                                                        else {
                                                            $.UifNotify('show', { 'type': 'info', 'message': 'Se genero el proceso de impresion numero ' + data.result + ' favor consultarlo en pantalla de impresion', 'autoclose': false });
                                                        }
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
                                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                                });
                                        }
                                        Modification.RedirectSearchController();
                                    });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ModificationNoDone, 'autoclose': true });
        }
    }

    static GetModificationReasons(selectedId) {
        ModificationRequest.GetModificationReasons().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectModificationReason").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectModificationReason").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }

    static GetModificationType(selectedId) {
        ModificationRequest.GetModificationType().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectModificationType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectModificationType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $("#selectModificationType").UifSelect('disabled', true);
                }
                Modification.DisableModificationDate(true);
            }

        });
    }

    static ValidateCollective() {
        if (($("#selectModificationReason").val() != 1 && $("#selectModificationReason").val() != 9)) {
            if (glbPolicyEndorsement.DescriptionRisk == null)
                return false;
        }
        return true;
    }

    static DisableModificationDate(disabled) {
        if (!glbPolicy.Id) {
            $('#inputCurrentFrom').UifDatepicker("setValue", FormatDate(glbPolicy.CurrentFrom));
            $('#inputCurrentTo').UifDatepicker("setValue", FormatDate(glbPolicy.Endorsement.CurrentTo));
        }

        if (enumModificationType.Modification == $("#selectModificationType").val()) {
            $("#inputCurrentFrom").prop('disabled', false);
            $("#inputCurrentTo").prop('disabled', true);
        }
        else if (enumModificationType.Prorogation == $("#selectModificationType").val()) {
            $("#inputCurrentFrom").prop('disabled', true);
            $("#inputCurrentTo").prop('disabled', false);
        }
        else if (enumModificationType.ModificationProrogation == $("#selectModificationType").val()) {
            $("#inputCurrentFrom").prop('disabled', false);
            $("#inputCurrentTo").prop('disabled', false);
        }
        else if ("" == $("#selectModificationType").val() || null == $("#selectModificationType").val()) {
            $("#inputCurrentFrom").prop('disabled', true);
            $("#inputCurrentTo").prop('disabled', true);
        }
        else {
            $("#inputCurrentFrom").prop('disabled', false);
            $("#inputCurrentTo").prop('disabled', false);
        }
        $('#inputDays').val(CalculateDays($('#inputCurrentFrom').val(), $('#inputCurrentTo').val()));
    }

    static LoadRecordObservation() {
        if (glbPolicy.EndorsementTexts != undefined && glbPolicy.EndorsementTexts.length > 0 || glbpolicyEndorsementTexts != null) {
            var recordText = "";
            ModificationRequest.GetModificationType().done(function (data) {
                if (data.success) {
                    glbPolicy.EndorsementTexts.forEach(function (item) {
                        var ModificationType = "";
                        data.result.forEach(function (itemData) {
                            if (itemData.Id == item.ModificationTypeId) {
                                ModificationType = "-" + itemData.Description;
                            }
                        });
                        recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + ModificationType + "\n" + item.Observations + "\n\r";
                    });
                }
                else {
                    glbPolicy.EndorsementTexts.forEach(function (item) {
                        recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
                    });
                }
                $('#inputRecord').val(recordText);
            });
        }
    }

    static PrintTemporal() {
        ModificationRequest.GenerateReportTemporary(glbPolicy.Endorsement.TemporalId, glbPolicy.Prefix.Id, glbPolicy.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }
}