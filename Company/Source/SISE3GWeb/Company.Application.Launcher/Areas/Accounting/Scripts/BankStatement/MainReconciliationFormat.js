/**
    * @file   MainReconciliationFormat.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

function RowModel() {
    this.Id;
    this.BankAccountCompanyId;
    this.FormatId;
    this.OperationType;
}

var deleteRowModel = new RowModel();
var currentEditIndex = 0;
var index = 0;
var reconciliationFormatId = 0;


/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainReconciliationFormat();
});

class MainReconciliationFormat extends Uif2.Page {

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
        $("#tableReconciliationFormat").on("rowAdd", this.AddReconciliationFormat);
        $("#ReconciliationFormatSaveAdd").on("click", this.SaveAddReconciliationFormat);
        $("#tableReconciliationFormat").on("rowEdit", this.EditReconciliationFormat);
        $("#ReconciliationFormatSaveEdit").on("click", this.SaveEditReconciliationFormat);
        $("#tableReconciliationFormat").on("rowDelete", this.RowDeleteReconciliationFormat);
        $("#ReconciliationFormatDeleteModal").on("click", this.DeleteReconciliationFormat);
    }

    /**
        * Permite abrir el modal de agregación de formato de conciliación bancaria.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con valores del formato de conciliación bancaria seleccionado.
        */
    AddReconciliationFormat(event, data) {
        $("#MainReconciliationFormatAddForm").find("#alertForm").UifAlert('hide');
        $('#MainReconciliationFormatAlert').UifAlert('hide');
        $("#MainReconciliationFormatAddForm").find("#selectBankReconciliation").val("");
        $("#MainReconciliationFormatAddForm").find("#selectFormat").val("");

        $('#MainReconciliationFormatModalAdd').UifModal('showLocal', Resources.AddReconciliationFormatAssignment);
    }

    /**
        * Permite grabar un formato de conciliación bancaria.
        *
        */
    SaveAddReconciliationFormat() {
        $("#MainReconciliationFormatAddForm").validate();

        if ($("#MainReconciliationFormatAddForm").valid()) {

            if (MainReconciliationFormat.ValidateDuplicateFormat($("#MainReconciliationFormatAddForm").find("#selectBankReconciliation").val(), $("#MainReconciliationFormatAddForm").find("#selectFormat").val()) == 0) {
                var rowModel = new RowModel();

                rowModel.BankAccountCompanyId = $("#MainReconciliationFormatAddForm").find("#selectBankReconciliation").val();
                rowModel.FormatId = $("#MainReconciliationFormatAddForm").find("#selectFormat").val();
                rowModel.Id = 0;
                rowModel.OperationType = "I";

                MainReconciliationFormat.SaveReconciliationFormat(rowModel, "I");

                $("#MainReconciliationFormatAddForm").formReset();
                $('#MainReconciliationFormatModalAdd').UifModal('hide');
            }
            else {
                $("#MainReconciliationFormatAddForm").find("#alertForm").UifAlert('show', Resources.MessageDuplicateReconciliationFormat, "warning");
            }
        }
    }


    /**
        * Permite abrir el modal de edición de formato de conciliación bancaria seleccionado.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del fromato de conciliación bancaria seleccionado.
        * @param {Object} position - Indice del fromato seleccionado.
        */
    EditReconciliationFormat(event, data, position) {
        $("#alertForm").UifAlert('hide');
        $('#MainReconciliationFormatAlert').UifAlert('hide');
        $("#MainReconciliationFormatEditForm").find("#alertForm").UifAlert('hide');
        currentEditIndex = position;
        reconciliationFormatId = data.Id;
        
        $("#MainReconciliationFormatEditForm").find("#selectFormat").val(data.FormatId);

        setTimeout(function () {
            $("#MainReconciliationFormatEditForm").find("#selectBankReconciliation").attr("disabled", "disabled");
            $("#MainReconciliationFormatEditForm").find("#selectBankReconciliation").val(data.BankAccountCompanyId);
        }, 300);

        $('#MainReconciliationFormatModalEdit').UifModal('showLocal', Resources.EditReconciliationFormatAssignment);
    }

    /**
        * Permite grabar un formato de conciliación bancaria.
        *
        */
    SaveEditReconciliationFormat() {
        $("#MainReconciliationFormatEditForm").validate();

        if ($("#MainReconciliationFormatEditForm").valid()) {

            if (MainReconciliationFormat.ValidateEditDuplicateRow($("#MainReconciliationFormatEditForm").find("#selectFormat").val()) == 0) {
                var rowModel = new RowModel();

                rowModel.BankAccountCompanyId = $("#MainReconciliationFormatEditForm").find("#selectBankReconciliation").val();
                rowModel.FormatId = $("#MainReconciliationFormatEditForm").find("#selectFormat").val();
                rowModel.Id = reconciliationFormatId;
                rowModel.OperationType = "U";

                MainReconciliationFormat.SaveReconciliationFormat(rowModel, "U");

                $("#MainReconciliationFormatEditForm").formReset();
                $('#MainReconciliationFormatModalEdit').UifModal('hide');
            }
            else {
                $("#MainReconciliationFormatEditForm").find("#alertForm").UifAlert('show', Resources.MessageExistsReconciliationFormat, "warning");
            }
        }
    }


    /**
        * Permite abrir el modal de confirmación de eliminación del formato de conciliación bancaria seleccionado.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores del fromato de conciliación bancaria seleccionado.
        */
    RowDeleteReconciliationFormat(event, data) {
        $('#MainReconciliationFormatAlert').UifAlert('hide');
        $("#alertForm").UifAlert('hide');
        $("#selectedMessageDelete").text(data.Id + " - " + data.BankAccountCompanyName + " - " + data.FormatName);
        $('#modalDeleteReconciliationFormat').appendTo("body").modal('show');  

        deleteRowModel.Id = data.Id;
        deleteRowModel.OperationType = "D";
    }

    /**
        * Permite eliminar el formato de conciliación bancaria seleccionado.
        *
        */
    DeleteReconciliationFormat() {
        $('#modalDeleteReconciliationFormat').modal('hide');

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankStatement/DeleteReconciliationFormat",
            data: JSON.stringify({ "reconciliationFormatModel": deleteRowModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data[0].ReconciliationFormatCode == 0) {
                $("#MainReconciliationFormatAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "BankStatement/GetReconciliationFormats";
                $('#tableReconciliationFormat').UifDataTable({ source: controller })
            }
            else {
                $("#MainReconciliationFormatAlert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "danger");
            }
            $('#modalDeleteReconciliationFormat').UifModal('hide');
        });
    }


    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Graba, actualiza un formato de conciliación bancaria.
        *
        */
    static SaveReconciliationFormat(rowModel, operationType) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankStatement/SaveReconciliationFormat",
            data: JSON.stringify({ "reconciliationFormatModel": rowModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data[0].ReconciliationFormatCode < 0) {
                    $("#MainReconciliationFormatAlert").UifAlert('show', data[0].MessageError, "danger");
                }
                else {
                    $("#MainReconciliationFormatAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "BankStatement/GetReconciliationFormats";
                    $('#tableReconciliationFormat').UifDataTable({ source: controller })
                }
            }
        });
    }

    /**
        * Valida el ingreso de campos obligatorios en modal de agregar.
        *
        */
    static ValidateAddForm() {
        if ($("#MainReconciliationFormatAddForm").find('#selectBankReconciliation').val() == "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainReconciliationFormatAddForm").find('#selectFormat').val() == "") {
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
        if ($("#MainReconciliationFormatEditForm").find('#selectBankReconciliation').val() == "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainReconciliationFormatEditForm").find('#selectFormat').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectTypeTax, "warning");
            return false;
        }

        return true;
    }

    /**
        * Permite validar que no se asigne el mismo formato de conciliación a la cuenta bancaria de la compañía.
        *
        * @param {Number} bankAccountCompanyId - Identificador de cuenta contable compañía.
        * @param {Number} formatId             - Identificador de formato de archivo.
        */
    static ValidateDuplicateRow(bankAccountCompanyId, formatId) {
        var rows = $("#tableReconciliationFormat").UifDataTable("getData");
        var exists = 0;

        for (var j in rows) {
            var item = rows[j];
            if (item.BankAccountCompanyId == bankAccountCompanyId && item.FormatId == formatId) {
                exists = 1
                break;
            }
        }

        return exists;
    }

    /**
        * Permite validar que no se asigne el mismo formato de conciliación a la cuenta bancaria de la compañía.
        *
        * @param {Number} bankAccountCompanyId - Identificador de cuenta contable compañía.
        * @param {Number} formatId             - Identificador de formato de archivo.
        */
    static ValidateDuplicateFormat(bankAccountCompanyId, formatId) {
        var rows = $("#tableReconciliationFormat").UifDataTable("getData");
        var exists = 0;

        for (var j in rows) {
            var item = rows[j];
            if (item.BankAccountCompanyId == bankAccountCompanyId) {
                exists = 1
                break;
            }
        }

        if (exists == 0) {
            for (var j in rows) {
                var item = rows[j];
                if (item.FormatId == formatId) {
                    exists = 1
                    break;
                }
            }
        }

        return exists;
    }

    /**
        * Permite validar que no se asigne el mismo formato de conciliación a la cuenta bancaria de la compañía.
        *
        * @param {Number} formatId             - Identificador de formato de archivo.
        */
    static ValidateEditDuplicateRow( formatId) {
        var rows = $("#tableReconciliationFormat").UifDataTable("getData");
        var exists = 0;

        for (var j in rows) {
            var item = rows[j];
            if (item.FormatId == formatId) {
                exists = 1
                break;
            }
        }

        return exists;
    }
}
