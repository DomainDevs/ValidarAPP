var glbObjectsDelete = [];
var objectIndex = null;
var formBranch = {};
var index;
var cityId;
var stateId;
$.ajaxSetup({ async: true });

class BranchParametrization extends Uif2.Page {
    /**
  * @summary 
     *  Metodo que se ejecuta al instanciar la clase     
  */
    getInitialState() {
        //Se cargan los datos en los campos iniciales
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        request('Parametrization/Branch/GetBranches', null, 'POST', Resources.Language.ErrorExistBranch, BranchParametrization.LoadListViewBranch);
        request("Parametrization/Branch/GetCountries", {}, "POST", Resources.Language.ErrorSearchCountry, BranchParametrization.SetContries);
        request("Parametrization/Branch/GetPhoneType", {}, "GET", Resources.Language.ErrorSearchCountry, BranchParametrization.SetPhoneType);
        request("Parametrization/Branch/GetAddressType", {}, "GET", Resources.Language.ErrorSearchCountry, BranchParametrization.SetAddressType);
        $('#chkIsIssue').prop("checked", true);
       
    }

    /**
   * @summary 
       *  Metodo con los eventos de todos los controles 
   */
    bindEvents() {
        $("#selectCountry").on("itemSelected", BranchParametrization.ChangeCountry);
        $("#selectState").on("itemSelected", BranchParametrization.ChangeState);
        $('#btnNewObject').click(BranchParametrization.Clear);
        $('#inputBranch').on('buttonClick', this.SearchBranch);
        $('#btnExit').click(this.Exit);
        $('#btnAcceptObject').click(this.AddObject);
        $('#btnSave').click(this.SaveBranches);
        $('#listViewBranch').on('rowEdit', BranchParametrization.ShowData);
        $('#btnExport').click(this.sendExcelBranch);

    }

    AddObject() {
        $("#formBranch").validate();
        if ($("#formBranch").valid()) {
            var object = BranchParametrization.GetForm();
            if ($("#Id").val() == "") {
                object.Id = 0;
            }
            else {
                object.Id = parseInt($("#Id").val());
            }
            if (object.IsIssue) {
                object.IsIssue = Resources.Language.IsIssue;
            } else {
                object.IsIssue = Resources.Language.IsNotIssue;
            }
            if (objectIndex == null) {
                var lista = $("#listViewBranch").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == object.Description.toUpperCase();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistBranchName, 'autoclose': true });
                }
                else {
                    object.Status = ParametrizationStatus.Create;
                    $("#listViewBranch").UifListView("addItem", object);
                }
            }
            else {
                var lista = $("#listViewBranch").UifListView('getData')
                var ifExist = lista.filter(function (item) {
                    return item.Description.toUpperCase() == object.Description.toUpperCase() && item.Id != $("#Id").val();
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistBranchName, 'autoclose': true });
                }
                else {

                    if (object.Id != 0) {
                        object.Status = ParametrizationStatus.Update;
                    }
                    else {
                        object.Status = ParametrizationStatus.Create;
                    }
                    $('#listViewBranch').UifListView('editItem', objectIndex, object);
                }
            }
            BranchParametrization.Clear();
        }
    }

    SaveBranches() {
        var itemModified = [];
        var dataTable = $("#listViewBranch").UifListView('getData');
        $.each(glbObjectsDelete, function (index, item) {
            item.Status = ParametrizationStatus.Delete;
            itemModified.push(item);
        })
        $.each(dataTable, function (index, value) {
            if (value.Status == 0 || value.Status == undefined) {
                value.Status = ParametrizationStatus.Original
            } if (value.IsIssue == Resources.Language.IsIssue) {
                value.IsIssue = true;
            }
            itemModified.push(value);
        });

        if (itemModified.length > 0) {
            request('Parametrization/Branch/SaveBranches', JSON.stringify({ branch: itemModified }), 'POST', Resources.Language.SaveSurcharge, BranchParametrization.ResultSave);
        }
        glbObjectsDelete = [];
    }

    /**
    * @summary
    * Setea la lista de paises
    * @param {List<Country>} contries
    * Lista de paises
    */
    static SetContries(contries) {
        let comboConfig = { sourceData: contries };
        $("#selectCountry").UifSelect(comboConfig);     
    }

    /**
   * @summary
   * Setea la lista de los tipos de teléfono   
   * Lista los tipos de teléfono   
   */
    static SetPhoneType(data) {
        let comboConfig = { sourceData: data };
        $("#selectPhoneType").UifSelect(comboConfig);
    }

    /**
   * @summary
   * Setea la lista de los tipos de direcciones     
   */
    static SetAddressType(data) {
        let comboConfig = { sourceData: data };
        $("#selectAddressType").UifSelect(comboConfig);
    }

    /**
    * @summary
    * 
    * @param {any} states
    */
    static SetStates(states) {
        let comboConfig = { sourceData: states };
        $("#selectState").UifSelect(comboConfig);

        if (stateId != undefined) {
            $("#selectState").UifSelect('setSelected', stateId);
        }
    }

    /**
    * @summary
    * Evento al cambiar el pais
    * @param {any} e
    * evento
    * @param {Object<Country>} country
    * Objeto seleccionado
    */
    static ChangeCountry(e, country) {

        $("#selectState").UifSelect();
        $("#selectCity").UifSelect();
        if (country.Id !== "") {
            request("Parametrization/Branch/GetStatesByCountry", JSON.stringify({ countryId: country.Id }), "POST", Resources.Language.ErrorConsultingDepartments, BranchParametrization.SetStates);
        }
    }

    /**
    * @summary
    * Evento al cambiar de Estado
    * @param {any} e
    * Evento
    * @param {Object<State>} state
    * Estado seleccionado
    */
    static ChangeState(e, state) {

        if (state.Id !== "") {
            const countryId = $("#selectCountry").UifSelect("getSelected");
            request("Parametrization/Branch/GetCitiesByCountryState", JSON.stringify({ countryId: countryId, stateId: state.Id }), "POST", Resources.Language.ErrorQueryingCities, BranchParametrization.SetCities);
        }
    }

    /**
    * @summary
    * setea la lista de cuidades
    * @param {List<City>} cities
    * Lista de ciudades
    */
    static SetCities(cities) {
        let comboConfig = { sourceData: cities };
        $("#selectCity").UifSelect(comboConfig);
        if (cityId != undefined) {
            $("#selectCity").UifSelect('setSelected', cityId);
        }
    }

    static ResultSave(data) {
        request('Parametrization/Branch/GetBranches', null, 'POST', Resources.Language.ErrorExistBranch, BranchParametrization.LoadListViewBranch);
        if (data.Message === null) {
            data.Message = 0;
        }
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
            AppResources.Aggregates + ':' + data.TotalAdded + '<br> ' +
            AppResources.Updated + ':' + data.TotalModified + '<br> ' +
            AppResources.Removed + ':' + data.TotalDeleted + '<br> ' +
            AppResources.Errors + ':' + data.Message,
            'autoclose': true
        });
    }

    static DeleteItemBrancht(deferred, result) {
        deferred.resolve();
        if (result.Id !==0 && result.Id !== "" && result.Id !== undefined) //Se elimina unicamente si existe en DB
        {
            result.Status = ParametrizationStatus.Delete;
            //glbObjectsDelete.push(result);
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listViewBranch").UifListView("addItem", result);
        }
       
        BranchParametrization.Clear();
    }

    SearchBranch() {
        var inputObject = $('#inputBranch').val();
        if (inputObject.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar });
        }

        else {
            request('Parametrization/Branch/GetCoBranchesByDescription', JSON.stringify({ description: inputObject }), 'POST', Resources.Language.ErrorExistBranch, BranchParametrization.ShowSearchAdv)
        }
        $('#inputBranch').val("");
    }

    static ShowSearchAdv(data) {
        if (data.length != 0) {
            $("#lvSearchAdvBranch").UifListView("clear");
            if (data.length === 1) {

                var list = $("#listViewBranch").UifListView('getData');
                $.each(list, function (key, value) {
                    if (value.Id == data[0].Id) {
                        index = key;
                    }
                });
                var object = {
                    Description: data[0].Branch.Description,
                    BranchId: data[0].Branch.Id,
                    IsIssue: data[0].IsIssue,
                    Adress: data[0].Address,
                    City: data[0].City.Id,
                    Country: data[0].Country.Id,
                    Id: data[0].Id,
                    PhoneNumbre: data[0].PhoneNumber,
                    PhoneType: data[0].PhoneType.Id,
                    State: data[0].State.Id,
                    SmallDescription: data[0].Branch.SmallDescription,
                    AddressType: data[0].AddressType.Id
                }
                if (object.IsIssue) {
                    object.IsIssue = Resources.Language.IsIssue;
                } else {
                    object.IsIssue = Resources.Language.IsNotIssue;
                }
                BranchParametrization.ShowData(null, object, index);
               
            } else {
                $.each(data, function (key, value) {
                    var object = {
                        Description: this.Branch.Description,
                        BranchId: this.Branch.Id,
                        IsIssue: this.IsIssue,
                        Adress: this.Address,
                        City: this.City.Id,
                        Country: this.Country.Id,
                        Id: this.Id,
                        PhoneNumbre: this.PhoneNumber,
                        PhoneType: this.PhoneType.Id,
                        State: this.State.Id,
                        SmallDescription: this.Branch.SmallDescription,
                        AddressType: this.AddressType.Id
                    }
                    if (object.IsIssue) {
                        object.IsIssue = Resources.Language.IsIssue;
                    } else {
                        object.IsIssue = Resources.Language.IsNotIssue;
                    }
                    $("#lvSearchAdvBranch").UifListView("addItem", object);
                });
                BranchAdvancedSearch.SearchAdvBranch();
            }
        }
    }

    static DownloadFile(data) {
        DownloadFile(data);
    }

    sendExcelBranch() {
        request('Parametrization/Branch/GenerateFileToExport', null, 'POST', Resources.Language.ErrorExistBranch, BranchParametrization.DownloadFile);
    }

    SelectSearch() {
        var list = $("#listViewBranch").UifListView('getData')
        var index = $(this).children()[0].innerHTML;
        var object = [];
        $.each(list, function (key, value) {
            if (value.Id == index) {
                object.push(value);
            }
        });
        BranchParametrization.ShowData(null, object[0], index);
        $('#inputBranch').val("");
        $('#modalDefaultSearch').UifModal("hide");
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    static LoadListViewBranch(data) {
        $("#listViewBranch").UifListView({ displayTemplate: "#BranchTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: BranchParametrization.DeleteItemBrancht, height: 300 });
        data.forEach(item => {
                var object = {
                    Description: item.Branch.Description,
                    BranchId: item.Branch.Id,
                    IsIssue: item.IsIssue,
                    Adress: item.Address,
                    City: item.City.Id,
                    Country: item.Country.Id,
                    Id: item.Id,
                    PhoneNumbre: item.PhoneNumber,
                    PhoneType: item.PhoneType.Id,
                    State: item.State.Id,
                    SmallDescription: item.Branch.SmallDescription,
                    AddressType: item.AddressType.Id
                }
                if (object.IsIssue) {
                    object.IsIssue = Resources.Language.IsIssue;
                } else {
                    object.IsIssue = Resources.Language.IsNotIssue;
                }
                $("#listViewBranch").UifListView("addItem", object);
        });
    }

    static GetForm() {
        var data = {};
        $("#formBranch").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.Description = $("#inputDescription").val();
        data.SmallDescription = $("#inputSmallDescription").val();
        data.Id = $("#Id").val();
        data.BranchId = $("#Id").val();       
        data.IsIssue = $("#chkIsIssue").is(":checked");
        data.Adress = $("#inputAdress").val();
        data.City = $("#selectCity").UifSelect("getSelected");
        data.Country = $("#selectCountry").UifSelect("getSelected");
        data.PhoneNumbre = $("#inputPhoneNumbre").val();
        data.PhoneType = $("#selectPhoneType").UifSelect("getSelected");
        data.State = $("#selectState").UifSelect("getSelected");
        data.AddressType = $("#selectAddressType").UifSelect("getSelected");

        return data;
    }

    static ShowData(event, result, index) {
        BranchParametrization.Clear();
        if (result.Id != undefined) {
            objectIndex = index;
            cityId = result.City;
            stateId = result.State;
            $("#Id").val(result.Id);
            $("#inputDescription").val(result.Description);
            $("#inputSmallDescription").val(result.SmallDescription);           
            if (result.State !== "") {              
                request("Parametrization/Branch/GetCitiesByCountryState", JSON.stringify({ countryId: result.Country, stateId: result.State }), "POST", Resources.Language.ErrorQueryingCities, BranchParametrization.SetCities);
            }
            if (result.Country !== "") {
                request("Parametrization/Branch/GetStatesByCountry", JSON.stringify({ countryId: result.Country }), "POST", Resources.Language.ErrorConsultingDepartments, BranchParametrization.SetStates);
            }
            $("#inputAdress").val(result.Adress);
            $("#inputPhoneNumbre").val(result.PhoneNumbre);
            $("#selectCountry").UifSelect('setSelected', result.Country);
            $("#selectPhoneType").UifSelect('setSelected', result.PhoneType);
            $("#selectAddressType").UifSelect('setSelected', result.AddressType);
            if (result.IsIssue == Resources.Language.IsIssue) {
                $('#chkIsIssue').prop("checked", true);
            } else {
                $('#chkIsIssue').prop("checked", false);
            }
        }
    }
    /**
       * @summary 
       * Limpiar formulario
    */
    static Clear() {
        $("#Id").val("");
        $("#inputDescription").val("");
        $("#inputDescription").focus();
        $("#inputSmallDescription").val("");
        $("#selectState").UifSelect();
        $("#selectCity").UifSelect();
        $("#inputAdress").val("");
        $("#inputPhoneNumbre").val("");       
        $("#selectCountry").UifSelect('setSelected', null);
        $("#selectPhoneType").UifSelect('setSelected', null);
        $("#selectAddressType").UifSelect('setSelected', null);
        objectIndex = null;
        cityId = null;
        stateId = null;
        ClearValidation("#formBranch");
    }

}