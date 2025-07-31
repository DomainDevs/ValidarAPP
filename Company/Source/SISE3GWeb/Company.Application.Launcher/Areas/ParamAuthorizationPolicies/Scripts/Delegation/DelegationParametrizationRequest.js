class DelegationParametrizationRequest {

    static GetModules() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'ParamAuthorizationPolicies/Delegation/GetModules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetSubModules() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'ParamAuthorizationPolicies/Delegation/GetSubModules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetHierarchies() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'ParamAuthorizationPolicies/Delegation/GetHierarchies',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDelegationByName(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "ParamAuthorizationPolicies/Delegation/GetListDelegation",
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubModuleItems(idModule) {
        return $.ajax({
            type: "POST",
            url: rootPath + "ParamAuthorizationPolicies/Delegation/GetSubModulesForItem",
            data: JSON.stringify({ idModule: idModule, }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}