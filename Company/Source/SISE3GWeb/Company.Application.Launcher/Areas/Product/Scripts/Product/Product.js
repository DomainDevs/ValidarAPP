
var productData;
var ProductHold = null;
var Product = { Id: 0, IsNew: true, Currencies: [{ Id: 0, Description: null, StatusTypeService: 2 }], StatusTypeService: 2 }
var promise = null;
var heightAgent = 400;
var DeductByCoverProductModel = {};
var resultOperation;
var DeductiblesByCoverageFinal = [];
var dateNow;

$(document).ready(function () {
    Initialize();
    Events();
    ShowPanelsProduct(MenuProductType.Product);
    ValidateDateDisableProduct();

    $('#formProduct :input').change(function () {
        if (Product.StatusTypeService !== 2) {
            Product.StatusTypeService = 3
        }
        Product.pendingChanges = true;
    });
    $('#selectCurrency').on('itemSelected', function (event, selectedItem) {
        $(this).valid();
        if (Product.StatusTypeService !== 2) {
            Product.StatusTypeService = 3
        }
        Product.pendingChanges = true;
    });
    $('#selectPolicyType').on('itemSelected', function (event, selectedItem) {
        $(this).valid();
        if (Product.StatusTypeService !== 2) {
            Product.StatusTypeService = 3
        }
        Product.pendingChanges = true;
    });
    $('#selectRiskType').on('itemSelected', function (event, selectedItem) {
        $(this).valid();
        if (Product.StatusTypeService !== 2) {
            Product.StatusTypeService = 3
        }
        Product.pendingChanges = true;

        //if (Product.ProductCoveredRisks == null) {
        Product.ProductCoveredRisks = [];
        Product.ProductCoveredRisks.push(
            {
                Id: parseInt(selectedItem.Id),
                Description: selectedItem.Text,
                MaxRiskQuantity: 0,
                GroupCoverages: null,
                PreRuleSetId: null,
                RuleSetId: null,
                ScriptId: null,
                StatusTypeService: 2
            });
        //var data = $("#listModalRiskType").UifListView("getData");
        //if (data != null && data.length > 0) {

        //    CreateRiskType(Product.ProductCoveredRisks[0], Product.ProductCoveredRisks[0].Id)
        //}
        //else {
        //    $("#listModalRiskType").UifListView('addItem', Product.ProductCoveredRisks[0]);
        //}
        //}
    });
    $('#selectTypeOfAssistance').on('itemSelected', function (event, selectedItem) {
        if (Product.StatusTypeService !== 2) {
            Product.StatusTypeService = 3;
        }
        Product.pendingChanges = true;
    });
    $('#selectProduct2G').on('itemSelected', function (event, selectedItem) {
        if (Product.StatusTypeService !== 2) {
            Product.StatusTypeService = 3;
        }
        $(this).valid();
        Product.pendingChanges = true;
    });
});
function Initialize() {
    $("#inputDescription").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
    $("#inputProductAgentsPercentage").OnlyDecimals(2);
    $("#inputProductAgentsPercentageAdditional").OnlyDecimals(2);



    //$("#inputIncentiveAMT").OnlyDecimals(2);
    //$("#inputIncentiveAMT").val(1);
    //$("#chkPolicies").prop("checked", true);
    if (Product.Id == 0) {
        DisabledProduc(true);
        $("#btnReportProduct").prop("disabled", true);
    }
    LoadCurrency();

    NewProduct();
    DisabledProduc(true);
    Product.pendingChanges = false;
}
function DisabledProduc(control) {
    $("#btnProductEnableDisableBtn").prop("disabled", control);
}

function Events() {
    $('#selectPrefixCommercial').on('itemSelected', function (event, selectedItem) {
        if (selectedItem.Id > 0) {
            ClearPrefix();
            ClearObjectInsured(selectedItem.Id);
            GetPolicyTypesByPrefixId(selectedItem.Id);
            GetRiskTypeByPrefixId(selectedItem.Id, 0);
            GetProduct2GByPrefixId(selectedItem.Id, 0);
            //LoadTypeOfAssistance(selectedItem.Id, 0);
            $(this).valid();
            Product.pendingChanges = true;
        }
    });
    //Crear Producto
    $("#btnProductNew").on("click", function () {
        NewProduct();
        DisabledProduc(true);
        Product.pendingChanges = false;
        $("#btnReportProduct").prop("disabled", true);
    });
    $("#btnCancel").on("click", function () {
        CancelProduct();
    });
    $("#btnReturnProduct").on("click", function () {
        var indexSelectComercial = $("#selectPrefixCommercial").UifSelect("getSelected");
        if (indexSelectComercial > 0 && $("#inputDescription").val().trim() != "") {
            Product.pendingChanges = true;
        }
        if (Product.pendingChanges) {
            $.UifDialog('confirm', { 'message': '\n ' + Resources.Language.MessageExitProduct }, function (result) {
                if (result) {
                    window.location = rootPath + "Home/Index";
                }
            });
        }
        else {
            window.location = rootPath + "Home/Index";
        }
    });
    //moneda
    $("#btnCurrency").on("click", function () {
        if (typeof Product.Currencies !== null) {
            //$('#selectCurrency').UifSelect("setSelected", Product.Currencies[0].Id);
            $.each($("#TableCurrency").UifDataTable("getData"), function (key, value) {
                if (arrayFind(Product.Currencies, function (v) { return v.Id === value.Id && v.StatusTypeService !== 4; }) > -1) {
                    $('#TableCurrency tbody tr:eq(' + key + ')').addClass('row-selected');
                    $('#TableCurrency tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
                }
                else {
                    $('#TableCurrency tbody tr:eq(' + key + ')').removeClass('row-selected');
                    $('#TableCurrency tbody tr:eq(' + key + ') td button span').addClass('glyphicon-unchecked').addClass('glyphicon-check');
                }
            });
        }
        $("#modalCurrency").UifModal('showLocal', Resources.Language.TypesCurrencies);
    });
    //Evento Combos
    $('#selectCurrency').on('itemSelected', function (event, selectedItem) {
        if (selectedItem.Id > 0) {
            SetCurrency(selectedItem.Id);
        }
    });

    $('#selectTypeOfAssistance').on('itemSelected', function (event, selectedItem) {
        if (selectedItem.Id > 0) {
            SetTypeOfAssistance(selectedItem.Id);
        }
    });

    //Intermediarios y Comisiones
    $("#btnAgents").on("click", function () {

        if (Product.Id == 0 || Product.pendingChanges === true) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSaveNewProduct, 'autoclose': true });
        }
        else {
            Preloader(true);
            window.setTimeout(GetProductAgents, 1);
        }
    });


    function GetProductAgents() {

        enabledDisabledControl(true);
        ClearAgent();
        AgentDeleted = [];
        var CurrentWorkAgents = [];

        if (CurrentWorkAgents == null || CurrentWorkAgents.length == 0) {
            lockScreen();
            setTimeout(function () {
                ProductRequestT.GetProductAgentByProductId(Product.Id, $("#selectPrefixCommercial").UifSelect("getSelected")).done(function (response) {
                    unlockScreen();
                    if (response.success) {
                        var CurrentWorkAgents = response.result;

                        $("#listViewSummaryAgent").UifListView({
                            displayTemplate: "#summaryAgentTemplate",
                            edit: false,
                            delete: false,
                            customAdd: false,
                            customEdit: false,
                            height: 300,
                            filter: false
                        });
                        $("#listViewSummaryAgent").UifListView("addItem", CurrentWorkAgents);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    unlockScreen();
                    Preloader(false);
                    $.UifNotify('show', { 'type': 'info', 'message': 'Error get agents' });
                });
            },200);
        }

        ShowPanelsProduct(MenuProductType.Agents);
        //setCommionAgent();
        LoadProductAgent(CurrentWorkAgents);
        Preloader(false);
    }

    function Preloader(showPreloader) {
        if (typeof showPreloader === 'undefined') {
            showPreloader = true;
        }
        if (showPreloader) {
            $("#DivPreloader").show();
        }
        else {
            $("#DivPreloader").hide();
        }
    }

    //Datos Adicionales
    $("#btnAdditionalData").on("click", function () {
        if (Product.pendingChanges == false && Product.Id > 0) {
            //if (AddDataPrefixId > 0) {
            LoadTablesAdditional();
            ShowPanelsProduct(MenuProductType.AdditionalData);
            //} else {
            //    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessagePrefix, 'autoclose': true });
            //    }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSaveNewProduct, 'autoclose': true });
        }
    });
    //Grabar Producto
    $("#btnRecordProduct").on("click", function (eve) {
        eve.preventDefault();
        SaveProduct(true);
        ScrollTop();
        Product.pendingChanges = false;
    });
    //Seleccion tipo riesgo
    $('#selectRiskType').on('blur', function () {
        var selectedItem = $('#selectRiskType').val();
        if (selectedItem > 0) {
            $("#selectRiskType").prop("disabled", false);
            setRiskTypeMain();
            if ($('#listModalRiskType').UifListView('getData').length == 0) {

                $('#listModalRiskType').UifListView('addItem', { RiskTypeId: Product.ProductCoveredRisks[0].RiskTypeId, Description: Product.ProductCoveredRisks[0].Description, MaxRiskQuantity: Product.ProductCoveredRisks[0].MaxRiskQuantity });
            }
            else if ($('#listModalRiskType').UifListView('getData').length == 1) {
                $('#listModalRiskType').UifListView("clear");
                $('#listModalRiskType').UifListView('addItem', { RiskTypeId: Product.ProductCoveredRisks[0].RiskTypeId, Description: Product.ProductCoveredRisks[0].Description, MaxRiskQuantity: Product.ProductCoveredRisks[0].MaxRiskQuantity });
            }
            else {
                $("#selectRiskType").prop("disabled", true);
            }
        }
    });
    //Seleccion tipo poliza
    $('#selectPolicyType').on('blur', function () {
        var selectedItem = $('#selectPolicyType').val();
        if (selectedItem > 0) {
            $("#selectPolicyType").prop("disabled", false);
            setPolicyTypeMain();
            if ($('#listModalPolicyType').UifListView('getData').length == 0) {
                PolicyType(Product.policyTypes);
            }
            else if ($('#listModalPolicyType').UifListView('getData').length == 1) {
                $('#listModalPolicyType').UifListView("clear");
                PolicyType(Product.policyTypes);
            }
            else {
                $("#selectPolicyType").prop("disabled", true);
            }
        }
    });
    // Habilitar / Deshabilitar Producto
    $('#btnProductEnableDisableBtn').on('click', function () {
        ValidateDateDisableProduct();
        EnableDisabledProduct();
        Product.pendingChanges = true;
        if (Product.StatusTypeService === 1) {
            Product.StatusTypeService = 3;
        }
    });

    //Tipo de Asistencia
    //$("#btnTypeOfAssistance").on("click", function () {
    //    if (ValidateTypeOfAssistance()) {
    //        clearchk();
    //        if (Product.AssistanceType !== null && typeof Product.AssistanceType !== 'undefined') {
    //            $.each(Product.AssistanceType, function (index, value) {
    //                if (value.StatusTypeService !== 4) {
    //                    SetTypeOfAssistance(value.Id);
    //                }
    //            });
    //        }

    //        $("#modalTypeOfAssistance").UifModal('showLocal', Resources.Language.TypesOfAssistance);
    //    }


    //    var datos = document.getElementsByTagName("th");
    //    for (var i = 0, len = datos.length; i < len; i++) {
    //        datos[i].style.padding = "0";
    //    };
    //    var datos = document.getElementsByTagName("td");
    //    for (var i = 0, len = datos.length; i < len; i++) {
    //        datos[i].style.padding = "0";
    //    };

    //});

    //$("#chkIncentive").on("click", function () {
    //    if ($("#chkIncentive").is(":checked")) {
    //        $('#inputIncentiveAMT').UifInputSearch('disabled', false);
    //    }
    //    else {
    //        $('#inputIncentiveAMT').UifInputSearch('disabled', true);
    //        //$("#inputIncentiveAMT").val(FormatMoney(Number(0).toFixed(numberDecimal), numberDecimal));
    //    }
    //});

    $("#chkRcAdditional").on("click", function () {
        if (Product.Id == 0) {
            if ($("#chkRcAdditional").is(":checked")) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.RcAdditionalNotChange, 'autoclose': true });
            }
        }
        else {
            return false;
        }
    });

    $('#inputDisabledDate').blur(function () {
        ValidateDateDisableProduct();
    });
    $('#inputProduct').on('search', function (event, value) {
        $.UifProgress('show');
        ObjectProduct.eventProductInputProduct(value);
        $.UifProgress('close');
    });
    $('#tableResults tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        switch (modalListType) {
            case 1:
                GetAgenciesByAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML, Product.PrefixId);
                break;
            case 2:
                ObjectProduct.getMainProductByPrefixIdDescription("", $(this).children()[0].innerHTML);
                break;
            default:
                break;
        }
        $('#modalDialogList').UifModal("hide");
    });
    $('#btnReportProduct').click(GenerateFileToProduct);
    $('#btnListProduct').click(sendExcelProduct);
}
function GenerateFileToProduct() {
    if (Product != null && Product.Id != 0) {

        ProductRequestT.GenerateFileToProduct(Product.Id, $("#inputDescription").val().trim()).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    var a = document.createElement('A');
                    a.href = data.result.Url;
                    a.download = data.result.FileName;
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        })
    }

}

function sendExcelProduct() {

    ProductRequestT.GenerateFileToProducts().done(function (data) {
        if (data.success) {
            if (data.result != null) {
                DownloadFile(data.result);
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
        }
    })
}

function GetRiskTypeByPrefixId(prefixId, selectedId) {
    var controller = rootPath + 'Product/Product/GetRiskTypeByPrefixId?prefixId=' + prefixId;
    if (selectedId == 0) {
        $("#selectRiskType").UifSelect({ source: controller });
    }
    else {
        $("#selectRiskType").UifSelect({ source: controller, selectedId: selectedId });
    }
    if ($("#selectRiskType option[value != '']").length > 0) {
        $('#selectRiskType').UifSelect("setSelected", selectedId);

        if (Product.ProductCoveredRisks == null) {
            Product.ProductCoveredRisks = [];
            Product.ProductCoveredRisks.push(
                {
                    Id: parseInt($('#selectRiskType').UifSelect("getSelected")),
                    Description: $('#selectRiskType').UifSelect("getSelectedText"),
                    MaxRiskQuantity: 0,
                    GroupCoverages: null,
                    PreRuleSetId: null,
                    RuleSetId: null,
                    ScriptId: null,
                    StatusTypeService: 2
                });
            var data = $("#listModalRiskType").UifListView("getData");
            if (data != null && data.length > 0) {

                CreateRiskType(Product.ProductCoveredRisks[0], Product.ProductCoveredRisks[0].Id)
            }
            else {
                $("#listModalRiskType").UifListView('addItem', Product.ProductCoveredRisks[0]);
            }
        }
    }
    if ($("#selectRiskType option[value != '']").length < 1) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectRiskType, 'autoclose': true })
    }
}
function GetPolicyTypesByPrefixId(prefixId, selectedId) {
    var controller = rootPath + 'Product/Product/GetPolicyTypesByPrefixId?prefixId=' + prefixId;
    if (selectedId == 0) {
        $("#selectPolicyType").UifSelect({ source: controller });
    }
    else {
        $("#selectPolicyType").UifSelect({ source: controller, selectedId: selectedId });
    }
    if ($("#selectPolicyType option[value!='']").length == 1) {
        $('#selectPolicyType').UifSelect("setSelected", $("#selectPolicyType option:eq(1)").val());
    }
    if ($("#selectPolicyType option").length == 0) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorPolicyTypesForBranch, 'autoclose': true })
    }
}

function GetProduct2GByPrefixId(prefixId, selectedId) {
    var controller = rootPath + 'Product/Product/GetProducts2G?prefixId=' + prefixId;
    if (selectedId == 0) {
        $("#selectProduct2G").UifSelect({ source: controller, native: false, filter: true });
    }
    else {
        $("#selectProduct2G").UifSelect({ source: controller, selectedId: selectedId, native: false, filter: true });
    }
    if ($("#selectProduct2G option[value!='']").length == 1) {
        $('#selectProduct2G').UifSelect("setSelected", $("#selectProduct2G option:eq(1)").val());
    }
    //if ($("#selectProduct2G option").length == 0) {
    //    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorProducts2GForBranch, 'autoclose': true })
    //}
}

function GetPolicyTypesByProductId(productId) {
    var objectGetPolicyTypesByProductId = ObjectProduct.ajaxGetPolicyTypesByProductId(productId);
    objectGetPolicyTypesByProductId.done(function (data) {
        if (ObjectProduct.successAjax(data)) {
            $("#selectPolicyType").UifSelect({ sourceData: data.result });
            //Product.PolicyTypes = data.result;
            if ($("#selectPolicyType option[value!=''] ").length > 0) {
                $('#selectPolicyType').UifSelect("setSelected", $("#selectPolicyType option:eq(1)").val());
            }
            if ($("#selectPolicyType option").length == 0) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorProductPolicyTypes, 'autoclose': true })
            }
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryPolicyTypesByProductId, 'autoclose': true });
    });
}

function NewProduct() {
    ClearGlobals();
    Agent = [];
    Product = { Id: 0, IsNew: true, Currencies: [{ Id: 0, Description: null, StatusTypeService: 2 }], StatusTypeService: 2 }
    ClearControls();
    $('#formProduct')[0].reset();
    clearValidation('#formProduct');
    ClearNewProduct();

    ProductRequestT.DateNow().done(function (data) {
        $("#inputCurrentDate").UifDatepicker('setValue', FormatDate(data));
    });

    //$.post(rootPath + 'Product/Product/DateNow', function (data) {
    //    $("#inputCurrentDate").UifDatepicker('setValue', FormatDate(data));
    //    $("#inputDisabledDate").UifDatepicker('setValue', FormatDate(data));
    //});
}
function ClearControls() {
    $("#selectPrefixCommercial").UifSelect();
    $("#selectProduct2G").UifSelect('setSelected', null);
    $("#selectProduct2G").UifSelect();
    $("#inputProduct").val('');
    $("#selectSearchProduct").UifSelect();
    $("#inputDescription").val('');
    $("#inputDescription").text('');
    $("#inputDescriptionReduced").val('');
    $("#inputDescriptionReduced").text('');
    $("#inputCurrentDate").UifDatepicker('clear');
    $("#inputDisabledDate").UifDatepicker('clear');
    $("#inputProductAgentsPercentage").val('');
    $("#inputProductAgentsPercentage").text('');
    $("#inputProductAgentsPercentageAdditional").val('');
    $("#inputProductAgentsPercentageAdditional").text('');

    $("#selectCurrency").UifSelect('setSelected', null);
    $("#selectPolicyType").UifSelect();
    $("#selectRiskType").UifSelect();
    $("#inputMaximumNumberRisk").val('1');
    $('#chkLightCotizador').attr('checked', false);
    $('#chkCollective').attr('checked', false);
    $('#chkRequest').attr('checked', false);
    $('#chkFlatRate').attr('checked', false);
    $('#chkPremium').attr('checked', false);
    $('#chkRcAdditional').attr('checked', false);
    DisabledControls(false);

    $("#inputCurrentDate").UifDatepicker('disabled', true);

    ////Company Product
    //$("#inputIncentiveAMT").val('');
    //$("#inputIncentiveAMT").text('');
    //$("#chkPolicies").prop('checked', false);
    //$("#chkIncentive").prop('checked', false);
    //$("#chkCrediScore").prop('checked', false);
    //$("#chkTransitTaxes").prop('checked', false);
    //$("#chkFasecolda").prop('checked', false);
    //$("#inputValidDaysTempQuote").val('');
    //$("#inputValidDaysTempPolicy").val('');
    ////End Company Product
    ClearPartial();
}
function DisabledControls(disabled) {
    $("#selectPrefixCommercial").prop("disabled", disabled);
    $("#inputCurrentDate").UifDatepicker('disabled', disabled);
}
function ClearNewProduct() {
    $("#selectPrefixCommercial").prop("disabled", false);
}
function CancelProduct() {
    ClearGlobals();
    Product = { Id: 0, IsNew: false, Currencies: [{ Id: 0, Description: null, StatusTypeService: 2 }], StatusTypeService: 2 }
    Agent = [];
    ClearControls();
    $('#formProduct')[0].reset();
    clearValidation("#formProduct");
}
function SaveProduct(showAlert) {
    if (typeof showAlert === 'undefined') {
        showAlert = true;
    }
    try {
        $("#formProduct").validate();
        if ($("#formProduct").valid()) {

            if (ValidateProduct(Product)) {
                $('#btnRecordProduct').prop("disabled", true);

                if (Product.StatusTypeService === 2 || Product.StatusTypeService === 3) {
                    getFormProduct();
                }
                if (Product.CurrentDate) {
                    Product.CurrentDate = FormatDate(Product.CurrentDate);
                }
                productData = JSON.parse(JSON.stringify(Product));

                ProductRequestT.SaveProduct(productData).done(function (data) {
                    $('#btnRecordProduct').prop("disabled", false);
                    console.log(data);
                    if (data.success) {
                        ObjectProduct.fillViewMainProduct(data.result);

                        if (showAlert) {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SuccessSaveProduct, 'autoclose': true });
                        }
                        DisabledProduc(false);
                        ValidateDateDisableProduct();
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $('#btnRecordProduct').prop("disabled", false);
                    resultOperation = false;
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSaveProduct, 'autoclose': true })
                })

            }
            else {
                $('#btnRecordProduct').prop("disabled", false);
            }

        }
        else {
            $('#btnRecordProduct').prop("disabled", false);
        }
    } catch (e) {
        alert(e);
        $('#btnRecordProduct').prop("disabled", false);
    }

}

function getFormProduct() {
    var product = $("#formProduct").serializeObject();

    if ($("#inputDisabledDate").val()) {
        Product.DisabledDate = FormatDate($("#inputDisabledDate").val());
    }
    Product.PrefixId = parseInt($("#selectPrefixCommercial").UifSelect("getSelected"));
    Product.Description = $("#inputDescription").val();
    Product.DescriptionReduced = $("#inputDescriptionReduced").val();
    Product.Percentage = NotFormatMoney(product.Percentage, numberDecimal)
    //Check
    Product.IsGreen = $('#chkLightCotizador').is(':checked');
    Product.IsRequest = $('#chkRequest').is(':checked');
    Product.IsFlatRate = $('#chkFlatRate').is(':checked');
    Product.IsPremium = $('#chkPremium').is(':checked');
    Product.IsRcAdditional = $('#chkRcAdditional').is(':checked');
    //tipo de proceso - USP-2731
    Product.IsInteractive = $('#chkIsInteractive').is(':checked');
    Product.IsCollective = $('#chkIsCollective').is(':checked');
    Product.IsMassive = $('#chkIsMassive').is(':checked');

    if (typeof Product.PolicyTypes === 'undefined' || Product.PolicyTypes.Lenght === 0) {
        var PoliciTypeAdd = {
            Id: parseInt($('#selectPolicyType').UifSelect("getSelected")),
            Description: $("#selectPolicyType").UifSelect("getSelectedText"),
            IsDefault: true,
            PrefixId: $("#selectPrefixCommercial").UifSelect('getSelected'),
            StatusTypeService: 2
        };
        Product.PolicyTypes = [];
        Product.PolicyTypes.push(PoliciTypeAdd);
    }
    Product.CurrentDate = product.CurrentDate;
    Product.DisabledDate = product.DisabledDate;
    Product.Percentage = product.Percentage;
    Product.PercentageAdditional = product.PercentageAdditional;

    Product.AdditDisCommissPercentage = null;
    Product.Currency = 0;
    Product.ProductCoveredRisks[0].MaxRiskQuantity = $("#inputMaximumNumberRisk").val();
    Product.Product2G = $("#selectProduct2G").UifSelect('getSelected');
    Product.VersionId = parseInt($("#inputVersionId").val());
    if (Product.AssistanceType === null) {
        delete Product.AssistanceType;
    }

}

//function Mapping() {
//    //var itemSelected = $("#TableTypeOfAssistance").UifDataTable("getSelected");
//    var AssistanceType_ = [];

//    if (Product.AssistanceType != null) {
//        $.each(Product.AssistanceType, function (index, value) {
//            AssistanceType_.push({ Id: value.AssistanceCode, PrefixCode: $("#selectPrefixCommercial").UifSelect("getSelected") })
//        }
//        );
//    }
//    return AssistanceType_;
//}

//ocultar paneles
function HidePanelsProduct(Menu) {
    switch (Menu) {
        case MenuProductType.Product:
            break;
        case MenuProductType.Agents:
            $("#modalAgents").UifModal('hide');
            break;
        case MenuProductType.PaymentPlan:
            $("#modalPaymentPlan").UifModal('hide');
            break;
        case MenuProductType.CopyProduct:
            $("#modalCopyProduct").UifModal('hide');
            break;
        case MenuProductType.AdditionalData:
            $("#modalAdditionalData").UifModal('hide');
            break;
        case MenuProductType.Commission:
            $("#modalAgenciesAgentComission").UifModal('hide');
            break;
        case MenuProductType.Incentives:
            $("#modalAgenciesAgentIncentives").UifModal('hide');
            break;
        case MenuProductType.Currency:
            $("#modalCurrency").UifModal('hide');
            break;
        case MenuProductType.PolicyType:
            $("#modalPolicyType").UifModal('hide');
            break;
        case MenuProductType.RiskType:
            $("#modalRiskType").UifModal('hide');
            break;
        case MenuProductType.TypeOfAssistance:
            $("#modalTypeOfAssistance").UifModal('hide');
            break;
        default:
            break;
    }
}
function HidePanelsProducts() {
    $("#modalProduct").hide();
    $("#modalInsuredObject").hide();
    $("#modalCoverage").hide();
    $("#modalRulesAndscripts").hide();
    $("#modalAdditionalData").hide();
    $("#modalAgents").hide();
    $("#modalProductCommissions").hide();
    $("#buttonsProduct").hide();
    $("#buttonsInsuredObject").hide();
    $("#buttonsCoverage").hide();
    $("#buttonsRulesAndscripts").hide();
    $("#buttonsAdditionalData").hide();
    $("#buttonsAgents").hide();
    $("#buttonsProductCommissions").hide();
}
//Mostrar Paneles
function ShowPanelsProduct(Menu) {
    HidePanelsProducts();
    if (Menu != MenuProductType.Product) {
        if ($("#inputDescription").val() != "") {
            $("#globalText").html(Resources.Language.LabelProduct + ": " + $("#inputDescription").val());
        }
        $("#mainProduct").hide();
        $("#mainProductNew").show();
    }
    else {
        $("#mainProductNew").hide();
        $("#mainProduct").show();
    }

    switch (Menu) {
        case MenuProductType.Coverage:
            $("#modalCoverage").show();
            $("#buttonsCoverage").show();
            $.uif2.helpers.setGlobalTitle(Resources.Language.MessageTechnicalConditions);
            break;
        case MenuProductType.Rule:
            $("#modalRulesAndscripts").show();
            $("#buttonsRulesAndscripts").show();
            $.uif2.helpers.setGlobalTitle(Resources.Language.LabelRulesAndScripts);
            break;
        case MenuProductType.AdditionalData:
            hideModuleAdditionalData();
            $("#modalAdditionalData").show();
            $("#buttonsAdditionalData").show();
            $.uif2.helpers.setGlobalTitle(Resources.Language.LabelAdditionalData);
            break;
        case MenuProductType.Product:
            $("#modalProduct").show();
            $("#buttonsProduct").show();
            $.uif2.helpers.setGlobalTitle(Resources.Language.LabelProducts);
            break;
        case MenuProductType.InsuredObject:
            $("#modalInsuredObject").show();
            $("#buttonsInsuredObject").show();
            $.uif2.helpers.setGlobalTitle(Resources.Language.MessageObjectInsurance);
            break;
        case MenuProductType.Agents:
            $("#modalAgents").show();
            $("#buttonsAgents").show();
            $.uif2.helpers.setGlobalTitle(Resources.Language.LabelIntermediariesCommissions);
            break;
        case MenuProductType.ProductCommissions:
            $("#modalProductCommissions").show();
            $("#buttonsProductCommissions").show();
            $.uif2.helpers.setGlobalTitle(Resources.Language.LabelProductCommissions);
            break;
        default:
            break;
    }
}
function ClearPartial() {
    //Agentes
    clearValidation('#formProductAgents');
    $('#formProductAgents')[0].reset();
    //$("#listModalAgentsData").UifListView({ source: null, customAdd: false, customEdit: true, add: false, edit: true, delete: true, deleteCallback: deleteAgentCallback, displayTemplate: "#agencyTemplate", selectionType: 'single', height: heightAgent });
    //Comisiones
    $('#formAgenciesAgentComission')[0].reset();
    $("#listModalAgencyComissionData").UifListView({ source: null, customDelete: true, customAdd: false, customEdit: false, add: false, edit: false, delete: true, displayTemplate: "#agenciesAgentTemplate", selectionType: 'single', height: 310 });
    //Monedas   
    $('#TableCurrency').UifDataTable('clear');
    LoadCurrency();
    //Tipos de riesgo
    CleanObjectRiskType();
    //Tipos de Poliza   
    CleanObjectPolicyType();
    //Reglas y Scripts
    $('#formRulesAndScripts')[0].reset();
    $("#tableNone").UifDataTable('clear');
    //Planes de pago
    $('#formPaymenPlan')[0].reset();
    $("#listviewPaymentPlan").UifListView({ displayTemplate: "#tmpForPaymentPlan", selectionType: 'multiple', height: 310 });
    $("#listviewPaymentPlanAssing").UifListView({ displayTemplate: "#tmpForPaymentPlan", selectionType: 'multiple', height: 310 });
    //Coberturas
    $('#formCoverage')[0].reset();
    $("#listModalAssignCoverage").UifListView({ source: null, displayTemplate: "#coverageTemplate", edit: true, delete: true, customEdit: true, deleteCallback: DeleteCoverage, height: 400, selectionType: 'single', drag: true });
    ////Datos adicionales
    $("#tableProductAtivities").UifDataTable('clear');
    $("#tableProductLimitRC").UifDataTable('clear');
    $("#tableProductDeduct").UifDataTable('clear');
}
function SetCurrency(id) {
    $.each($("#TableCurrency").UifDataTable("getData"), function (key, value) {
        if (id == value.Id) {
            $('#TableCurrency tbody tr:eq(' + key + ')').addClass('row-selected');
            $('#TableCurrency tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
        }
    });
}
function LoadProductAgent(ProductAgents) {
    $.each(ProductAgents, function (index, item) {
        if (item.Status == 4) {
            AgentDeleted.push(item);
        }
        else {
            if (item.AgencyComiss != null && item.AgencyComiss.length > 0) {
                item.CommisionText = Resources.Language.labelDifferentialCommission;
            }
            else {
                item.CommisionText = "";
            }
            if (item.StatusTypeService == 1) {
                item.DataItem = '<div class="item"><div class="display columns"><div class="template">' +
                    '<div><strong>' + item.FullName + '</strong></div>' +
                    '<div><strong>' + item.CommisionText + '</strong></div>' +
                    '<div class="hidden">' + item.StatusTypeService + '</div>' +
                    '</div><div class="toolbar buttons"><div class="card-button edit-button" onclick="return false"><i class="fa fa-pencil"></i></div><div class="card-button delete-button" onclick="return false"><i class="fa fa-trash"></i></div></div></div></div>';
            }
            if (item.StatusTypeService == 2) {
                item.DataItem = '<div class="item"><div class="display columns"><div class="template">' +
                    '<div class="success"><strong>' + item.FullName + '</strong></div>' +
                    '<div><strong>' + item.CommisionText + '</strong></div>' +
                    '<div class="hidden">' + item.StatusTypeService + '</div>' +
                    '</div><div class="toolbar buttons"><div class="card-button edit-button" onclick="return false"><i class="fa fa-pencil"></i></div><div class="card-button delete-button" onclick="return false"><i class="fa fa-trash"></i></div></div></div></div>';
            }
            if (item.StatusTypeService == 3) {
                item.DataItem = '<div class="item"><div class="display columns"><div class="template">' +
                    '<div class="warning"><strong>' + item.FullName + '</strong></div>' +
                    '<div><strong>' + item.CommisionText + '</strong></div>' +
                    '<div class="hidden">' + item.StatusTypeService + '</div>' +
                    '</div><div class="toolbar buttons"><div class="card-button edit-button" onclick="return false"><i class="fa fa-pencil"></i></div><div class="card-button delete-button" onclick="return false"><i class="fa fa-trash"></i></div></div></div></div>';
            }
        }
    });
    $('#TableModalAgentsData').DataTable().clear();
    $('#TableModalAgentsData').UifDataTable({ sourceData: ProductAgents });
    $("#TableModalAgentsData").removeClass();
    $("#TableModalAgentsData > thead > tr > th").css('width', 'auto');
    $("#TableModalAgentsData").wrap('<div class="uif-listview"></div>');
    $("#TableModalAgentsData").wrap('<div class="list item-list"></div>');
    $("#TableModalAgentsData").wrap('<div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 300px"></div>');
    $('<div class="slimScrollRail" style="width: 14px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div >').insertAfter("#TableModalAgentsData");
    $('<div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 14px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 14px; z-index: 99; right: 1px; height: 90.4523px;"></div >').insertAfter("#TableModalAgentsData");
    $("#TableModalAgentsData").wrap('<div class="content" style="overflow: auto; width: auto; height: 300px;"></div>');
    $('#formProductAgents .panel-table div.panel-heading.panel-heading-table').remove();
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').find('.uif-col.col-md-6.col-sm-6.col-xs-6').wrap('<div class="row"></div>');
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').find('.uif-col.col-md-6.col-sm-6.col-xs-6').addClass("col-md-10 col-sm-10 col-xs-10");
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').find('.uif-col.col-md-6.col-sm-6.col-xs-6').removeClass("col-md-6 col-sm-6 col-xs-6");
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').find('.uif-col:lt(2)').wrapAll('<div class="row"></div>');
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').wrap('<div class="row"></div>');
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').prepend('<div class="row"></div>');
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').addClass("uif-col-12");
    $('.uif-col-6.fit-col-secondary').find('.row.tfooter').removeClass("row tfooter");


}
function ClearProduct() {
    ClearGlobals();
    $("#inputDescription").val('');
    $("#inputDescription").text('');
    $("#inputDescriptionReduced").val('');
    $("#inputDescriptionReduced").text('');
    $("#inputCurrentDate").UifDatepicker('clear');
    $("#inputDisabledDate").UifDatepicker('clear');
    $("#inputProductAgentsPercentage").val('');
    $("#inputProductAgentsPercentage").text('');
    $("#inputProductAgentsPercentageAdditional").val('');
    $("#inputProductAgentsPercentageAdditional").text('');

    $("#selectCurrency").UifSelect('setSelected', null);
    $("#selectPolicyType").UifSelect('setSelected', null);
    $("#selectRiskType").UifSelect('setSelected', null);
    $("#inputMaximumNumberRisk").val('1');
    $('#chkLightCotizador').attr('checked', false);
    $('#chkIsCollective').attr('checked', false);
    $('#chkRequest').attr('checked', false);
    $('#chkFlatRate').attr('checked', false);
    $('#chkPremium').attr('checked', false);
    $('#chkIsInteractive').attr('checked', false);
    $('#chkIsMassive').attr('checked', false);

    DisabledControls(false);
    ClearPartial();
}
function SetCurrency(id) {
    $.each($("#TableCurrency").UifDataTable("getData"), function (key, value) {
        if (id == value.Id) {
            $('#TableCurrency tbody tr:eq(' + key + ')').addClass('row-selected');
            $('#TableCurrency tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
        }
    });
}
function LoadCurrency() {
    var controller = rootPath + 'Product/Product/GetCurrenciesSelect'
    $("#selectCurrency").UifSelect({ source: controller });
    $("#selectCurrency").UifSelect("setSelected", GetParameterByDescription("Currency"));
    loadTableCurrency();
    $("#selectCurrency").prop("disabled", true);
    //Monedas   
    //var controller = rootPath + 'Product/Product/GetCurrencies'
    //$("#TableCurrency").UifDataTable({ source: controller });
    //SetCurrency(ProductCurrency.pesos);
}

function loadTableCurrency(currencyProduct) {
    let data = $.ajax({
        type: "POST",
        url: rootPath + 'Product/Product/GetCurrencies',
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });
    //data.responseJSON.success
    if (data.responseJSON.success) {
        let model = [];
        if (currencyProduct) {
            $.each(data.responseJSON.result, function (key, value) {
                let rowModel = new RowModelCurrency();
                rowModel.Id = value.Id;
                rowModel.Description = value.Description;
                rowModel.DecimalQuantity = 0;

                var currencyDecimal = currencyProduct.filter(v => v.Id === value.Id);

                if (currencyDecimal.Count > 0) {
                    rowModel.DecimalQuantity = currencyDecimal[0].DecimalQuantity
                }

                model.push(rowModel);
            });
        }
        else {
            $.each(data.responseJSON.result, function (key, value) {
                let rowModel = new RowModelCurrency();
                rowModel.Id = value.Id;
                rowModel.Description = value.Description;
                rowModel.DecimalQuantity = 1;
                model.push(rowModel);
            });
        }
        $("#TableCurrency").UifDataTable({ sourceData: model })

        $.each($("#TableCurrency").UifDataTable("getData"), function (key, value) {
            if (typeof currencyProduct !== "undefined" && currencyProduct.some(v => v.Id === value.Id)) {
                $('#TableCurrency tbody tr:eq(' + key + ')').addClass('row-selected');
                $('#TableCurrency tbody tr:eq(' + key + ') td button span').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
            }
        });
    } 
}

//Limpiar Objetos del seguro
function ClearObjectInsured(selectItem) {
    // Solo periferico de cobertura   
    $("#selectModalInsuredObject, #selectModalAssignCoverageInsuredObject, #selectModalAllyCoverageInsuranceObject").UifSelect({ source: null });
}
function ClearGlobals() {
    if (Product.IsNew) {
        Product = { Id: 0, IsNew: true, Currencies: [{ Id: 0, Description: null, StatusTypeService: 2 }], StatusTypeService: 2 }
    }
    else {
        Product = { Id: 0, IsNew: false, Currencies: [{ Id: 0, Description: null, StatusTypeService: 2 }], StatusTypeService: 2 }
    }
    //Limpiar Reglas
    var ProductRules = {};
    //Agentes
    Agent = [];
    //obejtos del seguro
    var modalCoverageRiskTypeList = [];
    modalCoverageGroupCoverageList = [];
    modalCoverageInsuredObjectsList = [];
    modalCoverageCoveragesList = [];
    var modalCoverageAllyCoveragesList = [];
    //Planes de pago
    PayMentPlan = null;
    PayMentPlanAssing = null;
    PayMentFinancialPlan = null;
    Product.pendingChanges = false;

    glbDeductiblesByPrefix = [];
    glbBeneficiarytypes = [];
    tempModalDeductibles = [];
    modalDeductibleByCoverageListTemp = [];
}
function ClearPrefix() {
    ClearGlobals();
    $("#selectPolicyType").UifSelect('setSelected', null);
    $("#selectRiskType").UifSelect('setSelected', null);
    ClearPartial();
}
//Extras
function clearValidation(formElement) {
    var validator = $(formElement).validate();
    $('[name]', formElement).each(function () {
        validator.successList.push(this);
        validator.showErrors();
    });
    validator.resetForm();
    validator.reset();
}
function arrayFind(arr, fn) {
    for (var i = 0, len = arr.length; i < len; ++i) {
        if (fn(arr[i])) {
            return i;
        }
    }
    return -1;
}
function replacer(key, value) {
    if (value === null) {
        return undefined;
    }
    return value;
}
function ValidateDateDisableProduct() {
    if ($('#inputDisabledDate').val() !== "") {
        $('#btnProductEnableDisable').text(Resources.Language.ControlButtonEnabled);
    } else {
        $('#btnProductEnableDisable').text(Resources.Language.ControlButtonDisabled);
    }
}
function ValidateProduct(Product) {
    ProductRequestT.DateNow().done(function (data) {
        dateNow = FormatDate(data);
    });
    var IsValid = true;
    if (!validateAllCurrenciesHasPlanPayment(Product.FinancialPlan)) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.WarningCurrencyTypePaymentPlan, 'autoclose': true });
        IsValid = false;
    }

    if (Product.ProductCoveredRisks == null || Product.ProductCoveredRisks[0].GroupCoverages == null || Product.ProductCoveredRisks[0].GroupCoverages.length == 0) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InfoMinCoverageProduct, 'autoclose': true });
        IsValid = false;
    }
    if ($("#inputDisabledDate").val() !== "") {
        if (CompareDates(FormatDate($("#inputDisabledDate").val()), dateNow) == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorFromDateToDate, 'autoclose': true });
            IsValid = false;
        }
        else if (CompareDates(FormatDate($("#inputCurrentDate").val()), FormatDate($("#inputDisabledDate").val())) == 1) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorFromDateToDate, 'autoclose': true });
            IsValid = false;
        }
    }
    return IsValid;
}
function EnableDisabledProduct() {
    if ($('#btnProductEnableDisable').text() == Resources.Language.ControlButtonEnabled) {
        $('#inputDisabledDate').UifDatepicker('clear');
        $('#btnProductEnableDisable').text(Resources.Language.ControlButtonDisabled);
    } else {
        $('#inputDisabledDate').prop("disabled", true);

        $.post(rootPath + 'Product/Product/DateNow', function (data) {
            $("#inputDisabledDate").UifDatepicker('setValue', FormatDate(data));
        });

        //$.post(rootPath + 'Product/Product/DateNow', function (data) {
        //    $("#inputDisabledDate").UifDatepicker('setValue', FormatDate(data));
        //});
        $('#btnProductEnableDisable').text(Resources.Language.ControlButtonEnabled);
    }
}


function GetParameterByDescription(description) {
    var dataResult = '';

    ProductRequestT.GetParameterByDescription(description).done(function (data) {
        if (data.success) {
            dataResult = data.result;
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'danger', 'message': Resources.Underwriting.ErrorSearchTemporary, 'autoclose': true })
    });
    return dataResult;
}

function GetProductsByPrefixIdByDescription(prefixId, description) {
    return ProductRequestT.GetProductsByPrefixIdByDescription(prefixId, description);
}

function ShowModalList(dataTable) {
    $('#tableResults').UifDataTable('clear');
    $('#tableResults').UifDataTable('addRow', dataTable);
}

var ObjectProduct = {
    eventProductInputProduct: function (value) {
        if (ObjectProduct.validateParametersQuery(value)) {
            ObjectProduct.getMainProductByPrefixIdDescription(value);
        }
        else {
            unlockScreen();
        }
    },
    getMainProductByPrefixIdDescription: function (description, productId) {
        if ($("#selectSearchProduct").UifSelect("getSelected") == "") var product = productId
        else product = $("#selectSearchProduct").UifSelect("getSelected")
        var productSearch = {
            Prefix: { Id: product },
            Description: description.trim()
        }
        glbPrefixId = productSearch.Prefix.Id;

        var productQuery = ObjectProduct.ajaxGetProductAdvancedSearch(productSearch);
        productQuery.done(function (data) {  
            unlockScreen();
            if (data.length == 0) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MessageNotFoundProducts, 'autoclose': true });
            } else {
                if (data.length > 1) {
                    ObjectProduct.switchGiveCountProduct(data);
                }
                if (data.length == 1) {
                    Product.IsNew = false;
                    ObjectProduct.loadMainProduct(data[0]);
                }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            unlockScreen();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryProductMain, 'autoclose': true })
        });
    },
    ajaxGetProductAdvancedSearch: function (productSearch) {
        return ProductRequestT.GetProductAdvancedSearch(productSearch);

    },
    switchGiveCountProduct: function (result) {
        if (result != null && result.length == 1) {
            Product.IsNew = false;
            ObjectProduct.loadMainProduct(result[0]);
        }
        else if (result != null && result.length > 1) {
            Product.IsNew = false;
            ObjectAdvanceSearch.LoadProductAdvanced(result);
            dropDownSearchProduct.show();
        }
    },
    validateParametersQuery: function (value) {
        if (value.length >= 3) {
            var bandarameters = true;
            if ($("#selectSearchProduct").UifSelect("getSelected") == "" || $("#selectSearchProduct").UifSelect("getSelected") == null) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorSelectPrefix });
                bandarameters = false;
            }
            else if (value.trim() == "") {
                bandarameters = false;
            }
            return bandarameters;
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.LabelMinimumThreeCharacters });
            bandarameters = false;
        }
    },
    mainProductByPrefixIdDescription: function (description, productId) {
        var productQuery = ObjectProduct.ajaxGetMainProductByPrefixIdDescriptionProductId(description, $("#selectSearchProduct").UifSelect("getSelected"), productId);
        productQuery.done(function (data) {
            if (ObjectProduct.successAjax(data)) {
                ObjectProduct.switchGiveCountProduct(data.result);
                //GetRiskTypeByPrefixId($("#selectPrefixCommercial").UifSelect("getSelected"), productId);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryProductMain, 'autoclose': true })
        });
    },

    ajaxGetMainProductByPrefixIdDescriptionProductId: function (description, prefixId, productId) {
        return ProductRequestT.GetMainProductByPrefixIdDescriptionProductId(description, prefixId, productId);

    },


    loadMainProduct: function (product) {
        ClearProduct();
        ObjectProduct.fillViewMainProduct(product);
        //product.ProductCoveredRisks[0].RiskTypeId = product.ProductCoveredRisks[0].CoveredRiskType;
        ObjectProduct.currencyProduct(product);

        $("#selectPolicyType").UifSelect({ sourceData: product.PolicyTypes });
        //Product.PolicyTypes = data.result;
        if ($("#selectPolicyType option[value!=''] ").length > 0) {
            $('#selectPolicyType').UifSelect("setSelected", $("#selectPolicyType option:eq(1)").val());
        }
        if ($("#selectPolicyType option").length == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorProductPolicyTypes, 'autoclose': true })
            $('#selectPolicyType').prop("disabled", false);
        }
        //GetPolicyTypesByProductId(product.Id);
        //ObjectProduct.adjustPolicyTypes();
        ObjectProduct.typeRisk();
        DisabledProduc(false);
        $("#btnReportProduct").prop("disabled", false);
        ObjectProduct.disabledMainProduct();
        ValidateDateDisableProduct();
        Product.pendingChanges = false;
        $("#formProduct").valid();
    },
    typeRisk: function () {
        //tipos de Riesgo
        if (Product.ProductCoveredRisks !== null && Product.ProductCoveredRisks.length > 0) {
            //$.each(Product.ProductCoveredRisks, function (key, value) {
            //    this.GroupCoverages = Product.GroupCoverages;
            //    this.RiskTypeId = value.CoveredRiskType;
            //});
            GetRiskTypeByPrefixId(Product.PrefixId, Product.ProductCoveredRisks[0].Id);
            $('#inputMaximumNumberRisk').val(Product.ProductCoveredRisks[0].MaxRiskQuantity);
            RiskType(Product.ProductCoveredRisks);
        }
    },
    disabledMainProduct: function () {
        //$("#inputDisabledDate").UifDatepicker('disabled', false);
        $("#selectPolicyType").prop("disabled", true);
        $("#selectRiskType").UifSelect("disabled", true);
        $("#selectPrefixCommercial").prop("disabled", true);
    },
    fillViewMainProduct: function (product) {
        Product = product;
        $("#inputDescription").val(product.Description);
        $("#inputDescriptionReduced").val(product.DescriptionReduced);
        $("#selectPrefixCommercial").UifSelect("setSelected", product.PrefixId);
        if (product.VersionId === 0) {
            $("#inputVersionId").val(1);
        }
        else {
            $("#inputVersionId").val(product.VersionId);
        }
        $("#inputCurrentDate").UifDatepicker('clear');
        $("#inputDisabledDate").UifDatepicker('clear');
        if (product.CurrentDate != null) {
            $("#inputCurrentDate").UifDatepicker('setValue', FormatDate(product.CurrentDate));
        }
        if (product.DisabledDate != null) {
            $("#inputDisabledDate").UifDatepicker('setValue', FormatDate(product.DisabledDate));
            $("#inputDisabledDate").UifDatepicker('disabled', true);
        } else {
            $("#inputDisabledDate").UifDatepicker('disabled', false);
        }
        $("#inputProductAgentsPercentage").val(FormatMoney(Number(product.Percentage).toFixed(numberDecimal), numberDecimal));
        $("#inputProductAgentsPercentageAdditional").val(FormatMoney(Number(product.PercentageAdditional).toFixed(numberDecimal), numberDecimal));

        $('#chkLightCotizador').prop('checked', false);
        $('#chkCollective').prop('checked', false);
        $('#chkRequest').prop('checked', false);
        $('#chkFlatRate').prop('checked', false);
        $('#chkPremium').prop('checked', false);
        $('#chkRcAdditional').prop('checked', false);

        if (product.IsGreen) {
            $('#chkLightCotizador').prop('checked', true)
        }
        if (product.IsCollective) {
            $('#chkIsCollective').prop('checked', true);
        }
        if (product.IsRequest) {
            $('#chkRequest').prop('checked', true);
        }
        if (product.IsFlatRate) {
            $('#chkFlatRate').prop('checked', true);
        }
        if (product.IsPremium) {
            $('#chkPremium').prop('checked', true);
        }
        if (product.IsRcAdditional) {
            $('#chkRcAdditional').prop('checked', true);
        }
        //Company product
        if (product.Product2G != null) {
            GetProduct2GByPrefixId(product.PrefixId, product.Product2G);
        }
        else {
            GetProduct2GByPrefixId(product.PrefixId, 0);
        }
        Product.pendingChanges = false;
        //tipo de proceso
        if (product.IsInteractive) {
            $('#chkIsInteractive').prop('checked', true);
        }
        if (product.IsMassive) {
            $('#chkIsMassive').prop('checked', true);
        }
    },
    ajaxGetProductAgentByProductId: function (productId) {
        return ProductRequestT.GetProductAgentByProductId(productId, $("#selectSearchProduct").UifSelect("getSelected"));

    },
    currencyProduct: function (product) {
        //Monedas
        if (typeof product.Currencies != 'undefined') {

            $('#selectCurrency').UifSelect("setSelected", product.Currencies[0].Id);
            
            loadTableCurrency(product.Currencies);
        }
    },
    ajaxGetCoveredRiskByProductId: function (productId) {
        return ProductRequestT.GetCoveredRiskByProductId(Product.Id);
    },
    ajaxGetDataAditionalByProductId: function (productId) {

        return ProductRequestT.GetDataAditionalByProductId(Product.Id);

    },
    ajaxGetPolicyTypesByProductId: function (productId) {
        return ProductRequestT.GetPolicyTypesByProductId(productIds);

    },
    //getCoveredRiskByProductId: function () {
    //    if (Product.ProductCoveredRisks == null) {
    //        var objectGetCoveredRiskByProductId = ObjectProduct.ajaxGetCoveredRiskByProductId();
    //        objectGetCoveredRiskByProductId.done(function (data) {
    //            if (ObjectProduct.successAjax(data)) {
    //                Product.GroupCoverages = data.result.GroupCoverages;
    //                Product.ProductCoveredRisks = data.result.ProductCoveredRisks;
    //            }
    //        }).fail(function (jqXHR, textStatus, errorThrown) {
    //            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryCoveredRiskByProductId, 'autoclose': true });
    //        })
    //    }
    //},
    adjustProductOld: function (product) {
        if (product.ProductOld != null) {
            Product.ProductOld.StandardCommissionPercentage = String(Product.ProductOld.StandardCommissionPercentage).replace(".", ",");
            product.ProductOld.StandardCommissionPercentage = String(product.ProductOld.StandardCommissionPercentage).replace(".", ",");

            Product.ProductOld.AdditionalCommissionPercentage = String(Product.ProductOld.AdditionalCommissionPercentage).replace(".", ",");
            product.ProductOld.AdditionalCommissionPercentage = String(product.ProductOld.AdditionalCommissionPercentage).replace(".", ",");

            if (Product.ProductOld.AdditionalCommissionPercentage == "null") {
                Product.ProductOld.AdditionalCommissionPercentage = null;
            }

            if (product.ProductOld.AdditionalCommissionPercentage == "null") {
                product.ProductOld.AdditionalCommissionPercentage = null;
            }


            product.ProductOld.CurrentFrom = FormatDate(product.ProductOld.CurrentFrom);
            product.ProductOld.CurrentTo = FormatDate(product.ProductOld.CurrentTo);

            Product.ProductOld.CurrentFrom = FormatDate(Product.ProductOld.CurrentFrom);
            Product.ProductOld.CurrentTo = FormatDate(Product.ProductOld.CurrentTo);
        }

    },
    adjustProductAgent: function (data) {
        CurrentWorkAgents = data;
        //Agentes
        if (CurrentWorkAgents != null) {
            $.each(CurrentWorkAgents, function (keyProductAgents, ProductAgents) {
                if (ProductAgents.AgencyComiss != null) {

                    //Product.ProductAgents[keyProductAgents].AgentCode = Product.ProductAgents[keyProductAgents].IndividualId;

                    $.each(ProductAgents.AgencyComiss, function (keyCommiss, Commiss) {
                        CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].PercentageAdditional = FormatMoney(Number(Commiss.AdditionalCommissionPercentage).toFixed(numberDecimal), numberDecimal);
                        CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].AdditionalCommissionPercentage;

                        CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].Percentage = FormatMoney(Number(Commiss.CommissPercentage).toFixed(numberDecimal), numberDecimal);
                        CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].CommissPercentage;

                        CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].SchCommissPercentage;
                        CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].StDisCommissPercentage;
                        CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].AdditDisCommissPercentage;

                        //Product.ProductAgents[keyProductAgents].ProductAgencyCommiss[keyCommiss].AgentCode = Commiss.AgencyId;
                        //delete Product.ProductAgents[keyProductAgents].ProductAgencyCommiss[keyCommiss].AgencyId;

                        //Product.ProductAgents[keyProductAgents].ProductAgencyCommiss[keyCommiss].AgentId = Commiss.IndividualId;
                        //delete Product.ProductAgents[keyProductAgents].ProductAgencyCommiss[keyCommiss].IndividualId;

                        //Product.ProductAgents[keyProductAgents].ProductAgencyCommiss[keyCommiss].AgentName = ProductAgents.FullName
                    });
                }
            });
        }
    },
    adjustPolicyTypes: function () {
        //tipo poliza      
        if (typeof Product.PolicyTypes != 'undefined' && Product.PolicyTypes.length > 0) {
            if (Product.Prefix != null) {
                GetPolicyTypesByPrefixId(Product.Prefix.Id, 0)
            }
            $.each(Product.PolicyTypes, function (key, value) {
                if (this.IsDefault) {
                    $('#selectPolicyType').UifSelect("setSelected", this.Id);
                }
            });
            PolicyType(Product.PolicyTypes);
        }
    },
    getProductAgents: function () {
        enabledDisabledControl(true);
        ClearAgent();
        AgentDeleted = [];
        var CurrentWorkAgents = [];

        if (CurrentWorkAgents == null || CurrentWorkAgents.length == 0) {

            ProductRequestT.GetProductAgentByProductId(Product.Id, $("#selectPrefixCommercial").UifSelect("getSelected")).done(function (response) {
                if (response.success) {
                    var CurrentWorkAgents = response.result;

                    $("#listViewSummaryAgent").UifListView({
                        displayTemplate: "#summaryAgentTemplate",
                        edit: false,
                        delete: false,
                        customAdd: false,
                        customEdit: false,
                        height: 300,
                        filter: false
                    });
                    $("#listViewSummaryAgent").UifListView("addItem", CurrentWorkAgents);
                    //if (CurrentWorkAgents != null) {
                    //    $.each(CurrentWorkAgents, function (keyProductAgents, ProductAgents) {
                    //        if (ProductAgents.AgencyComiss != null) {
                    //            $.each(ProductAgents.AgencyComiss, function (keyCommiss, Commiss) {
                    //                CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].PercentageAdditional = FormatMoney(Number(Commiss.AdditionalCommissionPercentage).toFixed(numberDecimal), numberDecimal);
                    //                CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].AdditionalCommissionPercentage;

                    //                CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].Percentage = FormatMoney(Number(Commiss.CommissPercentage).toFixed(numberDecimal), numberDecimal);
                    //                CurrentWorkAgents[keyProductAgents].AgencyComiss[keyCommiss].CommissPercentage;
                    //            });
                    //        }
                    //    });
                    //}
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': 'Error get agents' });
            });

        }
    },

    successAjax: function (data) {
        if (data.success) {
            return true
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
        }
    }


}

function tipoPropiedad(key) {
    if (Array.isArray(eval(key))) {
        revArray(key);
    }
    else if (typeof (eval(key)) === 'object') {
        revObjecto(key);
    }
    else {
        revFinal(key);
    }
}

function revArray(key) {
    for (var i = 0; i < eval(key).length; i++) {
        var propActual = key + '[' + i + ']';
        tipoPropiedad(propActual);
    }
}

function revObjecto(key) {
    for (var subKey in eval(key)) {
        var propActual = key + '["' + subKey + '"]';
        tipoPropiedad(propActual);
    }
}

function revFinal(key) {
    var valor = eval(key);
    var propObj = key;
    if (eval(key) === '31/12/0000') {
        eval(key + '="01/01/0001"');
        console.log("Si:" + valor + "--propiedad:" + propObj);

    }
    else if (key == null) {
        eval(key + '=""');
    }
}
function checkOnlyNumbersProduct(e, value) {
    var unicode = e.charCode ? e.charCode : e.keyCode;
    if (unicode > 31 && (unicode < 48 || unicode > 57))
        return false;
}

function checkOnlyNumbersLength(e, value) {
    //Valida Caracteres
    var unicode = e.charCode ? e.charCode : e.keyCode;
    if (unicode > 31 && (unicode < 48 || unicode > 57)) return false;
    if (document.getElementById('inputMaximumNumberRisk').value.length == 4) return false;
}