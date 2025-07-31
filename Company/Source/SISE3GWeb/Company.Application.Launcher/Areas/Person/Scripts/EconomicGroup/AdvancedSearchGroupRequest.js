class AdvancedSearchGroupRequest {
    static GetEconomicGroupAdvancedSearch(economicGroup) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/EconomicGroup/GetEconomicGroupAdvancedSearch',
            data: JSON.stringify({ economicGroup: economicGroup }),
            datatype: "json",
            contentType: "application/json; charset=utf-8"
        });
    } 
}