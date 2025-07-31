$(() => {
    new JournalEntrySearch();
});

class JournalEntrySearch extends Uif2.Page {
    getInitialState() {
        $("#entryMovements").UifDataTable();
        $("#tblCostCenter").UifDataTable();
        $("#tblAnalysis").UifDataTable();
        $("#tblPostdated").UifDataTable();
    }
    bindEvents() {
        $("#searchJournalEntry").on("click", this.SearchJournalEntry)
        $("#clearJournalEntrySearch").on("click", this.ClearJournalEntrySearch)
        $("#revertJournalEntry").on("click", this.ConfirmJournalEntryRevertion)
        $("#printJournalEntry").on("click", this.PrintJournalEntry)
        $("#entryYear").on('keypress', this.OnlyNumbers)
        $("#transactionNumber").on('keypress', this.OnlyNumbers)
        $("#entryMovements").on("rowSelected", this.LoadDetails)
        $("#revertConfirmationModalYes").on("click", this.Revert)
    }
    SearchJournalEntry() {
        clearTables();
        $("#entryHeaderDescription").val("");
        $("#mainAlert").UifAlert('hide');
        $("#searchForm").validate();
        if ($("#searchForm").valid()) {
            var branchId = $("#ddlBranches").val();
            var year = $("#entryYear").val();
            var month = $("#entryMonth").val();
            var entryNumber = $("#transactionNumber").val();
            LoadJournalEntryMovements(branchId, year, month, entryNumber).then(function (journalEntryData) {
                if (journalEntryData.length == 0) {
                    $("#mainAlert").UifAlert('show', Resources.NoRecordsFound, "warning");
                } else {
                    $("#entryHeaderDescription").val(journalEntryData[0].EntryHeaderDescription);

                    for (var i = 0; i < journalEntryData.length; i++) {
                        journalEntryData[i].AccountingNatureDescription = FormatCurrency(FormatDecimal(journalEntryData[i].AccountingNatureDescription));
                        journalEntryData[i].CreditsAmountValue = FormatCurrency(FormatDecimal(journalEntryData[i].CreditsAmountValue));
                        journalEntryData[i].DebitsAmountLocalValue = FormatCurrency(FormatDecimal(journalEntryData[i].DebitsAmountLocalValue));
                        journalEntryData[i].DebitsAmountValue = FormatCurrency(FormatDecimal(journalEntryData[i].DebitsAmountValue));
                        $("#entryMovements").UifDataTable('addRow', journalEntryData[i]);
                    }
                }
            });
        }
    }
    ClearJournalEntrySearch() {
        clearSearchFields();
        $("#mainAlert").UifAlert('hide');
    }
    ConfirmJournalEntryRevertion() {
        if ($('#entryMovements').UifDataTable('getData').length != 0) {
            if ($('#entryMovements').UifDataTable('getData')[0].Status == $("#ViewBagReverted").val()) {
                $("#mainAlert").UifAlert('show', Resources.EntryAlreadyReversed, "warning");
            } else {
                $('#revertConfirmationModal').UifModal('showLocal', Resources.ConsultationReversion);
            }
        } else {
            $("#mainAlert").UifAlert('show', Resources.NoRecordsToContinue, "warning");
        }
    }
    PrintJournalEntry() {
        if ($('#entryMovements').UifDataTable('getData').length != 0) {
            var entryNumber = $('#entryMovements').UifDataTable('getData')[0].DailyEntryHeaderId;
            showJournalEntrySearchReport(entryNumber);
        } else {
            $("#mainAlert").UifAlert('show', Resources.NoRecordsToContinue, "warning");
        }
    }
    OnlyNumbers() {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }
    LoadDetails(event, data) {
        //lleno la tabla de Centro de Costos
        var controller = GL_ROOT + "Base/GetCostCentersByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + true;
        $("#tblCostCenter").UifDataTable({ source: controller });

        var controllerAnalyses = GL_ROOT + "Base/GetEntryAnalysesByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + true;
        $("#tblAnalysis").UifDataTable({ source: controllerAnalyses });

        var controllerPostdated = GL_ROOT + "Base/GetPostdatedByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + true;
        $("#tblPostdated").UifDataTable({ source: controllerPostdated });
    }
    Revert() {
        $('#revertConfirmationModal').modal('hide');
        revertEntry($("#transactionNumber").val()).then(function (revertData) {
            if (revertData.success == false) {
                $("#mainAlert").UifAlert('show', revertData.result, "danger");
            } else {
                clearSearchFields();
                $("#mainAlert").UifAlert('hide');
                $("#mainAlert").UifAlert('show', Resources.EntrySucessfullyReversed + " " + revertData.result, "success");
            }
        });
    }
};

function clearTables() {
    $('#entryMovements').dataTable().fnClearTable();
    $('#tblCostCenter').dataTable().fnClearTable();
    $('#tblAnalysis').dataTable().fnClearTable();
    $('#tblPostdated').dataTable().fnClearTable();
}

function LoadJournalEntryMovements(branchId, year, month, entryNumber) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: GL_ROOT + "JournalEntry/SearchDailyEntryMovements",
            data: {
                "branchId": branchId,
                "year": year,
                "month": month,
                "entryNumber": entryNumber
            },
            success: function (journalEntryData) {
                resolve(journalEntryData);
            }
        });
    });
}

function clearSearchFields() {
    //campos de búsqueda
    $("#ddlBranches").val("");
    $("#entryYear").val("");
    $("#entryMonth").val("");
    $("#transactionNumber").val("");
    $("#entryHeaderDescription").val("");

    //reinicia el formulario de búsqueda
    $("#searchForm").formReset();

    //limpia la tabla de movimientos
    $('#entryMovements').dataTable().fnClearTable();
    $('#tblCostCenter').dataTable().fnClearTable();
    $('#tblAnalysis').dataTable().fnClearTable();
    $('#tblPostdated').dataTable().fnClearTable();
}

function showJournalEntrySearchReport(entryNumber) {
    window.open(GL_ROOT + "Reports/ShowDailyEntryConsultationReport?entryNumber="
        + entryNumber, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function revertEntry(journalEntryId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "JournalEntry/ReverseJournalEntry",
            data: {
                "journalEntryId": journalEntryId
            },
            success: function (revertData) {
                resolve(revertData);
            }
        })
    });
}