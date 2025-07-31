var acceptDates = false;
var temporaryCancellation = null;
class Cancellation extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        acceptDates = false;
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            $('#inputCancellationDays').val('');
            $("#inputFilingNumber").OnlyDecimals(0);
            $('#inputCancellationFrom').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $("#btnSave").prop("disabled", true);
            $('#inputRecord').prop('disabled', true);
            if (glbPolicy.CurrentFrom != null)
                $('#inputCancellationFrom').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
            if (glbPolicy.Endorsement.Id > 0) {
                Cancellation.LoadSummaryEndorsement(glbPolicyEndorsement);
            }
            if (glbPolicy.Id > 0) {
                Cancellation.GetTemporalById(glbPolicy.Id);
            }
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
            this.ValidatePolicies();
            Cancellation.LoadRecordObservation();
        }
        GetDateIssue();
        $('#btnPrintTemporalCancellation').hide();
    }

    static LoadSummaryEndorsement(policyData) {

        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(policyData.Summary.AmountInsured);
            $('#labelCurrentPremium').text(policyData.Summary.Premium);
            $('#labelCurrentExpenses').text(policyData.Summary.Expenses);
            $('#labelCurrentSurcharges').text(policyData.Summary.Surcharges);
            $('#labelCurrentDiscounts').text(policyData.Summary.Discounts);
            $('#labelCurrentTaxes').text(policyData.Summary.Taxes);
            $('#labelCurrentTotalPremium').text(policyData.Summary.FullPremium);
        }
    }

    //Eventos
    bindEvents() {
        $('#selectCancellationType').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                Cancellation.EnabledCancellationFrom(selectedItem.Id);
                Cancellation.GetEndorsementReasonsByCancellationType(selectedItem.Id, 0);
            }
            else {
                $('#selectModificationReason').UifSelect();
            }
        });

        $('#inputCancellationFrom').on("datepicker.change", function (event, date) {
            Cancellation.ValidateCancellationDate();
        });

        $('#btnCalculate').on('click', function () {
            $("#btnSave").prop("disabled", true);
            Cancellation.CreateTemporal();
        });

        $('#btnCancel').on('click', function () {
            Cancellation.RedirectSearchController();
        });

        $('#btnSave').on('click', function () {
            Cancellation.CreateEndorsement();
        });

        $('#btnPrintTemporalCancellation').on('click', function () {
            Cancellation.PrintTemporal();
        });
    }

    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    let id = glbPolicy.Id;
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Cancellation.RedirectSearchController();
                }
            });
        }
    }

    static LoadCurrentSummaryEndorsement(policyData) {
        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(policyData.Summary.AmountInsured);
            $('#labelCurrentPremium').text(policyData.Summary.Premium);
            $('#labelCurrentExpenses').text(policyData.Summary.Expenses);
            $('#labelCurrentTaxes').text(policyData.Summary.Taxes);
            $('#labelCurrentTotalPremium').text(policyData.Summary.FullPremium);
            $('#labelCurrentSurcharges').text(policyData.Summary.Surcharges);
            $('#labelCurrentDiscounts').text(policyData.Summary.Discounts);

        }
    }

    static SummaryClear() {
        if (policyData.Summary != null) {
            $("#labelCurrentRisk").text('');
            $("#labelCurrentSum").text('');
            $("#labelCurrentPremium").text('');
            $("#labelCurrentExpenses").text('');
            $("#labelCurrentTaxes").text('');
            $("#labelCurrentTotalPremium").text('');
            $("#labelCurrentSurcharges").text('');
            $("#labelCurrentDiscounts").text('');
        }
    }


    static LoadEndorsementData(policy) {
        $('#selectCancellationType').UifSelect('setSelected', policy.Endorsement.CancellationTypeId);
        Cancellation.EnabledCancellationFrom(policy.Endorsement.CancellationTypeId);
        $('#inputCancellationFrom').UifDatepicker('setValue', FormatDate(policy.CurrentFrom));
        $('#inputCancellationDays').val(CalculateDays($('#inputCancellationFrom').val(), FormatDate(glbPolicy.Endorsement.CurrentTo)));
        Cancellation.GetEndorsementReasonsByCancellationType(policy.Endorsement.CancellationTypeId, policy.Endorsement.EndorsementReasonId);
        $('#inputText').val(policy.Endorsement.Text.TextBody);
        $('#inputObservations').val(policy.Endorsement.Text.Observations);
        Cancellation.LoadSummaryTemporal(policy);
    }

    static EnabledCancellationFrom(cancellationType) {
        switch (parseInt(cancellationType, 10)) {
            case CancellationType.BeginDate:
                $('#inputCancellationFrom').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
                $('#inputCancellationDays').val(CalculateDays($('#inputCancellationFrom').val(), FormatDate(glbPolicy.Endorsement.CurrentTo)));
                $('#inputCancellationFrom').UifDatepicker('disabled', true);
                break;
            case CancellationType.FromDate:
            case CancellationType.ShortTerm:
            case CancellationType.Nominative:
                $('#inputCancellationFrom').UifDatepicker('disabled', false);
                break;
            default:
                break;

        }
    }

    static GetEndorsementReasonsByCancellationType(cancellationType, selectedId) {
        var controller = "";
        CancellationRequest.GetEndorsementReasonsByCancellationType("Endorsement/Cancellation", cancellationType).done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    $('#selectEndorsementReason').UifSelect({ sourceData: controller });
                }
                else {
                    $('#selectEndorsementReason').UifSelect({ sourceData: controller, selectedId: selectedId });
                }
            }
        });
    }

    static LoadSummaryTemporal(temporalData) {
        glbPolicy.Id = temporalData.Id;
        temporaryCancellation = temporalData;
        if (temporalData.Summary != null) {
            $('#labelRisk').text(temporalData.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(temporalData.Summary.Premium));
            $('#labelExpenses').text(FormatMoney(temporalData.Summary.Expenses));
            $('#labelTaxes').text(FormatMoney(temporalData.Summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(temporalData.Summary.FullPremium));
            $('#labelCurrentSurcharges').text(temporalData.Summary.Surcharges, 2);
            $('#labelCurrentDiscounts').text(temporalData.Summary.Discounts, 2);
            $("#btnSave").prop("disabled", false);
        }
    }

    static ValidateCancellationDate() {
        if ($('#inputCancellationFrom').val() == "") {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.CancellationDateGreaterPolicy, 'autoclose': true });
        }
        else if (CompareDates(FormatDate(glbPolicy.CurrentFrom), $('#inputCancellationFrom').val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.CancellationDateGreaterPolicy, 'autoclose': true });
        }
        else if (CompareDates($('#inputCancellationFrom').val(), FormatDate(glbPolicy.Endorsement.CurrentTo)) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.CancellationDateLessPolicy, 'autoclose': true });
        }
        else {
            acceptDates = true;
            $('#inputCancellationDays').val(CalculateDays($('#inputCancellationFrom').val(), FormatDate(glbPolicy.Endorsement.CurrentTo)));
        }
    }

    static CreateTemporal() {
        $('#formCancellation').validate();
        Cancellation.ValidateCancellationDate();
        if (acceptDates) {
            if ($('#formCancellation').valid()) {

                var cancellationModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formCancellation);
                cancellationModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
                cancellationModel.CurrentTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
                cancellationModel.EndorsementFrom = $('#inputCancellationFrom').val();
                cancellationModel.EndorsementTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
                //cancellationModel.IssueDate 
                cancellationModel.EndorsementReasonDescription = $('#selectEndorsementReason').text();
                setTimeout(function () { lockScreen(); });
                CancellationRequest.CreateTemporal(glbPolicy.EndorsementController, cancellationModel).done(function (data) {
                    if (data.success) {
                        LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies);
                        Cancellation.LoadSummaryTemporal(data.result);
                        $('#labelTemporalId').text(data.result.Id);
                        $('#btnPrintTemporalCancellation').show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        $('#btnPrintTemporalCancellation').hide();
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorCreateTemporary, 'autoclose': true });
                    $('#btnPrintTemporalCancellation').hide();
                });
            }
        }
    }


    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static CreateEndorsement() {
        $('#formCancellation').validate();
        if ($('#formCancellation').valid()) {
            if (glbPolicy.Id != "") {
                CancellationRequest.CreateEndorsement(glbPolicy.EndorsementController).done(function (data) {
                    /// Lanza el resumen de los eventos, si no existen se genera la poliza
                    if (data.success) {
                        if (data.result != null) {
                            if (Array.isArray(data.result) && data.result.length > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(data.result, glbPolicy.Id, FunctionType.Individual);
                            } else {
                                Search.SetNewEndorsement(data.result, glbPolicy.Endorsement.Id);
                                $.UifDialog('confirm', { 'message': data.result + '\n ' + Resources.Language.MessagePrint }, function (result) {
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
                                    Cancellation.RedirectSearchController();
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
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                });
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageCalculatePremium, 'autoclose': true });
            }
        }
    }

    static GetTemporalById(temporalId) {
        CancellationRequest.GetTemporalById(glbPolicy.EndorsementController, temporalId).done(function (data) {
            if (data.success) {
                Cancellation.LoadEndorsementData(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static GetCurrentSummaryByEndorsementId(endorsementId) {
        CancellationRequest.GetCurrentSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                Cancellation.LoadCurrentSummaryEndorsement(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }
    static LoadData() {
        $('#inputCancellationFrom').val(FormatDate(glbPolicy.CurrentFrom));
        $("#inputCancellationDays").val(CalculateDays(FormatDate(glbPolicy.CurrentFrom), FormatDate(glbPolicy.Endorsement.CurrentTo)));

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
        CancellationRequest.GenerateReportTemporary(temporaryCancellation.Endorsement.TemporalId, glbPolicy.Prefix.Id, temporaryCancellation.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }
}