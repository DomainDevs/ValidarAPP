var currentDate = null;
var packageIdRule = [];
class ModalRuleSet extends Uif2.Page {
    getInitialState() {
        $("#txtDescriptionRuleSet").TextTransform(ValidatorType.UpperCase);
    }

    bindEvents() {
        $("#lsvRulesSet").on("rowAdd", this.AddRuleSet);
        $("#lsvRulesSet").on("rowEdit", this.EditRuleSet);
		$("#btnSaveRuleSet").on("click", this.SaveRuleSet);
		$("#chkActivo").on("click", this.ActivoUser);
    }

    /**
     * Guarda o edita un elemento a la lisview
     */
    SaveRuleSet() {
        if ($("#formModalRuleSet").valid()) {
            let ruleEdit = $("#lsvRulesSet").UifListView("getData")[0];
            let formModal = $("#formModalRuleSet").serializeObject();
            formModal.Level = $("#ddlLevel").UifSelect("getSelectedSource");
            formModal.Package = $("#ddlPackage").UifSelect("getSelectedSource");
            formModal.Type = 1;
            formModal.IsEvent = false;
            formModal.ActiveType = parseInt($('#txtActiveType').val());
            if (formModal.ActiveType > 0) {
                formModal.Active = $('#chkActivo').is(':checked');
            }
            else {
                formModal.ActiveType = null;
            }
            if (formModal.hdnNew == "true") {
                RulesSet.ClearListViews(1);
                RulesSet.SetListRulesSet([formModal]);
                ModalRuleSet.GetConceptsByRuleSet($("#ddlPackage").UifSelect("getSelected"), $("#ddlLevel").UifSelect("getSelected"));
            } else if (ruleEdit.Level.LevelId == formModal.Level.LevelId) {
                ruleEdit.Description = $("#txtDescriptionRuleSet").val();
                ruleEdit.ActiveType = parseInt($('#txtActiveType').val());
                if (ruleEdit.ActiveType > 0) {
                    ruleEdit.Active = $('#chkActivo').is(':checked');
                }
                else {
                    ruleEdit.ActiveType = null;
                }
                $("#lsvRulesSet").UifListView("editItem", 0, ruleEdit);
            }
            else {
                RulesSet.ClearListViews(1);
                RulesSet.SetListRulesSet([formModal]);
                ModalRuleSet.GetConceptsByRuleSet($("#ddlPackage").UifSelect("getSelected"), $("#ddlLevel").UifSelect("getSelected"));
            }
            ModalRuleSet.CloseModal();
        }
    }

    /**
    * @summary 
    * Evento al agregar una nueva regla al listView
    */
    AddRuleSet() {
        $("#hdnNew").val(true);
        ClearValidation("#formModalRuleSet");
        if ($("#ddlPackage").UifSelect("getSelected")) {
            ModalRuleSet.ClearForm();
            let _package = $("#ddlPackage").UifSelect("getSelectedSource");
            $("#txtPackage").val(_package.Description);
            RequestRules.GetCurrentDatetime().done(function (data) {
                if (data.success) {
                    currentDate = FormatDate(data.result);
                    $("#currentFrom").val(data.result.substring(0, 10));
                    $("#currentFrom").prop("disabled", true);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
            $("#currentTo").val("");
			$("#currentTo").prop("disabled", true);
			$('#chkActivo').prop('checked', false);
			$("#txtActivo").text(Resources.Language.Active);
			$('#txtActiveType').val(0);
            ModalRuleSet.ShowModal();
        }
    }

    /**
     * @summary 
     * Evento al editar una regla del listView
     * @param {} event 
     * @param {Object<Rule>} data
     * regla a editar
     * @param {int} index 
     * index a editar
     */
    EditRuleSet(event, data, index) {
        ClearValidation("#formModalRuleSet");
        $("#hdnNew").val(false);
        ModalRuleSet.ClearForm();

        $("#hdnIndexRuleSet").val(index);
        $("#txtDescriptionRuleSet").val(data.Description);
        $("#txtPackage").val(data.Package.Description);
        $("#ddlLevel").UifSelect("setSelected", data.Level.LevelId);
        $("#currentFrom").val(data.CurrentFrom);
        $("#currentFrom").prop("disabled", true);
        if (data.CurrentFrom === undefined) {
            $("#currentTo").val(data.CurrentTo);
            $("#currentTo").prop("disabled", true);

            RequestRules.GetCurrentDatetime().done(function (dataDate) {
                if (dataDate.success) {
                    currentDate = FormatDate(dataDate.result);
                    $("#currentFrom").UifDatepicker('setValue', FormatDate(dataDate.result.substring(0, 10)));
                    $("#currentFrom").prop("disabled", true);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': dataDate.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#currentTo").val(data.CurrentTo);
            $("#currentTo").prop("disabled", true);
        }

        if (data.RuleSetId) {
            $("#ddlLevel").attr("disabled", "disabled");
        } else {
            $("#ddlLevel").removeAttr("disabled");
        }

		$('#txtActiveType').val(data.ActiveType);

		if (data.Active == false && data.ActiveType == 1) {
			$('#chkActivo').prop('checked', false);
			$("#txtActivo").text(Resources.Language.LabelDeactivatedByTheUser);
		}
		else if (data.Active == false && data.ActiveType == 2) {
			$('#chkActivo').prop('checked', false);
			$("#txtActivo").text(Resources.Language.LabelDeactivatedByTheSystem);
		}
		else if (data.Active == true && data.ActiveType == 1) {
			$('#chkActivo').prop('checked', true);
			$("#txtActivo").text(Resources.Language.LabelActiveByTheUser);
		}
		else if (data.Active == true && data.ActiveType == 2) {
			$('#chkActivo').prop('checked', true);
			$("#txtActivo").text(Resources.Language.LabelActiveForTheSystem);
		}
		else {
			$('#chkActivo').prop('checked', false);
			$("#txtActivo").text(Resources.Language.Active);
		}

        ModalRuleSet.ShowModal();
	}

	ActivoUser() {
		if ($('#chkActivo').is(':checked')) {
			$('#txtActiveType').val(1);
			$("#txtActivo").text(Resources.Language.LabelActiveByTheUser);
		}
		else {
			$('#txtActiveType').val(1);
			$("#txtActivo").text(Resources.Language.LabelDeactivatedByTheUser);
		}
	}

    static GetConceptsByRuleSet(packageId, levelId) {
        RequestRules.GetEntitiesByPackageIdLevelId(packageId, levelId).done((dataE) => {
            if (dataE.success) {
                let listEntities = [];
                let levels = [];
                dataE.result.forEach((item) => {
                    listEntities.push(item.EntityId);
                    levels.push(item.LevelId);
                });

                RequestConcepts.GetConceptsByFilter(listEntities, "").done((result) => {
                    if (result.success) {
                        result.result.forEach((concept, i) => {
                            let facade = dataE.result.filter((facades) => {
                                return facades.EntityId === concept.Entity.EntityId;
                            })[0];
                            result.result[i].DescriptionFacade =
                                concept.Description + " (" + facade.Description + ")";
                            result.result[i].ConceptIdFacadeId =
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

                RequestRules.GetRulesByFilter(packageId, levels, true, false, "", false).done((data) => {
                    if (data.success) {
                        $("#ddlAssignInvokeRuleSet").UifSelect({ sourceData: data.result });
                        packageIdRule = data.result;
                        if ($("#lsvRulesSet").UifListView("getData").length > 0) {
                            let idRule = $("#lsvRulesSet").UifListView("getData")[0].RuleSetId;
                            packageIdRule = data.result.filter((paqRules) => {
                                return paqRules.RuleSetId != idRule;
                            });
                        }
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });

                RequestRules.GetRuleFunctionsByIdPackageLevels(packageId, levels).done((data) => {
                    if (data.success) {
                        $("#ddlAssignInvokeFunction").UifSelect({ sourceData: data.result });
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
                    }
                });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
    * @summary 
    * limpia el formulario de la ventana modal de reglas
    */
    static ClearForm() {
        $("#ddlLevel").removeAttr("disabled");
        $("#txtDescriptionRuleSet").val("");
        $("#hdnIndexRuleSet").val("");
        $("#txtPackage").val("");
        $("#ddlLevel").val("");
    }

    /**
    * @summary 
    * abre la ventana modal de reglas
    */
    static ShowModal() {
        $("#modalRuleSet").UifModal("showLocal", "Paquete de reglas");
    }

    /**
     * @summary 
     * Cierra la ventana modal de reglas
    */
    static CloseModal() {
        $("#modalRuleSet").UifModal("hide");
    }
}
