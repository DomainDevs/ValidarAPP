var prospectCode = 0;
var isNew = 0;
var individualId = -1;
var searchType = null;
class ProspectusNatural extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        lockScreen();
        $("#inputDaneCodePl").UifAutoComplete({
            source: rootPath + "Person/Person/GetDaneCodeByQuery",
            displayKey: "DANECode",
            queryParameter: "&query"
        });

        DocumentTypeRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#selectDocumentTypePrn").UifSelect({ sourceData: data.result });
            }
        });

        this.InitializeProspectusNatural();
        new Persons();
        if (glbPersonIndividualId != null) {
            Persons.GetProspectByDocumentNumSearchType(glbPersonIndividualId, searchType);
            glbPersonIndividualId = null;
        }

        if (glbPersonOnline != null) {
            glbPersonOnline.RolType = 3;
            $("#InputDocumentNumber").val(glbPersonOnline.ViewModel.HolderName)
        }
        
        if ($("#InputDocumentNumber").val() != "" && $('#selectDocumentTypePrn').val() != "") {
            ProspectusNatural.SearchDocumentNumberPrn();
        }

       
    }

    //Seccion Eventos
    bindEvents() {
        //Buscar personas por numero de documento
        $("#InputDocumentNumber").focusout(this.SearchDocumentNumberPrn);
        $('#inputCountryPl1').on("search", this.SearchCountryPl1);
        $("#inputDaneCodePl").on('itemSelected', this.ChangeDaneCodePl);
        $('#inputStatePl').on("search", this.SearchStatePl);
        $('#inputCityPl').on("search", this.SearchCityPl);
        $('#tblResultListCountries tbody').on('click', 'tr', this.ResultListCountries);
        $('#tblResultListStates tbody').on('click', 'tr', this.ResultListStates);
        $('#tblResultListCities tbody').on('click', 'tr', this.ResultListCities);
        $("#InputbirthdatePrn").on("datepicker.change", ProspectusNatural.BirthdatePrn());
        $("#InputbirthdatePrn").on("blur", function () {
            ProspectusNatural.BirthdatePrn();
        });
        // EESGE-172 Control De Busqueda >>
        $('#selectDocumentTypePrn').on('itemSelected', function (event, selectedItem) {
            ProspectusNatural.SearchDocumentNumberPrn();
            ProspectusNatural.ChangeTypeAndNumberDocument();
        });
    }

    InitializeProspectusNatural() {
        prospectCode = 0;
        searchType = TypePerson.ProspectNatural;
        isNew = 1;
        individualId = Person.New;
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#InputAgePrn").attr('disabled', 'disabled');
        $("#inputEmailPrn").ValidatorKey(ValidatorType.Emails, 1, 1);
        $('#InputDocumentNumber').ValidatorKey(ValidatorType.Number, 2, 0);
        $('#inputPhoneProspectNatural').ValidatorKey(ValidatorType.Number);
        $('#inputAddressAllPrn').ValidatorKey(ValidatorType.Addresses, 1, 1);
        ProspectusNatural.NewProspectNatural();
    }

    static SearchDocumentNumberPrn() {
        if ($("#InputDocumentNumber").val() != "") {
            PersonRequest.GetPersonByDocumentByDocumentType($('#InputDocumentNumber').val(), parseInt($('#selectDocumentTypePrn').val())).done(function (dataperson) {
                if (dataperson.success) {
                    if (dataperson.result) {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.DocumentNumberAlreadyRegistered });
                        $("#InputDocumentNumber").val("");
                    }
                }
            });
            ProspectusNatural.GetProspectByDocumentNum($("#InputDocumentNumber").val(), 3).done(function (data) {
                if (data.success) {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.DocumentNumberAlreadyRegistered });
                    $("#InputDocumentNumber").val("");
                }
            });
        }
    }

    static GetProspectByDocumentNum(documentNum, searchType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetProspectByDocumentNum',
            data: JSON.stringify({ documentNum: documentNum, searchType: searchType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    SearchCountryPl1(event, value) {
        $('#inputStatePl').val("");
        $('#inputCityPl').val("");
        $('#inputDaneCodePl').val("");
        if ($("#inputCountryPl1").val().trim().length > 0) {
            ProspectusNatural.prospectPnGetCountriesByDescription($("#inputCountryPl1").val());
        }
    }

    ChangeDaneCodePl() {
        ProspectusNatural.prospectPnGetCountryAndStateAndCityByDaneCode($('#inputCountryPl1').data("Id"), $("#inputDaneCodePl").val());
    }

    SearchStatePl(event, value) {
        $('#inputCityPl').val("");
        $('#inputDaneCodePl').val("");
        if ($("#inputStatePl").val().trim().length > 0) {
            ProspectusNatural.prospectPnGetStatesByCountryIdByDescription($("#inputCountryPl1").data('Id'), $("#inputStatePl").val());
        }
    }

    SearchCityPl(event, value) {
        $('#inputDaneCodePl').val("");
        if ($("#inputCityPl").val().trim().length > 0) {
            ProspectusNatural.prospectPnGetCitiesByCountryIdByStateIdByDescription($("#inputCountryPl1").data('Id'), $("#inputStatePl").data('Id'), $("#inputCityPl").val());
        }
    }

    ResultListCountries() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCountryPl1").data(dataCountry);
        //$("#inputCountryPl1").val(dataCountry.Description);
        $('#modalListSearchCountries').UifModal('hide');
    }

    ResultListStates() {
        var dataState = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputStatePl").data(dataState);
        $("#inputStatePl").val(dataState.Description);
        $('#modalListSearchStates').UifModal('hide');
    }

    ResultListCities() {
        var dataCities = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputCityPl").data(dataCities);
        $("#inputCityPl").val(dataCities.Description);
        $('#modalListSearchCities').UifModal('hide');
        ProspectusNatural.LoadDaneCodeProspectNatura();
    }

    static BirthdatePrn() {
        var date = $('#InputbirthdatePrn').val().split(DateSplit);
        if (!(isExpirationDate(date))) {
            if ($("#InputbirthdatePrn").val() != '') {
                var fecha = $("#InputbirthdatePrn").val();
                var age = Shared.calculateAge("#InputAgePrn", fecha);
                if (age > 117) {
                    $("#InputbirthdatePrn").UifDatepicker("clear");
                    $("#InputAgePrn").val("");
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.WarningAgeMax })
                }
            }
        }
        else {
            $("#InputbirthdatePrn").UifDatepicker("clear");
            $("#InputAgePrn").val("");
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.NoCanDateBirthGreaterCurrent + "<br>" })
        }
    }

    //Seccion Funciones
    static SetProspectNatural(prospect) {

        prospectCode = prospect.ProspectCode;
        individualId = prospect.ProspectCode;

        if (document.getElementById("selectSearchPersonType") != null) {
            $("#selectSearchPersonType").UifSelect("setSelected", TypePerson.ProspectNatural);
        }

        $("#inputDocument").val(prospect.Card.Description);
        $("#InputDocumentNumber").val(prospect.Card.Description);
        $("#selectDocumentTypePrn").val(prospect.Card.Id);
        $("#lblPersonCode").val(prospectCode);
        $("#InputSurnamePrn").val(prospect.SurName);
        $("#InputLastNamePrn").val(prospect.MotherLastName);
        $("#InputNamePrn").val(prospect.Name);
        $("#InputLastNamePrn").val(prospect.MotherLastName);
        $('#inputPhoneProspectNatural').val(prospect.PhoneNumber);
        $('#inputEmailPrn').val(prospect.EmailAddres);

        if (document.getElementById("selectMaritalStatusPrn") != null) 
        $("#selectMaritalStatusPrn").UifSelect("setSelected", prospect.MartialStatus);
        $('#inputGeneralInfoPrn').val(prospect.AdditionaInformation);

        if (prospect.Gender == 'M') {
            if (document.getElementById("selectGenderProspect") != null) 
           $("#selectGenderProspect").UifSelect("setSelected", 1);

       }
        else if (prospect.Gender == 'F') {
            if (document.getElementById("selectGenderProspect") != null) 
           $("#selectGenderProspect").UifSelect("setSelected", 2);

       }
       
        $("#InputbirthdatePrn").val(FormatDate(prospect.BirthDate, 1));
        Shared.calculateAge("#InputAgePrn", prospect.BirthDate);
       // $("#selectAddressPl1").UifSelect("setSelected", prospect.Address.Id);
        $("#inputAddressAllPrn").val(prospect.Address.Description);

        if (prospect.City != null) {
            if (prospect.DANECode != null) {
                $("#inputDaneCodePl").val(prospect.DANECode);
                // << EESGE-172 Control De Busqueda
                ProspectusNatural.prospectPnGetCountryAndStateAndCityByDaneCode(prospect.Country.Id, prospect.DANECode);
                // EESGE-172 Control De Busqueda >>
            }
        }
    }

    static RecordProspectNatural() {

        var prospectData = {
            ProspectCode: null,
            AdditionaInformation: null,
            BirthDate: null,
            Address: { Id: null, Description: null },
            City: { Id: null, Description: null },
            State: { Id: null, Description: null },
            Country: { Id: null, Description: null },
            EmailAddres: null,
            Gender: null,
            Card: { Id: null, Description: null },
            MartialStatus: null,
            MotherLastName: null,
            Name: null,
            SurName: null,
            PhoneNumber: null,
            IndividualTypePerson: null
        };

        var gender = "";

        if ($('#selectGenderProspect').UifSelect("getSelected") == 1) {
            gender = 'M'
        }
        else {
            gender = 'F'
        }

        prospectData.ProspectCode = $("#lblPersonCode").val();
        prospectData.AdditionaInformation = $('#inputGeneralInfoPrn').val();
        prospectData.BirthDate = $('#InputbirthdatePrn').UifSelect("getSelected");
        prospectData.Address.Id = $("#selectAddressPl1").val() == "" ? null : $("#selectAddressPl1").val();
        prospectData.Address.Description = $("#inputAddressAllPrn").val();
        prospectData.City.Id = $("#inputCityPl").data("Id");
        prospectData.State.Id = $("#inputStatePl").data("Id");
        prospectData.Country.Id = $("#inputCountryPl1").data("Id");
        prospectData.EmailAddres = $("#inputEmailPrn").val();
        prospectData.Gender = gender;
        prospectData.Card.Id = $("#selectDocumentTypePrn").val() == "" ? null : $("#selectDocumentTypePrn").val();
        prospectData.Card.Description = $("#InputDocumentNumber").val();
        prospectData.MartialStatus = $("#selectMaritalStatusPrn").val();
        prospectData.MotherLastName = $("#InputLastNamePrn").val();
        prospectData.Name = $("#InputNamePrn").val();
        prospectData.SurName = $("#InputSurnamePrn").val();
        prospectData.PhoneNumber = $("#inputPhoneProspectNatural").val();
        prospectData.IndividualTypePerson = IndividualTypePerson.Natural;
        prospectData.ProspectCode = IndividualTypePerson.Natural;
        prospectData.DANECode = $("#inputDaneCodePl").val();
        Prospect.CreateProspectPersonNatural(prospectData).done(function (data) {
            $('#btnSavePerson').attr("disabled", false);
            if (data.success) {
                isNew = 0;
                if (individualId == Person.New) {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.SuccessfulCreate }, function (result) {
                        if (gblProspectData == null) {
                            gblProspectData = {};
                        }
                        gblProspectData.IdCardNo = prospectData.Card.Description;
                        /*persona en linea*/
                        if (typeof glbPolicy != "undefined" && glbPolicy != null) {

                            if (glbPersonOnline != undefined && glbPersonOnline.RolType > 0) {
                                glbPersonOnline.IndividualId = data.result.ProspectCode;
                                glbPersonOnline.CustomerType = CustomerType.Prospect;

                            }
                            if (glbPolicy.TemporalType == TemporalType.Quotation) {
                                glbPolicy.Holder = undefined;
                                router.run("prtQuotation");
                            }
                            else {
                                router.run("prtTemporal");
                            }
                        }

                    });
                }
                else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.PersonCodeNo + ' ' + data.result.ProspectCode + " " + AppResourcesPerson.SuccessfulUpdate });
                }
                ProspectusNatural.NewProspectNatural();
                ProspectusNatural.SetProspectNatural(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static NewProspectNatural() {
        searchType = TypePerson.ProspectNatural;
        individualId = Person.New;
        prospectCode = 0;
        $("#inputAddressAllPrn").val("");
        $("#InputSurnamePrn").val("");
        $("#InputNamePrn").val("");
        $("#selectMaritalStatusPrn").UifSelect("setSelected", null);
        $("#selectGenderProspect").UifSelect("setSelected", null);
        $("#selectDocumentTypePrn").UifSelect("setSelected", null);
        $("#selectAddressPl1").UifSelect("setSelected", null);
        $("#InputbirthdatePrn").val("");
        $("#InputDocumentNumber").val("");
        $("#inputEmailPrn").val("");
        $("#inputPhoneProspectNatural").val("");
        $("#lblPersonCode").val("0");
        $("#InputAgePrn").val("");
        $("#InputLastNamePrn").val("");
        $("#inputDaneCodePl").val("");
        // << EESGE-172 Control De Busqueda    
        $("#inputStatePl").val("");
        $("#inputStatePl").data({ Id: null, Description: null });
        $("#inputCityPl").val("");
        $("#inputCityPl").data({ Id: null, Description: null });
        if (defaultValues != null) {
            Persons.SetDefaultValues(defaultValues);
        }
        else {
            Persons.GetDefaultValues();
        }
        $("#selectSearchPersonType").UifSelect("setSelected", searchType);
    }

    static validateProspectNatural() {
        var msjProspectNatural = "";

        if ($("#selectDocumentTypePrn").UifSelect("getSelected") == "") {
            msjProspectNatural = msjProspectNatural + AppResourcesPerson.LabelTypeDocument + '<br>';
        }
        if ($("#InputDocumentNumber").val() == "") {
            msjProspectNatural = msjProspectNatural + AppResourcesPerson.LabelNumberDocument + "<br>";
        }
        if ($("#InputSurnamePrn").val() == "") {
            msjProspectNatural = msjProspectNatural + AppResourcesPerson.Surname + " <br>";
        }
        if ($("#InputNamePrn").val() == "") {
            msjProspectNatural = msjProspectNatural + AppResourcesPerson.Names + " <br>";
        }
        if ($("#selectGenderProspect").UifSelect("getSelected") == "") {
            msjProspectNatural = msjProspectNatural + AppResourcesPerson.LabelGender + " <br>";
        }
        if ($("#InputbirthdatePrn").val() == "") {
            msjProspectNatural = msjProspectNatural + AppResourcesPerson.Birthdate + " <br>";
        }
        if (msjProspectNatural != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msjProspectNatural })
            return false;

        }
        return true;
    }

    // << EESGE-172 Control De Busqueda
    static LoadDaneCodeProspectNatura() {
        DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCountryPl1").data("Id"), $("#inputStatePl").data("Id"), $("#inputCityPl").data("Id")).done(function (data) {
            if (data.success) {
                $('#inputDaneCodePl').val("");
                $('#inputDaneCodePl').val(data.result);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static prospectPnGetCountriesByDescription(description) {
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

        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // State
    static prospectPnGetStatesByCountryIdByDescription(countryId, description) {
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
    static prospectPnGetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
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
                        } else {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageNotFoundCities, 'autoclose': true });
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
    static prospectPnGetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        DaneCodeRequest.GetCountryAndStateAndCityByDaneCode(countryId, daneCode).done(function (data) {
            if (data.success) {
                if (data.result !== null && Object.keys(data.result).length > 0) {
                    $("#inputCountryPl1").data({ Id: data.result.State.Country.Id, Description: data.result.State.Country.Description });
                    $("#inputStatePl").data({ Id: data.result.State.Id, Description: data.result.State.Description });
                    $("#inputCityPl").data({ Id: data.result.Id, Description: data.result.Description });
                    $("#inputCountryPl1").val($("#inputCountryPl1").data("Description"));
                    $("#inputStatePl").val($("#inputStatePl").data("Description"));
                    $("#inputCityPl").val($("#inputCityPl").data("Description"));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ChangeTypeAndNumberDocument() {
        if (glbPersonOnline == null) {
            if ($("#selectDocumentTypePrn").UifSelect("getSelectedSource").IsAlphanumeric) {
                $('#InputDocumentNumber').val("");
                $('#InputDocumentNumber').off('keypress').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
            } else {
                $('#InputDocumentNumber').val("");
                $("#InputDocumentNumber").off('keypress').OnlyDecimals(0);
            }
        } else {
            if ($("#selectDocumentTypePrn").UifSelect("getSelectedSource").IsAlphanumeric) {
                $('#InputDocumentNumber').off('keypress').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
            } else {
                $("#InputDocumentNumber").off('keypress').OnlyDecimals(0);
            }
        }
    }

    static ConvertCompanyDtoToModel(company) {
        var rstl = [];
        if (company.result.length > 0) {
            $.each(company.result, function (index, item) {
                let resultData = {
                    IdentificationDocument: {
                        DocumentType: {}
                    },
                    CountryOrigin: {},
                    Addresses: {
                        City: {
                            State: {
                                country: {}
                            }
                        }
                    },
                    Phones: {},
                    Emails: {}
                };
                resultData.Name = company.result[index].TradeName;
                resultData.IdentificationDocument.Number = company.result[index].TributaryIdNumber;
                resultData.IndividualId = company.result[index].ProspectCode;
                resultData.IdentificationDocument.DocumentType.Id = company.result[index].TributaryIdTypeCode;
                resultData.CountryOrigin.Id = company.result[index].Country;
                //resultData.CountryOrigin.Description = company.result[index].Addresses[0].CountryDescription;


                resultData.Addresses = company.result[index].Address;
                resultData.Phones = company.result[index].PhoneNumber;
                resultData.Emails = company.result[index].EmailAddres;


              

                rstl.push(resultData);
            });
        }
        return rstl;
    }
}