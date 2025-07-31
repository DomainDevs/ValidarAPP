var BankData = [];
var rowBankTransfersId = -1;
var BankTransfersId = 0;
var heightListViewBusinessName = 256;

class BankTransfers extends Uif2.Page {
    getInitialState() {

        $("#listVBankTransfers").UifListView({
            source: null,
            height: 430,
            displayTemplate: '#bankTransfersTemplate',
            edit: true,
            customEdit: true
        });
        BankTransfers.loadBank();
        BankTransfers.loadAccountType();
        BankTransfers.loadCurrency();
        BankTransfers.loadCountries();
    }
    bindEvents() {
        $('#inputBeneficiaryType').ValidatorKey(2, 1, 0);
        $("#btnBankNew").click(BankTransfers.ClearControlBankTransfers);
        BankTransfers.getForm().find('#selectBank').on('itemSelected', this.ChangeBank);
        $("#btnBankTransfers").click(BankTransfers.loadBeneficiary);
        BankTransfers.getForm().find('#Check').on('click', this.OnChecked);
        BankTransfers.getForm().find('#Transfer').on('click', this.OnCheckedTransfer);
        $("#btnBankAdd").click(this.startAddListVBank);
        $("#btnAcceptBankTransfers").click(this.SaveBankTransfers);
        $('#listVBankTransfers').on('rowEdit', this.BankEdit);
    }

    ChangeBank(event, data) {
        if (data.Id > 0) {
            BankTransfersRequest.GetBankById(data.Id).done(function (data) {
                if (data.success) {
                    $('#inputAddress').val(data.result.Address);
                    $("#inputCity").val(data.result.CityDescription);
                    $("#selectCountry").UifSelect("setSelected", data.result.CountryCode)
                    $('#inputAddress').prop('disabled', true);
                    $('#selectCountry').prop('disabled', true);
                    $('#inputCity').prop('disabled', true);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
             
            });
        }
    }

    static loadBeneficiary() {
        if (TypePerson.PersonNatural === parseInt($('#selectSearchPersonType').UifSelect("getSelected"))) {
            var cheqNum = $('#inputCheckPayableTo').val();
            $("#inputBeneficiaryType").val(cheqNum);

            BankTransfersRequest.GetBankTransfers(parseInt($('#lblPersonCode').val())).done(function (data) {
                BankTransfersRequest.GetAccountType().done(function (dataType) {
                    if (dataType.success) {
                        $.each(data.result, function (i, val) {
                            val.AccountTypeDescription = dataType.result.find(x => x.Code == val.AccountTypeId).Description;
                        });                        
                    }
                });
                $("#listVBankTransfers").UifListView({
                    sourceData: data.result,
                    height: 430,
                    displayTemplate: '#bankTransfersTemplate',
                    edit: true,
                    customEdit: true
                });
                if ($('#listVBankTransfers').UifListView("getData").length == 0) {
                    $('#Check').prop("checked", true);
                    BankTransfers.DisabledCombos();
                } else {
                    $('#Transfer').prop("checked", true);
                    BankTransfers.enabledCombos();
                }
            });
            

        }
        else if (TypePerson.PersonLegal === parseInt($('#selectSearchPersonType').UifSelect("getSelected"))) {
            var cheqName = $('#inputcheckpayable').val();
            $("#inputBeneficiaryType").val(cheqName);

            BankTransfersRequest.GetBankTransfers(parseInt($('#lblCompanyCode').val())).done(function (data) {
                BankTransfersRequest.GetAccountType().done(function (dataType) {
                    if (dataType.success) {
                        $.each(data.result, function (i, val) {
                            val.AccountTypeDescription = dataType.result.find(x => x.Code == val.AccountTypeId).Description;
                        });
                    }
                });
                $("#listVBankTransfers").UifListView({
                    sourceData: data.result,
                    height: 430,
                    displayTemplate: '#bankTransfersTemplate',
                    edit: true,
                    customEdit: true
                });

                if ($('#listVBankTransfers').UifListView("getData").length == 0) {
                    $('#Check').prop("checked", true);
                    BankTransfers.DisabledCombos();
                } else {
                    $('#Transfer').prop("checked", true);
                    BankTransfers.enabledCombos();
                }
            });
        }

     }

    static loadBank() {
        BankTransfersRequest.GetBank().done(function (data) {
            if (data.success) {
                BankTransfers.getForm().find("#selectBank").UifSelect({ sourceData: data.result });
            }
        });
    }

    static loadAccountType() {
        BankTransfersRequest.GetAccountType().done(function (data) {
            if (data.success) {
                BankTransfers.getForm().find("#selectAccountType").UifSelect({ sourceData: data.result });
            }
        });
    }
    static loadCurrency() {
        BankTransfersRequest.GetCurrencies().done(function (data) {
            if (data.success) {
                BankTransfers.getForm().find("#selectCurrency").UifSelect({ sourceData: data.result });
            }
        });
    }
    static loadCountries() {
        BankTransfersRequest.GetCountries().done(function (data) {
            if (data.success) {
                BankTransfers.getForm().find("#selectCountry").UifSelect({ sourceData: data.result });
            }
        });
    }

    OnCheckedTransfer() {
        if ($('input:radio[name=rbPaymentType]:checked').val() == "2") {
            BankTransfers.enabledCombos();
        }
    }
    OnChecked() {
        if ($('input:radio[name=rbPaymentType]:checked').val() == "1") {
            BankTransfers.DisabledCombos();
        }
    }


    //ChangeBank(event, selectedItem) {
    //    if (selectedItem.Id > 0) {
    //        BankTransfersRequest.GetBankBranches(selectedItem.Id, 0).done(function (data) {
    //            if (data.success) {
    //                BankTransfers.getForm().find("#selectBranchBank").UifSelect({ sourceData: data.result });
    //            }
    //        });
    //    }
    //    else {
    //        $("#selectBranchBank").UifSelect();
    //    }
    //}

    static getForm() {
        return $('#formBankTransfers');
    }
    static CreateBankTransfersModel() {



        var BanksTransfers = {
            Id: BankTransfersId !== 0 ? BankTransfersId :0,
            IndividualId: parseInt($('#lblPersonCode').val()) || parseInt($('#lblCompanyCode').val()),
            Check: $('#Check').is(':checked'),
            Transfer: $('#Transfer').is(':checked'),
            BankId: BankTransfers.getForm().find("#selectBank").UifSelect("getSelected"),
            BankDescription: BankTransfers.getForm().find("#selectBank").UifSelect("getSelectedText"),
            BankBranch: $("#inputBranchBank").val(),
            BankSquare: BankTransfers.getForm().find("#inputBankSquare").val(),
            AccountTypeId: BankTransfers.getForm().find("#selectAccountType").UifSelect("getSelected"),
            AccountTypeDescription: BankTransfers.getForm().find("#selectAccountType").UifSelect("getSelectedText"),
            AccountNumber: BankTransfers.getForm().find("#inputAccountNumber").val(),
            CurrencyId: $("#selectCurrency").UifSelect("getSelected"),
            CurrencyDescription: $("#selectCurrency").UifSelect("getSelectedText"),
            PaymentBeneficiary: $("#inputBeneficiaryType").val()
            //InscriptionDate : 
        }

        if ($('#Active').is(':checked')) {
            BanksTransfers.ActiveAccount = true;
        }
        else {
            BanksTransfers.ActiveAccount = false;
        }

        if ($('#DefaultAccount').is(':checked')) {
            BanksTransfers.DefaultAccount = true;
        }
        else {
            BanksTransfers.DefaultAccount = false;
        }

        if ($('#IntermediaryBank').is(':checked')) {
            BanksTransfers.IntermediaryBank = true;
        }
        else {
            BanksTransfers.IntermediaryBank = false;
        }
        
        return BanksTransfers;
    }

    SaveBankTransfers() {

        if ($('input:radio[name=rbPaymentType]:checked').val() == "2") {
            var dataBank = $("#listVBankTransfers").UifListView("getData");


            if (dataBank.length > 0) {
                lockScreen();
                BankTransfersRequest.SaveBankTransfers(dataBank).done(function (result) {
                    if (result.success && result.result != 'Error to create Bank') {
                        var dataBankTransfers = result.result[0];
                        var policyType = LaunchPolicies.ValidateInfringementPolicies(dataBankTransfers.InfringementPolicies, true);
                        let countAuthorization = dataBankTransfers.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                        if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                            if (countAuthorization > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(dataBankTransfers.InfringementPolicies, result.result[0].OperationId, FunctionType.PersonBankTransfers);
                            }
                        } else {
                            BusinessData = result.result;
                            $("#modalBankTransfers").UifModal("hide");
                            $.UifNotify('show', { 'type': 'info', 'message': 'Guardado correctamente', 'autoclose': true });
                        }
                    }
                    unlockScreen()
                }).fail(() => unlockScreen());
            }
        }
        else {
            $("#modalBankTransfers").UifModal("hide");
            $.UifNotify('show', { 'type': 'info', 'message': 'Guardado correctamente', 'autoclose': true });
        }
    }

    BankEdit(event, data, index) {
        rowBankTransfersId = index;
        BankTransfersId = data.Id;
        BankTransfers.EditBankTransfers(data, index);
        $('#Transfer').prop("checked", true);
        BankTransfers.enabledCombos();
    }

    static EditBankTransfers(data) {
        //BankTransfersId = data.NameNum;
        BankTransfers.getForm().find("#selectBank").UifSelect("setSelected", data.BankId);
        if (data.BankId > 0) {
            BankTransfersRequest.GetBankById(data.BankId).done(function (dataResult) {
                if (dataResult.success) {
                    BankTransfers.getForm().find("#inputCity").val(dataResult.result.CityDescription);
                    BankTransfers.getForm().find("#selectCountry").UifSelect("setSelected", dataResult.result.CountryCode);
                    BankTransfers.getForm().find("#inputAddress").val(dataResult.result.Address);
                    $('#inputAddress').prop('disabled', true);
                    $('#selectCountry').prop('disabled', true);
                    $('#inputCity').prop('disabled', true);
                }
            });
        }
        var InscriptionDate = FormatDate(data.InscriptionDate);
        if (InscriptionDate != null) {
            $("#InscriptionDate").UifDatepicker("setValue", InscriptionDate);
        }
        //BankTransfers.getForm().find("#selectBranchBank").UifSelect("setSelected", data.BankBranchId);
        BankTransfers.getForm().find('#inputBranchBank').val(data.BankBranch);
        BankTransfers.getForm().find('#inputBankSquare').val(data.BankSquare);

        BankTransfers.getForm().find("#inputBeneficiaryType").val(data.PaymentBeneficiary);
        BankTransfers.getForm().find("#selectAccountType").UifSelect("setSelected", data.AccountTypeId);
        BankTransfers.getForm().find("#selectCurrency").UifSelect("setSelected", data.CurrencyId);
        BankTransfers.getForm().find("#inputAccountNumber").val(data.AccountNumber);
        BankTransfers.getForm().find('#IntermediaryBank').prop("checked", data.IntermediaryBank);
        BankTransfers.getForm().find('#Active').prop("checked", data.ActiveAccount);
        BankTransfers.getForm().find('#DefaultAccount').prop("checked", data.DefaultAccount);
    }

    static ClearControlBankTransfers() {
        BankTransfers.DisabledBankTransfersControl(false);
        rowBankTransfersId = -1;
        BankTransfersId = 0;
        BankTransfers.getForm().find("#inputBranchBank").val("");
        BankTransfers.getForm().find('#selectBank').UifSelect('setSelected', "");
        BankTransfers.getForm().find('#inputAccountNumber').val('');
        BankTransfers.getForm().find('#inputBankSquare').val('');
        BankTransfers.getForm().find('#inputAddress').val('');
        BankTransfers.getForm().find('#inputCity').val("");
        BankTransfers.getForm().find('#selectCountry').UifSelect('setSelected', "");
        BankTransfers.getForm().find('#selectAccountType').UifSelect('setSelected', "");
        BankTransfers.getForm().find('#selectCurrency').UifSelect('setSelected', "");
        BankTransfers.getForm().find('#IntermediaryBank').prop("checked", false);
        BankTransfers.getForm().find('#Active').prop("checked", false);
        BankTransfers.getForm().find('#DefaultAccount').prop("checked", false);
    }


    static DisabledBankTransfersControl(control) {
        $("#selectBank").prop('disabled', control);
        $("#selectBankSquare").prop('disabled', control);
        $("#selectAccountType").prop('disabled', control);
        $("#selectCurrency").prop('disabled', control);
        $('#inputAddress').prop('disabled', true);
        $('#selectCountry').prop('disabled', true);
        $('#inputCity').prop('disabled', true);

    }

    startAddListVBank() {
        var listbankTrasfers = $("#listVBankTransfers").UifListView("getData");
        var accountNumber = BankTransfers.getForm().find("#inputAccountNumber").val();
        var listbankTrasfersFilter = listbankTrasfers.filter(function (item) {
            return item.DefaultAccount == true;
        });
        var listbankTrasfersFilterRepeat = listbankTrasfers.filter(function (item) {
            return item.AccountNumber == accountNumber;
        });
        if (listbankTrasfersFilter.length > 0 && BankTransfers.getForm().find('#DefaultAccount').prop("checked") === true && rowBankTransfersId == -1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageDefaultAccount, 'autoclose': true });
            //alert('Ya existe una cuenta por defecto');
        } else if (listbankTrasfersFilterRepeat.length > 0 && rowBankTransfersId == -1) {
            $.UifNotify('show', { 'type': 'info', 'message': 'Datos de cuenta repetidos.', 'autoclose': true });
            //alert('El numero de cuenta ya existe')
        }
        else {
            if (BankTransfers.ValidateBankTransfers()) {inputAccountNumber
                BankTransfers.CreateBankTransfers();
                BankTransfers.ClearControlBankTransfers();
            }
            
        }
        
    }

    static ValidateBankTransfers() {

        var msj = "";

        if ($('input:radio[name=rbPaymentType]:checked').val() == "2") {

            if ($("#inputBeneficiaryType").val() == "" || $("#inputBeneficiaryType").val() == null) {
                msj = AppResourcesPerson.MessageBeneficiaryEmpty + "<br>"
            }
            if (BankTransfers.getForm().find("#selectBank").UifSelect("getSelected") == null || BankTransfers.getForm().find("#selectBank").UifSelect("getSelected") == "") {
                msj = msj + AppResourcesPerson.MessageBankEmpty + " <br>"
            }
            if ($("#inputBranchBank").val() == "" || $("#inputBranchBank").val() == null) {
                msj = msj + AppResourcesPerson.MessageBranchBankEmpty + " <br>"
            }
            if ($("#inputBankSquare").val() == "" || $("#inputBankSquare").val() == null) {
                msj = AppResourcesPerson.MessageBankSquareEmpty + "<br>"
            }
            //if ($("#inputAddress").val() == "" || $("#inputAddress").val() == null) {
            //    msj = AppResourcesPerson.MessageAddressEmpty + "<br>"
            //}
            //if ($("#selectCountry").UifSelect("getSelected") == null || $("#selectCountry").UifSelect("getSelected") == "") {
            //    msj = msj + AppResourcesPerson.MessageCountryEmpty + " <br>"
            //}
            //if ($("#inputCity").val("") == "") {
            //    msj = msj + AppResourcesPerson.MessageCityEmpty + " <br>"
            //}
            if (BankTransfers.getForm().find("#selectAccountType").UifSelect("getSelected") == null || BankTransfers.getForm().find("#selectAccountType").UifSelect("getSelected") == "") {
                msj = msj + AppResourcesPerson.MessageAccountTypeEmpty + " <br>"
            }
            if (BankTransfers.getForm().find("#inputAccountNumber").val() == "" || BankTransfers.getForm().find("#inputAccountNumber").val() == null) {
                msj = AppResourcesPerson.MessageAccountNumberEmpty + "<br>"
            }
            if ($("#selectCurrency").UifSelect("getSelected") == null || $("#selectCurrency").UifSelect("getSelected") == "") {
                msj = msj + AppResourcesPerson.MessageCurrencyEmpty + " <br>"
            }
            if (msj != "") {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true });
                return false;
            }
        }

        if ($('input:radio[name=rbPaymentType]:checked').val() == "1") {

            if ($("#inputBeneficiaryType").val() == "" || $("#inputBeneficiaryType").val() == null) {
                msj = AppResourcesPerson.MessageBeneficiaryEmpty + "<br>"
            }
            if (msj != "") {
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true });
                return false;
            }

        }
        return true;
    }

    static LoadBankTransfers() {
        $("#listVBankTransfers").UifListView({ sourceData: BankData, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#bankTransfersTemplate", height: heightListViewBusinessName });
    }


    static DisabledCombos() {
        BankTransfers.getForm().find('#selectBank').prop('disabled', true);
        BankTransfers.getForm().find('#inputBankSquare').prop('disabled', true);
        $('#inputAddress').prop('disabled', true);
        $('#selectCountry').prop('disabled', true);
        $('#inputCity').prop('disabled', true);
        $('#inputAddress').prop('disabled', true);
        $('#inputBranchBank').prop('disabled', true);
        BankTransfers.getForm().find('#selectAccountType').prop('disabled', true);
        BankTransfers.getForm().find('#inputAccountNumber').prop('disabled', true);
        $('#selectAccountType').prop('disabled', true);
        $('#inputAccountNumber').prop('disabled', true);
        $('#selectCurrency').prop('disabled', true);
        $('#IntermediaryBank').prop('disabled', true);
        $('#DefaultAccount').prop('disabled', true);
        $('#Active').prop('disabled', true);
        $("#btnBankAdd").prop('disabled', true);
        BankTransfers.getForm().find('#selectBank').UifSelect('setSelected', "");
        BankTransfers.getForm().find('#inputAccountNumber').val('');
        BankTransfers.getForm().find('#inputBankSquare').val('');
        BankTransfers.getForm().find('#inputAddress').val('');
        BankTransfers.getForm().find('#inputCity').val("");
        BankTransfers.getForm().find('#selectCountry').UifSelect('setSelected', "");
        BankTransfers.getForm().find('#selectAccountType').UifSelect('setSelected', "");
        BankTransfers.getForm().find('#selectCurrency').UifSelect('setSelected', "");
    }

    static enabledCombos() {
        BankTransfers.getForm().find('#selectBank').prop('disabled', false);
        $('#inputBranchBank').prop('disabled', false);
        BankTransfers.getForm().find('#inputBankSquare').prop('disabled', false);
        $('#inputAddress').prop('disabled', true);
        $('#selectCountry').prop('disabled', true);
        $('#inputCity').prop('disabled', true);
        BankTransfers.getForm().find('#selectAccountType').prop('disabled', false);
        BankTransfers.getForm().find('#inputAccountNumber').prop('disabled', false);
        $('#selectAccountType').prop('disabled', false);
        $('#inputAccountNumber').prop('disabled', false);
        $('#selectCurrency').prop('disabled', false);
        $('#IntermediaryBank').prop('disabled', false);
        $('#DefaultAccount').prop('disabled', false);
        $('#Active').prop('disabled', false);
        $("#btnBankAdd").prop('disabled', false);
    }



    static CreateBankTransfers() {
        if (BankTransfersId == 0) {
            BankTransfersId = 0;
        }
        var BankTransfersTmp = BankTransfers.CreateBankTransfersModel();

        if (rowBankTransfersId == -1) {
            $("#listVBankTransfers").UifListView("addItem", BankTransfersTmp);
        }
        else {
            $("#listVBankTransfers").UifListView("editItem", rowBankTransfersId, BankTransfersTmp);

        }
    }

}