class ReversionRequest {

    static GetSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateTemporal(hiddenEndorsementController, reversionModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + hiddenEndorsementController + '/CreateTemporal',
            data: JSON.stringify({ reversionModel: reversionModel}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
     static GetReversionReasons() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Endorsement/Reversion/GetReversionReasons',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


}