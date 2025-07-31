class RequestSummaryAuthorization {

    static GetPoliciesTemporal(Id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/Policies/GetPoliciesTemporalByTemporalId',
            data: JSON.stringify({ id: Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteNotificationByTemporalId(Id, FunctionId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'AuthorizationPolicies/Policies/DeleteNotificationByTemporalId',
            data: JSON.stringify({ id: Id, functionId:FunctionId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    /**
    * @summary 
    * Llamado ajax que obtiene los usuarios autorizadores para la politica
     * @param {idPolicies} id de la politica
     * @param {idHierarchy} id de la jerarquia
     * @return 
     * lista de los usuarios autorizadores
    */
    static GetUsersAutorization(idPolicies, idHierarchy, withUserGroup) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies, "idHierarchy": idHierarchy, "withUserGroup": withUserGroup },
            url: rootPath + "AuthorizationPolicies/Summary/GetUsersAutorizationByIdPoliciesIdHierarchy"
        });
    }

    /**
    * @summary 
    * Llamado ajax que obtiene los usuarios notificadores para la politica
     * @param {idPolicies} id de la politica
     * @param {idHierarchy} id de la jerarquia
     * @return 
     * lista de los usuarios notificadores
    */
    static GetUsersNotification(idPolicies, idHierarchy, withUserGroup) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies, "idHierarchy": idHierarchy, "withUserGroup": withUserGroup },
            url: rootPath + "AuthorizationPolicies/Summary/GetUsersNotificationByIdPoliciesIdHierarchy"
        });
    }

    /**
   * @summary 
   * Llamado ajax que envia la solicitud de autorizacion de las politicas
    * @param {authorizationRequests} lista de las solicitudes
   */
    static SendAuthorizationRequests(authorizationRequests) {
        return $.ajax({
            type: "POST",
            data: { "authorizationRequests": authorizationRequests },
            url: rootPath + "AuthorizationPolicies/Summary/SendAuthorizationRequests"
        });
    }
}

class SummaryAuthorization extends Uif2.Page {

    getInitialState() {

        $("#ddlUserAutorization").UifMultiSelect();
        $("#ddlUserNotification").UifMultiSelect();

        $("#lvwPoliciesSummary").UifListView({
            customEdit: true,
            edit: true,
            displayTemplate: "#display-template-PoliciesSummary",
            height: 400
        });
    }

    bindEvents() {
        $("#lvwPoliciesSummary").on("rowEdit", this.SetFormPolicies);
        $("#btnSaveFormPolicies").on("click", this.SaveFormPolicies);
        $("#btnCancelFormPolicies").on("click", this.ClearFormPolicies);
        $("#btnSendRequestsPolicies").on("click", this.SendAuthorizationRequests);
    }


    /**
     * @summary 
     * Envia las solicitudes de autorizacion a los diferentes usuarios
    */
    SendAuthorizationRequests() {
        let policies = $("#lvwPoliciesSummary").UifListView("getData");
        let countWhitOutReason = policies.filter((item) => { return !item.DescriptionRequest && item.DescriptionRequest !== typeof (undefined); }).length;

        if (countWhitOutReason === 0) {
            let authorizationRequests = [];

            $.each(policies, (indexRequest, itemRequest) => {
                $.each(itemRequest.ListPolicies, (indexPolicie, itemPolicie) => {
                    let authorizationRequest = {};

                    authorizationRequest.Policies = { IdPolicies: itemPolicie.IdPolicies, Description: itemPolicie.Description, Message: itemPolicie.Message },
                        authorizationRequest.Key = $("#hdnKey").val();
                    authorizationRequest.Description = SummaryAuthorization.GetDescriptionConceptPolicies(itemPolicie.ConceptsDescription);
                    authorizationRequest.Status = 1;
                    authorizationRequest.NumberAut = itemPolicie.NumberAut;
                    authorizationRequest.IdHierarchyRequest = itemPolicie.IdHierarchyPolicy;
                    authorizationRequest.DescriptionRequest = itemRequest.DescriptionRequest;
                    authorizationRequest.AuthorizationAnswers = SummaryAuthorization.GetUsersAuthorizationSend(itemPolicie, itemRequest.usersAutorization);
                    authorizationRequest.NotificationUsers = SummaryAuthorization.GetUsersNotificationSend(itemRequest.usersNotification);
                    authorizationRequest.FunctionType = $("#hdnFunctionType").val();

                    authorizationRequests.push(authorizationRequest);
                });
            });

            RequestSummaryAuthorization.SendAuthorizationRequests(authorizationRequests).done((data) => {
                if (data.success) {
                    $.UifDialog("alert", { 'message': data.result }, function () {
                        if (authorizationRequests[0].FunctionType == FunctionType.PersonGuarantees) {
                            Guarantee.ReturnUnderwritingTemporal();
                        } else {
                            if (window.location.pathname !== "/Collective/Collective/Collective") {
                            window.location = rootPath + "Home/index";
                        }
                        }
                        
                    });
                    $("#modalPoliciesSummary").UifModal("hide");
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify("show", { 'type': "danger", 'message': AppResources.NotAllEventsHaveRequest, 'autoclose': true });
        }
    }

    /**
    * @summary 
    * Limpia el formulario
    **/
    ClearFormPolicies() {
        SummaryAuthorization.ClearFormPolicies();
    }

    /**
    * @summary 
    * Evento que Setea el formulario de edicion de la politica
    * @param {policies} politica seleccionada
    * @param  {index} indice de la politica dentro del listView
   **/
    SetFormPolicies(event, policies, index) {
        SummaryAuthorization.SetFormPolicies(policies, index);
    }

    /**
     * @summary 
     * Evento generado al guardar formulario
     * @param {e} Evento generado 
     */
    SaveFormPolicies(e) {
        e.preventDefault();
        SummaryAuthorization.SaveFormPolicies();
    }

    /**
     * @summary 
     * obtiene los usuarios autorizadores seleccionados
     * @return 
     * lista de AuthorizationAnswerModelView
    */
    static GetUsersAuthorizationSend(policies, usersAutorization) {
        let notificationUsers = [];
        let users = usersAutorization.filter((item) => {
            return item.Default === true;
        });

        $.each(users, (index, item) => {
            notificationUsers.push({
                IdUserAnswer: item.IdUser,
                IdHierarchyAnswer: policies.IdHierarchyAut,
                Status: 1,
                Required: item.Required,
                Enabled: true
            });
        });

        return notificationUsers;
    }

    /**
     * @summary 
     * obtiene los id's de los usuarios notificadores seleccionados
     * @return 
     * lista de int
    */
    static GetUsersNotificationSend(usersNotification) {
        let users = usersNotification
            .filter((item) => {
                return item.Default === true;
            })
            .map((item) => {
                return item.IdUser;
            });
        return users;
    }

    /***
     * @summary 
     * Obtiene una cadena con la informacion de los conceptos de descripcion
     * @param {listConcepts} lista de lso conceptos de descripcion
     * @return 
     * una cadena separada por | de los conceptos descripcion
     **/
    static GetDescriptionConceptPolicies(listConcepts) {
        let strDescription = "";

        $.each(listConcepts,
            (index, item) => {
                if (item.Description == "Número de operación") {
                    strDescription += item.Description + " : " + item.Value.replace(/\./g, "").replace(".", "").split(',')[0] + "|";
                }
                else
                    strDescription += item.Description + " : " + item.Value + "|";
            });
        return strDescription;
    }

    /**
    * @summary 
    * Realiza el guardado del formulario
    */
    static SaveFormPolicies() {
        if (SummaryAuthorization.ValidateFormPolicies()) {
            let index = parseInt($("#hdnIndexPolicies").val());
            let policies = $("#lvwPoliciesSummary").UifListView("getData")[index];

            let usersAutorization = $("#ddlUserAutorization").UifMultiSelect("getSelected");
            let usersNotification = $("#ddlUserNotification").UifMultiSelect("getSelected");

            if (usersAutorization.length < policies.NumberAut) {
                $.UifNotify("show", { 'type': "warning", 'message': "/*Los usuarios autorizadores deben ser minimo " + policies.NumberAut, 'autoclose': true });
            } else {
                $.each(policies.usersAutorization, (indexUser, itemUser) => {
                    itemUser.Default = false;
                    if (itemUser.Required === true) {
                        itemUser.Default = true;
                    }
                    if (usersAutorization != null)
                        if (usersAutorization.indexOf(itemUser.IdUser + "") !== -1) {
                            itemUser.Default = true;
                        }
                    policies.usersAutorization[indexUser] = itemUser;
                });
                $.each(policies.usersNotification, (indexUser, itemUser) => {
                    itemUser.Default = false;
                    if (usersNotification != null)
                        if (usersNotification.indexOf(itemUser.IdUser + "") !== -1) {
                            itemUser.Default = true;
                        }
                    policies.usersNotification[indexUser] = itemUser;
                });

                policies.DescriptionRequest = $("#txtReason").val();

                $("#lvwPoliciesSummary").UifListView("editItem", index, policies);
                SummaryAuthorization.ClearFormPolicies();
            }
        };
    }

    /**
     * @summary 
     * Valida si los datos del formulario son correctos
     **/
    static ValidateFormPolicies() {
        const usersAutorization = $("#ddlUserAutorization").UifMultiSelect("getSelected");
        const reason = $("#txtReason").val().trim();

        if (usersAutorization === null || reason === "") {
            $.UifNotify("show",
                {
                    'type': "info",
                    'message': AppResources.FieldsRequiredSummaryPolicies,
                    'autoclose': true
                });
            return false;
        }
        return true;
    }

    /**
     * @summary 
     * Limpia el formulario
     **/
    static ClearFormPolicies() {
        $("#ddlUserAutorization").UifMultiSelect();
        $("#ddlUserNotification").UifMultiSelect();
        $("#txtReason").val("");
        $("#hdnIndexPolicies").val("");

        $("#btnSaveFormPolicies").attr("disabled", "disabled");
        $("#btnCancelFormPolicies").attr("disabled", "disabled");
        $("#txtReason").attr("disabled", "disabled");
    }

    /**
     * @summary 
     * Setea el formulario de edicion de la politica
     * @param {policies} politica seleccionada
     * @param  {index} indice de la politica dentro del listView
    **/
    static SetFormPolicies(policies, index) {

        let promises = [];

        if (!policies.usersAutorization) {
            promises.push(new Promise((resolve, reject) => {
                RequestSummaryAuthorization.GetUsersAutorization(policies.IdPolicies, policies.IdHierarchyAut, true)
                    .done((data) => {
                        if (data.success) {
                            policies.usersAutorization = data.result;
                        } else {
                            policies.usersAutorization = [];
                        }
                        resolve();
                    })
                    .fail(() => {
                        reject();
                    });
            }));
        }
        if (!policies.usersNotification) {
            promises.push(new Promise((resolve, reject) => {
                RequestSummaryAuthorization.GetUsersNotification(policies.IdPolicies, policies.IdHierarchyAut, true)
                    .done((data) => {
                        if (data.success) {
                            policies.usersNotification = data.result;
                        } else {
                            policies.usersNotification = [];
                        }
                        resolve();
                    })
                    .fail(() => {
                        reject();
                    });
            }));
        }

        Promise.all(promises).then(() => {
            SummaryAuthorization.SetUsersAutorization(policies.IdPolicies, policies.IdHierarchyAut, policies.usersAutorization);
            SummaryAuthorization.SetUsersNotification(policies.IdPolicies, policies.IdHierarchyAut, policies.usersNotification);

            $("#lvwPoliciesSummary").UifListView("editItem", index, policies);

            $("#txtReason").val(policies.DescriptionRequest);
            $("#hdnIndexPolicies").val(index);
            $("#btnSaveFormPolicies").removeAttr("disabled");
            $("#btnCancelFormPolicies").removeAttr("disabled");
            $("#txtReason").removeAttr("disabled");
        });
    }

    /**
     * @summary 
     * Setea la lista de las politicas infringidas
     * @param {listPolicies} lista de politicas
    **/
    static SetListPolicies(listPolicies) {
        $("#lvwPoliciesSummary").UifListView("refresh");
        let listPoliciesTmp = [];
        $.each(listPolicies, (index, itemPolicie) => {
            let tmpPolicies = listPoliciesTmp.filter((item) => {
                return itemPolicie.IdHierarchyAut === item.IdHierarchyAut
                    && itemPolicie.IdPolicies === item.IdPolicies;
            });

            if (tmpPolicies.length === 0) {
                let tmpPolicie = listPolicies.filter((item) => {
                    return itemPolicie.IdHierarchyAut === item.IdHierarchyAut &&
                        itemPolicie.IdPolicies === item.IdPolicies;
                });

                let messages = [];

                $.each(tmpPolicie, (i, itemTmp) => {
                    let search = messages.filter((item) => { return item === itemTmp.Message; });

                    if (search.length === 0) {
                        messages.push(itemTmp.Message);
                    }
                });


                listPoliciesTmp.push({
                    Description: tmpPolicie[0].Description,
                    IdHierarchyAut: tmpPolicie[0].IdHierarchyAut,
                    IdPolicies: tmpPolicie[0].IdPolicies,
                    Message: messages,//tmpPolicie[0].Message,
                    NumberAut: tmpPolicie[0].NumberAut,
                    ListPolicies: tmpPolicie,
                    Count: tmpPolicie.length
                });
                $("#lvwPoliciesSummary").UifListView("addItem", listPoliciesTmp[listPoliciesTmp.length - 1]);
            }
        });

        //if (listPolicies.length === 1) {
        //    SummaryAuthorization.SetFormPolicies(listPoliciesTmp[0]);
        //} else {
        SummaryAuthorization.ClearFormPolicies();
        //}
        $("#modalPoliciesSummary").UifModal("showLocal", AppResources.TitleSolicitudeAuthorizationEvent);
    }

    /**
     * @summary 
     * obtiene y setea los usuarios autorizadores de la politica seleccionada
     * @param {idPolicies} id de la politica
     * @param {idHierarchy} id de la jerarquia
     * @param {listUsers} lista de usuarios
    **/
    static SetUsersAutorization(idPolicies, idHierarchy, listUsers) {
        let preSelected = SummaryAuthorization.GetSelectedUsers(listUsers);
        let usersRequired = SummaryAuthorization.GetRequiredUsers(listUsers);

        $("#ddlUserAutorization").UifMultiSelect({
            sourceData: listUsers
        });
        $("#ddlUserAutorization").UifMultiSelect("setSelected", preSelected);

        $.each($("#ddlUserAutorization").next("div").find("input[type=checkbox]"), (index, item) => {
            if (usersRequired.indexOf($(item).val()) !== -1) {
                $(item).attr("disabled", "disabled");
            }
        });
    }

    /**
    * @summary 
    * obtiene y setea los usuarios Notidicadores de la politica seleccionada
     * @param {idPolicies} id de la politica
     * @param {idHierarchy} id de la jerarquia
     * @param {listUsers} lista de usuarios
   **/
    static SetUsersNotification(idPolicies, idHierarchy, listUsers) {
        let preSelected = SummaryAuthorization.GetSelectedUsers(listUsers);

        $("#ddlUserNotification").UifMultiSelect({
            sourceData: listUsers
        });
        $("#ddlUserNotification").UifMultiSelect("setSelected", preSelected);
    }

    /**
     * @summary 
     * Obtiene los usuarios seleccionados por defecto
     * @param {listUsers} lista de usuarios
     * @return 
     * Lista de los id de los usuarios
    **/
    static GetSelectedUsers(listUsers) {
        return listUsers
            .filter((item) => {
                return item.Default === true;
            })
            .map((item) => {
                return item.IdUser;
            });
    }

    /**
     * @summary 
     * Obtiene los usuarios obligatorios
     * @param {listUsers} lista de usuarios
     * @return 
     * Lista de los id de los usuarios
    **/
    static GetRequiredUsers(listUsers) {
        return listUsers
            .filter((item) => {
                return item.Required === true;
            })
            .map((item) => {
                return "" + item.IdUser + "";
            });
    }
}

(() => { new SummaryAuthorization(); })();