///Clase donde se deben crear todas las peticiones ajax del modulo
//Peticiones ajax vista principal
class ClauseParametrizationRequest {

     //Obtener informacion 

    /**
     * @summary 
     *  Obtiene el listado de los niveles     
     */

    static GetLevels() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Clauses/GetLevels',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCommercialBranch() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Clauses/GetCommercialBranch',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCoveredRisk() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Clauses/GetCoveredRiskType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }


    static GetLineBusiness() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Clauses/GetLineBusiness',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetTextClause(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Clauses/GetTextClause",
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTextCoverage(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Clauses/GetCoverage",
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Clauses/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetClausesByNameAnTitle(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Clauses/GetListClause",
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    
}