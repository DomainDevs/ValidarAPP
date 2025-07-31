var conditionIndex = null;
var conditionIdSelected = null;

class ConditionTax extends Uif2.Page {
    getInitialState() {
        $("#lvConditionTax").UifListView({
            source: null,
            edit: true,
            delete: true,
            customEdit: true,
            displayTemplate: "#ConditionTaxTemplate",
            deleteCallback: ConditionTax.deleteCallbackConditionTax,
            selectionType: 'single',
            height: 300
        });

        ConditionTax.GetConditionsByTaxId();
        $('#titleTaxSelected').text(taxModel.Description);
    }
    bindEvents() {
        $('#IdCondition').attr("disabled", true);
        $('#AddCondition').click(ConditionTax.AddCondition);
        $('#CancelCondition').click(ConditionTax.CancelCondition);
        $('#ExitCondition').click(ConditionTax.ExitCondition);
        $('#lvConditionTax').on('rowEdit', ConditionTax.showData);
        $('#SaveCondition').click(ConditionTax.SaveConditionBD);
    }
    static showData(event, result, index) {
        ConditionTax.ClearCondition();
        conditionIndex = index;
        conditionIdSelected = result.IdCondition;

        $("#IdCondition").val(result.IdCondition);
        $("#DescriptionCondition").val(result.DescriptionCondition);
        $('#NationalRate').prop('checked', result.NationalRate);
        $('#Independent').prop('checked', result.Independent);
        $('#Enabled').prop('checked', result.Enabled);
    }
    static AddCondition() {
        $("#formConditionTax").validate();
        if ($("#formConditionTax").valid()) {
            var formConditionTax = $("#formConditionTax").serializeObject();
            if (formConditionTax.NationalRate == "on") {
                formConditionTax.NationalRate = true;
            } else {
                formConditionTax.NationalRate = false;
            }

            if (formConditionTax.Independent == "on") {
                formConditionTax.Independent = true;
            } else {
                formConditionTax.Independent = false;
            }
            if (formConditionTax.Enabled == "on") {
                formConditionTax.Enabled = true;
            } else {
                formConditionTax.Enabled = false;
            }
            if (conditionIndex != null) {
                formConditionTax.Status = ParametrizationStatus.Update;
                $("#lvConditionTax").UifListView('editItem', conditionIndex, formConditionTax);
            } else {
                formConditionTax.Status = ParametrizationStatus.Create;
                $("#lvConditionTax").UifListView("addItem", formConditionTax);
            }
            ConditionTax.ClearCondition();
        }
    }
    static deleteCallbackConditionTax(deferred, result) {
        deferred.resolve();

        var data = result;
        var taxCode = data.IdTax;
        var taxConditionCode = data.IdCondition;

        ConditionTaxRequests.DeleteSelectedTaxCondition(taxConditionCode, taxCode).done(function (response) {
            if (response.success) {
                if (response.result == true) {
                    data.Status = ParametrizationStatus.Delete;
                    data.allowEdit = false;
                    data.allowDelete = false;
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.TaxConditionDeleted, 'autoclose': true });
                }
                else {
                    ConditionTax.GetConditionsByTaxId();
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                    $("#lvConditionTax").UifListView("refresh");
                }
            }
            else {
                ConditionTax.GetConditionsByTaxId();
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                $("#lvConditionTax").UifListView("refresh");
            }
        });

    }
    static CancelCondition() {
        ConditionTax.ClearCondition();
    }
    static ClearCondition() {
        $("#IdCondition").val('');
        $("#DescriptionCondition").val('');
        $('#NationalRate').prop('checked', false);
        $('#Independent').prop('checked', false);
        $('#Enabled').prop('checked', false);
        conditionIndex = null;
        conditionIdSelected = null;
        ClearValidation("#formConditionTax");
    }
    static ExitCondition() {
        router.rlite.run("prtTax");
    }

    static SaveConditionBD() {
        var data = $("#lvConditionTax").UifListView("getData");
        if (data != null && data !== undefined) {
            var taxCode = taxModel.Id;
            var conditionTaxModel = [];
            $.each(data, function (index, value) {
                conditionTaxModel.push({
                    "IdCondition": data[index].IdCondition,
                    "IdTax": taxCode,
                    "DescriptionCondition": data[index].DescriptionCondition,
                    "NationalRate": data[index].NationalRate,
                    "Independent": data[index].Independent,
                    "Enabled": data[index].Enabled
                });
            });
            ConditionTaxRequests.SaveTaxCondition(conditionTaxModel).done(function (response) {
                if (response.success) {
                    if (response.result.length > 0) {
                        $("#lvConditionTax").UifListView("refresh");
                        ConditionTax.GetConditionsByTaxId();
                        ConditionTax.ClearCondition();
                        $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.TaxConditionSaved, 'autoclose': true });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RecordError, 'autoclose': true });
                        conditionTaxModel = [];
                    }
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorEmpty, 'autoclose': true });
        }
    }

    static GetConditionsByTaxId() {
        var taxCode = taxModel.Id;
        ConditionTaxRequests.GetConditionsByTaxId(taxCode).done(function (response) {
            if (response.success) {
                var data = response.result;
                if (data.length > 0) {
                    taxModel.TaxConditions = [];
                    $.each(data, function (index, value) {
                        data[index].Status = ParametrizationStatus.Original;
                        $("#lvConditionTax").UifListView("addItem", data[index]);
                        taxModel.TaxConditions.push({
                            "Id": data[index].IdCondition, "IdTax": taxCode, "Description": data[index].DescriptionCondition,
                            "HasNationalRate": data[index].NationalRate, "IsIndependent": data[index].Independent, "IsDefault": data[index].Enabled,
                        });
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.QueryNotData, 'autoclose': true });
                }
            }
        });
    }
}