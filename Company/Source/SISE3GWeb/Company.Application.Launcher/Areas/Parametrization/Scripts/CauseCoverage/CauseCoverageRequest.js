class CauseCoverageRequest {
    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLinesBusinessByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetLinesBusinessByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubLinesBusinessByLineBusinessId(lineBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetSubLinesBusinessByLineBusinessId',
            data: JSON.stringify({ lineBusinessId: lineBusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCausesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetCausesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBussinessId, subLineBussinessId, causeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId',
            data: JSON.stringify({ lineBussinessId: lineBussinessId, subLineBussinessId: subLineBussinessId, causeId: causeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCauseCoveragesByCauseId(causeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/GetCauseCoveragesByCauseId',
            data: JSON.stringify({causeId: causeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateCauseCoverage(causeId, coverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/CreateCauseCoverage',
            data: JSON.stringify({ causeId: causeId, coverageDTO: coverageDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteCauseCoverage(causeId, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/CauseCoverage/DeleteCauseCoverage',
            data: JSON.stringify({ causeId: causeId, coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}