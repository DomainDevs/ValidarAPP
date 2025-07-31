var glbRangeEntityDelete = [];
var glbRangeEntityValueDelete = [];
var dropDownAddRangeEntityValue;
var glbPositionEditRangeEntityValue;
class ConceptRangeEntity extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: true });
        request('RulesScripts/Concepts/GetRangeEntity', null, 'GET', Resources.Language.ErrMsgRangeEntityNotFound, ConceptRangeEntity.setRangeEntity);
        $("#Status").val(ParametrizationStatus.Create);
        ConceptRangeEntity.setRangeEntityValue(null);
        this.setDropDow();
    }
    bindEvents() {
        $("#btnCancelRangeEntity").click(ConceptRangeEntity.clearRangeEntity);
        $('#lsvRangeEntity').on('rowEdit', ConceptRangeEntity.editRangeEntity);
        $('#lsvRangeEntityValue').on('rowEdit', ConceptRangeEntity.editRangeEntityValue);
        $("#lsvRangeEntityValue").on('rowAdd', ConceptRangeEntity.showdropDownAddRangeEntityValue);
        $("#btnAddRangeEntity").on("click", ConceptRangeEntity.addRangeEntity);
        $("#btnExitRangeEntity").on("click", this.exit);
        $("#btnSaveRangeEntity").on("click", ConceptRangeEntity.SaveRangeEntity);
        $("#inputSearchRangeEntity").on("search", ConceptRangeEntityAdvanced.searchRangeEntity);
        $("#btnSearchAdvRange").on("click", ConceptRangeEntityAdvanced.showdropDownSearchAdvRange);        
    }    
    static clearRangeEntity() {
        $("#RangeEntityCode").val("");
        $("#RangeValueAt").val("");
        $("#Description").val("");
        $("#Description").focus();
        $("#Status").val(ParametrizationStatus.Create);
        $("#Index").val("");
        ConceptRangeEntity.hideAndClearDropDownAddRangeEntityValue();
        ConceptRangeEntity.setRangeEntityValue(null);
        glbRangeEntityValueDelete = [];
        ClearValidation("#formRangeEntity");
    }
    setDropDow() {
        dropDownAddRangeEntityValue = uif2.dropDown({
            source: rootPath + 'RulesScripts/Concepts/RangeEntityValueAdd',
            element: '#lsvRangeEntityValue .add-button',
            align: 'right',
            width: 400,
            height: 150,
            loadedCallback: ConceptRangeEntity.componentLoadedCallback
        });
    }
    static componentLoadedCallback() {
        $("#StatusRangeEntityValue").val(ParametrizationStatus.Create);
        $("#btnAddRangeEntityValue").on("click", ConceptRangeEntity.addRangeEntityValue);
        $("#btnCancelRangeEntityValue").on("click", ConceptRangeEntity.hideAndClearDropDownAddRangeEntityValue)
        $("#FromValue").ValidatorKey(ValidatorType.Number, 2, 0);
        $("#ToValue").ValidatorKey(ValidatorType.Number, 2, 0);
    }
    
    static setRangeEntity(data) {
        $("#lsvRangeEntity").UifListView({
            sourceData: data,
            displayTemplate: "#rangeEntityTemplate",
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: ConceptRangeEntity.deleteRangeEntity,
            height: 400
        });
    }

    static setRangeEntityValue(data) {
        $("#lsvRangeEntityValue").UifListView({
            sourceData: data,
            displayTemplate: "#rangeEntityValueTemplate",
            add: true,
            edit: true,
            delete: true,
            customAdd: true,
            customEdit: true,
            deleteCallback: ConceptRangeEntity.deleteRangeEntityValue,
            height: 250,
            title: Resources.Language.TitleRangeEntity
        });
    }

    static hideAndClearDropDownAddRangeEntityValue() {
        dropDownAddRangeEntityValue.hide();        
        $("#StatusRangeEntityValue").val(ParametrizationStatus.Create);       
        $("#IndexRangeEntityValue").val("");
        $("#RangeValueCode").val("");
        $("#FromValue").val("");
        $("#ToValue").val("");
        ClearValidation("#formRangeEntityValue");
    }

    static editRangeEntity(event, data, index) {
        $("#Index").val(index);
        $("#RangeEntityCode").val(data.RangeEntityCode);
        $("#RangeValueAt").val(data.RangeValueAt);
        $("#Description").val(data.Description);
        if (parseInt(data.RangeEntityCode) >= 0) {
            $("#Status").val(ParametrizationStatus.Update);
        }
        var listEntityValueWithoutDelete = data.RangeEntityValue.filter(item => { return item.StatusTypeService  !== ParametrizationStatus.Delete });
        var listEntityValueWithDelete = data.RangeEntityValue.filter(item => { return item.StatusTypeService  === ParametrizationStatus.Delete });
        if (listEntityValueWithDelete != null) {
            glbRangeEntityValueDelete = listEntityValueWithDelete;
        }
        ConceptRangeEntity.setRangeEntityValue(listEntityValueWithoutDelete);
    }

    static editRangeEntityValue(event, data, index) {
        $("#IndexRangeEntityValue").val(index);        
        $("#RangeValueCode").val(data.RangeValueCode);
        $("#FromValue").val(data.FromValue);
        $("#ToValue").val(data.ToValue);
        if (parseInt(data.RangeValueCode) >= 0) {
            $("#StatusRangeEntityValue").val(ParametrizationStatus.Update);
        }
        ClearValidation("#formRangeEntityValue");
        dropDownAddRangeEntityValue.show();
    }

    static showdropDownAddRangeEntityValue() {
        ConceptRangeEntity.hideAndClearDropDownAddRangeEntityValue();
        ClearValidation("#formRangeEntityValue");
        dropDownAddRangeEntityValue.show();
    }
    
    static addRangeEntity() {
        var formRangeEntity = $("#formRangeEntity").serializeObject();
        formRangeEntity.RangeEntityValue = $("#lsvRangeEntityValue").UifListView("getData");
        if (ConceptRangeEntity.validateAddRangeEntity(formRangeEntity)) {
            if (glbRangeEntityValueDelete.length > 0) //Si existen registros hijos eliminados se concatenan
            {
                formRangeEntity.RangeEntityValue = formRangeEntity.RangeEntityValue.concat(glbRangeEntityValueDelete);
            }
            if (parseInt(formRangeEntity.Index) >= 0) //Si el item es de ediccion
            {
                $("#lsvRangeEntity").UifListView("editItem", parseInt(formRangeEntity.Index), formRangeEntity);
            }
            else {
                $("#lsvRangeEntity").UifListView("addItem", formRangeEntity);
            }
            ConceptRangeEntity.clearRangeEntity();
        }
    }

    static addRangeEntityValue() {
        var formRangeEntityValue = $("#formRangeEntityValue").serializeObject();
        if (ConceptRangeEntity.validateAddRangeEntityValue(formRangeEntityValue)) {
            if (parseInt(formRangeEntityValue.Index) >= 0) //Si el item es de ediccion
            {
                $("#lsvRangeEntityValue").UifListView("editItem", formRangeEntityValue.Index, formRangeEntityValue);
            }
            else {
                $("#lsvRangeEntityValue").UifListView("addItem", formRangeEntityValue);
            }
            ConceptRangeEntity.hideAndClearDropDownAddRangeEntityValue();
        }
    }

    static validateAddRangeEntity(formRangeEntity) {
        var band = true;
        if (!($("#formRangeEntity").valid())) {
            band = false;
        }
        else if (formRangeEntity.RangeEntityValue.length <= 0) {
            band = false;
            $.UifNotify('show', { 'type': 'info', 'message': "Debe agregar por lo menos una lista de valores", 'autoclose': true });
        }
        else {
            const findDescription = function (item, index, array) { //Se valida si existe una lista de valores con la descripcion a agregar
                if (parseInt(formRangeEntity.RangeEntityCode) > 0) {
                    return item.Description.toUpperCase() === formRangeEntity.Description.toUpperCase()
                        && parseInt(item.RangeEntityCode) != parseInt(formRangeEntity.RangeEntityCode)
                }
                else {
                    return item.Description.toUpperCase() === formRangeEntity.Description.toUpperCase()
                        && parseInt(index) != parseInt(formRangeEntity.Index)
                }
            }
            if ($("#lsvRangeEntity").UifListView("findIndex", findDescription) >= 0) //Si existe una lista de valores con la misma descripcion
            {
                band = false;
                $.UifNotify('show', { 'type': 'info', 'message': "Ya existe una lista de valores con esta descripción", 'autoclose': true });
            }
        }
        return band;
    }

    static validateAddRangeEntityValue(formRangeEntityValue) {
        var band = true;
        if (!($("#formRangeEntityValue").valid())) {
            band = false;
        }
        else {
            const findDescription = function (item, index, array) { //Se valida si existe una lista de valores con la descripcion a agregar
                if (parseInt(formRangeEntityValue.RangeValueCode) > 0) {
                    return parseInt(item.FromValue) === parseInt(formRangeEntityValue.FromValue)
                        && parseInt(item.ToValue) === parseInt(formRangeEntityValue.ToValue)
                        && parseInt(item.RangeValueCode) != parseInt(formRangeEntityValue.RangeValueCode)
                }
                else {
                    return parseInt(item.FromValue) === parseInt(formRangeEntityValue.FromValue)
                        && parseInt(item.ToValue) === parseInt(formRangeEntityValue.ToValue)
                        && parseInt(index) != parseInt(formRangeEntityValue.Index)
                }
            }
            if ($("#lsvRangeEntityValue").UifListView("findIndex", findDescription) >= 0) //Si existe una lista de valores con la misma descripcion
            {
                band = false;
                $.UifNotify('show', { 'type': 'info', 'message': "Ya existe un rango de valores con estos valores", 'autoclose': true });
            }
        }
        return band;
    }

    static deleteRangeEntity(deferred, data) {//Elimina un elemento RangeEntity en la lista
        deferred.resolve();
        if (parseInt(data.RangeEntityCode) > 0)//En caso de q' el item venga de DB, el estado del item se actualiza a estado "eliminar"
        {
            data.StatusTypeService  = ParametrizationStatus.Delete;
            data.allowEdit = false;
            data.allowDelete = false;
            $("#lsvRangeEntity").UifListView("addItem", data);
        }
       
        ConceptRangeEntity.clearRangeEntity();
    }

    static deleteRangeEntityValue(deferred, data) {//Elimina un elemento RangeEntityValue en la lista
        deferred.resolve();
        if (parseInt(data.RangeEntityCode) > 0)//En caso de q' el item venga de DB, el estado del item se actualiza a estado "eliminar"
        {
            data.StatusTypeService  = ParametrizationStatus.Delete;
            //glbRangeEntityValueDelete.push(data);
            data.allowEdit = false;
            data.allowDelete = false;
            $("#lsvRangeEntityValue").UifListView("addItem", data);
        }
        
    }

    static SaveRangeEntity() {
        var formRangeEntity = $("#lsvRangeEntity").UifListView("getData");
        formRangeEntity = formRangeEntity.concat(glbRangeEntityDelete);
        formRangeEntity = formRangeEntity.filter(item => { return item.StatusTypeService !== 1 });

        if (formRangeEntity.length > 0) {
            request("RulesScripts/Concepts/SaveRangeEntity", JSON.stringify({ rangeEntities: formRangeEntity }), "Post", Resources.Language.ErrMsgRangeEntityNotFound, (result) => {

                let message = Resources.Language.Create;
                message += `</br> ${Resources.Language.Aggregates}: ${result.TotalAdded}`;
                message += `</br> ${Resources.Language.Updated}: ${result.TotalModified}`;
                message += `</br> ${Resources.Language.Removed}: ${result.TotalDeleted}`;
                message += `</br> ${Resources.Language.Errors}: ${result.Message}`;
                $.UifNotify("show", { 'type': "info", 'message': message, 'autoclose': true });

                request('RulesScripts/Concepts/GetRangeEntity', null, 'GET', Resources.Language.ErrMsgRangeEntityNotFound, ConceptRangeEntity.setRangeEntity);
                ConceptRangeEntity.clearRangeEntity();
                glbRangeEntityDelete = [];
            });
        }
    }

    exit() {
        window.location = rootPath + "Home/Index";
    }
}