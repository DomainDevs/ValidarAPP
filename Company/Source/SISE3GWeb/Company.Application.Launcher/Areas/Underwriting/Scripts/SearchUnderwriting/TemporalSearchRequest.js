
class TemporalSearchRequest {

    static SearchTemporaries(subscriptionSearchViewModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/SearchUnderwriting/SearchTemporaries',
            data: JSON.stringify({ subscriptionSearchViewModel: subscriptionSearchViewModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}