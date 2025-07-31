
class SubscriptionSearchRequest {

    static GetUserAgenciesByAgentIdDescription(agentId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/GetUserAgenciesByAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }    

    static GetUserAgenciesByAgentId(agentId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/GetUserAgenciesByAgentId',
            data: JSON.stringify({ agentId: agentId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetUsersByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/GetUsersByDescription',
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHolders(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/GetHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSearchHolders(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/GetSearchHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GenerateFileToExport(searchType, subscriptionSearchViewModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/SearchUnderwriting/GenerateFileToExport',
            data: JSON.stringify({ searchType: searchType, subscriptionSearchViewModel: subscriptionSearchViewModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static CreateNewVersionQuotation(operationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/CreateNewVersionQuotation',
            data: JSON.stringify({ operationId: operationId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}