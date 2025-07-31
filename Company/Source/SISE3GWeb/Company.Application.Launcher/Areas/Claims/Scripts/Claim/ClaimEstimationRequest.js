class ClaimEstimationRequest {

    static GetEstimationByClaimId(claimId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetEstimationByClaimId',
            data: JSON.stringify({ claimId: claimId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(riskId, occurrenceDate, companyParticipationPercentage) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage',
            data: JSON.stringify({ riskId: riskId, occurrenceDate: occurrenceDate, companyParticipationPercentage: companyParticipationPercentage }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStatusesByEstimationTypeId(estimationTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetStatusesByEstimationTypeId',
            data: JSON.stringify({ estimationTypeId: estimationTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetReasonsByStatusIdPrefixId(statusId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetReasonsByStatusIdPrefixId',
            data: JSON.stringify({ statusId: statusId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetActivePanelsByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetActivePanelsByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoverageDeductibleByCoverageId(coverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetCoverageDeductibleByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetThirdPartyByIndividualId(individualId)
    {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetThirdPartyByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetParticipantByParticipantId(participantId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetParticipantByParticipantId',
            data: JSON.stringify({ participantId: participantId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }    

    static GetStatuses() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetStatuses',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetReasonsByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetReasonsByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimDriverInformationByClaimCoverageId(claimCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimDriverInformationByClaimCoverageId',
            data: JSON.stringify({ claimCoverageId: claimCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimThirdPartyVehicleByClaimCoverageId(claimCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimThirdPartyVehicleByClaimCoverageId',
            data: JSON.stringify({ claimCoverageId: claimCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAffectedPropertyByClaimCoverageId(claimCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetAffectedPropertyByClaimCoverageId',
            data: JSON.stringify({ claimCoverageId: claimCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimedAmountByClaimCoverageId(claimCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimedAmountByClaimCoverageId',
            data: JSON.stringify({ claimCoverageId: claimCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredsByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetInsuredsByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(policyId, riskNum, coverageId, coverNum) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/SetClaimReserve/GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber',
            data: JSON.stringify({ policyId: policyId, riskNum: riskNum, coverageId: coverageId, coverNum: coverNum }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}