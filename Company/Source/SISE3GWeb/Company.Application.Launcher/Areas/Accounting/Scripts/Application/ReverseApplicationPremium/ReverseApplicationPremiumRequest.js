class ReverseApplicationPremiumRequest {
    static GetBranches() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/ReverseApplicationPremium/GetBranches',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/ReverseApplicationPremium/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPoliciesByQuery(prefixId, branchId, query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/ReverseApplicationPremium/GetPoliciesByQuery',
            data: JSON.stringify({ prefixId: prefixId, branchId: branchId, query: query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetEndorsementsByPolicyFilter(policyFilter) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/ReverseApplicationPremium/GetEndorsementsByPolicyFilter',
            data: JSON.stringify({ policyFilter: policyFilter }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetApplicationPremium(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/ReverseApplicationPremium/GetApplicationPremium',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ValidatePremiumTemporal(premiumFilterDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/ReverseApplicationPremium/ValidatePremiumTemporal',
            data: JSON.stringify({ premiumFilterDTO: premiumFilterDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveTempPremiumReversion(reversionFilterDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/ReverseApplicationPremium/SaveTempPremiumReversion',
            data: JSON.stringify({ reversionFilterDTO: reversionFilterDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}