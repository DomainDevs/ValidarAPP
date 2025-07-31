class RiskMarineCoverageRequest {

    static GetCalculationTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetCalculationTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRateTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetRateTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskMarine/GetTemporalCoveragesByRiskIdInsuredObjectId",
            data: JSON.stringify({ riskId: riskId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static QuotationCoverage(coverage, marineDTO, runRulesPre, runRulesPost) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/QuotationCoverage',
            data: JSON.stringify({ coverage: coverage, marineDTO: marineDTO, runRulesPre: runRulesPre, runRulesPost: runRulesPost }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDeductiblesByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetDeductiblesByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByInsuredObjectId(insuredObjectId, coverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetCoveragesByInsuredObjectId',
            data: JSON.stringify({ insuredObjectId: insuredObjectId, coverages: coverages }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetInsuredObjectsByProductIdGroupCoverageId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(productId, groupCoverageId, prefixId, insuredObjects) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetInsuredObjectsByProductIdGroupCoverageId_inCoverages',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, insuredObjects: insuredObjects }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(marineDTO, insuredObject, groupCoverageId, productId,
        depositPremiumPercent, idRate, rate, currentFrom, currentTo, insuredLimit, runRulesPre, runRulesPost) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetCoveragesByInsuredObjectIdGroupCoverageIdProductId',
            data: JSON.stringify({
                marineDTO: marineDTO, insuredObject: insuredObject, groupCoverageId: groupCoverageId, productId: productId,
                depositPremiumPercent: depositPremiumPercent, idRate: idRate, rate: rate, currentFrom: currentFrom,
                currentTo: currentTo, insuredLimit: insuredLimit, runRulesPre: runRulesPre, runRulesPost: runRulesPost
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveInsuredObject(RiskId, insuredObjectDTO, tempId, groupCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/SaveInsuredObject',
            data: JSON.stringify({ RiskId: RiskId, insuredObjectDTO: insuredObjectDTO, tempId: tempId, groupCoverageId: groupCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveCoverages(policyId, riskId, coverages, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/SaveCoverages',
            data: JSON.stringify({ policyId: policyId, riskId: riskId, coverages: coverages, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByRiskId(riskId, temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskMarine/GetCoveragesByRiskId',
            data: JSON.stringify({ riskId: riskId, temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}