
class QuotationSearchRequest {

    static SearchQuotations(subscriptionSearchViewModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/SearchQuotations',
            data: JSON.stringify({ subscriptionSearchViewModel: subscriptionSearchViewModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }    
}