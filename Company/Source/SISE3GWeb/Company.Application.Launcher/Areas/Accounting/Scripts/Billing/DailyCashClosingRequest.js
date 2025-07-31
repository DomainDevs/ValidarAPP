class DailyCashClosingRequest {
    static ValidateCheckCardDeposited(branch, currency, registerDate) {
        return $.ajax({
            type: "POST",
            url: ACC_ROOT + "Billing/ValidateCheckCardDeposited",
            data: { "branchId": branch, "currencyId": currency, "registerDate": registerDate }
        });
    }

    static ValidateCashDeposited(branch, currency, registerDate) {
        return $.ajax({
            type: "POST",
            url: ACC_ROOT + 'Billing/ValidateCashDeposited',
            data: { "branchId": branch, "currencyId": currency, "registerDate": registerDate }
        });
    }

    static GetCashierByBranchId(branchId) {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + "Billing/GetCashierByBranchId?branchId=" + branchId
        });
    }

    static GenerateCollectItemsExcel(collectControlId) {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + "Billing/GenerateCollectItemsExcel?collectControlId=" + collectControlId
        });
    }

    
}