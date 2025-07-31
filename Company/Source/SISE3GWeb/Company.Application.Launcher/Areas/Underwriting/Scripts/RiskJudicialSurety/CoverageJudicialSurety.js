//VARIABLES
var coverageData = { Id: 0 };
var coverageIndex = null;
var coverageText = null;
var coverageClauses = null;
var validCoverage = true;
var IsEdit = false;

/**
 * Clase Principal de ramo Judicial
 */
class CoverageJudicialSurety extends Uif2.Page {

    /**
     * Funcion que inicializa 
     */
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        CoverageJudicialSurety.setDefault();
        CoverageJudicialSurety.ShowPanelsCoverage(MenuType.Coverage);
        CoverageJudicialSurety.LoadControlsCoverageJudicialSurety();
        CoverageJudicialSurety.LoadCoverages(glbRisk.Coverages);

        if (glbCoverage.CoverageId > 0) {
            CoverageJudicialSurety.LoadCoverage();
        }
        else {
            CoverageJudicialSurety.GetRateTypes(0);
            CoverageJudicialSurety.GetCalculeTypes(0);
            CoverageJudicialSurety.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        }
        CoverageJudicialSurety.LoadTitle();


    }
    static setDefault() {
        $('#inputFromCoverage').UifDatepicker('disabled', false);
        $('#inputToCoverage').UifDatepicker('disabled', false);
        $('#inputDaysCoverage').UifDatepicker('disabled', true);
        $('#inputFromCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
        $('#inputToCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentTo));
        $('#inputDaysCoverage').val(CalculateDays($('#inputFromCoverage').val(), $('#inputToCoverage').val()));
    }

    /**
     * Eventos de los controles de la clase
     */
    bindEvents() {
        $('#btnAcceptCoverage').on('click', CoverageJudicialSurety.AddCoverage);
        $('#btnCancelCoverage').on('click', CoverageJudicialSurety.NewCoverage);
        $('#btnSaveCoverage').on('click', CoverageJudicialSurety.SaveCoverages);
        $('#btnCloseCoverage').on('click', CoverageJudicialSurety.CloseCoverages);
        //$('#listCoveragesEdit').on('rowAdd', CoverageJudicialSurety.RowAddCoverages);
        $('#listCoveragesEdit').on('rowEdit', function (event, data, index) {
            coverageData = data;
            coverageIndex = index;
            CoverageJudicialSurety.EditCoverage(index);
        });
        $('#listCoveragesEdit').on('rowDelete', function (event, data) {
            CoverageJudicialSurety.DeleteCoverage(data);
        });
        $('#inputDeclaredAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputDeclaredAmount').focusout(function (event) {
            event.stopPropagation();
            $(this).val(FormatMoney($(this).val()));
            CoverageJudicialSurety.UpdateAmounts();
        });
        $('#inputLimitAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputLimitAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            $(this).val(FormatMoney($(this).val()));
        });
        $('#inputSubLimitAmount').focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $('#inputSubLimitAmount').focusout(function (event) {
            event.stopPropagation();
            $('#inputPremiumCoverage').val(0);
            $('#inputSubLimitAmount').val(FormatMoney($(this).val()));
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

        $('#selectRateType').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                switch (selectedItem.Id) {
                    case RateType.Percentage:
                        $('#inputRateCoverage').attr('maxLength', 8)
                        break;
                    case RateType.Permilage:
                        $('#inputRateCoverage').attr('maxLength', 9)
                        break;

                    default:
                        //FixedValue
                        $('#inputRateCoverage').attr('maxLength', 20);
                        break;
                }
            }
            else {
                $('#inputPremiumCoverage').val(0);
            }
        });
        $("#inputAmountCoverage").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $("#inputAmountCoverage").focusout(function (event) {
            event.stopPropagation();
            $(this).val(FormatMoney($(this).val()));
            if ($.trim($("#inputAmountCoverage").val()) != "") {
                $("#inputSubLimitAmount").val($(this).val());
            }
        });

        $("#inputRateCoverage").focusin(function (event) {
            $(this).val(NotFormatMoney($(this).val()));
        });
        $("#inputRateCoverage").focusout(function (event) {
            event.stopPropagation();
            $(this).val(FormatMoney($(this).val()));
        });
        $('#selectCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                CoverageJudicialSurety.GetCoverageByCoverageId(selectedItem.Id);
            }
            else {
                $('#selectDeductible').UifSelect();
            }
        });
        $('#selectDeductible').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0 && coverageData.EndorsementType == EndorsementType.Modification) {
                coverageData.CoverStatus = CoverStatus.Modified;
            }
        });

        $("#inputFromCoverage").on("datepicker.change", (event, date) => { CoverageJudicialSurety.DateFromCoverage(event, date) });
        $("#inputToCoverage").on("datepicker.change", (event, date) => { CoverageJudicialSurety.DateToCoverage(event, date) });

        $("#btnCalculateCoverage").on("click", CoverageJudicialSurety.GetPremium);
    }

    static GetPremium() {

        if (CoverageJudicialSurety.ValidCoverageTariff()) {
            CoverageJudicialSurety.QuotationCoverage(false, false, false);
        }
        else {
            $("#inputPremiumCoverage").val(0);
            return false;
        }
    }

    static ValidCoverageTariff() {

        if ($("#selectCoverage").UifSelect("getSelected") == null || $("#selectCoverage").UifSelect("getSelected") == 0) {
            return false;
        }
        if ($("#selectCalculeType").UifSelect("getSelected") == null || $("#selectCalculeType").UifSelect("getSelected") == 0) {
            return false;
        }

        if ($.trim($("#inputSubLimitAmount").val()) == '' || $("#inputSubLimitAmount").val() == 0) {
            return false;
        }

        if ($.trim($("#inputRateCoverage").val()) == '' || $("#inputRateCoverage").val() == 0) {
            return false;
        }
        if ($("#selectRateType").UifSelect("getSelected") == null || $("#selectRateType").UifSelect("getSelected") == 0) {
            return false;
        }
        return true;
    }

    static QuotationCoverage(reload, add, runRules) {
        var coverageModel = CoverageJudicialSurety.CreateCoverageModel();
        coverageModel.PremiumAmount = 0;
        var riskData = {};
        riskData.IdInsuredActsAs = glbRisk.InsuredActAs;
        riskData.IdHolderActAs = glbRisk.HolderActAs;
        RiskJudicialRequest.QuotationCoverage(glbPolicy.Id, glbRisk.Id, coverageData, coverageModel, riskData, runRules).done(function (data) {

            if (data.success) {
                if (glbPolicy.Endorsement.EndorsementType == 2 && glbPolicyEndorsement.Endorsement.ModificationTypeId == 4) {
                    glbRisk.Coverages.forEach(function (item, index) {
                        if (data.result.length != undefined) {
                            data.result.forEach(function (item1, index) {
                                if (item.Id == item1.Id && parseInt($("#selectCoverage").val()) == item.Id && $("#selectRateType").val() == 1) {
                                    var premium1 = parseInt(NotFormatMoney($("#inputSubLimitAmount").val()) * item.OriginalRate / 100 * CalculateDays(item.CurrentFromOriginal, item.CurrentToOriginal)) / 365;
                                    item1.PremiumAmount = (premium1 - item1.PremiumAmount) * -1;
                                }
                                if (item.Id == item1.Id && parseInt($("#selectCoverage").val()) == item.Id && $("#selectRateType").val() == 3) {
                                    item.PremiumAmount = item1.OriginalRate - parseInt(NotFormatMoney($("#inputRateCoverage").val())) * -1;
                                }
                            });
                        } else {
                            if (item.Id == data.result.Id && parseInt($("#selectCoverage").val()) == item.Id && $("#selectRateType").val() == 1) {
                                var premium1 = parseInt(NotFormatMoney($("#inputSubLimitAmount").val()) * item.OriginalRate / 100 * CalculateDays(FormatDate(glbPolicy.Endorsement.CurrentFrom), FormatDate(glbPolicy.Endorsement.CurrentTo))) / 365;
                                data.result.PremiumAmount = (premium1 - data.result.PremiumAmount) * -1;
                            } else if (item.Id == data.result.Id && parseInt($("#selectCoverage").val()) == item.Id && $("#selectRateType").val() == 3) {
                                data.result.PremiumAmount = (data.result.OriginalRate - parseInt(NotFormatMoney($("#inputRateCoverage").val()))) * -1;
                            }
                        }
                    });
                }

                if (reload) {
                    IsEdit = reload
                }

                coverageData = data.result;

                if (coverageData != null) {
                    $("#selectCalculeType").UifSelect("setSelected", coverageData.CalculationType);
                    $("#selectRateType").UifSelect("setSelected", coverageData.RateType);
                    $("#inputDeclaredAmount").val(FormatMoney(coverageData.DeclaredAmount));
                    $("#inputLimitAmount").val(FormatMoney(coverageData.LimitAmount));
                    $("#inputSubLimitAmount").val(FormatMoney(coverageData.SubLimitAmount));
                    $("#inputLimitOccurrenceAmount").val(FormatMoney(coverageData.LimitOccurrenceAmount));
                    $("#inputLimitClaimantAmount").val(FormatMoney(coverageData.LimitClaimantAmount));
                    $("#inputMaxLiabilityAmount").val(FormatMoney(coverageData.MaxLiabilityAmount));
                    $("#inputRateCoverage").val(FormatMoney(coverageData.Rate));
                    $("#inputPremiumCoverage").val(FormatMoney(coverageData.PremiumAmount));

                    if (coverageData.Deductible != null && coverageData.Deductible.Id > 0) {
                        $("#selectDeductible").UifSelect("setSelected", coverageData.Deductible.Id);
                    }

                    CoverageJudicialSurety.ValidateCoverageRate();
                }
                else {
                    $("#inputPremiumCoverage").val(0);
                }

                if (add) {
                    $("#mainCoverage").validate();
                    var coverageEdit = null;
                    if ($("#mainCoverage").valid()) {
                        if (CoverageJudicialSurety.ValidateCoverageRate()) {
                            var selectedCoverage = $("#selectCoverage").UifSelect('getSelected');
                            if (selectedCoverage != "" && selectedCoverage != null) {
                                if (coverageData != null) {
                                    coverageData.Description = $("#selectCoverage").UifSelect('getSelectedText');
                                    coverageData.DeclaredAmount = FormatMoney(coverageData.DeclaredAmount);
                                    coverageData.LimitAmount = FormatMoney(coverageData.LimitAmount);
                                    coverageData.SubLimitAmount = FormatMoney(coverageData.SubLimitAmount);
                                    coverageData.LimitOccurrenceAmount = FormatMoney(coverageData.LimitOccurrenceAmount);
                                    coverageData.LimitClaimantAmount = FormatMoney(coverageData.LimitClaimantAmount);
                                    coverageData.MaxLiabilityAmount = FormatMoney(coverageData.MaxLiabilityAmount);
                                    coverageData.DisplayRate = FormatMoney(coverageData.Rate);
                                    coverageData.FlatRatePorcentage = FormatMoney(coverageData.FlatRatePorcentage);
                                    coverageData.Text = coverageText;
                                    if (clausesCoverage != null && clausesCoverage.length > 0) {
                                        coverageData.Clauses = clausesCoverage;
                                        clausesCoverage = null;
                                    }
                                    coverageData.IsPrimary = coverageData.IsPrimary;
                                    if (coverageData.IsPrimary == true) {
                                        coverageData.Rate = FormatMoney(coverageData.Rate);
                                        coverageData.PremiumAmount = FormatMoney(coverageData.PremiumAmount);
                                    } else {
                                        coverageData.Rate = 0;
                                        coverageData.PremiumAmount = 0;
                                    }
                                    coverageData.MainCoverageId = coverageData.MainCoverageId;
                                    coverageData.TypeCoverage = CoverageJudicialSurety.ValidateTypeCoverage(coverageData);

                                    var claimAmount = parseFloat(NotFormatMoney($("#inputLimitClaimantAmount").val()).replace(separatorDecimal, separatorThousands));
                                    var subLimit = parseFloat(NotFormatMoney($("#inputSubLimitAmount").val()).replace(separatorDecimal, separatorThousands));
                                    if (claimAmount > subLimit) {

                                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorLimitClaim, 'autoclose': true });
                                        return false;
                                    }
                                    if ($("#selectDeductible").UifSelect("getSelected") > 0) {
                                        coverageData.Deductible = $('#selectDeductible').UifSelect("getSelectedSource");
                                        if (coverageData.Deductible != null) {
                                            coverageData.Deductible.Description = $('#selectDeductible').UifSelect("getSelectedText");
                                        }
                                    }

                                    if (coverageData.MainCoverageId > 0) {
                                        coverageData.allowEdit = false;
                                        coverageData.allowDelete = false;
                                    } else {
                                        coverageData.allowEdit = true;
                                        coverageData.allowDelete = true;
                                    }

                                    if (coverageIndex == null) {
                                        var coverages = $("#listCoveragesEdit").UifListView('getData');

                                        let coveragePrimary = coverages.find(item => item.IsPrimary);
                                        if (coveragePrimary) {
                                            AmountMinimum = parseInt(NotFormatMoney(coveragePrimary.LimitAmount));

                                            if (AmountMinimum >= parseFloat(NotFormatMoney($("#inputDeclaredAmount").val()))) {
                                                $("#listCoveragesEdit").UifListView("addItem", coverageData);
                                            } else {
                                                CoverageJudicialSurety.ClearCoverage();
                                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSummaryCoverage, 'autoclose': true });
                                            }
                                        } else {
                                            AmountMinimum = parseInt(NotFormatMoney($("#inputLimitAmount").val()));

                                            if (coverageData.IsPrimary) {
                                                if (AmountMinimum >= parseFloat(NotFormatMoney($("#inputDeclaredAmount").val()))) {
                                                    $("#listCoveragesEdit").UifListView("addItem", coverageData);
                                                } else {
                                                    CoverageJudicialSurety.ClearCoverage();
                                                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSummaryCoverage, 'autoclose': true });
                                                }
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorPrincipalCoverageNotFound, 'autoclose': true });
                                            }
                                        }
                                    }
                                    else {
                                        $("#listCoveragesEdit").UifListView("editItem", coverageIndex, coverageData);
                                    }
                                    CoverageJudicialSurety.LoadSubTitles(0);
                                    CoverageJudicialSurety.UpdatePremium();
                                    CoverageJudicialSurety.ClearCoverage();
                                }
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
                            }
                        }
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });

    }
    static CreateCoverageModel() {
        var coverageModel = $("#mainCoverage").serializeObject();
        coverageModel.CoverageId = $('#selectCoverage').UifSelect('getSelected');
        coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.LimitAmount = NotFormatMoney($('#inputLimitAmount').val());
        coverageModel.DeclaredAmount = NotFormatMoney($('#inputDeclaredAmount').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.Rate = NotFormatMoney($('#inputRateCoverage').val());
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumCoverage').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;
        coverageModel.ContractAmountPercentage = NotFormatMoney($('#inputContractAmountPercentage').val());
        coverageModel.MaxLiabilityAmount = NotFormatMoney(coverageModel.MaxLiabilityAmount);
        coverageModel.LimitClaimantAmount = NotFormatMoney(coverageModel.LimitClaimantAmount);
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
            coverageModel.EndorsementType = EndorsementType.Modification;

        if ($("#selectDeductible").UifSelect("getSelected") > 0) {
            coverageModel.DeductibleId = $("#selectDeductible").UifSelect('getSelected');
            coverageModel.DeductibleDescription = $("#selectDeductible").UifSelect('getSelectedText');
        }
        return coverageModel;
    }

    static LoadCoverage() {
        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            if (glbCoverage.CoverageId == this.Id) {
                coverageIndex = key;
                coverageData = this;
            }
        });

        CoverageJudicialSurety.EditCoverage(coverageIndex);
    }

    static EditCoverage(index) {

        if (coverageData.IsVisible) {
            coverageText = coverageData.Text;
            coverageClauses = coverageData.Clauses;
            coverageData.OriginalSubLimitAmount = coverageData.SubLimitAmount;
            if (coverageData.LimitAmount == null) {
                coverageData.LimitAmount = 0;
                coverageData.DeclaredAmount = 0;
            }

            if (coverageData.PremiumAmount == null) {
                coverageData.PremiumAmount = 0;
            }

            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                coverageData.EndorsementType = EndorsementType.Modification;
            }

            if (coverageData.Rate == null) {
                coverageData.Rate = 0;
            }

            CoverageJudicialSurety.ValidateRateType();
            if (index != null) {
                CoverageJudicialSurety.SetCoverageEdit();
                $('#inputDeclaredAmount').val(FormatMoney(coverageData.DeclaredAmount));
                $("#inputLimitAmount").val(coverageData.LimitAmount);
                $("#inputSubLimitAmount").val(coverageData.SubLimitAmount);
                $('#inputLimitClaimantAmount').val(FormatMoney(coverageData.LimitClaimantAmount));
                $('#inputMaxLiabilityAmount').val(FormatMoney(coverageData.MaxLiabilityAmount));
                $("#inputRateCoverage").val(coverageData.Rate);
                $("#inputPremiumCoverage").val(coverageData.PremiumAmount);
            }
            else {

                $('#inputDeclaredAmount').val(FormatMoney(coverageData.DeclaredAmount));
                $('#inputLimitAmount').val(FormatMoney(coverageData.LimitAmount));
                $("#inputSubLimitAmount").val(coverageData.SubLimitAmount);
                $('#inputLimitClaimantAmount').val(FormatMoney(coverageData.LimitClaimantAmount));
                $('#inputMaxLiabilityAmount').val(FormatMoney(coverageData.MaxLiabilityAmount));
                $("#inputRateCoverage").val(FormatMoney(coverageData.Rate));
                $("#inputPremiumCoverage").val(FormatMoney(coverageData.PremiumAmount));
            }
            CoverageJudicialSurety.GetCalculeTypes(coverageData.CalculationType);
            CoverageJudicialSurety.GetRateTypes(coverageData.RateType);
            //$("#selectCalculeType").UifSelect("setSelected", coverageData.CalculationType);
            //$("#selectRateType").UifSelect("setSelected", coverageData.RateType);
            if (coverageData.Deductible != null) {
                CoverageJudicialSurety.GetDeductiblesByCoverageId(coverageData.Id, coverageData.Deductible.Id);
            }
            else {
                CoverageJudicialSurety.GetDeductiblesByCoverageId(coverageData.Id, 0);
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
        }
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
                            var coverage = CoverageJudicialSurety.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                            if (coverage != null) {
                                coverage.Rate = FormatMoney(coverage.Rate);
                                coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                                $("#listCoveragesEdit").UifListView("addItem", coverage);
                            }
                        }
                    }
                });
                CoverageJudicialSurety.ClearCoverage();
                CoverageJudicialSurety.UpdatePremium();
            }
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        RiskJudicialRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
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

    static SetCoverageEdit() {
        var coveragesData = [];
        coveragesData.push(coverageData);

        $("#selectCoverage").UifSelect();
        $("#selectCoverage").UifSelect({ sourceData: coveragesData, id: "Id", name: "Description", selectedId: coverageData.Id });
        $("#selectCoverage").prop('disabled', true);
    }

    static ValidateRateType() {
        var RateCoverage = parseInt($("#selectRateType").UifSelect("getSelected"), 10);
        switch (RateCoverage) {
            case RateType.Percentage:
                $("#inputRateCoverage").attr("maxLength", 9);
                break;
            case RateType.Permilage:
                $("#inputRateCoverage").attr("maxLength", 9);
                break;
            default:
                $("#inputRateCoverage").attr("maxLength", 20);
                break;
        }
    }

    static GetDeductiblesByCoverageId(coverageId, selectedId) {

        $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetDeductiblesByCoverageId',
            data: JSON.stringify({ coverageId: coverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectDeductible").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectDeductible").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverageDeductible, 'autoclose': true });
        });


        //var controller = rootPath + "Underwriting/RiskJudicialSurety/GetDeductiblesByCoverageId?coverageId=" + coverageId

    }

    static LoadCoverages(coverages) {
        if (coverages != null) {
            $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });

            $.each(coverages, function (key, value) {
                this.LimitAmount = FormatMoney(this.LimitAmount);
                this.SubLimitAmount = FormatMoney(this.SubLimitAmount);
                this.DisplayRate = FormatMoney(this.Rate, 2);
                this.Rate = FormatMoney(this.Rate);
                this.PremiumAmount = FormatMoney(this.PremiumAmount);
                this.FlatRatePorcentage = FormatMoney(this.FlatRatePorcentage);

                $("#listCoveragesEdit").UifListView("addItem", this);
            });
        }
    }

    static LoadControlsCoverageJudicialSurety() {
        $('#hiddenCoverageId').val(glbCoverage.CoverageId);
        $("#inputFromCoverage").ValidatorKey(ValidatorType.Dates, 1, 1);
        $("#inputToCoverage").ValidatorKey(ValidatorType.Dates, 1, 1);
        $("#inputAmountCoverage").OnlyDecimals(UnderwritingDecimal);
        $("#inputDeclaredAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputLimitAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputSubLimitAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputPremiumCoverage").OnlyDecimals(UnderwritingDecimal);
        $("#inputLimitClaimantAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputMaxLiabilityAmount").OnlyDecimals(UnderwritingDecimal);
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Emission) {
            $("#inputRateCoverage").OnlyDecimals(3);
        } else {
            $("#inputRateCoverage").OnlyDecimals1(3);
        }
        $("#inputContractAmountPercentage").OnlyDecimals(UnderwritingDecimal);
        $("#inputAmountInsured").text(FormatMoney(glbRisk.AmountInsured));
        $("#inputPremium").text(FormatMoney(glbRisk.Premium));
        if ($("#listCoveragesEdit").UifListView('getData').length <= 0) {
            $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 440 });

        }
        var policyFrom = glbPolicy.CurrentFrom;
        var policyTo = glbPolicy.CurrentTo;

        if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Emission) {
            policyFrom = glbPolicy.Endorsement.CurrentFrom;
        }
        $("#hiddenPolicyFrom").val(FormatDate(policyFrom));
        $("#inputFromCoverage").val(FormatDate(glbPolicy.CurrentFrom));
        $("#hiddenPolicyTo").val(FormatDate(glbPolicy.CurrentTo));

        if ($('#hiddenCoverageId').val > 0) {
            $.each(glbRisk.Coverages, function (key, value) {
                if ($('#hiddenCoverageId').val() == this.Id) {
                    $("#inputToCoverage").val(FormatDate(this.CurrentTo));
                }
            });
        }
        else {
            $("#inputToCoverage").val(FormatDate(glbPolicy.CurrentTo));
        }
        $('#inputDaysCoverage').val(CalculateDays($('#inputFromCoverage').val(), $('#inputToCoverage').val()));

    }

    static GetCoverageByCoverageId(coverageId) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCoverageByCoverageId',
            data: JSON.stringify({ coverageId: coverageId, groupCoverageId: glbRisk.GroupCoverage.Id, policyId: glbPolicy.Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                coverageData = data.result;
                CoverageJudicialSurety.EditCoverage(null);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverage, 'autoclose': true });
        });
    }

    static AddCoverage() {
        $("#mainCoverage").validate();
        if ($("#mainCoverage").valid() && $("#selectCoverage").UifSelect("getSelected") != null) {
            CoverageJudicialSurety.QuotationCoverage(true, true, true);

        }
        else if ($("#selectCoverage").UifSelect("getSelected") == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.RequiredAllFields, 'autoclose': true });
        }
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
    }

    static UpdatePremium() {
        var insuredAmount = 0;
        var premium = 0;

        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            insuredAmount += parseFloat(NotFormatMoney(this.LimitAmount).replace(separatorDecimal, separatorThousands));
            premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
        });

        $("#inputAmountInsured").text(FormatMoney(insuredAmount));
        $("#inputPremium").text(FormatMoney(premium));
    }

    static NewCoverage() {
        CoverageJudicialSurety.ClearCoverage();
        coverageIndex = null;
        CoverageJudicialSurety.GetCoveragesByProductIdGroupCoverageIdPrefixId();
    }

    static ClearCoverage() {
        $('#selectCoverage').UifSelect();
        $("#selectCalculeType").UifSelect("setSelected", null);
        $('#inputAmountCoverage').val(0);
        $('#inputSubLimitAmount').val(0);
        $('#inputRateCoverage').val(0);
        $("#selectRateType").UifSelect("setSelected", null);
        $('#inputPremiumCoverage').val(0);
        $('#selectDeductible').UifSelect();
        $("#hiddenFlatRatePorcentage").val(0);
        $("#hiddenIsVisible").val(true);
        $("#hiddenIsSelected").val(false);
        $("#hiddenIsMandatory").val(false);
        $('#selectedTexts').text("");
        $('#selectedClauses').text("");
        $('#inputDeclaredAmount').val(0);
        $('#inputLimitAmount').val(0);
        $('#inputLimitClaimantAmount').val(0);
        $('#inputMaxLiabilityAmount').val(0);
        coverageIndex = 0;
        coverageText = null;
        coverageClauses = null;
    }

    static GetCoveragesByProductIdGroupCoverageIdPrefixId() {
        var coveragesAdd = '';

        $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
            if (coveragesAdd.length > 0) {
                coveragesAdd += ',';
            }
            coveragesAdd += this.Id;
        });
        RiskJudicialRequest.GetCoveragesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $("#selectCoverage").UifSelect({ sourceData: data.result, id: "Id", name: "Description" });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    static SaveCoverages() {

        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');

        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
            this.LimitClaimantAmount = NotFormatMoney(this.LimitClaimantAmount);
            this.MaxLiabilityAmount = NotFormatMoney(this.MaxLiabilityAmount);
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
        });

        RiskJudicialRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, $("#listCoveragesEdit").UifListView('getData')).done(function (data) {
            if (data.success) {
                if (glbPolicy.Endorsement.EndorsementType == 2) {
                    glbRisk.Coverages = coveragesValues;
                }
                router.run("prtRiskJudicialSurety");
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
        });
    }

    static CloseCoverages() {
        router.run("prtRiskJudicialSurety");
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
        }
    }

    static RowAddCoverages() {
        CoverageJudicialSurety.ClearCoverage();
        CoverageJudicialSurety.GetCoveragesByProductIdGroupCoverageIdPrefixId();
    }

    static LoadTitle() {
        var title = AppResources.LabelAddModifyCoverage;
        $.uif2.helpers.setGlobalTitle(title);
    }
    static UpdateAmounts() {
        $('#inputLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputSubLimitAmount').val($('#inputDeclaredAmount').val());
        $('#inputLimitClaimantAmount').val($('#inputDeclaredAmount').val());
        $('#inputMaxLiabilityAmount').val($('#inputDeclaredAmount').val());
        $('#inputPremiumCoverage').val(0);
    }

    static GetRateTypes(selectedId) {
        CoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectRateType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectRateType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $("#selectRateType").UifSelect("setSelected", coverageData.RateType);
                }

            }
        });
    }
    static GetCalculeTypes(selectedId) {
        CoverageRequest.GetCalculeTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectCalculeType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectCalculeType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    $("#selectCalculeType").UifSelect("setSelected", coverageData.CalculationType);
                }
            }
        });
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

    static CalculateDays(f1, f2) {

        var cFecha1 = f1.split('/');
        var cFecha2 = f2.split('/');
        var fcFecha1 = Date.UTC(cFecha1[2], cFecha1[1] - 1, cFecha1[0]);
        var fcFecha2 = Date.UTC(cFecha2[2], cFecha2[1] - 1, cFecha2[0]);
        var dif = fcFecha2 - fcFecha1;
        var days = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (isNaN(days)) {
            return 0;
        }
        else {
            return days;
        }
    }

    static ValidateCoverageRate() {
        var type = $("#selectRateType").UifSelect("getSelected");
        type = parseInt(type);
        var rate = parseFloat(NotFormatMoney($("#inputRateCoverage").val()).replace(',', '.'));

        switch (type) {
            case RateType.FixedValue:
                return true;
            case RateType.Percentage:
            case RateType.Permilage:
                if (rate > 100 || rate < 0) {
                    $("#inputRateCoverage").val(0);
                    $("#inputPremiumCoverage").val(0);
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValueRate, 'autoclose': true });
                    return false
                } else {
                    return true;
                }
        }
        return false;
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
}



