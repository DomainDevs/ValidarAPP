$.ajaxSetup({ async: false });

var dropDownSearchSarlaft = null;

$(document).ready(function () {
    ObjectSarlaftSearch.bindEvents();
});

var ObjectSarlaftSearch =
{
    bindEvents: function () {
        var dateRange = {};



        dropDownSearchSarlaft = uif2.dropDown({
            source: rootPath + 'Sarlaft/Sarlaft/SarlaftSimpleSearch',
            element: '#inputSearch',
            align: 'right',
            width: 600,
            height: 470
        });

    },

   

    BindeventsTofunc: function () {
        
    },

   

    LoadSarlaftSimple: function (personas) {

            $("#listViewSearchAdvanced").UifListView({
                displayTemplate: "#advancedSearchTemplate",
                selectionType: 'single',
                source: null,
                height: 315
            });
        

        $("#btnLoadSarlaft").on("click", function () {
            dropDownSearchSarlaft.hide();
            var SarlaftSelected = $("#listViewSearchAdvanced").UifListView("getSelected");
            if (SarlaftSelected != "") {
                SarlaftParam.LoadUniqueSarlaft(SarlaftSelected);
            }
        });
        $("#btnCancelSearchAdv").on('click', function () {
            ObjectSarlaftSearch.ClearAdvanced();
            dropDownSearchSarlaft.hide();
        });

        
        if (personas != null) {
           $.each(personas, function (index, val) {
             $("#listViewSearchAdvanced").UifListView("addItem", personas[index]);
            });
        }
    },

    ClearAdvanced: function () {
        $("#listViewSearchAdvanced").UifListView("refresh");
        $("#inputSearch").val('');
    }
}


