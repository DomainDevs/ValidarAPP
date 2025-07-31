var dropDownSearchAdvRateTax = null;
var taxRateId = 0;
var titleTax = "";
var taxRateModel = {};
var selectedEconomicActivityId = null;

class RateTax extends Uif2.Page {
    getInitialState() {
        $.UifProgress('show');
        $("#CurrentFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        RateTax.LoadTaxConditions();
        RateTax.LoadTaxCategories();
        RateTax.LoadCountries();
        RateTax.LoadLinesBusiness();
        RateTax.LoadBranches();
        RateTax.LoadEconomicActivities();
        $('#Rate').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $('#Rate').OnlyDecimals(2);
        $('#MinimumTaxableBase').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $('#MinimumTaxableBase').OnlyDecimals(2);
        $('#Minimum').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $('#Minimum').OnlyDecimals(2);
        $('#RateAdditional').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $('#RateAdditional').OnlyDecimals(2);
        $('#MinimumAdditionalTaxableBase').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $('#MinimumAdditionalTaxableBase').OnlyDecimals(2);
        $('#AdditionalMinimum').ValidatorKey(ValidatorType.Decimal, 2, 1);
        $('#AdditionalMinimum').OnlyDecimals(2);

        selectedEconomicActivityId = null;
        $('#RateTaxTable').on('rowEdit', function (event, data, position) {
            RateTax.ShowModal(event, data, position);
        });

        if (taxModel.TaxRates.length > 0 && $.isEmptyObject(taxModel.TaxRates) != true) {
            RateTax.setDataTable(taxModel.TaxRates);
        }
        titleTax = taxModel.Description;
        $('#titleTax').text(titleTax);

        setTimeout(function () {
            $.UifProgress('close');
        }, 1000);
    }
    bindEvents() {
        //$('#Rate').ValidatorKey(ValidatorType.Number, 1, 1);
        //$('#MinimumTaxableBase').ValidatorKey(ValidatorType.Number, 1, 1);
        //$('#Minimum').ValidatorKey(ValidatorType.Number, 1, 1);
        //$('#RateAdditional').ValidatorKey(ValidatorType.Number, 1, 1);
        //$('#MinimumAdditionalTaxableBase').ValidatorKey(ValidatorType.Number, 1, 1);
        //$('#AdditionalMinimum').ValidatorKey(ValidatorType.Number, 1, 1);

        $('#AdvSearchRateTax').click(RateTax.ShowAdvancedSearch);
        dropDownSearchAdvRateTax = uif2.dropDown({
            source: rootPath + 'Parametrization/Tax/RateTaxAdvancedSearch',
            element: '#AdvSearchRateTax',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: RateTax.LoadbindignsSearchAdvanced,
        });
        RateTax.LoadbindignsSearchAdvanced();


        $('#btnSave').click(RateTax.SaveTaxRate);
        $('#addTaxRateModal').click(RateTax.ShowModal);
        $('#btnClose').click(RateTax.CloseModal);
        $('#ExitRate').click(RateTax.ExitRate);

        $('#EconomicActivity').on('itemSelected', RateTax.SelectEconomicActivity);
        $('#Country').on('itemSelected', RateTax.LoadStatesByCountry);
        $('#State').on('itemSelected', RateTax.LoadCitiesByCountryIdStateId);

        $('#LineBusiness').on('itemSelected', RateTax.GetCoveragesByLinesBusinessId);
    }

    static ExitRate() {
        router.rlite.run("prtTax");
    }
    static ShowModal(event, data, position) {
        taxRateId = 0;
        RateTax.CleanModal();
        $('#modalDialogTest').UifModal('showLocal', Resources.Language.AddTaxRate);
        RateTax.ShowHideSelects();
        if (data != null && event.type == 'rowEdit') {
            RateTax.SetDataModal(event, data, position);
        }
    }

    static CloseModal() {
        $('#modalDialogTest').UifModal('hide');
        RateTax.CleanModal();
    }

    static LoadTaxConditions() {
        RateTaxRequests.GetTaxConditions().done(function (response) {
            if (response.success) {
                if (taxModel != null && taxModel !== undefined && taxModel.TaxConditions != null && taxModel.TaxConditions !== undefined) {
                    TaxConditionsList = response.result;
                    var data = taxModel.TaxConditions;
                    var TaxConditionsListFiltered = [];
                    $.each(data, function (index, value) {
                        $.each(TaxConditionsList, function (index2, value2) {
                            if (data[index].Id == TaxConditionsList[index2].Id && data[index].IdTax == TaxConditionsList[index2].IdC) {
                                TaxConditionsListFiltered.push(TaxConditionsList[index2]);
                            }
                        });
                    });
                    $('#TaxCondition').UifSelect({ sourceData: TaxConditionsListFiltered });
                }
                else {
                    $('#TaxCondition').UifSelect({ sourceData: response.result });
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadTaxCategories() {
        RateTaxRequests.GetTaxCategories().done(function (response) {
            if (response.success) {
                if (taxModel != null && taxModel !== undefined && taxModel.TaxCategories != null && taxModel.TaxCategories !== undefined) {
                    TaxCategoriesList = response.result;
                    var data = taxModel.TaxCategories;
                    var TaxCategoriesListFiltered = [];
                    $.each(data, function (index, value) {
                        $.each(TaxCategoriesList, function (index2, value2) {
                            if (data[index].Id == TaxCategoriesList[index2].Id && data[index].IdTax == TaxCategoriesList[index2].IdC) {
                                TaxCategoriesListFiltered.push(TaxCategoriesList[index2]);
                            }
                        });
                    });

                    $('#TaxCategory').UifSelect({ sourceData: TaxCategoriesListFiltered });
                }
                else {
                    $('#TaxCategory').UifSelect({ sourceData: response.result });
                }

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadStatesByCountry(event, selectedItem) {
        var CountryId = selectedItem.Id;
        RateTaxRequests.GetStatesByCountry(CountryId).done(function (response) {
            if (response.success) {
                $('#State').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadStatesByCountryId(CountryId) {
        RateTaxRequests.GetStatesByCountry(CountryId).done(function (response) {
            if (response.success) {
                $('#State').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadCitiesByCountryIdStateId(event, selectedItem) {
        var countryId = $("#Country").val();
        var stateId = selectedItem.Id;
        RateTaxRequests.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
            if (response.success) {
                $('#City').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetCoveragesByLinesBusinessId(event, selectedItem) {
        var lineBusinessId = selectedItem.Id;

        RateTaxRequests.GetCoveragesByLinesBusinessId(lineBusinessId).done(function (response) {
            if (response.success) {
                $('#Coverage').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadCoveragesByLineBusinessIdModal(lineBusinessId, coverageId) {

        RateTaxRequests.GetCoveragesByLinesBusinessId(lineBusinessId).done(function (response) {
            if (response.success) {
                $('#Coverage').UifSelect({ sourceData: response.result });
                $('#Coverage').UifSelect("setSelected", coverageId);
                $("#Coverage").UifSelect("disabled", true);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });

    }

    static LoadCitiesByCountryIdStateIdModal(countryId, stateId, CityId) {
        RateTaxRequests.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
            if (response.success) {
                $('#City').UifSelect({ sourceData: response.result });
                $('#City').UifSelect("setSelected", CityId);
                $("#City").UifSelect("disabled", true);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }

        });
    }

    static LoadStatesByCountryModal(CountryId, StateId) {
        RateTaxRequests.GetStatesByCountry(CountryId).done(function (response) {
            if (response.success) {
                $('#State').UifSelect({ sourceData: response.result });
                $('#State').UifSelect("setSelected", StateId);
                $("#State").UifSelect("disabled", true);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadCountries() {
        RateTaxRequests.GetCountries().done(function (response) {
            if (response.success) {
                CountriesList = response.result;
                $('#Country').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static LoadStatesByCountryId(CountryId) {
        RateTaxRequests.GetStatesByCountry(CountryId).done(function (response) {
            if (response.success) {
                $('#State').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadLinesBusiness() {
        RateTaxRequests.GetLinesBusiness().done(function (response) {
            if (response.success) {
                LinesBusinessList = response.result;
                $('#LineBusiness').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadBranches() {
        RateTaxRequests.GetBranches().done(function (response) {
            if (response.success) {
                BranchesList = response.result;
                $('#TechnicalBranch').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadEconomicActivities() {
        RateTaxRequests.GetEconomicActivities().done(function (response) {
            if (response.success || response != null || response !== undefined) {
                EconomicActivitiesList = response;
                //$('#EconomicActivity').UifAutoComplete({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadbindignsSearchAdvanced() {
        $("#lvSearchAdvRateTax").UifListView({ source: null, displayTemplate: "#AdvSearchRateTaxTemplate", selectionType: 'single', height: 140 });
    }
    static ShowAdvancedSearch() {
        dropDownSearchAdvRateTax.show();
        $('.uif-dropdown').css('z-index', 1000);
    }

    static SelectEconomicActivity(event, selectedItem) {
        selectedEconomicActivityId = selectedItem.Id;
    }

    static SaveTaxRate() {
        lockScreen();
        $("#formRateTax").validate();
        if ($("#formRateTax").valid()) {
            RateTax.SetDataTaxRate();
            RateTaxRequests.SaveTaxRate(taxRateModel).done(function (response) {
                if (response.success) {
                    if (response.result.Id > 0) {
                        console.log(response.result);
                        $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.TaxRateSaved, 'autoclose': true });
                        //RateTax.setDataTable(response.result);
                        var search = titleTax;                        
                        TaxRequests.GetTaxByDescription(search).done(function (response) {
                            if (response.success) {                                
                                taxModel.TaxRates = response.result[0].TaxRates;
                                RateTax.setDataTable(taxModel.TaxRates);
                                RateTax.CloseModal();
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
                            }
                        }).always(function () {
                            unlockScreen();
                        });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RecordError, 'autoclose': true });
                        taxRateModel = {};
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RecordError, 'autoclose' : true });
                }
            }).always(function () {
                unlockScreen();
            });
        }
    }

    static SetDataTaxRate() {
        taxRateModel.CurrentFrom = $("#CurrentFrom").val();
        taxRateModel.Rate = $("#Rate").val();
        taxRateModel.MinimumTaxableBase = $("#MinimumTaxableBase").val();
        taxRateModel.Minimum = $("#Minimum").val();

        taxRateModel.BasicTaxIncludedAdditionalBase = $("#BasicTaxIncludedAdditionalBase").is(":checked");
        taxRateModel.RateAdditional = $("#RateAdditional").val();
        taxRateModel.MinimumAdditionalTaxableBase = $("#MinimumAdditionalTaxableBase").val();
        taxRateModel.AdditionalMinimum = $("#AdditionalMinimum").val();

        taxRateModel.TaxCondition = $("#TaxCondition").val() != null ? $("#TaxCondition").val() : 0;
        taxRateModel.TaxCategory = $("#TaxCategory").val() != null ? $("#TaxCategory").val() : 0;
        taxRateModel.State = $("#State").val() != null ? $("#State").val() : 0;
        taxRateModel.City = $("#City").val() != null ? $("#City").val() : 0;
        taxRateModel.Country = $("#Country").val() != null ? $("#Country").val() : 0;
        taxRateModel.EconomicActivityId = selectedEconomicActivityId != null ? selectedEconomicActivityId : 0;
        taxRateModel.LineBusiness = $("#LineBusiness").val() != null ? $("#LineBusiness").val() : 0;
        taxRateModel.TechnicalBranch = $("#TechnicalBranch").val() != null ? $("#TechnicalBranch").val() : 0;
        taxRateModel.Coverage = $("#Coverage").val() != null ? $("#Coverage").val() : 0;
        taxRateModel.IdTax = taxModel.Id;
        taxRateModel.IdRate = taxRateId;
    }

    static setDataTable(data) {

        //Formating decimals
        $.each(data, function (index, value) {
            data[index].TaxPeriodRate.Rate = data[index].TaxPeriodRate.Rate.toFixed(4).replace(".", ",");
            data[index].TaxPeriodRate.AdditionalRate = data[index].TaxPeriodRate.AdditionalRate.toFixed(4).replace(".", ",");
            data[index].TaxPeriodRate.MinTaxAMT = data[index].TaxPeriodRate.MinTaxAMT.toFixed(4).replace(".", ",");
            data[index].TaxPeriodRate.MinAdditionalTaxAMT = data[index].TaxPeriodRate.MinAdditionalTaxAMT.toFixed(4).replace(".", ",");
            data[index].TaxPeriodRate.MinBaseAMT = data[index].TaxPeriodRate.MinBaseAMT.toFixed(4).replace(".", ",");
            data[index].TaxPeriodRate.MinAdditionalBaseAMT = data[index].TaxPeriodRate.MinAdditionalBaseAMT.toFixed(4).replace(".", ",");
        });

        //CurrentFrom Date Formated
        var finalDate = "";
        var d;
        for (d = 0; d < data.length; d++) {
            var date = data[d].TaxPeriodRate.CurrentFrom;
            var NewDate = new Date(parseInt(date.substr(6)));
            var Day = NewDate.getDate();
            var Month = NewDate.getMonth() + 1;
            var Year = NewDate.getFullYear();
            if (Month < 10) {
                finalDate = Day + '/' + '0' + Month + '/' + Year;
            }
            else {
                finalDate = Day + '/' + Month + '/' + Year;
            }

            data[d].TaxPeriodRate.CurrentFrom = finalDate;
        }
        ClearValidation("#formRateTax");
        $('#RateTaxTable').UifDataTable({ sourceData: data });

        //Show-Hide Columns of Data Table
        $.each(data, function (index, value) {
            if (this["EconomicActivity"].Id == 0 || this["EconomicActivity"].Id == null) {
                var TitleColumn = $('#RateTaxTable th[data-property="EconomicActivity.Description"]').first();
                TitleColumn.hide();
                $('#RateTaxTable tbody tr').each(function () {
                    var ContentColumn = $('#RateTaxTable tbody td:nth-child(4)');
                    ContentColumn.hide();
                });
            }
            if (this["TaxState"].IdState == 0 || this["TaxState"].IdState == null) {
                var TitleColumn = $('#RateTaxTable th[data-property="TaxState.DescriptionState"]').first();
                TitleColumn.hide();
                $('#RateTaxTable > tbody >  tr').each(function () {
                    var ContentColumn = $('#RateTaxTable tbody td:nth-child(6)');
                    ContentColumn.hide();
                });
            }
            if (this["TaxState"].IdCountry == 0 || this["TaxState"].IdCountry == null) {
                var TitleColumn = $('#RateTaxTable th[data-property="TaxState.DescriptionCountry"]').first();
                TitleColumn.hide();
                $('#RateTaxTable > tbody > tr').each(function () {
                    var ContentColumn = $('#RateTaxTable tbody td:nth-child(5)');
                    ContentColumn.hide();
                });
            }
            if (this["TaxState"].IdCity == 0 || this["TaxState"].IdCity == null) {
                var TitleColumn = $('#RateTaxTable th[data-property="TaxState.DescriptionCity"]').first();
                TitleColumn.hide();
                $('#RateTaxTable > tbody > tr').each(function () {
                    var ContentColumn = $('#RateTaxTable tbody td:nth-child(7)');
                    ContentColumn.hide();
                });
            }
            if (this["LineBusiness"].Id == 0 || this["LineBusiness"].Id == null) {
                var TitleColumn = $('#RateTaxTable th[data-property="LineBusiness.Description"]').first();
                TitleColumn.hide();
                $('#RateTaxTable tbody tr').each(function () {
                    var ContentColumn = $('#RateTaxTable tbody td:nth-child(8)');
                    ContentColumn.hide();
                });
            }
            if (this["Branch"].Id == 0 || this["Branch"].Id == null) {
                var TitleColumn = $('#RateTaxTable th[data-property="Branch.Description"]').first();
                TitleColumn.hide();
                $('#RateTaxTable tbody tr').each(function () {
                    var ContentColumn = $('#RateTaxTable tbody td:nth-child(9)');
                    ContentColumn.hide();
                });
            }
            if (this["Coverage"].Id == 0 || this["Coverage"].Id == null) {
                var TitleColumn = $('#RateTaxTable th[data-property="Coverage.Description"]').first();
                TitleColumn.hide();
                $('#RateTaxTable tbody tr').each(function () {
                    var ContentColumn = $('#RateTaxTable tbody td:nth-child(10)');
                    ContentColumn.hide();
                });
            }
        });

        RateTax.CleanModal();
    }
    

    static ShowHideSelects() {
        //1	TAX_CONDITION_CODE
        //2	TAX_CATEGORY_CODE
        //3	STATE_CODE
        //4	COUNTRY_CODE
        //5	ECONOMIC_ACTIVITY_CODE
        //6	BRANCH_CODE
        //7	LINE_BUSINESS_CODE
        //8	COVERAGE_ID
        var FeeAppliesArray = ["TaxRateConditionDiv", "TaxRateCategoryDiv", "TaxRateStateDiv", "TaxRateCountryDiv", "TaxRateEconomicActivityDiv",
            "TaxRateTechnicalBranchDiv", "TaxRateLineBusinessDiv", "TaxRateCoverageDiv", "TaxRateCityDiv"];
        var FeesApplies = taxModel.TaxAttributes;
        var i = 0;
        var j = 0;

        for (i = 0; i < 9; i++) {
            $('#' + FeeAppliesArray[i] + '').hide();
        }

        for (i = 0; i < 9; i++) {
            for (j = 0; j < FeesApplies.length; j++) {
                if ((i + 1) == FeesApplies[j].Id) {
                    $('#' + FeeAppliesArray[i] + '').show();
                    if (FeeAppliesArray[i] == "TaxRateCountryDiv") {
                        $('#Country').UifSelect("setSelected", DefaultCountryId);
                    }
                    if (FeeAppliesArray[i] == "TaxRateStateDiv") {
                        RateTax.LoadStatesByCountryId(DefaultCountryId);
                    }
                }
            }
        }

    }

    static SetDataModal(event, data, position) {
      
        //Inhabilitar campos por edicion
        $("#CurrentFrom").prop('disabled', true);
        $("#Rate").prop('disabled', true);
        $("#RateAdditional").prop('disabled', true);
        $("#TaxCondition").UifSelect("disabled", true);
        $("#TaxCategory").UifSelect("disabled", true);
        $("#State").UifSelect("disabled", true);
        $("#Country").UifSelect("disabled", true);
        $("#City").UifSelect("disabled", true);
        $("#EconomicActivity").prop('disabled', true);
        $("#LineBusiness").UifSelect("disabled", true);
        $("#TechnicalBranch").UifSelect("disabled", true);
        $('#Coverage').UifSelect("disabled", true);

        //Set Data
        taxRateId = data.Id;
        $('#CurrentFrom').val(data.TaxPeriodRate.CurrentFrom);
        $('#Rate').val(data.TaxPeriodRate.Rate);
        $('#MinimumTaxableBase').val(data.TaxPeriodRate.MinBaseAMT);
        $('#Minimum').val(data.TaxPeriodRate.MinTaxAMT);
        $('input[name=BasicTaxIncludedAdditionalBase]').prop('checked', data.TaxPeriodRate.BaseTaxAdditional);
        $('#RateAdditional').val(data.TaxPeriodRate.AdditionalRate);
        $('#MinimumAdditionalTaxableBase').val(data.TaxPeriodRate.MinAdditionalBaseAMT);
        $('#AdditionalMinimum').val(data.TaxPeriodRate.MinAdditionalTaxAMT);
        $('#TaxCondition').UifSelect("setSelected", data.TaxCondition.Id);
        $('#TaxCategory').UifSelect("setSelected", data.TaxCategory.Id);
        RateTax.LoadStatesByCountryModal(data.TaxState.IdCountry, data.TaxState.IdState);
        RateTax.LoadCitiesByCountryIdStateIdModal(data.TaxState.IdCountry, data.TaxState.IdState, data.TaxState.IdCity);
        RateTax.LoadCoveragesByLineBusinessIdModal(data.LineBusiness.Id, data.Coverage.Id);
        $('#Country').UifSelect("setSelected", data.TaxState.IdCountry);
        $('#EconomicActivity').val(data.EconomicActivity.Description);
        selectedEconomicActivityId = data.EconomicActivity.Id;
        $('#LineBusiness').UifSelect("setSelected", data.LineBusiness.Id);
        $('#TechnicalBranch').UifSelect("setSelected", data.Branch.Id);
    }

    static CleanModal() {
        //Cleaning modal
        var date = new Date();
        function pad(n) { return n < 10 ? "0" + n : n; }
        var convertedDate = pad(date.getDate()) + "/" + pad(date.getMonth() + 1) + "/" + date.getFullYear();
        $("#CurrentFrom").UifDatepicker("setValue", convertedDate);
        $("#Rate").val('');
        $("#MinimumTaxableBase").val('');
        $("#Minimum").val('');

        $('input[name=BasicTaxIncludedAdditionalBase]').prop('checked', false);
        $("#RateAdditional").val('');
        $("#MinimumAdditionalTaxableBase").val('');
        $("#AdditionalMinimum").val('');

        $("#TaxCondition").UifSelect("setSelected", null);
        $("#TaxCategory").UifSelect("setSelected", null);
        $("#State").UifSelect("setSelected", null);
        $("#Country").UifSelect("setSelected", null);
        $("#City").UifSelect("setSelected", null);
        $("#EconomicActivity").val('');
        $("#LineBusiness").UifSelect("setSelected", null);
        $("#TechnicalBranch").UifSelect("setSelected", null);


        //Habilitar Campos
        $("#CurrentFrom").prop('disabled', false);
        $("#Rate").prop('disabled', false);
        $("#RateAdditional").prop('disabled', false);

        $("#TaxCondition").UifSelect("disabled", false);
        $("#TaxCategory").UifSelect("disabled", false);
        $("#State").UifSelect("disabled", false);
        $("#Country").UifSelect("disabled", false);
        $("#City").UifSelect("disabled", false);
        $("#EconomicActivity").prop('disabled', false);
        $("#LineBusiness").UifSelect("disabled", false);
        $("#TechnicalBranch").UifSelect("disabled", false);
        $("#Coverage").UifSelect("disabled", false);
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        $(this).val(FormatMoney($(this).val()));
    }

}