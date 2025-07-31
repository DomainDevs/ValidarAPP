$.ajaxSetup({ async: false });
var dropDownSearchProduct;
var ObjectAdvanceSearch =
    {
        bindEvents: function () {
            var dateRange = {};
            $("#btnSearchAdvProduct").on("click", function () {
                dropDownSearchProduct.show();
                ObjectAdvanceSearch.ClearAdvanced();
                //screenTop();
            });

            dropDownSearchProduct = uif2.dropDown({
                source: rootPath + 'Product/Product/AdvancedSearch',
                element: '#btnSearchAdvProduct',
                align: 'right',
                width: 600,
                height: 470
            });

            ObjectAdvanceSearch.componentLoadedCallback();

            $("#selectPrefixCommercialAdvanced").UifSelect({
                source: rootPath + 'Product/Product/GetPrefixes'
            });
            $("input[type=text]").TextTransform(ValidatorType.UpperCase);
            $("#inputFromIssueDate").UifDatepicker();
            $("#inputToIssueDate").UifDatepicker();
        },

        componentLoadedCallback: function () {
            //$("#inputDescriptionAdvanced").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
			$("#inputDescriptionAdvanced").mask(DescriptionAdvancedOnlyNumberLetter());
            $("#btnLoadProduct").on("click", function () {
                dropDownSearchProduct.hide();
                var ProductSelected = $("#listViewSearchAdvanced").UifListView("getSelected");
                if (ProductSelected.length > 0) {
                    ObjectProduct.mainProductByPrefixIdDescription("", ProductSelected[0].Id);
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageNotSelectedProduct, 'autoclose': true });
                }
            });

            $("#listViewSearchAdvanced").UifListView({
                displayTemplate: "#advancedSearchTemplate",
                selectionType: 'single',
                source: null,
                height: 200
            });

            $("#btnCancelSearchAdv").on('click', function () {
                ObjectAdvanceSearch.ClearAdvanced();
                dropDownSearchProduct.hide();
            });

            $('#btnAdvancedSearch').on("click", function () {
                if (valiteDataProduct()) {
                    var productAdvancedSearch = {
                        Prefix: { Id: $("#selectPrefixCommercialAdvanced").UifSelect("getSelected") },
                        Description: $("#inputDescriptionAdvanced").val().trim(),
                        CurrentFrom: $("#inputFromIssueDate").val(),
                        CurrentTo: $("#inputToIssueDate").val()
                    };

                    AdvancedSearchRequest.GetProductAdvancedSearch(productAdvancedSearch).done(function (data) {
                        if (data.length > 0) {
                            ObjectAdvanceSearch.LoadProductAdvanced(data);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageNotFoundProducts, 'autoclose': true });
                        }
                    }).error(function (data) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSearchProduct, 'autoclose': true });
                    });
                }
            });
        },

        LoadProductAdvanced: function (products) {
            $("#listViewSearchAdvanced").UifListView("refresh");
            if (products != null) {
                $.each(products, function (index, val) {
                    products[index].CurrentFrom = FormatDate(products[index].CurrentFrom);
                    $("#listViewSearchAdvanced").UifListView("addItem", products[index]);
                });
            }
        },

        ClearAdvanced: function () {
            $("#listViewSearchAdvanced").UifListView("refresh");
            $("#selectPrefixCommercialAdvanced").UifSelect("setSelected", null);
            $("#inputDescriptionAdvanced").val('');
            $("#inputFromIssueDate").UifDatepicker('clear');
            $("#inputToIssueDate").UifDatepicker('clear');
        }
    }
$(document).ready(function () {
    ObjectAdvanceSearch.bindEvents();
});
function DescriptionAdvancedOnlyNumberLetter() {
	var maxLength = '';
	for (var i = 0; i < 200; i++) {
		maxLength = maxLength + 'A';
	}
	for (var i = 0; i < 200; i++) {
		maxLength = maxLength + '0';
	}
	return maxLength;
}
function valiteDataProduct() {
    if ($("#selectPrefixCommercialAdvanced").UifSelect("getSelected") == null || $("#selectPrefixCommercialAdvanced").UifSelect("getSelected") == "") {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessagePrefix, 'autoclose': true });
        return false;
    }
    if ($("#inputDescriptionAdvanced").val() != "") {
        var value = $("#inputDescriptionAdvanced").val();
        if (value.length > 50) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorMaximumLength50, 'autoclose': true });
        }
    }
    
    return true;
}


