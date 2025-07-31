//Clase formulario principal
var formValidationPlate = {};
var glbValidationPlate;
var glbVersionVehicleFascolda = [];
var glbMakes = [];
var glbModels = [];
var glbVersions = [];
var fasecoldCode = '';
var windowSearchAdv = false;
var glbValidationPlateDelete = [];
var glbValidationPlate = {};
var ValidationPlateIndex = null;
class ValidationPlateParametrization extends Uif2.Page {

    //Iniciar formulario
    getInitialState() {

        //$('#Cod_Make').on("itemSelected", ValidationPlateParametrization.GetModel);
        ValidationPlateParametrization.GetMakes(0);
        ValidationPlateParametrization.GetCauses(0);
        $("#listViewValidationV").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: this.DeleteItemValidationPlate, displayTemplate: "#ValidationPlateTemplate", selectionType: 'single', height: 310 });
        ValidationPlateParametrizationRequest.GetListValidationPlate().done(function (data) {
            if (data.success) {
                ValidationPlateParametrization.loadValidationPlate(data.result);
                glbValidationPlate = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        //request('Parametrization/TextPrecatalogued/GetTexPrecataloged', null, 'GET', AppResources.ErrorSearchClauses, TextPrecataloguedParametrization.getTextPrecatalogued);

    }

    //eventos Formulario
    bindEvents() {
        $('#btnAccept').click(this.ValidationPlateAdd);
        $("#CodMake").on('itemSelected', this.ChangeMake);
        $("#CodModel").on('itemSelected', this.ChangeModel);
        $("#CodVersion").on('itemSelected', this.ChangeVersion);
        $("#CodFasecoldaText").focusin(this.FasecoldaCodeFocusIn);
        $("#CodFasecoldaText").focusout(this.FasecoldaCodeFocusOut);
        $('#btnTextPretacaloguedNew').click(ValidationPlateParametrization.clearFormValidationPlate);
        //editar item del list view
        $('#listViewValidationV').on('rowEdit', ValidationPlateParametrization.showData);
        //Buscar placa
        $('#inputValidateVehicleSearch').on('buttonClick', this.SearchDocument);
        //bton Agregar
        $('#btnAcceptVP').on('click', ValidationPlateParametrization.AddItem);
        //boton nuevo
        $('btnValidationPlateNew').on('click', ValidationPlateParametrization.clearPanel)
    }
    //agregar a list View 
    ValidationPlateAdd() {
    }

    SearchDocument() {
        $('#inputDocumentbutton').prop('disabled', 'disabled');
        if ($("#selectSearchPersonType").UifSelect("getSelected") == "" || $("#selectSearchPersonType").UifSelect("getSelected") == null) {
            $.UifNotify('show', {
                'type': 'info', 'message': AppResources.SelectTypePerson
            })
        }
        else {
            if ($('#inputDocument').val().trim() != "") {
                searchType = parseInt($("#selectSearchPersonType").UifSelect('getSelected'), 10);
                if (searchType == TypePerson.PersonNatural) {
                    Persons.GetPersonsByDocumentNumberNameSearchType($('#inputDocument').val().trim(), searchType);
                }
                if (searchType == TypePerson.PersonLegal) {
                    Persons.GetCompaniesByDocumentNumberNameSearchType($('#inputDocument').val().trim(), searchType);
                }
                if (searchType == TypePerson.ProspectNatural) {
                    modalListType = 5;
                    Persons.GetProspectByDocumentNumSearchType($('#inputDocument').val().trim(), searchType);
                }
                if (searchType == TypePerson.ProspectLegal) {
                    modalListType = 6;
                    Persons.GetProspectByDocumentNumSearchType($('#inputDocument').val().trim(), searchType);
                }

                $('#inputDocumentbutton').removeAttr('disabled');
            }
        }
    }

    ChangeMake(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ValidationPlateParametrization.GetModelsByMakeId(selectedItem.Id, 0);
        }
        else {
            $('#CodModel').UifSelect();
            $('#CodVersion').UifSelect();
            $('#CodCause').val(0);
        }
        $('#CodFasecoldaText').val('');
    }
    //evento cuando cambia el item de la lista de modelos  
    ChangeModel(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ValidationPlateParametrization.GetVersionsByMakeIdModelId($("#CodMake").UifSelect("getSelected"), selectedItem.Id, 0);
        }
        else {
            $('#CodModel').UifSelect();
            $('#CodVersion').UifSelect();
            $("#CodCause").UifSelect();
        }
        $('#CodFasecoldaText').val('');
    }

    DeleteItemValidationPlate(deferred, data) {
        deferred.resolve();
        var LtsValidationVeh = $("#listViewValidationV").UifListView('getData');
        $.each(LtsValidationVeh, function (index, value) {
            if (this.Plate == data.Plate) {
                if (value.Status != ParametrizationStatus.Create) {
                    value.Status = ParametrizationStatus.Delete;
                    if (glbValidationPlateDelete.length > 0) {
                        $.each(glbValidationPlateDelete, function (index, item) {

                            if (item.Plate != value.Plate) {
                                glbValidationPlateDelete.push(value);
                                $("#listViewValidationV").UifListView("addItem", this);
                            }
                        });
                    }
                    else {
                        glbValidationPlateDelete.push(value);
                        value.allowEdit = false;
                        value.allowDelete = false;
                        $("#listViewValidationV").UifListView("addItem", this);
                    }
                }
                else {
                    value.StatusTypeService = ParametrizationStatus.Original;
                    //value.allowEdit = false;
                    //value.allowDelete = false;
                    //$("#listViewLimitRc").UifListView("addItem", this);
                }
            }
        });
        //LimitRc.clearPanel();

    }


    ChangeVersion(event, selectedItem) {
        if (selectedItem.Id > 0) {
            ValidationPlateParametrization.GetFasecoldaCodeByMakeIdModelIdVersionId($("#CodMake").UifSelect("getSelected"), $("#CodModel").UifSelect("getSelected"), $("#CodVersion").UifSelect("getSelected"));
        }
        else {
            $('#CodFasecoldaText').val('');           
        }
    }

    FasecoldaCodeFocusIn() {
        fasecoldCode = $("#CodFasecoldaText").val().trim();
    }

    FasecoldaCodeFocusOut() {
        if ($("#CodFasecoldaText").val().trim().length > 0 && $("#CodFasecoldaText").val().trim() != fasecoldCode) {
            ValidationPlateParametrization.GetFasecoldaByCode($("#CodFasecoldaText").val().trim());
        }
    }

    static loadValidationPlate(ValidationPlate) {
        if (Array.isArray(ValidationPlate)) {
            if (ValidationPlate != undefined) {
                ValidationPlate.forEach(item => {
                    item.StatusTypeService = ParametrizationStatus.Original;                    
                    $("#listViewValidationV").UifListView("addItem", item);
                });
            }
        }

        //$("#listViewLimitRc").UifListView({ sourceData: LimitRc, add: false, edit: true, customEdit: true, delete: true, deleteCallback: this.DeleteItemLimitRc, displayTemplate: "#LimitRcTemplate", selectionType: 'single', height: 310 });

    }
    //cargar daytos de list view a controles form
    static showData(event, result, index) {
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        else if (result.length > 1) {
            LimitRc.ShowSearchAdv(result);
        }
        if (result.Plate != undefined) {
            ValidationPlateIndex = index;
            $("#PlateText").val(result.Plate);
            $("#MotorText").val(result.Motor);
            $("#ChassisText").val(result.Chassis);
            $("#CodFasecoldaText").val(result.CodFasecolda);
            ValidationPlateParametrization.GetFasecoldaByCode(result.CodFasecolda.trim());
            $("#IsEnabled").val();
            $("#CodCause").UifSelect('setSelected', result.CodCause);
            if (result.IsEnabled) {
                $('#IsEnabled').prop("checked", true);
            } else {
                $('#IsEnabled').prop("checked", false);
            }
            
        }
    }
    //limpiar form
    static clearFormValidationPlate() { }
    //Cargar lisView
    static LoadListViewValidationPlate(data) {
        $("#listViewValidationV").UifListView({ displayTemplate: "#VehicleTemplate", edit: true, delete: true, customAdd: true, customEdit: true, deleteCallback: ValidationPlateParametrization.DeleteItemBrancht, height: 300 });
        data.forEach(item => {
            var object = {
                Description: item.Branch.Description,
                BranchId: item.Branch.Id,
                IsIssue: item.IsIssue,
                Adress: item.Address,
                City: item.City.Id,
                Country: item.Country.Id,
                Id: item.Id,
                PhoneNumbre: item.PhoneNumber,
                PhoneType: item.PhoneType.Id,
                State: item.State.Id,
                SmallDescription: item.Branch.SmallDescription,
                AddressType: item.AddressType.Id
            }
            if (object.IsIssue) {
                object.IsIssue = Resources.Language.IsIssue;
            } else {
                object.IsIssue = Resources.Language.IsNotIssue;
            }
            $("#listViewBranch").UifListView("addItem", object);
        });
    }
    static DeleteItemBrancht(deferred, result) {
        deferred.resolve();
        if (result.Id !== 0 && result.Id !== "" && result.Id !== undefined) //Se elimina unicamente si existe en DB
        {
            result.Status = ParametrizationStatus.Delete;
            //glbObjectsDelete.push(result);
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listViewValidationV").UifListView("addItem", result);
        }

        BranchParametrization.Clear();
    }
    static GetFasecoldaByCode(code) {
        if (code.length == 8) {
            ValidationPlateParametrizationRequest.GetFasecoldaByCode(code, 0).done(function (data) {
                if (data.success) {
                    ValidationPlateParametrization.LoadVehicleByFasecolda(data.result);
                }
                else {
                    ValidationPlateParametrization.ClearFasecolda();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                ValidationPlateParametrization.ClearFasecolda();
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchFasecolda, 'autoclose': true });
            });
        }
    }
    //limpiar fasecolda
    static ClearFasecolda() {
        $("#CodMake").UifSelect("setSelected", null);
        $('#CodModel').UifSelect();
        $('#CodVersion').UifSelect();
    }
    static LoadVehicleByFasecolda(vehicle) {
        ValidationPlateParametrization.GetMakes(vehicle.MakeId);
        ValidationPlateParametrization.GetModelsByMakeId(vehicle.MakeId, vehicle.ModelId);
        ValidationPlateParametrization.GetVersionsByMakeIdModelId(vehicle.MakeId, vehicle.ModelId, vehicle.VersionId);
    }
    static GetCauses(selectedId) {
        ValidationPlateParametrizationRequest.GetCauses().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#CodCause").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#CodCause").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static GetMakes(selectedId) {
        ValidationPlateParametrizationRequest.GetMakes().done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#CodMake").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#CodMake").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static GetModelsByMakeId(makeId, selectedId) {
        ValidationPlateParametrizationRequest.GetModelByMake(makeId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#CodModel").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#CodModel").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static GetVersionsByMakeIdModelId(makeId, modelId, selectedId) {
        ValidationPlateParametrizationRequest.GetVersionsByMakeIdModelId(makeId, modelId).done(function (data) {
            if (data.success) {
                if (selectedId == 0) {
                    $("#CodVersion").UifSelect({ sourceData: data.result });
                }
                else {
                    $("#CodVersion").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static GetFasecoldaCodeByMakeIdModelIdVersionId(makeId, modelId, versionId) {
        ValidationPlateParametrizationRequest.GetFasecoldaCodeByMakeIdModelIdVersionId(makeId, modelId, versionId).done(function (data) {
            if (data.success) {
                $("#CodFasecoldaText").val(data.result.FasecoldaCode);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchFasecolda, 'autoclose': true });
        });
    }
    static AddItem() {
        $("#formValidationPlate").validate();
        if ($("#formValidationPlate").valid()) {

               
                    var ValidationPlateNew = {};            
            ValidationPlateNew.Plate = $("#PlateText").val();
            ValidationPlateNew.Motor = $("#MotorText").val();
            ValidationPlateNew.Chassis = $("#ChassisText").val();
            ValidationPlateNew.CodFasecolda = $("#CodFasecoldaText").val();
            ValidationPlateNew.MakeId = $("#CodMake").UifSelect("getSelected");
            ValidationPlateNew.ModelId = $("#CodModel").UifSelect("getSelected");
            ValidationPlateNew.VersionId = $("#CodVersion").UifSelect("getSelected");
            ValidationPlateNew.CauseId = $("#CodCause").UifSelect("getSelected");

            if (ValidationPlateIndex == null) {
                //        ValidationPlateNew.StatusTypeService = ParametrizationStatus.Create;
                //var ifExist = $("#listViewValidationV").UifListView('getData').filter(function (item) {
                //            return item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase();
                //        });
                //        if (ifExist.length > 0) {
                //            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistLimitRc, 'autoclose': true });
                //        }
                //        else {
                //            $("#listViewValidationV").UifListView("addItem", ValidationPlateNew);
                //        }
                    }
                    else {
                        ValidationPlateNew.Status = ParametrizationStatus.Update;
                var ifExist = $("#listViewValidationV").UifListView('getData').filter(function (item) {
                    return item.Plate.toUpperCase() == $("#PlateText").val().toUpperCase();
                        });
                        if (ifExist.length > 0) {
                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorExistLimitRc, 'autoclose': true });
                        }
                $('#listViewValidationV').UifListView('editItem', ValidationPlateIndex, ValidationPlateNew);
                    }
            ValidationPlateParametrization.clearPanel();              
           
        };
    }
    static clearPanel() {
        $("#PlateText").val('');
        $("#MotorText").val('');
        $("#ChassisText").val('');
        $("#CodFasecoldaText").val('');
        $("#CodMake").UifSelect('setSelected', null);
        $("#CodCause").UifSelect('setSelected', null);
        $("#CodMake").UifSelect('setSelected', null);
        $("#CodModel").UifSelect('setSelected', null); 
        $("#CodVersion").UifSelect('setSelected', null); 
        $('#IsEnabled').prop("checked", false);                
    }
    static GetForm() {
        var data = {};
        $("#formBranch").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.Description = $("#inputDescription").val();
        data.SmallDescription = $("#inputSmallDescription").val();
        data.Id = $("#Id").val();
        data.BranchId = $("#Id").val();
        data.IsIssue = $("#chkIsIssue").is(":checked");
        data.Adress = $("#inputAdress").val();
        data.City = $("#selectCity").UifSelect("getSelected");
        data.Country = $("#selectCountry").UifSelect("getSelected");
        data.PhoneNumbre = $("#inputPhoneNumbre").val();
        data.PhoneType = $("#selectPhoneType").UifSelect("getSelected");
        data.State = $("#selectState").UifSelect("getSelected");
        data.AddressType = $("#selectAddressType").UifSelect("getSelected");

        return data;
    }


}