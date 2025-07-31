class CopyProductRequest {

    static CopyProduct(copyProduct) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Product/Product/CreateCopyProduct',
            data: JSON.stringify({ copyProduct: copyProduct }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () { $('#btnModalCopyProductSave').prop("disabled", true); },
            complete: function () { $('#btnModalCopyProductSave').prop("disabled", false); }
        });
    } 

}