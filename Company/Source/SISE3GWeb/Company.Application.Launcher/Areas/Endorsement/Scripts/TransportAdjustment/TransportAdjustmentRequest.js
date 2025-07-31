
class TransportAdjustmentRequest {

static GetAdjustmentEndorsementByPolicyId(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetAdjustmentEndorsementByPolicyId',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetTransportsByPolicyId(policyId, currentFrom) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetTransportsByPolicyId',
            data: JSON.stringify({ policyId: policyId, currentFrom: currentFrom }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static CalculateDays(inputFrom, inputTo, billingPeriodId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/CalculateDays',
            data: JSON.stringify({ inputFrom: inputFrom, inputTo: inputTo, billingPeriodId: billingPeriodId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredObjectsByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetInsuredObjectsByRiskId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCompanyEndorsementByEndorsementTypeDeclration(policyId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetEndorsementByEndorsementTypeDeclarationPolicyId',
            data: JSON.stringify({ policyId: policyId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });

    }


    static CreateTemporal(adjustmentModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + glbPolicy.EndorsementController + '/CreateTemporal',
            dataType: 'json',
            data: JSON.stringify({ adjustmentModel: adjustmentModel }),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetTemporalById(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetTransportsByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateEndorsement() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportAdjustment/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id.trim(), policyNumber: glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrentSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}