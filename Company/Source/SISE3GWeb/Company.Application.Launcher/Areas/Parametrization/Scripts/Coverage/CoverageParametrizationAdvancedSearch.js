//Clase Busqueda avanzada
$.ajaxSetup({ async: true });
var dropDownSearchAdvCoverage;
class CoverageParametrizationAdvanced extends Uif2.Page {
    /**
     * @summary 
        *  Metodo que se ejecuta al instanciar la clase     
     */
    getInitialState() {        
        dropDownSearchAdvCoverage = uif2.dropDown({
            source: rootPath + 'Parametrization/Coverage/AdvancedSearch',
            element: '#btnShowAdvancedCoverage',
            align: 'right',
            width: 550,
            height: 551,
            container: "#main",
            loadedCallback: CoverageParametrizationAdvanced.componentLoadedCallback
        });
    }

    //EVENTOS CONTROLES

    /**
    * @summary 
        *  Metodo con los eventos de todos los controles 
    */
    bindEvents() {
        $("#btnShowAdvancedCoverage").on("click", this.ShowAdvanced);         
    }

    /**
    * @summary 
        *  Mostrar y cargar busqueda avanzada
    */    
    ShowAdvanced() {        
        dropDownSearchAdvCoverage.show();
        CoverageParametrizationAdvanced.ClearAdvanced();
    }

    /**
    * @summary 
        *  Metodo para cargar configuracion de la ventana de busqueda se deben añadir todos los controles  y eventos de la ventana en este metodo
    */
    static componentLoadedCallback() {

        request('Parametrization/Coverage/GetLineBusiness', null, 'GET', AppResources.ErrorSearchLineBusinessCONNEX, CoverageParametrizationAdvanced.getLineBusiness)

        $('#selectLineBusinessAdv').on('itemSelected', function (event, item) {
            CoverageParametrizationAdvanced.ChangeLineBusiness(event, item)
        });

        $("#btnLoadCoverage").on("click", CoverageParametrizationAdvanced.selectedItem);

        $("#listViewSearchAdvanced").UifListView({
            displayTemplate: "#advSearchTemplate",
            selectionType: 'single',
            source: null,
            height: 240
        });

        $("#btnCancelSearch").on('click', function () {
            CoverageParametrizationAdvanced.ClearAdvanced();
            dropDownSearchAdvCoverage.hide();
        })

        $('#btnAdvancedSearch').on("click", function myfunction() {           
            var params = $("#formAdv").serializeObject();
            if (params.Description === "" && params.PerilDescription === "" && params.LineBusinessId === "" && params.InsuredObjectId === "")
            {
                $.UifNotify("show", { 'type': "info", 'message': AppResources.EnterParameterSearch, 'autoclose': true });        
            }
            else if($("#formAdv").valid())
            {
                params = {
                    Description: params.Description, Peril: { Description: params.PerilDescription }, InsuredObject: { Id: params.InsuredObjectId }, LineBusiness: { Id: params.LineBusinessId}
                };
                request('Parametrization/Coverage/GetCoverageSMBySearchAdv', JSON.stringify({ coverageServiceModel: params }), 'POST', AppResources.ErrorGetCoverageByRelationCONNEX, CoverageParametrizationAdvanced.ShowSearchAdv)
            }
            
        });

    }

    static ChangeLineBusiness(event, selectedItem) {
        $("#inputCoverageNameAdv").val('');
        if (selectedItem.Id > 0) {
            request('Parametrization/Coverage/GetInsuredObjectsByLineBusinessId', JSON.stringify({ lineBusinessId: selectedItem.Id }), 'POST', AppResources.ErrorSearchInsuredObjectCONNEX, CoverageParametrizationAdvanced.getSubLineBusinessByLineBusinessId)            
        }
        else {
            $("#selectInsuranceObjectAdv").UifSelect();         
        }
    }

    static getSubLineBusinessByLineBusinessId(data)
    {
        $("#selectInsuranceObjectAdv").UifSelect({ sourceData: data.items});        
    }

    static getLineBusiness(data) {
        $("#selectLineBusinessAdv").UifSelect({ sourceData: data});
    }
    
    //METODOS ADICIONALES 

    /**
    * @summary 
     * Metodo para limpiar los campos
    */
    static ClearAdvanced() {
        $("#listViewSearchAdvanced").UifListView({ sourceData: null });
        $("#selectLineBusinessAdv").UifSelect("setSelected", null);
        $("#selectInsuranceObjectAdv").UifSelect();
        $("#inputPerilNameAdv").val('');
        $("#inputCoverageNameAdv").val('');
        $("#PerilDescription").val('');
    }


     /**
    * @summary 
     * Metodo para mostrar el resultado de la consulta en la listview
    */
    static LoadCoveragesAdvanced(coverages) {
        $("#listViewSearchAdvanced").UifListView("refresh");
        $.each(coverages, function (index, val) {
            $("#listViewSearchAdvanced").UifListView("addItem", coverages[index]);
        });
    }


    static ShowSearchAdv(data) {
        if (data.length > 0) {
            $("#listViewSearchAdvanced").UifListView({
                displayTemplate: "#advSearchTemplate",
                selectionType: 'single',
                sourceData: data,
                height: 240
            });
            dropDownSearchAdvCoverage.show();
        } else {
            $("#listViewSearchAdvanced").UifListView("clear");
        }
    }

    static selectedItem() {
        dropDownSearchAdvCoverage.hide();
        var coverageSelected = $("#listViewSearchAdvanced").UifListView("getSelected");
        if (parseInt(coverageSelected[0].Id) >= 0) {
            
            request('Parametrization/Coverage/GetCoverageSQMByDescriptionTechnicalBranchId', JSON.stringify({ description: coverageSelected[0].Id }), 'POST', AppResources.ErrorGetCoverageBy, CoverageParametrization.setCoverageSearch);            
            //en caso de traer toda la informacion se llamaria el metodo: //CoverageParametrization.setCoverageSearch([coverageSelected[0]]);
        }
    }
}