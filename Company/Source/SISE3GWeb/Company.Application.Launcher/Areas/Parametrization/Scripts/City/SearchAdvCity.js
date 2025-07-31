var dropDownSearchAdvCity = null;
$.ajaxSetup({ async: true });
class CitySearchAdv extends Uif2.Page {
    getInitialState() {
        
    }
    bindEvents() {
        //Evento que carga de  dropdownlist
        $('#btnSearchAdvCity').click(CitySearchAdv.ShowAdvancedSearch)
        dropDownSearchAdvCity = uif2.dropDown({
            source: rootPath + 'Parametrization/City/SearchAdvCity',
            element: '#btnSearchAdvCity',
            align: 'right',
            width: 560,
            height: 551,
            loadedCallback: CitySearchAdv.LoadBindFormCityAbm
        });
    }
    //Evento de boton para mostrar el dropdownlist para buscar
    static ShowAdvancedSearch() {    
        CitySearchAdv.CleanFormSearchAdvCity()
       
        CityParametrization.GetCountries().done(function (data) {
            if (data.success) {
                $('#ddlCountriesSearch').UifSelect({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });  
        dropDownSearchAdvCity.show();
    }

    //Eventos de los objetos de la vista de busqueda avanzada
    static LoadBindFormCityAbm() {        
        $('#ddlCountriesSearch').on('itemSelected', CitySearchAdv.getStates);
        $("#lvSearchAdvancedCity").UifListView({ sourceData: null, displayTemplate: "#templateSearchAdvancedCity", selectionType: 'single', height: 200 });
        $('#btnCancelSearchAdvCity').click(CitySearchAdv.CancelSearchCity);
        $('#btnOkSearchAdvCity').click(CitySearchAdv.OkSearchCity);
        $('#btnSearchAdvCityAccept').click(CitySearchAdv.AcceptSearchCity);
        
    }
    static CancelSearchCity() {       
        $('#DescriptionSearch').val('');
        dropDownSearchAdvCity.hide();
    }

       
    static AcceptSearchCity() {
        var form = $('#frmAdvCity').serializeObject();

        if (form.Country == "" && form.State == "" && form.Description =="")
        {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.EnterSearchCriteria, 'autoclose': true }); 
        }
        else
        {
            CitySearchAdv.GetCitySearchAdv().done(function (data) {
                if (data.success)
                {
                    if (data.result.length > 0)
                        CitySearchAdv.successSearchCity(data.result);
                    else {
                        $("#lvSearchAdvancedCity").UifListView("refresh");
                        $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ErrorDataNotFound, 'autoclose': true });
                    }
                }
                else 
                    $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCities, 'autoclose': true });              
            });  
        }            
    }

    static GetCitySearchAdv() {
        var CitySearch = {
            Country: { Id: $("#ddlCountriesSearch").UifSelect("getSelected") },
            State: { Id: $("#ddlStatesSearch").UifSelect("getSelected") },
            Description: $("#DescriptionSearch").val()
        };

        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/City/GetCityAdvancedSearch',
            data: JSON.stringify({ cityViewModel: CitySearch }),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static successSearchCity(data) {
        $("#lvSearchAdvancedCity").UifListView("refresh");
        $.each(data, function (index, val) {
            $("#lvSearchAdvancedCity").UifListView("addItem", val);
        });
    }

    static OkSearchCity() {
        
        var Selected = $("#lvSearchAdvancedCity").UifListView("getSelected");
        if (Selected.length > 0) {
            dropDownSearchAdvCity.hide();
            CityParametrization.showDataSearch(Selected[0]);            
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorListElement });
        }
    }

    static CleanFormSearchAdvCity() {
        $('#ddlCountriesSearch').UifSelect('setSelected', null);
        $('#ddlStatesSearch').UifSelect('setSelected', null);
        $('#DescriptionSearch').val('');
        $("#lvSearchAdvancedCity").UifListView({ source: null, displayTemplate: "#templateSearchAdvancedCity", selectionType: 'single', height: 180 });
    }

    static getStates(event, selectedItem) {
      
        if (selectedItem.Id != "" || selectedItem.id>0) {
            CityParametrization.GetListStates(selectedItem.Id).done(function (data) {
                if (data.success) {
                    $("#ddlStatesSearch").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
                if (data.result.length == 0) {
                    $('#ddlStatesSearch').UifSelect({ source: null }); 
                }
            });
        }
        else {
            $('#ddlStatesSearch').UifSelect({ source: null }); 
        }
    }

    static GetListStates(countryId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/City/GetStatesByCountryId?countryId=' + countryId,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }
}