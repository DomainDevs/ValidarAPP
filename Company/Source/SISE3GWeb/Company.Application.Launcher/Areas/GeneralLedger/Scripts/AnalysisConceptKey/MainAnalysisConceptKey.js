/**
    * @file   MainAnalysisConceptKey.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var analysisConceptKeyId = 0;
var analysisConceptId2 = 0;

var analysisConceptKeyModel = {
    Id: 0,
    AnalysisConceptId: 0,
    TableName: "",
    ColumnName: "",
    Description: ""
};

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainAnalysisConceptKey();
});

class MainAnalysisConceptKey extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
       

    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#AnalysisConcept").on("binded", this.BindedAnalysisConcept);
        $("#ConceptKeyTable").on('rowAdd', this.RowAddConceptKey);
        $("#ConceptKeyTable").on('rowEdit', this.RowEditConceptKey);
        $("#ConceptKeyTable").on('rowDelete', this.RowDeleteConceptKey);
        $("#modalAdd").find("#SaveAddConceptKey").on('click', this.SaveAddConceptKey);
        $("#modalEdit").find("#SaveEditConceptKey").on('click', this.SaveEditConceptKey);
        $("#deleteKeyModal").on('click', this.DeleteConceptKey);
        $("#AnalysisConcept").on("itemSelected", this.SelectAnalysisConcept);
    }

    /**
        * Setea elconcepto por default una vez que esta cargado.
        *
        */
    BindedAnalysisConcept() {
        $("#AnalysisConcept").val("");
    }

    /**
        * Permite agregar una clave de concepto.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con valores de la clave seleccionada.
        */
    RowAddConceptKey(event, data) {
        $("#formConceptKey").validate();
        if ($("#formConceptKey").valid()) {
            analysisConceptKeyId = 0;
            $("#alert").UifAlert('hide');
            $('#modalAdd').UifModal('showLocal', Resources.AddAnalysisConceptKey);
        }
    }

    /**
        * Permite editar una clave de concepto de análisis.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores de la clave seleccionada.
        * @param {Number} position - Indice de la clave seleccionada.
        */
    RowEditConceptKey(event, data, position) {
        $('#alert').UifAlert('hide');
        analysisConceptKeyId = data.Id;
        $("#editConceptKeyForm").find("#TableName").val(data.TableName);
        $("#editConceptKeyForm").find("#ColumnName").val(data.ColumnName);
        $("#editConceptKeyForm").find("#Description").val(data.Description);

        $('#modalEdit').UifModal('showLocal', Resources.EditAnalysisConceptKey);
    }

    /**
        * Permite eliminar una clave de concepto de análisis.
        *
        * @param {String} event    - Eliminar.
        * @param {Object} data     - Objeto con valores de la clave seleccionada.
        */
    RowDeleteConceptKey(event, data) {
        analysisConceptKeyId = data.Id;
        analysisConceptId2 = data.AnalysisConceptId;
        $('#alert').UifAlert('hide');
        $('#modalDeleteConceptKey').appendTo("body").modal('show');
    }

    /**
        * Graba una clave de concepto de análisis.
        *
        */
    SaveAddConceptKey() {
        $("#alertForm").UifAlert('hide');

        $("#addConceptKeyForm").validate();
        if ($("#addConceptKeyForm").valid()) {
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                },
                message: "<h1>" + Resources.MessageWaiting + "</h1>"
            });

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisConcept/SaveAnalysisConceptKey",
                data: JSON.stringify({
                    "analysisConceptKeyModel": MainAnalysisConceptKey.SetDataAddConceptKey()
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        $('#addConceptKeyForm').formReset();
                        $('#modalAdd').modal('hide');
                        $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
                        var controller = GL_ROOT + "AnalysisConcept/GetAnalysisConceptKeys?analysisConceptId=" + $("#AnalysisConcept").val();
                        $("#ConceptKeyTable").UifDataTable({ source: controller });
                    }
                    else {
                        $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger");
                    }
                    $.unblockUI();
                }
            });
        }
    }

    /**
        * Graba una clave de concepto de análisis.
        *
        */
    SaveEditConceptKey() {
        $("#alertForm").UifAlert('hide');

        $("#editConceptKeyForm").validate();
        if ($("#editConceptKeyForm").valid()) {
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                },
                message: "<h1>" + Resources.MessageWaiting + "</h1>"
            });

            $.ajax({
                type: "POST",
                url: GL_ROOT + "AnalysisConcept/SaveAnalysisConceptKey",
                data: JSON.stringify({
                    "analysisConceptKeyModel": MainAnalysisConceptKey.SetDataEditConceptKey()
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        $('#editConceptKeyForm').formReset();
                        $('#modalEdit').modal('hide');
                        $("#alert").UifAlert('show', Resources.SaveSuccessfully, "success");
                        var controller = GL_ROOT + "AnalysisConcept/GetAnalysisConceptKeys?analysisConceptId=" + $("#AnalysisConcept").val();
                        $("#ConceptKeyTable").UifDataTable({ source: controller });
                    }
                    else {
                        $("#alert").UifAlert('show', Resources.ErrorTransaction, "danger");
                    }
                    $.unblockUI();
                }
            });
        }
    }

    /**
        * Elimina una clave de concepto de análisis.
        *
        */
    DeleteConceptKey() {

        $('#modalDeleteConceptKey').modal('hide');

        $.ajax({
            type: "POST",
            url: GL_ROOT + "AnalysisConcept/DeleteAnalysisConceptKey",
            data: JSON.stringify(
                {
                    "analysisConceptKeyId": analysisConceptKeyId,
                    "analysisConceptId": analysisConceptId2
                }
            ),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.success) {
                    $("#alert").UifAlert('show', Resources.DeleteSuccessfully, "success");
                    var controller = GL_ROOT + "AnalysisConcept/GetAnalysisConceptKeys?analysisConceptId=" + $("#AnalysisConcept").val();
                    $("#ConceptKeyTable").UifDataTable({ source: controller });
                }
                else {
                    if (data.result === "1") {
                        $("#alert").UifAlert('show', Resources.YouCanNotDeleteTheRecord, "warning");
                    }
                    else {
                        $("#alert").UifAlert('show', data.result, "danger");
                    }
                }
            }
        });
    }

    /**
        * Permite obtener las claves dado el identificador de cocnepto de análisis.
        *
        * @param {String} event        - Eliminar.
        * @param {Object} selectedItem - Objeto con valores de la clave seleccionada.
        */
    SelectAnalysisConcept(event, selectedItem) {
        $('#alert').UifAlert('hide');
        if (selectedItem.Id > 0) {
         //   var controller = GL_ROOT + "AnalysisConcept/GetAnalysisConceptKeys";
           var controller = GL_ROOT + "AnalysisConcept/GetAnalysisConceptKeys?analysisConceptId=" + selectedItem.Id;
            $("#ConceptKeyTable").UifDataTable({ source: controller });
        }
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /*
     * *
        * Setea la data de la clave de concepto de análisis. Para Guadar 
        *
        */
    static SetDataAddConceptKey() {
        analysisConceptKeyModel.Id = analysisConceptKeyId;
        analysisConceptKeyModel.AnalysisConceptId = $("#AnalysisConcept").val();
        analysisConceptKeyModel.TableName = $("#addConceptKeyForm").find("#TableName").val();
        analysisConceptKeyModel.ColumnName = $("#addConceptKeyForm").find("#ColumnName").val();
        analysisConceptKeyModel.Description = $("#addConceptKeyForm").find("#Description").val();
        return analysisConceptKeyModel;
     
    }

    /*
    *
    * Setea la data de la clave de concepto de análisis. Para Editar
    *
    */
    static SetDataEditConceptKey() {
        analysisConceptKeyModel.Id = analysisConceptKeyId;
        analysisConceptKeyModel.AnalysisConceptId = $("#AnalysisConcept").val();
        analysisConceptKeyModel.TableName = $("#editConceptKeyForm").find("#TableName").val();
        analysisConceptKeyModel.ColumnName = $("#editConceptKeyForm").find("#ColumnName").val();
        analysisConceptKeyModel.Description = $("#editConceptKeyForm").find("#Description").val();
        return analysisConceptKeyModel;
    }

}