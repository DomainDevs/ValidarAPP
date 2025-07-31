$.ajaxSetup({ async: false });
ControlCondition = {
    variables: {
        conditionIdEdit: null,
        async: (function(){
            $.ajaxSetup({ async: false });
                
        }()),
    },
    Funcionalidad: {
        controlesEvent: (function () {
            $("#ConditionList").on('rowAdd', function (event, data, position) {
                ControlCondition.variables.conditionIdEdit = null
                ControlCondition.Funcionalidad.addEditCondition();
            })

            $('#conceptRightConditionSelect').on('itemSelected', function (event, selectedItem) {
                ControlCondition.ControlesClean.cleanControlBasicCondition()
                ControlCondition.ControlesClean.cleanControlEditorCondition()

                var res = (selectedItem.Id).split("/");
                ConceptCondition.ConceptId = Number(res[0]);
                ConceptCondition.EntityId = Number(res[1]);
                ConceptCondition.Description = selectedItem.Text;

                ControlCondition.Funcionalidad.itemSeleccionadoCondicionRight(ConceptCondition.ConceptId, ConceptCondition.EntityId);
            })

            $('#conditionIs').on('itemSelected', function (event, selectedItem) {
                Comparator = new oComparator();                
                Comparator.ComparatorCode = SearchCombo.ComparatorControlSet(Number(selectedItem.Id));
                Comparator.Description = selectedItem.Text;
                ControlCondition.Funcionalidad.Comparator(Comparator);
            });

            $('#conditionValueTypes').on('itemSelected', function (event, selectedItem) {

                ControlCondition.ControlesClean.cleanControlEditorCondition()

                ValueType = new oValueType();
                ValueType.Value = Number(selectedItem.Id);
                ValueType.Description = selectedItem.Text;
                ControlCondition.Funcionalidad.ValueTypes();
            });
        }()),
        addEditCondition: function () {
            $("#contentEditCondition").UifModal("showLocal", "Condiciones");
            $("#contentEditAction").UifModal("hide");

            $("#conceptRightConditionSelect").UifSelect({ source: null });
            ControlCondition.ControlesClean.cleanControlBasicCondition()
            ControlCondition.ControlesClean.cleanControlEditorCondition()

            ConceptCondition = new oConcept();
            ConceptCondition.EntityId = 315;
            let rule = $("#RuleSetList").UifListView("getData")[0];

            SearchCombo.ListaControlSearch("#conceptRightConditionSelect", ConceptCondition.EntityId, null, rule.LevelId);
        },
        itemSeleccionadoCondicionRight: function (ConceptId, EntityId) {
            $("#conditionIs").UifSelect({ source: rootPath + "RuleSet/GetConceptComparator?conceptId=" + ConceptId + "&entityId=" + EntityId });

            $("#panelConditionIs").show()
        },
        Comparator: function (Comparator, value) {
            $("#conditionValueTypes").UifSelect({ source: null })
            ControlCondition.ControlesClean.cleanControlEditorCondition()

            $("#referenciaConditionSelect").UifSelect({ source: null });

            switch ($("#conditionIs").val()) {
                case "1":
                case "2":
                case "13":
                case "14":
                case "15":
                case "16":
                    $("#panelConditionValueTypes").show()

                    if (value) {
                        if (value.ConceptValue != null) {
                            $("#conditionValueTypes").UifSelect("setSelected", 2);
                        }
                        else {
                            $("#conditionValueTypes").UifSelect("setSelected", 1);
                        }
                        ValueType = new oValueType();
                        ValueType.Value = $('#conditionValueTypes').UifSelect("getSelected");
                        ValueType.Description = $('#conditionValueTypes').UifSelect("getSelectedText");
                        ControlCondition.Funcionalidad.ValueTypes(value);
                    }
                    
                    break;
                case "3":
                case "4":
                    $("#panelConditionValueTypes").hide()
                    Condition = new oCondition();
                    Condition.Concept = ConceptCondition;
                    Condition.Comparator = Comparator;
                    Condition.ValueType = 0;
                    Condition.ConceptValue = null;
                    Condition.Expression = ConceptCondition.Description + " " + Comparator.Description;
                    $("#btnRecordCondition").prop("disabled", false);
                    break;
                default:
                    break;
            }
        },
        ValueTypes: function (value) {
            $("#referenciaConditionSelect").UifSelect({ source: null });

            switch ($("#conditionValueTypes").val()) {
                case "1":
                    var tipoControl = SearchCombo.obtencionTipoControl(ConceptCondition.ConceptId, ConceptCondition.EntityId, '');
                    switch (tipoControl.ConceptControlCode) {
                        case 1:
                            $("#panelConditionTextBox").show()
                            if (value) {
                                $("#conditionTextBox").val(value.Value);
                            }
                            break;
                        case 2:
                            if (tipoControl.BasicType == 3) {
                                $("#panelConditionNumberDecimalEditor").show()
                                $("#conditionNumberDecimalEditor").OnlyDecimals(15)
                                if (value) {
                                    $("#conditionNumberDecimalEditor").val(value.Value);
                                }
                            } else {
                                $("#panelConditionNumberEditor").show()
                                $("#conditionNumberEditor").ValidatorKey(1, 2, 1)
                                if (value) {
                                    $("#conditionNumberEditor").val(value.Value);
                                }
                            }
                            break;
                        case 3:
                            $("#panelConditionDateEditor").show()
                            if (value) {
                                $("#conditionDateEditor").val(value.Value);
                            }
                            break;
                        case 4:
                            $("#panelConditionListEditor").show()
                            SearchCombo.ArmadoControlLits("#conditionListEditor", JSON.stringify(tipoControl.ListListEntityValues))
                            if (value) {
                                $("#conditionListEditor").UifSelect("setSelected", value.Value);
                            }
                            break;
                        case 5:
                            $("#panelConditionReference").show();
                            var rule = $("#RuleSetList").UifListView("getData")[0];

                            SearchCombo.ListaControlSearch("#referenciaConditionSelect",
                                tipoControl.ForeignEntity,
                                null,
                                rule.LevelId);
                            if (value) {
                                $("#referenciaConditionSelect").UifSelect("setSelected", value.Value); 
                            }
                            break;
                        case 6:
                            break;
                        default:
                            break;
                    }
                    break;
                case "2":
                    $("#panelConditionConcept").show()
                    var rule = $("#RuleSetList").UifListView("getData")[0];

                    SearchCombo.ListaControlSearch("#conceptLeftConditionSelect", 315, null, rule.LevelId);
                    if (value) {
                        $("#conceptLeftConditionSelect").UifSelect("setSelected", value.ConceptValue.ConceptId + "/" + value.ConceptValue.EntityId);
                    }
                    break;
                default:
                    break;
            }
        },
        IdCondition: function () {
            var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)

            if (RuleSetComposite.RuleComposites[positionRuleEdit].Conditions.length > 0) {
                return SearchCombo.GetIdMayor(RuleSetComposite.RuleComposites[positionRuleEdit].Conditions, 'Id') + 1
            } else {
                return 0
            }
        },        
    },
    CreacionJson: (function () {
        $('#referenciaConditionSelect').on('itemSelected', function (event, selectedItem) {
            Condition = new oCondition();
            Condition.Id = ControlCondition.Funcionalidad.IdCondition()
            Condition.Concept = ConceptCondition;
            Condition.Comparator = Comparator;
            Condition.ValueType = 0;
            Condition.Value = Number(selectedItem.Id);
            Condition.ConceptValue = null;
            Condition.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Condition.Expression = ConceptCondition.Description + " " + Comparator.Description + " " + ValueType.Description + " " + selectedItem.Text + " (" + selectedItem.Id + ")";

            $("#btnRecordCondition").prop("disabled", false);
        });

        $('#conceptLeftConditionSelect').on('itemSelected', function (event, selectedItem) {
            ConceptValueCondition = new oConcept();
            var res = (selectedItem.Id).split("/");
            ConceptValueCondition.ConceptId = Number(res[0]);
            ConceptValueCondition.EntityId = Number(res[1]);
            ConceptValueCondition.Description = selectedItem.Text;

            Condition = new oCondition();
            Condition.Id = ControlCondition.Funcionalidad.IdCondition()
            Condition.Concept = ConceptCondition;
            Condition.Comparator = Comparator;
            Condition.ValueType = 0;
            Condition.ConceptValue = ConceptValueCondition;
            Condition.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Condition.Expression = ConceptCondition.Description + " " + Comparator.Description + " " + ValueType.Description + " " + selectedItem.Text;
            $("#btnRecordCondition").prop("disabled", false);
        });

        $("#conditionTextBox").keyup(function () {
            Condition = new oCondition();
            Condition.Id = ControlCondition.Funcionalidad.IdCondition()
            Condition.Concept = ConceptCondition;
            Condition.Comparator = Comparator;
            Condition.ValueType = 0;
            Condition.ConceptValue = null;
            Condition.Value = $("#conditionTextBox").val();
            Condition.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Condition.Expression = ConceptCondition.Description + " " + Comparator.Description + " " + ValueType.Description + " " + $("#conditionTextBox").val();
            $("#btnRecordCondition").prop("disabled", false);
        })

        $("#conditionNumberEditor").keyup(function () {
            Condition = new oCondition();
            Condition.Id = ControlCondition.Funcionalidad.IdCondition()
            Condition.Concept = ConceptCondition;
            Condition.Comparator = Comparator;
            Condition.ValueType = 0;
            Condition.ConceptValue = null;
            Condition.Value = Number($("#conditionNumberEditor").val());
            Condition.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Condition.Expression = ConceptCondition.Description + " " + Comparator.Description + " " + ValueType.Description + " " + $("#conditionNumberEditor").val();
            $("#btnRecordCondition").prop("disabled", false);
        })

        $("#conditionNumberDecimalEditor").keyup(function () {
            Condition = new oCondition();
            Condition.Id = ControlCondition.Funcionalidad.IdCondition()
            Condition.Concept = ConceptCondition;
            Condition.Comparator = Comparator;
            Condition.ConceptValue = null;
            Condition.ValueType = 0;
            if ((NotFormatMoney($("#conditionNumberDecimalEditor").val()).split(',')).length > 1) {
                //Condition.Value = (NotFormatMoney($("#conditionNumberDecimalEditor").val()).replace(separatorDecimal, '.'));
                Condition.Value = NotFormatMoney($("#conditionNumberDecimalEditor").val());
            }
            else {
                Condition.Value = ($("#conditionNumberDecimalEditor").val() + ",00000");
            }
            Condition.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Condition.Expression = ConceptCondition.Description + " " + Comparator.Description + " " + ValueType.Description + " " + $("#conditionNumberDecimalEditor").val();
            $("#btnRecordCondition").prop("disabled", false);
        })

        $('#conditionDateEditor').on("datepicker.change", function (event, date) {        
            Condition = new oCondition();
            Condition.Id = ControlCondition.Funcionalidad.IdCondition()
            Condition.Concept = ConceptCondition;
            Condition.Comparator = Comparator;
            Condition.ValueType = 0;
            Condition.ConceptValue = null;
            Condition.Value = $("#conditionDateEditor").val();
            Condition.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Condition.Expression = ConceptCondition.Description + " " + Comparator.Description + " " + ValueType.Description + " " + $("#conditionDateEditor").val();
            $("#btnRecordCondition").prop("disabled", false);
        })

        $('#conditionListEditor').on('itemSelected', function (event, selectedItem) {
            Condition = new oCondition();
            Condition.Id = ControlCondition.Funcionalidad.IdCondition()
            Condition.Concept = ConceptCondition;
            Condition.Comparator = Comparator;
            Condition.ConceptValue = null;
            Condition.ValueType = 0;
            Condition.Value = Number($('#conditionListEditor').UifSelect("getSelected"));
            Condition.RuleSetId = ControlRuleSet.variables.idRuleEdit;
            Condition.Expression = ConceptCondition.Description + " " + Comparator.Description + " " + ValueType.Description + " " + $('#conditionListEditor').UifSelect("getSelectedText");
            $("#btnRecordCondition").prop("disabled", false);
        })
    }()),
    ABMCondicion: {
        SeleccionarCondicion: (function () {
            $('#ConditionList').on('rowEdit', function (event, data, position) {

                var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
                ControlCondition.variables.conditionIdEdit = data.Id
                var positionConditionEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites[positionRuleEdit].Conditions, 'Id', ControlCondition.variables.conditionIdEdit)
                var objCondicionTemp = RuleSetComposite.RuleComposites[positionRuleEdit].Conditions[positionConditionEdit]
                              
                ControlCondition.Funcionalidad.addEditCondition();
                $("#conceptRightConditionSelect").UifSelect("setSelected", objCondicionTemp.Concept.ConceptId + "/" + objCondicionTemp.Concept.EntityId);

                ControlCondition.ControlesClean.cleanControlBasicCondition()
                ControlCondition.ControlesClean.cleanControlEditorCondition()

                ConceptCondition.ConceptId = objCondicionTemp.Concept.ConceptId;
                ConceptCondition.EntityId = objCondicionTemp.Concept.EntityId;
                ConceptCondition.Description = objCondicionTemp.Concept.Description;

                ControlCondition.Funcionalidad.itemSeleccionadoCondicionRight(ConceptCondition.ConceptId, ConceptCondition.EntityId);

                $("#conditionIs").UifSelect("setSelected", SearchCombo.ComparatorControlGet(objCondicionTemp));

                Comparator = new oComparator();
                Comparator.ComparatorCode = SearchCombo.ComparatorControlSet(Number($('#conditionIs').UifSelect("getSelected")));                
                Comparator.Description = $('#conditionIs').UifSelect("getSelectedText");
                ControlCondition.Funcionalidad.Comparator(Comparator, objCondicionTemp);

                $("#btnRecordCondition").prop("disabled", false);
            })
        }()),
        AdicionarEditar: (function () {
            $("#btnRecordCondition").click(function () {
                if (Condition == null) {
                    if (ControlCondition.variables.conditionIdEdit != null) {
                        $("#contentEditCondition").UifModal("hide");
                        $("#conceptRightConditionSelect").UifSelect()
                        ControlCondition.ControlesClean.cleanControlBasicCondition();
                        ControlCondition.ControlesClean.cleanControlEditorCondition();
                        ControlCondition.variables.conditionIdEdit = null
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': 'Condition Nula', 'autoclose': true });
                    }
                } else {
                    if (ControlCondition.variables.conditionIdEdit != null) {
                        //alert("editar")                                     
                        Condition.Id = ControlCondition.variables.conditionIdEdit
                        var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
                        var positionConditionEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites[positionRuleEdit].Conditions, 'Id', Condition.Id)

                        RuleSetComposite.RuleComposites[positionRuleEdit].Conditions[positionConditionEdit] = Condition

                        $("#ConditionList").UifListView("editItem", positionConditionEdit, RuleSetComposite.RuleComposites[positionRuleEdit].Conditions[positionConditionEdit]);

                        $("#contentEditCondition").UifModal("hide");
                        $("#conceptRightConditionSelect").UifSelect()
                        ControlCondition.ControlesClean.cleanControlBasicCondition();
                        ControlCondition.ControlesClean.cleanControlEditorCondition();
                        ControlCondition.variables.conditionIdEdit = null
                    }
                    else {
                        //alert("agregar")
                        var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
                        RuleSetComposite.RuleComposites[positionRuleEdit].Conditions.push(Condition)
                        $("#ConditionList").UifListView("addItem", Condition);

                        $("#contentEditCondition").UifModal("hide");
                        $("#conceptRightConditionSelect").UifSelect()
                        ControlCondition.ControlesClean.cleanControlBasicCondition();
                        ControlCondition.ControlesClean.cleanControlEditorCondition();
                    }
                }
            })
        }()),
        EliminarCondicion: function (deferred, data) {
                        
            var positionRuleEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites, 'RuleId', ControlRuleSet.variables.idRuleEdit)
            var positionConditionEdit = SearchCombo.GetIndexObjects(RuleSetComposite.RuleComposites[positionRuleEdit].Conditions, 'Id', data.Id)

            RuleSetComposite.RuleComposites[positionRuleEdit].Conditions.splice(positionConditionEdit, 1);

            deferred.resolve();
        }
    }, 
    ControlesClean: {
        cleanControlBasicCondition: function () {
            $("#panelConditionIs").hide()            
            $("#panelConditionValueTypes").hide()

            $("#btnRecordCondition").prop("disabled", true);
            Condition = null;            
            ConceptCondition = new oConcept();
            Comparator = new oComparator();
        },
        cleanControlEditorCondition: function () {
            $("#panelConditionReference").hide()
            $("#panelConditionConcept").hide()

            $("#panelConditionTextBox").hide()
            $("#panelConditionNumberEditor").hide()
            $("#panelConditionNumberDecimalEditor").hide()
            $("#panelConditionDateEditor").hide()
            $("#panelConditionListEditor").hide()

            $("#conditionTextBox").val(null)
            $("#conditionNumberEditor").val(null)
            $("#conditionNumberDecimalEditor").val(null)
            $("#conditionDateEditor").val(null)
            $("#ConditionListEditor").val(null)

            $("#btnRecordCondition").prop("disabled", true);
            Condition = null;
            ValueType = new oValueType();
        }
    },   
}
