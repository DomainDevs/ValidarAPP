var policy = {};
class PaymentPlan extends Uif2.Page {
    getInitialState() {
        PaymentPlan.GetCurrentSummary();
        PaymentPlan.LoadPaymentPlan();
        GetDateIssue();
    }
    bindEvents() {
        $('#selectPaymentPlan').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                PaymentPlan.GetQuotasbyPaymentPlanId(selectedItem.Id);
            }
            else {
                $('#tableQuotas').UifDataTable('clear');
            }
        });
    }
    static LoadPaymentPlan() {
        if (policy != null) {
            PaymentPlan.GetPaymentPlanIdByProductId(policy.Product.Id);
            PaymentPlan.GetQuotasbyPaymentPlanId($("#selectPaymentPlan").UifSelect("getSelected"));
        }
    }
    static GetCurrentSummary() {
        PaymentPlanRequest.GetCurrentSummary($("#hiddenEndorsementId").val()).done(function (data)
        {
            if (data.success) {
                policy = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }
    static GetPaymentPlanIdByProductId(productId) {
        var selectPaymenPlanId = 0;
        PaymentPlanRequest.GetPaymentPlanIdByProductId(productId).done(function (data) {
            if (data.success) {
                if (data.result.length >= 1) {
                    selectPaymenPlanId = data.result[0].Id;
                }
                PaymentPlan.GetPaymentPlanByProductId(policy.Product.Id, selectPaymenPlanId);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryPaymentPlan, 'autoclose': true });
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
        if (policy.Summary != null) {
            PaymentPlanRequest.GetQuotasbyPaymentPlanId(paymentPlanId, policy.Summary, FormatDate(policy.CurrentFrom), FormatDate(policy.CurrentTo), FormatFullDate(policy.IssueDate)).done(function (data) {
                if (data.success) {
                    PaymentPlan.LoadQuotas(data.result);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorCalculateQuotas, 'autoclose': true });
            });
        }
        else {
            $('#tableQuotas').UifDataTable('clear');
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
                    Detail: FormatDate(this.ExpirationDate) + '<br/>' + this.Percentage + '% <br/>' + FormatMoney(this.Amount),
                    ExpirationDate: FormatDate(this.ExpirationDate),
                    Amount: (this.Amount),
                    Percentage: (this.Percentage)
                });
            });
            if (paymentPlans.length > 0) {
                $('#tableQuotas').UifDataTable('addRow', paymentPlans);
            }
        }
    }
}

(function () {
    new PaymentPlan();
}());