class AjaxReassignPolicies {

    /**
    * @summary
    *   Solicitud ajax para la consulta de los estados
    */
    static GetStatus() {
        return $.ajax({
            type: "GET",
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetAllStatus",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    //GetUsers
    static GetUsers() {
        return $.ajax({
            type: "GET",
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetUsers",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    /**
    * @summary
    *   Solicitud ajax para la consulta del grupo de politicas
    */
    static GetGroupPolicies() {
        return $.ajax({
            type: "GET",
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetGroupPolicies",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetUsersAuthorization(groupId, policiesId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetUsersAuthorization",
            data: JSON.stringify({ groupId: groupId, policiesId, policiesId}),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    /**
    * @summary 
    *   Solicitud ajax para la consulta de las politicas
    * @param {number} status estado de la politica
    * @param {Date} dateInit fecha Inicial
    * @param {Date} dateEnd fecha final
    * @return {Object<AuthorizationRequestGroups>} respuesta de la peticion
    */
    static GetAuthorizationRequestGroups(groupPolicies, policies, idUser, userAuthorization, dateInit, dateEnd) {
        return $.ajax({
            type: "POST",
            data: { "groupPolicies": groupPolicies, "policies": policies, "idUser": idUser, "userAuthorization": userAuthorization, "strDateInit": dateInit, "strDateEnd": dateEnd },
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetAuthorizationRequestPendingGroups"
        });
    }

    static GetPoliciesByGroupPolicies(idGroup) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetPoliciesByGroupPolicies",
            data: JSON.stringify({ groupId: idGroup }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetDetailsAuthorizationRequestGroups(key, policiesId, idUser) {
        return $.ajax({
            type: "POST",
            data: { "key": key, "policiesId": policiesId, "idUser":idUser},
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetDetailsAuthorizationRequestGroups"
        });
    }

    static GetAuthorizationAnswersByRequestId(requestId) {
        return $.ajax({
            type: "POST",
            data: { "requestId": requestId },
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetAuthorizationAnswersByRequestId"
        });
    }


    static GetHierarchyByGroupPolicies(groupId) {
        return $.ajax({
            type: "POST",
            data: { "groupId": groupId },
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetHierarchyByGroupPolicies"
        });
    }

    static GetUsersAuthorizationHierarchy(policiesid, hierarchyId, userId) {
        return $.ajax({
            type: "POST",
            data: { "policiesid": policiesid, "hierarchyId": hierarchyId, "userId": userId},
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/GetUsersAuthorizationHierarchy",
            async: true
        });
    }


    static SaveReassign(policiesId, userAnswerId, key, hierarchyId, userReasignId, reason, policiesToReassign) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "AuthorizationPolicies/ReassignPolicies/ReasignAuthorizationAnswer",
            data: JSON.stringify({
                policiesId: policiesId, userAnswerId: userAnswerId, key: key, hierarchyId: hierarchyId,
                userReasignId: userReasignId, reason: reason, policiesToReassign: policiesToReassign}),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

}

class ReassignPolicies extends Uif2.Page{

    getInitialState() {
        this.LoadGroupPolicies();
        //this.GetUsersAuthorization();
        this.GetUsers();

        $("#dateInitPolicies").UifDatepicker();
        $("#dateEndPolicies").UifDatepicker();
        $("#tableDetailsRequests").UifDataTable({ widthColumns: [{ width: "5%", column: 1 }], hiddenColumns: [0] });
        $("#btnback").hide();
        $("#btnExitPolicyReassign").on("click", () => window.location = rootPath + "Home/Index");
    }

    bindEvents() {
        $("#SearchReassignPolicies").on("click", this.SearchReassignPolicies.bind(this));
        $("#dateInitPolicies").on("datepicker.change", (e, dateStart) => {
            const dateEnd = $("#dateEndPolicies").UifDatepicker("getValue");
            if (dateEnd < dateStart) {
                $("#dateEndPolicies").UifDatepicker("setValue", dateStart);
            }
        });
        $("#dateEndPolicies").on("datepicker.change", (e, dateEnd) => {
            const dateStart = $("#dateInitPolicies").UifDatepicker("getValue");
            if (dateEnd < dateStart) {
                $("#dateInitPolicies").UifDatepicker("setValue", dateEnd);
            }
        });
        $("#btnSearchDetailsReassign").on("click", this.SearchDetails.bind(this));
        $("#modalRequestDetails").on("modal.opened", () => {
            $("#tableDetailsRequests").UifDataTable({ widthColumns: [{ width: "5%", column: 1 }], hiddenColumns: [0] });
        });
        $("#btnback").on("click", this.BackModal);
        $("#btnSearchDetailsAnswerReassign").on("click", this.SearchDetailsReassingAnswer);
        $("#SelectGroupPolicies").on('itemSelected', this.ChangePolicies);
        $("#btnSearchReassign").on("click", this.SearchReassing.bind(this));
        $("#SelectHierarchy").on('itemSelected', this.ChangeUsers);
        $("#btnAssign").click(this.SaveReassign.bind(this));
        $("#SelectPolicies").on('itemSelected', this.ChangeUsersAuthorization);
        
        


    }

    BackModal() {
        $("#btnback").hide();
        $("#btnAssign").hide();
        $("#btnSearchDetailsAnswerReassign").show();
        $("#btnSearchReassign").hide();
        $("#divTableDetailsRequests").show();
        $("#divTableAnswersReassign").hide();
        $("#divTableReassign").hide();
    }

    // Cargar combo de grupo de politicas
    LoadGroupPolicies() {
        AjaxReassignPolicies.GetGroupPolicies().done(function (data) {
            if (data) {
                $("#SelectGroupPolicies").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify("show", { 'type': "info", 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadHierarchy(groupId) {

        AjaxReassignPolicies.GetHierarchyByGroupPolicies(groupId, 0).done(function (dataResult) {
            if (dataResult.success) {
                $("#SelectHierarchy").UifSelect({ sourceData: dataResult.result });
            }
        });
    }

    ChangeUsers(event, selectedItem) {
        if (selectedItem != null) {
            var select = $("#SelectPolicies").UifSelect("getSelectedSource").IdPolicies;
            var selectUser = $("#SelectAuthorizingUser").UifSelect("getSelectedSource").UserId;
            AjaxReassignPolicies.GetUsersAuthorizationHierarchy(select, selectedItem.Id, selectUser).done(function (data) {
                if (data.success) {
                    var users = data.result.map((x) => { return x.User });
                    $("#SelectUserAuth").UifSelect({
                        sourceData: users,
                        id: "UserId",
                        name: "AccountName"
                    });
                }
            });
        }
        else {
            $("#SelectUserAuth").UifSelect();
        }
    }

    SearchReassignPolicies() {
        const dataForm = this.GetForm();
        if (dataForm !== false) {
            AjaxReassignPolicies.GetAuthorizationRequestGroups(dataForm.groupPolicies.IdGroupPolicies, dataForm.policies.IdPolicies, dataForm.idUser.UserId, dataForm.userAuthorization.UserId, dataForm.dateStart, dataForm.dateEnd).done((data) => {
                if (data.success) {
                    $("#TablePendingRequests").UifDataTable("clear");
                    data.result.forEach((item) => {
                        item.DateRequest = FormatDate(item.DateRequest);
                        $("#TablePendingRequests").UifDataTable("addRow", item);
                    });
                }
                else {
                    $.UifNotify("show", { 'type': "info", 'message': Resources.Language.QueryNotData, 'autoclose': true });
                }
            });
        }
    }

    SearchDetails() {
        const dataForm = this.GetForm();
        let selected = $("#TablePendingRequests").UifDataTable("getSelected");
        if (selected !== null) {
            selected = selected[0];
            this.BackModal();
            AjaxReassignPolicies.GetDetailsAuthorizationRequestGroups(selected.Key, selected.PoliciesId, dataForm.idUser.UserId ).done((data) => {
                if (data.success) {
                    $("#tableDetailsRequests").UifDataTable("clear");
                    data.result.forEach((item) => {
                        $("#tableDetailsRequests").UifDataTable("addRow", item);
                    });
                    $("#modalRequestDetails").UifModal("showLocal", `${selected.DescriptionPolicie} - ${selected.Reference}`);
                }
                else {
                    $.UifNotify("show", { 'type': "info", 'message': Resources.Language.QueryNotData, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.SelectItemList, 'autoclose': true });
        }
    }

    SearchDetailsReassingAnswer() {
        let selected = $("#tableDetailsRequests").UifDataTable("getSelected");

        if (selected !== null) {
            selected = selected[0];
            $("#btnback").hide();
            $("#btnAssign").hide();
            $("#btnSearchDetailsAnswerReassign").hide();
            $("#btnSearchReassign").show();
            $("#divTableDetailsRequests").hide();
            $("#divTableAnswersReassign").show();
            $("#divTableReassign").hide();

            AjaxReassignPolicies.GetAuthorizationAnswersByRequestId(selected.AuthorizationRequestId).done(data => {
                if (data.success) {
                    $("#tableReassignAnswers").UifDataTable("clear");
                    data.result.forEach((item) => {
                        item.DateAnswer = FormatDate(item.DateAnswer);
                        $("#tableReassignAnswers").UifDataTable("addRow", item);
                    });
                } else {
                    $.UifNotify("show", { 'type': "info", 'message': Resources.Language.SelectItemList, 'autoclose': true });//QueryNotData
                }
            });
        } else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.SelectItemList, 'autoclose': true });
        }
    }

    Clean() {
        $("#SelectUserAuth").UifSelect();
        $("#inputReallocationReason").val("");
    }

    SearchReassing() {
        let selected = $("#tableReassignAnswers").UifDataTable("getSelected");
        if (selected !== null) {
            selected = $("#TablePendingRequests").UifDataTable("getSelected")[0];
            $("#btnback").hide();
            $("#btnAssign").show();
            $("#btnSearchDetailsAnswerReassign").hide();
            $("#btnSearchReassign").hide();
            $("#divTableDetailsRequests").hide();
            $("#divTableAnswersReassign").hide();
            $("#divTableReassign").show();
            this.Clean();

            ReassignPolicies.LoadHierarchy(selected.GroupPoliciesId);
            
        } else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.SelectItemList, 'autoclose': true });
        }
    }

    ChangePolicies(event, selectedItem) {
        selectedItem = $("#SelectGroupPolicies").UifSelect("getSelectedSource");
        if (selectedItem != null) {
            AjaxReassignPolicies.GetPoliciesByGroupPolicies(selectedItem.IdGroupPolicies).done(function (data) {
                if (data.success) {
                    $("#SelectPolicies").UifSelect({
                        sourceData: data.result,
                        id: "IdPolicies",
                        name: "Description"
                    });
                }
            });
        }
        else {
            $("#SelectPolicies").UifSelect();
        }
    }

    // Cargar combo de usuarios autorizadores
    ChangeUsersAuthorization(event, selectedItem) {
        if (selectedItem != null) {
            var selectGroup = $("#SelectGroupPolicies").UifSelect("getSelectedSource");
           
            AjaxReassignPolicies.GetUsersAuthorization(selectGroup.IdGroupPolicies, selectedItem.Id).done(function (data) {
                if (data.success) {
                    $("#SelectAuthorizingUser").UifSelect({
                        sourceData: data.result,
                        id: "UserId",
                        name: "AccountName"
                    });
                }
            });
        }
        else {
            $("#SelectAuthorizingUser").UifSelect();
        }
    }

    GetUsers() {
        AjaxReassignPolicies.GetUsers().done(function (data) {
            if (data) {
                $("#SelectIssuingUser").UifSelect({
                    sourceData: data.result,
                    id: "UserId",
                    name: "AccountName"
                });
            }
            else {
                $.UifNotify("show", { 'type': "info", 'message': data.result, 'autoclose': true });
            }
        });
    }

    SaveReassign() {

        const info = {};
      
        info.groupPolicies = $("#SelectGroupPolicies").UifSelect("getSelectedSource");
        info.policies = $("#SelectPolicies").UifSelect("getSelectedSource");
        info.idUser = $("#SelectIssuingUser").UifSelect("getSelectedSource");
        info.userAuthorization = $("#SelectAuthorizingUser").UifSelect("getSelectedSource");
        var hierarchyId = $("#SelectHierarchy").UifSelect("getSelectedSource");
        var userAuth = $("#SelectUserAuth").UifSelect("getSelectedSource");
        var reason = $("#inputReallocationReason").val();
        let selected = $("#TablePendingRequests").UifDataTable("getSelected");

        if (hierarchyId === undefined) {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.HierarchyRequired, 'autoclose': true });
            return false;
        }
        if (userAuth === undefined) {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.AuthorizingUserRequired, 'autoclose': true });
            return false;
        }
        if (reason === "") {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ReasonRequired, 'autoclose': true });
            return false;
        }
        AjaxReassignPolicies.SaveReassign(info.policies.IdPolicies, info.userAuthorization.UserId, selected[0].Key, hierarchyId.Id, userAuth.UserId, reason, null).done((result) => {
            if (result.success) {
                $.UifNotify('show', { 'type': 'info', 'message': result.result, 'autoclose': true });
                $("#divTableDetailsRequests").hide();
                $("#divTableAnswersReassign").hide();
                $("#divTableReassign").hide();
                $("#modalRequestDetails").modal('hide');
                this.SearchReassignPolicies();
                $("#SelectPolicies").trigger('itemSelected', { Id: $("#SelectPolicies").UifSelect("getSelected")});
            }
        });
    }

    GetForm() {
        const data = {};
        data.dateStart = $("#dateInitPolicies").val();
        data.dateEnd = $("#dateEndPolicies").val();
        data.groupPolicies = $("#SelectGroupPolicies").UifSelect("getSelectedSource");
        data.policies = $("#SelectPolicies").UifSelect("getSelectedSource");
        data.idUser = $("#SelectIssuingUser").UifSelect("getSelectedSource");
        data.userAuthorization = $("#SelectAuthorizingUser").UifSelect("getSelectedSource");

        if (data.dateStart === "") {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorInitialSelectionDate, 'autoclose': true });
            return false;
        }
        if (data.dateEnd === "") {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PolicyReportErrorFinalSelectionDate, 'autoclose': true });
            return false;
        }
        if (data.groupPolicies === undefined) {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.GroupPoliciesRequired, 'autoclose': true });
            return false;
        }
        if (data.policies === undefined) {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.PoliciesRequired, 'autoclose': true });
            return false;
        }
        if (data.idUser === undefined) {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.IssuingUserRequired, 'autoclose': true });
            return false;
        }
        if (data.userAuthorization === undefined) {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.AuthorizingUserRequired, 'autoclose': true });
            return false;
        }
        return data;
    }

}