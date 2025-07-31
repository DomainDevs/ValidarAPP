class EstimationTypeStatusRequest {
    static GetEstimationTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/EstimationTypeStatus/GetEstimationTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/EstimationTypeStatus/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatusesByEstimationTypeId(estimationTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/EstimationTypeStatus/GetStatusesByEstimationTypeId',
            data: JSON.stringify({ estimationTypeId: estimationTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEstimationTypeStatusUnassignedByEstimationTypeId(estimationTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/EstimationTypeStatus/GetEstimationTypeStatusUnassignedByEstimationTypeId',
            data: JSON.stringify({ estimationTypeId: estimationTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatuses() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/EstimationTypeStatus/GetStatuses',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateStatusByEstimationType(statusDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/EstimationTypeStatus/CreateStatusByEstimationType',
            data: JSON.stringify({ statusDTO: statusDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteStatusByEstimationType(statusDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/EstimationTypeStatus/DeleteStatusByEstimationType',
            data: JSON.stringify({ statusDTO: statusDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetReasonsByStatusIdPrefixId(statusId, prefixId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/EstimationTypeStatus/GetReasonsByStatusIdPrefixId',
            data: JSON.stringify({ statusId: statusId, prefixId: prefixId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateReason(reasonDTO) {
       
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/EstimationTypeStatus/CreateReason',
            data: JSON.stringify({ reasonDTO: reasonDTO }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static UpdateReason(reasonDTO) {
     
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/EstimationTypeStatus/UpdateReason',
            data: JSON.stringify({ reasonDTO: reasonDTO }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteReason(reasonId, statusId, prefixId) {

        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/EstimationTypeStatus/DeleteReason',
            data: JSON.stringify({ reasonId: reasonId, statusId: statusId, prefixId: prefixId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}