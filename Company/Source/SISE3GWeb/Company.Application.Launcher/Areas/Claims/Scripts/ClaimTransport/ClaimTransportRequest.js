class ClaimTransportRequest {
    static GetRisksByEndorsementId(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimTransport/GetRisksByEndorsementId',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    };

    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimTransport/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    };

    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimTransport/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    };

    static ExecuteClaimOperations(claimTransportDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimTransport/ExecuteClaimOperations',
            data: JSON.stringify({
                claimTransportDTO: claimTransportDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    };
}