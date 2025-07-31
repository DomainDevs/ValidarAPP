
$(() => {
    new ParametrizationTechnicalBranch();
});

/**
 * Variables Locales y Globales
 */
var moduleIndex = null;
var inputSearch = "";
var module = {};
var linebusiness = {};
var LinesBusinessCoveredRiskType = {};
var glbSubModule = {};
var glbRiskTypeByTechnicalBranch = {};
var modalListType;
var datalinebusiness = [];
var RiskTypeByTechnicalBranch = {};
var glbLinesBusinessCoveredRiskType = {};
var search;
var idCurrentLineBusiness = 0;
var listAsigned = {};
var listAsignedPeril = {};
var glbTechnicalBranchDelete = [];
var glbTechnicalBranchEdit = [];
var glbTechnicalBranchInsert = [];
var loadRisktypes = [];
class LineBusinessTechnicalBrach {


    /**
     * @sumary Funcion para guardar los ramos tecnicos en BD
     * @param {any} linebusiness
     */
    static SaveTechnicalBranch(linebusiness) {
        return $.ajax({
            type: 'POST',
            url: 'SaveTechnicalBranch',
            data: JSON.stringify({ linebusinesModel: linebusiness }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        }).done(function (data) {

            if (data.success) {
                linebusiness = [];
                ParametrizationTechnicalBranch.ClearControls();
                $.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });


    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

///**
// * Funcion que carga los tipos de riesgo en la vista de tipos de riesgo
// */
class CoveredRiskType {
    static GetRiskType() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/TechnicalBranch/GetRiskType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

/**
 * Clase Principal de ramo tecnico
 */

class ParametrizationTechnicalBranch extends Uif2.Page {

    /**
     * Funcion para inicilizar la vista
     */
    getInitialState() {
        new RiskTypeTechnicalBranch();
        CoveredRiskType.GetRiskType().done(function (data) {
            if (data.success) {
                loadRisktypes = data.result;

                $('#selectRiskTypeTechnicalBranchMain').UifSelect({ sourceData: data.result });
                $('#selectRiskTypeCovered').UifSelect({ sourceData: loadRisktypes });
                $('#selectRiskTypeTechnical').UifSelect({ sourceData: loadRisktypes });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //LineBussiness.GetLinesBusiness().done(function (data) {
        //    if (data.success) {
        //        datalinebusiness = data.result;
        //    }
        //    else {
        //        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        //    }
        //});
    }

    /**
     * Eventos de los controles de la clase
     */
    bindEvents() {
        $('#btnRiskTypeTechnicalBranch').on('click', this.OpenRyskType);
        $('#btnProtections').on('click', this.Protection);
        $("#inputTechnicalBranchSearch").on("buttonClick", ParametrizationTechnicalBranch.GetLinesBusinessById);
        $('#btnSearchAdv').click(this.SearchTechnicalBranch);
        $('#btnNewTechnicalBranch').click(this.SaveAll);
        $("#btnSave").click(this.SaveRiskTypeByTechnicalBran);
        $("#btnShowAdvanced").on("click", this.ShowAdvanced);
        $("#btnExit").click(this.Exit);
        $("#btnExport").click(this.sendExcelLineBusiness);
    }

    /**
     * Funcion para abrir la vista de tipos de riesgo para asociar al ramo tecnico
     */
    OpenRyskType() {
        $('#ModalRiskTypeTechnicalBranch').UifModal('showLocal', Resources.Language.MessageRiskType);
        ParametrizationTechnicalBranch.loadLinesBusiness();
    }

    /**
     * Funcion para visualizar la vista de Amparos
     */
    Protection() {

        $.each(listAsignedPeril, function (key, value) {
            var protectionObjectAsg =
                {
                    DescriptionLong: this.DescriptionLong,
                    Id: this.Id,
                };
            var findObject = function (element, index, array) {
                return element.Id === protectionObjectAsg.Id
            }
            var index = $("#listviewProtectionTechnicalBranch").UifListView("findIndex", findObject);
            if (index > -1) {
                $("#listviewProtectionTechnicalBranch").UifListView("deleteItem", index);
                $("#listviewProtectionTechnicalBranchAssing").UifListView("addItem", protectionObjectAsg);
            }
        });
        $('#ModalProtection').UifModal('showLocal', Resources.Language.LabelProtection);
    }

    /**
     * Funcion para descargar excel
     */
    sendExcelLineBusiness() {
        LineBusinessTechnicalBrach.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /**
     * Funcion para visualizar los registros encontrados en la busquedas cuando se encuentra mas de uno
     */
    SelectSearch() {
        ParametrizationTechnicalBranch.getTechnicalBranch($(this).children()[0].innerHTML, "");
        $('#modalDefaultSearch').UifModal("hide");
    }

    /**
     * Obtiene y guarda los tipos de riesgo asociados al ramo tecnico
     */
    SaveRiskTypeByTechnicalBran() {
        glbRiskTypeByTechnicalBranch = $("#listViewRiskTypeTechnicalBranch").UifListView("getData")
        linebusiness.ListLineBusinessCoveredrisktype = glbRiskTypeByTechnicalBranch
        var prueba = 0;
        $("#ModalRiskTypeTechnicalBranch").UifModal('hide');

    }

    /**
     * Funcion para limpiar los controles
     */
    static ClearControls() {
        $("#inputTechnicalBranchCode").val('');
        $('#inputTechnicalBranchCode').attr("disabled", false);
        $("#inputDescriptionLong").val('');
        $("#inputDescriptionShort").val('');
        $("#inputAbbreviation").val('');
        $("#selectRiskTypeTechnicalBranchMain").UifSelect("setSelected", null);
        $('#selectRiskTypeTechnicalBranchMain').attr("disabled", false);
        $("#inputTechnicalBranchSearch").val('');
        $("#listViewRiskTypeTechnicalBranch").UifListView('refresh');
        $("#listviewProtectionTechnicalBranchAssing").UifListView('refresh');
        $("#selectedProtections").text('');
        $("#selectedCoInsurance").text('');
        new ParametrizationProtections();
        // ParametrizationTechnicalBranch.countModal();
    }

    newClear() {
        ParametrizationTechnicalBranch.ClearControls();
    }
    /**
     * Funcion para obtener los datos ingresados en los controles
     */
    static getLineModel() {
        linebusiness.Id = $("#inputTechnicalBranchCode").val();
        linebusiness.LongDescription = $("#inputDescriptionLong").val();
        linebusiness.ShortDescription = $("#inputDescriptionShort").val();
        linebusiness.TyniDescription = $("#inputAbbreviation").val();

        glbRiskTypeByTechnicalBranch = $("#listViewRiskTypeTechnicalBranch").UifListView("getData");
        if (glbRiskTypeByTechnicalBranch.length > 1) {
            linebusiness.ListLineBusinessCoveredrisktype = glbRiskTypeByTechnicalBranch;
        }
        else {
            linebusiness.RiskTypeId = $("#selectRiskTypeTechnicalBranchMain").UifSelect("getSelected");
        }
    }

    /**
     * Funcion para obtener los Amparos que fueron asociados para el ramo tecnico 
     */
    static getProtectionAssigned() {
        linebusiness.ProtectionAssigned = ParametrizationProtections.GetAssigned();
    }

    /**
     * Funcion que envia todos los datos asociados al ramo tecnico a la BD
     */
    SaveAll() {

        ParametrizationTechnicalBranch.getProtectionAssigned();
        ParametrizationTechnicalBranch.getLineModel();
        LineBusinessTechnicalBrach.SaveTechnicalBranch(linebusiness);
    }

    /**
     * Funcion para realizar busquedas 
     * @param {any} id
     * @param {any} inputSearch
     */
    static getTechnicalBranch(id, inputSearch) {
        var find = false;
        var data = [];
        var search = datalinebusiness
        if (id == 0) {
            $.each(search, function (key, value) {
                if (
                    value.Description.toLowerCase().sistranReplaceAccentMark().includes(inputSearch.toLowerCase().sistranReplaceAccentMark())) {
                    value.key = key;
                    data.push(value);
                    find = true;
                }
            });
        }
        else {
            $.each(search, function (key, value) {
                if (value.Id == id) {
                    moduleIndex = key
                    value.key = key;
                    data.push(value);
                    find = true;
                    return false;
                }
            });
        }

        if (find == false) {
            $.UifNotify('show',
                { 'type': 'danger', 'message': Resources.Language.TechicalBranchNotExist, 'autoclose': true })
        } else {
            ParametrizationTechnicalBranch.showData(null, data, data.key);
        }
    }

    /**
     * Funcion para validar y visualizar los datos encontrados en la busqueda
     * @param {any} event
     * @param {any} result
     * @param {any} index
     */
    static showData(event, result, index) {
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        if (result.length > 1) {
            AdvancedSearchTechnicalBranch.ShowSearchAdv(result);
        }
        if (result.Id != undefined) {
            idCurrentLineBusiness = result.Id;
            if (result.ListLineBusinessCoveredrisktype.length > 1) {

                glbLinesBusinessCoveredRiskType = result.ListLineBusinessCoveredrisktype;
                $('#selectRiskTypeTechnicalBranchMain').attr("disabled", true);
                $("#selectRiskTypeTechnicalBranchMain").UifSelect("setSelected", result.ListLineBusinessCoveredrisktype[0].IdRiskType);
                $("#inputDescriptionLong").val(result.Description);
                $('#inputTechnicalBranchCode').attr("disabled", true);
                $("#inputTechnicalBranchCode").val(result.Id);
                $("#inputDescriptionShort").val(result.ShortDescription);
                $("#inputAbbreviation").val(result.TyniDescription);
            }
            else if (result.ListLineBusinessCoveredrisktype[0] != undefined) {
                glbLinesBusinessCoveredRiskType = result.ListLineBusinessCoveredrisktype;
                $("#selectRiskTypeTechnicalBranchMain").UifSelect("setSelected", result.ListLineBusinessCoveredrisktype[0].IdRiskType);
                $("#inputDescriptionLong").val(result.Description);
                $("#inputTechnicalBranchCode").val(result.Id);
                $('#inputTechnicalBranchCode').attr("disabled", true);
                $("#inputDescriptionShort").val(result.ShortDescription);
                $("#inputAbbreviation").val(result.TyniDescription);
                ParametrizationTechnicalBranch.loadLinesBusiness();
                ParametrizationInsuranceObjects.InsuranceObjects();
            }
            else {
                $("#inputDescriptionLong").val(result.Description);
                $("#inputTechnicalBranchCode").val(result.Id);
                $('#inputTechnicalBranchCode').attr("disabled", true);
                $("#inputDescriptionShort").val(result.ShortDescription);
                $("#inputAbbreviation").val(result.TyniDescription);
                ParametrizationTechnicalBranch.loadLinesBusiness();

            }
            ParametrizationProtections.ModalProtectionLoadFirstTime(idCurrentLineBusiness);
            var indexObject = $('#inputTechnicalBranchCode').val();
            ParametrizationTechnicalBranch.LoadInsuranceObjects(indexObject);
            ParametrizationTechnicalBranch.countModal();
        }
    }
    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }


    //------------ACCIONES PRINCIPALES Y COMUNES
    Exit() {
        window.location = rootPath + "Home/Index";
    }
    Close() {
        $("#ModalInsuranceObjects").UifModal('hide');
    }
    CloseProtection() {
        $("#ModalProtection").UifModal('hide');
    }



    static countModal() {

        if (listAsigned.length > 0) {
            $("#selectedCoInsurance").text(listAsigned.length);
        }
        if (listAsignedPeril.length > 0) {
            $("#selectedProtections").text(listAsignedPeril.length);
        }
    }
    //-----CARGAR LISTA DE TIPOS DE RIESGO ASOCIADOS AL RAMO TECNICO
    static loadLinesBusiness() {

        if (glbLinesBusinessCoveredRiskType.length > 0) {
            $("#listViewRiskTypeTechnicalBranch").UifListView("refresh");
            $.each(glbLinesBusinessCoveredRiskType, function (key, value) {
                //ParametrizationTechnicalBranch.loadRisktype();
                $.each(loadRisktypes, function (key, valueRisk) {
                    if (value.IdRiskType == valueRisk.Value) {
                        LinesBusinessCoveredRiskType.ListLineBusinessCoveredrisktype =
                            {
                                IdRiskType: valueRisk.Text,
                                IdLineBusiness: valueRisk.Value
                            }
                        $("#listViewRiskTypeTechnicalBranch").UifListView("addItem", LinesBusinessCoveredRiskType.ListLineBusinessCoveredrisktype);
                    }
                })
            })

        }
    }

    /**
     * Funcion para mostrar en la vista los resultados encontrados
     * @param {any} dataTable datos encontrados
     */
    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    /**
     * Funcion para salir de la vista e ir a la vista de Inicio
     */
    Exit() {
        window.location = rootPath + "Home/Index";
    }

    /**
     * Funcion para salir de la Vista de Objetos de Seguro
     */
    Close() {
        $("#ModalInsuranceObjects").UifModal('hide');
    }

    /**
     * Funcion para salir de la vista de Amparos
     */
    CloseProtection() {
        $("#ModalProtection").UifModal('hide');
    }



    /**
     * Funcion para abrir la vista de Busquedas Avanzadas
     */
    ShowAdvanced() {
        $('#selectRiskTypeCovered').UifSelect({
            sourceData: loadRisktypes
        });
        $("#listviewSearchPerson").UifListView(
            {
                displayTemplate: "#searchNaturalTemplate",
                selectionType: 'single',
                source: null,
                height: 180
            });
        dropDownSearch.show();
    }

    static LoadInsuranceObjects(indexObject) {
        if (indexObject == undefined) indexObject = $('#inputTechnicalBranchCode').val();
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/TechnicalBranch/GetObjectByLineBusinessId?idLineBusiness=' + indexObject,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        }).done(function (data) {
            if (data.success) {
                listAsigned = data.result;
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Parametrization/TechnicalBranch/GetPerilByLineBusinessId?idLineBusiness=' + indexObject,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                }).done(function (dataPerfil) {
                    if (dataPerfil.success) {
                        listAsignedPeril = dataPerfil.result;
                        ParametrizationTechnicalBranch.countModal();
                    }
                    else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': dataPerfil.result, 'autoclose': true
                        });
                    }
                });
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
    }

    static LoadPeril(indexObject) {
        if (indexObject == undefined) indexObject = $('#inputTechnicalBranchCode').val();
        $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/TechnicalBranch/GetPerilByLineBusinessId?idLineBusiness=' + indexObject,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        }).done(function (data) {
            if (data.success) {
                listAsignedPeril = data.result;
                ParametrizationTechnicalBranch.countModal();
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
    }
    static GetLinesBusinessById(event, selectedItem) {
        if (!selectedItem || !selectedItem.trim() || selectedItem.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorInputSearchCoverage, 'autoclose': true
            })
            return;
        }
        $.ajax({
            type: "POST",
            url: rootPath + "Parametrization/Parametrization/GetLinesBusinessByDescription?description=" + selectedItem,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        }).done(function (data) {
            if (data.success) {
                if (data.result.length === 0) {
                    ParametrizationTechnicalBranch.ClearControls();
                    $.UifNotify('show',
                        {
                            'type': 'danger', 'message': Resources.Language.TechicalBranchNotExist, 'autoclose': true
                        });
                }
                else {
                    ParametrizationTechnicalBranch.showData(null, data.result, data.result.key);

                }
                //listAsigned = data.result;
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
        //LineBussiness.GetLinesBusiness().done(function (data) {
        //    if (data.success) {
        //        datalinebusiness = data.result;
        //    }
        //    else {
        //        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        //    }
        //});

    }
}


