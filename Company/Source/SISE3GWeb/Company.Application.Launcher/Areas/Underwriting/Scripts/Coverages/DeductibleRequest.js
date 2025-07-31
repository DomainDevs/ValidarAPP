//Peticiones ajax vista principal
class DeductibleRequest {
    /**
    * @summary 
    *  Obtiene el listado de unidades    
    */
    static GetDeductibleUnit() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Deductible/GetDeductibleUnit',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    /**
    * @summary 
    *  Obtiene el listado de deducibles    
    */
    static GetDeductibleSubject() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Deductible/GetDeductibleSubject',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
        });
    }

    static GetRateTypesDeduct() {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + 'Underwriting/Deductible/GetRateTypesDeduct',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}