var InsObjAssign = [];
var InsObjAvailable = [];

class LineBusinessInsuranceObjectsParametrization extends Uif2.Page {
    /**
     * @summary 
        *  Metodo que se ejecuta al instanciar la clase     
     */
    getInitialState() {
        LineBusinessInsuranceObjectsParametrization.LoadInsObjAvailable();
    }

    //EVENTOS CONTROLES

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {
        $("#btnInsuranceObjects").click(this.btnClauses);
        //Asignar Todos
        $("#btnModalInsuranceobjectsLineBusinessAssignAll").click(this.CopyAllClause);
        //Asignar Uno
        $("#btnModalInsuranceobjectsLineBusinessAssign").click(this.CopyClauseSelected);
        //Desasignar Todos
        $("#btnModalInsuranceobjectsLineBusinessDeallocateAll").click(this.DeallocateClausesAll);
        //Desasignar Uno
        $("#btnModalInsuranceobjectsLineBusinessDeallocate").click(this.DeallocateClausesSelect);
        $("#btnSaveObjects").click(this.SaveClauses);
        $("#btnCloseobjects").click(LineBusinessInsuranceObjectsParametrization.HidePanelsClause);
    }

    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES
	btnClauses() {
		if (update == true) {
			$("#ModalInsuranceObjects").UifModal('showLocal', Resources.Language.LabelInsuredObjects);
			LineBusinessInsuranceObjectsParametrization.LoadClauses();
		} else {
			//$.UifNotify('show', { 'type': 'danger', 'message': "1", 'autoclose': true });
		}
    }

    CopyAllClause() {
        var data = $("#listviewInsuranceobjectsLineBusiness").UifListView('getData');
        if (data.length > 0) {
            data = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData').concat(data);
            $("#listviewInsuranceobjectsLineBusinessAssing").UifListView({ sourceData: data, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
            $("#listviewInsuranceobjectsLineBusiness").UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
        }
    }

    CopyClauseSelected() {
        var data = $("#listviewInsuranceobjectsLineBusiness").UifListView('getSelected');
        var ClauseNoAsign = $("#listviewInsuranceobjectsLineBusiness").UifListView('getData');
        var ClauseAssing = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData');

        $.each(data, function (index, data) {
            var findClause = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = $("#listviewInsuranceobjectsLineBusiness").UifListView("findIndex", findClause);
            $("#listviewInsuranceobjectsLineBusiness").UifListView("deleteItem", index);
        });
        ClauseAssing = ClauseAssing.concat(data);
        $("#listviewInsuranceobjectsLineBusinessAssing").UifListView({ sourceData: ClauseAssing, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
    }

    DeallocateClausesAll() {
        var data = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData')
        $.each(data, function (key, value) {
            delete data[key].IsDefaultDescription;
            data[key].IsMandatory = false;
        });

        if ($("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData').length > 0) {
            data = $("#listviewInsuranceobjectsLineBusiness").UifListView('getData').concat(data);
            $("#listviewInsuranceobjectsLineBusiness").UifListView({ sourceData: data, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
            $("#listviewInsuranceobjectsLineBusinessAssing").UifListView({ displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
        }
    }

    DeallocateClausesSelect() {
        var data = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getSelected');
        if ($("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData').length > 0) {
            var ClauseNoAsign = $("#listviewInsuranceobjectsLineBusiness").UifListView('getData');
            var ClauseAsign = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData');
            $.each(data, function (index, data) {
                delete this.IsDefaultDescription;
                this.IsMandatory = false;
                var findClause = function (element, index, array) {
                    return element.Id === data.Id
                }
                var index = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView("findIndex", findClause);
                $("#listviewInsuranceobjectsLineBusinessAssing").UifListView("deleteItem", index);
            });
            ClauseNoAsign = ClauseNoAsign.concat(data);
            $("#listviewInsuranceobjectsLineBusiness").UifListView({ sourceData: ClauseNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
        }
    }

    SaveClauses() {
        InsObjAssign = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData');
		var idLineBusiness = $("#inputLineBusinessCode").val();
		LineBusinessRequest.SaveInsuredObject(idLineBusiness, InsObjAssign).done(function (data)
		{
			if (data.success) {
				//LineBusinessParametrization.ClearControls();
				$.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});
		LineBusinessParametrization.countModal();
		LineBusinessInsuranceObjectsParametrization.HidePanelsClause();
    }

    AssignClauseMandatory() {
        var clauseSelected = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getSelected');
        if (clauseSelected.length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectClause, 'autoclose': true });
        }
        else {
            var clauseData = $("#listviewInsuranceobjectsLineBusinessAssing").UifListView('getData');
            $.each(clauseSelected, function (key, value) {
                let clauseId = this.Id;
                const findClause = function (element, index, array) {
                    return element.Id === clauseId
                }
                var index = $('#listviewInsuranceobjectsLineBusinessAssing').UifListView("findIndex", findClause);

                if (this.IsMandatory === true) {
                    delete this.IsDefaultDescription;
                    this.IsMandatory = false;
                }
                else {
                    this.IsDefaultDescription = AppResources.LabelMandatory;
                    this.IsMandatory = true;
                }
                $('#listviewInsuranceobjectsLineBusinessAssing').UifListView("editItem", index, this);
            });
        }

        $("#listviewInsuranceobjectsLineBusinessAssing .item").removeClass("selected");
    }

    //METODOS ADICIONALES 

    static LoadInsObjAvailable() {
        LineBusinessInsuranceObjectsRequest.GetInsuranceObjects().done(function (data) {
            if (data != null) {
                InsObjAvailable = data.result;
            }
        });
    }

    static LoadClauses() {
        var clausesTemp = [];
        $.each(InsObjAvailable, function (keyClause, clause) {
            var exist = false;
            $.each(InsObjAssign, function (keyClauseAssign, clauseAssign) {
                if (clauseAssign.IsMandatory) {
                    InsObjAssign[keyClauseAssign].IsDefaultDescription = AppResources.LabelMandatory;
                }
                if (clauseAssign.Id == clause.Id) {
                    exist = true;
                    return;
                }
            });
            if (!exist) {
                clausesTemp.push(InsObjAvailable[keyClause]);
            }
        });
        $("#listviewInsuranceobjectsLineBusiness").UifListView({ sourceData: clausesTemp, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
        $("#listviewInsuranceobjectsLineBusinessAssing").UifListView({ sourceData: InsObjAssign, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
    }

    static ClearInsuranceObjectsParametrization() {
        InsObjAssign = [];
        InsObjAvailable = [];
        LineBusinessInsuranceObjectsParametrization.LoadInsObjAvailable();
    }

    static HidePanelsClause() {
        $('#ModalInsuranceObjects').UifModal('hide');
    }
}
