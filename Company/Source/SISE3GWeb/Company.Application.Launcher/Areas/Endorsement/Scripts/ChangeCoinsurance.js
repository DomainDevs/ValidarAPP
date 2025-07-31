var gblChangeCoinsurance = {};
var coInsureType = null;
var CoInsAssignedIndex;
var temporaryCoInsurance = null;
class ChangeCoinsurance extends Uif2.Page {
    getInitialState() {
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
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

            $("#listChangeCoinsurance").UifListView({
                source: null,
                customDelete: false,
                customAdd: false,
                customEdit: false,
                add: false,
                edit: false,
                delete: false,
                displayTemplate: "#ChangeCoinsuranceTemplate",
                title: Resources.Language.LabelReasons,
                height: 200
            });

            $("#listChangeCoinsuranceModify").UifListView({
                source: null,
                customDelete: false,
                customAdd: false,
                customEdit: false,
                add: false,
                edit: false,
                delete: false,
                displayTemplate: "#ChangeCoinsuranceModifyTemplate",
                title: Resources.Language.LabelReasons,
                height: 200
            });

            $("#BusinessType").text(glbPolicy.BusinessTypeDescription);
            $('#inputIssueDate').text(FormatFullDate(glbPolicy.IssueDate));

            if (glbPolicy.Endorsement.Id > 0) {
                $('#inputIssueDate').UifDatepicker('disabled', true);
                ChangeCoinsurance.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
            }

            if (glbPolicy.Id > 0) {
                $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
                ChangeCoinsurance.GetTemporalById(glbPolicy.EndorsementController, glbPolicy.Id);
            }

            ChangeCoinsurance.LoadChangeCoinsuranceData(glbPolicy.CoInsuranceCompanies);
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
            $('#listChangeCoinsuranceModify').UifListView({ displayTemplate: '#ChangeCoinsuranceModifyTemplate', height: 250 });
            UnderwritingCoInsurance.GetBusinessTypes();
            UnderwritingCoInsurance.initialize();
            //$("#listCoInsuranceAcceptedAgents").UifListView({ displayTemplate: "#CoInsuranceAcceptedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
            $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
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
            $('#btnPrintTemporalCoInsurance').hide();
        }
        $('#inputRecord').prop('disabled', true);
        if (glbPolicy.EndorsementTexts != undefined && glbPolicy.EndorsementTexts.length > 0) {
            var recordText = "";
            glbPolicy.EndorsementTexts.forEach(function (item) {
                recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
            });
            $('#inputRecord').val(recordText);
        }
        this.ValidatePolicies();
    }

    bindEvents() {
        $('#btnCancel').on('click', ChangeCoinsurance.RedirectSearchController);
        $("#btnChangeCoinsurance").on('click', function () {
            ChangeCoinsurance.ShowSaveChangeCoinsurance();
        });
        $("#btnChangeCoinsuranceEndorsement").on('click', function () {
            UnderwritingCoInsurance.LoadPartialCoinsurance();
            $("#selectCoInsBusinessType").prop("disabled", true);
            $("#selectCoInsBusinessType").UifSelect("setSelected", glbPolicy.BusinessType);
        });
        $("#inputCurrentFrom").on("datepicker.change", function (event, date) {
            ChangeCoinsurance.ChangeCoinsuranceDate($("#inputCurrentFrom").val(), glbPolicyEndorsement.Endorsement.CurrentTo);
        });

        $("#btnPrintTemporalCoInsurance").click(ChangeCoinsurance.PrintTemporal);

        //Aceptado
        $('#inputCoInsAcceptedLeadingInsurer').on('itemSelected', UnderwritingCoInsurance.ChangeCoInsAccepted);
        $("#btnCoInsAcceptedAgent").on("click", UnderwritingCoInsurance.CoInsAcceptedAgent);
        $("#btnCoInsAcceptedAgentNew").on("click", ChangeCoinsurance.ClearAcceptedAgent);
        $('#inputCoInsAcceptedParticipation').focusout(UnderwritingCoInsurance.CalculateAcceptedPercentage);
        $('#inputAccepAgentsAgency').on('itemSelected', UnderwritingCoInsurance.ChangeCoInsAcceptedAgent);
        $('#inputAccepAgentsAgency').on('buttonClick', UnderwritingCoInsurance.SearchAgentsAgency);
        $('#tblResultListAcceptedAgents').on('rowSelected', UnderwritingCoInsurance.SelectSearch);

        //Cedido
        $('#inputCoInsAssignedInsured').on('itemSelected', UnderwritingCoInsurance.ChangeCoInsAssigned);
        $('#btnCoInsAssignedAccept').on("click", UnderwritingCoInsurance.CoInsAssignedAccept);
        $("#btnCoInsuranceSave").on("click", ChangeCoinsurance.CreateTemporal);
        $("#btnSave").on('click', ChangeCoinsurance.SavePolicy);

        $('#listChangeCoinsuranceModify').on('change', ChangeCoinsurance.showmensage);
        $('#listChangeCoinsuranceModify').on('click', ChangeCoinsurance.showmensage);
        

        $('#listChangeCoinsurance').on('change', ChangeCoinsurance.showmensage);
        $('#listChangeCoinsurance').on('click', ChangeCoinsurance.showmensage);

        
    
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

    static showmensage() {
        var item = $("#listChangeCoinsuranceModify").on("getSelected").text().trim().split('Participacion');
        if (item.length > 1) {
            var itemarray = item[1].trim().split("  ");
            var ParticipationPercentage = itemarray[0].trim().split(' ');
            var ExpensesPercentage = itemarray[2].trim().split(' ');


            $("#InputPercent").val(ParticipationPercentage[1].trim());
            $("#InputExchange").val(ExpensesPercentage[1].trim());
        }
    }

    static ShowSaveChangeCoinsurance() {
        $("#modalSaveConsolidationChange").UifModal('showLocal', Resources.Language.LabelDataConsolidationChange);
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
    }

    static LoadChangeCoinsuranceData(CoinsuranceCompany) {
        $("#listChangeCoinsurance").UifListView("refresh");
        $.each(CoinsuranceCompany, function (entryIndex, entry) {
            $("#listChangeCoinsurance").UifListView("addItem", this);
            $("#InputPercent").val(entry.ParticipationPercentage);
            $("#InputExchange").val(entry.ExpensesPercentage);
        });

    }

    static ShowModalBusinessType(BusinessType) {
        //Coasegurado Aceptado
        if (BusinessType == 2) {
            ChangeCoinsurance.ChangeCoInsBusinessType(event, BusinessType);
        } else {
            //Coasegurado Aceptado
            if (BusinessType == 3) {
                ChangeCoinsurance.ChangeCoInsBusinessType(event, BusinessType);
            }
        }
    }

    static ChangeCoInsBusinessType(event, selectedItem) {

        UnderwritingCoInsurance.ClearForm();
        event.stopImmediatePropagation();
        event.preventDefault();
        coInsureType = selectedItem;
        UnderwritingCoInsurance.ShowPanelBusinessType(selectedItem);
        $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
    }

    static ChangeCoinsuranceDate(modificationDate, currentTo) {
        if (CompareBetweenDates(modificationDate, FormatDate(glbPolicy.CurrentFrom), FormatDate(glbPolicy.CurrentTo))) {
            $('#inputDays').val(CalculateDays(modificationDate, currentTo));
        } else {
            $('#inputCurrentFrom').val("");
            $('#inputDays').val("");
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.LabelDateToDatePolicy, 'autoclose': true });
        }
    }

    static LoadCoinsuranceData(Coinsurance, nameListView) {
        $(nameListView).UifListView({ displayTemplate: '#ChangeCoinsuranceModifyTemplate', add: false, edit: false, delete: false, height: 250 });

        if (Coinsurance != null) {
            $.each(Coinsurance, function (entryIndex, entry) {
                $("#listChangeCoinsuranceModify").UifListView("addItem", this);
                $("#InputPercent").val(entry.ParticipationPercentage);
                $("#InputExchange").val(entry.ExpensesPercentage);
            });
        }
        $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
    }

    static LoadSummaryTemporal(temporalData) {
        glbPolicy.Id = temporalData.Id;
        ChangeCoinsurance.HidePanelsIssuance();
        ChangeCoinsurance.LoadCoinsuranceData(temporalData.CoInsuranceCompanies, "#listChangeCoinsuranceModify");
        temporaryCoInsurance = temporalData;
        if (temporalData.Summary != null) {
            $('#labelRisk').text(temporalData.Summary.RiskCount);
            $('#labelSum').text(FormatMoney(temporalData.Summary.AmountInsured));
            $('#labelPremium').text(FormatMoney(temporalData.Summary.Premium));
            $('#labelExpenses').text(FormatMoney(temporalData.Summary.Expenses));
            $('#labelTaxes').text(FormatMoney(temporalData.Summary.Taxes));
            $('#labelTotalPremium').text(FormatMoney(temporalData.Summary.FullPremium));
        }
    }

    static CreateTemporal() {
        $("#formCoInsurance").validate();
        if (!UnderwritingCoInsurance.ValidateGeneral($('#selectCoInsBusinessType').UifSelect("getSelected"))) {
            return false;
        }
        var valid = true;

        if ($('#selectCoInsBusinessType').UifSelect("getSelected") == BusinessType.Accepted) {
            $("#formCoInsurance").validate();
            valid = $("#formCoInsurance").valid();

            var percentageOrigin = parseFloat($('#inputCoInsAcceptedParticipationTotal').val().replace(separatorDecimal, separatorThousands));
            var percentage = parseFloat($('#inputCoInsAcceptedParticipation').val().replace(separatorDecimal, separatorThousands));
            if (percentageOrigin + percentage > 100) {
                $('#inputCoInsAcceptedLeadingPolicy').ValidatorKey(6, 1, 1);
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateMajorPercentage, 'autoclose': true });
                return false;
            }
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
            //creacion de temporla
            var ChangeCoinsuranceViewModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeCoinsurance);
            ChangeCoinsuranceViewModel.Agencies = glbPolicy.Agencies;
            ChangeCoinsuranceViewModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
            ChangeCoinsuranceViewModel.EndorsementFrom = FormatDate($("#inputCurrentFrom").val());
            ChangeCoinsuranceViewModel.CurrentTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
            ChangeCoinsuranceViewModel.ChangeCoinsuranceFrom = $("#inputCurrentFrom").val();
            ChangeCoinsuranceViewModel.AcceptedParticipationPercentageOwn = $('#inputCoInsAcceptedParticipationTotal').val();
            ChangeCoinsuranceViewModel.AcceptedCoinsurerId = $('#inputCoInsAcceptedLeadingInsurer').data('Id');
            ChangeCoinsuranceViewModel.AssignedParticipationPercentageOwn = $('#inputCoInsAssignedParticipationTotal').val();
            ChangeCoinsuranceViewModel.AssignedCoinsurerId = $('#inputCoInsAssignedInsured').data('Id');
            if (glbPolicy.BusinessType == 3) {
                var participationPercentageOwn = 100;
                ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies = $("#listCoInsuranceAssigned").UifListView("getData");
                ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies.forEach(function (item, index) {
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[index].ParticipationPercentage = item.Participation;
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[index].ExpensesPercentage = item.Expenses;
                    participationPercentageOwn -= item.Participation; 
                });
                ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies.forEach(function (item, index) {
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[index].ParticipationPercentageOwn = participationPercentageOwn;
                });
            } else {
                if (glbPolicy.BusinessType == 2) {
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies = [];
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0] = {};
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].Id = $('#inputCoInsAcceptedLeadingInsurer').data('Id');
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].Description = $("#inputCoInsAcceptedLeadingInsurer").val();
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].ParticipationPercentage = $("#inputCoInsAcceptedParticipation").val();
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].ExpensesPercentage = $("#inputCoInsAcceptedExpenses").val();
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].ParticipationPercentageOwn = $("#inputCoInsAcceptedParticipationTotal").val();
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].PolicyNumber = $("#inputCoInsAcceptedLeadingPolicy").val();
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].EndorsementNumber = $("#inputCoInsAcceptedEndorsement").val;
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].acceptCoInsuranceAgent = [];

                    //$("#listCoInsuranceAcceptedAgents").UifListView("getData").forEach(function (item, index) {
                    //    var acceptCoInsuranceAgent = {};
                    //    acceptCoInsuranceAgent.Agent = {};
                    //    acceptCoInsuranceAgent.Agent.IndividualId = item.IndividualId;
                    //    acceptCoInsuranceAgent.Agent.Id = item.Id;
                    //    acceptCoInsuranceAgent.Agent.FullName = item.Description;
                    //    acceptCoInsuranceAgent.ParticipationPercentage = item.Participation;

                    //    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].acceptCoInsuranceAgent.push(acceptCoInsuranceAgent);

                    //});
                }
            }

            ChangeCoinsuranceRequest.CreateTemporal(glbPolicy.EndorsementController, ChangeCoinsuranceViewModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies)
                        }
                        ChangeCoinsurance.LoadSummaryTemporal(data.result);
                        glbPolicy.Id = data.result.Id;
                        glbPolicy.CoInsuranceCompanies = data.result.CoInsuranceCompanies;
                        $('#btnPrintTemporalCoInsurance').show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        $('#btnPrintTemporalCoInsurance').hide();
                    }
                }
                else {
                    $('#btnPrintTemporalCoInsurance').hide();
                }
            });
        }
    }

    static CreateTemporalAcceptedCoinsurance() {
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

    static HidePanelsIssuance() {
        $("#modalCoInsurance").UifModal('hide');
    }

    static SavePolicy() {
        ChangeCoinsurance.CreateEndorsement();
    }

    static CreateEndorsement() {
        $('#formCoInsurance').validate();
        if ($('#formCoInsurance').valid()) {
            if (glbPolicy.Id != "") {
                var ChangeCoinsuranceViewModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeCoinsurance);
                ChangeCoinsuranceViewModel.TemporalId = glbPolicy.Id;
                ChangeCoinsuranceViewModel.Id = glbPolicy.Id;
                ChangeCoinsuranceViewModel.CurrentTo = FormatDate(glbPolicy.CurrentTo);
                ChangeCoinsuranceViewModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
                ChangeCoinsuranceViewModel.ChangeCoinsuranceFrom = $("#inputCurrentFrom").val();
                ChangeCoinsuranceViewModel.AcceptedParticipationPercentageOwn = $('#inputCoInsAcceptedParticipationTotal').val();
                ChangeCoinsuranceViewModel.AcceptedCoinsurerId = $('#inputCoInsAcceptedLeadingInsurer').data('Id');
                ChangeCoinsuranceViewModel.AssignedParticipationPercentageOwn = $('#inputCoInsAssignedParticipationTotal').val();
                ChangeCoinsuranceViewModel.AssignedCoinsurerId = $('#inputCoInsAssignedInsured').data('Id');
                if (glbPolicy.BusinessType == 3) {
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies = $("#listChangeCoinsuranceModify").UifListView("getData");
                    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies.forEach(function (item, index) {
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[index].ParticipationPercentage = item.Participation;
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[index].ExpensesPercentage = item.Expenses;

                    });
                } else {
                    if (glbPolicy.BusinessType == 2) {
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies = [];
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0] = {};
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].Id = $('#inputCoInsAcceptedLeadingInsurer').data('Id');
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].Description = $("#inputCoInsAcceptedLeadingInsurer").val();
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].ParticipationPercentage = $("#inputCoInsAcceptedParticipationTotal").val();
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].ExpensesPercentage = $("#inputCoInsAcceptedExpenses").val();
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].ParticipationPercentageOwn = $("#inputCoInsAcceptedParticipation").val();
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].PolicyNumber = $("#inputCoInsAcceptedLeadingPolicy").val();
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].EndorsementNumber = $("#inputCoInsAcceptedEndorsement").val;
                        ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].acceptCoInsuranceAgent = [];

                        //$("#listCoInsuranceAcceptedAgents").UifListView("getData").forEach(function (item, index) {
                        //    var acceptCoInsuranceAgent = {};
                        //    acceptCoInsuranceAgent.Agent = {};
                        //    acceptCoInsuranceAgent.Agent.IndividualId = item.IndividualId;
                        //    acceptCoInsuranceAgent.Agent.Id = item.Id;
                        //    acceptCoInsuranceAgent.Agent.FullName = item.Description;
                        //    acceptCoInsuranceAgent.ParticipationPercentage = item.Participation;
                            
                        //    ChangeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies[0].acceptCoInsuranceAgent.push(acceptCoInsuranceAgent);
                           
                        //});
                    }
                }
                $('#btnSave').attr('disabled', true);
                glbPolicy.Change = true;
                ChangeCoinsuranceRequest.CreateEndorsementChangeCoinsurance(glbPolicy.EndorsementController, ChangeCoinsuranceViewModel).done(function (data) {
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
                                        $.each(data.result, function (index, value) {
                                            PrinterRequest.PrintReportEndorsement(glbPolicy.Prefix.Id, value.Endorsement.PolicyId, value.Endorsement.Id).done(function (data) {
                                                if (data.success) {
                                                    if (data.result.Url != undefined) {
                                                        DownloadFile(data.result.Url, true, (url) => url.match(/([^\\]+.pdf)/)[0]);
                                                    }
                                                    else {
                                                        $.UifNotify('show', { 'type': 'info', 'message': 'Se genero el proceso de impresion numero ' + data.result + ' favor consultarlo en pantalla de impresion', 'autoclose': false });
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
                                    ChangeCoinsurance.RedirectSearchController();
                                });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                        }

                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorRecordTemporary, 'autoclose': true });
                });
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ModificationNoDone, 'autoclose': true });
            }
        }
    }
    static ChangeCoInsBusinessType(event, selectedItem) {

        UnderwritingCoInsurance.ClearForm();
        event.stopImmediatePropagation();
        event.preventDefault();
        coInsureType = selectedItem.Id;
        UnderwritingCoInsurance.ShowPanelBusinessType(selectedItem.Id);
        $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
    }

    static GetTemporalById(endorsementController, temporalId) {

        ChangeCoinsuranceRequest.GetTemporalById(endorsementController, temporalId).done(function (data) {
            if (data.success) {
                ChangeCoinsurance.LoadEndorsementData(data.result);
                $('#inputDays').val(CalculateDays($('#inputCurrentFrom').val(), glbPolicy.CurrentTo));
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static LoadEndorsementData(policy) {
        $('#inputIssueDate').text(FormatFullDate(glbPolicy.IssueDate));
        $('#inputIssueDate').UifDatepicker('setValue', (FormatDate(glbPolicy.IssueDate)));
        $('#inputCurrentFrom').UifDatepicker('setValue', FormatDate(policy.Endorsement.CurrentFrom));
        $('#inputCurrentTo').UifDatepicker('setValue', FormatDate(policy.CurrentTo));
        $('#inputChangeAgentDays').val(CalculateDays($('#inputCurrentFrom').val(), FormatDate(policy.CurrentTo)));
        $('#inputText').val(policy.Endorsement.Text.TextBody);
        $('#inputObservations').val(policy.Endorsement.Text.Observations);
        $("#inputTicketNumber").val(policy.Endorsement.TicketNumber);
        $("#inputTicketDate").UifDatepicker('setValue', FormatDate(policy.Endorsement.TicketDate));
        ChangeCoinsurance.LoadCoinsuranceData(policy.CoInsuranceCompanies, "#listChangeCoinsuranceModify");
    }

    static PrintTemporal() {
        ChangeCoinsuranceRequest.GenerateReportTemporary(temporaryCoInsurance.Endorsement.TemporalId, glbPolicy.Prefix.Id, temporaryCoInsurance.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }

    static ClearAcceptedAgent() {
        
        $('#inputCoInsAcceptedParticipationTotal').val('');
        $('#inputCoInsAcceptedParticipation').val('');
        $('#inputCoInsAcceptedExpenses').val('');
        $('#inputCoInsAcceptedLeadingPolicy').val('');
        $('#inputCoInsAcceptedLeadingInsurer').val('');
        
        UnderwritingCoInsurance.NewAgent();
    }

}