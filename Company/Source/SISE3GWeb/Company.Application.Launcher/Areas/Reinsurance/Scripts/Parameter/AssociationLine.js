/**
    * @file   AssociationLine.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var individualId = 0;
var associationLineId = 0;
var policyId = 0;
var lineBusinessIdSelected = 0;//combo ramo técnico
var subLineBusinessIdSelected = 0;//combo sub ramo técnico
var businessTypeIdSelected = 0;//combo tipo de operación
var prefixIdSelected = 0;//combo ramo 
var productIdSelect = 0;// Combo Producto
var timerLoadAssociation = 0;
var typeTimerPrefix = "";
var timePrefix;
var modalPromisePrefix;
var productToSelected = [];


//Model para contener los items chequeados
var oItemsCheckModel = {
    Ids: []
};

var oItemModel = {
    Value: 0
};


/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainAssocationLine();
});

class MainAssocationLine extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {

        $("#divAssociationPrefixTec").hide();
        $("#divAssociationPrefix").hide();
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#modalParams").on("itemSelected", "#SelectPrefix", this.SelectedPrefix);
        $("#modalParams").on("itemSelected", "#PrefixByProduct", this.SelectedProduct);
        $("#selectContractYear").on("itemSelected", this.SelectContractYear);
        $("#LineAssociationType").on("itemSelected", this.SelectedLineAssociationType);
        $("#modalParams").on("itemSelected", "#SelectLineBusiness", this.SelectedLineBusiness);

        $("#modalParams").on("itemSelected", "#SearchInsuredAssociationLine", this.AutocompleteInsuredAssociationLine);
        $("#modalParams").on("itemSelected", "#SearchInsuredNameAssociationLine", this.AutocompleteInsuredNameAssociationLine);
        $("#modalParams").on("itemSelected", "#PolicyNumberAssociationLine", this.AutocompletePolicyNumber);

        $("#modalParams").on("blur", "#PolicyNumberAssociationLine", this.BlurPolicyNumber);

        $("#tblAssociationPrefix").on("rowAdd", this.RowAddTableAssociationPrefix);
        $("#tblAssociationPrefix").on("rowEdit", this.RowEditTableAssociationPrefix);
        $("#tblAssociationPrefix").on("rowDelete", this.RowDeleteTableAssociationPrefix);
        $("#modalDeleteAssociationLine").find("#btnDeleteModal").on("click", this.DeleteAssociationLine);

        $("#modalParams").on("itemSelected", "#SelectSubLineBusiness", this.GetCoverages);
        $("#modalParams").on("blur", "#DateFrom", this.BlurDateFrom);
        $("#modalParams").on("blur", "#DateTo", this.BlurDateTo);
        $("#modalParams").on("binded", "#SelectLineBusiness", this.LineBusinessBind);
    }




    /**
        * Obtiene los productos por ramo.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del ramo seleccionado.
        */
    SelectedPrefix(event, selectedItem) {
        if ($('#SelectPrefix').val() > 0) {
            var controller = "";
            if ($('#LineAssociationType').val() == $("#ViewBagByPrefixRisk").val()) {
                MainAssocationLine.GetInsuredObject(selectedItem.Id);
            }
            else {
                // controller = REINS_ROOT + "Process/GetProductByPrefix?prefixId=" + selectedItem.Id;
                MainAssocationLine.GetProductByPrefix(selectedItem.Id).done(function(response) {
                    $("#PrefixByProduct").UifMultiSelect({ sourceData: response.result });
                });
                
            }
        }
    }

    /**
        * Oculta los mensajes.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del producto seleccionado.
        */
    SelectedProduct(event, selectedItem) {
        $("#alertPrefix").UifAlert('hide');
    }

    /**
        * Obtiene la asociación de líneas por año.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del año seleccionado.
        */
    SelectContractYear(event, selectedItem) {
        MainAssocationLine.RefreshAssociationLine();
    }

    /**
        * Obtiene la asociación de líneas por tipo de asociación.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del tipo de asociación seleccionado.
        */
    SelectedLineAssociationType(event, selectedItem) {
        $("#alertAsoLine").UifAlert('hide');
        $("#divAssociationPrefix").show();
        $("#divAssociationPrefixTec").hide();
        MainAssocationLine.RefreshAssociationLine();
    }

    /**
        * Obtiene los subramnos técnicos por ramo.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del ramo seleccionado.
        */
    SelectedLineBusiness(event, selectedItem) {
        if (selectedItem.Id > 0) {

            var controller = "";

            if ($('#LineAssociationType').val() == $("#ViewBagByLineBusinessSubLineBusinessRisk").val()) {

                controller = REINS_ROOT + "Parameter/GetSubLineBusiness?lineBusiness=" + selectedItem.Id;
                $("#SelectSubLineBusiness").UifSelect({ source: controller });

                MainAssocationLine.GetInsuredObject(selectedItem.Id);
            }
            else {
                controller = REINS_ROOT + "Parameter/GetSubLineBusiness?lineBusiness=" + selectedItem.Id;
                $("#TableSelectSubLine").UifDataTable({ source: controller });
                $("#SelectSubLineBusiness").UifSelect({ source: controller });
            }
        }
    }

    LineBusinessBind(event, data, index) {
        if (lineBusinessIdSelected > 0) {
            $("#SelectLineBusiness").val(lineBusinessIdSelected);
        }
    }

    /**
        * Obtiene los asegurados por número de documento.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del asegurado seleccionado.
        */
    AutocompleteInsuredAssociationLine(event, selectedItem) {

        if (selectedItem.Id > 0) {
            individualId = selectedItem.Id;
            $("#SearchInsuredNameAssociationLine").val(selectedItem.Name);
        }
    }

    /**
        * Obtiene los asegurados por nombre.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del asegurado seleccionado.
        */
    AutocompleteInsuredNameAssociationLine(event, selectedItem) {

        if (selectedItem.Id > 0) {
            individualId = selectedItem.Id;
            $("#SearchInsuredAssociationLine").val(selectedItem.DocumentNumber);
        }
    }

    /**
        * Obtiene las pólizas por número.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la póliza seleccionada.
        */
    AutocompletePolicyNumber(event, selectedItem) {
        if (selectedItem.Id > 0) {
            policyId = selectedItem.Id;
            $("#alertPrefix").UifAlert('hide');
            $("#PolicyNumberAssociationLine").blur(); 
        }
    }

    /**
        * Agrega un registro de asociación de línea.
        *
        * @param {String} event - Agregar.
        * @param {Object} data  - Objeto con valores nulos.
        */
    RowAddTableAssociationPrefix(event, data) {

        $('#formAssociationLine').validate();
        if ($('#formAssociationLine').valid()) {
            $("#alertAsoLine").UifAlert('hide');
            MainAssocationLine.SetPanels(true);
            $('#modalParams').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddAssociationLine?lineAssociationTypeId='
                + $("#LineAssociationType").val() + '&associationLineId=0' + '&year=' + $("#selectContractYear").val(),
                Resources.NewRegister + ":  " + $("#selectContractYear option:selected").text() + " / " + $("#LineAssociationType option:selected").text());
            MainAssocationLine.CheckModalAssociation('modalParams');
            lockScreen();

            if ($("#LineAssociationType").val() == "3") {
               MainAssocationLine.GetOperations().done(function (response) {
                        $("#SelectTypeOperation").UifSelect({ sourceData: response.result });
                });
            }

            setTimeout(function () {
                modalPromisePrefix.then(function (isShown) {
                    if (isShown) {
                        clearTimeout(timePrefix);
                        MainAssocationLine.SetPanels(false);
                        unlockScreen();
                    }
                });
            }, 300);


        }
        else {
            $("#tblAssociationPrefix").UifDataTable('clear');
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.RequiredFieldsMissing, 'autoclose': true });
        }
    }

    /**
        * Edita un registro de asociación de líneas.
        *
        * @param {String} event       - Editar.
        * @param {Object} selectedRow - Objeto con valores de la asociación de líneas seleccionada.
        */
    RowEditTableAssociationPrefix(event, selectedRow) {
        MainAssocationLine.CleanVariables();
        $("#alertAsoLine").UifAlert('hide');
        $('#modalParams').appendTo("body").UifModal('show', REINS_ROOT + 'Parameter/AddAssociationLine?lineAssociationTypeId=' +
            $("#LineAssociationType").val() + '&associationLineId=' + selectedRow.AssociationLineId +
            '&year=' + $("#selectContractYear").val(), Resources.EditRecord + ":  " + $("#selectContractYear option:selected").text() +
            " / " + $("#LineAssociationType option:selected").text());
        MainAssocationLine.SetPanels(false);
        MainAssocationLine.CheckModalAssociation('modalParams');
        modalPromisePrefix.then(function (isShown) {
            if (isShown) {
                clearTimeout(timePrefix);
                MainAssocationLine.LoadControlsEdit(selectedRow);

                lockScreen();

                setTimeout(function () {
                    timerLoadAssociation = window.setInterval(MainAssocationLine.LoadScrenAssociationLine, 1000);
                }, 1000);
            }
        });
    }

    /**
        * Elimina un registro de asociación de líneas.
        *
        * @param {String} event - Eliminar.
        * @param {Object} data  - Objeto con valores de la asociación de línea seleccionada a eliminar.
        */
    RowDeleteTableAssociationPrefix(event, data) {
        $.ajax({
            url: REINS_ROOT + "Parameter/LineIsUsed",
            data: { "lineId": data.LineId },
            success: function (dat) {
                if (dat.success) {
                    if (dat.result) {
                        $.UifNotify('show', { 'type': 'danger', 'message': Resources.AssociationLineIsUsed, 'autoclose': true });
                    }
                    else {
                        $('#modalDeleteAssociationLine').UifModal('showLocal', Resources.Lines);
                        associationLineId = data.AssociationLineId;
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': dat.result, 'autoclose': true });
                }
            },
        });
    }

    /**
        * Elimina un registro de asociación de líneas.
        *
        */
    DeleteAssociationLine() {
        $('#modalDeleteAssociationLine').modal('hide');
        $.ajax({
            url: REINS_ROOT + "Parameter/DeleteAssociationLines",
            data: { "associationLineId": associationLineId },
            success: function (data) {
                if (data) {
                    MainAssocationLine.RefreshAssociationLine();
                }
            }
        });
    }

    /**
    * Obtiene Cobertura por línea y sublinea.
    *
    * @param {String} event        - Seleccionar.
    * @param {Object} selectedItem - Objeto con valores del ramo seleccionado.
    */
    GetCoverages(event, selectedItem) {
        if (selectedItem.Id > 0) {
            MainAssocationLine.SetCoveragesValues($("#modalParams").find("#SelectLineBusiness").val(), selectedItem.Id);
        }
    }

    BlurDateFrom() {
        if ($("#modalParams").find('#DateTo').val() != "") {
            if (compare_dates($("#modalParams").find('#DateFrom').val(), $("#modalParams").find('#DateTo').val())) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateDateTo, 'autoclose': true });
                $("#modalParams").find('#DateFrom').val('');
            } else {
                $("#modalParams").find('#DateFrom').val($("#modalParams").find('#DateFrom').val());
                $("#alertPrefix").UifAlert('hide');
            }
        }
    }

    BlurPolicyNumber() {
        setTimeout(function () {
            if ($("#modalParams").find('#PolicyNumberAssociationLine').val() != "") {
                $("#PolicyNumberAssociationLine").UifAutoComplete("setValue", $("#PolicyNumberAssociationLine").UifAutoComplete("getValue").toString(10));
                if (policyId == 0) {
                    $("#modalParams").find('#PolicyNumberAssociationLine').val("");
                }
            }
            else {
                $("#modalParams").find('#PolicyNumberAssociationLine').val("");
            }

        }, 100);

    }


    BlurDateTo() {
        if ($("#modalParams").find('#DateFrom').val() != "") {
            if (compare_dates($("#modalParams").find('#DateFrom').val(), $("#modalParams").find('#DateTo').val())) {

                $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateDateTo, 'autoclose': true });

                $("#modalParams").find('#DateTo').val('');
            } else {
                $("#modalParams").find('#DateTo').val($("#modalParams").find('#DateTo').val());
                $("#alertPrefix").UifAlert('hide');
            }
        }
    }




    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Visualiza el modal de agregación o edición de asociación de línea.
        *
        * @param {String} modalName - Nombre del modal.
        */
    static CheckModalAssociation(modalName) {
        var isShown;
        return modalPromisePrefix = new Promise(function (resolve, reject) {
            timePrefix = setInterval(function () {
                if ($('#' + modalName).is(":visible")) {
                    isShown = true;
                    resolve(isShown);
                }
            }, 3);
        });
    }

    /**
        * Setea los campos al editar una sociación de línea.
        *
        * @param {Object} selectedRow - Objeto con valores del registro seleccionado.
        */
    static LoadControlsEdit(selectedRow) {
        if ($('#LineAssociationType').val() == $("#ViewBagByLineBusiness").val()) {//1
            typeTimerPrefix = "1";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {

            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByLineBusinessSubLineBusiness").val()) {//2
            typeTimerPrefix = "2";

            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {
                $("#SelectLineBusiness").val(lineBusinessIdSelected);
                setTimeout(function () {
                    $("#SelectLineBusiness").trigger("change");
                }, 2000);
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByOperationTypePrefix").val()) {//3
            typeTimerPrefix = "3";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function (oItemsCheckModel) {
                MainAssocationLine.GetOperations().done(function (response) {
                    $("#SelectTypeOperation").UifSelect({ sourceData: response.result });
                    $("#SelectTypeOperation").trigger("change");
                    $("#SelectTypeOperation").UifSelect("setSelected", businessTypeIdSelected);
                });
                    
               
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByInsured").val()) {//4
            typeTimerPrefix = "4";

            setTimeout(function () {
                individualId = parseInt($("#ValueFrom").val()); //recupera id del asegurado    
            }, 150);

            setTimeout(function () {
                MainAssocationLine.SetInsured(individualId);
            }, 400);
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByPrefix").val()) {//5
            typeTimerPrefix = "5";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByPolicy").val()) {//6
            typeTimerPrefix = "6";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {
                setTimeout(function () {
                    MainAssocationLine.SetPolicy(policyId);
                }, 400);
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByFacultativeIssue").val()) {//7
            typeTimerPrefix = "7";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {

            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByInsuredPrefix").val()) {//8
            typeTimerPrefix = "8";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {

                setTimeout(function () {
                    MainAssocationLine.SetInsured(individualId);
                }, 350);
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewByPrefixProduct").val()) {//13
            typeTimerPrefix = "9";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function (oItemsCheckModel) {
                setTimeout(function () {
                    $("#SelectPrefix").val(prefixIdSelected);
                    MainAssocationLine.BindedProduct(prefixIdSelected, productToSelected);
                }, 1500);
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByLineBusinessSubLineBusinessRisk").val()) {//9
            typeTimerPrefix = "10";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {
                setTimeout(function () {
                    $("#SelectLineBusiness").UifSelect("setSelected", lineBusinessIdSelected);
                    var controller = REINS_ROOT + "Parameter/GetSubLineBusiness?lineBusiness=" + lineBusinessIdSelected;
                    $("#SelectSubLineBusiness").UifSelect({ source: controller });
                }, 1500);
                $("#SelectSubLineBusiness").UifSelect("setSelected", subLineBusinessIdSelected);
                MainAssocationLine.GetInsuredObject(lineBusinessIdSelected); //llena la grilla
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByPrefixRisk").val()) {//10
            typeTimerPrefix = "11";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {
                setTimeout(function () {
                    MainAssocationLine.GetInsuredObject(prefixIdSelected);
                    $("#SelectPrefix").val(prefixIdSelected);
                }, 1500);
            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByPolicyLineBusinessSubLineBusiness").val()) {//11
            typeTimerPrefix = "12";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function () {

                setTimeout(function () {
                    $("#SelectLineBusiness").val(lineBusinessIdSelected);
                    var controller = REINS_ROOT + "Parameter/GetSubLineBusiness?lineBusiness=" + lineBusinessIdSelected;
                    $("#SelectSubLineBusiness").UifSelect({ source: controller });
                }, 1000);

                setTimeout(function () {
                    $("#SelectSubLineBusiness").val(subLineBusinessIdSelected);
                }, 1200);

                setTimeout(function () {
                    MainAssocationLine.SetPolicy(policyId);
                }, 2000);


            });
        }
        else if ($('#LineAssociationType').val() == $("#ViewBagByLineBusinessSubLineBusinessCoverage").val()) {//12
            typeTimerPrefix = "13";
            MainAssocationLine.LoadDataGrid(selectedRow.AssociationLineId).then(function (oItemsCheckModel) {

                setTimeout(function () {
                    $("#SelectLineBusiness").val(lineBusinessIdSelected);
                    var controller = REINS_ROOT + "Parameter/GetSubLineBusiness?lineBusiness=" + lineBusinessIdSelected;
                    $("#SelectSubLineBusiness").UifSelect({ source: controller });
                }, 1200);

                setTimeout(function () {
                    $("#SelectSubLineBusiness").val(subLineBusinessIdSelected);
                    MainAssocationLine.SetCoveragesValues(lineBusinessIdSelected, subLineBusinessIdSelected);
                }, 3000);

            });

        }
    }

    /**
        * Valida si esta seleccionado un ramo técnico.
        *
        */
    static ValidSelectLineBusiness() {

        if ($("#SelectLineBusiness").val() != "" && $("#SelectLineBusiness").val() != null) {
            return true;
        }
        else {
            $("#SelectLineBusiness").val(lineBusinessIdSelected);
            $("#SelectLineBusiness").trigger("change");

            return true;
        }
    }

    /**
        * Valida si se cargaron los controles muestra pantalla.
        *
        * @param {Object} selectedRow - Objeto con valores del registro seleccionado.
        */
    static LoadScrenAssociationLine() {

        var resulTimer = false;

        if (typeTimerPrefix == "1") {
            if ($("#TableSelectLineBusiness").UifDataTable("getData").length > 0) {
                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TableSelectLineBusiness");
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        }
        else if (typeTimerPrefix == "2") {
            if ((MainAssocationLine.ValidSelectLineBusiness() == true)
                && ($("#TableSelectSubLine").UifDataTable("getData").length > 0)) {
                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TableSelectSubLine");
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
            else {
                MainAssocationLine.ValidSelectLineBusiness();
            }
        }
        else if (typeTimerPrefix == "3") {
            if (($("#SelectTypeOperation").val() != "" && $("#SelectTypeOperation").val() != null)
                && ($("#TablePrefix").UifDataTable("getData").length > 0)) {
                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TablePrefix");
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        }
        else if (typeTimerPrefix == "4") {
            if (($("#SearchInsuredAssociationLine").val() != "" && $("#SearchInsuredAssociationLine").val() != null) &&
                ($("#SearchInsuredNameAssociationLine").val() != "" && $("#SearchInsuredNameAssociationLine").val() != null)) {
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        }
        else if (typeTimerPrefix == "5") {
            if ($("#TablePrefix").UifDataTable("getData").length > 0) {
                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TablePrefix");
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        }
        else if (typeTimerPrefix == "7") {
            if ($("#TablePrefix").UifDataTable("getData").length > 0) {
                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TablePrefix");
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        } else if (typeTimerPrefix == "8") {
            if (($("#SearchInsuredAssociationLine").val() != "" && $("#SearchInsuredAssociationLine").val() != null) &&
                ($("#SearchInsuredNameAssociationLine").val() != "" && $("#SearchInsuredNameAssociationLine").val() != null)
                && $("#TablePrefix").UifDataTable("getData").length > 0) {
                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TablePrefix");
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        }
        else if (typeTimerPrefix == "9") {

            if (($("#SelectPrefix").val() != "" && $("#SelectPrefix").val() != null) &&
                ($('#PrefixByProduct').UifMultiSelect('getSelected').length > 0)) {
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
            else {
                $("#SelectPrefix").val(prefixIdSelected);
                MainAssocationLine.BindedProduct(prefixIdSelected, productIdSelect);
            }
        }
        else if (typeTimerPrefix == "10") {

            if ((MainAssocationLine.ValidSelectLineBusiness() == true) && ($("#SelectSubLineBusiness").val() != "" && $("#SelectSubLineBusiness").val() != null)
                && ($("#TableInsuredObject").UifDataTable("getData").length > 0)) {

                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TableInsuredObject");
                clearInterval(timerLoadAssociation);
                resulTimer = true;

            } else {

                if (lineBusinessIdSelected > 0) {

                    $("#SelectPrefix").val(lineBusinessIdSelected);

                    setTimeout(function () {
                        $("#SelectSubLineBusiness").val(subLineBusinessIdSelected);
                    }, 1000);
                }
            }
        }
        else if (typeTimerPrefix == "11") {

            if (($("#SelectPrefix").val() != "" && $("#SelectPrefix").val() != null) && ($("#TableInsuredObject").UifDataTable("getData").length > 0)) {

                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TableInsuredObject");
                clearInterval(timerLoadAssociation);
                resulTimer = true;

            }
        }
        else if (typeTimerPrefix == "12") {

            if ((MainAssocationLine.ValidSelectLineBusiness() == true) && ($("#SelectSubLineBusiness").val() != "" && $("#SelectSubLineBusiness").val() != null)) {
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        }
        else if (typeTimerPrefix == "13") {
            if ((MainAssocationLine.ValidSelectLineBusiness() == true) && ($("#SelectSubLineBusiness").val() != "") && ($("#SelectSubLineBusiness").val() != null)
                && ($("#TableCoverages").UifDataTable("getData").length > 0)) {

                MainAssocationLine.FillItemsTable(oItemsCheckModel.Ids, "TableCoverages");
                clearInterval(timerLoadAssociation);
                resulTimer = true;
            }
        }
        else {
            clearInterval(timerLoadAssociation);
        }
        MainAssocationLine.SetPanels(false);

        if (resulTimer) {
            unlockScreen();
        }
    }

    /**
        * Setea controles de asegurado.
        *
        * @param {Number} insuredId - Identificador de asegurado.
        */
    static SetInsured(insuredId) {
        $.ajax({
            url: REINS_ROOT + "Parameter/GetInsuredById",
            data: { "individualId": insuredId },
            success: function (data) {

                $("#SearchInsuredAssociationLine").val(data.IdentificationNumber);
                $("#SearchInsuredNameAssociationLine").val(data.FullName);
            }
        });
    }

    /**
        * Setea controles de póliza.
        *
        * @param {Number} policyId - Identificador de póliza.
        */
    static SetPolicy(policyId) {

        setTimeout(function () {
            $.ajax({
                url: REINS_ROOT + "Parameter/GetPolicyById",
                data: { "policyId": policyId },
                success: function (data) {

                    if (data.length == 0) {
                        $("#PolicyNumberAssociationLine").val("");
                        $("#BranchAssociationLine").val("");
                        $("#PrefixAssociationLine").val("");
                    }
                    else if (data.Id > 0) {
                        $("#PolicyNumberAssociationLine").val(data.DocumentNumber);
                        $("#BranchAssociationLine").val(data.Branch.Id);
                        $("#PrefixAssociationLine").val(data.Prefix.Id);
                    }
                    $.unblockUI();
                }
            });
        }, 400);
    }


    /**
        * Muestra/oculta los controles dependiendo del tipo de asociación.
        *
        * @param {Booleano} isNew - Si es nuevo o edición.
        */
    static SetPanels(isNew) {

        setTimeout(function () {

            if ($('#LineAssociationType').val() == $("#ViewBagByLineBusiness").val()) {//1

                $("#LineBusinessTablePanel").show();
                $('#TableSelectLineBusiness').UifDataTable('order', [1, 'asc']); /// asc || desc
                $("#InsuredPanel").hide();
                $("#PolicyPanel").hide();
                $("#SelectBusinessType").hide();
                $("#LineBusinessComboPanel").hide();
                $("#SubLineTablePanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByLineBusinessSubLineBusiness").val()) {//2

                $("#SubLineTablePanel").show();
                $("#LineBusinessComboPanel").show();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#InsuredPanel").hide();
                $("#PolicyPanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByOperationTypePrefix").val()) {//3

                $("#SelectBusinessType").show();
                $("#SelectPrefixPanel").show();
                $('#TablePrefix').UifDataTable('order', [1, 'asc']); /// asc || desc
                $("#LineBusinessComboPanel").hide();
                $("#InsuredPanel").hide();
                $("#PolicyPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SubLineTablePanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByInsured").val()) {//4

                $("#InsuredPanel").show();
                $("#PolicyPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#LineBusinessComboPanel").hide();
                $("#SubLineTablePanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByPrefix").val()) {//5

                $("#SelectPrefixPanel").show();
                $('#TablePrefix').UifDataTable('order', [1, 'asc']); /// asc || desc
                $("#PrefixComboPanel").hide();
                $("#InsuredPanel").hide();
                $("#PolicyPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#LineBusinessComboPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#SubLineTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByPolicy").val()) {//6

                $("#PolicyPanel").show();
                $("#InsuredPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#LineBusinessComboPanel").hide();
                $("#SubLineTablePanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByFacultativeIssue").val()) {//7

                $("#SelectPrefixPanel").show();
                $('#TablePrefix').UifDataTable('order', [1, 'asc']); /// asc || desc
                $("#PrefixComboPanel").hide();
                $("#InsuredPanel").hide();
                $("#PolicyPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#LineBusinessComboPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#SubLineTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByInsuredPrefix").val()) {//8

                $("#InsuredPanel").show();
                $("#SelectPrefixPanel").show();
                $('#TablePrefix').UifDataTable('order', [1, 'asc']); /// asc || desc
                $("#LineBusinessComboPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#PolicyPanel").hide();
                $("#SelectBusinessType").hide();
                $("#SubLineTablePanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByLineBusinessSubLineBusinessRisk").val()) {//9

                $("#LineBusinessComboPanel").show();
                $("#SubLineBusinessComboPanel").show();
                $("#AutocompleteRiskPanel").show();
                $("#InsuredPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#PolicyPanel").hide();
                $("#SelectBusinessType").hide();
                $("#SubLineTablePanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByPrefixRisk").val()) //10 POR RAMO / RIESGO
            {
                $("#PrefixComboPanel").show();
                $("#AutocompleteRiskPanel").show();
                $("#LineBusinessComboPanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#InsuredPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#PolicyPanel").hide();
                $("#SelectBusinessType").hide();
                $("#SubLineTablePanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByPolicyLineBusinessSubLineBusiness").val()) {//11

                $("#PolicyPanel").show();
                $("#LineBusinessComboPanel").show();
                $("#SubLineBusinessComboPanel").show();
                $("#InsuredPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#SubLineTablePanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewBagByLineBusinessSubLineBusinessCoverage").val()) {//12  covertura

                $("#LineBusinessComboPanel").show();
                $("#SubLineBusinessComboPanel").show();
                $("#AutocompleteCoveragePanel").show();
                $("#InsuredPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#SubLineTablePanel").hide();
                $("#PrefixComboPanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#PolicyPanel").hide();
                $("#PrefixProductPanel").hide();
            }
            else if ($('#LineAssociationType').val() == $("#ViewByPrefixProduct").val()) {//13
                $("#PrefixComboPanel").show();
                $("#PrefixProductPanel").show();
                $("#LineBusinessComboPanel").hide();
                $("#SubLineBusinessComboPanel").hide();
                $("#AutocompleteCoveragePanel").hide();
                $("#InsuredPanel").hide();
                $("#LineBusinessTablePanel").hide();
                $("#SelectBusinessType").hide();
                $("#SubLineTablePanel").hide();
                $("#SelectPrefixPanel").hide();
                $("#SubPrefixTablePanel").hide();
                $("#AutocompleteRiskPanel").hide();
                $("#PolicyPanel").hide();
            }
            if (isNew) {
                var dateFrom = '01/01/' + $("#selectContractYear").UifSelect("getSelectedText");
                var resultDate = addDaysToDate((dateAddYear(dateFrom, 1)), -1)

                $('#modalParams').find("#DateFrom").UifDatepicker('setValue', dateFrom);
                $('#modalParams').find("#DateTo").UifDatepicker('setValue', resultDate);
            }
        }, 300);
    }

    /**
        * Carga los datos en el modal dependiendo del tipo de asociación.
        *
        * @param {Number} associationLineIdSelected - Identificador de la asociación de línea.
        */
    static LoadDataGrid(associationLineIdSelected) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: REINS_ROOT + "Parameter/GetAssociationLine",
                data: JSON.stringify({
                    "year": $("#selectContractYear").val(), "associationTypeId": $("#LineAssociationType").val(),
                    "associationLineId": associationLineIdSelected
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    oItemsCheckModel.Ids = [];

                    for (var i in data.aaData) {

                        var oItemModel = { Value: 0 };

                        if (data.aaData[i].AssociationTypeId == $("#ViewBagByLineBusiness").val()) {

                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.LINE_BUSINESS") {
                                oItemModel.Value = data.aaData[i].ValueFrom;
                                oItemsCheckModel.Ids.push(oItemModel);
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByLineBusinessSubLineBusiness").val()) {

                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.SUB_LINE_BUSINESS") {
                                oItemModel.Value = data.aaData[i].ValueFrom;
                                oItemsCheckModel.Ids.push(oItemModel);
                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.LINE_BUSINESS") {
                                lineBusinessIdSelected = data.aaData[i].ValueFrom;
                                $("#SelectLineBusiness").trigger("change");
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByOperationTypePrefix").val() ||
                            data.aaData[i].AssociationTypeId == $("#ViewBagByFacultativeIssue").val() ||
                            data.aaData[i].AssociationTypeId == $("#ViewBagByPrefix").val()) {

                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.PREFIX") {
                                oItemModel.Value = data.aaData[i].ValueFrom;
                                oItemsCheckModel.Ids.push(oItemModel);
                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "PARAM.BUSINESS_TYPE") {
                                businessTypeIdSelected = data.aaData[i].ValueFrom;
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByPolicy").val()) {//6
                            if (data.aaData[i].LineBusinessDescriptionFrom == "ISS.POLICY") {
                                policyId = data.aaData[i].ValueFrom;
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByInsuredPrefix").val()) {
                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.PREFIX") {
                                oItemModel.Value = data.aaData[i].ValueFrom;
                                oItemsCheckModel.Ids.push(oItemModel);
                            }
                            else {
                                individualId = data.aaData[i].ValueFrom;
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewByPrefixProduct").val()) {//13
                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.PREFIX") {
                                prefixIdSelected = data.aaData[i].ValueFrom;
                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "PROD.PRODUCT") {
                                productToSelected.push(data.aaData[i].ValueFrom);
                                
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByLineBusinessSubLineBusinessRisk").val()) {//9
                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.LINE_BUSINESS") {
                                lineBusinessIdSelected = data.aaData[i].ValueFrom;

                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.SUB_LINE_BUSINESS") {
                                subLineBusinessIdSelected = data.aaData[i].ValueFrom;

                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "QUO.INSURED_OBJECT") {
                                oItemModel.Value = data.aaData[i].ValueFrom;
                                oItemsCheckModel.Ids.push(oItemModel);
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByPrefixRisk").val()) {//10
                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.PREFIX") {
                                prefixIdSelected = data.aaData[i].ValueFrom;

                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "QUO.INSURED_OBJECT") {
                                $("#SelectPrefix").val(prefixIdSelected);
                                oItemModel.Value = data.aaData[i].ValueFrom;
                                oItemsCheckModel.Ids.push(oItemModel);
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByPolicyLineBusinessSubLineBusiness").val()) {//11
                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.LINE_BUSINESS") {
                                lineBusinessIdSelected = data.aaData[i].ValueFrom;
                            }
                            else if (data.aaData[i].TableName == "COMM.SUB_LINE_BUSINESS") {
                                subLineBusinessIdSelected = data.aaData[i].ValueFrom;
                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "ISS.POLICY") {
                                policyId = data.aaData[i].ValueFrom;
                            }
                        }
                        else if (data.aaData[i].AssociationTypeId == $("#ViewBagByLineBusinessSubLineBusinessCoverage").val()) {//12
                            if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.LINE_BUSINESS") {
                                lineBusinessIdSelected = data.aaData[i].ValueFrom;
                                $("#SelectLineBusiness").val(lineBusinessIdSelected);
                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "COMM.SUB_LINE_BUSINESS") {
                                subLineBusinessIdSelected = data.aaData[i].ValueFrom;
                            }
                            else if (data.aaData[i].LineBusinessDescriptionFrom == "QUO.COVERAGE") {
                                oItemModel.Value = data.aaData[i].ValueFrom;
                                oItemsCheckModel.Ids.push(oItemModel);
                            }
                        }


                    }
                    resolve(oItemsCheckModel);
                }
            });
        });
    }



    /**
        * Chequea los registros de la tabla que estén grabados.
        *
        * @param {Object} items - Objeto con valores a setear en la tabla.
        * @param {String} table - Nombre de la tabla según el tipo de asociación de línea.
        */
    static FillItemsTable(items, table) {
        if (items != null) {
            $.each(items, function (i, value) {
                var tableId = $("#" + table).UifDataTable("getData");
                if (tableId.length > 0) {
                    for (i in tableId) {
                        if (tableId[i].Id == value.Value) {
                            var selectedValue = { label: "Id", values: [value.Value] }
                            $("#" + table).UifDataTable('setSelect', selectedValue);
                            break;
                        }
                    }
                }
            });
        }

    }

    /**
        * Refresca la grilla de asociación de lìneas.
        *
        */
    static RefreshAssociationLine() {

        if ($("#selectContractYear").val() > 0 && $("#LineAssociationType").val() > 0) {

            var controller = REINS_ROOT + "Parameter/GetAssociationLine?year=" + $("#selectContractYear").val() +
                '&associationTypeId=' + $("#LineAssociationType").val() + "&associationLineId=0";

            $('#tblAssociationPrefix').UifDataTable({ source: controller });
            $('#tblAssociationPrefix').UifDataTable('order', [1, 'asc']);

            MainAssocationLine.CleanVariables();
        }
    }

    /**
        * Limpia los campos del modal.
        *
        */
    static CleanVariables() {
        individualId = 0;
        associationLineId = 0;
        policyId = 0;

        lineBusinessIdSelected = 0;//combo ramo técnico
        subLineBusinessIdSelected = 0;//combo sub ramo técnico
        businessTypeIdSelected = 0;//combo tipo de operación
        prefixIdSelected = 0;//combo ramo 
        productIdSelect = 0;

        //Model para contener los items chequeados
        oItemsCheckModel = {
            Ids: []
        };

        oItemModel = {
            Value: 0
        };
    }

    /**
        * Encera la póliza cada vez que se digite un número.
        *
        * @param {Object} event        - Seleccionar.
        * @param {String} policyNumber - Número de póliza.
        */
    static SetPolicyZero(event, policyNumber) {
        policyId = 0
        return JustNumbers(event, policyNumber);
    }

    /**
        * Obtiene los productos por ramo.
        *
        * @param {Number} prefixId  - Identificador de ramo.
        * @param {Number} productId - Identificador de producto.
        */
    static BindedProduct(prefixId, productToSelected) {

        if (prefixId > 0) {

            //var controller = REINS_ROOT + "Process/GetProductByPrefix?prefixId=" + prefixId;
            MainAssocationLine.GetProductByPrefix(prefixId).done(function (response) {
                $("#PrefixByProduct").UifMultiSelect({ sourceData: response.result });
                $("#SelectPrefix").trigger('change');
                $("#PrefixByProduct").UifMultiSelect('setSelected', productToSelected );
                
            });
            
           // $('#PrefixByProduct').UifMultiSelect('setSelected', productToSelected);
            //$("#PrefixByProduct").val(productId);
        }
    }

    /**
        * Obtiene los subramos dado el identificador del ramo técnico.
        *
        * @param {Number} lineBusinessId  - Identificador de ramo.
        */
    static SetLineBusines(lineBusinessId) {
        var result = false;
        return new Promise(function (resolve, reject) {

            if ($("#TableSelectSubLine").UifDataTable("getData").length > 0) {
                result = true;
            }
            else {
                result = false;
            }
            resolve(result);
        });
    }


    static SetCoveragesValues(lineBusinessId, subLineBusinessId) {
        var controller = REINS_ROOT + "Parameter/GetCoveragesByLineBusinessIdSubLineBusinessId?lineBusinessId="
            + lineBusinessId + "&subLineBusinessId=" + subLineBusinessId;
        $("#TableCoverages").UifDataTable({ source: controller });
    }

    static GetInsuredObject(prefixId) {
        var controller = REINS_ROOT + "Parameter/GetInsuredObjectByPrefixIdList?prefixId="
            + prefixId;
        $("#TableInsuredObject").UifDataTable({ source: controller });
    }

   static GetOperations() {
        return $.ajax({
            type: 'GET',
            url: REINS_ROOT + "Parameter/GetOperations",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProductByPrefix(prefixId) {
        return $.ajax({
            async: false,
            type: 'GET',
            url: REINS_ROOT + "Process/GetProductByPrefix?prefixId=" + prefixId,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}