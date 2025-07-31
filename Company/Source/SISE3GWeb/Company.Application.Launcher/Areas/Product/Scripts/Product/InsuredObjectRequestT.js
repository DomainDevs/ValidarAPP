class InsuredObjectRequestT {
    static ValidatePolicyByProductId(productId, riskId, coverId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/ValidatePolicyByProductId',
            data: JSON.stringify({ productId: productId, riskId: riskId, coverId: coverId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",          
         
        });
    }

    static ajaxGetCoveredRiskByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetCoveredProductById',
            data: JSON.stringify({ productId: productId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExistInsuredObjectProduct(productId, groupCoverageId, insuredObjectId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, insuredObjectId: insuredObjectId, riskId: riskId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoverageRiskTypeByPrefixId(prefixId) {
        var controller = rootPath + "Product/Product/CoverageGetRiskTypesByPrefixId";
        return $.ajax({
            type: "POST",
            url: controller,
            data: { prefixId: prefixId  }
        });
    }

    static GetCoverageGroupCoverageByRiskType(riskTypeId) {
        var controller = rootPath + 'Product/Product/CoverageGetGroupCoveragesByRiskTypeId?riskTypeId=' + riskTypeId;

        return $.ajax({
            type: "GET",
            url: controller
        });
    }

    static GetInsuredObjectsByPrefixId(prefixId, insuredObjectsAdd) {
        var controller = rootPath + 'Product/Product/CoverageGetInsuredObjectsByPrefixId?prefixId=' + prefixId + '&insuredObjectsAdd=' + insuredObjectsAdd;

        return $.ajax({
            type: "GET",
            url: controller
        });
    }

}