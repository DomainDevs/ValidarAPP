$.ajaxSetup({ async: false });
$(document).ready(function () {
    //Copiar producto
    $("#btnCopyProduct").on("click", function () {

        if (Product.Id == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageCopyProduct, 'autoclose': true })
        }
        else {
            ClearCopyProduct();
            $("#modalCopyProduct").UifModal('showLocal', Resources.Language.LabelCopyProduct);
        }
    });
    //Volver

    //Copiar Producto
    $("#btnModalCopyProduct").on("click", function () {
        CopyProduct(true);
        ScrollTop();

    });
});
function CopyProduct(showAlert) {
    $("#formCopyProduct").validate();
    if ($("#formCopyProduct").valid()) {

        CopyProductRequest.CopyProduct(CreateCopyProduct()).done(function (data) {
            if (data.success) {
                Product.IsNew = false;
                Product.Id = data.result;
                //var prefixId = Product.PrefixId === undefined ? Product.Prefix.Id : PrefixId;
                LoadCopyProduct();
                if (showAlert) {
                    CleanChecks();
                    $("#modalCopyProduct").UifModal('hide');
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ProductCopiedCorrectly, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
            Product.pendingChanges = false;
        }).fail(function (jqXHR, textStatus, errorThrown) {
            var resultOperation = false;
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveProduct, 'autoclose': true })
        });




    }
}
function CreateCopyProduct() {
    var CopyProduct = {};
    CopyProduct.Description = $("#inputModalCopyProductDescription").val().trim();
    CopyProduct.SmallDescription = $("#inputModalCopyProductReducedDescription").val().trim();
    CopyProduct.Id = Product.Id;
    CopyProduct.CopyGroupCoverages = $("#checkGroupCoverages").is(":checked");
    CopyProduct.CopyPaymentPlan = $("#checkPaymentPlan").is(":checked");
    CopyProduct.CopyRuleSet = $("#checkRuleSet").is(":checked");
    CopyProduct.CopyPrintingFormes = $("#checkPrintingFormes").is(":checked");
    CopyProduct.CopyAgent = $("#checkAgent").is(":checked");
    CopyProduct.CopyLimitRC = $("#checkLimitRC").is(":checked");
    CopyProduct.CopyScript = $("#checkScriptCP").is(":checked");
    CopyProduct.CopyActivityRisk = $("#checkActivityRisk").is(":checked");
    var pr = false;
    return CopyProduct;
}

function CleanChecks() {
    $("#checkGroupCoverages").prop("checked", true);
    $("#checkPaymentPlan").prop("checked", true);
    $("#checkRuleSet").prop("checked", true);
    $("#checkPrintingFormes").prop("checked", true);
    $("#checkAgent").prop("checked", true);
    $("#checkLimitRC").prop("checked", true);
    $("#checkScriptCP").prop("checked", true);
    $("#checkActivityRisk").prop("checked", true);
}

function LoadCopyProduct() {
    $("#inputDescription").val($("#inputModalCopyProductDescription").val().trim());
    $("#inputDescriptionReduced").val($("#inputModalCopyProductReducedDescription").val().trim());
    Product.Description = $("#inputModalCopyProductDescription").val().trim();
    Product.SmallDescription = $("#inputModalCopyProductReducedDescription").val().trim();
    $("#inputProduct").val(Product.Id);
    Product.pendingChanges = false;
    //cargar el producto copia
    ProductRequestT.GetProductByPrefixIdProductId($("#selectPrefixCommercial").UifSelect("getSelected"), Product.Id).done(function (data) {
        if (data.success) {
            Product = data.result[0];//obtiene el primero de la lista
            $("#inputDescription").val($("#inputModalCopyProductDescription").val().trim());
            $("#inputDescriptionReduced").val($("#inputModalCopyProductReducedDescription").val().trim());
            Product.Description = $("#inputModalCopyProductDescription").val().trim();
            Product.SmallDescription = $("#inputModalCopyProductReducedDescription").val().trim();
            $("#inputProduct").val(Product.Id);
            Product.pendingChanges = false;
            ObjectProduct.loadMainProduct(data.result[0]);
            //ObjectProduct.switchGiveCountProduct(data.result[0]);
            //GetRiskTypeByPrefixId($("#selectPrefixCommercial").UifSelect("getSelected"), Product.Id);
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
        }
    });
 }

function ClearCopyProduct() {
    $("#inputModalCopyProductDescription").val('');
    $("#inputModalCopyProductReducedDescription").val('');
}
