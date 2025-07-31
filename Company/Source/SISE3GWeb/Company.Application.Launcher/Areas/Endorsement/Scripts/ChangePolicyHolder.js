var glbChangePolicyHolderAlert = false;
var glbChangePolicyHolder = {};
var glbAddChangePolicyHolder = [];
var dataRisk;
var tempChangePolicyHolder = {};
var IndividualId = 0;
var IsConsortium = false;
var temporaryHolder = null;
class ChangePolicyHolder extends Uif2.Page {
    getInitialState() {
        $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
        if (glbPolicy.Endorsement.Id > 0) {
            ChangePolicyHolder.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
        }
        $("#listChangePolicyHolderCurrent").UifListView({
            source: null,
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: false,
            edit: false,
            delete: false,
            displayTemplate: "#ChangePolicyHolderTemplate",
            title: Resources.Language.LabelReasons,
            height: 200
        });
        $('#listChangePolicyHolderModify').UifListView({
            source: null,
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: false,
            edit: false,
            delete: false,
            displayTemplate: "#ChangePolicyHolderTemplate",
            title: Resources.Language.LabelReasons,
            height: 200
        });
        $('#listSaveChangePolicyHolder').UifListView({
            source: null,
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: false,
            edit: false,
            delete: false,
            displayTemplate: "#ChangePolicyHolderTemplate",
            title: Resources.Language.LabelReasons,
            height: 200
        });
        if (glbPolicy.Id > 0) {
            $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
            ChangePolicyHolder.GetTemporalById(glbPolicy.EndorsementController, glbPolicy.Id);
        }
        $('#inputIssueDate').text(FormatFullDate(glbPolicy.IssueDate));
        if (glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO || glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
            $.UifDialog('confirm', { 'message': Resources.Language.ChangePolicyHolderConsolidation }, function (result) {
                if (result) {
                    glbChangePolicyHolderAlert = true;
                }
                else {
                    glbChangePolicyHolderAlert = false;
                }
            });
        }

        ChangePolicyHolder.LoadPolicyHolderData(glbPolicy.Holder, "#listChangePolicyHolderCurrent");
        $("#btnPrintTemporalHolder").hide();
        $('#inputRecord').prop('disabled', true);
        this.ValidatePolicies();
    }

    bindEvents() {
        $("#inputCurrentFrom").on("datepicker.change", function (event, date) {
            ChangePolicyHolder.ChangeAgentDate($("#inputCurrentFrom").val(), glbPolicy.Endorsement.CurrentTo);
        });

        $('#btnChangePolicyHolderEndorsement').on('click', ChangePolicyHolder.ShowSaveChangePolicyHolder);

        $('#listSaveChangePolicyHolder').on('rowDelete', function (event, data) {
            ChangePolicyHolder.DeleteChangePolicyHolder(data);
        });

        $("#inputChangePolicyHolderName").on("buttonClick", function () {
            ChangePolicyHolder.SearchPersonOrCompany();
        });

        $('#btnChangePolicyHolderAccept').on('click', ChangePolicyHolder.AddNewChangePolicyHolder);

        $('#btnChangePolicyHolderSave').on('click', ChangePolicyHolder.CreateTemporal);

        $('#btnSave').on('click', ChangePolicyHolder.CreateEndorsementChangePolicyHolder);

        $('#btnChangePolicyHolderCancel').on('click', ChangePolicyHolder.CancelChangePolicyHolder);

        $('#btnCancel').on('click', ChangePolicyHolder.RedirectSearchController);

        $('#tableIndividualResults tbody').on('click', 'tr', ChangePolicyHolder.SelectIndividualResults);

        $("#btnPrintTemporalHolder").click(ChangePolicyHolder.PrintTemporal);
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

    static LoadCurrentSummaryEndorsement(policyData) {
        if (policyData.Summary != null) {
            $('#labelCurrentRisk').text(policyData.Summary.RiskCount);
            $('#labelCurrentSum').text(policyData.Summary.AmountInsured);
            $('#labelCurrentPremium').text(policyData.Summary.Premium);
            $('#labelCurrentExpenses').text(policyData.Summary.Expenses);
            $('#labelCurrentTaxes').text(policyData.Summary.Taxes);
            $('#labelCurrentTotalPremium').text(policyData.Summary.FullPremium);
            if (policyData.EndorsementTexts != undefined && policyData.EndorsementTexts.length > 0) {
                var recordText = "";
                policyData.EndorsementTexts.forEach(function (item) {
                    recordText = recordText + FormatDate(item.Date) + "-" + Resources.Language.Endorsement + " " + item.Number + "-" + item.Type + "\n" + item.Observations + "\n\r";
                });
                $('#inputRecord').val(recordText);
            }
        }
    }

    static ChangeAgentDate(modificationDate, currentTo) {
        if (CompareBetweenDates(modificationDate, FormatDate(glbPolicyEndorsement.CurrentFrom), FormatDate(glbPolicy.Endorsement.CurrentTo))) {
            $('#inputDays').val(CalculateDays(modificationDate, currentTo));
        } else {
            $('#inputCurrentFrom').val("");
            $('#inputDays').val("");
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.LabelDateToDatePolicy, 'autoclose': true });
        }
    }

    static LoadPolicyHolderData(PolicyHolder, nameListView) {

        if (PolicyHolder != null) {

            if (PolicyHolder.Name != null) {
                tempChangePolicyHolder.PolicyHolderName = (PolicyHolder.Name);
            }
            if (PolicyHolder.CompanyName.Address != null) {
                tempChangePolicyHolder.PolicyHolderAddress = PolicyHolder.CompanyName.Address.Description;
            }
            if (PolicyHolder.CompanyName.Phone != null) {
                tempChangePolicyHolder.PolicyHolderPhone = PolicyHolder.CompanyName.Phone.Description;
            }
            if (PolicyHolder.CompanyName.Email != null) {
                tempChangePolicyHolder.PolicyHolderEmail = PolicyHolder.CompanyName.Email.Description;
            }
            if (PolicyHolder.IndividualId > 0) {
                tempChangePolicyHolder.IndividualId = PolicyHolder.IndividualId;
            }
            if (nameListView == "#listChangePolicyHolderModify") {
                $('#listChangePolicyHolderModify').UifListView("addItem", tempChangePolicyHolder);
                $('#listSaveChangePolicyHolder').UifListView({ displayTemplate: '#ChangePolicyHolderTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: tempChangePolicyHolder });
            }
            else {
                $("#listChangePolicyHolderCurrent").UifListView("refresh");
                $("#listChangePolicyHolderCurrent").UifListView('addItem', tempChangePolicyHolder);
            }
        }
    }


    static ShowSaveChangePolicyHolder() {
        $('#formChangePolicyHolder').validate();
        if ($('#formChangePolicyHolder').valid()) {
            let getModifyData = $("#listChangePolicyHolderModify").UifListView("getData");
            if (getModifyData.length > 0) {
                let currentChangePolicyHolderCurrent = jQuery.extend(true, [], $("#listChangePolicyHolderCurrent").UifListView("getData"));
                $("#modalSaveChangePolicyHolder").UifModal('showLocal', Resources.Language.LabelDataChangePolicyHolder);
                $('#listSaveChangePolicyHolder').UifListView({ displayTemplate: '#ChangePolicyHolderTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: getModifyData });
            }
            else {
                let currentChangePolicyHolderCurrent = jQuery.extend(true, [], $("#listChangePolicyHolderCurrent").UifListView("getData"));
                $("#modalSaveChangePolicyHolder").UifModal('showLocal', Resources.Language.LabelDataPolicyHolder);
                let tempSaveChangePolicyHolder = {};
                let resultSaveChangePolicyHolder = []
                tempSaveChangePolicyHolder.PolicyHolderName = currentChangePolicyHolderCurrent[0].PolicyHolderName;
                tempSaveChangePolicyHolder.PolicyHolderAddress = currentChangePolicyHolderCurrent[0].PolicyHolderAddress;
                tempSaveChangePolicyHolder.PolicyHolderPhone = currentChangePolicyHolderCurrent[0].PolicyHolderPhone;
                tempSaveChangePolicyHolder.PolicyHolderEmail = currentChangePolicyHolderCurrent[0].PolicyHolderEmail;
                resultSaveChangePolicyHolder.push(tempSaveChangePolicyHolder);
                $('#listSaveChangePolicyHolder').UifListView({ displayTemplate: '#ChangePolicyHolderTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: resultSaveChangePolicyHolder });
            }
        }
    }

    static DeleteChangePolicyHolder() {
        let dataDelete = []
        $('#listSaveChangePolicyHolder').UifListView({ displayTemplate: '#SavePolicyHolderTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: dataDelete });
    }

    static SearchPersonOrCompany() {
        if ($("#inputChangePolicyHolderName").val().trim().length > 0) {
            lockScreen();
            var dataList = [];
            ChangePolicyHolderRequest.GetHolders($("#inputChangePolicyHolderName").val(), InsuredSearchType.DocumentNumber, CustomerType.Individual, TemporalType.Endorsement).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
                        return;
                    }
                    if (data.result.length == 1) {
                        $('#inputChangePolicyHolderName').data('Object', data.result[0]);
                        $('#inputChangePolicyHolderName').val(data.result[0].Name);
                        if (data.result[0].CompanyName.Address != null || data.result[0].CompanyName.Address != undefined) {
                            $('#inputChangePolicyHolderAddress').val(data.result[0].CompanyName.Address.Description);
                        }

                        if (data.result[0].CompanyName.Phone != null || data.result[0].CompanyName.Phone != undefined) {
                            $('#inputChangePolicyHolderPhone').val(data.result[0].CompanyName.Phone.Description);
                        }
                        if (data.result[0].CompanyName.Email != null || data.result[0].CompanyName.Email != undefined) {
                            $('#inputChangePolicyHolderEmail').val(data.result[0].CompanyName.Email.Description);
                        }
                        glbChangePolicyHolder = data.result[0].CompanyName;
                        glbChangePolicyHolder.Id = data.result[0].IndividualId;
                        glbChangePolicyHolder.TradeName = data.result[0].Name;
                        glbPolicy.holder = data.result;
                        if (glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO || glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
                            ChangePolicyHolder.GetInfoAfianzado(data.result[0].IndividualId);
                        }
                    }
                    else if (data.result.length > 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].InsuredId,
                                DocumentNum: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                            });
                        }
                        ChangePolicyHolder.ShowIndividualResults(dataList);
                        $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelSelectHolder);
                    }
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorHolderNotFound, 'autoclose': true });
                }

            });
        }
    }

    static AddNewChangePolicyHolder() {
        let currentSaveConsorlidationChangeData = $('#listSaveChangePolicyHolder').UifListView('getData');
        if (currentSaveConsorlidationChangeData.length <= 0) {
            if ($("#inputChangePolicyHolderName").val() != "" && $("#inputChangePolicyHolderAddress").val() != ""
                && $("#inputChangePolicyHolderPhone").val != null && $("#inputChangePolicyHolderEmail").val() != "") {
                let resultSaveChangePolicyHolder = [];
                tempChangePolicyHolder.PolicyHolderName = $('#inputChangePolicyHolderName').val();
                tempChangePolicyHolder.PolicyHolderAddress = $('#inputChangePolicyHolderAddress').val();
                tempChangePolicyHolder.PolicyHolderPhone = $('#inputChangePolicyHolderPhone').val();
                tempChangePolicyHolder.PolicyHolderEmail = $('#inputChangePolicyHolderEmail').val();
                tempChangePolicyHolder.IndividualId = glbChangePolicyHolder.Id
                resultSaveChangePolicyHolder.push(tempChangePolicyHolder);
                glbAddChangePolicyHolder = resultSaveChangePolicyHolder;
                $('#listSaveChangePolicyHolder').UifListView({ displayTemplate: '#ChangePolicyHolderTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: resultSaveChangePolicyHolder });
                ChangePolicyHolder.CancelChangePolicyHolder();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': 'Tomador con datos incompletos.', 'autoclose': true });
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Solo puede agregar un Tomador', 'autoclose': true });
        }
    }

    static CreateTemporal() {
        ChangePolicyHolder.GetChangePolicyHolder('#listSaveChangePolicyHolder');
        if (glbChangePolicyHolder != undefined && glbChangePolicyHolder != null) {
            var ChangePolicyHolderModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangePolicyHolder);
            ChangePolicyHolderModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
            ChangePolicyHolderModel.CurrentTo = FormatDate(glbPolicyEndorsement.Endorsement.CurrentTo);
            ChangePolicyHolderModel.EndorsementFrom = FormatDate($("#inputCurrentFrom").val());
            ChangePolicyHolderModel.ChangePolicyHolderFrom = FormatDate($("#inputCurrentFrom").val());
            ChangePolicyHolderModel.IsPrincipal = false;
            ChangePolicyHolderModel.Id = glbPolicy.Id;
            if (Array.isArray(glbPolicy.holder)) {
                ChangePolicyHolderModel.holder = glbPolicy.holder[0];
            } else {
                ChangePolicyHolderModel.holder = glbPolicy.holder;
            }

            if (glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO || glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
                ChangePolicyHolderModel.companyContract = {};
                ChangePolicyHolderModel.companyContract.IndividualId = IndividualId;
                ChangePolicyHolderModel.companyContract.Name = glbChangePolicyHolder.TradeName;
                ChangePolicyHolderModel.companyContract.CompanyName = glbChangePolicyHolder;
            }
            ChangePolicyHolderRequest.CreateTemporal(glbPolicy.EndorsementController, ChangePolicyHolderModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies);
                        }
                        glbPolicy.Id = data.result.Id;
                        ChangePolicyHolder.LoadSummaryTemporal(data.result);
                        ChangePolicyHolder.CancelChangePolicyHolder();
                        $("#btnPrintTemporalHolder").show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSaveChangeAgent, 'autoclose': true });
                        $("#btnPrintTemporalHolder").hide();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    $("#btnPrintTemporalHolder").hide();
                }
            });
        }
        else {
            $('#modalSaveChangePolicyHolder').modal('toggle');
        }
    }

    static GetChangePolicyHolder(namelistChangePolicyHolder) {
        var tablePolicyHolder = $(namelistChangePolicyHolder).UifListView('getData');
        IndividualId = tablePolicyHolder[0].IndividualId;
    }

    static LoadSummaryTemporal(temporalData) {
        glbPolicy.Id = temporalData.Id;
        temporaryHolder = temporalData;
        $("#modalSaveChangePolicyHolder").UifModal('hide');
        $('#listChangePolicyHolderModify').UifListView({ displayTemplate: '#ChangePolicyHolderTemplate', add: false, customDelete: true, height: 250, sourceData: glbAddChangePolicyHolder });
        $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
    }

    static CreateEndorsementChangePolicyHolder() {
        lockScreen();
        ChangePolicyHolder.GetChangePolicyHolder('#listChangePolicyHolderModify');
        var ChangePolicyHolderModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangePolicyHolder);
        ChangePolicyHolderModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
        ChangePolicyHolderModel.CurrentTo = FormatDate(glbPolicy.CurrentTo);
        ChangePolicyHolderModel.ChangePolicyHolderFrom = $("#inputCurrentFrom").val();
        ChangePolicyHolderModel.IsPrincipal = false;
        ChangePolicyHolderModel.Id = glbPolicy.Id;
        ChangePolicyHolderModel.holder = glbPolicy.holder[0];
        if (glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO || glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
            ChangePolicyHolderModel.companyContract = {};
            ChangePolicyHolderModel.companyContract.IndividualId = IndividualId;
            ChangePolicyHolderModel.companyContract.Name = glbChangePolicyHolder.TradeName;
            ChangePolicyHolderModel.companyContract.CompanyName = glbChangePolicyHolder;
        }
        ChangePolicyHolderRequest.CreateEndorsementChangePolicyHolder(glbPolicy.EndorsementController, ChangePolicyHolderModel).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    if (Array.isArray(data.result[0].InfringementPolicies) && data.result[0].InfringementPolicies.length > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result[0].InfringementPolicies, glbPolicy.Id, FunctionType.Individual);
                    }
                    else {
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
            unlockScreen();
        });
    }

    static CancelChangePolicyHolder() {
        $('#inputChangePolicyHolderName').val(null);
        $('#inputChangePolicyHolderAddress').val(null);
        $('#inputChangePolicyHolderPhone').val(null);
        $('#inputChangePolicyHolderEmail').val(null);
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static GetInfoAfianzado(individualId) {

        var opertionQuota = ChangePolicyHolder.GetInformationChangePolicyHolder(individualId);

        opertionQuota.done(function (r1) {
            ChangePolicyHolder.ValdateInfo();
        });
    }

    static GetInformationChangePolicyHolder(InsuredId) {
        var dfd = $.Deferred();
        var IndividualId = InsuredId;
        var LinebusinessId = glbPolicy.Prefix.Id;
        var SellAmount = glbPolicy.ExchangeRate.SellAmount;
        var array = new Array();
        EconomicGroupEvent = ConsortiumEvent = OperationQuotaEvent = null;

        array.push(
            new Promise((resolve, reject) => {
                RiskSuretyRequest.IsConsortiumindividualId(IndividualId).done(function (data) {
                    if (data.result) {
                        IsConsortium = true;
                    } else {
                        OperationQuotaCumulusRequest.GetOperatingQuotaEventByIndividualIdByLineBusinessId(IndividualId, LinebusinessId)
                            .done((OperationQuota) => {
                                if (OperationQuota.success) {
                                    if (OperationQuota.result) {
                                        if (OperationQuota.result.IdentificationId != 0) {
                                            OperationQuotaEvent = {
                                                Id: OperationQuota.result.OperatingQuotaEventID,
                                                Belongingto: '1.' + glbPolicy.Holder.Name,
                                                OperationCuotaInitial: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT,
                                                OperationCuotaAvalible: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT - OperationQuota.result.ApplyEndorsement.AmountCoverage,
                                                DateTo: FormatDate(OperationQuota.result.IndividualOperatingQuota.EndDateOpQuota),
                                                DateRegisty: FormatDate(OperationQuota.result.IssueDate),
                                                Cumulu: OperationQuota.result.ApplyEndorsement.AmountCoverage
                                            }
                                        }
                                    }
                                }
                                resolve(OperationQuota);
                            });
                    }

                }).fail(xhr => {
                    reject(xhr);
                })
            }));
		var endoso = false;

//		if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification || glbPolicy.Endorsement.EndorsementType == EndorsementType.Cancellation || glbPolicy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension) { endoso = true; }

        array.push(
            new Promise((resolve, reject) => {
				OperationQuotaCumulusRequest.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(IndividualId, LinebusinessId, endoso, glbPolicy.Endorsement.Id)
                    .done((Consortium) => {
                        if (Consortium.success) {
                            if (Consortium.result) {
                                if (Consortium.result.consortiumEventDTO != null) {

                                    ConsortiumEvent = {
                                        Id: Consortium.result.OperatingQuotaEventID,
                                        Belongingto: '2.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName,
                                        OperationQuotaConsortium: Consortium.result.consortiumEventDTO.OperationQuotaConsortium,
                                        OperationCuotaInitial: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT,
                                        OperationCuotaAvalible: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT - Consortium.result.ApplyEndorsement.AmountCoverage / SellAmount,
                                        DateTo: FormatDate(Consortium.result.IndividualOperatingQuota.EndDateOpQuota),
                                        DateRegisty: FormatDate(Consortium.result.IssueDate),
                                        Cumulu: Consortium.result.ApplyEndorsement.AmountCoverage
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
                OperationQuotaCumulusRequest.GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(IndividualId, LinebusinessId)
                    .done(function (EconomicGroup) {
                        if (EconomicGroup.success) {
                            if (EconomicGroup.result.EconomicGroupEventDTO != null) {
                                EconomicGroupEvent = {
                                    Id: EconomicGroup.result.OperatingQuotaEventID,
                                    Belongingto: '3.' + EconomicGroup.result.EconomicGroupEventDTO.economicgroupoperatingquotaDTO.EconomicGroupName,
                                    OperationCuotaInitial: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT,
                                    OperationCuotaAvalible: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT - EconomicGroup.result.ApplyEndorsement.AmountCoverage,
                                    DateTo: FormatDate(EconomicGroup.result.IndividualOperatingQuota.EndDateOpQuota),
                                    DateRegisty: FormatDate(EconomicGroup.result.IssueDate),
                                    Cumulu: EconomicGroup.result.ApplyEndorsement.AmountCoverage
                                }
                            }
                        }
                        resolve(EconomicGroup);
                    }).fail(xhr => {
                        reject(xhr);
                    })
            }));

        Promise.all(array).then(() => {
            dfd.resolve(array);
        }).catch(reason => {
            dfd.reject();
            console.log(reason);
        });
        return dfd.promise();
    }

    static ValdateInfo() {
        if (EconomicGroupEvent != null || ConsortiumEvent != null || OperationQuotaEvent != null) {
            if (EconomicGroupEvent != null) {
                //Grupo Economico
                if (EconomicGroupEvent.OperationCuotaAvalible < parseInt(NotFormatMoney(glbPolicy.Summary.AmountInsured))) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                }
            } else if (ConsortiumEvent != null) {
                //Consorcio
                if (ConsortiumEvent.OperationCuotaAvalible < parseInt(NotFormatMoney(glbPolicy.Summary.AmountInsured))) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                }
            } else if (OperationQuotaEvent != null) {
                // Individual
                if (OperationQuotaEvent.OperationCuotaAvalible < parseInt(NotFormatMoney(glbPolicy.Summary.AmountInsured))) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                ChangePolicyHolder.CancelChangePolicyHolder();
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorAvaliablePolicyHolderOperationQuota, 'autoclose': true });
            ChangePolicyHolder.CancelChangePolicyHolder();
        }
    }

    static GetTemporalById(endorsementController, temporalId) {

        ChangePolicyHolderRequest.GetTemporalById(endorsementController, temporalId).done(function (data) {
            if (data.success) {
                ChangePolicyHolderRequest.GetRiskTemporalById(endorsementController, temporalId).done(function (dataRisk) {
                    if (dataRisk.success) {
                        ChangePolicyHolder.LoadEndorsementData(data.result);
                        $('#inputDays').val(CalculateDays($('#inputCurrentFrom').val(), glbPolicy.CurrentTo));
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchTemp, 'autoclose': true });
        });

    }

    static LoadRiskByRisk(event, data) {
        glbRisk.forEach(function (item, index) {
            if (item.Id == data.Id) {
                $("#inputTextRisk").val(item.Risk.Text.TextBody);
                $("#selectRisk").UifSelect("setSelected", item.Id);
                $("#selectRisk").UifSelect("disabled", false);
                ChangePolicyHolder.LoadPolicyHolderData(item.Contractor);
            }
        });
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
        ChangePolicyHolder.LoadPolicyHolderData(policy.holder, "#listChangePolicyHolderModify");
    }

    static ShowIndividualResults(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static SelectIndividualResults(data) {
        var individualId = $(this).children()[0].innerHTML;
        var customerType = $(this).children()[1].innerHTML;
        ChangePolicyHolder.GetHolderByIndividualId(individualId, customerType);
    }

    static GetHolderByIndividualId(individualId, customerType) {
        HoldersRequest.GetHoldersByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                $('#inputChangePolicyHolderName').data('Object', data.result.holder);
                $('#inputChangePolicyHolderName').val(data.result.holder.Name);
                if (data.result.holder.CompanyName.Address != null || data.result.holder.CompanyName.Address != undefined) {
                    $('#inputChangePolicyHolderAddress').val(data.result.holder.CompanyName.Address.Description);
                }

                if (data.result.holder.CompanyName.Phone != null || data.result.holder.CompanyName.Phone != undefined) {
                    $('#inputChangePolicyHolderPhone').val(data.result.holder.CompanyName.Phone.Description);
                }
                if (data.result.holder.CompanyName.Email != null || data.result.holder.CompanyName.Email != undefined) {
                    $('#inputChangePolicyHolderEmail').val(data.result.holder.CompanyName.Email.Description);
                }
                glbChangePolicyHolder = data.result.holder.CompanyName;
                glbChangePolicyHolder.Id = data.result.holder.IndividualId;
                glbChangePolicyHolder.TradeName = data.result.holder.Name;
                glbPolicy.holder = data.result.holder;
                if (glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.CUMPLIMIENTO || glbChangePolicyHolderAlert && glbPolicy.Prefix.Id == PrefixType.ARRENDAMIENTO) {
                    ChangePolicyHolder.GetInfoAfianzado(data.result.holder.IndividualId);
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
            }
        });

        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    static PrintTemporal() {
        ChangePolicyHolderRequest.GenerateReportTemporary(temporaryHolder.Endorsement.TemporalId, glbPolicy.Prefix.Id, temporaryHolder.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }
}