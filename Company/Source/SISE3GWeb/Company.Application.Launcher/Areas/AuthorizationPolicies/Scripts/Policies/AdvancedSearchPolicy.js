var dropDownSearch;

class AdvancedSearchPolicy extends Uif2.Page {
    getInitialState() {
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'AuthorizationPolicies/Policies/AdvancedSearch',
            element: '#btnSearchAdvPolicies',
            align: 'right',
            width: 500,
            height: 300,
            loadedCallback: this.AdvSearchEvents
        });
        RequestGroupsPolicies.GetGroupsPolicies().done((data) => {
            if (data.success) {
                $("#ddlAvdGroupPolicies").UifSelect({
                    sourceData: data.result
                });
            }
        });
        RequestTypePolicies.GetTypePolicies().done((data) => {
            if (data.success) {
                $("#ddlAvdTypePolicies").UifSelect({
                    sourceData: data.result
                });
            }
        });
        RequestLevels.GetLevelsByIdPackage(10).done((data) => {
            if (data.success) {
                $("#ddlLevels").UifSelect({
                    sourceData: data.result
                });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': data.result, 'autoclose': true });
            }
        });
       
        $("#ChkEnabled").prop("checked", true);
        $("#ddlAvdTypePolicies").data('Object', null);
        $('#ddlAvdTypePolicies').val('');
        $("#ddlLevels").data('Object', null);
        $('#ddlLevels').val('');
        
    }

    bindEvents() {
        $("#btnSearchAdvPolicies").click(() => { this.ShowAdvancedSearch(); });
        $("#ddlAvdGroupPolicies").on("itemSelected", this.ChangeGroupPolicies);
        $("#ddlAvdTypePolicies").on("itemSelected", this.ChangeTypePolicies);
        $("#ddlLevels").on("itemSelected", this.ChangeLevels);
        $("#btnSearchPolicyAvd").click(() => { this.AvdSearchPolicy(); });
        $("#btnCancelSearchPolicyAvd").click(() => { this.AvdSearchPolicyCancel(); });
    }

    ShowAdvancedSearch() {
        dropDownSearch.show();
    }
    AdvSearchEvents() {

    }
    ChangeGroupPolicies() {
        
    }
    ChangeTypePolicies() { }
    ChangeLevels() { }

    AvdSearchPolicy() {
        
        var AdvancedSearch = {
            GroupPolicy: $("#ddlAvdGroupPolicies").UifSelect("getSelected"),
            TypePolicy: $("#ddlAvdTypePolicies").UifSelect("getSelected"),
            Level: $("#ddlLevels").UifSelect("getSelected"),
            Name: $("#txtName").val(),
            Message: $("#txtMessage").val(),
            Enabled: $('#ChkEnabled').is(':checked')
        };

        if (AdvancedSearch.GroupPolicy != "")
        {

            if ($("#ddlAvdGroupPolicies").UifSelect("getSelected") != null) {
                $("#ddlGroupPolicies").UifSelect("setSelected", $("#ddlAvdGroupPolicies").UifSelect("getSelected"));
                PoliciesAut.ChangeGroup(null, { Id: $("#ddlAvdGroupPolicies").UifSelect("getSelected")});
            }

            $("#lsvPolicies").UifListView("refresh");

            $.ajax({
                type: "POST",
                url: rootPath + "AuthorizationPolicies/Policies/GetPoliciesByFilter",
                data: JSON.stringify({
                    groupPolicyId: AdvancedSearch.GroupPolicy != "" ? AdvancedSearch.GroupPolicy : null,
                    typePolicyId: AdvancedSearch.TypePolicy != "" ? AdvancedSearch.TypePolicy : null,
                    levelId: AdvancedSearch.Level != "" ? AdvancedSearch.Level : null,
                    name: AdvancedSearch.Name != "" ? AdvancedSearch.Name : null,
                    message: AdvancedSearch.Message != "" ? AdvancedSearch.Message : null,
                    enabled: AdvancedSearch.Enabled
                }),
                datatype: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    $("#lsvPolicies").UifListView(
                        {
                            selectionType: "single",
                            displayTemplate: "#display-template-Policies",
                            delete: true,
                            deleteCallback: this.DeletePolices,
                            add: true,
                            edit: true,
                            customAdd: true,
                            customEdit: true
                        });
                    $.each(data.result, function (index, val) {
                        val.RequestDate = FormatDate(val.RequestDate);
                        $("#lsvPolicies").UifListView("addItem", val);
                    });
                    dropDownSearch.hide();
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchRequest, 'autoclose': true });
                }
            }).error(function (data) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchRequest, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchRequest, 'autoclose': true });
        }
    }
    AvdSearchPolicyCancel() {
        dropDownSearch.hide();
    }
   
}
