var dropDownSearchEvents = null;
var dropDownSearchEntities = null;

class EventsSarlaftAdvanceSearch extends Uif2.Page {
    getInitialState() {
        EventsSarlaftAdvanceSearch.InitialControls();
    }

    bindEvents() {

    }

    static InitialControls()
    {
        dropDownSearchEvents = uif2.dropDown({
            source: rootPath + 'EventsSarlaft/EventsSarlaft/GroupEventsAdvanceSearch',
            element: "#btnSearchAdvGroupEvents",
            align: "right",
            width: 600,
            height: 520,
            loadedCallback: EventsSarlaftAdvanceSearch.AdvancedSearchEvents
        });

        dropDownSearchEntities = uif2.dropDown({
            source: rootPath + 'EventsSarlaft/EventsSarlaft/EntitiesAdvanceSearch',
            element: "#btnSearchAdvGroupEvents",
            align: "right",
            width: 600,
            height: 520,
            loadedCallback: EventsSarlaftAdvanceSearch.AdvancedSearchEntities
        });
    }

    /*Busqueda Avanzada Eventos*/
    static AdvancedSearchEvents() {
        EventsSarlaftAdvanceSearch.ListViewAdvanceEvents(null);
        /*Selects*/
        $('#selectModuleAdv').on('itemSelected', function (event, item) {
            if (item.Id > 0)
                EventsSarlaftAdvanceSearch.loadSubModuleByModuleId(item);
            else
                $('#selectModuleAdv').UifSelect({ sourceData: null, enable: false });
        });

        /*Buttons*/
        $('#btnCancelAdvanceEvents').on('click', function (event, data) {
            EventsSarlaftAdvanceSearch.closeAdvanceSearch();
        });
        $('#btnAdvancedSearchEvents').on('click', function (event, data) {
            EventsSarlaftAdvanceSearch.searchGroupEvents();
        });
        $('#btnAcceptdAdvanceEvents').on('click', function (event, data) {
            EventsSarlaftAdvanceSearch.showDetailGroupEvent();
        });

        let dataStatus = [{ 'Id': -1, 'Description': 'TODOS' }, { 'Id': 1, 'Description': 'HABILITADO' }, { 'Id': 0, 'Description': 'DESHABILITADO' }];
        $('#selectStatus').UifSelect({ sourceData: dataStatus });
        $('#selectModuleAdv').UifSelect({ sourceData: modulesAndSubModules });
    }

    static loadSubModuleByModuleId(item) {
        EventsSarlaft.loadSubModuleByModuleId(item, function (subModules) {
            $('#selectSubModuleAdv').UifSelect({ sourceData: subModules, enable: true });
        });
    }

    static ListViewAdvanceEvents(data) {
        $('#listViewAdvanceEvents').UifListView({
            displayTemplate: '#templateAdvanceEvents',
            selectionType: 'single',
            sourceData: data,
            height: 150
        });
    }

    static searchGroupEvents() {
        EventsSarlaftAdvanceSearch.ListViewAdvanceEvents(null);
        let groupEvents = $('#listViewGroup').UifListView('getData');
        let form = $('#formAdvanceEvents').serializeObject();

        if (form.Id != '')
            groupEvents = groupEvents.filter(function (groupEvent) {
                return groupEvent.Id == form.Id
            });

        if (form.GroupEventDescription != '')
            groupEvents = groupEvents.filter(function (groupEvent) {
                return groupEvent.GroupEventDescription.includes(form.GroupEventDescription.toUpperCase())
            });

        if (form.ModuleId != '')
            groupEvents = groupEvents.filter(function (groupEvent) {
                return groupEvent.ModuleId == form.ModuleId
            });

        if (form.SubmoduleId != '')
            groupEvents = groupEvents.filter(function (groupEvent) {
                return groupEvent.SubmoduleId == form.SubmoduleId
            });

        if (form.ProcedureAuthorized != '')
            groupEvents = groupEvents.filter(function (groupEvent) {
                return groupEvent.ProcedureAuthorized.includes(form.ProcedureAuthorized.toUpperCase())
            });

        if (form.ProcedureReject != '')
            groupEvents = groupEvents.filter(function (groupEvent) {
                return groupEvent.ProcedureReject.includes(form.ProcedureReject.toUpperCase())
            });

        if (form.AuthorizationReport != '')
            groupEvents = groupEvents.filter(function (groupEvent) {
                return groupEvent.AuthorizationReport.includes(form.AuthorizationReport.toUpperCase())
            });

        groupEvents.forEach(function (element, index) {
            $('#listViewAdvanceEvents').UifListView('addItem', element);
        });
    }

    static closeAdvanceSearch() {
        dropDownSearchEvents.hide();
    }

    static showDetailGroupEvent() {
        let data = $('#listViewAdvanceEvents').UifListView('getSelected');

        let find = function (element, index, array) {
            return element.Id === data.Id
        }

        var index = $('#listViewGroup').UifListView("findIndex", find);
        EventsSarlaft.rowEditlistViewGroup(data[0], index);
        dropDownSearchEvents.hide();
    }

    static showAdvanceSearchGroupEvents() {
        dropDownSearchEvents.show();
    }

    /*Busqueda Avanzada entidades*/
    static AdvancedSearchEntities() {
        EventsSarlaftAdvanceSearch.ListViewAdvanceEntities(null);
        $('#btnCancelAdvanceEntities').on('click', function (event, data) {
            EventsSarlaftAdvanceSearch.closeAdvanceSearchEntities();
        });
        $('#btnAdvancedSearchEntities').on('click', function (event, data) {
            EventsSarlaftAdvanceSearch.searchGroupEntities();
        });
        $('#btnAcceptdAdvanceEntities').on('click', function (event, data) {
            EventsSarlaftAdvanceSearch.showDetailEntities();
        });
        $('#selectTypeValidationAdv').on('itemSelected', function (event, data) {

        });

        $('#inputStoredProcedureAdv').prop('disabled', true);
    }

    static ListViewAdvanceEntities(data) {
        $('#listViewAdvanceEntities').UifListView({
            displayTemplate: '#templateAdvanceEntities',
            selectionType: 'single',
            sourceData: data,
            height: 150
        });
    }

    static closeAdvanceSearchEntities() {
        dropDownSearchEntities.hide();
    }

    static searchGroupEntities() {
        EventsSarlaftAdvanceSearch.ListViewAdvanceEntities(null);
        let search = false;
        let entities = $('#listViewEntities').UifListView('getData');
        let form = $('#formAdvanceEntities').serializeObject();

        if (form.Id != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.Id == form.Id
            });
        }

        if (form.EntityDescription != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.EntityDescription.includes(form.EntityDescription.toUpperCase())
            });
        }

        if (form.selectTypeConsultId != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.selectTypeConsultId == form.selectTypeConsultId
            });
        }

        if (form.selectLevelId != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.selectLevelId == form.selectLevelId
            });
        }

        if (form.SourceTable != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.SourceTable.includes(form.SourceTable.toUpperCase())
            });
        }

        if (form.JoinTable != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.JoinTable.includes(form.JoinTable.toUpperCase())
            });
        }

        if (form.selectValidationTypeCode != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.selectValidationTypeCode == form.selectValidationTypeCode
            });
        }

        if (form.ValidationProcedure != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.ValidationProcedure.includes(form.ValidationProcedure.toUpperCase())
            });
        }

        if (form.ValidationTable != '') {
            search = true;
            entities = entities.filter(function (groupEvent) {
                return groupEvent.ValidationTable.includes(form.ValidationTable.toUpperCase())
            });
        }

        if (search) {
            entities.forEach(function (element, index) {
                $('#listViewAdvanceEntities').UifListView('addItem', element);
            });
        }
    }

    static showDetailEntities() {
        let data = $('#listViewAdvanceEntities').UifListView('getSelected');

        let find = function (element, index, array) {
            return element.Id === data.Id
        }

        var index = $('#listViewEntities').UifListView("findIndex", find);
        EventsSarlaft.rowEditlistViewEntities(data[0], index, true);
        dropDownSearchEntities.hide();

        setTimeout(function () {
            EventsSarlaft.disabledFormEntitiy(true);
        }, 800);
    }

    static showAdvanceSearchEntities() {
        dropDownSearchEntities.show();
    }
}