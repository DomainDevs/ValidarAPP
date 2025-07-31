class RiskTransportCoverageRequest {

    static GetCalculationTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCalculationTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRateTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetRateTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskTransport/GetTemporalCoveragesByRiskIdInsuredObjectId",
            data: JSON.stringify({ riskId: riskId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static QuotationCoverage(coverage, transportDTO, runRulesPre, runRulesPost) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/QuotationCoverage',
            data: JSON.stringify({ coverage: coverage, transportDTO: transportDTO, runRulesPre: runRulesPre, runRulesPost: runRulesPost }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static QuotationCoverages(coverages, transportDTO, runRulesPre, runRulesPost) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/QuotationCoverages',
            data: JSON.stringify({ coverages: coverages, transportDTO: transportDTO, runRulesPre: runRulesPre, runRulesPost: runRulesPost }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
        });
    }
    static GetDeductiblesByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetDeductiblesByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByInsuredObjectId(insuredObjectId, coverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCoveragesByInsuredObjectId',
            data: JSON.stringify({ insuredObjectId: insuredObjectId, coverages: coverages }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetInsuredObjectsByProductIdGroupCoverageId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(productId, groupCoverageId, prefixId, insuredObjects) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetInsuredObjectsByProductIdGroupCoverageId_inCoverages',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, insuredObjects: insuredObjects }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(transportDTO, insuredObject, groupCoverageId, productId,
        currentFrom, currentTo) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCoveragesByInsuredObjectIdGroupCoverageIdProductId',
            data: JSON.stringify({
                transportDTO: transportDTO, insuredObject: insuredObject, groupCoverageId: groupCoverageId, productId: productId,
                currentFrom: currentFrom, currentTo: currentTo
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveInsuredObject(RiskId, insuredObjectDTO, tempId, groupCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/SaveInsuredObject',
            data: JSON.stringify({ RiskId: RiskId, insuredObjectDTO: insuredObjectDTO, tempId: tempId, groupCoverageId: groupCoverageId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveCoverages(policyId, riskId, coverages, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/SaveCoverages',
            data: JSON.stringify({ policyId: policyId, riskId: riskId, coverages: coverages, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByRiskId(riskId, temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCoveragesByRiskId',
            data: JSON.stringify({ riskId: riskId, temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDiferences(riskId, defaultCoverages, allCoverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskTransport/GetDiferences",
            data: JSON.stringify({ riskId: riskId, defaultCoverages: defaultCoverages, allCoverages: allCoverages }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesByProductIdGroupCoverageId(productId, groupCoverageId, prefix, coverageId, selectInsuredObject) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskTransport/GetCoveragesByCoveragesAdd",
            data: JSON.stringify({ productId: productId, coverageGroupId: groupCoverageId, prefixId: prefix, coveragesAdd: coverageId, insuredObjectId: selectInsuredObject }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoverageByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskTransport/GetCoverageByCoverageId',
            data: JSON.stringify({ coverageId: coverageId, temporalId: glbPolicy.Id, groupCoverageId: glbRisk.CoverageGroupId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesByCoverageId(productId, groupCoverageId, prefix, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskTransport/GetCoveragesByCoverageId",
            data: JSON.stringify({ productId: productId, coverageGroupId: groupCoverageId, prefixId: prefix, coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}