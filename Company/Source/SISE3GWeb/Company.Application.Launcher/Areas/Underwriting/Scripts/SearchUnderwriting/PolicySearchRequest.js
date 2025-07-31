
class PolicySearchRequest {

    static SearchPolicies(subscriptionSearchViewModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/SearchPolicies',
            data: JSON.stringify({ subscriptionSearchViewModel: subscriptionSearchViewModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBranchs() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/GetBranchs',
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