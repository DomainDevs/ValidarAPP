class NoticeLocationRequest {
    static GetCountries() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetCountries',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetStatesByCountryId',
            data: JSON.stringify({ countryId: countryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, StateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyInfoByLocation(policyId, documentNumber, EndorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/GetPolicyInfo',
            data: JSON.stringify({ "policyId": policyId, "policyNumber": documentNumber, "endorsementId": "" }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByInsuredId(insuredId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeLocation/GetRisksByInsuredId',
            data: JSON.stringify({ "insuredId": insuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteNoticeOperations(noticeLocationDTO, contactInformationDTO, locationDTO, noticeCoverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeLocation/ExecuteNoticeOperations',
            data: JSON.stringify({
                "noticeLocationDTO": noticeLocationDTO,
                "contactInformationDTO": contactInformationDTO,
                "locationDTO": locationDTO,
                "noticeCoverageDTO": noticeCoverageDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskLocationByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeLocation/GetRiskLocationByRiskId',
            data: JSON.stringify({ "riskId": riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskLocationByClaimNoticeId(claimNoticeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeLocation/GetRiskLocationByClaimNoticeId',
            data: JSON.stringify({ "claimNoticeId": claimNoticeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskLocationByAddress(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeLocation/GetRisksLocationByAddress',
            data: JSON.stringify({ "query": query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}