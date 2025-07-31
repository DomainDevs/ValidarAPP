//Codigo de la pagina PaymentPlan.cshtml
var prvPaymentSchedules = [];
var FinancingRate = "";
class UnderwritingPaymentPlan extends Uif2.Page {
    getInitialState() {
        UnderwritingPaymentPlan.GetParameterId().done(function (data) {
            if (data.success) {
                FinancingRate = data.result;
            }
        });

        UnderwritingPaymentPlan.LoadPaymentSchedules();
    }

    //Seccion Eventos
    bindEvents() {
        $("#btnPaymentPlan").on('click', this.PaymentPlanLoad);
        $("#btnPaymentPlanSave").on("click", this.PaymentPlanSave);
        $('#selectPaymentPlan').on('itemSelected', this.ChangePaymentPlan);
    }

    static LoadPaymentSchedules() {
        UnderwritingPaymentPlan.GetPaymentSchedule().done(function (data) {
            if (data.success) {
                prvPaymentSchedules = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': "Error consulta de datos plan de pagos", 'autoclose': true });
        });
    }

    static GetPaymentSchedule() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetPaymentSchedules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    PaymentPlanLoad() {
        if (glbPolicy.Id == 0 && glbPolicy.TemporalType != TemporalType.Quotation) {
            if ($("#formUnderwriting").valid()) {
                Underwriting.SaveTemporalPartial(MenuType.PaymentPlan); 
                //$.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemp, 'autoclose': true });
            }
        }
        else {
            UnderwritingPaymentPlan.LoadPaymentPlan();
        }
    }

    PaymentPlanSave() {
        if ($("#selectPaymentPlan").val() > 0) {
            
            if (prvPaymentSchedules.indexOf(parseInt($("#selectPaymentPlan").val())) >= 0) {
                    if (UnderwritingPaymentPlan.ValidatePaymentPlan()) {
                        UnderwritingPromissory.SavePremiumFinance();
                    }
                } else {
                    UnderwritingPaymentPlan.SavePaymentPlan();
                }
                if (glbPolicy.TemporalType == TemporalType.Quotation) {
                    Underwriting.SaveTemporal(false);
                }
             
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectPaymentPlan, 'autoclose': true });
        }
    }

    ChangePaymentPlan(event, selectedItem) {
         $('#Total').text(0);
        if (selectedItem.Id > 0) {
            UnderwritingPaymentPlan.GetQuotasbyPaymentPlanId(selectedItem.Id);
        }
        else {
            $('#tableQuotas').UifDataTable('clear');
        }
    }

    static LoadPaymentPlan() {

       
        $("#formUnderwriting").validate();

        if (glbPolicy.Id > 0 && $("#formUnderwriting").valid()) {
            Underwriting.ShowPanelsIssuance(MenuType.PaymentPlan);

            if (glbPolicy.PaymentPlan != null) {
                UnderwritingPaymentPlan.GetPaymentPlanByProductId($("#selectProduct").val(), glbPolicy.PaymentPlan.Id);
                if (glbPolicy.PaymentPlan.Quotas != null) {
                    UnderwritingPaymentPlan.LoadQuotas(glbPolicy.PaymentPlan.Quotas);
                }

                if (glbPolicy.PaymentPlan.PremiumFinance != null && glbPolicy.PaymentPlan.PremiumFinance.NumberQuotas > 0) {
                    $("#inputInsuredCode").val(glbPolicy.Holder.IndividualId);
                    $("#inputInsuredName").val(glbPolicy.Holder.Name);
                    $("#dateTofrom").val(FormatDate(glbPolicy.CurrentFrom));
                    $("#dateToUntil").val(FormatDate(glbPolicy.CurrentTo));
                    $("#inputStatePay").val('Inicial');
                    //$("#inputFinancingRate").val(glbPolicy.PaymentPlan.PremiumFinance.FinanceRate),
                    //$("#inputStatePay").val(policyData.StatePay == 1 ? '' :'' ),
                    //$("#ValidSince").val(glbPolicy.PaymentPlan.PremiumFinance.ValidSince),
                    //$("#inputFinancingRate").val('19.25 %'); // Se debe traer el valor de la tabla parametrica.
                    $("#inputFinancingRate").val(FinancingRate + ' %'); 
                    $("#inputTotalValue").val(glbPolicy.PaymentPlan.PremiumFinance.PremiumValue)
                    $("#inputInputValueFinanceTwo").val(glbPolicy.PaymentPlan.PremiumFinance.FinanceToValue),
                    $("#inputValueFinance").val(glbPolicy.PaymentPlan.PremiumFinance.FinanceValue),
                    $("#inputMinimumValue").val(glbPolicy.PaymentPlan.PremiumFinance.MinimumValueToFinance),
                    $("#inputQuota").val(glbPolicy.PaymentPlan.PremiumFinance.NumberQuotas),
                    $("#inputPercentage").val(glbPolicy.PaymentPlan.PremiumFinance.PercentagetoFinance)
                    $("#inputTotalValue").val(glbPolicy.Summary.FullPremium);

                    
                } else {
                    $("#inputInsuredCode").val(glbPolicy.Holder.IndividualId);
                    $("#inputInsuredName").val(glbPolicy.Holder.Name);
                    $("#dateTofrom").val(FormatDate(glbPolicy.CurrentFrom));
                    $("#dateToUntil").val(FormatDate(glbPolicy.CurrentTo));
                    $("#inputStatePay").val('Inicial');
                    $("#inputTotalValue").val(glbPolicy.Summary.FullPremium);
                    //$("#inputFinancingRate").val('19.25 %'); // Se debe traer el valor de la tabla parametrica.
                    $("#inputFinancingRate").val(FinancingRate + ' %'); 
                    $("#inputQuota").val('0');
                    $("#inputPercentage").val('0');
                    $("#inputInputValueFinanceTwo").val('0');

                    
                }
                UnderwritingPaymentPlan.ConfigurationClass(glbPolicy.PaymentPlan.Id.toString());
            }
            else {
                UnderwritingPaymentPlan.GetPaymentPlanIdByProductId($("#selectProduct").val())
            }
        }
    }

    static GetPaymentPlanIdByProductId(productId) {
        var selectPaymenPlanId = 0;
        PaymentPlanRequest.GetPaymentPlanIdByProductId(productId).done(function (data) {
            if (data.success) {
                if (data.result.length >= 1) {
                    selectPaymenPlanId = data.result[0].Id;
                }
                UnderwritingPaymentPlan.GetPaymentPlanByProductId($("#selectProduct").val(), selectPaymenPlanId);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchPaymentPlan, 'autoclose': true });
        });
    }

    static GetPaymentPlanByProductId(productId, selectedId) {
        PaymentPlanRequest.GetPaymentPlanByProductId(productId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectPaymentPlan").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectPaymentPlan").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
        });        
    }

    static GetQuotasbyPaymentPlanId(paymentPlanId) {

        if (glbPolicy.Summary != null) {
            if (prvPaymentSchedules.indexOf(parseInt(paymentPlanId)) >= 0) {
                UnderwritingPaymentPlan.ConfigurationClass(paymentPlanId)
            } else {
                UnderwritingPaymentPlan.ConfigurationClass(paymentPlanId)
                    PaymentPlanRequest.GetQuotasbyPaymentPlanId(paymentPlanId, glbPolicy.Summary, $("#inputFrom").val(), $("#inputTo").val(), $('#inputIssueDate').text()).done(function (data) {
                        if (data.success) {
                            UnderwritingPaymentPlan.LoadQuotas(data.result);
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetQuotas, 'autoclose': true });
                    });
                }
        }
        else {
            $('#tableQuotas').UifDataTable('clear');
        }
    }

    static ConfigurationClass(paymentPlanId) {
        if (prvPaymentSchedules.indexOf(parseInt(paymentPlanId)) >= 0) {
            $("#tabPayment").removeClass("active");
            $("#tabPremiumFinance").addClass("active");
            $("#tabContentPayment").removeClass("active");
            $("#tabContentFinance").addClass("active");
            $("#tabPayment").css({ 'optionPayment': "none" });
            $("#tabPremiumFinance").css({ 'optionPayment': "block" });
            UnderwritingPaymentPlan.CalculatePercentage(paymentPlanId);           
        } else {
            $("#tabPremiumFinance").removeClass("active");
            $("#tabPayment").addClass("active");
            $("#tabContentFinance").removeClass("active");
            $("#tabContentPayment").addClass("active");
            $("#tabPremiumFinance").on('click', false);
        }
    }

    static CalculatePercentage(PlanId){
        switch (PlanId) { //quitar todo esto
            case EnumRateTypePrev.PreviCredit20.toString():
                $("#inputPercentage").val('70');
                $("#inputPercentage").prop('disabled', true);
                UnderwritingPromissory.CalculateValue();
                break;
            case EnumRateTypePrev.PreviCredit10.toString():
                $("#inputPercentage").val('70');
                $("#inputPercentage").prop('disabled', true);
                UnderwritingPromissory.CalculateValue();
                break;
            case EnumRateTypePrev.PreviCredit30.toString():
                $("#inputPercentage").val('70');
                $("#inputPercentage").prop('disabled', true);
                UnderwritingPromissory.CalculateValue();
                break;
            case EnumRateTypePrev.PremiumFinancing.toString():
                $("#inputPercentage").val('0');
                $("#inputQuota").val('0');
                $("#inputInputValueFinanceTwo").val('0');
                $("#inputPercentage").prop('disabled', false);
                break;
            default:
                break;
        }
    }

    static LoadQuotas(quotas) {
        $('#tableQuotas').UifDataTable('clear');
        if (quotas != null) {
            var totalAmount = 0;
            var paymentPlans = [];
            $.each(quotas, function (key, value) {
                totalAmount += this.Amount;
                paymentPlans.push({
                    Number: this.Number,
                    Detail: '<strong>'+AppResources.DueDate + ':</strong> ' + FormatDate(this.ExpirationDate) + '<br/><strong>' + AppResources.Percentage + ':</strong> ' + this.Percentage + '% <br/><strong> ' + AppResources.Quota + ':</strong> ' +FormatMoney(this.Amount),
                    ExpirationDate: FormatDate(this.ExpirationDate),
                    Amount: (this.Amount),
                    Percentage: (this.Percentage)
                });
            });

            if (paymentPlans.length > 0) {
                $('#tableQuotas').UifDataTable('addRow', paymentPlans);
            }
            $('#Total').text(totalAmount);
            
        }
    }

    static SavePaymentPlan() {


        var quotasValues = $("#tableQuotas").UifDataTable('getData');
        var paymentPlan = { Id: $("#selectPaymentPlan").val(), Description: $("#selectPaymentPlan option[value='" + $("#selectPaymentPlan").val() + "']").text() }
            PaymentPlanRequest.SavePaymentPlan(glbPolicy.Id, paymentPlan, quotasValues).done(function (data) {
                if (data.success) {
                    glbPolicy.PaymentPlan.PremiumFinance = null;
                    Underwriting.HidePanelsIssuance(MenuType.PaymentPlan);
                    glbPolicy.PaymentPlan = data.result;
                    Underwriting.LoadSubTitles(3);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSavePaymentPlan, 'autoclose': true });
                }
                
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSavePaymentPlan, 'autoclose': true });
            });
    }

    static ValidatePaymentPlan() {

        var result = true;

        if ($("#inputQuota").val() == "0" || $("#inputQuota").val() == "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveQuote, 'autoclose': true });
            result = false;
        }

        return result;
    }

    static GetParameterId() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetExtendedParameterByParameterId',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}