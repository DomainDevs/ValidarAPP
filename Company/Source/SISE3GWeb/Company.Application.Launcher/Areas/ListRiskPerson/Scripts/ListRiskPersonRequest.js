class ListRiskPersonRequest {
    static GetDocumentType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GetDocumentType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetListRisk() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GetListRisk',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static CreateListRiskPerson(listRiskModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/CreateListRiskPerson',
            data: JSON.stringify({ "listRisk": listRiskModel}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetListRiskPerson(advancedModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GetListRiskPerson',
            data: JSON.stringify({ DocumentNumber: advancedModel.DocumentNumber, Name: advancedModel.Name, Surname: advancedModel.Surname, NickName: advancedModel.NickName, ListRiskId: advancedModel.ListRisk }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetListRiskPersonByIndexKey(documentNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GetListRiskPersonByIndexKey',
            data: JSON.stringify({ DocumentNumber: documentNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAssignedListMantenance(documentNumber, listRisk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GetAssignedListMantenance',
            data: JSON.stringify({ DocumentNumber: documentNumber, ListRisk: listRisk }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetListRiskTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ListRiskPerson/ListRiskPerson/GetListRiskTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}