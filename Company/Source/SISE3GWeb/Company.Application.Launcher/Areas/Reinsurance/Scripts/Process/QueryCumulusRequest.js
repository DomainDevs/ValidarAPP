class QueryCumulusRequest {
  static GetLineBusiness() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Reinsurance/Process/GetLineBusiness',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubLineBusiness(lineBusiness) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetSubLineBusiness',
            data: JSON.stringify({ lineBusiness: lineBusiness }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCumulusByIndividual(individualId, lineBusiness, cumulusDate, isFuture, subLineBusiness, prefixCd) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetCumulusByIndividual',
            data: JSON.stringify({ individualId: individualId, lineBusiness: lineBusiness, cumulusDate: cumulusDate, isFuture: isFuture, subLineBusiness: subLineBusiness, prefixCd: prefixCd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCumulusDetailByIndividual(individualId, lineBusiness, cumulusDate, isFuture, subLineBusiness, prefixCd) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetCumulusDetailByIndividual',
            data: JSON.stringify({ individualId: individualId, lineBusiness: lineBusiness, cumulusDate: cumulusDate, isFuture: isFuture, subLineBusiness: subLineBusiness, prefixCd: prefixCd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDetailCumulusParticipantsEconomicGroup(economicGroupId, lineBusiness, cumulusDate, isFuture, subLineBusiness, prefixCd) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetDetailCumulusParticipantsEconomicGroup',
            data: JSON.stringify({ economicGroupId: economicGroupId, lineBusiness: lineBusiness, cumulusDate: cumulusDate, isFuture: isFuture, subLineBusiness: subLineBusiness, prefixCd: prefixCd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    } 

    static GetInsuredsByDescription(query) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GetInsuredsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ query : query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GenerateFileCumulusByIndividual(fileName,coverageReinsuranceCumulusDTOs) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Reinsurance/Process/GenerateFileCumulusByIndividual',
            data: JSON.stringify({ fileName: fileName, coverageReinsuranceCumulusDTOs: coverageReinsuranceCumulusDTOs }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static getPrefixes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Reinsurance/Process/GetPrefix',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}
