var prospectCode = 0;
var isNew = 0;
var individualId = -1;
var searchType = null;
class ProspectusLegal extends Uif2.Page {

    getInitialState() {
        ProspectRequest.GetDocumentType("2").done(function (data) {
            if (data.success) {
                $("#selectDocumentTypeprl").UifSelect({ sourceData: data.result });
            }
        });

        $("#inputDaneCodePll").UifAutoComplete({
            source: rootPath + "Prospect/Prospect/GetDaneCodeByQuery",
            displayKey: "DANECode",
            queryParameter: "&query"
        });

        this.InitializeProspectusLegal();
        
        if (glbPersonIndividualId != null) {
            Prospect.GetProspectLegalByIndividualId(glbPersonIndividualId);
            glbPersonIndividualId = null;
        }
        ProspectRequest.GetAddressesTypes().done(function (data) {
            if (data.success) {
                $("#selectTypeAddressPn").UifSelect({ sourceData: data.result });
                $("#selectCompanyAddressType").UifSelect({ sourceData: data.result });
                $("#selectAddressPll").UifSelect({ sourceData: data.result });
                $("#selectAddressPl1").UifSelect({ sourceData: data.result });
                //$("#selectAddress").UifSelect({ sourceData: data.result });
            }
        });
        ProspectRequest.GetGenderTypes().done(function (data) {
            if (data.success) {
                $("#selectGender").UifSelect({ sourceData: data.result });
                $("#selectGenderProspect").UifSelect({ sourceData: data.result });
            }
        });

        ProspectRequest.GetMaritalStatus().done(function (data) {
            if (data.success) {
                $("#selectMaritalStatusPn").UifSelect({ sourceData: data.result });
                $("#selectMaritalStatusPrn").UifSelect({ sourceData: data.result });
            }
        });

    }

    //Seccion Eventos
    bindEvents() {
        //Buscar personas por numero de documento
        //$("#InputDocumentNumber").focusout(this.SearchDocumentNumberprl);
        $('#inputCountryPll1').on("search", this.SearchCountryPll1);
        $("#btnSavePerson").click(this.RecordProspectLegal);
        $('#btnCancelView').on('click', this.Return);
        $("#inputDaneCodePll").on('itemSelected', this.ChangeDaneCodePll);
        $('#inputStatePll').on("search", this.SearchStatePll);
        $('#inputCityPll').on("search", this.SearchCityPll);
        $('#tblResultListCountries tbody').on('click', 'tr', this.ResultListCountries);
        $('#tblResultListStates tbody').on('click', 'tr', this.ResultListStates);
        $('#tblResultListCities tbody').on('click', 'tr', this.ResultListCities);
    }

    InitializeProspectusLegal() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputEmailPrl").ValidatorKey(ValidatorType.Emails, 1, 1);
        $('#InputDocumentNumber').ValidatorKey(ValidatorType.Number);
        $('#inputAddressAllPrl').ValidatorKey(ValidatorType.Addresses, 1, 1);
        $('#inputPhoneProspectusLegal').ValidatorKey(ValidatorType.Number);
        ProspectusLegal.NewProspectLegal();
    }

    //SearchDocumentNumberprl() {
    //    if ($("#InputDocumentNumber").val() != "") {
    //        $("#InputDigit").val(Shared.CalculateDigitVerify($("#InputDocumentNumber").val()));
    //        prospect.CreateProspectPersonLegal($("#InputDocumentNumber").val()).done(function (data) {
    //            if (data.success) {
    //                $.UifDialog('alert', { 'message': AppResources.DocumentNumberAlreadyRegistered });
    //                $("#InputDocumentNumber").val("");
    //                $("#InputDigit").val("0");
    //            }
    //        });
    //    }
    //}

    SearchCountryPll1(event, value) {
        $('#inputStatePll').val("");
        $('#inputCityPll').val("");
        $('#inputDaneCodePll').val("");
        if ($("#inputCountryPll1").val().trim().length > 0) {
            ProspectusLegal.prospectPlGetCountriesByDescription($("#inputCountryPll1").val());
        }
    }
    Return() {
        router.run("prtQuotation")
    }
    ChangeDaneCodePll() {
        ProspectusLegal.prospectPlGetCountryAndStateAndCityByDaneCode($('#inputCountryPll1').data("Id"), $("#inputDaneCodePll").val());
    }

    SearchStatePll(event, value) {
        $('#inputCityPll').val("");
        $('#inputDaneCodePll').val("");
        if ($("#inputStatePll").val().trim().length > 0) {
            ProspectusLegal.prospectPlGetStatesByCountryIdByDescription($("#inputCountryPll1").data('Id'), $("#inputStatePll").val());
        }
    }

    SearchCityPll(event, value) {
        $('#inputDaneCodePll').val("");
        if ($("#inputCityPll").val().trim().length > 0) {
            ProspectusLegal.prospectPlGetCitiesByCountryIdByStateIdByDescription($("#inputCountryPll1").data('Id'), $("#inputStatePll").data('Id'), $("#inputCityPll").val());
        }
    }

    ResultListCountries() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCountryPll1").data(dataCountry);
        $("#inputCountryPll1").val(dataCountry.Description);
        $('#modalListSearchCountries').UifModal('hide');
    }

    ResultListStates() {
        var dataState = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputStatePll").data(dataState);
        $("#inputStatePll").val(dataState.Description);
        $('#modalListSearchStates').UifModal('hide');
    }

    ResultListCities() {
        var dataCities = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputCityPll").data(dataCities);
        $("#inputCityPll").val(dataCities.Description);
        $('#modalListSearchCities').UifModal('hide');
        ProspectusLegal.LoadDaneCodeProspectLegal();
    }

    //Seccion Funciones
    static SetProspectLegal(prospectLegal) {

        //prospectCode = prospect.ProspectCode;
        //individualId = prospect.ProspectCode;

        if (document.getElementById("selectSearchPersonType") != null) {
            $("#selectSearchPersonType").UifSelect("setSelected", searchType);
        }

        $("#inputDocument").val(prospectLegal.TributaryIdNumber);
        $('#selectDocumentTypeprl').val(prospectLegal.TributaryIdTypeCode);
        $('#InputDocumentNumber').val(prospectLegal.TributaryIdNumber);
        $("#InputDigit").val(Shared.CalculateDigitVerify($("#InputDocumentNumber").val()));
        $("#lblPersonCode").val(prospectLegal.ProspectCode);
        $('#InputFirstName').val(prospectLegal.Name);
        $("#selectAddressPll").val(prospectLegal.Address.Id);
        $("#inputAddressAllPrl").val(prospectLegal.Address.Description);
        $("#inputPhoneProspectusLegal").val(prospectLegal.PhoneNumber);
        $("#inputEmailPrl").val(prospectLegal.EmailAddres);
        $("#inputGeneralInfoPrl").val(prospectLegal.AdditionaInformation);

        // << EESGE-172 Control De Busqueda
        if (prospectLegal.City != null) {
            if (prospectLegal.DANECode != null) {
                $("#inputDaneCodePll").val(prospectLegal.DANECode);
                // << EESGE-172 Control De Busqueda
                ProspectusLegal.prospectPlGetCountryAndStateAndCityByDaneCode(prospectLegal.Country.Id, prospectLegal.DANECode);
                // EESGE-172 Control De Busqueda >>
            }
        }
        // EESGE-172 Control De Busqueda >>
    }

    static validateProspectLegal() {
        var msjProspectlegal = "";
        if ($("#selectDocumentTypeprl").UifSelect("getSelected") <= 0) {
            msjProspectlegal = msjProspectlegal + AppResources.LabelTypeDocument + "<br>";
        }

        if ($("#InputDocumentNumber").val() == "") {
            msjProspectlegal = msjProspectlegal + AppResources.LabelDocumentNumber + "<br>";;
        }
        if ($("#InputFirstName").val() == "") {
            msjProspectlegal = msjProspectlegal + AppResources.LabelBusinessName;
        }

        if (msjProspectlegal != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelInformative + " <br>" + msjProspectlegal })
            return false;

        } else if ($("#inputEmailPrl").val() != "") {
            if (!ValidateEmail($("#inputEmailPrl").val())) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorEmail })
                return false;

            }
        }
        return true;
    }

    static NewProspectLegal() {
        searchType = TypePerson.ProspectLegal;
        individualId = Person.New;
        prospectCode = 0;
        $('#InputDocumentNumber').val("");
        $("#lblPersonCode").val("0");
        $('#InputFirstName').val("");
        $("#selectAddressPll").UifSelect("setSelected", null);
        $("#inputAddressAllPrl").val("");
        // << EESGE-172 Control De Busqueda    
        $("#inputStatePll").val("");
        $("#inputStatePll").data({ Id: null, Description: null });
        $("#inputCityPll").val("");
        $("#inputCityPll").data({ Id: null, Description: null });
        // EESGE-172 Control De Busqueda >>
        $("#inputDaneCodePll").val("");
        $("#inputPhoneProspectusLegal").val("");
        $("#inputEmailPrl").val("");
        $("#lblPersonCode").val("0");
        //if (defaultValues != null) {
        //    Persons.SetDefaultValues(defaultValues);
        //}
        //else {
        //    Persons.GetDefaultValues();
        //}
        //$("#selectSearchPersonType").UifSelect("setSelected", searchType);
    }

    RecordProspectLegal() {

        var prospectData = {
            ProspectCode: null,
            AdditionaInformation: null,
            Address: { Id: null, Description: null },
            City: { Id: null, Description: null },
            State: { Id: null, Description: null },
            Country: { Id: null, Description: null },
            EmailAddres: null,
            Name: null,
            PhoneNumber: null,
            IndividualTypePerson: null,
            TributaryIdTypeCode: null,
            TributaryIdNumber: null
        };

        prospectData.ProspectCode = $("#lblPersonCode").val();
        prospectData.AdditionaInformation = $('#inputGeneralInfoPrl').val();
        prospectData.Address.Id = $("#selectAddressPll").val() == "" ? null : $("#selectAddressPll").val();
        prospectData.Address.Description = $("#inputAddressAllPrl").val();
        prospectData.City.Id = $("#inputCityPll").data("Id");
        prospectData.State.Id = $("#inputStatePll").data("Id");
        prospectData.Country.Id = $("#inputCountryPll1").data("Id");
        prospectData.EmailAddres = $("#inputEmailPrl").val();
        prospectData.Name = $('#InputFirstName').val()
        prospectData.PhoneNumber = $("#inputPhoneProspectusLegal").val();
        prospectData.IndividualTypePerson = IndividualTypePerson.Natural;
        prospectData.TributaryIdTypeCode = $('#selectDocumentTypeprl').val();
        prospectData.TributaryIdNumber = $('#InputDocumentNumber').val();

        Prospect.CreateProspectPersonLegal(prospectData).done(function (data) {
            $('#btnSavePerson').attr("disabled", false);
            if (data.success) {
                
                ProspectusLegal.NewProspectLegal();
                ProspectusLegal.SetProspectLegal(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    // << EESGE-172 Control De Busqueda
    static LoadDaneCodeProspectLegal() {
        ProspectRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCountryPll1").data("Id"), $("#inputStatePll").data("Id"), $("#inputCityPll").data("Id")).done(function (data) {
            if (data.success) {
                $('#inputDaneCodePll').val("");
                $('#inputDaneCodePll').val(data.result);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static prospectPlGetCountriesByDescription(description) {
        if (description.length >= 3) {
            ProspectRequest.GetCountriesByDescription(description).done(function (data) {
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
                        $('#modalListSearchCountries').UifModal('showLocal', AppResources.ModalTitleCountries);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }

            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true });
        }
    }

    // State
    static prospectPlGetStatesByCountryIdByDescription(countryId, description) {
        if (description.length >= 3) {
            if (countryId != undefined) {
                ProspectRequest.GetStatesByCountryIdByDescription(countryId, description).done(function (data) {
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
                            $('#modalListSearchStates').UifModal('showLocal', AppResources.ModalTitleStates);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDocumentControlCountry, 'autoclose': true })
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // City
    static prospectPlGetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
        if (description.length >= 3) {
            if (countryId != undefined && stateId != undefined) {
                ProspectRequest.GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description).done(function (data) {
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
                            $('#modalListSearchCities').UifModal('showLocal', AppResources.ModalTitleCities);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateCountryState, 'autoclose': true })
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    // DANECode
    static prospectPlGetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        ProspectRequest.GetCountryAndStateAndCityByDaneCode(countryId, daneCode).done(function (data) {
            if (data.success) {
                if (data.result !== null && Object.keys(data.result).length > 0) {
                    $("#inputCountryPll1").data({ Id: data.result.State.Country.Id, Description: data.result.State.Country.Description });
                    $("#inputStatePll").data({ Id: data.result.State.Id, Description: data.result.State.Description });
                    $("#inputCityPll").data({ Id: data.result.Id, Description: data.result.Description });
                    $("#inputCountryPll1").val($("#inputCountryPll1").data("Description"));
                    $("#inputStatePll").val($("#inputStatePll").data("Description"));
                    $("#inputCityPll").val($("#inputCityPll").data("Description"));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}