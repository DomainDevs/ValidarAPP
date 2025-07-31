var dateToTop = "";
var dateFromTop = "";
var dateSystem = "";
var yearTop = 0;
var totalRecords = 0;
var prefixSelect = null;
var productSelect = 1;

//Xhr de autocomplete de varios parametros
$(document).ajaxSend(function (event, xhr, settings) {
    //solo ejecute cuando venga de Asociación de Líneas Reaseguros
    if ($("#BranchAssociationLine").val() != undefined && $("#PrefixAssociationLine").val() != undefined) {

        if (event.currentTarget.activeElement.id === "PolicyNumberAssociationLine") {
            if (settings.url.indexOf("GetPolicies") !== -1) {

                settings.url = settings.url + "&param=" + $("#BranchAssociationLine").val() + "/" + $("#PrefixAssociationLine").val();
            }
        }
    }
});

//Obtiene la fecha del sistema
getDateServer();

$('#modalParams').on('blur', '#DateFrom', function (event, selectedItem) {
    dateToTop = dateAddYear($('#modalParams').find("#DateFrom").val(), 3);
    dateToTop = addDaysToDate(dateToTop, -1);

});

$("#modalParams").on("keypress", "#PolicyNumberAssociationLine", function (event, selectedItem) {
    if ($.trim($("#BranchAssociationLine").UifSelect("getSelected")) == "" || $.trim($("#PrefixAssociationLine").UifSelect("getSelected")) == "") {
        $("#BranchAssociationLine").valid();
        $("#PrefixAssociationLine").valid();
        $("#PolicyNumberAssociationLine").UifAutoComplete("clean");
        $("#PolicyNumberAssociationLine").focusout();
    }
});

var oAsociationLine = {
    LineId: 0,
    AssociationLineId: 0,
    AssociationColumnId: 0,
    AssociationTypeId: 0,
    LineBusiness: 0,
    DateFrom: null,
    DateTo: null,
    ByLineBusiness: null,
    ByLineBusinessSubLineBusiness: null,
    ByOperationTypePrefix: null,
    ByInsured: null,
    ByPolicy: null,
    ByFacultativeIssue: null,
    ByInsuredPrefix: null,
    ByLineBusinessSubLineBusinessRisk: null,
    ByPolicyLineBusinessSubLineBusiness: null,
    ByPrefixRisk: null,
    ByLineBusinessSubLineBusinessCoverage: null,
    ByPrefixProduct: null,
    ByPrefix : null
};

//POR RAMO TECNICO 
var oByLineBusinessModel = {
    LineBusiness: [],
};

var oLineBusinessModel = {
    Id: 0
}

//POR RAMO / SUBRAMO TECNICO
var oByLineBusinessSubLineBusinessModel = {
    LineBusiness: null,
    SubLineBusiness: []
};

var oSubLineBusinessModel = {
    Id: 0
};

//POR RAMO/TIPO DE OPERACION
var oByOperationTypePrefixModel = {
    BusinessType: 0,
    Prefixes: []
}

var oPrefixModel = {
    Id: 0
}

//POR ASEGURADO
var oByInsuredModel = {
    Insured: null
}

var oIndividualModel = {
    IndividualId: 0
}

// POR RAMO COMERCIAL
var oByPrefixModel = {
    Prefix: []
};

//POR PÓLIZA
var oByPolicyModel = {
    Policy: null
}

var oPolicyModel = {
    Id: 0
}

//POR FACULTATIVO
var oByFacultativeIssueModel = {

    Prefixes: []
}

//POR ASEGURADO / RAMOS
var oByInsuredPrefixModel = {
    Insured: null,
    Prefixes: []
}

//POR RAMO TECNICO /SUBRAMO TECNICO /RIESGO
var oByLineBusinessSubLineBusinessRiskModel = {
    LineBusiness: null,
    SubLineBusiness: null,
    InsuredObject: []
};

//8. POR RAMO/ Objeto de Seguro(RIESGO)
var oByPrefixRiskModel = {
    PrefixId: 0,
    InsuredObject: []
};

//Objeto de Seguro
var oInsuredObjectModel = {
    Id: 0
};

//POR RAMO TECNICO /SUBRAMO TECNICO /COBERTURA
var oByLineBusinessSubLineBusinessCoverageModel = {
    LineBusiness: null,
    SubLineBusinessId: null,
    Coverage: []
};

//COBERTURA
var oCoverageModel = {
    Id: 0
};


//POR RAMO TECNICO /SUBRAMO TECNICO /POLIZA
var oByPolicyLineBusinessSubLineBusinessModel = {
    LineBusiness: null,
    SubLineBusinessId: null,
    Policy: null
};

//POR RAMO/PRODUCTO
var oByPrefixProductModel = {
    Prefix: null,
    Products: []
};

var oProductModel = {
    Id: 0
}

var oPrefixModel = {
    Id: 0
}

function CleanAsociationLine() {

    dateToTop = "";
    dateFromTop = "";
    dateSystem = "";
    yearTop = 0;
    totalRecords = 0;
    prefixSelect = null;

    oAsociationLine = {
        LineId: 0,
        AssociationLineId: 0,
        AssociationColumnId: 0,
        AssociationTypeId: 0,
        LineBusiness: 0,
        DateFrom: null,
        DateTo: null,
        ByLineBusiness: null,
        ByLineBusinessSubLineBusiness: null,
        ByOperationTypePrefix: null,
        ByInsured: null,
        ByPolicy: null,
        ByFacultativeIssue: null,
        ByInsuredPrefix: null,
        ByLineBusinessSubLineBusinessRisk: null,
        ByPolicyLineBusinessSubLineBusiness: null,
        ByPrefixRisk: null,
        ByLineBusinessSubLineBusinessCoverage: null,
        ByPrefixProduct: null,
        ByPrefix: null
    };

    //POR RAMO TECNICO y

    oByLineBusinessModel = {
        LineBusiness: [],
    };

    oLineBusinessModel = {
        Id: 0
    }

    //POR RAMO / SUBRAMO TECNICO
    oByLineBusinessSubLineBusinessModel = {
        LineBusiness: null,
        SubLineBusiness: []
    };

    oSubLineBusinessModel = {
        Id: 0
    };

    //POR RAMO/TIPO DE OPERACION
    oByOperationTypePrefixModel = {
        BusinessType: 0,
        Prefixes: []
    }

    oPrefixModel = {
        Id: 0
    }

    //POR ASEGURADO
    oByInsuredModel = {
        Insured: null
    }

    oIndividualModel = {
        IndividualId: 0
    }

    //POR RAMO COMERCIAL
    oByPrefixModel = {
        Prefix: null
    }

    //POR PÓLIZA
    oByPolicyModel = {
        Policy: null
    }

    oPolicyModel = {
        Id: 0
    }

    //POR FACULTATIVO
    oByFacultativeIssueModel = {
        Prefixes: []
    }

    //POR ASEGURADO / RAMOS
    oByInsuredPrefixModel = {
        Insured: 0,
        Prefixes: []
    }

    //POR RAMO TECNICO /SUBRAMO TECNICO /RIESGO
    oByLineBusinessSubLineBusinessRiskModel = {
        LineBusiness: null,
        SubLineBusiness: null,
        InsuredObject: []
    };

    //POR RAMO/ obj. segu  RIESGO
    oByPrefixRiskModel = {
        PrefixId: 0,
        InsuredObject: []
    };
    oInsuredObjectModel = {
        Id: 0
    };


    //POR RAMO TECNICO /SUBRAMO TECNICO /COBERTURA
    oByLineBusinessSubLineBusinessCoverageModel = {
        LineBusiness: null,
        SubLineBusinessId: null,
        Coverage: []
    };

    oCoverageModel = {
        Id: 0
    };


    //POR RAMO TECNICO /SUBRAMO TECNICO /POLIZA
    oByPolicyLineBusinessSubLineBusinessModel = {
        LineBusiness: null,
        SubLineBusiness: null,
        Policy: null
    };

    oByPrefixProductModel = {
        Prefix: null,
        Products: []
    };

    oProductModel = {
        Id: 0
    }

    oPrefixModel = {
        Id: 0
    }

}


//************************************************************************************************************************
// P R O G R A M A C I Ó N    D E    B O T O N E S    D E     M O D A L E S

//BOTÓN DE ACEPTAR DEL MODAL modalParams
function acceptDialogModalParams(event, modal) {
    $("#alertPrefix").UifAlert('hide');
    var alertPrefix = Resources.PolicyPrefixRequired;
    totalRecords = 0;

    if ($("#formLinePrefix").valid()) {
        if (!validateDatesFrom) {
            return;
        }
        if (validateDates($('#modalParams').find("#DateFrom").val(),
            $('#modalParams').find("#DateTo").val(), dateSystem.substring(6, 10))) {

            //ARMA EL OBJETO
            oAsociationLine.LineId = $("#LineId").val();
            oAsociationLine.AssociationLineId = $("#AssociationLineId").val();
            oAsociationLine.AssociationColumnId = $("#AssociationColumnId").val();
            oAsociationLine.AssociationTypeId = $("#LineAssociationType").val();
            oAsociationLine.DateFrom = $("#DateFrom").val();
            oAsociationLine.DateTo = $("#DateTo").val();
            oAsociationLine.ByLineBusinessSubLineBusiness = null;

            var productsMultiSelect = null;
            var lineBusinessSubLineSelect = null;
            var coverageSelect = null;
            var insuredObjectSelect = null;
            var i = 0;


            if (oAsociationLine.AssociationTypeId == $("#ViewBagByLineBusiness").val()) { //1 POR RAMO TECNICO

                oByLineBusinessModel.LineBusiness = [];
                lineBusinessSubLineSelect = $("#TableSelectLineBusiness").UifDataTable("getSelected");

                for (i in lineBusinessSubLineSelect) {
                    oLineBusinessModel = { Id: 0 };
                    oLineBusinessModel.Id = lineBusinessSubLineSelect[i].Id;
                    oByLineBusinessModel.LineBusiness.push(oLineBusinessModel);
                    totalRecords = lineBusinessSubLineSelect.length;
                }
                oAsociationLine.ByLineBusiness = oByLineBusinessModel;
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByLineBusinessSubLineBusiness").val()) {  //2 POR RAMO / SUBRAMO TECNICO

                oByLineBusinessModel = { Id: 0 }
                oByLineBusinessModel.Id = $("#SelectLineBusiness").val();
                oByLineBusinessSubLineBusinessModel.LineBusiness = oByLineBusinessModel
                oByLineBusinessSubLineBusinessModel.SubLineBusiness = [];
                lineBusinessSubLineSelect = $("#TableSelectSubLine").UifDataTable("getSelected");

                for (i in lineBusinessSubLineSelect) {
                    oSubLineBusinessModel = { Id: 0 };
                    oSubLineBusinessModel.Id = lineBusinessSubLineSelect[i].Id;
                    oByLineBusinessSubLineBusinessModel.SubLineBusiness.push(oSubLineBusinessModel);
                    totalRecords = lineBusinessSubLineSelect.length;
                }
                var alertPrefix = Resources.SubBranchRequired;
                oAsociationLine.ByLineBusinessSubLineBusiness = oByLineBusinessSubLineBusinessModel;
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByOperationTypePrefix").val()) {//3 POR RAMO / TIPO DE OPERACION
                prefixSelect = null;
                prefixSelect = $("#TablePrefix").UifDataTable("getSelected");
                oByOperationTypePrefixModel.BusinessType = $("#SelectTypeOperation").val();
                oByOperationTypePrefixModel.Prefixes = [];

                for (i in prefixSelect){
                    oPrefixModel = { Id: 0 }
                    oPrefixModel.Id = prefixSelect[i].Id;
                    oByOperationTypePrefixModel.Prefixes.push(oPrefixModel);
                    totalRecords = prefixSelect.length;
                }
                oAsociationLine.ByOperationTypePrefix = oByOperationTypePrefixModel;
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByInsured").val()) { //4 POR ASEGURADO
                oIndividualModel = { IndividualId: 0 }
                oIndividualModel.IndividualId = individualId;
                oByInsuredModel.Insured = oIndividualModel;
                oAsociationLine.ByInsured = oByInsuredModel;

                totalRecords = 1;
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByPrefix").val()) { //5 POR RAMO COMERCIAL
                oByPrefixModel.Prefix = [];
                prefixSelect = null;
                prefixSelect = $("#TablePrefix").UifDataTable("getSelected");

                for (prefix in prefixSelect) {
                    oByPrefixModel.Prefix.push(prefixSelect[prefix]);
                    totalRecords = prefixSelect.length;
                }

                oAsociationLine.ByPrefix = oByPrefixModel;
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByPolicy").val()) { //6 POR POLIZA
                if (policyId > 0) {
                    oPolicyModel = { Id: 0 }
                    oPolicyModel.Id = policyId;
                    oByPolicyModel.Policy = oPolicyModel
                    oAsociationLine.ByPolicy = oByPolicyModel;
                    totalRecords = 1;
                }
                else {
                    totalRecords = 0;
                }
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByFacultativeIssue").val()) { //7 POR FACULTATIVO
                prefixSelect = null;
                prefixSelect = $("#TablePrefix").UifDataTable("getSelected");

                oByFacultativeIssueModel.Prefixes = [];

                for (i in prefixSelect) {
                    oPrefixModel = { Id: 0 }
                    oPrefixModel.Id = prefixSelect[i].Id;
                    oByFacultativeIssueModel.Prefixes.push(oPrefixModel);

                    totalRecords = prefixSelect.length;
                }

                oAsociationLine.ByFacultativeIssue = oByFacultativeIssueModel;
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByInsuredPrefix").val()) {// 8 POR ASEGURADO /RAMO
                prefixSelect = null;
                prefixSelect = $("#TablePrefix").UifDataTable("getSelected");

                oByInsuredModel = { IndividualId: 0 }
                oByInsuredModel.IndividualId = individualId;
                oByInsuredPrefixModel.Insured = oByInsuredModel;
                oByInsuredPrefixModel.Prefixes = [];

                for (i in prefixSelect) {
                    oPrefixModel = { Id: 0 }
                    oPrefixModel.Id = prefixSelect[i].Id;
                    oByInsuredPrefixModel.Prefixes.push(oPrefixModel);

                    totalRecords = prefixSelect.length;
                }

                oAsociationLine.ByInsuredPrefix = oByInsuredPrefixModel;
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByLineBusinessSubLineBusinessRisk").val()) {//9 POR RAMO TÉCNICO / SUBRAMO TÉCNICO / RIESGO
                oLineBusinessModel = { Id: 0 }
                oLineBusinessModel.Id = $("#SelectLineBusiness").val();
                oByLineBusinessSubLineBusinessRiskModel.LineBusiness = oLineBusinessModel;
                oByLineBusinessSubLineBusinessRiskModel.LineBusinessId = $("#SelectLineBusiness").val();
                oSubLineBusinessModel = { Id: 0 }
                oSubLineBusinessModel.Id = $("#SelectSubLineBusiness").val();
                oByLineBusinessSubLineBusinessRiskModel.SubLineBusiness = oSubLineBusinessModel
                oByLineBusinessSubLineBusinessRiskModel.SubLineBusinessId = $("#SelectSubLineBusiness").val();
                oByLineBusinessSubLineBusinessRiskModel.InsuredObject = [];
                insuredObjectSelect = $("#TableInsuredObject").UifDataTable("getSelected");
                if (insuredObjectSelect != null) {
                    totalRecords = insuredObjectSelect.length;
                    for (i in insuredObjectSelect) {
                        oInsuredObjectModel = { Id: 0 };
                        oInsuredObjectModel.Id = insuredObjectSelect[i].Id;
                        oByLineBusinessSubLineBusinessRiskModel.InsuredObject.push(oInsuredObjectModel);
                    }
                    oAsociationLine.ByLineBusinessSubLineBusinessRisk = oByLineBusinessSubLineBusinessRiskModel;
                }
                else {
                    var alertPrefix = Resources.InsuredObjectRequired;
                    totalRecords = 0;
                }
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByPrefixRisk").val()) {//10 POR RAMO / OBJETO DE SEGURO (RIESGO)
                oByPrefixRiskModel.PrefixId = $("#SelectPrefix").val();
                oByPrefixRiskModel.InsuredObject = [];
                insuredObjectSelect = $("#TableInsuredObject").UifDataTable("getSelected");
                if (insuredObjectSelect != null) {
                    totalRecords = insuredObjectSelect.length;
                    for (i in insuredObjectSelect) {
                        oInsuredObjectModel = { Id: 0 };
                        oInsuredObjectModel.Id = insuredObjectSelect[i].Id;
                        oByPrefixRiskModel.InsuredObject.push(oInsuredObjectModel);
                    }
                    oAsociationLine.ByPrefixRisk = oByPrefixRiskModel;
                }
                else {
                    totalRecords = 0;
                    var alertPrefix = Resources.InsuredObjectRequired;
                }
                
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByPolicyLineBusinessSubLineBusiness").val()) {//11 POR POLIZA/ RAMO TÉCNICO / SUBRAMO TÉCNICO / 
                if (policyId > 0) {
                    oLineBusinessModel = { Id: 0 }
                    oLineBusinessModel.Id = $("#SelectLineBusiness").val();
                    oByPolicyLineBusinessSubLineBusinessModel.LineBusiness = oLineBusinessModel;
                    oSubLineBusinessModel = { Id: 0 }
                    oSubLineBusinessModel.Id = $("#SelectSubLineBusiness").val();
                    oByPolicyLineBusinessSubLineBusinessModel.SubLineBusiness = oSubLineBusinessModel;
                    oPolicyModel = { Id: 0 }
                    oPolicyModel.Id = policyId;
                    oByPolicyLineBusinessSubLineBusinessModel.Policy = oPolicyModel;
                    oAsociationLine.ByPolicyLineBusinessSubLineBusiness = oByPolicyLineBusinessSubLineBusinessModel;
                    totalRecords = 1;
                }
                else {
                    totalRecords = 0;
                    Resources.SubBranchRequired;
                }
            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewBagByLineBusinessSubLineBusinessCoverage").val()) {//12 POR RAMO TÉCNICO / SUBRAMO TÉCNICO / COBERTURA
                oLineBusinessModel = { Id: 0 }
                oLineBusinessModel.Id = $("#SelectLineBusiness").val();
                oByLineBusinessSubLineBusinessCoverageModel.LineBusiness = oLineBusinessModel;
                oSubLineBusinessModel = { Id: 0 }
                oSubLineBusinessModel.Id = $("#SelectSubLineBusiness").val();
                oByLineBusinessSubLineBusinessCoverageModel.SubLineBusiness = oSubLineBusinessModel;
                oByLineBusinessSubLineBusinessCoverageModel.Coverage = [];
                coverageSelect = $("#TableCoverages").UifDataTable("getSelected");
                if (coverageSelect != null) {
                    totalRecords = coverageSelect.length;
                    for (i in coverageSelect) {
                        oCoverageModel = { Id: 0 };
                        oCoverageModel.Id = coverageSelect[i].Id;
                        oByLineBusinessSubLineBusinessCoverageModel.Coverage.push(oCoverageModel);
                    }
                    oAsociationLine.ByLineBusinessSubLineBusinessCoverage = oByLineBusinessSubLineBusinessCoverageModel;
                }
                else {
                    var alertPrefix = Resources.ErrorDocumentCoverage;
                    totalRecords = 0;
                }
                

            }
            else if (oAsociationLine.AssociationTypeId == $("#ViewByPrefixProduct").val()) {//13 POR RAMO COMERCIAL / PRODUCTO

                oPrefixModel = { Id: 0 };
                oPrefixModel.Id = $("#SelectPrefix").val();
                oByPrefixProductModel.Prefix = oPrefixModel;
                oByPrefixProductModel.Products = [];
                productsMultiSelect = $('#PrefixByProduct').UifMultiSelect('getSelected');

                for (i in productsMultiSelect) {
                    oProductModel = { Id: 0 };
                    oProductModel.Id = productsMultiSelect[i];
                    oByPrefixProductModel.Products.push(oProductModel);
                    totalRecords = productsMultiSelect.length;
                }

                if (productsMultiSelect.length < 0) {
                    productSelect = 0;
                    totalRecords = 0;
                    policyId = 1;
                }

                oAsociationLine.ByPrefixProduct = oByPrefixProductModel;
                productSelect = productsMultiSelect.length;

            };

            if (totalRecords > 0) { //valida si han seleccionado al menos un registro en las grillas
                $.ajax({
                    async: false,
                    type: "POST",
                    url: REINS_ROOT + "Parameter/ValidateDuplicateLineAssociation",
                    data: JSON.stringify({ "associationLineDto": oAsociationLine }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        if (!data.result) {
                            $.ajax({
                                async: false,
                                type: "POST",
                                url: REINS_ROOT + "Parameter/SaveLineAssociation",
                                data: JSON.stringify({ "associationLineDto": oAsociationLine }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function () {
                                    $("#" + modal).UifModal('hide');
                                    CleanAsociationLine();
                                    MainAssocationLine.RefreshAssociationLine();
                                }
                            });
                        } else {
                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.DuplicateLineAssociation, 'autoclose': true });
                        }
                    }
                    }).fail(function (response) {
                        $.UifNotify('show', { 'type': 'warning', 'message': response.result , 'autoclose': true });
                });
            }
            else {
                if (policyId == 0) {
                    $.UifNotify('show', { 'type': 'warning', 'message': alertPrefix, 'autoclose': true });
                }
                else if (productSelect == 0) {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.ProductRequired, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.WarningSelectOneRecord, 'autoclose': true });
                }
            }
        }
    }
}

//Valida rangos específicos de fechas
function validateDates(dateFrom, dateTo, year) {

    dateToTop = dateAddYear(dateFrom, 2);
    dateToTop = addDaysToDate(dateToTop, -1);

    var yearFrom = dateFrom.substring(6, 10);
    var yearFromTop = year - 1;

    dateFromTop = "01" + "/" + "01" + "/" + yearFromTop;

    if (!compare_dates(dateFrom, dateFromTop)) {

        if (yearFrom != yearFromTop) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.MaxDateFromAssociationLine + " " + dateFromTop, 'autoclose': true });
            return false;
        }
    }

    if (compare_dates(dateTo, dateToTop)) {

        if (dateTo != dateToTop) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.MaxDateToAssociationLine, 'autoclose': true });
            return false;
        }
    }

    return true;
}

function validateDatesFrom() {
    var result = false;

    if ($('#modalParams').find("#DateFrom").val() != "" && $('#modalParams').find("#DateTo").val() != "") {

        if (IsDate($('#modalParams').find("#DateFrom").val()) == false || IsDate($('#modalParams').find("#DateTo").val()) == false) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.InvalidDates, 'autoclose': true });

        }
        else {
            if (compare_dates($('#modalParams').find("#DateFrom").val(), $('#modalParams').find("#DateTo").val())) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateDateFrom, 'autoclose': true });
            }
            else {
                result = true;
            }
        }
    }
    else {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.InvalidDates, 'autoclose': true });
    }
    return result;
}

//Obtiene la fecha del servidor
function getDateServer() {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId") == undefined) {

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "Billing/GetDate",
            success: function (data) {
                dateSystem = data;
            }
        });
    }
}

//Añade N años a la fecha enviada
function dateAddYear(paramDate, addYear) {

    var resultDate = "";
    if (paramDate != "") {
        var dateExercise = paramDate.split("/");
        var nextYear = parseInt(dateExercise[2]) + addYear;
        resultDate = dateExercise[0] + "/" + dateExercise[1] + "/" + nextYear;

    }

    return resultDate;
}
