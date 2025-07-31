$.ajaxSetup({ async: false });
var selectedOptions = {};
var operationId;
var account;
var userId;
var personId;
var temporalId;
//Validar el Temporal y el Administrador Actual del Temporal y Obtener el Listado de los Usuarios Administradores del Temporal
$(document).ready(function (event, data) {
    $("#inputSearchTemporal").ValidatorKey(ValidatorType.Number, 2, 0);
    $("#inputSearchUser").ValidatorKey(ValidatorType.Onlylettersandnumbers,3, 0);
    changeBarSearch();
    EnableDisableControls("-1", "inputSearchUser", "nameUser", "addUser");
});

$('#inputSearchTemporal').on('buttonClick', function (event, data) {
    $('#temporalId').text($('#inputSearchTemporal').val());
    EnableDisableControls("-1", "inputSearchUser", "nameUser", "addUser");

    $.get(rootPath + "Collective/Collective/GetAdministratorUsersTempByTempId?operationId=" + $('#temporalId').text(), function (responses) {
            if (responses.success) {
                if (!responses.result.tempId == 0) {
                    selectedOptions.temporalId = $('#temporalId').text();
                    selectedOptions.endoType = responses.result.EndoType;
                    changeBarSearch();
                    EnableDisableControls("0", "inputSearchUser");                    
                    var controller = rootPath + "Collective/Collective/GetAdministratorUsersTempByTempIdUserId?tempId=" + responses.result.tempId;
                    $('#tableUser').UifDataTable({ source: controller });
                    operationId = $('#temporalId').text();
                    temporalId = responses.result.tempId;
                }
                else {
                    selectedOptions.temporalId = null;
                    selectedOptions.endoType = null;
                    changeBarSearch();
                    $.UifNotify('show', { 'type': 'info', 'message': responses.result.ErrorMsg, 'autoclose': true });
                    $('#tableUser').UifDataTable('clear');
                    EnableDisableControls("-1", "inputSearchUser", "nameUser", "addUser");
                }
            }
            else {
                EnableDisableControls("-1", "inputSearchUser", "nameUser", "addUser");
                selectedOptions.temporalId = null;
                selectedOptions.endoType = null;
                changeBarSearch();
                $.UifNotify('show', { 'type': 'info', 'message': responses.result.ErrorMsg, 'autoclose': true });
            }
        });
});

//Validar y Obtener los datos del Usuario
$('#formAuthorizeUser').on('buttonClick', '#inputSearchUser', function () {
    if ($('#inputSearchUser').val() != "" && $('#inputSearchUser').val() != null)
    {
        $.ajax({
            type: "POST",
            url: rootPath+"Collective/Collective/GetUserPersonByAccount",
            data: JSON.stringify({ account: $('#inputSearchUser').val() }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"

    }).done(function (data) {
        if (data.success)
        {
            account = $('#inputSearchUser').val();
            userId = data.result.userId;
            personId = data.result.personId;
            $('#nameUser').val(data.result.nameUser);
            EnableDisableControls("0", "addUser");
        }
        else
        {
            $('#nameUser').val("");
            EnableDisableControls("-1", "addUser");
            $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorMsg, 'autoclose': true });
        }
    });
}
else
{
    $('#nameUser').val("");
    EnableDisableControls("-1", "addUser");
    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EnterUserCodeAuthorized, 'autoclose': true });
}
});

//Guardar el Nuevo Usuario Administrador del Temporal
$('#addUser').on('click', function () {
    if ($('#nameUser').val() != "" || $('#inputSearchTemporal') != "")
    {
        if ($('#inputSearchTemporal').val() == operationId) //Si cambian el Código del Temporal Intencionalmente antes AgregarUsuario
        {
            if ($('#inputSearchUser').val() == account) //Si Cambian el Código del Usuario Intencionalmente antes AgregarUsuario
            {
                //Crea un Nuevo Registro en la Tabla MSV.ADMINISTRADOR_USER_TEMP
                $.post("SetAdministratorUsersTempByTempIdUserId", { tempId: temporalId, userId: userId }, function (data) {
                    if (data.success) {
                        $('#inputSearchUser').val('');
                        $('#nameUser').val('');
                        EnableDisableControls("-1", "addUser");
                        var controller = rootPath + "Collective/Collective/GetAdministratorUsersTempByTempIdUserId?tempId=" + temporalId;
                        $('#tableUser').UifDataTable({ source: controller });
                    }
                    else
                    {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorMsg, 'autoclose': true });
                    }
                });
            }
            else
            {
                $('#nameUser').val('');
                EnableDisableControls("-1", "addUser");
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UserNotExist, 'autoclose': true });
            }
        }
        else
        {
            $('#inputSearchTemporal').val('');
            $('#temporalId').text('');
            EnableDisableControls("-1", "addUser");
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorTempNoExist, 'autoclose': true });
        }
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EmptyFields, 'autoclose': true });
    }
});

$("#closeWindow").on("click", function () {
    window.location.replace("Collective", "Collective");
});

//Función para Habilitar(0) ó Deshabilitar(-1) los controles 
//que se introducen (ID) como argumentos
//El primer argumento indica si es para Habilitar(0) o Deshabilitar(-1) 
function EnableDisableControls()
{
    if (arguments[0] == '-1')
    {
        for (var i = 1; i < arguments.length; i++) {
            $('#' + arguments[i]).attr('disabled', arguments[0]);
        }
    }
    else
    {
        for (var i = 1; i < arguments.length; i++) {
            $('#' + arguments[i]).removeAttr('disabled');
        }
    }
}

function changeBarSearch() {
    barSearch = 'Temporario' + ' : ';
    if ('temporalId' in selectedOptions && selectedOptions.temporalId !== null) {
        barSearch += selectedOptions.temporalId + ' - ';
    }
    if ('endoType' in selectedOptions && selectedOptions.endoType !== null) {
        barSearch += selectedOptions.endoType;
    }
    return true;
}