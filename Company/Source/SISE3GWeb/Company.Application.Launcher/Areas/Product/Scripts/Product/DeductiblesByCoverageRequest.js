class DeductiblesByCoverageRequest {

    static GetBeneficiaryTypes() {
        return $.ajax({
            type: 'POST',
            url: 'GetBeneficiaryTypes',
            data: JSON.stringify({}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetDeductiblesByProductByGroupCoverageByCoverageByBeneficiaryType(productId, groupCoverageId, coverageId, beneficiaryTypeId) {
        return $.ajax({
            type: 'POST',
            url: 'GetDeductiblesByProductByGroupCoverageByCoverageByBeneficiaryType',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, coverageId: coverageId, beneficiaryTypeId: beneficiaryTypeId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetDeductiblesByProductIdByGroupCoverageBycoverageId(productId, coverGroupId, CoverageID, BeneficiaryTypeCd, lineBusinnessCd) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Product/Product/GetDeductiblesByProductIdByGroupCoverageBycoverageId",
            data: JSON.stringify({ "productId": productId, "coverGroupId": coverGroupId, "CoverageID": CoverageID, "BeneficiaryTypeCd": BeneficiaryTypeCd, "lineBusinnessCd": lineBusinnessCd  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDeductiblesByPrefixId(prefixId, band) {
        return $.ajax({
            type: 'POST',
            url: 'GetDeductiblesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId, band: band }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

 

}