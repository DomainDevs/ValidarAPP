class NoticeVehicleRequest {
    static GetVehicleMakes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleMakes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetVehicleModelsByVehicleMakeId(vehicleMakeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleModelsByMakeId',
            data: JSON.stringify({ vehicleMakeId: vehicleMakeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetVehicleVersionsByVehicleMakeIdAndByVehicleModelId(vehicleMakeId, vehicleModelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleVersionsByMakeIdModelId',
            data: JSON.stringify({ vehicleMakeId: vehicleMakeId, vehicleModelId: vehicleModelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetVehicleVersionYearsByVehicleMakeIdAndByVehicleModelIdAndByVehicleVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleVersionYearsByMakeIdModelIdVersionId',
            data: JSON.stringify({ vehicleMakeId: vehicleMakeId, vehicleModelId: vehicleModelId, vehicleVersionId: vehicleVersionId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetVehicleColors() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleColors',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetDamageTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Notice/GetDamageTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetDamageResponsibilities() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Notice/GetDamageResponsibilities',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetDataPoliciesVehicleByIndividualId(individualId, myDate, type) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetDataPoliciesVehicleByIndividualId',
            data: JSON.stringify({ "individualId": individualId, "myDate": myDate, "type": type }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetPolicyByEndorsementIdModuleType(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/GetPolicyByEndorsementIdModuleType',
            data: JSON.stringify({ "endorsementId": endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static ExecuteNoticeOperations(noticeVehicleDTO, contactInformationDTO, vehicleDTO, noticeCoverageDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/ExecuteNoticeOperations',
            data: JSON.stringify({
                "noticeVehicleDTO": noticeVehicleDTO,
                "contactInformationDTO": contactInformationDTO,
                "vehicleDTO": vehicleDTO,
                "noticeCoverageDTO": noticeCoverageDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskByPlate(inputRiskPlate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetRiskByPlate',
            data: JSON.stringify({
                "query": inputRiskPlate
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByInsuredId(insuredId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetRisksByInsuredId',
            data: JSON.stringify({ "insuredId": insuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskVehicleByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetRiskVehicleByRiskId',
            data: JSON.stringify({ "riskId": riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskVehicleByClaimNoticeId(claimNoticeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetRiskVehicleByClaimNoticeId',
            data: JSON.stringify({ "claimNoticeId": claimNoticeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByPlate(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetRiskByPlate',
            data: JSON.stringify({ "query": query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}