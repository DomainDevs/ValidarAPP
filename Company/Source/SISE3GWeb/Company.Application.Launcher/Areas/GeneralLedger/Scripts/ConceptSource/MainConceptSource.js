
var conceptSourceId = 0;

var conceptSourceModel = {
    Id: 0,
    Description: ""
};


var SetDataSource = function (data) {

    conceptSourceModel.Id = data.Id;
    conceptSourceModel.Description = data.Description;
    return conceptSourceModel;

};

    var saveCallbackConcept = function (deferred, data) {
        $("#mainAlert").UifAlert('hide');
        $.ajax({
            type: "POST",
            url: GL_ROOT + "ConceptSource/SaveConceptSource",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $("#mainAlert").UifAlert('show', Resources.SaveSuccessfully, "success");
            } else {
                deferred.reject();
                $("#mainAlert").UifAlert('show', Resources.SaveError, "danger");
            }
        });
};

    var editCallbackConcept = function (deferred, data) {
        $("#mainAlert").UifAlert('hide');

        $.ajax({
            type: "POST",
            url: GL_ROOT + "ConceptSource/SaveConceptSource",
            data: JSON.stringify({
                "conceptSourceModel": SetDataSource(data)
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve(data.result);
                 $("#conceptSourceListView").UifListView("refresh");
                $("#mainAlert").UifAlert('show', Resources.EditSuccessfully, "success");

            } else {
                deferred.reject();
                $("#mainAlert").UifAlert('show', Resources.SaveError, "danger");
            }
        });
    };


    var deleteCallbackConcept = function (deferred, data) {
        $("#mainAlert").UifAlert('hide');
        $.ajax({
            type: "POST",
            url: GL_ROOT + "ConceptSource/DeleteConceptSource",
            data: JSON.stringify({
                "conceptSourceModel": SetDataSource(data)
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $("#conceptSourceListView").UifListView("refresh");
                $("#mainAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
            } else {
                deferred.reject();
                $("#mainAlert").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
            }
        });
};

    setTimeout(function () {
        $("#conceptSourceListView").UifListView({
            source: GL_ROOT + "ConceptSource/GetConceptSources",
            customDelete: false,
            customAdd: false,
            customEdit: false,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#conceptSourceListTemplate",
            addTemplate: '#add-template',
            addCallback: saveCallbackConcept,
            editCallback: editCallbackConcept,
            deleteCallback: deleteCallbackConcept
        });
    }, 500);




   

    
