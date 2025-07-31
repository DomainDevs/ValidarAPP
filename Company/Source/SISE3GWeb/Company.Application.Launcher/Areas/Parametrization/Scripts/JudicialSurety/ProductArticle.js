
var objectToAdd = [];
var objectToDelete = [];
var objectToUpdate = [];

class ProductArticle extends Uif2.Page {

    getInitialState() {
        ProductArticle.LoadProducts();
        ProductArticle.LoadArticles();
        ProductArticle.LoadProductArticles();
    }
    bindEvents() {
        $('#inputArticleProduct').on('buttonClick', ProductArticle.SearchArticuleProduct);
        $('#listViewProductArticle').on('rowEdit', ProductArticle.ShowData);
        $('#btnAcceptObject').click(ProductArticle.AddObject);
        $('#btnNewObject').click(ProductArticle.NewObject);
        $("#btnSave").click(ProductArticle.SaveObjects);
        
    }

    static getObjectModel(objectModel) {
        var returnObject = [];
        $.each(objectModel, function (key, value) {
            var object = new Object();
            object.ArticleId = parseInt(value.ArticleId);
            object.ProductId = parseInt(value.ProductId);
            object.ArticleDescription = value.ArticleDescription;
            object.ProductDescription = value.ProductDescription;
            returnObject.push(object);
        });
        return returnObject;
    }

    static SaveObjects() {
        if (objectToAdd.length > 0 || objectToDelete.length > 0 || objectToUpdate.length > 0) {

            if (objectToAdd.length>0) {
                objectToAdd = ProductArticle.getObjectModel(objectToAdd) 
            }
            if (objectToDelete.length > 0) {
                objectToDelete = ProductArticle.getObjectModel(objectToDelete)
            }
            if (objectToUpdate.length > 0) {
                objectToUpdate = ProductArticle.getObjectModel(objectToUpdate)
            }
            ProductArticleRequest.UpdateProductArticle(objectToDelete, objectToUpdate, objectToAdd).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', { type: 'success', message: data.result, autoclose: false });
                }
                else {
                    $.UifNotify('show', { type: 'danger', message: data.result, autoclose: false });
                }
                objectToAdd = [];
                objectToDelete = [];
                objectToUpdate = [];
                $("#listViewProductArticle").UifListView('clear');
                ProductArticle.LoadProductArticles();
            });
        }
    }

    static NewObject() {

        $("#selectArticle").attr("disabled", false);
        $("#selectProduct").attr("disabled", false);
        $("#selectArticle").UifSelect("setSelected", "");
        $("#selectProduct").UifSelect("setSelected", "");
        
    }

    static AddObject() {
        if ($("#selectProduct").val() != "" && $("#selectArticle").val() != "") {
            var newObject = [];
            var lista = $("#listViewProductArticle").UifListView('getData');
            var existObject = lista.filter(function (item) {
                return item.ArticleId == $("#selectArticle").val() && item.ProductId == $("#selectProduct").val()
            });
            if (existObject.length > 0) {
                $.UifNotify('show', { type: 'info', message: Resources.Language.ErrorExpansesTaxes, autoclose: true });
            }
            else {
                newObject.ArticleId = parseInt($("#selectArticle").val());
                newObject.ProductId = parseInt($("#selectProduct").val());
                newObject.ArticleDescription = $('select[name="Article"] option:selected').text();
                newObject.ProductDescription = $('select[name="Product"] option:selected').text();
                objectToAdd.push(newObject);
                newObject.Status = ParametrizationStatus.Create
                $("#listViewProductArticle").UifListView("addItem", newObject);
            }
        }
    }

    static ShowData(event, result, index) {
        if (result != undefined && result != null) {
            if (result.ArticleId > 0 && result.ProductId > 0) {
                ProductArticle.setData(result);
            }
        }
        
    }

    static setData(data) {
        $("#selectArticle").attr("disabled", true);
        $("#selectProduct").attr("disabled", true);
        $("#selectArticle").UifSelect("setSelected", data.ArticleId);
        $("#selectProduct").UifSelect("setSelected", data.ProductId);
    }

    static SearchArticuleProduct(event, data, index) {
        var inputObject = $('#inputArticleProduct').val();
        if (inputObject.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            $("#lvSearchAdvArticleProduct").UifListView("clear");
            ProductArticleRequest.LoadSearchProductArticles(inputObject).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        $("#lvSearchAdvArticleProduct").UifListView("clear");
                        $.each(data.result, function (key, value) {
                            $("#lvSearchAdvArticleProduct").UifListView("addItem", value);
                        });
                        ProductArticleSearch.SearchAdvAdvArticleProduct();
                    }
                }
            });
        }
    }

    static LoadProductArticles() {
        ProductArticleRequest.LoadProductArticles().done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#listViewProductArticle").UifListView({
                        displayTemplate: "#ArticleProductTemplate",
                        sourceData: data.result,
                        edit: true,
                        delete: true,
                        customAdd: true,
                        customEdit: true,
                        height: 300,
                        deleteCallback: ProductArticle.DeleteItemProductArticle
                    });
                }
                else {
                    $.UifNotify('show', { type: 'info', message: Resources.Language.getDataError, autoclose: true });
                }
            }
        });
    }
    static LoadProducts() {
        ProductArticleRequest.LoadProducts().done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#selectProduct").UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { type: 'info', message: Resources.Language.getDataError, autoclose: true });
                }
            }
        });
    }
    static LoadArticles() {
        ProductArticleRequest.LoadArticles().done(function (data) {
            if (data.success) {
                $("#selectArticle").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { type: 'info', message: Resources.Language.getDataError, autoclose: true });
            }
        });
    }
    static DeleteItemProductArticle(event, data) {
        event.resolve();
        var existObject = objectToAdd.filter(function (item) {
            return item.ArticleId == data.ArticleId && item.ProductId == data.ProductId
        });
        if (existObject.length > 0) {
            objectToAdd.pop(function (data) { return item.ArticleId == data.ArticleId && item.ProductId == data.ProductId; });
        }
        else {
            data.allowEdit = false;
            data.allowDelete = false;
            data.Status = ParametrizationStatus.Delete
            objectToDelete.push(data);
            $("#listViewProductArticle").UifListView("addItem", data);
        }

       
    }
}