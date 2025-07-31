//Codigo de la pagina Beneficiaries.cshtml
var beneficiaryIndex = null;
class UnderwritingBeneficiaries extends Uif2.Page {
    getInitialState() {
        $('#inputBeneficiaryName').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        $('#inputBeneficiaryParticipation').OnlyDecimals(UnderwritingDecimal);
        $('#listBeneficiaries').UifListView({ displayTemplate: '#beneficiaryTemplate', add: false, edit: true, delete: true, customEdit: true, deleteCallback: UnderwritingBeneficiaries.DeleteBeneficiary, height: 200 });
        UnderwritingBeneficiaries.GetBeneficiaryTypes();
    }

    bindEvents() {        
        $('#btnBeneficiaries').on('click', this.BeneficiariesLoad);
        $('input[type=radio][name=chkBeneficiaryRol]').change(this.ChangeBeneficiaryRol);
        $('#inputBeneficiaryName').on('buttonClick', this.SearchBeneficiary);
        $('#btnDetailBeneficiary').click(UnderwritingBeneficiaries.ShowBeneficiaryDetail);
        $('#inputBeneficiaryParticipation').focusin;
        $('#inputBeneficiaryParticipation').focusout;
        $('#btnBeneficiaryCancel').on('click', UnderwritingBeneficiaries.ClearBeneficiary);
        $('#btnBeneficiaryAccept').on('click', UnderwritingBeneficiaries.AddBeneficiary);
        $('#listBeneficiaries').on('rowEdit', this.BeneficiaryEdit);
        $('#btnBeneficiarySave').on('click', UnderwritingBeneficiaries.SaveBeneficiaries);
    }

    BeneficiariesLoad() {
        if (glbPolicy.Id == 0) {
            Underwriting.SaveTemporalPartial(MenuType.Beneficiaries);                
        }
        else {
            UnderwritingBeneficiaries.LoadBeneficiaries();
        }
    }

    ChangeBeneficiaryRol() {
        UnderwritingBeneficiaries.ClearBeneficiary();
        $('#chkBeneficiaryHolder').prop('checked', false);
        $('#chkBeneficiaryInsured').prop('checked', false);
        $('#chkBeneficiaryOther').prop('checked', false);

        if (this.value == 'holder') {
            $('#chkBeneficiaryHolder').prop('checked', true);
            $('#inputBeneficiaryName').data('Object', $('#inputHolder').data('Object'));
            $('#inputBeneficiaryName').data('Detail', $('#inputHolder').data('Detail'));
            $('#inputBeneficiaryName').val($('#inputHolder').data('Object').Name + ' (' + $('#inputHolder').data('Object').IdentificationDocument.Number + ')');
        }
        else if (this.value == 'insured') {
            $('#chkBeneficiaryInsured').prop('checked', true);
        }
        else if (this.value == 'other') {
            $('#chkBeneficiaryOther').prop('checked', true);
        }
    }

    SearchBeneficiary() {
        if ($('#inputBeneficiaryName').val().trim().length > 0) {
            UnderwritingBeneficiaries.GetBeneficiariesByDescription($('#inputBeneficiaryName').val().trim(), InsuredSearchType.DocumentNumber);
        }
    }

    BeneficiaryEdit(event, data, index) {
        beneficiaryIndex = index;
        UnderwritingBeneficiaries.EditBeneficiary(data);
    }

    static LoadBeneficiaries() {

        $('#formUnderwriting').validate();

        if (glbPolicy.Id > 0 && $('#formUnderwriting').valid()) {
            $('#formBeneficiaries').formReset();
            Underwriting.ShowPanelsIssuance(MenuType.Beneficiaries);
            $('#listBeneficiaries').UifListView({ source: null, displayTemplate: '#beneficiaryTemplate', add: false, edit: true, delete: true, customEdit: true, deleteCallback: UnderwritingBeneficiaries.DeleteBeneficiary, height: 200 });
            UnderwritingBeneficiaries.ClearBeneficiary();

            if (glbPolicy.DefaultBeneficiaries != null) {
                UnderwritingBeneficiaries.LoadDefaultBeneficiaries(glbPolicy.DefaultBeneficiaries);
            }
        }
    }

    static LoadDefaultBeneficiaries(beneficiaries) {
        $.each(beneficiaries, function (key, value) {
            //this.Participation = this.Participation;
            $('#listBeneficiaries').UifListView('addItem', this);
        })
        $('#labelTotalParticipation').text(UnderwritingBeneficiaries.GetBeneficiaryPercentageTotal(null));
    }

    static GetBeneficiariesByDescription(description, insuredSearchType) {

        var number = parseInt(description, 10);

        if (!isNaN(number) || description.length > 2) {
            BeneficiariesRequest.GetBeneficiariesByDescription(description, insuredSearchType).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputBeneficiaryName').data('Object', data.result[0]);
                        $('#inputBeneficiaryName').data('Detail', Underwriting.GetIndividualDetails(data.result[0]));
                        $('#inputBeneficiaryName').val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                    }
                    else if (data.result.length > 1) {
                        modalListType = 2;
                        var dataList = { dataObject: [] };

                        for (var i = 0; i < data.result.length; i++) {
                            dataList.dataObject.push({
                                Id: data.result[i].IndividualId,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                Type: "Persona"
                            });
                        }
                        Underwriting.ShowModalList(dataList.dataObject);
                        $('#modalDialogList').UifModal('showLocal', AppResources.LabelBeneficiary);
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
        var percentageTotal = UnderwritingBeneficiaries.GetBeneficiaryPercentageTotal(individualId);

        if ((percentage + percentageTotal) > 100 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        else {
            return true;
        }
    }

    static AddBeneficiary() {

        $('#formBeneficiaries').validate();
        if ($('#formBeneficiaries').valid()) {
            if (UnderwritingBeneficiaries.ValidBeneficiary()) {
                var beneficiary = $('#inputBeneficiaryName').data('Object');
                var beneficiaryAdd = {
                    Address: beneficiary.Address,
                    BeneficiaryType: $('#selectBeneficiaryType').UifSelect('getSelected'),
                    BeneficiaryTypeDescription: $('#selectBeneficiaryType').UifSelect('getSelectedText'),
                    CustomerType: beneficiary.CustomerType,
                    DetailId: beneficiary.DetailId,
                    Email: beneficiary.Email,
                    IndividualId: beneficiary.IndividualId,
                    Name: beneficiary.Name,
                    Participation: $('#inputBeneficiaryParticipation').val(),
                    Phone: beneficiary.Phone,
                    IdentificationDocument: beneficiary.IdentificationDocument
                };
                if (beneficiaryIndex == null) {
                    $('#listBeneficiaries').UifListView('addItem', beneficiaryAdd);
                }
                else {
                    $('#listBeneficiaries').UifListView('editItem', beneficiaryIndex, beneficiaryAdd);
                }

                UnderwritingBeneficiaries.ClearBeneficiary();
                $('#labelTotalParticipation').text(UnderwritingBeneficiaries.GetBeneficiaryPercentageTotal(null));
            }
        }
    }

    static ValidBeneficiary() {
        if ($('#inputBeneficiaryName').data('Object') == null) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorBeneficiary, 'autoclose': true });
            return false;
        }
        else {
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
            if (UnderwritingBeneficiaries.CalculateBeneficiaryPercentage($('#inputBeneficiaryName').data('Object').IndividualId) == false) {
                return false;
            }
        }
        return true;
    }

    static EditBeneficiary(beneficiary) {
        $('#inputBeneficiaryName').data('Object', beneficiary);
        $('#inputBeneficiaryName').data('Detail', Underwriting.GetIndividualDetails(beneficiary));
        $('#inputBeneficiaryName').val(beneficiary.Name + ' (' + beneficiary.IdentificationDocument.Number + ')');
        $('#selectBeneficiaryType').UifSelect('setSelected', beneficiary.BeneficiaryType);
        $('#inputBeneficiaryParticipation').val(beneficiary.Participation);
    }

    static DeleteBeneficiary(deferred, data) {
        deferred.resolve();
        UnderwritingBeneficiaries.ClearBeneficiary();
        $('#labelTotalParticipation').text(UnderwritingBeneficiaries.GetBeneficiaryPercentageTotal(data.IndividualId));
    }

    static ClearBeneficiary() {
        $('#formBeneficiaries').formReset();
        beneficiaryIndex = null;

        $('#inputBeneficiaryName').data('Object', null);
        $('#inputBeneficiaryName').data('Detail', null);
        $('#inputBeneficiaryName').val('');
        $('#selectBeneficiaryType').UifSelect('setSelected', null);
        $('#inputBeneficiaryParticipation').val('');
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

        var percentage = UnderwritingBeneficiaries.GetBeneficiaryPercentageTotal(null);

        if (percentage == 100) {
            BeneficiariesRequest.SaveBeneficiaries(glbPolicy.Id, $('#listBeneficiaries').UifListView('getData')).done(function (data) {
                if (data.success) {
                    Underwriting.HidePanelsIssuance(MenuType.Beneficiaries);
                    glbPolicy.DefaultBeneficiaries = data.result;
                    Underwriting.LoadSubTitles(7);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveBeneficiaries, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveBeneficiaries, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageValidateParticipationPercentage, 'autoclose': true });
        }
    }

    static ShowBeneficiaryDetail() {
        detailType = 2;
        $('#tableIndividualDetails').UifDataTable('clear');

        if ($('#inputBeneficiaryName').data('Detail') != null) {
            var beneficiaryDetails = $('#inputBeneficiaryName').data('Detail');

            if (beneficiaryDetails.length > 0) {
                $.each(beneficiaryDetails, function (id, item) {
                    $('#tableIndividualDetails').UifDataTable('addRow', item)
                    if (this.NameNum == $('#inputBeneficiaryName').data('Object').DetailId) {
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

    static GetBeneficiaryTypes() {
        BeneficiariesRequest.GetBeneficiaryTypes().done(function (data) {
            if (data.success) {                
                $("#selectBeneficiaryType").UifSelect({ sourceData: data.result });                
            }
        });
    }
    
}