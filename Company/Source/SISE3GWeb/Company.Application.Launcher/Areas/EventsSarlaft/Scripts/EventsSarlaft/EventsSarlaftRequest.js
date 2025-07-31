let promess = [{}];

class EventsSarlaftRequest {
    /*Grupo de Eventos*/
    static GetModules() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetModules',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetSubModulesByModuleId(ModuleId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetSubModulesByModuleId',
            data: JSON.stringify({ ModuleId: ModuleId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetSubModules() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetSubModules',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetEventGroups() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetEventGroups',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    //-------------------------------------------//
    static CreateGroupEvent(form) {
        let Url = '';
        if (form.Index != '') {
            Url = 'EventsSarlaft/EventsSarlaft/UpdateGroupEvent'
        } else {
            Url = 'EventsSarlaft/EventsSarlaft/CreateGroupEvent'
        }

        return $.ajax({
            type: 'POST',
            url: rootPath + Url,
            data: JSON.stringify({ eventGroup: form }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteGroupEvent(form) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/DeleteGroupEvent',
            data: JSON.stringify({ eventGroup: form }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    
    /*Entidades*/
    static GetEventEntities() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetEventEntities',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetQueryTypesCode() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetQueryTypesCode',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetLeveles() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetLeveles',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetValidationTypes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetValidationTypes',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDataTypes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetDataTypes',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetCodeTableByTable(tableId, tableName, column) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetCodeTableByTable',
            data: JSON.stringify({ tableId: tableId, tableName: tableName, column: column }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    //------------------------------------------//
    static DeleteEntity(entity) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/DeleteEntity',
            data: JSON.stringify({ entityId: entity }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateEntity(form) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/CreateEntity',
            data: JSON.stringify({ entity: form }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    /*Grupo de Condiciones*/
    static GetConditionsGroups() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetConditionsGroups',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDependencesByConditionId(conditionId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetDependencesByConditionId',
            data: JSON.stringify({ conditionId: conditionId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetEntitiesByConditionsGroupId(conditionId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetEntitiesByConditionsGroupId',
            data: JSON.stringify({ conditionsGroupId: conditionId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateCondition(condition) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/CreateCondition',
            data: JSON.stringify({ conditionGroup: condition }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteCondition(conditionId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/DeleteCondition',
            data: JSON.stringify({ conditionId: conditionId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateAssignEntity(condition) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/CreateAssignEntity',
            data: JSON.stringify({ eventCondition: condition }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateDependencies(dependence) {
        let Url = '';
        if (dependence.Index != '') {
            Url = 'EventsSarlaft/EventsSarlaft/UpdateDependencies'
        } else {
            Url = 'EventsSarlaft/EventsSarlaft/CreateDependencies'
        }

        return $.ajax({
            type: 'POST',
            url: rootPath + Url,
            data: JSON.stringify({ dependencies: dependence }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteDependencies(dependence) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/DeleteDependencies',
            data: JSON.stringify({ dependencies: dependence }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    
    /*Eventos*/
    static GetPrefixes() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetPrefixes',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetEventsByEventIdStateIdPrefixId(groupEventId, statusEventId, prefixId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetEventsByEventIdStateIdPrefixId',
            data: JSON.stringify({ groupEventId: groupEventId, statusEventId: statusEventId, prefixId: prefixId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetAccesses() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetAccesses',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetPrefixesByGroupIdEventId(groupId, eventId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetPrefixesByGroupIdEventId',
            data: JSON.stringify({ groupId: groupId, eventId: eventId}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetAccessesByEventIdGroupId(groupId, eventId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetAccessesByEventIdGroupId',
            data: JSON.stringify({ groupId: groupId, eventId: eventId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDelegationsByGroupIdEventId(groupId, eventId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetDelegationsByGroupIdEventId',
            data: JSON.stringify({ groupId: groupId, eventId: eventId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDelegationUsersByGroupIdEventIdHierarchyId(groupId, eventId, hierarchyId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetDelegationUsersByGroupIdEventIdHierarchyId',
            data: JSON.stringify({ groupId: groupId, eventId: eventId, hierarchyId: hierarchyId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetEventConditionsByGroupIdEventIdHierarchyId(groupId, eventId, hierarchyId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetEventConditionsByGroupIdEventIdHierarchyId',
            data: JSON.stringify({ groupId: groupId, eventId: eventId, hierarchyId: hierarchyId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetOperatorConditionByEntityIdQueryTypeId(entityId, queryTypeId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetOperatorConditionByEntityIdQueryTypeId',
            data: JSON.stringify({ entityId: entityId, queryTypeId: queryTypeId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetValuesByGroupIdEntityIdEventIdOperatorId(groupId, entityId, eventId, anotherDependences) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/GetValuesByGroupIdEntityIdEventIdOperatorId',
            data: JSON.stringify({ groupId: groupId, entityId: entityId, eventId: eventId, anotherDependences: anotherDependences }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    //--------------------------------------------//
    static CreateEvent(event, prefixes, executions, rejectCauses) {
        let Url = '';
        if (event.Index != '') {
            Url = 'EventsSarlaft/EventsSarlaft/UpdateEvent'
        } else {
            Url = 'EventsSarlaft/EventsSarlaft/CreateEvent'
        }
        
        return $.ajax({
            type: 'POST',
            url: rootPath + Url,
            data: JSON.stringify({ events: event, prefixes: prefixes, executions: executions, rejectCauses: rejectCauses}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static DeleteEvent(groupEventId, eventId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/DeleteEvent',
            data: JSON.stringify({ groupEventId: groupEventId, eventId: eventId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static UpdateDelegationUser(user, groupId, eventId, hierarchyId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/UpdateDelegationUser',
            data: JSON.stringify({ user: user, groupId: groupId, eventId: eventId, hierarchyId: hierarchyId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    
    static DeleteConditionEntity(condition) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/DeleteConditionEntity',
            data: JSON.stringify({ condition: condition }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    /*EXPORTAR DATA*/
    static ExportData(typeFile, data) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'EventsSarlaft/EventsSarlaft/ExportData',
            data: { data: JSON.stringify(data), typeFile: typeFile }
        });
    }
}