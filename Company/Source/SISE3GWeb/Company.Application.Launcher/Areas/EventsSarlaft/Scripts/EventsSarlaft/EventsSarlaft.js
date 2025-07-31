var modulesAndSubModules = null;

class EventsSarlaft extends Uif2.Page {

    getInitialState() {
        $('#listViewGroup').UifListView({
            customEdit: true, localMode: false, add: false, customDelete: false, remove: true, delete: true, edit: true, focusItem: true,
            displayTemplate: '#display-template-group', drag: false, sourceData: null, deleteCallback: EventsSarlaft.rowDeletelistViewGroup
        });
        EventsSarlaft.initialStateControls();
        EventsSarlaft.initialEventsSarlaft();
    }

    bindEvents() {
        /*ButtonGeneric*/
        $('#btnExport').on('click', function (event, data) {
            EventsSarlaft.exportData();
        });

        /*Grupo de Eventos*/
        /*Selects*/
        $('#selectModule').on('itemSelected', function (event, item) {
            if (item.Id > 0)
                EventsSarlaft.loadSubModuleByModuleId(item, false);
            else
                $('#selectSubModule').UifSelect({ sourceData: null, enable: false });
        });

        /*listView*/
        $('#listViewGroup').on('rowEdit', function (event, data, index) {
            EventsSarlaft.rowEditlistViewGroup(data, index);
        });

        /*Buttons*/
        $('#btnNewGroupEvent').on('click', function (event, data) {
            EventsSarlaft.newGroupEvent();
        });
        $('#btnAddGroupEvent').on('click', function (event, data) {
            EventsSarlaft.addGroupEvent();
        });
        $("#btnSearchAdvGroupEvents").on("click", EventsSarlaft.showSerchAdvance);

        /*Entidades*/
        /*listView*/
        $('#listViewEntities').on('rowAdd', function (event, data, index) {
            EventsSarlaft.rowAddlistViewEntities(data, index);
        });
        $('#listViewEntities').on('rowEdit', function (event, data, index) {
            EventsSarlaft.rowEditlistViewEntities(data, index, false);
        });

        /*AutoCompletes*/
        $('#inputNameEntitie').on('itemSelected', function (event, selectedItem) {
            EventsSarlaft.itemSelectedNameEntity(selectedItem);
        });
        $('#inputOriginTable').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0)
                EventsSarlaft.itemSelectedOriginTable(selectedItem);
        });
        $('#inputJoinTable').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0)
                EventsSarlaft.itemSelectedJoinTable(selectedItem);
        });
        $('#inputValidationsTables').on('itemSelected', function (event, selectedItem) {
            if (selectedItem.Id > 0)
                EventsSarlaft.itemValidationsTables(selectedItem);
        });

        /*Buttons*/
        $('#btnSaveEntity').on('click', function (event, data) {
            EventsSarlaft.saveEntity();
        });
        $('#btnExportarEntity').on('click', function (event, data) {
            EventsSarlaft.exportDataEntity();
        });

        /*Grupo de Condiciones*/
        /*ListView*/
        $('#assignEntity').on('click', function (event) {
            EventsSarlaft.openAssignEntityModal();
        });
        $('#assingDependence').on('click', function (event) {
            EventsSarlaft.openAssignDependence();
        });
        $('#listViewAssingDependence').on('rowEdit', function (event, data, index) {
            EventsSarlaft.rowEditViewAssignEntity(data, index);
        });
        $('#listViewAssingDependence').on('rowAdd', function (event, data, index) {
            EventsSarlaft.rowAddViewAssignEntity(data, index);
        });

        /*Buttons*/
        $('#btnSaveAssign').on('click', function (event, data) {
            EventsSarlaft.saveAssign();
        });
        $('#editFormDependence').on('Save', function () {
            EventsSarlaft.saveDependence();
        });;

        /*Eventos*/
        /*Selects*/
        $('#selectGroupEvent').on('itemSelected', function (event, item) {
            EventsSarlaft.loadEventsByGroupEventIdStatusIdPrefixId();
        });
        $('#selectStatusEvent').on('itemSelected', function (event, item) {
            EventsSarlaft.loadEventsByGroupEventIdStatusIdPrefixId();
        });
        $('#selectPrefix').on('itemSelected', function (event, item) {
            EventsSarlaft.loadEventsByGroupEventIdStatusIdPrefixId();
        });
        $('#selectValidationType').on('itemSelected', function (event, item) {
            EventsSarlaft.itemSelectedValidation(item);
        });
        $('#selectDelegation').on('itemSelected', function (event, item) {
            EventsSarlaft.itemSelectedDelegation(item);
        });
        $('#selectOperator').on('itemSelected', function (event, item) {
            if (item.Id > 0)
                EventsSarlaft.itemSelectedOperator();
            else
                $('#selectValue').UifSelect({ sourceData: null })
        });

        /*listView*/
        $('#listViewEvents').on('rowAdd', function (event, data, index) {
            EventsSarlaft.rowAddListViewEvents(data, index);
        });
        $('#listViewEvents').on('rowEdit', function (event, data, index) {
            EventsSarlaft.rowEditListViewEvents(data, index);
        });

        /*Buttons*/
        $('#conditions').on('click', function (event, data) {
            EventsSarlaft.openConditions();
        });
        $('#authorizedUsers').on('click', function (event, data) {
            EventsSarlaft.openAuthorizedUsers();
        });
        $('#btnSaveEvent').on('click', function (event, data) {
            EventsSarlaft.saveEvent();
        });

        /*DataTable*/
        $('#tableUser').on('rowEdit', function (event, data, position) {
            EventsSarlaft.rowEditTableUser(data, position);
        });        $('#editFormUser').on('Save', function () {
            EventsSarlaft.saveTableUser();
        });;
        $('#editFormCondition').on('Save', function () {
            EventsSarlaft.saveTableCondition();
        });;
    }

    /*Eventos de inicio*/
    static initialStateControls() {
        $('#selectSubModule').prop('disabled', true);
        $('#inputNameEntitie').TextTransform(ValidatorType.UpperCase);
        $('#selectCodeTableOrigin').prop('disabled', true);
        $('#selectDescriptionTableOrigin').prop('disabled', true);

        let dataStatus = [{ 'Id': -1, 'Description': 'TODOS' }, { 'Id': 1, 'Description': 'HABILITADO' }, { 'Id': 0, 'Description': 'DESHABILITADO' }];
        $('#selectStatusEvent').UifSelect({ sourceData: dataStatus });
    }

    static SetValuesOnEdit(moduleId, subModuleId) {
        if (moduleId != null || moduleId != undefined || module != 0) {
            EventsSarlaftRequest.GetModules().done(function (dataModules) {
                if (dataModules.success) {
                    $('#selectModule').UifSelect({ sourceData: dataModules.result, selectedId: moduleId });
                    if (subModuleId != null || subModuleId != undefined || subModuleId != 0) {
                        EventsSarlaftRequest.GetSubModules().done(function (data) {
                            if (data.success) {
                                var submodules = data.result.filter(subModels => subModels.ModuleId == moduleId)
                                $('#selectSubModule').UifSelect({ sourceData: submodules, selectedId: subModuleId });
                            }
                        });
                    }
                }
            });
        }

    }

    /*Metodos de consulta de inicio*/
    static initialEventsSarlaft() {
        /*Grupo de Eventos*/
        EventsSarlaftRequest.GetModules().done(function (dataModules) {
            if (dataModules.success) {
                modulesAndSubModules = dataModules.result;
                $('#selectModule').UifSelect({ sourceData: dataModules.result });


                EventsSarlaftRequest.GetSubModules().done(function (data) {
                    if (data.success) {
                        do {
                            modulesAndSubModules = modulesAndSubModules.map(function (Modules) {
                                Modules.companySubModules = data.result.filter(subModels => subModels.ModuleId == Modules.Id)
                                return Modules;
                            });

                            EventsSarlaftRequest.GetEventGroups().done(function (data) {
                                if (data.success) {
                                    $('#selectGroupEvent').UifSelect({ sourceData: data.result, enable: true });
                                    $('#selectEventsGroup').UifSelect({ sourceData: data.result, enable: true });

                                    data.result = data.result.map(function (eventsGroup) {
                                        eventsGroup.NameModule = modulesAndSubModules.find(function (module) { return module.Id == eventsGroup.ModuleId }).Description
                                        eventsGroup.NameSubmodule = modulesAndSubModules.find(function (module) { return module.Id == eventsGroup.ModuleId }).companySubModules.find(function (subModule) { return subModule.Id == eventsGroup.SubmoduleId }).Description
                                        eventsGroup.chkEnabledInd = eventsGroup.Enabled
                                        return eventsGroup;
                                    });

                                    $('#listViewGroup').UifListView({
                                        customEdit: true, localMode: false, add: false, customDelete: false, remove: true, delete: true, edit: true, focusItem: true,
                                        displayTemplate: '#display-template-group', drag: false, sourceData: data.result, deleteCallback: EventsSarlaft.rowDeletelistViewGroup
                                    });
                                }
                                else {
                                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                                }
                            });
                        } while (modulesAndSubModules == null)
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': dataModules.result, 'autoclose': true });
            }
        });

        $('#chkEnabled').prop('disabled', true);

        /*Entidades*/
        EventsSarlaftRequest.GetEventEntities().done(function (data) {
            if (data.success) {
                data.result = data.result.map(function (entities) {
                    entities.selectTypeConsultId = entities.QueryTypeId
                    entities.selectLevelId = entities.LevelId
                    entities.selectJoinTable = entities.JoinTable
                    entities.selectSourceDescription = entities.SourceDescription
                    entities.selectJoinSourceField = entities.JoinSourceField
                    entities.selectJoinTargetField = entities.JoinTargetField
                    entities.selectValidationTypeCode = entities.ValidationTypeId
                    entities.selectDataKeyCode = entities.DataKeyTypeId
                    entities.selectValidationField = entities.ValidationField
                    entities.selectDataTypeCode = entities.DataFieldTypeId
                    entities.selectKey1Field = entities.Key1Field
                    entities.selectKey2Field = entities.Key2Field
                    entities.selectKey3Field = entities.Key3Field
                    entities.selectKey4Field = entities.Key4Field
                    return entities;
                });

                $('#listViewEntities').UifListView({
                    customEdit: true, localMode: false, add: true, customAdd: true, customDelete: false, delete: true, edit: true, title: 'Entidades',
                    displayTemplate: '#display-template-Entities', drag: false, sourceData: data.result, deleteCallback: EventsSarlaft.rowDeleteViewEntities
                });

                $('#tableAssignEntity').UifDataTable({ sourceData: data.result });

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        EventsSarlaftRequest.GetQueryTypesCode().done(function (data) {
            if (data.success) {
                $('#selectTypeConsult').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
                $('#selectTypeConsultAdv').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        EventsSarlaftRequest.GetLeveles().done(function (data) {
            if (data.success) {
                $('#selectLevel').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
                $('#selectLevelAdv').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        EventsSarlaftRequest.GetValidationTypes().done(function (data) {
            if (data.success) {
                $('#selectTypeValidation').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
                $('#selectValidationType').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
                $('#selectTypeValidationAdv').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        EventsSarlaftRequest.GetDataTypes().done(function (data) {
            if (data.success) {
                $('#selectDataType').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
                $('#selectCodeTypeKey').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        /*Grupo de Condiciones*/
        EventsSarlaftRequest.GetConditionsGroups().done(function (data) {
            if (data.success) {
                $('#selectCondition').UifSelect({ sourceData: data.result, enable: true });

                data.result = data.result.map(function (ConditionsGroups) {

                    if (ConditionsGroups.RelatedEntities != null) {
                        ConditionsGroups.RelatedEntities = ConditionsGroups.RelatedEntities.substring('', ConditionsGroups.RelatedEntities.lastIndexOf(','));
                        ConditionsGroups.RelatedEntitiesId = ConditionsGroups.RelatedEntitiesId.substring('', ConditionsGroups.RelatedEntities.lastIndexOf(','));
                    };

                    return ConditionsGroups;
                })

                $('#listViewGroupConditions').UifListView({
                    customEdit: false, localMode: false, add: true, customAdd: false, customDelete: false, delete: true, edit: true, title: 'Grupo de Condiciones', focusItem: true, selectionType: 'single',
                    displayTemplate: '#display-template-GroupConditions', addTemplate: '#display-add-template-GroupConditions', drag: false, sourceData: data.result,
                    deleteCallback: EventsSarlaft.rowDeleteViewGroupConditions, editCallback: EventsSarlaft.rowEditViewGroupConditions, addCallback: EventsSarlaft.rowAddViewGroupConditions
                });

                $('#listViewGroupConditions .form .toolbar').css('text-align', 'end');

                $('#listViewGroupConditions .item-list .content .item .card-button.edit-button').click(function (data) {
                    setTimeout(function () {
                        $('#listViewGroupConditions .list .slimScrollDiv .content .item .form .uif-col-12 .row .uif-col-12 #inputCondition').prop('disabled', true);
                    }, 50);
                })
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        /*Eventos*/
        EventsSarlaftRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result.sort(sortAlphabet) });
                $('#tablePrefix').UifDataTable({ sourceData: data.result })
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

        EventsSarlaftRequest.GetAccesses().done(function (data) {
            if (data.success) {
                $('#tableExecution').UifDataTable({ sourceData: data.result })
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static showSerchAdvance() {
        if ($('#EventsGroupTab').hasClass('active')) {
            EventsSarlaftAdvanceSearch.showAdvanceSearchGroupEvents();
        } else if ($('#EntitiesTab').hasClass('active')) {
            EventsSarlaftAdvanceSearch.showAdvanceSearchEntities();
        }
    }

    /*Grupo de Eventos*/
    /*Eventos onChange*/
    static loadSubModuleByModuleId(item, callback) {
        EventsSarlaftRequest.GetSubModulesByModuleId(item.Id).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);

                $('#selectSubModule').UifSelect({ sourceData: data.result, enable: true });
                $('#selectSubModule').UifSelect('setFocus');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static rowEditlistViewGroup(item, index) {
        let module = { Id: item.ModuleId };
        //$('#formEventsGroup').Deserialize(item);
        $('#inputGroupEvent').val(item.Id);
        $('#inputNameGroupEvent').val(item.GroupEventDescription);
        EventsSarlaft.SetValuesOnEdit(item.ModuleId, item.SubmoduleId);
        $('#inputAuthorizationProcedure').val(item.ProcedureAuthorized);
        $('#inputRejectionProcedure').val(item.ProcedureReject);
        $('#inputAuthorizationReport').val(item.AuthorizationReport);
        var checkEnable = (item.chkEnabledInd = "on") ? true : false;
        $('#chkEnabled').prop('checked', checkEnable);
        $('#indexId').val(index);

        $('#chkEnabled').prop('disabled', false);
        $('#inputGroupEvent').prop('disabled', true);
        $('#selectModule').UifSelect('setSelected', item.ModuleId);

        //EventsSarlaft.loadSubModuleByModuleId(module, function (subModules) {
        //    $('#selectSubModule').UifSelect({ sourceData: subModules, enable: true, selectedId: String(item.SubmoduleId) });
        //});
    }

    static rowDeletelistViewGroup(deferred, data) {
        EventsSarlaftRequest.DeleteGroupEvent(data).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $.UifNotify('show', { 'type': 'success', 'message': 'Grupo de Eventos eliminado correctamente', 'autoclose': true });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static newGroupEvent() {
        $('#indexId').val('');
        $('#inputGroupEvent').prop('disabled', false);
        $('#inputGroupEvent').val('');
        $('#inputNameGroupEvent').val('');
        $('#selectModule').UifSelect('setSelected', -1)
        $('#selectSubModule').UifSelect('setSelected', -1)
        $('#inputAuthorizationProcedure').val('');
        $('#inputRejectionProcedure').val('');
        $('#inputAuthorizationReport').val('');
        $('#chkEnabled').prop('disabled', true);
    }

    static addGroupEvent() {
        $('#formEventsGroup').validate();
        if ($('#formEventsGroup').valid()) {
            if (EventsSarlaft.eventValidate()) {
                let form = $('#formEventsGroup').serializeObject();

                form.Enabled = form.chkEnabledInd;

                EventsSarlaftRequest.CreateGroupEvent(form).done(function (data) {
                    if (data.success) {
                        form.NameModule = $('#selectModule').UifSelect('getSelectedText');
                        form.NameSubmodule = $('#selectSubModule').UifSelect('getSelectedText');

                        if (form.Index != '') {
                            $('#listViewGroup').UifListView('editItem', form.Index, form);
                            $('#listViewGroup').UifListView('setEditing', form.Index, true);
                        } else {
                            $('#listViewGroup').UifListView('addItem', form);
                        }

                        $.UifNotify('show', { 'type': 'success', 'message': 'Grupo de Eventos guardado correctamente', 'autoclose': true });

                        EventsSarlaft.newGroupEvent();
                        $('#indexId').val('');
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': 'Ya existe el grupo de evento', 'autoclose': true });
            }
        }
    }

    static eventValidate() {
        let valid = true;
        let nameGroup = $('#inputNameGroupEvent').val();

        if ($('#formEventsGroup').serializeObject().Index == '') {
            let found = $('#listViewGroup').UifListView('getData').filter(function (groups) {
                return groups.GroupEventDescription === nameGroup;
            });
            if (found.length > 0) {
                return valid = false;
            } else {
                return valid;
            }
        } else {
            return valid;
        }
    }

    static exportData() {
        let typeFile = 0;
        let data = null;

        if ($('#EventsGroupTab').hasClass('active')) {
            typeFile = 1;
            data = $('#listViewGroup').UifListView('getData');

        } else if ($('#EntitiesTab').hasClass('active')) {
            typeFile = 2;
            data = $('#listViewEntities').UifListView('getData');

        } else if ($('#ConditionsGroupTab').hasClass('active')) {
            typeFile = 3;
            data = $('#listViewGroupConditions').UifListView('getData');

        } else if ($('#EventsTab').hasClass('active')) {
            typeFile = 4;
            data = $('#listViewEvents').UifListView('getData');

        }

        if (data.length > 0) {
            EventsSarlaftRequest.ExportData(typeFile, data).done(function (data) {
                if (data.success) {
                    DownloadFile(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'No hay data para exportar', 'autoclose': true });
        }
    }

    static exportDataEntity() {
        let dataEnitity = [];
        dataEnitity.push($('#formEntitiesEdit').serializeObject());
        if (data.length > 0) {
            EventsSarlaftRequest.ExportData(2, dataEnitity).done(function (data) {
                if (data.success) {
                    DownloadFile(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'No hay data para exportar', 'autoclose': true });
        }
    }

    /*Entidades*/
    /*Eventos onChange*/
    static rowAddlistViewEntities() {
        EventsSarlaft.disabledFormEntitiy(false);

        $('#btnExportarEntity').hide();
        $('#btnSaveEntity').show();

        $('#selectValidationKeyOne').prop('disabled', true);
        $('#selectValidationKeyTwo').prop('disabled', true);
        $('#selectValidationKeyThree').prop('disabled', true);
        $('#selectValidationKeyFour').prop('disabled', true);
        $('#selectValidateField').prop('disabled', true);

        $('#formEntitiesEdit').Deserialize({});

        $('#EntitiesEditModal').UifModal('showLocal', 'Agregar Entidad');
        $('#EntitiesEditModal .modal-dialog.modal-lg').attr('style', 'width: 80%');
    }

    static rowEditlistViewEntities(item, index, toExport) {
        EventsSarlaft.disabledFormEntitiy(false);

        $('#formEntitiesEdit').Deserialize(item);
        $('#indexEntity').val(index);
        if (item.SourceTable != null && item.SourceTable != '') {
            EventsSarlaft.getCodeTableOrigin(item.SourceTable.split('.')[1], function (result) {
                let idSourceCode = result.find(function (sourceCode) { return sourceCode.Description === item.SourceCode }).Id;
                let idSourceDescription = result.find(function (sourceDescription) { return sourceDescription.Description === item.SourceDescription }).Id;

                $('#selectCodeTableOrigin').UifSelect({ sourceData: result, selectedId: idSourceCode, enable: true });
                $('#selectDescriptionTableOrigin').UifSelect({ sourceData: result, selectedId: idSourceDescription, enable: true });
            });
        } else {
            $('#selectCodeTableOrigin').UifSelect({ enable: false });
            $('#selectDescriptionTableOrigin').UifSelect({ enable: false });
        }

        if (item.ValidationTable != null && item.ValidationTable != '') {
            EventsSarlaft.getCodeTableOrigin(item.ValidationTable.split('.')[1], function (result) {
                $('#selectValidationKeyOne').UifSelect({ sourceData: result, selectedId: item.selectKey1Field, enable: true });
                $('#selectValidationKeyTwo').UifSelect({ sourceData: result, selectedId: item.selectKey2Field, enable: true });
                $('#selectValidationKeyThree').UifSelect({ sourceData: result, selectedId: item.selectKey3Field, enable: true });
                $('#selectValidationKeyFour').UifSelect({ sourceData: result, selectedId: item.selectKey4Field, enable: true });
            });
        } else {
            $('#selectValidationKeyOne').UifSelect({ enable: false });
            $('#selectValidationKeyTwo').UifSelect({ enable: false });
            $('#selectValidationKeyThree').UifSelect({ enable: false });
            $('#selectValidationKeyFour').UifSelect({ enable: false });
        }

        if (toExport) {
            $('#EntitiesEditModal').UifModal('showLocal', 'Resultado Busqueda Avanzada');
            $('#EntitiesEditModal .modal-dialog.modal-lg').attr('style', 'width: 80%');
            $('#btnExportarEntity').show();
            $('#btnSaveEntity').hide();
        } else {
            $('#EntitiesEditModal').UifModal('showLocal', 'Editar Entidad');
            $('#EntitiesEditModal .modal-dialog.modal-lg').attr('style', 'width: 80%');
            $('#btnExportarEntity').hide();
            $('#btnSaveEntity').show();
        }
    }

    static rowDeleteViewEntities(deferred, data) {
        EventsSarlaftRequest.DeleteEntity(data.Id).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $.UifNotify('show', { 'type': 'success', 'message': 'Entidad eliminada correctamente', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static itemSelectedNameEntity(selectedItem) {
        $.UifNotify('show', { 'type': 'danger', 'message': 'Ya existe una entidad con este nombre', 'autoclose': true });
    }

    static itemSelectedOriginTable(selectedItem) {
        EventsSarlaftRequest.GetCodeTableByTable(selectedItem.Id, '', '').done(function (data) {
            if (data.success) {
                $('#selectCodeTableOrigin').UifSelect({ sourceData: data.result, enable: true });
                $('#selectDescriptionTableOrigin').UifSelect({ sourceData: data.result, enable: true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static itemSelectedJoinTable(selectedItem) {
        EventsSarlaftRequest.GetCodeTableByTable(selectedItem.Id, '', '').done(function (data) {
            if (data.success) {
                $('#selectTableCodeUnion').UifSelect({ sourceData: data.result, enable: true });
                $('#selectCodeUnion').UifSelect({ sourceData: data.result, enable: true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static getCodeTableOrigin(tableName, callback) {
        EventsSarlaftRequest.GetCodeTableByTable(0, tableName, '').done(function (data) {
            if (data.success) {
                return callback(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static getCodeTableJoin(tableName, callback) {
        EventsSarlaftRequest.GetCodeTableByTable(0, tableName, '').done(function (data) {
            if (data.success) {
                return callback(data.result);

                $('#selectTableCodeUnion').UifSelect({ sourceData: data.result, enable: true });
                $('#selectCodeUnion').UifSelect({ sourceData: data.result, enable: true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static itemValidationsTables(selectedItem) {
        EventsSarlaftRequest.GetCodeTableByTable(selectedItem.Id, '', '').done(function (data) {
            if (data.success) {
                $('#selectValidationKeyOne').UifSelect({ sourceData: data.result, enable: true });
                $('#selectValidationKeyTwo').UifSelect({ sourceData: data.result, enable: true });
                $('#selectValidationKeyThree').UifSelect({ sourceData: data.result, enable: true });
                $('#selectValidationKeyFour').UifSelect({ sourceData: data.result, enable: true });
                $('#selectValidateField').UifSelect({ sourceData: data.result, enable: true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static saveEntity() {
        $('#formEntitiesEdit').validate();
        if ($('#formEntitiesEdit').valid()) {
            let form = $('#formEntitiesEdit').serializeObject();

            EventsSarlaftRequest.CreateEntity(EventsSarlaft.CreateEntity(form)).done(function (data) {
                if (data.success) {

                    if (form.Index != '') {
                        $('#listViewEntities').UifListView('editItem', form.Index, form);
                        $.UifNotify('show', { 'type': 'success', 'message': 'Entidad guardada correctamente', 'autoclose': true });
                    } else {
                        $('#listViewEntities').UifListView('addItem', form);
                        form.Index = $('#listViewEntities').UifListView('getData').length - 1;
                        $.UifNotify('show', { 'type': 'success', 'message': 'Entidad agregada correctamente', 'autoclose': true });
                    }

                    $('#listViewEntities').UifListView('setEditing', form.Index, true);

                    $('#indexEntity').val('');
                    $('#EntitiesEditModal').UifModal('hide');
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static CreateEntity(form) {

        form.SourceCode = $('#selectCodeTableOrigin').UifSelect('getSelectedText');
        form.SourceDescription = $('#selectDescriptionTableOrigin').UifSelect('getSelectedText');
        form.QueryType = { QueryTypeCode: form.selectTypeConsultId };
        form.DataFieldType = { DataTypeCode: form.selectDataTypeCode };
        form.DataKeyType = { DataTypeCode: form.selectDataKeyCode };
        form.ValidationType = { ValidationTypeCode: form.selectValidationTypeCode };
        form.DataFieldTypeId = form.selectDataTypeCode;
        form.DataFieldTypeDescription = $('#selectDataType').UifSelect('getSelectedText');
        form.DataKeyTypeId = form.selectDataKeyCode;
        form.DataKeyTypeDescription = $('#selectCodeTypeKey').UifSelect('getSelectedText');
        form.QueryTypeId = form.selectTypeConsultId;
        form.QueryTypeDescription = $('#selectTypeConsult').UifSelect('getSelectedText');
        form.ValidationTypeId = form.selectValidationTypeCode;
        form.ValidationTypeDescription = $('#selectTypeValidation').UifSelect('getSelectedText');
        form.GroupBy = GroupByInd;
        form.ValidationField = form.selectValidationField;
        //form.ValidationKeyField = "";
        form.Description = form.EntityDescription;
        form.Key1Field = form.selectKey1Field;
        form.Key2Field = form.selectKey2Field;
        form.Key3Field = form.selectKey3Field;
        form.Key4Field = form.selectKey4Field;
        form.LevelId = form.selectLevelId;
        form.ValidationKeyField = "";
        form.JoinSourceField = form.selectJoinSourceField;
        form.JoinTargetField = form.selectJoinTargetField;

        return form;
    }

    static disabledFormEntitiy(disabled) {
        $('#inputNameEntitie').prop('disabled', disabled);
        $('#selectTypeConsult').prop('disabled', disabled);
        $('#selectLevel').prop('disabled', disabled);
        $('#inputOriginTable').prop('disabled', disabled);
        $('#selectCodeTableOrigin').prop('disabled', disabled);
        $('#selectDescriptionTableOrigin').prop('disabled', disabled);
        $('#inputJoinTable').prop('disabled', disabled);
        $('#selectTableCodeUnion').prop('disabled', disabled);
        $('#selectCodeUnion').prop('disabled', disabled);
        $('#inputDescriptionTableOrigin').prop('disabled', disabled);
        $('#selectTypeValidation').prop('disabled', disabled);
        $('#inputStoredProcedure').prop('disabled', disabled);
        $('#inputValidationsTables').prop('disabled', disabled);
        $('#selectCodeTypeKey').prop('disabled', disabled);
        $('#selectValidationKeyOne').prop('disabled', disabled);
        $('#selectValidationKeyTwo').prop('disabled', disabled);
        $('#selectValidationKeyThree').prop('disabled', disabled);
        $('#selectValidationKeyFour').prop('disabled', disabled);
        $('#selectValidateField').prop('disabled', disabled);
        $('#selectDataType').prop('disabled', disabled);
        $('#inputCondition').prop('disabled', disabled);
        $('#chkGroup').prop('disabled', disabled);
    }

    /*Grupo de condiciones*/
    static rowAddViewGroupConditions(deferred, form) {
        deferred.resolve();
        form.Id = 0;
        EventsSarlaftRequest.CreateCondition(form).done(function (data) {
            if (data.result) {
                setTimeout(function () {
                    $('#listViewGroupConditions').UifListView('addItem', form);
                    $.UifNotify('show', { 'type': 'success', 'message': 'Grupo de condiciones agregado correctamente', 'autoclose': true });
                }, 100);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static rowEditViewGroupConditions(deferred, data) {
        deferred.resolve(data);
        $.UifNotify('show', { 'type': 'success', 'message': 'Grupo de condiciones modificado correctamente', 'autoclose': true });
    }

    static rowDeleteViewGroupConditions(deferred, form) {
        EventsSarlaftRequest.DeleteCondition(form.Id).done(function (data) {
            if (data.result) {
                deferred.resolve();
                $.UifNotify('show', { 'type': 'success', 'message': 'Grupo de condiciones eliminado correctamente', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    /*Grupo de condiciones - Asignar Dependencias - Entidades*/
    static openAssignEntityModal() {
        if ($('#listViewGroupConditions').UifListView('getSelected').length > 0) {
            let conditionId = $('#listViewGroupConditions').UifListView('getSelected')[0].Id;

            let value = {
                label: 'Id',
                values: []
            };

            $('#tableAssignEntity').UifDataTable('getData').forEach(function (item, index) {
                value.values.push(item.Id);
            });

            $('#tableAssignEntity').UifDataTable('setUnselect', value);

            EventsSarlaftRequest.GetEntitiesByConditionsGroupId(conditionId).done(function (data) {
                if (data.success) {
                    let value = {
                        label: 'Id',
                        values: []
                    };

                    data.result.forEach(function (item, index) {
                        value.values.push(item.Id);
                    });

                    $('#tableAssignEntity').UifDataTable('setSelect', value);

                    $('#assignEntityModal').UifModal('showLocal', 'Asignar / Editar Entidades a las condiciones');
                    $('#assignEntityModal .modal-dialog.modal-lg').attr('style', 'width: 40%');
                } else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            })
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Seleccione un Grupo de Condiciones', 'autoclose': true });
        }
    }

    static openAssignDependence() {
        if ($('#listViewGroupConditions').UifListView('getSelected').length > 0) {
            $('#editFormDependence').UifInline('hide');

            let itemSelected = $('#listViewGroupConditions').UifListView('getSelected')[0];

            $('#listViewAssingDependence').UifListView('getData').forEach(function (item, index) {
                $('#listViewAssingDependence').UifListView("deleteItem", index);
            })

            EventsSarlaftRequest.GetDependencesByConditionId(itemSelected.Id).done(function (data) {
                if (data.success) {

                    $('#listViewAssingDependence').UifListView({
                        customEdit: true, localMode: false, add: true, customAdd: true, customDelete: false, delete: true, edit: true, title: 'Entidades', height: 350,
                        displayTemplate: '#display-template-AssingDependence', drag: false, sourceData: data.result,
                        deleteCallback: EventsSarlaft.rowDeleteViewAssignEntity
                    });

                    $('#inputGrupConditions').text(itemSelected.Description)
                    $('#assingDependenceModal').UifModal('showLocal', 'Asignar / Editar Entidades a las condiciones');
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Seleccione un Grupo de Condiciones', 'autoclose': true });
        }

    }

    static saveAssign() {
        let object = { descriptions: "", values: "" };

        let groupCondition = $('#listViewGroupConditions').UifListView('getSelected')[0];
        let tableAssing = $('#tableAssignEntity').UifDataTable('getSelected');

        groupCondition.RelatedEntities = "";
        groupCondition.RelatedEntitiesId = "";

        if (tableAssing != null) {
            tableAssing.forEach(function (item, index) {
                object.values += item.Id + ',';
                object.descriptions += item.EntityDescription + ',';
            });

            groupCondition.RelatedEntities = object.descriptions.substring('', object.descriptions.lastIndexOf(','));
            groupCondition.RelatedEntitiesId = object.values.substring('', object.values.lastIndexOf(','));
        }

        var conditions = groupCondition;

        EventsSarlaftRequest.CreateAssignEntity(EventsSarlaft.CreateEntities(conditions)).done(function (data) {
            if (data.result) {
                $('#listViewGroupConditions').UifListView('editItem', groupCondition.Id, groupCondition);
                $('#listViewGroupConditions').UifListView('setEditing', groupCondition.Id, true);

                $('#assignEntityModal').UifModal('hide');
                $.UifNotify('show', { 'type': 'success', 'message': 'Entidades guardadas correctamente', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static CreateEntities(conditions) {
        let condition = { Id: "", entities: [] };
        let entities = conditions.RelatedEntitiesId.split(',');
        entities.forEach(function (item, index) {
            let entity = { Id: parseInt(item) };
            condition.entities.push(entity);
        });

        condition.Id = conditions.Id;

        return condition;
    }

    static rowEditViewAssignEntity(item, index) {
        $('#formAssingDependence').Deserialize(item);
        $('#indexDependence').val(index);

        EventsSarlaft.getEntitiesByConditionsGroupId(item.ConditionsId, function (entities) {
            $('#selectEntityDepend').UifSelect({ sourceData: entities, enable: false, selectedId: item.DependesId });
            $('#selectEntityPrincipal').UifSelect({ sourceData: entities, enable: false, selectedId: item.EntityId });
        });

        $('#inputGroupConditions').val(item.Column);
        $('#editFormDependence').UifInline('show');
    }

    static saveDependence() {
        $('#formAssingDependence').validate();
        if ($('#formAssingDependence').valid()) {
            let form = $('#formAssingDependence').serializeObject();

            EventsSarlaftRequest.CreateDependencies(form).done(function (data) {
                if (data.success) {
                    form.DependsDescription = $('#selectEntityDepend').UifSelect('getSelectedText');
                    form.EntityDescription = $('#selectEntityPrincipal').UifSelect('getSelectedText');

                    if (form.Index != '') {
                        $('#listViewAssingDependence').UifListView('editItem', form.Index, form);
                        $('#listViewAssingDependence').UifListView('setEditing', form.Index, true);
                        $.UifNotify('show', { 'type': 'success', 'message': 'Entidad de condicion modificado correctamente', 'autoclose': true });
                    } else {
                        $('#listViewAssingDependence').UifListView('addItem', form);
                        $.UifNotify('show', { 'type': 'success', 'message': 'Entidad de condicion agregado correctamente', 'autoclose': true });
                    }

                    $('#editFormDependence').UifInline('hide');
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static getEntitiesByConditionsGroupId(conditionId, callback) {
        EventsSarlaftRequest.GetEntitiesByConditionsGroupId(conditionId).done(function (data) {
            if (data.success) {
                if (callback)
                    return callback(data.result);
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static rowAddViewAssignEntity() {
        let dependence = $('#listViewAssingDependence').UifListView('getData')[0]

        EventsSarlaft.getEntitiesByConditionsGroupId(dependence.ConditionsId, function (entities) {
            $('#conditionId').val(dependence.ConditionsId);
            $('#selectEntityDepend').UifSelect({ sourceData: entities, enable: true });
            $('#selectEntityPrincipal').UifSelect({ sourceData: entities, enable: true });
            $('#inputGroupConditions').val('');
            $('#editFormDependence').UifInline('show');
        });
    }

    static rowDeleteViewAssignEntity(deferred, data) {
        EventsSarlaftRequest.DeleteDependencies(data).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $.UifNotify('show', { 'type': 'success', 'message': 'Entidad de condicion eliminado correctamente', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });

    }

    /*Eventos*/
    static loadEventsByGroupEventIdStatusIdPrefixId() {
        let groupEventId = $('#selectGroupEvent').UifSelect('getSelected') == '' ? -1 : $('#selectGroupEvent').UifSelect('getSelected');
        let statusEventId = $('#selectStatusEvent').UifSelect('getSelected') == '' ? -1 : $('#selectStatusEvent').UifSelect('getSelected');
        let prefixId = $('#selectPrefix').UifSelect('getSelected') == '' ? -1 : $('#selectPrefix').UifSelect('getSelected');

        EventsSarlaftRequest.GetEventsByEventIdStateIdPrefixId(groupEventId, statusEventId, prefixId).done(function (data) {
            if (data.success) {
                data.result = data.result.map(function (event) {
                    event.State = event.Enabled === true ? Resources.Language.Enabled : Resources.Language.Disabled
                    event.selectEventsGroup = event.GroupEventId
                    event.selectEventConditionGroup = event.ConditionId
                    event.selectValidationType = event.ValidationTypeId
                    event.chkEnabled = event.Enabled
                    event.chkEnabledStop = event.EnabledStop
                    event.chkEnabledAuthorize = event.EnabledAuthorize
                    return event;
                })

                $('#listViewEvents').UifListView({
                    customEdit: true, localMode: false, add: true, customAdd: true, customDelete: false, delete: true, edit: true, title: 'Eventos', focusItem: true, selectionType: 'single',
                    displayTemplate: '#display-template-Events', drag: false, sourceData: data.result, deleteCallback: EventsSarlaft.rowDeleteListViewEvents
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static rowDeleteListViewEvents(deferred, item) {
        EventsSarlaftRequest.DeleteEvent(item.GroupEventId, item.Id).done(function (data) {
            if (data.success) {
                deferred.resolve();
                $.UifNotify('show', { 'type': 'success', 'message': 'Grupo de Eventos eliminado correctamente', 'autoclose': true });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static rowAddListViewEvents() {
        $('#formEventEdit').Deserialize({});

        let valuePrefix = { label: 'Id', values: [] };

        $('#tablePrefix').UifDataTable('getData').forEach(function (item, index) {
            valuePrefix.values.push(item.Id);
        });

        let valueExec = { label: 'Id', values: [] };

        $('#tableExecution').UifDataTable('getData').forEach(function (execution, index) {
            valueExec.values.push(execution.Id);
        });

        $('#tableExecution').UifDataTable('setUnselect', valueExec)
        $('#tablePrefix').UifDataTable('setUnselect', valuePrefix)

        $('#eventEditModal').UifModal('showLocal', 'Agregar un Evento');

        $('#selectEventsGroup').prop('disabled', false);
    }

    static rowEditListViewEvents(item, index) {
        $('#formEventEdit').Deserialize(item);
        $('#indexEvent').val(index);

        if (item.ValidationTypeId == 1) {
            $('#inputNameProcedure').UifAutoComplete('disabled', true)
        } else {
            $('#inputNameProcedure').UifAutoComplete('disabled', false)
        }

        //Quitar Seleccion DataTable
        let value = {
            label: 'Id',
            values: []
        };

        $('#tablePrefix').UifDataTable('getData').forEach(function (item, index) {
            value.values.push(item.Id);
        });

        $('#tablePrefix').UifDataTable('setUnselect', value)

        EventsSarlaftRequest.GetPrefixesByGroupIdEventId(item.Id, item.GroupEventId).done(function (data) {
            if (data.success) {
                let valuePrefix = {
                    label: 'Id',
                    values: []
                };

                data.result.forEach(function (prefix, index) {
                    valuePrefix.values.push(prefix.PrefixId);
                });

                $('#tablePrefix').UifDataTable('setSelect', valuePrefix);

                let valueExec = {
                    label: 'Id',
                    values: []
                };

                $('#tableExecution').UifDataTable('getData').forEach(function (execution, index) {
                    valueExec.values.push(execution.Id);
                });

                $('#tableExecution').UifDataTable('setUnselect', valueExec)

                EventsSarlaftRequest.GetAccessesByEventIdGroupId(item.Id, item.GroupEventId).done(function (response) {
                    if (response.success) {
                        let valueAccess = {
                            label: 'Id',
                            values: []
                        };

                        response.result.forEach(function (execution, index) {
                            valueAccess.values.push(execution.AccessId);
                        });

                        $('#listViewEvents').UifListView("setSelected", index, true);

                        $('#tableExecution').UifDataTable('setSelect', valueAccess);
                        $('#eventEditModal').UifModal('showLocal', 'Editar un Evento');

                        $('#selectEventsGroup').prop('disabled', true);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                    }
                });

            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static saveEvent() {
        let form = $('#formEventEdit').serializeObject();
        let event = $('#listViewEvents').UifListView('getSelected')[0];

        event = EventsSarlaft.CreateEvent(event, form);
        let prefixes = EventsSarlaft.CreatePrefixes();
        let executions = EventsSarlaft.CreateExecution();
        let rejectCauses = EventsSarlaft.CreateRejectCauses();

        EventsSarlaftRequest.CreateEvent(event, prefixes, executions, rejectCauses).done(function (response) {
            if (response.success) {

                if (event.Index != '') {
                    $('#listViewEvents').UifListView('editItem', event.Index, event);
                    $('#listViewEvents').UifListView('setEditing', event.Index, true);
                    $.UifNotify('show', { 'type': 'success', 'message': 'Evento actualizado correctamente', 'autoclose': true });
                } else {
                    let index = $('#listViewEvents').UifListView('getData').length;
                    $('#listViewEvents').UifListView('addItem', event);
                    $('#listViewEvents').UifListView('setEditing', index, true);
                    $.UifNotify('show', { 'type': 'success', 'message': 'Evento agregado correctamente', 'autoclose': true });
                }

                $('#eventEditModal').UifModal('hide');
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static CreateEvent(event, form) {
        if (typeof (event) == 'undefined') {
            event = {};
        }

        event.Index = form.Index;
        event.Description = form.Description;
        event.DescriptionErrorMessage = form.DescriptionErrorMessage;
        event.GroupEventDescription = $('#selectEventsGroup').UifSelect('getSelectedText');
        event.GroupEventId = form.selectEventsGroup;
        event.selectEventsGroup = form.selectEventsGroup;
        event.ConditionDescription = $('#selectCondition').UifSelect('getSelectedText');
        event.ConditionId = form.selectEventConditionGroup;
        event.selectEventConditionGroup = form.selectEventConditionGroup;
        event.ValidationTypeId = form.selectValidationType;
        event.selectValidationType = form.selectValidationType;
        event.State = $('#chkEnabledEvent').is(':checked') === true ? Resources.Language.Enabled : Resources.Language.Disabled;
        event.ProcedureName = form.ProcedureName;
        event.chkEnabled = $('#chkEnabledEvent').is(':checked');
        event.chkEnabledAuthorize = $('#chkAuthorize').is(':checked');
        event.chkEnabledStop = $('#chkStop').is(':checked');

        return event;
    }

    static CreatePrefixes() {
        let tablePrefixes = $('#tablePrefix').UifDataTable('getSelected');
        let prefixes = [];

        tablePrefixes.forEach(function (item, index) {
            let prefix = { Id: parseInt(item.Id) };
            prefixes.push(prefix);
        });

        return prefixes;
    }

    static CreateExecution() {
        let tableExecutions = $('#tableExecution').UifDataTable('getSelected');
        let executions = [];

        tableExecutions.forEach(function (item, index) {
            let execution = { Id: parseInt(item.Id) };
            executions.push(execution);
        });

        return executions;
    }

    static CreateRejectCauses() {
        let tableRejects = $('#tableExecution').UifDataTable('getSelected');
        let rejectCauses = [];

        rejectCauses.push({ Id: parseInt(0) });

        return rejectCauses;
        /*
        tableRejects.forEach(function (item, index) {
            let execution = { Id: parseInt(item.Id) };
            rejectCauses.push(execution);
        });

        return rejectCauses;*/
    }

    static openConditions() {
        if ($('#listViewEvents').UifListView('getSelected').length > 0) {
            $('#divCondition').html('');
            let event = $('#listViewEvents').UifListView('getSelected')[0];

            EventsSarlaftRequest.GetDelegationsByGroupIdEventId(event.GroupEventId, event.Id).done(function (data) {
                if (data.success) {
                    $('#divUser').prop('hidden', true);
                    $('#divCondition').prop('hidden', false);

                    $('#selectDelegation').UifSelect({ sourceData: data.result })

                    $('#labelGroupCondition').text(event.GroupEventDescription);
                    $('#labelEventEventCondition').text(event.ConditionDescription);

                    $('#editFormUser').UifInline('hide');
                    $('#editFormCondition').UifInline('hide');

                    $('#conditionsAndUsers').UifModal('showLocal', 'Condiciones');
                    $('#conditionsAndUsers .modal-dialog.modal-lg').attr('style', 'width: 60%');
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Seleccione un evento', 'autoclose': true });
        }
    }

    static openAuthorizedUsers() {
        if ($('#listViewEvents').UifListView('getSelected').length > 0) {
            let event = $('#listViewEvents').UifListView('getSelected')[0];

            EventsSarlaftRequest.GetDelegationsByGroupIdEventId(event.GroupEventId, event.Id).done(function (data) {
                if (data.success) {
                    $('#divUser').prop('hidden', false);
                    $('#divCondition').prop('hidden', true);

                    $('#selectDelegation').UifSelect({ sourceData: data.result });

                    $('#labelGroupCondition').text(event.GroupEventDescription);
                    $('#labelEventEventCondition').text(event.Description);

                    $('#editFormUser').UifInline('hide');
                    $('#editFormCondition').UifInline('hide');

                    $('#conditionsAndUsers').UifModal('showLocal', 'Usuarios Autorizados');
                    $('#conditionsAndUsers .modal-dialog.modal-lg').attr('style', 'width: 80%');

                    $('#tableUser').UifDataTable({ sourceData: [] });
                    $('#tableCondition').UifDataTable({ sourceData: [] });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Seleccione un evento', 'autoclose': true });
        }
    }

    static itemSelectedDelegation(itemSelected) {
        if (itemSelected.Id != '') {
            let event = $('#listViewEvents').UifListView('getSelected')[0];
            let hierarchyId = $('#selectDelegation').UifSelect('getSelected');

            if (!$('#divUser')[0].hidden) {
                EventsSarlaftRequest.GetDelegationUsersByGroupIdEventIdHierarchyId(event.GroupEventId, event.Id, hierarchyId).done(function (data) {
                    if (data.success) {

                        $('#tableUser').UifDataTable({ sourceData: data.result });
                        $('#editFormUser').UifInline('hide');
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            } else {

                AjaxRequest(rootPath + "EventsSarlaft/EventsSarlaft/GetEventConditionsViewByGroupIdEventIdHierarchyId", "POST", { "groupId": event.GroupEventId, "eventId": event.Id, "hierarchyId": hierarchyId },
                    function (result) {
                        $('#divCondition').html(result);
                        $('#tableCondition').UifDataTable({ sourceData: [] });
                        EventsSarlaftRequest.GetEventConditionsByGroupIdEventIdHierarchyId(event.GroupEventId, event.Id, hierarchyId).done(function (data) {
                            if (data) {
                                if (data.length > 0) {
                                    $('#tableCondition').UifDataTable('addRow', data);
                                    $('#editFormCondition').UifInline('hide');
                                    $('#tableCondition').on('rowEdit', function (event, data, position) {
                                        EventsSarlaft.rowEditTableConditions(data, position);
                                    });                                    $('#tableCondition').on('rowDelete', function (event, data, position) {
                                        EventsSarlaft.rowDeleteTableConditions(data, position);
                                    });                                }
                                $('#tableCondition').on('rowAdd', function (event, data, position) {
                                    EventsSarlaft.rowAddTableConditions();
                                });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }, null, null);
            }
        } else {
            $('#tableUser').UifDataTable({ sourceData: [] });
            $('#tableCondition').UifDataTable({ sourceData: [] });
        }
    }

    static itemSelectedOperator() {
        let item = $('#selectOperator').UifSelect('getSelectedSource');
        let event = $('#listViewEvents').UifListView('getSelected')[0];
        let anotherDependences = [{}];
        let entityId = $('#entityId').val();

        EventsSarlaftRequest.GetValuesByGroupIdEntityIdEventIdOperatorId(event.GroupEventId, entityId, event.Id, anotherDependences).done(function (data) {
            if (data.success) {
                let tbody = $('#controlsValues div')[0];
                for (let i = 0; i <= tbody.childElementCount; i++) {
                    tbody.childNodes.forEach(function (item, index) { $('#' + item.id).remove() });
                }

                if (item.NumValues > 1) {
                    for (let i = 0; i < item.NumValues; i++) {
                        let div = document.createElement('div');
                        let control = document.createElement('select');

                        div.className = 'uif-col-6';
                        div.id = 'divSelect' + i;

                        control.name = 'ValueId';
                        control.id = 'selectValue' + i;
                        control.className = 'uif-select form-control';
                        div.appendChild(control);
                        tbody.appendChild(div);

                        $('#selectValue' + i).attr('data-name', 'Description');
                        $('#selectValue' + i).attr('data-id', 'Id');
                        $('#selectValue' + i).UifSelect({ sourceData: data.result });
                    }
                } else {
                    let div = document.createElement('div');
                    let control = document.createElement('select');

                    div.className = 'uif-col-12';
                    div.id = 'divSelect';

                    control.name = 'ValueId';
                    control.id = 'selectValue';
                    control.className = 'uif-select form-control';
                    div.appendChild(control);
                    tbody.appendChild(div);

                    $('#selectValue').attr('data-name', 'Description');
                    $('#selectValue').attr('data-id', 'Id');
                    $('#selectValue').UifSelect({ sourceData: data.result });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static rowEditTableUser(item, index) {
        $('#formUserEvent').Deserialize(item);
        $('#formUserEvent').find('#indexUser').val(index);

        $('#formUserEvent').find('#chkAuthorized').prop('checked', item.Authorized);
        $('#formUserEvent').find('#chkNotified').prop('checked', item.Notificated);
        $('#formUserEvent').find('#chkDefaultEvent').prop('checked', item.NotificatedDefault);        $('#editFormUser').UifInline('show');
    }

    static saveTableUser() {
        let event = $('#listViewEvents').UifListView('getSelected')[0];
        let hierarchy = $('#selectDelegation').UifSelect('getSelected');
        let form = $('#formUserEvent').serializeObject();

        form.Authorized = $('#formUserEvent').find('#chkAuthorized').is(':checked');
        form.Notificated = $('#formUserEvent').find('#chkNotified').is(':checked');
        form.NotificatedDefault = $('#formUserEvent').find('#chkDefaultEvent').is(':checked');

        EventsSarlaftRequest.UpdateDelegationUser(form, event.GroupEventId, event.Id, hierarchy).done(function (data) {
            if (data.success) {
                $('#tableUser').UifDataTable('editRow', form, form.Index);
                $('#editFormUser').UifInline('hide');
                $.UifNotify('show', { 'type': 'success', 'message': 'Registro actualizado correctamente', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static rowEditTableConditions(item, index, callback, appealback) {
        let event = $('#listViewEvents').UifListView('getSelected')[0];
        let anotherDependences = [];

        EventsSarlaftRequest.GetEntitiesByConditionsGroupId(event.ConditionId).done(function (response) {
            if (response.success) {
                response = response.result[0];
                EventsSarlaftRequest.GetOperatorConditionByEntityIdQueryTypeId(response.Id, response.QueryTypeId).done(function (dataOperator) {
                    if (dataOperator.success) {
                        $('#formConditionEvent').find('#selectOperator').UifSelect({ sourceData: dataOperator.result, enable: true });
                        $('#labelEntities').text(response.EntityDescription);
                        $('#entityId').val(response.Id);

                        if (callback)
                            return callback(dataOperator);

                        EventsSarlaftRequest.GetValuesByGroupIdEntityIdEventIdOperatorId(event.GroupEventId, response.Id, event.Id, anotherDependences).done(function (data) {
                            if (data.success) {
                                let tbody = $('#controlsValues div')[0];

                                for (let i = 0; i <= tbody.childElementCount; i++) {
                                    tbody.childNodes.forEach(function (item, index) { $('#' + item.id).remove() });
                                }

                                let div = document.createElement('div');
                                let control = document.createElement('select');

                                div.className = 'uif-col-12';
                                div.id = 'divSelect';

                                control.name = 'ValueId';
                                control.id = 'selectValue';
                                control.className = 'uif-select form-control';
                                div.appendChild(control);
                                tbody.appendChild(div);

                                $('#selectValue').attr('data-name', 'Description');
                                $('#selectValue').attr('data-id', 'Id');
                                //$('#selectValue').UifSelect({ sourceData: data.result });

                                $('#formConditionEvent').find('#selectValue').UifSelect({ sourceData: data.result, enable: true });

                                $('#editFormCondition').UifInline('show');
                                $('#selectValue').UifSelect('setSelected', item.Id);

                                dataOperator.result.forEach(function (element, index) {
                                    if (element.Description.lastIndexOf(item.Description.split(' ')[0]) > -1) {
                                        $('#selectOperator').UifSelect('setSelected', element.ComparatorId);
                                    }
                                });
                                $('#formConditionEvent').find('#indexCondition').val(index);
                                $('#formConditionEvent').find('#conditionId').val(item.Id);
                            } else {
                                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                            }
                        });
                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': dataOperator.result, 'autoclose': true });
                    }
                });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static rowAddTableConditions() {
        EventsSarlaft.rowEditTableConditions(null, null, function (response) {
            $('#editFormCondition').Deserialize({});
            $('#editFormCondition').UifInline('show');
        }, null);
    }

    static rowDeleteTableConditions(data, index) {
        let event = $('#listViewEvents').UifListView('getSelected')[0];
        let delegation = $('#selectDelegation').UifSelect('getSelected');

        let condition = {
            GroupEventId: event.GroupEventId,
            EventId: event.Id,
            DelegationId: delegation.Id,
            ComparatorId: data.IdCondition
        }

        EventsSarlaftRequest.DeleteCondition(condition).done(function (data) {
            if (data.success) {
                $("#tableCondition").UifDataTable('deleteRow', index)
                $.UifNotify('show', { 'type': 'success', 'message': 'Condición eliminada correctamente', 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static saveTableCondition() {
        let form = $('#formConditionEvent').serializeObject();

        form.Operator = $('#selectOperator').UifSelect('getSelectedText');
        form.Description = $('#selectOperator').UifSelect('getSelectedText') + ' ';

        if (form.ValueId.length > 1) {
            let tbody = $('#controlsValues div')[0];
            var a = 0;

            tbody.childNodes.forEach(function (item, index) {
                if (typeof (item) != "undefined") {
                    form.Description += $('#' + item.id).UifSelect('getSelectedText') + ' y ';
                }
            });

            form.Description = form.Description.substring('', form.Description.lastIndexOf(' y '));
        } else {
            form.Description += $('#selectValue').UifSelect('getSelectedText');
        }

        if (form.Index != '') {
            $('#tableCondition').UifDataTable('editRow', form, form.Index)
            $.UifNotify('show', { 'type': 'success', 'message': 'Condicion actualizado correctamente', 'autoclose': true });
        } else {
            $('#tableCondition').UifDataTable('addRow', form);
            $.UifNotify('show', { 'type': 'success', 'message': 'Condicion agregado correctamente', 'autoclose': true });
        }

        $('#editFormCondition').UifInline('hide');
    }

    static itemSelectedValidation(itemSelected) {
        if (itemSelected.Id == 1) {
            $('#inputNameProcedure').UifAutoComplete('disabled', true)
        } else {
            $('#inputNameProcedure').UifAutoComplete('disabled', false)
        }
    }
}

function sortAlphabet(a, b) {
    if (a.Description < b.Description)
        return -1;
    if (a.Description > b.Description)
        return 1;
    return 0;
}