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
            height: 530,
            loadedCallback: this.AdvancedSearchEvents
        });
        
    }

    // Ejecucion de funciones segun evento
    bindEvents() {
        $('#btnSearchAdvScript').click(AdvancedSearchGroundsRejection.ShowAdvancedSearch);
        
    }
    // Eventos pre cargados
    AdvancedSearchEvents() {
        $("#btnCancelSearchScriptRejection").on("click", AdvancedSearchGroundsRejection.CancelSearchAdvanzed);
        $('#btnAdvancedSearchListView').on('click', AdvancedSearchGroundsRejection.GetSearchRejectionCauses);
        $('#inputGroundsRejectionSearch').on('buttonClick', AdvancedSearchGroundsRejection.GetSearchRejectionCause);
        $("#btnLoadRejectionCauses").on("click", AdvancedSearchGroundsRejection.GetLoadAdvanzeSearch);
        
          }

    static GetLoadAdvanzeSearch() {        
        var itemSelected = $("#listviewSearch").UifListView("getSelected");
        AdvancedSearchGroundsRejection.CleanAdvancedSearch();
        dropDownSearchAdvGroundsRejection.hide();
        if (itemSelected != "") {
            var data = [];
            data.result = itemSelected;
            AdvancedSearchGroundsRejection.editRejectionCause(data);
        }
    }
    // Muestra la vista parcial
    static ShowAdvancedSearch() {
        dropDownSearchAdvGroundsRejection.show();
        AdvancedSearchGroundsRejection.GetGroupPolicies(); 
        $("#listviewSearch").UifListView({
            displayTemplate: "#searchTemplate",
            selectionType: 'single',
            source: null,
            height: 200
        });
    }

    // Limpia formulario de busqueda avanzada
    static CleanAdvancedSearch() {  
        $("#IdDescriptionRejectionSearchAdvanzed").val("");
        $("#IdPolicyGroupSearchAdvanzed").val("");
    }

    // Oculta busqueda avanzada
    static CancelSearchAdvanzed() {
        dropDownSearchAdvGroundsRejection.hide();
    }

    /**
   * @summary Envia grupo de policies 
   */
    static GetGroupPolicies() {
        AjaxGroupRelection.GetGroupPolicie().done(response => {
            let result = response.result;
            if (response.success) {
                $("#IdPolicyGroupSearchAdvanzed").UifSelect({ sourceData: result });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static GetSearchRejectionCauses() {
        
        let GroupPolicies = $("#IdPolicyGroupSearchAdvanzed").val();
        let Description = $("#IdDescriptionRejectionSearchAdvanzed").val();
        if (GroupPolicies == "") {
            GroupPolicies = 0;
        }        
        AjaxGroupRelection.SearchAdvancedRejectionCauses(Description, GroupPolicies).done(function (data) {
                    AdvancedSearchGroundsRejection.LoadListViewSearchAvanzed(data);
                });
        
    }

    static GetSearchRejectionCause() {        
        let GroupPolicies = $("#IdPolicyGroupSearchAdvanzed").val();
        let Description = $("#inputGroundsRejectionSearch").val();
        if (GroupPolicies == null) {
            GroupPolicies = 0;
        }
        if (Description.length > 2) {
            AjaxGroupRelection.SearchAdvancedRejectionCauses(Description, GroupPolicies).done(function (data) {
                if (data.result.length > 1) {
                    dropDownSearchAdvGroundsRejection.show();
                    $("#listviewSearch").UifListView({
                        displayTemplate: "#searchTemplate",
                        selectionType: 'single',
                        source: null,
                        height: 200
                    });
                    AdvancedSearchGroundsRejection.LoadListViewSearchAvanzed(data);
                }
                else {                    
                    AdvancedSearchGroundsRejection.editRejectionCause(data);
                }
            });
          }
        else {
            $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorSearchAdvancedMiniumCharacters, 'autoclose': true });
        }
    }

    static LoadListViewSearchAvanzed(data) {
        if (data.success) {
            // Limpiar ListView
            $("#listviewSearch").UifListView("clear");

            // Llenar objeto con los datos obtenidos
            $.each(data.result, function (key, value) {
                // Agregar objeto al ListView

                $("#listviewSearch").UifListView("addItem", value);
            });
        }
    }

    static editRejectionCause(data) {        
        if (data != null)           
            $.each(data.result, function (key, value) {                  
                var item = data.result[0]; 
                var lista = $("#listGroundsRejection").UifListView('getData')
                var index = lista.findIndex(function (item) {
                    return item.id == value.id;
                });
                glbIndexGroundsRejection = index;
         });
        glbIdGroundsRejection = data.result[0].id;
        $("#IdPolicyGroup").UifSelect("setSelected", data.result[0].GroupPolicies.id);
        $("#IdDescriptionRejection").val(data.result[0].description);
    }
}

