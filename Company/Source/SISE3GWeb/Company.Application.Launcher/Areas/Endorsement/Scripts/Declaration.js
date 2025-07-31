var riskController = 'Declaration';
var EndorsmentDeclaration = {
    Enums: {},
    Endorsment: {},
    Declaration: {},
    Risks: {},
    Insurances: {},
    ObjectsCalculate: {}
};
var ValidatorElements = {};
var InsuranceObjectId;
var riskSelected;
var CoverageGroupIdTemporal;
var DeclarationsGroup = [];
class Declaration extends Uif2.Page {

    getInitialState() {
        riskController = 'Declaration';
        $('#DeclaredValue').ValidatorKey(1, 1, 1);
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

        if (glbRisk === null) {
            glbRisk = { Id: 0, Object: "Declaration", formRisk: "#formDeclaration", RecordScript: false, Class: Declaration };
        }

        glbPolicy.TemporalType = TemporalType.Endorsement;

        if (glbPolicy.Id > 0) {
            $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
            Declaration.GetTemporalById(glbPolicy.Id);
        } else {
            Declaration.GetDeclarationEndorsementByPolicyId(glbPolicy.Endorsement.PolicyId);
            Declaration.GetRisksByPolicyId(glbPolicy.Endorsement.PolicyId, $('#CurrentFrom').val());
        }

        Declaration.GetCurrentSummaryByEndorsementId(glbPolicy.Endorsement.Id);

        $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
        GetDateIssue();
        Declaration.ValidatePolicies();
    }

    bindEvents() {
        $("#btnDetail").click(() => { Declaration.LoadModalDetailDeclaration(); });
        $('#selectRisk').on('itemSelected', Declaration.ChangeRisk);
        $('#selectInsuranceObject').on('itemSelected', Declaration.EvalRiskByInsuredObject);

        $("#btnCalculate").on('click', function () {
            Declaration.CreateTemporal();
        });
        $("#btnCancel").on('click', function () {
            Declaration.RedirectSearchController();
        });

        $("#btnSave").on('click', function () {
            Declaration.CreateEndorsement();
        });

        $('#DeclaredValue').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#DeclaredValue').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });
    }

    static AsignValue() {
        if (Declaration.ValidateValue()) {
            var DeclaredValue = $("#DeclaredValue").val();
            return true;
        }
    }

    static EvalRiskByInsuredObject() {
        var declarationModel = $('#formDeclaration').serializeObject();
        DeclarationRequest.EvalRiskByInsuredObject(glbPolicy.Endorsement.PolicyId, declarationModel.RiskId, declarationModel.InsuranceObjectId).done(function (data) {
            if (data.success) {
                if (!data.result) {
                    $.UifNotify('show', { 'type': 'danger', 'message': "Ya existe una declaracion asociada al Riesgo con El Objeto del Seguro", 'autoclose': true });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': "No fue posible verificar el estado del riesgo y el objeto del seguro", 'autoclose': true });
            }
        });
    }

    static CalculateSummary() {
        if (this.validateCalculate()) {
            //Implement array to calculate
            riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
            var insuranceSelected = $("#selectInsuranceObject").UifSelect("getSelectedSource");
            DeclarationRequest.CreateEndorsement(glbPolicy, riskSelected, insuranceSelected).done(function (data) {
                if (data.success) {
                    $("#selectInsuranceObject").UifSelect({ sourceData: data.result.Insured });
                    EndorsmentDeclaration.Insurances = data.result.Insured;
                }
            });
        }
    }

    static validateCalculate() {
        if ($("#selectRisk").UifSelect("getSelected") == "" || $("#selectRisk").UifSelect("getSelected") == undefined || $("#selectRisk").UifSelect("getSelected") == null) {
            return false;
        }
        if ($("#selectInsuranceObject").UifSelect("getSelected") == "" || $("#selectInsuranceObject").UifSelect("getSelected") == undefined || $("#selectInsuranceObject").UifSelect("getSelected") == null) {
            return false;
        }
        return true;
    }

    static ChangeRisk(event, itemSelected) {
        riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
        if (itemSelected.Id > 0) {
            CoverageGroupIdTemporal = riskSelected.CoverageGroupId;
            Declaration.GetInsuredObjectsByRiskId(riskSelected);
        } else {
            $("#selectInsuranceObject").UifSelect({ enable: false });
        }
    }

    static GetInsuredObjectsByRiskId(riskSelected) {
        var riskId = $("#selectRisk").UifSelect("getSelected");
        if (riskId > 0) {
            DeclarationRequest.GetInsuredObjectsByRiskId(riskId).done(function (data) {
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

    //static GetRisksByPolicyId(PolicyId, currentFrom) {
    //    DeclarationRequest.GetRisksByPolicyId(PolicyId, currentFrom).done(function (data) {
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

    static GetRisksByPolicyId(PolicyId) {
        DeclarationRequest.GetRisksByPolicyId(PolicyId).done(function (data) {
            if (data.success) {
                $('#selectRisk').UifSelect({ sourceData: data.result });
                ValidatorElements = data.result;
            }
        });
    }

    static GetDeclarationEndorsementByPolicyId(policyId) {
        DeclarationRequest.GetDeclarationEndorsementByPolicyId(policyId).done(function (data) {
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
        DeclarationRequest.GetRiskByRiskId(riskSelected).done(function (data) {
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
        DeclarationRequest.GetTemporalById(TemporalId).done(function (data) {
            if (data.success) {
                Declaration.LoadEndorsementData(data.result);
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
            var DeclaredValue = parseFloat(NotFormatMoney($("#DeclaredValue").val()));
            var riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
            if (riskSelected != null || riskSelected != undefined) {
                if (riskSelected.LimitMaxRealeaseAmount != null || riskSelected.LimitMaxRealeaseAmount != undefined) {
                    if (DeclaredValue > riskSelected.LimitMaxRealeaseAmount) {
                        $.UifNotify('show', { type: 'danger', message: 'El valor declarado no puede ser mayor al limite por despacho', autoclose: true });
                        return false;
                    }
                }
                return true;
            } else {
                $.UifNotify('show', { type: 'danger', message: Resources.Language.ErrorSelectRisk, autoclose: true });
                return false;
            }
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.EmptyFields, 'autoclose': true });
        }
    }

    static CreateTemporal() {
        $('#formDeclaration').validate();

        if ($('#formDeclaration').valid()) {
            var DeclarationModel = $('#formDeclaration').serializeObject();
            if (Declaration.ValidateTechRequirements(DeclarationModel)) {
                DeclarationModel.DeclaredValue = NotFormatMoney(DeclarationModel.DeclaredValue);
                DeclarationRequest.CreateTemporal(DeclarationModel).done(function (data) {
                    if (data.success) {
                        Declaration.LoadEndorsementData(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorSearchTemp, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
                });
            }
        }
    }

    static ValidateTechRequirements(declarationModel) {
        var prefix = glbPolicy.Prefix.Id;
        var concurrentRisk;
        var result = false;
        switch (prefix) {
            case PrefixType.TRANSPORTE:
                $.each(ValidatorElements, function (index, value) {
                    if (declarationModel.RiskId == value.RiskId) {
                        concurrentRisk = value;
                        result = true;
                    }
                });
                if (concurrentRisk.LimitMaxRealeaseAmount < NotFormatMoney(declarationModel.DeclaredValue)) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.DeclaredValueOverLimit, 'autoclose': true });
                    var result = false
                }
                break;
            default:
                result = true;
                break;
        }
        return result;
    }

    static LoadEndorsementData(policy) {
        $('#labelTemporalId').prop('innerText', ' ' + policy.PolicyId);
        $('#hiddenTemporalId').val(policy.PolicyId);
        glbPolicy.Id = policy.PolicyId;
        glbPolicyEndorsement.Id = policy.PolicyId;
        $("#CurrentFrom").UifDatepicker('setValue', FormatDate(policy.CurrentFrom));
        $("#CurrentTo").UifDatepicker('setValue', FormatDate(policy.CurrentTo));
        $("#InputText").val(policy.Text);
        $("#InputObservation").val(policy.Observation);
        $("#DeclaredValue").val(FormatMoney(policy.DeclaredValue));
        $('#selectRisk').UifSelect('setSelected', policy.RiskId);
        riskSelected = $("#selectRisk").UifSelect("getSelectedSource");
        CoverageGroupIdTemporal = policy.CoverageGroupId;
        InsuranceObjectId = policy.InsuranceObjectId;
        Declaration.LoadSummaryTemporal(policy);
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

        DeclarationRequest.GetCurrentSummaryByEndorsementId(endorsementId).done(function (data) {
            if (data.success) {
                Declaration.LoadCurrentSummary(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static CreateEndorsement() {

        $("#formDeclaration").validate();

        if ($("#formDeclaration").valid()) {
            DeclarationRequest.CreateEndorsement().done(function (data) {
                /// Lanza el resumen de los eventos, si no existen se genera la poliza
                if (data.success) {
                    if (data.result != null) {
                        if (Array.isArray(data.result) && data.result.length > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result, $('#labelTemporalId').prop('innerText').trim());
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
                                    Declaration.RedirectSearchController();
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

    static ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(glbPolicy.Id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Modification.RedirectSearchController();
                }
            });
        }
    }

}