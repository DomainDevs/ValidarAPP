class AgentMovementsRequest {

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
    static GetAccoutingCompanies() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/Common/GetAccountingCompanies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAccountingAccountConceptByBranchId(branchUserDefault) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetAccountingAccountConceptByBranchId?branchId=" + branchUserDefault + "&sourceId=1",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetNatures(branchUserDefault) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetNatures",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCurrencies(branchUserDefault) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetCurrencies",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}