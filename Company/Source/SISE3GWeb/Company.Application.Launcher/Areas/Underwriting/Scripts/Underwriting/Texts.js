//Codigo de la pagina Texts.cshtml
class UnderwritingText extends Uif2.Page {
    getInitialState() {
        $('#Observations').show();
        $('#Records').hide();
    }

    //Seccion Eventos
    bindEvents() {
        $("#btnText").click(this.TextLoad);
        $("#inputTextPrecataloged").on('buttonClick', UnderwritingText.SearchText);
        $("#btnTextSave").click(UnderwritingText.SaveTexts);
        $('#tableTextResults tbody').on('click', 'tr', this.SelectSearchText);
        $("#btnTextCancel").on("click", this.TextCancel);
        $("#btnTextSelect").click(this.TextSelect);       
        $('#inputTextRecord').prop('disabled', true);
        UnderwritingText.LoadRecordObservation();
    }

    TextLoad() {
        $('#inputTextPrecataloged').val('');
        if (glbPolicy.Id == 0 && glbPolicy.TemporalType != TemporalType.Quotation) {
            if ($("#formUnderwriting").valid()) {
                Underwriting.SaveTemporalPartial(MenuType.Texts);    
            }
            UnderwritingText.LoadPartialText();
        }
        else {
            UnderwritingText.LoadPartialText();
        }
    }

    static SearchText() {

        if ($.trim($("#inputTextPrecataloged").val()) != "") {
            UnderwritingText.GetTextsByNameLevelIdConditionLevelId($("#inputTextPrecataloged").val(), glbPolicy.Prefix.Id);
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
        else {
            $("#inputText").val($("#inputText").val() + ' \n ' + $("#inputTextEdit").val());
        }
        $('#modalTextSearch').UifModal("hide");
    }

    static LoadPartialText() {

        $("#formUnderwriting").validate();

        if (glbPolicy.Id > 0 && $("#formUnderwriting").valid()) {
            Underwriting.ShowPanelsIssuance(MenuType.Texts)

            if (glbPolicy.Text != undefined) {
                $("#inputText").val(glbPolicy.Text.TextBody);
                $("#inputTextObservations").val(glbPolicy.Text.Observations);
            }
            else {
                $("#inputText").val('');
                $("#inputTextObservations").val('');
            }
        }
    }

    static GetTextsByNameLevelIdConditionLevelId(name, conditionLevelId) {
        TextRequest.GetTextsByNameLevelIdConditionLevelId(name, Levels.General, conditionLevelId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    UnderwritingText.ShowTextList(data.result);
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

            var obj = $("#formTexts").serializeObject();

            TextRequest.SaveTexts(glbPolicy.Id, obj).done(function (data) {
                if (data.success) {
                    Underwriting.HidePanelsIssuance(MenuType.Texts);
                    glbPolicy.Text = data.result;
                    Underwriting.LoadSubTitles(4);
                    if (glbPolicy.TemporalType == TemporalType.Quotation) {
                        Underwriting.SaveTemporal(false);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTexts, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTexts, 'autoclose': true });
            });
        }
    }

    static ShowTextList(dataTable) {
        $('#tableTextResults').UifDataTable('clear');
        $('#tableTextResults').UifDataTable('addRow', dataTable);
        $('#modalTextSearch').UifModal('showLocal', AppResources.LabelSelectText);
        $('#inputTextEdit').val('');
    }

    static LoadRecordObservation() {
        if (glbEndorsementTexts != undefined && glbEndorsementTexts.length > 0) {
            var recordText = "";
            glbEndorsementTexts.forEach(function (item) {
                recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
            });
            $('#inputTextRecord').val(recordText);
            $('#Records').show();
        }
        else {
            $('#Records').hide();
        }
    }
}