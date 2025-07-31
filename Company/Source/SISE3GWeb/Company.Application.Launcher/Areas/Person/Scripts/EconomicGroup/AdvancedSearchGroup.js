var templateAdvanced = null;
class AdvancedSearchGroup extends Uif2.Page {    

    getInitialState()
    {
    }

    bindEvents() {
        templateAdvanced = uif2.dropDown({
            source: rootPath + 'Person/EconomicGroup/AdvancedSearch',
            element: '#btnSearchGroup',
            align: 'right',
            width: 600,
            height: 470,
            loadedCallback: AdvancedSearchGroup.LoadbindignsSearchAdvanzed
        });
        $("#btnSearchGroup").on("click", AdvancedSearchGroup.ShowAdvancedSearch);
    }
    static LoadbindignsSearchAdvanzed() {
        $("#btnAdvancedSearch").on("click", AdvancedSearchGroup.getGroupAdvancedSearch);
        $("#btnLoadProduct").on("click", AdvancedSearchGroup.getDetailGroupAdvancedSearch);
    }
    static ShowAdvancedSearch() {
        templateAdvanced.show();
        $("#listviewSearch").UifListView({
            displayTemplate: "#groupAdvancedSearchResult",
            selectionType: 'single',
            source: null,
            height: 200
        });
    }

    static getDetailGroupAdvancedSearch() {
        var itemSelected = $("#listviewSearch").UifListView("getSelected");
        AdvancedSearchGroup.Clear();
        templateAdvanced.hide();
        if (itemSelected.length > 0) {
            EconomicGroup.mainGroupByIdDescription(itemSelected);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageNotSelectedEconomicGroup, 'autoclose': true });
        }
    }

    static Clear() {
        $("#inputEconomicGroupId").val("");
        $("#inputEconomicGroupName").val("");
    }

    static getGroupAdvancedSearch() {
        $("#listviewSearch").UifListView("clear");
        if (valiteDataProduct()) {
            var economicGroupAdvancedSearch = {
                TributaryIdNo: $("#inputEconomicGroupId").val().trim(),
                EconomicGroupName: $("#inputEconomicGroupName").val().trim(),
                Enabled: $("#inputEnabled").prop('checked')
            };

            AdvancedSearchGroupRequest.GetEconomicGroupAdvancedSearch(economicGroupAdvancedSearch).done(function (data) {
                if (data.length > 0) {
                    AdvancedSearchGroup.LoadProductAdvanced(data);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageNotFoundEconomicGroup , 'autoclose': true });
                }
            }).error(function (data) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchEconomicGroup , 'autoclose': true });
            });
        }
    }

    static LoadProductAdvanced(products) {
        $("#listviewSearch").UifListView("refresh");
        if (products != null) {
            $.each(products, function (index, val) {
                $("#listviewSearch").UifListView("addItem", products[index]);
            });
        }
    }
}

function valiteDataProduct() {
    //if (($("#inputEconomicGroupId").val() == null || $("#inputEconomicGroupId").val() == "")
    //    && ($("#inputEconomicGroupId").val() == null || $("#inputEconomicGroupName").val() == "")) {
    //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEnterDataEconomicGroup , 'autoclose': true });
    //    return false;
    //}
    return true;
}
