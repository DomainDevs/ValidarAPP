var ally_coverage_code = null; //
var coverage_code = null;
var ally_coverage_index = null;
var dropDownSearchAllyCoverage = null;
var gblDropDownListCoverage = null; //Lista con las coberturas aliadas
var gblDropDownListAllyCovDeleted = [];
var gblDropDownListAdv = null;
var allyCoverageUpdate = null;
var loadData = false;
var isDelete = false;
var allyCoverageIndex = null;
$.ajaxSetup({ async: true });

/**
 * Class type AllyCoverage
 * @author Germán F. Grimaldi
 * */
class AllyCoverage extends Uif2.Page {
    /**
     * Estado Inicial 
     * */
    getInitialState() {
        // Se define List model 
        $('#lsvAllyCoverages').UifListView({
            displayTemplate: "#display-template-AllyCoverage",
            source: null,
            selectionType: 'single',
            height: 400,
            edit: true,
            delete: true,
            customEdit: true,
            filter: true,
            filterColumns: ["AllyCoverageId", "AllyCoverageId_object.PrintDescription"],
            deleteCallback: this.DeleteAllyCoverage,
        });
       // Carga datos en el listView
        AllyCoverage.GetAlliesCoverage().done(function (data) {
            AllyCoverage.LoadAllyCoverage(data);
        });
        if (!loadData) {
            AllyCoverageCore.GetPrincipalCoverage().done(function (data) {
                if (data.success) {
                    gblDropDownListCoverage = data.result;
                    $('#PrincipalCoverageId').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });  
        }
        $('#IdAllyCoveragePct').ValidatorKey(ValidatorType.Number, 2, 0);
        $('#IdAllyCoveragePct').prop('disabled', true);
    }

    /**
     * Asignación delegados
     * */
    bindEvents() {
        $('#btnAllyCoverageNew').on('click', AllyCoverage.Clear);
        $('#btnAllyCoverageAccept').on('click', AllyCoverage.AddAllyCoverage);
        $('#PrincipalCoverageId').on('itemSelected', AllyCoverage.LoadAllyCoverageByCoverage);
        $("#lsvAllyCoverages").on('rowEdit', AllyCoverage.AllyCoverageShowData);
        $("#lsvAllyCoverages").on('rowDelete', this.DeleteAllyCoverage);
        $('#btnExport').on('click', AllyCoverage.SendExcelAllyCoverage);
        $('#btnSave').on('click', AllyCoverage.SaveAllyCoverage);
        $('#btnExit').on('click', AllyCoverage.Exit);
    }

    SearchAllyCoverage() {
        var inputAllyCover = $('#btnSearchAdvScript').val();
        if (inputAllyCover.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else if (inputAllyCover.length > 15) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMaximumChar, autoclose: false });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': 'data result test', 'autoclose': true });
        }
        $('#btnSearchAdvScript').val('');
    }

    static AddAllyCoverage() {
        $("#AllyCoverageForm").validate();
        if ($("#AllyCoverageForm").valid()) {
            let CoverageId = $("#PrincipalCoverageId").UifSelect("getSelected")
            let PrintDescription = $("#PrincipalCoverageId option:selected").text()
            let AllyCoveragePrintDescription = $("#IdAllyCoverage option:selected").text()
            let AllyCoverageId = $("#IdAllyCoverage").UifSelect("getSelected")
            let CoveragePct = $('#IdAllyCoveragePct').val()

            var allycoverage = {
                CoverageId,
                CoveragePct,
                AllyCoverageId,
                "CoverageId_object": {
                   PrintDescription
                },
                "AllyCoverageId_object": {
                    PrintDescription: AllyCoveragePrintDescription
                }
            }
            //create
            if (ally_coverage_index === null) {
                allycoverage.Status = ParametrizationStatus.Create;
                $("#lsvAllyCoverages").UifListView('addItem', allycoverage);
            } else {//edit
                allycoverage.Status = ParametrizationStatus.Update;
                $("#lsvAllyCoverages").UifListView('editItem', ally_coverage_index, allycoverage,);
            }
            AllyCoverage.Clear();
        }       
    }

    DeleteAllyCoverage(event, result) {
        event.resolve();
        if (result) {
            if (result != null && result.AllyCoverageId != null && result.CoverageId != null) {
                gblDropDownListAllyCovDeleted.push(result);
                result.Status = ParametrizationStatus.Delete;
                result.allowEdit = false;
                result.allowDelete = false;
                $("#lsvAllyCoverages").UifListView("addItem", result);
                isDelete = true;
            }
            //AllyCoverage.Clear();
        }
    }

    static setUpdateStatus(obj) {
        coverage_code = obj;
    }

    static AllyCoverageShowData(event, result, index) {
        AllyCoverage.Clear();
        if (result) {
            ally_coverage_code = { Id: result.AllyCoverageId };//Código cobertura aliada
            coverage_code = { Id: result.CoverageId };
            ally_coverage_index = index;

            $("#PrincipalCoverageId").UifSelect("setSelected", result.CoverageId);
            AllyCoverage.LoadAllyCoverageByCoverage(event, coverage_code);
            //$("#IdAllyCoverage").UifSelect("setSelected", result.AllyCoverageId);
            $("#IdAllyCoveragePct").val(result.CoveragePct);
            allyCoverageUpdate = {
                CoverageId: coverage_code.Id,
                AllyCoverageId: ally_coverage_code.Id,
                CoveragePct: result.CoveragePct
            };
            $("#PrincipalCoverageId, #IdAllyCoverage").prop("disabled", true);
            //$("#IdAllyCoverage").prop("disabled", true);
        } else
            $.UifNotify('show', { 'type': 'danger', 'message': 'Error al carga los datos', 'autoclose': true });
    }

    static AllyCoverageEditSearch(result) {
        AllyCoverage.Clear();
        //agregar al listview principal
        var index = $("#lsvAllyCoverages").UifListView("findIndex", function (element, index, array) {
            return (element.AllyCoverageId == result.AllyCoverageId && element.CoverageId == result.CoverageId);
        });
        if (index >= 0) {            
            $("#lsvAllyCoverages").UifListView('editItem', index, result, );
        }
        else {
            $("#lsvAllyCoverages").UifListView('addItem', result);

            index = $("#lsvAllyCoverages").UifListView("findIndex", function (element, index, array) {
                return (element.AllyCoverageId == result.AllyCoverageId && element.CoverageId == result.CoverageId);
            });
        }
        $("#lsvAllyCoverages").UifListView("setSelected", index, true);
        AllyCoverage.AllyCoverageShowData(null, result, index);
    }

    static LoadPrincipalCoverage() {
        AllyCoverageCore.GetPrincipalCoverage().done(function (data) {
            if (data) {
                $('#PrincipalCoverageId').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadAllyCoverageByCoverage(event, selectedItem) {//Busca por insured_object_id
        if (event != null && event.currentTarget.id !== "lsvAllyCoverages")
            coverage_code = null;//coverage_code
        if (selectedItem.Id == "") {
            $("#IdAllyCoverage").prop('disabled', 'disabled');
        }
        else {
            var object_insure_id = gblDropDownListCoverage.find(x => x.Id === Number(selectedItem.Id)).InsuredObjectId;
            var Id_Coverage = selectedItem.Id;
            if (Id_Coverage > 0) {
                AllyCoverageCore.GetAllyCoverage(object_insure_id).done(function (data) {
                    if (data.result) {
                        $('#IdAllyCoverage').UifSelect({ sourceData: data.result });
                        $("#IdAllyCoverage").UifSelect("setSelected", ally_coverage_code != null ? ally_coverage_code.Id : ally_coverage_code);
                        $('#IdAllyCoveragePct').prop('disabled', false);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $("#IdAllyCoverage").UifSelect('setSelected', null);
            }
        }
    }

    static LoadAllyCoverage(data) {
        if (data.success) {
            // Limpiar ListView
            $("#lsvAllyCoverages").UifListView("clear");
           
            // Llenar objeto con los datos obtenidos
            $.each(data.result, function (key, value) {
                // Agregar objeto al ListView                
                $("#lsvAllyCoverages").UifListView("addItem", value);
            });
            gblDropDownListAdv = data.result;
        } else {
            $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
        }
    }

    static GetAlliesCoverage() {
        //carga de todos los AllysCoverages
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/AllyCoverage/GetAllyCoverages',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static SendExcelAllyCoverage() {
        $.ajax({
            type: "POST",
            async: false,
            url: rootPath + 'Parametrization/AllyCoverage/GetFileToAllyCoverage',
            data: JSON.stringify({ fileName: 'ALLYCOB' }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                AllyCoverage.ExportFileSuccess(data.result);
                $.UifNotify('show', { 'type': 'info', 'message': 'Archivo Descargado correctamente', 'autoclose': true });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });

            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Se generó un error importando el excel', 'autoclose': true });
        });
    }
    static ExportFileSuccess(data) {
        DownloadFile(data);
    }

    static SaveAllyCoverage() {
         //Filtrar coberturas a crear, eliminar, editar
        var list = $("#lsvAllyCoverages").UifListView('getData').filter(x => [ParametrizationStatus.Create, ParametrizationStatus.Delete, ParametrizationStatus.Update].includes(x.Status));

        if (list !== null && list.length>0) {           
            $.ajax({
                type: "POST",
                async: false,
                url: rootPath + 'Parametrization/AllyCoverage/SaveListAllyCoverages',
                data: JSON.stringify({ listAllyCoverages: list}),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    AllyCoverage.Clear();
                    // Limpiar ListView del advance
                    $("#lvSearchAdvancedAllyCoverage").UifListView("clear");
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveAllYCoverage, 'autoclose': true });
            });
            //cargar listview
            // Carga datos en el listView
            AllyCoverage.GetAlliesCoverage().done(function (data) {
                AllyCoverage.LoadAllyCoverage(data);
            });    
        }     
    }

    static ReceiveCoverages(data) {
        // Llenar list con los datos obtenidos
        $.each(data, function (key, value) {
            // buscar en list y agregar 
            var index = $("#lsvAllyCoverages").UifListView("findIndex", function (element, index, array) {
                return (element.AllyCoverageId == value.AllyCoverageId && element.CoverageId == value.CoverageId);
            });
            if (index < 0) {
                $("#lsvAllyCoverages").UifListView("addItem", value);
            }
            else if(value.Status == ParametrizationStatus.Delete){
                $("#lsvAllyCoverages").UifListView('editItem', index, value, );
            }
        });
    }

    ShowSearchAdv(data) {
        $("#lsvAllyCoverages").UifListView({
            displayTemplate: "#display-template-AllyCoverage",
            selectionType: "single",
            height: 300
        });
        dropDownSearchAllyCoverage.show();
    }

    static GetAllyCoverageByDescription() {

    }
    
    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    static Clear() {
        $("#IdAllyCoveragePct").val(""); 
        //$("#IdAllyCoverage").UifSelect('setSelected', null);
        $("#IdAllyCoverage").empty();
        $("#PrincipalCoverageId").prop('disabled', false);
        $("#PrincipalCoverageId").UifSelect('setSelected', null);
        
        $("#IdAllyCoverage").UifSelect('setSelected', null);
        $("#IdAllyCoverage").prop('disabled', true);
        $('#IdAllyCoveragePct').prop('disabled', true);
        coverage_code = null;
        ally_coverage_code = null;
        ally_coverage_index = null;
    }
}

class AllyCoverageCore {
    static GetAllyCoverage(Id_Coverage) {
        return $.ajax({
            type: 'POST',
            async: false,
            url: rootPath + 'Parametrization/AllyCoverage/GetCoverageAllied',
            data: JSON.stringify({ id_object_insure: Id_Coverage, }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetAllyCoverageByCoverage(coverage) {
        return $.ajax({
            type: 'POST',
            async: true,
            url: rootPath + 'Parametrization/AllyCoverage/GetAllyCoverageByCoverage',
            data: JSON.stringify({ coverage: coverage }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetPrincipalCoverage() {
        loadData = true;
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/AllyCoverage/GetPrincipalCoverage',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static confirmDeleteAllyCoverage(data) {
        var test = data.result;
    }    
}