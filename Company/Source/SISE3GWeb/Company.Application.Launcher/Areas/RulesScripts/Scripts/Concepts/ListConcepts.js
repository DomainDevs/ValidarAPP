$.ajaxSetup({ async: false });
var conceptsAction = [];
var objConcepts = {
    Init: {
        Start: $(function () {
            objConcepts.Init.Components.CreateObjectToSave();

            objConcepts.Init.Components.Listview.Concepts();

            objConcepts.Init.Components.Selects.Module();
            objConcepts.Init.Components.Selects.Level(1, true);
            objConcepts.Init.Components.Selects.Entity();
            objConcepts.Init.Components.Selects.ConceptType();
            objConcepts.Init.Components.Selects.ConceptControl.Basic();
            objConcepts.Init.Components.Selects.ConceptControl.List();
            objConcepts.Init.Components.Selects.ConceptControl.Range();
            objConcepts.Init.Components.Selects.ConceptControl.Reference(1, 1);
            objConcepts.Init.Components.CheckBox();

            objConcepts.Events.Listview.Concepts.Add();
            objConcepts.Events.Listview.ListModal.AddEdit();
            objConcepts.Events.Listview.RangeModal.AddEdit();
            objConcepts.Events.Buttons.Return();
            objConcepts.Events.Buttons.Export();
            objConcepts.Events.Buttons.ClearForm();
            objConcepts.Events.Buttons.SaveForm();
            objConcepts.Events.Buttons.SaveConcepts();
            objConcepts.Events.Buttons.AddEditList();
            objConcepts.Events.Buttons.AddEditRange();
            objConcepts.Events.Buttons.Question();
            objConcepts.Events.Selects.Module();
            objConcepts.Events.Selects.Level();
            objConcepts.Events.Selects.ConceptType();
            objConcepts.Events.Selects.ConceptControl.Basic();
            objConcepts.Events.Modal.List.Accept();
            objConcepts.Events.Modal.Range.Accept();
            objConcepts.Events.Modal.Question.Accept();
            objConcepts.Events.Modal.Question.Reject();
            objConcepts.Events.Inputs.FocusoutConcept();
            objConcepts.Functions.PartialView.Hidden();
        }),
        //Inicializa los componentes
        Components: {
            CreateObjectToSave: function () {
                ConceptsToSave = {
                    ConceptBasicAdd: [],
                    ConceptBasicEdit: [],
                    ConceptBasicDelete: [],

                    ConceptListAdd: [],
                    ConceptListEdit: [],
                    ConceptListDelete: [],

                    ConceptRangeAdd: [],
                    ConceptRangeEdit: [],
                    ConceptRangeDelete: [],

                    ConceptReferenceAdd: [],
                    ConceptReferenceEdit: [],
                    ConceptReferenceDelete: [],
                };

            },
            Listview: {
                Concepts: function () {
                },
                PreviewList: {
                    AddEdit: function (item) {
                        if (item) {
                            $.ajax({
                                type: "GET",
                                url: rootPath + "RulesScripts/Concepts/GetListEntityValueByListEntityCode",
                                data: { listEntityCode: item },
                                dataType: "json",
                            }).done(function (data) {
                                $("#txtListName").val($("#ddlLists").UifSelect("getSelectedText"));
                                $("#lVModalList").UifListView(
                                {
                                    sourceData: data[0].ListEntityValue,
                                    displayTemplate: "#listEntityValueTemplate",
                                    height: 350
                                });
                            });
                        } else {
                            $("#lVModalList").UifListView(
                            {
                                sourceData: null,
                                displayTemplate: "#listEntityValueTemplate",
                                height: 350,
                                edit: true,
                                delete: true,
                                customEdit: true,
                                deleteCallback: function (deferred, data) { deferred.resolve(); },
                            });
                        }
                    },
                },
                PreviewRange: {
                    AddEdit: function (item) {
                        if (item) {
                            $.ajax({
                                type: "GET",
                                url: rootPath + "RulesScripts/Concepts/GetRangeEntityValueByRangeEntityCode",
                                data: { rangeEntityCode: item },
                                dataType: "json",
                            }).done(function (data) {
                                $("#txtRangeName").val($("#ddlRanges").UifSelect("getSelectedText"));
                                $("#lVModalRanges").UifListView(
                                {
                                    sourceData: data[0].RangeEntityValue,
                                    displayTemplate: "#RangeEntityValueTemplate",
                                    height: 350
                                });
                            });
                        } else {
                            $("#lVModalRanges").UifListView(
                            {
                                sourceData: null,
                                displayTemplate: "#RangeEntityValueTemplate",
                                height: 350,
                                edit: true,
                                delete: true,
                                customEdit: true,
                                deleteCallback: function (deferred, data) { deferred.resolve(); },
                            });
                        }
                    },
                },
            },
            Selects: {
                Module: function () {
                    $("#ddlModule").UifSelect({ source: rootPath + 'RulesScripts/RuleSet/GetPackages' });
                },
                Level: function (IdModule, isInit) {
                    if (IdModule > 0) {
                        $("#ddlLevel").UifSelect(
                            {
                                source: rootPath + "RulesScripts/RuleSet/GetLevels?packageId=" + IdModule,
                                selectedId: isInit ? 1 : null
                            });
                    }
                    else {
                        $("#ddlLevel").UifSelect({ source: "" });
                    }
                },
                Entity: function () {
                    var level = $("#ddlLevel").val();
                    if (level) {
                        var str = $($("#ddlLevel option[value = " + level + "]")).text();
                        $($("#ddlEntity").children()[0]).text("Facade " + str);
                    }
                    else {
                        $($("#ddlEntity").children()[0]).text("");
                    }
                },
                ConceptType: function () {
                    $("#ConceptTypeCode").UifSelect({ source: rootPath + "RulesScripts/Concepts/GetConceptTypes" });
                },
                ConceptControl: {
                    Basic: function () {
                        $("#ConceptControlCodeBasic").UifSelect({ source: rootPath + "RulesScripts/Concepts/GetBasicTypes" });
                    },
                    List: function () {
                        $("#ddlLists").UifSelect({ source: rootPath + "RulesScripts/Concepts/GetListEntitySelect" });
                    },
                    Range: function () {
                        $("#ddlRanges").UifSelect({ source: rootPath + "RulesScripts/Concepts/GetRangeEntitySelect" });
                    },
                    Reference: function (module, level) {
                        if (module && level) {
                            $("#ddlReferences").UifSelect({
                                source: rootPath + "RulesScripts/Concepts/GetReferencesEntitySelect?packageId=" + module + "&levelId=" + level,
                                native: false,
                                filter: true
                            });
                        }
                        else {
                            $("#ddlReferences").UifSelect({ sourceData: null });
                        }
                    },
                },
            },
            CheckBox: function () {
                $($("input[name='IsVisible'")[1]).remove();
                $($("input[name='IsNull'")[1]).remove();
                $($("input[name='IsPersistible'")[1]).remove();
            },
        },
    },

    //registra los eventos de los componentes
    Events: {
        Selects: {
            Module: function () {
                $("#ddlModule").on("itemSelected", function (event, selectedItem) {
                    if (!selectedItem) {
                        selectedItem = { Id: $("#ddlModule").val() };
                    }
                    objConcepts.Init.Components.Selects.Level(selectedItem.Id, false);
                    objConcepts.Init.Components.Selects.ConceptControl.Reference(selectedItem.Id, null);
                });
            },
            Level: function () {
                $("#ddlLevel").on("itemSelected", function (event, selectedItem) {
                    if (!selectedItem) {
                        selectedItem = { Id: $("#ddlLevel").val() };
                    }
                    objConcepts.Init.Components.Selects.Entity();
                    objConcepts.Init.Components.Selects.ConceptControl.Reference($("#ddlModule").val(), selectedItem.Id);
                });
            },
            ConceptType: function () {
                $("#ConceptTypeCode").on("itemSelected", function (event, selectedItem) {
                    if (!selectedItem) {
                        selectedItem = { Id: $("#ConceptTypeCode").val() };
                    }
                    objConcepts.Functions.PartialView.ViewTypeConcept(selectedItem.Id);
                });
            },
            ConceptControl: {
                Basic: function () {
                    $("#ConceptControlCodeBasic").on("itemSelected", function (event, selectedItem) {
                        if (!selectedItem) {
                            selectedItem = { Id: $("#ConceptControlCodeBasic").val() };
                        }
                        objConcepts.Functions.PartialView.ViewBasicType(selectedItem.Id);
                    });
                },
            },
        },
        Listview: {
            Concepts: {
                Add: function () {
                    $('#btnConceptAdd').on('click', function () {
                        objConcepts.Functions.Listview.Concepts.Add();
                    });
                },
            },
            ListModal: {
                AddEdit: function () {
                    $("#btnClearListConcept").on("click", function () {
                        objConcepts.Functions.Modal.ModalList.Clear();
                    });
                    $("#btnAddListConcept").on("click", function () {
                        if (!$("#hdnConceptListIndex").val()) {
                            objConcepts.Functions.Listview.ModalList.Add();
                        } else {
                            objConcepts.Functions.Listview.ModalList.Edit();
                        }
                    });
                    $('#lVModalList').on('rowEdit', function (event, data, index) {
                        $("#hdnConceptListIndex").val(index);
                        $("#txtListValue").val(data.ListValue);
                    });
                }
            },
            RangeModal: {
                AddEdit: function () {
                    $("#btnClearRangesConcept").on("click", function () {
                        objConcepts.Functions.Modal.ModalRange.Clear();
                    });
                    $("#btnAddRangesConcept").on("click", function () {
                        if (!$("#hdnConceptRangeIndex").val()) {
                            objConcepts.Functions.Listview.ModalRange.Add();
                        } else {
                            objConcepts.Functions.Listview.ModalRange.Edit();
                        }
                    });
                    $('#lVModalRanges').on('rowEdit', function (event, data, index) {
                        $("#hdnConceptRangeIndex").val(index);
                        $("#txtRangeFrom").val(data.FromValue);
                        $("#txtRangeTo").val(data.ToValue);
                    });
                }
            },
        },
        Buttons: {
            Return: function () {
                $("#btnExit").on("click", function () {
                    objConcepts.Functions.Exit();
                });
            },
            Export: function () {
                $("#btnExportConcepts").on("click", function () {
                    objConcepts.Functions.ExportConcepts();
                });
            },

            SaveConcepts: function () {
                $("#btnSaveConcepts").on("click", function () {
                    objConcepts.Functions.SaveConcepts();
                });
            },

            ClearForm: function () {
                $("#btnClearForm").on("click", function () {
                    objConcepts.Functions.Form.Clear();
                });
            },
            SaveForm: function () {
                $("#btnSaveForm").on("click", function () {
                    objConcepts.Functions.Form.Save();
                });
            },

            AddEditList: function () {
                $("#btnAddList").on("click", function () {
                    objConcepts.Functions.Modal.ModalList.Clear();
                    objConcepts.Functions.Modal.ModalList.Open("Lista de valores");
                    objConcepts.Init.Components.Listview.PreviewList.AddEdit($("#ddlLists").UifSelect("getSelected"));
                });
            },
            AddEditRange: function () {
                $("#btnAddRange").on("click", function () {
                    objConcepts.Functions.Modal.ModalRange.Clear();
                    objConcepts.Functions.Modal.ModalRange.Open("Rango de valores");
                    objConcepts.Init.Components.Listview.PreviewRange.AddEdit($("#ddlRanges").UifSelect("getSelected"));
                });
            },

            Question: function () {
                $("#btnQuestion").on("click", function () {
                    objConcepts.Functions.Modal.ModalQuestion.Open("Etiqueta para guiones");
                });
            },
        },
        Modal: {
            Question: {
                Accept: function () {
                    $("#btnSaveQuestion").on("click", function () {
                        $("#QuestionDescriptionTmp").val($("#QuestionDescription").val());
                    });

                },
                Reject: function () {
                    $("#btnCloseQuestion").on("click", function () {
                        $("#QuestionDescription").val($("#QuestionDescriptionTmp").val());
                    });
                },
            },
            List: {
                Accept: function () {
                    $("#btnModalListOk").on("click", function () {
                        objConcepts.Functions.Listview.ModalList.Save($("#ddlLists").UifSelect("getSelected"));
                    });
                },
            },
            Range: {
                Accept: function () {
                    $("#btnModalRangeOk").on("click", function () {
                        objConcepts.Functions.Listview.ModalRange.Save($("#ddlRanges").UifSelect("getSelected"));
                    });
                },
            },
        },
        Inputs: {
            FocusoutConcept: function () {
                $("#Description").focusout( function () {
                    objConcepts.Functions.GetConceptsByDescription();
                });
            },
        }
    },

    //Funcionalidad de la pantalla
    Functions: {
        Listview: {
            Concepts: {
                Edit: function (data, index) {
                    $("#hdnConceptIndex").val(index);
                    objConcepts.Functions.Form.Assign(data);
                },
                Delete: function (deferred, data) {
                    if (!data.IsStatic) {
                        if (data.ConceptId == 0) {
                            deferred.resolve();
                            objConcepts.Functions.Form.Clear();
                        } else {
                            $.ajax({
                                url: rootPath + "RulesScripts/Concepts/IsInUse",
                                data: {
                                    "conceptSrt": JSON.stringify(data),
                                },
                                type: "GET",
                            })
                            .done(function (result) {
                                if (!result.success) {
                                    switch (parseInt(data.ConceptTypeCode)) {
                                        case 1: //basico
                                            ConceptsToSave.ConceptBasicDelete.push(data);
                                            break;
                                        case 3: //List
                                            ConceptsToSave.ConceptListDelete.push(data);
                                            break;
                                        case 2: //Range
                                            ConceptsToSave.ConceptRangeDelete.push(data);
                                            break;
                                        case 4: //Reference
                                            ConceptsToSave.ConceptReferenceDelete.push(data);
                                            break;
                                    }

                                    deferred.resolve();
                                    objConcepts.Functions.Form.Clear();
                                } else {
                                    $.UifNotify('show', { type: 'info', message: result.result, autoclose: false });
                                }
                            });
                        }
                    }
                    else {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.NotDeleteStaticConcepts, autoclose: false });
                    }
                },
                Refresh: function () {
                }
            },
            ModalList: {
                Add: function () {
                    if ($("#txtListValue").val()) {
                        var data = { ListValue: $("#txtListValue").val(), type: "add" };
                        $("#lVModalList").UifListView("addItem", data);

                        $("#txtListValue").val("");
                        $("#hdnConceptListIndex").val("");
                    } else {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.ListItemIsRequired, autoclose: true });
                    }
                },
                Edit: function () {
                    if ($("#txtListValue").val()) {
                        var data = { ListValue: $("#txtListValue").val() };
                        var index = $("#hdnConceptListIndex").val();
                        $("#lVModalList").UifListView("editItem", index, data);

                        $("#txtListValue").val("");
                        $("#hdnConceptListIndex").val("");
                    }
                    else {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.ListItemIsRequired, autoclose: true });
                    }
                },
                Save: function (item) {
                    if (this.Validate()) {
                        if (item) {
                            var dataRequest = [{
                                Description: $("#txtListName").val(),
                                ListEntityAt: 0,
                                ListEntityCode: item,
                                ListEntityValue: $("#lVModalList").UifListView("getData").filter(function (item) {
                                    return item.type == "add"
                                })
                            }];

                            $.ajax({
                                type: "POST",
                                url: rootPath + "RulesScripts/Concepts/UpdateListEntityValueCreated",
                                data: {
                                    "listEntityData": dataRequest
                                },
                            }).done(function (data) {
                                if (data.success) {

                                    if (!$("#ddlLists").is(':disabled')) {
                                        objConcepts.Init.Components.Selects.ConceptControl.List();
                                        $("#ddlLists").UifSelect("setSelected", data.result[0].ListEntityCode);
                                    }
                                    objConcepts.Functions.Modal.ModalList.Close();
                                } else {
                                    $.UifNotify('show', { type: 'info', message: data.result, autoclose: true });
                                }
                            });

                        } else {
                            var dataRequest = [{
                                Description: $("#txtListName").val(),
                                ListEntityAt: 0,
                                ListEntityCode: 0,
                                ListEntityValue: $("#lVModalList").UifListView("getData")
                            }];

                            $.ajax({
                                type: "POST",
                                url: rootPath + "RulesScripts/Concepts/CreateListEntity",
                                data: {
                                    "listEntityData": dataRequest
                                },
                            }).done(function (data) {
                                if (data.success) {

                                    if (!$("#ddlLists").is(':disabled')) {
                                        objConcepts.Init.Components.Selects.ConceptControl.List();
                                        $("#ddlLists").UifSelect("setSelected", data.result[0].ListEntityCode);
                                    }
                                    objConcepts.Functions.Modal.ModalList.Close();
                                } else {
                                    $.UifNotify('show', { type: 'info', message: data.result, autoclose: true });
                                }
                            });
                        }
                    }
                },
                Validate: function () {
                    if (!$("#txtListName").val()) {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.NameTheListIsRequired, autoclose: true });
                        return false;
                    }
                    if ($("#lVModalList").UifListView("getData").length == 0) {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.ListLeastOneElement, autoclose: true });
                        return false;
                    }
                    return true;
                },
            },
            ModalRange: {
                Add: function () {
                    if ($("#txtRangeFrom").val() && $("#txtRangeTo").val()) {
                        var data = { FromValue: $("#txtRangeFrom").val(), ToValue: $("#txtRangeTo").val(), type: "add" };
                        $("#lVModalRanges").UifListView("addItem", data);

                        $("#txtRangeFrom").val("");
                        $("#txtRangeTo").val("");
                        $("#hdnConceptRangeIndex").val("");
                    } else {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.RankValuesAreRequired, autoclose: true });
                    }
                },
                Edit: function () {
                    if ($("#txtRangeFrom").val() && $("#txtRangeTo").val()) {
                        var data = { FromValue: $("#txtRangeFrom").val(), ToValue: $("#txtRangeTo").val() };
                        var index = $("#hdnConceptRangeIndex").val();
                        $("#lVModalRanges").UifListView("editItem", index, data);

                        $("#txtRangeFrom").val("");
                        $("#txtRangeTo").val("");
                        $("#hdnConceptRangeIndex").val("");
                    }
                    else {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.RankValuesAreRequired, autoclose: true });
                    }
                },
                Save: function (item) {
                    if (this.Validate()) {
                        if (item) {
                            var dataRequest = [{
                                Description: $("#txtRangeName").val(),
                                RangeEntityCode: item,
                                RangeValueAt: 0,
                                RangeEntityValue: $("#lVModalRanges").UifListView("getData").filter(function (item) {
                                    return item.type == "add"
                                })
                            }];

                            $.ajax({
                                type: "POST",
                                url: rootPath + "RulesScripts/Concepts/UpdateRangeEntityValueCreated",
                                data: {
                                    "rangeEntityData": dataRequest
                                },
                            }).done(function (data) {
                                if (data.success) {

                                    if (!$("#ddlRanges").is(':disabled')) {
                                        objConcepts.Init.Components.Selects.ConceptControl.Range();
                                        $("#ddlRanges").UifSelect("setSelected", data.result[0].RangeEntityCode);
                                    }

                                    objConcepts.Functions.Modal.ModalRange.Close();
                                } else {
                                    $.UifNotify('show', { type: 'info', message: data.result, autoclose: true });
                                }
                            });
                        } else {
                            var dataRequest = [{
                                Description: $("#txtRangeName").val(),
                                RangeEntityCode: 0,
                                RangeValueAt: 0,
                                RangeEntityValue: $("#lVModalRanges").UifListView("getData")
                            }];

                            $.ajax({
                                type: "POST",
                                url: rootPath + "RulesScripts/Concepts/CreateRangeEntity",
                                data: {
                                    "rangeEntityData": dataRequest
                                },
                            }).done(function (data) {
                                if (data.success) {

                                    if (!$("#ddlRanges").is(':disabled')) {
                                        objConcepts.Init.Components.Selects.ConceptControl.Range();
                                        $("#ddlRanges").UifSelect("setSelected", data.result[0].RangeEntityCode);
                                    }
                                    objConcepts.Functions.Modal.ModalRange.Close();
                                } else {
                                    $.UifNotify('show', { type: 'info', message: data.result, autoclose: true });
                                }
                            });
                        }
                    }
                },
                Validate: function () {
                    if (!$("#txtRangeName").val()) {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.RankNameIsRequired, autoclose: true });
                        return false;
                    }
                    if ($("#lVModalRanges").UifListView("getData").length == 0) {
                        $.UifNotify('show', { type: 'info', message: Resources.Language.ListLeastOneElement, autoclose: true });
                        return false;
                    }
                    return true;
                },
            },
        },
        Form: {
            Clear: function () {
                ClearValidation("#formConcepts");
                $("#hdnConceptIndex").val("");
                $("#ConceptId").val(0);
                $("#IsStatic").val(false);
                $("#ConceptControlCode").val(0);

                $("#Description").val("");
                $("#Description").focus();
                objConcepts.Init.Components.Selects.Module();
                objConcepts.Init.Components.Selects.Level(1, true);
                $("#ddlLevel").trigger("itemSelected");
                $("#IsVisible").prop("checked", true);
                $("#IsNull").prop("checked", true);
                $("#IsPersistible").prop("checked", true);
                $("#ConceptTypeCode").val("");
                $("#ConceptTypeCode").trigger("itemSelected");
                this.Enable();
                $("#ddlModule").UifSelect("disabled", false);
                $("#ddlLevel").UifSelect("disabled", false);

                $("#IdQuestion").val("");
                $("#QuestionDescription").val("");
                $("#Description").attr("tag", "");
            },

            Assign: function (data, index) {
                if ($("#hdnConceptIndex").val() == "") {
                    $("#hdnConceptIndex").val(index);
                }
                $("#ConceptId").val(data.ConceptId);
                $("#IsStatic").val(data.IsStatic);
                $("#Description").val(data.Description);
                $("#Description").attr("tag", data.Description);
                $("#ConceptControlCode").val(data.ConceptControlCode);

                if (data.EntityId) {
                    objConcepts.Functions.SetLevelEntity(data.EntityId);
                } else {
                    $("#ddlModule").UifSelect("setSelected", data.Module);
                    $("#ddlModule").trigger("itemSelected");
                    $("#ddlLevel").UifSelect("setSelected", data.Level);
                    $("#ddlLevel").trigger("itemSelected");
                }


                $("#IsVisible").prop('checked', data.IsVisible);
                $("#IsNull").prop('checked', data.IsNull);
                $("#IsPersistible").prop('checked', data.IsPersistible);

                $("#ConceptTypeCode").UifSelect("setSelected", data.ConceptTypeCode);
                $("#ConceptTypeCode").trigger("itemSelected");

                if (data.QuestionDescription) {
                    $("#IdQuestion").val(data.IdQuestion);
                    $("#QuestionDescription").val(data.QuestionDescription);
                    $("#QuestionDescriptionTmp").val(data.QuestionDescription);
                } else {
                    $("#IdQuestion").val("");
                    $("#QuestionDescription").val("");
                    $("#QuestionDescriptionTmp").val("");
                }

                switch (parseInt(data.ConceptTypeCode)) {
                    case 1: // basico
                        $("#ConceptControlCodeBasic").UifSelect("setSelected", data.BasicTypeCode);
                        $("#ConceptControlCodeBasic").trigger("itemSelected");
                        objConcepts.Functions.SetBasicConceptsValues(data);
                        break;
                    case 3:// Lista
                        $("#ddlLists").UifSelect("setSelected", data.ListEntityCode);
                    case 2:// Rango
                        $("#ddlRanges").UifSelect("setSelected", data.RangeEntityCode);
                    case 4:// Referencia
                        $("#ddlReferences").UifSelect("setSelected", data.FEntityId);
                        break;
                }




                if (data.IsStatic) {
                    objConcepts.Functions.Form.Disable();
                    $("#Description").attr("disabled", false);
                }
                else {
                    objConcepts.Functions.Form.Disable();
                    $("#IsVisible").attr("disabled", false);
                    $("#IsNull").attr("disabled", false);
                    $("#IsPersistible").attr("disabled", false);
                    $("#Description").attr("disabled", false);
                }
            },

            Disable: function () {
                $("#Description").attr("disabled", true);
                $("#ddlModule").UifSelect("disabled", true);
                $("#ddlLevel").UifSelect("disabled", true);
                $("#IsVisible").attr("disabled", true);
                $("#IsNull").attr("disabled", true);
                $("#IsPersistible").attr("disabled", true);
                $("#ConceptTypeCode").UifSelect("disabled", true);
                $("#ConceptControlCodeBasic").UifSelect("disabled", true);

                try {
                    $("#TxtMax").attr("disabled", true);
                    $("#TxtMin").attr("disabled", true);
                } catch (error) { }
                try {
                    $("#TxtMax").UifSelect("disabled", true);
                    $("#TxtMin").UifSelect("disabled", true);
                } catch (error) { }
                try {
                    $("#TxtMax").UifDatepicker('disabled', true);
                    $("#TxtMin").UifDatepicker('disabled', true);
                } catch (error) {}
                $("#TxtLength").attr("disabled", true);

                $("#ddlLists").UifSelect("disabled", true);
                $("#ddlRanges").UifSelect("disabled", true);
                $("#ddlReferences").UifSelect("disabled", true);
            },

            Enable: function () {
                $("#Description").attr("disabled", false);
                $("#ddlModule").UifSelect("disabled", true);
                $("#ddlLevel").UifSelect("disabled", true);
                $("#IsVisible").attr("disabled", false);
                $("#IsNull").attr("disabled", false);
                $("#IsPersistible").attr("disabled", false);
                $("#ConceptTypeCode").UifSelect("disabled", false);
                $("#ConceptControlCodeBasic").UifSelect("disabled", false);
                try {
                    $("#TxtMax").attr("disabled", false);
                    $("#TxtMin").attr("disabled", false);
                } catch (error)
                { }
                try {
                    $("#TxtMax").UifSelect("disabled", false);
                    $("#TxtMin").UifSelect("disabled", false);
                } catch (error) { }
                try {
                    $("#TxtMax").UifDatepicker('disabled', false);
                    $("#TxtMin").UifDatepicker('disabled', false);
                } catch (error) {}
                $("#TxtLength").attr("disabled", false);

                $("#ddlLists").UifSelect("disabled", false);
                $("#ddlRanges").UifSelect("disabled", false);
                $("#ddlReferences").UifSelect("disabled", false);
            },

            Save: function () {
                if (this.Validate()) {
                    var objForm = this.GetForm();
                    objForm.ConceptTypeDescription = $("#ConceptTypeCode option[value='" + $("#ConceptTypeCode").val() + "']").text();
                    objForm.IsStatic = false;
                    objForm.StaticDescription = "Dinámico";

                    if (objForm.ConceptId == 0) //nuevo
                    {
                        objForm.Status = "add";
                        if ($("#hdnConceptIndex").val()) {//editar uno nuevo
                            conceptsAction.forEach(function (concept) {
                                if (concept.ConceptId == objForm.ConceptId) {
                                    concept = objForm;
                                }
                            });
                        }
                        else {
                            objForm.Status = "add";
                            conceptsAction.push(objForm);
                        }
                    }
                    else { //editar
                        objForm.Status = "edit";
                        conceptsAction.push(objForm);
                    }
                    objConcepts.Functions.Form.Clear();
                    objConcepts.Functions.SaveConcepts();
                }
            },

            Validate: function () {
                if ($("#ConceptTypeCode").val() == "3") { // lista
                    if (!$("#ddlLists").val()) {
                        $.UifNotify('show', {
                            type: 'info', message: Resources.Language.SelectList, autoclose: true
                        });
                        return false;
                    }
                }

                if ($("#ConceptTypeCode").val() == "4") { //referencia
                    if (!$("#ddlReferences").val()) {
                        $.UifNotify('show', {
                            type: 'info', message: Resources.Language.SelectReference, autoclose: true
                        });
                        return false;
                    }
                }

                if ($("#ConceptTypeCode").val() == "2") { //rangos
                    if (!$("#ddlRanges").val()) {
                        $.UifNotify('show', {
                            type: 'info', message: Resources.Language.SelectRange, autoclose: true
                        });
                        return false;
                    }
                }

                return $("#formConcepts").valid();
            },

            GetForm: function () {
                var data = {
                };
                $("#formConcepts").serializeArray().map(function (x) {
                    data[x.name] = x.value;
                });
                data.Module = $("#ddlModule").val();
                data.Level = $("#ddlLevel").val();

                data.IsVisible = $("#IsVisible").is(":checked");
                data.IsNull = $("#IsNull").is(":checked");
                data.IsPersistible = $("#IsPersistible").is(":checked");

                data.IdQuestion = $("#IdQuestion").val() == "" ? 0 : $("#IdQuestion").val();
                data.QuestionDescription = $("#QuestionDescription").val();
                data.ConceptTypeCode = $("#ConceptTypeCode").val();
                if (data.ConceptId != 0) {
                    data.StatusTypeService = 3;
                }
                else {
                    data.StatusTypeService = 2;
                }

                switch (data.ConceptTypeCode) {
                    case "1": //basico
                        data.BasicTypeCode = $("#ConceptControlCodeBasic").val();
                        switch ($("#ConceptControlCodeBasic").val()) {
                            case "1": //Numerico //Decimal
                            case "3":
                                data.MaxValue = $("#TxtMax").val();
                                data.MinValue = $("#TxtMin").val();
                                break;
                            case "2": //texto
                                data.Length = $("#TxtLength").val();
                                break;
                            case "4"://Date
                                data.MaxDate = $("#TxtMax").val();
                                data.MinDate = $("#TxtMin").val();
                                break;
                        }
                        break;
                    case "3": //List
                        data.ListEntityCode = $("#ddlLists").val();
                        break;
                    case "2": //Range
                        data.RangeEntityCode = $("#ddlRanges").val();
                        break;
                    case "4": //reference
                        data.FEntityId = $("#ddlReferences").val();
                        break;
                }
                return data;
            },
        },
        PartialView: {
            Hidden: function () {
                this.HiddenTypeConcepts();
                this.HiddenBasicTypes();
            },
            HiddenTypeConcepts: function () {
                $("#ConceptTypeBasic").hide();
                $("#ConceptTypeList").hide();
                $("#ConceptTypeRange").hide();
                $("#ConceptTypeReference").hide();
            },
            HiddenBasicTypes: function () {
                $("#divTxtMax").html("<input type='text' id='TxtMax'/>");
                $("#divTxtMin").html("<input type='text' id='TxtMin'/>");

                $("#RangeNumeric").hide();
                $("#LengthStr").hide();
                $("#ddlRanges").val("");
                $("#ddlLists").val("");
                $("#ddlReferences").val("");
            },
            ViewTypeConcept: function (TypeConcept) {
                this.Hidden();
                switch (TypeConcept) {
                    case "1": //Basico
                        $("#ConceptTypeBasic").show();
                        $("#ConceptControlCodeBasic").val("");
                        break;
                    case "3": //Lista
                        $("#ConceptTypeList").show();
                        break;
                    case "2":
                        $("#ConceptTypeRange").show();
                        break;
                    case "4":
                        $("#ConceptTypeReference").show();
                        break;
                }
            },

            ViewBasicType: function (TypeBasic) {
                this.HiddenBasicTypes();
                switch (TypeBasic) {
                    case "1": //Numerico
                        $("#RangeNumeric").show();

                        $("#TxtMax").ValidatorKey(1, 2, 1);
                        $("#TxtMin").ValidatorKey(1, 2, 1);
                        break;
                    case "2": //texto
                        $("#LengthStr").show();

                        $("#TxtMax").ValidatorKey(1, 2, 1);
                        break;
                    case "3": //Decimal
                        $("#RangeNumeric").show();

                        $("#TxtMax").OnlyDecimals(2);
                        $("#TxtMin").OnlyDecimals(2);
                        break;
                    case "4"://Date
                        $("#RangeNumeric").show();

                        $("#TxtMax").addClass("uif-datepicker");
                        $("#TxtMin").addClass("uif-datepicker");

                        $("#TxtMax").UifDatepicker();
                        $("#TxtMin").UifDatepicker();
                        break;
                }
            },
        },
        Modal: {
            ModalList: {
                Clear: function () {
                    $("#txtListName").val("");
                    $("#txtListValue").val("");
                    $("#hdnConceptListIndex").val("");
                },
                Open: function (title) {
                    $('#modalLists').UifModal('showLocal', title);
                },
                Close: function () {
                    $('#modalLists').UifModal('hide');
                },
            },
            ModalRange: {
                Clear: function () {
                    $("#txtRangeName").val("");
                    $("#txtRangeFrom").val("");
                    $("#txtRangeTo").val("");
                    $("#hdnConceptRangeIndex").val("");

                    $("#txtRangeFrom").ValidatorKey(1, 2, 1);
                    $("#txtRangeTo").ValidatorKey(1, 2, 1);
                },
                Open: function (title) {
                    $('#modalRanges').UifModal('showLocal', title);
                },
                Close: function () {
                    $('#modalRanges').UifModal('hide');
                },
            },
            ModalReference: {
                Open: function (title) {
                    $('#modalReferences').UifModal('showLocal', title);
                },
            },
            ModalQuestion: {
                Open: function (title) {
                    $('#modalGuiones').UifModal('showLocal', title);
                },
            },
        },
        SetBasicConceptsValues: function (data) {
            if (data.ConceptId == "0") {
                if (data.Length) {
                    $("#TxtLength").val(data.Length);
                } else {
                    if (data.MinValue) {
                        $("#TxtMin").val(data.MinValue);
                    }
                    if (data.MaxValue) {
                        $("#TxtMax").val(data.MaxValue);
                    }
                    if (data.MinDate) {
                        $("#TxtMin").val(data.MinDate);
                    }
                    if (data.MaxDate) {
                        $("#TxtMax").val(data.MaxDate);
                    }
                }
            }
            else {
                $.ajax({
                    type: "GET",
                    url: rootPath + "RulesScripts/Concepts/GetBasicConceptsValues",
                    data: {
                        conceptSrt: JSON.stringify(data)
                    },
                    dataType: "json",
                }).done(function (data) {
                    if (data.success) {
                        if (data.result) {
                            if (data.result.Length) {
                                $("#TxtLength").val(data.result.Length);
                            } else {
                                if (data.result.MinValue) {
                                    $("#TxtMin").val(data.result.MinValue);
                                }
                                if (data.result.MaxValue) {
                                    $("#TxtMax").val(data.result.MaxValue);
                                }
                                if (data.result.MinDate) {
                                    $("#TxtMin").val(data.result.MinDate);
                                }
                                if (data.result.MaxDate) {
                                    $("#TxtMax").val(data.result.MaxDate);
                                }
                            }
                        }
                    } else {
                        $.UifNotify('show', {
                            type: 'info', message: data.result, autoclose: true
                        });
                    }
                });
            }
        },
        SetLevelEntity: function (EntityId) {
            $.ajax({
                type: "GET",
                url: rootPath + "RulesScripts/Concepts/GetEntity",
                data: {
                    "EntityId": EntityId
                },
            }).done(function (data) {
                if (data.success) {
                    $("#ddlModule").UifSelect("setSelected", data.result.ModuleId);
                    $("#ddlModule").trigger("itemSelected");
                    $("#ddlLevel").UifSelect("setSelected", data.result.LevelId);
                    $("#ddlLevel").trigger("itemSelected");
                } else {
                    $.UifNotify('show', {
                        type: 'info', message: data.result, autoclose: true
                    });
                }
            });
        },
        SaveConcepts: function () {
            var dataConcepts = conceptsAction;

            ConceptsToSave.ConceptBasicAdd = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 1 && item.Status == "add"
            });
            ConceptsToSave.ConceptBasicEdit = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 1 && item.Status == "edit"
            });
            //ConceptBasicDelete: [],

            ConceptsToSave.ConceptListAdd = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 3 && item.Status == "add"
            });
            ConceptsToSave.ConceptListEdit = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 3 && item.Status == "edit"
            });
            //ConceptListDelete: [],

            ConceptsToSave.ConceptRangeAdd = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 2 && item.Status == "add"
            });
            ConceptsToSave.ConceptRangeEdit = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 2 && item.Status == "edit"
            });
            //ConceptRangeDelete: [],
            ConceptsToSave.ConceptReferenceAdd = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 4 && item.Status == "add"
            });
            ConceptsToSave.ConceptReferenceEdit = dataConcepts.filter(function (item) {
                return item.ConceptTypeCode == 4 && item.Status == "edit"
            });
            //ConceptReferenceDelete: [],


            $.ajax({
                type: "POST",
                url: rootPath + "RulesScripts/Concepts/SaveConcepts",
                data: {
                    "ConceptBasicAdd": JSON.stringify(ConceptsToSave.ConceptBasicAdd),
                    "ConceptBasicEdit": JSON.stringify(ConceptsToSave.ConceptBasicEdit),
                    "ConceptBasicDelete": JSON.stringify(ConceptsToSave.ConceptBasicDelete),
                    "ConceptListAdd": JSON.stringify(ConceptsToSave.ConceptListAdd),
                    "ConceptListEdit": JSON.stringify(ConceptsToSave.ConceptListEdit),
                    "ConceptListDelete": JSON.stringify(ConceptsToSave.ConceptListDelete),
                    "ConceptRangeAdd": JSON.stringify(ConceptsToSave.ConceptRangeAdd),
                    "ConceptRangeEdit": JSON.stringify(ConceptsToSave.ConceptRangeEdit),
                    "ConceptRangeDelete": JSON.stringify(ConceptsToSave.ConceptRangeDelete),
                    "ConceptReferenceAdd": JSON.stringify(ConceptsToSave.ConceptReferenceAdd),
                    "ConceptReferenceEdit": JSON.stringify(ConceptsToSave.ConceptReferenceEdit),
                    "ConceptReferenceDelete": JSON.stringify(ConceptsToSave.ConceptReferenceDelete),
                },
            }).done(function (data) {
                if (data.success) {
                    objConcepts.Init.Components.Listview.Concepts();
                    objConcepts.Functions.Form.Clear();
                    conceptsAction = [];
                    $.UifNotify('show', {
                        type: 'info', message: data.result, autoclose: true
                    });
                } else {
                    $.UifNotify('show', {
                        type: 'info', message: data.result, autoclose: true
                    });
                }
                objConcepts.Init.Components.CreateObjectToSave();
            });
        },
        ExportConcepts: function () {
            $.ajax({
                type: "GET",
                url: rootPath + "RulesScripts/Concepts/FileConcepts",
                dataType: "json",
            }).done(function (data) {
                if (data.success) {
                    DownloadFile(data.result);
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        },
        Exit: function () {
            window.location = rootPath + "Home/Index";
        },
        GetConceptsByDescription: function () {
            var description = $("#Description").val();

            if ($("#Description").attr("tag") == $("#Description").val()) {
                description = "";
            }
            
            if (!(description == "")) {
                return $.ajax({
                    type: 'POST',
                    url: rootPath + "RulesScripts/Concepts/GetConceptsByDescription",
                    data: JSON.stringify({ description: $("#Description").val() }),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                }).done(function (data) {
                    if (!data.success) {
                        $("#Description").val('');
                        $.UifNotify('show', {
                            'type': 'info', 'message': data.result, 'autoclose': true
                        });
                    }
                });
            }
        }
    }
}
