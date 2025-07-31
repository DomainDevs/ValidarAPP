var IndividualId = 0;
var declinedTypeId = null;
class Third extends Uif2.Page {

    getInitialState() {

        Third.dataInitialThird();
    }

    static dataInitialThird() { 
        $("#InputDateModificationThird").UifDatepicker("setValue", new Date());
        $("#selectReasonLowThird").UifSelect("setSelected", null);

    }
    bindEvents() {
        $("#btnThird").click(this.showSupplier);
        $("#btnAcceptThird").click(this.processInsertThirdPerson);
        $('#selectReasonLowThird').on('itemSelected', (event, selectedItem) => {
            if (selectedItem.Id != "") {
                Third.ChangeDateDeclined();
            } else {
                $("#InputDateDeclinedThird").UifDatepicker("setValue", null);
            }
        });

    }

    static ChangeDateDeclined() {
        $("#InputDateDeclinedThird").UifDatepicker("setValue", new Date());
    }
    showSupplier() {
        IndividualId = $('#lblPersonCode').val() || $('#lblCompanyCode').val();
        if (individualId == Person.New || individualId <= 0 || individualId == undefined) {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
        else {
            //$.UifProgress('show');
            ThirdRequest.GetThirdDeclinedType().done(function (data) {
                if (data.success) {
                    $("#selectReasonLowThird").UifSelect({ sourceData: data.result });
                    if (data.result.length > 0) {
                        $("#selectReasonLowThird").UifSelect("setSelected", data.result[0].Id);
                    }
                }
            });
            $('#CodPersonIdThird').val(IndividualId);
            $('#CodPersonIdThird').text(IndividualId);
            ThirdRequest.GetThirdByIndividualId(individualId).done(function (data) {
                if (data.success && data.IndividualId > 0) {
                    $('#CodPersonIdThird').val(data.Id);
                    $('#CodPersonIdThird').text(data.Id);
                    $("#InputDateCreate").UifDatepicker("setValue", FormatDate(data.CreationDate));
                } else {
                    $("#InputDateCreate").UifDatepicker("setValue", new Date());
                }
            });
            Persons.ShowPanelsPerson(RolType.Third);
        }
    }

    processInsertThirdPerson() {
        var objBasicInformationThird = $("#formThirdParty").serializeObject();
        objBasicInformationThird.Id = $("#CodThird").html();
        var modelThird = Third.CreateModelThird();

        lockScreen();
        ThirdRequest.CreateThird(modelThird).done(function (data) {
            if (data.success) {

                var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                    if (countAuthorization > 0) {
                        LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonThird);
                    }
                } else {
                    Third.clearThird();
                    Third.loadThirdPerson(data.result);
                    $("#modalThird").UifModal("hide");
                    $.UifNotify('show', { 'type': 'info', 'message': 'Guardado con éxito.', 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
            unlockScreen();
        }).fail(() => unlockScreen());
    }

    static validateThird(objBasicInformationThird) {
        if (Third.validateBasicInformation(objBasicInformationThird)) {
            return true;
        }
    }

    static validateBasicInformation(objBasicInformationThird) {
        var arrayPropertiesRequired = { IndividualId: "Codigo de persona", CreationDate: "Fecha de creación" };
        var band = true;
        $.each(arrayPropertiesRequired, function (index, value) {
            if (objBasicInformationThird[index] == null || objBasicInformationThird[index] == "" || objBasicInformationThird[index] == "0") {
                $.UifNotify('show', { 'type': 'danger', 'message': value + " " + AppResourcesPerson.ErrorIsRrequired, 'autoclose': true });
                band = false;
                return false;
            }
        });
        return band;
    }

    static clearThird() {
        $('#CodThird').text("")
        $("#CodPersonIdThird").val("");
        $("#selectProviderDeclined").UifSelect("setSelected", null);
        $("#InputDateCreate").UifDatepicker("setValue", new Date());
        $("#InputDateModification").UifDatepicker("clear");
        $("#InputDateDeclined").UifDatepicker("clear");
        $("#selectReasonLowThird").UifSelect("setSelected", null);
        $("#notesThird").val(null);

    }

    static loadThirdPerson(objInformationThird) {
        if (objInformationThird != null && objInformationThird.Id > 0) {

            $("#CodThird").text(objInformationThird.Id);
            $("#notesThird").val(objInformationThird.Annotation);
            $("#selectReasonLowThird").UifSelect("setSelected", objInformationThird.DeclinedTypeId);
            Third.assignDate("#InputDateCreate", objInformationThird.CreationDate);
            Third.assignDate("#InputDateModificationThird", objInformationThird.ModificationDate);
            Third.assignDate("#InputDateDeclinedThird", objInformationThird.DeclinationDate);
            $('#checkThird').prop('checked', true);
            $("#selectReasonLowThird").prop("disabled", true);

        }
        else {
            Third.dataInitialThird();
        }
    }

    static assignDate(tag, date) {
        if (date != null && date != "") {
            $(tag).UifDatepicker("setValue", FormatDate(date));
        }
        else {
            $(tag).UifDatepicker("clear");
        }
    }

    static CreateModelThird() {
        var temp = {
            Id: $('#CodThird').html(),
            IndividualId: IndividualId,
            CreationDate: $("#InputDateCreate").val(),
            ModificationDate: $("#InputDateModification").val(),
            DeclinationDate: $("#InputDateDeclinedThird").val(),
            ModificationDate: $("#InputDateModificationThird").val(),
            DeclinedTypeId: $("#selectReasonLowThird").UifSelect("getSelectedSource") == null ? null : $("#selectReasonLowThird").UifSelect("getSelectedSource").Id,
            Annotation: $("#notesThird").val(),
        };

        return temp;
    }

    static GetThird(data) {

        if (data.IndividualId > 0) {
            $('#CodPersonIdThird').text(data.IndividualId);
            $('#CodThird').text(data.Id);
            $('#Id').val(data.Id);
            $('#InputDateCreate').val(FormatDate(data.CreationDate));
            $('#InputDateModificationThird').val(FormatDate(data.ModificationDate));
            $('#InputDateDeclinedThird').val(FormatDate(data.DeclinationDate));
            $('#selectReasonLowThird').val(data.DeclinedTypeId);
            $('#notesThird').val(data.Annotation);
            declinedTypeId = data.DeclinedTypeId;
            //if (data.DeclinedTypeId != null && data.DeclinedTypeId > 0) {
            //    $("#selectReasonLowThird").prop("disabled", true);
            //}

        } else {
            Third.clearThird();
        }

    }
}