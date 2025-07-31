class ClaimVehicleRequest {
    static GetDriverByDocumentNumber(documentNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimVehicle/GetDriverByDocumentNumber',
            data: JSON.stringify({ documentNumber: documentNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVehicleColors() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimVehicle/GetVehicleColors',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVehicleByLicensePlate(licensePlate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimVehicle/GetVehicleByLicensePlate',
            data: JSON.stringify({ plate: licensePlate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByEndorsementId(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimVehicle/GetRisksByEndorsementId',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteClaimOperations(claimVehicleDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimVehicle/ExecuteClaimOperations',
            data: JSON.stringify({
                claimVehicleDTO: claimVehicleDTO
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixesByCoveredRiskType() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimVehicle/GetPrefixesByCoveredRiskType',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }
}