class RiskFidelityRequest {
    static GetStateCityByDaneCode(DANECode, countryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/GetStateCityByDaneCode',
            data: JSON.stringify({ daneCode: DANECode, countryId: countryId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDaneCodeByCountryIdByStateIdByCityId(countryId, stateId, cityId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/GetDaneCodeByCountryIdByStateIdByCityId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId, cityId: cityId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetRiskById(id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/GetRiskById',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        })
    }

    static GetDaneCodebyCountryIdStateIdCityId(countryId, stateId, cityId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/GetDaneCodeByCountryIdByStateIdByCityId',
            data: JSON.stringify({
                countryId: countryId,
                stateId: stateId,
                cityId: cityId
            }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetInsuredsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetIndividualDetailsByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetRatingZone(prefixId,country, state) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/GetRatingZonesByPrefixIdCountryIdStateId',
            data: JSON.stringify({ prefixId: prefixId, countryId: country, stateId: state }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SaveRisk(riskData, riskDataCoverages, dynamicProperties) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/SaveRisk',
            data: JSON.stringify({ riskModel: riskData, coverages: riskDataCoverages, dynamicProperties: dynamicProperties }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteRisk(policyId, riskId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/DeleteRisk',
            data: JSON.stringify({ temporalId: policyId, riskId:riskId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskFidelity/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static RunRules(PolicyId,ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/RunRulesRiskPreFidelity',
            data: JSON.stringify({ policyId: PolicyId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPremium(riskData, riskDataCoverages, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetPremium',
            data: JSON.stringify({ riskModel: riskData, coverages: riskDataCoverages, dynamicProperties: dynamicProperties }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdatePolicyComponents(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/UpdatePolicyComponents',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatesByCountryId(id) {
        return $.ajax({
            type: "POST", 
            url: rootPath + 'Underwriting/RiskFidelity/GetStatesByCountryId',
            data: JSON.stringify({ countryId: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetRisksByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetRisksByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetCountries',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByProductIdGroupCoverageId(temporalId, productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetCoveragesByProductIdGroupCoverageId',
            data: JSON.stringify({ temporalId: temporalId,productId: productId, groupCoverageId: groupCoverageId,prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskActivitiesByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetRiskActivitiesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetOccupations() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskFidelity/GetOccupations',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetModuleDateIssueSynk() {
        return $.ajax({
            type: "POST",
            async: "false",
            url: rootPath + 'Underwriting/Underwriting/GetModuleDateIssue',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}