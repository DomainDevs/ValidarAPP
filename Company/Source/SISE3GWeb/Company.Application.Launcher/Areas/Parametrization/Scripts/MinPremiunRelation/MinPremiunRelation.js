class MinPremiunRelation extends Uif2.Page {

    getInitialState() {
        this.initForm();
        $("#listMiniumPremiun").UifListView({
            displayTemplate: "#templateMiniumPremiun",
            source: null,
            selecttionType: 'single',
            height: 400,
            filterColumns: ['Product.Description', 'EndorsementType.Description',' Branch.LongDescription']                
        });  
        $('#inputMinPremiunRelationSearch').on("search", this.getSearch);
    }

    bindEvents() {
        $('#listMiniumPremiun').on('rowEdit', MinPremiunRelation.editMinPremiunRelation);
        $("#btnDeleteMiniumPremiun").click(MinPremiunRelation.delete);
        $("#btnAddMiniumPremiun").click(MinPremiunRelation.clearForm);
        $("#btnSaveMiniumPremiun").click(MinPremiunRelation.saveMinPremiumRelation);
        $("#btnExitMiniumPremiun").click(this.redirectIndex);
        $("#btnExportMiniumPremiun").click(MinPremiunRelation.exportExcel);
        $("#PrefixId").on("itemSelected", MinPremiunRelation.getDataFromDependentOfPrefix);
        $("#ProductId").on("itemSelected", MinPremiunRelation.getClave);
      
        
    }

    initForm() {
        MinPremiunRelation.clearForm();
        MinPremiunRelation.getSearchPrefix();
        MinPremiunRelation.getPrefix();
        MinPremiunRelation.getBranch();
        MinPremiunRelation.getEndorsementType();
        MinPremiunRelation.getCurrency();
    }

    static clearForm() {

        $("#ProductId").UifSelect("setSelected", null);
        $("#ProductId").UifSelect({ sourceData: null });

        $("#ClaveId").UifSelect("setSelected", null);
        $("#ClaveId").UifSelect({ sourceData: null });

        $("#PrefixId").UifSelect("setSelected", null);
        $("#BranchId").UifSelect("setSelected", null);
        $("#EndorsementTypeId").UifSelect("setSelected", null);
        $("#CurrencyId").UifSelect("setSelected", null);
        $("#ProductId").UifSelect("setSelected", null);
        $("#ClaveId").UifSelect("setSelected", null);
        $("#MiniumPremiunValue").val(0);
        $("#MiniumSubValue").val(0);
        $("#Id").val(0);
        $("#StatusTypeService").val(null);

    }

    static editMinPremiunRelation(event, data, index) {

        MinPremiunRelation.clearForm();

        $("#PrefixId").UifSelect("setSelected", data.Prefix.Id);
        $("#BranchId").UifSelect("setSelected", data.Branch.Id);
        $("#EndorsementTypeId").UifSelect("setSelected", data.EndorsementType.Id);
        $("#CurrencyId").UifSelect("setSelected", data.Currency.Id);
        $("#MiniumPremiunValue").val(data.MiniumPremiunValue);
        $("#MiniumSubValue").val(data.MiniumSubValue);
        $("#StatusTypeService").val(3);
        $("#Id").val(data.Id);

        //$("#PrefixId").UifSelect("disabled", true);
        //$("#BranchId").UifSelect("disabled", true);
        //$("#EndorsementTypeId").UifSelect("disabled", true);
        //$("#CurrencyId").UifSelect("disabled", true);
        //$("#ProductId").UifSelect("disabled", true);
        //$("#ClaveId").UifSelect("disabled", true);


        if (data.Clave != null) {
            MinPremiunRelation.getClaveGrupo(data.Prefix.Id, data.Product.Id, data.Clave.Id);
        }
        MinPremiunRelation.getProductType(data.Prefix.Id, data.Product.Id);
    }

    static getForm() {
        var data = {};
        data.Prefix = {};
        if ($("#PrefixId").UifSelect("getSelected") != null && $("#PrefixId").UifSelect("getSelected") != "") {
            data.Prefix.Id = $("#PrefixId").UifSelect("getSelected");
            data.Prefix.Description = $("#PrefixId").UifSelect("getSelectedText");
        }
        data.Branch = {};
        if ($("#BranchId").UifSelect("getSelected") != null && $("#BranchId").UifSelect("getSelected") != "") {
            data.Branch.Id = $("#BranchId").UifSelect("getSelected");
            data.Branch.LongDescription = $("#BranchId").UifSelect("getSelectedText");
        }
        data.EndorsementType = {};
        if ($("#EndorsementTypeId").UifSelect("getSelected") != null && $("#EndorsementTypeId").UifSelect("getSelected") != "") {
            data.EndorsementType.Id = $("#EndorsementTypeId").UifSelect("getSelected");
            data.EndorsementType.Description = $("#EndorsementTypeId").UifSelect("getSelectedText");
        }
        data.Currency = {};
        if ($("#CurrencyId").UifSelect("getSelected") != null && $("#CurrencyId").UifSelect("getSelected") != "") {
            data.Currency.Id = $("#CurrencyId").UifSelect("getSelected");
            data.Currency.Description = $("#CurrencyId").UifSelect("getSelectedText");
        }
        data.Product = {};
        if ($("#ProductId").UifSelect("getSelected") != null && $("#ProductId").UifSelect("getSelected") != "") {
            data.Product.Id = $("#ProductId").UifSelect("getSelected");
            data.Product.Description = $("#ProductId").UifSelect("getSelectedText");
        }
        data.Clave = {};
        if ($("#ClaveId").UifSelect("getSelected") != null && $("#ClaveId").UifSelect("getSelected") != "") {
            data.Clave.Id = $("#ClaveId").UifSelect("getSelected");
            data.Clave.Description = $("#ClaveId").UifSelect("getSelectedText");
        }
        data.MiniumPremiunValue = $("#MiniumPremiunValue").val();
        data.MiniumSubValue = $("#MiniumSubValue").val();
        data.StatusTypeService = ($("#StatusTypeService").val() == "" ? 2 : $("#StatusTypeService").val());
        data.Id = $("#Id").val();
        return data;
    }

    static getPrefix() {
        MinPremiunRelationRequest.GetPrefix().done(response => {
            let result = response.result;
            if (response.success) {
                $("#PrefixId").UifSelect({
                    sourceData: result
                });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }

    static getSearch(PrefixId, ProductName) {
        if (PrefixId !== "" && ProductName.length >= 3) {
            MinPremiunRelationRequest.GetSearch(PrefixId, ProductName).done(response => {
                let result = response.result;
                $("#listMiniumPremiun").UifListView({
                    displayTemplate: "#templateMiniumPremiun",
                    source: null,
                    selecttionType: 'single',
                    height: 400,
                    filterColumns: ['Product.Description', 'EndorsementType.Description', ' Branch.LongDescription']
                });

                if (response.success) {
                    MinPremiunRelation.getAllDataMinPremiunRelation(result);
                    MinPremiunRelation.clearForm();
                 
                    $("#inputMinPremiunRelationSearch").val("");
                } else {
                    $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })                 

                }
            });
        }
        else {
            $.UifNotify("show", { 'type': "info", 'message': 'Seleccione un item y digite nombre a buscar', 'autoclose': true });
            $("#PrefixSearch").css('background-color', 'red');
        }     
    }

    static getSearchPrefix() {
        MinPremiunRelationRequest.GetPrefix().done(response => {
            let result = response.result;
            if (response.success) {
                $("#PrefixSearch").UifSelect({
                    sourceData: result
                });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }

    static getBranch() {
        MinPremiunRelationRequest.GetBranch().done(response => {
            let result = response.result;
            if (response.success) {
                $("#BranchId").UifSelect({
                    sourceData: result
                });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }

    static getEndorsementType() {
        MinPremiunRelationRequest.GetEndorsementType().done(response => {
            let result = response.result;
            if (response.success) {
                $("#EndorsementTypeId").UifSelect({
                    sourceData: result
                });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }

    static getCurrency() {
        MinPremiunRelationRequest.GetCurrency().done(response => {
            let result = response.result;
            if (response.success) {
                $("#CurrencyId").UifSelect({
                    sourceData: result
                });
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }

    static getProductType(PrefixId, ProductId) {
        MinPremiunRelationRequest.GetProductType(PrefixId).done(response => {
            let result = response.result;
            $("#ProductId").prop('required', false);
            $("#ProductLabel").removeClass("field-required");
            if (response.success) {
                $("#ProductId").UifSelect({
                    sourceData: result
                });
                if (result.length > 0) {
                    $("#ProductId").UifSelect("setSelected", ProductId);
                    $("#ProductId").prop('required', true);
                    $("#ProductLabel").addClass("field-required");
                }
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }

    static getClaveGrupo(PrefixId, productId, ClaveId) {
      
        MinPremiunRelationRequest.GetClave(productId, PrefixId).done(response => {
            let result = response.result;
            $("#ClaveId").prop('required', false);
            $("#ClaveLabel").removeClass("field-required");
            if (response.success) {
                $("#ClaveId").UifSelect({
                    sourceData: result
                });
                if (result.length > 0) {
                    $("#ClaveId").UifSelect("setSelected", ClaveId);
                    $("#ClaveId").prop('required', true);
                    $("#ClaveLabel").addClass("field-required");
                }
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }
    static getClave() {
        var productId = $("#ProductId").UifSelect("getSelected");
        var prefixId = $("#PrefixId").UifSelect("getSelected");
        MinPremiunRelationRequest.GetClave(productId, prefixId).done(response => {
            let result = response.result;
            $("#ClaveId").prop('required', false);
            $("#ClaveLabel").removeClass("field-required");
            if (response.success) {
                $("#ClaveId").UifSelect({
                    sourceData: result
                });
                if (result.length > 0) {                 
                    $("#ClaveId").prop('required', true);
                    $("#ClaveLabel").addClass("field-required");
                }
            } else {
                $.UifNotify("show", { 'type': "danger", 'message': result, 'autoclose': true })
            }
        });
    }

    static getDataFromDependentOfPrefix() {
        if ($.trim($("#PrefixId").val()) != "") {
            var PrefixId = $("#PrefixId").val();
            if (PrefixId == 7) {               
                $('#ClaveLabel').text('Grupo de cobertura');
            }
            else {
                $('#ClaveLabel').text('Clave');
            }
            MinPremiunRelation.getProductType(PrefixId, null);
            $("#ClaveId").UifSelect({ sourceData: null });
            
        }
    }

    static getAllDataMinPremiunRelation(data) {
        if (data.length >0) {
            $("#listMiniumPremiun").UifListView({
                displayTemplate: "#templateMinPremiunRelation",
                sourceData: data,
                selectionType: 'single',
                height: 400,
                edit: true,
                customEdit: true,
                filterColumns: ['Product.Description', 'EndorsementType.Description', ' Branch.LongDescription']                
            });
        }
        else {
            $.UifNotify("show", { 'type': "danger", 'message': AppResources.ErrorDataNotFound, 'autoclose': true })
        }
           
    }

    static generateFileToExport(data) {
        DownloadFile(data);
    }

    static saveMinPremiumRelation() {
        var data = MinPremiunRelation.getForm();
        if ($("#formMinPremiunRelation").valid()) {
            if (data.StatusTypeService == 2) {
                MinPremiunRelation.createMinPremiumRelation(data)
            } else if (data.StatusTypeService == 3) {
                MinPremiunRelation.updateMinPremiumRelation(data)
            }
        }
        else {
            $.UifNotify("show", { 'type': "danger", 'message': AppResources.DecisionTableErrorFilter, 'autoclose': true })
        }
    }

    static delete() {
        if ($("#Id").val() == 0 || $("#Id").val() == "") {
            MinPremiunRelation.clearForm();
        } else {
            var data = MinPremiunRelation.getForm();
            MinPremiunRelation.deleteMinPremiumRelation(data)
        }
    }

    static createMinPremiumRelation(data) {
        request('Parametrization/MinPremiunRelation/CreateMinPremiunRelation', JSON.stringify({ viewModel: data }), 'POST', AppResources.ErrorSaveMinPremiumRelation, MinPremiunRelation.confirmMinPremiumRelation);
    }

    static updateMinPremiumRelation(data) {
        request('Parametrization/MinPremiunRelation/UpdateMinPremiunRelation', JSON.stringify({ viewModel: data }), 'POST', AppResources.ErrorSaveMinPremiumRelation, MinPremiunRelation.confirmMinPremiumRelation);
    }

    static deleteMinPremiumRelation(data) {
        request('Parametrization/MinPremiunRelation/DeleteMinPremiunRelation', JSON.stringify({ viewModel: data }), 'POST', AppResources.ErrorSaveMinPremiumRelation, MinPremiunRelation.confirmMinPremiumRelation);
    }

    static confirmMinPremiumRelation(data) {
               
        $.UifNotify('show', {
            'type': 'info', 'message': data,
            'autoclose': true
        });
        request('Parametrization/MinPremiunRelation/GetAllMinPremiunRelation', null, 'GET', AppResources.ErrorSearchClauses, MinPremiunRelation.getAllDataMinPremiunRelation);
        MinPremiunRelation.clearForm();
    }

    getSearch(event, value) {
        MinPremiunRelation.getSearch($("#PrefixSearch").val(), $("#inputMinPremiunRelationSearch").val());
       
    }

    redirectIndex() {
        window.location = rootPath + "Home/Index";
    }

    static exportExcel() {
        //request('Parametrization/MinPremiunRelation/GenerateFileToExport', null, 'GET', AppResources.ErrorExportExcel, MinPremiunRelation.generateFileToExport);
      
        MinPremiunRelationRequest.GenerateFileToExport().done(function (data) {
                if (data.success) {
                    try {
                        var a = document.createElement('A');
                        a.href = data.result.Url;
                        a.download = data.result.FileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);

                    } catch (ex) {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });

        
    }
}