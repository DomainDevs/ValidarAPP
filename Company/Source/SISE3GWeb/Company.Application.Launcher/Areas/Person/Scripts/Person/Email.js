var emailId = 0;
var emailRowId = -1;
var emails = [];
var emailsGet = [];
var heightListViewEmail = 256;
class Email extends Uif2.Page {
    getInitialState() {
        this.InitializeEmail();
    }

    bindEvents() {

        $("#btnNewEmail").click(Email.ClearEmail);
        $("#btnAcceptEmail").click(this.AcceptEmail);
        $("#btnCreateEmail").click(this.CreateEmailBtn);
        $("#btnCancelEmail").click(Email.ClearEmail);
        $('#listEmails').on('rowEdit', this.EmailsEdit);
    }

    InitializeEmail() {
        $('#inputEmail').ValidatorKey(ValidatorType.Emails, 0, 0);
    }

    //funciones Inicializar
    static ClearEmail() {
        emailId = 0;
        emailRowId = -1;
        $("#selectEmailType").prop("disabled", false);
        $("#selectEmailType").UifSelect("setSelected", null);
        $('#inputEmail').val('');
        $('#chkEmailPrincipal').attr('checked', false);
    }

    AcceptEmail() {
        if (Email.ExistsPrincipalEmail()) {
            Email.SaveEmail();
            Email.ClearEmail();
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorExistsEmailMain, 'autoclose': true });
        }
       
    }

    CreateEmailBtn() {
        if (Email.ValidateEmailData()) {
            Email.CreateEmail();
            Email.ClearEmail();
        }
    }

    EmailsEdit(event, data, index) {
        emailRowId = index;
        Email.EditEmail(data, index);
    }

    static SaveEmail() {
        Email.SendEmail();
        switch (searchType) {
            case TypePerson.PersonNatural:
                Email.setPrincipalEmail();
                break;
            case TypePerson.PersonLegal:
                Email.setPrincipalEmailCompany();
                break;
            default:
                break;
        }
        Email.CloseEmail();

    }

    static ExistsPrincipalEmail() {
        var list = $("#listEmails").UifListView('getData');
        var exist = false;
        $.each(list, function (key, value) {
            if (value.IsPrincipal || value.IsMailingAddress) {
                exist = true;
            }
        });
        return exist;
    }

    static ValidateEmailData() {

        var email = Email.CreateEmailModel();
        var contEmailTypes = false;
        var list = $("#listEmails").UifListView('getData');
        $.each(list, function (key, value) {
            var item = this;
            if (item.EmailTypeId == email.EmailTypeId && email.Id == 0) {
                contEmailTypes = true;
                return false;
            }
            if (contEmailTypes) {
                return false;
            }
        })

        var error = "";
        if (contEmailTypes) {
            error = error + AppResourcesPerson.ErrorTypeEmail + '<br>';
        }
        if ($('#selectEmailType').UifSelect("getSelected") == '') {
            error = error + AppResourcesPerson.LabelEmailType + "<br>";
        }
        if ($('#inputEmail').val() == '') {
            error = error + AppResourcesPerson.LabelEmail + "<br>";
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + " <br>" + error, 'autoclose': true })
            return false;
        } else if (!ValidateEmail($('#inputEmail').val())) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmail, 'autoclose': true });
            return false;
        } else if (Email.PrincipalEmail()) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmailMain, 'autoclose': true });
            return false;
        }
        else if (Email.DuplicateEmail($('#inputEmail').val(), $('#selectEmailType').UifSelect("getSelected"))) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmailExists, 'autoclose': true });
            return false;
        }
        return true;
    }

    //seccion Creacion y Grabado
    static CreateEmail() {
        var emailTmp = Email.CreateEmailModel();
        if (emailTmp.IsPrincipal == true || emailTmp.IsMailingAddress == true) {
            switch (searchType) {
                case TypePerson.PersonNatural:
                    $("#selectEmailTypePn").UifSelect("setSelected", emailTmp.EmailTypeId);
                    $("#inputEmailPn").val(emailTmp.Description);
                    break;
                case TypePerson.PersonLegal:
                    $("#selectCompanyEmailType").UifSelect("setSelected", emailTmp.EmailTypeId);
                    $("#inputCompanyEmail").val(emailTmp.Description);
                    break;
            }
        }
        if (emailTmp.EmailTypeId == EmailType.ElectronicBilling) {
            $("#inputEmailElectronicBilling").val(emailTmp.Description);
        }

        if (emailRowId == -1) {
            $("#listEmails").UifListView("addItem", emailTmp);
        }
        else {
            $("#listEmails").UifListView("editItem", emailRowId, emailTmp);
        }
    }

    static CreateEmailModel() {
        var email = { EmailType: {} };
        email.Id = emailId;
        email.Description = $("#inputEmail").val();
        email.EmailTypeId = $("#selectEmailType").UifSelect("getSelected");
        email.EmailType.Description = $("#selectEmailType").UifSelect("getSelectedText");
        email.IsMailingAddress = $('#chkEmailPrincipal').is(':checked');
        emails.forEach(function (element) { (element.Id == emailId) ? emails.AplicationStaus = 1 : email.AplicationStaus = 0 });
        return email;
    }

    //seccion edit
    static EditEmail(data, index) {
        $("#selectEmailType").prop("disabled", false);
        emailId = data.Id;
        $("#inputEmail").val(data.Description);
        $("#selectEmailType").UifSelect("setSelected", data.EmailTypeId);
        if (data.EmailTypeId == EmailType.ElectronicBilling) {
            $("#selectEmailType").prop("disabled", true);
        }
        $('#chkEmailPrincipal').prop("checked", data.IsMailingAddress);
    }

    static SendEmail() {
        if ($("#listEmails").UifListView('getData').length > 0) {
            emails = $("#listEmails").UifListView('getData');
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmpty, 'autoclose': false })
        }
    }

    static setPrincipalEmail() {
        $.each($("#listEmails").UifListView('getData'), function (i, item) {
            if (item.IsMailingAddress == true || item.IsPrincipal==true) {
                $("#selectEmailTypePn").UifSelect("setSelected", item.EmailTypeId);
                $("#inputEmailPn").val(item.Description);
            }
        });
    }

    static setPrincipalEmailCompany(emails) {
        //$.each($("#listEmails").UifListView('getData'), function (i, item) {
        //    if (item.IsMailingAddress == true) {
        //        $("#selectCompanyEmailType").UifSelect("setSelected", item.EmailTypeId);
        //        $("#inputCompanyEmail").val(item.Description);
        //    }
        //});
        for (var index in emails) {
            if (emails[index].EmailTypeId == EmailType.ElectronicBilling) {
                $('#inputEmailElectronicBilling').val(emails[index].Description);
            } else {
                if (emails[index].IsPrincipal == true || emails[index].IsMailingAddress == true) {
                    $("#selectCompanyEmailType").UifSelect("setSelected", emails[index].EmailTypeId);
                    $("#inputCompanyEmail").val(emails[index].Description);
                }
               
            }
        }
    }

    //Cerrar
    static CloseEmail() {
        $('#modalEmail').UifModal('hide');
    }

    static CleanObjectsEmail() {
        emails = [];
        $("#listEmails").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#emailTemplate", height: heightListViewEmail, title: AppResourcesPerson.LabelEmail });
    }

    static FillObjectEmail() {
        emails.push({
            Id: 0,
            Description: $("#inputEmailPn").val(),
            EmailTypeId: $("#selectEmailTypePn").UifSelect("getSelected"),
            EmailTypeDescription: $("#selectEmailTypePn").UifSelect("getSelectedText"),
            IsPrincipal: true
        });
        emails.push({
            Id: 0,
            Description: $("#inputEmailElectronicBilling").val(),
            EmailTypeId: EmailType.ElectronicBilling
        });
    }

    static FillObjectEmailCompany() {
        emails.push({
            Id: 0,
            Description: $("#inputCompanyEmail").val(),
            EmailTypeId: $("#selectCompanyEmailType").UifSelect("getSelected"),
            IsPrincipal: true,
            EmailType: {
                Id: $("#selectCompanyEmailType").UifSelect("getSelected"),
                Description: $("#selectCompanyEmailType").UifSelect("getSelectedText")
            }
        });
        emails.push({
            Id: 0,
            Description: $("#inputEmailElectronicBilling").val(),
            EmailTypeId: EmailType.ElectronicBilling
        });
    }

    //seccion get
    static GetEmailsses() {
        var tempEmails = [];
        var contRowsEmails = $("#listEmails").UifListView('getData').length;
        if (emails != null && emails.length > 0) {
            let resultEmails = [];
            emails.forEach(function (element) {
                let tempDataEmails = { EmailType: {}, };
                tempDataEmails.EmailType.Description = element.Description;
                tempDataEmails.AplicationStaus = element.AplicationStaus;
                tempDataEmails.EmailType.Id = element.EmailTypeId;
                tempDataEmails.EmailTypeId = element.EmailTypeId;
                tempDataEmails.Id = element.Id;
                tempDataEmails.SmallDescription = element.SmallDescription;
                tempDataEmails.Description = element.Description;
                tempDataEmails.UpdateDate = element.UpdateDate;
                if (element.IsPrincipal == undefined) {
                    tempDataEmails.IsMailingAddress = element.IsMailingAddress;
                    tempDataEmails.IsPrincipal = element.IsMailingAddress;
                }
                else {
                    tempDataEmails.IsMailingAddress = element.IsPrincipal;
                    tempDataEmails.IsPrincipal = element.IsPrincipal;
                }
                
                resultEmails.push(tempDataEmails);
            });

            $("#listEmails").UifListView({ sourceData: resultEmails, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#emailTemplate", height: heightListViewEmail, title: AppResourcesPerson.LabelEmail });
            contRowsEmails = $("#listEmails").UifListView('getData').length;
            if (contRowsEmails > 0) {
                $.each($("#listEmails").UifListView('getData'), function (key, value) {
                    if (this.IsMailingAddress || this.IsPrincipal) {
                        $("#listEmails").UifListView("editItem", key, Email.SetEmails(value.Id));
                    }
                });
            }
            else {
                tempEmails.push(Email.SetEmails(0));
                $("#listEmails").UifListView({ sourceData: tempEmails, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#emailTemplate", height: heightListViewEmail, title: AppResourcesPerson.LabelEmail });
                emails = tempEmails;
            }
        }
        else {
            tempEmails.push(Email.SetEmails(0));
            $("#listEmails").UifListView({ sourceData: tempEmails, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#emailTemplate", height: heightListViewEmail, title: AppResourcesPerson.LabelEmail });
            emails = tempEmails;
        }
    }

    static SetEmails(id) {
        var email = { EmailType: {} };
        email.Id = id;
        email.IsMailingAddress = true;
        switch (searchType) {
            case TypePerson.PersonNatural:
                email.Description = $("#inputEmailPn").val();
                email.EmailTypeId = $("#selectEmailTypePn").UifSelect("getSelected");
                email.EmailType.Description = $("#selectEmailTypePn").UifSelect("getSelectedText");
                break;
            case TypePerson.PersonLegal:
                email.Description = $("#inputCompanyEmail").val();
                email.EmailTypeId = $("#selectCompanyEmailType").UifSelect("getSelected");
                email.EmailType.Description = $("#selectCompanyEmailType").UifSelect("getSelectedText");
                break
        }
        return email;
    }

    static UpdatePrincipalEmail() {
        var existBillingEmail = false;
        var existPrincipalEmail = false;
        $.each(emails, function (i, item) {
            if (item.IsPrincipal == true || item.IsMailingAddress == true) {
                item.EmailTypeId = $("#selectEmailTypePn").UifSelect("getSelected");
                item.Description = $("#inputEmailPn").val();
                existPrincipalEmail = true;
                item.IsPrincipal = true;
            }
            if (item.EmailTypeId != EmailType.ElectronicBilling) {
                item.AplicationStaus = 1;

            } else {
                existBillingEmail = true;
                item.AplicationStaus = 1;
                item.Description = $("#inputEmailElectronicBilling").val();
            }
            if (item.Id == 0) {
                item.AplicationStaus = 0;
            }
        });
        if (!existBillingEmail) {
            emails.push({
                Id: 0,
                Description: $("#inputEmailElectronicBilling").val(),
                EmailTypeId: EmailType.ElectronicBilling
            });
        }
        if (existPrincipalEmail == false) {
            var existEmail = false;
            $.each(emails, function (i, item) {
                var inputCompanyEmail = $("#inputEmailPn").val();
                var selectCompanyEmailType = $("#selectEmailTypePn").UifSelect("getSelected");
                if (item.EmailTypeId == selectCompanyEmailType && item.Description == inputCompanyEmail) {
                    item.IsPrincipal = true;
                    existEmail = true;
                }
            });
            if (existEmail == false) {
                Email.FillObjectEmailCompany();
            }
        }
    }

    static UpdatePrincipalEmailCompany() {
        var existBillingEmail = false;
        var existPrincipalEmail = false;
        $.each(emails, function (i, item) {
            if (item.IsPrincipal == true || item.IsMailingAddress == true) {
                item.EmailTypeId = $("#selectCompanyEmailType").UifSelect("getSelected");
                item.Description = $("#inputCompanyEmail").val();
                item.IsPrincipal = true;
                existPrincipalEmail = true;
            }
            if (item.EmailTypeId != EmailType.ElectronicBilling) {
                item.AplicationStaus = 1;
            } else {
                existBillingEmail = true;
                item.AplicationStaus = 1;
                item.Description = $("#inputEmailElectronicBilling").val();
            }
            if (item.Id == 0) {
                item.AplicationStaus = 0;
            }
        });
        if (!existBillingEmail) {
            emails.push({
                Id: 0,
                Description: $("#inputEmailElectronicBilling").val(),
                EmailTypeId: 23
            });
        }
        if (existPrincipalEmail == false) {
            var existEmail = false;
            $.each(emails, function (i, item) {
                var inputCompanyEmail = $("#inputCompanyEmail").val();
                var selectCompanyEmailType = $("#selectCompanyEmailType").UifSelect("getSelected");
                if (item.EmailTypeId == selectCompanyEmailType && item.Description == inputCompanyEmail) {
                    item.IsPrincipal = true;
                    existEmail = true;
                }
            });
            if (existEmail == false) {
                Email.FillObjectEmailCompany();
            }
            
        }
    }

    //Seccion Load
    static LoadEmailsses(emails) {
        $("#listEmails").UifListView({ sourceData: emails, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#emailTemplate", height: heightListViewEmail, title: AppResourcesPerson.LabelEmail });
    }

    //validaciones
    static ValidateEmailPerson() {
        var error = "";
        switch (searchType) {
            case TypePerson.PersonNatural:
                if ($("#lblPersonCode").val() == "") {
                    error = error + AppResourcesPerson.ErrorPersonExists + "<br>";
                }

                if ($("#selectEmailTypePn").UifSelect("getSelected") == "") {
                    error = error + AppResourcesPerson.LabelEmailType + "<br>";
                }
                if ($("#inputEmailPn").val() == "") {
                    error = error + AppResourcesPerson.Email + "<br>";

                }
                if (!ValidateEmail($("#inputEmailPn").val())) {
                    error = error + AppResourcesPerson.ErrorEmail + "<br>";
                }
                break;
            case TypePerson.PersonLegal:
                if ($("#lblCompanyCode").val() == "") {
                    error = error + AppResourcesPerson.ErrorCompanyExists + "<br>";
                }
                if ($("#selectCompanyEmailType").val() == "") {
                    error = error + AppResourcesPerson.LabelEmailType + "<br>";
                }
                if ($("#inputCompanyEmail").val() == "") {
                    error = error + AppResourcesPerson.Email + "<br>";
                }
                if (!ValidateEmail($("#inputCompanyEmail").val())) {
                    error = error + AppResourcesPerson.ErrorEmail + "<br>";
                }
                break;
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }

    static PrincipalEmail() {
        var principal = false;
        var isMain = $("#chkEmailPrincipal").is(":checked");
        if (isMain) {
            $.each($("#listEmails").UifListView('getData'), function (i, item) {
                if (item.IsMailingAddress == isMain && i != emailRowId) {
                    principal = true;
                    return false;
                }
            });
        }
        return principal;
    }

    static DuplicateEmail(email, typeEmail) {
        var duplicate = false;
        $.each($("#listEmails").UifListView('getData'), function (i, item) {
            if ($.trim(item.Description) == $.trim(email) && this.EmailTypeId == typeEmail && i != emailRowId) {
                duplicate = true;
                return false;
            }
        });

        return duplicate;
    }

    static ConvertAddressDtoToModel(emailDTO) {
        var emailModel = Email.CreateEmailModel();
        var rslt = [];
        if (emailDTO.Emails != null && emailDTO.Emails.length > 0) {
            for (var index = 0; index < emailDTO.Emails.length; index++) {
                emailModel.Id = 1; //emailDTO.Emails[index].Id;
                emailModel.Description = emailDTO.Emails[index].Description;
                emailModel.EmailTypeId = emailDTO.Emails[index].EmailTypeId;
                emailModel.EmailType.Description = emailDTO.Emails[index].Description;
                emailModel.IsMailingAddress = true; //emailDTO.Emails[index].isPrincipal;

                rslt.push(emailModel);
            }
        }
        return rslt;
    }

    static ConvertAddressModelToDTO(emailDTO) {
    }
}







