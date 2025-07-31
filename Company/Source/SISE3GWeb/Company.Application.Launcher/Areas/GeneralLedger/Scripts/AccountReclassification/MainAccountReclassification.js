/**
 * @file   MainAccountReclassification.js
 * @author Desarrollador
 * @version 0.1
 */

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
var accountingReclassificationCurrentEditIndex = 0;
var reclassificationDeleteRowModel = new AccountingReclassificationRowModel();
var accountingAccountOriginId = 0;
var accountingAccountOrigin = "";
var accountingAccountOriginName = "";
var accountingAccountDestinationId = 0;
var accountingAccountDestination = "";
var accountingAccountDestinationName = "";
var edit = 0;

function AccountingReclassificationRowModel() {
    this.Year;
    this.Month;
    this.AccountingAccountOriginId;
    this.AccountingAccountOrigin;
    this.AccountingAccountDestinationId;
    this.AccountingAccountDestination;
    this.PrefixOpening;
    this.BranchOpening;
}


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  DEFINICIÓN DE CLASE                                                     */
/*--------------------------------------------------------------------------------------------------------------------------*/

$(() => {
    new MainAccountingReclassification();
});

class MainAccountingReclassification extends Uif2.Page {
    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        var controller = GL_ROOT + "AccountReclassification/GetMonths";
        $("#AccountingAccountReclassificationMonth").UifSelect({ source: controller });
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#AccountingAccountReclassificationMonth").on("itemSelected", this.GetAccountingReclassification);
        $("#AccountingAccountReclassificationYear").on("blur", this.ValidateYear);
        $("#tableAccountingAccountReclassification").on("rowAdd", this.AddAccountingReclassification);
        $("#accountingAccountReclassificationSaveAdd").on("click", this.SaveAccountingReclassification);
        $("#tableAccountingAccountReclassification").on("rowEdit", this.EditAccountingReclassification);
        $("#accountingAccountReclassificationSaveEdit").on("click", this.UpdateAccountingReclassification);
        $("#tableAccountingAccountReclassification").on("rowDelete", this.DeleteAccountingReclassification);
        $("#accountingAccountReclassificationDeleteModal").on("click", this.DeleteModalAccountingReclassification);
        $("#AccountingAccountOrigin").on("itemSelected", this.AutocompleteAccountantAccountOrigin);
        $("#AccountingAccountDestination").on("itemSelected", this.AutocompleteAccountantAccountDestination);
        $("#accountingAccountReclassificationEditForm").find("#AccountingAccountOrigin").on("itemSelected", this.AutocompleteAccountantAccountOrigin);
        $("#accountingAccountReclassificationEditForm").find("#AccountingAccountDestination").on("itemSelected", this.AutocompleteAccountantAccountDestination);
        $("#AccountingAccountReclassificationMonth").on('binded', this.BindedMonthDefault);
    }

    /**
        * Obtiene la parametrización de reclasificación contable al seleccionar un mes.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del mes seleccionado.
        */
    GetAccountingReclassification(event, selectedItem) {
        $("#tableAccountingAccountReclassification").UifDataTable();
        if (selectedItem.Id > 0 && $("#AccountingAccountReclassificationYear").val() != "") {
            var controller = GL_ROOT + "AccountReclassification/GetAccountingReclassificationByMonthAndYear?month=" +
                             selectedItem.Id + "&year=" + $("#AccountingAccountReclassificationYear").val();
            $('#tableAccountingAccountReclassification').UifDataTable({ source: controller });
        }
        else {
            $('#tableAccountingAccountReclassification').dataTable().fnClearTable();
        }
    }

    /**
        * Controla que el año sea mayor a 1900.
        *
        */
    ValidateYear() {
        $("#alert").UifAlert('hide');
        if ($("#AccountingAccountReclassificationYear").val() != "") {
            if (parseInt($("#AccountingAccountReclassificationYear").val()) < 1900) {
                $("#alert").UifAlert('show', Resources.MessageYear, "warning");
                $("#AccountingAccountReclassificationYear").val("");
            }
            else {
                var controller = GL_ROOT + "AccountReclassification/GetAccountingReclassificationByMonthAndYear?month=" +
                                            $("#AccountingAccountReclassificationMonth").val() + "&year=" + $("#AccountingAccountReclassificationYear").val();
                $('#tableAccountingAccountReclassification').UifDataTable({ source: controller })
            }
        }
    }

    /**
        * Agrega un nuevo registro a la tabla de reclasificación de cuentas.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con valores nulos por default.
        */
    AddAccountingReclassification(event, data) {
        $("#alert").UifAlert('hide');
        edit = 0
        if ($("#AccountingAccountReclassificationMonth").val() != "" && $("#AccountingAccountReclassificationYear").val() != "") {
            $("#accountingAccountReclassificationAddForm").find("#AccountingAccountOrigin").removeAttr("disabled");
            $("#accountingAccountReclassificationAddForm").find("#ReclassificationMonth").val(" " + $("#AccountingAccountReclassificationMonth option:selected").text());
            $("#accountingAccountReclassificationAddForm").find("#ReclassificationYear").val(" " + $("#AccountingAccountReclassificationYear").val());
            $('#accountingAccountReclassificationModalAdd').UifModal('showLocal', Resources.AddAccountingReclassificationParameterization);
        }
        else {
            $("#accountingAccountReclassificationAddForm").validate();
            $("#accountingAccountReclassificationAddForm").valid();
        }
    }

    /**
        * Graba la parametrización reclasificación contable.
        *
        */
    SaveAccountingReclassification() {
        $("#accountingAccountReclassificationAddForm").validate();

        if ($("#accountingAccountReclassificationAddForm").valid()) {

            if (MainAccountingReclassification.ValidateAddForm() == true) {
                var reclassificationRowModel = new AccountingReclassificationRowModel();

                reclassificationRowModel.Year = $("#AccountingAccountReclassificationYear").val();
                reclassificationRowModel.Month = $("#AccountingAccountReclassificationMonth").val();
                reclassificationRowModel.AccountingAccountOriginId = accountingAccountOriginId;
                reclassificationRowModel.AccountingAccountOrigin = accountingAccountOrigin;
                reclassificationRowModel.AccountingAccountDestinationId = accountingAccountDestinationId;
                reclassificationRowModel.AccountingAccountDestination = accountingAccountDestination;
                reclassificationRowModel.PrefixOpening = $("#accountingAccountReclassificationAddForm").find("#selectPrefixOpening").val();
                reclassificationRowModel.BranchOpening = $("#accountingAccountReclassificationAddForm").find("#selectBranchOpening").val();

                MainAccountingReclassification.SaveParametrizationAccountingReclassification(reclassificationRowModel, "I");

                $("#accountingAccountReclassificationAddForm").formReset();
                $('#accountingAccountReclassificationModalAdd').UifModal('hide');
            }
            else {
                setTimeout(function () {
                    $("#alertForm").UifAlert('hide');
                }, 3000);
            }
        }
    }

    /**
        * Editar un registro de la tabla de reclasificación de cuentas.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del registro seleccionado.
        * @param {Number} position - Número de posición del registro seleccionado.
        */
    EditAccountingReclassification(event, data, position) {
        $("#alert").UifAlert('hide');
        accountingReclassificationCurrentEditIndex = position;
        edit = 1

        $("#accountingAccountReclassificationEditForm").find("#AccountingAccountOrigin").attr("disabled", "disabled");
        //$("#accountingAccountReclassificationEditForm").find("#ReclassificationMonth").val(" " + data.Month);
        $("#accountingAccountReclassificationEditForm").find("#ReclassificationMonth").val(" " + $("#AccountingAccountReclassificationMonth option:selected").text());
        $("#accountingAccountReclassificationEditForm").find("#ReclassificationYear").val(" " + data.Year);
        $("#accountingAccountReclassificationEditForm").find("#AccountingAccountOrigin").val(data.AccountingAccountOrigin);
        $("#accountingAccountReclassificationEditForm").find("#AccountingAccountOriginName").val(" " + data.AccountingAccountOriginName);
        $("#accountingAccountReclassificationEditForm").find("#AccountingAccountDestination").val(data.AccountingAccountDestination);
        $("#accountingAccountReclassificationEditForm").find("#AccountingAccountDestinationName").val(" " + data.AccountingAccountDestinationName);
        $("#accountingAccountReclassificationEditForm").find("#selectPrefixOpening").val(data.PrefixOpeningId);
        $("#accountingAccountReclassificationEditForm").find("#selectBranchOpening").val(data.BranchOpeningId);
        accountingAccountOriginId = data.AccountingAccountOriginId;
        accountingAccountOrigin = data.AccountingAccountOrigin;
        accountingAccountOriginName = data.AccountingAccountOriginName;
        accountingAccountDestinationId = data.AccountingAccountDestinationId;
        accountingAccountDestination = data.AccountingAccountDestination;
        accountingAccountDestinationName = data.AccountingAccountDestinationName;

        $('#accountingAccountReclassificationModalEdit').UifModal('showLocal', Resources.EditAccountingReclassificationParameterization);
    }

    /**
        * Actualiza la parametrización reclasificación contable.
        *
        */
    UpdateAccountingReclassification() {
        $("#accountingAccountReclassificationEditForm").validate();

        if ($("#accountingAccountReclassificationEditForm").valid()) {

            if (MainAccountingReclassification.ValidateEditForm() == true) {
                var reclassificationRowModel = new AccountingReclassificationRowModel();

                reclassificationRowModel.Year = $("#AccountingAccountReclassificationYear").val();
                reclassificationRowModel.Month = $("#AccountingAccountReclassificationMonth").val();
                reclassificationRowModel.AccountingAccountOriginId = accountingAccountOriginId;
                reclassificationRowModel.AccountingAccountOrigin = accountingAccountOrigin;
                reclassificationRowModel.AccountingAccountDestinationId = accountingAccountDestinationId;
                reclassificationRowModel.AccountingAccountDestination = accountingAccountDestination;
                reclassificationRowModel.PrefixOpening = $("#accountingAccountReclassificationEditForm").find("#selectPrefixOpening").val();
                reclassificationRowModel.BranchOpening = $("#accountingAccountReclassificationEditForm").find("#selectBranchOpening").val();

                MainAccountingReclassification.SaveParametrizationAccountingReclassification(reclassificationRowModel, "U");

                $("#accountingAccountReclassificationEditForm").formReset();
                $('#accountingAccountReclassificationModalEdit').UifModal('hide');
            }
            else {
                setTimeout(function () {
                    $("#alertForm").UifAlert('hide');
                }, 3000);
            }
        }
    }

    /**
        * Elimina un registro de la tabla de reclasificación de cuentas.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores del registro seleccionado.
        */
    DeleteAccountingReclassification(event, data) {
        $('#alert').UifAlert('hide');
        $('#modalDeleteAccountingAccountReclassification').appendTo("body").modal('show');


        reclassificationDeleteRowModel.Year = data.Year;
        reclassificationDeleteRowModel.Month = data.Month;
        reclassificationDeleteRowModel.AccountingAccountOriginId = data.AccountingAccountOriginId;
        reclassificationDeleteRowModel.AccountingAccountOrigin = data.AccountingAccountOrigin;
        reclassificationDeleteRowModel.AccountingAccountDestinationId = data.AccountingAccountDestinationId;
        reclassificationDeleteRowModel.AccountingAccountDestination = data.AccountingAccountDestination;
        reclassificationDeleteRowModel.PrefixOpening = data.PrefixOpening;
        reclassificationDeleteRowModel.BranchOpening = data.BranchOpening;
    }

    /**
        * Elimina un registro al presionar el botón de confirmación.
        *
        */
    DeleteModalAccountingReclassification() {
        $('#modalDeleteAccountingAccountReclassification').modal('hide');

        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountReclassification/DeleteAccountingReclassification",
            data: JSON.stringify({ "accountingReclassificationModel": reclassificationDeleteRowModel, "operationType": "D" }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (data.result[0].AccountingReclassificationCode == 0) {
                    $("#alert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    var controller = GL_ROOT + "AccountReclassification/GetAccountingReclassificationByMonthAndYear?month=" +
                            reclassificationDeleteRowModel.Month + "&year=" + reclassificationDeleteRowModel.Year;
                    $('#tableAccountingAccountReclassification').UifDataTable({ source: controller })
                }
                else {
                    $("#alert").UifAlert('show', data.result[0].MessageError, "danger");
                }
                $('#modalDeleteAccountingAccountReclassification').UifModal('hide');
                /*
                setTimeout(function () {
                    $("#alert").find('.close').click();
                }, 3000);
                */
            }
            else {
                $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger", data.result);
            }
        });
    }

    /**
        * Autocomplete número de cuenta contable origen.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del registro seleccionado.
        */
    AutocompleteAccountantAccountOrigin(event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        accountingAccountOriginId = selectedItem.AccountingAccountId;
        accountingAccountOrigin = selectedItem.AccountingNumber;
        accountingAccountOriginName = " " + selectedItem.Description;
        if (edit == 0) {
            $("#AccountingAccountOriginName").val(accountingAccountOriginName);
        }
        else {
            $("#accountingAccountReclassificationEditForm").find("#AccountingAccountOriginName").val(accountingAccountOriginName);
        }
    }

    /**
        * Autocomplete número de cuenta contable destino.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del registro seleccionado.
        */
    AutocompleteAccountantAccountDestination(event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        accountingAccountDestinationId = selectedItem.AccountingAccountId;
        accountingAccountDestination = selectedItem.AccountingNumber;
        accountingAccountDestinationName = " " + selectedItem.Description;

        if (edit == 0) {
            $("#AccountingAccountDestinationName").val(accountingAccountDestinationName);
        }
        else {
            $("#accountingAccountReclassificationEditForm").find("#AccountingAccountDestinationName").val(accountingAccountDestinationName);
        }
    }

    /**
        * Setea el mes por default una vez que esta cargado.
        *
        */
    BindedMonthDefault() {
        $("#AccountingAccountReclassificationMonth").val($("#ViewBagMonthDefault").val());
        var controller = GL_ROOT + "AccountReclassification/GetAccountingReclassificationByMonthAndYear?month=" + $("#ViewBagMonthDefault").val()
                         + "&year=" + $("#AccountingAccountReclassificationYear").val();
        $('#tableAccountingAccountReclassification').UifDataTable({ source: controller })
    }



    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICION DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Graba o actualiza la parametrización de la reclasificación contable.
        *
        * @param {Object} rowModel      - Objeto con valores de reclasificación contable.
        * @param {String} operationType - I (Insertar), U (actualizar), D (Eliminar).
        */
    static SaveParametrizationAccountingReclassification(rowModel, operationType) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "AccountReclassification/SaveAccountingReclassification",
            data: JSON.stringify({ "accountingReclassificationModel": rowModel, "operationType": operationType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    if (data.result[0].AccountingReclassificationCode < 0) {
                        $("#alert").UifAlert('show', data.result[0].MessageError, "danger");
                    }
                    else {
                        $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
                        var controller = GL_ROOT + "AccountReclassification/GetAccountingReclassificationByMonthAndYear?month=" +
                                         rowModel.Month + "&year=" + rowModel.Year;
                        $('#tableAccountingAccountReclassification').UifDataTable({ source: controller })
                    }
                    /*
                    setTimeout(function () {
                        $("#alert").UifAlert('hide');
                    }, 3000);
                    */
                }
                else {
                    $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger", data.result);
                }
            }
        });
    }

    /**
        * Valida el ingreso de campos antes de grabar un registro.
        *
        */
    static ValidateAddForm() {
        if ($("#accountingAccountReclassificationAddForm").find('#AccountingAccountOrigin').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectConduit, "warning");
            return false;
        }
        if ($("#accountingAccountReclassificationAddForm").find('#AccountingAccountDestination').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectBank, "warning");
            return false;
        }

        return true;
    }

    /**
        * Valida el ingreso de campos antes de actualizar un registro.
        *
        */
    static ValidateEditForm() {
        if ($("#accountingAccountReclassificationEditForm").find('#AccountingAccountOrigin').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectConduit, "warning");
            return false;
        }
        if ($("#accountingAccountReclassificationEditForm").find('#AccountingAccountDestination').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectBank, "warning");
            return false;
        }

        return true;
    }

}
