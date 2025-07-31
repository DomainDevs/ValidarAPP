class MinPremiunRelationRequest {
    static GetPrefix() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GetPrefix',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetSearch(PrefixId, ProductName) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GetMinPremiunRelationByPrefixAndProduct',
            data: JSON.stringify({ PrefixId: PrefixId, ProductName: ProductName }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetBranch() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GetBranch',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetEndorsementType() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GetEndorsementType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetCurrency() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GetCurrency',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetProductType(PrefixId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GetProductType',
            data: JSON.stringify({ PrefixId: PrefixId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetClave(productId, prefixId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GetClave',
            data: JSON.stringify({ ProductId: productId, PrefixId: prefixId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/MinPremiunRelation/GenerateFileToExport',           
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}