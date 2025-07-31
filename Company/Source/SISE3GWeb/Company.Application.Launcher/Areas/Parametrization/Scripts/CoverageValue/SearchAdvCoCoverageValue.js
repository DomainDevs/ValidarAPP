var dropDownSearchAdvCoCoverageValue= null;
$.ajaxSetup({ async: true });
class CoCoverageValueSearchAdv extends Uif2.Page {
    getInitialState() {
        
    }
    bindEvents() {
        //Evento que carga de  dropdownlist
        $('#btnSearchAdvCovergeValue').click(CoCoverageValueSearchAdv.ShowAdvancedSearch)
        dropDownSearchAdvCoCoverageValue = uif2.dropDown({
            source: rootPath + 'Parametrization/CoverageValue/CoverageValueSearchAdv',
            element: '#btnSearchAdvCovergeValue',
            align: 'right',
            width: 560,
            height: 551,
            loadedCallback: CoCoverageValueSearchAdv.LoadBindFormAbm
        });
        $("#btnOkSearchAdvCoverage").on('click', this.OkSearch);
    }

    OkSearch() {

        var Selected = $("#lvSearchAdvanced").UifListView("getSelected");
        if (Selected.length > 0) {
            dropDownSearchAdvCoCoverageValue.hide();
            ParametrizationCoverageValue.showDataSearch(Selected[0]);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorListElement });
        }
    }
    //Evento de boton para mostrar el dropdownlist para buscar
    static ShowAdvancedSearch() {
        CoCoverageValueSearchAdv.CleanFormSearchAdv()

        CoCoverageValueSearchAdv.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#ddlPrefixSearch').UifSelect({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        dropDownSearchAdvCoCoverageValue.show();
    }

    //Eventos de los objetos de la vista de busqueda avanzada
    static LoadBindFormAbm() {
        $('#ddlPrefixSearch').on('itemSelected', CoCoverageValueSearchAdv.GetCoverages);
        $("#lvSearchAdvanced").UifListView({ sourceData: null, displayTemplate: "#templateSearchAdvancedCoCoverage", selectionType: 'single', height: 200 });
        $('#btnSearchAdvAccept').click(CoCoverageValueSearchAdv.AcceptSearchAdv);
        $('#btnCancelAdv').click(CoCoverageValueSearchAdv.CancelSearch);       
        $("#btnOkSearchAdvCoverage").on('click', CoCoverageValueSearchAdv.OkSearch);
    }

    static CleanFormSearchAdv() {
        $('#ddlPrefixSearch').UifSelect('setSelected', null);
        $('#ddlCoverageSearch').UifSelect('setSelected', null);      
        $("#lvSearchAdvanced").UifListView({ source: null, displayTemplate: "#templateSearchAdvancedCoCoverage", selectionType: 'single', height: 180 });
    }

    static CancelSearch() {
     
        dropDownSearchAdvCoCoverageValue.hide();
    }

    static AcceptSearchAdv() {
        var form = $('#frmAdvCoCoverageValue').serializeObject();

        if (form.Coverage == "" && form.Prefix == "") {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.EnterSearchCriteria, 'autoclose': true });
        }
        else {
            CoCoverageValueSearchAdv.GetSearchAdv().done(function (data) {
                if (data.success) {
                    if (data.result.length > 0)
                        CoCoverageValueSearchAdv.successSearch(data.result);
                    else {
                        $("#lvSearchAdvanced").UifListView("refresh");
                        $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCoCoverageValue, 'autoclose': true });
                }
            });
        }
    }

    static GetSearchAdv() {
        var OpcionesSearch = {
            Coverage: { Id: $("#ddlCoverageSearch").UifSelect("getSelected") },
            Prefix: { Id: $("#ddlPrefixSearch").UifSelect("getSelected") }            
        };

        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/CoverageValue/GetAdvancedSearch',
            data: JSON.stringify({ coverageValueViewModel: OpcionesSearch }),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static successSearch(data) {
        $("#lvSearchAdvanced").UifListView("refresh");
        $.each(data, function (index, val) {
            $("#lvSearchAdvanced").UifListView("addItem", val);
        });
    }

    static OkSearch() {
        var Selected = $("#lvSearchAdvanced").UifListView("getSelected");
        if (Selected.length > 0) {
            dropDownSearchAdvCoCoverageValue.hide();
            ParametrizationCoverageValue.showDataSearch(Selected[0]);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorListElement });
        }
    }

    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/CoverageValue/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }


    static GetCoverages() {
        var prefixId = 0;
        if ($("#ddlPrefixSearch").val() != null && $("#ddlPrefixSearch").val() != "") {
            prefixId = $("#ddlPrefixSearch").val();
            CoCoverageValueSearchAdv.GetListCoverages(prefixId).done(function (data) {
                if (data.success) {
                    $("#ddlCoverageSearch").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
            });
        }
        else {
            $('#ddlCoverage').UifSelect({ source: null });
        }
    }

    static GetListCoverages(prefixId) {

        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/CoverageValue/GetCoverages?idPrefix=' + prefixId,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }
}