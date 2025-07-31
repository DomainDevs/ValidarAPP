var cityInsert = null;

$.ajaxSetup({ async: true });
class CityParametrization extends Uif2.Page {

    getInitialState() {       
        $("#lsvCities").UifListView({
            displayTemplate: "#template-cities",  
            customAdd: true,            
            height: 380,
            widht:100,
            customEdit: true, edit: true,
            delete: true,
            selectionType: "single",
            title: Resources.Language.TituloCiudades,        
            deleteCallback: CityParametrization.deleteRowCity,
        });
  
        CityParametrization.GetCountries().done(function (data) {            
           if (data.success) {
               $('#ddlCountries').UifSelect({ sourceData: data.result });               
             
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });    

        CityParametrization.GetCities().done(function (n)
        {
            if (n.success) {
                CityParametrization.SetListCities(n.result);
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': n.result, 'autoclose': true });
            }
        });
      
    }

    bindEvents() {     
        $('#ddlCountries').on('itemSelected', CityParametrization.getStates);

        $("#btnNew").click(CityParametrization.clearFormCities);  

        $('#btnSave').on('click', CityParametrization.saveCities);

        $('#lsvCities').on('rowEdit', CityParametrization.showData);     

        $('#searchCity').on("search", CityParametrization.searchRangeCity);

        $('#btnExportCity').click(CityParametrization.ExportFile);

        $('#btnSalir').click(CityParametrization.Exit);
 
    }

    
    static GetCities() {
        
        return $.ajax({
            type: "POST",           
            url: rootPath + "Parametrization/City/GetCities"
        });
    }

    static SetListCities(data) {
        $("#lsvCities").UifListView("refresh");
        $.each(data, function (index, val) {
            $("#lsvCities").UifListView("addItem", val);
        });       
    }

    static GetCountries() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/City/GetCountries',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetStatesTotal() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/City/GetStates',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static getStates() {
        if ($("#ddlCountries").val() != null && $("#ddlCountries").val() != "") {
            var countryId = $("#ddlCountries").val();
            CityParametrization.GetListStates(countryId).done(function (data) {
                if (data.success) {
                    $("#ddlStates").UifSelect({ sourceData: data.result, native: false, filter: true });
                }
            });
        }
        else
            $('#ddlStates').UifSelect({ source:null  });                
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

    static GetListCityByDescription(cityDescription) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/City/GetCitiesByDescription?cityDescription=' + cityDescription,
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static clearFormCities() {
        $("#Description").val("");
        $("#SmallDescription").val("");        
        $("#ddlCountries").UifSelect("setSelected", null);
        $("#ddlStates").UifSelect("setSelected", null);
        ClearValidation("#formcities");
    }

    static saveCities() {
        if ($('#IdCityH').val() == 0 || $('#IdCityH').val() =="") {
            cityInsert = {
                Country: { Id: $("#ddlCountries").UifSelect("getSelected") },
                State: { Id: $("#ddlStates").UifSelect("getSelected") },
                Description: $("#Description").val(),
                SmallDescription: $("#SmallDescription").val(),
                Id: 0
            };
        }
        else {
            cityInsert = {
                Country: { Id: $("#ddlCountries").UifSelect("getSelected") },
                State: { Id: $("#ddlStates").UifSelect("getSelected") },
                Description: $("#Description").val(),
                SmallDescription: $("#SmallDescription").val(),
                Id: $('#IdCityH').val()
            }
        }
        
        if ($('#formcities').valid()) {

            if ($('#IdCityH').val() != 0 ) {               

                $.ajax({
                    type: "POST",
                    url: rootPath + 'Parametrization/City/UpdateCity',
                    data: JSON.stringify({ cityViewModel: cityInsert }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {                    
                    if (data.success) {
                        cityInsert = null;
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

                        CityParametrization.GetCities().done(function (n) {
                            if (n.success) {
                                CityParametrization.SetListCities(n.result);
                            } else {
                                $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCities, 'autoclose': true });
                            }
                        });
                        CityParametrization.clearFormCities();
                        $('#IdCityH').val(0);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CitiesSaveError, 'autoclose': true })
                });
            }
            else {
                
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Parametrization/City/CreateCity',
                    data: JSON.stringify({ cityViewModel: cityInsert }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    var dataNum = data;
                    if (data.success) {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        cityInsert = null;
                        CityParametrization.GetCities().done(function (n) {
                            if (n.success) {
                                CityParametrization.SetListCities(n.result);
                            } else {
                                $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCities, 'autoclose': true });
                            }
                        });
                        CityParametrization.clearFormCities()
                        $('#IdCityH').val(0);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CitiesSaveError, 'autoclose': true })
                });
            }          
        }
     
    }
    
    static showData(event, result, index) {
        CityParametrization.clearFormCities();
        $('#IdCityH').val(result.Id); 
        if (result.Id != undefined) {
            $('#cityCode').val(result.Id)
            $('#Description').val(result.Description);
            $('#SmallDescription').val(result.SmallDescription);
            $("#ddlCountries").UifSelect("setSelected", result.Country.Id);        
            CityParametrization.getStates();
            $("#ddlStates").UifSelect("setSelected", result.State.Id);
                      
        }
            
    }
    // carga los datos de la vista de busqueda avanzada
    static showDataSearch(result) {
        CityParametrization.clearFormCities();
        

        if (result.Id != undefined) {
            $('#IdCityH').val(result.Id); 

            $('#cityCode').val(result.Id)
            $('#Description').val(result.Description);
            $('#SmallDescription').val(result.SmallDescription);
            $("#ddlCountries").UifSelect("setSelected", result.Country.Id);
            CityParametrization.getStates();
            $("#ddlStates").UifSelect("setSelected", result.State.Id);
            
        }

    }
 
    static searchRangeCity(event, value) {
        
        if (value != null && value.length > 2) {

            $("#lsvCities").UifListView("clear");
            $("#searchCity").val("");
            CityParametrization.GetListCityByDescription(value).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0)
                        CityParametrization.SetListCities(data.result);
                    else
                        $.UifNotify("show", { 'type': "info", 'message': Resources.Language.ListCitiesNotFound, 'autoclose': true });
                }
                else {
                    $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCities, 'autoclose': true });
                }

            });
        }
        else {
            $.UifNotify("show", { 'type': "info", 'message': Resources.Language.MessageInfoMinimumChar, 'autoclose': true });
        }
    }
   
    static ExportFile() {
      
        request('Parametrization/City/ExportFileCities', JSON.stringify({}), 'POST', AppResources.ErrorExportCity, CityParametrization.ExportFileSuccess);
    }

    static ExportFileSuccess(data) {
        DownloadFile(data);
    }

    static deleteRowCity(deferred, result) {      

        CityParametrization.clearFormCities()
        
        var CityDelete = {
            Id: result.Id,
            Country: result.Country,
            State: result.State,
            Description: result.Description            
        };

        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/City/DeleteCity',
            data: JSON.stringify({ cityViewModel: CityDelete }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            var dataNum = data;
            if (data.success) {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                CityParametrization.GetCities().done(function (n) {
                    if (n.success) {
                        CityParametrization.SetListCities(n.result);
                    } else {
                        $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorGetListCities, 'autoclose': true });
                    }
                });

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });              
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CitiesDeleteError, 'autoclose': true })
        });                   
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }
}