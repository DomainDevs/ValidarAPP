class CoInsurer extends Uif2.Page {
    getInitialState() {
        
    }

    bindEvents() {
        $("#btnCoInsurer").click(this.BtnCoInsurer);
        $("#btnRecordCoInsurer").click(this.RecordCoInsurer);
        $("#coInsurerDeclinedTypes").on('itemSelected', this.ChangeCoInsurerDeclineType);
        $("#linkPrefix").click(this.LoadPrefix);
        $("input[name=person]").change(this.AgentPerson);
    }

    static initializeCoInsurer(individualId) {
        $("#hidennRowId").val(0);
        $("#inputkey").ValidatorKey(ValidatorType.Number, 0, 0);
        $.UifProgress('show');
        CoInsurerRequest.GetCoInsurer(individualId).done(function (data) {
            $.UifProgress('close');
            if (data.success && data.result != null) {
                //$("#codeCoInsurer").val(data.result.InsuranceCompanyId);
                //$("#dateCreationDate").UifDatepicker('setValue', FormatDate(data.result.EnteredDate));
                //$("#dateModificationDate").UifDatepicker('setValue', FormatDate(data.result.ModifyDate));                
                //$("#dateDeclinationDate").UifDatepicker('setValue', FormatDate(data.result.DeclinedDate));
                //$("#inputObservationCoInsurer").val(data.result.Annotations);
                //$("#coInsurerDeclinedTypes").UifSelect("setSelected", data.result.ComDeclinedTypeCode);

                $("#codeCoInsurer").val(data.result.InsuraceCompanyId);
                $("#dateCreationDate").UifDatepicker('setValue', FormatDate(data.result.EnteredDate));
                $("#dateModificationDate").UifDatepicker('setValue', FormatDate(data.result.ModifyDate));
                $("#dateDeclinationDate").UifDatepicker('setValue', FormatDate(data.result.DeclinedDate));
                $("#inputObservationCoInsurer").val(data.result.Annotations);
                $("#coInsurerDeclinedTypes").UifSelect("setSelected", data.result.ComDeclinedTypeCode);
            }
            else {
                $("#dateCreationDate").UifDatepicker('setValue', DateNowPerson);
                $("#coInsurerDeclinedTypes").UifSelect('setSelected', null);
                $("#inputObservationCoInsurer").val("");
                $("#codeCoInsurer").val("");
            }
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
    }

    static CreateCoInsurer() {
        
        if (CoInsurer.ValidateCoInsurer()) {
            var coInsurerTmp = {
                IndividualId: $('#lblCompanyCode').val(),
                EnteredDate: $("#dateCreationDate").val(),
                ModifyDate: DateNowPerson,//$("#dateModificationDate").val(),
                DeclinedDate: $("#dateDeclinationDate").val(),
                ComDeclinedTypeCode: $("#coInsurerDeclinedTypes").UifSelect("getSelected"),
                Annotations: $("#inputObservationCoInsurer").val(),
                Description: $('#inputCompanyTradeName').val(),
                TributaryIdNo: $('#inputCompanyDocumentNumber').val().trim(),
                Street: $('#inputCompanyAddress').val(),
                AddressTypeCode: $('#selectCompanyAddressType').UifSelect('getSelected'),
                PhoneTypeCode: $('#selectCompanyPhoneType').UifSelect('getSelected'),
                PhoneNumber: $('#inputCompanyPhone').val(),
                InsuraceCompanyId: $('#codeCoInsurer').val(),
                IvaTypeCode: 1,
                CityCode: $("#inputCompanyCity").data('Id'),
                CountryCode: $("#inputCompanyCountry").data('Id'),
                StateCode: $("#inputCompanyState").data('Id')
            }
        }
         
        if (coInsurerTmp.InsuraceCompanyId > 0) {
            lockScreen();
            CoInsurerRequest.UpdateCompanyCoInsured(coInsurerTmp).done(function (data) {
                if (data.success) {
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                    let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonCoinsurer);
                        }
                    } else
                    {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.MessageInformation });
                        $("#codeCoInsurer").val(data.result.InsuranceCompanyId);
                        $('#checkCoInsurer').prop('checked', true);
                        $('#checkCoInsurer').addClass('primary');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                unlockScreen();
            }).fail(() => unlockScreen());;
        }
        else {
            lockScreen();
            CoInsurerRequest.CreateCoInsurer(coInsurerTmp).done(function (data) {
                if (data.success) {
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                    let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonCoinsurer);
                        }
                    } else {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.MessageInformation });
                        $("#codeCoInsurer").val(data.result.InsuranceCompanyId);
                        $('#checkCoInsurer').prop('checked', true);
                        $('#checkCoInsurer').addClass('primary');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                unlockScreen();
            }).fail(() => unlockScreen());
        }
    }

    static ValidateCoInsurer() {
        var msj = "";
        if ($("#dateCreationDate").val() == null) {
            msj = AppResourcesPerson.LabelTypeIntermediary + "<br>"
        }
        if ($("#dateModificationDate").val() == null) {
            msj = AppResourcesPerson.LabelTypeIntermediary + "<br>"
        }
        if ($("#coInsurerDeclinedTypes").UifSelect("getSelected") != null && $("#dateDeclinationDate").val() == null) {
            msj = AppResourcesPerson.LabelTypeIntermediary + "<br>"
        }

        if (msj != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + msj, 'autoclose': true })
            return false;
        }
        return true;
    }

    static GetOthersDeclinedTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Person/GetOthersDeclinedTypes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    BtnCoInsurer() {
        if (individualId == Person.New || individualId <= 0) {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
        else if ($("#selectSearchPersonType").val() != "2") {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ShouldBeLegalPerson });
        }
        else {
            CoInsurer.GetOthersDeclinedTypes().done(function (data) {
                if (data.success) {
                    $("#coInsurerDeclinedTypes").UifSelect({ sourceData: data.result });
                }
            });
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            $("#codPersonCoInsurer").val(individualId);
            $("#codPersonCoInsurer").text(individualId);
            CoInsurer.initializeCoInsurer(individualId);
            Persons.ShowPanelsPerson(RolType.CoInsurer);
        }
    }

    RecordCoInsurer() {
        if (CoInsurer.CreateCoInsurer()) {
        }
        else {
            return false;
        }
    }

    ChangeCoInsurerDeclineType() {
        if ($("#coInsurerDeclinedTypes").UifSelect("getSelected") == null || $("#coInsurerDeclinedTypes").UifSelect("getSelected") == "") {
            $("#dateDeclinationDate").UifDatepicker('setValue', null);
        }
        else {
            $("#dateDeclinationDate").UifDatepicker('setValue', DateNowPerson);
        }
    }
}