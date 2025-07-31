class AgentRequestT {

    static SaveAgents(listCiaParamAgentServiceModel, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/SaveAgents',
            async: true,
            data: { "listCiaParamAgentServiceModel": listCiaParamAgentServiceModel, "productId": productId }
        });
    }

    static SaveAllAgents(prefixId, productId, assigned) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/SaveAllAgents',
            data: JSON.stringify({ prefixId: prefixId, productId: productId, assigned: assigned }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProductAgentByProductIdByIndividualId(productId, individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetProductAgentByProductIdByIndividualId',
            data: JSON.stringify({ productId: productId, individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgenciesByAgentIdDescription(agentId, description, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetAgenciesByAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAgenciesByAgentCodeFullName(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetAgentByIndividualIdFullName',
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })

    }

    static GetAgents(productId, currentAgents, prefixId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Product/Product/GetAgents',
            data: JSON.stringify({
                productId: productId, currentAgents: currentAgents, prefixId: prefixId
            }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

}