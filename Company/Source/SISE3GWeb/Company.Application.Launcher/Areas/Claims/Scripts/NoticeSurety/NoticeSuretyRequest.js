class NoticeSuretyRequest {
    static GetBranches() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetBranches',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimSearch/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteNoticeOperations(noticeSuretyDTO, contactInformationDTO, suretyDTO, noticeCoverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeSurety/ExecuteNoticeOperations',
            data: JSON.stringify(
                {
                    noticeSuretyDTO: noticeSuretyDTO,
                    contactInformationDTO: contactInformationDTO,
                    suretyDTO: suretyDTO,
                    noticeCoverageDTO: noticeCoverageDTO
                }
            ),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByInsuredId(insuredId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeSurety/GetRisksByInsuredId',
            data: JSON.stringify({ insuredId: insuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksBySuretyIdPrefixId(suretyId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeSurety/GetRisksBySuretyIdPrefixId',
            data: JSON.stringify({ suretyId: suretyId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskSuretyByClaimNoticeId(claimNoticeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeSurety/GetRiskSuretyByClaimNoticeId',
            data: JSON.stringify({ "claimNoticeId": claimNoticeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskSuretyByRiskIdPrefixId(riskId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeSurety/GetRiskSuretyByRiskIdPrefixId',
            data: JSON.stringify({ riskId: riskId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksSuretyByInsured(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeSurety/GetSuretiesByDescription',
            data: JSON.stringify({ "query": query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}