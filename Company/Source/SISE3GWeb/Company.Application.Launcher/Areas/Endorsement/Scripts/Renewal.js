var OperationQuotaEvent;
var ConsortiumEvent;
var EconomicGroupEvent;
var OperationQuotaDecline;
var ConsortiumDecline;
var EconomicGroupDecline;
var ValidityParticipant1;
var CurrentDateQuota = new Date();
var TotalSumInsuredRisk;
var IndividualId;
var SellAmount;
var IsConsortium;

class Renewal extends Uif2.Page {
    getInitialState() {
        if (glbPolicy != null && glbPolicy.DocumentNumber != 0) {
            $("#inputFilingNumber").OnlyDecimals(0);
            $('#inputModifyTo').UifDatepicker();
            $('#inputModifyFrom').UifDatepicker();
            $('#inputFilingDate').UifDatepicker();
            $('.uif-datepicker').datepicker({
                format: DateFormat
            });
            $('#inputCurrentFrom').UifDatepicker('disabled', true);
            $('#inputCurrentTo').UifDatepicker('disabled', true);
            $('#inputModifyFrom').UifDatepicker('disabled', true);
            $('#inputIssueDate').UifDatepicker('disabled', true);
            $('#inputRecord').prop('disabled', true);

            if (glbPolicy.Id > 0) {
                Renewal.GetTemporalById(glbPolicy.Id);
            }
            else {
                Renewal.ShowPanels(false);
            }
            $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
            GetDateIssue();
            this.ValidatePolicies();
            Renewal.LoadRecordObservation();
        }
    }

    bindEvents() {
        $('#inputModifyTo').on("datepicker.change", function (event, date) {
            var days = CalculateDays($("#inputModifyFrom").val(), $('#inputModifyTo').val());
            if (days < 0) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDateVNoLess, 'autoclose': true });
                $('#inputModifyTo').val($("#inputModifyFrom").val());
                $("#inputModifyDays").val(0);
                return;
            }
            $("#inputModifyDays").val(days);
        });
        $("#btnSaveRenewal").on('click', function () {
            $("#formRenewal").validate();
            if ($("#formRenewal").valid()) {
                if (glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO || glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO) {
                    return Renewal.GetInformationAfianzado(individualId).then(function (data) {
                        var avalible = null;
                        var dateTo = null;
                        if (EconomicGroupEvent != undefined) {
                            //Grupo Economico
                            avalible = EconomicGroupEvent.OperationCuotaAvalible;
                            dateTo = EconomicGroupEvent.DateTo;
                        } else if (ConsortiumEvent != undefined && IsConsortium) {
                            //Consorcio
                            avalible = ConsortiumEvent.OperationCuotaAvalible;
                            dateTo = ConsortiumEvent.DateTo;
                        } else if (OperationQuotaEvent != undefined && ConsortiumEvent != undefined) {
                            //Participante Consorcio
                            avalible = ConsortiumEvent.OperationCuotaAvalible;
                            dateTo = ConsortiumEvent.DateTo;
                        } else if (OperationQuotaEvent != null && ConsortiumEvent == undefined) {
                            // Individual
                            avalible = OperationQuotaEvent.OperationCuotaAvalible;
                            dateTo = OperationQuotaEvent.DateTo;
                        }
                        if (OperationQuotaEvent.DateTo >= FormatDate(glbPolicy.CurrentFrom)) {
                            if (avalible >= parseFloat(NotFormatMoney(glbPolicy.Summary.AmountInsured).replace(',', '.'))) {
                                glbPolicy.Endorsement.CurrentFrom = $('#inputModifyFrom').val();
                                glbPolicy.Endorsement.CurrentTo = $('#inputModifyTo').val();
                                renewalModel = null;
                                renewalModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formRenewal);
                                renewalModel.IsUnderIdenticalConditions = true;
                                renewalModel.EndorsementFrom = FormatDate(glbPolicy.Endorsement.CurrentFrom);
                                renewalModel.EndorsementTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
                                renewalModel.Id = glbPolicy.Id;
                                Renewal.CreateEndorsement();
                            }
                            else {
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true });
                            }
                        }
                        else {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateOperatingQuota, 'autoclose': true });
                        }
                    });
                }
                else {
                    glbPolicy.Endorsement.CurrentFrom = $('#inputModifyFrom').val();
                    glbPolicy.Endorsement.CurrentTo = $('#inputModifyTo').val();
                    renewalModel = null;
                    renewalModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formRenewal);
                    renewalModel.IsUnderIdenticalConditions = true;
                    renewalModel.EndorsementFrom = FormatDate(glbPolicy.Endorsement.CurrentFrom);
                    renewalModel.EndorsementTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
                    renewalModel.Id = glbPolicy.Id;
                    Renewal.CreateEndorsement();
                }

            }
        });
        $("#btnIdenticalCondition").on('click', function () {
            glbPolicy.IsUnderIdenticalConditions = true;
            Renewal.ShowPanels(true);
            var renewalModel = null;
            renewalModel = $("#formRenewal").serializeObject();
            renewalModel.IsUnderIdenticalConditions = true;
            renewalModel.Id = glbPolicy.Id,
                renewalModel.PolicyId = glbPolicy.Endorsement.PolicyId,
                renewalModel.EndorsementId = glbPolicy.Endorsement.Id,
                renewalModel.EndorsementTypeOriginal = glbPolicy.Endorsement.EndorsementType,
                renewalModel.EndorsementType = EndorsementType.Renewal,
                renewalModel.EndorsementTemporal = glbPolicy.EndorsementTemporal,
                renewalModel.IsUnderIdenticalConditions = glbPolicy.IsUnderIdenticalConditions
            Renewal.CreateTemporal(renewalModel);
        });
        $("#btnOtherCondition").on('click', function () {
            glbPolicy.IsUnderIdenticalConditions = false;
            renewalModel = $("#formRenewal").serializeObject();
            renewalModel.IsUnderIdenticalConditions = false;
            Renewal.CreateTemporal(renewalModel);
        });
        $("#btnCancel").on('click', function () {
            Renewal.RedirectSearchController();
        });
        $("#btnCancelTemp").on('click', function () {
            Renewal.RedirectSearchController();
        });
        $("#btnPrintTemporalRenewal").click(Renewal.PrintTemporal);
    }

    ValidatePolicies() {
        if (glbPolicy.HasEvent) {
            $.UifDialog('confirm', { 'message': glbPolicy.Message }, function (responsePolicies) {
                if (responsePolicies) {
                    let id = glbPolicy.Id;
                    RequestSummaryAuthorization.DeleteNotificationByTemporalId(id, FunctionType.Individual).done()
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                        });
                }
                else {
                    Renewal.RedirectSearchController();
                }
            });
        }
    }

    static GetInformationAfianzado(InsuredId) {
        var IndividualId = glbPolicy.Holder.IndividualId;
        var isConsortiumTemp = false;
        var LinebusinessId = glbPolicy.Prefix.Id;
        SellAmount = glbPolicy.ExchangeRate.SellAmount;
        EconomicGroupEvent = ConsortiumEvent = OperationQuotaEvent = null;
        var array = new Array();
        var array1 = new Array();

        if (NotFormatMoney(glbPolicy.Summary.AmountInsured).replace(',', '.') == "0" || NotFormatMoney(glbPolicy.Summary.AmountInsured).replace(',', '.') == "") {
            TotalSumInsuredRisk = 0;
        } else {
            TotalSumInsuredRisk = parseFloat(NotFormatMoney(glbPolicy.Summary.AmountInsured).replace(',', '.'));
        }
        //Marca para tipo de Afianzado
        array1.push(
            new Promise((resolve, reject) => {
                OperationQuotaCumulusRequest.GetSecureType(IndividualId, LinebusinessId).done(function (dataMarca) {
                    if (dataMarca.result) {
                        glbPolicy.IsEconomicGroup = dataMarca.result.IsEconomicGroup;
                        glbPolicy.IsConsortium = dataMarca.result.IsConsortium;
                        glbPolicy.IsIndividual = dataMarca.result.IsIndividual;
                        glbPolicy.IsNotIndividual = dataMarca.result.IsNotIndividual;
                        lockScreen();
                    }
                    resolve(dataMarca);
                }).fail(xhr => {
                    reject(xhr);
                });
            }));

        lockScreen();

        array.push(
            new Promise((resolve, reject) => {
                RiskSuretyRequest.IsConsortiumindividualId(IndividualId).done(function (data) {
                    if (data.result) {
                        isConsortiumTemp = true;
                        IsConsortium = isConsortiumTemp;
                        resolve(data);
                    } else {
                        if (glbPolicy.IsIndividual) {
                            OperationQuotaCumulusRequest.GetOperatingQuotaEventByIndividualIdByLineBusinessId(IndividualId, LinebusinessId)
                                .done((OperationQuota) => {
                                    if (OperationQuota.success) {
                                        if (OperationQuota.result) {
                                            if (OperationQuota.result.IdentificationId != 0) {
                                                OperationQuotaEvent = {
                                                    Id: OperationQuota.result.OperatingQuotaEventID,
                                                    Belongingto: '1.' + ($("#inputSecure").data("Object") ? $("#inputSecure").data("Object").Name : ""),
                                                    OperationCuotaInitial: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount,
                                                    OperationCuotaAvalible: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount - OperationQuota.result.ApplyEndorsement.AmountCoverage / SellAmount,
                                                    DateTo: FormatDate(OperationQuota.result.IndividualOperatingQuota.EndDateOpQuota),
                                                    DateRegisty: FormatDate(OperationQuota.result.IssueDate),
                                                    Cumulu: OperationQuota.result.ApplyEndorsement.AmountCoverage / SellAmount,
                                                    InitDateOpQuota: OperationQuota.result.IndividualOperatingQuota.InitDateOpQuota,
                                                    EndDateOpQuota: OperationQuota.result.IndividualOperatingQuota.InitDateOpQuota
                                                }
                                                if (OperationQuota.result.declineInsured != null) {
                                                    OperationQuotaDecline = {
                                                        DeclineDate: OperationQuota.result.declineInsured.DeclineDate,
                                                        Decline: OperationQuota.result.declineInsured.Decline
                                                    }
                                                }
                                                $('#_chkIsIndividual').prop("checked", true);
                                                isConsortiumTemp = false;
                                            }
                                        }
                                    }
                                    resolve(OperationQuota);
                                });
                        }
                    }

                }).fail(xhr => {
                    reject(xhr);
                })
            }));
		var endoso = false;

//		if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification || glbPolicy.Endorsement.EndorsementType == EndorsementType.Cancellation || glbPolicy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension) { endoso = true; }

        if (glbPolicy.IsConsortium) {
            array.push(
                new Promise((resolve, reject) => {
					OperationQuotaCumulusRequest.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(IndividualId, LinebusinessId, endoso, glbPolicy.Endorsement.Id)
                        .done((Consortium) => {
                            if (Consortium.success) {
                                if (Consortium.result) {
                                    if (Consortium.result.consortiumEventDTO != null) {

                                        ConsortiumEvent = {
                                            Id: Consortium.result.OperatingQuotaEventID,
                                            OperationQuotaConsortium: Consortium.result.consortiumEventDTO.OperationQuotaConsortium / SellAmount,
                                            OperationCuotaInitial: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount,
                                            OperationCuotaAvalible: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount - Consortium.result.ApplyEndorsement.AmountCoverage / SellAmount,
                                            DateTo: FormatDate(Consortium.result.IndividualOperatingQuota.EndDateOpQuota),
                                            DateRegisty: FormatDate(Consortium.result.IssueDate),
                                            Cumulu: Consortium.result.ApplyEndorsement.AmountCoverage / SellAmount,
                                        }
                                        if (Consortium.result.consortiumEventDTO.consortiumDTO != null) {
                                            ConsortiumEvent.AssociationType = Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType;
                                            if (Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.CONSORTIUM) {
                                                $('#_chkIsConsortium').prop("checked", true);
                                                ConsortiumEvent.Belongingto = '2.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName;
                                            } else if (Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.TEMPORAL_UNION) {
                                                $('#_chkIsUT').prop("checked", true);
                                                ConsortiumEvent.Belongingto = '4.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName;
                                            } else if (Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.FUTURE_SOCIETY) {
                                                $('#_chkIsSF').prop("checked", true);
                                                ConsortiumEvent.Belongingto = '5.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName;
                                            }
                                        }

                                        if (Consortium.result.declineInsured != null) {
                                            ConsortiumDecline = {
                                                DeclineDate: Consortium.result.declineInsured.DeclineDate,
                                                Decline: Consortium.result.declineInsured.Decline
                                            }
                                        }
                                    }
                                }
                            }
                            resolve(Consortium);
                        })
                        .fail(xhr => {
                            reject(xhr);
                        })
                }));

            array.push(
                new Promise((resolve, reject) => {
                    OperationQuotaCumulusRequest.GetValidityParticipantCupoInConsortium(IndividualId, TotalSumInsuredRisk, LinebusinessId)
                        .done(function (ValidityParticipant) {
                            if (ValidityParticipant.success) {
                                if (ValidityParticipant.result.length > 0) {
                                    ValidityParticipant1 = ValidityParticipant.result;
                                } else {
                                    ValidityParticipant1 = null;
                                }
                            }
                            resolve(ValidityParticipant);
                        }).fail(xhr => {
                            reject(xhr);
                        })
                }));
        }
        if (glbPolicy.IsEconomicGroup) {
            array.push(
                new Promise((resolve, reject) => {
                    OperationQuotaCumulusRequest.GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(IndividualId, LinebusinessId)
                        .done(function (EconomicGroup) {
                            if (EconomicGroup.success) {
                                if (EconomicGroup.result.EconomicGroupEventDTO != null) {
                                    EconomicGroupEvent = {
                                        Id: EconomicGroup.result.OperatingQuotaEventID,
                                        Belongingto: '3.' + EconomicGroup.result.EconomicGroupEventDTO.economicgroupoperatingquotaDTO.EconomicGroupName,
                                        OperationCuotaInitial: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount,
                                        OperationCuotaAvalible: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount - EconomicGroup.result.ApplyEndorsement.AmountCoverage / SellAmount,
                                        DateTo: FormatDate(EconomicGroup.result.IndividualOperatingQuota.EndDateOpQuota),
                                        DateRegisty: FormatDate(EconomicGroup.result.IssueDate),
                                        Cumulu: EconomicGroup.result.ApplyEndorsement.AmountCoverage / SellAmount,
                                    }
                                    $('#_chkIsEconomicGroup').prop("checked", true);

                                    if (EconomicGroup.result.declineInsured != null) {
                                        EconomicGroupDecline = {
                                            DeclineDate: EconomicGroup.result.declineInsured.DeclineDate,
                                            Decline: EconomicGroup.result.declineInsured.Decline
                                        }
                                    }
                                }
                            }
                            resolve(EconomicGroup);
                        }).fail(xhr => {
                            reject(xhr);
                        })
                }));
        }


        array.push(
            new Promise((resolve, reject) => {
                UnderwritingRequest.GetModuleDateIssue().done(function (data) {
                    if (data.success) { CurrentDateQuota = FormatDate(data.result); }
                    resolve(data);
                }).fail(xhr => {
                    reject(xhr);
                });
            }));

        return Promise.all(array);
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

    static GetTemporalById(temporalId) {

        RenewalRequest.GetTemporalById(temporalId, glbPolicy.EndorsementController).done(function (data) {
            if (data.success) {
                if (data.result.Endorsement.IsUnderIdenticalConditions) {
                    Renewal.LoadEndorsementData(data.result);
                    if (glbPolicyEndorsement.ExchangeRate != undefined && glbPolicyEndorsement.ExchangeRate != null) {
                        Renewal.GetExchangeRateByCurrencyId(glbPolicyEndorsement.ExchangeRate.Currency.Id);
                        if (glbPolicy.IsUnderIdenticalConditions && (glbPolicy.Prefix.Id == PrefixType.RESPONSABILIDA_CIVIL || glbPolicy.Prefix.Id == PrefixType.CAUCION_JUDICIAL || glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO)) {
                            renewalModel = null;
                            renewalModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formRenewal);
                            renewalModel.IsUnderIdenticalConditions = true;
                            renewalModel.EndorsementFrom = FormatDate(glbPolicy.Endorsement.CurrentFrom);
                            renewalModel.EndorsementTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
                            renewalModel.Id = glbPolicy.Id;
                            RenewalRequest.CreateTemporal(renewalModel, glbPolicy.EndorsementController).done(function (data) { Renewal.LoadEndorsementData(data.result); });
                        }
                    }
                } else {
                    Renewal.LoadRiskView();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });
    }

    static LoadEndorsementData(policy) {
        Renewal.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
        if (policy.Endorsement.IsUnderIdenticalConditions) {
            glbPolicy.IsUnderIdenticalConditions = true;
            Renewal.ShowPanels(true);
        }
        glbPolicy.Id = policy.Id;
        glbPolicy.Endorsement.CurrentFrom = FormatDate(policy.CurrentFrom);
        glbPolicy.Endorsement.CurrentTo = FormatDate(policy.CurrentTo);
        if (glbPolicyEndorsement.Endorsement.TemporalId == 0) {
            glbPolicyEndorsement.Endorsement.TemporalId = policy.Endorsement.TemporalId;
        }
        $("#inputModifyFrom").UifDatepicker('setValue', FormatDate(policy.CurrentFrom));
        $("#inputModifyTo").UifDatepicker('setValue', FormatDate(policy.CurrentTo));
        $('#labelRisk').text(policy.Summary.RiskCount);
        $('#labelSum').text(FormatMoney(policy.Summary.AmountInsured));
        $('#labelPremium').text(FormatMoney(policy.Summary.Premium));
        $('#labelExpenses').text(FormatMoney(policy.Summary.Expenses));
        $('#labelTaxes').text(FormatMoney(policy.Summary.Taxes));
        $('#labelTotalPremium').text(FormatMoney(policy.Summary.FullPremium));

        $("#inputModifyDays").val(CalculateDays($("#inputModifyFrom").val(), $("#inputModifyTo").val()));

        if (!policy.Product.IsCollective) {
            $('#btnRiskTarif').hide();
        }
    }

    static CreateTemporal(renewalModel) {
        RenewalRequest.CreateTemporal(renewalModel, glbPolicy.EndorsementController).done(function (data) {
            if (data.success) {
                if (data.result.Endorsement.IsUnderIdenticalConditions) {
                    Renewal.LoadEndorsementData(data.result);
                } else {
                    Renewal.LoadRiskView();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static CreateEndorsement() {
        RenewalRequest.CreateTemporal(renewalModel, glbPolicy.EndorsementController).done(function (data) {
            if (data.success) {
                if (data.result.Endorsement.IsUnderIdenticalConditions) {
                    Renewal.LoadEndorsementData(data.result);
                    Renewal.CreateEndorsementIdenticalConditions();
                } else {
                    Renewal.LoadRiskView();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInformationEndorsement, 'autoclose': true });
        });
    }

    static CreateEndorsementIdenticalConditions() {
        RenewalRequest.CreateEndorsement(glbPolicy.Id, glbPolicy.DocumentNumber, glbPolicy.EndorsementController).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (Array.isArray(data.result) && data.result.length > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result, glbPolicy.Id, FunctionType.Individual);
                    } else {
                        Search.SetNewEndorsement(data.result, glbPolicy.Endorsement.Id);
                        $.UifDialog('confirm',
                            { 'message': data.result + '\n ' + Resources.Language.MessagePrint },
                            function (result) {
                                if (result) {
                                    var message = data.result.split(':');
                                    var endorsementNumber = parseFloat(message[2]);
                                    PrinterRequest.PrintReportFromOutside(glbPolicy.Branch.Id,
                                        glbPolicy.Prefix.Id,
                                        glbPolicy.DocumentNumber, parseFloat(message[3])).done(function (data) {
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
                                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                                        });
                                }
                                Renewal.RedirectSearchController();
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
    }

    static LoadCurrentSummary() {
        renewalCurrentSummary.AmountInsured = FormatMoney(renewalCurrentSummary.AmountInsured);
        renewalCurrentSummary.Premium = FormatMoney(renewalCurrentSummary.Premium);
        renewalCurrentSummary.Expenses = FormatMoney(renewalCurrentSummary.Expenses);
        renewalCurrentSummary.Taxes = FormatMoney(renewalCurrentSummary.Taxes);
        renewalCurrentSummary.FullPremium = FormatMoney(renewalCurrentSummary.FullPremium);
        $('#tableCurrentSummary').UifDataTable('clear');
        $('#tableCurrentSummary').UifDataTable('addRow', renewalCurrentSummary);
    }

    static LoadRiskView() {
        router.run("prtTemporal");
    }

    static ShowPanels(isVisible) {
        if (isVisible) {
            $("#panelRenewalIdenticalCondition").show();
            $("#buttonsIdenticalCondition").show();
            $("#panelOptionRenewal").hide();
            $("#buttonTemporalOption").hide();
        }
        else {
            $("#panelRenewalIdenticalCondition").hide();
            $("#buttonsIdenticalCondition").hide();
            $("#panelOptionRenewal").show();
            $("#buttonTemporalOption").show();
        }
    }

    static RedirectCollective(Id) {
        var collectiveModel = {
            IdLoad: Id,
            IdPrefixCommercial: glbPolicy.Prefix.Id,
            IdBranch: glbPolicy.Branch.Id,
            EndorsementId: glbPolicy.Endorsement.Id,
            PolicyId: glbPolicy.DocumentNumber,
            CreateRiskFromPolicy: true
        };

        $.redirect(rootPath + 'Collective/Collective/Collective', collectiveModel);
    }
    static LoadData() {
        $('#inputCurrentFrom').val(FormatDate(glbPolicy.Endorsement.CurrentTo));
        $('#inputCurrentTo').val(FormatDate(glbPolicy.Endorsement.CurrentTo));
    }

    static LoadRecordObservation() {
        if (glbPolicy.EndorsementTexts != undefined && glbPolicy.EndorsementTexts.length > 0) {
            var recordText = "";
            glbPolicy.EndorsementTexts.forEach(function (item) {
                recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
            });
            $('#inputRecord').val(recordText);
        }
    }

    static GetExchangeRateByCurrencyId(currencyId) {
        RenewalRequest.GetExchangeRateByCurrencyId(currencyId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    //$("#inputChange").val(FormatMoney(data.result.SellAmount));
                    if (FormatDate(data.result.RateDate) != FormatDate($("#inputModifyFrom").val())) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.OutdatedExchangeRate + ' ' + FormatDate(data.result.RateDate), 'autoclose': true });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.OutdatedExchangeRateExist, 'autoclose': true });
        });
    }

    static PrintTemporal() {
        RenewalRequest.GenerateReportTemporary(glbPolicyEndorsement.Endorsement.TemporalId, glbPolicy.Prefix.Id, glbPolicy.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }
}

