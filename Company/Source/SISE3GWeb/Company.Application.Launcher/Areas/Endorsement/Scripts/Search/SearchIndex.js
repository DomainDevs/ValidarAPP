var router = uif2.router({ container: "#main" });
var AppResources;
var resourceManager = uif2.resourceManager({ path: rootPath + "Layout/GetResources" });
$(document).ready(function () {
    resourceManager.getResources().then(function (resources) {

        AppResources = resources;
        //Cambio de texto
        var afterCallChangeTexts = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new Texts();
            }
            else {
                ExecuteEndorsement();
            }

        };
        router.add("prtModificationText", rootPath + "Endorsement/Modification/Texts", afterCallChangeTexts);
        //Cambio de Clausula
        var afterCallChangeClause = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new Clauses();
            }
            else {
                ExecuteEndorsement();
            }

        };
        router.add("prtModificationClauses", rootPath + "Endorsement/Modification/Clauses", afterCallChangeClause);
        //Reversion
        var afterCallReversion = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {

                new Reversion();

            }
            else {
                ExecuteEndorsement();
            }

        };
        router.add("prtReversion", rootPath + "Endorsement/Reversion/Reversion", afterCallReversion);

        //Cancelacion
        var afterCancellation = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {

                new Cancellation();

            }
            else {
                ExecuteEndorsement();
            }

        };
        router.add("prtCancellation", rootPath + "Endorsement/Cancellation/Cancellation", afterCancellation);
        //Modificacion
        var afterModification = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new Modification();
            }
            else {
                ExecuteEndorsement();
            }

        };
        router.add("prtModification", rootPath + "Endorsement/Modification/Modification", afterModification);
        //Renovacion temporal
        var afterCallEndorsement = function (params, error) {
            new UnderwritingEndorsement();
        }
        router.add("prtEndorsement", rootPath + "Underwriting/Underwriting/Temporal", afterCallEndorsement);

        //busqueda
        var afterCallSearch = function (params, error) {
            new Search();
        };
        router.add("prtSearch", rootPath + "Endorsement/Search/Search", afterCallSearch);
        //renovacion
        var afterCallRenewal = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new Renewal();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtRenewal", rootPath + "Endorsement/Renewal/Renewal", afterCallRenewal);
        //Extension
        var afterCallExtension = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new Extension();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtExtension", rootPath + "Endorsement/Extension/Extension", afterCallExtension);
        //Nota Credito
        var afterCallCrediNote = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new CreditNote();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtCreditNote", rootPath + "Endorsement/CreditNote/CreditNote", afterCallCrediNote);
        //Declaracion
        var afterCallDeclaration = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new Declaration();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtDeclaration", rootPath + "Endorsement/Declaration/Declaration", afterCallDeclaration);
        //Ajuste
        var afterCallAdjustment = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new Adjustment();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtAdjustment", rootPath + "Endorsement/Adjustment/Adjustment", afterCallAdjustment);
        //Cambio de agente
        var afterCallChangeAgent = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new ChangeAgent();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtChangeAgent", rootPath + "Endorsement/ChangeAgent/ChangeAgent", afterCallChangeAgent);
        //Cambio de Intermediarios
        var afterCallChangePolicyHolder = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new ChangePolicyHolder();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtChangePolicyHolder", rootPath + "Endorsement/ChangePolicyHolder/ChangePolicyHolder", afterCallChangePolicyHolder);

        //Cambio de Coasegurado
        var afterCallChangeCoinsurance = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new ChangeCoinsurance();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtChangeCoinsurance", rootPath + "Endorsement/ChangeCoinsurance/ChangeCoinsurance", afterCallChangeCoinsurance);

        //Cambio de Afianzado
        var afterCallChangeConsolidation = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new ChangeConsolidation();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtChangeConsolidation", rootPath + "Endorsement/ChangeConsolidation/ChangeConsolidation", afterCallChangeConsolidation);
        
        //Cambio de Terminos
        var afterCallChangeTerm = function (params, error) {
            if (glbPolicy != null && glbPolicy.DocumentNumber > 0) {
                new ChangeTerm();
            }
            else {
                ExecuteEndorsement();
            }
        };
        router.add("prtChangeTerm", rootPath + "Endorsement/ChangeTerm/ChangeTerm", afterCallChangeTerm);

        //********************************************************************

        var afterCallTemporal = function (params, error) {
            new UnderwritingTemporal();
            afterCallUnderwriting();
        };
        router.add("prtTemporal", rootPath + "Underwriting/Underwriting/Temporal", afterCallTemporal);

        var afterCallConditionText = function (params, error) {
            new ConditionText();
        };
        router.add("prtConditionText", rootPath + "Parametrization/ConditionText/ConditionText", afterCallConditionText);

        var afterCallSarlaft = function (params, error) {
            new InternationalOperations();
            new LegalRepresentative();
            new Links();
            new PartnersParam();
            new Peps();
            new SarlaftParam();
            new SarlaftRequest();
            
        };

        router.add("prtSarlaft", rootPath + "Sarlaft/Sarlaft/Sarlaft", afterCallSarlaft);
        var afterCallQuotation = function (params, error) {
            new UnderwritingQuotation();
            new QuotationAdvancedSearch();
            afterCallUnderwriting();
        };
        router.add("prtQuotation", rootPath + "Underwriting/Underwriting/Quotation", afterCallQuotation);

        var afterCallUnderwriting = function (params, error) {
            new Underwriting();
            new UnderwritingAdditionalData();
            new CommonAgent();
            new UnderwritingAgent();
            new UnderwritingClauses();
            new UnderwritingText();
            new UnderwritingCoInsurance();
            new UnderwritingPaymentPlan();
            new UnderwritingPromissory();
        }

        var afterCallRiskVehicle = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskVehicle();
                new RiskVehicleAdditionalData();
                new RiskVehicleAccessories();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();
                new OperationQuotaCumulus()

            } else {
                ExecuteEndorsement();
            }
        }
        router.add("prtRiskVehicle", rootPath + "Underwriting/RiskVehicle/Vehicle", afterCallRiskVehicle);

        var afterCallCoverageRiskVehicle = function (params, error) {
            if (glbRisk != null && glbRisk.Id > 0) {
                new RiskVehicleCoverage();
                new TextsCoverage();
                new ClausesCoverage();
            } else {
                ExecuteEndorsement();
            }
        }
        router.add("prtCoverageRiskVehicle", rootPath + "Underwriting/RiskVehicle/Coverage", afterCallCoverageRiskVehicle);

        var afterCallRiskThirdPartyLiability = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskThirdPartyLiability();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();
                new RiskTPLAdditionalData();
                new OperationQuotaCumulus()

            } else {
                ExecuteEndorsement();
            }
        }
        router.add("prtRiskThirdPartyLiability", rootPath + "Underwriting/RiskThirdPartyLiability/ThirdPartyLiability", afterCallRiskThirdPartyLiability);

        var afterCallCoverageRiskThirdPartyLiability = function (params, error) {
            if (glbRisk != null && glbRisk.Id > 0) {
                new RiskThirdPartyLiabilityCoverage();
                new TextsCoverage();
                new ClausesCoverage();
            } else {
                ExecuteEndorsement();
            }
        }
        router.add("prtCoverageRiskThirdPartyLiability", rootPath + "Underwriting/RiskThirdPartyLiability/Coverage", afterCallCoverageRiskThirdPartyLiability);

        var afterCallRiskProperty = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new Property();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();
                new RiskPropertyAdditionalData();
                new OperationQuotaCumulus()

            } else {
                ExecuteEndorsement();
            }
        };
        router.add("prtRiskProperty", rootPath + "Underwriting/RiskProperty/Property", afterCallRiskProperty);

        var afterCallInsuredObjectRiskProperty = function (params, error) {
            if (glbRisk != null && glbRisk.Id > 0) {
                new RiskPropertyInsuredObject();
                new TextsCoverage();
                new ClausesCoverage();
                new DeductiblesCoverage();
            } else {
                ExecuteEndorsement();
            }
        };
        router.add("prtInsuredObjectRiskProperty", rootPath + "Underwriting/RiskProperty/InsuredObject", afterCallInsuredObjectRiskProperty);

        var afterCallRiskSurety = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskSurety();
                new RiskSuretyContract();
                new RiskSuretyCrossGuarantees();
                //new RiskSuretyCrossGuaranteesRequest();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();
                new OperationQuotaCumulus()

            } else {
                ExecuteEndorsement();
            }

        }
        router.add("prtRiskSurety", rootPath + "Underwriting/RiskSurety/Surety", afterCallRiskSurety);

        var afterCallCoverageRiskSurety = function (params, error) {
            if (glbRisk != null && glbRisk.Id > 0) {
                new RiskSuretyCoverage();
                new TextsCoverage();
                new ClausesCoverage();
            } else {
                ExecuteEndorsement();
            }
        }
        router.add("prtCoverageRiskSurety", rootPath + "Underwriting/RiskSurety/Coverage", afterCallCoverageRiskSurety);

        var afterCallRiskLiability = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskLiability();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();
                new OperationQuotaCumulus()
            }
        }
        router.add("prtRiskLiability", rootPath + "Underwriting/RiskLiability/Liability", afterCallRiskLiability);

        var afterCallCoverageRiskLiability = function (params, error) {
            if (glbRisk != null && glbRisk.Id > 0) {
                new RiskLiabilityCoverage();
                new TextsCoverage();
                new ClausesCoverage();
                new DeductiblesCoverage();
            } else {
                ExecuteEndorsement();
            }
        };
        router.add("prtCoverageRiskLiability", rootPath + "Underwriting/RiskLiability/Coverage", afterCallCoverageRiskLiability);

        var afterCallRiskJudicialSurety = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskJudicialSurety();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();
                new ObjectCrossGuaranteesRiskJudicialSurety();
                new ObjectAdditionalDataRiskJudicialSurety();
                new OperationQuotaCumulus()
            } else {
                ExecuteEndorsement();
            }
        };
        router.add("prtRiskJudicialSurety", rootPath + "Underwriting/RiskJudicialSurety/JudicialSurety", afterCallRiskJudicialSurety);

        var afterCallCoverageRiskJudicialSurety = function (params, error) {
            new CoverageJudicialSurety();
            new TextsCoverage();
            new ClausesCoverage();
        };
        router.add("prtCoverageRiskJudicialSurety", rootPath + "Underwriting/RiskJudicialSurety/CoverageJudicialSurety", afterCallCoverageRiskJudicialSurety);

        var afterCallRiskTransport = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskTransport();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();
                new OperationQuotaCumulus()
            } else {
                ExecuteEndorsement();
            }
        };
        router.add("prtRisktransport", rootPath + "Underwriting/RiskTransport/Transport", afterCallRiskTransport);
        var afterCallRiskTransportCoverage = function (params, error) {
            if (glbRisk != null && glbRisk.Id > 0) {
                new RiskTransportCoverage();
                new TextsCoverage();
                new ClausesCoverage();
                new DeductiblesCoverage();
                new OperationQuotaCumulus()
            } else {
                ExecuteEndorsement();
            }
        };
        router.add("prtRisktransportCoverage", rootPath + "Underwriting/RiskTransport/RiskTransportCoverage", afterCallRiskTransportCoverage);
        //**************************************************************************************************************************************************************************************
        var afterCallRiskFidelityCoverage = function (params, error) {
            if (glbPolicy != null && glbRisk.Id > 0) {
                new RiskFidelityCoverage();
                new TextsCoverage();
                new ClausesCoverage();
                new ConceptsCoverage();
            } else {
                ExecuteUnderwriting();
            }
        };
        router.add("prtCoverageRiskFidelity", rootPath + "Underwriting/RiskFidelity/Coverage", afterCallRiskFidelityCoverage);

        var afterCallRiskFidelity = function (params, error) {  
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskFidelity();
                new RiskBeneficiary();
                new RiskClause();
                new RiskText();  
                new OperationQuotaCumulus()
            }
        };
        router.add("prtRiskFidelity", rootPath + "Underwriting/RiskFidelity/Fidelity", afterCallRiskFidelity);


        var afterCallRiskMarine = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskMarine();
                new OperationQuotaCumulus()
            } else {
                ExecuteUnderwriting();
            }
        };
        router.add("prtRiskmarine", rootPath + "Underwriting/RiskMarine/Marine", afterCallRiskMarine);


        var afterCallRiskAircraft = function (params, error) {
            if (glbPolicy != null && glbPolicy.Id > 0) {
                new RiskAircraft();
                new OperationQuotaCumulus()
            } else {
                ExecuteUnderwriting();
            }
        };
        router.add("prtRiskaircraft", rootPath + "Underwriting/RiskAircraft/Aircraft", afterCallRiskAircraft);

        var ExecuteEndorsement = function () {
            router.run("prtSearch");
            //if (GetQueryParameter('type') == 1) {

            //}
        }
        ExecuteEndorsement();

        var afterCallRiskMarinesCoverage = function (params, error) {
            if (glbPolicy != null && glbRisk.Id > 0) {
                new RiskMarineCoverage();
                new TextsCoverage();
                new ClausesCoverage();
                new OperationQuotaCumulus()
                //new ConceptsCoverage();
            } else {
                ExecuteUnderwriting();
            }
        };

        router.add("prtRiskmarineCoverage", rootPath + "Underwriting/RiskMarine/RiskMarineCoverage", afterCallRiskMarinesCoverage);

        var afterCallRiskAircraftCoverage = function (params, error) {
            if (glbRisk != null && glbRisk.Id > 0) {
                new RiskAircraftCoverage();
                new TextsCoverage();
                new ClausesCoverage();
                new OperationQuotaCumulus()
                //new ConceptsCoverage();

            } else {
                ExecuteUnderwriting();
            }
        };
        router.add("prtRiskaircraftCoverage", rootPath + "Underwriting/RiskAircraft/RiskAircraftCoverage", afterCallRiskAircraftCoverage);

      
    });
});