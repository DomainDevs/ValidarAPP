class ClaimAirCraftRequest {
    static GetRisksByEndorsementIdPrefixId(endorsementId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimAirCraft/GetRisksByEndorsementIdPrefixId',
            data: JSON.stringify({ "endorsementId": endorsementId, "prefixId": prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimAirCraft/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimAirCraft/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteClaimOperations(claimAirCraftDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimAirCraft/ExecuteClaimOperations',
            data: JSON.stringify({
                "claimAirCraftDTO": claimAirCraftDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}