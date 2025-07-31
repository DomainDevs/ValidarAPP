// Variable global
var dropDownSearchAdvGroupPolicies;

class AdvancedSearchGroupPolicies extends Uif2.Page {

    // inicializa eventos 
    getInitialState() {
        dropDownSearchAdvGroupPolicies = uif2.dropDown({
            source: rootPath + 'AuthorizationPolicies/GroupPolicies/AdvancedSearchGroupPolicies',
            element: '#btnSearchAdvScript',
            align: 'right',
            width: 500,
            height: 590,
            loadedCallback: this.AdvancedSearchEvents
        });

    }
    // Ejecucion de funciones segun evento
    bindEvents() {
        $('#btnSearchAdvScript').click(AdvancedSearchGroupPolicies.ShowAdvancedSearch);

    }
    // Eventos pre cargados
    AdvancedSearchEvents() {
        $("#IdModuleSearchAdvanzed").on("itemSelected", AdvancedSearchGroupPolicies.LoadSubModuleByIdModule);
        $("#btnCancelSearchScriptGroupPolicies").on("click", AdvancedSearchGroupPolicies.CancelSearchAdvanzed);
        $('#btnAdvancedSearchListView').on('click', AdvancedSearchGroupPolicies.GetSearchGroupPolicies);
        $('#inputGroundsGroupPolicies').on('buttonClick', AdvancedSearchGroupPolicies.GetSearchGroupPoliciesbyDescription);
        $("#btnLoadGroupPolicies").on("click", AdvancedSearchGroupPolicies.GetLoadAdvanzeSearch);
        $('#btnSearchAdvScript').click(AdvancedSearchGroupPolicies.ShowAdvancedSearch);
    }

    static GetLoadAdvanzeSearch() {

        var itemSelected = $("#listviewSearch").UifListView("getSelected");
        AdvancedSearchGroupPolicies.Clear();
        dropDownSearchAdvGroupPolicies.hide();
        if (itemSelected != "") {
            var data = [];
            data.result = itemSelected;
            $.each(data.result, function (key, value) {
                var item = data.result[0];
                var lista = $("#lsvGroupPolicies").UifListView('getData')
                var index = lista.findIndex(function (item) {
                    return item.IdGroupPolicies == value.IdGroupPolicies;
                });
                glbIndexGroundsRejection = index;
            }
            );
            AdvancedSearchGroupPolicies.GroupPoliciesShowData(event, data, glbIndexGroundsRejection);
        }
        //else {
        //    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorListElement });
        //}
    }
    // Get 
    static GetSearchGroupPoliciesbyDescription() {
        let Description = $("#inputGroundsGroupPolicies").val();
        let prefix = "";
        if (Description.length > 1) {
            let module = 0;
            let subModule = 0;
            EventModule.SearchAdvancedGroupPolicies(Description, module, subModule, prefix).done(function (data) {
                if (data.result.length > 1) {
                    AdvancedSearchGroupPolicies.ShowAdvancedSearch()
                    AdvancedSearchGroupPolicies.LoadListViewSearchAvanzed(data);
                }
                else {
                    $.each(data.result, function (key, value) {
                        var item = data.result[0];
                        var lista = $("#lsvGroupPolicies").UifListView('getData')
                        var index = lista.findIndex(function (item) {
                            return item.IdGroupPolicies == value.IdGroupPolicies;
                        });
                        glbIndexGroundsRejection = index;
                    });

                    AdvancedSearchGroupPolicies.GroupPoliciesShowData(event, data, glbIndexGroundsRejection);
                }

                AdvancedSearchGroupPolicies.Clear();
            });
        } else {
            $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.ErrorSearchAdvancedMiniumCharacters, 'autoclose': true });
        }
    }

    static GetSearchGroupPolicies() {
        let Description = $("#DescriptionGroupPoliciesSearchAdvanzed").val();

        let module = $("#IdModuleSearchAdvanzed").val();
        let subModule = $("#IdSubModuleSearchAdvanzed").val();
        let prefix = $("#IdPrefixSearchAdvanzed").val();
        if (module == "") {
            module = 0;
        }
        if (subModule == "" || subModule == null) {
            subModule = 0;
        }
        EventModule.SearchAdvancedGroupPolicies(Description, module, subModule, prefix).done(function (data) {
            AdvancedSearchGroupPolicies.LoadListViewSearchAvanzed(data);
            AdvancedSearchGroupPolicies.Clear();
        });
    }

    static GetSearchGroupPoliciesDescription() {
        let Description = $("#DescriptionGroupPoliciesSearchAdvanzed").val();
        let module = $("#IdModuleSearchAdvanzed").val();
        let subModule = $("#IdSubModuleSearchAdvanzed").val();
        let prefix = $("#IdPrefixSearchAdvanzed").val();
        if (module == "") {
            module = 0;
        }
        if (subModule == "" || subModule == null) {
            subModule = 0;
        }
        EventModule.SearchAdvancedGroupPolicies(Description, module, subModule, prefix).done(function (data) {
            AdvancedSearchGroupPolicies.LoadListViewSearchAvanzed(data);
            AdvancedSearchGroupPolicies.Clear();
        });

    }
    // Muestra la vista parcial
    static ShowAdvancedSearch() {
        dropDownSearchAdvGroupPolicies.show();
        $("#listviewSearch").UifListView({
            displayTemplate: "#searchTemplate",
            selectionType: 'single',
            source: null,
            height: 200
        });
        AdvancedSearchGroupPolicies.LoadTextBoxModule();
        AdvancedSearchGroupPolicies.LoadTextBoxPrefix();
    }

    static LoadTextBoxModule() {
        EventModule.GetModules().done(function (data) {
            if (data) {
                $('#IdModuleSearchAdvanzed').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    static LoadTextBoxPrefix() {
        EventModule.GetPrefixes().done(function (data) {
            if (data) {
                $('#IdPrefixSearchAdvanzed').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static LoadSubModuleByIdModule(event, selectedItem) {
        if (selectedItem.Id == "") {
            $("#IdSubModuleSearchAdvanzed").prop('disabled', 'disabled');
        }
        else {
            var IdModule = selectedItem.Id;
            if (IdModule > 0) {
                EventModule.GetSubModule(IdModule).done(function (data) {
                    if (data) {
                        $('#IdSubModuleSearchAdvanzed').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $("#IdSubmoduleSearchAdvanzed").UifSelect();
            }
        }
    }

    // Oculta busqueda avanzada
    static CancelSearchAdvanzed() {
        dropDownSearchAdvGroupPolicies.hide();
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

    static Clear() {
        //Limpia las validaciones del formulario
        ClearValidation("#GroupPoliciesForm");
        $("#DescriptionGroupPoliciesSearchAdvanzed").val("");
        $("#IdModuleSearchAdvanzed").UifSelect('setSelected', null);
        $("#IdSubModuleSearchAdvanzed").UifSelect('setSelected', null);
        $("#IdPrefixSearchAdvanzed").UifSelect('setSelected', null);
    }

    static GroupPoliciesShowData(event, result, index) {

        if (result) {
            GroupPoliciesIndex = index;
            GroupPoliciesId = result.result[0].IdGroupPolicies;
            $("#IdGroupPolicies").val(result.result[0].IdGroupPolicies);
            $("#DescriptionGroupPolicies").prop('disabled', 'disabled');
            $("#DescriptionGroupPolicies").val(result.result[0].Description);
            $("#IdModule").UifSelect('setSelected', result.result[0].Module.Id);
            EventModule.GetSubModule(result.result[0].Module.Id).done(function (data) {
                if (data) {
                    $('#IdSubModule').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
            $("#IdSubModule").UifSelect('setSelected', result.result[0].SubModule.Id);
            $("#IdPackage").UifSelect('setSelected', result.result[0].Package.PackageId);
            var prefix = result.result[0].Key.split(",", 2);
            $("#IdPrefix").UifSelect('setSelected', prefix[0]);
            EventModule.GetRiskByPrefix(prefix[0]).done(function (data) {
                if (data) {
                    if (data.result.length > 1) {
                        $('#IdRisk').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $('#IdRisk').UifSelect({ sourceData: data.result });
                        $('#IdRisk').val(data.result[0].Id);
                    }
                }
            });
            $("#IdRisk").UifSelect('setSelected', prefix[1]);
        }
    }
}