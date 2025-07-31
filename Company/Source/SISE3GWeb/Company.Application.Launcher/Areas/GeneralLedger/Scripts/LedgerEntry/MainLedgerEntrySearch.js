/**
    * @file   MainLedgerEntrySearch.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var ledgerEntryRecord = null;

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainLedgerEntrySearch();
});

class MainLedgerEntrySearch extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        $('#tblEntries').UifDataTable();
        $("#tblCostCenter").UifDataTable();
        $("#tblAnalysis").UifDataTable();
        $("#tblPostdated").UifDataTable();

    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#ledgerEntryYear").on("keypress", this.KeyPressLedgerEntryYear);
        $("#transactionNumber").on("keypress", this.KeyPressTransactionNumber);
        $("#SearchLedgerEntry").on("click", this.SearchLedgerEntry);
        $("#ClearLedgerEntrySearch").on('click', this.ClearLedgerEntrySearch);
        $("#tblEntries").on("rowSelected", this.RowSelectedTableLedgerEntry);
        $("#RevertLedgerEntry").on("click", this.RevertLedgerEntry);
        $("#LedgerEntryModalYes").on("click", this.LedgerEntryModalYes);
        $("#PrintLedgerEntry").on("click", this.PrintLedgerEntry);
        $("#ledgerEntrySearchModal").on("rowSelected", "#tblLedgerEntryItems", this.RowSelectedTableLedgerEntryItems);
        $("#LedgerEntryDetails").on("click", this.GetLedgerEntry);
        $("#CancelEntryDetails").on("click", this.CloseLedgerEntryModal);

    }

    /**
        * Permite ingresar solo números en el año.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressLedgerEntryYear(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el número de asiento.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressTransactionNumber(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite buscar el asiento de mayor en base al mes, año, número asiento.
        *
        * @param {String} event    - Click.
        */
    SearchLedgerEntry(event) {

        MainLedgerEntrySearch.ClearTablesLedgerEntrySearch();
        $("#mainAlert").UifAlert('hide');
        $("#searchForm").validate();
        if ($("#searchForm").valid()) {
            var filtered;
            var controller;

            //Si uno de los campos opcionales está como nulo, entonces abre el pop up para seleccionar la cuenta.
            if ($("#ddlBranchs").val() == "" || $("#ddlDestination").val() == "" || $("#ddlAccountingMovementType").val() == "") {
                filtered = true;
            } else {
                filtered = false;
            }

            if (filtered) {
                //Visualiza el modal para seleccionar el asiento.
                controller = GL_ROOT + "LedgerEntry/GetLedgerEntries?branchId=" + $("#ddlBranch").val() + "&year=" +
                    $("#ledgerEntryYear").val() + "&month=" + $("#ddlLedgerEntryMonth").val() + "&entryNumber=" +
                    $("#transactionNumber").val() + "&destinationId=" + $("#ddlDestination").val() + "&accountingMovementTypeId=" +
                    $("#ddlAccountingMovementType").val() + "&isFiltered=" + filtered;

                $("#tblLedgerEntryItems").UifDataTable({ source: controller });
                $('#ledgerEntrySearchModal').UifModal('showLocal', Resources.MovementsConsultationMayorSeats);
            } else {
                    controller = GL_ROOT + "LedgerEntry/GetLedgerEntries?branchId=" + $("#ddlBranch").val() + "&year=" +
                    $("#ledgerEntryYear").val() + "&month=" + $("#ddlLedgerEntryMonth").val() + "&entryNumber=" + $("#transactionNumber").val() +
                    "&destinationId=" + $("#ddlDestination").val() + "&accountingMovementTypeId=" +
                    $("#ddlAccountingMovementType").val() + "&isFiltered=" + filtered;

                $("#tblEntries").UifDataTable({ source: controller });

                setTimeout(function () {
                    if ($("#tblEntries").UifDataTable('getData').length == 0) {
                        $("#mainAlert").UifAlert('show', Resources.NoRecordsFound, "warning");
                    }
                }, 5000);
            }
        }
    }

    /**
        * Permite limpiar los filtros de la búsqueda de asiento.
        *
        * @param {String} event    - Click.
        */
    ClearLedgerEntrySearch(event) {
        MainLedgerEntrySearch.ClearFieldsLedgerEntrySearch();
        $("#mainAlert").UifAlert('hide');
    }

    /**
        * Permite seleccionar un movimiento para su edición.
        *
        * @param {String} event - Seleccionar.
        * @param {Object} data  - Objeto con valores del movimiento seleccionado.
        */
    RowSelectedTableLedgerEntry(event, data) {
        //Se llena la tabla de centro de costos
        var controller = GL_ROOT + "Base/GetCostCentersByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + false;
        $("#tblCostCenter").UifDataTable({ source: controller });

        controller = GL_ROOT + "Base/GetEntryAnalysesByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + false;
        $("#tblAnalysis").UifDataTable({ source: controller });

        controller = GL_ROOT + "Base/GetPostdatedByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + false;
        $("#tblPostdated").UifDataTable({ source: controller });
    }

    /**
        * Permite reversar el asiento de mayor.
        *
        * @param {String} event    - Click.
        */
    RevertLedgerEntry(event) {
        if ($('#tblEntries').UifDataTable('getData').length != 0) {
            if ($('#tblEntries').UifDataTable('getData')[0].Status == $("#ViewBagReverted").val()) {
                $("#mainAlert").UifAlert('show', Resources.EntryAlreadyReversed, "warning");
            } else {
                $("#entryConsultationModalMessageTitle").text(Resources.Warning);
                $("#entryConsultationMessageModal").text(Resources.ToReverseTransaction);
                $('#revertLedgerEntryModal').UifModal('showLocal', Resources.ConsultationReversion);
            }
        } else {
            $("#mainAlert").UifAlert('show', Resources.SelectedAccountBeforeContinuing, "warning");
        }
    }

    /**
        * Permite reversar el asiento de mayor.
        *
        */
    LedgerEntryModalYes() {
        $('#revertLedgerEntryModal').modal('hide');
        var destinationId = $("#ddlDestination").val() === "" ? 0 : $("#ddlDestination").val();
        var accountingMovementTypeId = $("#ddlAccountingMovementType").val() === "" ? 0 : $("#ddlAccountingMovementType").val();
        $.ajax({
            type: "POST",
            url: GL_ROOT + "LedgerEntry/LedgerEntryRevertion",
            data: JSON.stringify({
                "branchId": $("#ddlBranch").val(), "year": $("#ledgerEntryYear").val(), "month": $("#ddlLedgerEntryMonth").val(),
                "entryNumber": $("#transactionNumber").val(), "destinationId": destinationId,
                "accountingMovementTypeId": accountingMovementTypeId,
                "accountingDate": MainLedgerEntrySearch.GetAccountingDateRevertion(),
                "ledgerEntryId": MainLedgerEntrySearch.GetLedgerEntryId()
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {

            if (data.success == false) {

                $("#mainAlert").UifAlert('show', data.result, "danger");
            } else {
                MainLedgerEntrySearch.ClearFieldsLedgerEntrySearch();
                $("#mainAlert").UifAlert('show', Resources.EntrySucessfullyReversed + " " + data.result, "success");
            }
        });
    }

    /**
        * Permite imprimir el asiento de mayor.
        *
        * @param {String} event    - Click.
        */
    PrintLedgerEntry(event) {
        if ($('#tblEntries').UifDataTable('getData').length != 0) {
            var entryNumber = $('#transactionNumber').val();
            MainLedgerEntrySearch.ShowReportledgerEntrySearch(entryNumber);
        } else {
            $("#mainAlert").UifAlert('show', Resources.SelectedAccountBeforeContinuing, "warning");
        }
    }

    /**
        * Permite seleccionar un movimiento para su reversión.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del movimiento seleccionado.
        */
    RowSelectedTableLedgerEntryItems(event, selectedItem) {
        ledgerEntryRecord = selectedItem;
    }

    /**
        * Permite obtener el detalle del asiento de mayor.
        *
        */
    GetLedgerEntry() {
        if (ledgerEntryRecord == null) {
            $("#searchAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
        } else {
            $("#searchAlert").UifAlert('hide');

            var accountingDate = ledgerEntryRecord.Date.split("/");
            var year = accountingDate[2].split(" ")[0];
            var month = accountingDate[1];


            var controller = GL_ROOT + "LedgerEntry/GetLedgerEntries?branchId=" + ledgerEntryRecord.BranchId + "&year=" + year +
                             "&month=" + month + "&entryNumber=" + ledgerEntryRecord.EntryNumber + "&destinationId=" +
                             ledgerEntryRecord.EntryDestinationId + "&accountingMovementTypeId=" + ledgerEntryRecord.AccountingMovementTypeId +
                             "&isFiltered=" + false;

            $("#tblEntries").UifDataTable({ source: controller });
            $("#ddlBranch").val(ledgerEntryRecord.BranchId);

            if (ledgerEntryRecord.EntryDestinationId > 0) {
                $("#ddlDestination").val(ledgerEntryRecord.EntryDestinationId);
            }
            if (ledgerEntryRecord.AccountingMovementTypeId > 0) {
                $("#ddlAccountingMovementType").val(ledgerEntryRecord.AccountingMovementTypeId);
            }

            $('#ledgerEntrySearchModal').UifModal('hide');

            setTimeout(function () {
                if ($("#tblEntries").UifDataTable('getData').length == 0) {
                    $("#mainAlert").UifAlert('show', Resources.NoRecordsFound, "warning");
                }
            }, 5000);
        }
    }

    /**
        * Permite cerrar el modal de consulta del asiento de mayor.
        *
        */
    CloseLedgerEntryModal() {
        $("#searchAlert").UifAlert('hide');
        $("#tblLedgerEntryItems").dataTable().fnClearTable();
    }



    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Permite limpiar los campos de filtro de la consulta del asiento de mayor.
        *
        */
    static ClearFieldsLedgerEntrySearch() {
        //campos de búsqueda
        $("#ddlBranches").val("");
        $("#entryYear").val("");
        $("#entryMonth").val("");
        $("#transactionNumber").val("");

        //reinicia el formulario de búsqueda
        $("#searchForm").formReset();

        //limpia la tabla de movimientos
        $('#tblEntries').dataTable().fnClearTable();
        $('#tblCostCenter').dataTable().fnClearTable();
        $('#tblAnalysis').dataTable().fnClearTable();
        $('#tblPostdated').dataTable().fnClearTable();

        //borra el registro de asiento
        ledgerEntryRecord = null;
    }

    /**
        * Permite limpiar las tablas del asiento de mayor.
        *
        */
    static ClearTablesLedgerEntrySearch() {
        $('#tblEntries').dataTable().fnClearTable();
        $('#tblCostCenter').dataTable().fnClearTable();
        $('#tblAnalysis').dataTable().fnClearTable();
        $('#tblPostdated').dataTable().fnClearTable();
        $('#EntryConsultationFilter').dataTable().fnClearTable();
    }

    /**
        * Permite obtener los débitos y créditos del asiento de mayor.
        *
        */
    static GetDebitsAndCredits() {
        var result = false;
        var debits = 0;
        var credits = 0;

        var data = $("#tblEntries").UifDataTable('getData');

        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].AccountingNatureId == 2) //débito
                    debits = debits + parseFloat(((parseFloat(data[i].ExchangeRate.replace(",", ".")) * parseFloat(data[i].Amount.replace(",", ".")))).toFixed(2));
                if (data[i].AccountingNatureId == 1) //crédito
                    credits = credits + parseFloat(((parseFloat(data[i].ExchangeRate.replace(",", ".")) * parseFloat(data[i].Amount.replace(",", ".")))).toFixed(2));
            }
        }
    }

    /**
        * Permite visualizar el reporte del asiento de mayor en formato pdf.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del movimiento seleccionado.
        */
    static ShowReportledgerEntrySearch(entryNumber) {
        var destinationId = $("#ddlDestination").val() === "" ? 0 : $("#ddlDestination").val();
        var accountingMovementTypeId = $("#ddlAccountingMovementType").val() === "" ? 0 : $("#ddlAccountingMovementType").val();

        window.open(GL_ROOT + "LedgerEntry/GenerateLedgerEntryReport?branchId="
            + $("#ddlBranch").val() + '&year=' + $("#ledgerEntryYear").val() + '&month='
            + $("#ddlLedgerEntryMonth").val() + '&entryNumber=' + entryNumber + '&destinationId=' + destinationId
            + '&accountingMovementTypeId=' + accountingMovementTypeId, 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }

    /**
        * Permite obtener la fecha contable en la que fue generado el asiento de mayor.
        *
        */
    static GetAccountingDateRevertion() {
        var accountingDate = "";

        var data = $("#tblEntries").UifDataTable('getData');

        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                accountingDate = data[i].Date;
            }
        }

        return accountingDate;
    }

    /**
    * Permite obtener el identificador de cabecera con lo que fue generado el asiento.
    *
    */
    static GetLedgerEntryId() {
        var ledgerEntryId = 0;

        var data = $("#tblEntries").UifDataTable('getData');

        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                ledgerEntryId = data[i].DailyEntryHeaderId;
            }
        }

        return ledgerEntryId;
    }

}