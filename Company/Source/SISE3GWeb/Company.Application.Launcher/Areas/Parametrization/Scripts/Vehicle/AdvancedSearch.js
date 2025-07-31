var dropDownSearchAdvFasecolda = null;

class AdvancedSearchFasecolda extends Uif2.Page {
    getInitialState() {
        dropDownSearchAdvFasecolda = uif2.dropDown({
            source: rootPath + 'Parametrization/Vehicle/AdvancedSearch',
            element: '#btnShowAdvancedSearchFsecolda',
            align: 'right',
            width: 500,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });
        
      
    }
    ////////eventos de busqueda avanzada//////////////////////
    bindEvents() {
      //  //  $("#btnSearchAdvScript").on("click", this.ShowAdvanced);
      ////  $("#btnOkSearchSript").on("click", this.OkSearchAdv);
      //  $("#btnCancelSearchScript").on("click", AdvancedSearchBusiness.CancelSearchAdv);
      //  $('#btnShowAdvancedSearchFsecolda').on("click", this.searchScriptBusiness);
      //  //$("#btnOkSearchSript").click(AdvancedSearchBusiness.LoadSearch);
      //  this.componentLoadedCallback();
    }
    static CancelSearchAdv() {
        //AdvancedSearchBusiness.HideSearchAdv();
    }

    //OkSearchAdv() {
    //    let data = $("#listviewSearch").UifListView("getSelected");
    //    if (data.length === 1) {
    //        AdvancedSearchBusiness.showData(null, data, data.key);
    //    }
    //    AdvancedSearchBusiness.HideSearchAdv();
    //}
    static HideSearchAdv() {
        //dropDownSearchAdvPrefix.hide();
    }
    ////////Busqueda avanzada/////////////////
    //searchScriptBusiness() {
    //    var name = $("#inputCommercialBranchName").val();
    //    var code = $("#inputBranchCode").val();
    //    var prefixAdv = $("#ddlselectAdvanced").UifSelect("getSelected");
    //    var data = [];

    //    var search = $("#listViewBusinessBranch").UifListView('getData');
    //    if (name == "" && code == "" && prefixAdv == "") {
    //        data = search;
    //        find = true;
    //    }
    //    else {
    //        if (name != "") {
    //            $.each(search, function (key, value) {
    //                if (value.Description.toLowerCase().sistranReplaceAccentMark().includes(name.toLowerCase().sistranReplaceAccentMark())) {
    //                    value.key = key;
    //                    data.push(value);
    //                    find = true;
    //                }
    //            });
    //        }

    //        if (code != "") {
    //            var searchCode = [];
    //            if (name == "") {
    //                searchCode = search;
    //            }
    //            else {
    //                searchCode = data;
    //            }
    //            find = false;
    //            $.each(searchCode, function (key, value) {
    //                if (value.Id == parseInt(code)) {
    //                    value.key = key;
    //                    data.push(value);
    //                    find = true;
    //                }
    //            });
    //        }

    //        if (prefixAdv != "") {
    //            var searchPrefix = [];
    //            if (name == "" && code == "") {
    //                searchPrefix = search;
    //            }
    //            else {
    //                searchPrefix = data;
    //            }
          
    //            find = false;
    //            $.each(searchPrefix, function (key, value) {
    //                $.each(value.LineBusiness, function (key2, value2) {
    //                    if (value2.Id == parseInt(prefixAdv)) {
    //                        value.key = key2;
    //                        data.push(value);
    //                        find = true;
    //                    }
    //                });

    //            });
    //        }
    //    }

    //    if (find === false) {
    //        $.UifNotify('show',
    //            { 'type': 'danger', 'message': Resources.Language.MessageNotFoundPrefixes, 'autoclose': true })
    //    } else {
    //        if (data.length === 1) {
    //            ParametrizationPrefix.showData(null, data, data.key);
    //            dropDownSearchAdvPrefix.hide()
    //        } else {
    //            AdvancedSearchBusiness.ShowSearchAdv(data);
    //        }
    //    }
    //}
    static ShowSearchAdv(data) {
        //if ($("#listviewSearch").UifListView('getData').length > 0) {
        //    $("#listviewSearch").UifListView("clear");
        //}
        //if (data) {
        //    data.forEach(item => {
        //        $("#listviewSearch").UifListView("addItem", item);
        //    });
        //}
        //dropDownSearchAdvPrefix.show();
    }
   
    static ClearAdvanced() {
        //$("#inputCommercialBranchName").val('');
        //$("#inputBranchCode").val('');
        ////$("#ddlselectAdvanced").UifSelect("setSelected", null);
    }

    componentLoadedCallback() {
    
        //$("#listviewSearch").UifListView({
        //    displayTemplate: "#searchTemplate",
        //    selectionType: 'single',
        //    source: null,
        //    height: 200
        //});
        //$("#btnShowAdvancedSearchFsecolda").on("click", function () {
        //    $("#listviewSearch").UifListView("refresh");
        //    AdvancedSearchBusiness.ClearAdvanced();
        //    dropDownSearchAdvPrefix.show();
        //});
        //$("#btnLoad").on("click", function () {           
        //    var itemSelected = $("#listviewSearch").UifListView("getSelected");
        //    AdvancedSearchBusiness.ClearAdvanced();         
        //    dropDownSearchAdvPrefix.hide();
        //    if (itemSelected != "") {
        //        ParametrizationPrefix.SearchProcess(itemSelected[0].Id, 0);
        //    }
        //});
    }
}