var glbParameter = {};
var parameterIndex = null;
var parameterNew = {};
var parameterId = null;
var Parameter = {};

class VariableQueries {

    static GetParameterById(parameters) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Variable/GetParametersByParameterIds',
            data: JSON.stringify({ AssistanceCode: parameters }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}


class Variable extends Uif2.Page {
    getInitialState() {
        $('#btnTexts').attr("disabled", true);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#listViewVariable").UifListView({
            displayTemplate: "#VariableTemplate",
            edit: true,
            delete: true,
            customEdit: true,
            customDelete: true,
            height: 300
        });
        Variable.load();
        $('#inputDescription').attr("disabled", true);
    }

    static load() {
        VariableQueries.GetParameterById().done(function (data) {
            if (data.success) {
                glbParameter = data.result;
                Variable.loadParameter(data.result);

            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {
        $('#listViewVariable').on('rowEdit', Variable.showData);
        $('#btnUpdateVariable').on('click', this.addItemParameter);
        $('#btnSave').on('click', this.UpdateParameter);
        $('#btnExit').on("click", this.Exit);
    }

    static loadParameter(varParameter) {
        $("#listViewVariable").UifListView({ source: null, displayTemplate: "#VariableTemplate", edit: true, delete: false, customEdit: true, customDelete: false, height: 300 });
        $.each(varParameter, function (key, value) {
            $("#listViewVariable").UifListView("addItem", this);
        });
    }

    static showData(event, result, index) {
        Variable.clearPanel();
        if (result.length === 1) {
            index = result[0].key;
            result = result[0];
        }
        if (result.ParameterId !== undefined) {
            parameterIndex = index;
            parameterId = result.ParameterId;
            $("#inputDescription").val(result.Description);
            $("#inputVariable").val(result.Value);
            $('#inputDescription').attr("disabled", true);
        }
    }

    static clearPanel() {
        parameterIndex = null;
        $("#inputDescription").val('');
        $("#inputVariable").val('');
        $('#inputDescription').attr("disabled", true);
    }

    addItemParameter() {
        if ($("#inputDescription").val() == "") {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorLoadUpdateVariable, 'autoclose': true })
        }
        else {
            $("#formVariable").validate();
            if ($("#formVariable").valid()) {
                parameterNew = {};
                parameterNew.ParameterId = parameterId;
                parameterNew.Description = $("#inputDescription").val();
                parameterNew.Value = parseInt($("#inputVariable").val());

                parameterNew.Status = 'Modified';

                $('#listViewVariable').UifListView('editItem', parameterIndex, parameterNew);

                Variable.clearPanel();
            }
        }
       
    }

    UpdateParameter() {
        Parameter = $("#listViewVariable").UifListView('getData').filter(x => x.Status === "Modified");
        
        if (Parameter.length > 0 ) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/Variable/CreateParameter',
                data: JSON.stringify({ ParametroVM: Parameter }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    Variable.load();
                }
                $.UifNotify('show', { 'type': 'info', 'message': "Se Actualizaron correctamente " + data.result.TotalModified + " Registros", 'autoclose': true })
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result.Message, 'autoclose': true })
            });
        }
        Parameter = null;

    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }
}