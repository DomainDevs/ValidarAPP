
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var idAnaysisConcept = 0;

var analysisConceptModel = {
    AnalysisConceptId: 0,
    Description: "",
    AnalysisTreatmentId: 0
};

$(document).ready(function () {
    $("#analysisConceptModal").on('click', '#saveButton', function () {
        $("#analysisConceptModal").find("#addAnalysisConcept").validate();

        if (!$('#analysisConceptModal').find("#addAnalysisConcept").valid()) {
            return false;
        }
        else {
            analysisConceptModel.AnalysisConceptId = $('#analysisConceptModal').find('#AnalysisConceptId').val();
            analysisConceptModel.Description = $('#analysisConceptModal').find("#Description").val();
            analysisConceptModel.AnalysisTreatmentId = $('#analysisConceptModal').find("#AnalysisTreatmentId").val();

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisConcept/SaveAnalysisConcept",
                data: JSON.stringify({ "analysisConceptModel": analysisConceptModel }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#analysisConceptModal').UifModal('hide');
                    $("#AnalysisConceptListView").UifListView("refresh");

                    if (analysisConceptModel.AnalysisConceptId == 0) {
                        $("#alertAnalysisConcept").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertAnalysisConcept").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    });

    if ($("#hiddenAnalysisConcept").val() != undefined) {
        $("#btnDeleteModal").on('click', function () {
            $('#modalDelete').modal('hide');
            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisConcept/DeleteAnalysisConcept",
                data: JSON.stringify({ "id": idAnaysisConcept }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {

                if (data.success == true) {
                    $("#AnalysisConceptListView").UifListView("refresh");
                    $("#alertAnalysisConcept").UifAlert('show', Resources.DeleteSuccessfully, "success");
                }
                else {
                    $("#alertAnalysisConcept").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }
            });
        });
    }
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$('#AnalysisConceptListView').on("rowAdd", function (event) {
    $("#alertAnalysisConcept").UifAlert('hide');
    $('#analysisConceptModal').UifModal('show', GL_ROOT + "AnalysisConcept/AnalysisConceptModal", Resources.AddRecord);
});

$('#AnalysisConceptListView').on('rowEdit', function (event, data) {
    $("#alertAnalysisConcept").UifAlert('hide');
    $('#analysisConceptModal').UifModal('show', GL_ROOT + "AnalysisConcept/AnalysisConceptModal?id=" + data.AnalysisConceptId, Resources.EditRecord);
});

$('#AnalysisConceptListView').on('rowDelete', function (event, data) {
    $("#alertAnalysisConcept").UifAlert('hide');
    $('#modalDelete').modal('show');
    idAnaysisConcept = data.AnalysisConceptId;
});





/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE LISTVIEWS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#hiddenAnalysisConcept").val() != undefined) {
    $("#AnalysisConceptListView").UifListView(
        {
            source: GL_ROOT + "AnalysisConcept/GetAnalysisConcept",
            customDelete: true,
            customAdd: true,
            customEdit: true,
            add: true,
            edit: true,
            delete: true,
            displayTemplate: "#listTemplate"
        });
}