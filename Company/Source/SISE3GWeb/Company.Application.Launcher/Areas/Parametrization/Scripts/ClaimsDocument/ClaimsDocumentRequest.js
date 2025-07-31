class ClaimsDocumentRequest {
    
    static GetCompanyModule(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/ClaimsDocument/GetCompanyModule",
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCompanySubModule(moduleId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/ClaimsDocument/GetCompanySubModule",
            data: JSON.stringify({ moduleId: moduleId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/ClaimsDocument/GetPrefixes",
            data: JSON.stringify({}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteDocumentOperatios(claimsDocumentationDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsDocument/ExecuteDocumentOperatios',
            data: JSON.stringify({ "claimsDocumentationDTO": claimsDocumentationDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteDocumentation(Id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsDocument/DeleteDocumentation',
            data: JSON.stringify({ "Id": Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    } 


    static GetDocumentBySubmoduleId(SubmoduleId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ClaimsDocument/GetDocumentBySubmoduleId',
            data: JSON.stringify({ "SubmoduleId": SubmoduleId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    } 
}