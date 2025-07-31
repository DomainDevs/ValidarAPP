var dropDownSearchAdvLegalRepresentativeSing = null;

$(() => {
    new LegalRepresentativeSingAdvancedSearch();
});

class LegalRepresentativeSingAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvLegalRepresentativeSing = uif2.dropDown({
            source: rootPath + 'Printing/LegalRepresentativeSing/LegalRepresentativeSingAdvancedSearch',
            element: '#btnSearchAdvLegalRepresentativeSing',
            align: 'right',
            width: 600,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

	}

    bindEvents() {

	} 

    componentLoadedCallback() {
        $("#lvSearchAdvLegalRepresentativeSing").UifListView({
            displayTemplate: "#LegalRepresentativeSingTemplate",
            selectionType: "single",
            height: 450
        });

        $("#btnSearchAdvLegalRepresentativeSing").on("click", LegalRepresentativeSingAdvancedSearch.SearchAdvLegalRepresentativeSing);
        $("#btnCancelSearchAdv").on("click", LegalRepresentativeSingAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", LegalRepresentativeSingAdvancedSearch.OkSearchAdv);
    }
	
    static SearchAdvLegalRepresentativeSing() {
        dropDownSearchAdvLegalRepresentativeSing.show();
    }

    static CancelSearchAdv() {
		ParametrizationLegalRepresentativeSing.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvLegalRepresentativeSing").UifListView("getSelected");
        if (data.length === 1) {
			ParametrizationLegalRepresentativeSing.ShowData(null, data, data.key);
        }
		ParametrizationLegalRepresentativeSing.HideSearchAdv();
    }
}