/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var idCostCenter = 0;

var ModelCostCenter = {
    Id: 0,
    Description: "",
    IsProrated: false,
    CostCenterTypeId: 0
};

$(document).ready(function () {
    $("#costCenterModal").on('click', '#saveButton', function () {
        $("#alertCostCenter").UifAlert('hide');
        $("#costCenterModal").find("#addCostCenter").validate();

        if (!$('#costCenterModal').find("#addCostCenter").valid()) {
            return false;
        }
        else {
            ModelCostCenter.Id = $('#costCenterModal').find('#Id').val();
            ModelCostCenter.Description = $('#costCenterModal').find("#Description").val();
            ModelCostCenter.IsProrated = ($('#costCenterModal').find('#IsProrated').is(':checked'));
            ModelCostCenter.CostCenterTypeId = $('#costCenterModal').find("#CostCenterTypeId").val();

            $.ajax({
                type: "POST",
                url: GL_ROOT + "CostCenter/SaveCostCenter",
                data: JSON.stringify({ "costCenterModel": ModelCostCenter }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#costCenterModal').UifModal('hide');
                    $("#costCenterListView").UifListView("refresh");

                    if (ModelCostCenter.Id == 0) {
                        $("#alertCostCenter").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertCostCenter").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    });

    if ($("#ViewBagCostCenter").val() != undefined) {
        $('#modalDelete').find("#btnDeleteModal").on('click', function () {
            $("#alertCostCenter").UifAlert('hide');
            $('#modalDelete').modal('hide');
            $.ajax({
                type: "POST",
                url: GL_ROOT + "CostCenter/DeleteCostCenter",
                data: JSON.stringify({ "id": idCostCenter }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data == true) {
                    $("#costCenterListView").UifListView("refresh");
                    $("#alertCostCenter").UifAlert('show', Resources.DeleteSuccessfully, "success");
                }
                else {
                    $("#alertCostCenter").UifAlert('show', Resources.CostCenterValidation, "danger");
                }
            });
        });
    }

});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$('#costCenterListView').on("rowAdd", function (event) {
    $("#alertCostCenter").UifAlert('hide');
    $('#costCenterModal').UifModal('show', GL_ROOT + "CostCenter/CostCenterModal", Resources.AddRecord);
});

$('#costCenterListView').on('rowEdit', function (event, data) {
    $("#alertCostCenter").UifAlert('hide');
    $('#costCenterModal').UifModal('show', GL_ROOT + "CostCenter/CostCenterModal?id=" + data.Id, Resources.EditRecord);
});

$('#costCenterListView').on('rowDelete', function (event, data) {
    $("#alertCostCenter").UifAlert('hide');
    $('#modalDelete').modal('show');
    idCostCenter = data.Id;
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE LISTVIEWS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#ViewBagCostCenter").val() != undefined) {
    $("#costCenterListView").UifListView(
        {
            source: GL_ROOT + "CostCenter/GetCostCenters",
            customDelete: true,
            customAdd: true,
            customEdit: true,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#display-templateCenter",
        });
}
