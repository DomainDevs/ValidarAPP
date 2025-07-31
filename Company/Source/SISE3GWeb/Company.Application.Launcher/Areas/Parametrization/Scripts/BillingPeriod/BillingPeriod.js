var glbBillingPeriod = {};




class BillingPeriod {

    /**
    *@summary Obtiene la lista de Periodos de facturación .
    *@returns {Array} Lista de Periodos de facturación .
    */
    static GetBillingPeriod() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/BillingPeriod/GetBillingPeriod',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza los procesos del CRUD
    */
    static GenerateFileToExportBillingPeriod() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/BillingPeriod/GenerateFileToExportBillingPeriod',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class BillingPeriodControls {   
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get BillingPeriodTemplate() { return "#BillingPeriodTemplate"; }
    static get listBillingPeriod() { return $("#listBillingPeriod"); }    
    static get btnExit() { return $("#btnExit"); }        
    static get inputDescription() { return $("#inputDescription"); }   
    static get inputBILLING_PERIOD_CD() { return $("#inputBILLING_PERIOD_CD"); }   
    static get tableResults() { return $("#tableResults"); }
    static get formBillingPeriod() { return $("#formBillingPeriod"); }    
    static get sendExcelBillingPeriod() { return $("#btnExport"); }        
}

class ParametrizationBillingPeriod extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);        
        BillingPeriodControls.listBillingPeriod.UifListView({
            displayTemplate: "#BillingPeriodTemplate",
            edit: false,
            delete: false,
            deleteCallback: this.DeleteItemBillingPeriod,
            customAdd: false,
            customEdit: false,
            height: 300
        });

        /**
        * @summary Inicializa la lista 
        * @returns {Array} Lista de de
        */
        BillingPeriod.GetBillingPeriod().done(function (data) {
            if (data.success) {
                glbBillingPeriod = data.result;
                ParametrizationBillingPeriod.LoadBillingPeriod();
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
        BillingPeriodControls.btnExit.on("click", this.Exit);
        BillingPeriodControls.sendExcelBillingPeriod.on("click", this.sendExcelBillingPeriod);       
    }
    
    /**
    * @summary Carga la lista de
    */
    static LoadBillingPeriod() {
        BillingPeriodControls.listBillingPeriod.UifListView("clear");
        $.each(glbBillingPeriod, function (key, value) {
            var lBillingPeriod = {
                BILLING_PERIOD_CD: this.BILLING_PERIOD_CD,                                          // oJO
                DESCRIPTION: this.DESCRIPTION               //OJO
            }
            BillingPeriodControls.listBillingPeriod.UifListView("addItem", lBillingPeriod);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelBillingPeriod() {
        BillingPeriod.GenerateFileToExportBillingPeriod().done(function (data) {
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
    *@summary Muestra los 
    *@param {dataTable} * Captura el control donde se mostrara la lista de
    */
    static ShowDefaultResult(dataTable) {
        BillingPeriodControls.tableResults.UifDataTable('clear');
        BillingPeriodControls.tableResults.UifDataTable('addRow', dataTable);
    }
    
    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

