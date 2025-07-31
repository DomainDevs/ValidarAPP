var searchHolderTo = "";
var modalListType;
var IndividualType;
var glbIndividualId = 0;

class CounterGuarantee extends Uif2.Page {
    getInitialState() {
        $('#selectGuarantee').UifSelect();
        CounterGuarantee.setDefault();
    }

    bindEvents() {
        $("#inputSecure").on("buttonClick", this.SearchSecure);
        //Seleccionar un elemento de la modal de busqueda
        $('#tableIndividualResults tbody').on('click', 'tr', this.SelectIndividual);
        $('#selectGuarantee').on("change", () => $('#divDownload').hide());
    }

    SearchSecure() {
        $('#divDownload').hide();
        $('#selectGuarantee').UifSelect();
        searchHolderTo = "inputSecure";
        CounterGuarantee.LoadInputHolderByDescription();
    }

    static setDefault() {
        $('#inputSecure').ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }

    SelectIndividual(e) {
        var individualId = $(this).children()[0].innerHTML;
        var customerType = $(this).children()[1].innerHTML;
        CounterGuarantee.LoadInputHolderByIndividual(individualId, customerType, "#" + searchHolderTo);


        $('#modalIndividualSearch').UifModal("hide");
        $("#SearchindividualId").val('');
        $("#SearchCodeId").val('');
    }

    static LoadInputHolderByIndividual(individual, customerType, input) {
        individual = individual.toString()
        CounterGuaranteeRequest.GetHoldersByIndividualId(individual, customerType)
            .done(function (data) {
                if (data.success) {
                    CounterGuarantee.SetInputFromHoldersResult(input, data.result);
                } else {
                    showErrorToast(data.result);
                }
            }).fail(function (args) { showErrorToast(AppResources.ErrorConsultingInsured); })
    }

    static LoadInputHolderByDescription() {
        let selector = "#" + searchHolderTo;
        let description = $(selector).val();
        var descrip = parseInt(description) || description.trim();
        if (typeof (descrip) !== "number" && descrip.length < 3) {

            showInfoToast(AppResources.HolderSearchMinLength);
            return;
        }

        var customerType = 1;

        CounterGuaranteeRequest.GetHoldersByDescription(description, customerType)
            .done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        if (data.result.length == 0) {
                            CounterGuarantee.CleanInputFromHolderResult(selector);
                            showInfoToast(AppResources.MessageSearchInsureds);
                        }
                        else if (data.result.length == 1) {
                            CounterGuarantee.SetInputFromHoldersResult(selector, { holder: data.result[0] });
                        }
                        else {
                            modalListType = 1;
                            CounterGuarantee.ShowModalListRiskSurety(data.result);
                        }
                    }
                }
                else {
                    showErrorToast(data.result);
                }
            });

    }

    static ShowModalListRiskSurety(dataTable) {
        var datalist = [];
        dataTable.forEach(function (item) {
            datalist.push({
                Id: item.IndividualId,
                CustomerType: item.CustomerType,
                Code: item.InsuredId,
                DocumentNum: item.IdentificationDocument.Number,
                Description: item.Name,
                CustomerTypeDescription: item.CustomerTypeDescription,
                DocumentType: item.IdentificationDocument.DocumentType.Description
            });
        });
        $('#tableIndividualResults').UifDataTable('clear');
        $('#tableIndividualResults').UifDataTable('addRow', datalist);
        $('#modalIndividualSearch').UifModal('showLocal', AppResources.SelectInsured);
    }

    static SetInputFromHoldersResult(selector, result) {
        let secure = result.holder;
        IndividualType = secure.IndividualType;
        secure.details = result.details || [secure.CompanyName];
        $(selector).data("Object", secure);
        $(selector).val(secure.Name + ' (' + secure.IdentificationDocument.Number + ')');
        glbIndividualId = secure.IndividualId;
        CounterGuarantee.GetCounterGuaranteesByIndividualId(secure.IndividualId);
    }

    static CleanInputFromHolderResult(selector) {
        $(selector).data("Object", null);
        $(selector).val('');
    }

    static GetCounterGuaranteesByIndividualId(individualId) {
        CounterGuaranteeRequest.GetCounterGuaranteesByIndividualId(individualId).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    const dataSelect = data.result.map((item) => {
                        return { Description: item.Description, Id: item.InsuredGuarantee.Id};
                    })
                    $('#selectGuarantee').UifSelect({ sourceData: dataSelect });
                    $('#btnPrintCounterGuarantee').prop('disabled', false);
                }
                else {
                    showErrorToast(AppResources.CounterGuaranteeNotFound);
                }

            }
            else {

            }
        });
    }

    static PrintCounterGuarantees() {

        let guaranteeId = $('#selectGuarantee').val();
        $('#divDownload').hide();
        CounterGuaranteeRequest.PrintCounterGuarantees(guaranteeId, glbIndividualId).done(function (data) {
            if (data.success) {
                if (data.result.Url !== undefined) {

                    $('#divDownload').show();
                    $('#hrfPathName').text(data.result.Filename);
                    $('#hrfPathPdf').prop('href', data.result.Url);
                    $('#hrfPathPdf').prop('download', data.result.Filename);

                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            }
            else {
                if (data.result == Resources.Language.EndorsmentNotReinsured) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotPrinter + ": " + data.result, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPrinting + ": " + data.result, 'autoclose': true });
                }
            }
        });
    }
}


$(document).ready(function () {
    new CounterGuarantee();
    new CounterGuaranteeRequest();

    $("#btnPrintCounterGuarantee").on('click', CounterGuarantee.PrintCounterGuarantees);
});

