var prospectCode = 0;
var isNew = 0;
var individualId = -1;
var searchType = null;
class ProspectusLegal extends Uif2.Page {

    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        lockScreen();
        DocumentTypeRequest.GetDocumentType("2").done(function (data) {
            if (data.success) {
                $("#selectDocumentTypeprl").UifSelect({ sourceData: data.result });
            }
        });

        $("#inputDaneCodePll").UifAutoComplete({
            source: rootPath + "Person/Person/GetDaneCodeByQuery",
            displayKey: "DANECode",
            queryParameter: "&query"
        });

        this.InitializeProspectusLegal();
        new Persons();
        if (glbPersonIndividualId != null) {
            Persons.GetProspectByDocumentNumSearchType(glbPersonIndividualId, searchType);
            glbPersonIndividualId = null;
        }

        if (glbPersonOnline != null) {
            glbPersonOnline.RolType = 4;
            $("#InputDocumentNumber").val(glbPersonOnline.ViewModel.HolderName);
            setTimeout(function () {
                $("#BranchCheckDelivery").attr("disabled", "disabled");
                $("#InputDocumentNumber").focusout();
            }, 300);
        }
        unlockScreen();
        if ($("#InputDocumentNumber").val() != "" && $('#selectDocumentTypeprl').val() != "") {
            ProspectusLegal.SearchDocumentNumberprl();
        }
    }

    //Seccion Eventos
    bindEvents() {
        //Buscar personas por numero de documento
        $("#InputDocumentNumber").focusout(ProspectusLegal.SearchDocumentNumberprl);
        $('#inputCountryPll1').on("search", this.SearchCountryPll1);
        $("#inputDaneCodePll").on('itemSelected', this.ChangeDaneCodePll);
        $('#inputStatePll').on("search", this.SearchStatePll);
        $('#inputCityPll').on("search", this.SearchCityPll);
        $('#tblResultListCountries tbody').on('click', 'tr', this.ResultListCountries);
        $('#tblResultListStates tbody').on('click', 'tr', this.ResultListStates);
        $('#tblResultListCities tbody').on('click', 'tr', this.ResultListCities);

        // EESGE-172 Control De Busqueda >>
        $('#selectDocumentTypeprl').on('itemSelected', function (event, selectedItem) {
            ProspectusLegal.ChangeTypeAndNumberDocument();
            ProspectusLegal.SearchDocumentNumberprl();

        });
    }

    InitializeProspectusLegal() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputEmailPrl").ValidatorKey(ValidatorType.Emails, 1, 1);
        $('#InputDocumentNumber').ValidatorKey(ValidatorType.Number, 2, 0);
        $('#inputAddressAllPrl').ValidatorKey(ValidatorType.Addresses, 1, 1);
        $('#inputPhoneProspectusLegal').ValidatorKey(ValidatorType.Number);
        ProspectusLegal.NewProspectLegal();
    }

    static SearchDocumentNumberprl() {
        if ($("#InputDocumentNumber").val() != "") {
            $("#InputDigit").val(Shared.CalculateDigitVerify($("#InputDocumentNumber").val()));
            PersonRequest.GetCompanyByDocumentByDocumentType($('#InputDocumentNumber').val(), parseInt($('#selectDocumentTypeprl').val())).done(function (dataperson) {
                if (dataperson.success) {
                    if (dataperson.result) {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.DocumentNumberAlreadyRegistered });
                        $("#InputDocumentNumber").val("");
                        $("#InputDigit").val("0");
                    }
                }
            });
            ProspectusLegal.GetProspectByDocumentNum($("#InputDocumentNumber").val(), 4).done(function (data) {
                if (data.success) {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.DocumentNumberAlreadyRegistered });
                    $("#InputDocumentNumber").val("");
                    $("#InputDigit").val("0");
                }
            });
        }
    }

    SearchCountryPll1(event, value) {
        $('#inputStatePll').val("");
        $('#inputCityPll').val("");
        $('#inputDaneCodePll').val("");
        if ($("#inputCountryPll1").val().trim().length > 0) {
            ProspectusLegal.prospectPlGetCountriesByDescription($("#inputCountryPll1").val());
        }
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

    static GetProspectByDocumentNum(documentNum, searchType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Person/GetProspectByDocumentNum',
            data: JSON.stringify({ documentNum: documentNum, searchType: searchType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    //Seccion Funciones
    static SetProspectLegal(prospectLegal) {

        prospectCode = prospect.ProspectCode;
        individualId = prospect.ProspectCode;

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

        if (prospectLegal.DANECode != null) {
            $("#inputDaneCodePll").val(prospectLegal.DANECode);
            // << EESGE-172 Control De Busqueda
            ProspectusLegal.prospectPlGetCountryAndStateAndCityByDaneCode(prospectLegal.Country.Id, prospectLegal.DANECode);
            // EESGE-172 Control De Busqueda >>
        }
        // EESGE-172 Control De Busqueda >>
    }

    static validateProspectLegal() {
        var msjProspectlegal = "";
        if ($("#selectDocumentTypeprl").UifSelect("getSelected") <= 0) {
            msjProspectlegal = msjProspectlegal + AppResourcesPerson.LabelTypeDocument + "<br>";
        }

        if ($("#InputDocumentNumber").val() == "") {
            msjProspectlegal = msjProspectlegal + AppResourcesPerson.LabelDocumentNumber + "<br>";;
        }
        if ($("#InputFirstName").val() == "") {
            msjProspectlegal = msjProspectlegal + AppResourcesPerson.LabelBusinessName;
        }

        if (msjProspectlegal != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msjProspectlegal })
            return false;

        } else if ($("#inputEmailPrl").val() != "") {
            if (!ValidateEmail($("#inputEmailPrl").val())) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmail })
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
        $('#selectDocumentTypeprl').UifSelect('setSelected', "");
        $("#InputDigit").val("");
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
        $("#inputGeneralInfoPrl").val("");
        if (defaultValues != null) {
            Persons.SetDefaultValues(defaultValues);
        }
        else {
            Persons.GetDefaultValues();
        }
        $("#selectSearchPersonType").UifSelect("setSelected", searchType);
    }

    static RecordProspectLegal() {

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
            TributaryIdNumber: null,
            TradeName: null
        };

        if ($("#lblPersonCode").val() == undefined) {
            prospectData.ProspectCode = 0;
            prospectData.AdditionaInformation = "";
            prospectData.Address.Id = $("#selectCompanyAddressType").val();
            prospectData.Address.Description = $("#inputCompanyAddress").val();
            prospectData.City.Id = $("#inputCompanyCity").data("Id");
            prospectData.State.Id = $("#inputCompanyState").data("Id");
            prospectData.Country.Id = $("#inputCompanyCountry").data("Id");
            prospectData.EmailAddres = $("#selectCompanyEmailType").val();
            prospectData.Name = $('#inputCompanyTradeName').val();
            prospectData.PhoneNumber = $("#inputCompanyPhone").val();
            prospectData.IndividualTypePerson = IndividualTypePerson.Legal;
            prospectData.TributaryIdTypeCode = $('#selectCompanyDocumentType').val();
            prospectData.TributaryIdNumber = $('#inputCompanyDocumentNumber').val().trim();
            prospectData.TradeName = $('#inputCompanyTradeName').val();
        }
        else {
            prospectData.ProspectCode = $("#lblPersonCode").val();
            prospectData.AdditionaInformation = $('#inputGeneralInfoPrl').val();
            prospectData.Address.Id = $("#selectAddressPll").val() == "" ? null : $("#selectAddressPll").val();
            prospectData.Address.Description = $("#inputAddressAllPrl").val();
            prospectData.City.Id = $("#inputCityPll").data("Id");
            prospectData.State.Id = $("#inputStatePll").data("Id");
            prospectData.Country.Id = $("#inputCountryPll1").data("Id");
            prospectData.EmailAddres = $("#inputEmailPrl").val();
            prospectData.Name = $('#InputFirstName').val();
            prospectData.PhoneNumber = $("#inputPhoneProspectusLegal").val();
            prospectData.IndividualTypePerson = IndividualTypePerson.Legal;
            //prospectData.ProspectCode = IndividualTypePerson.Legal;
            prospectData.TributaryIdTypeCode = $('#selectDocumentTypeprl').val();
            prospectData.TributaryIdNumber = $('#InputDocumentNumber').val();
            prospectData.TradeName = $('#InputFirstName').val();
        }
        

        Prospect.CreateProspectPersonLegal(prospectData).done(function (data) {
            $('#btnSavePerson').attr("disabled", false);
            if (data.success) {
                isNew = 0;
                if (individualId == Person.New) {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.ShowSaveData }, function (result) {
                    /*persona en linea*/
                        if ($("#lblPersonCode").val() != undefined) {
                            if (typeof glbPolicy != "undefined" && glbPolicy != null) {
                                if (glbPersonOnline.RolType > 0) {
                                    glbPersonOnline.IndividualId = data.result.ProspectCode;
                                    glbPersonOnline.CustomerType = CustomerType.Prospect;
                                    if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) {
                                        router.run("prtQuotation");
                                    }
                                    else {
                                        router.run("prtTemporal");
                                    }
                                }
                            }
                        }
                    });
                }
                else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.PersonCodeNo + data.result.ProspectCode + " " + AppResourcesPerson.SuccessfulUpdate });
                }
                ProspectusLegal.SetProspectLegal(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    // << EESGE-172 Control De Busqueda
    static LoadDaneCodeProspectLegal() {
        DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCountryPll1").data("Id"), $("#inputStatePll").data("Id"), $("#inputCityPll").data("Id")).done(function (data) {
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
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true });
        }
    }

    // State
    static prospectPlGetStatesByCountryIdByDescription(countryId, description) {
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
    static prospectPlGetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
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
    static prospectPlGetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        DaneCodeRequest.GetCountryAndStateAndCityByDaneCode(countryId, daneCode).done(function (data) {
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

    static ChangeTypeAndNumberDocument() {
        if (glbPersonOnline == null) {
            if ($("#selectDocumentTypeprl").UifSelect("getSelectedSource").IsAlphanumeric) {
                $('#InputDocumentNumber').val("");
                $('#InputDocumentNumber').off('keypress').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
            } else {
                $('#InputDocumentNumber').val("");
                $("#InputDocumentNumber").off('keypress').OnlyDecimals(0);
            }
        } else {
            if ($("#selectDocumentTypeprl").UifSelect("getSelectedSource").IsAlphanumeric) {
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


                //TradeName { get; set; }
                //ProspectCode { get; set; }                
                //AdditionaInformation { get; set; }
                //Address { get; set; }
                //City { get; set; }
                //State { get; set; }
                //Country { get; set; }
                //DANECode { get; set; }
                //EmailAddres { get; set; }
                //Name { get; set; }
                //PhoneNumber { get; set; }
                //IndividualTypePerson { get; set; }
                //TributaryIdNumber { get; set; }
                //TributaryIdTypeCode { get; set; }

                rstl.push(resultData);
            });
        }
        return rstl;
    }
}