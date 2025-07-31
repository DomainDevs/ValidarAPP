var AccountingCompanySaveCallback = function (deferred, data) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/SaveAccountingCompany",
        data: JSON.stringify({ "companyDescription": data.Description }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            deferred.resolve();
            $("#alertAccountingCompany").UifAlert('show', Resources.SaveSuccessfully, "success");
        } else {
            deferred.reject();
            $("#alertAccountingCompany").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};

var AccountingCompanyEditCallback = function (deferred, data) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/UpdateAccountingCompany",
        data: JSON.stringify({ "cd": data.Id, "description": data.Description }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#AccountingCompanyList").UifListView("refresh");
            deferred.resolve(data.result);
            $("#alertAccountingCompany").UifAlert('show', Resources.EditSuccessfully, "success");
        } else {
            deferred.reject();
            $("#alertAccountingCompany").UifAlert('show', Resources.SaveError, "danger");
        }
    });
};

var AccountingCompanyDeleteCallback = function (deferred, data) {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Parameters/DeleteAccountingCompany",
        data: JSON.stringify({ "frmAccountingCompany": data.Id }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        deferred.resolve();
        $("#alertAccountingCompany").UifAlert('show', Resources.DeleteSuccessfully, "success");

    });
};


setTimeout(function () {
   $("#AccountingCompanyList").UifListView({
        source: ACC_ROOT + "Parameters/GetAccountingCompanies",
        customDelete: false,
        customAdd: false,
        customEdit: false,
        add: true,
        edit: true,
        delete: true,
        displayTemplate: "#AccountingCompanyListTemplate",
        addTemplate: '#add-template',
        addCallback: AccountingCompanySaveCallback,
        editCallback: AccountingCompanyEditCallback,
        deleteCallback: AccountingCompanyDeleteCallback
    });
}, 500);