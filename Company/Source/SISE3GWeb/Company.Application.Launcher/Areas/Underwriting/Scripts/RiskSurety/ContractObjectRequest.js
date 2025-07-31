class RiskSuretyContractRequest{
    static GetContratcObjectsByNameLevelIdConditionLevelId(name,levelId, conditionLevelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTextsByNameLevelIdConditionLevelId',
            data: JSON.stringify({ name: name, levelId: levelId, conditionLevelId: conditionLevelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveContractObject(riskId, textModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/SaveContractObject',
            data: JSON.stringify({ riskId: riskId, textModel: textModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}