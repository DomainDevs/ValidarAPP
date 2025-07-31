var InformationPersonalGlobal = {
    PersonType: { PersonTypeCode: "" }, EducativeLevel: { Id: "" }, Children: 0, HouseType: { Id: "" }, SocialLayer: { Id: "" }, SpouseName: "", Nationality: "",
    UserId: "", UpdateBy: "", PersonCode: "",
    LaborPerson: { Occupation: { Id: "" }, Speciality: { Id: "" }, OtherOccupation: { Id: "" }, IncomeLevel: { Id: "" }, CompanyName: "", JobSector: "", Position: "", CompanyPhone: { Id: "" }, Contact: "" },
    PersonInterestGroup: [],
    IndividualId: -1
};
var modalListType = 0;
var defaultValues = null;
var parameters = null;
var countryParameter;
var person = {};
var company = {};
var insuredTmp = { Id: 0 };
var prospect = {};
var guaranteeModel = null;
var economicActivity = {};
var state = {};
var city = {};
var ConsortiumEventParticipant = [];
var notificationSearch = null;
var addressTypes = null;
var emailTypes = null;
var RollAgent = null;
var RollCoinsured = null;
var RollEmployee = null;
var RollInsured = null;
var RollReinsurer = null;
var RollSupplier = null;
var RollThird = null;

class Persons extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        PersonRequest.GetPersonTypes().done(function (data) {
            if (data.success) {
                $("#selectSearchPersonType").UifSelect({ sourceData: data.result });
                $("#selectSearchPersonType").UifSelect("setSelected", searchType);
            }
        });

        if (searchType != null) {
            new Insured();
            new Agent();
            new Provider();
            new CoInsurer();
            new ReInsurer();
            new AditionalDataPn();
            new PersonTax();
            new Shared();
            new Address();
            new Phone();
            new Email();
            new Sarlaft();
            new Third();
            new Employee();
            new BankTransfers();
            new ElectronicBilling();
            Persons.showPanelButtonsRight();
            Shared.GetCurrentDate();

            PersonRequest.LoadInitialData(false).done(function (data) {
                if (data.success) {
                    addressTypes = data.result.AddressTypes;
                    emailTypes = data.result.EmailTypes;
                    $("#selectGender").UifSelect({ sourceData: data.result.GenderTypes });
                    $("#selectGenderProspect").UifSelect({ sourceData: data.result.GenderTypes });

                    $("#selectAddress").UifSelect({ sourceData: data.result.AddressTypesbyEmail });

                    $("#selectMaritalStatusPn").UifSelect({ sourceData: data.result.MaritalStatus });
                    $("#selectMaritalStatusPrn").UifSelect({ sourceData: data.result.MaritalStatus });

                    $("#selectTypeAddressPn").UifSelect({ sourceData: data.result.AddressTypes });
                    $("#selectCompanyAddressType").UifSelect({ sourceData: data.result.AddressTypes });
                    $("#selectAddressPll").UifSelect({ sourceData: data.result.AddressTypes });
                    $("#selectAddressPl1").UifSelect({ sourceData: data.result.AddressTypes });

                    $("#agentDeclinedTypes").UifSelect({ sourceData: data.result.AgentDeclinedTypes });
                    $("#selectReasonLow").UifSelect({ sourceData: data.result.AgentDeclinedTypes });

                    $("#OperatingQuota-Currency").UifSelect({ sourceData: data.result.Currencies });
                    $("#selectCurrencyRepresentLegal").UifSelect({ sourceData: data.result.Currencies });

                    $("#selectPhoneTypePn").UifSelect({ sourceData: data.result.PhoneTypes });
                    $("#selectCompanyPhoneType").UifSelect({ sourceData: data.result.PhoneTypes });
                    $("#selectPhoneType").UifSelect({ sourceData: data.result.PhoneTypes });

                    $("#selectEmailTypePn").UifSelect({ sourceData: data.result.EmailTypes });
                    $("#selectEmailType").UifSelect({ sourceData: data.result.EmailTypes });
                    $("#selectCompanyEmailType").UifSelect({ sourceData: data.result.EmailTypes });

                    $("#selectTypeExemption").UifSelect({ sourceData: data.result.ExonerationTypes });
                }
            });

            if (parameters == null) {
                ParameterRequest.GetParameters().done(function (data) {
                    if (data.success) {
                        parameters = data.result;
                        Persons.GetdefaultValueCountry();
                        Persons.GetdefaultValueCurrency();
                    }
                });
            }
            else {
                Persons.GetdefaultValueCountry();
                Persons.GetdefaultValueCurrency();
            }

            var individual = Persons.getQueryVariable("IndividualId");
            $("#individualId").val(individual);
            if (individual != null && individual != '') {
                individualId = individual;
            }
            if (individualId != 0 && individualId != -1) {
                Persons.LoadPerson(individualId);
            }

            var individualNotification = Persons.getQueryVariable("NotificationIndividualId");
            $("#individualId").val(individualNotification);
            if (individualNotification != null && individualNotification != '' && individualNotification != 0
                && individualNotification != -1 && notificationSearch == null) {
                individualId = individualNotification;
                PersonRequest.GetNotificationByIndividualId(individualNotification).done(function (data) {
                    if (data.success) {
                        if (data.result.PersonType == IndividualTypePerson.Natural) {
                            router.run("prtPersonNatural");
                            glbPersonIndividualId = individualNotification;
                            notificationSearch = true;
                        }

                        if (data.result.PersonType == IndividualTypePerson.Legal) {
                            router.run("prtPersonLegal");
                            glbPersonIndividualId = individualNotification;
                            notificationSearch = true;
                        }
                    }
                });
            }
            Persons.GetDefaultValues();
        }
        Persons.initializeInformationPersonalGlobal();
        Persons.getEnumsRoles();

        PrefixRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $("#OperatingQuota-LineBusinessCd").UifSelect({ sourceData: data.result });
            }
        });
    }

    static getEnumsRoles() {
        PersonRequest.GetEnumsRoles().done(function (data) {
            if (data.success) {
                RollAgent = data.result.Agent;
                RollCoinsured = data.result.Coinsured;
                RollEmployee = data.result.Employee;
                RollInsured = data.result.Insured;
                RollReinsurer = data.result.Reinsurer;
                RollSupplier = data.result.Supplier;
                RollThird = data.result.Third
            }
        });
    }

    //Seccion Eventos
    bindEvents() {
        $('#selectSearchPersonType').on('itemSelected', this.ChangeTypePersonDefault);
        //Botones de navegación a las vistas parciales 
        $("#btnSarlaft").click(this.BtnSarlaft);
        $("#btnStaffLabour").click(this.BtnStaffLabour);
        $("#btnPaymentMethods").click(this.PaymentMethods);
        $("#btnRepresentLegal").click(this.BtnRepresentlegal);
        $("#btnPartner").click(this.BtnPartner);
        $("#btnConsortiumMembers").click(this.BtnConsortiumMembers);
        $("#btnTradeName").click(this.TradeName);
        $("#btnTax").click(this.btnTax);
        $("#btnBankTransfers").click(this.BankTransfers);
        $("#btnElectronicBilling").click(this.ElectronicBilling);
        $("#btnCancelView").click(this.CancelView);
        $('#InputbirthdatePn').on("datepicker.change", this.BirthdatePn);
        //Buscar actividad economica PJ        
        $('#inputCompanyEconomyActivity').on('buttonClick', this.SearchEconomyActivity);
        //Buscar actividad economica PN
        $('#inputeconomyActivityPn').on('buttonClick', this.SearchEconomyActivityPN);
        //Grabar o actualizar individuo
        $("#btnSavePerson").click(Persons.SavePerson);
        /*Modal persona*/
        $("#btnNewPerson").click(this.ShowModalPerson);

        $("#btnAcceptNewPerson").click(this.AcceptNewPerson);

        $('#inputAgentPrincipal').on('buttonClick', this.SearchAgencies);

        $("#btnOperatingQuota").click(this.BtnOperatingQuota);

        //Buscar persona
        $('#inputDocument').on('buttonClick', this.SearchDocument);

        $("#btnGuarantee").click(this.Guarantee);

        $('#tableResultsPerson tbody').on('click', 'tr', this.SelectSearchPerson);

        $('#tableResultsAll tbody').on('click', 'tr', this.SelectSearchAll);

        $("#btnSearchAdvPerson").on("click", this.ShowSearchAdvPerson);

        $('#InputbirthdatePn').focusout(this.BirthdatePn);


    }

    static GetdefaultValueCurrency() {
        if (parameters != null) {
            var pCurrency = parameters.find(Persons.GetParameterByDescription, ['Currency']);
            if (pCurrency != undefined) {
                $("#OperatingQuota-Currency").UifSelect("setSelected", pCurrency.Value);
                $("#selectCurrencyRepresentLegal").UifSelect("setSelected", pCurrency.Value);
            }
        }
    }

    static getQueryVariable(variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) {
                return pair[1];
            }
        }
    }

    static GetdefaultValueCountry() {
        if (parameters != null) {
            var pCountry = parameters.find(Persons.GetParameterByDescription, ['Country']);
            if (pCountry != undefined) {
                var dataCountry = {
                    Id: pCountry.Value,
                    Description: pCountry.TextParameter
                }
                countryParameter = dataCountry.Id;
                $("#inputCountryPn").data(dataCountry);
                $("#inputCountryPn").val(dataCountry.Description);
                $("#inputCountryInfoPn").data(dataCountry);
                $("#inputCountryInfoPn").val(dataCountry.Description);
                $("#inputCountryAdPn").data(dataCountry);
                $("#inputCountryAdPn").val(dataCountry.Description);
                $("#inputCompanyCountry").data(dataCountry);
                $("#inputCompanyCountry").val(dataCountry.Description);
                $("#inputCompanyCountryOrigin").data(dataCountry);
                $("#inputCompanyCountryOrigin").val(dataCountry.Description);
                $("#inputCountryRepresentLegal").data(dataCountry);
                $("#inputCountryRepresentLegal").val(dataCountry.Description);
                $("#inputCountryPl1").data(dataCountry);
                $("#inputCountryPl1").val(dataCountry.Description);
                $("#inputCountryPll1").data(dataCountry);
                $("#inputCountryPll1").val(dataCountry.Description);
            }
        }
    }

    static GetParameterByDescription(parameter) {
        return parameter.Description == this[0];
    }

    static GetDefaultValues() {
        PersonRequest.GetDefaultValues().done(function (data) {
            if (data.success) {
                defaultValues = data.result;
                Persons.SetDefaultValues(data.result);
            }
        });
    }

    static SetDefaultValues(values) {
        $.each(values, function (index, value) {
            switch (this.ControlType) {
                case ControlType.Select:
                    if (document.getElementById(this.ControlName) != null) {
                        if ($("#" + this.ControlName + " option").length > 0) {
                            eval("$('#" + this.ControlName + "')." + this.ControlType + "('setSelected'," + this.ControlValue + ")")
                        }
                        else {
                            var controlName = this.ControlName;
                            var value = this.ControlValue;
                            $("#" + controlName).on("binded", function () {
                                $("#" + controlName).UifSelect("setSelected", value);
                            });
                        }
                    }
                    break;
                case ControlType.Datepicker:
                    if (document.getElementById(this.ControlName) != null) {
                        eval("$('#" + this.ControlName + "')." + this.ControlType + "('setValue'," + this.ControlValue + ")");
                    }
                    break;
                case ControlType.Input:
                    if (document.getElementById(this.ControlName) != null) {
                        eval("$('#" + this.ControlName + "').val('" + this.ControlValue + "')");
                    }
                    break;
                default:
                    break;
            }
        });
    }

    CancelView() {
        if (glbPolicy == null) {
            window.location = rootPath + "Home/Index";
        }
        else {
            if (glbPolicy.TemporalType == TemporalType.Quotation) {
                glbPersonOnline = null;
                router.run("prtQuotation");
            }
            else {
                glbPersonOnline = null;
                router.run("prtTemporal");
            }
        }
    }

    AcceptNewPerson() {
        Person.Insured = null;
        Person.ConsortiumMembers = null;
        $('#modalNewPerson').UifModal('hide');
        $("#inputDocument").val("");
        Person.ConsortiumMembers = [];
        if ($('input:radio[name=searchType]:checked').val() == "naturalPerson") {
            if (searchType != TypePerson.PersonNatural) {
                router.run("prtPersonNatural");
            }
            else {

                PersonNatural.NewPerson();
            }
        }

        if ($('input:radio[name=searchType]:checked').val() == "legalPerson") {
            if (searchType != TypePerson.PersonLegal) {
                router.run("prtPersonLegal");

            }
            else {
                PersonLegal.NewLegal();
            }
        }

        if ($('input:radio[name=searchType]:checked').val() == "naturalprospectus") {
            if (searchType != TypePerson.ProspectNatural) {
                router.run("prtProspectusNatural");
            }
            else {
                ProspectusNatural.NewProspectNatural();
            }
        }

        if ($('input:radio[name=searchType]:checked').val() == "legalprospectus") {
            if (searchType != TypePerson.ProspectLegal) {
                router.run("prtProspectusLegal");
            }
            else {
                ProspectusLegal.NewProspectLegal();
            }
        }
        SarlafstTmp = null;
    }

    static GetValidateSarlaft() {



    }

    static SavePerson() {
        var valid = "";
        switch (searchType) {
            case TypePerson.PersonNatural:
                //Grabado persona Naturales
                if (isAuthorize['0095']) {
                    if (PersonNatural.ValidatePerson()) {
                        if (individualId > 1) {
                            var SarlaftNew = {};
                            SarlaftNew.IndividualId = individualId;
                            SarlaftNew.ActivityEconomic = $("#inputCompanyEconomyActivity").data("Id");
                            Sarlaft.LoadSarlaft(SarlaftNew).then(function () {
                                if (Persons.validateSarlaftPerson()) {
                                    $('#btnSavePerson').attr("disabled", true);
                                    PersonNatural.RecordPerson(true);
                                    $('#btnSavePerson').attr("disabled", false);
                                }
                            });
                        }
                        else {
                            if (Persons.validateSarlaftPerson()) {
                                $('#btnSavePerson').attr("disabled", true);
                                PersonNatural.RecordPerson(true);
                            }
                        }
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.IsAuthorizePersonNatural });
                }
                break;
            case TypePerson.PersonLegal:
                valid = PersonLegal.ValidateCompany();
                if (isAuthorize['0150']) {
                    if (valid == "") {
                        if (individualId > 1) {
                            var SarlaftNew = {};
                            SarlaftNew.IndividualId = individualId;
                            SarlaftNew.ActivityEconomic = $("#inputCompanyEconomyActivity").data("Id");
                            Sarlaft.LoadSarlaft(SarlaftNew).then(function () {
                                if (Persons.validateSarlaftCompany()) {
                                    PersonLegal.RecordCompany(true);
                                }
                            });
                        }
                        else {
                            if (Persons.validateSarlaftCompany()) {
                                PersonLegal.RecordCompany(true);
                            }
                        }
                    }
                    else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + valid
                        })
                        if (valid == AppResourcesPerson.ValidateTypeAssociation) {
                            $('#btnSavePerson').attr("disabled", true);
                            ProspectusLegal.RecordProspectLegal();
                        }
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.IsAuthorizePersonLegal });
                }
                break;
            case TypePerson.ProspectNatural:
                if (isAuthorize['0153']) {
                    if (ProspectusNatural.validateProspectNatural()) {
                        $('#btnSavePerson').attr("disabled", true);
                        ProspectusNatural.RecordProspectNatural();
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.IsAuthorizeProspectNatural });
                }
                break;
            case TypePerson.ProspectLegal:
                if (isAuthorize['0156']) {
                    if (ProspectusLegal.validateProspectLegal()) {
                        $('#btnSavePerson').attr("disabled", true);
                        ProspectusLegal.RecordProspectLegal();
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.IsAuthorizeProspectLegal });
                }
                break;

            default:
                break;
        }
        $('#btnSavePerson').attr("disabled", false);
    }

    ChangeTypePersonDefault() {
        if ($("#selectSearchPersonType").UifSelect("getSelected") == "") {
            searchType = null
            router.run("prtPerson");
        }
        else {
            var typePersonDefault = parseInt($("#selectSearchPersonType").UifSelect("getSelected"));
            Persons.setTypePersonDefault(typePersonDefault);
        }
    }

    static setTypePersonDefault(typePersonDefault) {

        if (searchType != typePersonDefault) {
            searchType = typePersonDefault;
            Persons.initializeInformationPersonalGlobal();
            switch (typePersonDefault) {
                case TypePerson.PersonNatural:
                    if (isAuthorize['0095']) {
                        $('#btnSavePerson').hide()
                    } else {
                        $('#btnSavePerson').show()
                    }
                    router.run("prtPersonNatural");
                    break;
                case TypePerson.PersonLegal:
                    if (isAuthorize['0150']) {
                        $('#btnSavePerson').hide()
                    } else {
                        $('#btnSavePerson').show()
                    }
                    router.run("prtPersonLegal");
                    break;
                case TypePerson.ProspectNatural:
                    if (isAuthorize['0153']) {
                        $('#btnSavePerson').hide()
                    } else {
                        $('#btnSavePerson').show()
                    }
                    router.run("prtProspectusNatural");
                    break;
                case TypePerson.ProspectLegal:
                    if (isAuthorize['0156']) {
                        $('#btnSavePerson').hide()
                    } else {
                        $('#btnSavePerson').show()
                    }
                    router.run("prtProspectusLegal");
                    break;
                default:
                    break;
            }
        }
    }

    BtnSarlaft() {

        $.UifProgress('show');

        if (individualId == -1 || individualId == "" || individualId == undefined) {
            Sarlaft.getSarlaft(SarlafstTmp || []);
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Persons.ShowPanelsPerson(RolType.Sarlaft);
        } else {
            var SarlaftNew = {};
            SarlaftNew.IndividualId = individualId;
            SarlaftNew.ActivityEconomic = $("#inputCompanyEconomyActivity").data("Id");
            Sarlaft.LoadSarlaft(SarlaftNew);
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Persons.ShowPanelsPerson(RolType.Sarlaft);
        }
        $.UifProgress('close');
    }

    BtnStaffLabour() {
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            if (individualId != Person.New) {
                Persons.ShowPanelsPerson(RolType.StaffLabour);
                var invidualId = $('#lblPersonCode').val();
                $.UifProgress('show');
                LabourPersonRequest.GetLabourPersonByIndividualId(invidualId);
                $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            }
            else {
                var valid = "";
                if (searchType == TypePerson.PersonNatural) {
                    if (PersonNatural.ValidatePerson()) {
                        if (Persons.validateSarlaftPerson()) {
                            PersonNatural.RecordPerson(false, false, true);
                        }
                    }
                }
                else if (searchType == TypePerson.PersonLegal) {
                    valid = PersonLegal.ValidateCompany();
                    if (valid == "") {
                        PersonLegal.RecordCompany(false, false, true);
                    }
                    else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + valid
                        })
                    }
                }
            }
        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
    }

    ShowModalPerson() {
        Persons.ShowPanelsPerson(RolType.People);
    }

    static ShowPanelsPerson(rolType) {

        switch (rolType) {
            case RolType.People:
                $('#modalNewPerson').UifModal('showLocal', AppResourcesPerson.LabelPersonNew);
                break;
            case RolType.Address:
                $("#modalAddress").UifModal("showLocal", AppResourcesPerson.LabelAddresses);
                break;
            case RolType.Email:
                $("#modalEmail").UifModal("showLocal", AppResourcesPerson.Email);
                break;
            case RolType.Agent:
                $("#modalformAgent").UifModal('showLocal', AppResourcesPerson.MessageAgents);
                break;
            case RolType.CoInsurer:
                $("#modalformCoInsurer").UifModal('showLocal', AppResourcesPerson.MessageCoInsurer);
                break;
            case RolType.ReInsurer:
                $("#modalformReInsurer").UifModal('showLocal', AppResourcesPerson.MessageReInsurer);
                break;
            case RolType.Insured:
                $("#modalformInsured").UifModal('showLocal', AppResourcesPerson.MessageInsured);
                break;
            case RolType.PaymentMetod:
                $("#modalMeansPayment").UifModal('showLocal', AppResourcesPerson.MessageMehodPayment);
                break;
            case RolType.Partner:
                $("#modalPartNer").UifModal('showLocal', AppResourcesPerson.TitlePartner);
                break;
            case RolType.Sarlaft:
                $("#modalSarlaft").UifModal("showLocal", AppResourcesPerson.LabelSarlaft);
                break;
            case RolType.StaffLabour:
                $("#modalStaffLabour").UifModal("showLocal", AppResourcesPerson.InformationStaffLabour);
                break;
            case RolType.LegalRepresent:
                $("#modalRepresentlegal").UifModal("showLocal", AppResourcesPerson.Representlegal);
                $("#modalPeps").UifModal("showLocal", AppResourcesPerson.Representlegal);
                break;
            case RolType.TypeConsortiumMembers:
                $("#modalConsortiumMembers").UifModal("showLocal", AppResourcesPerson.MessageConsortiums);
                break;
            case RolType.BusinessName:
                $("#modalBusinessName").UifModal("showLocal", AppResourcesPerson.LabelBusinessName);
                break;
            case RolType.OperatingQuota:
                $("#modalOperatingQuota").UifModal("showLocal", AppResourcesPerson.LabelOperatingQuota);
                break;
            case RolType.Phone:
                $("#modalPhone").UifModal('showLocal', AppResourcesPerson.Phones);
                break;
            case RolType.Provider:
                $("#modalProvider").UifModal('showLocal', AppResourcesPerson.Provider);
                break;
            case RolType.Tax:
                $("#modalTax").UifModal('showLocal', AppResourcesPerson.Taxesperindividual);
                break;
            case RolType.AdditionalData:
                $("#modalAdditionalData").UifModal('showLocal', AppResourcesPerson.AdditionalInformation);
                break;
            case RolType.Third:
                $("#modalThird").UifModal('showLocal', AppResourcesPerson.Third);
                break;
            case RolType.Employee:
                $("#modalEmployee").UifModal('showLocal', AppResourcesPerson.Employee);
                break;
            case RolType.BankTranferens:
                $("#modalBankTransfers").UifModal('showLocal', AppResourcesPerson.BankTranferens);
                break;
            case RolType.ElectronicBilling:
                $("#modalElectronicBilling").UifModal('showLocal', AppResourcesPerson.ElectronicBilling);
                break;
            default:
                break;
        }
    }

    PaymentMethods() {
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            new MethodPayment();
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            MethodPayment.ClearControlMethodPayment(true);
            MethodPayment.GetPaymentMethod();
            Persons.ShowPanelsPerson(RolType.PaymentMetod);
        } else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }

    }

    BtnRepresentlegal() {

        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            $('#tabsRepresenLegal a:first').tab('show');
            if (individualId != Person.New || isNew == 1) {
                if (representLegalTmp == null) {
                    // Se inicializa y setea el país por defecto   
                    Persons.GetdefaultValueCountry();
                    Persons.GetdefaultValueCurrency();
                    RepresentLegal.ClearRepresentLegal();
                    RepresentLegal.LoadLegalRepresentative();
                }
                Persons.ShowPanelsPerson(RolType.LegalRepresent);
            }
            else {
                $.UifNotify('show', { 'type': 'error', 'message': AppResourcesPerson.MustExistIndividual, 'autoclose': true });
                return false;
            }
        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }

    }

    BtnPartner() {
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            DocumentTypeRequest.GetDocumentType("3").done(function (data) {
                if (data.success) {
                    $("#selectDocumentPartner").UifSelect({ sourceData: data.result });
                }
            });
            var PartnerNew = {};
            PartnerNew.IndividualId = $('#lblCompanyCode').val();
            $.UifProgress('show');
            PartnerRequest.GetAplicationPartnerByIndividualId(PartnerNew, "3");
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Persons.ShowPanelsPerson(RolType.Partner);
        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
    }

    BtnConsortiumMembers() {
        ConsortiumEventParticipant = Person.ConsortiumMembers
        ConsortiumInsuredCode = 0;
        var individualId = $('#lblCompanyCode').val();
        if (Person.Insured == null || Person.Insured.InsureCode < 1) {
            $.UifDialog('alert', { 'message': AppResourcesPerson.InsuredRequired });
            return false;
        }
        else {
            if ($("#selectTypePartnership").UifSelect("getSelected") != TypePartnership.Individual && $("#selectTypePartnership").UifSelect("getSelected") != "") {
                $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
                if (individualId != Person.New || isNew == 1) {
                    ConsortiumMember.clearConsortiumMembers();
                    if (individualId == "") {
                        ConsortiumMember.LoadConsortiumMembers(Person.ConsortiumMembers || []);
                        Persons.ShowPanelsPerson(RolType.TypeConsortiumMembers);
                    } else {
                        ConsortiumMember.LoadConsortium(individualId);
                        Persons.ShowPanelsPerson(RolType.TypeConsortiumMembers);
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.MessageIndividual });
                    return false;
                }
            } else {
                $.UifDialog('alert', { 'message': AppResourcesPerson.MessageConsortiumMembers });
                return false;
            }
        }
    }

    BankTransfers() {
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            if (individualId != Person.New) {
                Persons.ShowPanelsPerson(RolType.BankTranferens);
            }
            else {
                var valid = "";
                if (searchType == TypePerson.PersonNatural) {
                    if (PersonNatural.ValidatePerson()) {
                        if (Persons.validateSarlaftPerson()) {
                            PersonNatural.RecordPerson(false);
                            Persons.ShowPanelsPerson(RolType.BankTranferens);
                        }
                    }
                }
                else if (searchType == TypePerson.PersonLegal) {
                    valid = PersonLegal.ValidateCompany();
                    if (valid == "") {
                        PersonLegal.RecordCompany(false);
                        Persons.ShowPanelsPerson(RolType.BankTranferens);
                        OperatingQuota.clearObjectsOperatingQuota();
                        OperatingQuota.LoadOperatingQuota(individualId);
                    }
                    else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + valid
                        })
                    }
                }
                return false;
            }
        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }

    }


    ElectronicBilling() {
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            InsuredRequest.GetInsuredByIndividualId(individualId).done(function (data) {
                if (data.success) {

                    if (individualId != Person.New) {
                        Persons.ShowPanelsPerson(RolType.ElectronicBilling);
                    }
                    else {
                        var valid = "";
                        if (searchType == TypePerson.PersonNatural) {
                            if (PersonNatural.ValidatePerson()) {
                                if (Persons.validateSarlaftPerson()) {
                                    PersonNatural.RecordPerson(false);
                                    Persons.ShowPanelsPerson(RolType.ElectronicBilling);
                                    ElectronicBilling.ClearListControlElectronicBilling();
                                }
                            }
                        }
                        else if (searchType == TypePerson.PersonLegal) {
                            valid = PersonLegal.ValidateCompany();
                            if (valid == "") {
                                PersonLegal.RecordCompany(false);
                                Persons.ShowPanelsPerson(RolType.ElectronicBilling);
                                ElectronicBilling.ClearListControlElectronicBilling();

                            }
                            else {
                                $.UifNotify('show', {
                                    'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + valid
                                })
                            }
                        }
                        return false;
                    }
                } else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.PersonIsNotInsured });
                    return false;
                }
            });

        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }

    }



    btnTax() {
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            PersonTax.loadDates();
            TaxRequest.GetTax().done(function (data) {
                if (data.success) {
                    $("#selectTax").UifSelect({ sourceData: data.result });
                }
            });
            TaxRequest.GetCompanyIndividualRoleByIndividualId(individualId).done(function (data) {
                if (data.success) {
                    $("#selectRole").UifSelect({ sourceData: data.result });
                }
            });
            TaxRequest.GetStatesByStateTaxId($('#inputCountryPn').data("Id") || $('#inputCompanyCountry').data("Id")).done(function (data) {
                if (data.success) {
                    $("#StateCode").UifSelect({ sourceData: data.result });
                }
            });
            if (individualId != -1) {

                TaxRequest.GetIndividualTaxExeptionByIndividualId(individualId).done(function (data) {
                    if (data.success) {

                        for (var i = 0; i < data.result.length; i++) {
                            data.result[i].Datefrom = FormatDate(data.result[i].Datefrom);
                            data.result[i].DateUntil = FormatDate(data.result[i].DateUntil);
                            data.result[i].OfficialBulletinDate = FormatDate(data.result[i].OfficialBulletinDate);
                        }

                        $("#listVTax").UifListView({
                            sourceData: data.result,
                            height: 300,
                            displayTemplate: '#display-tax',
                            edit: true,
                            customEdit: true,
                            delete: true,
                            deleteCallback: PersonTax.deleteCallback
                        });
                    }
                });
            }

            if (individualId != Person.New) {
                Persons.ShowPanelsPerson(RolType.Tax);
            }
            else {
                var valid = "";
                if (searchType == TypePerson.PersonNatural) {
                    if (PersonNatural.ValidatePerson()) {
                        if (Persons.validateSarlaftPerson()) {
                            PersonNatural.RecordPerson(false);
                            Persons.ShowPanelsPerson(RolType.Tax);
                        }
                    }

                }
                else if (searchType == TypePerson.PersonLegal) {
                    valid = PersonLegal.ValidateCompany();
                    if (valid == "") {
                        PersonLegal.RecordCompany(false);
                        Persons.ShowPanelsPerson(RolType.Tax);
                    }
                    else {
                        $.UifNotify('show',
                            {
                                'type': 'info',
                                'message': AppResourcesPerson.LabelInformative + ":<br>" + valid
                            });
                    }
                }
                return false;
            }
        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
    }

    TradeName() {
        if (PersonLegal.ValidateAddressPersonLegal()) {
            var listBusinessName = [];
            Address.GetAddresses();
            Phone.GetPhonesses();
            Email.GetEmailsses();
            BusinessName.LoadAddresPhoneEmail();
            BusinessName.ClearBusinessName();

            var promise = [];
            if (addresses != addressesGet || Phonesses != PhonessesGet || emails != emailsGet) {
                promise.push(PersonLegal.RecordCompany(false));
            }

            Promise.all(promise).then(result => {

                if (individualId != -1) {
                    BusinessNameRequest.GetCompanyBusinessByIndividualId(individualId).done(function (data) {

                        if (data.success) {
                            data.result.forEach(function (element) {

                                let tempListBusinessName = { Address: { AddressType: {}, City: {} }, Email: { EmailType: {} }, Phone: { PhoneType: {} } };
                                tempListBusinessName.IndividualId = element.IndividualId;
                                (element.AddressID == 0) ? tempListBusinessName.AddressID = 1 : tempListBusinessName.AddressID = element.AddressID;
                                (element.PhoneID == 0) ? tempListBusinessName.PhoneID = 1 : tempListBusinessName.PhoneID = element.PhoneID;
                                (element.EmailID == 0) ? tempListBusinessName.EmailID = 1 : tempListBusinessName.EmailID = element.EmailID;
                                tempListBusinessName.NameNum = element.NameNum;
                                tempListBusinessName.TradeName = element.TradeName;
                                tempListBusinessName.Enabled = element.Enabled;
                                tempListBusinessName.IsMain = element.IsMain;
                                if (element.AddressID > 0) {
                                    addresses.find(function (elementAddress) {
                                        if (elementAddress.Id == element.AddressID) {
                                            tempListBusinessName.Address.Description = elementAddress.Description;
                                            tempListBusinessName.Address.City.Description = elementAddress.CityDescription;
                                        }
                                    });
                                    tempListBusinessName.Address.AddressType.Description = addressTypes.find(function (elementAddress) { return elementAddress.Id == addresses.find(function (elementAddress) { return elementAddress.Id == element.AddressID }).AddressTypeId }).Description; //addresses.find(function (elementAddress) { return elementAddress.Id == element.AddressID }).Description;
                                }

                                if (element.PhoneID > 0) {
                                    Phonesses.find(function (elementPhone) {
                                        if (elementPhone.Id == element.PhoneID) {
                                            tempListBusinessName.Phone.PhoneType.Description = elementPhone.PhoneType.Description;
                                            tempListBusinessName.Phone.Description = elementPhone.Description
                                        }
                                    });
                                }

                                if (element.EmailID > 0) {
                                    emailTypes.find(function (elementEmailType) {
                                        emails.find(function (elementEmail) {
                                            if (elementEmailType.Id == elementEmail.EmailTypeId && elementEmail.Id == element.EmailID) {
                                                tempListBusinessName.Email.EmailType.Description = elementEmailType.Description;
                                                tempListBusinessName.Email.Description = elementEmail.Description;
                                            }
                                        });
                                    });
                                }
                                listBusinessName.push(tempListBusinessName);
                                BusinessName.CleanObjectsBusinessName();
                                BusinessName.LoadAddresPhoneEmail();
                                BusinessName.LoadBusinessName(listBusinessName);


                            });
                        }
                    });
                }
                else {
                    BusinessName.LoadBusinessName();
                    BusinessName.LoadAddresPhoneEmail();
                }

                Persons.ShowPanelsPerson(RolType.BusinessName);
            });
        }
    }

    BirthdatePn() {
        var date = $('#InputbirthdatePn').val().split(DateSplit);
        if (!(isExpirationDate(date))) {
            if ($("#InputbirthdatePn").val() != '') {
                var fecha = $("#InputbirthdatePn").val();
                var age = Shared.calculateAge("#InputAgePn", fecha);
                if (age > 117) {
                    $("#InputbirthdatePn").UifDatepicker("clear");
                    $("#InputAgePn").val("");
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.WarningAgeMax })
                }
            }
        }
        else {
            $("#InputbirthdatePn").UifDatepicker("clear");
            $("#InputAgePn").val("");
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.NoCanDateBirthGreaterCurrent + "<br>" })
        }
    }

    SearchEconomyActivity() {
        if ($('#inputCompanyEconomyActivity').val().trim() != "" && $('#inputCompanyEconomyActivity').val().trim() != 0) {
            Persons.GetEconomyActivity($('#inputCompanyEconomyActivity').val().trim());
        }
    }

    SearchEconomyActivityPN() {
        if ($('#inputeconomyActivityPn').val().trim() != "" && $('#inputeconomyActivityPn').val().trim() != 0) {
            Persons.GetEconomyActivityPerson($('#inputeconomyActivityPn').val());
        }
    }

    SearchAgencies() {
        Persons.GetAgenciesByAgentIdDescription(0, $('#inputAgentPrincipal').val().trim());
    }

    BtnOperatingQuota() {
        new OperatingQuota();
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {

            if ($("#selectTypePartnership").val() == undefined || $("#selectTypePartnership").val() == 1) {
                $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
                if (individualId != Person.New) {
                    Persons.ShowPanelsPerson(RolType.OperatingQuota);
                    OperatingQuota.clearObjectsOperatingQuota();
                    OperatingQuota.LoadOperatingQuota(individualId);
                }
                else {
                    var valid = "";
                    if (searchType == TypePerson.PersonNatural) {
                        if (PersonNatural.ValidatePerson()) {
                            if (Persons.validateSarlaftPerson()) {
                                PersonNatural.RecordPerson(false);
                                Persons.ShowPanelsPerson(RolType.OperatingQuota);
                                OperatingQuota.clearObjectsOperatingQuota();
                                OperatingQuota.LoadOperatingQuota(individualId);
                            }
                        }

                    }
                    else if (searchType == TypePerson.PersonLegal) {
                        valid = PersonLegal.ValidateCompany();
                        if (valid == "") {
                            PersonLegal.RecordCompany(false);
                            Persons.ShowPanelsPerson(RolType.OperatingQuota);
                            OperatingQuota.clearObjectsOperatingQuota();
                            OperatingQuota.LoadOperatingQuota(individualId);
                        }
                        else {
                            $.UifNotify('show', {
                                'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + valid
                            })
                        }
                    }
                    return false;
                }
            }
            else {
                $.UifDialog('alert', { 'message': AppResourcesPerson.MessageOperationQuotaConsortium });
            }

        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
    }

    SearchDocument() {
        individualId = null;
        insuredTmp = null;
        Provider.clearProvider();
        $('#inputDocumentbutton').prop('disabled', 'disabled');

        if ($("#selectSearchPersonType").UifSelect("getSelected") == "" || $("#selectSearchPersonType").UifSelect("getSelected") == null) {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResourcesPerson.SelectTypePerson
            })
        }
        else {
            if ($('#inputDocument').val().trim() != "") {
                searchType = parseInt($("#selectSearchPersonType").UifSelect('getSelected'), 10);
                if (searchType == TypePerson.PersonNatural) {
                    Persons.GetPersonsByDocumentNumberNameSearchType($('#inputDocument').val().trim(), searchType);
                }
                if (searchType == TypePerson.PersonLegal) {
                    Persons.GetAplicationCompanyByDocument($('#inputDocument').val().trim(), searchType);
                }
                if (searchType == TypePerson.ProspectNatural) {
                    modalListType = 5;
                    Persons.GetProspectByDocumentNumSearchType($('#inputDocument').val().trim(), searchType);
                }
                if (searchType == TypePerson.ProspectLegal) {
                    modalListType = 6;
                    Persons.GetProspectByDocumentNumSearchType($('#inputDocument').val().trim(), searchType);
                }

                $('#inputDocumentbutton').removeAttr('disabled');
            }
        }
    }

    Guarantee() {
        if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
            var valid = "";
            $.UifProgress('show');
            InsuredRequest.GetInsuredByIndividualId(individualId).done(function (data) {
                $.UifProgress('close');
                if (data.success) {
                    if (data.result.Id != null) {
                        if (searchType == TypePerson.PersonNatural) {
                            PersonNatural.GuaranteeModel(individualId);
                        }
                        else if (searchType == TypePerson.PersonLegal) {
                            valid = PersonLegal.ValidateCompany();
                            if (valid == "") {
                                PersonLegal.GuaranteeModel(individualId);
                            }
                            else {
                                $.UifNotify('show', {
                                    'type': 'info', 'message': AppResourcesPerson.LabelInformative + " :<br>" + valid
                                })
                            }
                        }
                    } else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': data.result
                        })
                    }
                }
                else {
                    $.UifNotify('show', {
                        'type': 'info', 'message': data.result
                    })
                }
            }).fail(function (jqXHR, textStatus) {
                $.UifProgress('close');
            });
        }
        else {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
    }

    SelectSearchPerson() {
        switch (modalListType) {
            case 1:
                Persons.GetPersonByIndividualId($(this).children()[0].innerHTML);
                break;
            case 2:
                Persons.GetCompanyByIndividualId($(this).children()[0].innerHTML);
                break;
            case 3:
                $('#inputCompanyEconomyActivity').val($(this).children()[2].innerHTML + ' (' + $(this).children()[0].innerHTML + ')');
                $("#inputCompanyEconomyActivity").data("Id", $(this).children()[0].innerHTML);
                $('#inputeconomyActivityPn').val($(this).children()[2].innerHTML + ' (' + $(this).children()[0].innerHTML + ')');
                $("#inputeconomyActivityPn").data("Id", $(this).children()[0].innerHTML);
                economicActivity = { Id: $(this).children()[0].innerHTML, Description: $("#inputeconomyActivityPn").val() }
                break;
            case 5:
                ShowPanelsPerson(RolType.prospectNatural);
                break;
            case 6:
                ShowPanelsPerson(RolType.prospectLegal);
                break;
            case 7:
                GetCompanyParnertshipByIndividualId($(this).children()[0].innerHTML);
                break;
            default:
                break;
        }
        $('#modalDialogListPerson').UifModal("hide");
    }

    SelectSearchAll() {
        switch (modalListType) {
            case ModalListTypePerson.Holder:
                var individualId = $(this).children()[0].innerHTML;
                var type = InsuredSearchType.IndividualId;
                ConsortiumMember.GetHoldersByIndividualId(individualId, type);
                break;
            default:
                break;
        }
        $('#modalDialogListAll').UifModal("hide");
    }


    ShowSearchAdvPerson() {
        AdvancedSearch.clearFieldAdv();
        var typePersonDefault = parseInt($("#selectSearchPersonType").UifSelect("getSelected"));

        $("#listviewSearchPerson").UifListView(
            {
                displayTemplate: "#searchNaturalTemplate",
                selectionType: 'single',
                source: null,
                height: 180
            });
        dropDownSearch.show();
        $("#panelSearchPerson").show();
        $("#panelSearchCompany").hide();
        $("#selectTypePerson").UifSelect("setSelected", typePersonDefault);
        Persons.setTypePersonDefault(typePersonDefault);
        AdvancedSearch.TypePersonSelected();
    }

    //Seccion Validaciones
    static validateSarlaftPerson() {
        var msjSarlaft = "";
        if ($("#selectTypeExemption").UifSelect("getSelected") == "") {
            if ($('#listSarlaft').UifListView('getData').length == 0) {
                msjSarlaft = AppResourcesPerson.PersonWithoutExonerationSarlaftRequired;
            }
        }

        if (msjSarlaft != "") {
            $.UifDialog('alert', {
                'message': msjSarlaft
            });
            return false;
        }
        return true;
    }

    static GetEconomyActivityPerson(description) {
        EconomicActivityRequest.GetEconomicActivitiesByDescription(description).done(function (data) {
            if (data.success) {
                if (data.result.length === 0) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ActivityEconomicNotExist, 'autoclose': true });
                }
                else if (data.result.length == 1) {
                    $("#inputeconomyActivityPn").val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                    $("#inputeconomyActivityPn").data("Id", data.result[0].Id)
                    economicActivity = { Id: data.result[0].Id, Description: $("#inputeconomyActivityPn").val() }
                }
                else {
                    modalListType = 3;
                    var dataList = {
                        dataObject: []
                    };

                    for (var i = 0; i < data.result.length; i++) {
                        dataList.dataObject.push({
                            Id: data.result[i].Id,
                            Code: data.result[i].Id,
                            Description: data.result[i].Description
                        });
                    }
                    Persons.ShowModalList(dataList.dataObject);
                    $('#modalDialogListPerson').UifModal('showLocal', AppResourcesPerson.SelectEconomicActivity);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                $("#inputeconomyActivityPn").val('');
                $("#inputeconomyActivityPn").focus();
            }
        });
    }

    static GetEconomyActivity(description) {
        EconomicActivityRequest.GetEconomicActivitiesByDescription(description).done(function (data) {
            if (data.success && data.result.length > 0) {
                if (data.result.length == 1) {
                    $("#inputCompanyEconomyActivity").val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                    $("#inputCompanyEconomyActivity").data("Id", data.result[0].Id)
                }
                else {
                    modalListType = 3;
                    var dataList = {
                        dataObject: []
                    };

                    for (var i = 0; i < data.result.length; i++) {
                        dataList.dataObject.push({
                            Id: data.result[i].Id,
                            Code: data.result[i].Id,
                            Description: data.result[i].Description
                        });
                    }
                    Persons.ShowModalList(dataList.dataObject);
                    $('#modalDialogListPerson').UifModal('showLocal', AppResourcesPerson.SelectEconomicActivity);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                $("#inputCompanyEconomyActivity").val('');
                $("#inputCompanyEconomyActivity").focus();
            }
        });
    }

    static ShowModalList(dataTable) {
        $('#tableResultsPerson').UifDataTable('clear');
        var table = $('#tableResultsPerson').DataTable();
        table.on('draw', function () {
            $('#tableResults tbody td:nth-child(1)').hide();
            $('#tableResults thead th:eq(0)').hide();
        });
        $('#tableResultsPerson').UifDataTable('addRow', dataTable);
    }

    static initializeInformationPersonalGlobal() {

        InformationPersonalGlobal = {

            PersonType: { PersonTypeCode: "" },
            EducativeLevel: { Id: "" },
            Children: 0,
            HouseType: { Id: "" },
            SocialLayer: { Id: "" },
            SpouseName: "",
            Nationality: "",
            UpdateBy: "",
            UserId: "",
            PersonCode: ""
            ,
            LaborPerson: {
                Occupation: { Id: "" },
                Speciality: { Id: "" },
                OtherOccupation: { Id: "" },
                IncomeLevel: { Id: "" },
                CompanyName: "",
                JobSector: "",
                Position: "",
                CompanyPhone: { Id: "" },
                Contact: ""
            },
            PersonInterestGroup: [],
            IndividualId: -1
        };
        Address.CleanObjectAddress();
        if (glbPersonOnline != null) {
            $("#typeSearch").hide();
            $("#documentSearch").hide();
            $('#titleMain').toggleClass('column-40 column');
            $('#columnMain').toggleClass('uif-col-9 uif-col-12');
            $("#personsRol").hide();
            $("#personsMenu").hide();
            $("#titleSearch").hide();
            $("#btnNewPerson").hide();
            Persons.setDataOnlinePerson();
        }
        $("#individualId").val("");
        $('#inputDocument').val('');
    }

    static PeripheralTitle() {
        switch (searchType) {
            case TypePerson.PersonNatural:
                var Name = $("#InputNameIndividualPn").val();
                var PrimerApellido = $("#InputFirstNamePn").val();
                var NumDucPersonaNatura = $("#InputDocumentNumber").val();
                return Name + " " + PrimerApellido + " -CC: " + NumDucPersonaNatura;
                break;
            case TypePerson.PersonLegal:
                var tradeName = $("#inputCompanyTradeName").val();
                var documentNumber = $("#inputCompanyDocumentNumber").val().trim();
                var documentCompanyType = $("#selectCompanyDocumentType option:selected").text();
                return tradeName + " -" + documentCompanyType + ": " + documentNumber;
                break;
            default:
                break;
        }
    }

    static showPanelButtonsRight() {
        Persons.HidePanelButtonsRight();
        $("#panelButtonsRight").show();
        if (searchType == TypePerson.PersonNatural) {
            $("#btnStaffLabour").closest("li").show();
        }
        else if (searchType == TypePerson.PersonLegal) {
            $("#btnRepresentLegal").closest("li").show();
            $("#btnPartner").closest("li").show();
            $("#btnConsortiumMembers").closest("li").show();
            $("#btnTradeName").closest("li").show();
            $("#btnBankTransfers").closest("li").show();
            $("#btnElectronicBilling").closest("li").show();
        }
    }

    static HidePanelButtonsRight() {
        $("#btnStaffLabour").closest("li").hide();
        $("#btnRepresentLegal").closest("li").hide();
        $("#btnPartner").closest("li").hide();
        $("#btnConsortiumMembers").closest("li").hide();
        $("#btnTradeName").closest("li").hide();
    }


    static GetPersonsByDocumentNumberNameSearchType(DocumentNumber, searchTypeP) {
        PersonRequest.GetPersonByDocumentNumberByNameByFirstLastName(searchTypeP, DocumentNumber, $("#inputSurname").val(), $("#inputSecondsurname").val(), $("#inputNames").val()).done(function (msg) {
            if (msg.success) {
                if (msg.result.length == 0) {
                    Persons.SetDataPersonProspect(DocumentNumber);
                }
                else {
                    searchType: TypePerson.PersonNatural;
                    if (msg.result.IsAuthorizationRequest) {
                        Persons.ValidateAuthorizationRequest(msg.result, TypePerson.PersonNatural);
                    }
                    else {
                        if (msg.result.length == 1) {
                            var person = msg.result[0];
                            PersonNatural.ClearControlPersonNatural();
                            PersonNatural.SetPersonControl(person);
                            MethodPayment.GetPaymentMethod();
                            Persons.AddSubtitlesRightBar();
                        }
                        else if (msg.result.length > 1) {
                            Persons.ResultSeachList(DocumentNumber, msg);
                        }
                        if (msg.result.length != 0)
                            PersonRequest.GetTypeRolByIndividual(msg.result[0].Id).done(function (data) {
                                if (data.success) {
                                    Persons.ResetRolPerson();
                                    $.each(data.result, function (key, value) {
                                        Persons.ResultSeachRolPerson(value.RoleId);
                                    });
                                }
                            });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': msg.result, 'autoclose': true });
            }
        });
    }

    static AddSubtitlesRightBar() {
        $("#btnSarlaft").siblings("p").remove();
        if ($('#listSarlaft').UifListView('getData').length == 0) {
            $("#btnSarlaft").after("<p>(Sin datos)</p>")
        } else {
            var last = $('#listSarlaft').UifListView('getData').length - 1;
            var data = $('#listSarlaft').UifListView('getData');
            var value = data[last].Id;
            var index = data[last].RegistrationDate.indexOf('>') + 1;
            value += " - " + FormatDate(data[last].RegistrationDate.substring(index));
            $("#btnSarlaft").after("<p>(" + value + ")</p>")
        }
        /*informacion personal*/
        if (searchType == TypePerson.PersonNatural) {
            if ($("#lblPersonCode").val() != "") {
                $("#btnStaffLabour").siblings("p").remove();
            }
        }

        $("#btnPaymentMethods").siblings("p").remove();
        if ($('#listMeansPayment').UifListView('getData').length == 0) {
            $("#btnPaymentMethods").after("<p>(Sin datos)</p>");
        } else {
            var data = $('#listMeansPayment').UifListView('getData').length;
            $("#btnPaymentMethods").after("<p>(" + data + ")</p>");
        }

        $("#btnRepresentLegal").siblings("p").remove();
        if (representLegalTmp == null) {
            $("#btnRepresentLegal").after("<p>(Sin datos)</p>");
        } else {
            $("#btnRepresentLegal").after("<p>(Varios)</p>");
        }
    }

    static DisabledPanelsSearch(control) {
        $("#inputDocumentPerson").prop('disabled', control);
        $("#inputSurname").prop('disabled', control);
        $("#inputSecondsurname").prop('disabled', control);
        $("#inputNames").prop('disabled', control);
        $("#inputTradeName").prop('disabled', control);
        $("#inputAllDocument").prop('disabled', control);
        $("#inputAllSurname").prop('disabled', control);
        $("#inputAllSecondsurname").prop('disabled', control);
        $("#inputAllNames").prop('disabled', control);
        $("#inputAllTradeName").prop('disabled', control);
    }

    static ClearPanelPeople() {
        $('#checkInsured').prop('checked', false);
        $('#checkInsured').removeClass('primary');
        $('#checkProvider').prop('checked', false);
        $('#checkProvider').removeClass('primary');
        $('#checkAgent').prop('checked', false);
        $('#checkAgent').removeClass('primary');
        $('#checkReinsurer').prop('checked', false);
        $('#checkReinsurer').removeClass('primary');
        $('#checkCoInsurer').prop('checked', false);
        $('#checkCoInsurer').removeClass('primary');
    }

    static ShowPanelPeople() {
        Persons.ClearPanelPeople();
    }

    static validateSarlaftCompany() {
        var msjSarlaft = "";
        if ($("#selectTypeExemption").UifSelect("getSelected") == "") {
            if ($('#listSarlaft').UifListView('getData').length == 0) {
                if ($("#InputIncome").val() == "" && $("#InputExpenses").val() == "" && $("#InputTotalAssets").val() == "" && $("#InputTotalLiabilities").val() == "") {
                    msjSarlaft = AppResourcesPerson.MessageSarlaft;
                }
            }

        }
        if (msjSarlaft != "") {
            $.UifDialog('alert', {
                'message': msjSarlaft
            });
            return false;
        }
        return true;

    }

    static GetAplicationCompanyByDocument(documentNumber, searchTypeP) {

        PersonRequest.GetAplicationCompanyByDocument(searchTypeP, documentNumber, $("#inputTradeName").val()).done(function (data) {
            if (data.result.length != 0) {
                $("#HiddenInsuredCode").val("");
                var dta = data.result;
                if (dta.IsAuthorizationRequest) {
                    Persons.ValidateAuthorizationRequest(dta, TypePerson.PersonLegal);
                }
                else {
                    if (dta.length == 0) {
                        Persons.SetDataCompanyProspect(documentNumber);
                    }
                    else {
                        if (dta.length == 1) {
                            Persons.setTypePersonDefault(searchTypeP);
                            PersonLegal.NewLegal();
                            PersonLegal.LoadCompany(dta[0]);
                            company = dta[0];
                            ConsortiumMember.LoadConsortium(company.Id);
                            Provider.loadProviderPerson(dta[0].Provider);
                            Persons.ShowPanelPeople();
                            MethodPayment.GetPaymentMethod();
                            Persons.AddSubtitlesRightBar();
                        }
                        else {
                            Persons.ResultSeachList(documentNumber, dta);
                        }
                        PersonRequest.GetTypeRolByIndividual(dta[0].Id).done(function (data) {
                            if (data.success) {
                                Persons.ResetRolPerson();
                                $.each(data.result, function (key, value) {
                                    Persons.ResultSeachRolPerson(value.RoleId);
                                });
                            }
                        });
                    }
                }
            }
            else {
                Persons.SetDataCompanyProspect(documentNumber);
            }
        });
    }

    static ShowModalListAll(dataTable) {
        $('#tableResultsAll').UifDataTable('clear');
        var table = $('#tableResultsAll').DataTable();
        table.on('draw', function () {
            $('#tableResultsAll tbody td:nth-child(1)').hide();
            $('#tableResultsAll tbody td:nth-child(4)').hide();
            $('#tableResultsAll thead th:eq(0)').hide();
            $('#tableResultsAll thead th:eq(3)').hide();
        });
        $('#tableResultsAll').UifDataTable('addRow', dataTable);
    }

    static GetProspectByDocumentNumSearchType(DocumentNumber, searchTypeT) {

        PersonRequest.GetProspectByDocumentNum(DocumentNumber, searchTypeT).done(function (data) {
            if (data.result == null) {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResourcesPerson.MessagePersonEmpty, 'autoclose': true
                })
                return false;
            }
            else if (data.success) {
                if (data.result.TributaryIdNumber == null || data.result.TributaryIdNumber == " ") {
                    Persons.setTypePersonDefault(searchTypeT);
                    ProspectusNatural.SetProspectNatural(data.result);
                }
                else {
                    Persons.setTypePersonDefault(searchTypeT);
                    ProspectusLegal.SetProspectLegal(data.result);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static CreteInsuredGuarantee(codeInsured, individual, GuaranteeViewModel) {
        guaranteeModel = GuaranteeViewModel;
        if (codeInsured < 0) {
            var insuredTmp = Insured.CreateInsuredConsortiumModel(individual);
            InsuredRequest.CreateInsuredConsortium(insuredTmp).done(function (data) {
                if (data.success) {
                    $('#CodInsured').val(data.result.InsuredId);
                    $('#CodInsured').text(data.result.InsuredId);
                    router.run("prtGuarantee");
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            router.run("prtGuarantee");
        }
    }

    static GetAgenciesByAgentIdDescription(agentId, description) {
        var number = parseInt(description, 10);
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            AgentRequest.GetAgenciesByAgentIdDescription(agentId, description).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.length == 1) {
                            if (insuredTmp === null) {
                                insuredTmp = [];
                            }
                            insuredTmp.Agency = data.result[0];
                            $("#inputAgentPrincipal").val(data.result[0].FullName + ' (' + data.result[0].IndividualId + ')');
                            $('#inputAgentPrincipal').data("Agent", {
                                AgentId: data.result[0].IndividualId, IndividualId: data.result[0].IndividualId
                            })
                            Persons.GetAgenciesByAgentId(data.result[0].IndividualId);
                        }
                        else if (data.result.length > 1) {
                            modalListType = 3;
                            var dataList = {
                                dataObject: []
                            };

                            for (var i = 0; i < data.result.length; i++) {
                                dataList.dataObject.push({
                                    Id: data.result[i].IndividualId,
                                    Code: data.result[i].IndividualId,
                                    Description: data.result[i].FullName,
                                    Description2: data.result[i].FullName,
                                    DateDeclined: data.result[i].DateDeclined
                                });
                            }
                            Insured.ShowModalListInsured(dataList.dataObject);
                            $('#modalDialogListInsured').UifModal('showLocal', AppResourcesPerson.SelectMainAgent);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.NoAgentsDescriptionEntered, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifDialog('alert', {
                            'message': AppResourcesPerson.AgentDisabled
                        });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static GetAgenciesByAgentId(agentId, agencyId) {
        AgentRequest.GetAgenciesByAgentId(agentId).done(function (data) {
            if (data.success) {
                if (agencyId != undefined) {
                    $('#selectAgency').UifSelect({ sourceData: data.result, selectedId: agencyId });
                }
                else {
                    $('#selectAgency').UifSelect({ sourceData: data.result });
                }
            }
        });
    }

    static setDataOnlinePerson() {
        switch (glbPersonOnline.RolType) {
            case 1:
                Persons.SetDataPerson();
                break;
            case 2:
                Persons.SetDataCompany();
                break;
            case 3:
                $('#InputNamePrn').val(glbPersonOnline.Name);
                $('#InputDocumentNumber').val(glbPersonOnline.DocumentNumber);
                break;
            case 4:
                $('#InputFirstName').val(glbPersonOnline.Name);
                $('#InputDocumentNumber').val(glbPersonOnline.DocumentNumber);
                $("#InputDigit").val(Shared.CalculateDigitVerify(glbPersonOnline.DocumentNumber));
                break;
        }
    }

    static SetDataPersonProspect(individualId) {
        PersonRequest.GetProspectByDocumenNumberOrDescription(individualId, 1).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    prospect = data.result[0];
                    $("#selectDocumentTypePn").UifSelect("setSelected", glbPersonOnline == null ? prospect.IdentificationDocument.DocumentType.Id : glbPersonOnline.ViewModel.HolderDocumentType);
                    $('#InputDocumentNumber').val(prospect.IdentificationDocument.Number);
                    $("#InputFirstNamePn").val(prospect.Surname);
                    $("#InputLastNamePn").val(prospect.SecondSurname);
                    $("#InputNameIndividualPn").val(prospect.Name);
                    if (prospect.Gender == 'M') {
                        $('#selectGender').UifSelect("setSelected", GenderType.Male);
                    }
                    else
                        $('#selectGender').UifSelect("setSelected", GenderType.Female);

                    $("#selectMaritalStatusPn").UifSelect("setSelected", prospect.MaritalStatus);
                    $("#InputbirthdatePn").val(FormatDate(prospect.BirthDate, 1));
                    Shared.calculateAge("#InputAgePn", prospect.BirthDate);
                    if (prospect.CompanyName != null) {
                        if (prospect.CompanyName.Address != null && prospect.CompanyName.Address.AddressType.Id > 0) {
                            $("#selectCompanyAddressType").UifSelect("setSelected", prospect.CompanyName.Address.AddressType.Id);
                            $("#inputAddressAllPn").val(prospect.CompanyName.Address.Description);
                            if (prospect.CompanyName.Address.City != null) {
                                if (prospect.CompanyName.Address.City.State.Country.Description == null)
                                    prospect.CompanyName.Address.City.State.Country.Description = $("#inputCountryPn").val();
                                PersonNatural.GetCountriesPerson(prospect.CompanyName.Address.City);
                                PersonNatural.LoadDaneCodePersonNatural(prospect.CompanyName.Address.City.Id);
                            }
                        }
                        if (prospect.CompanyName.Phone != null) {
                            $("#inputPhonePn").val(prospect.CompanyName.Phone.Description);
                        }
                        if (prospect.CompanyName.Email != null) {
                            $("#inputEmailPn").val(prospect.CompanyName.Email.Description);
                        }
                    }
                } else {
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResourcesPerson.MessagePersonEmpty, 'autoclose': true
                    })
                    return false;
                }

            } else {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResourcesPerson.MessagePersonEmpty, 'autoclose': true
                })
                return false;
            }
        });
    }


    static SetDataPerson() {
        if (glbPersonOnline.IndividualId > 0) {
            PersonRequest.GetProspectByDocumenNumberOrDescription(glbPersonOnline.IndividualId, 2).done(function (data) {
                if (data.success) {
                    prospect = data.result[0];
                    $("#selectDocumentTypePn").UifSelect("setSelected", glbPersonOnline == null ? 1 : glbPersonOnline.ViewModel.HolderDocumentType);
                    $('#InputDocumentNumber').val(prospect.IdentificationDocument.Number);
                    $("#InputFirstNamePn").val(prospect.Surname);
                    $("#InputLastNamePn").val(prospect.SecondSurname);
                    $("#InputNameIndividualPn").val(prospect.Name);

                    if (prospect.Gender == 'M') {
                        $('#selectGender').UifSelect("setSelected", GenderType.Male);
                    }
                    else {
                        $('#selectGender').UifSelect("setSelected", GenderType.Female);
                    }

                    $("#selectMaritalStatusPn").UifSelect("setSelected", prospect.MaritalStatus);
                    $("#InputbirthdatePn").val(FormatDate(prospect.BirthDate, 1));
                    Shared.calculateAge("#InputAgePn", prospect.BirthDate);
                    if (prospect.CompanyName != null) {
                        if (prospect.CompanyName.Address != null && prospect.CompanyName.Address.AddressType.Id > 0) {
                            $("#selectCompanyAddressType").UifSelect("setSelected", prospect.CompanyName.Address.AddressType.Id);
                            $("#inputAddressAllPn").val(prospect.CompanyName.Address.Description);
                            if (prospect.CompanyName.Address.City != null) {
                                if (prospect.CompanyName.Address.City.State.Country.Description == null) {
                                    prospect.CompanyName.Address.City.State.Country.Description = $("#inputCountryPn").val();
                                }

                                PersonNatural.GetCountriesPerson(prospect.CompanyName.Address.City);
                                PersonNatural.LoadDaneCodePersonNatural(prospect.CompanyName.Address.City.Id);
                            }
                        }
                        if (prospect.CompanyName.Phone != null) {
                            $("#inputPhonePn").val(prospect.CompanyName.Phone.Description);
                        }
                        if (prospect.CompanyName.Email != null) {
                            $("#inputEmailPn").val(prospect.CompanyName.Email.Description);
                        }
                    }
                }
            });
        }
        else {
            $('#InputNameIndividualPn').val(glbPersonOnline.Name);
            $('#InputDocumentNumber').val(glbPersonOnline.DocumentNumber);
        }
    }

    static SetDataCompanyProspect(documentNum) {
        PersonRequest.GetProspectByDocumenNumberOrDescription(documentNum, 1).done(function (data) {
            if (data.success) {
                if (data.result.length > 0 && data.result[0].IndividualType == 2) {
                    prospect = data.result[0];
                    $("#selectCompanyDocumentType").UifSelect("setSelected", prospect.IdentificationDocument.DocumentType == null ? prospect.CustomerType : prospect.IdentificationDocument.DocumentType.Id);
                    $('#inputCompanyDocumentNumber').val(prospect.IdentificationDocument.Number);
                    $("#inputCompanyDigit").val(Shared.CalculateDigitVerify(prospect.IdentificationDocument.Number));
                    $('#inputCompanyTradeName').val(prospect.TradeName);
                    if (prospect.CompanyName != null) {
                        if (prospect.CompanyName.Address != null && prospect.CompanyName.Address.AddressType.Id > 0) {
                            $("#selectCompanyAddressType").UifSelect("setSelected", prospect.CompanyName.Address.AddressType.Id);
                            $("#inputCompanyAddress").val(prospect.CompanyName.Address.Description);
                            if (prospect.CompanyName.Address.City.State.Country.Description == null) {
                                prospect.CompanyName.Address.City.State.Country.Description = $("#inputCountryPn").val();
                            }
                            PersonLegal.GetCountriesCompany(prospect.CompanyName.Address.City);
                        }
                        if (prospect.CompanyName.Phone != null) {
                            $("#inputCompanyPhone").val(prospect.CompanyName.Phone.Description);
                        }
                        if (prospect.CompanyName.Email != null) {
                            $("#inputCompanyEmail").val(prospect.CompanyName.Email.Description);
                        }
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessagePersonEmpty, 'autoclose': true });
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static SetDataCompany() {
        if (glbPersonOnline.IndividualId > 0) {
            PersonRequest.GetProspectByDocumenNumberOrDescription(glbPersonOnline.IndividualId, 2).done(function (data) {
                if (data.success) {
                    prospect = data.result[0];
                    $("#selectCompanyDocumentType").UifSelect("setSelected", prospect.IdentificationDocument.DocumentType == null ? prospect.CustomerType : prospect.IdentificationDocument.DocumentType.Id);
                    $('#inputCompanyDocumentNumber').val(prospect.IdentificationDocument.Number);
                    $("#inputCompanyDigit").val(Shared.CalculateDigitVerify(prospect.IdentificationDocument.Number));
                    $('#inputCompanyTradeName').val(prospect.TradeName);
                    if (prospect.CompanyName != null) {
                        if (prospect.CompanyName.Address != null && prospect.CompanyName.Address.AddressType.Id > 0) {
                            $("#selectCompanyAddressType").UifSelect("setSelected", prospect.CompanyName.Address.AddressType.Id);
                            $("#inputCompanyAddress").val(prospect.CompanyName.Address.Description);
                            if (prospect.CompanyName.Address.City != null) {
                                if (prospect.CompanyName.Address.City.State.Country.Description == null) {
                                    prospect.CompanyName.Address.City.State.Country.Description = $("#inputCountryPn").val();
                                }
                                PersonLegal.GetCountriesCompany(prospect.CompanyName.Address.City);
                            }
                        }
                        if (prospect.CompanyName.Phone != null) {
                            $("#inputCompanyPhone").val(prospect.CompanyName.Phone.Description);
                        }
                        if (prospect.CompanyName.Email != null) {
                            $("#inputCompanyEmail").val(prospect.CompanyName.Email.Description);
                        }
                    }
                }
            });
        }
        else {
            $('#inputCompanyTradeName').val(glbPersonOnline.Name);
            $('#inputCompanyDocumentNumber').val(glbPersonOnline.DocumentNumber);
            $("#inputCompanyDigit").val(Shared.CalculateDigitVerify(glbPersonOnline.DocumentNumber));
        }
    }

    static LoadPerson(individualId) {
        if (searchType == TypePerson.PersonNatural) {
            PersonRequest.GetPersonByIndividualId(individualId).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        PersonNatural.ClearControlPersonNatural();
                        PersonNatural.SetPersonControl(data.result);
                        Persons.ShowPanelPeople();
                        Provider.loadProviderPerson(data.result.Provider);
                        PersonTax.loadDataTax(data.result.IndividualTaxs);
                        Persons.AddSubtitlesRightBar();
                    }
                }
            });
        }
        else {
            Persons.GetCompanyByIndividualId(individualId);
            searchType = TypePerson.PersonLegal;
            $("#legalPerson").attr("checked", true);
        }
    }

    static LoadNotificationPerson(individualId) {
        Persons.GetCompanyByIndividualId(individualId);
    }

    static GetPersonByIndividualId(individualId) {
        PersonRequest.GetPersonByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (data.result.IsAuthorizationRequest) {
                        Persons.ValidateAuthorizationRequest(data.result, TypePerson.PersonNatural)
                    }
                    else {
                        PersonNatural.ClearControlPersonNatural();
                        PersonNatural.SetPersonControl(data.result);

                        PersonRequest.GetTypeRolByIndividual(data.result.Id).done(function (data) {
                            if (data.success) {
                                Persons.ResetRolPerson();
                                $.each(data.result, function (key, value) {
                                    Persons.ResultSeachRolPerson(value.RoleId);
                                });
                            }
                        });

                        Persons.ShowPanelPeople();
                        Provider.loadProviderPerson(data.result.Provider);
                        PersonTax.loadDataTax(data.result.IndividualTaxs);
                        Persons.AddSubtitlesRightBar();
                    }
                }
            }
        });
    }

    static GetCompanyByIndividualId(individualId) {
        PersonRequest.GetCompanyByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (data.result.IsAuthorizationRequest) {
                        Persons.ValidateAuthorizationRequest(data.result, TypePerson.PersonLegal)
                    }
                    else {
                        PersonLegal.ClearControlPersonLegal();
                        PersonLegal.LoadCompany(data.result);
                        Persons.ShowPanelPeople();
                        Provider.loadProviderPerson(data.result.Provider);
                        PersonTax.loadDataTax(data.result.IndividualTaxs);
                        Persons.AddSubtitlesRightBar();

                        PersonRequest.GetTypeRolByIndividual(individualId).done(function (data1) {
                            if (data1.success) {
                                Persons.ResetRolPerson();
                                $.each(data1.result, function (key, value) {
                                    Persons.ResultSeachRolPerson(value.RoleId);
                                });
                            }
                        });
                    }
                }
            }
        });
    }

    static GetProspectByIndividualId(DocumentNumber, searchTypeT) {
        PersonRequest.GetProspectByIndividualId(DocumentNumber).done(function (data) {
            if (data.result == null) {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResourcesPerson.MessagePersonEmpty, 'autoclose': true
                })
                return false;
            }
            else if (data.success) {
                if (data.result.length == 1) {
                    if (data.result[0].TributaryIdNumber == null || data.result[0].TributaryIdNumber == " ") {
                        Persons.setTypePersonDefault(searchTypeT);
                        ProspectusNatural.SetProspectNatural(data.result);
                    }
                    else {
                        Persons.setTypePersonDefault(searchTypeT);
                        ProspectusLegal.SetProspectLegal(data.result[0]);
                    }

                }
                else {
                    Persons.ResultSeachList(DocumentNumber, data);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ResultSeachList(DocumentNumber, data) {
        AdvancedSearch.SearchAdvPerson(searchType);
        switch (searchType) {
            case TypePerson.PersonNatural:
                $("#selectTypePerson").UifSelect("setSelected", TypePerson.PersonNatural);
                AdvancedSearch.LoadPersonaAdv(data.result, "#searchNaturalTemplate");
                $("#inputDocumentPersonAdv").val(DocumentNumber);
                break;
            case TypePerson.PersonLegal:
                $("#selectTypePerson").UifSelect("setSelected", TypePerson.PersonLegal);
                AdvancedSearch.LoadPersonaAdv(data, "#searchLegalTemplate");
                $("#inputDocumentCompanyAdv").val(DocumentNumber);
                break;
            case TypePerson.ProspectNatural:
                $("#selectTypePerson").UifSelect("setSelected", TypePerson.ProspectNatural);
                AdvancedSearch.LoadPersonaAdv(data.result, "#searchNaturalTemplate");
                $("#inputDocumentPersonAdv").val(DocumentNumber);
                break;
            case TypePerson.ProspectLegal:
                $("#selectTypePerson").UifSelect("setSelected", TypePerson.ProspectLegal);
                AdvancedSearch.LoadPersonaAdv(data.result, "#searchLegalTemplate");
                $("#inputDocumentCompanyAdv").val(DocumentNumber);
                break;
            default:
                break;
        }
    }

    static ResultSeachRolPerson(idRol) {
        switch (idRol) {
            case RollAgent:
                $('#checkAgent').prop('checked', true);
                break;
            case RollCoinsured:
                $('#checkCoInsurer').prop('checked', true);
                break;
            case RollEmployee:
                $('#checkEmployee').prop('checked', true);
                break;
            case RollInsured:
                $('#checkInsured').prop('checked', true);
                break;
            case RollReinsurer:
                $('#checkReinsurer').prop('checked', true);
                break;
            case RollSupplier:
                $('#checkProvider').prop('checked', true);
                break;
            case RollThird:
                $('#checkThird').prop('checked', true);
                break;
        }
    }

    static ResetRolPerson() {
        $('#checkInsured').prop('checked', false);
        $('#checkAgent').prop('checked', false);
        $('#checkEmployee').prop('checked', false);
        $('#checkProvider').prop('checked', false);
        $('#checkThird').prop('checked', false);
        $('#checkReinsurer').prop('checked', false);
        $('#checkCoInsurer').prop('checked', false);
    }

    static ValidateRolPerson(individualId) {
        InsuredRequest.GetInsuredByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result.IndividualId > 0) {
                    $('#checkInsured').prop('checked', true);
                } else {
                    $('#checkInsured').prop('checked', false);
                }
            }
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
        ProviderRequest.ValidateSupplierByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result.IndividualId > 0) {
                    $('#checkProvider').prop('checked', true);
                } else {
                    $('#checkProvider').prop('checked', false);
                }
            }
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
        ThirdRequest.ValidateThirdByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result.IndividualId > 0) {
                    $('#checkThird').prop('checked', true);
                } else {
                    $('#checkThird').prop('checked', false);
                }
            }
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
        AgentRequest.GetAgent(individualId).done(function (data) {
            if (data.success) {
                if (data.result.IndividualId > 0) {
                    $('#checkAgent').prop('checked', true);
                } else {
                    $('#checkAgent').prop('checked', false);
                }
            }
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
        ReInsurerRequest.GetAplicationReInsurerByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result.IndividualId > 0) {
                    $('#checkReinsurer').prop('checked', true);
                } else {
                    $('#checkReinsurer').prop('checked', false);
                }
            }
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
        CoInsurerRequest.GetCoInsurer(individualId).done(function (data) {
            if (data.success) {
                if (data.result.IndividualId > 0) {
                    $('#checkCoInsurer').prop('checked', true);
                } else {
                    $('#checkCoInsurer').prop('checked', false);
                }
            }
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
    }

    static ValidateAuthorizationRequest(authorizationsModel, personType) {
        if (authorizationsModel.IsAuthorizationRequest) {
            if (authorizationsModel.AuthorizationRequests.every(x => x.Status == 1)) { //Pendientes
                $.UifDialog('alert', {
                    message: AppResourcesPerson.PersonWithAutorizationRequestPolicies
                });

                switch (personType) {
                    case TypePerson.PersonNatural:
                        PersonNatural.NewPerson();
                        break;
                    case TypePerson.PersonLegal:
                        PersonLegal.NewLegal();
                        break;
                    default:
                }
            }
            else if (authorizationsModel.AuthorizationRequests.every(x => x.Status == 3)) { //Rechazadas
                $.UifDialog('confirm', {
                    message: String.format(AppResourcesPerson.PersonWithRejectedPolicies)
                }, function (result) {
                    if (result) {
                        Persons.DisablePolicies(authorizationsModel.AuthorizationRequests);
                    }
                });
            }
        }
    }

    static DisablePolicies(authorizationRequests) {
        PersonRequest.DisablePolicies(authorizationRequests).done(function (response) {
            if (response.success) {
                $.UifNotify('show', {
                    'type': 'success', 'message': AppResourcesPerson.AllPoliciesRejected
                });
                $('#inputDocument').trigger('buttonClick');
            } else {
                $.UifNotify('show', {
                    'type': 'danger', 'message': AppResourcesPerson.CannotRejectPolicies
                });
            }
        });
    }
}