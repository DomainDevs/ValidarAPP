using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using COMMO = Sistran.Core.Application.CommonService.Models;
using RuleSetmodels = Sistran.Core.Application.RulesScriptsServices.Models;
using UTMO = Sistran.Core.Application.Utilities.Error;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class ScriptDAO
    {
        /// <summary>
        /// crea un Script
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static Entities.Script CreateScript(Entities.Script Script)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(Script);
                return Script;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateScript", ex);
            }

        }

        /// <summary>
        /// edita un Script
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static Entities.Script UpdateScript(Entities.Script Script)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(Script);
                return Script;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateScript", ex);
            }

        }

        /// <summary>
        /// elimina un Script
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static void DeleteScript(Entities.Script Script)
        {
            Transaction.Created += delegate (object sender, TransactionEventArgs e) { };
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e) { };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e) { };
                    try
                    {
                        Script.NodeId = null;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(Script);


                        var filter = new ObjectCriteriaBuilder().Property(Edge.Properties.ScriptId).Equal().Constant(Script.ScriptId);
                        var listEdge = EdgeDAO.ListEdge(filter.GetPredicate(), new string[] { Edge.Properties.EdgeId });
                        foreach (Edge edge in listEdge)
                        {
                            edge.NextNodeId = null;
                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(edge);
                        }

                        foreach (Edge edge in listEdge)
                        {
                            filter = new ObjectCriteriaBuilder().Property(EdgeAnswer.Properties.EdgeId).Equal().Constant(edge.EdgeId);
                            var ListEdgeAnswer = EdgeAnswerDAO.ListEdgeAnswer(filter.GetPredicate(), new string[] { EdgeAnswer.Properties.EdgeId });

                            foreach (EdgeAnswer edgeAnswer in ListEdgeAnswer)
                            {
                                EdgeAnswerDAO.DeleteEdgeAnswer(edgeAnswer);
                            }

                            EdgeDAO.DeleteEdge(edge);
                        }

                        filter = new ObjectCriteriaBuilder().Property(NodeQuestion.Properties.ScriptId).Equal().Constant(Script.ScriptId);
                        var ListNodeQuestion = NodeQuestionDAO.ListNodeQuestion(filter.GetPredicate(), new string[] { NodeQuestion.Properties.NodeId });
                        foreach (NodeQuestion nodeQuestion in ListNodeQuestion)
                        {
                            NodeQuestionDAO.DeleteNodeQuestion(nodeQuestion);
                        }

                        filter = new ObjectCriteriaBuilder().Property(Node.Properties.ScriptId).Equal().Constant(Script.ScriptId);
                        var ListNode = NodeDAO.ListNode(filter.GetPredicate(), new string[] { Node.Properties.NodeId });
                        foreach (Node node in ListNode)
                        {
                            NodeDAO.DeleteNode(node);
                        }

                        filter = new ObjectCriteriaBuilder().Property(Entities.Script.Properties.ScriptId).Equal().Constant(Script.ScriptId);
                        var ListScript = ScriptDAO.ListScript(filter.GetPredicate(), new string[] { Entities.Script.Properties.ScriptId });
                        foreach (Entities.Script script in ListScript)
                        {
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(script);
                        }

                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw new BusinessException("Error Obtener DeleteScript", ex);
                    }
                }
            }
        }

        /// <summary>
        /// obtien un  Script a partir de ScriptId
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static Entities.Script FindScript(int ScriptId)
        {
            try
            {
                Entities.Script Script = null;
                PrimaryKey key = Entities.Script.CreatePrimaryKey(ScriptId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    Script = (Entities.Script)daf.GetObjectByPrimaryKey(key);
                }

                return Script;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindScript", ex);
            }

        }

        /// <summary>
        /// obtiene un alista de Script a partir del filtro
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static BusinessCollection ListScript(Predicate filter, string[] sort)
        {
            try
            {
                BusinessCollection businessColletion = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessColletion = new BusinessCollection(daf.SelectObjects(typeof(Entities.Script), filter, sort));
                }
                return businessColletion;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListScript", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de Script 
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.Script> GetScriptList()
        {
            try
            {
                List<RuleSetmodels.Script> scriptList = new List<RuleSetmodels.Script>();

                #region select
                SelectQuery select = new SelectQuery();

                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.ScriptId, "Script"), "ScriptId"));
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.Description, "Script"), "Description"));
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.LevelId, "Script"), "LevelId"));
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.PackageId, "Script"), "PackageId"));

                select.AddSelectValue(new SelectValue(new Column(Level.Properties.Description, "Level"), "LevelDescription"));

                #endregion

                #region join
                Join join = new Join(new ClassNameTable(typeof(Entities.Script), "Script"),
                    new ClassNameTable(typeof(Level), "Level"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(Entities.Script.Properties.LevelId, "Script")
                    .Equal()
                    .Property(Level.Properties.LevelId, "Level")
                    .And()
                    .Property(Entities.Script.Properties.PackageId, "Script")
                    .Equal()
                    .Property(Level.Properties.PackageId, "Level")
                    .GetPredicate());
                #endregion

                #region filter
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Constant(1);
                filter.Equal();
                filter.Constant(1);
                #endregion

                select.Table = join;
                select.Where = filter.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    RuleSetmodels.Script scriptDTO = null;
                    while (reader.Read())
                    {
                        scriptDTO = new RuleSetmodels.Script();
                        scriptDTO.ScriptId = (int)reader["ScriptId"];
                        scriptDTO.Description = (string)reader["Description"];
                        scriptDTO.PackageId = (int)reader["PackageId"];
                        scriptDTO.LevelId = (int)reader["LevelId"];
                        scriptDTO.LevelDescription = (string)reader["LevelDescription"];

                        scriptList.Add(scriptDTO);
                    }
                }
                return scriptList;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScriptList", ex);
            }

        }

        /// <summary>
        /// obtiene un alista de Script a partir del levelId
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.Script> GetScriptByLevelId(int? module, int? level, string Name, string question)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.ScriptId, "sc")));
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.Description, "sc"), "ScriptDescription"));
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.PackageId, "sc")));
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.LevelId, "sc")));
                select.AddSelectValue(new SelectValue(new Column(Entities.Script.Properties.NodeId, "sc")));
                select.AddSelectValue(new SelectValue(new Column(Question.Properties.QuestionId, "q")));
                select.AddSelectValue(new SelectValue(new Column(Question.Properties.Description, "q"), "QuestionDescription"));
                select.AddSelectValue(new SelectValue(new Column(Package.Properties.Description, "p"), "PackageDescription"));
                select.AddSelectValue(new SelectValue(new Column(Level.Properties.Description, "l"), "LevelDescription"));

                Join join = new Join(new ClassNameTable(typeof(Entities.Script), "sc"), new ClassNameTable(typeof(NodeQuestion), "nq"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(Entities.Script.Properties.ScriptId, "sc").Equal().Property(NodeQuestion.Properties.ScriptId, "nq").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(Question), "q"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(Question.Properties.QuestionId, "q").Equal().Property(NodeQuestion.Properties.QuestionId, "nq").GetPredicate());

                join = join = new Join(join, new ClassNameTable(typeof(Package), "p"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(Entities.Script.Properties.PackageId, "sc").Equal().Property(Package.Properties.PackageId, "p").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(Level), "l"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(Entities.Script.Properties.LevelId, "sc").Equal().Property(Level.Properties.LevelId, "l").GetPredicate());


                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(Entities.Script.Properties.ScriptId, "sc").IsNotNull();
                if (module.HasValue)
                {
                    where.And().Property(Entities.Script.Properties.PackageId, "sc").Equal().Constant(module);
                }
                if (level.HasValue)
                {
                    where.And().Property(Entities.Script.Properties.LevelId, "sc").Equal().Constant(level);
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    where.And().Property(Entities.Script.Properties.Description, "sc").Like().Constant("%" + Name + "%");
                }
                if (!string.IsNullOrEmpty(question))
                {
                    where.And().Property(Question.Properties.Description, "q").Like().Constant("%" + question + "%");
                }

                select.Table = join;
                select.Where = where.GetPredicate();

                List<RuleSetmodels.Script> ListScript = new List<RuleSetmodels.Script>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        int IdScript = (int)reader["ScriptId"];
                        if (ListScript.Count(x => x.ScriptId == IdScript) == 0)
                        {
                            ListScript.Add(new RuleSetmodels.Script()
                            {
                                ScriptId = (int)reader["ScriptId"],
                                Description = (string)reader["ScriptDescription"],
                                PackageId = (int)reader["PackageId"],
                                LevelId = (int)reader["LevelId"],
                                NodeId = (int)reader["NodeId"],
                                LevelDescription = (string)reader["LevelDescription"],
                                PackageDescription = (string)reader["PackageDescription"],
                            });
                        }
                    }
                }
                return ListScript.OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScriptByLevelId", ex);
            }

        }

        internal static void CreateScript(RuleSetmodels.ScriptComposite scriptComposite, Entities.Script script)
        {
            script.NodeId = null;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(script);

            #region Elimina todo el guion
            var filter = new ObjectCriteriaBuilder().Property(Edge.Properties.ScriptId).Equal().Constant(script.ScriptId);
            var listEdge = EdgeDAO.ListEdge(filter.GetPredicate(), new[] { Edge.Properties.EdgeId });

            foreach (Edge edge in listEdge)
            {
                filter = new ObjectCriteriaBuilder().Property(EdgeAnswer.Properties.EdgeId).Equal().Constant(edge.EdgeId);
                var ListEdgeAnswer = EdgeAnswerDAO.ListEdgeAnswer(filter.GetPredicate(), new[] { EdgeAnswer.Properties.EdgeId });

                foreach (EdgeAnswer edgeAnswer in ListEdgeAnswer)
                {
                    EdgeAnswerDAO.DeleteEdgeAnswer(edgeAnswer);
                }

                EdgeDAO.DeleteEdge(edge);
            }

            filter = new ObjectCriteriaBuilder().Property(NodeQuestion.Properties.ScriptId).Equal().Constant(script.ScriptId);
            var ListNodeQuestion = NodeQuestionDAO.ListNodeQuestion(filter.GetPredicate(), new[] { NodeQuestion.Properties.NodeId });
            foreach (NodeQuestion nodeQuestion in ListNodeQuestion)
            {
                NodeQuestionDAO.DeleteNodeQuestion(nodeQuestion);
            }

            filter = new ObjectCriteriaBuilder().Property(Node.Properties.ScriptId).Equal().Constant(script.ScriptId);
            var ListNode = NodeDAO.ListNode(filter.GetPredicate(), new[] { Node.Properties.NodeId });
            foreach (Node node in ListNode)
            {
                NodeDAO.DeleteNode(node);
            }
            #endregion

            scriptComposite.Nodes = scriptComposite.Nodes.Where(x => x.Questions != null && x.Questions.Count > 0).OrderBy(x => x.NodeId).ToList();
            for (int nodeIndex = 1; nodeIndex <= scriptComposite.Nodes.Count; nodeIndex++)
            {
                RuleSetmodels.Node node = scriptComposite.Nodes[nodeIndex - 1];
                Node eNode = new Node(nodeIndex, script.ScriptId);
                eNode = NodeDAO.CreateNode(eNode);
                scriptComposite.Nodes[nodeIndex - 1].NodeId = eNode.NodeId;

                node.Questions = node.Questions.OrderBy(x => x.OrdenNum).ToList();
                for (int questionIndex = 1; questionIndex <= node.Questions.Count; questionIndex++)
                {
                    RuleSetmodels.Question question = node.Questions[questionIndex - 1];
                    NodeQuestion eNodeQuestion = new NodeQuestion(eNode.NodeId, question.QuestionId, script.ScriptId)
                    {
                        OrderNum = questionIndex
                    };
                    NodeQuestionDAO.CreateNodeQuestion(eNodeQuestion);

                    if (question.Edges != null)
                    {
                        for (int edgeIndex = 1; edgeIndex <= question.Edges.Count; edgeIndex++)
                        {
                            RuleSetmodels.Edge edge = question.Edges[edgeIndex - 1];

                            if (edge.NextNodeId.HasValue)
                            {
                                int? nextNode = scriptComposite.Nodes[nodeIndex - 1].Questions[questionIndex - 1].Edges[edgeIndex - 1].NextNodeId;
                                nextNode = scriptComposite.Nodes.Select((item, index) => new { item, index }).First(x => x.item.NodeId == nextNode).index + 1;
                                scriptComposite.Nodes[nodeIndex - 1].Questions[questionIndex - 1].Edges[edgeIndex - 1].NextNodeId = nextNode;
                            }
                        }
                    }
                }
            }

            for (int nodeIndex = 1; nodeIndex <= scriptComposite.Nodes.Count; nodeIndex++)
            {
                RuleSetmodels.Node node = scriptComposite.Nodes[nodeIndex - 1];
                for (int questionIndex = 1; questionIndex <= node.Questions.Count; questionIndex++)
                {
                    RuleSetmodels.Question question = node.Questions[questionIndex - 1];
                    if (question.Edges != null)
                    {
                        for (int edgeIndex = 1; edgeIndex <= question.Edges.Count; edgeIndex++)
                        {
                            RuleSetmodels.Edge edge = question.Edges[edgeIndex - 1];

                            if (edge.NextNodeId.HasValue)
                            {
                                Edge eEdge = new Edge
                                {
                                    NodeId = node.NodeId,
                                    QuestionId = question.QuestionId,
                                    ScriptId = script.ScriptId,
                                    IsDefault = false,
                                    NextNodeId = edge.NextNodeId.Value
                                };
                                eEdge = EdgeDAO.CreateEdge(eEdge);

                                EdgeAnswer eEdgeAnswer = new EdgeAnswer(eEdge.EdgeId, question.ConceptId, question.EntityId)
                                {
                                    ValueCode = edge.ValueCode
                                };

                                EdgeAnswerDAO.CreateEdgeAnswer(eEdgeAnswer);
                            }
                        }
                    }
                }
            }

            script.NodeId = 1;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(script);
        }

        /// <summary>
        /// obtiene un alista de Script a partir de una lista de niveles
        /// </summary>
        /// <param name="Script"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.Script> GetScriptsByLevels(List<RuleSetmodels.Level> level)
        {
            try
            {
                Predicate filter = null;
                ObjectCriteriaBuilder Objectfilter = null;
                if (level != null && level.Count > 0)
                {
                    Objectfilter = new ObjectCriteriaBuilder();
                    Objectfilter.Property(Entities.Script.Properties.LevelId);
                    Objectfilter.In();
                    Objectfilter.ListValue();
                    foreach (RuleSetmodels.Level item in level)
                    {
                        Objectfilter.Constant(item.LevelId);
                    }
                    Objectfilter.EndList();
                    filter = Objectfilter.GetPredicate();
                }
                else
                {
                    filter = null;
                }
                return ModelAssembler.CreateScripts(ListScript(filter, null));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScriptsByLevels", ex);
            }

        }
        public static List<RuleSetmodels.Script> GetScriptsByIds(List<int> ids)
        {
            try
            {
                Predicate filter = null;
                ObjectCriteriaBuilder Objectfilter = null;
                if (ids != null && ids.Count > 0)
                {
                    Objectfilter = new ObjectCriteriaBuilder();
                    Objectfilter.Property(Entities.Script.Properties.ScriptId);
                    Objectfilter.In();
                    Objectfilter.ListValue();
                    foreach (int item in ids)
                    {
                        Objectfilter.Constant(item);
                    }
                    Objectfilter.EndList();
                    filter = Objectfilter.GetPredicate();
                }
                else
                {
                    filter = null;
                }
                return ModelAssembler.CreateScripts(ListScript(filter, null));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetScriptsByIds", ex);
            }

        }

        /// <summary>
        /// Genera el archivo excel de guiones
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Ruta del archivo</returns>
        public static string GenerateScriptsReport(string fileName)
        {
            List<RuleSetmodels.Script> scripts = GetScriptList();

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ScriptReport;

            FileDAO fileDAO = new FileDAO();
            File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (RuleSetmodels.Script script in scripts)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = script.ScriptId.ToString();
                    fields[1].Value = script.Description;
                    fields[2].Value = script.LevelDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;


                file.Name = string.Format(fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy"));
                return fileDAO.GenerateFile(file);
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
