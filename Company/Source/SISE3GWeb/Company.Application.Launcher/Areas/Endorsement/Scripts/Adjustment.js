var PolicyModel;
var Risks;
var CoverageGroupId;
var Endorsements;
var BillingPeriodId;
var DeclarationPeriodId;
class Adjustment extends Uif2.Page {
    getInitialState() {
        $('#inputFrom').UifDatepicker('disabled', true);
        $('#inputTo').UifDatepicker('disabled', true);
        $("#inputFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        Declaration.GetCurrentSummaryByEndorsementId(glbPolicy.Endorsement.Id);
        if ($("#hiddenTemporalId").val() > 0) {
            Adjustment.GetTemporalById($("#hiddenTemporalId").val());
        }

        /*Se agregan los valores para completar el modelo*/
        $("#hiddenPrefixId").val(glbPolicy.Prefix.Id);
        $("#hiddenBranchId").val(glbPolicy.Branch.Id);
        $("#hiddenEndorsementId").val(glbPolicy.Endorsement.Id);
        $("#hiddenPolicyNumber").val(glbPolicy.DocumentNumber);
        $("#hiddenPolicyId").val(glbPolicy.Endorsement.PolicyId);
        $("#hiddenTemporalId").val(glbPolicy.Id);
        $("#hiddenProductId").val(glbPolicy.Product.Id);

        Adjustment.GetRisksByPolicyId();
        Adjustment.GetAdjustmentEndorsementByPolicyId();
        GetDateIssue();
        $("#inputTicketNumber").val("");
        $("#inputTicketDate").val("");
        $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
        $("#inputTicketDate").ValidatorKey(ValidatorType.Number, 2, 0);


        $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
    }

    bindEvents() {
        $('#btnSave').on('click', function () {
            $("#formAdjustment").validate();
            if ($("#formAdjustment").valid()) {
                Adjustment.CreateEndorsement();
            }
        });
        $("#btnDetail").click(() => { this.GetEndorsementTypeDeclration(); });
        $('#selectRisk').on('itemSelected', this.GetInsuredObjectsByRiskId);
        $('#selectInsuredObject').on('itemSelected', this.EvalRiskByInsuredObject);

        $("#btnCancel").on('click', function () {
            Adjustment.RedirectSearchController();
        });
        $("#btnCalculate").on('click', function () {
            Adjustment.CreateTemporal();
        });
    }


    EvalRiskByInsuredObject() {
        var adjustModel = $('#formAdjustment').serializeObject();
        AdjustmentRequest.EvalRiskByInsuredObject(glbPolicy.Endorsement.PolicyId, adjustModel.RiskId, adjustModel.InsuranceObjectId).done(function (data) {
            if (data.success) {
                if (!data.result) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Ya existe una declaracion asociada al Riesgo con El Objeto del Seguro", 'autoclose': true });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': "No fue posible verificar el estado del riesgo y el objeto del seguro", 'autoclose': true });
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
            AdjustmentRequest.GetInsuredObjectsByRiskId(riskId).done(function (data) {
                if (data.success) {
                    var insuredObjects = data.result;
                    $("#selectInsuredObject").UifSelect({ sourceData: insuredObjects, filter: true });
                }
            });
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
        AdjustmentRequest.GetCompanyEndorsementByEndorsementTypeDeclration($("#hiddenPolicyId").val(), riskId).done(function (data) {
            if (data.success) {
                $('#tableDetailAdjustment').UifDataTable('clear');
                $('#tableDetailAdjustment').UifDataTable('addRow', data.result);
                $('#modalDetailAdjustment').UifModal('showLocal', Resources.Language.LoadModalDetailAdjustment);

            }
        });
    }

    static LoadTitle() {
        var titlePage = $('#hiddenTitle').val();
        if (titlePage != undefined) {

            titlePage = titlePage.replace(/¡/g, '<strong>');
            titlePage = titlePage.replace(/!/g, '</strong>');
            $("#globalTitle").html(titlePage);
        }
    }

    static GetRisksByPolicyId() {
        AdjustmentRequest.GetRisksByPolicyId($("#hiddenPolicyId").val()).done(function (data) {
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
        AdjustmentRequest.GetAdjustmentEndorsementByPolicyId($("#hiddenPolicyId").val()).done(function (data) {
            if (data.success) {

                $("#inputFrom").UifDatepicker('setValue', FormatDate(data.result.CurrentFrom));
                $("#inputTo").UifDatepicker('setValue', FormatDate(data.result.CurrentTo));
                $("#inputDays").val(data.result.Days);
            }
        });
    }

    static RedirectSearchController() {
        var searchModel = {
            BranchId: $('#hiddenBranchId').val(),
            PrefixId: $('#hiddenPrefixId').val(),
            PolicyNumber: $('#hiddenPolicyNumber').val()
        };
        $.redirect(rootPath + 'Endorsement/Search/Search?type=2', searchModel);
    }


    static ToAbleCalculate(declarations) {
        var rate = (Risks[0].InsuredObjects[0].Rate / 100);
        var percentage = (Risks[0].InsuredObjects[0].DepositPremiumPercentage / 100);
        var totalDeclarations = 0;
        var intialPremium = NotFormatMoney($('#labelCurrentPremium').text());
        $.each(declarations, function (index, value) {
            totalDeclarations += (value.DeclaredAmount * rate);
        });
        intialPremium = (intialPremium - totalDeclarations);
        if (intialPremium < 0) {
            intialPremium = intialPremium * (-1);
        }
        return intialPremium;
    }

    static TotalCalculateDeclarations(declarations) {
        var rate = (Risks[0].InsuredObjects[0].Rate / 100);
        var percentage = (Risks[0].InsuredObjects[0].DepositPremiumPercentage / 100);
        var totalDeclarations = 0;
        var intialPremium = NotFormatMoney($('#labelCurrentPremium').text());
        $.each(declarations, function (index, value) {
            totalDeclarations += (value.DeclaredAmount);
        });

        return totalDeclarations;
    }

    static CreateTemporal() {
        $('#formAdjustment').validate();
        if ($('#formAdjustment').valid()) {
            var AdjustmentModel = $('#formAdjustment').serializeObject();
            AdjustmentModel.CurrentFrom = $("#inputFrom").UifDatepicker('getValue');
            AdjustmentModel.CurrentTo = $("#inputTo").UifDatepicker('getValue');
            AdjustmentModel.RiskId = $('#selectRisk').UifSelect('getSelected');
            AdjustmentModel.InsuredObjectId = $('#selectInsuredObject').UifSelect('getSelected');
            AdjustmentRequest.CreateTemporal(AdjustmentModel).done(function (data) {
                if (data.success) {
                    Adjustment.LoadEndorsementData(data.result);
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
        AdjustmentRequest.GetTemporalById(temporalId).done(function (data) {
            if (data.success) {
                Adjustment.LoadEndorsementData(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result.endorsement, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp.temporalData, 'autoclose': true });
        });
    }

    static LoadEndorsementData(adjustment) {
        $('#labelTemporalId').prop('innerText', ' ' + adjustment.PolicyId);
        $('#hiddenTemporalId').val(adjustment.PolicyId);
        glbPolicy.Id = adjustment.PolicyId;
        glbPolicyEndorsement.Id = adjustment.PolicyId;
        $("#inputFrom").UifDatepicker('setValue', FormatDate(adjustment.CurrentFrom));
        $("#inputTo").UifDatepicker('setValue', FormatDate(adjustment.CurrentTo));
        $("#inputDays").val(adjustment.Days);
        $("#InputText").val(adjustment.Text);
        $("#InputObservation").val(adjustment.Observation);
        $("#inputTicketNumber").val(adjustment.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(adjustment.TicketDate));

        Adjustment.LoadSummaryTemporal(adjustment);
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
            AdjustmentRequest.CreateEndorsement().done(function (data) {
                /// Lanza el resumen de los eventos, si no existen se genera la poliza
                if (data.success) {
                    if (data.result != null) {
                        if (Array.isArray(data.result) && data.result.length > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result, $('#hiddenTemporalId').val());
                        } else {
                            $.UifDialog('confirm',
                                { 'message': data.result + '\n ' + Resources.Language.MessagePrint },
                                function (result) {
                                    if (result) {
                                        var message = data.result.split(':');
                                        var endorsementNumber = parseFloat(message[2]);
                                        PrinterRequest.PrintReportFromOutside($('#hiddenBranchId').val(),
                                            $('#hiddenPrefixId').val(),
                                            $('#hiddenPolicyNumber').val(), parseFloat(message[3])).done(function (data) {
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
                                    Adjustment.RedirectSearchController();
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

    static GetCurrentSummaryByEndorsementId(endorsementId) {

        AdjustmentRequest.GetCurrentSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                Adjustment.LoadCurrentSummary(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static RedirectSearchController() {
        var searchModel = {
            BranchId: $('#hiddenBranchId').val(),
            PrefixId: $('#hiddenPrefixId').val(),
            PolicyNumber: $('#hiddenPolicyNumber').val()
        };

        if ($('#hiddenIsCollective').val() == "True") {
            $.redirect(rootPath + 'Endorsement/Search/#', searchModel);
        }
        else {
            $.redirect(rootPath + 'Endorsement/Search/#', searchModel);
        }
    }

}