class PolicyRequest {
    static GetCurrentSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetTemporalById(temporalId,controller) {

        return $.ajax({
            type: 'POST',
            url: rootPath + controller  + '/GetTemporalById',
            data: JSON.stringify({ id: temporalId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetAgentByPolicyIdEndorsementId(policyId, endorsementId, premium) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "EndorsementBase" + '/GetAgentsByPolicyIdEndorsementId',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId, premium: premium }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}