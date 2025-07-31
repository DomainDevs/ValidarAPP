$.ajaxSetup({ async: false });
var SearchCombo = {
    ListaControlSearch: function (idControl, entity, filter, level) {
        $.ajax({
            type: "POST",
            url: rootPath + "RulesScripts/DecisionTable/GetDataFromFilter",
            data: JSON.stringify({ entityId: entity, filter: null, Level: level }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            SearchCombo.ArmadoControlLits(idControl, data);
        });
    },
    ArmadoControlLits: function (idControl, data) {

        if (Object.keys(data).length !== 0 && $.parseJSON(data).length !== 0) {
            if (($.parseJSON(data)[0]).hasOwnProperty('IdEntidad')) {
                var objJson = [];
                $.each($.parseJSON(data),
                    function (i, item) {
                        var armado = {}
                        armado["Id"] = item.Id + "/" + item.IdEntidad;
                        armado["Descripción"] = item.Descripción;
                        objJson.push(armado);
                    })
                $(idControl).UifSelect({
                    sourceData: objJson,
                    id: "Id",
                    name: "Descripción",
                    filter: true,
                    native: false
                });
            } else {
                if (($.parseJSON(data)[0]).hasOwnProperty('RangeValueCode')) { //probar con carlos y jonthan
                    var objJson = [];
                    $.each($.parseJSON(data),
                        function (i, item) {
                            var armado = {}
                            armado["Id"] = item.RangeValueCode; //no se llama id seguramente da error
                            armado["Descripción"] =
                                item.FromValue + " - " + item.ToValue; //debe ser la union del from_value y to_value
                            objJson.push(armado);
                        })
                    $(idControl).UifSelect({
                        sourceData: objJson,
                        id: "Id",
                        name: "Descripción",
                        filter: true,
                        native: false
                    });
                } else if (($.parseJSON(data)[0]).hasOwnProperty('ListValueCode')) {
                    $(idControl).UifSelect({
                        sourceData: $.parseJSON(data),
                        id: "ListValueCode",
                        name: "ListValue",
                        filter: true,
                        native: false
                    });
                } else {
                    $(idControl).UifSelect({
                        sourceData: $.parseJSON(data),
                        id: "Id",
                        name: "Descripción",
                        filter: true,
                        native: false
                    });
                }
            }
        } else {
            $(idControl).UifSelect({
                sourceData: $.parseJSON(data),
                id: "Id",
                name: "Descripción",
                filter: true,
                native: false
            });
        }
    },
    obtencionTipoControl: async function (concept, entity, idControl) {
        var value = null;
        await $.ajax({
            type: "POST",
            url: rootPath + "RulesScripts/RuleSet/GetConceptControl",
            data: JSON.stringify({ entityId: entity, conceptId: concept }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (idControl != '') {
                    switch (data.result.ConceptControlCode) {
                        case 3:
                            if (idControl !== "#selectAnswer") {
                                $(idControl).UifDatepicker();
                            }

                            break;
                        case 4://Ubicación - ¿El riesgo es principal?   
                            if (data.result.ListListEntityValues != undefined) {
                                SearchCombo.ArmadoControlLits(idControl, JSON.stringify(data.result.ListListEntityValues));
                            }
                            else if (data.result.ListRangeEntityValues != undefined) {
                                SearchCombo.ArmadoControlLits(idControl, JSON.stringify(data.result.ListRangeEntityValues));
                            }

                            break;
                        case 5://Ramo comercial                    
                            SearchCombo.ListaControlSearch(idControl, data.result.ForeignEntity, null, null);
                            break;
                        default:
                            //1://Ubicación - Calle del riesgo // TextBox
                            //2://Prima Tecnica // Numero                            
                            //4://Ubicación - ¿El riesgo es principal? // List
                            //5://Ramo comercial // Referencia
                            //5://Ramo comercial // RichText 83,261
                            break;
                    }
                }

                value = data.result;
            }
        });

        return value;
    },
    castearControl: function (conceptId, entityId, valor) {
        return SearchCombo.obtencionTipoControl(conceptId, entityId, '').then(data => {
            switch (data.ConceptControlCode) {
                case 1://text          
                    break;
                case 2://numero
                    if (data.BasicType == 3) {//decimal
                        //preguntar y probar 
                        valor = parseFloat(valor);
                    }
                    else {
                        valor = Number(valor);
                    }
                    break;
                case 3://fecha
                    break;
                case 4://lista
                    if (data.ListEntityCode == 2) {
                        valor = Boolean(valor == 1);
                    }
                    else {
                        valor = Number(valor);
                    }
                    break;
                case 5://referencia
                    valor = Number(valor);
                    break;
                default:
                    null
                    break;
            }
            return {
                "Id": conceptId, "EntityId": entityId, "Value": valor
            };
        });
    },
    ComparatorControlGet: function (objCondicionTemp) {
        var valor = null;
        switch (objCondicionTemp.Comparator.ComparatorCode) {
            case 6:
                if (objCondicionTemp.Value == null && objCondicionTemp.ConceptValue == null) {
                    valor = 4;
                    break;
                }
                else {
                    valor = 2;
                    break;
                }
            case 7:
                if (objCondicionTemp.Value == null && objCondicionTemp.ConceptValue == null) {
                    valor = 3;
                    break;
                }
                else {
                    valor = 1;
                    break;
                }
            case 13:
                valor = 13;//6;
                break;
            case 14:
                valor = 14;//8;
                break;
            case 15:
                valor = 15;//5
                break;
            case 16:
                valor = 16;//7
                break;
            default:
        }
        return valor
    },
    ComparatorControlSet: function (ComparatorCode) {
        var valor = null;
        switch (ComparatorCode) {
            case 1:
            case 3:
                valor = 7;//6;
                break;
            case 2:
            case 4:
                valor = 6;//6;
                break;
            case 13:
                valor = 13;//6;
                break;
            case 14:
                valor = 14;//8;
                break;
            case 15:
                valor = 15;//5
                break;
            case 16:
                valor = 16;//7
                break;
            default:
        }
        return valor
    },
    GetIndexObjects: function (obj, key, val) {
        var contIndex = -1;
        var objIndex = -1;
        var newObj = false;
        $.each(obj, function () {
            contIndex++;
            var testObject = this;
            $.each(testObject, function (k, v) {
                if (val == v && k == key) {
                    newObj = testObject;
                    objIndex = contIndex;
                }
            });
        });
        return objIndex;
    },
    GetIdMayor: function (obj, key) {
        var Ids = [];
        $.each(obj, function () {
            $.each(this, function (k, v) {
                if (k == key) {
                    Ids.push(v);
                }
            });
        });

        Ids.sort(function (a, b) { return b - a });

        return Ids[0];
    },
    GetObjects: function (obj, key, val) {
        var contIndex = -1;
        var objIndex = -1;
        var newObj = false;
        $.each(obj, function () {
            contIndex++;

            var testObject = this;
            $.each(testObject, function (k, v) {
                if (val == v && k == key) {
                    newObj = testObject;
                    objIndex = contIndex;
                }
            });
        });
        //return newObj;
        return objIndex;
    },
    GetIndexObjectsbyTwoKey: function (obj, key, val, key2, val2) {
        var contIndex = -1;
        var objIndex = -1;

        $.each(obj, function () {
            contIndex++;
            var testObject = this;

            var existe = false;
            $.each(testObject, function (k, v) {
                if (val == v && k == key) {
                    existe = true;
                }
            })

            $.each(testObject, function (k, v) {
                if (val2 == v && k == key2 && existe) {
                    objIndex = contIndex;
                }
            })
        })
        return objIndex;
    },
}












