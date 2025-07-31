var detailTypesAvailable = [];
var glbDetailTypes;
var glbDetailTypesParam_Original;
var glbDetailTypesParamAssign_Original;
$.ajaxSetup({ async: true });
class DetailTypesParametrization extends Uif2.Page {
    /**
     * @summary 
        *  Metodo que se ejecuta al instanciar la clase     
     */
    getInitialState() {
        request('Parametrization/Coverage/GetDetailTypes', null, 'GET', AppResources.ErrorGetDetailTypesCONNEX, DetailTypesParametrization.LoadDetailTypesInitial)
    }

    //EVENTOS CONTROLES

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {
        $("#btnDetailTypes").click(this.btnDetailTypes);
        //Asignar Todos
        $("#btnModalDetailTypesAssignAll").click(this.CopyAllDetailTypes);
        //Asignar Uno
        $("#btnModalDetailTypesAssign").click(this.CopyDetailTypeSelected);
        //Desasignar Todos
        $("#btnModalDetailTypesDeallocateAll").click(this.DeallocateDetailTypesAll);
        //Desasignar Uno
        $("#btnModalDetailTypesDeallocate").click(this.DeallocateDetailTypeSelect);
        $("#btnModalDetailTypesSave").click(this.SaveDetailTypes);
        $("#btnModalDetailTypesClose").click(this.closeDetailType);
        $("#btnAssignDetailTypeMandatory").click(this.AssignDetailTypeMandatory);
        $("#selectCompositionType").on('itemSelected', this.changeSelectCompositionType);
    }

    //METODOS PARA EJECUTAR EN LOS EVENTOS DE LOS CONTROLES
    btnDetailTypes() {
        if ($("#formCoverage").valid()) {
            glbDetailTypesParam_Original = jQuery.extend(true, [], $("#listviewDetailTypes").UifListView("getData"));
            glbDetailTypesParamAssign_Original = jQuery.extend(true, [], $("#listviewDetailTypesAssing").UifListView("getData"));
            $('#modalDetailTypes').UifModal('showLocal', AppResources.DetailTypes);
        }
    }

    CopyAllDetailTypes() {
        if (DetailTypesParametrization.validateSelectedCompositionType()) {
            var data = $("#listviewDetailTypes").UifListView('getData');
            if (data.length > 0) {
                data = $("#listviewDetailTypesAssing").UifListView('getData').concat(data);
                $.each(data, function (position, value) {
                    value.CompositionTypeDescription = $("#selectCompositionType").UifSelect("getSelectedText");
                });
                $("#listviewDetailTypesAssing").UifListView({ sourceData: data, displayTemplate: "#detailTypeTemplateAssign", selectionType: 'multiple', height: 310 });
                $("#listviewDetailTypes").UifListView({ sourceData: [], displayTemplate: "#detailTypeTemplate", selectionType: 'multiple', height: 310 });
            }
        }
    }

    CopyDetailTypeSelected() {
        if (DetailTypesParametrization.validateSelectedCompositionType()) {
            var data = $("#listviewDetailTypes").UifListView('getSelected');
            $.each(data, function (position, value) {
                var findIndex = function (element, index, array) {
                    return element.Id === value.Id
                }
                var index = $("#listviewDetailTypes").UifListView("findIndex", findIndex);
                value.CompositionTypeDescription = $("#selectCompositionType").UifSelect("getSelectedText");
                $("#listviewDetailTypes").UifListView("deleteItem", index);
                $("#listviewDetailTypesAssing").UifListView("addItem", value);
            })
        }
    }

    DeallocateDetailTypesAll() {
        DetailTypesParametrization.LoadDetailTypesInitial(glbDetailTypes);
    }

    DeallocateDetailTypeSelect() {
        var data = $("#listviewDetailTypesAssing").UifListView('getSelected');
        $.each(data, function (position, value) {
            var findIndex = function (element, index, array) {
                return element.Id === value.Id
            }
            var index = $("#listviewDetailTypesAssing").UifListView("findIndex", findIndex);
            $("#listviewDetailTypesAssing").UifListView("deleteItem", index);
            value.IsMandatory = false;
            $("#listviewDetailTypes").UifListView("addItem", value);
        });
    }

    SaveDetailTypes() {
        //Validacion nivel de influencia debe tener x lo menos un tipo de detalle 
        if (DetailTypesParametrization.validateDetailType()) {
            DetailTypesParametrization.HidePanelsDetailType();
            DetailTypesParametrization.LoadSubTitle();
        }
    }
    static validateDetailType() {
        var band = true;
        if ($("#selectCompositionType").UifSelect("getSelected") > 1 && $("#listviewDetailTypesAssing").UifListView("getData").length === 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.MustRelatedOneDetailType, 'autoclose': true });
            band = false;
        }
        return band;
    }

    AssignDetailTypeMandatory() {
        var detailTypeSelected = jQuery.extend(true, [], $("#listviewDetailTypesAssing").UifListView('getSelected'));
        if (detailTypeSelected.length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectDetailType, 'autoclose': true });
        }
        else {            
            $.each(detailTypeSelected, function (key, value) {                
                const findDetailType = function (element, index, array) {
                    return element.Id === value.Id
                }
                var index = $('#listviewDetailTypesAssing').UifListView("findIndex", findDetailType);

                if (this.IsMandatory === true) {                    
                    this.IsMandatory = false;
                }
                else {
                    this.IsMandatory = true;
                }
                $('#listviewDetailTypesAssing').UifListView("editItem", index, this);
            });
        }

        $("#listviewDetailTypesAssing .item").removeClass("selected");
    }

    static LoadDetailTypesInitial(data) {      
        glbDetailTypes = jQuery.extend(true, [], data);
        $.each(data, function (position, value) {
            value.CompositionTypeDescription = $("#selectCompositionType").UifSelect("getSelectedText");
        });
        $("#listviewDetailTypes").UifListView({ sourceData: data, displayTemplate: "#detailTypeTemplate", selectionType: 'multiple', height: 310 });
        $("#listviewDetailTypesAssing").UifListView({ sourceData: [], displayTemplate: "#detailTypeTemplateAssign", selectionType: 'multiple', height: 310 });
        $("#selectedDetailTypes").text("");
    }

    static LoadDetailTypesByCoverageId(data) {        
        DetailTypesParametrization.LoadDetailTypesInitial(glbDetailTypes);//Recargo de informacion
        if (data.length > 0)
        {
            $("#selectedDetailTypes").text("(" + data.length + ")");
            $.each(data, function (index, item) {
                const findDetailTypes = function (element, index, array) {
                    return element.Id === item.Id
                }
                const resultIndex = $("#listviewDetailTypes").UifListView("findIndex", findDetailTypes);
                const itemAdd = $("#listviewDetailTypes").UifListView('getData')[resultIndex];
                if (itemAdd !== undefined) {
                    itemAdd.IsMandatory = item.IsMandatory;
                    itemAdd.CompositionTypeDescription = $("#selectCompositionType").UifSelect("getSelectedText");
                    $("#listviewDetailTypesAssing").UifListView("addItem", itemAdd);
                    $("#listviewDetailTypes").UifListView("deleteItem", resultIndex);
                }
            });
        }
    }

    static HidePanelsDetailType() {
        $('#modalDetailTypes').UifModal('hide');
    }

    static LoadSubTitle() {
        var detailTypesAssign = $("#listviewDetailTypesAssing").UifListView('getData');
        if (detailTypesAssign.length > 0) {
            $('#selectedDetailTypes').text("(" + detailTypesAssign.length + ")");
        }
        else {
            $('#selectedDetailTypes').text("");
        }
    }

    closeDetailType() {
        DetailTypesParametrization.HidePanelsDetailType();
        $("#listviewDetailTypes").UifListView({ sourceData: glbDetailTypesParam_Original, displayTemplate: "#detailTypeTemplate", selectionType: 'multiple', height: 310 });
        $("#listviewDetailTypesAssing").UifListView({ sourceData: glbDetailTypesParamAssign_Original, displayTemplate: "#detailTypeTemplateAssign", selectionType: 'multiple', height: 310 });
        DetailTypesParametrization.LoadSubTitle();
    }

    static validateSelectedCompositionType() {
        if (parseInt($("#selectCompositionType").UifSelect("getSelected"))>0)
        {
            return true;
        }
        else
        {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.MustRelatedLevelInfluence, 'autoclose': true });
            return false;
        }
    }
    changeSelectCompositionType(event, value) {
        var data = jQuery.extend(true, [], $("#listviewDetailTypesAssing").UifListView("getData"));
        if (data.length > 0)
        {
            if (!(value.Id > 0))
            {
                DetailTypesParametrization.LoadDetailTypesInitial(glbDetailTypes);
            }
            else if (data[0].CompositionTypeDescription != value.Text)
            {
                $.each(data, function (position, item) {
                    this.CompositionTypeDescription = value.Text;
                });
                $("#listviewDetailTypesAssing").UifListView({ sourceData: data, displayTemplate: "#detailTypeTemplateAssign", selectionType: 'multiple', height: 310 });
            } 
        }
    }
}