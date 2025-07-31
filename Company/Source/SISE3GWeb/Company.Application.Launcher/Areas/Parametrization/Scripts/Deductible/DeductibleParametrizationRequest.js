///Clase donde se deben crear todas las peticiones ajax del modulo
//Peticiones ajax vista principal
class DeductibleParametrizationRequest {

    //Obtener informacion 

    /**
     * @summary 
     *  Obtiene el listado de lineas de negocio     
     */
    static GetLineBusiness() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Deductible/GetLineBusiness',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    /**
    * @summary 
    *  Obtiene el listado de DEDUCTIBLE_UNIT
    */
    static GetDeductibleUnit() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Deductible/GetDeductibleUnit',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    /**
   * @summary 
   *  Obtiene el listado de DEDUCTIBLE_SUBJECT
   */
    static GetDeductibleSubject() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Deductible/GetDeductibleSubject',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    /**
    * @summary 
    *  Obtiene el listado de monedas
    */
    static GetCurrencies() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Deductible/GetCurrencies',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    /**
   * @summary 
   *  Obtiene el listado de tipos
   */
    static GetRateTypes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Deductible/GetRateTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    /**
      * @summary 
      *  Obtiene el listado de tipos
      */
    static GetDeductibles() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/Deductible/GetDeductibles',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    //Acciones CRUD
    static Save(deductibles) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Deductible/Save',
            data: JSON.stringify({ deductibles: deductibles }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    //Exportar excel
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Deductible/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    /**
   * @summary 
   *  Consulta avanzada
   */
    static GetDeductibleByDeductible(deductible) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Deductible/GetDeductibleByDeductible',
            data: JSON.stringify({ deductible: deductible }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}