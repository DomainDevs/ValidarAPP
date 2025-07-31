/**
    * @file   MainLedgerEntry.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var accountingConceptId = 0;
var accountingAccountCode = 0;
var isAgentEnabled = false;
var isCoinsuranceEnabled = false;
var isReinsuranceEnabled = false;
var isInsuredEnabled = false;
var isItemEnabled = false;

// Definicion de Modelo 
var accountingConceptModal = {
    Id: 0,
    AccountingAccountId: 0,
    Description: "",
    AgentEnabled: false,
    CoInsurancedEnabled: false,
    ReinsuranceEnabled: false,
    InsuredEnabled: false,
    ItemEnabled: false,
    AccountingAccount: []
};

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainAccountingConcept();
});

class MainAccountingConcept extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        //$("#btnAddMovement").on("click", this.ButtonAddMovements);
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        //Eventos de Ingreso 
        $("#accountingConceptTable").on('rowAdd', this.RowAddAccountingConcept);
        $("#MainAccountingConceptAddForm").on("itemSelected", "#accountingAccountNumber", this.AutocompleteAddAccountingAccountNumber);
        $("#MainAccountingConceptModalAdd").find("#SaveAddAccountingConcept").on('click', this.SaveAddAccountingConcept);

        // Eventos de editar 
        $("#accountingConceptTable").on('rowEdit', this.RowEditAccountingConcept);
        $("#MainAccountingConceptEditForm").on("itemSelected", "#accountingAccountNumber", this.AutocompleteEditAccountingAccountNumber);
        $("#MainAccountingConceptModalEdit").find("#SaveEditAccountingConcept").on('click', this.SaveEditAccountingConcept);
        $("#MainAccountingConceptModalEdit").on("itemSelected", "#accountingAccountNumber", this.AutocompleteEditAccountingConceptModal);

        // Eventos de Eliminar 
        $("#accountingConceptTable").on('rowDelete', this.RowDeleteAccountingConcept);
        $("#modalDeleteAccountingConcept").on('click', this.DeleteAccountingConcept);

        // Eventos de checks 
        // Insert  Check
        $("#MainAccountingConceptAddForm").find("#spanAgent").on('click', this.AgentChecks);
        $("#MainAccountingConceptAddForm").find("#spanCoinsurance").on('click', this.CoinsuranceChecks);
        $("#MainAccountingConceptAddForm").find("#spanReinsurance").on('click', this.ReinsuranceChecks);
        $("#MainAccountingConceptAddForm").find("#spanInsured").on('click', this.InsuredChecks);
        $("#MainAccountingConceptAddForm").find("#spanItems").on('click', this.ItemsChecks);

        // Edit Check
        $("#MainAccountingConceptEditForm").find("#spanAgent").on('click', this.AgentChecks);
        $("#MainAccountingConceptEditForm").find("#spanCoinsurance").on('click', this.CoinsuranceChecks);
        $("#MainAccountingConceptEditForm").find("#spanReinsurance").on('click', this.ReinsuranceChecks);
        $("#MainAccountingConceptEditForm").find("#spanInsured").on('click', this.InsuredChecks);
        $("#MainAccountingConceptEditForm").find("#spanItems").on('click', this.ItemsChecks);
    }

    // Eventos de tabla 
    /**
        * Ejecuta evento de tabla para Insertar
        * @param {Object} event    - Evento.
        */
    RowAddAccountingConcept(event) {
        $("#formAccountingConceptKey").validate();
        if ($("#formAccountingConceptKey").valid()) {
            accountingConceptId = 0;
            $("#mainAlert").UifAlert('hide');
            $('#MainAccountingConceptModalAdd').UifModal('showLocal', Resources.AddAccountingConcept);
        }
    }

    /**
        * Ejecuta evento de tabla para Editar
        * @param {Object} event    - Evento.
        * @param {Object} data     - Objeto con valores seleccionados.
    */
    RowEditAccountingConcept(event, data) {
        $('#mainAlert').UifAlert('hide');
        accountingConceptId = data.Id;
        accountingAccountCode = data.AccountingAccountId;
        $("#MainAccountingConceptEditForm").find("#accountingAccountNumber").val(data.AccountingAccount.Number);
        $("#MainAccountingConceptEditForm").find("#accountingAccountDescription").val(data.AccountingAccount.Description);

        MainAccountingConcept.SetEditChecks(data);
        $("#MainAccountingConceptEditForm").find("#Description").val(data.Description);

        $('#MainAccountingConceptModalEdit').UifModal('showLocal', Resources.EditAccountingConcept);
    }

    /**
        * Ejecuta evento de tabla para eliminar
        * @param {Object} event    - Evento Eliminar.
        * @param {Object} data     - Objeto con valores de la clave seleccionada.
        */
    RowDeleteAccountingConcept(event, data) {
        accountingConceptId = data.Id;
        $('#mainAlert').UifAlert('hide');
        $('#modalDeleteAccountingConcept').appendTo("body").modal('show');
    }

    // Eventos autocomplete
    /**
        * Ejecuta Autocomplete de Agregar para buscar cuenta contable.
        * @param {Object} event    - Evento.
        * @param {Object} selectedItem  - texto con valores a buscar.
        */
    AutocompleteAddAccountingAccountNumber(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#MainAccountingConceptModalAdd').find('#accountingAccountDescription').val(selectedItem.Number + " - " + selectedItem.Description);
        accountingAccountCode = selectedItem.AccountingAccountId;
        // $('#MainAccountingConceptModalAdd').find("#AccountingAccountId").val(accountingAccountCode);
    }

    /**
        * Ejecuta Autocomplete de Editar para buscar cuenta contable.
        * @param {Object} event    - Evento.
        * @param {Object} data     - texto con valores a buscar.
        */
    AutocompleteEditAccountingAccountNumber(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#MainAccountingConceptModalEdit').find('#accountingAccountDescription').val(selectedItem.Number + " - " + selectedItem.Description);
        accountingAccountCode = selectedItem.AccountingAccountId;
    }

    /**
        * Ejecuta Autocomplete de Editar para buscar cuenta contable.
        * @param {Object} event    - Evento.
        * @param {Object} data     - texto con valores a buscar.
        */
    AutocompleteEditAccountingConceptModal(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {
                $('#MainAccountingConceptModalEdit').find('#accountingAccountDescription').val(selectedItem.Number + " - " + selectedItem.Description);
                accountingAccountCode = selectedItem.AccountingAccountId;
                //$('#entryModalEdit').find("#entryModalAlert").UifAlert('hide');
            } else {
                $('#MainAccountingConceptModalEdit').find("#accountingAccountDescription").val('');
                accountingAccountCode =0;
            }
        } else {
           // $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            accountingAccountCode = 0;
            $("#MainAccountingConceptModalEdit").find("#accountingAccountDescription").val('');
        }
    }

    //Eventos de checks
    /**
        * Permite chequear y deschequear el ítem de agente
        */
    AgentChecks() {
        if (isAgentEnabled == true) {
            MainAccountingConcept.SetChecksSpanImage($(this), "N");
            isAgentEnabled = false;
        }
        else if (isAgentEnabled == false) {
            MainAccountingConcept.SetChecksSpanImage($(this), "S");
            isAgentEnabled = true;
        }
    }

    /**
        * Permite chequear y deschequear el ítem de coaseguros
        */
    CoinsuranceChecks() {
        if (isCoinsuranceEnabled == true) {
            MainAccountingConcept.SetChecksSpanImage($(this), "N");
            isCoinsuranceEnabled = false;
        }
        else if (isCoinsuranceEnabled == false) {
            MainAccountingConcept.SetChecksSpanImage($(this), "S");
            isCoinsuranceEnabled = true;
        }
    }

    /**
        * Permite chequear y deschequear el ítem de reaseguros
        */
    ReinsuranceChecks() {
        if (isReinsuranceEnabled == true) {
            MainAccountingConcept.SetChecksSpanImage($(this), "N");
            isReinsuranceEnabled = false;
        }
        else if (isReinsuranceEnabled == false) {
            MainAccountingConcept.SetChecksSpanImage($(this), "S");
            isReinsuranceEnabled = true;
        }
    }

    /**
        * Permite chequear y deschequear el ítem de asegurado
        */
    InsuredChecks() {
        if (isInsuredEnabled == true) {
            MainAccountingConcept.SetChecksSpanImage($(this), "N");
            isInsuredEnabled = false;
        }
        else if (isInsuredEnabled == false) {
            MainAccountingConcept.SetChecksSpanImage($(this), "S");
            isInsuredEnabled = true;
        }
    }

    /**
        * Permite chequear y deschequear habilita ítems
        */
    ItemsChecks() {
        if (isItemEnabled == true) {
            MainAccountingConcept.SetChecksSpanImage($(this), "N");
            isItemEnabled = false;
        }
        else if (isItemEnabled == false) {
            MainAccountingConcept.SetChecksSpanImage($(this), "S");
            isItemEnabled = true;
        }
    }

    // LLamadas a Metodos CRUD
    /**
      * Agregar Conceptos Contables
      */
    SaveAddAccountingConcept() {
        $("#alertForm").UifAlert('hide');

        $("#MainAccountingConceptAddForm").validate();
        if ($("#MainAccountingConceptAddForm").valid()) {
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
                url: GL_ROOT + "AccountingConcept/SaveAccountingConcept",
                data: JSON.stringify({
                    "accountingConceptModal": MainAccountingConcept.SetDataAccountingConcept("I")
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        $('#MainAccountingConceptAddForm').formReset();
                        $('#MainAccountingConceptModalAdd').modal('hide');
                        $("#mainAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                        var controller = GL_ROOT + "AccountingConcept/GetAccountingConcepts";
                        $("#accountingConceptTable").UifDataTable({ source: controller });
                    }
                    else {
                        $('#MainAccountingConceptModalAdd').modal('hide');
                        $("#mainAlert").UifAlert('show', Resources.ErrorTransaction, "danger");
                    }
                    $.unblockUI();
                }
            });
        }
    }

    /**
      * Editar Conceptos Contables
      */
    SaveEditAccountingConcept() {
        $("#alertForm").UifAlert('hide');

        $("#MainAccountingConceptEditForm").validate();
        if ($("#MainAccountingConceptEditForm").valid()) {
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
                url: GL_ROOT + "AccountingConcept/SaveAccountingConcept",
                data: JSON.stringify({
                    "accountingConceptModal": MainAccountingConcept.SetDataAccountingConcept("U")
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        $('#MainAccountingConceptEditForm').formReset();
                        $('#MainAccountingConceptModalEdit').modal('hide');
                        $("#mainAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
                        var controller = GL_ROOT + "AccountingConcept/GetAccountingConcepts";
                        $("#accountingConceptTable").UifDataTable({ source: controller });
                    }
                    else {
                        $("#mainAlert").UifAlert('show', Resources.ErrorTransaction, "danger");
                    }
                    $.unblockUI();
                }
            });
        }
    }

    /**
      * Eliminar Conceptos Contables
     */
    DeleteAccountingConcept() {
        $('#modalDeleteAccountingConcept').modal('hide');
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
            url: GL_ROOT + "AccountingConcept/DeleteAccountingConcept",
            data: JSON.stringify(
                {
                    "accountingConceptId": accountingConceptId                 
                }
            ),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    $("#mainAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    var controller = GL_ROOT + "AccountingConcept/GetAccountingConcepts";
                    $("#accountingConceptTable").UifDataTable({ source: controller });
                }
                else {
                    if (data.result === "1") {
                        $("#mainAlert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "warning");
                    }
                    else {
                        $("#mainAlert").UifAlert('show', data.result, "danger");
                    }
                }
                $.unblockUI();
            }
        });
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /** 
        * Recupera valores de la pantalla 
        * @param { String } option    - Opción (I-Ingreso,U-Update).
        */
    static SetDataAccountingConcept(option) {

        accountingConceptModal.Id = accountingConceptId;
        accountingConceptModal.AccountingAccountId = accountingAccountCode;
        if (option == "I") {
            accountingConceptModal.Description = $("#MainAccountingConceptAddForm").find("#Description").val();
        }
        else if (option == "U") {
            accountingConceptModal.Description = $("#MainAccountingConceptEditForm").find("#Description").val();

            if ($('#MainAccountingConceptModalEdit').find("#accountingAccountNumber").val() == "") {
                accountingAccountCode = 0;
                accountingConceptModal.AccountingAccountId = accountingAccountCode;
            }
        }

        accountingConceptModal.AgentEnabled = isAgentEnabled;
        accountingConceptModal.CoInsurancedEnabled = isCoinsuranceEnabled;
        accountingConceptModal.ReinsuranceEnabled = isReinsuranceEnabled;
        accountingConceptModal.InsuredEnabled = isInsuredEnabled;
        accountingConceptModal.ItemEnabled = isItemEnabled;

        //$("#MainAccountingConceptAddForm").find("#CoinsuranceCheck").prop('checked');
        return accountingConceptModal;
    }

    /** 
        * Coloca Imagen check en objeto span 
        *  @param { String } form    - Nombre de form Origen
        *  @param { String } span    - Nombre de span seleccionado.
        *  @param { String } option  - Opción (S/N).
     */
    static SetChecksImage(form,span,option)
    {
        if (option == "S") {
            $(form).find(span).removeClass("glyphicon glyphicon-unchecked");
            $(form).find(span).addClass("glyphicon glyphicon-check");
        }
        else {
            $(form).find(span).removeClass("glyphicon glyphicon-check");
            $(form).find(span).addClass("glyphicon glyphicon-unchecked");
        }
    }

    /** 
        * Coloca Check en image
        *  @param { object } argt - Objeto.
        *  @param { String } option    - Opción (S/N).
        */
    static SetChecksSpanImage(argt, option) {
        if (option == "S") {
            $(argt).removeClass("glyphicon glyphicon-unchecked");
            $(argt).addClass("glyphicon glyphicon-check");
        }
        else {
            $(argt).removeClass("glyphicon glyphicon-check");
            $(argt).addClass("glyphicon glyphicon-unchecked");
        }
    }

    /** 
        * Recupera valores check en Editar 
        *  @param { String } option    - Opción (I-Ingreso,U-Update).
        */
    static SetEditChecks(data)
    {
        var form;
        form = "#MainAccountingConceptEditForm";

        if (data.AgentEnabled == true) {
            MainAccountingConcept.SetChecksImage(form, "#spanAgent","S")
            isAgentEnabled = true;
        }
        else {
            MainAccountingConcept.SetChecksImage(form, "#spanAgent", "N")
            isAgentEnabled = false;
        }

        if (data.CoInsurancedEnabled == true) {
            MainAccountingConcept.SetChecksImage(form, "#spanCoinsurance", "S")
            isCoinsuranceEnabled = true;
        }
        else {
            MainAccountingConcept.SetChecksImage(form, "#spanCoinsurance", "N")
            isCoinsuranceEnabled = false;
        }

        if (data.ReinsuranceEnabled == true) {
            MainAccountingConcept.SetChecksImage(form, "#spanReinsurance", "S")
            isReinsuranceEnabled = true;
        }
        else {
            MainAccountingConcept.SetChecksImage(form, "#spanReinsurance", "N")
            isReinsuranceEnabled = false;
        }

        if (data.InsuredEnabled == true) {
            MainAccountingConcept.SetChecksImage(form, "#spanInsured", "S")
            isInsuredEnabled = true;
        }
        else {
            MainAccountingConcept.SetChecksImage(form, "#spanInsured", "N")
            isInsuredEnabled = false;
        }

        if (data.ItemEnabled == true) {
            MainAccountingConcept.SetChecksImage(form, "#spanItems", "S")
            isItemEnabled = true;
        }
        else {
            MainAccountingConcept.SetChecksImage(form, "#spanItems", "N")
            isItemEnabled = false;
        }
    }


}