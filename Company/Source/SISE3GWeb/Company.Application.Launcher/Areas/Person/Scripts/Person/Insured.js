var InsuredMain;
var AgentId;
var boolRegimeType = false;
var boolElectronicBiller = true;
class Insured extends Uif2.Page {
    getInitialState() {


    }

    //Seccion Eventos
    bindEvents() {
        $("#btnAcceptInsured").click(this.CreateInsuredFull);
        $("#btnInsured").click(this.BtnInsured);
        $('#selectLowReason').on('itemSelected', this.LowReason);
        $('#tableResultsInsured tbody').on('click', 'tr', this.ResultsInsured);
        $('#inputInsMain').on('buttonClick', Insured.LoadInsMain)
        $('#tableResultsInsMain tbody').on('click', 'tr', this.SelectSearchInsMain);
    }

    CreateInsuredFull() {
        if (Insured.validateInsured()) {
            lockScreen();
            insuredTmp = Insured.CreateInsuredModel();
            if (insuredTmp.IndividualId > 1) {
                lockScreen();
                InsuredRequest.CreateInsured(insuredTmp).done(function (data) {
                    if (data.success) {

                        var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                        let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                        if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                            if (countAuthorization > 0) {
                                LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonInsured);
                            }
                        } else {
                            $('#CodInsured').val(data.result.Id);
                            $('#CodInsured').text(data.result.Id);
                            $.UifDialog('alert', { 'message': AppResourcesPerson.InsuredCodeNo + data.result.Id + ' ' + AppResourcesPerson.CreatedSuccessfully }, function () { $('#btnCancel').click(); });
                            $('#checkInsured').prop('checked', true);
                            $('#checkInsured').addClass('primary');
                            if (glbPolicy != null)
                            {
                                if (glbPolicy.Summary.RisksInsured != null) {
                                    glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].CustomerType = CustomerType.Individual;
                                    glbPolicy.Summary.RisksInsured[0].Beneficiaries[0].IndividualId = data.result.IndividualId;
                                }
    
                                glbPersonOnline.IndividualId = data.result.IndividualId;
                                glbPersonOnline.CustomerType = CustomerType.Individual;
    
                                if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) {
                                    router.run("prtQuotation");
                                }
                                else {
                                    router.run("prtTemporal");
                                }
                            }
                        }

                        $("#modalElectronicBilling").UifModal('showLocal', AppResourcesPerson.ElectronicBilling);                        
                        ElectronicBilling.loadElectronicBilling();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                    unlockScreen();
                }).fail(() => unlockScreen());
            } else {
                Person.Insured = insuredTmp;
                $('#btnCancel').click();
            }
            unlockScreen();
        }
    }

    BtnInsured() {
        var TypePartnership = $('#selectSearchPersonType').val();
        switch (TypePartnership) {
            case "1":
                if ((parseInt($('#selectTypePartnership').val() || 0) > 1) || !(individualId == Person.New || individualId <= 0)) {
                    $.uif2.helpers.setGlobalTitle(AppResourcesPerson.TitleInsured + " " + Persons.PeripheralTitle());
                    Insured.loadInsured();
                }
                else {
                    $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
                }
                break;
            case "2":
                $.uif2.helpers.setGlobalTitle(AppResourcesPerson.TitleInsured + " " + Persons.PeripheralTitle());
                Insured.loadInsured();
                break;
        }
    }

    LowReason() {
        if ($('#selectLowReason').UifSelect("getSelected") == "") {
            $("#InputDateDeclinedInsured").UifDatepicker('setValue', null);
        }
        else {
            $("#InputDateDeclinedInsured").UifDatepicker('setValue', GetCurrentFromDate());
        }
    }

    ResultsInsured() {
        if ($(this).children()[4].innerHTML != "") {
            $.UifDialog('alert', { 'message': AppResourcesPerson.LabelAgentDisabled });
            return false;
        }
        if (insuredTmp === null) {
            insuredTmp = [];
        }
        insuredTmp.Agency = { Individual: { IndividualId: $(this).children()[0].innerHTML }, Id: 0, FullName: '' };
        $('#inputAgentPrincipal').val($(this).children()[3].innerHTML + ' (' + $(this).children()[1].innerHTML + ')');
        $('#inputAgentPrincipal').data("Agent", { AgentId: $(this).children()[1].innerHTML, IndividualId: $(this).children()[0].innerHTML })
        AgentId = $(this).children()[1].innerHTML;
        Persons.GetAgenciesByAgentId($(this).children()[0].innerHTML);
        $('#modalDialogListInsured').UifModal("hide");
    }

    static SelectSearchInsMain() {
        $(this).children()[0].innerHTML;
        InsuredMain = { IndividualId: $(this).children()[0].innerHTML, FullName: $(this).children()[1].innerHTML };
    }

    static LoadInsMain() {
        InsuredRequest.GetInsMain($('#inputInsMain').val()).done(function (data) {
            var dataList = {
                dataObject: []
            }

            if (data.result.length > 1) {
                for (var i = 0; i < data.result.length; i++) {
                    dataList.dataObject.push({
                        IndividualId: data.result[i].IndividualId,
                        FullName: data.result[i].FullName
                    });
                }
                Insured.ShowModalList(dataList.dataObject);

                $('#modalListInsMain').UifModal('showLocal', AppResourcesPerson.InsuredName);
            }
            else if (data.result.length == 1) {
                dataList.dataObject.push({
                    IndividualId: data.result[0].IndividualId,
                    FullName: data.result[0].FullName
                });
                InsuredMain = dataList.dataObject[0];
                $("#inputInsMain").val(InsuredMain.FullName);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': 'No se encontraron asegurados', 'autoclose': true });
            }

        });

    }

    static ShowModalList(dataTable) {
        $('#tableResultsInsMain').UifDataTable('clear');
        var table = $('#tableResultsInsMain').DataTable();
        table.on('draw', function () {
            $('#tableResults tbody td:nth-child(1)').hide();
            $('#tableResults thead th:eq(0)').hide();
        });
        $('#tableResultsInsMain').UifDataTable('addRow', dataTable);
    }

    static async loadInsured() {
        if (insuredTmp != null && insuredTmp.BranchId) {
            Persons.ShowPanelsPerson(RolType.Insured);
            return;
        }
        $.UifProgress('show');

        var insuredPromises = [];
        insuredPromises.push(InsuredRequest.GetInsuredDeclinedTypes());
        insuredPromises.push(InsuredRequest.GetInsProfiles());
        insuredPromises.push(InsuredRequest.GetInsBranchs());
        insuredPromises.push(InsuredRequest.GetInsSegment());

        var data = await Promise.all(insuredPromises);

        if (data[0].success) {
            $("#selectLowReason").UifSelect({ sourceData: data[0].result });
        }

        if (data[1].success) {
            $("#selectInsProfile").UifSelect({ sourceData: data[1].result });
        }

        if (data[2].success) {
            $("#selectBranchInsured").UifSelect({ sourceData: data[2].result });
        }

        if (data[3].success) {
            $("#selectInsSegment").UifSelect({ sourceData: data[3].result });
        }

        Insured.initializeControlInsured();
        $('#CodPersonId').val(individualId);
        $('#CodPersonId').text(individualId);
        $.UifProgress('show');
        InsuredRequest.GetInsuredByIndividualId(individualId).done(function (data) {
            $.UifProgress('close');
            if (data.success) {
                if (data.result != null) {
                    Insured.InsuredModel(data.result);
                    insuredTmp = data.result;
                } else {
                    $("#inputCreationDate").UifDatepicker("setValue", DateNowPerson);
                }
            }

        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
        Persons.ShowPanelsPerson(RolType.Insured);
    }

    static InsuredModel(data) {
        if (data.Id != null) {
            $('#CodInsured').val(data.Id);
            $('#CodInsured').text(data.Id);
        }

        $("#selectLowReason").val(data.InsDeclinesTypeId);
        $("#notesInsured").val(data.Annotations);
        $("#inputAgentPrincipal").val(data.IdDescription);


        if (data.InsDeclinesTypeId != null) {
            var dateDeclined = FormatDate(data.DeclinedDate);
            if (dateDeclined != null) {
                $("#InputDateDeclinedInsured").UifDatepicker("setValue", dateDeclined);
            }
        }

        $('#inputAgentPrincipal').data("Agentcy", null);
        if (data.AgentId != null) {
            Persons.GetAgenciesByAgentId(data.AgentId, data.AgencyId);
            $('#inputAgentPrincipal').data("Agent", { AgentId: data.AgentId, IndividualId: data.AgencyId });
        }

        if (data.EnteredDate != null) {
            $("#inputCreationDate").UifDatepicker('setValue', FormatDate(data.EnteredDate, 1));
        }
        if (data.ModifyDate != null) {
            $("#inputModificationDate").UifDatepicker('setValue', FormatDate(data.ModifyDate, 1));
        }


        if (data.IsInsured == true) {
            $('#rdAsegurado').prop('checked', true);
        } else {
            $('#rdAsegurado').prop('checked', false);
        }
        if (data.IsBeneficiary == true) {
            $('#rdbeneficiary').prop('checked', true);
        } else {
            $('#rdbeneficiary').prop('checked', false);
        }
        if (data.IsHolder == true) {
            $('#rdholder').prop('checked', true);
        } else {
            $('#rdholder').prop('checked', false);
        }
        if (data.IsPayer == true) {
            $('#rdPayer').prop('checked', true);
        } else {
            $('#rdPayer').prop('checked', false);
        }

        if (data.IsSms == true) {
            $('#rdAuthorizeSMS').prop('checked', true);
        } else {
            $('#rdAuthorizeSMS').prop('checked', false);
        }

        if (data.IsMailAddress == true) {
            $('#rdAuthorizeEmail').prop('checked', true);
        } else {
            $('#rdAuthorizeEmail').prop('checked', false);
        }

        $("#selectBranchInsured").val(data.BranchId);

        if (data.InsSegmentId > 0) {
            $("#selectInsSegment").val(data.InsSegmentId);
        }
        if (data.InsProfileId > 0) {
            $("#selectInsProfile").val(data.InsProfileId);
        }

        boolRegimeType = data.RegimeType;
        boolElectronicBiller = data.ElectronicBiller;

    }

    static CreateInsuredModel() {
        var isInsured = false, IsBeneficiary = false, IsHolder = false, IsPayer = false

        if ($('input[name=rdPrincipalInsured]')[0].checked == true) {
            isInsured = true;

        }
        if ($('input[name=rdPrincipalInsured]')[1].checked == true) {
            IsBeneficiary = true;

        }
        if ($('input[name=rdPrincipalInsured]')[2].checked == true) {
            IsHolder = true;

        }
        if ($('input[name=rdPrincipalInsured]')[3].checked == true) {
            IsPayer = true;
        }

        var agent = $('#inputAgentPrincipal').data("Agent");

        var dateDeclined = null;
        var dateModify = null;

        if ($("#selectLowReason").val() != "") {
            dateDeclined = DateNowPerson;
        }

        if ($("#CodInsured").val() != "") {
            dateModify = DateNowPerson;
        }

        var InsDeclinesType = $("#selectLowReason").val() == "" ? null : $("#selectLowReason").val();

        var temp = {
            Id: $('#CodInsured').val(),
            IndividualId: individualId,
            AgentId: (agent != null) ? agent.AgentId : null,
            AgencyId: $("#selectAgency").UifSelect("getSelected"),
            Annotations: $("#notesInsured").val(),
            EnteredDate: $("#inputCreationDate").val(),
            ModifyDate: dateModify,
            DeclinedDate: dateDeclined,
            UpdateDate: dateModify,
            InsDeclinesTypeId: InsDeclinesType,
            BranchId: $("#selectBranchInsured").val(),
            InsProfileId: $("#selectInsProfile").val(),
            InsSegmentId: $("#selectInsSegment").val(),
            IsSMS: $("#rdAuthorizeSMS").prop('checked'),
            IsMailAddress: $("#rdAuthorizeEmail").prop('checked'),
            IsBeneficiary: IsBeneficiary,
            IsHolder: IsHolder,
            IsPayer: IsPayer,
            IsInsured: isInsured,
            RegimeType: boolRegimeType,
            ElectronicBiller: boolElectronicBiller


            //NameAgent: $("#inputAgentPrincipal").val(),
            //Agent: agent,
            //EnteredDate: $("#inputCreationDate").val(),
            //DeclinedDate: dateDeclined,
            //InsDeclinesType: InsDeclinesType,
            //Annotations: $("#notesInsured").val(),
            //Profile: RolType.Insured,
            //ModifyDate: dateModify,
            //InsuredId: $("#CodInsured").val() == "" ? 0 : $("#CodInsured").val()
            //InsuredConcept: InsuredConcept,
            //BranchCode: $("#selectBranchInsured").val(),
            //CompanyInsuredSegment: { IndividualId: $("#selectInsSegment").val() },
            //CompanyInsuredProfile: { IndividualId: $("#selectInsProfile").val() },
            //CompanyInsuredMain: InsuredMain,
            //ReferedBy: $("#inputReferring").val(),

            //IsComercialClient: $("#rdCustomerBusinessAgreement").prop('checked'),

        };
        return temp;
    }

    static CreateInsuredAgent(insuredModel) {
        var agency = {
            Id: $("#selectAgency").UifSelect("getSelected"),
            agent: { Id: AgentId }
        }

        var modelInsuredAgent = {
            Id: insuredModel.Id,
            IndividualId: insuredModel.Id,
            Agency: agency
        };

        InsuredRequest.CreateInsuredAgent(modelInsuredAgent).done(function (data) {
            if (data.success) {
                $("#CodInsured").val(data.result["Insured"])
                $.UifDialog('alert', { 'message': AppResourcesPerson.SuccessfulCreatedInsured + ':' });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }

        });
    }

    static initializeControlInsured() {
        $("#inputAgentPrincipal").val("");
        $("#selectAgency").UifSelect('setSelected', null);
        $("#CodPersonId").val("");
        $("#CodPersonId").text("");
        $("#CodInsured").val("");
        $("#CodInsured").text("");
        $("#inputCreationDate").UifDatepicker("setValue", DateNowPerson);
        $("#selectLowReason").UifSelect('setSelected', null);
        $("#notesInsured").val("");
        $('#inputAgentPrincipal').data("Agent", null);
        $('#inputAgentPrincipal').data("Agentcy", null);
        $('#rdAsegurado').prop('checked', false);
        $('#rdbeneficiary').prop('checked', false);
        $('#rdholder').prop('checked', false);
        $('#rdPayer').prop('checked', false);
        $("#inputReferring").val("");
        $("#inputInsMain").val("");
        $("#selectInsProfile").UifSelect('setSelected', null);
        $("#selectBranchInsured").UifSelect('setSelected', null);
        $("#selectInsSegment").UifSelect('setSelected', null);
        InsuredMain: null;
        $("#rdAuthorizeSMS").prop('checked', false);
        $("#rdAuthorizeEmail").prop('checked', false);
        $("#rdCustomerBusinessAgreement").prop('checked', false);

    }

    static ShowModalListInsured(dataTable) {
        $('#tableResultsInsured').UifDataTable('clear');
        var table = $('#tableResultsInsured').DataTable();
        table.on('draw', function () {
            $('#tableResultsInsured tbody td:nth-child(1)').hide();
            $('#tableResultsInsured tbody td:nth-child(4)').hide();
            $('#tableResultsInsured tbody td:nth-child(5)').hide();
        });
        $('#tableResultsInsured thead th:eq(0)').hide();
        $('#tableResultsInsured thead th:eq(3)').hide();
        $('#tableResultsInsured thead th:eq(4)').hide();
        $('#tableResultsInsured').UifDataTable('addRow', dataTable);
    }

    static GetInsuredByIndividualId() {
        InsuredRequest.GetInsuredByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    if (data.result.InsuredId != null) {
                        $('#CodInsured').val(data.result.Id);
                    }
                } else {
                    var insuredTmp = Insured.CreateInsuredConsortiumModel();
                    InsuredRequest.CreateInsuredConsortium(insuredTmp).done(function (data) {
                        if (data.success) {
                            $('#CodInsured').val(data.result.Id);
                            $('#CodInsured').text(data.result.Id);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
            }
        });
    }

    static CreateInsuredConsortiumModel(individual) {
        var InsuredConcept = { InsuredCode: 0, isInsured: true, IsBeneficiary: false, IsHolder: false, IsPayer: false };
        var insured = {
            NameAgent: "",
            Id: individual,
            Agent: null,
            EnteredDate: DateNowPerson,
            DeclinedDate: "",
            InsDeclinesType: null,
            Annotations: null,
            Profile: RolType.Insured,
            ModifyDate: null,
            InsuredId: 0,
            IndividualId: individual,
            InsuredConcept: InsuredConcept
        }
        return insured;
    }

    static validateInsured() {

        var msjInsured = "";

        if ($("#selectBranchInsured").UifSelect("getSelected") == "") {
            msjInsured = msjInsured + AppResourcesPerson.Branch + '<br>';
        }
        if ($("#selectInsSegment").UifSelect("getSelected") == "") {
            msjInsured = msjInsured + AppResourcesPerson.LabelBusinessType + '<br>';
        }
        if ($("#selectInsProfile").UifSelect("getSelected") == "") {
            msjInsured = msjInsured + AppResourcesPerson.LabelWalletClassification + '<br>';
        }
        if (msjInsured != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + msjInsured })
            return false;
        }
        return true;
    }


}