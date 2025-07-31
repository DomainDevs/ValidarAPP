var temporaryTerm = null;

class ChangeTerm extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        $('#inputIssueDate').UifDatepicker('disabled', true);
        $(".datepicker").datepicker({
            format: DateFormat,
            language: "es"
        });
        $('#inputDateTransfer').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputRecord').prop('disabled', true);
        ChangeTerm.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
        GetDateIssue();
        if (glbPolicy.Id > 0) {
            PolicyRequest.GetTemporalById(glbPolicy.Id, glbPolicy.EndorsementController).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        ChangeTerm.LoadEndorsementData(data.result);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            ChangeTerm.AddToDateExtension();
        }
        $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
        this.ValidatePolicies();
        ChangeTerm.LoadRecordObservation();
        $("#btnPrintTemporalTerm").hide();
    }
    bindEvents() {

        $('#inputDateTransfer').on("datepicker.change", this.ValidateChangeTermDate);
        $('#btnCalculate').on('click', this.CreateTemporal);
        $('#btnCancel').on('click', ChangeTerm.RedirectSearchController);
        $('#btnSave').on('click', this.SavePolicy);
        $("#btnPrintTemporalTerm").click(ChangeTerm.PrintTemporal);
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
                    ChangeTerm.RedirectSearchController();
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
        }
    }
    static LoadEndorsementData(policy) {
        $('#inputDateTransfer').UifDatepicker('setValue', FormatDate(policy.CurrentFrom));
        $('#inputCurrentTo').UifDatepicker('setValue', FormatDate(policy.Endorsement.CurrentTo));
        $('#inputChangeTermDays').val(CalculateDays($('#inputDateTransfer').val(), FormatDate(policy.CurrentTo)));
        $('#inputText').val(policy.Endorsement.Text.TextBody);
        $('#inputObservations').val(policy.Endorsement.Text.Observations);
        ChangeTerm.LoadSummaryTemporal(policy);
    }
    static LoadSummaryTemporal(temporalData) {
        temporaryTerm = temporalData;
        glbPolicy.Id = temporalData.Id;
        if (temporalData.Summary != null) {
            $('#labelRisk').text(temporalData.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(temporalData.Summary.Premium));
            $('#labelExpenses').text(FormatMoney(temporalData.Summary.Expenses));
            $('#labelTaxes').text(FormatMoney(temporalData.Summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(temporalData.Summary.FullPremium));
        }
    }

    ValidateChangeTermDate() {
        if (CompareDates(glbPolicy.CurrentFrom, $('#inputDateTransfer').val()) == 1) {
            $('#inputDateTransfer').UifDatepicker('setValue', glbPolicy.CurrentFrom);
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ChangeTermDateGreaterPolicy, 'autoclose': true });
        }
        else {
            let days = CalculateDays(glbPolicyEndorsement.CurrentFrom, glbPolicy.Endorsement.CurrentTo)
            $('#inputCurrentTo').UifDatepicker('setValue', FormatDate(AddToDate($('#inputDateTransfer').val(), days), 0, 0));
            $('#inputChangeTermDays').val(CalculateDays($('#inputDateTransfer').val(), $('#inputCurrentTo').val()));

        }
    }
    CreateTemporal() {
        $('#formChangeTerm').validate();
        if ($('#formChangeTerm').valid()) {
            var changeTermModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeTerm);
            lockScreen();
            ChangeTermRequest.CreateTemporal(changeTermModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.InfringementPolicies != null) {
                            LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies);
                        }
                        ChangeTerm.LoadSummaryTemporal(data.result);
                        $("#btnPrintTemporalTerm").show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        unlockScreen();
                        $("#btnPrintTemporalTerm").hide();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    lockScreen();
                    $("#btnPrintTemporalTerm").hide();
                }
            });
        }
    }
    SavePolicy() {

        ChangeTerm.CreateEndorsement();
    }
    static CreateEndorsement() {
        $('#formChangeTerm').validate();
        if ($('#formChangeTerm').valid()) {
            if (glbPolicy.Id != "") {
                //var changeTermModel = $('#formChangeTerm').serializeObject();
                var changeTermModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeTerm);
                changeTermModel.CurrentTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
                changeTermModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
                changeTermModel.TemporalId = glbPolicy.Id;
                changeTermModel.EndorsementId = glbPolicy.Endorsement.Id;
                $('#btnSave').attr('disabled', true);
                lockScreen();
                ChangeTermRequest.CreateEndorsementChangeTerm(changeTermModel).done(function (data) {
                    $('#btnSave').removeAttr('disabled');
                    if (data.success) {
                        if (data.result != null && data.result.length > 0) {
                            if (data.result[0].InfringementPolicies.length > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(data.result[0].InfringementPolicies, data.result[0].Id, FunctionType.Individual);
                            } else {
                                var message = Resources.Language.SuccessfullyEndorsementGenerated + "." + Resources.Language.LabelNumberPolicy + ": " + glbPolicy.DocumentNumber + "." + Resources.Language.EndorsementNumber + ": " + data.result[0].Endorsement.Number;
                                $.UifDialog('confirm', { 'message': message + '\n ' + Resources.Language.MessagePrint }, function (result) {
                                    if (result) {
                                        $.each(data.result, function (index, value) {
                                            PrinterRequest.PrintReportEndorsement(glbPolicy.Prefix.Id,
                                                value.Endorsement.PolicyId, value.Endorsement.Id).done(function (data) {
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
                                        });
                                        //PrintReportFromOutside(glbPolicy.Branch.Id, glbPolicy.Prefix.Id,glbPolicy.DocumentNumber, data.result, 0);
                                    }
                                    ChangeTerm.RedirectSearchController();
                                });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        unlockScreen();
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                    unlockScreen();
                });
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageCalculatePremium, 'autoclose': true });
            }
        }
    }
    static AddToDateExtension() {
        $('#inputDateTransfer').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
        if (glbPolicy.Endorsement.CurrentTo != null) {
            $("#inputCurrentTo").val(FormatDate(glbPolicy.Endorsement.CurrentTo));
        }
        $("#inputChangeTermDays").val(CalculateDays($("#inputDateTransfer").val(), $("#inputCurrentTo").val()));
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
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
        ChangeTermRequest.GenerateReportTemporary(temporaryTerm.Endorsement.TemporalId, temporaryTerm.Prefix.Id, temporaryTerm.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }
}