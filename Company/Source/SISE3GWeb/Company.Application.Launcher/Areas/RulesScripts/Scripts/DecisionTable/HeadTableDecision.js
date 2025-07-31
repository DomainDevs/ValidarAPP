$.ajaxSetup({ async: false });
ControlTableCabecera = {
    selectLevel: $('#selectLevel').on("binded", function () {
        if ($('#selectLevel').UifSelect("getSelected") > 0) {
            $("#conditionSelect").UifSelect({ source: null });
            $("#actionSelect").UifSelect({ source: null });
            SearchCombo.ListaControlSearch('#conditionSelect', 315, null, $('#selectLevel').UifSelect("getSelected"));
            SearchCombo.ListaControlSearch('#actionSelect', 315, null, $('#selectLevel').UifSelect("getSelected"));
            $('#selectLevel').prop('disabled', true);
        }
    }),
    selectPackage: $('#selectPackage').on("binded", function () {
        if ($('#selectPackage').UifSelect("getSelected") > 0) {
            $('#selectPackage').prop('disabled', true);
        }
    }),
    variables: {
    },
    FiltradoSuperior: (function () {
        if ($("#RuleBase_RuleBaseId").val() != -1) {
            $("#selectLevel").UifSelect({ source: rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + $("#RuleBase_PackageId").val() });
        }
        $("#selectPackage").on('itemSelected', function (event, selectedItem) {
            var controller = rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + selectedItem.Id;
            if (selectedItem.Id > 0) {
                $("#selectLevel").UifSelect({ source: controller });
            }
        });
        $("#selectLevel").on('itemSelected', function (event, selectedItem) {
            var controller = rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + selectedItem.Id;
            if (selectedItem.Id > 0) {
                $("#conditionSelect").UifSelect({ source: null });
                $("#actionSelect").UifSelect({ source: null });

                SearchCombo.ListaControlSearch('#conditionSelect', 315, null, selectedItem.Id);
                SearchCombo.ListaControlSearch('#actionSelect', 315, null, selectedItem.Id);
            }
        });
    }()),

    Condiciones: {
        CondicionModal: (function () {
            $("#btnCondicion").click(function () {
                $("#selectLevelScript").UifSelect('setSelected', "");
                $('#ModalHeadCondition').UifModal('showLocal', Resources.Language.LabelCondition);
            })
        }()),
        CondicionAdd: (function () {
            $("#btnSaveAddCondition").click(function () {
                var str = $("#conditionSelect").val()
                var res = str.split("/");
                var name = $("#conditionSelect option:selected").text()
                var dataCondicion = $("#ListCondition").UifListView('getData');

                var OrderNum = [];
                $.each(dataCondicion, function (index, value) {
                    OrderNum[index] = value.OrderNum;
                });

                OrderNum.sort();

                var orden = dataCondicion.length == 0 ? 1 : OrderNum[dataCondicion.length - 1] + 1;

                var existConcept = SearchCombo.GetIndexObjectsbyTwoKey(dataCondicion, 'ConceptId', Number(res[0]), "EntityId", Number(res[1]))
                if (existConcept == -1) {
                    $("#ListCondition").UifListView("addItem", { "ConceptId": res[0], "ConceptName": name, "Description": name, "EntityId": res[1], "OrderNum": orden });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RepeatedConcept, 'autoclose': true });
                }

            })
        }()),
        CondicionesLlenar: (function () {
            $.ajax({
                type: "GET",
                url: rootPath + "RulesScripts/DecisionTable/GetConditionConcept/" + $("#RuleBase_RuleBaseId").val()
            }).done(function (data) {
                if (data.success) {

                    $("#ListCondition").UifListView({
                        autoHeight: true,
                        theme: 'dark',
                        sourceData: data.result,
                        customDelete: false,
                        customEdit: true,
                        delete: true,
                        displayTemplate: "#template-RuleSetList",
                        selectionType: 'single',
                        deleteCallback: function (deferred) {
                            deferred.resolve();
                        }

                    });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });

        }()),
        CondicionSeleccionar: (function () {
        }()),
    },
    Acciones: {
        AccionModal: (function () {
            $("#btnAccion").click(function () {
                $("#selectLevelScript").UifSelect('setSelected', "");
                $('#ModalHeadAction').UifModal('showLocal', Resources.Language.Action);
            })
        }()),
        AccionAdd: (function () {
            $("#btnSaveAddAction").click(function () {
                var str = $("#actionSelect").val()
                var res = str.split("/");
                var name = $("#actionSelect option:selected").text()
                var dataAction = $("#ListAction").UifListView('getData');

                var OrderNum = [];
                $.each(dataAction, function (index, value) {
                    OrderNum[index] = value.OrderNum;
                });

                OrderNum.sort();
                var orden = dataAction.length == 0 ? 1 : OrderNum[dataAction.length - 1] + 1;

                var existConcept = SearchCombo.GetIndexObjectsbyTwoKey(dataAction, 'ConceptId', Number(res[0]), "EntityId", Number(res[1]))

                if (existConcept == -1) {
                    $("#ListAction").UifListView("addItem", { "ConceptId": res[0], "ConceptName": name, "Description": name, "EntityId": res[1], "OrderNum": orden });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RepeatedConcept, 'autoclose': true });
                }
            })
        }()),
        AccionesLlenar: (function () {
            $.ajax({
                type: "GET",
                url: rootPath + "RulesScripts/DecisionTable/GetActionConcept/" + $("#RuleBase_RuleBaseId").val()
            }).done(function (data) {
                if (data.success) {
                    $("#ListAction").UifListView({
                        autoHeight: true,
                        sourceData: data.result,
                        customDelete: false,
                        customEdit: true,
                        delete: true,
                        displayTemplate: "#template-RuleSetList",
                        selectionType: 'single',
                        deleteCallback: function (deferred) {
                            deferred.resolve();
                        }
                    });
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }()),
        AccionSeleccionar: (function () {
        }()),
    },
    Guardar: (function () {
        $("#btnRecord").click(function () {
            var dataAction = $("#ListAction").UifListView('getData');
            var dataCondicion = $("#ListCondition").UifListView('getData');
            var ruleBase = null;
            if ($("#RuleBase_RuleBaseId").val() != -1) {
                ruleBase = {
                    "Description": $("#RuleBase_Description").val(),
                    "LevelId": $("#selectLevel").val(),
                    "PackageId": $("#selectPackage").val(),
                    "RuleBaseId": $("#RuleBase_RuleBaseId").val()
                };
            } else {
                ruleBase = {
                    "Description": $("#RuleBase_Description").val(),
                    "LevelId": $("#selectLevel").val(),
                    "PackageId": $("#selectPackage").val(),
                    "RuleBaseId": null
                };
            }

            if ($("#RuleBase_Description").val() == "") {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EnterNameDecisionTable, 'autoclose': true });
            }
            else if ($("#selectPackage").val() == "") {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EnterModule, 'autoclose': true });
            }
            else if ($("#selectLevel").val() == null || $("#selectLevel").val() == "") {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EnterLevel, 'autoclose': true });
            }
            else if (dataCondicion.length == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EnterCondition, 'autoclose': true });
            }
            else if (dataAction.length == 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EnterAction, 'autoclose': true });
            }
            else {
                var ruleComposite = { "Change": null, "Actions": dataAction, "Conditions": dataCondicion };
                DecisionTable = { "RuleBase": ruleBase, "RuleComposite": ruleComposite }

                $.ajax({
                    type: "POST",
                    url: rootPath + "RulesScripts/DecisionTable/CreateTableDecision",
                    data: JSON.stringify({ decisionTable: DecisionTable }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        window.location.href = rootPath + "RulesScripts/DecisionTable/Index";
                    }
                });
            }
        });
    }()),

    Salir: (function () {
        $("#btnExit").click(function () {
            window.location.href = rootPath + "RulesScripts/DecisionTable/Index";
        });
    }()),
}










