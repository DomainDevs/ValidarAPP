class ChargeRequest {

    static GetPaymentSourcesByChargeRequest() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/Charge/GetPaymentSourcesByChargeRequest',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    
    static GetEstimationsTypesIdByMovementTypeId(movementTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Charge/GetEstimationsTypesIdByMovementTypeId',
            data: JSON.stringify({ movementTypeId: movementTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentMovementTypesByPaymentSourceId(sourceId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Charge/GetPaymentMovementTypesByPaymentSourceId',
            data: JSON.stringify({ sourceId: sourceId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBranches() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/Charge/GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetModuleDateByModuleTypeMovementDate() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/Charge/GetModuleDateByModuleTypeMovementDate',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPersonTypes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/Charge/GetPersonTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Claims/Charge/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetRecoveriesByClaim(branchId, prefixId, policyDocumentNumber, number) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Claims/Charge/GetRecoveriesByClaim',
            data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyDocumentNumber: policyDocumentNumber ,claimNumber: number }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetSalvagesByClaim(branchId, prefixId, policyDocumentNumber ,number) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Claims/Charge/GetSalvagesByClaim',
            data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyDocumentNumber: policyDocumentNumber, claimNumber: number }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetSalvageBySalvageId(salvageId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Claims/Charge/GetSalvageBySalvageId',
            data: JSON.stringify({ salvageId: salvageId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    } 

    static GetRecoveryByRecoveryId(recoveryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Claims/Charge/GetRecoveryByRecoveryId',
            data: JSON.stringify({ recoveryId: recoveryId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    } 

    static GetPaymentConceptsByBranchIdMovementTypeId(branchId, movementTypeId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Claims/Charge/GetPaymentConceptsByBranchIdMovementTypeId',
            data: JSON.stringify({ branchId: branchId, movementTypeId: movementTypeId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    } 

    static SaveChargeRequest(chargeRequestsModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Claims/Charge/SaveChargeRequest',
            data: JSON.stringify({ chargeRequestsDTO: chargeRequestsModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetHolderByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/Charge/GetHolderByIndividualId",
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetChargeRequestByChargeRequestId(chargeRequestId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/Charge/GetChargeRequestByChargeRequestId",
            data: JSON.stringify({ chargeRequestId: chargeRequestId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/Charge/GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber",
            data: JSON.stringify({ prefixId: prefixId, branchId: branchId, policyDocumentNumber: policyDocumentNumber, claimNumber: claimNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}