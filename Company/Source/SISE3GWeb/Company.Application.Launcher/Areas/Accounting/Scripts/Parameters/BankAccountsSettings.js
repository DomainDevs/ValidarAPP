/**
    * @file   BankAccountsSettings.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var bankAccountSettingsAlert = 0;
var bankAccountSettingsNumerrors = 0;
var bankAccountSettingsValidate = "New";
var bankAccountSettingsIndividualId = 0;
var bankAccountSettingsBankId = 0;
var bankAccountSettingsBranchId = 0;
var bankAccountSettingsAccountBankId = 0;
var totalRows = 0;
var bankAccountSettingsDocumentNumber = 0;
var bankAccountSettingsAccountName = '';
var bankAccountSettingsNumber = "";
var bankAccountSettingsName = "";

var oBankAccount =
    {
        BankAccountCode: 0,
        IndividualId: 0,
        AccountTypeCode: 0,
        Number: 0,
        BankCode: 0,
        Enabled: 0,
        Default: 0,
        CurrencyCode: 0

    };

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainPersonBankAccount();
});

class MainPersonBankAccount extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        $("#BankAccountsForm1").hide();
        $("#BankAccountsForm2").hide();
        $("#BankAccountsForm3").hide();
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#NewAccountAccountSettings").on('click', this.ShowBankAccount);
        $('#tableBankAccounts').on('rowAdd', this.ShowBankAccount);
        $('#numberAccountAccountantAccountSettings').on('itemSelected', this.DocumentNumberAutocomplete);
        $('#nameAccountAccountantAccountSettings').on('itemSelected', this.PersonNameAutocomplete);
        $('#bankNameAccountSettings').on('itemSelected', this.BankAutocomplete);
        $("#SearchAccountAccountSettings").on('click', this.SearchBankAccounts);
        $("#numberAccountAccountantAccountSettings").on('blur', this.BlurDocumentNumber);
        $("#nameAccountAccountantAccountSettings").on('blur', this.BlurPersonName);
        $('#tableBankAccounts').on('rowDelete', this.ShowConfirmDeleteBankAccount);
        $("#BankAccountsDeleteAccountDialog").on('click', this.DeleteBankAccount);
        $('#tableBankAccounts').on('rowEdit', this.EditBankAccount);
        $("#AcceptAccountSettings").on('click', this.SaveBankAccount);
        $("#CancelAccountSettings").on('click', this.CancelSaveBankAccount);
    }

    /**
       * Visualiza pantalla de creación de cuentas.
       *
       */
    ShowBankAccount() {
        if ($("#numberAccountAccountantAccountSettings").val() != "") {
            $('#BankAccountsForm1').show();
            $('#BankAccountsForm2').show();
            $('#BankAccountsForm3').show();
            $('#BankAccountsForm4').hide();
            MainPersonBankAccount.EnabledModified();
            bankAccountSettingsValidate = "New";
        }
    }

    /**
        * Obtiene las cuentas bancarias de una persona.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la persona seleccionada.
        */
    DocumentNumberAutocomplete(event, selectedItem) {
        $("#nameAccountAccountantAccountSettings").val(selectedItem.Name);
        $("#numberAccountAccountantAccountSettings").val(selectedItem.DocumentNumber);
        bankAccountSettingsNumber = selectedItem.DocumentNumber;
        bankAccountSettingsName = selectedItem.Name;
        bankAccountSettingsIndividualId = selectedItem.IndividualId;
        MainPersonBankAccount.GetBankAccounts();
    }

    /**
        * Obtiene las cuentas bancarias de una persona.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la persona seleccionada.
        */
    PersonNameAutocomplete(event, selectedItem) {
        $("#nameAccountAccountantAccountSettings").val(selectedItem.Name);
        $("#numberAccountAccountantAccountSettings").val(selectedItem.DocumentNumber);
        bankAccountSettingsNumber = selectedItem.DocumentNumber;
        bankAccountSettingsName = selectedItem.Name;
        bankAccountSettingsIndividualId = selectedItem.IndividualId;
        MainPersonBankAccount.GetBankAccounts();
    }

    /**
        * Obtiene las bancos.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del banco seleccionado.
        */
    BankAutocomplete(event, selectedItem) {
        bankAccountSettingsBankId = selectedItem.bankId;
    }

    /**
       * Busca las cuentas bancarias de una persona.
       *
       */
    SearchBankAccounts() {
        MainPersonBankAccount.BankAccountSettingsRefreshGrid();
        $("#alertBankAccountSettings").UifAlert('hide');
    }

    /**
        * Setea el número de documento de la persona.
        *
        * @param {String} event        - Cambio de foco.
        */
    BlurDocumentNumber(event) {
        setTimeout(function () {
            $("#numberAccountAccountantAccountSettings").val(bankAccountSettingsNumber);
        }, 50);
    }

    /**
        * Setea el nombre de la persona.
        *
        * @param {String} event        - Cambio de foco.
        */
    BlurPersonName(event) {
        setTimeout(function () {
            $("#nameAccountAccountantAccountSettings").val(bankAccountSettingsName);
        }, 50);
    }

    /**
        * Visualiza mensaje de confirmación de eliminación de una cuenta bancaria de la persona.
        *
        * @param {String} event    - Eliminar.
        * @param {Object} data     - Objeto con valores de la cuenta bancaria seleccionada.
        * @param {Number} position - Indice de la fila seleccionada.
        */
    ShowConfirmDeleteBankAccount(event, data, position) {
        oBankAccount.BankAccountCode = data.BankAccountCode;
        oBankAccount.IndividualId = data.IndividualId;

        $('#BankAccountsDeleteDialog').appendTo("body").UifModal('showLocal');
    }

    /**
        * Elimina una cuenta bancaria de la persona.
        *
        */
    DeleteBankAccount() {
        var accountDelete = new Promise(function (resolve, deferred) {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "Parameters/DeleteAccountBank",
                data: JSON.stringify({ "accountBank": oBankAccount }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    resolve(data);
                }
            });
        });

        accountDelete.then(function (data) {
            if (data) {
                MainPersonBankAccount.BankAccountSettingsRefreshGrid();
                MainPersonBankAccount.SetDataAccountBankEmpty();
                $("#alertBankAccountSettings").UifAlert('show', Resources.DeleteSucces, "success");
                $("#BankAccountsDeleteDialog").modal('hide');
            }
        });
    }

    /**
        * Visualiza la edición de una cuenta bancaria de la persona.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores de la cuenta bancaria seleccionada.
        * @param {Number} position - Indice de la fila seleccionada.
        */
    EditBankAccount(event, data, position) {

        $('#BankAccountsForm1').show();
        $('#BankAccountsForm2').show();
        $('#BankAccountsForm3').show();
        $('#BankAccountsForm4').hide();
        
        $("#nameAccountAccountantAccountSettings").val(data.PersonName);
        $("#numberAccountAccountantAccountSettings").val(data.DocumentNumber);
        $("#AccountTypeAccountSettings").val(data.AccountTypeCode);
        $("#accountNumberAccountSettings").val(data.AccountNumber);
        $("#bankNameAccountSettings").val(data.BankDescription);
        $("#CurrencyAccountSettings").val(data.CurrencyCode);
        if (data.Default == true) {
            $("#checkDefaultAccountSettings").prop("checked", true);
        }
        if (data.Enabled == true) {
            $("#checkEnableAccountSettings").prop("checked", true);
        }
        bankAccountSettingsBankId = data.BankCode;
        bankAccountSettingsIndividualId = data.IndividualId;
        bankAccountSettingsAccountBankId = data.BankAccountCode;
        bankAccountSettingsValidate = "Modify";
    }

    /**
        * Graba una cuenta bancaria de la persona.
        *
        */
    SaveBankAccount() {

        $("#MainPersonBankAccountAddForm").validate();

        if ($("#MainPersonBankAccountAddForm").valid()) {
            bankAccountSettingsNumerrors = 0;

            if ((bankAccountSettingsNumerrors == 0) || (bankAccountSettingsNumerrors == undefined)) {
                bankAccountSettingsAlert = 0;
            } else {
                bankAccountSettingsAlert = 1;
                bankAccountSettingsNumerrors = 0;
            }
            if (bankAccountSettingsAlert == 0 && $("#AccountTypeAccountSettings").val() != "" &&
                        $("#CurrencyAccountSettings").val() != "" &&
                        $("#bankNameAccountSettings").val() != "" &&
                        $("#accountNumberAccountSettings").val() != "") {
                if (bankAccountSettingsValidate == "New") {
                    if (MainPersonBankAccount.ValidateExistAccount()) {

                    } else {

                        var saveAccountBank = new Promise(function (resolve, reject) {
                            $.ajax({
                                type: "POST",
                                url: ACC_ROOT + "Parameters/SaveAccountBank",
                                data: JSON.stringify({ "accountBank": MainPersonBankAccount.SetDataAccountBank() }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    resolve(data);
                                }
                            });
                        });

                        saveAccountBank.then(function (data) {
                            if (data > 0) {
                                MainPersonBankAccount.SetDataAccountBankEmpty();
                                MainPersonBankAccount.BankAccountSettingsRefreshGrid();
                                MainPersonBankAccount.ClearAfterSubmit();
                                $("#alertBankAccountSettings").UifAlert('show', Resources.AccountBankSaveSuccess, "success");
                            }
                        });
                    }
                }
                if (bankAccountSettingsValidate == "Modify") {

                    var updateAccountBank = new Promise(function (resolve, reject) {
                        $.ajax({
                            type: "POST",
                            url: ACC_ROOT + "Parameters/UpdateAccountBankSettings",
                            data: JSON.stringify({ "accountBank": MainPersonBankAccount.SetDataAccountBank() }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                resolve(data);
                            }
                        });
                    });

                    updateAccountBank.then(function (data) {
                        if (data > 0) {
                            MainPersonBankAccount.SetDataAccountBankEmpty();
                            MainPersonBankAccount.BankAccountSettingsRefreshGrid();
                            MainPersonBankAccount.ClearAfterSubmit();
                            bankAccountSettingsValidate = "New";
                            $("#alertBankAccountSettings").UifAlert('show', Resources.AccountBankSaveSuccess, "success");
                        }
                    });
                }

            } else {
                $("#alertBankAccountSettings").UifAlert('show', Resources.RequiredFieldsMissing, "warning");
            }
        }
    }

    /**
        * Cancela la grabación una cuenta bancaria de la persona.
        *
        */
    CancelSaveBankAccount() {
        MainPersonBankAccount.BankAccountSettingsShowConfirm();
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Visualiza el mensaje de confirmación de eliminación.
        *
        */
    static BankAccountSettingsShowConfirm() {
        $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': 'Cuentas Bancarias' }, function (result) {
            if (result) {
                MainPersonBankAccount.BankAccountSettingsClearFields();
                $('#BankAccountsForm1').hide();
                $('#BankAccountsForm2').hide();
                $('#BankAccountsForm3').hide();
                $('#BankAccountsForm4').show();
            }
        });
    };

    /**
        * Refresca la tabla de cuentas bancarias de una persona.
        *
        */
    static BankAccountSettingsRefreshGrid() {

        bankAccountSettingsDocumentNumber = $("#numberAccountAccountantAccountSettings").val();
        var controller = ACC_ROOT + "Parameters/SearchBankAccounts?documentNumber=" + bankAccountSettingsDocumentNumber;
        $("#tableBankAccounts").UifDataTable({ source: controller });
    };

    /**
        * Habilita / deshabilita los objetos de la pantalla.
        *
        */
    static EnabledModified() {
        $("#BranchAccountSettings").removeAttr("disabled");
        $("#AccountTypeAccountSettings").removeAttr("disabled");
        $("#CurrencyAccountSettings").removeAttr("disabled");
        $("#bankNameAccountSettings").removeAttr("disabled");
        $("#accountNumberAccountSettings").removeAttr("disabled");
        $("#numberAccountAccountantAccountSettings").removeAttr("disabled");
        $("#nameAccountAccountantAccountSettings").removeAttr("disabled");
        $("#checkEnableAccountSettings").removeAttr("disabled");
        $("#dateLowAccountSettings").removeAttr("disabled");
        $("#observationAccountAccountSettings").removeAttr("disabled");
    }

    /**
        * Valida si ya existe un registro en la tabla.
        *
        */
    static ValidateExistAccount() {

        var result = false;

        var idsTable = $("#tableBankAccounts").UifDataTable("getData");

        for (var i = 0; i < idsTable.length; i++) {
            //  trae el valor de una celda
            if ($("#accountNumberAccountSettings").val() == idsTable[i].AccountNumber) {
                $("#alertBankAccountSettings").UifAlert('show', Resources.AccountNumberAlreadyExists, "warning");
                result = true;
            }
        }
        return result;
    }

    /**
        * Setea el objeto de cuentas bancarias.
        *
        */
    static SetDataAccountBank() {

        bankAccountSettingsAccountName = $("#nameAccountAccountantAccountSettings").val();
        bankAccountSettingsDocumentNumber = $("#numberAccountAccountantAccountSettings").val();

        oBankAccount.AccountTypeCode = $("#AccountTypeAccountSettings").val();
        oBankAccount.IndividualId = $("#individualIdAccountSettings").val();

        if ($("#checkEnableAccountSettings").is(":checked")) {
            oBankAccount.Enabled = true;
        } else {
            oBankAccount.Enabled = false;
        }

        if ($("#checkDefaultAccountSettings").is(":checked")) {
            oBankAccount.Default = true;
        } else {
            oBankAccount.Default = false;
        }
        oBankAccount.Number = $("#accountNumberAccountSettings").val();
        oBankAccount.CurrencyCode = $("#CurrencyAccountSettings").val();
        oBankAccount.BankDescription = $("#bankNameAccountSettings").val();
        oBankAccount.IndividualId = bankAccountSettingsIndividualId;
        oBankAccount.BankCode = bankAccountSettingsBankId;
        oBankAccount.BankAccountCode = bankAccountSettingsAccountBankId;

        if (bankAccountSettingsValidate == "Modify") {

        } else {
            oBankAccount.AccountBankCode = 0;
        }

        return oBankAccount;
    }

    /**
        * Setea el objeto de cuentas bancarias con valors vacíos.
        *
        */
    static SetDataAccountBankEmpty() {
        oBankAccount =
        {
            BankAccountCode: 0,
            IndividualId: 0,
            AccountTypeCode: 0,
            Number: 0,
            BankCode: 0,
            Enabled: true,
            Default: true,
            CurrencyCode: 0
        };
    }

    /**
        * Limpia los campos de la pantalla de cuentas bancarias.
        *
        */
    static BankAccountSettingsClearFields() {
        $("#AccountTypeAccountSettings").val("");
        $("#CurrencyAccountSettings").val("");
        $("#bankNameAccountSettings").val("");
        $("#checkEnableAccountSettings").attr('checked', false);
        $("#checkDefaultAccountSettings").attr('checked', false);
        $("#accountNumberAccountSettings").val("");
        $("#numberAccountAccountantAccountSettings").val("");
        $("#nameAccountAccountantAccountSettings").val("");
        $("#tableBankAccounts").dataTable().fnClearTable();
    }

    /**
        * Limpia los campos de la pantalla de cuentas bancarias después de grabar.
        *
        */
    static ClearAfterSubmit() {
        $("#AccountTypeAccountSettings").val(-1);
        $("#CurrencyAccountSettings").val(-1);
        $("#bankNameAccountSettings").val('');
        $("#checkEnableAccountSettings").attr('checked', false);
        $("#checkDefaultAccountSettings").attr('checked', false);
        $("#accountNumberAccountSettings").val("");

        $('#BankAccountsForm1').hide();
        $('#BankAccountsForm2').hide();
        $('#BankAccountsForm3').hide();
        $('#BankAccountsForm4').show();
    }

    /**
       * Busca las cuentas bancarias de una persona.
       *
       */
    static GetBankAccounts() {
        MainPersonBankAccount.BankAccountSettingsRefreshGrid();
        $("#alertBankAccountSettings").UifAlert('hide');
    }
}













