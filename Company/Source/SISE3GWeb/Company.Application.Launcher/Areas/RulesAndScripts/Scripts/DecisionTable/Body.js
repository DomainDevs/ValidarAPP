class DecisionTableBody extends Uif2.Page {

    getInitialState() {
        $.ajaxSetup({ async: false});

        $.fn.dataTable.ext.errMode = function (data, xhr, error) {
            $.UifNotify("show", { 'type': "danger", 'message': error.split("-")[1], 'autoclose': true });
        };

        $("#Title").text(glbDecisionTable.Description);

        new Promise((resolve1, reject1) => {
            RequestDecisionTable.GetTableDecisionHead(glbDecisionTable.RuleBaseId).done((data1) => {
                if (data1.success) {
                    glbConcepts = [];

                    data1.result.conceptsCondition.forEach((item) => {
                        item.HeadType = "Condition";
                        glbConcepts.push(item);
                    });
                    data1.result.conceptsAction.forEach((item) => {
                        item.HeadType = "Action";
                        glbConcepts.push(item);
                    });

                    var promises = [];
                    glbConcepts.forEach((item, index) => {
                        var promise = new Promise((resolve, reject) => {
                            if (glbConcepts[index].HeadType === "Condition") {
                                RequestConcepts.GetComparatorConcept(item.ConceptId, item.Entity.EntityId).done(
                                    (data3) => {
                                        if (data3.success) {
                                            glbConcepts[index].Comparator = data3.result;
                                            resolve();
                                        } else {
                                            $.UifNotify("show", { 'type': "danger", 'message': data3.result, 'autoclose': true });
                                            reject();
                                        }
                                    });
                            } else {
                                resolve();
                            }
                        }).then(() => {
                            return new Promise((resolve, reject) => {
                                if (item.ConceptDependences.length === 0) {
                                    RequestConcepts.GetSpecificConceptWithVales(item.ConceptId, item.Entity.EntityId, [], item.ConceptType)
                                        .done((data2) => {
                                            if (data2.success) {
                                                data2.result.HeadType = glbConcepts[index].HeadType;
                                                data2.result.Comparator = glbConcepts[index].Comparator;
                                                glbConcepts[index] = data2.result;
                                                resolve();
                                            } else {
                                                $.UifNotify("show", { 'type': "danger", 'message': data2.result, 'autoclose': true });
                                                reject();
                                            }
                                        });
                                } else {
                                    resolve();
                                }
                            });
                        });
                        promises.push(promise);
                    });

                    Promise.all(promises).then(() => {
                        resolve1();
                    });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data1.result, 'autoclose': true });
                    reject1();
                }
            });
        }).then(() => {
            return DecisionTableBody.SetTableHeadDT();
        }).then(() => {
            DecisionTableBody.SetFormModalBuild();
        });
    }

    bindEvents() {
        $("#btnBack").on("click", this.Back);

        $("#tblDecisionTable").on("rowDelete", this.DeleteRowDecisionTable);
        $("#tblDecisionTable").on("rowAdd", this.AddRowDecisionTable);
        $("#tblDecisionTable").on("rowEdit", this.EditRowDecisionTable);
        $("#btnSaveRuleDt").on("click", this.SaveRuleDt);
        $("#btnSaveDecisionTable").on("click", this.SaveDecisionTable);
        $("#btnPublish").on("click", this.Publish);

        $("#tblDecisionTable").on("uift_drawed", this.OnDraw);
        $('#tableSearchResult tbody').on('click', "tr", this.tableResult);
    }
    OnDraw() {
        $('#tblDecisionTable').UifDataTable('setLoading', false)
    }

    Publish() {
        DecisionTableBody.SaveDecisionTable(true);
    }

    SaveDecisionTable() {
        DecisionTableBody.SaveDecisionTable(false);
    }

    SaveRuleDt() {
        let rule;
        let ruleId = $("#hdnRuleId").val();
        if (ruleId) {
            rule = {
                Description: `r_${ruleId}`,
                RuleId: ruleId,
                Conditions: DecisionTableBody.GetConditionModal(),
                Actions: DecisionTableBody.GetActionModal()
            };

            if (!rule.Status || rule.Status !== "Added") {
                rule.Status = "Edited";
            }
        } else {
            rule = {
                RuleId: 0,
                Description: "",
                Conditions: DecisionTableBody.GetConditionModal(),
                Actions: DecisionTableBody.GetActionModal(),
                Status: "Added"
            };
        }

        if (rule.Actions !== false && rule.Conditions !== false) {
            RequestDecisionTable.DecisionTableItem(glbDecisionTable.RuleBaseId, rule, rule.Status).done(result => {
                if (result.success) {
                    $("#tblDecisionTable").UifDataTable("redraw");
                    $("#modalTableEditAddData").UifModal("hide");
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': result.result, 'autoclose': true });
                }
            });
        }
    }

    AddRowDecisionTable() {
        DecisionTableBody.ClearFormModal();
        $("#modalTableEditAddData").UifModal("showLocal", "");
    }

    EditRowDecisionTable(e, item) {
        DecisionTableBody.ClearFormModal();
        DecisionTableBody.GetRuleByItem(glbDecisionTable.RuleBaseId, item.Rule.RuleId)
            .then(rule => {
                var item = {
                    Rule: rule
                }

                DecisionTableBody.SetFormModal(item.Rule, item.Rule.RuleId);
                $("#modalTableEditAddData").UifModal("showLocal", "");
            });

    }

    static GetRuleByItem(ruleBaseId, ruleId) {
        return new Promise((resolve, reject) => {
            RequestDecisionTable.GetRuleByItem(ruleBaseId, ruleId).done(result => {
                if (result.success) {
                    resolve(result.result)
                } else {
                    reject();
                    $.UifNotify("show", { 'type': "danger", 'message': "Ocurrio un error al consultar la información de la regla de este item", 'autoclose': true });
                }
            });
        });
    }
    DeleteRowDecisionTable(event, item, indexRow) {
        $.UifDialog('confirm', {
            title: Resources.Language.DecisionTable,
            message: Resources.Language.DeleteRecord

        }, function (result) {
            if (result) {

                DecisionTableBody.GetRuleByItem(glbDecisionTable.RuleBaseId, item.Rule.RuleId)
                    .then(rules => {
                        var item = {
                            Status: "Deleted",
                            Rule: rules
                        }

                        RequestDecisionTable.DecisionTableItem(glbDecisionTable.RuleBaseId, item.Rule, item.Status).done(result => {
                            if (result.success) {
                                $("#tblDecisionTable").UifDataTable("redraw");
                                $("#modalTableEditAddData").UifModal("hide");
                            } else {
                                $.UifNotify("show", { 'type': "danger", 'message': result.result, 'autoclose': true });
                            }
                        });
                    });
            }
        });
    }

    Back() {
        glbConcepts = null;
        glbDecisionTable = [glbDecisionTable];
        router.run("#prtDecisionTable");
    }

    getConcepts() {
        glbConcepts.forEach((item, index) => {
            RequestConcepts.GetSpecificConceptWithVales(item.ConceptId, item.Entity.EntityId, [], item.ConceptType)
                .done((data2) => {
                    if (data2.success) {
                        data2.result.HeadType = glbConcepts[index].HeadType;
                        glbConcepts[index] = data2.result;

                        if (glbConcepts[index].HeadType === "Condition") {
                            RequestConcepts.GetComparatorConcept(item.ConceptId, item.Entity.EntityId).done(
                                (data3) => {
                                    if (data3.success) {
                                        glbConcepts[index].Comparator = data3.result;
                                    } else {
                                        $.UifNotify("show",
                                            { 'type': "danger", 'message': data3.result, 'autoclose': true });
                                    }
                                });
                        }
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data2.result, 'autoclose': true });
                    }
                });
        });
    }

    static SaveDecisionTable(publish) {
        DecisionTableBody.ClearFormModal();

        const lastRule = {
            Description: "",
            RuleId: 0,
            Conditions: DecisionTableBody.GetConditionModal(),
            Actions: DecisionTableBody.GetActionModal(),
        };

        RequestDecisionTable.SaveDecisionTable(glbDecisionTable.RuleBaseId, lastRule).done((data) => {
            if (data.success) {
                glbDecisionTable.IsPublished = false;
                if (publish) {
                    DecisionTableBody.PublishDecisionTable();
                } else {
                    $("#tblDecisionTable").UifDataTable("redraw");
                    $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                }
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
    }

    static PublishDecisionTable() {
        //$($("#tblDecisionTable tbody tr")).removeClass("row-selected-warning");
        $("#btnPublish").attr("disabled", "disabled");
        $("#btnSaveDecisionTable").attr("disabled", "disabled");
        $.UifNotify("show", { 'type': "info", 'message': "Iniciando el proceso de validación y publicación", 'autoclose': false });

        RequestDecisionTable.PublishDecisionTable(glbDecisionTable.RuleBaseId, GetQueryParameter("IsEvent")).done((data) => {
            $("#btnPublish").removeAttr("disabled");
            $("#btnSaveDecisionTable").removeAttr("disabled");

            if (data.success) {
                $.UifNotify("update", { 'type': "success", 'message': data.result, 'autoclose': false });
                glbDecisionTable.IsPublished = true;
            } else {
                if (Array.isArray(data.result)) {
                    $.UifNotify("update", { 'type': "danger", 'message': "Algunas reglas no se ejecutarán", 'autoclose': false });
                    $("#tblDecisionTable").UifDataTable("redraw");
                } else {
                    $.UifNotify("update", { 'type': "danger", 'message': data.result, 'autoclose': false });
                }
            }
        });
    }

    static GetConditionModal() {
        let conditions = [];
        let isValid = true;

        glbConcepts.forEach((item) => {
            if (item.HeadType === "Condition" && isValid) {
                const controlOpId = $(`#Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`);
                const controlValId = $(`#Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`);

                let condition = {
                    ComparatorType: 1,
                    Concept: {
                        ConceptId: item.ConceptId,
                        Entity: { EntityId: item.Entity.EntityId },
                        ConceptControlType: item.ConceptControlType,
                        ConceptName: item.ConceptName,
                        ConceptType: item.ConceptType,
                        Description: item.Description,
                        KeyOrder: item.KeyOrder,
                        IsStatic: item.IsStatic,
                        IsReadOnly: item.IsReadOnly,
                        IsVisible: item.IsVisible,
                        IsNulleable: item.IsNulleable,
                        IsPersistible: item.IsPersistible
                    },
                    Comparator: null,
                    Expression: null
                };

                if (controlOpId.UifSelect("getSelected") !== "") {
                    condition.Comparator = controlOpId.UifSelect("getSelectedSource");

                    if (condition.Comparator.Symbol !== "Null" && condition.Comparator.Symbol !== "Not null") {
                        switch (item.ConceptControlType) {
                            case 1: //TextBox
                            case 2: //NumberEditor 
                            case 3: //DateEditor
                                condition.Expression = controlValId.val().trim();
                                break;

                            case 4: //ListBox
                                condition.Expression = controlValId.UifSelect("getSelectedSource");
                                break;
                            case 5: //SearchCombo
                                condition.Expression = controlValId.data("object");
                                break;
                        }
                        if (condition.Expression === "" || condition.Expression === undefined || condition.Expression === null) {
                            isValid = false;
                            $.UifNotify("show",
                                {
                                    'type': "danger",
                                    'message': `La condicion ${item.Description} no tiene valor`,
                                    'autoclose': true
                                });
                            return false;
                        }
                    }
                }
                conditions.push(condition);
            }
        });

        if (isValid) {
            return conditions;
        }
        return false;
    }

    static GetActionModal() {
        let actions = [];
        let isValid = true;

        glbConcepts.forEach((item) => {
            if (item.HeadType === "Action" && isValid) {
                const controlOpId = $(`#Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`);
                const controlValId = $(`#Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`);

                let action = {
                    AssignType: 1,
                    ComparatorType: 1,
                    Concept: {
                        ConceptId: item.ConceptId,
                        Entity: { EntityId: item.Entity.EntityId },
                        ConceptControlType: item.ConceptControlType,
                        ConceptName: item.ConceptName,
                        ConceptType: item.ConceptType,
                        Description: item.Description,
                        KeyOrder: item.KeyOrder,
                        IsStatic: item.IsStatic,
                        IsReadOnly: item.IsReadOnly,
                        IsVisible: item.IsVisible,
                        IsNulleable: item.IsNulleable,
                        IsPersistible: item.IsPersistible
                    },
                    Expression: null,
                    ArithmeticOperator: null

                };

                if (controlOpId.UifSelect("getSelected") !== "") {
                    action.ArithmeticOperator = controlOpId.UifSelect("getSelectedSource");
                    action.ArithmeticOperator.ArithmeticOperatorType = action.ArithmeticOperator.Id;

                    switch (item.ConceptControlType) {
                        case 1://TextBox
                        case 2://NumberEditor
                        case 3://DateEditor
                            action.Expression = controlValId.val();

                            break;
                        case 4://ListBox
                            action.Expression = controlValId.UifSelect("getSelectedSource");
                            break;
                        case 5://SearchCombo
                            action.Expression = controlValId.data("object");
                            break;
                    }

                    if (action.Expression === "" || action.Expression === undefined) {
                        isValid = false;
                        $.UifNotify("show", { 'type': "danger", 'message': `La acción ${item.Description} no tiene valor`, 'autoclose': true });
                        return false;
                    }
                }

                actions.push(action);
            }
        });

        if (isValid) {
            return actions;
        }
        return false;
    }

    static SetFormModal(data, ruleId) {
        $("#hdnRuleId").val(ruleId);
        data.Conditions.forEach((item) => {
            const concept = glbConcepts.find(x => {
                return x.HeadType === "Condition" && x.Entity.EntityId === item.Concept.Entity.EntityId && x.ConceptId === item.Concept.ConceptId;
            });
            const controlOpId = $(`#Op${concept.HeadType}-${concept.Entity.EntityId}-${concept.ConceptId}`);
            const controlValId = $(`#Val${concept.HeadType}-${concept.Entity.EntityId}-${concept.ConceptId}`);

            if (item.Comparator !== null) {
                controlOpId.UifSelect("setSelected", item.Comparator.Operator);

                if (item.Expression !== null) {
                    switch (concept.ConceptControlType) {
                        case 1://TextBox
                        case 2://NumberEditor
                            controlValId.val(item.Expression);
                            break;

                        case 3://DateEditor
                            controlValId.UifDatepicker("setValue", item.Expression);
                            break;

                        case 4://ListBox
                            if (item.Expression.RangeValueCode !== undefined) {
                                controlValId.UifSelect("setSelected", item.Expression.RangeValueCode);
                            } else if (item.Expression.ListValueCode !== undefined) {
                                controlValId.UifSelect("setSelected", item.Expression.ListValueCode);
                            }
                            else if (item.Expression.Id !== undefined) {
                                controlValId.UifSelect("setSelected", item.Expression.Id);
                                controlValId.trigger("change");
                            }
                            break;
                        case 5://SearchCombo
                           if (item.Expression.Id !== undefined) {
                                controlValId.val(item.Expression.Description);
                                controlValId.data("object", item.Expression);                                
                                DecisionTableBody.eventSelectedTable(controlValId);
                            }
                            break;
                    }
                }
            }
        });
        data.Actions.forEach((item) => {
            const concept = glbConcepts.find(x => {
                return x.HeadType === "Action" && x.Entity.EntityId === item.Concept.Entity.EntityId && x.ConceptId === item.Concept.ConceptId;
            });
            const controlOpId = $(`#Op${concept.HeadType}-${concept.Entity.EntityId}-${concept.ConceptId}`);
            const controlValId = $(`#Val${concept.HeadType}-${concept.Entity.EntityId}-${concept.ConceptId}`);

            if (item.ArithmeticOperator !== null) {
                controlOpId.UifSelect("setSelected", item.ArithmeticOperator.ArithmeticOperatorType);
                switch (concept.ConceptControlType) {
                    case 1://TextBox
                    case 2://NumberEditor
                        controlValId.val(item.Expression);
                        break;

                    case 3://DateEditor
                        controlValId.UifDatepicker("setValue", item.Expression);
                        break;

                    case 4://ListBox
                        if (item.Expression.RangeValueCode !== undefined) {
                            controlValId.UifSelect("setSelected", item.Expression.RangeValueCode);
                        } else if (item.Expression.ListValueCode !== undefined) {
                            controlValId.UifSelect("setSelected", item.Expression.ListValueCode);
                        }
                        else if (item.Expression.Id !== undefined) {
                            controlValId.UifSelect("setSelected", item.Expression.Id);
                            controlValId.trigger("change");
                        }
                        break;
                    case 5://SearchCombo
                        if (item.Expression.Id !== undefined) {
                            controlValId.val(item.Expression.Description);
                            controlValId.data("object", item.Expression);
                            DecisionTableBody.eventSelectedTable(controlValId);
                        }
                        break;
                }
            }
        });
    }

    static ClearFormModal() {
        $("#hdnRuleId").val("");
        glbConcepts.forEach((item) => {
            const controlOpId = $(`#Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`);
            const controlValId = $(`#Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`);

            controlOpId.UifSelect("setSelected", "");
            switch (item.ConceptControlType) {
                case 1://TextBox
                case 2://NumberEditor
                    controlValId.val("");
                    break;

                case 3://DateEditor
                    controlValId.UifDatepicker("clear");
                    break;

                case 4://ListBox
                    if (item.ConceptDependences === null || item.ConceptDependences.length === 0) {
                        controlValId.UifSelect("setSelected", "");
                    } else {
                        controlValId.UifSelect({
                            sourceData: null,
                            id: "Id",
                            name: "Description",
                            //native: false,
                            //filter: true
                        });
                    }
                    break;
                case 5://SearchCombo
                    if (item.ConceptDependences === null || item.ConceptDependences.length === 0) {
                        controlValId.val("");
                        controlValId.data("object", null);
                    }
                    else {
                        controlValId.UifInputSearch();
                        controlValId.val("");
                        controlValId.data("object", null);
                        controlValId.data("sourceData", null);
                    }
                    break;
            }
        });
    }

    static SetFormModalBuild() {
        glbConcepts.forEach((item) => {
            const row = $(`<div class="uif-col-12"></div>`);
            const name = $(`<div class="uif-col-4"><label>${item.Description}</label></div>`);
            let operator;
            let value;

            if (item.HeadType === "Condition") {
                operator = $(`<div class="uif-col-4"><select class="uif-select Op${item.HeadType}" id="Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}"></select></div>`);
                value = DecisionTableBody.GetComponentValue(item);

                row.append(name);
                row.append(operator);
                row.append(value[0]);

                $("#divModalConditions").append(row);

                item.Comparator = item.Comparator.filter(x => { return x.Symbol !== "Null" && x.Symbol !== "Not null" });

                $(`#Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`).UifSelect({
                    sourceData: item.Comparator,
                    id: "Operator",
                    name: "Description"
                });
                value[1](item);
            }

            if (item.HeadType === "Action") {
                operator = $(`<div class="uif-col-4"><select class="uif-select Op${item.HeadType}" id="Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}"></select></div>`);
                value = DecisionTableBody.GetComponentValue(item);

                row.append(name);
                row.append(operator);
                row.append(value[0]);

                $("#divModalActions").append(row);

                $(`#Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`).UifSelect({
                    sourceData: item.Operator,
                    id: "Id",
                    name: "Description"
                });
                value[1](item);
            }

            $(`#Op${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}`).on("change", this.eventChangeSelect);
        });
    }

    static eventSelectedTable(target) {
            const id = target.attr("id").split("-");
            let itemOp;
            let itemVal;

            itemVal = target.data("object");
            if (id[0] === "ValCondition") {
                itemOp = $(`#OpCondition-${id[1]}-${id[2]}`).UifSelect("getSelectedSource");
            } else if (id[0] === "ValAction") {
                itemOp = $(`#OpAction-${id[1]}-${id[2]}`).UifSelect("getSelectedSource");
            }

            let conceptsTmp;
            let type;
            if (id[0].indexOf("Condition") !== -1) {
                type = "Condition";
                conceptsTmp = glbConcepts.filter(x => {
                    return x.HeadType === type;
                });
            } else if (id[0].indexOf("Action") !== -1) {
                type = "Action";
                conceptsTmp = glbConcepts.filter(x => {
                    return x.HeadType === type;
                });
            }

            let conceptsDependency = [];

            conceptsTmp.forEach((c) => {
                if (c.ConceptDependences !== null) {
                    c.ConceptDependences.forEach((cd) => {
                        if (cd.DependsConcept.ConceptId === parseInt(id[2]) &&
                            cd.DependsConcept.Entity.EntityId === parseInt(id[1])) {
                            conceptsDependency.push(c);
                        }
                    });
                }
            });


            conceptsDependency.forEach((c) => {
                if (c.ConceptDependences !== null) {
                    let dependency = true;
                    let dependencies = [];
                    c.ConceptDependences.forEach((cd) => {
                        let parent = $(`#Val${type}-${cd.DependsConcept.Entity.EntityId}-${cd.DependsConcept.ConceptId}`).data("object");
                        if (parent === undefined || parent === null) {
                            dependency = false;
                        } else {
                            dependencies.push(parent.Id);
                        }
                    });

                    if (dependency === true) {
                        if (itemOp !== undefined && itemVal !== undefined && itemOp.Symbol === "=") {
                            RequestConcepts.GetSpecificConceptWithVales(c.ConceptId,
                                c.Entity.EntityId,
                                dependencies,
                                c.ConceptType)
                                .done((data) => {
                                    if (data.success) {
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).UifInputSearch();
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).val("");
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("object", null);
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("sourceData", data.result);
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).off().on("search", this.eventTableSearch);
                                    } else {
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).UifSelect();
                                        $.UifNotify("show",
                                            { 'type': "danger", 'message': data.result, 'autoclose': true });
                                    }
                                });
                        } else {
                            $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).val("");
                            $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("object", null);
                            $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("sourceData", null);
                        }
                    } else {
                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).val("");
                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("object", null);
                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("sourceData", null);
                    }
                }
            });
    }

    static eventChangeSelect(e) {
        const target = $(e.currentTarget);
        if (target.is("select")) {
            const id = target.attr("id").split("-");
            let itemOp;
            let itemVal;

            if (target.hasClass("OpCondition")) {
                itemOp = target.UifSelect("getSelectedSource");
                itemVal = $(`#ValCondition-${id[1]}-${id[2]}`).data("object");
            } else if (target.hasClass("OpAction")) {
                itemOp = target.UifSelect("getSelectedSource");
                itemVal = $(`#ValAction-${id[1]}-${id[2]}`).data("object");
            } else {
                itemVal = target.UifSelect("getSelectedSource");
                if (id[0] === "ValCondition") {
                    itemOp = $(`#OpCondition-${id[1]}-${id[2]}`).UifSelect("getSelectedSource");
                } else if (id[0] === "ValAction") {
                    itemOp = $(`#OpAction-${id[1]}-${id[2]}`).UifSelect("getSelectedSource");
                }
            }

            let conceptsTmp;
            let type;
            if (id[0].indexOf("Condition") !== -1) {
                type = "Condition";
                conceptsTmp = glbConcepts.filter(x => {
                    return x.HeadType === type;
                });
            } else if (id[0].indexOf("Action") !== -1) {
                type = "Action";
                conceptsTmp = glbConcepts.filter(x => {
                    return x.HeadType === type;
                });
            }

            let conceptsDependency = [];

            conceptsTmp.forEach((c) => {
                if (c.ConceptDependences !== null) {
                    c.ConceptDependences.forEach((cd) => {
                        if (cd.DependsConcept.ConceptId === parseInt(id[2]) &&
                            cd.DependsConcept.Entity.EntityId === parseInt(id[1])) {
                            conceptsDependency.push(c);
                        }
                    });
                }
            });


            conceptsDependency.forEach((c) => {
                if (c.ConceptDependences !== null) {
                    let dependency = true;
                    let dependencies = [];
                    c.ConceptDependences.forEach((cd) => {
                        let parent = $(`#Val${type}-${cd.DependsConcept.Entity.EntityId}-${cd.DependsConcept.ConceptId}`).data("object");
                        if (parent === undefined || parent === null) {
                            dependency = false;
                        } else {
                            dependencies.push(parent.Id);
                        }
                    });

                    if (dependency === true) {
                        if (itemOp !== undefined && itemVal !== undefined && itemOp.Symbol === "=") {
                            RequestConcepts.GetSpecificConceptWithVales(c.ConceptId,
                                c.Entity.EntityId,
                                dependencies,
                                c.ConceptType)
                                .done((data) => {
                                    if (data.success) {
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).UifInputSearch();
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).val("");
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("object", null);
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("sourceData", data.result);
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).off().on("search", this.eventSelectedTable);

                                        
                                    } else {
                                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).UifSelect();
                                        $.UifNotify("show",
                                            { 'type': "danger", 'message': data.result, 'autoclose': true });
                                    }
                                });
                        } else {
                            $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).UifInputSearch();
                            $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).val("");
                            $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("object", null);
                            $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("sourceData", null);                            
                        }
                    } else {
                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).UifInputSearch();
                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).val("");
                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("object", null);
                        $(`#Val${type}-${c.Entity.EntityId}-${c.ConceptId}`).data("sourceData", null);                        
                    }
                }
            });
        }
    }

    static GetComponentValue(item) {
        
        let value = ["", ""];

        switch (item.ConceptControlType) {
            case 1://TextBox
                value[0] = $(`<div class="uif-col-4"><input type="text" id="Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}"/></div>`);
                value[1] = () => { };
                break;

            case 2://NumberEditor
                value[0] = $(`<div class="uif-col-4"><input type="text" id="Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}"/></div>`);
                if (item.BasicType === 1) {
                    value[1] = (x) => {
                        $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).ValidatorKey(ValidatorType.RealNumbers, 3, 0);
                    };
                }
                else if (item.BasicType === 3) {
                    value[1] = (x) => {
                        $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).OnlyDecimals(15);
                    };
                }
                break;

            case 3://DateEditor
                value[0] = $(`<div class="uif-col-4"><input type="text" class="uif-datepicker" id="Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}"/></div>`);
                value[1] = (x) => {
                    $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).UifDatepicker();
                };
                break;

            case 4://ListBox
                value[0] = $(`<div class="uif-col-4"><select class="uif-select" id="Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}"></select></div>`);
                value[1] = (x) => {
                    if (x.ConceptType === 2) {//rango
                        x.RangeEntityValues.forEach((rang) => {
                            rang.Description = `${rang.FromValue} - ${rang.ToValue}`;
                        });
                        $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).UifSelect({
                            sourceData: x.RangeEntity.RangeEntityValues,
                            id: "RangeValueCode",
                            name: "Description"
                        });
                    }
                    else if (x.ConceptType === 3) { //Lista
                        $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).UifSelect({
                            sourceData: x.ListEntity.ListEntityValues,
                            id: "ListValueCode",
                            name: "ListValue"
                        });
                    }
                };
                break;

            case 5://SearchCombo
                value[0] = $(`<div class="uif-col-4"><input type="text" class="uif-input-search" id="Val${item.HeadType}-${item.Entity.EntityId}-${item.ConceptId}"/></div>`);
                value[1] = (x) => {
                    $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).UifInputSearch();
                    $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).val("");
                    $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).data("object", null);
                    $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).data("sourceData", x);
                    $(`#Val${x.HeadType}-${x.Entity.EntityId}-${x.ConceptId}`).off().on("search", this.eventTableSearch);

                };
                break;
        }

        return value;
    }

    static eventTableSearch(e, description) {
        const target = $(e.currentTarget);
        const idSearch = target.attr("id");
        $("#hiddenIdConceptTableSearch").val(idSearch);
        $(`#${idSearch}`).val("");
        $(`#${idSearch}`).data("object", null);
        var resultExpressionSearch = [];
        var dataTable = $(`#${idSearch}`).data("sourceData");
        $.each(dataTable != null ? dataTable.ReferenceValues : null , function (key, value) {
            if (value.Description.toLowerCase().sistranReplaceAccentMark().
                includes(description.toLowerCase().sistranReplaceAccentMark()) ||
                value.Id.toLowerCase().sistranReplaceAccentMark().
                    includes(description.toLowerCase().sistranReplaceAccentMark()))
            {
                resultExpressionSearch.push(value);
            }
        });

        if (resultExpressionSearch.length == 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true })
        }
        else if (resultExpressionSearch.length == 1) {
            $(`#${idSearch}`).val(resultExpressionSearch[0].Description);
            $(`#${idSearch}`).data("object", resultExpressionSearch[0]);
            DecisionTableBody.eventSelectedTable($(`#${idSearch}`));
        }
        else {
            $('#tableSearchResult').UifDataTable('clear');
            $("#tableSearchResult").UifDataTable({ sourceData: resultExpressionSearch })
            $('#modalTableSearchResult').UifModal('showLocal', dataTable.Description);
        }
    }

    tableResult() {
        const idSearch = $("#hiddenIdConceptTableSearch").val();
        $(`#${idSearch}`).val($('#tableSearchResult').DataTable().row(this).data().Description);
        $(`#${idSearch}`).data("object", $('#tableSearchResult').DataTable().row(this).data());
        $('#modalTableSearchResult').UifModal("hide");
        DecisionTableBody.eventSelectedTable($(`#${idSearch}`));
    }

    static SetTableHeadDT() {
        let operador;
        return new Promise((resolve, reject) => {
            RequestRules.GetArithmeticOperatorType().done((data) => {
                if (data.success) {
                    operador = data.result;

                    let index = 0;
                    let separator = false;

                    glbConcepts.forEach((item) => {
                        let element;

                        if (item.HeadType === "Condition") {
                            element = $(`<th data-property="Conditions.${index}">${item.Description}</th>`);
                            $("#tblDecisionTable thead tr").append(element);
                            index++;
                            return;
                        }

                        if (!separator) {
                            element = $(`<th  data-property="Separator"> >>>> </th>`);
                            $("#tblDecisionTable thead tr").append(element);
                            index = 0;
                            separator = true;
                        }

                        if (item.HeadType === "Action") {
                            element = $(`<th data-property="Actions.${index}">${item.Description}</th>`);
                            $("#tblDecisionTable thead tr").append(element);

                            if (item.BasicType === 2) {
                                item.Operator = operador.filter(x => {
                                    return x.Symbol === "=" || x.Symbol === "+";
                                });

                            } else if (item.BasicType === 4 ||
                                item.ConceptType === 2 ||
                                item.ConceptType === 3 ||
                                item.ConceptType === 4) {
                                item.Operator = operador.filter(x => {
                                    return x.Symbol === "=";
                                });
                            } else {
                                item.Operator = operador.filter(x => {
                                    return x.Symbol !== "Round ";
                                });
                            }
                            index++;
                            return;
                        }
                    });

                    $("#tblDecisionTable").UifDataTable("setLoading", true);
                    $("#tblDecisionTable").UifDataTable({ source: rootPath + "RulesAndScripts/DecisionTable/GetTableDecisionBody?ruleBaseId=" + glbDecisionTable.RuleBaseId, "serverSide": true });

                    resolve();
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    reject();
                }
            });
        });
    }
}

