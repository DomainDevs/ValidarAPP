var glbComponentParam;
class FinancialPlanParametrization extends Uif2.Page {
    getInitialState() {
        FinancialPlanParametrization.GetPaymentPlan();
        FinancialPlanParametrization.GetPaymentMethod();
        FinancialPlanParametrization.GetCurrencies();
        request('Parametrization/FinancialPlan/GetComponents', null, 'GET', AppResources.ErrorGetClausesCONNEX, FinancialPlanParametrization.LoadComponentInitial)   
        $("#StatusTypeService").val(ParametrizationStatus.Create);
    }
    bindEvents() {
        $("#btnExit").click(this.redirectIndex)
        //Asignar Todos
        $("#btnModalComponentAssignAll").click(this.CopyAllComponent);
        //Desasignar Todos
        $("#btnModalComponentDeallocateAll").click(this.DeallocateComponentAll);
        //Asignar Uno
        $("#btnModalComponentAssign").click(this.CopyComponentSelected);
        //Desasignar Uno
        $("#btnModalComponentDeallocate").click(this.DeallocateComponentSelect);
        $("#btnExport").click(this.sendExcelFinancialPlan);
        $("#btnCreateFinancialPlan").click(this.SaveFinancialPlan);
        $("#MinimunQuota").val("0")
        $("#MinimunQuota").prop("disabled", true);
        $('#IdPaymentPlan').on("itemSelected",this.PaymentPlan);
        $("#IdCurrency").on("itemSelected", FinancialPlanParametrization.SearchPaymentPlan);
        $("#StatusTypeService").val(ParametrizationStatus.Update);
        $("#IdPaymentMethod").on("itemSelected", this.itemSelectedIdPaymentMethod)
    }

    PaymentPlan() {
        $("#MinimunQuota").val("0");
        $("#IdPaymentMethod").UifSelect('setSelected', null);
        $("#IdCurrency").UifSelect('setSelected', null);
        FinancialPlanParametrization.LoadComponentInitial(glbComponentParam);        
    }

    itemSelectedIdPaymentMethod(e, selectedItem) {
        $("#IdCurrency").UifSelect("setSelected", null);
    }

    static SearchPaymentPlan(e, financialPlan) {

        if ($.trim($("#IdPaymentPlan").val()) != "" && $.trim($("#IdPaymentMethod").val()) && $.trim($("#IdCurrency").val())) {
            FinancialPlanParametrizationRequest.GetFinancialPlanForItems($("#IdPaymentPlan").val(), $("#IdPaymentMethod").val(), $("#IdCurrency").val()).done(function (data)
            {
                FinancialPlanParametrization.LoadComponentInitial(glbComponentParam);
                if (data.result.FinancialPlanServiceModels.length === 0) {
                    $.UifNotify("show", { 'type': "info", 'message': AppResources.ErrorConsultingFinancialPlan, 'autoclose': true });
                    $("#StatusTypeService").val(ParametrizationStatus.Create);
                    FinancialPlanParametrization.LoadComponentInitial(glbComponentParam);
                }
                
                else if (data.result.FinancialPlanServiceModels.length > 0)
                {
                    $("#Id").val(data.result.FinancialPlanServiceModels[0].Id);
                    $.each(data.result.FinancialPlanServiceModels[0].FirstPayComponentsServiceModel, function (index, item) {
                        const findFinancialPlan = function (element, index, array) {
                            return element.Id === item.ComponentId
                        }
                        const resultIndex = $("#listviewComponent").UifListView("findIndex", findFinancialPlan);
                        $("#listviewComponentAssing").UifListView({ sourceData: data.result.FinancialPlanServiceModels[0].FirstPayComponentsServiceModel, displayTemplate: "#tmpComponentForAssign", selectionType: 'multiple', height: 310, title: AppResources.ComponentNotDistribuitable, });
                        $("#listviewComponent").UifListView("deleteItem", resultIndex);
                        
                    });
                    $("#StatusTypeService").val(ParametrizationStatus.Update);

                }
            });
            
        }
    }

    static GetPaymentPlan() {
        FinancialPlanParametrizationRequest.GetPaymentPlan().done(response => {
            let result = response.result;
            if (response.success) {
                $("#IdPaymentPlan").UifSelect({ sourceData: result });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static GetPaymentMethod() {
        FinancialPlanParametrizationRequest.GetPaymentMethod().done(response => {
            let result = response.result;
            if (response.success) {
                $("#IdPaymentMethod").UifSelect({ sourceData: result });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }

    static GetCurrencies() {
        FinancialPlanParametrizationRequest.GetCurrencies().done(response => {
            let result = response.result;
            if (response.success) {
                $("#IdCurrency").UifSelect({ sourceData: result });

            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true });
            }
        });
    }
 


    //METODOS ADICIONALES

    static LoadComponentInitial(data) {
        glbComponentParam = data;
        $("#listviewComponent").UifListView({ sourceData: data, displayTemplate: "#tmpForComponent", selectionType: 'multiple', height: 310, title: AppResources.ComponentPremium});
        $("#listviewComponentAssing").UifListView({ sourceData: [], displayTemplate: "#tmpComponentForAssign", selectionType: 'multiple', height: 310, title: AppResources.ComponentNotDistribuitable });
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    CopyAllComponent() {
        var data = $("#listviewComponent").UifListView('getData');
        if (data.length > 0) {
            data = $("#listviewClausesAssing").UifListView('getData').concat(data);
            $("#listviewComponentAssing").UifListView({ sourceData: data, displayTemplate: "#tmpComponentForAssign", selectionType: 'multiple', height: 310, title: AppResources.ComponentNotDistribuitable });
            $("#listviewComponent").UifListView({ sourceData: [], displayTemplate: "#tmpForComponent", selectionType: 'multiple', height: 310, title: AppResources.ComponentPremium });
        }
    }
    DeallocateComponentAll() {
        FinancialPlanParametrization.LoadComponentInitial(glbComponentParam);
    }
    CopyComponentSelected() {
        var data = $("#listviewComponent").UifListView('getSelected');
        $.each(data, function (position, value) {
            var findIndex = function (element, index, array) {
                return element.Id === value.Id
            }
            var index = $("#listviewComponent").UifListView("findIndex", findIndex);
            $("#listviewComponent").UifListView("deleteItem", index);
            $("#listviewComponentAssing").UifListView("addItem", value);
        })
    }
    DeallocateComponentSelect() {
        var data = $("#listviewComponentAssing").UifListView('getSelected');
        $.each(data, function (position, value) {
            var findIndex = function (element, index, array) {
                return element.Id === value.Id
            }
            var index = $("#listviewComponentAssing").UifListView("findIndex", findIndex);
            $("#listviewComponentAssing").UifListView("deleteItem", index);
            value.IsMandatory = false;
            $("#listviewComponent").UifListView("addItem", value);
        });
    }
    sendExcelFinancialPlan(urlFile) {
        request('Parametrization/FinancialPlan/GenerateFileToExport', null, 'GET', AppResources.ErrorGenerateFileCONNEX, FinancialPlanParametrization.downloadFile);
    }

    static downloadFile(urlFile) {
        window.open(urlFile);
    }
    SaveFinancialPlan() {
        if ($("#formFinancialPlan").valid()) {
            var financial = $("#formFinancialPlan").serializeObject();
            financial.ListComponent = $("#listviewComponentAssing").UifListView('getData'); 
            var x = 0;
            for (x = 0; x < financial.ListComponent.length; x = x + 1){
                if (financial.ListComponent[x].Id === undefined)
                {
                    financial.ListComponent[x].Id = financial.ListComponent[x].ComponentId;
                }
                
            }
          
            request('Parametrization/FinancialPlan/ExecuteOperations', JSON.stringify({ financialPlan: financial }), 'POST', "Error", FinancialPlanParametrization.resultExecuteOpereations);
        }
    }

    static resultExecuteOpereations(data) {
        if (data.length === 1) {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.ErrorGeneralFinancialPlan, 'autoclose': true });
        }
        if (data === ParametrizationStatus.Create) {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.CreateFinancialPlan, 'autoclose': true });
        }
        if (data === ParametrizationStatus.Update) {
            $.UifNotify("show", { 'type': "info", 'message': AppResources.UpdateFinancialPlan, 'autoclose': true });
        }
        FinancialPlanParametrization.ClearForm();
    }

    static ClearForm() {
        $("#Id").val(null);
        $("#MinimunQuota").val("0");
        $("#IdPaymentPlan").UifSelect('setSelected', null);
        $("#IdPaymentMethod").UifSelect('setSelected', null);
        $("#IdCurrency").UifSelect('setSelected', null);
        $("#MinimunQuota").prop("disabled", true);
        FinancialPlanParametrization.LoadComponentInitial(glbComponentParam);
        ClearValidation("#formFinancialPlan");
    }
 
}