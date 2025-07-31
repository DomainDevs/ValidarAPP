var dropDownSearchSubBranch;
class SearchSubLinesBusiness {
    static SearchAdvancedSubLineBusiness(SubLinesBusiness) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/SubLineBusiness/GetSubLineBusinessAdvancedSearch',
            data: JSON.stringify({ subLineBusinessView: SubLinesBusiness }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class AdvancedSearchSubBranch extends Uif2.Page {
    getInitialState() {
        dropDownSearchSubBranch = uif2.dropDown({
            source: rootPath + 'Parametrization/SubLineBusiness/TechnicalSubBranchAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.AdvancedSearchEvents
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);

        $("#listViewSearchAdvancedSubRamo").UifListView({
            displayTemplate: "#advancedSearchSubBranchTemplate",
            selectionType: 'single',
            source: null,
            height: 180
        });

        LineBusiness.GetsLinesBusiness().done(function (data) {
            if (data.success) {
                $('#selectLineBussinesSearch').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {

    }

    static cancelSearch() {
        dropDownSearchSubBranch.hide();
    }

    static AdvancedQuerySubBranch() {

        var SubLinesBusiness = {};
        SubLinesBusiness =
            {
                Description: $("#inputDescriptionSearch").val().trim(),
                LineBusinessDescription: $("#selectLineBussinesSearch").UifSelect("getSelectedText"),
                LineBusinessId: $("#selectLineBussinesSearch").UifSelect("getSelected")
            }
        AdvancedSearchSubBranch.ClearAdvanced();
        SearchSubLinesBusiness.SearchAdvancedSubLineBusiness(SubLinesBusiness).done(function (data) {
            if (data.success) {
                AdvancedSearchSubBranch.ShowSearchAdv(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': 'No se encontro ningun resultado', 'autoclose': true })
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Error al intentar buscar subramo técnico', 'autoclose': true })

        });
    }


    static ShowSearchAdv(data) {
        if (data) {
            for (var i = 0; i < data.length; i++) {
                let item = {
                    Id: data[i].Id,
                    LineBusinessDescription: data[i].LineBusinessDescription,
                    Description: data[i].Description,
                    SmallDescription: data[i].SmallDescription,
                    LineBusinessId: data[i].LineBusinessId
                };

                $("#listViewSearchAdvancedSubRamo").UifListView("addItem", item);
            }
            dropDownSearchSubBranch.show();
        }
    }

    static LoadSubBranchAdvanced() {
        subBranchIndex = 0;
        var SelectedAdv = $("#listViewSearchAdvancedSubRamo").UifListView("getSelected");
        if (SelectedAdv.length > 0) {
            dropDownSearchSubBranch.hide();
            $("#selectLineBussines").UifSelect("setSelected", SelectedAdv[0].LineBusinessId);
            $("#inputDescription").val(SelectedAdv[0].Description);
            $("#inputDescriptionShort").val(SelectedAdv[0].SmallDescription);
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Debe seleccionar un elemento de la lista', 'autoclose': true })
        }
    }

    static ClearAdvanced() {
        if ($("#listViewSearchAdvancedSubRamo").UifListView('getData').length > 0) {
            $("#listViewSearchAdvancedSubRamo").UifListView("clear");
        }
        $("#inputDescriptionSearch").val('');
        $("#selectLineBussinesSearch").UifSelect("setSelected", null);
        $("#selectLineBussinesSearch").UifSelect("disabled", false);
    }
    AdvancedSearchEvents() {
        $("#btnCancelSearchAdv").on("click", AdvancedSearchSubBranch.cancelSearch);
        $('#btnShowSearch').on("click", AdvancedSearchSubBranch.AdvancedQuerySubBranch);
        $("#btnAcepSubBranch").click(AdvancedSearchSubBranch.LoadSubBranchAdvanced);
        LineBusiness.GetsLinesBusiness().done(function (data) {
            if (data.success) {
                $('#selectLineBussinesSearch').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}