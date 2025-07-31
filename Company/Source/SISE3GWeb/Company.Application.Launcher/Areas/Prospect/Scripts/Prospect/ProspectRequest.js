class ProspectRequest {
    static GetDocumentType(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetDocumentType",
            data: JSON.stringify({
                typeDocument: typeDocument
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDaneCodeByCountryIdByStateIdByCityId(countryId, stateId, cityId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetDaneCodeByCountryIdByStateIdByCityId",
            data: JSON.stringify({
                countryId: countryId,
                stateId: stateId,
                cityId: cityId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCountriesByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetCountriesByDescription",
            data: JSON.stringify({ Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetStatesByCountryIdByDescription(countryId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetStatesByCountryIdByDescription",
            data: JSON.stringify({ CountryId: countryId, Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Prospect/Prospect/GetCitiesByCountryIdByStateIdByDescription',
            data: JSON.stringify({ CountryId: countryId, StateId: stateId, Description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetCountryAndStateAndCityByDaneCode",
            data: JSON.stringify({ CountryId: countryId, DaneCode: daneCode }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetMaritalStatus() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetMaritalStatus",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGenderTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetGenderTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAddressesTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Prospect/Prospect/GetAddressesTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
class Prospect {

    static CreateProspectPersonNatural(prospectData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Prospect/Prospect/CreateProspectPersonNatural',
            data: JSON.stringify({ "prospectModel": prospectData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateProspectPersonLegal(prospectData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Prospect/Prospect/CreateProspectPersonLegal',
            data: JSON.stringify({ "prospectLegal": prospectData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}