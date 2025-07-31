// Variables de entorno
var glbIdGroundsRejection;
var glbIndexGroundsRejection;
var glbRejectionCauseDeleted = [];

/**
    * @summary Peticiona ajax a controlador
    */
class AjaxGroupRelection {
    static GetGroupPolicie()
    {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'ParamAuthorizationPolicies/GroundsRejection/GetGroupPolicies',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetRejectionCausesAll() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'ParamAuthorizationPolicies/GroundsRejection/GetRejectionCausesAll',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SearchAdvancedRejectionCauses(Description, GroupPolicie) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'ParamAuthorizationPolicies/GroundsRejection/GetRejectionCausesByDescription',
            data: JSON.stringify({ description : Description, groupPolicie : GroupPolicie }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

/**
    * @summary Clase del formulario principal
    */
class GroundsRejectionCause extends Uif2.Page {

    //---------------------INICIALIZADOR -------------------//

     /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        // Definir ListView
        $("#listGroundsRejection").UifListView({
            displayTemplate: "#templateGroundsRejection",
            source: null,
            selecttionType: 'single',
            height: 400,
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: this.deleteRejectionCause,
        }); 
        //Carga datos en el listView
        AjaxGroupRelection.GetRejectionCausesAll().done(function (data) {
            GroundsRejectionCause.loadRejectionCauses(data);
        });
        // Carga datos en el texbox (select)
        GroundsRejectionCause.GetGroupPolicies(); 
    }

     //---------------------EVENTOS -------------------//

   /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {
        $("#btnGroundsRejectionNew").on("click", GroundsRejectionCause.clearFormGroundsRejection);
        $("#btnGroundsRejectionAccept").on("click", GroundsRejectionCause.AddGroundsRejection);
        $("#btnSaveGroundsRejection").on("click", GroundsRejectionCause.OperationRejectionCauses);
        $("#btnExitGroundsRejection").on("click", this.redirectIndex);
        $("#listGroundsRejection").on("rowEdit", GroundsRejectionCause.editRejectionCause);  
        $("#btnExportGroundsRejection").click(this.exportExcel);
    }

    static loadRejectionCauses(data) {
        if (data.success) {
            // Limpiar ListView
            $("#listGroundsRejection").UifListView("clear");
            
            // Llenar objeto con los datos obtenidos
            $.each(data.result, function (key, value) {                    
                // Agregar objeto al ListView
                
                $("#listGroundsRejection").UifListView("addItem", value);
            });
        }
    }

    /**
    * @summary Editar datos 
    */
    static editRejectionCause(event, result, index) {        
        if (result != null) {
            glbIndexGroundsRejection = index;
            glbIdGroundsRejection = result.id;
            var id = (result.GroupPolicies.id == null ? result.GroupPolicies.Id : result.GroupPolicies.id);            
            $("#IdPolicyGroup").UifSelect("setSelected", id);
            $("#IdDescriptionRejection").val(result.description);
        }
    }
    /**
    * @summary Envia grupo de policies 
    */
    static GetGroupPolicies() {
        
        AjaxGroupRelection.GetGroupPolicie().done(response => {
            let result = response.result;
            
            if (response.success) {
                $("#IdPolicyGroup").UifSelect({ sourceData: result });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }
    // Envia datos a ListView
    static getGroundsRejection(data) {
        $("#listGroundsRejection").UifListView({
            displayTemplate: "#templateGroundsRejection",
            sourceData: data,
            selectionType: 'single',
            height: 400,
            edit: true,
            delete: true,
            deleteCallback: this.deleteRejectionCause,

        });
    }

     /**
    * @summary Envia grupo de policies 
    */
    static OperationRejectionCauses() {        
        var listRejectionCauses = $("#listGroundsRejection").UifListView('getData');        
        var listRejectionCauses = listRejectionCauses.filter(function (item) {
            return item.StatusTypeService > 1;
        });
        if (listRejectionCauses.length > 0) {
            request('ParamAuthorizationPolicies/GroundsRejection/OperationBaseRejectionCauses',
                JSON.stringify({ ListBaseRejectionCauses: listRejectionCauses }),
                'POST',
                AppResources.UnexpectedError, GroundsRejectionCause.confirmCreateBaseRejection);

        }
        else {
            if (glbRejectionCauseDeleted == null && glbRejectionCauseDeleted.length == 0) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': Resources.Language.ExpenseNotDataOperation, 'autoclose': true
                })
            } else {
                request('ParamAuthorizationPolicies/GroundsRejection/OperationBaseRejectionCauses',
                    JSON.stringify({ ListBaseRejectionCauses: glbRejectionCauseDeleted }),
                    'POST',
                    AppResources.UnexpectedError, GroundsRejectionCause.confirmCreateBaseRejection);
                   }
            
        }        
    }
    

   /**
    * @summary Limpia formulario motivos de rechazo
    */
    static clearFormGroundsRejection() {
        ClearValidation("#formGroundsRejection");
        $("#IdPolicyGroup").UifSelect("setSelected", null);
        $("#IdDescriptionRejection").val("");
        glbIndexGroundsRejection = null;
        glbIdGroundsRejection = null;
    }

 /**
    * @summary Agrega a listview registros
    */
    static AddGroundsRejection() {
        var FormGroundsRejection = GroundsRejectionCause.GetForm(glbIndexGroundsRejection);        
        if ($("#formGroundsRejection").valid()) {
            if (FormGroundsRejection.Id == null) {
                FormGroundsRejection.StatusTypeService = ParametrizationStatus.Create;
                $("#listGroundsRejection").UifListView("addItem", FormGroundsRejection);
            } else {
                
                FormGroundsRejection.StatusTypeService = ParametrizationStatus.Update;
                $("#listGroundsRejection").UifListView("editItem", glbIndexGroundsRejection,FormGroundsRejection);
            }
            GroundsRejectionCause.clearFormGroundsRejection();      
            
        }

    }

    /**
    * @summary Creacion del data del formulario
    */
    static GetForm(glbIndexGroundsRejection) {
        var data = {};
        data.Id = glbIdGroundsRejection;
        data.GroupPolicies = { Id: $("#IdPolicyGroup").val(), description: $("#IdPolicyGroup").UifSelect("getSelectedText") };
        data.description = $("#IdDescriptionRejection").val();
        data.StatusTypeService = ParametrizationStatus.Origin;
        return data;
    }

    /**
    * @summary redirecciona al inicio 
    */
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    static confirmCreateBaseRejection(data) {        
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate,
            'autoclose': true
        });
        AjaxGroupRelection.GetRejectionCausesAll().done(function (data) {
            GroundsRejectionCause.loadRejectionCauses(data)
        });
    }
    /**
    * @summary Elimina registro en listview y agrega a lista para eliminar en base
    */
    deleteRejectionCause(event, result)
    {        
        if (result != null && result.id != null) {
            // Agregar al Array de Registros a Eliminar
            result.StatusTypeService = ParametrizationStatus.Delete;
            glbRejectionCauseDeleted.push(result);
        }
        // Continuar con la accion del event
        event.resolve();
    }
    /**
    * @summary Exportar archivo excel.
    */
    exportExcel() {
        request('ParamAuthorizationPolicies/GroundsRejection/GenerateFileToExport', null, 'GET', "Error descargando archivo", GroundsRejectionCause.generateFileToExport);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }

}