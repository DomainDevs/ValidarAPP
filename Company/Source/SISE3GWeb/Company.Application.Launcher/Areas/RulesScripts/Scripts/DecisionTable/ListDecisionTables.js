$.ajaxSetup({ async: false });
ControlTableDecision = {
    CargaInicial: (function () {
        $('#btnPost').hide()
    }()),
    variables: {
    },
    DecisionTable: {
        ListDecisionTable: function (data) {
            if (data) {

                $("#DecisionTablelist").UifListView({
                    delete: true,
                    deleteCallback: (
                        function (deferred, data) {
                            $.ajax({
                                type: "POST",
                                url: rootPath + "DecisionTable/DeleteTableDecision?Id=" + data.RuleBaseId,
                                dataType: "json",
                                contentType: "application/json; charset=utf-8"
                            }).done(function (data) {

                                if (data.success) {
                                    deferred.resolve();
                                } else {
                                    deferred.reject();
                                }

                            });
                        }),
                    sourceData: data,
                    displayTemplate: "#template-DecisionTablelist",
                    selectionType: 'single',
                    customDelete: false
                });
            }
            else {
                $("#DecisionTablelist").UifListView({
                    delete: true,
                    deleteCallback: (

                        function (deferred, data) {
                            $.ajax({
                                type: "POST",
                                url: rootPath + "DecisionTable/DeleteTableDecision?Id=" + data.RuleBaseId,
                                dataType: "json",
                                contentType: "application/json; charset=utf-8"
                            }).done(function (data) {

                                if (data.success) {
                                    deferred.resolve();
                                } else {
                                    deferred.reject();
                                }

                            });
                        }),
                    source: rootPath + "DecisionTable/GetDecisionTablelist",
                    displayTemplate: "#template-DecisionTablelist",
                    selectionType: 'single',
                    customDelete: false
                });
            }
        },

        DecisionTableSeleccionar: function () {

            var RuleBase = $("#DecisionTablelist").UifListView("getSelected");
            if (RuleBase.length != 0) {
                return RuleBase[0];
                $('#btnPost').show();
            }
            $('#btnPost').hide();
            return null;
        },
        DecisionTableEditarCabecera: (function () {
            $("#Modificar").click(function () {
                var RuleBase = ControlTableDecision.DecisionTable.DecisionTableSeleccionar();
                if (RuleBase != null) {
                    $.redirect(rootPath + 'RulesScripts/DecisionTable/HeadTableDecision', RuleBase);
                } else {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.SelectItem, 'autoclose': true });
                }
            })
        }()),
        DecisionTableEditarCuerpo: (function () {
            $("#ModificarDatos").click(function () {
                var RuleBase = ControlTableDecision.DecisionTable.DecisionTableSeleccionar();
                if (RuleBase != null) {
                    $.redirect(rootPath + 'RulesScripts/DecisionTable/BodyTableDecision', RuleBase);
                } else {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.SelectItem, 'autoclose': true });
                }
            })
        }()),

        LoadFromFile: function () {
            $("#LoadFromFile").click(function () {
                $.redirect(rootPath + 'RulesScripts/DecisionTable/LoadFromFile');
            })
        }(),
    },
    Button: {
        CargarExcel: (function () {
        }()),
        Publicar: (function () {
        }()),
        Volver: (function () {
            $('#btnExit').click(function () {
                window.location.href = rootPath + "Home/Index";
            });
        }()),
        DecisionTableAdd: (function () {
            $("#btnNewTable").click(function () {
                RuleBase = new oRuleBase();
                $.redirect(rootPath + "RulesScripts/DecisionTable/HeadTableDecision", RuleBase);
            })
        }()),
        ExportDecisionTable: (function () {
            $("#btnExportDecisionTable").on("click", function () {
                if (true) {
                    var RuleBase = ControlTableDecision.DecisionTable.DecisionTableSeleccionar();
                    if (RuleBase != null) {
                        $.redirect(rootPath + 'RulesScripts/DecisionTable/ExportDecisionTable/', RuleBase);
                    } else {
                        $.UifNotify('show', { 'type': 'warning', 'message': Resources.Language.SelectItem, 'autoclose': true });
                    }
                }
            });
        }()),
    },
}

ControlTableDecision.DecisionTable.ListDecisionTable();