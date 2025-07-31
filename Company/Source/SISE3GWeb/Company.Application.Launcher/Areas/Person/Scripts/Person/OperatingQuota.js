var operatingQuotas = [];
var OperatingQuotaIndex = -1;
var heightOperationQuotaListView = 300;
var OperatingQuotaUpdate = 0;
var operationQuota = 0;
class OperatingQuota extends Uif2.Page {
    getInitialState() {
        OperatingQuota.UnbindEvents();
        //LineBusiness.GetLinesBusiness().done(function (data) {
        //    if (data.success) {
        //        $("#OperatingQuota-LineBusinessCd").UifSelect({ sourceData: data.result });
        //    }
        //});
        $("#OperatingQuota-Currency").prop("disabled", true);
        $('#OperatingQuota-OperatingQuotaAmt').OnlyDecimals(numberDecimal);
    }

    //Seccion Eventos
    bindEvents() {

        $("#btnrNewOperatingQuota").click(OperatingQuota.ClearControlOperatingQuota);
        $("#btnAcceptOperatingQuota").click(this.AcceptOperatingQuota);
        $("#btnAcceptOperatingQuotas").click(this.AcceptOperatingQuotas);
        $("#btnCancelOperatingQuotas").click(OperatingQuota.ClearControlOperatingQuota);
        $('#listOperatingQuota').on('rowDelete', this.OperatingQuotaDelete);
        $('#listOperatingQuota').on('rowEdit', this.OperatingQuotaEdit);
        $("#OperatingQuota-OperatingQuotaAmt").focusin(this.NotFormatMoneyIn);
        $("#OperatingQuota-OperatingQuotaAmt").focusout(this.FormatMoneyout);
    }

    static UnbindEvents() {
        $("#btnrNewOperatingQuota").unbind();
        $("#btnAcceptOperatingQuota").unbind();
        $("#btnAcceptOperatingQuotas").unbind();
        $("#btnCancelOperatingQuotas").unbind();
        $('#listOperatingQuota').unbind();
        $('#listOperatingQuota').unbind();
        $("#OperatingQuota-OperatingQuotaAmt").unbind();
        $("#OperatingQuota-OperatingQuotaAmt").unbind();
    }

    AcceptOperatingQuota() {

        if (OperatingQuota.ValidateOperatingQuota()) {
            OperatingQuota.AddOperatingQuota();
            OperatingQuota.ClearControlOperatingQuota();
        }
    }

    AcceptOperatingQuotas() {
        if ($("#listOperatingQuota").UifListView('getData').length > 0) {
            operatingQuotas = $("#listOperatingQuota").UifListView('getData');
            // Validar R1
            $.each(operatingQuotas, function (key, value) {
                value.Amount.Value = NotFormatMoney(this.Amount.Value);
            });
            OperatingQuota.SaveOperatingQuota(OperatingQuota.ConvertModelOperatingQuotaToOperatingQuotaDto(operatingQuotas));
            OperatingQuota.ClearControlOperatingQuota();

        }

    }

    OperatingQuotaDelete(event, data) {
        if ($('#listOperatingQuota').UifListView("getData").length > 1) {
            OperatingQuota.DeleteOperatingQuota(data);
        } else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.ErrorDeleteOperationQuota, 'autoclose': true });
        }
    }

    OperatingQuotaEdit(event, data, index) {
        OperatingQuotaUpdate = 1;
        OperatingQuotaIndex = index;
        OperatingQuota.DisabledOperatingQuotaControl(true);
        OperatingQuota.EditOperatingQuota(data);
    }

    NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    FormatMoneyout() {
        $(this).val(FormatMoney($(this).val()));
    }

    static clearObjectsOperatingQuota() {
        operatingQuotas = [];
        Persons.GetdefaultValueCurrency();
        $("#listOperatingQuota").UifListView({
            displayTemplate: "#operatingQuotaTemplate",
            add: false,
            edit: true,
            delete: false,
            customEdit: true,
            deleteCallback: OperatingQuota.deleteOperatingQuotaList,
            height: heightOperationQuotaListView
        });
    }

    static ClearControlOperatingQuota() {
        OperatingQuota.DisabledOperatingQuotaControl(false);
        OperatingQuotaIndex = -1;
        OperatingQuotaUpdate = 0;
        $('#OperatingQuota-LineBusinessCd').UifSelect('setSelected', "");
        Persons.GetdefaultValueCurrency();
        $('#OperatingQuota-OperatingQuotaAmt').val('');
        $('#OperatingQuota-CurrentTo').UifDatepicker('clear');
        $("#OperatingQuota-Currency").prop('disabled', true);
    }

    static AddOperatingQuota() {
        var OperatingQuotaTmp = OperatingQuota.CreateOperatingQuotaModel();
        if (OperatingQuotaIndex == -1) {
            $("#listOperatingQuota").UifListView('addItem', OperatingQuotaTmp);
        }
        else {
            $("#listOperatingQuota").UifListView('editItem', OperatingQuotaIndex, OperatingQuotaTmp);
        }
    }

    static CreateOperatingQuotaModel() {
        var OperatingQuota = { Amount: { Currency: {} }, LineBusiness: {} };

        OperatingQuota.IndividualId = individualId;
        OperatingQuota.Amount.Value = $("#OperatingQuota-OperatingQuotaAmt").val();
        OperatingQuota.Amount.Currency.Id = $("#OperatingQuota-Currency").UifSelect('getSelected');
        OperatingQuota.Amount.Currency.Description = $("#OperatingQuota-Currency").UifSelect('getSelectedText');
        OperatingQuota.LineBusiness.Id = $("#OperatingQuota-LineBusinessCd").UifSelect('getSelected');
        OperatingQuota.LineBusiness.Description = $("#OperatingQuota-LineBusinessCd").UifSelect('getSelectedText');
        OperatingQuota.CurrentTo = $('#OperatingQuota-CurrentTo').val();
        return OperatingQuota;
    }

    static EditOperatingQuota(data) {
        $('#OperatingQuota-LineBusinessCd').UifSelect('setSelected', data.LineBusiness.Id);
        $('#OperatingQuota-Currency').UifSelect('setSelected', data.Amount.Currency.Id);
        $('#OperatingQuota-OperatingQuotaAmt').val(data.Amount.Value);
        $('#OperatingQuota-CurrentTo').UifDatepicker('setValue', data.CurrentTo);
    }

    static ValidateOperatingQuota() {
        var error = "";
        var errorContain = "";
        var listViewData = $("#listOperatingQuota").UifListView("getData");
        if ($('#OperatingQuota-LineBusinessCd option:selected').val() == '' || $('#OperatingQuota-LineBusinessCd option:selected').val() == undefined) { error += AppResourcesPerson.LabelPrefixCommercial + " <br>" }
        if ($('#OperatingQuota-Currency option:selected').val() == '' || $('#OperatingQuota-Currency option:selected').val() == undefined) { error += AppResourcesPerson.LabelCurrency + " <br>" }
        if ($('#OperatingQuota-OperatingQuotaAmt').val() == '' || $('#OperatingQuota-OperatingQuotaAmt').val() == undefined) { error += AppResourcesPerson.LabelOperatingQuota + " <br>" }
        if ($('#OperatingQuota-CurrentTo').val() == '' || $('#OperatingQuota-CurrentTo').val() == undefined) { error += AppResourcesPerson.LabelCurrentTo + " <br>" }
        $.each(listViewData, function (key, value) {
            if (value.LineBusiness.Id == $('#OperatingQuota-LineBusinessCd').val() && OperatingQuotaUpdate == 0) {
                errorContain = "Solo puede agregar un cupo a cada ramo." + " <br>";
                return false;
            }
        });
        if (error == "" && errorContain == "") {
            return true;
        }
        else if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageFieldsAreMandatory + ':<br>' + error })
            return false;
        } else if (errorContain != "") {
            $.UifNotify('show', { 'type': 'info', 'message': errorContain })
            return false;
        }
    }

    //Obtiene el cupo operativo del afianzado
    static LoadOperatingQuota(individualId) {
        OperatingQuota.clearObjectsOperatingQuota();
        $.UifProgress('show');
        OperatingQuotaRequest.GetOperatingQuota(individualId).done(function (data) {
            $.UifProgress('close');
            if (data.success) {
                var operatingQuota = OperatingQuota.ConvertOperatingQuotaDtoToOperatingQuota(data);
                //OperatingQuota.LoadListViewOperating(data);
                OperatingQuota.LoadListViewOperating(operatingQuota);
            }
        }).fail(function (data) {
            $.UifProgress('close');
        });
    }

    static LoadListViewOperating(data) {
        if (data != null) {
            //operatingQuotas = data.result;
            operatingQuotas = data;
            $.each(operatingQuotas, function (key, value) {
                value.Amount.Value = FormatMoney(this.Amount.Value);
                value.CurrentTo = FormatDate(this.CurrentTo);
                var selector = "#OperatingQuota-Currency [value=" + value.Amount.Currency.Id + "]"
                value.Amount.Currency.Description = $(selector).text();
                selector = "#OperatingQuota-LineBusinessCd [value=" + value.LineBusiness.Id + "]"
                value.LineBusiness.Description = $(selector).text();

            });
            $("#listOperatingQuota").UifListView({
                //sourceData: data.result,
                sourceData: operatingQuotas,
                displayTemplate: "#operatingQuotaTemplate",
                add: false,
                edit: true,
                delete: false,
                customEdit: true,
                deleteCallback: OperatingQuota.deleteOperatingQuotaList,
                height: heightOperationQuotaListView
            });
        }
    }

    static DeleteOperatingQuota(Data, deferred) {
        OperatingQuotaRequest.DeleteOperatingQuota(OperatingQuota.ConvertModelOperatingQuotaToOperatingQuotaDto(Data)).done(function (data) {
            if (data.success) {
                deferred.resolve();
                deferred.done(operatingQuotas = $("#listOperatingQuota").UifListView('getData'));
                $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.MessageDeleteOperationQuota, 'autoclose': true });
            }
            else {
                deferred.reject()
            }
        });
    }

    static deleteOperatingQuotaList(deferred, data) {
        OperatingQuota.DeleteOperatingQuota(data, deferred)
        OperatingQuota.ClearControlOperatingQuota();
    }

    //Guarda cupo operativo
    static SaveOperatingQuota(operatingQuotaDTOs) {

        var OperationQuotaEventDTOs = [];
        operatingQuotaDTOs.forEach(function (item, index) {
            if (item.CurrencyId == 0) {
                OperationQuotaEventDTOs[index] = {};
                OperationQuotaEventDTOs[index].OperatingQuotaEventID = 0;
                OperationQuotaEventDTOs[index].OperatingQuotaEventType = EventOperatingQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA;
                OperationQuotaEventDTOs[index].IdentificationId = item.IndividualId;
                OperationQuotaEventDTOs[index].IssueDate = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                OperationQuotaEventDTOs[index].LineBusinessID = item.LineBusinessId;
                OperationQuotaEventDTOs[index].IndividualOperatingQuota = {};
                OperationQuotaEventDTOs[index].IndividualOperatingQuota.ParticipationPercentage = 100;
                OperationQuotaEventDTOs[index].IndividualOperatingQuota.IndividualID = item.IndividualId;
                OperationQuotaEventDTOs[index].IndividualOperatingQuota.LineBusinessID = item.LineBusinessId;
                OperationQuotaEventDTOs[index].IndividualOperatingQuota.ValueOpQuotaAMT = item.AmountValue;
                OperationQuotaEventDTOs[index].IndividualOperatingQuota.InitDateOpQuota = FormatFullDate(moment().format('YYYY/MM/DD H:mm'));
                OperationQuotaEventDTOs[index].IndividualOperatingQuota.EndDateOpQuota = FormatFullDate(item.CurrentTo + ' ' + moment().format('h:mm'));
            }
        });

        lockScreen();
        OperatingQuotaRequest.SaveOperatingQuotaByEvent(operatingQuotaDTOs, OperationQuotaEventDTOs).done(result => {
            if (result.success) {
                if (result.result != null) {
                    var dataOperating = result.result[0];
                    var policyType = LaunchPolicies.ValidateInfringementPolicies(dataOperating.InfringementPolicies, true);
                    let countAuthorization = dataOperating.InfringementPolicies.filter(item => { return item.Type === TypeAuthorizationPolicies.Authorization }).length;

                    if (countAuthorization > 0 || policyType === TypeAuthorizationPolicies.Restrictive) {
                        if (countAuthorization > 0) {
                            LaunchPolicies.RenderViewAuthorizationPolicies(dataOperating.InfringementPolicies, dataOperating.OperationId, FunctionType.PersonOperatingQuota);
                        }
                    } else {

                        OperatingQuota.ClearControlOperatingQuota();
                        $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.CreatedSuccessfully, 'autoclose': true });
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'error', 'message': result.result, 'autoclose': true });
            }
            unlockScreen();
        }).fail(() => unlockScreen());
    }

    static DisabledOperatingQuotaControl(control) {
        $("#OperatingQuota-LineBusinessCd").prop('disabled', control);
    }

    static ConvertOperatingQuotaDtoToOperatingQuota(requestData) {
        var resulOperatingQuota = [];
        if (requestData.result.length > 0) {
            $.each(requestData.result, function (index, item) {
                let operatingQuota = {
                    Amount: {
                        Currency: {}
                    },
                    LineBusiness: {}
                };
                operatingQuota.Amount.Currency.Id = item.CurrencyId;
                operatingQuota.Amount.Currency.Description = "Pesos";
                operatingQuota.Amount.Value = item.AmountValue;
                operatingQuota.CurrentTo = item.CurrentTo;
                operatingQuota.IndividualId = item.IndividualId;
                operatingQuota.LineBusiness.Id = item.LineBusinessId;
                operatingQuota.LineBusiness.Description = (item.LineBusinessId == 2 ? "CUMPLIMIENTO" : "CAUCION JUDICIAL");
                operatingQuota.LineBusinessId = item.LineBusinessId;
                operatingQuota.CurrencyId = item.CurrencyId;

                resulOperatingQuota.push(operatingQuota);
            });
        }
        return resulOperatingQuota;
    }

    static ConvertModelOperatingQuotaToOperatingQuotaDto(data) {
        var resultOperatingQuota = [];
        if (typeof data.length == 'undefined') {
            let operatingQuota = {};
            operatingQuota.AmountValue = data.Amount.Value.indexOf('.') < 0 ? data.Amount.Value : data.Amount.Value.replace('.', ',');
            operatingQuota.CurrencyId = data.Amount.Currency.Id;
            operatingQuota.CurrentTo = data.CurrentTo;
            operatingQuota.Id = 0;
            operatingQuota.IndividualId = data.IndividualId;
            operatingQuota.LineBusinessId = data.LineBusiness.Id;

            return operatingQuota; //.push(operatingQuota);
        }
        else {
            $.each(data, function (index, data) {
                let operatingQuota = {};
                operatingQuota.AmountValue = data.Amount.Value;
                operatingQuota.CurrencyId = data.Amount.Currency.Id;
                operatingQuota.CurrentTo = data.CurrentTo;
                operatingQuota.Id = 0;
                operatingQuota.IndividualId = data.IndividualId;
                operatingQuota.LineBusinessId = data.LineBusiness.Id;

                resultOperatingQuota.push(operatingQuota);
            });
            return resultOperatingQuota;
        }
    }
}