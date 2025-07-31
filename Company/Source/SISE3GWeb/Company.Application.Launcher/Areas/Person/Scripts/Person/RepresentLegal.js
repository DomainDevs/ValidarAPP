var representLegalTmp = null;
var fieldExist = false;
var documentTypeDefault = 1;
var currencyDefault = 0;

class RepresentLegal extends Uif2.Page {
    getInitialState() {
        $("#InputExpeditiondateRepresentLegal").UifDatepicker('setValue', new Date());
        $("#InputBirthdateRepresentLegal").UifDatepicker('setValue', new Date());
        $("#inputDaneCodeRepresentLegal").UifAutoComplete({
            source: rootPath + "Person/Person/GetDaneCodeByQuery",
            displayKey: "DANECode",
            queryParameter: "&query"
        });

        DocumentTypeRequest.GetDocumentType("3").done(function (data) {
            if (data.success) {
                $("#selectDocumentTypeRepresentLegal").UifSelect({ sourceData: data.result });
            }
        });
        this.InitializeRepresentLegal();
    }

    //Seccion Eventos
    bindEvents() {
        $('#InputExpeditiondateRepresentLegal').on("datepicker.change", this.ExpeditiondateRepresentLegal);
        $('#InputBirthdateRepresentLegal').on("datepicker.change", this.BirthdateRepresentLegal);
        $("#InputvalueRepresentLegal").focusin(this.NotFormatMoneyIn);
        $("#InputvalueRepresentLegal").focusout(this.FormatMoneyout);
        $("#btnrecordRepresentLegal").click(this.RecordRepresentLegal);
        $("#btnCloseRepresentLegal").click(this.CloseRepresentLegal);
        $('#inputCountryRepresentLegal').on("search", this.SearchCountryReprersentLegal);
        $("#inputDaneCodeRepresentLegal").on('itemSelected', this.ChangeDaneCodeRepresentLegal);
        $('#inputStateRepresentLegal').on("search", this.SearchStateRepresentLegal);
        $('#inputCityRepresentLegal').on("search", this.SearchCityRepresentLegal);
        $('#tblResultListCountries tbody').on('click', 'tr', this.SelectSearchCountries);
        $('#tblResultListStates tbody').on('click', 'tr', this.SelectSearchStates);
        $('#tblResultListCities tbody').on('click', 'tr', this.SelectSearchCities);

    }

    InitializeRepresentLegal() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#InputNumDocumentRepresentLegal').ValidatorKey(ValidatorType.Number);
        $('#InputvalueRepresentLegal').ValidatorKey(ValidatorType.Number);
        $('#InputPhoneReprersentLegal').ValidatorKey(ValidatorType.Number);
        $('#InputCellReprersentLegal').ValidatorKey(ValidatorType.Number);
        $("#InputPlaceOfBirthRepresentLegal").ValidatorKey(ValidatorType.lettersandnumbersAccent);
        $("#InputFullNameRepresentLegal").ValidatorKey(ValidatorType.lettersandnumbersAccent);
        $("#InputNationalityRepresentLegal").ValidatorKey(ValidatorType.Letter);
        $('#inputCountryRepresentLegal').ValidatorKey(2);
        $('#inputStateRepresentLegal').ValidatorKey(2);
        $('#inputCityRepresentLegal').ValidatorKey(2);
        $("#inputDaneCodeRepresentLegal").ValidatorKey(1);


    }

    ExpeditiondateRepresentLegal(event, date) {
        var validate = true;
        var date = $('#InputExpeditiondateRepresentLegal').val().split("/");
        if (isExpirationDate(date)) {
            $('#InputExpeditiondateRepresentLegal').val("");
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInvalidDate + "<br>" })
        }
    }

    BirthdateRepresentLegal(event, date) {
        var date = $('#InputBirthdateRepresentLegal').val().split("/");
        if (date != undefined && date != "") {
            if (isExpirationDate(date)) {
                $('#InputBirthdateRepresentLegal').val("");
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInvalidDate + "<br>" })
            }
        }
    }

    NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    FormatMoneyout() {
        $(this).val(FormatMoney($(this).val()));
    }

    RecordRepresentLegal() {
        if (RepresentLegal.ValidateRepresentLegal()) {
            RepresentLegal.CreateModelRepresentLegal();
            LegalRepresentativeRequest.CreateLegalRepresent(representLegalTmp, individualId).done(function (data) {
                if (data.success) {
                    Persons.AddSubtitlesRightBar();
                    $.UifNotify('show', { 'type': 'success', 'message': AppResourcesPerson.SavedLegalRepresent, 'autoclose': true });
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    CloseRepresentLegal() {
        RepresentLegal.ClearRepresentLegal();
    }

    SearchCountryReprersentLegal(event, value) {
        $('#inputStateRepresentLegal').val("");
        $('#inputCityRepresentLegal').val("");
        $('#inputDaneCodePn').val("");
        if ($("#inputCountryRepresentLegal").val().trim().length > 0) {
            RepresentLegal.ReprersentLegalGetCountriesByDescription($("#inputCountryRepresentLegal").val());
        }
    }

    ChangeDaneCodeRepresentLegal() {
        RepresentLegal.ReprersentLegalGetCountryAndStateAndCityByDaneCode($('#inputCountryRepresentLegal').data("Id"), $("#inputDaneCodeRepresentLegal").val());
    }

    SearchStateRepresentLegal(event, value) {
        $('#inputCityRepresentLegal').val("");
        $('#inputDaneCodePn').val("");
        if ($("#inputStateRepresentLegal").val().trim().length > 0) {
            RepresentLegal.ReprersentLegalGetStatesByCountryIdByDescription($("#inputCountryRepresentLegal").data('Id'), $("#inputStateRepresentLegal").val());
        }
    }

    SearchCityRepresentLegal(event, value) {
        $('#inputDaneCodePn').val("");
        if ($("#inputCityRepresentLegal").val().trim().length > 0) {
            RepresentLegal.ReprersentLegalGetCitiesByCountryIdByStateIdByDescription($("#inputCountryRepresentLegal").data('Id'), $("#inputStateRepresentLegal").data('Id'), $("#inputCityRepresentLegal").val());
        }
    }

    SelectSearchCountries() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCountryRepresentLegal").data(dataCountry);
        $("#inputCountryRepresentLegal").val(dataCountry.Description);
        $('#modalListSearchCountries').UifModal('hide');
    }

    SelectSearchStates() {
        var dataState = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputStateRepresentLegal").data(dataState);
        $("#inputStateRepresentLegal").val(dataState.Description);
        $('#modalListSearchStates').UifModal('hide');
    }

    SelectSearchCities() {
        var dataCities = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputCityRepresentLegal").data(dataCities);
        $("#inputCityRepresentLegal").val(dataCities.Description);
        $('#modalListSearchCities').UifModal('hide');
        //RepresentLegal.getDaneByCityIdStateIdCountryId();
    }

    static LoadLegalRepresentative() {
        $.UifProgress('show');
        LegalRepresentativeRequest.GetLegalRepresentByIndividualId(individualId).done(function (data) {
            $.UifProgress('close');
            if (data.success) {
                representLegalTmp = data.result;
                RepresentLegal.EditRepresentativeLegal();
            }
        }).fail(function (data) {
            $.UifProgress('close');
        });
    }

    //Seccion Funciones
    static getDaneByCityIdStateIdCountryId() {
        if (representLegalTmp != null) {
            if (representLegalTmp.City.Id != null && representLegalTmp.City.Id > 0) {
                DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId(representLegalTmp.Country.Id, representLegalTmp.State.Id, representLegalTmp.City.Id).done(
                    function (data) {
                        if (data.success) {
                            $('#inputDaneCodeRepresentLegal').val("");
                            $('#inputDaneCodeRepresentLegal').val(data.result);
                            $("#inputCityRepresentLegal").data(representLegalTmp.City);
                            $("#inputStateRepresentLegal").data(representLegalTmp.State);
                            $("#inputCountryRepresentLegal").data(representLegalTmp.Country);
                        } else {
                            $("#inputDaneCodeRepresentLegal").val("");
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
            }
        }
        else {
            DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCountryRepresentLegal").data("Id"), $("#inputStateRepresentLegal").data("Id"), $("#inputCityRepresentLegal").data("Id")).done(function (data) {
                if (data.success) {
                    $('#inputDaneCodeRepresentLegal').val(data.result);
                    DaneCodeRequest.GetCountryAndStateAndCityByDaneCode($("#inputCountryRepresentLegal").data("Id"), data.result)
                        .then(function (result) {
                            if (result.success === true) {
                                result = result.result;
                                $("#inputCityRepresentLegal").val(result.Description);
                                $("#inputStateRepresentLegal").val(result.State.Description);
                                $("#inputCountryRepresentLegal").val(result.State.Country.Description);
                            } else {
                                $.UifNotify('show', { 'type': 'info', 'message': result.result, 'autoclose': true });
                            }

                        });
                } else {
                    $("#inputCompanyDaneCode").val("");
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }


    }

    static EditRepresentativeLegal() {
        if (representLegalTmp != null) {
            if (representLegalTmp.individualId > 0) {
                $("#InputExpeditiondateRepresentLegal").val(FormatDate(representLegalTmp.ExpeditionDate));
                $("#InputFullNameRepresentLegal").val(representLegalTmp.FullName);
                $("#InputBirthdateRepresentLegal").val(FormatDate(representLegalTmp.BirthDate));
                $("#InputNationalityRepresentLegal").val(representLegalTmp.Nationality);
                $("#InputPhoneReprersentLegal").val(representLegalTmp.Phone);
                $("#InputCellReprersentLegal").val(representLegalTmp.CellPhone);
                $("#InputEmailReprersentLegal").val(representLegalTmp.Email);
                $("#selectCurrencyRepresentLegal").val(representLegalTmp.CurrencyId);
                $("#notesReprersentLegal").val(representLegalTmp.Description);
                $("#InputAddressReprersentLegal").val(representLegalTmp.Address);
                $("#InputPlaceOfBirthRepresentLegal").val(representLegalTmp.BirthPlace);
                $("#InputOfficeReprersentLegal").val(representLegalTmp.JobTitle);
                $("#InputvalueRepresentLegal").val(representLegalTmp.Value);
                $("#InputSurnamesManager").val(representLegalTmp.ManagerName);
                $("#InputSurnamesGenManager").val(representLegalTmp.GeneralManagerName);
                $("#InputSurnameContactCom").val(representLegalTmp.ContactName);
                $("#InputNotes").val(representLegalTmp.ContacstAdditionalInfo);
                $("#selectDocumentTypeRepresentLegal").val(representLegalTmp.DocumentTypeId);
                $("#InputNumDocumentRepresentLegal").val(representLegalTmp.NumberDocument);

                $("#inputCityRepresentLegal").val(representLegalTmp.City.Description);
                $("#inputStateRepresentLegal").val(representLegalTmp.State.Description);
                $("#inputCountryRepresentLegal").val(representLegalTmp.Country.Description);

                if (representLegalTmp.City.Id != null &&
                    representLegalTmp.State.Id != null &&
                    representLegalTmp.Country.Id != null) {

                    RepresentLegal.getDaneByCityIdStateIdCountryId();
                }
            }
            else {
                $("#InputExpeditiondateRepresentLegal").UifDatepicker('setValue', new Date());
                $("#InputBirthdateRepresentLegal").UifDatepicker('setValue', new Date());
            }

        } else {
            Persons.GetdefaultValueCountry();
        }
    }

    static ValidateRepresentLegal() {
        var error = "";
        if ($("#InputBirthdateRepresentLegal").val() == "") {
            error = error + 'Fecha de nacimiento <br>';
        }
        if ($("#InputNationalityRepresentLegal").val() == "") {
            error = error + 'Nacionalidad <br>';
        }
        if ($("#selectDocumentTypeRepresentLegal").val() == "") {
            error = error + 'Tipo de documento <br>';
        }
        if ($("#InputNumDocumentRepresentLegal").val() == "") {
            error = error + 'Número de documento <br>';
        }
        if ($("#InputExpeditiondateRepresentLegal").val() == "") {
            error = error + 'Fecha de expedición <br>';
        }
        if ($("#InputPlaceOfBirthRepresentLegal").val() == "") {
            error = error + 'Lugar de nacimiento <br>';
        }
        if ($("#InputFullNameRepresentLegal").val() == "") {
            error = error + 'Nombre y apellido <br>';
        }
        if ($("#InputAddressReprersentLegal").val() == "") {
            error = error + 'Dirección <br>';
        }
        if ($("#InputOfficeReprersentLegal").val() == "") {
            error = error + 'Cargo <br>';
        }
        if ($("#inputCountryRepresentLegal").data("Id") == undefined || $("#inputCountryRepresentLegal").data("Id") == null || $("#inputCountryRepresentLegal").data("Id") == "") {
            error = error + 'País <br>';
        }
        if ($("#inputStateRepresentLegal").data("Id") == undefined || $("#inputStateRepresentLegal").data("Id") == null || $("#inputStateRepresentLegal").data("Id") == "") {
            error = error + 'Departamento <br>';
        }
        if ($("#inputCityRepresentLegal").data("Id") == undefined || $("#inputCityRepresentLegal").data("Id") == null || $("#inputCityRepresentLegal").data("Id") == "") {
            error = error + 'Ciudad <br>';
        }
        if ($('#InputEmailReprersentLegal').val() == "") {
            error = error + 'Email <br>';
        } else {
            if (!ValidateEmail($('#InputEmailReprersentLegal').val())) {
                error = error + 'Debe ingresar un e-mail valido<br>';
            }
        }
        if ($("#InputvalueRepresentLegal").val() == "") {
            error = error + 'Valor <br>';
        }
        if ($("#selectCurrencyRepresentLegal").UifSelect("getSelected") == "") {
            error = error + 'Moneda <br>';
        }

        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorRequiredFields + ":<br>" + error })
            return false;
        }
        return true;
    }

    static ClearRepresentLegal() {
        $("#InputExpeditiondateRepresentLegal").val("");
        $("#InputFullNameRepresentLegal").val("");
        $("#InputBirthdateRepresentLegal").val("");
        $("#InputNationalityRepresentLegal").val("COLOMBIANO");
        $("#InputPhoneReprersentLegal").val("");
        $("#InputCellReprersentLegal").val("");
        $("#InputEmailReprersentLegal").val("");
        $("#selectCurrencyRepresentLegal").UifSelect("setSelected", currencyDefault);
        $("#notesReprersentLegal").val("");
        $("#InputAddressReprersentLegal").val("");
        $("#inputDaneCodeRepresentLegal").val("");
        $("#InputPlaceOfBirthRepresentLegal").val("");
        $("#InputOfficeReprersentLegal").val("");
        $("#InputNumDocumentRepresentLegal").val("");
        $("#InputvalueRepresentLegal").val("");
        $("#selectDocumentTypeRepresentLegal").UifSelect("setSelected", documentTypeDefault);
        // << EESGE-172 Control De Busqueda
        $("#inputCountryRepresentLegal").val("");
        $("#inputCountryRepresentLegal").data({ Id: null, Description: null });
        $("#inputStateRepresentLegal").val("");
        $("#inputStateRepresentLegal").data({ Id: null, Description: null });
        $("#inputCityRepresentLegal").val("");
        $("#inputCityRepresentLegal").data({ Id: null, Description: null });
        $("#InputSurnamesManager").val("");
        $("#InputSurnamesGenManager").val("");
        $("#InputSurnameContactCom").val("");
        $("#InputNotes").val("");

        // EESGE-172 Control De Busqueda >>
        representLegalTmp = null;
        if (defaultValues != null) {
            Persons.SetDefaultValues(defaultValues);
        }
        else {
            Persons.GetDefaultValues();
        }
        // Se inicializa y setea el país por defecto   
        Persons.GetdefaultValueCountry();
    }

    static CreateModelRepresentLegal() {
        if ($("#InputFullNameRepresentLegal").val() == "" && $("#InputNumDocumentRepresentLegal").val() == "" && $("#InputBirthdateRepresentLegal").val() == "") {
            representLegalTmp = null;
        } else {
            representLegalTmp = {

                individualId: individualId,
                Address: $("#InputAddressReprersentLegal").val(),
                Value: NotFormatMoney($("#InputvalueRepresentLegal").val()),
                CurrencyId: $("#selectCurrencyRepresentLegal").val(),
                BirthDate: $("#InputBirthdateRepresentLegal").val(),
                BirthPlace: $("#InputPlaceOfBirthRepresentLegal").val(),
                CellPhone: $("#InputCellReprersentLegal").val(),
                City: {
                    Id: $('#inputCityRepresentLegal').data("Id"),
                    Description: $('#inputCityRepresentLegal').val()
                },
                State: {
                    Id: $('#inputStateRepresentLegal').data("Id"),
                    Description: $('#inputStateRepresentLegal').val()
                },
                Country: {
                    Id: $('#inputCountryRepresentLegal').data("Id"),
                    Description: $('#inputCountryRepresentLegal').val()
                },
                ContactAdditionalInfo: $("#InputNotes").val(),
                ContactName: $("#InputSurnameContactCom").val(),
                Description: $("#notesReprersentLegal").val(),
                Email: $("#InputEmailReprersentLegal").val(),
                ExpeditionPlace: $("#InputPlaceOfBirthRepresentLegal").val(),
                FullName: $("#InputFullNameRepresentLegal").val(),
                GeneralManagerName: $("#InputSurnamesGenManager").val(),
                DocumentTypeId: $("#selectDocumentTypeRepresentLegal").val(),
                ExpeditionDate: $("#InputExpeditiondateRepresentLegal").val(),
                NumberDocument: $("#InputNumDocumentRepresentLegal").val(),
                JobTitle: $("#InputOfficeReprersentLegal").val(),
                ManagerName: $("#InputSurnamesManager").val(),
                Nationality: $("#InputNationalityRepresentLegal").val(),
                Phone: $("#InputPhoneReprersentLegal").val()

            };
        }
    }

    static ReprersentLegalGetCountriesByDescription(description) {
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
                        $('#tblResultListCountries').UifDataTable('clear');
                        $("#tblResultListCountries").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchCountries').UifModal('showLocal', AppResourcesPerson.ModalTitleCountries);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // State
    static ReprersentLegalGetStatesByCountryIdByDescription(countryId, description) {
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
                            $('#tblResultListStates').UifDataTable('clear');
                            $("#tblResultListStates").UifDataTable('addRow', dataStates);
                            $('#modalListSearchStates').UifModal('showLocal', AppResourcesPerson.ModalTitleStates);
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
    static ReprersentLegalGetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
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

                            $('#tblResultListCities').UifDataTable('clear');
                            $("#tblResultListCities").UifDataTable('addRow', dataCities);
                            $('#modalListSearchCities').UifModal('showLocal', AppResourcesPerson.ModalTitleCities);
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

    // DANECode
    static ReprersentLegalGetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        DaneCodeRequest.GetCountryAndStateAndCityByDaneCode(countryId, daneCode).done(function (data) {
            if (data.success) {
                if (data.result !== null && Object.keys(data.result).length > 0) {
                    $("#inputCountryRepresentLegal").data({ Id: data.result.State.Country.Id, Description: data.result.State.Country.Description });
                    $("#inputStateRepresentLegal").data({ Id: data.result.State.Id, Description: data.result.State.Description });
                    $("#inputCityRepresentLegal").data({ Id: data.result.Id, Description: data.result.Description });
                    $("#inputCountryRepresentLegal").val($("#inputCountryRepresentLegal").data("Description"));
                    $("#inputStateRepresentLegal").val($("#inputStateRepresentLegal").data("Description"));
                    $("#inputCityRepresentLegal").val($("#inputCityRepresentLegal").data("Description"));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}