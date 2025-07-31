var dropDownSearchAdvLimitRc;

class LimitRcAdvancedSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearchAdvLimitRc = uif2.dropDown({
            source: rootPath + 'Parametrization/LimitRc/LimitRcAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });
    }

    componentLoadedCallback() {
        $("#listviewSearchLimitRc").UifListView({
            displayTemplate: "#LimitRcTemplate",
            edit: true,
            delete: true,
            deleteCallback: this.DeleteItemLimitRc,
            customAdd: true,
            customEdit: true,
            height: 450
        });
        $("#btnShowAdvanced").on("click", LimitRcAdvancedSearch.SearchAdvLimitRc);
        $("#btnCancelSearchLimitRc").on("click", LimitRcAdvancedSearch.CancelSearchLimitRc);
        $("#btnLoadSearchLimitRc").on("click", LimitRcAdvancedSearch.LoadLimitRc);
       
    }

    bindEvents() {

    }

    static SearchAdvLimitRc() {
        $("#listviewSearchLimitRc").UifListView("clear");
        dropDownSearchAdvLimitRc.show();
    }

    static CancelSearchLimitRc() {
        dropDownSearchAdvLimitRc.hide();
    }

    static LoadLimitRc() {
        var SelectedAdv = $("#listviewSearchLimitRc").UifListView("getSelected");
        dropDownSearchAdvLimitRc.hide();
        if (SelectedAdv.length > 0) {
            LimitRc.showDataAdv(SelectedAdv[0]);
        }
        //LimitRc.clearPanel();
    }


}