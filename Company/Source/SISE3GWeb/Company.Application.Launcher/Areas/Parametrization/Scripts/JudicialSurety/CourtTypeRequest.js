$.ajaxSetup({ async: true });
class CourtTypeRequest {
    static LoadCourtsType() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/JudicialSurety/LoadCourtsType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static UpdateCourtType(courtTypeDelete, courtTypeUpdate, courtTypeInsert) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/JudicialSurety/UpdateCourtType',
            data: JSON.stringify({ "courtTypeDelete": courtTypeDelete, "courtTypeUpdate": courtTypeUpdate, "courtTypeInsert": courtTypeInsert }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static LoadSearchCourtType(smallDescription) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/JudicialSurety/LoadSearchCourtType',
            data: JSON.stringify({ "smallDescription": smallDescription }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });

    }
}