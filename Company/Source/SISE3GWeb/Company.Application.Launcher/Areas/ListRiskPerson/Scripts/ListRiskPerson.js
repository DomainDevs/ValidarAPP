var dropDownSearchAdvanced = null;
var panelIsVisible = false;
var glbPerson = {};
var isEditing = -1;
var lastFilter = [];
class ListRiskPerson extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvanced = uif2.dropDown({
            source: rootPath + 'ListRiskPerson/ListRiskPerson/AdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 550,
            height: 200,
            loadedCallback: this.AdvancedSearchEvents
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#inputDocumentNumber').ValidatorKey(1, 0, 0);
        $('#inputName').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        $('#inputLastName').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        $('#inputNickName').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
        ListRiskPerson.GetListRiskPersons();
        $.fn.UifDataTable.exporttoexcel = true;
    }

    bindEvents() {
        $('#btnShowAdvanced').click(ListRiskPerson.showAdvanceSearchPane);
        $('#btnFindSearchAdv').click(this.AdvancedSearchFind);
        $('#btnAddNewPersonSave').click(ListRiskPerson.AddListRiskperson);
        $('#inputDocumentSearch').on('buttonClick', ListRiskPerson.FindPersonByDocumentNumber);
        $('#btnExit').on('click', ListRiskPerson.Exit);
        $('#inputBirthDate').on('datepicker.change', ListRiskPerson.CalculateBirthDay);
        $('#inputBirthDate').on('blur', () => { if ($('#inputBirthDate').val() == "") $('#inputAge').val(""); });
        $('#tblListRiskPerson').on('rowEdit', function (event, data, position) {
            ListRiskPerson.EditListRiskPerson(data, position);
        });
        $('#tblRiskList').on('rowAdd', function (event, data, position) {
            glbPerson = {};
            isEditing = -1;
            ListRiskPerson.ViewAddModal();
        });

        $('#btnExportList').click(ListRiskPerson.xportToExcel);

        $('#tblRiskList').on('rowEdit', function (event, data, position) {
            if (data != null) {
                glbPerson = data;
                isEditing = position;
                ListRiskPerson.setViewModelToView(data);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorGetListRiskDataPerson, 'autoclose': true });
            }
        });

        $("#selectDocumentType").on("itemSelected", () => {
            $("#inputDocumentNumber").val("");
            let selected = $("#selectDocumentType").UifSelect("getSelectedSource");
            if (selected.IsAlphanumeric) {
                $('#inputDocumentNumber').off('keypress').ValidatorKey(ValidatorType.Onlylettersandnumbers, 0, 0);
            } else {
                $("#inputDocumentNumber").off('keypress').OnlyDecimals(0);
            }
        });
    }


    static BirthdatePn(dateIn) {
        var date = dateIn.split(DateSplit);
        if (!(isExpirationDate(date))) {
            var age = Shared.calculateAge("", dateIn);
            if (age > 117) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResourcesPerson.WarningAgeMax });
                return "";
            }
            return age;
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.NoCanDateBirthGreaterCurrent + "<br>" });
            return "";
        }
    }

    static setDataToTable(data) {
        data = data.map(x => {
            x.AssignmentDate = FormatDate(x.AssignmentDate);
            x.LastChangeDate = FormatDate(x.LastChangeDate);
            x.IssueDate = FormatDate(x.IssueDate);
            if (x.BirthDate == null) {
                x.Age = "";
            } else {
                x.BirthDate = FormatDate(x.BirthDate)
                x.Age = Shared.calculateAge("", x.BirthDate);
            }

            return x;
        });
        $('#tblRiskList').UifDataTable('clear');
        $('#tblRiskList').UifDataTable({ sourceData: data });
    }

    static async GetListRiskPersons() {
        lockScreen();
        try {
            let result = await ListRiskPersonRequest.GetAssignedListMantenance();
            if (result.success) {
                ListRiskPerson.setDataToTable(result.result);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': result.result, 'autoclose': true });
            }
        } finally {
            unlockScreen();
        }
    }

    static CalculateBirthDay(e, data) {
        let date = $('#inputBirthDate').val();
        $('#inputAge').val(ListRiskPerson.BirthdatePn(date));
    }

    static async FindPersonByDocumentNumber() {
        var documentNumber = $('#inputDocumentSearch').val();
        lockScreen();
        try {
            let data = await ListRiskPersonRequest.GetAssignedListMantenance(documentNumber, null);
            if (data.success) {
                ListRiskPerson.setDataToTable(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorDataNotFound, 'autoclose': true });
            }
        } finally {
            unlockScreen();
        }
    }

    static xportToExcel() {
        var rowCount = $("#tblListRiskPerson").UifDataTable('getData');
        if (rowCount.length > 0) {
            $('.panel-actions button:nth-child(2)').click();
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorExportDataCount, 'autoclose': true });
        }

    }

    static EditListRiskPerson(data, position) {
        if (data != null) {
            isEditing = position;
            ListRiskPerson.setViewModelToView(data);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorGetListRiskDataPerson, 'autoclose': true });
        }
    }

    static async setViewModelToView(data) {
        $('#modalAddPerson').UifModal('showLocal', Resources.Language.LabelNewPerson);
        glbPerson = data;
        await ListRiskPerson.GetDocumentType(data.DocumentType.Id);
        await ListRiskPerson.GetListRisk(data.ListRisk.Id);
        $('#inputDocumentNumber').val(data.IdCardNo);
        $('#inputName').val(data.Name);
        $('#inputLastName').val(data.LastName);
        $('#inputNickName').val(data.AliasName);
        $('#inputBirthDate').val(data.BirthDate);
        $('#inputAge').val(data.Age);
        if (data.Event === RiskListEventEnum.EXCLUDED) {
            $('#checkExcludedPerson').prop('checked', true);
        } else {
            $('#checkExcludedPerson').attr('checked', false);
        }

        var enabled = !(data.ListRisk.Id != 2 && data.ListRisk.Id != 3);

        $('#inputDocumentNumber').attr('disabled', true);
        $('#inputName').attr('disabled', enabled);
        $('#inputLastName').attr('disabled', enabled);
        $('#inputNickName').attr('disabled', enabled);
        $('#inputAge').attr('disabled', true);
        $("#inputBirthDate").attr('disabled', enabled);
        $('#selectDocumentType').attr('disabled', true);
        $('#selectListRisk').attr('disabled', true);
    }

    static ResultTotable() {
        var searchResult = $("#listViewResult").UifListView('getData');
        if (searchResult != null && searchResult.length > 0) {
            $("#tblListRiskPerson").UifDataTable('clear');
            $("#tblListRiskPerson").UifDataTable({ sourceData: searchResult });
            ListRiskPerson.showAdvanceSearchPane();

        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ListRiskSearchEmpty, 'autoclose': true });
        }

    }

    static SaveListRiskPerson() {
        $('#formNewListPerson').validate();
        if ($('#formNewListPerson').valid()) {
            glbPerson.Event = ($("#checkExcludedPerson").prop('checked')) ? RiskListEventEnum.EXCLUDED : RiskListEventEnum.INCLUDED;
            ListRiskPersonRequest.CreateListRiskPerson(glbPerson).done(function (data) {
                if (data.success) {
                    var result = data.result;
                    var item = {
                        DocumentType: result.DocumentType.Id,
                        IdCardNo: result.IdCardNo,
                        Name: result.Name,
                        ListRisk: result.ListRisk.Id,
                        ListRiskDescription: result.ListRisk.Description,
                        AssignmentDate: FormatDate(result.AssignmentDate),
                        LastChangeDate: FormatDate(result.LastChangeDate),
                        ExcludePerson: result.ExcludePerson
                    };
                    if (isEditing == -1) {
                        $("#tblListRiskPerson").UifDataTable('addRow', item);
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ListRiskPersonSaveOk, 'autoclose': true });
                    } else {
                        $("#tblListRiskPerson").UifDataTable('editRow', item, isEditing)
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.ListRiskPersonUpdateOk, 'autoclose': true });
                        isEditing = -1;
                    }

                    $('#modalAddPerson').UifModal('hide');
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }

            });

        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.AdvancedListRiskValidateForm, 'autoclose': true });
        }
    }

    static async AddListRiskperson() {
        if ($("#formNewListPerson").valid()) {
            var listRiskperson = ListRiskPerson.GetDataModel();
            lockScreen();
            try {
                let data = await ListRiskPersonRequest.CreateListRiskPerson(listRiskperson, lastFilter[0], lastFilter[1]);
                if (data.success) {
                    ListRiskPerson.setDataToTable(data.result);
                    $.UifNotify('show', { 'type': 'info', 'message': "La persona se guardo correctamente", 'autoclose': true });
                }
            } finally {
                unlockScreen();
            }
        }
    }

    static GetDataModel() {
        if ($("#selectDocumentType").UifSelect("getSelectedSource")) {
            glbPerson.DocumentType = {
                Id: $("#selectDocumentType").UifSelect("getSelectedSource").Id,
                Description: $("#selectDocumentType").UifSelect("getSelectedSource").Description
            };
        }

        glbPerson.IdCardNo = $('#inputDocumentNumber').val();
        glbPerson.Name = $('#inputName').val();
        glbPerson.LastName = $('#inputLastName').val();
        glbPerson.BirthDate = FormatDate($("#inputBirthDate").val());
        glbPerson.AliasName = $('#inputNickName').val();;
        glbPerson.ListRisk = { Id: $('#selectListRisk').val(), Description: $('#selectListRisk option:selected').text() }

        if ($('#checkExcludedPerson').prop('checked')) {
            glbPerson.Event = RiskListEventEnum.EXCLUDED;
            glbPerson.IsEnabled = RiskListEventEnum.EXCLUDED;
        } else {
            glbPerson.Event = RiskListEventEnum.INCLUDED;
            glbPerson.IsEnabled = RiskListEventEnum.INCLUDED;
        }


        return glbPerson;
    }

    static Exit() {
        window.location = rootPath + "Home/Index";
    }

    static async GetDocumentType(selectedId) {
        let data = await ListRiskPersonRequest.GetDocumentType()
        if (data.success) {
            if (data.result != null) {
                if (selectedId > 0) {
                    $("#selectDocumentType").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
                else {
                    $('#selectDocumentType').UifSelect({ sourceData: data.result });
                }
            }
        }
    }

    static async GetListRisk(selectedId, action) {
        let data = await ListRiskPersonRequest.GetListRisk();
        if (data.success) {
            if (data.result != null && action == null) {
                if (selectedId == 3 || selectedId == 2) {
                    $("#selectListRisk").UifSelect({ sourceData: data.result, selectedId: selectedId });
                }
                else {
                    if (selectedId == 0) {
                        $('#selectListRisk').UifSelect({ sourceData: data.result.filter(x => x.Id != 3 && x.Id != 2) });
                    } else {
                        $('#selectListRisk').UifSelect({ sourceData: data.result.filter(x => x.Id != 3 && x.Id != 2), selectedId: selectedId });
                    }
                }
            } else if (data.result != null && action != null) {
                $('#selectListRiskAdv').UifSelect({ sourceData: data.result });
            }
        }
    }

    static ClearAdvancedControls() {
        $('#inputDocumentNumberAdv').val("");
        $('#inputNameAdv').val("");
        $('#inputSurnameAdv').val("");
        $('#inputNickNameAdv').val("");
        $('#selectListRiskAdv').UifSelect("setSelected", null);
    }


    static ClearControls() {
        $('#inputDocumentNumber').val("");
        $('#inputDocumentNumber').attr('disabled', false);
        $('#inputName').val("");
        $('#inputName').attr('disabled', false);
        $('#inputLastName').val("");
        $('#inputLastName').attr('disabled', false);
        $('#checkExcludedPerson').attr('checked', false);
    }

    static async AdvancedSearchFind() {
        $('#formListRiskPersonSerachAdvanced').validate();
        if ($('#formListRiskPersonSerachAdvanced').valid()) {
            var advanceSearchModel = $('#formListRiskPersonSerachAdvanced').serializeObject();
            lastFilter.push(advanceSearchModel.DocumentNumber);
            lastFilter.push(advanceSearchModel.ListRisk);

            lockScreen();
            try {
                var data = await ListRiskPersonRequest.GetAssignedListMantenance(advanceSearchModel.DocumentNumber, advanceSearchModel.ListRisk);
                if (data.success) {
                    ListRiskPerson.setDataToTable(data.result);
                }
            }
            finally {
                lockScreen();
            }
        }
    }

    static async ViewAddModal() {
        $('#modalAddPerson').UifModal('showLocal', Resources.Language.LabelNewPerson);

        await ListRiskPerson.GetDocumentType(0);
        await ListRiskPerson.GetListRisk(0);
        $('#inputDocumentNumber').val("");
        $('#inputName').val("");
        $('#inputLastName').val("");
        $('#inputNickName').val("");
        $('#inputAge').val("");
        $("#inputBirthDate").UifDatepicker('clear');
        $("#checkExcludedPerson").prop('checked', false);

        $('#inputDocumentNumber').attr('disabled', false);
        $('#inputName').attr('disabled', false);
        $('#inputLastName').attr('disabled', false);
        $('#inputNickName').attr('disabled', false);
        $('#inputAge').attr('disabled', true);
        $("#inputBirthDate").attr('disabled', false);
        $('#selectDocumentType').attr('disabled', false);
        $('#selectListRisk').attr('disabled', false);
    }

    AdvancedSearchEvents() {
        ListRiskPerson.GetListRisk(0, "Advance");
        ListRiskPerson.ClearAdvancedControls();
        $('#inputDocumentNumberAdv').ValidatorKey(1, 1, 1);
        $('#inputNameAdv').ValidatorKey(2, 1, 1);
        $('#inputNickNameAdv').ValidatorKey(7, 1, 1);
        $('#btnFindSearchAdv').on("click", ListRiskPerson.AdvancedSearchFind);
        $('#btnCloseSearchView').on("click", ListRiskPerson.showAdvanceSearchPane)
        $('#btnOkSearchAdv').on("click", ListRiskPerson.ResultTotable);
    }

    static showAdvanceSearchPane() {
        if (!panelIsVisible) {
            dropDownSearchAdvanced.show();
        } else {
            dropDownSearchAdvanced.hide();
            ListRiskPerson.ClearAdvancedControls();
        }
        panelIsVisible = !panelIsVisible;
    }
}