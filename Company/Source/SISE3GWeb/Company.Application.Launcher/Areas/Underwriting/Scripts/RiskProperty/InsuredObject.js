//VARIABLES
var coverageData = { Id: 0 };
var indexListViewCoverage = null;
var deductibleData = {};
var coverageText = null;
var coverageClauses = null;
var coverageDeductibles = null;
var insuredObject = { Id: 0, Amount: 0, RateType: 0, Rate: 0, PercentDepositPrimium: 0, DeclarationPeriod: 0, BillingPeriodDepositPremium: 0 }
var isNewInsuredObject = false;
var heightCoverage = 420;
var declarationPeriodId = null;
var billingPeriodId = null;
var calculate = false;
var decimalRate = 4;
var lnglimitAmountIO = 0;
class RiskPropertyInsuredObject extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $('#inputFromCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
        $('#inputToCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentTo));
        lockScreen();
        RiskPropertyInsuredObject.initialize();
        RiskPropertyInsuredObject.loadData();

        if (glbRisk != null && glbRisk.Id > 0) {
            RiskPropertyInsuredObject.CalculateDays();
            if (glbRisk.Coverages != null) {
                $.each(glbRisk.Coverages, function (index, value) {
                    if (this.InsuredObject.Id == glbCoverage.insuredObjectId && this.MainCoverageId == 0) {
                        $("#inputLimitAmount").val(this.InsuredObject.Amount);
                    }
                });
            }
            RiskPropertyInsuredObject.GetRiskById(glbRisk.Id);
        }

        if (IsEditing) {
            if (glbCoverage.insuredObjectId != null && glbCoverage.insuredObjectId != "") {
                RiskPropertyInsuredObject.GetTemporalCoveragesByRiskIdInsuredObjectId(glbRisk.Id, glbCoverage.insuredObjectId, false);
            }
        }
    }
    static loadData() {
        RiskPropertyInsuredObject.GetInsuredObjects();
        RiskPropertyInsuredObject.GetRateTypes();
        RiskPropertyInsuredObject.GetCalculeTypes(1);
        RiskPropertyInsuredObject.GetFirstRiskTypes(1);
    }

    bindEvents() {
        $('#selectCoverage').on('itemSelected', this.selectCoverageItemselected);
        $('#selectInsuredObject').on('itemSelected', this.selectInsuredObjectitemselected);
        //guardar cobertura
        $("#btnSaveCoverage").on('click', this.btnSaveCoverageonClick);
        $("#btnCloseObject").on('click', this.btnCloseObjectClick);
        //Crear Cobertura
        $('#listCoveragesEdit').on('rowAdd', this.listCoveragesEditRowAdd);
        $('#listCoveragesEdit').on('rowEdit', this.listCoveragesEditRowEdit);
        $('#listCoveragesEdit').on('rowDelete', this.listCoveragesEditRowDelete);
        $("#btnAcceptObject").on("click", RiskPropertyInsuredObject.btnAcceptObjectOnClick);
        //Agregar cobertura a listview
        $("#btnAcceptCoverage").on("click", this.btnAcceptCoverageonclick);
        $("#btnCancelCoverage").on("click", this.CancelCoverage);
        $(".input-money").on('focusin', this.inputMoneyOnFocusin);
        $(".input-money").on('focusout', this.inputMoneyOnFocusout);
        $('#selectCoverage').on('focusout', this.selectCoverageOnFocusout);
        $('#selectDeductible').on('itemSelected', this.Deductibles);
        $('#selectMeasurementBenefit').on('itemSelected', this.selectMeasurementBenefitOnChange);
        $("#btnCalculateProperty").on('click', this.selectCalculeTypeOnFocusout);

        $('#inputLimitAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            insuredObject.Amount = value;
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));

        });

        $('#inputLimitAmount').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            //Habilita el botón de agregar cuando se define el limite asegurado para el objeto del seguro
            $('#btnAcceptCoverage').attr('disabled', false);
            value = NotFormatMoney(value);
            if (value != insuredObject.Amount) {
                RiskPropertyInsuredObject.btnAcceptObjectOnClick();
            }
        });

        $('#inputLimitAmount').keypress(function (e) {
            var chartlong = $('#inputLimitAmount').val()
            if (chartlong.length >= 16) {
                e.preventDefault();
            }
        });



        $('#inputInsuredDepositPremiumPercent').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            insuredObject.PercentDepositPrimium = value;
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputInsuredDepositPremiumPercent').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            value = NotFormatMoney(value);
            if (value != insuredObject.PercentDepositPrimium) {
                RiskPropertyInsuredObject.btnAcceptObjectOnClick();
            }
        });

        $('#inputInsuretRate').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            insuredObject.Rate = value;
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));

        });

        $('#inputInsuretRate').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));

            var text = $(this).val().split(',');
            if (text.length == 2) {
                if (parseInt(text[0]) > 100 || (parseInt(text[0]) >= 100 && parseInt(text[1]) > 0)) {
                    $(this).val("");
                }
            } else {
                if (parseInt(text[0]) > 100) {
                    $(this).val("");
                }
            }
            value = NotFormatMoney(value);
            if (value != insuredObject.Rate) {
                RiskPropertyInsuredObject.btnAcceptObjectOnClick();
            }
        });

        $('#inputSubLimitAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputSubLimitAmount').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            RiskPropertyInsuredObject.ValidateSubLimit();
        });

        $('#inputLimitOccurrence').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputLimitOccurrence').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });
        $('#inputLimitClaimant').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputLimitClaimant').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#inputDeclaredValue').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputDeclaredValue').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#inputAmountCoverage').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputAmountCoverage').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#LimitOccurrenceAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#LimitOccurrenceAmount').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#LimitClaimantAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#LimitClaimantAmount').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#inputExcessLimit').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(FormatMoney($(this).val()));
        });

        $('#inputExcessLimit').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#inputMaxLiabilitySurety').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputMaxLiabilitySurety').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $('#selectCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                RiskPropertyInsuredObject.GetCoverageByCoverageId(selectedItem.Id);
            }
            else {
                $('#selectDeductible').UifSelect();
            }
        });

        $('#inputRateCoverage').focusout(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            var type = $("#selectRateTypeCoverage").UifSelect("getSelected");
            type = parseInt(type);
            if (type == RateType.FixedValue) {
                value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            }
        });

        $('#selectRateTypeCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                RiskPropertyInsuredObject.ValidateRateType();
                calculate = false;
                $("#inputRateCoverage").val(0);
                $('#inputPremiumCoverage').val(0);
            }
        });
        $('#selectCalculeType').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                calculate = false;
                $("#inputRateCoverage").val(0);
                $('#inputPremiumCoverage').val(0);
            }
        });


    }
    static ValidateCoverageRate() {
        var type = $("#selectRateTypeCoverage").UifSelect("getSelected");
        type = parseInt(type);
        var rate = parseFloat($("#inputRateCoverage").val().replace(',', '.'));

        switch (type) {
            case RateType.FixedValue:
                return true;
            case RateType.Percentage:
            case RateType.Permilage:
                if (rate > 100 || rate < 0) {
                    $("#inputRateCoverage").val(0);
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValueRate, 'autoclose': true });
                    return false
                } else {
                    return true;
                }
        }
        return false;
    }


    static ValidateSubLimit() {
        var declaredValue = $('#inputDeclaredValue').val();
        var subLimit = $('#inputSubLimitAmount').val();
        if (subLimit > declaredValue) {
            $('#inputSubLimitAmount').val(declaredValue);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateSubLimitAmount, 'autoclose': true });
        }
    }
    RunRulesCoverage() {
        coverageData = RiskPropertyInsuredObject.GetFormCoverage();
        if (coverageData.RuleSetId != null) {
            InsuredObjectRequest.RunRulesCoverage(glbRisk.Id, coverageData, coverageData.RuleSetId).done(function (data) {
                if (data.success) {
                    if (data.result) {
                        coverageData = data.result;
                        RiskPropertyInsuredObject.SetFormCoverage(coverageData);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRunningPreRules, 'autoclose': true });
            });
        }
    }

    CancelCoverage() {
        RiskPropertyInsuredObject.ClearCoverage();
        var value = NotFormatMoney($('#inputLimitAmount').val());
        if (value > 0) {
            calculate = false;
            RiskPropertyInsuredObject.GetCalculeTypes(1);
            RiskPropertyInsuredObject.GetFirstRiskTypes(1);
            RiskPropertyInsuredObject.GetCoveragesNameByProductIdGroupCoverageId();
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorLimitAmountIO, 'autoclose': true });
        }
    }

    selectMeasurementBenefitOnChange(event) {
        if ($("#selectMeasurementBenefit").val() != null && $('#selectMeasurementBenefit').val() > 0 && $('#selectMeasurementBenefit').val() == 1) {

            $('#inputAmountCoverage').attr('disabled', true);
            $('#inputSubLimitAmount').attr('disabled', true);

        }
        else if ($("#selectMeasurementBenefit").val() != null && $('#selectMeasurementBenefit').val() > 0 && $('#selectMeasurementBenefit').val() == 2) {
            $('#inputAmountCoverage').attr('disabled', false);
            $('#inputSubLimitAmount').attr('disabled', false);
        }
        else {
            $('#inputAmountCoverage').attr('disabled', false);
            $('#inputSubLimitAmount').attr('disabled', false);
        }
    }

    selectCalculeTypeOnFocusout() {
        $('#inputPremiumCoverage').val(0);
        calculate = false;
        $("#formCoverage").validate();
        var DeclaredValue = parseFloat(NotFormatMoney($("#inputDeclaredValue").val()));
        var inputRateCoverage = $('#inputRateCoverage').val();
        if ($("#formCoverage").valid()) {
            if (DeclaredValue <= 0) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValidationDeclaredAmount, 'autoclose': true });
            }
            else {
                if (RiskPropertyInsuredObject.ValidateCoverageRate()) {
                    RiskPropertyInsuredObject.GetPremium();
                    calculate = true;
                }
            }
        }
    }
    Deductibles() {
        var deductId = $("#selectDeductible").UifSelect("getSelected");
        if (deductId != null && deductId > -2) {
            RiskPropertyInsuredObject.GetDeductiblesByCoverageId(parseInt($("#selectCoverage").UifSelect("getSelected")), parseInt(deductId));
        }
    }
    selectCoverageOnFocusout() {
        var coverageId = $("#selectCoverage").UifSelect("getSelected");
        if (coverageId != null && coverageId > 0) {
            InsuredObjectRequest.selectCoverageFocusout(glbPolicy.Id, glbRisk.Id, $("#selectCoverage").UifSelect("getSelected"), $("#selectInsuredObject").UifSelect("getSelected")).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        RiskPropertyInsuredObject.SetFormCoverage(data.result);
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverage, 'autoclose': true });
            });
        }
    }

    inputMoneyOnFocusout() {
        var value = NotFormatMoney($.trim($(this).val()));
        value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
        if (value == "") {
            value = 0;
        }
        if (event.target.id == "inputDeclaredValue") {
            $("#inputAmountCoverage").val(value);
            $("#inputSubLimitAmount").val(value);
            $("#inputLimitOccurrence").val(value);
            $("#inputLimitClaimant").val(value);
            $("#inputExcessLimit").val(value);
            $("#inputMaxLiabilitySurety").val(value);
        }
    }
    inputMoneyOnFocusin() {
        var value = NotFormatMoney($.trim($(this).val()));
        value == 0 ? $(this).val("") : $(this).val(value);
    }
    btnAcceptCoverageonclick() {
        if (calculate) {
            if ($("#selectCoverage").UifSelect("getSelected") != null && $("#selectCoverage").UifSelect("getSelected") != "") {
                var Amount = parseFloat(NotFormatMoney($("#inputAmountCoverage").val()));
                var DeclaredValue = parseFloat(NotFormatMoney($("#inputDeclaredValue").val()));
                if (Amount > DeclaredValue) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateLimitDeclaredValue, 'autoclose': true });
                    return;
                }
                $("#formCoverage").validate();
                if ($("#formCoverage").valid()) {
                    lockScreen();
                    coverageData = RiskPropertyInsuredObject.GetFormCoverage();
                    RiskPropertyInsuredObject.QuotationCoverage(coverageData, true);
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSelectCoverage, 'autoclose': true });
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageCalculatePremium, 'autoclose': true });
        }

        RiskPropertyInsuredObject.UpdatePremiunAndAmountInsured();
    }

    static btnAcceptObjectOnClick() {
        $("#mainObject").validate();
        if ($("#mainObject").valid()) {
            lockScreen();
            if (RiskPropertyInsuredObject.ValidateInsuredObject()) {
                RiskPropertyInsuredObject.SaveInsuredObject();
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInsuredObject, 'autoclose': true });
            }
        }
    }

    listCoveragesEditRowDelete(event, data) {
        if ($('#listCoveragesEdit').UifListView("getData").length > 1) {
            RiskPropertyInsuredObject.ClearCoverage();
            RiskPropertyInsuredObject.DeleteCoverage(data);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateDeleteCoverage, 'autoclose': true });
        }
    }

    listCoveragesEditRowEdit(event, data, index) {
        indexListViewCoverage = index;
        calculate = false;
        RiskPropertyInsuredObject.SetFormCoverage(data);
    }

    listCoveragesEditRowAdd() {
        if ($("#inputLimitAmount").val() != "" && NotFormatMoney($("#inputLimitAmount").val()) > 0) {
            RiskPropertyInsuredObject.ClearCoverage();
            RiskPropertyInsuredObject.GetCoveragesNameByProductIdGroupCoverageId(0);
            $("#selectDeductible").UifSelect();
            $("#selectCoverage").UifSelect("Disabled", true);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateLimitAmount, 'autoclose': true });
        }
    }

    btnCloseObjectClick() {
        if (!RiskPropertyInsuredObject.ValidatePrimaryCoverages()){
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorPremiunPrimaryCoverage, 'autoclose': true });
        }
        router.run("prtRiskProperty");
    }

    btnSaveCoverageonClick() {
        RiskPropertyInsuredObject.SaveCoverage();
    }

    selectInsuredObjectitemselected(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskPropertyInsuredObject.ClearCoverage();
            RiskPropertyInsuredObject.GetInsuredObjectByTemporalIdRiskIdInsuredObjectId(selectedItem.Id);
            RiskPropertyInsuredObject.GetFormInsuredObject();
            RiskPropertyInsuredObject.GetCoveragesByObjectId(selectedItem.Id);
            $('#selectInsuredObject').UifSelect("setSelected", selectedItem.Id);
            RiskPropertyInsuredObject.IsDeclaration();
        }
        else {
            RiskPropertyInsuredObject.ClearCoverage();
            $("#listCoveragesEdit").UifListView('refresh');
            $("#inputLimitAmount").val("0");
            $("inputInsuretRate").val("0");
        }
    }

    selectCoverageItemselected(event, itemSelected) {
        if (itemSelected.Id > 0) {
            //Habilita el boton de agregar cobertura
            $('#btnAcceptCoverage').attr('disabled', false);
            var intLimitAmountIO = NotFormatMoney($.trim($("#inputLimitAmount").val()));
            var declarative = $("#selectInsuredObject").UifSelect("getSelectedSource").IsDeclarative;

            var dblDepositPremiumPercent = $('#inputInsuredDepositPremiumPercent').val()
            if (intLimitAmountIO > 0) {
                if (declarative && dblDepositPremiumPercent == 0) {
                    $.UifDialog('confirm', { 'message': AppResources.DepositPremiumPercentInCero }, function (result) {
                        if (result) {
                            $('#inputPremiunDepositCoverage').val($('#inputInsuredDepositPremiumPercent').val());
                            $("#inputDeclaredValue").val($("#inputLimitAmount").val());
                            $("#inputRateCoverage").val($("#inputInsuretRate").val());
                            $("#selectRateTypeCoverage").UifSelect("setSelected", RateType.Permilage);
                            $('#inputAmountCoverage').val($("#inputLimitAmount").val());
                            $('#inputSubLimitAmount').val($("#inputLimitAmount").val());
                            $('#inputLimitOccurrence').val($("#inputLimitAmount").val());
                            $('#inputLimitClaimant').val($("#inputLimitAmount").val());
                            $('#inputExcessLimit').val($("#inputLimitAmount").val());
                            $('#MaxLiabilitySurety').val($("#inputLimitAmount").val());
                        }
                    });
                } else {
                    $('#inputPremiunDepositCoverage').val($('#inputInsuredDepositPremiumPercent').val());
                    $("#inputDeclaredValue").val($("#inputLimitAmount").val());
                    $("#inputRateCoverage").val($("#inputInsuretRate").val());
                    $("#selectRateTypeCoverage").UifSelect("setSelected", RateType.Permilage);
                    $('#inputAmountCoverage').val($("#inputLimitAmount").val());
                    $('#inputSubLimitAmount').val($("#inputLimitAmount").val());
                    $('#inputLimitOccurrence').val($("#inputLimitAmount").val());
                    $('#inputLimitClaimant').val($("#inputLimitAmount").val());
                    $('#inputExcessLimit').val($("#inputLimitAmount").val());
                    $('#inputMaxLiabilitySurety').val($("#inputLimitAmount").val());
                }
                RiskPropertyInsuredObject.GetDeductiblesByCoverageId($("#selectCoverage").UifSelect("getSelected"), null);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorLimitAmountIO, 'autoclose': true });
                $('#btnAcceptCoverage').attr('disabled', true);
                $("#selectCoverage").UifSelect("setSelected", "");
            }
        }
        else {
            $('#selectDeductible').UifSelect();
        }
    }

    static QuotationCoverage(coverageModel, save) {
        if (coverageModel != null) {
            InsuredObjectRequest.QuotationCoverage(glbRisk.Id, coverageModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $("#inputDeclaredAmount").val(FormatMoney(data.result.DeclaredAmount));
                        $("#inputAmountCoverage").val(FormatMoney(data.result.LimitAmount));
                        $("#inputSubLimitAmount").val(FormatMoney(data.result.SubLimitAmount));
                        $("#inputRateCoverage").val(FormatMoney(data.result.Rate));
                        $("#inputPremiumCoverage").val(FormatMoney(data.result.PremiumAmount.toFixed(numberDecimal)));
                        $("#selectRateTypeCoverage").UifSelect('setSelected', data.result.RateType);
                        if (save) {
                            RiskPropertyInsuredObject.SetDataCoverage(data.result);
                        }
                    }
                    else {
                        $("#inputPremiumCoverage").val(0);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
            });

        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        }
    }

    static SetDataCoverage(coverageData) {
        if (coverageData != null) {
            coverageData.CurrentFrom = FormatDate(coverageData.CurrentFrom);
            coverageData.CurrentTo = FormatDate(coverageData.CurrentTo);
            coverageData.Text = coverageText;
            coverageData.Clauses = coverageClauses;
            if (indexListViewCoverage == null) {
                //RiskPropertyInsuredObject.GetAllyCoverageByCoverage(coverageData);
                coverageData.SubLimitAmount = FormatMoney(coverageData.SubLimitAmount);
                coverageData.PremiumAmount = FormatMoney(coverageData.PremiumAmount, 2);
                coverageData.DisplayRate = FormatMoney(coverageData.Rate, decimalRate);
                coverageData.Description = $("#selectCoverage").UifSelect('getSelectedText');
                coverageData.Rate = FormatMoney(coverageData.Rate, decimalRate);
                coverageData.FlatRatePorcentage = FormatMoney(coverageData.FlatRatePorcentage);
                coverageData.TypeCoverage = RiskPropertyInsuredObject.ValidateTypeCoverage(coverageData);
                $("#listCoveragesEdit").UifListView("addItem", coverageData);
            } else {
                if (coverageData.IsPrimary) {
                    RiskPropertyInsuredObject.DeleteAllyCoveragebyCoverage(coverageData);
                    RiskPropertyInsuredObject.GetAllyCoverageByCoverage(coverageData);
                }
                coverageData.SubLimitAmount = FormatMoney(coverageData.SubLimitAmount);
                coverageData.LimitAmount = FormatMoney(coverageData.LimitAmount);
                coverageData.PremiumAmount = FormatMoney(coverageData.PremiumAmount, 2);
                coverageData.DisplayRate = FormatMoney(coverageData.Rate, decimalRate);
                coverageData.Description = $("#selectCoverage").UifSelect('getSelectedText');
                coverageData.Rate = FormatMoney(coverageData.Rate, decimalRate);
                coverageData.FlatRatePorcentage = FormatMoney(coverageData.FlatRatePorcentage);
                coverageData.TypeCoverage = RiskPropertyInsuredObject.ValidateTypeCoverage(coverageData);
                $("#listCoveragesEdit").UifListView("editItem", indexListViewCoverage, coverageData);

            }
            RiskPropertyInsuredObject.ClearCoverage();
            RiskPropertyInsuredObject.SaveCoverage();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateLimitAmount, 'autoclose': true });
        }
    }
    static GetAllyCoverageByCoverage(coverageData) {
        var coverages = $("#listCoveragesEdit").UifListView('getData');
        $.each(coverages, function (index, value) {
            if (coverageData.Id == value.MainCoverageId) {
                value.DeclaredAmount = coverageData.LimitAmount;
                value.LimitAmount = 0;
                value.SubLimitAmount = FormatMoney(coverageData.LimitAmount);
                value.MaxLiabilityAmount = coverageData.LimitAmount;
                value.LimitOccurrenceAmount = coverageData.LimitAmount;
                value.LimitClaimantAmount = coverageData.LimitAmount;
                value.Rate = 0;
                value.PremiumAmount = 0;
                value.CurrentFrom = coverageData.CurrentFrom;
                value.CurrentTo = coverageData.CurrentTo;
                $("#listCoveragesEdit").UifListView("editItem", index, value);
            }
        });
    }
    static GetRateTypes(selectedIdRate) {
        InsuredObjectRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#selectRateTypeCoverage').UifSelect({ sourceData: data.result });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQueryRateTypes, 'autoclose': true });
        });
    }
    static GetFirstRiskTypes(selectedIdFirstRisk) {

        InsuredObjectRequest.GetFirstRiskTypes().done(function (data) {
            if (data.success) {
                if (data.result) {
                    if (selectedIdFirstRisk == 0) {
                        $('#selectMeasurementBenefit').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectMeasurementBenefit").UifSelect({ sourceData: data.result, selectedId: selectedIdFirstRisk });
                    }

                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoveredRiskTypes, 'autoclose': true });
        });
    }
    static ReturnRisk() {
        InsuredObjectRequest.ReturnRisk(glbPolicy.Id).done(function (data) {
            if (data.success) {
                var policyModelsView = data.result;
                policyModelsView.riskId = glbRisk.Id;

                //lanza los eventos para la creación del riesgo
                let events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);
                if (events !== TypeAuthorizationPolicies.Restrictive) {
                    $.redirect(rootPath + 'Underwriting/RiskProperty/Property', policyModelsView);
                }
                //fin - lanza los eventos para la creación del riesgo
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemp, 'autoclose': true });
        });
    }
    static CalculateDays() {

        var cFecha1 = $("#inputFromCoverage").val().toString().split('/');
        var cFecha2 = $("#inputToCoverage").val().toString().split('/');
        var fcFecha1 = Date.UTC(cFecha1[2], cFecha1[1] - 1, cFecha1[0]);
        var fcFecha2 = Date.UTC(cFecha2[2], cFecha2[1] - 1, cFecha2[0]);
        var dif = fcFecha2 - fcFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (isNaN(dias)) {
            $("#inputDaysCoverage").val(0);
        }
        else {
            $("#inputDaysCoverage").val(dias);
        }
    }
    static GetCalculeTypes(selectedIdCalcule) {
        InsuredObjectRequest.GetCalculeTypes().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (selectedIdCalcule == 0) {
                        $('#selectCalculeType').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectCalculeType").UifSelect({ sourceData: data.result, selectedId: selectedIdCalcule });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
        });
    }
    static GetInsuredObjects() {
        InsuredObjectRequest.GetInsuredObjects(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (glbCoverage.insuredObjectId != undefined) {
                        $("#selectInsuredObject").UifSelect({ sourceData: data.result, selectedId: glbCoverage.insuredObjectId });
                        if (glbCoverage.insuredObjectId > 0) {
                            RiskPropertyInsuredObject.IsDeclaration();
                        }
                    }
                    else {
                        $("#selectInsuredObject").UifSelect({ sourceData: data.result });
                    }
                }
                RiskPropertyInsuredObject.GetFormInsuredObject();

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsuranceObjects, 'autoclose': true });
        });
    }
    static GetCoveragesByObjectId(selectInsuredObjectId) {
        RiskPropertyInsuredObject.GetTemporalCoveragesByRiskIdInsuredObjectId(glbRisk.Id, parseInt(selectInsuredObjectId), false);
    }

    static LoadDataCoverages(selectInsuredObjectId) {
        InsuredObjectRequest.GetCoveragesByObjectId(parseInt(selectInsuredObjectId), glbRisk.GroupCoverage.Id, glbPolicy.Product.Id).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    $('#selectCoverage').UifSelect({ sourceData: data.result });
                    if (data.result.length < 2) {
                        $('#selectCoverage').UifSelect("Disabled", true);
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsuranceObjects, 'autoclose': true });
        });
    }
    static GetCoverageViewModel(selectInsuredObjectId) {
        var coverageModelsView = {};
        coverageModelsView.TemporalId = glbPolicy.Id;
        coverageModelsView.RiskId = glbRisk.Id;
        coverageModelsView.CoverageGroup = glbRisk.GroupCoverage.Id;
        coverageModelsView.ProductId = glbPolicy.Product.Id;
        coverageModelsView.CoverageId = selectInsuredObjectId;

        return coverageModelsView;
    }

    static GetDeductiblesByCoverageId(coverageId, selectedDeductibleId) {
        var coverage = $("#listCoveragesEdit").UifListView("getData").filter(function (item) {
            return item.Id == coverageId;
        });

        //Es cobertura desde el boton nuevo
        if (coverage.length == 0) {
            InsuredObjectRequest.GetCoverageByCoverageId(coverageId).done(function (data) {
                if (data.success) {
                    coverage.push(data.result);
                }
            });
        }
        //Se obtiene la cobertura actual
        InsuredObjectRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
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
                } else {
                    $("#selectDeductible").UifSelect({ sourceData: data.result });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingDeductibles, 'autoclose': true });
        });
    }
    static GetCoverages() {

        InsuredObjectRequest.GetCoverages(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#selectCoverage").UifSelect({ sourceData: data.result });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverages, 'autoclose': true });
        });


    }
    static GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId, isCoverege) {
        InsuredObjectRequest.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId, false).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    isNewInsuredObject = false;
                    RiskPropertyInsuredObject.LoadCoverages(data.result);
                    var insuredObject = data.result[0].InsuredObject;
                    $("#inputLimitAmount").val(FormatMoney(insuredObject.Amount));
                    $("#inputInsuredDepositPremiumPercent").val(FormatMoney(insuredObject.DepositPremiunPercent));
                    $("#inputInsuretRate").val(FormatMoney((insuredObject.Rate == null ? 0 : insuredObject.Rate), decimalRate));
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverages, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetCoverages, 'autoclose': true });
            }
        });
    }
    static SaveInsuredObject() {
        var depositPremiumPercent = $("#inputInsuredDepositPremiumPercent").val();
        var insuredLimit = $("#inputLimitAmount").val();
        var rate = $("#inputInsuretRate").val();
        var search = true;

        if (depositPremiumPercent > 100 || depositPremiumPercent < 0) {
            search = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateDepositPremiumPercent, 'autoclose': true });
            $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
        }
        if (insuredLimit <= 0) {
            search = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValidationInsuredLimit, 'autoclose': true });
        }

        $.UifProgress('show');

        if (search) {
            var rate = $("#inputInsuretRate").val();
            if (rate == 0) {
                RiskPropertyInsuredObject.ValidateDataInsuredObject();
                rate = $("#inputInsuretRate").val();
            }

            insuredLimit = NotFormatMoney($("#inputLimitAmount").val());
            insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
            insuredObject.Description = $("#selectInsuredObject option:selected").text();
            insuredObject.DeclarationPeriod = $('#selectDeclarationPeriod').UifSelect("getSelected");
            insuredObject.BillingPeriodDepositPremium = $('#selectBillingPeriod').UifSelect("getSelected");
            insuredObject.Amount = insuredLimit;
            lnglimitAmountIO = insuredLimit;
            insuredObject.Rate = rate;

            insuredObject.IsDeclarative = $("#chkIsDeclaration").is(':checked')
            if (depositPremiumPercent == "" || depositPremiumPercent == null) { depositPremiumPercent = 0; };
            insuredObject.DepositPremiunPercent = depositPremiumPercent;
            InsuredObjectRequest.SaveInsuredObject(glbRisk.Id, insuredObject, glbPolicy.Id, glbRisk.GroupCoverage.Id).done(function (data) {
                if (data.success) {
                    InsuredObjectRequest.GetCalculateCoverages(glbRisk.Id, insuredObject,
                        depositPremiumPercent, rate, glbPolicy.CurrentFrom, glbPolicy.CurrentTo, insuredLimit, true, true).done(function (data) {
                            if (data.success) {
                                if (data.result != null && data.result.length > 0) {
                                    isNewInsuredObject = false;
                                    var coverages = $("#listCoveragesEdit").UifListView('getData');
                                    if (coverages.length == data.result.length) {
                                        RiskPropertyInsuredObject.LoadCoverages(data.result);
                                    }
                                    else {
                                        RiskPropertyInsuredObject.GetDiferences(data.result, coverages)
                                    }
                                } else {
                                    InsuredObjectRequest.GetTemporalCoveragesByInsuredObjectIdGroupCoverageId(insuredObject.Id, glbRisk.GroupCoverage.Id, glbPolicy.Product.Id).done(function (data) {
                                        if (data.success) {
                                            if (data.result != null && data.result.length > 0) {
                                                isNewInsuredObject = true;
                                                RiskPropertyInsuredObject.LoadCoverages(data.result);
                                            } else {
                                                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#objectDisplayTemplate", height: heightCoverage });
                                            }
                                        }
                                    });
                                }
                            }
                        });
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedInsuredObjectSuccessfully, 'autoclose': true });
                    ScrollTop();
                    RiskPropertyInsuredObject.SaveCoverage();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverage, 'autoclose': true });
            });
        }
        $.UifProgress('close');
    }

    static GetDiferences(defaultCoverages, allCoverages) {
        $.each(allCoverages, function (index, value) {
            value.SubLimitAmount = NotFormatMoney(value.SubLimitAmount).replace(separatorDecimal, '.');
            value.PremiumAmount = NotFormatMoney(value.PremiumAmount).replace(separatorDecimal, '.');
        });
        InsuredObjectRequest.GetDiferences(glbRisk.Id, defaultCoverages, allCoverages).done(function (data) {
            if (data.success) {
                RiskPropertyInsuredObject.LoadCoverages(data.result);
            }
        });
    }
    static SetFormCoverage(coverageValues) {

        if (coverageValues != null) {
            if (coverageData == null) {
                coverageData = { Id: 0 };
            }
            coverageData = coverageValues;

            $("#hiddenCoverageId").val(coverageValues.Id);
            if (coverageValues.LimitAmount == null) {
                coverageValues.LimitAmount = 0;
            }
            if (coverageValues.PremiumAmount == null) {
                coverageValues.PremiumAmount = 0;
            }
            if (coverageValues.Rate == null) {
                coverageValues.Rate = 0;
            }

            $("#inputAmountCoverage").val(coverageValues.LimitAmount);
            $("#inputAmountCoverage").val(NotFormatMoney($("#inputAmountCoverage").val()));
            $("#inputAmountCoverage").val(FormatMoney($("#inputAmountCoverage").val()));

            $("#inputSubLimitAmount").val(coverageValues.SubLimitAmount);
            $("#inputSubLimitAmount").val(NotFormatMoney($("#inputSubLimitAmount").val()));
            $("#inputSubLimitAmount").val(FormatMoney($("#inputSubLimitAmount").val()));

            $("#inputAmountCoverage").attr("disabled", false);
            $("#inputSubLimitAmount").attr("disabled", false);

            if (coverageValues.DeclaredAmount == null) {
                $("#inputDeclaredValue").val($("#inputLimitAmount").val());
            }
            else {
                $("#inputDeclaredValue").val(coverageValues.DeclaredAmount);
            }
            $("#inputDeclaredValue").val(NotFormatMoney($("#inputDeclaredValue").val()));
            $("#inputDeclaredValue").val(FormatMoney($("#inputDeclaredValue").val()));


            $("#inputLimitOccurrence").val(coverageValues.LimitOccurrenceAmount);
            $("#inputLimitOccurrence").val(NotFormatMoney($("#inputLimitOccurrence").val()));
            $("#inputLimitOccurrence").val(FormatMoney($("#inputLimitOccurrence").val()));

            $("#inputLimitClaimant").val(coverageValues.LimitClaimantAmount);
            $("#inputLimitClaimant").val(NotFormatMoney($("#inputLimitClaimant").val()));
            $("#inputLimitClaimant").val(FormatMoney($("#inputLimitClaimant").val()));

            $("#inputExcessLimit").val(coverageValues.ExcessLimit);
            $("#inputExcessLimit").val(NotFormatMoney($("#inputExcessLimit").val()));
            $("#inputExcessLimit").val(FormatMoney($("#inputExcessLimit").val()));

            $("#inputMaxLiabilitySurety").val(coverageValues.MaxLiabilityAmount);
            $("#inputMaxLiabilitySurety").val(NotFormatMoney($("#inputMaxLiabilitySurety").val()));
            $("#inputMaxLiabilitySurety").val(FormatMoney($("#inputMaxLiabilitySurety").val()));

            $("#inputPremiumCoverage").val(NotFormatMoney(coverageValues.PremiumAmount));
            $("#inputPremiumCoverage").val(FormatMoney($("#inputPremiumCoverage").val()));
            $("#inputRateCoverage").val(FormatMoney(coverageValues.Rate, decimalRate));
            $("#inputPremiunDepositCoverage").val(FormatMoney(coverageValues.DepositPremiunPercent));

            $("#TotalSumInsured").text(NotFormatMoney($("#TotalSumInsured").text()));
            $("#TotalPremium").text(NotFormatMoney($("#TotalPremium").text()));
            $("#TotalPremium").text(FormatMoney($("#TotalPremium").text()));
            $("#TotalSumInsured").text(FormatMoney($("#TotalSumInsured").text()));
            $("#hiddenCoverStatusName").val(coverageValues.CoverStatusName);
            $("#hiddenCoverStatus").val(coverageValues.CoverStatus);
            $("#hiddenIsVisible").val(coverageValues.IsVisible);
            $("#hiddenIsSelected").val(coverageValues.IsSelected);
            $("#hiddenIsMandatory").val(coverageValues.IsMandatory);
            $("#hiddenSubLineBusinessId").val(coverageValues.SubLineBusiness.Id);
            $("#hiddenLineBusinessId").val(coverageValues.SubLineBusiness.LineBusiness.Id);
            $("#selectCalculeType").UifSelect("setSelected", coverageValues.CalculationType);
            $("#selectRateTypeCoverage").UifSelect("setSelected", coverageValues.RateType);


            if (coverageValues.FirstRiskType == 1) {
                $('#inputAmountCoverage').attr('disabled', true);
                $('#inputSubLimitAmount').attr('disabled', true);
                $("#selectMeasurementBenefit").UifSelect("setSelected", coverageValues.FirstRiskType);
            }
            else if (coverageValues.FirstRiskType == 2) {
                $('#inputAmountCoverage').attr('disabled', false);
                $('#inputSubLimitAmount').attr('disabled', false);
                $("#selectMeasurementBenefit").UifSelect("setSelected", coverageValues.FirstRiskType);
            }
            else if (coverageValues.FirstRiskType == 0 || coverageValues.FirstRiskType == null) {
                $('#inputAmountCoverage').attr('disabled', false);
                $('#inputSubLimitAmount').attr('disabled', false);
                $("#selectMeasurementBenefit").UifSelect("setSelected", 1);
            }
            coverageClauses = coverageValues.Clauses;
            if (clausesCoverage != null) {
                clausesCoverage.Clauses = coverageValues.Clauses;
            }
            coverageText = coverageValues.Text;
            glbCoverage.CoverageId = coverageValues.Id;

            InsuredObjectRequest.GetCoveragesByCoverageId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coverageValues.Id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        $("#selectCoverage").UifSelect({ sourceData: data.result, selectedId: coverageValues.Id });
                        $("#selectCoverage").UifSelect("disabled", true);
                    }
                    var deductibleId = 0;
                    if (coverageValues.Deductible != null) {
                        deductibleId = coverageValues.Deductible.Id;
                        if (($('#selectDeductUnitCd Option').length == 0) && ($('#selectMinDeductUnitCd Option').length == 0) && ($('#selectMaxDeductUnitCd Option').length == 0)) {
                            DeductiblesCoverage.LoadDeductibleUnit();
                        }

                        if (($('#selectDeductSubjectCd Option').length == 0) && ($('#selectMinDeductSubjectCd Option').length == 0) && ($('#selectMaxDeductSubjectCd Option').length == 0)) {
                            DeductiblesCoverage.LoadDeductibleSubject();
                        }
                        coverageDeductibles = coverageValues.Deductible;
                        RiskPropertyInsuredObject.GetDeductiblesByCoverageId(coverageValues.Id, deductibleId);
                    }
                    else {
                        RiskPropertyInsuredObject.GetDeductiblesByCoverageId(coverageValues.Id, null);
                    }
                    RiskPropertyInsuredObject.LoadSubTitles(0);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
            });
        }
    }
    static GetFormCoverage() {
        var coverage = coverageData;

        coverage.Description = $("#selectCoverage").UifSelect("getSelectedText");
        coverage.CurrentFrom = $("#inputFromCoverage").val();
        $("#inputFromCoverage").data("dateFromCoverage", $("#inputFromCoverage").val());
        coverage.CurrentTo = $("#inputToCoverage").val();
        $("#inputToCoverage").data("dateToCoverage", $("#inputToCoverage").val());

        if ($.trim($("#inputAmountCoverage").val()) != "") {
            coverage.LimitAmount = NotFormatMoney($("#inputAmountCoverage").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.LimitAmount = 0;
        }
        if ($.trim($("#inputAmountCoverage").val()) != "") {
            coverage.Amount = NotFormatMoney($("#inputAmountCoverage").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.Amount = 0;
        }
        if ($.trim($("#inputSubLimitAmount").val()) != "") {
            coverage.SubLimitAmount = NotFormatMoney($("#inputSubLimitAmount").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.SubLimitAmount = 0;
        }
        if ($.trim($("#inputPremiumCoverage").val()) != "") {
            coverage.PremiumAmount = NotFormatMoney($("#inputPremiumCoverage").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.PremiumAmount = 0;
        }
        if ($.trim($("#inputExcessLimit").val()) != "") {
            coverage.ExcessLimit = NotFormatMoney($("#inputExcessLimit").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.ExcessLimit = 0;
        }
        if ($.trim($("#inputLimitClaimant").val()) != "") {
            coverage.LimitClaimantAmount = NotFormatMoney($("#inputLimitClaimant").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.LimitClaimantAmount = 0;
        }
        if ($.trim($("#inputLimitOccurrence").val()) != "") {
            coverage.LimitOccurrenceAmount = NotFormatMoney($("#inputLimitOccurrence").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.LimitOccurrenceAmount = 0;
        }

        if ($.trim($("#inputRateCoverage").val()) != "") {
            coverage.Rate = NotFormatMoney($("#inputRateCoverage").val(), decimalRate);
        }
        else {
            coverage.Rate = 0;
        }
        if ($.trim($("#inputDeclaredValue").val()) != "") {
            coverage.DeclaredAmount = NotFormatMoney($("#inputDeclaredValue").val());
        }
        else {
            coverage.DeclaredAmount = 0;
        }
        if ($.trim($("#inputMaxLiabilitySurety").val()) != "") {
            coverage.MaxLiabilityAmount = NotFormatMoney($("#inputMaxLiabilitySurety").val()).replace(separatorDecimal, '.');
        } else {
            coverage.MaxLiabilityAmount = 0;
        }

        if ($.trim($("#inputPremiunDepositCoverage").val()) != "") {
            coverage.DepositPremiumPercent = NotFormatMoney($("#inputPremiunDepositCoverage").val()).replace(separatorDecimal, '.');
        } else {
            coverage.DepositPremiumPercent = 0;
        }



        coverage.FirstRiskType = $("#selectMeasurementBenefit").UifSelect("getSelected");
        coverage.CalculationType = $("#selectCalculeType").UifSelect("getSelected");
        coverage.RateType = $("#selectRateTypeCoverage").UifSelect("getSelected");
        coverage.InsuredObject = insuredObject;
        coverage.Text = coverageText;
        coverage.Clauses = coverageClauses;
        coverage.declarationPeriodId = $('#selectDeclarationPeriod').UifSelect("getSelected");
        coverage.billingPeriodId = $('#selectBillingPeriod').UifSelect("getSelected");
        if (coverageDeductibles == null) {
            if ($("#selectDeductible").UifSelect("getSelected") > -1 && $("#selectDeductible").UifSelect("getSelected") != null && $("#selectDeductible").UifSelect("getSelected") != "") {
                coverage.Deductible = {
                    Id: $("#selectDeductible").UifSelect("getSelected"),
                    Description: $("#selectDeductible").UifSelect('getSelectedText')
                };
                coverageDeductibles = coverage.Deductible;
            } else {
                coverage.Deductible = null;
            }
        } else {
            if ($("#selectDeductible").UifSelect("getSelected") == null || $("#selectDeductible").UifSelect("getSelected") == "") {
                coverage.Deductible = null;
            } else {
                coverage.Deductible = coverageDeductibles;
            }
        }

        coverage.TemporalId = glbPolicy.Id;
        coverage.RiskId = glbRisk.Id;
        return coverage;
    }
    static ClearCoverage() {
        $("#selectCalculeType").UifSelect("setSelected", null);
        $("#selectCoverage").UifSelect("setSelected", null);
        $("#selectCoverage").UifSelect("disabled", true);
        $('#selectRateTypeCoverage').UifSelect("setSelected", null);
        $('#selectDeductible').UifSelect("setSelected", null);
        $('#selectDeductible').UifSelect();
        $('#selectMeasurementBenefit').UifSelect("setSelected", null);
        $('#inputAmountCoverage').val('0');
        $('#inputAmountCoverage').attr("disabled", false);
        $('#inputSubLimitAmount').val('0');
        $('#inputSubLimitAmount').attr("disabled", false);
        $('#inputRateCoverage').val('0');
        $('#inputPremiumCoverage').val('0');
        $('#inputPremiunDepositCoverage').val('0');
        $('#inputDeclaredValue').val('0');
        $('#inputLimitOccurrence').val('0');
        $('#inputMaxLiabilitySurety').val('0');
        $('#inputLimitClaimant').val('0');
        $('#inputExcessLimit').val('0');
        $('#selectedTexts').text("");
        $('#selectedClauses').text("");
        $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
        $("#inputToCoverage").UifDatepicker('setValue', FormatDate(glbPolicy.CurrentTo));

        coverageText = null;
        coverageClauses = null;
        coverageDeductibles = null;
        indexListViewCoverage = null;
        deductibleData = {};
        coverageData = { Id: 0 };
    }
    static DeleteCoverage(data) {
        if (stringToBoolean(data.IsMandatory) == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.MainCoverageId != 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteAllyCoverages, 'autoclose': true });
        }
        else {
            $.UifProgress('close');
            $.UifDialog('confirm', { 'message': AppResources.DeleteCoverage }, function (result) {
                if (result) {

                    var coverages = $("#listCoveragesEdit").UifListView('getData');
                    $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#objectDisplayTemplate", height: heightCoverage });

                    $.each(coverages, function (index, value) {
                        if (this.Id != data.Id && this.IsMandatory == true) {
                            value.TypeCoverage = RiskPropertyInsuredObject.ValidateTypeCoverage(value);
                            $("#listCoveragesEdit").UifListView("addItem", value);
                        }
                        else {
                            if (data.EndorsementType == EndorsementType.Modification && data.RiskCoverageId > 0) {
                                var coverage = RiskPropertyInsuredObject.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                                if (coverage != null) {
                                    coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                                    coverage.DisplayRate = FormatMoney(coverage.Rate);
                                    $('#listCoveragesEdit').UifListView('addItem', coverage);
                                }
                            }
                        }
                    });
                    RiskPropertyInsuredObject.UpdatePremiunAndAmountInsured();
                }
            });
        }
    }
    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        InsuredObjectRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
            if (data.success) {
                coverage = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorExcludeCoverage, 'autoclose': true });
        });

        return coverage;
    }
    static DeleteAllyCoveragebyCoverage(coverage) {
        var coverages = $("#listCoveragesEdit").UifListView('getData');
        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#objectDisplayTemplate", height: heightCoverage });

        $.each(coverages, function (index, value) {
            if (this.MainCoverageId.Id != coverage.Id) {
                $("#listCoveragesEdit").UifListView("addItem", value);
            }
        });
    }
    static initialize() {
        $("#inputLimitAmount").val($("#inputLimitAmount").val());
        $("#inputLimitAmount").val(FormatMoney($("#inputLimitAmount").val()));
        $("#inputLimitAmount").ValidatorKey(1, 0, 0);
        $("#inputFromCoverage").ValidatorKey(ValidatorType.Dates, 1, 1);
        $("#inputToCoverage").ValidatorKey(ValidatorType.Dates, 1, 1);
        $("#inputAmountCoverage").ValidatorKey(1, 0, 0);
        $("#inputSubLimitAmount").ValidatorKey(1, 0, 0);
        $("#inputPremiumCoverage").OnlyDecimals(UnderwritingDecimal);
        $('#inputInsuredDepositPremiumPercent').OnlyDecimals(UnderwritingDecimal);
        $("#inputRateCoverage").OnlyDecimals(3);
        $("#inputPercentageContract").OnlyDecimals(UnderwritingDecimal);
        $('#inputDays').attr('disabled', 'disabled');
        $('#selectCoverage').prop('disabled', true);
        $('#inputFromCoverage').UifDatepicker('disabled', true);
        $('#inputToCoverage').UifDatepicker('disabled', true);
        $("#inputLimitOccurrence").ValidatorKey(1, 0, 0);
        $("#inputLimitClaimant").ValidatorKey(1, 0, 0);
        $("#inputExcessLimit").ValidatorKey(1, 0, 0);
        $("#inputMaxLiabilitySurety").ValidatorKey(1, 0, 0);
        $("#inputDeclaredValue").ValidatorKey(1, 0, 0);
        $("#inputRateObject").OnlyDecimals(UnderwritingDecimal);
        $('#inputInsuretRate').OnlyDecimals(UnderwritingDecimal);
        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#objectDisplayTemplate", height: heightCoverage });
        $("#chkIsDeclaration").prop('disabled', true);

        $("#inputLimitAmount").val("0");
        $("#inputInsuredDepositPremiumPercent").val("0");
        $("#inputInsuretRate").val("0");
        $("#inputDeclaredValue").val("0");
        $("#inputAmountCoverage").val("0");
        $("#inputSubLimitAmount").val("0");
        $("#inputLimitOccurrence").val("0");
        $("#inputLimitClaimant").val("0");
        $("#inputExcessLimit").val("0");
        $("#inputMaxLiabilitySurety").val("0");
        $("#inputRateCoverage").val("0");


    }
    static GetFormInsuredObject() {
        insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
        insuredObject.LimitAmount = NotFormatMoney($("#inputLimitAmount").val());
        insuredObject.Description = $("#selectInsuredObject").UifSelect("getSelectedText");
    }
    static GetPremium() {

        if (RiskPropertyInsuredObject.ValidCoverageValue()) {
            coverageData = RiskPropertyInsuredObject.GetFormCoverage();
            if (coverageData != null) {
                RiskPropertyInsuredObject.QuotationCoverage(coverageData, false);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
            }
        }
    }
    static GetCoveragesNameByProductIdGroupCoverageId() {
        $("#selectCoverage").UifSelect();
        var coveragesAdd = '';
        $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
            if (coveragesAdd.length > 0) {
                coveragesAdd += ',';
            }
            coveragesAdd += this.Id;
        });

        InsuredObjectRequest.GetCoveragesNameByProductIdGroupCoverageId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd, $("#selectInsuredObject").UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $("#selectCoverage").UifSelect({ sourceData: data.result });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
        });
    }

    static ValidateTypeCoverage(model) {

        if (model.IsPrimary) {
            return AppResources.CoveragePrincipal;
        }
        else if (model.MainCoverageId > 0 && model.AllyCoverageId == null) {
            return AppResources.CoverageAdditional;
        }
        else if (model.AllyCoverageId > 0 && model.AllyCoverageId != null) {
            return AppResources.CoverageAllied;
        }
        else {
            return AppResources.CoverageAdditional;
        }
    }
    static LoadCoverages(coverages) {
        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: (!isNewInsuredObject), customEdit: true, edit: (!isNewInsuredObject), delete: (!isNewInsuredObject), displayTemplate: "#objectDisplayTemplate", height: heightCoverage });
        var totalSumInsured = 0;
        var totalPremium = 0;

        var cover;
        if (coverages != null) {
            $.each(coverages, function (index, val) {
                totalPremium = totalPremium + coverages[index].PremiumAmount;
                totalSumInsured = totalSumInsured + coverages[index].LimitAmount;
                coverages[index].DepositPremiunPercent = coverages[index].InsuredObject.DepositPremiunPercent;
                coverages[index].LimitAmount = FormatMoney(coverages[index].LimitAmount);
                coverages[index].SubLimitAmount = FormatMoney(coverages[index].SubLimitAmount);
                coverages[index].PremiumAmount = FormatMoney(coverages[index].PremiumAmount);
                coverages[index].DisplayRate = FormatMoney(coverages[index].Rate, decimalRate);
                coverages[index].LimitOccurrenceAmount = FormatMoney(coverages[index].LimitOccurrenceAmount);
                coverages[index].LimitClaimantAmount = FormatMoney(coverages[index].LimitClaimantAmount);
                coverages[index].ExcessLimit = FormatMoney(coverages[index].ExcessLimit);
                coverages[index].Rate = FormatMoney(coverages[index].Rate, decimalRate);
                val.TypeCoverage = RiskPropertyInsuredObject.ValidateTypeCoverage(val);
                val.allowEdit = RiskPropertyInsuredObject.ValidateAllows(val.AllyCoverageId);
                val.allowDelete = RiskPropertyInsuredObject.ValidateAllows(val.AllyCoverageId);
                cover = coverages[index];
                $("#listCoveragesEdit").UifListView('addItem', cover);
            });
        }
        $("#inputPremium").text(FormatMoney(totalPremium, 2));
        $("#inputAmountInsured").text(FormatMoney(totalSumInsured));
    }

    static ValidatePrimaryCoverages() {
        var result = true;
        var coverages = $("#listCoveragesEdit").UifListView('getData');
        if (coverages.length > 0) {
            $.each(coverages, function (key, value) {
                if (Number.parseInt(NotFormatMoney(this.PremiumAmount)) == 0 && this.IsPrimary) {
                    result = false; 
                }
            });
        } else {
            result = false;
        }

        return result;
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

    static ValidateDataInsuredObject() {
        var coverages = $("#listCoveragesEdit").UifListView('getData');

        if (coverages.length > 0) {
            $.each(coverages, function (key, value) {
                var RateCoverage = this.Rate;
                if (RateCoverage != null && RateCoverage != "0" && (this.IsPrimary || (this.MainCoverageId > 0))) {
                    
                    $('#inputInsuretRate').val(this.Rate);
                    var insuredLimit = NotFormatMoney($("#inputLimitAmount").val());
                    insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
                    insuredObject.Description = $("#selectInsuredObject option:selected").text();
                    insuredObject.DeclarationPeriod = $('#selectDeclarationPeriod').UifSelect("getSelected");
                    insuredObject.BillingPeriodDepositPremium = $('#selectBillingPeriod').UifSelect("getSelected");
                    insuredObject.Amount = insuredLimit;
                    insuredObject.Rate = $('#inputInsuretRate').val();
                    return false;
                }
            });
        }
    }

    static SaveCoverage() {
        if ($("#selectInsuredObject").UifSelect("getSelected") != '') {
            /*Valida las propiedades del objeto del seguro*/
            RiskPropertyInsuredObject.ValidateDataInsuredObject();

            InsuredObjectRequest.SaveInsuredObject(glbRisk.Id, insuredObject, glbPolicy.Id, glbRisk.GroupCoverage.Id).done(function (data) {
                if (data.success) {
                    var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
                    if (coveragesValues.length > 0) {
                        lockScreen();

                        $.each(coveragesValues, function (key, value) {
                            if (this.CurrentFrom.indexOf("Date") > -1) {
                                this.CurrentFrom = FormatDate(this.CurrentFrom);
                                this.CurrentTo = FormatDate(this.CurrentTo);
                            }
                            this.LimitAmount = NotFormatMoney(this.LimitAmount.toString());
                            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount.toString());
                            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount.toString());
                            this.PremiumAmount = NotFormatMoney(this.PremiumAmount.toString());
                            if (this.Rate != null) {
                                this.Rate = FormatMoney(this.Rate, decimalRate);
                            }
                        });

                        if ($('#selectDeclarationPeriod').UifSelect("getSelected") != "") {
                            declarationPeriodId = $('#selectDeclarationPeriod').UifSelect("getSelected");
                        } else {
                            declarationPeriodId = null;
                        }
                        var insuredObjectDesc = $('#selectInsuredObject').UifSelect("getSelectedText")
                        if ($('#selectBillingPeriod').UifSelect("getSelected") != "") {
                            billingPeriodId = $('#selectBillingPeriod').UifSelect("getSelected");
                        } else {
                            billingPeriodId = null;
                        }

                        InsuredObjectRequest.SaveCoverage(glbPolicy.Id, glbRisk.Id, $("#listCoveragesEdit").UifListView('getData'), $("#selectInsuredObject").UifSelect("getSelected"), insuredObjectDesc, declarationPeriodId, billingPeriodId).done(function (data) {
                            if (data.success) {
                                RiskPropertyInsuredObject.UpdatePremiunAndAmountInsured();
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageUpdateCoverage, 'autoclose': true });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        }).fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverage, 'autoclose': true });
                        });
                    }
                    else { 
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {   
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorInsuredObject, 'autoclose': true });
            });
            
            //lockScreen();
            //unlockScreen();
        }
    }
        
    static ValidateInsuredObject() {
        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
        var premium = 0;
        var countCoverage = 0;

        var blndeclarative = $("#selectInsuredObject").UifSelect("getSelectedSource").IsDeclarative;
        var dblRateIO = $('#inputInsuretRate').val();
        //*Aplica validacnones para el objeto declarativo*//
        if (blndeclarative && (dblRateIO == 0)) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidationValueRate, 'autoclose': true });
            return;
        } else if (!blndeclarative && (dblRateIO == 0)) {
            RiskPropertyInsuredObject.ValidateDataInsuredObject();
        }
                
        $.each(coveragesValues, function (index, value) {
            if (value.CoverStatus == CoverageStatus.Excluded) {
                countCoverage = countCoverage + 1;
            }
        });
        if (coveragesValues.length == countCoverage) {
            return false;
        }
        return true;
    }

    static PremiumTotal() {
        premiumTotal = 0;
        var premiumBase = 0;
        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            premiumBase = NotFormatMoney(value.PremiumAmount);
            premiumTotal = premiumTotal + parseFloat(premiumBase.replace(',', '.'));
        });
        $("#inputPremium").text(FormatMoney(premiumTotal));
        return premiumTotal;
    }
    static ShowPanelsCoverage(Menu) {
        switch (Menu) {
            case MenuType.Texts:
                $("#modalTexts").UifModal('showLocal', AppResources.LabelTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                break;
            default:
                break;
        }
    }
    static HidePanelsInsuredObject(Menu) {

        switch (Menu) {
            case MenuType.Texts:
                $("#modalTexts").UifModal('hide');
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('hide');
                break;
            default:
                break;
        }
    }
    static GetCoverageByCoverageId() {

        InsuredObjectRequest.GetCoverageByCoverageId($("#selectCoverage").UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $("#hiddenSubLineBusinessId").val(data.result.SubLineBusiness.Id);
                    $("#hiddenLineBusinessId").val(data.result.SubLineBusiness.LineBusiness.Id);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchCoverage, 'autoclose': true });
                    $("#hiddenSubLineBusinessId").val(0);
                    $("#hiddenLineBusinessId").val(0);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                $("#hiddenSubLineBusinessId").val(0);
                $("#hiddenLineBusinessId").val(0);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverage, 'autoclose': true });
            $("#hiddenSubLineBusinessId").val(0);
            $("#hiddenLineBusinessId").val(0);
        });
    }
    static GetInsuredObjectByTemporalIdRiskIdInsuredObjectId(selectInsuredObjectId) {
        InsuredObjectRequest.GetInsuredObjectByTemporalIdRiskIdInsuredObjectId(glbPolicy.Id, glbRisk.Id, selectInsuredObjectId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $("#inputLimitAmount").val(FormatMoney(data.result.LimitAmount));
                }
                else {
                    $("#inputLimitAmount").val(0);
                }
            }
            else {
                $("#inputLimitAmount").val(0);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchInsuredObject, 'autoclose': true });
            $("#inputLimitAmount").val(0);
        });
    }
    static LoadTitle() {
        var title = AppResources.LabelTitleCoverage + " - " + AppResources.LabeltitleRisk + ": " + glbRisk.FullAddress;
        $.uif2.helpers.setGlobalTitle(title);
    }
    static GetRiskById(id) {

        if (id != "") {
            InsuredObjectRequest.GetRiskById(glbPolicy.Id, id).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        RiskPropertyInsuredObject.LoadTitle(data.result);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
            });
        }
    }
    static ValidCoverageValue() {
        if ($("#selectCoverage").UifSelect("getSelected") == null || $("#selectCoverage").UifSelect("getSelected") == 0) {
            return false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
        }
        else {
            return true;
        }
    }

    static UpdatePremiunAndAmountInsured() {
        var coverages = $("#listCoveragesEdit").UifListView('getData');
        var totalSumInsured = 0;
        var totalPremium = 0;
        $.each(coverages, function (index, value) {
            totalPremium += parseFloat(NotFormatMoney(coverages[index].PremiumAmount).replace(separatorDecimal, separatorThousands));
            totalSumInsured += parseFloat(NotFormatMoney(coverages[index].LimitAmount).replace(separatorDecimal, separatorThousands));
        });
        $("#inputAmountInsured").text(FormatMoney(totalSumInsured.toFixed(2)));
        $("#inputPremium").text(FormatMoney(totalPremium.toFixed(2)));
    }
    static LoadSubTitles(subTitle) {
        if (subTitle == 0 || subTitle == 1) {
            if (coverageText != null) {
                if (coverageText.TextBody == null) {
                    coverageText.TextBody = "";
                }

                if (coverageText.TextBody.length > 0) {
                    $('#selectedTexts').text("(" + AppResources.LabelWithText + ")");
                }
                else {
                    $('#selectedTexts').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedTexts').text("(" + AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 2) {
            if (coverageClauses != null) {
                if (coverageClauses.length > 0) {
                    $('#selectedClauses').text("(" + coverageClauses.length + ")");
                }
                else {
                    $('#selectedClauses').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedClauses').text("(" + AppResources.LabelWithoutData + ")");
            }
        }
    }
    static GetCoverageByCoverageId(coverageId) {
        InsuredObjectRequest.GetCoverageByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                coverageData = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverage, 'autoclose': true });
        });

    }

    static SetCoverageEdit() {
        $('#selectCoverage').UifSelect();

        $('#selectCoverage').append($('<option>', {
            value: coverageData.Id,
            text: coverageData.Description
        }));

        $('#selectCoverage').UifSelect('setSelected', coverageData.Id);
    }

    static GetDeclarationPeriods(callback) {
        InsuredObjectRequest.GetDeclarationPeriods().done(function (data) {
            if (data.success) {
                return callback(data.result)
                //$('#selectDeclarationPeriod').UifSelect({ sourceData: data.result, filter: true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetBillingPeriods(callback) {
        InsuredObjectRequest.GetBillingPeriods().done(function (data) {
            if (data.success) {
                return callback(data.result)
                //$('#selectBillingPeriod').UifSelect({ sourceData: data.result, filter: true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static IsDeclaration() {
        var declarative = $("#selectInsuredObject").UifSelect("getSelectedSource").IsDeclarative;
        if (declarative) {
            $("#chkIsDeclaration").prop('checked', true);
            $('#inputInsuredDepositPremiumPercent').attr("disabled", false);
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                if (glbRisk.DeclarationPeriodCode != null && glbRisk.DeclarationPeriodCode > 0) {
                    RiskPropertyInsuredObject.GetDeclarationPeriods(function (declarationPeriod) {
                        $('#selectDeclarationPeriod').UifSelect({ sourceData: declarationPeriod, filter: true, selectedId: glbRisk.DeclarationPeriodCode });
                        $('#selectDeclarationPeriod').UifSelect("disabled", true);
                    });
                } else {
                    RiskPropertyInsuredObject.GetDeclarationPeriods(function (declarationPeriod) {
                        $('#selectDeclarationPeriod').UifSelect({ sourceData: declarationPeriod, filter: true, selectedId: declarationPeriod[0].Id });
                        $('#selectDeclarationPeriod').UifSelect("disabled", true);
                    });
                }
                if (glbRisk.BillingPeriodDepositPremium != null && glbRisk.BillingPeriodDepositPremium > 0) {
                    RiskPropertyInsuredObject.GetBillingPeriods(function (billingPeriod) {
                        $('#selectBillingPeriod').UifSelect({ sourceData: billingPeriod, filter: true, selectedId: glbRisk.BillingPeriodDepositPremium });
                        $('#selectBillingPeriod').UifSelect("disabled", true);
                    });
                } else {
                    RiskPropertyInsuredObject.GetBillingPeriods(function (billingPeriod) {
                        $('#selectBillingPeriod').UifSelect({ sourceData: billingPeriod, filter: true, selectedId: billingPeriod[0].Id });
                        $('#selectBillingPeriod').UifSelect("disabled", true);
                    });
                }
            } else {
                if (glbRisk.DeclarationPeriodCode != null && glbRisk.DeclarationPeriodCode > 0) {
                    RiskPropertyInsuredObject.GetDeclarationPeriods(function (declarationPeriod) {
                        $('#selectDeclarationPeriod').UifSelect({ sourceData: declarationPeriod, filter: true, selectedId: glbRisk.DeclarationPeriodCode });
                        $('#selectDeclarationPeriod').UifSelect("disabled", false);
                    });
                } else {
                    RiskPropertyInsuredObject.GetDeclarationPeriods(function (declarationPeriod) {
                        $('#selectDeclarationPeriod').UifSelect({ sourceData: declarationPeriod, filter: true, selectedId: declarationPeriod[0].Id });
                        $('#selectDeclarationPeriod').UifSelect("disabled", false);
                    });
                }
                if (glbRisk.BillingPeriodDepositPremium != null && glbRisk.BillingPeriodDepositPremium > 0) {
                    RiskPropertyInsuredObject.GetBillingPeriods(function (billingPeriod) {
                        $('#selectBillingPeriod').UifSelect({ sourceData: billingPeriod, filter: true, selectedId: glbRisk.BillingPeriodDepositPremium });
                        $('#selectBillingPeriod').UifSelect("disabled", false);
                    });
                } else {
                    RiskPropertyInsuredObject.GetBillingPeriods(function (billingPeriod) {
                        $('#selectBillingPeriod').UifSelect({ sourceData: billingPeriod, filter: true, selectedId: billingPeriod[0].Id });
                        $('#selectBillingPeriod').UifSelect("disabled", false);
                    });
                }
            }
        }
        else {
            $("#chkIsDeclaration").prop('checked', false);
            $('#inputInsuredDepositPremiumPercent').attr("disabled", true);
            RiskPropertyInsuredObject.GetDeclarationPeriods(function (declarationPeriod) {
                $('#selectDeclarationPeriod').UifSelect({ sourceData: declarationPeriod, filter: true });
                $('#selectDeclarationPeriod').UifSelect("disabled", true);
            });
            RiskPropertyInsuredObject.GetBillingPeriods(function (billingPeriod) {
                $('#selectBillingPeriod').UifSelect({ sourceData: billingPeriod, filter: true });
                $('#selectBillingPeriod').UifSelect("disabled", true);
            });
        }
    }
    static ValidateRateType() {
        var RateCoverage = parseInt($("#selectRateType").UifSelect("getSelected"), 10);
        switch (RateCoverage) {
            case RateType.Percentage:
                $("#inputRateCoverage").attr("maxLength", 9)
                break;
            case RateType.Permilage:
                $("#inputRateCoverage").attr("maxLength", 9)
                break;
            default:
                //FixedValue
                $("#inputRateCoverage").attr("maxLength", 20)
                break;
        }
    }

}
