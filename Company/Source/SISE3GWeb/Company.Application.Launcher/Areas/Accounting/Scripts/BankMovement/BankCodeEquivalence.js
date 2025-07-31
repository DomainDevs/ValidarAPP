/**
    * @file   BankCodeEquivalence.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
function RowModel() {
    this.BankId;
    this.Id;
    this.BankMovementId;
    this.BankCode;
    this.HasVoucher;
}

var currentEditIndex = 0;
var index = 0;
var bankMovementId = 0;
var itHasVoucher = 0;
var deleteRowModel;
var HasVoucherCheck = false;

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainBankCodeEquivalence();
});

class MainBankCodeEquivalence extends Uif2.Page {

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
        $("#selectBank").on("itemSelected", this.GetBankCodes);
        $("#tableBankCodes").on("rowAdd", this.AddBankCodeEquivalence);
        $("#MainBankCodesSaveAdd").on("click", this.SaveAddBankCodeEquivalence);
        $("#tableBankCodes").on("rowEdit", this.EditBankCodeEquivalence);
        $("#MainBankCodesSaveEdit").on("click", this.SaveEditBankCodeEquivalence);
        $("#tableBankCodes").on("rowDelete", this.RowDeleteBankCodeEquivalence);
        $("#MainBankCodesDeleteModal").on("click", this.DeleteBankCodeEquivalence);        
        $("#MainBankCodesEditForm").find("#checkHasVoucher").on("click", this.ClickSpanEdit);
        $("#MainBankCodesAddForm").find("#checkHasVoucher").on("click", this.ClickSpanAdd);
    }

    /**
        * Permite obtener las equivalencias entre códigos bancarios al seleccionar el banco.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del banco seleccionado.
        */
    GetBankCodes(event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        $("#MainBankCodesAlert").UifAlert('hide');
        $("#tableBankCodes").UifDataTable();
        if (selectedItem.Id > -1) {
            var controller = ACC_ROOT + "BankMovement/GetBankCodesByBankId?bankId=" + $('#selectBank').val();
            $('#tableBankCodes').UifDataTable({ source: controller })
        }
        else {
            $('#tableBankCodes').dataTable().fnClearTable();
        }
    }

    /**
        * Permite abrir el modal de agregación de equivalencias entre códigos bancarios.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con valores del movimiento bancario seleccionado.
        */
    AddBankCodeEquivalence(event, data) {
        $("#alertForm").UifAlert('hide');
        $("#MainBankCodesAlert").UifAlert('hide');
        $("#formBankCodes").validate();

        if ($("#formBankCodes").valid()) {
            MainBankCodeEquivalence.GetMaximumCode();
            $("#MainBankCodesAddForm").find("#BankDescription").val($("#selectBank").val() + " - " + $("#selectBank option:selected").html());
            $("#MainBankCodesAddForm").find("#BankDescription").attr("disabled", "disabled");
            $("#MainBankCodesAddForm").find("#selectSiseMovement").val("");
            $("#MainBankCodesAddForm").find("#BankCodeEquivalente").val("");
            $("#MainBankCodesAddForm").find("#checkHasVoucher").removeClass('glyphicon glyphicon-checked');
            $("#MainBankCodesAddForm").find("#checkHasVoucher").addClass('glyphicon glyphicon-unchecked');
            $('#MainBankCodesModalAdd').UifModal('showLocal', Resources.AddBankCodeEquivalence);
        }
    }

    /**
        * Permite grabar una equivalencia entre movimiento sise y código bancario.
        *
        */
    SaveAddBankCodeEquivalence() {
        $("#MainBankCodesAddForm").validate();

        if ($("#MainBankCodesAddForm").valid()) {

            if (MainBankCodeEquivalence.ValidateAddForm() == true) {
                var rowModel = new RowModel();

                rowModel.BankCode = $("#MainBankCodesAddForm").find("#BankCodeEquivalente").val();
                rowModel.BankId = $("#selectBank").val();
                rowModel.BankMovementId = $("#MainBankCodesAddForm").find("#selectSiseMovement").val();
                if ($("#MainBankCodesAddForm").find('#checkHasVoucher').hasClass("glyphicon glyphicon-unchecked")) {
                    rowModel.HasVoucher = 0;
                }
                else if ($("#MainBankCodesAddForm").find('#checkHasVoucher').hasClass("glyphicon glyphicon-check")) {
                    rowModel.HasVoucher = 1;
                }
                rowModel.Id = 0;

                MainBankCodeEquivalence.SaveBankCodeEquivalence(rowModel, "I");

                $("#MainBankCodesAddForm").formReset();
                $('#MainBankCodesModalAdd').UifModal('hide');
            }            
        }
    }


    /**
        * Permite abrir el modal de edición de equivalencias entre códigos bancarios.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores de la equivalencia código bancario seleccionado.
        * @param {Object} position - Indice de la equivalencia código bancario seleccionado.
        */
    EditBankCodeEquivalence(event, data, position) {
        $("#alertForm").UifAlert('hide');
        $("#MainBankCodesAlert").UifAlert('hide');
        currentEditIndex = position;

        bankMovementId = data.Id;
        $("#MainBankCodesEditForm").find("#BankDescription").val($("#selectBank").val() + " - " + $("#selectBank option:selected").html());
        $("#MainBankCodesEditForm").find("#BankDescription").attr("disabled", "disabled");

        $("#MainBankCodesEditForm").find("#selectSiseMovement").val(data.Id);
        $("#MainBankCodesEditForm").find("#selectSiseMovement").attr("disabled", "disabled");

        $("#MainBankCodesEditForm").find("#BankCodeEquivalente").val(data.BankMovementCode);
        $("#MainBankCodesEditForm").find("#BankCodeEquivalente").attr("disabled", "disabled");

        if (data.HasVoucher == true) {            
           
            setTimeout(function () {
                if (!$("#MainBankCodesEditForm").find("#checkHasVoucher").hasClass('glyphicon glyphicon-check')) {
                    $("#MainBankCodesEditForm").find("#checkHasVoucher").removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                }
            }, 250);
            itHasVoucher = 1;
        }
        else {            
            
            setTimeout(function () {
                if ($("#MainBankCodesEditForm").find("#checkHasVoucher").hasClass('glyphicon glyphicon-check')) {
                    $("#MainBankCodesEditForm").find("#checkHasVoucher").removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
                }
            }, 250);
            itHasVoucher = 0;
        }


        $('#MainBankCodesModalEdit').UifModal('showLocal', Resources.EditBankCodeEquivalence);
    }

    /**
        * Permite grabar una equivalenciaódigo bancario.
        *
        */
    SaveEditBankCodeEquivalence() {
        $("#MainBankCodesEditForm").validate();

        if ($("#MainBankCodesEditForm").valid()) {

            if (MainBankCodeEquivalence.ValidateEditForm() == true) {
                var rowModel = new RowModel();

                rowModel.BankCode = $("#MainBankCodesEditForm").find("#BankCodeEquivalente").val();
                rowModel.BankId = $("#selectBank").val();
                rowModel.BankMovementId = $("#MainBankCodesEditForm").find("#selectSiseMovement").val();
                
                if ($("#MainBankCodesEditForm").find('#checkHasVoucher').hasClass("glyphicon glyphicon-unchecked") ||
                    $("#MainBankCodesEditForm").find('#checkHasVoucher').hasClass("glyphicon-unchecked glyphicon")) {
                    rowModel.HasVoucher = 0;
                }
                else if ($("#MainBankCodesEditForm").find('#checkHasVoucher').hasClass("glyphicon glyphicon-check") ||
                    $("#MainBankCodesEditForm").find('#checkHasVoucher').hasClass("glyphicon-check glyphicon")) {
                    rowModel.HasVoucher = 1;
                }                


                rowModel.Id = 0;

                MainBankCodeEquivalence.SaveBankCodeEquivalence(rowModel, "U");

                $("#MainBankCodesEditForm").formReset();
                $('#MainBankCodesModalEdit').UifModal('hide');
            }            
        }
    }


    /**
        * Permite abrir el modal de confirmación de eliminación de la equivalencia entre códigos bancarios.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores de la equivalencia código bancario seleccionado.
        */
    RowDeleteBankCodeEquivalence(event, data) {
        $("#alertForm").UifAlert('hide');
        $('#MainBankCodesAlert').UifAlert('hide');
        
        $("#selectedMessageDelete").text(data.Id + " - " + data.Description + " - " + data.BankMovementCode);

        $('#modalDeleteBankCodes').appendTo("body").modal('show');
        deleteRowModel = new RowModel();
        deleteRowModel.BankId = data.BankId;
        deleteRowModel.BankMovementId = data.Id;
        deleteRowModel.BankCode = data.BankMovementCode;
        
                
    }

    /**
        * Permite eliminar el equivalente código bancario seleccionado.
        *
        */
    DeleteBankCodeEquivalence() {
        $('#modalDeleteBankCodes').modal('hide');
       
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankMovement/DeleteBankCodeEquivalence",
            data: JSON.stringify({ "bankCodeEquivalenceModel": deleteRowModel, "operationType": "D" }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data[0].ResultEquivalenceCode == 0) {
                $("#MainBankCodesAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                
                var controller = ACC_ROOT + "BankMovement/GetBankCodesByBankId?bankId=" + deleteRowModel.BankId;
                $('#tableBankCodes').UifDataTable({ source: controller })
            }
            else {
                $("#MainBankCodesAlert").UifAlert('show', data[0].MessageError, "danger");
            }
            $('#modalDeleteBankCodes').UifModal('hide');
           
        });
    }

    /**
        * Permite eliminar el equivalente código bancario seleccionado.
        *
        */
    ClickSpanEdit() {
        if ($("#ViewBagControlSpanEquivalence").val() == "true") {
            
            if ($("#MainBankCodesEditForm").find("#checkHasVoucher").hasClass('glyphicon glyphicon-check')) {
                $("#MainBankCodesEditForm").find("#checkHasVoucher").removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked')            
            }            
            else if (!$("#MainBankCodesEditForm").find("#checkHasVoucher").hasClass('glyphicon glyphicon-check')) {
                $("#MainBankCodesEditForm").find("#checkHasVoucher").removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check')
            }
        }
    }    
    

    /**
            * check para número de comprobante, nuevo registro.
            *
            */
    ClickSpanAdd() {
        if ($("#ViewBagControlSpanEquivalence").val() == "true") {            
            if ($("#MainBankCodesAddForm").find("#checkHasVoucher").hasClass('glyphicon glyphicon-check')) {
                $("#MainBankCodesAddForm").find("#checkHasVoucher").removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked')
            }                
            else if (!$("#MainBankCodesAddForm").find("#checkHasVoucher").hasClass('glyphicon glyphicon-check')) {
                $("#MainBankCodesAddForm").find("#checkHasVoucher").removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check')
            }
        }
    }
        
    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Graba, actualiza una equivalencia código bancario.
        *
        */
    static SaveBankCodeEquivalence(rowModel, operationType) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BankMovement/SaveBankCodeEquivalence",
            data: JSON.stringify({ "bankCodeEquivalenceModel": rowModel, "operationType": operationType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data[0].ResultEquivalenceCode < 0) {
                    $("#MainBankCodesAlert").UifAlert('show', data[0].MessageError, "danger");
                }
                else {
                    $("#MainBankCodesAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                    
                    var controller = ACC_ROOT + "BankMovement/GetBankCodesByBankId?bankId=" + rowModel.BankId;
                    $('#tableBankCodes').UifDataTable({ source: controller })
                }                                  
            }
        });
    }

    /**
        * Valida el ingreso de campos obligatorios en modal de agregar.
        *
        */
    static ValidateAddForm() {
        if ($("#MainBankCodesAddForm").find('#BankCodeEquivalente').val() == "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainBankCodesAddForm").find('#selectSiseMovement').val() == "") {
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
        if ($("#MainBankCodesEditForm").find('#BankCodeEquivalente').val() == "") {
            $("#alertForm").UifAlert('show', Resources.EntryDescription, "warning");
            return false;
        }
        if ($("#MainBankCodesEditForm").find('#selectSiseMovement').val() == "") {
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
        var rows = $("#tableBankCodes").UifDataTable("getData");
        var maximumCode = 0;

        for (var j in rows) {
            var item = rows[j];
            maximumCode += rows[j].Id;
        }

        $("#MainBankCodesAddForm").find("#BankMovementCode").val(maximumCode);
    }

}
