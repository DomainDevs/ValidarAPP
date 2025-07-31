var addresses = [];
var addressesGet = [];
var rowAddressId = -1;
var DataId = 0;
var heightListView = 256;

class Address extends Uif2.Page {
    getInitialState() {
        $("#inputDaneCode").UifAutoComplete({
            source: rootPath + "Person/Person/GetDaneCodeByQuery",
            displayKey: "DANECode",
            queryParameter: "&query"
        });
        this.InitializeAdress();
        Address.ClearAddress();
    }

    //Seccion Eventos --Siempre deben Ejecutarse cuando se carga el Dom
    bindEvents() {
        $("#btnCreateAddress").click(this.BtnCreateAddress);
        $("#btnAcceptAddress").click(this.AcceptAddress);
        $('#btnNewAddress').click(Address.ClearAddress);
        $("#btnCloseAddress").click(this.BtnCloseAddress);
        $('#listAddresses').on('rowEdit', this.AddressesEdit);
        // << EESGE-172 Control De Busqueda               
        $('#inputCountryAdPn').on("search", this.SearchCountryAdPn);
        $("#inputDaneCode").on('itemSelected', this.DaneCodeSelected);
        $('#inputStateAdPn').on("search", this.SearchStateAdPn);
        $('#inputCityAdPn').on("search", this.SearchCityAdPn);
        $('#tblResultListCountriesAdPn tbody').on('click', 'tr', this.SelectSearchCountriesAdPn);
        $('#tblResultListStatesAdPn tbody').on('click', 'tr', this.SelectSearchStatesAdPn);
        $('#tblResultListCitiesAdPn tbody').on('click', 'tr', this.SelectSearchCitiesAdPn);
        // EESGE-172 Control De Busqueda >>
    }

    static CleanObjectAddress() {
        addresses = [];
        addressesGet = [];
        $("#listAddresses").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#addresTemplate", height: heightListView });
    }

    InitializeAdress() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#inputDaneCode').attr('disabled', false);
        $('#inputDaneCode').ValidatorKey();
        $('#inputAddressAll').ValidatorKey(ValidatorType.Addresses, 1, 1);
        $("#inputCountryAdPn").ValidatorKey(2);
        $("#inputStateAdPn").ValidatorKey(2);
        $("#inputCityAdPn").ValidatorKey(2);
    }

    static ClearAddress() {
        $("#hidennAddressId").val(0);
        $("#inputAddressAll").val("");
        $('#rdPrincipal').attr('checked', true);
        $("#selectAddress").UifSelect("setSelected", null);
        $("#inputCountryAdPn").val("");
        $("#inputStateAdPn").val("");
        $("#inputCityAdPn").val("");
        $("#inputCountryAdPn").data({ Id: null, Description: null });
        $("#inputStateAdPn").data({ Id: null, Description: null });
        $("#inputCityAdPn").data({ Id: null, Description: null });
        $('#inputDaneCode').attr('disabled', false);
        $('#inputDaneCode').val("");
        //$('#inputCompanyDaneCode').attr('disabled', false);
        //$('#inputCompanyDaneCode').val("");
        //$('#inputCompanyState').data({ Id: null, Description: null });
        //$('#inputCompanyCity').data({ Id: null, Description: null });
        Address.InitializeGloblalAddress();
        // Se inicializa y setea el país por defecto   
        Persons.GetdefaultValueCountry();
    }

    BtnCreateAddress() {
        if (Address.ValidateAddress()) {
            Address.CreateAddress();
            Address.ClearAddress();
        }
    }

    AcceptAddress() {
        if (Address.ValidateAddresses()) {
            Address.SaveAddresses();
            Address.ClearAddress();
        }
    }

    BtnCloseAddress() {
        Address.ClearAddress();
        Address.CloseAddress();
    }

    AddressesEdit(event, data, index) {
        rowAddressId = index;
        Address.EditAddress(data, index);
    }

    SearchCountryAdPn() {
        $('#inputStateAdPn').val("");
        $('#inputCityAdPn').val("");
        $('#inputDaneCode').val("");
        if ($("#inputCountryAdPn").val().trim().length > 0) {
            Address.GetAddressPersonCountriesByDescription($("#inputCountryAdPn").val());
        }
    }

    DaneCodeSelected() {
        Address.GetAddressPersonCountryAndStateAndCityByDaneCode($('#inputCountryAdPn').data("Id"), $("#inputDaneCode").val());
    }

    SearchStateAdPn(event, value) {
        $('#inputCityAdPn').val("");
        $('#inputDaneCode').val("");
        if ($("#inputStateAdPn").val().trim().length > 0) {
            Address.GetAddressPersonStatesByCountryIdByDescription($("#inputCountryAdPn").data('Id'), $("#inputStateAdPn").val());
        }
    }

    SearchCityAdPn(event, value) {
        $('#inputDaneCode').val("");
        if ($("#inputCityAdPn").val().trim().length > 0) {
            Address.GetAddressPersonCitiesByCountryIdByStateIdByDescription($("#inputCountryAdPn").data('Id'), $("#inputStateAdPn").data('Id'), $("#inputCityAdPn").val());
        }
    }

    SelectSearchCountriesAdPn() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCountryAdPn").data(dataCountry);
        $("#inputCountryAdPn").val(dataCountry.Description);
        $('#modalListSearchCountriesAdPn').UifModal('hide');
    }

    SelectSearchStatesAdPn() {
        var dataState = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputStateAdPn").data(dataState);
        $("#inputStateAdPn").val(dataState.Description);
        $('#modalListSearchStatesAdPn').UifModal('hide');
    }

    SelectSearchCitiesAdPn() {
        var dataCities = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputCityAdPn").data(dataCities);
        $("#inputCityAdPn").val(dataCities.Description);
        $('#modalListSearchCitiesAdPn').UifModal('hide');
        Address.LoadCity(dataCities.Id);
    }
    static InitializeGloblalAddress() {
        rowAddressId = -1;
        DataId = 0;
    }

    //Validaciones
    static ValidateAddress() {
        var msj = "";
        if ($("#selectAddress").UifSelect("getSelected") == null || $("#selectAddress").UifSelect("getSelected") == "") {
            msj = AppResources.LabelTypeAddress + "<br>"
        }
        if ($("#inputAddressAll").val() == null || $("#inputAddressAll").val() == "") {
            msj = msj + "Dirección<br>"
        }
        if ($("#inputCountryAdPn").data('Id') == null || $("#inputStateAdPn").data('Id') == "") {
            msj = msj + "País<br>"
        }
        else if ($("#inputCountryAdPn").data('Id') == countryParameter) {
            if ($("#inputStateAdPn").data('Id') == null || $("#inputStateAdPn").data('Id') == "") {
                msj = msj + "Departamento<br<"
            }
            if ($("#inputCityAdPn").data('Id') == null || $("#inputCityAdPn").data('Id') == "") {
                msj = msj + "Cíudad<br>"
            }
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msj, 'autoclose': true })
            return false;
        } else if (Address.DuplicateAddress($("#selectAddress").val(), $("#hidennRowId").val())) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorAddressDuplicate, 'autoclose': true });
            return false;
        }
        else if (Address.PrincipalAddress($("#hidennRowId").val())) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorAddressMain, 'autoclose': true });
            return false;
        }
        return true;
    }

    static ValidateAddresses() {
        var validate = false;
        var countMailAddress = 0;
        $.each($("#listAddresses").UifListView('getData'), function (i, item) {
            if (item.IsPrincipal == 1) {
                countMailAddress++;
            };
        });
        if (countMailAddress != 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorAddressMain, 'autoclose': true })
        }
        else {
            validate = true;
        }
        return validate;
    }

    //Creacion
    static CreateAddress() {

        var addressTmp = Address.CreateAddresModel();
        if (rowAddressId == -1) {
            $("#listAddresses").UifListView("addItem", addressTmp);
        }
        else {
            $("#listAddresses").UifListView("editItem", rowAddressId, addressTmp);
        }
    }

    static SaveAddresses() {

        Address.SendAddresses();
        switch (searchType) {
            case TypePerson.PersonNatural:
                Address.setPrincipalAddress();
                break;
            case TypePerson.PersonLegal:
                Address.setPrincipalAddressLegal();
                break;
            default:
                break;
        }
        Address.CloseAddress();
    }

    static CloseAddress() {
        $('#modalAddress').UifModal('hide');
    }

    //seccion edit
    static EditAddress(data, index) {

        DataId = data.Id;
        $("#inputAddressAll").val(data.Description);
        $("#selectAddress").UifSelect("setSelected", data.AddressTypeId);
        $('#rdPrincipal').prop("checked", data.IsPrincipal);
        var dataCountry = {
            Id: data.CountryId,
            Description: data.CountryDescription
        }

        $("#inputCountryAdPn").data(dataCountry);
        $("#inputCountryAdPn").val(dataCountry.Description);

        $("#inputDaneCode").val(data.DANECode);

        var dataState = {
            Id: data.StateId,
            Description: data.StateDescription
        }

        $("#inputStateAdPn").data(dataState);
        $("#inputStateAdPn").val(dataState.Description);

        var dataCity = {
            Id: data.CityId,
            Description: data.CityDescription
        }

        $("#inputCityAdPn").data(dataCity);
        $("#inputCityAdPn").val(dataCity.Description);

    }
    static setPrincipalAddress() {
        $.each($("#listAddresses").UifListView('getData'), function (i, item) {
            if (item.IsPrincipal == true) {
                $("#selectCompanyAddressType").UifSelect("setSelected", item.AddressTypeId);
                $("#inputAddressAllPn").val(item.Description);
                $("#inputStatePn").val(item.StateDescription);
                $("#inputCityPn").val(item.CityDescription);
                Address.LoadDaneCodePersonNatural(item.CountryId, item.StateId, item.CityId);
                var AddressModel = { AddressType: {}, State: { Country: {} } };
                AddressModel.Id = item.CityId;
                AddressModel.Description = item.CityDescription;
                AddressModel.DANECode = item.DANECode;
                AddressModel.State.Id = item.StateId;
                AddressModel.State.Description = item.StateDescription;
                AddressModel.State.Country.Id = item.CountryId;
                AddressModel.State.Country.Description = item.CountryDescription;
                PersonNatural.GetCountriesPerson(AddressModel);
            }
        });
    }

    static setPrincipalAddressLegal(addresses) {
        var listAddress = $("#listAddresses").UifListView('getData');
        $.each(listAddress, function (i, item) {
            if (item.IsPrincipal == true || listAddress.length == 1) {
                $("#selectCompanyAddressType").UifSelect("setSelected", item.AddressTypeId);
                $("#inputCompanyAddress").val(item.Description);
                $("#inputCompanyState").val(item.StateDescription);
                $("#inputCompanyCity").val(item.CityDescription);
                Address.LoadDaneCodePersonLegal(item.CountryId, item.StateId, item.CityId);

                var AddressModel = { AddressType: {}, State: { Country: {} } };
                AddressModel.Id = item.CityId;
                AddressModel.Description = item.CityDescription;
                AddressModel.DANECode = item.DANECode;
                AddressModel.State.Id = item.StateId;
                AddressModel.State.Description = item.StateDescription;
                AddressModel.State.Country.Id = item.CountryId;
                AddressModel.State.Country.Description = item.CountryDescription;
                PersonLegal.GetCountriesPersonLegal(AddressModel);
            }
        });
    }

    static UpdatePrincipalAddress() {

        $.each(addresses, function (i, item) {
            if (item.IsPrincipal == true) {
                item.AddressTypeId = $("#selectCompanyAddressType").UifSelect("getSelected");
                item.Description = $("#inputAddressAllPn").val();
                item.CityId = $("#inputCityPn").data("Id");
                item.StateId = $("#inputStatePn").data("Id");
                item.CountryId = $("#inputCountryPn").data("Id");
                item.AplicationStaus = 1;
            }
            else {
                item.AddressTypeId = item.AddressTypeId;
                item.Description = item.Description;
                item.CityId = item.CityId;
                item.CityDescription = item.CityDescription;
                item.StateId = item.StateId;
                item.StateDescription = item.StateDescription;
                item.CountryId = item.CountryId;
            }
        });
    }
    static UpdatePrincipalAddressCompany() {
        $.each(addresses, function (i, item) {
            if (item.IsPrincipal === true) {
                item.AddressTypeId = $("#selectCompanyAddressType").UifSelect("getSelected");

                item.Description = $("#inputCompanyAddress").val();
                item.CityId = $("#inputCompanyCity").data("Id");
                item.StateId = $("#inputCompanyState").data("Id");
                item.CountryId = $("#inputCompanyCountry").data("Id");
                item.AplicationStaus = 1;

            }
            else {
                item.AddressTypeId = item.AddressTypeId;
                item.Description = item.Description;
                item.CityId = item.CityId;
                item.CityDescription = item.CityDescription;
                item.StateId = item.StateId;
                item.StateDescription = item.StateDescription;
                item.CountryId = item.CountryId;



            }
        });
    }

    static GetAddressPersonCountriesByDescription(description) {
        if (description.length >= 3) {
            CountryRequest.GetCountriesByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result !== null && data.result.length > 0) {
                        var dataCountries = [];
                        $.each(data.result, function (index, value) {
                            dataCountries.push({
                                Id: value.Id,
                                Description: value.Description
                            });
                        });
                        $('#tblResultListCountriesAdPn').UifDataTable('clear');
                        $("#tblResultListCountriesAdPn").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchCountriesAdPn').UifModal('showLocal', AppResourcesPerson.ModalTitleCountries);
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // DANECode
    static GetAddressPersonCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        DaneCodeRequest.GetCountryAndStateAndCityByDaneCode(countryId, daneCode).done(function (data) {
            if (data.success) {
                if (data.result !== null && Object.keys(data.result).length > 0) {
                    $("#inputCountryAdPn").data({ Id: data.result.State.Country.Id, Description: data.result.State.Country.Description });
                    $("#inputStateAdPn").data({ Id: data.result.State.Id, Description: data.result.State.Description });
                    $("#inputCityAdPn").data({ Id: data.result.Id, Description: data.result.Description });
                    $("#inputCountryAdPn").val($("#inputCountryAdPn").data("Description"));
                    $("#inputStateAdPn").val($("#inputStateAdPn").data("Description"));
                    $("#inputCityAdPn").val($("#inputCityAdPn").data("Description"));
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageNotFoundCountries, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    // State
    static GetAddressPersonStatesByCountryIdByDescription(countryId, description) {
        if (description.length >= 3) {
            if (countryId != undefined) {
                StateRequest.GetStatesByCountryIdByDescription(countryId, description).done(function (data) {
                    if (data.success) {
                        if (data.result !== null && data.result.length > 0) {
                            var dataStates = [];
                            $.each(data.result, function (index, value) {
                                dataStates.push({
                                    Id: value.Id,
                                    Description: value.Description
                                });
                            });

                            $('#tblResultListStatesAdPn').UifDataTable('clear');
                            $("#tblResultListStatesAdPn").UifDataTable('addRow', dataStates);
                            $('#modalListSearchStatesAdPn').UifModal('showLocal', AppResourcesPerson.ModalTitleStates);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorDocumentControlCountry, 'autoclose': true })
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // City
    static GetAddressPersonCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
        if (description.length >= 3) {
            if (countryId != undefined && stateId != undefined) {
                CityRequest.GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description).done(function (data) {
                    if (data.success) {
                        if (data.result !== null && data.result.length > 0) {
                            var dataCities = [];
                            $.each(data.result, function (index, value) {
                                dataCities.push({
                                    Id: value.Id,
                                    Description: value.Description
                                });
                            });

                            $('#tblResultListCitiesAdPn').UifDataTable('clear');
                            $("#tblResultListCitiesAdPn").UifDataTable('addRow', dataCities);
                            $('#modalListSearchCitiesAdPn').UifModal('showLocal', AppResourcesPerson.ModalTitleCities);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorValidateCountryState, 'autoclose': true })
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    static GetAddresses() {
        var tempAdress = [];

        $("#listAddresses").UifListView({ sourceData: addresses, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#addresTemplate", height: heightListView });
        var contRowsAddress = $("#listAddresses").UifListView('getData').length;
        if (contRowsAddress > 0) {
            $.each($("#listAddresses").UifListView('getData'), function (key, value) {
                if (this.IsPrincipal) {
                    switch (searchType) {
                        case TypePerson.PersonNatural:

                            $("#listAddresses").UifListView("editItem", key, Address.SetAddressForId(value));
                            break;
                        case TypePerson.PersonLegal:
                            $("#listAddresses").UifListView("editItem", key, Address.SetAddressCompanyById(value));
                            break;
                    }
                }
                else {
                    switch (searchType) {
                        case TypePerson.PersonNatural:
                            $("#listAddresses").UifListView("editItem", key, Address.SetAddressForId(value));
                            break;
                        case TypePerson.PersonLegal:
                            $("#listAddresses").UifListView("editItem", key, Address.SetAddressCompanyById(value));
                            break;
                    }
                }
            });

        }
        else {
            switch (searchType) {
                case TypePerson.PersonNatural:
                    tempAdress.push(Address.SetAddress(0));
                    break;
                case TypePerson.PersonLegal:
                    tempAdress.push(Address.SetAddressCompany(0));
                    break;

            }
            $("#listAddresses").UifListView({ sourceData: tempAdress, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#addresTemplate", height: heightListView });
            addresses = tempAdress;
        }
    }

    static FillObjectForMainAddress() {
        addresses.push({
            AddressTypeId: $("#selectCompanyAddressType").val(),
            CityId: $("#inputCityPn").data("Id"),
            CityDescription: $("#inputCityPn").data("Description"),
            StateId: $("#inputStatePn").data("Id"),
            StateDescription: $("#inputStatePn").data("Description"),
            CountryId: $("#inputCountryPn").data("Id"),
            CountryDescription: $("#inputCountryPn").data("Description"),
            Description: $("#inputAddressAllPn").val(),
            IsPrincipal: $('#rdPrincipalPn').is(':checked'),
            StreetTypeId: null,
            AplicationStaus: 0
        });
    }

    static SendAddresses() {
        if ($("#listAddresses").UifListView('getData').length > 0) {
            addresses = $("#listAddresses").UifListView('getData');
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorAddressEmpty, 'autoclose': false })
        }
    }

    static CreateAddresModel() {
        var Address = { AddressType: {}, City: { State: { Country: {} } } };
        Address.Id = DataId;
        Address.Description = $("#inputAddressAll").val();
        Address.AddressTypeId = $("#selectAddress").UifSelect("getSelected");
        Address.AddressType.Description = $("#selectAddress").UifSelect("getSelectedText");
        Address.IsPrincipal = $('#rdPrincipal').is(':checked');
        Address.CityId = $("#inputCityAdPn").data('Id');
        Address.CityDescription = $("#inputCityAdPn").data('Description');
        Address.City.DANECode = $("#inputDaneCode").val();
        Address.StateId = $("#inputStateAdPn").data('Id');
        Address.StateDescription = $("#inputStateAdPn").data('Description');
        Address.CountryId = $("#inputCountryAdPn").data('Id');
        Address.CountryDescription = $("#inputCountryAdPn").data('Description');
        if (Address.Id == 0) {
            Address.AplicationStaus = 0;
        }
        else {
            Address.AplicationStaus = 1;
        }

        return Address;
    }


    //Seccion Get Load
    static LoadAddress(Address) {

        $("#listAddresses").UifListView({ sourceData: Address, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#addresTemplate", height: heightListView });
    }

    static LoadCity(selectedItem) {
        if (selectedItem > 0) {
            DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCountryAdPn").data("Id"), $("#inputStateAdPn").data("Id"), $("#inputCityAdPn").data("Id")).done(function (data) {
                if (data.success) {
                    $('#inputDaneCode').val("");
                    $('#inputDaneCode').val(data.result);
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static SetMainAddressPersonLegal() {
        addresses.push({
            Id: 0,
            Description: $("#inputCompanyAddress").val(),
            AddressType: { Id: $("#selectCompanyAddressType").UifSelect("getSelected") },
            CityId: $("#inputCompanyCity").data("Id"),
            StateId: $("#inputCompanyState").data("Id"),
            CountryId: $("#inputCompanyCountry").data("Id"),
            AddressTypeId: $("#selectCompanyAddressType").UifSelect("getSelected"),
            DANECode: $("#inputCompanyDaneCode").UifSelect("getSelected"),
            IsPrincipal: true,
            AplicationStaus: 0
        });
    }

    static SetAddress(id) {
        var address = { AddressType: {}, City: { State: { Country: {} } } };
        address.Id = id;
        //address.UpdateUser = addresses[0].UpdateUser,
        //    address.UpdateDate = FormatDate(addresses[0].UpdateDate, 1),
        address.Description = $("#inputAddressAllPn").val();
        address.AddressTypeId = $("#selectCompanyAddressType").UifSelect("getSelected");
        address.AddressType.Description = $("#selectCompanyAddressType").UifSelect("getSelectedText");
        address.IsPrincipal = $('#rdPrincipalPn').is(':checked');
        address.City.Id = $("#inputCityPn").data("Id");
        address.City.Description = $("#inputCityPn").data("Description");
        address.City.DANECode = $("#inputDaneCodePn").val();
        address.City.State.Id = $("#inputStatePn").data("Id");
        address.City.State.Description = $("#inputStatePn").data("Description");
        address.City.State.Country.Id = $("#inputCountryPn").data("Id");
        address.City.State.Country.Description = $("#inputCountryPn").data("Description");
        address.AplicationStaus = 3;
        return address;
    }

    static SetAddressForId(value) {
        var address = { AddressType: {}, City: { State: { Country: {} } }, };
        address.Id = value.Id;
        $("#selectAddress").UifSelect("setSelected", value.AddressTypeId);
        address.Description = value.Description;
        address.AddressTypeId = value.AddressTypeId;
        address.AddressType.Description = $("#selectAddress").UifSelect("getSelectedText");
        address.IsPrincipal = value.IsPrincipal
        var DANECode = DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId(value.CountryId, value.StateId, value.CityId).done(function (data) {
            if (data.success) {
                address.DANECode = data.result;
            }
        });
        address.CountryId = value.CountryId;
        address.CountryDescription = value.CountryDescription;
        address.StateId = value.StateId;
        address.StateDescription = value.StateDescription;
        address.CityId = value.CityId;
        address.CityDescription = value.CityDescription;
        address.AplicationStaus = 3;
        $("#selectAddress").UifSelect("setSelected", null);
        return address;
    }

    static SetAddressCompanyById(value) {
        var address = { AddressType: {}, City: { State: { Country: {} } }, };
        address.Id = value.Id;
        //address.UpdateUser = addresses[0].UpdateUser,
        //    address.UpdateDate = FormatDate(addresses[0].UpdateDate, 1);
        var DANECode = DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId(value.CountryId, value.StateId, value.CityId).done(function (data) {
            if (data.success) {
                address.DANECode = data.result;
            }
        });
        address.Description = value.Description;
        $("#selectAddress").UifSelect("setSelected", value.AddressTypeId);
        address.AddressTypeId = value.AddressTypeId;
        address.AddressType.Description = $("#selectAddress").UifSelect("getSelectedText");
        address.IsPrincipal = value.IsPrincipal
        address.CityId = value.CityId;
        address.CityDescription = value.CityDescription;
        address.StateId = value.StateId;
        address.StateDescription = value.StateDescription;
        address.CountryId = value.CountryId;
        address.CountryDescription = value.CountryDescription;
        address.AplicationStaus = 3;
        $("#selectAddress").UifSelect("setSelected", null);
        return address;
    }

    static SetAddressCompany(Id) {
        var address = { AddressType: {}, City: { State: { Country: {} } }, };
        address.Id = Id;
        //address.UpdateUser = addresses[0].UpdateUser,
        //    address.UpdateDate = FormatDate(addresses[0].UpdateDate, 1);
        address.Description = $("#inputCompanyAddress").val();
        address.AddressTypeId = $("#selectCompanyAddressType").UifSelect("getSelected");
        address.AddressType.Description = $("#selectCompanyAddressType").UifSelect("getSelectedText");
        address.IsPrincipal = $('#chkCompanyPrincipal').is(':checked');
        address.City.Id = $("#inputCompanyCity").data("Id");
        address.City.Description = $("#inputCompanyCity").data("Description");
        address.City.DANECode = $("#inputCompanyDaneCode").val();
        address.City.State.Id = $("#inputCompanyState").data("Id");
        address.City.State.Description = $("#inputCompanyState").data("Description");
        address.City.State.Country.Id = $("#inputCompanyCountry").data("Id");
        address.City.State.Country.Description = $("#inputCompanyCountry").data("Description");
        address.AplicationStaus = 3;
        return address;
    }

    static DuplicateAddress(address, rowId) {
        var duplicate = false;
        $.each($("#listAddresses").UifListView('getData'), function (i, item) {
            if ($.trim(item.AddressTypeId) == $.trim(address) && i != rowAddressId) {
                duplicate = true;
                return false;
            }
        });

        return duplicate;
    }

    static PrincipalAddress(rowId) {
        var duplicate = false;
        var IsPrincipal = $("#rdPrincipal").is(":checked");
        if (IsPrincipal) {
            $.each($("#listAddresses").UifListView('getData'), function (i, item) {
                if (item.IsPrincipal == IsPrincipal && i != rowAddressId) {
                    duplicate = true;
                    return false;
                }
            });
        }

        return duplicate;
    }

    static ConvertAddressDtoToModel(AddressesDTO) {
        var address = Address.CreateAddresModel();
        var rslt = [];
        if (AddressesDTO != null) {
            for (var index = 0; index < AddressesDTO.length; index++) {
                address.Id = AddressesDTO[index].AddressTypeId;
                address.Description = AddressesDTO[index].Description;
                address.AddressTypeId = AddressesDTO[index].AddressTypeId;
                address.AddressType.Description = AddressesDTO[index].Description;
                address.IsPrincipal = true;
                address.City.Id = AddressesDTO[index].CityId;
                address.City.Description = AddressesDTO[index].CountryDescription;
                address.City.DANECode = '';
                address.City.State.Id = AddressesDTO[index].StateId;
                address.City.State.Description = AddressesDTO[index].StateDescription;
                address.City.State.Country.Id = AddressesDTO[index].CountryId;
                address.City.State.Country.Description = AddressesDTO[index].CountryDescription;

                rslt.push(address);
            }
        }
        return rslt;
    }

    static ConvertAddressModelToDTO(AddressesModel) {
        var Address = {};
        AddressesDTO.AddressTypeId = Address.Id;
        AddressesDTO.Description = Address.Description;
        AddressesDTO.AddressTypeId = Address.AddressTypeId;
        AddressesDTO.Description = Address.AddressType.Description;
        AddressesDTO.CityId = Address.City.Id;
        AddressesDTO.StateId = Address.City.State.Id;
        AddressesDTO.CountryId = Address.City.State.Country.Id;
        return Address;
    }

    static LoadDaneCodePersonNatural(countryId, stateId, cityId) {
        DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId(countryId, stateId, cityId).done(function (data) {
            if (data.success) {
                $('#inputDaneCodePn').val("");
                $('#inputDaneCodePn').val(data.result);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        return true;
    }

    static LoadDaneCodePersonLegal(countryId, stateId, cityId) {
        DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId(countryId, stateId, cityId).done(function (data) {
            if (data.success) {
                $('#inputCompanyDaneCode').val("");
                $('#inputCompanyDaneCode').val(data.result);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        return true;
    }
}