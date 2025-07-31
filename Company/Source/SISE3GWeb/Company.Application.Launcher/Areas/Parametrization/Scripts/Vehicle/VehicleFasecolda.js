
/**
 * Variables Locales y Globales
 */
var glbMakes = [];
var glbModels = [];
var glbVersions = [];
var glbVersionVehicleFascolda = [];
var ObjValues = [];

class VehicleFasecoldaRequest {

    static GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(makeId, modelId, versionId, fasecoldaMakeId, fasecoldaModelId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Vehicle/GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId, fasecoldaMakeId: fasecoldaMakeId, fasecoldaModelId: fasecoldaModelId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetVersionVehicleFasecoldaByFasecoldaId(fasecoldaId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Vehicle/GetVersionVehicleFasecoldaByFasecoldaId',
            data: JSON.stringify({ fasecoldaId: fasecoldaId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    
}

class VehicleFasecolda extends Uif2.Page {

    /**
     * Funcion para inicilizar la vista
     */
    getInitialState() {

    }

    /**
     * Eventos de los controles de la clase
     */
    bindEvents() {

    }

    static GetMakes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Vehicle/GetMakes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetModelsByMakeId(makeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Vehicle/GetModelsByMakeId',
            data: JSON.stringify({ makeId: makeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVersionsByMakeIdModelId(makeId, modelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/Vehicle/GetVersionsByMakeIdModelId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    

    static GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(makeId, modelId, versionId, fasecoldaMakeId, fasecoldaModelId) {
        VehicleFasecoldaRequest.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(makeId, modelId, versionId, fasecoldaMakeId, fasecoldaModelId).done(function (data) {
            if (data.success) {
                if (data.result.length>0) {
                    marcasList = [];
                    glbVersionVehicleFascolda = data.result;

                    //Colocar descipcion de Marca
                    if (Search) {
                        //colocar descripcion de Marca

                        $.each(glbMakes, function (index, value) {
                            $.each(glbVersionVehicleFascolda, function (key, val) {

                                if (value.Id == val.makeVehicle.Id) {
                                    val.makeVehicle.Description = value.Description;
                                }
                            });
                        });
                        
                        //Fasecolda.GetModelsByMakeId(glbVersionVehicleFascolda[0].makeVehicle.Id, 0);
                        //Fasecolda.GetVersionsByMakeIdModelId(glbVersionVehicleFascolda[0].makeVehicle.Id, glbVersionVehicleFascolda[0].modelVehicle.Id, 0);

                    }
                    else {
                        $.each(glbVersionVehicleFascolda, function (key, value) {
                            if (windowSearchAdv) {
                                value.makeVehicle.Description = $("#selectBrandVehicleAdvSearch").UifSelect('getSelectedText')
                            }
                            else {
                                value.makeVehicle.Description = $("#selectBrandVehicle").UifSelect('getSelectedText')
                            }
                        });
                    }

                    marcasList = glbVersionVehicleFascolda
                    ObjValues = marcasList;
                    //colocar descripcion de Modelos
                    $.each(glbModels, function (index, value) {
                        $.each(marcasList, function (key, val) {

                            if (value.Id == val.modelVehicle.Id) {
                                val.modelVehicle.Description = value.Description;
                            }
                        });
                    });

                    //colocar descripcion de Version
                    $.each(glbVersions, function (index, value) {
                        $.each(marcasList, function (key, val) {

                            if (value.Id == val.versionVehicle.Id) {
                                val.versionVehicle.Description = value.Description;
                            }
                        });
                    });

                    $.each(marcasList, function (index, value) {
                        value.fasecoldaCodeView = value.MakeVehicleCode + value.ModelVehicleCode;
                    });

                    if (windowSearchAdv) {
                        if (marcasList.length > 0) {
                            //Fasecolda.LoadValues();
                            Fasecolda.MapValues();
                            AdvancedSearchFasecolda.ConstructFasecoldaSearch();
                            $("#listviewSearchADvFasecolda").UifListView({
                                sourceData: marcasList,
                                displayTemplate: "#searchTemplateFasecolda",
                                selectionType: "single",
                                height: 400
                            });
                        }
                    }
                    else {
                        if (marcasList.length > 0) {
                            if (Search) {
                                Fasecolda.MapValues();
                                //colocar descripcion de Modelos
                                $.each(glbModels, function (index, value) {
                                    $.each(marcasList, function (key, val) {

                                        if (value.Id == val.modelVehicle.Id) {
                                            val.modelVehicle.Description = value.Description;
                                        }
                                    });
                                });

                                //colocar descripcion de Version
                                $.each(glbVersions, function (index, value) {
                                    $.each(marcasList, function (key, val) {

                                        if (value.Id == val.versionVehicle.Id) {
                                            val.versionVehicle.Description = value.Description;
                                        }
                                    });
                                });

                                $("#listViewFasecolda").UifListView({ sourceData: marcasList, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
                            }
                            else {
                                //Fasecolda.LoadValues();
                                Fasecolda.MapValues();
                                $("#listViewFasecolda").UifListView({ sourceData: marcasList, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
                            }
                        }
                    }
                    var prueba = true;
                    var evaluation = false;
                }
                else {  
                    if (Search) {                        
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageNoDataForTheSearch, 'autoclose': true })
                        Search = false;
                    }
                    else {
                        $('#InputBrandCodeFasecolda').prop("disabled", false);
                        $('#InputVersionCodeFasecolda').prop("disabled", false);
                        $("#listViewFasecolda").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}


