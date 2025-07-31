var glbChangeConsolidation = {};
var glbAddChangeConsolidation = [];
var dataRisk;
var tempChangeConsolidation = {};
var IndividualId = 0;
var temporaryConsolidation = null;
class ChangeConsolidation extends Uif2.Page {
    getInitialState() {
        ChangeConsolidation.GetRiskByEndorsementByPolicy();
        $.uif2.helpers.setGlobalTitle(glbPolicyEndorsement.Title);
        if (glbPolicy.Endorsement.Id > 0) {
            ChangeConsolidation.LoadCurrentSummaryEndorsement(glbPolicyEndorsement);
        }
        $("#listChangeConsolidationCurrent").UifListView({
            source: null,
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: false,
            edit: false,
            delete: false,
            displayTemplate: "#ChangeConsolidationTemplate",
            title: Resources.Language.LabelReasons,
            height: 200
        });
        $('#listChangeConsolidationModify').UifListView({
            source: null,
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: false,
            edit: false,
            delete: false,
            displayTemplate: "#ChangeConsolidationTemplate",
            title: Resources.Language.LabelReasons,
            height: 200
        });
        $('#listSaveChangeConsolidation').UifListView({
            source: null,
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: false,
            edit: false,
            delete: false,
            displayTemplate: "#ChangeConsolidationTemplate",
            title: Resources.Language.LabelReasons,
            height: 200
        });
        if (glbPolicy.Id > 0) {
            $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
            ChangeConsolidation.GetTemporalById(glbPolicy.EndorsementController, glbPolicy.Id);
        }
        $('#inputIssueDate').text(FormatFullDate(glbPolicy.IssueDate));
        $('#btnPrintTemporalConsolidation').hide();
        $('#inputRecord').prop('disabled', true);
        this.ValidatePolicies();
    }

    bindEvents() {
        $("#inputCurrentFrom").on("datepicker.change", function (event, date) {
            ChangeConsolidation.ChangeAgentDate($("#inputCurrentFrom").val(), glbPolicy.CurrentTo);
        });

        $('#btnChangeConsolidationEndorsement').on('click', ChangeConsolidation.ShowSaveChangeConsolidation);

        $('#listSaveChangeConsolidation').on('rowDelete', function (event, data) {
            ChangeConsolidation.DeleteChangeConsolidation(data);
        });

        $("#inputChangeConsolidationName").on("buttonClick", function () {
            ChangeConsolidation.SearchPersonOrCompany();
        });

        $('#btnChangeConsolidationAccept').on('click', ChangeConsolidation.AddNewChangeConsolidation);

        $('#btnChangeConsolidationSave').on('click', ChangeConsolidation.CreateTemporal);

        $('#btnSave').on('click', ChangeConsolidation.CreateEndorsementChangeConsolidation);

        $('#btnChangeConsolidationCancel').on('click', ChangeConsolidation.CancelConsolidationChange);

        $('#btnCancel').on('click', ChangeConsolidation.RedirectSearchController);

        $('#selectRisk').on('itemSelected', function (event, item) {
            ChangeConsolidation.LoadRiskByRisk(event, item)
        });

        $('#tableIndividualResults tbody').on('click', 'tr', ChangeConsolidation.SelectIndividualResults);

        $("#btnPrintTemporalConsolidation").click(ChangeConsolidation.PrintTemporal);
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

    static GetRiskByEndorsementByPolicy() {
        ChangeConsolidationRequest.GetRiskByPolicyId(glbPolicy.Endorsement.PolicyId, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                if (data.result) {
                    glbRisk = data.result;
                    glbRisk.forEach(function (item, index) {
                        item.Description = item.Contractor.Name;
                        item.Id = item.Risk.RiskId;
                    });
                    $("#selectRisk").UifSelect({ sourceData: glbRisk });
                    if (data.result.length == 1) {
                        $("#inputTextRisk").val(glbRisk[0].Risk.Text.TextBody);
                        $("#selectRisk").UifSelect("setSelected", glbRisk[0].Id);
                        $("#selectRisk").UifSelect("disabled", true);
                        ChangeConsolidation.LoadConsolidationData(glbRisk[0].Contractor);
                    } else {
                        $("#selectRisk").UifSelect("disabled", false);
                    }

                }
            }
        });

    }

    static ChangeAgentDate(modificationDate, currentTo) {
        if (CompareBetweenDates(modificationDate, FormatDate(glbPolicy.CurrentFrom), FormatDate(glbPolicy.CurrentTo))) {
            $('#inputDays').val(CalculateDays(modificationDate, currentTo));
        } else {
            $('#inputCurrentFrom').val("");
            $('#inputDays').val("");
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.LabelDateToDatePolicy, 'autoclose': true });
        }
    }

    static LoadConsolidationData(Contractor, nameListView) {

        if (Contractor != null) {

            if (Contractor.Name != null) {
                tempChangeConsolidation.ConsolidationName = (Contractor.Name);
            }
            if (Contractor.CompanyName.Address != null) {
                tempChangeConsolidation.ConsolidationAddress = Contractor.CompanyName.Address.Description;
            }
            if (Contractor.CompanyName.Phone != null) {
                tempChangeConsolidation.ConsolidationPhone = Contractor.CompanyName.Phone.Description;
            }
            if (Contractor.CompanyName.Email != null) {
                tempChangeConsolidation.ConsolidationEmail = Contractor.CompanyName.Email.Description;
            }
            if (Contractor.IndividualId > 0) {
                tempChangeConsolidation.IndividualId = Contractor.IndividualId;
            }
            if (nameListView == "#listChangeConsolidationModify") {
                $('#listChangeConsolidationModify').UifListView("addItem", tempChangeConsolidation);
                $('#listSaveChangeConsolidation').UifListView({ displayTemplate: '#ChangeConsolidationTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: tempChangeConsolidation });
            }
            else {
                $("#listChangeConsolidationCurrent").UifListView("refresh");
                $("#listChangeConsolidationCurrent").UifListView('addItem', tempChangeConsolidation);
            }
        }
    }


    static ShowSaveChangeConsolidation() {
        $('#formChangeConsolidation').validate();
        if ($('#formChangeConsolidation').valid()) {
            ChangeConsolidation.CancelConsolidationChange();
            let getModifyData = $("#listChangeConsolidationModify").UifListView("getData");
            if (getModifyData.length > 0) {
                let currentChangeConsolidationCurrent = jQuery.extend(true, [], $("#listChangeConsolidationCurrent").UifListView("getData"));
                $("#modalSaveChangeConsolidation").UifModal('showLocal', Resources.Language.LabelDataChangeConsolidation);
                $('#listSaveChangeConsolidation').UifListView({ displayTemplate: '#ChangeConsolidationTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: getModifyData });
            }
            else {
                let currentChangeConsolidationCurrent = jQuery.extend(true, [], $("#listChangeConsolidationCurrent").UifListView("getData"));
                $("#modalSaveChangeConsolidation").UifModal('showLocal', Resources.Language.LabelDataChangeConsolidation);
                let tempSaveChangeConsolidation = {};
                let resultSaveChangeConsolidation = []
                tempSaveChangeConsolidation.ConsolidationName = currentChangeConsolidationCurrent[0].ConsolidationName;
                tempSaveChangeConsolidation.ConsolidationAddress = currentChangeConsolidationCurrent[0].ConsolidationAddress;
                tempSaveChangeConsolidation.ConsolidationPhone = currentChangeConsolidationCurrent[0].ConsolidationPhone;
                tempSaveChangeConsolidation.ConsolidationEmail = currentChangeConsolidationCurrent[0].ConsolidationEmail;
                resultSaveChangeConsolidation.push(tempSaveChangeConsolidation);
                $('#listSaveChangeConsolidation').UifListView({ displayTemplate: '#ChangeConsolidationTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: resultSaveChangeConsolidation });
            }
        }
    }

    static DeleteChangeConsolidation() {
        let dataDelete = []
        $('#listSaveChangeConsolidation').UifListView({ displayTemplate: '#SavePolicyHolderTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: dataDelete });
    }

    static SearchPersonOrCompany() {
        if ($("#inputChangeConsolidationName").val().trim().length > 0) {
            lockScreen();
            var dataList = [];
            ChangeConsolidationRequest.GetHolders($("#inputChangeConsolidationName").val(), InsuredSearchType.DocumentNumber, CustomerType.Individual, TemporalType.Endorsement).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
                        return;
                    }
                    if (data.result.length == 1) {
                        $('#inputChangeConsolidationName').data('Object', data.result[0]);
                        $('#inputChangeConsolidationName').val(data.result[0].Name);

                        if (data.result[0].CompanyName != null) {
                            if (data.result[0].CompanyName.Address != null || data.result[0].CompanyName.Address != undefined) {
                                $('#inputChangeConsolidationAddress').val(data.result[0].CompanyName.Address.Description);
                            }

                            if (data.result[0].CompanyName.Phone != null || data.result[0].CompanyName.Phone != undefined) {
                                $('#inputChangeConsolidationPhone').val(data.result[0].CompanyName.Phone.Description);
                            }

                            if (data.result[0].CompanyName.Email != null || data.result[0].CompanyName.Email != undefined) {
                                $('#inputChangeConsolidationEmail').val(data.result[0].CompanyName.Email.Description);
                            }
                            glbChangeConsolidation = data.result[0].CompanyName;
                            glbChangeConsolidation.Id = data.result[0].IndividualId;
                            glbChangeConsolidation.TradeName = data.result[0].Name;
                            ChangeConsolidation.GetInfoAfianzado(data.result[0].IndividualId);
                        } else {
                            $.UifNotify('show', { 'type': 'info', 'message': 'La Informacion del Afianzado no esta Completa', 'autoclose': true });
                            ChangeConsolidation.CancelConsolidationChange();
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

                        ChangeConsolidation.ShowIndividualResults(dataList);
                        $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    ChangeConsolidation.CancelConsolidationChange();
                }
            });
        }
    }

    static AddNewChangeConsolidation() {
        let currentSaveConsorlidationChangeData = $('#listSaveChangeConsolidation').UifListView('getData');
        if (currentSaveConsorlidationChangeData.length <= 0) {
            let resultSaveChangeConsolidation = [];
            tempChangeConsolidation.ConsolidationName = $('#inputChangeConsolidationName').val();
            tempChangeConsolidation.ConsolidationAddress = $('#inputChangeConsolidationAddress').val();
            tempChangeConsolidation.ConsolidationPhone = $('#inputChangeConsolidationPhone').val();
            tempChangeConsolidation.ConsolidationEmail = $('#inputChangeConsolidationEmail').val();
            tempChangeConsolidation.IndividualId = glbChangeConsolidation.Id
            resultSaveChangeConsolidation.push(tempChangeConsolidation);
            glbAddChangeConsolidation = resultSaveChangeConsolidation;
            $('#listSaveChangeConsolidation').UifListView({ displayTemplate: '#ChangeConsolidationTemplate', add: false, delete: true, customDelete: true, height: 250, sourceData: resultSaveChangeConsolidation });
            ChangeConsolidation.CancelConsolidationChange();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Solo puede agregar un afianzado', 'autoclose': true });
        }

    }

    static CreateTemporal() {
        if (glbChangeConsolidation != undefined && glbChangeConsolidation != null) {
            var changeConsolidationModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeConsolidation);
            ChangeConsolidation.GetChangeConsolidation('#listSaveChangeConsolidation');
            changeConsolidationModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
            changeConsolidationModel.EndorsementFrom = FormatDate($("#inputCurrentFrom").val());
            changeConsolidationModel.CurrentTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
            changeConsolidationModel.ChangeConsolidationFrom = $("#inputCurrentFrom").val();
            changeConsolidationModel.IsPrincipal = false;
            changeConsolidationModel.Id = glbPolicy.Id;
			changeConsolidationModel.companyContract = dataRisk; /*Validar porque el policyId llega en 0 y por eso no llena el datarisk*/
			changeConsolidationModel.companyContract.IndividualId = IndividualId;
            changeConsolidationModel.companyContract.Name = glbChangeConsolidation.TradeName;
            changeConsolidationModel.companyContract.CompanyName = glbChangeConsolidation;
            ChangeConsolidationRequest.CreateTemporal(glbPolicy.EndorsementController, changeConsolidationModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.InfringementPolicies != null && data.result.InfringementPolicies.length > 0) {
                            LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies);
                        }
                        glbPolicy.Id = data.result.Id;
                        ChangeConsolidation.LoadSummaryTemporal(data.result);
                        ChangeConsolidation.CancelConsolidationChange();
                        $('#btnPrintTemporalConsolidation').show();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSaveChangeAgent, 'autoclose': true });
                        $('#btnPrintTemporalConsolidation').hide();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    $('#btnPrintTemporalConsolidation').hide();
                }
            });
        }
        else {
            $('#modalSaveChangeConsolidation').modal('toggle');
        }
    }

    static GetChangeConsolidation(namelistChangeConsolidation) {
        var tableChangeConsolidation = $(namelistChangeConsolidation).UifListView('getData');
        IndividualId = tableChangeConsolidation[0].IndividualId;
    }

    static LoadSummaryTemporal(temporalData) {
        glbPolicy.Id = temporalData.Id;
        temporaryConsolidation = temporalData;
        $("#modalSaveChangeConsolidation").UifModal('hide');
        $('#listChangeConsolidationModify').UifListView({ displayTemplate: '#ChangeConsolidationTemplate', add: false, customDelete: true, height: 250, sourceData: glbAddChangeConsolidation });
        $('#labelTemporalId').prop('innerText', ' ' + glbPolicy.Id);
    }

    static CreateEndorsementChangeConsolidation() {
        ChangeConsolidation.GetChangeConsolidation('#listChangeConsolidationModify');
        var changeConsolidationModel = SharedEndorsementBase.SetPolicyBase(FormsEndorsement.formChangeConsolidation);
        changeConsolidationModel.CurrentFrom = FormatDate(glbPolicy.CurrentFrom);
        changeConsolidationModel.CurrentTo = FormatDate(glbPolicy.Endorsement.CurrentTo);
        changeConsolidationModel.ChangeConsolidationFrom = $("#inputCurrentFrom").val();
        changeConsolidationModel.IsPrincipal = false;
        changeConsolidationModel.Id = glbPolicy.Id;
        if (dataRisk == undefined) {
            changeConsolidationModel.companyContract = {};
            changeConsolidationModel.companyContract.Contractor = {};
        } else {
            changeConsolidationModel.companyContract = dataRisk;
        }
        changeConsolidationModel.companyContract.Contractor.IndividualId = IndividualId;
        changeConsolidationModel.companyContract.Contractor.Name = glbChangeConsolidation.TradeName;
        changeConsolidationModel.companyContract.Contractor.CompanyName = glbChangeConsolidation;
        ChangeConsolidationRequest.CreateEndorsementChangeConsolidation(glbPolicy.EndorsementController, changeConsolidationModel).done(function (data) {
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
        });
    }

    static CancelConsolidationChange() {
        $('#inputChangeConsolidationName').val(null);
        $('#inputChangeConsolidationAddress').val(null);
        $('#inputChangeConsolidationPhone').val(null);
        $('#inputChangeConsolidationEmail').val(null);
    }

    static RedirectSearchController() {
        Search.DeleteEndorsementControl();
        Search.LoadSiteEndorsement(EndorsementMovements.Search);
    }

    static GetInfoAfianzado(individualId) {

        var opertionQuota = ChangeConsolidation.GetInformationChangeConsolidation(individualId);

        opertionQuota.done(function (r1) {
            Promise.all(r1).then(() => { 
                ChangeConsolidation.ValdateInfo();
            });
        });
    }

    static GetInformationChangeConsolidation(InsuredId) {
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
                        glbRisk.IsConsortium = true;
                        resolve(true);
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
                                        OperationCuotaAvalible: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT - Consortium.result.ApplyEndorsement.AmountCoverage,
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
        glbRisk.forEach(function (item, index) {
            if (item.Id == parseInt($("#selectRisk").val())) {
                dataRisk = item;
            }
        });
        if (EconomicGroupEvent != null || ConsortiumEvent != null || OperationQuotaEvent != null) {
            if (EconomicGroupEvent != null) {
                //Grupo Economico
                if (EconomicGroupEvent.OperationCuotaAvalible < dataRisk.Value.Value) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                    ChangeConsolidation.CancelConsolidationChange();
                }
            } else if (ConsortiumEvent != null) {
                //Consorcio
                if (ConsortiumEvent.OperationCuotaAvalible < dataRisk.Value.Value) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                    ChangeConsolidation.CancelConsolidationChange();
                }
            } else if (OperationQuotaEvent != null) {
                // Individual
                if (OperationQuotaEvent.OperationCuotaAvalible < dataRisk.Value.Value) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                    ChangeConsolidation.CancelConsolidationChange();
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
                ChangeConsolidation.CancelConsolidationChange();
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorAvaliableOperationQuota, 'autoclose': true });
            ChangeConsolidation.CancelConsolidationChange();
        }
    }

    static GetTemporalById(endorsementController, temporalId) {

        ChangeConsolidationRequest.GetTemporalById(endorsementController, temporalId).done(function (data) {
            if (data.success) {
                ChangeConsolidationRequest.GetRiskTemporalById(endorsementController, temporalId).done(function (resultRisk) {
                    if (resultRisk.success) {
                        data.result.Contractor = resultRisk.result;
                        glbRisk[0].Contractor = resultRisk.result.Contractor;
                        glbRisk.forEach(function (item, index) {
                            item.Description = item.Contractor.Name;
                            item.Id = item.Risk.RiskId;
                        });
                        ChangeConsolidation.GetHolderByIndividualId(resultRisk.result.Contractor.IndividualId, null);                        
                        glbAddChangeConsolidation.push(tempChangeConsolidation);                        
                        ChangeConsolidation.LoadEndorsementData(data.result);
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
                ChangeConsolidation.LoadConsolidationData(item.Contractor);
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
        ChangeConsolidation.LoadConsolidationData(policy.Contractor.Contractor, "#listChangeConsolidationModify");
    }

    static ShowIndividualResults(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static SelectIndividualResults(data) {
        var individualId = $(this).children()[0].innerHTML;
        var customerType = $(this).children()[1].innerHTML;
        ChangeConsolidation.GetHolderByIndividualId(individualId, customerType);
    }

    static GetHolderByIndividualId(individualId, customerType) {
        HoldersRequest.GetHoldersByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                $('#inputChangeConsolidationName').data('Object', data.result.holder.Name);
                $('#inputChangeConsolidationName').val(data.result.holder.Name);

                if (data.result.holder.CompanyName.Address != null || data.result.holder.CompanyName.Address != undefined) {
                    $('#inputChangeConsolidationAddress').val(data.result.holder.CompanyName.Address.Description);
                } else { $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ValidateAddressHolder, 'autoclose': false }); }

                if (data.result.holder.CompanyName.Phone != null || data.result.holder.CompanyName.Phone != undefined) {
                    $('#inputChangeConsolidationPhone').val(data.result.holder.CompanyName.Phone.Description);
                } else { $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ValidatePhoneHolder, 'autoclose': false }); }

                if (data.result.holder.CompanyName.Email != null || data.result.holder.CompanyName.Email != undefined) {
                    $('#inputChangeConsolidationEmail').val(data.result.holder.CompanyName.Email.Description);
                } else { $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ValidateEmailHolder, 'autoclose': false });}
                glbChangeConsolidation = data.result.holder.CompanyName;
                glbChangeConsolidation.Id = data.result.holder.IndividualId;
                glbChangeConsolidation.TradeName = data.result.holder.Name;
                ChangeConsolidation.GetInfoAfianzado(data.result.holder.IndividualId);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.DontExistBonded, 'autoclose': true });
            }
        });

        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    static PrintTemporal() {
        ChangeConsolidationRequest.GenerateReportTemporary(temporaryConsolidation.Endorsement.TemporalId, glbPolicy.Prefix.Id, temporaryConsolidation.Id).done(function (data) {
            if (data.success) {
                DownloadFile(data.result.Url, true, (Url) => Url.match(/([^\\]+.pdf)/)[0]);
            }
        });
    }
}