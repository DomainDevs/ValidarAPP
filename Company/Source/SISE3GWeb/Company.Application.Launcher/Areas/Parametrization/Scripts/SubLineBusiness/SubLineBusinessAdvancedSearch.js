var dropDownSearchSubBranch = null;
class SubLineBusinessAdvancedSearch extends Uif2.Page {

    /**
 * @summary 
    *  Metodo que se ejecuta al instanciar la clase     
 */
    getInitialState() {
        dropDownSearchSubBranch = uif2.dropDown({
            source: rootPath + 'Parametrization/SubLineBusiness/SubLineBusinessAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: SubLineBusinessAdvancedSearch.componentLoadedCallback
        });

        //$("#inputDescriptionSearch").substring(1);
        request('Parametrization/SubLineBusiness/GetsLinesBusiness', null, 'GET', Resources.Language.ErrorSearch, SubLineBusinessAdvancedSearch.GetLineBusiness);
    }


    /**
   * @summary 
       *  Metodo para cargar configuracion de la ventana de busqueda se deben añadir todos los controles  y eventos de la ventana en este metodo
   */
    static componentLoadedCallback() {
        $("#btnAcepSubBranch").on("click", function () {
            dropDownSearchSubBranch.hide();
            let data = $("#listViewSearchAdvancedSubRamo").UifListView("getSelected");
            if (data.length === 1) {
                SubLineBusinessAdvancedSearch.editTechnicalBranch(null, data, data.key);
            }
        });

        $("#listViewSearchAdvancedSubRamo").UifListView({
            displayTemplate: "#advancedSearchSubBranchTemplate",
            selectionType: 'single',
            source: null,
            height: 240
        });

        $("#btnCancelSearchAd").on('click', function () {
            dropDownSearchSubBranch.hide(); //este es el boton borrar
        });

        $('#btnShowSearch').on("click", function myfunction() {
            if ($.trim($("#inputDescriptionSearch").val()) == "" &&
                $.trim($("#selectLineBussinesSearch").val()) == ""){
                $.UifDialog('alert', { 'message': AppResources.EnterSearchCriteria });
                return false;
            }
            SubLineBusinessAdvancedSearch.GetTechnicalBranchAdv();
        });
    }

    bindEvents() {
        $("#btnShowAdvanced").on("click", this.SearchAdv);
    }

    static GetLineBusiness(data) {
        $("#selectLineBussinesSearch").UifSelect({ sourceData: data });
    }

    SearchAdv() {
        SubLineBusinessAdvancedSearch.ShowSearchAdv();
    }

    CancelSearchAdv() {
        SubLineBusinessAdvancedSearch.HideSearchAdv();
    }

    static ShowSearchAdv(data) {
        $("#listViewSearchAdvancedSubRamo").UifListView("refresh");
        $("#inputDescriptionSearch").val("");
        $("#selectLineBussinesSearch").UifSelect('setSelected', null);
        dropDownSearchSubBranch.show();
    }
    static HideSearchAdv() {
        dropDownSearchSubBranch.hide();
    }
    static SearchQuery() {
        if ($.trim($("#inputDescriptionSearch").val()) == "" &&
            $.trim($("#selectLineBussinesSearch").val()) == "") {
            $.UifDialog('alert', { 'message': AppResources.EnterSearchCriteria });
            return false;
        }
        SubLineBusinessAdvancedSearch.GetTechnicalBranchAdv();
    }

    static editTechnicalBranch(event, result, index) {
        $("#Id").val(result[0].Id);
        $("#inputDescriptionShort").val(result[0].SmallDescription);
        $("#inputDescription").val(result[0].Description);
        $("#selectLineBussines").UifSelect("setSelected", result[0].LineBusinessQuery.Id);
        $("#selectLineBussines").UifSelect("disabled", true);
        $("#inputLoadId").val(result[0].Description);
        ClearValidation("#formSubBranch");
    }

    //Seccion Funciones
    static GetTechnicalBranchAdv() {
        var subLineBusinessView = {};
        subLineBusinessView.Description = $("#inputDescriptionSearch").val();
        subLineBusinessView.LineBusinessId = $("#selectLineBussinesSearch").UifSelect("getSelected");
        request('Parametrization/SubLineBusiness/GetSubLineBusinessAdvancedSearch', JSON.stringify({ subLineBusinessView: subLineBusinessView }), 'POST', Resources.Language.ErrorSearch, SubLineBusinessAdvancedSearch.FillListView);
    }

   
    static FillListView(data) {
        $("#listViewSearchAdvancedSubRamo").UifListView(
            {
                displayTemplate: "#advancedSearchSubBranchTemplate",
                selectionType: 'single',
                source: null,
                height: 240
            });
        if (data.length > 0) {
            var SubTechnicalSearch = data;
            SubTechnicalSearch.forEach(item => {
                item.Status = ParametrizationStatus.Original;
                $("#listViewSearchAdvancedSubRamo").UifListView("addItem", item);
            });
        }
    }

}
