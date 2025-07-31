var ComissionAgents = [];
var AgentComissionId = 0;
var rowAgentCommissionId = -1;

class ComissionAgency extends Uif2.Page {

    getInitialState() {
        $("#InputEffectiveDate").val(DateNowPerson);

    }
    //Seccion Eventos
    bindEvents() {
        $("#btnNewAgentComission").click(ComissionAgency.ClearControlCommissionAgenct);
        $("#selectAgencyBranchComission").on("itemSelected", this.PrefixSelected); 
        $("#selectPrefixTechnicalComission").on("itemSelected", this.SubBranchSelected);
        $("#btnacceptAgentComission").click(this.AcceptAgentComission);
        $('#listAgentComisson').on('rowEdit', this.AgentComissionEdit);
        $('#listAgentComisson').on('rowDelete', this.AgentComissionDelete);
        $("#linkComission").click(this.LoadCombos);
    }

    LoadCombos() {
        if (individualId > 0) {
            ComissionAgency.GetAgencybyIndividualId(individualId);
            ComissionAgency.GetPrefixesByAgentId(individualId);
            Agent.PrefixByIndividual(individualId);
            $("#InputEffectiveDate").val(DateNowPerson);
        }
    }

    AgentComissionEdit(event, data, index) {
        rowAgentCommissionId = index;
        ComissionAgency.EditAgentComission(data, index);
    }

    //seccion grabado
    static SaveAgentComission() {
        var ComissionAgents = $("#listAgentComisson").UifListView('getData');
        var individualId = $('#CodPersonAgent').val();

        var ComissionAgentsFilter = ComissionAgents.filter(function (item) {
            return item.StatusTypeService > 1;
        });

        var CommissionObject = { ComissionAgents: ComissionAgentsFilter }
        if (ComissionAgentsFilter.length > 0) {
            AgentRequest.CreateAgent(CommissionObject, individualId)
        }
    }

    //seccion edit
    static EditAgentComission(data, index) {
        AgentComissionId = data.Id;
        $("#selectAgencyComission").UifSelect("setSelected", data.AgencyId);
        $("#selectAgencyBranchComission").UifSelect("setSelected", data.PrefixId);
        if (data.LineBusinessId != null) {
            ComissionAgency.GetLineBussiness(data.PrefixId).done(function (result) {
                if (result != null) {
                    $("#selectPrefixTechnicalComission").UifSelect({ sourceData: result.data });
                    $("#selectPrefixTechnicalComission").UifSelect("setSelected", data.LineBusinessId);
                }
            });
        }
        if (data.SubLineBusinessId != null) {
            ComissionAgency.GetSubLineBussiness(data.LineBusinessId).done(function (result) {
                if (result != null) {
                    $("#selectTechnicalSubLineComission").UifSelect({ sourceData: result.data });
                    $("#selectTechnicalSubLineComission").UifSelect("setSelected", data.SubLineBusinessId);
                }
            });
        }
        $("#PercentageCommission").val(data.PercentageCommission);
        $("#PercentageCommissionAditionals").val(data.PercentageAdditional);
        if (data.DateCommission != null) {
            $("#InputEffectiveDate").UifDatepicker('setValue', data.DateCommission);
        }
    }

    // seccion Eliminacion
    AgentComissionDelete(event, data) {
        if ($('#listAgentComisson').UifListView("getData").length > 0) {

            ComissionAgency.DeleteAgentComission(data);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorDeleteOperationQuota, 'autoclose': true });
        }
    }

    static DeleteAgentComission(Data, deferred) {
        var IndividualId;
        var StatusTypeService;

        IndividualId = $('#lblPersonCode').val();
        StatusTypeService = ParametrizationStatus.Delete;

        var AgentComission = {};
        Data.IndividualId = IndividualId;
        Data.StatusTypeService = StatusTypeService;
        AgentComission.ComissionAgents = [Data];
        AgentRequest.CreateAgent(AgentComission).done(function (data) {
            if (data.success) {
                deferred.resolve();
                deferred.done(AgentComission = $("#listAgentComisson").UifListView('getData'));
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageDeleteAgentCommission, 'autoclose': true });
            }
            else {
                deferred.reject()
            }
        });
    }

    static deleteAgentCommissionList(deferred, data) {
        ComissionAgency.DeleteAgentComission(data, deferred)
        ComissionAgency.ClearControlCommissionAgenct();
    }

    //Clear
    static ClearControlCommissionAgenct() {
        rowAgentCommissionId = -1;
        AgentComissionId = 0;
        $("#PercentageCommission").val("");
        $("#PercentageCommissionAditionals").val("");
        if (glbPolicy == null) {
            $("#selectAgencyComission").UifSelect('setSelected', null);
            $("#selectAgencyBranchComission").UifSelect('setSelected', null);
            $("#selectPrefixTechnicalComission").UifSelect('setSelected', null);
            $("#selectTechnicalSubLineComission").UifSelect('setSelected', null);
        }
        $("#InputEffectiveDate").val(DateNowPerson);
    }
    
    AcceptAgentComission() {
        if (ComissionAgency.ValidateAgentComission()) {
            ComissionAgency.CreateAgentComission();
            ComissionAgency.ClearControlCommissionAgenct();
        }
    }
    //Seccion Creacion
    static CreateAgentComission() {
        var agentComissionTmp = ComissionAgency.CreateAgentComissionModel();
        if (rowAgentCommissionId == -1) {
            $("#listAgentComisson").UifListView("addItem", agentComissionTmp);
        }
        else {
            $("#listAgentComisson").UifListView("editItem", rowAgentCommissionId, agentComissionTmp);
        }
    }

    static CreateAgentComissionModel() {
        var AgentComission = {};
        AgentComission.IndividualId = $('#lblPersonCode').val();
        AgentComission.Id = AgentComissionId;
        AgentComission.AgencyId = $("#selectAgencyComission").UifSelect("getSelected");
        AgentComission.Agency = $("#selectAgencyComission").UifSelect("getSelectedText");
        AgentComission.PrefixId = $("#selectAgencyBranchComission").UifSelect('getSelected');
        AgentComission.prefix = $("#selectAgencyBranchComission").UifSelect("getSelectedText");
        AgentComission.LineBusinessId = $("#selectPrefixTechnicalComission").UifSelect("getSelected");
        AgentComission.lineBusiness = $("#selectPrefixTechnicalComission").UifSelect("getSelectedText");
        AgentComission.SubLineBusinessId = $("#selectTechnicalSubLineComission").UifSelect("getSelected");
        AgentComission.subLineBusiness = $("#selectTechnicalSubLineComission").UifSelect("getSelectedText");
        AgentComission.PercentageCommission = $("#PercentageCommission").val();
        AgentComission.PercentageAdditional = $("#PercentageCommissionAditionals").val();
        AgentComission.DateCommission = $("#InputEffectiveDate").val();
        if (AgentComission.Id) {
            AgentComission.StatusTypeService = ParametrizationStatus.Update;
        }
        else {
            AgentComission.StatusTypeService = ParametrizationStatus.Create;
        }
        
        return AgentComission;
    }

    static CleanObjectCommissionAgent() {
        ComissionAgents = [];
        $("#listAgentComisson").UifListView({ source: null, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#ComissionTemplate", height: 256, deleteCallback:null });
    }
    static LoadCommisiionRequest(IndividualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetAgentCommissionIndividualId",
            data: JSON.stringify({ IndividualId: IndividualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) { ComissionAgency.LoadAgentComission(data.result) }
        });
    }

  
    //Seccion Load
    static LoadAgentComission(data) {
        $("#listAgentComisson").UifListView({ sourceData: data, customAdd: false, customEdit: true, add: false, edit: true, delete: true, displayTemplate: "#ComissionTemplate", height: 256, deleteCallback: ComissionAgency.deleteAgentCommissionList });
    }

    //Validaciobnes
    static ValidateAgentComission() {
        var msj = "";
        if ($("#selectAgencyComission").UifSelect("getSelected") == null || $("#selectAgencyComission").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.LabelAgency + "<br>"
        }
        if ($("#selectPrefixTechnicalComission").UifSelect("getSelected") == null || $("#selectAgencyBranchComission").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.LabelPrefixCommercial + "<br>"
        }
        if ($("#selectPrefixTechnicalComission").UifSelect("getSelected") == null || $("#selectPrefixTechnicalComission").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.LabelLineBusiness + "<br>"
        }
        if ($("#selectTechnicalSubLineComission").UifSelect("getSelected") == null || $("#selectTechnicalSubLineComission").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.LabelSubLineBusiness + "<br>"
        }
        if ($('#PercentageCommission').val().trim().length == 0) {

            msj = msj + AppResourcesPerson.LabelPercentageCommission + "<br>"
        }
        if (parseFloat($('#PercentageCommission').val().replace(separatorDecimal, separatorThousands)) > 100) {
            msj = msj + AppResourcesPerson.MessageValidateCommissionPercentage + "<br>"
        }
        if (parseFloat($('#PercentageCommissionAditionals').val().replace(separatorDecimal, separatorThousands)) > 100) {
            msj = msj + AppResourcesPerson.MessageValidateAdditionalCommissionPercentage + "<br>"
        }
        if ($('#InputEffectiveDate').val() < DateNowPerson)
        {
            msj = msj + AppResourcesPerson.NoCanCurrentFromLessCurrent + "<br>"
        }
        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msj, 'autoclose': true })
            return false;
        }
        return true;
    }
    


    static FormatAgentCommission() {
        if (ComissionAgents != null) {
            $.each(ComissionAgents, function (key, item) {
                item.DateCommission = FormatDate(item.DateCommission);
            });
        }
    }


    PrefixSelected(event, selectedItem) {
        ComissionAgency.loadLineBusiness(selectedItem);
    }
    SubBranchSelected(event, selectedItem) {
        ComissionAgency.loadselectTechnicalSubLineComission(selectedItem);
    }

    //cargar el sublineBussiness
    static loadselectTechnicalSubLineComission(selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = rootPath + "Person/Person/GetSubLinesBusinessByLineBusinessId?LinesBusiness=" + selectedItem.Id;
            $("#selectTechnicalSubLineComission").UifSelect({ source: controller });
        }
        else {
            $("#selectTechnicalSubLineComission").UifSelect();
        }
    }

    //cargar el LineBussiness
    static loadLineBusiness(selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = rootPath + "Person/Person/GetLineBusinessByPrefixId?prefixId=" + selectedItem.Id;
            $("#selectPrefixTechnicalComission").UifSelect({ source: controller });
        }
        else {
            $("#selectPrefixTechnicalComission").UifSelect();
        }
    }

    static GetAgencybyIndividualId(individualId) {
        ComissionAgencyRequest.GetAgencybyIndividualId(individualId).done(function (data) {
            
            $("#selectAgencyComission").UifSelect({ sourceData: data.data });
        });
    }
    static GetPrefixesByAgentId(individualId) {
        ComissionAgencyRequest.GetPrefixesByAgentId(individualId).done(function (data) {
            $("#selectAgencyBranchComission").UifSelect({ sourceData: data.result });
        });
    }


    //carga los combos al editar 
    static GetLineBussiness(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetLineBusinessByPrefixId?prefixId=" + prefixId,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubLineBussiness(LinesBusinessId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetSubLinesBusinessByLineBusinessId?LinesBusiness=" + LinesBusinessId,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


}


