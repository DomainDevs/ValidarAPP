$.ajaxSetup({ async: false });
$(document).ready(function () {
    EventsTypeOfAssistance();
});
function EventsTypeOfAssistance() {
    //Guardar tipo asistencia
    $("#btnmodalTypeOfAssistanceSave").on("click", function () {
        SaveProductTypeOfAssistance();
        Product.pendingChanges = true;
        HidePanelsProduct(MenuProductType.TypeOfAssistance);
    });
    $("#btnmodalTypeOfAssistanceClose").on("click", function () {
        //clearchk();
        HidePanelsProduct(MenuProductType.TypeOfAssistance);
    });
}

function SaveProductTypeOfAssistance() {
    var assistances = $("#TableTypeOfAssistance").UifDataTable("getSelected");
    var fullAssistances = $("#TableTypeOfAssistance").UifDataTable("getData");
    if (assistances === null) {
        assistances = [];
    }
    if (Product.AssistanceType !== null && typeof Product.AssistanceType !== 'undefined') {
        $.each(Product.AssistanceType, function (key, value) {
            var ifExist = [];
            if (assistances.length > 0) {
                ifExist = assistances.filter(function (item) {
                    return item.Id === value.Id;
                });
            }
            var ifExistFull = fullAssistances.filter(function (item) {
                return item.Id === value.Id;
            });
            if (ifExist.length === 0 && ifExistFull.length > 0) {
                if (value.StatusTypeService === 1 || value.StatusTypeService === 4) {
                    value.StatusTypeService = 4;
                }
                else {
                    Product.AssistanceType.splice(key, 1);
                }
            }
            else {
                if (value.StatusTypeService === 4) {
                    value.StatusTypeService = 1;
                }
            }
        });
        if (assistances.length > 0) {
            $.each(assistances, function (key, value) {
                var ifExist = Product.AssistanceType.filter(function (item) {
                    return item.Id === value.Id;
                });
                if (ifExist.length === 0) {
                    Product.AssistanceType.push({
                        Id: value.Id,
                        Description: null,
                        StatusTypeService: 2
                    })
                }
            });
        }
    }
    else {
        
        if (assistances.length > 0) {
            Product.AssistanceType = [];
            $.each(assistances, function (key, value) {
                Product.AssistanceType.push({
                    Id: value.Id,
                    Description: null,
                    StatusTypeService: 2
                });

            });
        }
    }
    if (assistances.length>0) {
        $('#selectTypeOfAssistance').UifSelect("setSelected", assistances[0].Id);
    }
    else {
        $('#selectTypeOfAssistance').UifSelect("setSelected", "");
    }
}