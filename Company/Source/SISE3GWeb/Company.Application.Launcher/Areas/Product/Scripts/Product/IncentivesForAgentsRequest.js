class IncentivesForAgentsRequest {

    static GetIncentivesByProductIdByIndividualIdByAgentAgencyID(productId, indivicualId, AgentAgencyId) {
        return $.ajax({
            type: 'POST',
            url: 'GetIncentivesByProductIdByIndividualIdByAgentAgencyID',
            data: JSON.stringify({ productId: productId, indivicualId: indivicualId , AgentAgencyId:AgentAgencyId  }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });

    }

 

}