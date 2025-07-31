$.ajaxSetup({ async: false });
var dropDownSearchScript;
var objAdvanceSearchScript = {
    Init: {
        Start: $(function () {
            objAdvanceSearchScript.Init.Components.AdvancedSearch.Form();
            objAdvanceSearchScript.Init.Components.Selects.Package();
            objAdvanceSearchScript.Init.Components.Selects.Level(0);
            objAdvanceSearchScript.Init.Components.Listview.Scripts();
            objAdvanceSearchScript.Init.Components.Autocomplete();

            objAdvanceSearchScript.Events.AdvancedSearch.Show();
            objAdvanceSearchScript.Events.AdvancedSearch.Search();
            objAdvanceSearchScript.Events.AdvancedSearch.OkButton();
            objAdvanceSearchScript.Events.AdvancedSearch.CancelButton();
            objAdvanceSearchScript.Events.Search.Script();
            objAdvanceSearchScript.Events.Selects.Package();
            objAdvanceSearchScript.Events.Selects.Level();
        }),

        //Inicializa los componentes
        Components: {
            AdvancedSearch: {
                Form: function () {
                    dropDownSearchScript = uif2.dropDown({
                        source: rootPath + "RulesScripts/Scripts/AdvancedSearchScript",
                        element: "#btnSearchAdvScript",
                        align: "right",
                        width: 600,
                        height: 551,
                        loadedCallback: function () { }
                    });
                },
                Search: function () {
                    $("#btnsearchScriptAdv").on("click", function () {
                        objAdvanceSearchScript.Functions.AdvancedSearch.Search();
                    });
                },
                OkButton: function () {
                    $("#btnOkSearchSript").on("click", function () {
                        objAdvanceSearchScript.Functions.AdvancedSearch.OkButton();
                    });
                },
                CancelButton: function () {
                    $("#btnCancelSearchScript").on("click", function () {
                        objAdvanceSearchScript.Functions.AdvancedSearch.CancelButton();
                    });
                },
            },
            Selects: {
                Package: function () {
                    $("#selectPackageFilter").UifSelect({ source: rootPath + "RulesScripts/Scripts/GetPackages" });
                },
                Level: function (IdModule) {
                    if (IdModule > 0) {
                        $("#selectLevelFilter").UifSelect(
                            {
                                source: rootPath + "Scripts/GetLevels?packageId=" + IdModule,
                                filter: true
                            });
                    }
                    else {
                        $("#selectLevelFilter").UifSelect({ source: "" });
                    }
                },
            },
            Listview: {
                Scripts: function (module, level, Name, Question) {
                    if (!module && !level && !Name && !Question) {
                        $("#lvSearchScript").UifListView({ source: null, height: 240, selectionType: "single" });
                    } else {
                        $("#lvSearchScript").UifListView(
                        {
                            selectionType: "single",
                            source: rootPath + "RulesScripts/Scripts/GetScriptByLevelId?module=" + module + "&level=" + level + "&Name=" + Name + "&Question=" + Question,
                            displayTemplate: "#template-Scripts",
                            height: 200
                        });
                    }
                },
                ScriptsBasic: function (data) {
                    $("#lvSearchScript").UifListView(
                       {
                           selectionType: "single",
                           sourceData: data,
                           displayTemplate: "#template-Scripts",
                           height: 240
                       });
                }
            },
            Autocomplete: function () {
                $("#txtNameScript").UifAutoComplete({});
                $("#txtQuestionScript").UifAutoComplete({});
            }
        }
    },

    //registra los eventos de los componentes
    Events: {
        AdvancedSearch: {
            Show: function () {
                $("#btnSearchAdvScript").on("click", function () {
                    objAdvanceSearchScript.Functions.AdvancedSearch.Show();
                });
            },
            Search: function () {
                $("#btnsearchScriptAdv").on("click", function () {
                    objAdvanceSearchScript.Functions.AdvancedSearch.Search();
                });
            },
            OkButton: function () {
                $("#btnOkSearchSript").on("click", function () {
                    objAdvanceSearchScript.Functions.AdvancedSearch.OkButton();
                });
            },
            CancelButton: function () {
                $("#btnCancelSearchScript").on("click", function () {
                    objAdvanceSearchScript.Functions.AdvancedSearch.CancelButton();
                });
            },
        },
        Selects: {
            Package: function () {
                $("#selectPackageFilter").on("itemSelected", function (event, selectedItem) {
                    objAdvanceSearchScript.Init.Components.Selects.Level(selectedItem.Id);
                    objAdvanceSearchScript.Init.Components.Listview.Scripts();
                });
            },
            Level: function () {
                $("#selectLevelFilter").on("itemSelected", function (event, selectedItem) {
                    objAdvanceSearchScript.Init.Components.Listview.Scripts();
                });
            }
        },
        Search: {
            Script: function () {
                $("#SearchScript").on("search", function (event, value) {
                    objAdvanceSearchScript.Functions.SearchScript(value);
                })
            }
        }
    },

    //Funcionalidad de la pantalla
    Functions: {
        AdvancedSearch: {
            ClearSearch: function () {
                objAdvanceSearchScript.Init.Components.Listview.Scripts();
                objAdvanceSearchScript.Init.Components.Selects.Package();
                objAdvanceSearchScript.Init.Components.Selects.Level(0);
                $("#txtNameScript").val("");
                $("#txtQuestionScript").val("");
                this.Hide()
            },
            Show: function () {
                dropDownSearchScript.show();
            },
            Hide: function () {
                dropDownSearchScript.hide();
            },
            Search: function () {
                var module = $("#selectPackageFilter").val();
                var level = $("#selectLevelFilter").val();
                var Name = $("#txtNameScript").val();
                var Question = $("#txtQuestionScript").val();

                if (!module && !level && !Name && !Question) {
                    $.UifNotify("show", { type: "info", message: Resources.Language.SelectAtLeastTwoSearchCriteria, autoclose: false });
                }
                else if ((!level && !Name && !Question) || (!module && !Name && !Question) ||
                         (!module && !level && !Question) || (!module && !level && !Name)) {
                    $.UifNotify("show", { type: "info", message: Resources.Language.SelectAtLeastTwoSearchCriteria, autoclose: false });
                }
                else {
                    objAdvanceSearchScript.Init.Components.Listview.Scripts(module, level, Name, Question);
                }
            },
            OkButton: function () {
                var object = $("#lvSearchScript").UifListView("getSelected");
                if (object.length > 0) {
                    ControlScript.Functions.Form.Assign(object[0]);
                }
                this.ClearSearch();
            },
            CancelButton: function () {
                this.ClearSearch();
            },
        },
        SearchScript: function (value) {
            if (value.length < 3) {
                $.UifNotify("show", { type: "info", message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
            } else {
                $.ajax({
                    type: "GET",
                    url: rootPath + "RulesScripts/Scripts/GetScriptsAutocomplete",
                    data: {
                        "query": value
                    },
                }).done(function (data) {
                    $("#SearchScript").val("");
                    if (data.length == 0) {
                        $.UifNotify("show", { type: "info", message: Resources.Language.NoItemsFound, autoclose: false });
                    }
                    else if (data.length == 1) {
                        ControlScript.Functions.Form.Assign(data[0]);
                    } else {
                        objAdvanceSearchScript.Functions.AdvancedSearch.Show();
                        objAdvanceSearchScript.Init.Components.Listview.ScriptsBasic(data);
                    }
                });
            }
        },
    },
}