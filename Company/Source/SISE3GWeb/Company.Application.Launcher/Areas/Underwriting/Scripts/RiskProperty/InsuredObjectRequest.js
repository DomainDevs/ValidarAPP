class InsuredObjectRequest {
    static selectCoverageFocusout(tempId, riskId, coverageId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCoverageToAddByRiskId",
            data: JSON.stringify({ tempId: tempId, riskId: riskId, coverageId: coverageId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDiferences(riskId, defaultCoverages, allCoverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetDiferences",
            data: JSON.stringify({ riskId: riskId, defaultCoverages: defaultCoverages, allCoverages: allCoverages }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static QuotationCoverage(riskId, coverage) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/QuotationCoverage",
            data: JSON.stringify({ riskId: riskId, coverage: coverage }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAllyCoverageByCoverage(tempId, riskId, groupCoverageId, coverage) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetAllyCoverageByCoverage",
            data: JSON.stringify({ tempId: tempId, riskId: riskId, groupCoverageId: groupCoverageId, coverage: coverage }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static ReturnRisk(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetPolicyModelsView',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetTemporalCoveragesByRiskIdInsuredObjectId",
            data: JSON.stringify({ riskId: riskId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCalculateCoverages(riskId, insuredObject, depositPremiumPercent, rate, currentFrom,
        currentTo, insuredLimit, runRulesPre, runRulesPost) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetCalculateCoveragesByInsuredObjectId',
            data: JSON.stringify({
                riskId: riskId, insuredObject: insuredObject,
                depositPremiumPercent: depositPremiumPercent, rate: rate, currentFrom: currentFrom,
                currentTo: currentTo, insuredLimit: insuredLimit, runRulesPre: runRulesPre, runRulesPost: runRulesPost
            }),
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTemporalCoveragesByInsuredObjectIdGroupCoverageId(insuredObjectId, groupCoverageId, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCoveragesByInsuredObjectIdGroupCoverageIdProductId",
            data: JSON.stringify({ insuredObjectId: insuredObjectId, groupCoverageId: groupCoverageId, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveInsuredObject(RiskId, objectModel, tempId, groupCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/SaveInsuredObject',
            data: JSON.stringify({ RiskId: RiskId, objectModel: objectModel, tempId: tempId, groupCoverageId: groupCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskProperty/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static SaveCoverage(TemporalId, RiskId, coverageModel, insuredObjectId, insuredObjectDesc, declarationPeriodId, billingPeriodId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/SaveCoverage',
            data: JSON.stringify({ TemporalId: TemporalId, RiskId: RiskId, coverageModel: coverageModel, insuredObjectId: insuredObjectId, insuredObjectDesc: insuredObjectDesc, declarationPeriodId: declarationPeriodId, billingPeriodId: billingPeriodId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoverageByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetCoverageByCoverageId',
            data: JSON.stringify({ coverageId: coverageId, riskId: glbRisk.Id, temporalId: glbPolicy.Id, groupCoverageId: glbRisk.GroupCoverage.Id }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectByTemporalIdRiskIdInsuredObjectId(temporalId, riskId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetInsuredObjectByTemporalIdRiskIdInsuredObjectId',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskById(temporalId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetRiskById',
            data: JSON.stringify({ temporalId: temporalId, id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRateTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetRateTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetFirstRiskTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetFirstRiskTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCalculeTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCalculeTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjects(productId, groupCoverageId, prefix) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetInsuredsObjectsByProductIdGroupCoverageId",
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefix }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesByObjectId(selectInsuredObjectId, groupCoverageId, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCoveragesByInsuredObjectIdGroupCoverageIdProductId",
            data: JSON.stringify({ insuredObjectId: selectInsuredObjectId, groupCoverageId: groupCoverageId, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDeductiblesByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetDeductiblesByCoverageId",
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoverages(productId, groupCoverageId, prefix) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCoverages",
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefix }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesByCoverageId(productId, groupCoverageId, prefix, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCoveragesByCoverageId",
            data: JSON.stringify({ productId: productId, coverageGroupId: groupCoverageId, prefixId: prefix, coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesNameByProductIdGroupCoverageId(productId, groupCoverageId, prefix, coverageId, selectInsuredObject) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/GetCoveragesByCoveragesAdd",
            data: JSON.stringify({ productId: productId, coverageGroupId: groupCoverageId, prefixId: prefix, coveragesAdd: coverageId, insuredObjectId: selectInsuredObject }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static RunRulesCoverage(riskId, coverage, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskProperty/RunRulesCoverage",
            data: JSON.stringify({ riskId: riskId, coverage: coverage, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDeclarationPeriods() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetDeclarationPeriods',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBillingPeriods() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetBillingPeriods',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}