
class AccessStatusType {
    static GetStatus() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/Access/GetStatus',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class AccessAdvSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'UniqueUser/Access/AccessAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 500,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {
     
        this.componentLoadedCallback();
    }

    static getSubModule() {
        AccessObject.GetSubModule($("#searchModule").val()).done(function (data) {
            if (data.success) {
                $("#searchSubModule").UifSelect({ sourceData: data.result, native: false, filter: true });
            }
        });
    }
    cancelSearch() {
        $("#listViewSearchAdvanced").UifListView("refresh");
        AccessAdvSearch.ClearAdvanced();
        dropDownSearch.hide();
    }
    static ClearAdvanced() {
        $("#searchModule").UifSelect("setSelected", null);
        $("#searchSubModule").UifSelect();        
        $("#searchEnabledType").UifSelect("setSelected", null);
        $("#searchAccessType").UifSelect("setSelected", null);
    }
    componentLoadedCallback() {
        $('#searchModule').on('itemSelected', AccessAdvSearch.getSubModule);
        $('#btnAdvancedSearch').on("click", AccessAdvSearch.AdvancedQuery);    
        $("#btnCancel").on('click', function () {
            dropDownSearch.hide();
        })

        AccessStatusType.GetStatus().done(function (data) {
            if (data.success) {
                $('#searchEnabledType').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        AccessType.GetAccessType().done(function (data) {
            if (data.success) {
                $('#searchAccessType').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        Modules.GetEnabledModules().done(function (data) {
            if (data.success) {
                $('#searchModule').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        $("#listViewSearchAdvanced").UifListView({
            displayTemplate: "#advancedSearchTemplate",
            selectionType: 'single',
            source: null,
            height: 220
        });
        $("#btnShowAdvanced").on("click", function () {
            $("#listViewSearchAdvanced").UifListView("refresh");
            AccessAdvSearch.ClearAdvanced();
            dropDownSearch.show();
        });

        $("#btnLoad").on("click", function () {            
            AccessAdvSearch.ClearAdvanced();
            dropDownSearch.hide();
            var accsesSelected = $("#listViewSearchAdvanced").UifListView("getSelected");
            if (accsesSelected != "") {
                Access.getAccess(accsesSelected[0].Id, 0);
            }
        });
    }
    static AdvancedQuery() {
        $("#listViewSearchAdvanced").UifListView("refresh");
        var accessAdvancedSearch = {
            SubModule: { Id: $("#searchSubModule").val(), Module: { Id: $("#searchModule").val() } },
            Status: $("#searchEnabledType").val(),
            ObjectTypeId: $("#searchAccessType").val()
        };

        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/Access/GetAccessAdvancedSearch',
            data: JSON.stringify({ access: accessAdvancedSearch }),
            datatype: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            AccessAdvSearch.LoadAdvanced(data);
        }).error(function (data) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchAccess, 'autoclose': true });
        });
    }
    static LoadAdvanced(access) {
        $("#listViewSearchAdvanced").UifListView("refresh");
        $.each(access, function (index, val) {
            $("#listViewSearchAdvanced").UifListView("addItem", access[index]);
        });
    }


}





