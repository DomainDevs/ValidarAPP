class PaymentRequestClaimsMovementsRequest {

    static GetPersonTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/Common/GetPersonTypes',
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
    static GetSalesPointByBranchId(branchUserDefault) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/Common/GetSalesPointByBranchId?branchId=' + branchUserDefault,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAccountingCompanies() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetAccountingCompanies",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetPrefixes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRequestTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetRequestTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
   
}