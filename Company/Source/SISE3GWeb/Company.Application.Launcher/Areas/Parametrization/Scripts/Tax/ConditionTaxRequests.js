$.ajaxSetup({ async: true });
class ConditionTaxRequests {
    static GetConditionsByTaxId(taxId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetConditionsByTaxId',
            dataType: 'json',
            data: JSON.stringify({ taxId: taxId }),
            contentType: 'application/json; charset=utf-8',
        });
    }

    static SaveTaxCondition(conditionTaxModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/SaveTaxCondition',
            dataType: 'json',
            data: JSON.stringify({ conditionTaxViewModelList: conditionTaxModel }),
            contentType: 'application/json; charset=utf-8',
        });
    }

    static DeleteSelectedTaxCondition(conditionId, taxId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/DeleteSelectedTaxCondition',
            dataType: 'json',
            data: JSON.stringify({ conditionId: conditionId, taxId: taxId }),
            contentType: 'application/json; charset=utf-8',
        });
    } 
}