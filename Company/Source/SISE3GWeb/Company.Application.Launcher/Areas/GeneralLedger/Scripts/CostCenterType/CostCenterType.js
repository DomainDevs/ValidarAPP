
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var idCostCenterType = 0;
var modelCostCenterType = {
    CostCenterTypeId: 0,
    Description: ""
};

$(document).ready(function () {
    $("#costCenterTypeModal").on('click', '#saveButton', function () {
        $("#costCenterTypeModal").find("#addCostCenterType").validate();

        if (!$('#costCenterTypeModal').find("#addCostCenterType").valid()) {
            return false;
        }
        else {
            modelCostCenterType.CostCenterTypeId = $('#costCenterTypeModal').find('#CostCenterTypeId').val();
            modelCostCenterType.Description = $('#costCenterTypeModal').find("#Description").val();

            $.ajax({
                type: "POST",
                url: GL_ROOT + "CostCenterType/SaveCostCenterType",
                data: JSON.stringify({ "costCenterType": modelCostCenterType }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#costCenterTypeModal').UifModal('hide');
                    $("#costCenterTypeListView").UifListView("refresh");

                    if (modelCostCenterType.CostCenterTypeId == 0) {
                        $("#alertCostCenterType").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertCostCenterType").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    });

    if ($("#ViewBagCenterType").val() != undefined) {
        $('#modalDelete').find("#btnDeleteModal").on('click', function () {
            $('#modalDelete').modal('hide');

            $.ajax({
                type: "POST",
                url: GL_ROOT + "CostCenterType/DeleteCostCenterType",
                data: JSON.stringify({ "id": idCostCenterType }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.result == true) {
                    $("#alertCostCenterType").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    $("#costCenterTypeListView").UifListView("refresh");
                }
                else {
                    $("#alertCostCenterType").UifAlert('show', Resources.YouCanNotDeleteTheRecord + " de " + Resources.CostCenter, "danger");
                }
            });
        });
    }

});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$('#costCenterTypeListView').on("rowAdd", function (event) {
    $("#alertCostCenterType").UifAlert('hide');
    $('#costCenterTypeModal').UifModal('show', GL_ROOT + "CostCenterType/CostCenterTypeModal", Resources.AddRecord);
});

$('#costCenterTypeListView').on('rowEdit', function (event, data) {
    $("#alertCostCenterType").UifAlert('hide');
    $('#costCenterTypeModal').UifModal('show', GL_ROOT + "CostCenterType/CostCenterTypeModal?id=" + data.CostCenterTypeId, Resources.EditRecord);
});

$('#costCenterTypeListView').on('rowDelete', function (event, data) {
    $("#alertCostCenterType").UifAlert('hide');
    $('#modalDelete').modal('show');
    idCostCenterType = data.CostCenterTypeId;
});





/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE GRIDS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#ViewBagCenterType").val() != undefined) {
    $("#costCenterTypeListView").UifListView(
        {
            source: GL_ROOT + "CostCenterType/GetCostCentersTypes",
            customDelete: true,
            customAdd: true,
            customEdit: true,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#listTemplateCostCenter"
        });
}
