var glbBusinessType = {};


class BusinessTypes {

    /**
    *@summary Obtiene la lista de tipos de negocios cubiertos.
    *@returns {Array} Lista de tipos de negocios cubiertos.
    */
    static GetBusinnesTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/BusinessType/GetBusinnesTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    /**
    *@summary Realiza proceso de exportacion a excel
    */
    static GenerateFileToExportBusinessTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/BusinessType/GenerateFileToExportBusinessTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class BusinessTypeControls {
    static get SelectorTypeText() { return $("input[type=text]"); }
    static get BusinessTypeTemplate() { return "#BusinessTypeTemplate"; }
    static get listBusinessType() { return $("#listBusinessType"); }
    static get btnExit() { return $("#btnExit"); }
    static get inputSMALL_DESCRIPTION() { return $("#inputSMALL_DESCRIPTION"); }
    static get inputBUSINESS_TYPE_CD() { return $("#inputBUSINESS_TYPE_CD"); }
    static get tableResults() { return $("#tableResults"); }
    static get formBusinessType() { return $("#formBusinessType"); }
    static get sendExcelBusinessType() { return $("#btnExport"); }
}

class ParametrizationBusinessType extends Uif2.Page {


    //---------------------INICIALIZADOR -------------------//

    /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        BusinessTypeControls.listBusinessType.UifListView({
            displayTemplate: "#BusinessTypeTemplate",
            edit: false,
            delete: false,
            deleteCallback: this.DeleteItemBusinessType,
            customAdd: false,
            customEdit: false,
            height: 300
        });

        /**
        * @summary Inicializa la lista 
        * @returns {Array} Lista de
        */
        BusinessTypes.GetBusinnesTypes().done(function (data) {
            if (data.success) {
                glbBusinessType = data.result;
                ParametrizationBusinessType.LoadBusinessType();
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
        BusinessTypeControls.btnExit.on("click", this.Exit);
        BusinessTypeControls.sendExcelBusinessType.on("click", this.sendExcelBusinessType);
    }

    /**
    * @summary Carga la lista de 
    */
    static LoadBusinessType() {
        BusinessTypeControls.listBusinessType.UifListView("clear");
        $.each(glbBusinessType, function (key, value) {
            var lBusinessType = {
                BUSINESS_TYPE_CD: this.BUSINESS_TYPE_CD,
                SMALL_DESCRIPTION: this.SMALL_DESCRIPTION
            }
            BusinessTypeControls.listBusinessType.UifListView("addItem", lBusinessType);
        });
    }

    /**
    * @summary Descarga archivo excel
    */
    sendExcelBusinessType() {
        BusinessTypes.GenerateFileToExportBusinessTypes().done(function (data) {
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
        BusinessTypeControls.tableResults.UifDataTable('clear');
        BusinessTypeControls.tableResults.UifDataTable('addRow', dataTable);
    }

    /**
   *@summary Redirecciona al index
   */
    Exit() {
        window.location = rootPath + "Home/Index";
    }
}

