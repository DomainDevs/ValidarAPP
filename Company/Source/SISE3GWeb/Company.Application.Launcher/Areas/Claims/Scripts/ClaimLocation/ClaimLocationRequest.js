class ClaimLocationRequest {
    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimLocation/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetRisksByEndorsementId(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimLocation/GetRisksByEndorsementId',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteClaimOperations(claimLocationDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimLocation/ExecuteClaimOperations',
            data: JSON.stringify({
                claimLocationDTO: claimLocationDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}