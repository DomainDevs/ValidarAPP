class RiskSuretyCrossGuaranteesRequest {

    static CreateInsuredGuarantee(insured) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateInsuredConsortium',
            data: JSON.stringify({ "insured": insured }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredGuaranteeByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetInsuredGuaranteeByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveGuarantees(riskId, guarantees) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/SaveGuarantees',
            data: JSON.stringify({ riskId: riskId, guarantees: guarantees }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredGuaranteeRelationPolicy(guaranteeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetInsuredGuaranteeRelationPolicy',
            data: JSON.stringify({ guaranteeId: guaranteeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}