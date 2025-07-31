var glbCoveredRiskType = {};

$(() => {
    new ParametrizationCoveredRiskType();
});


class CoveredRiskTypes {

    /**
    *@summary Obtiene la lista de tipos de riesgo cubiertos.
    *@returns {Array} Lista de tipos de riesgo cubiertos.
    */
    static GetCoveredRiskTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/CoveredRiskType/GetCoveredRiskTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD para las Sucursales
    */
    static GenerateFileToExportCoveredRiskTypes() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExportCoveredRiskTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class CoveredRiskTypeControls {   
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get CoveredRiskTypeTemplate() { return "#CoveredRiskTypeTemplate"; }
    static get listCoveredRiskType() { return $("#listCoveredRiskType"); }    
    static get btnExit() { return $("#btnExit"); }        
    static get inputLongDescription() { return $("#inputLongDescription"); }
    static get inputShortDescription() { return $("#inputShortDescription"); }    
    static get inputId() { return $("#inputId"); }
    static get tableResults() { return $("#tableResults"); }
    static get formCoveredRiskType() { return $("#formCoveredRiskType"); }    
    static get sendExcelCoveredRiskType() { return $("#btnExport"); }        
}

class ParametrizationCoveredRiskType extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);        
        CoveredRiskTypeControls.listCoveredRiskType.UifListView({
            displayTemplate: "#CoveredRiskTypeTemplate",
            edit: false,
            delete: false,
            deleteCallback: this.DeleteItemCoveredRiskType,
            customAdd: false,
            customEdit: false,
            height: 300
        });

        /**
        * @summary Inicializa la lista de Perfiles de Asegurado
        * @returns {Array} Lista de de Perfiles de Asegurado
        */
        CoveredRiskTypes.GetCoveredRiskTypes().done(function (data) {
            if (data.success) {
                glbCoveredRiskType = data.result;
                ParametrizationCoveredRiskType.LoadCoveredRiskTypes();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });


       

    }
    //---------------------EVENTOS -------------------//

    /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {        
        CoveredRiskTypeControls.btnExit.on("click", this.Exit);
        CoveredRiskTypeControls.sendExcelCoveredRiskType.on("click", this.sendExcelCoveredRiskType);       
    }
    
    /**
    * @summary Carga la lista de tipos de riesgo cubiertos.
    */
    static LoadCoveredRiskTypes() {
        CoveredRiskTypeControls.listCoveredRiskType.UifListView("clear");
        $.each(glbCoveredRiskType, function (key, value) {
            var lCoveredRiskTypes = {
                Id: this.Id,
                LongDescription: this.LongDescription,
                ShortDescription: this.ShortDescription
            }
            CoveredRiskTypeControls.listCoveredRiskType.UifListView("addItem", lCoveredRiskTypes);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelCoveredRiskType() {
        CoveredRiskTypes.GenerateFileToExportCoveredRiskTypes().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
    }
       
    /**
    *@summary Muestra los tipos de riesgo cubiertos Encontrados
    *@param {dataTable} * Captura el control donde se mostrara la lista de tipos de riesgo cubiertos encontrados
    */
    static ShowDefaultResult(dataTable) {
        CoveredRiskTypeControls.tableResults.UifDataTable('clear');
        CoveredRiskTypeControls.tableResults.UifDataTable('addRow', dataTable);
    }
    
    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

