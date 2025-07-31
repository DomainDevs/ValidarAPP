class ProductionReportRequest
{
    static GetBranches() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetBranches',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetPrefixes',
            data: JSON.stringify({ }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSalePointsByBranchId(branchId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetSalePointsByBranchId',
            data: JSON.stringify({ branchId: branchId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProductsByAgentIdPrefixId(agentId, prefixId, isCollective) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetProductsByAgentIdPrefixId',
            data: JSON.stringify({ agentId: agentId, prefixId: prefixId, isCollective: isCollective }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetUserAgenciesByAgentIdDescription(agentId, description)
    {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetUserAgenciesByAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetUserAgenciesByAgentId(agentId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetUserAgenciesByAgentId',
            data: JSON.stringify({ agentId: agentId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExistProductAgentByAgentIdPrefixIdProductId(agentId, prefixId, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/ExistProductAgentByAgentIdPrefixIdProductId',
            data: JSON.stringify({ agentId: agentId, prefixId: prefixId, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixesByAgentIdAgentsRequest(agentId)
    {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/ProductionReport/GetPrefixesByAgentIdAgents',
            data: JSON.stringify({ agentId: agentId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetBillingGroupByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Massive/RequestGrouping/GetBillingGroupByDescription',
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgencyByAgentIdAgencyId(agentId, agencyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetAgencyByAgentIdAgencyId',
            data: JSON.stringify({ agentId: agentId, agencyId: agencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetFileProductionReport(agentId, branchId,  prefixId, productId, inputFrom, inputTo) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ProductionReport/GetFileProductionReport',
            data: JSON.stringify({ agentId: agentId, branchId: branchId,  prefixId: prefixId, productId: productId, inputFrom: inputFrom, inputTo: inputTo  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}