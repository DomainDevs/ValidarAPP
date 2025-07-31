var glbProduct2G = {};
var Product2GNew = {};
var glbProduct2gDelete = [];
var Product2g = {};
var Product2gDelete = [];
var Product2gAdd = {};
var Product2gIndex = null;
var Product2GByPrefixCode = [];
var status = "Add";
class Product2GQueries {
    static GetProduct2G() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Product2G/GetProduct2G',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetProduct2GByPrefixCode(PrefixCode) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/Product2G/GetProduct2GByPrefixCode',
            data: JSON.stringify({ PrefixCode: PrefixCode}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateFileToExport() {
        return $.ajax({
            type: 'POST',
			url: rootPath + 'Parametrization/Product2G/GenerateFileToExport',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    
}

class Product2G extends Uif2.Page {

    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#listViewProduct2g").UifListView({ source: null, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Product2G.DeleteItemProduct2g, displayTemplate: "#Product2gTemplate", selectionType: 'single', height: 310 });
        
        PrefixQueries.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#ddlBusinessType').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        Product2GQueries.GetProduct2G().done(function (data) {
            if (data.success) {
                glbProduct2G = data.result;
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
		});

		$("#inputProductId").ValidatorKey(ValidatorType.Number, 2, 0);
    }

    bindEvents() {
        $('#btnAcceptProduct2g').on('click', this.addItemProduct2G);
        $('#btnNewProduct2g').on('click', Product2G.clearPanel);
        $('#btnExit').click(this.exit);
        $('#btnSave').on('click', this.saveProduct2G);
        $('#btnExport').on('click', this.sendExcelProduct2G);
		$("#ddlBusinessType").on('itemSelected', Product2G.GetProduct2GByPrefixCode);
        $('#listViewProduct2g').on('rowEdit', Product2G.showData);
        $('#inputProduct2gSearch').on('itemSelected', Product2G.SearchProduct2G);
        $('#inputProduct2gSearch').on("buttonClick", Product2G.SearchProduct2G);
        $("#listViewProduct2g").on('rowDelete', Product2G.DeleteItemProduct2g);
    }

    static DeleteItemProduct2g(deferred, data) {
        

         deferred.resolve();
         var index = $("#listViewProduct2g").UifListView("getData").indexOf(data);
         if (data.ProductId !== "" && data.ProductId !== undefined && data.Status == "Original") {
             data.Status = "Delete";
             data.allowEdit = false;
             data.allowDelete = false;
             Product2gDelete.push(data);
             $("#listViewProduct2g").UifListView("addItem", data);
         }

         Product2G.clearPanel();
    }

    static clearPanel() {
        ClearValidation("#formProduct2g");
        Product2gIndex = null;
        $("#inputProductId").val('');
        $("#inputDescription").val('');
        $("#ddlBusinessType").UifSelect("disabled", false);
    }

	static GetProduct2GByPrefixCode() {
		Product2gIndex = null;
		$("#inputProductId").val('');
		$("#inputDescription").val('');
        if ($("#listViewProduct2g").UifListView('getData').length > 0) {
            $("#listViewProduct2g").UifListView("clear");
        }
		var PrefixCode = $("#ddlBusinessType").UifSelect("getSelected");
        Product2GQueries.GetProduct2GByPrefixCode(PrefixCode).done(function (data) {
            if (data.success) {
                //Product2G.clearPanel();
                Product2GByPrefixCode = data.result;
                Product2G.loadProduct2G();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static loadProduct2G() {
        var Product2GList = [];
        $("#listViewProduct2g").UifListView("clear");
        $.each(Product2GByPrefixCode, function (key, value) {
           var Product2GListItem = {
                ProductId: this.ProductId,
                PrefixCode: this.PrefixCode,
                PrefixDescription: this.PrefixDescription,
                Description: this.Description,
                Status: 'Original'
            }
            Product2GList.push(Product2GListItem);            
        });
        $("#listViewProduct2g").UifListView({ sourceData: Product2GList, add: false, edit: true, customEdit: true, delete: true, deleteCallback: Product2G.DeleteItemProduct2g, displayTemplate: "#Product2gTemplate", selectionType: 'single', height: 310 });
        
    }

    static showData(event, result, index) {
        //Product2G.clearPanel();
        if (result.length == 1) {
            index = result[0].key;
            result = result[0];
        }
        else if (result.length > 1) {
            Product2G.ShowSearchAdv(result);
        }
        if (result.ProductId != undefined) {
            Product2gIndex = index;
			$("#inputProductId").val(result.ProductId);
			$("#inputProductId").attr("tag", result.ProductId)
			$("#inputDescription").val(result.Description);
			$("#inputDescription").attr("tag", result.Description)
            $("#ddlBusinessType").UifSelect("setSelected", result.PrefixCode);
            $("#ddlBusinessType").UifSelect("disabled", true);
            result.Status = "Modified";
            status = result.Status;
        }
    }

    static SearchProduct2G() {
        var find = false;
		var data = [];
		var description = $('#inputProduct2gSearch').val();
        if (description.length < 3) {
            $.UifNotify('show', {
				'type': 'danger', 'message': Resources.Language.MessageInfoMinimumChar, 'autoclose': true
            })
		}
		else
		{
            $.each(glbProduct2G, function (i, value) {
                if (
                    (value.Description.includes($("#inputProduct2gSearch").val()))

                ) {
                    value.key = i;
                    data.push(value);
                    find = true;
                }
            });
			
            if (find == false) {
                $.UifNotify('show',
                    {
                        'type': 'danger', 'message': Resources.Language.Product2GNotFound, 'autoclose': true
                    })
            } else {
                Product2G.showData(null, data, data.key);
            }
		}

        $("#inputProduct2gSearch").val("");
    }

    static showDataAdv(product2G) {
        var find = false;
        var data = [];
        $.each(glbProduct2G, function (key, value) {
            if ((value.ProductId == product2G.ProductId)) {
                Product2gIndex = null;
                data.push(value);
                find = true;
            }
        });
        if (find == false) {
            $.UifNotify('show',
                {
                    'type': 'danger', 'message': Resources.Language.Product2GNotFound, 'autoclose': true
                })
        }

        if ($("#listViewProduct2g").UifListView('getData').length > 0) {
            $("#listViewProduct2g").UifListView("clear");
        }

        $("#inputProductId").val(product2G.ProductId);
        $("#ddlBusinessType").UifSelect("setSelected", product2G.PrefixCode);
        $("#inputDescription").val(product2G.Description);
        
    }

    static ShowSearchAdv(data) {
       
        if (data) {
            $("#lvSearchAdvProduct2G").UifListView(
                {
                    displayTemplate: "#Product2gAdvTemplate",
                    selectionType: 'single',
                    source: null,
                    height: 400
                });
            data.forEach(item => {
                $("#lvSearchAdvProduct2G").UifListView("addItem", item);
            });
        }
        dropDownSearchAdvProduct2G.show();
    }

    deleteItemProduct2G(deferred, data) {
        deferred.resolve();
        var index = $("#listViewProduct2g").UifListView("getData").indexOf(data);
        if (data.ProductId !== "" && data.ProductId !== undefined && data.Status =="Original" ) {
            data.Status = "Delete";
            data.allowEdit = false;
            data.allowDelete = false;
            Product2gDelete.push(data);
            $("#listViewProduct2g").UifListView("addItem", data);
        }

        Product2G.clearPanel();
        
    }

    saveProduct2G() {
        Product2g = $("#listViewProduct2g").UifListView('getData').filter(x => x.Status === "Modified");
        Product2gAdd = $("#listViewProduct2g").UifListView('getData').filter(x => x.Status === "Add");

        //Product2gDelete = $("#listViewProduct2g").UifListView('getData').filter(x => x.Status === "Delete");
        $.each(Product2gDelete, function (index, item) {
            Product2g.push(item);
        });

        Product2gAdd = $("#listViewProduct2g").UifListView('getData').filter(x => x.Status === "Add");
        $.each(Product2gAdd, function (index, item) {
            Product2g.push(item);
        });

        if (Product2g.length > 0) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Parametrization/Product2G/SaveProduct2G',
                data: JSON.stringify({ Product2GUpdate: Product2g }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    //Product2G.loadProduct2G();
                    //Product2G.loadProduct2G(data.result[1]);
                    var list = $("#listViewProduct2g").UifListView('getData');
                    var filter = $("#listViewProduct2g").UifListView('getData').filter(x => x.Status != "Original" && x.Status != "Delete");

                    $.each(filter, function (key, value) {
                        var index = list.indexOf(this);
                        this.Status = "Original";
                        $("#listViewProduct2g").UifListView("editItem", index, this);
                    });

                    Product2G.GetProduct2GByPrefixCode();
                    $.each(data.result[0], function (index, item) {
                        $.UifNotify('show', { 'type': 'info', 'message': item, 'autoclose': true })
                    });
                    Product2gDelete = [];
                    Product2GQueries.GetProduct2G().done(function (data) {
                        if (data.success) {
                            glbProduct2G = data.result;
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveProduct2G, 'autoclose': true })
            });
        }
        Product2g = null;

    }

    sendExcelProduct2G() {
        Product2GQueries.GenerateFileToExport().done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    addItemProduct2G() {
        $("#formProduct2g").validate();
        if ($("#formProduct2g").valid()) {
            Product2GNew = {};
            Product2GNew.ProductId = parseInt($("#inputProductId").val());
            Product2GNew.PrefixCode = $("#ddlBusinessType").UifSelect("getSelected");
            Product2GNew.PrefixDescription = $("#ddlBusinessType").UifSelect("getSelectedText");
            Product2GNew.Description = $("#inputDescription").val();
            Product2GNew.Status = status;
            if (Product2gIndex == null) {
                var ifExist = $("#listViewProduct2g").UifListView('getData').filter(function (item) {
                    return item.ProductId == $("#inputProductId").val() || item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase()
                });
                if (ifExist.length > 0) {
					$.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistProduct2G, 'autoclose': true });
                }
                else {
                    Product2GNew.Status = "Add";
                    $("#listViewProduct2g").UifListView("addItem", Product2GNew);
                }
            }
            else {
               
				var ifExist = $("#listViewProduct2g").UifListView('getData').filter(function (item) {
                    return ((item.ProductId == $("#inputProductId").val() && item.ProductId != $("#inputProductId").attr("tag")) ||
						(item.Description.toUpperCase() == $("#inputDescription").val().toUpperCase() && item.Description.toUpperCase() != $("#inputDescription").attr("tag").toUpperCase()));
                });
                if (ifExist.length > 0) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorExistProduct2G, 'autoclose': true });
				}
				else {
					$('#listViewProduct2g').UifListView('editItem', Product2gIndex, Product2GNew);
				}
            }

            Product2G.clearPanel();
        }
    }

    exit() {
        window.location = rootPath + "Home/Index";
    }

    
}