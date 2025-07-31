
class UserStatusType {
    static GetUserStatus() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'UniqueUser/UniqueUser/GetStatusUser',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class UserAdvSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'UniqueUser/UniqueUser/UserAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 520,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });
 
    }
    bindEvents() {
        $("#inputName").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("#inputName").on("itemSelected", this.assignUserId);        
        $("#inputAdvCreationDate").UifDatepicker();
        $("#inputAdvLastModificationDate").UifDatepicker();
     //   this.componentLoadedCallback();        
    }
    static ClearAdvanced() {
        $("#listViewSearchAdvance").UifListView("clear");
        $("#inputName").val('');
        $("#inputUserId").val('');
        $("#inputIdentificationNumber").val('');
        $("#inputAdvCreationDate").val('');
        $("#inputAdvLastModificationDate").val('');
        $("#selectAdvEnabled").UifSelect("setSelected", null);
    }
    componentLoadedCallback() {
        $('#btnAdvancedSearch').on("click", UserAdvSearch.AdvancedQuery);

        $('#inputName').UifAutoComplete({
            source: rootPath + "UniqueUser/UniqueUser/GetUsersByQuery",
            displayKey: "AccountName",
            queryParameter: "&query"
        });

        //$("#listViewSearchAdvanced").UifListView({
        //    displayTemplate: "#advancedSearchTemplate",
        //    selectionType: 'single',
        //    edit: false,
        //    delete: false,
        //    customEdit: false,
        //    height: 190
        //});
        $("#btnShowAdvanced").on("click", function () {
            UserAdvSearch.ClearAdvanced();
            dropDownSearch.show();
            $('#selectAdvEnabled').UifSelect({ sourceData: userStatus });
        });

        $("#btnCancelUserAdv").on('click', function () {
            UserAdvSearch.ClearAdvanced();
            dropDownSearch.hide();
        })

        $("#btnLoadUser").on("click", function () {
            dropDownSearch.hide();
            var userSelected = $("#listViewSearchAdvance").UifListView("getSelected");
            UserAdvSearch.ClearAdvanced();
            if (userSelected != "") {
                UniqueUser.getUserById(userSelected[0].AccountName, 0, userSelected[0].UserId);
            }
        });
    }
    assignUserId(event, selectedItem) {
        if (selectedItem != null) {
            $("#inputName").data("UserAdvanced", selectedItem.IndividualId);
        }
        else {
            $("#inputName").data("UserAdvanced", null);
        }
    }
    getAutocomplete(tag, param) {
        if ($(tag).UifAutoComplete('getValue') == null || $(tag).UifAutoComplete('getValue') == "") {
            $(tag).data(param, null);
        }
        return $(tag).data(param);
    }
	static AdvancedQuery() {
		$("#listViewSearchAdvance").UifListView("refresh");
		var userAdvancedSearch = {
			AccountName: $("#inputName").val(),
			UserId: $("#inputUserId").val(),
			IdentificationNumber: $("#inputIdentificationNumber").val(),
			CreationDate: $("#inputAdvCreationDate").val(),
			Status: $("#selectAdvEnabled").val(),
			LastModificationDate: $("#inputAdvLastModificationDate").val(),
		};

		$.ajax({
			type: "POST",
			url: rootPath + 'UniqueUser/UniqueUser/GetUserAdvancedSearch',
			data: JSON.stringify({ user: userAdvancedSearch }),
			datatype: "json",
			contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            console.log(data);
            if (data.success) {
                if (data.result.length > 0) {
                    UserAdvSearch.LoadUserAdvanced(data.result);
                } else {
                    $("#listViewSearchAdvance").UifListView("clear");
                }
			
			}
			else {
				$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchUser, 'autoclose': true });
			}
		}).error(function (data) {
			$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchUser, 'autoclose': true });
		});
    }
    static LoadUserAdvanced(users) {
        $("#listViewSearchAdvance").UifListView({
            displayTemplate: "#advancedSearchTemplat",
            selectionType: 'single',
            edit: false,
            delete: false,
            customEdit: false,
            height: 190
        });
        $.each(users, function (index, val) {
            $("#listViewSearchAdvance").UifListView("addItem", users[index]);
        });
        
    }


}





