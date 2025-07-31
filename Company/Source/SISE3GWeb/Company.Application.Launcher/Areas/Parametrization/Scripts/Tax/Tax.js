var dropDownSearchAdvTax = null;
var IdTaxSelected = null;
var taxModel = {};


var CountriesList = {};
var EconomicActivitiesList = {};
var TaxConditionsList = {};
var TaxCategoriesList = {};
var BranchesList = {};
var LinesBusinessList = {};
var DefaultCountryId = null;

class ParametrizationTax extends Uif2.Page {
    getInitialState() {
        $.UifProgress('show');
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#CurrentFrom").ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
        $("#CurrentFrom").UifDatepicker("setValue", new Date());
        ParametrizationTax.LoadRateTypeTax();
        ParametrizationTax.LoadRateTypeAdditionalTax();
        ParametrizationTax.LoadBaseConditionTax();
        ParametrizationTax.LoadFeesApplies();
        ParametrizationTax.LoadRoles();
        if (DefaultCountryId == null) {
            ParametrizationTax.GetDefaultCountry();
        }
        $('#Retention').click(ParametrizationTax.CheckRetention);
        ParametrizationTax.CheckIfTaxExists();
        $('#AdvSearch').hide();

        setTimeout(function () {
            $.UifProgress('close');
        }, 1600);
    }
    bindEvents() {
        $('#Save').click(ParametrizationTax.SaveTaxParametrization);
        $('#Category').click(ParametrizationTax.ShowCategoryTax);
        $('#Condition').click(ParametrizationTax.ShowCondition);
        $('#Rate').click(ParametrizationTax.ShowRate);
        $('#Exit').click(ParametrizationTax.redirectIndex);
        $('#New').click(ParametrizationTax.cleanFields);
        $('#SearchTax').on("buttonClick", ParametrizationTax.SearchTax);
        //$('#AdvSearch').click(ParametrizationTax.ShowAdvancedSearch);
        $("#Excel").click(ParametrizationTax.ExportExcelTax);

        if (dropDownSearchAdvTax == null) {
            dropDownSearchAdvTax = uif2.dropDown({
                source: rootPath + 'Parametrization/Tax/TaxSearch',
                element: '#AdvSearch',
                align: 'right',
                width: 400,
                height: 400,
                loadedCallback: ParametrizationTax.LoadbindignsSearchAdvanced,
            });
        }

    }

    static LoadbindignsSearchAdvanced() {
        $("#lvSearchAdvTax").UifListView({
            source: null,
            displayTemplate: "#advancedSearchTaxTemplate",
            selectionType: 'single',
            height: 400,
            filter: false,
        });

        $('#lvSearchAdvTax').on('itemSelected', function (event, item) {
            ParametrizationTax.SelectSearchedTax(event, item);
        })
    }
    static ShowCategoryTax() {
        if (IdTaxSelected == null) {
            $.UifNotify('show', { 'type': 'info', 'message': 'Solo se puede asociar Categorias cuando el impuesto se encuentre registrado.' });
        }
        else {
            router.rlite.run("prtCategoryTax");
        }
    }

    static GetDefaultCountry() {
        TaxRequests.GetDefaultCountry().done(function (response) {
            if (response.success) {
                DefaultCountryId = response.result;
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static ShowCondition() {
        if (IdTaxSelected == null) {
            $.UifNotify('show', { 'type': 'info', 'message': 'Solo se puede asociar Condiciones cuando el impuesto se encuentre registrado.' });
        } else {
            router.rlite.run("prtConditionTax");
        }
    }
    static ShowRate() {
        if (IdTaxSelected == null && (taxModel != null && taxModel !== undefined)) {
            $.UifNotify('show', { 'type': 'info', 'message': 'Solo se puede asociar una Tasa cuando el impuesto se encuentre registrado.' });
        }
        else if (taxModel.TaxCategories == null || taxModel.TaxCategories === undefined) {
            $.UifNotify('show', { 'type': 'info', 'message': 'Debe tener alguna categoria de impuesto creada' });
        }
        else if (taxModel.TaxConditions == null || taxModel.TaxConditions === undefined) {
            $.UifNotify('show', { 'type': 'info', 'message': 'Debe tener alguna condición de impuesto creada' });
        }
        else {
            router.rlite.run("prtRateTax");
        }
    }
    static SaveTaxParametrization() {
        $("#formTax").validate();
        if ($("#formTax").valid()) {
            ParametrizationTax.SetDataTax();
            if (ParametrizationTax.ValidateFeesAppliesSelected()) {
                TaxRequests.SaveTax(taxModel).done(function (response) {
                    if (response.success) {

                        if (response.result.Id > 0) {
                            $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.TaxSaved, 'autoclose': true });
                            ParametrizationTax.setDataForm(response.result);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RecordError, 'autoclose': true });
                            taxModel = {};
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RecordError, 'autoclose': true });
                        taxModel = {};
                    }
                });
            }
        }
    }
    static ShowAdvancedSearch(data) {
        if (data.length > 0) {
            var dataTest = [];
            $.each(data, function (index, value) {
                dataTest.push({ "Id": data[index].Id, "Description": data[index].Description, "TinyDescription": data[index].TinyDescription });
                $("#lvSearchAdvTax").UifListView("addItem", dataTest[index]);
            });
        }
        $('#AdvSearch').show();
        dropDownSearchAdvTax.show();
        $('.uif-dropdown').css('z-index', 1000);
        $('#AdvSearch').hide();
    }
    static LoadRateTypeTax() {
        TaxRequests.GetRateTypeTax().done(function (response) {
            if (response.success) {
                $('#RateTypeTax').UifSelect({ sourceData: response.result });
                if (taxModel != null && taxModel !== undefined &&
                    taxModel.RateType != null && taxModel.RateType !== undefined) {
                    $('#RateTypeTax').UifSelect("setSelected", taxModel.RateType.Id);
                    $("#RateTypeTax").UifSelect("disabled", true);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadRateTypeAdditionalTax() {
        TaxRequests.GetRateTypeAdditionalTax().done(function (response) {
            if (response.success) {
                $('#RateTypeAdditionalTax').UifSelect({ sourceData: response.result });
                if (taxModel != null && taxModel !== undefined &&
                    taxModel.AdditionalRateType != null && taxModel.AdditionalRateType !== undefined) {
                    $('#RateTypeAdditionalTax').UifSelect("setSelected", taxModel.AdditionalRateType.Id);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadBaseConditionTax() {
        TaxRequests.GetBaseConditionTax().done(function (response) {
            if (response.success) {
                $('#ConditionBaseTax').UifSelect({ sourceData: response.result });
                if (taxModel != null && taxModel !== undefined &&
                    taxModel.BaseConditionTax != null && taxModel.BaseConditionTax !== undefined) {
                    $('#ConditionBaseTax').UifSelect("setSelected", taxModel.BaseConditionTax.Id);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadBaseTaxWithholding() {
        TaxRequests.GetBaseTaxWithholding().done(function (response) {
            if (response.success) {
                $('#BaseTaxWithholding').UifSelect({ sourceData: response.result });
                if (taxModel != null && taxModel !== undefined &&
                    taxModel.RetentionTax != null && taxModel.RetentionTax !== undefined) {
                    $('#BaseTaxWithholding').UifSelect("setSelected", taxModel.RetentionTax.Id);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadFeesApplies() {
        TaxRequests.GetFeesApplies().done(function (response) {
            if (response.success) {
                $('#FeesApplies').UifMultiSelect({ sourceData: response.result });
                if (taxModel != null && taxModel !== undefined &&
                    taxModel.TaxAttributes != null && taxModel.TaxAttributes !== undefined) {
                    var i;
                    for (i = 0; i < taxModel.TaxAttributes.length; i++) {
                        if (taxModel.TaxAttributes[i].Id != null && taxModel.TaxAttributes[i].Id !== undefined) {
                            $('#FeesApplies').UifMultiSelect('setSelected', taxModel.TaxAttributes[i].Id);
                            $('#FeesApplies').UifMultiSelect('disabled', true);
                        }
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static LoadRoles() {
        TaxRequests.GetRoles().done(function (response) {
            if (response.success) {
                $('#Roles').UifMultiSelect({ sourceData: response.result });
                if (taxModel != null && taxModel !== undefined &&
                    taxModel.TaxRoles != null && taxModel.TaxRoles !== undefined) {
                    var i;
                    for (i = 0; i < taxModel.TaxRoles.length; i++) {
                        if (taxModel.TaxRoles[i].Id != null && taxModel.TaxRoles[i].Id !== undefined) {
                            $('#Roles').UifMultiSelect('setSelected', taxModel.TaxRoles[i].Id);
                        }
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        });
    }
    static redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    static cleanFields() {
        $('#Description').val('');
        $('#Abbreviation').val('');
        $("#CurrentFrom").UifDatepicker("setValue", new Date());

        $('#RateTypeAdditionalTax').UifSelect("setSelected", null);
        $('#ConditionBaseTax').UifSelect("setSelected", null);

        $('input[name=AccrualCalculation]').prop('checked', false);
        $('input[name=MinimumBaseCalculation]').prop('checked', false);
        $('input[name=RateAdditionalCalculation]').prop('checked', false);
        $('input[name=Enabled]').prop('checked', false);

        $('input[name=Retention]').prop('checked', false);
        $("#BaseTaxWithholding").prop("disabled", true);
        $("#BaseTaxWithholding").UifSelect();

        $('#FeesApplies').UifMultiSelect('deSelectAll');
        $('#Roles').UifMultiSelect('deSelectAll');
        IdTaxSelected = null;
        taxModel = {};

        $("#Description").prop('disabled', false);
        $("#Abbreviation").prop('disabled', false);
        $("#CurrentFrom").prop('disabled', false);

        $("#RateTypeTax").UifSelect('disabled', false);
        $("#RateTypeTax").UifSelect("setSelected", null);

        $('#FeesApplies').UifMultiSelect('disabled', false);

        $('#SearchTax').val('');
        $('#lvSearchAdvTax').UifListView("refresh");
        dropDownSearchAdvTax.hide();
        $('#AdvSearch').hide();

        ClearValidation("#formTax");
    }

    static CheckRetention() {
        if ($("#Retention").is(":checked")) {
            $("#BaseTaxWithholding").prop("disabled", false);
            ParametrizationTax.LoadBaseTaxWithholding();
        } else {
            $("#BaseTaxWithholding").prop("disabled", true);
            $("#BaseTaxWithholding").UifSelect();
        }
    }

    static ValidateFeesAppliesSelected() {
        var FeesAppliesSelected = $("#FeesApplies").UifMultiSelect('getSelected');
        var result = true;
        $.each(FeesAppliesSelected, function (index, value) {
            switch (value) {
                //Para marca Ciudad es Obligatorio marcar pais y departamento
                case "9":
                    if (FeesAppliesSelected.includes("3") == false || FeesAppliesSelected.includes("4") == false) {
                        result = false;
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorValidateFeesAppliesCity, 'autoclose': true });
                        taxModel = {};
                    }
                    break;
                //Para marcar Departamento es Obligatorio marcar pais
                case "3":
                    if (FeesAppliesSelected.includes("4") == false) {
                        result = false;
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorValidateFeesAppliesState, 'autoclose': true });
                        taxModel = {};
                    }
                    break;
                //Para marcar Covertura, Obligatorio marcar Ramo
                case "8":
                    if (FeesAppliesSelected.includes("7") == false) {
                        result = false;
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorValidateFeesAppliesCoverage, 'autoclose': true });
                        taxModel = {};
                    }
                    break;

            };
        });
        return result;
    }

    static SetDataTax() {
        var feesRequired = ["1", "2"];
        taxModel.Description = $("#Description").val();
        taxModel.Abbreviation = $("#Abbreviation").val();
        taxModel.CurrentFrom = $("#CurrentFrom").val();

        taxModel.RateTypeTax = $("#RateTypeTax").val();
        taxModel.RateTypeAdditionalTax = $("#RateTypeAdditionalTax").val();
        taxModel.ConditionBaseTax = $("#ConditionBaseTax").val();

        taxModel.AccrualCalculation = $("#AccrualCalculation").is(":checked");
        taxModel.MinimumBaseCalculation = $("#MinimumBaseCalculation").is(":checked");
        taxModel.RateAdditionalCalculation = $("#RateAdditionalCalculation").is(":checked");

        taxModel.Enabled = $("#Enabled").is(":checked");
        taxModel.Retention = $("#Retention").is(":checked");
        taxModel.BaseTaxWithholding = $("#BaseTaxWithholding").val() != null ? $("#BaseTaxWithholding").val() : 0;

        taxModel.FeesApplies = $("#FeesApplies").UifMultiSelect('getSelected');
        $.each(feesRequired, function (index, value) {
            taxModel.FeesApplies.push(value);
        });

        taxModel.Roles = $("#Roles").UifMultiSelect('getSelected');
    }

    static SearchTax() {
        lockScreen();
        var search = $('#SearchTax').val();
        if (search.length < 3) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.LabelMinimumThreeCharacters });
        }
        else {
            TaxRequests.GetTaxByDescription(search).done(function (response) {
                if (response.success) {
                    ParametrizationTax.SearchTaxDescriptionSuccess(response);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
                }
            }).always(function() {
                unlockScreen();
            });
        }
    }

    static SearchTaxDescriptionSuccess(data) {

        //console.log(data);
        if (data != null && data.result.length != 0) {
            ParametrizationTax.cleanFields();

            if (data.result.length == 1) {
                ParametrizationTax.setDataForm(data.result[0]);
            }
            else {
                $('#lvSearchAdvTax').UifListView("refresh");
                ParametrizationTax.ShowAdvancedSearch(data.result);
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchModel, 'autoclose': true });
        }
    }

    static setDataForm(data) {

        //ClearValidation("#formTax");
        $('#Description').val(data.Description);
        $('#Abbreviation').val(data.TinyDescription);

        var finalDate = "";
        var date = data.CurrentFrom;
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
        $('#CurrentFrom').val(finalDate);

        $('#RateTypeTax').UifSelect("setSelected", data.RateType.Id);
        $('#RateTypeAdditionalTax').UifSelect("setSelected", data.AdditionalRateType.Id);
        $('#ConditionBaseTax').UifSelect("setSelected", data.BaseConditionTax.Id);

        data.IsEarned ? $('input[name=AccrualCalculation]').prop('checked', true) : $('input[name=AccrualCalculation]').prop('checked', false);
        data.IsSurPlus ? $('input[name=MinimumBaseCalculation]').prop('checked', true) : $('input[name=MinimumBaseCalculation]').prop('checked', false);
        data.IsAdditionalSurPlus ? $('input[name=RateAdditionalCalculation]').prop('checked', true) : $('input[name=RateAdditionalCalculation]').prop('checked', false);
        data.Enabled ? $('input[name=Enabled]').prop('checked', true) : $('input[name=Enabled]').prop('checked', false);
        if (data.IsRetention) {
            $('input[name=Retention]').prop('checked', true);
            ParametrizationTax.LoadBaseTaxWithholding();
            //    setTimeout(function () {
            $('#BaseTaxWithholding').UifSelect("setSelected", data.RetentionTax.Id);
            //    }, 200);
        }
        else {
            $('input[name=Retention]').prop('checked', false);
            $('#BaseTaxWithholding').UifSelect();
        }

        var i;
        for (i = 0; i < data.TaxAttributes.length; i++) {
            if (data.TaxAttributes[i].Id != null && data.TaxAttributes[i].Id !== undefined) {
                $('#FeesApplies').UifMultiSelect('setSelected', data.TaxAttributes[i].Id);
            }
            else {
                $('#FeesApplies').UifMultiSelect('setSelected', data.TaxAttributes[i]);
            }
        }
        for (i = 0; i < data.TaxRoles.length; i++) {
            if (data.TaxRoles[i].Id != null && data.TaxRoles[i].Id !== undefined) {
                $('#Roles').UifMultiSelect('setSelected', data.TaxRoles[i].Id);
            }
            else {
                $('#Roles').UifMultiSelect('setSelected', data.TaxRoles[i]);
            }
        }

        //Inhabilitar campos por edicion
        $("#Description").prop('disabled', true);
        $("#Abbreviation").prop('disabled', true);
        $("#CurrentFrom").prop('disabled', true);

        $("#RateTypeTax").UifSelect("disabled", true);

        $('#FeesApplies').UifMultiSelect('disabled', true);

        //Asignando IdTax para uso en otras páginas
        IdTaxSelected = data.Id;
        //taxModel.Id = IdTaxSelected;
        //taxModel.TaxRates = data.TaxRates;
        taxModel = data;


        //ParametrizationTax.SetDataTax();
    }

    static CheckIfTaxExists() {
        if (taxModel != null && taxModel !== undefined && $.isEmptyObject(taxModel) != true) {

            //parsing string-numeric data to decimal data of TaxPeriodRate
            var taxModelForReturn = taxModel;

            $.each(taxModelForReturn.TaxRates, function (index, value) {
                taxModelForReturn.TaxRates[index].TaxPeriodRate.Rate = parseFloat(taxModelForReturn.TaxRates[index].TaxPeriodRate.Rate);
                taxModelForReturn.TaxRates[index].TaxPeriodRate.AdditionalRate = parseFloat(taxModelForReturn.TaxRates[index].TaxPeriodRate.AdditionalRate);
                taxModelForReturn.TaxRates[index].TaxPeriodRate.MinTaxAMT = parseFloat(taxModelForReturn.TaxRates[index].TaxPeriodRate.MinTaxAMT);
                taxModelForReturn.TaxRates[index].TaxPeriodRate.MinAdditionalTaxAMT = parseFloat(taxModelForReturn.TaxRates[index].TaxPeriodRate.MinAdditionalTaxAMT);
                taxModelForReturn.TaxRates[index].TaxPeriodRate.MinBaseAMT = parseFloat(taxModelForReturn.TaxRates[index].TaxPeriodRate.MinBaseAMT);
                taxModelForReturn.TaxRates[index].TaxPeriodRate.MinAdditionalBaseAMT = parseFloat(taxModelForReturn.TaxRates[index].TaxPeriodRate.MinAdditionalBaseAMT);
            });

            ParametrizationTax.setDataForm(taxModelForReturn);
        }
    }

    // Exportal Excel
    static ExportExcelTax() {
        lockScreen();
        if (IdTaxSelected == null) {
            $.UifNotify('show', { 'type': 'info', 'message': 'Solo se puede crear el excel cuando el impuesto se encuentre registrado.' });
        }
        else {
            TaxRequests.GenerateFileToExport(IdTaxSelected).done(function (data) {
                if (data.success) {
                    DownloadFile(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).always(function () {
                unlockScreen();
            });
        }
    }

    //Seleccionar impuesto de busqueda avanzada
    static SelectSearchedTax(event, item) {
        lockScreen();
        ParametrizationTax.cleanFields();
        var taxId = item.Id;
        var taxDescription = item.Description;
        TaxRequests.GetTaxByIdAndDescription(taxId, taxDescription).done(function (response) {
            if (response.success) {
                ParametrizationTax.setDataForm(response.result[0]);
                dropDownSearchAdvTax.hide();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': response.result, 'autoclose': true });
            }
        }).always(function () {
            unlockScreen();
        });
    }


}