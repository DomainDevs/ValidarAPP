class DecisionTableFile extends Uif2.Page {
    getInitialState() {
        $("#loadFileXML").fileupload({
            url: rootPath + "RulesAndScripts/DecisionTable/LoadPreviewXmlFile",
            done: this.LoadPreviewXmlFile
        });
        $("#loadFileXLS").fileupload({
            formData: { nameXml: $("#nameFileXML").val() },
            url: rootPath + "RulesAndScripts/DecisionTable/LoadPreviewXlsFile",
            done: this.LoadPreviewXlsFile
        });

        $("#loadFileXML").UifFileInput({ input: false });
        $("#loadFileXLS").UifFileInput({ input: false });
    }

    bindEvents() {
        $("#btnBack").on("click", this.Back);
        $("#btnLoad").on("click", this.SendLoadDesicionTable);
    }

    Back() {
        glbConcepts = null;
        glbDecisionTable = null;
        router.run("#prtDecisionTable");
    }


    LoadPreviewXlsFile(e, data) {
        if (data._response.result.success === undefined) {
            $("#divPreviewXls").html(data._response.result);
        }
        else {
            if (data._response.result.success === true) {
                $.UifNotify("show", { 'type': "success", 'message': data._response.result.result, 'autoclose': true });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data._response.result.result, 'autoclose': true });               
            }
        }
    }

    LoadPreviewXmlFile(e, data) {
        if (data._response.result.success) {
            $("#divPreviewXls").html('<input type="hidden" id="nameFileXLS" value="" /><textarea rows="25" readonly></textarea>');

            $("#txtPreviewXML").text(data._response.result.result[1]);
            $("#nameFileXML").val(data._response.result.result[0]);

            $("#loadFileXLS").fileupload({
                formData: { nameXml: $("#nameFileXML").val() },
                url: rootPath + "RulesAndScripts/DecisionTable/LoadPreviewXlsFile",
                done: DecisionTableFile.LoadPreviewXlsFile
            });
        }
        else {
            $.UifNotify("show", { 'type': "info", 'message': data._response.result.result, 'autoclose': true });
        }
    }

    static LoadPreviewXlsFile(e, data) {
        if (data._response.result.success === undefined) {
            $("#divPreviewXls").html(data._response.result);
        }
        else {
            if (data._response.result.success === true) {
                $.UifNotify("show", { 'type': "success", 'message': data._response.result.result, 'autoclose': true });
            } else {
                if (data.result.result.url!=null && data.result.result.url.length>0) {
                    $.UifNotify("show", { 'type': "danger", 'message': "Archivo excel con errores", 'autoclose': true });
                    DecisionTableFile.DecisionTableWithErrors(data.result.url, data.result.FileName);
                }
                else
                {
                    $.UifNotify("show", { 'type': "danger", 'message': data._response.result.result, 'autoclose': true });
                }
            }
        }
    }


    SendLoadDesicionTable() {
        const pathXml = $("#nameFileXML").val();
        const pathXls = $("#nameFileXLS").val();

        if (pathXml !== "" && pathXls !== "") {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.AwaitProcessLoadExcelDT, 'autoclose': false });
            $("#btnLoad").attr("disabled", "disabled");

            RequestDecisionTable.PreviewXlsFile(pathXml, pathXls, true, GetQueryParameter("IsEvent"))
                .done((data) => {
                    if (data.success) {
                        const str = Resources.Language.WereCreated + " " +
                            data.result.CountRules + " " +
                            Resources.Language.Records + "," +
                            data.result.CountCondition + " " + Resources.Language.LabelConditions + "," +
                            data.result.CountActions + " " + Resources.Language.Actions;

                        $.UifDialog("alert", {
                            title: Resources.Language.SuccessfulLoading,
                            message: str
                        }, () => {
                            glbDecisionTable = data.result.RuleBase;
                            router.run("#prtDecisionTableData");
                            });
                        $.UifNotify("close")
                    }                    
                    else if (!data.success) {                        
                        if (data.result.url !== "" && data.result.url !== undefined)
                        {
                            $.UifNotify("update", { 'type': "danger", 'message': "Archivo excel con errores", 'autoclose': true });
                            DecisionTableFile.DecisionTableWithErrors(data.result.url, data.result.FileName);
                        }
                        else
                        {
                            $.UifNotify("update", { 'type': "danger", 'message': data.result, 'autoclose': true });
                        }
                        
                    }
                    $("#btnLoad").removeAttr("disabled");
                });
        }
        else {
            $.UifNotify("update", { 'type': "danger", 'message': Resources.Language.FilesRequired, 'autoclose': true });
        }
    }

    static DecisionTableWithErrors(url, FileName) {
        var a = document.createElement('A');
        a.href = url;
        a.download = FileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
       // DownloadFile(url);
    }
}