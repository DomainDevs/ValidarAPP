var FinancingRate = "";
class UnderwritingPromissory extends Uif2.Page {
    getInitialState() {
        $("#inputQuota").OnlyDecimals(UnderwritingDecimal);
        $("#inputPercentage").OnlyDecimals(UnderwritingDecimal);
        $("#inputInputValueFinanceTwo").OnlyDecimals(UnderwritingDecimal);
        $("#linkPayment").on('click', false);
        $("#linkPaymentPlan").on('click', false);
        UnderwritingPromissory.Load();
        UnderwritingPaymentPlan.GetParameterId().done(function (data) {
            if (data.success) {
                FinancingRate = data.result;
            }
        });
    }

    bindEvents() {
        $("#inputPercentage").on('focusout', UnderwritingPromissory.CalculateValue);
    }
    static SavePremiumFinance() {

        //$("#formTexts").validate();
        var premiumFinance = UnderwritingPromissory.GetPremiumFinance();
        premiumFinance = {
            Id: $("#selectPaymentPlan").val(),
            Description: $("#selectPaymentPlan option[value='" + $("#selectPaymentPlan").val() + "']").text(),
            PremiumFinance: premiumFinance
        }
        //if ($("#formTexts").valid()) {
        PremiumFinance.SavePaymentPremiumFinance(glbPolicy.Id, premiumFinance).done(function (data) {
                if (data.success) {
                    glbPolicy.PaymentPlan.Quotas = null;
                    glbPolicy.PaymentPlan = data.result;
                    Underwriting.HidePanelsIssuance(MenuType.PaymentPlan);
                    Underwriting.LoadSubTitles(3);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSavePaymentPlan, 'autoclose': true });
            });
        //}
    }

    static GetPremiumFinance() {
        var objIndividual = {
            IndividualId: glbPolicy.Holder.IndividualId,
            InsuredCode: glbPolicy.Holder.InsuredId,
            FullName: glbPolicy.Holder.Name
        }
        var PremiumFinance = {
            FinanceRate: FinancingRate,
            StatePay: 1,
            CurrentFrom: $("#dateTofrom").val(),
            CurrentTo: $("#dateToUntil").val(),
            Insured: objIndividual,
            NumberQuotas: parseInt(($("#inputQuota").val())),
            PremiumValue: parseFloat(($("#inputTotalValue").val())),
            PercentagetoFinance: parseFloat(($("#inputPercentage").val())),
            FinanceValue: parseFloat(($("#inputValueFinance").val())),
            FinanceToValue: parseFloat(($("#inputInputValueFinanceTwo").val())),
            MinimumValueToFinance: parseFloat(($("#inputMinimumValue").val())) 
        };

        return PremiumFinance;
    }

    static CalculateValue() {

        var percentage = parseFloat(($("#inputPercentage").val())).toFixed(2);
        var TotalValue = parseFloat(($("#inputTotalValue").val())).toFixed(2);

        var ValueFinance = (percentage * TotalValue) / 100;
        
        $("#inputValueFinance").val(ValueFinance.toFixed(2));
        $("#inputInputValueFinanceTwo").val(ValueFinance.toFixed(2));
    }

    static Load() {
        if (glbPolicy.Holder != undefined) {
            $("#inputInsuredCode").val(glbPolicy.Holder.IndividualId);
            $("#inputInsuredName").val(glbPolicy.Holder.Name);
            $("#dateTofrom").val(glbPolicy.CurrentFrom);
            $("#dateToUntil").val(glbPolicy.CurrentTo);
            $("#inputStatePay").val('Inicial');
            $("#inputTotalValue").val(glbPolicy.Summary.FullPremium);
            

        }
        

    }



}