var categoryIndex = null;
var categoryIdSelected = null;

class CategoryTax extends Uif2.Page {
    getInitialState() {
        $("#lvCategoryTax").UifListView({
            source: null,
            edit: true,
            delete: true,
            customEdit: true,
            displayTemplate: "#CategoryTaxTemplate",
            deleteCallback: CategoryTax.deleteCallbackCategoryTax,
            selectionType: 'single', height: 300
        });
        CategoryTax.GetCategoriesByTaxId();
        $('#titleTaxSelected').text(taxModel.Description);
    }

    bindEvents() {
        $('#IdCategory').attr("disabled", true);
        $('#AddCategory').click(CategoryTax.AddCategory);
        $('#CancelCategory').click(CategoryTax.CancelCategory);
        $('#lvCategoryTax').on('rowEdit', CategoryTax.showData);
        $('#ExitCategory').click(CategoryTax.ExitCategory);
        $('#SaveCategory').click(CategoryTax.SaveCategoryBD);
    }

    static showData(event, result, index) {
        CategoryTax.ClearCategory();
        categoryIndex = index;
        categoryIdSelected = result.IdCategory;
        $("#IdCategory").val(result.IdCategory);
        $("#DescriptionCategory").val(result.DescriptionCategory);
    }

    static AddCategory() {
        $("#formCategoryTax").validate();
        if ($("#formCategoryTax").valid()) {
            var formCategoryTax = $("#formCategoryTax").serializeObject();
            if (categoryIndex != null) {
                formCategoryTax.Status = ParametrizationStatus.Update;
                $("#lvCategoryTax").UifListView('editItem', categoryIndex, formCategoryTax);
            } else {
                formCategoryTax.Status = ParametrizationStatus.Create;
                $("#lvCategoryTax").UifListView("addItem", formCategoryTax);  
            }
            CategoryTax.ClearCategory();
        }
    }

    static deleteCallbackCategoryTax(deferred, result) {
        deferred.resolve();

        var data = result;
        var taxCode = data.IdTax;
        var taxCategoryCode = data.IdCategory;

        CategoryTaxRequests.DeleteSelectedTaxCategory(taxCategoryCode, taxCode).done(function (response) {
            if (response.success) {
                if (response.result == true) {
                    data.Status = ParametrizationStatus.Delete;
                    data.allowEdit = false;
                    data.allowDelete = false;
                    $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.TaxCategoryDeleted, 'autoclose': true });
                }
                else {
                    CategoryTax.GetCategoriesByTaxId();
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                    $("#lvCategoryTax").UifListView("refresh");
                }
            }
            else {
                CategoryTax.GetCategoriesByTaxId();
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                $("#lvCategoryTax").UifListView("refresh");
            }
        });
    }
    static CancelCategory() {
        CategoryTax.ClearCategory();
    }

    static ClearCategory() {
        $("#IdCategory").val('');
        $("#DescriptionCategory").val('');
        categoryIndex = null;
        categoryIdSelected = null;
        ClearValidation("#formCategoryTax");
    }

    static ExitCategory() {
        router.rlite.run("prtTax");
    }

    static SaveCategoryBD() {
        var data = $("#lvCategoryTax").UifListView("getData");
        if (data != null && data !== undefined) {
            var taxCode = taxModel.Id;
            var categoryTaxModel = [];
            $.each(data, function (index, value) {
                categoryTaxModel.push({ "IdCategory": data[index].IdCategory, "IdTax": taxCode, "DescriptionCategory": data[index].DescriptionCategory });
            });
            CategoryTaxRequests.SaveTaxCategory(categoryTaxModel).done(function (response) {
                if (response.success) {
                    if (response.result.length > 0) {
                        $("#lvCategoryTax").UifListView("refresh");
                        CategoryTax.GetCategoriesByTaxId();
                        CategoryTax.ClearCategory();
                        $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.TaxCategorySaved, 'autoclose': true });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RecordError, 'autoclose': true });
                        categoryTaxModel = [];
                    }
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorEmpty, 'autoclose': true });
        }


    }

    static GetCategoriesByTaxId() {
        var taxCode = taxModel.Id;
        CategoryTaxRequests.GetCategoriesByTaxId(taxCode).done(function (response) {
            if (response.success) {
                var data = response.result;
                if (data.length > 0) {
                    taxModel.TaxCategories = [];
                    $.each(data, function (index, value) {
                        data[index].Status = ParametrizationStatus.Original;
                        $("#lvCategoryTax").UifListView("addItem", data[index]);
                        taxModel.TaxCategories.push({ "Id": data[index].IdCategory, "IdTax": taxCode, "Description": data[index].DescriptionCategory });
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.QueryNotData, 'autoclose': true });
                }
            }
        });
    }
}