var dropDownSearchAdvInsurancesObjects = null;
$(() => {
    new InsurancesObjectsAdvancedSearch();
});
class InsurancesObjectsAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvInsurancesObjects = uif2.dropDown({
            source: rootPath + 'Parametrization/InsurancesObjects/InsurancesObjectsAdvancedSearch',
            element: '#btnSearchAdvInsurancesObject',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }

    componentLoadedCallback() {
        $("#lvSearchAdvInsurancesObjects").UifListView({
            displayTemplate: "#InsurancesObjectsTemplate",
            selectionType: "single",
            height: 400
        });

        $("#btnSearchAdvInsurancesObject").on("click", InsurancesObjectsAdvancedSearch.ShowSearchAdv);
        $("#btnCancelSearchAdv").on("click", InsurancesObjectsAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", InsurancesObjectsAdvancedSearch.OkSearchAdv);
    }

    static ShowSearchAdv() {
        dropDownSearchAdvInsurancesObjects.show();
    }

   
    static CancelSearchAdv() {
        ParametrizationInsurancesObjects.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvInsurancesObjects").UifListView("getSelected");
        if (data.length === 1) {
            let object =
            {
                Id: data[0].Id,
                Description: data[0].Description,
                SmallDescription: data[0].SmallDescription,
                IsDeraclarative: data[0].IsDeraclarative,
                DeclarativeDescription: data[0].DeclarativeDescription
            };
            if (object.IsDeraclarative == true) {
                object.DeclarativeDescription = Resources.Language.IsDeraclarative;
            }
            var lista = $("#listViewInsurancesObjects").UifListView('getData');
            var index = lista.findIndex(function (item) {
                return item.Id = object.Id;
            });
            ParametrizationInsurancesObjects.ShowData(null, object, index);
        }
        ParametrizationInsurancesObjects.HideSearchAdv();
    }

}