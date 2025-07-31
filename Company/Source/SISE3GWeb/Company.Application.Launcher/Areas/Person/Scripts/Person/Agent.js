var AgentCode = 0;
var agentModify = false;
var prefixTmp = [];
class Agent extends Uif2.Page {
    getInitialState() {
        new Agency();
        new ComissionAgency();
        
        Agent.initializeAgent();
        this.ControlsAgent(true);

        
        }

    //Seccion Eventos
    bindEvents() {
        $("#btnAgent").click(this.BtnAgent);
        $("#btnRecordAgent").click(this.SaveAgent);
        //Cargar Ramos           
        $("#linkPrefix").click(this.LoadPrefix);
        $("input[name=person]").change(this.AgentPerson);
        $('#InputAgentExecutive').on('buttonClick', this.SearchAgentExecutive);
        $('#tableResultsAgentExecutive tbody').on('click', 'tr', this.SelectSearchAgentExecutive);
        $('#agentDeclinedTypes').on('itemSelected', this.SelectedProviderDeclined)

    }
    SaveAgent()
    {
        
        var agent = {
            EmployeePerson: { Id: null }
        };
        agent.IndividualId = individualId;
        ///Se crea agente
        if (Agent.ValidateAgent()) {

            var dateDeclined = null;
            var dateModified = null;
            var typeDeclined = null;
            var fullName = "";
            if (searchType == TypePerson.PersonNatural) {
                fullName = $("#InputFirstNamePn").val() + " " + $("#InputLastNamePn").val() + " " + $("#InputNameIndividualPn").val()
            }
            if (searchType == TypePerson.PersonLegal) {
                fullName = $("#inputcheckpayable").val();
            }

            if ($("#InputDateDeclinedAgent").val() == "") {
                dateDeclined = dateDeclined;
            }
            else {
                dateDeclined = $("#InputDateDeclinedAgent").val();
            }
            if ($("#InputDateModifidAgent").val() == "" && !agentModify) {
                dateModified = dateModified;
            }
            else {
                dateModified = DateNowPerson;
            }
            if ($("#agentDeclinedTypes").UifSelect("getSelected") != null && $("#agentDeclinedTypes").UifSelect("getSelected") != "") {
                typeDeclined = $("#agentDeclinedTypes").val();
            }
            if ($('#agentDeclinedTypes').val() != null) {
                agent.AgentDeclinedTypeId = $('#agentDeclinedTypes').val();
            }

            agent.AgentTypeId = $('#typeIntermediary').val();
            if (agent.AgentDeclinedTypeId > 0) {
                agent.AgentDeclinedTypeId = $('#agentDeclinedTypes').val();
                agent.DateDeclined = $('#InputDateDeclinedAgent').val();
            }
            agent.DateCurrent = $('#InputDateCreateAgent').val();
            agent.DateModification = $('#InputDateModifidAgent').val();
            agent.Annotations = $('#notesAgent').val();
            agent.IdGroup = $('#AgentGroup').val();
            agent.IdChanel = $('#AgentSalesChannel').val();
            agent.FullNName = $('#AgentCheckPayableTo').val();
            agent.Locker = $('#AgentLocker').val();
            if ($("#InputAgentExecutive").val()) { agent.EmployeePerson = { Id: $("#InputAgentExecutive").data("Id") }; }
            else { agent.EmployeePerson = null; }

            if ($('#checkCommissionDiscount').is(':checked')) { agent.CommissionDiscountAgreement = $('#checkCommissionDiscount').is(':checked'); }
            else { agent.CommissionDiscountAgreement = 0; }

           
            
            //se obtiene datos de las agencias
            var agencyAgent = $("#listAgency").UifListView('getData');
            var agencyAgentFilter = agencyAgent.filter(function (item) {
                item.StatusTypeService = 3;
                return item.StatusTypeService > 1;
            });
            agent.Agencies = agencyAgentFilter;

            //se obitene daots de los ramos seleccionados
            var tmpPrefix = $("#tablePrefix").UifDataTable('getSelected')
            var datatableprefix = [];
            if (tmpPrefix != null) {
                $(tmpPrefix).each(function (ind, eve) {
                    datatableprefix.push({
                        Id: this.Id,
                        AgentId: individualId
                    });
                });
            }
            else {
                datatableprefix = null;
            }
            agent.Prefixes = datatableprefix;

            //se obtiene datos de las comiciones
            var ComissionAgents = $("#listAgentComisson").UifListView('getData');
            var ComissionAgentsFilter = ComissionAgents.filter(function (item) {
                return item.StatusTypeService > 1;
            });
            agent.ComissionAgents = ComissionAgentsFilter;

            lockScreen();
            AgentRequest.CreateAgent(agent).done(function (dataAgent) {
                if (dataAgent.success) {
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(dataAgent.result.InfringementPolicies, true);
                    let countAuthorization = dataAgent.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(dataAgent.result.InfringementPolicies, dataAgent.result.OperationId, FunctionType.PersonIntermediary);
                        }
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageCreateAgent, 'autoclose': true });
                        Agency.LoadAgencyRequest(agent.IndividualId);
                        ComissionAgency.LoadCommisiionRequest(agent.IndividualId);
                        Agent.ClearControlAgent();
                        $('#modalformAgent').UifModal('hide');
                        $('#checkAgent').prop('checked', true);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorCreateAgent, 'autoclose': true });
                }
                unlockScreen();
            }).fail(() => unlockScreen());
        }
        
        
        
    }

    SelectedProviderDeclined(event, selectedItem) {
        if (selectedItem.Id != "") {
            $("#InputDateDeclinedAgent").UifDatepicker("setValue", DateNowPerson);
        }
        else {
            $("#InputDateDeclinedAgent").UifDatepicker("clear");
        }
    }


    static initializeAgent() {
        $("#hidennRowId").val(0);
        $("#inputkey").ValidatorKey(ValidatorType.Number, 0, 0);
        $("#PercentageCommission").OnlyPercentage();
        $("#PercentageCommission").OnlyDecimals(2);
        $("#PercentageCommissionAditionals").OnlyPercentage;
        $("#PercentageCommissionAditionals").OnlyDecimals(2);
        $('#InputDateDeclined').datepicker('setStartDate', DateNowPerson);
        $("#AgentLocker").ValidatorKey(ValidatorType.Number, 0, 0);
      
        //Paneles
        $('#tablePrefix').UifDataTable('clear');
        Agent.ClearControlAgent();
        Agency.ClearControlAgency();
        ComissionAgency.ClearControlCommissionAgenct();
       
    }

    ControlsAgent(EnabledDisabled) {
        $('#InputDateDeclinedAgent').UifDatepicker('disabled', EnabledDisabled);
        $('#InputDateCreateAgent').UifDatepicker('disabled', EnabledDisabled);
        $('#InputDateModifidAgent').UifDatepicker('disabled', EnabledDisabled);
        $('#agentDeclinedTypes').prop('disabled', EnabledDisabled);
        $("#agentActive").prop("checked", EnabledDisabled);
        $("#active").prop("checked", EnabledDisabled);
    }

    async BtnAgent() {
        if (individualId == Person.New || individualId <= 0) {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
        else {

            var agentPromises = [];

            agentPromises.push(AgentRequest.GetAgentTypes());
            agentPromises.push(AgentRequest.GetGroupAgent());
            agentPromises.push(AgentRequest.GetSalesChannel());

            var data = await Promise.all(agentPromises);

            if (data[0].success) {
                $("#typeIntermediary").UifSelect({ sourceData: data[0].result });
            }
            if (data[1].success) {
                $("#AgentGroup").UifSelect({ sourceData: data[1].result });
            }
            if (data[2].success) {
                $("#AgentSalesChannel").UifSelect({ sourceData: data[2].result });
            }

            $('#tabsAgent a:first').tab('show');
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            Agent.initializeAgent();
            Agent.GetPrefixes();
            Agent.PrefixByIndividual(individualId);
            $("#CodPersonAgent").val(individualId);
            $("#CodPersonAgent").text(individualId);
            Agent.GetAgent(individualId);
            Persons.ShowPanelsPerson(RolType.Agent);
        }
    }

    RecordAgent() {
        if (Agent.CreateAgent()) {
            $('#modalformAgent').UifModal('hide');
        }
        else {
            return false;
        }
    }

    LoadPrefix() {
        if (individualId > 0) {
            Agent.PrefixByIndividual(individualId)
        }
    }

    AgentPerson() {
        if ($('input:radio[name=person]:checked').val() == "mdisabled") {
            $('#agentDeclinedTypes').prop('disabled', false);
            $('#InputDateDeclinedAgent').UifDatepicker('setValue', DateNowPerson);
        }
        else {
            $("#agentDeclinedTypes").UifSelect('setSelected', null);
            $('#agentDeclinedTypes').prop('disabled', true);
            $('#InputDateDeclinedAgent').val("");
        }
    }

    //Seccion Load Get
    static GetAgent(individualId) {
        $.UifProgress('show');
        AgentRequest.GetAgent(individualId).done(function (data) {
            $.UifProgress('close');
            if (data.success) {
                
                if (data.result != null) {
                    Agent.LoadAgent(data.result)                    
                }
                else {
                    $("#hidennRowId").val(0);
                    AgentCode = 0;
                    Agency.CleanObjectAgentAgency();
                    Agency.ClearControlAgency();
                    ComissionAgency.CleanObjectCommissionAgent();
                    ComissionAgency.ClearControlCommissionAgenct();
                }
            }
            else {
                AgentCode = 0;
                Agency.CleanObjectAgentAgency();
                Agency.ClearControlAgency();
                Agent.ClearControlAgent();
                ComissionAgency.CleanObjectCommissionAgent();
                ComissionAgency.ClearControlCommissionAgenct();
            }            
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });;
    }

    static LoadAgent(agent) {
        agentModify = false;
        if (agent.AgentType != null) {
            agentModify = true;
        } else {
            agentModify = false;
        }

        $("#hidennRowId").val(0);
        AgentCode = agent.Id;
        $('#AgentGroup').UifSelect("setSelected", agent.IdGroup);
        $('#AgentSalesChannel').UifSelect("setSelected", agent.IdChanel);
        $("#CodIntermediary").val(agent.Id);
        $("#CodIntermediary").text(agent.Id);
        if (agent.AgentTypeId != null) {
            $('#typeIntermediary').UifSelect("setSelected", agent.AgentTypeId);
        }
        $("#checkCommissionDiscount").attr('checked', agent.CommissionDiscountAgreement);
        $('#AgentLocker').val(agent.Locker);
        $('#AgentCheckPayableTo').val(agent.FullNName);
        $('#InputDateCreateAgent').UifDatepicker('setValue', FormatDate(agent.DateCurrent, 1));
        $("#InputDateCreateAgent").val(FormatDate(agent.DateCurrent));
        if (agent.EmployeePerson != null) {
            $("#InputAgentExecutive").data("Id");
            $("#InputAgentExecutive").val(agent.EmployeePerson.Name +  ' (' + agent.EmployeePerson.IdCardNo + ')');
        }
        
        if (agent.DateModification != null) {
            $("#InputDateModifidAgent").val(FormatDate(agent.DateModification, 1));
        }
        $("#notesAgent").val(agent.Annotations);
        if (agent.AgentDeclinedTypeId != null) {
            if (agent.AgentDeclinedTypeId == 0) {
                $("#agentActive").prop('checked', true);
                $("#InputDateDeclinedAgent").val("");
            }
            else {
                $("#downAgent").prop('checked', true);
                $("#agentDeclinedTypes").UifSelect('setSelected', agent.AgentDeclinedTypeId);
                $('#InputDateDeclinedAgent').UifDatepicker('setValue', FormatDate(agent.DateDeclined, 1));
            }
        }
        
        //Agencias   

        Agency.LoadAgencyRequest(agent.IndividualId); 
        Agency.CleanObjectAgentAgency();
        //Agency.FormatAgency();

        //Comisiones
        ComissionAgency.CleanObjectCommissionAgent();
        //ComissionAgent = agent.ComissionAgent;
        //ComissionAgency.FormatAgentCommission();
        ComissionAgency.LoadCommisiionRequest(agent.IndividualId);
       // ComissionAgency.LoadAgentComission();
    }

    static GetPrefixes() {
        PrefixRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                var datatableprefix = [];
                $(data.result).each(function (ind, eve) {
                    datatableprefix.push({
                        Id: this.Id,
                        Description: this.Description
                    });

                });
                $('#tablePrefix').UifDataTable('clear')
                $('#tablePrefix').UifDataTable('addRow', datatableprefix);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });                
            }
        });
    }

    static PrefixByIndividual(IndividualId) {
        AgentRequest.GetAgentPrefix(IndividualId).done(function (data) {
            if (data.success) {
                var dataTable = $("#tablePrefix").UifDataTable('getData');
                $.each(dataTable, function (key, value) {
                    $.each(data.result, function (key1, value1) {
                        if (value.Id == value1.Id) {
                            $('#tablePrefix tbody tr:eq(' + key + ')').addClass('row-selected');
                            $('#tablePrefix tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
                            return;
                        }
                    })
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ClearControlAgent() {
        
        $("#CodPersonAgent").text($("#lblPersonCode").val());
        $("#CodPersonAgent").val($("#lblPersonCode").val());
        $("#CodIntermediary").val("");
        $("#CodIntermediary").text("");
        $("#typeIntermediary").val("");
        $("#agentDeclinedTypes").UifSelect('setSelected', null);
        $("#InputDateDeclinedAgent").val("");
        $("#InputDateCreateAgent").UifDatepicker('setValue', DateNowPerson);
        $("#InputDateModifidAgent").val("");
        $("#notesAgent").val("");
        $("#AgentGroup").val("");
        $("#InputAgentExecutive").val("");
    }


    //Seccion Grabado
    static CreateAgent() {
        if (Agent.ValidateAgent()) {
            Agency.SaveAgencies();
            ComissionAgency.SaveAgentComission();
            prefixTmp = $('#tablePrefix').UifDataTable('getSelected');
            var dateDeclined = null;
            var dateModified = null;
            var typeDeclined = null;
            var fullName = "";
            if (searchType == TypePerson.PersonNatural) {
                fullName = $("#InputFirstNamePn").val() + " " + $("#InputLastNamePn").val() + " " + $("#InputNameIndividualPn").val()
            }
            if (searchType == TypePerson.PersonLegal) {
                fullName = $("#inputcheckpayable").val();
            }

            if ($("#InputDateDeclinedAgent").val() == "") {
                dateDeclined = dateDeclined;
            }
            else {
                dateDeclined = $("#InputDateDeclinedAgent").val();
            }
            if ($("#InputDateModifidAgent").val() == "" && !agentModify) {
                dateModified = dateModified;
            }
            else {
                dateModified = DateNowPerson;
            }
            if ($("#agentDeclinedTypes").UifSelect("getSelected") != null && $("#agentDeclinedTypes").UifSelect("getSelected") != "") {
                typeDeclined = $("#agentDeclinedTypes").val();
            }

            //Ramos       
            var datatableprefix = [];
            var tmpPrefix = $("#tablePrefix").UifDataTable('getSelected')
            if (tmpPrefix != null) {
                $(tmpPrefix).each(function (ind, eve) {
                    datatableprefix.push({
                        Id: this.Id,
                        Description: this.Description
                    });

                });
            }
            else {
                datatableprefix = null;
            }

            //Alliance       
            var dataAlliance = glbAlliancesToDelete;
            var tmpAlliance = $("#listAlliance").UifListView('getData');
            if (tmpAlliance != null) {
                $(tmpAlliance).each(function (ind, eve) {
                    if (this.Status != undefined) {
                        dataAlliance.push({
                            IndividualId: this.IndividualId,
                            AgencyAgencyId: this.Id,
                            AllianceId: this.AllianceId,
                            IsSpecialImpression: this.IsSpecialImpression,
                            Status: this.Status
                        });
                    }
                });
                glbAlliancesToDelete = [];
            }
            else {
                dataAlliance = null;
            }
            fullName = $("#AgentCheckPayableTo").val();	    
	    var agentTmp = { IndividualId: individualId, DateCurrent: $("#InputDateCreateAgent").val(), DateDeclined: dateDeclined, AgentDeclinedType: { Id: typeDeclined }, AgentType: { Id: $("#typeIntermediary").val() }, FullName: fullName, DateModification: dateModified, Annotations: $("#notesAgent").val(), GroupAgent: { Id: $("#AgentGroup").val() }, SalesChannel: { Id: $("#AgentSalesChannel").val() }, Locker: $("#AgentLocker").val(), EmployeePerson: { Id: $("#InputAgentExecutive").data("Id") }, Agencies: Agencies, Prefixes: datatableprefix, AgentAgencies: dataAlliance, ComissionAgent: ComissionAgent}
            AgentRequest.CreateAgent(agentTmp).done(function (data) {
                if (data.success) {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.MessageInformation });
                    $('#checkAgent').prop('checked', true);
                    $('#checkAgent').addClass('primary');

                    $('#btnAgent').trigger('click');
                    if (data.result.Prefixes != null) {
                        for (var i = 0; i < data.result.Prefixes.length; i++) 
                            $.UifNotify('show', { 'type': 'danger', 'message': 'El Ramo ' + data.result.Prefixes[i].Description + ' no se puede eliminar', 'autoclose': true });                    
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });                    
                }
            });
        }
    }

    //seccion Validaciones
    static ValidateAgent() {
        var msj = "";
        if ($("#typeIntermediary").UifSelect("getSelected") == null || $("#typeIntermediary").UifSelect("getSelected") == "") {
            msj = AppResourcesPerson.LabelTypeIntermediary + "<br>"
        }
        if ($("#downAgent").is(':checked')) {
            if ($("#InputDateDeclinedAgent").val() == "") {
                msj = msj + AppResourcesPerson.LabelDeclinedDate + "<br>"
            }
            if ($("#agentDeclinedTypes").UifSelect("getSelected") == null || $("#agentDeclinedTypes").UifSelect("getSelected") == "") {
                msj = msj + AppResourcesPerson.LabelReasonLow + "<br>"
            }
        }

        if ($("#AgentCheckPayableTo").val() == "") {
            msj = msj + AppResourcesPerson.LabelCheckNameOf + "<br>"
        }

        if ($("#AgentGroup").UifSelect("getSelected") == null || $("#AgentGroup").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.LabelAgentGroup + "<br>"
        }

        if ($("#AgentSalesChannel").UifSelect("getSelected") == null || $("#AgentSalesChannel").UifSelect("getSelected") == "") {
            msj = msj + AppResourcesPerson.LabelAgentChannel + "<br>"
        }

        if ($("#AgentLocker").val() == "") {
            msj = msj + AppResourcesPerson.LabelAgentLocker + "<br>"
        }

        if ($("#listAgency").UifListView('getData').length < 1) {
            msj = msj + AppResourcesPerson.LabelIntermediaryAgencies + "<br>"
        }

        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true })
            return false;
        }
        return true;
    }

    static HideControl() {
        HidePanelsPerson();
        ShowPanelsPerson(RolType.People);
        switch (searchType) {
            case TypePerson.PersonNatural:
                $("#panelPersonNatural").show();
                break;
            case TypePerson.PersonLegal:
                $("#panelPersonLegal").show();
                break;
            default:
                break;
        }
    }

    SearchAgentExecutive() {
        if ($('#InputAgentExecutive').val().trim() != "" && $('#InputAgentExecutive').val().trim() != 0) {
            Agent.GetAgentExecutive($('#InputAgentExecutive').val());
        }
    }
    static GetAgentExecutive(description) {
        if (description.length >= 3)
        {
            AgentRequest.GetEmployeePersonByFullName(description).done(function (data)
        {
            if (data.success && data.result.length > 0) {
                if (data.result.length == 1) {
                    $("#InputAgentExecutive").val(data.result[0].Name + ' ' + data.result[0].MotherLastName + ' (' + data.result[0].IdCardNo + ')');
                    $("#InputAgentExecutive").data("Id", data.result[0].Id)
                }
                else {
                    modalListType = 1;
                    var dataList = {
                        dataObject: []
                    };

                    for (var i = 0; i < data.result.length; i++) {
                        dataList.dataObject.push({
                            IdCardNo: data.result[i].IdCardNo,
                            Name: data.result[i].Name + ' ' + data.result[i].MotherLastName,
                            IndividualId: data.result[i].Id
                        });
                    }
                    Agent.ShowModalList(dataList.dataObject);
                    $('#modalDialogListAgentExecutive').UifModal('showLocal', AppResourcesPerson.SelectAccountExecutive);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                $("#InputAgentExecutive").val('');
                $("#InputAgentExecutive").focus();
            }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageInfoMinimumChar, 'autoclose': true })
        }
    }

    static ShowModalList(dataTable) {
        $('#tableResultsAgentExecutive').UifDataTable('clear');
        var table = $('#tableResultsAgentExecutive').DataTable();
        table.on('draw', function () {
            $('#tableResults tbody td:nth-child(1)').hide();
            $('#tableResults thead th:eq(0)').hide();
        });
        $('#tableResultsAgentExecutive').UifDataTable('addRow', dataTable);
    }

    SelectSearchAgentExecutive() {
        var $this = this;
        var data = $('#tableResultsAgentExecutive').UifDataTable('getData');
        var row = data.filter(function (record) {
            return record.IdCardNo == $($this).children()[0].innerHTML
        })
        switch (modalListType) {
            case 1:
                $('#InputAgentExecutive').val(row[0].Name + ' (' + row[0].IdCardNo + ')');
                $("#InputAgentExecutive").data("Id", row[0].IndividualId);                
                break;
            default:
                break;
        }
        $('#modalDialogListAgentExecutive').UifModal("hide");
    }

   
    
    
}