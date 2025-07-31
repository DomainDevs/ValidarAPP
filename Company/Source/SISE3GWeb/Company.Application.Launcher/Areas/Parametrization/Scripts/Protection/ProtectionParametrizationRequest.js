///Clase donde se deben crear todas las peticiones ajax del modulo
//Peticiones ajax vista principal
class ProtectionParametrizationRequest {
       
    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Protection/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}