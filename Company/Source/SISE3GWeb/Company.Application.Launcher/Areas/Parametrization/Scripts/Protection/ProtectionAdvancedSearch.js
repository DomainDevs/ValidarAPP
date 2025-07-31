var dropDownSearchProtection = null;

class ProtectionAdvancedSearch extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: true });

        dropDownSearchProtection = uif2.dropDown({
            source: rootPath + 'Parametrization/Protection/ProtectionAdvancedSearch',
            element: '#inputSearchProtection',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });
    }

    bindEvents() {      
       
    }

    componentLoadedCallback() {
        $("#lvSearchAdvProtection").UifListView({
            displayTemplate: "#ProtectionTemplate",
            selectionType: "single",
            height: 400
        });
        $("#btnCancelPerilAdv").on("click", ProtectionAdvancedSearch.CancelSearchAdv);
        $("#btnOkPerilSearchAdv").on("click", ProtectionAdvancedSearch.OkSearchAdv);
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvProtection").UifListView({
            displayTemplate: "#ProtectionTemplate",
            selectionType: "single",
            height: 400
        });
        $("#lvSearchAdvProtection").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvProtection").UifListView("addItem", item);
            });
        }
        dropDownSearchProtection.show();
    }

    static CancelSearchAdv() {
        dropDownSearchProtection.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvProtection").UifListView("getSelected");
        if (data.length === 1) {
            ProtectionParametrization.ShowData(null, data, data.key);
        }
        ProtectionParametrization.HideSearchAdv();
    }

}
