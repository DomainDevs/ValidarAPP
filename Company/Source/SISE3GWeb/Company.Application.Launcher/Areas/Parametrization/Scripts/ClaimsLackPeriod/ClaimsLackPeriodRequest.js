class ClaimsLackPeriodRequest {
    
    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsLackPeriod/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCausesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsLackPeriod/GetCausesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCauseCoveragesByCauseId(causeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsLackPeriod/GetCauseCoveragesByCauseId',
            data: JSON.stringify({ causeId: causeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
 
  
}
