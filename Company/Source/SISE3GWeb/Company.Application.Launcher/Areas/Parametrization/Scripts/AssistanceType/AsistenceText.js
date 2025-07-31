var glbAssistanceText = {};
var AssistanceTextNew = {};
var assistanceTextIndex = null;
var ClauseCode;
var TextId;

class AssistanceTextRequest {

    static GetClauses() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AssistanceType/GetClauses',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetAssistanceText(AssistanceCd) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AssistanceType/GetAssistanceText',
            data: JSON.stringify({ AssistanceCd: AssistanceCd }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class AsistenceText extends Uif2.Page {
    getInitialState() {

    }

    bindEvents() {
        $("#inputTextClause").on("buttonClick", AsistenceText.LoadClause);
        $('#btnAcceptAssistanceText').on('click', this.addItemAssistance);
        $('#btnNewAssistanceText').on('click', AsistenceText.clearPanel);
        $('#listViewAssistanceText').on('rowEdit', AsistenceText.showData);
        $("#btnCloseText").click(this.CancelView);
        $('#btnAcceptText').on('click', this.saveAssistanceText);
    }

    static loadAssistanceText(AssistanceText) {
        $("#listViewAssistanceText").UifListView({ source: null, displayTemplate: "#AssistanceTextTemplate", edit: true, delete: false, customEdit: true, customDelete: false, height: 300 });
        AssistanceText.forEach(item => {
            item.Status = 'Add';
            $("#listViewAssistanceText").UifListView("addItem", item);
        });
    }

    static LoadClause() {
        var clause = {};
        $('#inputTextClause').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        var description = $("#inputTextClause").val()
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/AssistanceType/GetClausesByIdBydescription',
            data: JSON.stringify({ description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                clause = data.result[0];
                if (data.result.length > 0) {
                    $("#inputTextClause").data("Object", clause);
                    $("#inputTextClause").val(clause.Name + ' (' + clause.Id + ')');
                    ClauseCode = clause.Id;
                }
                else {
                    $("#inputTextClause").val('');
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageSearchClauses, 'autoclose': true });
                }
            }
           
        }).fail(function (xhr, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorConsultingAssistanceText, 'autoclose': true });
        });


    }

    static clearPanel() {
        assistanceTextIndex = null;
        TextId = null;
        ClauseCode = null;
        $("#inputDescriptionText").val('');
        $("#inputTextClause").val('');
        $("#rdEnabledText").is("checked", false);
    }

    static showData(event, result, index) {
        AsistenceText.clearPanel();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }

        //if (result.AssistanceTextId != undefined) {
            assistanceTextIndex = index;
            TextId = result.AssistanceTextId;
            $("#inputDescriptionText").val(result.Text);
            $("#rdEnabledText").prop("checked", result.Enable);
            $("#inputTextClause").val(result.ClauseDescription);
            $("#ddlBusinessType").UifSelect("setSelected", result.PrefixCd);
            $("#ddlBusinessType").UifSelect("disabled", true);
            ClauseCode = result.ClauseCd3G;
        //}
    }

    addItemAssistance() {
        $("#formAssistanceText").validate();
        if ($("#formAssistanceText").valid()) {
            AssistanceTextNew = {};

            AssistanceTextNew.AssistanceTextId = TextId;

            AssistanceTextNew.Text = $("#inputDescriptionText").val();
            AssistanceTextNew.Enable = $("#rdEnabledText").is(":checked");
            AssistanceTextNew.ClauseDescription = $("#inputTextClause").UifSelect("getSelected");
            AssistanceTextNew.EnabledDescription = $("#rdEnabledText").is(":checked") == true ? Resources.Language.LabelEnabled : Resources.Language.Disabled;
            if (ClauseCode === undefined || ClauseCode === null) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSelectClause, 'autoclose': true });
                $("#inputTextClause").val('');
                return false;
            }
            AssistanceTextNew.ClauseCd3G = ClauseCode;
            AssistanceTextNew.AssistanceCd = parseInt($("#inputAssistanceCode").val());
            AssistanceTextNew.PrefixCd = $("#ddlBusinessType").UifSelect("getSelected");
            AssistanceTextNew.ClauseCd2G = 0;

            AssistanceTextNew.Status = 'Modified';

            if (assistanceTextIndex == null) {
                var ifExist = $("#listViewAssistanceText").UifListView('getData').filter(function (item) {
                    return item.Text.toUpperCase() == $("#inputDescriptionText").val().toUpperCase()
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistAssistanceText, 'autoclose': true });
                }
                else {
                    $("#listViewAssistanceText").UifListView("addItem", AssistanceTextNew);
                }
            }
            else {
                var ifExist = $("#listViewAssistanceText").UifListView('getData').filter(function (item) {
                    return
                    ((item.Text.toUpperCase() == $("#inputDescriptionText").val().toUpperCase()))

                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistAssistanceText, 'autoclose': true });
                }
                $('#listViewAssistanceText').UifListView('editItem', assistanceTextIndex, AssistanceTextNew);
            }
            AsistenceText.clearPanel();
        }
    }

    saveAssistanceText() {
        AssistanceText = $("#listViewAssistanceText").UifListView('getData').filter(x => x.Status === "Modified");
        $('#modalTexts').UifModal("hide");
    }

    CancelView() {
        $('#modalTexts').UifModal("hide");
    }



}
