var individualSearchType = 1;
var IndividualType;
var riskControllerCoverage;
var dynamicProperties = null;
var insuredObjectId = null;
var idRate = null;
var pressBtnAcept = false;
var coverageData = { Id: 0 };
var coverageIndex = null;
var textCoverage = null;
var clausesCoverage = null;
var EditSubLineBusiness = null;
var currentOriginalLimitAmount = 0;
var currentOriginalSubLimitAmount = 0;
let CoveragesListMarine = [];
var isNewInsuredObject = true;

class RiskMarineCoverage extends Uif2.Page {

    getInitialState() {
        riskControllerCoverage = 'RiskMarineCoverage';
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskMarineCoverage", formMarineCoverage: "#formMarineCoverage", RecordScript: false, Class: RiskMarineCoverage };
        }
        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputFromCoverage").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#inputToCoverage").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $('#inputDeclaredAmount').ValidatorKey(1, 1, 1);
        $('#inputLimitAmount').ValidatorKey(1, 1, 1);
        $('#inputSubLimitAmount').ValidatorKey(1, 1, 1);
        $('#inputMaxLiabilityAmount').ValidatorKey(1, 1, 1);
        $('#inputLimitOccurrenceAmount').ValidatorKey(1, 1, 1);
        $('#inputLimitClaimantAmount').ValidatorKey(1, 1, 1);
        $('#inputRateAmount').ValidatorKey(1, 1, 1);
        //$('#inputDepositPremiumPercentAmount').ValidatorKey(1, 1, 1);
        $('#inputPremiumAmount').ValidatorKey(1, 1, 1);
        $('#inputInsuredLimit').ValidatorKey(1, 1, 1);
        $('#inputInsuredDepositPremiumPercent').ValidatorKey(1, 1, 1);
        $('#inputInsuretRate').ValidatorKey(1, 1, 1);
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
        //$("#inputDepositPremiumPercentAmount").val(0);
        $("#inputPremiumAmount").val(0);
        RiskMarineCoverage.GetInsuredRateTypes();
        RiskMarineCoverage.GetCalculationTypes();
        RiskMarineCoverage.GetRateTypes();
        RiskMarineCoverage.LoadTemporal(glbPolicy);

        if (IsEditing) {

            if (!RiskMarineCoverage.GetTemporalCoveragesByRiskIdInsuredObjectId(glbRisk.Id, glbCoverage.CoverageId)) {
                RiskMarineCoverage.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, glbRisk.CoverageGroupId, glbPolicy.Prefix.Id);
                pressBtnAcept = true;
            }
        } else {
            RiskMarineCoverage.GetInsuredObjectsByProductIdGroupCoverageId(glbPolicy.Product.Id, glbRisk.CoverageGroupId, glbPolicy.Prefix.Id);
        }
    }

    static LoadTemporal(policyData) {
        $("#inputFromCoverage").UifDatepicker('setValue', FormatDate(policyData.CurrentFrom));
        $("#inputToCoverage").UifDatepicker('setValue', FormatDate(policyData.CurrentTo));
        RiskMarineCoverage.CalculateDays();
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
        $('#selectInsuredObject').on('itemSelected', this.GetInsuredObjectId);
        $('#selectInsuredRate').on('itemSelected', this.GetInsuredRate);
        $("#inputFromCoverage").on('datepicker.change', this.ChangeFrom);
        $("#inputToCoverage").on('datepicker.change', this.ChangeTo);
        $('#btnAcceptObject').click(this.ShowDetail);
        $('#btnNewCoverage').click(this.newCoverage);
        $('#btnAcceptCoverage').on('click', RiskMarineCoverage.AddCoverage);
        $('#btnSaveCoverage').on('click', RiskMarineCoverage.SaveCoverages);
        $('#listCoveragesEdit').on('rowEdit', this.EditCoverage);
        $('#listCoveragesEdit').on('rowDelete', this.CoverageDelete);
        $("#btnCloseCoverage").on("click", this.CloseCoverage);
        $("#btnTextCoverage").on('click', TextsCoverage.TextCoverage);
        $("#btnClausesCoverage").on('click', ClausesCoverage.LoadPartialClauses);

        $('#inputInsuredLimit').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputInsuredLimit').focusout(function (event) {
            event.stopPropagation();
            $(this).val(FormatMoney($(this).val()));
        });

        $('#inputDeclaredAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputDeclaredAmount').focusout(function (event) {
            event.stopPropagation();
            $(this).val(FormatMoney($(this).val()));
            RiskMarineCoverage.UpdateAmounts();
        });
        $('#inputLimitAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            $(this).val(FormatMoney($(this).val()));
        });
        $('#inputSubLimitAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            $(this).val(FormatMoney($(this).val()));
        });
        $('#inputLimitClaimantAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitClaimantAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            $(this).val(FormatMoney($(this).val()));
        });
        $('#inputMaxLiabilityAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputMaxLiabilityAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            $(this).val(FormatMoney($(this).val()));
        });
        $('#inputLimitOccurrenceAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitOccurrenceAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            $(this).val(FormatMoney($(this).val()));
        });
        //$('#inputDepositPremiumPercentAmount').focusout(function (event) {
        //    event.stopPropagation();
        //    RiskMarineCoverage.QuotationCoverage();
        //});
    }
    static GetCoverageModel() {
        var coverageModel = $("#formMarineCoverage").serializeObject();
        coverageModel.Id = $('#hiddenCoverageId').val();
        coverageModel.Number = $('#hiddenNumber').val();
        coverageModel.CoverageId = $("#selectCoverage").UifSelect("getSelected");
        coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.DeclaredAmount = $('#inputDeclaredAmount').val();
        coverageModel.LimitAmount = NotFormatMoney($('#inputLimitAmount').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.MaxLiabilityAmount = NotFormatMoney($('#inputMaxLiabilityAmount').val());
        coverageModel.LimitOccurrenceAmount = NotFormatMoney($('#inputLimitOccurrenceAmount').val());
        coverageModel.LimitClaimantAmount = NotFormatMoney($('#inputLimitClaimantAmount').val());
        coverageModel.CalculationTypeId = $('#selectCalculationType').val();
        coverageModel.Rate = NotFormatMoney($('#inputRateAmount').val());
        coverageModel.RateTypeId = $('#selectRateType').val();
        //coverageModel.DepositPremiumPercent = NotFormatMoney($('#inputDepositPremiumPercentAmount').val());
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumAmount').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;
        coverageModel.OriginalLimitAmount = currentOriginalLimitAmount;
        coverageModel.OriginalSubLimitAmount = currentOriginalSubLimitAmount;
        var InsuredObject = $("#formObjectMarine").serializeObject();
        coverageModel.InsuredObject = InsuredObject;
        if (coverageIndex == null) {
            coverageModel.CoverStatus = 2;
            coverageModel.IsMandatory = false;
        }
        else {
            coverageModel.CoverStatus = 4;
            coverageModel.IsMandatory = coverageData.IsMandatory;
        }
        if (EditSubLineBusiness != null && EditSubLineBusiness != undefined) {
            coverageModel.SubLineBusiness = EditSubLineBusiness;
        } else {
            if (CoveragesListMarine != null && CoveragesListMarine.length > 0) {
                for (var i = 0; i < CoveragesListMarine.length; i++) {
                    if (CoveragesListMarine[i].Id == coverageModel.CoverageId) {
                        coverageModel.SubLineBusiness = CoveragesListMarine[i].SubLineBusiness;
                        break;
                    }
                }
            }
        }

        if (policyIsFloating) {
            $("#chkIsDeclarative").parent().show();//Muestra el control
            $("#chkIsDeclarative").UifSelect('disabled', true);//Des habilita el control
            $("#chkIsDeclarative").prop('checked', true);//Deja el control en true
        }
        else {
            $("#chkIsDeclarative").parent().show();//Muestra el control
            //$("#chkIsDeclarative").UifSelect('disabled', true);//Des habilita el control
            //$("#chkIsDeclarative").prop('checked', false);//Deja el control en true
            //$("#chkIsDeclarative").parent().hide();//Oculta el control
        }
        coverageModel.IsDeclarative = $("#chkIsDeclarative").val();

        if ($("#selectDeductible").UifSelect("getSelected") > 0) {
            coverageModel.DeductibleId = $("#selectDeductible").UifSelect("getSelected");
            coverageModel.DeductibleDescription = $("#selectDeductible").UifSelect('getSelectedText');
        }
        return coverageModel;
    }
    static UpdatePremium() {
        var insuredAmount = 0;
        var premium = 0;

        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
            insuredAmount += parseFloat(NotFormatMoney(this.DeclaredAmount).replace(separatorDecimal, separatorThousands));
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
        //$('#inputDepositPremiumPercentAmount').val(0);
        $('#selectDeductible').UifSelect();
        $('#hiddenNumber').val('');
        currentOriginalLimitAmount = 0;
        currentOriginalSubLimitAmount = 0;

        coverageIndex = null;
        textCoverage = null;
        clausesCoverage = null;
        coverageData = null;
    }
    CloseCoverage() {
        RiskMarineCoverage.ClearCoverage();
        IsEditing = false;
        router.run("prtRiskMarine");
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
                new TextsCoverage();
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                new ClausesCoverage();
                break;
            case MenuType.Script:
                RiskMarineCoverage.LoadScript();
                break;
            default:
                break;
        }
    }
    static QuotationCoverage() {
        var Coverage = RiskMarineCoverage.GetCoverageModel();
        var selectedCoverage = $("#selectCoverage").UifSelect('getSelected');
        if (selectedCoverage != "" && selectedCoverage != null) {
            RiskMarineCoverageRequest.QuotationCoverage(Coverage, marineDto, false, true).done(function (data) {
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
                        Coverage.Text = textCoverage;
                        Coverage.Clauses = clausesCoverage;
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
        var coverage = RiskMarineCoverage.GetCoverageModel();
        var selectedCoverage = $("#selectCoverage").UifSelect('getSelected');
        if (selectedCoverage != "" && selectedCoverage != null) {
            RiskMarineCoverageRequest.QuotationCoverage(coverage, marineDto, false, true).done(function (data) {
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
                        coverage.Text = textCoverage;
                        coverage.Clauses = clausesCoverage;
                        coverage.Number = data.result.Number;
                        coverage.SubLineBusiness = data.result.SubLineBusiness;
                        if (coverageIndex == null) {
                            $("#listCoveragesEdit").UifListView("addItem", coverage);
                        }
                        else {
                            $("#listCoveragesEdit").UifListView("editItem", coverageIndex, coverage);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
                    }
                    RiskMarineCoverage.UpdatePremium();
                    RiskMarineCoverage.ClearCoverage();
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

    newCoverage() {
        if (pressBtnAcept) {
            var coverages = $("#listCoveragesEdit").UifListView('getData');
            RiskMarineCoverageRequest.GetCoveragesByInsuredObjectId(insuredObjectId, coverages).done(function (data) {
                if (data.success) {
                    CoveragesListMarine = data.result;
                    RiskMarineCoverage.ClearCoverage();
                    $("#selectCalculationType").UifSelect("setSelected", CalculationType.Direct);
                    $("#selectRateType").UifSelect("setSelected", RateType.Percentage);
                    $("#selectCoverage").UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            })
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPressAccept, 'autoclose': true });
        }
    }
    static SaveInsuredObject() {

        insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
        insuredObject.Description = $("#selectInsuredObject option:selected").text();
        RiskMarineCoverageRequest.SaveInsuredObject(glbRisk.Id, insuredObject, glbPolicy.Id, glbRisk.CoverageGroupId).done(function (data) {
            if (data.success) {
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverage, 'autoclose': true });
        });
    }
    ShowDetail() {
        $("#formObjectMarine").validate();
        if ($("#formObjectMarine").valid()) {
            var depositPremiumPercent = $("#inputInsuredDepositPremiumPercent").val();
            if (depositPremiumPercent > 100 || depositPremiumPercent < 0) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateDepositPremiumPercent, 'autoclose': true });
                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
            } else {
                var groupId = glbRisk.CoverageGroupId;
                var productId = glbPolicy.Product.Id;
                var rate = $("#inputInsuretRate").val();
                var insuredLimit = NotFormatMoney($("#inputInsuredLimit").val());
                RiskMarineCoverageRequest.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(marineDto, insuredObject, groupId, productId,
                    depositPremiumPercent, idRate, rate, glbPolicy.CurrentFrom, glbPolicy.CurrentTo, insuredLimit, true, false).done(function (data) {
                        if (data.success) {
                            RiskMarineCoverage.SaveInsuredObject();
                            if (data.result.length == 0) {
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ObjectInsuranceNotAssociatedCoverages, 'autoclose': true });
                            } else {
                                RiskMarineCoverage.LoadCoverages(data.result);
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
    static SaveCoverages(event) {

        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');

        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
        });
        RiskMarineCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, $("#listCoveragesEdit").UifListView('getData'), insuredObjectId).done(function (data) {
            if (data.success) {
                if (event.currentTarget.id === "btnSaveCoverage") {
                    IsEditing = false;
                    router.run("prtRiskMarine");
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
        });
    }

    ChangeFrom(event, date) {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
            $("#inputToCoverage").UifDatepicker('setValue', AddToDate($("#inputFromCoverage").val(), 0, 0, 1));
        }
        else if (CompareDates(FormatDate(glbPolicy.Endorsement.CurrentFrom), $("#inputFromCoverage").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidatePolicyDate, 'autoclose': true });
            $("#inputFromCoverage").UifDatepicker('setValue', GetCurrentFromDate());
        }
        RiskMarineCoverage.CalculateDays();
    }

    ChangeTo(event, date) {
        if (CompareDates($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalDate, 'autoclose': true });
            $("#inputToCoverage").UifDatepicker('setValue', AddToDate($("#inputFromCoverage").val(), 0, 0, 1));
        }
        RiskMarineCoverage.CalculateDays();
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
        RiskMarineCoverageRequest.GetCalculationTypes().done(function (data) {
            if (data.success) {
                $("#selectCalculationType").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }
    static GetRateTypes() {
        RiskMarineCoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                $("#selectRateType").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }
    static GetInsuredRateTypes() {
        RiskMarineCoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                $("#selectInsuredRate").UifSelect({ sourceData: data.result });
                $("#selectInsuredRate").UifSelect("setSelected", RateType.Percentage);
                idRate = $('#selectInsuredRate').val();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

    static GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId) {
        if (IsEditing) {
            RiskMarineCoverageRequest.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId, marineDto.InsuredObjects).done(function (data) {
                if (data.success) {
                    $("#selectInsuredObject").UifSelect({ sourceData: data.result, selectedId: glbCoverage.CoverageId });
                    insuredObjectId = glbCoverage.CoverageId;
                    if (data.result.length == 1) {
                        $("#selectInsuredObject").UifSelect("setSelected", data.result[0].Id);
                        insuredObjectId = $('#selectInsuredObject').val();
                    }
                    RiskMarineCoverage.GetFormInsuredObject();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            })
        } else {
            RiskMarineCoverageRequest.GetInsuredObjectsByProductIdGroupCoverageId_inCoverages(productId, groupCoverageId, prefixId, marineDto.InsuredObjects).done(function (data) {
                if (data.success) {
                    $("#selectInsuredObject").UifSelect({ sourceData: data.result, selectedId: glbCoverage.CoverageId });
                    insuredObjectId = glbCoverage.CoverageId;
                    if (data.result.length == 1) {
                        $("#selectInsuredObject").UifSelect("setSelected", data.result[0].Id);
                        insuredObjectId = $('#selectInsuredObject').val();
                    }
                    RiskMarineCoverage.GetFormInsuredObject();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            })
        }

    }

    static GetCoveragesByRiskId(riskId, temporalId) {
        RiskMarineCoverageRequest.GetCoveragesByRiskId(riskId, temporalId).done(function (data) {
            if (data.success) {
                RiskMarineCoverage.LoadCoverages(data.result);
            }
            return data.success;
        });
    }
    static GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId) {
        RiskMarineCoverageRequest.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    isNewInsuredObject = false;
                    RiskMarineCoverage.LoadCoverages(data.result);
                } else {
                    var depositPremiumPercent = $("#inputInsuredDepositPremiumPercent").val();
                    var groupId = glbRisk.CoverageGroupId;
                    var productId = glbPolicy.Product.Id;
                    var rate = 0;
                    var insuredLimit = NotFormatMoney($("#inputInsuredLimit").val());
                    var InsuredObject = $("#formObjectMarine").serializeObject();
                    RiskMarineCoverageRequest.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(marineDto, InsuredObject, groupId, productId,
                        depositPremiumPercent, idRate, rate, glbPolicy.CurrentFrom, glbPolicy.CurrentTo, insuredLimit, true, false).done(function (data) {
                            if (data.success) {
                                if (data.result != null && data.result.length > 0) {
                                    isNewInsuredObject = true;
                                    RiskMarineCoverage.LoadCoverages(data.result);
                                } else {
                                    $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
                                }
                            }
                        });
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ObjectInsuranceNotAssociatedCoverages, 'autoclose': true });
                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
            }


        });
    }
    static GetCoveragesByObjectId(selectInsuredObjectId) {
        RiskMarineCoverage.GetTemporalCoveragesByRiskIdInsuredObjectId(glbRisk.Id, parseInt(selectInsuredObjectId), true);
    }
    static GetFormInsuredObject() {
        insuredObject.Id = $("#selectInsuredObject").UifSelect("getSelected");
        insuredObject.Amount = NotFormatMoney($("#inputLimitAmount").val());
        insuredObject.Description = $("#selectInsuredObject").UifSelect("getSelectedText");
    }
    GetInsuredObjectId(event, itemSelected) {
        insuredObjectId = itemSelected.Id;
        RiskMarineCoverage.GetFormInsuredObject();
        RiskMarineCoverage.GetCoveragesByObjectId(insuredObjectId);

    }
    GetInsuredRate(event, itemSelected) {
        idRate = itemSelected.Id;
    }
    static LoadCoverages(coverages) {
        var insuredAmount = 0;
        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 400 });
        $.each(coverages, function (index, val) {
            this.InsuredObject = this.InsuredObject;
            this.SubLineBusiness = this.SubLineBusiness;
            this.Number = index + 1;
            this.CoverageId = this.Id;
            this.Id = this.Id;
            this.CoverStatusName = this.CoverStatusName;
            this.DeclaredAmount = FormatMoney(this.DeclaredAmount);
            this.LimitAmount = FormatMoney(this.LimitAmount);
            this.SubLimitAmount = FormatMoney(this.SubLimitAmount);
            this.PremiumAmount = FormatMoney(this.PremiumAmount);
            this.DisplayRate = FormatMoney(this.Rate, 2);
            this.Rate = FormatMoney(this.Rate);
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
        RiskMarineCoverage.ClearCoverage();
        RiskMarineCoverage.UpdatePremium();
    }
    EditCoverage(event, data, index) {
        coverageData = data;
        coverageIndex = index;
        textCoverage = coverageData.Text;
        EditSubLineBusiness = coverageData.SubLineBusiness;
        clausesCoverage = coverageData.Clauses;
        if (coverageData != null) {
            coverageText = coverageData.Text;
            coverageClauses = coverageData.Clauses;
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
                RiskMarineCoverage.SetCoverageEdit(data);
            }
            $("#selectCalculationType").val(data.CalculationTypeId);
            $("#hiddenNumber").val(data.Number);
            $("#inputDeclaredAmount").val(data.DeclaredAmount);
            $("#inputLimitAmount").val(data.LimitAmount);
            $("#inputSubLimitAmount").val(data.SubLimitAmount);
            $("#inputMaxLiabilityAmount").val(FormatMoney(data.MaxLiabilityAmount));
            $("#inputLimitOccurrenceAmount").val(FormatMoney(data.LimitOccurrenceAmount));
            $("#inputLimitClaimantAmount").val(FormatMoney(data.LimitClaimantAmount));
            $("#inputRateAmount").val(data.Rate);
            //$("#inputDepositPremiumPercentAmount").val(data.DepositPremiumPercent);
            $('#inputPremiumAmount').val(data.PremiumAmount);
            $("#selectRateType").val(data.RateTypeId);
            currentOriginalLimitAmount = data.OriginalLimitAmount;
            currentOriginalSubLimitAmount = data.OriginalSubLimitAmount;
            if (coverageData.DeductibleId != null) {
                RiskMarineCoverage.GetDeductiblesByCoverageId(coverageData.Id, coverageData.DeductibleId);
            }
            else {
                RiskMarineCoverage.GetDeductiblesByCoverageId(coverageData.Id, 0);
            }
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
        RiskMarineCoverage.DeleteCoverage(data);
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
            RiskMarineCoverage.UpdatePremium();
            RiskMarineCoverage.ClearCoverage();
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.CoverageEliminated, 'autoclose': true });
        }
    }
    static GetDeductiblesByCoverageId(coverageId, selectedId) {
        RiskMarineCoverageRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectDeductibleType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectDeductibleType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static UpdateAmounts() {
        $('#inputLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputSubLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputLimitClaimantAmount').val($('#inputDeclaredAmount').val());
        $('#inputMaxLiabilityAmount').val($('#inputDeclaredAmount').val());
        $('#inputPremiumCoverage').val(0);
    }
}
