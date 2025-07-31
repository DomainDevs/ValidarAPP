/**
    * @file   AccountsCatalog.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var completeWithCeros = $("#ViewBagCompleteWithCeros").val();
var accoutingAccountLength = $("#ViewBagAccoutingAccountLength").val();
var selected;
var checkCostCenterId = false;
var checkAnalysisId = false;
var idAccountingAccount = 0;
var ModelAccountinAccount = {
    AccountingAccountId: 0,
    AccountingAccountParentId: 0,
    AccountingAccountNumber: "",
    AccountingAccountName: "",
    BranchId: 0,
    AccountingNatureId: 0,
    CurrencyId: 0,
    RequireAnalysis: 0,
    AnalysisId: 0,
    RequireCostCenter: 0,
    CostCenterId: 0,
    Comments: " ",
    AccountingAccountApplication: 0,
    AccountingAccountType: 0,
    CostCenters: [],
    PrefixId: 0
};

var accountingAccountReplicationPromise;

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainAccountsCatalog();
});

class MainAccountsCatalog extends Uif2.Page {

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
        $("#accountingAccountReplicationModal").find("#accountReplicationAccept").on('click', this.AccountReplicate);
        $("#modalAccountingAccount").on('click', "#saveButton", this.SaveAccountingAccount);
        $("#modalAccountingAccount").on('click', "#checkAnalysisId", this.DisabledCheckAnalysis);
        $("#modalAccountingAccount").on('click', "#checkCostCenterId", this.DisabledCheckCostCenter);
        $('#modalAccountingAccount').on('click', '#EnableBilling', this.CheckEnableBilling);
        $("#modalAccountingAccount").on('click', "#EnableAccountingTitle", this.CheckEnableAccountingTitle);
        $("#modalAccountingAccount").on('click', "#EnableIsOfficialNomenclature", this.CheckEnableIsOfficialNomenclature);
        $("#modalAccountingAccount").on("shown.bs.modal", this.ShowBSModal);
        $("#modalAccountingAccount").find("#costCenters").on('itemSelected', this.SelectCostCenter);
        $("#modalAccountingAccount").find("#prefixes").on('itemSelected', this.SelectPrefixes);
        $("#btnDeleteModal").on('click', this.DeleteAccountingAccount);
        $("span").on('click', this.CheckSpan);
        $("#createNode").on('click', this.CreateNode);
        $("#editNode").on('click', this.EditNode);
        $("#deleteNode").on('click', this.DeleteNode);
        $("#replicateNode").on('click', this.ReplicateNode);
    }



    /**
        * Replica el catálogo de cuentas contables.
        *
        */
    AccountReplicate() {

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


        $('#accountingAccountReplicationModal').UifModal('hide');
        MainAccountsCatalog.ReplicateAccount(selected[0]);
        accountingAccountReplicationPromise.then(function (accountData) {
            if (accountData) {
                //Mensaje de éxito
                if (accountData.success) {
                    $("#alertAccountingAccount").UifAlert('show', Resources.ReplicationSuccessful, "success");
                    setTimeout(function () {
                        location.reload(true);
                    }, 3000);
                } else {
                    $("#alertAccountingAccount").UifAlert('show', 'Error en proceso', "warning");
                }
            }
        });
    }

    /**
        * Graba / actualiza una cuenta contable.
        *
        */
    SaveAccountingAccount() {
        MainAccountsCatalog.AccountingAccountComplete();
        if ($('#modalAccountingAccount').find("#addAccountingAccount").valid()) {
            ModelAccountinAccount.AccountingAccountId = $('#modalAccountingAccount').find("#AccountingAccountId").val();
            ModelAccountinAccount.AccountingAccountParentId = $('#modalAccountingAccount').find("#AccountingAccountParentId").val();
            ModelAccountinAccount.AccountingAccountNumber = $('#modalAccountingAccount').find("#AccountingAccountNumber").val();
            ModelAccountinAccount.AccountingAccountName = $('#modalAccountingAccount').find("#AccountingAccountName").val();
            ModelAccountinAccount.BranchId = $('#modalAccountingAccount').find("#BranchId").val();
            ModelAccountinAccount.AccountingNatureId = $('#modalAccountingAccount').find("#AccountingNatureId").val();
            if ($('#modalAccountingAccount').find("#CurrencyId").val() == "") {
                ModelAccountinAccount.CurrencyId = -1;
            }
            else {
                ModelAccountinAccount.CurrencyId = $('#modalAccountingAccount').find("#CurrencyId").val();
            }

            ModelAccountinAccount.RequireAnalysis = ($('#modalAccountingAccount').find('#checkAnalysisId').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            ModelAccountinAccount.AnalysisId = $('#modalAccountingAccount').find("#AnalysisId").val();
            ModelAccountinAccount.RequireCostCenter = ($('#modalAccountingAccount').find('#checkCostCenterId').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
            ModelAccountinAccount.CostCenterId = $('#modalAccountingAccount').find("#CostCenterId").val();
            ModelAccountinAccount.Comments = $('#modalAccountingAccount').find("#Comments").val();
            ModelAccountinAccount.AccountingAccountType = $('#modalAccountingAccount').find("#type  option:selected").val();

            if ($("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").hasClass('glyphicon glyphicon-check')) {
                ModelAccountinAccount.AccountingAccountApplication = 3
            }
            else {
                if ($("#modalAccountingAccount").find("#EnableBilling").hasClass('glyphicon glyphicon-check') && $("#modalAccountingAccount").find("#EnableAccountingTitle").hasClass('glyphicon glyphicon-check')) {
                    ModelAccountinAccount.AccountingAccountApplication = 4
                }
                else {
                    if ($("#modalAccountingAccount").find("#EnableBilling").hasClass('glyphicon glyphicon-check')) {
                        ModelAccountinAccount.AccountingAccountApplication = 1
                    }
                    if ($("#modalAccountingAccount").find("#EnableAccountingTitle").hasClass('glyphicon glyphicon-check')) {
                        ModelAccountinAccount.AccountingAccountApplication = 2
                    }
                }
            }

            ModelAccountinAccount.CostCenters = $('#modalAccountingAccount').find('#costCenters').val();
            ModelAccountinAccount.PrefixId = $('#modalAccountingAccount').find('#PrefixId').val();

            var correct = [];
            var edit = (ModelAccountinAccount.AccountingAccountId == 0) ? 0 : 1;
            $.ajax({
                type: "POST",
                async: false,
                url: 'ValidateAccountingAccount',
                data: JSON.stringify({ "accountingAccountModel": ModelAccountinAccount, "edit": edit }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                correct = data;
            });

            if (correct.IsSucessful) {
                $.ajax({
                    type: "POST",
                    url: 'SaveAccountingAccount',
                    data: JSON.stringify({ "accountingAccountModel": ModelAccountinAccount }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $('#modalAccountingAccount').UifModal('hide');

                        if (ModelAccountinAccount.AccountingAccountId == 0) {
                            $("#alertAccountingAccount").UifAlert('show', Resources.AddSuccessfully, "success");
                        } else {
                            $("#alertAccountingAccount").UifAlert('show', Resources.EditSuccessfully, "success");
                        }
                        setTimeout(function () {
                            location.reload(true);
                        }, 3000);
                    }
                });
            } else {
                switch (correct.TypeId) {
                    case 1:
                        $('#modalAccountingAccount').find("#alertAccount").UifAlert('show', Resources.AccountThereAlreadyIs, "warning");
                        break;
                    case 2:
                        $('#modalAccountingAccount').find("#alertAccount").UifAlert('show', Resources.ParentAccountError, "warning");
                        break;
                    case 3:
                        $('#modalAccountingAccount').find("#alertAccount").UifAlert('show', Resources.AccountingNumberLengthError, "warning");
                        break;
                    case 4:
                        $('#modalAccountingAccount').find("#alertAccount").UifAlert('show', Resources.ParentAccountError, "warning");
                        break;
                    case 5:
                        $('#modalAccountingAccount').find("#alertAccount").UifAlert('show', Resources.AccountNoParent, "warning");
                        break;
                    case 6:
                        $('#modalAccountingAccount').find("#alertAccount").UifAlert('show', Resources.LedgerAccountNumber + " " + Resources.TooMuchBig, "warning");
                        break;
                }
            }
        }
    }

    /**
        * Habilita / deshabilita concepto de análisis asociados a la cuenta contable.
        *
        */
    DisabledCheckAnalysis() {
        if ($(this).hasClass("glyphicon glyphicon-unchecked")) {
            $(this).removeClass("glyphicon glyphicon-unchecked");
            $(this).addClass("glyphicon glyphicon-check");
            $('#modalAccountingAccount').find('#AnalysisId').attr("disabled", false);
        } else if ($(this).hasClass("glyphicon glyphicon-check")) {
            $(this).removeClass("glyphicon glyphicon-check");
            $(this).addClass("glyphicon glyphicon-unchecked");
            $('#modalAccountingAccount').find('#AnalysisId').attr("disabled", "disabled");
            $('#modalAccountingAccount').find('#AnalysisId').val("");
        }
    }

    /**
        * Habilita / deshabilita centros de costo asociados a la cuenta contable.
        *
        */
    DisabledCheckCostCenter() {
        if ($(this).hasClass("glyphicon glyphicon-unchecked")) {
            $(this).removeClass("glyphicon glyphicon-unchecked");
            $(this).addClass("glyphicon glyphicon-check");
            $('#modalAccountingAccount').find('#CostCenterId').attr("disabled", false);
        } else if ($(this).hasClass("glyphicon glyphicon-check")) {
            $(this).removeClass("glyphicon glyphicon-check");
            $(this).addClass("glyphicon glyphicon-unchecked");
            $('#modalAccountingAccount').find('#CostCenterId').attr("disabled", "disabled");
            $('#modalAccountingAccount').find('#CostCenterId').val("");
        }
    }

    /**
        * Chequea / deschequea el checkbox caja.
        *
        */
    CheckEnableBilling() {
        if ($("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").hasClass('glyphicon glyphicon-check')) {
            $("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").click();
        }

        if ($("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").hasClass('glyphicon glyphicon-unchecked')) {
            if ($("#modalAccountingAccount").find("#EnableBilling").hasClass('glyphicon glyphicon-check')) {
                $("#modalAccountingAccount").find("#EnableBilling").removeClass('glyphicon glyphicon-check');
                $("#modalAccountingAccount").find("#EnableBilling").addClass('glyphicon glyphicon-unchecked');
            } else {
                $("#modalAccountingAccount").find("#EnableBilling").removeClass('glyphicon glyphicon-unchecked');
                $("#modalAccountingAccount").find("#EnableBilling").addClass('glyphicon glyphicon-check');
            }
        }
    }

    /**
        * Chequea / deschequea el checkbox contabilidad.
        *
        */
    CheckEnableAccountingTitle() {
        if ($("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").hasClass('glyphicon glyphicon-check')) {
            $("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").click();
        }

        if ($("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").hasClass('glyphicon glyphicon-unchecked')) {
            if ($("#modalAccountingAccount").find("#EnableAccountingTitle").hasClass('glyphicon glyphicon-check')) {
                $("#modalAccountingAccount").find("#EnableAccountingTitle").removeClass('glyphicon glyphicon-check');
                $("#modalAccountingAccount").find("#EnableAccountingTitle").addClass('glyphicon glyphicon-unchecked');
            } else {
                $("#modalAccountingAccount").find("#EnableAccountingTitle").removeClass('glyphicon glyphicon-unchecked');
                $("#modalAccountingAccount").find("#EnableAccountingTitle").addClass('glyphicon glyphicon-check');
            }
        }
    }

    /**
        * Chequea / deschequea el checkbox nomenclatura oficial.
        *
        */
    CheckEnableIsOfficialNomenclature() {
        if ($("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").hasClass('glyphicon glyphicon-check')) {
            $("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").removeClass('glyphicon glyphicon-check');
            $("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").addClass('glyphicon glyphicon-unchecked');
        } else {
            $("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").removeClass('glyphicon glyphicon-unchecked');
            $("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").addClass('glyphicon glyphicon-check');

            //SI SE ACTIVA NOMENCLATURA OFICIAL SE QUITA MARCA A CAJA
            if ($("#modalAccountingAccount").find("#EnableBilling").hasClass('glyphicon glyphicon-check')) {
                $("#modalAccountingAccount").find("#EnableBilling").removeClass('glyphicon glyphicon-check');
                $("#modalAccountingAccount").find("#EnableBilling").addClass('glyphicon glyphicon-unchecked');
            }
            //SI SE ACTIVA NOMENCLATURA OFICIAL SE QUITA MARCA A CONTABILIDAD
            if ($("#modalAccountingAccount").find("#EnableAccountingTitle").hasClass('glyphicon glyphicon-check')) {
                $("#modalAccountingAccount").find("#EnableAccountingTitle").removeClass('glyphicon glyphicon-check');
                $("#modalAccountingAccount").find("#EnableAccountingTitle").addClass('glyphicon glyphicon-unchecked');
            }
        }
    }

    /**
        * Visualiza el modal de cuentas contables.
        *
        */
    ShowBSModal() {
        setTimeout(function () {
            checkCostCenterId = ($('#modalAccountingAccount').find('#RequireCostCenter').val() == 1) ? true : false;
            checkAnalysisId = ($('#modalAccountingAccount').find('#RequireAnalysis').val() == 1) ? true : false;

            MainAccountsCatalog.SetChecks(checkCostCenterId, checkAnalysisId);

            var retsult = [];
            var edit = (selected[0] == 0) ? 0 : 1;

            if (edit != 0) {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: 'GetAccountingAccountDetails',
                    data: JSON.stringify({ "accountingAccountModel": selected[0] }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    retsult = data;
                });

                if (!(retsult.AccountingAccountApplication >= 0)) {
                    retsult.AccountingAccountApplication = 0;
                }


                switch (retsult.AccountingAccountApplication) {
                    case 1:
                        $("#modalAccountingAccount").find("#EnableBilling").click();
                        break;
                    case 2:
                        $("#modalAccountingAccount").find("#EnableAccountingTitle").click();
                        break;
                    case 3:
                        $("#modalAccountingAccount").find("#EnableIsOfficialNomenclature").click();
                        break;
                    case 4:
                        $("#modalAccountingAccount").find("#EnableBilling").click();
                        $("#modalAccountingAccount").find("#EnableAccountingTitle").click();
                        break;
                    case 0:
                        break;
                }

                $('#modalAccountingAccount').find('#costCenters').UifMultiSelect('setSelected', retsult.CostCenters);
                $('#modalAccountingAccount').find('#prefixes').UifMultiSelect('setSelected', retsult.Prefixes);
            }
        }, 500);
    }

    /**
        * Obtiene los centros de costo seleccionados.
        *
        * @param {String} event         - Seleccionar.
        * @param {Object} selectedItems - Objeto con valores del centro de costo seleccionado.
        */
    SelectCostCenter(event, selectedItems) {
        ModelAccountinAccount.CostCenters = selectedItems;
    }

    /**
        * Obtiene los ramos seleccionados.
        *
        * @param {String} event         - Seleccionar.
        * @param {Object} selectedItems - Objeto con valores del ramo seleccionado.
        */
    SelectPrefixes(event, selectedItems) {
        ModelAccountinAccount.Prefixes = selectedItems;
    }
    
    /**
        * Elimina una cuenta contable.
        *
        */
    DeleteAccountingAccount() {
        if ($("#hiddenAccountingAccount").val() != undefined) {
            $('#modalDelete').modal('hide');
            $.ajax({
                type: "POST",
                url: GL_ROOT + "AccountingAccount/DeleteAccountingAccount",
                data: JSON.stringify({ "accountingAccountId": idAccountingAccount }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data == true) {
                    $("#alertAccountingAccount").UifAlert('show', Resources.DeleteSuccessfully, "success");
                } else {
                    $("#alertAccountingAccount").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }
                setTimeout(function () {
                    location.reload(true);
                }, 3000);
            });
        }
    }

    /**
        * Chequea / deschequea un span.
        *
        */
    CheckSpan() {
        if ($("#hiddenAccountingAccount").val() != undefined) {
            if ($(this).hasClass("glyphicon glyphicon-unchecked")) {
                $(this).removeClass("glyphicon glyphicon-unchecked");
                $(this).addClass("glyphicon glyphicon-check");
            } else if ($(this).hasClass("glyphicon glyphicon-check")) {
                $(this).removeClass("glyphicon glyphicon-check");
                $(this).addClass("glyphicon glyphicon-unchecked");
            }
        }
    }

    /**
        * Crea un nodo.
        *
        */
    CreateNode() {
        selected = $("#treeAccountingAccount").UifTreeView('getSelected');
        if (selected[0]) {
            $('#modalAccountingAccount').appendTo("body").UifModal('show',
                GL_ROOT + "AccountingAccount/AccountingAccountModal?parentId=" + selected[0], Resources.AddRecord);
        } else {
            $("#alertAccountingAccount").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
        }
    }

    /**
        * Edita un nodo.
        *
        */
    EditNode() {
        selected = $("#treeAccountingAccount").UifTreeView('getSelected');
        if (!selected[0]) {
            $("#alertAccountingAccount").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
        } else if (selected[0] > 10) {
            $('#modalAccountingAccount').appendTo("body").UifModal('show', GL_ROOT + "AccountingAccount/AccountingAccountModal?id=" + selected[0], Resources.EditRecord);
        } else {
            $("#alertAccountingAccount").UifAlert('show', Resources.AccountParentRestrictionWarning, 'danger');
        }
    }

    /**
        * Elimina un nodo.
        *
        */
    DeleteNode() {
        selected = $("#treeAccountingAccount").UifTreeView('getSelected');
        if (!selected[0]) {
            $("#alertAccountingAccount").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
        } else if (MainAccountsCatalog.HasChildren(selected[0])) {
            $("#alertAccountingAccount").UifAlert('show', Resources.ParentAccountNotDelete, 'danger');
        } else if (MainAccountsCatalog.OnEntry(selected[0])) {
            $("#alertAccountingAccount").UifAlert('show', Resources.SeatsAccountNotDelete, 'danger');
        } else if (MainAccountsCatalog.OnConcept(selected[0])) {
            $("#alertAccountingAccount").UifAlert('show', Resources.OnConceptAccountNotDelete, 'danger');
        } else {
            $('#modalDelete').appendTo("body").modal('show');
            idAccountingAccount = selected[0];
        }
    }

    /**
        * Replica un nodo.
        *
        */
    ReplicateNode() {
        selected = $("#treeAccountingAccount").UifTreeView('getSelected');
        if (!selected[0]) {
            $("#alertAccountingAccount").UifAlert('show', Resources.SelectedAccountBeforeContinuing, 'danger');
        } else if (selected[0] > 10) {
            $('#accountingAccountReplicationModal').appendTo("body").UifModal('showLocal', Resources.Replicate);
        } else {
            $("#alertAccountingAccount").UifAlert('show', Resources.AccountParentReplicationRestrictionWarning, 'danger');
        }
    }


    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Completa la cuenta contable con ceros.
        *
        */
    static AccountingAccountComplete() {
        $('#modalAccountingAccount').find("#alertAccount").find('.close').click();
        var accountNumber = $('#modalAccountingAccount').find("#AccountingAccountNumber").val();

        if (completeWithCeros == "1") {
            accountNumber = MainAccountsCatalog.PaddingRight(accountNumber, '0', accoutingAccountLength);
        }

        $('#modalAccountingAccount').find("#AccountingAccountNumber").val(accountNumber);
    }

    /**
        * Comprueba que la cuenta contable no tenga cuentas hijas.
        *
        * @param {Number} accountingAccountId - Identificasdor de cuenta contable.
        * 
        * @return {boolean} Verdadero o falso.
        */
    static HasChildren(accountingAccountId) {
        var result = false;
        $.ajax({
            async: false,
            type: "POST",
            url: 'HasChildren',
            data: JSON.stringify({ accountingAccountId: accountingAccountId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data == true) {
                result = true;
            }
        });
        return result;
    }

    /**
        * Comprueba que la cuenta no está siendo usada en asientos de diario o de mayor.
        *
        * @param {Number} accountingAccountId - Identificasdor de cuenta contable.
        *
        * @return {boolean} Verdadero o falso.
        */
    static OnEntry(accountingAccountId) {
        var result = false;
        $.ajax({
            async: false,
            type: "POST",
            url: 'OnEntry',
            data: JSON.stringify({ accountingAccountId: accountingAccountId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data == true) {
                result = true;
            }
        });
        return result;
    }

    /**
        * Comprueba que la cuenta no esté en un concepto contable.
        *
        * @param {Number} accountingAccountId - Identificasdor de cuenta contable.
        * 
        * @return {boolean} Verdadero o falso.
        */
    static OnConcept(accountingAccountId) {
        var result = false;
        $.ajax({
            async: false,
            type: "POST",
            url: 'OnConcept',
            data: JSON.stringify({ accountingAccountId: accountingAccountId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data == true) {
                result = true;
            }
        });
        return result;
    }

    /**
        * Chequea o deschequea el checkbox de centro de costo y análisis.
        *
        * @param {Number} checkCostCenterId - Identificador de centro de costo.
        * @param {Number} checkAnalysisId   - Identificador de análisis.
        */
    static SetChecks(checkCostCenterId, checkAnalysisId) {
        if (checkCostCenterId == true) {
            if (!$('#modalAccountingAccount').find('#checkCostCenterId').hasClass('glyphicon glyphicon-check')) {
                $('#modalAccountingAccount').find('#checkCostCenterId').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
            }
            $('#modalAccountingAccount').find('#CostCenterId').attr("disabled", false);
        } else {
            if ($('#modalAccountingAccount').find('#checkCostCenterId').hasClass('glyphicon glyphicon-check')) {
                $('#modalAccountingAccount').find('#checkCostCenterId').removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
            }
            $('#modalAccountingAccount').find('#CostCenterId').attr("disabled", "disabled");
        }

        if (checkAnalysisId == true) {
            if (!$('#modalAccountingAccount').find('#checkAnalysisId').hasClass('glyphicon glyphicon-check')) {
                $('#modalAccountingAccount').find('#checkAnalysisId').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
            }
            $('#modalAccountingAccount').find('#AnalysisId').attr("disabled", false);
        } else {
            if ($('#modalAccountingAccount').find('#checkAnalysisId').hasClass('glyphicon glyphicon-check')) {
                $('#modalAccountingAccount').find('#checkAnalysisId').removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
            }
            $('#modalAccountingAccount').find('#AnalysisId').attr("disabled", "disabled");
        }
    }

    /**
        * Llena la cadena con un caracter especificado a la derecha.
        *
        * @param {String} string       - Cadena.
        * @param {String} character    - Caracter.
        * @param {String} stringLength - Longitud de la cadena.
        * 
        * @return {String} cadena rellnada con el caracter.
        */
    static PaddingRight(string, character, stringLength) {
        if (!string || !character || string.length >= stringLength) {
            return string;
        }
        var max = (stringLength - string.length) / character.length;
        for (var i = 0; i < max; i++) {
            string += character;
        }
        return string;
    }

    /**
        * Replica una cuenta contable.
        *
        * @param {Number} accountingAccountId - Indentificador de cuenta contable.
        *
        * @return {String} cadena rellnada con el caracter.
        */
    static ReplicateAccount(accountingAccountId) {
        return accountingAccountReplicationPromise = new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: GL_ROOT + "AccountingAccount/ReplicateAccount",
                data: { "accountingAccountId": accountingAccountId }
            }).done(function (accountData) {
                resolve(accountData);
            });
        });
    }
   

}