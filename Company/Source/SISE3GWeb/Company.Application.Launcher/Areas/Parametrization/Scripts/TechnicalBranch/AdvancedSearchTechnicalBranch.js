var dropDownSearch;
var modelLineBusiness = {};

$(() => {
    new AdvancedSearchTechnicalBranch();
});

class SearchAdvanced {
    static SearchAdvancedLineBusiness(LinesBusiness) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Parametrization/TechnicalBranch/GetLineBusinessAdvancedSearch',
            data: JSON.stringify({ LineBusinessView: LinesBusiness }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}



class AdvancedSearchTechnicalBranch extends Uif2.Page {
    getInitialState() {
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'Parametrization/TechnicalBranch/SearchAdvancedTechnicalBranch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.AdvancedSearchEvents
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);


        $("#listViewSearchAdvanced").UifListView(
            {
                displayTemplate: "#AdvancedBranchTemplate",
                selectionType: 'single',
                source: null,
                height: 180
            });
    }

    AdvancedSearchEvents() {
        $("#btnSearchAdvScript").on("click", AdvancedSearchTechnicalBranch.ShowAdvanced);
        $("#btnCancelSearchAdv").on("click", AdvancedSearchTechnicalBranch.cancelSearch);
        $("#btnAdvancedSearchTechnicalBranch").on("click", AdvancedSearchTechnicalBranch.SearchTechicalBranchAdvanced);
        $("#btnLoadUser").click(AdvancedSearchTechnicalBranch.LoadBranchAdvanced);
    }

    bindEvents() {


    }
    static cancelSearch() {
      
        AdvancedSearchTechnicalBranch.ClearControls();
        dropDownSearch.hide();
    }


    static SearchTechicalBranchAdvanced() {
        var LinesBusiness = {};
       
        LinesBusiness =
        {
            Description: $("#inputNameTechnicalBranch").val().trim(),
            RiskTypeId: $("#selectRiskTypeCovered").UifSelect("getSelected")
        }
        AdvancedSearchTechnicalBranch.ClearControls();
        SearchAdvanced.SearchAdvancedLineBusiness(LinesBusiness).done(function (data) {
            if (data.success) {

                AdvancedSearchTechnicalBranch.ShowSearchAdv(data.result);
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
            height: 180
        });
        if (data) {
            let dataList = [];

            for (let i = 0; i < data.length; i++) {
                if (data[i].ListLineBusinessCoveredrisktype.length > 0) {
                    dataList.push({
                        Id: data[i].Id,
                        Description: data[i].Description,
                        ShortDescription: data[i].ShortDescription,
                        TyniDescription: data[i].TyniDescription,
                        ListLineBusinessCoveredrisktype: data[i].ListLineBusinessCoveredrisktype
                    });
                }
                else {
                    dataList.push({
                        Id: data[i].Id,
                        Description: data[i].Description,
                        ShortDescription: data[i].ShortDescription,
                        TyniDescription: data[i].TyniDescription
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
        $("#inputNameTechnicalBranch").val('');
        $("#selectRiskTypeCovered").UifSelect("setSelected", null);
        $("#listViewSearchAdvanced").UifListView({ sourceData: null, displayTemplate: "#AdvancedBranchTemplate", selectionType: 'single', height: 150 });
    }


    GetRiskTypeTechnicalBranch() {
        var selectedItem = $('#selectRiskType').val();
        if (selectedItem > 0) {
            $("#selectRiskType").prop("disabled", false);
            AdvancedSearchTechnicalBranch.setRiskTypeMain();
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

            if (SelectedAdv[0].ListLineBusinessCoveredrisktype) {
                if (SelectedAdv[0].ListLineBusinessCoveredrisktype.length > 1) {

                    glbLinesBusinessCoveredRiskType = SelectedAdv[0].ListLineBusinessCoveredrisktype;
                    $('#selectRiskTypeTechnicalBranchMain').attr("disabled", true);
                    $("#selectRiskTypeTechnicalBranchMain").UifSelect("setSelected",
                        SelectedAdv[0].ListLineBusinessCoveredrisktype[0].IdRiskType);
                    $("#inputDescriptionLong").val(SelectedAdv[0].Description);
                    $("#inputTechnicalBranchCode").val(SelectedAdv[0].Id);
                    $("#inputDescriptionShort").val(SelectedAdv[0].ShortDescription);
                    $("#inputAbbreviation").val(SelectedAdv[0].TyniDescription);
                    $('#btnRiskTypeTechnicalBranch').attr("disabled", false);
                    ParametrizationTechnicalBranch.loadLinesBusiness();
                } else if (SelectedAdv[0].ListLineBusinessCoveredrisktype.length == 1) {
                    glbLinesBusinessCoveredRiskType = SelectedAdv[0].ListLineBusinessCoveredrisktype;
                    //$('#selectRiskTypeTechnicalBranchMain').attr("disabled", true);
                    $("#selectRiskTypeTechnicalBranchMain").UifSelect("setSelected",
                        SelectedAdv[0].ListLineBusinessCoveredrisktype.IdRiskType);
                    $("#inputDescriptionLong").val(SelectedAdv[0].Description);
                    $("#inputTechnicalBranchCode").val(SelectedAdv[0].Id);
                    $("#inputDescriptionShort").val(SelectedAdv[0].ShortDescription);
                    $("#inputAbbreviation").val(SelectedAdv[0].TyniDescription);
                    $('#btnRiskTypeTechnicalBranch').attr("disabled", false);
                    ParametrizationTechnicalBranch.loadLinesBusiness();
                }
            } else {
                $("#inputDescriptionLong").val(SelectedAdv[0].Description);
                $("#inputTechnicalBranchCode").val(SelectedAdv[0].Id);
                $("#inputDescriptionShort").val(SelectedAdv[0].ShortDescription);
                $("#inputAbbreviation").val(SelectedAdv[0].TyniDescription);
                $('#btnRiskTypeTechnicalBranch').attr("disabled", true);
                ParametrizationTechnicalBranch.loadLinesBusiness();
            }
            AdvancedSearchTechnicalBranch.ClearControls();
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': 'Debe seleccionar un elemento de la lista', 'autoclose': true })
        }
        ParametrizationTechnicalBranch.LoadInsuranceObjects();
    }
}