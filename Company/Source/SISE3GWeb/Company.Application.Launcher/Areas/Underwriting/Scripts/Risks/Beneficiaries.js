//Codigo de la pagina Beneficiaries.cshtml
var beneficiaryIndex = null;
var result = null;
var participation = null;

class RiskBeneficiary extends Uif2.Page {
    getInitialState() {
        $('#inputBeneficiaryName').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $('#inputBeneficiaryParticipation').OnlyDecimals(UnderwritingDecimal);
        $('#listBeneficiaries').UifListView({ displayTemplate: '#beneficiaryTemplate', add: false, edit: true, delete: true, customEdit: true, deleteCallback: RiskBeneficiary.DeleteBeneficiary, height: 300 });
        RiskBeneficiary.GetBeneficiaryTypes();
        //$("#inputBeneficiaryName").data("Object", $('#inputInsured').data("Object"));
        //$("#inputBeneficiaryName").data("Detail", $('#inputInsured').data("Detail"));
        //$("#inputBeneficiaryName").val($('#inputInsured').data("Object").Name + ' (' + $('#inputInsured').data("Object").IdentificationDocument.Number + ')');
    }

    bindEvents() {
        $('#btnBeneficiary').on('click', RiskBeneficiary.BeneficiaryLoad);
        $('input[type=radio][name=chkBeneficiaryRol]').change(RiskBeneficiary.BeneficiaryRol);
        $('#inputBeneficiaryName').on('buttonClick', RiskBeneficiary.SearchBeneficiary);
        $('#btnDetailBeneficiary').click(RiskBeneficiary.ShowBeneficiaryDetail);
        $('#inputBeneficiaryParticipation').focusin;
        $('#inputBeneficiaryParticipation').focusout;
        $('#btnBeneficiaryCancel').on('click', RiskBeneficiary.ClearBeneficiary);
        $('#btnBeneficiaryAccept').on('click', RiskBeneficiary.AddBeneficiary);
        $('#listBeneficiaries').on('rowEdit', RiskBeneficiary.BeneficiaryEdit);
        $('#btnBeneficiaryClose').on('click', RiskBeneficiary.BeneficiaryClose);
        $('#btnBeneficiarySave').on('click', RiskBeneficiary.BeneficiarySave);
    }

    static BeneficiaryLoad() {
        individualSearchType = 2;
        RiskBeneficiary.LoadBeneficiary();

    }

    static BeneficiaryRol() {
        RiskBeneficiary.ClearBeneficiary();
        $('#chkBeneficiaryHolder').prop('checked', false);
        $('#chkBeneficiaryInsured').prop('checked', false);
        $('#chkBeneficiaryOther').prop('checked', false);

        if (this.value == 'holder')
        {
            $('#chkBeneficiaryHolder').prop('checked', true);
            RiskBeneficiary.GetBeneficiariesByDescription(glbPolicy.Holder.IndividualId, InsuredSearchType.IndividualId, glbPolicy.Holder.CustomerType);
            
        }
        else if (this.value == 'insured') {
            $('#chkBeneficiaryInsured').prop('checked', true);
            $("#inputBeneficiaryName").data("Object", $('#inputInsured').data("Object"));
            $("#inputBeneficiaryName").data("Detail", $('#inputInsured').data("Detail"));
            $("#inputBeneficiaryName").val($('#inputInsured').data("Object").Name + ' (' + $('#inputInsured').data("Object").IdentificationDocument.Number + ')');
        }
        else if (this.value == 'other') {
            $('#chkBeneficiaryOther').prop('checked', true);
        }
    }

    static SearchBeneficiary() {
        searchHolderTo = 'inputBeneficiaryName';
        if ($('#inputBeneficiaryName').val().trim().length > 0)
        {
                var typeSearch = $('input:radio[name=chkBeneficiaryRol]:checked').val();
                if (typeSearch == "insured") {
                    if (glbRisk.Class == undefined) {
                        window[glbRisk.Object].GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputBeneficiaryName").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual);
                    }
                    else {
                        glbRisk.Class.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputBeneficiaryName").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual);
                    }
                }
                else
                    if (typeSearch == "holder")
                    {
                    Underwriting.GetHoldersByDescriptionInsuredSearchTypeCustomerType($("#inputBeneficiaryName").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual);
                     }
                    else
                    {
                    RiskBeneficiary.GetBeneficiariesByDescription($('#inputBeneficiaryName').val().trim(), InsuredSearchType.DocumentNumber);
                      }
            }
        }
    

    static BeneficiaryEdit(event, data, index) {
        beneficiaryIndex = index;
        RiskBeneficiary.EditBeneficiary(data);
    }

    static BeneficiaryClose() {
        individualSearchType = 1;
        $("#modalBeneficiaries").UifModal('hide');
    }

    static BeneficiarySave() {
        individualSearchType = 1;
        RiskBeneficiary.SaveBeneficiaries();
    }

    static ShowBeneficiary() {
        if ($("#inputBeneficiaryName").data("Object") != undefined) {
            RiskBeneficiary.ShowBeneficiaryDetail($("#inputBeneficiaryName").data("Object"));
        }
    }

    static LoadBeneficiary() {

        $("#mainRisk").validate();
        if (glbRisk.Id == 0) {
            if (glbRisk.Class == undefined) {
                window[glbRisk.Object].SaveRisk(MenuType.Beneficiaries, 0);
            }
            else {
                glbRisk.Class.SaveRisk(MenuType.Beneficiaries, 0);
            }

        }

        if (glbRisk.Id > 0) {
            if (glbRisk.Class == undefined) {
                window[glbRisk.Object].ShowPanelsRisk(MenuType.Beneficiaries);
            }
            else {
                glbRisk.Class.ShowPanelsRisk(MenuType.Beneficiaries);
            }

            $("#formBeneficiaries").formReset();
            $("#listBeneficiaries").UifListView({ source: null, displayTemplate: "#beneficiaryTemplate", add: false, edit: true, delete: true, customEdit: true, deleteCallback: RiskBeneficiary.DeleteBeneficiary, height: 300 });
            RiskBeneficiary.ClearBeneficiary();

            if (glbRisk.Beneficiaries != null) {
                RiskBeneficiary.LoadBeneficiaries(glbRisk.Beneficiaries);
            }
        }
    }

    static LoadBeneficiaries(beneficiaries) {
        $.each(beneficiaries, function (key, value) {
            //this.Participation = this.Participation;
            $('#listBeneficiaries').UifListView('addItem', this);
        })
        $('#labelTotalParticipation').text(RiskBeneficiary.GetBeneficiaryPercentageTotal(null));
    }

    static GetBeneficiariesByDescription(description, insuredSearchType, customerType) {
        customerType = customerType || 1;
        var number = parseInt(description, 10);

        if ((!isNaN(number) || description.length > 2) && (description != 0)) {
            BeneficiariesRequest.GetBeneficiariesByDescription(description, insuredSearchType, customerType).done(function (data) {
              
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputBeneficiaryName').data('Object', data.result[0]);
                        
                        if (glbRisk.Class == undefined) {
                            $('#inputBeneficiaryName').data('Detail', window[glbRisk.Object].GetIndividualDetails(window[glbRisk.Object].GetIndividualDetailsByIndividualId(data.result[0].IndividualId, data.result[0].CustomerType)));
                        }
                        else {
                            $('#inputBeneficiaryName').data('Detail', glbRisk.Class.GetIndividualDetails(glbRisk.Class.GetIndividualDetailsByIndividualId(data.result[0].IndividualId, data.result[0].CustomerType)));
                        }
                        $('#inputBeneficiaryName').val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                    }
                    else if (data.result.length > 1) {
                        var dataList = { dataObject: [] };

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.dataObject.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].CodeBeneficiary,
                                DocumentNum: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: "Individuo",
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                            });
                        }
                        if (glbRisk.Class == undefined) {
                            window[glbRisk.Object].ShowModalList(dataList.dataObject);
                        }
                        else {
                            glbRisk.Class.ShowModalList(dataList.dataObject);
                        }
                        $('#modalIndividualSearch').UifModal('showLocal', AppResources.LabelBeneficiary);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchBeneficiaries, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchBeneficiaries, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchBeneficiaries, 'autoclose': true });
            });
        }
    }

    static CalculateBeneficiaryPercentage(individualId) {
        var percentage = parseFloat($('#inputBeneficiaryParticipation').val().replace(separatorDecimal, separatorThousands));
        var percentageTotal = RiskBeneficiary.GetBeneficiaryPercentageTotal(individualId);

        if ((percentage + percentageTotal) > 100 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        else {
            return true;
        }
    }

    static AddBeneficiary() {

        var beneficiary = $('#inputBeneficiaryName').data('Object');
        $('#formBeneficiaries').validate();
        
        if ($('#formBeneficiaries').valid()) {
            if (RiskBeneficiary.ValidBeneficiary()) {
                var beneficiaryAdd = {
                    BeneficiaryType: { Id: $('#selectBeneficiaryType').UifSelect('getSelected') },
                    BeneficiaryTypeDescription: $('#selectBeneficiaryType').UifSelect('getSelectedText'),
                    CustomerType: beneficiary.CustomerType,
                    IndividualId: beneficiary.IndividualId,
                    IndividualType: glbPolicy.Holder.IndividualType,
                    Name: beneficiary.Name,
                    Participation: $('#inputBeneficiaryParticipation').val(),
                    IdentificationDocument: beneficiary.IdentificationDocument,
                    CompanyName: beneficiary.CompanyName
                };

                var percentageList = RiskBeneficiary.GetBeneficiaryPercentageTotal();

                if (beneficiaryIndex == null) {
                    
                    if ((parseInt(percentageList) + parseInt(beneficiaryAdd.Participation)) > 100) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
                    } else {
                        $('#listBeneficiaries').UifListView('addItem', beneficiaryAdd);
                    }
                }
                else {
                    if (((parseInt(percentageList) - parseInt(participation)) + parseInt(beneficiaryAdd.Participation)) > 100) {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
                    } else {
                        $('#listBeneficiaries').UifListView('editItem', beneficiaryIndex, beneficiaryAdd);
                    }
                }
                
                $('#labelTotalParticipation').text(RiskBeneficiary.GetBeneficiaryPercentageTotal(null));
                RiskBeneficiary.ClearBeneficiary();
            }
        }
        
    }
    static ValidBeneficiary() {
        if ($('#inputBeneficiaryName').data('Object') == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorBeneficiary, 'autoclose': true });
            return false;
        }
        else {
            var details = $('#inputBeneficiaryName').data('Detail');
            if (Array.isArray(details))
            {
                if (details.length > 0) {
                    if (RiskBeneficiary.ValidateTradeNameDetails(details[0])) {
                        return false;
                    }
                    if (beneficiaryIndex == null) {
                        var exists = false;
                        $.each($('#listBeneficiaries').UifListView('getData'), function (index, value) {
                            if (this.IndividualId == $('#inputBeneficiaryName').data('Object').IndividualId) {
                                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateBeneficiary, 'autoclose': true });
                                exists = true;
                            }
                        });
                        if (exists) {
                            return false;
                        }
                    }
                    if (RiskBeneficiary.CalculateBeneficiaryPercentage($('#inputBeneficiaryName').data('Object').IndividualId) == false) {
                        return false;
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorBeneficiaryCorrespondenceAddress, 'autoclose': true });
                    return false;
                }
            }
          
        }
        return true;
    }

    static EditBeneficiary(beneficiary) {

        $('#inputBeneficiaryName').data('Object', beneficiary);
        if (glbRisk.Class == undefined) {
            $('#inputBeneficiaryName').data('Detail', window[glbRisk.Object].GetIndividualDetails(window[glbRisk.Object].GetIndividualDetailsByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType)));
        }
        else {
            $('#inputBeneficiaryName').data('Detail', glbRisk.Class.GetIndividualDetails(glbRisk.Class.GetIndividualDetailsByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType)));
        }
        $('#inputBeneficiaryName').val(beneficiary.Name + ' (' + beneficiary.IdentificationDocument.Number + ')');
        $('#selectBeneficiaryType').UifSelect('setSelected', beneficiary.BeneficiaryType.Id);
        $('#inputBeneficiaryParticipation').val(beneficiary.Participation);
        participation = beneficiary.Participation;
        //RiskVehicle.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(beneficiary.IdentificationDocument.Number, 2, null)
        glbRisk.Class.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(beneficiary.IdentificationDocument.Number, InsuredSearchType.DocumentNumber, CustomerType.Individual);



    }

    static DeleteBeneficiary(deferred, data) {
        deferred.resolve();
        RiskBeneficiary.ClearBeneficiary();
        $('#labelTotalParticipation').text(RiskBeneficiary.GetBeneficiaryPercentageTotal(data.IndividualId));
    }

    static ClearBeneficiary() {
        
        beneficiaryIndex = null;

        $('#inputBeneficiaryName').data('Object', null);
        $('#inputBeneficiaryName').data('Detail', null);
        $('#inputBeneficiaryName').val('');
        $('#selectBeneficiaryType').UifSelect('setSelected', null);
        $('#inputBeneficiaryParticipation').val('');
        $('#formBeneficiaries').formReset();

    }

    static GetBeneficiaryPercentageTotal(individualId) {
        var percentage = 0;
        $.each($('#listBeneficiaries').UifListView('getData'), function (key, value) {
            if ((beneficiaryIndex == null && individualId == null) || (individualId != this.IndividualId)) {
                percentage += parseFloat(this.Participation);
            }
        });

        return percentage;
    }
    

    static SaveBeneficiaries() {

        var percentage = RiskBeneficiary.GetBeneficiaryPercentageTotal();
        var keyPercentage = false;
        if (glbPolicy.Prefix.Id == PrefixType.RESPONSABILIDA_CIVIL) {
            if (percentage > 0 && percentage <= 100)
                keyPercentage = true;
        }
        else {
            if (percentage == 100)
                keyPercentage = true;
        }


        if (keyPercentage) {
            BeneficiariesRequest.SaveBeneficiariesRisk(glbRisk.Id, $("#listBeneficiaries").UifListView('getData'), riskController).done(function (data) {
                if (data.success) {
                    if (glbRisk.Class == undefined) {
                        window[glbRisk.Object].ShowPanelsRisk(MenuType.Risk);
                        glbRisk.Beneficiaries = data.result;
                        window[glbRisk.Object].LoadSubTitles(1);
                    }
                    else {
                        glbRisk.Class.ShowPanelsRisk(MenuType.Risk);
                        glbRisk.Beneficiaries = data.result;
                        glbRisk.Class.LoadSubTitles(1);
                    }


                    $("#modalBeneficiaries").UifModal('hide');
                }
                else {
                    $.UifNotify('show', { 'type': 'error', 'message': AppResources.ErrorSaveBeneficiaries, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'error', 'message': AppResources.ErrorSaveBeneficiaries, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
        }
    }

    static ShowBeneficiaryDetail() {
        $('#tableIndividualDetails').UifDataTable('clear');
        $("#inputBeneficiaryName").data("Detail", RiskBeneficiary.GetIndividualDetailsBeneficiary($("#inputBeneficiaryName").data("Object").CompanyName));
        if ($('#inputBeneficiaryName').data('Detail') != null) {
            var beneficiaryDetails = $('#inputBeneficiaryName').data('Detail');
            var arrayBeneficiaryDetails = [];
            arrayBeneficiaryDetails.push(beneficiaryDetails);
            if (arrayBeneficiaryDetails.length > 0) {
                $.each(arrayBeneficiaryDetails, function (id, item) {
                    $('#tableIndividualDetails').UifDataTable('addRow', item)
                    if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum > 0 && $("#inputBeneficiaryName").data("Object").CompanyName.NameNum == this.NameNum) {
                        $('#tableIndividualDetails tbody tr:eq(' + id + ')').removeClass('row-selected').addClass('row-selected');
                        $('#tableIndividualDetails tbody tr:eq(' + id + ') td button span').removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');
                    }
                    else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && this.IsMain == true) {
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

        $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelBeneficiaryDetail);
    }

    static GetIndividualDetailsBeneficiary(individualDetails) {
        if (individualDetails != undefined && individualDetails != null) {
            if ($.type(individualDetails) === "array") {
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
                        }
                        else if ($("#inputInsured").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputInsured").data("Object").CompanyName = this;
                        }
                    }
                    else if (individualSearchType == 2) {
                        if ($("#inputBeneficiaryName").data("Object") != null && $("#inputBeneficiaryName").data("Object").CompanyName == null) {
                            if (this.IsMain) {
                                $("#inputBeneficiaryName").data("Object").CompanyName = this;
                            }
                        }
                        else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = this;
                        }
                    }
                    else if (individualSearchType == 3) {
                        if ($("#inputSecure").data("Object") != null && $("#inputSecure").data("Object").CompanyName == null) {
                            if (this.IsMain) {
                                $("#inputSecure").data("Object").CompanyName = this;
                            }
                        }
                        else if ($("#inputSecure").data("Object") != null && $("#inputSecure").data("Object").CompanyName.NameNum == 0 && this.IsMain) {
                            $("#inputSecure").data("Object").CompanyName = this;
                        }
                    }
                });
            }
            else {
                if (individualDetails.Address != null) {
                    individualDetails.Detail = individualDetails.Address.Description;
                }
                if (individualDetails.TradeName != null) {
                    individualDetails.Detail = '<b>' + individualDetails.TradeName + '</b>' + '<br/>' + individualDetails.Detail;
                }
                if (individualDetails.Phone != null) {
                    individualDetails.Detail += '<br/>' + individualDetails.Phone.Description;
                }
                if (individualDetails.Email != null) {
                    individualDetails.Detail += '<br/>' + individualDetails.Email.Description;
                }
                if (individualSearchType == 1) {
                    if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName == null) {
                        if (individualDetails.IsMain) {
                            $("#inputInsured").data("Object").CompanyName = individualDetails;
                        }
                    }
                    else if ($("#inputInsured").data("Object") != null && $("#inputInsured").data("Object").CompanyName.NameNum == 0 && individualDetails.IsMain) {
                        $("#inputInsured").data("Object").CompanyName = individualDetails;
                    }
                }
                else if (individualSearchType == 2) {
                    if ($("#inputBeneficiaryName").data("Object") != null && $("#inputBeneficiaryName").data("Object").CompanyName == null) {
                        if (individualDetails.IsMain) {
                            $("#inputBeneficiaryName").data("Object").CompanyName = individualDetails;
                        }
                    }
                    else if ($("#inputBeneficiaryName").data("Object").CompanyName.NameNum == 0 && individualDetails.IsMain) {
                        $("#inputBeneficiaryName").data("Object").CompanyName = individualDetails;
                    }
                }
                else if (individualSearchType == 3) {
                    if ($("#inputSecure").data("Object").CompanyName == null) {
                        if (individualDetails.IsMain) {
                            $("#inputSecure").data("Object").CompanyName = individualDetails;
                        }
                    }
                    else if ($("#inputSecure").data("Object").CompanyName.NameNum == 0 && individualDetails.IsMain) {
                        $("#inputSecure").data("Object").CompanyName = individualDetails;
                    }
                }
            }
        }

        return individualDetails;
    }

    static GetBeneficiaryTypes() {
        BeneficiariesRequest.GetBeneficiaryTypes().done(function (data) {
            if (data.success) {
                $("#selectBeneficiaryType").UifSelect({ sourceData: data.result });
            }
        });
    }
    static ValidateTradeNameDetails(Details) {
        var TradeNameDetails = AppResources.ErrorBeneficiaryNo;
        if (Details != null) {
            if (Details.Address == null) {
                TradeNameDetails += AppResources.Address + ", ";
            }
            if (Details.Phone == null) {
                TradeNameDetails += AppResources.Phone + ", ";
            }
            if (TradeNameDetails != AppResources.ErrorBeneficiaryNo) {
                $("#inputBeneficiaryName").val('');
                $.UifNotify('show', { 'type': 'danger', 'message': TradeNameDetails, 'autoclose': true });
                return true;
            }
        }
    }
    static ValidateSarlaftBeneficiary(individualId) {

        if (glbPolicy.TemporalType == TemporalType.Policy) {
            var result = false;
            if (glbPolicy.JustificationSarlaft != null)
                return true;
            if (glbPolicy.Holder.IndividualId != individualId)
            {
                UnderwritingRequest.ValidateGetSarlaft(individualId).done(function (data) {
                    if (data.success) {

                        switch (data.result) {
                            case SarlaftValidationState.NOT_EXISTS:
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExistsBeneficiary, 'autoclose': true });
                                result = false;
                                break;
                            case SarlaftValidationState.EXPIRED:
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftExpiredBeneficiary, 'autoclose': true });
                                result = false;
                                break;
                            case SarlaftValidationState.OVERCOME:
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftOvercomeBeneficiary, 'autoclose': true });
                                result = false;
                                break;
                            case SarlaftValidationState.ACCURATE:
                                result = true;
                                break;
                            case SarlaftValidationState.PENDING:
                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ValidateSarlaftPendingBeneficiary, 'autoclose': true });
                                result = false;
                                break;
                            default:
                                result = false;
                                break;
                        }

                        return result;
                    }
                    else {
                        return false;
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    return false;
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorValidateSarlaft, 'autoclose': true });
                });
            }
            else
                return true;
            return result;
        }
        else {
            return true;
        }
    }
}