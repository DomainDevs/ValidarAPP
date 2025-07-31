var dropDownSearchAdv = null;
//$.ajaxSetup({ async: false });
class AdvancedSearchParametrization extends Uif2.Page {
    /**
   * @summary 
      *  Metodo que se ejecuta al instanciar la clase     
   */
    getInitialState() {
        dropDownSearchAdv = uif2.dropDown({
            source: rootPath + 'Parametrization/Deductible/AdvancedSearch',
            element: '#advDeductibleSearch',
            align: 'right',
            width: 550,
            height: 551,            
            loadedCallback: AdvancedSearchParametrization.componentLoadedCallback
        });

        //$("#inputValueAdv").OnlyDecimals(4);
        //request('Parametrization/Deductible/GetLineBusiness', null, 'GET', Resources.Language.ErrorSearch, AdvancedSearchParametrization.GetLineBusiness);
        //request('Parametrization/Deductible/GetDeductibleUnit', null, 'GET', Resources.Language.ErrorSearch, AdvancedSearchParametrization.GetDeductibleUnit);
        //request('Parametrization/Deductible/GetDeductibleSubject', null, 'GET', Resources.Language.ErrorSearch, AdvancedSearchParametrization.GetDeductibleSubject);
        //
        //AdvancedSearchParametrization.GetData1().done(function (data) {
        //    
        //    if (data.success) {
        //        //$('#ddlCountries').UifSelect({ sourceData: data.result });
        //        $("#selectLineBusinessAdv").UifSelect({ sourceData: data.result });
        //    }
        //    else {
        //        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        //    }
        //});
    }

    bindEvents() {


        $("#advDeductibleSearch").on("click", this.SearchAdv);
    }

    static GetData1() {
        return $.ajax({
            async: false,
            type: 'GET',
            url: rootPath + 'Parametrization/Deductible/GetLineBusiness',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    * @summary 
        *  Metodo para cargar configuracion de la ventana de busqueda se deben añadir todos los controles  y eventos de la ventana en este metodo
    */
    static componentLoadedCallback() {
        $("#btnLoad").on("click", function () {
            dropDownSearchAdv.hide();
            let data = $("#listViewSearchAdvanced").UifListView("getSelected");
            if (data.length === 1) {
                DeductibleParametrization.ShowData(null, data, data.key);
            }
        });

        $("#listViewSearchAdvanced").UifListView({
            displayTemplate: "#DeductibleTemplate",
            selectionType: 'single',
            source: null,
            height: 230
        });

        $("#btnCancelSearch").on('click', function () {
            dropDownSearchAdv.hide();
        })

        $('#btnAdvancedSearchDeduct').on("click", function myfunction() {
            if ($.trim($("#selectLineBusinessAdv").val()) == "" &&
                $.trim($("#selectUnitAdv").val()) == "" &&
                $.trim($("#selectApplyOnAdv").val()) == "" &&
                $.trim($("#inputValueAdv").val()) == "") {
                $.UifDialog('alert', { 'message': AppResources.EnterSearchCriteria });
                return false;
            }
            AdvancedSearchParametrization.GetDeductibleAdv();
        });

        $("#inputValueAdv").OnlyDecimals(4);
        request('Parametrization/Deductible/GetLineBusiness', null, 'GET', Resources.Language.ErrorSearch, AdvancedSearchParametrization.GetLineBusiness);
        request('Parametrization/Deductible/GetDeductibleUnit', null, 'GET', Resources.Language.ErrorSearch, AdvancedSearchParametrization.GetDeductibleUnit);
        request('Parametrization/Deductible/GetDeductibleSubject', null, 'GET', Resources.Language.ErrorSearch, AdvancedSearchParametrization.GetDeductibleSubject);
    }

    static GetLineBusiness(data) {
        $("#selectLineBusinessAdv").UifSelect({ sourceData: data });
    }
    static GetDeductibleUnit(data) {   
        $("#selectUnitAdv").UifSelect({ sourceData: data });
    }
    static GetDeductibleSubject(data) {
        $("#selectApplyOnAdv").UifSelect({ sourceData: data });
    }
    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES
    SearchAdv() {
        AdvancedSearchParametrization.ShowSearchAdv();
    }
    CancelSearchAdv() {
        AdvancedSearchParametrization.HideSearchAdv();
    }
    static ShowSearchAdv(data) {
        $("#listViewSearchAdvanced").UifListView("refresh");
        $("#selectLineBusinessAdv").UifSelect('setSelected', null);
        $("#selectUnitAdv").UifSelect('setSelected', null);
        $("#selectApplyOnAdv").UifSelect('setSelected', null);
        $("#inputValueAdv").val("");
        dropDownSearchAdv.show();
    }
    static HideSearchAdv() {
        dropDownSearchAdv.hide();
    }
    static SearchQuery() {
        if ($.trim($("#selectLineBusinessAdv").val()) == "" &&
            $.trim($("#selectUnitAdv").val()) == "" &&
            $.trim($("#selectApplyOnAdv").val()) == "" &&
            $.trim($("#inputValueAdv").val()) == "") {
            $.UifDialog('alert', { 'message': AppResources.EnterSearchCriteria });
            return false;
        }
        AdvancedSearchParametrization.GetDeductibleAdv();
    }
    static OkSearchAdv() {
        let data = $("#listViewSearchAdvanced").UifListView("getSelected");
        if (data.length === 1) {
            DeductibleParametrization.ShowData(null, data, data.key);
        }
        AdvancedSearchParametrization.HideSearchAdv();
    }

    //Seccion Funciones
    static GetDeductibleAdv() {
        var deductible = {};
        deductible.LineBusinessId = parseInt($("#selectLineBusinessAdv").UifSelect("getSelected"));
        deductible.ApplyOnId = parseInt($("#selectApplyOnAdv").UifSelect("getSelected"));
        deductible.UnitId = parseInt($("#selectUnitAdv").UifSelect("getSelected"));
        deductible.Value = parseFloat($("#inputValueAdv").val());
        request('Parametrization/Deductible/GetDeductibleByDeductible', JSON.stringify({ deductible: deductible }), 'POST', Resources.Language.ErrorSearch, AdvancedSearchParametrization.FillListView);
    }

    static FillListView(data) {
        $("#listViewSearchAdvanced").UifListView(
            {
                displayTemplate: "#DeductibleTemplate",
                selectionType: 'single',
                source: null,
                height: 230
            });
        if (data.length > 0) {
            var deductiblesSearch = data;
            deductiblesSearch.forEach(item => {
                item.Status = ParametrizationStatus.Original;
                $("#listViewSearchAdvanced").UifListView("addItem", item);
            });
        }
    }

}
