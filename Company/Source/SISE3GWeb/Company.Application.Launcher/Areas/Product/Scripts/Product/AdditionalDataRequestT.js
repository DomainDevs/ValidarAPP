class AdditionalDataRequestT {

    static LoadProductDeductibles(productId, prefixCode) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Product/Product/GetProductDeductiblesByPrefix",
            data: JSON.stringify({ productId: productId, prefixCode: prefixCode }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadProductActivities(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Product/Product/GetRiskCommercialClass",
            data: JSON.stringify({ productId: productId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadProductForm(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Product/Product/GetProductForm",
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadProductLimitRc(policyTypes, productId, prefixCode) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Product/Product/GetLimitRc",
            data: JSON.stringify({ policyTypes: policyTypes, productId: productId, prefixCode: prefixCode}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static SaveDataAdditional(listRiskCommercialClass, listCiaParamLimitsRCServiceModel, listCiaParamDeductibleProductServiceModel, listCiaParamFormServiceModel, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Product/Product/SaveAdditionalData",
            data: JSON.stringify({ listRiskCommercialClass: listRiskCommercialClass, listCiaParamLimitsRCServiceModel: listCiaParamLimitsRCServiceModel, listCiaParamDeductibleProductServiceModel: listCiaParamDeductibleProductServiceModel, listCiaParamFormServiceModel: listCiaParamFormServiceModel, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });       
    }
}