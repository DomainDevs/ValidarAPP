$.ajaxSetup({ async: true });
class RateTaxRequests {
    static GetTaxConditions() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetTaxConditions',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetCoveragesByLinesBusinessId(lineBusinessId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetCoveragesByLinesBusinessId',
            data: JSON.stringify({ lineBusinessId: lineBusinessId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetCoverages() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetCoverages',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetTaxCategories() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetTaxCategories',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetCities() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetCities',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetStates() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetStates',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetStatesByCountry(CountryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetStatesByCountry',
            data: JSON.stringify({ CountryId: CountryId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetCountries() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetCountries',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetLinesBusiness() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetLinesBusiness',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetBranches() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetEconomicActivities() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetEconomicActivities',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
    static SaveTaxRate(taxRateModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/SaveTaxRate',
            data: JSON.stringify({ taxRateViewModel: taxRateModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

}