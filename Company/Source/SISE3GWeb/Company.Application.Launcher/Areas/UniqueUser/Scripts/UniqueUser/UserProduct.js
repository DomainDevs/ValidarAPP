var product = {};
var tempModalProducts = {};
var previousPrefix = null;

class UserProduct extends Uif2.Page {
    getInitialState() {
        UserProduct.GetPrefix().done(function (dataPrefix) {
            if (dataPrefix.success) {
                $('#selectPrefixCodeSearch').UifSelect({ sourceData: dataPrefix.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    bindEvents() {
        $('#btnProduct').on('click', this.saveAndLoad);
        $('#selectPrefixCodeSearch').on('itemSelected', UserProduct.ShowResultTable);
        $('#btnRemoveAllProducts').on("click", this.RemoveAll);
        $('#btnAssingAllProducts').on("click", this.AssignAll);
        $("#tableUserProduct tbody").on("click", "tr", this.Assign);
        $('#btnProductsSave').on("click", this.SaveProducts);
    }
    saveAndLoad() {
        modalSave = true;
        if (glbUser != null) {
            if (glbUser.UserId == 0) {
                if (UniqueUser.saveUniqueUser()) {
                    UserProduct.loadPartialProduct();
                }
            }
            else {
                UserProduct.loadPartialProduct();
                //$("#selectPrefixCodeSearch").val(glbUser.UniqueUsersProduct[0].PrefixCode);
                UserProduct.ShowResultTable();
            }

            if (product.length == undefined) {
                UserProduct.GetProductsByUserId().done(function (dataProduct) {
                    if (dataProduct.success) {
                        product = dataProduct.result;
                        if (tempModalProducts.length == undefined) {
                            tempModalProducts = dataProduct.result;
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }
    Assign() {
        var rowEdit = $('#tableUserProduct').DataTable().row(this).data();
        //$('<span class="glyphicon glyphicon-ok">' + '</span>').appendTo($(this).find('td')[1]);
        if (rowEdit.Enabled == true) {
            if (rowEdit.Assign == true) {
                $('#tableUserProduct').DataTable().row(this).data({ Assign: false, Enabled: rowEdit.Enabled, PrefixCode: rowEdit.PrefixCode, ProductDescription: rowEdit.ProductDescription, ProductId: rowEdit.ProductId, UserId: rowEdit.UserId });
            }
            else {
                $('#tableUserProduct').DataTable().row(this).data({ Assign: true, Enabled: rowEdit.Enabled, PrefixCode: rowEdit.PrefixCode, ProductDescription: rowEdit.ProductDescription, ProductId: rowEdit.ProductId, UserId: rowEdit.UserId });
            }
        }
        else {
            $('#tableUserProduct').DataTable().row(this).data({ Assign: false, Enabled: rowEdit.Enabled, PrefixCode: rowEdit.PrefixCode, ProductDescription: rowEdit.ProductDescription, ProductId: rowEdit.ProductId, UserId: rowEdit.UserId });
        }
        
    }
    AssignAll() {
        var table = $("#tableUserProduct").UifDataTable('getData');
        $.each(table, function (key, value) {
            if (this.Enabled == true) {
                this.Assign = true;
            }
        });
        $("#tableUserProduct").UifDataTable({ sourceData: table })
    }
    RemoveAll() {
        var table = $("#tableUserProduct").UifDataTable('getData');
        $.each(table, function (key, value) {
            this.Assign = false;
        });
        $("#tableUserProduct").UifDataTable({ sourceData: table })
    }
    static ShowResultTable() {
        var filter = [];
        var result = [];
        var table = $("#tableUserProduct").UifDataTable('getData');
        if (table.length > 0) {
            $.each(tempModalProducts, function (key, value) {
                if (value.PrefixCode == previousPrefix) {
                }
                else {
                    result.push(this);
                }
            });
            tempModalProducts = result;
            $.each(table, function (key, value) {
                tempModalProducts.push(this);
            });
            $.each(tempModalProducts, function (key, value) {
                if (this.PrefixCode == $("#selectPrefixCodeSearch").UifSelect("getSelected")) {
                    filter.push(this);
                }
            });

            $("#tableUserProduct").UifDataTable({ sourceData: filter });
            previousPrefix = $("#selectPrefixCodeSearch").UifSelect("getSelected");
        }
        else {
            $.each(tempModalProducts, function (key, value) {
                if (this.PrefixCode == $("#selectPrefixCodeSearch").UifSelect("getSelected")) {
                    filter.push(this);
                }
            });

            $("#tableUserProduct").UifDataTable({ sourceData: filter });
            previousPrefix = $("#selectPrefixCodeSearch").UifSelect("getSelected");
        }

        
    }
    SaveProducts() {
        var result = [];
        var table = $("#tableUserProduct").UifDataTable('getData');
        if (table.length > 0) {
            $.each(tempModalProducts, function (key, value) {
                if (value.PrefixCode == previousPrefix) {
                }
                else {
                    result.push(this);
                }
            });
            tempModalProducts = result;
            $.each(table, function (key, value) {
                tempModalProducts.push(this);
            });
            previousPrefix = null;
            product = tempModalProducts;
            tempModalProducts = {};

        }
        else {
            previousPrefix = null;
            product = tempModalProducts;
            tempModalProducts = {};
        }

        

        var listProducts = [];
        if (product.length > 0) {
            $.each(product, function (key, value) {
                if (value.Assign == true) {
                    listProducts.push(this);
                }
            });
            if (listProducts.length > 1) {
                $('#selectedProduct').text(AppResources.LabelVarious);
                glbUser.UniqueUsersProduct = listProducts;
            }
            if (listProducts.length == 1) {
                $('#selectedProduct').text(listProducts[0].ProductDescription);
                glbUser.UniqueUsersProduct = listProducts;
            }
            if (listProducts.length < 1) {
                $('#selectedProduct').text("");
                glbUser.UniqueUsersProduct = null;
            }
            
        }

        $("#modalProduct").UifModal('hide');
        $("#selectPrefixCodeSearch").UifSelect("setSelected", null);
    }
    static loadPartialProduct() {
        UniqueUser.showPanelsUser(MenuType.UserProduct);
        if ($('#colnamemodalproduct').length) {

        }
        else {
            $('#modalProduct div.modal-header').first().css({ 'background': '#365380', 'color': 'white' });
            $('#modalProduct div.modal-header').first().find("button").wrap("<div class='column-25 even pull-right'></div>");
            $('#modalProduct div.modal-header').first().find("h4").wrap("<div class='column-75 pull-left' id='colnamemodalproduct'></div>");
            $('#modalProduct div.modal-header').first().find("button").wrap("<div class='input-group uif-inputsearch-group'></div>");
            $('<input class="uif-input-search uif-inputsearch-pc form-control" style="background:#365380; border:none; color:white" readonly id="usernamemodal">').insertBefore($('#modalProduct div.modal-header').first().find("button"));
        }
        $("#usernamemodal").val($("#LoginName").val());
    }
    static GetPrefix() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Massive/RenewalRequestGrouping/GetPrefixes",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetProductsByUserId() {
        var data = glbUser.UserId;
        return $.ajax({
            type: "POST",
            url: rootPath + "UniqueUser/UniqueUser/GetUniqueUserProductsStatusByUserId",
            data: JSON.stringify({
                userId: data
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

