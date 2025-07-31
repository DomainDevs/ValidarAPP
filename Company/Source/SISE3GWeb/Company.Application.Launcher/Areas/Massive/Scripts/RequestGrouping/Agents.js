$.ajaxSetup({ async: false });
var agentIndex = 0;
var agenciesTmp2 = [];
var isPrincipal = false;

$("#inputAgentsAgent").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
$('#inputAgentsParticipation').OnlyDecimals(2);

//Evento del botón cancelar
$('#btnAgentsCancel').on('click', function () {
    clearAgentForm();
});

//Evento del boton cerrar Cierra la ventana de intermediario
$('#btnAgentsClose').on('click',function () {    
    RequestGrouping.ShowPanel(MenuTypeGrouping.REQUESTGROUPING);

    if (!readOnlyRequest.IsReadOnly) {
        agenciesTmp = [];
    }
    clearAgentForm();
});

$('#selectAgentsAgency').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        ValidateAgencyAgent(selectedItem.Id);
    }
    else {
        $("#selectAgentsAgency").UifSelect();
    }
});

function ValidateAgencyAgent(agencyId) {
    
    var agentId = 0;
    agentId = $("#inputAgentsAgent").data("Object").Agent.IndividualId;
    $.ajax({
        type: "POST",
        url: rootPath + 'Massive/RequestGrouping/GetAgencyByAgentIdAgencyId',
        data: JSON.stringify({ agentId: agentId, agencyId: agencyId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#inputAgentsAgent").data("Object").Code = data.result.Code;
            $("#inputAgentsAgent").data("Object").FullName = data.result.FullName;
            $("#inputAgentsAgent").data("Object").Branch = data.result.SalePoint.Branch;            
        }
        else {
            $("#selectAgentsAgency").UifSelect("setSelected", null);            
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        resultSave = false;
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAgents, 'autoclose': true });
    });
}

//Busca si ya se encuentra un intermediario                     
function existAgent(id, list) {
    var result = -1;
    for (var i = 0; i < list.length; i++) {        
        if (list[i].Agency.Agent.IndividualId != null && list[i].Agency.Agent.IndividualId == id && list[i].Agency.Id == $("#selectAgentsAgency").UifSelect('getSelected')) {
            result = i;
            break;
        }
    }
    return result;
}

//Carga la información del intermediario en modo solo lectura
function LoadListViewAgenciesReadOnly() {

    if (readOnlyRequest != null && readOnlyRequest.IsReadOnly) {
        $("#listAgencies").UifListView(
            {
                displayTemplate: "#agencyTemplate",
                addTemplate: '#add-template',
                height:250
            });

        if (agenciesTmp != null) {
            $.each(agenciesTmp, function (index, val) {
                $("#listAgencies").UifListView("addItem", agenciesTmp[index]);                
            });
            $('#selectedAgents').text(Resources.Language.LabelParticipants+": (" + agenciesTmp.length + ") "+Resources.Language.LabelCommission+": " + FormatMoney(comissionProduct) + "%");
        }
    }
}

//Carga la lista de intermediarios
function LoadListViewAgencies() {

    if (readOnlyRequest != null && readOnlyRequest.IsReadOnly) {
        LoadListViewAgenciesReadOnly();
        var participationTotal = calculateParticipation();
        $("#lblAgentsTotalParticipation").text(participationTotal + "%");
    }
    else {

        $('#listAgencies').UifListView({ displayTemplate: '#agencyTemplate', add: false, edit: true, delete: true, customEdit: true, customDelete: true, height: 250 });        
        var totalParticipation = 0;        
        if (requestGroup.CoRequestEndorsement[0].CoRequestAgent != null) {            
            $.each(requestGroup.CoRequestEndorsement[0].CoRequestAgent, function (index, value) {
                var totalAmount = 0;                
                totalParticipation += parseFloat(this.Agency.Participation.toString().replace(separatorDecimal, separatorThousands));
                this.Participation = FormatMoney(this.Agency.Participation);
                this.TotalAmount = FormatMoney(this.Agency.Amount);
                $('#listAgencies').UifListView('addItem', this);
            });

            $('#lblAgentsTotalParticipation').text(FormatMoney(totalParticipation)+"%");
        }
    }
}

//Evento al editar intermediario
$('#listAgencies').on('rowEdit', function (event, data, index) {
    isPrincipal = data.Agency.IsPrincipal;
    showAgent(data);
    agentIndex = index;
});

//Muestra la información de un intermediario seleccionado
function showAgent(data) {
    agentSearchType = 2;
    GetAgenciesByAgentIdDescription(data.Agency.Agent.IndividualId, data.Agency.Code);
    $("#inputAgentsParticipation").val(data.Agency.Participation);
}

//Agrega un nuevo intermediario o actualiza la información de uno existente
$("#btnAgentsAccept").on('click', function () { 
    $("#formAgents").validate();
    var isValid = $("#formAgents").valid();

    if (isValid) {
        if ($('#inputAgentsAgent').data('Object') == null) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateAgent, 'autoclose': true });
            return false;
        }
        pushAgent();
    }
});

//Limpia el formulario
function clearAgentForm() {
    $("#inputAgentsAgent").val("");
    $("#inputAgentsAgent").data("Object", null);
    var participationTotal = calculateParticipation();
    var participationAgent = (100 - parseFloat(participationTotal.toString().replace(separatorDecimal, separatorThousands)));

    $("#inputAgentsParticipation").val(participationAgent);

    $("#selectAgentsAgency").UifSelect({ source: null });
}

//Agrega un nuevo intermediario en el arreglo temporal
function pushAgent() {
    var list = $("#listAgencies").UifListView("getData");
    var result = existAgent($("#inputAgentsAgent").data("Object").Agent.IndividualId, list);

    if (result < 0) {
        if (parseFloat($("#lblAgentsTotalParticipation").text().replace(separatorDecimal, separatorThousands)) == 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
            return;
        }
        isPrincipal = false;
    }

    agent = {
        Agency: {
            Id: $('#selectAgentsAgency').UifSelect('getSelected'),            
            IsPrincipal: isPrincipal,
            Participation: $("#inputAgentsParticipation").val(),
            Agent: {
                IndividualId: $("#inputAgentsAgent").data("Object").Agent.IndividualId,
                FullName: $("#inputAgentsAgent").data("Object").FullName
            },
            Code: $("#inputAgentsAgent").data("Object").Code
        }
    };

    
    var participationTotal = calculateParticipation();


    if (result >= 0) {

        var list = $("#listAgencies").UifListView("getData");

        participationTotal = participationTotal - parseFloat(list[result].Agency.Participation.toString().replace(separatorDecimal, separatorThousands));
        participationTotal = participationTotal + parseFloat(agent.Agency.Participation.toString().replace(separatorDecimal, separatorThousands));

        if (participationTotal > 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
        }

        else {
            $("#listAgencies").UifListView("editItem", result, agent);
            clearAgentForm();
            $("#lblAgentsTotalParticipation").text(participationTotal + "%");
        }
    }
    else {
        participationTotal = participationTotal + parseFloat(agent.Agency.Participation.toString().replace(separatorDecimal, separatorThousands));

        if (participationTotal > 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
        }
        else {            
            $("#listAgencies").UifListView("addItem", agent);
            clearAgentForm();
            $("#lblAgentsTotalParticipation").text(participationTotal + "%");
        }
    }
}

//Calcula la participación de todos los intermediarios
function calculateParticipation() {
    var participationTotal = 0;

    var list = $("#listAgencies").UifListView("getData");

    for (var i = 0; i < list.length; i++) {
        participationTotal = participationTotal + parseFloat(list[i].Agency.Participation.toString().replace(separatorDecimal, separatorThousands));
    }

    return participationTotal;
}

//Calcula la participación intermediario principal
function calculateParticipationPrincipal(paticipationTmp) {
    var list = $("#listAgencies").UifListView("getData");

    for (var i = 0; i < list.length; i++) {
        if (list[i].Agency.IsPrincipal) {
            var participationTotal = parseFloat(list[i].Agency.Participation.toString().replace(separatorDecimal, separatorThousands)) + parseFloat((paticipationTmp).replace(separatorDecimal, separatorThousands));
            list[i].Agency.Participation = participationTotal;
            list[i].Participation = participationTotal;
            $("#listAgencies").UifListView("editItem", i, list[i]);            
            break;
        }
    }
}

//Guarda en memoria los cambios realizados
$("#btnAgentsSave").on('click',function () {

    var participationTotal = calculateParticipation();

    if (participationTotal > 100) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
    }
    else if (participationTotal < 100) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SumPercentagesNoGreater });
    }
    else {

        requestGroup.CoRequestEndorsement[0].CoRequestAgent = $("#listAgencies").UifListView("getData");

        var list = $("#listAgencies").UifListView("getData");
        $('#selectedAgents').text(Resources.Language.LabelParticipants + ": (" + list.length + ") " + Resources.Language.LabelCommission + ": " + FormatMoney(comissionProduct) + "%");
        RequestGrouping.HidePanels(MenuTypeGrouping.AGENTS);
    }
});


//Evento eliminar Intermediario
$('#listAgencies').on('rowDelete', function (event, data) {
    if (data.IsPrincipal) {
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
        calculateParticipationPrincipal(participationTmp);
        var participationTotal = calculateParticipation();
        $("#lblAgentsTotalParticipation").text(participationTotal + "%");        
    }
});

//Obtiene las comisiones
function GetComissionByProductId(productId) {
    $.ajax({
        type: "POST",
        url: rootPath + 'RequestGrouping/GetProductsByProductId',
        data: JSON.stringify({ productId: productId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            if (data.result) {
                comissionProduct = data.result.StandardCommissionPercentage;
                //$("#selectedAgents").text('Participantes: 1' + ' Comisión: '+  comissionProduct + '%');
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryCommission });
    });
}
