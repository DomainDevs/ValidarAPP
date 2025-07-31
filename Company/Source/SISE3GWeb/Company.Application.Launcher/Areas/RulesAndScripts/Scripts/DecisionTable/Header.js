class DecisionTableHeader extends Uif2.Page {

    getInitialState() {
        $.ajaxSetup({ async: false });
        $("#LsvCondition").UifListView({
            displayTemplate: "#template-Condition-Action",
            delete: true,
            add: true,
            customAdd: true,
            //drag: true,
            height: 250,
            title: Resources.Language.LabelConceptsCondition,
            deleteCallback: this.deleteConcept.bind(this, "Condition")
        });
        $("#LsvAction").UifListView({
            displayTemplate: "#template-Condition-Action",
            delete: true,
            add: true,
            customAdd: true,
            //drag: true,
            height: 250,
            title: Resources.Language.LabelConceptsAction,
            deleteCallback: this.deleteConcept.bind(this, "Action")
        });
        $("#ddlConcepts").UifSelect();

        if (isEvent === "true") {
            RequestPackage.GetPackagePolicies().done((data) => {
                if (data.success) {
                    $("#ddlPackage").UifSelect({
                        sourceData: data.result,
                        selectedId: glbDecisionTable == null ? 0 : glbDecisionTable.Package.PackageId
                    });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            RequestPackage.GetPackages().done((data) => {
                if (data.success) {
                    $("#ddlPackage").UifSelect({
                        sourceData: data.result,
                        selectedId: glbDecisionTable == null ? 0 : glbDecisionTable.Package.PackageId
                    });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }

        if (glbDecisionTable) {
            $("#txtDescriptionRuleBase").val(glbDecisionTable.Description);
            RequestLevels.GetLevelsByIdPackage(glbDecisionTable.Package.PackageId).done((data) => {
                if (data.success) {
                    $("#ddlLevel").UifSelect({
                        sourceData: data.result,
                        selectedId: glbDecisionTable.Level.LevelId
                    });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });

            DecisionTableHeader.GetConcepts();
            $("#ddlConcepts").UifSelect({ sourceData: glbConcepts });

            RequestDecisionTable.GetTableDecisionHead(glbDecisionTable.RuleBaseId).done((data) => {
                if (data.success) {
                    DecisionTableHeader.SetListConditions(data.result.conceptsCondition);
                    DecisionTableHeader.SetListActions(data.result.conceptsAction);
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });


            $("#ddlPackage").attr("disabled", "disabled");
            $("#ddlLevel").attr("disabled", "disabled");
        }
        else {
            $("#ddlPackage").removeAttr("disabled");
            $("#ddlLevel").removeAttr("disabled");
            $("#ddlLevel").UifSelect();
        }
    }

    bindEvents() {
        $("#LsvCondition").on("rowAdd", this.AddConcept.bind(this, "condition"));
        $("#LsvAction").on("rowAdd", this.AddConcept.bind(this, "action"));

        $("#btnSaveConceptHeader").on("click", this.SaveConcept);

        $("#btnBack").on("click", DecisionTableHeader.Back);
        $("#btnSaveHead").on("click", this.SaveHead);
        $("#ddlPackage").on("itemSelected", this.ChangePackage);
        $("#ddlLevel").on("itemSelected", this.ChangeLevel);
    }

    ChangeLevel(e, item) {
        glbConcepts = null;
        if (item.Id) {
            DecisionTableHeader.GetConcepts();
        }
        $("#ddlConcepts").UifSelect({ sourceData: glbConcepts });
    }

    ChangePackage(e, item) {
        if (item.Id) {
            RequestLevels.GetLevelsByIdPackage(item.Id).done((data) => {
                if (data.success) {
                    $("#ddlLevel").UifSelect({
                        sourceData: data.result
                    });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SaveHead() {
        if ($("#formRuleBase").valid()) {
            let conditions = $("#LsvCondition").UifListView("getData");
            let actions = $("#LsvAction").UifListView("getData");

            if (conditions.length === 0) {
                $.UifNotify("show", { 'type': "danger", 'message': "Debe asignar al menos un concepto en las condiciones", 'autoclose': true });
                return;
            }
            if (actions.length === 0) {
                $.UifNotify("show", { 'type': "danger", 'message': "Debe asignar al menos un concepto en las acciones", 'autoclose': true });
                return;
            }

            if (glbDecisionTable && glbDecisionTable.RuleBaseId) {//update
                glbDecisionTable.Description = $("#txtDescriptionRuleBase").val();

                RequestDecisionTable.UpdateTableDecisionHead(glbDecisionTable, conditions, actions).done((data) => {
                    if (data.success) {
                        $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                        DecisionTableHeader.Back();
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });

            } else {//create
                glbDecisionTable = {
                    Description: $("#txtDescriptionRuleBase").val(),
                    IsEvent: GetQueryParameter("IsEvent"),
                    Level: $("#ddlLevel").UifSelect("getSelectedSource"),
                    Package: $("#ddlPackage").UifSelect("getSelectedSource"),
                    RuleBaseType: 2
                }
                RequestDecisionTable.CreateTableDecisionHead(glbDecisionTable, conditions, actions).done((data) => {
                    if (data.success) {
                        glbDecisionTable = data.result;
                        $.UifNotify("show", { 'type': "success", 'message': AppResources.MessageSavedSuccessfully, 'autoclose': true });
                        DecisionTableHeader.Back();
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }

   static Back() {
       glbConcepts = null;
       if (glbDecisionTable != null && glbDecisionTable != undefined)
       {
           glbDecisionTable = [glbDecisionTable];
       }
       router.run("#prtDecisionTable");
    }

    SaveConcept() {
        if ($("#formConceptHeader").valid()) {
            let concept = $("#ddlConcepts").UifSelect("getSelectedSource");
            let type = $("#hdnType").val();
            let index = $("#hdnIdIndex").val() === "" ? null : parseInt($("#hdnIdIndex").val());
            let save = true;
            let list;

            if (type === "condition") {
                list = $("#LsvCondition");
            } else if (type === "action") {
                list = $("#LsvAction");
            }

            let data = list.UifListView("getData");
            data.forEach((item, i) => {
                if (item.ConceptIdFacadeId === concept.ConceptIdFacadeId) {
                    if (i !== index) {
                        save = false;
                        $.UifNotify("show", { 'type': "danger", 'message': "El concepto ya se ha asignado", 'autoclose': true });
                        return;
                    }
                }
            });

            if (concept.ConceptDependences.length > 0) {
                concept.ConceptDependences.forEach((c) => {
                    let conceptTmp = data.find((x) => { return x.ConceptId === c.DependsConcept.ConceptId && x.Entity.EntityId === c.DependsConcept.Entity.EntityId });

                    if (conceptTmp === undefined) {
                        save = false;
                        $.UifNotify("show", { 'type': "info", 'message': `Se debe asignar el concepto: "${c.DependsConcept.Description}"`, 'autoclose': true });
                    }
                });
            }

            if (save) {
                if (!index) {
                    list.UifListView("addItem", concept);
                } else {
                    list.UifListView("editItem", concept, index);
                }
                $("#modalConceptHeader").UifModal("hide");
            }
        }
    }

    AddConcept(param) {
        DecisionTableHeader.OpenModalConceptHeader();
        $("#hdnType").val(param);
    }

    deleteConcept(type, deferred, item) {
        let list;
        if (type === "Condition") {
            list = $("#LsvCondition");
        } else if (type === "Action") {
            list = $("#LsvAction");
        }

        let concepts = list.UifListView("getData");
        let hasDependency = false;
        let dependence = [];

        concepts.forEach((c) => {
            c.ConceptDependences.forEach((cd) => {
                if (cd.DependsConcept.ConceptId === item.ConceptId && cd.DependsConcept.Entity.EntityId === item.Entity.EntityId) {
                    hasDependency = true;
                    dependence.push(c);
                }
            });
        });



        if (hasDependency === true) {
            let message = `Desea eliminar el concepto "${item.Description}" y sus conceptos dependientes?`;
            dependence.forEach((x) => {
                message += `\n* ${x.Description}`;
            });

            $.UifDialog("confirm", {
                "title": "Eliminar dependencias",
                "message": message
            }, function (result) {
                if (result === true) {
                    let newConcepts = [];
                    dependence.push(item);

                    concepts.forEach((c) => {
                        if (dependence.find((x) => { return x.ConceptId === c.ConceptId && x.Entity.EntityId === c.Entity.EntityId }) === undefined) {
                            newConcepts.push(c);
                        }
                    });

                    deferred.resolve();

                    if (type === "Condition") {
                        DecisionTableHeader.SetListConditions(newConcepts);
                    } else if (type === "Action") {
                        DecisionTableHeader.SetListActions(newConcepts);
                    }
                }
            });
        }
        else {
            deferred.resolve();
        }
    }

    static OpenModalConceptHeader() {
        $("#modalConceptHeader").UifModal("showLocal", "Conceptos");
        $("#ddlConcepts").UifSelect("setSelected", 0);
        $("#hdnType").val("");
        $("#hdnIdIndex").val("");
    }

    static SetListConditions(data) {
        $("#LsvCondition").UifListView("clear");
        data.forEach((item) => {

            let concept = glbConcepts.filter((c) => {
                return c.Entity.EntityId === item.Entity.EntityId && c.ConceptId === item.ConceptId;
            })[0];

            $("#LsvCondition").UifListView("addItem", concept);
        });
    }

    static SetListActions(data) {
        $("#LsvAction").UifListView("clear");
        data.forEach((item) => {

            let concept = glbConcepts.filter((c) => {
                return c.Entity.EntityId === item.Entity.EntityId && c.ConceptId === item.ConceptId;
            })[0];

            $("#LsvAction").UifListView("addItem", concept);
        });
    }

    static GetConcepts() {
        if (!glbConcepts) {
            let packageId = $("#ddlPackage").UifSelect("getSelected");
            let levelId = $("#ddlLevel").UifSelect("getSelected");

            RequestRules.GetEntitiesByPackageIdLevelId(packageId, levelId).done((dataF) => {
                if (dataF.success) {

                    let entities = [];
                    dataF.result.forEach((item) => {
                        entities.push(item.EntityId);
                    });

                    RequestConcepts.GetConceptsByFilter(entities, "").done((data) => {
                        if (data.success) {
                            data.result.forEach((concept, index) => {
                                let facade = dataF.result.filter((facades) => {
                                    return facades.EntityId === concept.Entity.EntityId;
                                })[0];

                                data.result[index].DescriptionFacade = data.result[index].Description + " (" + facade.Description + ")";
                                data.result[index].ConceptIdFacadeId = concept.ConceptId + "-" + concept.Entity.EntityId;
                            });

                            glbConcepts = data.result;
                        } else {
                            $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                        }
                    });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
    }
}