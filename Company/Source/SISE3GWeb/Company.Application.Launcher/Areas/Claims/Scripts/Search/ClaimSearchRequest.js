class ClaimSearchRequest
{
    static GetBranches() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetBranches',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimSearch/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSearchClaims(oClaimModel)
    {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/SearchClaims',
            data: JSON.stringify(oClaimModel),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVehicleMakes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleMakes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVehicleModelsByMakeId(vehicleMakeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleModelsByMakeId',
            data: JSON.stringify({ vehicleMakeId: vehicleMakeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVehicleVersionsByMakeIdModelId(vehicleMakeId, vehicleModelId)
    {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleVersionsByMakeIdModelId',
            data: JSON.stringify({ vehicleMakeId: vehicleMakeId, vehicleModelId: vehicleModelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVehicleVersionYearsByMakeIdModelIdVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId)
    {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/NoticeVehicle/GetVehicleVersionYearsByMakeIdModelIdVersionId',
            data: JSON.stringify({ vehicleMakeId: vehicleMakeId, vehicleModelId: vehicleModelId, vehicleVersionId: vehicleVersionId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/Claim/GetCountries',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetStatesByCountryId',
            data: JSON.stringify({ countryId: countryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimPrefixCoveredRiskTypeByPrefixCode(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/GetClaimPrefixCoveredRiskTypeByPrefixCode',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchClaimNotice(searchModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/SearchNotices',
            data: JSON.stringify(searchModel),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchPolicy(searchModelPolicy) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/SearchPolicy',
            data: JSON.stringify(searchModelPolicy),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentSource() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/PaymentRequest/GetPaymentSource',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBranchesByUserId() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimSearch/GetBranchesByUserId',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchPaymentRequests(paymentRequest) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/SearchPaymentRequests',
            data: JSON.stringify({ paymentRequestDTO: paymentRequest}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SearchChargeRequests(chargeRequest) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/SearchChargeRequests',
            data: JSON.stringify({ chargeRequestDTO: chargeRequest }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateObjectedClaimNotice(notice) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/ObjectNotice',
            data: JSON.stringify({ notice: notice }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSearchTypes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/ClaimSearch/GetSeachTypes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByPlate(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/GetRiskByPlate',
            data: JSON.stringify({ "query": query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskLocationByAddress(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/GetRiskLocationByAddress',
            data: JSON.stringify({ "query": query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskSuretyByInsuredId(query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/GetSuretiesByDescription',
            data: JSON.stringify({ "query": query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SendEmailToAgendNotice(subject, message, mailDestination) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/SendEmailToAgendNotice',
            data: JSON.stringify({ "subject": subject, "message": message, "mailDestination": mailDestination }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static ScheduleNotice(subject, message, startEventDate, finishEventDate, noticeNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Notice/ScheduleNotice',
            data: JSON.stringify({ "subject": subject, "message": message, "startEventDate": startEventDate, "finishEventDate": finishEventDate, "noticeNumber": noticeNumber}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetHoldersByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/GetHoldersByIndividualId',
            data: JSON.stringify({ "individualId": individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredsByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/GetInsuredsByIndividualId',
            data: JSON.stringify({ "individualId": individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetParticipantsByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/ClaimSearch/GetParticipantsByIndividualId',
            data: JSON.stringify({ "individualId": individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentBeneficiaryByBeneficiaryId(beneficiaryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetPaymentBeneficiaryByBeneficiaryId',
            data: JSON.stringify({ "beneficiaryId": beneficiaryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}