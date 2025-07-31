//var PolicyModel;
//var Risks;
//var CoverageGroupId;
//var Endorsements;
//var BillingPeriodId;
//var DeclarationPeriodId;
class TransportAdjustment extends Uif2.Page {
    getInitialState() {
        $('#inputFrom').UifDatepicker('disabled', true);
        $('#inputTo').UifDatepicker('disabled', true);
        $("#inputFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputTicketNumber").val("");
        $("#inputTicketDate").val("");
        $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
        $("#inputTicketDate").ValidatorKey(ValidatorType.Number, 2, 0);
        if (glbPolicy.Endorsement.Id > 0) {
            TransportAdjustment.LoadCurrentSummary(glbPolicy);
        }

        if (glbPolicy.Id > 0) {
            TransportAdjustment.GetTemporalById(glbPolicy.Id);
        }
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
        TransportAdjustment.GetTransportsByPolicyId();
        TransportAdjustment.GetAdjustmentEndorsementByPolicyId();
        GetDateIssue();

    }

    bindEvents() {
        $('#btnSave').on('click', function () {
            $("#formAdjustment").validate();
            if ($("#formAdjustment").valid()) {
                TransportAdjustment.CreateEndorsement();
            }
        });
        $("#btnDetail").click(() => { this.GetEndorsementTypeDeclration(); });
        $('#selectRisk').on('itemSelected', this.GetInsuredObjectsByRiskId);
        $("#btnCancel").on('click', function () {
            TransportAdjustment.RedirectSearchController();
        });
        $("#btnCalculate").on('click', function () {
            TransportAdjustment.CreateTemporal();
        });
    }

    //static LoadTitle() {
    //    var titlePage = $('#hiddenTitle').val();
    //    titlePage = titlePage.replace(/¡/g, '<strong>');
    //    titlePage = titlePage.replace(/!/g, '</strong>');
    //    $("#globalTitle").html(titlePage);
    //}

    static GetTransportsByPolicyId() {
        TransportAdjustmentRequest.GetTransportsByPolicyId($("#hiddenPolicyId").val()).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    Risks = data.result;
                    BillingPeriodId = Risks[0].BillingPeriodId;
                    DeclarationPeriodId = Risks[0].DeclarationPeriodId;
                    $("#selectRisk").UifSelect({ sourceData: data.result, filter: true });
                }
            }
        });
    }

    static GetAdjustmentEndorsementByPolicyId() {
        TransportAdjustmentRequest.GetAdjustmentEndorsementByPolicyId($("#hiddenPolicyId").val()).done(function (data) {
            if (data.success) {

                $("#inputFrom").UifDatepicker('setValue', FormatDate(data.result.CurrentFrom));
                $("#inputTo").UifDatepicker('setValue', FormatDate(data.result.CurrentTo));
                $("#inputDays").val(data.result.Days);
            }
        });
    }

    GetInsuredObjectsByRiskId() {
        var riskId = $("#selectRisk").UifSelect("getSelected");
        if (riskId > 0) {
            $.each(Risks, function (index, val) {
                var riskid = Risks[index].RiskId;
                if (riskid == riskId);
                {
                    CoverageGroupId = Risks[index].CoverageGroupId;
                }
            });
            TransportAdjustmentRequest.GetInsuredObjectsByRiskId(riskId).done(function (data) {
                if (data.success) {
                    var insuredObjects = data.result;
                    $("#selectInsuredObject").UifSelect({ sourceData: insuredObjects, filter: true });
                }
            });
            //TransportAdjustment.CalculateDates();
        }
        else {
            $("#selectInsuredObject").UifSelect();
        }
    }

    addAjustment() {
        $("#formAdjustment").validate();
        if ($("#formAdjustment").valid()) {
            var enabled = true;
            var enabledDescription = Resources.Language.LabelEnabled;
        }
    }
    //static CalculateDates() {
    //    TransportAdjustmentRequest.CalculateDays($("#inputFrom").val().toString(), $("#inputTo").val().toString(), BillingPeriodId).done(function (data) {
    //        if (data.success) {
    //            $("#inputFrom").val(FormatDate(data.result.CurrentFrom));
    //            $("#inputTo").val(FormatDate(data.result.CurrentTo));
    //            $("#inputDays").val(FormatDate(data.result.Days));
    //        }
    //        else {
    //            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
    //        }
    //        });
    //}

    GetEndorsementTypeDeclration() {

        var isuredobjectId = $("#selectInsuredObject").UifSelect("getSelected");
        if (isuredobjectId == null || isuredobjectId == undefined) {
            $.UifNotify('show', { type: 'info', message: 'Seleccione objeto del seguro', autoclose: true });
            return;
        }
        var riskId = $("#selectRisk").UifSelect("getSelected");
        if (riskId == null || riskId == undefined) {
            $.UifNotify('show', { type: 'info', message: 'Seleccione un riesgo', autoclose: true });
            return;
        }
        TransportAdjustmentRequest.GetCompanyEndorsementByEndorsementTypeDeclration($("#hiddenPolicyId").val(), riskId).done(function (data) {
            if (data.success) {
                $('#tableDetailAdjustment').UifDataTable('clear');
                $('#tableDetailAdjustment').UifDataTable('addRow', data.result);
                $('#modalDetailAdjustment').UifModal('showLocal', Resources.Language.LoadModalDetailAdjustment);

            }
        });
    }

    //static RedirectSearchController() {
    //    var searchModel = {
    //        BranchId: glbPolicy.Branch.Id,
    //        PrefixId: glbPolicy.Prefix.Id,
    //        PolicyNumber:glbPolicy.DocumentNumber
    //    };
    //    $.redirect(rootPath + 'Endorsement/Search/Search?type=2', searchModel);
    //}

    static CreateTemporal() {
        $('#formAdjustment').validate();
        if ($('#formAdjustment').valid()) {
            var AdjustmentModel = $('#formAdjustment').serializeObject();
            AdjustmentModel.CurrentFrom = $("#inputFrom").UifDatepicker('getValue');
            AdjustmentModel.CurrentTo = $("#inputTo").UifDatepicker('getValue');
            TransportAdjustmentRequest.CreateTemporal(AdjustmentModel).done(function (data) {
                if (data.success) {
                    TransportAdjustment.LoadEndorsementData(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
            });
        }
    }

    static GetTemporalById(temporalId) {
        TransportAdjustmentRequest.GetTemporalById(temporalId).done(function (data) {
            if (data.success) {
                TransportAdjustment.LoadEndorsementData(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result.endorsement, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp.temporalData, 'autoclose': true });
        });
    }
    static LoadEndorsementData(ajustment) {
        $('#labelTemporalId').prop('innerText', ' ' + ajustment.PolicyId);
        $('#hiddenTemporalId').val(ajustment.PolicyId);
        $("#inputFrom").UifDatepicker('setValue', FormatDate(ajustment.CurrentFrom));
        $("#inputTo").UifDatepicker('setValue', FormatDate(ajustment.CurrentTo));
        $("#inputDays").val(ajustment.Days);
        $("#InputText").val(ajustment.Text);
        $("#InputObservation").val(ajustment.Observation);
        $("#inputTicketNumber").val(ajustment.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(ajustment.TicketDate));

        TransportAdjustment.LoadSummaryTemporal(ajustment);
    }

    static LoadSummaryTemporal(temporalData) {
        $('#hiddenTemporalId').val(temporalData.PolicyId);

        if (temporalData != null) {
            $('#labelRisk').text(temporalData.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Sum));
            $('#labelPremium').text(FormatMoney(temporalData.Premiun));
            $('#labelExpenses').text(FormatMoney(temporalData.Expense));
            $('#labelTaxes').text(FormatMoney(temporalData.Tax));
            $('#labelSurcharge').text(FormatMoney(temporalData.Surcharge));
            $('#labelDiscount').text(FormatMoney(temporalData.Discount));
            $('#labelTotalPremium').text(FormatMoney(temporalData.TotalPremiun));
        }
    }


    static CreateEndorsement() {

        $("#formAdjustment").validate();

        if ($("#formAdjustment").valid()) {
            TransportAdjustmentRequest.CreateEndorsement().done(function (data) {
                /// Lanza el resumen de los eventos, si no existen se genera la poliza
                if (data.success) {
                    if (data.result != null) {
                        if (Array.isArray(data.result) && data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result, glbPolicy.Id.trim());
                        } else {
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
                                                    DownloadFile(data.result.Url, true, (url) => url.match(/([^\\]+.pdf)/)[0]);
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
                                    TransportAdjustment.RedirectSearchController();
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
        }
    }

    static LoadCurrentSummary(policyData) {
        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(FormatMoney(policyData.Summary.AmountInsured));
            $('#labelCurrentPremium').text(FormatMoney(policyData.Summary.Premium));
            $('#labelCurrentExpenses').text(FormatMoney(policyData.Summary.Expenses));
            $('#labelCurrentTaxes').text(FormatMoney(policyData.Summary.Taxes));
            $('#labelCurrentTotalPremium').text(FormatMoney(policyData.Summary.FullPremium));
            $('#labelCurrentSurcharges').text(FormatMoney(policyData.Summary.Surcharges));
            $('#labelCurrentDiscounts').text(FormatMoney(policyData.Summary.Discounts));
        }
    }

}
    



