class RequestLevels {
    /**
    * @summary 
    * peticion ajax que Obtiene los niveles asociados al grupo de politicas
    **/
    static GetLevelsByIdGroupPolicies(idGroupPolicies, level) {
        return $.ajax({
            type: "POST",
            data: { "idGroupPolicies": idGroupPolicies, "level": level},
            url: rootPath + "AuthorizationPolicies/Policies/GetLevelsByIdGroupPolicies"
        });
    }

    /**
     * @summary 
     * Obtiene los niveles por el paquete
     * @param {int} idPackage 
     * id del paquete
     */
    static GetLevelsByIdPackage(idPackage) {
        return $.ajax({
            type: "POST",
            data: { "idPackage": idPackage },
            url: rootPath + "RulesAndScripts/RuleSet/GetLevelsByIdPackage"
        });
    }
}

class RequestPackage {
    /**
     * @summary 
     * peticion ajax que consulta los paquetes asociados a politicas
    **/
    static GetPackagePolicies() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "AuthorizationPolicies/Policies/GetPackagePolicies"
        });
    }

    /**
     * @summary 
     * peticion ajax que Obtiene los paquetes habilitados
    **/
    static GetPackages() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSet/GetPackages"
        });
    }
}

class RequestRules {
    /**
    * @summary
    * peticion ajax que obtiene los paquetes de reglas por el filtro
    * @param {idPackage} id del paquete
    * @param {levels} lista de niveles a consultar
     * @param {withDecisionTable} incluir tablas de decision
    * @param {isPolicie} es una politica
    * @param {filter} like para la descripcion
    **/
    static GetRulesByFilter(idPackage, levels, withDecisionTable, isPolicie, filter, maxRow) {
        return $.ajax({
            type: "POST",
            data: {
                "idPackage": idPackage,
                "levels": levels,
                "withDecisionTable": withDecisionTable,
                "isPolicie": isPolicie,
                "filter": filter,
                "maxRow": maxRow
            },
            url: rootPath + "RulesAndScripts/RuleSet/GetRulesByFilter"
        });
    }

    /**
    * @summary
    * peticion ajax que obtiene los paquetes de reglas que son DT
    * @param {idPackage} id del paquete
    **/
    static GetRulesDecisionTable(idPackage) {
        return $.ajax({
            type: "POST",
            data: { "idPackage": idPackage },
            url: rootPath + "RulesAndScripts/RuleSet/GetRulesDecisionTable"
        });
    }

    /**
     * @summary
     * Obtiene el paquete de regla completo, con sus respectivas reglas del xml
     * @param {int} idRuleSet 
     * id de la regla
     */
    static GetRuleSetByIdRuleSet(idRuleSet, deserializeXml) {
        return $.ajax({
            type: "POST",
            data: { "idRuleSet": idRuleSet, "deserializeXml": deserializeXml },
            url: rootPath + "RulesAndScripts/RuleSet/GetRuleSetByIdRuleSet"
        });
    }

    /**
    *@summary
    *obtiene los tipos de comparadores para la condicion
    **/
    static GetConditionComparatorType() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSet/GetConditionComparatorType"
        });
    }

    /**
   *@summary
   *obtiene los tipos de comparadores para la accion
   **/
    static GetActionComparatorType() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSet/GetActionComparatorType"
        });
    }

    /**
    *@summary
    *Obtine los tipos de acciones para la regla
    **/
    static GetActionType() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSet/GetActionType"
        });
    }

    /**
    * @summary 
    * Obtine los tipos de invocaciones para la accion
    **/
    static GetInvokeType() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSet/GetInvokeType"
        });
    }

    /**
     * @summary 
     * Obtine los tipos de operadores aritmeticos para la accion
     */
    static GetArithmeticOperatorType() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + "RulesAndScripts/RuleSet/GetArithmeticOperatorType"
        });
    }

    /**
     * @summary 
     * obtiene los tipos de comparadores para la condicion
    **/
    static ValidateExpression(expression) {
        return $.ajax({
            type: "POST",
            data: { "expression": expression },
            url: rootPath + "RulesAndScripts/RuleSet/ValidateExpression"
        });
    }

    /**
     * @summary 
     * Obtiene las funciones de reglas que concuerden con la busqueda
     * @param {int} idPackage 
     * id del paquete
     * @param {List<int>} levels 
     * id de los niveles
     */
    static GetRuleFunctionsByIdPackageLevels(idPackage, levels) {
        return $.ajax({
            type: "POST",
            data: { "idPackage": idPackage, "levels": levels },
            url: rootPath + "RulesAndScripts/RuleSet/GetRuleFunctionsByIdPackageLevels"
        });
    }

    /**
     * Obtiene las entities de la tabla positionEntity por paquete y nivel
     * @param {int} packageId 
     * id del paquete
     * @param {int} levelId 
     * id del nivel
     * @returns {} 
     */
    static GetEntitiesByPackageIdLevelId(packageId, levelId) {
        return $.ajax({
            type: "POST",
            data: { "packageId": packageId, "levelId": levelId },
            url: rootPath + "RulesAndScripts/RuleSet/GetEntitiesByPackageIdLevelId"
        });
    }

    /**
    * @summary 
    *  Realiza la creacion de un paquete de reglas
    * @param {} ruleSet 
    * paquete de reglas a crear
    */
    static CreateRuleSet(ruleSet) {
        return $.ajax({
            type: "POST",
            data: { "ruleSet": JSON.stringify(ruleSet) },
            url: rootPath + "RulesAndScripts/RuleSet/CreateRuleSet"
        });
    }

    /**
     * @summary 
     * Realiza la modificacion del paquete de reglas
     * @param {} ruleSet 
     * paquete de reglas a modificar
     */
    static UpdateRuleSet(ruleSet) {
        return $.ajax({
            type: "POST",
            data: { "ruleSet": JSON.stringify(ruleSet) },
            url: rootPath + "RulesAndScripts/RuleSet/UpdateRuleSet"
        });
    }

    /**
     * @summary
     * Realiza la eliminacion del paquete de reglas
     * @param {number} ruleSetId
     */
    static DeleteRuleSet(ruleSetId) {
        return $.ajax({
            type: "POST",
            data: { "ruleSetId": ruleSetId },
            url: rootPath + "RulesAndScripts/RuleSet/DeleteRuleSet"
        });
    }


    static ExportRuleSet(ruleSetId) {
        return $.ajax({
            type: "POST",
            data: { "ruleSetId": ruleSetId },
            url: rootPath + "RulesAndScripts/RuleSet/ExportRuleSet"
        });
    }

    static ImportRuleSet(data) {
        return $.ajax({
            url: rootPath + "RulesAndScripts/RuleSet/ImportRuleSet",
            type: "POST",
            "data": data,
            contentType: false,
            cache: false,
            processData: false
        });
    }

    static GetCurrentDatetime() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'RulesAndScripts/RuleSet/DateNow',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static ExportRules() {
        return $.ajax({
            type: 'GET',
            async: true,
            url: rootPath + 'RulesAndScripts/RuleSet/ExportRules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class RulesSet extends Uif2.Page {

    getInitialState() {
        $.ajaxSetup({ async: false });
        $("#lsvRulesSet").UifListView({
            displayTemplate: "#template-policies-rulesSet",
            edit: true,
            customEdit: true,            
            add: true,
            customAdd: true,
            height: 150,
            selectionType: "single",            
            title: Resources.Language.LabelRuleSet
        });
        $("#lsvRules").UifListView({
            displayTemplate: "#template-policies-rules",
            edit: true,
            customEdit: true,
            delete: true,
            add: true,
            customAdd: true,
            drag: true,
            selectionType: "single",
            height: 210,
            title: Resources.Language.Rules,
            deleteCallback: (deferred, result) => {
                deferred.resolve();
                $("#lsvCondition").UifListView("clear");
                $("#lsvAction").UifListView("clear");
                if (result.StatusType !== ParametrizationStatus.Create)
                {
                    result.StatusType = ParametrizationStatus.Delete;
                    result.allowEdit = false;
                    result.allowDelete = false;
                    $("#lsvRules").UifListView("addItem", result);
                }
            }
        });
        $("#lsvCondition").UifListView({
            displayTemplate: "#template-policies-rules-Condition",
            edit: true,
            customEdit: true,
            delete: true,
            add: true,
            customAdd: true,
            drag: true,
            height: 441,
            title: Resources.Language.LabelConditions,
            deleteCallback: ModalConditionRule.DeleteCondition
        });
        $("#lsvAction").UifListView({
            displayTemplate: "#template-policies-rules-Action",
            edit: true,
            customEdit: true,
            delete: true,
            add: true,
            customAdd: true,
            drag: true,
            height: 441,
            title: Resources.Language.Actions,
            deleteCallback: ModalActionRule.DeleteAction
        });



        RequestPackage.GetPackages().done((data) => {
            if (data.success) {
                $("#ddlPackage").UifSelect({
                    sourceData: data.result,
                    selectedId: 1
                });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
        RequestLevels.GetLevelsByIdPackage(1).done((data) => {
            if (data.success) {
                $("#ddlLevel").UifSelect({
                    sourceData: data.result
                });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });


    }

    bindEvents() {
        $("#ddlPackage").on("itemSelected", this.ChangePackage);
        $("#lsvRules").on("itemSelected", this.SetListConditionAction);
        $("#lsvRulesSet").on("itemSelected", this.SetResaltRules);
        $("#btnRecordRule").on("click", this.RecordRule);
        $("#SearchRule").on("search", this.SearchRuleSet);
        $("#btnSearchAdvRules").on("click", AdvancedSearchRules.showDropDow);
        $("#btnExit").on("click", this.Exit);
        $("#btnExportRule").on("click", this.ExportRule);
        $("#btnLoadFromFile").on("change", this.LoadFromFile);
        $("#lsvCondition").on('draggingEnded', ModalConditionRule.DraggingEnded);
        $("#lsvAction").on('draggingEnded', ModalActionRule.DraggingEnded);
        $("#btnExportAllRule").click(this.exportRulesByFilter);
        
    }

    LoadFromFile() {
        const upload = document.getElementById("btnLoadFromFile");
        const file = upload.files[0];
        if (file) {
            $.UifNotify("show", { type: "info", message: "Iniciando Proceso", autoclose: false });
            const formData = new FormData();
            formData.append("file-0", file);
            RequestRules.ImportRuleSet(formData).done((data) => {
                $("#btnLoadFromFile").val("");
                if (data.success) {
                    if (data.result.FileExceptions != "" && data.result.FileExceptions != null && data.result.FileExceptions != undefined) {
                        $.UifNotify("update", { 'type': "danger", 'message': Resources.Language.ErrorCreatingUpload, 'autoclose': true });
                        DownloadFile(data.result.FileExceptions);
                    }
                    else {
                        let ruleSet = data.result;

                        ModalRuleSet.GetConceptsByRuleSet(ruleSet.Package.PackageId, ruleSet.Level.LevelId);
                        $("#ddlPackage").UifSelect("setSelected", data.result.Package.PackageId);
                        $("#SearchRule").val(data.result.RuleSetId.toString());
                        $("#SearchRule").trigger("search", [data.result.RuleSetId].toString());
                        $("#lsvRulesSet").UifListView("setSelected", 0, true);

                        $.UifNotify("update", { 'type': "success", 'message': Resources.Language.MsgImportRule, 'autoclose': true });
                    }
                    
                } else {
                    $.UifNotify("update", { 'type': "danger", 'message': data.result.split("|").join("<br>"), 'autoclose': true });
                }
            });
        }
    }


    ExportRule() {
        const rule = RulesSet.GetSelectedRuleSet();
        if (rule !== false) {
            RequestRules.ExportRuleSet(rule.RuleSetId).done((data) => {
                if (data.success) {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                } else {
                    window.open(rootPath + "RulesAndScripts/RuleSet/ExportRuleSet?ruleSetId=" + rule.RuleSetId);
                }
            });
        }
    }

    Exit() {
        $.UifDialog('confirm', { 'message': AppResources.SegureExit, 'title': AppResources.Confirmation }, function (result) {
            if (result === true) {
                window.location = rootPath + "Home/Index";
            }
        });
    }

    SearchRuleSet(event, value) {
        //RulesSet.ClearListViews(1);
        $("#SearchRule").val("");
        let packageId = $("#ddlPackage").UifSelect("getSelected");

        if (value && packageId) {
            RequestRules.GetRulesByFilter(packageId, [], false, false, value.trim(), true).done((data) => {
                if (data.success) {
                    RulesSet.SetListAdvancedRulesSet(data.result);
                    if (data.result.length > 1) {
                        AdvancedSearchRules.showDropDow();
                    }
                    else if (data.result.length==0)
                    {
                        $.UifNotify("show", { 'type': "danger", 'message': 'No existe el paquete de reglas', 'autoclose': true });
                    }
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static UpdateRule() {
        const selected = $("#lsvRules").UifListView("getSelected")[0];
        const findCountry = function (element) {
            return element.Description === selected.Description;
        }
        const index = $("#lsvRules").UifListView("findIndex", findCountry);
        selected.HasError = false;

        const conditionError = selected.Conditions.find((x) => x.HasError === true);
        const actionError = selected.Actions.find((x) => x.HasError === true);

        if (conditionError === actionError) {
            selected.HasError = false;
        } else {
            selected.HasError = true;
        }
        if (selected.StatusType != ParametrizationStatus.Create) {
            selected.StatusType = ParametrizationStatus.Update;
        }
        $("#lsvRules").UifListView("editItem", index, selected);
    }

    RecordRule() {
        let ruleSet = $("#lsvRulesSet").UifListView("getSelected");

        if (ruleSet.length !== 0) {
            let rule = $("#lsvRules").UifListView("getData").filter(function (item) {
                return item.StatusType != ParametrizationStatus.Delete;
            });
            let valid = true;
            let error = false;

            if (rule.length === 0) {
                $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.EnterMinimumRule, 'autoclose': true });
                valid = false;
            } else {
                rule.forEach((item) => {
                    if (item.HasError) {
                        error = true;
                    }

                    if (item.Conditions.length === 0) {
                        $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.MsgRuleMinCondition, 'autoclose': true });
                        valid = false;
                        return;
                    }
                    if (item.Actions.length === 0) {
                        $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.MsgRuleMinAction, 'autoclose': true });
                        valid = false;
                        return;
                    }
                });
            }
            if (!valid) {
                return;
            }
            if (error) {
                $.UifNotify("show", { 'type': "danger", 'message': "Existen reglas con errores", 'autoclose': true });
                return;
            }

            ruleSet[0].Rules = rule;
            if (ruleSet[0].RuleSetId) {
                RequestRules.UpdateRuleSet(ruleSet[0])
                    .done(data => {
                        if (data.success) {
                            const ruleSelected = $("#lsvRulesSet").UifListView("getSelected")[0];
                            RequestRules.GetCurrentDatetime().done(function (dataDateTime) {
                                if (dataDateTime.success) {
                                    currentDate = FormatDate(dataDateTime.result);
                                    ruleSelected.CurrentTo = dataDateTime.result.substring(0, 10);
                                    ruleSelected.HasError = false;
                                    $("#lsvRulesSet").UifListView("editItem", 0, ruleSelected);
                                    RulesSet.ClearColors();
                                }

                            });
                            $.UifNotify("show", { 'type': "success", 'message': Resources.Language.MessageSavedSuccessfully, 'autoclose': true });
                        } else {
                            $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                        }
                    });
            } else {
                RequestRules.CreateRuleSet(ruleSet[0])
                    .done(data => {
                        if (data.success) {
                            const ruleSelected = $("#lsvRulesSet").UifListView("getSelected")[0];
                            ruleSelected.RuleSetId = data.result.RuleSetId;
                            ruleSelected.CurrentFrom = FormatDate(data.result.CurrentFrom, 1);
                            ruleSelected.CurrentTo = FormatDate(data.result.CurrentTo, 1);
                            $("#lsvRulesSet").UifListView("editItem", 0, ruleSelected);
                            RulesSet.ClearColors();
                            $.UifNotify("show", { 'type': "success", 'message': Resources.Language.MessageSavedSuccessfully, 'autoclose': true });
                        } else {
                            $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                        }
                    });
            }
        }
    }

    SetResaltRules() {        
        $("#lsvRulesSet").UifListView("setSelected", 0, true);
    }

    static SetListRules() {
        let ruleSet = $("#lsvRulesSet").UifListView("getSelected");
        RulesSet.ClearListViews(2);
        if (ruleSet.length !== 0) {
            if (ruleSet[0].RuleSetId) {
                RulesSet.GetXmlByRuleId(ruleSet[0].RuleSetId);
            }
        }
    }

    SetListConditionAction() {
        RulesSet.ClearListViews(3);
        let ruleSelected = $("#lsvRules").UifListView("getSelected");
        if (ruleSelected.length !== 0 && ruleSelected[0].StatusType != ParametrizationStatus.Delete) {
            RulesSet.SetListCondition(ruleSelected[0].Conditions);
            RulesSet.SetListAction(ruleSelected[0].Actions);
        }
    }

    ChangePackage(event, item) {
        if (item.Id) {

        } else {
            RulesSet.ClearListViews(1);
        }
    }

    deleteRuleSet(deferred, data) {
        if (data.RuleSetId) {
            RequestRules.DeleteRuleSet(data.RuleSetId).done((result) => {
                if (result.success) {
                    deferred.resolve();
                    $("#lsvRules").UifListView("clear");
                    $("#lsvCondition").UifListView("clear");
                    $("#lsvAction").UifListView("clear");
                    $.UifNotify("show", { 'type': "success", 'message': result.result, 'autoclose': true });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': result.result, 'autoclose': true });
                }
            });

        } else {
            deferred.resolve();
            $("#lsvRules").UifListView("clear");
            $("#lsvCondition").UifListView("clear");
            $("#lsvAction").UifListView("clear");
        }
    }

    static SetListConditionActionNew() {
        RulesSet.ClearListViews(3);
        let ruleSelected = $("#lsvRules").UifListView("getSelected");
        if (ruleSelected.length !== 0 && ruleSelected[0].StatusType != ParametrizationStatus.Delete) {
            RulesSet.SetListCondition(ruleSelected[0].Conditions);
            RulesSet.SetListAction(ruleSelected[0].Actions);
        }
    }

    static HideSearchAdv() {
        dropDownSearchAdv.hide();
    }

    static GetXmlByRuleId(ruleId) {
        objConcepts = [];
        RequestRules.GetRuleSetByIdRuleSet(ruleId, true).done((data) => {
            if (data.success) {
                let ruleSet = $("#lsvRulesSet").UifListView("getSelected")[0];
                ModalRuleSet.GetConceptsByRuleSet(ruleSet.Package.PackageId, ruleSet.Level.LevelId);

                data.result.Rules.forEach((rule) => {
                    $("#lsvRules").UifListView("addItem", rule);
                });                
                if (data.result.HasError) {
                    ruleSet.HasError = true;
                    $("#lsvRulesSet").UifListView("editItem", 0, ruleSet);
                }
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
    }

    static SetListRulesSet(rules) {
        RulesSet.ClearListViews(1);
        rules.forEach(item => {
            item.CurrentFrom = FormatDate(item.CurrentFrom, 1);
            if (item.RuleSetVer === 1) {
                item.CurrentTo = null;

            }
            else {
                item.CurrentTo = FormatDate(item.CurrentTo, 1);
            }

            $("#lsvRulesSet").UifListView("addItem", item);
            $("#lsvRulesSet").UifListView("setSelected", 0, true);
            RulesSet.SetListRules();
        });
	}

	static SetListAdvancedRulesSet(rules) {
		$("#listViewAdv").UifListView("clear")
		rules.forEach(item => {
			item.CurrentFrom = FormatDate(item.CurrentFrom, 1);
			if (item.RuleSetVer === 1) {
				item.CurrentTo = null;
			}
			else {
				item.CurrentTo = FormatDate(item.CurrentTo, 1);
			}
            if (rules.length > 1) {
                $("#listViewAdv").UifListView("addItem", item);
            }
            else {
                RulesSet.SetListRulesSet([item]);
                //RulesSet.ClearListViews(1);
                //$("#lsvRulesSet").UifListView("addItem", item);
            }
		});
	}

    static ClearListViews(list) {
        switch (list) {
            case 1:
                $("#lsvRulesSet").UifListView("clear");
                $("#lsvRules").UifListView("clear");
                $("#lsvCondition").UifListView("clear");
                $("#lsvAction").UifListView("clear");
                break;
            case 2:
                $("#lsvRules").UifListView("clear");
                $("#lsvCondition").UifListView("clear");
                $("#lsvAction").UifListView("clear");
                break;
            case 3:
                $("#lsvCondition").UifListView("clear");
                $("#lsvAction").UifListView("clear");
                break;
        }
    }

    static GetSelectedRuleSet() {
        let selected = $("#lsvRulesSet").UifListView("getSelected");

        if (selected.length === 0) {
            $.UifNotify("show", { 'type': "warning", 'message': "Seleccione un paquete de reglas", 'autoclose': true });
            return false;
        } else {
            return selected[0];
        }
    }

    static SetListCondition(conditions) {
        conditions.forEach((condition) => {
            $("#lsvCondition").UifListView("addItem", ModalConditionRule.FillCondition(condition));
        });
    }

    static SetListAction(actions) {
        actions.forEach((action) => {
            $("#lsvAction").UifListView("addItem", ModalActionRule.FillAction(action));
        });
    }

    exportRulesByFilter() {

        RequestRules.ExportRules().done((data) => {
            if (data.success) {
                window.open(data.result);
            }
            else {
                $.UifNotify("show", { 'type': "info", 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ClearColors() {
        var listRules = $("#lsvRules").UifListView("getData").filter(function (item) { return item.StatusType != ParametrizationStatus.Delete });
        $("#lsvAction").UifListView("clear");
        $("#lsvCondition").UifListView("clear");
        $("#lsvRules").UifListView("clear");
        $.each(listRules, function (index, item) {
            item.StatusType = null;
            $("#lsvRules").UifListView("addItem", item);
        })
    }

}