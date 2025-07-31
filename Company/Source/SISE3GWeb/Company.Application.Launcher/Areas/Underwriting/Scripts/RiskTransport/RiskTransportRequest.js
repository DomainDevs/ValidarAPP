class RiskTransportRequest {

    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetIndividualDetailsByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetLeapYear() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetLeapYear',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCargoTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCargoTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPackagingTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetPackagingTypes',           
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTransportTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetTransportTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyType(prefixId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetPolicyType',
            data: JSON.stringify({ PrefixId: prefixId, Id: id }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCountries',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetStateByCountryId',
            data: JSON.stringify({ countryId: countryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }



    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetInsuredObjectsByProductIdGroupCoverageId',
            data: JSON.stringify({ ProductId: productId, GroupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetViaTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetViaTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDeclarationPeriods() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetDeclarationPeriods',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBillingPeriods() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetBillingPeriods',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetCitiesByStateIdByCountryId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCitiesByStateIdByCountryId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveRisk(temporalId, riskModel, dynamicPropetties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/SaveRisk',
            data: JSON.stringify({ temporalId: temporalId, riskModel: riskModel, dynamicPropetties: dynamicPropetties }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static RunRules(policyId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/RunRules',
            data: JSON.stringify({ policyId: policyId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskById(id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetRiskById',
            data: JSON.stringify({ id: id }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteRisk(temporalId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/DeleteRisk',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTextsByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetTextsByRiskId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHolderTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Underwriting/RiskTransport/GetHolderTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}