var objUserPolicies = [];
var rowIndexAut;
var rowIndexNot;

class RequestConcepts {
    /***
     * @summary 
     * Obtiene los conceptos asignados a la politica
     * @param {idPolicies} id de la politica
    **/
    static GetConceptDescriptionsByIdPolicies(idPolicies) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies },
            url: rootPath + "AuthorizationPolicies/Policies/GetConceptDescriptionsByIdPolicies"
        });
    }

    /***
    * @summary 
    *  Guarda los conceptos asociados a la politica
    * @param {idPolicies} id de la politica
    * @param {conceptDescriptions} lista de conceptos
    **/
    static SaveConceptDescriptions(idPolicies, conceptDescriptions) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies, "conceptDescriptions": conceptDescriptions },
            url: rootPath + "AuthorizationPolicies/Policies/SaveConceptDescriptions"
        });
    }

    /***
     * @summary 
     * btiene los conceptos segun el filtro 
    * @param {idEntity} id de la entidad
     * @param {filter} like de la descripcion
     **/
    static GetConceptsByFilter(listEntities, filter) {
        return $.ajax({
            type: "POST",
            data: { "listEntities": listEntities, "filter": filter },
            url: rootPath + "RulesAndScripts/Concepts/GetConceptsByFilter"
        });
    }

    /**
    * @summary 
    *  obtiene el comparador del del concepto para la condicion de la regla
    * @param {int} idConcept
    * id del concepto
    * @param {int} idEntity
    * id de la entidad
    **/
    static GetComparatorConcept(idConcept, idEntity) {
        return $.ajax({
            type: "POST",
            data: { "idConcept": idConcept, "idEntity": idEntity },
            url: rootPath + "RulesAndScripts/RuleSet/GetComparatorConcept"
        });
    }

    /**
    *@summary
    *Obtiene el concepto especifico con sus respectivos valores
    *@param (int) idEntity
    *id de la entidad</param>
    *@param (int) idConcept 
    *id del concepto
    *@param (int) conceptType
    *tipo de concepto
    **/
    static GetSpecificConceptWithVales(idConcept, idEntity, dependency, conceptType) {
        return $.ajax({
            type: "POST",
            data: { "idConcept": idConcept, "idEntity": idEntity, "dependency": dependency, "conceptType": conceptType },
            url: rootPath + "RulesAndScripts/Concepts/GetSpecificConceptWithVales"
        });
    }

}

class RequestGroupsPolicies {
    /**
    * @summary 
    *  peticion ajax que obtiene los grupos de politicas
    */
    static GetGroupsPolicies() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "AuthorizationPolicies/Policies/GetGroupsPolicies"
        });
    }
}

class RequestPolicies {
    /**
    * @summary 
    * peticion ajax que obtiene las de politicas del grupo 
    * @param {idGroup} id del grupo de politicas
    */
    static GetPoliciesByIdGroup(idGroup) {
        return $.ajax({
            type: "POST",
            data: { "idGroup": idGroup },
            url: rootPath + "AuthorizationPolicies/Policies/GetPoliciesByIdGroup"
        });
    }

    /**
   * @summary 
   * Realiza la creacion de una politica con su respactiva regla
   * @param {Models.PoliciesAut} policies 
   * politica a crear
   * @param {RUModels._RuleSet} ruleSet
   * regla a crear
   */
    static CreateRulePolicies(policies, ruleSet) {
        return $.ajax({
            type: "POST",
            data: { "policies": policies, "ruleSet": JSON.stringify(ruleSet) },
            url: rootPath + "AuthorizationPolicies/Policies/CreateRulePolicies"
        });
    }

    /**
     * @summary 
     * Realiza la modificacion de una politica con su respactiva regla
     * @param {Models.PoliciesAut} policies 
     * politica a modificar
     * @param {RUModels._RuleSet} ruleSet
     * regla a modificar
     */
    static UpdateRulePolicies(policies, ruleSet) {
        return $.ajax({
            type: "POST",
            data: { "policies": policies, "ruleSet": JSON.stringify(ruleSet) },
            url: rootPath + "AuthorizationPolicies/Policies/UpdateRulePolicies"
        });
    }

    /**
   * @summary 
   * peticion ajax que guarda la politica
   * @param {policies} id del grupo de politicas
   */
    static SaveRulesPolicies(policies, idHierarchyDt) {
        return $.ajax({
            type: "POST",
            data: { "policies": policies, "idHierarchyDt": idHierarchyDt },
            url: rootPath + "AuthorizationPolicies/Policies/SaveRulesPolicies"
        });
    }

    /***
     * @summary 
     *  Elimina una politica y su respectiva regla
     * @param {idPolicies} id de politica
    **/
    static DeleteRulePolicies(idPolicies) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies },
            url: rootPath + "AuthorizationPolicies/Policies/DeleteRulePolicies"
        });
    }

    static ImportRuleSet(data) {
        return $.ajax({
            url: rootPath + "AuthorizationPolicies/Policies/ImportRuleSet",
            type: "POST",
            "data": data,
            contentType: false,
            cache: false,
            processData: false
        });
    }
}

class RequestRulesPolicies {
    /**
     * @summary 
     * peticion ajax que consulta las politicas-reglas segun el filtro
    * @param {idPackage}id del paquete
    * @param {idGroup} id del grupo
    * @param {type} tipo de politica
    * @param {position} posicion de la politica
    * @param {filter} filtro tipo like
    **/
    static GetRulesPoliciesByFilter(idPackage, idGroup, type, position, filter) {
        return $.ajax({
            type: "POST",
            data: { "idPackage": idPackage, "idGroup": idGroup, "type": type, "position": position, "filter": filter },
            url: rootPath + "AuthorizationPolicies/Policies/GetRulesPoliciesByFilter"
        });
    }
}

class RequestTypePolicies {
    /**
    * @summary 
    * peticion ajax que consulta los tipos de politicas 
   **/
    static GetTypePolicies() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "AuthorizationPolicies/Policies/GetTypePolicies"
        });
    }
}

class RequestUserAuthorization {
    /**
     * @summary 
     * peticion ajax que Crea los usuarios autorizadores para la politica
     * @param {idPolicies} id de la politica
     * @param {users} lista de usuarios
     * @param {countMin} numero minimo de autorizadores
    **/
    static CreateUsersAutorization(idPolicies, users, countMin) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies, "users": users, "countMin": countMin },
            url: rootPath + "AuthorizationPolicies/Policies/CreateUsersAutorization"
        });
    }
}

class RequestUserNotification {
    /**
     * @summary 
     * peticion ajax que Crea los usuarios notificadores para la politica
     * @param {idPolicies} id de la politica
     * @param {users} lista de usuarios
    **/
    static CreateUsersNotification(idPolicies, users) {
        return $.ajax({
            type: "POST",
            data: { "idPolicies": idPolicies, "users": users },
            url: rootPath + "AuthorizationPolicies/Policies/CreateUsersNotification"
        });
    }
}

class PoliciesAut extends Uif2.Page {

    getInitialState() {
        $.ajaxSetup({ async: false });
        RequestGroupsPolicies.GetGroupsPolicies().done((data) => {
            if (data.success) {
                $("#ddlGroupPolicies").UifSelect({
                    sourceData: data.result
                });
            }
        });
        RequestTypePolicies.GetTypePolicies().done((data) => {
            if (data.success) {
                $("#ddlTypePolicies").UifSelect({
                    sourceData: data.result
                });
            }
        });



        $("#ddlConcepts").UifSelect();
        $("#ddlLevelConceptsDescription").UifSelect();
        $("#ddlHierarchyDt").UifSelect();
        $("#ddlLevelPolicies").UifSelect();
        $("#ddlHierarchy").UifSelect();
        $("#lsvPolicies").UifListView({
            selectionType: "single",
            displayTemplate: "#display-template-Policies",
            delete: true,
            deleteCallback: this.DeletePolices,
            add: true,
            edit: true,
            customAdd: true,
            customEdit: true
        });
        $("#lsvConceptsDescription").UifListView({
            displayTemplate: "#display-template-ConceptsDescription",
            delete: true,
            add: false,
            edit: false,
            drag: true,
            height: 300,
            deleteCallback: (deferred, data) => { deferred.resolve(); }
        });

        $("#txtNumberAut").UifMask({ pattern: "000" });

        $("#editUserAut").UifInline();
        $("#editUserNot").UifInline();
    }

    bindEvents() {
        $("#btnExitPolicies").on("click", this.Exit);
        $("#btnPoliciesRules").on("click", this.ShowPoliciesRules);

        $("#lsvPolicies").on("rowAdd", this.AddPolicie);
        $("#lsvPolicies").on("rowEdit", this.EditPolicie);

        $("#ddlGroupPolicies").on("itemSelected", PoliciesAut.ChangeGroup);
        $("#linkAuthorizedUsers").on("click", this.linkAuthorizedUsers);
        $("#linkNotificatedUsers").on("click", this.linkNotificatedUsers);
        $("#linkConceptsDescription").on("click", this.linkConceptsDescription);
        $("#ddlHierarchyAutUsers").on("itemSelected", this.ChangeHierarchyAutUsers);
        $("#ddlHierarchyNotUsers").on("itemSelected", this.ChangeHierarchyNotUsers);
        $("#ddlLevelConceptsDescription").on("itemSelected", this.ChangeLevelConceptsDescription);
        $("#tableAutUsers").on("rowEdit", this.RowEditAutUser);
        $("#tableNotUsers").on("rowEdit", this.RowEditNotUser);
        $("#editUserAut").on("Save", this.SaveEditUserAut);
        $("#editUserNot").on("Save", this.SaveEditUserNot);
        $("#btnSendPolicies").on("click", this.SendPolicies);
        $("#btnSendAuthorizedUsers").on("click", this.SendAuthorizedUsers);
        $("#btnSendNotificatedUsers").on("click", this.SendNotificatedUsers);
        $("#btnSendConceptsDescription").on("click", this.SendConceptsDescription);
        $("#btnAddConceptDescription").on("click", this.AddConceptDescription);
        $("#modalPolicie").on("closed.modal", this.CloseModal);
        $("#modalAuthorizedUsers").on("closed.modal", this.CloseModal);
        $("#modalNotificatedUsers").on("closed.modal", this.CloseModal);
        $("#modalConceptsDescription").on("closed.modal", this.CloseModal);
        $("#txtSearchPolicies").on("buttonClick", this.SearchPolicies);
        $("#chkEnabled").on("change", this.ChangeEnabled);
        $("#ddlTypePolicies").on("change", this.ChangeTypePolicies);

        if (gblPolicies && gblPolicies.GroupPolicies) {
            $("#ddlGroupPolicies").UifSelect("setSelected", gblPolicies.GroupPolicies.IdGroupPolicies);
            $("#ddlGroupPolicies").trigger("itemSelected", [{ Id: gblPolicies.GroupPolicies.IdGroupPolicies.toString() }]);

            gblPolicies = {};
        }
    }

    ChangeTypePolicies() {
        let checked = $("#chkEnabled").is(":checked");
        let type = $("#ddlTypePolicies").val();

        if (checked && type == 3) {
            let index = $("#hdnModalPolicieIndex").val();
            let policie = $("#lsvPolicies").UifListView("getData")[index];

            RequestSummaryAuthorization.GetUsersAutorization(policie.IdPolicies, null, false).done((data) => {
                if (data.success) {
                    if (data.result.length === 0) {
                        $("#chkEnabled").removeAttr("checked");
                    }
                } else {
                    $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                }
            });

            RequestConcepts.GetConceptDescriptionsByIdPolicies(policie.IdPolicies).done((data) => {
                if (data.success) {
                    if (data.result.length === 0) {
                        $("#chkEnabled").removeAttr("checked");
                    }
                } else {
                    $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                }
            });
        }
    }

    ChangeEnabled() {
        let checked = $("#chkEnabled").is(":checked");
        if (checked) {
            let index = $("#hdnModalPolicieIndex").val();
            let policie = $("#lsvPolicies").UifListView("getData")[index];

            if ($("#ddlTypePolicies").val() == 3) {
                RequestSummaryAuthorization.GetUsersAutorization(policie.IdPolicies, null, false).done((data) => {
                    if (data.success) {
                        if (data.result.length === 0) {
                            $("#chkEnabled").removeAttr("checked");
                            $.UifNotify("show", { "type": "warning", "message": "No se puede habilitar sin asignar usuarios autorizadores", "autoclose": true });
                        }
                    } else {
                        $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                    }
                });
            }

            RequestConcepts.GetConceptDescriptionsByIdPolicies(policie.IdPolicies).done((data) => {
                if (data.success) {
                    if (data.result.length === 0) {
                        $("#chkEnabled").removeAttr("checked");
                        $.UifNotify("show", { "type": "warning", "message": "No se puede habilitar sin asignar informacion para el autorizador", "autoclose": true });
                    }
                } else {
                    $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                }
            });
        }
    }

    AddPolicie() {
        $("#hdnModalPolicieIndex").val("");
        if ($("#ddlGroupPolicies").UifSelect("getSelected")) {
            let policie = {
                GroupPolicies: $("#ddlGroupPolicies").UifSelect("getSelectedSource")
            }
            $("#ddlLevelPolicies").removeAttr("disabled");
            $("#chkEnabled").attr("disabled", "disabled");
            PoliciesAut.SetFormPolicie(policie);
            PoliciesAut.ShowModalPolicies(1);
        }
    }

    EditPolicie(event, data, index) {
        PoliciesAut.ShowModalPolicies(1);
        $("#ddlLevelPolicies").attr("disabled", "disabled");
        $("#chkEnabled").removeAttr("disabled");
        $("#hdnModalPolicieIndex").val(index);
        PoliciesAut.SetFormPolicie(data);
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    ShowPoliciesRules(e) {
        e.preventDefault();
        let policies = PoliciesAut.GetPolicieSelected();
        if (policies !== false) {
            router.run("prtPoliciesRules");
            gblPolicies = policies[0];
        }
    }

    /**
     * Realiza la busqueda basica de las politicas
     * @param {event} e 
     * @param {string} value 
     */
    SearchPolicies(e, value) {
        let group = parseInt($("#ddlGroupPolicies").UifSelect("getSelected"));
        PoliciesAut.SetListRulesPolicies(null, group, null, null, value);
        $("#txtSearchPolicies").val("");
    }

    /**
     * @summary 
     * Evento que elimina una politica
     * @param {evento} e 
     * @param {policies} item 
     * Politica que se va a eliminar
     */
    DeletePolices(e, item) {
        RequestPolicies.DeleteRulePolicies(item.IdPolicies)
            .done((data) => {
                if (data.success) {
                    e.resolve();
                    $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
    }

    /**
     * @summary 
     * Evento al cambiar el nivel del concepto 
     * @param {event} e 
     * @param {level} item 
     * nivel seleccionado
     */
    ChangeLevelConceptsDescription(e, item) {
        $("#txtSearchPolicies").val("");
        if (item.Id) {
            RequestConcepts.GetConceptsByFilter([item.Id], "")
                .done((data) => {
                    if (data.success) {
                        $("#ddlConcepts").UifSelect({
                            sourceData: data.result,
                            native: false
                        });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        } else {
            $("#ddlConcepts").UifSelect();
        }
    }

    /**
     * @summary 
     * Agrega un nuevo concepto a la lista los conceptos asociados a la politica
     */
    AddConceptDescription() {
        let newConceptId = $("#ddlConcepts").UifSelect("getSelected");
        if (newConceptId) {
            let concepts = $("#lsvConceptsDescription").UifListView("getData");
            let entityId = parseInt($("#ddlLevelConceptsDescription").UifSelect("getSelected"));
            let description = $("#ddlConcepts").UifSelect("getSelectedText");

            var filter = concepts.filter((item) => {
                return item.Concept.ConceptId === parseInt(newConceptId) && item.Concept.Entity.EntityId === parseInt(entityId);
            });

            if (filter.length === 0) {
                let newConcept = {
                    Concept: {
                        Description: description,
                        ConceptId: parseInt(newConceptId),
                        Entity: {
                            EntityId: entityId
                        },
                        ConceptControlType: 1,
                        ConceptType: 1
                    },
                    EntityDescription: $("#ddlLevelConceptsDescription option[value=" + entityId + "]").text()
                };
                $("#lsvConceptsDescription").UifListView("addItem", newConcept);
            } else {
                $.UifNotify("show",
                    { 'type': "warning", 'message': "El concepto ya pertenece a la politica", 'autoclose': true });
            }
        } else {
            $.UifNotify("show", { 'type': "warning", 'message': "Debe seleccionar un concepto", 'autoclose': true });
        }
    }

    /**
     * @summary 
     * Realiza el guardado de lo conceptos asociados a la politica
     */
    SendConceptsDescription() {
        let police = PoliciesAut.GetPolicieSelected()[0];
        let index = PoliciesAut.GetIndexPolicie(police.IdPolicies);
        let concepts = $("#lsvConceptsDescription").UifListView("getData");

        RequestConcepts.SaveConceptDescriptions(police.IdPolicies, concepts)
            .done((data) => {
                if (data.success) {
                    police.ConceptsDescription = concepts;
                    $("#lsvPolicies").UifListView("editItem", index, police);
                    PoliciesAut.ShowModalPolicies(-1);
                    $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
    }

    /**
     * @summary 
     * Realiza el guardado basico de la politica
     **/
    SendPolicies() {
        if ($("#formModalPolicie").valid()) {
            let index = $("#hdnModalPolicieIndex").val();
            let policie = {};

            if (!index) {
                policie.Description = $("#txtNamePolicie").val();
                policie.GroupPolicies = $("#ddlGroupPolicies").UifSelect("getSelectedSource");
                policie.GroupPolicies.EntityDescription = $("#ddlLevelPolicies").UifSelect("getSelectedSource").Description;
                policie.IdHierarchyPolicy = $("#ddlHierarchy").val();
                policie.Position = $("#ddlLevelPolicies").val();
                policie.Message = $("#txtMessagePolicie").val();
                policie.Type = $("#ddlTypePolicies").val();
                policie.IdHierarchyAut = 0;
                policie.Enabled = false;

                $("#modalPolicie").UifModal("hide");
                router.run("prtPoliciesRules");
                gblPolicies = policie;
            }
            else {
                policie = $("#lsvPolicies").UifListView("getData")[index];
                policie.Description = $("#txtNamePolicie").val();
                policie.GroupPolicies = $("#ddlGroupPolicies").UifSelect("getSelectedSource");
                policie.GroupPolicies.EntityDescription = $("#ddlLevelPolicies").UifSelect("getSelectedSource").Description;
                policie.IdHierarchyPolicy = $("#ddlHierarchy").val();
                policie.Position = $("#ddlLevelPolicies").val();
                policie.Message = $("#txtMessagePolicie").val();
                policie.Type = $("#ddlTypePolicies").val();
                policie.Enabled = $("#chkEnabled").is(":checked");

                RequestPolicies.SaveRulesPolicies(policie, null)
                    .done((data) => {
                        if (data.success) {
                            $("#lsvPolicies").UifListView("editItem", index, policie);
                            PoliciesAut.ShowModalPolicies(-1);
                            $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                        } else {
                            $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                        }
                    });
            }
        }
    }

    /**
     * @summary Evento al cerrar la modal
     */
    CloseModal() {
        $("#hdnModalPolicieIndex").val("");
        $("#txtNamePolicie").val("");
        $("#txtPackageMain").val("");
        $("#txtModuleMain").val("");
        $("#txtSubModuleMain").val("");
        $("#ddlLevelConceptsDescription").val("");
        $("#ddlConcepts").UifSelect();
        objUserPolicies = [];
        $(".field-validation-error").html("");
    }

    /**
     * @summary 
     * realiza el guardado de la modal de usuarios autorizadores
     */
    SendAuthorizedUsers() {
        if ($("#formModalAuthorizedUsers").valid()) {
            let police = PoliciesAut.GetPolicieSelected()[0];
            let index = PoliciesAut.GetIndexPolicie(police.IdPolicies);

            let countMin = $("#txtNumberAut").val();
            let valid = true;
            let isDefault = true;
            let tempUsers = [];

            $.each(objUserPolicies,
                (i, item) => {
                    var filter = objUserPolicies.filter((x) => {
                        return parseInt(x.IdHierarchy) === parseInt(item.IdHierarchy);
                    });

                    if (filter.length < countMin) {
                        valid = false;
                        return;
                    }

                    if (filter.filter((x) => { return x.Default === true }).length < 1) {
                        isDefault = false;
                        return;
                    }


                    tempUsers.push({
                        Policies: { IdPolicies: police.IdPolicies, Type: 1 },
                        User: { UserId: item.IdUser },
                        Hierarchy: { Id: item.IdHierarchy },
                        Default: item.Default,
                        Required: item.Required
                    });
                });

            if (countMin > 0) {
                if (valid) {
                    if (isDefault) {
                        RequestUserAuthorization.CreateUsersAutorization(police.IdPolicies, tempUsers, countMin)
                            .done((data) => {
                                if (data.success) {
                                    if (objUserPolicies.length !== 0)
                                        police.UserAuthorization = objUserPolicies;
                                    police.NumberAut = parseInt(countMin);
                                    $("#lsvPolicies").UifListView("editItem", index, police);
                                    objUserPolicies = [];
                                    PoliciesAut.ShowModalPolicies(-1);
                                    $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                                } else {
                                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                                }
                            });
                    }
                    else {
                        $.UifNotify("show",
                            {
                                'type': "danger",
                                'message': "No todas las jerarquias tiene un usuario predeterminado",
                                'autoclose': true
                            });
                    }
                }
                else {
                    $.UifNotify("show",
                        {
                            'type': "danger",
                            'message': "No todas las jerarquias tiene los usuarios minimos",
                            'autoclose': true
                        });
                }

            } else {
                $.UifNotify("show",
                    {
                        'type': "danger",
                        'message': "Deben asignarse numero de usuarios autorizadores",
                        'autoclose': true
                    });
            }
        }
    }

    /**
   * @summary 
   * realiza el guardado de la modal de usuarios notificadores
   */
    SendNotificatedUsers() {
        let police = PoliciesAut.GetPolicieSelected()[0];
        let index = PoliciesAut.GetIndexPolicie(police.IdPolicies);

        let tempUsers = [];

        $.each(objUserPolicies,
            (i, item) => {
                var filter = objUserPolicies.filter((x) => {
                    return parseInt(x.IdHierarchy) === parseInt(item.IdHierarchy);
                });

                tempUsers.push({
                    Policies: { IdPolicies: police.IdPolicies, Type: 1 },
                    User: { UserId: item.IdUser },
                    Hierarchy: { Id: item.IdHierarchy },
                    Default: item.Default
                });
            });

        RequestUserNotification.CreateUsersNotification(police.IdPolicies, tempUsers)
            .done((data) => {
                if (data.success) {
                    if (objUserPolicies.length !== 0)
                        police.UserNotification = objUserPolicies;
                    $("#lsvPolicies").UifListView("editItem", index, police);
                    objUserPolicies = [];
                    PoliciesAut.ShowModalPolicies(-1);
                    $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
    }

    /**
     * @summary 
     * realiza el guardado local de la edicion de una fila de la tabla de usuarios autorizadores
     */
    SaveEditUserAut() {
        let index = $("#hdnmIndexAutUser").val();
        let item = $("#tableAutUsers").UifDataTable("getData")[index];

        item.Authorized = $("#chkAutAutorized").prop("checked");
        item.Required = $("#chkAutRequired").prop("checked");
        item.Default = $("#chkAutDefault").prop("checked");

        if ($("#chkAutAutorized").prop("checked") ||
            $("#chkAutRequired").prop("checked") ||
            $("#chkAutDefault").prop("checked")) {
            item.Authorized = true;
        }

        $("#tableAutUsers").UifDataTable("editRow", item, rowIndexAut);
        PoliciesAut.EditTempUsersPolicies(item);
        $("#editUserAut").UifInline("hide");
    }

    /**
    * @summary 
    * realiza el guardado local de la edicion de una fila de la tabla de usuarios notificadores
    */
    SaveEditUserNot() {
        let index = $("#hdnmIndexNotUser").val();
        let item = $("#tableNotUsers").UifDataTable("getData")[index];

        item.Notificated = $("#chkNotNotificated").prop("checked");
        item.Default = $("#chkNotDefault").prop("checked");

        if ($("#chkNotNotificated").prop("checked") || $("#chkNotDefault").prop("checked")) {
            item.Notificated = true;
        }

        $("#tableNotUsers").UifDataTable("editRow", item, rowIndexNot);
        PoliciesAut.EditTempUsersPolicies(item);
        $("#editUserNot").UifInline("hide");
    }

    /**
    * @summary 
    * Evento al seleccionar una fila de la tabla usuarios autorizadores
    */
    RowEditAutUser(event, data, position) {
        rowIndexAut = position;
        let tableData = $("#tableAutUsers").UifDataTable("getData");

        var posicion = PoliciesAut.GetPositionDataTable(tableData, data);
        let disable = PoliciesAut.SearchDisabled(data);
        if (disable) {
            $.UifNotify("show", { "type": "warning", "message": Resources.Language.UserDisable, "autoclose": true });
            return;
        }

        $("#hdnmIndexAutUser").val(posicion);
        $("#chkAutAutorized").removeAttr("checked");
        $("#chkAutRequired").removeAttr("checked");
        $("#chkAutDefault").removeAttr("checked");

        if (data.Authorized) {
            $("#chkAutAutorized").prop("checked", "checked");
        }
        if (data.Required) {
            $("#chkAutRequired").prop("checked", "checked");
        }
        if (data.Default) {
            $("#chkAutDefault").prop("checked", "checked");
        }
        $("#editUserAut").UifInline("show");
    }

    /**
    * @summary 
    * Evento al seleccionar una fila de la tabla usuarios notificadores
    */
    RowEditNotUser(event, data, position) {
        rowIndexNot = position;
        let tableData = $("#tableNotUsers").UifDataTable("getData");

        var posicion = PoliciesAut.GetPositionDataTable(tableData, data);

        let disable = PoliciesAut.SearchDisabled(data);
        if (disable) {
            $.UifNotify("show", { "type": "warning", "message": Resources.Language.UserDisable, "autoclose": true });
            return;
        }

        $("#hdnmIndexNotUser").val(posicion);
        $("#chkNotNotificated").removeAttr("checked");
        $("#chkNotDefault").removeAttr("checked");

        if (data.Notificated) {
            $("#chkNotNotificated").prop("checked", "checked");
        }
        if (data.Default) {
            $("#chkNotDefault").prop("checked", "checked");
        }
        $("#editUserNot").UifInline("show");
    }

    /**
     * @summary
     * Método que devuelve la posición de un elemento dentro de una tabla.
     * Compara con el nombre de usuario.
     * @param {any} tableData
     * @param {Object} data
     */
    static GetPositionDataTable(tableData, data) {
        let arreglo = [];
        var posicion;

        arreglo = tableData.map((currentElement, index) => {
            return {
                id: index,
                account: currentElement.Account
            };
        });

        arreglo.filter((item) => {
            if (item.account === data.Account)
                posicion = item.id;
        });

        return posicion;
    }

    /**
     * @summary 
     * evento al cambiar la jerarquia de usuarios autorizadores
     * @param {e} evento
     * @param {item} item seleccionado
     */
    ChangeHierarchyAutUsers(e, item) {
        if (item.Id) {
            var policie = PoliciesAut.GetPolicieSelected()[0];
            UserRequest.GetUsersByHierarchyModuleSubmodule(item.Id,
                policie.GroupPolicies.Module.Id,
                policie.GroupPolicies.SubModule.Id)
                .done((data) => {
                    if (data.success) {
                        PoliciesAut.SetTableAutUsers(policie, data.result, item.Id);
                    } else {
                        $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                    }
                });
        } else {
            $("#tableAutUsers").UifDataTable("clear");
        }
    }

    /**
     * @summary 
     * evento al cambiar la jerarquia de usuarios notificadores
     * @param {e} evento
     * @param {item} item seleccionado
     */
    ChangeHierarchyNotUsers(e, item) {
        if (item.Id) {
            var policie = PoliciesAut.GetPolicieSelected()[0];
            UserRequest.GetUsersByHierarchyModuleSubmodule(item.Id,
                policie.GroupPolicies.Module.Id,
                policie.GroupPolicies.SubModule.Id)
                .done((data) => {
                    if (data.success) {
                        PoliciesAut.SetTableNotUsers(policie, data.result, item.Id);
                    } else {
                        $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                    }
                });
        } else {
            $("#tableNotUsers").UifDataTable("clear");
        }
    }

    /**
    * evento al seleccionar el linkAuthorizedUsers
    */
    linkAuthorizedUsers(e) {
        e.preventDefault();
        var policie = PoliciesAut.GetPolicieSelected();
        if (policie !== false) {
            RequestSummaryAuthorization.GetUsersAutorization(policie[0].IdPolicies, null, false).done((data) => {
                if (data.success) {
                    let index = PoliciesAut.GetIndexPolicie(policie[0].IdPolicies);
                    policie[0].UserAuthorization = data.result;
                    if (objUserPolicies.length === 0) {
                        objUserPolicies = policie[0].UserAuthorization;
                    }
                    $("#lsvPolicies").UifListView("editItem", index, policie[0]);

                    PoliciesAut.SetAuthorizedUsers(policie[0]);
                    PoliciesAut.ShowModalPolicies(3);
                } else {
                    $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                }
            });
        }
    }


    /**
    * evento al seleccionar el linkNotificatedUsers
    */
    linkNotificatedUsers(e) {
        e.preventDefault();
        let policie = PoliciesAut.GetPolicieSelected();
        if (policie !== false) {
            RequestSummaryAuthorization.GetUsersNotification(policie[0].IdPolicies, null, false).done((data) => {
                if (data.success) {
                    let index = PoliciesAut.GetIndexPolicie(policie[0].IdPolicies);
                    policie[0].UserNotification = data.result;

                    if (objUserPolicies.length === 0) {
                        objUserPolicies = policie[0].UserNotification;
                    }

                    PoliciesAut.SetNotificatedUsers(policie[0]);
                    PoliciesAut.ShowModalPolicies(4);

                    $("#lsvPolicies").UifListView("editItem", index, policie[0]);
                } else {
                    $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                }
            });
        }
    }

    /**
    * evento al seleccionar el linkConceptsDescription
    */
    linkConceptsDescription(e) {
        e.preventDefault();
        let policie = PoliciesAut.GetPolicieSelected();
        if (policie !== false) {
            let level = policie[0].Position;

            RequestRules.GetEntitiesByPackageIdLevelId($("#ddlGroupPolicies").UifSelect("getSelectedSource").Package.PackageId, level).done((data1) => {
                data1.result.splice(0, 1);
                $("#ddlLevelConceptsDescription").UifSelect({
                    sourceData: data1.result
                });

                if (!policie[0].ConceptsDescription) {
                    RequestConcepts.GetConceptDescriptionsByIdPolicies(policie[0].IdPolicies).done((data) => {
                        if (data.success) {
                            let index = PoliciesAut.GetIndexPolicie(policie[0].IdPolicies);
                            policie[0].ConceptsDescription = data.result;
                            PoliciesAut.SetListConceptsDescription(policie[0]);
                            $("#lsvPolicies").UifListView("editItem", index, policie[0]);
                        } else {
                            $.UifNotify("show", { "type": "warning", "message": data.result, "autoclose": true });
                        }
                    });
                } else {
                    PoliciesAut.SetListConceptsDescription(policie[0]);
                }
                PoliciesAut.ShowModalPolicies(5);
            });
        }
    }

    /**
     * evento al cambiar el grupo de eventos
     * @param {event} evento 
     * @param {item } item seleccionado
     */
    static ChangeGroup(event, item) {
        PoliciesAut.SetListRulesPolicies(null, item.Id, null, null, null, "");
        if (item.Id) {
            RequestLevels.GetLevelsByIdGroupPolicies(item.Id, null).done((data) => {
                if (data.success) {
                    let dataTmp = data.result.filter((x) => {
                        return x.Position !== 0;
                    });


                    $("#ddlLevelPolicies").UifSelect({
                        sourceData: dataTmp
                    });

                }
            });
        } else {
            $("#ddlLevelPolicies").UifSelect();
        }
    }

    static SetListConceptsDescription(policie) {
        $("#lsvConceptsDescription").UifListView("clear");
        $.each(policie.ConceptsDescription, (i, concept) => {

            let item = {
                Concept: {
                    Description: concept.Concept.Description,
                    ConceptId: concept.Concept.ConceptId,
                    Entity: { EntityId: concept.Concept.Entity.EntityId },
                    ConceptControlType: 1,
                    ConceptType: 1
                },
                EntityDescription: $("#ddlLevelConceptsDescription option[value=" + concept.Concept.Entity.EntityId + "]").text()
            };

            $("#lsvConceptsDescription").UifListView("addItem", item);
        });
    }

    /**
     * @summary 
     * realiza el guardado de la lista temporal de los usuarios
    **/
    static EditTempUsersPolicies(user) {
        let index = -1;
        let filterUser = objUserPolicies.filter((x) => {
            return x.IdUser === user.UserId;
        });
        if (filterUser.length !== 0) {
            index = jQuery.inArray(filterUser[0], objUserPolicies);
        }


        if (index === -1) {
            objUserPolicies.push({
                IdUser: user.UserId,
                UserName: user.Account,
                Authorized: user.Authorized,
                Notificated: user.Notificated,
                Required: user.Required,
                Default: user.Default,
                IdHierarchy: user.hierarchy
            });
        } else {
            if (user.Authorized || user.Notificated) {
                filterUser[0].Required = user.Required;
                filterUser[0].Default = user.Default;

                objUserPolicies[index] = filterUser[0];
            } else {
                objUserPolicies.splice(index, 1);
            }
        }
    }

    /**
     * @summary 
     * setea la tabla de usuarios autorizadores
     * @param {police} politica seleccionada
     * @param {allUsers} todos los usuarios de la jerarquia
      * @param {idHierarchy} jerarquia seleccionada
    **/
    static SetTableAutUsers(police, allUsers, idHierarchy) {
        $("#tableAutUsers").UifDataTable("clear");
        $.each(allUsers, (index, item) => {
            let user = {
                UserId: item.UserId,
                Account: item.AccountName,
                Authorized: false,
                Required: false,
                Default: false,
                hierarchy: idHierarchy,
                DisabledDate: FormatDate(item.DisableDate)
            };


            var filterUser = objUserPolicies.filter((x) => {
                return x.IdUser === item.UserId && x.IdHierarchy === parseInt(idHierarchy);
            });

            if (filterUser.length !== 0) {
                user.Authorized = true;
                user.Required = filterUser[0].Required;
                user.Default = filterUser[0].Default;
            }

            $("#tableAutUsers").UifDataTable("addRow", user);
        });
    }

    /**
    * @summary 
    * setea la tabla de usuarios notificadores
    * @param {police} politica seleccionada
    * @param {allUsers} todos los usuarios de la jerarquia
     * @param {idHierarchy} jerarquia seleccionada
    **/
    static SetTableNotUsers(police, allUsers, idHierarchy) {
        $("#tableNotUsers").UifDataTable("clear");
        $.each(allUsers, (index, item) => {
            let user = {
                UserId: item.UserId,
                Account: item.AccountName,
                Notificated: false,
                Default: false,
                hierarchy: idHierarchy,
                DisabledDate: item.DisabledDate
            };


            var filterUser = objUserPolicies.filter((x) => {
                return x.IdUser === item.UserId && x.IdHierarchy === parseInt(idHierarchy);
            });

            if (filterUser.length !== 0) {
                user.Notificated = true;
                user.Default = filterUser[0].Default;
            }

            $("#tableNotUsers").UifDataTable("addRow", user);
        });
    }

    /**
    * @summary 
    * setea las jerarquias a partir de la politica seleccionada
    * @param {police} politica seleccionada
    **/
    static SetAuthorizedUsers(policie) {
        HierarchyRequest.GetCoHierarchiesAssociationByModuleSubModule(policie.GroupPolicies.Module.Id, policie.GroupPolicies.SubModule.Id)
            .done((data) => {
                var filterList = data.data.filter((item) => {
                    return item.Id < policie.IdHierarchyPolicy;
                });

                filterList.forEach((item) => {
                    let counHierarchy = policie.UserAuthorization.filter((x) => { return x.IdHierarchy === item.Id });
                    if (counHierarchy.length !== 0) {
                        item.Description += ` (${counHierarchy.length})`;
                    }
                });

                $("#ddlHierarchyAutUsers").UifSelect({ sourceData: filterList });
            });

        $("#txtNumberAut").val(policie.NumberAut);
    }

    /**
    * @summary 
    * setea las jerarquias a partir de la politica seleccionada
    * @param {police} politica seleccionada
    **/
    static SetNotificatedUsers(policie) {
        HierarchyRequest.GetCoHierarchiesAssociationByModuleSubModule(policie.GroupPolicies.Module.Id, policie.GroupPolicies.SubModule.Id)
            .done((data) => {
                var filterList = data.data.filter((item) => {
                    return item.Id < policie.IdHierarchyPolicy;
                });


                filterList.forEach((item) => {
                    let counHierarchy = policie.UserNotification.filter((x) => { return x.IdHierarchy === item.Id });
                    if (counHierarchy.length !== 0) {
                        item.Description += ` (${counHierarchy.length})`;
                    }
                });

                $("#ddlHierarchyNotUsers").UifSelect({ sourceData: filterList });
            });
    }

    /**
     * @summary 
     * setea el formulario general de la politica
     * @param {police} politica seleccionada
    **/
    static SetFormPolicie(policie) {
        $("#txtNamePolicie").val(policie.Description);
        $("#txtPackageMain").val(policie.GroupPolicies.Package.Description);
        $("#txtModuleMain").val(policie.GroupPolicies.Module.Description);
        $("#txtSubModuleMain").val(policie.GroupPolicies.SubModule.Description);

        if (policie.Enabled) {
            $("#chkEnabled").prop('checked', true);
        } else {
            $("#chkEnabled").prop('checked', false);
        }

        HierarchyRequest.GetCoHierarchiesAssociationByModuleSubModule(policie.GroupPolicies.Module.Id, policie.GroupPolicies.SubModule.Id)
            .done((data) => {
                $("#ddlHierarchy").UifSelect({ sourceData: data.data });
                $("#ddlHierarchy").UifSelect("setSelected", policie.IdHierarchyPolicy);
            });

        $("#ddlLevelPolicies").UifSelect("setSelected", policie.Position);
        $("#txtMessagePolicie").val(policie.Message);
        $("#ddlTypePolicies").UifSelect("setSelected", policie.Type);
    }

    /**
     * @summary 
     * oculta y muestra las modales
     * @param {modal} id del modal
    **/
    static ShowModalPolicies(modal) {
        $("#tableAutUsers").UifDataTable("clear");
        $("#tableNotUsers").UifDataTable("clear");

        $("#modalPolicieModal").UifModal("hide");
        $("#modalPolicie").UifModal("hide");
        $("#modalAuthorizedUsers").UifModal("hide");
        $("#modalNotificatedUsers").UifModal("hide");
        $("#modalConceptsDescription").UifModal("hide");

        switch (modal) {
            case 1:
                $("#modalPolicie").UifModal("showLocal", "Detalles de la Politica");
                break;
            case 3:
                $("#modalAuthorizedUsers").UifModal("showLocal", "Usuarios Autorizadores");
                break;
            case 4:
                $("#modalNotificatedUsers").UifModal("showLocal", "Usuarios Notificadores");
                break;
            case 5:
                $("#modalConceptsDescription").UifModal("showLocal", "Conceptos Descripcion");
                break;
        }
    }

    /**
    * @summary 
    * obtiene la politica seleccionada
    **/
    static GetPolicieSelected() {
        let policies = $("#lsvPolicies").UifListView("getSelected");
        if (policies.length === 0) {
            $.UifNotify("show", { 'type': "warning", 'message': "Seleccione una Politica", 'autoclose': true });
            return false;
        } else {
            return policies;
        }
    }

    /**
    * @summary 
    * obtiene el index de la politica seleccionada
    * @param {modal} id del modal
    **/
    static GetIndexPolicie(idPolicies) {
        const find = function (element, index, array) {
            return element.IdPolicies === idPolicies;
        }
        return $("#lsvPolicies").UifListView("findIndex", find);
    }

    /**
    * @summary 
    * setea la lista de reglas-politicas segun el filtro
    * @param {idPackage}id del paquete
    * @param {idGroup} id del grupo
    * @param {type} tipo de politica
    * @param {position} posicion de la politica
    * @param {filter} filtro tipo like
    **/
    static SetListRulesPolicies(idPackage, idGroup, type, position, filter) {
        $("#lsvPolicies").UifListView("refresh");
        if (idGroup) {
            RequestRulesPolicies.GetRulesPoliciesByFilter(idPackage, idGroup, type, position, filter)
                .done((data) => {
                    if (data.success) {
                        let policies = data.result;
                        $.each(policies, (index, item) => {
                            item.index = index;
                            $("#lsvPolicies").UifListView("addItem", item);
                        });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }
    }

    
    static SearchDisabled(data) {
        var disable = false;

        if (data != null &&  data.DisabledDate != null)
            disable = true;
        return disable;
    }
    
}

