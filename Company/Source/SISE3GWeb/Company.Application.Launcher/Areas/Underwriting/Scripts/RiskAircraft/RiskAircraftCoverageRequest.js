class RiskAircraftCoverageRequest {

    static GetCalculationTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetCalculationTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRateTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetRateTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Underwriting/RiskAircraft/GetTemporalCoveragesByRiskIdInsuredObjectId",
            data: JSON.stringify({ riskId: riskId, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static QuotationCoverage(coverage, aircraftDTO, runRulesPre, runRulesPost) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/QuotationCoverage',
            data: JSON.stringify({ coverage: coverage, aircraftDTO: aircraftDTO, runRulesPre: runRulesPre, runRulesPost: runRulesPost }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDeductiblesByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetDeductiblesByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByInsuredObjectId(insuredObjectId, coverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetCoveragesByInsuredObjectId',
            data: JSON.stringify({ insuredObjectId: insuredObjectId, coverages: coverages }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetInsuredObjectsByProductIdGroupCoverageId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(productId, groupCoverageId, prefixId, insuredObjects) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetInsuredObjectsByProductIdGroupCoverageId_inCoverages',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, insuredObjects: insuredObjects }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(aircraftDTO, insuredObject, groupCoverageId, productId,
        depositPremiumPercent, idRate, rate, currentFrom, currentTo, insuredLimit, runRulesPre, runRulesPost) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetCoveragesByInsuredObjectIdGroupCoverageIdProductId',
            data: JSON.stringify({
                aircraftDTO: aircraftDTO, insuredObject: insuredObject, groupCoverageId: groupCoverageId, productId: productId,
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
            url: rootPath + 'Underwriting/RiskAircraft/SaveInsuredObject',
            data: JSON.stringify({ RiskId: RiskId, insuredObjectDTO: insuredObjectDTO, tempId: tempId, groupCoverageId: groupCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveCoverages(policyId, riskId, coverages, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/SaveCoverages',
            data: JSON.stringify({ policyId: policyId, riskId: riskId, coverages: coverages, insuredObjectId: insuredObjectId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByRiskId(riskId, temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskAircraft/GetCoveragesByRiskId',
            data: JSON.stringify({ riskId: riskId, temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}