$.ajaxSetup({ async: false });
//Codigo de la pagina PaymentPlan.cshtml
$(document).ready(function () {
    $("#selectPaymentPlan").attr("disabled", "disabled");
    $("#btnPaymentPlan").on('click', function () {
        if (policy.Id > 0) {
            LoadPaymentPlan();
        }
    });

    $("#btnPaymentPlanClose").on("click", function () {
        
        HidePanelsSearch();
        ShowPanelsSearch(MenuType.Search);
    });
});

function LoadPaymentPlan() {
    
    
    if (policy.Id > 0) {
        HidePanelsSearch();
        ShowPanelsSearch(MenuType.PaymentPlan);
        if (policy.PaymentPlan != null) {
            GetPaymentPlanByProductId(policy.Product.Id, policy.PaymentPlan.Id);
            if (policy.PaymentPlan.Quotas != null) {
                LoadQuotas(policy.PaymentPlan.Quotas);
            }
        }
    }
}

function GetPaymentPlanByProductId(productId, selectedId) {    
    PaymentPlanRequest.GetPaymentPlanByProductId(productId).done(function(data) {
        if (data.success) {
            if (selectedId == 0) {
                $("#selectPaymentPlan").UifSelect({ sourceData: data.result });
            }
            else {
                $("#selectPaymentPlan").UifSelect({ sourceData: data.result, selectedId: selectedId });
            }
            $("#selectPaymentPlan option:eq(0)").text(" ");
            $("#selectPaymentPlan").attr("disabled", "disabled");
        }
    });     
}

function LoadQuotas(quotas) {
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
