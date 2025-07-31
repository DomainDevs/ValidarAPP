class CoverageRequestT {

    static ValidatePolicyByProductId(productId, riskId, coverId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/ValidatePolicyByProductId',
            data: JSON.stringify({ productId: productId, riskId: riskId, coverId: coverId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",        
        });

    }

    static GetCoveragesByTechnicalPlanId(technicalPlanId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetCoveragesByTechnicalPlanId',
            data: JSON.stringify({ technicalPlanId: technicalPlanId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesPrincipalByInsuredObjectId(insuredObjectId) {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Product/Product/GetCoveragesPrincipalByInsuredObjectId?insuredObjectId=' + insuredObjectId,
            dataType: "json",
            contentType: "application/json; charset=utf-8"       
        });
    }

    static ExistCoverageProduct(productId, groupCoverageId, insuredObjectId, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, insuredObjectId: insuredObjectId , coverageId: coverageId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoverageProductByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetCoverageProductByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


}