/**
    * @file   AccountingClosing.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var time;
var operationId = 0;
var branchId = 0;
var prefixId = 0;
var individualId = 0;
var policyNumber = "";
var operationType = "";
var importMax = 0;
var limitAmount = 0;

var oAmortizationItems = {
    AmortizationItem: []
};

var oAmortizationItem = {
    ProcessId: 0,
    ImputationId: 0,
    AmortizationItemId: 0,
    BranchId: 0,
    BranchName: "",
    PrefixId: 0,
    PrefixName: "",
    PolicyId: 0,
    PolicyNumber: "",
    EndorsementNumber: 0,
    InsuredId: 0,
    InsuredDocumentNumber: "",
    InsuredName: "",
    InsuredPersonTypeId: 0,
    AgentId: 0,
    AgentDocumentNumber: "",
    AgentName: "",
    AgentPersonTypeId: 0,
    PayerId: 0,
    PayerDocumentNumber: "",
    PayerName: "",
    PayerPersonTypeId: 0,
    CurrencyId: 0,
    CurrencyName: "",
    IncomeAmount: 0,
    Exchange: 0,
    Amount: 0,
    ImputationReceiptNumber: 0
};

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/

$(() => {
    new MainAutomaticAmortization();
});

class MainAutomaticAmortization extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        $("#UserNameAmortization").attr("disabled", "disabled");
        $("#ProcessDate").attr("disabled", "disabled");
        $("#TotalDebits").attr("disabled", "disabled");
        $("#TotalCredits").attr("disabled", "disabled");

        $("#ApplyAmortization").hide();

        $("#LowAmortization").hide();
        $("#PrintAmortization").hide();
        $("#DeleteAmortization").hide();

        setTimeout(function () {
            if ($("#ViewBagImputationType").val() == undefined &&
                $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
                $("#ViewBagBillControlId").val() == undefined) {

                $.ajax({
                    url: ACC_ROOT + "Common/GetLimitAmountAmortization",
                    success: function (data) {
                        if (data != null) {
                            $("#AmortizedValue").val(data);
                        }
                    }
                });
            }
        }, 500);

        setTimeout(function () {
            MainAutomaticAmortization.RefreshGridAmortization();
        }, 500);
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#selectBranch").on("binded", this.BindedBranch);
        $("#selectOperation").on("itemSelected", this.SelectOperation);
        $("#AmortizedValue").on("change", this.ChangeAmortizedValue);
        $("#InsuredDocumentNumber").on("itemSelected", this.AutocompleteInsuredDocumentNumber);
        $("#InsuredName").on("itemSelected", this.AutocompleteInsuredName);
        $("#CleanSearchAmortization").on("click", this.CleanSearchAmortization);
        $("#GenerateAmortization").on("click", this.GenerateAmortization);
        $("#ApplyAmortization").on('click', this.ApplyAmortization);
        $("#tablePendingProcessesAmortization").on("rowSelected", this.RowSelectedTableProcess);
        $("#ProcessAmortizationPolicies").on("search", this.SearchAmortizationProcess);
        $("#PrintAmortization").on('click', this.PrintAmortization);
        $("#tabMainAutomaticAmortization").on("change", this.ChangeTabAmortization);
        $("#LowAmortization").on("click", this.LowAmortization);
        $("#DeleteAmortization").on("click", this.DeleteAmortization);
        $("#tableAmortizationPolicies").on("rowSelected", this.RowSelectedTableAmortization);
        $("#modalSave").find("#GenerateModalAmortization").on("click", this.GenerateModalAmortization);
        $("#modalDeleteAmortization").find("#DeleteModalAmortization").on("click", this.DeleteModalAmortization);
        $('#StartDateAmortization').on("datepicker.change", this.ChangeStartDateAmortization);
        $("#EndDateAmortization").on("datepicker.change", this.ChangeEndDateAmortization);
        $("#SelectStatusProcess").on("itemSelected", this.ItemSelectedStatusProcess);
    }

    /**
        * Setea la sucursal por default después de cargar todas las sucursales.
        *
        */
    BindedBranch() {
        if ($("#ViewBagBranchDisable").val() == "1") {
            $("#selectBranch").attr("disabled", "disabled");
        }
        else {
            $("#selectBranch").removeAttr("disabled");
        }
    }

    /**
        * Obtiene los procesos de amortización al seleccionar el tipo de operación.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del tipo de operación seleccionado.
        */
    SelectOperation(event, selectedItem) {
        if (selectedItem.Id > 0) {
            //MainAutomaticAmortization.RefreshGridAmortization();
        }
    }

    /**
        * Valida que el valor a amortizar no sobrepase el valor parametrizado.
        *
        */
    ChangeAmortizedValue() {
        if ($("#AmortizedValue").val() > 1) {
            $("#alert").UifAlert('show', Resources.YouCanNotEnterValueGreater, "warning");
            $("#AmortizedValue").val(1);
            setTimeout(function () {
                $("#alert").UifAlert('hide');
            }, 5000);
        }
        else {
            $("#AmortizedValue").val();
        }
    }

    /**
        * Obtiene los asegurados por número de documento.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del asegurado seleccionado.
        */
    AutocompleteInsuredDocumentNumber(event, selectedItem) {
        individualId = selectedItem.Id;
        if (individualId > 0) {
            $('#InsuredDocumentNumber').val(selectedItem.DocumentNumber);
            $('#InsuredName').val(selectedItem.Name);
        }
        else {
            $('#InsuredDocumentNumber').val("");
            $('#InsuredName').val("");
        }
    }

    /**
        * Obtiene los asegurados por nombre.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del asegurado seleccionado.
        */
    AutocompleteInsuredName(event, selectedItem) {
        individualId = selectedItem.Id;
        if (individualId > 0) {
            $('#InsuredDocumentNumber').val(selectedItem.DocumentNumber);
            $('#InsuredName').val(selectedItem.Name);
        }
        else {
            $('#InsuredDocumentNumber').val("");
            $('#InsuredName').val("");
        }
    }

    /**
        * Limpia los parámetros de generación del proceso de amortización.
        *
        */
    CleanSearchAmortization() {
        $('#selectOperation').val("");
        $('#selectBranch').val("");
        $("#selectPrefix").val("");
        $("#PolicyNumber").val("");
        $("#InsuredDocumentNumber").val("");
        $("#InsuredName").val("");
        $("#existing").val("");
        $("#alert").UifAlert('hide');
    }

    /**
        * Genera el proceso de amortización.
        *
        */
    GenerateAmortization() {
        $("#alert").UifAlert('hide');
        $("#formAutomaticAmortization").validate();

        if ($("#formAutomaticAmortization").valid()) {
            if ($("#PolicyNumber").val() != "") {
                if (($("#selectBranch").val() == "") || ($("#selectPrefix").val() == "")) {
                    $("#alert").UifAlert('show', Resources.SelectBranchPrefix, "danger");
                    return;
                }
            }
            operationType = "G";
            $("#saveMessageTitle").text(Resources.GeneratingAutomaticAmortization);
            $("#saveMessageModal").text(Resources.MessageGenerateAutomaticAmortizationDialog);
            $('#modalSave').appendTo("body").modal('show');
            return true;
        }
    }

    /**
        * Aplica la amortización de pólizas.
        *
        */
    ApplyAmortization() {
        var rows = $("#tableAmortizationPolicies").UifDataTable("getData");
        if (rows != null) {
            operationType = "A";
            $("#saveMessageTitle").text(Resources.ApplyAmortization);
            $("#saveMessageModal").text(Resources.MessageGenerateAutomaticAmortizationDialog);
            $('#modalSave').appendTo("body").modal('show');
            return true;
        }
        else {
            $("#alertProcess").UifAlert('show', Resources.NoRecordsToContinue, "warning");

        }
    }

    /**
        * Selecciona un proceso de amortización.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Objeto con valores del proceso seleccionado.
        * @param {Object} position - Indice del proceso seleccionado.
        */
    RowSelectedTableProcess(event, data, position) {
        $("#alert").UifAlert('hide');
        $("#ApplyAmortization").hide();
        $("#PrintAmortization").hide();
        $("#DeleteAmortization").hide();

        if ((data.IsActive == "Actived")) {
            $("#LowAmortization").show();
        }
        else {
            $("#LowAmortization").hide();
        }
    }

    /**
        * Busca la amortización de pólizas por número de proceso.
        *
        */
    SearchAmortizationProcess(event, value) {
        $("#alertProcess").UifAlert('hide');
        MainAutomaticAmortization.GetProcessDetailsAmortization(value);
    }

    /**
        * Genera el reporte de pendientes de aplicación.
        *
        */
    PrintAmortization() {
        $("#formAutomaticAmortization").validate();

        if ($("#formAutomaticAmortization").valid()) {

            if ($("#ProcessAmortizationPolicies").val() != "") {

                MainAutomaticAmortization.ShowAmortizationByProcessNumber($("#ProcessAmortizationPolicies").val());
                MainAutomaticAmortization.ShowReportAmortization();
            }
        }
    }

    /**
        * Clic en los tabs para ocultar los botones.
        *
        * @param {String} event - Seleccionar.
        * @param {Object} newly - Tab nuevo.
        * @param {Object} old   - Tab viejo.
        */
    ChangeTabAmortization(event, newly, old) {
        $("#ApplyAmortization").hide();
        $("#LowAmortization").hide();
        $("#PrintAmortization").hide();
        $("#DeleteAmortization").hide();

        if (newly.hash == "#tabProcess" || old == "#tabProcess") {
            var pendingProcess = $("#tablePendingProcessesAmortization").UifDataTable("getSelected");
            if (pendingProcess != null) {
                if ((pendingProcess[0].Progress == Resources.Finalized) && (pendingProcess[0].IsActive == "Actived")) {
                    $("#LowAmortization").show();
                }
                else {
                    $("#LowAmortization").hide();
                }
            }
        }
        else if (newly.hash == "#tabAmortizationPolicies" || old == "#tabAmortizationPolicies") {

            var ids = $("#tableAmortizationPolicies").UifDataTable("getData");

            if (ids.length > 0) {
                $("#ApplyAmortization").show();
                $("#PrintAmortization").show();
                $("#DeleteAmortization").show();
            }
        }
    }

    /**
        * Da de baja un proceso automático de amortizaciones.
        *
        */
    LowAmortization() {

        var process = $("#tablePendingProcessesAmortization").UifDataTable("getSelected");
        if (process != null) {
            operationType = "L";
            $("#saveMessageTitle").text(Resources.Low);
            $("#saveMessageModal").text(Resources.LowProcessConfirmation);
            $('#modalSave').appendTo("body").modal('show');
            return true;
        }
    }

    /**
        * Elimina temporales de amortizaciones.
        *
        */
    DeleteAmortization() {
        $("#alertProcess").UifAlert('hide');
        var rows = $("#tableAmortizationPolicies").UifDataTable("getSelected");
        if (rows != null) {
            $('#modalDeleteAmortization').appendTo("body").modal('show');
            return true;
        }
        else {
            $("#alertProcess").UifAlert('show', Resources.SelectRecords, "warning");
        }
    }

    /**
        * Selecciona un proceso de amortización.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Objeto con valores del proceso seleccionado.
        * @param {Object} position - Indice del proceso seleccionado.
        */
    RowSelectedTableAmortization(event, data, position) {
        $("#alertProcess").UifAlert('hide');
        var rows = $("#tableAmortizationPolicies").UifDataTable("getData");

        if (rows != null) {
            for (var i = 0; i < rows.length; i++) {
                $($("#tableAmortizationPolicies > tbody > tr")[i]).find('button').prop('checked');
                MainAutomaticAmortization.RefreshGridAmortization;
            }
        }
        else {
            $("#ApplyAmortization").hide();
            MainAutomaticAmortization.RefreshGridAmortization;
        }
    }

    /**
        * Genera el proceso de amortización.
        *
        */
    GenerateModalAmortization() {
        $('#modalSave').modal('hide');
        $("#alert").UifAlert('hide');
        // Generación
        if (operationType == "G") {
            operationId = ($('#selectOperation').val() != "") ? $('#selectOperation').val() : -1;
            branchId = ($('#selectBranch').val() != "") ? $('#selectBranch').val() : -999;
            prefixId = ($('#selectPrefix').val() != "") ? $('#selectPrefix').val() : -999;
            policyNumber = ($('#PolicyNumber').val() != "") ? $('#PolicyNumber').val() : "0";
            limitAmount = ($('#AmortizedValue').val() != "") ? $('#AmortizedValue').val() : 1.00;

            if ($('#selectOperation').val() != "") {
                $('div#container').show();

                $("#GenerateAmortization").attr("disabled", "disabled");
                lockScreen();
                setTimeout(function () {
                    MainAutomaticAmortization.GenerateAutomaticAmortization(operationId, branchId, prefixId, individualId, policyNumber, limitAmount, $("#StartDateAmortization").val(), $("#StartDateAmortization").val()).then(function (AutomaticAmortization) {
                        if (AutomaticAmortization[0].ProcessNumber > 0) {
                            $("#alert").UifAlert('show', Resources.MessageProcessAutomaticAmortization, "success");
                            $("#tableAmortizationPolicies").dataTable().fnClearTable();
                            time = window.setInterval(MainAutomaticAmortization.RefreshGridAmortization, 8000);
                        }
                        else {
                            $("#alert").UifAlert('show', AutomaticAmortization[0].MessageError, "danger");
                        }
                        $('div#container').hide();
                        $("#GenerateAmortization").removeAttr("disabled");
                        unlockScreen();
                    });
                }, 300);

                
            }
            else {
                $("#alert").UifAlert('show', Resources.SelectOperationType, "danger");
            }
        }

        // Baja
        if (operationType == "L") {
            var process = $("#tablePendingProcessesAmortization").UifDataTable("getSelected");
            if (process != null) {
                $("#LowAmortization").attr("disabled", "disabled");
                lockScreen();
                MainAutomaticAmortization.LowAutomaticAmortization(process[0].Id).then(function (LowAutomaticAmortizationData) {
                    if (LowAutomaticAmortizationData[0].ProcessNumber == 0) {
                        $("#alert").UifAlert('show', Resources.MessageLowProcessAutomaticCreditNotes, "success");
                        $("#tableAmortizationPolicies").dataTable().fnClearTable();
                        time = window.setInterval(MainAutomaticAmortization.RefreshGridAmortization, 8000);
                    }
                    else {
                        $("#alert").UifAlert('show', LowAutomaticAmortizationData[0].MessageError, "danger");
                    }
                    $('div#container').hide();
                    $("#LowAmortization").removeAttr("disabled");
                    MainAutomaticAmortization.GetAmortizations();
                    unlockScreen();
                });
            }
        }

        // Aplicar
        if (operationType == "A") {
            $("#ApplyAmortization").attr("disabled", "disabled");
            lockScreen();
            var itemsToApplied = MainAutomaticAmortization.SetDataAmortizations();
            var totalDebits = $("#TotalDebits").val().replace("$", "").replace(/,/g, "").replace(" ", "");
            var totalCredits = $("#TotalCredits").val().replace("$", "").replace(/,/g, "").replace(" ", "")

            MainAutomaticAmortization.ApplyAmortizations(itemsToApplied, totalDebits, totalCredits).then(function (ApplyData) {
                if (ApplyData.success == false) {
                    $("#alertProcess").UifAlert('show', ApplyData.result, "danger");
                }
                else {

                    if (ApplyData.result[0].ProcessNumber > 0) {
                        $("#alertProcess").UifAlert('show', Resources.ApplySuccessfullyAmortization + " " + Resources.BillNum
                            + ApplyData.result[0].ReceiptNumber, "success");
                        $("#tableAmortizationPolicies").dataTable().fnClearTable();
                        time = window.setInterval(MainAutomaticAmortization.RefreshGridAmortization, 8000);
                    }
                    else {
                        $("#alertProcess").UifAlert('show', ApplyData.result[0].MessageError, "danger");
                    }
                }
                MainAutomaticAmortization.GetAmortizations();
                $('div#container').hide();
                $("#ApplyAmortization").removeAttr("disabled");
                unlockScreen();
            });
        }
    }

    /**
        * Elimina el proceso de amortización.
        *
        */
    DeleteModalAmortization() {

        $('#modalDeleteAmortization').modal('hide');
        $("#DeleteAmortization").attr("disabled", "disabled");
        lockScreen();

        setTimeout(function () {
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "AutomaticAmortization/DeleteAutomaticAmortization",
                data: { "itemsToAppliedModel": MainAutomaticAmortization.SetDeleteAmortization() },
                success: function (data) {

                    if (data.success == false) {
                        $("#alertProcess").UifAlert('show', data.result, "danger");
                    }
                    else {

                        if (data.result[0].ProcessNumber == 0) {
                            $("#alertProcess").UifAlert('show', Resources.DeleteSuccessfully, "success");
                            MainAutomaticAmortization.GetProcessDetailsAmortization($("#ProcessAmortizationPolicies").val());
                            time = window.setInterval(MainAutomaticAmortization.RefreshGridAmortization, 8000);
                        }
                        else {
                            $("#alertProcess").UifAlert('show', data.result[0].MessageError, "danger");
                        }
                    }
                    MainAutomaticAmortization.GetAmortizations();
                    $('div#container').hide();
                    $("#DeleteAmortization").removeAttr("disabled");
                    unlockScreen();
                }

            });
        }, 1000);


    }

    /**
        * Selecciona una fecha inicio de filtro para el proceso de amortización.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} date     - Objeto con valores de la fecha seleccionada.
        */
    ChangeStartDateAmortization(event, date) {
        $("#alertProcess").UifAlert('hide');
        if ($("#EndDateAmortization").val() != "") {

            if (compare_dates($("#StartDateAmortization").val(), $("#EndDateAmortization").val())) {
                $("#alertProcess").UifAlert('show', Resources.ValidateDateTo, 'warning');
                $("#StartDateAmortization").val('');
            } else {
                $("#StartDateAmortization").val(date);
            }
        }
    }

    /**
        * Selecciona una fecha fin de filtro para el proceso de amortización.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} date     - Objeto con valores de la fecha seleccionada.
        */
    ChangeEndDateAmortization(event, date) {
        $("#alertProcess").UifAlert('hide');
        if ($("#StartDateAmortization").val() != "") {
            if (compare_dates($("#StartDateAmortization").val(), $("#EndDateAmortization").val())) {
                $("#alertProcess").UifAlert('show', Resources.ValidateDateFrom, 'warning');
                $("#EndDateAmortization").val('');
            } else {
                $("#EndDateAmortization").val(date);
            }
        }
    }

    /**
        * Obtiene los procesos de amortización por estado.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del estado seleccionado.
        */
    ItemSelectedStatusProcess(event, selectedItem) {
        MainAutomaticAmortization.RefreshGridAmortization();
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Visualiza el reporte pendiente de amortización en formato pdf.
        *
        * @param {Number} processAmortization - Número de proceso de amortización.
        */
    static ShowAmortizationByProcessNumber(processAmortization) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "AutomaticAmortization/GetAmortizationByProcessNumberReport",
            data: { "processAmortization": processAmortization },
            success: function (data) { }
        });
    }

    /**
        * Busca el detalle del proceso de cruce automático de amortizaciones.
        *
        * @param {Number} processNumber - Número de proceso de amortización.
        */
    static GetProcessDetailsAmortization(processNumber) {
        $("#tableAmortizationPolicies").dataTable().fnClearTable();
        $("#ApplyAmortization").hide();
        $("#LowAmortization").hide();
        $("#PrintAmortization").hide();
        $("#DeleteAmortization").hide();

        $("#UserNameAmortization").val("");
        $("#ProcessDate").val("");
        $("#TotalDebits").val("");
        $("#TotalCredits").val("");

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "AutomaticAmortization/GetHeaderAmortizationByProcessNumber",
            data: { "processNumber": processNumber },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success == false) {
                    $("#alertProcess").UifAlert('show', data.result, "danger");
                }
                else {

                    if (data[0].UserName != "-1") {
                        $("#UserNameAmortization").val(data[0].UserName);
                        $("#ProcessDate").val(data[0].ProcessDate);
                        $("#TotalDebits").val("$ " + NumberFormat(data[0].TotalCredits, "2", ".", ","));
                        $("#TotalCredits").val("$ " + NumberFormat(data[0].TotalDebits, "2", ".", ","));

                        var controller = ACC_ROOT + "AutomaticAmortization/GetAmortizationByProcessNumber?processNumber=" + processNumber;
                        $("#tableAmortizationPolicies").UifDataTable({ source: controller });
                        $("#ApplyAmortization").show();
                        $("#PrintAmortization").show();
                        $("#DeleteAmortization").show();
                    }
                    else {
                        $("#alertProcess").UifAlert('show', Resources.NoRecordsToContinue, "warning");
                    }

                }



            }
        });
    }

    /**
        * Refresca la grilla de proceso amortización.
        *
        */
    static RefreshGridAmortization() {
        operationId = ($('#selectOperation').val() != "") ? $('#selectOperation').val() : -1;
        branchId = ($('#selectBranch').val() != "") ? $('#selectBranch').val() : -999;
        prefixId = ($('#selectPrefix').val() != "") ? $('#selectPrefix').val() : -999;
        policyNumber = ($('#PolicyNumber').val() != "") ? $('#PolicyNumber').val() : "0";

        if (MainAutomaticAmortization.ValidateAmortizationProcess() == true) {
            window.clearInterval(time);
        }

        var controller = ACC_ROOT + "AutomaticAmortization/GetAmortizationProcessByStatus?statusId=" + $("#SelectStatusProcess").val();
        $("#tablePendingProcessesAmortization").UifDataTable({ source: controller });
    }

    /**
        * Setea el modelo de amortización.
        *
        * @param {Number} processAmortization - Número de proceso de amortización.
        */
    static AutomaticAmortization(processAmortization) {

        oAmortizationItems.AmortizationItem = [];

        if (processAmortization.length > 0) {
            if (processAmortization != null) {

                for (var i in processAmortization) {
                    oAmortizationItem = {
                        BranchId: 0,
                        BranchName: "",
                        PrefixId: 0,
                        PrefixName: "",
                        PolicyId: 0,
                        Policy: "",
                        Endorsement: 0,
                        Insured: "",
                        Payer: "",
                        PrincipalAgent: "",
                        CurrencyId: 0,
                        Currency: "",
                        Amount: 0,
                        Change: 0,
                        LocalAmount: 0,
                        ApplicationReceiptNumber: "",
                        EntryNumber: "",
                        ProcessNumber: 0
                    };

                    oAmortizationItem.BranchId = processAmortization[i].BranchId;
                    oAmortizationItem.BranchName = processAmortization[i].Branch;
                    oAmortizationItem.PrefixId = processAmortization[i].PrefixId;
                    oAmortizationItem.PrefixName = processAmortization[i].Prefix;
                    oAmortizationItem.PolicyId = processAmortization[i].PolicyId;
                    oAmortizationItem.Policy = processAmortization[i].Policy;
                    oAmortizationItem.Endorsement = processAmortization[i].Endorsement;
                    oAmortizationItem.Insured = processAmortization[i].Insured;
                    oAmortizationItem.Payer = processAmortization[i].Payer;
                    oAmortizationItem.PrincipalAgent = processAmortization[i].PrincipalAgent;
                    oAmortizationItem.CurrencyId = processAmortization[i].CurrencyId;
                    oAmortizationItem.Currency = processAmortization[i].Currency;
                    oAmortizationItem.Amount = processAmortization[i].Amount;
                    oAmortizationItem.Change = processAmortization[i].Change;
                    oAmortizationItem.LocalAmount = processAmortization[i].LocalAmount;
                    oAmortizationItem.ApplicationReceiptNumber = processAmortization[i].ApplicationReceiptNumber;
                    oAmortizationItem.EntryNumber = processAmortization[i].EntryNumber;
                    oAmortizationItem.ProcessNumber = processAmortization[i].ProcessNumber;
                    oAmortizationItems.AmortizationItem.push(oAmortizationItem);
                }
            }
        }

        return oAmortizationItems;
    }

    /**
        * Setea el modelo de ítems seleccionados para aplicar.
        *
        */
    static SetDataAmortizations() {

        oAmortizationItems.AmortizationItem = [];

        var rows = $("#tableAmortizationPolicies").UifDataTable("getData");
        if (rows != null) {
            $("#ApplyAmortization").show();
            for (var j = 0; j < rows.length; j++) {
                oAmortizationItem = {
                    ProcessId: 0,
                    ImputationId: 0,
                    AmortizationItemId: 0,
                    BranchId: 0,
                    BranchName: "",
                    PrefixId: 0,
                    PrefixName: "",
                    PolicyId: 0,
                    PolicyNumber: "",
                    EndorsementNumber: 0,
                    InsuredId: 0,
                    InsuredDocumentNumber: "",
                    InsuredName: "",
                    InsuredPersonTypeId: 0,
                    AgentId: 0,
                    AgentDocumentNumber: "",
                    AgentName: "",
                    AgentPersonTypeId: 0,
                    PayerId: 0,
                    PayerDocumentNumber: "",
                    PayerName: "",
                    PayerPersonTypeId: 0,
                    CurrencyId: 0,
                    CurrencyName: "",
                    IncomeAmount: 0,
                    Exchange: 0,
                    Amount: 0,
                    ImputationReceiptNumber: 0
                };

                oAmortizationItem.ProcessId = $("#ProcessAmortizationPolicies").val();
                oAmortizationItem.ImputationId = rows[j].ImputationId;
                oAmortizationItem.AmortizationItemId = rows[j].Id;
                oAmortizationItem.BranchId = rows[j].BranchId;
                oAmortizationItem.BranchName = rows[j].Branch;
                oAmortizationItem.PrefixId = rows[j].PrefixId;
                oAmortizationItem.PrefixName = rows[j].Prefix;
                oAmortizationItem.PolicyId = rows[j].PolicyId;
                oAmortizationItem.PolicyNumber = rows[j].Policy;
                oAmortizationItem.EndorsementNumber = rows[j].Endorsement;
                oAmortizationItem.InsuredId = 0;
                oAmortizationItem.InsuredDocumentNumber = rows[j].Insured;
                oAmortizationItem.InsuredName = "";
                oAmortizationItem.InsuredPersonTypeId = rows[j].InsuredPersonType;
                oAmortizationItem.AgentId = 0;
                oAmortizationItem.AgentDocumentNumber = rows[j].PrincipalAgent;
                oAmortizationItem.AgentName = "";
                oAmortizationItem.AgentPersonTypeId = rows[j].AgentPersonType;
                oAmortizationItem.PayerId = 0;
                oAmortizationItem.PayerDocumentNumber = rows[j].Payer;
                oAmortizationItem.PayerName = "";
                oAmortizationItem.PayerPersonTypeId = rows[j].PayerPersonType;
                oAmortizationItem.CurrencyId = rows[j].CurrencyId;
                oAmortizationItem.CurrencyName = rows[j].Currency;
                oAmortizationItem.IncomeAmount = parseFloat(ClearFormatCurrency(rows[j].Amount));
                oAmortizationItem.Exchange = parseFloat(ClearFormatCurrency(rows[j].Change));
                oAmortizationItem.Amount = parseFloat(ClearFormatCurrency(rows[j].LocalAmount));
                if (rows[j].ApplicationReceiptNumber == "") {
                    oAmortizationItem.ImputationReceiptNumber = 0;
                }
                else {
                    oAmortizationItem.ImputationReceiptNumber = rows[j].ApplicationReceiptNumber;
                }



                oAmortizationItems.AmortizationItem.push(oAmortizationItem);
            }
        }

        return oAmortizationItems;
    }

    /**
        * Setea el modelo de ítems seleccionados para eliminar.
        *
        */
    static SetDeleteAmortization() {

        oAmortizationItems.AmortizationItem = [];

        var rows = $("#tableAmortizationPolicies").UifDataTable("getSelected");

        if (rows != null) {
            for (var j = 0; j < rows.length; j++) {
                oAmortizationItem = {
                    ProcessId: 0,
                    ImputationId: 0,
                    AmortizationItemId: 0,
                    BranchId: 0,
                    BranchName: "",
                    PrefixId: 0,
                    PrefixName: "",
                    PolicyId: 0,
                    PolicyNumber: "",
                    EndorsementNumber: 0,
                    InsuredId: 0,
                    InsuredDocumentNumber: "",
                    InsuredName: "",
                    InsuredPersonTypeId: 0,
                    AgentId: 0,
                    AgentDocumentNumber: "",
                    AgentName: "",
                    AgentPersonTypeId: 0,
                    PayerId: 0,
                    PayerDocumentNumber: "",
                    PayerName: "",
                    PayerPersonTypeId: 0,
                    CurrencyId: 0,
                    CurrencyName: "",
                    IncomeAmount: 0,
                    Exchange: 0,
                    Amount: 0,
                    ImputationReceiptNumber: 0
                };

                oAmortizationItem.ProcessId = $("#ProcessAmortizationPolicies").val();
                oAmortizationItem.ImputationId = rows[j].ImputationId;
                oAmortizationItem.AmortizationItemId = rows[j].ItemId;
                oAmortizationItem.BranchId = rows[j].BranchId;
                oAmortizationItem.BranchName = rows[j].Branch;
                oAmortizationItem.PrefixId = rows[j].PrefixId;
                oAmortizationItem.PrefixName = rows[j].Prefix;
                oAmortizationItem.PolicyId = rows[j].PolicyId;
                oAmortizationItem.PolicyNumber = rows[j].Policy;
                oAmortizationItem.EndorsementNumber = rows[j].Endorsement;
                oAmortizationItem.InsuredId = 0;
                oAmortizationItem.InsuredDocumentNumber = rows[j].Insured;
                oAmortizationItem.InsuredName = "";
                oAmortizationItem.InsuredPersonTypeId = rows[j].InsuredPersonType;
                oAmortizationItem.AgentId = 0;
                oAmortizationItem.AgentDocumentNumber = rows[j].PrincipalAgent;
                oAmortizationItem.AgentName = "";
                oAmortizationItem.AgentPersonTypeId = rows[j].AgentPersonType;
                oAmortizationItem.PayerId = 0;
                oAmortizationItem.PayerDocumentNumber = rows[j].Payer;
                oAmortizationItem.PayerName = "";
                oAmortizationItem.PayerPersonTypeId = rows[j].PayerPersonType;
                oAmortizationItem.CurrencyId = rows[j].CurrencyId;
                oAmortizationItem.CurrencyName = rows[j].Currency;

                oAmortizationItem.IncomeAmount = parseFloat(ClearFormatCurrency(rows[j].Amount));
                oAmortizationItem.Exchange = parseFloat(ClearFormatCurrency(rows[j].Change));
                oAmortizationItem.Amount = parseFloat(ClearFormatCurrency(rows[j].LocalAmount));


                oAmortizationItem.ImputationReceiptNumber = rows[j].ApplicationReceiptNumber;

                oAmortizationItems.AmortizationItem.push(oAmortizationItem);
            }
        }

        return oAmortizationItems;
    }

    /**
        * Visualiza el reporte en formato pdf.
        *
        */
    static ShowReportAmortization() {
        window.open(ACC_ROOT + "AutomaticAmortization/ShowAutomaticAmortization", 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }

    /**
        * Valida si el proceso ha finalizado.
        *
        */
    static ValidateAmortizationProcess() {
        var process = $('#tablePendingProcessesAmortization').UifDataTable('getData');
        var count = 0;

        for (var i in process) {
            if (process[i].Progress == Resources.Finalized) {
                count++;
            }
            else {
                return false;
            }

        }
        return ((count > 0) && (process.length > 0))
    }

    /**
        * Obtiene los procesos de amortización al iniciar la pantalla.
        *
        */
    static GetAmortizations() {
        operationId = ($('#selectOperation').val() != "") ? $('#selectOperation').val() : -1;
        branchId = ($('#selectBranch').val() != "") ? $('#selectBranch').val() : -999;
        prefixId = ($('#selectPrefix').val() != "") ? $('#selectPrefix').val() : -999;
        policyNumber = ($('#PolicyNumber').val() != "") ? $('#PolicyNumber').val() : "0";

        var controller = ACC_ROOT + "AutomaticAmortization/GetAmortizationProcessByStatus?statusId=" + $("#SelectStatusProcess").val();
        $("#tablePendingProcessesAmortization").UifDataTable({ source: controller });
    }

    static GenerateAutomaticAmortization(operationId, branchId, prefixId, individualId, policyNumber, limitAmount, startDate, endDate) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: ACC_ROOT + "AutomaticAmortization/GenerateAutomaticAmortization",
                data: {
                    "operationTypeId": operationId, "branchId": branchId, "prefixId": prefixId,
                    "individualId": individualId, "policyNumber": policyNumber, "limitAmount": limitAmount,
                    "startDate": startDate, "endDate": endDate
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (AutomaticAmortization) {
                    resolve(AutomaticAmortization);
                }
            });
        });
    }

    static LowAutomaticAmortization(processId) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: ACC_ROOT + "AutomaticAmortization/LowAutomaticAmortization",
                data: { "processId": processId },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (LowAutomaticAmortizationData) {
                    resolve(LowAutomaticAmortizationData);
                }
            });
        });
    }

    static ApplyAmortizations(itemsToApplied, totalDebits, totalCredits) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                // async: false,
                type: "POST",
                url: ACC_ROOT + "AutomaticAmortization/ApplyAmortizations",
                data: {
                    "itemsToAppliedModel": itemsToApplied,
                    "totalDebits": totalDebits,
                    "totalCredits": totalCredits
                },
                success: function (ApplyData) {
                    resolve(ApplyData);
                }
            });
        });
    }

}
