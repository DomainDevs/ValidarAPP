var isNew = 0;
var individualId = -1;
var searchType = null;
var bandFutureSociety = false;
var InfoPersona;
var RestingedPartnerShip = [
    2, //CONSORCIO
    3, //UNIÓN TEMPORAL
    4  //SOCIEDADES FUTURAS
]
class PersonLegal extends Uif2.Page {
    getInitialState() {
        lockScreen();
        new RepresentLegal();
        new StaffLabour();
        new Partner();
        new ConsortiumMember();
        new BusinessName();

        $("#inputCompanyDaneCode").UifAutoComplete({
            source: rootPath + "Person/Person/GetDaneCodeByQuery",
            displayKey: "DANECode",
            queryParameter: "&query"
        });

        PersonRequest.LoadInitialLegalData(2).done(function (data) {
            if (data.success) {
                $("#selectCompanyDocumentType").UifSelect({ sourceData: data.result.DocumentTypes });
                $("#selectCompanyTypePnSearch").UifSelect({ sourceData: data.result.DocumentTypes });

                $("#selectTypePartnership").UifSelect({ sourceData: data.result.AssociationTypes });

                $("#selectCompanyTypePartnership").UifSelect({ sourceData: data.result.CompanyTypes });
            }
        });

        this.InitializeCompany();
        Persons.AddSubtitlesRightBar();
        new Persons();
        if (glbPersonIndividualId != null) {
            Persons.GetCompanyByIndividualId(glbPersonIndividualId);
            glbPersonIndividualId = null;
        }

        if (glbPersonOnline != null) {
            glbPersonOnline.RolType = 2;
            $("#inputCompanyDocumentNumber").val(glbPersonOnline.ViewModel.HolderIdentificationDocument);
            $("#inputCompanyDocumentNumber").val(glbPersonOnline.ViewModel.HolderName);
            $("#inputCompanyDigit").val(Shared.CalculateDigitVerify($('#inputCompanyDocumentNumber').val().trim()));
        }

        unlockScreen();
        if ($("#selectSearchPersonType").val() == TypePerson.PersonLegal.toString()) {
            $("#btnEmployee").hide();
        }

        $("#inputCompanyDocumentNumber").ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
    }

    //Seccion Eventos
    bindEvents() {
        $("#inputCompanyTradeName").focusout(this.ValidateListRiskPerson);
        $("#inputCompanyDocumentNumber").focusout(this.ValidateListRiskPerson);
        $("#inputCompanyDocumentNumber").focusout(this.SearchCompanyDocument);
        $("#btnLegAddresses").click(this.AddressesLeg);
        $("#btnCompanyEmail").click(this.CompanyEmail);
        $("#btnPhoneCompany").click(this.PhoneCompany);
        $("#inputCompanyTradeName").focusout(this.CompanyTradeName);
        $('#inputCompanyCountry').on("search", this.SearchCompanyCountry);
        $('#inputCompanyCountryOrigin').on("search", this.SearchCompanyCountryOrigin);
        $("#inputCompanyDaneCode").on('itemSelected', this.ChangeCompanyDaneCode);
        $('#inputCompanyState').on("search", this.SearchCompanyState);
        $('#inputCompanyCity').on("search", this.SearchCompanyCity);
        $('#tblResultListCountries tbody').on('click', 'tr', this.SelectSearchCountries);
        $('#tblResultListCountriesOrigin tbody').on('click', 'tr', this.SelectSearchCountriesOrigin);
        $('#tblResultListStates tbody').on('click', 'tr', this.SelectSearchStates);
        $('#tblResultListCities tbody').on('click', 'tr', this.SelectSearchStatesCities);
        $("#selectTypePartnership").on('itemSelected', this.ChangeTypePartnership);
        $('#selectCompanyDocumentType').on('itemSelected', this.ChangeTributaryTypeAndNumberDocument);
        $('#tblPerson2G').on('rowSelected', this.SelectedRowModalPerson2G);
    }

    ValidateListRiskPerson() {
        let documentNumber = $("#inputCompanyDocumentNumber").val().trim();
        let name = $("#inputCompanyTradeName").val().trim();

        PersonRequest.ValidateListRiskPerson(documentNumber, name).done(data => {
            if (data.success && data.result !== null) {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
    }


    ChangeTypePartnership(event, selectedItem) {
        $("#inputCompanyDocumentNumber").prop("disabled", false);
        if (selectedItem.Id == TypePartnership.Future || selectedItem.Id == TypePartnership.Consortium || selectedItem.Id == TypePartnership.TemporalUnion) {
            PersonRequest.GetUserAssignedConsortiumByparameterFutureSocietyByuserId().done(function (data) {
                if (data.success) {
                    if (glbPersonOnline == null) {
                        bandFutureSociety = true;
                        $("#inputCompanyDocumentNumber").val(data.result.NitAssignedConsortium);
                        $("#inputCompanyDocumentNumber").prop("disabled", "disabled");
                        $("#inputCompanyDigit").val(Shared.CalculateDigitVerify($('#inputCompanyDocumentNumber').val().trim()))
                    } else {
                        $('#btnSavePerson').attr("disabled", true);
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.ValidateTypeAssociation, 'autoclose': true });
                    }
                }
                else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.MessageCompany + data.result.IndividualId + " " + AppResourcesPerson.SuccessfulUpdate });
                    $("#inputCompanyDocumentNumber").prop("disabled", "disabled");
                }

            });
        }
        else if (bandFutureSociety) {
            bandFutureSociety = false;
            $("#inputCompanyDocumentNumber").val("");
            $("#inputCompanyDigit").val("");
        }
        if (glbPersonOnline != null) {
            $('#btnSavePerson').attr("disabled", false);
        }
    }

    SearchCompanyDocument() {
        if ($.trim($("#inputCompanyDocumentNumber").val()) != "") {
            PersonRequest.GetAplicationCompanyByDocument(TypePerson.PersonLegal, $("#inputCompanyDocumentNumber").val().trim()).done(function (data) {
                if (data.success) {
                    if (data.result.length >= 1) {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.DocumentNumberAlreadyRegistered });
                        $("#inputCompanyDigit").val("0");
                        $("#inputCompanyDocumentNumber").val("");
                    }
                    else {
                        PersonRequest.GetPerson2gByDocumentNumber($("#inputCompanyDocumentNumber").val().trim(), true).done(function (data) {
                            if (data.success) {
                                if (data.result !== null && data.result.length > 0) {
                                    var dataStates = [];
                                    $.each(data.result, function (index, value) {
                                        dataStates.push({
                                            PersonId: value.IndividualId,
                                            Role: value.Role.Description,
                                            FullName: value.FullName
                                        });
                                    });
                                    $('#tblPerson2G').UifDataTable('clear');
                                    $("#tblPerson2G").UifDataTable('addRow', dataStates);
                                    $('#modalPerson2G').UifModal('showLocal', Resources.Language.People2g);
                                }
                            }
                        });
                    }
                }
                $("#inputCompanyDigit").val(Shared.CalculateDigitVerify($('#inputCompanyDocumentNumber').val().trim()))
            });
        }
    }

    ChangeTributaryTypeAndNumberDocument() {
        $('#inputCompanyDocumentNumber').val("");
        if ($("#selectCompanyDocumentType").UifSelect("getSelectedSource")) {
            if ($("#selectCompanyDocumentType").UifSelect("getSelectedSource").IsAlphanumeric) {
                $('#inputCompanyDocumentNumber').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
            } else {
                $("#inputCompanyDocumentNumber").ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
            }
            if ($("#selectCompanyDocumentType").UifSelect("getSelectedSource").Id == 2) {
                $("#inputCompanyDocumentNumber").ValidatorKey(ValidatorType.OnlylettersandnumbersNIT, 6, 0);
            }
            else {
                $("#inputCompanyDocumentNumber").ValidatorKey(ValidatorType.Onlylettersandnumbers, 7, 0);
            }
        }
    }

    AddressesLeg() {
        if (PersonLegal.ValidateAddressPersonLegal()) {
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Address.ClearAddress();
            Address.GetAddresses();
            Persons.ShowPanelsPerson(RolType.Address);
        }
    }

    CompanyEmail() {
        if (Email.ValidateEmailPerson()) {
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Email.ClearEmail();
            Email.GetEmailsses();
            Persons.ShowPanelsPerson(RolType.Email);
        }
    }

    PhoneCompany() {
        if (PersonLegal.ValidatePhoneCompany()) {
            if (individualId <= 0) {
                let message = "";
                message = AppResourcesPerson.ErrorCompanyExists + "<br>";
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + message })
            }
            else {
                $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
                Phone.ClearPhone();
                Phone.GetPhonesses();
                Persons.ShowPanelsPerson(RolType.Phone);
            }
        }
    }

    CompanyTradeName() {
        $("#inputcheckpayable").val($("#inputCompanyTradeName").val());
    }

    SearchCompanyCountry(event, value) {
        $('#inputCompanyState').val("");
        $('#inputCompanyCity').val("");
        $('#inputDaneCodePn').val("");
        if ($("#inputCompanyCountry").val().trim().length > 0) {
            PersonLegal.companyGetCountriesByDescription($("#inputCompanyCountry").val());
        }
    }

    SearchCompanyCountryOrigin(event, value) {
        $('#inputCompanyState').val("");
        $('#inputCompanyCity').val("");
        $('#inputDaneCodePn').val("");
        if ($("#inputCompanyCountryOrigin").val().trim().length > 0) {
            PersonLegal.companyGetCountriesOriginByDescription($("#inputCompanyCountryOrigin").val());
        }
    }

    ChangeCompanyDaneCode() {
        PersonLegal.companyGetCountryAndStateAndCityByDaneCode($('#inputCompanyCountry').data("Id"), $("#inputCompanyDaneCode").val());
    }

    SearchCompanyState(event, value) {
        $('#inputCompanyCity').val("");
        $('#inputDaneCodePn').val("");
        if ($("#inputCompanyState").val().trim().length > 0) {
            PersonLegal.companyGetStatesByCountryIdByDescription($("#inputCompanyCountry").data('Id'), $("#inputCompanyState").val());
        }
    }

    SearchCompanyCity(event, value) {
        $('#inputDaneCodePn').val("");
        if ($("#inputCompanyCity").val().trim().length > 0) {
            PersonLegal.companyGetCitiesByCountryIdByStateIdByDescription($("#inputCompanyCountry").data('Id'), $("#inputCompanyState").data('Id'), $("#inputCompanyCity").val());
        }
    }

    static LoadDaneCodePersonLegal() {
        if (!isNull($("#inputCompanyCountry").data("Id")) && !isNull($("#inputCompanyState").data("Id")) && !isNull($("#inputCompanyCity").data("Id"))) {
            DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCompanyCountry").data("Id"), $("#inputCompanyState").data("Id"), $("#inputCompanyCity").data("Id")).done(function (data) {
                if (data.success) {
                    $('#inputCompanyDaneCode').val("");
                    $('#inputCompanyDaneCode').val(data.result);
                    //PersonLegal.SearchDaneCodePn();
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SelectSearchCountries() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCompanyCountry").data(dataCountry);
        $("#inputCompanyCountry").val(dataCountry.Description);
        $('#modalListSearchCountries').UifModal('hide');
    }

    SelectSearchCountriesOrigin() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCompanyCountryOrigin").data(dataCountry);
        $("#inputCompanyCountryOrigin").val(dataCountry.Description);
        $('#modalListSearchCountriesOrigin').UifModal('hide');
    }

    SelectSearchStates() {
        var dataState = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputCompanyState").data(dataState);
        $("#inputCompanyState").val(dataState.Description);
        $('#modalListSearchStates').UifModal('hide');
    }

    SelectSearchStatesCities(e) {
        var dataCities = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputCompanyCity").data(dataCities);
        $("#inputCompanyCity").val(dataCities.Description);
        $('#modalListSearchCities').UifModal('hide');
        PersonLegal.GetDaneCodeCompany();
    }

    //Seccion Funciones
    InitializeCompany() {
        isNew = 1;
        individualId = Person.New;
        searchType = TypePerson.PersonLegal;
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //$("#inputCompanyDocumentNumber").ValidatorKey();
        $("#inputCompanyPhone").ValidatorKey(ValidatorType.PhoneCompany, 2, 1);
        $('#inputCompanyAddress').ValidatorKey(ValidatorType.Addresses, 0, 1);
        $("#inputCompanyEmail").ValidatorKey(ValidatorType.Emails, 0, 1);
        PersonLegal.NewLegal();
    }

    static EnabledControlsCompany(control) {
        $("#selectCompanyDocumentType").prop("disabled", control);
        $("#inputCompanyDocumentNumber").prop("disabled", control);
        //$("#inputCompanyTradeName").prop("disabled", control);
        $("#selectTypePartnership").prop("disabled", control);
    }
    //seccion Load

    static LoadCompany(company) {
        searchType = TypePerson.PersonLegal;
        $.uif2.helpers.setGlobalTitle(company.BusinessName + " -NIT: " + company.Document);
        isNew = 0;
        individualId = company.Id;
        ConsortiumMember.CleanObjectConsortiumMembers();
        PersonLegal.EnabledControlsCompany(true);
        $("#selectCompanyDocumentType").UifSelect("setSelected", company.DocumentTypeId);
        //$("#selectCompanyTypePnSearch").UifSelect("setSelected", company.DocumentTypeId);
        //$("#selectTypeExemption").UifSelect("setSelected", company.ExonerationTypeCode);
        $("#UpdateBy").val(company.UpdateBy);
        //$("#LastUpdate").val(FormatDate(person.LastUpdate), 1);
        $("#LastUpdate").val(FormatDate(""), 1);
        $("#inputDocument").val(company.Document);
        $("#inputCompanyDocumentNumber").val(company.Document);
        $("#inputCompanyDigit").val(Shared.CalculateDigitVerify(company.Document));
        $("#lblCompanyCode").val(company.Id);
        $("#inputCompanyTradeName").val(company.BusinessName);
        if ($("#inputcheckpayable").val(company.CheckPayable) === null || $("#inputcheckpayable").val(company.CheckPayable) === undefined || $("#inputcheckpayable").val(company.CheckPayable) === "") {
            $("#inputcheckpayable").val(company.BusinessName);
        } else {
            $("#inputcheckpayable").val(company.CheckPayable);
        }

        $('#AgentCheckPayableTo').val(company.BusinessName);
        PersonLegal.GetCountryOriginByCode(company.CountryOriginId);
        $("#selectTypePartnership").UifSelect("setSelected", company.AssociationTypeId);

        $("#selectCompanyTypePartnership").UifSelect("setSelected", company.CompanyTypeId);

        $("#inputCompanyEconomyActivity").data("Id", company.EconomicActivityId);

        if (company.EconomicActivityDescription != null)
            $("#inputCompanyEconomyActivity").val(company.EconomicActivityDescription + ' (' + company.EconomicActivityId + ')');

        if (company.Addresses != null) {
            addresses = company.Addresses;
            addressesGet = addresses;
            Address.LoadAddress(company.Addresses);
            Address.setPrincipalAddressLegal(company.Addresses);
            PersonLegal.LoadDaneCodePersonLegal($("#inputCompanyCity").data("Id"));
        }
        else {
            Persons.GetdefaultValueCountry();
        }
        if (company.Phones != null) {
            Phonesses = company.Phones;
            PhonessesGet = Phonesses;
            Phone.LoadPhonesses(Phonesses);
            Phone.setPrincipalPhoneCompany(Phonesses);
        }
        if (company.Emails != null) {
            emails = company.Emails;
            emailsGet = emails;
            Email.LoadEmailsses(emails);
            Email.setPrincipalEmailCompany(emails);
        }

        //if (company.ExonerationTypeCode != null && company.ExonerationTypeCode > 0) {
        //    $("#selectTypeExemption").UifSelect("setSelected", company.ExonerationTypeCode);
        //}
        //else {
        //    $("#selectTypeExemption").UifSelect("setSelected", null);
        //}

        if (company.Sarlafts != null) {
            Sarlafs = company.Sarlafts;
            Sarlaft.LoadSarlaft(company.Sarlafts);
        }
        if (company.Partners != null) {
            Partners = company.Partners;
            Partner.LoadPartners();
        }
        if (company.PaymentMethodAccount != null) {
            PaymentMethod = company.PaymentMethodAccount;
            MethodPayment.LoadPaymentMethod(PaymentMethod);
        }

        Person.Insured = Insured;

        if (company.ConsortiumMembers != null) {
            $.each(company.ConsortiumMembers, function (index, value) {
                value.StartDate = FormatDate(value.StartDate)
            });
            ConsortiumMembers = company.ConsortiumMembers;
            ConsortiumMember.LoadConsortiumMembers();
            Person.ConsortiumMembers = ConsortiumMembers;
        }
        if (company.LegalRepresentative != null) {

            if (company.LegalRepresentative.IdentificationDocument.DocumentType != null) {
                $("#selectDocumentTypeRepresentLegal").UifSelect("setSelected", company.LegalRepresentative.IdentificationDocument.DocumentType.Id);
            }
            $("#InputExpeditiondateRepresentLegal").val(FormatDate(company.LegalRepresentative.ExpeditionDate));
            $("#InputFullNameRepresentLegal").val(company.LegalRepresentative.FullName);
            var newFormattedDate = FormatDate(company.LegalRepresentative.BirthDate)
            $("#InputBirthdateRepresentLegal").val(newFormattedDate);
            $("#InputNationalityRepresentLegal").val(company.LegalRepresentative.Nationality);
            $("#InputPhoneReprersentLegal").val(company.LegalRepresentative.Phone);
            $("#InputCellReprersentLegal").val(company.LegalRepresentative.CellPhone);
            $("#selectCurrencyRepresentLegal").UifSelect("setSelected", "0");
            $("#InputEmailReprersentLegal").val(company.LegalRepresentative.Email);
            if (company.LegalRepresentative.AuthorizationAmount.Currency !== null)
                $("#selectCurrencyRepresentLegal").UifSelect("setSelected", company.LegalRepresentative.AuthorizationAmount.Currency.Id);
            $("#notesReprersentLegal").val(company.LegalRepresentative.Description);
            $("#InputAddressReprersentLegal").val(company.LegalRepresentative.Address);
            if (company.LegalRepresentative.City.Id !== null) {
                $("#inputCityRepresentLegal").data({ Id: company.LegalRepresentative.City.Id, Description: company.LegalRepresentative.City.Description });
                $("#inputCityRepresentLegal").val(company.LegalRepresentative.City.Description);
                if (company.LegalRepresentative.City.State.Id !== null) {
                    $("#inputStateRepresentLegal").data({ Id: company.LegalRepresentative.City.State.Country.Id, Description: company.LegalRepresentative.City.State.Description });
                    $("#inputStateRepresentLegal").val(company.LegalRepresentative.City.State.Description);
                    if (company.LegalRepresentative.City.State.Country.Id !== null) {
                        $("#inputCountryRepresentLegal").data({ Id: company.LegalRepresentative.City.State.Country.Id, Description: company.LegalRepresentative.City.State.Country.Description });
                        $("#inputCountryRepresentLegal").val(company.LegalRepresentative.City.State.Country.Description);
                    }
                }
                RepresentLegal.getDaneByCityIdStateIdCountryId();
            }
            $("#InputPlaceOfBirthRepresentLegal").val(company.LegalRepresentative.BirthPlace);
            $("#InputOfficeReprersentLegal").val(company.LegalRepresentative.JobTitle);
            $("#InputNumDocumentRepresentLegal").val(company.LegalRepresentative.IdentificationDocument.Number);
            $("#InputvalueRepresentLegal").val(FormatMoney(company.LegalRepresentative.AuthorizationAmount.Value));
            $("#selectDocumentTypeRepresentLegal").UifSelect("getSelected", company.LegalRepresentative.IdentificationDocument.DocumentType.Id);
            $("#InputSurnamesManager").val(company.LegalRepresentative.ManagerName);
            $("#InputSurnamesGenManager").val(company.LegalRepresentative.GeneralManagerName);
            $("#InputSurnameContactCom").val(company.LegalRepresentative.ContactName);
            $("#InputNotes").val(company.LegalRepresentative.ContactAdditionalInfo);

            RepresentLegal.SendRepresentLegal();
        }

        if (company.CompanyNames != null) {
            BusinessData = company.CompanyNames;
        }
        Persons.ValidateRolPerson(individualId);
        if ($("#inputCompanyTradeName").val() != "" && company.CheckPayable == null) {
            $("#inputcheckpayable").val($("#inputCompanyTradeName").val());
        }
        else {
            $("#inputcheckpayable").val(company.CheckPayable);
        }
    }

    static ShowCompany() {
        if (glbPersonOnline.RolType > 0) {
            PersonLegal.LoadConfirmationCompany();
        }
        else {
            $.UifDialog('confirm', { 'message': AppResourcesPerson.NoExistCompanyWantCreate }, function (result) {
                /*persona en linea*/
                if (result) {
                    PersonLegal.LoadConfirmationCompany();
                }
            });
        }
    }

    static LoadConfirmationCompany() {

        individualId = Person.New;
        isNew = 1;
        PersonLegal.NewLegal();
        HidePanelsPerson();
        DisabledPanelsSearch(true);
        ClearRepresentLegal();
        $("#panelPersonLegal").show();
        $("#btnBottomMain").show();
        var raz = $("#inputTradeName").val();
        $("#inputCompanyDigit").val(Shared.CalculateDigitVerify($("#inputCompanyDocumentNumber").val().trim()));
        $("#selectCompanyDocumentType").UifSelect("setSelected", DocumentType.NIT);
        $("#selectCompanyDocumentType").focus();
        Persons.GetdefaultValueCountry();
    }

    //seccion Grabar
    static RecordCompany(showAlert, Rguarantee, RstaffLabour) {
        var dfd = $.Deferred();
        var array = new Array();
        var newIndividualId = "";
        var dataCompany = {};
        if (showAlert == undefined) {
            showAlert = true;
        }
        if (PaymentMethod == null || PaymentMethod.length == 0) {
            MethodPayment.FillObjectMethodPayment();
        }
        if (addresses == null || addresses.length == 0) {
            Address.SetMainAddressPersonLegal();
        }
        else {
            Address.UpdatePrincipalAddressCompany();
        }

        if (Phonesses == null || Phonesses.length == 0) {
            Phone.FillObjectPhoneCompany();
        }
        else {
            Phone.UpdatePrincipalPhoneCompany();
        }
        if (emails == null || emails.length == 0) {
            Email.FillObjectEmailCompany();
        }
        else {
            Email.UpdatePrincipalEmailCompany();
        }
        if (Sarlafs != null && Sarlafs.length > 0) {
            Sarlaft.UnFormatSarlaft();
        }

        var companyData = PersonLegal.CreateCompanyModel();
        $('#btnSavePerson').attr("disabled", true);
        lockScreen();
        array.push(
            new Promise((resolve, reject) => {
                PersonRequest.CreateCompany(companyData).done(function (data) {
                    $('#btnSavePerson').prop("disabled", false);
                    if (data.success) {
                        var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                        let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                        if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                            if (countAuthorization > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies,
                                    data.result.OperationId,
                                    FunctionType.PersonGeneral);
                            }
                            reject();
                        } else {
                            dataCompany = data.result;
                            addresses = data.result.Addresses;
                            addressesGet = addresses;
                            Phonesses = data.result.Phones;
                            PhonessesGet = Phonesses;
                            emails = data.result.Emails;
                            emailsGet = emails;
                            isNew = 0;
                            newIndividualId = data.result.Id;
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        reject();
                    }
                    resolve();
                })
                    .fail(xhr => {
                        reject(xhr);
                    })
            }));

        Promise.all(array).then(() => {
            if (individualId == Person.New) {
                individualId = newIndividualId
                $("#lblCompanyCode").val(newIndividualId);
                $("#lblCompanyCode").text(newIndividualId);
                if (companyData.AssociationTypeId == TypePartnership.Individual) {
                    $.UifDialog('confirm', { 'message': AppResourcesPerson.MessageCompany + newIndividualId + ' ' + AppResourcesPerson.SuccessfullyCreatedWantCeateInsured }, function (result) {
                        if (result) {
                            PersonLegal.ClearObjectsCompany();
                            PersonLegal.ReturnRecord(dataCompany, Rguarantee, RstaffLabour);
                            Provider.loadProviderPerson(dataCompany.Provider);
                            Insured.loadInsured();
                        }
                        else {
                            PersonLegal.ClearObjectsCompany();
                            PersonLegal.ReturnRecord(dataCompany, Rguarantee, RstaffLabour);
                            Provider.loadProviderPerson(dataCompany.Provider);
                        }
                    });
                }
                //else if (companyData.AssociationTypeId == "2") {
                //    Insured.loadInsured();
                //}
                else {
                    if (showAlert) {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.MessageCompany + newIndividualId + " " + AppResourcesPerson.LabelPersonCreate });
                    }
                    $('#checkInsured').prop('checked', true);
                    $('#checkInsured').addClass('primary');

                    PersonLegal.ClearObjectsCompany();
                    PersonLegal.ReturnRecord(dataCompany, Rguarantee, RstaffLabour);
                }
            }
            else {
                individualId = newIndividualId
                PersonLegal.ReturnRecord(dataCompany, Rguarantee, RstaffLabour);
                if (showAlert) {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.MessageCompany + newIndividualId + " " + AppResourcesPerson.SuccessfulUpdate });
                }
            }
            dfd.resolve(true);
        }).catch(reason => {
            dfd.reject();
            console.log(reason);
            unlockScreen();
        });
        return dfd.promise();
    }


    static ReturnRecord(data, Rguarantee, RstaffLabour) {
        /*persona en linea*/
        if (typeof glbPolicy != "undefined" && glbPolicy != null) {
            if (glbPersonOnline.RolType > 0) {
                glbPersonOnline.IndividualId = data.Id;
                glbPersonOnline.DocumentNumber = data.Document;
                glbPersonOnline.CustomerType = CustomerType.Individual;
                /*if (glbPolicy.TemporalType == TemporalType.Quotation) {
                if (glbPolicy.TemporalType == TemporalType.Quotation) {
                    router.run("prtQuotation");
                }
                else {
                    router.run("prtTemporal");
                }
            }*/
            }
        }

        if (glbUser != "undefined" && glbUser != null) {
            glbPersonOnline.IndividualId = data.IndividualId;
            glbPersonOnline.DocumentNumber = data.IdentificationDocument.Number;
            router.run("prtUniqueUser");
        }

        if (Rguarantee) {
            var GuaranteeViewModel = {
                ContractorId: data.IndividualId,
                ContractorNumber: $("#inputCompanyDocumentNumber").val().trim(),
                ContractorName: $('#inputCompanyTradeName').val(),
                ContractorDocumentType: $("#selectCompanyDocumentType option:selected").text(),
                searchType: TypePerson.PersonLegal
            }

            var codeInsured = -1;
            var individualId = data.IndividualId;
            InsuredRequest.GetInsuredByIndividualId(individualId).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        codeInsured = data.result.InsuredId;
                        Persons.CreteInsuredGuarantee(codeInsured, individualId, GuaranteeViewModel);
                    }
                }
                else {
                    Persons.CreteInsuredGuarantee(codeInsured, individualId, GuaranteeViewModel);
                }
            });
        }
        else if (RstaffLabour) {
            PersonLegal.ClearObjectsCompany();
            PersonLegal.LoadCompany(data);
            Persons.ShowPanelPeople();
            Persons.ShowPanelsPerson(RolType.StaffLabour);
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
        }
        else {
            PersonLegal.ClearObjectsCompany();
            PersonLegal.LoadCompany(data);
        }
    }

    static GuaranteeModel(individualId) {
        var GuaranteeViewModel = {
            ContractorId: individualId,
            ContractorNumber: $("#inputCompanyDocumentNumber").val().trim(),
            ContractorName: $('#inputCompanyTradeName').val(),
            searchType: TypePerson.PersonLegal,
            Address: $('#inputCompanyAddress').val(),
            ContractorDocumentType: $("#selectCompanyDocumentType option:selected").text(),
            PhoneNumber: $("#inputCompanyPhone").val(),
            CityText: $("#inputCompanyCity").data("Description"),
            StateText: $("#inputCompanyState").data("Description")
        }
        guaranteeModel = GuaranteeViewModel;
        router.run("prtGuaranteeE");
    }

    static CreateCompanyModel() {
        var isExonerated = false;

        var SarlaftExoneration = [];
        SarlaftExoneration = {
            RolId: IndividualRol.Insured,

            EnteredDate: DateNowPerson,
            IsExonerated: isExonerated
        };
        var individualRols = [];
        if ($('#checkInsured').is(':checked')) {
            individualRols.push({ Id: IndividualRol.Insured });
        }
        if ($('#checkProvider').is(':checked')) {
            individualRols.push({ Id: IndividualRol.Provider });
        }
        if ($('#checkThird').is(':checked')) {
            individualRols.push({ Id: IndividualRol.Third });
        }
        if ($('#checkAgent').is(':checked')) {
            individualRols.push({ Id: IndividualRol.Intermediary });
        }
        if ($('#checkReinsurer').is(':checked')) {
            individualRols.push({ Id: IndividualRol.Reinsured });
        }
        if ($('#checkCoInsurer').is(':checked')) {
            individualRols.push({ Id: IndividualRol.Insured });
        }
        if ($('#checkEmployee').is(':checked')) {
            individualRols.push({ Id: IndividualRol.Employee });
        }
        if (Person.ConsortiumMembers != undefined) {
            var ConsortiumEventDTO = {};
            ConsortiumEventDTO.ConsortiumEventEventType = EventConsortium.CREATE_CONSORTIUM;
            ConsortiumEventDTO.IndividualConsortiumID = individualId;
            ConsortiumEventDTO.IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
            ConsortiumEventDTO.consortiumDTO = {};
            ConsortiumEventDTO.consortiumDTO.IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
            ConsortiumEventDTO.consortiumDTO.ConsotiumId = individualId;
            ConsortiumEventDTO.consortiumDTO.ConsortiumName = $('#inputCompanyTradeName').val();
            ConsortiumEventDTO.consortiumDTO.UpdateDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
            ConsortiumEventDTO.consortiumDTO.AssociationType = $('#selectTypePartnership').val();
            ConsortiumEventDTO.consortiumDTO.AssociationTypeDesc = $('#selectCompanyDocumentType').UifSelect("getSelectedText");

            var ConsortiumEventDTOs = []
            if (Person.ConsortiumMembers.length == 0) {
                Person.ConsortiumMembers = $("#listConsortiumMembers").UifListView('getData');
            }
            Person.ConsortiumMembers.forEach(function (item, index) {
                ConsortiumEventDTOs[index] = {};
                ConsortiumEventDTOs[index].ConsortiumEventEventType = EventConsortium.INICIAL_EVENT;
                ConsortiumEventDTOs[index].IndividualConsortiumID = individualId;
                ConsortiumEventDTOs[index].IndividualId = item.IndividualId;
                ConsortiumEventDTOs[index].IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO = {};
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.IndividualConsortiumId = individualId;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.IndividualPartnerId = item.IndividualId;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.PartnerName = item.FullName;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.ConsortiumId = item.InsuredCode;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.InitDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.EndDate = null;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.ParticipationRate = item.ParticipationRate;
                ConsortiumEventDTOs[index].ConsortiumpartnersDTO.Enabled = item.Enabled;
                if (ConsortiumEventParticipant != undefined) {
                    ConsortiumEventParticipant.forEach(function (item1, index1) {
                        if (ConsortiumEventDTOs[index].IndividualId == item1.IndividualId) {
                            ConsortiumEventDTOs[index].ConsortiumEventEventType = EventConsortium.MODIFY_INDIVIDUAL_TO_CONSORTIUM;
                        }
                    });
                }
                if (item.Enabled == false) {
                    ConsortiumEventDTOs[index].ConsortiumEventEventType = EventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM;
                    ConsortiumEventDTOs[index].ConsortiumpartnersDTO.ParticipationRate = 0;
                }
            });
        }
        //Person.ConsortiumMembers no existe por ende no carga los consorcios nsp-2168

        var companyData = {
            EconomicActivityId: $("#inputCompanyEconomyActivity").data("Id"),
            Id: individualId,
            DocumentTypeId: $('#selectCompanyDocumentType').UifSelect("getSelected"),
            Document: $('#inputCompanyDocumentNumber').val().trim(),
            BusinessName: $('#inputCompanyTradeName').val(),
            AssociationTypeId: $("#selectTypePartnership").UifSelect("getSelected"),
            CompanyTypeId: $('#selectCompanyTypePartnership').UifSelect("getSelected"),
            CountryOriginId: $("#inputCompanyCountryOrigin").data("Id"),
            CheckPayable: $('#inputcheckpayable').val(),
            VerifyDigit: Shared.CalculateDigitVerify($('#inputCompanyDocumentNumber').val().trim()),
            Sarlaft: SarlafstTmp,
            ExonerationTypeCode: null, //$('#selectTypeExemption').UifSelect("getSelected"),
            Addresses: addresses,
            Phones: Phonesses,
            Emails: emails,
            ConsortiumMembers: $("#listConsortiumMembers").UifListView('getData'),
            ConsortiumEventDTO: ConsortiumEventDTO,
            ConsortiumEventDTOs: ConsortiumEventDTOs,
            Insured: Person.Insured,
            NitAssociationType: $("#selectTypePartnership").UifSelect("getSelected") != 1 ? $('#inputCompanyDocumentNumber').val().trim() : 0
        };
        if ($("#selectTypePartnership").UifSelect("getSelected") != 1 && Person.Insured.IndividualId == undefined && company.Insured != undefined) {
            companyData.Insured = company.Insured;
        }
        return companyData;
    }

    //seccion Inicial
    static NewLegal() {
        searchType = TypePerson.PersonLegal;
        individualId = Person.New;
        isNew = 1;
        PersonLegal.ClearControlPersonLegal();
        StaffLabour.ClearStaffLabor();
        Insured.initializeControlInsured();
        insuredTmp = { Id: 0 };
        PersonLegal.EnabledControlsCompany(false);
        $("#chkCompanyPrincipal").prop("disabled", true);
        $("#btnBottomMain").show();
        $("#panelPersonLegal").show();
        PersonLegal.EnabledDisabledCompany(false);
        if (defaultValues != null) {
            Persons.SetDefaultValues(defaultValues);
        }
        else {
            Persons.GetDefaultValues();
        }
        Persons.AddSubtitlesRightBar();
        $("#selectSearchPersonType").UifSelect("setSelected", searchType);
    }

    static ClearControlPersonLegal() {
        $("#lblCompanyCode").val("");
        $("#lblCompanyCode").text("");


        if (document.getElementById("selectCompanyDocumentType") != null)
            $("#selectCompanyDocumentType").UifSelect("setSelected", null);
        $("#selectCompanyTypePartnership").UifSelect("setSelected", null);
        $("#inputCompanyState").val("");
        $("#inputCompanyState").data({ Id: null, Description: null });
        $("#inputCompanyCity").val("");
        $("#inputCompanyCity").data({ Id: null, Description: null });
        $("#selectCompanyPhoneType").UifSelect("setSelected", null);
        $("#selectCompanyEmailType").UifSelect("setSelected", null);
        $("#selectCompanyAddressType").UifSelect("setSelected", null);
        $("#selectTypePartnership").UifSelect("setSelected", null);
        $("#inputCompanyAddress").val("");
        $("#inputCompanyDaneCode").val("");
        $("#inputCompanyPhone").val("");
        $("#inputCompanyEconomyActivity").val("");
        $("#inputCompanyEconomyActivity").text("");
        $("#inputCompanyEmail").val("");
        $("#inputcheckpayable").val("");
        $("#inputCompanyDocumentNumber").val("");
        $("#inputCompanyDigit").val("");
        $("#inputCompanyTradeName").val("");
        $("#inputEmailElectronicBilling").val("");
        PersonLegal.ClearObjectsCompany();
        Persons.GetdefaultValueCountry();
        Persons.ClearPanelPeople();
    }

    static ClearObjectsCompany() {
        Address.ClearAddress();
        Email.ClearEmail();
        Phone.ClearPhone();
        Sarlaft.ClearSarlaft(false);
        MethodPayment.ClearControlMethodPayment(true);
        Email.CleanObjectsEmail();
        Address.CleanObjectAddress();
        Phone.CleanObjectPhonesses();
        Sarlaft.CleanObjectsSarlaft();
        MethodPayment.CleanObjectsPaymentMeans();
        Partner.CleanObjectPartner();
        BusinessName.CleanObjectsBusinessName();
        ConsortiumMember.CleanObjectConsortiumMembers();
        RepresentLegal.ClearRepresentLegal();
        Provider.clearProvider();
        PersonTax.clearTax();
    }

    //Seccion Get
    static GetCountriesCompany(city) {
        if (city != null) {
            if (city.State.Country != null) {
                var dataCountry = {
                    Id: city.State.Country.Id,
                    Description: city.State.Country.Description
                }
                $("#inputCompanyCountry").data(dataCountry);
                $("#inputCompanyCountry").val(dataCountry.Description);
            }

            if (city.State != null && city.State.Id != 0) {
                var dataState = {
                    Id: city.State.Id,
                    Description: city.State.Description
                }
                $("#inputCompanyState").data(dataState);
                $("#inputCompanyState").val(dataState.Description);

                var dataCity = {
                    Id: city.Id,
                    Description: city.Description
                }
                $("#inputCompanyCity").data(dataCity);
                $("#inputCompanyCity").val(dataCity.Description);
                PersonLegal.GetDaneCodeCompany();
            }
        }
    }

    static EnabledDisabledCompany(control) {
        $("#selectCompanyDocumentType").attr("disabled", control);
        $("#inputCompanyDocumentNumber").attr("disabled", control);
        $("#inputCompanyTr  adeName").attr("disabled", control);
        $("#selectTypePartnership").attr("disabled", control);
    }

    //seccion Validacion 
    static ValidateCompany() {
        var msjCompany = "";
        //if ($("#selectTypePartnership").UifSelect("getSelected") != TypePartnership.Individual) {
        //    if (ConsortiumMembers.length == 0) {
        //        msjCompany = msjCompany + AppResources.MessageConsortiumMembersMandatory + '<br>';
        //    }
        //}
        if ($.trim($('#inputCompanyEmail').val()) == "") {
            msjCompany = msjCompany + AppResourcesPerson.Email + "<br>";
        } else {
            if (!ValidateEmail($('#inputCompanyEmail').val())) {
                msjCompany = msjCompany + AppResourcesPerson.ErrorEmail + " <br>";
            }
        }

        if ($("#inputEmailElectronicBilling").val() == "") {
            msjCompany = msjCompany + AppResourcesPerson.ElectronicBillingEmailLabel + "<br>";
        } else {
            if (!ValidateEmail($('#inputEmailElectronicBilling').val())) {
                msjCompany = msjCompany + AppResourcesPerson.ElectronicBillingEmailNoValid + " <br>";
            }
        }


        if ($.trim($("#inputCompanyEconomyActivity").data("Id")) == "") {
            msjCompany = msjCompany + AppResourcesPerson.EconomicActivity + " <br>";
        }

        if ($("#selectCompanyDocumentType").UifSelect("getSelected") == "" || $("#selectCompanyDocumentType").UifSelect("getSelected") == null) {
            msjCompany = AppResourcesPerson.LabelTypeDocument + " <br>";
        }

        if ($("#inputCompanyDocumentNumber").val().trim() == "") {
            msjCompany = msjCompany + AppResourcesPerson.LabelDocumentNumber + " <br>";
        }

        if ($("#inputCompanyTradeName").val() == "") {
            msjCompany = msjCompany + AppResourcesPerson.TradeName + " <br>";
        }

        if ($("#inputCompanyCountryOrigin").data("Id") == "") {
            msjCompany = msjCompany + AppResourcesPerson.LabelCountryOrigin + " <br>";
        }

        if ($("#selectCompanyAddressType").UifSelect("getSelected") == "" || $("#selectCompanyAddressType").UifSelect("getSelected") == null) {
            msjCompany = msjCompany + AppResourcesPerson.LabelTypeAddress + " <br>";
        }

        if ($("#inputCompanyAddress").val() == "") {
            msjCompany = msjCompany + AppResourcesPerson.LabelAddress + " <br>";
        }

        if ($("#inputCompanyCountry").data("Id") == "") {
            msjCompany = msjCompany + AppResourcesPerson.LabelCountry + " <br>";
        }
        else //if ($("#inputCompanyCountry").data("Id") == countryParameter)
        {
            if ($("#inputCompanyState").data("Id") == "" || $("#inputCompanyState").data("Id") == null) {
                msjCompany = msjCompany + AppResourcesPerson.LabelState + " <br>";
            }

            if ($("#inputCompanyCity").data("Id") == "" || $("#inputCompanyCity").data("Id") == null) {
                msjCompany = msjCompany + AppResourcesPerson.LabelCity + " <br>";
            }
        }

        if ($("#selectCompanyPhoneType").UifSelect("getSelected") == "" || $("#selectCompanyPhoneType").UifSelect("getSelected") == null) {
            msjCompany = msjCompany + AppResourcesPerson.LabelPhoneType + " <br>";
        }

        if ($("#inputCompanyPhone").val() == "") {
            msjCompany = msjCompany + AppResourcesPerson.LabelPhone + " <br>";
        }
        if ($("#selectCompanyEmailType").UifSelect("getSelected") == "" || $("#selectCompanyEmailType").UifSelect("getSelected") == null) {
            msjCompany = msjCompany + AppResourcesPerson.LabelEmailType + " <br>";
        }

        if ($("#selectCompanyTypePartnership").UifSelect("getSelected") == "" || $("#selectCompanyTypePartnership").UifSelect("getSelected") == null) {
            msjCompany = msjCompany + AppResourcesPerson.CompanyType;
        }

        if (glbPersonOnline != null && parseInt($("#selectTypePartnership").val()) != TypeOfAssociation.INDIVIDUAL) {
            return AppResourcesPerson.ValidateTypeAssociation;
        }

        if ($("#selectTypePartnership").UifSelect("getSelected") == "" || $("#selectTypePartnership").UifSelect("getSelected") == null) {
            msjCompany = msjCompany + AppResourcesPerson.LabelTypePartnership + " <br>";
        } else if ((parseInt($('#selectTypePartnership').val() || 0) > 1)) {
            if (ConsortiumMembers.length == 0) {
                msjCompany = msjCompany + AppResourcesPerson.MessageConsortiumMembersMandatory + '<br>';
            }

            if (Person.Insured == null || Person.Insured.InsureCode < 1) {
                msjCompany = msjCompany + AppResourcesPerson.InsuredRequired + '<br>';
            }
        }

        return msjCompany;
    }

    static ValidateAddressPersonLegal() {
        var error = "";
        if ($.trim($("#inputCompanyAddress").val()) == "") {
            error = error + AppResourcesPerson.LabelAddressAll + "<br>";
        }
        if ($('#chkCompanyPrincipal').is(':checked') == false) {
            error = error + AppResourcesPerson.LabelAddressMain + "<br>";
        }
        if ($("#inputCompanyState").data("Id") == null || $("#inputCompanyState").data("Id") == "") {
            error = error + AppResourcesPerson.LabelDepartment + "<br>";
        }
        if ($("#inputCompanyCity").data("Id") == null || $("#inputCompanyCity").data("Id") == "") {
            error = error + AppResourcesPerson.LabelCity + "<br>";
        }
        if ($("#selectCompanyAddressType").UifSelect("getSelected") == null || $("#selectCompanyAddressType").UifSelect("getSelected") == "") {
            error = error + AppResourcesPerson.LabelTypeAddress + "<br>";
        }
        if (individualId <= 0) {
            error += AppResourcesPerson.ErrorCompanyExists + "<br>";
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }

    static ValidatePhoneCompany() {
        var error = "";
        if ($("#selectCompanyPhoneType").UifSelect("getSelected") == "" || $("#selectCompanyPhoneType").UifSelect("getSelected") == null) {
            error = error + AppResourcesPerson.LabelPhoneType + "<br>";
        }
        if ($("#inputCompanyPhone").val() == "") {
            error = error + AppResourcesPerson.LabelPhone + "<br>";
        }

        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }

    static GetDaneCodeCompany() {
        if ($("#inputCompanyCity").data("Id") != null && $("#inputCompanyCity").data("Id") > 0) {
            DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCompanyCountry").data("Id"), $("#inputCompanyState").data("Id"), $("#inputCompanyCity").data("Id")).done(function (data) {
                if (data.success) {
                    $('#inputCompanyDaneCode').val("");
                    $('#inputCompanyDaneCode').val(data.result);
                    PersonLegal.SearchDaneCodePn($("#inputCompanyCountry").data("Id"));
                } else {
                    $("#inputCompanyDaneCode").val("");
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static companyGetCountriesByDescription(description) {
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
                        $('#modalListSearchCountries').UifModal('showLocal', AppResources.ModalTitleCountries);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageNotFoundCountries, 'autoclose': true });
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

    static companyGetCountriesOriginByDescription(description) {
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
                        $('#tblResultListCountriesOrigin').UifDataTable('clear');
                        $("#tblResultListCountriesOrigin").UifDataTable('addRow', dataCountries);
                        $('#modalListSearchCountriesOrigin').UifModal('showLocal', AppResources.ModalTitleCountries);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageNotFoundCountries, 'autoclose': true });
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
    static companyGetStatesByCountryIdByDescription(countryId, description) {
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
                            $('#modalListSearchStates').UifModal('showLocal', AppResources.ModalTitleStates);
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

    static SearchDaneCodePn(countryId) {
        PersonLegal.companyGetCountryAndStateAndCityByDaneCode(countryId, $("#inputCompanyDaneCode").val());
    }
    // City
    static companyGetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
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
    static companyGetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        DaneCodeRequest.GetCountryAndStateAndCityByDaneCode(countryId, daneCode).done(function (data) {
            if (data.success) {
                if (data.result !== null && Object.keys(data.result).length > 0) {
                    $("#inputCompanyCountry").data({ Id: data.result.State.Country.Id, Description: data.result.State.Country.Description });
                    $("#inputCompanyState").data({ Id: data.result.State.Id, Description: data.result.State.Description });
                    $("#inputCompanyCity").data({ Id: data.result.Id, Description: data.result.Description });
                    $("#inputCompanyCountry").val($("#inputCompanyCountry").data("Description"));
                    $("#inputCompanyState").val($("#inputCompanyState").data("Description"));
                    $("#inputCompanyCity").val($("#inputCompanyCity").data("Description"));
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
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
                    CompanyExtended: {
                        AssociationType: {}
                    },
                    CompanyType: {},
                    EconomicActivity: {},
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
                resultData.Name = company.result[index].BusinessName;
                resultData.IdentificationDocument.Number = company.result[index].Document;
                resultData.IndividualId = company.result[index].Id;
                resultData.IdentificationDocument.DocumentType.Id = company.result[index].DocumentTypeId;
                resultData.UpdateBy = '';
                resultData.LastUpdate = '';
                resultData.CountryOrigin.Id = company.result[index].CountryOriginId;
                resultData.CountryOrigin.Description = company.result[index].Addresses != null ? (company.result[index].Addresses[0] != undefined ? company.result[index].Addresses[0].CountryDescription : "") : "";
                resultData.CompanyExtended.AssociationType.Id = company.result[index].AssociationTypeId;
                resultData.CompanyType.Id = company.result[index].CompanyTypeId;
                resultData.EconomicActivity.Id = company.result[index].EconomicActivityId;
                resultData.EconomicActivity.Description = company.result[index].EconomicActivityDescription;
                resultData.Addresses = Address.ConvertAddressDtoToModel(company.result[index].Addresses);
                resultData.Phones = Phone.ConvertAddressDtoToModel(company.result[index]);
                resultData.Emails = Email.ConvertAddressDtoToModel(company.result[index]);
                resultData.ExonerationTypeCode = company.result[index].ExonerationTypeCode;


                rstl.push(resultData);
            });
        }
        return rstl;
    }

    static GetCountryOriginByCode(CountryCd) {
        CountryRequest.GetCountriesById(CountryCd).done(function (response) {
            if (response.success) {
                var dataCountry = {
                    Id: response.result.Id,
                    Description: response.result.Description
                };
                $("#inputCompanyCountryOrigin").data(dataCountry);
                $("#inputCompanyCountryOrigin").val(dataCountry.Description);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result })
            }
        });
    }

    static GetCountriesPersonLegal(city) {
        if (city != null) {
            var dataCountry = {
                Id: city.State.Country.Id,
                Description: city.State.Country.Description
            }
            $("#inputCompanyCountry").data(dataCountry);
            $("#inputCompanyCountry").val(dataCountry.Description);

            var dataState = {
                Id: city.State.Id,
                Description: city.State.Description
            }
            $("#inputCompanyState").data(dataState);
            $("#inputCompanyState").val(dataState.Description);
            state = dataState;

            if (city.State.Id != 0) {
                var dataCity = {
                    Id: city.Id,
                    Description: city.Description,
                    DANECode: city.DANECode
                }
                $("#inputCompanyCity").data(dataCity);
                $("#inputCompanyCity").val(dataCity.Description);
                city = dataCity;

                $('#inputCompanyDaneCode').UifAutoComplete('setValue', city.DANECode);
            }
        }
    }

    SelectedRowModalPerson2G(event, data, position) {
        $.UifDialog('confirm', { 'message': AppResourcesPerson.Person2GMigrateSure }, function (result) {
            if (result) {
                var personId = data.PersonId;
                PersonRequest.GetCompany2gByPersonId(personId, true).done(function (data) {
                    if (data.success) {
                        if (data.result !== null) {
                            searchType = parseInt($("#selectSearchPersonType").UifSelect('getSelected'), 10);
                            Persons.GetAplicationCompanyByDocument($('#inputCompanyDocumentNumber').val().trim(), searchType);

                            $('#modalPerson2G').UifModal('hide');
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result })
                        $('#modalPerson2G').UifModal('hide');
                    }
                });
            }
            else {
                $("#tblPerson2G").UifDataTable('unselect');
            }
        });

    }
}

function GetCompaniesByDocumentNumberByNameBySearchType(documentNumberSearch, nameSearch, searchType, showmsj) {
    PersonRequest.GetCompaniesByDocumentNumberByNameBySearchType(searchType, documentNumberSearch, nameSearch).done(function (data) {
        if (data.success) {
            if (data.result.length == 1) {
                HidePanelsPerson();
                $("#panelPersonLegal").show();
                $("#btnBottomMain").show();
                NewLegal();
                DisabledPanelsSearch(true);
                company = JSON.stringify(data.result[0]);
            }
            else {
                modalListType = 2;
                var dataList = { dataObject: [] };

                for (var i = 0; i < data.result.length; i++) {
                    dataList.dataObject.push({
                        Id: data.result[i].IndividualId,
                        Code: data.result[i].IdentificationDocument.Number,
                        Description: data.result[i].Name
                    });
                }
                ShowModalList(dataList.dataObject);
                $('#modalDialogListPerson').UifModal('showLocal', AppResourcesPerson.SelectCompany);
            }
        }
        else {
            PersonLegal.ShowCompany();
        }
    });

}