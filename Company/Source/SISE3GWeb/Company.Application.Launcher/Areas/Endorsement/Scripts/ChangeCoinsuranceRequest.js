
class ChangeCoinsuranceRequest {

    static CreateTemporal(endorsementController, ChangeCoinsuranceViewModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateTemporal',
            data: JSON.stringify({ ChangeCoinsuranceViewModel: ChangeCoinsuranceViewModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateEndorsementChangeCoinsurance(endorsementController, ChangeCoinsuranceViewModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateEndorsementChangeCoinsurance',
            data: JSON.stringify({ ChangeCoinsuranceViewModel: ChangeCoinsuranceViewModel }),
            async: true,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetTemporalById(endorsementController, temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + endorsementController + '/GetTemporalById',
            data: JSON.stringify({ id: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
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
