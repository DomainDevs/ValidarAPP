//var policy = {};
//var riskController = "TransportCreditNote";
//var Risks;
//var endorsement;
//var RiskCoverages;
//var splitTiltle;
//var AmountPremium;
//var NumResult;
//var NumFloat;
//var Numfinal;
//var Percentage;
//var riskId;
//var coverageId;
//var EndorsementTypeTemporal;
//var MaximumPercentPremium;


class TransportCreditNote extends Uif2.Page {
    getInitialState() {
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
            TransportCreditNote.GetEndorsementTypes();
            if (glbPolicy.Endorsement.Id > 0) {
                TransportCreditNote.LoadCurrentSummaryEndorsement(glbPolicy);
                $("#inputFrom").val(FormatDate(glbPolicy.CurrentFrom));
                $("#inputTo").val(FormatDate(glbPolicy.Endorsement.CurrentTo));
                $("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputTo").val()));
            }        

            TransportCreditNoteRequest.GetMaximumPremiumPercetToReturn($('#hiddenPolicyId').val()).done(function (data) {
                if (data.success) {
                    MaximumPercentPremium = data.result;
                }
                else {
                    MaximumPercentPremium = 100;
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });

            $("#inputFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $("#inputTo").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $('#PremiumToReturn').ValidatorKey(1, 1, 1);
            if (glbPolicy.Id > 0) {
                TransportCreditNote.GetTemporalById(glbPolicy.Id);
            }         
            
            $("#inputTicketNumber").val("");
            $("#inputTicketDate").val("");
            $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
            $("#inputTicketDate").ValidatorKey(ValidatorType.Number, 2, 0);
        }
    }

    //Eventos

    bindEvents() {
        $("#inputFrom").on('datepicker.change', this.ChangeFrom);
        $("#inputTo").on('datepicker.change', this.ChangeTo);
        $('#PremiumToReturn').on('', TransportCreditNote.callback);
        $('#btnSave').on('click', function () {
            $("#formCreditNote").validate();
            if ($("#formCreditNote").valid()) {
                TransportCreditNote.CreateEndorsement();
            }
        });

        $("#btnCancel").on('click', function () {
            TransportCreditNote.RedirectSearchController();
        });
        $("#btnCalculate").on('click', function () {
            TransportCreditNote.CreateTemporal();
        });
        $('#selectEndorsementType').on('itemSelected', TransportCreditNote.GetTransportsByPolicyIdByEndorsementId);
        $('#selectRisk').on('itemSelected', TransportCreditNote.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId);
        $("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputTo").val()));
        $("#PremiumToReturn").focusin(TransportCreditNote.NotFormatMoneyIn);
        $("#PremiumToReturn").focusout(TransportCreditNote.FormatMoneyOut);
        $('#PremiumToReturn').focusout(this.AsignValue);
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        var NumString = $("#PremiumToReturn").val();
        NumFloat = parseFloat(NumString);
        if (NumFloat > 0 && NumFloat != NumResult) {
            var numMultiplicy = NumFloat * (-1);
            NumResult = numMultiplicy.toString();
            $("#PremiumToReturn").val(NumResult);
        }
        $(this).val(FormatMoney($(this).val()));
    }

    AsignValue() {
        if (TransportCreditNote.Validate()) {
            NumResult = $("#PremiumToReturn").val();
            $('#btnCalculate').removeAttr('disabled');
        } else {
            $('#btnCalculate').prop('disabled', 'disabled');
        }
    }

    static Validate() {

        var premiuntoReturn = Math.abs(NumFloat);
        var selectCoverage = $('#selectCoverage').UifSelect("getSelectedSource");

        if (premiuntoReturn > selectCoverage.PremiumAmount) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMaximumPremiumPercentExceeded, 'autoclose': true });
            return false;
        }

        var percentage = TransportCreditNote.GePercentage(selectCoverage.PremiumAmount);
        if (premiuntoReturn > percentage) {
            var exceeded = FormatMoney(premiuntoReturn - percentage, 2);
            var message = Resources.Language.MaximumPremiumPercentExceeded;
            message = message.replace('{0}', exceeded);
            $.UifNotify('show', { 'type': 'info', 'message': message, 'autoclose': true });
            return false;
        }
        return true;
    }

    static GePercentage(premiumAmount) {
        return premiumAmount / 100 * MaximumPercentPremium;
    }

    addCreditNote() {
        $("#formCreditNote").validate();
        if ($("#formCreditNote").valid()) {
            var enabled = true;
            var enabledDescription = Resources.Language.LabelEnabled;
        }
    }

    //static LoadTitle() {
    //    var titlePage = $('#hiddenTitle').val();
    //    //Busqueda fecha de emision
    //    splitTiltle = titlePage.split('¡');
    //    var strTitle = splitTiltle.toString();

    //    splitTiltle = strTitle.split('!');
    //    strTitle = splitTiltle.toString();

    //    splitTiltle = strTitle.split(',');

    //    //carga titulo vista
    //    titlePage = titlePage.replace(/¡/g, '<strong>');
    //    titlePage = titlePage.replace(/!/g, '</strong>');
    //    $("#globalTitle").html(titlePage);
    //    $('#inputIssueDate').prop('outerText', splitTiltle[8].trim());
    //}


    static callback(event, selectedItem) {
        TransportCreditNoteRequest.GetGroupCoverages(sds, sds).done(function (data) {
            data //retorno el servicio
        }).fail(function (xhrj, request) {

        });
    }

    static GetEndorsementTypes() {
        TransportCreditNoteRequest.GetEndorsementWithPremium($('#hiddenPolicyId').val()).done(function (data) {
            //TransportCreditNoteRequest.GetEndorsementTypes($('#hiddenPolicyId').val()).done(function (data) {
            if (data.success) {
                $("#selectEndorsementType").UifSelect({ sourceData: data.result });
                endorsement = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetTransportsByPolicyIdByEndorsementId() {
        if (EndorsementTypeTemporal != null) {
            $('#selectEndorsementType').UifSelect('setSelected', EndorsementTypeTemporal);
        }
        if ($("#selectEndorsementType").UifSelect("getSelected") > 0) {
            var idendoso = $("#selectEndorsementType").UifSelect("getSelected");

            //TransportCreditNoteRequest.GetRiksByPolicyIdByEndorsementId($("#hiddenPolicyId").val(), idendoso).done(function (data) {
            TransportCreditNoteRequest.GetTransportsByPolicyIdByEndorsementId($("#hiddenPolicyId").val(), idendoso).done(function (data) {
                if (data.success) {
                    Risks = data.result.Transports;
                    $("#selectRisk").UifSelect({ sourceData: Risks, filter: true });
                    if (riskId != null) {
                        $('#selectRisk').UifSelect('setSelected', riskId);
                        TransportCreditNote.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId();
                    }
                    $.each(endorsement, function () {
                        if (idendoso == this.Id) {
                            $("#inputFrom").val(FormatDate(this.CurrentFrom));
                            $("#inputTo").val(FormatDate(this.CurrentTo));
                            $("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputTo").val()));
                        }
                    });

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });

        }

    }

    static LoadCurrentSummaryEndorsement(policyData) {
        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(FormatMoney(policyData.Summary.AmountInsured));
            $('#labelCurrentPremium').text(FormatMoney(policyData.Summary.Premium));
            $('#labelCurrentExpenses').text(FormatMoney(policyData.Summary.Expenses));
            $('#labelCurrentTaxes').text(FormatMoney(policyData.Summary.Taxes));
            $('#labelCurrentSurcharges').text(FormatMoney(policyData.Summary.Surcharges));
            $('#labelCurrentDiscounts').text(FormatMoney(policyData.Summary.Discounts));
            $('#labelCurrentTotalPremium').text(FormatMoney(policyData.Summary.FullPremium));
        }
    }

    static GetCompanyCoveragesByPolicyIdEndorsementIdRiskId() {
        if ($("#selectRisk").UifSelect("getSelected")) {
            var idendoso = $("#selectEndorsementType").UifSelect("getSelected");
            var idrisk = $("#selectRisk").UifSelect("getSelected");
            TransportCreditNoteRequest.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId($("#hiddenPolicyId").val(), idendoso, idrisk).done(function (data) {
                if (data.success) {
                    RiskCoverages = data.result.CoverageDTOs;
                    $("#selectCoverage").UifSelect({ sourceData: RiskCoverages, filter: true });
                    if (coverageId != null) {
                        $('#selectCoverage').UifSelect('setSelected', coverageId);
                    }
                    var idcoverage = $("#selectCoverage").UifSelect("getSelected");
                    if (idcoverage > 0) {
                        $.each(RiskCoverages, function (index, val) {
                            var idcover = RiskCoverages[index].Id;
                            if (idrisk == idcover);
                            {
                                AmountPremium = RiskCoverages[index].AmountPremium;

                            }
                        });
                        AmountPremium = RiskCoverages
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });

                }
            });
        }
    }

    static CreateTemporal() {
        $("#formCreditNote").validate();

        if ($('#PremiumToReturn').val() != "") {
            if ($("#formCreditNote").valid()) {
                var creditNoteModel = $("#formCreditNote").serializeObject();
                creditNoteModel.RiskDTOs = Risks;
                creditNoteModel.EndorsementTypes = [];
                creditNoteModel.PolicyDTOs = policy;
                creditNoteModel.Risk = [];
                creditNoteModel.Coverage = [];
                creditNoteModel.CoverageDTOs = RiskCoverages;
                creditNoteModel.TemporalId = $('#labelTemporalId').prop('innerText');
                creditNoteModel.PolicyId = $('#hiddenPolicyId').val();
                //creditNoteModel.PremiumAmount = $('#PremiumToReturn').val();
                creditNoteModel.PremiumToReturn = NotFormatMoney($('#PremiumToReturn').val());
                $.each(endorsement, function () {
                    if (this.Id == $('#selectEndorsementType').UifSelect("getSelected")) {
                        creditNoteModel.EndorsementTypes.push(this);
                    }
                });
                $.each(Risks, function () {
                    if (this.Id == $('#selectRisk').UifSelect("getSelected")) {
                        creditNoteModel.Risk.push(this);
                    }
                });
                $.each(RiskCoverages, function () {
                    if (this.Id == $("#selectCoverage").UifSelect("getSelected")) {
                        creditNoteModel.Coverage.push(this);
                    }
                });
                TransportCreditNoteRequest.CreateTemporal(creditNoteModel).done(function (data) {
                    if (data.success) {
                        TransportCreditNote.LoadSummaryTemporal(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
                });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EmptyFields, 'autoclose': true });
        }

    }

    static LoadSummaryTemporal(temporalData) {
        $('#labelTemporalId').prop('innerText', temporalData.TemporalId);
        $('#hiddenPolicyId').val(temporalData.PolicyId);
        $('#hiddenTemporalId').val(temporalData.PolicyId);
        if (temporalData.Summary != null) {
            $('#labelRisk').text(temporalData.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(temporalData.Summary.Premium));
            $('#labelExpenses').text(FormatMoney(temporalData.Summary.Expenses));
            $("#labelSurcharge").val(FormatMoney(temporalData.Summary.Surcharges));
            $("#labelDiscount").val(FormatMoney(temporalData.Summary.Discounts));
            $('#labelTaxes').text(FormatMoney(temporalData.Summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(temporalData.Summary.FullPremium));
        }
    }
    static GetCreditNoteModel(creditNoteModel) {
        creditNoteModel = {
            validityDateFrom: splitTiltle[8].trim(),
            validityDateTo: splitTiltle[10].trim(),
            Text: $('#InputText').val(),
            Observation: $('#InputObservation').val(),
            Risk: Risks,
            EndorsementType: endorsement,
            PolicyDTOs: policy,
            RiskDTOs: Risks,
            riskCoverageId: $('#selectRisk').UifSelect("getSelected"),
            coverage: RiskCoverages
        };
        return creditNoteModel;
    }
    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
        //var searchModel = {
        //    BranchId: glbPolicy.Branch.Id,
        //    PrefixId: glbPolicy.Prefix.Id,
        //    PolicyNumber: glbPolicy.DocumentNumber
        //};
        //$.redirect(rootPath + 'Endorsement/Search/Search?type=2', searchModel);
    }

    static CreateEndorsement() {
        $("#formCreditNote").validate();
        if ($("#formCreditNote").valid()) {
            TransportCreditNoteRequest.CreateEndorsement().done(function (data) {
                /// Lanza el resumen de los eventos, si no existen se genera la poliza
                if (data.success) {
                    if (data.result != null) {
                        if (Array.isArray(data.result) && data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result, $('#labelTemporalId').prop('innerText').trim());
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
                                    TransportCreditNote.RedirectSearchController();
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

    static GetTemporalById(TemporalId) {
        $('#labelTemporalId').prop('innerText', ' ' + TemporalId);
        TransportCreditNoteRequest.GetTemporalById(TemporalId).done(function (data) {
            if (data.success) {
                TransportCreditNote.LoadEndorsementData(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static GetCurrentSummaryByEndorsementId(endorsementId) {

        TransportCreditNoteRequest.GetCurrentSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                TransportCreditNote.LoadCurrentSummaryEndorsement(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static LoadEndorsementData(policy) {
        $("#inputFrom").UifDatepicker('setValue', FormatDate(policy.validityDateFrom));
        $("#inputTo").UifDatepicker('setValue', FormatDate(policy.validityDateTo));
        $("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputFrom").val()));
        $("#PremiumToReturn").val(policy.PremiumToReturn);
        $("#InputText").val(policy.Text);
        $("#InputObservation").val(policy.Observation);
        $('#selectEndorsementType').UifSelect('setSelected', policy.EndorsementType);
        EndorsementTypeTemporal = policy.EndorsementType;
        riskId = policy.RiskId;
        TransportCreditNote.GetTransportsByPolicyIdByEndorsementId();
        coverageId = policy.CoverageId;
        TransportCreditNote.LoadSummaryTemporal(policy);
        $("#inputTicketNumber").val(policy.Endorsement.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(policy.Endorsement.TicketDate));
    }

}
