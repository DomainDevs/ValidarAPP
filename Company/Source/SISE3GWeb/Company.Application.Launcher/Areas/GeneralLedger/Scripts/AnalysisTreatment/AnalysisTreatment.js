/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var idAnalysisTreatment = 0;

var analysisTreatmentModel = {
    AnalysisTreatmentId: 0,
    Description: ""
};

$(document).ready(function () {
    $("#analysisTreatmentModal").on('click', '#saveButton', function () {
        $("#analysisTreatmentModal").find("#addAnalysisTreatment").validate();

        if (!$('#analysisTreatmentModal').find("#addAnalysisTreatment").valid()) {
            return false;
        }
        else {
            analysisTreatmentModel.AnalysisTreatmentId = $('#analysisTreatmentModal').find('#AnalysisTreatmentId').val();
            analysisTreatmentModel.Description = $('#analysisTreatmentModal').find("#Description").val();

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisTreatment/SaveAnalysisTreatment",
                data: JSON.stringify({ "analysisTreatment": analysisTreatmentModel }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#analysisTreatmentModal').UifModal('hide');
                    $("#AnalysisTreatmentListView").UifListView("refresh");

                    if (analysisTreatmentModel.AnalysisTreatmentId == 0) {
                        $("#alertAnalysisTreatment").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertAnalysisTreatment").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    });

    if ($("#hiddenAnalysisTreatment").val() != undefined) {
        $("#btnDeleteModal").on('click', function () {
            $('#modalDelete').modal('hide');
            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisTreatment/DeleteAnalysisTreatment",
                data: JSON.stringify({ "id": idAnalysisTreatment }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {

                if (data.success == true) {
                    $("#AnalysisTreatmentListView").UifListView("refresh");
                    $("#alertAnalysisTreatment").UifAlert('show', Resources.DeleteSuccessfully, "success");
                }
                else {
                    $("#alertAnalysisTreatment").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }
            });
        });
    }
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
$('#AnalysisTreatmentListView').on("rowAdd", function (event) {
    $("#alertAnalysisTreatment").UifAlert('hide');
    $('#analysisTreatmentModal').UifModal('show', GL_ROOT + "AnalysisTreatment/AnalysisTreatmentModal", Resources.AddRecord);
});

$('#AnalysisTreatmentListView').on('rowEdit', function (event, data) {
    $("#alertAnalysisTreatment").UifAlert('hide');
    $('#analysisTreatmentModal').UifModal('show', GL_ROOT + "AnalysisTreatment/AnalysisTreatmentModal?id=" + data.AnalysisTreatmentId, Resources.EditRecord);
});

$('#AnalysisTreatmentListView').on('rowDelete', function (event, data) {
    $("#alertAnalysisTreatment").UifAlert('hide');
    $('#modalDelete').modal('show');
    idAnalysisTreatment = data.AnalysisTreatmentId;
});


/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE GRIDS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#hiddenAnalysisTreatment").val() != undefined) {
    $("#AnalysisTreatmentListView").UifListView(
        {
            source: GL_ROOT + "AnalysisTreatment/GetAnalysisTreatment",
            customDelete: true,
            customAdd: true,
            customEdit: true,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#listTemplateAnalysis"
        });
}