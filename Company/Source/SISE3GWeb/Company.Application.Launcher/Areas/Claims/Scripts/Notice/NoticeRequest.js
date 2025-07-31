class NoticeRequest {
    static GetEstimationsType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Notice/GetEstimationsType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetDocumentTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Notice/GetDocumentTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetCountries',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetClaimBranchesbyUserId() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Notice/GetClaimBranchesbyUserId',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetStatesByCountryId',
            data: JSON.stringify({ countryId: countryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetBranches() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetBranches',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimSearch/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetInsuredsByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/GetInsuredsByIndividualId',
            data: JSON.stringify({ individualId: individualId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetNoticeByNoticeId(noticeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/GetNoticeByNoticeId',
            data: JSON.stringify({ "noticeId": noticeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimsByPolicyId(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/GetClaimsByPolicyId',
            data: JSON.stringify({ policyId: parseInt(policyId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteNoticeCoverageByCoverage(noticeId, coverageId, individualId, estimateTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/DeleteNoticeCoverageByCoverage',
            data: JSON.stringify({ noticeId: parseInt(noticeId), coverageId: parseInt(coverageId), individualId: parseInt(individualId), estimateTypeId: parseInt(estimateTypeId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
            
    static GetDefaultCountry() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Notice/GetDefaultCountry',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"            
        });
    }
    
}