/**
    * @file   MainBankMovement.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

function RowModel() {
    this.Id;
    this.Description;
    this.ShortDescription;
    this.AccountingNatureCompany;
    this.IsDebitCompany;
    this.IsDebitBank;
}

var deleteRowModel = new RowModel();
var currentEditIndex = 0;
var index = 0;
var bankMovementId = 0;


/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainBankMovement();
});

class MainBankMovement extends Uif2.Page {

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
        $("#tableBankMovement").on("rowAdd", this.AddBankMovement);
        $("#MainBankMovementSaveAdd").on("click", this.SaveAddBankMovement);
        $("#tableBankMovement").on("rowEdit", this.EditBankMovement);
        $("#MainBankMovementSaveEdit").on("click", this.SaveEditBankMovement);
        $("#tableBankMovement").on("rowDelete", this.RowDeleteBankMovement);
        $("#MainBankMovementDeleteModal").on("click", this.DeleteBankMovement);
    }

    /**
        * Permite abrir el modal de agregación de movimiento bancario.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con valores del movimiento bancario seleccionado.
        */
    AddBankMovement(event, data) {
        $("#alertForm").UifAlert('hide');
        $('#MainBankMovementAlert').UifAlert('hide');
        MainBankMovement.GetMaximumCode();
        $("#MainBankMovementAddForm").find("#BankMovementCode").attr("disabled", "disabled");
        $('#MainBankMovementModalAdd').UifModal('showLocal', Resources.AddBankingMovement);
    }

    /**
        * Permite grabar un movimiento bancario.
        *
        */
    SaveAddBankMovement() {
        $("#MainBankMovementAddForm").validate();

        if ($("#MainBankMovementAddForm").valid()) {

            if (MainBankMovement.ValidateAddForm() == true) {
                var rowModel = new RowModel();

                rowModel.AccountingNatureCompany = $("#MainBankMovementAddForm").find("#selectAccountingNature").val();
                rowModel.Description = $("#MainBankMovementAddForm").find("#Description").val();
                rowModel.Id = $("#MainBankMovementAddForm").find("#BankMovementCode").val();
                if ($("#MainBankMovementAddForm").find('#selectAccountingNature').val() == 1) {
                    rowModel.IsDebitBank = 0;
                    rowModel.IsDebitCompany = 1;
                }
                else {
                    rowModel.IsDebitBank = 1;
                    rowModel.IsDebitCompany = 0;
                }
                rowModel.ShortDescription = $("#MainBankMovementAddForm").find("#ShortDescription").val();

                MainBankMovement.SaveBankMovement(rowModel, "I");

                $("#MainBankMovementAddForm").formReset();
                $('#MainBankMovementModalAdd').UifModal('hide');
            }            
        }
    }


    /**
        * Permite abrir el modal de edición del movimiento bancario seleccionado.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del movimiento bancario seleccionado.
        * @param {Object} position - Indice del movimiento bancario seleccionado.
        */
    EditBankMovement(event, data, position) {
        $("#alertForm").UifAlert('hide');
        $('#MainBankMovementAlert').UifAlert('hide');
        currentEditIndex = position;
        bankMovementId = data.Id;
        $("#MainBankMovementEditForm").find("#BankMovementCode").attr("disabled", "disabled");
        $("#MainBankMovementEditForm").find("#BankMovementCode").val(data.Id);
        $("#MainBankMovementEditForm").find("#Description").val(data.Description);
        $("#MainBankMovementEditForm").find("#ShortDescription").val(data.ShortDescription);
        $("#MainBankMovementEditForm").find("#selectAccountingNature").val(data.AccountingNatureId);

        $('#MainBankMovementModalEdit').UifModal('showLocal', Resources.EditBankingMovement);
    }

    /**
        * Permite grabar un movimiento bancario.
        *
        */
    SaveEditBankMovement() {
        $("#MainBankMovementEditForm").validate();

        if ($("#MainBankMovementEditForm").valid()) {

            if (MainBankMovement.ValidateEditForm() == true) {
                var rowModel = new RowModel();

                rowModel.AccountingNatureCompany = $("#MainBankMovementEditForm").find("#selectAccountingNature").val();
                rowModel.Description = $("#MainBankMovementEditForm").find("#Description").val();
                rowModel.Id = bankMovementId;
                if ($("#MainBankMovementEditForm").find('#selectAccountingNature').val() == 1) {
                    rowModel.IsDebitBank = 0;
                    rowModel.IsDebitCompany = 1;
                }
                else {
                    rowModel.IsDebitBank = 1;
                    rowModel.IsDebitCompany = 0;
                }
                rowModel.ShortDescription = $("#MainBankMovementEditForm").find("#ShortDescription").val();

                MainBankMovement.SaveBankMovement(rowModel, "U");

                $("#MainBankMovementEditForm").formReset();
                $('#MainBankMovementModalEdit').UifModal('hide');
            }            
        }
    }


    /**
        * Permite abrir el modal de confirmación de eliminación del movimiento bancario seleccionado.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores del movimiento bancario seleccionado.
        */
    RowDeleteBankMovement(event, data) {
        $('#MainBankMovementAlert').UifAlert('hide');
        $("#alertForm").UifAlert('hide');
        $("#selectedMessageDelete").text(data.Id + " - " + data.Description);
        $('#modalDeleteBankMovement').appendTo("body").modal('show');
        
        deleteRowModel.Id = data.Id;            
    }

    /**
        * Permite eliminar el movimiento bancario seleccionado.
        *
        */
    DeleteBankMovement() {
        $('#modalDeleteBankMovement').modal('hide');

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankMovement/DeleteBankMovement",            
            data: JSON.stringify({ "bankMovementModel": deleteRowModel, "operationType": "D" }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data[0].BankMovementCode == 0) {
                $("#MainBankMovementAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "BankMovement/GetBankMovements";
                $('#tableBankMovement').UifDataTable({ source: controller })
            }
            else {
                $("#MainBankMovementAlert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "danger");
            }
            $('#modalDeleteBankMovement').UifModal('hide');            
        });
    }


    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Graba, actualiza un movimiento bancario.
        *
        */
    static SaveBankMovement(rowModel, operationType) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankMovement/SaveBankMovement",
            data: JSON.stringify({ "bankMovementModel": rowModel, "operationType": operationType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data[0].BankMovementCode < 0) {
                    $("#MainBankMovementAlert").UifAlert('show', data[0].MessageError, "danger");
                }
                else {
                    $("#MainBankMovementAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "BankMovement/GetBankMovements";
                    $('#tableBankMovement').UifDataTable({ source: controller })
                }                
            }
        });
    }

    /**
        * Valida el ingreso de campos obligatorios en modal de agregar.
        *
        */
    static ValidateAddForm() {
        if ($("#MainBankMovementAddForm").find('#Description').val() == "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainBankMovementAddForm").find('#selectAccountingNature').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectTypeTax, "warning");
            return false;
        }

        return true;
    }

    /**
        * Valida el ingreso de campos obligatorios en modal de editar.
        *
        */
    static ValidateEditForm() {
        if ($("#MainNetworkEditForm").find('#Description').val() == "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainNetworkEditForm").find('#selectAccountingNature').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectTypeTax, "warning");
            return false;
        }

        return true;
    }

    /**
        * Obtiene el máximo código de movimiento bancario.
        *
        */
    static GetMaximumCode() {
        var rows = $("#tableBankMovement").UifDataTable("getData");
        var maximumCode = 0;

        for (var j in rows) {
            var item = rows[j];
            maximumCode = parseInt(item.Id);
        }
        maximumCode++;

        $("#MainBankMovementAddForm").find("#BankMovementCode").val(maximumCode);
    }

}
