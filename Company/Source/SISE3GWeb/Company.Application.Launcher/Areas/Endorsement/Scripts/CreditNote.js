var policy = {};
var riskController = "CreditNote";
var Risks;
var endorsement;
var RiskCoverages;
var splitTiltle;
var AmountPremium;
var NumResult;
var NumFloat;
var Numfinal;
var Percentage;
var riskId;
var coverageId;
var EndorsementTypeTemporal;
var MaximumPercentPremium;

class CreditNote extends Uif2.Page {
    getInitialState() {
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
            CreditNote.GetEndorsementTypes();
            if (glbPolicy.Endorsement.Id > 0) {
                CreditNote.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
                $("#inputFrom").val(FormatDate(glbPolicy.CurrentFrom));
                $("#inputTo").val(FormatDate(glbPolicy.Endorsement.CurrentTo));
                $("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputTo").val()));
            }

            CreditNoteRequest.GetMaximumPremiumPercetToReturn(glbPolicy.Endorsement.PolicyId).done(function (data) {
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
                $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
                CreditNote.GetTemporalById(glbPolicy.Id);
            } 

            $("#inputTicketNumber").val("");
            $("#inputTicketDate").val("");
            $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
            $("#inputTicketDate").ValidatorKey(ValidatorType.Number, 2, 0);

            /*Se agregan los valores para completar el modelo*/
            $("#hiddenPrefixId").val(glbPolicy.Prefix.Id);
            $("#hiddenBranchId").val(glbPolicy.Branch.Id);
            $("#hiddenEndorsementId").val(glbPolicy.Endorsement.Id);
            $("#hiddenPolicyNumber").val(glbPolicy.DocumentNumber);
            $("#hiddenPolicyId").val(glbPolicy.Endorsement.PolicyId);
            $("#hiddenTemporalId").val(glbPolicy.Id);
            $("#hiddenProductId").val(glbPolicy.Product.Id);
        }
    }

    //Eventos

    bindEvents() {
        $("#inputFrom").on('datepicker.change', this.ChangeFrom);
        $("#inputTo").on('datepicker.change', this.ChangeTo);
        $('#PremiumToReturn').on('', CreditNote.callback);

        $("#btnSave").on('click', function () {
            CreditNote.CreateEndorsement();
        });


        $("#btnCancel").on('click', function () {
            CreditNote.RedirectSearchController();
        });
        $("#btnCalculate").on('click', function () {
            CreditNote.CreateTemporal();
        });
        $('#selectEndorsementType').on('itemSelected', CreditNote.GetTransportsByPolicyIdByEndorsementId);
        $('#selectRisk').on('itemSelected', CreditNote.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId);
        $("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputTo").val()));
        $("#PremiumToReturn").focusin(CreditNote.NotFormatMoneyIn);
        $("#PremiumToReturn").focusout(CreditNote.FormatMoneyOut);
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
        $(this).val($(this).val());
    }

    AsignValue() {
        if (CreditNote.Validate()) {
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

        var percentage = CreditNote.GePercentage(selectCoverage.PremiumAmount);
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

    static callback(event, selectedItem) {
        CreditNoteRequest.GetGroupCoverages(sds, sds).done(function (data) {
            data //retorno el servicio
        }).fail(function (xhrj, request) {

        });
    }

    static GetEndorsementTypes() {
        CreditNoteRequest.GetEndorsementWithPremium(glbPolicy.Endorsement.PolicyId).done(function (data) {
            
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

            //CreditNoteRequest.GetRiksByPolicyIdByEndorsementId(glbPolicy.Endorsement.PolicyId, idendoso).done(function (data) {
            CreditNoteRequest.GetTransportsByPolicyIdByEndorsementId(glbPolicy.Endorsement.PolicyId, idendoso).done(function (data) {
                if (data.success) {
                    Risks = data.result.Transports;
                    $("#selectRisk").UifSelect({ sourceData: Risks, filter: true });
                    if (riskId != null) {
                        $('#selectRisk').UifSelect('setSelected', riskId);
                        CreditNote.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId();
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
            $('#labelCurrentSum').text(policyData.Summary.AmountInsured);
            $('#labelCurrentPremium').text(policyData.Summary.Premium);
            $('#labelCurrentExpenses').text(policyData.Summary.Expenses);
            $('#labelCurrentTaxes').text(policyData.Summary.Taxes);
            $('#labelCurrentSurcharges').text(policyData.Summary.Surcharges);
            $('#labelCurrentDiscounts').text(policyData.Summary.Discounts);
            $('#labelCurrentTotalPremium').text(policyData.Summary.FullPremium);
        }
    }

    static GetCompanyCoveragesByPolicyIdEndorsementIdRiskId() {
        if ($("#selectRisk").UifSelect("getSelected")) {
            var idendoso = $("#selectEndorsementType").UifSelect("getSelected");
            var idrisk = $("#selectRisk").UifSelect("getSelected");
            CreditNoteRequest.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(glbPolicy.Endorsement.PolicyId, idendoso, idrisk).done(function (data) {
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
                creditNoteModel.PolicyId = glbPolicy.Endorsement.PolicyId;
                creditNoteModel.CurrentFrom = 
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
                CreditNoteRequest.CreateTemporal(creditNoteModel).done(function (data) {
                    if (data.success) {

                       //CreditNote.LoadSummaryTemporal(data.result);
                       CreditNote.LoadEndorsementData(data.result);
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
       
        if (temporalData.Summary != null) {
            $('#labelTemporalId').prop('innerText', temporalData.PolicyId);
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
            CreditNoteRequest.CreateEndorsement().done(function (data) {
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
                                    CreditNote.RedirectSearchController();
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
        CreditNoteRequest.GetTemporalById(TemporalId).done(function (data) {
            if (data.success) {
                CreditNote.LoadEndorsementData(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static GetCurrentSummaryByEndorsementId(endorsementId) {

        CreditNoteRequest.GetCurrentSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                CreditNote.LoadCurrentSummaryEndorsement(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static LoadEndorsementData(Policy) {
        $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.PolicyId);
        $('#hiddenTemporalId').val(glbPolicy.PolicyId);
        glbPolicy.Id = glbPolicy.PolicyId;
        glbPolicyEndorsement.Id = glbPolicy.PolicyId;
        $("#CurrentFrom").UifDatepicker('setValue', FormatDate(Policy.CurrentFrom));
        $("#CurrentTo").UifDatepicker('setValue', FormatDate(Policy.CurrentTo));
        $("#InputText").val(Policy.Text);
        $("#InputObservation").val(Policy.Observation);
        $("#DeclaredValue").val(FormatMoney(Policy.DeclaredValue));
        $('#selectRisk').UifSelect('setSelected', Policy.RiskId);
        riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
        $('#selectEndorsementType').UifSelect('setSelected', Policy.EndorsementType);
        EndorsementTypeTemporal = Policy.EndorsementType;
        riskId = Policy.RiskId;
        CoverageGroupIdTemporal = Policy.CoverageGroupId;
        InsuranceObjectId = Policy.InsuranceObjectId;
        CreditNote.GetTransportsByPolicyIdByEndorsementId();
        coverageId = Policy.CoverageId;
        CreditNote.LoadSummaryTemporal(Policy);
        $("#inputTicketNumber").val(Policy.Endorsement.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(Policy.Endorsement.TicketDate));

        //$('#labelTemporalId').prop('innerText', ' ' + policy.PolicyId);
        //$('#hiddenTemporalId').val(policy.PolicyId);
        //glbPolicy.Id = policy.PolicyId;
        //glbPolicyEndorsement.Id = policy.PolicyId;
        //$("#CurrentFrom").UifDatepicker('setValue', FormatDate(policy.CurrentFrom));
        //$("#CurrentTo").UifDatepicker('setValue', FormatDate(policy.CurrentTo));
        //$("#InputText").val(policy.Text);
        //$("#InputObservation").val(policy.Observation);
        //$("#inputDays").val(CalculateDays($("#inputFrom").val(), $("#inputFrom").val()));
        //$("#PremiumToReturn").val(policy.PremiumToReturn);
        //$("#InputText").val(policy.Text);
        //$("#InputObservation").val(policy.Observation);
        //$('#selectEndorsementType').UifSelect('setSelected', policy.EndorsementType);
        //EndorsementTypeTemporal = policy.EndorsementType;
        //riskId = policy.RiskId;
        //CreditNote.GetTransportsByPolicyIdByEndorsementId();
        //coverageId = policy.CoverageId;
        //CreditNote.LoadSummaryTemporal(policy);
        //$("#inputTicketNumber").val(policy.Endorsement.TicketNumber);
        //$("#inputTicketDate").UifDatepicker('setValue', FormatDate(policy.Endorsement.TicketDate));
    }

}
