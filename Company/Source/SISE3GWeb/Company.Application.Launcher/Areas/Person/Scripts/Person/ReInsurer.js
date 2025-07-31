class ReInsurer extends Uif2.Page {
    getInitialState() {
        
    }

    bindEvents() {
        $("#btnReInsurer").click(this.BtnReInsurer);
        $("#btnRecordReInsurer").click(this.RecordReInsurer);
        $("#reInsurerDeclinedTypes").on('itemSelected', this.ChangeReInsurerDeclineType);
        //$("#linkPrefix").click(this.LoadPrefix);
        //$("input[name=person]").change(this.AgentPerson);
    }

    static initializeReInsurer(individualId) {
        $("#hidennRowId").val(0);
        $("#inputkey").ValidatorKey(ValidatorType.Number, 0, 0);
        $.UifProgress('show');
        ReInsurerRequest.GetAplicationReInsurerByIndividualId(individualId).done(function (data) {
            $.UifProgress('close');
            if (data.success && data.result != null) {
                $("#codeReInsurer").val(data.result.Id);
                $("#dateCreationDateReinsurer").UifDatepicker('setValue', DateNowPerson);
                $("#dateModificationDateReinsurer").UifDatepicker('setValue', DateNowPerson);
                $("#dateDeclinationDateReinsurer").UifDatepicker('setValue', FormatDate(data.result.DeclinedDate));
                $("#inputObservationReInsurer").val(data.result.Annotations);
                $("#reInsurerDeclinedTypes").UifSelect("setSelected", data.result.DeclaredTypeCD);
            }
            else {                 
                $("#dateCreationDateReinsurer").UifDatepicker('setValue', DateNowPerson);
                $("#reInsurerDeclinedTypes").UifSelect('setSelected', null);
                $("#inputObservationReInsurer").val("");
                $("#codeReInsurer").val("");
            }
            ReInsurer.ControlEnabledDisabled(false);
        }).fail(function (jqXHR, textStatus) {
            $.UifProgress('close');
        });
    }

    static CreateReInsurer() {
        if (ReInsurer.ValidateReInsurer()) {
            var reInsurerTmp = {
                IndividualId: individualId,
                EnteredDate: $("#dateCreationDateReinsurer").val(),
                ModifyDate: $("#dateModificationDateReinsurer").val(),
                DeclinedDate: $("#dateDeclinationDateReinsurer").val(),
                DeclaredTypeCD: $("#reInsurerDeclinedTypes").UifSelect("getSelected"),
                Annotations: $("#inputObservationReInsurer").val(),
                Id : $("#codeReInsurer").val()
            }

            lockScreen()
            ReInsurerRequest.CreateAplicationReInsurer(reInsurerTmp).done(function (data) {
                if (data.success) {
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                    let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonReinsurer);
                        }
                    } else {
                        $.UifDialog('alert', { 'message': AppResourcesPerson.MessageInformation });
                        $("#codeReInsurer").val(data.result.Id);
                        $('#checkReinsurer').prop('checked', true);
                        $('#checkReinsurer').addClass('primary');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                unlockScreen();
            }).fail(() => unlockScreen());
        }
    }

    static ValidateReInsurer() {
        var msj = "";
        if ($("#dateCreationDate").val() == null) {
            msj = AppResourcesPerson.LabelTypeIntermediary + "<br>"
        }
        if ($("#dateModificationDate").val() == null) {
            msj = AppResourcesPerson.LabelTypeIntermediary + "<br>"
        }
        if ($("#reInsurerDeclinedTypes").UifSelect("getSelected") != null && $("#dateDeclinationDate").val() == null) {
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

    BtnReInsurer() {
        if (individualId == Person.New || individualId <= 0) {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
        else if ($("#selectSearchPersonType").val() != "2") {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ShouldBeLegalPerson });
        }
        else {
            ReInsurer.GetOthersDeclinedTypes().done(function (data) {
                if (data.success) {
                    $("#reInsurerDeclinedTypes").UifSelect({ sourceData: data.result });
                }
            });
            $.uif2.helpers.setGlobalTitle(Persons.PeripheralTitle());
            $("#codPersonReInsurer").val(individualId);
            $("#codPersonReInsurer").text(individualId);
            ReInsurer.initializeReInsurer(individualId);
            Persons.ShowPanelsPerson(RolType.ReInsurer);
        }
    }

    RecordReInsurer() {
        if (ReInsurer.CreateReInsurer()) {
        }
        else {
            return false;
        }
    }

    ChangeReInsurerDeclineType() {
        if ($("#reInsurerDeclinedTypes").UifSelect("getSelected") == null || $("#reInsurerDeclinedTypes").UifSelect("getSelected") == "") {
            $("#dateDeclinationDateReinsurer").UifDatepicker('setValue', null);
        }
        else {
            $("#dateDeclinationDateReinsurer").UifDatepicker('setValue', DateNowPerson);
        }
    }

    static ControlEnabledDisabled(control) {
        $("#dateDeclinationDateReinsurer").UifDatepicker('disabled', control);
        $("#reInsurerDeclinedTypes").UifSelect("disabled", control);

    } 

        
}