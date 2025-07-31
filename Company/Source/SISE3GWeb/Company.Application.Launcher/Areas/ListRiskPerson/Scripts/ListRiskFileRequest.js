class ListRiskFileRequest {
    static GetListRisk() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GetListRisk',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateLoadListRiskFile(listRiskDTO) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'ListRiskPerson/ListRiskFile/CreateLoadListRiskFile',
            data: JSON.stringify({ listRiskLoadDTO: listRiskDTO }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetProcessStatusById(processId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'ListRiskPerson/ListRiskFile/GetProcessStatusById',
            data: JSON.stringify({ processId: processId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetErrorExcelProcessListRisk(loadProcessId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'ListRiskPerson/ListRiskFile/GetErrorExcelProcessListRisk',
            data: JSON.stringify({ loadProcessId: loadProcessId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SetInitialProcessFile(listRiskProcessId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'ListRiskPerson/ListRiskFile/SetInitialProcessFile',
            data: JSON.stringify({ listRiskProcessId: listRiskProcessId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GenerateListRiskRequest() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GenerateListRiskRequest',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}