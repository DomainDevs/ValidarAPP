var deductiblesAvailable = [];
var glbDeductiblesParam;
var glbDeductiblesParam_Original;
var glbDeductiblesParamAssign_Original;
$.ajaxSetup({ async: true });
class DeductiblesParametrization extends Uif2.Page {

    /**
     * @summary 
        *  Metodo que se ejecuta al instanciar la clase     
     */
    getInitialState() {        
        request('Parametrization/Coverage/GetDeductibles', null, 'GET', AppResources.ErrorGetDeductiblesCONNEX, DeductiblesParametrization.LoadDeductiblesInitial);        
    }

    //EVENTOS CONTROLES

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {

        $("#btnDeductibles").click(this.btnDeductibles);
        //Asignar Todos
        $("#btnModalDeductiblesAssignAll").click(this.CopyAllDeductibles);
        //Asignar Uno
        $("#btnModalDeductiblesAssign").click(this.CopyDeductibleSelected);
        //Desasignar Todos
        $("#btnModalDeductiblesDeallocateAll").click(this.DeallocateDeductiblesAll);
        //Desasignar Uno
        $("#btnModalDeductiblesDeallocate").click(this.DeallocateDeductiblesSelect);
        $("#btnModalDeductiblesSave").click(this.SaveDeductibles);
        $("#btnModalDeductiblesClose").click(this.closeDeductible);
        $("#btnModalMarkCoverageDeductibleAsDefault").click(this.AssignDeductibleDefault);

    }

    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES
    btnDeductibles() {
        if ($("#formCoverage").valid()) {
            glbDeductiblesParam_Original = jQuery.extend(true, [], $("#listviewDeductibles").UifListView("getData"));
            glbDeductiblesParamAssign_Original = jQuery.extend(true, [], $("#listviewDeductiblesAssing").UifListView("getData")); 
            $("#modalDeductibles").UifModal('showLocal', AppResources.LabelDeductibles);
        }
        
    }

    CopyAllDeductibles() {
        var data = $("#listviewDeductibles").UifListView('getData');
        if (data.length > 0) {
            data = $("#listviewDeductiblesAssing").UifListView('getData').concat(data);            
            $("#listviewDeductiblesAssing").UifListView({ sourceData: data, displayTemplate: "#tmpDeductiblesForAssign", selectionType: 'multiple', height: 310 });
            $("#listviewDeductibles").UifListView({ sourceData: [], displayTemplate: "#tmpForDeductibles", selectionType: 'multiple', height: 310 });         
        }
    }

    CopyDeductibleSelected() {
        var data = $("#listviewDeductibles").UifListView('getSelected');
        $.each(data, function (position, value) {
            var findIndex = function (element, index, array) {
                return element.Id === value.Id
            }
            var index = $("#listviewDeductibles").UifListView("findIndex", findIndex);
            $("#listviewDeductibles").UifListView("deleteItem", index);
            $("#listviewDeductiblesAssing").UifListView("addItem", value);
        })
    }
    
    
    DeallocateDeductiblesAll() {
        DeductiblesParametrization.LoadDeductiblesInitial(glbDeductiblesParam);
    }

    DeallocateDeductiblesSelect(data) {
        var data = $("#listviewDeductiblesAssing").UifListView('getSelected');
        $.each(data, function (position, value) {
            var findIndex = function (element, index, array) {
                return element.Id === value.Id
            }
            var index = $("#listviewDeductiblesAssing").UifListView("findIndex", findIndex);
            $("#listviewDeductiblesAssing").UifListView("deleteItem", index);
            value.IsMandatory = false;
            $("#listviewDeductibles").UifListView("addItem", value);
        });
    }

    SaveDeductibles() {        
        DeductiblesParametrization.HidePanelsDeductible();
        DeductiblesParametrization.LoadSubTitle();        
    }    

    AssignDeductibleDefault() {        
        var deductibleSelected = jQuery.extend(true, [], $("#listviewDeductiblesAssing").UifListView('getSelected'));
        if (deductibleSelected.length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectDeductibles, 'autoclose': true });
        }
        else if (deductibleSelected.length > 1) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectMaxiumDeductibles, 'autoclose': true });
        }
        else {
            deductibleSelected = deductibleSelected[0];
            var deductibleData = $("#listviewDeductiblesAssing").UifListView('getData');
            $.each(deductibleData, function (key, value) {
                if (value.Id == deductibleSelected.Id) {                    
                    deductibleSelected.IsMandatory = true;
                    $('#listviewDeductiblesAssing').UifListView("editItem", key, deductibleSelected);
                }
                else {                    
                    value.IsMandatory = false;
                    $('#listviewDeductiblesAssing').UifListView("editItem", key, value);
                }
            });
        }
        $("#listviewDeductiblesAssing .item").removeClass("selected");
    }

    static LoadDeductiblesByCoverageId(data) {
        DeductiblesParametrization.GetDeductiblesParam().done(function (dataDeductibles) {
            if (dataDeductibles.success) {
                DeductiblesParametrization.LoadDeductiblesInitial(dataDeductibles.result);//Recargo de informacion
                if (data.length > 0) {
                    $("#selectedDeductibles").text("(" + data.length + ")");
                    $.each(data, function (index, item) {
                        const findDeductible = function (element, index, array) {
                            return element.Id === item.Id
                        }
                        const resultIndex = $("#listviewDeductibles").UifListView("findIndex", findDeductible);
                        const itemAdd = $("#listviewDeductibles").UifListView('getData')[resultIndex];
                        if (itemAdd !== undefined) {
                            itemAdd.IsMandatory = item.IsMandatory;
                            $("#listviewDeductiblesAssing").UifListView("addItem", itemAdd);
                            $("#listviewDeductibles").UifListView("deleteItem", resultIndex);
                        }
                    });
                }
            }
        });

        
    }


    static LoadDeductiblesInitial(data) {        
        glbDeductiblesParam = data;
        $("#listviewDeductibles").UifListView({ sourceData: data, displayTemplate: "#tmpForDeductibles", selectionType: 'multiple', height: 310 });
        $("#listviewDeductiblesAssing").UifListView({ sourceData: [], displayTemplate: "#tmpDeductiblesForAssign", selectionType: 'multiple', height: 310 });        
        $("#selectedDeductibles").text("");
    }

    static HidePanelsDeductible() {
        $('#modalDeductibles').UifModal('hide');
    }    

    closeDeductible() {
        DeductiblesParametrization.HidePanelsDeductible();
        $("#listviewDeductibles").UifListView({ sourceData: glbDeductiblesParam_Original, displayTemplate: "#tmpForDeductibles", selectionType: 'multiple', height: 310 });
        $("#listviewDeductiblesAssing").UifListView({ sourceData: glbDeductiblesParamAssign_Original, displayTemplate: "#tmpDeductiblesForAssign", selectionType: 'multiple', height: 310 });
        DeductiblesParametrization.LoadSubTitle();
    }

    static LoadSubTitle() {
        var deductiblesAssign = $("#listviewDeductiblesAssing").UifListView('getData');
        if (deductiblesAssign.length > 0) {
            $('#selectedDeductibles').text("(" + deductiblesAssign.length + ")");
        }
        else {
            $('#selectedDeductibles').text("");
        }
    }
    static GetDeductiblesParam() {
        return $.ajax({
            url: rootPath + 'Parametrization/Coverage/GetDeductibles',
            async: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}