using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using TM = System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    using Entities;
    using Framework.Contexts;
    using Framework.Transactions;
    using Sistran.Core.Application.Utilities.DataFacade;

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ScriptsServiceEEProvider : IScriptsService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Models.ConceptControl GetConceptControl(int conceptId, int entityId)
        {
            try
            {
                return RuleSetDAO.GetConceptControl(conceptId, entityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptControl", ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string GetDataFromFilter(int entityId, List<ConditionFilter> filter)
        {
            try
            {
                return RuleSetDAO.GetDataFromFilter(entityId, filter);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDataFromFilter", ex);
            }

        }

        /// <summary>
        /// obtiene todos los guiones 
        /// </summary>
        /// <returns></returns>
        public List<Models.Script> GetScripts()
        {
            try
            {
                return ScriptDAO.GetScriptList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScripts", ex);
            }

        }

        /// <summary>
        /// obtiene todas las preguntas por nivel
        /// </summary>
        /// <param name="levels"></param>
        /// <returns></returns>
        public List<Models.Question> GetQuestionsByLevel(Enums.Level levels)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.OpenParenthesis();
                switch (levels)
                {
                    case Enums.Level.General:
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 83);
                        break;
                    case Enums.Level.Coverage:
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 83);
                        filter.Or();
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 84);
                        filter.Or();
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 85);
                        break;
                    case Enums.Level.Component:
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 83);
                        filter.Or();
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 86);
                        break;
                    case Enums.Level.Commission:
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 83);
                        filter.Or();
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 87);
                        break;
                    case Enums.Level.Risk:
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 83);
                        filter.Or();
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 85);
                        break;
                    default:
                        break;
                }

                filter.CloseParenthesis();

                BusinessCollection Question = QuestionDAO.ListQuestions(filter.GetPredicate(), null);
                return ModelAssembler.CreateQuestions(Question);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetQuestionsByLevel", ex);
            }

        }

        /// <summary>
        /// obtiene todas las preguntas
        /// </summary>
        /// <returns></returns>
        public List<Models.Question> GetQuestions()
        {
            try
            {
                BusinessCollection Question = QuestionDAO.ListQuestions(null, null);
                return ModelAssembler.CreateQuestions(Question);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetQuestions", ex);
            }

        }

        /// <summary>
        /// Obtiene los datos de la pregunta
        /// </summary>
        /// <param name="IdQuestion">id de la pregunta</param>
        /// <returns></returns>
        public Models.Question GetQuestion(int IdQuestion)
        {
            try
            {
                Entities.Question Question = QuestionDAO.FindQuestion(IdQuestion);
                return ModelAssembler.CreateQuestion(Question);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetQuestion", ex);
            }
        }

        /// <summary>
        /// obtiene las pregutnas y respuestas asociadas al guion
        /// </summary>
        /// <param name="ScriptId">id del guion </param>
        /// <returns></returns>
        public ScriptComposite GetScriptComposite(int ScriptId)
        {
            try
            {
                List<TM.Task> task = new List<TM.Task>();
                Entities.Script scriptEN = null;
                BusinessCollection NodeEN = null;
                ScriptComposite scriptComposite = new ScriptComposite();
                #region ObtenerDao
                task.Add(TP.Task.Run(() =>
                {
                    try
                    {
                        scriptEN = ScriptDAO.FindScript(ScriptId);
                    }
                    finally
                    {
                        DataFacadeManager.Dispose();
                    }
                }));
                task.Add(TP.Task.Run(() =>
                {
                    try
                    {
                        ObjectCriteriaBuilder filterNode = new ObjectCriteriaBuilder();
                        filterNode.PropertyEquals(Entities.Node.Properties.ScriptId, ScriptId);
                        NodeEN = NodeDAO.ListNode(filterNode.GetPredicate(), null);
                    }
                    finally
                    {
                        DataFacadeManager.Dispose();
                    }
                }));
                TM.Task.WaitAll(task.ToArray());

                #endregion
                //Obtenemos todass preguntas
                List<Models.Question> QuestionList = GetQuestions();
                //Obtenemos el guion
                scriptComposite.Script = ModelAssembler.CreateScript(scriptEN);

                List<Models.Node> Nodes = ModelAssembler.CreateNodes(NodeEN);

                scriptComposite.Nodes = new List<Models.Node>();
                //scriptComposite.Nodes.AddRange(Nodes);
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                for (int i = 0; i < Nodes.Count; i++)
                {
                    scriptComposite.Nodes.Add(Nodes[i]);
                    //obtenemos las preguntas
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(Entities.NodeQuestion.Properties.NodeId, Nodes[i].NodeId);
                    filter.And();
                    filter.PropertyEquals(Entities.NodeQuestion.Properties.ScriptId, ScriptId);

                    List<Models.NodeQuestion> NodeQuestions = ModelAssembler.CreateNodeQuestions(NodeQuestionDAO.ListNodeQuestion(filter.GetPredicate(), new string[] { Entities.NodeQuestion.Properties.OrderNum }));

                    scriptComposite.Nodes[i].Questions = new List<Models.Question>();

                    for (int j = 0; j < NodeQuestions.Count; j++)
                    {
                        Models.Question Questions = QuestionList.Where(x => x.QuestionId == NodeQuestions[j].QuestionId).FirstOrDefault();
                        Questions.OrdenNum = NodeQuestions[j].OrderNum;
                        scriptComposite.Nodes[i].Questions.Add(Questions);

                        #region Edges
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(Entities.Edge.Properties.NodeId, NodeQuestions[j].NodeId);
                        filter.And();
                        filter.PropertyEquals(Entities.Edge.Properties.ScriptId, NodeQuestions[j].ScriptId);
                        filter.And();
                        filter.PropertyEquals(Entities.Edge.Properties.QuestionId, NodeQuestions[j].QuestionId);
                        List<Models.Edge> Edges = ModelAssembler.CreateEdges(EdgeDAO.ListEdge(filter.GetPredicate(), null));

                        scriptComposite.Nodes[i].Questions[j].Edges = new List<Models.Edge>();
                        scriptComposite.Nodes[i].Questions[j].Edges = Edges;
                        #endregion

                        //sacar el concepto de la pregunta
                        string concept = null;
                        Models.ConceptControl conceptControl = GetConceptControl(Questions.ConceptId, Questions.EntityId);
                        #region datos Concepto
                        switch (conceptControl.ConceptControlCode)
                        {
                            case 4://lista       
                                if (conceptControl is Models.RangeControl)
                                {
                                    concept = JsonConvert.SerializeObject(((Models.RangeControl)conceptControl).ListRangeEntityValues);
                                }
                                else
                                {
                                    concept = JsonConvert.SerializeObject(((Models.ListBoxControl)conceptControl).ListListEntityValues);
                                }
                                break;
                            case 5://refetencia
                                concept = GetDataFromFilter(((Models.SearchComboControl)conceptControl).ForeignEntity, null);
                                break;
                        }
                        #endregion

                        #region Edge_Answer
                        for (int k = 0; k < Edges.Count; k++)
                        {
                            if (EdgeAnswerDAO.FindEdgeAnswer(Edges[k].EdgeId, scriptComposite.Nodes[i].Questions[j].ConceptId, scriptComposite.Nodes[i].Questions[j].EntityId) != null)
                            {
                                Models.EdgeAnswer EdgeAnswer = ModelAssembler.CreateEdgeAnswer(EdgeAnswerDAO.FindEdgeAnswer(Edges[k].EdgeId, scriptComposite.Nodes[i].Questions[j].ConceptId, scriptComposite.Nodes[i].Questions[j].EntityId));

                                switch (conceptControl.ConceptControlCode)
                                {
                                    case 4://lista
                                        if (concept.Contains("RangeEntityCode"))
                                        {
                                            var RangeValue = JArray.Parse(concept).Where(s => s["RangeValueCode"].Value<string>() == (EdgeAnswer.ValueCode.ToString())).ToList()[0];
                                            scriptComposite.Nodes[i].Questions[j].Edges[k].Description = RangeValue["FromValue"] + " - " + RangeValue["ToValue"];
                                            scriptComposite.Nodes[i].Questions[j].Edges[k].ValueCode = EdgeAnswer.ValueCode;
                                        }
                                        else if (concept.Contains("ListValueCode"))
                                        {
                                            var ListValue = JArray.Parse(concept).Where(s => s["ListValueCode"].Value<string>() == (EdgeAnswer.ValueCode.ToString())).ToList()[0]["ListValue"];
                                            scriptComposite.Nodes[i].Questions[j].Edges[k].Description = ListValue.ToString();
                                            scriptComposite.Nodes[i].Questions[j].Edges[k].ValueCode = EdgeAnswer.ValueCode;
                                        }
                                        break;
                                    case 5://refetencia
                                        var Descripcion = JArray.Parse(concept).Where(s => s["Id"].Value<string>() == (EdgeAnswer.ValueCode.ToString())).ToList()[0]["Descripción"];
                                        scriptComposite.Nodes[i].Questions[j].Edges[k].Description = Descripcion.ToString();
                                        scriptComposite.Nodes[i].Questions[j].Edges[k].ValueCode = EdgeAnswer.ValueCode;
                                        break;
                                    default:
                                        scriptComposite.Nodes[i].Questions[j].Edges[k].Description = EdgeAnswer.ValueCode.ToString();
                                        break;
                                }
                            }
                        }
                        #endregion
                    }
                }
                return scriptComposite;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScriptComposite", ex);
            }

        }

        /// <summary>
        /// Crea un guion completo
        /// </summary>
        /// <param name="ScriptComposite">compone todo un guion com preguntas y respuestas</param>
        /// <returns></returns>
        public ScriptComposite CreateScriptComposite(ScriptComposite ScriptComposite)
        {
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        Script script;
                        if (ScriptComposite.Script.ScriptId == 0)
                        {
                            //grabo nuevo				
                            //grabo el encabezado
                            script = new Entities.Script();
                            script.Description = ScriptComposite.Script.Description;
                            script.LevelId = ScriptComposite.Script.LevelId;
                            script.PackageId = ScriptComposite.Script.PackageId;

                            ScriptDAO.CreateScript(script);
                            ScriptDAO.CreateScript(ScriptComposite, script);

                        }
                        else
                        {
                            //edito la encabezado
                            script = ScriptDAO.FindScript(ScriptComposite.Script.ScriptId);
                            script.Description = ScriptComposite.Script.Description;
                            script.LevelId = ScriptComposite.Script.LevelId;
                            script.PackageId = ScriptComposite.Script.PackageId;
                            ScriptDAO.UpdateScript(script);
                            ScriptDAO.CreateScript(ScriptComposite, script);
                        }

                        transaction.Complete();
                        return this.GetScriptComposite(script.ScriptId);
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw new BusinessException("Error Create ScriptComposite", ex);
                    }
                }
            }
        }

        /// <summary>
        /// obtiene los guiones por nivel 
        /// </summary>
        /// <param name="levelId">id del nivel</param>
        /// <returns></returns>
        public List<Models.Script> GetScriptByLevelId(int? module, int? level, string Name, string Question)
        {
            try
            {
                return ScriptDAO.GetScriptByLevelId(module, level, Name, Question);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScriptByLevelId", ex);
            }

        }

        /// <summary>
        /// obtiene los giones por diferentes niveles
        /// </summary>
        /// <param name="level">listado de niveles</param>
        /// <returns></returns>
        public List<Models.Script> GetScriptsByLevels(List<Models.Level> level)
        {
            try
            {
                return ScriptDAO.GetScriptsByLevels(level);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScriptsByLevels", ex);
            }

        }

        /// <summary>
        /// Elimina un guion 
        /// </summary>
        /// <param name="ScriptId"></param>
        public void DeleteScript(int ScriptId)
        {
            try
            {
                var script = ScriptDAO.FindScript(ScriptId);
                ScriptDAO.DeleteScript(script);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error DeleteScript", ex);
            }

        }

        public List<Models.Script> GetScriptsByIds(List<int> ids)
        {
            try
            {
                return ScriptDAO.GetScriptsByIds(ids);
            }
            catch (Exception ex)
            {

                throw new BusinessException("Error GetScriptsByIds", ex);
            }

        }

        /// <summary>
        /// Genera el reporte de guiones
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns></returns>
        public string GenerateScriptsReport(string fileName)
        {
            try
            {
                return ScriptDAO.GenerateScriptsReport(fileName);
            }
            catch (Exception ex)
            {

                throw new BusinessException("Error GenerateScriptsReport", ex);
            }

        }

        /// <summary>
        /// obtiene todas las preguntas por nivel
        /// </summary>
        /// <param name="levels"></param>
        /// <returns></returns>
        public List<Models.Question> GetQuestionsByLevelAutomaticQuota(Enums.LevelAutomaticQuota levels)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.OpenParenthesis();
                switch (levels)
                {
                    case Enums.LevelAutomaticQuota.General:
                        filter.PropertyEquals(Entities.Question.Properties.EntityId, 724);
                        break;
                    default:
                        break;
                }

                filter.CloseParenthesis();

                BusinessCollection Question = QuestionDAO.ListQuestions(filter.GetPredicate(), null);
                return ModelAssembler.CreateQuestions(Question);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetQuestionsByLevel", ex);
            }

        }
    }
}
