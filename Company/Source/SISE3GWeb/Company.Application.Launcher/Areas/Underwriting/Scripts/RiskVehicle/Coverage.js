var coverageData = { Id: 0 };
var coverageIndex = null;
var coverageText = null;
var coverageClauses = null;
var validCoverage = true;
class RiskVehicleCoverage extends Uif2.Page {
    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $('#inputFromCoverage').UifDatepicker('disabled', true);
        $('#inputToCoverage').UifDatepicker('disabled', true);
        $('#inputFromCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentFrom));
        $('#inputToCoverage').UifDatepicker('setValue', FormatDate(glbPolicy.CurrentTo));
        $('#inputDaysCoverage').val(CalculateDays($('#inputFromCoverage').val(), $('#inputToCoverage').val()));
        RiskVehicleCoverage.ShowPanelsCoverage(MenuType.Coverage);
        $("#inputAmountCoverage").OnlyDecimals(UnderwritingDecimal);
        $("#inputSubLimitAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputRateCoverage").OnlyDecimals(3);
        $("#inputPremiumCoverage").OnlyDecimals(UnderwritingDecimal);
        $("#inputAmountInsured").text(FormatMoney(glbRisk.AmountInsured));
        $("#inputPremium").text(FormatMoney(glbRisk.Premium));

        RiskVehicleCoverage.LoadCoverages(glbRisk.Coverages);
        if (glbCoverage.CoverageId > 0) {
            RiskVehicleCoverage.LoadCoverage();
        }
        else {
            RiskVehicleCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
            RiskVehicleCoverage.GetCalculeTypes(0);
            RiskVehicleCoverage.GetRateTypes(0);
        }

        RiskVehicleCoverage.LoadTitle();
    }

    bindEvents() {
        $('#selectCalculeType').on('itemSelected', this.ChangeCalculeType);
        $("#inputAmountCoverage").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputAmountCoverage").focusout(this.AmountCoverageFocusOut);
        $("#inputSubLimitAmount").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputSubLimitAmount").focusout(this.SubLimitAmountFocusOut);
        $("#inputRateCoverage").focusin(Underwriting.NotFormatMoneyIn);
        $("#inputRateCoverage").focusout(this.RateCoverageFocusOut);
        $('#selectRateType').on('itemSelected', this.ChangeCalculeTypeRateType);
        $('#listCoveragesEdit').on('rowAdd', this.CoverageAdd);
        $('#listCoveragesEdit').on('rowEdit', this.CoverageEdit);
        $('#listCoveragesEdit').on('rowDelete', (event, data) => { RiskVehicleCoverage.DeleteCoverage(event, data) });
        $('#selectCoverage').on('itemSelected', this.ChangeCoverage);
        $('#selectDeductible').on('itemSelected', this.ChangeDeductible);
        $("#btnAcceptCoverage").on("click", RiskVehicleCoverage.AddCoverage);
        $("#btnCancelCoverage").on("click", this.CancelCoverage);
        $("#btnCloseCoverage").on("click", this.CloseCoverage);
        $("#btnSaveCoverage").on("click", RiskVehicleCoverage.SaveCoverages);
    }

    ChangeCalculeType(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskVehicleCoverage.GetPremium();
        }
    }

    AmountCoverageFocusOut(event) {
        event.stopPropagation();
        $(this).val(FormatMoney($(this).val()));
        if ($.trim($("#inputAmountCoverage").val()) != "") {
            $("#inputSubLimitAmount").val($(this).val());
            RiskVehicleCoverage.GetPremium();
        }
    }

    SubLimitAmountFocusOut(event) {
        event.stopPropagation();
        $('#inputPremiumCoverage').val(0);
        $(this).val(FormatMoney($(this).val()));
    }

    RateCoverageFocusOut(event) {
        event.stopPropagation();
        $(this).val(FormatMoney($(this).val()));
        RiskVehicleCoverage.GetPremium();
    }

    ChangeCalculeTypeRateType(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var RateCoverage = parseInt(selectedItem.Id)
            switch (RateCoverage) {
                case RateType.Percentage:
                    $("#inputRateCoverage").attr("maxLength", 8)
                    break;
                case RateType.Permilage:
                    $("#inputRateCoverage").attr("maxLength", 9)
                    break;
                default:
                    //FixedValue
                    $("#inputRateCoverage").attr("maxLength", 20);
                    break;
            }
            RiskVehicleCoverage.GetPremium();
        }
        else {
            $('#inputPremiumCoverage').val(0);
        }
    }

    CoverageAdd(event) {
        RiskVehicleCoverage.ClearCoverage();
        RiskVehicleCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
    }

    CoverageEdit(event, data, index) {
        coverageData = data;
        coverageIndex = index;
        RiskVehicleCoverage.EditCoverage(index);
    }

    ChangeCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskVehicleCoverage.GetCoverageByCoverageId(selectedItem.Id);
        }
        else {
            $('#selectDeductible').UifSelect();
        }
    }

    ChangeDeductible(event, selectedItem) {
        if (selectedItem.Id > 0 && coverageData.EndorsementType == EndorsementType.Modification) {
            coverageData.CoverStatus = CoverStatus.Modified;
        }
    }

    CancelCoverage() {
        RiskVehicleCoverage.ClearCoverage();
        RiskVehicleCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
    }

    CloseCoverage() {
        lockScreen();
        router.run("prtRiskVehicle");
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

    static LoadCoverage() {
        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            if (glbCoverage.CoverageId == this.Id) {
                coverageIndex = key;
                coverageData = this;
            }
        });
        RiskVehicleCoverage.GetCalculeTypes(coverageData.CalculationType);
        RiskVehicleCoverage.GetRateTypes(coverageData.RateType);

        RiskVehicleCoverage.EditCoverage(coverageIndex);
    }

    static SetCoverageEdit() {
        var coveragesData = [];
        coveragesData.push(coverageData);

        $("#selectCoverage").UifSelect();
        $("#selectCoverage").UifSelect({ sourceData: coveragesData, id: "Id", name: "Description", selectedId: coverageData.Id });
        $("#selectCoverage").prop('disabled', true);
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

    static GetCoveragesByProductIdGroupCoverageIdPrefixId() {
        var coveragesAdd = '';

        $.each($("#listCoveragesEdit").UifListView("getData"), function (key, value) {
            if (coveragesAdd.length > 0) {
                coveragesAdd += ',';
            }
            coveragesAdd += this.Id;
        });

        RiskVehicleCoverageRequest.GetCoveragesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd).done(function (data) {
            if (data.success) {
                $("#selectCoverage").UifSelect({ sourceData: data.result, id: "Id", name: "Description" });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetCoverageByCoverageId(coverageId) {
        RiskVehicleCoverageRequest.GetCoverageByCoverageId(coverageId, glbRisk.GroupCoverage.Id, glbPolicy.Id).done(function (data) {
            if (data.success) {
                coverageData = data.result;
                RiskVehicleCoverage.EditCoverage(null);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchCoverage, 'autoclose': true });
        });
    }

    static GetDeductiblesByCoverageId(coverageId, selectedId) {
        CoverageRequest.GetDeductiblesByCoverageId(coverageId).done(function (data) {
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
        });
    }

    static AddCoverage() {

        $("#mainCoverage").validate();
        if ($("#mainCoverage").valid()) {
            var coverageModel = RiskVehicleCoverage.GetCoverageModel();
            if (coverageModel != null) {
                RiskVehicleCoverageRequest.QuotationCoverage(glbPolicy.Id, glbRisk.Id, coverageModel, coverageData).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            coverageModel = data.result;
                            coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
                            coverageModel.Number = data.result.Number;
                            coverageModel.LimitAmount = FormatMoney(data.result.LimitAmount);
                            coverageModel.SubLimitAmount = FormatMoney(data.result.SubLimitAmount);
                            coverageModel.DisplayRate = FormatMoney(data.result.Rate);
                            coverageModel.Rate = FormatMoney(data.result.Rate);

                            coverageModel.PremiumAmount = FormatMoney(data.result.PremiumAmount);
                            coverageModel.FlatRatePorcentage = FormatMoney(data.result.FlatRatePorcentage);                           
                            coverageModel.RateType = data.result.RateType;
                            coverageModel.Text = coverageText;
                            coverageModel.Clauses = coverageClauses;
                            if (coverageIndex == null) {
                                $("#listCoveragesEdit").UifListView("addItem", coverageModel);
                            }
                            else {
                                $("#listCoveragesEdit").UifListView("editItem", coverageIndex, coverageModel);
                            }

                            RiskVehicleCoverage.LoadSubTitles(0);
                            RiskVehicleCoverage.UpdatePremium();
                            RiskVehicleCoverage.ClearCoverage();
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
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
            }
        }
    }

    static EditCoverage(index) {

        if (coverageData.IsVisible) {
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
                RiskVehicleCoverage.SetCoverageEdit();
            }

            $("#selectCalculeType").UifSelect("setSelected", coverageData.CalculationType);
            $("#selectRateType").UifSelect("setSelected", coverageData.RateType);

            $('#selectCalculeType').UifSelect('disabled', true);
            $('#selectRateType').UifSelect('disabled', true);

            $("#inputAmountCoverage").val(coverageData.LimitAmount);
            $("#inputSubLimitAmount").val(coverageData.SubLimitAmount);
            $("#inputRateCoverage").val(coverageData.Rate);
            $("#inputPremiumCoverage").val(coverageData.PremiumAmount);

            if (coverageData.Deductible != null) {
                RiskVehicleCoverage.GetDeductiblesByCoverageId(coverageData.Id, coverageData.Deductible.Id);
            }
            else {
                RiskVehicleCoverage.GetDeductiblesByCoverageId(coverageData.Id, 0);
            }

            RiskVehicleCoverage.LoadSubTitles(0);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
        }

        $("#mainCoverage").valid();
    }

    static DeleteCoverage(event, data) {

        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
        }
        else {
            var coverages = $("#listCoveragesEdit").UifListView('getData');
            $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });

            $.each(coverages, function (index, value) {
                if (this.Id != data.Id) {
                    $("#listCoveragesEdit").UifListView("addItem", this);
                }
                else {
                    if (data.EndorsementType == EndorsementType.Modification && data.RiskCoverageId > 0) {
                        var coverage = RiskVehicleCoverage.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                        if (coverage != null) {
                            coverage.Rate = FormatMoney(coverage.Rate);
                            coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                            $("#listCoveragesEdit").UifListView("addItem", coverage);
                        }
                    }
                }
            });
            RiskVehicleCoverage.ClearCoverage();
            RiskVehicleCoverage.UpdatePremium();
            $('#selectCalculeType').UifSelect('disabled', true);
            $('#selectRateType').UifSelect('disabled', true);
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        RiskVehicleCoverageRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
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

        RiskVehicleCoverage.ValidCoverageValue();
        if (validCoverage) {
            var coverage = RiskVehicleCoverage.GetCoverageModel();
            RiskVehicleCoverageRequest.QuotationCoverage(glbPolicy.Id, glbRisk.Id, coverage, coverageData).done(function (data) {
                if (data.success) {
                    var coverageModel = data.result;
                    if (coverageModel != null) {
                        $("#selectCalculeType").UifSelect("setSelected", coverageModel.CalculationType);
                        $("#inputAmountCoverage").val(FormatMoney(coverageModel.LimitAmount));
                        $("#inputSubLimitAmount").val(FormatMoney(coverageModel.SubLimitAmount));
                        $("#inputRateCoverage").val(FormatMoney(coverageModel.Rate));
                        $("#selectRateType").UifSelect("setSelected", coverageModel.RateType);
                        $("#inputPremiumCoverage").val(FormatMoney(coverageModel.PremiumAmount));

                        $('#selectCalculeType').UifSelect('disabled', true);
                        $('#selectRateType').UifSelect('disabled', true);
                        if (coverageModel.Deductible != null) {
                            if (coverageModel.Deductible.Id > 0) {
                                $("#selectDeductible").UifSelect("setSelected", coverageModel.Deductible.Id);
                            }
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
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorQuotationCoverage, 'autoclose': true });
            });
        }
    }

    static GetCoverageModel() {
        var coverageModel = $("#mainCoverage").serializeObject();
        coverageModel.CoverageId = $("#selectCoverage").UifSelect("getSelected");
        coverageModel.Id = $("#selectCoverage").UifSelect("getSelected");
        coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.LimitAmount = NotFormatMoney($('#inputAmountCoverage').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.Rate = NotFormatMoney($('#inputRateCoverage').val());
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumCoverage').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;
        coverageModel.CalculationType = $("#selectCalculeType").val();
        coverageModel.IsVisible = true;
        coverageModel.SubLineBusiness = glbRisk.Coverages[0].SubLineBusiness;
        coverageModel.CoverStatus = glbRisk.Coverages[0].CoverStatus;
        $('#selectCalculeType').UifSelect('disabled', true);
        $('#selectRateType').UifSelect('disabled', true);

        if ($("#selectDeductible").UifSelect("getSelected") > 0) {
            coverageModel.Deductible = {
                Id: $("#selectDeductible").UifSelect("getSelected"),
                Description: $("#selectDeductible").UifSelect('getSelectedText')
            }
        }
        return coverageModel;
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

    static SaveCoverages() {        
        lockScreen();
        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
        });
        RiskVehicleCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, $("#listCoveragesEdit").UifListView('getData')).done(function (data) {
            if (data.success) {
                router.run("prtRiskVehicle");
            }
            else {             
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {            
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSaveCoverages, 'autoclose': true });
        });
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
        $('#selectCalculeType').UifSelect('disabled', true);
        $('#selectRateType').UifSelect('disabled', true);

        coverageIndex = null;
        coverageText = null;
        coverageClauses = null;
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

        if ($.trim($("#inputRateCoverage").val()) == '' || $("#inputRateCoverage").val() == 0) {
            validCoverage = false;
        }
        if ($("#selectRateType").UifSelect("getSelected") == null || $("#selectRateType").UifSelect("getSelected") == 0) {
            validCoverage = false;
        }
    }

    static LoadTitle() {
        var title = AppResources.LabelTitleCoverage + " - " + AppResources.LabeltitleRisk + ": " + glbRisk.LicensePlate + " " + glbRisk.Make.Description + " " + glbRisk.Version.Description;
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
    static GetCalculeTypes(selectedId) {
        CoverageRequest.GetCalculeTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectCalculeType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectCalculeType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
                $('#selectCalculeType').UifSelect('disabled', true);
            }
        });
    }
    static GetRateTypes(selectedId) {
        CoverageRequest.GetRateTypes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectRateType").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectRateType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }

                $('#selectRateType').UifSelect('disabled', true);
            }
        });
    }
}