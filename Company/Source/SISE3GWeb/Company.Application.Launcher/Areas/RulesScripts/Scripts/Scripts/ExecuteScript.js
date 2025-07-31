class ExecuteScript {

    constructor() {
        this.Questions = [];
        this.WindowName = null;
        this.ClassName = null;
        this.DynamicProperties = [];
    }

    static GetScript(scriptId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Scripts/GetScriptComposite?ScriptId=" + scriptId
        });
    }
    static GetQuestion(questionId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Scripts/GetQuestionControl?QuestionId=" + questionId
        });
    }

    static Execute(scriptId, windowName, className, dynamicProperties) {
        ExecuteScript.GetScript(scriptId).done(data => {
            ExecuteScript.ExecuteWithData(data, windowName, className, dynamicProperties);
        });
    }

    static ExecuteWithData(data, windowName, className, dynamicProperties) {
        const executeScript = new ExecuteScript();


        if (typeof dynamicProperties == "undefined") {
            dynamicProperties = [];
        }
        if (dynamicProperties == null) {
            dynamicProperties = [];
        }


        executeScript.DrawScript(data, windowName, className, dynamicProperties);

    }

    AssignData() {
        this.Questions.forEach((item, index) => {
            var dProperty = this.DynamicProperties.find(x => x.Id === item.ConceptId);

            if (dProperty !== undefined) {
                if ($(`#${item.QuestionId}`).hasClass("uif-select")) {
                    $(`#${item.QuestionId}`).UifSelect("setSelected", Number(dProperty.Value));
                    $(`#${item.QuestionId}`).trigger("change", {});
                }
                else {
                    $(`#${item.QuestionId}`).val(dProperty.Value);
                    $(`#${item.QuestionId}`).trigger("change", {});
                }
            }
        });
    }

    DrawScript(data, windowName, className, dynamicProperties) {
        this.WindowName = windowName;
        this.ClassName = className;
        this.DynamicProperties = this.DynamicProperties.concat(dynamicProperties);

        var node = data.Nodes.find(x => { return x.NodeId === 1 });

        this.DrawNode(data.Nodes, node, "").then(result => {
            const drawScript = [];
            $("#QuestionPanelScript").html(result);
            $.each(this.Questions, (index, value) => {
                drawScript.push(SearchCombo.obtencionTipoControl(value.ConceptId, value.EntityId, `#${value.QuestionId}`));
            });
            return Promise.all(drawScript);
        }).then((resolve) => {
            $("#btnRecordScrip").unbind("click");
            $(".inputDinamic").unbind("change");

            $(".inputDinamic").on("change", this.OnChange.bind(this));
            $("#btnRecordScrip").on("click", this.OnSave.bind(this));


            this.OpenModal(data.Script.Description);
            setTimeout(() => { this.AssignData() }, 100);
        }).catch(() => {

        });
    }

    OnSave() {

        let isValid = this.Validate();

        if (isValid) {
            let ids = $(".uif-select .form-control.inputDinamic");
            let dynamicPropertiesTemp = [];
            let dynamicProperties = [];

            for (let x = 0; x < ids.length; x++) {
                var concept = $(ids[x]).data("concept");
                var entity = $(ids[x]).data("entity");
                var value = $(ids[x]).val();
                switch (ids[x].type) {
                    case "select-one":
                    case "text":
                    case "date":
                    case "number":
                        if ($(`#${$(ids[x]).attr("id")}`).is(":visible") === true) {
                            dynamicProperties.push(SearchCombo.castearControl(concept, entity, value).then(data => {
                                data.Status = true;
                                dynamicPropertiesTemp.push(data);
                            }));
                        } else {
                            dynamicProperties.push(SearchCombo.castearControl(concept, entity, value).then(data => {
                                data.Status = false;
                                dynamicPropertiesTemp.push(data);
                            }));
                        }

                        break;
                    default:
                        break;
                }
            }

            Promise.all(dynamicProperties).then(() => {
                dynamicPropertiesTemp.forEach(item => {
                    let dynamicProperty = this.DynamicProperties.find(x => { return x.Id === item.Id });
                    if (dynamicProperty !== undefined) {
                        this.DynamicProperties.splice(this.DynamicProperties.indexOf(dynamicProperty), 1);
                    }
                });

                const items = dynamicPropertiesTemp
                    .filter((item) => { return item.Status === true })
                    .map((item) => { return { "Id": item.Id, "Value": item.Value, "EntityId": item.EntityId } });
                this.DynamicProperties = this.DynamicProperties.concat(items);




                if (this.ClassName !== undefined && this.ClassName !== null) {
                    if (this.ClassName.PostScript !== undefined && this.ClassName.PostScript !== null) {
                        this.ClassName.PostScript(true, this.DynamicProperties);//el guion fue modificado
                    }
                } else {
                    if (window[this.WindowName].PostScript !== undefined && window[this.WindowName].PostScript !== null) {
                        window[this.WindowName].PostScript(true, this.DynamicProperties);//el guion fue modificado
                    }
                }

                $("#ModalExecuteScript").UifModal("hide");
            });
        } else {
            if (this.ClassName !== undefined && this.ClassName !== null) {
                if (this.ClassName.PostScript !== undefined && this.ClassName.PostScript !== null) {
                    this.ClassName.PostScript(false, this.DynamicProperties);//el guion no fue modificado
                }
            } else {
                if (window[this.WindowName].PostScript !== undefined && window[this.WindowName].PostScript !== null) {
                    window[this.WindowName].PostScript(false, this.DynamicProperties);//el guion no fue modificado
                }
            }
        }
    }

    OnChange(e) {
        const questionId = $(e.currentTarget).attr("id");
        const question = this.Questions.find(x => { return x.QuestionId == questionId });

        if (question !== undefined && question !== null) {

            if ($(`#${questionId}`).hasClass("uif-select")) {
                $(`#${questionId}`).UifSelect("setSelected", $(`#${questionId}`).val());
            }

            if (question.Edges.length !== 0) {
                let value = $(e.currentTarget).val();

                question.Edges.forEach(edge => {
                    $(`#edge_${questionId}_${edge.EdgeId}`).addClass("collapse");
                    if (value !== "" && edge.ValueCode == value) {
                        $(`#edge_${questionId}_${edge.EdgeId}`).removeClass("collapse");
                        $.each($(`#edge_${questionId}_${edge.EdgeId} .inputDinamic`), (index, x) => { $(x).val("") });
                    }
                });
            }
        }
    }

    Validate() {
        let ids = $(".inputDinamic");
        let isValid = true;

        for (let x = 0; x < ids.length; x++) {
            switch (ids[x].type) {
                case "select-one":
                case "text":
                case "date":
                case "number":
                    if ($(`#${$(ids[x]).attr("id")}`).is(":visible") === true) {
                        if ($(`#${$(ids[x]).attr("id")}`).val() !== "") {
                            $(`#${$(ids[x]).attr("id")}`).closest(".panel-heading").removeClass("has-error");
                        }
                        else {
                            $(`#${$(ids[x]).attr("id")}`).closest(".panel-heading").addClass("has-error");
                            isValid = false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        if (!isValid) {
            $.UifNotify("show", { 'type': "danger", 'message': Resources.Language.AnswerAllQuestions, 'autoclose': true });
        }
        return isValid;
    }

    DrawNode(nodes, node) {
        const drawQuestions = [];

        node.Questions.sort((a, b) => {
            return a.OrdenNum > b.OrdenNum ? 1 : 0;
        }).forEach(question => {
            drawQuestions.push(this.DrawQuestion(nodes, node, question));
            this.Questions.push(question);
        });

        return Promise.all(drawQuestions).then(data => {
            return `<div id="node_${node.NodeId}" class="panel-group" role="tablist" aria-multiselectable="true"> ${data.join(" ")} </div>`;
        });
    }

    DrawQuestion(nodes, node, question) {
        return SearchCombo.obtencionTipoControl(question.ConceptId, question.EntityId, "").then(data => {
            let formulario = '<div class="panel panel-default">';
            formulario += `<div class="panel-heading" role="tab" id="heading${question.QuestionId}" style="color:#337ab7;">`;
            formulario += `<h4 class="panel-title">${question.Description}</h4>`;

            switch (data.ConceptControlCode) {
                case 1:
                    formulario += `<input type="text" class="form-control inputDinamic" data-entity="${question.EntityId}" data-concept="${question.ConceptId}" id="${question.QuestionId}">`;
                    break;
                case 2:
                    formulario += `<input type="number" class="form-control inputDinamic" data-entity="${question.EntityId}" data-concept="${question.ConceptId}" id="${question.QuestionId}">`;
                    break;
                case 7:
                    formulario += `<input type="text" class="form-control inputDinamic" data-entity="${question.EntityId}" data-concept="${question.ConceptId}" id="${question.QuestionId}">`;
                    break;
                case 3:
                    formulario += `<input type="text" class="uif-datepicker form-control inputDinamic" data-entity="${question.EntityId}" data-concept="${question.ConceptId}" id="${question.QuestionId}">`;
                    break;
                case 4:
                case 5:
                    formulario += `<select class="uif-select form-control inputDinamic" data-entity="${question.EntityId}" data-concept="${question.ConceptId}" id="${question.QuestionId}"></select>`;
                    break;
                default:
                    break;
            }
            formulario += "</div>";


            if (question.Edges.length !== 0) {
                const drawEdges = [];
                question.Edges.forEach((edge) => {
                    const nextNode = nodes.find((x) => { return x.NodeId === edge.NextNodeId });

                    if (nextNode !== undefined) {
                        drawEdges.push(this.DrawNode(nodes, nextNode).then(resolve => {
                            let formEdge = "";
                            formEdge += `<div id="edge_${question.QuestionId}_${edge.EdgeId}" role="tabpanel" class="panel-collapse collapse" aria-labelledby="heading${question.QuestionId}">`;
                            formEdge += '<div class="panel-body">';
                            formEdge += resolve;
                            formEdge += "</div>";
                            formEdge += "</div>";
                            return formEdge;
                        }));
                    }
                });

                if (drawEdges.length > 0) {
                    return Promise.all(drawEdges).then((resolve) => {
                        return formulario + resolve.join(" ") + "</div>";
                    });
                }
            }

            formulario += "</div>";
            return formulario;
        });
    }

    OpenModal(title) {
        $("#ModalExecuteScript").UifModal("showLocal", title);
    }

    
}