/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE MODELOS Y VARIABLES
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

var timeEntry = window.setInterval(validateCompanyEntry, 800);
var currentEditIndex = 0;
var rowSelected;
var postdatedExchangeRate = 0;
var tablePosition = -1;
var tableTypeId = 0;
var entryTypeId = 0;
var lastCostCenterPercentageValue = 0;
var entryEdit;
var entryCostCenterEdit;
var entryAnalysisEdit;
var entryPostDatedEdit;
var modalDelete;
var printDataEntryNumber = 0;
var dataTable = null;
var copyModal = null;
var chooseModal = null;
var documentNumberEntry = "";
var accounts = [];
var isEntryType = 0;

//Definición de modelos para la tabla de Movimientos
function RowModel() {
    this.BranchId;
    this.SalePointId;
    this.CompanyId;
    this.DestinationId;
    this.AccountingMovementTypeId;
    this.Date;
    this.AccountingAccountId;
    this.AccountingAccountNumber;
    this.AccountingAccountDescription;
    this.AccountingNatureId;
    this.AccountingNatureDescription;
    this.CurrencyId;
    this.CurrencyDescription;
    this.ExchangeRate;
    this.Amount;
    this.IndividualId;
    this.DocumentNumber;
    this.Name;
    this.ConceptId;
    this.ConceptDescription;
    this.BankReconciliationId;
    this.BankReconciliationDescription;
    this.ReceiptNumber;
    this.ReceiptDate;
    this.Description;
    this.CostCenters = [];
    this.Analyses = [];
    this.Postdated = [];
}

//Definición de modelos para la tabla de Centro de Costos
function CostCenterRowModel() {
    this.CostCenterId;
    this.Description;
    this.PercentageAmount;
}

//Definición de modelos para la tabla de Análisis
function AnalysisRowModel() {
    this.AnalysisId;
    this.AnalysConceptId;
    this.Key;
    this.Description;
}

//Definición de modelos para la tabla de Posfechados
function PostDatedRowModel() {
    this.PostDateTypeId;
    this.DocumentNumber;
    this.CurrencyId;
    this.ExchangeRate;
    this.IssueAmount;
    this.LocalAmount;
}

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#ddlAccountingCompanies").attr("disabled", "disabled");
$("#ddlAccountingCompanies").prop("disabled", "disabled");

getDate();
LoadEntryTypes();
setTimeout(function () {
    try {
        getAccountingDate(Resources.AccountingModuleDateId);
        setSalePointEntry();
    } catch (e) {}
}, 400);

$('#btnAddMovement').click(function () {
    isEntryType = 0;
    $("#mainForm").validate();
    if ($("#mainForm").valid()) {
        if (entryTypeId != 0) {
            $("#EntryTypeModalWarning").UifModal('showLocal', Resources.Warning);
        } else {
            $('#entryModalAdd').UifModal('showLocal', Resources.AddMovement);
            LockFields();
            entryTypeId = 0;
        }
    }
});

//Evento de selección en tabla de movimientos
$('#entryTable').on('rowSelected', function (event, data) {
    rowSelected = data;

    //Lleno los centros de costos asociados
    if (rowSelected.CostCenters.length > 0) {

        $('#tblCostCenter').dataTable().fnClearTable();

        for (var i = 0; i < rowSelected.CostCenters.length; i++) {
            $('#tblCostCenter').UifDataTable('addRow', rowSelected.CostCenters[i]);
        }

        //valido que los centros de costos lleguen al 100%
        if (CostCenterPercentageTotal() < 100) {}
    } else {
        $('#tblCostCenter').dataTable().fnClearTable();
    }

    //lleno los analisis asociados
    if (rowSelected.Analyses.length > 0) {
        $('#tblAnalysis').dataTable().fnClearTable();
        for (var j = 0; j < rowSelected.Analyses.length; j++) {
            $('#tblAnalysis').UifDataTable('addRow', rowSelected.Analyses[j]);
        }
    } else {
        $('#tblAnalysis').dataTable().fnClearTable();
    }

    //lleno los postfechados asociados
    if (rowSelected.Postdated.length > 0) {
        $('#tblPostdated').dataTable().fnClearTable();
        for (var k = 0; k < rowSelected.Postdated.length; k++) {
            $('#tblPostdated').UifDataTable('addRow', rowSelected.Postdated[k]);
        }
    } else {
        $('#tblPostdated').dataTable().fnClearTable();
    }
});

$("#entryTypes").on('click', "li", function (event, data, position) {
    isEntryType = 1;
    entryTypeId = $(this).attr('value');
    $("#mainForm").validate();
    if ($("#mainForm").valid()){
        if ($('#entryTable').UifDataTable('getData').length != 0) {
            $("#EntryTypeModalWarning").UifModal("showLocal", Resources.Warning);
        } else
            AddEntryType(entryTypeId);
    }    
});

//Evento al dar clic en ventana de advertencia
$("#modalChoose").on('click', "#btnChoose", function (event, data, position) {
    $('#entryTable').dataTable().fnClearTable();
    $('#tblCostCenter').dataTable().fnClearTable();
    $('#tblAnalysis').dataTable().fnClearTable();
    if (entryTypeId != 0) {
        AddEntryType(entryTypeId);
    }
});

//Evento de eliminación en tabla de movimientos
$('#entryTable').on('rowDelete', function (event, data, position) {


    $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

    tablePosition = position;
    tableTypeId = 4;

});

//Evento de edición en tabla de movimientos
$('#entryTable').on('rowEdit', function (event, data, position) {

    entryEdit = $("#entryModalEdit").detach();
    currentEditIndex = position;

    entryEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
    $("#entryEditForm").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
    $("#entryModalEdit").appendTo("#entryEditForm"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

    $("#entryEditForm").find("#AccountingAccountId").val(data.AccountingAccountId);
    $("#entryEditForm").find("#conceptId").val(data.ConceptId);
    $("#entryEditForm").find("#conceptDescription").val(data.ConceptDescription);
    $("#entryEditForm").find("#accountingAccountNumber").val(data.AccountingAccountNumber);
    $("#entryEditForm").find("#accountingAccountDescription").val(data.AccountingAccountDescription);
    $("#entryEditForm").find("#AccountingNatureId").val(data.AccountingNatureId);
    $("#entryEditForm").find("#ddlCurrencies").val(data.CurrencyId);
    $("#entryEditForm").find("#exchange").val(data.ExchangeRate);
    $("#entryEditForm").find("#Amount").val(data.AmountFormated);
    $("#entryEditForm").find("#IndividualId").val(data.IndividualId);
    $("#entryEditForm").find("#entryPayerDocumentNumber").val(data.DocumentNumber);
    $("#entryEditForm").find("#entryPayerName").val(data.Name);
    $("#entryEditForm").find("#Description").val(data.Description);
    $("#entryEditForm").find("#BankReconciliationId").val(data.BankReconciliationId);
    $("#entryEditForm").find("#ReceiptNumber").val(data.ReceiptNumber);
    $("#entryEditForm").find("#receiptDate").val(data.ReceiptDate);

    rowSelected = data;

    //Lleno los centros de costos asociados
    if (rowSelected.CostCenters.length > 0) {
        $('#tblCostCenter').dataTable().fnClearTable();

        for (var i = 0; i < rowSelected.CostCenters.length; i++) {
            $('#tblCostCenter').UifDataTable('addRow', rowSelected.CostCenters[i]);
        }
    } else {
        $('#tblCostCenter').dataTable().fnClearTable();
    }


    //lleno los analisis asociados
    if (rowSelected.Analyses.length > 0) {
        $('#tblAnalysis').dataTable().fnClearTable();
        for (var j = 0; j < rowSelected.Analyses.length; j++) {
            $('#tblAnalysis').UifDataTable('addRow', rowSelected.Analyses[j]);

        }
    } else {
        $('#tblAnalysis').dataTable().fnClearTable();
    }

    //lleno los postfechados asociados
    if (rowSelected.Postdated.length > 0) {
        $('#tblPostdated').dataTable().fnClearTable();
        for (var k = 0; k < rowSelected.Postdated.length; k++) {
            $('#tblPostdated').UifDataTable('addRow', rowSelected.Postdated[k]);
        }
    } else {
        $('#tblPostdated').dataTable().fnClearTable();
    }
});

// Evento para Agregar en la tabla de movimientos
$('#btnEntrySaveAdd').click(function () {
    if ($("#entryAddForm").find("#AccountingAccountId").val() > 0) {
        if ($("#entryAddForm").find("#IndividualId").val() > 0 || $("#entryAddForm").find("#IndividualId").val() != "") {
            $("#entryAddForm").validate();
            if ($("#entryAddForm").valid()) {

                var exchangeRate = 0;
                var rowModel = new RowModel();

                rowModel.BranchId = $("#mainForm").find("#ddlBranches").val();
                rowModel.SalePointId = $("#mainForm").find("#ddlSalePoints").val();
                rowModel.CompanyId = $("#mainForm").find("#ddlAccountingCompanies").val();
                rowModel.DestinationId = $("#mainForm").find("#ddlDestinations").val();
                rowModel.AccountingMovementTypeId = $("#mainForm").find("#ddlAccountingMovementTypes").val();
                rowModel.Date = $("#mainForm").find("#Date").val();
                rowModel.AccountingAccountId = $("#entryAddForm").find("#AccountingAccountId").val();
                rowModel.AccountingAccountNumber = $("#entryAddForm").find("#accountingAccountNumber").val();
                rowModel.AccountingAccountDescription = $("#entryAddForm").find("#accountingAccountDescription").val();
                rowModel.AccountingNatureId = $("#entryAddForm").find("#AccountingNatureId").val();
                rowModel.AccountingNatureDescription = $("#entryAddForm").find("#AccountingNatureId option:selected").text();
                rowModel.CurrencyId = $("#mainForm").find("#ddlCurrencies").val();
                rowModel.CurrencyDescription = $("#mainForm").find("#ddlCurrencies option:selected").text();
                exchangeRate = $("#mainForm").find("#exchange").val();
                rowModel.ExchangeRate = exchangeRate.replace(",", ".");
                rowModel.ExchangeRateFormated = exchangeRate; //.replace(",", ".");
                rowModel.Amount = $("#entryAddForm").find("#Amount").val().replace(",", ".");
                rowModel.AmountFormated = $("#entryAddForm").find("#Amount").val(); //.replace(",", ".");
                rowModel.IndividualId = $("#entryAddForm").find("#IndividualId").val();
                rowModel.DocumentNumber = $("#entryAddForm").find("#entryPayerDocumentNumber").val();
                rowModel.Name = $("#entryAddForm").find("#entryPayerName").val();
                rowModel.ConceptId = $("#entryAddForm").find("#conceptId").val();
                rowModel.ConceptDescription = $("#entryAddForm").find("#conceptDescription").val();
                rowModel.BankReconciliationId = $("#entryAddForm").find("#BankReconciliationId").val();
                rowModel.BankReconciliationDescription = $("#entryAddForm").find("#BankReconciliationId option:selected").text();
                rowModel.ReceiptNumber = $("#entryAddForm").find("#ReceiptNumber").val();
                rowModel.ReceiptDate = $("#entryAddForm").find("#receiptDate").val();
                rowModel.Description = $("#entryAddForm").find("#Description").val();

                $('#entryTable').UifDataTable('addRow', rowModel);
                $("#entryAddForm").formReset();
                $('#entryModalAdd').UifModal('hide');
                $('#tblCostCenter').dataTable().fnClearTable();
                $('#tblAnalysis').dataTable().fnClearTable();
                $('#tblPostdated').dataTable().fnClearTable();
                rowSelected = null;
            }
        } else {
            $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.NoRegisteredPerson, "info");
        }
    } else {
        $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.NoEnteredAccountingAccount, "info");
    }

});

// Evento para Editar en la tabla de movimientos
$('#btnEntrySaveEdit').click(function () {
    if ($("#entryEditForm").find("#AccountingAccountId").val() > 0) {
        if ($("#entryEditForm").find("#IndividualId").val() > 0 || $("#entryEditForm").find("#IndividualId").val() != "") {
            $("#entryEditForm").validate();

            if ($("#entryEditForm").valid()) {

                var exchangeRate = 0;
                var rowModel = new RowModel();

                rowModel.BranchId = $("#mainForm").find("#ddlBranches").val();
                rowModel.SalePointId = $("#mainForm").find("#ddlSalePoints").val();
                rowModel.CompanyId = $("#mainForm").find("#ddlAccountingCompanies").val();
                rowModel.DestinationId = $("#mainForm").find("#ddlDestinations").val();
                rowModel.AccountingMovementTypeId = $("#mainForm").find("#ddlAccountingMovementTypes").val();
                rowModel.Date = $("#mainForm").find("#Date").val();
                rowModel.AccountingAccountId = $("#entryEditForm").find("#AccountingAccountId").val();
                rowModel.AccountingAccountNumber = $("#entryEditForm").find("#accountingAccountNumber").val();
                rowModel.AccountingAccountDescription = $("#entryEditForm").find("#accountingAccountDescription").val();
                rowModel.AccountingNatureId = $("#entryEditForm").find("#AccountingNatureId").val();
                rowModel.AccountingNatureDescription = $("#entryEditForm").find("#AccountingNatureId option:selected").text();
                rowModel.CurrencyId = $("#mainForm").find("#ddlCurrencies").val();
                rowModel.CurrencyDescription = $("#mainForm").find("#ddlCurrencies option:selected").text();
                exchangeRate = $("#mainForm").find("#exchange").val();
                rowModel.ExchangeRate = exchangeRate.replace(",", ".");
                rowModel.ExchangeRateFormated = exchangeRate; //.replace(",", ".");
                rowModel.Amount = $("#entryEditForm").find("#Amount").val().replace(",", ".");
                rowModel.AmountFormated = $("#entryEditForm").find("#Amount").val(); //.replace(",", ".");
                rowModel.IndividualId = $("#entryEditForm").find("#IndividualId").val();
                rowModel.DocumentNumber = $("#entryEditForm").find("#entryPayerDocumentNumber").val();
                rowModel.Name = $("#entryEditForm").find("#entryPayerName").val();
                rowModel.ConceptId = $("#entryEditForm").find("#conceptId").val();
                rowModel.ConceptDescription = $("#entryEditForm").find("#conceptDescription").val();
                rowModel.BankReconciliationId = $("#entryEditForm").find("#BankReconciliationId").val();
                rowModel.BankReconciliationDescription = $("#entryEditForm").find("#BankReconciliationId option:selected").text();
                rowModel.ReceiptNumber = $("#entryEditForm").find("#ReceiptNumber").val();
                rowModel.ReceiptDate = $("#entryEditForm").find("#receiptDate").val();
                rowModel.Description = $("#entryEditForm").find("#Description").val();
                rowModel.CostCenters = $('#tblCostCenter').UifDataTable('getData');
                rowModel.Analyses = $('#tblAnalysis').UifDataTable('getData');

                $('#entryTable').UifDataTable('editRow', rowModel, currentEditIndex);
                $("#entryEditForm").formReset();
                $('#entryModalEdit').UifModal('hide');
            }
        } else {
            $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.NoRegisteredPerson, "info");
        }
    } else {
        $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.NoEnteredAccountingAccount, "info");
    }
});

//Evento si se cierra el modal de movimientos.
$("#btnEntryCancelAdd").click(function () {
    ClearModalFields();
    $("#entryAddForm").formReset();
    if ($('#entryTable').UifDataTable('getData').length == 0)
        UnlockFields();
});

//Evento si se cierra el modal de edicion de movimientos.
$("#btnEntryCancelEdit").click(function () {
    ClearModalFields();
    $("#entryEditForm").formReset();
    if ($('#entryTable').UifDataTable('getData').length == 0)
        UnlockFields();
});

// Evento de la tabla tblCostCenter utilizada solo para Ingreso de Centros de Costos
$('#tblCostCenter').on('rowAdd', function () {

    if (rowSelected == undefined) {
        $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
    } else {
        $("#recordAlert").UifAlert('hide');
        $('#entryModalAddFrmCostCenter').appendTo("body").UifModal('showLocal', Resources.AddCostCenter);
        $("#entryAddFrmCostCenter").appendTo("body");
        $("#entryModalAddFrmCostCenter").appendTo("#entryAddFrmCostCenter");
    }
});

// Evento de la tabla tblCostCenter para eliminar registros
$('#tblCostCenter').on('rowDelete', function (event, data, position) {

    $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

    tablePosition = position;
    tableTypeId = 1;
});

// Evento de la tabla tblCostCenter para editar registros
$('#tblCostCenter').on('rowEdit', function (event, data, position) {

    entryCostCenterEdit = $("#entryModalEditFrmCostCenter").detach();
    costCenterCurrentEditIndex = position;

    entryCostCenterEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
    $("#entryEditFrmCostCenter").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
    $("#entryModalEditFrmCostCenter").appendTo("#entryEditFrmCostCenter"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

    $("#entryEditFrmCostCenter").find("#CostCenterId").val(data.CostCenterId);
    $("#entryEditFrmCostCenter").find("#Description").val(data.Description);
    $("#entryEditFrmCostCenter").find("#PercentageAmount").val(data.PercentageAmountFormated);
});

// Evento para Agregar en la tabla de Centros de Costo
$('#btnSaveAddCostCenter').click(function () {

    $("#entryAddFrmCostCenter").validate();
    if ($("#entryAddFrmCostCenter").valid()) {

        var costCenterRowModel = new CostCenterRowModel();

        costCenterRowModel.CostCenterId = $("#entryAddFrmCostCenter").find("#CostCenterId").val();
        costCenterRowModel.Description = $("#entryAddFrmCostCenter").find("#Description").val();
        costCenterRowModel.PercentageAmount = $("#entryAddFrmCostCenter").find("#PercentageAmount").val().replace(",", ".");
        costCenterRowModel.PercentageAmountFormated = $("#entryAddFrmCostCenter").find("#PercentageAmount").val();

        if (!IsDuplicatedCostCenters(costCenterRowModel.CostCenterId)) {
            if ((((CostCenterPercentageTotal() + parseFloat(parseFloat(costCenterRowModel.PercentageAmount.replace(",", ".")).toFixed(2))) <= 100))) {

                rowSelected.CostCenters.push(costCenterRowModel);

                $('#tblCostCenter').UifDataTable('addRow', costCenterRowModel);
                $("#entryAddFrmCostCenter").formReset();
                $('#entryModalAddFrmCostCenter').UifModal('hide');
                $("#entryModalAddFrmCostCenter").find("#entryCostCenterAlert").UifAlert('hide');

            } else {
                $("#entryModalAddFrmCostCenter").find("#entryCostCenterAlert").UifAlert('show', Resources.ValidateCostCenterPercentage, "danger");
            }
        } else {
            $("#entryModalAddFrmCostCenter").find("#entryCostCenterAlert").UifAlert('show', Resources.DuplicatedRecord, "danger");
        }
    }
});

// Evento para Editar en la tabla de Centros de Costo
$('#btnSaveEditCostCenter').click(function () {
    $("#entryEditFrmCostCenter").validate();
    if ($("#entryEditFrmCostCenter").valid()) {

        var costCenterRowModel = new CostCenterRowModel();

        costCenterRowModel.CostCenterId = $("#entryEditFrmCostCenter").find("#CostCenterId").val();
        costCenterRowModel.Description = $("#entryEditFrmCostCenter").find("#Description").val();
        costCenterRowModel.PercentageAmount = $("#entryEditFrmCostCenter").find("#PercentageAmount").val().replace(",", ".");
        costCenterRowModel.PercentageAmountFormated = $("#entryEditFrmCostCenter").find("#PercentageAmount").val();
        $('#tblCostCenter').UifDataTable('editRow', costCenterRowModel, costCenterCurrentEditIndex);
        rowSelected.CostCenters[costCenterCurrentEditIndex] = costCenterRowModel; //actualizo el registro en la variable rowSelected

        if (CostCenterPercentageTotal() > 100) {
            $("#entryModalEditFrmCostCenter").find("#entryCostCenterAlert").UifAlert('show', Resources.SumPercentagesCostCentersDifferent, "danger");
        } else {
            $("#entryEditFrmCostCenter").formReset();
            $('#entryModalEditFrmCostCenter').UifModal('hide');
            $("#entryModalEditFrmCostCenter").find("#entryCostCenterAlert").UifAlert('hide');
        }
    }
});

// Evento para cancelar en la tabla de Centros de Costo
$('#btnCancelAddCostCenter').click(function () {
    $("#entryAddFrmCostCenter").find("#CostCenterId").val("");
    $("#entryAddFrmCostCenter").find("#Description").val("");
    $("#entryAddFrmCostCenter").find("#PercentageAmount").val("");
    $("#entryAddFrmCostCenter").formReset();
});

// Evento para cerrar modal de Centros de Costo en edicion
$('#btnCancelEditCostCenter').click(function () {

    var costCenterRowModel = new CostCenterRowModel();

    costCenterRowModel.CostCenterId = $("#entryEditFrmCostCenter").find("#CostCenterId").val();
    costCenterRowModel.Description = $("#entryEditFrmCostCenter").find("#Description").val();
    costCenterRowModel.PercentageAmount = lastCostCenterPercentageValue.replace(",", ".");
    costCenterRowModel.PercentageAmountFormated = lastCostCenterPercentageValue;
    $('#tblCostCenter').UifDataTable('editRow', costCenterRowModel, costCenterCurrentEditIndex);
    rowSelected.CostCenters[costCenterCurrentEditIndex] = costCenterRowModel; //actualizo el registro en la variable rowSelected

    $("#entryEditFrmCostCenter").find("#CostCenterId").val("");
    $("#entryEditFrmCostCenter").find("#Description").val("");
    $("#entryEditFrmCostCenter").find("#PercentageAmount").val("");
    $("#entryEditFrmCostCenter").formReset();
});

// Evento de la tabla tblAnalysis utilizada para Ingreso de Análisis
$('#tblAnalysis').on('rowAdd', function () {
    if (rowSelected == undefined) {
        $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
    } else {
        $("#recordAlert").UifAlert('hide');
        $('#entryModalAddFrmAnalysis').appendTo("body").UifModal('showLocal', Resources.AnalysisAdd); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
        $("#entryAddFrmAnalysis").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
        $("#entryModalAddFrmAnalysis").appendTo("#entryAddFrmAnalysis"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.
    }
});

// Evento de la tabla tblAnalysis para eliminar registros
$('#tblAnalysis').on('rowDelete', function (event, data, position) {
    $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

    tablePosition = position;
    tableTypeId = 2;

});

// Evento de la tabla tblAnalysis para editar registros
$('#tblAnalysis').on('rowEdit', function (event, data, position) {

    entryAnalysisEdit = $("#entryModalEditFrmAnalysis").detach();
    analysisCurrentEditIndex = position;

    entryAnalysisEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
    $("#entryEditFrmAnalysis").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
    $("#entryModalEditFrmAnalysis").appendTo("#entryEditFrmAnalysis"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

    $("#entryEditFrmAnalysis").find("#AnalysisId").val(data.AnalysisId);
    var controller = GL_ROOT + "Base/GetPaymentConceptsByAnalysisCode?analysisCodeId=" + data.AnalysisId;
    $("#entryModalEditFrmAnalysis").find("#AnalysConceptId").UifSelect({
        source: controller,
        selectedId: data.AnalysConceptId
    });

    $("#entryEditFrmAnalysis").find("#Key").val(data.Key);
    $("#entryEditFrmAnalysis").find("#Description").val(data.Description);

});

// Evento para Agregar en la tabla de Análisis
$('#btnCancelSaveAddAnalysis').click(function () {
    $("#entryAddFrmAnalysis").validate();
    if ($("#entryAddFrmAnalysis").valid()) {
        var analysisRowModel = new AnalysisRowModel();

        analysisRowModel.AnalysisId = $("#entryAddFrmAnalysis").find("#AnalysisId").val();
        analysisRowModel.AnalysisDescription = $("#entryAddFrmAnalysis").find("#AnalysisId option:selected").text();
        analysisRowModel.AnalysConceptId = $("#entryAddFrmAnalysis").find("#AnalysConceptId").val();
        analysisRowModel.AnalysConceptDescription = $("#entryAddFrmAnalysis").find("#AnalysConceptId option:selected").text();
        analysisRowModel.Key = $("#entryAddFrmAnalysis").find("#Key").val();
        analysisRowModel.Description = $("#entryAddFrmAnalysis").find("#Description").val();

        if (!IsDuplicatedAnalysis(analysisRowModel.AnalysisId, analysisRowModel.AnalysConceptId, analysisRowModel.Key)) {
            rowSelected.Analyses.push(analysisRowModel);

            $('#tblAnalysis').UifDataTable('addRow', analysisRowModel);
            $("#entryAddFrmAnalysis").formReset();
            $('#entryModalAddFrmAnalysis').UifModal('hide');
            $("#entryModalAddFrmAnalysis").find("#entryAnalysisAlert").UifAlert('hide');
        } else {
            $("#entryModalAddFrmAnalysis").find("#entryAnalysisAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
        }
    }
});

// Evento para Editar en la tabla de Análisis
$('#btnEntrySaveEditAnalysis').click(function () {
    $("#entryEditFrmAnalysis").validate();
    if ($("#entryEditFrmAnalysis").valid()) {

        var analysisRowModel = new AnalysisRowModel();

        analysisRowModel.AnalysisId = $("#entryEditFrmAnalysis").find("#AnalysisId").val();
        analysisRowModel.AnalysisDescription = $("#entryEditFrmAnalysis").find("#AnalysisId option:selected").text();
        analysisRowModel.AnalysConceptId = $("#entryEditFrmAnalysis").find("#AnalysConceptId").val();
        analysisRowModel.AnalysConceptDescription = $("#entryEditFrmAnalysis").find("#AnalysConceptId option:selected").text();
        analysisRowModel.Key = $("#entryEditFrmAnalysis").find("#Key").val();
        analysisRowModel.Description = $("#entryEditFrmAnalysis").find("#Description").val();

        if (!IsDuplicatedAnalysis(analysisRowModel.AnalysisId, analysisRowModel.AnalysConceptId, analysisRowModel.Key)) {

            $('#tblAnalysis').UifDataTable('editRow', analysisRowModel, analysisCurrentEditIndex);
            rowSelected.Analyses[analysisCurrentEditIndex] = analysisRowModel; //actualizo el registro en la variable rowSelected
            $("#entryEditFrmAnalysis").formReset();
            $('#entryModalEditFrmAnalysis').UifModal('hide');
            $("#entryModalEditFrmAnalysis").find("#entryAnalysisAlert").UifAlert('hide');
        } else {
            $("#entryModalEditFrmAnalysis").find("#entryAnalysisAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
        }
    }
});

// Evento para cerrar modal de analisis
$('#btnEntryCancelAddAnalysis').click(function () {
    $("#entryAddFrmAnalysis").formReset();
});

// Evento para cerrar modal de analisis en edicion
$('#btnEntryCancelEditAnalysis').click(function () {
    $("#entryEditFrmAnalysis").formReset();
});

// Evento de la tabla tblPostdated utilizada para Ingreso de Posfechados
$('#tblPostdated').on('rowAdd', function () {
    if (rowSelected == undefined) {
        $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
    } else {
        $("#recordAlert").UifAlert('hide');
        $('#entryModalAddFrmPostdated').appendTo("body").UifModal('showLocal', Resources.PostDatedAdd); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
        $("#entryAddFrmPostdated").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
        $("#entryModalAddFrmPostdated").appendTo("#entryAddFrmPostdated"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.
    }
});

// Evento de la tabla tblPostdated para eliminar registros de postfechados
$('#tblPostdated').on('rowDelete', function (event, data, position) {

    $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

    tablePosition = position;
    tableTypeId = 3;

});

// Evento de la tabla tblPostdated para editar registros de postfechados
$('#tblPostdated').on('rowEdit', function (event, data, position) {

    entryPostDatedEdit = $("#entryModalEditFrmPostdated").detach();
    postdatedCurrentEditIndex = position;

    entryPostDatedEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
    $("#entryEditFrmPostdated").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
    $("#entryModalEditFrmPostdated").appendTo("#entryEditFrmPostdated"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

    $("#entryEditFrmPostdated").find("#PostDateTypeId").val(data.PostDateTypeId);
    $("#entryEditFrmPostdated").find("#CurrencyId").val(data.CurrencyId);
    postdatedExchangeRate = data.ExchangeRateFormated;
    $("#entryEditFrmPostdated").find("#DocumentNumber").val(data.DocumentNumber);
    $("#entryEditFrmPostdated").find("#IssueAmount").val(data.IssueAmountFormated);
    $("#entryEditFrmPostdated").find("#LocalAmount").val(data.LocalAmountFormated);
});

// Evento para Agregar en la tabla de Posfechados
$('#btnEntrySaveAddFrmPostdated').click(function () {
    $("#entryAddFrmPostdated").validate();
    if ($("#entryAddFrmPostdated").valid()) {
        var postDatedRowModel = new PostDatedRowModel();

        postDatedRowModel.PostDateTypeId = $("#entryAddFrmPostdated").find("#PostDateTypeId").val();
        postDatedRowModel.PostDateTypeDescription = $("#entryAddFrmPostdated").find("#PostDateTypeId option:selected").text();
        postDatedRowModel.DocumentNumber = $("#entryAddFrmPostdated").find("#DocumentNumber").val();
        postDatedRowModel.CurrencyId = $("#entryAddFrmPostdated").find("#CurrencyId").val();
        postDatedRowModel.CurrencyDescription = $("#entryAddFrmPostdated").find("#CurrencyId option:selected").text();
        postDatedRowModel.ExchangeRate = postdatedExchangeRate.toString().replace(",", ".");
        postDatedRowModel.ExchangeRateFormated = postdatedExchangeRate;
        postDatedRowModel.IssueAmount = $("#entryAddFrmPostdated").find("#IssueAmount").val().replace(",", ".");
        postDatedRowModel.IssueAmountFormated = $("#entryAddFrmPostdated").find("#IssueAmount").val();
        postDatedRowModel.LocalAmount = $("#entryAddFrmPostdated").find("#LocalAmount").val().replace(",", ".");
        postDatedRowModel.LocalAmountFormated = $("#entryAddFrmPostdated").find("#LocalAmount").val();

        if (!IsDuplicatedPostDated(postDatedRowModel.PostDateTypeId, postDatedRowModel.DocumentNumber)) {
            rowSelected.Postdated.push(postDatedRowModel);

            $('#tblPostdated').UifDataTable('addRow', postDatedRowModel);
            $("#entryAddFrmPostdated").formReset();
            $('#entryModalAddFrmPostdated').UifModal('hide');
            $('#entryModalAddFrmPostdated').find("#entryPostdatedAlert").UifAlert('hide');
        } else {
            $('#entryModalAddFrmPostdated').find("#entryPostdatedAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
        }
    }
});

// Evento para Editar en la tabla de Posfechados
$('#btnEntrySaveEditFrmPostdated').click(function () {
    $("#entryEditFrmPostdated").validate();
    if ($("#entryEditFrmPostdated").valid()) {
        var postDatedRowModel = new PostDatedRowModel();

        postDatedRowModel.PostDateTypeId = $("#entryEditFrmPostdated").find("#PostDateTypeId").val();
        postDatedRowModel.PostDateTypeDescription = $("#entryEditFrmPostdated").find("#PostDateTypeId option:selected").text();
        postDatedRowModel.DocumentNumber = $("#entryEditFrmPostdated").find("#DocumentNumber").val();
        postDatedRowModel.CurrencyId = $("#entryEditFrmPostdated").find("#CurrencyId").val();
        postDatedRowModel.CurrencyDescription = $("#entryEditFrmPostdated").find("#CurrencyId option:selected").text();
        postDatedRowModel.ExchangeRate = postdatedExchangeRate.toString().replace(",", ".");
        postDatedRowModel.ExchangeRateFormated = postdatedExchangeRate;
        postDatedRowModel.IssueAmount = $("#entryEditFrmPostdated").find("#IssueAmount").val().replace(",", ".");
        postDatedRowModel.IssueAmountFormated = $("#entryEditFrmPostdated").find("#IssueAmount").val();
        postDatedRowModel.LocalAmount = $("#entryEditFrmPostdated").find("#LocalAmount").val().replace(",", ".");
        postDatedRowModel.LocalAmountFormated = $("#entryEditFrmPostdated").find("#LocalAmount").val();

        $('#tblPostdated').UifDataTable('editRow', postDatedRowModel, postdatedCurrentEditIndex);
        rowSelected.Postdated[postdatedCurrentEditIndex] = postDatedRowModel; //actualizo el registro en la variable rowSelected
        $("#entryEditFrmPostdated").formReset();
        $('#entryModalEditFrmPostdated').UifModal('hide');
    }
});

// Evento para cerrar modal de postfechados
$('#btnEntryCancelAddFrmPostdated').click(function () {
    $("#entryAddFrmPostdated").formReset();
});

// Evento para cerrar modal de postfechados en edicion
$('#btnEntryCancelEditFrmPostdated').click(function () {
    $("#entryEditFrmPostdated").formReset();
});

//Evento para grabar el asiento
$('#btnSaveAllEntry').click(function () {
    $("#mainForm").validate();
    if ($("#mainForm").valid()) {
        if (ValidateCreditsAndDebits()) {
            if (ValidateAllCostCenters()) {

                lockScreen();

                dataTable = $('#entryTable').UifDataTable('getData');
                $.ajax({
                    type: "POST",
                    url: GL_ROOT + "Entry/SaveEntryRequest",
                    data: JSON.stringify({
                        accountingEntryModels: $('#entryTable').UifDataTable('getData')
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success == false) {
                        $("#entryMainAlert").UifAlert('show', data.result, "danger");
                    } else {
                        
                        $('#entryTable').dataTable().fnClearTable();
                        $('#tblCostCenter').dataTable().fnClearTable();
                        $('#tblAnalysis').dataTable().fnClearTable();
                        $('#tblPostdated').dataTable().fnClearTable();
                        ClearFieldsEntry();
                        UnlockFields();
                        $("#entryMainAlert").UifAlert('show', Resources.EntrySuccessfullySaved + " " + data.result, "success");
                        $("#printEntryConfirmationText").html("<p class= 'text-info'>" + Resources.EntrySuccessfullySaved + " " + data.result + "</p>" + "<br/>" + Resources.PrintReport + ' ?');
                        $('#printEntryConfirmationModal').UifModal('showLocal', " ");
                        printDataEntryNumber = data.result;
                    }

                    unlockScreen();
                });
            } else {
                $("#entryMainAlert").UifAlert('show', Resources.Not100percentCostCenter, "warning");
            }

        } else {
            $("#entryMainAlert").UifAlert('show', Resources.UnbalancedEntry, "warning");
        }
    }
});

//Boton de eleccion para imprimir Reporte
$('#btnChoose').click(function () {

    if (dataTable != null) {
        showReportEntry(dataTable, printDataEntryNumber);
        setTimeout(function () {
            copyModal.modal('hide');
            printDataEntryNumber = 0;
            dataTable = null;
        }, 2000);
    }
});

$("#printEntryConfirmationAccept").on("click", function(){    
    if (dataTable != null) {
        showReportEntry(dataTable, printDataEntryNumber);
        setTimeout(function () {
            $("#printEntryConfirmationModal").UifModal('hide');
            printDataEntryNumber = 0;
            dataTable = null;
        }, 2000);
    }
});

//Evento para solo ingresar números en el campo de Importe en modal de inserción
$("#entryModalAdd").on('keypress', '#Amount', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

//Evento para solo ingresar números en el campo de Importe en modal de edicion
$("#entryModalEdit").on('keypress', '#Amount', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

///Evento para solo ingresar números en el campo de porcentaje de centro de costos en modal de inserción
$("#entryAddFrmCostCenter").on('keypress', '#PercentageAmount', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

//Evento para solo ingresar números en el campo de porcentaje de centro de costos en modal de edicion
$("#entryEditFrmCostCenter").on('keypress', '#PercentageAmount', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

//Evento para solo ingresar números en el campo de Importe de postfechados en modal de inserción
$("#entryAddFrmPostdated").on('keypress', '#IssueAmount', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

//Evento para solo ingresar números en el campo de Número de Documento en modal de edicion
$("#entryEditFrmPostdated").on('keypress', '#DocumentNumber', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

//Evento para solo ingresar números en el campo de Número de Documento de postfechados en modal de inserción
$("#entryAddFrmPostdated").on('keypress', '#DocumentNumber', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

//Evento para solo ingresar números en el campo de Importe en modal de edicion
$("#entryEditFrmPostdated").on('keypress', '#IssueAmount', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

//Evento para búsqueda de datos de pagador en modal de inserción
$('#entryModalAdd').on('itemSelected', '#entryPayerDocumentNumber', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.IndividualId > 0) {
            documentNumberEntry = selectedItem.DocumentNumber;
            $("#btnEntrySaveAdd").attr("disabled", false);
            $("#entryModalAdd").find("#entryPayerName").val(selectedItem.Name);
            $("#entryModalAdd").find("#IndividualId").val(selectedItem.IndividualId);
            $("#entryModalAdd").find("#entryPersonAlert").UifAlert('hide');
        } else {
            $('#entryModalAdd').find("#entryPersonAlert").UifAlert('show', Resources.PersonNotFound, "warning");
            $("#entryModalAdd").find("#IndividualId").val("");
            $("#entryModalAdd").find("#entryPayerName").val("");
        }
    }
});

$("#entryModalAdd").on('blur', '#entryPayerDocumentNumber', function (event) {
    validPayerDocumentNumberEntryAdd();
});

//Evento para búsqueda de datos de pagador en modal de edición
$('#entryModalEdit').on('itemSelected', '#entryPayerDocumentNumber', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.IndividualId > 0) {
            documentNumberEntry = selectedItem.DocumentNumber;
            $("#btnEntrySaveEdit").attr("disabled", false);
            $("#entryModalEdit").find("#entryPayerName").val(selectedItem.Name);
            $("#entryModalEdit").find("#IndividualId").val(selectedItem.IndividualId);
            $("#entryModalEdit").find("#entryPersonAlert").UifAlert('hide');
        } else {
            $('#entryModalEdit').find("#entryPersonAlert").UifAlert('show', Resources.PersonNotFound, "warning");
            $("#entryModalEdit").find("#IndividualId").val("");
            $("#entryModalEdit").find("#entryPayerName").val("");
        }
    }
});

$("#entryModalEdit").on('blur', '#entryPayerDocumentNumber', function (event) {
    validPayerDocumentNumberEntryEdit();
});

//Evento para búsqueda conceptos por Id en modal de inserción
$('#entryModalAdd').on('itemSelected', '#conceptId ', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.Id > 0) {

            var accountingAccountId = selectedItem.AccountingAccountId;
            if (accountingAccountId > 0) {
                $('#entryModalAdd').find("#AccountingAccountId").val(accountingAccountId);
                $('#entryModalAdd').find("#conceptDescription").val(selectedItem.Description);
                $('#entryModalAdd').find("#accountingAccountNumber").val(selectedItem.AccountingNumber);
                $('#entryModalAdd').find("#accountingAccountDescription").val(selectedItem.AccountingNumber + " - " + selectedItem.AccountingName);
                $("#entryModalAdd").find("#entryModalAlert").UifAlert('hide');
            } else {
                $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.AccountingConceptNotAssociatedAccount, "info");
                $('#entryModalAdd').find("#AccountingAccountId").val(accountingAccountId);
                $('#entryModalAdd').find("#accountingAccountNumber").val('');
                $('#entryModalAdd').find("#accountingAccountDescription").val('');
                $('#entryModalAdd').find("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
            }
        } else {
            $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            $('#entryModalAdd').find("#AccountingAccountId").val('');
            $('#entryModalAdd').find("#conceptDescription").val('');
            $('#entryModalAdd').find("#accountingAccountNumber").val('');
            $('#entryModalAdd').find("#conceptDescription").val('');
            $('#entryModalAdd').find("#accountingAccountDescription").val('');
        }
    } else {
        $('#entryModalAdd').find("#conceptId").val('');
        $('#entryModalAdd').find("#conceptDescription").val('');
    }
});

//Evento para búsqueda conceptos por Id en modal de edición
$('#entryModalEdit').on('itemSelected', '#conceptId ', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.Id > 0) {
            var accountingAccountId = selectedItem.AccountingAccountId;
            if (accountingAccountId > 0) {
                $('#entryModalEdit').find("#AccountingAccountId").val(accountingAccountId);
                $('#entryModalEdit').find("#conceptDescription").val(selectedItem.Description);
                $('#entryModalEdit').find("#accountingAccountNumber").val(selectedItem.AccountingNumber);
                $('#entryModalEdit').find("#accountingAccountDescription").val(selectedItem.AccountingNumber + " - " + selectedItem.AccountingName);
                $("#entryModalEdit").find("#entryModalAlert").UifAlert('hide');
            } else {
                $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.AccountingConceptNotAssociatedAccount, "info");
                $('#entryModalEdit').find("#AccountingAccountId").val(accountingAccountId);
                $('#entryModalEdit').find("#accountingAccountNumber").val('');
                $('#entryModalEdit').find("#accountingAccountDescription").val('');
                $('#entryModalEdit').find("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
            }
        } else {
            $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            $('#entryModalEdit').find("#AccountingAccountId").val('');
            $('#entryModalEdit').find("#conceptDescription").val('');
            $('#entryModalEdit').find("#accountingAccountNumber").val('');
            $('#entryModalEdit').find("#conceptDescription").val('');
            $('#entryModalEdit').find("#accountingAccountDescription").val('');
        }
    }
});

//Evento para búsqueda conceptos por Descripción en modal de inserción
$('#entryModalAdd').on('itemSelected', '#conceptDescription', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.Id > 0) {
            var accountingAccountId = selectedItem.AccountingAccountId;
            $("#entryModalAdd").find("#AccountingAccountId").val(accountingAccountId);
            $("#entryModalAdd").find("#conceptId").val(selectedItem.Id);
            $("#entryModalAdd").find("#accountingAccountNumber").val(selectedItem.AccountingNumber);
            $("#entryModalAdd").find("#accountingAccountDescription").val(selectedItem.AccountingNumber + " - " + selectedItem.AccountingName);
            $("#entryModalAdd").find("#entryModalAlert").UifAlert('hide');
        } else {
            $("#entryModalAdd").find('#conceptId').val('');
            $("#entryModalAdd").find('#conceptDescription').val('');
        }
    } else {
        $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
        $("#conceptId").val('');
        $("#accountingAccountNumber").val('');
        $("#accountingAccountDescription").val('');
    }
});

//Evento para búsqueda conceptos por Descripción en modal de edición
$('#entryModalEdit').on('itemSelected', '#conceptDescription', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.Id > 0) {
            var accountingAccountId = selectedItem.AccountingAccountId;
            $('#entryModalEdit').find("#AccountingAccountId").val(accountingAccountId);
            $('#entryModalEdit').find("#conceptId").val(selectedItem.Id);
            $('#entryModalEdit').find("#accountingAccountNumber").val(selectedItem.AccountingNumber);
            $('#entryModalEdit').find("#accountingAccountDescription").val(selectedItem.AccountingNumber + " - " + selectedItem.AccountingName);
            $('#entryModalEdit').find("#entryModalAlert").UifAlert('hide');
        } else {
            $('#entryModalEdit').find("#conceptId").val('');
            $('#entryModalEdit').find("#conceptDescription").val('');
        }
    } else {
        $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
        $("#entryModalEdit").find("#conceptId").val('');
        $("#entryModalEdit").find("#accountingAccountNumber").val('');
        $("#entryModalEdit").find("#accountingAccountDescription").val('');
    }
});

//Evento para búsqueda de cuenta contable en modal de inserción
$('#entryModalAdd').on('itemSelected', '#accountingAccountNumber', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.AccountingAccountId > 0) {
            var accountingAccountId = selectedItem.AccountingAccountId;
            $("#AccountingAccountId").val(accountingAccountId);
            $("#conceptId").val(selectedItem.PaymentConceptCode);
            $("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
            $("#accountingAccountDescription").val(selectedItem.AccountNumber + " - " + selectedItem.AccountName);
            $("#entryModalAdd").find("#entryModalAlert").UifAlert('hide');
        } else {
            $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            $("#conceptId").val('');
            $("#conceptDescription").val('');
            $("#accountingAccountDescription").val('');
        }
    }
});

//Evento para búsqueda de cuenta contable en modal de edición
$('#entryModalEdit').on('itemSelected', '#accountingAccountNumber', function (event, selectedItem) {
    if (selectedItem != null) {
        if (selectedItem.AccountingAccountId > 0) {
            var accountingAccountId = selectedItem.AccountingAccountId;
            $("#AccountingAccountId").val(accountingAccountId);
            $("#conceptId").val(selectedItem.PaymentConceptCode);
            $("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
            $("#accountingAccountDescription").val(selectedItem.AccountNumber + " - " + selectedItem.AccountName);
            $("#entryModalEdit").find("#entryModalAlert").UifAlert('hide');
        } else {
            $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            $("#conceptId").val('');
            $("#conceptDescription").val('');
            $("#accountingAccountDescription").val('');
        }
    }
});

//Metodo de carga de Puntos de venta a partir de la sucursal
$('#ddlBranches').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = GL_ROOT + "Base/GetSalePointsbyBranch?branchId=" + selectedItem.Id;
        $("#ddlSalePoints").UifSelect({
            source: controller
        });
    } else
        $("#ddlSalePoints").UifSelect();
});

//Metodo de carga de tasa de cambio a partir de la moneda
$("#ddlCurrencies").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id == 0) {
        $("#exchange").val('1');
        $("#exchangeAlert").UifAlert('hide');
        return;
    }
    if (selectedItem.Id > -1) {
        $.ajax({
            url: GL_ROOT + "Base/GetExchangeByCurrency",
            data: {
                "currencyId": selectedItem.Id
            },
            success: function (data) {
                if (data.length > 0) {
                    $("#exchangeAlert").UifAlert('hide');
                    $("#exchange").val(parseFloat(data));
                } else {
                    $("#exchangeAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                    $("#exchange").val('');
                }
            },
            error: function (result) {
                $("#exchange").val('');
                $("#exchangeAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
            }
        });
    } else {
        $("#exchange").val();
    }
});

//Metodo de carga de tasa de cambio a partir de la moneda en postfechados en insercion
$("#entryAddFrmPostdated").on('itemSelected', '#CurrencyId', function (event, selectedItem) {
    if (selectedItem.Id == 0) {
        postdatedExchangeRate = 1;

        if ($("#entryAddFrmPostdated").find("#IssueAmount").val() != "" || $("#entryAddFrmPostdated").find("#IssueAmount").val() != undefined) {
            var total = 0;
            total = ($("#entryAddFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
            $("#entryAddFrmPostdated").find("#LocalAmount").val(total);
        }
        $("#entryModalAddFrmPostdated").find("#entryPostdatedAlert").UifAlert('hide');
        return;
    }
    if (selectedItem.Id > -1) {
        $.ajax({
            url: GL_ROOT + "Base/GetExchangeByCurrency",
            data: {
                "currencyId": selectedItem.Id
            },
            success: function (data) {
                if (data.length > 0) {
                    $("#entryModalAddFrmPostdated").find("#entryPostdatedAlert").UifAlert('hide');
                    postdatedExchangeRate = parseFloat(data);
                    if ($("#entryAddFrmPostdated").find("#IssueAmount").val() != "" || $("#entryAddFrmPostdated").find("#IssueAmount").val() != undefined) {
                        var total = 0;
                        total = ($("#entryAddFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
                        $("#entryAddFrmPostdated").find("#LocalAmount").val(total);
                    }
                } else {
                    $("#entryModalAddFrmPostdated").find("#entryPostdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                    postdatedExchangeRate = 0;
                }
            },
            error: function (result) {
                postdatedExchangeRate = 0;
                $("#entryModalAddFrmPostdated").find("#entryPostdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
            }
        });
    } else {
        postdatedExchangeRate = 0;
    }
});

//Calculo del importe local cuando se modifica el importe de emision en modal de inserción
$("#entryAddFrmPostdated").on('blur', '#IssueAmount', function (event) {
    if ($("#entryAddFrmPostdated").find("#IssueAmount").val() != "" || $("#entryAddFrmPostdated").find("#IssueAmount").val() != undefined) {
        var total = 0;
        total = ($("#entryAddFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
        $("#entryAddFrmPostdated").find("#LocalAmount").val(total);
    }
});

//Metodo de carga de tasa de cambio a partir de la moneda en postfechados en edicion
$("#entryEditFrmPostdated").on('itemSelected', '#CurrencyId', function (event, selectedItem) {
    if (selectedItem.Id == 0) {
        postdatedExchangeRate = 1;

        if ($("#entryEditFrmPostdated").find("#IssueAmount").val() != "" || $("#entryEditFrmPostdated").find("#IssueAmount").val() != undefined) {
            var total = 0;
            total = ($("#entryEditFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
            $("#entryEditFrmPostdated").find("#LocalAmount").val(total);
        }

        $("#entryModalEditFrmPostdated").find("#entryPostdatedAlert").UifAlert('hide');
        return;
    }
    if (selectedItem.Id > -1) {
        $.ajax({
            url: GL_ROOT + "Base/GetExchangeByCurrency",
            data: {
                "currencyId": selectedItem.Id
            },
            success: function (data) {
                if (data.length > 0) {
                    $("#entryModalEditFrmPostdated").find("#entryPostdatedAlert").UifAlert('hide');
                    postdatedExchangeRate = parseFloat(data);

                    if ($("#entryEditFrmPostdated").find("#IssueAmount").val() != "" || $("#entryEditFrmPostdated").find("#IssueAmount").val() != undefined) {
                        var total = 0;
                        total = ($("#entryEditFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
                        $("#entryEditFrmPostdated").find("#LocalAmount").val(total);
                    }
                } else {
                    $("#entryModalEditFrmPostdated").find("#entryPostdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                    postdatedExchangeRate = 0;
                }
            },
            error: function (result) {
                postdatedExchangeRate = 0;
                $("#entryModalEditFrmPostdated").find("#entryPostdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
            }
        });
    } else {
        postdatedExchangeRate = 0;
    }
});

//Calculo del importe local cuando se modifica el importe de emision en modal de inserción
$("#entryEditFrmPostdated").on('blur', '#IssueAmount', function (event) {
    if ($("#entryEditFrmPostdated").find("#IssueAmount").val() != "" || $("#entryEditFrmPostdated").find("#IssueAmount").val() != undefined) {
        var total = 0;
        total = ($("#entryEditFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
        $("#entryEditFrmPostdated").find("#LocalAmount").val(total);
    }
});

//Carga los datos del autocomplete en los campos de cuenta contable en modal de Insercion
$('#entryAddForm').on('itemSelected', '#accountingAccountNumber', function (event, selectedItem) {
    var data = JSON.stringify(selectedItem);
    $('#entryModalAdd').find('#accountingAccountDescription').val(selectedItem.Number + " - " + selectedItem.Description);
    $('#entryModalAdd').find("#AccountingAccountId").val(selectedItem.AccountingAccountId);
    $('#entryModalAdd').find("#conceptId").val("");
    $('#entryModalAdd').find("#conceptDescription").val("");
});

//Carga los datos del autocomplete en los campos de cuenta contable en modal de Insercion
$('#entryEditForm').on('itemSelected', '#accountingAccountNumber', function (event, selectedItem) {
    var data = JSON.stringify(selectedItem);
    $('#entryEditForm').find('#accountingAccountDescription').val(selectedItem.Number + " - " + selectedItem.Description);
    $('#entryEditForm').find("#AccountingAccountId").val(selectedItem.AccountingAccountId);
    $('#entryEditForm').find("#conceptId").val("");
    $('#entryEditForm').find("#conceptDescription").val("");
});

//Carga los datos del autocomplete en los campos de centro de costos en modal de Insercion
$('#entryAddFrmCostCenter').on('itemSelected', '#CostCenterId', function (event, selectedItem) {
    var data = JSON.stringify(selectedItem);
    $('#entryAddFrmCostCenter').find('#Description').val(selectedItem.Description);
});

//Carga los datos del autocomplete en los campos de centro de costos en modal de Edición
$('#entryEditFrmCostCenter').on('itemSelected', '#CostCenterId', function (event, selectedItem) {
    var data = JSON.stringify(selectedItem);
    $('#entryEditFrmCostCenter').find('#Description').val(selectedItem.Description);
});

//Carga los datos del autocomplete en los campos de centro de costos en modal de inserción
$('#entryAddFrmCostCenter').on('itemSelected', '#Description', function (event, selectedItem) {
    var data = JSON.stringify(selectedItem);
    $('#entryAddFrmCostCenter').find('#CostCenterId').val(selectedItem.CostCenterId);
});

//Carga los datos del autocomplete en los campos de centro de costos en modal del edición
$('#entryEditFrmCostCenter').on('itemSelected', '#Description', function (event, selectedItem) {
    var data = JSON.stringify(selectedItem);
    $('#entryEditFrmCostCenter').find('#CostCenterId').val(selectedItem.CostCenterId);
});

//Carga los conceptos de pago a partir del Análisis en el modal de análisis en inserción
$('#entryAddFrmAnalysis').on('itemSelected', '#AnalysisId', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = GL_ROOT + "Base/GetPaymentConceptsByAnalysisCode?analysisCodeId=" + selectedItem.Id;
        $("#entryModalAddFrmAnalysis").find("#AnalysConceptId").UifSelect({
            source: controller
        });
    } else
        $("#entryModalAddFrmAnalysis").find("#AnalysConceptId").UifSelect();
});

//Carga los conceptos de pago a partir del Análisis en el modal de análisis en edición
$('#entryEditFrmAnalysis').on('itemSelected', '#AnalysisId', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = GL_ROOT + "Base/GetPaymentConceptsByAnalysisCode?analysisCodeId=" + selectedItem.Id;
        $('#entryModalEditFrmAnalysis').find("#AnalysConceptId").UifSelect({
            source: controller
        });
    } else
        $('#entryModalEditFrmAnalysis').find("#AnalysConceptId").UifSelect();
});

//Borra el registro seleccionado, botón perteneciente al modal de eliminación
$("#entryDeleteModalWarningAccept").click(function () {
    if ($("#ViewBagOptionId").val() == "Entry") {
        $("#entryDeleteModalWarning").UifModal('hide');

        Delete(tableTypeId, tablePosition);
        $("#entryMainAlert").UifAlert('show', Resources.RecordSuccessfullyDeleted, "success");

        //validacion solo para centros de costos
        $("#btnAcceptMessageModal").click(function () {
            var costCenterData = $("#tblCostCenter").UifDataTable('getData');
            if (costCenterData.length > 0) {
                if (tableTypeId == 1) {
                    if (CostCenterPercentageTotal() != 100) {
                        $("#entryMainAlert").UifAlert('show', Resources.SumPercentagesCostCentersDifferent, "info");
                    }
                }
            }
        });
    }
});

setTimeout(function () {
    $("#ddlAccountingCompanies").attr("disabled", "disabled");
}, 1000);

$("#EntryTypeModalWarning").find("#EntryTypeModalWarningAccept").on("click", function () {
    if (isEntryType == 1){
        if (entryTypeId != 0) {
            accounts = [];
            $("#EntryTypeModalWarning").UifModal("hide");
            $('#entryTable').dataTable().fnClearTable();
            AddEntryType(entryTypeId);
        }
    }else{
        entryTypeId = 0;
        accounts = [];
        $('#entryTable').dataTable().fnClearTable();
        $("#EntryTypeModalWarning").UifModal("hide");
        $('#btnAddMovement').trigger('click');
    }
    
});
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//FUNCIONES
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

//Valida todos los centros de costos
function ValidateAllCostCenters() {
    var result = false;
    var data = $("#entryTable").UifDataTable('getData');
    var totalCostCenters = 0;

    for (var i = 0; i < data.length; i++) {
        if (data[i].CostCenters.length > 0) {
            for (var j = 0; j < data[i].CostCenters.length; j++) {
                totalCostCenters = totalCostCenters + parseFloat(data[i].CostCenters[j].PercentageAmountFormated);
            }
            if (totalCostCenters < 100 || totalCostCenters > 100) {
                totalCostCenters = 0;
                result = false;
                break;
            } else {
                result = true;
            }
        } else {
            result = true;
        }
        totalCostCenters = 0;
    }
    return result;
}

//Valida porcentaje de centros de costos
function CostCenterPercentageTotal() {
    var totalPercentage = 0;
    var costCenterData = $("#tblCostCenter").UifDataTable('getData');

    if (costCenterData.length > 0) {
        for (var i = 0; i < costCenterData.length; i++) {
            totalPercentage = totalPercentage + parseFloat(costCenterData[i].PercentageAmount.replace(",", "."));
        }
    }
    return totalPercentage;
}

//Valida centro de costos previamente ingresados
function IsDuplicatedCostCenters(costCenterId) {
    var result = false;
    var count = 0;
    var costCenterData = $("#tblCostCenter").UifDataTable('getData');

    if (costCenterData.length > 0) {
        for (var i = 0; i < costCenterData.length; i++) {
            if (costCenterData[i].CostCenterId == costCenterId)
                count = count + 1;
        }
    }

    if (count > 0)
        result = true;
    return result;
}

//Valida analisis previamente ingresados
function IsDuplicatedAnalysis(analysisCodeId, analysisConceptId, key) {
    var result = false;
    var count = 0;
    var analysisData = $("#tblAnalysis").UifDataTable('getData');

    if (analysisData.length > 0) {
        for (var i = 0; i < analysisData.length; i++) {
            if (analysisData[i].AnalysisId == analysisCodeId && analysisData[i].AnalysConceptId == analysisConceptId && analysisData[i].Key == key)
                count = count + 1;
        }
    }

    if (count > 0)
        result = true;

    return result;
}

//Valida postfechados previamente ingresados
function IsDuplicatedPostDated(postDatedTypeId, documentNumber) {
    var result = false;
    var count = 0;
    var postdatedData = $("#tblPostdated").UifDataTable('getData');

    if (postdatedData.length > 0) {
        for (var i = 0; i < postdatedData.length; i++) {
            if (postdatedData[i].PostDateTypeId == postDatedTypeId && postdatedData[i].DocumentNumber == documentNumber)
                count = count + 1;
        }
    }

    if (count > 0)
        result = true;
    return result;
}

//limpia los campos del formulario principal
function ClearFieldsEntry() {
    //$("#ddlEntryTypes").removeAttr("disabled");
    $("#ddlBranches").val("");
    $("#ddlSalePoints").val("");
    $("#ddlAccountingCompanies").val(Resources.DefaultCompanyValue);
    $("#ddlDestinations").val("");
    $("#ddlAccountingMovementTypes").val("");
    getAccountingDate(Resources.AccountingModuleDateId);
    $("#ddlCurrencies").val("");
    $("#exchange").val("");
    isEntryType = 0;
    entryTypeId = 0;
    accounts = [];
}

//limpia los campos en el modal
function ClearModalFields() {
    $("#entryAddForm").find("#AccountingAccountId").val("");
    $("#entryAddForm").find("#accountingAccountNumber").val("");
    $("#entryAddForm").find("#accountingAccountDescription").val("");
    $("#entryAddForm").find("#AccountingNatureId").val("");
    $("#entryAddForm").find("#Amount").val("");
    $("#entryAddForm").find("#IndividualId").val("");
    $("#entryAddForm").find("#entryPayerDocumentNumber").val("");
    $("#entryAddForm").find("#entryPayerName").val("");
    $("#entryAddForm").find("#conceptId").val("");
    $("#entryAddForm").find("#conceptDescription").val("");
    $("#entryAddForm").find("#BankReconciliationId").val("");
    $("#entryAddForm").find("#ReceiptNumber").val("");
    $("#entryAddForm").find("#receiptDate").val("");
    $("#entryAddForm").find("#Description").val("");
}

//funcion para eliminación de registros en tablas de centros de costos/analisis/postfechados
// 1 - Centro de Costos
// 2 - Analisis
// 3 - Postfechados
// 4 - Movimientos
function Delete(tableId, position) {
    //borro centros de costos
    if (tableId == 1) {

        $('#tblCostCenter').UifDataTable('deleteRow', position);
        rowSelected.CostCenters.splice(position, 1);
    }

    //borro analisis
    if (tableId == 2) {
        $('#tblAnalysis').UifDataTable('deleteRow', position);
        rowSelected.Analyses.splice(position, 1);
    }

    //borro postfechados
    if (tableId == 3) {
        $('#tblPostdated').UifDataTable('deleteRow', position);
        rowSelected.Postdated.splice(position, 1);
    }

    //borro movimientos
    if (tableId == 4) {
        $('#entryTable').UifDataTable('deleteRow', position);

        //limpio tabla de centro de costos
        $('#tblCostCenter').dataTable().fnClearTable();

        //limpio tabla de analásis
        $('#tblAnalysis').dataTable().fnClearTable();

        //limpio tabla de postfechados
        $('#tblPostdated').dataTable().fnClearTable();

        //"de-selecciono" la fila
        rowSelected = null;

        if ($('#entryTable').UifDataTable('getData').length == 0)
            UnlockFields();
    }
}

//Valida y setea combo de Compañía
function validateCompanyEntry() {
    if ($("#ddlAccountingCompanies").val() != "" && $("#ddlAccountingCompanies").val() != null) {

        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#ddlAccountingCompanies").attr("disabled", true);
        } else {
            $("#ddlAccountingCompanies").attr("disabled", false);
        }
        clearInterval(timeEntry);
    }
}

//Llena el combo de asientos tipo.
function LoadEntryTypes() {
    var loadEntryTypes = new Promise(function (resolve, reject) {
        $.ajax({
            type: 'Post',
            url: GL_ROOT + "Entry/GetEntryTypeModels",
            success: function (data) {
                resolve(data);
            }
        });
    });
    loadEntryTypes.then(function (data) {
        if (data.success) {
            var items = $("#entryTypes").empty();
            for (x in data.result) {
                items.append('<li value="' + data.result[x].EntryTypeId + '"><a data-toggle="dropdown" href="#">' + data.result[x].EntryTypeDescription + '</a></li>');
            }
        }
    });
}

//funcion para cargar la fecha actual desde el servidor
function getDate() {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId").val() == undefined) {

        $.ajax({
            url: GL_ROOT + "Base/GetDate",
            success: function (data) {
                $("#Date").val(data);
            }
        });
    }
}

//funcion que obtiene la fecha contable
function getAccountingDate(moduleDateId) {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId") == undefined) {

        $.ajax({
            url: GL_ROOT + "Base/GetAccountingDate",
            data: {
                "moduleDateId": moduleDateId
            },
            success: function (data) {
                $("#Date").val(data);
            }
        });
    }
}

//funcion que muestra reporte en pantalla
function showReportEntry(dataTable, entryNumber) {
    
    window.open(GL_ROOT + "Reports/SearchNewEntry?branchId=" + dataTable[0].BranchId +
        '&date=' + dataTable[0].Date + '&entryNumber=' + entryNumber + '&destinationId=' +
        dataTable[0].DestinationId + '&accMovTypeId=' + dataTable[0].AccountingMovementTypeId,
        'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//Setea el punto de venta de default
function setSalePointEntry() {
    if ($('#ddlBranches').val() > 0) {
        var controller = GL_ROOT + "Base/GetSalePointsbyBranch?branchId=" + $('#ddlBranches').val();
        $("#ddlSalePoints").UifSelect({ source: controller });

        //Setea el punto de venta de default
        setTimeout(function () {
            $("#ddlSalePoints").val($("#ViewBagSalePointBranchUserDefault").val());
        }, 500);
    }
};

//Bloqueo de campos en formulario principal
function LockFields() {
    $("#ddlBranches").attr("disabled", true);
    $("#ddlSalePoints").attr("disabled", true);
    $("#ddlAccountingCompanies").attr("disabled", "disabled");
    $("#ddlDestinations").attr("disabled", true);
    $("#ddlAccountingMovementTypes").attr("disabled", true);
    $("#ddlCurrencies").attr("disabled", true);
}

//Debloqueo de campos en formulario principal
function UnlockFields() {
    $("#ddlEntryTypes").removeAttr("disabled");
    $("#ddlBranches").removeAttr("disabled");
    $("#ddlSalePoints").removeAttr("disabled");
    $("#ddlDestinations").removeAttr("disabled");
    $("#ddlAccountingMovementTypes").removeAttr("disabled");
    $("#ddlCurrencies").removeAttr("disabled");
}

//Validacion de Débitos y créditos
function ValidateCreditsAndDebits() {
    var result = false;
    var debits = 0;
    var credits = 0;

    var data = $("#entryTable").UifDataTable('getData');

    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
			if (data[i].AccountingNatureId == 2) //debito  mod:Danc 2018-05-21  aumenta tostring 
				//	debits = debits + parseFloat(((parseFloat(data[i].ExchangeRate.replace(",", ".")) * parseFloat(data[i].Amount.replace(",", ".")))).toFixed(2));
				debits = debits + parseFloat(((parseFloat(data[i].ExchangeRate.toString().replace(",", ".")) * parseFloat(data[i].Amount.toString().replace(",", ".")))).toFixed(2));
            if (data[i].AccountingNatureId == 1) //credito
				credits = credits + parseFloat(((parseFloat(data[i].ExchangeRate.toString().replace(",", ".")) * parseFloat(data[i].Amount.toString().replace(",", ".")))).toFixed(2));
        }
        if (credits == debits)
            result = true;
    }
    return result;
}

function AddEntryType(entryTypeId) {
    GetEntryTypeAccountingsByEntryType(entryTypeId).then(function (entryTypeAccountings) {
        if (entryTypeAccountings.success) {
            if (entryTypeAccountings.result.length > 0) {
                for (x in entryTypeAccountings.result) {

                    var rowModel = new RowModel();

                    rowModel.BranchId = $("#mainForm").find("#ddlBranches").val();
                    rowModel.SalePointId = $("#mainForm").find("#ddlSalePoints").val();
                    rowModel.CompanyId = $("#mainForm").find("#ddlAccountingCompanies").val();
                    rowModel.DestinationId = $("#mainForm").find("#ddlDestinations").val();
                    rowModel.ExchangeRate = entryTypeAccountings.result[x].ExchangeRate;
                    rowModel.ExchangeRateFormated = 0;
                    rowModel.Amount = 0;
                    rowModel.AmountFormated = 0;
                    rowModel.IndividualId = 0;
                    rowModel.DocumentNumber = 0;
                    rowModel.Name = "";
                    rowModel.BankReconciliationId = 0;
                    rowModel.BankReconciliationDescription = "";
                    rowModel.ReceiptNumber = "";
                    rowModel.ReceiptDate = "";
                    rowModel.AccountingMovementTypeId = entryTypeAccountings.result[x].AccountingMovementTypeId;
                    rowModel.Date = $("#mainForm").find("#Date").val();
                    rowModel.AccountingNatureId = entryTypeAccountings.result[x].AccountingNatureId;
                    rowModel.AccountingNatureDescription = entryTypeAccountings.result[x].AccountingNatureDescription;
                    rowModel.Description = entryTypeAccountings.result[x].Description;
                    rowModel.AccountingAccountId = entryTypeAccountings.result[x].AccountingAccountId;
                    rowModel.AccountingAccountNumber = entryTypeAccountings.result[x].AccountingAccountNumber;
                    rowModel.AccountingAccountDescription = entryTypeAccountings.result[x].AccountingAccountDescription;
                    rowModel.CurrencyId = entryTypeAccountings.result[x].CurrencyId;
                    rowModel.CurrencyDescription = entryTypeAccountings.result[x].CurrencyDescription;
                    rowModel.ConceptId = entryTypeAccountings.result[x].PaymentConceptCd;
                    rowModel.ConceptDescription = entryTypeAccountings.result[x].PaymentConceptDescription;

                    var costCenterModel = new CostCenterRowModel();
                    costCenterModel.CostCenterId = entryTypeAccountings.result[x].CostCenterId;
                    costCenterModel.Description = entryTypeAccountings.result[x].CostCenterDescription;
                    costCenterModel.PercentageAmount = "0";
                    costCenterModel.PercentageAmountFormated = "0";

                    rowModel.CostCenters.push(costCenterModel);

                    var analysisModel = new AnalysisRowModel();
                    analysisModel.AnalysisId = entryTypeAccountings.result[x].AnalysisId;
                    analysisModel.AnalysisDescription = entryTypeAccountings.result[x].AnalysisDescription;
                    analysisModel.AnalysConceptId = 0;
                    analysisModel.AnalysConceptDescription = "";
                    analysisModel.Key = "";
                    analysisModel.Description = "";

                    rowModel.Analyses.push(analysisModel);

                    accounts.push(rowModel);
                }

                if (accounts.length > 0) {
                    for (var i = 0; i < accounts.length; i++) {
                        $('#entryTable').UifDataTable('addRow', accounts[i]);
                    }
                    $("#ddlCurrencies").val(accounts[0].CurrencyId);
                    $("#ddlCurrencies").trigger('change');
                }
            }
        }
    });
}

function validPayerDocumentNumberEntryAdd() {
    if (documentNumberEntry != $("#entryModalAdd").find("#entryPayerDocumentNumber").val()) {
        $('#entryModalAdd').find("#entryPersonAlert").UifAlert('show', Resources.NoRegisteredPerson, "warning");
        $("#btnEntrySaveAdd").attr("disabled", true);
    } else {
        $("#entryModalAdd").find("#entryPersonAlert").UifAlert('hide');
        $("#btnEntrySaveAdd").attr("disabled", false);
    }
}

function validPayerDocumentNumberEntryEdit() {
    if (documentNumberEntry != $("#entryModalEdit").find("#entryPayerDocumentNumber").val()) {
        $('#entryModalEdit').find("#entryPersonAlert").UifAlert('show', Resources.NoRegisteredPerson, "warning");
        $("#btnEntrySaveEdit").attr("disabled", true);
    } else {
        $("#entryModalEdit").find("#entryPersonAlert").UifAlert('hide');
        $("#btnEntrySaveEdit").attr("disabled", false);
    }
}

function GetEntryTypeAccountingsByEntryType(entryTypeId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "Post",
            url: GL_ROOT + "Entry/GetEntryTypeAccountingsByEntryType?entryTypeId=" + entryTypeId,
            success: function (entryTypeAccountings) {
                resolve(entryTypeAccountings)
            }
        });
    });
}