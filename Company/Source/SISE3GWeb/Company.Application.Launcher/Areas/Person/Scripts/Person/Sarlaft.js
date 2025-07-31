var Sarlafs = [{ FinancialSarlaft: {} }];
var SarlafstTmp;
var SarlaftId = 0;
var SarlaftRowId = -1;
var ForeingCurrency = 0;
var heightListViewSarlaft = 256;

class Sarlaft extends Uif2.Page {
    getInitialState() {
        Sarlaft.unBindEvents();
        Sarlaft.InitializeSarlaft();
    }
    //Seccion Eventos
    bindEvents() {
        
        $("#btnNewSarlaft").click(this.NewSarlaft);
        $("#btnCreateSarlaft").click(this.BtnCreateSarlaft);
        $("#btnAcceptSarlaft").click(this.CreateSarlaft);
        $("#btnCancelSarlaft").click(this.CancelSarlaft);
        $('#listSarlaft').on('rowEdit', this.SarlaftEdit);
       
       $("#chkTransactionsForeignCurrencies").change(this.TransactionsForeignCurencies);

        $("#InputIncome").focusin(this.NotFormatMoneyIn);
        $("#InputTotalLiabilities").focusin(this.NotFormatMoneyIn);
        $("#InputExpenses").focusin(this.NotFormatMoneyIn);
        $("#InputTotalAssets").focusin(this.NotFormatMoneyIn);
        $("#InputOtherIncomeValue").focusin(this.NotFormatMoneyIn);
        $("#InputDetailsOtherOperations").focusin(this.NotFormatMoneyIn);

        $("#InputIncome").focusout(this.FormatMoneyOut);
        $("#InputExpenses").focusout(this.FormatMoneyOut);
        $("#InputTotalAssets").focusout(this.FormatMoneyOut);
        $("#InputTotalLiabilities").focusout(this.FormatMoneyOut);
        $("#InputOtherIncomeValue").focusout(this.FormatMoneyOut);
        $("#InputDetailsOtherOperations").focusout(this.FormatMoneyOut);
    }

    static unBindEvents() {
        $("#btnNewSarlaft").unbind();
        $("#btnCreateSarlaft").unbind();
        $("#btnAcceptSarlaft").unbind();
        $("#btnCancelSarlaft").unbind();
        $('#listSarlaft').unbind();

        $("#chkTransactionsForeignCurrencies").unbind();

        $("#InputIncome").unbind();
        $("#InputTotalLiabilities").unbind();
        $("#InputExpenses").unbind();
        $("#InputTotalAssets").unbind();
        $("#InputOtherIncomeValue").unbind();
        $("#InputDetailsOtherOperations").unbind();

        $("#InputIncome").unbind();
        $("#InputExpenses").unbind();
        $("#InputTotalAssets").unbind();
        $("#InputTotalLiabilities").unbind();
        $("#InputOtherIncomeValue").unbind();
        $("#InputDetailsOtherOperations").unbind();
    }

    TransactionsForeignCurencies() {
        if ($('#chkTransactionsForeignCurrencies').prop('checked')) {
            $("#InputDetailsOtherOperations").val("").prop('disabled', false);
        }
        else {
            $("#InputDetailsOtherOperations").val("").prop('disabled', true);
        }
    }
 
    static InitializeSarlaft() {
        $("#InputDetailsOtherOperations").prop('disabled', 'disabled');
       
    }

    

    static ClearSarlaft(enabledDisabled) {
        SarlaftId = 0;
        //SarlaftRowId = -1;
        $("#InputIncome").val("").prop('disabled', enabledDisabled);
        $("#InputExpenses").val("").prop('disabled', enabledDisabled);
        $("#InputTotalAssets").val("").prop('disabled', enabledDisabled);
        $("#InputTotalLiabilities").val("").prop('disabled', enabledDisabled);
        $("#InputOtherIncomeValue").val("").prop('disabled', enabledDisabled);
        $("#InputDetailsOtherOperations").val("").prop('disabled', enabledDisabled);
        $("#InputOtherIncomeConcepts").val("").prop('disabled', enabledDisabled);
        $("#chkTransactionsForeignCurrencies").prop("disabled", enabledDisabled);
        $("#chkTransactionsForeignCurrencies").prop("checked", enabledDisabled);
        if ($("#chkTransactionsForeignCurrencies").is(':checked')) {
            $("#InputDetailsOtherOperations").val("").prop('disabled', false);
        }
        else {
            $("#InputDetailsOtherOperations").val("").prop('disabled', true);
        }
        
    }

    NewSarlaft() {
        Sarlaft.ClearSarlaft(false);
    }

    BtnCreateSarlaft() {
        if (Sarlaft.ValidateSarlaft()) {
            Sarlaft.CreateSarlaft();
            Sarlaft.ClearSarlaft(false);
        }
    }

    AcceptSarlaft() {
        Sarlaft.SaveSarlaft();
        Persons.AddSubtitlesRightBar();
        Sarlaft.ClearSarlaft(false);
    }

    CancelSarlaft() {
        Sarlaft.ClearSarlaft(false);
    }

    SarlaftEdit(event, data, index) {
        Sarlaft.ClearSarlaft(true);
        SarlaftRowId = index;
        Sarlaft.EditSarlaft(data, index);
    }

    TransactionsForeignCurrencies() {
        if ($(this).is(':checked')) {
            $("#InputDetailsOtherOperations").prop('disabled', false);
        }
        else {
            $("#InputDetailsOtherOperations").prop('disabled', true);
        }
    }

    NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    FormatMoneyOut() {
        $(this).val(FormatMoney($(this).val()));
    }

    
    

    static CleanObjectsSarlaft() {
        Sarlafs = [];
        $("#listSarlaft").UifListView({ source: null, customDelete: false, customAdd: false, customEdit: true, add: false, edit: true, delete: false, displayTemplate: "#sarlaftTemplate", height: heightListViewSarlaft });
    }

    static ValidateSarlaft() {
        var error = "";
        if ($("#InputIncome").val() == "" || $("#InputIncome").val() <= 0) {
            error = error + "Ingresos <br>";
        }
        if ($("#InputExpenses").val() == "" || $("#InputExpenses").val() <= 0) {
            error = error + "Egresos <br>";
        }
        if ($("#InputTotalAssets").val() == "" || $("#InputTotalAssets").val() <= 0) {
            error = error + "Total Activos <br>";
        }
        if ($("#InputTotalLiabilities").val() == "" || $("#InputTotalLiabilities").val() <= 0) {
            error = error + "Total Pasivos <br>";
        }
        if ($('#chkTransactionsForeignCurrencies').prop('checked')) {

            if ($("#InputDetailsOtherOperations").val() == "" || $("#InputDetailsOtherOperations").val() <= 0) {
                error = error + "Monto otras operaciones <br>";
            }
        }

        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + "<br>" + error })
            return false;
        }
        return true;
    }

    //seccion Creacion y Grabado
    static CreateSarlaft() {
        var SarlaftTmp = Sarlaft.CreateSarlaftModel();
        if (SarlaftTmp.Id > 0) {
            SarlaftTmp.StatusTypeService = ParametrizationStatus.Update;
            $("#listSarlaft").UifListView("editItem", SarlaftRowId, SarlaftTmp);
            
        }
        else {
            SarlaftTmp.StatusTypeService = ParametrizationStatus.Create;
            $("#listSarlaft").UifListView("addItem", SarlaftTmp);
        }
    }

    static CreateSarlaftModel() {
        var Sarlaft = { finacialSarlaft: {}};
        Sarlaft.Id = SarlaftId;
        Sarlaft.RegistrationDate = DateNowPerson;
        Sarlaft.finacialSarlaft.SarlaftId = SarlaftId;
        Sarlaft.finacialSarlaft.IncomeAmount = parseInt($("#InputIncome").val().replace(new RegExp(/\.+/g), ""));
        Sarlaft.finacialSarlaft.ExpenseAmount = parseInt($("#InputExpenses").val().replace(new RegExp(/\.+/g), ""));        
        Sarlaft.finacialSarlaft.ExtraIncomeAmount = ($("#InputOtherIncomeValue").val() == "") ? 0 : parseInt($("#InputOtherIncomeValue").val().replace(new RegExp(/\.+/g), ""));
        Sarlaft.finacialSarlaft.AssetsAmount = parseInt($("#InputTotalAssets").val().replace(new RegExp(/\.+/g), ""));
        Sarlaft.finacialSarlaft.LiabilitiesAmount = parseInt($("#InputTotalLiabilities").val().replace(new RegExp(/\.+/g), ""));
        Sarlaft.finacialSarlaft.Description = ($("#InputOtherIncomeConcepts").val() == "") ? " " : $("#InputOtherIncomeConcepts").val();
        Sarlaft.finacialSarlaft.IncomeAmount = parseInt($("#InputIncome").val().replace(new RegExp(/\.+/g), ""));
        Sarlaft.finacialSarlaft.ForeignTransactionAmount = parseInt($("#InputDetailsOtherOperations").val().replace(new RegExp(/\.+/g), ""));
        Sarlaft.StatusTypeService = $("#StatusTypeService").val();
        if ($("#chkTransactionsForeignCurrencies").is(':checked')) {
            Sarlaft.finacialSarlaft.IsForeignTransaction = true;
        }
        else {
            Sarlaft.finacialSarlaft.IsForeignTransaction = false;
        }
        Sarlaft.ActivityEconomic = $('#inputeconomyActivityPn').data('Id');
        Sarlaft.IndividualId = $('#lblPersonCode').val();
        return Sarlaft;
    }

    static SaveSarlaft() {
        Sarlaft.SendSarlaft();
        Sarlaft.CloseSarlaft();
    }


    static SendSarlaft() {
        if ($("#listSarlaft").UifListView('getData').length > 0) {
            Sarlafs = $("#listSarlaft").UifListView('getData');
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorEmpty, 'autoclose': false })
        }
    }

    //Cerrar
    static CloseSarlaft() {
        $('#modalSarlaft').UifModal('hide');
    }

    static FormatSarlaft() {
        $.each(Sarlafs, function (index, item) {
            item.RegistrationDate = FormatDate(item.RegistrationDate);
            if (item.finacialSarlaft != null) {
                item.finacialSarlaft.IncomeAmount = FormatMoney(item.finacialSarlaft.IncomeAmount);
                item.finacialSarlaft.ExpenseAmount = FormatMoney(item.finacialSarlaft.ExpenseAmount);
                item.finacialSarlaft.ExtraIncomeAmount = FormatMoney(item.finacialSarlaft.ExtraIncomeAmount);
                item.finacialSarlaft.AssetsAmount = FormatMoney(item.finacialSarlaft.AssetsAmount);
                item.finacialSarlaft.LiabilitiesAmount = FormatMoney(item.finacialSarlaft.LiabilitiesAmount);
                item.finacialSarlaft.IncomeAmount = FormatMoney(item.finacialSarlaft.IncomeAmount);
                item.finacialSarlaft.ForeignTransactionAmount = FormatMoney(item.finacialSarlaft.ForeignTransactionAmount);
            }

        });
    }
    static UnFormatSarlaft() {
        $.each(Sarlafs, function (index, item) {
            if (item.finacialSarlaft != null) {
                item.finacialSarlaft.IncomeAmount = NotFormatMoney(item.finacialSarlaft.IncomeAmount);
                item.finacialSarlaft.ExpenseAmount = NotFormatMoney(item.finacialSarlaft.ExpenseAmount);
                item.finacialSarlaft.ExtraIncomeAmount = NotFormatMoney(item.finacialSarlaft.ExtraIncomeAmount);
                item.finacialSarlaft.AssetsAmount = NotFormatMoney(item.finacialSarlaft.AssetsAmount);
                item.finacialSarlaft.LiabilitiesAmount = NotFormatMoney(item.finacialSarlaft.LiabilitiesAmount);
                item.finacialSarlaft.IncomeAmount = NotFormatMoney(item.finacialSarlaft.IncomeAmount);
                item.finacialSarlaft.ForeignTransactionAmount = NotFormatMoney(item.finacialSarlaft.ForeignTransactionAmount);
            }
        });
    }

    //seccion edit
    static EditSarlaft(data, index) {
       
        SarlaftId = data.Id;
        $("#InputIncome").val(FormatMoney(data.finacialSarlaft.IncomeAmount));
        $("#InputExpenses").val(FormatMoney(data.finacialSarlaft.ExpenseAmount));
        $("#InputTotalAssets").val(FormatMoney(data.finacialSarlaft.AssetsAmount));
        $("#InputTotalLiabilities").val(FormatMoney(data.finacialSarlaft.LiabilitiesAmount));
        $("#InputOtherIncomeValue").val(FormatMoney(data.finacialSarlaft.ExtraIncomeAmount));
        $("#InputOtherIncomeConcepts").val(FormatMoney(data.finacialSarlaft.Description));
        $("#SarlaftId").val(data.finacialSarlaft.SarlaftId);
        $("#InputDetailsOtherOperations").val(FormatMoney(data.finacialSarlaft.ForeignTransactionAmount));
        $("#InputDetailsOtherOperations").prop('disabled', true);
        if (data.IsForeignTransaction) {
            $('#chkTransactionsForeignCurrencies').prop('checked', true);
        }
        else {
            $('#chkTransactionsForeignCurrencies').prop('checked', false);
        }

    }
    static LoadSarlaft(SarlaftNew) {
        var promise = new Promise(function (resolver, rechazar) {
            return $.ajax({
                type: 'POST',
                url: rootPath + 'Person/Person/GetPersonSarlaftByIndividualID',
                data: JSON.stringify({ IndividualId: SarlaftNew.IndividualId }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                //success: function (data) {
                //    Sarlaft.getSarlaft(data.result);

            }).done(function (data) {
                $.UifProgress('close');
                Sarlaft.getSarlaft(data.result);
                resolver();
            }).fail(function (data) {
                $.UifProgress('close');
                rechazar();
            });
           
        });
        return promise;
        
    }
    static getSarlaft(data) {

        $.each(data, function (index, item) {
            data[index].RegistrationDate = FormatDate(item.RegistrationDate);
        });
        $("#listSarlaft").UifListView({
            sourceData: data,
            customDelete: false,
            customAdd: false,
            customEdit: true,
            add: false,
            edit: true,
            delete: false,
            displayTemplate: "#sarlaftTemplate",
            height: heightListViewSarlaft

        });
        
    }

    CreateSarlaft() {
        
         var SarlaftNew = $("#listSarlaft").UifListView('getData');
         var SarlaftFilter = SarlaftNew.filter(function (item) {
             return item.StatusTypeService > 1;
         });
         if (individualId > 0) {
             if (SarlaftFilter.length > 0) {
                 SarlaftFilter.forEach(function(item){
                     item.IndividualId = individualId;
                     item.ActivityEconomic = $("#inputCompanyEconomyActivity").data("Id");
                 })
                 Sarlaft.CreateSarlafRequest(SarlaftFilter).done(function (data) {
                     if (data.success) {
                         $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelSarlaftCreate, 'autoclose': true })
                     }
                 });
                 SarlafstTmp = null;
             }
         }
         else {
             if (SarlaftFilter.length > 0) {
                 SarlafstTmp = SarlaftFilter;
             }
         }
         $('#modalSarlaft').UifModal('hide'); 
         
         
    }

    static CreateSarlafRequest(SarlaftFilter) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Person/Person/CreatePersonNaturalSarlaft',
            data: JSON.stringify({ individualSarlaftDTO: SarlaftFilter, }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            //success: function (data) { Sarlaft.getSarlaft(data.result) },
        });
        
    }
    
}














