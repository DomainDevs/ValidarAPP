
var idConceptSource = 0;

var movementTypeModel = {
    Id: 0,
    ConceptSourceId : 0,
    Description: ""
};


$('#ConceptSources').on('itemSelected', function (event, selectedItem) {
    $("#mainAlert").UifAlert('hide');
    if (selectedItem.Id > 0) {
                
       // if ($("#movementTypeListView").UifListView("getData").length> 0) {
            //$("#movementTypeListView").UifListView("refresh");
        //}
        idConceptSource = selectedItem.Id;
        $("#movementTypeListView").UifListView(
            {
                source: GL_ROOT + "MovementType/GetMovementTypesByConceptSourceId?conceptSourceId=" + selectedItem.Id,
                customAdd: false,
                customEdit: false,
                add: true,
                edit: true,
                delete: true,
                displayTemplate: "#movementTypeListTemplate",
                addTemplate: '#add-template',
                addCallback: SaveCallbackMovementType,
                editCallback: EditCallbackMovementType,
                deleteCallback: DeleteCallbackMovementType
            });
    }
});


var SetDataMovement = function (data) {
    //var q = data.ConceptSourceId;
    movementTypeModel.Id = data.Id;
    movementTypeModel.ConceptSourceId = idConceptSource; 
    movementTypeModel.Description = data.Description;
    return movementTypeModel;

};

var SaveCallbackMovementType = function (deferred, data) {
        $("#mainAlert").UifAlert('hide');
        $.ajax({
            type: "POST",
            url: GL_ROOT + "MovementType/SaveMovementType",
            data: JSON.stringify({
                "movementTypeModel": SetDataMovement(data)
            }),
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

var EditCallbackMovementType = function (deferred, data) {
        $("#mainAlert").UifAlert('hide');

        $.ajax({
            type: "POST",
            url: GL_ROOT + "MovementType/SaveMovementType",
            data: JSON.stringify({
                "movementTypeModel": SetDataMovement(data)
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve(data.result);
                $("#movementTypeListView").UifListView("refresh");
                $("#mainAlert").UifAlert('show', Resources.EditSuccessfully, "success");

            } else {
                deferred.reject();
                $("#mainAlert").UifAlert('show', Resources.SaveError, "danger");
            }
        });
    };


var DeleteCallbackMovementType = function (deferred, data) {
        $("#mainAlert").UifAlert('hide');
        $.ajax({
            type: "POST",
            url: GL_ROOT + "MovementType/DeleteMovementType",
            data: JSON.stringify({
                "movementTypeModel": SetDataMovement(data)
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $("#movementTypeListView").UifListView("refresh");
                $("#mainAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");
            } else {
                deferred.reject();
                $("#mainAlert").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
            }
        });
};

   




   

    
