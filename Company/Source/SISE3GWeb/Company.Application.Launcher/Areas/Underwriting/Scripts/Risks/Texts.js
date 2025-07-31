//Codigo de la pagina Texts.cshtml
class RiskText extends Uif2.Page {
    getInitialState() {
        $('#Observations').hide();
        $('#Records').hide();
    }

    bindEvents() {
     
        $("#btnTexts").on('click', this.TextsLoad);
        $("#inputTextPrecataloged").on('buttonClick', this.SearchTextPrecataloged);        
        $("#btnTextSave").on("click",RiskText.SaveTexts);
        $('#tableTextResults tbody').on('click', 'tr', this.SelectSearchText);
        $("#btnTextCancel").on("click", this.TextCancel);
        $("#btnTextSelect").on("click", this.TextSelect);
      
    }

    TextsLoad() {
        $('#inputTextPrecataloged').val('');
        RiskText.LoadPartialText();
    }

    SearchTextPrecataloged() {
        if ($.trim($("#inputTextPrecataloged").val()) != "") {
            RiskText.GetTextsByNameLevelIdConditionLevelId($("#inputTextPrecataloged").val().trim(), glbPolicy.Product.CoveredRisk.CoveredRiskType);
        }
    }

    SelectSearchText(e) {
        e.preventDefault();
        $("#inputTextPrecataloged").val($(this).children()[1].innerHTML);
        $("#inputTextEdit").val($(this).children()[2].innerHTML);
    }

    TextCancel() {
        $('#modalTextSearch').UifModal("hide");
    }

    TextSelect() {
        if ($("#inputText").val().trim().length == 0) {
            $("#inputText").val($("#inputTextEdit").val());
        }
        else if ($("#inputText").val().trim().length > 0) {
            $("#inputText").val($("#inputTextEdit").val());
        }
        else {
            $("#inputText").val($("#inputText").val() + ' \n ' + $("#inputTextEdit").val());
        }
        $('#modalTextSearch').UifModal("hide");
    }

    static LoadPartialText() {

        $(glbRisk.formRisk).validate();

        if (glbRisk.Id == 0) {
            if (glbRisk.Class == undefined) {
                window[glbRisk.Object].SaveRisk(MenuType.Texts, 0, false);
            }
            else {
                glbRisk.Class.SaveRisk(MenuType.Texts, 0, false);
            }

        }

        if (glbRisk.Id > 0) {


            if (glbRisk.Class == undefined) {
                window[glbRisk.Object].ShowPanelsRisk(MenuType.Texts);
            }
            else {
                glbRisk.Class.ShowPanelsRisk(MenuType.Texts);
            }

            if (glbRisk.Text != undefined) {
                $("#inputText").val(glbRisk.Text.TextBody);
            }
            else {
                $("#inputText").val('');
            }
        }
    }

    static GetTextsByNameLevelIdConditionLevelId(name, conditionLevelId) {
        TextRequest.GetTextsByNameLevelIdConditionLevelId(name, Levels.Risk, conditionLevelId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    RiskText.ShowTextList(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchTexts, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorLoadingTexts, 'autoclose': true });
            }
        });
    }

    static SaveTexts() {

        $("#formTexts").validate();

        if ($("#formTexts").valid()) {
            //El riesgo del texto original sólo cambia añadiendo nuevo texto-riesgo debajo
            if (glbRisk.Text != null) {
                var text = $("#inputText").val();
                $("#inputText").val(text);

                //--WILLIAM SANCHEZ
                //Descartar esta validacion porque incluye el texto original y el nuevo para guardar
                //if (text.replace(/[^a-zA-Z ]/g, "") != glbRisk.Text.TextBody.replace(/[^a-zA-Z ]/g, "")) {
                //    var textConcat = ($("#inputText").val() != "" && glbRisk.Text != null) ? glbRisk.Text.TextBody + "\n\n" + $("#inputText").val() : $("#inputText").val();
                //    $("#inputText").val(textConcat);
                //}
            }         

            var obj = $("#formTexts").serializeObject();

            TextRequest.SaveTextsRisk(glbRisk.Id, $("#formTexts").serializeObject(), riskController).done(function (data) {
                if (data.success) {
                    if (glbRisk.Class == undefined) {
                        window[glbRisk.Object].ShowPanelsRisk(MenuType.Risk);
                        glbRisk.Text = data.result;
                        window[glbRisk.Object].LoadSubTitles(2);
                    }
                    else {
                        glbRisk.Class.ShowPanelsRisk(MenuType.Risk);
                        glbRisk.Text = data.result;
                        glbRisk.Class.LoadSubTitles(2);
                    }

                    $("#modalTexts").UifModal('hide');

                }
                else {
                    $.UifNotify('show', { 'type': 'error', 'message': AppResources.ErrorSaveTexts, 'autoclose': true });
                }
            });
        }
    }

    static ShowTextList(dataTable) {
        $('#tableTextResults').UifDataTable('clear');
        $('#tableTextResults').UifDataTable('addRow', dataTable);
        $('#modalTextSearch').UifModal('showLocal', AppResources.LabelSelectText);
        $('#inputTextEdit').val('');
    }
}
