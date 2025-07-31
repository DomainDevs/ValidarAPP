$(() => {
    new FasecoldaSIMIT();
});

class FasecoldaSIMIT extends Uif2.Page {
    getInitialState() {
        FasecoldaSIMIT.GetDocumentTypes().done(function (data) {
            if (data.success) {
                $("#selectDocTypeSimit").UifSelect({ sourceData: data.result });
            }
        });
    }

    bindEvents() {
        $('#btnReloadCaptchaSIMIT').click(FasecoldaSIMIT.ReloadCaptcha);
        $('#inputVerificationCodeSimit').focusout(FasecoldaSIMIT.ValidateCaptcha);
        $('#btnSearchSIMIT').click(FasecoldaSIMIT.btnSearchSIMIT);
        $('#btnExitSISA').click(this.ExitSIMIT);
    }

    static GetTextCaptcha() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Externals/FasecoldaSISA/GetTextCaptcha',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetDocumentTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Externals/FasecoldaSISA/GetDocumentTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static btnSearchSIMIT() {
        let document = $('#inputDocNumberSimit').val();
        let docType = $('#selectDocTypeSimit').val();
        if (document != '' && docType > 0 && $('#inputVerificationCodeSimit').val() != '') {
            FasecoldaSIMIT.CleanForm();
            return $.ajax({
                type: 'POST',
                url: rootPath + 'Externals/FasecoldaSISA/GetFasecoldaSIMIT',
                data: JSON.stringify({ documentType: docType, documentNumber: document }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8'
            }).done(function (data) {
                if (data.success) {
                    $('#tableSIMITResults').UifDataTable('addRow', data.result.ExternalInfrigements);
                    $('#resultDiv').css('display', 'block');
                }
            });
        }
        else {
            $.UifNotify('show', {
                'type': 'info', 'message': Resources.Language.LabelDocumentNumber + ' ' + Resources.Language.FieldRequired, 'autoclose': true
            });
        }
    }

    static CleanForm() {
        $('#inputDocNumberSimit').val('');
        $('#selectDocTypeSimit').UifSelect('setSelected', null);
        $('#inputVerificationCodeSimit').val('');
        $('#tableSIMITResults').UifDataTable('clear');
        $('#btnSearchSIMIT').attr("disabled", true)
        FasecoldaSIMIT.ReloadCaptcha();
    }

    static ReloadCaptcha() {
        $("#captchaImageSIMIT").attr("src", $("#captchaImageSIMIT").attr("src") + '?' + Math.random());
    }

    static ValidateCaptcha() {
        if ($("#inputVerificationCode").val() == "") {
            $('#btnSearch').attr("disabled", true)
        } else {
            FasecoldaSIMIT.GetTextCaptcha().done(function (data) {
                if (data.success) {
                    if (data.result == $("#inputVerificationCodeSimit").val()) {
                        $('#btnSearchSIMIT').attr("disabled", false)
                    }
                    else {
                        $.UifNotify('show', {
                            'type': 'info', 'message': Resources.Language.MSGWRN_CAPTCHA, 'autoclose': true
                        });
                    }
                }
                else {
                    $.UifNotify('show', {
                        'type': 'info', 'message': data.result, 'autoclose': true
                    });
                }
            });
        }
    }

    ExitSIMIT() {
        window.location = rootPath + "Home/Index";
    }
}