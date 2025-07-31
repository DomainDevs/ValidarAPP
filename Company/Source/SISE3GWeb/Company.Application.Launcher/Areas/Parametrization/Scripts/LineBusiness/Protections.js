var ProtectionAssign = [];
var ProtectionAvailable = [];

class LineBusinessProtectionsParametrization extends Uif2.Page {
    /**
     * @summary 
        *  Metodo que se ejecuta al instanciar la clase     
     */
    getInitialState() {
        LineBusinessProtectionsParametrization.LoadProtectionAvailable();
    }

    //EVENTOS CONTROLES

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {
        $("#btnProtections").click(this.btnProtections);
        //Asignar Todos
        $("#btnModalProtectionLineBusinessAssignAll").click(this.CopyAllProtection);
        //Asignar Uno
        $("#btnModalProtectionLineBusinessAssign").click(this.CopyProtectionSelected);
        //Desasignar Todos
        $("#btnModalProtectionLineBusinessDeallocateAll").click(this.DeallocateProtectionsAll);
        //Desasignar Uno
        $("#btnModalProtectionLineBusinessDeallocate").click(this.DeallocateProtectionsSelect);
        $("#btnSaveProtection").click(this.SaveProtections);
        $("#btnCloseProtection").click(LineBusinessProtectionsParametrization.HidePanelsProtection);
    }

    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES
	btnProtections() {
		if (update == true) {
			$("#ModalProtection").UifModal('showLocal', Resources.Language.LabelProtections);
			LineBusinessProtectionsParametrization.LoadProtections();
		} else {
			//$.UifNotify('show', { 'type': 'danger', 'message': "2", 'autoclose': true });
		}
    }

    CopyAllProtection() {
        var data = $("#listviewProtectionLineBusiness").UifListView('getData');
        if (data.length > 0) {
            data = $("#listviewProtectionLineBusinessAssing").UifListView('getData').concat(data);
            $("#listviewProtectionLineBusinessAssing").UifListView({ sourceData: data, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
            $("#listviewProtectionLineBusiness").UifListView({ customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
        }
    }

    CopyProtectionSelected() {
        var data = $("#listviewProtectionLineBusiness").UifListView('getSelected');
        var ProtectionNoAsign = $("#listviewProtectionLineBusiness").UifListView('getData');
        var ProtectionAssing = $("#listviewProtectionLineBusinessAssing").UifListView('getData');

        $.each(data, function (index, data) {
            var findProtection = function (element, index, array) {
                return element.Id === data.Id
            }
            var index = $("#listviewProtectionLineBusiness").UifListView("findIndex", findProtection);
            $("#listviewProtectionLineBusiness").UifListView("deleteItem", index);
        });
        ProtectionAssing = ProtectionAssing.concat(data);
        $("#listviewProtectionLineBusinessAssing").UifListView({ sourceData: ProtectionAssing, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
    }

    DeallocateProtectionsAll() {
        var data = $("#listviewProtectionLineBusinessAssing").UifListView('getData')
        $.each(data, function (key, value) {
            delete data[key].IsDefaultDescription;
            data[key].IsMandatory = false;
        });

        if ($("#listviewProtectionLineBusinessAssing").UifListView('getData').length > 0) {
            data = $("#listviewProtectionLineBusiness").UifListView('getData').concat(data);
            $("#listviewProtectionLineBusiness").UifListView({ sourceData: data, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
            $("#listviewProtectionLineBusinessAssing").UifListView({ displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
        }
    }

    DeallocateProtectionsSelect() {
        var data = $("#listviewProtectionLineBusinessAssing").UifListView('getSelected');
        if ($("#listviewProtectionLineBusinessAssing").UifListView('getData').length > 0) {
            var ProtectionNoAsign = $("#listviewProtectionLineBusiness").UifListView('getData');
            var ProtectionAsign = $("#listviewProtectionLineBusinessAssing").UifListView('getData');
            $.each(data, function (index, data) {
                delete this.IsDefaultDescription;
                this.IsMandatory = false;
                var findProtection = function (element, index, array) {
                    return element.Id === data.Id
                }
                var index = $("#listviewProtectionLineBusinessAssing").UifListView("findIndex", findProtection);
                $("#listviewProtectionLineBusinessAssing").UifListView("deleteItem", index);
            });
            ProtectionNoAsign = ProtectionNoAsign.concat(data);
            $("#listviewProtectionLineBusiness").UifListView({ sourceData: ProtectionNoAsign, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: false, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
        }
    }

    SaveProtections() {
		ProtectionAssign = $("#listviewProtectionLineBusinessAssing").UifListView('getData');
		var idLineBusiness = $("#inputLineBusinessCode").val();
		LineBusinessRequest.SavePeril(idLineBusiness, ProtectionAssign).done(function (data) {
			if (data.success) {
				//LineBusinessParametrization.ClearControls();
				$.UifNotify('show', { 'type': 'info', 'message': data.result.message, 'autoclose': true });
			}
			else {
				$.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
			}
		});
		LineBusinessParametrization.countModal();
		LineBusinessProtectionsParametrization.HidePanelsProtection();
    }

    AssignProtectionMandatory() {
        var ProtectionSelected = $("#listviewProtectionLineBusinessAssing").UifListView('getSelected');
        if (ProtectionSelected.length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectProtection, 'autoclose': true });
        }
        else {
            var ProtectionData = $("#listviewProtectionLineBusinessAssing").UifListView('getData');
            $.each(ProtectionSelected, function (key, value) {
                let ProtectionId = this.Id;
                const findProtection = function (element, index, array) {
                    return element.Id === ProtectionId
                }
                var index = $('#listviewProtectionLineBusinessAssing').UifListView("findIndex", findProtection);

                if (this.IsMandatory === true) {
                    delete this.IsDefaultDescription;
                    this.IsMandatory = false;
                }
                else {
                    this.IsDefaultDescription = AppResources.LabelMandatory;
                    this.IsMandatory = true;
                }
                $('#listviewProtectionLineBusinessAssing').UifListView("editItem", index, this);
            });
        }

        $("#listviewProtectionLineBusinessAssing .item").removeClass("selected");
    }

    //METODOS ADICIONALES 

    static LoadProtectionAvailable() {
        LineBusinessProtectionsRequest.GetPerils().done(function (data) {
            if (data != null) {
                ProtectionAvailable = data.result;
            }
        });
    }

    static LoadProtections() {
        var ProtectionsTemp = [];
        $.each(ProtectionAvailable, function (keyProtection, Protection) {
            var exist = false;
            $.each(ProtectionAssign, function (keyProtectionAssign, ProtectionAssign) {
                if (ProtectionAssign.IsMandatory) {
                    ProtectionAssign[keyProtectionAssign].IsDefaultDescription = AppResources.LabelMandatory;
                }
                if (ProtectionAssign.Id == Protection.Id) {
                    exist = true;
                    return;
                }
            });
            if (!exist) {
                ProtectionsTemp.push(ProtectionAvailable[keyProtection]);
            }
        });
        $("#listviewProtectionLineBusiness").UifListView({ sourceData: ProtectionsTemp, displayTemplate: "#tmpForInsuranceObject", selectionType: 'multiple', height: 310 });
        $("#listviewProtectionLineBusinessAssing").UifListView({ sourceData: ProtectionAssign, displayTemplate: "#tmpForInsuranceObjectAsigned", selectionType: 'multiple', height: 310 });
    }

    static ClearProtectionsParametrization() {
        ProtectionAssign = [];
        ProtectionAvailable = [];
        LineBusinessProtectionsParametrization.LoadProtectionAvailable();
    }

    static HidePanelsProtection() {
        $('#ModalProtection').UifModal('hide');
    }
}
