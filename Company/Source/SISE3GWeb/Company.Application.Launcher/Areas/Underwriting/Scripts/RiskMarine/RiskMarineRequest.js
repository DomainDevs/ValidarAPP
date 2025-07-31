class RiskMarineRequest {

    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetMarineTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetMarineTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPolicyType(prefixId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetPolicyType',
            data: JSON.stringify({PrefixId: prefixId, Id: id}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetInsuredObjectsByProductIdGroupCoverageId',
            data: JSON.stringify({ProductId: productId , GroupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPolicyType(prefixId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetPolicyType',
            data: JSON.stringify({ PrefixId: prefixId, Id: id}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCitiesByStateIdByCountryId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetCitiesByStateIdByCountryId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveRisk(temporalId, riskModel, dynamicPropetties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/SaveRisk',
            data: JSON.stringify({ temporalId: temporalId, riskModel: riskModel, dynamicPropetties: dynamicPropetties }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static RunRules(policyId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/RunRules',
            data: JSON.stringify({ policyId: policyId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskById(id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetRiskById',
            data: JSON.stringify({ id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static DeleteRisk(temporalId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/DeleteRisk',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    } 
    static GetTextsByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetTextsByRiskId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetUsesMarine(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetUsesMarine',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}