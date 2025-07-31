class RiskLiabilityCoverageRequest {
    static GetCoveragesByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetCoveragesByRiskId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoverageByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetCoverageByCoverageId',
            data: JSON.stringify({ coverageId: coverageId, riskId: glbRisk.Id, temporalId: glbPolicy.Id, groupCoverageId: glbRisk.GroupCoverage.Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {

        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static QuotationCoverage(coverageModel, coverageData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/QuotationCoverage',
            data: JSON.stringify({ coverageModel: coverageModel, coverage: coverageData }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    
    }

    static SaveCoverages(policyId,riskId,coverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/SaveCoverages',
            data: JSON.stringify({ temporalId: policyId, riskId: riskId, coverages: coverages  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ReturnRisk(PolicyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetPolicyModelsView',
            data: JSON.stringify({ temporalId: PolicyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAllyCoverageByCoverage(glbPolicyId, glbRiskId, GroupCoverageId, coverageData) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskLiability/GetAllyCoverageByCoverage",
            data: JSON.stringify({ tempId: glbPolicyId, riskId: glbRiskId, groupCoverageId: GroupCoverageId, coverage: coverageData }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId, coveragesAdd) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskLiability/GetCoveragesByProductIdGroupCoverageIdPrefixId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, coveragesAdd: coveragesAdd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}