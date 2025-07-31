//Codigo de la pagina Agents.cshtml

var agencyIndex = null;
var commissionGral = null;
var baseCalculate = 0;
let agencyEdit = null;

$(() => {
    new CommonAgent();
});
class CommonAgent extends Uif2.Page {

    getInitialState() {
    }
    bindEvents() {
        $('#inputAgents').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $('#inputAgents').on('buttonClick', CommonAgent.GetAgenciesByAgentIdDesciptionProductId);
        $('#inputAgentsPercentage').OnlyDecimals(2);
        $('#inputAgentsPercentageAdditional').OnlyDecimals(2);
        $('#btnAgentsCancel').on('click', function () {
            CommonAgent.ClearAgency();
        });
        $('#listAgencies').on('rowEdit', function (event, data, index) {
            CommonAgent.EditAgency(data, index);
        });
        $('#btnAgentsClose').on('click', CommonAgent.HidePanel);

        $('#tableCommonResults tbody').on('click', 'tr', function (e) {
            CommonAgent.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
            $('#modalCommonSearch').UifModal("hide");
        });
    }
    static EditAgency(agency, index) {
        agencyEdit = agency;
        agencyIndex = index;
        $("#agentIsPrincipal").removeAttr("checked");
        if (agency.IsPrincipal == true) {
            $("#agentIsPrincipal").prop("checked", "checked");
        }
        $('#inputAgents').data('Object', agency);
        $('#inputAgents').val(agency.FullName);
        $('#inputAgentsParticipation').val(agency.Participation);
        if (agency.Commissions != undefined && agency.Commissions.length > 0) {
            var percentage = parseFloat(agency.Commissions[0].Percentage) > 0 ? agency.Commissions[0].Percentage : agency.Commissions[0].AgentPercentage;
            $('#inputAgentsPercentage').val(percentage);
            $('#inputAgentsPercentageAdditional').val(agency.Commissions[0].PercentageAdditional);
            if (agency.IsPrincipal) {
                $('#inputAgentsPercentage').prop('disabled', false);
            }
            else {
                $('#inputAgentsPercentage').prop('disabled', true);
            }
        }
        else {
            $('#inputAgentsPercentage').prop('disabled', false);
        }
        if (String(agency.IsPrincipal) == 'true') {
            $('#inputAgents').UifInputSearch('disabled', true);
        }
        else {
            $('#inputAgents').UifInputSearch('disabled', false);
        }

        CommonAgent.GetSalePointsByBranchId(agency.Branch.Id, agency.Branch.SalePoints[0].Id);
    }
    static HidePanel() {
        $("#modalAgents").UifModal('hide');
    }
    static GetAgencies() {
        var agencies = $('#listAgencies').UifListView('getData');
        $.each(agencies, function (index, value) {
            value.Agent.DateCurrent = FormatDate(value.Agent.DateCurrent);
            if (this.Commissions != null) {
                $.each(this.Commissions, function (index, value) {
                    this.Percentage = this.Percentage;
                    this.PercentageAdditional = this.PercentageAdditional;
                    this.CalculateBase = NotFormatMoney(this.CalculateBase);
                    this.Amount = NotFormatMoney(this.Amount);
                });
            }
        });
        return agencies;
    }
    static GetAgenciesByList(listName) {
        var agencies = $(listName).UifListView('getData');
        $.each(agencies, function (index, value) {
            value.Agent.DateCurrent = FormatDate(value.Agent.DateCurrent);
            if (this.Commissions != null) {
                $.each(this.Commissions, function (index, value) {
                    this.Percentage = this.Percentage;
                    this.PercentageAdditional = this.PercentageAdditional;
                    this.CalculateBase = NotFormatMoney(this.CalculateBase);
                    this.Amount = NotFormatMoney(this.Amount);
                });
            }
        });
        return agencies;
    }
    static GetListAgencies() {
        var agencies = $('#listAgencies').UifListView('getData');
        $.each(agencies, function (index, item) {
            item.Participation = parseFloat(replaceCode(String(item.Participation)));
            if (item.Commissions != undefined && item.Commissions.length > 0) {
                if (item.Commissions[0].Percentage === null || item.Commissions[0].Percentage === "") {
                    item.Commissions[0].Percentage = 0;
                    agency.PercentageAdditional = 0;
                }
            }
        });
        return agencies;
    }
    static GetFormAgent() {
        var agency = Object.assign({}, $('#inputAgents').data('Object'), $('#formAgents').serializeObject());
        agency.BaseCalculate = $("#labelPremium").text();
        agency.TotalPercentage = agency.Percentage;
        agency.TotalAmount = 0;
        agency.Participation = parseFloat(replaceCode(agency.Participation));
        agency.IsPrincipal = $("#agentIsPrincipal").prop("checked");
        var AgentType = 0;
        var DateCurrent = null;

        if ($('#inputAgents').data('Object') != undefined && $('#inputAgents').data('Object') != null) {
            if ($('#inputAgents').data('Object').Agent.AgentType != null && $('#inputAgents').data('Object').Agent.AgentType.Id != null)
                AgentType = $('#inputAgents').data('Object').Agent.AgentType.Id;

            if ($('#inputAgents').data('Object').Agent.DateCurrent != null && $('#inputAgents').data('Object').Agent.DateCurrent != "")
                DateCurrent = $('#inputAgents').data('Object').Agent.DateCurrent;

            agency.Agent = {
                AgentType: { Id: AgentType },
                IndividualId: $('#inputAgents').data('Object').Agent.IndividualId,
                FullName: $('#inputAgents').data('Object').Agent.FullName,
                DateCurrent: FormatDate(DateCurrent)
            };
        }

        if (agency.PercentageAdditional === null || agency.PercentageAdditional === "") {
            agency.PercentageAdditional = 0;
        }


        agency.Commissions = [{
            AgentPercentage: agency.Percentage,
            Percentage: 0,
            PercentageAdditional: agency.PercentageAdditional,
            //Adicionar calculos 
            CalculateBase: agency.BaseCalculate,
            Amount: 0
        }];

        agency.BranchDescription = $('#selectBranch').find('option:selected').text();
        agency.SalePoint = $('#selectAgentSalesPoint').find('option:selected').text();

        agency.Branch = {
            Id: $('#selectBranch').find('option:selected').val(),
            Description: $('#selectBranch').find('option:selected').text(),
            SalePoints: []
        };

        agency.Branch.SalePoints.push({ Id: $('#selectAgentSalesPoint').find('option:selected').val(), Description: $('#selectAgentSalesPoint').find('option:selected').text() });
        
        return agency;
    }
    static GetPrefixesByAgentIdAgents(agentId) {
        $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetPrefixesByAgentIdAgents',
            data: JSON.stringify({ agentId: agentId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (!data.success) {
                $("#inputAgents").data("Object", null);
                $("#inputAgents").val('');
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchPrefixAgent, 'autoclose': true });
        });
    }
    static ValidAgency() {
        var agentEdit = CommonAgent.GetFormAgent();
        $('#formAgents').validate();
        if ($('#formAgents').valid()) {
            if ($('#inputAgents').data('Object') == null) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateAgent, 'autoclose': true });
                return false;
            }
            else {
                if (agencyIndex == null) {
                    var exists = false;
                    var principalParticipation = 0;

                    if (!$("#agentIsPrincipal").prop("checked") && (agentEdit.Code != agentEdit.Agent.IndividualId) && ($('#inputAgentsPercentage').val() == 0 || $('#inputAgentsPercentage').val() > 100)) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ValErrorp, 'autoclose': true });
                        return false;
                    }

                    $.each($('#listAgencies').UifListView('getData'), function (index, value) {
                        if (this.Id == $('#inputAgents').data('Object').Id && this.Code == $('#inputAgents').data('Object').Code) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateAgent, 'autoclose': true });
                            exists = true;
                        }
                    });

                    if (exists) {
                        return false;
                    }
                }

            }
            return true;
        }
    }
    static LoadAgencies(agencies, template) {
        $('#listAgencies').UifListView({ displayTemplate: '#' + template, add: false, edit: true, delete: true, customEdit: true, customDelete: true, height: 250 });
        var totalParticipation = 0;

        if (agencies != null) {
            $.each(agencies, function (index, value) {
                var totalAmount = 0;
                totalParticipation += parseFloat(this.Participation);
                this.Participation = this.Participation;
                if (this.Commissions != null && this.Commissions.length > 0) {
                    $.each(this.Commissions, function (index, value) {
                        this.Percentage = this.Percentage;
                        this.PercentageAdditional = this.PercentageAdditional;
                        this.CalculateBase = FormatMoney(this.CalculateBase);
                        //transformar cualquier valor a string
                        var str = (this.Amount).toString();
                        var validador = str.indexOf(",");
                        if (validador == -1) {
                            totalAmount += parseFloat(this.Amount);
                            this.Amount = FormatMoney(this.Amount);
                        }
                        else {
                            totalAmount = this.Amount;
                        }
                    });
                    this.TotalPercentage = this.Commissions[0].Percentage;
                }
                this.TotalAmount = FormatMoney(totalAmount);
                this.FullName = this.Agent.FullName;
                this.IndividualId = this.Agent.IndividualId;
                Object.defineProperty(this, 'BaseCalculate', {
                    value: this.Commissions[0].CalculateBase,
                    writable: true,
                    enumerable: true,
                    configurable: true
                });

                if (this.Branch.SalePoints != null && this.Branch.SalePoints.length > 0) {
                    this.BranchDescription = this.Branch.Description;
                    this.SalePoint = this.Branch.SalePoints[0].Description;
                } else {
                    this.BranchDescription = $('#selectBranch').find('option:selected').text();
                    this.SalePoint = $('#selectSalesPoint').find('option:selected').text();
                }

                $('#listAgencies').UifListView('addItem', this);
            });

            $('#labelAgentsTotalParticipation').text((totalParticipation));
            if ($("#listAgencies").UifListView('getData').length > 1) {
                CommonAgent.LoadListAgenciesCommision();
            }
        }
    }
    static ReCalculateParticipation(agencies, agentEdit, indexListAgentPrincipal, indexListAgentEdit, agenciesUpdate) {
        var restAgentEdit = -agentEdit.Participation;
        var bandParticipation = true;
        var restAgentPrincipal = 0;
        var totalParticipation = 0;

        if (indexListAgentEdit >= 0) //El intermediario principal no puede editarse su participacion
        {
            if (String(agencies[indexListAgentEdit].IsPrincipal) === "true") {
                bandParticipation = false;
            }
            agencies[indexListAgentEdit] = agentEdit;   //wmw asignamos los cambios editados

            if (agencies[indexListAgentEdit].Branch.Id == undefined || agencies[indexListAgentEdit].Branch.Id == null || agencies[indexListAgentEdit].Branch.Id == 0) {
                agencies[indexListAgentEdit].Branch.Id = agencyEdit.Branch.Id;
                agencies[indexListAgentEdit].Branch.Description = agencyEdit.Branch.Description;
                agencies[indexListAgentEdit].BranchDescription = agencyEdit.Branch.Description;
            }
        }
        //Identififca la participación total
        $.each(agencies, function (index, item) {
            totalParticipation = totalParticipation += parseFloat(item.Participation);
        })
        if (indexListAgentPrincipal >= 0 && bandParticipation) //Si hay por lo menos un registro en la lista
        {
            var itemAgentPrincipal = agencies[indexListAgentPrincipal];
            /*if (indexListAgentEdit >= 0) {
                restAgentEdit = agencies[indexListAgentEdit].Participation + restAgentEdit;
            }*/
            if (totalParticipation <= 100) {
                restAgentPrincipal = itemAgentPrincipal.Participation + (100 - totalParticipation);
            } else {
                restAgentPrincipal = itemAgentPrincipal.Participation + restAgentEdit;
            }


            if (restAgentPrincipal > 0) {
                agenciesUpdate[indexListAgentPrincipal].Participation = restAgentPrincipal;
                if (indexListAgentEdit >= 0) {
                    agenciesUpdate[indexListAgentEdit].Participation = agentEdit.Participation;
                }
                else {
                    agenciesUpdate[agenciesUpdate.length - 1].Participation = agentEdit.Participation;
                }
            }
            else {
                //Asigna el minímo valor al intermediario principal
                restAgentPrincipal = agenciesUpdate[indexListAgentPrincipal].Participation + restAgentPrincipal


                if (restAgentPrincipal <= 0) {
                    restAgentEdit = agenciesUpdate[indexListAgentPrincipal].Participation - 1;
                    agenciesUpdate[indexListAgentPrincipal].Participation = 1;
                } else {
                    agenciesUpdate[indexListAgentPrincipal].Participation = restAgentPrincipal;
                }

                agentEdit.Participation = Math.abs(restAgentEdit);

                if (restAgentEdit == 0) {
                    agenciesUpdate = null;
                }
            }
        } else {

        }

        return agenciesUpdate;

    }
    static ClearAgency() {
        agencyIndex = null;
        $("#agentIsPrincipal").removeAttr("checked");
        $('#inputAgents').UifInputSearch('disabled', false);
        $('#inputAgents').data('Object', null);
        $('#inputAgents').val('');
        $('#inputAgentsParticipation').val(100 - parseFloat($('#labelAgentsTotalParticipation').text().replace(separatorDecimal, separatorThousands)));
        $('#inputAgentsPercentage').val('0');
        $('#inputAgentsPercentageAdditional').val('0');
        $('#inputAgentsPercentage').prop('disabled', false);
        $('#selectAgentSalesPoint').UifSelect("setSelected", null);
    }
    static SetAgentPrincipal(agentEdit, agencies, indexListAgentPrincipal, indexListAgentEdit) {
        if (indexListAgentPrincipal < 0) {
            indexListAgentPrincipal = 0; //Se asigna para evitar error con listview -1
        }
        if (agencies.length === 0) //Si la lista esta vacia, agrega el Agent como principal
        {
            agencies.push(agentEdit);
            agencies[0].IsPrincipal = true;
            agencies[0].Participation = 100;
        }
        else {
            if (String(agentEdit.IsPrincipal) === 'true' && agencyIndex != null) {
                agencies[indexListAgentPrincipal].IsPrincipal = false;
                agencies[indexListAgentEdit].IsPrincipal = true;
            }
            else if (String(agentEdit.IsPrincipal) === 'true' && agencyIndex === null) {
                agencies[indexListAgentPrincipal].IsPrincipal = false;
                agencies.push(agentEdit);
            }
            else if (agencyIndex === null) {
                agencies.push(agentEdit);
            }
        }
        
        return agencies;

    }
    static UpdateCommission(agencies, agentEdit) {
        $.each(agencies, function (index, item) {
            if (agentEdit != null) {

                if (String(agentEdit.IsPrincipal) === "true" && item.Code == agentEdit.Code) {
                    if (agentEdit.Percentage != undefined) {
                        commissionGral = agentEdit.Percentage; //Si agentEdit es el principal se actualizan las comisiones igual para todos
                    }
                    item.TotalPercentage = commissionGral; //wmw cambia TotalPercentage al principal
                    if (item.Commissions != undefined) {  //wmw cambia Percentage al principal
                        //item.Commissions[0].Percentage = commissionGral;
                        item.Commissions[0].Percentage = 0;
                    }
                } else {

                    //if (String(agentEdit.IsPrincipal) === "false") {
                    if (item.Commissions != undefined) {  //wmw cambia Percentage al principal
                        item.Commissions[0].AgentPercentage = parseFloat(item.Commissions[0].Percentage) > 0 ? item.Commissions[0].Percentage : item.Commissions[0].AgentPercentage;
                        item.Commissions[0].Percentage = 0;
                    }
                }
            }


            if (item.TotalAmount != undefined) {
                if (item.BaseCalculate != undefined) {
                    if (item.BaseCalculate == "") {
                        item.BaseCalculate = baseCalculate;
                    }
                    if (!$.isNumeric(item.BaseCalculate)) {
                        item.BaseCalculate = item.BaseCalculate.replace(separatorThousands, "");
                        item.BaseCalculate = item.BaseCalculate.replace(separatorThousands, "");
                        item.BaseCalculate = item.BaseCalculate.replace(separatorDecimal, separatorThousands);
                    }

                    let localPercentaje = Number(item.TotalPercentage);

                    if (glbPolicy.Endorsement.EndorsementType == EndorsementType.ChangeAgent) {
                        item.TotalAmount = 0;
                    } else {
                        item.TotalAmount = FormatMoney(item.BaseCalculate * (localPercentaje * item.Participation)/10000);
                    }
                }
            }
        });
        return agencies;
    }
    static GetIndexListAgentPrincipal(agencies) {
        const findAgentPrincipal = function (element, index, array) {
            return String(element.IsPrincipal) === 'true'
        }
        var index = $("#listAgencies").UifListView("findIndex", findAgentPrincipal);
        return index;
    }
    static GetIndexListAgentEdit(agentEdit) {
        const findAgentEdit = function (element, index, array) {
            return element.Id === agentEdit.Id && element.Code === agentEdit.Code
        }
        var index = $("#listAgencies").UifListView("findIndex", findAgentEdit);
        return index;
    }
    static ParticipationSummary() {
        var totalParticipation = 0;
        $.each($('#listAgencies').UifListView('getData'), function (index, value) {
            totalParticipation += parseFloat(this.Participation.replace(separatorDecimal, separatorThousands));
        });
        $('#labelAgentsTotalParticipation').text(totalParticipation);
    }
    static UpdateListAgencies(agencies, template) {
        $('#listAgencies').UifListView({
            displayTemplate: '#' + template,
            add: false,
            edit: true,
            delete: true,
            customEdit: true,
            customDelete: true,
            height: 250,
            sourceData: agencies
        });
    }
    static CalculateTotalParticipation(agencies, totalParticipation) {
        $.each(agencies, function (index, item) {
            totalParticipation = totalParticipation += parseFloat(item.Participation);
            if (totalParticipation > 100) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMajorPercentage, 'autoclose': true });
            }
        })
        $('#labelAgentsTotalParticipation').text(totalParticipation);
    }
    static DeleteAgencyList(agencies, agencyDelete, agenciesUpdated) {
        $.each(agencies, function (index, item) {
            if (item.Id != agencyDelete.Id || item.Code != agencyDelete.Code) {
                agenciesUpdated.push(item);
            }
        })
        return agenciesUpdated;
    }
    static AssignAgentMount(agencies, agencyDelete) {
        $.each(agencies, function (index, item) {
            if (String(item.IsPrincipal) === "true") {
                item.Participation = parseFloat(item.Participation) + parseFloat(agencyDelete.Participation);
            }
        })
        return agencies;
    }
    static ShowDefaultResults(dataTable) {
        $('#tableCommonResults').UifDataTable('clear');
        $('#tableCommonResults').UifDataTable('addRow', dataTable);
    }
    static GetAgenciesByAgentIdDescription(agentId, description) {
        if (agentId != undefined || description != undefined) {
            var number = parseInt(description, 10);
            if ((!isNaN(number) || description.length > 2) && (description != 0)) {

                AgencyAjax.GetAgenciesByAgentIdDesciptionProductId(agentId, description, glbPolicy.Product.Id).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            $("#inputAgents").data("Object", data.result[0]);
                            $("#inputAgents").val(data.result[0].FullName);
                            CommonAgent.SearchCommission();
                        }
                        else if (data.result.length > 1) {
                            modalListType = 6;
                            var dataList = [];

                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].Agent.IndividualId,
                                    Code: data.result[i].Code,
                                    Description: data.result[i].Agent.FullName
                                });
                            }
                            CommonAgent.ShowDefaultResults(dataList);
                            $('#modalCommonSearch').UifModal('showLocal', AppResources.LabelAgentPrincipal);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorNotAgents });
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }

                });
            }
        }
    }
    static ValidateAgentUser(agentId, description) {
        var number = parseInt(description, 10);
        var bandValidateAgentUser = false;
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Underwriting/Underwriting/GetUserAgenciesByAgentIdDescription',
                data: JSON.stringify({ agentId: agentId, description: description }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        bandValidateAgentUser = true;
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.UserNoHaveAgentEnabled, 'autoclose': true });
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchAgents, 'autoclose': true });
            });
        }
        return bandValidateAgentUser;
    }

    static GetAgenciesByAgentIdDesciptionProductId(agentId, description, productId) {
        if (agentId != undefined || description != undefined) {
            var number = parseInt(description, 10);
            //var validateAgentUser = CommonAgent.ValidateAgentUser(agentId, description);
            // eliminado no realizaba busqueda de los intermediarios

            if ((!isNaN(number) || description.length > 2) && (description != 0)) {
                AgencyAjax.GetAgenciesByAgentIdDesciptionProductId(agentId, description, productId).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            $("#inputAgents").data("Object", data.result[0]);
                            $("#inputAgents").val(data.result[0].FullName);
                            CommonAgent.SearchCommission();
                        }
                        else if (data.result.length > 1) {
                            var dataList = [];
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].Agent.IndividualId,
                                    Code: data.result[i].Code,
                                    Description: data.result[i].FullName
                                });
                            }
                            CommonAgent.ShowDefaultResults(dataList);
                            $('#modalCommonSearch').UifModal('showLocal', AppResources.LabelAgentPrincipal);
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
    static DeleteAgency(data) {
        if (data.IsPrincipal) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateDeleteAgent });
        }
        else {
            var participationTmp = data.Participation;
            var index = $("#listAgencies").UifListView("findIndex",
                function (item) {
                    if (item.Agent.IndividualId == data.Agent.IndividualId && item.Id == data.Id) {
                        return true;
                    } else {
                        return false;
                    }
                });
            $("#listAgencies").UifListView("deleteItem", index);
            CommonAgent.CalculateTotalParticipation(CommonAgent.GetAgencies(), 0);
        }
    }

    static SearchCommission() {
        $('#inputAgentsPercentage').val(commissionGral);
        $('#inputAgentsPercentage').prop('disabled', true);
    }

    static LoadListAgenciesCommision() {
        var agenciesUpdate = null;
        var listCount = $("#listAgencies").UifListView('getData').length;
        $.each($("#listAgencies").UifListView('getData'), function (index, value) {
            if (!value.IsPrincipal || listCount == 1) {
                var agencies = CommonAgent.GetListAgencies();
                var indexListAgentPrincipal = CommonAgent.GetIndexListAgentPrincipal();
                agenciesUpdate = CommonAgent.ReCalculateParticipation(agencies, value, indexListAgentPrincipal, index, agencies);
                if (agenciesUpdate != null) {
                    agenciesUpdate = CommonAgent.UpdateCommission(agenciesUpdate, value);
                    CommonAgent.CalculateTotalParticipation(agenciesUpdate, 0);
                    CommonAgent.UpdateListAgencies(agenciesUpdate, 'agencyTemplate');
                }
            }
        });
    }

    static GetSalePointsByBranchId(branchId, selectedId) {
        UnderwritingRequest.GetSalePointsByBranchId(branchId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    if (selectedId == 0) {
                        $("#selectAgentSalesPoint").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectAgentSalesPoint").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }
                }
                else {
                    $("#selectAgentSalesPoint").UifSelect();
                    $.UifNotify('show', { 'type': 'info', 'message': 'No se hallan puntos de venta asociados', 'autoclose': true });
                }
            }
        });
    }

}


class AgencyAjax {
    static GetAgenciesByAgentIdDescription(agentId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetAgenciesByAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAgenciesByAgentId(agentId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetAgenciesByAgentId',
            data: JSON.stringify({ agentId: agentId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAgencyByAgentIdAgencyId(agentId, agencyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetAgencyByAgentIdAgencyId',
            data: JSON.stringify({ agentId: agentId, agencyId: agencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }
    static GetAgenciesByAgentIdDesciptionProductId(agentId, description, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetAgenciesByAgentIdDesciptionProductId',
            data: JSON.stringify({ agentId: agentId, description: description, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }
}