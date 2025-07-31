
$(() => {
    new IncentivesForAgents();
});

/**
 * Variables Locales y Globales
 */
var IndexAgentIA = -1;
var IncentivesByProductByAgentByAgency = [];
var IncentiveValue;
var productId;
var individualId;
var AgentAgencyId = 0;
var agentcyIncentive = {};
var agentcyIncentiveFinal = {};
var ListIncentivesForAgentsIA = [];
var amount = 0;
var AgentInfo;
var currentRiskTypeIndex = -1;

var ListIncentives = [];
var ListIncentivesDeletedTemp = [];
var ListIncentivesCurrentDeletedTemp = [];
var ListIncentivesNewsDeletedTemp = [];
var ListIncentivesWorkTemp = [];


agentcyIncentiveFinal.ListIncentivesForAgents = [];

/**
*Clase donde se renombran los controles
*/



/**
 * Clase Principal de Deducibles por cobertura
 */

class IncentivesForAgents extends Uif2.Page {

    /**
     * Funcion para inicilizar la vista
     */
    getInitialState() {        
		$("#listModalAgencyIncentivesData").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: IncentivesForAgents.DeleteItemIncentive, displayTemplate: "#agenciesAgentIncentiveTemplate", selectionType: 'single', height: 310 });
    }

    /**
     * Eventos de los controles de la clase
     */
    bindEvents() {
        $("#btnIncentivesForAgents").on("click", IncentivesForAgents.LoadIncentivesForAgent);
        $("#btnSaveAgenciesAgentIncentive").on("click", IncentivesForAgents.SaveAgencyListIncentives);
        $("#btnModalAgenciesAgentIncentivesSave").on("click", IncentivesForAgents.SaveIncentives);
        $("#btnClearAgenciesAgentIncentive").on("click", IncentivesForAgents.ClearAgenciesAgentIncentives);
        $("#btnModalAgenciesAgentIncentivesClose").on("click", HidePanelsProduct(MenuProductType.Incentives));
		$('#listModalAgencyIncentivesData').on('rowDelete', IncentivesForAgents.DeleteItemIncentive);
        $('#listModalAgencyIncentivesData').on('rowEdit', function (event, data, position) {
            currentRiskTypeIndex = position;
            IncentivesForAgents.EditItem(currentRiskTypeIndex);
        });
    }

	static LoadIncentivesForAgent() {
		IncentivesForAgents.GetIncentives();
        $("#modalAgenciesAgentIncentives").UifModal('showLocal', Resources.Language.LabelIncentivesForAgents);        
    }

    static GetAgenciesByAgentId(agentId) {
        var controller = rootPath + 'Product/Product/GetAgenciesByAgentId?agentId=' + agentId;
        $("#selectModalAgenciesAgentIA").UifSelect({ source: controller });

    }

    static LoadIncentiveAgency() {
        if (Agent[IndexAgentIA] != null) {
            if (Agent[IndexAgentIA].ProductAgencyCommiss != null) {
            }
        }
    }

    static GetIncentives() {
		IncentivesForAgents.GetValuesOfControls();

		var currentDeleted = [];
		var currentWork = [];
		$.each(ListIncentivesWorkTemp, function (index, item) {
			if (individualId == item.IndividualId) {
				currentWork.push(item);
			}
		});
		$.each(ListIncentivesDeletedTemp, function (index, item) {
			if (individualId == item.IndividualId) {
				currentDeleted.push(item);
			}
		});
		if (currentDeleted.length == 0 && currentWork.length == 0) {
			$.each(ListIncentives, function (index, item) {
				if (individualId == item.IndividualId) {
					if (item.Incentives[0].Status != 4) {
						currentWork.push(item);
					}
					else {
						currentDeleted.push(item);
					}
				}
			});
		}
		if (currentDeleted.length > 0 || currentWork.length > 0) {
			IncentivesForAgents.SetIncentivesByProductIdByIndividualId(currentWork);
		}
		else {
			IncentivesForAgents.GetIncentivesByProductIdByIndividualIdByAgentAgencyID(productId, individualId, AgentAgencyId);
		}
    }

    static GetIncentivesByProductIdByIndividualIdByAgentAgencyID(productId, indivicualId, AgentAgencyId) {
        //if (AgentAgencyId != '') {

        return IncentivesForAgentsRequest.GetIncentivesByProductIdByIndividualIdByAgentAgencyID(productId, indivicualId,0).done(function (data) {

            if (data.success) {
                IncentivesForAgents.ClearListView();
                IncentivesByProductByAgentByAgency = data.result;
                if (IncentivesByProductByAgentByAgency.length > 0) {
                    var IncentiveAgentsListFinal = {};

                    $.each(IncentivesByProductByAgentByAgency, function (key, value) {
                        //se debe borrar el arreglo en cada iteracion
                        agentcyIncentive = {};
                        agentcyIncentive.AgencyId = parseInt(value.AgentAgencyId);
                        agentcyIncentive.AgencyName = $("#selectModalAgenciesAgentIA option[value='" + value.AgentAgencyId + "']").text();
                        agentcyIncentive.IncentiveValue = parseInt(value.Incentives[0].Incentive_Amt);
                        agentcyIncentive.AgentCode = value.AgentAgencyId;
                        agentcyIncentive.Status = 1;

                        $("#listModalAgencyIncentivesData").UifListView("addItem", agentcyIncentive);

                        IncentiveAgentsListFinal =
                            {
                                Id: agentcyIncentive.AgencyId,
                                Incentive_Amt: agentcyIncentive.IncentiveValue
                            }
                        agentcyIncentiveFinal.ListIncentivesForAgents.push(IncentiveAgentsListFinal);

                    });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });        

        
	}

	static SetIncentivesByProductIdByIndividualId(currentIncentiveWorkData) {

		IncentivesForAgents.ClearListView();
		if (currentIncentiveWorkData.length>0) {
			IncentivesByProductByAgentByAgency = currentIncentiveWorkData;
				if (IncentivesByProductByAgentByAgency.length > 0) {
					var IncentiveAgentsListFinal = {};

					$.each(IncentivesByProductByAgentByAgency, function (key, value) {
						//se debe borrar el arreglo en cada iteracion
						agentcyIncentive = {};
                        agentcyIncentive.AgencyId = value.AgentAgencyId;
						agentcyIncentive.AgencyName = $("#selectModalAgenciesAgentIA option[value='" + value.AgentAgencyId + "']").text();
                        agentcyIncentive.IncentiveValue = parseInt(value.Incentives[0].Incentive_Amt);
						agentcyIncentive.Status = value.Incentives[0].Status;
						agentcyIncentive.AgentCode = value.AgentAgencyId;
                        agentcyIncentive.Status = value.Status;

						$("#listModalAgencyIncentivesData").UifListView("addItem", agentcyIncentive);

						IncentiveAgentsListFinal =
							{
								Id: agentcyIncentive.AgencyId,
								Incentive_Amt: agentcyIncentive.IncentiveValue
							}
						agentcyIncentiveFinal.ListIncentivesForAgents.push(IncentiveAgentsListFinal);

					});
					listViewColors("listModalAgencyIncentivesData");
				}
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
	}


    static ClearListView() {
        $("#listModalAgencyIncentivesData").UifListView("clear");
    }

    static MapValues() {
        amount = agentcyIncentive.IncentiveValue;
        if (amount > 0) {
            $("#inputIncentiveValue").val(amount);
            $("#inputIncentiveValue").prop("disabled", true);
        }
        else {
            $("#inputIncentiveValue").val('');
            $("#inputIncentiveValue").prop("disabled", false);
        }

    }

    static ClearValuesInput() {
        amount = 0;
        $("#inputIncentiveValue").val('');
        $("#inputIncentiveValue").prop("disabled", false);
    }


    static EditItem(item) {
        $("#selectModalAgenciesAgentIA").UifSelect('setSelected', $("#listModalAgencyIncentivesData").UifListView("getData")[item].AgentCode);
        $("#inputIncentiveValue").prop("disabled", false);
        $("#inputIncentiveValue").val($("#listModalAgencyIncentivesData").UifListView("getData")[item].IncentiveValue);
    }

    static GetValuesOfControls() {
        $("#inputIncentiveValue").val('');
        $("#inputIncentiveValue").prop("disabled", false);
        productId = $("#inputAgentPrincipal").data("Object").ProductId;
        individualId = $("#inputAgentPrincipal").data("Object").IndividualId;
        AgentAgencyId = $("#selectModalAgenciesAgentIA").UifSelect("getSelected");;
    }

    static SaveAgencyListIncentives() {
        if ($("#selectModalAgenciesAgentIA").UifSelect('getSelected') > 0) {
            if ($("#inputIncentiveValue").val() != 0) {
                IncentivesForAgents.AddIncentiveAgent();
                Product.pendingChanges = true;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageErrorSaveIncentive, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InfoSaveAgencySelected, 'autoclose': true });
        }
    }

    static AddIncentiveAgent() {
        var ExistAgent = false;
        var IndexModalAgencies;
        amount = $("#inputIncentiveValue").val();
        var AgencySelect = $("#selectModalAgenciesAgentIA").UifSelect('getSelected');
        $("#formAgenciesAgentIncentives").validate();
        if ($("#formAgenciesAgentIncentives").valid()) {
            $.each($("#listModalAgencyIncentivesData").UifListView("getData"), function (index, value) {
				if (value.AgentCode == AgencySelect) {
					ExistAgent = true;
					IndexModalAgencies = index;
					if (value.Status == 1 || value.Status == 3) {
						$('#listModalAgencyIncentivesData').UifListView('editItem', IndexModalAgencies, IncentivesForAgents.CreateAgentcyIncentive(3));
					}
					else {
						$('#listModalAgencyIncentivesData').UifListView('editItem', IndexModalAgencies, IncentivesForAgents.CreateAgentcyIncentive(2));
					}
                    
                }
            });
            if (!ExistAgent) {
                $("#listModalAgencyIncentivesData").UifListView("addItem", IncentivesForAgents.CreateAgentcyIncentive(2));
            }
		}
		listViewColors("listModalAgencyIncentivesData");
    }

    static CreateAgentcyIncentive(status) {
        agentcyIncentive = {};
        agentcyIncentive.AgentId = $("#inputModalAgenciesAgentDescriptionIA").data("IndividualId");
        if (agentcyIncentive.AgentId == undefined) {
            agentcyIncentive.AgentId = AgentInfo.IndividualId;
        }
        agentcyIncentive.IndividualId = agentcyIncentive.AgentId
        agentcyIncentive.AgentName = $("#inputModalAgenciesAgentDescriptionIA").val();
        agentcyIncentive.AgencyId = $("#selectModalAgenciesAgentIA").UifSelect('getSelected');
        agentcyIncentive.AgencyName = $("#selectModalAgenciesAgentIA").UifSelect('getSelectedText');
        if (IncentivesByProductByAgentByAgency[0] != undefined) {          
            agentcyIncentive.IncentiveValue = $("#inputIncentiveValue").val();
        }
        else {
            IncentiveValue = $("#inputIncentiveValue").val()
            agentcyIncentive.IncentiveValue = IncentiveValue;
        }

        agentcyIncentive.AgentCode = $("#selectModalAgenciesAgentIA").val();
		agentcyIncentive.Status = status;

        IncentivesForAgents.ClearAgenciesAgentIncentives();
        IncentivesForAgents.ClearValuesInput();
        return agentcyIncentive;
    }

    static ClearObject() {
        agentcyIncentive.AgentId = null;
        agentcyIncentive.AgentName = null;
        agentcyIncentive.AgencyId = null;
        agentcyIncentive.AgencyName = null;
        agentcyIncentive.IncentiveValue = null;
    }

    static ClearAgenciesAgentIncentives() {
        $("#selectModalAgenciesAgentIA").val('');
        $("#inputIncentiveValue").val('');
        $("#inputIncentiveValue").prop("disabled", false);
    }

    static SaveIncentives() {
        if ($("#listModalAgencyIncentivesData").UifListView('getData').length == 0) {
        }
        else {
			var IncentiveAgentsList = {};
			var updateDataIncentive = [];
            agentcyIncentive.ListIncentivesForAgents = [];

            ListIncentivesForAgentsIA = $("#listModalAgencyIncentivesData").UifListView('getData');
            $.each(ListIncentivesForAgentsIA, function (key, value) {
					var updatedIncentive = {};
					updatedIncentive.ProductId = AgentInfo.ProductId;
					updatedIncentive.IndividualId = AgentInfo.IndividualId;
					updatedIncentive.AgentAgencyId = value.AgencyId;
					updatedIncentive.Incentives = [];
					var Incentives = {};
                    Incentives.Id = parseInt(value.AgencyId);
                    Incentives.Incentive_Amt = parseInt(value.IncentiveValue);
					updatedIncentive.Incentives.push(Incentives);
                    updatedIncentive.Status = value.Status;
					updateDataIncentive.push(updatedIncentive);
			
                IncentiveAgentsList =
                    {
                        Id: value.AgencyId,
                        Incentive_Amt: value.IncentiveValue
                    }
                agentcyIncentive.ListIncentivesForAgents.push(IncentiveAgentsList);
			});
			$.each(ListIncentivesWorkTemp, function (key, value) {
				var ifExist = updateDataIncentive.filter(function (item) {
					return item.IndividualId
						== value.IndividualId && item.AgentAgencyId
					== value.AgentAgencyId;
				});
				if (ifExist.length > 0) {
				}
				else {
					updateDataIncentive.push(value);
				}
			});
			$.each(ListIncentivesCurrentDeletedTemp, function (key, value) {
				ListIncentivesDeletedTemp.push(value);
			});
			ListIncentivesCurrentDeletedTemp = [];
			ListIncentivesWorkTemp = updateDataIncentive;
            agentcyIncentive.CommisionText = Resources.Language.labelDifferentialCommission;
        }
        HidePanelsProduct(MenuProductType.Incentives);
    }

	static DeleteItemIncentive(event, data) {
		if (data.Status != 2) {
			var deletedIncentive = {};
			deletedIncentive.ProductId = AgentInfo.ProductId;
			deletedIncentive.IndividualId = AgentInfo.IndividualId;
			deletedIncentive.AgentAgencyId = data.AgencyId;
			deletedIncentive.Incentives = [];
			var Incentives = {};
            Incentives.Id = parseInt(data.AgencyId);
            Incentives.Incentive_Amt = parseInt(data.IncentiveValue);
			deletedIncentive.Incentives.push(Incentives);
            deletedIncentive.Status = 4;
			ListIncentivesCurrentDeletedTemp.push(deletedIncentive);
		}
		else {
			var deletedIncentive = {};
			deletedIncentive.ProductId = AgentInfo.ProductId;
			deletedIncentive.IndividualId = AgentInfo.IndividualId;
			deletedIncentive.AgentAgencyId = data.AgencyId;
			deletedIncentive.Incentives = [];
			var Incentives = {};
            Incentives.Id = parseInt(data.AgencyId);
            Incentives.Incentive_Amt = parseInt(data.IncentiveValue);
			deletedIncentive.Incentives.push(Incentives);
            deletedIncentive.Status = 4;
			ListIncentivesNewsDeletedTemp.push(deletedIncentive);
		}
		event.resolve();
		IncentivesForAgents.ClearAgenciesAgentIncentives();
	}
    static LoadListIncentives() {
        $.each(IncentivesByProductByAgentByAgency, function (index, value) {
            agentcyIncentive.AgentId = $("#inputModalAgenciesAgentDescriptionIA").data("IndividualId");
            agentcyIncentive.AgentName = $("#inputModalAgenciesAgentDescriptionIA").val();
            agentcyIncentive.AgencyId = $("#selectModalAgenciesAgentIA").UifSelect('getSelected');
            agentcyIncentive.AgencyName = $("#selectModalAgenciesAgentIA").UifSelect('getSelectedText');
            agentcyIncentive.IncentiveValue = value.IncentiveAmount;
            agentcyIncentive.AgentCode = $("#selectModalAgenciesAgentIA").val();
            //agentcyIncentive.ListIncentivesForAgents = ListIncentivesForAgentsIA;
            //IncentivesForAgents.ClearAgenciesAgentIncentives();
        });

        return agentcyIncentive;
    }

    static ValidateExistance() {
        var ExistAgent = false;
        var IndexModalAgencies;
        $.each($("#listModalAgencyIncentivesData").UifListView("getData"), function (index, value) {
            //if (value.AgentId == $("#inputAgenciesAgentDescriptionIA").data("IndividualId") && value.AgentCode == $("#selectModalAgenciesAgentIA").UifSelect('getSelected')) {
            if (value.AgentCode == $("#selectModalAgenciesAgentIA").UifSelect('getSelected')) {
                ExistAgent = true;
                IndexModalAgencies = index;
                return false;
            }
        });
        if (!ExistAgent) {
            $("#listModalAgencyIncentivesData").UifListView("addItem", IncentivesForAgents.LoadListIncentives());
        }
        else {
            $("#listModalAgencyIncentivesData").UifListView("editItem", IndexModalAgencies, IncentivesForAgents.LoadListIncentives());
        }
    }

}

