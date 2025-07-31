$.ajaxSetup({ async: true });
class CategoryTaxRequests {
    static GetCategoriesByTaxId(taxId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/GetCategoriesByTaxId',
            dataType: 'json',
            data: JSON.stringify({ taxId: taxId }),
            contentType: 'application/json; charset=utf-8',
        });
    }

    static SaveTaxCategory(categoryTaxModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/SaveTaxCategory',
            dataType: 'json',
            data: JSON.stringify({ categoryTaxViewModelList: categoryTaxModel }),
            contentType: 'application/json; charset=utf-8',
        });
    } 

    static DeleteSelectedTaxCategory(categoryId, taxId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Tax/DeleteSelectedTaxCategory',
            dataType: 'json',
            data: JSON.stringify({ categoryId: categoryId, taxId: taxId }),
            contentType: 'application/json; charset=utf-8',
        });
    } 
}