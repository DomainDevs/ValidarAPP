$.ajaxSetup({ async: true });
class TaxRequests {
    static GetRateTypeTax() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetRateTypeTax',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetRateTypeAdditionalTax() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetRateTypeAdditionalTax',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetDefaultCountry() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Tax/GetDefaultCountry',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetBaseConditionTax() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetBaseConditionTax',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetBaseTaxWithholding() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetBaseTaxWithHolding',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetFeesApplies() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetFeesApplies',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetRoles() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetRoles',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static SaveTax(taxModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/SaveTax',
            data: JSON.stringify({ taxViewModel: taxModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetTaxByDescription(description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetTaxByDescription',
            data: JSON.stringify({ Description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });

    }
    static GetTaxByIdAndDescription(taxId, taxDescription) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetTaxByIdAndDescription',
            data: JSON.stringify({ taxId: taxId, taxDescription: taxDescription  }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });

    }
    static GoToRateTaxPartialView(taxViewModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/RateTax',
            data: JSON.stringify({ TaxViewModel: taxViewModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GenerateFileToExport(taxId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GenerateFileToExport',
            data: JSON.stringify({ taxId: taxId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}