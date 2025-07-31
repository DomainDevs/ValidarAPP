class ModalConditionRule extends Uif2.Page {

    getInitialState() {
        if (gblPolicies.GroupPolicies) {
            RequestLevels.GetLevelsByIdGroupPolicies(gblPolicies.GroupPolicies.IdGroupPolicies, gblPolicies.Position)
                .done((data) => {
                    if (data.success) {
                        let lista = [];
                        let valid = true;
                        data.result.forEach((item) => {
                            if (valid) {
                                lista.push(item.EntityId);
                            }
                            if (item.Description === gblPolicies.GroupPolicies.EntityDescription) {
                                valid = false;
                            }
                        });
                        RequestConcepts.GetConceptsByFilter(lista, "")
                            .done((result) => {
                                if (result.success) {
                                    result.result.forEach((concept, index) => {
                                        let facade = data.result.filter((facades) => {
                                            return facades.EntityId === concept.Entity.EntityId;
                                        })[0];
                                        result.result[index].DescriptionFacade =
                                            concept.Description + " (" + facade.Description + ")";
                                        result.result[index].ConceptIdFacadeId =
                                            concept.ConceptId + "-" + concept.Entity.EntityId;
                                    });

                                    $("#ddlConceptCondition").UifSelect({
                                        sourceData: result.result,
                                        native: false
                                    });
                                    $("#ddlActionConcept").UifSelect({
                                        sourceData: result.result,
                                        native: false
                                    });
                                    $("#ddlTempExpressionValue").UifSelect({
                                        sourceData: result.result,
                                        native: false
                                    });

                                    objConcepts = result.result;
                                } else {
                                    $.UifNotify("show",
                                        { 'type': "danger", 'message': result.result, 'autoclose': true });
                                }
                            });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }

        RequestRules.GetConditionComparatorType()
            .done((data) => {
                if (data.success) {
                    $("#ddlConditionComparatorType").UifSelect({ sourceData: data.result });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });

        $("#ddlConditionIs").UifSelect();
        ModalConditionRule.ShowPanel(null);

        $("#txtExpressionNumber").ValidatorKey(ValidatorType.RealNumbers);
        $("#txtExpressionDecimal").OnlyDecimals(5);
    }

    bindEvents() {
        $("#lsvCondition").on("rowAdd", this.AddConditionRule);
        $("#lsvCondition").on("rowEdit", this.EditConditionRule);
        $("#btnSaveConditionRule").on("click", this.SaveConditionRule);
        $("#ddlConceptCondition").on("itemSelected", this.ChangeConceptCondition);
        $("#ddlConditionIs").on("itemSelected", this.ChangeConditionComparator);
        $("#ddlConditionComparatorType").on("itemSelected", this.ChangeConditionComparatorType);

        $("#txtExpressionText").on("keyup", this.EnabledSaveButton.bind(this, "ExpressionText"));
        $("#txtExpressionNumber").on("keyup", this.EnabledSaveButton.bind(this, "ExpressionNumber"));
        $("#txtExpressionDecimal").on("keyup", this.EnabledSaveButton.bind(this, "ExpressionDecimal"));
        $("#txtExpressionDate").on("keyup", this.EnabledSaveButton.bind(this, "ExpressionDate"));
        $("#txtExpressionDate").on("datepicker.change", this.EnabledSaveButton.bind(this, "ExpressionDate"));
        $("#dllExpressionList").on("itemSelected", this.EnabledSaveButton.bind(this, "ExpressionList"));
        $('#tableConceptResult tbody').on('click', "tr", ModalConditionRule.TableResult);
    }

    static TableResult() {
        objectRules = [];
        $("#txtExpressionSearch").val($('#tableConceptResult').DataTable().row(this).data().Description);
        $("#txtExpressionSearch").data("object",$('#tableConceptResult').DataTable().row(this).data());
        objectRules = $('#tableConceptResult').DataTable().row(this).data();
        $("#btnSaveConditionRule").removeAttr("disabled");
        $('#modalConceptTableResult').UifModal("hide");
    }

    EnabledSaveButton(item) {
        $("#btnSaveConditionRule").attr("disabled", "disabled");
        switch (item) {
            case "ExpressionText":
                if ($("#txtExpressionText").val()) {
                    $("#btnSaveConditionRule").removeAttr("disabled");
                }

                break;

            case "ExpressionNumber":
                if ($("#txtExpressionNumber").val()) {
                    $("#btnSaveConditionRule").removeAttr("disabled");
                }
                break;

            case "ExpressionDecimal":
                if ($("#txtExpressionDecimal").val()) {
                    $("#btnSaveConditionRule").removeAttr("disabled");
                }
                break;

            case "ExpressionDate":
                if ($("#txtExpressionDate").val()) {
                    $("#btnSaveConditionRule").removeAttr("disabled");
                }
                break;

            case "ExpressionList":
                if ($("#dllExpressionList").val()) {
                    $("#btnSaveConditionRule").removeAttr("disabled");
                }
                break;
        }
    }

    ChangeConditionComparatorType(e, item) {
        
        if (item.Id) {
            let conceptTmp = $("#ddlConceptCondition").UifSelect("getSelectedSource");
            switch (item.Id) {
                case "1": //Constant Value
                    var dependence = [];
                    if (conceptTmp.ConceptDependences.length > 0) {
                        let conditions = $("#lsvCondition").UifListView("getData");
                        conceptTmp.ConceptDependences.forEach((c) => {
                            let conceptTmp2 = conditions.find((x) => { return x.Concept.ConceptId === c.DependsConcept.ConceptId && x.Concept.Entity.EntityId === c.DependsConcept.Entity.EntityId });
                            dependence.push(conceptTmp2.Expression.Id);
                        });
                    }

                    RequestConcepts.GetSpecificConceptWithVales(conceptTmp.ConceptId, conceptTmp.Entity.EntityId, dependence, conceptTmp.ConceptType)
                        .done((data) => {
                            if (data.success) {
                                let conceptTemp = data.result;

                                switch (conceptTmp.ConceptControlType) {
                                    case 1://TextBox
                                        if (conceptTemp.BasicType === 2) {
                                            ModalConditionRule.ShowPanel(2);
                                            if (conceptTemp.Length) {
                                                $("#txtExpressionText").attr("maxlength", conceptTemp.Length);
                                            }
                                        }
                                        break;

                                    case 2://NumberEditor
                                        if (conceptTemp.BasicType === 1)//Numeric 
                                        {
                                            ModalConditionRule.ShowPanel(3);
                                            if (conceptTemp.MaxValue) {
                                                $("#txtExpressionNumber").attr("max", conceptTemp.MaxValue);
                                            }
                                            if (conceptTemp.MinValue) {
                                                $("#txtExpressionNumber").attr("min", conceptTemp.MinValue);
                                            }
                                        }
                                        else if (conceptTemp.BasicType === 3)//Decimal 
                                        {
                                            ModalConditionRule.ShowPanel(4);
                                            if (conceptTemp.MaxValue) {
                                                $("#txtExpressionDecimal").attr("max", conceptTemp.MaxValue);
                                            }
                                            if (conceptTemp.MinValue) {
                                                $("#txtExpressionDecimal").attr("min", conceptTemp.MinValue);
                                            }
                                        }
                                        break;

                                    case 3://DateEditor
                                        if (conceptTemp.BasicType === 4) {
                                            ModalConditionRule.ShowPanel(5);
                                            if (conceptTemp.MaxDate) {
                                                $("#txtExpressionDate").UifDatepicker("setMaxDate", FormatDate(conceptTemp.MaxDate));
                                            }
                                            if (conceptTemp.MinDate) {
                                                $("#txtExpressionDate").UifDatepicker("setMinDate", FormatDate(conceptTemp.MinDate));
                                            }
                                        }
                                        break;

                                    case 4://ListBox
                                        if (conceptTemp.ConceptType === 3) {
                                            ModalConditionRule.ShowPanel(6);
                                            $("#dllExpressionList").UifSelect({
                                                sourceData: conceptTemp.ListEntity.ListEntityValues,
                                                id: "ListValueCode",
                                                name: "ListValue",
                                                native: false,
                                                filter: true
                                            });
                                        }
                                        else if (conceptTemp.ConceptType === 2) {
                                            ModalConditionRule.ShowPanel(6);

                                            conceptTemp.RangeEntity.RangeEntityValues.forEach((range, index) => {
                                                conceptTemp.RangeEntity.RangeEntityValues[index].Description = range.FromValue + " - " + range.ToValue;
                                            });
                                            $("#dllExpressionList").UifSelect({
                                                sourceData: conceptTemp.RangeEntity.RangeEntityValues,
                                                id: "RangeValueCode",
                                                name: "Description",
                                                native: false,
                                                filter: true
                                            });
                                        }

                                        break;

                                    case 5://SearchCombo => Pasa de serarchcombo a tablepanelExpressionSearch
                                        ModalConditionRule.ShowPanel(7);
                                        $('#txtExpressionSearch').UifInputSearch();
                                        $('#txtExpressionSearch').off().on("search", function (event, description) {
                                            var resultExpressionSearch = [];
                                            $.each(conceptTemp.ReferenceValues, function (key, value) {
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
                                                $("#btnSaveConditionRule").attr("disabled", "disabled");
                                            }
                                            else if (resultExpressionSearch.length == 1) {
                                                objectRules = [];
                                                $("#txtExpressionSearch").val(resultExpressionSearch[0].Description);
                                                $("#txtExpressionSearch").data("object", resultExpressionSearch[0]);
                                                objectRules = resultExpressionSearch[0];
                                                $("#btnSaveConditionRule").removeAttr("disabled");
                                            }
                                            else {
                                                $('#tableConceptResult').UifDataTable('clear');
                                                $("#tableConceptResult").UifDataTable({ sourceData: resultExpressionSearch })
                                                $('#modalConceptTableResult').UifModal('showLocal', conceptTemp.Description);
                                            }
                                        });
                                        break;
                                }
                            } else {
                                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                            }
                        });
                    break;

                case "2": //Concept Value
                    ModalConditionRule.ShowPanel(6);
                    $("#dllExpressionList").UifSelect({
                        sourceData: objConcepts,
                        id: "ConceptIdFacadeId",
                        name: "DescriptionFacade",
                        native: false,
                        filter: true
                    });
                    break;

                case "3": //Expression Value
                    ModalConditionRule.ShowPanel(2);
                    break;
            }
        } else {
            ModalConditionRule.ShowPanel(1);
        }
    }

    ChangeConditionComparator(e, item) {

        $("#ddlConditionComparatorType").UifSelect("setSelected", "");

        ModalConditionRule.ShowPanel(null);

        if (item.Id) {
            let comparator = $("#ddlConditionIs").UifSelect("getSelectedSource");

            if (comparator.Operator === 7 || comparator.Operator === 8)//null and not null
            {
                $("#btnSaveConditionRule").removeAttr("disabled");
            } else {
                ModalConditionRule.ShowPanel(1);
            }

        }
    }

    ChangeConceptCondition(e, item) {
        ModalConditionRule.ShowPanel(null);
        let dependences = true;

        if (item.Id) {

            let concept = $("#ddlConceptCondition").UifSelect("getSelectedSource");

            if (concept.ConceptDependences.length > 0) {
                let conditions = jQuery.extend(true, [], $("#lsvCondition").UifListView("getData"));
                let itemEdit = parseInt($("#hdnIndexConditionRule").val());
                if (itemEdit >= 0) {
                    conditions.splice(itemEdit, 1);
                }
                concept.ConceptDependences.forEach((c) => {
                    let conceptTmp = conditions.find((x) => { return x.Concept.ConceptId === c.DependsConcept.ConceptId && x.Concept.Entity.EntityId === c.DependsConcept.Entity.EntityId });

                    if (conceptTmp === undefined) {
                        dependences = false;
                        $.UifNotify("show", { 'type': "info", 'message': `Se debe asignar el concepto: "${c.DependsConcept.Description}"`, 'autoclose': true });
                    }
                    else if (conceptTmp.Comparator.Operator !== 1 || conceptTmp.ComparatorType !== 1) {
                        dependences = false;
                        $.UifNotify("show", { 'type': "info", 'message': `El concepto "${c.DependsConcept.Description}" debe ser igual a un valor constante`, 'autoclose': true });
                    }
                });
            }

            if (dependences === true) {
                RequestConcepts.GetComparatorConcept(concept.ConceptId, concept.Entity.EntityId)
                    .done((data) => {
                        if (data.success) {
                            $("#ddlConditionIs").UifSelect({ sourceData: data.result });
                        }
                        else {
                            $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                        }
                    });
            }
        } else {
            $("#ddlConditionIs").UifSelect();

        }
    }

    /**
     * Guarda o edita un elemento a la lisview
     */
    SaveConditionRule() {
        if ($("#formModalConditionRule").valid()) {
            let objCondition = {};

            let index = $("#hdnIndexConditionRule").val();

            if ($("#ddlConditionComparatorType").val() === "3") {
                if (!ModalConditionRule.ValidateExpression($("#txtExpressionText").val())) {
                    return;
                }
            }

            objCondition.Concept = $("#ddlConceptCondition").UifSelect("getSelectedSource");
            let conditionIs = $("#ddlConditionIs").UifSelect("getSelectedSource");

            if (conditionIs.Operator === 7) //NULL
            {
                objCondition.ComparatorType = 1;
                objCondition.Expression = null;
                objCondition.Comparator = {
                    Description: Resources.Language.EqualsTo,
                    Operator: 1,
                    Symbol: "="
                }
            }
            else if (conditionIs.Operator === 8) //NOT NULL
            {
                objCondition.ComparatorType = 1;
                objCondition.Expression = null;
                objCondition.Comparator = {
                    Description: Resources.Language.DistinctTo,
                    Operator: 6,
                    Symbol: "<>"
                }
            } else {
                objCondition.Comparator = conditionIs;
                objCondition.ComparatorType = $("#ddlConditionComparatorType").UifSelect("getSelectedSource").Id;

                if ($("#panelExpressionText").is(":visible")) {
                    objCondition.Expression = $("#txtExpressionText").val();
                }
                else if ($("#panelExpressionNumber").is(":visible")) {
                    objCondition.Expression = $("#txtExpressionNumber").val();
                }
                else if ($("#panelExpressionDecimal").is(":visible")) {
                    objCondition.Expression = $("#txtExpressionDecimal").val();
                }
                else if ($("#panelExpressionDate").is(":visible")) {
                    objCondition.Expression = $("#txtExpressionDate").val();
                }
                else if ($("#panelExpressionList").is(":visible")) {
                    objCondition.Expression = $("#dllExpressionList").UifSelect("getSelectedSource");
                }
                else if ($("#panelExpressionSearch").is(":visible")) {
                    objCondition.Expression = $("#txtExpressionSearch").data("object");
                }
            }

            let selected = $("#lsvRules").UifListView("getSelected")[0];
            if (!index) //Crear condicion nueva
            {
                $("#lsvCondition").UifListView("addItem", ModalConditionRule.FillCondition(objCondition));
                $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                    if (rule.RuleId === selected.RuleId) {
                        rule.Conditions.push(objCondition);
                        $("#lsvRules").UifListView("editItem", indexRule, rule );
                    }
                });
            } else //editar condicion
            {
                $("#lsvCondition").UifListView("editItem", index, ModalConditionRule.FillCondition(objCondition));
                $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                    if (rule.RuleId === selected.RuleId) {
                        rule.Conditions[index] = objCondition;
                        $("#lsvRules").UifListView("editItem", indexRule, rule);
                    }
                });
            }
            RulesSet.UpdateRule();
            ModalConditionRule.CloseModal();
        }
    }

    /**
    * @summary 
    * Evento al agregar una nueva condiciones de reglas al listView
    */
    AddConditionRule() {
        let rules = $("#lsvRules").UifListView("getSelected");

        if (rules.length !== 0) {
            $("#hdnIndexConditionRule").val("");
            ModalConditionRule.ClearForm();
            ModalConditionRule.ShowModal();
        }
        else {
            $.UifNotify("show", { 'type': "warning", 'message': "Seleccione una regla", 'autoclose': true });
        }
    }


    EditConditionRule(event, data, index) {
        let rules = $("#lsvRules").UifListView("getSelected");

        if (rules.length !== 0) {
            let conditions = $("#lsvCondition").UifListView("getData");
            let hasDependency = false;
            let dependence = [];

            conditions.forEach((c) => {
                c.Concept.ConceptDependences.forEach((cd) => {
                    if (cd.DependsConcept.ConceptId === data.Concept.ConceptId && cd.DependsConcept.Entity.EntityId === data.Concept.Entity.EntityId) {
                        hasDependency = true;
                        dependence.push(c);
                    }
                });
            });

            if (hasDependency === true) {
                let message = `Para editar el concepto "${data.Concept.Description}" y debe eliminar los conceptos dependientes?`;
                dependence.forEach((x) => {
                    message += `\n* ${x.Concept.Description}`;
                });

                $.UifDialog("confirm", {
                    "title": "Eliminar dependencias",
                    "message": message
                }, function (result) {
                    if (result === true) {
                        let newConditions = [];
                        let selected = $("#lsvRules").UifListView("getSelected")[0];
                        $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                            if (rule.RuleId === selected.RuleId) {
                                rule.Conditions.forEach((condition) => {
                                    if (dependence.find((x) => { return x.Concept.ConceptId === condition.Concept.ConceptId && x.Concept.Entity.EntityId === condition.Concept.Entity.EntityId }) === undefined) {
                                        newConditions.push(condition);
                                    }
                                });
                                rule.Conditions = newConditions;
                                $("#lsvRules").UifListView("editItem", rule, indexRule);
                            }
                        });

                        $("#lsvCondition").UifListView("clear");
                        RulesSet.SetListCondition(newConditions);

                        ModalConditionRule.ClearForm();
                        ModalConditionRule.ShowModal();
                        $("#hdnIndexConditionRule").val(index);
                        ModalConditionRule.SetForm(data);
                    }
                });
            }
            else {
                ModalConditionRule.ClearForm();
                ModalConditionRule.ShowModal();
                $("#hdnIndexConditionRule").val(index);
                ModalConditionRule.SetForm(data);
            }
        } else {
            $.UifNotify("show", { 'type': "warning", 'message': "Seleccione una regla", 'autoclose': true });
        }
    }

    static DraggingEnded(event, item) {
        let rules = $("#lsvRules").UifListView("getSelected");
        if (rules.length !== 0) {
            const rule = rules[0];
            const conditions = $("#lsvCondition").UifListView("getData");
            rules = $("#lsvRules").UifListView("getData");

            for (let i = 0; i < rules.length; i++) {
                if (rules[i].RuleId === rule.RuleId) {
                    rule.Conditions = conditions;
                    if (rule.StatusType != ParametrizationStatus.Create) {
                        rule.StatusType = ParametrizationStatus.Update;
                    }
                    $("#lsvRules").UifListView("editItem", i, rule);
                }
            }

        }
    }

    static DeleteCondition(deferred, data) {

        let conditions = $("#lsvCondition").UifListView("getData");
        let hasDependency = false;
        let dependence = [];

        conditions.forEach((c) => {
            c.Concept.ConceptDependences.forEach((cd) => {
                if (cd.DependsConcept.ConceptId === data.Concept.ConceptId && cd.DependsConcept.Entity.EntityId === data.Concept.Entity.EntityId) {
                    hasDependency = true;
                    dependence.push(c);
                }
            });
        });

        if (hasDependency === true) {
            let message = `Desea eliminar el concepto "${data.Concept.Description}" y sus conceptos dependientes?`;
            dependence.forEach((x) => {
                message += `\n* ${x.Concept.Description}`;
            });

            $.UifDialog("confirm", {
                "title": "Eliminar dependencias",
                "message": message
            }, function (result) {
                if (result === true) {
                    let newConditions = [];
                    let selected = $("#lsvRules").UifListView("getSelected")[0];
                    $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                        if (rule.RuleId === selected.RuleId) {
                            dependence.push(data);

                            rule.Conditions.forEach((condition) => {
                                if (dependence.find((x) => { return x.Concept.ConceptId === condition.Concept.ConceptId && x.Concept.Entity.EntityId === condition.Concept.Entity.EntityId }) === undefined) {
                                    newConditions.push(condition);
                                }
                            });
                            rule.Conditions = newConditions;
                            if (rule.StatusType != ParametrizationStatus.Create) {
                                rule.StatusType = ParametrizationStatus.Update;
                            }
                            $("#lsvRules").UifListView("editItem", rule, indexRule);
                        }
                    });
                    deferred.resolve();
                    $("#lsvCondition").UifListView("clear");
                    RulesSet.SetListCondition(newConditions);
                }
            });
        }
        else {
            let selected = $("#lsvRules").UifListView("getSelected")[0];
            $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                if (rule.RuleId === selected.RuleId) {
                    rule.Conditions.forEach((condition, indexCondition) => {
                        if (condition === data) {
                            rule.Conditions.splice(indexCondition, 1);
                            return;
                        }
                    });
                    if (rule.StatusType != ParametrizationStatus.Create) {
                        rule.StatusType = ParametrizationStatus.Update;
                    }
                    $("#lsvRules").UifListView("editItem", indexRule, rule );
                }
            });
            deferred.resolve();
        }
    }

    static SetForm(condition) {
        $("#ddlConceptCondition").UifSelect("setSelected", condition.Concept.ConceptId + "-" + condition.Concept.Entity.EntityId);
        $("#ddlConceptCondition").trigger("itemSelected", [{ Id: condition.Concept.ConceptId + "-" + condition.Concept.Entity.EntityId }]);

        if (condition.Comparator.Operator === 1 && condition.Expression === null) {
            $("#ddlConditionIs").UifSelect("setSelected", 7);
        }
        else if (condition.Comparator.Operator === 6 && condition.Expression === null) {
            $("#ddlConditionIs").UifSelect("setSelected", 8);
        } else {
            $("#ddlConditionIs").UifSelect("setSelected", condition.Comparator.Operator);
            $("#ddlConditionIs").trigger("itemSelected", [{ Id: condition.Comparator.Operator.toString() }]);

            $("#ddlConditionComparatorType").UifSelect("setSelected", condition.ComparatorType);
            $("#ddlConditionComparatorType").trigger("itemSelected", [{ Id: condition.ComparatorType.toString() }]);


            if (condition.Expression.ListValueCode !== undefined) {
                $("#dllExpressionList").UifSelect("setSelected", condition.Expression.ListValueCode);
            }
            else if (condition.Expression.RangeValueCode) {
                $("#dllExpressionList").UifSelect("setSelected", condition.Expression.RangeValueCode);
            }
            else if (condition.Expression.ConceptId) {
                $("#dllExpressionList").UifSelect("setSelected", condition.Expression.ConceptId + "-" + condition.Expression.Entity.EntityId);
            }
            else if (condition.Expression.Id) {
                //$("#dllExpressionList").UifSelect("setSelected", condition.Expression.Id);
                $("#txtExpressionSearch").val(condition.Expression.Description);
                $("#txtExpressionSearch").data("object", condition.Expression);
            }
            else {
                $("#txtExpressionText").val(condition.Expression);
                $("#txtExpressionNumber").val(condition.Expression);
                $("#txtExpressionDecimal").val(condition.Expression);
                $("#txtExpressionDate").val(condition.Expression);
                
            }
        }

        $("#btnSaveConditionRule").removeAttr("disabled");
    }

    static ValidateExpression(expression) {
        let result = false;
        RequestRules.ValidateExpression(expression)
            .done((data) => {
                if (data.success) {
                    $("#txtExpressionText").val(data.result);
                    result = true;
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    result = false;
                }
            });
        return result;
    }

    static ShowPanel(panel) {
        $("#btnSaveConditionRule").attr("disabled", "disabled");
        $("#panelConditionComparatorType").hide();
        $("#panelConditionExpression").hide();
        $("#panelExpressionText").hide();
        $("#panelExpressionNumber").hide();
        $("#panelExpressionDecimal").hide();
        $("#panelExpressionDate").hide();
        $("#panelExpressionList").hide();
        $("#panelExpressionSearch").hide();
        $("#txtExpressionText").removeAttr("maxlength");
        $("#txtExpressionNumber").removeAttr("max");
        $("#txtExpressionNumber").removeAttr("min");
        $("#txtExpressionDecimal").removeAttr("max");
        $("#txtExpressionDecimal").removeAttr("min");
        $("#txtExpressionDate").UifDatepicker("setMaxDate", null);
        $("#txtExpressionDate").UifDatepicker("setMinDate", null);

        $("#txtExpressionText").val("");
        $("#txtExpressionNumber").val("");
        $("#txtExpressionDecimal").val("");
        $("#txtExpressionDate").val("");
        $("#txtExpressionSearch").val("");
        $("#txtExpressionSearch").data("object", null);


        switch (panel) {
            case 1:
                $("#panelConditionComparatorType").show();
                break;
            case 2:
                $("#panelConditionComparatorType").show();
                $("#panelConditionExpression").show();
                $("#panelExpressionText").show();
                break;
            case 3:
                $("#panelConditionComparatorType").show();
                $("#panelConditionExpression").show();
                $("#panelExpressionNumber").show();
                break;
            case 4:
                $("#panelConditionComparatorType").show();
                $("#panelConditionExpression").show();
                $("#panelExpressionDecimal").show();
                break;
            case 5:
                $("#panelConditionComparatorType").show();
                $("#panelConditionExpression").show();
                $("#panelExpressionDate").show();
                break;
            case 6:
                $("#panelConditionComparatorType").show();
                $("#panelConditionExpression").show();
                $("#panelExpressionList").show();
                break;
            case 7:
                $("#panelConditionComparatorType").show();
                $("#panelConditionExpression").show();
                $("#panelExpressionSearch").show();
                break;
        }
        $("#formModalConditionRule").valid();
    }

    /**
    * @summary 
    * limpia el formulario de la ventana modal de condiciones de reglas
    */
    static ClearForm() {
        ModalConditionRule.ShowPanel(null);
        $("#hdnIndexConditionRule").val("");
        $("#ddlConceptCondition").UifSelect("setSelected", "");
        $("#ddlConditionIs").UifSelect();
        $("#ddlConditionComparatorType").UifSelect("setSelected", "");
    }

    /**
    * @summary 
    * abre la ventana modal de condiciones de reglas
    */
    static ShowModal() {
        $("#modalConditionRule").UifModal("showLocal", "Condición");
    }

    /**
     * @summary 
     * Cierra la ventana modal de condiciones de reglas
    */
    static CloseModal() {
        $("#modalConditionRule").UifModal("hide");
    }

    static FillCondition(condition) {
        
        let str = [];
        str.push(`${Resources.Language.If} ${condition.Concept.Description.toUpperCase()}`);
        str.push(`${Resources.Language.Is} ${condition.Comparator.Description} `);

        if (condition.Expression === null || condition.Expression === undefined) {
            str[1] += Resources.Language.Null;
        } else {
            if (!condition.HasError) {
                switch (condition.ComparatorType) {
                case 1: //ConstantValue = 1,
                    switch (condition.Concept.ConceptType) {
                    case 2: //Rango
                        str[1] += `${Resources.Language.ValueBetween}  `;
                        str.push(`${condition.Expression.FromValue} ${Resources.Language.And} ${condition.Expression
                            .ToValue}`);
                        break;
                    case 3: //Lista
                        str[1] += `${Resources.Language.TheValue} `;
                        str.push(condition.Expression.ListValue);
                        break;
                    case 4: //Referencia
                        str[1] += `${Resources.Language.TheValue} `;
                        str.push(condition.Expression.Description.toUpperCase() + "(" + condition.Expression.Id + ")");
                        break;
                    default:
                        str[1] += `${Resources.Language.TheValue} `;
                        str.push(condition.Expression);
                        break;
                    }
                    break;
                case 2: //ConceptValue = 2,
                    str[1] += `${Resources.Language.ConceptValue} `;
                    str.push(condition.Expression.Description.toUpperCase());
                    break;
                case 3: //ExpressionValue = 3,
                    str[1] += `${Resources.Language.ExpressionValue} `;
                    str.push(condition.Expression);
                    break;
                case 4: //TemporalyValue = 4
                    str[1] += `${Resources.Language.TemporalValue} `;
                    str.push(condition.Expression);
                    break;
                }
            }
        }

        condition.ExpressionStr = str;
        return condition;
    }
}