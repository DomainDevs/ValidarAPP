var personID = null;
var InsuredindividualID = null;
//Codigo de la pagina Temporal.cshtml
class SearchInsured extends Uif2.Page {
    getInitialState() {
        if (glbPolicy == null) {
            glbPolicy = { Id: 0, TemporalType: TemporalType.Policy, Endorsement: { EndorsementType: EndorsementType.Emission } };
        }
        $("#inputHolder_").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        try {
            $('#tableIndividualResults_').HideColums({ control: '#tableIndividualResults_', colums: [0, 1] });
        } catch (e){
            //
        }
        //$('#tableTextResults').HideColums({ control: '#tableTextResults', colums: [2] });
        $("#inputHolder_").data("Object", null);
        $("#inputHolder_").data("Detail", null);
    }
    //Seccion Eventos
    bindEvents() {
        $('#tableIndividualResults_ tbody').on('click', 'tr', SearchInsured.SelectIndividualResults);
        $($('#inputHolder_').next()).click(function () { SearchInsured.SearchHolder() });
        //$("#inputHolder_").on('click', this.ShowDetail);
        $("#btnSearch_").on('click', this.SearchInsuredById);
        $("#btnNew").click(this.Exit);
    }

    SearchInsuredById() {
        SearchInsured.GuaranteeLoad(InsuredindividualID);
    }

    static GuaranteeLoad(individualId) {

        var individual = individualId;//$("#inputSecure").data("Object").IndividualId;
        var GuaranteeViewModel = {
            ContractorId: individual,
            searchType: TypePerson.PersonNatural,
            isSearch: true
        };
        //glbPolicy.TemporalType = 4;
        if (parseInt(individual) >= 0) {
            guaranteeModel = GuaranteeViewModel;
        }
        router.run("prtGuaranteeE");
    }

    static SetIndividualDetail() {
        var details = $('#tableIndividualDetails').UifDataTable('getSelected');

        if (details == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectAddress, 'autoclose': true });
        }
        else {
            if (detailType == 1) {
                $("#inputHolder_").data("Object").CompanyName = details[0];
            }
            else if (detailType == 2) {
                $("#inputBeneficiaryName").data("Object").CompanyName = details[0];
            }

            $('#modalIndividualDetail').UifModal('hide');
        }
    }

    static GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        
        var number = parseInt(description, 10);
        var dataList = [];
        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            UnderwritingRequest.GetHolders(description, insuredSearchType, customerType, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    if (data.result.length == 0 && individualSearchType == 2) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                    }
                    else if (data.result.length == 0) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessagePerson, 'autoclose': true });
                    }
                    else if (data.result.length == 1) {
                        SearchInsured.LoadHolder(data.result[0]);
                    }
                    else if (data.result.length > 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription
                            });
                        }

                        SearchInsured.ShowIndividualResults(dataList); 
                        $('#modalIndividualSearch_').UifModal('showLocal', AppResources.LabelSelectHolder);
                    }
                }
                else {
                    if (individualSearchType == 2) {
                        $("#inputBeneficiaryName").data("Object", null);
                        $("#inputBeneficiaryName").data("Detail", null);
                        $("#inputBeneficiaryName").val('');
                    }
                    else {
                        $("#inputHolder").data("Object", null);
                        $("#inputHolder").data("Detail", null);
                        $("#inputHolder").val('');
                    }
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchHolders, 'autoclose': true });
            });
        }
    }

    static ShowIndividualResults(dataTable) {
        $('#tableIndividualResults_').UifDataTable('clear');
        $('#tableIndividualResults_').UifDataTable('addRow', dataTable);
    }

    static SelectIndividualResults(e) {
        
        SearchInsured.GetHoldersByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, $(this).children()[1].innerHTML);
        $('#modalIndividualSearch_').UifModal("hide");
    }

    SearchTemporal() {
        $('#inputTemporalbutton').prop('disabled', true);
        UnderwritingTemporal.GetTemporalById($('#inputTemporal').val().trim());
        $('#inputTemporalbutton').prop('disabled', false);
    }
    
    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    New() {
        SearchInsured.ClearControls();
        //Underwriting.GetDefaultValues();
    }

    static SearchHolder() {
        
        if ($("#inputHolder_").val().trim().length > 0) {
            SearchInsured.GetHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputHolder_").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual);
        }
    }

    //ShowDetail() {
    //    if ($("#inputHolder_").val().length > 0) {
    //        SearchInsured.ShowModalHolderDetail();
    //    }
    //}

    static GetTemporalById(id) {
        if (id > 0) {
            TemporalRequest.GetTemporalByIdTemporalType(id, glbPolicy.TemporalType).done(function (data) {
                if (data.success) {
                    Underwriting.UpdateGlbPolicy(data.result);
                    Underwriting.LoadTemporal(glbPolicy);
                    if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
                        Underwriting.LoadSiteRisk();
                    }
                }
                else {
                    Underwriting.ClearControls();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Underwriting.ClearControls();
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
            });
        }
    }
    
    
    static ClearControls() {
        $("#inputHolder_").val("");
    }

    //static ShowModalHolderDetail() {
    //    
    //    if ($("#inputHolder_").data('Detail') !== null) {
    //            var holderDetails = $("#inputHolder_").data('Detail');

    //            if (holderDetails.length > 0) {
    //                $.each(holderDetails, function (id, item) {
    //                    $('#tableIndividualDetails').UifDataTable('addRow', item)
    //                    if ($("#inputHolder_").data("Object") != null && $("#inputHolder_").data("Object").CompanyName.NameNum > 0 && $("#inputHolder_").data("Object").CompanyName.NameNum == this.NameNum) {
    //                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
    //                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
    //                    }
    //                    else if ($("#inputHolder_").data("Object") != null && $("#inputHolder_").data("Object").CompanyName.NameNum == 0 && this.IsMain == true) {
    //                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
    //                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
    //                    }
    //                });

    //                if ($('#tableIndividualDetails').UifDataTable('getSelected') == null) {
    //                    $('#tableIndividualDetails tbody tr:eq(0)').removeClass('row-selected').addClass('row-selected');
    //                    $('#tableIndividualDetails tbody tr:eq(0) td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
    //                }
    //            }
    //        }
    //}
    static LoadHolder(holder) {
        if (individualSearchType == 2) {
            $("#inputBeneficiaryName").data("Object", holder);
            $("#inputBeneficiaryName").val(holder.Name + ' (' + holder.IdentificationDocument.Number + ')');
        }
        else {
            $("#inputHolder_").data("Object", holder);
            if (holder.IdentificationDocument != null) {
                personID = holder.IdentificationDocument.Number;
                $("#inputHolder_").val(holder.Name + ' (' + holder.IdentificationDocument.Number + ')');
            }

        }
        InsuredindividualID = holder.IndividualId;

        if (holder.CustomerType == CustomerType.Individual) {
            Underwriting.GetIndividualDetailsByIndividualId(holder.IndividualId, holder.CustomerType);
            $("#btnConvertProspect").hide();
        }
        else if (glbPolicy.TemporalType != TemporalType.Quotation) {
            $("#btnConvertProspect").show();
        }

    }

    static LoadTemporal(policyData) {
        if (policyData.TemporalType == TemporalType.Quotation) {
            $("#inputQuotation").val(policyData.Endorsement.QuotationId);
        }
        else {
            $("#inputTemporal").val(policyData.Id);
        }
        Underwriting.LoadHolder(policyData.Holder);
        agentSearchType = 1;
        Underwriting.LoadAgencyPrincipal(policyData.Agencies);
        Underwriting.GetBranches(policyData.Branch.Id);
        Underwriting.GetJustificationSarlaft(policyData.JustificationSarlaft);
        Underwriting.LoadTicket(policyData.Endorsement);
        Underwriting.LoadCalculateMinimumPremium(policyData.CalculateMinPremium);
        if (policyData.Branch.SalePoints != null && policyData.Branch.SalePoints.length > 0) {
            Underwriting.GetSalePointsByBranchId(policyData.Branch.Id, policyData.Branch.SalePoints[0].Id);
        }
        else {
            Underwriting.GetSalePointsByBranchId(policyData.Branch.Id, 0);
        }
        Underwriting.GetPrefixesByAgentId($("#inputAgentPrincipal").data("Object").Agent.IndividualId, policyData.Prefix.Id);
        Underwriting.GetProductsByAgentIdPrefixId($("#inputAgentPrincipal").data("Object").Agent.IndividualId, policyData.Prefix.Id, policyData.Product.Id);
        Underwriting.GetPolicyTypesByProductId(policyData.Product.Id, policyData.PolicyType.Id);
        $("#inputFrom").UifDatepicker('setValue', FormatDate(policyData.CurrentFrom));
        $("#inputTo").UifDatepicker('setValue', FormatDate(policyData.CurrentTo));
        Underwriting.GetCurrenciesByProductId(policyData.Product.Id, policyData.ExchangeRate.Currency.Id);
        Underwriting.CalculateDays();
        $("#selectHour").val(policyData.TimeHour);
        $("#inputChange").val(FormatMoney(policyData.ExchangeRate.SellAmount));
        if (policyData.ExchangeRate.SellAmount == 1) {
            $("#inputChange").prop('disabled', true);
        }
        else {
            $("#inputChange").prop('disabled', false);
        }
        if (policyData.BillingGroup != null && policyData.BillingGroup.Id > 0) {
            Underwriting.GetBillingGroupByDescription(policyData.BillingGroup.Id);
        }
        if (policyData.Request != null && policyData.Request.Id > 0) {
            Underwriting.GetCoRequestByDescription(policyData.Request.Id, false);
            Underwriting.DisabledControlRequest(true);
        }

        dynamicProperties = policyData.DynamicProperties;
        Underwriting.LoadSummary(policyData.Summary);

        Underwriting.LoadTitle(policyData);
        Underwriting.LoadSubTitles(0);
        Underwriting.DisabledControlByEndorsementType(policyData.Endorsement.EndorsementType, policyData.Prefix.Id, true);

        if (glbPolicy.Endorsement != null) {
            if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Renewal) {
                $('#btnIssuePolicy').text(AppResources.PolicyRenovate);
            }
            else {
                $('#btnIssuePolicy').text(AppResources.IssuePolicy);
            }
        }
        else {
            $('#btnIssuePolicy').text(AppResources.IssuePolicy);
        }
    }

    
}
