class GuaranteeRequest {
    static GetInsuredGuaranteeByIndividualId(individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Guarantees/Guarantees/GetInsuredGuaranteeByIndividualId",
            data: JSON.stringify({
                individualId: individualId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetStatesByCountry(countryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Guarantees/Guarantees/GetStatesByCountryId",
            data: JSON.stringify({
                countryId: countryId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetParameterByDescription(description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Person/GetParameterByDescription",
            data: JSON.stringify({
                Description: description
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetCountriesByDescription(description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Guarantees/Guarantees/GetCountriesByDescription",
            data: JSON.stringify({
                Description: description
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }
    static GetStatesByCountryIdByDescription(countryId, description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Guarantees/Guarantees/GetStatesByCountryIdByDescription",
            data: JSON.stringify({
                CountryId: countryId,
                Description: description
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Guarantees/Guarantees/GetCitiesByCountryIdByStateIdByDescription",
            data: JSON.stringify({
                CountryId: countryId,
                StateId: stateId,
                Description: description
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Guarantees/Guarantees/GetCountries",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static SaveInsuredGuarantee(data) {

        try {
            return $.ajax({
                type: "POST",
                url: rootPath + "Guarantees/Guarantees/SaveInsuredGuarantee",
                data: JSON.stringify({
                    guarantee: data
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            });
        } catch (err) {
            var msg = err.message;
        }

    }

    static GetGuarantees() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Guarantees/Guarantees/GetGuarantees",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDocumentationReceived(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Guarantees/Guarantees/GetDocumentationReceivedByGuaranteeId",
            data: JSON.stringify({
                guaranteeId: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Guarantees/Guarantees/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetPrefixAssociated(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Guarantees/Guarantees/GetPrefixAssociated",
            data: JSON.stringify({
                coveredRiskType: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredGuaranteeLogs(individualId, guaranteeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Guarantees/Guarantees/GetInsuredGuaranteeLogs",
            data: JSON.stringify({
                individualId: individualId, guaranteeId: guaranteeId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGuaranteesStatusByUserId() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Guarantees/Guarantees/GetGuaranteesStatusByUserId",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGuaranteesRoutesByGuaranteeStatusId(guaranteeStatusId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Guarantees/Guarantees/GetGuaranteesRoutesByGuaranteeStatusId",
            data: JSON.stringify({
                guaranteeStatusId: guaranteeStatusId
            }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
}