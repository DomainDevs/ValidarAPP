var glbListEntityDelete = [];
var glbListEntityValueDelete = [];
var dropDownAddListEntityValue;
var glbPositionEditListEntityValue;
class ConceptListEntity extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: true });
        request("RulesScripts/Concepts/GetListEntity", null, "GET", Resources.Language.ErrMsgListEntityNotFound, ConceptListEntity.setListEntity);
        $("#Status").val(ParametrizationStatus.Create);
        ConceptListEntity.setListEntityValue(null);
        this.setDropDow();
    }
    bindEvents() {
        $("#btnCancelListEntity").click(ConceptListEntity.clearListEntity);
        $("#lsvListEntity").on("rowEdit", ConceptListEntity.editListEntity);
        $("#lsvListEntityValue").on("rowEdit", ConceptListEntity.editListEntityValue);
        $("#lsvListEntityValue").on("rowAdd", ConceptListEntity.showdropDownAddListEntityValue);
        $("#btnAddListEntity").on("click", ConceptListEntity.addListEntity);
        $("#btnExitListEntity").on("click", this.exit);
        $("#btnSaveListEntity").on("click", ConceptListEntity.SaveListEntity);
        $("#inputSearchListEntity").on("search", ConceptListEntityAdvanced.searchListEntity);
        $("#btnSearchAdvRange").on("click", ConceptListEntityAdvanced.showdropDownSearchAdvList);
    }
    static clearListEntity() {
        $("#ListEntityCode").val("");
        $("#ListValueAt").val("");
        $("#Description").val("");
        $("#Description").focus();
        $("#Status").val(ParametrizationStatus.Create);
        $("#Index").val("");
        ConceptListEntity.hideAndClearDropDownAddListEntityValue();
        ConceptListEntity.setListEntityValue(null);
        glbListEntityValueDelete = [];
        ClearValidation("#formListEntity");
    }
    setDropDow() {
        dropDownAddListEntityValue = uif2.dropDown({
            source: rootPath + "RulesScripts/Concepts/ListEntityValueAdd",
            element: "#lsvListEntityValue .add-button",
            align: "right",
            width: 400,
            height: 150,
            loadedCallback: ConceptListEntity.componentLoadedCallback
        });
    }
    static componentLoadedCallback() {
        $("#StatusListEntityValue").val(ParametrizationStatus.Create);
        $("#btnAddListEntityValue").on("click", ConceptListEntity.addListEntityValue);
        $("#btnCancelListEntityValue").on("click", ConceptListEntity.hideAndClearDropDownAddListEntityValue);
    }

    static setListEntity(data) {
        $("#lsvListEntity").UifListView({
            sourceData: data,
            displayTemplate: "#listEntityTemplate",
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: ConceptListEntity.deleteListEntity,
            height: 400
        });
    }

    static setListEntityValue(data) {
        $("#lsvListEntityValue").UifListView({
            sourceData: data,
            displayTemplate: "#listEntityValueTemplate",
            add: true,
            edit: true,
            delete: true,
            customAdd: true,
            customEdit: true,
            deleteCallback: ConceptListEntity.deleteListEntityValue,
            height: 250
        });
    }

    static hideAndClearDropDownAddListEntityValue() {
        dropDownAddListEntityValue.hide();
        $("#inputListValue").val("");
        $("#IndexListEntityValue").val("");
        $("#StatusListEntityValue").val(ParametrizationStatus.Create);
        $("#ListValueCode").val("");
        ClearValidation("#formListEntityValue");
    }

    static editListEntity(event, data, index) {
        $("#Index").val(index);
        $("#ListEntityCode").val(data.ListEntityCode);
        $("#ListValueAt").val(data.ListValueAt);
        $("#Description").val(data.Description);
        if (parseInt(data.ListEntityCode) >= 0) {
            $("#Status").val(ParametrizationStatus.Update);
        }
        var listEntityValueWithoutDelete = data.ListEntityValue.filter(item => { return item.StatusTypeService !== ParametrizationStatus.Delete });
        var listEntityValueWithDelete = data.ListEntityValue.filter(item => { return item.StatusTypeService === ParametrizationStatus.Delete });
        if (listEntityValueWithDelete != null) {
            glbListEntityValueDelete = listEntityValueWithDelete;
        }
        ConceptListEntity.setListEntityValue(listEntityValueWithoutDelete);
        $("#bandUpdate").val(true);
    }

    static editListEntityValue(event, data, index) {
        $("#IndexListEntityValue").val(index);
        $("#inputListValue").val(data.ListValue);
        $("#ListValueCode").val(data.ListValueCode);
        if (parseInt(data.ListValueCode) >= 0) {
            $("#StatusListEntityValue").val(ParametrizationStatus.Update);
        }
        ClearValidation("#formListEntityValue");
        dropDownAddListEntityValue.show();
    }

    static showdropDownAddListEntityValue() {
        ConceptListEntity.hideAndClearDropDownAddListEntityValue();
        ClearValidation("#formListEntityValue");
        dropDownAddListEntityValue.show();
    }

    static addListEntity() {
        var formListEntity = $("#formListEntity").serializeObject();
        formListEntity.ListEntityValue = $("#lsvListEntityValue").UifListView("getData");        
        if (ConceptListEntity.validateAddListEntity(formListEntity)) {
            if (glbListEntityValueDelete.length > 0) //Si existen registros hijos eliminados se concatenan
            {
                formListEntity.ListEntityValue = formListEntity.ListEntityValue.concat(glbListEntityValueDelete);
            }
            if (parseInt(formListEntity.Index) >= 0) //Si el item es de ediccion
            {
                formListEntity.bandUpdate = true;
                $("#lsvListEntity").UifListView("editItem", parseInt(formListEntity.Index), formListEntity);
                
            }
            else {
                formListEntity.bandAdd = true;
                $("#lsvListEntity").UifListView("addItem", formListEntity);
            }
            ConceptListEntity.clearListEntity();
        }
    }

    static addListEntityValue() {
        var formListEntityValue = $("#formListEntityValue").serializeObject();
        if (ConceptListEntity.validateAddListEntityValue(formListEntityValue)) {
            if (parseInt(formListEntityValue.Index) >= 0) //Si el item es de ediccion
            {
                formListEntityValue.bandUpdateEntityValue = true;
                $("#lsvListEntityValue").UifListView("editItem", formListEntityValue.Index, formListEntityValue);
            }
            else {
                formListEntityValue.bandAddEntityValue = true;
                $("#lsvListEntityValue").UifListView("addItem", formListEntityValue);
            }
            ConceptListEntity.hideAndClearDropDownAddListEntityValue();
        }
    }

    static validateAddListEntity(formListEntity) {
        var band = true;
        if (!($("#formListEntity").valid())) {
            band = false;
        }
        else if (formListEntity.ListEntityValue.length <= 0) {
            band = false;
            $.UifNotify("show", { 'type': "info", 'message': "Debe agregar por lo menos una lista de valores", 'autoclose': true });
        }
        else {
            const findDescription = function (item, index, array) { //Se valida si existe una lista de valores con la descripcion a agregar
                if (parseInt(formListEntity.ListEntityCode) > 0) {
                    return item.Description.toUpperCase() === formListEntity.Description.toUpperCase() &&
                        parseInt(item.ListEntityCode) != parseInt(formListEntity.ListEntityCode);
                }
                else {
                    return item.Description.toUpperCase() === formListEntity.Description.toUpperCase() &&
                        parseInt(index) != parseInt(formListEntity.Index);
                }
            }
            if ($("#lsvListEntity").UifListView("findIndex", findDescription) >= 0) //Si existe una lista de valores con la misma descripcion
            {
                band = false;
                $.UifNotify("show", { 'type': "info", 'message': "Ya existe una lista de valores con esta descripción", 'autoclose': true });
            }
        }
        return band;
    }

    static validateAddListEntityValue(formListEntityValue) {
        var band = true;
        if (!($("#formListEntityValue").valid())) {
            band = false;
        }
        else {
            const findDescription = function (item, index, array) { //Se valida si existe una lista de valores con la descripcion a agregar
                if (parseInt(formListEntityValue.ListValueCode) > 0) {
                    return item.ListValue.toUpperCase() === formListEntityValue.ListValue.toUpperCase()
                        && parseInt(item.ListValueCode) != parseInt(formListEntityValue.ListValueCode)
                }
                else {
                    return item.ListValue.toUpperCase() === formListEntityValue.ListValue.toUpperCase()
                        && parseInt(index) != parseInt(formListEntityValue.Index)
                }
            }
            if ($("#lsvListEntityValue").UifListView("findIndex", findDescription) >= 0) //Si existe una lista de valores con la misma descripcion
            {
                band = false;
                $.UifNotify("show", { 'type': "info", 'message': "Ya existe una lista de valores con esta descripción", 'autoclose': true });
            }
        }
        return band;
    }

    static deleteListEntity(deferred, data) {//Elimina un elemento ListEntity en la lista
        deferred.resolve();
        if (parseInt(data.ListEntityCode) > 0)//En caso de q' el item venga de DB, el estado del item se actualiza a estado "eliminar"
        {
            data.StatusTypeService = ParametrizationStatus.Delete;
            //glbListEntityDelete.push(data);
            data.allowEdit = false;
            data.allowDelete = false;
            $("#lsvListEntity").UifListView("addItem", data);
        }
        ConceptListEntity.clearListEntity();
    }

    static deleteListEntityValue(deferred, data) {//Elimina un elemento ListEntityValue en la lista
        deferred.resolve();
        if (parseInt(data.ListEntityCode) > 0)//En caso de q' el item venga de DB, el estado del item se actualiza a estado "eliminar"
        {
            data.StatusTypeService = ParametrizationStatus.Delete;
            //glbListEntityValueDelete.push(data);
            data.allowEdit = false;
            data.allowDelete = false;
            $("#lsvListEntityValue").UifListView("addItem", data);
        }
        
    }

    static SaveListEntity() {
        var formListEntity = $("#lsvListEntity").UifListView("getData");
        formListEntity = formListEntity.concat(glbListEntityDelete);
        formListEntity = formListEntity.filter(item => { return item.StatusTypeService !== 1 });

        if (formListEntity.length > 0) {
            request("RulesScripts/Concepts/SaveListEntity", JSON.stringify({ listEntities: formListEntity }), "Post", Resources.Language.ErrMsgListEntityNotFound, (result) => {

                let message = Resources.Language.Create;
                message += `</br> ${Resources.Language.Aggregates}: ${result.TotalAdded}`;
                message += `</br> ${Resources.Language.Updated}: ${result.TotalModified}`;
                message += `</br> ${Resources.Language.Removed}: ${result.TotalDeleted}`;
                message += `</br> ${Resources.Language.Errors}: ${result.Message}`;
                $.UifNotify("show", { 'type': "info", 'message': message, 'autoclose': true });

                request("RulesScripts/Concepts/GetListEntity", null, "GET", Resources.Language.ErrMsgListEntityNotFound, ConceptListEntity.setListEntity);
                ConceptListEntity.clearListEntity();
                glbListEntityDelete = [];
            });
        }
    }

    exit() {
        window.location = rootPath + "Home/Index";
    }
}