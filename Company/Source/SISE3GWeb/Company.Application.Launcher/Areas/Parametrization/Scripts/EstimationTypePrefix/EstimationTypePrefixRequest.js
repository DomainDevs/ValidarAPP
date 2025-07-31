class EstimationTypePrefixRequest {

    static GetPrefixes() {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "EstimationTypePrefix/GetPrefixes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLineBusinessByEstimation(estimationTypeId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "EstimationTypePrefix/GetPrefixesByEstimationId",
            data: JSON.stringify({ "estimationTypeId": estimationTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveEstimationTypePrefix(estimationTypeId, listPrefix) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "EstimationTypePrefix/CreateEstimationTypePrefix",
            data: JSON.stringify({ "estimationTypeId": estimationTypeId, "PrefixesDTO": listPrefix }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }







}
