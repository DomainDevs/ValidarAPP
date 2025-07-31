var riskController = 'RiskFidelity';
var dynamicProperties = null;
var individualSearchType = 1;
var issueDate = null;
var gettingIssueDate = false;
var defaultDate = new Date(0, 0, 0, 0, 0, 0, 0);

class RiskFidelity extends Uif2.Page {

    getInitialState() {
        //VARIABLES
        if (glbRisk == null) {
            glbRisk = { Id: 0, Object: "RiskFidelity", formRisk: "#formFidelity", RecordScript: false, Class: RiskFidelity };
        }

        riskController = "RiskFidelity";
        $("#btnConvertProspect").hide();
        $('#tableIndividualResults').HideColums({ control: '#tableIndividualResults', colums: [0, 1] });
        $('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#inputInsured').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#listCoverages").UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: "#coverageTemplate", title: AppResources.LabelTitleCoverages, height: 540 });
        RiskFidelity.ShowPanelsRisk(MenuType.Risk);
        RiskFidelity.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
    }

    bindEvents() {

        $('#selectRisk').on('itemSelected', RiskFidelity.ChangeRisk);
        $("#inputInsured").on('buttonClick', this.SearchInsured);
        $("#btnDetail").on('click', this.ShowDetail);
        $('#btnIndividualDetailAccept').on('click', RiskFidelity.SetIndividualDetail);
        $('#selectGroupCoverage').on('itemSelected', this.ChangeGroupCoverage);
        $('#listCoverages').on('rowAdd', this.AddCoverage);
        $('#listCoverages').on('rowEdit', this.EditCoverage);
        $('#listCoverages').on('rowDelete', this.CoverageDelete);
        $("#btnAccept").on('click', this.Accept);
        $("#btnClose").on('click', this.Close);
        $("#btnIndividualDetailAccept").on('click', RiskFidelity.SetIndividualDetail);
        $("#btnScript").on('click', RiskFidelity.LoadScript);
        $("#btnAcceptNewPersonOnline").click(this.AcceptNewPersonOnline);
        $('#btnConvertProspect').click(this.ConvertProspect);
        $("#btnAddRisk").on('click', this.AddRisk);
        $('#btnDeleteRisk').on('click', RiskFidelity.DeleteRisk);
        $("#btnAcceptNewPersonOnline").click(this.AcceptNewPersonOnline);
        $('#btnConvertProspect').click(this.ConvertProspect);
        $('#tableIndividualResults tbody').on('click', 'tr', function (e) {
            if (individualSearchType == 2) {
                RiskBeneficiary.GetBeneficiariesByDescription($(this).children()[0].innerHTML, InsuredSearchType.IndividualId);
            }
            else {
                RiskFidelity.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
            }
            $('#modalIndividualSearch').UifModal('hide');
        });

    }
    static getPersonOnline() {
        if (glbPersonOnline != null) {
            if (glbPersonOnline.Rol == 2) {
                if (glbPersonOnline.IndividualId > 0) {
                    RiskFidelity.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPersonOnline.IndividualId, InsuredSearchType.IndividualId, glbPersonOnline.CustomerType);
                    if (glbPersonOnline.CustomerType == CustomerType.Individual) {
                        Underwriting.ConvertProspectToInsured(glbPersonOnline.IndividualId, glbPersonOnline.DocumentNumber);
                    }
                }
                else {
                    $("#inputInsured").data("Object", null);
                    $("#inputInsured").data("Detail", null);
                    if (glbPersonOnline.DocumentNumber.length > 0) {
                        $("#inputInsured").val(glbPersonOnline.DocumentNumber);
                    }
                    else {
                        $("#inputInsured").val(glbPersonOnline.Name);
                    }
                }
                if (glbRisk.Id > 0) {
                    RiskFidelity.GetRiskById(glbRisk.Id);
                }
                else {
                    RiskFidelity.LoadViewModel(glbPersonOnline.ViewModel)
                }
                glbPersonOnline = null;
            }
        }
    }
    static GetRisksByTemporalId(temporalId, selectedId) {
        var controller = ""

        RiskFidelityRequest.GetCiaRiskByTemporalId(temporalId).done(function (data) {
            if (data.success) {
                if (data.result != null && data.result.length > 0) {
                    if (selectedId == 0) {

                        $("#selectRisk").UifSelect({ sourceData: data.result });
                    }
                    else {
                        $("#selectRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                    }

                }

                if (glbPersonOnline != null) {
                    RiskFidelity.getPersonOnline();
                }
                else if (glbPolicy.TemporalType == TemporalType.Endorsement && selectedId == 0 && data.result.length > 0) {
                    $("#selectRisk").UifSelect("setSelected", data.result[0].Id);
                    RiskFidelity.GetRiskById(data.result[0].Id);

                }

                else if (glbRisk.Id > 0) {
                    RiskFidelity.GetRiskById(glbRisk.Id);
                }
                else {

                    RiskFidelity.GetGroupCoverages(glbPolicy.Product.Id, 0);
                    RiskFidelity.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
                    RiskFidelity.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);

                }
                if (glbRisk.DiscoveryDate != undefined && glbRisk.DiscoveryDate != null && glbRisk.DiscoveryDate > defaultDate) {
                    $("#inputDiscoveryDate").UifDatepicker('setValue', FormatDate(glbRisk.DiscoveryDate));
                }
                else {
                    RiskFidelity.GetIssueDate();
                }

                if (!glbPolicy.Product.IsFlatRate) {
                    $('#inputRate').prop('disabled', true);
                }
                RiskFidelity.LoadTitle();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }


        });
    }
    ChangeGroupCoverage(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskFidelity.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, selectedItem.Id);
        }
        else {
            $("#listCoverages").UifListView("refresh");
        }
        $('#inputPremium').text(0);
        $("#inputAmountInsured").text(0);
    }

    AddCoverage(event) {
        RiskFidelity.SaveRisk(MenuType.Coverage, 0);
    }

    EditCoverage(event, data, index) {
        if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageEditCoverage, 'autoclose': true });
            return false;
        }
        else {
            RiskFidelity.SaveRisk(MenuType.Coverage, data.Id);
        }
    }

    CoverageDelete(event, data) {
        RiskFidelity.DeleteCoverage(data);
    }

    Accept() {
        RiskFidelity.SaveRisk(MenuType.Risk, 0);
        ScrollTop();
    }

    Close() {
        glbRisk = { Id: 0, Object: "RiskFidelity", Class: RiskFidelity };
        RiskFidelity.UpdatePolicyComponents();
    }

    AddRisk() {
        const data = $(".uif-listview").UifListView("getData");
        $.each(data, function (index, item) {
            $(".uif-listview").UifListView("deleteItem", index);
        });
        RiskFidelity.ClearControls();
        RiskFidelity.RunRules(glbPolicy.Product.CoveredRisk.PreRuleSetId);
    }

    ShowDetail() {
        if ($("#inputInsured").data("Object") != undefined) {
            RiskFidelity.ShowInsuredDetail();
        }
    }

    AcceptNewPersonOnline() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskFidelity.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByDescription($("#inputInsured").val().trim());
    }

    ConvertProspect() {
        glbPersonOnline = {
            Rol: 2,
            ViewModel: RiskFidelity.GetRiskDataModel()
        };

        UnderwritingPersonOnline.RedirectToPersonsByIndividualIdIndividualTypeCustomerType($("#inputInsured").data("Object").IndividualId, $("#inputInsured").data("Object").IndividualType, $("#inputInsured").data("Object").CustomerType, 0);
    }

    SearchInsured() {
        if ($("#inputInsured").val().trim().length > 0) {
            $("#inputInsured").data("Object", null);
            $("#inputInsured").data("Detail", null);
            RiskFidelity.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputInsured").val().trim(), InsuredSearchType.DocumentNumber, null);
        }
    }

    static ChangeRisk(event, selectedItem) {
        if (selectedItem.Id > 0) {
            RiskFidelity.GetRiskById(selectedItem.Id);
            RiskFidelity.DisabledControlByEndorsementType(true);
        }
        else {
            RiskFidelity.ClearControls();
        }
    }


    static GetGroupCoverages(productId, selectedId) {
        var controller = "";
        RiskFidelityRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: controller });
                }
                else {
                    $("#selectGroupCoverage").UifSelect({ sourceData: controller, selectedId: selectedId });
                }
            }
        });
    }

    static ShowPanelsRisk(Menu) {
        switch (Menu) {
            case MenuType.Risk:
                break;
            case MenuType.Texts:
                $('#modalTexts').UifModal('showLocal', AppResources.LabelTexts);
                break;
            case MenuType.Clauses:
                $('#modalClauses').UifModal('showLocal', AppResources.LabelClauses);
                break;
            case MenuType.Beneficiaries:
                $('#modalBeneficiaries').UifModal('showLocal', AppResources.LabelBeneficiaries);
                break;
            case MenuType.Concepts:
                $('#modalConcepts').UifModal('showLocal', AppResources.Concepts + ': ' + $('#inputPlate').val());
                break;
            case MenuType.Script:
                RiskFidelity.LoadScript();
                break;
        }
    }

    static LoadTitle() {
        $.uif2.helpers.setGlobalTitle(glbPolicy.Title);
    }

    static GetRiskById(id) {
        if (id > 0 && glbPolicy.Endorsement != null) {
            RiskFidelityRequest.GetRiskById(id).done(function (data) {
                if (data.success) {
                    glbRisk.RecordScript = true;
                    RiskFidelity.LoadRisk(data.result);
                    RiskFidelity.DisabledControlByEndorsementType(true);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchRisk, 'autoclose': true });
            });
        }
    }

    static LoadRisk(riskData) {

        if (riskData.Risk != null && riskData.Risk.MainInsured != null) {
            if (riskData.Risk.MainInsured.CustomerType == CustomerType.Individual) {
                $("#btnConvertProspect").hide();
            } else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        if (riskData.Risk != null) {
            if ($('#selectRisk').length != 0) {
                if (riskData.Risk.Id > 0) {
                      $('#selectRisk').UifSelect('setSelected', riskData.Risk.Id);
                }

                if (riskData.Risk.MainInsured != null && riskData.Risk.MainInsured !== null && riskData.Risk.MainInsured != undefined) {
                    $('#inputInsured').data('Object', riskData.Risk.MainInsured);
                    $('#inputInsured').data('Detail', RiskFidelity.GetIndividualDetails(RiskFidelity.GetIndividualDetailsByIndividualId(riskData.Risk.MainInsured.IndividualId, riskData.Risk.MainInsured.CustomerType)));
                    $('#inputInsured').val(decodeURIComponent(riskData.Risk.MainInsured.Name + ' (' + riskData.Risk.MainInsured.IdentificationDocument.Number + ')'));
                }
                if (riskData.Risk.RiskActivity != null) {
                    RiskFidelity.GetRiskActivities(riskData.Risk.RiskActivity.Id);
                }
                else {
                    RiskFidelity.GetRiskActivities(0);
                }
                if (riskData.DiscoveryDate != undefined && riskData.DiscoveryDate != null && riskData.DiscoveryDate > defaultDate) {
                    $("#inputDiscoveryDate").UifDatepicker('setValue', FormatDate(riskData.DiscoveryDate));
                } else {
                    RiskFidelity.GetIssueDate();
                }

                if (riskData.IdOccupation != null) {
                    RiskFidelity.GetOccupations(riskData.IdOccupation);
                }
                else {
                    RiskFidelity.GetOccupations(0);
                }
                if (riskData.Risk.GroupCoverage != null) {
                    RiskFidelity.GetGroupCoverages(glbPolicy.Product.Id, riskData.Risk.GroupCoverage.Id);
                }

                if (riskData.Risk.Coverages != null) {
                    RiskFidelity.LoadCoverages(riskData.Risk.Coverages);
                }
                else if (riskData.Risk.GroupCoverage != null && riskData.Risk.GroupCoverage != 0) {
                    RiskFidelity.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Product.Id, riskData.Risk.GroupCoverage.Id);
                    $('#inputAmountInsured').text(0);
                    $('#inputPremium').text(0);
                }
                dynamicProperties = riskData.Risk.DynamicProperties;
            }
        }

        $('#chkIsDeclarative').prop('checked', riskData.IsDeclarative);
        RiskFidelity.UpdateGlbRisk(riskData);
        RiskFidelity.LoadSubTitles(0);
    }

    

    static LoadCoverages(coverages) {
        var totalAmount = 0;
        var totalPremium = 0;
        $('#listCoverages').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', height: 340, title: AppResources.LabelTitleCoverages });
        $.each(coverages, function (index, val) {
            totalPremium = totalPremium + coverages[index].PremiumAmount;
            totalAmount = totalAmount + coverages[index].LimitAmount;
            coverages[index].SubLimitAmount = FormatMoney(coverages[index].SubLimitAmount);
            coverages[index].PremiumAmount = FormatMoney(coverages[index].PremiumAmount);
            coverages[index].Rate = FormatMoney(coverages[index].Rate);
            $('#listCoverages').UifListView('addItem', coverages[index]);
        });
        $('#inputAmountInsured').text(FormatMoney(totalAmount));
        $('#inputPremium').text(FormatMoney(totalPremium));
    }

    static GetCoveragesByProductIdGroupCoverageId(productId, groupCoverageId) {
        var controller = "";
        RiskFidelityRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Id, productId, groupCoverageId, glbPolicy.Prefix.Id).done(function (data) {
            if (data.success) {
                controller = data.result;

                RiskFidelity.LoadCoverages(controller);

                $('#listCoverages').UifListView({ sourceData: controller, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', height: 340, title: AppResources.LabelCoverages });
            }

        });

    }

    static DisabledControlByEndorsementType(disabled) {
        var endorsementType = parseInt(glbPolicy.Endorsement.EndorsementType, 10);
        switch (endorsementType) {
            case EndorsementType.Renewal: {
                $('#inputInsured').prop('disabled', disabled);
                $('#selectRiskActivity').prop('disabled', disabled);
                $('#selectOccupation').prop('disabled', disabled);
                $('#inputDiscoveryDate').prop('disabled', disabled); 
                break;
            }
            case EndorsementType.Modification: {
                $('#inputInsured').prop('disabled', disabled);
                $('#selectRiskActivity').prop('disabled', disabled);
                $('#selectOccupation').prop('disabled', disabled);
                $('#inputDiscoveryDate').prop('disabled', disabled); 
                break;
            }
        }
    }

    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            if (customerType == null && glbPolicy.TemporalType == TemporalType.Policy) {
                customerType = CustomerType.Individual;
            }
            RiskFidelityRequest.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (individualSearchType == 1 || individualSearchType == 2) {
                        if (data.result.length == 0 && individualSearchType != 2) {
                            UnderwritingPersonOnline.ShowOnlinePerson();
                        }
                        else if (data.result.length == 0) {
                            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchInsureds, 'autoclose': true });
                        }
                        else if (data.result.length == 1) {
                            RiskFidelity.LoadInsured(data.result[0]);
                        }
                        else if (data.result.length > 1) {
                            modalListType = 1;
                            for (var i = 0; i < data.result.length; i++) {
                                dataList.push({
                                    Id: data.result[i].IndividualId,
                                    CustomerType: data.result[i].CustomerType,
                                    Code: data.result[i].IdentificationDocument.Number,
                                    Description: data.result[i].Name,
                                    CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                    DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                                });
                            }

                            RiskFidelity.ShowModalList(dataList);
                            $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
                        }
                    }
                } else {
                    if (individualSearchType == 1) {
                        $('#inputInsured').data("Object", null);
                        $('#inputInsured').data("Detail", null);
                        $('#inputInsured').val('');
                    }
                    else if (individualSearchType == 2) {
                        $('#inputBeneficiaryName').data("Object", null);
                        $('#inputBeneficiaryName').data("Detail", null);
                        $('#inputBeneficiaryName').val('');
                    }

                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (xhr, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorConsultingInsured, 'autoclose': true });
            });
        }
    }

    static ShowInsuredDetail() {
        $('#tableIndividualDetails').UifDataTable('clear');

        if ($('#inputInsured').data('Detail') != null) {
            var insuredDetails = $('#inputInsured').data('Detail');

            if (insuredDetails.length > 0) {
                $.each(insuredDetails, function (id, item) {
                    $('#tableIndividualDetails').UifDataTable('addRow', item)
                    if ($("#inputInsured").data("Object").CompanyName.NameNum > 0 && $("#inputInsured").data("Object").CompanyName.NameNum == this.NameNum) {
                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                    else if ($("#inputInsured").data("Object").CompanyName.NameNum == 0 && this.IsMain == true) {
                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                });

                if ($('#tableIndividualDetails').UifDataTable('getSelected') == null) {
                    $('#tableIndividualDetails tbody tr:eq(0)').removeClass('row-selected').addClass('row-selected');
                    $('#tableIndividualDetails tbody tr:eq(0) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                }
            }
        }

        $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelInsuredDetail);
    }

    static GetIndividualDetails(individualDetails) {
        if (individualDetails.length > 0) {
            $.each(individualDetails, function (id, item) {
                this.Detail = this.Address.Description;
                if (this.TradeName != null) {
                    this.Detail = '<b>' + this.TradeName + '</b>' + '<br/>' + this.Detail;
                }
                if (this.Phone != null) {
                    this.Detail += '<br/>' + this.Phone.Description;
                }
                if (this.Email != null) {
                    this.Detail += '<br/>' + this.Email.Description;
                }
                if (individualSearchType == 1) {
                    if ($("#inputInsured").data("Object").CompanyName == null) {
                        if (this.IsMain) {
                            $("#inputInsured").data("Object").CompanyName = this;
                        }
                    } else if ($("#inputInsured").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                        $("#inputInsured").data("Object").CompanyName = this;
                    }
                } else if (individualSearchType == 2) {
                    if ($("#inputBeneficiaryName").data("Object").CompanyName == null) {
                        if (this.IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = this;
                        }
                    } else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                        $("#inputBeneficiaryName").data("Object").CompanyName = this;
                    }
                }
            });
        }


        return individualDetails;
    }

    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        var resultData = {};

        RiskFidelityRequest.GetIndividualDetailsByIndividualId(individualId, customerType).done(function (data) {
            if (data.success) {
                resultData = data.result;
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchIndividualDetail, 'autoclose': true });

        });

        return resultData;
    }

    static SetIndividualDetail() {
        var details = $('#tableIndividualDetails').UifDataTable('getSelected');

        if (details == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectAddress, 'autoclose': true });
        }
        else {
            if (individualSearchType == 1) {
                $("#inputInsured").data("Object").CompanyName = details[0];
            }
            else if (individualSearchType == 2) {
                $("#inputBeneficiaryName").data("Object").CompanyName = details[0];
            }

            $('#modalIndividualDetail').UifModal('hide');
        }
    }

    static ClearControls() {

        $('#selectRisk').UifSelect("setSelected", null);
        $('#selectedTexts').UifSelect("setSelected", null);
        $('#selectedClauses').UifSelect("setSelected", null);
        $('#selectedBeneficiaries').UifSelect("setSelected", null);
        $('#inputPrice').val('');
        $('#selectGroupCoverage').prop('disabled', false);
        $("#listCoverages").UifListView("refresh");
        $('#inputRate').text(0);
        $('#lblAddress').val('');
        $('#lblPhone').val('');
        $('#lblEmail').val('');
        $('#inputAmountInsured').text(0);
        $('#inputPremium').text(0);
        $('#selectRiskActivity').UifSelect('setSelected', null);
        $("#selectOccupation").UifSelect('setSelected', null);
        RiskFidelity.GetIssueDate();
        $('#chkIsDeclarative').prop('checked', false);
        RiskFidelity.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
        glbRisk.RecordScript = false;
        RiskFidelity.UpdateGlbRisk({ Id: 0 });
        $('#selectedConceptsRisk').text('');
    }

    static SaveRisk(redirec, coverageId) {

        var recordScript = false;
        $('#formFidelity').validate();
        if (glbPolicy.Product.CoveredRisk.ScriptId == null || glbPolicy.Product.CoveredRisk.ScriptId == 0 || redirec == MenuType.Script) {
            recordScript = true;
        }
        else {
            recordScript = glbRisk.RecordScript;
        }

        if ($('#formFidelity').valid()) {
            if (!RiskFidelity.ValidateDiscoveryDate()) {
                return;
            }

            if (recordScript) {
                var riskData = RiskFidelity.GetRiskDataModel();
                RiskFidelityRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                    if (data.success) {
                        RiskFidelity.UpdateGlbRisk(data.result);
                        RiskFidelity.LoadSubTitles(0);
                        RiskFidelity.ShowSaveRisk(glbRisk.Id, redirec, coverageId);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
                });
            }
            else {
                RiskFidelity.LoadScript();
            }
        };
    }

    static GetRiskDataModel() {
        var riskData = $('#formFidelity').serializeObject();
        riskData.TemporalId = glbPolicy.Id;
        riskData.PrefixId = glbPolicy.Prefix.Id;
        riskData.ProductId = glbPolicy.Product.Id;
        riskData.PolicyTypeId = glbPolicy.PolicyType.Id;
        riskData.InsuredId = glbPolicy.Holder.IndividualId;
        riskData.HolderId = glbPolicy.Holder.IndividualId;
        riskData.EndorsementType = glbPolicy.Endorsement.EndorsementType;
        riskData.Id = $('#selectRisk').val();
        if ($('#inputInsured').data('Object') != undefined) {
            riskData.InsuredId = $('#inputInsured').data('Object').IndividualId;
            riskData.InsuredDocumentNumber = $('#inputInsured').data('Object').IdentificationDocument.Number;
            riskData.InsuredName = $('#inputInsured').data('Object').Name;
            riskData.InsuredCustomerType = $('#inputInsured').data('Object').CustomerType;
            riskData.InsuredBirthDate = FormatDate($("#inputInsured").data("Object").BirthDate);
            riskData.InsuredGender = $("#inputInsured").data("Object").Gender;
            if ($('#inputInsured').data('Object').CompanyName != null) {
                riskData.InsuredDetailId = $('#inputInsured').data('Object').CompanyName.NameNum;
                riskData.InsuredAddressId = $('#inputInsured').data('Object').CompanyName.Address.Id;
                if ($("#inputInsured").data("Object").CompanyName.Phone != null) {
                    riskData.InsuredPhoneId = $("#inputInsured").data("Object").CompanyName.Phone.Id;
                }
                if ($("#inputInsured").data("Object").CompanyName.Email != null) {
                    riskData.InsuredEmailId = $("#inputInsured").data("Object").CompanyName.Email.Id;
                }
            }

            if ($("#inputInsured").data("Object").IdentificationDocument != null && $("#inputInsured").data("Object").IdentificationDocument.DocumentType != null && $("#inputInsured").data("Object").IdentificationDocument.DocumentType.Id != null)
                riskData.InsuredDocumentTypeId = $("#inputInsured").data("Object").IdentificationDocument.DocumentType.Id;
            else
                if ($("#inputInsured").data("Object").IndividualType != null && $("#inputInsured").data("Object").IndividualType > 0)
                    riskData.InsuredDocumentTypeId = $("#inputInsured").data("Object").IndividualType;
                else
                    riskData.InsuredDocumentTypeId = 1;

        }
        riskData.RiskActivityId = $("#selectRiskActivity").UifSelect("getSelected");
        riskData.RiskActivityDescription = $("#selectRiskActivity").UifSelect("getSelectedText");
        riskData.OccupationDescription = $("#selectOccupation").UifSelect("getSelectedText");
        riskData.DiscoveryDate = $('#inputDiscoveryDate').val();
        riskData.GroupCoverage = $('#selectGroupCoverage').val();
        riskData.IsDeclarative = $('#chkIsDeclarative').is(':checked');
        riskData.IdOccupation = $('#selectOccupation').val();


        var coveragesValues = $("#listCoverages").UifListView('getData');
        $.each(coveragesValues, function (key, value) {
            this.CurrentFrom = FormatDate(this.CurrentFrom);
            this.CurrentTo = FormatDate(this.CurrentTo);
            this.LimitAmount = NotFormatMoney(this.LimitAmount);
            this.SubLimitAmount = NotFormatMoney(this.SubLimitAmount);
            this.PremiumAmount = NotFormatMoney(this.PremiumAmount);
            this.Rate = NotFormatMoney(this.Rate);
        });

        riskData.Coverages = coveragesValues;
        return riskData;
    }

    static ShowSaveRisk(riskId, redirec, coverageId) {
        if ($("#selectRisk").UifSelect("getSelected") == null || $("#selectRisk").UifSelect("getSelected") == 0) {
            RiskFidelity.GetRisksByTemporalId(glbPolicy.Id, riskId);
        }

        //lanza los eventos para la creación de el riesgo
        let events = LaunchPolicies.ValidateInfringementPolicies(glbRisk.InfringementPolicies);

        if (events !== TypeAuthorizationPolicies.Restrictive) {
            RiskFidelity.RedirectAction(redirec, riskId, coverageId);
        }
        //fin - lanza los eventos para la creación de el riesgo
    }

    static RedirectAction(redirec, riskId, coverageId) {
        switch (redirec) {
            case MenuType.Underwriting:
                RiskFidelity.ReturnUnderwriting();
                break;
            case MenuType.Risk:
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
                break;
            case MenuType.Coverage:
                RiskFidelity.ReturnCoverage(coverageId);
                break;
            case MenuType.Beneficiaries:
                RiskFidelity.ShowPanelsRisk(MenuType.Beneficiaries);
                break;
            case MenuType.Texts:
                RiskFidelity.ShowPanelsRisk(MenuType.Texts);
                break;
            case MenuType.Clauses:
                RiskFidelity.ShowPanelsRisk(MenuType.Clauses);
                break;
            case MenuType.Concepts:
                RiskFidelity.ShowPanelsRisk(MenuType.Concepts);
                break;
            case MenuType.Script:
                RiskFidelity.ShowPanelsRisk(MenuType.Script);
                break;
            default:
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveRisk, 'autoclose': true });
        }
    }

    static ReturnUnderwriting() {
        var url = '';

        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            glbPolicy.EndorsementController = "FidelityModification";
            router.run("prtModification")
        }
        else {
            if (glbPolicy.TemporalType == TemporalType.Quotation || glbPolicy.TemporalType == TemporalType.TempQuotation) {
                router.run("prtQuotation");
            }
            else {
                router.run("prtTemporal");
            }
        }
    }
    

    static ReturnCoverage(coverageId) {
        glbCoverage = {
            CoverageId: coverageId,
            Object: "RiskFidelityCoverage",
            Class: RiskFidelityCoverage

        }
        router.run("prtCoverageRiskFidelity");
    }

    static DeleteRisk() {
        glbRisk.Id = 0;
        if ($('#selectRisk').val() > 0) {
            RiskFidelityRequest.DeleteRisk(glbPolicy.Id, $('#selectRisk').val()).done(function (data) {
                if (data.success) {
                    if (parseInt(glbPolicy.Endorsement.EndorsementType, 10) == EndorsementType.Emission || parseInt($('#hiddenEndorsementType').val(), 10) == EndorsementType.Renewal || glbRisk.Status == RiskStatus.Included) {
                        RiskFidelity.ClearControls();
                        RiskFidelity.GetRisksByTemporalId(glbPolicy.Id, 0);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteRisk, 'autoclose': true });
                        ScrollTop();
                    }
                    else {
                        RiskFidelity.GetRisksByTemporalId(glbPolicy.Id, $('#selectRisk').UifSelect('getSelected'));
                        RiskFidelity.GetRiskById($('#selectRisk').UifSelect('getSelected'));
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDeleteRisk, 'autoclose': true });
            });
        }
    }

    static LoadSubTitles(subTitle) {
        if (subTitle == 0 || subTitle == 1) {
            if (glbRisk.Beneficiaries != null) {
                if (glbRisk.Beneficiaries.length > 0) {
                    $('#selectedBeneficiaries').text('(' + glbRisk.Beneficiaries.length + ')');
                }
                else {
                    $('#selectedBeneficiaries').text('(' + AppResources.LabelWithoutData + ')');
                }
            }
            else {
                $('#selectedBeneficiaries').text('(' + AppResources.LabelWithoutData + ')');
            }
        }

        if (subTitle == 0 || subTitle == 2) {
            if (glbRisk.Text != null) {
                if (glbRisk.Text.TextBody == null) {
                    glbRisk.Text.TextBody = '';
                }

                if (glbRisk.Text.TextBody.length > 0) {
                    $('#selectedTexts').text('(' + AppResources.LabelWithText + ')');
                }
                else {
                    $('#selectedTexts').text('(' + AppResources.LabelWithoutData + ')');
                }
            }
            else {
                $('#selectedTexts').text('(' + AppResources.LabelWithoutData + ')');
            }
        }

        if (subTitle == 0 || subTitle == 3) {
            if (glbRisk.Clauses != null) {
                if (glbRisk.Clauses.length > 0) {
                    $('#selectedClauses').text('(' + glbRisk.Clauses.length + ')');
                }
                else {
                    $('#selectedClauses').text('(' + AppResources.LabelWithoutData + ')');
                }
            }
            else {
                $('#selectedClauses').text('(' + AppResources.LabelWithoutData + ')');
            }
        }
        if (subTitle == 0 || subTitle == 4) {
            if (glbRisk.Concepts != null) {
                if (glbRisk.Concepts.length > 0) {
                    $('#selectedConceptsRisk').text("(" + glbRisk.Concepts.length + ")");
                }
                else {
                    $('#selectedConceptsRisk').text("(" + AppResources.LabelWithoutData + ")");
                }
            }
            else {
                $('#selectedConceptsRisk').text("(" + AppResources.LabelWithoutData + ")");
            }
        }
    }

    static ShowModalList(dataTable) {
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', dataTable);
    }

    static DeleteCoverage(data) {

        if (data.IsMandatory == true) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteMandatoryCoverage, 'autoclose': true });
        }
        else if (data.IsVisible == false) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageDeleteCoverage, 'autoclose': true });
        }
        else {
            var coverages = $('#listCoverages').UifListView('getData');
            $('#listCoverages').UifListView({ source: null, customDelete: true, customAdd: true, customEdit: true, add: true, edit: true, delete: true, displayTemplate: '#coverageTemplate', title: AppResources.LabelTitleCoverages, height: 340 });

            $.each(coverages, function (index, value) {
                if (this.Id != data.Id) {
                    $('#listCoverages').UifListView('addItem', this);
                }
                else {
                    if (data.EndorsementType == EndorsementType.Modification && data.RiskCoverageId > 0) {
                        var coverage = RiskFidelity.ExcludeCoverage(glbPolicy.Id, glbRisk.Id, data.RiskCoverageId, data.Description);

                        if (coverage != null) {
                            coverage.PremiumAmount = FormatMoney(coverage.PremiumAmount);
                            $('#listCoverages').UifListView('addItem', coverage);
                        }
                    }
                }
            });
            RiskFidelity.UpdatePremiunAndAmountInsured();
        }
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        var coverage = null;
        RiskFidelityRequest.ExcludeCoverage(temporalId, riskId, riskCoverageId, description).done(function (data) {
            if (data.success) {
                coverage = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorExcludeCoverage, 'autoclose': true });
        });

        return coverage;
    }

    static UpdatePremiunAndAmountInsured() {
        var coverages = $('#listCoverages').UifListView('getData');
        var totalSumInsured = 0;
        var totalPremium = 0;
        $.each(coverages, function (index, value) {
            totalPremium = totalPremium + parseFloat(NotFormatMoney(coverages[index].PremiumAmount));
            totalSumInsured = totalSumInsured + parseFloat(NotFormatMoney(coverages[index].LimitAmount));

        });
        $('#inputPremium').text(FormatMoney(totalPremium));
        $('#inputAmountInsured').text(FormatMoney(totalSumInsured));
    }

    static LoadScript() {
        if (glbRisk.Id == 0) {
            RiskFidelity.SaveRisk(MenuType.Script, 0);
        }

        if (glbRisk.Id > 0 && glbPolicy.Product.CoveredRisk.ScriptId > 0) {
            ExecuteScript.Execute(glbPolicy.Product.CoveredRisk.ScriptId, glbRisk.Object, glbRisk.Class, dynamicProperties);
        }
    }

    static UpdateGlbRisk(data) {
        var recordScript = glbRisk.RecordScript;
        glbRisk = data
        $.extend(glbRisk, data.Risk);
        glbRisk.Risk = null;
        glbRisk.Object = "RiskFidelity";
        glbRisk.RecordScript = recordScript;
        glbRisk.Class = RiskFidelity;

        formRisk: "#formFidelity";
        if (glbRisk.CompanyRisk != null && glbRisk.CompanyRisk.CompanyInsured != null) {
            if ($("#inputInsured").data("Object") != null) {
                $("#inputInsured").data("Object").CompanyName = glbRisk.CompanyRisk.CompanyInsured.CompanyName;
            }
        }
    }

    static RunRules(ruleSetId) {
        RiskFidelityRequest.RunRules(glbPolicy.Id, ruleSetId).done(function (data) {
            RiskFidelity.LoadRisk(data.result);
        }).fail(function () {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorRunRules, 'autoclose': true });
        });
    }

    static PostScript(isModify, dynamicproperties) {
        dynamicProperties = jQuery.extend(true, [], dynamicproperties);
        glbRisk.RecordScript = isModify;
        if (isModify) {
            RiskFidelity.GetPremium();
        }
    }

    static GetPremium() {
        $("#formFidelity").validate();

        if ($("#formFidelity").valid()) {
            if (!RiskFidelity.ValidateDiscoveryDate()) {
                return;
            }

            var riskData = RiskFidelity.GetRiskDataModel();

            $.ajax({
                type: "POST",
                url: rootPath + 'Underwriting/RiskFidelity/GetPremium',
                data: JSON.stringify({ riskModel: riskData, coverages: riskData.Coverages, dynamicProperties: dynamicProperties }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            })
            RiskFidelityRequest.GetPremium(riskData, riskData.Coverages, dynamicProperties).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        RiskFidelity.LoadRisk(data.result);
                    }
                }
                else {
                    $("#inputPremium").text(0);
                    $("#inputAmountInsured").text(0);
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCalculatePremium, 'autoclose': true });
            });
        }
    }

    static LoadInsured(insured) {
        if (individualSearchType == 1) {
            $("#inputInsured").data("Object", insured);
            $("#inputInsured").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                $("#inputInsured").data("Detail", RiskFidelity.GetIndividualDetails(insured.CompanyName));
                $("#btnConvertProspect").hide();
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
        else if (individualSearchType == 2) {
            $("#inputBeneficiaryName").data("Object", insured);
            $("#inputBeneficiaryName").val(insured.Name + ' (' + insured.IdentificationDocument.Number + ')');
            if (insured.CustomerType == CustomerType.Individual) {
                $("#inputBeneficiaryName").data("Detail", RiskFidelity.GetIndividualDetails(insured.CompanyName));
                $("#btnConvertProspect").hide();
            }
            else if (glbPolicy.TemporalType != TemporalType.Quotation) {
                $("#btnConvertProspect").show();
            }
        }
    }

    static LoadViewModel(viewModel) {
        
        RiskFidelity.GetRiskActivities(viewModel.RiskActivityId);
        RiskFidelity.GetOccupations(viewModel.IdOccupation);
        $('#chkIsDeclarative').prop('checked', viewModel.IsDeclarative);
        if (viewModel.Coverages != "") {
            $.each(viewModel.Coverages, function (key, value) {
                this.LimitAmount = parseFloat((this.LimitAmount).replace(',', '.'));
                this.SubLimitAmount = parseFloat((this.SubLimitAmount).replace(',', '.'));
                this.PremiumAmount = parseFloat((this.PremiumAmount).replace(',', '.'));
            });
            RiskFidelity.LoadCoverages(viewModel.Coverages);
        }
        else if (viewModel.GroupCoverage != "") {
            RiskFidelity.GetCoveragesByGroupCoverageId(viewModel.GroupCoverage);
        }
        RiskFidelity.LoadSubTitles(0);
    }
    static GetCoveragesByGroupCoverageId(groupCoverageId) {
        RiskFidelityRequest.GetCoveragesByProductIdGroupCoverageId(glbPolicy.Id, groupCoverageId).done(function (data) {
            if (data.success) {
                RiskFidelity.LoadCoverages(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchCoveragesAccesories, 'autoclose': true });
        });
    }
    static UpdatePolicyComponents() {
        RiskFidelityRequest.UpdatePolicyComponents(glbPolicy.Id).done(function (data) {
            if (data.success) {
                Underwriting.UpdateGlbPolicy(data.result);
                RiskFidelity.ReturnUnderwriting();
            } else {
                $.UifNotify("show", { "type": "danger", "message": AppResources.ErrorSaveTemporary, 'autoclose': true });
            }
        });
    }

    static GetRiskActivities(selectedId) {
        var controller = "";
        RiskFidelityRequest.GetRiskActivitiesByProductId(glbPolicy.Product.Id).done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    $("#selectRiskActivity").UifSelect({ sourceData: controller });
                }
                else {
                    $("#selectRiskActivity").UifSelect({ sourceData: controller, selectedId: selectedId });
                }
            }
        });
    }

    static GetOccupations(selectedId) {
        var controller = "";
        RiskFidelityRequest.GetOccupations().done(function (data) {
            if (data.success) {
                controller = data.result;
                if (selectedId == 0) {
                    $("#selectOccupation").UifSelect({ sourceData: controller });
                }
                else {
                    $("#selectOccupation").UifSelect({ sourceData: controller, selectedId: selectedId });
                }
            }
        });
    }

    static GetGroupCoverages(productId, selectedId) {
        RiskVehicleRequest.GetGroupCoverages(productId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#selectGroupCoverage").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetIssueDate() {
        if (issueDate == null && !gettingIssueDate) {
            gettingIssueDate = true;
            RiskFidelityRequest.GetModuleDateIssueSynk().done(function (data) {
                gettingIssueDate = false;
                if (data.success) {
                    issueDate = data.result;
                    $("#inputDiscoveryDate").UifDatepicker('setValue', FormatDate(issueDate));
                } else {
                    issueDate = null;
                }
            });
        }
        if (issueDate != null) {
            $("#inputDiscoveryDate").UifDatepicker('setValue', FormatDate(issueDate));
        }
    }

    static ValidateDiscoveryDate() {
        if (issueDate == undefined || issueDate == null) {
            RiskFidelity.GetIssueDate();
            if (issueDate == null) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetIssueDate, 'autoclose': true });
                return false;
            }
        }

        if (CompareDates($('#inputDiscoveryDate').val(), FormatDate(issueDate))) {
            var message = Resources.Language.ErrorDiscoveryDateFidelity;
            message = message.replace('{0}', FormatDate(issueDate));
            $.UifNotify('show', { 'type': 'danger', 'message': message, 'autoclose': true });
            return false;
        }
        return true;
    }

}