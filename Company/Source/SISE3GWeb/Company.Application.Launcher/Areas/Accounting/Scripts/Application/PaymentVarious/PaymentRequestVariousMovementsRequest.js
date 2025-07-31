class PaymentRequestVariousMovementsRequest {

    static GetPersonTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/Common/GetPersonTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAccountingCompanies() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/Common/GetAccountingCompanies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetBranchs() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/Common/GetBranchs',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    // Ajuste Jira SMT-1559 Inicio
    static GetSearchPaymentRequestVarious(searchBy, individualId, branchId, requestNumber, voucherNumber, dateFrom, dateTo) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/PaymentVarious/SearchPaymentRequestVarious',
            data: JSON.stringify({ searchBy: searchBy, individualId: individualId, branchId: branchId, requestNumber: requestNumber, voucherNumber: voucherNumber, dateFrom: dateFrom, dateTo: dateTo}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    // Ajuste Jira SMT-1559 Fin


    static RequestVariousMovements(imputationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/PaymentVarious/GetTempPaymentRequestItems',
            data: JSON.stringify({ imputationId: imputationId, isPaymentVarious:'true'}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
}