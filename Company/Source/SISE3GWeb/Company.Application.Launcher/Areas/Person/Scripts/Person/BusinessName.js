var BusinessData = [];
var rowBusinessNameId = -1;
var BusinessNameId = 0;
var heightListViewBusinessName = 256;
class BusinessName extends Uif2.Page {
    getInitialState() {

        $("#listBusinessName").UifListView({
            source: null,
            height: 300,
            displayTemplate: '#businessNameTemplate',
            edit: true,
            customEdit: true
        });
        BusinessName.ClearBusinessName();
    }

    //Seccion Eventos
    bindEvents() {
        $("#btnNewBusinessName").click(BusinessName.ClearBusinessName);
        $("#btnCreateBusinessName").click(this.BtnCreateBusinessName);
        $("#btnAcceptBusinessName").click(this.SaveBusinessName);
        $('#listBusinessName').on('rowEdit', this.BusinessNameEdit);
    }

    static ClearBusinessName() {
        rowBusinessNameId = -1;
        //BusinessNameId = 0;
        $("#InputBusinessName").val('');
        $("#selectAddressBusinessName").UifSelect('setSelected', "");
        $("#selectPhoneBusinessName").UifSelect('setSelected', "");
        $("#selectEmailBusinessName").UifSelect('setSelected', "");
        $("#EnableId").prop("checked", false);
        $("#IsMainBusinessName").prop("checked", false);
    }

    BtnCreateBusinessName() {
        if (BusinessName.ValidateBusinessName()) {
            BusinessName.CreateBusinessName();
            BusinessName.ClearBusinessName();
        }
    }

    SaveBusinessName() {

        var dataBusiness = $("#listBusinessName").UifListView("getData");
        if (dataBusiness.length > 0) {
            if (dataBusiness.find(x => x.IsMain == true) != undefined) {
                lockScreen();
                BusinessNameRequest.SaveBusinessName(dataBusiness).done(function (result) {
                    if (result.success && result.result != 'Error to create Tax') {
                        var policyType = LaunchPolicies.ValidateInfringementPolicies(result.result[0].InfringementPolicies, true);
                        let countAuthorization = result.result[0].InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                        if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                            if (countAuthorization > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(result.result[0].InfringementPolicies, result.result[0].OperationId, FunctionType.PersonBusinessName);
                            }
                        } else {
                            //PersonTax.clearTax();
                            BusinessData = result.result;
                            BusinessName.LoadBusinessName(BusinessData);
                            $("#modalBusinessName").UifModal("hide");
                            //dataTax.status = 2;
                            $.UifNotify('show', { 'type': 'info', 'message': 'Razon Social Guardada', 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': result.result, 'autoclose': true });
                        $("#modalBusinessName").UifModal("hide");
                        //PersonTax.clearTax();
                    }
                    unlockScreen();
                }).fail(() => unlockScreen());;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.BusinessNameMain, 'autoclose': false });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmpty, 'autoclose': false });
        }
    }

    BusinessNameEdit(event, data, index) {
        BusinessData = [];
        rowBusinessNameId = index;
        BusinessData.push(data);
        BusinessName.EditBusinessName(data, index);
    }

    static LoadBusinessName(businessData) {
        $("#listBusinessName").UifListView({ sourceData: businessData, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#businessNameTemplate", height: heightListViewBusinessName });
    }

    static CreateBusinessName() {
        if (BusinessNameId == 0) {
            BusinessNameId = 0;
        }
        var BusinessNameTmp = BusinessName.CreateBusinessNameModel();
        if (rowBusinessNameId == -1) {
            $("#listBusinessName").UifListView("addItem", BusinessNameTmp);
        }
        else {
            $("#listBusinessName").UifListView("editItem", rowBusinessNameId, BusinessNameTmp);
        }
    }

    static CreateBusinessNameModel() {

        var BusinessName = { Address: { AddressType: {}, City: {} }, Email: { EmailType: {} }, Phone: { PhoneType: {} } };
        BusinessName.AddressID = $("#selectAddressBusinessName").UifSelect("getSelected");
        BusinessName.PhoneID = $("#selectPhoneBusinessName").UifSelect("getSelected");
        BusinessName.EmailID = $("#selectEmailBusinessName").UifSelect("getSelected");
        BusinessName.IndividualId = individualId;
        if (BusinessData.length > 0) {
            if (BusinessData[0].NameNum > 0) {
                BusinessName.NameNum = BusinessData[0].NameNum;
                BusinessData = [];
            }

        }
        else {
            BusinessName.NameNum = 1;
            var a = $("#listBusinessName").UifListView('getData');
            if (a.length > 0) {
                BusinessName.NameNum = a.length + 1;
            }
        }

        BusinessName.TradeName = $("#InputBusinessName").val();
        BusinessName.Address.Id = $("#selectAddressBusinessName").UifSelect("getSelected");
        BusinessName.Address.Description = $("#selectAddressBusinessName").UifSelect("getSelectedText");
        BusinessName.Address.City.Description = $("#selectAddressBusinessName").UifSelect("getSelectedSource").CityDescription;
        BusinessName.Address.AddressType.Id = $("#selectAddressBusinessName").UifSelect("getSelectedSource").AddressTypeId;
        BusinessName.Address.AddressType.Description = addressTypes.find(function (elementAddress) { return elementAddress.Id == $("#selectAddressBusinessName").UifSelect("getSelectedSource").AddressTypeId }).Description;
        BusinessName.Email.Id = $("#selectEmailBusinessName").UifSelect("getSelected");
        BusinessName.Email.Description = $("#selectEmailBusinessName").UifSelect("getSelectedText");
        BusinessName.Email.EmailType.Id = $("#selectEmailBusinessName").UifSelect("getSelectedSource").Id;
        BusinessName.Email.EmailType.Description = emailTypes.find(function (elementEmailType) { return elementEmailType.Id == $("#selectEmailBusinessName").UifSelect("getSelectedSource").EmailTypeId }).Description;
        BusinessName.Phone.Id = $("#selectPhoneBusinessName").UifSelect("getSelected");
        BusinessName.Phone.Description = $("#selectPhoneBusinessName").UifSelect("getSelectedText");
        BusinessName.Phone.PhoneType.Id = $("#selectPhoneBusinessName").UifSelect("getSelectedSource").Id;
        BusinessName.Phone.PhoneType.Description = $("#selectPhoneBusinessName").UifSelect("getSelectedSource").PhoneType.Description;
        if ($('#EnableId').is(':checked')) {
            BusinessName.Enabled = true;
        }
        else {
            BusinessName.Enabled = false;
        }
        if ($('#IsMainBusinessName').is(':checked')) {
            BusinessName.IsMain = true;
        }
        else {
            BusinessName.IsMain = false;
        }
        return BusinessName;
    }

    //seccion edit
    static EditBusinessName(data, index) {
        BusinessNameId = data.NameNum;
        $("#InputBusinessName").val(data.TradeName);
        $("#selectAddressBusinessName").UifSelect("setSelected", data.AddressID);
        $("#selectPhoneBusinessName").UifSelect("setSelected", data.PhoneID);
        $("#selectEmailBusinessName").UifSelect("setSelected", data.EmailID);
        $("#EnableId").prop("checked", data.Enabled);
        $("#IsMainBusinessName").prop("checked", data.IsMain);
    }

    static CleanObjectsBusinessName() {
        BusinessData = [];
        $("#listBusinessName").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#businessNameTemplate", height: heightListViewBusinessName });
    }

    static LoadAddresPhoneEmail() {
        var PhoneType = [];
        var AddressType = [];
        var EmailType = [];
        if (Phonesses != null && Phonesses.length > 0) {
            $('#selectPhoneBusinessName').UifSelect({ sourceData: Phonesses, id: 'Id', name: 'Description' });
        }
        if (addresses != null && addresses.length > 0) {
            $('#selectAddressBusinessName').UifSelect({ sourceData: addresses, id: 'Id', name: 'Description' });
        }
        if (emails != null && emails.length > 0) {
            $('#selectEmailBusinessName').UifSelect({ sourceData: emails, id: 'Id', name: 'Description' });
        }
    }

    //Seccion Validaciones
    static ValidateBusinessName() {
        var msj = "";
        if ($("#InputBusinessName").val() == "" || $("#InputBusinessName").val() == null) {
            msj = AppResourcesPerson.MessageBusinessNameEmpty + "<br>";
        }
        if ($("#selectAddressBusinessName").UifSelect("getSelected") == null || $("#selectAddressBusinessName").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.MessageAddressEmpty + " <br>";
        }
        if ($("#selectPhoneBusinessName").UifSelect("getSelected") == null || $("#selectPhoneBusinessName").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.MessagePhoneEmpty + " <br>";
        }
        if ($("#selectEmailBusinessName").UifSelect("getSelected") == null || $("#selectEmailBusinessName").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.MessageEmailEmpty + " <br>";
        }
        if (!BusinessName.IsMainBusinessName()) {
            msj = msj + AppResourcesPerson.ErrorBusinessNameMain + " <br>";
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true });
            return false;
        }
        return true;
    }

    static IsMainBusinessName() {
        if ($("#listBusinessName").UifListView('getData').length > 0 && $('#IsMainBusinessName').is(':checked')) {
            if (BusinessNameId == 0 && rowBusinessNameId == -1) {
                if (($("#listBusinessName").UifListView('getData').find(x => x.IsMain == true)) != undefined) {
                    return false;
                }
            }
            if (BusinessNameId == 0 && rowBusinessNameId > -1) {
                if (!($("#listBusinessName").UifListView('getData')[rowBusinessNameId].IsMain) &&
                    ($("#listBusinessName").UifListView('getData').find(x => x.IsMain == true)) != undefined) {
                    return false;
                }
            }
            if (BusinessNameId > 0 && rowBusinessNameId == -1) {
                if (($("#listBusinessName").UifListView('getData').find(x => x.IsMain == true)) != undefined) {
                    return false;
                }
            }
            if (BusinessNameId > 0 && rowBusinessNameId > -1) {
                if (!($("#listBusinessName").UifListView('getData')[rowBusinessNameId].IsMain) &&
                    ($("#listBusinessName").UifListView('getData').find(x => x.IsMain == true)) != undefined) {
                    return false;
                }
            }
        }
        return true;
    }
}