var AmountMinimum;
var Isprimary = false;
var IsEdit = false;
class RiskLiabilityCoverage extends Uif2.Page {
    getInitialState() {
        coverageData = { Id: 0 };
        coverageIndex = null;
        coverageText = null;
        coverageClauses = null;
        coverageDeductibles = null;
        validCoverage = true;

        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $('#inputFromCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
        $('#inputToCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentTo));
        $('#inputDaysCoverage').val(CalculateDays($('#inputFromCoverage').val(), $('#inputToCoverage').val()));
        $('#hiddenCoverageId').val(glbCoverage.CoverageId);

        RiskLiabilityCoverage.ShowPanelsCoverage(MenuType.Coverage);

        // $("#inputDeclaredAmount").ValidatorKey(1, 0, 0);
        // $("#inputLimitAmount").ValidatorKey(1, 0, 0);
        //$("#inputSubLimitAmount").ValidatorKey(1, 0, 0);
        $("#inputLimitOccurrenceAmount").ValidatorKey(1, 0, 0);
        //$("#inputLimitClaimantAmount").ValidatorKey(1, 0, 0);
        //$("#inputMaxLiabilityAmount").ValidatorKey(1, 0, 0);
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
            $("#inputRate").OnlyDecimals(3);
        } else {
            $("#inputRate").OnlyDecimals1(3);
        }
        $("#inputPremiumAmount").OnlyDecimals(UnderwritingDecimal);

        $("#inputAmountInsured").text(FormatMoney(glbRisk.AmountInsured));
        $("#inputTotalPremium").text(FormatMoney(glbRisk.Premium));

        $("#inputDeclaredAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputLimitAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputSubLimitAmount").OnlyDecimals(UnderwritingDecimal);

        $("#inputMaxLiabilityAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputLimitClaimantAmount").OnlyDecimals(UnderwritingDecimal);

        RiskLiabilityCoverage.GetCoveragesByRiskId(glbRisk.Id);
        RiskLiabilityCoverage.LoadTitle();
    }

    //Eventos
    bindEvents() {
        $("#inputDeclaredAmount").focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
        });

        $("#inputDeclaredAmount").focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));

            $("#inputLimitAmount").val($(this).val());
            $("#inputSubLimitAmount").val($(this).val());
            $("#inputLimitOccurrenceAmount").val($(this).val());
            $("#inputLimitClaimantAmount").val($(this).val());
            $("#inputMaxLiabilityAmount").val($(this).val());
        });

        $("#inputLimitAmount").focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
            $('#inputPremiumAmount').val(0);
        });

        $("#inputLimitAmount").focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            $('#inputPremiumAmount').val(0);
        });

        $("#inputSubLimitAmount").focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
            $('#inputPremiumAmount').val(0);
        });

        $("#inputSubLimitAmount").focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            $('#inputPremiumAmount').val(0);
        });

        $("#inputLimitOccurrenceAmount").focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
            $('#inputPremiumAmount').val(0);
        });

        $("#inputLimitOccurrenceAmount").focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            $('#inputPremiumAmount').val(0);
        });

        $("#inputLimitClaimantAmount").focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
            $('#inputPremiumAmount').val(0);
        });

        $("#inputLimitClaimantAmount").focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            $('#inputPremiumAmount').val(0);
        });

        $("#inputMaxLiabilityAmount").focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
            $('#inputPremiumAmount').val(0);
        });

        $("#inputMaxLiabilityAmount").focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
            $('#inputPremiumAmount').val(0);
        });

        $('#selectCalculeType').on('itemSelected', function (event, selectedItem) {
            $('#inputPremiumAmount').val(0);
        });

        $('#selectRateType').on('itemSelected', function (event, selectedItem) {
            $('#inputPremiumAmount').val(0);
            if (selectedItem.Id > 0) {
                var RateCoverage = parseInt(selectedItem.Id);
                switch (RateCoverage) {
                    case RateType.Percentage:
                        $("#inputRate").prop("maxLength", 8);
                        break;
                    case RateType.Permilage:
                        $("#inputRate").prop("maxLength", 9);
                        break;
                    default:
                        $("#inputRate").prop("maxLength", 20);
                        break;
                }
            }
        });

        $("#inputRate").focusout(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == "" ? $(this).val(0) : $(this).val(FormatMoney($(this).val()));
        });

        $("#inputRate").focusin(function (event) {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(value);
        });

        $('#listCoveragesEdit').on('rowAdd', function (event) {
            RiskLiabilityCoverage.ClearCoverage();
            RiskLiabilityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
            $("#selectDeductible").UifSelect();
        });

        $('#listCoveragesEdit').on('rowEdit', function (event, data, index) {
            coverageData = Object.assign({}, data);
            coverageIndex = index;
            RiskLiabilityCoverage.EditCoverage(index);
        });

        $('#listCoveragesEdit').on('rowDelete', function (event, data) {
            RiskLiabilityCoverage.DeleteCoverage(data);
        });

        $('#selectCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                RiskLiabilityCoverage.GetCoverageByCoverageId(selectedItem.Id);
                RiskLiabilityCoverage.GetDeductiblesByCoverageId($("#selectCoverage").UifSelect("getSelected"), null);
            }
            else {
                $('#selectDeductible').UifSelect();
            }
        });

        $("#btnAcceptCoverage").on("click", function () {
            RiskLiabilityCoverage.AddCoverage();
        });

        $("#btnCancelCoverage").on("click", function () {
            RiskLiabilityCoverage.ClearCoverage();
            RiskLiabilityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        });


        $("#btnCloseCoverage").on("click", function () {
            router.run("prtRiskLiability");
        });

        $("#btnSaveCoverage").on("click", function () {
            RiskLiabilityCoverage.SaveCoverages();
        });
        $('#btnDeductibleCoverage').on('click', RiskLiabilityCoverage.GetDeductibleCoverage);
        $('#btnCalculateCoverage').on('click', RiskLiabilityCoverage.GetPremium.bind(false, false));
        $("#inputFromCoverage").on("datepicker.change", (event, date) => { RiskLiabilityCoverage.DateFromCoverage(event, date) });
        $("#inputToCoverage").on("datepicker.change", (event, date) => { RiskLiabilityCoverage.DateToCoverage(event, date) });
    }

    //Ocultar paneles
    static HidePanelCoverage(Menu) {

        switch (Menu) {
            case MenuType.Coverage:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('hide');
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('hide');
                break;
            case MenuType.Deductibles:
                $("#modalDeductibles").UifModal('hide');
                break;
        }
    }
    //Carga de datos y visualización de la ventana modal.
    static GetDeductibleCoverage() {
        if (($('#selectDeductUnitCd Option').length == 0) && ($('#selectMinDeductUnitCd Option').length == 0) && ($('#selectMaxDeductUnitCd Option').length == 0)) {
            DeductiblesCoverage.LoadDeductibleUnit();
        }
        if (($('#selectDeductSubjectCd Option').length == 0) && ($('#selectMinDeductSubjectCd Option').length == 0) && ($('#selectMaxDeductSubjectCd Option').length == 0)) {
            DeductiblesCoverage.LoadDeductibleSubject();
        }
        DeductiblesCoverage.GetRateTypesDeduct();
        DeductiblesCoverage.FormatDeductibles();
        if (($('#selectDeductible').UifSelect('getSelected') != '') && ($('#selectDeductible').UifSelect('getSelected') != null)) {
            var deductibleId = $('#selectDeductible').UifSelect('getSelected');

            DeductiblesCoverage.GetDeductibles(deductibleId);
        }
        if (errorLoad == 0) {
            if (glbCoverage.Class == undefined) {
                window[glbCoverage.Object].ShowPanelsCoverage(MenuType.Deductibles);
            }
            else {
                glbCoverage.Class.ShowPanelsCoverage(MenuType.Deductibles);
            }
        }
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

    static DateFromCoverage(event, date) {
        if (CompareDates(glbPolicy.CurrentFrom, $("#inputFromCoverage").val()) == 1) {
            $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateInitialPolicyDate, 'autoclose': true });
        }
        else if (CompareDates($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 1) {
            $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateInitialDate, 'autoclose': true });
        }
        else if (CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 0) {
            $("#inputFromCoverage").UifDatepicker('setValue', glbPolicy.CurrentFrom);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateInitialDate, 'autoclose': true });
        }
        else {
            $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()));
        }

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && enumModificationType.Prorogation == glbPolicy.Endorsement.ModificationTypeId) {
            if (CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()) > 1) {
                $("#inputFromCoverage").UifDatepicker('setValue', $("#inputToCoverage").val());
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateProrogation, 'autoclose': true });
            }
        }

    }

    static DateToCoverage(event, date) {
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && enumModificationType.Modification == glbPolicy.Endorsement.ModificationTypeId) {
            if (CompareDates($("#inputToCoverage").val(), glbCoverage.CurrentTo) == 1) {
                $("#inputToCoverage").UifDatepicker('setValue', coverageData.CurrentTo);
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateModification, 'autoclose': true });
            }

        }
        if (CompareDates($("#inputToCoverage").val(), glbPolicy.CurrentTo) == 1) {
            $("#inputToCoverage").UifDatepicker('setValue', glbPolicy.CurrentTo);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalPolicyDate, 'autoclose': true });
        }
        else if (CompareDates($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 1) {
            $("#inputToCoverage").UifDatepicker('setValue', glbPolicy.CurrentTo);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalDate, 'autoclose': true });
        }
        else if (CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()) == 0) {
            $("#inputToCoverage").UifDatepicker('setValue', glbPolicy.CurrentTo);
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateFinalDate, 'autoclose': true });
        }
        else {
            $("#inputDaysCoverage").val(CalculateDays($("#inputFromCoverage").val(), $("#inputToCoverage").val()));
        }
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
            return AppResources.CoverageAdditional
        }
    }

    static LoadCoverage(coverages) {

        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });

        $.each(coverages, function (index, val) {
            coverages[index].DeclaredAmount = FormatMoney(coverages[index].DeclaredAmount);
            coverages[index].LimitAmount = FormatMoney(coverages[index].LimitAmount);
            coverages[index].SubLimitAmount = FormatMoney(coverages[index].SubLimitAmount);
            coverages[index].LimitOccurrenceAmount = FormatMoney(coverages[index].LimitOccurrenceAmount);
            coverages[index].LimitClaimantAmount = FormatMoney(coverages[index].LimitClaimantAmount);
            coverages[index].MaxLiabilityAmount = FormatMoney(coverages[index].MaxLiabilityAmount);
            coverages[index].DisplayRate = FormatMoney(coverages[index].Rate, 2);
            coverages[index].Rate = FormatMoney(coverages[index].Rate);
            coverages[index].PremiumAmount = FormatMoney(coverages[index].PremiumAmount);
            coverages[index].FlatRatePorcentage = FormatMoney(coverages[index].FlatRatePorcentage);
            coverages[index].TypeCoverage = RiskLiabilityCoverage.ValidateTypeCoverage(coverages[index]);
            coverages[index].PreRuleSetId = FormatMoney(coverages[index].PreRuleSetId);
            coverages[index].ruleSetId = FormatMoney(coverages[index].ruleSetId);

            if (coverages[index].AllyCoverageId != null) {
                coverages[index].allowEdit = false;
                coverages[index].allowDelete = false;
            } else {
                coverages[index].allowEdit = true;
                coverages[index].allowDelete = true;
            }

            $("#listCoveragesEdit").UifListView("addItem", coverages[index]);
        });

        RiskLiabilityCoverage.UpdatePremium();

        if ($("#hiddenCoverageId").val() > 0) {
            $.each(coverages, function (key, value) {
                if ($('#hiddenCoverageId').val() == this.Id) {
                    coverageIndex = key;
                    coverageData = this;
                }
            });

            RiskLiabilityCoverage.EditCoverage(coverageIndex);
        }
        else {
            RiskLiabilityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        }
        RiskLiabilityCoverage.GetRateTypes(coverageData.RateType);

    }

    static SetCoverageEdit() {
        var coveragesData = [];
        coveragesData.push(coverageData);

        $("#selectCoverage").UifSelect();
        $("#selectCoverage").UifSelect({ sourceData: coveragesData, id: "Id", name: "Description", selectedId: coverageData.Id });
        $("#selectCoverage").prop('disabled', true);
    }

    static GetCoveragesByRiskId(riskId) {
        var coverages = null;

        RiskLiabilityCoverageRequest.GetCoveragesByRiskId(riskId).done(function (data) {
            if (data.success) {
                coverages = data.result;
                if (IsEditing) {
                    RiskLiabilityCoverage.LoadCoverage(data.result);
                } else {
                    RiskLiabilityCoverageRequest.RunRulesCoverage(glbPolicy.Id, riskId, coverages).done(function (data) {
                        RiskLiabilityCoverage.LoadCoverage(data.result);
                    }).fail(function () {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRunRules, 'autoclose': true });
                    });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverages, 'autoclose': true });
        });
    }

    static GetCoveragesByProductIdGroupCoverageIdPrefixId() {
        var coveragesAdd = '';

        $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
            if (coveragesAdd.length > 0) {
                coveragesAdd += ',';
            }
            coveragesAdd += this.Id;
        });

        RiskLiabilityCoverageRequest.GetCoveragesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd).done(function (data) {
            if (data.success) {
                $("#selectCoverage").UifSelect({ sourceData: data.result, id: "Id", name: "Description" });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetCoverageByCoverageId(coverageId) {
        RiskLiabilityCoverageRequest.GetCoverageByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                coverageData = data.result;
                RiskLiabilityCoverage.EditCoverage(null);
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
            RiskLiabilityCoverageRequest.GetCoverageByCoverageId(coverageId).done(function (data) {
                if (data.success) {
                    coverage.push(data.result);
                }
            });
        }
        //Se obtiene la cobertura actual
        RiskLiabilityCoverageRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
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

    static AddCoverage() {

        $("#mainCoverage").validate();
        var coverageEdit = null;
        if ($("#mainCoverage").valid()) {
            if (RiskLiabilityCoverage.ValidateCoverageRate()) {
                var selectedCoverage = $("#selectCoverage").UifSelect('getSelected');
                if (selectedCoverage != "" && selectedCoverage != null) {
                    var coverageModel = RiskLiabilityCoverage.QuotationCoverage(true, true);
                    if (coverageModel != null) {
                        coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
                        coverageModel.DeclaredAmount = FormatMoney(coverageModel.DeclaredAmount);
                        coverageModel.LimitAmount = FormatMoney(coverageModel.LimitAmount);
                        coverageModel.SubLimitAmount = FormatMoney(coverageModel.SubLimitAmount);
                        coverageModel.LimitOccurrenceAmount = FormatMoney(coverageModel.LimitOccurrenceAmount);
                        coverageModel.LimitClaimantAmount = FormatMoney(coverageModel.LimitClaimantAmount);
                        coverageModel.MaxLiabilityAmount = FormatMoney(coverageModel.MaxLiabilityAmount);
                        coverageModel.DisplayRate = FormatMoney(coverageModel.Rate);
                        coverageModel.FlatRatePorcentage = FormatMoney(coverageModel.FlatRatePorcentage);
                        coverageModel.Text = coverageText;
                        if (clausesCoverage != null && clausesCoverage.length > 0) {
                            coverageModel.Clauses = clausesCoverage;
                            clausesCoverage = null;
                        }
                        coverageModel.IsPrimary = coverageModel.IsPrimary;
                        if (coverageModel.IsPrimary == true) {
                            coverageModel.Rate = FormatMoney(coverageModel.Rate);
                            coverageModel.PremiumAmount = FormatMoney(coverageModel.PremiumAmount);
                        } else {
                            coverageModel.Rate = 0;
                            coverageModel.PremiumAmount = 0;
                        }
                        coverageModel.MainCoverageId = coverageModel.MainCoverageId;
                        coverageModel.TypeCoverage = RiskLiabilityCoverage.ValidateTypeCoverage(coverageModel);

                        var claimAmount = parseFloat(NotFormatMoney($("#inputLimitClaimantAmount").val()).replace(separatorDecimal, separatorThousands));
                        var subLimit = parseFloat(NotFormatMoney($("#inputSubLimitAmount").val()).replace(separatorDecimal, separatorThousands));
                        if (claimAmount > subLimit) {

                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorLimitClaim, 'autoclose': true });
                            return false;
                        }
                        if ($("#selectDeductible").UifSelect("getSelected") > 0) {
                            coverageModel.Deductible = $('#selectDeductible').UifSelect("getSelectedSource");
                            if (coverageModel.Deductible != null) {
                                coverageModel.Deductible.Description = $('#selectDeductible').UifSelect("getSelectedText");
                            }
                        }

                        if (coverageModel.MainCoverageId > 0) {
                            coverageModel.allowEdit = false;
                            coverageModel.allowDelete = false;
                        } else {
                            coverageModel.allowEdit = true;
                            coverageModel.allowDelete = true;
                        }

                        if (coverageIndex == null) {
                            var coverages = $("#listCoveragesEdit").UifListView('getData');

                            let coveragePrimary = coverages.find(item => item.IsPrimary);
                            if (coveragePrimary) {
                                AmountMinimum = parseInt(NotFormatMoney(coveragePrimary.LimitAmount));

                                if (AmountMinimum >= parseFloat(NotFormatMoney($("#inputDeclaredAmount").val()))) {
                                    $("#listCoveragesEdit").UifListView("addItem", coverageModel);
                                } else {
                                    RiskLiabilityCoverage.ClearCoverage();
                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSummaryCoverage, 'autoclose': true });
                                }
                            } else {
                                AmountMinimum = parseInt(NotFormatMoney($("#inputLimitAmount").val()));

                                if (coverageModel.IsPrimary) {
                                    if (AmountMinimum >= parseFloat(NotFormatMoney($("#inputDeclaredAmount").val()))) {
                                        $("#listCoveragesEdit").UifListView("addItem", coverageModel);
                                    } else {
                                        RiskLiabilityCoverage.ClearCoverage();
                                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSummaryCoverage, 'autoclose': true });
                                    }
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorPrincipalCoverageNotFound, 'autoclose': true });
                                }
                            }
                        }
                        else {
                            $("#listCoveragesEdit").UifListView("editItem", coverageIndex, coverageModel);
                        }
                        /*Valida el calculo para las coberturas adicionales asociadas a la cobertura principal editada*/
                        RiskLiabilityCoverage.GetAddCoverageByCoverage(coverageModel);
                        /*Valida el calculo para las coberturas aliadas asociadas a la cobertura principal editada*/
                        RiskLiabilityCoverage.GetAllyCoverageByCoverage(coverageModel);
                        RiskLiabilityCoverage.LoadSubTitles(0);
                        RiskLiabilityCoverage.UpdatePremium();
                        RiskLiabilityCoverage.ClearCoverage();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
                }
            }
        }
    }

    static RunRulesCoverages(temporalId, ruleSetId) {
        RiskLiabilityRequest.RunRulesCoverage(temporalId, risk.Coverages, ruleSetId).done(function (data) {
            RiskLiabilityCoverage.LoadCoverage(data.result);
        }).fail(function () {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRunRules, 'autoclose': true });
        });
    }



    static EditCoverage(index) {

        if (coverageData[0] != undefined) {
            coverageData = coverageData[0];
        }
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
            RiskLiabilityCoverage.SetCoverageEdit();
        }

        $("#selectCalculeType").UifSelect("setSelected", coverageData.CalculationType);
        $("#selectRateType").UifSelect('setSelected', coverageData.RateType);
        $("#inputDeclaredAmount").val(coverageData.DeclaredAmount);
        $("#inputLimitAmount").val(coverageData.LimitAmount);
        $("#inputSubLimitAmount").val(coverageData.SubLimitAmount);
        $("#inputLimitOccurrenceAmount").val(coverageData.LimitOccurrenceAmount);
        $("#inputLimitClaimantAmount").val(coverageData.LimitClaimantAmount);
        $("#inputMaxLiabilityAmount").val(coverageData.MaxLiabilityAmount);
        $("#inputRate").val(coverageData.Rate);
        $("#inputPremiumAmount").val(coverageData.PremiumAmount);

        InsuredObjectRequest.GetCoveragesByCoverageId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coverageData.Id).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $("#selectCoverage").UifSelect({ sourceData: data.result, id: "Id", name: "Description", selectedId: coverageData.Id });
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
                    RiskLiabilityCoverage.GetDeductiblesByCoverageId(coverageData.Id, deductibleId);
                } else {
                    RiskLiabilityCoverage.GetDeductiblesByCoverageId(coverageData.Id, null);
                }
                RiskLiabilityCoverage.LoadSubTitles(0);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.Error, 'autoclose': true });
        });

        $("#mainCoverage").valid();
    }

    static DeleteCoverage(data) {

        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
        }
        else {
            var coverages = $("#listCoveragesEdit").UifListView('getData');
            if (coverages != null && coverages != "" && coverages.length == 1 && data.EndorsementType == EndorsementType.Modification) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
            }
            else {
                $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });

                $.each(coverages, function (index, value) {
                    if (this.Id != data.Id) {
                        $("#listCoveragesEdit").UifListView("addItem", this);
                    }
                    else {
                        if (data.EndorsementType == EndorsementType.Modification && data.RiskCoverageId > 0) {
                            var coverage = RiskLiabilityCoverage.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                            if (coverage != null) {
                                coverage.Rate = FormatMoney(coverage.Rate);
                                coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                                coverage.DisplayRate = FormatMoney(coverage.Rate);
                                $("#listCoveragesEdit").UifListView("addItem", coverage);
                            }
                        }
                    }
                });
                RiskLiabilityCoverage.ClearCoverage();
                RiskLiabilityCoverage.UpdatePremium();
            }
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;

        RiskLiabilityCoverageRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
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

    static GetPremium(runRules) {

        RiskLiabilityCoverage.ValidCoverageValue();
        if (validCoverage) {
            var coverageModel = RiskLiabilityCoverage.QuotationCoverage(false, runRules);
            if (coverageModel != null) {
                $("#selectCalculeType").UifSelect("setSelected", coverageModel.CalculationType);
                $("#inputDeclaredAmount").val(FormatMoney(coverageModel.DeclaredAmount));
                $("#inputLimitAmount").val(FormatMoney(coverageModel.LimitAmount));
                $("#inputSubLimitAmount").val(FormatMoney(coverageModel.SubLimitAmount));
                $("#inputLimitOccurrenceAmount").val(FormatMoney(coverageModel.LimitOccurrenceAmount));
                $("#inputLimitClaimantAmount").val(FormatMoney(coverageModel.LimitClaimantAmount));
                $("#inputMaxLiabilityAmount").val(FormatMoney(coverageModel.MaxLiabilityAmount));
                $("#inputRate").val(FormatMoney(coverageModel.Rate));
                $("#selectRateType").UifSelect("setSelected", coverageModel.RateType);
                $("#inputPremiumAmount").val(FormatMoney(coverageModel.PremiumAmount));

                if (coverageModel.Deductible != null && coverageModel.Deductible.Id > 0) {
                    $("#selectDeductible").UifSelect("setSelected", coverageModel.Deductible.Id);
                }
            }
            else {
                $("#inputPremiumAmount").val(0);
            }
        }
    }
    static GetFormCoverage() {
        var coverage = coverageData;

        coverage.Description = $("#selectCoverage").UifSelect("getSelectedText");
        coverage.CurrentFrom = $("#inputFromCoverage").val();
        $("#inputFromCoverage").data("dateFromCoverage", $("#inputFromCoverage").val());
        coverage.CurrentTo = $("#inputToCoverage").val();
        $("#inputToCoverage").data("dateToCoverage", $("#inputToCoverage").val());

        if ($.trim($("#inputLimitAmount").val()) != "") {
            coverage.LimitAmount = parseFloat(NotFormatMoney($("#inputLimitAmount").val()).replace(separatorDecimal, '.'));
        }
        else {
            coverage.LimitAmount = 0;
        }
        if ($.trim($("#inputLimitAmount").val()) != "") {
            coverage.Amount = NotFormatMoney($("#inputLimitAmount").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.Amount = 0;
        }
        if ($.trim($("#inputSubLimitAmount").val()) != "") {
            coverage.SubLimitAmount = parseFloat(NotFormatMoney($("#inputSubLimitAmount").val()).replace(separatorDecimal, '.'));
        }
        else {
            coverage.SubLimitAmount = 0;
        }
        if ($.trim($("#inputPremiumAmount").val()) != "") {
            coverage.PremiumAmount = NotFormatMoney($("#inputPremiumAmount").val()).replace(separatorDecimal, '.');
        }
        else {
            coverage.PremiumAmount = 0;
        }

        if ($.trim($("#inputLimitClaimantAmount").val()) != "") {
            coverage.LimitClaimantAmount = parseFloat(NotFormatMoney($("#inputLimitClaimantAmount").val()).replace(separatorDecimal, '.'));
        }
        else {
            coverage.LimitClaimantAmount = 0;
        }
        if ($.trim($("#inputLimitOccurrenceAmount").val()) != "") {
            coverage.LimitOccurrenceAmount = parseFloat(NotFormatMoney($("#inputLimitOccurrenceAmount").val()).replace(separatorDecimal, '.'));
        }
        else {
            coverage.LimitOccurrenceAmount = 0;
        }

        if ($.trim($("#inputRate").val()) != "") {
            coverage.Rate = NotFormatMoney($("#inputRate").val(), 4);
        }
        else {
            coverage.Rate = 0;
        }
        if ($.trim($("#inputDeclaredAmount").val()) != "") {
            coverage.DeclaredAmount = NotFormatMoney($("#inputDeclaredAmount").val());
        }
        else {
            coverage.DeclaredAmount = 0;
        }
        if ($.trim($("#inputMaxLiabilityAmount").val()) != "") {
            coverage.MaxLiabilityAmount = parseFloat(NotFormatMoney($("#inputMaxLiabilityAmount").val()).replace(separatorDecimal, '.'));
        } else {
            coverage.MaxLiabilityAmount = 0;
        }
        coverage.FirstRiskType = $("#selectMeasurementBenefit").UifSelect("getSelected");
        coverage.CalculationType = $("#selectCalculeType").UifSelect("getSelected");
        coverage.RateType = $("#selectRateType").UifSelect("getSelected");
        coverage.Text = coverageText;
        coverage.Clauses = coverageClauses;
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


    static QuotationCoverage(reload, runRules) {
        coverageData = RiskLiabilityCoverage.GetFormCoverage();
        if (glbPolicy.Endorsement.EndorsementType == 2) {
            coverageData.ModificationTypeId = glbPolicyEndorsement.Endorsement.ModificationTypeId;
        }
        RiskLiabilityCoverageRequest.QuotationCoverage(coverageData, glbRisk.Id, runRules).done(function (data) {
            if (data.success) {
                if (glbPolicy.Endorsement.EndorsementType == 2) {
                    if (glbPolicyEndorsement.Endorsement.ModificationTypeId == 4) {
                        glbRisk.Coverages.forEach(function (item, index) {
                            if (data.result.length != undefined) {
                                data.result.forEach(function (item1, index) {
                                    if (item.Id == item1.Id && parseInt($("#selectCoverage").val()) == item.Id && $("#selectRateType").val() == 1) {
                                        var premium1 = item.OriginalSubLimitAmount * (item.OriginalRate / 100) * CalculateDays(item.CurrentFromOriginal, item.CurrentToOriginal) / 365;
                                        item1.PremiumAmount = (premium1 - item1.PremiumAmount) * -1;
                                    }
                                });
                            } else {
                                if (item.Id == data.result.Id && parseInt($("#selectCoverage").val()) == item.Id && $("#selectRateType").val() == 1) {
                                    var premium1 = item.OriginalSubLimitAmount * (item.OriginalRate / 100) * CalculateDays(FormatDate(glbPolicy.Endorsement.CurrentFrom), FormatDate(glbPolicy.Endorsement.CurrentTo)) / 365;
                                    data.result.PremiumAmount = (premium1 - data.result.PremiumAmount) * -1;
                                }
                            }
                        });
                    }
                }
                if (reload) {
                    IsEdit = reload
                }
                coverageData = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });

        return coverageData;
    }

    static UpdatePremium() {
        var insuredAmount = 0;
        var premium = 0;

        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            if (this.IsPrimary == true) {
                insuredAmount += parseFloat(NotFormatMoney(this.LimitAmount).replace(separatorDecimal, separatorThousands));
                premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
            }

        });

        $("#inputAmountInsured").text(FormatMoney(insuredAmount));
        $("#inputTotalPremium").text(FormatMoney(premium));
    }

    static SaveCoverages() {
        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
        var coveragePrimary = coveragesValues.find(item => item.IsPrimary);
        var result = false;
        var cover = 0;
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate($('#inputFromCoverage').val());
            this.CurrentTo = FormatDate($('#inputToCoverage').val());
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.LimitOccurrenceAmount = NotFormatMoney(this.LimitOccurrenceAmount);
            this.LimitClaimantAmount = NotFormatMoney(this.LimitClaimantAmount);
            this.MaxLiabilityAmount = NotFormatMoney(this.MaxLiabilityAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);

        });
      
        RiskLiabilityCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, coveragesValues).done(function (data) {
            if (data.success) {
                if (glbPolicy.Endorsement.EndorsementType == 2) {
                    glbRisk.Coverages = coveragesValues;
                    RiskLiability.UpdatePolicyComponents(false);
                    
                }
                router.run("prtRiskLiability");
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
        });
        //}

    }

    static ClearCoverage() {
        $('#selectCoverage').UifSelect();
        $("#selectCalculeType").UifSelect("setSelected", null);
        $('#inputDeclaredAmount').val(0);
        $('#inputLimitAmount').val(0);
        $('#inputSubLimitAmount').val(0);
        $('#inputLimitOccurrenceAmount').val(0);
        $('#inputLimitClaimantAmount').val(0);
        $('#inputMaxLiabilityAmount').val(0);
        $('#inputRate').val(0);
        $("#selectRateType").UifSelect("setSelected", null);
        $('#inputPremiumAmount').val(0);
        $('#selectDeductible').UifSelect();
        $("#hiddenFlatRatePorcentage").val(0);
        $("#hiddenIsVisible").val(true);
        $("#hiddenIsSelected").val(false);
        $("#hiddenIsMandatory").val(false);
        $('#selectedTexts').text("");
        $('#selectedClauses').text("");
        $("#selectRateTypeDeduct").UifSelect("setSelected", null);
        $('#rateDeductible').val(0);

        coverageIndex = null;
        coverageText = null;
        coverageClauses = null;
        coverageDeductibles = null;
    }

    static ReturnRisk() {
        RiskLiabilityCoverageRequest.ReturnRisk(glbPolicy.Id).done(function (data) {
            if (data.success) {
                var policyModelsView = data.result;
                policyModelsView.riskId = glbRisk.Id;

                $.redirect(rootPath + 'Underwriting/RiskLiability/Liability', policyModelsView);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
        });
    }

    static ValidCoverageValue() {
        validCoverage = true;
        if ($("#selectCoverage").UifSelect("getSelected") == null || $("#selectCoverage").UifSelect("getSelected") == 0) {
            validCoverage = false;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
        }

        if ($("#selectCalculeType").UifSelect("getSelected") == null || $("#selectCalculeType").UifSelect("getSelected") == 0) {
            validCoverage = false;
        }

        if ($.trim($("#inputSubLimitAmount").val()) == '' || $("#inputSubLimitAmount").val() == 0) {
            validCoverage = false;
        }

        //if ($.trim($("#inputRate").val()) == '' || $("#inputRate").val() == 0) {
        //    validCoverage = false;
        //}
        if ($("#selectRateType").UifSelect("getSelected") == null || $("#selectRateType").UifSelect("getSelected") == 0) {
            validCoverage = false;
        }
    }

    static LoadTitle() {
        var title = AppResources.LabelTitleCoverage + " - " + AppResources.LabeltitleRisk + ": " + glbRisk.FullAddress;
        $.uif2.helpers.setGlobalTitle(title);
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

    static GetAllyCoverageByCoverage(coverageData) {
        coverageData.DeclaredAmount = NotFormatMoney(coverageData.DeclaredAmount);
        coverageData.SublimitPercentage = NotFormatMoney(coverageData.SublimitPercentage);

        RiskLiabilityCoverageRequest.GetAllyCoverageByCoverage(glbPolicy.Id, glbRisk.Id, glbRisk.GroupCoverage.Id, coverageData).done(function (data) {
            if (data.success) {
                for (var i = 0; i < data.result.length; i++) {

                    const finCoverage = function (element, index, array) {
                        return element.AllyCoverageId === data.result[i].Id;
                    }
                    var index = $("#listCoveragesEdit").UifListView("findIndex", finCoverage);
                    var coverage = $("#listCoveragesEdit").UifListView('getData').filter(function (coverages) {
                        return coverages.Id === data.result[i].Id;
                    });

                    if (coverage.length == 0) {
                        $("#listCoveragesEdit").UifListView("addItem", data.result[i]);
                    } else {
                        coverage = coverage[0];
                        coverage.Description = data.result[i].Description;
                        coverage.LimitAmount = FormatMoney(data.result[i].LimitAmount);
                        coverage.SubLimitAmount = FormatMoney(data.result[i].SubLimitAmount);
                        coverage.DeclaredAmount = FormatMoney(data.result[i].DeclaredAmount);
                        coverage.PremiumAmount = FormatMoney(data.result[i].PremiumAmount);
                        coverage.LimitOccurrenceAmount = FormatMoney(data.result[i].LimitOccurrenceAmount);
                        coverage.LimitClaimantAmount = FormatMoney(data.result[i].LimitClaimantAmount);
                        coverage.MaxLiabilityAmount = FormatMoney(data.result[i].MaxLiabilityAmount);
                        coverage.Rate = FormatMoney(data.result[i].Rate);
                        coverage.DisplayRate = FormatMoney(data.result[i].Rate, 2);
                        coverage.FlatRatePorcentage = FormatMoney(data.result[i].FlatRatePorcentage);
                        coverage.ruleSetId = FormatMoney(data.result[i].ruleSetId);
                        coverage.PreRuleSetId = FormatMoney(data.result[i].PreRuleSetId);
                        if (data.result[i].AllyCoverageId != null) {
                            coverage.allowEdit = false;
                            coverage.allowDelete = false;
                        } else {
                            coverage.allowEdit = true;
                            coverage.allowDelete = true;
                        }

                        $("#listCoveragesEdit").UifListView("editItem", index, coverage);
                    }
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });
        /*Vuelve a asignar los formatos*/
        coverageData.DeclaredAmount = FormatMoney(coverageData.DeclaredAmount);
        coverageData.SublimitPercentage = FormatMoney(coverageData.SublimitPercentage);
    }

    static GetAddCoverageByCoverage(coverageData) {
        coverageData.DeclaredAmount = NotFormatMoney(coverageData.DeclaredAmount);
        coverageData.SublimitPercentage = NotFormatMoney(coverageData.SublimitPercentage);
        coverageData.Rate = coverageData.Rate;
        coverageData.RateType = coverageData.RateType;
        coverageData.CalculationType = coverageData.CalculationType;

        RiskLiabilityCoverageRequest.GetAddCoverageByCoverage(glbPolicy.Id, glbRisk.Id, glbRisk.GroupCoverage.Id, coverageData).done(function (data) {
            if (data.success) {
                for (var i = 0; i < data.result.length; i++) {

                    const finCoverage = function (element, index, array) {
                        return element.Id === data.result[i].Id;
                    }
                    var index = $("#listCoveragesEdit").UifListView("findIndex", finCoverage);
                    var coverage = $("#listCoveragesEdit").UifListView('getData').filter(function (coverages) {
                        return coverages.Id === data.result[i].Id;
                    });

                    if (coverage.length == 0) {
                        $("#listCoveragesEdit").UifListView("addItem", data.result[i]);
                    } else {
                        coverage = coverage[0];
                        coverage.Description = data.result[i].Description;
                        coverage.LimitAmount = FormatMoney(data.result[i].LimitAmount);
                        coverage.SubLimitAmount = FormatMoney(data.result[i].SubLimitAmount);
                        coverage.DeclaredAmount = FormatMoney(data.result[i].DeclaredAmount);
                        coverage.PremiumAmount = FormatMoney(data.result[i].PremiumAmount);
                        coverage.LimitOccurrenceAmount = FormatMoney(data.result[i].LimitOccurrenceAmount);
                        coverage.LimitClaimantAmount = FormatMoney(data.result[i].LimitClaimantAmount);
                        coverage.MaxLiabilityAmount = FormatMoney(data.result[i].MaxLiabilityAmount);
                        coverage.Rate = FormatMoney(data.result[i].Rate);
                        coverage.DisplayRate = FormatMoney(data.result[i].Rate, 2);
                        coverage.FlatRatePorcentage = FormatMoney(data.result[i].FlatRatePorcentage);
                        coverage.ruleSetId = FormatMoney(data.result[i].ruleSetId);
                        coverage.PreRuleSetId = FormatMoney(data.result[i].PreRuleSetId);
                        if (data.result[i].AllyCoverageId != null) {
                            coverage.allowEdit = false;
                            coverage.allowDelete = false;
                        } else {
                            coverage.allowEdit = true;
                            coverage.allowDelete = true;
                        }

                        $("#listCoveragesEdit").UifListView("editItem", index, coverage);
                    }
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });
        /*Vuelve a asignar los formatos*/
        coverageData.DeclaredAmount = FormatMoney(coverageData.DeclaredAmount);
        coverageData.SublimitPercentage = FormatMoney(coverageData.SublimitPercentage);

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
            return AppResources.CoverageBasic
        }
    }

    static ValidateCoverageRate() {
        var type = $("#selectRateType").UifSelect("getSelected");
        type = parseInt(type);
        var rate = parseFloat($("#inputRate").val().replace(',', '.'));

        switch (type) {
            case RateType.FixedValue:
                return true;
            case RateType.Percentage:
            case RateType.Permilage:
                if (rate > 100 || rate < 0) {
                    $("#inputRate").val(0);
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValueRate, 'autoclose': true });
                    return false
                } else {
                    return true;
                }
        }
        return false;
    }

    static GetRateTypes(selectedId) {
        CoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectRateType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectRateType").UifSelect({ sourceData: data.result });
                    $("#selectRateType").UifSelect('setSelected', selectedId);
                }

            }
        });
    }

}
