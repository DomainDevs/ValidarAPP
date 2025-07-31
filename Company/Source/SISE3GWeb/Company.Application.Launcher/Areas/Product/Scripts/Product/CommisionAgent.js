$.ajaxSetup({ async: false });
var IndexAgent = -1;
var ComissDeleted = [];
$(document).ready(function () {
    InitializeCommision();
    EventCommisionAgent();
});
function InitializeCommision() {
    $("#listModalAgencyComissionData").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: deleteAgentCallback, displayTemplate: "#agenciesAgentTemplate", selectionType: 'single', height: 310 });
    $("#inputAgentPrincipal").ValidatorKey(ValidatorType.Onlylettersandnumbers, ValidatorType.Onlylettersandnumbers, 1);
    $("#inputModalAgenciesComission").OnlyDecimals(2);
    $("#inputModalAgenciesAdditionalComission").OnlyDecimals(2);
    $("#inputModalAgenciesComission").val('1');
}

function clearAgenciesAgentComission() {
    $("#selectModalAgenciesAgent").UifSelect("setSelected", null);
    //$("#selectModalAgenciesAgent").val('');
    $("#inputModalAgenciesComission").val('1');
    $("#inputModalAgenciesAdditionalComission").val('0');
}

function EventCommisionAgent() {
    //Carga Pantalla Comisiones
    $("#btnAddAgenciesAgentComission").on("click", function () {
        //if (ValidateCommisionAgent()) {
            ClearAgencyComission();
            loadCommisionAgent();
            $("#inputModalAgenciesComission").val('1');
        //}
    });
    //agregar Comision
    $("#btnSaveAgenciesAgentComission").on("click", function () {
        SaveAgencyList();
    });
    //Grabar Comision
    $("#btnModalAgenciesAgentComissionSave").on("click", function () {
        saveCommision();
    });
    //Cerrar Comision
    $("#btnModalAgenciesAgentComissionClose").on("click", function () {
        ComissDeleted = [];
        HidePanelsProduct(MenuProductType.Commission);
    });
    $("#listModalAgencyComissionData").on("rowEdit", function (event, item, index) {
        console.log(item)
        $("#selectModalAgenciesAgent").UifSelect("setSelected", item.AgencyId);
        //$("#selectModalAgenciesAgent").val(item.AgencyId);
        $("#inputModalAgenciesComission").val(item.CommissPercentage);
        //$("#inputModalAgenciesComission").val(item.Percentage);
        $("#inputModalAgenciesAdditionalComission").val(item.AdditionalCommissionPercentage);
        //$("#inputModalAgenciesAdditionalComission").val(item.PercentageAdditional);
    });

    $("#btnCancelAgenciesAgentComission").on("click", function () {
        clearAgenciesAgentComission();
    });
}

function loadCommisionAgent() {
    $("#inputModalAgenciesAgentDescription").val($("#inputAgentPrincipal").data("Object").FullName);
    $("#inputModalAgenciesAgentDescription").data("IndividualId", $("#inputAgentPrincipal").data("Object").IndividualId)
    GetAgenciesByAgentId($("#inputAgentPrincipal").data("Object").IndividualId)
    //Cargar Agencias
    IndexAgent = AgentRowId;//LoadIndexAgent();
    LoadComissionAgency();
    $("#modalAgenciesAgentComission").UifModal('showLocal', Resources.Language.ButtonComissionAgencies);
}

// Obtiene la informacion de la agencia
function GetAgenciesByAgentId(agentId) {
    var controller = rootPath + 'Product/Product/GetAgenciesByAgentId?agentId=' + agentId;
    $("#selectModalAgenciesAgent").UifSelect({ source: controller });

}

function AddCommisionAgent() {
    var ExistAgent = false;
    var IndexModalAgencies;
    if ($("#inputModalAgenciesAdditionalComission").val()=='') {
        $("#inputModalAgenciesAdditionalComission").val('0')
    }
    $("#formAgenciesAgentComission").validate();
    if ($("#formAgenciesAgentComission").valid()) {
        $.each($("#listModalAgencyComissionData").UifListView("getData"), function (index, value) {
            if (value.IndividualId == $("#inputModalAgenciesAgentDescription").data("IndividualId") && value.AgencyId == $("#selectModalAgenciesAgent").UifSelect('getSelected')) {
                ExistAgent = true;
                IndexModalAgencies = index;
                return false;
            }
        });
        var agentComissionItem = CreateAgentcyComission();
        if (!ExistAgent) {
            agentComissionItem.StatusTypeService = 2;
            $("#listModalAgencyComissionData").UifListView("addItem", agentComissionItem);
        }
        else {
            agentComissionItem.StatusTypeService = 3;
            $("#listModalAgencyComissionData").UifListView("editItem", IndexModalAgencies, agentComissionItem);
        }
    }
    //listViewColors("listModalAgencyComissionData");
}

function saveCommision() {

    if ($("#listModalAgencyComissionData").UifListView('getData').length == 0) {
        $("#inputAgentPrincipal").data("Object").AgencyComiss = null;
        if (ComissDeleted.length > 0) {
            $("#inputAgentPrincipal").data("Object").AgencyComiss = [];
            $.each(ComissDeleted, function (index, value) {
                $("#inputAgentPrincipal").data("Object").AgencyComiss.push(value);
            });
        }
    }
    else {
        $("#inputAgentPrincipal").data("Object").AgencyComiss = [];
        $("#inputAgentPrincipal").data("Object").AgencyComiss = $("#listModalAgencyComissionData").UifListView('getData');
        $("#inputAgentPrincipal").data("Object").CommisionText = Resources.Language.labelDifferentialCommission;
        if (ComissDeleted.length > 0) {
            $.each(ComissDeleted, function (index, value) {
                $("#inputAgentPrincipal").data("Object").AgencyComiss.push(value);
            });
        }
    }
    Product.pendingChanges = true;
    if ($("#inputAgentPrincipal").data("Object").StatusTypeService != 2 && $("#chkAgentAssigned").is(':checked')) {
        $("#inputAgentPrincipal").data("Object").StatusTypeService = 3;
    }
    HidePanelsProduct(MenuProductType.Commission);
}

function ProductAgencyCommissions() {
    this.AgentId = $("#inputModalAgenciesAgentDescription").data("IndividualId");
    this.AgentName = $("#inputModalAgenciesAgentDescription").val();
    this.AgencyId = $("#selectModalAgenciesAgent").UifSelect('getSelected');
    this.AgencyName = $("#selectModalAgenciesAgent").UifSelect('getSelectedText');
    //this.Percentage = $("#inputModalAgenciesComission").val() == 0 ? 1 : $("#inputModalAgenciesComission").val();
    this.CommissPercentage = $("#inputModalAgenciesComission").val() == 0 ? 1 : $("#inputModalAgenciesComission").val();
    //this.PercentageAdditional = $("#inputModalAgenciesAdditionalComission").val();
    this.AdditionalCommissionPercentage = $("#inputModalAgenciesAdditionalComission").val();
    //this.AgencyId = $("#selectModalAgenciesAgent").val();
}

function CreateAgentcyComission() {
    var agentcyComission = {};
    agentcyComission.IndividualId = $("#inputModalAgenciesAgentDescription").data("IndividualId");
    agentcyComission.AgentName = $("#inputModalAgenciesAgentDescription").val();
    agentcyComission.AgencyId = $("#selectModalAgenciesAgent").UifSelect('getSelected');
    agentcyComission.AgencyName = $("#selectModalAgenciesAgent").UifSelect('getSelectedText');
    agentcyComission.CommissPercentage = $("#inputModalAgenciesComission").val() == 0 ? 1 : $("#inputModalAgenciesComission").val(); //parseFloat();
    //agentcyComission.Percentage = $("#inputModalAgenciesComission").val() == 0 ? 1 : $("#inputModalAgenciesComission").val();
    agentcyComission.AdditionalCommissionPercentage = $("#inputModalAgenciesAdditionalComission").val() == 0 || $("#inputModalAgenciesAdditionalComission").val() == "" ? null : $("#inputModalAgenciesAdditionalComission").val();  //parseFloat();
        //$("#inputModalAgenciesAdditionalComission").val()
    //agentcyComission.PercentageAdditional = $("#inputModalAgenciesAdditionalComission").val() == 0 || $("#inputModalAgenciesAdditionalComission").val() == "" ? null : parseFloat($("#inputModalAgenciesAdditionalComission").val());
    agentcyComission.StatusTypeService = 2;
    agentcyComission.ProductId = Product.Id;
    clearAgenciesAgentComission();
    return agentcyComission;
}

function ClearAgencyComission() {
    $('#formAgenciesAgentComission')[0].reset();
    ComissDeleted = [];
    $("#listModalAgencyComissionData").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: deleteAgentCallback, displayTemplate: "#agenciesAgentTemplate", selectionType: 'single', height: 310 });

}

function LoadIndexAgent() {
    var IndexTemp = -1;
    $.each(Agent, function (index, value) {
        if (value.IndividualId == $("#inputModalAgenciesAgentDescription").data("IndividualId")) {
            IndexTemp = index;
            return false;
        }
    });
    return IndexTemp;
}

function LoadComissionAgency() {
    if ($("#inputAgentPrincipal").data("Object") != null) {
        if ($("#inputAgentPrincipal").data("Object").AgencyComiss != null) {
            var comissList = $("#inputAgentPrincipal").data("Object").AgencyComiss.filter(function (itemListNew) {
                return itemListNew.StatusTypeService
                    == 1 || itemListNew.StatusTypeService == 2 || itemListNew.StatusTypeService == 3;
            });
            ComissDeleted = $("#inputAgentPrincipal").data("Object").AgencyComiss.filter(function (itemListNew) {
                return itemListNew.StatusTypeService
                    == 4;
            });
            if (comissList.length > 0) {
                $("#listModalAgencyComissionData").UifListView({ sourceData: comissList, add: false, edit: true, customEdit: true, delete: true, deleteCallback: deleteAgentCallback, displayTemplate: "#agenciesAgentTemplate", selectionType: 'single', height: 310 });           
            }
        }
    }
}

// SaveAgencyList - Funcion para el boton "Guardar" periferico de agencias
function SaveAgencyList() {
    if ($("#selectModalAgenciesAgent").UifSelect('getSelected') > 0) {
        AddCommisionAgent();
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InfoSaveAgencySelected, 'autoclose': true });
    }
}
function ValidateCommisionAgent() {
    if (AgentRowId == -1) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectAgency, 'autoclose': true })
        return false;
    }
    return true;
}