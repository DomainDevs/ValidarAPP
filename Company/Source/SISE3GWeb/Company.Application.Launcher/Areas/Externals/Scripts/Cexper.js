
var resultPoliciesHistorical = [];
var resultSinisterHistorical = [];
var resultSimit = [];

class Cexper extends Uif2.Page {

    getInitialState() {
        $('#inputLicensePlate').prop("disabled", false);
        $("#inputLicensePlate").TextTransform(ValidatorType.UpperCase);
        Cexper.GetDocumentType("3").done(function (data) {
            if (data.success) {
                $("#DocumentType").UifSelect({ sourceData: data.result });
            }
        });

    }

    bindEvents() {

        $('#btnSearch').click(Cexper.SearchCexper);
        $('#tableResultsPolicies').on('rowSelected', this.TablePolicyLoad);
        $('#tableResultsSinister').on('rowSelected', this.TableSinisterLoad);
        $('#btnExitPolicyCexperDetail').click(this.ExitPolicyCexperDetail);
        $('#btnExitSinisterCexperDetail').click(this.ExitSinisterCexperDetail);
        $("#TypeCrossing").on("itemSelected", this.TypeCrossingSelected);
        $("#TypeCrossingSinister").on("itemSelected", this.TypeCrossingSinisterSelected);
        $('#btnExitCexper').click(this.ExitCexper);
    }

    TypeCrossingSelected(event, selectedItem) {
        var TypeCrossing = selectedItem.Text;
        if (selectedItem.Id == 0) {
            $('#tableResultsPolicies').UifDataTable({ sourceData: resultPoliciesHistorical });
        }
        else {
            $("#tableResultsPolicies").UifDataTable('clear');
            for (var i = 0; i < resultPoliciesHistorical.length; i++) {
                if (TypeCrossing == resultPoliciesHistorical[i].InsuredTypeDocument) {
                    $('#tableResultsPolicies').UifDataTable('addRow', resultPoliciesHistorical[i] );
                }
            }
        }
    }

    TypeCrossingSinisterSelected(event, selectedItem) {
        var TypeCrossing = selectedItem.Text;
        if (selectedItem.Id == 0) {
            $('#tableResultsSinister').UifDataTable({ sourceData: resultSinisterHistorical });
        }
        else {
            $("#tableResultsSinister").UifDataTable('clear');
            for (var i = 0; i < resultSinisterHistorical.length; i++) {
                if (TypeCrossing == resultSinisterHistorical[i].InsuredTypeDocument) {
                    $('#tableResultsSinister').UifDataTable('addRow', resultSinisterHistorical[i]);
                }
            }
        }
    }

    static SearchCexper() {

        if ($('#formCexper').valid()) {
            
            var licensePlate = $('#inputLicensePlate').val();
            var documentType = $('#DocumentType').val();
            var documentNumber = $('#inputDocumentNumber').val();

            $("#DocumentType").UifSelect("disabled", true);
            $("#inputLicensePlate").attr('disabled', 'disabled');
            $("#inputDocumentNumber").attr('disabled', 'disabled');
            $('#btnSearch').attr('disabled', 'disabled');

            return $.ajax({
                type: 'POST',
                url: rootPath + 'Externals/Cexper/GetCexper',
                data: JSON.stringify({ licensePlate, documentType, documentNumber }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8'
            }).done(function (data) {
                $('#resultDiv').css('display', 'none');
                Cexper.CleanForm();
                if (data.success) {
                    if (data.result.PoliciesInfo.length != 0) {
                        $('#tableResultsPolicies').UifDataTable('addRow', data.result.PoliciesInfo);
                        resultPoliciesHistorical = data.result.PoliciesInfo;
                    }
                    else {
                        $('#tableResultsPolicies').UifDataTable({ sourceData: data.result.PoliciesInfo });
                    }
                    if (data.result.SinisterInfo.length != 0) {
                        $('#tableResultsSinister').UifDataTable('addRow', data.result.SinisterInfo);
                        resultSinisterHistorical = data.result.SinisterInfo;
                    }
                    else {
                        $('#tableResultsSinister').UifDataTable({ sourceData: data.result.SinisterInfo });
                    }
                    if (data.result.Simit.length != 0) {
                        $('#tableSimitResults').UifDataTable('addRow', data.result.Simit);
                        resultSimit = data.result.Simit;
                    }
                    else {
                        $('#tableSimitResults').UifDataTable({ sourceData: data.result.Simit });
                    }
                    Cexper.GetDocumentType("3").done(function (dato) {
                        if (dato.success) {
                            $("#TypeCrossing").UifSelect({ sourceData: dato.result });
                            $("#TypeCrossingSinister").UifSelect({ sourceData: dato.result });
                        }
                    });
                    $('#resultDiv').css('display', 'block');
                    $('#linkPromissoryNote').click();
                }
                else
                {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
                $("#DocumentType").UifSelect("disabled", false);
                $("#inputLicensePlate").removeAttr('disabled');
                $("#inputDocumentNumber").removeAttr('disabled');
                $('#btnSearch').removeAttr('disabled');
            });
        } else {
            var msj = "";
            if (licensePlate === "") {
                msj = msj + Resources.Language.LabelPlate + " <br>";
            } if (documentNumber === "") {
                msj = msj + Resources.Language.LabelDocumentNumber + " <br>";
            } if ($("#DocumentType").UifSelect("getSelected") === "") {
                msj = msj + Resources.Language.LabelTypeDocument + " <br>";
            } if (msj !== "") {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.LabelInformative + " <br>" + msj, 'autoclose': true });
                return false;
            }
            return true;
        }
    }

    static GetTypeCrossing(response) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Externals/Cexper/GetTypeCrossing',
            dataType: 'json',
            data: JSON.stringify({ response: response }),
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                $("#TypeCrossing").UifSelect({ sourceData: data.result });
            }
        });
    }

    static GetTypeCrossingSinister(response) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Externals/Cexper/GetTypeCrossingSinister',
            dataType: 'json',
            data: JSON.stringify({ response: response }),
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                $("#TypeCrossingSinister").UifSelect({ sourceData: data.result });
            }
        });
    }

    static GetDocumentType(typeDocument) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Externals/Cexper/GetDocumentType",
            data: JSON.stringify({ typeDocument: typeDocument }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    TablePolicyLoad() {
        var policy = $("#tableResultsPolicies").UifDataTable("getSelected");
        if (policy !== null) {
            $('#CompanyName').val(policy[0].CompanyName);
            $('#PolicyNumbers').val(policy[0].PolicyNumber);
            $('#Order').val(policy[0].Order);
            $('#StatusPolicy').val(policy[0].Valid);
            $('#EffectiveDate').val(FormatDate(policy[0].EffectiveDate));
            $('#PolicyClass').val(policy[0].PolicyClass);
            $('#LicensePlate').val(policy[0].Plate);
            $('#Engine').val(policy[0].Engine);
            $('#Chassis').val(policy[0].Chassis);
            $('#Make').val(policy[0].Brand);
            $('#ClassVehicle').val(policy[0].Class);
            $('#TypeVehicle').val(policy[0].Type);
            $('#Model').val(policy[0].Model);
            $('#Color').val(policy[0].Color);
            $('#Service').val(policy[0].Service);
            $('#InsuredTypeDocument').val(policy[0].InsuredTypeDocument);
            $('#InsuredDocumentNumber').val(policy[0].DocumentNumber);
            $('#InsuredName').val(policy[0].Insured);
            $('#PTD').val(policy[0].PTD);
            $('#PPD').val(policy[0].PPD);
            $('#PH').val(policy[0].PH);
            $('#PPH').val(policy[0].PPH);
            $('#RC').val(policy[0].RC);
            $('#ModalPolicyCexperDetail').UifModal('showLocal', 'Detalle póliza');
            $("#tableResultsPolicies").UifDataTable("unselect");
            
        }
    }

    TableSinisterLoad() {
        var Sinister = $("#tableResultsSinister").UifDataTable("getSelected");
        if (Sinister !== null) {
                $('#protection').val(Sinister[0].Amparos[0].NombreAmparado);
                $('#Claim').val(Sinister[0].Amparos[0].ValorReclamaAmparo);
                $('#paid').val(Sinister[0].Amparos[0].ValorPagadoAmparo);
                $('#Date').val(FormatDate(Sinister[0].Amparos[0].FechaSiniestro));
                $('#status').val(Sinister[0].Amparos[0].Estado);
            $('#ModalSinisterCexperDetail').UifModal('showLocal', 'Detalle Siniestro');
            $("#tableResultsSinister").UifDataTable("unselect");

        }
    }

    ExitPolicyCexperDetail() {
        $('#ModalPolicyCexperDetail').UifModal('hide');
    }

    ExitSinisterCexperDetail() {
        $('#ModalSinisterCexperDetail').UifModal('hide');
    }

    static CleanForm() {
        //$('#inputLicensePlate').val('');
        $('#DocumentType').UifSelect('setSelected', null);
        //$('#inputDocumentNumber').val('');
        $('#inputVerificationCode').val('');
        $('#tableResultsPolicies').UifDataTable('clear');
        $('#tableResultsSinister').UifDataTable('clear');
        $('#tableSimitResults').UifDataTable('clear');
        //$('#btnSearch').attr("disabled", true)
    }

    ExitCexper() {
        window.location = rootPath + "Home/Index";
    }
}
