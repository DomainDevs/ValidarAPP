$.ajaxSetup({ async: false });
var objLoadFile = {
    Init: $(function () {
        jQuery.ajaxSetup({ async: true });
        objLoadFile.Events.Buttons();
        objLoadFile.Events.FileUploads.Xml();
        objLoadFile.Events.FileUploads.Xls();
        objLoadFile.Events.TextsArea.Xls();
        objLoadFile.Events.TextsArea.Xml();
    }),

    Events: {
        Buttons: function () {
            $("#btnExit").on("click", function () {
                $.redirect(rootPath + 'RulesScripts/DecisionTable/Index');
            });
            $("#btnLoad").on("click", function () {
                objLoadFile.Functions.SendLoadDesicionTable();
            });
        },

        FileUploads: {
            Xml: function () {
                $("#LoadFileXML").fileupload({
                    url: rootPath + "RulesScripts/DecisionTable/LoadPreviewXMLFile",
                    done: function (e, data) {
                        if (data._response.result.success) {
                            $("#TxtPreviewXML").text(data._response.result.result[1]);
                            $("#nameFileXML").val(data._response.result.result[0]);
                            objLoadFile.Events.FileUploads.Xls();
                        }
                        else {
                            $.UifNotify("show", { 'type': "info", 'message': data._response.result.result, 'autoclose': true });
                        }
                    }
                });
            },
            Xls: function () {
                $("#divPreviewXls").html('<input type="hidden" id="nameFileXLS" value="" /><textarea rows="25" readonly></textarea>');
                $("#nameFileXLS").val("");
                $("#LoadFileXLS").fileupload({
                    formData: { nameXml: $("#nameFileXML").val() },
                    url: rootPath + 'RulesScripts/DecisionTable/LoadPreviewXLSFile',
                    done: function (e, data) {
                        if (data._response.result.success == undefined) {
                            $("#divPreviewXls").html(data._response.result);
                            objLoadFile.Events.TextsArea.Xls();
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data._response.result.result, 'autoclose': true });
                        }
                    }
                });
            }
        },

        TextsArea: {
            Xml: function () {
                $("#TxtPreviewXML").dblclick(function () {
                    if ($("#TxtPreviewXML").text().length != 0) {
                        objLoadFile.Events.Modal.PreviewFile($("#TxtPreviewXML"));
                    }
                });
            },
            Xls: function () {
                $("#TxtPreviewXLS").dblclick(function () {
                    if ($("#TxtPreviewXLS").text().length != 0) {
                        objLoadFile.Events.Modal.PreviewFile($("#TxtPreviewXLS"));
                    }
                });
            },
        },

        Modal: {
            PreviewFile: function (element) {
                $('#ModalPreviewFile').UifModal('showLocal', '');
                $('#ModalContent').html(element[0].outerHTML)
            }
        }
    },

    Functions: {
        SendLoadDesicionTable: function () {
            var pathXml = $("#nameFileXML").val();
            var pathXls = $("#nameFileXLS").val();

            if (pathXml != "" && pathXls != "") {
                $.ajax({
                    type: "POST",
                    url: rootPath + "RulesScripts/DecisionTable/PreviewXLSFile",
                    data: { "pathXml": pathXml, "pathXls": pathXls, "save": true },
                    beforeSend: function myfunction() {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.AwaitProcessLoadExcelDT, 'autoclose': true });
                        $("#btnLoad").attr("disabled", "disabled");
                    },
                    success: function (data) {
                        if (data.success) {
                            var str = Resources.Language.WereCreated + " " +
                                data.result.CountRules + " " +
                                Resources.Language.Records + "," +
                                data.result.CountCondition + " " + Resources.Language.LabelConditions + "," +
                                data.result.CountActions + " " + Resources.Language.Actions;

                            $.UifDialog('alert', {
                                title: Resources.Language.SuccessfulLoading,
                                message: str
                            }, function (result) {
                                $.redirect(rootPath + 'RulesScripts/DecisionTable/BodyTableDecision', data.result.RuleBase);
                            });

                        } else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                        $("#btnLoad").removeAttr("disabled");
                    }
                });
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.FilesRequired, 'autoclose': true });
            }
        }
    }
}