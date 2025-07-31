var dropDownSearchAdvFasecolda = null;
var evaluate = false;
var windowSearchAdv = false;

class AdvancedSearchFasecolda extends Uif2.Page {
    getInitialState() {
        dropDownSearchAdvFasecolda = uif2.dropDown({
            source: rootPath + 'Parametrization/Vehicle/AdvancedSearchFasecolda',
            element: '#btnShowAdvancedSearchFsecolda',
            align: 'right',
            width: 500,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });
        $("#selectBrandVehicleAdvSearch").UifSelect({ sourceData: glbMakes });
      
    }
    ////////eventos de busqueda avanzada//////////////////////
    bindEvents() {
        //$("#selectBrandVehicleAdvSearch").on('itemSelected', AdvancedSearchFasecolda.ChangeMakeAdv);
        //$("#selectModelVehicleAdvSearch").on('itemSelected', AdvancedSearchFasecolda.ChangeModelAdv);
        //$("#selectVersionVehicleAdvSearch").on('itemSelected', AdvancedSearchFasecolda.ChangeVersionAdv);
        
    }

    componentLoadedCallback() {
        $("#listviewSearchADvFasecolda").UifListView({
            source: null,
            displayTemplate: "#searchTemplateFasecolda",
            selectionType: "single",
            height: 400
        });

        $("#selectBrandVehicleAdvSearch").on('itemSelected', AdvancedSearchFasecolda.ChangeMakeAdv);
        $("#selectModelVehicleAdvSearch").on('itemSelected', AdvancedSearchFasecolda.ChangeModelAdv);
        $("#selectVersionVehicleAdvSearch").on('itemSelected', AdvancedSearchFasecolda.ChangeVersionAdv);
        $("#selectBrandVehicleAdvSearch").UifSelect({ sourceData: glbMakes });
        $("#btnAdvancedSearchFasecolda").on("click", AdvancedSearchFasecolda.SearchFasecolda);
        $("#btnAcceptAdvancedSearchFasecolda").on("click", AdvancedSearchFasecolda.AcceptFasecolda);
        $("#btnCancelSearchFasecolda").on("click", AdvancedSearchFasecolda.CancelSearchAdv);
        
    }

    static ChangeMakeAdv(event, selectedItem) {
        windowSearchAdv = true;
        evaluate = windowSearchAdv;
        if (selectedItem.Id > 0) {
            Fasecolda.GetModelsByMakeId(selectedItem.Id, 0);            
        }       
    }

    static ChangeModelAdv(event, selectedItem) {
        windowSearchAdv = true;
        if (selectedItem.Id > 0) {
            Fasecolda.GetVersionsByMakeIdModelId($("#selectBrandVehicleAdvSearch").UifSelect("getSelected"), selectedItem.Id);            
        }
        else {            
        }
        $('#inputFasecoldaCode').val('');
    }

    static ChangeVersionAdv(event, selectedItem) {

        $("#InputBrandCodeFasecolda").val('');
        $("#InputVersionCodeFasecolda").val('');
        if (selectedItem.Id > 0) {
            Fasecolda.GetVersionsByMakeIdModelId($("#selectBrandVehicleAdvSearch").UifSelect("getSelected"), $("#selectModelVehicleAdvSearch").UifSelect("getSelected"), selectedItem.Id);
            
        }
    }
    

    static SearchFasecolda() {
        if (AdvancedSearchFasecolda.ValidateForm()){
            AdvancedSearchFasecolda.Initialize();
            
            VehicleFasecolda.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId($("#selectBrandVehicleAdvSearch").UifSelect('getSelected'),
                $("#selectModelVehicleAdvSearch").UifSelect('getSelected'), $("#selectVersionVehicleAdvSearch").UifSelect('getSelected'),
                $("#inputModeloCodeFasecoldaAdvSearch").val(), $("#inputModeloCodeFasecoldaAdvSearch").val());
            
            
            
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorNotFoundFilter, 'autoclose': true });
        }        
    }

    static ValidateForm() {
        evaluate = false;
        if ($("#selectBrandVehicleAdvSearch").UifSelect('getSelected') != "" && $("#selectBrandVehicleAdvSearch").UifSelect('getSelected') != null) {
            evaluate = true;
        }
        if ($("#selectModelVehicleAdvSearch").UifSelect('getSelected') != "" && $("#selectModelVehicleAdvSearch").UifSelect('getSelected') != null) {
            evaluate = true;
        }
        if ($("#selectVersionVehicleAdvSearch").UifSelect('getSelected') != "" && $("#selectVersionVehicleAdvSearch").UifSelect('getSelected') != null) {
            evaluate = true;
        }
        if ($("#inputModeloCodeFasecoldaAdvSearch").val() != "") {
            evaluate = true;
        }
        if ($("#inputModeloCodeFasecoldaAdvSearch").val() != "") {
            evaluate = true;
        }

        validate = evaluate;
        return evaluate;
        
    }

    static ConstructFasecoldaSearch() {
        
        ObjFasecolda.makeVehicle.Id = marcasList[0].makeVehicle.Id;
        ObjFasecolda.modelVehicle.Id = marcasList[0].modelVehicle.Id;
        ObjFasecolda.versionVehicle.Id = marcasList[0].versionVehicle.Id;
        ObjFasecolda.MakeVehicleCode = marcasList[0].MakeVehicleCode;
        ObjFasecolda.ModelVehicleCode = marcasList[0].ModelVehicleCode;
        ObjFasecolda.makeVehicle.Description = marcasList[0].makeVehicle.Description;
        ObjFasecolda.modelVehicle.Description = marcasList[0].modelVehicle.Description;
        ObjFasecolda.versionVehicle.Description = marcasList[0].versionVehicle.Description;
        
    }

    static Initialize() {

        ObjFasecolda =
            {
                MakeVehicleCode: null,
                ModelVehicleCode: null
            }
        ObjFasecolda.makeVehicle =
            {
                Id: null,
                Description: null
            }

        ObjFasecolda.modelVehicle =
            {
                Id: null,
                Description: null
            }
        ObjFasecolda.versionVehicle =
            {
                Id: null,
                Description: null
            }
    }

    static AcceptFasecolda() {
        var SelectedAdv = $("#listviewSearchADvFasecolda").UifListView("getSelected");        
        if (SelectedAdv.length > 0) {
            Fasecolda.ShowInformationToSearchAdv(SelectedAdv[0]);
            AdvancedSearchFasecolda.Clear();
            dropDownSearchAdvFasecolda.hide();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorListElement, 'autoclose': true });
        }        
    }


    static Clear() {
        $("#selectBrandVehicleAdvSearch").UifSelect("setSelected", null);
        $("#selectModelVehicleAdvSearch").UifSelect("setSelected", null);
        $("#selectVersionVehicleAdvSearch").UifSelect("setSelected", null);
        $("#inputModeloCodeFasecoldaAdvSearch").val(null);
        $("#inputModeloCodeFasecoldaAdvSearch").val(null);
        
        ObjFasecolda = {};
        validate = false;
        windowSearchAdv = false;

        $("#listviewSearchADvFasecolda").UifListView({
            source: null,
            displayTemplate: "#searchTemplateFasecolda",
            selectionType: "single",
            height: 400
        });
        //Fasecolda.CLear();
    }

    static CancelSearchAdv() {
        validate = false;
        dropDownSearchAdvFasecolda.hide();
    }
    
    static HideSearchAdv() {
        
    }
   
    static ShowSearchAdv(data) {
       
    }
   
    static ClearAdvanced() {
        
    }

   
}