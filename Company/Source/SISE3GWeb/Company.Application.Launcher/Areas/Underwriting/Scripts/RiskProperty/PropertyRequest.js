class PropertyRequest {
    static inputDaneCodeItemSelectedFocusout(inputDaneCode, selectCountry) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetStateCityByDaneCode",
            data: JSON.stringify({
                daneCode: inputDaneCode,
                countryId: selectCountry
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskById(temporalId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetRiskById',
            data: JSON.stringify({ temporalId: temporalId, id: id }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveRisk(temporalId, riskModel, insuredObjects, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/SaveRisk',
            data: JSON.stringify({ temporalId: temporalId, riskModel: riskModel, insuredObjects: insuredObjects, dynamicProperties: dynamicProperties }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static DeleteRisk(temporalId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/DeleteRisk',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
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
    static GetInsuredObjectsByTemporalIdRiskId(temporalId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetInsuredObjectsByTemporalIdRiskId',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId }),
            dataType: "json",
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
    static DeleteInsuredObject(temporalId, riskId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/DeleteInsuredObjectByRiskIdInsuredObjectId',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRatingZone(prefixId, countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Riskproperty/GetRatingZonesByPrefixIdCountryIdStateId',
            data: JSON.stringify({ prefixId: prefixId, countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static RunRules(policyId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/RunRulesRisk',
            data: JSON.stringify({ policyId: policyId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPremium(riskModel, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetPremium',
            data: JSON.stringify({ riskModel: riskModel, dynamicProperties: dynamicProperties }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static getDaneCode(countryId, stateId, cityId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetDaneCodeByCountryIdByStateIdByCityId",
            data: JSON.stringify({
                countryId: countryId,
                stateId: stateId,
                cityId: cityId
            }),
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
    static SelectCountryItemSelected(countryParameterProperty) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetStatesByCountryId",
            data: JSON.stringify({ countryId: countryParameterProperty }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetStatesByCountryId(selectCountry) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetStatesByCountryId",
            data: JSON.stringify({ countryId: selectCountry }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCitiesByCountryIdStateId(selectCountry, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCitiesByCountryIdStateId",
            data: JSON.stringify({ countryId: selectCountry, stateId: stateId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetGroupCoverages",
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetRiskTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCountries() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCountries",
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetSubActivityRisksByActivityRiskId(ActivityRiskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetSubActivityRisksByActivityRiskId',
            data: JSON.stringify({ ActivityRiskId: ActivityRiskId }),
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    };

    static GetRisksByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetRisksByTemporalId",
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectsByProductIdGroupCoverageId(allInsuredObject, riskModel, isSelected) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetInsuredObjectsByProductIdGroupCoverageId",
            data: JSON.stringify({
                allInsuredObject: allInsuredObject,
                riskModel: riskModel,
                isSelected: isSelected
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskActivitiesByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetRiskActivitiesByProductId",
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCompanyRisksByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetCompanyRisksByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectsByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetInsuredsObjectsByProductIdGroupCoverageId",
            data: JSON.stringify({
                productId: productId,
                groupCoverageId: groupCoverageId,
                prefixId: prefixId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRouteTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetRouteTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSuffixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetSuffixes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetApartmentsOrOffices() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetApartmentsOrOffices",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDaneCodeByQuery() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetDaneCodeByQuery",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetSubActivityRisksByActivityRiskId(ActivityRiskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetSubActivityRisksByActivityRiskId',
            data: JSON.stringify({ ActivityRiskId: ActivityRiskId }),
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAssuranceMode() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetAssuranceMode',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTransformAddress(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTransformAddress',
            data: JSON.stringify({ query: query }),
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetNomenclaturesAll() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetNomenclatures',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}