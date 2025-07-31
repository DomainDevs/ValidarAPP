class AuthorizationPersonRiskListRequest {

    static GetDocumentType(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/AuthorizationPersonRiskList/GetDocumentType',
            data: JSON.stringify({ typeDocument: typeDocument }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSarlaftEventGroup() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AuthorizationPolicies/AuthorizationPersonRiskList/GetSarlaftEventGroup',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEventAuthorizationsByUserId(objAuthorizationRiskListModelView) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/AuthorizationPersonRiskList/GetEventAuthorizationsByUserId',
            data: JSON.stringify({ authorizationRiskListModelView: objAuthorizationRiskListModelView}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static AuthorizeRiskList(authorizeRiskListViewModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/AuthorizationPersonRiskList/AuthorizeRiskListOperation',
            data: JSON.stringify({ authorizationRiskListModelView: authorizeRiskListViewModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}