
var filterCoverages = [];

class RiskFidelityCoverage extends Uif2.Page {
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

        RiskFidelityCoverage.ShowPanelsCoverage(MenuType.Coverage);

        $("#inputDeclaredAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputLimitAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputSubLimitAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputLimitOccurrenceAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputLimitClaimantAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputMaxFidelityAmount").OnlyDecimals(UnderwritingDecimal);
        $("#inputRate").OnlyDecimals(3);
        $("#inputPremiumAmount").OnlyDecimals(UnderwritingDecimal);

        $("#inputAmountInsured").text(FormatMoney(glbRisk.AmountInsured));
        $("#inputTotalPremium").text(FormatMoney(glbRisk.Premium));

        RiskFidelityCoverage.GetCoveragesByRiskId(glbRisk.Id);
        RiskFidelityCoverage.GetDeductiblesByCoverageId($("#hiddenCoverageId").val(), $("#hiddenDeductibleId").val());

        RiskFidelityCoverage.LoadTitle();
    }

    //Eventos
    bindEvents() {
        $("#inputDeclaredAmount").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $("#inputDeclaredAmount").focusout(function (event) {
            $(this).val(FormatMoney($(this).val()));
            if ($.trim($("#inputDeclaredAmount").val()) != "") {
                $("#inputLimitAmount").val($(this).val());
                $("#inputSubLimitAmount").val($(this).val());
                $("#inputLimitOccurrenceAmount").val($(this).val());
                $("#inputLimitClaimantAmount").val($(this).val());
                $("#inputMaxFidelityAmount").val($(this).val());
                RiskFidelityCoverage.GetPremium();
            }
        });

        $("#inputLimitAmount").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $("#inputLimitAmount").focusout(function (event) {
            $(this).val(FormatMoney($(this).val()));
            RiskFidelityCoverage.GetPremium();
        });

        $("#inputSubLimitAmount").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $("#inputSubLimitAmount").focusout(function (event) {
            $(this).val(FormatMoney($(this).val()));
            RiskFidelityCoverage.GetPremium();
        });

        $("#inputLimitOccurrenceAmount").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $("#inputLimitOccurrenceAmount").focusout(function (event) {
            $(this).val(FormatMoney($(this).val()));
            RiskFidelityCoverage.GetPremium();
        });

        $("#inputLimitClaimantAmount").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $("#inputLimitClaimantAmount").focusout(function (event) {
            $(this).val(FormatMoney($(this).val()));
            RiskFidelityCoverage.GetPremium();
        });

        $("#inputMaxFidelityAmount").focusin(function () {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $("#inputMaxFidelityAmount").focusout(function (event) {
            $(this).val(FormatMoney($(this).val()));
            RiskFidelityCoverage.GetPremium();
        });

        $('#selectCalculeType').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                RiskFidelityCoverage.GetPremium();
            }
        });

        $('#selectRateType').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                var RateCoverage = parseInt(selectedItem.Id)
                switch (RateCoverage) {
                    case RateType.Percentage:
                        $("#inputRate").prop("maxLength", 8)
                        break;
                    case RateType.Permilage:
                        $("#inputRate").prop("maxLength", 9)
                        break;
                    default:
                        $("#inputRate").prop("maxLength", 20);
                        break;
                }
                RiskFidelityCoverage.GetPremium();
            }
            else {
                $('#inputPremiumAmount').val(0);
            }
        });

        $("#inputRate").focusout(function (event) {
            $(this).val(FormatMoney($(this).val()));
            RiskFidelityCoverage.GetPremium();
        });

        $("#inputRate").focusin(function (event) {
            $(this).val(NotFormatMoney($(this).val()));
        });

        $('#listCoveragesEdit').on('rowAdd', function (event) {
            RiskFidelityCoverage.ClearCoverage();
            RiskFidelityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        });

        $('#listCoveragesEdit').on('rowEdit', function (event, data, index) {
            coverageData = data;
            coverageIndex = index;
            RiskFidelityCoverage.EditCoverage(index);
        });

        $('#listCoveragesEdit').on('rowDelete', function (event, data) {
            RiskFidelityCoverage.DeleteCoverage(data);
        });

        $('#selectCoverage').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                for (var i = 0; i < filterCoverages.length; i++) {
                    if (filterCoverages[i].Id == selectedItem.Id) {
                        coverageData = filterCoverages[i];
                        break;
                    }
                }

                RiskFidelityCoverage.EditCoverage(null);
            }
            else {
                $('#selectDeductible').UifSelect();
            }
        });

        $("#btnAcceptCoverage").on("click", function () {
            RiskFidelityCoverage.AddCoverage();
        });

        $("#btnCancelCoverage").on("click", function () {
            RiskFidelityCoverage.ClearCoverage();
            RiskFidelityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        });


        $("#btnCloseCoverage").on("click", function () {
            router.run("prtRiskFidelity");
        });

        $("#btnSaveCoverage").on("click", function () {
            RiskFidelityCoverage.SaveCoverages();
        });



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
        }
    }

    static ShowPanelsCoverage(Menu) {
        switch (Menu) {
            case MenuType.Coverage:
                break;
            case MenuType.Texts:
                $("#modalTexts").UifModal('showLocal',AppResources.LabelTexts);
                break;
            case MenuType.Clauses:
                $("#modalClauses").UifModal('showLocal',AppResources.LabelClauses);
                break;
        }
    }

    static LoadCoverage(coverages) {

        $("#listCoveragesEdit").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", height: 420 });

        $.each(coverages, function (key, value) {
            this.DeclaredAmount = FormatMoney(this.DeclaredAmount);
            this.LimitAmount = FormatMoney(this.LimitAmount);
            this.SubLimitAmount = FormatMoney(this.SubLimitAmount);
            this.LimitOccurrenceAmount = FormatMoney(this.LimitOccurrenceAmount);
            this.LimitClaimantAmount = FormatMoney(this.LimitClaimantAmount);
            this.MaxFidelityAmount = FormatMoney(this.MaxFidelityAmount);
            this.DisplayRate = FormatMoney(this.Rate, 2);
            this.Rate = FormatMoney(this.Rate);
            this.PremiumAmount = FormatMoney(this.PremiumAmount);
            this.FlatRatePorcentage = FormatMoney(this.FlatRatePorcentage);

            $("#listCoveragesEdit").UifListView("addItem", this);
        });

        RiskFidelityCoverage.UpdatePremium();

        if ($("#hiddenCoverageId").val() > 0) {
            $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
                if ($('#hiddenCoverageId').val() == this.Id) {
                    coverageIndex = key;
                    coverageData = this;
                }
            });
            RiskFidelityCoverage.EditCoverage(coverageIndex);
        }
        else {
            RiskFidelityCoverage.GetCoveragesByProductIdGroupCoverageIdPrefixId();
        }


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

        RiskFidelityCoverageRequest.GetCoveragesByRiskId(riskId).done(function (data) {
            if (data.success) {
                coverages = data.result;
                RiskFidelityCoverage.LoadCoverage(coverages);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.ErrorSearchCoverages, 'autoclose': true });
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

        RiskFidelityCoverageRequest.GetCoveragesByProductIdGroupCoverageIdPrefixId(glbPolicy.Product.Id, glbRisk.GroupCoverage.Id, glbPolicy.Prefix.Id, coveragesAdd).done(function (data) {
            if (data.success) {
                filterCoverages = data.result;
                $("#selectCoverage").UifSelect({ sourceData: data.result, id: "Id", name: "Description" });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }); 
    }

    static GetCoverageByCoverageId(coverageId) {
        RiskFidelityCoverageRequest.GetCoverageByCoverageId(coverageId).done(function (data) {
            if (data.success) {
                coverageData = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.ErrorSearchCoverage, 'autoclose': true });
        });

        RiskFidelityCoverage.EditCoverage(null);
    }

    static GetDeductiblesByCoverageId(coverageId, selectedId) {
        var controller = rootPath + "Underwriting/RiskFidelity/GetDeductiblesByCoverageId?coverageId=" + coverageId;

        if (selectedId == 0) {
            $("#selectDeductible").UifSelect({ source: controller });
        }
        else {
            $("#selectDeductible").UifSelect({ source: controller, selectedId: selectedId });
        }
    }

    static AddCoverage() {

        $("#mainCoverage").validate();

        if ($("#mainCoverage").valid()) {
            var coverageModel = RiskFidelityCoverage.QuotationCoverage();

            if (coverageModel != null) {
                coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
                coverageModel.DeclaredAmount = FormatMoney(coverageModel.DeclaredAmount);
                coverageModel.LimitAmount = FormatMoney(coverageModel.LimitAmount);
                coverageModel.SubLimitAmount = FormatMoney(coverageModel.SubLimitAmount);
                coverageModel.LimitOccurrenceAmount = FormatMoney(coverageModel.LimitOccurrenceAmount);
                coverageModel.LimitClaimantAmount = FormatMoney(coverageModel.LimitClaimantAmount);
                coverageModel.MaxFidelityAmount = FormatMoney(coverageModel.MaxFidelityAmount);
                coverageModel.DisplayRate = FormatMoney(coverageModel.Rate);
                coverageModel.Rate = FormatMoney(coverageModel.Rate);
                coverageModel.PremiumAmount = FormatMoney(coverageModel.PremiumAmount);
                coverageModel.FlatRatePorcentage = FormatMoney(coverageModel.FlatRatePorcentage);
                coverageModel.Text = coverageText;
                coverageModel.Clauses = coverageClauses;

                if (coverageIndex == null) {
                    $("#listCoveragesEdit").UifListView("addItem", coverageModel);
                }
                else {
                    $("#listCoveragesEdit").UifListView("editItem", coverageIndex, coverageModel);
                }

                RiskFidelityCoverage.GetAllyCoverageByCoverage(coverageModel);
                RiskFidelityCoverage.LoadSubTitles(0);
                RiskFidelityCoverage.UpdatePremium();
                RiskFidelityCoverage.ClearCoverage();
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
                RiskFidelityCoverage.SetCoverageEdit();
            }

            $("#selectCalculeType").UifSelect("setSelected", coverageData.CalculationType);
            $("#selectRateType").UifSelect("setSelected", coverageData.RateType);
            $("#inputDeclaredAmount").val(coverageData.DeclaredAmount);
            $("#inputLimitAmount").val(coverageData.LimitAmount);
            $("#inputSubLimitAmount").val(coverageData.SubLimitAmount);
            $("#inputLimitOccurrenceAmount").val(coverageData.LimitOccurrenceAmount);
            $("#inputLimitClaimantAmount").val(coverageData.LimitClaimantAmount);
            $("#inputMaxFidelityAmount").val(coverageData.MaxFidelityAmount);
            $("#inputRate").val(coverageData.Rate);
            $("#inputPremiumAmount").val(coverageData.PremiumAmount);

            if (coverageData.Deductible != null) {
                RiskFidelityCoverage.GetDeductiblesByCoverageId(coverageData.Id, coverageData.Deductible.Id);
            }
            else {
                RiskFidelityCoverage.GetDeductiblesByCoverageId(coverageData.Id, 0);
            }

            RiskFidelityCoverage.LoadSubTitles(0);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.MessageEditCoverage, 'autoclose': true });
        }

        $("#mainCoverage").valid();
    }

    static DeleteCoverage(data) {

        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.MessageDeleteCoverage, 'autoclose': true });
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
                        var coverage = RiskFidelityCoverage.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                        if (coverage != null) {
                            coverage.Rate = FormatMoney(coverage.Rate);
                            coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                            coverage.DisplayRate = FormatMoney(coverage.Rate);
                            $("#listCoveragesEdit").UifListView("addItem", coverage);
                        }
                    }
                }
            });
            RiskFidelityCoverage.ClearCoverage();
            RiskFidelityCoverage.UpdatePremium();
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;

        RiskFidelityCoverageRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
            if (data.success) {
                coverage = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.ErrorExcludeCoverage, 'autoclose': true });
        });

        return coverage;
    }

    static GetPremium() {

        RiskFidelityCoverage.ValidCoverageValue();
        if (validCoverage) {
            var coverageModel = RiskFidelityCoverage.QuotationCoverage();
            if (coverageModel != null) {
                $("#selectCalculeType").UifSelect("setSelected", coverageModel.CalculationType);
                $("#inputDeclaredAmount").val(FormatMoney(coverageModel.DeclaredAmount));
                $("#inputLimitAmount").val(FormatMoney(coverageModel.LimitAmount));
                $("#inputSubLimitAmount").val(FormatMoney(coverageModel.SubLimitAmount));
                $("#inputLimitOccurrenceAmount").val(FormatMoney(coverageModel.LimitOccurrenceAmount));
                $("#inputLimitClaimantAmount").val(FormatMoney(coverageModel.LimitClaimantAmount));
                $("#inputMaxFidelityAmount").val(FormatMoney(coverageModel.MaxFidelityAmount));
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

    static QuotationCoverage() {
        var coverage = null;

        var coverageModel = $("#mainCoverage").serializeObject();
        coverageModel.CoverageId = $("#selectCoverage").UifSelect("getSelected");
        coverageModel.Description = $("#selectCoverage").UifSelect('getSelectedText');
        coverageModel.CurrentFrom = $('#inputFromCoverage').val();
        coverageModel.CurrentTo = $('#inputToCoverage').val();
        coverageModel.DeclaredAmount = NotFormatMoney($('#inputDeclaredAmount').val());
        coverageModel.LimitAmount = NotFormatMoney($('#inputLimitAmount').val());
        coverageModel.SubLimitAmount = NotFormatMoney($('#inputSubLimitAmount').val());
        coverageModel.LimitOccurrenceAmount = NotFormatMoney($('#inputLimitOccurrenceAmount').val());
        coverageModel.LimitClaimantAmount = NotFormatMoney($('#inputLimitClaimantAmount').val());
        coverageModel.MaxFidelityAmount = NotFormatMoney($('#inputMaxFidelityAmount').val());
        coverageModel.Rate = NotFormatMoney($('#inputRate').val());
        coverageModel.PremiumAmount = NotFormatMoney($('#inputPremiumAmount').val());
        coverageModel.TemporalId = glbPolicy.Id;
        coverageModel.RiskId = glbRisk.Id;

        if ($("#selectDeductible").UifSelect("getSelected") > 0) {
            coverageModel.DeductibleId = $("#selectDeductible").UifSelect("getSelected");
            coverageModel.DeductibleDescription = $("#selectDeductible").UifSelect('getSelectedText');
        }

        RiskFidelityCoverageRequest.QuotationCoverage(coverageModel, coverageData).done(function (data) {
            if (data.success) {
                coverage = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });

        return coverage;

    }

    static UpdatePremium() {
        var insuredAmount = 0;
        var premium = 0;

        $.each($("#listCoveragesEdit").UifListView('getData'), function (key, value) {
            insuredAmount += parseFloat(NotFormatMoney(this.LimitAmount).replace(separatorDecimal, separatorThousands));
            premium += parseFloat(NotFormatMoney(this.PremiumAmount).replace(separatorDecimal, separatorThousands));
        });

        $("#inputAmountInsured").text(FormatMoney(insuredAmount.toFixed(2)));
        $("#inputTotalPremium").text(FormatMoney(premium.toFixed(2)));
    }

    static SaveCoverages() {


        var coveragesValues = $("#listCoveragesEdit").UifListView('getData');

        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.DeclaredAmount = NotFormatMoney(this.DeclaredAmount);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.LimitOccurrenceAmount = NotFormatMoney(this.LimitOccurrenceAmount);
            this.LimitClaimantAmount = NotFormatMoney(this.LimitClaimantAmount);
            this.MaxFidelityAmount = NotFormatMoney(this.MaxFidelityAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
        });
        RiskFidelityCoverageRequest.SaveCoverages(glbPolicy.Id, glbRisk.Id, $("#listCoveragesEdit").UifListView('getData')).done(function (data) {
            if (data.success) {
                router.run("prtRiskFidelity");
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.ErrorSaveCoverages, 'autoclose': true });
        });
    }

    static ClearCoverage() {
        $('#selectCoverage').UifSelect();
        $("#selectCalculeType").UifSelect("setSelected", null);
        $('#inputDeclaredAmount').val(0);
        $('#inputLimitAmount').val(0);
        $('#inputSubLimitAmount').val(0);
        $('#inputLimitOccurrenceAmount').val(0);
        $('#inputLimitClaimantAmount').val(0);
        $('#inputMaxFidelityAmount').val(0);
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

        coverageIndex = null;
        coverageText = null;
        coverageClauses = null;
    }

    static ReturnRisk() {
        RiskFidelityCoverageRequest.ReturnRisk(glbPolicy.Id).done(function (data) {
            if (data.success) {
                var policyModelsView = data.result;
                policyModelsView.riskId = glbRisk.Id;

                $.redirect(rootPath + 'Underwriting/RiskFidelity/Fidelity', policyModelsView);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.ErrorSearchTemporary, 'autoclose': true });
        });
    }

    static ValidCoverageValue() {
        validCoverage = true;
        if ($("#selectCoverage").UifSelect("getSelected") == null || $("#selectCoverage").UifSelect("getSelected") == 0) {
            validCoverage = false;
            $.UifNotify('show', { 'type': 'info', 'message':AppResources.ErrorSelectCoverage, 'autoclose': true });
        }

        if ($("#selectCalculeType").UifSelect("getSelected") == null || $("#selectCalculeType").UifSelect("getSelected") == 0) {
            validCoverage = false;
        }

        if ($.trim($("#inputSubLimitAmount").val()) == '' || $("#inputSubLimitAmount").val() == 0) {
            validCoverage = false;
        }

        if ($.trim($("#inputRate").val()) == '' || $("#inputRate").val() == 0) {
            validCoverage = false;
        }
        if ($("#selectRateType").UifSelect("getSelected") == null || $("#selectRateType").UifSelect("getSelected") == 0) {
            validCoverage = false;
        }
    }

    static LoadTitle() {
        var title = AppResources.LabelTitleCoverage + " - " + AppResources.LabeltitleRisk + ": " + glbRisk.Description;
        $.uif2.helpers.setGlobalTitle(title);
    }

    static LoadSubTitles(subTitle) {
        if (subTitle == 0 || subTitle == 1) {
            if (coverageText != null) {
                if (coverageText.TextBody == null) {
                    coverageText.TextBody = "";
                }

                if (coverageText.TextBody.length > 0) {
                    $('#selectedTexts').text("(" +AppResources.LabelWithText + ")");
                }
                else {
                    $('#selectedTexts').text("(" +AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedTexts').text("(" +AppResources.LabelWithoutData + ")");
            }
        }

        if (subTitle == 0 || subTitle == 2) {
            if (coverageClauses != null) {
                if (coverageClauses.length > 0) {
                    $('#selectedClauses').text("(" + coverageClauses.length + ")");
                }
                else {
                    $('#selectedClauses').text("(" +AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedClauses').text("(" +AppResources.LabelWithoutData + ")");
            }
        }
    }

    static GetAllyCoverageByCoverage(coverageData) {
        RiskFidelityCoverageRequest.GetAllyCoverageByCoverage(glbPolicy.Id, glbRisk.Id, glbRisk.GroupCoverage.Id, coverageData).done(function (data) {
            if (data.success) {
                for (var i = 0; i < data.result.length; i++) {
                    data.result[i].DeclaredAmount = FormatMoney(data.result[i].DeclaredAmount);
                    data.result[i].LimitAmount = FormatMoney(data.result[i].LimitAmount);
                    data.result[i].SubLimitAmount = FormatMoney(data.result[i].SubLimitAmount);
                    data.result[i].LimitOccurrenceAmount = FormatMoney(data.result[i].LimitOccurrenceAmount);
                    data.result[i].LimitClaimantAmount = FormatMoney(data.result[i].LimitClaimantAmount);
                    data.result[i].MaxFidelityAmount = FormatMoney(data.result[i].MaxFidelityAmount);
                    data.result[i].PremiumAmount = FormatMoney(data.result[i].PremiumAmount);
                    $("#listCoveragesEdit").UifListView("addItem", data.result[i]);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message':AppResources.ErrorQuotationCoverage, 'autoclose': true });
        });
    }

}
