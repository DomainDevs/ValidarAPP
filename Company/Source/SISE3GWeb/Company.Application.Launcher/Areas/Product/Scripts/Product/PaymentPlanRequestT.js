class PaymentPlanRequestT {
    static ajaxGetFinancialPlanByProductId(productId) {     
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetFinancialPlanByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadPaymenPlan(currencies) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetPaymentSchudeleByCurrencies',
            data: JSON.stringify({ currencies }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

 

}