var OperationQuotaEvent;
var ConsortiumEvent;
var EconomicGroupEvent;
var OperationQuotaDecline;
var ConsortiumDecline;
var EconomicGroupDecline;
var ValidityParticipant1;
var ConsortiumEvents = {}
var CurrentDateQuota = new Date();
var TotalSumInsuredRisk;
var OperationQuotaConsortiumEvent1 = [];
var IndividualId;
var SellAmount;
class OperationQuotaCumulus extends Uif2.Page {

	getInitialState() {
		$(document).ajaxStop($.unblockUI);
		OperationQuotaCumulus.AssignDecimal();
		UnderwritingRequest.GetModuleDateIssue().done((data) => {
			if (data.success) { CurrentDateQuota = FormatDate(data.result); }
		});
	}

	bindEvents() {
		$("#btnStrengthenedInformation").on('click', OperationQuotaCumulus.OpenModalOperationQuotaCumulus);
		$("#inputOperationalCapacity").focusout(OperationQuotaCumulus.FormatMoneyOut);
		$("#inputOperationalCapacity").focusin(OperationQuotaCumulus.NotFormatMoneyIn);
		$("#inputAccumulation").focusout(OperationQuotaCumulus.FormatMoneyOut);
		$("#inputAccumulation").focusin(OperationQuotaCumulus.NotFormatMoneyIn);
		$("#inputAvailable").focusout(OperationQuotaCumulus.FormatMoneyOut);
		$("#inputAvailable").focusin(OperationQuotaCumulus.NotFormatMoneyIn);
		$('#inputAvaliableOperationQuota').focusout(OperationQuotaCumulus.FormatMoneyOut);
		$('#inputAvaliableOperationQuota').focusin(OperationQuotaCumulus.NotFormatMoneyIn);
	}


	static OpenModalOperationQuotaCumulus() {

		$("#tableOperationQuotaCumulus").UifDataTable('clear');
		$('#_chkIsIndividual').prop("checked", false);
		$('#_chkIsConsortium').prop("checked", false);
		$('#_chkIsEconomicGroup').prop("checked", false);
		$('#_chkIsUT').prop("checked", false);
		$('#_chkIsSF').prop("checked", false);
		$("#mainRiskSurety").validate();
		if ($("#mainRiskSurety").valid()) {

			//Participante
            OperationQuotaCumulusRequest.GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(IndividualId, glbPolicy.Prefix.Id)
				.done((Consortium) => {
					if (Consortium.success) {
						if (Consortium.result.length > 0) {
							Consortium.result.forEach(function (item, index) {
								var OperationQuotaConsortiumEvent = {
									Id: item.OperatingQuotaEventID,
									OperationCuotaInitial: item.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount,
									OperationCuotaAvalible: item.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount - item.ApplyEndorsement.AmountCoverage / SellAmount,
									Cumulu: item.ApplyEndorsement.AmountCoverage / SellAmount,
								}
								if (item.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.CONSORTIUM) {
									$('#_chkIsConsortium').prop("checked", true);
									OperationQuotaConsortiumEvent.Belongingto = '2.' + item.consortiumEventDTO.consortiumDTO.ConsortiumName;
								} else if (item.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.TEMPORAL_UNION) {
									$('#_chkIsUT').prop("checked", true);
									OperationQuotaConsortiumEvent.Belongingto = '4.' + item.consortiumEventDTO.consortiumDTO.ConsortiumName;
								} else if (item.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.FUTURE_SOCIETY) {
									$('#_chkIsSF').prop("checked", true);
									OperationQuotaConsortiumEvent.Belongingto = '5.' + item.consortiumEventDTO.consortiumDTO.ConsortiumName;
								}
								OperationQuotaConsortiumEvent1.push(OperationQuotaConsortiumEvent);
							});
							$('#tableOperationQuotaCumulus').UifDataTable('addRow', OperationQuotaConsortiumEvent1);
							OperationQuotaConsortiumEvent1 = [];
						}

					}
				});

			//Individual
			if (OperationQuotaEvent != undefined && OperationQuotaEvent != null) {
				$('#_chkIsIndividual').prop("checked", true);
				$('#tableOperationQuotaCumulus').UifDataTable('addRow', OperationQuotaEvent);
			}
			//Grupo Economico
			if (EconomicGroupEvent != undefined && EconomicGroupEvent != null) {
				$('#_chkIsEconomicGroup').prop("checked", true);
				$('#tableOperationQuotaCumulus').UifDataTable('addRow', EconomicGroupEvent);
			}
			//Consorcio
			if (glbRisk.IsConsortium && ConsortiumEvent != undefined && ConsortiumEvent != null) {
				if (ConsortiumEvent.AssociationType == TypeOfAssociation.CONSORTIUM) {
					$('#_chkIsConsortium').prop("checked", true);
				} else if (ConsortiumEvent.AssociationType == TypeOfAssociation.TEMPORAL_UNION) {
					$('#_chkIsUT').prop("checked", true);
				} else if (ConsortiumEvent.AssociationType == TypeOfAssociation.FUTURE_SOCIETY) {
					$('#_chkIsSF').prop("checked", true);
				}
				$('#tableOperationQuotaCumulus').UifDataTable('addRow', ConsortiumEvent);
			}

			$('#modalOperationQuotaCumulus').UifModal('showLocal', Resources.Language.OperationQuotaCumulus);
			$('#inputSuretyName').text($('#inputSecure').val());
			$('#inputSearchPolicy').text($('#inputTemporal').val());
		}
	}

	static AssignDecimal() {
		$('#inputContractValue').OnlyDecimals(UnderwritingDecimal);
	}

	static async GetInformationAfianzado(InsuredId) {
		IndividualId = InsuredId;
		glbRisk.IsConsortium = false;
		SellAmount = glbPolicy.ExchangeRate.SellAmount;
		EconomicGroupEvent = ConsortiumEvent = OperationQuotaEvent = null;
		var array = new Array();

		if ($('#inputTotalSumInsuredRisk').text() == "0" || $('#inputTotalSumInsuredRisk').text() == "") {
			TotalSumInsuredRisk = 0;
		} else {
			TotalSumInsuredRisk = parseInt(NotFormatMoney($('#inputTotalSumInsuredRisk').text()));
		}
		//Marca para tipo de Afianzado
		lockScreen();
        var dataMarca = await OperationQuotaCumulusRequest.GetSecureType(IndividualId, glbPolicy.Prefix.Id);
        var endoso = false;

        if (dataMarca.result) {
			glbPolicy.IsEconomicGroup = dataMarca.result.IsEconomicGroup;
			glbPolicy.IsConsortium = dataMarca.result.IsConsortium;
			glbPolicy.IsIndividual = dataMarca.result.IsIndividual;
			glbPolicy.IsNotIndividual = dataMarca.result.IsNotIndividual;
		}


		await RiskSuretyRequest.IsConsortiumindividualId(IndividualId).then(data => {
			glbPolicy.IsConsortium = data.result;
			glbRisk.IsConsortium = data.result;
			IsConsortium = glbRisk.IsConsortium;
			return;
		})


		array.push(
			new Promise((resolve, reject) => {
                OperationQuotaCumulusRequest.GetOperatingQuotaEventByIndividualIdByLineBusinessId(IndividualId, glbPolicy.Prefix.Id)
					.done((OperationQuota) => {
						if (OperationQuota.success) {
							if (OperationQuota.result) {
								if (OperationQuota.result.IndividualOperatingQuota != 0) {
									OperationQuotaEvent = {
										Id: OperationQuota.result.OperatingQuotaEventID,
										Belongingto: '1.' + ($("#inputSecure").data("Object") ? $("#inputSecure").data("Object").Name : ""),
										OperationCuotaInitial: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount,
										OperationCuotaAvalible: OperationQuota.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount - OperationQuota.result.ApplyEndorsement.AmountCoverage / SellAmount,
										DateTo: FormatDate(OperationQuota.result.IndividualOperatingQuota.EndDateOpQuota),
										DateRegisty: FormatDate(OperationQuota.result.IssueDate),
										Cumulu: OperationQuota.result.ApplyEndorsement.AmountCoverage / SellAmount,
										InitDateOpQuota: OperationQuota.result.IndividualOperatingQuota.InitDateOpQuota,
										EndDateOpQuota: OperationQuota.result.IndividualOperatingQuota.InitDateOpQuota
									}

									OperationQuotaDecline = null;
									if (OperationQuota.result.declineInsured != null) {
										OperationQuotaDecline = {
											DeclineDate: OperationQuota.result.declineInsured.DeclineDate,
											Decline: OperationQuota.result.declineInsured.Decline
										}
									}
									$('#_chkIsIndividual').prop("checked", true);
								}
							}
						}
						resolve(OperationQuota);
					})
					.fail(err => reject(err));
			}));
		
        if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification ||
            glbPolicy.Endorsement.EndorsementType == EndorsementType.Cancellation ||
            glbPolicy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension)
        { endoso = true; }
		if (glbPolicy.IsConsortium) {
			array.push(
				new Promise((resolve, reject) => {
                    OperationQuotaCumulusRequest.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(IndividualId, glbPolicy.Prefix.Id, endoso, glbPolicy.Endorsement.Id)
						.done((Consortium) => {
							if (Consortium.success) {
								if (Consortium.result) {
									if (Consortium.result.consortiumEventDTO != null) {

										ConsortiumEvent = {
											Id: Consortium.result.OperatingQuotaEventID,
											OperationQuotaConsortium: Consortium.result.consortiumEventDTO.OperationQuotaConsortium / SellAmount,
											OperationCuotaInitial: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount,
											OperationCuotaAvalible: Consortium.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount - Consortium.result.ApplyEndorsement.AmountCoverage / SellAmount,
											DateTo: FormatDate(Consortium.result.IndividualOperatingQuota.EndDateOpQuota),
											DateRegisty: FormatDate(Consortium.result.IssueDate),
											Cumulu: Consortium.result.ApplyEndorsement.AmountCoverage / SellAmount,
										}
										if (Consortium.result.consortiumEventDTO.consortiumDTO != null) {
											ConsortiumEvent.AssociationType = Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType;
											if (Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.CONSORTIUM) {
												$('#_chkIsConsortium').prop("checked", true);
												ConsortiumEvent.Belongingto = '2.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName;
											} else if (Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.TEMPORAL_UNION) {
												$('#_chkIsUT').prop("checked", true);
												ConsortiumEvent.Belongingto = '4.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName;
											} else if (Consortium.result.consortiumEventDTO.consortiumDTO.AssociationType == TypeOfAssociation.FUTURE_SOCIETY) {
												$('#_chkIsSF').prop("checked", true);
												ConsortiumEvent.Belongingto = '5.' + Consortium.result.consortiumEventDTO.consortiumDTO.ConsortiumName;
											}
										}
										ConsortiumDecline = null;
										if (Consortium.result.declineInsured != null) {
											ConsortiumDecline = {
												DeclineDate: Consortium.result.declineInsured.DeclineDate,
												Decline: Consortium.result.declineInsured.Decline
											}
										}
									}
								}
							}
							resolve(Consortium);
						})
						.fail(xhr => {
							reject(xhr);
						})
				}));
		
		}
		if (glbPolicy.IsEconomicGroup) {
			array.push(
				new Promise((resolve, reject) => {
                    OperationQuotaCumulusRequest.GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(IndividualId, glbPolicy.Prefix.Id)
						.done(function (EconomicGroup) {
							if (EconomicGroup.success) {
								if (EconomicGroup.result.EconomicGroupEventDTO != null) {
									EconomicGroupEvent = {
										Id: EconomicGroup.result.OperatingQuotaEventID,
										Belongingto: '3.' + EconomicGroup.result.EconomicGroupEventDTO.economicgroupoperatingquotaDTO.EconomicGroupName,
										OperationCuotaInitial: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount,
										OperationCuotaAvalible: EconomicGroup.result.IndividualOperatingQuota.ValueOpQuotaAMT / SellAmount - EconomicGroup.result.ApplyEndorsement.AmountCoverage / SellAmount,
										DateTo: FormatDate(EconomicGroup.result.IndividualOperatingQuota.EndDateOpQuota),
										DateRegisty: FormatDate(EconomicGroup.result.IssueDate),
										Cumulu: EconomicGroup.result.ApplyEndorsement.AmountCoverage / SellAmount,
									}
									$('#_chkIsEconomicGroup').prop("checked", true);

									EconomicGroupDecline = null;
									if (EconomicGroup.result.declineInsured != null) {
										EconomicGroupDecline = {
											DeclineDate: EconomicGroup.result.declineInsured.DeclineDate,
											Decline: EconomicGroup.result.declineInsured.Decline
										}
									}
								}
							}
							resolve(EconomicGroup);
						}).fail(xhr => {
							reject(xhr);
						})
				}));
		}

		array.push(
			new Promise((resolve, reject) => {
				UnderwritingRequest.GetModuleDateIssue().done(function (data) {
					if (data.success) { CurrentDateQuota = FormatDate(data.result); }
					resolve(data);
				}).fail(xhr => {
					reject(xhr);
				});
			}));

		return Promise.all(array);
	}

	static SetInformationAfianzado() {
		var result = true;

		if (glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification || glbPolicyEndorsement.Summary.AmountInsured != 0) {
			var ValueAmount = undefined;
		} else if (glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification && ValueAmount == undefined) {
			if (glbPolicy.Summary.AmountInsured != undefined && glbPolicy.Summary.AmountInsured != null && parseInt(NotFormatMoney(glbPolicy.Summary.AmountInsured)) > 0 && ValueAmount == undefined) {
				ValueAmount = parseInt(NotFormatMoney(glbPolicy.Summary.AmountInsured));
			}

			if (glbPolicyEndorsement.Summary.AmountInsured != undefined && glbPolicyEndorsement.Summary.AmountInsured != null && parseInt(NotFormatMoney(glbPolicyEndorsement.Summary.AmountInsured)) > 0 && ValueAmount == undefined
				|| glbPolicyEndorsement.Summary.AmountInsured != undefined && glbPolicyEndorsement.Summary.AmountInsured != null && parseInt(NotFormatMoney(glbPolicyEndorsement.Summary.AmountInsured)) > 0 && ValueAmount == 0) {
				ValueAmount = parseInt(NotFormatMoney(glbPolicyEndorsement.Summary.AmountInsured));
			}
		}
		if (EconomicGroupEvent != undefined) {
			//Grupo Economico
			if (OperationQuotaCumulus.ValidateValidity(EconomicGroupEvent) == false) {
				return false;
			}
			$("#inputOperationalCapacity").val(FormatMoney(EconomicGroupEvent.OperationCuotaInitial.toFixed(UnderwritingDecimal)));
			$("#inputAccumulation").val(FormatMoney(EconomicGroupEvent.Cumulu.toFixed(UnderwritingDecimal)));
			$("#inputAvailable").val(FormatMoney(EconomicGroupEvent.OperationCuotaAvalible.toFixed(UnderwritingDecimal)));
			$("#inputAvaliableOperationQuota").hide();
			$("#LabelAvaliableOperationQuota").hide();
			if (parseInt(NotFormatMoney(ValueAmount)) < parseInt(NotFormatMoney($("#inputTotalSumInsuredRisk").text())) && glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification
				|| glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
				if (!CompareDateEquals(EconomicGroupEvent.DateTo, glbPolicy.CurrentFrom)) {
					result = false;
					$.UifNotify('show', { 'type': 'danger', 'message': AppResources.QuotaIsNotCurrent, 'autoclose': true });
				}
				if (EconomicGroupDecline != null && EconomicGroupDecline != undefined) {
					result = false;
					$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicySecureDisabled, 'autoclose': true });
					$("#inputOperationalCapacity").val(0);
					$("#inputAccumulation").val(0);
					$("#inputAvailable").val(0);
				}
			}

		}
		else if (ConsortiumEvent != undefined && ConsortiumDecline == undefined) {
			//Consorcio
			if (OperationQuotaCumulus.ValidateValidity(ConsortiumEvent) == false) {
				return false;
			}
			$("#inputOperationalCapacity").val(FormatMoney(ConsortiumEvent.OperationCuotaInitial.toFixed(UnderwritingDecimal)));
			$("#inputAccumulation").val(FormatMoney(ConsortiumEvent.Cumulu.toFixed(UnderwritingDecimal)));
			$("#inputAvailable").val(FormatMoney(ConsortiumEvent.OperationCuotaAvalible.toFixed(UnderwritingDecimal)));
			$('#inputAvaliableOperationQuota').val(FormatMoney(ConsortiumEvent.OperationQuotaConsortium.toFixed(UnderwritingDecimal)));
			$("#inputAvaliableOperationQuota").show();
			$("#LabelAvaliableOperationQuota").show();
			if (parseInt(NotFormatMoney(ValueAmount)) < parseInt(NotFormatMoney($("#inputTotalSumInsuredRisk").text())) && glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification
				|| glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
				if (TotalSumInsuredRisk != 0 && TotalSumInsuredRisk >= NotFormatMoney($('#inputAvaliableOperationQuota').val())) {
					result = false;
					$.UifNotify('show', {
						'type': 'danger', 'message': AppResources.MessageValidateOperatingAvailable, 'autoclose': true
					});
				}
				if (ConsortiumDecline != null && ConsortiumDecline != undefined) {
					result = false;
					$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicySecureDisabled, 'autoclose': true });
					$("#inputOperationalCapacity").val(0);
					$("#inputAccumulation").val(0);
					$("#inputAvailable").val(0);
				}
			}
		}
		else if (OperationQuotaEvent != null) {

			// Individual
			if (OperationQuotaCumulus.ValidateValidity(OperationQuotaEvent) == false) {
				return false;
			}
			$("#inputOperationalCapacity").val(FormatMoney(OperationQuotaEvent.OperationCuotaInitial.toFixed(UnderwritingDecimal)));
			$("#inputAccumulation").val(FormatMoney(OperationQuotaEvent.Cumulu.toFixed(UnderwritingDecimal)));
			$("#inputAvailable").val(FormatMoney(OperationQuotaEvent.OperationCuotaAvalible.toFixed(UnderwritingDecimal)));
			$("#inputAvaliableOperationQuota").hide();
			$("#LabelAvaliableOperationQuota").hide();
			if (parseInt(NotFormatMoney(ValueAmount)) < parseInt(NotFormatMoney($("#inputTotalSumInsuredRisk").text())) && glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification
				|| glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
				if (!CompareDateEquals(OperationQuotaEvent.DateTo, glbPolicy.CurrentFrom)) {
					result = false;
					$.UifNotify('show', { 'type': 'danger', 'message': AppResources.QuotaIsNotCurrent, 'autoclose': true });
				}
				if (OperationQuotaDecline != null && OperationQuotaDecline != undefined) {
					result = false;
					$("#inputOperationalCapacity").val(0);
					$("#inputAccumulation").val(0);
					$("#inputAvailable").val(0);
					$.UifNotify('show', { 'type': 'danger', 'message': AppResources.ErrorPolicySecureDisabled, 'autoclose': true });
				}
			}
		} else {
			result = false;
			$("#inputOperationalCapacity").val(0);
			$("#inputAccumulation").val(0);
			$("#inputAvailable").val(0);
		}

		if (parseInt(NotFormatMoney(ValueAmount)) < parseInt(NotFormatMoney($("#inputTotalSumInsuredRisk").text())) && glbPolicy.Endorsement.EndorsementType == EndorsementType.Modification
			|| glbPolicy.Endorsement.EndorsementType != EndorsementType.Modification) {
			if (ValidityParticipant1 != undefined && ValidityParticipant1 != null) {
				ValidityParticipant1.forEach(function (item, index) {
					if (CompareDateEquals(FormatDate(item.IndividualOperatingQuota.EndDateOpQuota), glbPolicy.CurrentFrom) && FormatDate(item.IndividualOperatingQuota.EndDateOpQuota) != null) {

					} else if (item.IndividualOperatingQuota.IndividualID != 0) {
						OperationQuotaCumulusRequest.GetIdentificationPersonOrCompanyByIndividualId(item.IndividualOperatingQuota.IndividualID).done(function (dataPerson) {
							if (dataPerson.success) {
								result = false;
								$.UifNotify('show', {
									'type': 'danger', 'message': 'Consorciado: ' + dataPerson.result.Name + ' con Numero de Documento : ' + dataPerson.result.Document + ' ' + AppResources.QuotaIsNotCurrent, 'autoclose': true
								});
							}
						});
					} else if (item.consortiumEventDTO.IndividualId != 0) {
						OperationQuotaCumulusRequest.GetIdentificationPersonOrCompanyByIndividualId(item.consortiumEventDTO.IndividualId).done(function (dataPerson) {
							if (dataPerson.success) {
								result = false;
								$.UifNotify('show', {
									'type': 'danger', 'message': 'Consorciado: ' + dataPerson.result.Name + ' con Numero de Documento : ' + dataPerson.result.Document + ' ' + AppResources.QuotaIsNotCurrent, 'autoclose': true
								});
							}
						});
					}


					if (item.consortiumEventDTO.IsConsortium) {
						OperationQuotaCumulusRequest.GetIdentificationPersonOrCompanyByIndividualId(item.IndividualOperatingQuota.IndividualID).done(function (dataPerson) {
							if (dataPerson.success) {
								result = false;
								$.UifNotify('show', {
									'type': 'danger', 'message': 'Consorciado: ' + dataPerson.result.Name + ' con Numero de Documento : ' + dataPerson.result.Document + ' no tiene cupo disponible.', 'autoclose': true
								});
							}
						});
					}
				});
			}
		}
		return result;
	}

	static NotFormatMoneyIn() {
		$(this).val(NotFormatMoney($(this).val()));
	}

	static FormatMoneyOut() {
		$(this).val(FormatMoney($(this).val()));
	}

	static ValidateValidity(ObjectEvent) {
		if (isNaN(ObjectEvent.DateTo) && CurrentDateQuota != null && CurrentDateQuota != undefined && OperationQuotaCumulus.CompareDates(CurrentDateQuota, ObjectEvent.DateTo)) {
			$.UifNotify('show', { 'type': 'danger', 'message': AppResources.QuotaIsNotCurrent, 'autoclose': true });

			$("#inputOperationalCapacity").val(0);
			$("#inputAccumulation").val(0);
			$("#inputAvailable").val(0);

			return false;
		}

	}

	static CompareDates(fecha, fecha2) {
		var xMonth = fecha.substring(3, 5);
		var xDay = fecha.substring(0, 2);
		var xYear = fecha.substring(6, 10);
		var yMonth = fecha2.substring(3, 5);
		var yDay = fecha2.substring(0, 2);
		var yYear = fecha2.substring(6, 10);

		if (xYear > yYear) {
			return (true)
		}

		else {
			if (xYear == yYear) {
				if (xMonth > yMonth) {
					return (true)
				}
				else {
					if (xMonth == yMonth) {
						if (xDay > yDay)
							return (true);
						else
							return (false);
					}
					else
						return (false);
				}
			}
			else
				return (false);
		}
	}

}
