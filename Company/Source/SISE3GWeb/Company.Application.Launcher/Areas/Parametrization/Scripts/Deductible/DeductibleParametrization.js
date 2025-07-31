//Clase formulario principal
var deductibleParametrizationData = {};
var deductibleIndex = null;
var dropDownSearchAdv = null;
var LineBusinessDeductible = null;
var UnitsDeductible = null;
var ApplyOnDeductible = null;
var glbDeductibleDelete = [];

$.ajaxSetup({ async: true });
class DeductibleParametrization extends Uif2.Page {
    /**
   * @summary 
      *  Metodo que se ejecuta al instanciar la clase     
   */
    getInitialState() {
        //Se cargan los datos en los campos iniciales
        request('Parametrization/Deductible/GetLineBusiness', null, 'GET', Resources.Language.ErrorSearch, DeductibleParametrization.GetLineBusiness);
        request('Parametrization/Deductible/GetDeductibleUnit', null, 'GET', Resources.Language.ErrorSearch, DeductibleParametrization.GetDeductibleUnit);
        request('Parametrization/Deductible/GetDeductibleSubject', null, 'GET', Resources.Language.ErrorSearch, DeductibleParametrization.GetDeductibleSubject);
        request('Parametrization/Deductible/GetCurrencies', null, 'GET', Resources.Language.ErrorSearch, DeductibleParametrization.GetCurrencies);
        request('Parametrization/Deductible/GetRateTypes', null, 'GET', Resources.Language.ErrorSearch, DeductibleParametrization.GetRateTypes);
        request('Parametrization/Deductible/GetDeductibles', null, 'GET', Resources.Language.ErrorSearch, DeductibleParametrization.GetDeductibles);
        //Se asignan el formato de los controles
        $("#inputMax").OnlyDecimals(4);
        $("#inputMin").OnlyDecimals(4);
        $("#inputValue").OnlyDecimals(4);
        $("#inputRate").OnlyDecimals(4);
        DeductibleParametrization.ClearForm();
        DeductibleParametrization.GetdefaultValueCurrency();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //Se instancian la busqueda avanzada  
        //new AdvancedSearchParametrization();
    }

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {
        $('#selectUnit').on('itemSelected', DeductibleParametrization.UpdateDescription);
        $('#selectUnitMin').on('itemSelected', DeductibleParametrization.UpdateDescription);
        $('#selectApplyOnMin').on('itemSelected', DeductibleParametrization.UpdateDescription);
        $('#selectCurrency').on('itemSelected', DeductibleParametrization.UpdateDescription);
        $("#inputValue").focusout(DeductibleParametrization.UpdateDescription);
        $("#inputMax").focusout(DeductibleParametrization.UpdateMax);
        $("#inputMin").focusout(DeductibleParametrization.UpdateDescription);
        $("#inputRate").focusout(DeductibleParametrization.UpdateDescription);
        $("#btnExit").click(DeductibleParametrization.Exit);
        $("#btnNewCoverage").click(DeductibleParametrization.ClearForm);
        $("#btnExport").click(DeductibleParametrization.SendExcelDeductible);
        $("#btnSaveDeductible").click(DeductibleParametrization.Save);
        $('#btnAccept').on('click', DeductibleParametrization.AddItem);
        $('#listViewDeductible').on('rowEdit', DeductibleParametrization.ShowData);
        $("#btnNew").click(DeductibleParametrization.ClearForm);
        //Buscar Deducible por Id
        $('#inputProcess').on('buttonClick', DeductibleParametrization.SearchById);

        $('#selectType').on('itemSelected', function (event, selectedItem) {
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
    }


    //EVENTOS CONTROLES
    static GetLineBusiness(data) {
        LineBusinessDeductible = { sourceData: data };
        $("#selectLineBusiness").UifSelect(LineBusinessDeductible);
    }
    static GetDeductibleUnit(data) {
        UnitsDeductible = { sourceData: data };
        $("#selectUnit").UifSelect(UnitsDeductible);
        $("#selectUnitMin").UifSelect(UnitsDeductible);
        $("#selectUnitMax").UifSelect(UnitsDeductible);
    }
    static GetDeductibleSubject(data) {
        ApplyOnDeductible = { sourceData: data };
        $("#selectApplyOn").UifSelect(ApplyOnDeductible);
        $("#selectApplyOnMin").UifSelect(ApplyOnDeductible);
        $("#selectApplyOnMax").UifSelect(ApplyOnDeductible);
    }
    static GetCurrencies(data) {
        let comboConfig = { sourceData: data };
        $("#selectCurrency").UifSelect(comboConfig);
    }
    static GetRateTypes(data) {
        let comboConfig = { sourceData: data };
        $("#selectType").UifSelect(comboConfig);
    }
    static GetDeductibles(data) {
        DeductibleParametrization.LoadDeductibles(data);
    }
    static LoadDeductibles(deductibles) {
        if (Array.isArray(deductibles)) {
            $("#listViewDeductible").UifListView({
                sourceData: deductibles,
                displayTemplate: "#DeductibleTemplate",
                selectionType: 'single',
                edit: true,
                delete: true,
                customEdit: true,
                height: 400,
                deleteCallback: DeductibleParametrization.deleteCallbackList
            });
        }
    }

    static deleteCallbackList(deferred, result) {
        deferred.resolve();
        if (result.DeductibleId !== "" && result.DeductibleId !== undefined && parseInt(result.DeductibleId) !== 0) //Se elimina unicamente si existe en DB
        {
            result.Status = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listViewDeductible").UifListView("addItem", result);
        }
    }

    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES 
    static SearchById() {
        if ($('#inputProcess').val() != "") {
            const findDeduct = function (element, index, array) {
                return element.DeductibleId === parseInt($('#inputProcess').val());
            }
            const result = $("#listViewDeductible").UifListView("find", findDeduct);

            if (result != undefined) {
                DeductibleParametrization.ShowData(null, result, null);
            }
        }
    }
    static ShowData(event, result, index) {
        DeductibleParametrization.ClearForm();
        //Si los datos a mostrar vienen de la busqueda avanzada
        if (result.length == 1) {
            result = result[0];
        }
        if (result.DeductibleId != undefined) {
            deductibleIndex = index;
            $("#inputId").val(result.DeductibleId);
            if (result.DeductibleId != 0) {
                $('#inputProcess').val(result.DeductibleId);
            }
            $("#inputDescription").val(result.TotalDescription);
            $("#inputRate").val(parseFloat(result.Rate).toFixed(4));
            if (result.Min != null && result.Min != 0) {
                $("#inputMin").val(parseFloat(result.Min).toFixed(4));
            }
            else {
                $("#inputMin").val(0);
            }
            if (result.Max != null && result.Max != 0) {
                $("#inputMax").val(parseFloat(result.Max).toFixed(4));
            }
            else {
                $("#inputMax").val(0);
            }
            $("#inputValue").val(parseFloat(result.Value).toFixed(4));
            $("#selectCurrency").UifSelect("setSelected", result.CurrencyId);
            $("#selectType").UifSelect("setSelected", result.Type);
            $("#selectUnit").UifSelect("setSelected", result.UnitId);
            $("#selectApplyOn").UifSelect("setSelected", result.ApplyOnId);
            $("#selectUnitMin").UifSelect("setSelected", result.UnitMinId);
            $("#selectApplyOnMin").UifSelect("setSelected", result.ApplyOnMinId);
            if (result.ApplyOnMaxId != undefined && result.ApplyOnMaxId != 0) {
                $("#selectUnitMax").UifSelect("setSelected", result.UnitMaxId);
                $("#selectApplyOnMax").UifSelect("setSelected", result.ApplyOnMaxId);
            }
            $("#selectLineBusiness").UifSelect("setSelected", result.LineBusinessId);
            ClearValidation("#formDeductible");
        }
    }
    static Save() {
        var itemModified = [];
        var dataTable = $("#listViewDeductible").UifListView('getData');
        $.each(dataTable, function (index, value) {
            if (value.Status != undefined && value.Status != ParametrizationStatus.Original) {
                itemModified.push(value);
            }
        });
        if (itemModified.length > 0) {
            request('Parametrization/Deductible/Save', JSON.stringify({ deductibles: itemModified }), 'POST', Resources.Language.ErrorSearch, DeductibleParametrization.ResultSave);
        }
    }
    static ResultSave(data) {
        if (Array.isArray(data.data)) {
            $.UifNotify('show', { 'type': 'info', 'message': data.message, 'autoclose': true });
            glbDeductibleDelete = [];
            DeductibleParametrization.LoadDeductibles(data.data);
        }
    }
    static UpdateMax() {
        if ($("#inputMax").val() != "") {
            var num = parseFloat($("#inputMax").val().replace(',', '.')).toFixed(4);
            $("#inputMax").val(num);
        }
        else {
            $("#inputMax").val(0);
        }
    }
    static UpdateDescription() {
        var description = "";
        var min = "";
        var unit = "";
        var currency = "";
        if ($("#inputMin").val() != "") {
            var num = parseFloat($("#inputMin").val().replace(',', '.')).toFixed(4);
            $("#inputMin").val(num);

            min = ' - Minimo: ' + $("#inputMin").val()
            if ($("#selectUnitMin").val() != "") {
                min = min + ' ' + $("#selectUnitMin").UifSelect("getSelectedText");
            }

            if ($("#selectApplyOnMin").val() != "") {
                min = min + ' Sobre: ' + $("#selectApplyOnMin").UifSelect("getSelectedText");
            }
        } else {
            $("#inputMin").val(0);
        }

        if ($("#selectUnit").val() != "") {
            unit = ' ' + $("#selectUnit").UifSelect("getSelectedText");
        }
        if ($("#selectCurrency").val() != "") {
            currency = ' ' + $("#selectCurrency").UifSelect("getSelectedText");
        }
        var num = 0;
        if ($("#inputValue").val() != "") {
            num = parseFloat($("#inputValue").val().replace(',', '.')).toFixed(4);
        }
        $("#inputValue").val(num);
        description = $("#inputValue").val() + currency + unit + min;
        $("#inputDescription").val(description);
    }
    static Exit() {
        window.location = rootPath + "Home/Index";
    }
    /**
   * @summary 
      * Limpiar formulario
   */
    static ClearForm() {
        deductibleIndex = null;
        $("#selectLineBusiness").UifSelect('setSelected', null);
        $("#selectLineBusiness").focus();
        $("#selectUnit").UifSelect('setSelected', null);
        $("#selectApplyOn").UifSelect('setSelected', null);
        $("#selectUnitMin").UifSelect('setSelected', null);
        $("#selectApplyOnMin").UifSelect('setSelected', null);
        $("#selectUnitMax").UifSelect('setSelected', null);
        $("#selectApplyOnMax").UifSelect('setSelected', null);
        $("#selectType").UifSelect('setSelected', null);
        $("#selectCurrency").UifSelect('setSelected', null);
        $("#inputDescription").val("");
        $("#inputRate").val(0);
        $("#inputValue").val(0);
        $("#inputMin").val(0);
        $("#inputMax").val(0);
        $("#inputCoverageName").val("");
        $("#inputId").val("");
        $('#inputProcess').val("");
        ClearValidation("#formDeductible");
    }
    static AddItem() {
        $("#formDeductible").validate();
        if ($("#formDeductible").valid()) {
            var deductibleNew = {};
            if ($("#inputId").val() == "") {
                deductibleNew.DeductibleId = 0;
            }
            else {
                deductibleNew.DeductibleId = parseInt($("#inputId").val());
            }
            deductibleNew.TotalDescription = $("#inputDescription").val();
            deductibleNew.Rate = parseFloat($("#inputRate").val());
            deductibleNew.Type = $("#selectType").UifSelect("getSelected");
            deductibleNew.CurrencyId = $("#selectCurrency").UifSelect("getSelected")
            deductibleNew.CurrencyDescription = $("#selectCurrency").UifSelect("getSelectedText");
            deductibleNew.Min = parseFloat($("#inputMin").val());
            deductibleNew.UnitMinId = $("#selectUnitMin").UifSelect("getSelected");
            deductibleNew.UnitMinDescription = $("#selectUnitMin").UifSelect("getSelectedText");
            deductibleNew.ApplyOnMinId = $("#selectApplyOnMin").UifSelect("getSelected");
            deductibleNew.ApplyOnMinDescription = $("#selectApplyOnMin").UifSelect("getSelectedText");

            if ($("#selectUnitMax").UifSelect("getSelected") != "" && $("#selectApplyOnMax").UifSelect("getSelected") != "") {
                deductibleNew.UnitMaxId = $("#selectUnitMax").UifSelect("getSelected");
                deductibleNew.Max = parseFloat($("#inputMax").val());
                deductibleNew.ApplyOnMaxId = $("#selectApplyOnMax").UifSelect("getSelected");
            }
            deductibleNew.Value = parseFloat($("#inputValue").val());
            deductibleNew.UnitId = $("#selectUnit").UifSelect("getSelected")
            deductibleNew.UnitDescription = $("#selectUnit").UifSelect("getSelectedText");

            deductibleNew.ApplyOnId = $("#selectApplyOn").UifSelect("getSelected");
            deductibleNew.DeductibleSubjectDescription = $("#selectApplyOn").UifSelect("getSelectedText");
            deductibleNew.LineBusinessId = $("#selectLineBusiness").UifSelect("getSelected");
            deductibleNew.LineDescription = $("#selectLineBusiness").UifSelect("getSelectedText");
            //Si los datos a guardar venían de la busqueda avanzada, se busca el index de la lista principal
            if (deductibleIndex == null && deductibleNew.DeductibleId != 0) {
                const findDeduct = function (element, index, array) {
                    return element.DeductibleId === deductibleNew.DeductibleId
                }
                deductibleIndex = $("#listViewDeductible").UifListView("findIndex", findDeduct);
            }
            if (deductibleIndex == null) {
                deductibleNew.Status = ParametrizationStatus.Create;
                deductibleNew.StatusTypeService = ParametrizationStatus.Create;
                var ifExist = $("#listViewDeductible").UifListView('getData').filter(function (item) {
                    if (item.TotalDescription != null) {
                        return item.TotalDescription.toUpperCase() == $("#inputDescription").val().toUpperCase();
                    }

                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistDeductible, 'autoclose': true });
                }
                else {
                    $("#listViewDeductible").UifListView("addItem", deductibleNew);
                }
            }
            else {
                deductibleNew.Status = ParametrizationStatus.Update;
                deductibleNew.StatusTypeService = ParametrizationStatus.Update;
                var ifExist = $("#listViewDeductible").UifListView('getData').filter(function (item) {
                    return (item.TotalDescription != null && item.TotalDescription.toUpperCase() == $("#inputDescription").val().toUpperCase()) && item.DeductibleId != $("#inputId").val()
                        && item.UnitMaxId.toString() == $("#selectUnitMax").UifSelect("getSelected")
                        && item.ApplyOnMaxId.toString() == $("#selectApplyOnMax").UifSelect("getSelected");
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistDeductible, 'autoclose': true });
                }
                $('#listViewDeductible').UifListView('editItem', deductibleIndex, deductibleNew);
            }
            DeductibleParametrization.ClearForm();
        };
    }

    /**
 * @summary 
    * Exportar excel
 */
    static SendExcelDeductible() {
        //request('Parametrization/Deductible/GenerateFileToExport', null, 'GET', Resources.Language.ErrorSearch, DeductibleParametrization.DownloadFile);
        DeductibleParametrizationRequest.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetdefaultValueCurrency() {
        if (parameters == null) {
            ParameterRequest.GetParameters().done(function (data) {
                if (data.success) {
                    parameters = data.result;
                    var pCurrency = parameters.find(Persons.GetParameterByDescription, ['Currency']);
                    if (pCurrency != undefined) {
                        $("#selectCurrency").UifSelect("setSelected", pCurrency.Value);
                    }

                }
            });
        }
    }
}
