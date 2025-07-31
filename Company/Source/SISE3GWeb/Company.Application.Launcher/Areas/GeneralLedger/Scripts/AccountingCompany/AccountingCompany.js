var accountingCompanyRowIndex = 0; //variable definida.

$(() => {
    new AccountingCompany();
});

class Company {
    constructor() {
        this.AccountingCompanyId = 0;
        this.Description = "";
        this.Default = false;
    }

    static SaveAccountingCompany(company) {
        return $.ajax({
            type: 'POST',
            url: GL_ROOT + "AccountingCompany/SaveAccountingCompany",
            data: JSON.stringify({ accountingCompanyModel: company }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static DeleteAccountingCompany(id) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: 'POST',
                url: GL_ROOT + "AccountingCompany/DeleteAccountingCompany",
                data: JSON.stringify({ id: id }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (deleteResult) {
                    resolve(deleteResult);
                }
            });
        });
    }
}

class AccountingCompany extends Uif2.Page {
    getInitialState() {
        $("#listViewAccountingCompany").UifListView(
            {
                source: "GetAccountingCompanies",
                customDelete: true,
                customAdd: true,
                customEdit: true,
                add: true,
                edit: true,
                delete: true,
                displayTemplate: "#display-template"
            });
    }

    bindEvents() {
        $('#listViewAccountingCompany').on("rowAdd", this.addRow);
        $('#listViewAccountingCompany').on("rowEdit", this.editRow);
        $('#listViewAccountingCompany').on("rowDelete", this.deleteRow);
        $("#btnDeleteModal").click(this.deleteCompanyRecord);
        $("#modalAccountingCompany").on('click', '#AccountingCompanySaveButton', this.saveRecord);
    }

    addRow() {
        $('#modalAccountingCompany').UifModal('show', GL_ROOT + 'AccountingCompany/AccountingCompanyModal', Resources.AddRecord);
    }

    editRow(event, data) {
        $('#modalAccountingCompany').UifModal('show', GL_ROOT + 'AccountingCompany/AccountingCompanyModal?id=' + data.AccountingCompanyId, Resources.EditRecord);
    }

    deleteRow(event, data) {
        $('#modalDelete').modal('show');
        accountingCompanyRowIndex = data.AccountingCompanyId;
    }

    saveRecord() {
        if ($('#modalAccountingCompany').find("#addAccountingCompany").valid()) {
            var company = new Company();
            company.AccountingCompanyId = $('#modalAccountingCompany').find('#AccountingCompanyId').val();
            company.Description = $('#modalAccountingCompany').find("#Description").val();
            company.Default = ($('#modalAccountingCompany').find('#Default').is(':checked'));

            Company.SaveAccountingCompany(company).then(function (data) {
                if (data == false) {
                    $('#modalAccountingCompany').find("#alertAccountingCompanyModel").UifAlert('show', Resources.AlreadyDefaultCompany, "danger");
                } else {
                    $('#modalAccountingCompany').UifModal('hide');
                    $("#listViewAccountingCompany").UifListView("refresh");

                    if (company.AccountingCompanyId == 0) {
                        $("#alertAccountingCompany").UifAlert('show', Resources.AddSuccessfully, "success");
                    } else {
                        $("#alertAccountingCompany").UifAlert('show', Resources.EditSuccessfully, "success");
                    }
                }
            });
        }
    }

    deleteCompanyRecord() {
        $('#modalDelete').modal('hide');
        Company.DeleteAccountingCompany(accountingCompanyRowIndex).then(function (deleteResult) {

            if (deleteResult == -1) {
                $("#alertAccountingCompany").UifAlert('show', Resources.AccountingCompanyUsed, "danger");
            }
            else if (deleteResult == false) {
                $("#alertAccountingCompany").UifAlert('show', Resources.MessageDeleteCompany, "danger");
            }
            else if (deleteResult == 0) {
                $("#alertAccountingCompany").UifAlert('show', Resources.ErrorDeletingRecord, "danger");
            }
            else {
                $("#alertAccountingCompany").UifAlert('show', Resources.DeleteSuccessfully, "success");
                $("#listViewAccountingCompany").UifListView("refresh");
            }
        });
    }
}