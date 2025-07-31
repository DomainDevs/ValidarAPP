///Clase donde se deben crear todas las peticiones ajax del modulo
//Peticiones ajax vista principal

class CancellationRequest {

    static GetCurrentSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

  
    static GetTemporalById(endorsementController, temporalId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/GetTemporalById',
            data: JSON.stringify({ id: temporalId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateTemporal(endorsementController, cancellationModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateTemporal',
            data: JSON.stringify({ cancelationViewModel: cancellationModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateEndorsement(endorsementController) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id, policyNumber: glbPolicy.DocumentNumber}),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetEndorsementReasonsByCancellationType(route ,cancellationType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + route +'/GetEndorsementReasonsByCancellationType?cancellationType=' + cancellationType,
            dataType: "json",
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