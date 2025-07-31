
var glbTablesExpreses = null;
class UnderwritingTableExpreses extends Uif2.Page {

    getInitialState() {
        UnderwritingTableExpreses.loadInputsDefault();
        request("Underwriting/Underwriting/GetTablesExpreses", null, "GET", "ERROR", UnderwritingTableExpreses.loadListTablesExpreses);

        $("#listViewTablesExpreses").UifListView({
            displayTemplate: "#TablesExpresesTemplate",
            delete: true,
            customAdd: true,
            customEdit: true,
            height: 150,
            deleteCallback: UnderwritingTableExpreses.deleteItemListTablesExpreses,
        });            
    }

    bindEvents() {
        $("#inputTaxes").val(parseFloat(0).toFixed(2)).OnlyDecimals(UnderwritingDecimal);
        $("#btnTableExpreses").on("click", UnderwritingTableExpreses.TablesExpresesLoad);
        $("#btnNewTablesExpreses").on("click", UnderwritingTableExpreses.ClearControls);
        $("#dllParameterizedExpenses").on("itemSelected", UnderwritingTableExpreses.getExecuteType);
        $("#btnAcceptTablesExpreses").on("click", UnderwritingTableExpreses.addItemListTablesExpreses);
        $("#btnTablesExpresesSave").on("click", UnderwritingTableExpreses.saveTableExpreses);
    }

    static TablesExpresesLoad() {
        Underwriting.ShowPanelsIssuance(MenuType.TablesExpreses);
    }

    static ClearControls() {
        $("#dllParameterizedExpenses").UifSelect("setSelected", null);
        $("#inputExecuteType").val("");
        $("#inputTaxesType").val("");
        $("#inputTaxes").val("0.00");
        $("#inputMandatory").attr("checked", false);
        $("#inpuInitiallyIncluded").attr("checked", false);
        $("#inputTotalTablesExpreses").val("Total Gastos de Suscripción");
    }

    static loadListTablesExpreses(data) {
        glbTablesExpreses = data;
        $("#dllParameterizedExpenses").UifSelect({ sourceData: data });
    }

    static getExecuteType(event, selectedItem) {
        var Taxe = glbTablesExpreses.find(x => x.Id === Number(selectedItem.Id)).Taxes;
        var RulesId = glbTablesExpreses.find(x => x.Id === Number(selectedItem.Id)).RuleId;
        var TaxesTypes = glbTablesExpreses.find(x => x.Id === Number(selectedItem.Id)).TaxesType;
        if (selectedItem.Id > 0) {
            if (Taxe != 0 && (RulesId === null || RulesId === 0)) {
                UnderwritingTableExpreses.loadInputsDefault(Taxe, RulesId)
                $("#inputExecuteType").val("TASA UNICA");
                $("#inputTaxesType").val(TaxesTypes);
                
            } else if (Taxe === 0 && (RulesId != null || RulesId != 0)) {
                UnderwritingTableExpreses.loadInputsDefault(Taxe, RulesId)
                $("#inputExecuteType").val("REGLAS DE NEGOCIO");
            }
            
        }
    }

    static loadInputsDefault(Taxe, RulesId) {
        if (Taxe != 0 && (RulesId === null || RulesId === 0)) {
            $("#containerTaxes").show();
            $("#containerRules").hide();
            $("#inputRuleDescription").val("");

        } else if (Taxe === 0 && (RulesId != null || RulesId != 0)) {
            $("#containerTaxes").hide();
            $("#inputTaxesType").val("");
            $("#inputTaxes").val("0.00");
            $("#containerRules").show();
        }
        else {
            $("#containerTaxes").hide();
            $("#containerRules").hide();
        }
    }

    static addItemListTablesExpreses() {
        $("#formTablesExpreses").validate();
        if ($("#formTablesExpreses").valid()) {
            var vIsObligatory, vIsInitiallyIncluded;
            if ($("input:checkbox[name=inputMandatory]:checked").val() === "on")
            {
                vIsObligatory = true;
            }
            else {
                vIsObligatory = false;
            }

            if ($("input:checkbox[name=inpuInitiallyIncluded]:checked").val() === "on") {
                vIsInitiallyIncluded = true;
            }
            else {
                vIsInitiallyIncluded = false;
            }

            if ($("#inputExecuteType").val() === "TASA UNICA") {
                var object = {
                    Id: $("#dllParameterizedExpenses").UifSelect("getSelected"),
                    Description: $('select[name="parameterizedExpenses"] option:selected').text(),
                    TaxesType: $("#inputTaxesType").val(),
                    Taxes: $("#inputTaxes").val(),
                    IsObligatory: vIsObligatory,
                    IsInitiallyIncluded: vIsInitiallyIncluded,
                    StatusTypeService: ParametrizationStatus.Create
                };
                $("#listViewTablesExpreses").UifListView("addItem", object);
            } else if ($("#inputExecuteType").val() === "REGLAS DE NEGOCIO") {
                var object = {
                    Id: $("#dllParameterizedExpenses").UifSelect("getSelected"),
                    Description: $('select[name="parameterizedExpenses"] option:selected').text(),
                    RuleDescription: $("#inputRuleDescription").val(),
                    IsObligatory: vIsObligatory,
                    IsInitiallyIncluded: vIsInitiallyIncluded,
                    StatusTypeService: ParametrizationStatus.Create
                };
                $("#listViewTablesExpreses").UifListView("addItem", object);
            }
        }
    }

    static deleteItemListTablesExpreses(deferred, result) {
        deferred.resolve();
        var tableExpresesList = $("#listViewTablesExpreses").UifListView("getData");
        if (result.Id !== "" && result.Id !== undefined) {
            result.StatusTypeService = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listViewTablesExpreses").UifListView("addItem", result);
        }

    }

    static saveTableExpreses() {
        
        var vlistTablesExpreses = $("#listViewTablesExpreses").UifListView("getData");
        request("Underwriting/Underwriting/SaveTablesExpreses", JSON.stringify({ temporalId: 1, tablesExpresesModelsViews: vlistTablesExpreses }),"POST", "ERROR", UnderwritingTableExpreses.CompletedOperation);

    }

    static CompletedOperation(data) {
        if (data.Message === null) {
            data.Message = 0;
        }
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
                "Agregados" + ':' + data.TotalAdded + '<br> ' +
                "Actualizados" + ':' + data.TotalModified + '<br> ' +
                "Eliminados" + ':' + data.TotalDeleted + '<br> ' +
                "Errores" + ':' + data.Message,
            'autoclose': true
        });
    }
}