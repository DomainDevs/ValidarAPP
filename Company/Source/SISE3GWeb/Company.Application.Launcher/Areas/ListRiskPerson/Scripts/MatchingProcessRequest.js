class MatchingProcessRequest {
    static GetPersonRiskList(searchValue) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/MatchingProcess/GetPersonRiskList',
            data: JSON.stringify({ SearchValue: searchValue }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static StartCompleteProcess(searchValue) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/MatchingProcess/StartCompleteProcess',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchProcess(searchValue) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/MatchingProcess/SearchProcess',
            dataType: "json",
            data: JSON.stringify({ SearchValue: searchValue }),
            contentType: "application/json; charset=utf-8"
        });
    }
}