class RiskLiabilityRequest {
    static GetStateCityByDaneCode(DANECode, countryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/GetStateCityByDaneCode',
            data: JSON.stringify({ daneCode: DANECode, countryId: countryId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDaneCodeByCountryIdByStateIdByCityId(countryId, stateId, cityId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/GetDaneCodeByCountryIdByStateIdByCityId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId, cityId: cityId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetRiskById(id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/GetRiskById',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDaneCodebyCountryIdStateIdCityId(countryId, stateId, cityId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/GetDaneCodeByCountryIdByStateIdByCityId',
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

    static GetRatingZone(prefixId, country, state) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/GetRatingZonesByPrefixIdCountryIdStateId',
            data: JSON.stringify({ prefixId: prefixId, countryId: country, stateId: state }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SaveRisk(riskData, riskDataCoverages, dynamicProperties) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/SaveRisk',
            data: JSON.stringify({ riskModel: riskData, coverages: riskDataCoverages, dynamicProperties: dynamicProperties }),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteRisk(policyId, riskId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/DeleteRisk',
            data: JSON.stringify({ temporalId: policyId, riskId: riskId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskLiability/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static RunRules(PolicyId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/RunRulesRiskPreLiability',
            data: JSON.stringify({ policyId: PolicyId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPremium(riskData, riskDataCoverages, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetPremium',
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
            url: rootPath + 'Underwriting/RiskLiability/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
        });
    }

    static GetStatesByCountryId(id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetStatesByCountryId',
            data: JSON.stringify({ countryId: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
        });
    }

    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetCompanyRisksByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetRisksByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetRisksByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetCountries',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
        });
    }

    static GetCoveragesByProductIdGroupCoverageId(temporalId, productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetCoveragesByProductIdGroupCoverageId',
            data: JSON.stringify({ temporalId: temporalId, productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskActivitiesByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetRiskActivitiesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetSubActivityRisksByActivityRiskId(ActivityRiskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetSubActivityRisksByActivityRiskId',
            data: JSON.stringify({ ActivityRiskId: ActivityRiskId }),
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    };
    static GetAssuranceMode() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetAssuranceMode',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetNomenclaturesAll() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetNomenclatures',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}