/**
    * @file   MainAutomaticDebitFormat.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

function RowModel() {
    this.Id;
    this.BankNetworkId;
    this.FormatId;
    this.FileUsing;
    this.OperationType;
}

var deleteRowModel = new RowModel();
var currentEditIndex = 0;
var index = 0;
var automaticDebitFormatId = 0;


/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainAutomaticDebitFormat();
});

class MainAutomaticDebitFormat extends Uif2.Page {

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
        $("#selectBankNetwork").on("itemSelected", this.GetAutomaticDebitFormats);
        $("#tableAutomaticDebitFormat").on("rowAdd", this.AddAutomaticDebitFormat);
        $("#DebitFormatSaveAdd").on("click", this.SaveAddAutomaticDebitFormat);
        $("#tableAutomaticDebitFormat").on("rowEdit", this.EditAutomaticDebitFormat);
        $("#DebitFormatSaveEdit").on("click", this.SaveEditAutomaticDebitFormat);
        $("#tableAutomaticDebitFormat").on("rowDelete", this.RowDeleteAutomaticDebitFormat);
        $("#DebitFormatDeleteModal").on("click", this.DeleteAutomaticDebitFormat);
    }

    /**
        * Permite obtener los formatos parametrizados para la red seleccionada.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la red seleccionada.
        */
    GetAutomaticDebitFormats(event, selectedItem) {
        if (selectedItem.Id > 0) {
            $("#tableAutomaticDebitFormat").UifDataTable();
            var controller = ACC_ROOT + "AutomaticDebit/GetAutomaticDebitFormatsByBankNetworkId?bankNetworkId=" + selectedItem.Id;
            $("#tableAutomaticDebitFormat").UifDataTable({ source: controller });
        }
    }

    /**
        * Permite abrir el modal de agregación de formato de débito automático.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con valores del formato de débito automático seleccionado.
        */
    AddAutomaticDebitFormat(event, data) {
        $("#formDebitFormat").validate();

        if ($("#formDebitFormat").valid()) {
            $("#MainDebitFormatAddForm").find("#alertForm").UifAlert('hide');
            $('#MainDebitFormatAlert').UifAlert('hide');
            $('#MainDebitFormatModalAdd').UifModal('showLocal', Resources.AddAutomaticDebitFormatAssignment);
        }
    }

    /**
        * Permite grabar una asignación de formato de débito automático a una red.
        *
        */
    SaveAddAutomaticDebitFormat() {
        $("#MainDebitFormatAddForm").validate();

        if ($("#MainDebitFormatAddForm").valid()) {

            if (parseInt(MainAutomaticDebitFormat.ValidateDuplicateFormat($("#selectBankNetwork").val(), $("#MainDebitFormatAddForm").find("#selectFileUsing").val(), $("#MainDebitFormatAddForm").find("#selectFormat").val())) === 0) {
                var rowModel = new RowModel();

                rowModel.BankNetworkId = $("#selectBankNetwork").val();
                rowModel.FileUsing = $("#MainDebitFormatAddForm").find("#selectFileUsing").val();
                rowModel.FormatId = $("#MainDebitFormatAddForm").find("#selectFormat").val();
                rowModel.Id = 0;
                rowModel.OperationType = "I";

                MainAutomaticDebitFormat.SaveAutomaticDebitFormat(rowModel, "I");

                $("#MainDebitFormatAddForm").formReset();
                $('#MainDebitFormatModalAdd').UifModal('hide');
            }
            else {
                $("#MainDebitFormatAddForm").find("#alertForm").UifAlert('show', Resources.MessageDuplicateAutomaticDebitFormat, "warning");
            }
        }
    }


    /**
        * Permite abrir el modal de edición de asignación formato de débito automático seleccionado.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del fromato de débito automático seleccionado.
        * @param {Object} position - Indice del formato seleccionado.
        */
    EditAutomaticDebitFormat(event, data, position) {
        $("#alertForm").UifAlert('hide');
        $('#MainDebitFormatAlert').UifAlert('hide');
        $("#MainDebitFormatEditForm").find("#alertForm").UifAlert('hide');
        currentEditIndex = position;
        automaticDebitFormatId = data.Id;

        $("#MainDebitFormatEditForm").find("#selectFormat").val(data.FormatId);

        setTimeout(function () {
            $("#MainDebitFormatEditForm").find("#selectFileUsing").attr("disabled", "disabled");
            $("#MainDebitFormatEditForm").find("#selectFileUsing").val(data.FileUsingId);
        }, 300);

        $('#MainDebitFormatModalEdit').UifModal('showLocal', Resources.EditAutomaticDebitFormatAssignment);
    }

    /**
        * Permite grabar / actualizar una asignación de formato de débito automático a una red.
        *
        */
    SaveEditAutomaticDebitFormat() {
        $("#MainDebitFormatEditForm").validate();

        if ($("#MainDebitFormatEditForm").valid()) {

            if (parseInt(MainAutomaticDebitFormat.ValidateEditDuplicateRow($("#MainDebitFormatEditForm").find("#selectFormat").val())) === 0) {
                var rowModel = new RowModel();

                rowModel.BankNetworkId = $("#selectBankNetwork").val();
                rowModel.FileUsing = $("#MainDebitFormatEditForm").find("#selectFileUsing").val();
                rowModel.FormatId = $("#MainDebitFormatEditForm").find("#selectFormat").val();
                rowModel.Id = automaticDebitFormatId;
                rowModel.OperationType = "U";

                MainAutomaticDebitFormat.SaveAutomaticDebitFormat(rowModel, "U");

                $("#MainDebitFormatEditForm").formReset();
                $('#MainDebitFormatModalEdit').UifModal('hide');
            }
            else {
                $("#MainDebitFormatEditForm").find("#alertForm").UifAlert('show', Resources.MessageExistsAutomaticDebitFormat, "warning");
            }
        }
    }


    /**
        * Permite abrir el modal de confirmación de eliminación del formato de débito automático seleccionado.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores del formato de débito automático seleccionado.
        */
    RowDeleteAutomaticDebitFormat(event, data) {
        $('#MainDebitFormatAlert').UifAlert('hide');
        $("#alertForm").UifAlert('hide');
        $("#selectedMessageDelete").text(data.Id + " - " + data.FileUsing + " - " + data.FormatName);
        $('#modalDeleteDebitFormat').appendTo("body").modal('show');

        deleteRowModel.Id = data.Id;
        deleteRowModel.OperationType = "D";
    }

    /**
        * Permite eliminar el formato de débito automático seleccionado.
        *
        */
    DeleteAutomaticDebitFormat() {
        $('#modalDeleteDebitFormat').modal('hide');

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "AutomaticDebit/DeleteAutomaticDebitFormat",
            data: JSON.stringify({ "automaticDebitFormatModel": deleteRowModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (parseInt(data[0].AutomaticDebitFormatCode) === 0) {
                $("#MainDebitFormatAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                var controller = ACC_ROOT + "AutomaticDebit/GetAutomaticDebitFormatsByBankNetworkId?bankNetworkId=" + $("#selectBankNetwork").val();
                $('#tableAutomaticDebitFormat').UifDataTable({ source: controller });
            }
            else {
                $("#MainDebitFormatAlert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "danger");
            }
            $('#modalDeleteDebitFormat').UifModal('hide');
        });
    }


    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Graba, actualiza un formato de conciliación bancaria.
        *
        * @param {Object} rowModel      - Objeto con valores del formato de débito automático seleccionado.
        * @param {String} operationType - Eliminación.
        */
    static SaveAutomaticDebitFormat(rowModel, operationType) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "AutomaticDebit/SaveAutomaticDebitFormat",
            data: JSON.stringify({ "automaticDebitFormatModel": rowModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data[0].AutomaticDebitFormatCode < 0) {
                    $("#MainDebitFormatAlert").UifAlert('show', data[0].MessageError, "danger");
                }
                else {
                    $("#MainDebitFormatAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    var controller = ACC_ROOT + "AutomaticDebit/GetAutomaticDebitFormatsByBankNetworkId?bankNetworkId=" + $("#selectBankNetwork").val();
                    $('#tableAutomaticDebitFormat').UifDataTable({ source: controller })
                }
            }
        });
    }

    /**
        * Valida el ingreso de campos obligatorios en modal de agregar.
        *
        * @returns {boolean} true or false
        */
    static ValidateAddForm() {
        if ($("#MainDebitFormatAddForm").find('#selectFileUsing').val().toString() === "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainDebitFormatAddForm").find('#selectFormat').val().toString() === "") {
            $("#alertForm").UifAlert('show', Resources.SelectTypeTax, "warning");
            return false;
        }

        return true;
    }

    /**
        * Valida el ingreso de campos obligatorios en modal de editar.
        *
        * @returns {boolean} true or false
        */
    static ValidateEditForm() {
        if ($("#MainDebitFormatEditForm").find('#selectFileUsing').val().toString() === "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainDebitFormatEditForm").find('#selectFormat').val().toString() === "") {
            $("#alertForm").UifAlert('show', Resources.SelectTypeTax, "warning");
            return false;
        }

        return true;
    }

    /**
        * Permite validar que no se asigne el mismo formato de conciliación a la cuenta bancaria de la compañía.
        *
        * @param {number} bankNetworkId - Identificador de cuenta contable compañía.
        * @param {number} formatId      - Identificador de formato de archivo.
        * @returns {number}             - Existe o no
        */
    static ValidateDuplicateRow(bankNetworkId, formatId) {
        var rows = $("#tableAutomaticDebitFormat").UifDataTable("getData");
        var exists = 0;

        for (var j in rows) {
            var item = rows[j];
            if (parseInt(item.bankNetworkId) === parseInt(bankNetworkId) && parseInt(item.FormatId) === parseInt(formatId)) {
                exists = 1;
                break;
            }
        }

        return exists;
    }

    /**
        * Permite validar que no se asigne el mismo formato de conciliación a la cuenta bancaria de la compañía.
        *
        * @param {Number} bankNetworkId - Identificador de red bancaria.
        * @param {String} fileUsingId   - Uso del archivo.
        * @param {Number} formatId      - Identificador de formato de archivo.
        * @returns {number}             - Existe o no
        */
    static ValidateDuplicateFormat(bankNetworkId, fileUsingId, formatId) {
        var rows = $("#tableAutomaticDebitFormat").UifDataTable("getData");
        var exists = 0;

        for (var i in rows) {
            var item = rows[i];
            if (parseInt(item.BankNetworkId) === parseInt(bankNetworkId) && parseInt(item.FileUsingId) === parseInt(fileUsingId)) {
                exists = 1;
                break;
            }
        }

        if (parseInt(exists) === 0) {
            for (var k in rows) {
                var items = rows[k];
                if (parseInt(items.FormatId) === parseInt(formatId)) {
                    exists = 1;
                    break;
                }
            }
        }

        return exists;
    }

    /**
        * Permite validar que no se asigne el mismo formato de conciliación a la cuenta bancaria de la compañía.
        *
        * @param {Number} formatId - Identificador de formato de archivo.
        * @returns {number}        - Existe o no
        */
    static ValidateEditDuplicateRow(formatId) {
        var rows = $("#tableAutomaticDebitFormat").UifDataTable("getData");
        var exists = 0;

        for (var j in rows) {
            var item = rows[j];
            if (parseInt(item.FormatId) === parseInt(formatId)) {
                exists = 1;
                break;
            }
        }

        return exists;
    }
}
