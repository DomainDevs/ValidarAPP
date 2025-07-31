var idGuarantee = null;
//Seccion Funciones
var objPrefixAssocieted =
    {
        bindEvents: function () {
            $("#btnPrefixAssociated").click(function () {
                objPrefixAssocieted.LoadPartialPrefixAssociated();
                Guarantee.HidePanelsGuarantee();
                $("#buttonsGuarantee").hide();
                Guarantee.ShowPanelsGuarantee(MenuType.PREFIXASSOCIATED);
            });
            $("#btnCancelPrefixAssociated").click(function () {
                Guarantee.HidePanelsGuarantee();
                Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
            });

            $("#btnNewBranchAssociated").click(function () {
                var table = $("#tablePrefixAssociated").UifDataTable('getSelected');
                var num = 0;
                var text = ""
                if (guaranteeId == 0) {
                    if (guaranteeTmp.InsuredGuarantee.listPrefix != null) {
                        //guaranteeTmp.InsuredGuarantee.listPrefix.length = 0;
                        while (guaranteeTmp.InsuredGuarantee.listPrefix.length > 0) {
                            guaranteeTmp.InsuredGuarantee.listPrefix.pop();
                        }
                        if (table != null) {
                            for (var k = 0; k < table.length; k++) {
                                num = (k + 1);
                                guaranteeTmp.InsuredGuarantee.listPrefix.push({
                                    PrefixCode: table[k].Id,
                                    GuaranteeId: guaranteeId,
                                    IndividualId: individualId
                                })
                            }
                        }
                    }
                    else {
                        var listPrefix = [];
                        guaranteeTmp.InsuredGuarantee.listPrefix = listPrefix;
                        if (table != null) {
                            for (var k = 0; k < table.length; k++) {
                                num = (k + 1);
                                guaranteeTmp.InsuredGuarantee.listPrefix.push({
                                    PrefixCode: table[k].Id,
                                    GuaranteeId: guaranteeId,
                                    IndividualId: individualId
                                })
                            }
                        }
                    }
                }
                else {
                    for (var i = 0; i < guarantee.length; i++) {
                        if (guarantee[i].InsuredGuarantee.Id == guaranteeId) {
                            idGuarantee = i;
                        }
                    }
                    guarantee[idGuarantee].InsuredGuarantee.listPrefix.length = 0;
                    if (table != null) {
                        for (var k = 0; k < table.length; k++) {
                            num = (k + 1);
                            guarantee[idGuarantee].InsuredGuarantee.listPrefix.push({
                                PrefixCode: table[k].Id,
                                GuaranteeId: guaranteeId,
                                IndividualId: individualId
                            })
                        }
                    }
                }
                window.PrefixSelected = num;
                if (num == 0) {
                    num = "Sin datos";
                }
                text = "(" + num + ")";
                $('#selectedPrefixAssociated').html(text);
                Guarantee.HidePanelsGuarantee();
                Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);

            });
        },

        LoadPartialPrefixAssociated: function () {
            //$("#tablePrefixAssociated").UifDataTable({ sourceData: null});
            var num = 0;
            var text = ""
            $('#PrefixAssociated-NumberDocument').val(numberDoc);
            $('#inputPrefixSecureName').val($("#inputContractorName").text());
            $('#inputTypeGuaranteePrefixAssociated').val($("#selectGuarantees").UifSelect('getSelectedText'));
            
            GuaranteeRequest.GetPrefixes($('#hardRiskType_Code').val()).done(function(data) {
                if (data.success) {
                    $("#tablePrefixAssociated").UifDataTable({ sourceData: data.result });

                    var table = $('#tablePrefixAssociated >tbody >tr')
                    //var status = false;
                    
                    if (guaranteeId == 0) {
                        if (guaranteeTmp.InsuredGuarantee != undefined && guaranteeTmp.InsuredGuarantee.listPrefix != null && guaranteeTmp.InsuredGuarantee.listPrefix.length > 0) {
                            //num = guaranteeTmp.InsuredGuarantee.listPrefix.length;
                            for (var j = 0; j < guaranteeTmp.InsuredGuarantee.listPrefix.length; j++) {
                                for (var k = 0; k < table.length; k++) {
                                    //num = (k + 1);
                                    if ($(table[k].children[0]).text() == guaranteeTmp.InsuredGuarantee.listPrefix[j].PrefixCode) {
                                        $(table[k].children[2].children[0].children[0]).trigger('click');
                                        //num++;
                                        //status = true;
                                    }

                                }
                            }
                        }
                        else {
                            //num = 1;
                            for (var i = 0; i < table.length; i++) {
                                if ($(table[i].children[2]).text() == SubCoveredRiskType.Surety) {
                                    $(table[i].children[3].children[0].children[0]).trigger('click');
                                    num++;
                                    //status = true;
                                }
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < guarantee.length; i++) {
                            if (guarantee[i].InsuredGuarantee.Id == guaranteeId) {
                                //idGuarantee = i;
                                //num = guarantee[i].InsuredGuarantee.listPrefix.length;
                                if (guarantee[i].InsuredGuarantee.listPrefix.length > 0)
                                {
                                    for (var j = 0; j < guarantee[i].InsuredGuarantee.listPrefix.length; j++) {
                                        for (var k = 0; k < table.length; k++) {
                                            //num = (k + 1);
                                            if ($(table[k].children[0]).text() == guarantee[i].InsuredGuarantee.listPrefix[j].PrefixCode) {
                                                $(table[k].children[2].children[0].children[0]).trigger('click');
                                                num++;
                                                //status = true;
                                            }
                                        }
                                    }
                                } else {
                                        for (var z = 0; z < table.length; z++) {
                                            if ($(table[z].children[2]).text() == SubCoveredRiskType.Surety) {
                                                $(table[z].children[3].children[0].children[0]).trigger('click');
                                                num++;
                                                break;
                                                //status = true;
                                            }
                                        }
                                }
                                
                            }
                        }
                    }
                    window.PrefixSelected = num;
                    if (num == 0) {
                        num = "Sin datos";
                    }
                    text = "(" + num + ")";
                    $('#selectedPrefixAssociated').html(text);
                }
            });
           // $("#tablePrefixAssociated").UifDataTable({ source: rootPath + "Person/Guarantee/GetPrefixAssociated?COVERED_RISK_TYPE_CD=" + $('#hardRiskType_Code').val() });
                       
        }

    }

