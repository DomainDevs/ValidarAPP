$.ajaxSetup({ async: false });
///Div donde se renderiza la pantalla (popUp), resumen de los evetos
document.write("<div id='Script-Events' class='main-container'></div>");

///Array que contiene los eventos generados en la ejecucion actual
var ArrayEvents = [];



///Metodo que obtiene los eventos generados en la ejecucion actual
///_Location : descripcion de la pantalla (ENUM)
///_IdTemp_: id del Json del temporal
///_key1: id del json del riesgo
///_key2:
///_key3:
///_key4:
/// RETURN: 
///        true  -- si existen eventos restrictivos
///        false -- si no existen eventos restrictivos
function GetGeneratedEvents(eventsCriteria) {

    ArrayEvents = [];
    AjaxRequest(rootPath + "Events/Notification/GetGeneratedEvents", "POST",
        {
            "eventsCriteria": eventsCriteria
        }, function (data) {
            $.each(data, function (index, item) {
                ArrayEvents.push({
                    DescriptionError: "(" + item.Count + ") " + item.DescriptionError,
                    EnabledStop: item.EnabledStop,
                    EventId: item.EventId
                });
            });
        }, null, null);
    var result = LaunchEvent();
    return result;
}

///recorre el array de eventos generados, mostrando por pantalla (UifNotify,UifDialog)
///el evento generado
/// RETURN: 
///        true  -- si existen eventos restrictivos
///        false -- si no existen eventos restrictivos
function LaunchEvent() {
    var count = 0;
    var i;
    for (i = 0; i < ArrayEvents.length; i++) {
        if (ArrayEvents[i].EnabledStop === true) {
            $.UifDialog('alert', { 'message': ArrayEvents[i].DescriptionError, 'title': Resources.Language.LabelRestrictiveEvents });
            return EventType.Restrictive;
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': ArrayEvents[i].DescriptionError, 'autoclose': true });
        }
        count++;
    }
    if (count > 0)
        return EventType.Notification;
    else
        return EventType.Nothing;
}

///obtiene el resumen de los eventos del temporal
///_IdTemp_: id del Json del temporal
/// RETURN: 
///        true  -- si no existen eventos 
///        false -- existen eventos
function GetEventsSummary(_idtemp, type, module, subModule, listDelegationResult) {
    var _result = false;

    if (listDelegationResult !== null && listDelegationResult !== undefined) {
        $.each(listDelegationResult, (index, item) => {
            item.EventDate = FormatDate(item.EventDate);
        });
    }

    AjaxRequest(rootPath + "Events/Notification/GetEventsSummary", "POST",
        {
            "IdModule": module,
            "IdSubmodule": subModule,
            "IdTemp": _idtemp,
            "Type": type,
            "delegationResult": listDelegationResult
        }, function (data) {
            if (data.success == true) {
                _result = true;
            }
            else if (data.success == false) {
                $.UifDialog('alert', { 'message': data.result });
                _result = false;
            }
            else {
                $("#Script-Events").html(data);
            }
        }, null, null);
    return _result;
}

//**************Metodos genericos*************************/

/*metodo generico para enviar una peticion ajax
_url = url de la paticion (obligatorio)
_method = "POST" o "GET" (obligatorio)
_data = array de datos {numero: 1....} 
_element = elemento que donde cargaran los datos 
_done = funcion  al ser exitoso 
_error = funcion al generarse un error
*/
function AjaxRequest(_url, _method, _data, _done, _error, _async) {
    $.ajax({
        url: _url,
        method: _method,
        data: _data,
        async: _async == null ? false : true
    }).success(function (result) {
        _done(result);
    }).error(function (xhr, status) {
        if (_error != null) {
            _error(xhr, status);
        }
    });
}