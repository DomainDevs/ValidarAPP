var gblLink = {};
var linkStatus = null;
var individualId = null;

var RolAgent = null;
var RolCoinsured = null;
var RolEmployee = null;
var RolInsured = null;
var RolReinsurer = null;
var RolSupplier = null;
var RolThird = null;
class Links extends Uif2.Page {
    getInitialState() {

        Links.GetRelationship();
        Links.GetLinkTypeBeneficiary();
        Links.GetLinkTypeInsured();
        Links.GetLinkTypeInsuredBeneficiary();
        Links.InitialLinks();
        

    }

    //Seccion Eventos
    bindEvents() {
        $('#selectPolicyholderInsured').on('itemSelected', this.EnableOtherPolicyholderInsured);
        $('#selectPolicyholderBeneficiary').on('itemSelected', this.EnableOtherPolicyholderBeneficiary);
        $('#selectInsuredBeneficiary').on('itemSelected', this.EnableOtherInsuredBeneficiary);
        $('#btnLinkSave').on('click', Links.ExecuteOperationsLink);
    }

    static InitialLinks() {
        Links.GetLinkTypeInsured();
        Links.GetLinkTypeInsuredBeneficiary();
        Links.GetLinkTypeBeneficiary();
        Links.getEnumsRole();
        
    }

    EnableOtherPolicyholderInsured(event, selectedItem) {
        if (selectedItem.Id == 5) {
            $("#inputOtherPolicyholderInsured").prop('disabled', false);
        } else {
            $("#inputOtherPolicyholderInsured").prop('disabled', true);
        }
    }

    EnableOtherPolicyholderBeneficiary(event, selectedItem) {
        if (selectedItem.Id == 5) {
            $("#inputOtherPolicyholderBeneficiary").prop('disabled', false);
        } else {
            $("#inputOtherPolicyholderBeneficiary").prop('disabled', true);
        }
    }

    EnableOtherInsuredBeneficiary(event, selectedItem) {
        if (selectedItem.Id === 5) {
            $("#inputOtherInsuredBeneficiary").prop('disabled', false);
        } else {
            $("#inputOtherInsuredBeneficiary").prop('disabled', true);
        }
    }

    static GetIndividualLinks() {
        $("#selectPolicyholderInsured, #selectPolicyholderBeneficiary, #selectInsuredBeneficiary").UifSelect("setSelected", null);
        $("#selectPolicyholderInsured, #selectPolicyholderBeneficiary, #selectInsuredBeneficiary").UifSelect("disabled", false);
        if (gblSarlaft.LinksDTO != null && gblSarlaft.LinksDTO.length > 0) {
            Links.EditLiks(gblSarlaft.LinksDTO);
            if (individualId == null)
                if (gblSarlaft.SarlaftDTO.IndividualId != "undefined" || gblSarlaft.SarlaftDTO.IndividualId != null || gblSarlaft.SarlaftDTO.IndividualId != "" || gblSarlaft.SarlaftDTO.IndividualId > 0)
                    Links.GetRolByIndividualId(gblSarlaft.SarlaftDTO.IndividualId);
                else
                    Links.GetRolByIndividualId(gblPerson.IndividualId);
            else
                Links.GetRolByIndividualId(individualId);
        } else {
            individualId = gblIndivualId;
            if (individualId !== null) {
                var linkSarlaftId = 0;
                if (newSarlaftId != null) {
                    linkSarlaftId = newSarlaftId
                }
                else if (gblSarlaft.SarlaftDTO != undefined && gblSarlaft.SarlaftDTO != null) {
                    linkSarlaftId = gblSarlaft.SarlaftDTO.Id;
                }
                else {
                    linkSarlaftId = $('#SarlaftId').val();
                }
                SarlaftRequest.GetIndividualLinksByIndividualId(individualId, linkSarlaftId).done(function (data) {
                    if (data.success) {
                        if (!sarlaftIsNew) {
                            $("#selectPolicyholderInsured, #selectPolicyholderBeneficiary, #selectInsuredBeneficiary").UifSelect("disabled", true);
                        } else {
                            gblSarlaft.LinksDTO = data.result;
                        }
                        if (data.result.length > 0) {
                            gblLink = data.result;
                            if (sarlaftIsNew)
                                linkStatus = 2;
                            else
                                linkStatus = 3;
                            //$("#selectPolicyholderInsured, #selectPolicyholderBeneficiary, #selectInsuredBeneficiary").UifSelect("disabled", true);
                            Links.EditLiks(gblLink);
                        } else {

                            linkStatus = 2;
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
                Links.GetRolByIndividualId(individualId);

                if (parseInt(typePerson) == IndividualTypePerson.Legal) 
                        $("#btnEmployee").hide();
                else
                    $("#btnEmployee").show();
            }
        }
        
    }

    static EditLiks(data) {
        for (var i in data) {
            switch (parseInt((data[i].RelationShipCode+""))) {
                case 1:
                    $("#select1").UifSelect("setSelected", data[i].RelationShipCode);
                    $("#selectPolicyholderInsured").UifSelect("setSelected", data[i].LinkTypeCode);
                    //$("#selectPolicyholderInsured").UifSelect("disabled", true);
                    if (data[i].LinkTypeCode == 5) {
                        $("#inputOtherPolicyholderInsured").prop('disabled', false);
                    }
                    $("#inputOtherPolicyholderInsured").val(data[i].Description);
                    $("#StatusLink").val(data[i].Status);
                    individualId = data[i].Id;
                    break;
                case 2:
                    $("#select2").UifSelect("setSelected", data[i].RelationShipCode);
                    $("#selectPolicyholderBeneficiary").UifSelect("setSelected", data[i].LinkTypeCode);
                    //$("#selectPolicyholderBeneficiary").UifSelect("disabled", true);
                    if (data[i].LinkTypeCode == 5) {
                        $("#inputOtherPolicyholderBeneficiary").prop('disabled', false);
                    }
                    $("#inputOtherPolicyholderBeneficiary").val(data[i].Description);
                    $("#StatusLink").val(data[i].Status);
                    individualId = data[i].Id;
                    break;
                case 3:
                    $("#select3").UifSelect("setSelected", data[i].RelationShipCode);
                    $("#selectInsuredBeneficiary").UifSelect("setSelected", data[i].LinkTypeCode);
                    //$("#selectInsuredBeneficiary").UifSelect("disabled", true);
                    if (data[i].LinkTypeCode == 5) {
                        $("#inputOtherInsuredBeneficiary").prop('disabled', false);
                    }
                    $("#inputOtherInsuredBeneficiary").val(data[i].Description);
                    $("#StatusLink").val(data[i].Status);
                    individualId = data[i].Id   ;
                    break;
                default:
                    break;
            }
        }
    }

    static GetRelationship() {
        SarlaftRequest.GetRelationship().done(function (data) {
            if (data.success) {
                for (var i in data.result) {
                    switch (data.result[i].Id) {
                        case 1:
                            $("#select1").UifSelect({
                                sourceData: data.result,
                                selectedId: data.result[i].Id,
                                enable: false
                            });
                            break;
                        case 2:
                            $("#select2").UifSelect({
                                sourceData: data.result,
                                selectedId: data.result[1].Id,
                                enable: false
                            });
                            break;
                        case 3:
                            $("#select3").UifSelect({
                                sourceData: data.result,
                                selectedId: data.result[2].Id,
                                enable: false
                            });
                            break;
                    }
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetLinkTypeInsured() {
        SarlaftRequest.GetLinkTypeResult().done(function (data) {
            if (data.success) {
                $('#selectPolicyholderInsured').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetLinkTypeBeneficiary() {
        SarlaftRequest.GetLinkTypeResult().done(function (data) {
            if (data.success) {
                $('#selectPolicyholderBeneficiary').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static GetLinkTypeInsuredBeneficiary() {
        SarlaftRequest.GetLinkTypeResult().done(function (data) {
            if (data.success) {
                $('#selectInsuredBeneficiary').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }

    static ExecuteOperationsLink() {
        var NewLinks = [];
        var PolicyHolderInsured = {};
        var PolicyholderBeneficiary = {};
        var InsuredBeneficiary = {};

        if (Links.Validate()) {
            if ($("#StatusLink").val() != "" && $("#StatusLink").val() == ParametrizationStatus.Original) {
                if (newSarlaftId == null) {
                    linkStatus = ParametrizationStatus.Update;
                }
                else {
                    linkStatus = ParametrizationStatus.Create;
                }
            }
            if (linkStatus != null) {
                PolicyHolderInsured = {
                    Id: individualId,
                    RelationShipCode: parseInt($("#select1").UifSelect("getSelected")),
                    LinkTypeCode: parseInt($("#selectPolicyholderInsured").UifSelect("getSelected")),
                    Description: $("#inputOtherPolicyholderInsured").val(),
                    Status: linkStatus
                }
                NewLinks.push($.extend({}, PolicyHolderInsured));
                PolicyholderBeneficiary = {
                    Id: individualId,
                    RelationShipCode: parseInt($("#select2").UifSelect("getSelected")),
                    LinkTypeCode: parseInt($("#selectPolicyholderBeneficiary").UifSelect("getSelected")),
                    Description: $("#inputOtherPolicyholderBeneficiary").val(),
                    Status: linkStatus
                }
                NewLinks.push($.extend({}, PolicyholderBeneficiary));
                InsuredBeneficiary = {
                    Id: individualId,
                    RelationShipCode: parseInt($("#select3").UifSelect("getSelected")),
                    LinkTypeCode: parseInt($("#selectInsuredBeneficiary").UifSelect("getSelected")),
                    Description: $("#inputOtherInsuredBeneficiary").val(),
                    Status: linkStatus
                }
                NewLinks.push($.extend({}, InsuredBeneficiary));

                gblSarlaft.LinksDTO = NewLinks;
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.LinksSuccessfully, 'autoclose': true });
                $('#modalLinks').UifModal('hide');
            }
        }
    }

    static Validate() {

        var msj = "";

        if ($("#selectPolicyholderInsured").UifSelect("getSelected") == null || $("#selectPolicyholderInsured").UifSelect("getSelected") == "") {
            msj = AppResources.Link + "<br>"
        }

        if ($("#selectPolicyholderBeneficiary").UifSelect("getSelected") == null || $("#selectPolicyholderBeneficiary").UifSelect("getSelected") == "") {
            msj = AppResources.Link + "<br>"
        }

        if ($("#selectInsuredBeneficiary").UifSelect("getSelected") == null || $("#selectInsuredBeneficiary").UifSelect("getSelected") == "") {
            msj = AppResources.Link + "<br>"
        }

        if (msj != "") {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCheckRequiredFields + "<br>" + "<strong>" + msj + "</strong>", 'autoclose': true })
            return false;
        }

        return true;
    }

    static DisabledLinks() {
        $("#inputOtherPolicyholderInsured").prop("disabled", true);
        $('#inputOtherPolicyholderBeneficiary').prop("disabled", true);
        $('#inputOtherInsuredBeneficiary').prop("disabled", true);
        $("#selectPolicyholderInsured").UifSelect("disabled", true);
        $("#selectPolicyholderBeneficiary").UifSelect("disabled", true);
        $("#selectInsuredBeneficiary").UifSelect("disabled", true);
        $("#btnLinkSave").hide();
    }

    static EnabledLinks() {
        $("#inputOtherPolicyholderInsured").prop("disabled", false);
        $('#inputOtherPolicyholderBeneficiary').prop("disabled", false);
        $('#inputOtherInsuredBeneficiary').prop("disabled", false);
        $("#selectPolicyholderInsured").UifSelect("disabled", false);
        $("#selectPolicyholderBeneficiary").UifSelect("disabled", false);
        $("#selectInsuredBeneficiary").UifSelect("disabled", false);
        $("#btnLinkSave").show();
    }

    static ClearFields() {
        $("#selectPolicyholderInsured").UifSelect("setSelected", null);
        $("#selectPolicyholderBeneficiary").UifSelect("setSelected", null);
        $("#selectInsuredBeneficiary").UifSelect("setSelected", null);
        $("#inputOtherPolicyholderInsured").val("");
        $("#inputOtherPolicyholderBeneficiary").val("");
        $("#inputOtherInsuredBeneficiary").val("");
        $("#StatusLink").val("");
        linkStatus = null;
    }

    static ResultSeachRolPerson(idRol) {
        switch (idRol) {
            case RolInsured:
                $('#checkInsured').prop('checked', true);
                break;
            case RolAgent:
                $('#checkAgent').prop('checked', true);
                break;
            case RolEmployee:
                $('#checkEmployee').prop('checked', true);
                break;
            case RolSupplier:
                $('#checkProvider').prop('checked', true);
                break;
            case RolThird:
                $('#checkThird').prop('checked', true);
                break;
            case RolReinsurer:
                $('#checkReinsurer').prop('checked', true);
                break;
            case RolCoinsured:
                $('#checkCoInsurer').prop('checked', true);
                break;
        }
    }

    static ResetRolPerson() {
        $('#checkInsured').prop('checked', false);
        $('#checkAgent').prop('checked', false);
        $('#checkEmployee').prop('checked', false);
        $('#checkProvider').prop('checked', false);
        $('#checkThird').prop('checked', false);
        $('#checkReinsurer').prop('checked', false);
        $('#checkCoInsurer').prop('checked', false);
    }


    static GetRolByIndividualId(individualId) {
        SarlaftRequest.GetTypeRolByIndividual(individualId).done(function (data) {
            if (data.success) {
                Links.ResetRolPerson();
                $.each(data.result, function (key, value) {
                    Links.ResultSeachRolPerson(value.RoleId);
                });
            }
        });
    }

    static getEnumsRole() {
        PersonRequest.GetEnumsRoles().done(function (data) {
            if (data.success) {
                RolAgent = data.result.Agent;
                RolCoinsured = data.result.Coinsured;
                RolEmployee = data.result.Employee;
                RolInsured = data.result.Insured;
                RolReinsurer = data.result.Reinsurer;
                RolSupplier = data.result.Supplier;
                RolThird = data.result.Third
            }
        });
    }


   

}
