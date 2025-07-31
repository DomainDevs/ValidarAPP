var index = 0;
var individualId = -1;
var individualName = '';
var payToValue = "";
var chagePayment = true;
var documentNumber = "";
var journalPayerIndividualId = 0;
var personCode = 0;
var agentTypeId = -1;
var viewBagBranchDefault = $('#ViewBagBranchDefault').val();

var timeReportTrl = 0;
var isRunningReportTrl
var processTableRunningReportTrl;

$(() => {
    new MainTaxRetentionList();
});

$(window).load(function () {
    //Eventos de Autocomplete
    loadAutocompleteEventsTrl('SearchSuppliers');
    loadAutocompleteEventsTrl('SearchEmployee');
});


class MainTaxRetentionList extends Uif2.Page {
    getInitialState() {
        MainTaxRetentionList.cleanAutocompleteTrl('SearchSuppliers');
        MainTaxRetentionList.cleanAutocompleteTrl('SearchEmployee');

        timeReportTrl = window.setInterval(MainTaxRetentionList.getMassiveReportsTrl, 6000);
        MainTaxRetentionList.getMassiveReportsTrl();
    }

    bindEvents() {
        $("#TRLBeneficiary").on("itemSelected", this.BeneficiaryEvents);
        $("#TRLeportGenerate").on("click", this.TaxRetentionList);
        $("#TRLReportClean").on("click", this.CleanEventsTrl);
        $("#TRLReportModal").on("click", this.ReportModalEventTrl);
        $("#tableMassiveProcessReport").on("rowEdit", this.whenSelectRowTrl);
    }

    BeneficiaryEvents(event, selectedItem) {
        individualId = 0;
        individualName = "";
        chagePayment = true;
        var enableAjax = 0;

        if (chagePayment) {
            payToValue = selectedItem.Id;
            MainTaxRetentionList.cleanAutocompleteTrl('SearchSuppliers');
            MainTaxRetentionList.cleanAutocompleteTrl('SearchEmployee');

            if (selectedItem.Id != "") {
                $("#BeneficiaryDocumentNumberData").hide();
                $("#BeneficiaryNameData").hide();

                if (selectedItem.Id == $("#ViewBagSupplierCodeMainOther").val()) { // 1 Proveedor
                    $('#SearchSuppliersByDocumentNumberRequest').parent().parent().show();
                    $("#SearchSuppliersByNameRequest").parent().parent().show();

                } else if (selectedItem.Id == $("#ViewBagEmployeeCodMainOther").val()) { // 11 Empleado
                    $("#SearchEmployeeByDocumentNumberRequest").parent().parent().show();
                    $("#SearchEmployeeByNameRequest").parent().parent().show();

                } else if (selectedItem.Id == '') {
                    MainTaxRetentionList.cleanAutocompleteTrl('SearchSuppliers');
                    MainTaxRetentionList.cleanAutocompleteTrl('SearchEmployee');

                } else {
                    $("#BeneficiaryDocumentNumberData").show();
                    $("#BeneficiaryNameData").show();

                    MainTaxRetentionList.cleanAutocompleteTrl('SearchSuppliers');
                    MainTaxRetentionList.cleanAutocompleteTrl('SearchEmployee');
                }
            } else {
                $("#BeneficiaryDocumentNumberData").show();
                $("#BeneficiaryNameData").show();
                MainTaxRetentionList.cleanAutocompleteTrl('SearchSuppliers');
                MainTaxRetentionList.cleanAutocompleteTrl('SearchEmployee');
            }
        }
    }

    CleanEventsTrl() {
        $("#TRLReportBranch").UifSelect("setSelected", viewBagBranchDefault);
        $('#TRLReportCurrency').val('');
        $('#TRLPaymentOrder').val('');
        $('#TRLBeneficiary').val('');
        $('#BeneficiaryDocumentNumber').val('');
        $('#SearchSuppliersByDocumentNumberRequest').val('');
        $('#SearchEmployeeByDocumentNumberRequest').val('');
        $('#BeneficiaryName').val('');
        $('#SearchSuppliersByNameRequest').val('');
        $('#SearchEmployeeByNameRequest').val('');
        $('#AccountingNatureCd').val('');
    }

    ///////////////////////////////////////////////////////////
    // SI CONFIRMA LA EJECUCION DEL MODAL                    //
    ///////////////////////////////////////////////////////////
    ReportModalEventTrl() {
        $('#modalMassiveTotRecords').modal('hide');

        processTableRunningReportTrl = undefined;
        clearInterval(timeReportTrl);

        $.ajax({
            url: ACC_ROOT + "Reports/TaxRetentionReports",
            data: {
                "branchId": $("#TRLReportBranch").val(), "currencyId": $("#TRLReportCurrency").val(),
                "paymentOrder": $("#TRLPaymentOrder").val(), "beneficiaryTypeId": $("#TRLBeneficiary").val(),
                "individualId": individualId, "taxId": $("#TRLTaxes").val()
            },
            success: function (data) {
                if (data.success && data.result == 0) {
                    MainTaxRetentionList.getMassiveReportsTrl();

                    if (!isRunningReportTrl) {
                        timeReportTrl = window.setInterval(MainTaxRetentionList.getMassiveReportsTrl, 6000);
                    }
                } else {
                    $("#alertFormTrl").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
                }
            }
        });
    }

    ///////////////////////////////////////////////////////////
    // RECUPERA EL TOTAL DE REGISTROS A PROCESAR //
    ///////////////////////////////////////////////////////////
    TaxRetentionList() {
        $("#alertFormTrl").UifAlert('hide');

        $("#TRLReportForm").validate();
        if ($("#TRLReportForm").valid()) {

            lockScreen();

            setTimeout(function () {
                MainTaxRetentionList.getTotalRecordTrl();
            }, 300);
            
        }
    }

    /**
   * @param {Object} event - evento de edición en la tabla.
   * @param {Object} data - datos de la fila de la tabla.
   * @param {Number} position - número de fila seleccionada.
   */
    whenSelectRowTrl(event, data, position) {
        $("#alertFormTrl").UifAlert('hide');
        var progresNum = data.Progress.split(" ");
        var isGenerateIndex = data.UrlFile.indexOf("/");

        if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
            $("#AccountingReportsForm").validate();
            if ($("#AccountingReportsForm").valid()) {
                processTableRunningReportTrl = undefined;
                clearInterval(timeReportTrl);

                MainTaxRetentionList.generateFileTrl(data.ProcessId, data.RecordsNumber, data.Description);
            }

        } else if (isGenerateIndex > 0) {
            MainTaxRetentionList.SetDownloadLinkTrl(data.UrlFile);
        } else {
            $("#alertFormTrl").UifAlert('show', Resources.GenerationReportProcess, "danger");
        }
    }

    static cleanAutocompleteTrl(identifier) {
        $('#' + identifier + 'ByDocumentNumberRequest').val("");
        $('#' + identifier + 'ByNameRequest').val("");

        journalPayerIndividualId = 0;

        $('#' + identifier + 'ByDocumentNumberRequest').parent().parent().hide();
        $('#' + identifier + 'ByNameRequest').parent().parent().hide();
    }

    static getTotalRecordTrl() {
        $.ajax({
            async: false,
            url: ACC_ROOT + "Reports/GetTotalRecordsTaxRetention",
            data: {
                "branchId": $("#TRLReportBranch").val(), "currencyId": $("#TRLReportCurrency").val(),
                "paymentOrder": $("#TRLPaymentOrder").val(), "beneficiaryTypeId": $("#TRLBeneficiary").val(),
                "individualId": individualId, "taxId": $("#TRLTaxes").val()
            },
            success: function (data) {
                if (data.success) {
                    var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                    $('#pnlModalTotRecords').text(msj);
                    if (data.result.records > 0) {
                        $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                    } else {
                        msj = Resources.MsgTotRecordsToProcess;
                        $("#alertFormTrl").UifAlert('show', msj, "warning");
                    }
                }
                else {
                    $("#alertFormTrl").UifAlert('show', Resources.MessageInternalError, "danger");
                }
                unlockScreen();
            }
        });
    }

    static getMassiveReportsTrl() {
        var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.SearchTaxesRetention.toUpperCase();
        $("#tableMassiveProcessReport").UifDataTable({ source: controller });

        setTimeout(function () {
            processTableRunningReportTrl = $('#tableMassiveProcessReport').UifDataTable('getData');
        }, 1000);

        if (processTableRunningReportTrl != undefined) {
            if (validateProcessReport(processTableRunningReportTrl, processTableRunningReportTrl.length)) {
                clearInterval(timeReportTrl);
                isRunningReportTrl = false;
                $("#alertFormTrl").UifAlert('hide');
            } else {
                isRunningReportTrl = true;
            }
        }
    };

    static generateFileTrl(processId, records, reportDescription) {
        $.ajax({
            url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
            data: {
                "processId": processId, "reportTypeDescription": reportDescription,
                "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
            },
            success: function (data) {
                if (data[0].ErrorInfo == null) {
                    if (data[0].ExportedFileName == "-1") {
                        $("#alertFormTrl").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                    }
                    else {
                        MainTaxRetentionList.getMassiveReportsTrl();
                        if (!isRunningReportTrl) {
                            timeReportTrl = window.setInterval(MainTaxRetentionList.getMassiveReportsTrl, 6000);
                        }
                    }
                } else {
                    $("#alertFormTrl").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
                }
            }
        });
    }

    static SetDownloadLinkTrl(fileName) {
        var url_path = '';
        if (ACC_ROOT.length > 13) {
            var url = ACC_ROOT.replace("Accounting/", "")
            url_path = url + fileName;
        }
        else {
            url_path = ACC_ROOT + fileName;
        }
        reportFileExist(url_path, "alertFormTrl", "selectFileTypePartial");
    }
}

function loadAutocompleteEventsTrl(identifier) {
    $('#' + identifier + 'ByDocumentNumberRequest').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesTrl(identifier, selectedItem);
    });

    $('#' + identifier + 'ByNameRequest').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesTrl(identifier, selectedItem);
    });
}

function fillAutocompletesTrl(identifier, selectedItem) {
    individualId = 0;

    $('#' + identifier + 'ByDocumentNumberRequest').val(selectedItem.DocumentNumber);
    $('#' + identifier + 'ByNameRequest').val(selectedItem.Name);

    if (selectedItem.Id != undefined) {
        if (selectedItem.IndividualId == undefined) {
            individualId = selectedItem.Id;
        } else {
            individualId = selectedItem.IndividualId;
        }

        if (selectedItem.InsuredCode != undefined) {
            personCode = selectedItem.InsuredCode;
        }
        if (selectedItem.SupplierCode != undefined) {
            personCode = selectedItem.SupplierCode;
        }
    } else {
        individualId = selectedItem.CoinsuranceIndividualId;
    }
}