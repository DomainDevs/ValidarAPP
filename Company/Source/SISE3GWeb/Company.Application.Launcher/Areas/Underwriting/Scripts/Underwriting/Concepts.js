//Codigo de la pagina Conceptos.cshtml
class UnderwritingConcepts extends Uif2.Page {
    getInitialState() {
        //ConceptsCoverage.LoadConcepts();
    }
    bindEvents() {
        $('#btnConcepts').on('click', UnderwritingConcepts.ConceptsGeneral);
    }

    //static LoadConcepts() {

    //    $("#formUnderwriting").validate();

    //    if (glbPolicy.Id > 0 && $("#formUnderwriting").valid()) {
    //        Underwriting.ShowPanelsIssuance(MenuType.Concepts);
    //    }

    //}

    static ShowModalDynamicProperties(title) {
        debugger
        if (dynamicProperties != null) {
            let promises = [];

            dynamicProperties.forEach(item => {
                var conceptTmp = dynamicConcepts.find(x => { return x.ConceptId == item.Id && x.Entity.EntityId == item.EntityId });

                let promise = new Promise((resolve, reject) => {
                    if (conceptTmp) {
                        resolve({ Concept: conceptTmp, Value: item.Value });
                    } else {
                        RequestConcepts.GetConceptByIdConceptIdEntity(item.Id, item.EntityId)
                            .done(data => {
                                if (data.success) {
                                    resolve({ Concept: data.result, Value: item.Value });
                                } else {
                                    reject(`Error consultando concepto ${item.Id}-${item.EntityId}`);
                                }
                            })
                            .fail(() => { reject(`Error consultando concepto ${item.Id}-${item.EntityId}`); });
                    }
                }).then(result => {
                    if (conceptTmp) {
                        return result;
                    }
                    return new Promise((resolve, reject) => {
                        RequestConcepts
                            .GetSpecificConceptWithVales(item.Id, item.EntityId, [], result.Concept.ConceptType)
                            .done(data => {
                                if (data.success) {
                                    data.result.Description += ` (${item.Id})`;
                                    resolve({ Concept: data.result, Value: result.Value });
                                } else {
                                    reject(`Error consultando concepto ${result.Concept.ConceptId}-${result.Concept.Entity.EntityId}`);
                                }
                            })
                            .fail(() => {
                                reject(`Error consultando concepto ${result.Concept.ConceptId}-${result.Concept.Entity.EntityId}`);
                            });
                    });
                }).then(result => {
                    switch (result.Concept.ConceptType) {
                        case 1: //Basico
                            switch (result.Concept.BasicType) {
                                case 1://Numeric
                                case 3://Decimal
                                    result.Value = result.Value;
                                    break;
                                case 2://Text
                                    break;
                                case 4://Date
                                    break;
                            }
                            break;
                        case 2://Rango
                            var rangeEntity = result.Concept.RangeEntity.RangeEntityValues.find(
                                x => {
                                    return x.RangeValueCode == result.Value;
                                });

                            result.Value = `${rangeEntity.FromValue}-${rangeEntity.ToValue} (${rangeEntity.RangeValueCode})`;
                            break;                        
                        //case 3://Lista
                        //    if (result.Concept.ListEntity.ListEntityCode == 2 && typeof (result.Value) == "string") {
                        //        result.Value = result.Value.toLowerCase() === "true";
                        //    }

                        //    var listEntity = result.Concept.ListEntity.ListEntityValues.find(x => {
                        //        return x.ListValueCode == result.Value;
                        //    });

                        //    result.Value = `${listEntity.ListValue} (${listEntity.ListValueCode})`;
                        //    break;
                        case 4://Referencia
                            var referenceEntity = result.Concept.ReferenceValues.find(x => {
                                return x.Id == result.Value;
                            });

                            result.Value = `${referenceEntity.Description} (${referenceEntity.Id})`;
                            break;
                    }

                    dynamicConcepts.push(result.Concept);
                    return { Concept: result.Concept, Value: result.Value };
                });

                promises.push(promise);
            });

            Promise.all(promises).then(values => {
                $("#tableConcepts").UifDataTable('clear');
                $('#modalConcepts').UifModal('showLocal', title);
                $("#tableConcepts").UifDataTable({ sourceData: values });
            }).catch(xhr => {
                console.log(xhr);
            });
        }
    }
 
    static ConceptsGeneral() {
        $("#formUnderwriting").validate();
        if ($("#formUnderwriting").valid()) {
            if ($("#inputTemporal").val() != null && $("#inputTemporal").val() != "") {
                ConceptsCoverage.ShowModalDynamicProperties('Conceptos General');
            }
        }
    }

}
