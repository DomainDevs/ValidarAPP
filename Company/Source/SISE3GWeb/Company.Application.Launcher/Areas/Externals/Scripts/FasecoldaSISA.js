class FasecoldaSISA extends Uif2.Page {
    getInitialState() {
        $("#inputPlate").TextTransform(ValidatorType.UpperCase);
        $("#inputEngine").TextTransform(ValidatorType.UpperCase);
        $("#inputChassis").TextTransform(ValidatorType.UpperCase);
    }

    bindEvents() {
        //        $('#btnReloadCaptcha').click(FasecoldaSISA.ReloadCaptcha);
        //        $('#inputVerificationCode').focusout(FasecoldaSISA.ValidateCaptcha);
        $('#btnSearch').click(FasecoldaSISA.SearchFasecolda);
        $('#tablePolicyInfoResults').on('rowSelected', this.TablePolicyLoad);
        $('#tableClaimsResults').on('rowSelected', this.TableClaimLoad);
        $('#btnExitPolicyDetail').click(this.ExitPolicyDtl);
        $('#btnExitClaimsDetail').click(this.ExitClaimsDtl);
        $('#btnExitSISA').click(this.ExitSISA);
    }

    ExitPolicyDtl() {
        $('#ModalDetailInfo').UifModal('hide');
    }

    ExitClaimsDtl() {
        $('#ModalClaimDetail').UifModal('hide');
    }

    ExitSISA() {
        window.location = rootPath + "Home/Index";
    }

    TablePolicyLoad() {
        var policy = $("#tablePolicyInfoResults").UifDataTable("getSelected");
        if (policy != null) {
            FasecoldaSISA.CleanInfoPolicy();
            $('#inputCompanyDtl').val(policy[0].CompanyName);
            $('#inputPolicyNumberDtl').val(policy[0].PolicyNumber);
            $('#inputOrderDtl').val(policy[0].Order);
            $('#inputStatusDtl').val(Resources.Language.LabelValid + ' - ' + policy[0].Valid);
            $('#inputStartValidDtl').val(FormatDate(policy[0].EffectiveDate));
            $('#inputEndValidDtl').val(FormatDate(policy[0].EndEffectiveDate));
            $('#inputPlateDtl').val(policy[0].Plate);
            $('#inputBrandDtl').val(policy[0].Brand);
            $('#inputClassDtl').val(policy[0].Class);
            $('#inputModelDtl').val(policy[0].Model);
            $('#inputEngineDtl').val(policy[0].Engine);
            $('#inputChassisDtl').val(policy[0].Chassis);
            $('#inputTypeDtl').val(policy[0].Type);
            $('#inputDocTypeIsdDtl').val(policy[0].InsuredTypeDocument);
            $('#inputDocNumberIsdDtl').val(policy[0].DocumentNumber);
            $('#inputNameIsdDtl').val(policy[0].Insured);
            $('#inputDocTypeHdrDtl').val(policy[0].HolderTypeDocument);
            $('#inputDocNumberHdrDtl').val(policy[0].HolderDocumentNumber);
            $('#inputNameHdrDtl').val(policy[0].HolderName);
            $('#inputDocTypeBfrDtl').val(policy[0].BeneficiaryTypeDocument);
            $('#inputDocNumberBfrDtl').val(policy[0].BeneficiaryDocumentNumber);
            $('#inputNameBfrDtl').val(policy[0].BeneficiaryName);
            $('#inputPTDDtl').val(policy[0].PTD);
            $('#inputPPDDtl').val(policy[0].PPD);
            $('#inputPHDtl').val(policy[0].PH);
            $('#inputPPHDtl').val(policy[0].PPD);
            $('#inputRCDtl').val(policy[0].RC);
            $('#inputAmountDtl').val(policy[0].InsuredAmount);
            $('#ModalDetailInfo').UifModal('showLocal', 'Detalle póliza');
        }
    }

    TableClaimLoad() {
        var claim = $("#tableClaimsResults").UifDataTable("getSelected");
        if (claim != null) {
            FasecoldaSISA.CleanInfoClaim();
            $('#inputNumClaimDtl').val(claim[0].ClaimNumber);
            $('#inputCompanyClaimDtl').val(claim[0].CompanyName);
            $('#inputPolicyNumberClaimDtl').val(claim[0].PolicyNumber);
            $('#inputClaimDateDtl').val(FormatDate(claim[0].ClaimDate));
            $('#inputOrderClaimDtl').val(claim[0].Order);
            $('#inputPlateClaimDtl').val(claim[0].Plate);
            $('#inputBrandClaimDtl').val(claim[0].Brand);
            $('#inputClassClaimDtl').val(claim[0].Class);
            $('#inputModelClaimDtl').val(claim[0].Model);
            $('#inputEngineClaimDtl').val(claim[0].Engine);
            $('#inputChassisClaimDtl').val(claim[0].Chassis);
            $('#inputTypeClaimDtl').val(claim[0].Type);
            $('#inputDocTypeIsdClaimDtl').val(claim[0].InsuredTypeDocument);
            $('#inputDocNumberIsdClaimDtl').val(claim[0].DocumentNumber);
            $('#inputNameIsdClaimDtl').val(claim[0].Insured);
            $('#inputDocTypeHdrClaimDtl').val(claim[0].HolderTypeDocument);
            $('#inputDocNumberHdrClaimDtl').val(claim[0].HolderDocumentNumber);
            $('#inputNameHdrClaimDtl').val(claim[0].HolderName);
            $('#inputDocTypeBfrClaimDtl').val(claim[0].BeneficiaryTypeDocument);
            $('#inputDocNumberBfrClaimDtl').val(claim[0].BeneficiaryDocumentNumber);
            $('#inputNameBfrClaimDtl').val(claim[0].BeneficiaryName);
            $('#inputAmountIsdClaimDtl').val(claim[0].InsuredAmount);
            $('#tableProtectionsResults').UifDataTable('addRow', claim[0].Protection);
            //$('#inputTotalClaimDtl').val(claim[0].InsuredAmount);
            $('#ModalClaimDetail').UifModal('showLocal', 'Detalle Siniestro');
        }
    }

    static CleanInfoPolicy() {
        $('#inputCompanyDtl').val('');
        $('#inputPolicyNumberDtl').val('');
        $('#inputOrderDtl').val('');
        $('#inputStatusDtl').val('');
        $('#inputStartValidDtl').val('');
        $('#inputEndValidDtl').val('');
        $('#inputPlateDtl').val('');
        $('#inputBrandDtl').val('');
        $('#inputClassDtl').val('');
        $('#inputModelDtl').val('');
        $('#inputEngineDtl').val('');
        $('#inputChassisDtl').val('');
        $('#inputTypeDtl').val('');
        $('#inputDocTypeIsdDtl').val('');
        $('#inputDocNumberIsdDtl').val('');
        $('#inputNameIsdDtl').val('');
        $('#inputDocTypeHdrDtl').val('');
        $('#inputDocNumberHdrDtl').val('');
        $('#inputNameHdrDtl').val('');
        $('#inputDocTypeBfrDtl').val('');
        $('#inputDocNumberBfrDtl').val('');
        $('#inputNameBfrDtl').val('');
        $('#inputPTDDtl').val('');
        $('#inputPPDDtl').val('');
        $('#inputPHDtl').val('');
        $('#inputPPHDtl').val('');
        $('#inputRCDtl').val('');
        $('#inputAmountDtl').val('');
    }

    static CleanInfoClaim() {
        $('#inputNumClaimDtl').val('');
        $('#inputCompanyClaimDtl').val('');
        $('#inputPolicyNumberClaimDtl').val('');
        $('#inputClaimDateDtl').val('');
        $('#inputOrderClaimDtl').val('');
        $('#inputPlateClaimDtl').val('');
        $('#inputBrandClaimDtl').val('');
        $('#inputClassClaimDtl').val('');
        $('#inputModelClaimDtl').val('');
        $('#inputEngineClaimDtl').val('');
        $('#inputChassisClaimDtl').val('');
        $('#inputTypeClaimDtl').val('');
        $('#inputDocTypeIsdClaimDtl').val('');
        $('#inputDocNumberIsdClaimDtl').val('');
        $('#inputNameIsdClaimDtl').val('');
        $('#inputDocTypeHdrClaimDtl').val('');
        $('#inputDocNumberHdrClaimDtl').val('');
        $('#inputNameHdrClaimDtl').val('');
        $('#inputDocTypeBfrClaimDtl').val('');
        $('#inputDocNumberBfrClaimDtl').val('');
        $('#inputNameBfrClaimDtl').val('');
        $('#inputAmountIsdClaimDtl').val('');
        $('#inputTotalClaimDtl').val('');
        $('#tableProtectionsResults').UifDataTable('clear');
    }

    //static ReloadCaptcha() {
    //    $("#captchaImage").attr("src", $("#captchaImage").attr("src") + '?' + Math.random());
    //}

    static GetTextCaptcha() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Externals/FasecoldaSISA/GetTextCaptcha',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    //static ValidateCaptcha() {
    //    if ($("#inputVerificationCode").val() == "") {
    //        $('#btnSearch').attr("disabled", true)
    //    } else {
    //        FasecoldaSISA.GetTextCaptcha().done(function (data) {
    //            if (data.success) {
    //                if (data.result == $("#inputVerificationCode").val()) {
    //                    $('#btnSearch').attr("disabled", false)
    //                }
    //                else {
    //                    $.UifNotify('show', {
    //                        'type': 'info', 'message': Resources.Language.MSGWRN_CAPTCHA, 'autoclose': true
    //                    });
    //                }
    //            }
    //            else {
    //                $.UifNotify('show', {
    //                    'type': 'info', 'message': data.result, 'autoclose': true
    //                });
    //            }
    //        });
    //    }
    //}

    static SearchFasecolda() {
        let plate = $('#inputPlate').val();
        let engine = $('#inputEngine').val();
        let chassis = $('#inputChassis').val();
        if (plate != "") {
            if ($('#formFasecolda').valid()) {
                FasecoldaSISA.CleanForm();
                $('#inputPlate').attr('disabled', true);
                $('#inputEngine').attr('disabled', true);
                $('#inputChassis').attr('disabled', true);
                $('#btnSearch').attr('disabled', true);

                return $.ajax({
                    type: 'POST',
                    url: rootPath + 'Externals/FasecoldaSISA/GetFasecoldaInfo',
                    data: JSON.stringify({ plate: plate, engine: engine, chassis: chassis }),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                }).done(function (data) {
                    if (data.success) {
                        if (data.result.PoliciesInfo.length > 0)
                            $('#tablePolicyInfoResults').UifDataTable('addRow', data.result.PoliciesInfo);
                        else
                            $('#tablePolicyInfoResults').UifDataTable({ sourceData: data.result.PoliciesInfo });
                        if (data.result.Claims.length > 0)
                            $('#tableClaimsResults').UifDataTable('addRow', data.result.Claims);
                        else
                            $('#tableClaimsResults').UifDataTable({ sourceData: data.result.Claims });
                        $('#tableGuiedValuesResults').UifDataTable('addRow', data.result.GuideInfo);
                        $('#tableVINVerificationResults').UifDataTable('addRow', data.result.VINVerification);
                        $('#linkPromissoryNote').click();
                    } else {
                        $.UifNotify('show', {
                            'type': 'danger', 'message': data.result, 'autoclose': true
                        });
                    }
                    $('#inputPlate').removeAttr('disabled');
                    $('#inputEngine').removeAttr('disabled');
                    $('#inputChassis').removeAttr('disabled');
                    $('#btnSearch').removeAttr('disabled');

                });
            }
        }
        else {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorDocumentPlate, 'autoclose': true
            });
        }
    }

    static CleanForm() {
        $('#inputVerificationCode').val('');
        $('#tablePolicyInfoResults').UifDataTable('clear');
        $('#tableClaimsResults').UifDataTable('clear');
        $('#tableGuiedValuesResults').UifDataTable('clear');
        $('#tableVINVerificationResults').UifDataTable('clear');
        //$('#btnSearch').attr("disabled", true)
        //FasecoldaSISA.ReloadCaptcha();
    }
}