class RulesAndscriptRequestT {

    static GetAllRuleSetsByLevelId() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetAllRuleSetsByLevelId',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });

    }

    static GetPackageEnabled() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetPackageEnabled'
        });
    }

}