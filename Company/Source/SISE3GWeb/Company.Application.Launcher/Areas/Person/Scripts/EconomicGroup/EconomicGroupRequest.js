class EconomicGroupRequest {
    static CreateEconomicGroup(economicGroup, listGroupDeltail) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/EconomicGroup/SaveEconomicGroup",
            data: JSON.stringify({ economicGroup: economicGroup}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetTrbutaryType() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/EconomicGroup/GetTributaryType",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    static GetHolders(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/EconomicGroup/GetHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIndividualGroupDetail(economicGroupId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/EconomicGroup/GetIndividualDetail',
            data: JSON.stringify({ EconomicGroupId: economicGroupId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEconomicGroupByDocument(name, number) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/EconomicGroup/GetEconomicGroupByDocument',
            data: JSON.stringify({ groupName: name, document: number  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIndividualsDetail(id, idEconomicGroup) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/EconomicGroup/GetIndividualDetails',
            data: JSON.stringify({ individualId: id, idEconomicGroup: idEconomicGroup }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/EconomicGroup/GetPrefixes",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
    
    static CreateEconomicGroupEvent(economicGroupEventDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/EconomicGroup/CreateEconomicGroupEvent',
            data: JSON.stringify({ economicGroupEventDTO: economicGroupEventDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static AssigendIndividualToEconomicGroupEvent(economicGroupEventDTOs) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/EconomicGroup/AssigendIndividualToEconomicGroupEvent',
            data: JSON.stringify({ economicGroupEventDTOs: economicGroupEventDTOs}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}