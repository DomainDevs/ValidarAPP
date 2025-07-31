var glbDiscount = null;
class UnderwritingDiscounts extends Uif2.Page {

    getInitialState() {
        request('Underwriting/Underwriting/GetDiscounts', null, 'GET', AppResources.ErrorSearchRiskType, UnderwritingDiscounts.changeDiscountssParametrization);

        $("#listViewDiscounts").UifListView({
            displayTemplate: "#DiscountsTemplate",
            delete: true,
            customAdd: true,
            customEdit: true,
            height: 150,
            deleteCallback: UnderwritingDiscounts.deleteDiscountList,
        });
    }

    bindEvents() {
        $("#inputTaxeType").val("");
        $("#inputTaxe").val(parseFloat(0).toFixed(2)).OnlyDecimals(UnderwritingDecimal);
        $("#btnDiscounts").on("click", UnderwritingDiscounts.DiscountLoad);
        $("#btnNewDiscounts").on("click", UnderwritingDiscounts.ClearDiscounts);
        $("#btnAcceptDiscount").on("click", UnderwritingDiscounts.addDiscountList);
        $("#dllDiscountsParametrization").on("itemSelected", UnderwritingDiscounts.getDiscountsType);
        $("#btnDiscountSave").on("click", UnderwritingDiscounts.ExecuteOperationDiscount);
    }

    static DiscountLoad() {
        if (glbPolicy.Id === 0) {
            if ($("#formUnderwriting").valid()) {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageValidateTemp, 'autoclose': true });
            }
        }
        else {
            Underwriting.ShowPanelsIssuance(MenuType.Discounts);
        }
        
    }

    static ClearDiscounts() {
        $("#dllDiscountsParametrization").UifSelect("setSelected", null);
        $("#inputTaxeType").val("");
        $("#inputTaxe").val("");
        $("#inputTotalDiscount").val("Total Descuento");
    }

    static changeDiscountssParametrization(data) {
        glbDiscount = data;
        $("#dllDiscountsParametrization").UifSelect({ sourceData: data });
    }

    static getDiscountsType(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var object = glbDiscount.find(x => x.Id === Number(selectedItem.Id)).TaxeType;
            switch (object) {
                case 1:
                    $("#inputTaxeType").val("PORCENTAJE");
                    break;
                case 2:
                    $("#inputTaxeType").val("MILAJE");
                    break;
                case 3:
                    $("#inputTaxeType").val("IMPORTE");
                    break;
            }
        }
    }

    static addDiscountList() {
        $("#formDiscounts").validate();
        if ($("#formDiscounts").valid()) {
            var IdDiscount = $("#dllDiscountsParametrization").UifSelect("getSelected");
            var DescriptionDiscount = glbDiscount.find(x => x.Id === Number(IdDiscount)).Description;
            var TaxeTypeId = glbDiscount.find(x => x.Id === Number(IdDiscount)).TaxeType;
            var TaxeTypeDescription = $("#inputTaxeType").val();
            var DiscountNew = {
                Id: IdDiscount, 
                Description: DescriptionDiscount,
                TaxeType: TaxeTypeId,
                TaxeTypeDescription: $("#inputTaxeType").val(), 
                Taxe: $("#inputTaxe").val(),
                StatusTypeService : ParametrizationStatus.Create
            };
            $("#listViewDiscounts").UifListView("addItem", DiscountNew);
            UnderwritingDiscounts.ClearDiscounts();
        }
    }

    static deleteDiscountList(deferred, result) {
        deferred.resolve();
        var DiscountList = $("#listViewDiscounts").UifListView("getData");
        if (result.Id !== "" && result.Id !== undefined) {
            result.StatusTypeService = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listViewDiscounts").UifListView("addItem", result);
        }
    }

    static ExecuteOperationDiscount() {
        var item = $("#listViewDiscounts").UifListView("getData");
        //item[0].totalDiscounts = 0;
        request("Underwriting/Underwriting/SaveDiscounts", JSON.stringify({ temporalId: glbPolicy.Id, discounts: item }), "POST", "Error al " + item.StatusTypeService, UnderwritingDiscounts.ComfirmExecuteOperationDiscount);   
    }

    static ComfirmExecuteOperationDiscount(data) {
        if (data.Message === null) {
            data.Message = 0;
        }
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate + ':<br>' +
                "Agregados" + ':' + data.TotalAdded + '<br> ' +
                "Actualizados" + ':' + data.TotalModified + '<br> ' +
                "Eliminados" + ':' + data.TotalDeleted + '<br> ' +
                "Errores" + ':' + data.Message,
            'autoclose': true
        });
    }
}