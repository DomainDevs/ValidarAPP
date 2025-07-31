//Codigo de la pagina Extension.cshtml
class Extension extends Uif2.Page {

    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            $("#inputFilingNumber").OnlyDecimals(0);
            $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
            $("#inputTicketDate").ValidatorKey(ValidatorType.Number, 2, 0);
            $('#inputExtensionFrom').UifDatepicker('disabled', true);
            $("#inputExtensionTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $('#inputRecord').prop('disabled', true);
            GetDateIssue();
            Extension.LoadData();
            Extension.ShowCoinsurance();
            Extension.LoadCurrentSummary(glbPolicyEndorsement);
            Extension.LoadRecordObservation();
            if (glbPolicy.Id > 0) {
                Extension.GetTemporalById(glbPolicy.Id);
            }
            else {
                Extension.AddToDateExtension();
            }
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);

            this.ValidatePolicies();
            $("#btnRiskTarif").hide();
            if (glbPolicy.Id > 0) {
                $('#btnPrintTemporalExtension').show();
            }
            else {
                $('#btnPrintTemporalExtension').hide();
            }
        }
    }

    bindEvents() {
        $('#inputExtensionTo').on("datepicker.change", function (event, date) {
            Extension.ValidateExtensionDate();
        });

        $("#btnCalculate").on('click', function () {
            Extension.ValidateCreateTemporalCoinsurance();
        });

        $("#btnCancel").on('click', function () {
            Extension.RedirectSearchController();
        });

        $("#btnSave").on('click', function () {
            $("#formExtension").validate();
            if ($("#formExtension").valid()) {
                if ($("#hiddenTemporalId").val() != "") {
                    if (glbPolicy.PaymentPlan != null && (glbPolicy.PaymentPlan.Id == EnumRateTypePrev.PreviCredit30 || glbPolicy.PaymentPlan.Id == EnumRateTypePrev.PreviCredit10 || glbPolicy.PaymentPlan.Id  == EnumRateTypePrev.PreviCredit20)) {
                        $.UifDialog('confirm',
                            { 'message': Resources.Language.NotificationPaymentPlan },
                            function (result) {
                                if (result) {
                                    Extension.CreateEndorsement();
                                }
                            });
                    } else {
                        Extension.CreateEndorsement();
                    }
                } else{
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageCalculatePremium, 'autoclose': true });
                }
            }
        });
        $("#btnPrintTemporalExtension").click(Extension.PrintTemporal);
    }

    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(glbPolicy.Id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Extension.RedirectSearchController();
                }
            });
        }
    }

    static GetCurrentSummaryByEndorsementId(endorsementId) {

        ExtensionRequest.GetCurrentSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                Extension.LoadCurrentSummary(data.result);
                Extension.GetPaymentPlanEndorsementId(data.result.Endorsement.PolicyId, data.result.Endorsement.Id);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static GetTemporalById(temporalId) {
        ExtensionRequest.GetTemporalById(temporalId).done(function (data) {
            if (data.success) {
                Extension.LoadEndorsementData(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static LoadCurrentSummary(policyData) {
        $("#inputCurrentDays").val(CalculateDays($("#inputcurrentFrom").val(), $("#inputCurrentTo").val()));
        $('#labelLastEndorsement').val(policyData.Endorsement.Number + ' - ' + policyData.Endorsement.EndorsementTypeDescription);
        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(policyData.Summary.AmountInsured);
            $('#labelCurrentPremium').text(policyData.Summary.Premium);
            $('#labelCurrentExpenses').text(policyData.Summary.Expenses);
            $('#labelCurrentTaxes').text(policyData.Summary.Taxes);
            $('#labelCurrentTotalPremium').text(policyData.Summary.FullPremium);
        }

    }

    static LoadEndorsementData(policy) {
        glbPolicy.Id = policy.Id;
        $("#inputExtensionFrom").UifDatepicker('setValue', FormatDate(policy.CurrentFrom));
        $("#inputExtensionTo").UifDatepicker('setValue', FormatDate(policy.CurrentTo));
        $("#inputDays").val(CalculateDays($("#inputExtensionFrom").val(), $("#inputExtensionTo").val()));
        $("#selectEndorsementReason").UifSelect("setSelected", policy.Endorsement.EndorsementReasonId);
        $("#inputText").val(policy.Endorsement.Text.TextBody);
        $("#inputObservations").val(policy.Endorsement.Text.Observations);
        if (policy.Endorsement.TicketNumber != null && policy.Endorsement.TicketNumber != undefined) {
            $("#inputTicketNumber").val(policy.Endorsement.TicketNumber);
        }
        if (policy.Endorsement.TicketDate != null && policy.Endorsement.TicketDate != undefined) {
            $("#inputTicketDate").UifDatepicker('setValue', FormatDate(policy.Endorsement.TicketDate));
        }
        Extension.LoadSummaryTemporal(policy);
        if ( policy!= null && policy.PaymentPlan != null)
           glbPolicy.PaymentPlan = policy.PaymentPlan;
        
    }

    static LoadSummaryTemporal(temporalData) {
        if (temporalData.Summary != null) {
            $('#labelRisk').text(temporalData.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(temporalData.Summary.Premium));
            $('#labelExpenses').text(FormatMoney(temporalData.Summary.Expenses));
            $('#labelTaxes').text(FormatMoney(temporalData.Summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(temporalData.Summary.FullPremium));
        }
    }

    static ValidateExtensionDate() {
        if (CompareDates(glbPolicy.Endorsement.CurrentTo, $('#inputExtensionTo').val()) == 1) {
            $('#inputExtensionTo').UifDatepicker('setValue', glbPolicy.Endorsement.CurrentTo);
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ExtensionGreaterInitialDate, 'autoclose': true });
        }
        else {
            $('#inputDays').val(CalculateDays(glbPolicy.Endorsement.CurrentTo, $('#inputExtensionTo').val()));
        }
    }

    static ValidateCreateTemporalCoinsurance() {
        if (glbPolicy.BusinessType == BusinessType.Accepted) {
            if ($("#inputBusinessTypeDescription").val() != 0) {
                Extension.CreateTemporal();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ValidateCoInsuranceAccepted, 'autoclose': true });
            }
        }
        else {
            Extension.CreateTemporal();
        }
    }

    static CreateTemporal() {
        $('#formExtension').validate();
        if (Extension.validateEndorsement) {
            if ($('#formExtension').valid()) {
                var extensionModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formExtension);
                extensionModel.EndorsementReasonDescription = $('#selectEndorsementReason').text();
                if (glbPolicy.EndorsementController != null) {
                    lockScreen();
                    ExtensionRequest.CreateTemporal(extensionModel).done(function (data) {
                        if (data.success) {
                            if (data.result != null) {
                                LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies);
                                Extension.LoadEndorsementData(data.result);
                                $('#btnPrintTemporalExtension').show();
                            }
                        }
                        else {
                            $('#btnPrintTemporalExtension').hide();
                            unlockScreen();
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $('#btnPrintTemporalExtension').hide();
                        unlockScreen();
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
                    });
                }
            }
            else {
                unlockScreen();
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.RequiredAllFields, 'autoclose': true });
            }
        }
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static CreateEndorsement() {
        $("#formExtension").validate();
        if ($("#formExtension").valid()) {
            lockScreen();
            ExtensionRequest.CreateEndorsement().done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (Array.isArray(data.result) && data.result.length > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result, glbPolicy.Id, FunctionType.Individual);
                        } else {
                            Search.SetNewEndorsement(data.result, glbPolicy.Endorsement.Id);
                            $.UifDialog('confirm',
                                { 'message': data.result + '\n ' + Resources.Language.MessagePrint },
                                function (result) {
                                    if (result) {
                                        var message = data.result.split(':');
                                        var endorsementNumber = parseFloat(message[2]);
                                        PrinterRequest.PrintReportFromOutside(glbPolicy.Branch.Id,
                                            glbPolicy.Prefix.Id,
                                           glbPolicy.DocumentNumber, parseFloat(message[3])).done(function (data) {
                                                if (data.success) {
                                                    if (data.result.Url != undefined) {
                                                        DownloadFile(data.result.Url, true, (url) => url.match(/([^\\]+.pdf)/)[0]);
                                                    }
                                                    else {
                                                        $.UifNotify('show', { 'type': 'info', 'message': 'Se genero el proceso de impresion numero ' + data.result + ' favor consultarlo en pantalla de impresion', 'autoclose': false });
                                                    }
                                                }
                                                else {
                                                    if (data.result == Resources.Language.EndorsmentNotReinsured) {
                                                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotPrinter + ": " + data.result, 'autoclose': true });
                                                    }
                                                    else {
                                                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                                    }
                                                }
                                            }).fail(function (data) {
                                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                            });
                                    }
                                    Extension.RedirectSearchController();
                                });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.FirstValueRiskCalculated, 'autoclose': true });
            });
        }
    }

    static ExcelRiskTarif() {
        if (glbPolicy.Id == "") {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoFoundInformationTemporary, 'autoclose': true })
        }
        else {
            window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=0&errors=" + false
                + "&tariffed=" + true
                + "&temporalId=" + glbPolicy.Id
                + "&operationId=" + glbPolicy.Id;
        }
    }

    static GetPaymentPlanEndorsementId(temporalId, endorsementId) {


        ExtensionRequest.GetPaymentPlanScheduleByPolicyEndorsementId(temporalId, endorsementId).done(function (data) {
            if (data.success) {
                glbPaymentPlan = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static load() {
        $("#inputFrom").val(FormatDate(this.CurrentFrom));
        $("#inputTo").val(FormatDate(this.CurrentTo));
        $("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputTo").val()));
    }

    static AddToDateExtension() {
        var date = $("#inputExtensionTo").val();
        if (date != null) {
            $("#inputExtensionTo").val(AddToDate(date, 30, 0, 0));
        }
        $("#inputDays").val(CalculateDays($("#inputExtensionFrom").val(), $("#inputExtensionTo").val()));
    }
    static ShowCoinsurance() {
        if (glbPolicy.BusinessType == BusinessType.Accepted) {
            $("#divCoInsuranceAccepted").show();
        }
        else {
            $("#divCoInsuranceAccepted").hide();
        }
    }
    static LoadData() {
        $('#inputcurrentFrom').val(FormatDate(glbPolicy.CurrentFrom));
        $('#inputCurrentTo').val(FormatDate(glbPolicy.Endorsement.CurrentTo));
        $("#inputCurrentDays").val(CalculateDays(FormatDate(glbPolicy.CurrentFrom), FormatDate(glbPolicy.Endorsement.CurrentTo)));
        $('#inputExtensionFrom').val(FormatDate(glbPolicy.Endorsement.CurrentTo));
        $('#inputExtensionTo').val(FormatDate(glbPolicy.Endorsement.CurrentTo));
        Extension.GetEndorsementReasons();
    }

    static validateEndorsement() {
        var result = true;
        if (CalculateDays($('#inputExtensionFrom').val(), $('#inputExtensionTo').val()) == 0) {
            result = false;
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ExtensionGreaterInitialDate, 'autoclose': true });
        }
        if (CompareDates($('#inputExtensionFrom').val(), $('#inputExtensionTo').val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ExtensionGreaterInitialDate, 'autoclose': true });
        }
        return result;
    }

    static GetEndorsementReasons(selectedId) {
        ExtensionRequest.GetExtensionReasons().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectEndorsementReason").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectEndorsementReason").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }

        });
    }

    static LoadRecordObservation() {
        if (glbPolicy.EndorsementTexts != undefined && glbPolicy.EndorsementTexts.length > 0) {
            var recordText = "";
            glbPolicy.EndorsementTexts.forEach(function (item) {
                recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
            });
            $('#inputRecord').val(recordText);
        }
    }

    static PrintTemporal() {
        ExtensionRequest.GetSummaryByTemporalId(glbPolicy.Id).done(function (data) {
            if (data.success) {
                ExtensionRequest.GenerateReportTemporary(data.result.TempId, glbPolicy.Prefix.Id, glbPolicy.Id).done(function (data) {
                    if (data.success) {
                        DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
                    }
                });
            }
        });
    }

}
$(() => {
    new Extension();
});
