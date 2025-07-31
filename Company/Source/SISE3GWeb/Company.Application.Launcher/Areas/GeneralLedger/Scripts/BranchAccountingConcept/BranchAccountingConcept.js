/**
    * @file   ProcessListEfecty.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var branchAccountingConceptId = 0;
var userBranchAccountingConceptId = 0;
var branchId = 0;
var userId = 0;
var movementTypeId = 0;
var conceptSourceId = 0;

var oBranchAccountingConceptModel = {
    Id: 0,
    BranchId: 0,
    UserId: 0,
    MovementTypeId: 0,
    ConceptSourceId: 0,
    AccountingConceptModels: []
};


var oAccountingConceptModel = {
    Id: 0
};


// Atacha evento click al seleccionar un registro de la tabla y al des seleccionarlo

$('body').delegate('#branchAccountingConceptTable tbody tr', "click", function () {    
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        //totalBallotDepositSlip();
    }
    else {
        $(this).siblings('.selected').removeClass('selected');
        $(this).addClass('selected');
        //totalBallotDepositSlip();
    }
});

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/

$(() => {
    new BranchAccountingConcept();
});

class BranchAccountingConcept extends Uif2.Page {

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {

        $("#conceptSources").on("itemSelected", this.SetMovementTypes);

        $("#movementTypes").on("itemSelected", this.SelectMovementTypes);
        $("#branchs").on("itemSelected", this.SelectBranch);
        $("#users").on("itemSelected", this.SelectAutoCompleteUsers);

        $("#branchAccountingConceptTable").on('rowAdd', this.RowAddAccountingConcept);
        $("#modalAdd").find("#SaveModal").on('click', this.SaveBranchAccountingConcept);

        $("#branchAccountingConceptTable").on('rowDelete', this.RowDeleteBranchAccountingConcept);
        $("#modalDeleteBranchAccountingConcept").on('click', this.DeleteBranchAccountingConcept);
    }

    /**
        * Permite grabar la relación de concepto contable con sucursal y usuario.
        *
        */
    SaveBranchAccountingConcept() {
        $("#alertForm").UifAlert('hide');
        $("#addAccountingConceptForm").validate();
        if ($("#addAccountingConceptForm").valid()) {
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                },
                message: "<h1>" + Resources.MessageWaiting + "</h1>"
            });

            $.ajax({
                type: "POST",
                url: GL_ROOT + "BranchAccountingConcept/SaveBranchAccountingConcept",
                data: JSON.stringify({
                    "branchAccountingConceptModel": BranchAccountingConcept.SetDataBranchAccountingConcept()
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        $('#addAccountingConceptForm').formReset();
                        $('#modalAdd').modal('hide');
                        $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
                        var controller = GL_ROOT + "BranchAccountingConcept/GetUserBranchAccountingConceptByUserByBranch?userId=" +
                            userId + "&branchId=" + $("#branchs").val() + "&conceptSourceId=" + conceptSourceId + "&movementTypeId=" + movementTypeId;
                        $("#branchAccountingConceptTable").UifDataTable({ source: controller });                                                
                    }
                    else {
                        $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger");
                    }
                    // $("#modalAdd").find("#accountingConceptTable").dataTable().fnClearTable();
                    $("#modalAdd").find("#accountingConceptTable").UifDataTable('unselect');

                    $.unblockUI();
                }
            });
        }
    }

    /**
        * Permite eliminar una relación sucursal concepto contable.
        *
        */
    DeleteBranchAccountingConcept() {
        $('#modalDeleteBranchAccountingConcept').modal('hide');

        $.ajax({
            type: "POST",
            url: GL_ROOT + "BranchAccountingConcept/DeleteBranchAccountingConcept",
            data: JSON.stringify(
                {
                    "userBranchAccountingConceptId": userBranchAccountingConceptId,
                    "branchAccountingConceptId": branchAccountingConceptId
                }
            ),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    $("#alert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    var controller = GL_ROOT + "BranchAccountingConcept/GetUserBranchAccountingConceptByUserByBranch?userId="
                        + userId + "&branchId=" + $("#branchs").val() + "&conceptSourceId=" + conceptSourceId + "&movementTypeId=" + movementTypeId;
                    $("#branchAccountingConceptTable").UifDataTable({ source: controller });                    
                }
                else {
                    if (data.result === "1") {
                        $("#alert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "warning");
                    }
                    else {
                        $("#alert").UifAlert('show', data.result, "danger");
                    }
                }
            }
        });
    }


    /**
        * Permite obtener los conceptos contables asociados a una sucursal.
        *
        * @param {String} event        - Seleccionar sucursal.
        * @param {Object} selectedItem - Objeto con la información de la sucursal seleccionada.
        */
    SelectBranch(event, selectedItem) {
        $('#alert').UifAlert('hide');
        if (selectedItem.Id != "") {
            branchId = selectedItem.Id;
            var controller = GL_ROOT + "BranchAccountingConcept/GetUserBranchAccountingConceptByUserByBranch?userId=" +
                userId + "&branchId=" + $("#branchs").val() + "&conceptSourceId=" + conceptSourceId + "&movementTypeId=" + movementTypeId;
            $("#branchAccountingConceptTable").UifDataTable({ source: controller });
        }
    }

    /**
        * Permite obtener los conceptos contables asociados a un tipo de movimiento.
        *
        * @param {String} event        - Seleccionar tipo de movimiento.
        * @param {Object} selectedItem - Objeto  con la información del tipo de movimiento seleccionado.
        */
    SetMovementTypes(event, selectedItem) {
        $('#alert').UifAlert('hide');
        if (selectedItem.Id != "") {
            conceptSourceId = selectedItem.Id;
            var controller = GL_ROOT + "BranchAccountingConcept/GetMovementTypesByConceptSourceId?conceptSourceId=" + conceptSourceId;
            $("#movementTypes").UifSelect({ source: controller });
            $("#movementTypes").removeProp("disabled");
        }
        else {
            $("#movementTypes").UifSelect();
            // setDataFieldsEmptyBank();
        }
    }

    /**
        * Permite obtener los usuarios por autocomplete.
        *
        * @param {String} event        - Seleccionar usuario.
        * @param {Object} selectedItem - Objeto con la información del usuario seleccionado.
        */
    SelectAutoCompleteUsers(event, selectedItem) {
        if (selectedItem != null) {
            userId = selectedItem.id;
            var controller = GL_ROOT + "BranchAccountingConcept/GetUserBranchAccountingConceptByUserByBranch?userId=" +
                userId + "&branchId=" + $("#branchs").val() + "&conceptSourceId=" + conceptSourceId + "&movementTypeId=" + movementTypeId;
            $("#branchAccountingConceptTable").UifDataTable({ source: controller });
        }
    }

    /**
        * Permite obtener el identificador del tipo de movimiento.
        *
        * @param {String} event        - Seleccionar tipo de movimiento.
        * @param {Object} selectedItem - Objeto con la información del tipo de movimiento.
        */
    SelectMovementTypes(event, selectedItem) {
        $('#alert').UifAlert('hide');
        if (selectedItem.Id != null) {
            movementTypeId = selectedItem.Id;
            var controller = GL_ROOT + "BranchAccountingConcept/GetUserBranchAccountingConceptByUserByBranch?userId="
                + userId + "&branchId=" + branchId + "&conceptSourceId=" + conceptSourceId + "&movementTypeId=" + movementTypeId;
            $("#branchAccountingConceptTable").UifDataTable({ source: controller });
        }
    }

    /**
        * Permite visualizar el modal de conceptos contables al presionar el botón agregar.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con la información del concepto contable seleccionado.
        */
    RowAddAccountingConcept(event, data) {
        $("#formBranchAccountingConcept").validate();
        if ($("#formBranchAccountingConcept").valid()) {
            // analysisConceptKeyId = 0;
            $("#alert").UifAlert('hide');
            $('#modalAdd').UifModal('showLocal', Resources.AddAccountingConcept);
        }
    }

    /**
        * Permite visualizar el modla de confirmación de eliminación de una relación sucursal concepto contable.
        *
        * @param {String} event - Seleccionar cocnepto contable.
        * @param {Object} data  - Objeto con la información del concepto contable seleccionado.
        */
    RowDeleteBranchAccountingConcept(event, data) {
        userBranchAccountingConceptId = data.UserBranchId;
        branchAccountingConceptId = data.BranchAccountingConceptId;

        $('#alert').UifAlert('hide');
        $('#modalDeleteBranchAccountingConcept').appendTo("body").modal('show');
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/    

    /**
     * Setea el modelo de sucursal concepto contable. */
     /**/
    static SetDataBranchAccountingConcept() {        
        oBranchAccountingConceptModel.AccountingConceptModels = [];
        $.each($('#branchAccountingConceptTable').UifDataTable('getData'), function (index, value) {
            oAccountingConceptModel = {
                Id: value.AccountingConcept.Id
            };
            oBranchAccountingConceptModel.AccountingConceptModels.push(oAccountingConceptModel);
        });
        oBranchAccountingConceptModel.Id = branchAccountingConceptId;
        oBranchAccountingConceptModel.BranchId = branchId;
        oBranchAccountingConceptModel.UserId = userId;
        oBranchAccountingConceptModel.MovementTypeId = movementTypeId;
        oBranchAccountingConceptModel.ConceptSourceId = conceptSourceId;

        var accountingConceptTable = $("#modalAdd").find("#accountingConceptTable").UifDataTable("getSelected");

        if (accountingConceptTable != null) {
            $.each(accountingConceptTable, function (index, value) {                
                oAccountingConceptModel = {
                    Id: 0
                };
                oAccountingConceptModel.Id = value.Id;
                oBranchAccountingConceptModel.AccountingConceptModels.push(oAccountingConceptModel);
            });
        }
        return oBranchAccountingConceptModel;
    }
}
