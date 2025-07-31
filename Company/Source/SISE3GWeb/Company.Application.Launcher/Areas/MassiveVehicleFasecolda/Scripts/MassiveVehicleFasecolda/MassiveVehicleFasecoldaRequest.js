class MassiveVehicleFasecoldaRequest {
    static CreateLoadMassiveVehicleFasecolda(fasecoldaDTO) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/CreateLoadMassiveVehicleFasecolda',
            data: JSON.stringify({ fasecoldaDTO: fasecoldaDTO}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetErrorExcelProcessVehicleFasecolda(loadProcessId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/GetErrorExcelProcessVehicleFasecolda',
            data: JSON.stringify({ loadProcessId: loadProcessId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetVehicleFasecoldaByProccessId(loadProcessId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/GetVehicleFasecoldaByProccessId',
            data: JSON.stringify({ loadProcessId: loadProcessId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ValidateFileStructureMassiveVehicle(fasecoldaDTO, fasecoldaType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/ValidateFileStructureMassiveVehicle',
            data: JSON.stringify({ fasecoldaDTO: fasecoldaDTO, fasecoldaType: fasecoldaType}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateProccessMassiveVehicleFasecolda(processId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/GenerateProccessMassiveVehicleFasecolda',
            data: JSON.stringify({ processId: processId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static AddDataFromFile(excellWorkSheet, fileName ) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'MassiveVehicleFasecolda/MassiveVehicleFasecolda/AddDataFromFile',
            data: JSON.stringify({ excellWorkSheet: excellWorkSheet, fileName: fileName}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}