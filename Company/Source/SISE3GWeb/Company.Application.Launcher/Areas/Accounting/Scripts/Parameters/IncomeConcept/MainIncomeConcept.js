
    var saveCallbackConcept = function (deferred, data) {
        $("#alertIncomeConcept").UifAlert('hide');
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/MainIncomeConcept",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $("#alertIncomeConcept").UifAlert('show', Resources.SaveSuccessfully, "success");
            } else {
                deferred.reject();
                $("#alertIncomeConcept").UifAlert('show', Resources.SaveError, "danger");
            }
        });
    };
    var editCallbackConcept = function (deferred, data) {
        $("#alertIncomeConcept").UifAlert('hide');
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/UpdateIncomeConcept",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve(data.result);
                $("#alertIncomeConcept").UifAlert('show', Resources.EditSuccessfully, "success");
            } else {
                deferred.reject();
                $("#alertIncomeConcept").UifAlert('show', Resources.SaveError, "danger");
            }
        });
    };

    var deleteCallbackConcept = function (deferred, data) {
        $("#alertIncomeConcept").UifAlert('hide');
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Parameters/DeleteIncomeConcept",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $("#alertIncomeConcept").UifAlert('show', Resources.DeleteSuccessfully, "success");
            } else {
                deferred.reject();
                $("#alertIncomeConcept").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
            }
        });
    };
    setTimeout(function () {
        $("#incomeConceptListView").UifListView({
            source: ACC_ROOT + "Parameters/GetIncomeConcepts",
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#IncomeConceptListTemplate",
            addTemplate: '#add-template',
            addCallback: saveCallbackConcept,
            editCallback: editCallbackConcept,
            deleteCallback: deleteCallbackConcept
        });
    }, 500);


