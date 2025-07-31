var coverageDeductibles = null;
class RiskThirdPartyLiabilityCoverage extends Uif2.Page {
    getInitialState() {
        coverageData = { Id: 0 };
        coverageIndex = null;
        coverageText = null;
        coverageClauses = null;
        validCoverage = true;

        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $('#inputFromCoverage').UifDatepicker('disabled', true);
        $('#inputToCoverage').UifDatepicker('disabled', true);
        $('#inputFromCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
        $('#inputToCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentTo));
        $('#inputDaysCoverage').val(CalculateDays($('#inputFromCoverage').val(), $('#inputToCoverage').val()));
        $('#hiddenCoverageId').val(glbCoverage.CoverageId);
        RiskThirdPartyLiabilityCoverage.ShowPanelsCoverage(MenuType.Coverage);
        RiskThirdPartyLiabilityCoverage.GetCoveragesByRiskId(glbRisk.Id);
        RiskThirdPartyLiabilityCoverage.GetDeductiblesByCoverageId($('#hiddenCoverageId').val(), glbRisk.Deductible.Id);
        RiskThirdPartyLiabilityCoverage.GetRateTypes()
        RiskThirdPartyLiabilityCoverage.LoadTitle();
    }
    bindEvents() {
        $('#inputDeclaredAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputLimitAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputLimitOccurrence').OnlyDecimals(UnderwritingDecimal);
        $('#inputSubLimitAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputLimitClaimantAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputMaxLiabilityAmount').OnlyDecimals(UnderwritingDecimal);
        $('#inputRateCoverage').OnlyDecimals(3);
        $('#inputPremiumCoverage').OnlyDecimals(UnderwritingDecimal);

        $('#selectCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                RiskThirdPartyLiabilityCoverage.GetCoverageByCoverageId(selectedItem.Id);
            }
            else {
                $('#selectDeductible').UifSelect();
            }
        });
        $('#inputDeclaredAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputDeclaredAmount').focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
            RiskThirdPartyLiabilityCoverage.UpdateAmounts();
            event.stopPropagation();
        });
        $('#inputLimitAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitAmount').focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
            $('#inputPremiumCoverage').val(0);
            event.stopPropagation();
        });
        $('#inputLimitOccurrence').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitOccurrence').focusout(function (event) {
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
            event.stopPropagation();
        });
        $('#inputSubLimitAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputSubLimitAmount').focusout(function (event) {
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
            event.stopPropagation();
        });
        $('#inputLimitClaimantAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputLimitClaimantAmount').focusout(function (event) {
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
            event.stopPropagation();
        });
        $('#inputMaxLiabilityAmount').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });

        $('#inputMaxLiabilityAmount').focusout(function (event) {
            $('#inputPremiumCoverage').val(0);
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(NotFormatMoney($(this).val()));
            event.stopPropagation();
        });
        $('#selectCalculeType').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                $("#inputRateCoverage").val(0);
                $('#inputPremiumCoverage').val(0);
                RiskThirdPartyLiabilityCoverage.GetPremium();
            }
        });
        $('#selectRateTypeCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                $("#inputRateCoverage").val(0);
                $('#inputPremiumCoverage').val(0);
                switch (selectedItem.Id) {
                    case RateType.Percentage:
                        $('#inputRateCoverage').attr('maxLength', 8);
                        break;
                    case RateType.Permilage:
                        $('#inputRateCoverage').attr('maxLength', 9);
                        break;
                    case RateType.FixedValue:
                        $('#inputRateCoverage').attr('maxLength', 20);
                        break;
                    default:

                }
                RiskThirdPartyLiabilityCoverage.GetPremium();
            }
            else {
                $('#inputPremiumCoverage').val(0);
            }
        });
        $('#inputRateCoverage').focusin(function (event) {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputRateCoverage').focusout(function (event) {
            event.stopPropagation();
            var value = NotFormatMoney($.trim($(this).val()));
            RiskThirdPartyLiabilityCoverage.GetPremium();
        });
        $('#btnCancelCoverage').on('click', function () {
            RiskThirdPartyLiabilityCoverage.ClearCoverage();
        });
        $('#btnAcceptCoverage').on('click', function () {
            RiskThirdPartyLiabilityCoverage.AddCoverage();
        });
        $('#listCoveragesEdit').on('rowAdd', function (event) {
            RiskThirdPartyLiabilityCoverage.ClearCoverage();
            RiskThirdPartyLiabilityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        });
        $('#listCoveragesEdit').on('rowEdit', function (event, data, index) {
            coverageData = data;
            coverageIndex = index;
            RiskThirdPartyLiabilityCoverage.EditCoverage(index);
        });
        $('#listCoveragesEdit').on('rowDelete', function (event, data) {
            RiskThirdPartyLiabilityCoverage.DeleteCoverage(data);
        });
        $('#btnCloseCoverage').click(function () {
            router.run("prtRiskThirdPartyLiability");
        });
        $('#btnSaveCoverage').click(function () {
            RiskThirdPartyLiabilityCoverage.SaveCoverages();
        });
        $('#btnNewCoverage').click(function () {
            RiskThirdPartyLiabilityCoverage.ClearCoverage();
            RiskThirdPartyLiabilityCoverage.newCoverage();
        });


    }


    static ShowPanelsCoverage(Menu) {
        switch (Menu) {
            case MenuType.Coverage:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('showLocal', AppResources.LabelTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Deductibles:
                $("#modalDeductibles").UifModal('showLocal', AppResources.LabelDeductibles);
                break;
        }
    }

    static LoadCoverage() {
        $.each($('#listCoveragesEdit').UifListView('getData'), function (key, value) {
            if ($('#hiddenCoverageId').val() == this.Id) {
                coverageIndex = key;
                coverageData = this;
                coverageData.DeclaredAmount = NotFormatMoney(coverageData.DeclaredAmount);
                coverageData.LimitAmount = NotFormatMoney(coverageData.LimitAmount);
                coverageData.SubLimitAmount = NotFormatMoney(coverageData.SubLimitAmount);
                coverageData.LimitClaimantAmount = NotFormatMoney(coverageData.LimitClaimantAmount);
                coverageData.MaxLiabilityAmount = NotFormatMoney(coverageData.MaxLiabilityAmount);
                coverageData.PremiumAmount = NotFormatMoney(coverageData.PremiumAmount);
                coverageData.FlatRatePorcentage = NotFormatMoney(coverageData.FlatRatePorcentage);
                coverageData.RateType = NotFormatMoney(coverageData.RateType);
                coverageData.Rate = coverageData.Rate;
                coverageData.TypeCoverage = coverageData.TypeCoverage;
            }
        });
        RiskThirdPartyLiabilityCoverage.EditCoverage(coverageIndex);
    }

    static SetCoverageEdit() {
        $('#selectCoverage').UifSelect();

        $('#selectCoverage').append($('<option>', {
            value: coverageData.Id,
            text: coverageData.Description
        }));

        $('#selectCoverage').UifSelect('setSelected', coverageData.Id);
    }

    static GetCoveragesByRiskId(riskId) {
        var coverages = null;
        RiskThirdPartyLiabilityCoverageRequest.GetCoveragesByRiskId(riskId).done(function (data) {
            if (data.success) {
                coverages = data.result;
                if (coverages != null) {
                    $('#listCoveragesEdit').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', height: 470 });

                    $.each(coverages, function (key, value) {
                        this.DeclaredAmount = FormatMoney(this.DeclaredAmount);
                        this.SubLimitAmount = FormatMoney(this.SubLimitAmount);
                        this.DisplayRate = FormatMoney(this.Rate);
                        this.PremiumAmount = FormatMoney(this.PremiumAmount);
                        if (this.AllyCoverageId != null) {
                            this.allowEdit = false;
                            this.allowDelete = false;
                        } else {
                            this.allowEdit = true;
                            this.allowDelete = true;
                        }
                        this.TypeCoverage = RiskThirdPartyLiability.ValidateTypeCoverage(this);
                        $('#listCoveragesEdit').UifListView('addItem', this);
                    });
                    RiskThirdPartyLiabilityCoverage.UpdatePremium();
                }
                if ($('#hiddenCoverageId').val() > 0) {
                    RiskThirdPartyLiabilityCoverage.LoadCoverage();
                }
                else {
                    RiskThirdPartyLiabilityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
        });
    }

    static GetCoveragesByProductIdGroupCoverageIdPrefixId() {
        var coveragesAdd = '';
        $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
            if (coveragesAdd.length > 0) {
                coveragesAdd += ',';
            }
            coveragesAdd += this.Id;
        })
        RiskThirdPartyLiabilityCoverageRequest.GetCoveragesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd).done(function (data) {
            if (data.success) {
                $('#selectCoverage').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
        });

    }

    static GetCoverageByCoverageId(coverageId) {
        RiskThirdPartyLiabilityCoverageRequest.GetCoverageByCoverageId(coverageId, glbRisk.Id, glbPolicy.Id, glbRisk.GroupCoverage.Id).done(function (data) {
            if (data.success) {
                coverageData = data.result;
                RiskThirdPartyLiabilityCoverage.EditCoverage(null);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverage, 'autoclose': true });
        });

    }

    static GetDeductiblesByCoverageId(coverageId, selectedDeductibleId) {
        var coverage = $("#listCoveragesEdit").UifListView("getData").filter(function (item) {
            return item.Id == coverageId;
        });

        //Es cobertura desde el boton nuevo
        if (coverage.length == 0) {
            RiskThirdPartyLiabilityCoverageRequest.GetCoverageByCoverageId(coverageId, glbRisk.Id, glbPolicy.Id, glbRisk.GroupCoverage.Id).done(function (data) {
                if (data.success) {
                    coverage.push(data.result);
                }
            });
        }

        RiskThirdPartyLiabilityCoverageRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                if (selectedDeductibleId != null && coverage[0].Deductible != null) {
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
                    $("#selectDeductible").UifSelect('setSelected', selectedDeductibleId);
                }
                else {
                    $("#selectDeductible").UifSelect({ sourceData: data.result });
                }
                if ((selectedDeductibleId == '0') && (coverageDeductibles != null)) {
                    $('#selectDeductible').empty();
                    $('#selectDeductible').append($('<option value=' + "''" + '>- Seleccione un item -</option>'));
                    $('#selectDeductible').append($('<option value=' + "'0'" + '>' + coverageDeductibles.Description + '</option > '));
                    $('#selectDeductible').UifSelect('setSelected', '0');
                }
                //if (selectedId == 0) {
                //    $('#selectDeductible').UifSelect({ sourceData: data.result });
                //}
                //else {
                //    $('#selectDeductible').UifSelect({ sourceData: data.result, selectedId: selectedId });
                //}
                //if (DATA.result != null) {
                //    coverageDeductibles = data.result;
                //}

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingDeductibles, 'autoclose': true });
        });

    }

    static AddCoverage() {
        $('#mainCoverage').validate();
        if ($('#mainCoverage').valid()) {
            var coverageModel = RiskThirdPartyLiabilityCoverage.QuotationNewCoverage();
        }
    }

    static QuotationNewCoverage() {
        RiskThirdPartyLiabilityCoverage.ValidateCoverageRate();
        var coverage = null;
        var coverageModel = $('#mainCoverage').serializeObject();
        coverageModel.CoverageId = $('#selectCoverage').UifSelect('getSelected');
        coverageModel.Description = $('#selectCoverage').UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.DeclaredAmount = NotFormatMoney($('#inputDeclaredAmount').val());
        coverageModel.LimitAmount = NotFormatMoney($('#inputLimitAmount').val());
        coverageModel.LimitOccurrence = NotFormatMoney($('#inputLimitOccurrence').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.LimitClaimantAmount = NotFormatMoney($('#inputLimitClaimantAmount').val());
        coverageModel.MaxLiabilityAmount = NotFormatMoney($('#inputMaxLiabilityAmount').val());
        coverageModel.CalculationTypeId = $('#selectCalculeType').UifSelect('getSelected');
        coverageModel.RateType = $('#selectRateTypeCoverage').UifSelect('getSelected');
        coverageModel.Rate = NotFormatMoney($('#inputRateCoverage').val());
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumCoverage').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;

        if ($('#selectDeductible').UifSelect('getSelected') != "" || $('#selectDeductible').UifSelect('getSelected') != null) {
            coverageModel.DeductibleId = $('#selectDeductible').UifSelect('getSelected');
            coverageModel.DeductibleDescription = $('#selectDeductible').UifSelect('getSelectedText');
        }
        if (coverageDeductibles == null) {
            if ($("#selectDeductible").UifSelect("getSelected") > -1) {
                coverageModel.Deductible = {
                    Id: $("#selectDeductible").UifSelect("getSelected"),
                    Description: $("#selectDeductible").UifSelect('getSelectedText')
                };
                coverageDeductibles = coverageModel.Deductible;
            }
        } else {
            if ($("#selectDeductible").UifSelect("getSelected") == null) {
                coverageModel.Deductible = null;
            } else {
                coverageData.Deductible = coverageDeductibles;
            }
        }
        RiskThirdPartyLiabilityCoverageRequest.QuotationCoverage(coverageData, coverageModel).done(function (data) {
            if (data.success) {
                coverageModel = data.result;
                if (coverageModel != null) {
                    coverageModel.Description = $('#selectCoverage').UifSelect('getSelectedText');
                    coverageModel.SubLimitAmount = FormatMoney(coverageModel.SubLimitAmount);
                    coverageModel.DisplayRate = FormatMoney(coverageModel.Rate);
                    coverageModel.PremiumAmount = FormatMoney(coverageModel.PremiumAmount);
                    coverageModel.Text = coverageText;
                    coverageModel.Clauses = coverageClauses;
                    coverageModel.RateType = $('#selectRateTypeCoverage').UifSelect('getSelected');
                    coverageModel.TypeCoverage = RiskThirdPartyLiabilityCoverage.ValidateTypeCoverage(this);
                    if (coverageIndex == null) {
                        $('#listCoveragesEdit').UifListView('addItem', coverageModel);
                    }
                    else {
                        $('#listCoveragesEdit').UifListView('editItem', coverageIndex, coverageModel);
                    }

                    RiskThirdPartyLiabilityCoverage.UpdatePremium();
                    RiskThirdPartyLiabilityCoverage.ClearCoverage();
                }
            }
            else {
                coverage = "";
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });
    }

    static EditCoverage(index) {
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
            RiskThirdPartyLiabilityCoverage.SetCoverageEdit();
        }
        $('#selectCalculeType').UifSelect('setSelected', coverageData.CalculationType);
        $('#selectRateTypeCoverage').UifSelect('setSelected', coverageData.RateType);
        $('#inputDeclaredAmount').val(coverageData.DeclaredAmount);
        $('#inputLimitAmount').val(coverageData.LimitAmount);
        $('#inputLimitOccurrence').val(coverageData.LimitOccurrence);
        $('#inputSubLimitAmount').val(coverageData.SubLimitAmount);
        $('#inputLimitClaimantAmount').val(coverageData.LimitClaimantAmount);
        $('#inputMaxLiabilityAmount').val(coverageData.MaxLiabilityAmount);
        $('#inputRateCoverage').val(coverageData.Rate);
        $('#inputPremiumCoverage').val(coverageData.PremiumAmount);
        var deductibleId = 0;
        if (($('#selectDeductUnitCd Option').length == 0) && ($('#selectMinDeductUnitCd Option').length == 0) && ($('#selectMaxDeductUnitCd Option').length == 0)) {
            DeductiblesCoverage.LoadDeductibleUnit();
        }
        if (($('#selectDeductSubjectCd Option').length == 0) && ($('#selectMinDeductSubjectCd Option').length == 0) && ($('#selectMaxDeductSubjectCd Option').length == 0)) {
            DeductiblesCoverage.LoadDeductibleSubject();
        }
        if (coverageData.Deductible != null) {
            deductibleId = coverageData.Deductible.Id;
            coverageDeductibles = coverageData.Deductible;
            RiskThirdPartyLiabilityCoverage.GetDeductiblesByCoverageId(coverageData.Id, coverageData.Deductible.Id);
        }
        //else {
        //    RiskThirdPartyLiabilityCoverage.GetDeductiblesByCoverageId(coverageData.Id, 0);
        //}

        $('#mainCoverage').valid();
    }

    static DeleteCoverage(data) {
        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else {
            var coverages = $('#listCoveragesEdit').UifListView('getData');
            $('#listCoveragesEdit').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', height: 470 });

            $.each(coverages, function (index, value) {
                if (this.Id != data.Id) {
                    $('#listCoveragesEdit').UifListView('addItem', this);
                }
                else {
                    if (data.EndorsementType == EndorsementType.Modification) {
                        var coverage = RiskThirdPartyLiabilityCoverage.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                        if (coverage != null) {
                            RiskThirdPartyLiabilityCoverage.Rate = FormatMoney(coverage.Rate);
                            RiskThirdPartyLiabilityCoverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                            $('#listCoveragesEdit').UifListView('addItem', coverage);
                        }
                    }
                }

                RiskThirdPartyLiabilityCoverage.UpdatePremium();
            });
            RiskThirdPartyLiabilityCoverage.ClearCoverage();
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        RiskThirdPartyLiabilityCoverageRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
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

    static GetPremium() {
        RiskThirdPartyLiabilityCoverage.ValidCoverageValue();
        if (validCoverage) {
            var coverageModel = RiskThirdPartyLiabilityCoverage.QuotationCoverage();
            if (coverageModel != null) {
                coverageData = coverageModel;
                RiskThirdPartyLiabilityCoverage.EditCoverage(coverageIndex);
            }
            else {
                $('#inputPremiumCoverage').val(0);
            }
        }
    }

    static QuotationCoverage() {
        var coverage = null;

        var coverageModel = $('#mainCoverage').serializeObject();
        coverageModel.CoverageId = $('#selectCoverage').UifSelect('getSelected');
        coverageModel.Description = $('#selectCoverage').UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.DeclaredAmount = NotFormatMoney($('#inputDeclaredAmount').val());
        coverageModel.LimitAmount = NotFormatMoney($('#inputLimitAmount').val());
        coverageModel.LimitOccurrence = NotFormatMoney($('#inputLimitOccurrence').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.LimitClaimantAmount = NotFormatMoney($('#inputLimitClaimantAmount').val());
        coverageModel.MaxLiabilityAmount = NotFormatMoney($('#inputMaxLiabilityAmount').val());
        coverageModel.CalculationTypeId = $('#selectCalculeType').UifSelect('getSelected');
        coverageModel.RateType = $('#selectRateTypeCoverage').UifSelect('getSelected');
        coverageModel.Rate = NotFormatMoney($('#inputRateCoverage').val());
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumCoverage').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;

        if ($('#selectDeductible').UifSelect('getSelected') > 0) {
            coverageModel.DeductibleId = $('#selectDeductible').UifSelect('getSelected');
            coverageModel.DeductibleDescription = $('#selectDeductible').UifSelect('getSelectedText');
        }
        RiskThirdPartyLiabilityCoverageRequest.QuotationCoverage(coverageData, coverageModel).done(function (data) {
            if (data.success) {
                coverage = data.result;
                if (coverage != null) {
                    $('#inputPremiumCoverage').val(coverage.PremiumAmount);
                }
                else {
                    $('#inputPremiumCoverage').val(0);
                }
            }
            else {
                coverage = "";
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });

        return coverage;

    }

    static UpdatePremium() {
        var premium = 0;
        var totalLimitAmount = 0;

        $.each($('#listCoveragesEdit').UifListView('getData'), function (key, value) {
            premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
            totalLimitAmount += parseFloat(NotFormatMoney(this.LimitAmount).replace(separatorDecimal, separatorThousands));
        });

        $('#inputAmountInsured').text(FormatMoney(totalLimitAmount));
        $('#inputPremium').text(FormatMoney(premium.toFixed(2)));
    }

    static SaveCoverages() {
        var coveragesValues = $('#listCoveragesEdit').UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
        });
        RiskThirdPartyLiabilityCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, coveragesValues).done(function (data) {
            if (data.success) {
                router.run("prtRiskThirdPartyLiability");
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
        });
    }

    static ClearCoverage() {
        $('#selectCoverage').UifSelect();
        $('#selectCalculeType').UifSelect('setSelected', null);
        $('#inputDeclaredAmount').val(0);
        $('#inputSubLimitAmount').val(0);
        $('#inputLimitAmount').val(0);
        $('#inputLimitOccurrence').val(0);
        $('#inputLimitClaimantAmount').val(0);
        $('#inputMaxLiabilityAmount').val(0);
        $('#inputRateCoverage').val(0);
        $('#selectRateTypeCoverage').UifSelect('setSelected', null);
        $('#inputPremiumCoverage').val(0);
        $('#selectDeductible').UifSelect();
        $('#hiddenFlatRatePorcentage').val(0);
        $('#hiddenIsVisible').val(true);
        $('#hiddenIsSelected').val(false);
        $('#hiddenIsMandatory').val(false);

        coverageIndex = null;
        coverageText = null;
        coverageClauses = null;
    }

    static ReturnRisk() {
        RiskThirdPartyLiabilityCoverageRequest.ReturnRisk(glbPolicy.Id).done(function (data) {
            if (data.success) {
                var policyModelsView = data.result;
                policyModelsView.riskId = glbRisk.Id;

                $.redirect(rootPath + 'Underwriting/RiskThirdPartyLiability/ThirdPartyLiability', policyModelsView);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
        });
    }

    static ValidCoverageValue() {
        validCoverage = true;
        if (!$('#selectCoverage').UifSelect('getSelected') > 0) {
            validCoverage = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
        }

        if (!$('#selectCalculeType').UifSelect('getSelected') > 0) {
            validCoverage = false;
        }

        if ($.trim($('#inputSubLimitAmount').val()) == '' || $('#inputSubLimitAmount').val() == 0) {
            validCoverage = false;
        }

        if ($.trim($('#inputRateCoverage').val()) == '' || $('#inputRateCoverage').val() == 0) {
            validCoverage = false;
        }

        if (!$('#selectRateTypeCoverage').UifSelect('getSelected') > 0) {
            validCoverage = false;
        }
    }

    static LoadTitle() {
        var title = AppResources.LabelTitleCoverage + " - " + AppResources.LabeltitleRisk + ": " + glbRisk.LicensePlate + " " + glbRisk.Description;
        $.uif2.helpers.setGlobalTitle(title);
    }

    static UpdateAmounts() {
        $('#inputLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputLimitOccurrence').val($('#inputDeclaredAmount').val());
        $('#inputSubLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputLimitClaimantAmount').val($('#inputDeclaredAmount').val());
        $('#inputMaxLiabilityAmount').val($('#inputDeclaredAmount').val());
        $('#inputPremiumCoverage').val(0);
    }

    static newCoverage() {
        var coveragesAdd = '';

        $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
            if (coveragesAdd.length > 0) {
                coveragesAdd += ',';
            }
            coveragesAdd += this.Id;
        });
        RiskThirdPartyLiabilityCoverageRequest.GetCoveragesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd).done(function (data) {
            if (data.success) {
                $("#selectCoverage").UifSelect({ sourceData: data.result, id: "Id", name: "Description" });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });


    }
    static GetRateTypes() {
        RiskThirdPartyLiabilityRequest.GetRateTypes().done(function (data) {
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
    static ValidateTypeCoverage(model) {

        if (model.IsPrimary) {
            return AppResources.CoveragePrincipal;
        }
        else if (model.MainCoverageId > 0 && model.AllyCoverageId == null) {
            return AppResources.CoverageAdditional;
        }
        else if (model.AllyCoverageId > 0 && model.AllyCoverageId != null) {
            return AppResources.CoverageAllied
        }
        else {
            return AppResources.CoverageAdditional;
        }

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

}

