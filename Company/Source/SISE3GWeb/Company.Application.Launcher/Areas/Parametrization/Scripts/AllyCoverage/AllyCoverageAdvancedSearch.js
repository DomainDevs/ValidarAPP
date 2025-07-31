var dropDownSearchAdvAllyCoverage = null;
var gblDropDownListCoverageAdvSearch = [];
class AllyCoverageAdvancedSearch extends Uif2.Page {
    getInitialState() {

    }
    bindEvents() {
        //Evento que carga de  dropdownlist
        $('#btnSearchAdvScript').click(AllyCoverageAdvancedSearch.ShowAdvancedSearch);
        dropDownSearchAdvAllyCoverage = uif2.dropDown({
            source: rootPath + 'Parametrization/AllyCoverage/SearchAdvancedAllyCoverage',
            element: '#btnSearchAdvScript',
            align: 'right',
            width: 560,
            height: 551,
            filter: true,
            filterColumns: ["AllyCoverageId", "AllyCoverageId_object.PrintDescription"],
            loadedCallback: AllyCoverageAdvancedSearch.LoadBindFormAllyCoverageAbm,
        });
    }
    
    //Eventos de los objetos de la vista de busqueda avanzada
    static LoadBindFormAllyCoverageAbm() {
        $('#ddlAdvPrincipalCov').on('itemSelected', AllyCoverageAdvancedSearch.LoadAllyCoverage);
        $("#btnCancelSearchAllyCoverage").on("click", AllyCoverageAdvancedSearch.CancelSearchAdvanzed);
        $('#btnSearchAllyCoverage').click(AllyCoverageAdvancedSearch.LoadCoverages);
        $("#lvSearchAdvancedAllyCoverage").UifListView({
            sourceData: null, displayTemplate: "#templateSearchAdvancedAllyCoverage", selectionType: 'single', height: 200, filter: true,
            filterColumns: ["AllyCoverageId", "AllyCoverageId_object.PrintDescription"]
        });
        $('#btnAcceptAdvAllyCoverage').click(AllyCoverageAdvancedSearch.ExecuteAdvancedSearch);
    }

    DeleteSelectCoverage(event, result) {
        event.resolve();
        if (result) {
            if (result != null && result.AllyCoverageId != null && result.CoverageId != null) {
                result.Status = ParametrizationStatus.Delete;
                result.allowEdit = false;
                result.allowDelete = false;
            }
            AllyCoverage.ReceiveCoverages(result);
            // Limpiar ListView
            $("#lvSearchAdvancedAllyCoverage").UifListView("refresh");
            dropDownSearchAdvAllyCoverage.hide();
        }
    }

    static ExecuteAdvancedSearch() {
        var Selected = $("#lvSearchAdvancedAllyCoverage").UifListView("getSelected");
        if (Selected.length > 0) {
            dropDownSearchAdvAllyCoverage.hide();
            AllyCoverageAdvancedSearch.showDataSearch(Selected[0]);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorListElement });
        }
    }

    // carga los datos de la vista de busqueda avanzada
    static showDataSearch(result) {
        if (result) {
            AllyCoverage.AllyCoverageEditSearch(result);
            AllyCoverage.setUpdateStatus;
        }
        
    }

    static LoadAllyCoverage(event, selectedItem) {
        if (selectedItem.Id == "") {
            $("#ddlAdvCoverages").prop('disabled', 'disabled');
        }
        else {
            var object_insure_id = gblDropDownListCoverageAdvSearch.find(x => x.Id === Number(selectedItem.Id)).InsuredObjectId;
            //var t = object_insure_id.foo;
            var Id_Coverage = selectedItem.Id;
            if (Id_Coverage > 0) {
                AllyCoverageAdvancedSearch.GetAllyCoverageAdvancedSearch(object_insure_id).done(function (data) {
                    if (data.result) {
                        $('#ddlAdvCoverages').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $("#ddlAdvCoverages").UifSelect('setSelected', null);
            }
        }
    }


    static LoadCoverages() {
        var dataCoverageToSearch = {
            CoverageId: $('#ddlAdvPrincipalCov').val(),
            AllyCoverageId: $("#ddlAdvCoverages").val(),
            CoveragePct: "0"
        }

        if ($("#ddlAdvPrincipalCov").UifSelect('getSelectedSource') !== undefined && $("#ddlAdvCoverages").UifSelect('getSelectedSource') !== undefined) {
            //Carga datos en el listView
            AllyCoverageAdvancedSearch.advancedSearchAllyCoverage(dataCoverageToSearch).done(function (data) {
                AllyCoverageAdvancedSearch.LoadSearchAdvAllyCoverage(data);
            });
        }
        else {
            AllyCoverageCore.GetAllyCoverageByCoverage($("#ddlAdvPrincipalCov").UifSelect('getSelectedSource')).done(function (data) {
                AllyCoverageAdvancedSearch.LoadSearchAdvAllyCoverage(data);
            });
        }     
    }

    static LoadSearchAdvAllyCoverage(data) {
        if (data.success) {
            // Limpiar ListView
            $("#lvSearchAdvancedAllyCoverage").UifListView("refresh");

            // Llenar list con los datos obtenidos
            $.each(data.result, function (key, value) {
                // Agregar objeto al ListView                
                $("#lvSearchAdvancedAllyCoverage").UifListView("addItem", value);
            });

            //Enviar a la lista principal
            AllyCoverage.ReceiveCoverages(data.result);
        } else {
            $.UifNotify("show", { 'type': "danger", 'message': n.result, 'autoclose': true });
        }
    }

    //Oculta busqueda avanzada
    static CancelSearchAdvanzed() {
        dropDownSearchAdvAllyCoverage.hide();
    }

    static advancedSearchAllyCoverage(dataCoverage) {
        //carga de todos los AllysCoverages
        return $.ajax({
            type: 'POST',
            async: false,
            url: rootPath + 'Parametrization/AllyCoverage/SearchAdvObjectsAllyCoverage',
            data: JSON.stringify({ allyC_ModelSearch: dataCoverage }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    // Muestra la vista parcial
    static ShowAdvancedSearch() {
            AllyCoverageCore.GetPrincipalCoverage().done(function (data) {
                if (data.success) {
                    $('#ddlAdvPrincipalCov').UifSelect({ sourceData: data.result });
                    gblDropDownListCoverageAdvSearch = data.result;
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
            dropDownSearchAdvAllyCoverage.show();
    }
        
    static GetAllyCoverageAdvancedSearch(object_insure_id) {
        return $.ajax({
            type: 'POST',
            async: false,
            url: rootPath + 'Parametrization/AllyCoverage/GetCoverageAllied',
            data: JSON.stringify({ id_object_insure: object_insure_id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static ClearControls() {

    }

    static GetCoveragePrincipal() {

    }

    static Getcoverage() {

    }
}