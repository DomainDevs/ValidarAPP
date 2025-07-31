var AssistanceNew = {};
var assistanceIndex = null;
var _Prefix = {};
var glbAssistance = {};
var glbAssistanceText = {};
var AssistanceCd;
var Assistances = {};
var AssistanceText = {};
$.ajaxSetup({ async: true });
class AssistanceQueries {
    static GetAssistanceType() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AssistanceType/GetAssistanceType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetAssistanceByAssistanceCodeDescriptionPrefix(AssistanceCode, descripcion, PrefixCode) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AssistanceType/GetAssistanceByAssistanceCodeDescriptionPrefix',
            data: JSON.stringify({ AssistanceCode: AssistanceCode, descripcion: descripcion, PrefixCode: PrefixCode }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AssistanceType/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}


class ParametrizationAssistanceType extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputAssistanceCode").ValidatorKey();
        $("#listViewAssistanceType").UifListView({
            displayTemplate: "#AssistanceTemplate",
            edit: true,
            delete: true,
            customEdit: true,
            customDelete: true,
            height: 300
        });
        PrefixQueries.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#ddlBusinessType').UifSelect({ sourceData: data.result });
                _Prefix = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGetPrefixes, 'autoclose': true });
            }
        });
        AssistanceQueries.GetAssistanceType().done(function (data) {
            if (data.success) {
                ParametrizationAssistanceType.loadAssistance(data.result);
                glbAssistance = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGetAssistanceType, 'autoclose': true });
            }
        });
        $('#btnTexts').attr("disabled", true);
    }

    bindEvents() {

        $('#btnAcceptAssistanceType').on('click', this.addItemAssistance);
        $('#btnNewAssistanceType').on('click', ParametrizationAssistanceType.NewAssistance);
        $('#listViewAssistanceType').on('rowEdit', ParametrizationAssistanceType.showData);
        $('#btnSave').on('click', this.saveAssistance);
        $('#btnExport').on('click', this.sendExcelAssistance);
        $('#btnExit').click(this.exit);
        $('#inputAssistanceTypeSearch').on('itemSelected', ParametrizationAssistanceType.SearchAssistance);
        $('#inputAssistanceTypeSearch').on("buttonClick", ParametrizationAssistanceType.SearchAssistance);
        $("#btnShowAdvanced").on("click", this.ShowAdvanced);
        $("#btnTexts").click(this.Texts);
    }

    static loadAssistance(Assistance) {
        $("#listViewAssistanceType").UifListView({ source: null, displayTemplate: "#AssistanceTemplate", edit: true, delete: false, customEdit: true, customDelete: false, height: 300 });
        Assistance.forEach(item => {
            $("#listViewAssistanceType").UifListView("addItem", item);
        });
    }

    static NewAssistance() {
        
        $("#inputAssistanceTypeSearch").val('');
        $("#inputAssistanceCode").val('');
        $('#inputAssistanceCode').attr("disabled", false);
        $('#inputAssistanceCode').focus();
        $("#ddlBusinessType").UifSelect("setSelected", null);
        $("#ddlBusinessType").UifSelect("disabled", false);
        $("#inputDescription").val('');
        $("#rdEnabled").is(":checked", false);
        $('#btnTexts').attr("disabled", true);
        //$("#formAssistanceType").valid()
        ClearValidation("#formAssistanceType");
    }

    static clearPanel() {
        assistanceIndex = null;

        $("#inputAssistanceTypeSearch").val('');
        $("#inputAssistanceCode").val('');
        $('#inputAssistanceCode').attr("disabled", false);
        $("#ddlBusinessType").UifSelect("setSelected", null);
        $("#ddlBusinessType").UifSelect("disabled", false);
        $("#inputDescription").val('');
        $("#rdEnabled").is(":checked", false);
        //$("#listViewAssistanceType").UifListView(
        //    {
        //        displayTemplate: "#AssistanceTemplate",
        //        selectionType: 'single',
        //        source: null,
        //        height: 180
        //    });
        $('#btnTexts').attr("disabled", true);
    }

    static showData(event, result, index) {
        ParametrizationAssistanceType.clearPanel();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        else if (result.length > 1) {
            ParametrizationAssistanceType.ShowSearchAdv(result);
        }
        if (result.AssistanceCode != undefined) {
            assistanceIndex = index;
            ClearValidation("#formAssistanceType");
            $("#inputAssistanceCode").val(result.AssistanceCode);
            $('#inputAssistanceCode').attr("disabled", true);
            $("#inputDescription").val(result.Description);
            $("#ddlBusinessType").UifSelect("setSelected", result.PrefixCode);
            $("#ddlBusinessType").UifSelect("disabled", true);
            $("#rdEnabled").prop("checked", result.Enabled);
        }
        $('#btnTexts').attr("disabled", false);
    }

    static showDataAdv(assistanceType) {
        var find = false;
        var data = [];
        $.each(glbAssistance, function (key, value) {
            if (
                (value.AssistanceCode == assistanceType.AssistanceCode)

            ) {
                assistanceIndex = key;
                data.push(value);
                find = true;
            }
        });
        if (find == false) {
            $.UifNotify('show',
                {
                    'type': 'danger', 'message': Resources.Language.AssistanceTypeNotFound, 'autoclose': true
                })
        }
        $("#inputAssistanceCode").val(assistanceType.AssistanceCode);
        $('#inputAssistanceCode').attr("disabled", true);
        $("#inputDescription").val(assistanceType.Description);
        $("#ddlBusinessType").UifSelect("setSelected", assistanceType.PrefixCode);
        $('#ddlBusinessType').attr("disabled", true);
        $("#rdEnabled").prop("checked", assistanceType.Enabled);
    }

    static SearchAssistance(description) {
        description = $('#inputAssistanceTypeSearch').val();
        var find = false;
        var data = [];
        if (description.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        } else {
            glbAssistance = $("#listViewAssistanceType").UifListView('getData');
            $.each(glbAssistance, function (i, value) {
                if (
                    (value.Description.includes($("#inputAssistanceTypeSearch").val()))

                ) {
                    value.key = i;
                    data.push(value);
                    find = true;
                }
            });


            if (find == false) {
                $.UifNotify('show',
                    {
                        'type': 'danger', 'message': Resources.Language.AssistanceTypeNotFound, 'autoclose': true
                    })
            } else {
                ParametrizationAssistanceType.showData(null, data, data.key);
            }
        }
        $("#inputAssistanceTypeSearch").val("");

    }

    static ShowSearchAdv(data) {
        
        AssistanceAdvancedSearch.clearPanel();
        $('#ddlBusinessTypeAdv').UifSelect({
            sourceData: _Prefix
        });
        if (data) {
            $("#listviewSearchAssistance").UifListView(
                {
                    displayTemplate: "#searchAssistanceTemplate",
                    selectionType: 'single',
                    source: null,
                    height: 180
                });
            data.forEach(item => {
                $("#listviewSearchAssistance").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvAssistance.show();
    }

    static HideSearchAdv() {
        dropDownSearchAdvAssistance.hide();
    }

    Texts() {
        if ($("#rdEnabled").is(":checked")) {
            ParametrizationAssistanceType.ShowPanelsAssistance();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': "Tipo de asistencia deshabilitada ", 'autoclose': true });
        }

    }

    static ShowPanelsAssistance() {
        AssistanceCd = parseInt($("#inputAssistanceCode").val());
        $("#modalTexts").UifModal("showLocal", "Textos Asistencia: " + $("#inputDescription").val() + ' (' + $("#inputAssistanceCode").val() + ')'
            + ' - ' + $("#ddlBusinessType").UifSelect("getSelectedText"));
        AssistanceTextRequest.GetAssistanceText(AssistanceCd).done(function (data) {
            if (data.success) {
                AsistenceText.loadAssistanceText(data.result);
                AssistanceTextNew.ClauseDescription = $("#inputTextClause").UifSelect("getSelected");
                AssistanceTextNew.ClauseCd3G = ClauseCode;
                glbAssistanceText = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorExistAssistanceText, 'autoclose': true });
            }
        });

    }

    ShowAdvanced() {
        
        $('#ddlBusinessTypeAdv').UifSelect({
            sourceData: _Prefix
        });
        $("#listviewSearchAssistance").UifListView(
            {
                displayTemplate: "#searchAssistanceTemplate",
                selectionType: 'single',
                source: null,
                height: 180
            });
        dropDownSearchAdvAssistance.show();
    }

    addItemAssistance() {
        $("#formAssistanceType").validate();
        if ($("#formAssistanceType").valid()) {
            AssistanceNew = {};
            AssistanceNew.AssistanceCode = parseInt($("#inputAssistanceCode").val());
            AssistanceNew.PrefixCode = $("#ddlBusinessType").UifSelect("getSelected");
            AssistanceNew.PrefixDescription = $("#ddlBusinessType").UifSelect("getSelectedText");
            AssistanceNew.Description = $("#inputDescription").val();
            AssistanceNew.Enabled = $("#rdEnabled").is(":checked");
            AssistanceNew.EnabledDescription = $("#rdEnabled").is(":checked") == true ? Resources.Language.LabelEnabled : Resources.Language.Disabled;

            

            if (assistanceIndex == null) {
                AssistanceNew.Status = 2;
                var ifExist = $("#listViewAssistanceType").UifListView('getData').filter(function (item) {
                    return item.AssistanceCode == $("#inputAssistanceCode").val() || item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase()
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistAssistance, 'autoclose': true });
                }
                else {
                    $("#listViewAssistanceType").UifListView("addItem", AssistanceNew);
                }
            }
            else {
                AssistanceNew.Status = 3;
                var ifExist = $("#listViewAssistanceType").UifListView('getData').filter(function (item) {
                    return
                    ((item.AssistanceCode != $("#inputAssistanceCode").val()) &&
                        (item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase()))

                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistAssistance, 'autoclose': true });
                }
                $('#listViewAssistanceType').UifListView('editItem', assistanceIndex, AssistanceNew);
            }
            ParametrizationAssistanceType.clearPanel();
        }
    }

    saveAssistance() {
        Assistances = $("#listViewAssistanceType").UifListView('getData').filter(x => x.Status === 2 || x.Status === 3);

        $.each(glbBusinessBranchDelete, function (index, item) {
            Assistances.push(item);
        });

        if (Assistances.length > 0 || AssistanceText.length > 0) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/AssistanceType/SaveAssistanceType',
                data: JSON.stringify({ Assistances: Assistances, AssistanceText: AssistanceText }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    ParametrizationAssistanceType.loadAssistance(data.result[1]);
                    $.each(data.result[0], function (index, item) {
                        $.UifNotify('show', { 'type': 'info', 'message': item, 'autoclose': true })
                    });
                    $.each(data.result[2], function (index, item) {
                        $.UifNotify('show', { 'type': 'info', 'message': item, 'autoclose': true })
                    });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveAssistance, 'autoclose': true })
            });
        }
        Assistances = null;
        AssistanceText = null;
    }

    sendExcelAssistance() {
        AssistanceQueries.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    exit() {
        window.location = rootPath + "Home/Index";
    }

}