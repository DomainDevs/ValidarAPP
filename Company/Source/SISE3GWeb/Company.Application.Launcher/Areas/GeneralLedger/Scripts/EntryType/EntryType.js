/**
    * @file   MainLedgerEntry.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var ModelType = {
    EntryTypeId: 0,
    EntryTypeDescription: "",
    EntryTypeSmallDescription: "",
};

var ModelEntryType = {
    EntryTypeAccountingId: 0,
    EntryTypeCd: 0,
    Description: "",
    AccountingNatureId: 0,
    CurrencyId: 0,
    AccountingAccountId: 0,
    AccountingMovementTypeId: 0,
    AnalysisId: 0,
    CostCenterId: 0,
    PaymentConceptCd: 0
};

var idEntryType = 0;
var entryTypeAccountId = 0;
var accountNumberName = $('#accountingModal').find('#AccountingAccountDescription').val();

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainEntryType();
});

class MainEntryType extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#btnDeleteModalAcc").on("click", this.DeleteModalEntryTypeItem);
        $("#modalEntryTpe").on('click', '#saveEntryType', this.SaveEntryType);
        $('#accountingModal').on('itemSelected', '#AccountingAccountDescription', this.AutocompleteAccountingAccountDescription);
        $("#modalEntryTpe").on('keypress', '#AccountingAccountDescription', this.KeyPresAccountingAccountDescription);
        $("#accountingModal").on('blur', '#AccountingAccountDescription', this.BlurAccountingAccountDescription);
        $("#accountingModal").on('click', '#saveButtonAEntryType', this.SaveButtonEntryType);
        $("#tblEntryType").on('rowSelected', this.RowSelectedTableEntryType);
        $("#tblEntryType").on('rowAdd', this.RowAddTableEntryType);
        $('#tblEntryType').on('rowEdit', this.RowEditTableEntryType);
        $('#tblEntryType').on('rowDelete', this.RowDeleteTableEntryType);
        $("#tblAccount").on('rowAdd', this.RowAddTableEntryTypeItem);
        $('#tblAccount').on('rowEdit', this.RowEditTableEntryTypeItem);
        $('#tblAccount').on('rowDelete', this.RowDeleteTableEntryTypeItem);
        $('#btnDeleteModal').on("click", this.DeleteModalEntryType);
    }

    /**
        * Elimina un detalle de asiento tipo.
        *
        */
    DeleteModalEntryTypeItem() {
        $('#modalDeleteAcc').modal('hide');

        $.ajax({
            type: "POST",
            url: "DeleteEntryTypeAccount",
            data: JSON.stringify({ "id": entryTypeAccountId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data == true) {
                var controller = GL_ROOT + "EntryType/GetAccountsByEntryType?entryTypeId=" + $('#tempEntryTypeId').val();
                $("#tblAccount").UifDataTable({ source: controller });

                $("#alertAccount").UifAlert('show', Resources.DeleteSuccessfully, "success");
            }
            else {
                $("#alertAccount").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
            }
        });
    }

    /**
        * Graba un asiento tipo.
        *
        */
    SaveEntryType() {
        $('#modalEntryTpe').find("#addEntryType").validate();
        if (!$('#modalEntryTpe').find("#addEntryType").valid()) {
            return false;
        }
        else {
            ModelType.EntryTypeId = $('#modalEntryTpe').find("#EntryTypeId").val();
            ModelType.EntryTypeDescription = $('#modalEntryTpe').find("#EntryTypeDescription").val();
            ModelType.EntryTypeSmallDescription = $('#modalEntryTpe').find("#EntryTypeSmallDescription").val();

            var token = $('#modalEntryTpe').find('input[name=__RequestVerificationToken]').val();

            $.ajax({
                type: "POST",
                url: 'SaveEntryType',
                data: JSON.stringify({ "entryTypeModel": ModelType }),
                headers: { "__RequestVerificationToken": token },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    $('#modalEntryTpe').UifModal('hide');
                    $("#tblEntryType").UifDataTable();
                    $('#tempEntryTypeId').val(0);
                    $("#tblAccount").UifDataTable('clear')

                    if (data == 0) {
                        $("#alertEntryType").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertEntryType").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    }

    /**
        * Permite buscar las cuentas contables por descripción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la cuenta contable seleccionada.
        */
    AutocompleteAccountingAccountDescription(event, selectedItem) {
        accountNumberName = selectedItem.AccountingAccountNumberName;
        $('#accountingModal').find('#AccountingAccountDescription').val(accountNumberName);
        $('#accountingModal').find('#AccountingAccountId').val(selectedItem.AccountingAccountId);
    }


    /**
        * Permite ingresar solo números en la cuenta contable.
        *
        * @param {String} event        - Presionar tecla.
        */
    KeyPresAccountingAccountDescription(event) {
        if (event.which != 32 && event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            }
        }
    }

    /**
        * Pierde el foco el campo nombre cuenta contable.
        *
        * @param {String} event        - Pierde el foco.
        */
    BlurAccountingAccountDescription(event) {
        $("#alertEntryType").find('.close').click();
        setTimeout(function () {
            $('#accountingModal').find("#AccountingAccountDescription").val(accountNumberName);
        }, 50);
    }


    /**
        * Graba un asiento tipo.
        *
        */
    SaveButtonEntryType() {
        $('#accountingModal').find("#addEntryTypeAccount").validate();
        if (!$('#accountingModal').find("#addEntryTypeAccount").valid()) {
            return false;
        }
        else {
            ModelEntryType.EntryTypeAccountingId = $('#accountingModal').find("#EntryTypeAccountingId").val();
            ModelEntryType.EntryTypeCd = $('#accountingModal').find("#EntryTypeCd").val();
            ModelEntryType.Description = $('#accountingModal').find("#Description").val();
            ModelEntryType.AccountingNatureId = $("#accountingModal").find("#AccountingNatureId").val();
            ModelEntryType.CurrencyId = $('#accountingModal').find("#CurrencyId").val();
            ModelEntryType.AccountingAccountId = $('#accountingModal').find("#AccountingAccountId").val();
            ModelEntryType.AccountingMovementTypeId = $('#accountingModal').find("#AccountingMovementTypeId").val();
            ModelEntryType.AnalysisId = $('#accountingModal').find("#AnalysisId").val();
            ModelEntryType.CostCenterId = $('#accountingModal').find("#CostCenterId").val();
            ModelEntryType.PaymentConceptCd = $('#accountingModal').find("#PaymentConceptCd").val();

            $.ajax({
                type: "POST",
                url: 'SaveEntryTypeAccounting',
                data: JSON.stringify({ "entryTypeAccountingModel": ModelEntryType }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    $('#accountingModal').UifModal('hide');
                    var controller = GL_ROOT + "EntryType/GetAccountsByEntryType?entryTypeId=" + $('#tempEntryTypeId').val();
                    $("#tblAccount").UifDataTable({ source: controller });

                    if (ModelEntryType.EntryTypeAccountingId == 0) {
                        $("#alertAccount").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertAccount").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    }

    /**
        * Permite obtener el detalle del asiento tipo.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedRow  - Objeto con valores de la cabecera del asiento tipo.
        */
    RowSelectedTableEntryType(event, selectedRow) {
        $('#alertAccount').UifAlert('hide');
        $('#alertEntryType').UifAlert('hide');

        $('#tempEntryTypeId').val(selectedRow.EntryTypeId);
        var controller = GL_ROOT + "EntryType/GetAccountsByEntryType?entryTypeId=" + selectedRow.EntryTypeId;

        $("#tblAccount").UifDataTable({ source: controller });
    }


    /**
        * Permite agregar la cabecera del asiento tipo.
        *
        * @param {String} event - Seleccionar.
        * @param {Object} data  - Objeto con valores de la cabecera del asiento tipo.
        */
    RowAddTableEntryType(event, data) {
        $('#alertAccount').UifAlert('hide');
        $('#alertEntryType').UifAlert('hide');

        $('#modalEntryTpe').UifModal('show', GL_ROOT + "EntryType/EntryTypeModal", Resources.AddRecord);
    }

    /**
        * Permite editar la cabecera del asiento tipo.
        *
        * @param {String} event - Editar.
        * @param {Object} data  - Objeto con valores de la cabecera del asiento tipo.
        */
    RowEditTableEntryType(event, selectedRow) {
        $('#alertAccount').UifAlert('hide');
        $('#alertEntryType').UifAlert('hide');

        $('#modalEntryTpe').UifModal('show', GL_ROOT + "EntryType/EntryTypeModal?id=" + selectedRow.EntryTypeId, Resources.EditRecord);
    }

    /**
        * Permite eliminar la cabecera del asiento tipo.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores de la cabecera del asiento tipo.
        */
    RowDeleteTableEntryType(event, selectedRow) {
        $('#alertAccount').UifAlert('hide');
        $('#alertEntryType').UifAlert('hide');

        idEntryType = selectedRow.EntryTypeId;
        $('#modalDelete').modal('show');
    }

    /**
        * Permite agregar el detalle del asiento tipo.
        *
        * @param {String} event - Seleccionar.
        * @param {Object} data  - Objeto con valores del detalle del asiento tipo.
        */
    RowAddTableEntryTypeItem(event, data) {

        if ($('#tempEntryTypeId').val() == 0) {
            $("#alertAccount").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "danger");
        }
        else {
            $('#alertAccount').UifAlert('hide');
            $('#alertEntryType').UifAlert('hide');

            $('#accountingModal').UifModal('show', GL_ROOT + "EntryType/AccountingModal?entryTypeId=" + $('#tempEntryTypeId').val(), Resources.AddRecord);
        }
    }

    /**
        * Permite editar el detalle del asiento tipo.
        *
        * @param {String} event - Editar.
        * @param {Object} data  - Objeto con valores del detalle del asiento tipo.
        */
    RowEditTableEntryTypeItem(event, data) {

        $('#alertAccount').UifAlert('hide');
        $('#alertEntryType').UifAlert('hide');

        $('#accountingModal').UifModal('show', GL_ROOT + "EntryType/AccountingModal?id="
            + data.EntryTypeAccountingId + '&entryTypeId=' + data.EntryTypeCd, Resources.EditRecord);
    }

    /**
        * Permite eliminar el detalle del asiento tipo.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores del detalle del asiento tipo.
        */
    RowDeleteTableEntryTypeItem(event, selectedRow) {
        $('#alertAccount').UifAlert('hide');
        $('#alertEntryType').UifAlert('hide');

        entryTypeAccountId = selectedRow.EntryTypeAccountingId
        $('#modalDeleteAcc').modal('show');
    }

    
    /**
        * Permite eliminar el asiento tipo.
        *
        */
    DeleteModalEntryType() {
        if ($("#hiddenEntryType").val() != undefined) {
            $('#modalDelete').modal('hide');

            $.ajax({
                type: "POST",
                url: "DeleteEntryType",
                data: JSON.stringify({ "id": idEntryType }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data == true) {
                    $('#tblEntryType').UifDataTable();
                    $("#alertEntryType").UifAlert('show', Resources.DeleteSuccessfully, "success");
                }
                else {
                    $("#alertEntryType").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }

                $("#tblAccount").dataTable().fnClearTable();
                $('#tempEntryTypeId').val(0);
            });
        }
    }

}


