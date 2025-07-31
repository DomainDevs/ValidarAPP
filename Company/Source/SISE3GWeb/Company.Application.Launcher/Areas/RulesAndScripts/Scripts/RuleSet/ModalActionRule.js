var objectRules = [];
class ModalActionRule extends Uif2.Page {

    getInitialState() {
        if (gblPolicies.GroupPolicies) {
            let levels = [];
            for (var i = 1; i <= gblPolicies.Position; i++) {
                levels.push(i);
            }
            RequestRules.GetRulesByFilter(gblPolicies.GroupPolicies.Package.PackageId, levels, true, true, "", false)
                .done((data) => {
                    if (data.success) {
                        packageIdRule = data.result;
                        $('#tableResultsRulesScripts').UifDataTable('clear');
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });

            RequestRules.GetRuleFunctionsByIdPackageLevels(gblPolicies.GroupPolicies.Package.PackageId, levels)
                .done((data) => {
                    if (data.success) {
                        $("#ddlAssignInvokeFunction").UifSelect({ sourceData: data.result });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }

        RequestRules.GetActionType()
            .done((data) => {
                if (data.success) {
                    $("#ddlActionType").UifSelect({ sourceData: data.result });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });

        RequestRules.GetInvokeType()
            .done((data) => {
                if (data.success) {
                    $("#ddlInvokeType").UifSelect({ sourceData: data.result });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });

        RequestRules.GetActionComparatorType()
            .done((data) => {
                if (data.success) {
                    $("#ddlActionComparatorType").UifSelect({ sourceData: data.result });
                    $("#ddlTempValueExpressionType").UifSelect({ sourceData: data.result });

                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });


        RequestRules.GetArithmeticOperatorType()
            .done((data) => {
                if (data.success) {
                    $("#ddlTempValueOperatorType").UifSelect({ sourceData: data.result });
                    $("#ddlConceptOperatorType").UifSelect({ sourceData: data.result });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });

        ModalActionRule.ShowPanel(null);
    }

    bindEvents() {
        $("#lsvAction").on("rowAdd", this.AddActionRule);
        $("#lsvAction").on("rowEdit", this.EditActionRule);
        $("#ddlActionType").on("itemSelected", this.ChangeActionType);
        $("#ddlInvokeType").on("itemSelected", this.ChangeInvokeType);
        $("#ddlActionConcept").on("itemSelected", this.ChangeActionConcept);
        $("#ddlActionComparatorType").on("itemSelected", this.ChangeActionComparatorType);
        $("#txtActionTemp").on("blur", this.ChangeActionTemp);
        $("#ddlTempValueOperatorType").on("itemSelected", this.ChangeValueOperatorType);
        $("#ddlConceptOperatorType").on("itemSelected", this.ChangeConceptOperatorType);
        $("#ddlTempValueExpressionType").on("itemSelected", this.ChangeTempValueExpressionType);
        $("#txtConceptExpressionText").on("keyup", this.EnabledSaveButton.bind(this, 1));
        $("#txtConceptExpressionNumber").on("keyup", this.EnabledSaveButton.bind(this, 2));
        $("#txtConceptExpressionDecimal").on("keyup", this.EnabledSaveButton.bind(this, 3));
        $("#txtConceptExpressionDate").on("keyup", this.EnabledSaveButton.bind(this, 4));
        $("#txtConceptExpressionDate").on("datepicker.change", this.EnabledSaveButton.bind(this, 4));
        $("#dllConceptExpressionList").on("itemSelected", this.EnabledSaveButton.bind(this, 5));
        $("#txtAssignInvoke").on("keyup", this.EnabledSaveButton.bind(this, 6));
        $("#ddlAssignInvokeRuleSet").on("itemSelected", this.EnabledSaveButton.bind(this, 7));
        $("#ddlAssignInvokeFunction").on("itemSelected", this.EnabledSaveButton.bind(this, 8));
        $("#txtTempExpressionValue").on("keyup", this.EnabledSaveButton.bind(this, 9));
        $("#txtTempExpressionValueNum").on("keyup", this.EnabledSaveButton.bind(this, 10));
        $("#ddlTempExpressionValue").on("itemSelected", this.EnabledSaveButton.bind(this, 11));
        $("#btnSaveActionRule").on("click", this.SaveActionRule);
        $("#inputModalRiskInitialRulesPackage").on('click', this.CurrentSearchDisable);
        $("#inputModalRiskInitialRulesPackage").on('buttonClick', this.CurrentSearch);
        $('#tableResultsRulesScript tbody').on('click', "tr", this.TableResultRulesScript);
        $('#tableConceptResult tbody').on('click', "tr", ModalActionRule.TableResult);
    }

    TableResultRulesScript() {
        objectRules = [];
        $("#inputModalRiskInitialRulesPackage").val($('#tableResultsRulesScript').DataTable().row(this).data().Description);
        objectRules = $('#tableResultsRulesScript').DataTable().row(this).data();
        $("#btnSaveActionRule").removeAttr("disabled");
        $('#modalRulesAndScriptsResults').UifModal("hide");
    }

    static TableResult() {
        objectRules = [];
        $("#txtConceptExpressionSearch").val($('#tableConceptResult').DataTable().row(this).data().Description);
        $("#txtConceptExpressionSearch").data("object", $('#tableConceptResult').DataTable().row(this).data());
        objectRules = $('#tableConceptResult').DataTable().row(this).data();
        $("#btnSaveActionRule").removeAttr("disabled");
        $('#modalConceptTableResult').UifModal("hide");
    }

    CurrentSearchDisable(e) {
        $("#btnSaveActionRule").attr("disabled", "disabled");

    }
    CurrentSearch() {
        var result = [];
        $.each(packageIdRule, function (key, value) {
            if ((value.Description.toLowerCase().sistranReplaceAccentMark().includes($("#inputModalRiskInitialRulesPackage").val().toLowerCase().sistranReplaceAccentMark())) ||
                (value.RuleSetId.toString().toLowerCase().sistranReplaceAccentMark().includes($("#inputModalRiskInitialRulesPackage").val().toLowerCase().sistranReplaceAccentMark()))) {
                result.push(value);
            }
        });
        if (result.length == 0) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.TechnicalPlanNotFound, 'autoclose': true
            })
        } else {
            $('#tableResultsRulesScript').UifDataTable('clear');
            $('#tableResultsRulesScript').UifDataTable('addRow', result);
            $('#modalRulesAndScriptsResults').UifModal('showLocal', Resources.Language.LabelInitialRulesPackage);
        }
    }

    GetGeneralInitialFilterByDescription() {

    }

    EnabledSaveButton(item) {
        $("#btnSaveActionRule").attr("disabled", "disabled");
        switch (item) {
            case 1:
                if ($("#txtConceptExpressionText").val()) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;

            case 2:
                if ($("#txtConceptExpressionNumber").val()) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;

            case 3:
                if ($("#txtConceptExpressionDecimal").val()) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;

            case 4:
                if ($("#txtConceptExpressionDate").val()) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;

            case 5:
                if ($("#dllConceptExpressionList").UifSelect("getSelected")) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;

            case 6:
                if ($("#txtAssignInvoke").val()) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;

            case 7:
                if ($("#ddlAssignInvokeRuleSet").UifSelect("getSelected")) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;
            case 8:
                if ($("#ddlAssignInvokeFunction").UifSelect("getSelected")) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;

            case 9:
                if ($("#txtTempExpressionValue").val()) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;
            case 10:
                if ($("#txtTempExpressionValueNum").val()) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;
            case 11:
                if ($("#ddlTempExpressionValue").UifSelect("getSelected")) {
                    $("#btnSaveActionRule").removeAttr("disabled");
                }
                break;
        }
    }

    ChangeTempValueExpressionType(e, item) {
        if (item.Id) {
            switch (item.Id) {
                case "1":
                    ModalActionRule.ShowPanel(17);
                    break;
                case "2":
                    ModalActionRule.ShowPanel(18);
                    break;
                case "3":
                case "4":
                    ModalActionRule.ShowPanel(16);
                    break;
            }
        } else {
            ModalActionRule.ShowPanel(15);
        }
    }

    ChangeValueOperatorType(e, item) {
        if (item.Id) {
            ModalActionRule.ShowPanel(15);
            $("#ddlTempValueExpressionType").UifSelect("setSelected", "");
        } else {
            ModalActionRule.ShowPanel(14);
        }
    }

    ChangeActionTemp() {
        let tempValue = $("#txtActionTemp").val();
        if (tempValue && !$("#panelAssignTempExpression").is(":visible")) {
            ModalActionRule.ShowPanel(14);
            $("#ddlTempValueOperatorType").UifSelect("setSelected", "");
        }
        else if (!tempValue) {
            ModalActionRule.ShowPanel(3);
            $("#ddlTempValueOperatorType").UifSelect("setSelected", "");
        }
    }

    ChangeActionComparatorType(e, item) {
        if (item.Id) {
            let conceptTmp = $("#ddlActionConcept").UifSelect("getSelectedSource");
            switch (item.Id) {
                case "1": //Constant Value
                    var dependence = [];
                    if (conceptTmp.ConceptDependences.length > 0) {
                        let actions = $("#lsvAction").UifListView("getData");
                        conceptTmp.ConceptDependences.forEach((c) => {
                            let conceptTmp2 = actions.find((x) => {
                                if (x.AssignType === 1) {
                                    return x.Concept.ConceptId === c.DependsConcept.ConceptId &&
                                        x.Concept.Entity.EntityId === c.DependsConcept.Entity.EntityId
                                }
                            });

                            if (conceptTmp2 !== undefined) {
                                dependence.push(conceptTmp2.Expression.Id);
                            }
                        });
                    }

                    RequestConcepts.GetSpecificConceptWithVales(conceptTmp.ConceptId, conceptTmp.Entity.EntityId, dependence, conceptTmp.ConceptType)
                        .done((data) => {
                            if (data.success) {
                                let conceptTemp = data.result;

                                switch (conceptTmp.ConceptControlType) {
                                    case 1://TextBox
                                        if (conceptTemp.BasicType === 2) {
                                            ModalActionRule.ShowPanel(9);
                                            if (conceptTemp.Length) {
                                                $("#txtConceptExpressionText").attr("maxlength", conceptTemp.Length);
                                            }
                                        }
                                        break;

                                    case 2://NumberEditor
                                        if (conceptTemp.BasicType === 1)//Numeric 
                                        {
                                            ModalActionRule.ShowPanel(10);
                                            if (conceptTemp.MaxValue) {
                                                $("#txtConceptExpressionNumber").attr("max", conceptTemp.MaxValue);
                                            }
                                            if (conceptTemp.MinValue) {
                                                $("#txtConceptExpressionNumber").attr("min", conceptTemp.MinValue);
                                            }
                                        }
                                        else if (conceptTemp.BasicType === 3)//Decimal 
                                        {
                                            ModalActionRule.ShowPanel(11);
                                            if (conceptTemp.MaxValue) {
                                                $("#txtConceptExpressionDecimal").attr("max", conceptTemp.MaxValue);
                                            }
                                            if (conceptTemp.MinValue) {
                                                $("#txtConceptExpressionDecimal").attr("min", conceptTemp.MinValue);
                                            }
                                        }
                                        break;

                                    case 3://DateEditor
                                        if (conceptTemp.BasicType === 4) {
                                            ModalActionRule.ShowPanel(12);
                                            if (conceptTemp.MaxDate) {
                                                $("#txtConceptExpressionDate").UifDatepicker("setMaxDate", FormatDate(conceptTemp.MaxDate));
                                            }
                                            if (conceptTemp.MinDate) {
                                                $("#txtConceptExpressionDate").UifDatepicker("setMinDate", FormatDate(conceptTemp.MinDate));
                                            }
                                        }
                                        break;

                                    case 4://ListBox
                                        if (conceptTemp.ConceptType === 3) {
                                            ModalActionRule.ShowPanel(13);
                                            $("#dllConceptExpressionList").UifSelect({
                                                sourceData: conceptTemp.ListEntity.ListEntityValues,
                                                id: "ListValueCode",
                                                name: "ListValue",
                                                native: false,
                                                filter: true
                                            });
                                        }
                                        else if (conceptTemp.ConceptType === 2) {
                                            ModalActionRule.ShowPanel(13);

                                            conceptTemp.RangeEntity.RangeEntityValues.forEach((range, index) => {
                                                conceptTemp.RangeEntity.RangeEntityValues[index].Description = range.FromValue + " - " + range.ToValue;
                                            });
                                            $("#dllConceptExpressionList").UifSelect({
                                                sourceData: conceptTemp.RangeEntity.RangeEntityValues,
                                                id: "RangeValueCode",
                                                name: "Description",
                                                native: false,
                                                filter: true
                                            });
                                        }

                                        break;

                                    case 5://SearchCombo => Pasa de serarchcombo a tablepanelExpressionSearch
                                        ModalActionRule.ShowPanel(19);
                                        $('#txtConceptExpressionSearch').UifInputSearch();
                                        $('#txtConceptExpressionSearch').off().on("search", function (event, description) {
                                            var resultExpressionSearch = [];
                                            $.each(conceptTemp.ReferenceValues, function (key, value) {
                                                if (value.Description.toLowerCase().sistranReplaceAccentMark().
                                                    includes(description.toLowerCase().sistranReplaceAccentMark()) ||
                                                    value.Id.toLowerCase().sistranReplaceAccentMark().
                                                        includes(description.toLowerCase().sistranReplaceAccentMark())) {
                                                    resultExpressionSearch.push(value);
                                                }
                                            });

                                            if (resultExpressionSearch.length == 0) {
                                                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true })
                                                $("#btnSaveActionRule").attr("disabled", "disabled");
                                            }
                                            else if (resultExpressionSearch.length == 1) {
                                                objectRules = [];
                                                $("#txtConceptExpressionSearch").val(resultExpressionSearch[0].Description);
                                                $("#txtConceptExpressionSearch").data("object", resultExpressionSearch[0]);
                                                objectRules = resultExpressionSearch[0];
                                                $("#btnSaveActionRule").removeAttr("disabled");
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
                    ModalActionRule.ShowPanel(13);
                    $("#dllConceptExpressionList").UifSelect({
                        sourceData: objConcepts,
                        id: "ConceptIdFacadeId",
                        name: "DescriptionFacade",
                        native: false,
                        filter: true
                    });
                    break;

                case "3"://Expression Value
                case "4"://temporal Value
                    ModalActionRule.ShowPanel(9);
                    break;
            }
        } else {
            ModalActionRule.ShowPanel(8);
        }
    }

    ChangeConceptOperatorType(e, item) {
        if (item.Id) {
            ModalActionRule.ShowPanel(8);
            $("#ddlActionComparatorType").UifSelect("setSelected", "");
        } else {
            ModalActionRule.ShowPanel(7);
        }
    }

    ChangeActionConcept(e, item) {
        let dependences = true;
        if (item.Id) {
            let concept = $("#ddlActionConcept").UifSelect("getSelectedSource");

            if (concept.ConceptDependences.length > 0) {
                let actions = jQuery.extend(true, [], $("#lsvAction").UifListView("getData"));
                let itemEdit = parseInt($("#hdnIndexActionRule").val());
                if (itemEdit >= 0) {
                    actions.splice(itemEdit, 1);
                }
                concept.ConceptDependences.forEach((c) => {
                    let conceptTmp = actions.find((x) => {
                        if (x.AssignType === 1) {
                            return x.Concept.ConceptId === c.DependsConcept.ConceptId && x.Concept.Entity.EntityId === c.DependsConcept.Entity.EntityId
                        }
                    });

                    if (conceptTmp === undefined) {
                        dependences = false;
                        $.UifNotify("show", { 'type': "info", 'message': `Se debe asignar el concepto: "${c.DependsConcept.Description}"`, 'autoclose': true });
                    }
                    else if (conceptTmp.ArithmeticOperator.ArithmeticOperatorType !== 5 || conceptTmp.AssignType !== 1 || conceptTmp.ComparatorType !== 1) {
                        dependences = false;
                        $.UifNotify("show", { 'type': "info", 'message': `El concepto "${c.DependsConcept.Description}" se le debe asignar a un valor constante`, 'autoclose': true });
                    }
                });
            }

            if (dependences === true) {
                if (concept.ConceptType === 2 || concept.ConceptType === 3 || concept.ConceptType === 4) {
                    ModalActionRule.ShowPanel(8);
                    $("#ddlConceptOperatorType").UifSelect("setSelected", 5);
                    $("#ddlActionComparatorType").UifSelect("setSelected", "");
                    $("#ddlConceptOperatorType").attr("disabled", "disabled");

                } else if (concept.ConceptType === 1 && concept.ConceptControlType === 3) {
                    ModalActionRule.ShowPanel(8);
                    $("#ddlConceptOperatorType").UifSelect("setSelected", 5);
                    $("#ddlActionComparatorType").UifSelect("setSelected", "");
                    $("#ddlConceptOperatorType").attr("disabled", "disabled");
                } else {
                    ModalActionRule.ShowPanel(7);
                    $("#ddlConceptOperatorType").UifSelect("setSelected", "");
                    $("#ddlConceptOperatorType").removeAttr("disabled");
                }
            }
        } else {
            ModalActionRule.ShowPanel(1);
        }
    }

    ChangeInvokeType(e, item) {

        if (item.Id) {
            switch (item.Id) {
                case "1"://MessageInvoke
                    ModalActionRule.ShowPanel(4);
                    break;

                case "2"://RuleInvoke
                    ModalActionRule.ShowPanel(5);
                    $("#inputModalRiskInitialRulesPackage").val("");

                    break;

                case "3"://FunctionInvoke
                    ModalActionRule.ShowPanel(6);
                    $("#ddlAssignInvokeFunction").UifSelect("setSelected", "");
                    break;
            }
        } else {
            ModalActionRule.ShowPanel(2);
        }
    }

    ChangeActionType(e, item) {
        if (item.Id) {
            switch (item.Id) {
                case "1"://ConceptAssign
                    ModalActionRule.ShowPanel(1);
                    $("#ddlActionConcept").UifSelect("setSelected", "");
                    break;

                case "2"://InvokeAssign
                    ModalActionRule.ShowPanel(2);
                    $("#ddlInvokeType").UifSelect("setSelected", "");
                    break;

                case "3"://TemporalAssign
                    ModalActionRule.ShowPanel(3);
                    $("#txtActionTemp").val("");
                    break;
            }
        } else {
            ModalActionRule.ShowPanel(null);
        }
    }

    SaveActionRule() {
        if ($("#formModalActionRule").valid()) {
            let objAction = { AssignType: $("#ddlActionType").UifSelect("getSelectedSource").Id };
            let index = $("#hdnIndexActionRule").val();

            switch (objAction.AssignType) {
                case 1: //Concept Assing
                    objAction.Concept = $("#ddlActionConcept").UifSelect("getSelectedSource");


                    objAction.ArithmeticOperator = {
                        Description: $("#ddlConceptOperatorType").UifSelect("getSelectedSource").Description,
                        ArithmeticOperatorType: $("#ddlConceptOperatorType").UifSelect("getSelectedSource").Id
                    }
                    objAction.ComparatorType = $("#ddlActionComparatorType").UifSelect("getSelectedSource").Id;

                    switch (objAction.ComparatorType) {
                        case 1://constantValue
                            if ($("#panelConceptExpressionText").is(":visible")) {
                                objAction.Expression = $("#txtConceptExpressionText").val();
                            }
                            else if ($("#panelConceptExpressionNumber").is(":visible")) {
                                objAction.Expression = $("#txtConceptExpressionNumber").val();
                            }
                            else if ($("#panelConceptExpressionDecimal").is(":visible")) {
                                objAction.Expression = $("#txtConceptExpressionDecimal").val();
                            }
                            else if ($("#panelConceptExpressionDate").is(":visible")) {
                                objAction.Expression = $("#txtConceptExpressionDate").val();
                            }
                            else if ($("#panelConceptExpressionList").is(":visible")) {
                                objAction.Expression = $("#dllConceptExpressionList").UifSelect("getSelectedSource");
                            }
                            else if ($("#panelConceptExpressionSearch").is(":visible")) {
                                objAction.Expression = $("#txtConceptExpressionSearch").data("object");
                            }
                            break;

                        case 2://ConceptValue
                            objAction.Expression = $("#dllConceptExpressionList").UifSelect("getSelectedSource");
                            break;

                        case 3://ExpressionValue
                            if (!ModalActionRule.ValidateExpression($("#txtConceptExpressionText").val(), $("#txtConceptExpressionText"))) {
                                return;
                            }
                            objAction.Expression = $("#txtConceptExpressionText").val();
                            break;

                        case 4://TemporalyValue
                            objAction.Expression = $("#txtConceptExpressionText").val();
                            break;
                    }
                    break;

                case 2://Invoke Assing
                    objAction.InvokeType = $("#ddlInvokeType").UifSelect("getSelectedSource").Id;

                    if ($("#panelAssignInvokeTxt").is(":visible")) {
                        objAction.Expression = $("#txtAssignInvoke").val();
                    }
                    else if ($("#panelAssignInvokeRuleSet").is(":visible")) {
                        objAction.Expression = objectRules;
                    }
                    else if ($("#panelAssignInvokeFunction").is(":visible")) {
                        objAction.Expression = $("#ddlAssignInvokeFunction").UifSelect("getSelectedSource");
                    }
                    break;

                case 3://TEmporal Assing

                    objAction.ValueTemp = $("#txtActionTemp").val();
                    objAction.ArithmeticOperator = {
                        Description: $("#ddlTempValueOperatorType").UifSelect("getSelectedSource").Description,
                        ArithmeticOperatorType: $("#ddlTempValueOperatorType").UifSelect("getSelectedSource").Id
                    }
                    objAction.ComparatorType = $("#ddlTempValueExpressionType").UifSelect("getSelectedSource").Id;


                    if ($("#panelTempExpressionValueText").is(":visible")) {
                        if (objAction.ComparatorType === 3) {
                            if (!ModalActionRule.ValidateExpression($("#txtTempExpressionValue").val(), $("#txtTempExpressionValue"))) {
                                return;
                            }
                        }
                        objAction.Expression = $("#txtTempExpressionValue").val();
                    }
                    else if ($("#panelTempExpressionValueDecimal").is(":visible")) {
                        objAction.Expression = $("#txtTempExpressionValueNum").val();
                    }
                    else if ($("#panelTempExpressionValueList").is(":visible")) {
                        objAction.Expression = $("#ddlTempExpressionValue").UifSelect("getSelectedSource");
                    }
                    break;
            }

            let selected = $("#lsvRules").UifListView("getSelected")[0];
            if (!index) //Crear condicion nueva
            {
                $("#lsvAction").UifListView("addItem", ModalActionRule.FillAction(objAction));
                $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                    if (rule.RuleId === selected.RuleId) {
                        rule.Actions.push(objAction);
                        $("#lsvRules").UifListView("editItem", indexRule, rule);
                    }
                });
            } else //editar condicion
            {
                $("#lsvAction").UifListView("editItem", index, ModalActionRule.FillAction(objAction));
                $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                    if (rule.RuleId === selected.RuleId) {
                        rule.Actions[index] = objAction;
                        $("#lsvRules").UifListView("editItem", indexRule, rule);
                    }
                });
            }
            RulesSet.UpdateRule();
            ModalActionRule.CloseModal();
        }
    }

    AddActionRule() {
        let rules = $("#lsvRules").UifListView("getSelected");

        if (rules.length !== 0) {
            $("#hdnIndexActionRule").val("");
            ModalActionRule.ClearForm();
            ModalActionRule.ShowModal();
        } else {
            $.UifNotify("show", { 'type': "warning", 'message': "Seleccione una regla", 'autoclose': true });
        }
    }

    EditActionRule(event, data, index) {
        let rules = $("#lsvRules").UifListView("getSelected");

        if (rules.length !== 0) {
            let actions = $("#lsvAction").UifListView("getData");
            let hasDependency = false;
            let dependence = [];

            if (data.AssignType === 1) {
                actions.forEach((c) => {
                    if (c.AssignType === 1) {
                        c.Concept.ConceptDependences.forEach((cd) => {
                            if (cd.DependsConcept.ConceptId === data.Concept.ConceptId &&
                                cd.DependsConcept.Entity.EntityId === data.Concept.Entity.EntityId) {
                                hasDependency = true;
                                dependence.push(c);
                            }
                        });
                    }
                });


                if (hasDependency === true) {
                    let message = `Para editar el concepto "${data.Concept.Description}" y debe eliminar los conceptos dependientes?`;
                    dependence.forEach((x) => {
                        message += `\n* ${x.Concept.Description}`;
                    });

                    $.UifDialog("confirm",
                        {
                            "title": "Eliminar dependencias",
                            "message": message
                        },
                        function (result) {
                            if (result === true) {
                                let newActions = [];
                                let selected = $("#lsvRules").UifListView("getSelected")[0];
                                $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                                    if (rule.RuleId === selected.RuleId) {
                                        rule.Actions.forEach((action) => {
                                            if (action.AssignType === 1) {
                                                if (dependence.find((x) => {
                                                    return x.Concept.ConceptId === action.Concept.ConceptId &&
                                                        x.Concept.Entity.EntityId === action.Concept.Entity.EntityId
                                                }) ===
                                                    undefined) {
                                                    newActions.push(action);
                                                }
                                            } else {
                                                newActions.push(action);
                                            }
                                        });
                                        rule.Actions = newActions;
                                        $("#lsvRules").UifListView("editItem", indexRule, rule);
                                    }
                                });

                                $("#lsvAction").UifListView("clear");
                                RulesSet.SetListAction(newActions);

                                ModalActionRule.ClearForm();
                                ModalActionRule.ShowModal();
                                $("#hdnIndexActionRule").val(index);
                                ModalActionRule.SetForm(data);
                            }
                        });
                } else {
                    ModalActionRule.ClearForm();
                    ModalActionRule.ShowModal();
                    $("#hdnIndexActionRule").val(index);
                    ModalActionRule.SetForm(data);
                }
            } else {
                ModalActionRule.ClearForm();
                ModalActionRule.ShowModal();
                $("#hdnIndexActionRule").val(index);
                ModalActionRule.SetForm(data);
            }
        }
    }

    static DraggingEnded(event, item) {
        let rules = $("#lsvRules").UifListView("getSelected");
        if (rules.length != 0) {
            let rule = rules[0];
            let actions = $("#lsvAction").UifListView("getData");
            rules = $("#lsvRules").UifListView("getData");

            for (let i = 0; i < rules.length; i++) {
                if (rules[i].RuleId === rule.RuleId) {
                    rule.Actions = actions;
                    if (rule.StatusType != ParametrizationStatus.Create) {
                        rule.StatusType = ParametrizationStatus.Update;
                    }
                    $("#lsvRules").UifListView("editItem", i, rule);
                }
            }
        }
    }

    static DeleteAction(deferred, data) {

        if (data.AssignType === 1) {
            let hasDependency = false;
            let dependence = [];
            let actions = $("#lsvAction").UifListView("getData");

            actions.forEach((c) => {
                if (c.AssignType === 1) {
                    c.Concept.ConceptDependences.forEach((cd) => {
                        if (cd.DependsConcept.ConceptId === data.Concept.ConceptId &&
                            cd.DependsConcept.Entity.EntityId === data.Concept.Entity.EntityId) {
                            hasDependency = true;
                            dependence.push(c);
                        }
                    });
                }
            });

            if (hasDependency === true) {
                let message = `Desea eliminar el concepto "${data.Concept.Description}" y sus conceptos dependientes?`;
                dependence.forEach((x) => {
                    message += `\n* ${x.Concept.Description}`;
                });

                $.UifDialog("confirm",
                    {
                        "title": "Eliminar dependencias",
                        "message": message
                    },
                    function (result) {
                        if (result === true) {
                            let newActions = [];
                            let selected = $("#lsvRules").UifListView("getSelected")[0];
                            $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                                if (rule.RuleId === selected.RuleId) {
                                    dependence.push(data);

                                    rule.Actions.forEach((action) => {
                                        if (action.AssignType === 1) {
                                            if (dependence.find((x) => {
                                                return x.Concept.ConceptId === action.Concept.ConceptId &&
                                                    x.Concept.Entity.EntityId === action.Concept.Entity.EntityId
                                            }) ===
                                                undefined) {
                                                newActions.push(action);
                                            }
                                        } else {
                                            newActions.push(action);
                                        }
                                    });
                                    rule.Actions = newActions;
                                    if (rule.StatusType != ParametrizationStatus.Create) {
                                        rule.StatusType = ParametrizationStatus.Update;
                                    }
                                    $("#lsvRules").UifListView("editItem", indexRule, rule);
                                }
                            });
                            deferred.resolve();
                            $("#lsvAction").UifListView("clear");
                            RulesSet.SetListAction(newActions);
                        }
                    });

            } else {
                let selected = $("#lsvRules").UifListView("getSelected")[0];
                $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                    if (rule.RuleId === selected.RuleId) {
                        rule.Actions.forEach((action, indexAction) => {
                            if (action === data) {
                                rule.Actions.splice(indexAction, 1);
                                return;
                            }
                        });
                        if (rule.StatusType != ParametrizationStatus.Create) {
                            rule.StatusType = ParametrizationStatus.Update;
                        }
                        $("#lsvRules").UifListView("editItem", indexRule, rule);
                    }
                });
                deferred.resolve();
            }
        } else {
            let selected = $("#lsvRules").UifListView("getSelected")[0];
            $("#lsvRules").UifListView("getData").forEach((rule, indexRule) => {
                if (rule.RuleId === selected.RuleId) {
                    rule.Actions.forEach((action, indexAction) => {
                        if (action === data) {
                            rule.Actions.splice(indexAction, 1);
                            return;
                        }
                    });
                    if (rule.StatusType != ParametrizationStatus.Create) {
                        rule.StatusType = ParametrizationStatus.Update;
                    }
                    $("#lsvRules").UifListView("editItem", indexRule, rule);
                }
            });
            deferred.resolve();
        }
    }

    static SetForm(action) {
        $("#ddlActionType").UifSelect("setSelected", action.AssignType);
        $("#ddlActionType").trigger("itemSelected", [{ Id: action.AssignType.toString() }]);

        switch (action.AssignType) {
            case 1: //assign concept
                $("#ddlActionConcept").UifSelect("setSelected", action.Concept.ConceptId + "-" + action.Concept.Entity.EntityId);
                $("#ddlActionConcept").trigger("itemSelected", [{ Id: action.Concept.ConceptId + "-" + action.Concept.Entity.EntityId }]);

                $("#ddlConceptOperatorType").UifSelect("setSelected", action.ArithmeticOperator.ArithmeticOperatorType);
                $("#ddlConceptOperatorType").trigger("itemSelected", [{ Id: action.ArithmeticOperator.ArithmeticOperatorType.toString() }]);

                $("#ddlActionComparatorType").UifSelect("setSelected", action.ComparatorType);
                $("#ddlActionComparatorType").trigger("itemSelected", [{ Id: action.ComparatorType.toString() }]);


                if (action.Expression.ListValueCode !== undefined) {
                    $("#dllConceptExpressionList").UifSelect("setSelected", action.Expression.ListValueCode);
                }
                else if (action.Expression.RangeValueCode) {
                    $("#dllConceptExpressionList").UifSelect("setSelected", action.Expression.RangeValueCode);
                }
                else if (action.Expression.ConceptId) {
                    $("#dllConceptExpressionList").UifSelect("setSelected", action.Expression.ConceptId + "-" + action.Expression.Entity.EntityId);
                }
                else if (action.Expression.Id) {
                    //$("#dllConceptExpressionList").UifSelect("setSelected", action.Expression.Id);
                    $("#txtConceptExpressionSearch").val(action.Expression.Description);
                    $("#txtConceptExpressionSearch").data("object", action.Expression);
                }
                else {
                    $("#txtConceptExpressionText").val(action.Expression);
                    $("#txtConceptExpressionNumber").val(action.Expression);
                    $("#txtConceptExpressionDecimal").val(action.Expression);
                    $("#txtConceptExpressionDate").val(action.Expression);
                }



                break;

            case 2:// invoke 
                $("#ddlInvokeType").UifSelect("setSelected", action.InvokeType);
                $("#ddlInvokeType").trigger("itemSelected", [{ Id: action.InvokeType.toString() }]);
                switch (action.InvokeType) {
                    case 1://message
                        $("#txtAssignInvoke").val(action.Expression);
                        break;
                    case 2://rule set
                        $("#inputModalRiskInitialRulesPackage").val(action.Expression.Description);

                        break;
                    case 3://function
                        $("#ddlAssignInvokeFunction").UifSelect("setSelected", action.Expression.RuleFunctionId);
                        break;
                }
                break;

            case 3://temporaly value

                $("#txtActionTemp").val(action.ValueTemp);
                $("#txtActionTemp").trigger("blur");

                $("#ddlTempValueOperatorType").UifSelect("setSelected", action.ArithmeticOperator.ArithmeticOperatorType);
                $("#ddlTempValueOperatorType").trigger("itemSelected", [{ Id: action.ArithmeticOperator.ArithmeticOperatorType.toString() }]);

                $("#ddlTempValueExpressionType").UifSelect("setSelected", action.ComparatorType);
                $("#ddlTempValueExpressionType").trigger("itemSelected", [{ Id: action.ComparatorType.toString() }]);


                if (action.Expression.ConceptId) {
                    $("#ddlTempExpressionValue").UifSelect("setSelected", action.Expression.ConceptId + "-" + action.Expression.Entity.EntityId);
                }
                else {
                    $("#txtTempExpressionValue").val(action.Expression);
                    $("#txtTempExpressionValueNum").val(action.Expression);
                }
                break;
        }

        $("#btnSaveActionRule").removeAttr("disabled");
    }

    static ValidateExpression(expression, element) {
        let result = false;
        RequestRules.ValidateExpression(expression)
            .done((data) => {
                if (data.success) {
                    $(element).val(data.result);
                    result = true;
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    result = false;
                }
            });
        return result;
    }

    static ShowPanel(panel) {
        $("#btnSaveActionRule").attr("disabled", "disabled");
        $("#panelAssignConcept").hide();
        $("#panelAssignInvoke").hide();
        $("#panelAssignTemp").hide();
        $("#panelAssignInvokeTxt").hide();
        $("#panelAssignInvokeRuleSet").hide();
        $("#panelAssignInvokeFunction").hide();
        $("#panelAssignConceptType").hide();
        $("#panelAssignConceptOperator").hide();
        $("#panelActionConceptExpression").hide();

        $("#panelConceptExpressionText").hide();
        $("#panelConceptExpressionNumber").hide();
        $("#panelConceptExpressionDecimal").hide();
        $("#panelConceptExpressionDate").hide();
        $("#panelConceptExpressionList").hide();
        $("#panelConceptExpressionSearch").hide();

        $("#panelAssignTempOperator").hide();
        $("#panelAssignTempExpression").hide();


        $("#panelAssignTempExpressionValue").hide();
        $("#panelTempExpressionValueText").hide();
        $("#panelTempExpressionValueDecimal").hide();
        $("#panelTempExpressionValueList").hide();

        $("#txtTempExpressionValue").val("");
        $("#txtTempExpressionValueNum").val("");

        $("#txtConceptExpressionText").removeAttr("maxlength");
        $("#txtConceptExpressionNumber").removeAttr("max");
        $("#txtConceptExpressionNumber").removeAttr("min");
        $("#txtConceptExpressionDecimal").removeAttr("max");
        $("#txtConceptExpressionDecimal").removeAttr("min");

        $("#txtConceptExpressionText").val("");
        $("#txtConceptExpressionNumber").val("");
        $("#txtConceptExpressionDecimal").val("");
        $("#txtConceptExpressionDate").val("");
        $("#txtConceptExpressionSearch").val("");
        $("#txtConceptExpressionSearch").data("object", null);

        $("#txtAssignInvoke").val("");

        switch (panel) {
            case 1:
                $("#panelAssignConcept").show();
                break;

            case 2:
                $("#panelAssignInvoke").show();
                break;

            case 3:
                $("#panelAssignTemp").show();
                break;
            case 4:
                $("#panelAssignInvoke").show();
                $("#panelAssignInvokeTxt").show();
                break;
            case 5:
                $("#panelAssignInvoke").show();
                $("#panelAssignInvokeRuleSet").show();
                break;
            case 6:
                $("#panelAssignInvoke").show();
                $("#panelAssignInvokeFunction").show();
                break;

            case 7:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                break;

            case 8:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                $("#panelAssignConceptType").show();
                break;
            case 9:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                $("#panelAssignConceptType").show();
                $("#panelActionConceptExpression").show();
                $("#panelConceptExpressionText").show();
                break;

            case 10:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                $("#panelAssignConceptType").show();
                $("#panelActionConceptExpression").show();
                $("#panelConceptExpressionNumber").show();
                break;
            case 11:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                $("#panelAssignConceptType").show();
                $("#panelActionConceptExpression").show();
                $("#panelConceptExpressionDecimal").show();
                break;
            case 12:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                $("#panelAssignConceptType").show();
                $("#panelActionConceptExpression").show();
                $("#panelConceptExpressionDate").show();
                break;
            case 13:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                $("#panelAssignConceptType").show();
                $("#panelActionConceptExpression").show();
                $("#panelConceptExpressionList").show();
                break;

            case 14:
                $("#panelAssignTemp").show();
                $("#panelAssignTempOperator").show();
                break;

            case 15:
                $("#panelAssignTemp").show();
                $("#panelAssignTempOperator").show();
                $("#panelAssignTempExpression").show();
                break;

            case 16:
                $("#panelAssignTemp").show();
                $("#panelAssignTempOperator").show();
                $("#panelAssignTempExpression").show();
                $("#panelAssignTempExpressionValue").show();
                $("#panelTempExpressionValueText").show();
                break;
            case 17:
                $("#panelAssignTemp").show();
                $("#panelAssignTempOperator").show();
                $("#panelAssignTempExpression").show();
                $("#panelAssignTempExpressionValue").show();
                $("#panelTempExpressionValueDecimal").show();
                break;
            case 18:
                $("#panelAssignTemp").show();
                $("#panelAssignTempOperator").show();
                $("#panelAssignTempExpression").show();
                $("#panelAssignTempExpressionValue").show();
                $("#panelTempExpressionValueList").show();
                break;
            case 19:
                $("#panelAssignConcept").show();
                $("#panelAssignConceptOperator").show();
                $("#panelAssignConceptType").show();
                $("#panelActionConceptExpression").show();
                $("#panelConceptExpressionSearch").show();
                break;
        }

        $("#formModalConditionRule").valid();
    }

    static ClearForm() {
        ModalActionRule.ShowPanel(null);
        $("#ddlActionType").UifSelect("setSelected", "");
    }

    static ShowModal() {
        $("#modalActionRule").UifModal("showLocal", "Acción");
    }

    static CloseModal() {
        $("#modalActionRule").UifModal("hide");
    }

    static FillAction(action) {
        let str = [];

        switch (action.AssignType) {
            case 1: //ConceptAssign = 1,
                str.push(`${Resources.Language.ToConcept} ${action.Concept.Description.toUpperCase()}`);
                str.push(action.ArithmeticOperator.Description + " ");

                if (!action.HasError) {
                    switch (action.ComparatorType) {
                        case 1: //ConstantValue = 1,
                            str[1] += Resources.Language.TheValue;
                            switch (action.Concept.ConceptType) {
                                case 2: //Rango
                                    str.push(`(${action.Expression.FromValue} - ${action.Expression.ToValue})`);
                                    break;
                                case 3: //Lista
                                    str.push(action.Expression.ListValue);
                                    break;
                                case 4: //Referencia
                                    str.push(action.Expression.Description.toUpperCase() + "(" + action.Expression.Id + ")");
                                    break;
                                default:
                                    str.push(action.Expression);
                                    break;
                            }
                            break;

                        case 2: //ConceptValue = 2, 
                            str[1] += Resources.Language.TheConcept;
                            str.push(action.Expression.Description.toUpperCase());
                            break;

                        case 3: //ExpressionValue = 3, 
                            str[1] += Resources.Language.ExpressionValue;
                            str.push(action.Expression);
                            break;

                        case 4: //TemporalyValue = 4
                            str[1] += Resources.Language.TemporalValue;
                            str.push(`'${action.Expression}'`);
                            break;
                    }
                }
                break;

            case 2: //InvokeAssign = 2,
                str.push(Resources.Language.Invoke + " ");
                if (!action.HasError) {
                    switch (action.InvokeType) {
                        case 1: //MessageInvoke
                            str[0] += Resources.Language.TheMessage;
                            str.push(`'${action.Expression}'`);
                            break;
                        case 2: //RuleSetInvoke
                            str[0] += Resources.Language.TheRuleSet;
                            str.push(` '${action.Expression.Description}'`);
                            break;
                        case 3: //FunctionInvoke
                            str[0] += Resources.Language.TheFunction;
                            str.push(` '${action.Expression.Description}'`);
                            break;
                        default:
                    }
                }
                break;

            case 3: //TemporalAssign = 3
                str.push(`${Resources.Language.ToTemporalValue} '${action.ValueTemp}'`);
                str.push(`${action.ArithmeticOperator.Description} `);

                if (!action.HasError) {
                    switch (action.ComparatorType) {
                        case 1://ConstantValue = 1, 
                            str[1] += Resources.Language.TheValue;
                            str.push(action.Expression);
                            break;
                        case 2://ConceptValue = 2, 
                            str[1] += Resources.Language.TheConcept;
                            str.push(action.Expression.Description.toUpperCase());
                            break;
                        case 3://ExpressionValue = 3, 
                            str[1] += Resources.Language.ExpressionValue;
                            str.push(action.Expression);
                            break;
                        case 4://TemporalyValue = 4
                            str[1] += Resources.Language.TemporalValue;
                            str.push(`'${action.Expression}'`);
                            break;
                    }
                }
                break;
        }

        action.ExpressionStr = str;
        return action;
    }
}