///Clase donde se deben crear todas las peticiones ajax del modulo
//Peticiones ajax vista principal
class TextPrecataloguedParametrizationRequest {

    //Obtener informacion 

    /**
     * @summary 
     *  Obtiene el listado de los niveles     
     */

    static GetLevels() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/TextPrecatalogued/GetLevels',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetTextPrecatalogued() {
        request('Parametrization/TextPrecatalogued/GetTexPrecataloged', null, 'GET', AppResources.ErrorSearchTextPrecatalogued
            , TextPrecataloguedParametrization.getTextPrecatalogued);
    }

    static GetCommercialBranch() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/TextPrecatalogued/GetCommercialBranch',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCoveredRisk() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/TextPrecatalogued/GetCoveredRiskType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetTextCoverage(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/TextPrecatalogued/GetCoverage",
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateTextPrecatalogued(texPrecataloguedFilters) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/TextPrecatalogued/CreateTextPrecatalogued",
            data: JSON.stringify({ textPrecatalogueds :texPrecataloguedFilters }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            
        });
        

    }

    //static GenerateFileToExport() {
    //    return $.ajax({
    //        type: 'POST',
    //        url: rootPath + 'Parametrization/Clauses/GenerateFileToExport',
    //        dataType: 'json',
    //        contentType: 'application/json; charset=utf-8',
    //    });
    //}

    //static GetClausesByNameAnTitle(description) {
    //    return $.ajax({
    //        type: "POST",
    //        url: rootPath + "Parametrization/Clauses/GetListClause",
    //        data: JSON.stringify({ description: description }),
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8"
    //    });
    //}



}