$.ajaxSetup({ async: false });
var modalListType = 0;
var heightAgent = 400;
var AgentRowId = -1;
var Agent = [];
var AgentDeleted = [];
var agentSearchType;

$(document).ready(function () {
    enabledDisabledControl(true);
    EventsAgent();
});

function EventsAgent() {
    //$("#inputAgentPrincipal").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    //$("#inputAgentPrincipal").mask(AgentPrincipalOnlyNumberLetter());
    //Agregar agentes
    //$("#btnSaveProductAgent").on("click", function () {
    //    if (ValidateAgents()) {
    //        enabledDisabledControl(true);
    //        SaveAgents();
    //        AgentRowId = -1;
    //    }
    //});
    //Cargar agente
    $("#inputAgentPrincipal").on('buttonClick', function () {
        agentSearchType = 1;
        GetAgenciesByAgentIdDescription(0, $("#inputAgentPrincipal").val().trim(), $("#selectPrefixCommercial").UifSelect("getSelected"));
    });
    //Grabar Agentes
    $("#btnSaveProductAgent").on("click", function () {
        if ($("#inputAgentPrincipal").data("Object") != null) {
            if ($("#inputAgentPrincipal").data("Object").StatusTypeService == 2 && !$("#chkAgentAssigned").is(':checked')) {
                $.UifNotify('show', { 'type': 'danger', 'message': "Seleccionar asignado", 'autoclose': true }) //Resources.Language.ErrorSaveProduct
                return;
            }

            if ($("#inputAgentPrincipal").data("Object").StatusTypeService != 1) {
                $('#btnSaveProductAgent').prop("disabled", true);
                //var Agents = $('#TableModalAgentsData').UifDataTable('getData').filter(function (item) {
                //    return item.StatusTypeService !== 1;
                //});
                //if (AgentDeleted.length > 0) {
                //    $.each(AgentDeleted, function (index, item) {
                //        item.StatusTypeService = 4;
                //        Agents.push(item);
                //    });
                //}
                var Agents = [];
                Agents.push($("#inputAgentPrincipal").data("Object"));
                if (Agents.length > 0) {
                    //$.each(Agents, function (index, item) {
                    //	delete item.CommisionText;
                    //             delete item.DataItem;
                    //             delete item.LockerId;
                    //	delete item.ErrorServiceModel;
                    //	delete item.FullName;
                    //});
                    var agentData = JSON.parse(JSON.stringify(Agents));

                    AgentRequestT.SaveAgents(agentData, Product.Id).done(function (data) {
                        $('#btnSaveProductAgent').prop("disabled", false);
                        if (data.success) {
                            //ObjectProduct.fillViewMainProduct(data.result);
                            //AgentRowId = -1;
                            Agent = [];
                            AgentDeleted = [];
                            //CurrentWorkAgents = [];
                            ObjectProduct.getProductAgents();
                            ClearAgent();
                            //ShowPanelsProduct(MenuProductType.Product);
                            Product.pendingChanges = false;

                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.AgentUpdatedCorrectly, 'autoclose': true });

                            //DisabledProduc(false);
                            //ValidateDateDisableProduct();
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $('#btnModalAgentsSave').prop("disabled", false);
                        resultOperation = false;
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveAgent, 'autoclose': true })
                    });
                   
                }
                else {
                    $('#btnModalAgentsSave').prop("disabled", false);
                }
            }
        }
        //AgentRowId = -1;
        //Agent = [];
        //AgentDeleted = [];
        //CurrentWorkAgents = [];
        //ClearAgent();        
        //ShowPanelsProduct(MenuProductType.Product);
        //Product.pendingChanges = false;
    });

    $("#btnModalAgentsClose").on("click", function () {
        AgentRowId = -1;
        Agent = [];
        AgentDeleted = [];
        //CurrentWorkAgents = [];
        ShowPanelsProduct(MenuProductType.Product);
    });
    //Desasignar todos
    $("#btnModalAgentsDeallocateAll").on("click", function () {
        $.UifDialog(
            'confirm',
            { 'message': 'Desea Confirmar?', 'title': 'Está realmente seguro' },
            function (result) {
                if (result) {
                    $("#DivPreloader").show();
                    window.setTimeout(function () {
                        SaveAllAgents(0);
                        //AgentDeleted = [];
                        //AgentDeleted.push({
                        //    IndividualId: 0,
                        //    StatusTypeService: 4,   
                        //    ProductId: 0
                        //});
                        ////$.each($('#TableModalAgentsData').DataTable().rows().data(), function (index, value) {
                        ////    if (value.StatusTypeService == 1 || value.StatusTypeService == 3) {
                        ////        AgentDeleted.push(this);
                        ////    }
                        ////});
                        //$('#TableModalAgentsData').DataTable().clear();
                        //$('#TableModalAgentsData').DataTable().draw();
                        //ClearAgent();
                        //$("#DivPreloader").hide();
                    }, 1)
                    //$.UifNotify('show', { 'type': 'info', 'message': 'El resultado del dialogo fue: ', 'autoclose': true });
                }
        });
    });
    //Asignar todos
    $("#btnModalAgentsAssignAll").on("click", function () {
        $("#DivPreloader").show();
        window.setTimeout(function () {
            SaveAllAgents(1);
            //GetAgents().done(function (responseAgents) {
            //    if (responseAgents.length > 0) {
            //        //if (AgentDeleted.length > 0) {
            //        //    $.each(responseAgents, function (index, itemResponse) {
            //        //        var ifExist = AgentDeleted.filter(function (item) {
            //        //            return item.IndividualId
            //        //                == itemResponse.IndividualId;
            //        //        });
            //        //        if (ifExist.length > 0) {
            //        //            itemResponse = ifExist[0];
            //        //            itemResponse.StatusTypeService = 3;
            //        //        }
            //        //        if (itemResponse.AgencyComiss != "undefined" && itemResponse.AgencyComiss !== null) {
            //        //            if (itemResponse.AgencyComiss.length > 0) {
            //        //                $.each(itemResponse.AgencyComiss, function (indexComiss, itemComiss) {
            //        //                    if (itemComiss.StatusTypeService !== 2) {
            //        //                        itemComiss.StatusTypeService = 4;
            //        //                    }
            //        //                    else {
            //        //                        itemResponse.AgencyComiss.splice(indexComiss, 1);
            //        //                    }
            //        //                });
            //        //            }
            //        //        }
            //        //        if (itemResponse.Incentives != "undefined" && itemResponse.Incentives !== null) {
            //        //            if (itemResponse.Incentives.length > 0) {
            //        //                $.each(itemResponse.Incentives, function (indexIncentive, itemIncentives) {
            //        //                    if (itemIncentives.StatusTypeService !== 2) {
            //        //                        itemIncentives.StatusTypeService = 4;
            //        //                    }
            //        //                    else {
            //        //                        itemResponse.Incentives.splice(indexIncentive, 1);
            //        //                    }                                        
            //        //                });
            //        //            }
            //        //        }
            //        //    });
            //        //    $.each(AgentDeleted, function (indexAgentDelete, itemAgentDelete) {
            //        //        var ifExist = responseAgents.filter(function (item) {
            //        //            return item.IndividualId
            //        //                == itemAgentDelete.IndividualId && item.StatusTypeService !==2;
            //        //        });
            //        //        if (ifExist.length > 0) {
            //        //            AgentDeleted.splice(indexAgentDelete, 1);
            //        //        }
            //        //    });
            //        //}
            //        $('#TableModalAgentsData').DataTable().rows.add(responseAgents);
            //        $('#TableModalAgentsData').DataTable().draw();
            //    }
            //    else {
            //        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGettingAgents, 'autoclose': true });
            //    }
            //});
            //ClearAgent();
            //$("#DivPreloader").hide();
        }, 1)
    });
    $("#listModalAgentsData").on("rowEdit", function (event, data, position) {
        AgentRowId = position;
        EnabledDisabledAgent(true);
        $("#inputAgentPrincipal").data("Object", data);
        $("#inputAgentPrincipal").val(data.FullName);
        enabledDisabledControl(false);
    });
    $("#btnCancelProductAgent").on("click", function () {
        ClearAgent();
        EnabledDisabledAgent(false);
        enabledDisabledControl(true);
    });
    $("#TableModalAgentsData tbody").on("click", "tr .card-button.delete-button", DeleteItemResolve);
    $("#TableModalAgentsData tbody").on("click", "tr .card-button.edit-button", EditItemTable);

    $("#chkAgentAssigned").on("click", function () {
        if ($("#inputAgentPrincipal").data("Object") != null) {
            if ($("#chkAgentAssigned").is(':checked')) {
                EnabledDisabledAgent(true);
                enabledDisabledControl(false);
                if ($("#inputAgentPrincipal").data("Object").StatusTypeService == 4) {
                    $("#inputAgentPrincipal").data("Object").StatusTypeService = 3;
                }
            } else {
                if ($("#inputAgentPrincipal").data("Object").StatusTypeService != 2) {
                    $("#inputAgentPrincipal").data("Object").StatusTypeService = 4;
                }
                //EnabledDisabledAgent(false);
                enabledDisabledControl(true);
            }
        }
    }); 
}

function AgentPrincipalOnlyNumberLetter() {
    var maxLength = '';
    for (var i = 0; i < 200; i++) {
        maxLength = maxLength + 'A';
    }
    for (var i = 0; i < 200; i++) {
        maxLength = maxLength + '0';
    }
    return maxLength;
}

function DeleteItemResolve() {
    if ($("#TableModalAgentsData tbody").find(".item.active").length > 0) {
    } else {
        $(this).parent().parent().parent().addClass("active");
        $('<div class="delete"><div class="alert alert-danger alert-dismissible error" role="alert">¿ Esta seguro que quiere eliminar el registro ?  <a onclick="DeleteItem(' + $('#TableModalAgentsData').DataTable().row($(this).parents('tr')).index() + ')">Eliminar</a> <span class="separator">|</span><a onclick="RemoveDelete()">Cancelar</a></div></div>').insertAfter($(this).parent().parent());
    }
}
function EditItemTable() {
    AgentRowId = $('#TableModalAgentsData').DataTable().row($(this).parents('tr')).index();
    EnabledDisabledAgent(true);
    $("#inputAgentPrincipal").data("Object", $('#TableModalAgentsData').DataTable().row($(this).parents('tr')).data());
    $("#inputAgentPrincipal").val($('#TableModalAgentsData').DataTable().row($(this).parents('tr')).data().FullName);

    if ($("#inputAgentPrincipal").data("Object").FullName != "") {
        $("#inputAgenciesAgentDescriptionIA").val($("#inputAgentPrincipal").data("Object").FullName);
    }
    AgentInfo = $("#inputAgentPrincipal").data("Object");
    $("#inputAgenciesAgentDescriptionIA").data("IndividualId", $("#inputAgentPrincipal").data("Object").IndividualId);
    IncentivesForAgents.GetAgenciesByAgentId($("#inputAgentPrincipal").data("Object").IndividualId);
    IndexAgentIA = AgentRowId;
    enabledDisabledControl(false);
}
function DeleteItem(index) {
    var currentItem = $('#TableModalAgentsData').DataTable().row(index).data();
    if (currentItem.StatusTypeService != 2) {
        currentItem.StatusTypeService = 4;
        AgentDeleted.push($('#TableModalAgentsData').DataTable().row(index).data());
    }
    $("#TableModalAgentsData").UifDataTable('deleteRow', index);
    ClearAgent();
}
function RemoveDelete() {
    $("#TableModalAgentsData .delete").remove();
    $("#TableModalAgentsData tbody").find(".item.active").removeClass("active");
}
function SaveAgents() {
    if (AgentRowId == -1) {
        var list = $('#TableModalAgentsData').DataTable().rows().data();

        var ifExist = list.filter(function (item) {
            return item.IndividualId
                == $("#inputAgentPrincipal").data("Object").IndividualId;
        });
        if (ifExist.length > 0) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorExistAgent, 'autoclose': true
            });
        }
        else {
            $("#inputAgentPrincipal").data("Object").StatusTypeService = 2;
            $("#inputAgentPrincipal").data("Object").ProductId = Product.Id;
            if ($("#inputAgentPrincipal").data("Object").AgencyComiss != null && $("#inputAgentPrincipal").data("Object").AgencyComiss.length > 0) {
                $("#inputAgentPrincipal").data("Object").CommisionText = Resources.Language.labelDifferentialCommission;
            }
            else {
                $("#inputAgentPrincipal").data("Object").CommisionText = "";
            }

            $("#inputAgentPrincipal").data("Object").DataItem = '<div class="item"><div class="display columns"><div class="template">' +
                '<div class="success"><strong>' + $("#inputAgentPrincipal").data("Object").FullName + '</strong></div>' +
                '<div><strong>' + $("#inputAgentPrincipal").data("Object").CommisionText + '</strong></div>' +
                '<div class="hidden">' + $("#inputAgentPrincipal").data("Object").StatusTypeService + '</div>' +
                '</div><div class="toolbar buttons"><div class="card-button edit-button" onclick="return false"><i class="fa fa-pencil"></i></div><div class="card-button delete-button" onclick="return false"><i class="fa fa-trash"></i></div></div></div></div>';
            $("#TableModalAgentsData").UifDataTable('addRow', $("#inputAgentPrincipal").data("Object"));
        }
    }
    else {
        if ($("#inputAgentPrincipal").data("Object").StatusTypeService == 1 || $("#inputAgentPrincipal").data("Object").StatusTypeService == 3) {
            if ($("#inputAgentPrincipal").data("Object").AgencyComiss != null && $("#inputAgentPrincipal").data("Object").AgencyComiss.length > 0) {
                var comissList = $("#inputAgentPrincipal").data("Object").AgencyComiss.filter(function (itemListNew) {
                    return itemListNew.StatusTypeService
                        == 1 || itemListNew.StatusTypeService == 2 || itemListNew.StatusTypeService == 3;
                });
                if (comissList.length > 0) {
                    $("#inputAgentPrincipal").data("Object").CommisionText = Resources.Language.labelDifferentialCommission;
                }
                else {
                    $("#inputAgentPrincipal").data("Object").CommisionText = "";
                }

            }
            else {
                $("#inputAgentPrincipal").data("Object").CommisionText = "";
            }
            $("#inputAgentPrincipal").data("Object").StatusTypeService = 3;
            $("#inputAgentPrincipal").data("Object").DataItem = '<div class="item"><div class="display columns"><div class="template">' +
                '<div class="warning"><strong>' + $("#inputAgentPrincipal").data("Object").FullName + '</strong></div>' +
                '<div><strong>' + $("#inputAgentPrincipal").data("Object").CommisionText + '</strong></div>' +
                '<div class="hidden">' + $("#inputAgentPrincipal").data("Object").StatusTypeService + '</div>' +
                '</div><div class="toolbar buttons"><div class="card-button edit-button" onclick="return false"><i class="fa fa-pencil"></i></div><div class="card-button delete-button" onclick="return false"><i class="fa fa-trash"></i></div></div></div></div>';
        } else {
            if ($("#inputAgentPrincipal").data("Object").AgencyComiss != null && $("#inputAgentPrincipal").data("Object").AgencyComiss.length > 0) {
                $("#inputAgentPrincipal").data("Object").CommisionText = Resources.Language.labelDifferentialCommission;
            }
            else {
                $("#inputAgentPrincipal").data("Object").CommisionText = "";
            }
            $("#inputAgentPrincipal").data("Object").StatusTypeService = 2;
            $("#inputAgentPrincipal").data("Object").DataItem = '<div class="item"><div class="display columns"><div class="template">' +
                '<div class="success"><strong>' + $("#inputAgentPrincipal").data("Object").FullName + '</strong></div>' +
                '<div><strong>' + $("#inputAgentPrincipal").data("Object").CommisionText + '</strong></div>' +
                '<div class="hidden">' + $("#inputAgentPrincipal").data("Object").StatusTypeService + '</div>' +
                '</div><div class="toolbar buttons"><div class="card-button edit-button" onclick="return false"><i class="fa fa-pencil"></i></div><div class="card-button delete-button" onclick="return false"><i class="fa fa-trash"></i></div></div></div></div>';
        }
        $("#TableModalAgentsData").UifDataTable('editRow', $("#inputAgentPrincipal").data("Object"), AgentRowId);
    }
    ClearAgent();
}
function GetAgents() {
    var dataRows = [];
    $.each($('#TableModalAgentsData').DataTable().rows().data(), function (index, value) {
        dataRows.push(value.IndividualId);
    });

    return AgentRequestT.GetAgents(Product.Id, dataRows, $("#selectPrefixCommercial").UifSelect("getSelected"));
    
}

function GetAgenciesByAgentCodeFullName(description) {

    var number = parseInt(description, 10);
    if (!isNaN(number) || description.length > 2) {

        AgentRequestT.GetAgenciesByAgentCodeFullName(description).done(function (data) {
            if (data.success) {
                if (data.result.length == 1) {
                    $("#inputAgentPrincipal").data("Object", data.result[0]);
                    $("#inputAgentPrincipal").val(data.result[0].FullName);
                }
                else if (data.result.length > 1) {
                    modalListType = 1;
                    var dataList = { dataObject: [] };
                    for (var i = 0; i < data.result.length; i++) {
                        dataList.dataObject.push({
                            Id: data.result[i].IndividualId,
                            Code: data.result[i].IndividualId,
                            Description: data.result[i].FullName
                        });
                    }
                    ShowModalList(dataList.dataObject);
                    $('#modalDialogList').UifModal('showLocal', Resources.Language.SelectMainAgent);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNotAgents, 'autoclose': true })
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAgents, 'autoclose': true })
        });
    }
}
function GetAgenciesByAgentIdDescription(agentId, description, prefixId) {
    var Agents = $('#TableModalAgentsData').DataTable().rows().data();
    var number = parseInt(description, 10);
    if ((!isNaN(number) || description.length > 2) && (description != 0)) {
        
        AgentRequestT.GetAgenciesByAgentIdDescription(agentId, description, prefixId).done(function (data) {
            if (data.success) {
                if (data.result.length == 1) {
                    $("#inputAgentPrincipal").data("Object", data.result[0]);
                    $("#inputAgentPrincipal").data("Object").IndividualId = data.result[0].IndividualId;
                    $("#inputAgentPrincipal").data("Object").FullName = data.result[0].FullName;
                    $("#inputAgentPrincipal").val(data.result[0].FullName);
                    if (Agents.length > 0) {
                        $("#TableModalAgentsData_filter").find(".form-control.input-sm").val(data.result[0].FullName);
                        $("#TableModalAgentsData_filter").find(".form-control.input-sm").keyup();
                    }
                    AgentRequestT.GetProductAgentByProductIdByIndividualId(Product.Id, data.result[0].IndividualId).done(function (dataR) {
                        if (dataR.success) {
                            if (dataR.result.length == 1) {
                                $("#inputAgentPrincipal").data("Object", dataR.result[0]);
                                $("#inputAgentPrincipal").data("Object").IndividualId = dataR.result[0].IndividualId;
                                $("#inputAgentPrincipal").data("Object").FullName = dataR.result[0].FullName;
                                $("#inputAgentPrincipal").val(dataR.result[0].FullName);
                                $('#chkAgentAssigned').prop('checked', true);

                                EnabledDisabledAgent(true);
                                enabledDisabledControl(false);
                            } else {
                                EnabledDisabledAgent(false);
                                enabledDisabledControl(true);
                            }
                        }
                    });

                    if ($("#inputAgentPrincipal").data("Object").FullName != "") {
                        $("#inputAgenciesAgentDescriptionIA").val($("#inputAgentPrincipal").data("Object").FullName);
                    }
                    AgentInfo = $("#inputAgentPrincipal").data("Object");
                    $("#inputAgenciesAgentDescriptionIA").data("IndividualId", $("#inputAgentPrincipal").data("Object").IndividualId);
                    IncentivesForAgents.GetAgenciesByAgentId($("#inputAgentPrincipal").data("Object").IndividualId);
                    //IndexAgentIA = AgentRowId;

                }
                else if (data.result.length > 1) {
                    modalListType = 1;
                    var dataList = [];
                    for (var i = 0; i < data.result.length; i++) {
                        dataList.push({
                            Id: data.result[i].IndividualId,
                            Code: data.result[i].LockerId,
                            Description: data.result[i].FullName
                        });
                    }
                    ShowModalList(dataList);
                    $('#modalDialogList').UifModal('showLocal', Resources.Language.LabelAgentPrincipal);
                }
                else {
                    if (agentSearchType == 1) {
                        $("#inputAgentPrincipal").data("Object", null);
                        $("#inputAgentPrincipal").val('');
                    }
                    else if (agentSearchType == 2) {
                        $("#inputAgentsAgency").data("Object", null);
                        $("#inputAgentsAgency").val('');
                    }
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageSearchAgents, 'autoclose': true })
                }
            }
            else {
                if (agentSearchType == 1) {
                    $("#inputAgentPrincipal").data("Object", null);
                    $("#inputAgentPrincipal").val('');
                }
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchAgents, 'autoclose': true })
        });      
    }
}
var deleteAgentCallback = function (deferred, data) {
    if (data.StatusTypeService !== 2) {
        data.StatusTypeService = 4;
        ComissDeleted.push(data);
    }
    deferred.resolve();
};

function CreateAgent(AgentView) {
    var Agent = {};
    Agent.IndividualId = AgentView.IndividualId;
    Agent.FullName = AgentView.FullName;
    return Agent;
}

function DeleteAgent(data) {
    var IndexNew = -1;
    var agents = $("#listModalAgentsData").UifListView('getData');
    var agentTemp = Agent.slice(0);
    Agent = [];
    $("#listModalAgentsData").UifListView({ source: null, customAdd: false, customEdit: true, add: false, edit: true, delete: true, deleteCallback: deleteAgentCallback, displayTemplate: "#agencyTemplate", selectionType: 'single', height: heightAgent });

    $.each(agents, function (index, value) {
        if (this.IndividualId != data.IndividualId) {
            IndexNew = IndexNew + 1;
            Agent.push(this);
            $("#listModalAgentsData").UifListView("addItem", this);
            //Buscar Egencia
            $.each(agentTemp, function (indexAgent, valueAgent) {
                if (valueAgent.IndividualId == value.IndividualId) {
                    Agent[IndexNew].ProductAgencyCommiss = agentTemp[indexAgent].ProductAgencyCommiss;
                    agentTemp.splice(indexAgent, 1);
                    return false;
                }
            });


        }
    });
}

function SaveTemporal() {
    //Copiar por valor
    var AgentTemp = Agent.slice(0);
    var exist = false;
    Agent = []
    var agents = $("#listModalAgentsData").UifListView('getData');
    $.each(agents, function (index, value) {
        exist = false;
        Agent.push(this);
        //Buscar Agencia
        $.each(AgentTemp, function (indexAgent, valueAgent) {
            if (valueAgent.IndividualId == value.IndividualId) {
                Agent[index].ProductAgencyCommiss = AgentTemp[indexAgent].ProductAgencyCommiss;
                AgentTemp.splice(indexAgent, 1);
                exist = true;
                return false;
            }
        });
        //existe
        if (!exist) {
            Agent[index].ProductAgencyCommiss = [];
        }

    });
}
function ClearAgent() {
    AgentRowId = -1;
    EnabledDisabledAgent(false);
    $("#inputAgentPrincipal").val('');
    $("#inputAgentPrincipal").data("Object", null);
    $('#chkAgentAssigned').prop('checked', false);
}
function EnabledDisabledAgent(control) {
    $("#inputAgentPrincipal").prop("disabled", control);
}
function CleanObjectAgent() {
    $("#listModalAgentsData").UifListView({ source: null, customAdd: false, customEdit: true, add: false, edit: true, delete: true, deleteCallback: deleteAgentCallback, displayTemplate: "#agencyTemplate", selectionType: 'single', height: heightAgent });
}
function ValidateAgents() {
    var msj = "";
    if ($("#inputAgentPrincipal").data("Object") == undefined) {
        msj = Resources.Language.ErrorSelectAgent + "<br>"
    }
    if (msj != "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelInformative + " <br>" + msj, 'autoclose': true })
        return false;
    }
    return true;
}

function enabledDisabledControl(control) {
    $("#btnAddAgenciesAgentComission").prop("disabled", control);
    $("#btnIncentivesForAgents").prop("disabled", control);
}
function setCommionAgent() {
    if (Product != null && CurrentWorkAgents != null) {
        $.each(CurrentWorkAgents, function (index, value) {
            if (value.AgencyComiss != null && value.AgencyComiss.length > 0) {
                value.CommisionText = Resources.Language.labelDifferentialCommission;
            }
        });
    }
}

function SaveAllAgents(assigned) {
    
        AgentRequestT.SaveAllAgents($("#selectPrefixCommercial").UifSelect("getSelected"), Product.Id, assigned).success(function (response) {
        if (response.success) {
            if (response.result == 'True') {
                if (assigned != 0) {
                    $.UifNotify('show', { 'type': 'info', 'message': "Se asignaron todos los intermediarios" });
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': "Se desasignaron todos los intermediarios" });
                }
                ObjectProduct.getProductAgents();
                ClearAgent();
                $("#DivPreloader").hide();
            } else {
                $("#DivPreloader").hide();
                $.UifNotify('show', { 'type': 'info', 'message': 'Error al ejecutar transacción' });
            }
        }      
    }).error(function (data) {
        //Preloader(false);
        $.UifNotify('show', { 'type': 'info', 'message': 'Error al ejecutar transacción' });
    });


 
}
