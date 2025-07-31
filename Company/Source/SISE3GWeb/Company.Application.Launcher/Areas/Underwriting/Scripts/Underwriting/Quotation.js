//Codigo de la pagina Quotation.cshtml
class UnderwritingQuotation extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        $('#divDateOfEstablishment').hide();
        $('#divRegistrationNumber').hide();
        $('#btnIssuePolicy').prop('disabled', true);
        $("#inputQuotation").ValidatorKey(ValidatorType.Number, 2, 0);
        
        if (glbPolicy == null) {
            glbPolicy = { Id: 0, TemporalType: TemporalType.Quotation, Endorsement: { EndorsementType: EndorsementType.Emission, QuotationVersion: 1 } };
        }

        $('#btnChargue').parent().hide();
        $('#btnBeneficiaries').parent().hide();
        $("#divDaysValidity").show();
        $("#btnConvertProspect").hide();
        if (glbPolicy.Id > 0) {
            $("#btnNewVersion").prop('disabled', false);
            $("#btnSaveQuotation").prop('disabled', false);

            if (glbPolicy.Endorsement.QuotationId == 0 || glbPolicy.Endorsement.QuotationVersion == 0) {
                $("#inputQuotation").val("No. Temporal " + glbPolicy.Id);
                $("#btnNewVersion").prop('disabled', true);
                $("#btnSaveTempQuotation").prop('disabled', false);
            }
            else {
                $("#inputQuotation").val("No. Cotización " + glbPolicy.Endorsement.QuotationId);
                $("#btnSaveTempQuotation").prop('disabled', true);
                $("#btnSaveQuotation").prop('disabled', true);
                $("#btnIssuePolicy").prop('disabled', false);
            }
        }
        else {
            $("#btnNewVersion").prop('disabled', true);
            $("#btnSaveQuotation").prop('disabled', true);
        }

        if (gblProspectData == null) {
            gblProspectData = { IdCardNo: 0 }
        }

        if (glbPolicy.Holder === undefined && gblProspectData.IdCardNo != 0) {
            $("#inputHolder").val(gblProspectData.IdCardNo);
            this.SearchHolder();
        }

        if (glbPersonOnline != null && glbPersonOnline.Rol != 0 && glbPersonOnline.CustomerType == 2) {
            Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.ViewModel.HolderName, 1, null);
        } if (glbPersonOnline != null && glbPersonOnline.Rol != 0) {
            if (glbPersonOnline.ViewModel.HolderIdentificationDocument != null )
                Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.ViewModel.HolderIdentificationDocument, glbPersonOnline.CustomerType, null);
            else
                Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.ViewModel.HolderName, 1, null);
        } else
        {
            glbPersonOnline = null;
        }
    }

    bindEvents() {
        $('#inputQuotation').on('buttonClick', this.Quotation);
        $("#inputQuotation").on("keydown", this.QuotationKey);
        $("#inputHolder").on('buttonClick', this.SearchHolder);
        $("#btnNewVersion").click(UnderwritingQuotation.CreateNewVersionQuotation);
        $("#btnNewQuotation").click(UnderwritingQuotation.NewQuotation);
        $("#btnSaveTempQuotation").click(this.SaveTempQuotation);
        $("#btnSaveQuotation").click(this.SaveQuotation);
        $("#btnIssuePolicy").click(UnderwritingQuotation.ValidateAndIssuePolicy);
    }

    Quotation() {
        $('#inputQuotationbutton').prop('disabled', true);
        UnderwritingQuotation.GetPoliciesByQuotationIdVersionPrefixId($('#inputQuotation').val().trim(), 0, 0, 0);
    }

    QuotationKey(event) {
        if (event.which == 13) {
            $('#inputQuotationbutton').prop('disabled', true);
            UnderwritingQuotation.GetPoliciesByQuotationIdVersionPrefixId($('#inputQuotation').val().trim(), 0, 0, 0);
            $('#inputQuotationbutton').prop('disabled', false);

        }
    }

    SearchHolder() {
        if ($("#inputHolder").val().trim().length > 0) {
            Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputHolder").val().trim(), InsuredSearchType.DocumentNumber, null);
        }
    }

    static NewQuotation() {
        ScrollTop();

        UnderwritingQuotation.ClearControls();
    }

    SaveTempQuotation() {
        glbPolicy.TemporalType = TemporalType.TempQuotation;
        Underwriting.SaveTemporal(true);
    }

    static EnabledTempButtonsTemp(Id) {
        glbPolicy.TemporalType = TemporalType.TempQuotation;
        $("#inputQuotation").val("No. Temporal " + Id);
        if (Id > 0) {
            $("#btnNewVersion").prop('disabled', true);
            $("#btnSaveQuotation").prop('disabled', false);
            $("#btnIssuePolicy").prop('disabled', true);
        }
        else {
            $("#btnNewVersion").prop('disabled', true);
            $("#btnSaveQuotation").prop('disabled', true);
        }
        ScrollTop();
    }

    static EnabledButtonsSaveQuotetion(Id) {
        glbPolicy.TemporalType = TemporalType.Quotation;
        $("#inputQuotation").val("No. cotización " + Id);
        if (Id > 0) {
            $("#btnNewVersion").prop('disabled', false);
            $("#btnSaveQuotation").prop('disabled', true);
            $("#btnSaveTempQuotation").prop('disabled', true);
            $("#btnIssuePolicy").prop('disabled', false);
        }
        else {
            $("#btnNewVersion").prop('disabled', true);
            $("#btnSaveQuotation").prop('disabled', true);
        }
        ScrollTop();
    }


    SaveQuotation() {
        if (glbPolicy.Summary.RiskCount == 0) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyRisk, 'autoclose': true });
        }
        else {
            glbPolicy.TemporalType = TemporalType.Quotation;
            if (glbPolicy.Endorsement.QuotationId == 0) {
                glbPolicy.Endorsement.QuotationId = -1;
            }
            Underwriting.SaveTemporal(true);
            //$("#inputQuotation").val(glbPolicy.Id);
            $('#btnIssuePolicy').prop('disabled', false);
            ScrollTop();
        }
    }

    static GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId) {
        if (quotationId > 0) {
            QuotationRequest.GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId).done(function (data) {
                $('#inputQuotationbutton').prop('disabled', false);
                if (data.success) {
                    if (data.result.length == 1) {
                        glbPolicy = data.result[0];
                        Underwriting.LoadTemporal(glbPolicy);
                        $('#btnIssuePolicy').prop('disabled', false);
                        $('#btnSaveQuotation').prop('disabled', false);
                        $('#btnIssuePolicy').prop('disabled', true);
                    }
                    else if (data.result.length > 1) {
                        QuotationAdvancedSearch.showDropDownQuotationAdv();
                        QuotationAdvancedSearch.fillListAdvSearchQuotation(data.result);
                    }
                }
                else {
                    UnderwritingQuotation.ClearControls();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $('#inputQuotationbutton').prop('disabled', false);
                UnderwritingQuotation.ClearControls();
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }

    static ClearControls() {
        glbPolicy = { Id: 0, TemporalType: TemporalType.Quotation, Endorsement: { EndorsementType: EndorsementType.Emission, QuotationVersion: 1, QuotationId: 0 } };
        $('#btnSaveTempQuotation').prop('disabled', false);
        $('#btnIssuePolicy').prop('disabled', true);
        $('#btnNewVersion').prop('disabled', true);
        $("#inputQuotation").val("");
        Underwriting.ClearControls();

    }

    static CreateNewVersionQuotation() {
        if (glbPolicy.Id > 0) {
            $("#formUnderwriting").validate();
            if ($("#formUnderwriting").valid()) {
                QuotationRequest.CreateNewVersionQuotation(glbPolicy.Id).done(function (data) {
                    if (data.success) {
                        glbPolicy = data.result;
                        Underwriting.LoadTemporal(glbPolicy);
                        //
                        glbPolicy.TemporalType = TemporalType.TempQuotation;
                        Underwriting.SaveTemporal(true);
                        $("#btnSaveTempQuotation").prop('disabled', false);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelVersion + ' ' + AppResources.MessageCreate, 'autoclose': true });
                    }
                    else {
                        UnderwritingQuotation.ClearControls();
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    UnderwritingQuotation.ClearControls();
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCreateNewVersionQuotation, 'autoclose': true });
                });
            }
        }
    }

    static IssuePolicy() {
        $("#formUnderwriting").validate();
        if ($("#formUnderwriting").valid()) {
            var policyData = Underwriting.GetPolicyModel(false);
            UnderwritingRequest.SaveTemporal(policyData, dynamicProperties, policyData.polities).done(function (data) {
                if (data.success) {

                    Underwriting.UpdateGlbPolicy(data.result);
                    Underwriting.LoadTitle(glbPolicy);
                    Underwriting.LoadSubTitles(0);
                    UnderwritingQuotation.UpdateRisksQuotation();
                }
                else {
                    //$.UifProgress('close');
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                //$.UifProgress('close');
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
            });
        }
    }

    static ValidateAndIssuePolicy() {
        if (glbPolicy != null && glbPolicy.Id != 0 && glbPolicy.Endorsement != null && glbPolicy.Endorsement.QuotationId != 0) {
            // $.UifProgress('show');
            lockScreen();
            if (UnderwritingQuotation.ValidateHolder() && UnderwritingQuotation.ValidateMainInsured()) {

                QuotationRequest.ValidateQuotation(glbPolicy.Endorsement.QuotationId, glbPolicy.Prefix.Id, glbPolicy.Branch.Id).done(function (data) {
                    if (data.success) {
                        if (data.result == null) {
                            UnderwritingQuotation.IssuePolicy();
                            //$.UifProgress('close');
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }
                    else {
                        //$.UifProgress('close');
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });


            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotation, 'autoclose': true });
        }
    }

    static ValidateMainInsured() {
        return true;
        if (glbPolicy.Summary.RisksInsured.length > 0) {
            UnderwritingRequest.ValidateMainInsured(glbPolicy.Summary.RisksInsured).done(function (data) {
                if (!data.success) {
                    $.UifNotify('show', {
                        'type': 'danger', 'message': AppResources.InsuredNotPerson, 'autoclose': true
                    });
                    isPerson = false;
                } 
                $.each(data1, function (index, value) {
                    if (value.Beneficiaries[0].CustomerType === 2) {
                        $.UifNotify('show', {
                            'type': 'danger', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true
                        });
                    }
                });
            }).fail(function (jqXHR, textStatus, errorThrown) {
                isPerson = false;
            });

            return isPerson;
        }
    }

    static validateProspect() {
        if (glbPolicy.Holder.CustomerType == 2) {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResources.ErrorPolicyholderWithoutRol + ' , ' + AppResources.ErrorholderWithoutRol, 'autoclose': true
            });
        }
        if (glbPolicy.Summary.RisksInsured != null) {
            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Holder.CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResources.InsuredNotPerson, 'autoclose': true
                });
            }

            if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType == 2) {
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true
                });
            }
        }
        
    }

    static ValidateHolder() {
        if (glbPolicy.Holder.InsuredId != 0) {
            UnderwritingQuotation.validateProspect();            
            return true;
        }
        else {
            //Validacion para prospectos 
            if (glbPolicy.Holder.CustomerType == CustomerType.Prospect) {
                $("#btnConvertProspect").show();
                $.UifNotify('show', {
                    'type': 'info', 'message': AppResources.ErrorPolicyholderWithoutRol + ' , ' + AppResources.ErrorholderWithoutRol, 'autoclose': true
                });
                return true;
            }
            else {

                //Validacion para persona juridica o natural sin ningun rol
                $.UifDialog('confirm', {
                    'message': AppResources.ErrorPolicyholderWithoutRol + ' ' + AppResources.WantCreateInsured
                }, function (result) {
                    if (result) {
                        var individualId = glbPolicy.Holder.IndividualId;
                        DateNowPerson = GetCurrentFromDate();
                        var insuredTmp = Insured.CreateInsuredConsortiumModel(individualId);
                        InsuredRequest.CreateInsuredConsortium(insuredTmp).done(function (data) {
                            if (data.success) {
                                glbPolicy.Holder.InsuredId = data.result.IndividualId;
                                UnderwritingQuotation.ValidateAndIssuePolicy();
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                });
            }            

        }
    }

    static validateProspect() {
        if (glbPolicy.Holder.CustomerType == 2) {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResources.ErrorPolicyholderWithoutRol + ' , ' + AppResources.ErrorholderWithoutRol, 'autoclose': true
            });
        }

        if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Holder.CustomerType == 2) {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResources.InsuredNotPerson, 'autoclose': true
            });
        }

        if (glbPolicy.Summary != null && glbPolicy.Summary.RisksInsured.length > 0 && glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType == 2) {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResources.BeneficiaryNotPerson, 'autoclose': true
            });
        }
    }

    static DisabledButtonsQuotation() {
        var enabledButton = false;
        if (glbPolicy.TemporalType == 1) {
            enabledButton = true;
        }
        if (enabledButton) {
            $('.card-button.edit-button').hide();
            $('.card-button.delete-button').hide();
        }
        $("#btnCoInsuranceSave").prop('disabled', enabledButton);
        $("#btnAgentsSave").prop('disabled', enabledButton);
        $("#btnPaymentPlanSave").prop('disabled', enabledButton);
        $("#btnClausesSave").prop('disabled', enabledButton);
        $("#btnBeneficiarySave").prop('disabled', enabledButton);
        if (riskController == "RiskSurety") {
            if (glbRisk != null && glbRisk.Id != null && glbRisk.Id > 0) {
                $("#btnRiskAccept").prop('disabled', enabledButton);
                $("#btnTextSave").prop('disabled', enabledButton);
                $("#btnContractObjectSave").prop('disabled', enabledButton);
                $("#btnClausesSave").prop('disabled', enabledButton);
                $("#btnCrossGuaranteesSave").prop('disabled', enabledButton);
                $("#btnCrossGuaranteesNew").prop('disabled', enabledButton);
                $("#btnAddRiskSurety").prop('disabled', enabledButton);
                $("#btnDeleteRisk").prop('disabled', enabledButton);
                $("#selectRiskSuretyGroupCoverage").prop('disabled', enabledButton);
                $("#inputContractValue").prop('disabled', enabledButton);
                $("#selectContractType").prop('disabled', enabledButton);
                $("#selectClassofContract").prop('disabled', enabledButton);
            }
        }
        if (riskController == "RiskVehicle") {
            $("#btnAccept").prop('disabled', enabledButton);
            $("#btnAdditionalDataSave").prop('disabled', enabledButton);
            $("#btnTextSave").prop('disabled', enabledButton);
            $("#btnClausesSave").prop('disabled', enabledButton);
            $("#btnSaveCoverage").prop('disabled', enabledButton);
            $("#btnAddRisk").prop('disabled', enabledButton);
            $("#btnDeleteRisk").prop('disabled', enabledButton);
            $("#selectGroupCoverage").prop('disabled', enabledButton);
        }
        if (riskController == "RiskLiability" || riskController == 'RiskProperty' || riskController == 'RiskThirdPartyLiability' || riskController == 'RiskTransport') {
            $("#btnAccept").prop('disabled', enabledButton);
            $("#btnAddRisk").prop('disabled', enabledButton);
            $("#btnTextSave").prop('disabled', enabledButton);
            $("#btnDeleteRisk").prop('disabled', enabledButton);
            $("#btnAcceptCoverage").prop('disabled', enabledButton);
            $("#btnSaveCoverage").prop('disabled', enabledButton);
            if (riskController == 'RiskProperty' || riskController == 'RiskThirdPartyLiability')
                $("#btnAdditionalDataSave").prop('disabled', enabledButton);
        }
    }

    static CreateTemporalEmissionFromQuotation() {
        QuotationRequest.CreateTemporalFromQuotation(glbPolicy.Id).done(function (data) {
            if (data.success) {
                //glbPolicy.TemporalType = TemporalType.Policy;
                glbPolicy = data.result;

                $.UifNotify('show', { 'type': 'info', 'message': "Su temporal de emision es " + data.result.Id.toString() + ".", 'autoclose': true });
                router.run("prtTemporal");
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConvertQuotation, 'autoclose': true });
        });

    }
    static UpdateRisksQuotation() {
        if (glbPolicy.Id > 0) {
            UnderwritingRequest.UpdateRisks(glbPolicy.Id, Underwriting.GetControllerRisk()).done(function (data) {
                if (data.success) {
                    glbPolicy = data.result;
                    Underwriting.LoadTemporal(glbPolicy);
                    UnderwritingQuotation.CreateTemporalEmissionFromQuotation();
                }
                else {
                    Underwriting.ClearControls();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }

    static ValidateBeneficiary() {
        Underwriting.ValidateBeneficiary(glbPolicy.Holder.IndividualId, glbPolicy.Holder.IdentificationDocument.Number);
    }
}