class Employee extends Uif2.Page {
    getInitialState() {
        Employee.clearInputEmployee();

    }

    bindEvents() {
        $("#btnEmployee").click(this.showEmployee);
        $('#selectReasonLowEmployee').on('itemSelected', (event, selectedItem) => { Employee.ChangeDateDeclined(); });
        $("#btnAcceptEmployee").click(this.saveEmployee);
        $("#InputCodeEmployee").prop("disabled", "disabled");
        $('#rbEnabledEmployee').on('mousedown', function (e) {
            var wasChecked = $(this).prop('checked');
            this.turnOff = wasChecked;
            $(this).prop('checked', !wasChecked);
        });

        $('#rbEnabledEmployee').on('click', function (e) {
            $(this).prop('checked', !this.turnOff);
            this['turning-off'] = !this.turnOff;
        });
        $("#rbEnabledEmployee").prop('checked', false);
        $("#rbEnabledEmployee").prop('disabled', true);
    }

    showEmployee() {
        Employee.getBranch();
        $("#InputDateCreate").UifDatepicker("setValue", new Date());
        $("#InputDateDeclinedEmployee").UifDatepicker("setValue", new Date());
        $("#InputDateModificationEmployee").UifDatepicker("setValue", new Date());
        IndividualId = $('#lblPersonCode').val() || $('#lblCompanyCode').val();
        if (individualId == Person.New || individualId <= 0 || individualId == undefined) {
            $.UifDialog('alert', { 'message': AppResourcesPerson.ErrorPersonEmpty });
        }
        else {
            EmployeeRequest.GetDeclinedType().done(function (data) {
                if (data.success) {
                    $("#selectReasonLowEmployee").UifSelect({ sourceData: data.result });
                    if (data.result.length > 0) {
                        $("#selectReasonLowEmployee").UifSelect("setSelected", data.result[0].Id);
                    }
                }
            });
            $('#CodPersonIdEmployee').val(IndividualId);
            $('#CodPersonIdEmployee').text(IndividualId);
            EmployeeRequest.GetEmployeByIndividualId(individualId).done(function (data) {
                if (data.success) {
                    if (data.result.IndividualId > 0) {
                        $("#InputCodeEmployee").prop("disabled", true);
                        $("#InputDateModificationEmployee").UifDatepicker("setValue", new Date());
                        $("#selectBranchEmployee").UifSelect("setSelected", data.result.BranchId);
                        $("#InputCodeEmployee").val(data.result.FileNumber);
                        $("#InputDateCreateEmployee").val(FormatDate(data.result.EntryDate));
                        $("#InputDateDeclinedEmployee").val(data.result.EgressDate == null ? "" : FormatDate(data.result.EgressDate));
                        $("#selectReasonLowEmployee").UifSelect("setSelected", data.result.DeclinedTypeId == 0 ? null : data.result.DeclinedTypeId);
                        $("#notesEmployee").val(data.result.Annotation);
                        if (data.result.DeclinedTypeId != null && data.result.DeclinedTypeId > 0) {
                            $("#selectReasonLowEmployee").prop("disabled", true)
                        }
                        if ($('#InputDateDeclinedEmployee').val() == "") {
                            $("#rbEnabledEmployee").prop('disabled', true);
                        }
                        else {
                            $("#rbEnabledEmployee").prop('disabled', false);
                        }
                        $("#rbEnabledEmployee").prop('checked', false);
                    }
                    else {
                        Employee.clearInputEmployee();
                    }

                }
            });
            Persons.ShowPanelsPerson(RolType.Employee);
        }
    }

    saveEmployee() {
        var employeeModel = Employee.setModelEmployee();
        if (Employee.validateEmployee(employeeModel)) {
            lockScreen();
            EmployeeRequest.SaveEmployee(employeeModel).done(function (data) {
                if (data.success) {
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(data.result.InfringementPolicies, true);
                    let countAuthorization = data.result.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;
                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(data.result.InfringementPolicies, data.result.OperationId, FunctionType.PersonEmployed);
                        }
                    } else {
                        Employee.clearInputEmployee();
                        Employee.loadEmployeeData(data.result);
                        $("#modalEmployee").UifModal("hide");
                        $.UifNotify('show', { 'type': 'info', 'message': 'Guardado con éxito.', 'autoclose': true });
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
                unlockScreen()
            }).fail(() => { unlockScreen() });
        }
    }

    static loadEmployeeData(result) {
        $("#selectBranchEmployee").UifSelect("setSelected", result.BranchId);
        $("#InputCodeEmployee").val(result.FileNumber);
        $("#InputDateCreateEmployee").val(result.EntryDate);
        $("#InputDateModificationEmployee").val(result.EgressDate);
        $("#InputDateDeclinedEmployee").val(result.DeclinedDate);
        $("#selectReasonLowEmployee").UifSelect("setSelected", result.DeclinedTypeId);
        $("#notesEmployee").val(result.Annotation);
        if ($('#InputDateDeclinedEmployee').val() == "") {
            $("#rbEnabledEmployee").prop('disabled', true);
        }
        else {
            $("#rbEnabledEmployee").prop('disabled', false);
        }
        $("#rbEnabledEmployee").prop('checked', false);
    }

    static setModelEmployee() {
        if ($("#rbEnabledEmployee").prop('checked')) {
            $("#InputDateDeclinedEmployee").val("");
        }
        var temp = {
            IndividualId: IndividualId,
            BranchId: $("#selectBranchEmployee").UifSelect("getSelectedSource") == null ? null : $("#selectBranchEmployee").UifSelect("getSelectedSource").Id,
            FileNumber: $("#InputCodeEmployee").val() == null ? "" : $("#InputCodeEmployee").val(),
            EntryDate: $("#InputDateCreateEmployee").val(),
            ModificationDate: $("#InputDateModificationEmployee").val(),
            EgressDate: $("#InputDateDeclinedEmployee").val(),
            DeclinedTypeId: $("#selectReasonLowEmployee").UifSelect("getSelectedSource") == null ? null : $("#selectReasonLowEmployee").UifSelect("getSelectedSource").Id,
            Annotation: $("#notesEmployee").val(),
        };

        return temp;
    }

    static getBranch() {
        BranchRequest.GetBranchs().done(function (data) {
            if (data.success && data.result.length > 0) {
                $("#selectBranchEmployee").UifSelect({ sourceData: data.result });
                $("#selectBranchEmployee").UifSelect("setSelected", null);
            }
        });
    }

    static clearInputEmployee() {
        $("#InputDateCreateEmployee").UifDatepicker("setValue", new Date());
        $("#InputDateDeclinedEmployee").val("");
        $("#InputCodeEmployee").val("");
        $("#InputDateModificationEmployee").UifDatepicker("setValue", new Date());
        $("#selectReasonLowEmployee").UifSelect("setSelected", null);
        $("#selectBranchEmployee").UifSelect("setSelected", null);
        $("#notesEmployee").val("");
    }

    static ChangeDateDeclined() {
        if ($("#selectReasonLowEmployee").val() == "") {
            $("#InputDateDeclinedEmployee").val("");
        } else {
            $("#InputDateDeclinedEmployee").UifDatepicker("setValue", new Date());
        }

    }

    static validateBasicInformation(objBasicInformation) {
        var arrayPropertiesRequired = { IndividualId: "Codigo de persona", BranchId: "Campo Sucursal" };
        var band = true;
        $.each(arrayPropertiesRequired, function (index, value) {
            if (objBasicInformation[index] == null || objBasicInformation[index] == "" || objBasicInformation[index] == "0") {
                $.UifNotify('show', { 'type': 'danger', 'message': value + " " + AppResourcesPerson.ErrorIsRrequired, 'autoclose': true });
                band = false;
                return false;
            }
        });
        return band;
    }

    static validateEmployee(objBasicInformation) {
        if (Employee.validateBasicInformation(objBasicInformation)) {
            return true;
        }
    }
}