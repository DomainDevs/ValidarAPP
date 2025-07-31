class RiskPropertyAdditionalDataRequest {

    static SaveAdditionalData(selectRisk, additionalData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/SaveAdditionalData',
            data: JSON.stringify({ riskId: selectRisk, propertyAdditionalDataViewModel: additionalData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetConstructionType() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetConstructionType',
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetRiskTypes',
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskUses() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskProperty/GetRiskUses',
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

}