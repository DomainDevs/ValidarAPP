class ProductRequestT {

    static DateNow() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/DateNow',
            async: false,
            //data: {}
        });
    }

    static GenerateFileToProduct(productId, productDescription) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GenerateFileToProduct',
            async: true,
            data: { "productId": productId, "productDescription": productDescription  }
        });
    }

    static GenerateFileToProducts() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GenerateFileToProducts',
            async: true,

        });
    }

    static GetProductByPrefixIdProductId(prefixId, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetProductByPrefixIdProductId',
            data: JSON.stringify({ prefixId: prefixId, productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static SaveProduct(productModelsView) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/SaveProduct',
            async: true,
            data: { "productModelsView": productModelsView  }
        });
    }

    static GetParameterByDescription(description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetParameterByDescription',
            data: JSON.stringify({ description: description  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProductsByPrefixIdByDescription(prefixId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetProductsByPrefixIdByDescription',
            data: JSON.stringify({ prefixId: prefixId, description: description  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProductAdvancedSearch(product) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetProductAdvancedSearch',
            data: JSON.stringify({ product: product  }),
            datatype: "json",
            contentType: "application/json; charset=utf-8"

        });
    }

    static GetMainProductByPrefixIdDescriptionProductId(description, prefixId, productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetMainProductByPrefixIdDescriptionProductId',
            data: JSON.stringify({ description: description, prefixId: prefixId, productId: productId  })
            , dataType: "json",
            contentType: "application/json; charset=utf-8"
        });

    }

    static GetProductAgentByProductId(productId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetProductAgentByProductId',
            data: JSON.stringify({ productId: productId, prefixId: prefixId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveredRiskByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetCoveredRiskByProductId',
            data: JSON.stringify({ productId: productId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDataAditionalByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetDataAditionalByProductId',
            data: JSON.stringify({ productId: productId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyTypesByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetPolicyTypesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProductAgentByProductId(productId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/GetProductAgentByProductId',
            data: JSON.stringify({ productId: productId, prefixId: prefixId  }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",          
        });
    }

    static GetParameterDecimalQty() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Product/Product/GetParameterDecimalQty",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

}