var agentIndex = -1;
var agentSearchType = 1;
var isPrincipal = false;
var modalListType;
$(() => {
    new RenewalRequestGroupingAgent();
});
class RenewalRequestGroupingAgent extends Uif2.Page {
    getInitialState() {

        this.initializeAgent();
        this.ControlsAgent(false);
    }
    //Seccion Eventos
    bindEvents() {
        $('#btnAgentsClose').on('click', this.AgentClose);
        $('#btnAgentsCancel').on('click', Agent.ClearControlAgent);      
        $('#listAgencies').on('rowEdit', this.AgentEdit);
        $('#listAgencies').on('rowDelete', this.AgentDelete);
        $('#tableResults tbody').on('click', 'tr', this.SelectSearch);
        $("#btnAgentsSave").on('click', this.AgentsSave);
        $("#btnAgentsAccept").on('click', this.AgentsAccept);
        $("#inputAgentsAgent").on('buttonClick', this.GetAgenciesByAgentIdDescription);
    }  
    initializeAgent() {
        $("#inputAgentsAgent").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $('#inputAgentsParticipation').OnlyDecimals(2);
        $('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        //Agent.ClearControlAgent();
    }
    GetAgenciesByAgentIdDescription()
    {
        var agent = [];
        var description = $('#inputAgentsAgent').val().trim();
        var number = parseInt(description, 10);
        if((!isNaN(number) && number != 0) || description.length > 2)
        {
            RenewalRequestGroupingAgentRequest.GetAgenciesByAgentIdDescription(0,$("#inputAgentsAgent").val().trim()).done(function(dataAgency){
                if(dataAgency.success)
                {
                    if(dataAgency.result.length == 1)
                    {
                        $("#inputAgentsAgent").data("Object", data.result[0]);
                        $("#inputAgentsAgent").val(data.result[0].Agent.FullName);
                        RenewalRequestGroupingAgentRequest.GetAgenciesByAgentId(data.result[0].Agent.IndividualId, 0).done(function(dataAgencies){
                            if (dataAgencies.success) {
                                $('#selectAgentsAgency').UifSelect({ sourceData: dataAgency.result});                
                            }
                            else {
                                $("#selectAgentsAgency").UifSelect("setSelected", null);              
                            }
                        });
                    }
                    else if (dataAgency.result.length > 1) {
                        modalListType = 2;
                        for (var i = 0; i < dataAgency.result.length; i++) {
                            agent.push({
                                Id: dataAgency.result[i].Agent.IndividualId,
                                Code: dataAgency.result[i].Code,
                                Description: dataAgency.result[i].Agent.FullName
                            });
                        }
                        Agent.ShowDefaultResults(agent);
                        $('#modalDialogList').UifModal('showLocal', Resources.Language.SelectMainAgent);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorNotAgents });
                    }
                    
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }

            });
        }
    }
    static calculateParticipation() {
        var participationTotal = 0;
        var list = $("#listAgencies").UifListView("getData");
        for (var i = 0; i < list.length; i++) {
            participationTotal = participationTotal + parseFloat(list[i].Agency.Participation.toString().replace(separatorDecimal, separatorThousands));
        }
        return participationTotal;
    }
    ControlsAgent(EnabledDisabled) {
        $('#inputAgentsAgent').prop('disabled', EnabledDisabled);
        $('#selectAgentsAgency').prop('disabled', EnabledDisabled);
        $('#inputAgentsParticipation').prop('disabled', EnabledDisabled);   
    } 

    AgentEdit(event, data, index) {
        isPrincipal = data.Agency.IsPrincipal;
        if (isPrincipal) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateEditAgent });
        }
        else
        {
            agentIndex = index;
            Agent.EditAgency(data, index);
        }       

    }
    AgentDelete(event, data) {
        if (data.Agency.IsPrincipal) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateDeleteAgent });
        }
        else {
            var participationTmp = data.Agency.Participation;
            var index = $("#listAgencies").UifListView("findIndex",
            function (item) {
                if (item.Agency.Agent.IndividualId == data.Agency.Agent.IndividualId && item.Agency.Id == data.Agency.Id) {
                    return true;
                } else {
                    return false;
                }
            });
            $("#listAgencies").UifListView("deleteItem", index);
            Agent.calculateParticipationPrincipal(participationTmp);
            var participationTotal = Agent.calculateParticipation();
            $("#lblAgentsTotalParticipation").text(participationTotal + "%");
        }
    }
    AgentsSave() {
        var participationTotal = Agent.calculateParticipation();
        if (participationTotal > 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
        }
        else if (participationTotal < 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
        }
        else {
            $("#inputRequest").data("renewalRequest").CoRequestEndorsement[0].CoRequestAgent= $("#listAgencies").UifListView("getData");
            $('#selectedAgents').text(Resources.Language.LabelParticipants + ": (" +  $("#listAgencies").UifListView("getData").length + ") " + Resources.Language.LabelCommission + ": " + FormatMoney(participationTotal) + "%");
            $("#modalAgents").UifModal('hide');
        }
    }
    AgentsAccept() {
        $("#formAgents").validate();
        var isValid = $("#formAgents").valid();
        if (isValid) {
            if ($('#inputAgentsAgent').data('Object') == null) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateAgent, 'autoclose': true });                
            }
            else
            {
                if(!Agent.DuplicateKeyAgency())
                {
                    Agent.pushAgent();
                }
                else
                {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageAgencyExist, 'autoclose': true });
                }
            }
        }
    } 
    static DuplicateKeyAgency() {
        var duplicate = false;
        $.each($("#listAgencies").UifListView('getData'), function (key, item) {
            if (agentIndex != key && item.Agency.Agent.IndividualId !=null && item.Agency.Agent.IndividualId== $("#inputAgentsAgent").data("Object").Agent.IndividualId && item.Agency.Id == $("#selectAgentsAgency").UifSelect('getSelected')) {
                duplicate = true;
                return false;
            }
        });
        return duplicate;
    }
    static pushAgent() {
        var participationTotal = Agent.calculateParticipation();
        if(agentIndex!=-1)
        {
            participationTotal = participationTotal - parseFloat($("#listAgencies").UifListView("getData")[agentIndex].Agency.Participation.toString().replace(separatorDecimal, separatorThousands));
        }
        participationTotal = participationTotal + parseFloat($("#inputAgentsParticipation").val());
        if (participationTotal > 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
        }
        else
        {
            var agent = {
                Agency: {
                    Id: $('#selectAgentsAgency').UifSelect('getSelected'),
                    IsPrincipal: isPrincipal,
                    Participation: $("#inputAgentsParticipation").val(),
                    Agent: {
                        IndividualId: $("#inputAgentsAgent").data("Object").Agent.IndividualId,
                        FullName: $("#inputAgentsAgent").data("Object").Agent.FullName
                    },
                    Code: $("#inputAgentsAgent").data("Object").Code,
                    FullName:$('#selectAgentsAgency').UifSelect('getSelectedText')
                }
            };
            if (agentIndex == -1) {
                if (parseFloat($("#lblAgentsTotalParticipation").text().replace(separatorDecimal, separatorThousands)) == 100) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
                    return;
                }
                $("#listAgencies").UifListView("addItem", agent);
            }
            else {

                $("#listAgencies").UifListView("editItem", agentIndex, agent);
            }
            Agent.ClearControlAgent();
            $("#lblAgentsTotalParticipation").text(participationTotal + "%");   
        }
    }
    static calculateParticipationPrincipal(paticipationTmp) {
        var list = $("#listAgencies").UifListView("getData");
        for (var i = 0; i < list.length; i++) {
            if (list[i].Agency.IsPrincipal) {
                var participationTotal = parseFloat(list[i].Agency.Participation.toString().replace(separatorDecimal, separatorThousands)) + parseFloat(paticipationTmp.toString().replace(separatorDecimal, separatorThousands));
                list[i].Agency.Participation = participationTotal;
                list[i].Participation = participationTotal;
                $("#listAgencies").UifListView("editItem", i, list[i]);
                break;
            }
        }
    }   
    SelectSearch() {
        switch (modalListType) {
            case 2:
                RenewalRequestGroupingAgentRequest.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML).done(function (data) {
                    if (data.success) {
                        $("#inputAgentsAgent").data("Object", data.result[0]);
                        $("#inputAgentsAgent").val(data.result[0].Agent.FullName);    
                        RenewalRequestGroupingAgentRequest.GetAgenciesByAgentId( data.result[0].Agent.IndividualId).done(function(dataAgency){
                            if (dataAgency.success) {
                                $('#selectAgentsAgency').UifSelect({ sourceData: dataAgency.result});
                            }
                            else {
                                $("#selectAgentsAgency").UifSelect("setSelected", null);          
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }

                        });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });               
                break;           
            default:
                break;
        }        
        $('#modalDialogList').UifModal("hide");
    }
    AgentClose() {
        $("#modalAgents").UifModal('hide');
        Agent.ClearControlAgent();
    }

    static ClearControlAgent() {
        agentIndex=-1;
        isPrincipal=false;
        $("#inputAgentsAgent").val("");       
        $("#selectAgentsAgency").UifSelect({ source: null });
        $("#inputAgentsAgent").data("Object", null);
        var participationTotal = Agent.calculateParticipation();
        var participationAgent = (100 - parseFloat(participationTotal.toString().replace(separatorDecimal, separatorThousands)));
        $("#inputAgentsParticipation").val(participationAgent);
    }
    //seccion edit
    static EditAgency(data, index) {
        $("#inputAgentsAgent").data("Object", data.Agency);
        $("#inputAgentsAgent").val(data.Agency.Agent.FullName);          
        $("#inputAgentsParticipation").val(data.Agency.Participation);
        RenewalRequestGroupingAgentRequest.GetAgenciesByAgentId(data.Agency.Agent.IndividualId).done(function (dataAgency) {
            if (dataAgency.success) {
                $('#selectAgentsAgency').UifSelect({ sourceData: dataAgency.result, selectedId: data.Agency.Id});
                
            }
            else {
                $("#selectAgentsAgency").UifSelect("setSelected", null);              
            }

        }); 
    }
    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }
}

class RenewalRequestGroupingAgentRequest {
    static GetAgenciesByAgentIdDescription(agentId, description)
    {
        return $.ajax({
            type: "POST",
            url:  'GetAgenciesByAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAgenciesByAgentId(agentId)
    {
        return $.ajax({
            type: "POST",
            url: 'GetAgenciesByAgentId',
            data: JSON.stringify({ agentId: agentId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAgencyByAgentIdAgencyId(agentId,agencyId)
    {
        $.ajax({
            type: "POST",
            url: rootPath + 'GetAgencyByAgentIdAgencyId',
            data: JSON.stringify({ agentId: agentId, agencyId: agencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

}