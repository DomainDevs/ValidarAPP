class ChangeConsolidationRequest {

    static GetRiskByPolicyId(policyId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ContractObject/GetRiskByPolicyId',
            data: JSON.stringify({ policyId: policyId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHolders(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Endorsement/ChangeConsolidation/GetHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateTemporal(endorsementController, ChangeConsolidationModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateTemporal',
            data: JSON.stringify({ ChangeConsolidationViewModel: ChangeConsolidationModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateEndorsementChangeConsolidation(endorsementController, changeConsolidationModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateEndorsementChangeConsolidation',
            data: JSON.stringify({ changeConsolidationViewModel: changeConsolidationModel }),
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

    static GetRiskTemporalById(endorsementController, temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + endorsementController + '/GetRiskTemporalById',
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