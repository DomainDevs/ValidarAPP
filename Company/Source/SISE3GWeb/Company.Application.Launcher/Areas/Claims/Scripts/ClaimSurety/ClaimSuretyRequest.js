class ClaimSuretyRequest {
    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimSurety/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetPaymentMovementTypesByPaymentSourceId(sourceId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Charge/GetPaymentMovementTypesByPaymentSourceId',
            data: JSON.stringify({ sourceId: sourceId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByEndorsementIdPrefixId(endorsementId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSurety/GetRisksByEndorsementIdPrefixId',
            data: JSON.stringify({ endorsementId: endorsementId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteClaimOperations(claimSuretyDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSurety/ExecuteClaimOperations',
            data: JSON.stringify({
                claimSuretyDTO: claimSuretyDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}