var guaranteeDocuments = []

class DocumentationReceived extends Uif2.Page {

    getInitialState() { }

    bindEvents() {

        $("#btnDocumentationReceived").click(function () {
            dataInsuredGuarantee = Guarantee.GetinsuredGuarantee();
            if (dataInsuredGuarantee.Id != null) {
                DocumentationReceived.LoadPartialDocumentationReceived();
                Guarantee.HidePanelsGuarantee();
                $("#buttonsGuarantee").hide();
                Guarantee.ShowPanelsGuarantee(MenuType.DOCUMENTATION);
            } else {
                $.UifDialog('alert', { 'message': "Validar guardado de contragarantia." });
            }
        });

        $("#btnCancelDocumentation").click(function () {
            Guarantee.HidePanelsGuarantee();
            Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
        });

        $("#btnNewDocumentatioReceived").click(function () {
            var data = Guarantee.GetinsuredGuarantee();

            if (data.Id != null) {
                var selectedDocuments = $("#tableDocumentationReceived").UifDataTable('getSelected');

                var Documents = []
                var documentsQnt = 0

                $.each(selectedDocuments, function () {
                    if (guaranteeDocuments.findIndex(x => x.DocumentCode === this.DocumentCode) < 0) {
                        Documents.push({
                            DocumentCode: this.DocumentCode,
                            GuaranteeCode: this.GuaranteeCode,
                            IndividualId: individualId,
                            GuaranteeId: data.Id,
                            ParametrizationStatus: 2 //Se crea el objeto

                        });
                        documentsQnt++
                    } else {
                        Documents.push({
                            DocumentCode: this.DocumentCode,
                            GuaranteeCode: this.GuaranteeCode,
                            IndividualId: individualId,
                            GuaranteeId: data.Id,
                            ParametrizationStatus: 3 //Se actualiza el objeto
                        });
                        documentsQnt++
                    }
                })
                $.each(guaranteeDocuments, function () {
                    if (selectedDocuments.findIndex(x => x.DocumentCode === this.DocumentCode) < 0) {
                        Documents.push({
                            DocumentCode: this.DocumentCode,
                            GuaranteeCode: this.GuaranteeCode,
                            IndividualId: individualId,
                            GuaranteeId: data.Id,
                            ParametrizationStatus: 4 //Se elimina el objeto
                        });
                    }
                })
                GuaranteeRequest.CreateInsuredGuaranteeDocumentation(Documents);
                $('#selectedDocumentationReceived').html("(" + documentsQnt + ")");
                Guarantee.HidePanelsGuarantee();
                Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
            }
        });
    }

    static LoadPartialDocumentationReceived() {
        var guarantee = Guarantee.GetinsuredGuarantee();
        $('#Documentation-NumberDocument').val(numberDoc);
        $('#SecureName').val($("#inputContractorName").text());
        $('#inputTypeGuaranteeDocumentation').val($("#selectGuarantees").UifSelect('getSelectedText'));
        if (guarantee.Id != null) {
            GuaranteeRequest.GetDocumentationReceived(guarantee.Guarantee.GuaranteeType.Id).done(function (data) {
                if (data.success) {

                    $("#tableDocumentationReceived").UifDataTable({ sourceData: data.result });
                    DocumentationReceived.LoadDocumentationReceived(individualId, guarantee.Id)

                }
            });
        }
    }
    static LoadDocumentationReceived(individualId, guaranteeId) {
        GuaranteeRequest.GetDocumentationReceivedByIndividualAndGuaranteeId(individualId, guaranteeId).done(function (data) {
            if (data.success) {
                var object = [];
                guaranteeDocuments = data.result;
                $.each(data.result, function () {
                    object.push(this.DocumentCode);
                });
                var items = {
                    label: 'DocumentCode',
                    values: object
                }
                $('#selectedDocumentationReceived').html("(" + object.length + ")");
                $("#tableDocumentationReceived").UifDataTable('setSelect', items)
            }

        });
    }


}
