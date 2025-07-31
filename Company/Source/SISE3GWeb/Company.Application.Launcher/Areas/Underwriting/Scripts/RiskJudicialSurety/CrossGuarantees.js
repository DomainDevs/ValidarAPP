//Codigo de la pagina AdditionalData.cshtml
//$.ajaxSetup({ async: false });

class ObjectCrossGuaranteesRiskJudicialSurety extends Uif2.Page {
    getInitialState() {
        var val = 0;

    }

    bindEvents() {
        $("#btnCounterGuarantees").on("click", function (event) {
            event.preventDefault();
            ObjectCrossGuaranteesRiskJudicialSurety.LoadCrossGuarantee();
        });

        $("#btnCrossGuaranteesClose").on("click", function () {

            RiskJudicialSurety.HidePanelsRisk(MenuType.CrossGuarantees);
            RiskJudicialSurety.LoadSubTitles(5);
            if (loadRisks) {
                RiskSurety.GetRiskSuretyById(glbPolicy.Id, glbRisk.Id);
                loadRisks = false;
            }
        });

        $("#btnCrossGuaranteesSave").on("click", this.SaveGuarantees);
        $("#btnCrossGuaranteesNew").on("click", this.Guarantee);
        $('#tableGuarantees tbody').on('click', 'tr', this.SelectSearchGuarantees);
    }

    SelectSearchGuarantees() {
        if (!($(this).find('td button span').hasClass("glyphicon glyphicon-check"))) {
            var rowIndex = ($(this)[0].rowIndex - 1);
            RiskJudicialRequest.GetInsuredGuaranteeRelationPolicy($(this).children()[1].innerHTML).done(function (data) {
                if (data.success) {
                    if (data.result) {
                        $("#tableGuarantees tbody tr:eq(" + rowIndex + " )").removeClass("row-selected");
                        $("#tableGuarantees tbody tr:eq(" + rowIndex + " ) td button span").removeClass("glyphicon glyphicon-check").addClass("glyphicon glyphicon-unchecked");
                        $.UifNotify('show', { 'type': 'info', 'message': AppResources.GuaranteeRelation, 'autoclose': true });
                    }
                }
            });
        }
    }

    Guarantee() {
        //$("#modalCrossGuarantees").UifModal('hide');
        //var individual = $("#inputInsuredJudSur").data("Object").IndividualId;
        //var GuaranteeViewModel = {
        //    ContractorId: individual,
        //    searchType: TypePerson.PersonLegal,
        //    returnController: riskController
        //}
     
        //if (parseInt(individual) >= 0) {
        //    guaranteeModel = GuaranteeViewModel;
        //}
        //router.run("prtGuarantee");



        //-----------------------
        var individual = $("#inputInsuredJudSur").data("Object").IndividualId;
        var GuaranteeViewModel = {
            ContractorId: individual,
            searchType: TypePerson.PersonNatural,
            returnController: riskController,
            isPolicyRisk: true
        }

        if (parseInt(individual) >= 0) {
            guaranteeModel = GuaranteeViewModel;
            /*REQ_633
            * Antes de ir a las contra-garantías hay que guardar el riesgo. 
            */
            var riskData = RiskJudicialSurety.GetRiskDataModel();
            RiskSuretyCrossGuarantees.sleep(850).then(() => {
                RiskJudicialRequest.SaveRisk(glbPolicy.Id, riskData, dynamicProperties, true).done(function (data) {
                    if (data.success) {
                        //Actualización glbRisk
                        RiskJudicialSurety.UpdateGlbRisk(data.result);

                    } else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                        return false;
                    }
                });
            });

        }
        $("#modalCrossGuarantees").UifModal('hide');
        router.run("prtGuaranteeE");
    }
    GetCrossGuarantees() {
    }

    static HideCrossGuarantees() {
    }

    static ClearCrossGuarantees() {
    }

    OpenCrossGuarantees() {

    }

    static LoadCrossGuarantee() {

        $("#formJudicialSurety").validate();
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification) {
            $('#btnCrossGuaranteesSave').prop('disabled', true);
        }
        if (glbRisk.Id == 0) {
            if (glbRisk.Class == undefined) {
                window[glbRisk.Object].SaveRisk(MenuType.CrossGuarantees, 0, false, false);
            }
            else {
                glbRisk.Class.SaveRisk(MenuType.CrossGuarantees, 0, false, false);
            }
        }
        if (glbRisk.Class == undefined) {
            window[glbRisk.Object].ShowPanelsRisk(MenuType.CrossGuarantees);
        }
        else {
            glbRisk.Class.ShowPanelsRisk(MenuType.CrossGuarantees);
        }

        ObjectCrossGuaranteesRiskJudicialSurety.GetInsuredGuaranteeByIndividualId();

    }

    SaveGuarantees() {

        var guarantees = $("#tableGuarantees").UifDataTable('getSelected');
        if (guarantees != null) {
            $.each(guarantees, function (key, value) {
                this.Amount = NotFormatMoney(this.Amount);
                var GuaranteeStatus = { Id: this.IdCode, Description: this.Status, IsEnabledInd: this.IsEnabledInd, IsEnabledSubscription: this.ISEnabledSubscription };
                this.InsuredGuarantee = { Id: this.Id, AppraisalAmount: this.Amount, IsCloseInd: this.IsCloseInd, Status: GuaranteeStatus };
            });

            $.ajax({
                type: "POST",
                url: rootPath + 'Underwriting/RiskJudicialSurety/SaveGuarantees',
                data: JSON.stringify({ riskId: glbRisk.Id, guarantees: guarantees }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    RiskJudicialSurety.HidePanelsRisk(MenuType.CrossGuarantees);
                    glbRisk.Guarantees = data.result;
                    RiskJudicialSurety.GetRisksByTemporalId(glbPolicy.Id, glbRisk.Id);
                    RiskJudicialSurety.LoadSubTitles(5);
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveGuarantees, 'autoclose': true });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSaveGuarantees, 'autoclose': true });
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSelectGuarantee, 'autoclose': true });
        }
    }

    static GetInsuredGuaranteeByIndividualId() {
        $('#tableGuarantees').UifDataTable('clear');
        var individualId = $("#inputInsuredJudSur").data("Object").IndividualId;
        ObjectCrossGuaranteesRiskJudicialSurety.GetInsuredGuaranteesByIndividualId(individualId).done(function (data) {
            if (data.result.length > 0) {
                
                ObjectCrossGuaranteesRiskJudicialSurety.LoadGuarantees(data.result);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchGuarantees, 'autoclose': true });
        });

        
        //$.ajax({
        //    type: "POST",
        //    url: rootPath + 'Underwriting/RiskJudicialSurety/GetInsuredGuaranteeByIndividualId',
        //    data: JSON.stringify({ individualId: individualId }),
        //    dataType: "json",
        //    contentType: "application/json; charset=utf-8"
        //}).done(function (data) {
        //    if (data.success) {
        //        ObjectCrossGuaranteesRiskJudicialSurety.LoadGuarantees(data.result);
        //    }
        //}).fail(function (jqXHR, textStatus, errorThrown) {
        //    $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchGuarantees, 'autoclose': true });
        //});
    }
    static GetOpen(openEnabled) {
        switch (openEnabled) {
            case true:
                return "NO"
                break;
            default:
                return "SI"
        }
    }
    static LoadGuarantees(guarantees) {
        $('#tableGuarantees').UifDataTable('clear');
        if (guarantees != null) {
            var guaranteesList = [];
            $.each(guarantees, function (key, value) {
                guaranteesList.push({
                    Description: this.Description + "/" + this.GuaranteeType.Description + "/",
                    Id: this.InsuredGuarantee.Id,
                    Amount: this.InsuredGuarantee.DocumentValueAmount,
                    IsOpen: ObjectCrossGuaranteesRiskJudicialSurety.GetOpen(this.InsuredGuarantee.IsCloseInd),
                    Status: this.InsuredGuarantee.GuaranteeStatus.Description,
                    IdCode: this.InsuredGuarantee.GuaranteeStatus.Code,
                    IsEnabledInd: this.InsuredGuarantee.GuaranteeStatus.IsEnabledInd,
                    IsEnabledSubscription: this.InsuredGuarantee.GuaranteeStatus.IsEnabledSubscription,
                    IsCloseInd: this.InsuredGuarantee.IsCloseInd
                });
                if (this.GuaranteeType.Code == GuaranteeType.Mortage || this.GuaranteeType.Code == GuaranteeType.Pledge) {
                    guaranteesList[guaranteesList.length - 1].Amount = FormatMoney(this.InsuredGuarantee.AppraisalAmount);
                }
                if (this.GuaranteeType.Code == GuaranteeType.PromissoryNote || this.GuaranteeType.Code == GuaranteeType.Fixedtermdeposit) {
                    guaranteesList[guaranteesList.length - 1].Amount = FormatMoney(this.InsuredGuarantee.DocumentValueAmount);
                }

            });
            if (guaranteesList.length > 0) {
                $.each(guaranteesList, function (key, value) {
                    if (value.Amount == null) {
                        value.Amount = 0;
                    }
                });
                $('#tableGuarantees').UifDataTable('addRow', guaranteesList);
            }
        }
        if (glbRisk.Guarantees != null) {
            $.each($('#tableGuarantees').UifDataTable('getData'), function (id, item) {
                var guaranteeId = this.Id;
                $.each(glbRisk.Guarantees, function (idG, itemG) {
                    if (guaranteeId == this.Id) {
                        $("#tableGuarantees tbody tr:eq(" + id + " )").removeClass("row-selected").addClass("row-selected");
                        $("#tableGuarantees tbody tr:eq(" + id + " ) td button span").removeClass("glyphicon glyphicon-unchecked").addClass("glyphicon glyphicon-check");
                    }
                });
            });
        }
    }


    static GetInsuredGuaranteesByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetInsuredGuaranteeByIndividualId',
            data: JSON.stringify({ individualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}










