class AdvancedSearchRequest {

    static GetProductAdvancedSearch(product) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetProductAdvancedSearch',
            data: JSON.stringify({ product: product }),
            datatype: "json",
            contentType: "application/json; charset=utf-8"
        });
    } 

}