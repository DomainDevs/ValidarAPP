var dropDownSearchAdvProduct2G;
class Product2GAdvancedSearch extends Uif2.Page {
    getInitialState() {
       
        dropDownSearchAdvProduct2G = uif2.dropDown({
            source: rootPath + 'Parametrization/Product2G/Product2GAdvancedSearch',
            element: '#inputProduct2gSearch',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
	bindEvents() {

    }
    componentLoadedCallback() {     
		$("#btnCanSearchAdv").on("click", Product2GAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", Product2GAdvancedSearch.OkSearchAdv);
    }  

    static CancelSearchAdv() {
		dropDownSearchAdvProduct2G.hide();
    }

    static OkSearchAdv() {
        var data = $("#lvSearchAdvProduct2G").UifListView("getSelected");
        if (data.length > 0) {
            Product2G.showDataAdv(data[0]);
        }
		dropDownSearchAdvProduct2G.hide();
    }

    static ShowSearchAdv(data) {

        $("#lvSearchAdvProduct2G").UifListView("clear");
        if (data) {
            data.forEach(item => {
                $("#lvSearchAdvProduct2G").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvProduct2G.show();
    }
}