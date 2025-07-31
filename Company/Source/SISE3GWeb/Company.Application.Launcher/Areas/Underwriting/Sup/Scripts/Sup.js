class Sup extends Uif2.Page  {
    getInitialState() {
        SupRequest.GetSupKey($('#userName').val()).done(function (data) {
            if (data.success && Sup.CheckSUPExist(data.result)) {
                var iframe = $('<iframe>', {
                    src: data.result,
                    style: 'width: 100%!important; border:none;overflow-y'
                });
                $('#supSourceFrame').append(iframe);
                var height = $('.page').height() - $('header').height() - $('.main-title').height();
                $(iframe).css('height', height + 'px');
                $(window).on('resize', function () {
                    $(iframe).css('height', $('.page').height() - $('header').height() - $('.main-title').height());
                });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': "Ocurrió un error cargando SUP,  contacte al administrador.", 'autoclose': true });
                $('#supError').prop('hidden', false);
            }
           
        });
        
        $(document).ready(function () {
            Sup.adaptateScreen() 
        });
    }

    static adaptateScreen() {
        $('footer').remove();
        $('.main-container').addClass('full-screen-sup');
        $('#main').addClass('full-screen-sup');
    }
    bindEvents() {
        
    }

    static CheckSUPExist(url) {
        try {
            var http = new XMLHttpRequest();
            http.open('GET', url, false);
            http.setRequestHeader("Content-Type", "application/json");
            http.setRequestHeader("Access-Control-Allow-Origin", "*");
            http.send();
            if (http.status == 200)
                return true;
            else
                return false;
        } catch (err) {
            return false;
        }
    
    }  
}

class SupRequest{
    static GetSupKey(user) {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Sup/Sup/GetSupKey',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: {
                UserLogin:user
            }
        });
    }
}
