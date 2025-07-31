$.ajaxSetup({ async: false });
var ViewScript = function (scriptId) {
     
    MiControlViewScript.GetScript(scriptId);
    MiControlViewScript.Pintar();

    $("#ModalViewScript").UifModal("showLocal", Resources.Language.ViewGuion);

}

var ViewScriptWithData = function (data) {

    ScriptViewComposite = {};
    ScriptViewComposite = data;
    MiControlViewScript.Pintar();

    $("#ModalViewScript").UifModal("showLocal", Resources.Language.ViewGuion);

}

var MiControlViewScript = {
    GetScript: function (scriptId) {
        htmlViewScript = "";
        $.ajax({
            type: "POST",
            url: rootPath + "Scripts/GetScriptComposite?ScriptId=" + scriptId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                ScriptViewComposite = {}
                ScriptViewComposite = data;
            }
        })
    },
    ArmarControl: function (nodes, nodeId, IdResuesta2) {

        $.each(nodes[nodeId - 1].Questions, function (index, value) {

            //htmlViewScript += '<div>' + value.Description;//pregunta NodeId  + QuestionId + OrdenNum
            var IdPregunta = "PreguntaView-" + nodeId + "-" + value.QuestionId + "-" + (index + 1) + "-" + IdResuesta2;
            htmlViewScript += MiControlViewScript.CrearPanel(nodeId, IdPregunta, IdPregunta, value.Description, true);

            $.each(value.Edges, function (ind, val) {
                //Respuesta  

                //htmlViewScript += '<div>' + val.Description;//respuesta  EdgeId  
                var IdResuesta = "RespuestaView-" + nodeId + "-" + val.ValueCode + "-" + (ind + 1) + "-" + IdPregunta;
                htmlViewScript += '<div class="panel-group" id="pgRespuesta' + nodeId + '" role="tablist" aria-multiselectable="true" >';
                htmlViewScript += MiControlViewScript.CrearPanel(nodeId, IdResuesta, IdResuesta, val.Description, false);

                if (val.NextNodeId != null) {
                    var IdResuesta2 = "RespuestaView-" + nodeId + "-" + val.ValueCode + "-" + (ind + 1);
                    MiControlViewScript.ArmarControl(nodes, val.NextNodeId, IdResuesta2);
                }
                htmlViewScript += "</div></div></div>";
                htmlViewScript += "</div>";
            })

            htmlViewScript += "</div></div></div>";

        })
        return htmlViewScript
    },
    CrearPanel: function (idPanelGroup, question, answer, title, respuesta) {

        var formulario = "";
        formulario += '<div class="panel panel-default">';

        if (respuesta) {            
            formulario += ' <div class="panel-heading" role="tab" id="heading' + question + '" style="background-color:#337ab7;">';
            formulario += '     <h4 class="panel-title">';
            formulario += "         " + title;
            formulario += "     </h4>";
            formulario += " </div>";
            formulario += ' <div id="Body' + answer + '" class="panel-collapse collapse in bg-info" role="tabpanel" aria-labelledby="heading' + question + '">';
            formulario += '     <div class="panel-body">';            
        }
        else {        
            formulario += ' <div class="panel-heading" role="tab" id="heading' + question + '">';
            formulario += '     <h4 class="panel-title">';
            formulario += "         " + title;
            formulario += "     </h4>";
            formulario += " </div>";
            formulario += ' <div id="Body' + answer + '" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading' + question + '">';
            formulario += '     <div class="panel-body">';            
        }

        //formulario += '         ' + Body;
        //formulario += '     </div>';
        //formulario += ' </div>';
        //formulario += '</div>';

        return formulario;
    },
    CreatePanelGroup: function (idPanelGroup, contenido) {

        var control = "";

        control += '<div class="panel-group" id="pgPregunta' + idPanelGroup + '" role="tablist" aria-multiselectable="true">';

        control += contenido;

        control += "</div>";

        return control;
    },
    Pintar: function () {
        htmlViewScript = "";
        $("#QuestionPanelViewScript").html(MiControlViewScript.CreatePanelGroup(ScriptViewComposite.Nodes[0].NodeId, MiControlViewScript.ArmarControl(ScriptViewComposite.Nodes, ScriptViewComposite.Nodes[0].NodeId, "")));
    },
    GetIndexObjects: function (obj, key, val) {
        var contIndex = -1;
        var objIndex = -1;
        var newObj = false;
        $.each(obj, function () {
            contIndex++;
            var testObject = this;
            $.each(testObject, function (k, v) {
                if (val == v && k == key) {
                    newObj = testObject;
                    objIndex = contIndex;
                }
            });
        });
        return objIndex;
    }
};
