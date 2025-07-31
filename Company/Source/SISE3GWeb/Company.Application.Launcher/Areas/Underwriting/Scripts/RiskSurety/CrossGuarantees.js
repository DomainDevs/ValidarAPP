var guaranteeModel = null;

class RiskSuretyCrossGuarantees extends Uif2.Page {
    getInitialState() { }

    bindEvents() {
        //Contragarantías
        $("#btnCounterGuarantees").on("click", function (event) {
            event.preventDefault();
            RiskSuretyCrossGuarantees.LoadCrossGuarantee();
        });


        $("#btnCrossGuaranteesClose").on("click", function () {

            RiskSurety.HidePanelsRisk(MenuType.CrossGuarantees);
            RiskSurety.LoadSubTitles(5);
            if (loadRisks) {
                RiskSurety.GetRiskSuretyById(glbPolicy.Id, glbRisk.Id);
                loadRisks = false;
            }
        });

        $("#btnCrossGuaranteesSave").on("click", function () {

            RiskSuretyCrossGuarantees.SaveGuarantees();
        });

        $("#btnCrossGuaranteesNew").on("click", RiskSuretyCrossGuarantees.Guarantee);
        $('#tableGuarantees tbody').on('click', 'tr', this.SelectSearchGuarantees);

    }

    SelectSearchGuarantees() {
        if (!($(this).find('td button span').hasClass("glyphicon glyphicon-check"))) {
            var rowIndex = ($(this)[0].rowIndex - 1);
            RiskSuretyCrossGuaranteesRequest.GetInsuredGuaranteeRelationPolicy($(this).children()[1].innerHTML).done(function (data) {
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

    static Guarantee() {
        var individual = $("#inputSecure").data("Object").IndividualId;
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
            var riskData = RiskSurety.GetRiskDataModel();
            RiskSuretyCrossGuarantees.sleep(850).then(() => {
                RiskSuretyRequest.SaveRisk(riskData, riskData.Coverages, dynamicProperties, true).done(function (data) {
                    if (data.success) {
                        //Actualización glbRisk
                        RiskSurety.UpdateGlbRisk(data.result);

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

    // sleep time expects milliseconds
    static sleep(time) {
        return new Promise((resolve) => setTimeout(resolve, time));
    }

    static CreateInsuredGuarantee(individual) {

        var InsuredConcept = { InsuredCode: 0, isInsured: true, IsBeneficiary: false, IsHolder: false, IsPayer: false };
        var insured = {
            NameAgent: "",
            Id: individual,
            Agent: null,
            EnteredDate: DateNowPerson,
            DeclinedDate: "",
            InsDeclinesType: null,
            Annotations: null,
            Profile: RolType.Insured,
            ModifyDate: null,
            InsuredId: 0,
            IndividualId: individual,
            InsuredConcept: InsuredConcept
        };
        RiskSuretyCrossGuaranteesRequest.CreateInsuredGuarantee(insured).done(function (data) {
            if (data.success) {
                $('#CodInsured').val(data.result.InsuredId);
                $('#CodInsured').text(data.result.InsuredId);
            }
            else {
                $.UifDialog('alert', { 'message': AppResources.ErrorCreateInsured });
            }
        });
    }

    static GetInsuredGuaranteeByIndividualId() {
        $('#tableGuarantees').UifDataTable('clear');

        var individualId = $("#inputSecure").data("Object").IndividualId;
        RiskSuretyCrossGuaranteesRequest.GetInsuredGuaranteeByIndividualId(individualId).done(function (data) {
            if (data.result.length > 0) {
                RiskSuretyCrossGuarantees.LoadGuarantees(data.result);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorSearchGuarantees, 'autoclose': true });
        });
    }

    static LoadGuarantees(guarantees) {
        $('#tableGuarantees').UifDataTable('clear');
        if (guarantees != null) {
            var guaranteesList = [];
            $.each(guarantees, function (key, value) {
                guaranteesList.push({
                    Description: this.Description + "/" + this.GuaranteeType.Description + "/",
                    Id: this.InsuredGuarantee.Id,
                    Amount: value.InsuredGuarantee.DocumentValueAmount,
                    IsOpen: RiskSuretyCrossGuarantees.GetOpen(this.InsuredGuarantee.IsCloseInd),
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
                    if (guaranteeId == this.InsuredGuarantee.Id) {
                        $("#tableGuarantees tbody tr:eq(" + id + " )").removeClass("row-selected").addClass("row-selected");
                        $("#tableGuarantees tbody tr:eq(" + id + " ) td button span").removeClass("glyphicon glyphicon-unchecked").addClass("glyphicon glyphicon-check");
                    }
                });
            });
        }
    }

    static GetOpen(openEnabled) {
        switch (openEnabled) {
            case true:
                return "NO"
                break;
            default:
                return "SI"
                break;
        }
    }

    static SaveGuarantees() {

        var guarantees = $("#tableGuarantees").UifDataTable('getSelected');
        if (guarantees != null) {
            $.each(guarantees, function (key, value) {
                this.Amount = NotFormatMoney(this.Amount);
                var GuaranteeStatus = { Id: this.IdCode, Description: this.Status, IsEnabledInd: this.IsEnabledInd, IsEnabledSubscription: this.ISEnabledSubscription };
                this.InsuredGuarantee = { Id: this.Id, AppraisalAmount: this.Amount, IsCloseInd: this.IsCloseInd, Status: GuaranteeStatus };
            });

     

            RiskSuretyCrossGuaranteesRequest.SaveGuarantees(glbRisk.Id, guarantees).done(function (data) {
                if (data.success) {
                    RiskSurety.HidePanelsRisk(MenuType.CrossGuarantees);
                    glbRisk.Guarantees = data.result;
                    if (loadRisks) {
                        RiskSurety.GetRiskSuretyById(glbPolicy.Id, glbRisk.Id);
                        loadRisks = false;
                    }
                    RiskSurety.LoadSubTitles(5);
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

    static LoadCrossGuarantee() {
        var EnableCrossGuarantee = RiskSurety.GetEnableCrossGuarantee();
        $("#mainRiskSurety").validate();
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && EnableCrossGuarantee) 
        {
            $('#btnCrossGuaranteesSave').prop('disabled', true);
        }
        if (glbRisk.Id == 0) {
            RiskSurety.SaveRisk(MenuType.CrossGuarantees, 0, false, false);
            loadRisks = true;
        }
        if (glbRisk.Id > 0) {
            RiskSurety.ShowPanelsRisk(MenuType.CrossGuarantees);
            RiskSuretyCrossGuarantees.GetInsuredGuaranteeByIndividualId();
        }
    }

}