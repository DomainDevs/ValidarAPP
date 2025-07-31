class WorkFlowPoliciesRequest {

    static GetEventGroups() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'AuthorizationPolicies/WorkFlowPolicies/GetEventGroups',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEventAuthorizationsByUserId(userId, eventGroupId, startDate, finishDate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/WorkFlowPolicies/GetEventAuthorizationsByUserId',
            data: JSON.stringify({ userId: userId, eventGroupId: eventGroupId, startDate: startDate, finishDate: finishDate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateEventAuthorizations(eventAuthorizations, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/WorkFlowPolicies/CreateEventAuthorizations',
            data: JSON.stringify({ eventAuthorizationDTOs: eventAuthorizations, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    //static ValidationAccessAndHierarchys() {
    //    return $.ajax({
    //        type: "POST",
    //        url: rootPath + 'AuthorizationPolicies/WorkFlowPolicies/ValidationAccessAndHierarchys',
    //        dataType: 'json',
    //        contentType: 'application/json; charset=utf-8'
    //    });
    //}

    static GetEventAuthorizationsByUserIdInitial(eventGroupId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/WorkFlowPolicies/GetEventAuthorizationsByUserIdInitial',
            data: JSON.stringify({  eventGroupId: eventGroupId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}