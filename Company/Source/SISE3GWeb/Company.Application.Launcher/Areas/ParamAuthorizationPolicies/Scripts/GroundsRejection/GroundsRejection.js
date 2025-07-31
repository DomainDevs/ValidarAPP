// Variables de entorno
var glbIdGroundsRejection;

/**
    * @summary Clase del formulario principal
    */
class GroundsRejection extends Uif2.Page {

    //---------------------INICIALIZADOR -------------------//

     /**
    * @summary Inicializa el estado de la vista
    */
    getInitialState() {

        $("#listGroundsRejection").UifListView({
            displayTemplate: "#templateGroundsRejection",
            source: null,
            selecttionType: 'single',
            height: 400
        });
        //request('Parametrization/GroundsRejection/GetParametrizationGroundsRejection', null, 'GET', AppResources.ErrorSearchClauses, GroundsRejection.getGroundsRejection);
    }

     //---------------------EVENTOS -------------------//

   /**
    * @summary Captura los eventos de los controles
    */
    bindEvents() {
        $("#btnGroundsRejectionNew").on("click", GroundsRejection.clearFormGroundsRejection);
        $("#btnGroundsRejectionNew").on("click", GroundsRejection.AddGroundsRejection);
        $("#btnExitGroundsRejection").on("click", this.redirectIndex);
    }
    // Envia datos a ListView
    static getGroundsRejection(data) {
        $("#listGroundsRejection").UifListView({
            displayTemplate: "#templateGroundsRejection",
            sourceData: data,
            selectionType: 'single',
            height: 400,
            edit: true,
            customEdit: true

        });
    }

   /**
    * @summary Limpia formulario motivos de rechazo
    */
    static clearFormGroundsRejection() {
        ClearValidation("#formGroundsRejection");
        $("#IdPolicyGroup").UifSelect("disabled", false);
        $("#IdDescriptionRejection").val("");
    }

 /**
    * @summary Agrega a listview registros
    */
    static AddGroundsRejection() {
        var FormGroundsRejection = GroundsRejection.GetForm();       
        if ($("#formGroundsRejection").valid()) {
            if (FormGroundsRejection.Id == null) {
                FormGroundsRejection.StatusType = ParametrizationStatus.Create;
                $("#listGroundsRejection").UifListView("addItem", FormGroundsRejection);
            } else {
                FormGroundsRejection.StatusType = ParametrizationStatus.Update;
                $("#listGroundsRejection").UifListView("editItem", FormGroundsRejection);
            }
                        
        }

    }

    /**
    * @summary Creacion del data del formulario
    */
    static GetForm() {
        var data = {};
        data.Id = glbIdGroundsRejection;
        data.GrupoPolitica = $("#IdPolicyGroup").UifSelect("getSelectedText");
        data.DescripcionRechazo = $("#IdDescriptionRejection").val();
        data.StatusType = ParametrizationStatus.Origin;
        return data;
    }

    /**
    * @summary redirecciona al inicio 
    */
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

}