$.ajaxSetup({ async: false });
var objAdvanceSearchDt = {
    Init: {
        Start: $(function () {
            objAdvanceSearchDt.Init.Components.AdvancedSearch.Form();
            objAdvanceSearchDt.Init.Components.Selects.Package();
            objAdvanceSearchDt.Init.Components.Selects.Level(0);
            objAdvanceSearchDt.Init.Components.Listview.DecisionTable();

            objAdvanceSearchDt.Events.AdvancedSearch.Show();
            objAdvanceSearchDt.Events.AdvancedSearch.Search();
            objAdvanceSearchDt.Events.AdvancedSearch.OkButton();
            objAdvanceSearchDt.Events.AdvancedSearch.CancelButton();
            objAdvanceSearchDt.Events.Search.DecisionTable();
            objAdvanceSearchDt.Events.Selects.Package();
            objAdvanceSearchDt.Events.Selects.Level();
            objAdvanceSearchDt.Events.Selects.Published();
        }),

        //Inicializa los componentes
        Components: {
            AdvancedSearch: {
                Form: function () {
                    dropDownSearchDt = uif2.dropDown({
                        source: rootPath + 'RulesScripts/DecisionTable/AdvancedSearchDt',
                        element: '#btnSearchAdvDecisionTable',
                        align: 'right',
                        width: 600,
                        height: 500,
                        loadedCallback: function () { }
                    });
                },
            },
            Selects: {
                Package: function () {
                    $("#selectPackage").UifSelect({ source: rootPath + 'RulesScripts/RuleSet/GetPackages' });
                },
                Level: function (IdModule) {
                    if (IdModule > 0) {
                        $("#selectLevel").UifSelect(
                            {
                                source: rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + IdModule,
                            });
                    }
                    else {
                        $("#selectLevel").UifSelect({ source: "" });
                    }
                },
            },
            Listview: {
                DecisionTable: function (module, level, Published) {
                    if (!module && !level) {
                        $("#lvSearchDesicionTable").UifListView({ source: null, height: 250, selectionType: 'single' });
                    } else {
                        $("#lvSearchDesicionTable").UifListView(
                        {
                            selectionType: 'single',
                            source: rootPath + "DecisionTable/GetDecisionTablelistByLevelId?module=" + module + "&level=" + level + "&Published=" + Published,
                            displayTemplate: "#template-DecisionTablelist",
                            height: 250
                        });
                    }
                },
                DecisionTableBasic: function (data) {
                    $("#lvSearchDesicionTable").UifListView(
                        {
                            selectionType: 'single',
                            sourceData: data,
                            displayTemplate: "#template-DecisionTablelist",
                            height: 250
                        });
                },
            }
        }
    },

    //registra los eventos de los componentes
    Events: {
        AdvancedSearch: {
            Show: function () {
                $("#btnSearchAdvDecisionTable").on("click", function () {
                    objAdvanceSearchDt.Functions.AdvancedSearch.Show();
                });
            },
            Search: function () {
                $("#btnsearchDtAdv").on("click", function () {
                    objAdvanceSearchDt.Functions.AdvancedSearch.Search();
                });
            },
            OkButton: function () {
                $("#btnOkSearchDt").on("click", function () {
                    objAdvanceSearchDt.Functions.AdvancedSearch.OkButton();
                });
            },
            CancelButton: function () {
                $("#btnCancelSearchDt").on("click", function () {
                    objAdvanceSearchDt.Functions.AdvancedSearch.CancelButton();
                });
            },
        },
        Selects: {
            Package: function () {
                $("#selectPackage").on("itemSelected", function (event, selectedItem) {
                    objAdvanceSearchDt.Init.Components.Selects.Level(selectedItem.Id);
                    objAdvanceSearchDt.Init.Components.Listview.DecisionTable();
                });
            },
            Level: function () {
                $("#selectLevel").on("itemSelected", function (event, selectedItem) {
                    objAdvanceSearchDt.Init.Components.Listview.DecisionTable();
                });
            },
            Published: function () {
                $("#selectPublished").on("change", function () {
                    objAdvanceSearchDt.Init.Components.Listview.DecisionTable();
                });
            },
        },
        Search: {
            DecisionTable: function () {
                $("#SearchDecisionTable").on("search", function (event, value) {
                    objAdvanceSearchDt.Functions.SearchDecisionTable(value);
                })
            }
        }
    },

    //Funcionalidad de la pantalla
    Functions: {
        AdvancedSearch: {
            ClearSearch: function () {
                objAdvanceSearchDt.Init.Components.Listview.DecisionTable();
                objAdvanceSearchDt.Init.Components.Selects.Package();
                objAdvanceSearchDt.Init.Components.Selects.Level(0);
                this.Hide()
            },
            Show: function () {
                dropDownSearchDt.show();
            },
            Hide: function () {
                dropDownSearchDt.hide();
            },
            Search: function () {
                var module = $("#selectPackage").val();
                var level = $("#selectLevel").val();
                var Published = $("#selectPublished").val();

                if (!module && !level) {
                    $.UifNotify('show', { type: 'info', message: Resources.Language.SelectAtLeastTwoSearchCriteria, autoclose: false });
                }
                else {
                    objAdvanceSearchDt.Init.Components.Listview.DecisionTable(module, level, Published);
                }
            },
            OkButton: function () {
                var object = $("#lvSearchDesicionTable").UifListView("getSelected");
                if (object) {
                    ControlTableDecision.DecisionTable.ListDecisionTable(object);
                }
                this.ClearSearch();
            },
            CancelButton: function () {
                this.ClearSearch();
            },
        },
        SearchDecisionTable: function (value) {
            if (value.length == 0) {
                ControlTableDecision.DecisionTable.ListDecisionTable();
            }
            else if (value.length < 3) {
                $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
            } else {
                $.ajax({
                    type: "GET",
                    url: rootPath + "RulesScripts/DecisionTable/GetDecisionTableByDescription",
                    data: {
                        "Description": value
                    },
                }).done(function (data) {
                    $("#SearchDecisionTable").val("");
                    if (data.length == 0) {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.NoItemsFound, autoclose: false });
                    }
                    else if (data.length == 1) {
                        ControlTableDecision.DecisionTable.ListDecisionTable(data);
                    } else {
                        objAdvanceSearchDt.Functions.AdvancedSearch.Show();
                        objAdvanceSearchDt.Init.Components.Listview.DecisionTableBasic(data);
                    }
                });
            }
        },
    }
}