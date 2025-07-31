class ChangePassword extends Uif2.Page {
    GetQueryParameter(parameterName) {
        var queryParameters = window.location.search.substring(1);
        var parameters = queryParameters.split("&");
        for (var i = 0; i < parameters.length; i++) {
            var parameter = parameters[i].split("=");
            if (parameter[0] == parameterName) {
                return parameter[1];
            }
        }
    }

    bindEvents() {
        $("#btnAccept").on("click", this.ChangePassword);
        $("#btnExit").on("click", this.Exit);
        $("#inputOldPassword").ValidatorKey(ValidatorType.Password, 1, 1);  
        $("#inputPassword").ValidatorKey(ValidatorType.Password, 1, 1);  
        $("#inputConfirmPassword").ValidatorKey(ValidatorType.Password, 1, 1);  
    }

   
    ChangePassword() {
        if ($("#formChangePassword").valid()) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Account/SavePassword',
                data: JSON.stringify({ OldPassword: $("#inputOldPassword").val(), Password: $("#inputPassword").val(), ConfirmPassword: $("#inputConfirmPassword").val() }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    window.location = rootPath + "Home/Index";
                }
                else {
                    for (var i = 0; i < data.result.length; i++) {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result[i], 'autoclose': true });
                    }
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorChanginPassword, 'autoclose': true });
            });
        }
    }

    Exit() {
        if (GetQueryParameter("cause") != undefined) {
            window.location = rootPath + "Account/LogOff";
        }
        else {
            window.location = rootPath + "Home/Index";
        }
    }


}