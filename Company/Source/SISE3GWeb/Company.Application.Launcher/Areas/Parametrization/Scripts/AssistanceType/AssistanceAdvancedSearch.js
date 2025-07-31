var dropDownSearchAdvAssistance;

class AssistanceQueriesAdv {
   
    static GetAssistanceByAssistanceCodeDescriptionPrefix(AssistanceCode, descripcion, PrefixCode) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AssistanceType/GetAssistanceByAssistanceCodeDescriptionPrefix',
            data: JSON.stringify({ AssistanceCode: AssistanceCode, descripcion: descripcion, PrefixCode: PrefixCode }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

}


class AssistanceAdvancedSearch extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        dropDownSearchAdvAssistance = uif2.dropDown({
            source: rootPath + 'Parametrization/AssistanceType/AssistanceAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.AdvancedSearchEventsAssistance
        });
        PrefixQueries.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#ddlBusinessTypeAdv').UifSelect({ sourceData: data.result });
                Prefix = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    AdvancedSearchEventsAssistance() {
        AssistanceAdvancedSearch.clearPanel();

        $("#btnCancelSearchAssistance").on("click", AssistanceAdvancedSearch.CancelSearchAssistance);
        $("#selectTypeAssistance").on("itemSelected", AssistanceAdvancedSearch.TypePersonSelected);
        $("#btnsearchAssistanceAdv").click(AssistanceAdvancedSearch.SearchAssistanceAdv);
        $("#btnLoadAssistance").click(AssistanceAdvancedSearch.LoadAssistance);
        //AssistanceAdvancedSearch.LoadSelectTypePerson();
    }

    bindEvents() {
        $("#btnCancelSearchAssistance").on("click", AssistanceAdvancedSearch.CancelSearchAssistance);
        $("#selectTypeAssistance").on("itemSelected", AssistanceAdvancedSearch.TypePersonSelected);
        $("#btnsearchAssistanceAdv").click(AssistanceAdvancedSearch.SearchAssistanceAdv);
        $("#btnLoadAssistance").click(AssistanceAdvancedSearch.LoadAssistance);
    }

    static SearchAssistanceAdv() {
        if ($.trim($("#inputDescriptionAdv").val()) == "" &&
            $.trim($("#inputAssistanceCodeAdv").val()) == "" &&
            $.trim($("#ddlBusinessTypeAdv").val()) == "") {
            $.UifDialog('alert', { 'message': AppResources.EnterSearchCriteria });
            return false;
        }
        AssistanceAdvancedSearch.GetAssistanceByAssistanceCodeDescriptionPrefixAdv();
    }

    static GetAssistanceByAssistanceCodeDescriptionPrefixAdv() {
        var DescriptionAdv = $("#inputDescriptionAdv").val().toUpperCase();
        var AssistanceCodeAdv = 0;
        var BusinessTypeAdv = 0;
        if ($("#inputAssistanceCodeAdv").val() != "") {
            AssistanceCodeAdv = parseInt($("#inputAssistanceCodeAdv").val());
        }
       
        if ($("#ddlBusinessTypeAdv").UifSelect("getSelected") != "") {
            BusinessTypeAdv = parseInt($("#ddlBusinessTypeAdv").UifSelect("getSelected"));
        }
        
        AssistanceQueriesAdv.GetAssistanceByAssistanceCodeDescriptionPrefix(AssistanceCodeAdv, DescriptionAdv ,
            BusinessTypeAdv).done(function (data) {
                if (data.success) {
                    AssistanceAdvancedSearch.LoadAssistanceAdv(data.result, "#searchAssistanceTemplate");
                }
            });
    }

    static LoadAssistanceAdv(Assistance, templateAssistance) {
        $("#listviewSearchAssistance").UifListView(
            {
                displayTemplate: templateAssistance,
                selectionType: 'single',
                source: null,
                height: 220
            });
        $.each(Assistance, function (index, item) {
           
            $("#listviewSearchAssistance").UifListView("addItem", item);
        });
    }

    static LoadAssistance() {
        var SelectedAdv = $("#listviewSearchAssistance").UifListView("getSelected");
        dropDownSearchAdvAssistance.hide();
        if (SelectedAdv.length > 0) {
            ParametrizationAssistanceType.showDataAdv(SelectedAdv[0]);
        }
        AssistanceAdvancedSearch.clearPanel();
    }

    static clearPanel() {
        $("#inputAssistanceCodeAdv").val('');
        $("#inputDescriptionAdv").val('');
        $("#ddlBusinessTypeAdv").UifSelect("setSelected", null);
        $("#ddlBusinessTypeAdv").UifSelect("disabled", false);
        $("#listviewSearchAssistance").UifListView(
            {
                displayTemplate: "#searchAssistanceTemplate",
                selectionType: 'single',
                source: null,
                height: 220
            });
        //if ($("#listviewSearchAssistance").UifListView('getData').length > 0) {
        //    $("#listviewSearchAssistance").UifListView("clear");
        //}
    }

    static CancelSearchAssistance() {
        dropDownSearchAdvAssistance.hide();
    }
}