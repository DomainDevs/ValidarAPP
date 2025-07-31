 
class ProfileAdvSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'UniqueUser/Profile/ProfileAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 500,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });
    }
    bindEvents() {
       
      //  $("#btnCancel").on("click", this.cancelSearch);
        this.componentLoadedCallback();
    }

   
    //cancelSearch() {
    //    $("#listViewSearchAdvanced").UifListView("refresh");
    //    ProfileAdvSearch.ClearAdvanced();
    //    dropDownSearch.hide();
    //}
    static ClearAdvanced() {
        $("#inputSearchProfile").val('');
        $("#listViewSearchAdvancedp").UifListView("clear");
        $("#inputSearch").val('');
        
    }

    static HideSearchAdv() {
        dropDownSearch.hide();
    }
    componentLoadedCallback() {     
        $('#btnAdvancedSearch').on("click", this.AdvancedQuery);
        $("#btnCancel").on('click', function () {
            ProfileAdvSearch.HideSearchAdv();
            ProfileAdvSearch.ClearAdvanced();
           
        })
       
        


        $('#btnShowAdvanced').on('click', function () {
          
            dropDownSearch.show();
            ProfileAdvSearch.getAllProfileByDescription();
    
        });

        $("#btnLoad").on("click", function () {            
            
            dropDownSearch.hide();
            var selected = $("#listViewSearchAdvancedp").UifListView("getSelected");
            if (selected != "") {
                Profile.getProfileByDescription(selected[0].Id, "");
            }
            ProfileAdvSearch.ClearAdvanced();
        });
    }
    static getAllProfileByDescription() {
        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/Profile/GetProfileByDescription',
            data: JSON.stringify({ description: '', id: 0 }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                ProfileAdvSearch.LoadAdvanced(data.result);
                
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ProfileNotFound, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearch, 'autoclose': true })
        });
    }
    AdvancedQuery() {
        $("#listViewSearchAdvancedp").UifListView("refresh");
        var profileAdvancedSearch = {
            Description: $("#inputSearch").val() 
        };

        $.ajax({
            type: "POST",
            url: rootPath + 'UniqueUser/Profile/GetProfileByDescription',
            data: JSON.stringify({ profile: profileAdvancedSearch }),
            datatype: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                AccessAdvSearch.LoadAdvanced(data);
            }
        }).error(function (data) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchAccess, 'autoclose': true });
            });
        screenTop();
    }
    static LoadAdvanced(profiles) {
        $("#listViewSearchAdvancedp").UifListView({
            displayTemplate: "#advancedSearchTemplatep",
            selectionType: 'single',
            height: 400
        });

        $.each(profiles, function (index, val) {
            $("#listViewSearchAdvancedp").UifListView("addItem", profiles[index]);
        });
    }


}





