//Codigo de la pagina Agents.cshtml
class UnderwritingAgent extends Uif2.Page {
    getInitialState() {
        $('#checkboxIsPrincipal').hide();
    }

    bindEvents() {
        $('#inputAgents').on('buttonClick', this.SearchAgentsAgency);
        $('#btnAgentsDetail').on('click', UnderwritingAgent.ShowAgentCommissions);
        $('#btnAgentsSave').on('click', UnderwritingAgent.SaveAgencies);
        $('#btnAgents').on('click', this.SaveAndLoadAgent);
        $('#btnAgentsAccept').on('click', UnderwritingAgent.AddAgency);
        $('#listAgencies').on('rowDelete', this.Delete);
    }

    SearchAgentsAgency() {
        agentSearchType = 2;
        CommonAgent.GetAgenciesByAgentIdDesciptionProductId(0, $('#inputAgents').val().trim(), glbPolicy.Product.Id);
    }

    SaveAndLoadAgent() {
        if (UnderwritingAgent.ValidateCorrelation()) {
            if (glbPolicy.Id == 0 && glbPolicy.TemporalType != TemporalType.Quotation) {
                if ($("#formUnderwriting").valid()) {
                    Underwriting.SaveTemporalPartial(MenuType.Agents);
                }
                UnderwritingAgent.LoadPartialAgents();
            }
            else {
                // UnderwritingAgent.LoadPartialAgents();
                UnderwritingAgent.LoadPartialAgents();
            }

        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorColectiveRelation, 'autoclose': true });
        }
    }

    Delete(event, data) {
        UnderwritingAgent.DeleteAgency(data);
    }

    static ShowAgentCommissions() {
        $('#tableCommissions').UifDataTable('clear');

        $.each(glbPolicy.Agencies, function (index, value) {
            var detailAgency = {};

            detailAgency.AgentName = this.Agent.FullName;
            detailAgency.AgencyName = this.FullName;
            detailAgency.Participation = FormatMoney(this.Participation);

            $.each(this.Commissions, function (index, value) {
                var detailCommission = {};

                detailCommission.AgentName = detailAgency.AgentName;
                detailCommission.AgencyName = detailAgency.AgencyName;
                detailCommission.Participation = detailAgency.Participation;
                if (this.SubLineBusiness != null) {
                    detailCommission.LineBusiness = this.SubLineBusiness.LineBusiness.Description;
                    detailCommission.SubLineBusiness = this.SubLineBusiness.Description;
                }
                else {
                    detailCommission.LineBusiness = '';
                    detailCommission.SubLineBusiness = '';
                }
                detailCommission.CalculateBase = this.CalculateBase;
                detailCommission.Percentage = FormatMoney(this.Percentage);
                detailCommission.PercentageAdditional = FormatMoney(this.PercentageAdditional);
                detailCommission.Amount = this.Amount;

                $('#tableCommissions').UifDataTable('addRow', detailCommission)
            });
        });

        $("#modalAgentsCommissions").UifModal('showLocal', AppResources.LabelCommissions);
    }

    static LoadPartialAgents() {
        $('#formAgents').formReset();
        if (glbPolicy.Id > 0 && $("#formUnderwriting").valid()) {
            Underwriting.ShowPanelsIssuance(MenuType.Agents);
            if (glbPolicy.Agencies != undefined) {
                //Underwriting.SaveTemporal();
                var tempAgent = jQuery.extend(true, [], glbPolicy.Agencies);
                CommonAgent.LoadAgencies(tempAgent, 'agencyTemplate');
                if (tempAgent[0].Commissions[0] != null || tempAgent[0].Commissions[0] != undefined) {
                    commissionGral = tempAgent[0].Commissions[0].Percentage;
                }
                //$("#inputAgentsPercentage").val(commissionGral);
                $("#inputAgentsPercentage").val('0');
                $("#inputAgentsPercentageAdditional").val('0');
            }
        }
        else {
            Underwriting.ShowPanelsIssuance(MenuType.Agents);
        }
    }

    static ValidateCorrelation() {
        var validated = true;
        if (Underwriting.getQueryVariable("isCollective") == "true" && glbPolicy.Id != 0) {
            UnderwritingTemporal.GetCollectiveRelation().done(function (data) {
                if (data.success) {
                    if (data.result) {
                        validated = false;
                    }
                }
            });
        }
        return validated;
    }

    static AddAgency() {
        if (CommonAgent.ValidAgency()) {
            var agentEdit = CommonAgent.GetFormAgent();
            var agencies = CommonAgent.GetListAgencies();
            var indexListAgentPrincipal = CommonAgent.GetIndexListAgentPrincipal();
            var indexListAgentEdit = CommonAgent.GetIndexListAgentEdit(agentEdit);
            var agenciesUpdate = CommonAgent.SetAgentPrincipal(agentEdit, agencies, indexListAgentPrincipal, indexListAgentEdit);
            if (agentEdit.Code == agentEdit.Agent.IndividualId && agentEdit.Commissions[0].Percentage > 0) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.AgentDirectNoComision, 'autoclose': true });
            }
            else {
                agenciesUpdate = CommonAgent.ReCalculateParticipation(agencies, agentEdit, indexListAgentPrincipal, indexListAgentEdit, agenciesUpdate);
                if (agenciesUpdate != null) {
                    agenciesUpdate = CommonAgent.UpdateCommission(agenciesUpdate, agentEdit);
                    CommonAgent.CalculateTotalParticipation(agenciesUpdate, 0);
                    CommonAgent.UpdateListAgencies(agenciesUpdate, 'agencyTemplate');
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSetCommissions, 'autoclose': true });
                }
                if (glbPolicy.PolicyOrigin == PolicyOrigin.Collective) {
                    CommonAgent.ParticipationSummary();
                }
                else {
                    agentEdit.Commissions[0].Percentage = commissionGral;
                    //  UnderwritingAgent.GetCommissions(agentEdit);
                }

            }
            CommonAgent.ClearAgency();
        }
    }

    static SaveAgencies() {
        if ($('#labelAgentsTotalParticipation').text() == 100) {
            var agencies = CommonAgent.GetAgencies();
            UnderwritingAgentRequest.SaveCommissions(glbPolicy.Id, agencies).done(function (data) {
                if (data.success) {
                    Underwriting.HidePanelsIssuance(MenuType.Agents);
                    glbPolicy.Agencies = data.result;
                    Underwriting.LoadSubTitles(2);
                    agentSearchType = 1;
                    if (glbPolicy.TemporalType == TemporalType.Quotation) {
                        Underwriting.SaveTemporal(false);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveAgents, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTotalParticipationAgent, 'autoclose': true });
        }
    }

    static GetCommissions(agency) {
        var agencies = CommonAgent.GetAgencies();
        UnderwritingAgentRequest.GetCommissions(glbPolicy.Id, agency, agencies).done(function (data) {
            if (data.success) {
                //data.result, trae el resultado de todos los cálculos que se necesitan para visualizar la pantalla correctamente
                CommonAgent.LoadAgencies(data.result, 'agencyTemplate');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorGetCommissions, 'autoclose': true });
        });
    }

    static DeleteAgency(agencyDelete) {
        if (String(agencyDelete.IsPrincipal) == 'true') {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateDeleteAgent, 'autoclose': true });
        }
        else {
            var agencies = CommonAgent.GetListAgencies();
            agencies = CommonAgent.AssignAgentMount($('#listAgencies').UifListView('getData'), agencyDelete);
            agencies = CommonAgent.DeleteAgencyList(agencies, agencyDelete, []);
            CommonAgent.CalculateTotalParticipation(agencies, 0);
            CommonAgent.UpdateListAgencies(agencies, 'agencyTemplate');

            CommonAgent.ClearAgency();
            CommonAgent.LoadListAgenciesCommision();
        }
    }
}