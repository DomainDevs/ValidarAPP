var dropDownSearch;
var modelLineBusiness = {};

class AdvancedSearchLineBusiness extends Uif2.Page {
    getInitialState() {
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'Parametrization/LineBusiness/SearchAdvanced',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: AdvancedSearchLineBusiness.componentLoadedCallback
        });
    }

    bindEvents() {

    }
    static componentLoadedCallback() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#btnSearchAdvScript").on("click", AdvancedSearchLineBusiness.ShowAdvanced);
        $("#btnCancelLineBusinessSearchAdv").on("click", AdvancedSearchLineBusiness.cancelSearch);
        $("#btnAdvancedSearchLineBusiness").on("click", AdvancedSearchLineBusiness.SearchTechicalBranchAdvanced);
        $("#btnLoadUser").click(AdvancedSearchLineBusiness.LoadBranchAdvanced);
        $("#listViewSearchAdvanced").UifListView(
            {
                displayTemplate: "#AdvancedBranchTemplate",
                selectionType: 'single',
                source: null,
                height: 300
            });
    }

    static cancelSearch() {

        AdvancedSearchLineBusiness.ClearControls();
        dropDownSearch.hide();
    }


    static SearchTechicalBranchAdvanced() {
		var coveredRiskTypeId = 0;
		if ($("#selectRiskTypeCovered").UifSelect("getSelected") != "") {
			coveredRiskTypeId = $("#selectRiskTypeCovered").UifSelect("getSelected");
		}
		LineBusinessSearchAdvancedRequest.SearchAdvancedLineBusiness($("#inputNameLineBusiness").val().trim(), coveredRiskTypeId).done(function (data) {
            if (data.success) {

                AdvancedSearchLineBusiness.ShowSearchAdv(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': 'No se encontro ningun resultado', 'autoclose': true })
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Error al intentar buscar ramo técnico', 'autoclose': true })

        });
    }

    static ShowSearchAdv(data) {

        $("#listViewSearchAdvanced").UifListView(
            {
                displayTemplate: "#AdvancedBranchTemplate",
                selectionType: 'single',
                source: null,
                height: 300
            });
        if (data) {
            let dataList = [];

            for (let i = 0; i < data.length; i++) {
                if (data[i].CoveredRiskTypes.length > 0) {
                    dataList.push({
                        Id: data[i].Id,
                        Description: data[i].Description,
                        ShortDescription: data[i].SmallDescription,
                        CoveredRiskTypes: data[i].CoveredRiskTypes
                    });
                }
                else {
                    dataList.push({
                        Id: data[i].Id,
                        Description: data[i].Description,
                        ShortDescription: data[i].SmallDescription
                    });
                }
            }

            $.each(dataList, function (index, val) {
                $("#listViewSearchAdvanced").UifListView("addItem", dataList[index]);
            });

            dropDownSearch.show();
        }
    }

    static ClearControls() {
        $("#inputNameLineBusiness").val('');
        $("#selectRiskTypeCovered").UifSelect("setSelected", null);
        $("#listViewSearchAdvanced").UifListView({ sourceData: null, displayTemplate: "#AdvancedBranchTemplate", selectionType: 'single', height: 150 });
    }


    GetRiskTypeLineBusiness() {
        var selectedItem = $('#selectRiskType').val();
        if (selectedItem > 0) {
            $("#selectRiskType").prop("disabled", false);
            AdvancedSearchLineBusiness.setRiskTypeMain();
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
    }

    static LoadBranchAdvanced() {
        var SelectedAdv = $("#listViewSearchAdvanced").UifListView("getSelected");
        if (SelectedAdv.length > 0) {
            dropDownSearch.hide();

            LineBusinessParametrization.GetLinesBusinessById(null, "", SelectedAdv[0].Id);

            AdvancedSearchLineBusiness.ClearControls();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Debe seleccionar un elemento de la lista', 'autoclose': true })
        }
    }
}