var Phonesses = [];
var PhonessesGet = [];
var PhoneRowId = -1;
var PhoneId = 0;
var countryParameterAddress = 1;
var heightListViewPhone = 256;
var glbPhoneAplicationStaus = null;
class Phone extends Uif2.Page {
    getInitialState() {
        this.InitializePhone();
        Phone.ClearPhone();
    }
    //Seccion Eventos
    bindEvents() {
        $("#btnNewPhone").click(Phone.ClearPhone);
        $("#btnCreatePhone").click(this.CreatePhoneBtn);
        $("#btnAcceptPhone").click(this.AcceptPhone);
        $("#btnCancelPhone").click(Phone.ClearPhone);
        $('#listPhonesses').on('rowEdit', this.PhonessesEdit);
        $("#inputPhone").on("focusout", Phone.SetMask);
        $("#inputPhone").on("focusin", Phone.maskOut);
        $("#inputCityCodeAdPnPhone").on("focusin", Phone.maskOut);
        $("#inputCityCodeAdPnPhone").on("focusout", Phone.SetMask);

    }

    InitializePhone() {
        $('#inputPhone').ValidatorKey(ValidatorType.Number, 1, 1);
        $("#inputCountryCodeAdPnPhone").ValidatorKey(1);
        $("#inputCityCodeAdPnPhone").ValidatorKey(1);
        $('#inputExtensionPhone').ValidatorKey(1);
    }

    //funciones Inicializar
    static ClearPhone() {
        PhoneRowId = -1;
        PhoneId = 0;
        $("#selectPhoneType").UifSelect("setSelected", null);
        $('#inputPhone').val('');
        $('#chkPhonePrincipal').prop('checked', false);
        $("#inputCountryCodeAdPnPhone").val("");
        $("#inputCityCodeAdPnPhone").val("");
        $("#inputExtensionPhone").val("");
        $("#inputScheduleAvailability").val("");
    }

    CreatePhoneBtn() {
        if (Phone.ValidatePhoneData()) {
            Phone.CreatePhone();
            Phone.ClearPhone();
        }
    }

    AcceptPhone() {
        Phone.SavePhones();
        Phone.ClearPhone();
    }

    PhonessesEdit(event, data, index) {
        PhoneRowId = index;
        Phone.EditPhone(data, index);
    }
    //limpiar objetos
    static CleanObjectPhonesses() {
        Phonesses = [];
        $("#listPhonesses").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#phoneTemplate", height: heightListViewPhone, title: AppResourcesPerson.LabelPhone });
    }

    //Seccion Load
    static LoadPhonesses(phonesses) {
        $("#listPhonesses").UifListView({ sourceData: phonesses, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#phoneTemplate", height: heightListViewPhone, title: AppResourcesPerson.LabelPhone });
    }

    static FillObjectPhone() {
        Phonesses.push({
            Id: 0,
            Description: $("#inputPhonePn").val(),
            PhoneTypeId: $("#selectPhoneTypePn").UifSelect("getSelected"),
            PhoneTypeDescription: $("#selectPhoneTypePn").UifSelect("getSelectedText"),
            IsPrincipal: true
        });
    }

    static FillObjectPhoneCompany() {
        Phonesses.push({
            Id: 0,
            Description: $("#inputCompanyPhone").val(),
            PhoneTypeId: $("#selectCompanyPhoneType").UifSelect("getSelected"),
            PhoneType: {
                Id: $("#selectCompanyPhoneType").UifSelect("getSelected"),
                Description: $("#selectPhoneTypePn").UifSelect("getSelectedText")
            },
            IsPrincipal: true
        });
    }

    static GetPhonesses() {
        var tempPhones = [];

        if (Phonesses != null && Phonesses.length > 0) {
            var phonetypes = $.makeArray($("#selectCompanyPhoneType option")
                .map(function (i, item) {
                    return {
                        Id: $(item).val(),
                        Description: $(item).text()
                    }
                }));
            Phonesses.forEach(function (item) {
                var pt = phonetypes.find(function (phonetype) { return item.PhoneTypeId == phonetype.Id });
                item.PhoneType = pt;
            });

            let listPhonesses = [];

            Phonesses.forEach(function (element) {
                let tempPhonesses = { PhoneType: {} };
                (element.PhoneType != null) ? tempPhonesses.PhoneType.Description = element.PhoneType.Description : tempPhonesses.PhoneType.Description = element.PhoneTypeDescription;
                (element.PhoneType != null) ? tempPhonesses.PhoneType.Id = element.PhoneType.Id : tempPhonesses.PhoneType.Id = element.PhoneTypeId;
                tempPhonesses.CityCode = element.CityCode;
                tempPhonesses.Description = element.Description;
                (element.IsMain != null) ? tempPhonesses.IsMain = element.IsMain : tempPhonesses.IsMain = 0;
                tempPhonesses.UpdateUser = element.UpdateUser;
                tempPhonesses.UpdateUser = element.UpdateDate;
                tempPhonesses.Extension = element.Extension;
                tempPhonesses.ScheduleAvailability = element.ScheduleAvailability;
                tempPhonesses.CountryCode = element.CountryCode;
                tempPhonesses.AplicationStaus = element.AplicationStaus;
                tempPhonesses.Id = element.Id;
                tempPhonesses.PhoneTypeId = element.PhoneTypeId;

                listPhonesses.push(tempPhonesses)
            });

            $("#listPhonesses").UifListView({ sourceData: listPhonesses, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#phoneTemplate", height: heightListViewPhone, title: AppResourcesPerson.LabelPhone });
            var contRowsPhones = $("#listPhonesses").UifListView('getData').length;
            if (contRowsPhones > 0) {
                $.each($("#listPhonesses").UifListView('getData'), function (key, value) {
                    if (this.IsPrincipal) {
                        switch (searchType) {
                            case TypePerson.PersonNatural:
                                $("#listPhonesses").UifListView("editItem", key, Phone.SetPhones(value.Id));
                                break;
                            case TypePerson.PersonLegal:
                                $("#listPhonesses").UifListView("editItem", key, Phone.SetPhoneCompany(value.Id));
                                break;
                        }
                    }
                });
            }
            else {
                switch (searchType) {
                    case TypePerson.PersonNatural:
                        tempPhones.push(Phone.SetPhones(0));
                        break;
                    case TypePerson.PersonLegal:
                        tempPhones.push(Phone.SetPhoneCompany(0));
                        break;
                }
                $("#listPhonesses").UifListView({ sourceData: tempPhones, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#phoneTemplate", height: heightListViewPhone, title: AppResourcesPerson.LabelPhone });
                Phonesses = tempPhones;
            }
        }
        else {
            switch (searchType) {
                case TypePerson.PersonNatural:
                    tempPhones.push(Phone.SetPhones(0));
                    break;
                case TypePerson.PersonLegal:
                    tempPhones.push(Phone.SetPhoneCompany(0));
                    break;
            }
            $("#listPhonesses").UifListView({ sourceData: tempPhones, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#phoneTemplate", height: heightListViewPhone, title: AppResourcesPerson.LabelPhone });
            Phonesses = tempPhones;
        }
    }

    static maskOut() {
        var texto = $("#inputPhone").val();
        if (texto.includes('(')) {
            var splitPhone = $("#inputPhone").val().split(')');
            $("#inputPhone").val(splitPhone[1]);
            var phoneCorrect = splitPhone[1].replace(/\ /g, "");
            $("#inputPhone").val(phoneCorrect);
        }
        else {
            $("#inputPhone").unmask();
        }
    }
    static SetMask() {
        var texto = $("#inputPhone").val();
        if ($("#inputPhone").val().length == 7) {
            if ($('#inputCountryCodeAdPnPhone').val() != '') {
                if ($('#inputCityCodeAdPnPhone').val() != '') {
                    var code = '(' + $('#inputCountryCodeAdPnPhone').val() + $('#inputCityCodeAdPnPhone').val() + ')';
                    $('#inputPhone').val(Phone.maskPhone(code, $('#inputPhone').val()));
                    //$('#inputPhone').val(Phone.maskPhone($('#inputPhone').val()));
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorMissingCityCode, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorMissingCountryCode, 'autoclose': true });
            }
        }
        else if ($('#inputCountryCodeAdPnPhone').val() != '') {
            $('#inputPhone').mask('000 000 0000');
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorMissingCountryCode, 'autoclose': true });
        }
    }
    static maskPhone(code, tele) {
        var cd = code + tele.substring(0, 3) + ' ' + tele.substring(3, 7);
        return cd;
    }

    //seccion Creacion y Grabado
    static CreatePhone() {   
        var phoneTmp = Phone.CreatePhoneModel();
        if (PhoneRowId == -1) {
            $("#listPhonesses").UifListView("addItem", phoneTmp);
        }
        else {
            $("#listPhonesses").UifListView("editItem", PhoneRowId, phoneTmp);
        }
    }

    static CreatePhoneModel() {
        var Phone = { PhoneType: { Description: $("#selectPhoneType").UifSelect("getSelectedText") } };
        Phone.Id = PhoneId;

        var texto = $("#inputPhone").val();

        if (($("#inputPhone").val().length >= 10 && $("#inputPhone").val().length <= 13) && texto.includes('(')) {
            var splitPhone = $("#inputPhone").val().split(')');
            var phoneCorrect = splitPhone[1].replace(/\ /g, "");
            Phone.Description = phoneCorrect;
        }
        else {
            var splitPhone = $("#inputPhone").val();
            var phoneCorrect = splitPhone.replace(/\ /g, "");
            Phone.Description = phoneCorrect;
        }
        Phone.PhoneTypeId = $("#selectPhoneType").UifSelect("getSelected");
        Phone.IsPrincipal = $('#chkPhonePrincipal').is(':checked');
        Phone.Extension = $("#inputExtensionPhone").val();
        Phone.ScheduleAvailability = $("#inputScheduleAvailability").val();
        Phone.CountryCode = $("#inputCountryCodeAdPnPhone").val();
        Phone.CityCode = $("#inputCityCodeAdPnPhone").val();
        if (Phonesses.length > 0) {
            if (Phonesses.find(x => x.Id == Phone.Id) != undefined) {
                Phone.AplicationStaus = 1;
            }
            else {
                Phone.AplicationStaus = 0;
            }
        }
        else {
            Phone.AplicationStaus = 0;
        }
        return Phone;
    }

    static SavePhones() {
        Phone.SendPhones();
        switch (searchType) {
            case TypePerson.PersonNatural:
                Phone.setPrincipalPhone();
                break;
            case TypePerson.PersonLegal:
                Phone.setPrincipalPhoneCompany();
                break;
            default:
                break;
        }
        Phone.ClosePhone();
    }

    static SendPhones() {        
        if ($("#listPhonesses").UifListView('getData').length > 0) {
            Phonesses = $("#listPhonesses").UifListView('getData');
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmpty, 'autoclose': false })
        }
    }

    //seccion edit
    static EditPhone(data, index) {
        PhoneId = data.Id;
        glbPhoneAplicationStaus = 1;
        $("#selectPhoneType").UifSelect("setSelected", data.PhoneTypeId || data.PhoneType.Id);
        $('#chkPhonePrincipal').prop("checked", data.IsPrincipal);
        $("#inputCountryCodeAdPnPhone").val(data.CountryCode);
        $("#inputCityCodeAdPnPhone").val(data.CityCode);
        $("#inputPhone").val(data.Description);
        if ($("#inputPhone").val().length > 7) {
            $('#inputPhone').mask('000 000 0000');
        }
        else {
            var code = '(09' + $('#inputCityCodeAdPnPhone').val() + ')';
            $('#inputPhone').val(Phone.maskPhone(code, $('#inputPhone').val()));
        }
        $('#inputExtensionPhone').val(data.Extension);
        $('#inputScheduleAvailability').val(data.ScheduleAvailability);
    }


    //seccion set
    static setPrincipalPhone() {
        $.each($("#listPhonesses").UifListView('getData'), function (i, item) {
            if (item.IsPrincipal == true || $("#listPhonesses").UifListView('getData').length == 1) {
                $("#selectPhoneTypePn").UifSelect("setSelected", item.PhoneType.Id || item.PhoneTypeId);
                $("#inputPhonePn").val(item.Description);
            }
        });
    }

    static setPrincipalPhoneCompany(phones) {
        //$.each($("#listPhonesses").UifListView('getData'), function (i, item) {
        //    if (item.IsPrincipal == true || $("#listPhonesses").UifListView('getData').length == 1) {
        //        $("#selectCompanyPhoneType").UifSelect("setSelected", item.PhoneType.Id);
        //        $("#inputCompanyPhone").val(item.Description);
        //    }
        //});

        for (var index in phones) {
            if (phones[index].IsPrincipal == true) {
                $("#selectCompanyPhoneType").UifSelect("setSelected", phones[index].PhoneTypeId);
                $("#inputCompanyPhone").val(phones[index].Description);
            }
            else {
                var phone = $("#listPhonesses").UifListView('getData');
                $("#selectCompanyPhoneType").UifSelect("setSelected", phone[0].PhoneTypeId);
                $("#inputCompanyPhone").val(phone[0].Description);
            }

        }

    }

    static SetPhones(id) {
        var phone = {
            Id: id,
            IsPrincipal: true,
            Description: $("#inputPhonePn").val(),
            PhoneTypeId: $("#selectPhoneTypePn").UifSelect("getSelected"),
            CountryCode: $("#inputCountryCodeAdPnPhone").val(),
            CityCode: $("#inputCityCodeAdPnPhone").val(),
            ScheduleAvailability: $("#inputScheduleAvailability").val(),
            Extension: $("#inputExtensionPhone").val(),
            PhoneType: {
                Description: $("#selectPhoneTypePn").UifSelect("getSelectedText"),
                Id: $("#selectPhoneTypePn").UifSelect("getSelected")
            }
        };
        //phone.Id = id;
        //phone.Description = $("#inputPhonePn").val();
        //phone.PhoneTypeId = $("#selectPhoneTypePn").UifSelect("getSelected");
        //phone.Description = $("#selectPhoneTypePn").UifSelect("getSelectedText");
        //phone.IsPrincipal = true;
        //phone.UpdateUser = Phonesses[0].UpdateUser;
        //phone.UpdateDate = FormatDate(Phonesses[0].UpdateDate, 1);
        //phone.CountryCode = $("#inputCountryCodeAdPnPhone").val();
        //phone.CityCode = $("#inputCityCodeAdPnPhone").val();
        //phone.ScheduleAvailability = $("#inputScheduleAvailability").val();
        //phone.Extension = $("#inputExtensionPhone").val();
        return phone;
    }

    static SetPhoneCompany(id) {
        var phone = { PhoneType: {} };
        phone.Id = id;
        phone.Description = $("#inputCompanyPhone").val();
        phone.PhoneTypeId = $("#selectCompanyPhoneType").UifSelect("getSelected");
        phone.Description = $("#selectCompanyPhoneType").UifSelect("getSelectedText");
        phone.IsPrincipal = true;
        phone.UpdateUser = Phonesses[0].UpdateUser;
        phone.UpdateDate = FormatDate(Phonesses[0].UpdateDate, 1);
        phone.CountryCode = Phonesses[0].CountryCode;
        phone.CityCode = Phonesses[0].CityCode;
        phone.ScheduleAvailability = Phonesses[0].ScheduleAvailability;
        phone.Extension = Phonesses[0].Extension;


        return phone;
    }

    static UpdatePrincipalPhone() {
        $.each(Phonesses, function (i, item) {
            if (item.IsPrincipal == true) {
                item.PhoneTypeId = $("#selectPhoneTypePn").UifSelect("getSelected");
                //item.Description = $("#selectPhoneTypePn").UifSelect("getSelectedText");
                item.Description = $("#inputPhonePn").val();
            }
        });
    }

    static UpdatePrincipalPhoneCompany() {
        $.each(Phonesses, function (i, item) {
            if (item.IsPrincipal == true) {
                item.PhoneTypeId = $("#selectCompanyPhoneType").UifSelect("getSelected");
                //item.Description = $("#selectCompanyPhoneType").UifSelect("getSelectedText");
                item.Description = $("#inputCompanyPhone").val();
                item.AplicationStaus = 1;
            }
        });
    }

    //Cerrar
    static ClosePhone() {
        $('#modalPhone').UifModal('hide');
    }

    //validaciones
    static ValidatePhoneData() {
        var error = "";
        if ($('#selectPhoneType').UifSelect("getSelected") == '') {
            error = error + AppResourcesPerson.LabelPhoneType + "<br>";
        }
        if ($('#inputPhone').val() == '') {
            error = error + AppResourcesPerson.LabelPhoneNumber + "<br>";
        }
        if ($('#inputCountryCodeAdPnPhone').val() == '') {
            error = error + AppResourcesPerson.LabelCountry + "<br>";
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + error, 'autoclose': true })
            return false;
        }
        else if (Phone.PrincipalPhone()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorPhoneMain, 'autoclose': true });
            return false;
        } else if (Phone.DuplicatePhone($('#inputPhone').val(), $('#selectPhoneType').UifSelect("getSelected"))) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorNumberPhone, 'autoclose': true });
            return false;
        }
        return true;
    }

    static PrincipalPhone() {
        var principal = false;
        var IsPrincipal = $("#chkPhonePrincipal").is(":checked");
        if (IsPrincipal) {
            $.each($("#listPhonesses").UifListView('getData'), function (i, item) {
                if (item.IsPrincipal == IsPrincipal && i != PhoneRowId) {
                    principal = true;
                    return false;
                }
            });
        }
        return principal;
    }

    static DuplicatePhone(phone, typePhone) {
        var duplicate = false;
        if (phone != undefined) {
            var indexPhone = phone.indexOf(")");
            if (indexPhone > -1) {
                phone = phone.substring((indexPhone + 1), phone.length).replace(" ", "");
            }
        }
        $.each($("#listPhonesses").UifListView('getData'), function (i, item) {
            if ($.trim(item.Description) == $.trim(phone) && item.PhoneTypeId == typePhone && i != PhoneRowId) {
                duplicate = true;
                return false;
            }
        });

        return duplicate;
    }

    static ConvertAddressDtoToModel(phonesDTO) {
        var phoneModel = Phone.CreatePhoneModel();
        var rslt = [];

        if (phonesDTO.Phones != null && phonesDTO.Phones.length > 0) {
            for (var index = 0; index < phonesDTO.Phones.length; index++) {
                phoneModel.PhoneType = phonesDTO.Phones[index].PhoneTypeId;
                phoneModel.Description = phonesDTO.Phones[index].Description;
                phoneModel.Id = phonesDTO.Phones[index].PhoneTypeId;
                if (phonesDTO.Addresses[index] != undefined) {
                    phoneModel.CountryCode = phonesDTO.Addresses[index].CountryId;
                    phoneModel.CityCode = phonesDTO.Addresses[index].CityId;
                } else if (phonesDTO.Addresses.length > 0) {
                    phoneModel.CountryCode = phonesDTO.Addresses[0].CountryId;
                    phoneModel.CityCode = phonesDTO.Addresses[0].CityId;
                }
                rslt.push(phoneModel);
            }     
        }
        
        return rslt;
    }

    static ConvertAddressModelToDTO(phonesDTO) {
        var phones = {};
        phonesDTO.AddressTypeId = Address.Id;
        phonesDTO.Description = Address.Description;
        phonesDTO.AddressTypeId = Address.AddressType.Id;
        phonesDTO.Description = Address.AddressType.Description;
        phonesDTO.CityId = Address.City.Id;
        phonesDTO.StateId = Address.City.State.Id;
        phonesDTO.CountryId = Address.City.State.Country.Id;
        return phones;
    }

}