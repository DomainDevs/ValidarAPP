var temporaryAgent = null;

class ChangeAgent extends Uif2.Page {

    getInitialState() {
        $(document).ajaxStop($.unblockUI);
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {

            glbPolicy.Endorsement.EndorsementType = EndorsementType.ChangeAgent;

            $('#inputIssueDate').UifDatepicker('disabled', true);
            $('#inputCurrentFrom').ValidatorKey(ValidatorType.Dates, ValidatorType.Dates, 1);
            $('#inputRecord').prop('disabled', true);

            if (glbPolicy.Endorsement.Id > 0) {
                ChangeAgent.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
            }
            if (glbPolicy.Id > 0) {
                PolicyRequest.GetTemporalById(glbPolicy.Id, glbPolicy.EndorsementController).done(function (data) {
                    if (data.success) {
                        ChangeAgent.LoadEndorsementData(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            GetDateIssue();
            if (glbPolicy.Endorsement.PolicyId > 0 && glbPolicy.Endorsement.Id > 0) {
                PolicyRequest.GetAgentByPolicyIdEndorsementId(glbPolicy.Endorsement.PolicyId, glbPolicy.Endorsement.Id, Number(NotFormatMoney(glbPolicy.Summary.Premium).replace(",", "."))).done(function (data) {
                    if (data.success) {
                        //AgentsCurrent = data.result;
                        ChangeAgent.LoadAgentData(data.result, "#listAgenciesCurrent");
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }

            if (glbPolicy.Branch != null && glbPolicy.Branch.Id > 0) {
                CommonAgent.GetSalePointsByBranchId(glbPolicy.Branch.Id, 0);
            }

            $("#inputTicketNumber").ValidatorKey(ValidatorType.Number, 2, 0);
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
            $('#listAgenciesModify').UifListView({ displayTemplate: '#agencyTemplate', height: 250 });
            $('#btnAgentsDetail').on('click', function () {
                ChangeAgent.ShowAgentCommissions();
            });
            $("#btnPrintTemporalAgent").hide();
            this.ValidatePolicies();
        }
    }
    bindEvents() {
        $('#btnCancel').on('click', ChangeAgent.RedirectSearchController);
        $('#btnSave').on('click', this.SavePolicy);
        $("#btnAgentsEndorsement").on("click", AgentEndorsement.ShowAgentEndorsement);
        $('#btnAgentsSave').on('click', this.CreateTemporal);
        $('#btnAgentsAccept').on('click', AgentEndorsement.AddAgency);
        $('#btnAgentsCancel').on('click', CommonAgent.ClearAgency);
        $('#inputAgents').on('buttonClick', function () {
            agentSearchType = 2;
            CommonAgent.GetAgenciesByAgentIdDesciptionProductId(0, $('#inputAgents').val().trim(), glbPolicy.Product.Id);
        });
        $('#tableResults tbody').on('click', 'tr', function (e) {
            CommonAgent.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
            $('#modalDefaultSearch').UifModal("hide");
        });
        $("#inputCurrentFrom").on("datepicker.change", function (event, date) {
            ChangeAgent.ChangeAgentDate($("#inputCurrentFrom").val(), glbPolicyEndorsement.Endorsement.CurrentTo);
        });

        //$('#listAgencies').on('rowEdit', RiskBeneficiary.BeneficiaryEdit);

        $('#listAgencies').on('rowDelete', function (event, data) {
            AgentEndorsement.DeleteAgency(data);
        });

        $('#listAgencies').on('rowEdit', function (event, data, index) {
            CommonAgent.EditAgency(data, index);
        });
        $("#btnPrintTemporalAgent").click(ChangeAgent.PrintTemporal);

    

        $('#tableCommonResults tbody').on('click', 'tr', function (e) {
            CommonAgent.GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML);
            $('#modalCommonSearch').UifModal("hide");
        });
    }

    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(glbPolicy.Id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Modification.RedirectSearchController();
                }
            });
        }
    }

    static ShowAgentCommissions(agencies) {
        var agencies = $('#listAgencies').UifListView('getData');
        $('#tableCommissions').UifDataTable('clear');
        $.each(agencies, function (index, value) {
            var detailAgency = {};

            detailAgency.AgentName = this.Agent.FullName;
            detailAgency.AgencyName = this.FullName;
            detailAgency.Participation = FormatMoney(this.Participation);
            var base = this.BaseCalculate;
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
                detailCommission.CalculateBase = base;
                detailCommission.Percentage = FormatMoney(this.Percentage);
                detailCommission.PercentageAdditional = FormatMoney(this.PercentageAdditional);
                detailCommission.Amount = this.Amount;

                $('#tableCommissions').UifDataTable('addRow', detailCommission)
            });
        });

        $("#modalAgentsCommissions").UifModal('showLocal', Resources.Language.LabelCommissions);
    }
    static ChangeAgentDate(modificationDate, currentTo) {
        if (CompareBetweenDates(modificationDate, FormatDate(glbPolicy.CurrentFrom), currentTo)) {
            $('#inputDays').val(CalculateDays(modificationDate, currentTo));
        } else {
            $('#inputCurrentFrom').val("");
            $('#inputDays').val("");
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.LabelDateToDatePolicy, 'autoclose': true });
        }
    }
    static LoadCurrentSummaryEndorsement(policyData) {
        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(policyData.Summary.AmountInsured);
            $('#labelCurrentPremium').text(policyData.Summary.Premium);
            $('#labelCurrentExpenses').text(policyData.Summary.Expenses);
            $('#labelCurrentTaxes').text(policyData.Summary.Taxes);
            $('#labelCurrentTotalPremium').text(policyData.Summary.FullPremium);

        }
        if (glbPolicy.EndorsementTexts != undefined && glbPolicy.EndorsementTexts.length > 0) {
            var recordText = "";
            glbPolicy.EndorsementTexts.forEach(function (item) {
                recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
            });
            $('#inputRecord').val(recordText);
        }
    }
    static LoadEndorsementData(policy) {
        $('#inputCurrentFrom').UifDatepicker('setValue', FormatDate(policy.Endorsement.CurrentFrom));
        $('#inputCurrentTo').UifDatepicker('setValue', FormatDate(policy.CurrentTo));
        $('#inputChangeAgentDays').val(CalculateDays($('#inputCurrentFrom').val(), FormatDate(policy.CurrentTo)));
        $('#inputText').val(policy.Endorsement.Text.TextBody);
        $('#inputObservations').val(policy.Endorsement.Text.Observations);
        $("#inputTicketNumber").val(policy.Endorsement.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(policy.Endorsement.TicketDate));
        ChangeAgent.LoadSummaryTemporal(policy);
    }

    static LoadAgentData(agencies, nameListView) {
        $(nameListView).UifListView({ displayTemplate: '#agencyTemplate', add: false, edit: false, delete: false, height: 250 });
        var totalParticipation = 0;
        if (agencies != null) {
            $.each(agencies, function (index, value) {
                var totalAmount = 0;
                totalParticipation += parseFloat(this.Participation);
                this.Participation = FormatMoney(this.Participation);
                if (this.Commissions != null) {
                    $.each(this.Commissions, function (index, value) {
                        this.Percentage = FormatMoney(this.Percentage);
                        this.PercentageAdditional = FormatMoney(this.PercentageAdditional);
                        this.CalculateBase = FormatMoney(this.CalculateBase);
                        totalAmount += parseFloat(this.Amount);
                        this.Amount = FormatMoney(this.Amount);
                    });
                    this.TotalPercentage = this.Commissions[0].AgentPercentage > 0 ? this.Commissions[0].AgentPercentage : this.Commissions[0].Percentage;
                    this.Agent = {
                        IndividualId: this.Agent.IndividualId,
                        FullName: this.Agent.FullName,
                        DateCurrent: this.Agent.DateCurrent
                    }
                }
                this.TotalAmount = FormatMoney(totalAmount);
                this.BaseCalculate = glbPolicy.Summary.Premium;

                if (this.Branch != null && this.Branch.SalePoints != null && this.Branch.SalePoints.length > 0) {
                    this.BranchDescription = this.Branch.Description;
                    this.SalePoint = this.Branch.SalePoints[0].Description;
                }

                $(nameListView).UifListView('addItem', this);
            });

            $('#labelAgentsTotalParticipation').text(FormatMoney(totalParticipation));
        }
    }
    static LoadSummaryTemporal(temporalData) {
        temporaryAgent = temporalData;
        glbPolicy.Id = temporalData.Id;
        AgentEndorsement.HidePanelsIssuance();
        ChangeAgent.LoadAgentData(temporalData.Agencies, "#listAgenciesModify");

        if (temporalData.Summary != null) {
            $('#labelRisk').text(temporalData.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(temporalData.Summary.Premium));
            $('#labelExpenses').text(FormatMoney(temporalData.Summary.Expenses));
            $('#labelTaxes').text(FormatMoney(temporalData.Summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(temporalData.Summary.FullPremium));
        }
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }
    CreateTemporal() {
        if (ChangeAgent.validationAddAgents()) {
            var changeAgentModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeAgent);
            changeAgentModel.Agencies = CommonAgent.GetAgencies();
            if (changeAgentModel.Agencies.length > 0) {
                $.each(changeAgentModel.Agencies, function (index, value) {
                    if (value.Agent.AgentType == undefined) {
                        Object.defineProperty(changeAgentModel.Agencies[index].Agent, 'AgentType', {
                            value: { Id: value.AgentType.Id },
                            writable: true,
                            enumerable: true,
                            configurable: true
                        });
                    }
                });
            }
            changeAgentModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
            changeAgentModel.EndorsementFrom = FormatDate($("#inputCurrentFrom").val());
            changeAgentModel.CurrentTo = FormatDate(glbPolicyEndorsement.Endorsement.CurrentTo);
            changeAgentModel.ChangeAgentFrom = $("#inputCurrentFrom").val();
            lockScreen();
            AsynchronousProcess.CreateTemporal(glbPolicy.EndorsementController, changeAgentModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies)
                        }
                        ChangeAgent.LoadSummaryTemporal(data.result);
                        glbPolicy.Id = data.result.Id;
                        $("#btnPrintTemporalAgent").show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSaveChangeAgent, 'autoclose': true });
                        $("#btnPrintTemporalAgent").hide();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    $("#btnPrintTemporalAgent").hide();
                }
            });
        }
    }

    static validationAddAgents() {
        var bandCountAgents = true;
        if (!(parseInt($("#labelAgentsTotalParticipation").text()) == 100)) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPercentageTotal, 'autoclose': true });
            bandCountAgents = false;
        }
        var bandPrincipal = false;
        var agentAdd = $("#listAgencies").UifListView("getData");
        $.each(agentAdd, function (index, item) {
            if (String(item.IsPrincipal) === "true") {
                bandPrincipal = true;
            }
        });
        if (!bandPrincipal) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPrincipalIntermediary, 'autoclose': true });
        }
        if (bandCountAgents && bandPrincipal) {
            return true;
        }

    }

    SavePolicy() {
        ChangeAgent.CreateEndorsement();
    }
    static CreateEndorsement() {
        $('#formChangeAgent').validate();
        if ($('#formChangeAgent').valid()) {
            if (glbPolicy.Id != "") {
                var changeAgent = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeAgent);
                changeAgent.TemporalId = glbPolicy.Id;
                changeAgent.Id = glbPolicy.Id;
                changeAgent.CurrentTo = FormatDate(glbPolicy.CurrentTo);
                changeAgent.EndorsementFrom = FormatDate($("#inputCurrentFrom").val());
                changeAgent.ChangeAgentFrom = $("#inputCurrentFrom").val();
                changeAgent.Agencies = CommonAgent.GetAgenciesByList("#listAgenciesModify");
                $('#btnSave').attr('disabled', true);
                glbPolicy.Change = true;
                lockScreen();   
                AsynchronousProcess.CreateEndorsementChangeAgent(glbPolicy.EndorsementController, changeAgent).done(function (data) {
                    $('#btnSave').removeAttr('disabled');
                    if (data.success) {
                        if (data.result != null && data.result.length > 0) {
                            if (Array.isArray(data.result[0].InfringementPolicies) && data.result[0].InfringementPolicies.length > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(data.result[0].InfringementPolicies, glbPolicy.Id, FunctionType.Individual);
                            } else {
                                glbPolicy.EndorsementOriginId = glbPolicy.Endorsement.Id;
                                glbPolicy.Endorsement.Id = data.result[0].EndorsementId;
                                glbPolicy.Change = true;
                                glbPolicy.Endorsement.Number = data.result[0].EndorsementNumber;
                                glbPolicy.Id = 0;
                                var message = Resources.Language.SuccessfullyEndorsementGenerated + "." + Resources.Language.LabelNumberPolicy + ": " + glbPolicy.DocumentNumber + "." + Resources.Language.EndorsementNumber + ": " + data.result[0].Endorsement.Number;
                                $.UifDialog('confirm', { 'message': message + '\n ' + Resources.Language.MessagePrint }, function (result) {
                                    if (result) {
                                        $.each(data.result, function (index, item) {
                                            PrinterRequest.PrintReportEndorsement(glbPolicy.Prefix.Id, item.Endorsement.PolicyId, item.Endorsement.Id).done(function (data) {
                                                if (data.success) {
                                                    if (data.result.Url != undefined) {
                                                        DownloadFile(data.result.Url, true, (url) => url.match(/([^\\]+.pdf)/)[0]);
                                                    }
                                                }
                                                else {
                                                    if (data.result == Resources.Language.EndorsmentNotReinsured) {
                                                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotPrinter + ": " + data.result, 'autoclose': true });
                                                    }
                                                    else {
                                                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                                    }
                                                }
                                            }).fail(function (data) {
                                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                            });
                                        });
                                    }
                                    ChangeAgent.RedirectSearchController();
                                });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                        }

                    }
                    else {
                        unlockScreen();
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    unlockScreen();
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                });
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ModificationNoDone, 'autoclose': true });
            }
        }
    }

    static PrintTemporal() {
        AsynchronousProcess.GenerateReportTemporary(temporaryAgent.Endorsement.TemporalId, temporaryAgent.Prefix.Id, temporaryAgent.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }
}



class AsynchronousProcess {
    static CreateTemporal(endorsementController, changeAgentModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateTemporal',
            data: JSON.stringify({ changeAgentModel: changeAgentModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateEndorsementChangeAgent(endorsementController, changeAgent) {
        return $.ajax({
            type: 'POST',
            url: rootPath + endorsementController + '/CreateEndorsementChangeAgent',
            data: JSON.stringify({ changeAgentModel: changeAgent }),
            async: true,
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateReportTemporary(tempId, prefixId, operationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GenerateReportTemporary',
            data: JSON.stringify({ temporaryId: tempId, prefixId: prefixId, riskSince: 0, riskUntil: 0, operationId: operationId, tempAuthorization: false }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class AgentEndorsement {
    static ShowAgentEndorsement() {
        $('#formChangeAgent').validate();
        if ($('#formChangeAgent').valid()) {
            $("#modalAgents").UifModal('showLocal', Resources.Language.LabelDataAgents);
            commissionGral = $("#listAgenciesCurrent").UifListView("getData")[0].TotalPercentage;
            $("#inputAgentsPercentage").val(commissionGral);
            $("#inputAgentsPercentageAdditional").val(0);
            var agencies = jQuery.extend(true, [], $("#listAgenciesCurrent").UifListView("getData"));
            if (agencies.length > 0) {
                baseCalculate = agencies[0].BaseCalculate;
            }
            $('#listAgencies').UifListView({ displayTemplate: '#agencyTemplate', add: false, edit: true, delete: true, customEdit: true, customDelete: true, height: 250, sourceData: agencies });
        }
    }

    static AssignAgentPrincipal(agencies, agencyDelete) {
        $.each(agencies, function (index, item) {
            if (item.Id != agencyDelete.Id || item.Code != agencyDelete.Code) {
                if (String(agencyDelete.IsPrincipal) === "true") {
                    item.IsPrincipal = true;
                    agencyDelete.IsPrincipal = false;
                }
            }
        })
        return agencies;
    }

    static AddAgency() {
        if (CommonAgent.ValidAgency()) {
            var agentEdit = CommonAgent.GetFormAgent();
            var agencies = CommonAgent.GetListAgencies();
            var indexListAgentPrincipal = CommonAgent.GetIndexListAgentPrincipal();
            var indexListAgentEdit = CommonAgent.GetIndexListAgentEdit(agentEdit);
            var agenciesUpdate = CommonAgent.SetAgentPrincipal(agentEdit, agencies, indexListAgentPrincipal, indexListAgentEdit);
           
            agenciesUpdate = CommonAgent.ReCalculateParticipation(agencies, agentEdit, indexListAgentPrincipal, indexListAgentEdit, agenciesUpdate);

            if (agenciesUpdate != null) {
                agenciesUpdate = CommonAgent.UpdateCommission(agenciesUpdate, agentEdit, agenciesUpdate);
                CommonAgent.CalculateTotalParticipation(agenciesUpdate, 0);
                $.each(agenciesUpdate, function (key, item) {
                    item.Participation = item.Participation.toString().replace(separatorThousands, separatorDecimal);
                })
                CommonAgent.UpdateListAgencies(agenciesUpdate, 'agencyTemplate');
            }
            CommonAgent.ClearAgency();
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
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.UserNoHaveAgentEnabled, 'autoclose': true });
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchAgents, 'autoclose': true });
            });
        }
        return bandValidateAgentUser;

    }
    static HidePanelsIssuance() {
        $("#modalAgents").UifModal('hide');
    }

    static DeleteAgency(agencyDelete) {
        var agencies = CommonAgent.GetListAgencies();
        agencies = CommonAgent.AssignAgentMount(agencies, agencyDelete);
        agencies = CommonAgent.DeleteAgencyList(agencies, agencyDelete, []);
        CommonAgent.CalculateTotalParticipation(agencies, 0);
        agencies = CommonAgent.UpdateCommission(agencies, null);
        CommonAgent.UpdateListAgencies(agencies, 'agencyTemplate');
        CommonAgent.ClearAgency();
    }

    //Limpia el formulario
    //static clearAgentForm()
    //{
    //$("#inputAgentsAgent").val("");
    //$("#inputAgentsAgent").data("Object", null);
    //var participationTotal = calculateParticipation();
    //var participationAgent = (100 - parseFloat(participationTotal.toString().replace(separatorDecimal, separatorThousands)));

    //$("#inputAgentsParticipation").val(participationAgent);

    //$("#selectAgentsAgency").UifSelect({ source: null });
    //}

    static ClearAgency() {
        
        temporaryAgent = null
        $("#agentIsPrincipal").removeAttr("checked");
        $('#inputAgents').UifInputSearch('disabled', false);
        $('#inputAgents').data('Object', null);
        $('#inputAgents').val('');
        $('#inputAgentsParticipation').val(100 - parseFloat($('#labelAgentsTotalParticipation').text().replace(separatorDecimal, separatorThousands)));
        $('#inputAgentsPercentage').val('0');
        $('#inputAgentsPercentageAdditional').val('0');
    }


}
