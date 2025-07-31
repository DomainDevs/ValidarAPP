class ClaimsSubCauseRequest {

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsSubCause/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCausesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsSubCause/GetCausesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubCausesByCause(CauseId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsSubCause/GetSubCausesByCauseId',
            data: JSON.stringify({ "causeId": CauseId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteSubCauseOperations(subcause) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsSubCause/ExecuteSubCauseOperations',
            data: JSON.stringify({ "subCause": subcause }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteSubCause(subCauseId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsSubCause/DeleteSubCause',
            data: JSON.stringify({ "subCauseId": subCauseId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
