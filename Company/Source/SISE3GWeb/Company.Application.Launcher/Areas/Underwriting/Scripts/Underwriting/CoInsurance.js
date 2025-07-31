//Codigo de la pagina CoInsurance.cshtml
var coInsureType = 1;
var CoInsAssignedIndex = null;
var AgentJs = {};
AgentJs = {
    Id: "",
    Description: "",
    Participation: "",
    IndividualId: ""
};

class UnderwritingCoInsurance extends Uif2.Page {
    getInitialState() {
        $("#inputCoInsAssignedInsured").UifAutoComplete({
            source: rootPath + "Underwriting/Underwriting/GetCoInsuranceCompaniesByDescription",
            displayKey: "Description",
            queryParameter: "&query"
        });

        $("#inputCoInsAcceptedLeadingInsurer").UifAutoComplete({
            source: rootPath + "Underwriting/Underwriting/GetCoInsuranceCompaniesByDescription",
            displayKey: "Description",
            queryParameter: "&query"
        });

        UnderwritingCoInsurance.GetBusinessTypes();
        UnderwritingCoInsurance.initialize();
        //$("#listCoInsuranceAcceptedAgents").UifListView({ displayTemplate: "#CoInsuranceAcceptedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        //$('#panelAccepted').hide();
        //$('.panelAssigned').hide();
        $('#inputCoInsAcceptedLeadingInsurer').ValidatorKey(3, 1, 1);
        $('#inputCoInsAcceptedParticipation').OnlyDecimals(UnderwritingDecimal);
        $('#inputCoInsAcceptedParticipationTotal').OnlyDecimals(UnderwritingDecimal);
        $('#inputCoInsAcceptedExpenses').OnlyPercentage();
        $('#inputCoInsAcceptedLeadingPolicy').ValidatorKey(6, 1, 1);
        $('#inputCoInsAcceptedEndorsement').ValidatorKey(6, 1, 1);
        $('#inputCoInsAssignedParticipation').OnlyPercentage();
        $('#inputCoInsAcceptedAgentParticipation').OnlyPercentage();
        $('#inputCoInsAssignedExpenses').OnlyPercentage();
        UnderwritingCoInsurance.ClearPanelAccepted();
        UnderwritingCoInsurance.ClearPanelAssigned();
        UnderwritingCoInsurance.CoinsuredAssingList();
        //UnderwritingCoInsurance.CoinsuredAcceptedAgentList();

    }

    //Seccion Eventos
    bindEvents() {
        $("#btnCoInsurance").on('click', UnderwritingCoInsurance.CoInsuranceLoad);
        $('#selectCoInsBusinessType').on('itemSelected', UnderwritingCoInsurance.ChangeCoInsBusinessType);
        //Aceptado
        $('#inputCoInsAcceptedLeadingInsurer').on('itemSelected', UnderwritingCoInsurance.ChangeCoInsAccepted);
        $("#btnCoInsAcceptedAgent").on("click", UnderwritingCoInsurance.CoInsAcceptedAgent);
        $("#btnCoInsAcceptedAgentNew").on("click", UnderwritingCoInsurance.NewAgent);
        $('#inputCoInsAcceptedParticipation').focusout(UnderwritingCoInsurance.CalculateAcceptedPercentage);
        $('#inputAccepAgentsAgency').on('itemSelected', UnderwritingCoInsurance.ChangeCoInsAcceptedAgent);
        $('#inputAccepAgentsAgency').on('buttonClick', UnderwritingCoInsurance.SearchAgentsAgency);
        $('#tblResultListAcceptedAgents').on('rowSelected', UnderwritingCoInsurance.SelectSearch);

        //Cedido
        $('#inputCoInsAssignedInsured').on('itemSelected', UnderwritingCoInsurance.ChangeCoInsAssigned);
        $('#btnCoInsAssignedAccept').on("click", UnderwritingCoInsurance.CoInsAssignedAccept);
        $("#btnCoInsuranceSave").on("click", UnderwritingCoInsurance.CoInsuranceSave);
    }

    static SelectSearchAGent(event, data, position) {
        $("#inputAccepAgentsAgency").data("Object", data.result[0]);
        $("#inputAccepAgentsAgency").val(data.result[0].FullName);
    }


    static SelectSearch(event, data, position) {
        $("#inputAccepAgentsAgency").data("Object", data);
        $("#inputAccepAgentsAgency").val(data.FullName);
        $('#modalListAcceptedAgents').UifModal('hide');

    }
    static SearchAgentsAgency() {
        agentSearchType = 2;
        UnderwritingCoInsurance.GetAgenciesByAgentIdDesciptionProductId1(0, $('#inputAccepAgentsAgency').val().trim(), glbPolicy.Product.Id);
    }

    static GetAgenciesByAgentIdDesciptionProductId1(agentId, description, productId) {
        if (agentId != undefined || description != undefined) {
            var number = parseInt(description, 10);
            if ((!isNaN(number) || description.length > 2) && (description != 0)) {
                UnderwritingRequest.GetAgentsByDescription(agentId, description, productId).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            $("#inputAccepAgentsAgency").data("Object", data.result[0]);
                            $("#inputAccepAgentsAgency").val(data.result[0].FullName);
                        }
                        else if (data.result.length > 1) {
                            var dataList = [];
                            var uniqueList = [];
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    IndividualId: data.result[i].IndividualId,
                                    Id: data.result[i].Id,
                                    FullName: data.result[i].FullName,
                                    AgentId: data.result[i].AgentId
                                });
                            }
                            uniqueList = dataList.filter((obj, pos, arr) => {
                                return arr.map(mapObj =>
                                    mapObj.FullName).indexOf(obj.FullName) === pos;
                            });

                            UnderwritingCoInsurance.ShowDefaultResults(uniqueList);
                            $('#modalListAcceptedAgents').UifModal('showLocal', AppResources.LabelAgentPrincipal);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorNotAgents, 'autoclose': true });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }

    static ShowDefaultResults(dataTable) {
        $('#tblResultListAcceptedAgents').UifDataTable('clear');
        $('#tblResultListAcceptedAgents').UifDataTable('addRow', dataTable);
    }

    static NewAgent() {
        UnderwritingCoInsurance.ClearAcceptedAgent();
    }

    static CoInsuranceLoad() {
        
        if (glbPolicy.Id == 0 && glbPolicy.TemporalType != TemporalType.Quotation) {
            if ($("#formUnderwriting").valid()) {
                Underwriting.SaveTemporalPartial(MenuType.CoInsurance);
            }
        }
        else {
            UnderwritingCoInsurance.LoadPartialCoinsurance();

        }
        UnderwritingCoInsurance.ClearAssignedInsured();
    }


    static ChangeCoInsBusinessType(event, selectedItem) {

        UnderwritingCoInsurance.ClearForm();
        event.stopImmediatePropagation();
        event.preventDefault();
        coInsureType = selectedItem.Id;
        UnderwritingCoInsurance.ShowPanelBusinessType(selectedItem.Id);
        $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
    }

    static ChangeCoInsAccepted(event, selectedItem) {
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', selectedItem.Id);
    }

    static ChangeCoInsAcceptedAgent(event, selectedItem) {
        $('#inputAccepAgentsAgency').data('Id', selectedItem.Id);
        $('#inputAccepAgentsAgency').data('Description', selectedItem.Description);
    }

    static ChangeCoInsAssigned(event, selectedItem) {
        $('#inputCoInsAssignedInsured').data('Id', selectedItem.Id);
    }

    static CoInsAssignedAccept() {

        if ($("#inputCoInsAssignedParticipation").val() >= 100) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateCoInsAssignedpercentage, 'autoclose': true });
            return false;
        }

        if ($("#inputCoInsAssignedParticipation").val() <= 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMajorOwnParticipationPercentage, 'autoclose': true });
            return false;
        }

        if (CoInsAssignedIndex == null) {
            if (!UnderwritingCoInsurance.ValidateCoInsuranceAssigned($('#inputCoInsAssignedInsured').data('Id'))) {
                if (UnderwritingCoInsurance.CalculateAssingPercentage(false, -1)) {
                    UnderwritingCoInsurance.addItemCoInsAssignedExpenses();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateCoInsurance, 'autoclose': true });
            }
        }
        else {
            if (UnderwritingCoInsurance.CalculateAssingPercentage(false, CoInsAssignedIndex)) {
                $("#listCoInsuranceAssigned").UifListView("editItem", CoInsAssignedIndex, UnderwritingCoInsurance.GetCoInsAssignedExpenses());
                var total = UnderwritingCoInsurance.CalculateCoInsuranceAssignedTotalAmount();
                UnderwritingCoInsurance.ClearAssignedInsured();
            }
        }

    }

    static CoInsAcceptedAgent() {
        if ($('#inputCoInsAcceptedAgentParticipation').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        //if ($("#listCoInsuranceAcceptedAgents").UifListView('getData').length > 10) {
        //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MoreThan10Intermediates, 'autoclose': true });
        //    return false;
        //}

        if ($("#inputAgentsAgencyParticipation").val() >= 100) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateCoInsAssignedpercentage, 'autoclose': true });
            return false;
        }

        if (CoInsAssignedIndex == null) {
            if (!UnderwritingCoInsurance.ValidateCoInsuranceAcceptedAgent($('#inputAccepAgentsAgency').data("Object").Id, $('#inputAccepAgentsAgency').data("Object").IndividualId)) {
                if (UnderwritingCoInsurance.CalculateAssingAgentPercentage(false, -1)) {
                    UnderwritingCoInsurance.addItemCoInsAcceptedAgent();
                    $("#inputCoInsAcceptedAgenTotalPercentage").text(UnderwritingCoInsurance.GetPercentajeListAgents(false, -1));
                    UnderwritingCoInsurance.ClearAcceptedAgent();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.Agent, 'autoclose': true });
            }
        }
        //else {
        //    if (UnderwritingCoInsurance.CalculateAssingAgentPercentage(false, CoInsAssignedIndex)) {
        //        $.each($("#listCoInsuranceAcceptedAgents").UifListView('getData'), function (key, value) {
        //            if (key == CoInsAssignedIndex) {
        //                AgentJs.Id = this.Id;
        //                AgentJs.IndividualId = this.IndividualId;
        //                AgentJs.Description = this.Description;
        //                AgentJs.Participation = parseFloat($('#inputCoInsAcceptedAgentParticipation').val().replace(separatorDecimal, separatorThousands));
        //            }
        //        })
        //        $("#listCoInsuranceAcceptedAgents").UifListView("editItem", CoInsAssignedIndex, UnderwritingCoInsurance.GetCoInsAcceptedAgent());
        //        $("#inputCoInsAcceptedAgenTotalPercentage").text(UnderwritingCoInsurance.GetPercentajeListAgents(false, -1));
        //        UnderwritingCoInsurance.ClearAcceptedAgent();
        //    }
        //}

    }


    static CoInsuranceSave() {
        glbPolicy.BusinessType = $('#selectCoInsBusinessType').UifSelect("getSelected");
        if (coInsureType == BusinessType.Accepted) {
            var percentageOrigin = parseFloat($('#inputCoInsAcceptedParticipationTotal').val().replace(separatorDecimal, separatorThousands));
            var percentage = parseFloat($('#inputCoInsAcceptedParticipation').val().replace(separatorDecimal, separatorThousands));
            if (percentageOrigin + percentage > 100) {
                $('#inputCoInsAcceptedLeadingPolicy').ValidatorKey(6, 1, 1);
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMajorPercentage, 'autoclose': true });
                return false;
            }
        }
        UnderwritingCoInsurance.SaveCoInsurance();
        if (glbPolicy.TemporalType == TemporalType.Quotation) {
            Underwriting.SaveTemporal(false);
        }
    }

    static LoadPartialCoinsurance() {
        $("#formChangeCoinsurance").validate();
        if (glbPolicy.Endorsement.PolicyId > 0 && $("#formChangeCoinsurance").valid()) {
            Underwriting.ShowPanelsIssuance(MenuType.CoInsurance);
            UnderwritingCoInsurance.GetBusinessTypes();
            UnderwritingCoInsurance.LoadCoinsurance();
            //$("#selectCoInsBusinessType").prop("disabled", true);
            //$("#selectCoInsBusinessType").UifSelect("setSelected", glbPolicy.BusinessType);
        } else {
            $("#formUnderwriting").validate();
            if (glbPolicy.Id > 0 && $("#formUnderwriting").valid()) {
                Underwriting.ShowPanelsIssuance(MenuType.CoInsurance);

                $("#selectCoInsBusinessType").UifSelect("setSelected", glbPolicy.BusinessType);
                $("#hiddenCoinsuranceId").val(glbPolicy.Id);

                if (glbPolicy.CoInsuranceCompanies != undefined) {
                    UnderwritingCoInsurance.LoadCoinsurance();
                }
                else {
                    $('#selectCoInsBusinessType').val(Underwriting.GetParameterByDescription('BusinessType'));
                }
            }

        }

    }

    static LoadCoinsurance() {
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
        switch (parseInt(glbPolicy.BusinessType)) {
            case BusinessType.Accepted:
                $('#panelAccepted').show();
                //$("#listCoInsuranceAcceptedAgents").UifListView("refresh");
                UnderwritingCoInsurance.LoadCoinsuranceAccepted();
                break;
            case BusinessType.Assigned:
                $('.panelAssigned').show();
                $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
                UnderwritingCoInsurance.LoadCoinsuranceAssigned();
                break;
            default:
                break;
        }
    }
    //Llena inicial
    static LoadCoinsuranceAccepted() {
        $('#inputCoInsAcceptedParticipationTotal').val(glbPolicy.CoInsuranceCompanies[0].ParticipationPercentageOwn);
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', glbPolicy.CoInsuranceCompanies[0].Id);
        $('#inputCoInsAcceptedLeadingInsurer').val(glbPolicy.CoInsuranceCompanies[0].Description);
        $('#inputCoInsAcceptedParticipation').val(glbPolicy.CoInsuranceCompanies[0].ParticipationPercentage);
        $('#inputCoInsAcceptedExpenses').val(glbPolicy.CoInsuranceCompanies[0].ExpensesPercentage);
        $('#inputCoInsAcceptedLeadingPolicy').val(glbPolicy.CoInsuranceCompanies[0].PolicyNumber);
        $('#inputCoInsAcceptedEndorsement').val(glbPolicy.CoInsuranceCompanies[0].EndorsementNumber);

        //var AccepteAgents = {};
        //CoInsAssignedIndex = null;
        //$.each(glbPolicy.CoInsuranceCompanies[0].acceptCoInsuranceAgent, function (key, value) {
        //    AccepteAgents = {
        //        Id: this.Agent.Id,
        //        IndividualId: this.Agent.IndividualId,
        //        Description: this.Agent.FullName,
        //        Participation: this.ParticipationPercentage
        //    };
        //    $("#listCoInsuranceAcceptedAgents").UifListView("addItem", AccepteAgents);
        //});
        //$("#inputCoInsAcceptedAgenTotalPercentage").text(UnderwritingCoInsurance.GetPercentajeListAgents(false, -1));
    }


    static LoadCoinsuranceAssigned() {
        $('#inputCoInsAssignedParticipationTotal').val(glbPolicy.CoInsuranceCompanies[0].ParticipationPercentageOwn);
        var assignedCompanies = [];
        CoInsAssignedIndex = null;
        $.each(glbPolicy.CoInsuranceCompanies, function (key, value) {
            var assignedCompanies = {};
            assignedCompanies =
                {
                    Id: this.Id,
                    Description: this.Description,
                    Participation: this.ParticipationPercentage,
                    Expenses: this.ExpensesPercentage
                };
            $("#listCoInsuranceAssigned").UifListView("addItem", assignedCompanies);
        });
    }

    static ShowPanelBusinessType(businessTypeId) {
        UnderwritingCoInsurance.ClearPanelAccepted();
        UnderwritingCoInsurance.ClearPanelAssigned();
        switch (businessTypeId) {
            case '1':
                $('#panelAccepted').hide();
                $('.panelAssigned').hide();
                break;
            case '2':
                $('#panelAccepted').show();
                $('.panelAssigned').hide();
                break;
            case '3':
                $('#panelAccepted').hide();
                $('.panelAssigned').show();
                UnderwritingCoInsurance.LoadCoinsuranceAssigned();
                break;
            default:
                $('#panelAccepted').hide();
                $('.panelAssigned').hide();
        }
    }

    static CalculateAcceptedPercentage() {

        //Se Valida Solo Si es Mayor a 100
        var percentageOrigin = parseFloat($('#inputCoInsAcceptedParticipationTotal').val().replace(separatorDecimal, separatorThousands));
        if ($('#inputCoInsAcceptedParticipation').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateValueParticipationPercentage, 'autoclose': true });
            return false;
        }
        else {

            var percentage = parseFloat($('#inputCoInsAcceptedParticipation').val().replace(separatorDecimal, separatorThousands));

            if (percentage + percentageOrigin > 100 || isNaN(percentage)) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMinorPercentage, 'autoclose': true });
            }
        }
    }

    static CalculateAssingPercentage(isEdit, Id) {

        var percentage = 0;

        var total = parseFloat($('#inputCoInsAssignedParticipation').val().replace(separatorDecimal, separatorThousands));
        var percentageTotal = UnderwritingCoInsurance.GetPercentajeTotal(isEdit, Id);

        if (percentage + total + percentageTotal >= 100 || (percentage + percentageTotal) >= 100 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        else {
            $('#inputCoInsAssignedParticipationTotal').val(100 - (total + percentage + percentageTotal));

            return true;
        }
    }

    //static CalculateAssingAgentPercentage(isEdit, Id) {



    //    //var percentage = parseFloat($('#inputCoInsAssignedParticipationTotal').val().replace(separatorDecimal, separatorThousands));
    //    var total = parseFloat($('#inputCoInsAcceptedAgentParticipation').val().replace(separatorDecimal, separatorThousands));
    //    var percentageTotal = UnderwritingCoInsurance.GetPercentajeListAgents(isEdit, Id);
    //    //$('#inputCoInsAssignedParticipationTotal').val(100 - (percentage + percentageTotal));

    //    if (total + percentageTotal > 100 || isNaN(total)) {
    //        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValidateParticipationPercentage, 'autoclose': true });
    //        return false;
    //    }
    //    else {
    //        //$('#inputCoInsAssignedParticipationTotal').val(100 - (total + percentage + percentageTotal));
    //        //$('#inputCoInsAssignedParticipationTotal').val();
    //        return true;
    //    }
    //}

    static GetPercentajeTotal(isEdit, Id) {
        var percentage = 0;

        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (key, value) {
            if (key != Id) {
                percentage += parseFloat(this.Participation);
            }
        });

        return percentage;
    }

    //static GetPercentajeListAgents(isEdit, Id) {
    //    var percentage = 0;

    //    $.each($("#listCoInsuranceAcceptedAgents").UifListView('getData'), function (key, value) {
    //        if (key != Id) {
    //            percentage = parseFloat(String(percentage).replace(separatorDecimal, separatorThousands)) + parseFloat(this.Participation);
    //        }
    //    })
    //    if (percentage == 100) {
    //        $('#btnCoInsAcceptedAgent').attr('disabled', true);
    //    }
    //    else {
    //        $('#btnCoInsAcceptedAgent').attr('disabled', false);
    //    }
    //    return percentage;
    //}

    static addItemCoInsAssignedExpenses() {
        if (UnderwritingCoInsurance.ValidDataTable()) {
            $("#listCoInsuranceAssigned").UifListView("addItem", UnderwritingCoInsurance.GetCoInsAssignedExpenses());
            UnderwritingCoInsurance.ClearAssignedInsured();
        }
    }

    //static addItemCoInsAcceptedAgent() {
    //    if (UnderwritingCoInsurance.ValidAgent()) {
    //        $("#listCoInsuranceAcceptedAgents").UifListView("addItem", UnderwritingCoInsurance.GetCoInsAcceptedAgent());
    //        UnderwritingCoInsurance.ClearAcceptedAgent();
    //    }
    //}

    static GetCoInsAssignedExpenses() {
        var inputCoInsAssignedInsured = {
            Id: $('#inputCoInsAssignedInsured').data('Id'),
            Description: $('#inputCoInsAssignedInsured').val(),
            Participation: $('#inputCoInsAssignedParticipation').val(),
            Expenses: $('#inputCoInsAssignedExpenses').val()
        };
        return inputCoInsAssignedInsured;
    }
    static GetCoInsAcceptedAgent() {
        if (CoInsAssignedIndex == null) {
            AgentJs.Id = $('#inputAccepAgentsAgency').data("Object").Id;
            AgentJs.Description = $('#inputAccepAgentsAgency').data("Object").FullName;
            AgentJs.Participation = parseFloat($('#inputCoInsAcceptedAgentParticipation').val().replace(separatorDecimal, separatorThousands));
            AgentJs.IndividualId = $('#inputAccepAgentsAgency').data("Object").IndividualId;
        }

        var inputCoInsAssignedInsured = {
            Id: AgentJs.Id,
            Description: AgentJs.Description,
            Participation: AgentJs.Participation,
            IndividualId: AgentJs.IndividualId,
        };
        return inputCoInsAssignedInsured;
    }

    static ClearPanelAccepted() {
        //$("#listCoInsuranceAcceptedAgents").UifListView("refresh");
        $('#inputCoInsAcceptedParticipationTotal').val('');
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', null);
        $('#inputCoInsAcceptedLeadingInsurer').val('');
        $('#inputCoInsAcceptedParticipation').val('');
        $('#inputCoInsAcceptedExpenses').val('');
        $('#inputCoInsAcceptedLeadingPolicy').val('');
        $('#inputCoInsAcceptedEndorsement').val('');
    }

    static ClearPanelAssigned() {
        CoInsAssignedIndex = null;
        $('#inputCoInsAssignedParticipationTotal').val('');
        $('#inputCoInsAssignedInsured').data('Id', null);
        $('#inputCoInsAssignedInsured').val('');
        $('#inputCoInsAssignedParticipation').val('');
        $('#inputCoInsAssignedExpenses').val('');
    }


    static ClearAssignedInsured() {
        CoInsAssignedIndex = null;
        $('#inputCoInsAssignedInsured').data('Id', null);
        $('#inputCoInsAssignedInsured').val('');
        $('#inputCoInsAssignedParticipation').val('');
        $('#inputCoInsAssignedExpenses').val('');
    }

    //static ClearAcceptedAgent() {
    //    CoInsAssignedIndex = null;
    //    $('#inputAccepAgentsAgency').prop('disabled', '');
    //    $('#inputAccepAgentsAgency').data('');
    //    $('#inputAccepAgentsAgency').val('');
    //    $('#inputCoInsAcceptedAgentParticipation').val('');

    //   /* $('#inputCoInsAcceptedParticipationTotal').val('');
    //    $('#inputCoInsAcceptedParticipation').val('');
    //    $('#inputCoInsAcceptedExpenses').val('');
    //    $('#inputCoInsAcceptedLeadingPolicy').val('');
    //    $('#inputCoInsAcceptedLeadingInsurer').val('');*/
    //}


    static ValidDataTable() {

        if ($('#inputCoInsAssignedInsured').data('Id') == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateCoInsurance, 'autoclose': true });
            return false;
        }
        if ($('#inputCoInsAssignedParticipation').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        if ($('#inputCoInsAssignedExpenses').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateExpensesPercentage, 'autoclose': true });
            return false;
        }

        if (parseFloat($('#inputCoInsAssignedExpenses').val().replace(separatorDecimal, separatorThousands)) >= 100) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateExpensesPercentage, 'autoclose': true });
            return false;
        }
        return true;
    }

    static ValidAgent() {

        if ($('#inputAccepAgentsAgency').data('Object').Id == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateCoInsurance, 'autoclose': true });
            return false;
        }
        if ($('#inputCoInsAcceptedAgentParticipation').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateParticipationPercentage, 'autoclose': true });
            return false;
        }

        return true;
    }

    static SaveCoInsurance() {

        $("#formCoInsurance").validate();
        if (!UnderwritingCoInsurance.ValidateGeneral($('#selectCoInsBusinessType').UifSelect("getSelected"))) {
            return false;
        }
        var valid = true;

        if ($('#selectCoInsBusinessType').UifSelect("getSelected") == BusinessType.Accepted) {
            $("#formCoInsurance").validate();
            valid = $("#formCoInsurance").valid();

            //if ((UnderwritingCoInsurance.GetPercentajeListAgents(false, -1)) < 100) {
            //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
            //    return false;
            //}

        }
        else {
            $("#formCoInsurance").validate().cancelSubmit = true;
            valid = true;
        }


        if (valid) {
            var coInsurance = $("#formCoInsurance").serializeObject();
            coInsurance.AcceptedParticipationPercentageOwn = $('#inputCoInsAcceptedParticipationTotal').val();
            coInsurance.AcceptedCoinsurerId = $('#inputCoInsAcceptedLeadingInsurer').data('Id');
            coInsurance.AssignedParticipationPercentageOwn = $('#inputCoInsAssignedParticipationTotal').val();
            coInsurance.AssignedCoinsurerId = $('#inputCoInsAssignedInsured').data('Id');
            //$("#listCoInsuranceAcceptedAgents").UifListView("refresh");
            CoInsuranceRequest.SaveCoinsurance(coInsurance, UnderwritingCoInsurance.GetAssignedCompanies()).done(function (data) {
                if (data.success) {
                    Underwriting.HidePanelsIssuance(MenuType.CoInsurance);
                    if (glbPolicy.BusinessType != data.result.BusinessType) {
                        Underwriting.UpdateRisks();
                    } else {
                        glbPolicy.BusinessType = data.result.BusinessType;
                        glbPolicy.CoInsuranceCompanies = data.result.CoInsuranceCompanies;
                        var riskInsured = glbPolicy.Summary.RisksInsured;
                        glbPolicy.Summary = data.result.Summary;
                        glbPolicy.Summary.RisksInsured = riskInsured;
                        Underwriting.LoadSummary(glbPolicy.Summary);
                        Underwriting.LoadSubTitles(1);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSaveCoinsurance, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveCoinsurance, 'autoclose': true });

            });
        }
    }
    static GetAssignedAgents() {
        var assignedAgents = [];
        $.each($("#listCoInsuranceAccepted").UifListView('getData'), function (key, value) {
            assignedAgents.push({
                Id: this.Id,//$('#inputAgentsAgency').data('Id'),
                Description: this.Description,// $('#inputAgentsAgency').data('Description'),
                //ParticipationPercentage: this.Participation,
                //ExpensesPercentage: this.Expenses,
                ParticipationPercentageOwn: $('#inputCoInsAcceptedParticipation').val()
            });
        })
        return assignedAgents;
    }
    static GetAssignedCompanies() {
        var AccepteAgents = [];
        var assignedCompanies = [];
        if ($('#selectCoInsBusinessType').UifSelect("getSelected") == BusinessType.Accepted) {
            //$.each($("#listCoInsuranceAcceptedAgents").UifListView('getData'), function (key, value) {
            //    AccepteAgents.push({
            //        Agent: {
            //            IndividualId: this.IndividualId,
            //            Id: this.Id,
            //            FullName: this.Description
            //        },
            //        ParticipationPercentage: this.Participation
            //    });
            //});
            var coInsurance = $("#formCoInsurance").serializeObject();
            //coInsurance.AcceptedParticipationPercentageOwn = $('#inputCoInsAcceptedParticipationTotal').val();
            //coInsurance.AcceptedCoinsurerId = $('#inputCoInsAcceptedLeadingInsurer').data('Id');
            //coInsurance.AssignedParticipationPercentageOwn = $('#inputCoInsAssignedParticipationTotal').val();
            //coInsurance.AssignedCoinsurerId = $('#inputCoInsAssignedInsured').data('Id');
            assignedCompanies.push({
                Id: $('#inputCoInsAcceptedLeadingInsurer').data('Id'),
                Description: coInsurance.AcceptedCoinsurerName,
                ParticipationPercentage: coInsurance.AcceptedParticipationPercentage,
                ExpensesPercentage: coInsurance.AcceptedExpensesPercentage,
                ParticipationPercentageOwn: $('#inputCoInsAcceptedParticipationTotal').val(),
                PolicyNumber: coInsurance.AcceptedPolicyNumber,
                EndorsementNumber: coInsurance.AcceptedEndorsementNumber,
                acceptCoInsuranceAgent: AccepteAgents
            });
        }
        else {
            $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (key, value) {
                assignedCompanies.push({
                    Id: this.Id,
                    Description: this.Description,
                    ParticipationPercentage: this.Participation,
                    ExpensesPercentage: this.Expenses,
                    ParticipationPercentageOwn: $('#inputCoInsAssignedParticipationTotal').val(),
                    acceptCoInsuranceAgent: AccepteAgents
                });
            })
        }
        return assignedCompanies;
    }


    static initialize() {
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
    }

    static CoinsuredAssingList() {
        $('#btnCoInsAssignedNew').on('click', function (event) {
            UnderwritingCoInsurance.ClearPanelAssigned();
        });

        $('#listCoInsuranceAssigned').on('rowEdit', function (event, data, index) {

            CoInsAssignedIndex = index;
            UnderwritingCoInsurance.SetCoInsuranceAssigned(data);
        });

        $('#listCoInsuranceAssigned').on('rowDelete', function (event, data) {
            UnderwritingCoInsurance.ClearPanelAssigned();
            UnderwritingCoInsurance.DeleteCoInsuranceAssigned(data);
        });
    }

    //static CoinsuredAcceptedAgentList() {
    //    $('#listCoInsuranceAcceptedAgents').on('rowEdit', function (event, data, index) {
    //        $('#inputAccepAgentsAgency').prop('disabled', 'disabled');
    //        $('#btnCoInsAcceptedAgent').attr('disabled', false);
    //        CoInsAssignedIndex = index;
    //        UnderwritingCoInsurance.SetCoInsuranceAcceptedAgent(data);
    //    });

    //    $('#listCoInsuranceAcceptedAgents').on('rowDelete', function (event, data) {
    //        UnderwritingCoInsurance.ClearAcceptedAgent();
    //        UnderwritingCoInsurance.DeleteCoInsuranceAcceptedAgent(data);
    //    });
    //}
    static SetCoInsuranceAssigned(dataCoInsuranceAssigned) {
        $('#inputCoInsAssignedInsured').data('Id', dataCoInsuranceAssigned.Id),
            $('#inputCoInsAssignedInsured').val(dataCoInsuranceAssigned.Description),
            $('#inputCoInsAssignedParticipation').val(dataCoInsuranceAssigned.Participation),
            $('#inputCoInsAssignedExpenses').val(dataCoInsuranceAssigned.Expenses)
    }

    static DeleteCoInsuranceAssigned(data) {
        var CoInsuranceAssigned = $("#listCoInsuranceAssigned").UifListView('getData');
        $("#listCoInsuranceAssigned").UifListView({ source: null, displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });

        $.each(CoInsuranceAssigned, function (index, value) {
            if (this.Id != data.Id) {
                $("#listCoInsuranceAssigned").UifListView("addItem", this);
            }
        });
        var total = UnderwritingCoInsurance.CalculateCoInsuranceAssignedTotalAmount();
        $("#inputCoInsAssignedParticipationTotal").val(100 - total);
    }

    //static SetCoInsuranceAcceptedAgent(dataCoInsuranceAcceptedAgent) {
    //    $('#inputAccepAgentsAgency').val(dataCoInsuranceAcceptedAgent.Description)
    //    $('#inputCoInsAcceptedAgentParticipation').val(dataCoInsuranceAcceptedAgent.Participation);
    //}

    //static DeleteCoInsuranceAcceptedAgent(data) {
    //    var CoInsuranceAcceptAgent = $("#listCoInsuranceAcceptedAgents").UifListView('getData');
    //    $("#listCoInsuranceAcceptedAgents").UifListView("refresh");
    //    $.each(CoInsuranceAcceptAgent, function (index, value) {
    //        if (!(this.Id == data.Id && this.IndividualId == data.IndividualId)) {
    //            $("#listCoInsuranceAcceptedAgents").UifListView("addItem", this);
    //        }
    //    });
    //    $("#inputCoInsAcceptedAgenTotalPercentage").text(UnderwritingCoInsurance.GetPercentajeListAgents(false, -1))
    //}

    static CalculateCoInsuranceAssignedTotalAmount() {
        var totalOriginal = 0;
        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (key, value) {
            totalOriginal += parseFloat(NotFormatMoney(this.Participation));
        })
        return totalOriginal;
    }

    //static CalculateCoInsuranceAgentsTotalAmount() {
    //    var totalOriginal = 0;
    //    $.each($("#listCoInsuranceAcceptedAgents").UifListView('getData'), function (key, value) {
    //        totalOriginal += parseFloat(NotFormatMoney(this.Participation));
    //    })
    //    return totalOriginal;
    //}

    static ValidateCoInsuranceAssigned(id) {
        var exists = false;

        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (index, value) {
            if (this.Id == id) {
                exists = true;
            }
        });

        return exists;
    }
    //static ValidateCoInsuranceAcceptedAgent(id, IndividualId) {
    //    var exists = false;
    //    $.each($("#listCoInsuranceAcceptedAgents").UifListView('getData'), function (index, value) {
    //        if (this.Id == id && this.IndividualId == IndividualId) {
    //            exists = true;
    //        }
    //    });

    //    return exists;
    //}

    static ValidateEditCoInsuranceAssigned(isEdit, Id) {
        var percentageTotal = UnderwritingCoInsurance.GetPercentajeTotal(isEdit, Id);
        var percentage = parseFloat($('#inputCoInsAssignedParticipationTotal').val().replace(separatorDecimal, separatorThousands));

        if ((percentage + percentageTotal) > 100 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        if (percentage >= 100) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMinorOwnParticipationPercentage, 'autoclose': true });
            return false;
        }
        //if (percentage <= 0) {
        //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMajorOwnParticipationPercentage, 'autoclose': true });
        //    return false;
        //}
        if ((percentage + percentageTotal) <= 0 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateSumPercentage, 'autoclose': true });
            return false;
        }
        return true;
    }

    static ValidateGeneral(BusinessTypeId) {
        if (BusinessTypeId != "") {
            BusinessTypeId = parseInt(BusinessTypeId, 10);
            switch (BusinessTypeId) {
                case BusinessType.Accepted:

                    break;
                case BusinessType.Assigned:
                    return UnderwritingCoInsurance.ValidateEditCoInsuranceAssigned(false, -1);
                    break;
                default:
                    break;
            }
            return true;
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateBusinessType, 'autoclose': true });
            return false;
        }
    }
    static GetBusinessTypes() {
        CoInsuranceRequest.GetBusinessTypes().done(function (data) {
            if (data.success) {
                $("#selectCoInsBusinessType").UifSelect({ sourceData: data.result });
                if (glbPolicy.Endorsement.PolicyId > 0) {
                    $("#selectCoInsBusinessType").prop("disabled", true);
                    $("#selectCoInsBusinessType").UifSelect("setSelected", glbPolicy.BusinessType);
                }
            }
        });
    }

    static ClearForm() {
        //Coaseguro aceptado
        $("#inputAccepAgentsAgency").val('');
        $("#inputCoInsAcceptedAgentParticipation").val('');
        $("#inputCoInsAcceptedParticipationTotal").val('');
        $("#inputCoInsAcceptedLeadingInsurer").val('');
        $("#inputCoInsAcceptedParticipation").val('');
        $("#inputCoInsAcceptedExpenses").val('');
        $("#inputCoInsAcceptedLeadingPolicy").val('');
        $("#inputCoInsAcceptedEndorsement").val('');
        //$("#listCoInsuranceAcceptedAgents").UifListView("clear");

        //Coaseguro cedido
        $("#inputCoInsAssignedInsured").val('');
        $("#inputCoInsAssignedParticipation").val('');
        $("#inputCoInsAssignedExpenses").val('');
        //$("#listCoInsuranceAssigneds").UifListView("clear");
    }
}