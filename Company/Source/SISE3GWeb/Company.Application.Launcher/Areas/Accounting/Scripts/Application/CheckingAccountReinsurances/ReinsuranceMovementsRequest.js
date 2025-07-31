class ReinsuraceMovementsRequest {

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
    static GetAccountingAccountConceptByBranchId(branchUserDefault) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetAccountingAccountConceptByBranchId?branchId=" + branchUserDefault + "&sourceId=3",
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
    static GetTechnicalPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetTechnicalPrefixes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTechnicalSubPrefixesByPrefixId(idPrefix) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetTechnicalSubPrefixesByPrefixId?prefixId=" + idPrefix,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetContractTypeEnabled() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Reinsurance/Parameter/GetContractTypeEnabled",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetContractStretchs(contractId) {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetContractStretchs?contractId=" + contractId,
            dataType: "json",
            displayRecords: 5,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetNatures() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetNatures",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCurrencies() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetCurrencies",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetYearMonths() {
        return $.ajax({
            type: "GET",
            url: rootPath + "Accounting/Common/GetYearMonths",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEndorsementIdByPolicyEndorsement(state) {
        return $.ajax({
            type: "POST",
            url: rootPath + "CheckingAccountReinsurances/GetEndorsementIdByPolicyEndorsement",
            dataType: "json",
            data: {
                "policyNumber": state.viewModel.selectedReinsurancePolicy,
                "endorsementNumber": state.viewModel.selectedReinsuranceEndorsement,
                "branchId": state.viewModel.selectedReinsurancePolicyBranchId,
                "prefixId": state.viewModel.selectedReinsurancePolicyPrefixId
            }
        });
    }
}