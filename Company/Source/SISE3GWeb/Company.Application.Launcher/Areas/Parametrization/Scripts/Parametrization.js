class SubBranch{
    static GetSubLinesBusinessByLineBusinessId() {
        return $.ajax({
            type: 'POST',
            url: 'GetSubLinesBusinessByLineBusinessId',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class LineBussiness {
    static GetLinesBusiness() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Parametrization/Parametrization/GetLinesBusiness",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class Prefix {
    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: 'GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}


class Protection {
    static GetProtectionsAll() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Parametrization/Parametrization/GetProtectionsAll",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}
