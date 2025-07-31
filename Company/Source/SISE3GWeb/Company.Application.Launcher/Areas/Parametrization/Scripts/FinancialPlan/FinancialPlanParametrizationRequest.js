class FinancialPlanParametrizationRequest {
    static GetPaymentPlan() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/FinancialPlan/GetPaymentPlans',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPaymentMethod() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/FinancialPlan/GetPaymentMethods',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetCurrencies() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/FinancialPlan/GetCurrencies',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetFinancialPlanForItems(idPaymentPlan,idPaymentMethod,idCurrency) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/FinancialPlan/GetFinancialPlanForItems",
            data: JSON.stringify({ idPaymentPlan: idPaymentPlan, idPaymentMethod: idPaymentMethod, idCurrency: idCurrency }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}