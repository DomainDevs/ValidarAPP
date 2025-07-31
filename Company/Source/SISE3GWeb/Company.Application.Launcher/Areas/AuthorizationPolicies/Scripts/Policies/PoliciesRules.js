var gblPolicies = {};
var objConcepts = [];

class PoliciesRules extends Uif2.Page {

    getInitialState() {
        $.ajaxSetup({ async: false });
        $("#btnExportRule").hide();
        $("#Title").text(gblPolicies.Description);
        $("#lsvRules").UifListView({
            displayTemplate: "#template-policies-rules",
            edit: true,
            customEdit: true,
            delete: true,
            add: true,
            customAdd: true,
            drag: true,
            selectionType: "single",
            height: 550,
            title: "Reglas",
            deleteCallback: (deferred) => {
                deferred.resolve();
                $("#lsvCondition").UifListView("clear");
                $("#lsvAction").UifListView("clear");
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
            height: 250,
            title: "Condiciones",
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
            height: 250,
            title: "Acciones",
            deleteCallback: ModalActionRule.DeleteAction
        });

        if (gblPolicies.IdPolicies) {
            $("#btnExportRule").show();
            RequestRules.GetRuleSetByIdRuleSet(gblPolicies.IdPolicies, true)
                .done((data) => {
                    if (data.success) {
                        data.result.Rules.forEach((itemRule) => {
                            $("#lsvRules").UifListView("addItem", itemRule);
                        });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }
    }

    bindEvents() {
        $("#btnBackToPolicies").on("click", this.BackToPolicies);
        $("#btnSavePoliciesRules").on("click", this.SaveRulePolicies);
        $("#lsvRules").on("itemSelected", this.SetListConditionAction);
        $("#lsvCondition").on('draggingEnded', ModalConditionRule.DraggingEnded);
        $("#lsvAction").on('draggingEnded', ModalActionRule.DraggingEnded);
        $("#btnExportRule").on("click", this.ExportRule);
        $("#btnLoadFromFile").on("change", this.LoadFromFile);
    }

    LoadFromFile() {
        const upload = document.getElementById("btnLoadFromFile");
        const file = upload.files[0];
        if (file) {
            $.UifNotify("show", { type: "info", message: "Iniciando Proceso", autoclose: false });
            const formData = new FormData();
            formData.append("file-0", file);
            formData.append("policies.IdPolicies", gblPolicies.IdPolicies);
            formData.append("policies.GroupPolicies.IdGroupPolicies", gblPolicies.GroupPolicies.IdGroupPolicies);
            formData.append("policies.Type", gblPolicies.Type);
            formData.append("policies.Description", gblPolicies.Description);
            formData.append("policies.Position", gblPolicies.Position);
            formData.append("policies.IdHierarchyPolicy", gblPolicies.IdHierarchyPolicy);
            formData.append("policies.IdHierarchyAut", gblPolicies.IdHierarchyAut);
            formData.append("policies.NumberAut", gblPolicies.NumberAut);
            formData.append("policies.Message", gblPolicies.Message);
            formData.append("policies.Enabled", gblPolicies.Enabled);
            formData.append("ruleSetName", gblPolicies.RuleSet === undefined ? gblPolicies.Description : gblPolicies.RuleSet.Description);

            RequestPolicies.ImportRuleSet(formData).done((data) => {
                $("#btnLoadFromFile").val("");
                if (data.success) {
                    if (data.result.RuleSet.FileExceptions != "" && data.result.RuleSet.FileExceptions != null && data.result.RuleSet.FileExceptions != undefined) {
                        $.UifNotify("update", { 'type': "danger", 'message': Resources.Language.ErrorCreatingUpload, 'autoclose': true });
                        DownloadFile(data.result.RuleSet.FileExceptions);
                    }
                    else {
                        $("#lsvRules").UifListView("clear");
                        $("#lsvCondition").UifListView("clear");
                        $("#lsvAction").UifListView("clear");

                        gblPolicies.IdPolicies = data.result.IdPolicies;
                        data.result.RuleSet.Rules.forEach((itemRule) => {
                            $("#lsvRules").UifListView("addItem", itemRule);
                        });
                        $.UifNotify("update", { 'type': "success", 'message': Resources.Language.MsgImportRule, 'autoclose': true });
                    }
                } else {
                    $.UifNotify("update", { 'type': "danger", 'message': data.result.split("|").join("<br>"), 'autoclose': true });
                }
            });
        }
    }

    ExportRule() {
        const rule = gblPolicies.RuleSet;
        if (rule !== false) {
            RequestRules.ExportRuleSet(rule.RuleSetId).done((data) => {
                if (data.success) {
                    $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                } else {
                    window.location.href = rootPath +
                        "RulesAndScripts/RuleSet/ExportRuleSet?ruleSetId=" +
                        rule.RuleSetId;
                }
            });
        }
    }

    SaveRulePolicies() {
        let rule = $("#lsvRules").UifListView("getData");
        let valid = true;

        if (rule.length === 0) {
            $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.EnterMinimumRule, 'autoclose': true });
            valid = false;
        } else {



            rule.forEach((item) => {
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

        let ruleSet = {
            Rules: rule,
            Type: 1
        }

        if (!gblPolicies.IdPolicies) {
            RequestPolicies.CreateRulePolicies(gblPolicies, ruleSet)
                .done(data => {
                    if (data.success) {
                        PoliciesRules.backToPolicies();
                        $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });

        } else {
            RequestPolicies.UpdateRulePolicies(gblPolicies, ruleSet)
                .done(data => {
                    if (data.success) {
                        PoliciesRules.backToPolicies();
                        $.UifNotify("show", { 'type': "success", 'message': data.result, 'autoclose': true });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
        }
    }

    SetListConditionAction() {
        $("#lsvCondition").UifListView("clear");
        $("#lsvAction").UifListView("clear");

        let ruleSelected = $("#lsvRules").UifListView("getSelected");
        if (ruleSelected.length !== 0) {
            PoliciesRules.SetListCondition(ruleSelected[0].Conditions);
            PoliciesRules.SetListAction(ruleSelected[0].Actions);
        }
    }

    BackToPolicies() {
        PoliciesRules.backToPolicies();
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

    static backToPolicies() {
        objConcepts = [];
        router.run("prtPolicies");
    }
}