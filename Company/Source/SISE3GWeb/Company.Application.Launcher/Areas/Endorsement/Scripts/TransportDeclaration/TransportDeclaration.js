class TransportDeclaration extends Uif2.Page {

    getInitialState() {
        GetDateIssue();
        riskController = 'TransportDeclaration';
        if (glbRisk === null) {
            glbRisk = { Id: 0, Object: "TransportDeclaration", formRisk: "#formDeclaration", RecordScript: false, Class: TransportDeclaration };
        }
        TransportDeclaration.LoadTitle();
        TransportDeclaration.GetTransportsByPolicyId($('#hiddenPolicyId').val(), $('#CurrentFrom').val());
        TransportDeclaration.GetDeclarationEndorsementByPolicyId($('#hiddenPolicyId').val());
        $('#DeclaredValue').ValidatorKey(1, 1, 1);
        if ($('#hiddenEndorsementId').val() > 0) {
            TransportDeclaration.GetCurrentSummaryByEndorsementId($('#hiddenEndorsementId').val());
        }
        if (glbPolicy.Id > 0) {
            TransportDeclaration.GetTemporalById(glbPolicy.Id);
        }
        $("#inputTicketNumber").val("");
        $("#inputTicketDate").val("");
        $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
        $("#inputTicketDate").ValidatorKey(ValidatorType.Number, 2, 0);
    }

    bindEvents() {
        $("#btnDetail").click(() => { this.LoadModalDetailDeclaration(); });
        $('#selectRisk').on('itemSelected', this.ChangeRisk);
        $("#DeclaredValue").focusin(TransportDeclaration.NotFormatMoneyIn);
        $("#DeclaredValue").focusout(TransportDeclaration.FormatMoneyOut);
        $("#btnCalculate").on('click', function () {
            TransportDeclaration.CreateTemporal();
        });
        $("#btnCancel").on('click', function () {
            TransportDeclaration.RedirectSearchController();
        });
        $("#btnSave").on('click', function () {
            $("#formDeclaration").validate();
            if ($("#formDeclaration").valid()) {
                TransportDeclaration.CreateEndorsement();
            }
        });
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        if (TransportDeclaration.AsignValue()) {
            $(this).val(FormatMoney($(this).val()));
        } else {
            $(this).val(FormatMoney($(this).val()));
        }
    }

    static AsignValue() {
        if (TransportDeclaration.ValidateValue()) {
            var DeclaredValue = $("#DeclaredValue").val();
            return true;
        }
    }

    CalculateSummary() {
        if (this.validateCalculate()) {
            //Implement array to calculate
            riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
            var insuranceSelected = $("#selectInsuranceObject").UifSelect("getSelectedSource");
            TransportDeclarationRequest.CreateEndorsement(glbPolicy, riskSelected, insuranceSelected).done(function (data) {
                if (data.success) {
                    $("#selectInsuranceObject").UifSelect({ sourceData: data.result.Insured });
                    EndorsmentDeclaration.Insurances = data.result.Insured;
                }
            });
        }
    }

    validateCalculate() {
        if ($("#selectRisk").UifSelect("getSelected") == "" || $("#selectRisk").UifSelect("getSelected") == undefined || $("#selectRisk").UifSelect("getSelected") == null) {
            return false;
        }
        if ($("#selectInsuranceObject").UifSelect("getSelected") == "" || $("#selectInsuranceObject").UifSelect("getSelected") == undefined || $("#selectInsuranceObject").UifSelect("getSelected") == null) {
            return false;
        }
        return true;
    }

    ChangeRisk(event, itemSelected) {
        riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
        if (itemSelected.Id > 0) {
            CoverageGroupIdTemporal = riskSelected.CoverageGroupId;
            TransportDeclaration.GetInsuredObjectsByRiskId(riskSelected);
            document.getElementById("DeclaredValue").focus();
            TransportDeclaration.AsignValue();
        } else {
            $("#selectInsuranceObject").UifSelect({ enable: false });
            document.getElementById("DeclaredValue").focus();
            TransportDeclaration.AsignValue();
        }
    }

    static GetInsuredObjectsByRiskId(riskSelected) {
        var riskId = $("#selectRisk").UifSelect("getSelected");
        if (riskId > 0) {
            TransportDeclarationRequest.GetInsuredObjectsByRiskId(riskId).done(function (data) {
                if (data.success) {
                    $("#selectInsuranceObject").UifSelect({ sourceData: data.result });
                    EndorsmentDeclaration.Insurances = data.result;
                    if (InsuranceObjectId != null) {
                        $('#selectInsuranceObject').UifSelect('setSelected', InsuranceObjectId);
                    }
                }
            });
        } else {
            $("#selectRisk").UifSelect();
        }

    }

    //static GetTransportsByPolicyId(PolicyId, currentFrom) {
    //    TransportDeclarationRequest.GetTransportsByPolicyId(PolicyId, currentFrom).done(function (data) {
    //        if (data.success) {
    //            $('#selectRisk').UifSelect({ sourceData: data.result.Transports });
    //            $("#CurrentFrom").val(FormatDate(data.result.Endorsment.CurrentFrom));
    //            $("#CurrentTo").val(FormatDate(data.result.Endorsment.CurrentTo));
    //            $("#inputDays").val(data.result.Days);
    //            EndorsmentDeclaration.Risks = data.result.Transports;
    //            EndorsmentDeclaration.Declaration = data.result;
    //        }

    //    });
    //}
    
    static GetTransportsByPolicyId(PolicyId) {
        TransportDeclarationRequest.GetTransportsByPolicyId(PolicyId).done(function (data) {
            if (data.success) {
                $('#selectRisk').UifSelect({ sourceData: data.result });
            }
        });
    }

    static GetDeclarationEndorsementByPolicyId(policyId) {
        TransportDeclarationRequest.GetDeclarationEndorsementByPolicyId(policyId).done(function (data) {
            if (data.success) {
                $("#CurrentFrom").val(FormatDate(data.result.CurrentFrom));
                $("#CurrentTo").val(FormatDate(data.result.CurrentTo));
                $("#inputDays").val(data.result.Days);
                EndorsmentDeclaration.Declaration = data.result;
            }
        });
    }

    LoadModalDetailDeclaration() {
        var riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
        if (riskSelected == null || riskSelected == undefined) {
            $.UifNotify('show', { type: 'info', message: 'Seleccione un riesgo', autoclose: true });
            return;
        }
        riskSelected.PolicyId = $("#hiddenPolicyId").val();
        TransportDeclarationRequest.GetTransportByRiskId(riskSelected).done(function (data) {
            if (data.success) {
                $("#Description").val(data.result.Transport.Description);
                $("#PackagingDescription").val(data.result.Transport.PackagingDescription);
                $("#Source").val(data.result.Transport.Source);
                $("#Target").val(data.result.Transport.Target);
                $("#TransportTypeDescriptions").val(data.result.Transport.TransportTypeDescriptions);
                $("#ViaDescription").val(data.result.Transport.ViaDescription);
                $('#modalDetailDeclaration').UifModal('showLocal', Resources.Language.LabelDeclarationDetail);
            }
        })
    }


    addCreditNote() {
        $("#formDeclaration").validate();
        if ($("#formDeclaration").valid()) {
            var enabled = true;
            var enabledDescription = Resources.Language.LabelEnabled;
        }
    }

    static GetTemporalById(TemporalId) {
        $('#labelTemporalId').prop('innerText', ' ' + TemporalId);
        TransportDeclarationRequest.GetTemporalById(TemporalId).done(function (data) {
            if (data.success) {
                TransportDeclaration.LoadEndorsementData(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }
    static LoadTitle() {
        var titlePage = $('#hiddenTitle').val();
        titlePage = titlePage.replace(/ยก/g, '<strong>');
        titlePage = titlePage.replace(/!/g, '</strong>');
        $("#globalTitle").html(titlePage);
    }

    static ValidateValue() {

        if ($("#DeclaredValue").val() != '') {
            var DeclaredValue = parseFloat($("#DeclaredValue").val());
            var riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
            if (riskSelected != null || riskSelected != undefined) {
                if (riskSelected.LimitMaxRealeaseAmount != null || riskSelected.LimitMaxRealeaseAmount != undefined) {
                    if (DeclaredValue > riskSelected.LimitMaxRealeaseAmount) {
                        $.UifNotify('show', { type: 'info', message: 'El valor declarado no puede ser mayor al limite por despacho', autoclose: true });
                        return false;
                    }
                }
                return true;
            } else {
                $.UifNotify('show', { type: 'info', message: 'Seleccione un riesgo', autoclose: true });
                return false;
            }
        }
    }
    static CreateTemporal() {
        $('#formDeclaration').validate();
        if ($('#DeclaredValue').val() != "") {
            if ($('#formDeclaration').valid()) {
                var DeclarationModel = $('#formDeclaration').serializeObject();
                DeclarationModel.DeclaredValue = NotFormatMoney(DeclarationModel.DeclaredValue);
                TransportDeclarationRequest.CreateTemporal(DeclarationModel).done(function (data) {
                    if (data.success) {
                        TransportDeclaration.LoadEndorsementData(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorSearchTemp, 'autoclose': true });
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
    static LoadEndorsementData(policy) {
        $('#labelTemporalId').prop('innerText', ' ' + policy.PolicyId);
        $('#hiddenTemporalId').val(policy.PolicyId);
        $("#CurrentFrom").UifDatepicker('setValue', FormatDate(policy.CurrentFrom));
        $("#CurrentTo").UifDatepicker('setValue', FormatDate(policy.CurrentTo));
        $("#InputText").val(policy.Text);
        $("#InputObservation").val(policy.Observation);
        $("#DeclaredValue").val(policy.DeclaredValue);
        $('#selectRisk').UifSelect('setSelected', policy.RiskId);
        riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
        CoverageGroupIdTemporal = policy.CoverageGroupId;
        InsuranceObjectId = policy.InsuranceObjectId;
        TransportDeclaration.LoadSummaryTemporal(policy);
        $("#inputTicketNumber").val(policy.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(policy.TicketDate));
    }

    static LoadSummaryTemporal(temporalData) {
        if (temporalData.Summary != null) {
            $('#labelTemporalId').prop('innerText', ' ' + temporalData.PolicyId);
            $('#labelRisk').text(temporalData.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Summary.Sum));
            $('#labelPremium').text(FormatMoney(temporalData.Summary.Premiun));
            $('#labelExpenses').text(FormatMoney(temporalData.Summary.Expense));
            $('#labelTaxes').text(FormatMoney(temporalData.Summary.Tax));
            $('#labelTotalPremium').text(FormatMoney(temporalData.Summary.TotalPremiun));
            $('#labelDiscount').text(FormatMoney(temporalData.Summary.Discount));
            $('#labelSurcharge').text(FormatMoney(temporalData.Summary.surcharge));

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

        TransportDeclarationRequest.GetCurrentSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                TransportDeclaration.LoadCurrentSummary(data.result);
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
            BranchId: glbPolicy.Branch.Id,
            PrefixId: glbPolicy.Prefix.Id,
            PolicyNumber: glbPolicy.DocumentNumber
        };

        if ($('#hiddenIsCollective').val() == "True") {
            $.redirect(rootPath + 'Endorsement/Search/Search?type=2', searchModel);
        }
        else {
            $.redirect(rootPath + 'Endorsement/Search/Search?type=2', searchModel);
        }
    }

    static CreateEndorsement() {

        $("#formDeclaration").validate();

        if ($("#formDeclaration").valid()) {
            TransportDeclarationRequest.CreateEndorsement().done(function (data) {
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
                                    TransportDeclaration.RedirectSearchController();
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

}