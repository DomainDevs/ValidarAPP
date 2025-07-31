var objTables = {
    //Al iniciar la pantalla
    Init: $(function () {
        //jQuery.ajaxSetup({ async: true });
        $('#btnPost').hide();
        jQuery(document.body).on('focusin', '.number', function (event) {
            $(".number").ValidatorKey(1, 2, 1)
        });
        jQuery(document.body).on('focusin', '.decimal', function (event) {
            $(".decimal").OnlyDecimals(15)
        });

        objTables.Events.Buttons();
        objTables.Events.DataTable();
        objTables.Events.Modal();

        objTables.Functions.GetAllDataDecisionTable();
    }),

    Events: {
        //Asigna los eventos de la Datetable
        DataTable: function () {
            $('#tableData').on('rowAdd', function (event, selectedRow, position) {
                objTables.Functions.DataTable.AddRule(true);
            });
            $('#tableData').on('rowEdit', function (event, selectedRow, position) {
                objTables.Functions.DataTable.EditRule(selectedRow);
            });
            $('#tableData').on('rowDelete', function (event, selectedRow, position) {
                objTables.Functions.DataTable.DeleteRule(selectedRow, position);
            });
        },

        //Asigna los eventos del modal
        Modal: function () {
            $("#btnSaveRulesModalEditAddData").click(function () {
                objTables.Functions.Modal.ResolveModal();
            });
        },

        //Asigna los eventos a los botones
        Buttons: function () {
            $('#btnExit').click(function () {
                window.location.href = rootPath + "RulesScripts/DecisionTable/Index";
            });
            $("#btnRecord").click(function () {
                objTables.Functions.Validator.ValidateDecisionTable();
            });
            $("#btnPost").click(function () {
                objTables.Functions.PublishDecisionTable();
            });
        },
    },

    GetData: {
        //Obtiene las condiciones
        GetConditions: function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: rootPath + "DecisionTable/GetConditionConcept/" + $("#RuleBase_RuleBaseId").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done(function (data) {
                if (data.success) {
                    resolve(data.result);
                } else {
                    reject(data.result);
                }
                
                //ConceptoCondicion = data;
            }).fail(function (xhr, error) {
                reject(xhr, error);
            });
        },

        //Obtiene las Acciones
        GetActions: function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: rootPath + "DecisionTable/GetActionConcept/" + $("#RuleBase_RuleBaseId").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done(function (data) {
                if (data.success) {
                    resolve(data.result);
                } else {
                    reject(data.result);
                }
            }).fail(function (xhr, error) {
                reject(xhr, error);
            });
        },

        //Obtiene el contenido de la tabla
        GetDecisionTable: function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: rootPath + "DecisionTable/GetDecisionTableData/" + $("#RuleBase_RuleBaseId").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done(function (data) {
                if (data.success) {
                    resolve(data.result);
                } else {
                    reject(data.result);
                }
            }).fail(function (xhr, error) {
                reject(xhr, error);
            });
        },

        GetDataToComposite: {
            GetRulesAdd: function () {
                var list = DecisionTableComposite.RulesComposite.filter(function (item) {
                    return item.Status == "added";
                });

                var objectNew = {
                    RuleBase: DecisionTableComposite.RuleBase,
                    RulesComposite: list
                };

                return objectNew;
            },
            GetRulesEdit: function () {
                var list = DecisionTableComposite.RulesComposite.filter(function (item) {
                    return item.Status == "edited";
                });
                var objectNew = {
                    RuleBase: DecisionTableComposite.RuleBase,
                    RulesComposite: list
                };
                return objectNew;
            },
            GetRulesDelete: function () {
                var list = DecisionTableComposite.RulesComposite.filter(function (item) {
                    return item.Status == "deleted";
                });
                var objectNew = {
                    RuleBase: DecisionTableComposite.RuleBase,
                    RulesComposite: list
                };
                return objectNew;
            },
        },
    },

    SetData: {
        //Pinta la cabecera de la tabla de decision 
        SetHeaderDateTable: function () {
            if (ConceptoCondicion != null && ConceptoAccion != null) {
                var titulo = "";
                $("#tableData thead tr").append('<th data-property="' + 'RuleId' + '" data-visible="false">' + 'RuleId' + '</th>');
                $("#tableData thead tr").append('<th data-property="' + 'Status' + '" data-visible="false">' + 'Status' + '</th>');

                for (j in ConceptoCondicion) {
                    if (ConceptoCondicion[j].hasOwnProperty('Description')) {
                        titulo = ConceptoCondicion[j].Description;
                        $("#tableData thead tr").append('<th data-property="' + titulo + ' condicion">' + titulo + '</th>');
                    }
                }

                titulo = "-->";
                $("#tableData thead tr").append('<th data-property="' + titulo + '">' + titulo + '</th>');

                for (j in ConceptoAccion) {
                    if (ConceptoAccion[j].hasOwnProperty('Description')) {
                        titulo = ConceptoAccion[j].Description;
                        $("#tableData thead tr").append('<th data-property="' + titulo + ' accion">' + titulo + '</th>');
                    }
                }
            }
        },

        //Pinta el contenido de la taba de decision
        SetBodyDateTable: function () {
            if (DecisionTableComposite != null) {
                var ruleBase = [];


                $.each(DecisionTableComposite.RulesComposite, function (index, item) {
                    if (item.Status != "deleted") {
                        var ruleObject = {
                            "RuleId": item.RuleId,
                            "-->": " - ",
                            "Status": item.Status == undefined ? "" : item.Status
                        };
                        //recorre las condiciones
                        $.each(item.Conditions, function (indexCondition, condition) {
                            var conditionDescription = condition.Concept.Description;
                            var ConceptDescriptionValue = null;

                            if (condition.Comparator == null) {
                                ConceptDescriptionValue = condition.DescriptionValue;
                            }
                            else {
                                switch (condition.Concept.ConceptControlCode) {
                                    case 1:
                                    case 2:
                                    case 3:
                                        ConceptDescriptionValue = condition.Comparator.Symbol + " " + condition.DescriptionValue;
                                        break;
                                    default:
                                        ConceptDescriptionValue = condition.Comparator.Symbol + " " + condition.DescriptionValue + " (" + condition.Value + ") ";
                                        break;
                                }
                            }
                            ruleObject[conditionDescription + ' condicion'] = ConceptDescriptionValue;
                        });
                        //recorre las acciones
                        $.each(this.Actions, function (indexAction, action) {
                            var actionDescription = action.ConceptLeft.Description;
                            var actionDescriptionValue = null;

                            if (this.Operator == null) {
                                actionDescriptionValue = action.Expression;
                            }
                            else {
                                switch (action.ConceptLeft.ConceptControlCode) {
                                    case 1:
                                    case 2:
                                    case 3:
                                        actionDescriptionValue = action.Operator.Symbol + " " + action.Expression;
                                        break;
                                    default:
                                        actionDescriptionValue = action.Operator.Symbol + " " + action.Expression + " (" + action.ValueRight + ") ";
                                        break;
                                }
                            }
                            ruleObject[actionDescription + ' accion'] = actionDescriptionValue;
                        });
                        ruleBase.push(ruleObject);
                    }
                });
                $("#tableData").UifDataTable("clear");
                $("#tableData").UifDataTable({ source: null, filter: true, paginate: false, edit: true, delete: true, order: false });
                if (ruleBase.length != 0) {
                    $('#tableData').UifDataTable('addRow', ruleBase);
                }
            }
        },
    },

    Functions: {
        //Obtiene toda la informacion necesaria (condiciones,acciones, data)
        GetAllDataDecisionTable: function () {
            var PrmGetDecisionTable = new Promise(objTables.GetData.GetDecisionTable);
            var PrmGetConditions = new Promise(objTables.GetData.GetConditions);
            var PrmGetActions = new Promise(objTables.GetData.GetActions);

            Promise.all([PrmGetConditions, PrmGetActions]).then(data => {
                ConceptoCondicion = data[0];
                ConceptoAccion = data[1];

                objTables.SetData.SetHeaderDateTable();
            }).catch(function (error) {
                $.UifNotify('show', {
                    'type': 'info', 'message': Resources.Language.UnexpectedError, 'autoclose': true
                });
            });

            Promise.all([PrmGetDecisionTable, PrmGetConditions, PrmGetActions]).then(data => {
                DecisionTableComposite = data[0];
                objTables.Functions.SortRulesComposite.Sort();
                objTables.SetData.SetBodyDateTable();
            }).catch(function (error) {
                $.UifNotify('show', {
                    'type': 'info', 'message': Resources.Language.UnexpectedError, 'autoclose': true
                });
            });
        },

        //Valida la tabla de decision 
        Validator: {
            ValidateDecisionTable: function () {
                $('#btnPost').hide();

                var RulesValidatorVar = this.GetRulesValidator();

                if (!this.ValidateRuleDefault(RulesValidatorVar)) {
                    this.SetRuleDefault();
                    RulesValidatorVar = this.GetRulesValidator();
                }

                this.SendValidateDecisionTable(RulesValidatorVar);
            },

            //Envia al controlador los datos a validar
            SendValidateDecisionTable: function (RulesValidatorVar) {
                $.ajax({
                    type: "POST",
                    url: rootPath + "DecisionTable/ValidateTableDecision",
                    data: {
                        RulesValidator: RulesValidatorVar
                    }
                }).done(function (data) {
                    $($("#tableData tbody tr")).removeClass("row-selected-warning");
                    if (data.success) {
                        objTables.Functions.SaveDecisionTable();
                    }
                    else {
                        var index = 0;
                        $.each(DecisionTableComposite.RulesComposite, function (indexRule, item) {
                            if (item.Status != "deleted") {
                                if (data.result.indexOf(item.RuleId) != -1) {
                                    $($("#tableData tbody tr")[index]).addClass("row-selected-warning");
                                }
                                index++;
                            }
                        });
                        $.UifNotify('show', {
                            'type': 'info', 'message': Resources.Language.InvalidTable, 'autoclose': true
                        });
                    }
                }).fail(function (xhr, status) {
                    $.UifNotify('show', {
                        'type': 'info', 'message': Resources.Language.UnexpectedError, 'autoclose': true
                    });
                });
            },

            //Arma el objeto de reglas a validar
            GetRulesValidator: function () {
                var listRules = new Array();
                $.each(DecisionTableComposite.RulesComposite, function (index, item) {
                    if (item.Status != "deleted") {
                        listRules.push({
                            idRule: item.RuleId,
                            Conditions: objTables.Functions.Validator.GetConditionsValidator(item.Conditions)
                        });
                    }
                });
                return listRules;
            },

            //Arma las condiciones a Validar
            GetConditionsValidator: function (conditions) {
                var listConditions = new Array();
                $.each(conditions, function (index, item) {
                    listConditions.push({
                        idConcept: item.Concept.ConceptId,
                        idEntity: item.Concept.EntityId,
                        valueOperator: item.Comparator != null ? item.Comparator.ComparatorCode : null,
                        symbolOperator: item.Comparator != null ? item.Comparator.Symbol : "Ind",
                        value: item.Value == null ? " " : item.Value
                    });
                });
                return listConditions;
            },

            //valida que exista la regla default
            ValidateRuleDefault: (function (RulesValidator) {
                if (RulesValidator.length == 0) {
                    return false;
                }

                var lastRule = RulesValidator[RulesValidator.length - 1];
                var countConditionLast = lastRule.Conditions.filter(function (x) { return x.symbolOperator == "Ind"; }).length;
                var totalCondition = lastRule.Conditions.length;
                if (countConditionLast != totalCondition) {
                    return false;
                }
                return true;
            }),

            //Asigna la regla por defecto
            SetRuleDefault: function () {
                objTables.Functions.DataTable.AddRule(false);
                objTables.Functions.Modal.ResolveModal();
            },
        },

        SaveDecisionTable: function () {

            var RulesAdd = objTables.GetData.GetDataToComposite.GetRulesAdd();
            var RulesEdit = objTables.GetData.GetDataToComposite.GetRulesEdit();
            var RulesDelete = objTables.GetData.GetDataToComposite.GetRulesDelete();

            $.ajax({
                type: "POST",
                url: rootPath + "DecisionTable/CreateTableDecisionRow",
                data: {
                    "RulesAdd": RulesAdd,
                    "RulesEdit": RulesEdit,
                    "RulesDelete": RulesDelete
                },
                success: function (data) {
                    if (data.success) {
                        $.each(DecisionTableComposite.RulesComposite, function (indexRule, rule) {
                            DecisionTableComposite.RulesComposite[indexRule].Status = undefined;
                        });
                        $.UifNotify('show', {
                            'type': 'success', 'message': Resources.Language.MessageSavedSuccessfully, 'autoclose': true
                        });
                        $('#btnPost').show();
                    } else {
                        $.UifNotify('show', {
                            'type': 'warning', 'message': Resources.Language.EnterMinimumRule, 'autoclose': true
                        });
                        $('#btnPost').hide();
                    }
                },
                error: function (xhr, status) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': Resources.Language.UnexpectedError, 'autoclose': true
                    });
                }
            });
        },

        //Publica la tabla de decision
        PublishDecisionTable: function () {
            $.ajax({
                type: "POST",
                url: rootPath + "DecisionTable/PostTableDecision?id=" + $("#RuleBase_RuleBaseId").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        $.UifNotify('show', {
                            'type': 'success', 'message': Resources.Language.PublishedCorrectly, 'autoclose': true
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
        },

        //Ordena los datos de la RuleComposite
        SortRulesComposite: {
            //reordena la lista RulesComposite
            Sort: function () {
                DecisionTableComposite.RulesComposite.sort(function (x, y) {
                    return objTables.Functions.SortRulesComposite.RulesCompositeComparer.Compare(x, y, DecisionTableComposite.RulesComposite[0].Conditions);
                });
            },
            RulesCompositeComparer: {
                //Compara las condiciones y da un indice de orden
                Compare: function (x, y, _conditions) {
                    var symbols = ["=", "<", "<=", ">", ">=", "<>"];

                    var conditionsx = x.Conditions;
                    var conditionsy = y.Conditions;

                    //Concept conddef in _conditions
                    for (var index = 0; index < _conditions.length ; index++) {
                        var conddef = _conditions[index];
                        var entityId = conddef.Concept.EntityId;
                        var conceptId = conddef.Concept.ConceptId;

                        //Condition
                        var condx = this.FindCondition(entityId, conceptId, conditionsx);
                        var condy = this.FindCondition(entityId, conceptId, conditionsy);

                        if (condx == null || condx.Comparator == null) {
                            if (condy != null && condy.Comparator != null) {
                                return 1;
                            }
                        }
                        else {
                            if (condy == null || condy.Comparator == null) {
                                return -1;
                            }

                            var comp = this.CompareTo(symbols.indexOf(condx.Comparator.Symbol), symbols.indexOf(condy.Comparator.Symbol));
                            if (comp != 0) {
                                return comp;
                            }
                            else {
                                if ((isNaN(parseFloat(condx.Value)) == false) && (isNaN(parseFloat(condy.Value)) == false)) {
                                    if (parseFloat(condx.Value) < parseFloat(condy.Value)) {
                                        comp = -1;
                                    }
                                    else if (parseFloat(condx.Value) == parseFloat(condy.Value)) {
                                        comp = 0;
                                    }
                                    else {
                                        comp = 1;
                                    }
                                }
                                else {
                                    comp = this.CompareTo(condx.Value.toLowerCase(), condy.Value.toLowerCase());
                                }
                                if (comp != 0) {
                                    var ct = condx.Comparator.Symbol;
                                    if (ct == ">" || ct == ">=") {
                                        return -comp;
                                    }
                                    return comp;
                                }
                            }
                        }
                    }
                    return this.CompareTo(x.RuleId, y.RuleId);
                },

                //Retorna una condicion buscada
                //int entityId, int conceptId, IList<Condition> conditions
                FindCondition: function (entityId, conceptId, conditions) {
                    var returnValue;
                    $.each(conditions, function (index, cond) {
                        if (cond.Concept.EntityId == entityId && cond.Concept.ConceptId == conceptId) {
                            returnValue = cond;
                            return;
                        }
                    });

                    return returnValue;
                },

                //Compara dos valores
                CompareTo(x, y) {
                    if (x > y) {
                        return 1;
                    }
                    else if (x == y) {
                        return 0;
                    }
                    else if (x < y) {
                        return -1;
                    }
                }
            },
        },

        Modal: {
            //Abre la pantalla modal
            ShowModal: function () {
                $("#EditAddRowTable").html('');
                $('#BodyModalTableEditAddData').UifModal('showLocal', Resources.Language.RulesModalEditAddData);
            },

            //Cierra el modal
            HideModal: function () {
                $('#BodyModalTableEditAddData').UifModal('hide', '');
            },

            //Carga el formulario
            LoadForm: function (Conditions, Actions, add) {
                var formulario = '';
                formulario += '<h4><strong>' + Resources.Language.LabelConditions +'</strong></h4>';
                formulario += '<hr>';
                formulario += '<div class="row uif-row-pc">';
                formulario += ' <div class="uif-col col-md-12">';
                $.each(Conditions, function (index, value) {
                    formulario += ' <div class="uif-col col-md-12">';
                    formulario += '  <div class="uif-col col-md-4">';
                    formulario += '   <div class="ptitle" id="tilteCondition' + ((add) ? index : this.Id) + '"></div>';
                    formulario += '  </div>';
                    formulario += '  <div class="uif-col col-md-4">';
                    formulario += '   <select class="uif-select form-control" id="comparator' + ((add) ? index : this.Id) + '" data-name="Symbol" data-id="ComparatorSymbol" data-filter="true"></select>';
                    formulario += '  </div>';
                    formulario += '  <div class="uif-col col-md-4">';
                    formulario += objTables.Functions.Modal.AssignControl(((add) ? this : this.Concept), 'controlCondition'.concat((add) ? index : this.Id));
                    formulario += '  </div>';
                    formulario += '  </div>';
                });
                formulario += ' </div>';
                formulario += '</div>';

                formulario += '<h4><strong>' + Resources.Language.Actions +'</strong></h4>';
                formulario += '<hr>';
                formulario += '<div class="row uif-row-pc">';
                formulario += ' <div class="uif-col col-md-12">';
                $.each(Actions, function (index, value) {
                    formulario += ' <div class="uif-col col-md-12">';
                    formulario += '  <div class="uif-col col-md-4">';
                    formulario += '   <div class="ptitle" id="tilteAction' + ((add) ? index : this.Id) + '"></div>';
                    formulario += '  </div>';
                    formulario += '  <div class="uif-col col-md-4">';
                    formulario += '   <select class="form-control" id="operator' + ((add) ? index : this.Id) + '"></select>';
                    formulario += '  </div>';
                    formulario += '  <div class="uif-col col-md-4">';
                    formulario += objTables.Functions.Modal.AssignControl(((add) ? this : this.ConceptLeft), 'controlAction'.concat((add) ? index : this.Id));
                    formulario += '  </div>';
                    formulario += '  </div>';
                });
                formulario += ' </div>';
                formulario += '</div>';

                $("#EditAddRowTable").html(formulario);
            },

            //Asigna los correspondientes controles
            AssignControl: function (concepto, id) {
                var formulario = '';
                var tipoControl = SearchCombo.obtencionTipoControl(concepto.ConceptId, concepto.EntityId, '');
                switch (tipoControl.ConceptControlCode) {
                    case 1:
                        formulario += '   <input type="text" class="form-control" id="' + id + '">';
                        break;
                    case 3:
                        //formulario += '   <input type="date" class="form-control" id="' + id + '">';
                        formulario += '   <input type="text" value="" class="uif-datepicker" id="' + id + '">';

                        break;
                    case 2:
                        if (tipoControl.BasicType == 3) {
                            formulario += '   <input type="text" class="form-control decimal" id="' + id + '">';

                        } else {
                            formulario += '   <input type="text" class="form-control number" id="' + id + '">';
                        }
                        break;
                    case 4:
                    case 5:
                        formulario += '   <select class="form-control" id="' + id + '"></select>';
                        break;
                    case 6:
                        break;
                    default:
                        break;
                }
                return formulario
            },

            //carga los datos del formulario
            LoadDataForm: function (Conditions, Actions, add) {
                $.each(Conditions, function (index, valor) {
                    $("#comparator" + ((add) ? index : valor.Id)).UifSelect({
                        source: rootPath + "RuleSet/GetConceptComparator?conceptId=" + ((add) ? this.ConceptId : this.Concept.ConceptId) + "&entityId=" + ((add) ? this.EntityId : this.Concept.EntityId)
                    });

                    SearchCombo.obtencionTipoControl(
                        ((add) ? this.ConceptId : this.Concept.ConceptId),
                        ((add) ? this.EntityId : this.Concept.EntityId),
                        "#controlCondition" + ((add) ? index : this.Id));
                });

                $.each(Actions, function (index, valor) {

                    $.ajax({
                        type: "POST",
                        url: rootPath + "DecisionTable/GetOperationTypes",
                        data: JSON.stringify({
                            conceptId: ((add) ? this.ConceptId : this.ConceptLeft.ConceptId), entityId: ((add) ? this.EntityId : this.ConceptLeft.EntityId)
                        }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            $("#operator" + ((add) ? index : valor.Id)).append($('<option>', {
                                value: '', text: 'Indistinto'
                            }));
                            $.each(data.result, function (key, item) {
                                $("#operator" + ((add) ? index : valor.Id)).append($('<option>', {
                                    value: item.OperatorCode,
                                    text: item.Symbol
                                }));
                            });
                        }
                    });

                    SearchCombo.obtencionTipoControl(
                        ((add) ? this.ConceptId : this.ConceptLeft.ConceptId),
                        ((add) ? this.EntityId : this.ConceptLeft.EntityId),
                        "#controlAction" + ((add) ? index : this.Id));
                });
            },

            //asigna el formulario
            AssignForm: function (Conditions, Actions, add) {
                $.each(Conditions, function (index, value) {
                    $("#tilteCondition" + ((add) ? index : this.Id)).html(((add) ? this.Description : this.Concept.Description));
                    if (this.Comparator != null && !add) {
                        $("#comparator" + this.Id).val(SearchCombo.ComparatorControlGet(this));
                    }
                    if ($.trim(this.Value) != "" && !add) {
                        switch (this.Concept.ConceptControlCode) {
                            case 1:
                            case 2:
                            case 3:
                                $("#controlCondition" + this.Id).val(this.Value);
                                break;
                            default:
                                $("#controlCondition" + this.Id).UifSelect("setSelected", this.Value);
                                break;
                        }
                    }
                });

                $.each(Actions, function (index, value) {
                    $("#tilteAction" + ((add) ? index : this.Id)).html(((add) ? this.Description : this.ConceptLeft.Description));
                    if (this.Operator != null && !add) {
                        $("#operator" + this.Id).val(this.Operator.OperatorCode);
                    }
                    if ($.trim(this.ValueRight) != "" && !add) {
                        switch (this.ConceptLeft.ConceptControlCode) {
                            case 1:
                            case 2:
                            case 3:
                                $("#controlAction" + this.Id).val(this.ValueRight);
                                break;
                            default:
                                $("#controlAction" + this.Id).UifSelect("setSelected", this.ValueRight);
                                break;
                        }
                    }
                });
            },

            //Guardar Modal
            ResolveModal: function () {
                if (!RulesComposite.Conditions.length) {//nueva
                    RulesComposite.Status = "added";
                    //Se adiciona al Objeto
                    if (DecisionTableComposite.RulesComposite.length > 0) {
                        RulesComposite.RuleId = SearchCombo.GetIdMayor(DecisionTableComposite.RulesComposite, 'RuleId') + 1
                    } else {
                        RulesComposite.RuleId = 1
                    }

                    $.each(ConceptoCondicion, function (index, value) {
                        var Condition = new oCondition();
                        Condition.Concept = this;
                        Condition.Id = index + 1;

                        Condition.Value = $("#controlCondition" + index).val();
                        if ($("#controlCondition" + this.Id).hasClass('decimal')) {
                            Condition.Value = $("#controlCondition" + this.Id).val().replace(",", ".");
                        }

                        if ($("#comparator" + index).val() != "" && $("#controlCondition" + index).val() != "") {

                            Condition.Comparator = {
                                "Symbol": $("#comparator" + index + " option:selected").text(),
                                "ComparatorCode": SearchCombo.ComparatorControlSet(Number($("#comparator" + index).val()))
                            };

                            if ($("#controlCondition" + index)[0].tagName == "SELECT") {
                                Condition.DescriptionValue = $("#controlCondition" + index + " option:selected").text();
                                Condition.Value = Number($("#controlCondition" + index).val());
                            } else {
                                Condition.DescriptionValue = $("#controlCondition" + index).val();
                            }
                        }
                        else {
                            Condition.DescriptionValue = "Indistinto";
                            Condition.Comparator = null;
                        }

                        RulesComposite.Conditions.push(Condition)
                    });

                    $.each(ConceptoAccion, function (index, value) {
                        var Action = new oAction();
                        Action.ConceptLeft = this
                        Action.Id = index + 1;

                        Action.ValueRight = $("#controlAction" + index).val();
                        if ($("#controlAction" + this.Id).hasClass('decimal')) {
                            Action.ValueRight = $("#controlAction" + this.Id).val().replace(",", ".");
                        }


                        if ($("#operator" + index).val() != "" && $("#controlAction" + index).val() != "") {

                            Action.Operator = {
                                "Symbol": $("#operator" + index + "  option:selected").text(),
                                "OperatorCode": $("#operator" + index).val()
                            };
                            if ($("#controlAction" + index)[0].tagName == "SELECT") {
                                Action.Expression = $("#controlAction" + index + " option:selected").text();
                                Action.ValueRight = Number($("#controlAction" + index).val());
                            } else {
                                Action.Expression = $("#controlAction" + index).val();
                            }
                        }
                        else {
                            Action.Expression = "Indistinto";
                            Action.Operator = null;
                        }

                        RulesComposite.Actions.push(Action)
                    });

                    DecisionTableComposite.RulesComposite.push(RulesComposite)
                }
                else {
                    if (RulesComposite.Status == undefined) {
                        RulesComposite.Status = "edited";
                    }

                    var contPintado = 0;
                    $.each(RulesComposite.Conditions, function (index, value) {
                        //modifica el objeto
                        if ($("#comparator" + this.Id).val() != "" && $("#controlCondition" + this.Id).val() != "") {
                            if ($("#controlCondition" + this.Id)[0].tagName == "SELECT") {
                                this.DescriptionValue = $("#controlCondition" + this.Id + " option:selected").text();
                                this.Value = Number($("#controlCondition" + this.Id).val());
                            } else {
                                this.DescriptionValue = $("#controlCondition" + this.Id).val();

                                this.Value = $("#controlCondition" + this.Id).val();
                                if ($("#controlCondition" + this.Id).hasClass('decimal')) {
                                    this.Value = $("#controlCondition" + this.Id).val().replace(",", ".");
                                }
                            }
                            if (this.Comparator == null) {
                                this.Comparator = {
                                };
                            }
                            this.Comparator.Symbol = $("#comparator" + this.Id + " option:selected").text();
                            this.Comparator.Description = null;
                            this.Comparator.ComparatorCode = SearchCombo.ComparatorControlSet(Number($("#comparator" + this.Id).val()));
                        }
                        else {
                            this.DescriptionValue = "Indistinto";
                            this.Value = null;
                            this.Comparator = null;
                        }
                    });

                    contPintado++;

                    $.each(RulesComposite.Actions, function (index, value) {

                        if ($("#operator" + this.Id).val() != "" && $("#controlAction" + this.Id).val() != "") {
                            if ($("#controlAction" + this.Id)[0].tagName == "SELECT") {
                                this.Expression = $("#controlAction" + this.Id + " option:selected").text();
                                this.ValueRight = Number($("#controlAction" + this.Id).val())
                            }
                            else {
                                this.Expression = $("#controlAction" + this.Id).val()
                                this.ValueRight = $("#controlAction" + this.Id).val()
                                if ($("#controlAction" + this.Id).hasClass('decimal')) {
                                    this.ValueRight = $("#controlAction" + this.Id).val().replace(",", ".");
                                }
                            }
                            if (this.Operator == null) {
                                this.Operator = {
                                };
                            }
                            this.Operator.Symbol = $("#operator" + this.Id + "  option:selected").text();
                            this.Operator.OperatorCode = $("#operator" + this.Id).val();
                            this.Operator.Description = null;
                        } else {
                            this.Operator = null;
                            this.Expression = "Indistinto";
                            this.ValueRight = null;
                        }
                    });
                }
                objTables.Functions.SortRulesComposite.Sort();
                objTables.SetData.SetBodyDateTable();
                objTables.Functions.Modal.HideModal();
            },
        },

        DataTable: {
            //PopUp para agregar un registro a la tabla
            AddRule: function (IsShow) {
                RulesComposite = new oRuleComposite();
                if (IsShow) {
                    objTables.Functions.Modal.ShowModal();
                }
                objTables.Functions.Modal.LoadForm(ConceptoCondicion, ConceptoAccion, true);
                objTables.Functions.Modal.LoadDataForm(ConceptoCondicion, ConceptoAccion, true);
                objTables.Functions.Modal.AssignForm(ConceptoCondicion, ConceptoAccion, true);
                $('#btnPost').hide();
            },

            //PopUp para editar un registro a la tabla
            EditRule: function (selectedRow) {
                if (selectedRow.RuleId > 0) {
                    var ruleIndex = SearchCombo.GetObjects(DecisionTableComposite.RulesComposite, 'RuleId', selectedRow.RuleId)
                    RulesComposite = DecisionTableComposite.RulesComposite[ruleIndex];

                    objTables.Functions.Modal.ShowModal();
                    objTables.Functions.Modal.LoadForm(RulesComposite.Conditions, RulesComposite.Actions, false);
                    objTables.Functions.Modal.LoadDataForm(RulesComposite.Conditions, RulesComposite.Actions, false);
                    objTables.Functions.Modal.AssignForm(RulesComposite.Conditions, RulesComposite.Actions, false);
                    $('#btnPost').hide();
                }
            },

            //Elimina un registro de la tabla
            DeleteRule: function (selectedRow, position) {
                $('#tableData').UifDataTable('deleteRow', position);

                var index = -1;
                $.each(DecisionTableComposite.RulesComposite, function (indexRule, rule) {
                    if (rule.RuleId == selectedRow.RuleId) {
                        index = indexRule;
                        return;
                    }
                });

                if (selectedRow.Status == "added") {
                    DecisionTableComposite.RulesComposite.splice(index, 1);
                }
                else {
                    DecisionTableComposite.RulesComposite[index].Status = "deleted";
                }
                $('#btnPost').hide();
            },
        },
    }
}




