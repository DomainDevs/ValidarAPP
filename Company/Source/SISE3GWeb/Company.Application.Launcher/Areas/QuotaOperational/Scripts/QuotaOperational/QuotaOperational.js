class QuotaOperationalScripts extends Uif2.Page {

    getInitialState() {
        QuotaOperationalScripts.loadCombosModal();
        //$('#inputOperationalQuotaValue').OnlyDecimals(2);
    }

    bindEvents() {
        $('#inputHolder').on('buttonClick', QuotaOperationalScripts.SearchHolder);
        $('#tnExit').on('click', QuotaOperationalScripts.Exit);
        $('#btnDelete').on('click', QuotaOperationalScripts.DeleteQuotaOperation);
        $('#btnNew').on('click', QuotaOperationalScripts.showModalNew);
        $('#btnEdit').on('click', QuotaOperationalScripts.showModalEdit);
        $('#btnQuotaOperationalClose').on('click', QuotaOperationalScripts.hideModal);
        $('#btnQuotaOperationalSave').on('click', QuotaOperationalScripts.SaveQuotaOperation);
        $('#btnQuotaOperationalEdit').on('click', QuotaOperationalScripts.UpdateQuotaOperation);
        //$('#tableIndividualDetail tbody').on('click', 'tr', QuotaOperationalScripts.SetIndividualDetail);
        $('#btnIndividualSave').on('click', QuotaOperationalScripts.SetIndividualDetail)
    }

    static SearchHolder() {
        let dataList = [];
        if ($('#inputHolder').val().trim().length > 0) {
            QuotaOperationalRequestScript.GetInsuredsByDescriptionInsuredSearchTypeCustomerType($("#inputHolder").val().trim(), InsuredSearchType.DocumentNumber, CustomerType.Individual, 2).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputTomadoId').val(data.result[0].IdentificationDocument.Number);
                        $('#inputHolder').val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                        $('#inputHiddenIndividualId').val(parseInt(data.result[0].IndividualId));
                        $('#inputHiddenInsuredId').val(parseInt(data.result[0].InsuredId));
                        QuotaOperationalScripts.loadTableQuotaOperational(data.result[0].InsuredId, data.result[0].IndividualId);
                    }
                    else if (data.result.length > 1) {
                        for (var i = 0; i < data.result.length; i++) {
                            dataList.push({
                                Id: data.result[i].IndividualId,
                                CustomerType: data.result[i].CustomerType,
                                Code: data.result[i].InsuredId,
                                DocumentNum: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name,
                                CustomerTypeDescription: data.result[i].CustomerTypeDescription,
                                DocumentType: data.result[i].IdentificationDocument.DocumentType.Description
                            });
                        }
                        QuotaOperationalScripts.ShowModalHolderDetail(dataList);
                        $('#modalIndividualDetail').UifModal('showLocal', AppResources.LabelHolderDetail);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                        $('#tableResultQuotaOperational').UifDataTable("clear");
                        $('#inputTomadoId').val("");
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                    $('#tableResultQuotaOperational').UifDataTable("clear");
                    $('#inputTomadoId').val("");
                }
            });
        }
    }

    static Exit() {
        window.location = rootPath + 'Home/Index';
    }

    static DeleteQuotaOperation() {
        let documentNumber = $('#inputTomadoId').val();
        let selectValueTableQuotaOperational = $('#tableResultQuotaOperational').UifDataTable('getSelected');
        if (documentNumber != null && documentNumber != "") {
            if (selectValueTableQuotaOperational != null && selectValueTableQuotaOperational != "") {
                let currenciMoney = QuotaOperationalScripts.CurrencyMoney(selectValueTableQuotaOperational[0].Money);
                let lineBusiness = QuotaOperationalScripts.CurrencyLineBusiness(selectValueTableQuotaOperational[0].LineBussines);
                let objDeleteQuotaOperation = {};
                objDeleteQuotaOperation.CurrencyId = currenciMoney;
                objDeleteQuotaOperation.amountValue = selectValueTableQuotaOperational[0].QuotaOperational;
                objDeleteQuotaOperation.individualId = $('#inputHiddenIndividualId').val();
                objDeleteQuotaOperation.lineBusinessId = lineBusiness;
                QuotaOperationalRequestScript.DeleteQuotaOperation(objDeleteQuotaOperation).done(function (data) {
                    if (data.success) {
                        QuotaOperationalScripts.loadTableQuotaOperational($('#inputHiddenInsuredId').val(), $('#inputHiddenIndividualId').val());
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.DeleteQuotaOperationalSuccess, 'autoclose': true });
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.DeleteQuotaOperationalError, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.SelectQuotaOperational, 'autoclose': true });
            }
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Debe buscar un tomador', 'autoclose': true });
        }
    }

    static UpdateQuotaOperation() {
        let quotaOperationData = $('#formQuotaOperational').serializeObject();
        let individualId = $('#inputHiddenIndividualId').val();
        let documentNumber = $('#inputTomadoId').val();
        if (documentNumber != null && documentNumber != "") {
            let quotaOperationModel = {};
            quotaOperationModel.CurrencyId = quotaOperationData.Currency;
            quotaOperationModel.amountValue = NotFormatMoney(quotaOperationData.amountValue);
            quotaOperationModel.individualId = individualId;
            quotaOperationModel.lineBusinessId = quotaOperationData.PrefixId;
            quotaOperationModel.DateEnd = $('#inputDateEnd').val();
            QuotaOperationalRequestScript.UpdateQuotaOperation(quotaOperationModel).done(function (data) {
                if (data.success) {
                    QuotaOperationalScripts.hideModal();
                    QuotaOperationalScripts.loadTableQuotaOperational($('#inputHiddenInsuredId').val(), $('#inputHiddenIndividualId').val());
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.UpdateQuotaOperationalSuccess, 'autoclose': true });
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.UpdateQuotaOperationalError, 'autoclose': true });
                }
            });
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Debe buscar un tomador', 'autoclose': true });
        }
    }

    static SaveQuotaOperation() {
        if ($('#formQuotaOperational').valid()) {
            let quotaOperationData = $('#formQuotaOperational').serializeObject();
            let individualId = $('#inputHiddenIndividualId').val();
            let documentNumber = $('#inputTomadoId').val();
            if (documentNumber != null && documentNumber != "") {
                let quotaOperationModel = {};
                quotaOperationModel.CurrencyId = quotaOperationData.Currency;
                quotaOperationModel.amountValue = NotFormatMoney(quotaOperationData.amountValue);
                quotaOperationModel.individualId = individualId;
                quotaOperationModel.lineBusinessId = quotaOperationData.PrefixId;
                quotaOperationModel.DateEnd = $('#inputDateEnd').val();
                QuotaOperationalRequestScript.CreateQuotaOperation(quotaOperationModel).done(function (data) {
                    if (data.success) {
                        QuotaOperationalScripts.hideModal();
                        QuotaOperationalScripts.loadTableQuotaOperational($('#inputHiddenInsuredId').val(), $('#inputHiddenIndividualId').val());
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.SaveQuotaOperationalSuccess, 'autoclose': true });
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.SaveQuotaOperationalError, 'autoclose': true });
                    }
                });
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': 'Debe buscar un tomador', 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': 'Debe buscar un tomador', 'autoclose': true });
        }

    }

    static showModalNew() {
        let insuredName = $('#inputHolder').val().trim();
        $('#modalQuotaOperational').UifModal('showLocal', 'Cupo Operativo');
        $('#inputTomador').val(insuredName);
        $('#btnQuotaOperationalEdit').hide();
        $('#btnQuotaOperationalSave').show();
    }

    static showModalEdit() {
        let selectValueTableQuotaOperational = $('#tableResultQuotaOperational').UifDataTable('getSelected');
        if (selectValueTableQuotaOperational != "" && selectValueTableQuotaOperational != null) {
            $('#modalQuotaOperational').UifModal('showLocal', 'Cupo Operativo');
            $('#btnQuotaOperationalSave').hide();
            $('#btnQuotaOperationalEdit').show();
            QuotaOperationalScripts.loadModalControls(selectValueTableQuotaOperational[0]);
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.SelectQuotaOperational, 'autoclose': true });
        }
    }
    static hideModal() {
        $("#modalQuotaOperational").UifModal('hide');
        QuotaOperationalScripts.clearModalControls();
    }

    static CurrencyMoney(item) {
        let vcurrencyMoney = null;
        switch (item) {
            case 'Pesos':
                vcurrencyMoney = 0;
                break;
            case 'Dolares':
                vcurrencyMoney = 1;
                break;
            case 'Yenes':
                vcurrencyMoney = 2;
                break;
            case 'Euros':
                vcurrencyMoney = 3;
                break;
        }
        return vcurrencyMoney;
    }

    static CurrencyLineBusiness(item) {
        let vCurrencyLinebusiness = null;
        switch (item) {
            case 'Cumplimiento':
                vCurrencyLinebusiness = 2;
                break;
            case 'Vehiculo':
                vCurrencyLinebusiness = 7;
                break;
            case 'Caución Judicial':
                vCurrencyLinebusiness = 29;
                break;
        }
        return vCurrencyLinebusiness;
    }

    static loadModalControls(objQuotaronOperationModel) {
        let lineBusiness = QuotaOperationalScripts.CurrencyLineBusiness(objQuotaronOperationModel.LineBussines);
        let currenyMoney = QuotaOperationalScripts.CurrencyMoney(objQuotaronOperationModel.Money);
        let insuredName = $('#inputHolder').val().trim();
        $('#inputTomador').val(insuredName);
        $('#inputLineBusiness').UifSelect('setSelected', lineBusiness);
        $('#inputLineBusiness').UifSelect('disabled', true);
        $('#inputMoneyName').UifSelect('setSelected', currenyMoney);
        $('#inputMoneyName').UifSelect('disabled', true);
        $('#inputOperationalQuotaValue').val(objQuotaronOperationModel.QuotaOperational);              
        $('#inputDateEnd').val(objQuotaronOperationModel.CurrentTo);

    }

    static clearModalControls() {
        $('#inputTomador').val('');
        $('#inputLineBusiness').UifSelect('setSelected', null);
        $('#inputLineBusiness').UifSelect('disabled', false);
        $('#inputMoneyName').UifSelect('setSelected', null);
        $('#inputMoneyName').UifSelect('disabled', false);
        $('#inputOperationalQuotaValue').val('');
       
        $('#inputDateEnd').val('');

    }

    static loadCombosModal() {
        QuotaOperationalRequestScript.GetPrefixes().done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $('#inputLineBusiness').UifSelect({ sourceData: data.result });
                }
            }
        });

        QuotaOperationalRequestScript.GetCurrencies().done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $('#inputMoneyName').UifSelect({ sourceData: data.result });
                }
            }
        });

    }

    static loadTableQuotaOperational(objInsuredId, objIndividualId) {
        QuotaOperationalRequestScript.GetOperatingQuotaByIndividualId(objInsuredId, objIndividualId).done(function (data2) {
            if (data2.success) {
                var listQuotaOperational = [];
                if (data2.result.length >= 1) {
                    data2.result.forEach(function (item,index) {
                        var listItemQuotaOperational = {};
                        switch (item.LineBusinessId) {
                            case PrefixCollective.Surety:
                                listItemQuotaOperational.LineBussines = 'Cumplimiento';
                                break;
                            case PrefixCollective.Vehicle:
                                listItemQuotaOperational.LineBussines = 'Vehiculo';
                                break;
                            case PrefixCollective.Liability:
                                listItemQuotaOperational.LineBussines = 'Caución Judicial';
                                break;
                            default: 
                                listItemQuotaOperational.LineBussines = 'Otros ramos';
                                break;
                        }
                        switch (item.CurrencyId) {
                            case ProductCurrency.pesos:
                                listItemQuotaOperational.Money = 'Pesos';
                                break;
                            case ProductCurrency.dolares:
                                listItemQuotaOperational.Money = 'Dolares';
                                break;
                            case ProductCurrency.yenes:
                                listItemQuotaOperational.Money = 'Yenes';
                                break;
                            case ProductCurrency.euros:
                                listItemQuotaOperational.Money = 'Euros';
                                break;
                        }
                        listItemQuotaOperational.QuotaOperational = QuotaOperationalScripts.formatNumberMiles(item.Amount);
                        listItemQuotaOperational.CurrentTo = FormatDate(item.CurrentTo);
                        listQuotaOperational.push(listItemQuotaOperational);
                    });
                }
                $('#tableResultQuotaOperational').UifDataTable({ sourceData: listQuotaOperational });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSearchHolders, 'autoclose': true });
                $('#tableResultQuotaOperational').UifDataTable("clear");
                $('#inputTomadoId').val("");
            }
        });
    }

    static SetIndividualDetail() {
        let individualDetail = $('#tableIndividualDetail').UifDataTable('getSelected');
        $('#modalIndividualDetail').UifModal('hide');
        if (individualDetail.length > 0) {
            $('#inputTomadoId').val(individualDetail[0].DocumentNum);
            $('#inputHolder').val(individualDetail[0].Description + ' (' + individualDetail[0].DocumentNum + ')');
            $('#inputHiddenIndividualId').val(parseInt(individualDetail[0].Id));
            $('#inputHiddenInsuredId').val(individualDetail[0].Code);
            QuotaOperationalScripts.loadTableQuotaOperational(individualDetail[0].Code, individualDetail[0].Id);
        }
    }

    static ShowModalHolderDetail(dataTable) {
        $('#tableIndividualDetail').UifDataTable('clear');
        $('#tableIndividualDetail').UifDataTable({ sourceData: dataTable});

    }

    static formatNumberMiles(num) {
        if (!num || num == 'NaN') { return '-' };
        if (num == 'Infinity') { return '&#x221e;' };
        num = num.toString().replace(/\$|\,/g, '');
        if (isNaN(num)) {
            num = "0";
        }; 
        //num = (num = Math.abs(num));
        var sign = (num == (num = Math.abs(num)));
        num = Math.floor(num * 100 + 0.50000000001);
        var cents = (num % 100);
        num = Math.floor(num / 100).toString();
        if (cents < 10) {
            cents = "0" + cents;
        };
        for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++) {
            num = num.substring(0, num.length - (4 * i + 3)) + '.' + num.substring(num.length - (4 * i + 3));
        };
            
        return (((sign) ? '' : '-') + num + ',' + cents);
    }
}