class ChangeTermRequest {
    static CreateTemporal(changeTermModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + glbPolicy.EndorsementController + '/CreateTemporal',
            data: JSON.stringify({ changeTermModel: changeTermModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateEndorsementChangeTerm(changeTermModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + glbPolicy.EndorsementController + '/CreateEndorsementChangeTerm',
            data: JSON.stringify({ changeTermModel: changeTermModel }),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateReportTemporary(tempId, prefixId, operationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GenerateReportTemporary',
            data: JSON.stringify({ temporaryId: tempId, prefixId: prefixId, riskSince: 0, riskUntil: 0, operationId: operationId, tempAuthorization: false }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
