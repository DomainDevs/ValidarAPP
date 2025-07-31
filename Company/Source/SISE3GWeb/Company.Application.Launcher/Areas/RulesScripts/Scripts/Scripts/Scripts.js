$.ajaxSetup({ async: false });
var ScriptComposite;
var NodoId;
var NewNodeId;
var ScriptId;
var htmlScript;
var QuestionId;
var Question;
var PositionQuestion;
var PositionAnwer;
var NodeIndex;
var NextNodeId;
var Edges;

var ControlScript = {
    Init: {
        Start: $(function () {
            ControlScript.Functions.Form.Clear();

            ControlScript.Init.Components.Selects.Package(1);
            ControlScript.Init.Components.Selects.Level(1, 2);

            ControlScript.Events.Selects.Package();
            ControlScript.Events.Buttons.New();
            ControlScript.Events.Buttons.Delete();
            ControlScript.Events.Buttons.Export();
            ControlScript.Events.Buttons.MainQuestion();
            ControlScript.Events.Buttons.Question();
            ControlScript.Events.Buttons.Answer();
            ControlScript.Events.Buttons.Save();
            ControlScript.Events.Buttons.Exit();
            ControlScript.Events.Buttons.Execute();
        }),

        Components: {
            Selects: {
                Package: function (itemId) {
                    $("#selectPackageScript").UifSelect({
                        source: rootPath + "RulesScripts/Scripts/GetPackages",
                        selectedId: itemId ? itemId : null
                    });
                },
                Level: function (IdModule, ItemId) {
                    if (IdModule) {
                        $("#selectLevelScript").UifSelect(
                            {
                                source: rootPath + "RulesScripts/Scripts/GetLevels?packageId=" + IdModule,
                                selectedId: ItemId ? ItemId : null
                            });
                    }
                    else {
                        $("#selectLevelScript").UifSelect({ source: "" });
                    }
                },
                Question: function () {
                    var IdLevel = $("#selectLevelScript").UifSelect("getSelected");
                    var module = parseInt($("#selectPackageScript").UifSelect("getSelected"));
                    if (IdLevel) {
                        $("#selectQuestion").UifSelect(
                            {
                                source: rootPath + "RulesScripts/Scripts/GetQuestionsControlByLevel?level=" + IdLevel +"?module="+ module,
                                filter: true,
                                native: false
                            });
                    } else {
                        $("#selectQuestion").UifSelect({ source: "" });
                    }
                },
            },
        },
    },

    Events: {
        Selects: {
            Package: function () {
                $("#selectPackageScript").on("itemSelected", function (event, selectedItem) {
                    ControlScript.Init.Components.Selects.Level(selectedItem.Id);
                });
            },
        },
        Buttons: {
            New: function () {
                $("#btnNew").on("click", function () {
                    ControlScript.Functions.Form.Clear();
                });
            },
            Delete: function () {
                $("#btnDelete").on("click", function () {
                    if ($("#ScriptId").val()) {
                        $.UifDialog('confirm', { 'message': Resources.Language.ConfirmDeleteScript }, function (result) {
                            if (result) {
                                $.ajax({
                                    type: "POST",
                                    url: rootPath + "RulesScripts/Scripts/DeleteScript",
                                    dataType: "json",
                                    data: JSON.stringify({ ScriptId: $("#ScriptId").val() }),
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        if (data) {
                                            $.UifNotify("show",
                                                {
                                                    'type': "success",
                                                    'message': Resources.Language.DeletedSuccessfullyScript,
                                                    'autoclose': true
                                                });
                                            ControlScript.Functions.Form.Clear();
                                        } else {
                                            $.UifNotify("show", {
                                                'type': "danger", 'message': Resources.Language.SorryAProblem, 'autoclose': true
                                            });
                                        }
                                    },
                                    error: function (xhr, status) {
                                        $.UifNotify("show", {
                                            'type': "danger", 'message': Resources.Language.SorryAProblem, 'autoclose': true
                                        });
                                    },
                                })
                            }
                        });
                    }
                    else {
                        $.UifNotify("show", {
                            'type': "danger", 'message': Resources.Language.ErrorDeleteRecords, 'autoclose': true
                        });
                    }
                });
            },
            Export: function () {
                $("#btnExport").on("click", function () {
                    $.ajax({
                        type: "POST",
                        url: rootPath + "RulesScripts/Scripts/GenerateFileToExport",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.success) {
                                DownloadFile(data.result);
                            } else {
                                $.UifNotify("show", {
                                    'type': "danger", 'message': Resources.Language.SorryAProblem, 'autoclose': true
                                });
                            }
                        },
                        error: function (xhr, status) {
                            $.UifNotify("show", {
                                'type': "danger", 'message': Resources.Language.SorryAProblem, 'autoclose': true
                            });
                        },
                    })
                });
            },
            Execute: function () {
                $("#btnExecuteScript").on("click", function () {
                    $("#formScript").validate()
                    if ($("#formScript").valid()) {
                        if (ScriptComposite.Nodes.length != 0) {
                            if (ScriptComposite.Script == null) {
                                ScriptComposite.Script = new oScript($("#nameScript").val(), $("#selectLevelScript").UifSelect("getSelected"), $("#selectPackageScript").UifSelect("getSelected"), ScriptId);
                            }
                            ExecuteScript.ExecuteWithData(ScriptComposite, "ControlScript");
                        }
                    }
                })
            },
            MainQuestion: function () {
                $("#addPreguntaRama1").on("click", function () {
                    ControlScript.Functions.Questions.AddMain();
                })
            },
            Question: function () {
                $(".addQuestion").on("click", function () {
                    ControlScript.Functions.Questions.AddQuestion();
                });

                jQuery(document.body).on("click", ".agregarPregunta", function (event) {
                    ControlScript.Functions.Questions.AgregarPregunta(this);
                });
                jQuery(document.body).on("click", ".deletePregunta", function (event) {
                    ControlScript.Functions.Questions.DeletePregunta(this);
                });
            },
            Answer: function () {
                $(".addAnswer").click(function () {
                    ControlScript.Functions.Questions.AddAnswer();
                });
                jQuery(document.body).on("click", ".agregarRespuesta", function (event) {
                    ControlScript.Functions.Questions.AgregarRespuesta(this);
                });
                jQuery(document.body).on("click", ".deleteRespuesta", function (event) {
                    ControlScript.Functions.Questions.DeleteRespuesta(this);
                });
            },
            Save: function () {
                $("#btnRecord").click(function () {
                    ControlScript.Functions.Form.Save();
                });
            },
            Exit: function () {
                $("#btnExit").click(function () {
                    window.location = rootPath + "Home/Index";
                });
            },
        },
    },

    Functions: {
        Form: {
            Save: function () {
                $("#formScript").validate();
                if ($("#formScript").valid()) {
                    if (ScriptComposite.Nodes.length != 0) {
                        if (ScriptComposite.Script == null) {
                            ScriptComposite.Script = new oScript($("#nameScript").val(), $("#selectLevelScript").UifSelect("getSelected"), $("#selectPackageScript").UifSelect("getSelected"), ScriptId);
                        }
                        else {
                            ScriptComposite.Script.Description = $("#nameScript").val();
                            ScriptComposite.Script.PackageId = $("#selectPackageScript").UifSelect("getSelected");
                            ScriptComposite.Script.LevelId = $("#selectLevelScript").UifSelect("getSelected");
                        }

                        $.ajax({
                            type: "POST",
                            url: rootPath + "RulesScripts/Scripts/SaveQuestion",
                            data: JSON.stringify({
                                ScriptComposite: ScriptComposite
                            }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.success != false) {
                                    ScriptComposite = data;
                                    ControlScript.Functions.Control.Pintar();
                                    //ControlScript.Init.Components.ListView.Scripts();
                                    $.UifNotify("show",
                                        {
                                            'type': "success",
                                            'message': Resources.Language.MessageSavedSuccessfully,
                                            'autoclose': true
                                        });
                                    ControlScript.Functions.Form.Clear();
                                } else {
                                    $.UifNotify("show", {
                                        'type': "danger", 'message': Resources.Language.SorryAProblem, 'autoclose': true
                                    });
                                }
                            },
                            error: function (xhr, status) {
                                $.UifNotify("show", {
                                    'type': "danger", 'message': Resources.Language.SorryAProblem, 'autoclose': true
                                });
                            },
                        })
                    }
                    else {
                        $.UifNotify("show", {
                            'type': "danger", 'message': Resources.Language.EnterMinimumQuestion, 'autoclose': true
                        });
                    }
                }
            },
            Clear: function () {
                $("#ScriptId").val("");
                $("#nameScript").val("");
                $("#nameScript").focus();
                ControlScript.Init.Components.Selects.Package();
                ControlScript.Init.Components.Selects.Level();

                $("#selectPackageScript").UifSelect("disabled", false);
                $("#selectLevelScript").UifSelect("disabled", false);

                ScriptComposite = {
                    Nodes: []
                };
                $("#QuestionPanel").html("");
            },
            Assign: function (data) {
                ScriptId = data.ScriptId;
                $("#ScriptId").val(data.ScriptId);
                $("#nameScript").val(data.Description);
                $("#selectPackageScript").UifSelect("setSelected", data.PackageId);
                ControlScript.Init.Components.Selects.Level(data.PackageId, data.LevelId);

                $("#formScript").validate();
                $("#formScript").valid();

                $("#selectPackageScript").UifSelect("disabled", true);
                $("#selectLevelScript").UifSelect("disabled", true);

                ControlScript.Functions.GetScriptComposite(data.ScriptId);
                ControlScript.Functions.Control.Pintar();

            },
        },
        Modal: {
            ModalQuestion: {
                Open: function (title) {
                    $("#ModalPregunta").UifModal("showLocal", title);
                },
                Close: function () {
                    $("#ModalPregunta").UifModal("hide");
                },
            },
            ModalAnswer: {
                Open: function (title) {
                    $("#ModalRespuesta").UifModal("showLocal", title);
                },
                Close: function () {
                    $("#ModalRespuesta").UifModal("hide");
                },
            },
        },
        Questions: {
            AddMain: function () {
                NodoId = 1
                NewNodeId = false
                ScriptId = 0;
                ControlScript.Functions.Modal.ModalQuestion.Open(Resources.Language.AddQuestion);
                ControlScript.Init.Components.Selects.Question();
            },
            AddQuestion: function () {
                if ($("#selectQuestion").UifSelect("getSelected")) {
                    var Question = new oQuestion($("#selectQuestion").UifSelect("getSelected"), $("#selectQuestion").UifSelect("getSelectedText"));
                    Question.ConceptId = $("#selectQuestion").UifSelect("getSelectedSource").ConceptId;
                    Question.EntityId = $("#selectQuestion").UifSelect("getSelectedSource").EntityId;

                    if (ScriptComposite.Nodes.length !== 0 && !NewNodeId) {
                        var indexNode = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", NodoId);
                        var indexQuestion = SearchCombo.GetIndexObjects(ScriptComposite.Nodes[indexNode].Questions, "QuestionId", Question.QuestionId)
                        if (indexQuestion == -1) {
                            if (SearchCombo.GetIdMayor(ScriptComposite.Nodes[indexNode].Questions)) {
                                Question.OrdenNum = SearchCombo.GetIdMayor(ScriptComposite.Nodes[indexNode].Questions, "OrdenNum") + 1;
                            }
                            ScriptComposite.Nodes[indexNode].Questions.push(Question);
                        }
                        else {
                            $.UifNotify("show", {
                                'type': "info", 'message': Resources.Language.QuestionAlreadyExists, 'autoclose': true
                            });
                        }

                    } else {
                        if (ScriptComposite.Nodes.length !== 0) {
                            NodoId = SearchCombo.GetIdMayor(ScriptComposite.Nodes, "NodeId") + 1;
                        } else {
                            NodoId = 1;
                        }

                        var Node = new oNode(NodoId, [Question], ScriptId);
                        ScriptComposite.Nodes.push(Node);
                    }
                    ControlScript.Functions.Control.Pintar();
                    ControlScript.Functions.Modal.ModalQuestion.Close();
                }
            },
            AgregarPregunta: function (obj) {
                var pre = ($(obj).parents(".panel-collapse").attr("id")).split("-");
                var res = ($(obj).parents(".panel-heading").attr("id")).split("-");
                NodoId = Number(pre[1]);
                NodeIndex = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", NodoId);

                QuestionId = Number(pre[3]);
                PositionQuestion = ScriptComposite.Nodes[NodeIndex].Questions.indexOf(ScriptComposite.Nodes[NodeIndex].Questions.find(x => { return x.QuestionId == QuestionId }));

                let valueCode = res[7];
                PositionAnwer = SearchCombo.GetIndexObjects(ScriptComposite.Nodes[NodeIndex].Questions[PositionQuestion].Edges, "ValueCode", valueCode);

                NextNodeId = ScriptComposite.Nodes[NodeIndex].Questions[PositionQuestion].Edges[PositionAnwer].NextNodeId;
                if (NextNodeId != null) {
                    NodoId = NextNodeId;
                    NewNodeId = false;
                }
                else {
                    NodoId = SearchCombo.GetIdMayor(ScriptComposite.Nodes, "NodeId") + 1;
                    ScriptComposite.Nodes[NodeIndex].Questions[PositionQuestion].Edges[PositionAnwer].NextNodeId = NodoId;
                    NewNodeId = true;
                }

                ControlScript.Functions.Modal.ModalQuestion.Open(Resources.Language.AddQuestion);
                ControlScript.Init.Components.Selects.Question();
            },
            AddAnswer: function () {
                if ($("#selectAnswer").UifSelect("getSelected")) {
                    var nodeIndex = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", NodoId);
                    var indexQuestion = SearchCombo.GetIndexObjects(ScriptComposite.Nodes[nodeIndex].Questions, "QuestionId", QuestionId);

                    var Edge = new oEdge($("#selectAnswer").UifSelect("getSelectedText"), NodoId, QuestionId, $("#selectAnswer").UifSelect("getSelected"));
                    var indexEdge = SearchCombo.GetIndexObjects(ScriptComposite.Nodes[nodeIndex].Questions[indexQuestion].Edges, "ValueCode", Edge.ValueCode);

                    if (indexEdge == -1) {
                        ScriptComposite.Nodes[nodeIndex].Questions[indexQuestion].Edges.push(Edge);
                    }
                    else {
                        $.UifNotify("show", {
                            'type': "info", 'message': Resources.Language.AnswerAlreadyExists, 'autoclose': true
                        });
                    }
                    ControlScript.Functions.Control.Pintar();
                    ControlScript.Functions.Modal.ModalAnswer.Close();
                }
            },
            AgregarRespuesta: function (obj) {
                $("#selectAnswer").UifSelect({
                    source: null
                });
                var res = ($(obj).parents(".panel-heading").attr("id")).split("-");
                NodoId = Number(res[1]);
                QuestionId = Number(res[3]);

                var question = null;
                $.ajax({
                    type: "POST",
                    url: rootPath + "Scripts/GetQuestionControl?QuestionId=" + QuestionId,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        Question = data;
                    }
                })

                SearchCombo.obtencionTipoControl(Question.ConceptId, Question.EntityId, "#selectAnswer")
                    .then(tipoControl => {
                        if (tipoControl.ConceptControlCode == 4 || tipoControl.ConceptControlCode == 5) {
                            ControlScript.Functions.Modal.ModalAnswer.Open(Resources.Language.AddReply);
                        }
                    });
            },
            DeleteNode(nodeId) {
                let nodeIndex = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", nodeId);
                if (nodeIndex !== -1) {
                    if (ScriptComposite.Nodes[nodeIndex].Questions) {
                        ScriptComposite.Nodes[nodeIndex].Questions.forEach((item, indexQuestion) => {
                            ControlScript.Functions.Questions.DeleteQuestion(nodeId, indexQuestion, item);
                        });
                    }

                    nodeIndex = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", nodeId);
                    ScriptComposite.Nodes.splice(nodeIndex, 1);
                }
            },
            DeleteQuestion(nodeId, indexQuestion, questionToDelete) {
                if (questionToDelete.Edges) {
                    questionToDelete.Edges.forEach(item => {
                        ControlScript.Functions.Questions.DeleteNode(item.NextNodeId);
                    });
                }

                var nodeIndex = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", nodeId);
                ScriptComposite.Nodes[nodeIndex].Questions.splice(indexQuestion, 1);
            },
            DeleteAnswer() {

            },
            DeleteRespuesta: function (obj) {
                var pre = ($(obj).parents(".panel-collapse").attr("id")).split("-");
                var res = ($(obj).parents(".panel-heading").attr("id")).split("-");

                NodoId = Number(pre[1]);
                PositionQuestion = Number(pre[3]) - 1;
                PositionAnwer = Number(res[3]) - 1;

                NodeIndex = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", NodoId);
                NextNodeId = ScriptComposite.Nodes[NodeIndex].Questions[PositionQuestion].Edges[PositionAnwer].NextNodeId;

                ScriptComposite.Nodes[NodeIndex].Questions[PositionQuestion].Edges.splice(PositionAnwer, 1);
                ControlScript.Functions.Control.Pintar();
            },
            DeletePregunta: function (obj) {
                var res = ($(obj).parents(".panel-heading").attr("id")).split("-");
                var indexNode = SearchCombo.GetIndexObjects(ScriptComposite.Nodes, "NodeId", Number(res[1]));
                var indexQuestion = SearchCombo.GetIndexObjects(ScriptComposite.Nodes[indexNode].Questions, "QuestionId", Number(res[3]));
                var questionToDelete = ScriptComposite.Nodes[indexNode].Questions[indexQuestion];

                ControlScript.Functions.Questions.DeleteQuestion(Number(res[1]), indexQuestion, questionToDelete);
                if (ScriptComposite.Nodes.length === 1) {
                    if (ScriptComposite.Nodes[0].Questions.length === 0) {
                        ScriptComposite.Nodes.splice(0, 1);
                    }
                }

                ControlScript.Functions.Control.Pintar();
            }
        },
        Control: {
            Pintar: function () {
                htmlScript = "";
                if (ScriptComposite.Nodes.length !== 0) {
                    $("#QuestionPanel").html(this.CreatePanelGroup(1, this.ArmarControl(ScriptComposite.Nodes, 1, "")));
                } else {
                    $("#QuestionPanel").html("");
                }
            },
            CreatePanelGroup: function (idPanelGroup, contenido) {
                var control = "";
                control += '<div class="panel-group" id="pgPregunta' + idPanelGroup + '" role="tablist" aria-multiselectable="true">';
                control += contenido;
                control += "</div>";
                return control;
            },
            ArmarControl: function (nodes, nodeId) {
                var questions = nodes.find(x => { return x.NodeId === nodeId })
                    .Questions.sort((a, b) => {
                        return a.OrdenNum > b.OrdenNum ? 1 : 0;
                    });

                $.each(questions, function (index, value) {
                    var IdPregunta = "NodoId-" + nodeId + "-QuestionId-" + this.QuestionId + "-OrdenNum-" + this.OrdenNum;
                    htmlScript += ControlScript.Functions.Control.CrearPanel(nodeId, IdPregunta, IdPregunta, this.Description, true);

                    $.each(this.Edges, function (ind, val) {
                        //Respuesta  
                        var IdResuesta = "NodoId-" + nodeId + "-QuestionId-" + value.QuestionId + "-OrdenNum-" + value.OrdenNum + "-ValueCode-" + this.ValueCode;
                        htmlScript += '<div class="panel-group" id="pgRespuesta' + nodeId + '" role="tablist" aria-multiselectable="true">';
                        htmlScript += ControlScript.Functions.Control.CrearPanel(nodeId, IdResuesta, IdResuesta, this.Description, false);

                        if (this.NextNodeId != null) {
                            ControlScript.Functions.Control.ArmarControl(nodes, this.NextNodeId);
                        }
                        htmlScript += "</div></div></div>";
                        htmlScript += "</div>";
                    })
                    htmlScript += "</div></div></div>";
                })
                return htmlScript
            },
            CrearPanel: function (idPanelGroup, question, answer, title, respuesta) {
                var formulario = "";

                formulario += '<div class="panel panel-default">';
                formulario += ' <div class="panel-heading" role="tab" id="heading' + question + '">';
                formulario += '     <h4 class="panel-title">' + title + "</h4>";
                formulario += '     <div style="float: right;margin-top: -22px;">'
                formulario += '         <button type="button" data-toggle="collapse" data-parent="#pg' + idPanelGroup + '" href="#Body' + answer + '" aria-controls="Body' + answer + '" aria-expanded="true" class="btn btn-default">';
                formulario += '             <span class="glyphicon glyphicon-chevron-down"></span>';
                formulario += "         </button>";
                if (respuesta) {
                    formulario += '         <button type="button" class="btn btn-primary agregarRespuesta">';
                    formulario += '             <span class="glyphicon glyphicon-plus"></span>';
                    formulario += "         </button>";

                    formulario += '         <button type="button" class="btn btn-default deletePregunta">';
                    formulario += '             <span class="glyphicon glyphicon-trash"></span>';
                    formulario += "         </button>";

                }
                else {
                    formulario += '         <button type="button" class="btn btn-primary agregarPregunta">';
                    formulario += '             <span class="glyphicon glyphicon-plus"></span>';
                    formulario += "         </button>";

                    //formulario += '         <button type="button" class="btn btn-default deleteRespuesta">';
                    //formulario += '             <span class="glyphicon glyphicon-trash"></span>';
                    //formulario += '         </button>';
                }

                formulario += "  </div>";
                formulario += " </div>";
                formulario += ' <div id="Body' + answer + '" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading' + question + '">';
                formulario += '     <div class="panel-body">';
                return formulario;
            },
        },
        GetScriptComposite: function (scriptId) {
            htmlScript = "";
            $.ajax({
                type: "POST",
                url: rootPath + "Scripts/GetScriptComposite?ScriptId=" + scriptId
            })
                .done(data => {
                    if (data.success == false) {
                        $.UifNotify("show", { 'type': "danger", 'message': data.result.ErrorMsg, 'autoclose': true });
                    } else {
                        ScriptComposite = data
                    }
                })
                .fail(() => {
                    $.UifNotify("show", { 'type': "danger", 'message': "Error consultado guion", 'autoclose': true });
                })
        },
    }
}