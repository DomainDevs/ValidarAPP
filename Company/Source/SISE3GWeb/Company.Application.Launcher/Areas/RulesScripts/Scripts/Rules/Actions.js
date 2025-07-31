$.ajaxSetup({ async: false });
ControlAction = {
    variables: {
        actionIdEdit: null,
        async: (function () {
            $.ajaxSetup({ async: false });
        }()),
    },
    Funcionalidad: {
        controlesEvent: (function () {
            $("#ActionList").on('rowAdd', function (event, data, position) {
                ControlAction.variables.actionIdEdit = null
                ControlAction.Funcionalidad.addEditAction();
            })

            $('#GetActionTypeCollection').on('itemSelected', function (event, selectedItem) {
                ControlAction.Funcionalidad.AssignType(selectedItem.Id, selectedItem.Text)
            });

            $('#panelActionSearchCombo').on('itemSelected', function (event, selectedItem) {
                ControlAction.Funcionalidad.ConceptLeft(selectedItem.Id)
            });

            $('#GetOperationTypes').on('itemSelected', function (event, selectedItem) {
                ControlAction.Funcionalidad.Operator()
            });

            $('#GetValueTypes').on('itemSelected', function (event, selectedItem) {
                ControlAction.Funcionalidad.ValueTypes()
            });

            $('#GetFunctionTypes').on('itemSelected', function (event, selectedItem) {
                ControlAction.Funcionalidad.GetFunctionTypes()
            });
        }()),
        addEditAction: function () {
            ControlAction.ControlesClean.cleanControlBasicAction()

            $("#contentEditAction").UifModal("showLocal", "Acciones");
            $("#contentEditCondition").UifModal("hide");

            $("#panelEditAccion").show()

            $("#GetActionTypeCollection").UifSelect({ source: null });
        },
        AssignType: function (Id, Text, Value) {
            ControlAction.ControlesClean.cleanControlBasicAction()

            ConceptAction = new oConcept();
            ConceptAction.EntityId = 315;

            ActionType = new oActionType();
            ActionType.Code = Id;
            ActionType.Descripcion = Text;

            switch ($("#GetActionTypeCollection").val()) {
                case "1":
                    var rule = $("#RuleSetList").UifListView("getData")[0];
                    SearchCombo.ListaControlSearch("#conceptoActionSelect", ConceptAction.EntityId, null, rule.LevelId);
                    $("#panelActionSearchCombo").show()
                    if (Value) {
                        $("#conceptoActionSelect").UifSelect("setSelected", Value.ConceptLeft.ConceptId + "/" + Value.ConceptLeft.EntityId);
                        ControlAction.Funcionalidad.ConceptLeft(Value.ConceptLeft.ConceptId + "/" + Value.ConceptLeft.EntityId, Value)
                    }
                    break;
                case "2":
                    $("#panelGetFunctionTypes").show()
                    if (Value) {
                        if (Value.Message != null) {
                            $("#GetFunctionTypes").UifSelect("setSelected", 1);
                            $("#panelActionTextBox").show()
                            $("#actionTextBox").val(Value.Message);
                        }
                        if (Value.RuleSetId > 0) {
                            $("#GetFunctionTypes").UifSelect("setSelected", 2);
                            $("#panelRuleSet").show()
                            ControlAction.Funcionalidad.GetFunctionTypes()
                            $("#RuleSetSelect").UifSelect("setSelected", Value.RuleSetId);
                        }
                        if (Value.IdFuction != null) {
                            $("#GetFunctionTypes").UifSelect("setSelected", 3);
                            $("#panelActionListEditor").show()
                            ControlAction.Funcionalidad.GetFunctionTypes()
                            $("#actionListEditor").UifSelect("setSelected", Value.IdFuction);
                        }
                    }
                    break;
                case "3":
                    $("#panelActionTemporaryName").show()
                    $("#panelOperationTypes").show()
                    $("#GetOperationTypes").UifSelect({ source: rootPath + "RuleSet/GetOperationTypes?conceptId=0&entityId=0" });
                    if (Value) {
                        $("#actionTemporaryName").val(Value.TemporalNameLeft)
                        $("#GetOperationTypes").UifSelect("setSelected", Value.Operator.OperatorCode);
                        $("#panelOperationTypes").show()
                        ControlAction.Funcionalidad.Operator(Value)
                    } else {
                        $("#actionTemporaryName").val()
                    }

                    break;
                default:
                    break;
            }
        },
        ConceptLeft: function (Id, Value) {
            $("#panelOperationTypes").hide()
            $("#panelValueTypes").hide()

            ControlAction.ControlesClean.cleanControlBasicComun();

            var res = (Id).split("/");
            ConceptAction.ConceptId = Number(res[0]);
            ConceptAction.EntityId = Number(res[1]);
            ConceptAction.Description = $('#conceptoActionSelect').UifSelect("getSelectedText");

            $("#panelOperationTypes").show()
            $("#GetOperationTypes").UifSelect({ source: rootPath + "RuleSet/GetOperationTypes?conceptId=" + ConceptAction.ConceptId + "&entityId=" + ConceptAction.EntityId });
            if (Value) {
                $("#GetOperationTypes").UifSelect("setSelected", Value.Operator.OperatorCode);
                ControlAction.Funcionalidad.Operator(Value)
            }
        },
        Operator: function (Value) {
            Operator = new oOperator();
            Operator.OperatorCode = Number($('#GetOperationTypes').UifSelect("getSelected"));
            Operator.Description = $('#GetOperationTypes').UifSelect("getSelectedText");

            ControlAction.ControlesClean.cleanControlBasicComun()
            $("#panelValueTypes").hide()
            $("#GetValueTypes").UifSelect({ source: null });

            if ($("#GetOperationTypes").val() != null && $('#GetOperationTypes').val() >= 0) {
                $("#panelValueTypes").show()
                $("#GetValueTypes").UifSelect({ source: rootPath + "RuleSet/GetValueTypes" });
                if (Value) {
                    if (Value.ValueRight != null) {
                        $("#GetValueTypes").UifSelect("setSelected", 1);
                    }
                    if (Value.ConceptRight) {
                        $("#GetValueTypes").UifSelect("setSelected", 2);
                    }
                    if (Value.TemporalNameRight) {
                        $("#GetValueTypes").UifSelect("setSelected", 4);
                    }
                    ControlAction.Funcionalidad.ValueTypes(Value)
                }
            }
        },
        ValueTypes: function (value) {
            ControlAction.ControlesClean.cleanControlBasicComun()
            ValueType = new oValueType();
            ValueType.Value = Number($('#GetValueTypes').UifSelect("getSelected"));
            ValueType.Description = $('#GetValueTypes').UifSelect("getSelectedText");

            switch ($("#GetValueTypes").val()) {
                case "1":
                    var tipoControl = null
                    if (ConceptAction.ConceptId != null) {
                        tipoControl = SearchCombo.obtencionTipoControl(ConceptAction.ConceptId, ConceptAction.EntityId, '');
                    }
                    if (tipoControl) {
                        switch (tipoControl.ConceptControlCode) {
                            case 1:
                                $("#panelActionTextBox").show()
                                if (value) {
                                    $("#actionTextBox").val(value.ValueRight);
                                }
                                break;
                            case 2:
                                if (tipoControl.BasicType == 3) {
                                    $("#panelActionNumberDecimalEditor").show()
                                    $("#actionNumberDecimalEditor").OnlyDecimals(15)
                                    if (value) {
                                        $("#actionNumberDecimalEditor").val(value.ValueRight);
                                    }
                                } else {
                                    $("#panelActionNumberEditor").show()
                                    $("#actionNumberEditor").ValidatorKey(1, 2, 1)
                                    if (value) {
                                        $("#actionNumberEditor").val(value.ValueRight);
                                    }
                                }
                                break;
                            case 3:
                                $("#panelActionDateEditor").show()
                                if (value) {
                                    $("#actionDateEditor").val(value.ValueRight);
                                }
                                break;
                            case 4:
                                $("#panelActionListEditor").show()
                                SearchCombo.ArmadoControlLits("#actionListEditor", JSON.stringify(tipoControl.ListListEntityValues));
                                if (value) {
                                    $("#actionListEditor").UifSelect("setSelected", value.ValueRight);
                                }
                                break;
                            case 5:
                                $("#panelActionConceptReference").show()
                                var rule = $("#RuleSetList").UifListView("getData")[0];
                                SearchCombo.ListaControlSearch("#referenciaActionSelect", tipoControl.ForeignEntity, rule.LevelId);
                                if (value) {
                                    $("#referenciaActionSelect").UifSelect("setSelected", value.ValueRight);
                                }
                                break;
                            case 6:
                                break;
                            default:
                                break;
                        }
                    } else {
                        $("#panelActionNumberEditor").show()
                        $("#actionNumberEditor").ValidatorKey(1, 2, 1)
                        if (value) {
                            $("#actionNumberEditor").val(value.ValueRight)
                        }

                    }

                    break;
                case "2":
                    $("#panelActionConcept").show()
                    var rule = $("#RuleSetList").UifListView("getData")[0];

                    SearchCombo.ListaControlSearch("#conceptLeftActionSelect", 315, null, rule.LevelId);
                    if (value) {
                        $("#conceptLeftActionSelect").UifSelect("setSelected", value.ConceptRight.ConceptId + "/" + value.ConceptRight.EntityId);
                    }
                    break;
                case "3":
                    break;
                case "4":
                    $("#panelTemporalNameRight").show()
                    if (value) {
                        $("#TemporalNameRight").val(value.TemporalNameRight)
                    }
                    break;
                default:
                    break;
            }
        },
        GetFunctionTypes: function () {
            ControlAction.ControlesClean.cleanControlBasicComun()

            InvokeType = new oInvokeType();
            InvokeType.Value = $("#GetFunctionTypes").UifSelect("getSelected");
            InvokeType.Description = $("#GetFunctionTypes").UifSelect("getSelectedText");

            switch ($("#GetFunctionTypes").val()) {
                case "1":
                    $("#panelActionTextBox").show()
                    break;
                case "2":
                    $("#panelRuleSet").show()
                    var rule = $("#RuleSetList").UifListView("getData")[0];
                    SearchCombo.ListaControlSearch("#RuleSetSelect", 101, null, rule.LevelId);
                    break;
                case "3":
                    $("#panelActionListEditor").show()
                    $("#actionListEditor").UifSelect({ source: rootPath + "RuleSet/GetRuleFunctions", id: "FunctionName", name: "Description", filter: true, native: false });
                    break;
                default:
                    break;
            }



        },
        IdAction: function () {
            var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)

            if (RuleSetComposite.RuleComposites[positionRuleEdit].Actions.length > 0) {
                return SearchCombo.GetIdMayor(RuleSetComposite.RuleComposites[positionRuleEdit].Actions, 'Id') + 1
            } else {
                return 0
            }
        },
    },
    CreacionJson: (function () {
        $('#referenciaActionSelect').on('itemSelected', function (event, selectedItem) {
            Action = new oAction();
            Action.Id = ControlAction.Funcionalidad.IdAction()
            Action.ConceptLeft = ConceptAction;
            Action.Operator = Operator;
            Action.ValueType = 0;
            Action.ValueRight = Number(selectedItem.Id);
            Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + selectedItem.Text;
            $("#btnRecordAction").prop("disabled", false);
        });

        $('#conceptLeftActionSelect').on('itemSelected', function (event, selectedItem) {
            ConceptRight = new oConcept();
            var res = (selectedItem.Id).split("/");
            ConceptRight.ConceptId = Number(res[0]);
            ConceptRight.EntityId = Number(res[1]);
            ConceptRight.Description = selectedItem.Text;

            if ($("#actionTemporaryName").is(":visible") && $("#actionTemporaryName").val().length > 0) {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.AssignType = 2
                Action.Operator = Operator;
                Action.TemporalNameLeft = $("#actionTemporaryName").val();
                Action.ConceptRight = ConceptRight;
                Action.Expression = ActionType.Descripcion + " \"" + Action.TemporalNameLeft + "\" " + Operator.Description + " El " + ValueType.Description + " " + selectedItem.Text;
                $("#btnRecordAction").prop("disabled", false);
            } else {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.ConceptLeft = ConceptAction;
                Action.Operator = Operator;
                Action.ValueType = 0;
                Action.ConceptRight = ConceptRight;
                Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + selectedItem.Text;
                $("#btnRecordAction").prop("disabled", false);
            }
        });

        $("#TemporalNameRight").keyup(function () {
            if (ActionType.Code == 1) {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.ConceptLeft = ConceptAction;
                Action.Operator = Operator;
                Action.ValueType = 0;
                Action.TemporalNameRight = $("#TemporalNameRight").val();
                Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + $("#TemporalNameRight").val();
                $("#btnRecordAction").prop("disabled", false);
            }
            else {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.TemporalNameLeft = $("#actionTemporaryName").val();
                Action.Operator = Operator;
                Action.ValueType = 0;
                Action.ValueRight = $("#TemporalNameRight").val();
                Action.Expression = ActionType.Descripcion + " " + $("#actionTemporaryName").val() + " " + Operator.Description + " El " + ValueType.Description + " " + $("#TemporalNameRight").val();
                $("#btnRecordAction").prop("disabled", false);
            }

        })

        $("#actionTextBox").keyup(function () {
            if (ActionType.Code == 1) {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.ConceptLeft = ConceptAction;
                Action.Operator = Operator;
                Action.ValueType = 0;
                Action.ValueRight = $("#actionTextBox").val();
                Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + $("#actionTextBox").val();
                $("#btnRecordAction").prop("disabled", false);
            }
            else {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.AssignType = 1
                Action.Message = $("#actionTextBox").val();
                Action.Expression = ActionType.Descripcion + " " + InvokeType.Description + " \"" + $("#actionTextBox").val() + "\"";
                $("#btnRecordAction").prop("disabled", false);
            }
        })

        $("#actionTemporaryName").change(function () {
            $("#panelOperationTypes").show()
            $("#GetOperationTypes").UifSelect({ source: rootPath + "RuleSet/GetOperationTypes?conceptId=0&entityId=0" });
            $("#btnRecordAction").prop("disabled", true);
            $("#panelValueTypes").hide()
            $("#GetValueTypes").UifSelect({ source: null });
            $("#panelActionNumberEditor").hide()
            $("#actionNumberEditor").val(null)
        })

        $("#actionNumberEditor").keyup(function () {

            if ($("#actionTemporaryName").is(":visible") && $("#actionTemporaryName").val().length > 0) {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.AssignType = 2
                Action.Operator = Operator;
                Action.ValueRight = $("#actionNumberEditor").val();
                Action.TemporalNameLeft = $("#actionTemporaryName").val();
                Action.Expression = ActionType.Descripcion + " \"" + Action.TemporalNameLeft + "\" " + Operator.Description + " El " + ValueType.Description + " " + $("#actionNumberEditor").val();
                $("#btnRecordAction").prop("disabled", false);
            } else {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.ConceptLeft = ConceptAction;
                Action.Operator = Operator;
                Action.ValueType = 0;
                Action.ValueRight = Number($("#actionNumberEditor").val());
                Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + $("#actionNumberEditor").val();
                $("#btnRecordAction").prop("disabled", false);
            }
        })

        $("#actionNumberDecimalEditor").keyup(function () {
            Action = new oAction();
            Action.Id = ControlAction.Funcionalidad.IdAction()
            Action.ConceptLeft = ConceptAction;
            Action.Operator = Operator;
            Action.ValueType = 0;
            //Action.ValueRight = $("#actionNumberDecimalEditor").val();

            if ((NotFormatMoney($("#actionNumberDecimalEditor").val()).split(',')).length > 1) {
                Action.ValueRight = NotFormatMoney($("#actionNumberDecimalEditor").val());
            }
            else {
                Action.ValueRight = ($("#actionNumberDecimalEditor").val() + ",00000");
            }
            Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + $("#actionNumberDecimalEditor").val();
            $("#btnRecordAction").prop("disabled", false);
        })

        $("#actionDateEditor").keyup(function () {
            Action = new oAction();
            Action.Id = ControlAction.Funcionalidad.IdAction()
            Action.ConceptLeft = ConceptAction;
            Action.Operator = Operator;
            Action.ValueType = 0;
            Action.ValueRight = $("#actionDateEditor").val();
            Action.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + $("#actionDateEditor").val();
            $("#btnRecordAction").prop("disabled", false);

        })

        $('#RuleSetSelect').on('itemSelected', function (event, selectedItem) {
            Action = new oAction();
            Action.Id = ControlAction.Funcionalidad.IdAction()
            Action.AssignType = 1
            Action.RuleSetId = selectedItem.Id;
            Action.DescriptionRuleSet = selectedItem.Text;
            Action.RuleSetId = Number($('#RuleSetSelect').UifSelect("getSelected"));
            Action.Expression = ActionType.Descripcion + " " + InvokeType.Description + " " + selectedItem.Text
            $("#btnRecordAction").prop("disabled", false);
        });

        $('#actionListEditor').on('itemSelected', function (event, selectedItem) {
            if (ActionType.Code == 1) {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.ConceptLeft = ConceptAction;
                Action.Operator = Operator;
                Action.ValueType = 0;
                Action.ValueRight = Number($('#actionListEditor').UifSelect("getSelected"));
                Action.RuleSetId = ControlRuleSet.variables.idRuleEdit;
                Action.Expression = ActionType.Descripcion + " " + ConceptAction.Description + " " + Operator.Description + " El " + ValueType.Description + " " + $('#actionListEditor').UifSelect("getSelectedText");
                $("#btnRecordAction").prop("disabled", false);
            } else {
                Action = new oAction();
                Action.Id = ControlAction.Funcionalidad.IdAction()
                Action.AssignType = 1
                Action.IdFuction = selectedItem.Id;
                Action.DescriptionFunction = selectedItem.Text;
                Action.RuleSetId = ControlRuleSet.variables.idRuleEdit;
                Action.Expression = ActionType.Descripcion + " " + InvokeType.Description + " " + selectedItem.Text
                $("#btnRecordAction").prop("disabled", false);
            }
        });
    }()),
    ABMAccion: {
        SeleccionarAccion: (function () {
            $('#ActionList').on('rowEdit', function (event, data, position) {
                var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
                ControlAction.variables.actionIdEdit = data.Id
                var positionActionEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites[positionRuleEdit].Actions, 'Id', ControlAction.variables.actionIdEdit)
                var objActionTemp = RuleSetComposite.RuleComposites[positionRuleEdit].Actions[positionActionEdit]
                ControlAction.Funcionalidad.addEditAction();

                $("#GetActionTypeCollection").UifSelect("setSelected", objActionTemp.AssignType + 1);
                ControlAction.Funcionalidad.AssignType($('#GetActionTypeCollection').UifSelect("getSelected"), $('#GetActionTypeCollection').UifSelect("getSelectedText"), objActionTemp)
                $("#btnRecordAction").prop("disabled", false);
            })
        }()),
        AdicionarEditar: (function () {
            $("#btnRecordAction").click(function () {
                if (Action == null) {
                    if (ControlAction.variables.actionIdEdit != null) {
                        $("#contentEditAction").UifModal("hide");
                        $("#GetActionTypeCollection").UifSelect({ source: null });
                        ControlAction.ControlesClean.cleanControlBasicAction();
                        ControlAction.ControlesClean.cleanControlBasicComun();
                        ControlAction.variables.actionIdEdit = null
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': 'Action Nula', 'autoclose': true });
                    }
                } else {
                    if (ControlAction.variables.actionIdEdit != null) {
                        //alert("editar")
                        Action.Id = ControlAction.variables.actionIdEdit
                        var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
                        var positionActionEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites[positionRuleEdit].Actions, 'Id', ControlAction.variables.actionIdEdit)

                        RuleSetComposite.RuleComposites[positionRuleEdit].Actions[positionActionEdit] = Action

                        $("#ActionList").UifListView("editItem", positionActionEdit, RuleSetComposite.RuleComposites[positionRuleEdit].Actions[positionActionEdit]);

                        $("#contentEditAction").UifModal("hide");
                        $("#GetActionTypeCollection").UifSelect({ source: null });
                        ControlAction.ControlesClean.cleanControlBasicAction();
                        ControlAction.ControlesClean.cleanControlBasicComun();
                        ControlAction.variables.actionIdEdit = null
                    }
                    else {
                        //alert("agregar")
                        var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
                        RuleSetComposite.RuleComposites[positionRuleEdit].Actions.push(Action)
                        $("#ActionList").UifListView("addItem", Action);

                        $("#contentEditAction").UifModal("hide");
                        $("#GetActionTypeCollection").UifSelect({ source: null });
                        ControlAction.ControlesClean.cleanControlBasicAction();
                        ControlAction.ControlesClean.cleanControlBasicComun();
                        ControlAction.variables.actionIdEdit = null
                    }
                }
            })
        }()),
        EliminarAccion: function (deferred, data) {
            var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
            var positionConditionEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites[positionRuleEdit].Actions, 'Id', data.Id)

            RuleSetComposite.RuleComposites[positionRuleEdit].Actions.splice(positionConditionEdit, 1);

            deferred.resolve();
        }
    },
    ControlesClean: {
        cleanControlBasicAction: function () {
            $("#panelActionSearchCombo").hide()
            $("#conceptoActionSelect").UifSelect({ source: null });
            $("#panelGetFunctionTypes").hide()
            $("#GetFunctionTypes").UifSelect({ source: null });

            $("#panelOperationTypes").hide()
            $("#GetOperationTypes").UifSelect({ source: null });

            $("#panelValueTypes").hide()
            $("#GetValueTypes").UifSelect({ source: null });

            ControlAction.ControlesClean.cleanControlBasicComun()
            $("#panelActionTemporaryName").hide()
            $("#actionTemporaryName").val(null)
        },
        cleanControlBasicComun: function () {
            $("#panelActionConcept").hide()
            $("#referenciaActionSelect").UifSelect({ source: null });
            $("#panelRuleSet").hide()
            $("#RuleSetSelect").UifSelect({ source: null });
            $("#panelActionConceptReference").hide()
            $("#referenciaActionSelect").UifSelect({ source: null });
            $("#panelActionTextBox").hide()
            $("#actionTextBox").val(null)
            $("#panelTemporalNameRight").hide()
            $("#TemporalNameRight").val(null)
            $("#panelActionNumberEditor").hide()
            $("#actionNumberEditor").val(null)
            $("#panelActionNumberDecimalEditor").hide()
            $("#actionNumberDecimalEditor").val(null)
            $("#panelActionDateEditor").hide()
            $("#actionDateEditor").val(null)
            $("#panelActionListEditor").hide()
            $("#actionListEditor").val(null)

            Action = null;
            $("#btnRecordAction").prop("disabled", true);
            $("#TemporalNameRight").val(null)
            $("#actionTextBox").val(null)
        }
    },
}

//var Action = Action || {};

