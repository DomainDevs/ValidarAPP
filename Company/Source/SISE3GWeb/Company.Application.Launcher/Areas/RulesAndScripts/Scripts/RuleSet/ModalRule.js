
class ModalRule extends Uif2.Page {
    getInitialState() {
    }

    bindEvents() {
        $("#lsvRules").on("rowAdd", this.AddRule);
        $("#lsvRules").on("rowEdit", this.EditRule);
        $("#btnSaveRule").on("click", this.SaveRule);
    }

    /**
     * Guarda o edita un elemento a la lisview
     */
    SaveRule() {
        if ($("#formModalRule").valid()) {
            let index = $("#hdnIndexRule").val();

            if (index) {
                let rule = $("#lsvRules").UifListView("getData")[index];
                rule.Description = $("#txtDescriptionRule").val();
                if (rule.StatusType === null || rule.StatusType === undefined)
                {
                    rule.StatusType = ParametrizationStatus.Update; 
                }
                $("#lsvRules").UifListView("editItem", index, rule);
            } else {
                let orderedById = $("#lsvRules").UifListView("getData").sort(function (a, b) {
                    return a.RuleId > b.RuleId ? 1 : 0;
                });
                let nextIndex = 1;
                if (orderedById.length !== 0) {
                    nextIndex = orderedById[orderedById.length - 1].RuleId + 1;
                }

                let rule = {
                    RuleId: nextIndex,
                    Description: $("#txtDescriptionRule").val(),
                    Actions: [],
                    Conditions: [],
                    Parameters: []
                };
                rule.StatusType = ParametrizationStatus.Create;
                $("#lsvRules").UifListView("addItem", rule);
            }

            ModalRule.CloseModal();
            var lsvRulesLength = $("#lsvRules").UifListView("getData");
            $("#lsvRules").UifListView("setSelected", lsvRulesLength.length - 1, true);
            RulesSet.SetListConditionActionNew();
        }
    }

    /**
    * @summary 
    * Evento al agregar una nueva regla al listView
    */
    AddRule() {
        ClearValidation("#formModalRule");
        ModalRule.ClearForm();
        if (gblPolicies.GroupPolicies) {
            ModalRule.ShowModal();
        } else {
            let ruleSet = RulesSet.GetSelectedRuleSet();
            if (ruleSet) {
                ModalRule.ShowModal();
            }
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
    EditRule(event, data, index) {
        ClearValidation("#formModalRule");
        ModalRule.ClearForm();

        $("#hdnIndexRule").val(index);
        $("#txtDescriptionRule").val(data.Description);

        ModalRule.ShowModal();
    }

    /**
   * @summary 
   * limpia el formulario de la ventana modal de reglas
  */
    static ClearForm() {
        $("#txtDescriptionRule").val("");
        $("#hdnIndexRule").val("");
    }

    /**
    * @summary 
    * abre la ventana modal de reglas
   */
    static ShowModal() {
        $("#modalRule").UifModal("showLocal", "Regla");
    }

    /**
     * @summary 
     * Cierra la ventana modal de reglas
    */
    static CloseModal() {
        $("#modalRule").UifModal("hide");
    }
}