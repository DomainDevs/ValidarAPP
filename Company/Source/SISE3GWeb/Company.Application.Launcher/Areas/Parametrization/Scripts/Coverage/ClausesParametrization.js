var clausesAvailable = [];
var glbClausesParam;
var glbClausesParam_Original;
var glbClausesParamAssign_Original;
$.ajaxSetup({ async: true });
class ClausesParametrization extends Uif2.Page {
    /**
     * @summary 
        *  Metodo que se ejecuta al instanciar la clase     
     */
    getInitialState() {
        request('Parametrization/Coverage/GetClauses', null, 'GET', AppResources.ErrorGetClausesCONNEX, ClausesParametrization.LoadClausesInitial)        
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
        $("#btnModalClausesClose").click(this.closeClauses);
        $("#btnModalMarkCoverageClauseAsMandatory").click(this.AssignClauseMandatory);
    }

    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES
    btnClauses() {        
        if ($("#formCoverage").valid())
        {
            glbClausesParam_Original = jQuery.extend(true, [], $("#listviewClauses").UifListView("getData")); 
            glbClausesParamAssign_Original = jQuery.extend(true, [], $("#listviewClausesAssing").UifListView("getData")); 
            $("#modalClauses").UifModal('showLocal', AppResources.LabelClauses);        
        }
    }    

    CopyAllClause() {
        var data = $("#listviewClauses").UifListView('getData');
        if (data.length > 0) {
            data = $("#listviewClausesAssing").UifListView('getData').concat(data);
            $("#listviewClausesAssing").UifListView({ sourceData: data, displayTemplate: "#tmpClausesForAssign", selectionType: 'multiple', height: 310 });
            $("#listviewClauses").UifListView({ sourceData:[], displayTemplate: "#tmpForClauses", selectionType: 'multiple', height: 310 });                                  
        }
    }

    CopyClauseSelected() {
        var data = $("#listviewClauses").UifListView('getSelected');                
        $.each(data, function (position, value) {
            var findIndex = function (element, index, array) {
                return element.Id === value.Id
            }
            var index = $("#listviewClauses").UifListView("findIndex", findIndex);
            $("#listviewClauses").UifListView("deleteItem", index);
            $("#listviewClausesAssing").UifListView("addItem", value);
        })
    }

    DeallocateClausesAll() {
        ClausesParametrization.LoadClausesInitial(glbClausesParam);
    }

    DeallocateClausesSelect() {
        var data = $("#listviewClausesAssing").UifListView('getSelected');
        $.each(data, function (position, value) {
            var findIndex = function (element, index, array) {
                return element.Id === value.Id
            }
            var index = $("#listviewClausesAssing").UifListView("findIndex", findIndex);
            $("#listviewClausesAssing").UifListView("deleteItem", index);
            value.IsMandatory = false;
            $("#listviewClauses").UifListView("addItem", value);
        });
    }

    SaveClauses() {        
        ClausesParametrization.HidePanelsClause();
        ClausesParametrization.LoadSubTitle();
    }       
        
    AssignClauseMandatory() {
        var clauseSelected = jQuery.extend(true, [], $("#listviewClausesAssing").UifListView('getSelected'));
        if (clauseSelected.length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectClause, 'autoclose': true });
        }       
        else {                 
            $.each(clauseSelected, function (key, value) {                 
                const findClause = function (element, index, array) {
                    return element.Id === value.Id;
                }
                var index = $('#listviewClausesAssing').UifListView("findIndex", findClause);

                if (this.IsMandatory === true) {                    
                    this.IsMandatory = false;
                }
                else {                    
                    this.IsMandatory = true;
                }
                $('#listviewClausesAssing').UifListView("editItem", index, this);                
            });
        }

        $("#listviewClausesAssing .item").removeClass("selected");
    }

    //METODOS ADICIONALES     

    static LoadClausesInitial(data) {            
        glbClausesParam = data;
        $("#listviewClauses").UifListView({ sourceData: data, displayTemplate: "#tmpForClauses", selectionType: 'multiple', height: 310 });
        $("#listviewClausesAssing").UifListView({ sourceData: [], displayTemplate: "#tmpClausesForAssign", selectionType: 'multiple', height: 310 });                
        $("#selectedClauses").text("");
    }

    static LoadClausesByCoverageId(data) {
        ClausesParametrization.GetClausesParam().done(function (dataClauses) {
            if (dataClauses.success) {
                ClausesParametrization.LoadClausesInitial(dataClauses.result); //Recargo de informacion
                if (data.length > 0) {
                    $("#selectedClauses").text("(" + data.length + ")");
                    $.each(data, function (index, item) {
                        const findClauses = function (element, index, array) {
                            return element.Id === item.Id
                        }
                        const resultIndex = $("#listviewClauses").UifListView("findIndex", findClauses);
                        const itemAdd = $("#listviewClauses").UifListView('getData')[resultIndex];
                        if (itemAdd !== undefined) {
                            itemAdd.IsMandatory = item.IsMandatory;
                            $("#listviewClausesAssing").UifListView("addItem", itemAdd);
                            $("#listviewClauses").UifListView("deleteItem", resultIndex);
                        }
                    });
                }
            }
        });
       
    }
    
    static HidePanelsClause() {
        $('#modalClauses').UifModal('hide');        
    }

    closeClauses() {
        ClausesParametrization.HidePanelsClause();
        $("#listviewClausesAssing").UifListView({ sourceData: glbClausesParamAssign_Original, displayTemplate: "#tmpClausesForAssign", selectionType: 'multiple', height: 310 });
        $("#listviewClauses").UifListView({ sourceData: glbClausesParam_Original, displayTemplate: "#tmpForClauses", selectionType: 'multiple', height: 310 });               
        ClausesParametrization.LoadSubTitle();
    }
    static LoadSubTitle() {
        var clausesAssign = $("#listviewClausesAssing").UifListView('getData');
        if (clausesAssign.length > 0) {
            $('#selectedClauses').text("(" + clausesAssign.length + ")");
        }
        else {
            $('#selectedClauses').text("");
        }      
    }
    static GetClausesParam() {
        return $.ajax({
            url: rootPath + 'Parametrization/Coverage/GetClauses',
            async: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}

