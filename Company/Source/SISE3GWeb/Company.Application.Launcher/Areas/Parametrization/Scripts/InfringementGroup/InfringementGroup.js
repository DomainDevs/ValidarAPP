var groupIndex = null;
var groupStatus = null;
var groupCode = null;
var glbInfringementGroupsCreate = [];
var dropDownSearchAdvInfringementGroup = null;
var InfringementAll = [];

$(() => {
    new InfringementGroup();
});

class InfringementGroup extends Uif2.Page {
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        InfringementGroup.GetAllInfringementGroup().done(function (data) {
            if (data.success) {
                InfringementGroup.LoadListViewInfringementGroup(data.result);
            }
        });

        InfringementGroup.GetAllInfrigement().done(function (data) {
            if (data.success) {
                InfringementAll = data.result;
                    //Infringement.LoadListViewInfrigement(data.result);
            }
        });

        dropDownSearchAdvInfringementGroup = uif2.dropDown({
            source: rootPath + 'Parametrization/InfringementGroup/InfringementGroupSearch',
            element: '#inputInfringementGroup',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: function () { }
		});

		$("#inputInfrigementOneYear").ValidatorKey(ValidatorType.Number, 2, 0);
		$("#inputInfrigementThreeYear").ValidatorKey(ValidatorType.Number, 2, 0);
    }

    bindEvents() {
        $('#btnNewInfringementGroup').click(InfringementGroup.Clear);
        $('#btnAddInfringementGroup').click(this.AddInfringementGroup);
        $('#listViewInfringementGroup').on('rowEdit', InfringementGroup.ShowData);
        $('#btnExitInfringementGroup').click(this.Exit);
        $('#btnExportInfringementGroup').click(this.ExportFile);
        $('#btnSaveInfringementGroup').click(this.SaveInfringementGroups);
        $('#inputInfringementGroup').on('buttonClick', this.SearchInfringementGroup);
    }

    static GetAllInfrigement() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Infringement/GetInfringement',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    AddInfringementGroup() {
        $("#formInfringementGroup").validate();
        if ($("#formInfringementGroup").valid()) {
            var group = InfringementGroup.GetForm();

            var StatusOld = !group.IsActive;
       
            
            var ifExist = InfringementAll.filter(function (item) {
                return item.InfringementGroupCode == group.InfringementGroupCode
            });
            if (ifExist.length > 0 && !$("#chkIsActive").is(":checked")) {
                
                $('#chkIsActive').prop("checked", StatusOld);
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorMessageAssociatedInfractions, 'autoclose': true });
            }
            else {
                if (group.IsActive) {
                    group.LabelGroupEnabled = Resources.Language.Enabled;
                }
                else {
                    group.LabelGroupEnabled = Resources.Language.Disabled;
                }
                if (groupIndex == null) {
                    var lista = $("#listViewInfringementGroup").UifListView('getData');
                    var ifExist = lista.filter(function (item) {
                        return item.Description.toUpperCase() == group.Description.toUpperCase();
                    });
                    if (ifExist.length > 0 && groupStatus != ParametrizationStatus.Create) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistInfringementGroup, 'autoclose': true });
                    }
                    else {
                        InfringementGroup.SetStatus(group, ParametrizationStatus.Create);
                        $("#listViewInfringementGroup").UifListView("addItem", group);
                    }
                }
                else {
                    if (groupIndex != undefined && groupStatus != undefined) {
                        InfringementGroup.SetStatus(group, groupStatus);
                    } else {
                        InfringementGroup.SetStatus(group, ParametrizationStatus.Update);
                    }
                    $('#listViewInfringementGroup').UifListView('editItem', groupIndex, group);
                }
                InfringementGroup.Clear();
            }
        }
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    SaveInfringementGroups() {
        InfringementGroup.SetListToSend();
        $.ajax({
            type: "POST",
            url: 'SaveInfringementGroups',
            data: JSON.stringify({ lstInfringementGroups: glbInfringementGroupsCreate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            glbInfringementGroupsCreate = [];
            var error = data.result.Message == null ? 0 : data.result.Message;
                $.UifNotify('show', {
                    'type': 'info', 'message': Resources.Language.MessageUpdate + ': <br>' +
                    Resources.Language.Aggregates + ': ' + data.result.TotalAdded + '<br> ' +
                    Resources.Language.Updated + ': ' + data.result.TotalModified + '<br> ' +
                    Resources.Language.Removed + ': ' + data.result.TotalDeleted + '<br> ' +
                    Resources.Language.Errors + ': ' + error,
                    'autoclose': true
                });
                InfringementGroup.GetAllInfringementGroup().done(function (data) {
                    if (data.success) {
                        InfringementGroup.LoadListViewInfringementGroup(data.result);
                    }
                });
        });
    }

    SearchInfringementGroup() {
        var inputInfringementGroup = $('#inputInfringementGroup').val();
        if (inputInfringementGroup.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            $.ajax({
                type: "POST",
                url: 'GetInfringementGroupsByDescription',
                data: JSON.stringify({ description: inputInfringementGroup }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    var groups = data.result;
                    if (data.result.length > 1) {
                        InfringementGroup.ShowSearchAdv(groups);
                    }
                    else {
                        $.each(groups, function (key, value) {
                            var lista = $("#listViewInfringementGroup").UifListView('getData')
                            var index = lista.findIndex(function (item) {
                                return item.InfringementGroupCode == value.InfringementGroupCode;
                            });
                            InfringementGroup.ShowData(null, value, index);
                        });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result.ErrorDescription, 'autoclose': true });
                }
            });
        }
        $('#inputInfringementGroup').val('');
    }

    ExportFile() {
        InfringementGroup.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', {
                    'type': 'info', 'message': data.result, 'autoclose': true
                });
            }
        });
    }

    static GetAllInfringementGroup()
    {
        $("#listViewInfringementGroup").UifListView({ displayTemplate: "#InfringementGroupTemplate", edit: true, delete: false, customAdd: true, customEdit: true, height: 300 });
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/InfringementGroup/GetInfringementGroup',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static LoadListViewInfringementGroup(data) {
        $("#listViewInfringementGroup").UifListView({ displayTemplate: "#InfringementGroupTemplate", edit: true, delete: false, customAdd: true, customEdit: true, height: 300 });
        $.each(data, function (key, value) {
            var group =
                {
                    InfringementGroupCode: this.InfringementGroupCode,
                    Description: this.Description,
                    InfrigementOneYear: this.InfrigementOneYear,
                    InfrigementThreeYear: this.InfrigementThreeYear,
                    IsActive: this.IsActive
                };
            if (group.IsActive) {
                group.LabelGroupEnabled = Resources.Language.Enabled;
            }
            else {
                group.LabelGroupEnabled = Resources.Language.Disabled;
            }
            $("#listViewInfringementGroup").UifListView("addItem", group);
        })
    }

    static Clear() {
        $("#chkIsActive").prop("checked", "");
        $("#inputInfringementGroupDescription").val("");
        $("#inputInfringementGroupDescription").focus();
        $("#inputInfrigementOneYear").val("");
        $("#inputInfrigementThreeYear").val("");
        groupIndex = null;
        groupStatus = null;
        groupCode = null;
    }

    static GetForm() {
        var data = {
        };
        $("#formInfringementGroup").serializeArray().map(function (x) {
            data[x.name] = x.value;
        });
        data.InfringementGroupCode = groupCode;
        data.Description = $("#inputInfringementGroupDescription").val();
        data.IsActive = $("#chkIsActive").is(":checked");
        data.InfrigementOneYear = $("#inputInfrigementOneYear").val();
        data.InfrigementThreeYear = $("#inputInfrigementThreeYear").val();
        return data;
    }

    static SetStatus(object, status) {
        object.StatusTypeService = status;
    }

    static ShowSearchAdv(data) {
        $("#lvSearchAdvInfringementGroup").UifListView({
            displayTemplate: "#InfringementGroupTemplate",
            selectionType: "single",
            height: 300
        });
        $("#btnCancelSearchAdv").on("click", InfringementGroup.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", InfringementGroup.OkSearchAdv);
        $("#lvSearchAdvInfringementGroup").UifListView("clear");
        if (data) {
            data.forEach(item => {
                var group =
                    {
                        InfringementGroupCode: item.InfringementGroupCode,
                        Description: item.Description,
                        InfrigementOneYear: item.InfrigementOneYear,
                        InfrigementThreeYear: item.InfrigementThreeYear,
                        IsActive: item.IsActive
                    };
                if (group.IsActive) {
                    group.LabelGroupEnabled = Resources.Language.Enabled;
                }
                else {
                    group.LabelGroupEnabled = Resources.Language.Disabled;
                }
                $("#lvSearchAdvInfringementGroup").UifListView("addItem", group);
            });
        }
        dropDownSearchAdvInfringementGroup.show();
    }

    static ShowData(event, result, index) {
        InfringementGroup.Clear();
        groupIndex = index;
        groupStatus = result.StatusTypeService;
        groupCode = result.InfringementGroupCode;
        $("#inputInfringementGroupDescription").val(result.Description);
        $("#inputInfrigementOneYear").val(result.InfrigementOneYear);
        $("#inputInfrigementThreeYear").val(result.InfrigementThreeYear);
        if (result.IsActive) {
            $('#chkIsActive').prop("checked", true);
        }
    }

    static SetListToSend() {
        var groupsData = $('#listViewInfringementGroup').UifListView('getData');
        $.each(groupsData, function (key, value) {
            /// Validar que no se agregue más de una vez, si doy guardar varias veces
            if (value.StatusTypeService != undefined) {
                glbInfringementGroupsCreate.push(value);
            }
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static CancelSearchAdv() {
        dropDownSearchAdvInfringementGroup.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvInfringementGroup").UifListView("getSelected");
        if (data.length == 1) {
            var group =
                {
                    InfringementGroupCode: data[0].InfringementGroupCode,
                    Description: data[0].Description,
                    InfrigementOneYear: data[0].InfrigementOneYear,
                    InfrigementThreeYear: data[0].InfrigementThreeYear,
                    IsActive: data[0].IsActive
                };
            var lista = $("#listViewInfringementGroup").UifListView('getData');
            var index = lista.findIndex(function (item) {
                return item.InfringementGroupCode == group.InfringementGroupCode;
            });
            InfringementGroup.ShowData(null, group, index);
        }
        dropDownSearchAdvInfringementGroup.hide();
    }
}