/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var idDestination = 0;

var destinationModel = {
    DestinationId: 0,
    Description: ""
};

$(document).ready(function () {
    $("#destinationModal").on('click', '#saveButton', function () {

        $("#destinationModal").find("#addDestination").validate();

        if (!$('#destinationModal').find("#addDestination").valid()) {
            return false;
        }
        else {
            destinationModel.DestinationId = $('#destinationModal').find('#DestinationId').val();
            destinationModel.Description = $('#destinationModal').find("#Description").val();

            $.ajax({
                type: "POST",
                url: GL_ROOT + "Destination/SaveDestination",
                data: JSON.stringify({ "destination": destinationModel }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#destinationModal').UifModal('hide');
                    $("#destinationListView").UifListView("refresh");

                    if (destinationModel.DestinationId == 0) {
                        $("#alertDestination").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertDestination").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    });

    if ($("#ViewBagDestination").val() != undefined) {
        $('#modalDelete').find("#btnDeleteModal").on('click', function () {
            $('#modalDelete').modal('hide');
            $.ajax({
                type: "POST",
                url: GL_ROOT + "Destination/DeleteDestination",
                data: JSON.stringify({ "id": idDestination }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success == true) {
                    $("#alertDestination").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    $("#destinationListView").UifListView("refresh");
                }
                else {
                    $("#alertDestination").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }
            });
        });
    }

});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$('#destinationListView').on("rowAdd", function (event) {
    $("#alertDestination").UifAlert('hide');
    $('#destinationModal').UifModal('show', GL_ROOT + "Destination/DestinationModal", Resources.AddRecord);
});

$('#destinationListView').on('rowEdit', function (event, data) {
    $("#alertDestination").UifAlert('hide');
    $('#destinationModal').UifModal('show', GL_ROOT + "Destination/DestinationModal?id=" + data.DestinationId, Resources.EditRecord);
});

$('#destinationListView').on('rowDelete', function (event, data) {
    $("#alertDestination").UifAlert('hide');
    $('#modalDelete').UifModal('show');
    idDestination = data.DestinationId;
});





/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE LISTVIEWS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#ViewBagDestination").val() != undefined) {
    $("#destinationListView").UifListView(
        {
            source: GL_ROOT + "Destination/GetDestinations",
            customDelete: true,
            customAdd: true,
            customEdit: true,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#listTemplateDestination"
        });
}
