/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var idAnalysisConceptByCode = 0;
var resultAnalysisConcept = 0;
var conceptIdAnalysisConcept = 0;

$(document).ready(function () {
    $('#modalAnalysisConceptByCode').on('hidden.bs.modal', function (e) {
        conceptIdAnalysisConcept = 0;
    })
    $("#modalAnalysisConceptByCode").find('#tblAnalysisConceptByCode').on('rowSelected', function (event, data, position) {
        $("#alertAnalysisConceptByCode").UifAlert('hide');
        $("#alertTable").UifAlert('hide');
        conceptIdAnalysisConcept = data.AnalysisConceptId;
    });

    $("#modalAnalysisConceptByCode").find("#saveButton").click(function () {
        if (conceptIdAnalysisConcept == 0) {
            $("#alertTable").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "danger");
        }
        else {

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisConceptByCode/SaveAnalysisConceptAnalysis",
                data: JSON.stringify({ "analysisId": idAnalysisConceptByCode, "analysisConceptId": conceptIdAnalysisConcept }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#modalAnalysisConceptByCode').UifModal('hide');
                    $("#listViewAnalysisConceptByCode").UifListView("refresh");

                    if (data == true) {
                        $("#alertAnalysisConceptByCode").UifAlert('show', Resources.AddSuccessfully, "success");
                    }
                    else {
                        $("#alertAnalysisConceptByCode").UifAlert('show', Resources.SaveError, "danger");
                    }
                }
            });
        }
    });
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
$("#listViewAnalysisConceptByCode").UifListView(
    {
        source: null,
        customDelete: false,
        customAdd: true,
        customEdit: false,
        add: true,
        edit: false,
        delete: true,
        displayTemplate: "#listTemplateAnalysisConcept",
        deleteCallback: deleteCallback
    });

$('#dllCodes').on('itemSelected', function (event, selectedItem) {
    $("#alertAnalysisConceptByCode").UifAlert('hide');
    $("#alertTable").UifAlert('hide');
    if (selectedItem.Id > 0) {

        $("#listViewAnalysisConceptByCode").UifListView(
            {
                source: GL_ROOT + "AnalysisConceptByCode/GetAnalysisConceptByCodeId?id=" + selectedItem.Id,
                customDelete: false,
                customAdd: true,
                customEdit: false,
                add: true,
                edit: false,
                delete: true,
                displayTemplate: "#listTemplateAnalysisConcept",
                deleteCallback: deleteCallback
            });
    }
});

$('#listViewAnalysisConceptByCode').on("rowAdd", function (event) {
    $("#alertAnalysisConceptByCode").UifAlert('hide');
    $("#alertTable").UifAlert('hide');

    if ($('#dllCodes').val() == "") {
        $("#alertAnalysisConceptByCode").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "danger");
    }
    else {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "AnalysisConceptByCode/GetCountRemainingAnalysisConcepts",
            data: JSON.stringify({ "id": $('#dllCodes').val() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                resultAnalysisConcept = data;
            }
        }).done(function (data) {

            if (resultAnalysisConcept == 0) {
                $("#alertAnalysisConceptByCode").UifAlert('show', Resources.MessageAnalysisConcept, "danger");
            }
            else {
                idAnalysisConceptByCode = $('#dllCodes').val();
                $('#modalAnalysisConceptByCode').UifModal('showLocal', Resources.AddConcept + " - " + Resources.Code + ": " + idAnalysisConceptByCode);
                var controller = GL_ROOT + "AnalysisConceptByCode/GetRemainingAnalysisConcepts?id=" + idAnalysisConceptByCode;
                $("#tblAnalysisConceptByCode").UifDataTable({ source: controller });
            }
        });
    }
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE GRIDS
/*--------------------------------------------------------------------------------------------------------------------------------------------------------*/

var deleteCallback = function (deferred, data) {
    $.ajax({
        type: "POST",
        url: GL_ROOT + "AnalysisConceptByCode/DeleteAnalysisConceptAnalysis",
        data: JSON.stringify({ "id": data.AnalysisConceptAnalysisId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        $("#listViewAnalysisConceptByCode").UifListView("refresh");
        if (data.success) {
            deferred.resolve();
            $("#alertAnalysisConceptByCode").UifAlert('show', Resources.DeleteSuccessfully, "success");
        }
        else {
            deferred.reject();
            $("#alertAnalysisConceptByCode").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
        }
    });
};