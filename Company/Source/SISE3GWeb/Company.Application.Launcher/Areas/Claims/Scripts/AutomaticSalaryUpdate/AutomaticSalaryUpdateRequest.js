class AutomaticSalaryUpdateRequest {

    static GetBranches() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/AutomaticSalaryUpdate/GetBranches',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/AutomaticSalaryUpdate/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetMinimumSalaryByYear(year) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetMinimumSalaryByYear',
            data: JSON.stringify({ year: year }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchClaimsBySalaryEstimation(oClaimModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/AutomaticSalaryUpdate/SearchClaimsBySalaryEstimation',
            data: JSON.stringify(oClaimModel),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateEstimationsSalaries(subClaims) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/AutomaticSalaryUpdate/UpdateEstimationsSalaries',
            data: JSON.stringify({ subClaimsDTO: subClaims }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}
