var dropDownSearchAdvPrefix = null;

///////////Creacion de propiedades//////////////////
//var dropDownSearch;
//var glbLinesBusiness = {};
//var LinesBusiness = {};
//var glbPrefixeSearch = {};


////////////////Carga de busqueda avanzada llamado a controlador /////////////
//class SearchLinesBusiness {
//    //static GetLinesBusiness() {
//    //    return $.ajax({
//    //        type: 'POST',
//    //        url: 'GetLinesBusiness',
//    //        dataType: 'json',
//    //        contentType: 'application/js    on; charset=utf-8',
//    //    });
//    //}
//    static SearchAdvanced(LinesBusiness) {
//        return $.ajax({
//            type: "POST",
//            url: rootPath + 'Parametrization/Prefix/SearchAdvanced',
//            data: JSON.stringify({ BusinessBranch: LinesBusiness }),
//            dataType: "json",
//            contentType: "application/json; charset=utf-8"
//        });
//    }
//}
///////////////Clase de busqueda avanzada ///////////////////////////
class AdvancedSearchBusiness extends Uif2.Page {
    getInitialState() {


        LinesBussiness.GetLinesBusiness().done(function (data) {
            if (data.success) {
                linesBusiness = data.result;
                $('#ddlselectAdvanced').UifSelect({ sourceData: linesBusiness });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }
    ////////eventos de busqueda avanzada//////////////////////
    bindEvents() {
        dropDownSearchAdvPrefix = uif2.dropDown({
            source: rootPath + 'Parametrization/Prefix/AdvancedSearch',
            element: '#btnSearchAdvScript',
            align: 'right',
            width: 520,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });
        //  $("#btnSearchAdvScript").on("click", this.ShowAdvanced);
        //  $("#btnOkSearchSript").on("click", this.OkSearchAdv);
       
        //$("#btnOkSearchSript").click(AdvancedSearchBusiness.LoadSearch);
        //this.componentLoadedCallback();
    }
    static CancelSearchAdv() {
        AdvancedSearchBusiness.HideSearchAdv();
    }

    //OkSearchAdv() {
    //    let data = $("#listviewSearch").UifListView("getSelected");
    //    if (data.length === 1) {
    //        AdvancedSearchBusiness.showData(null, data, data.key);
    //    }
    //    AdvancedSearchBusiness.HideSearchAdv();
    //}
    static HideSearchAdv() {
        dropDownSearchAdvPrefix.hide();
    }
    ////////Busqueda avanzada/////////////////
    static searchScriptBusiness() {
        var name = $("#inputCommercialBranchName").val();
        var code = $("#inputBranchCode").val();
        var prefixAdv = $("#ddlselectAdvanced").UifSelect("getSelected");
        var data = [];
        var find = false;

        var search = $("#listViewBusinessBranch").UifListView('getData');
        if (name == "" && code == "" && prefixAdv == "") {
            data = search;
            find = true;
        }
        else {
            if (name != "") {
                $.each(search, function (key, value) {
                    if (value.Description.toLowerCase().sistranReplaceAccentMark().includes(name.toLowerCase().sistranReplaceAccentMark())) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }

            if (code != "") {
                var searchCode = [];
                if (name == "") {
                    searchCode = search;
                }
                else {
                    searchCode = data;
                }
                find = false;
                $.each(searchCode, function (key, value) {
                    if (value.Id == parseInt(code)) {
                        value.key = key;
                        data.push(value);
                        find = true;
                    }
                });
            }

            if (prefixAdv != "") {
                var searchPrefix = [];
                if (name == "" && code == "") {
                    searchPrefix = search;
                }
                else {
                    searchPrefix = data;
                }

                find = false;
                $.each(searchPrefix, function (key, value) {
                    $.each(value.LineBusiness, function (key2, value2) {
                        if (value2.Id == parseInt(prefixAdv)) {
                            value.key = key2;
                            data.push(value);
                            find = true;
                        }
                    });

                });
            }
        }

        if (find) {
            //if (data.length === 1) {
            //    ParametrizationPrefix.showData(null, data, data.key);
            //    dropDownSearchAdvPrefix.hide()
            //} else {
            AdvancedSearchBusiness.ShowSearchAdv(data);
            //}
            //$.UifNotify('show',
            //    { 'type': 'info', 'message': Resources.Language.MessageNotFoundPrefixes, 'autoclose': true })
        } 
    }
    static ShowSearchAdv(data) {
        if ($("#listviewSearch").UifListView('getData').length > 0) {
            $("#listviewSearch").UifListView("clear");
        }
        if (data) {
            data.forEach(item => {
                $("#listviewSearch").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvPrefix.show();
    }

    static ClearAdvanced() {
        $("#inputCommercialBranchName").val('');
        $("#inputBranchCode").val('');
        //$("#ddlselectAdvanced").UifSelect("setSelected", null);
    }

    componentLoadedCallback() {
        $("#btnCancelSearchScript").on("click", AdvancedSearchBusiness.CancelSearchAdv);
        $('#btnAdvancedSearch').click(AdvancedSearchBusiness.searchScriptBusiness);
        $("#listviewSearch").UifListView({
            displayTemplate: "#searchTemplate",
            selectionType: 'single',
            source: null,
            height: 240
        });
        $("#btnSearchAdvScript").on("click", function () {
            $("#listviewSearch").UifListView("refresh");
            AdvancedSearchBusiness.ClearAdvanced();
            dropDownSearchAdvPrefix.show();
        });
        $("#btnLoad").on("click", function () {
            var itemSelected = $("#listviewSearch").UifListView("getSelected");
            AdvancedSearchBusiness.ClearAdvanced();
            dropDownSearchAdvPrefix.hide();
            if (itemSelected != "") {
                ParametrizationPrefix.SearchProcess(itemSelected[0].Id, 0);
            }
        });
    }
}