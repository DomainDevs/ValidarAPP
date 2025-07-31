class SarlaftRequest {


    static GetEconomicActivities(description) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetEconomicActivities',
            data: { query: description },
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteEvent(tempId) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/ExecuteEvents',
            data: { tempId: tempId },
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPersonTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetPersonTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetBranch() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetSignatureBranch',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTypeDocument(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetDocumentType',
            data: JSON.stringify({ typeDocument: typeDocument }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetUserByUserId() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetUserByUserId',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveSarlaft(customerKnowledgeDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/SaveSarlaft',
            data: JSON.stringify({ customerKnowledgeDTO: customerKnowledgeDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPersonByDocumentNumberAndSearchType(documentNum, searchType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetPersonByDocumentNumberAndSearchTypeList',
            data: JSON.stringify({ documentNum: documentNum, searchType: searchType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetSarlaft(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetSarlaft',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLastSarlaftId(gblPerson) {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + 'Sarlaft/Sarlaft/GetLastSarlaftId',
            data: JSON.stringify({ person: gblPerson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSarlaftFormNumber(sarObject) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetSarlaftFormNumber',
            data: JSON.stringify({ sarlaft: sarObject }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSarlaftBySarlaftId() {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + 'Sarlaft/Sarlaft/GetSarlaftBySarlaftId',
            data: JSON.stringify({ sarlaftId: sarlaftId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetSarlaftExonerationByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetSarlaftExonerationByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCountry() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetCountry',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetState(CountryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetState',
            data: JSON.stringify({ CountryId: CountryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCities(CountryId, StateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetCity',
            data: JSON.stringify({ CountryId: CountryId, StateId: StateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetOperationTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetOperationType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetProductType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetProductType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCurrencies() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetCurrency',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInternationalOperationsBySarlaftId(sarlaftId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetInternationalOperationsBySarlaftId',
            data: JSON.stringify({ sarlaftId: sarlaftId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ExecuteOperation(internationalOperation) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/ExecuteOperation',
            data: JSON.stringify({ internationalOperationDTO: internationalOperation }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetLegalRepresentByIndividualId(individualId, sarlaftId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetLegalRepresentByIndividualId',
            data: JSON.stringify({ individualId: individualId, sarlaftId: sarlaftId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPartnersByIndividualId(individualId, sarlaftId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetPartnersByIndividualId',
            data: JSON.stringify({ individualId: individualId, sarlaftId: sarlaftId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPepsByIndividualId(individualId, sarlaftId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetPepsByIndividualId',
            data: JSON.stringify({ individualId: individualId, sarlaftId: sarlaftId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveLegalRepresent(representLegalTmp) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/SaveLegalRepresent',
            data: JSON.stringify({ legalRepresentative: representLegalTmp }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SavePartner(partner) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/SavePartner',
            data: JSON.stringify({ partner: partner }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInterviewResult() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetInterwievResult',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetLinkTypeResult() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetLinkType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRelationship() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Sarlaft/Sarlaft/GetRelationship',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetIndividualLinksByIndividualId(individualId, sarlaftId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetIndividualLinksByIndividualId',
            data: JSON.stringify({ individualId: individualId, sarlaftId: sarlaftId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ExecuteOperationLink(links) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/ExecuteOperationLink',
            data: JSON.stringify({ linkDTOs: links }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetValidationListPartners(listPartners) {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + 'Sarlaft/Sarlaft/ValidateListPartners',
            data: JSON.stringify({ partners: listPartners }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetValidationListRepresentPartners(representant ,listPartners) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/ValidationListRepresentPartners',
            data: JSON.stringify({ representant: representant, partners: listPartners }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidationAccessAndHierarchysByUser() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/ValidationAccessAndHierarchysByUser',
            data:{ },
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DisablePolicies(authorizationRequests) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/Authorize/DisablePolicies',
            data: JSON.stringify({ authorizationRequests: authorizationRequests }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadInitialLegalData(typeDocument) {
       return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/LoadInitialLegalData",
            data: JSON.stringify({
                typeDocument: typeDocument
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetTypeRolByIndividual(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Sarlaft/Sarlaft/GetTypeRolByIndividual",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadInitialData(isEmail) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/LoadInitialData",
            data: JSON.stringify({
                isEmail: isEmail
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: false
        });
    }


    static GetCountries() {
        return $.ajax({
            type: 'GET',
            url: rootPath + "Sarlaft/Sarlaft/GetCountrie",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStates(idCountry) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetStatesByCountry",
            data: JSON.stringify({ idCountry: idCountry }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCities(countryId, stateId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetCitiesByCountryIdStateId",
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDefaultCountry() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetDefaultCountry",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRoles() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetRoles",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCategoria() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetCategoria",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetConsanguinidad() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetConsanguinidad",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetRelacion() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetRelacion",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }


    static SaveExoneration(sarlaftExonerationtDTO) {
        return $.ajax({
            type: "POST",
            async: false, 
            url: rootPath + 'Sarlaft/Sarlaft/SaveExoneration',
            data: JSON.stringify({ sarlaftExonerationtDTO: sarlaftExonerationtDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetOppositor() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetOppositor",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSociety() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetSociety",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetNationality() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Sarlaft/Sarlaft/GetNationality",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInterviewManagerByDescription(InterviewManager) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Sarlaft/Sarlaft/GetInterviewManagerByDescription',
            data: JSON.stringify({ InterviewManager: InterviewManager }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    

}