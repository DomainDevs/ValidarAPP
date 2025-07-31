//Codigo de la pagina Temporal.cshtml
class UnderwritingTemporal extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        if (glbPolicy == null) {
            glbPolicy = { Id: 0, TemporalType: TemporalType.Policy, Endorsement: { EndorsementType: EndorsementType.Emission } };
        }
        else {
            if (glbPolicy.TemporalType != TemporalType.Endorsement)
                glbPolicy.TemporalType = TemporalType.Policy;
        }

        $("#inputTemporal").ValidatorKey(ValidatorType.Number, 2, 0);
        ////Se bloquean los botones que no aplican para Colectivas
        if (Underwriting.getQueryVariable("isCollective") == "true") {
            $('#btnInclusionRisk').parent().hide()
            $('#btnChargue').parent().show();
            $('#btnBeneficiaries').parent().show();
            $('#btnRecord').hide();
            $("#btnIssuePolicy").hide();
        }
        else {
            $('#btnInclusionRisk').parent().show();
            $('#btnChargue').parent().hide();
            $('#btnBeneficiaries').parent().hide();
            $('#btnRecord').show();
            $("#btnIssuePolicy").show();
            $("#btnConvertProspect").hide();
        }
        if (glbPersonOnline != null) {
            Underwriting.GetBranches(0);
        }

        if (glbPersonOnline != null && glbPersonOnline.Rol != 0 && glbPersonOnline.CustomerType == 2) {
            Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.ViewModel.HolderName, 1, null);
        } if (glbPersonOnline != null && glbPersonOnline.Rol != 0) {
            if (glbPersonOnline.ViewModel.HolderIdentificationDocument != null)
                Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.ViewModel.HolderIdentificationDocument, glbPersonOnline.CustomerType, null);
            else
                Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.ViewModel.HolderName, 1, null);
        } else {
            glbPersonOnline = null;
        }

        //UnderwritingTemporal.checkhierarchy();
    }
    //Seccion Eventos
    bindEvents() {
        $('#btnChargue').on('click', this.LoadCollective);
        $('#inputTemporal').on('buttonClick', this.SearchTemporal);
        $("#inputTemporal").on("keydown", this.TemporalKey);
        $("#btnIssuePolicy").click(this.Issue);
        $("#btnSaveTemporal").click(this.Save);
        $("#btnNewTemporal").click(this.New);
        $("#inputHolder").on('buttonClick', this.SearchHolder);
        $("#SearchindividualId").on('buttonClick', UnderwritingTemporal.SearchByindividualId);
        $("#SearchCodeId").on('buttonClick', UnderwritingTemporal.SearchByindividualCode);
        $("#inputDateEstablishment").on('blur', this.ValidDate);
        $("#btnPrintTemporal").click(this.PrintTemporal);
    }

    ValidDate() {
        $(this).valid();
    }

    SearchTemporal() {
        $('#inputTemporalbutton').prop('disabled', true);
        UnderwritingTemporal.GetTemporalById($('#inputTemporal').val().trim());
        $('#inputTemporalbutton').prop('disabled', false);
    }

    TemporalKey(event) {
        if (event.which == 13) {
            $('#inputTemporalbutton').prop('disabled', true);
            UnderwritingTemporal.GetTemporalById($('#inputTemporal').val().trim());
            $('#inputTemporalbutton').prop('disabled', false);
        }
    }

    Issue() {
        UnderwritingTemporal.IssuePolicy();
        ScrollTop();
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    Save() {
        glbPolicy.Id = $("#inputTemporal").val();
        Underwriting.SaveTemporal(true);
        $("#inputTemporal").val(glbPolicy.Id);
        ScrollTop();
    }

    New() {
        ScrollTop();
        UnderwritingTemporal.ClearControls();
        //Underwriting.GetDefaultValues();
    }

    SearchHolder() {
        if ($("#inputHolder").val().trim().length > 0) {
            lockScreen();
            Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputHolder").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual);
        }
    }

    PrintTemporal() {
        if (glbPolicy.Id > 0) {
            Underwriting.PrintTemporal();
        }
    }

    static validateProspect() {
        var result = true;

        if (glbPolicy.TemporalType == 2) {
            if (glbPolicy.Holder.CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': AppResources.ErrorPolicyholderWithoutRol + ' , ' + AppResources.ErrorholderWithoutRol, 'autoclose': true
                });
                result = false;
            }

            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Holder.CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': AppResources.InsuredNotPerson, 'autoclose': true
                });
                result = false;
            }

            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true
                });
                result = false;
            }
        } else {
            if (glbPolicy.Holder.CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResources.ErrorPolicyholderWithoutRol + ' , ' + AppResources.ErrorholderWithoutRol, 'autoclose': true
                });
                result = false;
            }

            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Holder.CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResources.InsuredNotPerson, 'autoclose': true
                });
                result = true;
            }

            /*if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true
                });
                result = true;
            }*/
        }

        return result;

    }

    static SearchByindividualId() {
        if ($("#SearchindividualId").val().trim().length > 0) {
            Underwriting.GetIndividualsByIndividualId($("#SearchindividualId").val().trim(), InsuredSearchType.IndividualId, CustomerType.Individual);
        }
    }

    static SearchByindividualCode() {
        if ($("#SearchCodeId").val().trim().length > 0) {
            Underwriting.GetIndividualsByIndividualCode($("#SearchCodeId").val().trim(), InsuredSearchType.CodeIndividual, CustomerType.Individual);
        }
    }

    static ValidateSarlaft(individualId) {
        return true; //se quita temporalmente la validación ya que NASE no requiere sarlaft
        if (glbPolicy.TemporalType == TemporalType.Policy) {
            var result = false;
            UnderwritingRequest.ValidateGetSarlaft(individualId).done(function (data) {
                if (data.success) {

                    switch (data.result) {
                        case SarlaftValidationState.NOT_EXISTS:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExists, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.EXPIRED:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExpired, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.OVERCOME:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftOvercome, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.ACCURATE:
                            result = true;
                            break;
                        case SarlaftValidationState.PENDING:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftPending, 'autoclose': true });
                            result = false;
                            break;
                        default:
                            result = false;
                            break;
                    }

                    return result;
                }
                else {
                    return false;
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                return false;
                Underwriting.ClearControls();
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
            });
            return result;
        }
        else {
            return true;
        }
    }

    static ValidateSarlaftInsured(individualId) {

        if (glbPolicy.TemporalType == TemporalType.Policy) {
            var result = false;
            if (glbPolicy.JustificationSarlaft != null)
                return true;
            UnderwritingRequest.ValidateGetSarlaft(individualId).done(function (data) {
                if (data.success) {

                    switch (data.result) {
                        case SarlaftValidationState.NOT_EXISTS:
                            showErrorToast(AppResources.ValidateSarlaftExistsInsured);
                            result = false;
                            break;
                        case SarlaftValidationState.EXPIRED:
                            showErrorToast(AppResources.ValidateSarlaftExpiredInsured);
                            result = false;
                            break;
                        case SarlaftValidationState.OVERCOME:
                            showErrorToast(AppResources.ValidateSarlaftOvercomeInsured);
                            result = false;
                            break;
                        case SarlaftValidationState.ACCURATE:
                            result = true;
                            break;
                        case SarlaftValidationState.PENDING:
                            showErrorToast(AppResources.ValidateSarlaftPendingInsured);
                            result = false;
                            break;
                        default:
                            result = false;
                            break;
                    }

                    return result;
                }
                else {

                    return false;
                    showInfoToast(AppResources.ErrorValidateSarlaft);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                return false;
                showErrorToast(AppResources.ErrorValidateSarlaft)
            });

            return result;
        }
        else {

            return true;
        }
    }

    static ValidateSarlaftIntermediary(individualId) {
        return true;
        if (glbPolicy.TemporalType == TemporalType.Policy) {
            var result = false;
            UnderwritingRequest.ValidateGetSarlaft(individualId).done(function (data) {
                if (data.success) {

                    switch (data.result) {
                        case SarlaftValidationState.NOT_EXISTS:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExistsIntermediary, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.EXPIRED:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExpiredIntermediary, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.OVERCOME:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftOvercomeIntermediary, 'autoclose': true });
                            result = false;
                            break;
                        case SarlaftValidationState.ACCURATE:
                            result = true;
                            break;
                        case SarlaftValidationState.PENDING:
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftPendingIntermediary, 'autoclose': true });
                            result = false;
                            break;
                        default:
                            result = false;
                            break;
                    }

                    return result;
                }
                else {
                    return false;
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                return false;
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
            });
            return result;
        }
        else {
            return true;
        }
    }

    static GetTemporalById(id) {
        if (id > 0) {
            Underwriting.ClearControls();
            UnderwritingTemporal.getInfoPolicy(id, glbPolicy.TemporalType, glbPolicy.Id);
        } else {
            Underwriting.ClearControls();
        }
    }

    static validateProspect() {
        var result = true;

        if (glbPolicy.TemporalType == 2) {
            if (glbPolicy.Holder.CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': AppResources.ErrorPolicyholderWithoutRol + ' , ' + AppResources.ErrorholderWithoutRol, 'autoclose': true
                });
                result = false;
            }

            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Holder.CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': AppResources.InsuredNotPerson, 'autoclose': true
                });
                result = false;
            }

            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true
                });
                result = false;
            }
        } else {
            if (glbPolicy.TemporalType == 1 || glbPolicy.TemporalType == 4) {
            }
            else {
                if (glbPolicy.Holder.CustomerType == 2) {
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResources.ErrorPolicyholderWithoutRol + ' , ' + AppResources.ErrorholderWithoutRol, 'autoclose': true
                    });
                    result = false;
                }

                if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Holder.CustomerType == 2) {
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResources.InsuredNotPerson, 'autoclose': true
                    });
                    result = true;
                }

                if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType == 2) {
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true
                    });
                    result = true;
                }
            }
        }

        return result;

    }

    static IssuePolicy() {
        if (glbPolicy.Summary.AmountInsured > 0 && glbPolicy.Summary.Premium > 0) {
            $("#formUnderwriting").validate();

            UnderwritingTemporal.validateExchangeRate().then(function (data) {
                var validateExchange = data;
                if ($("#formUnderwriting").valid() && UnderwritingTemporal.validateProspect() && validateExchange) {

                    var policyData = Underwriting.GetPolicyModel(false);
                    if ((policyData !== null && policyData !== undefined)) {
                        lockScreen();
                        UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities).done(function (data) {
                            if (data.success) {
                                if (data.result != null) {
                                    if (glbPolicy.Id > 0) {
                                        if (glbPolicy.Summary.RiskCount > 0) {
                                            UnderwritingRequest.UpdateRisks(glbPolicy.Id, Underwriting.GetControllerRisk()).done(function (data) {
                                                if (data.success) {
                                                    if (data.result != null) {
                                                        glbPolicy = data.result;
                                                        Underwriting.LoadTitle(glbPolicy);
                                                        Underwriting.LoadSubTitles(0);

                                                        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
                                                            if (glbPolicy.Product.CoveredRisk.CoveredRiskType == CoveredRiskType.Vehicles) {
                                                                Underwriting.ExistRisk();
                                                            }
                                                            else if (glbPolicy.Product.CoveredRisk.SubCoveredRiskType == SubCoveredRiskType.Surety || glbPolicy.Product.CoveredRisk.SubCoveredRiskType == SubCoveredRiskType.Lease) {
                                                                Underwriting.ValidateAvailableAmountByTemporalId();
                                                            }
                                                            else if (glbPolicy.Product.CoveredRisk.SubCoveredRiskType == SubCoveredRiskType.Transport) {
                                                                if (glbPolicy.PolicyType.IsFloating && glbPolicy.Summary.AmountInsured == 0 && glbPolicy.Summary.RiskCount > 0) {
                                                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyInZero, 'autoclose': true });
                                                                } else {
                                                                    UnderwritingTemporal.CreatePolicy();
                                                                }
                                                            }
                                                            else {
                                                                UnderwritingTemporal.CreatePolicy();
                                                            }
                                                        } else {
                                                            let arrayPaymentPlanValidate = [14, 38, 39, 40];
                                                            if (data.result.PaymentPlan.PremiumFinance != null || !arrayPaymentPlanValidate.includes(data.result.PaymentPlan.Id)) {

                                                                TemporalRequest.CreateEndorsement(glbPolicy.Id, glbPolicy.DocumentNumber).done(function (data) {
                                                                    /// Lanza el resumen de los eventos, si no existen se genera la poliza
                                                                    if (data.success) {
                                                                        if (Array.isArray(data.result)) {
                                                                            $.UifProgress('close');
                                                                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result, glbPolicy.Id, FunctionType.Individual);
                                                                        } else {
                                                                            var messageService = data.result.split(':');
                                                                            var policyNumber = parseFloat(messageService[1]);
                                                                            var endorsementNumber = parseFloat(messageService[2]);
                                                                            var endorsementId = parseFloat(messageService[3]);
                                                                            var message = data.result.split('.');
                                                                            var showMessage = message[1] + ". " + message[2];//Avoid endorsementId
                                                                            showMessage = (message[4] === null || message[4] === "undefined" || message[4] == " ") ? showMessage : showMessage + ". " + message[4] + ". " + message[5];
                                                                            $.UifProgress('close');
                                                                            $.UifDialog('confirm',
                                                                                { 'message': showMessage + '\n' + AppResources.MessagePrint },
                                                                                function (result) {
                                                                                    if (result) {
                                                                                        PrinterRequest.PrintReportFromOutside(glbPolicy.Branch.Id,
                                                                                            glbPolicy.Prefix.Id,
                                                                                            policyNumber, endorsementId).done(function (data) {
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

                                                                                        UnderwritingTemporal.ClearControls();
                                                                                    }
                                                                                    else {
                                                                                        UnderwritingTemporal.ClearControls();
                                                                                    }
                                                                                });
                                                                        }

                                                                    }
                                                                    else {
                                                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                                                    }
                                                                }).fail(function (jqXHR, textStatus, errorThrown) {
                                                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveEndorsement, 'autoclose': true });
                                                                });
                                                            } else {
                                                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSavePolicyPaymentPlan, 'autoclose': true });
                                                            }

                                                        }
                                                    }
                                                    else {
                                                        Underwriting.ClearControls();
                                                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveEndorsement, 'autoclose': true });
                                                    }
                                                }
                                                else {
                                                    Underwriting.ClearControls();
                                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                                }
                                            }).fail(function (jqXHR, textStatus, errorThrown) {
                                                Underwriting.ClearControls();
                                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                                            });
                                        }
                                        else {
                                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.PolicyNeedRisk, 'autoclose': true });
                                        }
                                    }
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
                                }
                            } else {
                                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                            }
                        }).fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
                        });
                    }
                    else {
                        if (glbPolicy.Summary.RiskCount <= 0) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.PolicyNeedRisk, 'autoclose': true });
                        }
                    }
                }
            });



        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPolicyInZero, 'autoclose': true });
        }
    }
    static CreatePolicy() {
        TemporalRequest.CreatePolicy(glbPolicy.Id, glbPolicy.TemporalType).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (Array.isArray(data.result)) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result, glbPolicy.Id, FunctionType.Individual);
                    } else {
                        var Message = "";
                        if (glbPolicy.PaymentPlan.PremiumFinance != null) {
                            Message = data.result.Message + '\n' + AppResources.MessagePrint;
                        } else {
                            Message = data.result.Message + '\n' + AppResources.MessagePrint;
                        }

                        if (!data.result.IsReinsured) {
                            PrinterRequest.GetReinsurancePolicy(glbPolicy.Id, data.result.EndorsementId, glbPolicy.Prefix.Id).done(function (data) {
                                if (data.success) {
                                    if (!data.result) {
                                        Message = data.result.Message + "\n" + AppResources.PolicyNotReinsured;
                                    }
                                }
                            });
                        }

                        $.UifDialog('confirm', { 'message': Message },
                            function (result) {
                                if (result) {

                                    if (data.result.IsReinsured) {
                                        var position = data.result.Message.indexOf(":");
                                        var policyNumber = "";
                                        if (position != -1) {
                                            policyNumber = data.result.Message.substr(position + 2);
                                        }
                                        if (glbPolicy.TemporalType == 1) {
                                            PrintReportFromOutside(0, 0, 0, 0, glbPolicy.Id);
                                        } else {
                                            PrinterRequest.PrintReportFromOutside(glbPolicy.Branch.Id,
                                                glbPolicy.Prefix.Id,
                                                policyNumber).done(function (data) {
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
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': 'La póliza no se encuentra reasegurada', 'autoclose': false });
                                    }
                                }
                                var notificationId = Underwriting.getQueryVariable("notificationId");
                                if (notificationId != null) {
                                    NotificationsRequest.UpdateNotificationParameter(notificationId);
                                    UnderwritingTemporal.Exit();
                                }
                                UnderwritingTemporal.ClearControls();
                            });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRecordTemporary, 'autoclose': true });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRecordTemporary, 'autoclose': true });
        });
    }

    static ClearControls() {
        glbPolicy = { Id: 0, TemporalType: TemporalType.Policy, Endorsement: { EndorsementType: EndorsementType.Emission } };
        $("#inputTemporal").val("");
        let endorsementIdOld = localStorage.getItem('EndorsementIdOld');
        if (endorsementIdOld > 0 || endorsementIdOld != undefined || endorsementIdOld != null) {
            TemporalRequest.DeleteEndorsementControl(endorsementIdOld);
            endorsementIdOld = 0;
            localStorage.removeItem('EndorsementIdOld');
        }
        Underwriting.ClearControls();
    }

    LoadCollective() {
        Underwriting.SaveTemporalPartial(MenuType.Collective);
    }

    static RedirectCollective(controller) {
        var collectiveModel = $("#formUnderwriting").serializeObject();
        collectiveModel.TempId = glbPolicy.Id;
        collectiveModel.Id = glbPolicy.Id;

        if (glbPolicy.Product != undefined) {
            if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                if (glbPolicy.BusinessType == BusinessType.Direct) {
                    $.redirect(rootPath + 'Collective/' + controller, collectiveModel);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateBusinessTypeCollective, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageVadilateTemporaryCollective, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectPolicy, 'autoclose': true });
        }
    }

    //static checkhierarchy() {
    //    TemporalRequest.ValidationAccessAndHierarchy().done(function (data) {
    //        if (data.success) {
    //            // Tiene Jerarquia no hacer nada
    //        }
    //        else {
    //            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
    //        }
    //    }).fail(function (jqXHR, textStatus, errorThrown) {
    //        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserWithOutHierarchyModule, 'autoclose': true });
    //    });
    //}

    static getInfoPolicy(id, TemporalType, PolicyId) {
        var temporalInfo;
        TemporalRequest.GetTemporalByIdTemporalType(id, TemporalType, PolicyId).done(function (data) {
            if (data.success) {
                temporalInfo = data.result;
                UnderwritingRequest.GetBranches().done(function (data) {
                    if (data.success) {
                        var branchId = temporalInfo.Branch.Id;
                        RequestSummaryAuthorization.GetPoliciesTemporal(id).done(function (data1) {
                            if (data1.result != null) {
                                $.UifDialog('confirm', { 'message': data1.result.Message }, function (responsePolicies) {
                                    if (responsePolicies) {
                                        RequestSummaryAuthorization.DeleteNotificationByTemporalId(id, FunctionType.Individual)
                                            .done(function (data2) {
                                                if (data2.success) {
                                                    if (data.result.find(x => x.Id == branchId) != undefined) {
                                                        Underwriting.UpdateGlbPolicy(temporalInfo);
                                                        Underwriting.LoadTemporal(glbPolicy);
                                                        //if (glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO || glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
                                                        //    UnderwritingTemporal.GetRiskByTemporalId(glbPolicy);
                                                        //}
                                                        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                                                            Underwriting.LoadSiteRisk();
                                                        }
                                                    } else {
                                                        Underwriting.ClearControls();
                                                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.BranchPermissionError, 'autoclose': true });
                                                    }
                                                } else {
                                                    Underwriting.ClearControls();
                                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                                                }
                                            });
                                    } else { Underwriting.ClearControls(); }
                                });
                            } else {
                                if (data.result.find(x => x.Id == branchId) != undefined) {
                                    Underwriting.UpdateGlbPolicy(temporalInfo);
                                    Underwriting.LoadTemporal(glbPolicy);
                                    //if (glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO || glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
                                    //    UnderwritingTemporal.GetRiskByTemporalId(glbPolicy);
                                    //}
                                    if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                                        Underwriting.LoadSiteRisk();
                                    }
                                } else {
                                    Underwriting.ClearControls();
                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.BranchPermissionError, 'autoclose': true });
                                }
                            }
                        });
                    } else {
                        Underwriting.ClearControls();
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.BranchPermissionError, 'autoclose': true });
                    }
                });
            }
            else {
                Underwriting.ClearControls();
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Underwriting.ClearControls();
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
        });
    }

    static validateSarlaftRiskFromPolicy() {
        return true;
        var result = false;
        if (glbPolicy.Summary.RisksInsured != null) {

            $.each(glbPolicy.Summary.RisksInsured, function (index, risk) {
                if (UnderwritingTemporal.ValidateSarlaftInsured(risk.Insured.IndividualId)) {
                    if (risk.Beneficiaries.length > 0) {
                        $.each(risk.Beneficiaries, function (index, item) {
                            if (RiskBeneficiary.ValidateSarlaftBeneficiary(item.IndividualId)) {
                                result = true;
                            }
                            else {
                                result = false;
                            }
                        });
                    } else {
                        result = false;
                    }
                }
                else {
                    result = false;
                }
            });
        }
        return result;
    }

    static validateExchangeRate() {
        return new Promise(function (resolve, reject) {
            var result = true;
            if (glbPolicy != null && glbPolicy.ExchangeRate != null && glbPolicy.ExchangeRate.Currency != null && glbPolicy.ExchangeRate.Currency.Id != 0) {
                UnderwritingRequest.GetExchangeRateByCurrencyId(glbPolicy.ExchangeRate.Currency.Id).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            $("#inputChange").val(FormatMoney(data.result.SellAmount));
                            if (FormatDate(data.result.RateDate) != FormatDate(IssueDate)) {

                                $.UifNotify('show', {
                                    'type': 'danger', 'message': Resources.Language.updateexchangerate, 'autoclose': true
                                });
                                result = false;
                            }
                            else {
                                result = true;
                            }
                        }
                    }
                    resolve(result);
                });
            }
            else {
                resolve(result);
            }
        });

    }
}