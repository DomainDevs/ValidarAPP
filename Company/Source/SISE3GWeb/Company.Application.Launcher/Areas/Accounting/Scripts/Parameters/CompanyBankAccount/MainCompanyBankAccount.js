/**
    * @file   MainCompanyBankAccount.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var accountingAccountId = 0;
var accountingAccountNumber = "";
var accountingAccountName = "";
var companyBankAccountId = 0;
var disableDate;
var description = 0;
var date = "";
var status = 0;
var accountName = "";
var accountNumber = "";

var oCompanyBankAccount =
    {
        CompanyBankAccountId: 0,
        BankId: 0,
        BranchId: 0,
        AccountTypeId: 0,
        CurrencyId: 0,
        AccountNumber: null,
        Description: null,
        Enabled: 0,
        Default: 0,
        DisabledDate: 0,
        AccountingAccountId: 0,
        AccountingAccountNumber: null,
        AccountingAccountName:null,
    };

var saveMainBankAccount = function (deferred, data) {
    $("#alertCompanyBankAccount").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "BankAccounts/SaveCompanyBankAccount",
        data: JSON.stringify({ "companyBankAccountModel": MainCompanyBankAccount.SetCompanyBankAccount() }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data > 0) {
            deferred.resolve();
            $("#alertCompanyBankAccount").UifAlert('show', Resources.SaveSuccessfully, "success");
            $("#BankSelectBankAccount").removeAttr("disabled");
            $("#BranchSelect").removeAttr("disabled");
            MainCompanyBankAccount.RefreshCompanyBankAccount();
            MainCompanyBankAccount.InitializeDataCompanyBankAccount();
        } else if (data == -1) {
            $("#alertCompanyBankAccount").UifAlert('show', Resources.AccountNumberAlreadyExists, "danger");
        }
        else {
            deferred.reject();
            $("#alertCompanyBankAccount").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainCompanyBankAccount();
});

class MainCompanyBankAccount extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        var controller = ACC_ROOT + "Common/GetBranchs";
        $("#BranchSelect").UifSelect({ source: controller });
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#BranchSelect").on("itemSelected", this.BranchItemSelected);
        $("#BranchSelect").on("binded", this.BindedBranchDefault);
        $("#BankSelectBankAccount").on("itemSelected", this.BankItemSelected);
        $("#tableCompanyBankAccount").on("rowEdit", this.TableCompanyBankAccountRowEdit);
        $("#AccountBankRefresh").on("click", this.RefreshCompanyBankAccountTable);
        $("#statusBankAccount").on("itemSelected", this.StatusBankAccountItemSelected);
        $("#saveButtonBankAccount").on("click", this.SaveCompanyBankAccount);
        $("#tableCompanyBankAccount").on("rowAdd", this.ShowAddCompanyBankAccount);
        $("#MainCompanyBankAccountAddForm").find("#CompanyBanKAccountSaveAdd").on("click", this.SaveAddCompanyBankAccount);
        $("#modalAddCompanyBankAccount").find('#AccountingNumberBankAccount').on("itemSelected", this.AutocompletAccountingAccountNumber);
        $("#modalAddCompanyBankAccount").find('#AccountingNameBank').on("itemSelected", this.AutocompletAccountingAccountName);
    }

    /**
        * Setea la sucursal por default una vez que esta cargado.
        *
        */
    BindedBranchDefault() {
        if ($("#ViewBagBranchDisableBankAccount").val() == "1") {
            $("#BranchSelect").attr("disabled", "disabled");
        }
        else {
            $("#BranchSelect").removeAttr("disabled");
        }
        MainCompanyBankAccount.GetBanks();
    }

    
    //autocomplete por número de cuenta
    AutocompletAccountingAccountNumber(event, selectedItem) {
        accountingAccountId = selectedItem.AccountingAccountId;
        $('#AccountingNumberBankAccount').val(selectedItem.AccountingNumber);               
        $('#AccountingNameBank').val(selectedItem.AccountingName);
    }
    //autocomplete por nombre de cuenta
    AutocompletAccountingAccountName(event, selectedItem) {
        accountingAccountId = selectedItem.AccountingAccountId;
        $('#AccountingNumberBankAccount').val(selectedItem.AccountingNumber);
        $('#AccountingNameBank').val(selectedItem.AccountingName);
   }


    /**
        * Obtiene los bancos parametrizados para la sucursal seleccionada.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la sucursal seleccionada.
        */
    BranchItemSelected(event, selectedItem) {
        $("#alertCompanyBankAccount").UifAlert('hide');

        if ($('#BranchSelect').val() > 0) {
            var controller = ACC_ROOT + "Parameters/GetBanksByBranchId?branchId=" + $('#BranchSelect').val();
            $("#BankSelectBankAccount").UifSelect({ source: controller });
        }
        else {
            $("#BankSelectBankAccount").UifSelect();
            $("#tableCompanyBankAccount").find('.cancel-button').click();
            $("#tableCompanyBankAccount").UifSelect({ source: null });
            $('.add-button').hide();
        }
    }

    /**
        * Obtiene las cuentas bancarias parametrizados para el banco seleccionado.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del banco seleccionado.
        */
    BankItemSelected(event, selectedItem) {
        $("#alertCompanyBankAccount").UifAlert('hide');
        $("#tableCompanyBankAccount").UifDataTable()
        if ($('#BankSelectBankAccount').val()!= "") {
            
                var controller = ACC_ROOT + "BankAccounts/GetCompanyBankAccountByBranchIdBankId?branchId="
                                + $('#BranchSelect').val() + "&bankId=" + $('#BankSelectBankAccount').val();
                $('#tableCompanyBankAccount').UifDataTable({ source: controller })
            }
        
        else {
            $('#tableCompanyBankAccount').dataTable().fnClearTable();
        }
    }

    //Levanta modal selecciona banco
    TableCompanyBankAccountRowEdit(event, data, position){
        $("#alertCompanyBankAccount").UifAlert('hide');
        $("#modalCompanyBankAccount").find("#Description").val(data.Description);
        //isEdit = true;
        companyBankAccountId = data.CompanyBankAccountId;
        disableDate = data.DisableDate;
        MainCompanyBankAccount.BindingStatus(data.Enabled);
            $('#modalCompanyBankAccount').UifModal('showLocal', Resources.EditCompanyBankAccount);
            $("#modalCompanyBankAccount").find("#dateBankAccount").val("");
            if (data.DisableDate != "") {
                $("#modalCompanyBankAccount").find("#dateBankAccount").val(disableDate);
            }
    }

    /**
        * Refresca la tabla de cuentas bancarias de la compañía.
        *
        */
    static RefreshCompanyBankAccount() {
        var controller = ACC_ROOT + "BankAccounts/GetCompanyBankAccountByBranchIdBankId?branchId="
                                + $('#BranchSelect').val() + "&bankId=" + $('#BankSelectBankAccount').val();
        $('#tableCompanyBankAccount').UifDataTable({ source: controller })
    }
    /**
        * Obtiene los bancos parametrizados para la sucursal seleccionada.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la sucursal seleccionada.
        */
    StatusBankAccountItemSelected(event, selectedItem) {
        $("#alertCompanyBankAccountModal").UifAlert('hide');
        if ($('#statusBankAccount').val() != 0 || $('#statusBankAccount').val() == null || $('#statusBankAccount').val() == "") {
            $("#dateBankAccount").attr("disabled", "disabled");
            $("#dateBankAccount").val("");
        }
        else {
            $("#dateBankAccount").attr("disabled", false);
        }
    }

    /**
        * Graba una cuenta bancaria de la compañía.
        *
        */
    SaveCompanyBankAccount() {
        $("#editCompanyBankAccount").validate();

        if ($("#editCompanyBankAccount").valid()) {
            $("#alertCompanyBankAccountModal").UifAlert('hide');
            description = $('#modalCompanyBankAccount').find('#Description').val();
            date = $('#modalCompanyBankAccount').find('#dateBankAccount').val();
            status = $('#modalCompanyBankAccount').find('#statusBankAccount').val();

            if (MainCompanyBankAccount.ValidateDisabledDate()) {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "BankAccounts/UpdateCompanyBankAccount",
                    data: JSON.stringify({
                        "companyBankAccountId": companyBankAccountId, "description": description,
                        "disabledDate": date, "enabled": status
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $("#editCompanyBankAccount").formReset();
                        $('#modalCompanyBankAccount').UifModal('hide');
                        //$("#companyBankAccountListView").UifListView('refresh');

                        $("#alertCompanyBankAccount").UifAlert('show', Resources.EditSuccessfully, "success");
                        MainCompanyBankAccount.RefreshCompanyBankAccount();
                        MainCompanyBankAccount.InitializeDataCompanyBankAccount();
                    }
                });
            }
            else {
                $("#alertCompanyBankAccountModal").UifAlert('show', Resources.DateRequired, "danger");
            }
        }
    }

    /**
        * Abre el modal de inserción de una cuenta bancarai de la compañía.
        *
        * @param {String} event        - Agregar.
        */
    ShowAddCompanyBankAccount(event) {
        if ($('#BankSelectBankAccount').val() != null) {
            $("#modalAddCompanyBankAccount").find("#AccountTypeId").val("");
            $("#modalAddCompanyBankAccount").find("#AccountNumber").val("");
            $("#modalAddCompanyBankAccount").find("#Enabled").val("");
            $("#modalAddCompanyBankAccount").find("#CurrencyId").val("");
            $("#modalAddCompanyBankAccount").find("#AccountingNumberBankAccount").val("");
            $("#modalAddCompanyBankAccount").find("#AccountingNameBank").val("");

            $('#modalAddCompanyBankAccount').UifModal('showLocal', Resources.AddCompanyBankAccount);
        }
        else {
            $("#alertCompanyBankAccount").UifAlert('show', "Se requiere elegir banco para creacion de cuenta bancaria", "danger");}
        
    }

    /**
        * Graba una cuenta bancaria de la compañia.
        *
        */
    SaveAddCompanyBankAccount() {
        $("#alertAddCompanyBankAccount").UifAlert('hide');

        $("#MainCompanyBankAccountAddForm").validate();

        if ($("#MainCompanyBankAccountAddForm").valid()) {

            if (accountingAccountId === 0 || accountingAccountId === null || accountingAccountId === undefined) {
                $("#alertAddCompanyBankAccount").UifAlert('show', Resources.ValidateExistAccountingAcount, "warning");
            }
            else {
                if (MainCompanyBankAccount.ValidateAccountNumber($("#modalAddCompanyBankAccount").find("#AccountNumber").val()) == false) {
                    $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "BankAccounts/SaveCompanyBankAccount",
                        data: JSON.stringify({ "companyBankAccountModel": MainCompanyBankAccount.SetCompanyBankAccount() }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    }).done(function (data) {
                        if (data > 0) {
                            $('#modalAddCompanyBankAccount').UifModal('hide');
                            $("#alertCompanyBankAccount").UifAlert('show', Resources.SaveSuccessfully, "success");
                            $("#BankSelectBankAccount").removeAttr("disabled");
                            $("#BranchSelect").removeAttr("disabled");
                            MainCompanyBankAccount.RefreshCompanyBankAccount();
                            MainCompanyBankAccount.InitializeDataCompanyBankAccount();
                        } else if (data == -1) {
                            $("#alertAddCompanyBankAccount").UifAlert('show', Resources.AccountNumberAlreadyExists, "danger");
                        }
                        else {
                            $("#alertAddCompanyBankAccount").UifAlert('show', Resources.SaveError, "danger");
                        }
                    });
                }
                else {
                    $("#alertAddCompanyBankAccount").UifAlert('show', Resources.AccountNumberAlreadyExists, "danger");
                }
            }
            
        }
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Obtiene los bancos parametrizados para una sucursal.
        *
        */
    static GetBanks() {
        if ($('#BranchSelect').val() > 0) {
            var controller = ACC_ROOT + "Parameters/GetBanksByBranchId?branchId=" + $('#BranchSelect').val();
            $("#BankSelectBankAccount").UifSelect({ source: controller });
        }
    }

    /**
        * Valida el ingreso de la fecha de deshabilitación de la cuenta bancaria.
        *
        */
    static ValidateDisabledDate() {
        var isValidate = true;
        if (status == "0") {// && (date == "" || date == null)) {
            if (date == "" || date == null) {
                isValidate = false;
            }
        }
        else if (status == "") {
            isValidate = false;
        }

        return isValidate;
    }

    /**
        * Habilita / deshabilita la fecha al editar una cuenta bancaria.
        *
        * @param {Number} statusCode - Identificador de estado.
        */
    static BindingStatus(statusCode) {
        var statusId = statusCode ? 1 : 0;

        var controller = ACC_ROOT + "Parameters/GetStatus";
        $("#modalCompanyBankAccount").find('#statusBankAccount').UifSelect({ source: controller, selectedId: statusId });

        if (statusId == 0) {
            $("#dateBankAccount").attr("disabled", false);
        }
        else {
            $("#dateBankAccount").attr("disabled", "disabled");
        }
    }

    /**
        * Setea el modelo de cuentas bancarias de la compañía.
        *
        */
    static SetCompanyBankAccount() {
        oCompanyBankAccount.CompanyBankAccountId = 0;
        oCompanyBankAccount.AccountTypeId = $("#modalAddCompanyBankAccount").find("#AccountTypeId").val();
        oCompanyBankAccount.AccountNumber = $("#modalAddCompanyBankAccount").find("#AccountNumber").val();
        oCompanyBankAccount.BankId = $("#BankSelectBankAccount").val();
        oCompanyBankAccount.Enabled = $("#modalAddCompanyBankAccount").find("#Enabled").val();
        oCompanyBankAccount.Default = 1;
        oCompanyBankAccount.CurrencyId = $("#modalAddCompanyBankAccount").find("#CurrencyId").val();
        oCompanyBankAccount.DisabledDate = ""
        oCompanyBankAccount.BranchId = $("#BranchSelect").val();
        oCompanyBankAccount.Description = "";
        oCompanyBankAccount.AccountingAccountId = accountingAccountId;
        return oCompanyBankAccount;
    }

    /**
        * Limpia el modelo de cuentas bancarias de la compañía.
        *
        */
    static InitializeDataCompanyBankAccount() {
        oCompanyBankAccount =
        {
            CompanyBankAccountId: 0,
            BankId: 0,
            BranchId: 0,
            AccountTypeId: 0,
            CurrencyId: 0,
            AccountNumber: null,
            Description: null,
            Enabled: 0,
            Default: 0,
            DisabledDate: 0,
            AccountingAccountId:0,
            AccountingAccountNumber: null,
            AccountingAccountName:null,
        };
    }

    /**
        * Permite validar que no ingrese el mismo número de cuenta.
        *
        * @param {Number} accountNumber - Número de cuenta avalidar.
        */
    static ValidateAccountNumber(accountNumber) {

        var exists = 0;

        var ids = $("#tableCompanyBankAccount").UifDataTable('getData');
        if (ids.length > 0) {
            for (var i in ids) {
                if (accountNumber == ids[i].AccountNumber) {
                    exists = 1;
                    break;
                }
            }
        }
        return exists;
    }

    static validateNumber(event,obj) {
        accountingAccountId = 0;
        return JustNumbers(event, obj)
    }
    
}