class ClaimsPersonCompanyRequest {
    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLinesBusinessByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetLinesBusinessByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubLinesBusinessByLineBusinessId(lineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetSubLinesBusinessByLineBusinessId',
            data: JSON.stringify({ lineBusinessId: lineBusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCausesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetCausesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBussinessId, subLineBussinessId, causeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId',
            data: JSON.stringify({ lineBussinessId: lineBussinessId, subLineBussinessId: subLineBussinessId, causeId: causeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCauseCoveragesByCauseId(causeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetCauseCoveragesByCauseId',
            data: JSON.stringify({ causeId: causeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateCauseCoverage(causeId, coverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/CreateCauseCoverage',
            data: JSON.stringify({ causeId: causeId, coverageDTO: coverageDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteCauseCoverage(causeId, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/DeleteCauseCoverage',
            data: JSON.stringify({ causeId: causeId, coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    
    static GetCoveragesByLineBusinessId(lineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetCoveragesByLineBusinessId',
            data: JSON.stringify({lineBusinessId: lineBusinessId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    static GetEstimationsType() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsPersonCompany/GetEstimationsType',
            data: JSON.stringify({ }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    static GetPaymentConcept() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/ClaimsPersonCompany/GetPaymentConcept",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}