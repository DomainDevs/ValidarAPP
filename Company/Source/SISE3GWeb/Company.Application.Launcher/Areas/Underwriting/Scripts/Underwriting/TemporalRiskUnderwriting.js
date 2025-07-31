


//class TemporalRiskUnderwriting extends Uif2.Page {

//    getInitialState() {

//    }

//    bindEvents() {
//        //$("#btnRiskFromPolicy").on("click", TemporalRiskUnderwriting.ShowModalRiskFromPolicy);
//    }

//    static ShowModalRiskFromPolicy() {
//        var resultOperation = false;
//        $("#formUnderwriting").validate();
//        if ($("#formUnderwriting").valid() && UnderwritingTemporal.ValidateSarlaft(glbPolicy.Holder.IndividualId) && UnderwritingTemporal.ValidateSarlaftIntermediary(glbPolicy.Agencies[0].Agent.IndividualId)) {
//            //guardar temporal
//            var policyData = Underwriting.GetPolicyModel();
//            UnderwritingRequest.SaveTemporal(policyData, dynamicProperties).done(function (data) {
//                if (data.success) {
//                    resultOperation = true;
//                    Underwriting.UpdateGlbPolicy(data.result);
//                    Underwriting.LoadTitle(glbPolicy);
//                    Underwriting.LoadSubTitles(0);

//                    if (glbPolicy.TemporalType != TemporalType.Quotation) {
//                        if (glbPolicy.Holder.InsuredId == 0) {
//                            if (gblProspectData.IdCardNo == null) {
//                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyholderWithoutRol, 'autoclose': true });
//                                resultOperation = false;
//                            }
//                        }
//                        else if (FormatDate(glbPolicy.Holder.DeclinedDate) != '01/01/0001') {
//                            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicyholderDisabled, 'autoclose': true });
//                            resultOperation = false;
//                        }
//                    }
//                    if (resultOperation !== false) {
//                        let events = TypeAuthorizationPolicies.Nothing;
//                        if (glbPolicy.InfringementPolicies != null) {
//                            /// se lanzan los eventos de la pantalla principal
//                            events = LaunchPolicies.ValidateInfringementPolicies(glbPolicy.InfringementPolicies, true);
//                        }

//                        UnderwritingRequest.GetBranches().done(function (data) {
//                            if (data.success) {
//                                $("#selectBranchRisk").UifSelect({ sourceData: data.result });
//                            }
//                        });
//                        TemporalRequest.GetPrefixes().done(dataPrefix => {
//                            if (dataPrefix.success) {
//                                $('#selectPrefixRisk').UifSelect({ sourceData: dataPrefix.result });
//                            }
//                        });
//                        $("#inputPolicyNumber").val('');
//                        $('#modalRiskFromPolicy').UifModal('showLocal', "Recuperar Riesgo desde otra póliza");
//                    }
//                }
//                else {
//                    resultOperation = false;
//                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
//                }
//            }).fail(function (jqXHR, textStatus, errorThrown) {
//                resultOperation = false;
//                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveTemporary, 'autoclose': true });
//            });
//        }
//    }


//    static SearchRiskFromPolicy() {
//        var type = true;
//        searchPolicyModel = $('#formRiskFromPolicy').serializeObject();
//        searchPolicyModel.BranchId = $("#selectBranchRisk").UifSelect("getSelected");
//        searchPolicyModel.PrefixId = $("#selectPrefixRisk").UifSelect("getSelected");

//        var endorsement = null;

//        //Traer los endosos asociados a la poliza
//        UnderwritingRequest.GetEndorsementsByPrefixIdBranchIdPolicyNumber(searchPolicyModel.BranchId, searchPolicyModel.PrefixId, searchPolicyModel.Id, type).done(function (data) {
//            if (data.success) {
//                endorsement = data.result.endorsements[0];
//                if (endorsement.EndorsementType == 3) {
//                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NoMovesCanceledPolicy, 'autoclose': true });
//                }
//                else {
//                    //Traer informacion de la poliza asociada al ultimo endoso realizado
//                    UnderwritingRequest.GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsement.Id, true).done(function (dataSt) {

//                        if (dataSt.result != null) {

//                            var date = new Date();
//                            //Validar si el producto del temporal y el de la poliza a recuperar es distinto
//                            if (dataSt.result.Product.Id != glbPolicy.Product.Id) {
//                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorDifferentProduct, 'autoclose': true });
//                            }
//                            else if (FormatDate(dataSt.result.Endorsement.CurrentTo) > FormatDate(date)) {
//                                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorCurrentPolicy, 'autoclose': true });
//                            }
//                            else {
//                                //Cargar riesgos y guardarlos a la temporal que esta en glbPolicy
//                                UnderwritingRequest.GetRiskByPolicyId(endorsement.PolicyId, glbPolicy.Id).done(function (dataRisk) {

//                                    if (dataRisk.success) {
//                                        var totalRisks = parseInt(glbPolicy.Summary.RiskCount, 10) + parseInt(dataRisk.result.length, 10);
//                                        $("#selectedInclusionRisk").text("(" + totalRisks + ")");
//                                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.MessageSavedRiskSuccessfully, 'autoclose': true });
//                                        Underwriting.UpdateRisks();
//                                        $("#modalRiskFromPolicy").UifModal("hide");
//                                    }
//                                    else {
//                                        $.UifNotify('show', { 'type': 'danger', 'message': dataRisk.result, 'autoclose': true });
//                                    }
//                                });
//                            }
//                        }
//                        else {
//                            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
//                        }
//                    });
//                }
//            }
//            else {
//                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
//            }
//        }).fail(function (jqXHR, textStatus, errorThrown) {
//            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorQueryAssociatedEndorsement, 'autoclose': true });
//        });
//    }

//}
