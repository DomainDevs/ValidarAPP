$.ajaxSetup({ async: false });
ControlRuleSet = {
    variables: {
        idRuleEdit: null,
    },

    PaquetesRelgas: {
        PagetesReglasAdd: (function () {
            $("#RuleSetList").on('rowAdd', function (event, data, position) {
                $('#ModalRuleSet').UifModal('showLocal', Resources.Language.AddRulesPack);
            })

            $("#selectPackageRuleSet").on('itemSelected', function (event, selectedItem) {
                var controller = rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + selectedItem.Id;
                if (selectedItem.Id > 0) {
                    $("#selectLevelRuleSet").UifSelect({ source: controller });
                }
            })

            $("#addRuleSet").click(function () {
                RuleSetComposite = new oRuleSetComposite();
                Level = new oLevel();
                Level.Description = $('#selectLevelRuleSet').UifSelect("getSelectedText")
                Level.PackageId = $('#selectPackageRuleSet').UifSelect("getSelected")
                Level.LevelId = $('#selectLevelRuleSet').UifSelect("getSelected")

                Package = new oPackage();
                Package.Description = $('#selectPackageRuleSet').UifSelect("getSelectedText")
                Package.PackageId = $('#selectPackageRuleSet').UifSelect("getSelected")

                RuleSet = new oRuleSet();
                RuleSet.RuleSetId = 0 //correccion o preguntrar si existe alguna regla con 0 debe guardar primero la regla
                RuleSet.PackageId = $('#selectPackageRuleSet').UifSelect("getSelected")
                RuleSet.LevelId = $('#selectLevelRuleSet').UifSelect("getSelected")
                RuleSet.Description = $('#nameRuleSet').val()
                RuleSet.Level = Level
                RuleSet.IsEvent = getQueryVariable("IsEvent") ? true : false;
                RuleSet.RuleSetVer = 0
                RuleSet.CurrentFrom = "11/11/2015"
                RuleSet.Package = Package
                RuleSetComposite.RuleSet = RuleSet;

                RuleSetComposite.RuleComposites = []

                $("#RuleSetList").UifListView("addItem", RuleSet);
                $('#ModalRuleSet').UifModal('hide');
            })

            $("#btnSearchAdvRules").click(function () {
                ObjectAdvanceSearchRules.dropDownRules.show();
            });

            $("#btnExitRuleSet").click(function () {
                window.location = rootPath + "Home/Index";
            });
        }()),
        PagetesReglasLlenar: (function () {
            RuleSetComposite = new oRuleSetComposite();
            var IsEvent = getQueryVariable("IsEvent") ? true : false;
            $("#RuleSetList").UifListView({
                source: rootPath + "RulesScripts/RuleSet/GetAllRuleSets?IsEvent=" + IsEvent,
                customEdit: true,
                customAdd: true,
                edit: true,
                delete: true,
                displayTemplate: "#template-RuleSetList",
                selectionType: 'single',
                title: Resources.Language.LabelRuleSet,
                add: true,
                height: 415,
                deleteCallback: function (deferred, data) {
                    if (data.RuleSetId !== 0) {
                        $.ajax({
                            type: "GET",
                            data: {
                                "ruleSetId": data.RuleSetId
                            },
                            url: rootPath + "RulesScripts/RuleSet/DeleteRuleSet"
                        }).done(function (result) {
                            if (result.success) {
                                $.ajax({
                                    type: "POST",
                                    url: rootPath + "RulesScripts/RuleSet/GetAllRuleSets",
                                    data: {
                                        "IsEvent": false
                                    },
                                }).done(function (data) {
                                    ControlRuleSet.fillRulePack(data);
                                });
                                $.UifNotify('show', {
                                    'type': 'success', 'message': result.result, 'autoclose': true
                                });
                                ControlRuleSet.PaquetesRelgas.PagetesReglasLimpiar();
                                deferred.resolve();
                            } else {
                                $.UifNotify('show', {
                                    'type': 'danger', 'message': result.result, 'autoclose': true
                                });
                            }
                        });
                    } else {
                        deferred.resolve();
                    }
                }
            });

            $("#RulesList").UifListView({
                source: null,
                title: Resources.Language.Rules,
                height: 415,
                drag: true
            });

            $("#ActionList").UifListView({
                autoHeight: true,

                source: null,
                height: 182,
                title: Resources.Language.Actions
            });

            $("#ConditionList").UifListView({
                autoHeight: true,

                source: null,
                height: 181,
                title: Resources.Language.LabelConditions
            });
        }()),
        PagetesReglasEditar: (function () {
            $.ajaxSetup({ async: false });

            $('#RuleSetList').on('rowEdit', function (event, data) {
                ControlRuleSet.fillRulePack([data]);

                ControlRuleSet.LimpiarCondicionesAcciones()

                ControlRuleSet.PaquetesRelgas.PagetesReglasLimpiar()

                RuleSet = new oRuleSet();
                RuleSet = data;

                if (RuleSet.RuleSetId != 0) {
                    $.ajax({
                        type: "POST",
                        url: rootPath + "RuleSet/GetRuleSetComposite?ruleSetId=" + data.RuleSetId,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            RuleSetComposite = new oRuleSetComposite();
                            RuleSetComposite = data;
                            RuleSetComposite.RuleSet = RuleSet;
                        },
                        error: function (xhr, status) {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SorryAProblem, 'autoclose': true });
                        },
                        complete: function (xhr, status) {
                        }
                    })

                }
                ControlRuleSet.Relgas.ReglaLlenar()
            })
        }()),
        PagetesReglasLimpiar: function () {

            $("#RulesList").UifListView({
                source: null,
                title: Resources.Language.LabelRules,
                height: 415
            });

            $("#ActionList").UifListView({
                autoHeight: true,
                source: null,
                height: 182,
                title: Resources.Language.Actions
            });

            $("#ConditionList").UifListView({
                autoHeight: true,
                source: null,
                height: 181,
                title: Resources.Language.LabelConditions
            });
        },
    },

    Relgas: {
        ReglaAdd: (function () {
            $("#RulesList").on('rowAdd', function (event, data, position) {
                $('#ModalRule').UifModal('showLocal', Resources.Language.AddRule);
            })

            $("#addRule").click(function () {
                RuleComposite = new oRuleComposite()
                RuleComposite.RuleId = 0//importante que ponga el mayor mas uno de los que estan en el arreglo
                RuleComposite.RuleName = $('#nameRule').val()

                if (!RuleSetComposite.RuleComposites)
                    RuleSetComposite.RuleComposites = [];

                RuleSetComposite.RuleComposites.push(RuleComposite)

                $("#RulesList").UifListView("addItem", RuleComposite);

                $('#ModalRule').UifModal('hide');
                $('#nameRule').val(null)
            })
        }()),
        ReglaLlenar: function () {
            $("#RulesList").UifListView({
                sourceData: RuleSetComposite.RuleComposites,
                customEdit: true,
                customAdd: true,
                edit: true,
                add: true,
                delete: true,
                displayTemplate: "#template-RulesList",
                selectionType: 'single',
                title: Resources.Language.LabelRules,
                height: 415,
                deleteCallback: ControlRuleSet.Relgas.ReglaEliminar
            });
        },
        ReglaEliminar: function (defered, data) {
            RuleSetComposite.RuleComposites.forEach(function (item, index) {
                if (data.RuleName === item.RuleName) {
                    $("#ActionList").UifListView({
                        autoHeight: true,
                        source: null,
                        height: 182,
                        title: Resources.Language.Actions
                    });

                    $("#ConditionList").UifListView({
                        autoHeight: true,
                        source: null,
                        height: 181,
                        title: Resources.Language.LabelConditions
                    });
                    RuleSetComposite.RuleComposites.splice(index, 1);
                    defered.resolve();
                    return;
                }
            });

        },
        ReglaLlenarEditar: (function () {
            $('#RulesList').on('rowEdit', function (event, data, position) {
                ControlRuleSet.LimpiarCondicionesAcciones()
                positionRuleEdit = position;
                ControlRuleSet.variables.idRuleEdit = data.RuleId;

                if (SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', data.RuleId) != -1) {
                    RuleComposite = new oRuleComposite()
                    RuleComposite = data;
                }
                ControlRuleSet.ActionLlenar()
                ControlRuleSet.ConditionLlenar()
            })
        }()),
    },
    ActionLlenar: function () {
        if (typeof RuleComposite != 'undefined') {
            $("#ActionList").UifListView({
                autoHeight: true,
                sourceData: RuleComposite.Actions,
                customDelete: false,
                customEdit: true,
                customAdd: true,
                add: true,
                edit: true,
                delete: true,
                displayTemplate: "#template-Action",
                selectionType: 'single',
                deleteCallback: ControlAction.ABMAccion.EliminarAccion,
                height: 182,
                title: Resources.Language.Actions
            });
        }

    },
    ConditionLlenar: function () {
        if (typeof RuleComposite != 'undefined') {
            $("#ConditionList").UifListView({
                autoHeight: true,
                sourceData: RuleComposite.Conditions,
                customDelete: false,
                customEdit: true,
                customAdd: true,
                edit: true,
                delete: true,
                add: true,
                displayTemplate: "#template-Condition",
                selectionType: 'single',
                deleteCallback: ControlCondition.ABMCondicion.EliminarCondicion,
                height: 181,
                title: Resources.Language.LabelConditions
            });
        }
    },
    LimpiarCondicionesAcciones: function () {
        $("#contentEditAction").UifModal("hide");
        ControlCondition.ControlesClean.cleanControlBasicCondition()
        ControlCondition.ControlesClean.cleanControlEditorCondition()
        $("#contentEditCondition").UifModal("hide");
        ControlAction.ControlesClean.cleanControlBasicAction()
    },
    Guardar: (function () {
        $("#btnRecordRule").click(function () {
            $.ajax({
                type: "POST",
                url: rootPath + "RuleSet/CreateRuleSet",
                data: JSON.stringify({ ruleSetComposite: RuleSetComposite }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        ControlRuleSet.PaquetesRelgas.PagetesReglasLimpiar();
                        $.UifNotify('show', {
                            'type': 'success', 'message': Resources.Language.PublishedCorrectly, 'autoclose': true
                        });
                        var IsEvent = getQueryVariable("IsEvent") ? true : false;
                        $("#RuleSetList").UifListView({
                            source: rootPath + "RulesScripts/RuleSet/GetAllRuleSets?IsEvent=" + IsEvent,

                            customEdit: true,
                            customAdd: true,
                            edit: true,
                            delete: true,
                            displayTemplate: "#template-RuleSetList",
                            selectionType: 'single',
                            title: Resources.Language.LabelRuleSet,
                            add: true,
                            height: 415,
                            deleteCallback: function (deferred, data) {
  
                                if (data.RuleSetId !== 0) {
                                    $.ajax({
                                        type: "GET",
                                        data: {
                                            "ruleSetId": data.RuleSetId
                                        },
                                        url: rootPath + "RulesScripts/RuleSet/DeleteRuleSet"
                                    }).done(function (result) {
                                        if (result.success) {
                                            $.ajax({
                                                type: "POST",
                                                url: rootPath + "RulesScripts/RuleSet/GetAllRuleSets",
                                                data: {
                                                    "IsEvent": false
                                                },
                                            }).done(function (data) {
                                                ControlRuleSet.fillRulePack(data);
                                            });
                                            $.UifNotify('show', {
                                                'type': 'success', 'message': result.result, 'autoclose': true
                                            });
                                            ControlRuleSet.PaquetesRelgas.PagetesReglasLimpiar();
                                            deferred.resolve();
                                        } else {
                                            $.UifNotify('show', {
                                                'type': 'danger', 'message': result.result, 'autoclose': true
                                            });
                                        }
                                    });
                                } else {
                                    deferred.resolve();
                                }
                            }
                        });
                    } else {
                        $.UifNotify('show', {
                            'type': 'danger', 'message': Resources.Language.SorryAProblem, 'autoclose': true
                        });
                    }
                },
                error: function (xhr, status) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.SorryAProblem, 'autoclose': true
                    });
                },
            });
        })
    }()),
    fillRulePack: function (dataRuleSelected) {
        ControlRuleSet.LimpiarCondicionesAcciones();
        ControlRuleSet.PaquetesRelgas.PagetesReglasLimpiar();
        $("#RuleSetList").UifListView({
            sourceData: dataRuleSelected,

            customEdit: true,
            customAdd: true,
            edit: true,
            delete: true,
            displayTemplate: "#template-RuleSetList",
            selectionType: 'single',
            title: Resources.Language.LabelRuleSet,
            add: true,
            height: 415,
            deleteCallback: function (deferred, data) {
                if (data.RuleSetId !== 0) {
                    $.ajax({
                        type: "GET",
                        data: {
                            "ruleSetId": data.RuleSetId
                        },
                        url: rootPath + "RulesScripts/RuleSet/DeleteRuleSet"
                    }).done(function (result) {
                        if (result.success) {
                            $.ajax({
                                type: "POST",
                                url: rootPath + "RulesScripts/RuleSet/GetAllRuleSets",
                                data: {
                                    "IsEvent": false
                                },
                            }).done(function (data) {
                                ControlRuleSet.fillRulePack(data);
                            });
                            $.UifNotify('show', {
                                'type': 'success', 'message': result.result, 'autoclose': true
                            });
                            ControlRuleSet.PaquetesRelgas.PagetesReglasLimpiar();
                            deferred.resolve();
                        } else {
                            $.UifNotify('show', {
                                'type': 'danger', 'message': result.result, 'autoclose': true
                            });
                        }
                    });
                } else {
                    deferred.resolve();
                }
            }
        });
    },
}


function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
}
