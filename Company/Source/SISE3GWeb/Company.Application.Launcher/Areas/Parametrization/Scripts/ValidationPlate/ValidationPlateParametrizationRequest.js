///Clase donde se deben crear todas las peticiones ajax del modulo
//Peticiones ajax vista principal
class ValidationPlateParametrizationRequest {
    static GetMakes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/ValidationPlate/GetMakes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetModelByMake(Id) {
        return $.ajax({
            url: rootPath + 'Parametrization/ValidationPlate/GetModelsByMakeId',
            async: false,
            data: { MakeID: Id },
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetVersionsByMakeIdModelId(makeId, modelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ValidationPlate/GetVersionsByMakeIdModelId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCauses() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/ValidationPlate/GetCauses',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetFasecoldaCodeByMakeIdModelIdVersionId(makeId, modelId, versionId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ValidationPlate/GetFasecoldaCodeByMakeIdModelIdVersionId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetFasecoldaByCode(code, year) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/ValidationPlate/GetFasecoldaByCode',
            data: JSON.stringify({ code: code, year: year }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetListValidationPlate() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/ValidationPlate/GetValidationPlate',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}