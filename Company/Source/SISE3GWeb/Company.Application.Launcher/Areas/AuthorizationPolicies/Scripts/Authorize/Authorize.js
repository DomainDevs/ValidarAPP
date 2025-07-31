var dropDownAccept = uif2.dropDown({
    source: rootPath + "AuthorizationPolicies/Authorize/PanelAccept",
    element: "#btnAceptar",
    direction: "top",
    align: "right",
    width: 380,
    height: 300
});
var dropDownReasign = uif2.dropDown({
    source: rootPath + "AuthorizationPolicies/Authorize/PanelReassign",
    element: "#btnReasignar",
    align: "right",
    direction: "top",
    width: 380,
    height: 380
});
var dropDownReject = uif2.dropDown({
    source: rootPath + "AuthorizationPolicies/Authorize/PanelReject",
    element: "#btnRechazar",
    align: "right",
    direction: "top",
    width: 380,
    height: 300
});
var SearchAdvPoliciesAut = uif2.dropDown({
    source: rootPath + "AuthorizationPolicies/Authorize/AdvancedSearch",
    element: "#btnSearchAdvPoliciesAut",
    align: "right",
    width: 550,
    height: 300
});

var listRisks = new Array();
class RequestAuthorization {
    /**
    * @summary 
    *   Solicitud ajax para la consulta de las politicas
    * @param {idGroup} id grupo de la politica
    * @param {idPolicies} id de la politica
    * @param {status} estado de la politica
    * @param {dateInit} fecha nicial 
    * @param {dateEnd} fecha final 
    * @param {sort} like nombre de la politica
    */
    static GetAuthorizationAnswerByFilter(idGroup, idPolicies, status, dateInit, dateEnd, sort) {
        return $.ajax({
            type: "POST",
            data: { "idGroup": idGroup, "idPolicies": idPolicies, "status": status, "strDateInit": dateInit, "strDateEnd": dateEnd, "sort": sort },
            url: rootPath + "AuthorizationPolicies/Authorize/GetAuthorizationAnswersByFilter"
        });
    }

    /**
     * @summary 
     *   Solicitud ajax para la consulta de las politicas
     * @param {idGroup} id grupo de la politica
     * @param {idPolicies} id de la politica
     * @param {status} estado de la politica
     * @param {dateInit} fecha nicial 
     * @param {dateEnd} fecha final 
     * @param {sort} like nombre de la politica
     */
    static GetAuthorizationAnswersReasignByFilter(idGroup, idPolicies, dateInit, dateEnd, sort) {
        return $.ajax({
            type: "POST",
            data: { "idGroup": idGroup, "idPolicies": idPolicies, "strDateInit": dateInit, "strDateEnd": dateEnd, "sort": sort },
            url: rootPath + "AuthorizationPolicies/Authorize/GetAuthorizationAnswersReasignByFilter"
        });
    }

    /**
  * @summary 
  *   Solicitud ajax consultar las jerarquias superiores parametrizadas a la politica
  * @param {policiesId} id de la politica
  * @param {hierarchyId} jerarquia del usuario actual
  * @param {userId} id del usuario actual
  */
    static GetAuthorizationHierarchy(policiesId, hierarchyId, userId) {
        return $.ajax({
            type: "POST",
            data: { "policiesId": policiesId, "hierarchyId": hierarchyId, "userId": userId },
            url: rootPath + "AuthorizationPolicies/Authorize/GetAuthorizationHierarchy"
        });
    }



    /**
    * @summary 
    * Solicitud ajax para reasignar una politica
    * @param {authorizationAnswerId} id de la autorizacion
    * @param {hierarchyId} jerarquia reasignada
    * @param {userId} id del usuario reasignado
    * @param {reason} razon de la reasignacion
   **/
    static SendReasingAuthorization(policiesId, userAnswerId, key, hierarchyId, userReasignId, reason, policiesToReassign, functionType) {
        return $.ajax({
            type: "POST",
            data: {
                "policiesId": policiesId,
                "userAnswerId": userAnswerId,
                "key": key,
                "hierarchyId": hierarchyId,
                "userReasignId": userReasignId,
                "reason": reason,
                "policiesToReassign": policiesToReassign,
                "functionType": functionType
            },
            url: rootPath + "AuthorizationPolicies/Authorize/ReasingAuthorization"
        });
    }

    /**
   * @summary 
   * Solicitud ajax para Rechazar una  autorizacion pendiente
   * @param {authorizationsAnswer} id de la autorizacion
   * @param {reason} razon del rechazo
    **/
    static SendRejectAuthorization(policiesId, userAnswerId, key, reason, idRejection, policiesToReject, functionType) {
        return $.ajax({
            type: "POST",
            data: {
                "policiesId": policiesId,
                "userAnswerId": userAnswerId,
                "key": key,
                "reason": reason,
                "idRejection": idRejection,
                "policiesToReject": policiesToReject,
                "functionType": functionType
            },
            url: rootPath + "AuthorizationPolicies/Authorize/RejectAuthorization"
        });
    }

    /**
    * @summary 
    * Solicitud ajax para aceptar una  autorizacion pendiente
    * @param {authorizationsAnswer} id de la autorizacion
    * @param {reason} razon de la aceptacion
   **/
    static SendAcceptAuthorization(policiesId, userAnswerId, key, reason, policiesToAccept, functionType) {
        return $.ajax({
            type: "POST",
            data: {
                "policiesId": policiesId,
                "userAnswerId": userAnswerId,
                "key": key,
                "reason": reason,
                "policiesToAccept": policiesToAccept,
                "functionType": functionType
            },
            url: rootPath + "AuthorizationPolicies/Authorize/AcceptAuthorization"
        });
    }

    /**
     * @summary 
     * peticion ajax que obtiene el historial de las autorizaciones
    **/
    static GetHistoryReasign(policiesId, userAnswerId, key) {
        return $.ajax({
            type: "POST",
            data: {
                "policiesId": policiesId, "userAnswerId": userAnswerId, "key": key
            },
            url: rootPath + "AuthorizationPolicies/Authorize/GetHistoryReasign"
        });
    }

    static GetAuthorizationAnswerDescriptions(idPolicies, idUser, status, key) {
        return $.ajax({
            type: "POST",
            data: {
                "idPolicies": idPolicies,
                "idUser": idUser,
                "status": status,
                "key": key
            },
            url: rootPath + "AuthorizationPolicies/Authorize/GetAuthorizationAnswerDescriptions"
        });
    }

    static GetAuthorizationAnswerDescription(idPolicies, key) {
        return $.ajax({
            type: "POST",
            data: {
                "idPolicies": idPolicies,
                "key": key
            },
            url: rootPath + "AuthorizationPolicies/Authorize/GetAuthorizationAnswerDescription"
        });
    }

    static GenerateFileToExport(idPolicies, idUser, status, key) {
        return $.ajax({
            type: "POST",
            data: {
                "idPolicies": idPolicies,
                "idUser": idUser,
                "status": status,
                "key": key
            },
            url: rootPath + "AuthorizationPolicies/Authorize/GenerateFileToExport"
        });
    }

    static GetAuthorizeOption() {
        return $.ajax({
            type: "POST",
            url: rootPath + "AuthorizationPolicies/Authorize/GetAuthorizeOption"
        });
    }

    static GetRejectOption() {
        return $.ajax({
            type: "POST",
            url: rootPath + "AuthorizationPolicies/Authorize/GetRejectOption"
        });
    }

    static GetReassignOption() {
        return $.ajax({
            type: "POST",
            url: rootPath + "AuthorizationPolicies/Authorize/GetReassignOption"
        });
    }



    static GetRejectionCauses(groupPolicyId) {
        return $.ajax({
            type: "POST",
            data: {
                "groupPolicyId": groupPolicyId
            },
            url: rootPath + "ParamAuthorizationPolicies/GroundsRejection/GetRejectionCausesByGroupPolicyId"

        });
    }
}

class AuthorizationPolicies extends Uif2.Page {

    getInitialState() {
        $("#ddlAdvPolicies").UifSelect();

        $("#ddlStatusPolicies").UifSelect({
            sourceData: [
                { Id: 1, Description: "Pendiente" }, { Id: 3, Description: "Rechazado" },
                { Id: 2, Description: "Autorizado" }, { Id: -1, Description: "Reasignado" }
            ],
            id: "Id",
            name: "Description"
        });
        $("#ddlStatusPolicies").UifSelect("setSelected", "1");

        $("#ddlHierarchy").UifSelect();
        $("#ddlHierarchyUser").UifSelect();

        $("#tableHistory").UifDataTable();
        $("#lsvPoliciesAuthorization").UifListView({
            selectionType: "single",
            displayTemplate: "#display-template-PoliciesAuthorization"
        });


        $("#txtDateInit").UifDatepicker("setValue", new Date());
        $("#txtDateEnd").UifDatepicker("setValue", new Date());

        if (GetQueryParameter("key") !== undefined && GetQueryParameter("IdPolicies") !== undefined) {
            let selectedId = $("#ddlStatusPolicies").UifSelect("getSelected");
            AuthorizationPolicies.ShowPanels(selectedId);
            AuthorizationPolicies.ClearFormPolicies();
            if (selectedId) {
                AuthorizationPolicies.SetListPolicies(null, GetQueryParameter("IdPolicies"), selectedId, null, null, GetQueryParameter("key"), true);
            } else {
                $("#lsvPoliciesAuthorization").UifListView("refresh");
            }
        } else {
            AuthorizationPolicies.ChangeStatusPolicies();
        }

        RequestAuthorization.GetAuthorizeOption().done(function (data) {
            if (data.success) {
                $('#selectPoliciesToAcceptOption').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        RequestAuthorization.GetRejectOption().done(function (data) {
            if (data.success) {
                $('#selectPoliciesToRejectOption').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        RequestAuthorization.GetReassignOption().done(function (data) {
            if (data.success) {
                $('#selectPoliciesToReassignOption').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        RequestGroupsPolicies.GetGroupsPolicies().done(function (data) {
            if (data.success) {
                $("#ddlAdvGroupPolicies").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $("#btnAceptar").hide();
        $("#btnReasignar").hide();
        $("#btnRechazar").hide();
        $("#btnPrinting").hide();
        $("#btnExport").hide();
    }

    bindEvents() {
        $("#lsvPoliciesAuthorization").on("itemSelected", AuthorizationPolicies.SetFormPolicies);
        $("#ddlStatusPolicies").on("itemSelected", this.ChangeStatus);
        $("#btnAceptar").on("click", this.ShowAccepPanel);
        $("#btnReasignar").on("click", this.ShowReasignPanel);
        $("#btnRechazar").on("click", this.ShowRejectPanel);
        $("#btnExport").on("click", this.GenerateFileToExport);

        $("#btnSendReassign").on("click", this.SendReassign);
        $("#btnSendReject").on("click", this.SendReject);
        $("#btnSendAccept").on("click", this.SendAccept);
        $("#btnHistory").on("click", this.ShowModalHistory);
        $("#txtSearchAuthorization").on("buttonClick", this.SearchAuthorization);

        $('#selectPoliciesToAcceptOption').on('itemSelected', this.ShowPoliciesToAcceptOption);
        $('#selectPoliciesToRejectOption').on('itemSelected', this.ShowPoliciesToRejectOption);
        $('#selectPoliciesToReassignOption').on('itemSelected', this.ShowPoliciesToReassignOption);

        $('#inputPoliciesToAcceptRangeFrom').on('change', () => { this.ChangePoliciesRangeFrom(listRisks.length, "#inputPoliciesToAcceptRangeFrom", "#inputPoliciesToAcceptRangeTo") });
        $('#inputPoliciesToAcceptRangeTo').on('change', () => { this.ChangePoliciesRangeTo(listRisks.length, "#inputPoliciesToAcceptRangeFrom", "#inputPoliciesToAcceptRangeTo") });
        $('#inputPoliciesToRejectRangeFrom').on('change', () => { this.ChangePoliciesRangeFrom(listRisks.length, "#inputPoliciesToRejectRangeFrom", "#inputPoliciesToRejectRangeTo") });
        $('#inputPoliciesToRejectRangeTo').on('change', () => { this.ChangePoliciesRangeTo(listRisks.length, "#inputPoliciesToRejectRangeFrom", "#inputPoliciesToRejectRangeTo") });
        $('#inputPoliciesToReassignRangeFrom').on('change', () => { this.ChangePoliciesRangeFrom(listRisks.length, "#inputPoliciesToReassignRangeFrom", "#inputPoliciesToReassignRangeTo") });
        $('#inputPoliciesToReassignRangeTo').on('change', () => { this.ChangePoliciesRangeTo(listRisks.length, "#inputPoliciesToReassignRangeFrom", "#inputPoliciesToReassignRangeTo") });

        $("#inputPoliciesToAccept").on('keypress', AuthorizationPolicies.checkKey);
        $("#inputPoliciesToReassign").on('keypress', AuthorizationPolicies.checkKey);
        $("#inputPoliciesToReject").on('keypress', AuthorizationPolicies.checkKey);

        $("#btnSearchAdvPoliciesAut").on("click", this.ShowSearchAdvPoliciesAut);
        $("#ddlAdvGroupPolicies").on("itemSelected", this.SetPoliciesByGroup);

        $("#btnCancelSearchAdv").on("click", () => SearchAdvPoliciesAut.hide());
        $("#btnSearchAdv").on("click", this.SearchAdvPoliciesAut);
        $("#btnPrinting").on("click", this.Print);
        $("#ddlHierarchy").on("itemSelected", this.GetUsersHierarchy);

    }

    Print() {
        let selected = $("#lsvPoliciesAuthorization").UifListView("getSelected");
        if (selected.length === 1) {
            $.ajax({
                type: 'POST',
                url: rootPath + 'Printing/Printer/GetSummaryByTemporalId',
                data: JSON.stringify({ temporalId: selected[0].Key, temporalAutho: true }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8'
            }).done(function (data) {
                if (data.success) {
                    $.ajax({
                        type: 'POST',
                        async: true,
                        url: rootPath + 'Printing/Printer/GenerateReportTemporary',
                        data: {
                            temporaryId: data.result.TempId,
                            prefixId: data.result.CommonProperties.PrefixId,
                            riskSince: data.result.CommonProperties.RiskSince,
                            riskUntil: data.result.CommonProperties.RiskUntil,
                            operationId: data.result.OperationId,
                            tempAuthorization: true
                        }
                    }).done(function (data1) {
                        if (data1.success) {
                            DownloadFile(data1.result.Url, true, () => { return data.result.OperationId });
                        } else {
                            if (data1.result == Resources.Language.EndorsmentNotReinsured) {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotPrinter + ": " + data1.result, 'autoclose': true });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data1.result, 'autoclose': true });
                            }
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchPolicy, 'autoclose': true });
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
            });
        }
    }

    static SelectedAutorization(key, idPolicies, isInit) {
        const index = $("#lsvPoliciesAuthorization").UifListView("findIndex", (x) => { return x.PoliciesId === idPolicies && x.Key === key });
        if (index !== -1) {
            $("#lsvPoliciesAuthorization").UifListView("setSelected", index, true);
            const data = $("#lsvPoliciesAuthorization").UifListView("getSelected")[0];
            AuthorizationPolicies.SetFormPolicies(null, data);
        }

        if (isInit) {
            AuthorizationPolicies.GetAuthorization(key, idPolicies);
        }
    }

    static GetAuthorization(key, idPolicies) {
        RequestAuthorization.GetAuthorizationAnswerDescription(idPolicies, key).done(data => {
            if (data.success) {
                if (data.result.length > 0) {
                    var item = AuthorizationPolicies.FormatMessage(data.result[0]);
                    $.UifNotify('show', { 'type': 'info', 'message': item, 'autoclose': true });
                }
            }
        });
    }

    static FormatMessage(Message) {

        let array = Message.split("|");
        if (array[0] == 2)
            return Resources.Language.AuthorizedPolicie + array[1] + " con fecha:" + array[2];
        else
            return Resources.Language.DenyPolitics + array[1] + " con fecha:" + array[2];

    }
    /**
     * @summary 
     * realiza la busqueda avanzada de las politicas
     */
    SearchAdvPoliciesAut() {
        let idGroup = $("#ddlAdvGroupPolicies").UifSelect("getSelected");
        let idPolicies = $("#ddlAdvPolicies").UifSelect("getSelected");
        let dateInit = $("#txtDateInit").val();
        let dateEnd = $("#txtDateEnd").val();
        let status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));

        SearchAdvPoliciesAut.hide();
        AuthorizationPolicies.SetListPolicies(idGroup, idPolicies, status, dateInit, dateEnd, "");
    }

    /**
     * @summary 
     * evento que muestra la busqueda avanzada de las politicas
     */
    ShowSearchAdvPoliciesAut() {
        $("#ddlAdvGroupPolicies").UifSelect("setSelected", "");
        $("#ddlAdvPolicies").UifSelect({
            native: false,
            filter: true
        });
        $("#txtDateInit").UifDatepicker("setValue", new Date());
        $("#txtDateEnd").UifDatepicker("setValue", new Date());
        SearchAdvPoliciesAut.show();
    }

    /**
     * @summary 
     * evento que setea el ddl de las politicas Adv
     * @param {} event 
     * @param {item seleccionado} item 
     * @returns {} 
     */
    SetPoliciesByGroup(event, item) {
        if (!item.Id) {
            item.Id = -1;
        }

        RequestPolicies.GetPoliciesByIdGroup(item.Id).done((data) => {
            if (data.success) {
                $("#ddlAdvPolicies").UifSelect({
                    sourceData: data.result,
                    native: false,
                    filter: true
                });
            }
        });
    }

    /**
     * @summary 
     * 
     * @param {event}
     * @param {value} valor buscado
     */
    SearchAuthorization(event, value) {
        AuthorizationPolicies.ClearFormPolicies();
        let status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
        AuthorizationPolicies.SetListPolicies(null, null, status, null, null, value);
    }

    /**
     *@summary 
     * levanta el modal con el resumen de las autorizaciones reasignadas 
     **/
    ShowModalHistory() {
        let index = $("#hdnAutorizatioIndex").val();
        let policie = $("#lsvPoliciesAuthorization").UifListView("getData")[index];

        RequestAuthorization.GetHistoryReasign(policie.PoliciesId, policie.UserAnswerId, policie.Key)
            .done((data) => {
                if (data.success) {
                    $("#tableHistory").UifDataTable("clear");

                    for (let i = 0; i < data.result.length; i++) {
                        let item = {
                            user: data.result[i].UserAnswer.AccountName,
                            hierarchy: data.result[i].HierarchyAnswer.Description,
                            description: data.result[i].DescriptionReasign,
                            date: FormatDate(data.result[i].DateReasign),
                            state: "Pendiente",
                            action: "Reasignado"
                        };


                        $("#tableHistory").UifDataTable("addRow", item);
                    }
                    let item = {
                        user: data.result[data.result.length - 1].UserReasign.AccountName,
                        hierarchy: data.result[data.result.length - 1].HierarchyReasign.Description,
                        description: data.result[data.result.length - 1].AuthorizationAnswer.DescriptionAnswer,
                        date: FormatDate(data.result[data.result.length - 1].AuthorizationAnswer.DateAnswer)
                    };

                    switch (data.result[0].AuthorizationAnswer.Status) {
                        case TypeStatusPolicies.Authorized:
                            item.state = "Autorizada";
                            item.action = "Autorizada";
                            break;
                        case TypeStatusPolicies.Rejected:
                            item.state = "Rechazada";
                            item.action = "Rechazada";
                            break;
                        case TypeStatusPolicies.Pending:
                            item.action = "";
                            item.state = "Pendiente";
                            break;
                    }
                    $("#tableHistory").UifDataTable("addRow", item);
                    $("#modalHistory").UifModal("showLocal", "Historial de las reasignaciones");

                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
    }

    /**
     * @summary 
     * Evento que envia el formulario de reasignacion de autorizacion
     **/
    SendReassign() {
        let index = $("#hdnAutorizatioIndex").val();
        let hierarchy = $("#ddlHierarchy").UifSelect("getSelected");
        let user = $("#ddlHierarchyUser").UifSelect("getSelected");
        let reason = $("#txtReasonReasign").val();

        if ($("#formReassign").valid()) {
            if (index && hierarchy && user && reason) {
                var policiesToReassign = AuthorizationPolicies.policiestoProcess($("#selectPoliciesToReassignOption").UifSelect("getSelected"), $('#inputPoliciesToReassignRangeFrom').val() - 1, $('#inputPoliciesToReassignRangeTo').val() - 1, $('#inputPoliciesToReassign').val());
                if (policiesToReassign !== undefined) {
                    const policies = $("#lsvPoliciesAuthorization").UifListView("getData")[index];
                    RequestAuthorization.SendReasingAuthorization(policies.PoliciesId,
                        policies.UserAnswerId,
                        policies.Key,
                        hierarchy,
                        user,
                        reason,
                        policiesToReassign,
                        policies.FunctionType)
                        .done((data) => {
                            if (data.success) {
                                $("#lsvPoliciesAuthorization").UifListView("deleteItem", index);
                                let status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
                                let value = $("#txtSearchAuthorization").val();

                                AuthorizationPolicies.ClearFormPolicies();
                                AuthorizationPolicies.HidePanels();
                                AuthorizationPolicies.SetListPolicies(null, null, status, null, null, value);
                                $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                            } else {
                                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                            }
                        });
                }
            }
        }
    }

    /**
    * @summary 
    * Evento que envia el formulario para aceptar una politica
    **/
    SendAccept() {
        let index = $("#hdnAutorizatioIndex").val();
        let reason = $("#txtReasonAccept").val();

        if ($("#formAccept").valid()) {
            if (reason) {
                var policiesToAccept = AuthorizationPolicies.policiestoProcess($("#selectPoliciesToAcceptOption").UifSelect("getSelected"), $('#inputPoliciesToAcceptRangeFrom').val() - 1, $('#inputPoliciesToAcceptRangeTo').val() - 1, $('#inputPoliciesToAccept').val());
                if (policiesToAccept !== undefined) {
                    const policies = $("#lsvPoliciesAuthorization").UifListView("getData")[index];
                    RequestAuthorization
                        .SendAcceptAuthorization(policies.PoliciesId, policies.UserAnswerId, policies.Key, reason, policiesToAccept, policies.FunctionType)
                        .done((data) => {
                            if (data.success) {
                                $("#lsvPoliciesAuthorization").UifListView("deleteItem", index);
                                let status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
                                let value = $("#txtSearchAuthorization").val();

                                AuthorizationPolicies.ClearFormPolicies();
                                AuthorizationPolicies.HidePanels();
                                AuthorizationPolicies.SetListPolicies(null, null, status, null, null, value);
                                $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                            } else {
                                if (data.result != null) {
                                    if (data.result.startsWith("**")) {
                                        //mostrar como cuadro de dialogo
                                        $.UifDialog('alert',
                                            { 'message': data.result },
                                            function (result) {
                                                //No hacer nada, sólo informativo
                                            });
                                        $('.modal-body.modal-body-dialog-alert p').prop('style', 'white-space: pre-line')
                                    } else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }
                                }
                                else {
                                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                                }
                            }
                        });
                }
            }
        }
    }

    /**
    * @summary 
    * Evento que envia el formulario para rechazar una autorizacion
    **/
    SendReject() {
        let index = $("#hdnAutorizatioIndex").val();
        let reason = $("#txtReasonReject").val();
        let idRejection = parseInt($("#RejectionCausesId").val());

        if ($("#formReject").valid()) {
            if (reason) {
                var policiesToReject = AuthorizationPolicies.policiestoProcess($("#selectPoliciesToRejectOption").UifSelect("getSelected"), $('#inputPoliciesToRejectRangeFrom').val() - 1, $('#inputPoliciesToRejectRangeTo').val() - 1, $('#inputPoliciesToReject').val());
                if (policiesToReject !== undefined) {
                    const policies = $("#lsvPoliciesAuthorization").UifListView("getData")[index];

                    RequestAuthorization
                        .SendRejectAuthorization(policies.PoliciesId,
                            policies.UserAnswerId,
                            policies.Key,
                            reason,
                            idRejection,
                            policiesToReject,
                            policies.FunctionType)
                        .done((data) => {
                            if (data.success) {
                                $("#lsvPoliciesAuthorization").UifListView("deleteItem", index);
                                let status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
                                let value = $("#txtSearchAuthorization").val();

                                AuthorizationPolicies.ClearFormPolicies();
                                AuthorizationPolicies.HidePanels();
                                AuthorizationPolicies.SetListPolicies(null, null, status, null, null, value);
                                $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                            } else {
                                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                            }
                        });
                }
            }
        }
    }

    /**
     *@summary 
     * Evento que obtiene las jerarquias autorizadoras superioes o iguales a la actual
     * @param {index} index de la autorizacion
     * @param {item} autorizacion seleccionada
     */
    GetUsersHierarchy(index, item) {
        AuthorizationPolicies.SetUsersAuthorizationHierarchy(item);
    }

    /**
     * @summary 
     * Evento que  muestra el panel de aceptar
     */
    ShowAccepPanel() {
        $("#selectPoliciesToAcceptOption").val("");
        $("#divIndividualAccept").hide();
        $("#divRangeAccept").hide();
        $("#inputPoliciesToAccept").val("");
        $("#inputPoliciesToAcceptRangeFrom").val("");
        $("#inputPoliciesToAcceptRangeTo").val("");
        if (listRisks.length == 1) {
            $('#selectPoliciesToAcceptOption').prop("disabled", true);
            $('#selectPoliciesToAcceptOption').val(1);
        } else {
            $('#selectPoliciesToAcceptOption').prop("disabled", false);
        }
        $("#txtReasonAccept").val("");

        AuthorizationPolicies.HidePanels();
        dropDownAccept.show();
    }

    /**
     * @summary 
     *  Evento que  muestra el panel de reasignar
     */
    ShowReasignPanel() {
        $("#selectPoliciesToReassignOption").val("");
        $("#divIndividualReassign").hide();
        $("#divRangeReassign").hide();
        $("#inputPoliciesToReassign").val("");
        $("#inputPoliciesToReassignRangeFrom").val("");
        $("#inputPoliciesToReassignRangeTo").val("");
        if (listRisks.length == 1) {
            $('#selectPoliciesToReassignOption').prop("disabled", true);
            $('#selectPoliciesToReassignOption').val(1);
        } else {
            $('#selectPoliciesToReassignOption').prop("disabled", false);
        }
        $("#txtReasonReasign").val("");

        AuthorizationPolicies.HidePanels();
        dropDownReasign.show();
    }

    /**
     * @summary 
     *  Evento que  muestra el panel de rechazar
     */
    ShowRejectPanel() {
        $("#selectPoliciesToRejectOption").val("");
        $("#divIndividualReject").hide();
        $("#divRangeReject").hide();
        $("#inputPoliciesToReject").val("");
        $("#inputPoliciesToRejectRangeFrom").val("");
        $("#inputPoliciesToRejectRangeTo").val("");
        if (listRisks.length == 1) {
            $('#selectPoliciesToRejectOption').prop("disabled", true);
            $('#selectPoliciesToRejectOption').val(1);
        } else {
            $('#selectPoliciesToRejectOption').prop("disabled", false);
        }
        $("#txtReasonReject").val("");

        AuthorizationPolicies.HidePanels();
        AuthorizationPolicies.LoadTextRejectionCauses();
        dropDownReject.show();
    }

    /**
   * @summary 
   *  Evento al cambiar el ddl del estado de la autorizacion
   */
    ChangeStatus() {
        $("#txtSearchAuthorization").val("");
        AuthorizationPolicies.ChangeStatusPolicies();
    }



    /**
    * @summary 
    * Evento que Setea el formulario de edicion de la politica
    * @param {policies} politica seleccionada
    * @param  {index} indice de la politica dentro del listView
    **/
    static SetFormPolicies(event, policies) {
        AuthorizationPolicies.ClearFormPolicies();
        if ($("#lsvPoliciesAuthorization").UifListView("getSelected").length === 1) {

            let status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));

            let findAutorization = function (element) {
                return element.PoliciesId === policies.PoliciesId && element.Key === policies.Key;
            }
            var index = $("#lsvPoliciesAuthorization").UifListView("findIndex", findAutorization);

            $("#hdnAutorizatioIndex").val(index);


            if (status === TypeStatusPolicies.Pending) {
                AuthorizationPolicies.SetFormPoliciesPending(policies);
            }
            else if (status === TypeStatusPolicies.Authorized) {
                AuthorizationPolicies.SetFormPoliciesAutorized(policies);
            }
            else if (status === TypeStatusPolicies.Rejected) {
                AuthorizationPolicies.SetFormPoliciesRejected(policies);
            }
            else {
                AuthorizationPolicies.SetFormPoliciesReasigned(policies);
            }
        }
    }

    /**
     *@summary 
     * Oculta todos los paneles
     **/
    static HidePanels() {
        dropDownReasign.hide();
        dropDownReject.hide();
        dropDownAccept.hide();
    }

    /**
    *@summary 
     * refresca la lista de autorizaciones y los formularios
    **/
    static ChangeStatusPolicies() {
        let selectedId = $("#ddlStatusPolicies").UifSelect("getSelected");
        AuthorizationPolicies.ShowPanels(selectedId);
        AuthorizationPolicies.ClearFormPolicies();
        if (selectedId) {
            AuthorizationPolicies.SetListPolicies(null, null, selectedId, null, null, "");
        } else {
            $("#lsvPoliciesAuthorization").UifListView("refresh");
        }
    }

    /**
     * @summary 
     * setea el formulario de la autorizacion reasignada
     * @param {policies} politica reasignada a visualizar
    **/
    static SetFormPoliciesReasigned(policies) {
        const status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
        RequestAuthorization.GetAuthorizationAnswerDescriptions(policies.PoliciesId, policies.UserAnswerId, status, policies.Key).done(data => {
            if (data.success) {
                let strDescription = '';
                if (data.result.length == 1) {
                    let strDescription = "<ol>";
                    var item = data.result[0];
                    let array = item.split("|");
                    strDescription += "<li><ul>";
                    listRisks.push(array[0]);
                    $.each(array, (indexArray, itemArray) => {
                        if (itemArray !== "") {
                            strDescription += "<li>" + itemArray + "</li>";
                        }
                    });
                    strDescription += "</ul></li></br>";
                    strDescription += "</ol>";
                    $("#btnExport").hide();
                }
                else {
                    $.each(data.result, (index, item) => {
                        let array = item.split("|");
                        listRisks.push(array[0]);
                    });
                    strDescription = Resources.Language.DetailToExcel;
                    $("#btnExport").show();
                }


                $("#divAuthorizationReasign").find(".txtDescription").html(strDescription);
                $("#divAuthorizationReasign").find(".txtNamePolicies").html(policies.DescriptionPolicie);
                $("#divAuthorizationReasign").find(".txtNameUser").html(policies.AccountName);
                $("#divAuthorizationReasign").find(".txtDatePolicie").html(policies.DateRequest);
                $("#divAuthorizationReasign").find(".txtReference").html(policies.Key);
                $("#divAuthorizationReasign").find(".txtReasonRequest").html(policies.DescriptionRequest);
                $("#divAuthorizationReasign").find(".txtReasonAnswer").html(policies.DescriptionAnswer);
                $("#divAuthorizationReasign").find(".txtDateAnswer").html(FormatDate(policies.DateAnswer));

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });

        let strDescription = "<ol>";
        $.each(policies.ListAutorizations, (index, item) => {
            let array = item.AuthorizationRequest.Description.split("|");
            strDescription += "<li><ul>";
            $.each(array, (indexArray, itemArray) => {
                if (itemArray !== "") {
                    strDescription += "<li>" + itemArray + "</li>";
                }
            });
            strDescription += "</ul></li></br>";
        });
        strDescription += "</ol>";
    }

    /**
    * @summary 
    * setea el formulario de la autorizacion rechazada
    * @param {policies} politica rechazada a visualizar
   **/
    static SetFormPoliciesRejected(policies) {
        const status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
        lockScreen();
        RequestAuthorization.GetAuthorizationAnswerDescriptions(policies.PoliciesId, policies.UserAnswerId, status, policies.Key).done(data => {
            if (data.success) {
                let strDescription = '';
                if (data.result.length == 1) {
                    strDescription = "<ol>";
                    var item = data.result[0];
                    let array = item.split("|");
                    strDescription += "<li><ul>";
                    listRisks.push(array[0]);
                    $.each(array, (indexArray, itemArray) => {
                        if (itemArray !== "") {
                            strDescription += "<li>" + itemArray + "</li>";
                        }
                    });
                    strDescription += "</ul></li></br>";
                    strDescription += "</ol>";
                    $("#btnExport").hide();
                }
                else {
                    $.each(data.result, (index, item) => {
                        let array = item.split("|");
                        listRisks.push(array[0]);
                    });
                    strDescription = Resources.Language.DetailToExcel;
                    $("#btnExport").show();
                }

                $("#divAuthorizationRejected").find(".txtDescription").html(strDescription);
                $("#divAuthorizationRejected").find(".txtNamePolicies").html(policies.DescriptionPolicie);
                $("#divAuthorizationRejected").find(".txtNameUser").html(policies.AccountName);
                $("#divAuthorizationRejected").find(".txtDatePolicie").html(policies.DateRequest);
                $("#divAuthorizationRejected").find(".txtReference").html(policies.Key);
                $("#divAuthorizationRejected").find(".txtReasonRequest").html(policies.DescriptionRequest);
                $("#divAuthorizationRejected").find(".txtReasonAnswer").html(policies.DescriptionAnswer);
                $("#divAuthorizationRejected").find(".txtDateAnswer").html(FormatDate(policies.DateAnswer));

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
            lockScreen();
        }).fail(() => unlockScreen());;
    }

    /**
   * @summary 
   * setea el formulario de la autorizacion autorizada
   * @param {policies} politica autorizada a visualizar
  **/
    static SetFormPoliciesAutorized(policies) {
        const status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
        lockScreen();
        RequestAuthorization.GetAuthorizationAnswerDescriptions(policies.PoliciesId, policies.UserAnswerId, status, policies.Key).done(data => {
            if (data.success) {
                let strDescription = '';
                if (data.result.length == 1) {
                    strDescription = "<ol>";
                    var item = data.result[0];
                    let array = item.split("|");
                    strDescription += "<li><ul>";
                    listRisks.push(array[0]);
                    $.each(array, (indexArray, itemArray) => {
                        if (itemArray !== "") {
                            strDescription += "<li>" + itemArray + "</li>";
                        }
                    });
                    strDescription += "</ul></li></br>";
                    strDescription += "</ol>";
                    $("#btnExport").hide();
                }
                else {
                    $.each(data.result, (index, item) => {
                        let array = item.split("|");
                        listRisks.push(array[0]);
                    });
                    strDescription = Resources.Language.DetailToExcel;
                    $("#btnExport").show();
                }

                $("#divAuthorizationAuthorized").find(".txtDescription").html(strDescription);
                $("#divAuthorizationAuthorized").find(".txtNamePolicies").html(policies.DescriptionPolicie);
                $("#divAuthorizationAuthorized").find(".txtNameUser").html(policies.AccountName);
                $("#divAuthorizationAuthorized").find(".txtDatePolicie").html(policies.DateRequest);
                $("#divAuthorizationAuthorized").find(".txtReference").html(policies.Key);
                $("#divAuthorizationAuthorized").find(".txtReasonRequest").html(policies.DescriptionRequest);
                $("#divAuthorizationAuthorized").find(".txtReasonAnswer").html(policies.DescriptionAnswer);
                $("#divAuthorizationAuthorized").find(".txtDateAnswer").html(FormatDate(policies.DateAnswer));

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
            unlockScreen();
        }).fail(() => unlockScreen());;
    }

    /**
     * @summary 
     * setea el formulario para la politica pendiente
     * @param {policies} politica seleccionada
    **/
    static SetFormPoliciesPending(policies) {
        const status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));

        listRisks = new Array();

        AuthorizationPolicies.SetAuthorizationHierarchy(policies.PoliciesId, policies.HierarchyAnswerId, policies.UserAnswerId);
        lockScreen();
        RequestAuthorization.GetAuthorizationAnswerDescriptions(policies.PoliciesId, policies.UserAnswerId, status, policies.Key).done(data => {
            if (data.success) {
                let strDescription = '';

                if (data.result.length == 1) {
                    strDescription = "<ol>";
                    var item = data.result[0];
                    let array = item.split("|");
                    strDescription += "<li><ul>";
                    listRisks.push(array[0]);
                    array.splice(0, 1);
                    $.each(array, (indexArray, itemArray) => {
                        if (itemArray !== "") {
                            strDescription += `<li>${itemArray}</li>`;
                        }
                    });
                    strDescription += "</ul></li></br>";
                    strDescription += "</ol>";
                    $("#btnExport").hide();
                }
                else {
                    $.each(data.result, (index, item) => {
                        let array = item.split("|");
                        listRisks.push(array[0]);
                    });
                    strDescription = Resources.Language.DetailToExcel;
                    $("#btnExport").show();
                }

                $("#btnAceptar").show();
                $("#btnReasignar").show();
                $("#btnRechazar").show();

                if (policies.FunctionType === 1) {
                    $("#btnPrinting").show();
                }
                else if (policies.FunctionType === 2 || policies.FunctionType === 3) {
                    $("#btnExport").show();
                }

                if (policies.Required) {
                    $("#btnReasignar").hide();
                } else {
                    $("#btnReasignar").show();
                }


                strDescription = "<ol>";
                $.each(data.result, (index, item) => {
                    let array = item.split("|");
                    strDescription += "<li><ul>";
                    $.each(array, (indexArray, itemArray) => {
                        if (itemArray !== "") {
                            strDescription += `<li>${itemArray}</li>`;
                        }
                    });
                    strDescription += "</ul></li></br>";
                });
                strDescription += "</ol>";


                $("#divAuthorizationPending").find(".txtDescription").html(strDescription);
                $("#divAuthorizationPending").find(".txtNamePolicies").html(policies.DescriptionPolicie);
                $("#divAuthorizationPending").find(".txtNameUser").html(policies.AccountName);
                $("#divAuthorizationPending").find(".txtDatePolicie").html(policies.DateRequest);
                $("#divAuthorizationPending").find(".txtReference").html(policies.Key);
                $("#divAuthorizationPending").find(".txtReasonRequest").html(policies.DescriptionRequest);
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
            unlockScreen();
        }).fail(() => unlockScreen());
    }

    /**
     * @summary 
     * limpia los formulario 
    **/
    static ClearFormPolicies() {
        $("#hdnAutorizatioIndex").val("");

        $(".txtNamePolicies").html("");
        $(".txtNameUser").html("");
        $(".txtDatePolicie").html("");
        $(".txtReference").html("");
        $(".txtDescription").html("");
        $(".txtReasonRequest").html("");
        $(".txtReasonAnswer").html("");
        $(".txtDateAnswer").html("");

        /*Panel de rechazo*/
        $("#formReject").find("#txtReasonReject").val("");

        /*Panel de aceptar*/
        $("#formAccept").find("#txtReasonAccept").val("");

        /*Panel de reasignar*/
        $("#formReassign").find("#txtReasonReasign").val("");
        $("#ddlHierarchy").UifSelect();
        $("#ddlHierarchyUser").UifSelect();

        $("#btnAceptar").hide();
        $("#btnReasignar").hide();
        $("#btnRechazar").hide();
        $("#btnPrinting").hide();
        $("#btnExport").hide();
    }

    /**
     * @summary 
     * setea las jerarquias autorizadoras parametrizadas de nivel superior
     * @param {policiesId} id de la politica 
     * @param {hierarchyId} jerarquia del usuario actual
     * @param {userId} id del usuario actual
    **/
    static SetAuthorizationHierarchy(policiesId, hierarchyId, userId) {
        RequestAuthorization.GetAuthorizationHierarchy(policiesId, hierarchyId, userId)
            .done((data) => {
                if (data.success) {
                    $("#ddlHierarchy").UifSelect({ sourceData: data.result });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
    }

    /**
     * @summary 
     * setea los usuarios autorizadores parametrizados de la jerarquia seleccionada
     * @param {hierarchyId} id de la jerarquia
    **/
    static SetUsersAuthorizationHierarchy(hierarchyId) {
        $("#ddlHierarchyUser").UifSelect();
        let index = $("#hdnAutorizatioIndex").val();
        let policie = $("#lsvPoliciesAuthorization").UifListView("getData")[index];

        if (hierarchyId.Id) {
            RequestSummaryAuthorization.GetUsersAutorization(policie.PoliciesId, hierarchyId.Id, false)
                .done((data) => {
                    if (data.success) {

                        data.result = data.result.filter(x => { return x.IdUser != policie.UserAnswerId });
                        $("#ddlHierarchyUser").UifSelect({ sourceData: data.result });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }
    }

    /**
    * @summary 
    * Setea la lista de las politicas
    * @param {status} estado de la politica
    * @param {dateInit} fecha nicial 
    * @param {dateEnd} fecha final 
    * @param {sort} like nombre de la politica
    */
    static SetListPolicies(idGroup, idPolicies, status, dateInit, dateEnd, sort, isInit) {
        $("#lsvPoliciesAuthorization").UifListView("refresh");
        if (status != -1) {
            RequestAuthorization.GetAuthorizationAnswerByFilter(idGroup, idPolicies, status, dateInit, dateEnd, sort)
                .done(data => {
                    if (data.success) {
                        AuthorizationPolicies.SetListAuthorizations(data.result);
                        if (GetQueryParameter("key") !== undefined && GetQueryParameter("IdPolicies") !== undefined) {
                            if (isInit) {
                                AuthorizationPolicies.SelectedAutorization(GetQueryParameter("key"), parseInt(GetQueryParameter("IdPolicies")), isInit);
                            }
                        }
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }
        else {
            RequestAuthorization.GetAuthorizationAnswersReasignByFilter(idGroup, idPolicies, dateInit, dateEnd, sort)
                .done(data => {
                    if (data.success) {
                        AuthorizationPolicies.SetListAuthorizations(data.result);
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }
    }

    /**
     * @summary 
     * setea la lista de autoriaciones
    **/
    static SetListAuthorizations(result) {
        result.forEach(item => {
            item.DateRequest = FormatDate(item.DateRequest);
            $("#lsvPoliciesAuthorization").UifListView("addItem", item);
        });
    }

    /**
    * @summary 
    * muestra u oculta los paneles dependiendo es estado de la politica
    * @param {status} estado de la politica
    */
    static ShowPanels(status) {
        $("#divAuthorizationRejected").hide();
        $("#divAuthorizationPending").hide();
        $("#divAuthorizationReasign").hide();
        $("#divAuthorizationAuthorized").hide();
        $("#footerAuthorizationPending").hide();
        $("#footerAuthorizationReasign").hide();

        switch (parseInt(status)) {
            case TypeStatusPolicies.Pending:
                $("#divAuthorizationPending").show();
                $("#footerAuthorizationPending").show();
                break;
            case TypeStatusPolicies.Authorized:
                $("#divAuthorizationAuthorized").show();
                break;
            case TypeStatusPolicies.Rejected:
                $("#divAuthorizationRejected").show();
                break;
            case TypeStatusPolicies.Reassigned:
                $("#divAuthorizationReasign").show();
                $("#footerAuthorizationReasign").show();
                break;
            default:
        }
    }

    ShowPoliciesToAcceptOption() {
        var option = $("#selectPoliciesToAcceptOption").UifSelect("getSelected");
        if (option == 3) {
            let policie = $("#lsvPoliciesAuthorization").UifListView("getSelected")[0];
            $("#divIndividualAccept").hide();
            $("#divRangeAccept").show();
            $("#inputPoliciesToAccept").val("");
            $("#inputPoliciesToAcceptRangeFrom").val(1);
            $("#inputPoliciesToAcceptRangeTo").val(policie.Count);
        } else if (option == 2) {
            $("#divIndividualAccept").show();
            $("#divRangeAccept").hide();
        } else {
            $("#divIndividualAccept").hide();
            $("#divRangeAccept").hide();
            $("#inputPoliciesToAccept").val("");
        }
    }

    ShowPoliciesToRejectOption() {
        var option = $("#selectPoliciesToRejectOption").UifSelect("getSelected");
        if (option == 3) {
            let policie = $("#lsvPoliciesAuthorization").UifListView("getSelected")[0];
            $("#divIndividualReject").hide();
            $("#divRangeReject").show();
            $("#inputPoliciesToReject").val("");
            $("#inputPoliciesToRejectRangeFrom").val(1);
            $("#inputPoliciesToRejectRangeTo").val(policie.Count);
        } else if (option == 2) {
            $("#divIndividualReject").show();
            $("#divRangeReject").hide();
        } else {
            $("#divIndividualReject").hide();
            $("#divRangeReject").hide();
            $("#inputPoliciesToReject").val("");
        }
    }

    ShowPoliciesToReassignOption() {
        var option = $("#selectPoliciesToReassignOption").UifSelect("getSelected");
        if (option == 3) {
            let policie = $("#lsvPoliciesAuthorization").UifListView("getSelected")[0];
            $("#divIndividualReassign").hide();
            $("#divRangeReassign").show();
            $("#inputPoliciesToReassign").val("");
            $("#inputPoliciesToReassignRangeFrom").val(1);
            $("#inputPoliciesToReassignRangeTo").val(policie.Count);
        } else if (option == 2) {
            $("#divIndividualReassign").show();
            $("#divRangeReassign").hide();

        } else {
            $("#divIndividualReassign").hide();
            $("#divRangeReassign").hide();
            $("#inputPoliciesToReassign").val("");
        }
    }

    ChangePoliciesRangeTo(records, rangeFromId, rangeToId) {
        var rangeFrom = parseInt($(rangeFromId).val());
        var rangeTo = parseInt($(rangeToId).val());

        if (parseInt(rangeTo, 10) > parseInt(records, 10)) {
            rangeTo = records;
        }

        if (rangeTo < rangeFrom && rangeFrom >= 1) {
            rangeTo = rangeFrom;
        }
        $(rangeFromId).val(rangeFrom);
        $(rangeToId).val(rangeTo);
    }

    ChangePoliciesRangeFrom(records, rangeFromId, rangeToId) {
        var rangeFrom = parseInt($(rangeFromId).val());
        var rangeTo;

        if ($(rangeToId).val() !== "") {
            rangeTo = parseInt($(rangeToId).val());
        } else {
            rangeTo = parseInt(records);
        }

        if (rangeFrom < 1) {
            rangeFrom = 1;
        }
        if (rangeFrom > rangeTo && rangeTo <= records) {
            rangeFrom = rangeTo;
        }
        $(rangeFromId).val(rangeFrom);
        $(rangeToId).val(rangeTo);
    }

    static policiestoProcess(option, rangeFrom, rangeTo, strEvents) {
        if (option == 0) {
            return undefined;
        }
        var listToProcess = new Array();

        if (option == 3) {
            for (var i = rangeFrom; i <= rangeTo; i++) {
                listToProcess.push(listRisks[i]);
            }
            return listToProcess;
        } else if (option == 2) {
            var error = 0;
            if ($('#inputEventsToProcess').val() == "") {
                $.UifDialog('alert',
                    { 'message': Resources.Language.MessageErrorExclusion },
                );
            } else {
                events = strEvents.split(",");

                var events = events.filter(function (x) {
                    return (x !== (undefined || null || ''));
                });

                if (events.length == 0) {
                    $.UifDialog('alert',
                        { 'message': Resources.Language.MessageErrorWithOutPolicies },
                    );
                    error++;
                } else {
                    $.each(events, function (index, value) {
                        if (typeof listRisks[value - 1] === 'undefined') {
                            $.UifDialog('alert',
                                { 'message': Resources.Language.MessageErrorPolicies },
                            );
                            error++;
                            return false;
                        }
                        else {
                            listToProcess.push(listRisks[value - 1]);
                        }
                    });
                    if (error > 0) {
                        return undefined;
                    } else {
                        return listToProcess;
                    }
                }
            }
        } else {
            return null;
        }
    }

    static checkKey(event) {
        return /\d|,/.test(event.key);
    }

    GenerateFileToExport() {
        var itemSelected = $("#lsvPoliciesAuthorization").UifListView('getSelected');
        const status = parseInt($("#ddlStatusPolicies").UifSelect("getSelected"));
        RequestAuthorization.GenerateFileToExport(itemSelected[0].PoliciesId, itemSelected[0].UserAnswerId, status, itemSelected[0].Key).done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
        *@summary 
         * refresca la lista de autorizaciones y los formularios
        **/
    static LoadTextRejectionCauses() {
        let index = $("#hdnAutorizatioIndex").val();
        const policies = $("#lsvPoliciesAuthorization").UifListView("getData")[index];
        var groupPolicyId = policies.GroupPoliciesId;
        RequestAuthorization.GetRejectionCauses(groupPolicyId).done(function (data) {
            if (data) {
                $('#RejectionCausesId').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}