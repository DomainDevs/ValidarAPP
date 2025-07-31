class AuthorizationSarlaftOperationRequest {
    static GetSarlaftEventGroup() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AuthorizationPolicies/AuthorizationSarlaftOperation/GetSarlaftEventGroup',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchSuspectOperations(authorizationSuspectOperationViewModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/AuthorizationSarlaftOperation/SearchSuspectOperations',
            data: JSON.stringify({ authorizationSuspectOperationViewModel: authorizationSuspectOperationViewModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAuthorizationReasons(EventGroupId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/AuthorizationSarlaftOperation/GetAuthorizationReasons',
            data: JSON.stringify({ EventGroupId: EventGroupId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static AuthorizeSuspectOperation(authorizeSuspectOperationViewModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/AuthorizationSarlaftOperation/AuthorizeSuspectOperation',
            data: JSON.stringify({ authorizationSuspectOperationModelView: authorizeSuspectOperationViewModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}