class ClaimFidelityRequest {
    static GetRisksByEndorsementId(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimFidelity/GetRisksByEndorsementId',
            data: JSON.stringify({ "endorsementId": endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimFidelity/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static ExecuteClaimOperations(claimFidelityDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimFidelity/ExecuteClaimOperations',
            data: JSON.stringify({
                "claimFidelityDTO": claimFidelityDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}