class RiskAircraftRequest {

    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPolicyType(prefixId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetPolicyType',
            data: JSON.stringify({ PrefixId: prefixId, Id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetInsuredObjectsByProductIdGroupCoverageId',
            data: JSON.stringify({ ProductId: productId, GroupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPolicyType(prefixId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetPolicyType',
            data: JSON.stringify({ PrefixId: prefixId, Id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveRisk(temporalId, riskModel, dynamicPropetties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/SaveRisk',
            data: JSON.stringify({ temporalId: temporalId, riskModel: riskModel, dynamicPropetties: dynamicPropetties }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static RunRules(policyId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/RunRules',
            data: JSON.stringify({ policyId: policyId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskById(id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetRiskById',
            data: JSON.stringify({ id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static DeleteRisk(temporalId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/DeleteRisk',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTextsByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetTextsByRiskId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetComboboxes(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetComboboxes',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetModels(makeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetModels',
            data: JSON.stringify({ makeId: makeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}