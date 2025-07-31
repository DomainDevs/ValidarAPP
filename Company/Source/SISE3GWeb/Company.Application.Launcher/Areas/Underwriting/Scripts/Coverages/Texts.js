//Codigo de la pagina Texts.cshtml
class TextsCoverage extends Uif2.Page {

    getInitialState() {
        $('#Observations').hide();
        $('#Records').hide();
    }

    bindEvents() {
        $('#btnTextCoverage').on('click', this.TextCoverage);
        $('#inputTextPrecataloged').on('buttonClick', this.TextPrecataloged);
        $('#btnTextSave').on('click',TextsCoverage.SaveTexts);
        $('#tableTextResults tbody').on('click', 'tr', this.TextResults);
        $('#btnTextCancel').on('click', this.TextCancel);
        $('#btnTextSelect').on('click', this.TextSelect);
        $("#inputText").TextTransform(ValidatorType.UpperCase)
    }

    TextCoverage() {
        $('#inputTextPrecataloged').val('');
        TextsCoverage.LoadPartialText();
    }

    TextPrecataloged() {
        if ($.trim($('#inputTextPrecataloged').val()) != '') {
            TextsCoverage.GetTextsByNameLevelIdConditionLevelId($('#inputTextPrecataloged').val().trim(), glbCoverage.CoverageId);
        }
    }

    TextResults(e) {
        e.preventDefault();
        $('#inputTextPrecataloged').val($(this).children()[1].innerHTML);
        $('#inputTextEdit').val($(this).children()[2].innerHTML);
    }

    TextCancel() {
        $('#modalTextSearch').UifModal('hide');
    }

    TextSelect() {
        if ($('#inputText').val().trim().length == 0) {
            $('#inputText').val($('#inputTextEdit').val());
        }
        else {
            $('#inputText').val($('#inputText').val() + ' \n ' + $('#inputTextEdit').val());
        }
        $('#modalTextSearch').UifModal('hide');
    }

    static LoadPartialText() {
        if ($('#selectCoverage').val() > 0) {

            if (glbCoverage.Class == undefined) {
                window[glbCoverage.Object].ShowPanelsCoverage(MenuType.Texts);
            }
            else {
                glbCoverage.Class.ShowPanelsCoverage(MenuType.Texts);
            }

            if (coverageText != null) {
                $('#inputText').val(coverageText.TextBody);
            }
            else {
                $('#inputText').val('');
            }

           var  selectCoverage = $('#selectCoverage').val();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
        }
    }

    static GetTextsByNameLevelIdConditionLevelId(name, conditionLevelId) {
        TextRequest.GetTextsByNameLevelIdConditionLevelId(name, Levels.Coverage, conditionLevelId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    TextsCoverage.ShowTextList(data.result);
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

        $('#formTexts').validate();
        if ($('#formTexts').valid()) {
            coverageText = $('#formTexts').serializeObject();
            if (glbCoverage.Class == undefined) {
                window[glbCoverage.Object].ShowPanelsCoverage(MenuType.Coverage);
            }
            else {
                glbCoverage.Class.ShowPanelsCoverage(MenuType.Coverage);
            }
            $('#modalTexts').UifModal('hide');
        }
    }

    static ShowTextList(dataTable) {
        $('#tableTextResults').UifDataTable('clear');
        $('#tableTextResults').UifDataTable('addRow', dataTable);
        $('#modalTextSearch').UifModal('showLocal', AppResources.LabelSelectText);
        $('#inputTextEdit').val('');
    }
}