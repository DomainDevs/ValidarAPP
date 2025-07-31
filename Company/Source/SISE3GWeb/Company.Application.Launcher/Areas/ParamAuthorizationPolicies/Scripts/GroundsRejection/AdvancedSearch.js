// Variable global
var dropDownSearchAdvGroundsRejection;

class AdvancedSearchGroundsRejection extends Uif2.Page {

    // inicializa eventos 
    getInitialState() {        
        dropDownSearchAdvGroundsRejection = uif2.dropDown({
            source: rootPath + 'ParamAuthorizationPolicies/GroundsRejection/AdvancedSearch',
            element: '#btnSearchAdvScript',
            align: 'right',
            width: 500,
            height: 530
        });
        
    }

    // Ejecucion de funciones segun evento
    bindEvents() {
        $('#btnSearchAdvScript').click(AdvancedSearchGroundsRejection.ShowAdvancedSearch);
        $("#btnCancelSearchScript").click(AdvancedSearchGroundsRejection.CancelSearchAdvanzed);
    }

    // Muestra la vista parcial
    static ShowAdvancedSearch() {
        dropDownSearchAdvGroundsRejection.show();
        $("#listviewSearch").UifListView({
            displayTemplate: "#searchTemplate",
            selectionType: 'single',
            source: null,
            height: 200
        });
    }

    // Limpia formulario de busqueda avanzada
    static CleanAdvancedSearch() {          
    }

    // Oculta busqueda avanzada
    static CancelSearchAdvanzed() {
        dropDownSearchAdvGroundsRejection.hide();
    }
}

