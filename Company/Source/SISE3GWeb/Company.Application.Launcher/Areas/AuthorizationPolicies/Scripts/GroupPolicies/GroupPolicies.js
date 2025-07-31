var GroupPoliciesIndex;
var GroupPoliciesId;
var glbGroupPoliciesDeleted = [];

class GroupPolicies extends Uif2.Page {
    getInitialState() {
        // Definir List View
        $('#lsvGroupPolicies').UifListView({
            displayTemplate: "#display-template-GroupPolicies",
            source: null,
            selecttionType: 'single',
            height: 400,
            edit: true,
            delete: true,
            deleteCallback: this.GroupPoliciesDelete,
            customEdit: true,
        });
        //Carga datos en el listView
        PoliciesGroup.GetGroupPolicies().done(function (data) {
            GroupPolicies.LoadListViewGroupPolicies(data);
        });
        // carga datos en textBox Module
        GroupPolicies.LoadTextBoxModule();
        // carga datos en textBox Package
        GroupPolicies.LoadTextBoxPackage();
        // carga datos en textBox Prefix
        GroupPolicies.LoadTextBoxPrefix();
    }

    bindEvents() {
        $("#btnExitGroupPolicies").on('click', GroupPolicies.Exit);
        $('#btnSaveGroupPolicies').on('click', GroupPolicies.OperationGroupPolicies);
        $('#btnGroupPoliciesNew').on('click', GroupPolicies.Clear);
        $('#btnGroupPoliciesAccept').on('click', this.btnGroupPoliciesAccept);
        $('#lsvGroupPolicies').on('rowEdit', GroupPolicies.GroupPoliciesShowData);
        $('#lsvGroupPolicies').on('rowDelete', this.GroupPoliciesDelete);
        $("#IdModule").on("itemSelected", GroupPolicies.LoadSubModuleByIdModule);
        $("#IdPrefix").on("itemSelected", GroupPolicies.LoadRiskByIdPrefix);
    }

    static OperationGroupPolicies() {
        var listGroupPolicies = $("#lsvGroupPolicies").UifListView('getData');
        var listGroupPolicies = listGroupPolicies.filter(function (item) {
            return item.StatusTypeService > 1;
        });
        if (listGroupPolicies.length > 0) {
            request('AuthorizationPolicies/GroupPolicies/ExecuteOperationGroupPolicies',
                JSON.stringify({ groupPolicies: listGroupPolicies }),
                'POST',
                AppResources.UnexpectedError, GroupPolicies.confirmCreateGroupPolicies);
        }
        else {
            if (glbGroupPoliciesDeleted == null && glbGroupPoliciesDeleted.length == 0) {
                $.UifNotify('show', {
                    'type': 'danger', 'message': Resources.Language.ExpenseNotDataOperation, 'autoclose': true
                });
            } else {
                request('AuthorizationPolicies/GroupPolicies/ExecuteOperationGroupPolicies',
                    JSON.stringify({ groupPolicies: glbGroupPoliciesDeleted }),
                    'POST',
                    AppResources.UnexpectedError, GroupPolicies.confirmCreateGroupPolicies);
            }

        }
    }

    //  Funcion carga datos a ListView
    static LoadListViewGroupPolicies(data) {
        if (data.success) {
            //REQ_#613
            //reas: Se hace comentario la línea 77 a 80 teniendo en cuenta que el Id 
            //que va a quedar registrado en la base de datos no se debe mostrar al usuario en el formulario, 
            //únicamente cuando se consulte un registro.
            //author: Diego A. Leon
            //date: 24/08/2018
            //valida el Id siguiente 
            //let Indexvalid = (data.result.length) - 1;
            //let IdNext = Math.max(data.result[Indexvalid].IdGroupPolicies) + 1;
            //$("#IdGroupPolicies").val(IdNext);
            //$("#IdGroupPolicies").text(IdNext);

            // Limpiar ListView
            $("#lsvGroupPolicies").UifListView("clear");

            // Llenar objeto con los datos obtenidos
            $.each(data.result, function (key, value) {
                // Agregar objeto al ListView                
                $("#lsvGroupPolicies").UifListView("addItem", value);
            });
        }
    }

    static LoadTextBoxPrefix() {
        EventModule.GetPrefixes().done(function (data) {
            if (data) {
                $('#IdPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadTextBoxModule() {
        EventModule.GetModules().done(function (data) {
            if (data) {
                $('#IdModule').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadTextBoxPackage() {
        PoliciesGroup.GetPackageEnabled().done(function (data) {
            if (data.success) {
                $('#IdPackage').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GroupPoliciesShowData(event, result, index) {
        GroupPolicies.Clear();
        if (result) {
            GroupPoliciesIndex = index;
            GroupPoliciesId = result.IdGroupPolicies;
            $("#IdGroupPolicies").val(result.IdGroupPolicies);
            $("#DescriptionGroupPolicies").prop('disabled', 'disabled');
            $("#DescriptionGroupPolicies").val(result.Description);
            $("#IdModule").UifSelect('setSelected', result.Module.Id);
            EventModule.GetSubModule(result.Module.Id).done(function (data) {
                if (data) {
                    $('#IdSubModule').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
            $("#IdSubModule").UifSelect('setSelected', result.SubModule.Id);
            $("#IdPackage").UifSelect('setSelected', result.Package.PackageId);

            var prefix = result.Key.split(",", 2);
            $("#IdPrefix").UifSelect('setSelected', prefix[0]);
            EventModule.GetRiskByPrefix(prefix[0]).done(function (data) {
                if (data) {
                    //REQ_483
                    //reas: Corrección validación data
                    //author: Germán F. Grimaldi
                    //date: 08/08/2018
                    //Inicio
                    //if (data.result.length > 1) {
                    //    $('#IdRisk').UifSelect({ sourceData: data.result });
                    //}
                    //else {
                    //    $('#IdRisk').UifSelect({ sourceData: data.result });
                    //    $('#IdRisk').val(data.result[0].Id);
                    //}
                    if (data.result.length != 1) {
                        $('#IdRisk').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $('#IdRisk').UifSelect({ sourceData: data.result });
                        $('#IdRisk').val(data.result[0].Id);
                    }
                    //Fin
                }
            });
            $("#IdRisk").UifSelect('setSelected', prefix[1]);
        }
    }

    GroupPoliciesDelete(event, result) {
        //REQ_#613
        //reas: Modificación método para persistir y almacenar los registros que 
        //serán eliminados y hacerlos visibles en plataforma.
        //author: Diego A. Leon
        //date: 24/08/2018
        event.resolve();
        if (result) {
            if (result != null && result.IdGroupPolicies != null) {
                glbGroupPoliciesDeleted.push(result);
                result.StatusTypeService = ParametrizationStatus.Delete;
                result.allowEdit = false;
                result.allowDelete = false;
                $("#lsvGroupPolicies").UifListView("addItem", result);
            }
            GroupPolicies.Clear();
        }
    }

    static Clear() {
        //Limpia las validaciones del formulario
        ClearValidation("#GroupPoliciesForm");
        $("#IdGroupPolicies").val("");
        $("#DescriptionGroupPolicies").prop('disabled', false);
        $("#DescriptionGroupPolicies").val("");
        $("#IdModule").UifSelect('setSelected', null);
        $("#IdSubModule").UifSelect('setSelected', null);
        $("#IdPackage").UifSelect('setSelected', null);
        $("#IdPrefix").UifSelect('setSelected', null);
        $("#IdRisk").UifSelect('setSelected', null);
        $("#IdRisk").prop('disabled', 'disabled');
        GroupPoliciesIndex = null;
        GroupPoliciesId = null;
        glbGroupPoliciesDeleted = [];
    }

    btnGroupPoliciesAccept() {
        $("#GroupPoliciesForm").validate();
        var valid = $("#GroupPoliciesForm").valid();
        if (valid) {
            var group = GroupPolicies.GetForm();
            if (group.IdGroupPolicies == null) {
                group.StatusTypeService = ParametrizationStatus.Create;
                $("#lsvGroupPolicies").UifListView("addItem", group);
            }
            else {
                group.StatusTypeService = ParametrizationStatus.Update;
                $("#lsvGroupPolicies").UifListView("editItem", GroupPoliciesIndex, group);
            }
            GroupPolicies.Clear();
        }

    }

    static LoadSubModuleByIdModule(event, selectedItem) {
        if (selectedItem.Id == "") {
            $("#IdSubModule").prop('disabled', 'disabled');
        }
        else {
            var IdModule = selectedItem.Id;
            if (IdModule > 0) {
                EventModule.GetSubModule(IdModule).done(function (data) {
                    if (data.result) {
                        $('#IdSubModule').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $("#IdSubmodule").UifSelect();
            }
        }
    }

    static LoadRiskByIdPrefix(event, selectedItem) {
        if (selectedItem.Id == "") {
            $("#IdRisk").prop('disabled', 'disabled');
        }
        else {
            var IdRisk = selectedItem.Id;
            if (IdRisk > 0) {
                EventModule.GetRiskByPrefix(IdRisk).done(function (data) {
                    if (data.result) {
                        //REQ_483
                        //reas: Corrección validación data
                        //author: Germán F. Grimaldi
                        //date: 08/08/2018
                        //Inicio
                        //if (data.result.length > 1) {
                        //    $('#IdRisk').UifSelect({ sourceData: data.result });
                        //}
                        //else
                        //{
                        //    $('#IdRisk').UifSelect({ sourceData: data.result });
                        //    $('#IdRisk').val(data.result[0].Id);
                        //}
                        //dev
                        if (data.result.length != 1) {
                            $('#IdRisk').UifSelect({ sourceData: data.result });
                        }
                        else {
                            $('#IdRisk').UifSelect({ sourceData: data.result });
                            $('#IdRisk').val(data.result[0].Id);
                        }
                        //Fin
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $("#IdSubmodule").UifSelect();
            }
        }
    }

    static GetForm() {
        var data = {};
        if (GroupPoliciesId != 0) {
            data.IdGroupPolicies = GroupPoliciesId;
        }
        data.Description = $("#DescriptionGroupPolicies").val();
        data.Module = { Id: $("#IdModule").val(), Description: $("#IdModule").UifSelect("getSelectedText") };
        data.SubModule = { Id: $("#IdSubModule").val(), Description: $("#IdSubModule").UifSelect("getSelectedText") };
        data.Package = { PackageId: $("#IdPackage").val(), Description: $("#IdPackage").UifSelect("getSelectedText") };
        data.Prefix = { Id: $("#IdPrefix").val(), Description: $("#IdPrefix").UifSelect("getSelectedText") };
        data.Risk = { Id: $("#IdRisk").val(), Description: $("#IdRisk").UifSelect("getSelectedText") };
        //REQ_483
        //reas: Corrección definición atributo.
        //author: Germán F. Grimaldi
        //date: 08/08/2018
        //Inicio
        //data.key = $("#IdPrefix").val() + "," + $("#IdRisk").val();
        data.Key = $("#IdPrefix").val() + "," + $("#IdRisk").val();
        //Fin
        data.StatusTypeService = ParametrizationStatus.Origin;
        return data;
    }

    static confirmCreateGroupPolicies(data) {
        //REQ_#613
        //reas: Modificación método de mensajes mostrados al terminar una operación 
        //y evitar que tome siempre como predeterminado MessageUpdate
        //author: Diego A. Leon
        //date: 24/08/2018
        if (data[0].StatusTypeService == 5) {
            $.UifNotify('show', {
                'type': 'danger', 'message': 'El grupo esta siendo usado',
                'autoclose': true
            });
        } else {
            switch (data[0].StatusTypeService) {
                case 2: //Insert
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResources.MessageCreate,
                        'autoclose': true
                    });
                    break;
                case 3: //Update
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResources.MessageUpdate,
                        'autoclose': true
                    });
                    break;
                case 4: //Delete
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResources.MessageDelete,
                        'autoclose': true
                    });
                    break;
                default: //Original
                    $.UifNotify('show', {
                        'type': 'info', 'message': AppResources.MessageUpdate,
                        'autoclose': true
                    });
                    break;
            }
        }
        PoliciesGroup.GetGroupPolicies().done(function (data) {
            GroupPolicies.LoadListViewGroupPolicies(data);
        });
        GroupPolicies.Clear();
    }

    //REQ_#613
    //reas: Funcionalidad botón salir
    //author: Diego A. Leon
    //date: 24/08/2018
    static Exit() {
        window.location = rootPath + "Home/Index";
    }
}



class PoliciesGroup {
    static GetGroupPolicies() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'AuthorizationPolicies/GroupPolicies/GetGroupPolicies',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetPackageEnabled() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'AuthorizationPolicies/GroupPolicies/GetPackage',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static CreatePoliciesGroup(group) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'CreatePoliciesGroup',
            data: JSON.stringify(group),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static UpdatePoliciesGroup(group) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UpdatePoliciesGroup',
            data: JSON.stringify(group),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}
class EventModule {
    static GetModules() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'AuthorizationPolicies/GroupPolicies/GetModules',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetSubModule(IdModule) {
        return $.ajax({
            type: 'POST',
            async: false,
            url: rootPath + 'AuthorizationPolicies/GroupPolicies/GetSubModules',
            data: JSON.stringify({ module: IdModule, }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'AuthorizationPolicies/GroupPolicies/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetRiskByPrefix(IdRisk) {
        return $.ajax({
            type: 'POST',
            async: false,
            url: rootPath + 'AuthorizationPolicies/GroupPolicies/GetCoveredRiskType',
            data: JSON.stringify({ prefix: IdRisk, }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static SearchAdvancedGroupPolicies(description, module, subModule, prefix) {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + 'AuthorizationPolicies/GroupPolicies/GetGroupPoliciesByDescription',
            data: JSON.stringify({ description: description, module: module, subModule: subModule, prefix: prefix }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
