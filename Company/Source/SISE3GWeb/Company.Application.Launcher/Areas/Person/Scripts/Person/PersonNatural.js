var isNew = 0;
var individualId = -1;
var searchType = null;
var glbPersonOnline = null;
class PersonNatural extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        $('#InputDocumentNumber').ValidatorKey(ValidatorType.Number, 0, 0);
        $('#InputFirstNamePn').ValidatorKey(2, 0, 0);
        $('#InputLastNamePn').ValidatorKey(2, 0, 0);
        $('#InputNameIndividualPn').ValidatorKey(2, 0, 0);
        $('#InputBirthPlacePn').ValidatorKey(2, 3, 0);
        new StaffLabour();
        $("#inputDaneCodePn").UifAutoComplete({
            source: rootPath + "Person/Person/GetDaneCodeByQuery",
            displayKey: "DANECode",
            queryParameter: "&query"
        });
        this.InitializePersonNatural();
        Persons.AddSubtitlesRightBar();

        DocumentTypeRequest.GetDocumentType("1").done(function (data) {
            if (data.success) {
                $("#selectDocumentTypePn").UifSelect({ sourceData: data.result });
                $("#selectDocumentTypePnSearch").UifSelect({ sourceData: data.result });
            }
        });
        new Persons();
        if (glbPersonIndividualId != null) {
            Persons.GetPersonByIndividualId(glbPersonIndividualId);
            glbPersonIndividualId = null;
        }

        if (glbPersonOnline != null) {
            glbPersonOnline.RolType = 1;
            $("#InputDocumentNumber").val(glbPersonOnline.ViewModel.HolderName)
        }
        unlockScreen();
        $("#inputCheckPayableTo").val($("#InputNameIndividualPn").val() + ' ' + $("#InputFirstNamePn").val() + ' ' + $("#InputLastNamePn").val());
    }

    //Seccion Eventos
    bindEvents() {

        $("#inputDocumentPerson").focusout(this.SearchDocumentPerson);

        $("#InputDocumentNumber").focusout(this.DocumentNumberPn);

        $("#InputDocumentNumber").focusout(this.ValidateListRiskPerson);
        $("#InputFirstNamePn").focusout(this.ValidateListRiskPerson);
        $("#InputLastNamePn").focusout(this.ValidateListRiskPerson);
        $("#InputNameIndividualPn").focusout(this.ValidateListRiskPerson);

        $("#InputNameIndividualPn").focusout(this.CompanyTradeName);

        $("#btnNatAddresses").click(this.AddressesPn);

        $("#btnPhone").click(this.PhonePn);

        $("#btnNatEmail").click(this.EmailPn);

        // << EESGE-172 Control De Busqueda

        $('#inputCountryPn').on("search", this.SearchContryPn);

        $("#inputDaneCodePn").on('itemSelected', this.ChangeDaneCodePn);

        $('#inputStatePn').on("search", this.SearchStatePn);

        $('#inputCityPn').on("search", this.SearchCityPn);

        $('#tblResultListCountries tbody').on('click', 'tr', this.SelectSearchCountries);

        $('#tblResultListStates tbody').on('click', 'tr', this.SelectSearchStates);

        $('#tblResultListCities tbody').on('click', 'tr', this.SelectSearchCities);
        // EESGE-172 Control De Busqueda >>
        $('#selectDocumentTypePn').on('itemSelected', this.ChangeTypeAndNumberDocument);

        $('#tblPerson2G').on('rowSelected', this.SelectedRowModalPerson2G);

    }

    ValidateListRiskPerson() {
        let documentNumber = $("#InputDocumentNumber").val().trim();
        let firstName = $("#InputFirstNamePn").val().trim();
        let lastName = $("#InputLastNamePn").val().trim();
        let name = $("#InputNameIndividualPn").val().trim();

        let fullName = `${name} ${firstName} ${lastName}`

        PersonRequest.ValidateListRiskPerson(documentNumber, fullName).done(data => {
            if (data.success && data.result !== null) {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
    }

    BirthDatePnChange() {
        Shared.calculateAge("#InputAgePn", $("#InputbirthdatePn").val())
    }

    DocumentNumberPn() {
        if ($("#InputDocumentNumber").val() != "") {
            PersonRequest.GetPersonByDocumentNumberByNameByFirstLastName(searchType, $(this).val()).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.length >= 1) {
                            $.UifDialog('alert', { 'message': AppResourcesPerson.DocumentNumberAlreadyRegistered });
                            $("#InputDocumentNumber").val("");
                        }
                        else {
                            PersonRequest.GetPerson2gByDocumentNumber($("#InputDocumentNumber").val(), false).done(function (data) {
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
                }
            });
        }
    }

    ChangeTypeAndNumberDocument() {
        $('#InputDocumentNumber').val("");
        if ($("#selectDocumentTypePn").UifSelect("getSelectedSource").IsAlphanumeric) {
            $('#InputDocumentNumber').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        } else {
            $("#InputDocumentNumber").ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        }
    }

    SearchDocumentPerson() {
        if ($("#inputDocumentPerson").val() != '') {
            Persons.GetPersonsByDocumentNumberNameSearchType($("#inputDocumentPerson").val(), searchType);
        }
    }

    AddressesPn() {
        if (individualId != -1) {
            if (PersonNatural.ValidateAddressPersonNatural()) {
                $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
                Address.ClearAddress();
                Address.GetAddresses();
                Persons.ShowPanelsPerson(RolType.Address);
            }
        }
    }

    CompanyTradeName() {
        $("#inputCheckPayableTo").val($("#InputNameIndividualPn").val() + ' ' + $("#InputFirstNamePn").val() + ' ' + $("#InputLastNamePn").val());
    }

    PhonePn() {
        if (PersonNatural.ValidatePhonePersonNatural("", true)) {
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Phone.ClearPhone();
            Phone.GetPhonesses();
            Persons.ShowPanelsPerson(RolType.Phone);
        }
    }

    EmailPn() {
        if (Email.ValidateEmailPerson()) {
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Email.ClearEmail();
            Email.GetEmailsses();
            Persons.ShowPanelsPerson(RolType.Email);
        }
    }

    SearchContryPn(event, value) {
        $('#inputStatePn').val("");
        $('#inputCityPn').val("");
        $('#inputDaneCodePn').val("");
        if ($("#inputCountryPn").val().trim().length > 0) {
            PersonNatural.GetCountriesByDescription($("#inputCountryPn").val());
        }
    }

    ChangeDaneCodePn() {
        if ($('#inputCountryPn').data("Id") > 0) {
            PersonNatural.GetCountryAndStateAndCityByDaneCode($('#inputCountryPn').data("Id"), $("#inputDaneCodePn").val());
        }
    }

    SearchStatePn(event, value) {
        $('#inputCityPn').val("");
        $('#inputDaneCodePn').val("");
        if ($("#inputStatePn").val().trim().length > 0) {
            PersonNatural.GetStatesByCountryIdByDescription($("#inputCountryPn").data('Id'), $("#inputStatePn").val());
        }
    }

    SearchCityPn(event, value) {
        $('#inputDaneCodePn').val("");
        if ($("#inputCityPn").val().trim().length > 0) {
            PersonNatural.GetCitiesByCountryIdByStateIdByDescription($("#inputCountryPn").data('Id'), $("#inputStatePn").data('Id'), $("#inputCityPn").val());
        }
    }

    SelectSearchCountries() {
        var dataCountry = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputCountryPn").data(dataCountry);
        $("#inputCountryPn").val(dataCountry.Description);
        $('#modalListSearchCountries').UifModal('hide');
    }

    SelectSearchStates() {
        var dataState = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }
        $("#inputStatePn").data(dataState);
        $("#inputStatePn").val(dataState.Description);
        $('#modalListSearchStates').UifModal('hide');
        state = dataState;
    }

    SelectSearchCities() {
        var dataCities = {
            Id: $(this).children()[0].innerHTML,
            Description: $(this).children()[1].innerHTML
        }

        $("#inputCityPn").data(dataCities);
        $("#inputCityPn").val(dataCities.Description);
        $('#modalListSearchCities').UifModal('hide');
        city = dataCities;

        PersonNatural.LoadDaneCodePersonNatural();
    }


    //Seccion Funciones
    InitializePersonNatural() {
        isNew = 1;
        individualId = Person.New;
        searchType = TypePerson.PersonNatural;
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#InputAgePn").attr('disabled', 'disabled');
        $('.OnlyNumber').ValidatorKey(ValidatorType.Number);
        $("#inputEmailPn").ValidatorKey(ValidatorType.Emails, 0, 1);
        $('#inputAddressAllPn').ValidatorKey(ValidatorType.Addresses, 0, 1);
        $("#inputPhonePn").ValidatorKey(ValidatorType.PhonePn, 2, 1);
        PersonNatural.NewPerson();
    }

    //Seccion Grabado
    static RecordPerson(showAlert, Rguarantee, RstaffLabour) {

        if (showAlert == undefined) {
            showAlert = true;
        }
        var gender = "";
        if ($('#selectGender').UifSelect("getSelected") == GenderType.Male) {
            gender = 'M'
        }
        else {
            gender = 'F'
        }

        if ($('#lblPersonCode').val() == "") {
            individualId = Person.New;

        } else {
            individualId = $('#lblPersonCode').val();
        }
        var isExonerated = false;

        var SarlaftExoneration = [];
        SarlaftExoneration = {
            RolId: 1,

            EnteredDate: DateNowPerson,
            IsExonerated: isExonerated
        };
        var educativeLevelPersonalInformation = { Id: 0 };
        var childrenPersonalInformation;
        var houseTypeHouseTypePersonalInformation = { Id: 0 };
        var socialLayerPersonalInformation = { Id: 0 };
        var spouseNamePersonalInformation;
        var nationalPersonalInformation;
        var userId;
        var updateBy
        educativeLevelPersonalInformation = InformationPersonalGlobal.EducativeLevel;
        childrenPersonalInformation = InformationPersonalGlobal.Children;
        houseTypeHouseTypePersonalInformation = InformationPersonalGlobal.HouseType;
        socialLayerPersonalInformation = InformationPersonalGlobal.SocialLayer;
        spouseNamePersonalInformation = InformationPersonalGlobal.SpouseName;
        userId = InformationPersonalGlobal.UserId;
        updateBy = InformationPersonalGlobal.UpdateBy;

        var laborPersonPersonalInformation = { Occupation: { Id: 0 } };
        if (LaboralPersonalInformation.length > 0) {
            laborPersonPersonalInformation = LaboralPersonalInformation.LaborPerson;
        }
        if (addresses == null || addresses.length == 0) {
            Address.FillObjectForMainAddress();
        }
        else {
            Address.UpdatePrincipalAddress();
        }
        if (Phonesses == null || Phonesses.length == 0) {
            Phone.FillObjectPhone();
        }
        else {
            Phone.UpdatePrincipalPhone();
        }
        if (emails == null || emails.length == 0) {
            Email.FillObjectEmail();
        }
        else {
            Email.UpdatePrincipalEmail();
        }
        if (PaymentMethod == null || PaymentMethod.length == 0) {
            MethodPayment.FillObjectMethodPayment();
        }
        if (Sarlafs != null && Sarlafs.length > 0) {
            Sarlaft.UnFormatSarlaft();
        }
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

        var bitrtDate = null;
        var maritalStatusId = null;
        if ($("#selectMaritalStatusPn").UifSelect("getSelected") == "" || $("#selectMaritalStatusPn").UifSelect("getSelected") == null) {
            maritalStatusId = 7;
        }
        else {
            maritalStatusId = $('#selectMaritalStatusPn').UifSelect("getSelected");
        }
        if ($("#InputbirthdatePn").val() == "") {
            bitrtDate = FormatDate("01/01/1900");
        }
        else {
            bitrtDate = $('#InputbirthdatePn').UifSelect("getSelected");
        }

        var PersonData = {
            Id: individualId,
            Names: $('#InputNameIndividualPn').val(),
            Surname: $('#InputFirstNamePn').val(),
            SecondSurname: $('#InputLastNamePn').val(),
            DocumentTypeId: $('#selectDocumentTypePn').UifSelect("getSelected"),
            Document: $('#InputDocumentNumber').val(),
            Addresses: addresses,
            Phones: Phonesses,
            //Informacion laboral
            Gender: gender,
            MaritalStatusId: maritalStatusId,
            BirthDate: bitrtDate,
            BirthPlace: $('#InputBirthPlacePn').val(),
            CheckPayable: $('#inputCheckPayableTo').val(),
            EconomicActivityId: $("#inputeconomyActivityPn").data("Id"),
            DataProtection: $('#rdAceptadoPn').is(':checked'),
            Sarlaft: SarlafstTmp,
            Emails: emails,
            ExonerationTypeCode: null//$('#selectTypeExemption').UifSelect("getSelected"),
        };

        var PersonType;
        if (LaboralPersonalInformation.length > 0) {
            PersonType = LaboralPersonalInformation.Personal.PersonType;
        }

        lockScreen();
        PersonRequest.CreatePerson(PersonData).done(function (data) {
            if (data.success) {
                var dataPerson = data.result;
                var policyType = LaunchPolicies.ValidateInfringementPolicies(dataPerson.InfringementPolicies, true);
                let countAuthorization = dataPerson.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                    if (countAuthorization > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonGeneral);
                    }
                } else {
                    isNew = 0;
                    if (individualId == Person.New) {
                        individualId = data.result.Id;
                        $("#lblPersonCode").val(individualId);
                        $.UifDialog('confirm', { 'message': AppResourcesPerson.LabelPersonCode + data.result.Id + " " + AppResourcesPerson.LabelPersonCreate + " " + AppResourcesPerson.LabelInsuredCreate }, function (result) {
                            if (result) {
                                PersonNatural.ReturnRecord(dataPerson, Rguarantee, RstaffLabour);
                                addresses = data.result.Addresses;
                                Insured.loadInsured();
                            }
                            else {
                                PersonNatural.ReturnRecord(dataPerson, Rguarantee, RstaffLabour);
                            }
                        });
                    }
                    else {
                        PersonNatural.ReturnRecord(dataPerson, Rguarantee, RstaffLabour);
                        Provider.loadProviderPerson(data.result.Provider);
                        if (showAlert) {
                            $.UifDialog('alert', { 'message': AppResourcesPerson.LabelPersonCode + data.result.Id + " " + AppResourcesPerson.LabelPersonUpdate });
                        }
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
            unlockScreen();
        }).fail(() => unlockScreen());
    }


    static SearchDaneCodePn() {
        if ($('#inputCountryPn').data("Id") != null && $('#inputCountryPn').data("Id") > 0) {
            PersonNatural.GetCountryAndStateAndCityByDaneCode($('#inputCountryPn').data("Id"), $("#inputDaneCodePn").val());
        }

    }

    static ReturnRecord(data, Rguarantee, RstaffLabour) {
        if (typeof glbPolicy != "undefined" && glbPolicy != null && glbPersonOnline != null) {
            if (glbPersonOnline.RolType > 0) {
                glbPersonOnline.IndividualId = data.Id;
                glbPersonOnline.DocumentNumber = data.Document;
                glbPersonOnline.CustomerType = CustomerType.Individual;
            }
        }
        if (glbUser != null && glbUser.UserId != null) {
            glbPersonOnline.IndividualId = data.Id;
            glbPersonOnline.DocumentNumber = data.Document;
            router.run("prtUniqueUser");
        }

        if (Rguarantee) {
            var GuaranteeViewModel = {
                ContractorId: data.Id,
                ContractorNumber: $("#InputDocumentNumber").val(),
                ContractorName: $('#InputNameIndividualPn').val() + " " + $('#InputFirstNamePn').val() + " " + $('#InputLastNamePn').val(),
                searchType: TypePerson.PersonNatural,
                Address: $('#inputAddressAllPn').val(),
                ContractorDocumentType: $("#selectDocumentTypePn option:selected").text(),
                PhoneNumber: $("#inputPhonePn").val(),
                CityText: $("#inputCityPn").data("Description"),
                StateText: $("#inputStatePn").data("Description")
            }
            var codeInsured = -1;
            var individual = data.Id;
            InsuredRequest.GetInsuredByIndividualId(individual).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        codeInsured = data.result.InsuredId;
                        Persons.CreteInsuredGuarantee(codeInsured, individual, GuaranteeViewModel);
                    }
                }
                else {
                    Persons.CreteInsuredGuarantee(codeInsured, individual, GuaranteeViewModel);
                }
            });
        }
        else if (RstaffLabour) {
            PersonNatural.ClearControlPersonNatural();
            PersonNatural.SetPersonControl(data);
            Persons.ShowPanelsPerson(RolType.StaffLabour);
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
        }
        else {
            PersonNatural.ClearControlPersonNatural();
            PersonNatural.SetPersonControl(data);
        }
    }

    static GuaranteeModel(individualId) {
        var GuaranteeViewModel = {
            ContractorId: individualId,
            ContractorNumber: $("#InputDocumentNumber").val(),
            ContractorName: $('#InputNameIndividualPn').val() + " " + $('#InputFirstNamePn').val() + " " + $('#InputLastNamePn').val(),
            searchType: TypePerson.PersonNatural,
            Address: $('#inputAddressAllPn').val(),
            ContractorDocumentType: $("#selectDocumentTypePn option:selected").text(),
            PhoneNumber: $("#inputPhonePn").val(),
            CityText: $("#inputCityPn").data("Description"),
            StateText: $("#inputStatePn").data("Description")
        }
        var codeInsured = -1;
        guaranteeModel = GuaranteeViewModel;
        router.run("prtGuaranteeE");
    }

    //Validaciones
    static ValidatePerson() {
        var error = "";
        if ($("#selectDocumentTypePn").UifSelect("getSelected") == "" || $("#selectDocumentTypePn").UifSelect("getSelected") == null) {
            error = error + AppResourcesPerson.LabelTypeDocument + '<br>';
        }
        if ($("#selectGender").UifSelect("getSelected") == "" || $("#selectGender").UifSelect("getSelected") == null) {
            error = error + AppResourcesPerson.LabelGender + '<br>';
        }
        if ($("#InputDocumentNumber").val() == "") {
            error = error + AppResourcesPerson.LabelNumberDocument + "<br>";
        }
        if ($("#InputFirstNamePn").val() == "") {
            error = error + AppResourcesPerson.LabelFirstName + "<br>";
        }
        if ($("#InputNameIndividualPn").val() == "") {
            error = error + AppResourcesPerson.LabelName + "<br>";
        }
        //if ($("#selectMaritalStatusPn").UifSelect("getSelected") == "" || $("#selectMaritalStatusPn").UifSelect("getSelected") == null) {
        //    error = error + AppResourcesPerson.LabelMaritalStatus + "<br>";
        //}
        if ($("#selectCompanyAddressType").UifSelect("getSelected") == "" || $("#selectCompanyAddressType").UifSelect("getSelected") == null) {
            error = error + AppResourcesPerson.LabelTypeAddress + "<br>";
        }
        if ($("#inputAddressAllPn").val() == "") {
            error = error + AppResourcesPerson.LabelAddressAll + "<br>";
        }
        //if ($("#InputbirthdatePn").val() == "") {
        //    error = error + AppResourcesPerson.LabelBirthDate + "<br>";
        //}
        if ($("#inputStatePn").data("Id") == "" || $("#inputStatePn").data("Id") == null) {
            error = error + AppResourcesPerson.LabelDepartment + "<br>";

        }
        if ($("#inputCityPn").data("Id") == "" || $("#inputCityPn").data("Id") == null) {
            error = error + AppResourcesPerson.LabelCity + "<br>";
        }
        error = this.ValidatePhonePersonNatural(error, false);

        if ($("#selectEmailTypePn").UifSelect("getSelected") == "" || $("#selectEmailTypePn").UifSelect("getSelected") == null) {
            error = error + AppResourcesPerson.LabelEmailType + "<br>";
        }
        if ($("#inputEmailPn").val() == "") {
            error = error + AppResourcesPerson.Email + "<br>";
        }
        else {
            if (!ValidateEmail($('#inputEmailPn').val())) {
                error = error + AppResourcesPerson.ErrorEmail + "<br>";
            }

        }

        if ($("#inputEmailElectronicBilling").val() == "") {
            error = error + AppResourcesPerson.ElectronicBillingEmailLabel + "<br>";
        } else {
            if (!ValidateEmail($('#inputEmailElectronicBilling').val())) {
                error = error + AppResourcesPerson.ElectronicBillingEmailNoValid + "<br>";
            }

        }


        if ($("#inputeconomyActivityPn").val() == "" || $("#inputeconomyActivityPn").data("Id") == undefined) {
            error = error + AppResourcesPerson.LabelEconomyActivity + "<br>";

        }

        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }

    // DANECode
    static GetCountryAndStateAndCityByDaneCode(countryId, daneCode) {
        DaneCodeRequest.GetCountryAndStateAndCityByDaneCode(countryId, daneCode).done(function (data) {
            if (data.success) {
                if (data.result !== null && Object.keys(data.result).length > 0) {
                    $("#inputCountryPn").data({ Id: data.result.State.Country.Id, Description: data.result.State.Country.Description });
                    $("#inputStatePn").data({ Id: data.result.State.Id, Description: data.result.State.Description });
                    $("#inputCityPn").data({ Id: data.result.Id, Description: data.result.Description });
                    $("#inputCountryPn").val($("#inputCountryPn").data("Description"));
                    $("#inputStatePn").val($("#inputStatePn").data("Description"));
                    $("#inputCityPn").val($("#inputCityPn").data("Description"));
                    state = $("#inputCityPn").data();
                    city = $("#inputCityPn").data();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static NewPerson() {
        searchType = TypePerson.PersonNatural;
        individualId = Person.New;
        isNew = 1;
        PersonNatural.ClearControlPersonNatural();
        StaffLabour.ClearStaffLabor();
        Employee.clearInputEmployee();
        Insured.initializeControlInsured();
        insuredTmp = { Id: 0 };
        $("#btnBottomMain").show();
        $("#panelPersonNatural").show();
        $("#selectDocumentTypePn").removeAttr('disabled');
        $("#InputDocumentNumber").removeAttr('disabled');
        $("#InputFirstNamePn").removeAttr('disabled');
        $("#InputLastNamePn").removeAttr('disabled');
        $("#InputNameIndividualPn").removeAttr('disabled');
        $('#rdPrincipalPn').prop('checked', true);
        $('#rdPrincipalPn').prop('disabled', true);
        $('#rdAceptadoPn').prop('checked', true);
      
        if (defaultValues != null) {
            Persons.SetDefaultValues(defaultValues);
        }
        else {
            Persons.GetDefaultValues();
        }
        Persons.AddSubtitlesRightBar();
        $("#selectSearchPersonType").UifSelect("setSelected", searchType);
    }

    static ClearControlPersonNatural() {
        $("#selectDocumentTypePn").UifSelect("setSelected", null);
        $("#selectMaritalStatusPn").UifSelect("setSelected", null);
        $("#selectCompanyAddressType").UifSelect("setSelected", null);
        $("#inputStatePn").val("");
        $("#inputCityPn").val("");
        $("#selectPhoneTypePn").UifSelect("setSelected", null);
        $("#selectEmailTypePn").UifSelect("setSelected", null);
        $("#selectGender").UifSelect("setSelected", null);
        $("#InputDocumentNumber").val('');
        $("#lblPersonCode").val('');
        $('#optionsGenderManPn').attr('checked', true);
        $("#InputFirstNamePn").val('');
        $("#InputLastNamePn").val('');
        $("#InputNameIndividualPn").val('');
        $("#InputbirthdatePn").val('');
        $("#InputAgePn").val('');
        $("#InputBirthPlacePn").val('');
        $("#inputAddressAllPn").val('');
        $('#rdPrincipalPn').prop('checked', true);
        $('#rdPrincipalPn').prop('disabled', true);
        $("#inputPhonePn").val('');
        $("#inputEmailPn").val('');
        $("#inputCheckPayableTo").val("");
        $('#inputDaneCodePn').val('');
        $('#inputeconomyActivityPn').val('');
        $('#inputEmailElectronicBilling').val('');
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
        StaffLabour.ClearStaffLabor();
        PersonTax.clearTax();
        Provider.clearProvider();
        Persons.ClearPanelPeople();
        $('#checkThird').prop('checked', false);
        $('#checkThird').removeClass('primary');
        $('#checkEmployee').prop('checked', false);
        $('#checkEmployee').removeClass('primary');
        // Se inicializa y setea el país por defecto   
        Persons.GetdefaultValueCountry();
        
    }

    static GetCountriesByDescription(description) {
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
    static GetStatesByCountryIdByDescription(countryId, description) {
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
    static GetCitiesByCountryIdByStateIdByDescription(countryId, stateId, description) {
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

    static LoadDaneCodePersonNatural() {
        if ($("#inputCountryPn").data("Id") != null && $("#inputCountryPn").data("Id") > 0) {
            DaneCodeRequest.GetDaneCodeByCountryIdByStateIdByCityId($("#inputCountryPn").data("Id"), $("#inputStatePn").data("Id"), $("#inputCityPn").data("Id")).done(function (data) {
                if (data.success) {
                    $('#inputDaneCodePn').val("");
                    $('#inputDaneCodePn').val(data.result);
                    PersonNatural.SearchDaneCodePn();

                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }

    }

    static ValidateAddressPersonNatural() {
        var error = "";
        if ($("#selectCompanyAddressType").UifSelect("getSelected") == "" || $("#selectCompanyAddressType").UifSelect("getSelected") == null) {
            error = error + AppResourcesPerson.LabelTypeAddress + "<br>";
        }
        if ($("#inputAddressAllPn").val() == "") {
            error = error + AppResourcesPerson.LabelAddressAll + "<br>";
        }
        if ($("#inputStatePn").data("Id") == "" || $("#inputStatePn").data("Id") == null) {
            error = error + AppResourcesPerson.LabelDepartment + "<br>";

        }
        if ($("#inputCityPn").data("Id") == "" || $("#inputCityPn").data("Id") == null) {
            error = error + AppResourcesPerson.LabelCity + "<br>";
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }

    static SetPersonControl(person) {

        searchType = TypePerson.PersonNatural;
        $.uif2.helpers.setGlobalTitle(person.Names + " " + person.Surname + " -CC: " + person.Document);

        $('#AgentCheckPayableTo').val(person.Names);
        if (document.getElementById("selectSearchPersonType") != null) {
            $("#selectSearchPersonType").UifSelect("setSelected", searchType);
        }
        $("#inputDocument").val(person.Document);
        PersonNatural.DisabledPersonControl(true);
        $("#selectDocumentTypePn").UifSelect("setSelected", person.DocumentTypeId);
        $("#InputDocumentNumber").val(person.Document);
        $("#lblPersonCode").val(person.Id);
        individualId = person.Id;
        $("#CodPersonId").text(person.Id);

        if (person.Gender == 'M') {
            $('#selectGender').UifSelect("setSelected", GenderType.Male);
        }
        else if (person.Gender == 'F') {
            $('#selectGender').UifSelect("setSelected", GenderType.Female);
        }
        $("#InputFirstNamePn").val(person.Surname);
        $("#InputLastNamePn").val(person.SecondSurname);
        $("#InputNameIndividualPn").val(person.Names);

        if ($("#inputCheckPayableTo").val(person.CheckPayable) === null || $("#inputCheckPayableTo").val(person.CheckPayable) === undefined || $("#inputCheckPayableTo").val(person.CheckPayable) === "") {
            $("#inputCheckPayableTo").val((person.Names) + ' ' + (person.Surname) + ' ' + ((person.SecondSurname == null ? "" : "")).trimEnd());

        } else {
            $("#inputCheckPayableTo").val(person.CheckPayable);
        }
        $("#selectMaritalStatusPn").UifSelect("setSelected", person.MaritalStatusId);
        $('#rdAceptadoPn').prop('checked', false);
        if (person.DataProtection) {
            $('#rdAceptadoPn').prop('checked', true);
        }
        if (FormatDate(person.BirthDate) > FormatDate("01/01/1900")) {
            $("#InputbirthdatePn").val(FormatFullDate(person.BirthDate, true));
            Shared.calculateAge("#InputAgePn", person.BirthDate);
        }
        $("#InputBirthPlacePn").val(person.BirthPlace);
        if (person.PersonStateType != null) {
            if ($("#selectPersonIndividualType").length > 0) {
                $("#selectPersonIndividualType").UifSelect("setSelected", person.PersonStateType);
            }
        }


        InformationPersonalGlobal.SpouseName = person.SpouseName;
        InformationPersonalGlobal.EducativeLevel = person.EducativeLevel;
        InformationPersonalGlobal.PersonType = person.PersonType == null ? "" : person.PersonType.PersonTypeCode;
        InformationPersonalGlobal.Children = person.Children;
        InformationPersonalGlobal.HouseType = person.HouseType;
        InformationPersonalGlobal.SocialLayer = person.SocialLayer;
        InformationPersonalGlobal.UpdateBy = person.UpdateBy;
        InformationPersonalGlobal.UserId = person.Id;

        $("#UpdateBy").val(person.UpdateBy);
        $("#LastUpdate").val(FormatDate(person.LastUpdate), 1);

        $("#inputSpouseName").val(person.SpouseName);
        $("#inputNchildren").val(person.Children);
        if ($("#selectStratum").length > 0) {
            $("#selectStratum").UifSelect("setSelected", person.SocialLayer);
        }
        if ($("#selectLevelEducation").length > 0) {
            $("#selectLevelEducation").UifSelect("setSelected", person.EducativeLevel);
        }
        if ($("#selectTypeHouse").length > 0) {
            $("#selectTypeHouse").UifSelect("setSelected", person.HouseType);
        }

        if (economicActivity.Id == person.EconomicActivityId && person.EconomicActivityDescription == null) {
            $("#inputeconomyActivityPn").data("Id", person.EconomicActivityId);
            $("#inputeconomyActivityPn").val(person.EconomicActivityId + ' (' + economicActivity.Description + ')');
        }
        else {
            $("#inputeconomyActivityPn").data("Id", person.EconomicActivityId);
            $("#inputeconomyActivityPn").val(person.EconomicActivityId + ' (' + person.EconomicActivityDescription + ')');
            economicActivity = { Id: person.EconomicActivityId, Description: person.EconomicActivityDescription };
        }

        if (person.Addresses != null) {
            addresses = person.Addresses;
            Address.LoadAddress(person.Addresses);
            Address.setPrincipalAddress(person.Addresses);
            PersonNatural.LoadDaneCodePersonNatural($("#inputCityPn").data("Id"));

        }
        else {
            Persons.GetdefaultValueCountry();
        }
        if (person.Phones != null && person.Phones.length > 0) {
            Phonesses = person.Phones;
            $('#selectPhoneTypePn').val(person.Phones[0].PhoneTypeId);
            $('#inputPhonePn').val(person.Phones[0].Description);
        }
        if (person.Emails != null && person.Emails.length > 0) {
            emails = person.Emails;
            $.each(person.Emails, function (index, value) {
                if (value.EmailTypeId == 23) {
                    $('#inputEmailElectronicBilling').val(value.Description);
                } else {
                    if (value.IsPrincipal || value.IsMailingAddress) {
                        $('#selectEmailTypePn').val(value.EmailTypeId);
                        $('#inputEmailPn').val(value.Description);
                    }
                }
            });

        }

        if (person.LaborPerson != null) {
            InformationPersonalGlobal.LaborPerson = person.LaborPerson;
            $("#selectIncomeLevel").UifSelect("setSelected", person.LaborPerson.IncomeLevel.Id);
            $("#selectOccupation").UifSelect("setSelected", person.LaborPerson.Occupation.Id);
            $("#inputNameCompany").val(person.LaborPerson.CompanyName);
            $("#selectOccupation option:selected").text(person.LaborPerson.OccupationCode),
                $("#inputSector").val(person.LaborPerson.JobSector);
            $("#inputPhoneConpany").val(person.LaborPerson.Phone);
            $("#selectSpecialty").UifSelect("setSelected", person.LaborPerson.Speciality.Id);
            $("#selectOtherOccupation").UifSelect("setSelected", person.LaborPerson.OtherOccupation.Id);
            $("#inputPhoneConpany").val(person.LaborPerson.CompanyPhone.Id);
            $("#inputofifce").val(person.LaborPerson.Position);
            $("#inputContactCompany").val(person.LaborPerson.Contact);
            StaffLabour.GetPersonInterestGroups($("#lblPersonCode").val());
            OccupationRequest.GetOccupations().done(function (data) {
                if (data.success) {
                    if ($("#selectOccupation").UifSelect("getSelected") == "") {
                        $("#selectOccupation").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectOccupation").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.Occupation.Id });
                    }
                    if ($("#selectOtherOccupation").UifSelect("getSelected") == "") {
                        $("#selectOtherOccupation").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectOtherOccupation").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.OtherOccupation.Id });
                    }
                }
            });

            SpecialtyRequest.GetSpecialties().done(function (data) {
                if (data.success) {
                    if ($("#selectSpecialty").UifSelect("getSelected") == "") {
                        $("#selectSpecialty").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectSpecialty").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.Speciality.Id });
                    }
                }
            });

            IncomeLevelRequest.GetIncomeLevels().done(function (data) {
                if (data.success) {
                    if ($("#selectIncomeLevel").UifSelect("getSelected") == "") {
                        $("#selectIncomeLevel").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectIncomeLevel").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.IncomeLevel.Id });
                    }
                }
            });
        }

        if (person.LaborPerson != null) {
            InformationPersonalGlobal.LaborPerson = person.LaborPerson;
            $("#selectIncomeLevel").UifSelect("setSelected", person.LaborPerson.IncomeLevel.Id);
            $("#selectOccupation").UifSelect("setSelected", person.LaborPerson.Occupation.Id);
            $("#inputNameCompany").val(person.LaborPerson.CompanyName);
            $("#selectOccupation option:selected").text(person.LaborPerson.OccupationCode),
                $("#inputSector").val(person.LaborPerson.JobSector);
            $("#inputPhoneConpany").val(person.LaborPerson.Phone);
            $("#selectSpecialty").UifSelect("setSelected", person.LaborPerson.Speciality.Id);
            $("#selectOtherOccupation").UifSelect("setSelected", person.LaborPerson.OtherOccupation.Id);
            $("#inputPhoneConpany").val(person.LaborPerson.CompanyPhone.Id);
            $("#inputofifce").val(person.LaborPerson.Position);
            $("#inputContactCompany").val(person.LaborPerson.Contact);
            StaffLabour.GetPersonInterestGroups($("#lblPersonCode").val());
            OccupationRequest.GetOccupations().done(function (data) {
                if (data.success) {
                    if ($("#selectOccupation").UifSelect("getSelected") == "") {
                        $("#selectOccupation").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectOccupation").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.Occupation.Id });
                    }
                    if ($("#selectOtherOccupation").UifSelect("getSelected") == "") {
                        $("#selectOtherOccupation").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectOtherOccupation").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.OtherOccupation.Id });
                    }
                }
            });

            SpecialtyRequest.GetSpecialties().done(function (data) {
                if (data.success) {
                    if ($("#selectSpecialty").UifSelect("getSelected") == "") {
                        $("#selectSpecialty").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectSpecialty").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.Speciality.Id });
                    }
                }
            });

            IncomeLevelRequest.GetIncomeLevels().done(function (data) {
                if (data.success) {
                    if ($("#selectIncomeLevel").UifSelect("getSelected") == "") {
                        $("#selectIncomeLevel").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectIncomeLevel").UifSelect({ sourceData: data.result, selectedId: person.LaborPerson.IncomeLevel.Id });
                    }
                }
            });
        }

        //if (person.ExonerationTypeCode != null && person.ExonerationTypeCode > 0) {
        //    $("#selectTypeExemption").UifSelect("setSelected", person.ExonerationTypeCode);
        //}

        if (person.Sarlaft != undefined && person.Sarlaft != null && Array.isArray(person.Sarlaft)) {
            Sarlaft.getSarlaft(person.Sarlafs);
        }
        Persons.ValidateRolPerson(individualId);
    }

    static DisabledPersonControl(control) {
        $("#selectDocumentTypePn").prop('disabled', control);
        $("#InputDocumentNumber").prop('disabled', control);
        //$("#InputFirstNamePn").prop('disabled', control);
        //$("#InputLastNamePn").prop('disabled', control);
        //$("#InputNameIndividualPn").attr('disabled', control);
    }

    static GetCountriesPerson(city) {
        if (city != null) {
            var dataCountry = {
                Id: city.State.Country.Id,
                Description: city.State.Country.Description
            }
            $("#inputCountryPn").data(dataCountry);
            $("#inputCountryPn").val(dataCountry.Description);

            var dataState = {
                Id: city.State.Id,
                Description: city.State.Description
            }
            $("#inputStatePn").data(dataState);
            $("#inputStatePn").val(dataState.Description);
            state = dataState;

            if (city.State.Id != 0) {
                var dataCity = {
                    Id: city.Id,
                    Description: city.Description,
                    DANECode: city.DANECode
                }
                $("#inputCityPn").data(dataCity);
                $("#inputCityPn").val(dataCity.Description);
                city = dataCity;

                $('#inputDaneCodePn').UifAutoComplete('setValue', city.DANECode);
            }
        }
    }

    static ValidatePhonePersonNatural(error, notify) {
        if ($("#listPhonesses").UifListView('getData').length === 0) {
            if ($("#selectPhoneTypePn").UifSelect("getSelected") == "" || $("#selectPhoneTypePn").UifSelect("getSelected") == null) {
                error = error + AppResourcesPerson.LabelPhoneType + "<br>";
            }
            if ($("#inputPhonePn").val() == "") {
                error = error + AppResourcesPerson.LabelPhone + "<br>";
            }
        }
        if (error != "" && notify) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + error })
            return false;
        } else if (!notify) {
            return error;
        } else {
            return true;
        }
    }


    SelectedRowModalPerson2G(event, data, position) {
        $.UifDialog('confirm', { 'message': AppResourcesPerson.Person2GMigrateSure }, function (result) {
            if (result) {
                var personId = data.PersonId;
                PersonRequest.GetPerson2gByPersonId(personId, false).done(function (data) {
                    if (data.success) {
                        if (data.result !== null) {
                            searchType = parseInt($("#selectSearchPersonType").UifSelect('getSelected'), 10);
                            Persons.GetPersonsByDocumentNumberNameSearchType($('#InputDocumentNumber').val().trim(), searchType);

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
