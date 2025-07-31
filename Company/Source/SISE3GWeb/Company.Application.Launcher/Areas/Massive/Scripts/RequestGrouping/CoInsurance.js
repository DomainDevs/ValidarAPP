var coInsureType = "";
var coInsureTypeId;
var CoInsAssignedIndex;

$(() => {
    new CoInsurance();
    $("#inputRequest").data("coInsurance", null);
});

class CoInsurance extends Uif2.Page {

    getInitialState() {
        coInsureTypeId = 1;
        CoInsAssignedIndex = null;
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
        $('#inputCoInsAcceptedLeadingInsurer').ValidatorKey(3, 1, 1);
        $('#inputCoInsAcceptedParticipation').OnlyDecimals(2);
        $('#inputCoInsAcceptedParticipationTotal').OnlyDecimals(2);
        $('#inputCoInsAcceptedExpenses').OnlyPercentage();
        $('#inputCoInsAcceptedLeadingPolicy').ValidatorKey(6, 1, 1);
        $('#inputCoInsAcceptedEndorsement').ValidatorKey(6, 1, 1);
        $('#inputCoInsAcceptedLeadingInsurer').ValidatorKey(3, 1, 1);
        $('#inputCoInsAssignedParticipation').OnlyPercentage();
        $('#inputCoInsAssignedExpenses').OnlyPercentage();
        CoInsurance.ClearPanelAccepted();
        CoInsurance.ClearPanelAssigned();
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
        CoInsurance.CoinsuredAssingList();

        this.coInsuranceType = $("#selectedBusinessType");
    }

    bindEvents() {
        
        $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $('#selectCoInsBusinessType').on('itemSelected', (event, selectedItem) => { this.businessTypeSelected(selectedItem);});
        $('#inputCoInsAcceptedLeadingInsurer').on('itemSelected', (event, selectedItem) => { this.acceptedLeadingInsured(selectedItem); });
        $('#inputCoInsAcceptedParticipation').focusout(() => { this.CalculateAcceptedPercentage(); });
        $('#inputCoInsAssignedInsured').on('itemSelected', (event, selectedItem) => { this.setAssignedInsured(selectedItem); });
        $('#btnCoInsAssignedAccept').on("click", () => { this.assignAccepted(); });
        $("#btnCoInsuranceClose").on("click", () => { RequestGrouping.HidePanels(MenuTypeGrouping.CoInsurance); });
        $("#btnCoInsuranceSave").on("click", () => { this.coInsuranceSave(); });
    }
    coInsuranceSave()
    {
        if (coInsureTypeId == BusinessType.Accepted) {
            var percentageOrigin = parseFloat($('#inputCoInsAcceptedParticipationTotal').val().replace(separatorDecimal, separatorThousands));
            var percentage = parseFloat($('#inputCoInsAcceptedParticipation').val().replace(separatorDecimal, separatorThousands));
            if (percentageOrigin + percentage > 100) {
                $('#inputCoInsAcceptedLeadingPolicy').ValidatorKey(6, 1, 1);
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateMajorPercentage, 'autoclose': true });
                return false;
            }
        }
        CoInsurance.SaveCoInsurance();

        this.coInsuranceType.text("(" + coInsureType + ")");
    }

    assignAccepted ()
    {
        if ($("#inputCoInsAssignedParticipation").val() >= 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateCoInsAssignedpercentage, 'autoclose': true });
            return false;
        }
        if (CoInsAssignedIndex == null) {
            if (!CoInsurance.ValidateCoInsuranceAssigned($('#inputCoInsAssignedInsured').data('Id'))) {
                if (CoInsurance.CalculateAssingPercentage(false, -1)) {
                    CoInsurance.addItemCoInsAssignedExpenses();
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateCoInsurance, 'autoclose': true });
            }
        }
        else {
            if (CoInsurance.CalculateAssingPercentage(false, CoInsAssignedIndex)) {
                $("#listCoInsuranceAssigned").UifListView("editItem", CoInsAssignedIndex, CoInsurance.GetCoInsAssignedExpenses());
                var total = CoInsurance.CalculateCoInsuranceAssignedTotalAmount();
                CoInsurance.ClearAssignedInsured();
            }
        }
    }

    acceptedLeadingInsured(selectedItem)
    {
        $("#alert").hide();
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', selectedItem.Id);
    }

    setAssignedInsured(selectedItem)
    {
        $('#inputCoInsAssignedInsured').data('Id', selectedItem.Id);
    }

    businessTypeSelected(selectedItem)
    {
        coInsureType = selectedItem.Text;
        coInsureTypeId = selectedItem.Id;
        this.ShowPanelBusinessType(selectedItem.Id);
        $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
    }

    static LoadCoinsurance () {
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
        switch (glbRequest.BusinessType) {
            case BusinessType.Accepted:
                $('#panelAccepted').show();
                CoInsurance.LoadCoinsuranceAccepted();
                break;
            case BusinessType.Assigned:
                $('.panelAssigned').show();
                $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
                CoInsurance.LoadCoinsuranceAssigned();
                break;
            default:
                break;
        }
    }

    static LoadCoinsuranceAccepted () {
        $('#inputCoInsAcceptedParticipationTotal').val(glbRequest.InsuranceCompanies[0].ParticipationPercentageOwn);
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', glbRequest.InsuranceCompanies[0].Id);
        $('#inputCoInsAcceptedLeadingInsurer').val(glbRequest.InsuranceCompanies[0].Description);
        $('#inputCoInsAcceptedParticipation').val(glbRequest.InsuranceCompanies[0].ParticipationPercentage);
        $('#inputCoInsAcceptedExpenses').val(glbRequest.InsuranceCompanies[0].ExpensesPercentage);
        $('#inputCoInsAcceptedLeadingPolicy').val(glbRequest.InsuranceCompanies[0].PolicyNumber);
        $('#inputCoInsAcceptedEndorsement').val(glbRequest.InsuranceCompanies[0].EndorsementNumber);
    }

    static LoadCoinsuranceAssigned () {
        $('#inputCoInsAssignedParticipationTotal').val(glbRequest.InsuranceCompanies[0].ParticipationPercentageOwn);
        $("#listCoInsuranceAssigned").UifListView({ sourceData: glbRequest.InsuranceCompanies, displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        var total = CoInsurance.CalculateCoInsuranceAssignedTotalAmount();
        $("#inputCoInsAssignedParticipationTotal").val(100 - total);
    }

    ShowPanelBusinessType (businessTypeId) {
        CoInsurance.ClearPanelAccepted();
        CoInsurance.ClearPanelAssigned();
        switch (businessTypeId) {
            case '1':
                $('#panelAccepted').hide();
                $('.panelAssigned').hide();
                break;
            case '2':
                $('#panelAccepted').show();
                $('.panelAssigned').hide();
                break;
            case '3':
                $('#panelAccepted').hide();
                $('.panelAssigned').show();
                break;
            default:
                $('#panelAccepted').hide();
                $('.panelAssigned').hide();
        }
    }

    CalculateAcceptedPercentage () {
        $("#alert").hide();
        //Se Valida Solo Si es Mayor a 100
        var percentageOrigin = parseFloat($('#inputCoInsAcceptedParticipationTotal').val().replace(separatorDecimal, separatorThousands));
        if ($('#inputCoInsAcceptedParticipation').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateValueParticipationPercentage, 'autoclose': true });
            return false;
        }
        else {

            var percentage = parseFloat($('#inputCoInsAcceptedParticipation').val().replace(separatorDecimal, separatorThousands));

            if (percentage + percentageOrigin > 100 || isNaN(percentage)) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateMinorPercentage, 'autoclose': true });
            }
        }
    }

    static CalculateAssingPercentage (isEdit, Id) {
        $("#alert").hide();
        var percentage = parseFloat($('#inputCoInsAssignedParticipation').val().replace(separatorDecimal, separatorThousands));
        var percentageTotal = CoInsurance.GetPercentajeTotal(isEdit, Id);


        if ((percentage + percentageTotal) >= 100 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        else {
            $('#inputCoInsAssignedParticipationTotal').val(100 - (percentage + percentageTotal));
            return true;
        }
    }

    static GetPercentajeTotal (isEdit, Id) {
        var percentage = parseFloat(0);

        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (key, value) {
            if (key != Id) {
                percentage += parseFloat(ParticipationPercentage.replace(separatorDecimal, separatorThousands));
            }
        })

        return percentage;
    }

    static addItemCoInsAssignedExpenses () {
        if (this.ValidDataTable()) {
            $("#listCoInsuranceAssigned").UifListView("addItem", CoInsurance.GetCoInsAssignedExpenses());
            CoInsurance.ClearAssignedInsured();
        }
    }

    static GetCoInsAssignedExpenses () {
        var inputCoInsAssignedInsured = {
            Id: $('#inputCoInsAssignedInsured').data('Id'),
            Description: $('#inputCoInsAssignedInsured').val(),
            ParticipationPercentage: $('#inputCoInsAssignedParticipation').val(),
            ExpensesPercentage: $('#inputCoInsAssignedExpenses').val()
        };
        return inputCoInsAssignedInsured;
    }
     
    static ClearAll()
    {
        $('#selectCoInsBusinessType').UifSelect("setSelected", "");
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
        CoInsurance.ClearPanelAccepted();
        CoInsurance.ClearPanelAssigned();
        CoInsurance.ClearAssignedInsured();
    }

    static ClearPanelAccepted () {
        $('#inputCoInsAcceptedParticipationTotal').val(0);
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', null);
        $('#inputCoInsAcceptedLeadingInsurer').val('');
        $('#inputCoInsAcceptedParticipation').val('');
        $('#inputCoInsAcceptedExpenses').val('');
        $('#inputCoInsAcceptedLeadingPolicy').val('');
        $('#inputCoInsAcceptedEndorsement').val('');
    }

    static ClearPanelAssigned() {

        CoInsAssignedIndex = null;
        $('#inputCoInsAssignedParticipationTotal').val(100);
        $('#inputCoInsAssignedInsured').data('Id', null);
        $('#inputCoInsAssignedInsured').val('');
        $('#inputCoInsAssignedParticipation').val('');
        $('#inputCoInsAssignedExpenses').val('');
        $("#listCoInsuranceAssigned").UifListView({ sourceData: null,  displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
    }

    static ClearAssignedInsured () {
        CoInsAssignedIndex = null;
        $('#inputCoInsAssignedInsured').data('Id', null);
        $('#inputCoInsAssignedInsured').val('');
        $('#inputCoInsAssignedParticipation').val('');
        $('#inputCoInsAssignedExpenses').val('');
    }

    static ValidDataTable () {
        $("#alert").hide();
        if ($('#inputCoInsAssignedInsured').data('Id') == null) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateCoInsurance, 'autoclose': true });
            return false;
        }
        if ($('#inputCoInsAssignedParticipation').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        if ($('#inputCoInsAssignedExpenses').val().trim().length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateExpensesPercentage, 'autoclose': true });
            return false;
        }

        if (parseFloat($('#inputCoInsAssignedExpenses').val().replace(separatorDecimal, separatorThousands)) >= 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateExpensesPercentage, 'autoclose': true });
            return false;
        }
        return true;
    }

    static SaveCoInsurance () {
        $("#alert").hide();
        $("#formCoInsurance").validate();
        if (!CoInsurance.ValidateGeneral($('#selectCoInsBusinessType').UifSelect("getSelected"))) {
            return false;
        }
        var valid = true;

        if ($('#selectCoInsBusinessType').UifSelect("getSelected") == BusinessType.Accepted) {
            $("#formCoInsurance").validate();
            valid = $("#formCoInsurance").valid();
        }
        else {
            $("#formCoInsurance").validate().cancelSubmit = true;
            valid = true;
        }


        if (valid) {
            var coInsurance = $("#formCoInsurance").serializeObject();
            coInsurance.AcceptedParticipationPercentageOwn = $('#inputCoInsAcceptedParticipationTotal').val();
            coInsurance.AcceptedCoinsurerId = $('#inputCoInsAcceptedLeadingInsurer').data('Id');
            coInsurance.AssignedParticipationPercentageOwn = $('#inputCoInsAssignedParticipationTotal').val();
            coInsurance.AssignedCoinsurerId = $('#inputCoInsAssignedInsured').data('Id');
            glbRequest.BusinessType = parseInt($('#selectCoInsBusinessType').UifSelect("getSelected"), 10);
            switch (glbRequest.BusinessType) {
                case BusinessType.Accepted:
                    glbRequest.InsuranceCompanies = CoInsurance.SaveinsuranceAccepted();
                    break;
                case BusinessType.Assigned:
                    glbRequest.InsuranceCompanies = $("#listCoInsuranceAssigned").UifListView('getData');
                    break;
                default:
                    break;
            }
            AsynchronousProcess.GetBusinessTypeById(glbRequest.BusinessType).done(function (dataBusinessType) {
                if (dataBusinessType.success) {
                    $('#selectedBusinessType').text(dataBusinessType.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': dataBusinessType.result, 'autoclose': true });
                }
            });
            RequestGrouping.HidePanels(MenuTypeGrouping.CoInsurance);
        }
    }
    static SaveinsuranceAccepted () {
        glbRequest.InsuranceCompanies = [];
        var InsuranceCompanie =
            {
                ParticipationPercentageOwn: $('#inputCoInsAcceptedParticipationTotal').val(),
                Id: $('#inputCoInsAcceptedLeadingInsurer').data('Id'),
                Description: $('#inputCoInsAcceptedParticipation').val(),
                ParticipationPercentage: $('#inputCoInsAcceptedParticipation').val(),
                ExpensesPercentage: $('#inputCoInsAcceptedExpenses').val(),
                PolicyNumber: $('#inputCoInsAcceptedLeadingPolicy').val(),
                EndorsementNumber: $('#inputCoInsAcceptedEndorsement').val(),

            }
        glbRequest.InsuranceCompanies.push(InsuranceCompanie);
        return glbRequest.InsuranceCompanies;
    }
    static GetAssignedCompanies () {
        return $("#listCoInsuranceAssigned").UifListView('getData');
    }

    static CoinsuredAssingList () {
        $('#btnCoInsAssignedNew').on('click', function (event) {
            CoInsurance.ClearPanelAssigned();
        });

        $('#listCoInsuranceAssigned').on('rowEdit', function (event, data, index) {
            CoInsAssignedIndex = index;
            CoInsurance.SetCoInsuranceAssigned(data);
        });

        $('#listCoInsuranceAssigned').on('rowDelete', function (event, data) {
            CoInsurance.ClearPanelAssigned();
            CoInsurance.DeleteCoInsuranceAssigned(data);
        });
    }

    static SetCoInsuranceAssigned (dataCoInsuranceAssigned) {
        $('#inputCoInsAssignedInsured').data('Id', dataCoInsuranceAssigned.Id),
        $('#inputCoInsAssignedInsured').val(dataCoInsuranceAssigned.Description),
        $('#inputCoInsAssignedParticipation').val(dataCoInsuranceAssigned.ParticipationPercentage),
        $('#inputCoInsAssignedExpenses').val(dataCoInsuranceAssigned.ExpensesPercentage)
    }

    static DeleteCoInsuranceAssigned (data) {
        var CoInsuranceAssigned = $("#listCoInsuranceAssigned").UifListView('getData');
        $("#listCoInsuranceAssigned").UifListView({ source: null, displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });

        $.each(CoInsuranceAssigned, function (index, value) {
            if (this.Id != data.Id) {
                $("#listCoInsuranceAssigned").UifListView("addItem", this);
            }
        });
        var total = CoInsurance.CalculateCoInsuranceAssignedTotalAmount();
        $("#inputCoInsAssignedParticipationTotal").val(100 - total);
    }

    static CalculateCoInsuranceAssignedTotalAmount () {
        var totalOriginal = 0;
        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (key, value) {
            totalOriginal += parseFloat(value.ParticipationPercentage.toString().replace(separatorDecimal, separatorThousands));
        })
        return totalOriginal;
    }

    static ValidateCoInsuranceAssigned (id) {
        var exists = false;

        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (index, value) {
            if (this.Id == id) {
                exists = true;
            }
        });

        return exists;
    }

    static ValidateEditCoInsuranceAssigned (isEdit, Id) {
        var percentageTotal = CoInsurance.GetPercentajeTotal(isEdit, Id);
        var percentage = parseFloat($('#inputCoInsAssignedParticipationTotal').val().replace(separatorDecimal, separatorThousands));

        if ((percentage + percentageTotal) > 100 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        if (percentage >= 100) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateMinorOwnParticipationPercentage, 'autoclose': true });
            return false;
        }
        if (percentage <= 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateMajorOwnParticipationPercentage, 'autoclose': true });
            return false;
        }
        if ((percentage + percentageTotal) <= 0 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateSumPercentage, 'autoclose': true });
            return false;
        }
        return true;
    }

    static ValidateGeneral (BusinessTypeId) {
        if (BusinessTypeId != "") {
            BusinessTypeId = parseInt(BusinessTypeId, 10);
            switch (BusinessTypeId) {
                case BusinessType.Accepted:

                    break;
                case BusinessType.Assigned:
                    return CoInsurance.ValidateEditCoInsuranceAssigned(false, -1);
                    break;
                default:
                    break;
            }
            return true;
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateBusinessType, 'autoclose': true });
            return false;
        }
    }
}

class AsynchronousProcess {
    static GetBusinessTypeById(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetBusinessTypeById',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}