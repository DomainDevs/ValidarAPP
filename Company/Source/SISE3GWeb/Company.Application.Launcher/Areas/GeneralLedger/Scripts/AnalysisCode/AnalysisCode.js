/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var idAnalysisCode = 0;
var ModelAnalysisCode = {
    Id: 0,
    Description: "",
    CheckBalance: false,
    CheckModule: false,
    AccountingNatureId: 1
};

$(document).ready(function () {
    $("#modalAnalysisCodes").on('click', '#saveButton', function () {

        if (!$("#modalAnalysisCodes").find("#addAnalysisCode").valid()) {
            return false;
        }
        else {
            ModelAnalysisCode.Id = $("#modalAnalysisCodes").find('#Id').val();
            ModelAnalysisCode.Description = $("#modalAnalysisCodes").find("#Description").val();
            ModelAnalysisCode.CheckBalance = ($('#modalAnalysisCodes').find('#CheckBalance').is(':checked'));
            ModelAnalysisCode.CheckModule = ($('#modalAnalysisCodes').find('#CheckModule').is(':checked'));
            ModelAnalysisCode.AccountingNatureId = $("#modalAnalysisCodes").find("#AccountingNatureId").val();

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisCode/SaveAnalysisCodes",
                data: JSON.stringify({ "analysisCodesModel": ModelAnalysisCode }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#modalAnalysisCodes').UifModal('hide');
                    $("#listViewAnalysisCodes").UifListView("refresh");

                    if (ModelAnalysisCode.Id == 0) {
                        $("#alertAnalysisCodes").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertAnalysisCodes").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    });

    if ($("#ViewBagAnalysisCode").val() != undefined) {
        $('#modalDelete').find("#btnDeleteModal").on('click', function () {
            $('#modalDelete').modal('hide');

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisCode/DeleteAnalysisCodes",
                data: JSON.stringify({ "id": idAnalysisCode }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data == true) {
                    $("#listViewAnalysisCodes").UifListView("refresh");
                    $("#alertAnalysisCodes").UifAlert('show', Resources.DeleteSuccessfully, "success");
                }
                else {
                    $("#alertAnalysisCodes").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
                }
            });
        });
    }
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$('#listViewAnalysisCodes').on("rowAdd", function (event) {
    $("#alertAnalysisCodes").UifAlert('hide');
    $('#modalAnalysisCodes').UifModal('show', GL_ROOT + "AnalysisCode/AnalysisCodeModal", Resources.AddRecord);
});

$('#listViewAnalysisCodes').on('rowEdit', function (event, data) {
    $("#alertAnalysisCodes").UifAlert('hide');
    $('#modalAnalysisCodes').UifModal('show', GL_ROOT + "AnalysisCode/AnalysisCodeModal?id=" + data.Id, Resources.EditRecord);
});

$('#listViewAnalysisCodes').on('rowDelete', function (event, data) {
    $("#alertAnalysisCodes").UifAlert('hide');
    $('#modalDelete').modal('show');
    idAnalysisCode = data.Id;
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE GRIDS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#ViewBagAnalysisCode").val() != undefined) {
    $("#listViewAnalysisCodes").UifListView({
        source: GL_ROOT + "AnalysisCode/GetAnalysisCodes",
        customDelete: true,
        customAdd: true,
        customEdit: true,
        add: true,
        edit: true,
        delete: true,
        displayTemplate: "#display-templateAnalysisCode",
    });
}
