var dropDownSearchAdvCourtType = null;
class CourtTypeSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvCourtType = uif2.dropDown({
            source: rootPath + 'Parametrization/JudicialSurety/CourtTypeSearch',
            element: '#inputCourtType',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }

    componentLoadedCallback() {
        $("#lvSearchAdvCourtType").UifListView({
            displayTemplate: "#CourtTypeTemplate",
            selectionType: "single",
            height: 400
        });
        $("#btnCancelCourtTypeSearch").on("click", CourtTypeSearch.CancelSearchAdv);
        $("#btnOkCourtTypeSearchAdv").on("click", CourtTypeSearch.OkSearchAdv);
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvCourtType").UifListView("getSelected");
        if (data != undefined && data != null && data[0] != undefined && data[0] != null) {
            if (data[0].Id > 0) {
                CourtType.setData(data[0]);
            }
        }
        dropDownSearchAdvCourtType.hide();
    }
    static CancelSearchAdv() {
        dropDownSearchAdvCourtType.hide();
    }

    static SearchAdvAdvCourtType() {
        dropDownSearchAdvCourtType.show();
    }

}