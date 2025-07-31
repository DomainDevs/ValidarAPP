var objDocumentationReceived =
    {
        bindEvents: function () {
            var idGuarantee = null;
            $("#btnDocumentationReceived").click(function () {
                //Validación Garantía - Documentación recibida
                //$("#selectGuaranteeList").validate();
                //$("#AddPromissoryNote").validate();
                objDocumentationReceived.LoadPartialDocumentationReceived();
                Guarantee.HidePanelsGuarantee();
                $("#buttonsGuarantee").hide();
                Guarantee.ShowPanelsGuarantee(MenuType.DOCUMENTATION);
                //var promissoryNoteValid = $("#AddPromissoryNote").valid();

                //if (Guarantee.ValidateGuarantee())
                //{
                    
                //}
            });

            $("#btnCancelDocumentation").click(function () {
                Guarantee.HidePanelsGuarantee();
                Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
            });

            $("#btnNewDocumentatioReceived").click(function () {

                var table = $("#tableDocumentationReceived").UifDataTable('getSelected');
                var text = "";
                var num = 0;
                if (guaranteeId == 0) {
                    if (guaranteeTmp.InsuredGuarantee.listDocumentation != null) {
                        guaranteeTmp.InsuredGuarantee.listDocumentation.length = 0;
                        if (table != null) {
                            for (var k = 0; k < table.length; k++) {
                                num = (k + 1);
                                guaranteeTmp.InsuredGuarantee.listDocumentation.push({
                                    DocumentCode: table[k].DocumentCode,
                                    GuaranteeId: guaranteeId,
                                    IndividualId: individualId,
                                    GuaranteeCode: table[k].GuaranteeCode
                                });
                                window.documentSelected = num;
                            }
                        }
                    }
                    else {
                        var listDocumentation = [];
                        guaranteeTmp.InsuredGuarantee.listDocumentation = listDocumentation;
                        if (table != null) {
                            for (var k = 0; k < table.length; k++) {
                                num = (k + 1);
                                guaranteeTmp.InsuredGuarantee.listDocumentation.push({
                                    DocumentCode: table[k].DocumentCode,
                                    GuaranteeId: guaranteeId,
                                    IndividualId: individualId,
                                    GuaranteeCode: table[k].GuaranteeCode
                                });
                                window.documentSelected = num;
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

                    guarantee[idGuarantee].InsuredGuarantee.listDocumentation.length = 0;
                    if (table != null) {
                        for (var k = 0; k < table.length; k++) {
                            num = (k + 1);
                            guarantee[idGuarantee].InsuredGuarantee.listDocumentation.push({
                                DocumentCode: table[k].DocumentCode,
                                GuaranteeId: guaranteeId,
                                IndividualId: individualId,
                                GuaranteeCode: table[k].GuaranteeCode/*GuaranteeCode*/
                            });
                        }
                    }
                }
                window.documentSelected = num;
                if (num == 0) {
                    num = "Sin datos";
                }
                text = "(" + num + ")";
                $('#selectedDocumentationReceived').html(text);
                Guarantee.HidePanelsGuarantee();
                Guarantee.ShowPanelsGuarantee(MenuType.GUARANTEE);
            });
        },

        LoadPartialDocumentationReceived: function () {
            
            $('#Documentation-NumberDocument').val(numberDoc);
            $('#SecureName').val($("#inputContractorName").text());
            $('#inputTypeGuaranteeDocumentation').val($("#selectGuarantees").UifSelect('getSelectedText'));

            var code = 0;
            if (guaranteeCode === "") {
                Guarantee.selectGuaranteeCodeEnum();
                code = guaranteeTmp.Code;
            }
            else {
                code = guaranteeCode;
            }

            objDocumentationReceived.LoadDocumentationReceived();
        },
        
        LoadDocumentationReceived: function ()
        {
            var text = "";
            var num = 0;
            window.documentSelected = -1;
            GuaranteeRequest.GetDocumentationReceived(guaranteeCode).done(function (data) {
                if (data.success) {
                    $("#tableDocumentationReceived").UifDataTable({ sourceData: data.result });

                    var table = $('#tableDocumentationReceived >tbody >tr')
                    //var status = false;

                    if (guaranteeId == 0) {
                        if (guaranteeTmp.InsuredGuarantee.listDocumentation != null) {
                            //num = guaranteeTmp.InsuredGuarantee.listDocumentation.length;
                            for (var j = 0; j < guaranteeTmp.InsuredGuarantee.listDocumentation.length; j++) {
                                for (var k = 0; k < table.length; k++) {
                                    //num = (k + 1);
                                    if ($(table[k].children[0]).text() == guaranteeTmp.InsuredGuarantee.listDocumentation[j].DocumentCode) {
                                        $(table[k].children[2].children[0].children[0]).trigger('click');
                                        //status = true;
                                        num++;
                                    }
                                }
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < guarantee.length; i++) {
                            //num = guarantee[i].InsuredGuarantee.listDocumentation.length;
                            if (guarantee[i].InsuredGuarantee.Id == guaranteeId) {
                                for (var j = 0; j < guarantee[i].InsuredGuarantee.listDocumentation.length; j++) {
                                    for (var k = 0; k < table.length; k++) {
                                        //num = (k + 1);
                                        if ($(table[k].children[0]).text() == guarantee[i].InsuredGuarantee.listDocumentation[j].DocumentCode) {
                                            $(table[k].children[2].children[0].children[0]).trigger('click');
                                            //status = true;
                                            num++;
                                        }
                                    }
                                }
                                //break;
                            }
                        }
                    }
                    window.documentSelected = num;
                    if (num == 0) {
                        num = "Sin datos";
                    }
                    text = "(" + num + ")";
                    $('#selectedDocumentationReceived').html(text);
                }
            });
        }
    }

