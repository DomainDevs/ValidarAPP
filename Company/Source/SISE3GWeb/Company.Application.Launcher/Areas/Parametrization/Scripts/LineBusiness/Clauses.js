var clausesAssign = [];
var clausesAvailable = [];

class LineBusinessClausesParametrization extends Uif2.Page {
    /**
     * @summary 
        *  Metodo que se ejecuta al instanciar la clase     
     */
    getInitialState() {
        LineBusinessClausesParametrization.LoadClausesAvailable();
    }

    //EVENTOS CONTROLES

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {
        $("#btnClauses").click(this.btnClauses);
        //Asignar Todos
        $("#btnModalClausesAssignAll").click(this.CopyAllClause);
        //Asignar Uno
        $("#btnModalClausesAssign").click(this.CopyClauseSelected);
        //Desasignar Todos
        $("#btnModalClausesDeallocateAll").click(this.DeallocateClausesAll);
        //Desasignar Uno
        $("#btnModalClausesDeallocate").click(this.DeallocateClausesSelect);
        $("#btnModalClausesSave").click(this.SaveClauses);
        $("#btnModalClausesClose").click(LineBusinessClausesParametrization.HidePanelsClause);
        $("#btnModalMarkCoverageClauseAsMandatory").click(this.AssignClauseMandatory);
    }

    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES
	btnClauses() {
		if (update == true) {
			$("#modalClauses").UifModal('showLocal', Resources.Language.LabelClauses);
			LineBusinessClausesParametrization.LoadClauses();
		} else {
			//$.UifNotify('show', { 'type': 'danger', 'message':"3", 'autoclose': true });
		}
    }

    CopyAllClause() {
        var data = $("#listviewClauses").UifListView('getData');
        if (data.length > 0) {
            data = $("#listviewClausesAssing").UifListView('getData').concat(data);
            $("#listviewClausesAssing").UifListView({ sourceData: data, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpClausesForAssign", selectionType: 'multiple', height: 310 });
            $("#listviewClauses").UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForClauses", selectionType: 'multiple', height: 310 });
        }
    }

    CopyClauseSelected() {
        var data = $("#listviewClauses").UifListView('getSelected');
        var ClauseNoAsign = $("#listviewClauses").UifListView('getData');
        var ClauseAssing = $("#listviewClausesAssing").UifListView('getData');

        $.each(data, function (index, data) {
            var findClause = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = $("#listviewClauses").UifListView("findIndex", findClause);
            $("#listviewClauses").UifListView("deleteItem", index);
        });
        ClauseAssing = ClauseAssing.concat(data);
        $("#listviewClausesAssing").UifListView({ sourceData: ClauseAssing, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpClausesForAssign", selectionType: 'multiple', height: 310 });
    }

    DeallocateClausesAll() {
        var data = $("#listviewClausesAssing").UifListView('getData')
        $.each(data, function (key, value) {
            delete data[key].IsDefaultDescription;
            data[key].IsMandatory = false;
        });

        if ($("#listviewClausesAssing").UifListView('getData').length > 0) {
            data = $("#listviewClauses").UifListView('getData').concat(data);
            $("#listviewClauses").UifListView({ sourceData: data, displayTemplate: "#tmpForClauses", selectionType: 'multiple', height: 310 });
            $("#listviewClausesAssing").UifListView({ displayTemplate: "#tmpClausesForAssign", selectionType: 'multiple', height: 310 });
        }
    }

    DeallocateClausesSelect() {
        var data = $("#listviewClausesAssing").UifListView('getSelected');
        if ($("#listviewClausesAssing").UifListView('getData').length > 0) {
            var ClauseNoAsign = $("#listviewClauses").UifListView('getData');
            $.each(data, function (index, data) {
                delete this.IsDefaultDescription;
                this.IsMandatory = false;
                var findClause = function (element, index, array) {
                    return element.Id === data.Id
                }
                var index = $("#listviewClausesAssing").UifListView("findIndex", findClause);
                $("#listviewClausesAssing").UifListView("deleteItem", index);
            });
            ClauseNoAsign = ClauseNoAsign.concat(data);
            $("#listviewClauses").UifListView({ sourceData: ClauseNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForClauses", selectionType: 'multiple', height: 310 });
        }
    }

    SaveClauses() {
		clausesAssign = $("#listviewClausesAssing").UifListView('getData');
		var idLineBusiness = $("#inputLineBusinessCode").val();
		LineBusinessRequest.SaveClause(idLineBusiness, clausesAssign).done(function (data) {
			if (data.success) {
				//LineBusinessParametrization.ClearControls();
				$.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});
		LineBusinessParametrization.countModal();
		LineBusinessClausesParametrization.HidePanelsClause();
    }

    AssignClauseMandatory() {
        var clauseSelected = jQuery.extend(true, [], $("#listviewClausesAssing").UifListView('getSelected'));
        if (clauseSelected.length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectClause, 'autoclose': true });
        }
        else {
            var clauseData = $("#listviewClausesAssing").UifListView('getData');
            $.each(clauseSelected, function (key, value) {
                let clauseId = this.Id;
                const findClause = function (element, index, array) {
                    return element.Id === clauseId
                }
                var index = $('#listviewClausesAssing').UifListView("findIndex", findClause);

                if (this.IsMandatory === true) {
                    delete this.IsDefaultDescription;
                    this.IsMandatory = false;
                }
                else {
                    this.IsDefaultDescription = AppResources.LabelMandatory;
                    this.IsMandatory = true;
                }
                $('#listviewClausesAssing').UifListView("editItem", index, this);
            });
        }

        $("#listviewClausesAssing .item").removeClass("selected");
    }

    //METODOS ADICIONALES 

    static LoadClausesAvailable() {
        LineBusinessClausesRequest.GetClauses().done(function (data) {
            if (data != null) {
                clausesAvailable = data.result;
            }
        });
    }

    static LoadClauses() {
        var clausesTemp = [];
        $.each(clausesAvailable, function (keyClause, clause) {
            var exist = false;
            $.each(clausesAssign, function (keyClauseAssign, clauseAssign) {
                if (clauseAssign.IsMandatory) {
                    clausesAssign[keyClauseAssign].IsDefaultDescription = AppResources.LabelMandatory;
                }
                if (clauseAssign.Id == clause.Id) {
                    exist = true;
                    return;
                }
            });
            if (!exist) {
                clausesTemp.push(clausesAvailable[keyClause]);
            }
        });
        $("#listviewClauses").UifListView({ sourceData: clausesTemp, displayTemplate: "#tmpForClauses", selectionType: 'multiple', height: 310 });
        $("#listviewClausesAssing").UifListView({ sourceData: clausesAssign, displayTemplate: "#tmpClausesForAssign", selectionType: 'multiple', height: 310 });
    }

    static ClearClausesParametrization() {
        clausesAssign = [];
        clausesAvailable = [];
        LineBusinessClausesParametrization.LoadClausesAvailable();
    }

    static HidePanelsClause() {
        $('#modalClauses').UifModal('hide');
    }
}