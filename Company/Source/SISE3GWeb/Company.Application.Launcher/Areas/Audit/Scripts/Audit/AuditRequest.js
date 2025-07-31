class AuditRequest {
    static GetAudit(auditModelView) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Audit/Audit/GetAudit',
            data: JSON.stringify({ auditModelView: auditModelView }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPackages() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Audit/Audit/GetPackages',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTypeTransaction() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Audit/Audit/GetTypeTransaction',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetUserByAccountName(user) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Audit/Audit/GetUserByAccountName',
            data: JSON.stringify({ user: user }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetObjectName(object) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Audit/Audit/GetObjectName',
            data: JSON.stringify({ object: object }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    //Exportar excel
    static GenerateFileToExport(auditModelView) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Audit/Audit/GenerateFileToExport',
            data: JSON.stringify({ auditModelView: auditModelView }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetEntityByQuery(query, idPackage) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Audit/Audit/GetEntityByQuery',
            data: JSON.stringify({ query: query, idPackage: idPackage }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}