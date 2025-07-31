//Clase formulario principal
var glbDelegationEdit;
var glbListDelegationDelete = [];
var status;
class DelegationParametrization extends Uif2.Page {
    getInitialState() {
        DelegationParametrization.GetModule();
        DelegationParametrization.GetHierarchy();

        $("#listDelegation").UifListView({
            displayTemplate: "#templateDelegation",
            source: null,
            selecttionType: 'single',
            height: 400
        });
        request('ParamAuthorizationPolicies/Delegation/GetParametrizationDelegation', null, 'GET', AppResources.ErrorSearchClauses, DelegationParametrization.getDelegation);
    }
    bindEvents() {
        $('#listDelegation').on('rowEdit', DelegationParametrization.editDelegation);
        $("#btnDelegationNew").click(DelegationParametrization.clearFormDelegation);
        $('#btnDelegationAccept').click(this.addDelegation);
        $('#inputDelegationSearch').on("search", this.SearchDelegation);
        $('#tblResultListDelegationTotal').on('rowSelected', this.SelectSearchDelegationByName);
        $("#btnSaveDelegation").click(DelegationParametrization.createDelegation);
        $("#btnExitDelegation").click(this.redirectIndex);
        $("#btnExportDelegation").click(this.exportExcel);
        $("#IdModule").on("itemSelected", DelegationParametrization.SearchSubModule);

    }

    static GetModule() {
        DelegationParametrizationRequest.GetModules().done(response => {
            let result = response.result;

            if (response.success) {
                $("#IdModule").UifSelect({ sourceData: result });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static SearchSubModule(e, financialPlan) {
        if ($.trim($("#IdModule").val()) != "") {
            var idModule = $("#IdModule").val();
            DelegationParametrizationRequest.GetSubModuleItems(idModule).done(response => {
                let result = response.result;
                if (response.success) {
                    $("#IdSubModule").UifSelect({ sourceData: result });
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
                }
            });
        }

    }

    static GetHierarchy() {
        DelegationParametrizationRequest.GetHierarchies().done(response => {
            let result = response.result;
            if (response.success) {
                $("#IdHierarchy").UifSelect({ sourceData: result });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static getDelegation(data) {
        $("#listDelegation").UifListView({
            displayTemplate: "#templateDelegation",
            sourceData: data,
            selectionType: 'single',
            height: 400,
            edit: true,
            customEdit: true

        });
    }

    static editDelegation(event, result, index) {
        DelegationParametrization.clearFormDelegation();
        glbDelegationEdit = result;
        glbDelegationEdit.Index = index;
        var object = [];

        if (result.Module) {
            $("#IdModule").UifSelect('setSelected', result.Module);
            $("#IdModule").UifSelect("disabled", true);
            $("#IdSubModule").UifSelect("disabled", true);
            $("#IdHierarchy").UifSelect("disabled", true);
            $("#IdSubModule").UifSelect("setSelected", result.SubModule);
            $("#IdHierarchy").UifSelect("setSelected", result.Hierarchy);
            $("#Description").val(result.Description);
        }
        else {
            object.push(result.SubModuleServicesQueryModel);
            $("#IdSubModule").UifSelect({ sourceData: object });
            $("#IdModule").UifSelect('setSelected', result.ModuleServiceQueryModel.Id);
            $("#IdSubModule").UifSelect("setSelected", result.SubModuleServicesQueryModel.Id);
            $("#IdHierarchy").UifSelect("setSelected", result.HierarchyServiceQueryModel.Id);
            $("#IdModule").UifSelect("disabled", true);
            $("#IdSubModule").UifSelect("disabled", true);
            $("#IdHierarchy").UifSelect("disabled", true);
            $("#Description").val(result.Description);
            status = ParametrizationStatus.Update;
        }

        if (result.IsExclusionary) {
            $("#IsExclusionary").prop('checked', true);
        }
        else {
            $("#IsExclusionary").prop('checked', false);
        }

        if (result.IsEnabled) {
            $("#IsEnabled").prop('checked', true);

        }
        else {
            $("#IsEnabled").prop('checked', false);

        }

    }

    static clearFormDelegation() {
        ClearValidation("#formDelegation");
        glbDelegationEdit = null;
        status = ParametrizationStatus.Create;
        $("#IdModule").val("");
        $("#IdSubModule").val("");
        $("#IdHierarchy").val("");
        $("#Description").val("");
        $('#IsExclusionary').prop('checked', false);
        $('#IsEnabled').prop('checked', false);
        $("#IdModule").UifSelect("disabled", false);
        $("#IdSubModule").UifSelect("disabled", false);
        $("#IdHierarchy").UifSelect("disabled", false);
        $("#IdModule").UifSelect("setSelected", null);
        $("#IdSubModule").UifSelect("setSelected", null);
        $("#IdSubModule").UifSelect({ sourceData: null });
        $("#IdHierarchy").UifSelect("setSelected", null);        
        $("#inputDelegationSearch").val("");
    }

    static GetForm() {
        var data = {};

        data.Description = $("#Description").val();
        data.ModuleText = $("#IdModule").UifSelect("getSelectedText");
        data.Module = $("#IdModule").UifSelect("getSelected");
        data.SubModuleText = $("#IdSubModule").UifSelect("getSelectedText");
        data.SubModule = $("#IdSubModule").UifSelect("getSelected");
        data.HierarchyText = $("#IdHierarchy").UifSelect("getSelectedText");
        data.Hierarchy = $("#IdHierarchy").UifSelect("getSelected");
        return data;
    }

    addDelegation() {
        var formDelegation = DelegationParametrization.GetForm();
        if ($('#IsExclusionary').is(':checked')) {
            formDelegation.IsExclusionary = true;
        }
        if ($('#IsEnabled').is(':checked')) {
            formDelegation.IsEnabled = true;
        }
        if (DelegationParametrization.validateAddDelegation(formDelegation)) {

            if (status == ParametrizationStatus.Update) {

                DelegationParametrization.UpdateDelegationParametrization(formDelegation);
            }
            else {
                DelegationParametrization.InsertDelegationParametrization(formDelegation);
            }

            DelegationParametrization.clearFormDelegation();
        }
    }

    static UpdateDelegationParametrization(formDelegation) {
        formDelegation.StatusTypeService = ParametrizationStatus.Update;
        $("#listDelegation").UifListView("editItem", glbDelegationEdit.Index, formDelegation);
    }

    static InsertDelegationParametrization(formDelegation) {
        formDelegation.StatusTypeService = ParametrizationStatus.Create;
        if (glbDelegationEdit != null) {
            $("#listDelegation").UifListView("editItem", glbDelegationEdit.Index, formDelegation);
        }
        else {
            if (DelegationParametrization.validateWhetherOtherDelegationIsEqual(formDelegation)) {
                $("#listDelegation").UifListView("addItem", formDelegation);
            }            
        }
    }

    static validateAddDelegation(formDelegation) {
        var band = false;
        if ($("#formDelegation").valid()) {
            band = true;
        }
        return band;
    }

    static validateWhetherOtherDelegationIsEqual(formDelegation) {
        var isValid = true;
        var delegations = $("#listDelegation").UifListView('getData');
        $.each(delegations, function (index, value) {
            if (value.ModuleServiceQueryModel.Id == formDelegation.Module && value.SubModuleServicesQueryModel.Id == formDelegation.SubModule && value.HierarchyServiceQueryModel.Id == formDelegation.Hierarchy) {
                $.UifNotify("show", { 'type': "danger", 'message': 'No se puede agregar delegación</br>Ya existe una con la siguiente clasificación: </br>módulo: <i><b>' + formDelegation.ModuleText + '</b></i></br>submodulo: <i><b>' + formDelegation.SubModuleText + '</b></i></br>jerarquia: <i><b>' + formDelegation.HierarchyText + '</b></i>', 'autoclose': true });
                isValid = false;
            }            
        });    
        return isValid;
    }

    SearchDelegation(event, value) {
        if ($.trim($("#inputDelegationSearch").val()) != "") {
            DelegationParametrization.GetDelegation($("#inputDelegationSearch").val());
        }
    }

    static GetDelegation(description) {
        DelegationParametrizationRequest.GetDelegationByName(description).done(function (data) {
            if (description.length >= 3) {
                if (data.success) {
                    if (data.result.length > 0) {
                        if (data.result.length === 1) {

                            const findId = function (element, index, array) {
                                return element.Id === data.result[0].Id
                            }
                            var indexDelegation = $("#listDelegation").UifListView("findIndex", findId);
                            DelegationParametrization.editDelegation(null, data.result[0], indexDelegation);
                            $("#Level").val(data.result[0].ConditionLevelServiceQueryModel.Id);

                        }
                        else {
                            DelegationParametrization.ShowListDelegtion(data.result);
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchTexts, 'autoclose': true });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.MessageInfoMinimumChar, 'autoclose': true });
            }
        });
    }

    static ShowListDelegtion(dataTable) {
        $('#tblResultListDelegationTotal').UifDataTable('clear');
        $('#tblResultListDelegationTotal').UifDataTable('addRow', dataTable);
        $('#modalListSearchDelegation').UifModal('showLocal', AppResources.ListDelegation);
    }

    SelectSearchDelegationByName(event, dataListDelegation, position) {
        const findId = function (element, index, array) {
            return element.Id === dataListDelegation.Id
        }
        var indexDelegation = $("#listDelegation").UifListView("findIndex", findId);
        DelegationParametrization.editDelegation(event, dataListDelegation, indexDelegation);

        $('#modalListSearchDelegation').UifModal('hide');
    }


    static createDelegation() {
        var itemModified = [];
        var delegations = $("#listDelegation").UifListView('getData');
        $.each(glbListDelegationDelete, function (index, item) {
            item.Status = ParametrizationStatus.Delete;
            itemModified.push(item);
        })


        $.each(delegations, function (index, value) {
            if (value.StatusTypeService == 0 || value.StatusTypeService == undefined) {
                value.Status = ParametrizationStatus.Original
            }
            itemModified.push(value);
        });

        var delegationFilter = itemModified.filter(function (value) {
            return value.StatusTypeService > 1;
        });

        if (delegationFilter.length > 0) {
            request('ParamAuthorizationPolicies/Delegation/CreateParametrizationDelegation', JSON.stringify({ parametrizationDelegationVM: delegationFilter }), 'POST', AppResources.ErrorSaveDelegation, DelegationParametrization.confirmCreateParametrizationDelegation);
        }
    }
    static confirmCreateParametrizationDelegation(data) {
        glbListDelegationDelete = [];
        DelegationParametrization.clearFormDelegation();
        request('ParamAuthorizationPolicies/Delegation/GetParametrizationDelegation', null, 'GET', AppResources.ErrorGetDelegation, DelegationParametrization.getDelegation)
        if (data.Message === null) {
            data.Message = 0;
        }
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
            "Agregados" + ':' + data.TotalAdded + '<br> ' +
            "Actualizados" + ':' + data.TotalModified + '<br> ' +
            "Errores" + ':' + data.Message,
            'autoclose': true
        });
    }

    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    exportExcel() {
        request('ParamAuthorizationPolicies/Delegation/GenerateFileToExport', null, 'GET', "Error descargando archivo", DelegationParametrization.generateFileToExport);
    }
    static generateFileToExport(data) {
        DownloadFile(data);
    }
}