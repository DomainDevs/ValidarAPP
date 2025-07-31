class OperationQuotaCumulusRequest {

    static GetOperatingQuotaEventByIndividualIdByLineBusinessId(IndividualId, LinebusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetOperatingQuotaEventByIndividualIdByLineBusinessId',
            data: JSON.stringify({ "IndividualId": IndividualId, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetOperatingQuotaEventByIndividualIdByLineBusinessIdTempRisk(IndividualId, LinebusinessId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetOperatingQuotaEventByIndividualIdByLineBusinessId',
            data: JSON.stringify({ "IndividualId": IndividualId, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(IndividualId, LinebusinessId,endoso,Id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId',
			data: JSON.stringify({ "IndividualId": IndividualId, "LinebusinessId": LinebusinessId, "Endorsement": endoso, "Id": Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(IndividualId, LinebusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId',
            data: JSON.stringify({ "IndividualId": IndividualId, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessIdTempRisk(IndividualId, LinebusinessId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId',
            data: JSON.stringify({ "IndividualId": IndividualId, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(IndividualId, LinebusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId',
            data: JSON.stringify({ "IndividualId": IndividualId, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessIdTempRisk(IndividualId, LinebusinessId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId',
            data: JSON.stringify({ "IndividualId": IndividualId, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetValidityParticipantCupoInConsortium(consortiumId, AmountInsured, LinebusinessId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetValidityParticipantCupoInConsortium',
            data: JSON.stringify({ "consortiumId": consortiumId, "AmountInsured": AmountInsured, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIdentificationPersonOrCompanyByIndividualId(individualId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetIdentificationPersonOrCompanyByIndividualId',
            data: JSON.stringify({ "individualId": individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    //static GetCompanyByIndividualId(individualId) {
    //    return $.ajax({
    //        async: false,
    //        type: "POST",
    //        url: rootPath + 'Underwriting/RiskSurety/GetCompanyByIndividualId',
    //        data: JSON.stringify({ "individualId": individualId }),
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8"
    //    });
    //}


    static GetSecureType(individualId, LinebusinessId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetSecureType',
            data: JSON.stringify({ "individualId": individualId, "LinebusinessId": LinebusinessId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}