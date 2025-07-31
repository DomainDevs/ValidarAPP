class RiskThirdPartyLiabilityCoverageRequest extends Uif2.Page {
    static GetCoveragesByRiskId(riskId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetCoveragesByRiskId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetCoverageByCoverageId(coverageId, riskId, temporalId, groupCoverageId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetCoverageByCoverageId',
            data: JSON.stringify({ coverageId: coverageId, riskId: riskId, temporalId: temporalId, groupCoverageId: groupCoverageId}),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }
    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
   
    static QuotationCoverage(coverage, coverageModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/QuotationCoverage',
            data: JSON.stringify({ coverage: coverage ,coverageModel: coverageModel}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveCoverages(temporalId,  riskId, coverages) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/SaveCoverages',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, coverages: coverages}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static ReturnRisk(temporalId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetPolicyModelsView',
            data: JSON.stringify({ temporalId: temporalId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId, coveragesAdd) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetCoveragesByProductIdGroupCoverageIdPrefixId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, coveragesAdd: coveragesAdd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDeductiblesByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetDeductiblesByCoverageId',
            data: JSON.stringify({ coverageId: coverageId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

 }
