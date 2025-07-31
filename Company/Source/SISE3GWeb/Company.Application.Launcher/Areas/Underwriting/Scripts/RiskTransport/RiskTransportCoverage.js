var individualSearchType = 1;
var IndividualType;
var riskControllerCoverage;
var dynamicProperties = null;
var insuredObjectId = null;
var coverageDeductibles = null;
var idRate = null;
var pressBtnAcept = false;
var coverageData = { Id: 0 };
var coverageIndex = null;
var coverageText = null;
var coverageClauses = null;
var EditSubLineBusiness = null;
var coverageNumber = null;
var ModelCoverageEdit = {};
var saveCover = false;

class RiskTransportCoverage extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        lockScreen();
        riskControllerCoverage = 'RiskTransportCoverage';
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskTransportCoverage", formTransportCoverage: "#formTransportCoverage", RecordScript: false, Class: RiskTransportCoverage };
        }
        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputFromCoverage").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputToCoverage").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputDeclaredAmount").ValidatorKey(1, 1, 1);
        $('#inputLimitAmount').ValidatorKey(1, 1, 1);
        $('#inputSubLimitAmount').ValidatorKey(1, 1, 1);
        $('#inputMaxLiabilityAmount').ValidatorKey(1, 1, 1);
        $('#inputLimitOccurrenceAmount').ValidatorKey(1, 1, 1);
        $('#inputLimitClaimantAmount').ValidatorKey(1, 1, 1);
        $('#inputPremiumAmount').ValidatorKey(1, 1, 1);
        $('#inputInsuredLimit').ValidatorKey(1, 1, 1);
        $("#modalTexts").hide();
        $("#modalClauses").hide();

        $("#inputInsuredDepositPremiumPercent").val(0);
        $("#inputInsuredLimit").val(0);
        $("#inputDeclaredAmount").val(0);
        $("#inputLimitAmount").val(0);
        $("#inputSubLimitAmount").val(0);
        $("#inputMaxLiabilityAmount").val(0);
        $("#inputLimitOccurrenceAmount").val(0);
        $("#inputLimitClaimantAmount").val(0);
        $("#inputDepositPremiumPercentAmount").val(0);
        $('#inputRateAmount').val();
        $("#inputPremiumAmount").val(0);
        if (!IsEditing) {
            RiskTransportCoverage.GetInsuredRateTypes();
        }
        saveCover = false;
        RiskTransportCoverage.GetCalculationTypes();
        RiskTransportCoverage.GetRateTypes(function (rateTypes) {
            $("#selectRateType").UifSelect({
                sourceData: rateTypes
            });
            RiskTransportCoverage.DisableControls(glbPolicy.Endorsement.EndorsementType);
        });
        RiskTransportCoverage.LoadTemporal(glbPolicy);
        if (policyIsFloating) {
            $("#chkIsDeclarative").parent().show();
            $("#chkIsDeclarative").prop('disabled', true);
            $("#chkIsDeclarative").prop('checked', true);
            $("#inputInsuredDepositPremiumPercent").prop('disabled', false);
        }
        else {
            $("#chkIsDeclarative").parent().hide();
            $("#chkIsDeclarative").prop('checked', false);
            $("#inputInsuredDepositPremiumPercent").prop('disabled', true);
            $("#inputDepositPremiumPercentAmount").prop('disabled', true);
        }
        if (IsEditing) {
            RiskTransportCoverage.GetTemporalCoveragesByRiskIdInsuredObjectId(glbRisk.Id, glbCoverage.CoverageId)
            RiskTransportCoverage.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, glbRisk.CoverageGroupId, glbPolicy.Prefix.Id);
            pressBtnAcept = true;
        } else {
            RiskTransportCoverage.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, glbRisk.CoverageGroupId, glbPolicy.Prefix.Id);
        }

        RiskTransportCoverage.DisableControls(glbPolicy.Endorsement.EndorsementType);
        
    }

    static ValidateInsudedValue() {
        var holderType = glbRisk.HolderTypeId;
        var insuredObjectValue = NotFormatMoney($('#inputInsuredLimit').val());
        var value;
        switch (holderType) {
            case HolderType.Generador:
                value = glbRisk.ReleaseAmount;
                break;
            case HolderType.Transportador:
                value = glbRisk.FreightAmount;
                break;
            default:
        }
        if (insuredObjectValue <= 0) {
            $('#inputInsuredLimit').val(FormatMoney(value));
        }

    }

    static LoadTemporal(policyData) {
        $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(policyData.CurrentFrom));
        $("#inputToCoverage").UifDatepicker('setValue', FormatDate(policyData.CurrentTo));
        RiskTransportCoverage.CalculateDays();
    }
    bindEvents() {
        $('#inputDeclaredAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputLimitAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputSubLimitAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputLimitClaimantAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputMaxLiabilityAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputRateCoverage').OnlyDecimals(3);
        $('#inputPremiumCoverage').OnlyDecimals(UnderwritingDecimal);
        $('#inputLimitOccurrenceAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputInsuredLimit').OnlyDecimals(UnderwritingDecimal);
        $('#inputInsuredDepositPremiumPercent').OnlyDecimals(UnderwritingDecimal);
        $('#inputDepositPremiumPercentAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputInsuretRate').OnlyDecimals(UnderwritingDecimal);
        $('#inputRateAmount').OnlyDecimals(UnderwritingDecimal);
        $('#selectInsuredObject').on('itemSelected', this.GetInsuredObjectId);
        $('#selectInsuredRate').on('itemSelected', this.GetInsuredRate);
        $('#selectRateType').on('itemSelected', this.ResetRateValue);
        $('#selectCalculationType').on('itemSelected', this.ResetRateValue);
        $("#inputFromCoverage").on('datepicker.change', this.ChangeFrom);
        $("#inputToCoverage").on('datepicker.change', this.ChangeTo);
        $('#btnAcceptObject').click(this.ShowDetail);
        $('#btnNewCoverage').click(this.newCoverage);
        $('#btnAcceptCoverage').on('click', RiskTransportCoverage.AddCoverage);
        $('#btnSaveCoverage').on('click', RiskTransportCoverage.SaveCoverages);
        $('#listCoveragesEdit').on('rowEdit', this.EditCoverage);
        $('#listCoveragesEdit').on('rowDelete', this.CoverageDelete);
        $("#btnCloseCoverage").on("click", this.CloseCoverage);
        $('#inputInsuredDepositPremiumPercent').on('focusin', this.inputMoneyOnFocusin);
        $('#inputInsuretRate').on('focusin', this.inputMoneyOnFocusin);
        $('#inputLimitAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#inputSubLimitAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#inputMaxLiabilityAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#inputLimitOccurrenceAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#inputLimitClaimantAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#inputRateAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#inputDepositPremiumPercentAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#inputDepositPremiumPercentAmount').focusout(this.assignZero);
        $('#inputInsuredDepositPremiumPercent').focusout(this.assignZero);
        $('#inputLimitAmount').focusout(this.assignZero);
        $('#inputSubLimitAmount').focusout(this.assignZero);
        $('#inputMaxLiabilityAmount').focusout(this.assignZero);
        $('#inputLimitOccurrenceAmount').focusout(this.assignZero);
        $('#inputLimitClaimantAmount').focusout(this.assignZero);
        $('#inputRateAmount').focusout(this.assignZero);
        $('#inputRateAmount').on('focusin', this.inputMoneyOnFocusin);
        $('#selectCoverage').on('itemSelected', this.selectCoverageItemselected);
        $('#selectDeductible').on('itemSelected', this.Deductibles);
        $('#inputInsuretRate').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));

            if ($('#selectInsuredRate').UifSelect('getSelected') == RateType.FixedValue/* 'Importe'*/) {
                var limitInsured = parseInt(NotFormatMoney($('#inputInsuredLimit').val()));
                var valRate = parseInt($(this).val());
                if (limitInsured < valRate) {
                    $(this).val(0);
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ImportAboveLimitAmmount, 'autoclose': true });
                } else {
                    $(this).val(FormatMoney(valRate));
                }
            } else {
                var text = $(this).val().split(',');
                if (text.length == 2) {
                    if (parseInt(text[0]) > 100 || parseInt(text[0]) >= 100 && parseInt(text[1]) > 0) {
                        $(this).val(0);
                    }
                } else {
                    if (parseInt(text[0]) > 100) {
                        $(this).val(0);
                    }
                }
            }
        });

        $('#inputInsuredDepositPremiumPercent').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputInsuredDepositPremiumPercent').focusout(function () {
            var text = $(this).val().split(',');
            if (text.length == 2) {
                if (parseInt(text[0]) > 100 || parseInt(text[0]) >= 100 && parseInt(text[1]) > 0) {
                    $(this).val(0);
                }
            } else {
                if (parseInt(text[0]) > 100) {
                    $(this).val(0);
                }
            }
        });

        $('#inputInsuredLimit').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputInsuredLimit').focusout(function (event) {
            event.stopPropagation();
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });
        $('#inputDeclaredAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputDeclaredAmount').focusout(function (event) {
            event.stopPropagation();
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            RiskTransportCoverage.UpdateAmounts();
        });
        $('#inputLimitAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });
        $('#inputSubLimitAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputSubLimitAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#inputLimitClaimantAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitClaimantAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });
        $('#inputMaxLiabilityAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputMaxLiabilityAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });
        $('#inputLimitOccurrenceAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitOccurrenceAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#inputDepositPremiumPercentAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputDepositPremiumPercentAmount').focusout(function () {
            event.stopPropagation();
            var text = $(this).val().split(',');
            if (text.length == 2) {
                if (parseInt(text[0]) > 100 || parseInt(text[0]) >= 100 && parseInt(text[1]) > 0) {
                    $(this).val(0);
                }
            } else {
                if (parseInt(text[0]) > 100) {
                    $(this).val(0);
                }
            }
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            RiskTransportCoverage.QuotationCoverage();
        });


        $('#inputRateAmount').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
            if ($('#selectRateType').UifSelect('getSelectedText') == 'Importe') {
                var limitInsured = parseInt(NotFormatMoney($('#inputDeclaredAmount').val()));
                var text = parseInt($(this).val());
                if (limitInsured < text) {
                    $(this).val(0);
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ImportAboveLimitAmmount, 'autoclose': true });
                }
            } else {
                var value = NotFormatMoney($.trim($(this).val()));
                value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
                var text = $(this).val().split(',');
                if (text.length == 2) {
                    if (parseInt(text[0]) > 100 || parseInt(text[0]) >= 100 && parseInt(text[1]) > 0) {
                        $(this).val(0);
                    }
                } else {
                    if (parseInt(text[0]) > 100) {
                        $(this).val(0);
                    }
                }
            }
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                var text = $(this).val().split(',');
                if (text.length == 2) {
                    if (parseInt(text[0]) > 100 || parseInt(text[0]) >= 100 && parseInt(text[1]) > 0) {
                        $(this).val(0);
                    }
                } else {
                    if (parseInt(text[0]) > 100) {
                        $(this).val(0);
                    }
                }
                var value = NotFormatMoney($.trim($(this).val()));
                value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
                RiskTransportCoverage.QuotationCoverage();
            }
        });
    }
    ResetRateValue() {
        $('#inputRateAmount').val(0);
    }

    assignZero() {
        var value = NotFormatMoney($.trim($(this).val()));
        value == "" ? $(this).val(0) : $(this).val(value);
    }
    inputMoneyOnFocusin() {
        var value = NotFormatMoney($.trim($(this).val()));
        value == 0 ? $(this).val("") : $(this).val(value);
    }

    static GetCoverageModel() {
        //var coverageModel = $("#formTransportCoverage").serializeObject();
        var coverageModel = coverageData;
        coverageModel.Id = $('#hiddenCoverageId').val();
        coverageModel.RuleSetId = $('#hiddenRuleSetId').val();
        coverageModel.PosRuleSetId = $('#hiddenPosRuleSetId').val();
        coverageModel.CoverageId = $("#selectCoverage").UifSelect("getSelected");
        coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.DeclaredAmount = NotFormatMoney($('#inputDeclaredAmount').val());
        coverageModel.LimitAmount = NotFormatMoney($('#inputLimitAmount').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.MaxLiabilityAmount = NotFormatMoney($('#inputMaxLiabilityAmount').val());
        coverageModel.LimitOccurrenceAmount = NotFormatMoney($('#inputLimitOccurrenceAmount').val());
        coverageModel.LimitClaimantAmount = NotFormatMoney($('#inputLimitClaimantAmount').val());
        coverageModel.CalculationTypeId = $('#selectCalculationType').val();
        coverageModel.Rate = NotFormatMoney($('#inputRateAmount').val());
        coverageModel.RateTypeId = $('#selectRateType').val();
        coverageModel.DepositPremiumPercent = NotFormatMoney($('#inputDepositPremiumPercentAmount').val());
        if (coverageModel.DepositPremiumPercent == undefined || coverageModel.DepositPremiumPercent == null) {
            coverageModel.DepositPremiumPercent = 0;
        }
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumAmount').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;
        coverageModel.InsuredObject = $("#formObjectTransport").serializeObject();
        coverageModel.EndorsementType = glbPolicy.Endorsement.EndorsementType;
        //if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
        //    if (coverageIndex == null) {
        //        coverageModel.CoverStatus = 2;
        //        coverageModel.IsMandatory = false;
        //    }
        //    else {
        //        coverageModel.CoverStatus = 4;
        //        coverageModel.IsMandatory = coverageData.IsMandatory;
        //    }
        //}

        coverageModel.SubLineBusiness = EditSubLineBusiness;
        if (policyIsFloating) {
            $("#chkIsDeclarative").parent().show();//Muestra el control
            $("#chkIsDeclarative").prop('disabled', true);//Des habilita el control
            $("#chkIsDeclarative").prop('checked', true);//Deja el control en true
        }
        else {
            $("#chkIsDeclarative").parent().show();//Muestra el control
            $("#chkIsDeclarative").prop('disabled', true);//Des habilita el control
            $("#chkIsDeclarative").prop('checked', false);//Deja el control en true
            $("#chkIsDeclarative").parent().hide();//Oculta el control
        }
        coverageModel.IsDeclarative = $("#chkIsDeclarative").val();

        if ($("#selectDeductible").UifSelect("getSelected") > 0) {
            coverageModel.DeductibleId = $("#selectDeductible").UifSelect("getSelected");
            coverageModel.DeductibleDescription = $("#selectDeductible").UifSelect('getSelectedText');
        }
        if (coverageText != null) {
            coverageModel.Text = coverageText;
        }
        if (coverageClauses != null) {
            coverageModel.Clauses = coverageClauses;
        }
        if ($("#selectDeductible").UifSelect("getSelected") != null && $("#selectDeductible").UifSelect("getSelected") != "") {
            if (coverageDeductibles == null) {
                if ($("#selectDeductible").UifSelect("getSelected") > -1) {
                    coverageModel.Deductible = {
                        Id: $("#selectDeductible").UifSelect("getSelected"),
                        Description: $("#selectDeductible").UifSelect('getSelectedText')
                    };
                    coverageModel = coverage.Deductible;
                }
                else {
                    coverageModel.Deductible = null;
                }
            } else {
                if ($("#selectDeductible").UifSelect("getSelected") == null || $("#selectDeductible").UifSelect("getSelected") == "") {
                    coverageModel.Deductible = null;
                } else {
                    coverageModel.Deductible = coverageDeductibles;
                }
            }
        }
        return coverageModel;
    }
    static UpdatePremium() {
        var insuredAmount = 0;
        var premium = 0;

        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
            insuredAmount += parseFloat(NotFormatMoney(this.LimitAmount).replace(separatorDecimal, separatorThousands));
        });

        $("#inputAmountInsured").text(FormatMoney(insuredAmount.toFixed(2)));
        $("#inputPremium").text(FormatMoney(premium.toFixed(2)));
    }
    static ClearCoverage() {
        $('#selectCoverage').UifSelect();
        $("#selectCalculationType").UifSelect("setSelected", null);
        $("#inputDeclaredAmount").val(0);
        $("#inputLimitAmount").val(0);
        $("#inputSubLimitAmount").val(0);
        $("#inputMaxLiabilityAmount").val(0);
        $("#inputLimitOccurrenceAmount").val(0);
        $("#inputLimitClaimantAmount").val(0);
        $("#inputRateAmount").val(0);
        $("#selectRateType").UifSelect("setSelected", null);
        $('#inputPremiumAmount').val(0);
        $('#inputDepositPremiumPercentAmount').val(0);
        $('#selectDeductible').UifSelect();

        coverageIndex = null;
        coverageText = null;
        coverageClauses = null;
        coverageData = { Id: 0 };
    }
    CloseCoverage() {
        RiskTransportCoverage.ClearCoverage();
        IsEditing = false;
        if (!saveCover) {
            $.UifDialog('confirm', { 'message': Resources.Language.ConfirmSaveModal }, function (result) {
                if (result) {
                    RiskTransportCoverage.SaveCoverages();
                    saveCover = false;
                    router.run("prtRiskTransport");
                } else {
                    router.run("prtRiskTransport");
                }
            });
        } else {
            router.run("prtRiskTransport");
        }

    }

    static ShowPanelsCoverage(Menu) {
        if (isNaN(Menu)) {
            Menu = Menu.data.Menu;
        }
        switch (Menu) {
            case MenuType.Coverage:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('showLocal', AppResources.LabelDataTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Script:
                RiskTransportCoverage.LoadScript();
                break;
            case MenuType.Deductibles:
                $("#modalDeductibles").UifModal('showLocal', AppResources.LabelDeductibles);
                break;
            default:
                break;
        }
    }
    static QuotationCoverage() {
        var Coverage = RiskTransportCoverage.GetCoverageModel();
        var selectedCoverage = $("#selectCoverage").UifSelect('getSelected');
        if (selectedCoverage != "" && selectedCoverage != null) {
            RiskTransportCoverageRequest.QuotationCoverage(Coverage, transportDto, false, true).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        Coverage.Id = $("#selectCoverage").UifSelect('getSelected');
                        Coverage.CoverageId = $("#selectCoverage").UifSelect('getSelected');
                        Coverage.Description = $("#selectCoverage").UifSelect('getSelectedText');
                        Coverage.DeclaredAmount = Coverage.DeclaredAmount;
                        Coverage.DisplayRate = FormatMoney(Coverage.Rate);
                        Coverage.FlatRatePorcentage = FormatMoney(Coverage.FlatRatePorcentage);
                        Coverage.LimitAmount = FormatMoney(data.result.LimitAmount);
                        Coverage.SubLimitAmount = FormatMoney(data.result.SubLimitAmount);
                        Coverage.MaxLiabilityAmount = FormatMoney(data.result.MaxLiabilityAmount);
                        Coverage.LimitOccurrenceAmount = FormatMoney(data.result.LimitOccurrenceAmount);
                        Coverage.LimitClaimantAmount = FormatMoney(data.result.LimitClaimantAmount);
                        Coverage.Rate = FormatMoney(data.result.Rate);
                        Coverage.PremiumAmount = FormatMoney(data.result.PremiumAmount);
                        Coverage.CoverStatusName = data.result.CoverStatusName;
                        Coverage.Text = coverageText;
                        Coverage.Clauses = coverageClauses;
                        Coverage.TypeCoverage = Coverage.TypeCoverage;
                        RiskTransportCoverage.UpdatePremium();
                        $('#inputPremiumAmount').val(Coverage.PremiumAmount);

                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
        }
    }

    static AddCoverage() {

        $("#formTransportCoverage").validate();
        var DeclaredAmount = $("#inputDeclaredAmount").val();
        var LimitAmount = $("#inputLimitAmount").val();
        var SubLimitAmount = $("#inputSubLimitAmount").val();
        var MaxLiabilityAmount = $("#inputMaxLiabilityAmount").val();
        var LimitOccurrenceAmount = $("#inputLimitOccurrenceAmount").val();
        var LimitClaimantAmount = $("#inputLimitClaimantAmount").val();
        var CalculationType = $("#selectCalculationType").val();
        var RateType = $("#selectRateType").val();
        var RateAmount = $("#inputRateAmount").val();
        var selectedCoverage = $("#selectCoverage").UifSelect('getSelected');
        if ($("#formTransportCoverage").valid()) {
            if (DeclaredAmount == "") {
                $("#ReqinputDeclaredAmount").show();
                return;
            } else {
                $("#ReqinputDeclaredAmount").hide();
            }
            if (LimitAmount == "") {
                $("#ReqinputLimitAmount").show();
                return;
            } else {
                $("#ReqinputLimitAmount").hide();
            }
            if (SubLimitAmount == "") {
                $("#ReqinputSubLimitAmount").show();
                return;
            } else {
                $("#ReqinputSubLimitAmount").hide();
            }
            if (MaxLiabilityAmount == "") {
                $("#ReqinputMaxLiabilityAmount").show();
                return;
            } else {
                $("#ReqinputMaxLiabilityAmount").hide();
            }
            if (LimitOccurrenceAmount == "") {
                $("#ReqinputLimitOccurrenceAmount").show();
                return;
            } else {
                $("#ReqinputLimitOccurrenceAmount").hide();
            }
            if (LimitClaimantAmount == "") {
                $("#ReqinputLimitClaimantAmount").show();
                return;
            } else {
                $("#ReqinputLimitClaimantAmount").hide();
            }
            if (CalculationType == "") {
                $("#ReqselectCalculationType").show();
                return;
            } else {
                $("#ReqselectCalculationType").hide();
            }
            if (RateType == "") {
                $("#ReqselectRateType").show();
                return;
            } else {
                $("#ReqselectRateType").hide();
            }
            if (RateAmount == "") {
                $("#ReqinputRateAmount").show();
                return;
            } else {
                $("#ReqinputRateAmount").hide();
            }
            if (selectedCoverage == "") {
                $("#ReqselectCoverage").show();
                return;
            } else {
                $("#ReqselectCoverage").hide();
            }

            var coverage = RiskTransportCoverage.GetCoverageModel();
            var selectedCoverage = $("#selectCoverage").UifSelect('getSelectedSource');

            coverage.IsPrimary = selectedCoverage.IsPrimary;
            coverage.MainCoverageId = selectedCoverage.MainCoverageId
            coverage.AllyCoverageId = selectedCoverage.AllyCoverageId;
            coverage.RuleSetId = selectedCoverage.RuleSetId;
            coverage.PosRuleSetId = selectedCoverage.PosRuleSetId;

            if (coverage.DeclaredAmount <= 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateformCoverage, 'autoclose': true });
            }
            else {
                if (selectedCoverage.Id != "" && selectedCoverage.Id != null) {
                    RiskTransportCoverageRequest.QuotationCoverage(coverage, transportDto, true, true).done(function (data) {
                        if (data.success) {
                            if (data.result != null) {
                                coverage.Id = $("#selectCoverage").UifSelect('getSelected');
                                coverage.CoverageId = $("#selectCoverage").UifSelect('getSelected');
                                coverage.Description = $("#selectCoverage").UifSelect('getSelectedText');
                                coverage.DeclaredAmount = coverage.DeclaredAmount;
                                coverage.DisplayRate = FormatMoney(coverage.Rate);
                                coverage.FlatRatePorcentage = FormatMoney(coverage.FlatRatePorcentage);
                                coverage.LimitAmount = FormatMoney(data.result.LimitAmount);
                                coverage.SubLimitAmount = FormatMoney(data.result.SubLimitAmount);
                                coverage.MaxLiabilityAmount = FormatMoney(data.result.MaxLiabilityAmount);
                                coverage.LimitOccurrenceAmount = FormatMoney(data.result.LimitOccurrenceAmount);
                                coverage.LimitClaimantAmount = FormatMoney(data.result.LimitClaimantAmount);
                                coverage.Rate = FormatMoney(data.result.Rate);
                                coverage.PremiumAmount = FormatMoney(data.result.PremiumAmount);
                                coverage.CoverStatusName = data.result.CoverStatusName;
                                coverage.Text = coverageText;
                                coverage.Clauses = coverageClauses;
                                coverage.TypeCoverage = RiskTransportCoverage.ValidateTypeCoverage(coverage);
                                if (coverageNumber != null) {
                                    coverage.Number = coverageNumber;
                                }
                                if (coverageIndex == null) {
                                    coverageNumber = $("#listCoveragesEdit").UifListView("getData").length;
                                    coverage.Number = coverageNumber + 1;
                                    coverage.SubLineBusiness = $("#selectCoverage").UifSelect("getSelectedSource").SubLineBusiness;
                                    $("#listCoveragesEdit").UifListView("addItem", coverage);
                                }
                                else {
                                    $("#listCoveragesEdit").UifListView("editItem", coverageIndex, coverage);
                                }
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
                            }
                            RiskTransportCoverage.UpdatePremium();
                            RiskTransportCoverage.ClearCoverage();
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
                }
            }
        } else {
            if (selectedCoverage == "") {
                $("#ReqselectCoverage").show();
                return;
            } else {
                $("#ReqselectCoverage").hide();
            }
        }
    }
    newCoverage() {
        if (pressBtnAcept) {
            $("#selectCoverage").UifSelect();
            var coveragesAdd = '';
            $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
                if (coveragesAdd.length > 0) {
                    coveragesAdd += ',';
                }
                coveragesAdd += this.Id;
            });

            RiskTransportCoverageRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, glbRisk.CoverageGroupId, glbPolicy.Prefix.Id, coveragesAdd, $("#selectInsuredObject").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        RiskTransportCoverage.ClearCoverage();
                        $("#selectCalculationType").UifSelect("setSelected", CalculationType.Direct);
                        //$("#selectRateType").UifSelect("setSelected", RateType.Percentage);
                        $("#selectCoverage").UifSelect({ sourceData: data.result });
                        $('#inputDeclaredAmount').val($('#inputInsuredLimit').val());
                        $('#inputLimitAmount').val($('#inputInsuredLimit').val());
                        $('#inputSubLimitAmount').val($('#inputInsuredLimit').val());
                        $('#inputMaxLiabilityAmount').val($('#inputInsuredLimit').val());
                        $('#inputLimitOccurrenceAmount').val($('#inputInsuredLimit').val());
                        $('#inputLimitClaimantAmount').val($('#inputInsuredLimit').val());
                        $("#selectRateType").UifSelect("setSelected", $('#selectInsuredRate').val());
                        $('#inputRateAmount').val(NotFormatMoney($('#inputInsuretRate').val()));
                        $('#inputDepositPremiumPercentAmount').val($('#inputInsuredDepositPremiumPercent').val());
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
            });
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPressAccept, 'autoclose': true });
        }
    }
    static SaveInsuredObject() {
        insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
        insuredObject.InsuredLimitAmount = NotFormatMoney($("#inputInsuredLimit").val());
        insuredObject.DepositPremiumPercentage = $("#inputInsuredDepositPremiumPercent").val();
        insuredObject.Rate = NotFormatMoney($("#inputInsuretRate").val());
        insuredObject.RateTypeId = $("#selectInsuredRate").UifSelect("getSelected");
        insuredObject.Description = $("#selectInsuredObject option:selected").text();
        insuredObject.PremiumAmount = NotFormatMoney($('#inputPremium').text());
        RiskTransportCoverageRequest.SaveInsuredObject(glbRisk.Id, insuredObject, glbPolicy.Id, glbRisk.CoverageGroupId).done(function (data) {
            if (data.success) {
                insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
                RiskTransportCoverage.GetTemporalCoveragesByRiskIdInsuredObjectId(glbRisk.Id, insuredObject.Id);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverage, 'autoclose': true });
        });
    }
    ShowDetail() {
        $("#formObjectTransport").validate();
        if ($("#formObjectTransport").valid()) {
            var depositPremiumPercent = $("#inputInsuredDepositPremiumPercent").val();
            var insuredLimit = $("#inputInsuredLimit").val();
            var search = true;
            if (depositPremiumPercent > 100 || depositPremiumPercent < 0) {
                search = false;
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateDepositPremiumPercent, 'autoclose': true });
                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
            } else if (insuredLimit <= 0) {
                search = false;
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValidationInsuredLimit, 'autoclose': true });
                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
            }

            if (search) {
                var groupId = glbRisk.CoverageGroupId;
                var productId = glbPolicy.Product.Id;
                insuredObject = $("#formObjectTransport").serializeObject();
                RiskTransportCoverage.GetFormInsuredObject();
                
                RiskTransportCoverageRequest.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(transportDto, insuredObject, groupId, productId,
                    glbPolicy.CurrentFrom, glbPolicy.CurrentTo).done(function (data) {
                        if (data.success) {
                            if (data.result.length == 0) {
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ObjectInsuranceNotAssociatedCoverages, 'autoclose': true });
                            } else {
                                var coverages = $("#listCoveragesEdit").UifListView('getData');
                                if (coverages.length == data.result.length) {
                                    RiskTransportCoverage.LoadCoverages(data.result);
                                    RiskTransportCoverage.SaveInsuredObject();
                                } else {
                                    RiskTransportCoverage.GetDiferences(data.result, coverages)
                                }
                                RiskTransportCoverage.SaveCoverages();
                            }
                            pressBtnAcept = true;
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    })
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields, 'autoclose': true });
        }
    }
    static GetDiferences(defaultCoverages, allCoverages) {
        $.each(allCoverages, function (index, value) {
            value.MaxLiabilityAmount = NotFormatMoney(value.MaxLiabilityAmount).replace(separatorDecimal, '.');
            value.LimitAmount = NotFormatMoney(value.LimitAmount).replace(separatorDecimal, '.');
            value.LimitOccurrenceAmount = NotFormatMoney(value.LimitOccurrenceAmount).replace(separatorDecimal, '.');
            value.LimitClaimantAmount = NotFormatMoney(value.LimitClaimantAmount).replace(separatorDecimal, '.');
            value.DeclaredAmount = NotFormatMoney(value.DeclaredAmount).replace(separatorDecimal, '.');
            value.SubLimitAmount = NotFormatMoney(value.SubLimitAmount).replace(separatorDecimal, '.');
            value.PremiumAmount = NotFormatMoney(value.PremiumAmount, numberDecimal).replace(separatorDecimal, '.');
            if (value.CalculationTypeId != undefined) {
                value.CalculationType = value.CalculationTypeId;
            }
            if (value.RateTypeId != undefined) {
                value.RateType = value.RateTypeId;
            }
        });
        $.each(defaultCoverages, function (index, value) {
            value.CalculationType = value.CalculationTypeId;
            value.RateType = value.RateTypeId;
        });
        RiskTransportCoverageRequest.GetDiferences(glbRisk.Id, defaultCoverages, allCoverages).done(function (data) {
            if (data.success) {
                RiskTransportCoverage.LoadCoverages(data.result);
            }
        });
    }
    static SaveCoverages(event) {
        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
        var premiumValue = NotFormatMoney($('#inputPremium').text());

        if (!policyIsFloating && parseFloat(premiumValue) <= 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValidationValuePremium, 'autoclose': true });
        } else {
            $.each(coveragesValues, function (key, value) {
                this.CurrentFrom = FormatDate(this.CurrentFrom);
                this.CurrentTo = FormatDate(this.CurrentTo);
                this.LimitAmount = NotFormatMoney(this.LimitAmount);
                this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
                this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
                this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
                this.LimitOccurrenceAmount = NotFormatMoney(this.LimitOccurrenceAmount);
                this.LimitClaimantAmount = NotFormatMoney(this.LimitClaimantAmount);
                this.MaxLiabilityAmount = NotFormatMoney(this.MaxLiabilityAmount);
                this.Rate = NotFormatMoney(this.Rate);
                if (this.CalculationType != undefined) {
                    this.CalculationTypeId = this.CalculationType;
                }
                if (this.RateType != undefined) {
                    this.RateTypeId = this.RateType;
                }
            });
            RiskTransportCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, coveragesValues, insuredObjectId).done(function (data) {
                if (data.success) {
                    saveCover = true;
                    if (event != null && event.currentTarget.id === "btnSaveCoverage") {
                        IsEditing = false;
                        saveCover = false;
                        router.run("prtRiskTransport");
                        setTimeout(function () {
                            lockScreen();
                        }, 300);
                    }
                }
                else {
                    saveCover = false;
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
            });
        }
    }

    ChangeFrom(event, date) {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
            $("#inputToCoverage").UifDatepicker('setValue', AddToDate($("#inputFromCoverage").val(), 0, 0, 1));
        }
        else if (CompareDates(FormatDate(glbPolicy.Endorsement.CurrentFrom), $("#inputFromCoverage").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidatePolicyDate, 'autoclose': true });
            $("#inputFromCoverage").UifDatepicker('setValue', GetCurrentFromDate());
        }
        RiskTransportCoverage.CalculateDays();
    }

    ChangeTo(event, date) {
        if (CompareDates($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalDate, 'autoclose': true });
            $("#inputToCoverage").UifDatepicker('setValue', AddToDate($("#inputFromCoverage").val(), 0, 0, 1));
        }
        RiskTransportCoverage.CalculateDays();
    }

    static CalculateDays() {
        var aFecha1 = $("#inputFromCoverage").val().toString().split('/');
        var aFecha2 = $("#inputToCoverage").val().toString().split('/');
        var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
        var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
        var dif = fFecha2 - fFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (isNaN(dias)) {
            $("#inputDaysCoverage").val(0);
        }
        else {
            $("#inputDaysCoverage").val(dias);
        }
    }

    static GetCalculationTypes() {
        RiskTransportCoverageRequest.GetCalculationTypes().done(function (data) {
            if (data.success) {
                $("#selectCalculationType").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }
    static GetRateTypes(callback) {
        RiskTransportCoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);
                $("#selectRateType").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static GetInsuredRateTypes(rateid) {
        RiskTransportCoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (rateid != undefined) {
                    $("#selectInsuredRate").UifSelect({ sourceData: data.result, selectedId: rateid });

                } else {
                    $("#selectInsuredRate").UifSelect({ sourceData: data.result });
                }
                idRate = $('#selectInsuredRate').val();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
            RiskTransportCoverage.DisableControls(glbPolicy.Endorsement.EndorsementType);
        });
    }

    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        if (IsEditing) {
            RiskTransportCoverageRequest.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId, transportDto.InsuredObjects).done(function (data) {
                if (data.success) {
                    $("#selectInsuredObject").UifSelect({ sourceData: data.result, selectedId: glbCoverage.CoverageId });
                    insuredObjectId = glbCoverage.CoverageId;
                    if (data.result.length == 1) {
                        $("#selectInsuredObject").UifSelect("setSelected", data.result[0].Id);
                        insuredObjectId = $('#selectInsuredObject').val();
                    }
                    RiskTransportCoverage.GetFormInsuredObject();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            RiskTransportCoverageRequest.GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(productId, groupCoverageId, prefixId, transportDto.InsuredObjects).done(function (data) {
                if (data.success) {
                    $("#selectInsuredObject").UifSelect({ sourceData: data.result, selectedId: glbCoverage.CoverageId });
                    insuredObjectId = glbCoverage.CoverageId;
                    if (data.result.length == 1) {
                        $("#selectInsuredObject").UifSelect("setSelected", data.result[0].Id);
                        insuredObjectId = $('#selectInsuredObject').val();
                    }
                    RiskTransportCoverage.GetFormInsuredObject();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }

    }

    static GetCoveragesByRiskId(riskId, temporalId) {
        RiskTransportCoverageRequest.GetCoveragesByRiskId(riskId, temporalId).done(function (data) {
            if (data.success) {
                RiskTransportCoverage.LoadCoverages(data.result);
            }
            return data.success;
        });
    }

    static QuotateCoverages(coverages) {
        var quotate = null;
        RiskTransportCoverageRequest.QuotationCoverages(coverages, transportDto, true, false).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    quotate = data.result;
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
        return quotate;
    }
    static GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId) {
        RiskTransportCoverageRequest.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    var coverages = RiskTransportCoverage.formatCoverage(data.result);
                        insuredObject = data.result[0].InsuredObject;
                        RiskTransportCoverage.ValidateInsudedValue();
                        RiskTransportCoverage.LoadCoverages(coverages);
                    
                    $("#inputInsuredDepositPremiumPercent").val(FormatMoney(insuredObject.DepositPremiumPercentage));
                    $("#inputInsuretRate").val(NotFormatMoney(insuredObject.Rate.toString().replace('.', ',')));
                    RiskTransportCoverage.GetInsuredRateTypes(insuredObject.RateTypeId);
                } else {
                    var groupId = glbRisk.CoverageGroupId;
                    var productId = glbPolicy.Product.Id;
                    InsuredObject = $("#formObjectTransport").serializeObject();
                    RiskTransportCoverage.GetFormInsuredObject();
                    RiskTransportCoverageRequest.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(transportDto, InsuredObject, groupId, productId,
                        glbPolicy.CurrentFrom, glbPolicy.CurrentTo,).done(function (data) {
                            if (data.success) {
                                if (data.result != null && data.result.length > 0) {
                                    RiskTransportCoverage.LoadCoverages(data.result);
                                } else {
                                    $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
                                }
                            }
                        });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
            }
        });
    }
    static GetCoveragesByObjectId(selectInsuredObjectId) {
        RiskTransportCoverage.GetTemporalCoveragesByRiskIdInsuredObjectId(glbRisk.Id, parseInt(selectInsuredObjectId), true);
    }
    static GetFormInsuredObject() {

        insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
        insuredObject.Amount = NotFormatMoney($("#inputLimitAmount").val());
        insuredObject.Description = $("#selectInsuredObject").UifSelect("getSelectedText");
        insuredObject.Rate = NotFormatMoney($("#inputInsuretRate").val());
        insuredObject.DepositPremiumPercentage = NotFormatMoney($("#inputInsuredDepositPremiumPercent").val());
        insuredObject.RateTypeId = $("#selectInsuredRate").UifSelect("getSelected");
        insuredObject.InsuredLimitAmount = NotFormatMoney($("#inputInsuredLimit").val());
    }

    GetInsuredObjectId(event, itemSelected) {
        insuredObjectId = itemSelected.Id;
        RiskTransportCoverage.GetFormInsuredObject();
        RiskTransportCoverage.GetCoveragesByObjectId(insuredObjectId);
    }

    GetInsuredRate(event, itemSelected) {
        idRate = itemSelected.Id;
        $('#inputInsuretRate').val(0);
    }

    static ValidateTypeCoverage(model) {

        if (model.IsPrimary) {
            return AppResources.CoveragePrincipal;
        }
        else if (model.AllyCoverageId > 0 && model.AllyCoverageId != null) {
            return AppResources.CoverageAllied;
        }
        else if (model.MainCoverageId >= 0 && model.AllyCoverageId == null) {
            return AppResources.CoverageAdditional;
        }
    }




    static LoadCoverages(coverages) {
        var insuredAmount = 0;
        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
        $.each(coverages, function (index, val) {
            this.CalculationTypeId = this.CalculationTypeId;
            this.RateTypeId = this.RateTypeId;
            this.InsuredObject = this.InsuredObject;
            this.SubLineBusiness = this.SubLineBusiness;
            this.Number = index + 1;
            this.CoverageId = this.Id;
            this.Id = this.Id;
            this.CoverStatusName = this.CoverStatusName;
            this.CoverStatus = this.CoverStatus;
            this.DeclaredAmount = FormatMoney(this.DeclaredAmount);
            this.LimitAmount = FormatMoney(this.LimitAmount);
            this.SubLimitAmount = FormatMoney(this.SubLimitAmount);
            this.LimitOccurrenceAmount = FormatMoney(this.LimitOccurrenceAmount);
            this.LimitClaimantAmount = FormatMoney(this.LimitClaimantAmount);
            this.MaxLiabilityAmount = FormatMoney(this.MaxLiabilityAmount);
            this.PremiumAmount = FormatMoney(this.PremiumAmount, 2);
            this.DisplayRate = FormatMoney(this.Rate, 2);
            this.Rate = FormatMoney(this.Rate);
            this.TypeCoverage = RiskTransportCoverage.ValidateTypeCoverage(this);

            val.allowEdit = RiskTransportCoverage.ValidateAllows(this.AllyCoverageId);
            val.allowDelete = RiskTransportCoverage.ValidateAllows(this.AllyCoverageId);

            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.CurrentFromOriginal = FormatDate(this.CurrentFromOriginal);
            this.CurrentToOriginal = FormatDate(this.CurrentToOriginal);
            if (this.Deductible != null) {
                this.DeductibleDescription = this.Deductible.Description;
                this.DeductibleId = this.Deductible.Id;
            }
            this.SubLineBusiness = this.SubLineBusiness;

            $("#listCoveragesEdit").UifListView("addItem", this);
        });
        RiskTransportCoverage.ClearCoverage();
        RiskTransportCoverage.UpdatePremium();
    }

    static ValidateAllows(AllyCoverage) {
        var result = false;
        if (AllyCoverage != undefined && AllyCoverage != null && AllyCoverage > 0) {
            result = false;
        }
        else {
            result = true
        }
        return result;
    }

    EditCoverage(event, data, index) {
        coverageData = data;
        coverageNumber = coverageData.Number;
        coverageIndex = index;
        coverageText = coverageData.Text;
        EditSubLineBusiness = coverageData.SubLineBusiness;

        coverageClauses = coverageData.Clauses;
        if (coverageData != null) {
            coverageText = coverageData.Text;

            if (coverageData.Clauses != null) {
                coverageClauses = coverageData.Clauses;
            } else {
                coverageClauses = null;
            }
            if (coverageData.LimitAmount == null) {
                coverageData.LimitAmount = 0;
            }
            if (coverageData.PremiumAmount == null) {
                coverageData.PremiumAmount = 0;
            }
            if (coverageData.Rate == null) {
                coverageData.Rate = 0;
            }
            if (index != null) {
                RiskTransportCoverage.SetCoverageEdit(data);
            }
            if (data.CalculationTypeId != undefined) {
                $("#selectCalculationType").val(data.CalculationTypeId);
            }
            if (data.CalculationType != undefined) {
                $("#selectCalculationType").val(data.CalculationType);
            }
            if (data.RateTypeId != undefined) {
                $("#selectRateType").val(data.RateTypeId);
            }
            if (data.RateType != undefined) {
                $("#selectRateType").val(data.RateType);
            }
            $("#inputDeclaredAmount").val(data.DeclaredAmount);
            $("#inputLimitAmount").val(data.LimitAmount);
            $("#inputSubLimitAmount").val(data.SubLimitAmount);
            $("#inputMaxLiabilityAmount").val(data.MaxLiabilityAmount);
            $("#inputLimitOccurrenceAmount").val(data.LimitOccurrenceAmount);
            $("#inputLimitClaimantAmount").val(data.LimitClaimantAmount);
            $("#inputRateAmount").val(data.Rate);
            $("#inputDepositPremiumPercentAmount").val(data.DepositPremiumPercent);
            $('#inputPremiumAmount').val(data.PremiumAmount);
            $("#selectRateType").val(data.RateTypeId);
            $('#hiddenRuleSetId').val(data.RuleSetId);
            $('#hiddenPosRuleSetId').val(data.PosRuleSetId);

            RiskTransportCoverageRequest.GetCoveragesByCoverageId(glbPolicy.Product.Id, glbRisk.CoverageGroupId, glbPolicy.Prefix.Id, coverageData.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $("#selectCoverage").UifSelect({ sourceData: data.result, selectedId: coverageData.Id });
                        $("#selectCoverage").UifSelect("disabled", true);
                    }
                    var deductibleId = 0;
                    if (coverageData.Deductible != null) {
                        deductibleId = coverageData.Deductible.Id;
                        if (($('#selectDeductUnitCd Option').length == 0) && ($('#selectMinDeductUnitCd Option').length == 0) && ($('#selectMaxDeductUnitCd Option').length == 0)) {
                            DeductiblesCoverage.LoadDeductibleUnit();
                        }

                        if (($('#selectDeductSubjectCd Option').length == 0) && ($('#selectMinDeductSubjectCd Option').length == 0) && ($('#selectMaxDeductSubjectCd Option').length == 0)) {
                            DeductiblesCoverage.LoadDeductibleSubject();
                        }
                        coverageDeductibles = coverageData.Deductible;
                        RiskTransportCoverage.GetDeductiblesByCoverageId(coverageData.Id, deductibleId);
                    }
                    else {
                        RiskTransportCoverage.GetDeductiblesByCoverageId(coverageData.Id, null);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
        }
    }
    static SetCoverageEdit(data) {
        var coveragesData = [];
        coveragesData.push(data);

        $("#selectCoverage").UifSelect();
        $("#selectCoverage").UifSelect({ sourceData: coveragesData, id: "Id", name: "Description", selectedId: coverageData.Id });
        $("#selectCoverage").prop('disabled', true);
    }

    CoverageDelete(event, data) {
        RiskTransportCoverage.DeleteCoverage(data);
    }
    static DeleteCoverage(data) {
        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else {
            var coverages = $("#listCoveragesEdit").UifListView('getData');
            $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });

            $.each(coverages, function (index, value) {
                if (this.Id != data.Id) {
                    $("#listCoveragesEdit").UifListView("addItem", this);
                }
            });
            RiskTransportCoverage.UpdatePremium();
            RiskTransportCoverage.ClearCoverage();
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
        }
    }
    static UpdateAmounts() {
        $('#inputLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputSubLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputLimitClaimantAmount').val($('#inputDeclaredAmount').val());
        $('#inputMaxLiabilityAmount').val($('#inputDeclaredAmount').val());
        $('#inputLimitOccurrenceAmount').val($('#inputDeclaredAmount').val());
        $('#inputPremiumCoverage').val(0);
    }
    static DisableControls(typeEndorsement) {
        switch (typeEndorsement) {
            case 2:
                $("#inputInsuredDepositPremiumPercent").prop('disabled', true);
                $("#inputDepositPremiumPercentAmount").prop('disabled', true);
                $("#inputInsuretRate").prop('disabled', false);
                $("#inputRateAmount").prop('disabled', true);
                $("#selectInsuredRate").UifSelect('disabled', false);
                $("#selectRateType").UifSelect('disabled', true);
                break;
        }
    }
    static formatCoverage(coverages) {
        $.each(coverages, function (key, value) {

            if (this.CurrentFrom == undefined) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInvalidCoverageDates + ': ' + value.Description, 'autoclose': true });
                return false;
            }
            else if (value.CurrentFrom.indexOf("Date") > -1) {
                value.CurrentFrom = FormatDate(value.CurrentFrom);
                value.CurrentTo = FormatDate(value.CurrentTo);
            }
            if (value.CurrentFromOriginal.indexOf("Date") > -1) {
                value.CurrentFromOriginal = FormatDate(value.CurrentFromOriginal);
                value.CurrentToOriginal = FormatDate(value.CurrentToOriginal);
            }

        });
        return coverages;
    }
    Deductibles() {
        var deductId = $("#selectDeductible").UifSelect("getSelected");
        if (deductId != null && deductId > -2) {
            RiskTransportCoverage.GetDeductiblesByCoverageId(parseInt($("#selectCoverage").UifSelect("getSelected")), parseInt(deductId));
        }
    }
    selectCoverageItemselected(event, itemSelected) {
        if (itemSelected.Id > 0) {
            var value = NotFormatMoney($.trim($("#inputLimitAmount").val()));
            if (value > 0) {
                $("#inputDeclaredValue").val($("#inputLimitAmount").val());
                $("#inputRateCoverage").val($("#inputInsuretRate").val());
                $("#selectRateType").UifSelect("setSelected", RateType.Percentage);
            }
            RiskTransportCoverage.GetDeductiblesByCoverageId($("#selectCoverage").UifSelect("getSelected"), null);
        }
        else {
            $('#selectDeductible').UifSelect();
        }
    }
    static GetDeductiblesByCoverageId(coverageId, selectedDeductibleId) {
        var coverage = $("#listCoveragesEdit").UifListView("getData").filter(function (item) {
            return item.Id == coverageId;
        });

        //Es cobertura desde el boton nuevo
        if (coverage.length == 0) {
            RiskTransportCoverageRequest.GetCoverageByCoverageId(coverageId).done(function (data) {
                if (data.success) {
                    coverage.push(data.result);
                }
            });
        }
        //Se obtiene la cobertura actual
        RiskTransportCoverageRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedDeductibleId != null) {
                        if (selectedDeductibleId == coverage[0].Deductible.Id) {
                            $.each(data.result, function (key, value) {
                                if (value.Id == coverage[0].Deductible.Id) {
                                    data.result[key] = null;
                                    data.result[key] = coverage[0].Deductible;
                                }
                            });
                            var deducible = data.result.filter(function (item) {
                                return item.Id == selectedDeductibleId;
                            });
                            if (deducible.length == 0) {
                                data.result.push(coverage[0].Deductible);
                            }
                        }
                        $("#selectDeductible").UifSelect({ sourceData: data.result, selectedId: selectedDeductibleId });
                        $("#selectDeductible").UifSelect("setSelected", selectedDeductibleId);
                        var deductible = data.result.filter(function (item) {
                            return item.Id == selectedDeductibleId;
                        });
                        coverageDeductibles = deductible[0];
                    }
                    else {
                        $("#selectDeductible").UifSelect({ sourceData: data.result });
                    }
                }
                else {
                    $("#selectDeductible").UifSelect({ sourceData: data.result });
                }
                if (data.result.length == 0) {
                    coverageDeductibles = null;
                    $("#selectDeductible").UifSelect({ sourceData: null });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingDeductibles, 'autoclose': true });
        });
    }

    static ValidateRateType() {
        var RateCoverage = parseInt($("#selectInsuredRate").UifSelect("getSelected"), 10);
        switch (RateCoverage) {
            case RateType.Percentage:
                $("#inputRateAmount").attr("maxLength", 9);
                break;
            case RateType.Permilage:
                $("#inputRateAmount").attr("maxLength", 9);
                break;
            default:
                //FixedValue
                $("#inputRateAmount").attr("maxLength", 20);
                break;
        }
    }

}
