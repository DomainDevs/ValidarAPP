var idItemGuarantee = null;
var objBinnacle =
    {
        bindEvents: function () {          
            $("#btnBinnacle").click(function () {
                objBinnacle.LoadPartialBinnacle();
                Guarantee.HidePanelsGuarantee();
                $("#buttonsGuarantee").hide();
                Guarantee.ShowPanelsGuarantee(MenuType.BINNACLE);
            });
            //Seccion Eventos
            $("#btnCancelBinnacle").click(function () {
                Guarantee.HidePanelsGuarantee();
                Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
            });

            $("#btnNewBinnacle").click(function () {

                $("#AddBinnacle").validate();
                var isValid = $("#AddBinnacle").valid();
                var text = "";

                if (isValid) {
                    var observation = $('#Observation').val();
                    var num = $('#Observation').val() == '' ? 0 : 1;
                    if (guaranteeId == 0) {
                        if (guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLog != null) {
                            //guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLog.Description = observation;
                            //guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLog.Description = observation;
                            guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLogObject = {
                                Description: observation
                            };
                        }
                        else {
                            var object = {};
                            object.IndividualId = individualId;
                            object.GuaranteeId = guaranteeId;
                            object.Description = observation;
                            guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLog = object;
                            guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLogObject = {
                                Description: observation
                            }
                        }
                    }
                    else {
                        if (guarantee[idItemGuarantee].InsuredGuarantee.InsuredGuaranteeLog != null) {
                            //guarantee[idItemGuarantee].InsuredGuarantee.InsuredGuaranteeLog.Description = observation;
                            //guarantee[idItemGuarantee].InsuredGuarantee.InsuredGuaranteeLog.Description = observation;
                            guarantee[idItemGuarantee].InsuredGuarantee.InsuredGuaranteeLogObject = {
                                Description : observation
                            }
                        }
                        else {
                            var object = {};
                            object.IndividualId = individualId;
                            object.GuaranteeId = guaranteeId;
                            object.GuaranteeStatusCode = guaranteeStatusCode;
                            object.Description = observation;
                            guarantee[idItemGuarantee].InsuredGuarantee.InsuredGuaranteeLog = object;
                            guarantee[idItemGuarantee].InsuredGuarantee.InsuredGuaranteeLogObject = object;
                        }
                    }
                    text = ("{0}" + (num > 0 ? "Con datos" : "Sin datos") + "{1}").format("(",")");
                    // $('#selectedBinnacle').html(observation); 
                    $('#selectedBinnacle').html(text);
                    Guarantee.HidePanelsGuarantee();
                    Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
                }
            });
        },

        LoadPartialBinnacle: function () {
            objBinnacle.CleanListBinnacle();
            var num = 0;
            var text = "";
            $('#Binnacle-NumberDocument').val(numberDoc);
            $('#inputTypeGuarantee').val($("#selectGuarantees").UifSelect('getSelectedText'));
            $('#inputBinacleSecureName').val($("#inputContractorName").text());
            //Con el último registro lleno las observaciones
            if (guaranteeId == 0) {
                if (guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLog != null) {
                    //$('#Observation').val(guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLog.Description);
                    //$('#Observation').val(guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLog.Description);
                    $('#Observation').val(guaranteeTmp.InsuredGuarantee.InsuredGuaranteeLogObject.Description);
                    num = 1;
                }
                else {
                    $('#Observation').val("");
                }
            }
            else {
                for (var i = 0; i < guarantee.length; i++) {
                    if (guarantee[i].InsuredGuarantee.Id == guaranteeId) {
                        idItemGuarantee = i;
                        if (guarantee[i].InsuredGuarantee.InsuredGuaranteeLog != null) {
                            if (guarantee[i].InsuredGuarantee.InsuredGuaranteeLog.length > 0) {
                                $.each(guarantee[i].InsuredGuarantee.InsuredGuaranteeLog, function (index, val) {
                                    guarantee[i].InsuredGuarantee.InsuredGuaranteeLog[index].LogDate = FormatFullDate(val.LogDate);
                                });

                                $("#listBinnacle").UifListView(
                                    {
                                        displayTemplate: "#display-binnacle",
                                        sourceData: guarantee[i].InsuredGuarantee.InsuredGuaranteeLog, //rootPath + 'Guarantees/Guarantees/GetInsuredGuaranteeLogs?individualId=' + individualId + '&guaranteeId=' + guaranteeId,
                                        height: 200
                                        
                                    });
                                num = guarantee[i].InsuredGuarantee.InsuredGuaranteeLog.length;

                                //var binnnacles_ = jQuery.extend(true, [], $("#listBinnacle").UifListView("getData"));

                                //Llenar list de Binnacles
                                //$("#listBinnacle").UifListView({ source: rootPath + 'Guarantees/Guarantees/GetInsuredGuaranteeLogs?individualId=' + individualId + '&guaranteeId=' + guaranteeId, displayTemplate: "#display-binnacle", height: 200 });
                                //$("#listBinnacle").UifListView(
                                //{
                                //    customAdd: true,
                                //    customEdit: false,
                                //    //delete: true,
                                //    //deleteCallback: objGuarantors.deleteGuarantor,
                                //    edit: true,
                                //    displayTemplate: "#modalBinnacle",
                                //    //addTemplate: '#AddBinnacle',//'#add-template',
                                //    height: 300
                                //});

                                //First option of list order by [Desc]
                                //$('#Observation').val(guarantee[i].InsuredGuarantee.InsuredGuaranteeLog[0].Description);
                            }
                        }
                        else {
                            $('#Observation').val("");
                        }
                        break;
                    }
                }
            }
            if (num == 0) {
                num = "Sin datos";
            }
            text = "(" + num + ")";
            //$('#selectedBinnacle').html($('#Observation').val());
            $('#selectedBinnacle').html(text); 
        },
        CleanListBinnacle: function ()
        {
            $("#listBinnacle").UifListView({ sourceData: null, height: 'auto' });
        }
        
    }
