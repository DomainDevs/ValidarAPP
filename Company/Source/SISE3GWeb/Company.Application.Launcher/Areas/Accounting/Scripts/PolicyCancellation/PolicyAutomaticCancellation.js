/**
 * @file   MainPolicyAutomaticCancellation.js
 * @author Desarrollador
 * @version 0.1
 */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var massiveTime;
var individualId = -1;
var insuredId = -1;
var intermediaryId = -1;
var intermediaryCompanyId = -1;
var intermediaryDocumentNumber = "";
var grouperId = -1;
var intermediaryName = "";
var operationType = "";
var processType = 1;
var branchId = 0;
var salePointId = 0;
var prefixId = 0;
var policyNumber = "";
var cancellationtypeId = 0;
var businessId = 0;


/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainPolicyAutomaticCancellation();
});

class MainPolicyAutomaticCancellation extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        var controller = ACC_ROOT + "Common/GetBranchs";
        $("#SelectBranchAutomatic").UifSelect({ source: controller });

        $("#PickerCutDate").val($("#ViewBagIssueDate").val());
        $("#PickerIssueDateFrom").val($("#ViewBagIssueDate").val());
        $("#PickerIssueDateTo").val($("#ViewBagIssueDate").val());

        MainPolicyAutomaticCancellation.HiddenButtons();
        MainPolicyAutomaticCancellation.RefreshMassiveProcess();
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#SelectBranchAutomatic").on('binded', this.BindedBranch);
        $("#SelectBranchAutomatic").on('itemSelected', this.ItemSelectedBranch);
        $("#InsuredDocumentNumber").on('itemSelected', this.AutocompleteInsuredDocumentNumber);
        $("#InsuredName").on('itemSelected', this.AutocompleteInsuredName);
        $("#IntermediaryDocumentNumber").on('itemSelected', this.AutocompleteIntermediaryDocumentNumber);
        $("#IntermediaryName").on('itemSelected', this.AutocompleteIntermediaryName);
        $("#PolicyNumber").on('blur', this.ValidatePolicyNumber);

        $("#InsuredSearch").on('search', this.GetInsuredByNumberOrName);
        $("#tableInsured").on('rowSelected', this.RowSelectedInsured);
        $("#IntermediarySearch").on('search', this.GetMediatorByNumberOrName);
        $("#tableIntermediary").on('rowSelected', this.RowSelectedIntermediary);
        $("#GrouperSearch").on('search', this.GetGrouperByNumberOrName);
        $("#tableGroupers").on('rowSelected', this.RowSelectedGrouper);

        $('#PickerCutDate').on('datepicker.change', this.ValidateLaterIssueDate);
        $('#PickerIssueDateFrom').on('datepicker.change', this.ValidateDateFromNoGreaterThanDateTo);
        $('#PickerIssueDateTo').on('datepicker.change', this.ValidateDateToNoLessThanDateFrom);

        $("#CleanSearch").on('click', this.CleanFields);
        $("#AutoCancellationGenerate").on('click', this.GenerateSelectionPolicies);
        $("#GenerateModalCancellation").on('click', this.ExecuteCancellationProcess);
        $("#PolicyProcessNumber").on('search', this.GetPolicyCancellationProcess);
        $("#tabMainPolicyAutomaticCancellation").on('change', this.ChangeTabMain);
        $("#PolicyCancel").on('click', this.CleanDataPolicy);
        $("#PolicyExportExcel").on('click', this.ExportToExcelDataPolicy);
        $("#PolicyImportExcel").on('click', this.OpenInputFileDataPolicy);
        $("#SelectFilePolicy").on('click', this.LoadDataPolicy);
        $("#PolicyProcessCancellation").on('click', this.ProcessPolicyCancellation);

        $("#ProcessedProcessNumber").on('search', this.GetCancellationProcessed);
        $("#CancellationCancel").on('click', this.CleanDataCancellaton);
        $("#CancellationExportExcel").on('click', this.ExportToExcelDataCancellation);
        $("#CancellationImportExcel").on('click', this.OpenInputFileDataCancellation);
        $("#SelectFileCancellation").on('click', this.LoadDataCancellation);
        $("#ProcessCrossingCollections").on('click', this.ProcessCrossingCollections);

        $("#ErrorProcessNumber").on('search', this.GetMistakeProcess);
        $("#ErrorCancel").on('click', this.CleanDataMistake);
        $("#ErrorExportExcel").on('click', this.ExportToExcelDataMistake);
        $("#ErrorImportExcel").on('click', this.OpenInputFileDataMistake);
        $("#SelectFileMistake").on('click', this.LoadDataMistake);
        $("#ReprocessCancellation").on('click', this.ReprocessCancellation);

        $("#AppliedProcessNumber").on('search', this.GetAppliedCancellations);
        $("#AppliedExportExcel").on('click', this.ExportToExcelDataApplied);

    }

    /**
        * Bloquea el combo de sucursal una vez que esta cargado.
        *
        */
    BindedBranch() {
        if ($("#ViewBagBranchDisable").val() == "1") {
            $("#SelectBranchAutomatic").attr("disabled", "disabled");
        }
        else {
            $("#SelectBranchAutomatic").removeAttr("disabled");
        }
        var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + $("#SelectBranchAutomatic").val();
        $("#SelectSalePoint").UifSelect({ source: controller });
        $("#SelectCancellationToBeMade").val("1");
    }

    /**
        * Obtiene los puntos de venta de una sucursal.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la sucursal seleccionada.
        */
    ItemSelectedBranch(event, selectedItem) {
        if (selectedItem.Id > 0 && selectedItem.Id != null) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#SelectSalePoint").UifSelect({ source: controller });
        }
        else {
            $("#SelectSalePoint").UifSelect();
        }
    }

    /**
       * Autocomplete número documento asegurado.
       *
       * @param {String} event         - Seleccionar número de documento.
       * @param {Object} selectedItem  - Objeto con valores del registro seleccionado.
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
       * Autocomplete nombre de asegurado.
       *
       * @param {String} event         - Seleccionar nombre.
       * @param {Object} selectedItem  - Objeto con valores del registro seleccionado.
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
       * Autocomplete número documento intermediario.
       *
       * @param {String} event         - Seleccionar número de documento.
       * @param {Object} selectedItem  - Objeto con valores del registro seleccionado.
       */
    AutocompleteIntermediaryDocumentNumber(event, selectedItem) {
        $("#alert").UifAlert('hide');
        intermediaryId = selectedItem.Id;
        if (intermediaryId > 0) {
            $('#IntermediaryDocumentNumber').val(selectedItem.BrokerDocumentNumber);
            $('#IntermediaryName').val(selectedItem.BrokerName);
            intermediaryCompanyId = selectedItem.BrokerIndividualId;
            intermediaryDocumentNumber = selectedItem.BrokerDocumentNumber;
            intermediaryName = selectedItem.BrokerName;
        }
        else {
            $('#IntermediaryDocumentNumber').val("");
            $('#IntermediaryName').val("");
            intermediaryCompanyId = -1;
        }
    }

    /**
       * Autocomplete nombre de intermediario.
       *
       * @param {String} event         - Seleccionar nombre.
       * @param {Object} selectedItem  - Objeto con valores del registro seleccionado.
       */
    AutocompleteIntermediaryName(event, selectedItem) {
        $("#alert").UifAlert('hide');
        intermediaryId = selectedItem.Id;
        if (intermediaryId > 0) {
            $('#IntermediaryDocumentNumber').val(selectedItem.BrokerDocumentNumber);
            $('#IntermediaryName').val(selectedItem.BrokerName);
            intermediaryCompanyId = selectedItem.BrokerIndividualId;
            intermediaryDocumentNumber = selectedItem.BrokerDocumentNumber;
            intermediaryName = selectedItem.BrokerName;
        }
        else {
            $('#IntermediaryDocumentNumber').val("");
            $('#IntermediaryName').val("");
            intermediaryCompanyId = -1;
        }
    }

    /**
       * Valida la existencia de la póliza.
       *
       */
    ValidatePolicyNumber() {
        $("#alert").UifAlert('hide');

        if ($("#PolicyNumber").val() != "") {
            if ($("#SelectBranchAutomatic").val() == "") {
                $("#alert").UifAlert('show', Resources.SelectBranch, "warning");
                return;
            }
            if ($("#SelectPrefix").val() == "") {
                $("#alert").UifAlert('show', Resources.SelectBranchPrefix, "warning");
                return;
            }

            $.ajax({
                type: "GET",
                url: ACC_ROOT + "PolicyCancellation/GetPolicyByBranchPrefixPolicyNumber",
                data: {
                    "branchId": $("#SelectBranchAutomatic").val(),
                    "prefixId": $("#SelectPrefix").val(),
                    "policyNumber": $("#PolicyNumber").val()
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (data == 0) {
                        $("#alert").UifAlert('show', Resources.PolicyNotFound, "danger");
                        $("#PolicyNumber").val("");
                    }
                }
            });
        }
    }

    /**
       * Limpia los campos de búsqueda de selección de pólizas a cancelar.
       *
       */
    CleanFields() {
        //$("#SelectCancellationToBeMade").val("");
        $("#SelectBranchAutomatic").val("");
        $("#SelectSalePoint").val("");
        $("#SelectPrefix").val("");
        $("#InsuredDocumentNumber").val("");
        $("#InsuredName").val("");
        $("#IntermediaryDocumentNumber").val("");
        $("#IntermediaryName").val("");
        $("#SelectGrouper").val("");
        $("#SelectBusiness").val("");
        $("#PickerCutDate").val("");
        $("#PickerIssueDateFrom").val("");
        $("#PickerIssueDateTo").val("");
    }

    /**
        * Busca los asegurados por número de documento o nombre.
        *
        * @param {String} event - Buscar.
        * @param {Object} value - Número documento o nombre.
        */
    GetInsuredByNumberOrName(event, value) {
        $("#alert").UifAlert('hide');

        if ($("#InsuredSearch").val() != "") {
            var controller = ACC_ROOT + "PolicyCancellation/GetInsuredByNumberOrName?numberName=" + value;
            $("#tableInsured").UifDataTable({ source: controller });

            $('#modalSearchInsured').UifModal('showLocal', Resources.SelectInsured);
        }
        else {
            $("#alert").UifAlert('show', Resources.PlaceHolderInsured, "warning");
        }
    }

    /**
        * Selecciona un asegurado de la tabla y cierra el modal de búsqueda.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Información del registro seleccionado.
        * @param {Number} position - Posición del registro seleccionado.
        */
    RowSelectedInsured(event, data, position) {
        $('#modalSearchInsured').UifModal('hide');
        individualId = data.IndividualId;
        insuredId = data.Id;
        $("#InsuredSearch").val(data.DocumentNumber + " - " + data.InsuredName);
    }


    /**
        * Busca los intermdiarios por número de documento o nombre.
        *
        * @param {String} event - Buscar.
        * @param {Object} value - Número documento o nombre.
        */
    GetMediatorByNumberOrName(event, value) {
        $("#alert").UifAlert('hide');

        if ($("#IntermediarySearch").val() != "") {
            var controller = ACC_ROOT + "PolicyCancellation/GetIntermediaryByNumberOrName?numberName=" + value;
            $("#tableIntermediary").UifDataTable({ source: controller });

            $('#modalSearchIntermediary').UifModal('showLocal', Resources.SelectIntermediary);
        }
        else {
            $("#alert").UifAlert('show', Resources.PlaceHolderIntermediary, "warning");
        }
    }

    /**
        * Selecciona un intermediario de la tabla y cierra el modal de búsqueda.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Información del registro seleccionado.
        * @param {Number} position - Posición del registro seleccionado.
        */
    RowSelectedIntermediary(event, data, position) {
        $('#modalSearchIntermediary').UifModal('hide');
        intermediaryId = data.IndividualId;
        intermediaryCompanyId = data.Id;
        $("#IntermediarySearch").val(data.DocumentNumber + " - " + data.IntermediaryName);
    }

    /**
        * Busca los agrupadores por código o nombre.
        *
        * @param {String} event - Buscar.
        * @param {Object} value - Código o nombre.
        */
    GetGrouperByNumberOrName(event, value) {
        $("#alert").UifAlert('hide');

        if ($("#GrouperSearch").val() != "") {
            var controller = ACC_ROOT + "PolicyCancellation/GetGrouperByNumberOrName?numberName=" + value;
            $("#tableGroupers").UifDataTable({ source: controller });

            $('#modalSearchGrouper').UifModal('showLocal', Resources.SelectGrouper);
        }
        else {
            $("#alert").UifAlert('show', Resources.PlaceHolderGrouper, "warning");
        }
    }

    /**
        * Selecciona un agrupador de la tabla y cierra el modal de búsqueda.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Información del registro seleccionado.
        * @param {Number} position - Posición del registro seleccionado.
        */
    RowSelectedGrouper(event, data, position) {
        $('#modalSearchGrouper').UifModal('hide');
        grouperId = data.Id;
        $("#GrouperSearch").val(data.Id + " - " + data.Description)
    }

    /**
        * Valida que la fecha corte no sea mayor a fecha emisión del día.
        *
        * @param {String} event - Seleccionar.
        * @param {Date}   date  - Fecha seleccionada.
        */
    ValidateLaterIssueDate(event, date) {
        $("#alert").UifAlert('hide');

        if ($("#ViewBagIssueDate").val() != "") {

            if (compare_dates($('#PickerCutDate').val(), $("#ViewBagIssueDate").val())) {

                $("#alert").UifAlert('show', Resources.ValidateCutDate, "warning");

                //$("#PickerCutDate").val('');
                $("#PickerCutDate").UifDatepicker('setValue', $("#ViewBagIssueDate").val());
            }
            else {
                $("#PickerCutDate").val($('#PickerCutDate').val());
            }
        }
    }

    /**
        * Valida que la fecha emisión desde no sea mayor a fecha emisión hasta.
        *
        * @param {String} event - Seleccionar.
        * @param {Date}   date  - Fecha seleccionada.
        */
    ValidateDateFromNoGreaterThanDateTo(event, date) {
        $("#alert").UifAlert('hide');

        if ($("#PickerIssueDateTo").val() != "") {

            if (compare_dates($('#PickerIssueDateFrom').val(), $("#PickerIssueDateTo").val())) {

                $("#alert").UifAlert('show', Resources.ValidateIssueDateFrom, "warning");

                //$("#PickerIssueDateFrom").val($("#ViewBagIssueDate").val());
                $("#PickerIssueDateFrom").UifDatepicker('setValue', $("#ViewBagIssueDate").val());
            }
            else {
                //$("#PickerIssueDateFrom").val($('#PickerIssueDateFrom').val());
                if ($("#PickerCutDate").val() != "") {
                    if (compare_dates($('#PickerIssueDateFrom').val(), $("#PickerCutDate").val())) {

                        $("#alert").UifAlert('show', Resources.ValidateIssueDateFromCutDate, "warning");

                        //$("#PickerIssueDateFrom").val($("#ViewBagIssueDate").val());
                        $("#PickerIssueDateFrom").UifDatepicker('setValue', $("#ViewBagIssueDate").val());
                    }
                    else {
                        $("#PickerIssueDateFrom").val($('#PickerIssueDateFrom').val());
                    }
                }
            }
        }
    }

    /**
        * Valida que la fecha emisión hasta no sea menor a fecha emisión desde.
        *
        * @param {String} event - Seleccionar.
        * @param {Date}   date  - Fecha seleccionada.
        */
    ValidateDateToNoLessThanDateFrom(event, date) {
        $("#alert").UifAlert('hide');

        if ($("#PickerIssueDateFrom").val() != "") {
            if (compare_dates($("#PickerIssueDateFrom").val(), $('#PickerIssueDateTo').val())) {

                $("#alert").UifAlert('show', Resources.ValidateIssueDateTo, "warning");

                //$("#PickerIssueDateTo").val($("#ViewBagIssueDate").val());
                $("#PickerIssueDateTo").UifDatepicker('setValue', $("#ViewBagIssueDate").val());
            }
            else {
                //$("#PickerIssueDateTo").val($('#PickerIssueDateTo').val());
                if ($("#PickerCutDate").val() != "") {
                    if (compare_dates($("#PickerIssueDateTo").val(), $('#PickerCutDate').val())) {

                        $("#alert").UifAlert('show', Resources.ValidateIssueDateToCutDate, "warning");

                        //$("#PickerIssueDateTo").val($("#ViewBagIssueDate").val());
                        $("#PickerIssueDateTo").UifDatepicker('setValue', $("#ViewBagIssueDate").val());
                    }
                    else {
                        $("#PickerIssueDateTo").val($('#PickerIssueDateTo').val());
                    }
                }
            }
        }
    }

    /**
       * Generación proceso búsqueda de las pólizas a cancelar.
       *
       */
    GenerateSelectionPolicies() {
        $("#formCancellation").validate();

        if ($("#formCancellation").valid()) {
            if ($("#PolicyNumber").val() != "") {
                if (($("#SelectBranchAutomatic").val() == "") || ($("#SelectPrefix").val() == "")) {
                    $("#alert").UifAlert('show', Resources.SelectBranchPrefix, "danger");
                    return;
                }
            }

            operationType = "G";
            $("#saveMessageModal").text(Resources.MessagePolicySelection);
            $('#modalSave').appendTo("body").modal('show');
            return true;
        }
    }

    /**
       * Busca la cancelación automática de pólizas por número de proceso.
       *
       * @param {String} event - Buscar.
       * @param {Object} value - Número de proceso.
       */
    GetPolicyCancellationProcess(event, value) {
        $("#alertPolicyProcess").UifAlert('hide');

        MainPolicyAutomaticCancellation.HiddenButtons();

        $("#PolicyUserName").val("");
        $("#PolicyProcessDate").val("");
        $("#PolicyTotalRecords").val("");
        $("#PolicyTotalPremium").val("");
        $("#tablePolicies").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": value, "tab": "G" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#PolicyUserName").val(data[0].UserName);
                        $("#PolicyProcessDate").val(data[0].ProcessDate);
                        $("#PolicyTotalRecords").val(data[0].TotalRecords);
                        $("#PolicyTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            value + "&tab=G";
                        $("#tablePolicies").UifDataTable({ source: controller });
                        $("#PolicyCancel").show();
                        $("#PolicyExportExcel").show();
                        $("#PolicyImportExcel").show();
                        $("#PolicyProcessCancellation").show();

                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetTotalPremium();
                            $("#PolicyTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertPolicyProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }

    /**
        * Oculta los botones al seleccionar un tab.
        *
        * @param {String} event - Seleccionar tab.
        * @param {Object} newly - Nuevo tab seleccionado.
        * @param {Object} old   - Viejo tab seleccionado.
        */
    ChangeTabMain(event, newly, old) {
        MainPolicyAutomaticCancellation.HiddenButtons();

        if (newly.hash == "#tabPolicies" || old == "#tabPolicies") {
            var ids = $("#tablePolicies").UifDataTable("getData");

            $("#PolicyCancel").show();
            $("#PolicyExportExcel").show();
            $("#PolicyImportExcel").show();
            $("#PolicyProcessCancellation").show();

            if (ids.length > 0) {
                $("#PolicyCancel").show();
                $("#PolicyExportExcel").show();
                $("#PolicyImportExcel").show();
                $("#PolicyProcessCancellation").show();
            }
        }
        else if (newly.hash == "#tabCancellationsProcessed" || old == "#tabCancellationsProcessed") {

            var ids = $("#tableProcessed").UifDataTable("getData");
            $("#CancellationCancel").show();
            $("#CancellationExportExcel").show();
            $("#CancellationImportExcel").show();
            $("#ProcessCrossingCollections").show();

            if (ids.length > 0) {
                $("#CancellationCancel").show();
                $("#CancellationExportExcel").show();
                $("#CancellationImportExcel").show();
                $("#ProcessCrossingCollections").show();
            }
        }
        else if (newly.hash == "#tabErrorWhenCanceling" || old == "#tabErrorWhenCanceling") {

            var ids = $("#tableMistakes").UifDataTable("getData");
            $("#ErrorCancel").show();
            $("#ErrorExportExcel").show();
            $("#ErrorImportExcel").show();
            $("#ReprocessCancellation").show();

            if (ids.length > 0) {
                $("#ErrorCancel").show();
                $("#ErrorExportExcel").show();
                $("#ErrorImportExcel").show();
                $("#ReprocessCancellation").show();
            }
        }
        else if (newly.hash == "#tabCancellationsApplied" || old == "#tabCancellationsApplied") {

            var ids = $("#tableAppliedCancellation").UifDataTable("getData");
            $("#AppliedExportExcel").show();

            if (ids.length > 0) {
                $("#AppliedExportExcel").show();
            }
        }
    }

    /**
        * Limpia los campos de de consulta y llenado de pólizas a cancelar.
        *
        */
    CleanDataPolicy() {
        $("#PolicyProcessNumber").val("");
        $("#PolicyUserName").val("");
        $("#PolicyProcessDate").val("");
        $("#PolicyTotalRecords").val("");
        $("#PolicyTotalPremium").val("");

        $("#tablePolicies").UifDataTable("clear");
    }

    /**
        * Exporta la selección de pólizas a cancelar a un archivo excel.
        *
        */
    ExportToExcelDataPolicy() {
        var ids = $("#tablePolicies").UifDataTable("getData");

        if (ids.length > 0) {
            $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "PolicyAutomaticCancellation/GeneratePoliciesToExcel",
                data: {
                    "processNumber": $("#PolicyProcessNumber").val(),
                    "processDate": $("#PolicyProcessDate").val()
                },
            }).done(function (data) {
                // Get the file name for download
                if (data.fileName != "") {
                    // Use window.location.href for redirect to download action for download the file
                    window.location.href = ACC_ROOT + "PolicyCancellation/Download/?file=" + data.fileName;
                }
                $.msg('unblock');
            });
        }
    }

    /**
        * Abre el modal de selección del archivo a importar.
        *
        */
    OpenInputFileDataPolicy() {
        if ($("#PolicyProcessNumber").val() != "") {
            $('#modalPolicyImport').UifModal('showLocal', Resources.SelectFile);
        }
        else {
            $("#alertPolicyProcess").UifAlert('show', Resources.SelectProcess, "warning");
        }
    }

    /**
        * Carga el archivo en la tabla de selección de pólizas a cancelar.
        *
        */
    LoadDataPolicy() {
        $("#policyImportForm").validate();

        if ($("#policyImportForm").valid()) {

            $('#modalPolicyImport').UifModal('hide');
            MainPolicyAutomaticCancellation.UploadAjaxDataPolicy($("#PolicyProcessNumber").val(), $("#PolicyProcessDate").val());
        }
    }

    /**
       * Procesa la cancelación automática de pólizas.
       *
       */
    ProcessPolicyCancellation() {
        var ids = $("#tablePolicies").UifDataTable("getData");

        if (ids.length > 0) {

            operationType = "P";
            $("#saveMessageModal").text(Resources.MessageCancellationProcess);
            $('#modalSave').appendTo("body").modal('show');
            //$('#modalSave').UifModal('showLocal', Resources.MessageCancellationProcess);
            return true;
        }
        else {
            $("#alertPolicyProcess").UifAlert('show', Resources.SelectProcess, "warning");
        }
    }

    /**
       * Ejecuta el proceso de la cancelación automática de pólizas si el tipo de operación es:
       * G - Genera la búsqueda de pólizas a cencelar.
       * P - Procesa la cancelación en base a la selección de pólizas.
       * C - Procesa el cruce de cobranzas.
       * R - Reprocesa la cancelación en base a la selección de pólizas con error.
       */
    ExecuteCancellationProcess() {
        //$("#divLoading").show();
        $('#modalSave').modal('hide');
        lockScreen();

        // Generar la búsqueda
        if (operationType == "G") {
            branchId = ($('#SelectBranchAutomatic').val() != "") ? $('#SelectBranchAutomatic').val() : -1;
            salePointId = ($('#SelectSalePoint').val() != "") ? $('#SelectSalePoint').val() : -1;
            prefixId = ($('#SelectPrefix').val() != "") ? $('#SelectPrefix').val() : -1;
            policyNumber = ($('#PolicyNumber').val() != "") ? $('#PolicyNumber').val() : "-1";
            businessId = ($('#SelectBusiness').val() != "") ? $('#SelectBusiness').val() : -1;
            cancellationtypeId = ($('#SelectCancellationToBeMade').val() != "") ? $('#SelectCancellationToBeMade').val() : -1;


            $("#AutoCancellationGenerate").attr("disabled", "disabled");

            // Se obtiene el total de pólizas a cancelar
            $.ajax({
                url: ACC_ROOT + "PolicyCancellation/GetTotalRecords",
                data: {
                    "branchId": branchId,
                    "salePointId": salePointId,
                    "prefixId": prefixId,
                    "policyNumber": policyNumber,
                    "insuredId": insuredId,
                    "intermediaryId": intermediaryId,
                    "grouperId": grouperId,
                    "businessId": businessId,
                    "cutDate": $("#PickerCutDate").val(),
                    "issueDateFrom": $("#PickerIssueDateFrom").val(),
                    "issueDateTo": $("#PickerIssueDateTo").val(),
                    "cancellationtypeId": cancellationtypeId
                },
                success: function (data) {
                    if (data[0].TotalRecords > 0) {
                        $.ajax({
                            url: ACC_ROOT + "PolicyCancellation/GeneratePolicyAutomaticCancellation",
                            data: {
                                "branchId": branchId,
                                "salePointId": salePointId,
                                "prefixId": prefixId,
                                "policyNumber": policyNumber,
                                "insuredId": insuredId,
                                "intermediaryId": intermediaryId,
                                "grouperId": grouperId,
                                "businessId": businessId,
                                "cutDate": $("#PickerCutDate").val(),
                                "issueDateFrom": $("#PickerIssueDateFrom").val(),
                                "issueDateTo": $("#PickerIssueDateTo").val(),
                                "cancellationtypeId": cancellationtypeId
                            },
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data[0].ProcessNumber > 0) {
                                    $("#alert").UifAlert('show', Resources.MessageProcessAutomaticCancellation, "success");
                                    processType = 0;
                                    MainPolicyAutomaticCancellation.GetMassiveProcess();
                                    massiveTime = window.setInterval(MainPolicyAutomaticCancellation.RefreshMassiveProcess, 3000);
                                    processType = 1;
                                }
                                else {
                                    clearInterval(massiveTime); // detiene el time
                                    $("#alert").UifAlert('show', data[0].MessageError, "danger");
                                    $("#divLoading").hide();
                                }
                                $("#AutoCancellationGenerate").removeAttr("disabled");

                                setTimeout(function () {
                                    $("#alert").UifAlert('hide');
                                }, 5000);
                                //$("#divLoading").hide();
                                $.unblockUI();
                            }
                        });

                    } else {
                        $("#alert").UifAlert('show', Resources.MessageTotalPolicies, "info");
                        $("#AutoCancellationGenerate").removeAttr("disabled");
                        //$("#divLoading").hide();
                        $.unblockUI();
                    }
                },
            });

        }

        // Procesar la cancelación
        if (operationType == "P") {
            //$("#divLoading").show();

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "PolicyAutomaticCancellation/SaveCancellationPolicy",
                data: { "processNumber": $("#PolicyProcessNumber").val() },
                success: function (data) {

                    if (data.success) {
                        $("#alertPolicyProcess").UifAlert('show', Resources.MessageSuccessfullyPolicyCancellation, "success");
                        MainPolicyAutomaticCancellation.GetPolicyCancellationByProcess($("#PolicyProcessNumber").val());
                    }
                    else {
                        $("#alertPolicyProcess").UifAlert('show', Resources.ErrorTransaction, "danger", data.result);
                    }
                    //$("#divLoading").hide();
                    $.unblockUI();
                }
            });
        }

        // Cruce cobranzas
        if (operationType == "C") {
            //$("#divLoading").show();

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "PolicyAutomaticCancellation/SaveCollectionsCrossing",
                data: { "processNumber": $("#ProcessedProcessNumber").val() },
                success: function (data) {

                    if (data.success) {
                        $("#alertProcessedProcess").UifAlert('show', Resources.MessageSuccessfulCollectionCrossing, "success");
                        MainPolicyAutomaticCancellation.GetCancellationProcessedByProcess($("#ProcessedProcessNumber").val());
                    }
                    else {
                        $("#alertProcessedProcess").UifAlert('show', Resources.ErrorTransaction, "danger", data.result);
                    }
                    //$("#divLoading").hide();
                    $.unblockUI();
                }
            });
        }

        // Reprocesar la cancelación
        if (operationType == "R") {
            //$("#divLoading").show();

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "PolicyAutomaticCancellation/MoveCancellationPolicies",
                data: { "processNumber": $("#ErrorProcessNumber").val() },
                success: function (data) {

                    if (data.success) {
                        $("#alertMistakeProcess").UifAlert('show', Resources.MessageSuccessfullyMovePolicyCancellation, "success");
                        MainPolicyAutomaticCancellation.GetMistakeProcessByProcess($("#ErrorProcessNumber").val());
                    }
                    else {
                        $("#alertMistakeProcess").UifAlert('show', Resources.ErrorTransaction, "danger", data.result);
                    }
                    //$("#divLoading").hide();
                    $.unblockUI();
                }
            });
        }

    }

    /**
       * Busca la cancelación automática de pólizas por número de proceso.
       *
       * @param {String} event - Buscar.
       * @param {Object} value - Número de proceso.
       */
    GetCancellationProcessed(event, value) {
        $("#alertProcessedProcess").UifAlert('hide');

        //MainPolicyAutomaticCancellation.HiddenButtons();

        $("#ProcessedUserName").val("");
        $("#ProcessedProcessDate").val("");
        $("#ProcessedTotalRecords").val("");
        $("#ProcessedTotalPremium").val("");
        $("#tableProcessed").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": value, "tab": "P" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#ProcessedUserName").val(data[0].UserName);
                        $("#ProcessedProcessDate").val(data[0].ProcessDate);
                        $("#ProcessedTotalRecords").val(data[0].TotalRecords);
                        $("#ProcessedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            value + "&tab=P";
                        $("#tableProcessed").UifDataTable({ source: controller });
                        $("#CancellationCancel").show();
                        $("#CancellationExportExcel").show();
                        $("#CancellationImportExcel").show();
                        $("#ProcessCrossingCollections").show();

                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetProcessedTotalPremium();
                            $("#ProcessedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertProcessedProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }

    /**
        * Limpia los campos de la consulta y llenado de cancelaciones procesadas.
        *
        */
    CleanDataCancellaton() {
        $("#ProcessedProcessNumber").val("");
        $("#ProcessedUserName").val("");
        $("#ProcessedProcessDate").val("");
        $("#ProcessedTotalRecords").val("");
        $("#ProcessedTotalPremium").val("");

        $("#tableProcessed").UifDataTable("clear");
    }

    /**
        * Exporta las cancelaciones procesadas a un archivo excel.
        *
        */
    ExportToExcelDataCancellation() {
        var ids = $("#tableProcessed").UifDataTable("getData");

        if (ids.length > 0) {
            var process = $('#tableProcessed').UifDataTable('getData');
            var premium = 0;

            for (var i in ids) {
                if (ids[i].ApplicationTemporaryId == "") {
                    ++premium;
                }
            }

            if (premium > 0) {
                $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "PolicyCancellation/GenerateCancellationsToExcel",
                    data: {
                        "processNumber": $("#ProcessedProcessNumber").val(),
                        "processDate": $("#ProcessedProcessDate").val()
                    },
                }).done(function (data) {
                    // Get the file name for download
                    if (data.fileName != "") {
                        // Use window.location.href for redirect to download action for download the file
                        window.location.href = ACC_ROOT + "PolicyCancellation/Download/?file=" + data.fileName;
                    }
                    $.msg('unblock');
                });
            }
            else {
                $("#alertProcessedProcess").UifAlert('show', Resources.MessageExportProcessedCancellation, "info");
            }

        }
    }

    /**
        * Abre el modal de selección del archivo a importar.
        *
        */
    OpenInputFileDataCancellation() {
        if ($("#ProcessedProcessNumber").val() != "") {
            $('#modalCancellationImport').UifModal('showLocal', Resources.SelectFile);
        }
        else {
            $("#alertProcessedProcess").UifAlert('show', Resources.SelectProcess, "warning");
        }
    }

    /**
        * Carga el archivo en la tabla de cancelaciones procesadas.
        *
        */
    LoadDataCancellation() {
        $("#cancellationImportForm").validate();

        if ($("#cancellationImportForm").valid()) {

            $('#modalCancellationImport').UifModal('hide');
            MainPolicyAutomaticCancellation.UploadAjaxDataCancellation($("#ProcessedProcessNumber").val(), $("#ProcessedProcessDate").val());
        }
    }

    /**
       * Procesa el cruce de cobranzas de la cancelación automática de pólizas.
       *
       */
    ProcessCrossingCollections() {
        var ids = $("#tableProcessed").UifDataTable("getData");

        if (ids.length > 0) {

            operationType = "C";
            $("#saveMessageModal").text(Resources.MessageCollectionCrossing);
            $('#modalSave').appendTo("body").modal('show');
            return true;
        }
        else {
            $("#alertProcessedProcess").UifAlert('show', Resources.SelectProcess, "warning");
        }
    }

    /**
       * Busca los errores de cancelación automática de pólizas por número de proceso.
       *
       * @param {String} event - Buscar.
       * @param {Object} value - Número de proceso.
       */
    GetMistakeProcess(event, value) {
        $("#alertMistakeProcess").UifAlert('hide');

        //MainPolicyAutomaticCancellation.HiddenButtons();

        $("#MistakeUserName").val("");
        $("#MistakeProcessDate").val("");
        $("#MistakeTotalRecords").val("");
        $("#MistakeTotalPremium").val("");
        $("#tableMistakes").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": value, "tab": "R" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#MistakeUserName").val(data[0].UserName);
                        $("#MistakeProcessDate").val(data[0].ProcessDate);
                        $("#MistakeTotalRecords").val(data[0].TotalRecords);
                        $("#MistakeTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            value + "&tab=R";
                        $("#tableMistakes").UifDataTable({ source: controller });
                        $("#ErrorCancel").show();
                        $("#ErrorExportExcel").show();
                        $("#ErrorImportExcel").show();
                        $("#ReprocessCancellation").show();
                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetMistakeTotalPremium();
                            $("#MistakeTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertMistakeProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }

    /**
        * Limpia los campos de la consulta y llenado de errores en cancelaciones.
        *
        */
    CleanDataMistake() {
        $("#ErrorProcessNumber").val("");
        $("#MistakeUserName").val("");
        $("#MistakeProcessDate").val("");
        $("#MistakeTotalRecords").val("");
        $("#MistakeTotalPremium").val("");

        $("#tableMistakes").UifDataTable("clear");
    }

    /**
        * Exporta los errores de cancelaciones a un archivo excel.
        *
        */
    ExportToExcelDataMistake() {
        var ids = $("#tableMistakes").UifDataTable("getData");

        if (ids.length > 0) {
            $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "PolicyCancellation/GenerateMistakesToExcel",
                data: {
                    "processNumber": $("#ErrorProcessNumber").val(),
                    "processDate": $("#MistakeProcessDate").val()
                },
            }).done(function (data) {
                // Get the file name for download
                if (data.fileName != "") {
                    // Use window.location.href for redirect to download action for download the file
                    window.location.href = ACC_ROOT + "PolicyCancellation/Download/?file=" + data.fileName;
                }
                $.msg('unblock');
            });
        }
    }

    /**
        * Abre el modal de selección del archivo a importar.
        *
        */
    OpenInputFileDataMistake() {
        if ($("#ErrorProcessNumber").val() != "") {
            $('#modalMistakeImport').UifModal('showLocal', Resources.SelectFile);
        }
        else {
            $("#alertMistakeProcess").UifAlert('show', Resources.SelectProcess, "warning");
        }
    }

    /**
        * Carga el archivo en la tabla de errorese en cancelaciones procesadas.
        *
        */
    LoadDataMistake() {
        $("#mistakeImportForm").validate();

        if ($("#mistakeImportForm").valid()) {

            $('#modalMistakeImport').UifModal('hide');
            MainPolicyAutomaticCancellation.UploadAjaxDataMistake($("#ErrorProcessNumber").val(), $("#MistakeProcessDate").val());
        }
    }

    /**
       * Mueve los registros con error a la pestaña de selección pólizas a cancelar para luego
       * reprocesar la cancelación automática de pólizas.
       *
       */
    ReprocessCancellation() {
        var ids = $("#tableMistakes").UifDataTable("getData");

        if (ids.length > 0) {

            operationType = "R";
            $("#saveMessageModal").text(Resources.MessageCancellationMistake);
            $('#modalSave').appendTo("body").modal('show');
            return true;
        }
        else {
            $("#alertMistakeProcess").UifAlert('show', Resources.SelectProcess, "warning");
        }
    }

    /**
       * Busca las cancelaciones aplicadas por número de proceso.
       *
       * @param {String} event - Buscar.
       * @param {Object} value - Número de proceso.
       */
    GetAppliedCancellations(event, value) {
        $("#alertAppliedProcess").UifAlert('hide');

        //MainPolicyAutomaticCancellation.HiddenButtons();

        $("#AppliedUserName").val("");
        $("#AppliedProcessDate").val("");
        $("#AppliedTotalRecords").val("");
        $("#AppliedTotalPremium").val("");
        $("#tableAppliedCancellation").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": value, "tab": "A" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#AppliedUserName").val(data[0].UserName);
                        $("#AppliedProcessDate").val(data[0].ProcessDate);
                        $("#AppliedTotalRecords").val(data[0].TotalRecords);
                        $("#AppliedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            value + "&tab=A";
                        $("#tableAppliedCancellation").UifDataTable({ source: controller });
                        $("#AppliedExportExcel").show();

                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetAppliedTotalPremium();
                            $("#AppliedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertAppliedProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }

    /**
        * Exporta las cancelaciones aplicadas a un archivo excel.
        *
        */
    ExportToExcelDataApplied() {
        var ids = $("#tableAppliedCancellation").UifDataTable("getData");

        if (ids.length > 0) {
            $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "PolicyCancellation/GenerateAppliedCancellationsToExcel",
                data: {
                    "processNumber": $("#AppliedProcessNumber").val(),
                    "processDate": $("#AppliedProcessDate").val()
                },
            }).done(function (data) {
                // Get the file name for download
                if (data.fileName != "") {
                    // Use window.location.href for redirect to download action for download the file
                    window.location.href = ACC_ROOT + "PolicyCancellation/Download/?file=" + data.fileName;
                }
                $.msg('unblock');
            });
        }
    }


    /*-------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                     DEFINICIÓN DE FUNCIONES                                                   */
    /*-------------------------------------------------------------------------------------------------------------------------------*/

    /**
    * Obtiene los procesos de cancelación de pólizas generados por usuario.
    *
    */
    static GetMassiveProcess() {
        var controller = ACC_ROOT + "PolicyCancellation/GetPendingProcesses";
        $("#tablePendingProcesses").UifDataTable({ source: controller });
    };

    /**
    * Refresca la tabla de procesos de cancelación de pólizas.
    *
    */
    static RefreshMassiveProcess() {
        var validateProcess = true;
        var process = $('#tablePendingProcesses').UifDataTable('getData');

        if (processType == 0) {
            MainPolicyAutomaticCancellation.GetMassiveProcess();
        }
        else {
            // Se valida que no se esten ejecutando procesos
            if (process.length == 0) {
                MainPolicyAutomaticCancellation.GetMassiveProcess();
            }
            else {
                if (MainPolicyAutomaticCancellation.ValidateMassiveProcess() == true) {
                    clearInterval(massiveTime);
                    $("#divLoading").hide();
                }
                else {
                    MainPolicyAutomaticCancellation.GetMassiveProcess();
                }
            }
        }
    }

    /**
    * Valida si el proceso ha finalizado.
    *
    */
    static ValidateMassiveProcess() {
        var process = $('#tablePendingProcesses').UifDataTable('getData');
        var count = 0;

        for (var i in process) {
            if (process[i].Finalized == "T") {
                count = 0
            }
            else {
                count++;
                return;
            }
        }

        if (count > 0) {
            return false;
        }
        else {
            return true;
        }
    }

    /**
       * Da formato a un número para su visualización.
       *
       * @param {Number} number             - Valor a formatear.
       * @param {Number} decimals           - Número de decimales.
       * @param {String} decimalSeparator   - Coma o punto decimal.
       * @param {Object} thousandsSeparator - Separador de miles.
       */
    static NumberFormat(number, decimals, decimalSeparator, thousandsSeparator) {
        var parts, array;

        if (!isFinite(number) || isNaN(number = parseFloat(number))) {
            return "";
        }
        if (typeof decimalSeparator === "undefined") {
            decimalSeparator = ",";
        }
        if (typeof thousandsSeparator === "undefined") {
            thousandsSeparator = "";
        }

        // Redondeamos
        if (!isNaN(parseInt(decimals))) {
            if (decimals >= 0) {
                number = number.toFixed(decimals);
            }
            else {
                number = (
                    Math.round(number / Math.pow(10, Math.abs(decimals))) * Math.pow(10, Math.abs(decimals))
                ).toFixed();
            }
        }
        else {
            number = number.toString();
        }

        // Damos formato
        parts = number.split(".", 2);
        array = parts[0].split("");
        for (var i = array.length - 3; i > 0 && array[i - 1] !== "-"; i -= 3) {
            array.splice(i, 0, thousandsSeparator);
        }
        number = array.join("");

        if (parts.length > 1) {
            number += decimalSeparator + parts[1];
        }

        return number;
    }

    /**
       * Oculta los botones de cada pestaña de la pantalla.
       *
       */
    static HiddenButtons() {

        $("#PolicyCancel").hide();
        $("#PolicyExportExcel").hide();
        $("#PolicyImportExcel").hide();
        $("#PolicyProcessCancellation").hide();

        $("#CancellationCancel").hide();
        $("#CancellationExportExcel").hide();
        $("#CancellationImportExcel").hide();
        $("#ProcessCrossingCollections").hide();

        $("#ErrorCancel").hide();
        $("#ErrorExportExcel").hide();
        $("#ErrorImportExcel").hide();
        $("#ReprocessCancellation").hide();

        $("#AppliedExportExcel").hide();
    }

    /**
       * Carga el archivo a la tabla selección de pólizas a cancelar.
       *
       * @param {Number} processNumber - Número proceso cancelación automática de pólizas.
       * @param {String} processDate   - Fecha y hora de proceso cancelación automática de pólizas.
       */
    static UploadAjaxDataPolicy(processNumber, processDate) {
        var inputPolicyFile = document.getElementById("LoadFilePolicy");
        var file = inputPolicyFile.files[0];
        if (file == undefined) {
            $("#alertPolicyProcess").UifAlert('show', Resources.SelectFile, "danger");
            setTimeout(function () {
                $("#alertPolicyProcess").hide();
            }, 3000);
        }
        else {
            //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });
            $("#divLoading").show();
            var data = new FormData();
            data.append('uploadedFile', file);
            data.append('processNumber', processNumber);
            data.append('processDate', processDate);

            var url = ACC_ROOT + "PolicyCancellation/ReadPolicyFileInMemory";
            var validationPromise = new Promise(function (resolve, reject) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    contentType: false,
                    data: data,
                    processData: false,
                    cache: false,
                    success: function (data) {
                        resolve(data);
                    }
                });
            });
            validationPromise.then(function (data) {
                if (data.length > 0) {
                    if (data[1]) {
                        $("#alertPolicyProcess").UifAlert('show', Resources.SelectPoliciesSucessful, "success");
                        MainPolicyAutomaticCancellation.GetPolicyCancellationByProcess($("#PolicyProcessNumber").val());
                    }
                    else {
                        if (data[0] == "NoProcessNumber") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.WrongPolicySelection + " - " + Resources.ProcessNumber, "danger");
                        }
                        else if (data[0] == "NoProcessDate") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.WrongPolicySelection + " - " + Resources.ProcessDate, "danger");
                        }
                        else if (data[0] == "NoCorrespondColumns") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.WrongColumnsPolicySelection, "danger");
                        }
                        else if (data[0] == "BadFileExtension") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.WrongFormatBlankColumns, "danger");
                        }
                        else if (data[0] == "NegativeId") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.IdsNoNegative, "danger");
                        }
                        else if (data[0] == "NotParameterizedFormat") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.WrongNotParametrizedDesignFormat, "danger");
                        }
                        else if (data[0] == "NotAllowedPreNotification") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.WrongNotAllowedPreNotification, "danger");
                        }
                        else if (data[0] == "SuccessfulLoadBankResponse") {
                            //BankResponseProcess();
                        }
                        else if (data[0] == "ErrorLoadBankResponse") {
                            $("#alertPolicyProcess").UifAlert('show', Resources.WrongLoadBankResponseMistakes, "danger");
                        }
                        else {
                            //$("#alertPolicyProcess").UifAlert('show', Resources.DocumentErrors, "danger");
                        }
                    }

                    setTimeout(function () {
                        //$("#alertPolicyProcess").hide();
                    }, 3000);
                    $("#divLoading").hide();
                }
                if (data.length == 0) {
                    $("#alertPolicyProcess").UifAlert('show', Resources.SaveSuccessfully, "success");
                    setTimeout(function () {
                        $("#alertPolicyProcess").hide();
                    }, 3000);
                }
            });
        }
    }

    /**
        * Carga el archivo a la tabla cancelaciones procesadas.
        *
        * @param {Number} processNumber - Número proceso cancelación automática de pólizas.
        * @param {String} processDate   - Fecha proceso cancelación automática de pólizas.
        */
    static UploadAjaxDataCancellation(processNumber, processDate) {
        var inputPolicyFile = document.getElementById("LoadFileCancellation");
        var file = inputPolicyFile.files[0];
        if (file == undefined) {
            $("#alertProcessedProcess").UifAlert('show', Resources.SelectFile, "danger");
            setTimeout(function () {
                $("#alertProcessedProcess").hide();
            }, 3000);
        }
        else {
            $("#divLoading").show();
            var data = new FormData();
            data.append('uploadedFile', file);
            data.append('processNumber', processNumber);
            data.append('processDate', processDate);

            var url = ACC_ROOT + "PolicyCancellation/ReadCancellationFileInMemory";
            var validationPromise = new Promise(function (resolve, reject) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    contentType: false,
                    data: data,
                    processData: false,
                    cache: false,
                    success: function (data) {
                        resolve(data);
                    }
                });
            });
            validationPromise.then(function (data) {
                if (data.length > 0) {
                    if (data[1]) {
                        $("#alertProcessedProcess").UifAlert('show', Resources.AppliesCollectionSucessful, "success");
                        MainPolicyAutomaticCancellation.GetCancellationProcessedByProcess($("#ProcessedProcessNumber").val());
                    }
                    else {
                        if (data[0] == "NoProcessNumber") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.WrongPolicyCancellation + " - " + Resources.ProcessNumber, "danger");
                        }
                        else if (data[0] == "NoProcessDate") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.WrongPolicyCancellation + " - " + Resources.ProcessDate, "danger");
                        }
                        else if (data[0] == "NoCorrespondColumns") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.WrongColumnsPolicyCancellation, "danger");
                        }
                        if (data[0] == "BadFileExtension") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.WrongFormatBlankColumns, "danger");
                        }
                        else if (data[0] == "NegativeId") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.IdsNoNegative, "danger");
                        }
                        else if (data[0] == "NotParameterizedFormat") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.WrongNotParametrizedDesignFormat, "danger");
                        }
                        else if (data[0] == "NotAllowedPreNotification") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.WrongNotAllowedPreNotification, "danger");
                        }
                        else if (data[0] == "SuccessfulLoadBankResponse") {
                            //BankResponseProcess();
                        }
                        else if (data[0] == "ErrorLoadBankResponse") {
                            $("#alertProcessedProcess").UifAlert('show', Resources.WrongLoadBankResponseMistakes, "danger");
                        }
                        else {
                            //$("#alertProcessedProcess").UifAlert('show', Resources.DocumentErrors, "danger");
                        }
                    }

                    setTimeout(function () {
                        //$("#alertProcessedProcess").hide();
                    }, 3000);
                    $("#divLoading").hide();
                }
                if (data.length == 0) {
                    $("#alertProcessedProcess").UifAlert('show', Resources.SaveSuccessfully, "success");
                    setTimeout(function () {
                        $("#alertProcessedProcess").hide();
                    }, 3000);
                }
            });
        }
    }

    /**
        * Carga el archivo a la tabla de errores en cancelaciones procesadas.
        *
        * @param {Number} processNumber - Número proceso cancelación automática de pólizas.
        * @param {String} processDate   - Fecha proceso cancelación automática de pólizas.
        */
    static UploadAjaxDataMistake(processNumber, processDate) {
        var inputPolicyFile = document.getElementById("LoadFileMistake");
        var file = inputPolicyFile.files[0];
        if (file == undefined) {
            $("#alertMistakeProcess").UifAlert('show', Resources.SelectFile, "danger");
            setTimeout(function () {
                $("#alertMistakeProcess").hide();
            }, 3000);
        }
        else {
            $("#divLoading").show();
            var data = new FormData();
            data.append('uploadedFile', file);
            data.append('processNumber', processNumber);
            data.append('processDate', processDate);

            var url = ACC_ROOT + "PolicyCancellation/ReadMistakeFileInMemory";
            var validationPromise = new Promise(function (resolve, reject) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    contentType: false,
                    data: data,
                    processData: false,
                    cache: false,
                    success: function (data) {
                        resolve(data);
                    }
                });
            });
            validationPromise.then(function (data) {
                if (data.length > 0) {
                    if (data[1]) {
                        $("#alertMistakeProcess").UifAlert('show', Resources.ReprocessSucessful, "success");
                        MainPolicyAutomaticCancellation.GetMistakeProcessByProcess($("#ErrorProcessNumber").val());
                    }
                    else {
                        if (data[0] == "NoProcessNumber") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.WrongMistakeCancellation + " - " + Resources.ProcessNumber, "danger");
                        }
                        else if (data[0] == "NoProcessDate") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.WrongMistakeCancellation + " - " + Resources.ProcessDate, "danger");
                        }
                        else if (data[0] == "NoCorrespondColumns") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.WrongColumnsMistakeCancellation, "danger");
                        }
                        if (data[0] == "BadFileExtension") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.WrongFormatBlankColumns, "danger");
                        }
                        else if (data[0] == "NegativeId") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.IdsNoNegative, "danger");
                        }
                        else if (data[0] == "NotParameterizedFormat") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.WrongNotParametrizedDesignFormat, "danger");
                        }
                        else if (data[0] == "NotAllowedPreNotification") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.WrongNotAllowedPreNotification, "danger");
                        }
                        else if (data[0] == "SuccessfulLoadBankResponse") {
                            //BankResponseProcess();
                        }
                        else if (data[0] == "ErrorLoadBankResponse") {
                            $("#alertMistakeProcess").UifAlert('show', Resources.WrongLoadBankResponseMistakes, "danger");
                            //LoadBankExportReportToExcel();
                        }
                        else {
                            //$("#alertMistakeProcess").UifAlert('show', Resources.DocumentErrors, "danger");
                        }
                    }
                    setTimeout(function () {
                        //$("#alertMistakeProcess").hide();
                    }, 3000);
                    $("#divLoading").hide();
                }
                if (data.length == 0) {
                    $("#alertMistakeProcess").UifAlert('show', Resources.SaveSuccessfully, "success");
                    setTimeout(function () {
                        $("#alertMistakeProcess").hide();
                    }, 3000);
                }
            });
        }
    }

    /**
    * Obtiene el total de importe de primas.
    *
    */
    static GetTotalPremium() {
        var process = $('#tablePolicies').UifDataTable('getData');
        var premium = 0;

        for (var i in process) {
            if (process[i].Annul == "Si" && process[i].Processed == "No") {
                premium += process[i].Amount;
            }
        }

        return premium;
    }

    /**
       * Busca la cancelación automática de pólizas por número de proceso.
       *
       * @param {Number} process - Número de proceso.
       */
    static GetPolicyCancellationByProcess(process) {
        //$("#alertPolicyProcess").UifAlert('hide');

        MainPolicyAutomaticCancellation.HiddenButtons();

        $("#PolicyUserName").val("");
        $("#PolicyProcessDate").val("");
        $("#PolicyTotalRecords").val("");
        $("#PolicyTotalPremium").val("");
        $("#tablePolicies").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": process, "tab": "G" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#PolicyUserName").val(data[0].UserName);
                        $("#PolicyProcessDate").val(data[0].ProcessDate);
                        $("#PolicyTotalRecords").val(data[0].TotalRecords);
                        $("#PolicyTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyAutomaticCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            process + "&tab=G";
                        $("#tablePolicies").UifDataTable({ source: controller });
                        $("#PolicyCancel").show();
                        $("#PolicyExportExcel").show();
                        $("#PolicyImportExcel").show();
                        $("#PolicyProcessCancellation").show();

                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetTotalPremium();
                            $("#PolicyTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertPolicyProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }

    /**
    * Obtiene el total de importe de primas de las cancelaciones procesadas.
    *
    */
    static GetProcessedTotalPremium() {
        var process = $('#tableProcessed').UifDataTable('getData');
        var premium = 0;

        for (var i in process) {
            if (process[i].AppliesCollection == "Si") {
                premium += process[i].Amount;
            }
        }

        return premium;
    }

    /**
       * Busca la cancelación automática de pólizas por número de proceso.
       *
       * @param {Number} process - Número de proceso.
       */
    static GetCancellationProcessedByProcess(process) {
        //$("#alertProcessedProcess").UifAlert('hide');

        $("#ProcessedUserName").val("");
        $("#ProcessedProcessDate").val("");
        $("#ProcessedTotalRecords").val("");
        $("#ProcessedTotalPremium").val("");
        $("#tableProcessed").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": process, "tab": "P" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#ProcessedUserName").val(data[0].UserName);
                        $("#ProcessedProcessDate").val(data[0].ProcessDate);
                        $("#ProcessedTotalRecords").val(data[0].TotalRecords);
                        $("#ProcessedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            process + "&tab=P";
                        $("#tableProcessed").UifDataTable({ source: controller });
                        $("#CancellationCancel").show();
                        $("#CancellationExportExcel").show();
                        $("#CancellationImportExcel").show();
                        $("#ProcessCrossingCollections").show();

                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetProcessedTotalPremium();
                            $("#ProcessedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertProcessedProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }

    /**
    * Obtiene el total de importe de primas de las cancelaciones erróneas.
    *
    */
    static GetMistakeTotalPremium() {
        var process = $('#tableMistakes').UifDataTable('getData');
        var premium = 0;

        for (var i in process) {
            if (process[i].Reprocess == "Si") {
                premium += process[i].Amount;
            }
        }

        return premium;
    }

    /**
       * Busca los errores de cancelación automática de pólizas por número de proceso.
       *
       * @param {Number} process - Número de proceso.
       */
    static GetMistakeProcessByProcess(process) {
        //$("#alertMistakeProcess").UifAlert('hide');

        $("#MistakeUserName").val("");
        $("#MistakeProcessDate").val("");
        $("#MistakeTotalRecords").val("");
        $("#MistakeTotalPremium").val("");
        $("#tableMistakes").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": process, "tab": "R" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#MistakeUserName").val(data[0].UserName);
                        $("#MistakeProcessDate").val(data[0].ProcessDate);
                        $("#MistakeTotalRecords").val(data[0].TotalRecords);
                        $("#MistakeTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            process + "&tab=R";
                        $("#tableMistakes").UifDataTable({ source: controller });
                        $("#ErrorCancel").show();
                        $("#ErrorExportExcel").show();
                        $("#ErrorImportExcel").show();
                        $("#ReprocessCancellation").show();
                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetMistakeTotalPremium();
                            $("#MistakeTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertMistakeProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }


    /**
      * Obtiene el total de importe de primas de las cancelaciones aplicadas.
      *
      */
    static GetAppliedTotalPremium() {
        var process = $('#tableAppliedCancellation').UifDataTable('getData');
        var premium = 0;

        for (var i in process) {
            premium += process[i].Amount;
        }

        return premium;
    }

    /**
       * Busca la cancelación automática de pólizas por número de proceso.
       *
       * @param {Number} process - Número de proceso.
       */
    static GetCancellationProcessedByProcessNumber(processNumber) {
        //$("#alertProcessedProcess").UifAlert('hide');

        $("#ProcessedUserName").val("");
        $("#ProcessedProcessDate").val("");
        $("#ProcessedTotalRecords").val("");
        $("#ProcessedTotalPremium").val("");
        $("#tableProcessed").dataTable().fnClearTable();

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "PolicyCancellation/GetCancellationHeaderByProcessNumber",
            data: { "processNumber": processNumber, "tab": "P" },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.length > 0) {

                    if (data[0].UserName != "-1") {
                        $("#ProcessedUserName").val(data[0].UserName);
                        $("#ProcessedProcessDate").val(data[0].ProcessDate);
                        $("#ProcessedTotalRecords").val(data[0].TotalRecords);
                        $("#ProcessedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(data[0].TotalPremium, "2", ".", ","));

                        var controller = ACC_ROOT + "PolicyCancellation/GetCancellationDetailByProcessNumber?processNumber=" +
                            process + "&tab=P";
                        $("#tableProcessed").UifDataTable({ source: controller });
                        $("#CancellationCancel").show();
                        $("#CancellationExportExcel").show();
                        $("#CancellationImportExcel").show();
                        $("#ProcessCrossingCollections").show();

                        setTimeout(function () {
                            var totalPremium = MainPolicyAutomaticCancellation.GetProcessedTotalPremium();
                            $("#ProcessedTotalPremium").val("$ " + MainPolicyAutomaticCancellation.NumberFormat(totalPremium, "2", ".", ","));
                        }, 3000);
                    }
                    else {
                        $("#alertProcessedProcess").UifAlert('show', Resources.MessageAutomaticCreditNotesNotFound, "danger");
                    }
                }
            }
        });
    }
}