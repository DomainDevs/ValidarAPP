class RiskSuretyCoverageRequest {

    static SaveCoverages(policyId,riskId,coverage) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/SaveCoverages',
            data: JSON.stringify({ tempId: policyId, riskId: riskId, coverages: coverage }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    ReturnRisk(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetPolicyModelsView',
            data: JSON.stringify({ temporalId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCoveragesByRiskId',
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static QuotationCoverage(coverageModel, coverageData, runRulesPost, PolicyId,  listCompanyCoverage) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/QuotationCoverage',
            data: JSON.stringify({ coverageModel: coverageModel, coverage: coverageData, RunRulesPost: runRulesPost, PolicyId: PolicyId, listCompanyCoverage}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoverageByCoverageId(coverageId, riskId, policyId, RiskGroupCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCoverageByCoverageId',
            data: JSON.stringify({ coverageId: coverageId, riskId: riskId, temporalId: policyId, groupCoverageId: RiskGroupCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId, coveragesAdd) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Coverage/GetCoveragesByProductIdGroupCoverageIdPrefixId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, coveragesAdd: coveragesAdd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetListCoverageTemporal(policyId, riskId, coverage) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetListCoverageTemporal',
            data: JSON.stringify({ tempId: policyId, riskId: riskId, coverages: coverage }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}