
var ObjectCoInsurance =
{
    bindEvents: function () {
        coInsureType = 1;
        CoInsAssignedIndex = null;
        ObjectCoInsurance.initialize();
        $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
        //Coaseguro Aceptado
        $('#inputCoInsAcceptedLeadingInsurer').ValidatorKey(3, 1, 1);
        $('#inputCoInsAcceptedParticipation').OnlyDecimals(2);
        $('#inputCoInsAcceptedParticipationTotal').OnlyDecimals(2);
        $('#inputCoInsAcceptedExpenses').OnlyPercentage();
        $('#inputCoInsAcceptedLeadingPolicy').ValidatorKey(6, 1, 1);
        $('#inputCoInsAcceptedEndorsement').ValidatorKey(6, 1, 1);
        $('#inputCoInsAcceptedLeadingInsurer').ValidatorKey(3, 1, 1);
        $('#inputCoInsAssignedParticipation').OnlyPercentage();
        $('#inputCoInsAssignedExpenses').OnlyPercentage();
        ObjectCoInsurance.ClearPanelAccepted();
        ObjectCoInsurance.ClearPanelAssigned();
        $('#selectCoInsBusinessType').on('itemSelected', function (event, selectedItem) {
            event.stopImmediatePropagation();
            event.preventDefault();
            coInsureType = selectedItem.Id;
            ObjectCoInsurance.ShowPanelBusinessType(selectedItem.Id);
            $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        });
        $('#inputCoInsAcceptedLeadingInsurer').on('itemSelected', function (event, selectedItem) {
            $("#alert").hide();
            $('#inputCoInsAcceptedLeadingInsurer').data('Id', selectedItem.Id);
        });
        $('#inputCoInsAcceptedParticipation').focusout(function () {
            ObjectCoInsurance.CalculateAcceptedPercentage();
        });

        //Coaseguro Cedido

        $('#inputCoInsAssignedInsured').on('itemSelected', function (event, selectedItem) {
            $('#inputCoInsAssignedInsured').data('Id', selectedItem.Id);
        });
        $('#btnCoInsAssignedAccept').on("click", function () {

            if ($("#inputCoInsAssignedParticipation").val() >= 100) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateCoInsAssignedpercentage, 'autoclose': true });
                return false;
            }

            if (CoInsAssignedIndex == null) {
                if (!ObjectCoInsurance.ValidateCoInsuranceAssigned($('#inputCoInsAssignedInsured').data('Id'))) {
                    if (ObjectCoInsurance.CalculateAssingPercentage(false, -1)) {
                        ObjectCoInsurance.addItemCoInsAssignedExpenses();
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateCoInsurance, 'autoclose': true });
                }
            }
            else {
                if (ObjectCoInsurance.CalculateAssingPercentage(false, CoInsAssignedIndex)) {
                    $("#listCoInsuranceAssigned").UifListView("editItem", CoInsAssignedIndex, ObjectCoInsurance.GetCoInsAssignedExpenses());
                    var total = ObjectCoInsurance.CalculateCoInsuranceAssignedTotalAmount();
                    ObjectCoInsurance.ClearAssignedInsured();
                }
            }

        });

        $("#btnCoInsuranceClose").on("click", function () {
            RenewalRequestGrouping.HidePanels(MenuType.CoInsurance);
        });

        $("#btnCoInsuranceSave").on("click", function () {
            if (coInsureType == BusinessType.Accepted) {
                var percentageOrigin = parseFloat($('#inputCoInsAcceptedParticipationTotal').val().replace(separatorDecimal, separatorThousands));
                var percentage = parseFloat($('#inputCoInsAcceptedParticipation').val().replace(separatorDecimal, separatorThousands));
                if (percentageOrigin + percentage > 100) {
                    $('#inputCoInsAcceptedLeadingPolicy').ValidatorKey(6, 1, 1);
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateMajorPercentage, 'autoclose': true });
                    return false;
                }
            }
            ObjectCoInsurance.SaveCoInsurance();
        });

        ObjectCoInsurance.CoinsuredAssingList();
    },


    LoadCoinsurance: function () {
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
        switch (glbRequest.BusinessType) {
            case BusinessType.Accepted:
                $('#panelAccepted').show();
                ObjectCoInsurance.LoadCoinsuranceAccepted();
                break;
            case BusinessType.Assigned:
                $('.panelAssigned').show();
                $("#listCoInsuranceAssigned").UifListView({ displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
                ObjectCoInsurance.LoadCoinsuranceAssigned();
                break;
            default:
                break;
        }
    },

    LoadCoinsuranceAccepted: function () {
        $('#inputCoInsAcceptedParticipationTotal').val(glbRequest.InsuranceCompanies[0].ParticipationPercentageOwn);
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', glbRequest.InsuranceCompanies[0].Id);
        $('#inputCoInsAcceptedLeadingInsurer').val(glbRequest.InsuranceCompanies[0].Description);
        $('#inputCoInsAcceptedParticipation').val(glbRequest.InsuranceCompanies[0].ParticipationPercentage);
        $('#inputCoInsAcceptedExpenses').val(glbRequest.InsuranceCompanies[0].ExpensesPercentage);
        $('#inputCoInsAcceptedLeadingPolicy').val(glbRequest.InsuranceCompanies[0].PolicyNumber);
        $('#inputCoInsAcceptedEndorsement').val(glbRequest.InsuranceCompanies[0].EndorsementNumber);
    },

    LoadCoinsuranceAssigned: function () {
        $('#inputCoInsAssignedParticipationTotal').val(glbRequest.InsuranceCompanies[0].ParticipationPercentageOwn);
        $("#listCoInsuranceAssigned").UifListView({ sourceData: glbRequest.InsuranceCompanies, displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });
        var total = CoInsurance.CalculateCoInsuranceAssignedTotalAmount();
        $("#inputCoInsAssignedParticipationTotal").val(100 - total);
    },

    ShowPanelBusinessType: function (businessTypeId) {
        ObjectCoInsurance.ClearPanelAccepted();
        ObjectCoInsurance.ClearPanelAssigned();
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
    },

    CalculateAcceptedPercentage: function () {
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
    },

    CalculateAssingPercentage: function (isEdit, Id) {
        $("#alert").hide();
        var percentage = parseFloat($('#inputCoInsAssignedParticipation').val().replace(separatorDecimal, separatorThousands));
        var percentageTotal = ObjectCoInsurance.GetPercentajeTotal(isEdit, Id);


        if ((percentage + percentageTotal) >= 100 || isNaN(percentage)) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageValidateParticipationPercentage, 'autoclose': true });
            return false;
        }
        else {
            $('#inputCoInsAssignedParticipationTotal').val(100 - (percentage + percentageTotal));
            return true;
        }
    },

    GetPercentajeTotal: function (isEdit, Id) {
        var percentage = 0;

        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (key, value) {
            if (key != Id) {
                percentage += parseFloat(this.ParticipationPercentage);
            }
        })

        return percentage;
    },

    addItemCoInsAssignedExpenses: function () {
        if (ObjectCoInsurance.ValidDataTable()) {
            $("#listCoInsuranceAssigned").UifListView("addItem", ObjectCoInsurance.GetCoInsAssignedExpenses());
            ObjectCoInsurance.ClearAssignedInsured();
        }
    },

    GetCoInsAssignedExpenses: function () {
        var inputCoInsAssignedInsured = {
            Id: $('#inputCoInsAssignedInsured').data('Id'),
            Description: $('#inputCoInsAssignedInsured').val(),
            ParticipationPercentage: $('#inputCoInsAssignedParticipation').val(),
            ExpensesPercentage: $('#inputCoInsAssignedExpenses').val()
        };
        return inputCoInsAssignedInsured;
    },

    ClearPanelAccepted: function () {
        $('#inputCoInsAcceptedParticipationTotal').val(0);
        $('#inputCoInsAcceptedLeadingInsurer').data('Id', null);
        $('#inputCoInsAcceptedLeadingInsurer').val('');
        $('#inputCoInsAcceptedParticipation').val('');
        $('#inputCoInsAcceptedExpenses').val('');
        $('#inputCoInsAcceptedLeadingPolicy').val('');
        $('#inputCoInsAcceptedEndorsement').val('');
    },

    ClearPanelAssigned: function () {
        CoInsAssignedIndex = null;
        $('#inputCoInsAssignedParticipationTotal').val(100);
        $('#inputCoInsAssignedInsured').data('Id', null);
        $('#inputCoInsAssignedInsured').val('');
        $('#inputCoInsAssignedParticipation').val('');
        $('#inputCoInsAssignedExpenses').val('');
    },

    ClearAssignedInsured: function () {
        CoInsAssignedIndex = null;
        $('#inputCoInsAssignedInsured').data('Id', null);
        $('#inputCoInsAssignedInsured').val('');
        $('#inputCoInsAssignedParticipation').val('');
        $('#inputCoInsAssignedExpenses').val('');
    },

    ValidDataTable: function () {
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
    },

    SaveCoInsurance: function () {
        $("#alert").hide();
        $("#formCoInsurance").validate();
        if (!ObjectCoInsurance.ValidateGeneral($('#selectCoInsBusinessType').UifSelect("getSelected"))) {
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
                    glbRequest.InsuranceCompanies = ObjectCoInsurance.SaveinsuranceAccepted();
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
            RenewalRequestGrouping.HidePanels(MenuType.CoInsurance);
        }
    },
    SaveinsuranceAccepted: function () {
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
    },
    GetAssignedCompanies: function () {
        return $("#listCoInsuranceAssigned").UifListView('getData');
    },

    initialize: function () {
        $('#panelAccepted').hide();
        $('.panelAssigned').hide();
    },

    CoinsuredAssingList: function () {
        $('#btnCoInsAssignedNew').on('click', function (event) {
            ObjectCoInsurance.ClearPanelAssigned();
        });

        $('#listCoInsuranceAssigned').on('rowEdit', function (event, data, index) {
            CoInsAssignedIndex = index;
            ObjectCoInsurance.SetCoInsuranceAssigned(data);
        });

        $('#listCoInsuranceAssigned').on('rowDelete', function (event, data) {
            ObjectCoInsurance.ClearPanelAssigned();
            ObjectCoInsurance.DeleteCoInsuranceAssigned(data);
        });
    },

    SetCoInsuranceAssigned: function (dataCoInsuranceAssigned) {
        $('#inputCoInsAssignedInsured').data('Id', dataCoInsuranceAssigned.Id),
        $('#inputCoInsAssignedInsured').val(dataCoInsuranceAssigned.Description),
        $('#inputCoInsAssignedParticipation').val(dataCoInsuranceAssigned.ParticipationPercentage),
        $('#inputCoInsAssignedExpenses').val(dataCoInsuranceAssigned.ExpensesPercentage)
    },

    DeleteCoInsuranceAssigned: function (data) {
        var CoInsuranceAssigned = $("#listCoInsuranceAssigned").UifListView('getData');
        $("#listCoInsuranceAssigned").UifListView({ source: null, displayTemplate: "#CoInsuranceAssignedTemplate", edit: true, delete: true, customAdd: true, customEdit: true, customDelete: true, height: 300 });

        $.each(CoInsuranceAssigned, function (index, value) {
            if (this.Id != data.Id) {
                $("#listCoInsuranceAssigned").UifListView("addItem", this);
            }
        });
        var total = ObjectCoInsurance.CalculateCoInsuranceAssignedTotalAmount();
        $("#inputCoInsAssignedParticipationTotal").val(100 - total);
    },

    CalculateCoInsuranceAssignedTotalAmount: function () {
        var totalOriginal = 0;
        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (key, value) {
            totalOriginal += parseFloat(value.ParticipationPercentage.toString().replace(separatorDecimal, separatorThousands));
        })
        return totalOriginal;
    },

    ValidateCoInsuranceAssigned: function (id) {
        var exists = false;

        $.each($("#listCoInsuranceAssigned").UifListView('getData'), function (index, value) {
            if (this.Id == id) {
                exists = true;
            }
        });

        return exists;
    },

    ValidateEditCoInsuranceAssigned: function (isEdit, Id) {
        var percentageTotal = ObjectCoInsurance.GetPercentajeTotal(isEdit, Id);
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
    },

    ValidateGeneral: function (BusinessTypeId) {
        if (BusinessTypeId != "") {
            BusinessTypeId = parseInt(BusinessTypeId, 10);
            switch (BusinessTypeId) {
                case BusinessType.Accepted:

                    break;
                case BusinessType.Assigned:
                    return ObjectCoInsurance.ValidateEditCoInsuranceAssigned(false, -1);
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