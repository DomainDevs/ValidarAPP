$.ajaxSetup({ async: false });
var objAdvConcepts = {
    Init: {
        Start: $(function () {
            objAdvConcepts.Init.Components.AdvancedSearch.Form();
            objAdvConcepts.Init.Components.Selects.Module();
            objAdvConcepts.Init.Components.Selects.Level(1, true);
            objAdvConcepts.Init.Components.Autocomplete.DescriptionConcept();
            objAdvConcepts.Init.Components.Listview.Concepts();

            objAdvConcepts.Events.AdvancedSearch.Show();
            objAdvConcepts.Events.AdvancedSearch.Search();
            objAdvConcepts.Events.AdvancedSearch.OkButton();
            objAdvConcepts.Events.AdvancedSearch.CancelButton();
            objAdvConcepts.Events.Autocomplete.DescriptionConcept();
            objAdvConcepts.Events.Selects.Module();
            objAdvConcepts.Events.Selects.Level();
            objAdvConcepts.Events.Selects.Filter();
            objAdvConcepts.Events.Search.Concept();
        }),

        //Inicializa los componentes
        Components: {
            AdvancedSearch: {
                Form: function () {
                    dropDownSearchConcepts = uif2.dropDown({
                        source: rootPath + 'RulesScripts/Concepts/AdvancedSearchConcepts',
                        element: '#btnSearchAdvConcepts',
                        align: 'right',
                        width: 600,
                        height: 551,
                        loadedCallback: function () { }
                    });
                },
            },
            Selects: {
                Module: function () {
                    $("#ddlModuleAdv").UifSelect({ source: rootPath + 'RulesScripts/RuleSet/GetPackages' });
                },
                Level: function (IdModule, isInit) {
                    if (IdModule > 0) {
                        $("#ddlLevelAdv").UifSelect(
                            {
                                source: rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + IdModule,
                                selectedId: isInit ? 1 : null
                            });
                    }
                    else {
                        $("#ddlLevelAdv").UifSelect({ source: "" });
                    }
                },
            },
            Autocomplete: {
                DescriptionConcept: function () {
                    $("#txtDescriptionAdv").UifAutoComplete({
                        source: rootPath + "RulesScripts/Concepts/GetConceptsAutocomplete",
                        displayKey: "Description"
                    });
                },
            },
            Listview: {
                Concepts: function (module, level, filter, description) {
                    if (!module && !level && !filter && !description) {
                        $("#lvSearchConcepts").UifListView({ source: null, height: 240, selectionType: 'single' });
                    } else {
                        $("#lvSearchConcepts").UifListView(
                        {
                            selectionType: 'single',
                            source: rootPath + "RulesScripts/Concepts/GetConceptsByIdModuleIdLevelDescription?IdModule=" + module + "&IdLevel=" + level + "&Filter=" + filter + "&Description=" + description,
                            displayTemplate: "#display-template-concepts",
                            height: 240
                        });
                    }
                },
                ConceptsBasic: function (data) {
                    $("#lvSearchConcepts").UifListView(
                     {
                         selectionType: 'single',
                         sourceData: data,
                         displayTemplate: "#display-template-concepts",
                         height: 240

                     });
                },
            },
        },
    },

    //registra los eventos de los componentes
    Events: {
        AdvancedSearch: {
            Show: function () {
                $("#btnSearchAdvConcepts").on("click", function () {
                    objAdvConcepts.Functions.AdvancedSearch.Show();
                });
            },
            Search: function () {
                $("#btnsearchConceptAdv").on("click", function () {
                    objAdvConcepts.Functions.AdvancedSearch.Search();
                });
            },
            OkButton: function () {
                $("#btnOkSearchConcept").on("click", function () {
                    objAdvConcepts.Functions.AdvancedSearch.OkButton();
                });
            },
            CancelButton: function () {
                $("#btnCancelSearchConcept").on("click", function () {
                    objAdvConcepts.Functions.AdvancedSearch.CancelButton();
                });
            },
        },
        Selects: {
            Module: function () {
                $("#ddlModuleAdv").on("itemSelected", function (event, selectedItem) {
                    objAdvConcepts.Init.Components.Selects.Level(selectedItem.Id, false);
                    objAdvConcepts.Init.Components.Listview.Concepts();
                });
            },
            Level: function () {
                $("#ddlLevelAdv").on("itemSelected", function (event, selectedItem) {
                    objAdvConcepts.Init.Components.Listview.Concepts();
                });
            },
            Filter: function () {
                $("#dllTypeAdv").on("change", function () {
                    objAdvConcepts.Init.Components.Listview.Concepts();
                });
            },
        },
        Autocomplete: {
            DescriptionConcept: function () {
                $("#txtDescriptionAdv").on('itemSelected', function (event, selectedItem) {
                    objAdvConcepts.Init.Components.Listview.Concepts();
                });
            },
        },
        Search: {
            Concept: function () {
                $("#SearchConcept").on("search", function (event, value) {
                    objAdvConcepts.Functions.SearchConcept(value);
                })
            }
        },
    },

    //Funcionalidad de la pantalla
    Functions: {
        AdvancedSearch: {
            ClearSearch: function () {
                objAdvConcepts.Init.Components.Listview.Concepts();
                objAdvConcepts.Init.Components.Selects.Module();
                objAdvConcepts.Init.Components.Selects.Level(1, true);
                $("#dllTypeAdv").val(0);
                $("#txtDescriptionAdv").val("");
                this.Hide()
            },
            Show: function () {
                dropDownSearchConcepts.show();
            },
            Hide: function () {
                dropDownSearchConcepts.hide();
            },
            Search: function () {
                var module = $("#ddlModuleAdv").val();
                var level = $("#ddlLevelAdv").val();
                var filter = $("#dllTypeAdv").val();
                var description = $("#txtDescriptionAdv").val();

                if (!module && !level && !description) {
                    $.UifNotify('show', { type: 'info', message: Resources.Language.SelectAtLeastTwoSearchCriteria, autoclose: false });
                }
                else {
                    objAdvConcepts.Init.Components.Listview.Concepts(module, level, filter, description);
                }
            },
            OkButton: function () {
                var object = $("#lvSearchConcepts").UifListView("getSelected");
                if (object) {
                
                    $.each(object, function (key, value) {
                        var lista = $("#lvConcepts").UifListView('getData')
                        var index = lista.findIndex(function (item) {
                            return item.ConceptId == value.ConceptId;

                        });
                        objConcepts.Functions.Form.Assign(object[0], index);
                    });                    
                }
                this.ClearSearch();
            },
            CancelButton: function () {
                this.ClearSearch();
            },
        },
        SearchConcept: function (value) {
            if (value.length < 3) {
                $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
            } else {
                $.ajax({
                    type: "GET",
                    url: rootPath + "RulesScripts/Concepts/GetConceptsByIdModuleIdLevelDescription",
                    data: {
                        "Filter":0,
                        "Description": value
                    },
                }).done(function (data) {
                    $("#SearchConcept").val("");
                    if (data.length == 0) {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.NoItemsFound, autoclose: false });
                    }
                    else if (data.length == 1) {
                        
                        $.each(data, function (key, value) {
                            var lista = $("#lvConcepts").UifListView('getData')
                            var index = lista.findIndex(function (item) {
                                return item.ConceptId == value.ConceptId;
                            });
                            objConcepts.Functions.Form.Assign(data[0], index);
                        })
                        
                    } else {
                        objAdvConcepts.Functions.AdvancedSearch.Show();
                        objAdvConcepts.Init.Components.Listview.ConceptsBasic(data);
                    }
                });
            }
        },
    },
}