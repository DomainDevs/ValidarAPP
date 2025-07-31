class AutomaticQuotaRequest {

    static GetDocumentTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetDocumentTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetAgentProgram() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetAgentProgram',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetUtility() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetUtility',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetIndicatorConcepts() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetIndicatorConcepts',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetReportListSisconc() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetReportListSisconc',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetRiskCenterList() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetRiskCenterList',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetRestrictiveList() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetRestrictiveList',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetPromissoryNoteSignature() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetPromissoryNoteSignature',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetGuaranteeStatus() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetGuaranteeStatus',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetGuaranteesTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetGuaranteesTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetUserAgenciesByAgentIdDescription(agentId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetUserAgenciesByAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEconomicActivitiesByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetEconomicActivitiesByDescription",
            data: JSON.stringify({
                description: description
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetScriptsAutocomplete(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "RulesScripts/Scripts/GetScriptsAutocomplete",
            data: JSON.stringify({
                Query: description
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetCountries',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetStatesByCountryId",
            data: JSON.stringify({ countryId: countryId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, StateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
    static GetParameters() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetParameters',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetAutomaticQuotaOperation(Id) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetAutomaticQuotaOperation',
            data: { Id: Id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static SaveAutomaticQuotaGeneralJSON(automatic, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/SaveAutomaticQuotaGeneralJSON',
            data: JSON.stringify({ AutomaticDTO: automatic, dynamicProperties: dynamicProperties }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveAutomaticQuotaGeneral(general, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/SaveAutomaticQuotaGeneral',
            data: JSON.stringify({ automaticQuotaDto: general, dynamicProperties: dynamicProperties }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPersonByDescriptionInsuredSearchTypeCustomerType(description) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetPersonByDescriptionInsuredSearchTypeCustomerType',
            data: { description: description },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
    static GetUserByName(name) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetUserByName',
            data: JSON.stringify({ name: name }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetUserSession() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetUserSession',
            data: JSON.stringify({}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveAutomaticQuotaThirdJSON(third) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/SaveAutomaticQuotaThirdJSON',
            data: JSON.stringify({ thirdDTO: third }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteCalculate(temp, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/ExecuteCalculate',
            data: JSON.stringify({ id: temp, dynamicProperties: dynamicProperties }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveAutomaticQuotaUtilityJSON(ListUtility, automatic) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/SaveAutomaticQuotaUtilityJSON',
            data: JSON.stringify({ listUtlityDTO: ListUtility, automaticDTO: automatic }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCountriesByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetCountriesByDescription",
            data: JSON.stringify({ Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetStatesByCountryIdByDescription(countryId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetStatesByCountryIdByDescription",
            data: JSON.stringify({ CountryId: countryId, Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetCitiesByCountryIdByStateIdByDescription',
            data: JSON.stringify({ CountryId: countryId, StateId: stateId, Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAutomaticQuotaDeserealizado(Id) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetAutomaticQuotaDeserealizado',
            data: { Id: Id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
    static GetAddressByIndividualIdCompany(individualId) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetAddressByIndividualIdCompany',
            data: { individualId: individualId },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
    static GetAgencyByIndividualId(individualId) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetAgencyByIndividualId',
            data: { individualId: individualId },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static CreateProspectPersonNatural(prospectData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/CreateProspectPersonNatural',
            data: JSON.stringify({ "prospectModel": prospectData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountryByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetCountryByCountryId",
            data: JSON.stringify({ countryId: countryId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetStatesByCountryIdByStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetStatesByCountryIdByStateId",
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCitiesByCountryIdByStateIdById(countryId, stateId, cityId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetCitiesByCountryIdByStateIdById",
            data: JSON.stringify({ countryId: countryId, stateId: stateId, cityId: cityId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAutomaticQuota(Id) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AutomaticQuota/AutomaticQuota/GetAutomaticQuota',
            data: { Id: Id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetPersonTypes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + "AutomaticQuota/AutomaticQuota/GetPersonTypes",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: false
        });
    }
}
