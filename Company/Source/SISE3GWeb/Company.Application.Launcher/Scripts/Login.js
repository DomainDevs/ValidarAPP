$('form').submit(function () {
    if ($('form').valid()) {
        $.UifProgress('show');
        $('input[type=submit]').attr('disabled', true);
        $('#password').attr('disabled', true);
    }
});