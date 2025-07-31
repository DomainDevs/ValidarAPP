//Codigo de la pagina Deductible.cshtml
var errorLoad = 0;
var myDeductible = {};
class DeductiblesCoverage extends Uif2.Page {
    getInitialState() {
        DeductiblesCoverage.FormatDeductibles();
        DeductiblesCoverage.InitializeCoverageDeductibles();
    }
    //Eventos de los objetos.
    bindEvents() {
        DeductiblesCoverage.initNumberInput();
        $('#btnDeductibleCoverage').on('click', DeductiblesCoverage.GetDeductibleCoverage);

        $('#inputDeductValue').on('focusout', DeductiblesCoverage.LoadDeductSummary);
        $('#inputMinDeductValue').on('focusout', DeductiblesCoverage.LoadDeductSummary);
        $('#inputMaxDeductValue').on('focusout', DeductiblesCoverage.LoadDeductSummary);

        $('#selectDeductUnitCd').on('itemSelected', DeductiblesCoverage.LoadDeductSummary);
        $('#selectDeductSubjectCd').on('itemSelected', DeductiblesCoverage.LoadDeductSummary);
        $('#selectMinDeductUnitCd').on('itemSelected', DeductiblesCoverage.LoadDeductSummary);
        $('#selectMinDeductSubjectCd').on('itemSelected', DeductiblesCoverage.LoadDeductSummary);
        $('#selectMaxDeductUnitCd').on('itemSelected', DeductiblesCoverage.LoadDeductSummary);
        $('#selectMaxDeductSubjectCd').on('itemSelected', DeductiblesCoverage.LoadDeductSummary);

        $('#btnDeductibleSave').on('click', DeductiblesCoverage.DeductibleSave);
        $('#inputDeductValue').focusin(function () {
            var value = NotFormatMoney($.trim($(this).val()));
            value == 0 ? $(this).val("") : $(this).val(NotFormatMoney($(this).val()));
        });
        //
        $('#inputDeductValue').OnlyDecimals(UnderwritingDecimal);
        $('#inputMinDeductValue').OnlyDecimals(UnderwritingDecimal);
        $('#inputMaxDeductValue').OnlyDecimals(UnderwritingDecimal);
        $('#inputDeductValue').on('focusin', this.inputMoneyOnFocusin);
        $('#inputDeductValue').focusout(this.assignZero);
        $('#inputMinDeductValue').on('focusin', this.inputMoneyOnFocusin);
        $('#inputMinDeductValue').focusout(this.assignZero);
        $('#inputMaxDeductValue').on('focusin', this.inputMoneyOnFocusin);
        $('#inputMaxDeductValue').focusout(this.assignZero);

        $('#selectRateTypeDeduct').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0) {
                var RateCoverage = parseInt(selectedItem.Id);
                switch (RateCoverage) {
                    case RateType.Percentage:
                        $("#rateDeductible").prop('disabled', false);
                        $("#rateDeductible").prop("maxLength", 3);
                        break;
                    default:
                        $("#rateDeductible").val(0);
                        $("#rateDeductible").prop('disabled', true);
                        break;
                }
            }
        });



    }
    assignZero() {
        var value = NotFormatMoney($.trim($(this).val()));
        value == "" ? $(this).val(0) : $(this).val(FormatMoney(value));
    }
    inputMoneyOnFocusin() {
        var value = NotFormatMoney($.trim($(this).val()));
        value == 0 ? $(this).val("") : $(this).val(value);
    }
    //Carga de datos y visualización de la ventana modal.
    static GetDeductibleCoverage() {
        if ($('#selectCoverage').val() > 0) {
            if (($('#selectDeductUnitCd Option').length == 0) && ($('#selectMinDeductUnitCd Option').length == 0) && ($('#selectMaxDeductUnitCd Option').length == 0)) {
                DeductiblesCoverage.LoadDeductibleUnit();
            }
            if (($('#selectDeductSubjectCd Option').length == 0) && ($('#selectMinDeductSubjectCd Option').length == 0) && ($('#selectMaxDeductSubjectCd Option').length == 0)) {
                DeductiblesCoverage.LoadDeductibleSubject();
            }

            if ($("#selectRateTypeDeduct").UifSelect("getSelected") == "") {
                DeductiblesCoverage.GetRateTypesDeduct();
            }

            DeductiblesCoverage.FormatDeductibles();
            if (($('#selectDeductible').UifSelect('getSelected') != '') && ($('#selectDeductible').UifSelect('getSelected') != null)) {
                var deductibleId = parseInt($('#selectDeductible').UifSelect('getSelected'));

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
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectCoverage, 'autoclose': true });
        }

    }

    //Formato inicial de los objetos del formulario
    static FormatDeductibles() {
        $('#inputDeductValue').val('0');
        $('#inputMinDeductValue').val('0');
        $('#inputMaxDeductValue').val('0');
        $('#selectDeductUnitCd').UifSelect('setSelected', null);
        $('#selectDeductSubjectCd').UifSelect('setSelected', null);
        $('#selectMinDeductUnitCd').UifSelect('setSelected', null);
        $('#selectMinDeductSubjectCd').UifSelect('setSelected', null);
        $('#selectMaxDeductUnitCd').UifSelect('setSelected', null);
        $('#selectMaxDeductSubjectCd').UifSelect('setSelected', null);
        $('#Description').val('');
        $('#rateDeductible').val('0');
        $('#selectRateTypeDeduct').UifSelect('setSelected', null);
        $('.field-validation-error').removeClass('field-validation-error');
    }

    //Carga de las listas desplegables de unidades del deducible
    static LoadDeductibleUnit() {
        DeductibleRequest.GetDeductibleUnit().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#selectDeductUnitCd').UifSelect({ sourceData: data.result });
                    $('#selectMinDeductUnitCd').UifSelect({ sourceData: data.result });
                    $('#selectMaxDeductUnitCd').UifSelect({ sourceData: data.result });
                    errorLoad = 0;
                }
            }
            else {
                errorLoad = 1;
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            errorLoad = 1;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetUnit, 'autoclose': true });
        });
    }

    //Carga de las listas desplegables del subject del deducible
    static LoadDeductibleSubject() {
        DeductibleRequest.GetDeductibleSubject().done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    $('#selectDeductSubjectCd').UifSelect({ sourceData: data.result });
                    $('#selectMinDeductSubjectCd').UifSelect({ sourceData: data.result });
                    $('#selectMaxDeductSubjectCd').UifSelect({ sourceData: data.result });
                    errorLoad = 0;
                }
            }
            else {
                errorLoad = 1;
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            errorLoad = 1;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorGetDeductibles, 'autoclose': true });
        });
    }

    static GetRateTypesDeduct() {
        DeductibleRequest.GetRateTypesDeduct().done(function (data) {
            if (data.success) {
                if ($('#selectRateTypeDeduct').val() != "" && $('#selectRateTypeDeduct').val() != null && $('#selectRateTypeDeduct').val() >0 ) {
                    $('#selectRateTypeDeduct').UifSelect({ sourceData: data.result, selectedItem: coverageDeductibles.RateType});    
                } else {
                    $('#selectRateTypeDeduct').UifSelect({ sourceData: data.result });    
                }
            }
        });
    }

    static ValidateDeductiblesRate() {
        var type = $("#selectRateTypeDeduct").UifSelect("getSelected");
        type = parseInt(type);
        var rate = parseFloat($("#rateDeductible").val().replace(',', '.'));

        switch (type) {
            case RateType.FixedValue:
                $("#rateDeductible").val(0);
                $("#rateDeductible").prop('disabled', true);
                return true;
            case RateType.Percentage:
                if (rate > 100 || rate < 0) {
                    $("#rateDeductible").val(0);
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorValueRate, 'autoclose': true });
                    return false
                } else {
                    return true;
                }
        }
        return false;
    }
    static ChangeDeductible() {
        if (coverageDeductibles.DeductValue != myDeductible.DeductValue) { return true }
        if (coverageDeductibles.MinDeductValue != myDeductible.MinDeductValue) { return true }
        if (coverageDeductibles.MaxDeductValue != myDeductible.MaxDeductValue) { return true }
        if (coverageDeductibles.DeductibleUnit != null && coverageDeductibles.DeductibleUnit.Id != myDeductible.DeductibleUnit.Id) { return true }
        if (coverageDeductibles.DeductibleSubject != null && coverageDeductibles.DeductibleSubject.Id != myDeductible.DeductibleSubject.Id) { return true }
        if (coverageDeductibles.MinDeductibleUnit != null && coverageDeductibles.MinDeductibleUnit.Id != myDeductible.MinDeductibleUnit.Id) { return true }
        if (coverageDeductibles.MinDeductibleSubject != null && coverageDeductibles.MinDeductibleSubject.Id != myDeductible.MinDeductibleSubject.Id) { return true }
        if (coverageDeductibles.MaxDeductibleUnit != null && coverageDeductibles.MaxDeductibleUnit.Id != myDeductible.MaxDeductibleUnit.Id) { return true }
        if (coverageDeductibles.MaxDeductibleSubject != null && coverageDeductibles.MaxDeductibleSubject.Id != myDeductible.MaxDeductibleSubject.Id) { return true }
        if (coverageDeductibles.Rate != myDeductible.RateDeduct) { return true }
        if (coverageDeductibles.RateType != myDeductible.RateType) { return true }
        return false;
    }
    //Posiciona en las listas desplegables la configuración de un deducible existente.
    static GetDeductibles(deductibleId) {
        if (($('#selectDeductUnitCd Option').length != 0) && ($('#selectMinDeductUnitCd Option').length != 0) && ($('#selectMaxDeductUnitCd Option').length != 0) &&
            ($('#selectDeductSubjectCd Option').length != 0) && ($('#selectMinDeductSubjectCd Option').length != 0) && ($('#selectMaxDeductSubjectCd Option').length != 0)) {
            myDeductible = Object.assign({}, coverageDeductibles)
            if (coverageDeductibles.Id == deductibleId) {
                $('#inputDeductValue').val(FormatMoney(coverageDeductibles.DeductValue));
                $('#inputMinDeductValue').val(FormatMoney(coverageDeductibles.MinDeductValue));
                $('#inputMaxDeductValue').val(FormatMoney(coverageDeductibles.MaxDeductValue));
                $('#rateDeductible').val(FormatMoney(coverageDeductibles.Rate));
                $("#selectRateTypeDeduct").UifSelect('setSelected', coverageDeductibles.RateType);

                if (coverageDeductibles.DeductibleUnit != null) {
                    $('#selectDeductUnitCd').UifSelect('setSelected', coverageDeductibles.DeductibleUnit.Id);
                }
                if (coverageDeductibles.DeductibleSubject != null) {
                    $('#selectDeductSubjectCd').UifSelect('setSelected', coverageDeductibles.DeductibleSubject.Id);
                }
                if (coverageDeductibles.MinDeductibleUnit != null) {
                    $('#selectMinDeductUnitCd').UifSelect('setSelected', coverageDeductibles.MinDeductibleUnit.Id);
                }
                if (coverageDeductibles.MinDeductibleSubject != null) {
                    $('#selectMinDeductSubjectCd').UifSelect('setSelected', coverageDeductibles.MinDeductibleSubject.Id);
                }
                if (coverageDeductibles.MaxDeductibleUnit != null) {
                    $('#selectMaxDeductUnitCd').UifSelect('setSelected', coverageDeductibles.MaxDeductibleUnit.Id);
                }
                if (coverageDeductibles.MaxDeductibleSubject != null) {
                    $('#selectMaxDeductSubjectCd').UifSelect('setSelected', coverageDeductibles.MaxDeductibleSubject.Id);
                }

                $('#Description').val(coverageDeductibles.Description);
                errorLoad = 0;
            }
        }
        else {
            errorLoad = 1;
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorGetUnit, 'autoclose': true });
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorGetDeductibles, 'autoclose': true });
        }
    }

    //Arma una cadena con la descripción de la configuración del deducible
    static LoadDeductSummary() {
        var deduct = $('#inputDeductValue').val() + ' ';
        var unit = $('#selectDeductUnitCd').UifSelect('getSelected') != '' ? $('#selectDeductUnitCd').UifSelect('getSelectedText') + ' ' : '';
        var subject = $('#selectDeductSubjectCd').UifSelect('getSelected') != '' ? $('#selectDeductSubjectCd').UifSelect('getSelectedText') + ' - ' : ' - ';
        var deductSummary = deduct + unit + subject;

        var minDeduc = $('#inputMinDeductValue').val() + ' ';
        var minUnit = $('#selectMinDeductUnitCd').UifSelect('getSelected') != '' ? $('#selectMinDeductUnitCd').UifSelect('getSelectedText') + ' ' : '';
        var minSubject = $('#selectMinDeductSubjectCd').UifSelect('getSelected') != '' ? $('#selectMinDeductSubjectCd').UifSelect('getSelectedText') + ' - ' : ' - ';
        var minDeducSummary = minDeduc + minUnit + minSubject;

        var maxDeduct = $('#inputMaxDeductValue').val() + ' ';
        var maxUnit = $('#selectMaxDeductUnitCd').UifSelect('getSelected') != '' ? $('#selectMaxDeductUnitCd').UifSelect('getSelectedText') + ' ' : '';
        var maxSubject = $('#selectMaxDeductSubjectCd').UifSelect('getSelected') != '' ? $('#selectMaxDeductSubjectCd').UifSelect('getSelectedText') + ' ' : '';
        var maxDeductSummary = maxDeduct + maxUnit + maxSubject;

        $('#Description').val(deductSummary + minDeducSummary + maxDeductSummary);
    }

    //Transpone la configuración del deducible a lacoverageDeductibles variable coverageDeductibles
    static DeductibleSave() {
        $('#formDeductibles').validate();

        if ($('#formDeductibles').valid()) {

            if (DeductiblesCoverage.ValidateDeductiblesRate()) {
                if (coverageDeductibles == null) {
                    DeductiblesCoverage.InitializeCoverageDeductibles();
                }
                if (coverageDeductibles.Description != $('#Description').val()) {
                    DeductiblesCoverage.SerializeDeductible();
                    DeductiblesCoverage.ClearAddSelectDeductible();
                }
                $('#modalDeductibles').UifModal('hide');
            }

        }
    }

    //Serializa el formulario del deducible y 
    //Transpone la configuración del deducible a lacoverageDeductibles variable coverageDeductibles
    static SerializeDeductible() {
        var serializeDeductibles = $('#formDeductibles').serializeObject();

        coverageDeductibles.Description = serializeDeductibles.Description;
        coverageDeductibles.DeductValue = NotFormatMoney(serializeDeductibles.DeductValue);
        if (serializeDeductibles.DeductUnitCd != null && serializeDeductibles.DeductUnitCd > -1) {
            if (coverageDeductibles.DeductibleUnit == null) {
                coverageDeductibles.DeductibleUnit = {}
            }
            coverageDeductibles.DeductibleUnit.Id = serializeDeductibles.DeductUnitCd;
        }
        if (coverageDeductibles.DeductibleSubject == null) {
            coverageDeductibles.DeductibleSubject = {}
        }
        coverageDeductibles.DeductibleSubject.Id = serializeDeductibles.DeductSubjectCd;
        coverageDeductibles.MinDeductValue = NotFormatMoney(serializeDeductibles.MinDeductValue);
        if (coverageDeductibles.MinDeductibleUnit == null) {
            coverageDeductibles.MinDeductibleUnit = {}
        }
        if (coverageDeductibles.MinDeductibleSubject == null) {
            coverageDeductibles.MinDeductibleSubject = {}
        }
        coverageDeductibles.MinDeductibleUnit.Id = serializeDeductibles.MinDeductUnitCd;
        coverageDeductibles.MinDeductibleSubject.Id = serializeDeductibles.MinDeductSubjectCd;
        coverageDeductibles.MaxDeductValue = NotFormatMoney(serializeDeductibles.MaxDeductValue);
        if (coverageDeductibles.MaxDeductibleUnit == null) {
            coverageDeductibles.MaxDeductibleUnit = {}
        }
        coverageDeductibles.MaxDeductibleUnit.Id = serializeDeductibles.MaxDeductUnitCd;
        if (coverageDeductibles.MaxDeductibleSubject == null) {
            coverageDeductibles.MaxDeductibleSubject = {}
        }
        coverageDeductibles.MaxDeductibleSubject.Id = serializeDeductibles.MaxDeductSubjectCd;
        if (DeductiblesCoverage.ChangeDeductible()) {
            coverageDeductibles.Id = -1
        }
        coverageDeductibles.RateType = serializeDeductibles.RateTypeDeduct;
        coverageDeductibles.Rate = serializeDeductibles.RateDeduct;
    }

    //Si el objeto no existe, inicializa coverageDeductibles
    static InitializeCoverageDeductibles() {
        coverageDeductibles = {};
        coverageDeductibles.Description = '';
        coverageDeductibles.DeductibleUnit = {};
        coverageDeductibles.DeductibleSubject = {};
        coverageDeductibles.MinDeductibleUnit = {};
        coverageDeductibles.MinDeductibleSubject = {};
        coverageDeductibles.MaxDeductibleUnit = {};
        coverageDeductibles.MaxDeductibleSubject = {};
        //coverageDeductibles.RateType = {};
        //coverageDeductibles.RateDeduct = '';

    }

    //Limpia la lista desplegable y agrega la descripción de la configuración del deducible.
    static ClearAddSelectDeductible() {
        $('#selectDeductible').empty();
        $('#selectDeductible').append($('<option value=' + "''" + '>- Seleccione un item -</option>'));
        $('#selectDeductible').append($('<option value=' + "'-1'" + '>' + $('#Description').val() + '</option > '));
        $('#selectDeductible').UifSelect('setSelected', '-1');
    }

    //Inicializa y mantiene el formato de los TextBoxFor
    static initNumberInput() {
        $('.decimal-number').keypress(function (event) {
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) &&
                ((event.which < 48 || event.which > 57) &&
                    (event.which != 0 && event.which != 8))) {
                event.preventDefault();
            }
            var text = $(this).val();
            if ((text.indexOf('.') != -1) &&
                (text.substring(text.indexOf('.')).length > 2) &&
                (event.which != 0 && event.which != 8) &&
                ($(this)[0].selectionStart >= text.length - 2)) {
                event.preventDefault();
            }
        });
    }
}