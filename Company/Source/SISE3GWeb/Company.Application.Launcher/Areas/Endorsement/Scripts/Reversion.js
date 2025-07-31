class Reversion extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            Reversion.GetEndorsementReasons(0);
            $('#Nroregistration').ValidatorKey(ValidatorType.Number, 2, 0);
            $('#Record').prop('disabled', true);
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
            if (glbPolicy.DocumentNumber != null) {
                Reversion.LoadSummaryEndorsement(glbPolicyEndorsement);
            }
            GetDateIssue();
            this.ValidatePolicies();
            Reversion.LoadRecordObservation();
        }
    }
    bindEvents() {

        $('#btnCancel').on('click', function () {
            Reversion.RedirectSearchController();
        });

        $('#btnSaveReversion').on('click', function () {
            $('#formReversion').validate();
            var campo = $('#Nroregistration').val();
            if ($('#formReversion').valid()) {
                Reversion.CreateTemporal();
            }

        });
    }

    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message}, function (responsePolicies) {
                if (responsePolicies) {                   
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(glbPolicy.Id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Reversion.RedirectSearchController();
                }
            });
        }
    }  

    static GetSummaryByEndorsementId(endorsementId) {
        ReversionRequest.GetSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                Reversion.LoadSummaryEndorsement(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static LoadSummaryEndorsement(policyData) {
        //No reversion on emision 0
        if (policyData.Endorsement.Number <= 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.DisabledEmisionReversion, 'autoclose': true });
            $('#btnSaveReversion').prop('disabled', true);
        }

        if (policyData.Summary != null) {
            $('#hiddenEndorsementFrom').val(policyData.CurrentFrom);
            $('#hiddenEndorsementTo').val(policyData.CurrentTo);
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



    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static CreateTemporal() {

        var reversionModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formReversion);   
        lockScreen();
        ReversionRequest.CreateTemporal(glbPolicy.EndorsementController, reversionModel).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.Id, FunctionType.Individual);
                    } else {
                        var message = Resources.Language.SuccessfullyEndorsementGenerated + "." + Resources.Language.LabelNumberPolicy + ": " + glbPolicy.DocumentNumber + "." + Resources.Language.EndorsementNumber + ": " + data.result.Endorsement.Number;
                        glbPolicy.EndorsementOriginId = glbPolicy.Endorsement.Id;
                        glbPolicy.Endorsement.Id = data.result.EndorsementId;
                        glbPolicy.Endorsement.Number = data.result.Endorsement.Number;
                        glbPolicy.Change = true;
                        glbPolicy.Id = 0;
                        $.UifDialog('confirm', { 'message': message + '\n ' + Resources.Language.MessagePrint }, function (result) {
                            if (result) {
                                PrinterRequest.PrintReportEndorsement(glbPolicy.Prefix.Id,
                                    data.result.Endorsement.PolicyId, data.result.Endorsement.Id).done(function (data) {
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
                                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                    });

                                //PrintReportFromOutside(glbPolicy.Branch.Id, glbPolicy.Prefix.Id, glbPolicy.DocumentNumber, data.result.Endorsement.Number, 0);
                            }
                            Reversion.RedirectSearchController();
                        });
                    }
                }
                else {
                    unlockScreen();
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorCreateTemporary, 'autoclose': true });
                }
            }
            else {
                unlockScreen();
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            unlockScreen();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorCreateTemporary, 'autoclose': true });
        });
    }

   

    static GetEndorsementWorkFlow(PolyciId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Endorsement/Reversion/GetEndorsementWorkFlow',
            data: JSON.stringify({ PolyciId: PolyciId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
  static GetEndorsementReasons(selectedId) {
      ReversionRequest.GetReversionReasons().done(function (data) {
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
            $('#Record').val(recordText);
        }
    }

}