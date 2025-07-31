var dropDownSearchAdvSalePoint = null;
var index;
$.ajaxSetup({ async: true });
class SalePointAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvSalePoint = uif2.dropDown({
            source: rootPath + 'Parametrization/SalePoint/SalePointAdvancedSearch',
            element: '#inputSalePoint',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }
    componentLoadedCallback() {
        $("#lvSearchAdvSalePoint").UifListView({
            displayTemplate: "#SalePointTemplate",
            selectionType: "single",
            height: 400
        });

        $("#btnSearchAdvSalePoint").on("click", SalePointAdvancedSearch.SearchAdvSalePoint);
        $("#btnCancelSalePointSearch").on("click", SalePointAdvancedSearch.CancelSearchAdv);
        $("#btnOkSaleSearchAdv").on("click", SalePointAdvancedSearch.OkSearchAdv);
    }

    static SearchAdvSalePoint() {
        dropDownSearchAdvSalePoint.show();
    }

    static CancelSearchAdv() {
        dropDownSearchAdvSalePoint.hide();
    }

    static OkSearchAdv() {        
        let data = $("#lvSearchAdvSalePoint").UifListView("getSelected");      
        SalePointParametrization.SalePointSearch(data[0]);
       
        dropDownSearchAdvSalePoint.hide();
    }
}