class GuaranteeRequest {
    static GetInsuredGuaranteeByIndividualId(individualId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Guarantee/GetInsuredGuaranteeByIndividualId",
            data: JSON.stringify({
                individualId: individualId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetInsuredGuaranteeByIndividualIdById(individualId, id, typeId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Guarantee/GetInsuredGuaranteeByIndividualIdById",
            data: JSON.stringify({
                individualId: individualId,
                id: id,
                typeId: typeId
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetGuarantorByIndividualIdById(individualId, id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Guarantee/GetGuarantorByIndividualIdById",
            data: JSON.stringify({
                individualId: individualId,
                guaranteeId: id
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static CreateGuarantor(guarantors) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Guarantee/CreateGuarantor",
            data: JSON.stringify({
                guarantorDTOs: guarantors
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    static GetStatesByCountry(countryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Person/Guarantee/GetStatesByCountryId",
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
            url: rootPath + "Person/Guarantee/GetCountriesByDescription",
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
            url: rootPath + "Person/Guarantee/GetStatesByCountryIdByDescription",
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
            url: rootPath + "Person/Guarantee/GetCitiesByCountryIdByStateIdByDescription",
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
            url: rootPath + "Person/Guarantee/GetCountries",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveInsuredGuarantee(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/SaveInsuredGuarantee",
            data: JSON.stringify({
                guarantee: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsuredGuaranteeMortgage(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/CreateInsuredGuaranteeMortgage",
            data: JSON.stringify({
                insuredGuaranteeMortgage: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsuredGuaranteePledge(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/CreateInsuredGuaranteePledge",
            data: JSON.stringify({
                insuredGuaranteePledge: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsuredGuaranteePromissoryNote(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/CreateInsuredGuaranteePromissoryNote",
            data: JSON.stringify({
                insuredGuaranteePromissoryNote: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsuredGuaranteeFixedTermDeposit(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/CreateInsuredGuaranteeFixedTermDeposit",
            data: JSON.stringify({
                insuredGuaranteePFixedTermDeposit: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsuredGuaranteeOthers(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/CreateInsuredGuaranteeOthers",
            data: JSON.stringify({
                insuredGuaranteeOthers: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGuarantees() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/GetGuarantees",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDocumentationReceived(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/GetDocumentationReceivedByGuaranteeId",
            data: JSON.stringify({
                guaranteeId: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDocumentationReceivedByIndividualAndGuaranteeId(individualId, guaranteeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/GetDocumentationReceivedByIndividualAndGuaranteeId",
            data: JSON.stringify({
                individualId: individualId,
                guaranteeId: guaranteeId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }



    static CreateInsuredGuaranteeLog(insuredGuaranteeLog) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/CreateInsuredGuaranteeLog",
            data: JSON.stringify({
                insuredGuaranteeLog: insuredGuaranteeLog
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateInsuredGuaranteeDocumentation(data) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/CreateInsuredGuaranteeDocumentation",
            data: JSON.stringify({
                insuredGuaranteeDocumentations: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static CreateInsuredGuaranteePrefix(prefixAssociateds) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Guarantee/SaveGuaranteePrefixAssocieted',
            data: JSON.stringify({ prefixAssociateds: prefixAssociateds }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixAssociated(id) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/GetPrefixAssociated",
            data: JSON.stringify({
                coveredRiskType: id
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredGuaranteeLog(individualId, guaranteeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/GetInsuredGuaranteeLog",
            data: JSON.stringify({
                individualId: individualId, guaranteeId: guaranteeId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAplicationInsuredGuaranteePrefixByindividualIdByguaranteeId(individualId, guaranteeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Guarantee/GetAplicationInsuredGuaranteePrefixByindividualIdByguaranteeId",
            data: JSON.stringify({
                individualId: individualId,
                guaranteeId: guaranteeId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}