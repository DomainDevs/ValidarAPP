var PaymentMethodIndex = null;
var PaymentMethodStatus = null;
var PaymentMethodCode = null;
var glbPaymentMethodEdit = null;
var glbListPaymentMethodDelete = [];
$.ajaxSetup({ async: true });
//$(() => {
//    new PaymentMethodParametrization();
//});  


class PaymentMethodParametrization extends Uif2.Page {


    getInitialState() {
        
        $("#listPaymentMethods").UifListView({
            displayTemplate: "#templatePaymentMethod",
            source: null,
            selectionType: 'single',
            height: 350
        });
        request('Parametrization/PaymentMethod/GetPaymentMethods', null, 'GET', AppResources.ErrorSearchPaymentMethod, PaymentMethodParametrization.getPaymentMethods)        
                
    }



    bindEvents() {
        
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#listPaymentMethods').on('rowEdit', PaymentMethodParametrization.editPaymentMethod);
        $("#btnNewPaymentMethod").click(PaymentMethodParametrization.clearFormPaymentMethod);
        $("#btnAddPaymentMethod").click(this.addPaymentMethod);
        $("#btnCreatePaymentMethod").click(PaymentMethodParametrization.createPaymentMethod);
        $("#btnExport").click(this.exportExcel);
        $("#btnExit").click(this.redirectIndex);
        $('#PaymentMethodTypeId').val($("#PaymentMethodTypeId_Id").val());
        $('#inputPaymentMethod').on("buttonClick", PaymentMethodParametrization.getPaymentMethodsByDesc);

    }


    static getPaymentMethodsByDesc() {
        var inputPaymentMethod = $('#inputPaymentMethod').val();
        if (inputPaymentMethod.length < 3) {
            $.UifNotify('show', { type: 'info', message: Resources.Language.MessageInfoMinimumChar, autoclose: false });
        }
        else {
            $("#listPaymentMethods").UifListView({
                displayTemplate: "#templatePaymentMethod",
                source: null,
                selectionType: 'single',
                height: 350
            });
            request('Parametrization/PaymentMethod/GetPaymentMethodsByDescription', { description: $('#inputPaymentMethod').val() }, 'GET', AppResources.ErrorSearchPaymentMethod, PaymentMethodParametrization.getPaymentMethods)

        }

    }

    static getPaymentMethods(data) {
        $("#listPaymentMethods").UifListView({
            displayTemplate: "#templatePaymentMethod",
            sourceData: data,
            selectionType: 'single',
            height: 350,
            edit: true,
            delete: true,
            customEdit: true,
            deleteCallback: PaymentMethodParametrization.deleteCallbackListPaymentMethod,
        });
    }
    static editPaymentMethod(event, result, index) {
        
        PaymentMethodParametrization.clearFormPaymentMethod();
        glbPaymentMethodEdit = result;
        glbPaymentMethodEdit.Index = index;
        $("#Id").val(result.Id);
        $("#Description").val(result.Description);
        $("#SmallDescription").val(result.SmallDescription);
        $("#PaymentMethodTypeId_Id").val(result.PaymentMethodType.Id);
        
        PaymentMethodParametrization.buildHmtlDistribution();
    }
    addPaymentMethod() {
        
        
        if ($("#formPaymentMethod").valid()) {            
            var formPaymentMethod = $("#formPaymentMethod").serializeObject();
            formPaymentMethod.PaymentMethodType = { Id: $("#PaymentMethodTypeId_Id").val(), Description: $("#PaymentMethodTypeId_Id").UifSelect("getSelectedText") };
            if (formPaymentMethod.Id != "") {
                
                PaymentMethodParametrization.UpdatePaymentMethodParametrization(formPaymentMethod);
            }
            else {
                PaymentMethodParametrization.InsertPaymentMethodParametrization(formPaymentMethod);
            }
            PaymentMethodParametrization.clearFormPaymentMethod();
        }
    }    
   
    static clearFormPaymentMethod() {
        
        glbPaymentMethodEdit = null;
        $("#Id").val("");
        $("#Description").val("");
        $("#Description").focus();
        $("#SmallDescription").val("");
        $("#PaymentMethodTypeId_Id").val("");
        $("#PaymentMethodTypeId").val("");
        $("#PaymentMethodTypeDesc").val("");
        ClearValidation("#formPaymentMethod");
    }
    static UpdatePaymentMethodParametrization(formPaymentMethod) {
        
        formPaymentMethod.StatusTypeService = ParametrizationStatus.Update;
        $("#listPaymentMethods").UifListView("editItem", glbPaymentMethodEdit.Index, formPaymentMethod);
    }
    static InsertPaymentMethodParametrization(formPaymentMethod) {
        
        formPaymentMethod.StatusTypeService = ParametrizationStatus.Create;
        if (glbPaymentMethodEdit !== null)
        {
            $("#listPaymentMethods").UifListView("editItem", glbPaymentMethodEdit.Index, formPaymentMethod);
        }
        else
        {
            $("#listPaymentMethods").UifListView("addItem", formPaymentMethod);
        }
    }
    static deleteCallbackListPaymentMethod(deferred, result) {
        deferred.resolve();
        if (result.Id !== "" && result.Id !== undefined) //Se elimina unicamente si existe en DB
        {
            result.StatusTypeService = ParametrizationStatus.Delete;
            result.allowEdit = false;
            result.allowDelete = false;
            $("#listPaymentMethods").UifListView("addItem", result);
        }
        
        PaymentMethodParametrization.clearFormPaymentMethod();
    }
    static createPaymentMethod() {
        var listPaymentMethods = $("#listPaymentMethods").UifListView('getData');
        $.each(glbListPaymentMethodDelete, function (index,item) {
            listPaymentMethods.push(item);
        })
        var listPaymentMethods = listPaymentMethods.filter(function (item)
        {
            return item.StatusTypeService > 1;
        });
        if (listPaymentMethods.length > 0)
        {            
            request('Parametrization/PaymentMethod/SavePaymentMethod', JSON.stringify({ paymentMethodVM: listPaymentMethods }), 'POST', AppResources.ErrorSavePaymentMethod, PaymentMethodParametrization.confirmCreateParametrizationPaymentMethod);
           
        }
    }
    static confirmCreateParametrizationPaymentMethod(data) {
        
        glbListPaymentMethodDelete = [];
        PaymentMethodParametrization.clearFormPaymentMethod();
        request('Parametrization/PaymentMethod/GetPaymentMethods', null, 'GET', AppResources.ErrorSearchPaymentMethod, PaymentMethodParametrization.getPaymentMethods)
        
        if (data.Message === null)
        {
            data.Message = 0;
        }
        $.UifNotify('show', {
            'type': 'info', 'message': AppResources.MessageUpdate+':<br>' +
            AppResources.Aggregates + ':' + data.TotalAdded + '<br> ' +
            AppResources.Updated+':' + data.TotalModified + '<br> ' +
            AppResources.Removed+':' + data.TotalDeleted + '<br> ' +
            AppResources.Errors+':' + data.Message,
            'autoclose': true
        });
    }

    exportExcel() {
        request('Parametrization/PaymentMethod/GenerateFileToPaymentMethod', null, 'GET', AppResources.ErrorGeneratingExcelFile, PaymentMethodParametrization.generateFileToExport);
    }
    static generateFileToExport(data) {        
        DownloadFile(data);
    }
    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }
}