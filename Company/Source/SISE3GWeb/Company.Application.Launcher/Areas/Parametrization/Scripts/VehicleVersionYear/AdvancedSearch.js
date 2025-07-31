//$(() => {
//    new VehicleVersionYearParametrizationAdv();
//});

var dropDownSearchAdv;
var vehicleObj = {};

class VehicleVersionYearParametrizationAdv extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: true });
        dropDownSearchAdv = uif2.dropDown({
            source: rootPath + 'Parametrization/VehicleVersionYear/AdvancedSearch',
            element: '#btnShowSearchAdv',
            align: 'right',
            width: 550,
            height: 551,
            container: "#main",
            loadedCallback: VehicleVersionYearParametrizationAdv.componentLoadedCallback
        });
        
    }

    bindEvents() {
    }

    static showDropDown() {
        dropDownSearchAdv.show();
    }

    static componentLoadedCallback() {        
        $("#FasecoldaCode").ValidatorKey(ValidatorType.Number, 0, 0);
        VehicleVersionYearParametrizationAdv.setListAdv(null);
        request('Parametrization/VehicleVersionYear/GetMakes', null, 'GET', AppResources.ErrorGetMakesCONNEX, VehicleVersionYearParametrizationAdv.setMakes);
        $("#MakeIdAdv").on("itemSelected", VehicleVersionYearParametrizationAdv.changeMake);
        $("#ModelIdAdv").on("itemSelected", VehicleVersionYearParametrizationAdv.changeModel);
        $("#VersionIdAdv").on("itemSelected", VehicleVersionYearParametrizationAdv.changeVersion);
        $("#btnSearchAdv").on("click", VehicleVersionYearParametrizationAdv.searchAdv);
        $("#btnAcceptSearchAdv").on("click", VehicleVersionYearParametrizationAdv.setVehicle);
        $("#btnCancelSearchAdv").on("click", VehicleVersionYearParametrizationAdv.btnClose);
    }

    static setListAdv(data) {
        $("#lsvSearchAdvanced").UifListView({
            displayTemplate: "#searchAdvTemplate",
            selectionType: 'single',
            sourceData: data,
            height: 240
        });
    }

    static setMakes(data) {
        $("#MakeIdAdv").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
    }

    static setModels(data) {
        $('#FasecoldaCode').val(null);
        $("#ModelIdAdv").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
    }

    static setVersions(data) {
        $("#VersionIdAdv").UifSelect({ sourceData: data.items, selectedId: data.selectedId });
    }

    static changeMake(event, selectedItem) {
        $("#formSearchAdv").valid();
        VehicleVersionYearParametrizationAdv.setVersions({ items: null }); //Siempre debe limipar la version, sin importar cual marca se selecciono
        if (selectedItem.Id > 0) {
            request('Parametrization/VehicleVersionYear/GetModelsByMakeId', JSON.stringify({ makeId: selectedItem.Id }), 'POST', AppResources.ErrorGetModelsCONNEX, VehicleVersionYearParametrizationAdv.setModels)
        }
        else {
            VehicleVersionYearParametrizationAdv.setModels({ items: null });
        }
    }

    static changeModel(event, selectedItem) {
        $("#formSearchAdv").valid();
        var selectedItemMake = $("#MakeIdAdv").UifSelect("getSelected");
        if (selectedItem.Id > 0 && selectedItemMake > 0) {
            request('Parametrization/VehicleVersionYear/GetVersionsByMakeIdModelId', JSON.stringify({ makeId: selectedItemMake, modelId: selectedItem.Id }), 'POST', AppResources.ErrorGetVersionsCONNEX, VehicleVersionYearParametrizationAdv.setVersions)
        }
        else {
            VehicleVersionYearParametrizationAdv.setVersions({ items: null });
        }
    }

    static changeVersion(event, selectedItem) {
        $("#formSearchAdv").valid();        
    }
    

    static searchAdv() {
        if ($("#FasecoldaCode").val() > 0) {
            var inputSearch = $("#FasecoldaCode").val();

            if (inputSearch.length < 3) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageInfoMinimumChar, 'autoclose': true })
            }
            else if (inputSearch.length > 8) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageInfoMaximumEightChar, 'autoclose': true })
            }
            else {

                //$('#FasecoldaCode').val(null);
                $("#lsvSearchAdvanced").UifListView({
                    source: null,
                    displayTemplate: "#searchAdvTemplate",
                    selectionType: 'single',
                    height: 240
                });

                var vehicle =
                    {
                        CurrencyId: 0,
                        Id: 0,
                        MakeId: 0,
                        ModelId: 0,
                        Price: 0,
                        Status: 0,
                        VersionId: 0,
                        Year: 0
                    }

                

                VehicleVersionYearParametrizationAdv.GetVersionVehicleFasecoldaByFasecoldaId(inputSearch).done(function (data) {
                    if (data.success) {
                        if (data.result.length > 0) {
                            VehicleFasecolda.GetMakes().done(function (dataMakes) {
                                if (dataMakes.success) {
                                    glbMakes = dataMakes.result;
                                    //$("#MakeIdAdv").UifSelect({ sourceData: dataMakes.result, selectedId: data.result[0].makeVehicle.Id });
                                    vehicle.MakeId = data.result[0].makeVehicle.Id;

                                    VehicleVersionYearParametrizationAdv.GetModelsByMakeId(data.result[0].makeVehicle.Id).done(function (dataModels) {
                                        if (dataModels.success) {
                                            glbModels = dataModels.result;
                                            //$("#ModelIdAdv").UifSelect({ sourceData: dataModels.result, selectedId: data.result[0].modelVehicle.Id });

                                            vehicle.ModelId = data.result[0].modelVehicle.Id;

                                            VehicleVersionYearParametrizationAdv.GetVersionsByMakeIdModelId(data.result[0].makeVehicle.Id, data.result[0].modelVehicle.Id).done(function (dataVersion) {
                                                if (dataVersion.success) {
                                                    glbVersions = dataVersion.result;
                                                    //$("#VersionIdAdv").UifSelect({ sourceData: dataVersion.result, selectedId: data.result[0].versionVehicle.Id });

                                                    vehicle.VersionId = data.result[0].versionVehicle.Id;
                                                    vehicleObj = vehicle;
                                                    if (vehicleObj.Id != undefined) {
                                                        request('Parametrization/VehicleVersionYear/GetVehicleVersionYearServiceModel', JSON.stringify({ vehicle: vehicleObj }), 'POST', AppResources.ErrorGetVehicleRelationCONNEX, VehicleVersionYearParametrizationAdv.setListVehicles)
                                                        vehicleObj = {};
                                                    }

                                                    marcasList = data.result;

                                                }
                                                else {
                                                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                                }
                                            });
                                        }
                                        else {
                                            $.UifNotify('show', { 'type': 'info', 'message': dataModels.result, 'autoclose': true });
                                        }
                                    });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'info', 'message': dataMakes.result, 'autoclose': true });
                                }
                            });
                        } else {
                            VehicleVersionYearParametrizationAdv.clear();
                            $('#inputFasecoldaCode').val(null);
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchVehicleFasecolda, 'autoclose': true })
                        }
                    }
                });


            }       
            //$.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorNotSelectedRegisterDelete, 'autoclose': true });
        }
        else {
            $("#FasecoldaCode").val(0);
            if ($("#formSearchAdv").valid())
                request('Parametrization/VehicleVersionYear/GetVehicleVersionYearServiceModel', JSON.stringify({ vehicle: $("#formSearchAdv").serializeObject() }), 'POST', AppResources.ErrorGetVehicleRelationCONNEX, VehicleVersionYearParametrizationAdv.setListVehicles)
        }        
    }

    static SearchFasecolda() {
        var inputSearch = $("#FasecoldaCode").val();

        if (inputSearch.length < 3) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageInfoMinimumChar, 'autoclose': true })
        }
        else if (inputSearch.length > 8) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.MessageInfoMaximumEightChar, 'autoclose': true })
        }
        else {

            $('#FasecoldaCode').val(null);
            $("#lsvSearchAdvanced").UifListView({
                source: null,
                displayTemplate: "#searchAdvTemplate",
                selectionType: 'single',
                height: 240
            });

            var vehicle =
                {
                    CurrencyId: 0,
                    Id: 0,
                    MakeId: 0,
                    ModelId: 0,
                    Price: 0,
                    Status: 0,
                    VersionId: 0,
                    Year: 0
                }

            var inNakeFasecoldaId = inputSearch.substr(0, 3)
            var inModelFasecoldaId = inputSearch.substr(3, inputSearch.length)

            VehicleVersionYearParametrizationAdv.GetVersionVehicleFasecoldaByFasecoldaId(inputSearch).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        VehicleFasecolda.GetMakes().done(function (dataMakes) {
                            if (dataMakes.success) {
                                glbMakes = dataMakes.result;
                                //$("#MakeIdAdv").UifSelect({ sourceData: dataMakes.result, selectedId: data.result[0].makeVehicle.Id });
                                vehicle.MakeId = data.result[0].makeVehicle.Id;

                                VehicleVersionYearParametrizationAdv.GetModelsByMakeId(data.result[0].makeVehicle.Id).done(function (dataModels) {
                                    if (dataModels.success) {
                                        glbModels = dataModels.result;
                                        //$("#ModelIdAdv").UifSelect({ sourceData: dataModels.result, selectedId: data.result[0].modelVehicle.Id });

                                        vehicle.ModelId = data.result[0].modelVehicle.Id;

                                        VehicleVersionYearParametrizationAdv.GetVersionsByMakeIdModelId(data.result[0].makeVehicle.Id, data.result[0].modelVehicle.Id).done(function (dataVersion) {
                                            if (dataVersion.success) {
                                                glbVersions = dataVersion.result;
                                                //$("#VersionIdAdv").UifSelect({ sourceData: dataVersion.result, selectedId: data.result[0].versionVehicle.Id });

                                                vehicle.VersionId = data.result[0].versionVehicle.Id;
                                                vehicleObj = vehicle;
                                                //$.each(data.result, function (key, vehicleFasecolda) {
                                                //    $.each(glbMakes, function (key, make) {
                                                //        if (make.Id == vehicleFasecolda.makeVehicle.Id) {
                                                //            vehicleFasecolda.makeVehicle.Description = make.Description;
                                                //        }
                                                //    });

                                                //    $.each(glbModels, function (key, model) {
                                                //        if (model.Id == vehicleFasecolda.modelVehicle.Id) {
                                                //            vehicleFasecolda.modelVehicle.Description = model.Description;
                                                //        }
                                                //    });
                                                //    $.each(glbVersions, function (key, version) {
                                                //        if (version.Id == vehicleFasecolda.versionVehicle.Id) {
                                                //            vehicleFasecolda.versionVehicle.Description = version.Description;
                                                //        }
                                                //    });
                                                //});
                                                marcasList = data.result;
                                                //$("#InputBrandCodeFasecolda").val(data.result[0].MakeVehicleCode);
                                                //$("#InputVersionCodeFasecolda").val(data.result[0].ModelVehicleCode);
                                                //$('#InputBrandCodeFasecolda').prop("disabled", false);
                                                //$('#InputVersionCodeFasecolda').prop("disabled", false);
                                                //$("#selectBrandVehicle").prop("disabled", true);
                                                //$("#selectModelVehicle").prop("disabled", true);
                                                //$("#selectVersionVehicle").prop("disabled", true);
                                                //$('#inputFasecoldaCode').val(null);

                                                //$("#listViewFasecolda").UifListView({ sourceData: marcasList, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Fasecolda.deleteFasecoldaCallback, displayTemplate: "#FasecoldaTemplate", selectionType: 'single', height: 310 });
                                            }
                                            else {
                                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                            }
                                        });
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': dataModels.result, 'autoclose': true });
                                    }
                                });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': dataMakes.result, 'autoclose': true });
                            }
                        });
                    } else {
                        Fasecolda.Clear();
                        $('#inputFasecoldaCode').val(null);
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchVehicleFasecolda, 'autoclose': true })
                    }
                }
            });


        }
    }

    static setListVehicles(data) {
        VehicleVersionYearParametrizationAdv.setListAdv(data);
    }

    static setVehicle() {
        var result = $("#lsvSearchAdvanced").UifListView("getSelected");
        if (result.length === 1) {
            VehicleVersionYearParametrization.clear();
            VehicleVersionYearParametrization.setVehicleSelect(result);
            VehicleVersionYearParametrization.setVehicleWhithoutSelect(result);
        }
        VehicleVersionYearParametrizationAdv.btnClose();
    }

    static clear() {
        VehicleVersionYearParametrizationAdv.setListAdv(null);
        $("#MakeIdAdv").UifSelect("setSelected", null);
        $('#FasecoldaCode').val(null);
        VehicleVersionYearParametrizationAdv.setModels({ items: null });
        VehicleVersionYearParametrizationAdv.setVersions({ items: null });
        ClearValidation("#formSearchAdv");
    }

    static btnClose() {
        VehicleVersionYearParametrizationAdv.clear();
        dropDownSearchAdv.hide();
    }

    static GetVersionVehicleFasecoldaByFasecoldaId(fasecoldaId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Vehicle/GetVersionVehicleFasecoldaByFasecoldaId',
            data: JSON.stringify({ fasecoldaId: fasecoldaId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
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

}