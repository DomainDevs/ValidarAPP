var dropDownSearchAdvBusinessConfiguration = null;
$(() => {
    new BusinessConfigurationAdvancedSearch();
});
class BusinessConfigurationAdvancedSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearchAdvBusinessConfiguration = uif2.dropDown({
            source: rootPath + 'Parametrization/BusinessConfiguration/BusinessConfigurationAdvancedSearch',
            element: '#btnSearchAdvBusinessConfiguration',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });
        PrefixRequest.GetPrefix().done(function (data) {
            if (data.success) {
                $("#selectPrefixCodeSearch").UifSelect({ sourceData: data.result, id: "PrefixCode", name: "PrefixDescription", native: false, filter: false });
            }
        });

    }
    bindEvents() {
    }
    componentLoadedCallback() {
        $("#lvSearchAdvBusinessConfiguration").UifListView({
            displayTemplate: "#BusinessConfigurationTemplate",
            selectionType: "single",
            height: 240
        });

        $("#btnSearchAdvBusinessConfiguration").on("click", BusinessConfigurationAdvancedSearch.SearchAdvBusinessConfiguration);
        $("#btnCancelSearchAdv").on("click", BusinessConfigurationAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", BusinessConfigurationAdvancedSearch.OkSearchAdv);
        $("#btnAdvancedSearchQuery").on("click", BusinessConfigurationAdvancedSearch.AdvancedSearch);
    }
    static SearchAdvBusinessConfiguration() {
        dropDownSearchAdvBusinessConfiguration.show();
        $("#inputDescriptionSearch").val(BusinessConfigurationControls.inputSearchBusinessConfiguration.val());
        $("#inputCodeBusinessSearch").val("");
        PrefixRequest.GetPrefix().done(function (data) {
            if (data.success) {
                $("#selectPrefixCodeSearch").UifSelect({ sourceData: data.result, id: "PrefixCode", name: "PrefixDescription", native: false, filter: false });
            }
        });
    }

    static CancelSearchAdv() {
        ParametrizationBusinessConfiguration.HideSearchAdv();
        BusinessConfigurationAdvancedSearch.clearPanel();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvBusinessConfiguration").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationBusinessConfiguration.ShowData(null, data, data.key);
        }
        ParametrizationBusinessConfiguration.HideSearchAdv();
    }

    static AdvancedSearch() {
        var description = document.getElementById('inputDescriptionSearch').value;
        var e = document.getElementById("selectPrefixCodeSearch");
        var prefix = e.options[e.selectedIndex].value;
        var code = document.getElementById('inputCodeBusinessSearch').value;
        var find = false;
        var data = [];
        var search = glbBusinessConfiguration;
        if (description.length > 0 && description.length < 3) {
            $.UifNotify('show', {
                'type': 'danger', 'message': Resources.Language.ErrorSearchCharMin, 'autoclose': true
            })
        } else {
            if (description == "" && prefix == "" && code != "") {
                $.each(search, function (key, value) {
                    if ((value.BusinessId == code
                        )
                    ) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }
            if (description == "" && prefix != "" && code == "") {
                $.each(search, function (key, value) {
                    if ((value.PrefixCode.PrefixCode == prefix
                    )
                    ) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }
            if (description != "" && prefix == "" && code == "") {
                $.each(search, function (key, value) {
                    if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark()))
                    ) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }
            if (description != "" && prefix != "" && code != "") {
                $.each(search, function (key, value) {
                    if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark())) && (value.PrefixCode.PrefixCode == prefix
                        ) && (value.BusinessId == code)
                    ) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }
            if (description == "" && prefix != "" && code != "") {
                $.each(search, function (key, value) {
                    if ((value.PrefixCode.PrefixCode == prefix
                        ) && (value.BusinessId == code)
                    ) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }
            if (description != "" && prefix == "" && code != "") {
                $.each(search, function (key, value) {
                    if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark())) && (value.BusinessId == code)
                    ) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }
            if (description != "" && prefix != "" && code == "") {
                $.each(search, function (key, value) {
                    if ((value.Description.toLowerCase().sistranReplaceAccentMark().
                        includes(description.toLowerCase().sistranReplaceAccentMark())) && (value.PrefixCode.PrefixCode == prefix
                        )
                    ) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }


            if (find == false) {
                $.UifNotify('show',
                    {
                        'type': 'danger', 'message': Resources.Language.BusinessConfigurationNotFound, 'autoclose': true
                    })
            } else {
                ParametrizationBusinessConfiguration.ShowData(null, data, data.key);
                if (data.length == 1) {
                    dropDownSearchAdvBusinessConfiguration.hide();
                }
            }
        }
    }

    static clearPanel() {
        $("#inputDescriptionSearch").val('');
        $("#BusinessId").val('');
        $("#selectPrefixCodeSearch").UifSelect("setSelected", null);
        $("#selectPrefixCodeSearch").UifSelect("disabled", false);
        if ($("#lvSearchAdvBusinessConfiguration").UifListView('getData').length > 0) {
            $("#lvSearchAdvBusinessConfiguration").UifListView("clear");
        }
    }
}
function check(e, value) {
    //Valida Caracteres
    var unicode = e.charCode ? e.charCode : e.keyCode;
    if (unicode > 31 && (unicode < 48 || unicode > 57))
    if (document.getElementById('inputCodeBusinessSearch').value.length > 4) return false;
}
function checkLength() {
    var fieldLengthCode = document.getElementById('inputCodeBusinessSearch').value.length;
    var fieldLengthDescription = document.getElementById('inputDescriptionSearch').value.length;
    //Valida cantidad
    if (fieldLengthCode <= 4) {
        return true;
    }
    else {
        var str = document.getElementById('inputCodeBusinessSearch').value;
        str = str.substring(0, str.length - 1);
        document.getElementById('inputCodeBusinessSearch').value = str;
    }
    if (fieldLengthDescription <= 50) {
        return true;
    }
    else {
        var str = document.getElementById('inputDescriptionSearch').value;
        str = str.substring(0, str.length - 1);
        document.getElementById('inputDescriptionSearch').value = str;
    }
}