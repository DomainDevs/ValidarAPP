using Sistran.Co.Application.Data;
//using Sistran.Core.Application.Cache;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Helper;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using EnParam = Sistran.Core.Application.Parameters.Entities;
using EnRules = Sistran.Core.Application.Script.Entities;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using USMOD = Sistran.Core.Services.UtilitiesServices.Models;


namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class _RuleSetDao
    {
        /// <summary>
        /// Listado de reglas 
        /// </summary>
        /// <returns></returns>
        public List<MRules._RuleSet> GetRuleSets()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleSet.Properties.IsEvent, typeof(EnRules.RuleSet).Name).Equal().Constant(0);
            filter.And().Not().Property(EnRules.RuleSet.Properties.Description, typeof(EnRules.RuleSet).Name).Like().Constant("DT%");
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EnRules.RuleSet), filter.GetPredicate()));
            return _ModelAssembler.CreateListRuleSet(businessCollection.Cast<EnRules.RuleSet>().ToList());
        }

        /// <summary>
        /// Obtiene los paquetes de reglas por el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id los niveles a buscar</param>
        /// <param name="withDecisionTable">incluir tablas de decision</param>
        /// <param name="isPolicie">es una politica</param>
        /// <param name="filter">like para la descripcion</param>        
        /// <returns></returns>
        public List<MRules._RuleSet> GetRulesByFilter(int idPackage, List<int> levels, bool withDecisionTable, bool isPolicie, string filter, bool maxRows)
        {
            SelectQuery select = new SelectQuery();
            if (maxRows)
            {
                select.MaxRows = Convert.ToInt32(50);
            }

            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.RuleSetId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.Description, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.CurrentFrom, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.CurrentTo, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.RuleSetVer, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.IsEvent, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.Active, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.TypeActive, "r")));


            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.PackageId, "p")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PDescription"));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.LevelId, "l")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.Description, "l"), "LDescription"));

            Join join = new Join(new ClassNameTable(typeof(EnRules.RuleSet), "r"), new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleSet.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(EnParam.Levels), "l"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleSet.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Levels.Properties.PackageId, "l")

                    .And().Property(EnRules.RuleSet.Properties.LevelId, "r")
                    .Equal().Property(EnParam.Levels.Properties.LevelId, "l")
                    .GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

            where.Property(EnRules.RuleSet.Properties.PackageId, "r").Equal().Constant(idPackage);
            where.And().Property(EnRules.RuleSet.Properties.IsEvent, "r").Equal().Constant(isPolicie);

            if (levels != null && levels.Count != 0)
            {
                where.And().Property(EnRules.RuleSet.Properties.LevelId, "r").In()
                    .ListValue();
                levels.ForEach(x => where.Constant(x));
                where.EndList();
            }
            if (!withDecisionTable)
            {
                where.And().Not().Property(EnRules.RuleSet.Properties.Description, "r").Like().Constant("DT%");
            }
            if (!string.IsNullOrEmpty(filter))
            {
                int ruleSetId;
                if (!Int32.TryParse(filter, out ruleSetId))
                {
                    where.And().Property(EnRules.RuleSet.Properties.Description, "r").Like()
                        .Constant("%" + filter + "%");
                }
                else
                {
                    where.And().Property(EnRules.RuleSet.Properties.RuleSetId, "r").Equal().Constant(ruleSetId);
                }
            }

            select.Table = join;
            select.Where = where.GetPredicate();

            List<MRules._RuleSet> result = new List<MRules._RuleSet>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    var a = reader["CurrentTo"];
                    result.Add(new MRules._RuleSet
                    {
                        RuleSetId = (int)reader["RuleSetId"],
                        Description = (string)reader["Description"],
                        CurrentFrom = (DateTime)reader["CurrentFrom"],
                        CurrentTo = reader["CurrentTo"] == null ? (DateTime?)null : (DateTime)reader["CurrentTo"],
                        IsEvent = (bool)reader["IsEvent"],
                        Type = Enums.RuleBaseType.Sequence,
                        RuleSetVer = (int)reader["RuleSetVer"],
                        Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PDescription"] },
                        Level = new MRules._Level { LevelId = (int)reader["LevelId"], Description = (string)reader["LDescription"] },
                        Rules = new List<MRules._Rule>(),
                        Active = Convert.ToBoolean(reader["Active"]),
                        ActiveType = reader["TypeActive"] == null ? (Utilities.Enums.ActiveRuleSetType?)null : (Utilities.Enums.ActiveRuleSetType)reader["TypeActive"]
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Obtiene los paquetes de reglas para la busqueda avanzada
        /// </summary>
        /// <param name="ruleSet">filtro de regla</param>        
        /// <returns>lista de paquetes de reglas</returns>
        public List<MRules._RuleSet> GetRulesByRuleSet(MRules._RuleSet ruleSet)
        {
            SelectQuery select = new SelectQuery();
            select.MaxRows = Convert.ToInt32(50);

            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.RuleSetId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.Description, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.CurrentFrom, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.CurrentTo, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.RuleSetVer, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.IsEvent, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.Active, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleSet.Properties.TypeActive, "r")));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.PackageId, "p")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PDescription"));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.LevelId, "l")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.Description, "l"), "LDescription"));

            Join join = new Join(new ClassNameTable(typeof(EnRules.RuleSet), "r"), new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleSet.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(EnParam.Levels), "l"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleSet.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Levels.Properties.PackageId, "l")

                    .And().Property(EnRules.RuleSet.Properties.LevelId, "r")
                    .Equal().Property(EnParam.Levels.Properties.LevelId, "l")
                    .GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.RuleSet.Properties.IsEvent, "r").Equal().Constant(0);
            where.And().Not().Property(EnRules.RuleSet.Properties.Description, "r").Like().Constant("DT%");

            if (ruleSet.Package != null && ruleSet.Package.PackageId != 0)
            {
                where.And().Property(EnRules.RuleSet.Properties.PackageId, "r").Equal().Constant(ruleSet.Package.PackageId);
            }

            if (ruleSet.Level != null && ruleSet.Level.LevelId != 0)
            {
                where.And().Property(EnRules.RuleSet.Properties.LevelId, "r").Equal().Constant(ruleSet.Level.LevelId);
            }

            if (!string.IsNullOrEmpty(ruleSet.Description))
            {
                where.And().Property(EnRules.RuleSet.Properties.Description, "r").Like().Constant("%" + ruleSet.Description + "%");
            }

            if (ruleSet.RuleSetId > 0)
            {
                where.And().Property(EnRules.RuleSet.Properties.RuleSetId, "r").Equal().Constant(ruleSet.RuleSetId);
            }

            if (ruleSet.CurrentFrom > DateTime.MinValue)
            {
                where.And().Property(EnRules.RuleSet.Properties.CurrentFrom, "r").GreaterEqual().Constant(Convert.ToDateTime(ruleSet.CurrentFrom).Date);
                where.And().Property(EnRules.RuleSet.Properties.CurrentFrom, "r").Less().Constant(Convert.ToDateTime(ruleSet.CurrentFrom.Date).AddHours(24));
            }

            if (ruleSet.CurrentTo > DateTime.MinValue)
            {
                where.And().Property(EnRules.RuleSet.Properties.CurrentTo, "r").GreaterEqual().Constant(Convert.ToDateTime(ruleSet.CurrentTo).Date);
                where.And().Property(EnRules.RuleSet.Properties.CurrentTo, "r").Less().Constant(Convert.ToDateTime(ruleSet.CurrentTo).AddHours(24));
            }

            select.Table = join;
            select.Where = where.GetPredicate();
            List<MRules._RuleSet> result = new List<MRules._RuleSet>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    var a = reader["CurrentTo"];
                    result.Add(new MRules._RuleSet
                    {
                        RuleSetId = (int)reader["RuleSetId"],
                        Description = (string)reader["Description"],
                        CurrentFrom = (DateTime)reader["CurrentFrom"],
                        CurrentTo = reader["CurrentTo"] == null ? (DateTime?)null : (DateTime)reader["CurrentTo"],
                        IsEvent = (bool)reader["IsEvent"],
                        Type = Enums.RuleBaseType.Sequence,
                        RuleSetVer = (int)reader["RuleSetVer"],
                        Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PDescription"] },
                        Level = new MRules._Level { LevelId = (int)reader["LevelId"], Description = (string)reader["LDescription"] },
                        Rules = new List<MRules._Rule>(),
                        Active = Convert.ToBoolean(reader["Active"]),
                        ActiveType = reader["TypeActive"] == null ? (Utilities.Enums.ActiveRuleSetType?)null : (Utilities.Enums.ActiveRuleSetType)reader["TypeActive"]
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// obtiene los paquetes de reglas que son DT
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        public List<MRules._RuleSet> GetRulesDecisionTable(int idPackage)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.RuleSet.Properties.PackageId, typeof(EnRules.RuleSet).Name).Equal().Constant(idPackage);
            where.And().Property(EnRules.RuleSet.Properties.Description, typeof(EnRules.RuleSet).Name).Like().Constant("DT%");


            List<EnRules.RuleSet> businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(EnRules.RuleSet), where.GetPredicate(), new[] { EnRules.RuleSet.Properties.Description }))
                .Cast<EnRules.RuleSet>().ToList();

            return _ModelAssembler.CreateListRuleSet(businessCollection);
        }

        /// <summary>
        /// Obtiene el paquete de regla completo, con sus respectivas reglas del xml
        /// </summary>
        /// <param name="idRuleSet">id de la regla</param>
        /// <param name="deserializeXml">saber si se deserializa el XML</param>
        /// <returns></returns>
        public MRules._RuleSet GetRuleSetByIdRuleSet(int idRuleSet, bool deserializeXml)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleSet.Properties.RuleSetId, typeof(EnRules.RuleSet).Name).Equal().Constant(idRuleSet);
            EnRules.RuleSet businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnRules.RuleSet), filter.GetPredicate()))
                .Cast<EnRules.RuleSet>().First();
            MRules._RuleSet modelRuleSet = _ModelAssembler.CreateRuleSet(businessCollection);
            if (deserializeXml)
            {
                modelRuleSet.Rules = XmlHelperReader.GetRuleSetByXml(businessCollection.RuleSetXml).Rules;
            }
            modelRuleSet.HasError = modelRuleSet.Rules.Any(b => b.HasError);

            return modelRuleSet;
        }

        /// <summary>
        /// obtiene los tipos de comparadores para la condicion
        /// </summary>
        /// <returns></returns>
        public List<MRules._ComparatorType> GetConditionComparatorType()
        {
            ResourceManager resx = Resources.Errors.ResourceManager;

            return new List<MRules._ComparatorType>
            {
                new MRules._ComparatorType
                {
                    Id = (int)Enums.ComparatorType.ConstantValue,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.ComparatorType), Enums.ComparatorType.ConstantValue))
                },
               new MRules._ComparatorType
               {
                   Id =(int) Enums.ComparatorType.ConceptValue,
                   Description = resx.GetString(Enum.GetName(typeof(Enums.ComparatorType), Enums.ComparatorType.ConceptValue))
                },
               new MRules._ComparatorType
               {
                   Id =(int) Enums.ComparatorType.ExpressionValue,
                   Description =  resx.GetString(Enum.GetName(typeof(Enums.ComparatorType), Enums.ComparatorType.ExpressionValue))
                }
            };
        }

        /// <summary>
        /// obtiene los tipos de comparadores para la accion
        /// </summary>
        /// <returns></returns>
        public List<MRules._ComparatorType> GetActionComparatorType()
        {
            ResourceManager resx = Resources.Errors.ResourceManager;

            return new List<MRules._ComparatorType>
            {
                new MRules._ComparatorType
                {
                    Id = (int)Enums.ComparatorType.ConstantValue,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.ComparatorType), Enums.ComparatorType.ConstantValue))
                },
                new MRules._ComparatorType
                {
                    Id =(int) Enums.ComparatorType.ConceptValue,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.ComparatorType), Enums.ComparatorType.ConceptValue))
                },
                new MRules._ComparatorType
                {
                    Id =(int) Enums.ComparatorType.ExpressionValue,
                    Description =  resx.GetString(Enum.GetName(typeof(Enums.ComparatorType), Enums.ComparatorType.ExpressionValue))
                },
                new MRules._ComparatorType
                {
                    Id =(int) Enums.ComparatorType.TemporalyValue,
                    Description =  resx.GetString(Enum.GetName(typeof(Enums.ComparatorType), Enums.ComparatorType.TemporalyValue))
                }
            };
        }

        /// <summary>
        /// Obtine los tipos de acciones para la regla
        /// </summary>
        /// <returns></returns>
        public List<MRules._ActionType> GetActionType()
        {
            ResourceManager resx = Resources.Errors.ResourceManager;
            return new List<MRules._ActionType>
            {
                new MRules._ActionType
                {
                    Id = (int)Enums.AssignType.ConceptAssign,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.AssignType), Enums.AssignType.ConceptAssign))
                },
                new MRules._ActionType
                {
                    Id =(int) Enums.AssignType.InvokeAssign,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.AssignType), Enums.AssignType.InvokeAssign))
                },
                new MRules._ActionType
                {
                    Id =(int) Enums.AssignType.TemporalAssign,
                    Description =  resx.GetString(Enum.GetName(typeof(Enums.AssignType), Enums.AssignType.TemporalAssign))
                }
            };
        }

        /// <summary>
        /// Obtine los tipos de acciones para la regla
        /// </summary>
        /// <returns></returns>
        public List<MRules._InvokeType> GetInvokeType()
        {
            ResourceManager resx = Resources.Errors.ResourceManager;
            return new List<MRules._InvokeType>
            {
                new MRules._InvokeType
                {
                    Id = (int)Enums.InvokeType.MessageInvoke,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.InvokeType), Enums.InvokeType.MessageInvoke))
                },
                new MRules._InvokeType
                {
                    Id =(int) Enums.InvokeType.RuleSetInvoke,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.InvokeType), Enums.InvokeType.RuleSetInvoke))
                },
                new MRules._InvokeType
                {
                    Id =(int) Enums.InvokeType.FunctionInvoke,
                    Description =  resx.GetString(Enum.GetName(typeof(Enums.InvokeType), Enums.InvokeType.FunctionInvoke))
                }
            };
        }

        /// <summary>
        /// Obtine los tipos de operadores aritmeticos para la accion   
        /// </summary>
        /// <returns></returns>
        public List<MRules._ArithmeticOperatorType> GetArithmeticOperatorType()
        {
            ResourceManager resx = Resources.Errors.ResourceManager;
            return new List<MRules._ArithmeticOperatorType>
            {
                new MRules._ArithmeticOperatorType
                {
                    Id = (int)Enums.ArithmeticOperatorType.Add,
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, Enums.ArithmeticOperatorType>(Enums.ArithmeticOperatorType.Add).Value,
                    Description = resx.GetString(Enums.ArithmeticOperatorType.Add.ToString())
                },
                new MRules._ArithmeticOperatorType
                {
                    Id = (int) Enums.ArithmeticOperatorType.Subtract,
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, Enums.ArithmeticOperatorType>(Enums.ArithmeticOperatorType.Subtract).Value,
                    Description = resx.GetString(Enums.ArithmeticOperatorType.Subtract.ToString())
                },
                new MRules._ArithmeticOperatorType
                {
                    Id = (int) Enums.ArithmeticOperatorType.Multiply,
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, Enums.ArithmeticOperatorType>(Enums.ArithmeticOperatorType.Multiply).Value,
                    Description = resx.GetString(Enums.ArithmeticOperatorType.Multiply.ToString())
                },
                new MRules._ArithmeticOperatorType
                {
                    Id = (int) Enums.ArithmeticOperatorType.Divide,
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, Enums.ArithmeticOperatorType>(Enums.ArithmeticOperatorType.Divide).Value,
                    Description = resx.GetString(Enums.ArithmeticOperatorType.Divide.ToString())
                },
                new MRules._ArithmeticOperatorType
                {
                    Id = (int) Enums.ArithmeticOperatorType.Assign,
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, Enums.ArithmeticOperatorType>(Enums.ArithmeticOperatorType.Assign).Value,
                    Description = resx.GetString(Enums.ArithmeticOperatorType.Assign.ToString())
                },
                new MRules._ArithmeticOperatorType
                {
                    Id = (int) Enums.ArithmeticOperatorType.Round,
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, Enums.ArithmeticOperatorType>(Enums.ArithmeticOperatorType.Round).Value,
                    Description = resx.GetString(Enums.ArithmeticOperatorType.Round.ToString())
                }
            };
        }

        /// <summary>
        ///  Realiza el guardado del paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a guardar</param>
        /// <returns></returns>
        public MRules._RuleSet CreateRuleSet(MRules._RuleSet ruleSet, byte[] ruleXml)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleSet.Properties.Description, typeof(EnRules.RuleSet).Name).Equal().Constant(ruleSet.Description);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EnRules.RuleSet), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                throw new ValidationException("Ya existe una regla con el mismo nombre");
            }
            EnRules.RuleSet rule;

            if (ruleSet.RuleSetId == 0)
            {
                rule = new EnRules.RuleSet
                {
                    Description = ruleSet.Description,
                    IsEvent = ruleSet.IsEvent,
                    CurrentFrom = DateTime.Now,
                    CurrentTo = DateTime.Now,
                    LevelId = ruleSet.Level.LevelId,
                    PackageId = ruleSet.Package.PackageId,
                    RuleSetVer = 1,
                    RuleSetXml = ruleXml,
                    Active = ruleSet.Active,
                    TypeActive = ruleSet.ActiveType == (Utilities.Enums.ActiveRuleSetType?)null ? null : (int?)(Utilities.Enums.ActiveRuleSetType)(ruleSet.ActiveType)

                };

            }
            else
            {
                rule = new EnRules.RuleSet(ruleSet.RuleSetId)
                {
                    Description = ruleSet.Description,
                    IsEvent = ruleSet.IsEvent,
                    CurrentFrom = DateTime.Now,
                    CurrentTo = DateTime.Now,
                    LevelId = ruleSet.Level.LevelId,
                    PackageId = ruleSet.Package.PackageId,
                    RuleSetVer = 1,
                    RuleSetXml = ruleXml,
                    Active = ruleSet.Active,
                    TypeActive = ruleSet.ActiveType == (Utilities.Enums.ActiveRuleSetType?)null ? null : (int?)(Utilities.Enums.ActiveRuleSetType)(ruleSet.ActiveType)
                };
            }


            DataFacadeManager.Instance.GetDataFacade().InsertObject(rule);

            return _ModelAssembler.CreateRuleSet(rule);
        }

        /// <summary>
        ///  Realiza la modificacion del paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a modificar</param>
        /// <returns></returns>
        public MRules._RuleSet UpdateRuleSet(MRules._RuleSet ruleSet, byte[] ruleXml)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleSet.Properties.Description, typeof(EnRules.RuleSet).Name).Equal().Constant(ruleSet.Description);
            filter.And().Property(EnRules.RuleSet.Properties.RuleSetId, typeof(EnRules.RuleSet).Name).Distinct().Constant(ruleSet.RuleSetId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EnRules.RuleSet), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                throw new ValidationException("Ya existe una regla con el mismo nombre");
            }

            PrimaryKey key = EnRules.RuleSet.CreatePrimaryKey(ruleSet.RuleSetId);
            EnRules.RuleSet rule = (EnRules.RuleSet)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            rule.Description = ruleSet.Description;
            if (ruleXml != null)
            {
                rule.RuleSetVer = rule.RuleSetVer + 1;
                rule.CurrentTo = DateTime.Now;
                rule.RuleSetXml = ruleXml;
            }
            rule.LevelId = ruleSet.Level.LevelId;
            rule.Active = ruleSet.Active;
            rule.TypeActive = ruleSet.ActiveType == (Utilities.Enums.ActiveRuleSetType?)null ? null : (int?)(Utilities.Enums.ActiveRuleSetType)ruleSet.ActiveType;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(rule);

            return ruleSet;
        }

        public void DeleteRuleSet(int ruleSetId)
        {
            PrimaryKey key = EnRules.RuleSet.CreatePrimaryKey(ruleSetId);
            EnRules.RuleSet ruleSet = (EnRules.RuleSet)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(ruleSet);
        }

        public async Task<MRules._RuleSet> ImportRuleSet(MRules._RuleSet ruleSet)
        {
            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ImportHelper importHelper = new ImportHelper();
                    MRules._RuleSet rule = await importHelper.ImportRuleSet(ruleSet);

                    transaction.Complete();
                    if (rule.HasError)
                    {
                        return rule;
                    }
                    else
                    {
                        return this.GetRuleSetByIdRuleSet(rule.RuleSetId, true);
                    }

                }
                catch (Exception ex)
                {
                    if (ex.GetType().Name == "BusinessException" || ex.GetBaseException().Source == "System.ServiceModel")
                    {
                        throw new ValidationException(ex.Message, ex);
                    }
                    else
                    {
                        throw new ValidationException("Error al importar reglas", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Genera archivo txt con las reglas con errores
        /// </summary>
        public void GetRulesWithExceptions()
        {
            string path = ConfigurationManager.AppSettings["ReportExportPath"] + "\\" + "Reporte_errores" + ".txt";

            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine("Fecha de inicio: " + DateTime.Now);
            }
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleSet.Properties.RuleSetId);
            filter.GreaterEqual();
            filter.Constant(938);

            List<EnRules.RuleSet> businessCollections = DataFacadeManager.Instance.GetDataFacade().List(typeof(EnRules.RuleSet), filter.GetPredicate()).Cast<EnRules.RuleSet>().ToList();

            foreach (var businessCollection in businessCollections)
            {
                try
                {
                    MRules._RuleSet modelRuleSet = new MRules._RuleSet()
                    {
                        Rules = XmlHelperReader.GetRuleSetByXml(businessCollection.RuleSetXml).Rules
                    };
                }
                catch (AggregateException e)
                {
                    using (var tw = new StreamWriter(path, true))
                    {
                        e.InnerExceptions.ToList().ForEach(item => tw.WriteLine(e.Message + " (" + businessCollection.RuleSetId + ")" + item.Message));
                    }
                }
            }
            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine("Fecha fin: " + DateTime.Now);
            }
        }
    }

    public class _ConceptDao
    {
        /// <summary>
        /// Obtiene los conceptos segun el filtro 
        /// </summary>
        /// <param name="listEntities">lista de id de entidades</param>
        /// <param name="filter">like de la descripcion</param>
        /// <returns></returns>
        public List<MRules._Concept> GetConceptByFilter(List<int> listEntities, string filter)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.Concept.Properties.EntityId, typeof(EnRules.Concept).Name).In().ListValue();
            listEntities.ForEach(x => where.Constant(x));
            where.EndList();

            if (!string.IsNullOrEmpty(filter))
            {
                where.And().Property(EnRules.Concept.Properties.ConceptName, typeof(EnRules.Concept).Name).Like()
                    .Constant(filter);
            }

            GetConceptView view = new GetConceptView();
            ViewBuilder builder = new ViewBuilder("GetConceptView")
            {
                Filter = where.GetPredicate()
            };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<MRules._Concept> result = new List<MRules._Concept>();
            foreach (EnRules.Concept concept in view.Concept.Cast<EnRules.Concept>().ToList())
            {
                MRules._Concept modelConcept = new MRules._Concept
                {
                    ConceptId = concept.ConceptId,
                    Entity = new Models.Entity { EntityId = concept.EntityId },
                    Description = concept.Description,
                    ConceptName = concept.ConceptName,
                    ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
                    KeyOrder = concept.KeyOrder,
                    IsStatic = concept.IsStatic,
                    ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
                    IsReadOnly = concept.IsReadOnly,
                    IsVisible = concept.IsVisible,
                    IsNulleable = concept.IsNullable,
                    IsPersistible = concept.IsPersistible,
                    ConceptDependences = new List<MRules._ConceptDependence>()
                };

                foreach (EnRules.ConceptDependencies dependency in view.ConceptDependency.Cast<EnRules.ConceptDependencies>().Where(x => x.EntityId == concept.EntityId && x.ConceptId == concept.ConceptId).OrderBy(x => x.DependsOrder).ToList())
                {
                    EnRules.Concept dependsConcept = view.Concept2.Cast<EnRules.Concept>().FirstOrDefault(x => x.EntityId == dependency.DependsEntityId && x.ConceptId == dependency.DependsConceptId);
                    if (dependsConcept != null)
                    {
                        modelConcept.ConceptDependences.Add(new MRules._ConceptDependence
                        {
                            DependsConcept = new MRules._Concept
                            {
                                ConceptId = dependsConcept.ConceptId,
                                Entity = new Models.Entity { EntityId = dependsConcept.EntityId },
                                Description = dependsConcept.Description,
                                ConceptName = dependsConcept.ConceptName,
                                ConceptType = (Enums.ConceptType)dependsConcept.ConceptTypeCode,
                                KeyOrder = dependsConcept.KeyOrder,
                                IsStatic = dependsConcept.IsStatic,
                                ConceptControlType = (Enums.ConceptControlType)dependsConcept.ConceptControlCode,
                                IsReadOnly = dependsConcept.IsReadOnly,
                                IsVisible = dependsConcept.IsVisible,
                                IsNulleable = dependsConcept.IsNullable,
                                IsPersistible = dependsConcept.IsPersistible
                            },
                            Order = dependency.DependsOrder,
                            ColumnName = dependency.ColumnName
                        });
                    }
                }

                result.Add(modelConcept);
            }
            return result.OrderBy(x => x.Description).ToList();
        }
        //private List<MRules._Concept> GetConceptCache(List<int> listEntities, string filter)
        //{

        //    //var entityConcepts = (List<Sistran.Core.Application.Utilities.Entities.Concept>)InProcCache.Get(Utilities.Helper.RulesConstant.Entityconcept, Utilities.Helper.RulesConstant.Entityconcept);
        //    //var conceptDependencies = (List<Utilities.Entities.ConceptDependencies>)InProcCache.Get(Utilities.Helper.RulesConstant.ConceptDependencies, Utilities.Helper.RulesConstant.ConceptDependencies);
        //    //List<Sistran.Core.Application.Utilities.Entities.Concept> concepts = null;
        //    if (string.IsNullOrEmpty(filter))
        //    {
        //        concepts = entityConcepts?.Where(x => listEntities.Contains(x.EntityId)).ToList();
        //    }
        //    else
        //    {
        //        concepts = entityConcepts?.Where(x => listEntities.Contains(x.EntityId) && x.ConceptName == filter).ToList();
        //    }
        //    if (concepts != null && concepts.Count > 0 && conceptDependencies != null)
        //    {
        //        ConcurrentBag<MRules._Concept> result = new ConcurrentBag<MRules._Concept>();
        //        Parallel.ForEach(concepts, concept =>
        //        {
        //            MRules._Concept modelConcept = new MRules._Concept
        //            {
        //                ConceptId = concept.ConceptId,
        //                Entity = new Models.Entity { EntityId = concept.EntityId },
        //                Description = concept.Description,
        //                ConceptName = concept.ConceptName,
        //                ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
        //                KeyOrder = concept.KeyOrder,
        //                IsStatic = concept.IsStatic,
        //                ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
        //                IsReadOnly = concept.IsReadOnly,
        //                IsVisible = concept.IsVisible,
        //                IsNulleable = concept.IsNullable,
        //                IsPersistible = concept.IsPersistible,
        //                ConceptDependences = new List<MRules._ConceptDependence>()
        //            };
        //            Parallel.ForEach(conceptDependencies.Where(x => x.EntityId == concept.EntityId && x.ConceptId == concept.ConceptId).OrderBy(x => x.DependsOrder).ToList(), dependency =>
        //            {
        //                Sistran.Core.Application.Utilities.Entities.Concept dependsConcept = concepts.FirstOrDefault(x => x.EntityId == dependency.DependsEntityId && x.ConceptId == dependency.DependsConceptId);
        //                if (dependsConcept != null)
        //                {
        //                    modelConcept.ConceptDependences.Add(new MRules._ConceptDependence
        //                    {
        //                        DependsConcept = new MRules._Concept
        //                        {
        //                            ConceptId = dependsConcept.ConceptId,
        //                            Entity = new Models.Entity { EntityId = dependsConcept.EntityId },
        //                            Description = dependsConcept.Description,
        //                            ConceptName = dependsConcept.ConceptName,
        //                            ConceptType = (Enums.ConceptType)dependsConcept.ConceptTypeCode,
        //                            KeyOrder = dependsConcept.KeyOrder,
        //                            IsStatic = dependsConcept.IsStatic,
        //                            ConceptControlType = (Enums.ConceptControlType)dependsConcept.ConceptControlCode,
        //                            IsReadOnly = dependsConcept.IsReadOnly,
        //                            IsVisible = dependsConcept.IsVisible,
        //                            IsNulleable = dependsConcept.IsNullable,
        //                            IsPersistible = dependsConcept.IsPersistible
        //                        },
        //                        Order = dependency.DependsOrder,
        //                        ColumnName = dependency.ColumnName
        //                    });
        //                }
        //            });
        //            result.Add(modelConcept);
        //        });
        //        return result.ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

        /// <summary>
        /// Obtiene los conceptos por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public MRules._Concept GetConceptByIdConceptIdEntity(int idConcept, int idEntity)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.Concept.Properties.EntityId, typeof(EnRules.Concept).Name).Equal().Constant(idEntity);
            where.And().Property(EnRules.Concept.Properties.ConceptId, typeof(EnRules.Concept).Name).Equal().Constant(idConcept);

            GetConceptView view = new GetConceptView();
            ViewBuilder builder = new ViewBuilder("GetConceptView")
            {
                Filter = where.GetPredicate()
            };

            IDataFacade daf = DataFacadeManager.Instance.GetDataFacade();
            daf.FillView(builder, view);
            daf.Dispose();

            List<MRules._Concept> result = new List<MRules._Concept>();
            foreach (EnRules.Concept concept in view.Concept.Cast<EnRules.Concept>().ToList())
            {
                MRules._Concept modelConcept = new MRules._Concept
                {
                    ConceptId = concept.ConceptId,
                    Entity = new Models.Entity { EntityId = concept.EntityId },
                    Description = concept.Description,
                    ConceptName = concept.ConceptName,
                    ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
                    KeyOrder = concept.KeyOrder,
                    IsStatic = concept.IsStatic,
                    ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
                    IsReadOnly = concept.IsReadOnly,
                    IsVisible = concept.IsVisible,
                    IsNulleable = concept.IsNullable,
                    IsPersistible = concept.IsPersistible,
                    ConceptDependences = new List<MRules._ConceptDependence>()
                };

                foreach (EnRules.ConceptDependencies dependency in view.ConceptDependency.Cast<EnRules.ConceptDependencies>().Where(x => x.EntityId == concept.EntityId && x.ConceptId == concept.ConceptId).ToList())
                {
                    EnRules.Concept dependsConcept = view.Concept2.Cast<EnRules.Concept>().FirstOrDefault(x => x.EntityId == dependency.DependsEntityId && x.ConceptId == dependency.DependsConceptId);
                    if (dependsConcept != null)
                    {
                        modelConcept.ConceptDependences.Add(new MRules._ConceptDependence
                        {
                            DependsConcept = new MRules._Concept
                            {
                                ConceptId = dependsConcept.ConceptId,
                                Entity = new Models.Entity { EntityId = dependsConcept.EntityId },
                                Description = dependsConcept.Description,
                                ConceptName = dependsConcept.ConceptName,
                                ConceptType = (Enums.ConceptType)dependsConcept.ConceptTypeCode,
                                KeyOrder = dependsConcept.KeyOrder,
                                IsStatic = dependsConcept.IsStatic,
                                ConceptControlType = (Enums.ConceptControlType)dependsConcept.ConceptControlCode,
                                IsReadOnly = dependsConcept.IsReadOnly,
                                IsVisible = dependsConcept.IsVisible,
                                IsNulleable = dependsConcept.IsNullable,
                                IsPersistible = dependsConcept.IsPersistible
                            },
                            Order = dependency.DependsOrder,
                            ColumnName = dependency.ColumnName
                        });
                    }
                }

                result.Add(modelConcept);
            }

            return result.First();
        }

        /// <summary>
        /// Obtiene el concepto especifico con sus respectivos valores
        /// </summary>
        /// <param name="idEntity">id de la entidad</param>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="conceptType">tipo de concepto</param>
        /// <returns></returns>
        public object GetSpecificConceptWithVales(int idConcept, int idEntity, string[] dependency, Enums.ConceptType conceptType)
        {
            switch (conceptType)
            {
                case Enums.ConceptType.Basic:
                    return this.GetBasicConceptByIdConceptIdEntity(idConcept, idEntity);

                case Enums.ConceptType.Range:
                    return this.GetRangeConceptByIdConceptIdEntity(idConcept, idEntity);

                case Enums.ConceptType.List:
                    return this.GetListConceptByIdConceptIdEntity(idConcept, idEntity);

                case Enums.ConceptType.Reference:
                    return this.GetReferenceConceptByIdConceptIdEntity(idConcept, idEntity, dependency);
            }
            throw new BusinessException("error al obtener el concepto especifico");
        }

        /// <summary>
        /// Obtiene el concepto basico por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public MRules._BasicConcept GetBasicConceptByIdConceptIdEntity(int idConcept, int idEntity)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.Concept.Properties.ConceptId, "Concept").Equal().Constant(idConcept);
            where.And().Property(EnRules.Concept.Properties.EntityId, "Concept").Equal().Constant(idEntity);

            GetBasicConceptView view = new GetBasicConceptView();
            ViewBuilder builder = new ViewBuilder("GetBasicConceptView")
            {
                Filter = where.GetPredicate()
            };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            MRules._BasicConcept basicConcept = _ModelAssembler.CreateBasicConcept((EnRules.Concept)view.Concept[0]);

            basicConcept.BasicType = (Enums.BasicType)Enum.Parse(typeof(Enums.BasicType), ((EnRules.BasicConcept)view.BasicConcept[0]).BasicTypeCode.ToString());

            foreach (EnRules.BasicConceptCheck check in view.BasicConceptCheck)
            {
                if (check.BasicCheckCode == 1)//maximo
                {
                    if (check.IntValue != null)
                    {
                        basicConcept.MaxValue = check.IntValue;
                    }
                    if (check.DateValue != null)
                    {
                        basicConcept.MaxDate = check.DateValue;
                    }
                }
                else if (check.BasicCheckCode == 2) //minimo
                {
                    if (check.IntValue != null)
                    {
                        basicConcept.MinValue = check.IntValue;
                    }
                    if (check.DateValue != null)
                    {
                        basicConcept.MinDate = check.DateValue;
                    }
                }
                else //length
                {
                    if (check.IntValue != null)
                    {
                        basicConcept.Length = check.IntValue;
                    }
                }
            }
            return basicConcept;
        }

        /// <summary>
        /// Obtiene los conceptos por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public MRules._RangeConcept GetRangeConceptByIdConceptIdEntity(int idConcept, int idEntity)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.Concept.Properties.ConceptId, "Concept").Equal().Constant(idConcept);
            where.And().Property(EnRules.Concept.Properties.EntityId, "Concept").Equal().Constant(idEntity);

            GetRangeConceptView view = new GetRangeConceptView();
            ViewBuilder builder = new ViewBuilder("GetRangeConceptView")
            {
                Filter = where.GetPredicate()
            };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            MRules._RangeConcept rangeConcept = _ModelAssembler.CreateRangeConcept((EnRules.Concept)view.Concept[0]);

            rangeConcept.RangeEntity.RangeEntityCode = ((EnRules.RangeConcept)view.RangeConcept[0]).RangeEntityCode;
            rangeConcept.RangeEntity.DescriptionRange = ((EnRules.RangeEntity)view.RangeEntity[0]).Description;
            rangeConcept.RangeEntity.RangeValueAt = ((EnRules.RangeEntity)view.RangeEntity[0]).RangeValueAt;
            rangeConcept.RangeEntity.RangeEntityValues =
                _ModelAssembler.CreateRangeEntityValues(view.RangeEntityValue.Cast<EnRules.RangeEntityValue>()
                    .ToList());

            return rangeConcept;
        }

        /// <summary>
        /// Obtiene los conceptos por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public MRules._ListConcept GetListConceptByIdConceptIdEntity(int idConcept, int idEntity)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.Concept.Properties.ConceptId, "Concept").Equal().Constant(idConcept);
            where.And().Property(EnRules.Concept.Properties.EntityId, "Concept").Equal().Constant(idEntity);

            GetListConceptView view = new GetListConceptView();
            ViewBuilder builder = new ViewBuilder("GetListConceptView")
            {
                Filter = where.GetPredicate()
            };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            MRules._ListConcept listConcept = _ModelAssembler.CreateListConcept((EnRules.Concept)view.Concept[0]);

            listConcept.ListEntity.ListEntityCode = ((EnRules.ListConcept)view.ListConcept[0]).ListEntityCode;
            listConcept.ListEntity.DescriptionList = ((EnRules.ListEntity)view.ListEntity[0]).Description;
            listConcept.ListEntity.ListValueAt = ((EnRules.ListEntity)view.ListEntity[0]).ListValueAt;
            listConcept.ListEntity.ListEntityValues =
                _ModelAssembler.CreateListEntityValues(view.ListEntityValue.Cast<EnRules.ListEntityValue>().ToList());

            return listConcept;
        }

        /// <summary>
        /// Obtiene los conceptos por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public MRules._ReferenceConcept GetReferenceConceptByIdConceptIdEntity(int idConcept, int idEntity, string[] dependency)
        {
            try
            {
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EnRules.Concept.Properties.ConceptId, "Concept").Equal().Constant(idConcept);
                where.And().Property(EnRules.Concept.Properties.EntityId, "Concept").Equal().Constant(idEntity);

                GetReferenceConceptView view = new GetReferenceConceptView();
                ViewBuilder builder = new ViewBuilder("GetReferenceConceptView")
                {
                    Filter = where.GetPredicate()
                };
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                MRules._ReferenceConcept referenceConcept = _ModelAssembler.CreateReferenceConcept((EnRules.Concept)view.Concept[0]);

                referenceConcept.FEntity = _ModelAssembler.CreateEntity((EnRules.Entity)view.Entity[0]);
                referenceConcept.ReferenceValues = new List<MRules._ReferenceValue>();

                if (string.IsNullOrEmpty(referenceConcept.FEntity.PropertySearch) || string.IsNullOrEmpty(referenceConcept.FEntity.BusinessView))
                {
                    throw new NullReferenceException($"Las propiedades (BusinessView,PropertySearch) del valor referencia ({referenceConcept.FEntity}){referenceConcept.FEntity.Description}, no puede ser nulos o vacio");
                }
                string[] fields = referenceConcept.FEntity.PropertySearch.Split(';');
                string strColumns = string.Join(",", fields.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Split(',')[0]));

                NameValue[] parameters = new NameValue[3];
                parameters[0] = new NameValue("@TABLES", referenceConcept.FEntity.BusinessView);
                parameters[1] = new NameValue("@FIELDS", strColumns.Substring(0, strColumns.Length - 1));

                if (dependency != null && dependency.Length != 0)
                {
                    string filter = "";

                    for (int index = 0; index < view.ConceptDependency.Cast<EnRules.ConceptDependencies>().Count(); index++)
                    {
                        EnRules.ConceptDependencies dependence = view.ConceptDependency.Cast<EnRules.ConceptDependencies>().ToList()[index];

                        if (!string.IsNullOrEmpty(filter))
                        {
                            filter += " AND ";
                        }

                        filter += dependence.ColumnName + " = " + dependency[index];
                    }

                    parameters[2] = new NameValue("@FILTER", filter);
                }
                else
                {
                    parameters[2] = new NameValue("@FILTER", "1=1");
                }


                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    DataTable datas = dynamicDataAccess.ExecuteSPDataTable("SCR.GET_DATA_FROM_FILTER", parameters);

                    foreach (DataRow objeData in datas.Rows)
                    {
                        referenceConcept.ReferenceValues.Add(new MRules._ReferenceValue
                        {
                            Id = objeData[0].ToString(),
                            Description = objeData[1].ToString()
                        });
                    }
                    referenceConcept.ReferenceValues = referenceConcept.ReferenceValues.OrderBy(x => x.Description).ToList();
                }

                return referenceConcept;
            }
            catch (Exception e)
            {
                throw new Exception($"Error consultando el Concepto referencia: ConceptId:{idConcept}-EntityId:{idEntity}", e);
            }
        }

        /// <summary>
        /// obtiene el comparador del del concepto para la condicion de la regla
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public List<MRules._Comparator> GetComparatorConcept(int idConcept, int idEntity)
        {
            ResourceManager resx = Resources.Errors.ResourceManager;
            MRules._Concept concept = this.GetConceptByIdConceptIdEntity(idConcept, idEntity);
            List<MRules._Comparator> comparatorConcepts = new List<MRules._Comparator>
            {
                new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.Equals,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType), Enums.OperatorConditionType.Equals)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums.OperatorConditionType
                            .Equals).Value
                },
                new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.Distinct,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType),Enums.OperatorConditionType.Distinct)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums.OperatorConditionType
                            .Distinct).Value
                }
            };


            if (concept.IsNulleable || !concept.IsStatic)
            {
                comparatorConcepts.Add(new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.Null,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType), Enums.OperatorConditionType.Null)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums.OperatorConditionType
                            .Null).Value
                });

                comparatorConcepts.Add(new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.NotNull,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType), Enums.OperatorConditionType.NotNull)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums.OperatorConditionType
                            .NotNull).Value
                });
            }

            if (concept.ConceptType == Enums.ConceptType.Basic &&
                (concept.ConceptControlType == Enums.ConceptControlType.NumberEditor ||
                 concept.ConceptControlType == Enums.ConceptControlType.DateEditor))
            {
                comparatorConcepts.Add(new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.GreaterThan,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType), Enums.OperatorConditionType.GreaterThan)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums
                            .OperatorConditionType.GreaterThan).Value
                });

                comparatorConcepts.Add(new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.LessThan,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType), Enums.OperatorConditionType.LessThan)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums
                            .OperatorConditionType.LessThan).Value
                });

                comparatorConcepts.Add(new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.GreaterThanOrEquals,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType), Enums.OperatorConditionType.GreaterThanOrEquals)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums
                            .OperatorConditionType.GreaterThanOrEquals).Value
                });

                comparatorConcepts.Add(new MRules._Comparator
                {
                    Operator = Enums.OperatorConditionType.LessThanOrEquals,
                    Description = resx.GetString(Enum.GetName(typeof(Enums.OperatorConditionType), Enums.OperatorConditionType.LessThanOrEquals)),
                    Symbol = EnumHelper
                        .GetValueMember<EnumMemberAttribute, Enums.OperatorConditionType>(Enums
                            .OperatorConditionType.LessThanOrEquals).Value
                });
            }
            return comparatorConcepts;
        }

        public MRules._Concept InsertConcept(MRules._Concept concept)
        {
            EnRules.Concept conceptEntity = _EntityAssembler.CreateConcept(concept);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(conceptEntity);

            concept.ConceptId = conceptEntity.ConceptId;

            switch (concept)
            {
                case MRules._BasicConcept _:
                    EnRules.BasicConcept basicConcept = _EntityAssembler.CreateBasicConcept(concept as MRules._BasicConcept);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(basicConcept);

                    EnRules.BasicConceptCheck conceptCheckMax = new EnRules.BasicConceptCheck(1, conceptEntity.ConceptId, conceptEntity.EntityId);
                    EnRules.BasicConceptCheck conceptCheckMin = new EnRules.BasicConceptCheck(2, conceptEntity.ConceptId, conceptEntity.EntityId);
                    EnRules.BasicConceptCheck conceptCheckLenght = new EnRules.BasicConceptCheck(3, conceptEntity.ConceptId, conceptEntity.EntityId);

                    switch (((MRules._BasicConcept)concept).BasicType)
                    {
                        case Enums.BasicType.Numeric:
                        case Enums.BasicType.Decimal:
                            if (((MRules._BasicConcept)concept).MaxValue.HasValue)
                            {
                                conceptCheckMax.IntValue = ((MRules._BasicConcept)concept).MaxValue;
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(conceptCheckMax);
                            }
                            if (((MRules._BasicConcept)concept).MinValue.HasValue)
                            {
                                conceptCheckMin.IntValue = ((MRules._BasicConcept)concept).MinValue;
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(conceptCheckMin);
                            }
                            break;

                        case Enums.BasicType.Text:
                            if (((MRules._BasicConcept)concept).Length.HasValue)
                            {
                                conceptCheckLenght.IntValue = ((MRules._BasicConcept)concept).Length;
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(conceptCheckLenght);
                            }
                            break;

                        case Enums.BasicType.Date:
                            if (((MRules._BasicConcept)concept).MaxDate.HasValue)
                            {
                                conceptCheckMax.DateValue = ((MRules._BasicConcept)concept).MaxDate;
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(conceptCheckMax);
                            }
                            if (((MRules._BasicConcept)concept).MinDate.HasValue)
                            {
                                conceptCheckMin.DateValue = ((MRules._BasicConcept)concept).MinDate;
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(conceptCheckMin);
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("Tipo de concepto basico no valido " + ((MRules._BasicConcept)concept).BasicType);
                    }
                    break;

                case MRules._RangeConcept _:
                    EnRules.RangeConcept rangeConcept = _EntityAssembler.CreateRangeConcept(concept as MRules._RangeConcept);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(rangeConcept);
                    break;

                case MRules._ListConcept _:
                    EnRules.ListConcept listConcept = _EntityAssembler.CreateListConcept(concept as MRules._ListConcept);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(listConcept);

                    break;
                case MRules._ReferenceConcept _:
                    EnRules.ReferenceConcept referenceConcept = _EntityAssembler.CreateReferenceConcept(concept as MRules._ReferenceConcept);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(referenceConcept);
                    break;
            }

            return concept;
        }
    }

    public class _RuleFunctionDao
    {
        /// <summary>
        /// Obtiene uan funcion de reglas por su nombre
        /// </summary>
        /// <param name="nameRuleFunction">nombre de la funcion</param>
        /// <returns></returns>
        public MRules._RuleFunction GetRuleFunctionByNameRuleFunction(string nameRuleFunction)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleFunction.Properties.FunctionName).Equal().Constant(nameRuleFunction);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnRules.RuleFunction), filter.GetPredicate()));

            EnRules.RuleFunction function = businessCollection.Cast<EnRules.RuleFunction>().FirstOrDefault();

            return function != null ? _ModelAssembler.CreateRuleFunction(function) : null;
        }

        /// <summary>
        /// Obtiene las funciones de reglas que concuerden con la busqueda
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id de los niveles</param>
        /// <returns></returns>
        public List<MRules._RuleFunction> GetRuleFunctionsByIdPackageLevels(int idPackage, List<int> levels)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleFunction.Properties.PackageId).Equal().Constant(idPackage);

            if (levels.Count != 0)
            {
                filter.And().Property(EnRules.RuleFunction.Properties.LevelId, typeof(EnRules.RuleFunction).Name).In()
                    .ListValue();
                levels.ForEach(x => filter.Constant(x));
                filter.EndList();
            }


            List<EnRules.RuleFunction> businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnRules.RuleFunction), filter.GetPredicate(), new[] { EnRules.RuleFunction.Properties.Description }))
                .Cast<EnRules.RuleFunction>().ToList();

            return _ModelAssembler.CreateListRuleFunctions(businessCollection);
        }
    }

    public class _PositionEntityDao
    {
        public List<Models.Entity> GetEntitiesByPackageIdLevelId(int packageId, int levelId)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(EnParam.Entity.Properties.EntityId, "e")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Entity.Properties.Description, "e")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Entity.Properties.LevelId, "e")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Entity.Properties.PackageId, "e")));

            Join join = new Join(new ClassNameTable(typeof(EnParam.Entity), "e"), new ClassNameTable(typeof(EnRules.PositionEntity), "pe"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnParam.Entity.Properties.EntityId, "e")
                    .Equal().Property(EnRules.PositionEntity.Properties.EntityId, "pe").GetPredicate()
            };

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.PositionEntity.Properties.PackageId, "pe").Equal().Constant(packageId);
            filter.And().Property(EnRules.PositionEntity.Properties.LevelId, "pe").Equal().Constant(levelId);

            select.Table = join;
            select.Where = filter.GetPredicate();
            select.AddSortValue(new SortValue(new Column(EnRules.PositionEntity.Properties.Position, "pe"), SortOrderType.Ascending));

            List<Models.Entity> entities = new List<Models.Entity>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    entities.Add(new Models.Entity
                    {
                        EntityId = (int)reader["EntityId"],
                        LevelId = (int)reader["LevelId"],
                        PackageId = (int)reader["PackageId"],
                        Description = (string)reader["Description"]
                    });
                }
            }
            return entities;
        }
    }

    public class _PackageDao
    {
        /// <summary>
        /// Obtiene los paquetes habilitados
        /// </summary>
        /// <returns></returns>
        public List<MRules._Package> GetPackages()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnParam.Package.Properties.Disabled, typeof(EnParam.Package).Name).Equal().Constant(0);
            List<EnParam.Package> businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnParam.Package), filter.GetPredicate(), new[] { EnParam.Package.Properties.Description }))
                .Cast<EnParam.Package>().ToList();
            return _ModelAssembler.CreateListPackages(businessCollection);
        }
    }

    public class _LevelDao
    {
        /// <summary>
        /// Obtiene los niveles por el paquete
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <returns></returns>
        public List<MRules._Level> GetLevelsByIdPackage(int idPackage)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnParam.Levels.Properties.PackageId, typeof(EnParam.Levels).Name).Equal().Constant(idPackage);
            List<EnParam.Levels> businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnParam.Levels), filter.GetPredicate()))
                .Cast<EnParam.Levels>().ToList();

            return _ModelAssembler.CreateListLevels(businessCollection);
        }
    }

    public class _RuleBaseDao
    {
        /// <summary>
        /// Obtiene el listado de tablas de decision por el filtro indicado
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">lista de los niveles</param>
        /// <param name="isPolicie">si es politica</param>
        /// <param name="filter">filtro like de la descripcion</param>
        /// <returns></returns>
        public List<MRules._RuleBase> GetDecisionTableByFilter(int? idPackage, List<int> levels, bool isPolicie, string filter, int tableId, DateTime? dateFrom, bool? isPublished)
        {
            SelectQuery select = new SelectQuery();
            select.MaxRows = 50;
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleBaseId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.Description, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleBaseTypeCode, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.PackageId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.LevelId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.CurrentFrom, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleBaseVersion, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.IsPublished, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleEnumerator, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.IsEvent, "r")));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.PackageId, "p")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PDescription"));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.LevelId, "l")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.Description, "l"), "LDescription"));

            Join join = new Join(new ClassNameTable(typeof(EnRules.RuleBase), "r"), new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleBase.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(EnParam.Levels), "l"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleBase.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Levels.Properties.PackageId, "l")

                    .And().Property(EnRules.RuleBase.Properties.LevelId, "r")
                    .Equal().Property(EnParam.Levels.Properties.LevelId, "l")
                    .GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.RuleBase.Properties.IsEvent, "r").Equal().Constant(isPolicie);

            if (idPackage != null)
            {
                where.And().Property(EnRules.RuleBase.Properties.PackageId, "r").Equal().Constant(idPackage);
            }
            if (levels != null && levels.Count != 0)
            {
                where.And().Property(EnRules.RuleBase.Properties.LevelId, "r").In().ListValue();
                levels.ForEach(x => where.Constant(x));
                where.EndList();
            }
            if (!string.IsNullOrEmpty(filter))
            {
                where.And().Property(EnRules.RuleBase.Properties.Description, "r").Like().Constant("%" + filter + "%");
            }
            if (tableId > 0)
            {
                where.And().Property(EnRules.RuleBase.Properties.RuleBaseId, "r").Equal().Constant(tableId);
            }
            if (dateFrom != null)
            {
                where.And().Property(EnRules.RuleBase.Properties.CurrentFrom, "r").GreaterEqual().Constant(Convert.ToDateTime(dateFrom).Date);
                where.And().Property(EnRules.RuleBase.Properties.CurrentFrom, "r").Less().Constant(Convert.ToDateTime(dateFrom).AddHours(24));
            }
            if (isPublished != null)
            {
                where.And().Property(EnRules.RuleBase.Properties.IsPublished, "r").Equal().Constant(isPublished);
            }
            select.Table = join;
            select.Where = where.GetPredicate();
            select.AddSortValue(new SortValue(new Column(EnRules.RuleBase.Properties.CurrentFrom, "r"), SortOrderType.Descending));

            List<MRules._RuleBase> result = new List<MRules._RuleBase>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    result.Add(new MRules._RuleBase
                    {
                        RuleBaseId = (int)reader["RuleBaseId"],
                        Description = (string)reader["Description"],
                        RuleBaseType = Enums.RuleBaseType.Option,
                        Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PDescription"] },
                        Level = new MRules._Level { LevelId = (int)reader["LevelId"], Description = (string)reader["LDescription"] },
                        CurrentFrom = ((DateTime)reader["CurrentFrom"]).ToString("dd/MM/yyyy"),
                        Version = (int)reader["RuleBaseVersion"],
                        IsPublished = (bool)reader["IsPublished"],
                        RuleEnumerator = (int)reader["RuleEnumerator"],
                        IsEvent = (bool)reader["IsEvent"]
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// elimina la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        public void DeleteTableDecision(int ruleBaseId)
        {
            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                    //elimina RULE_ACTION_CONCEPT
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(EnRules.RuleActionConcept.Properties.RuleBaseId, ruleBaseId);
                    DataFacadeManager.Instance.GetDataFacade()
                        .DeleteObjects(typeof(EnRules.RuleActionConcept), filter.GetPredicate());

                    //elimina RULE_CONDITION_CONCEPT
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(EnRules.RuleConditionConcept.Properties.RuleBaseId, ruleBaseId);
                    DataFacadeManager.Instance.GetDataFacade()
                        .DeleteObjects(typeof(EnRules.RuleConditionConcept), filter.GetPredicate());

                    //elimina RULE_CONDITION
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(EnRules.RuleCondition.Properties.RuleBaseId, ruleBaseId);
                    DataFacadeManager.Instance.GetDataFacade()
                        .DeleteObjects(typeof(EnRules.RuleCondition), filter.GetPredicate());

                    //elimina RULE_ACTION
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(EnRules.RuleAction.Properties.RuleBaseId, ruleBaseId);
                    DataFacadeManager.Instance.GetDataFacade()
                        .DeleteObjects(typeof(EnRules.RuleAction), filter.GetPredicate());

                    //elimina rules
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(EnRules.Rule.Properties.RuleBaseId, ruleBaseId);
                    DataFacadeManager.Instance.GetDataFacade()
                        .DeleteObjects(typeof(EnRules.Rule), filter.GetPredicate());

                    //emilina rule base
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(EnRules.RuleBase.Properties.RuleBaseId, ruleBaseId);
                    DataFacadeManager.Instance.GetDataFacade()
                        .DeleteObjects(typeof(EnRules.RuleBase), filter.GetPredicate());
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new BusinessException("Error al eliminar tabla de decisión", e);
                }
            }
        }


        /// <summary>
        /// obtiene los conceptos_condicion de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        public List<MRules._Concept> GetConceptsConditionByRuleBaseId(int ruleBaseId)
        {
            _ConceptDao conceptDao = new _ConceptDao();
            List<MRules._Concept> concepts = new List<MRules._Concept>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(EnRules.RuleConditionConcept.Properties.RuleBaseId, ruleBaseId);
            IEnumerable<EnRules.RuleConditionConcept> conditionConcepts = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                 .SelectObjects(typeof(EnRules.RuleConditionConcept), filter.GetPredicate(), new[] { EnRules.RuleConditionConcept.Properties.OrderNum }))
                 .Cast<EnRules.RuleConditionConcept>();

            foreach (EnRules.RuleConditionConcept concept in conditionConcepts)
            {
                concepts.Add(conceptDao.GetConceptByIdConceptIdEntity(concept.ConceptId, concept.EntityId));
            }

            return concepts;
        }

        /// <summary>
        /// obtiene la conceptos_accion de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        public List<MRules._Concept> GetConceptsActionByRuleBaseId(int ruleBaseId)
        {
            _ConceptDao conceptDao = new _ConceptDao();
            List<MRules._Concept> concepts = new List<MRules._Concept>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(EnRules.RuleActionConcept.Properties.RuleBaseId, ruleBaseId);
            IEnumerable<EnRules.RuleActionConcept> actionConcepts = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(EnRules.RuleActionConcept), filter.GetPredicate(), new[] { EnRules.RuleActionConcept.Properties.OrderNum }))
                .Cast<EnRules.RuleActionConcept>();

            foreach (EnRules.RuleActionConcept concept in actionConcepts)
            {
                concepts.Add(conceptDao.GetConceptByIdConceptIdEntity(concept.ConceptId, concept.EntityId));
            }

            return concepts;
        }


        /// <summary>
        /// obtiene la Data de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <returns></returns>
        public List<MRules._Rule> GetTableDecisionBody(int ruleBaseId)
        {
            _ConceptDao conceptDao = new _ConceptDao();

            Dictionary<string, List<MRules._Comparator>> comparators = new Dictionary<string, List<MRules._Comparator>>();
            ConcurrentBag<MRules._Rule> rulesResult = new ConcurrentBag<MRules._Rule>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleConditionConcept.Properties.RuleBaseId, typeof(EnRules.RuleConditionConcept).Name).Equal().Constant(ruleBaseId);
            RuleConditionConceptView conditionConceptView = new RuleConditionConceptView();
            ViewBuilder builderCondition = new ViewBuilder("RuleConditionConcept") { Filter = filter.GetPredicate() };
            DataFacadeManager.Instance.GetDataFacade().FillView(builderCondition, conditionConceptView);

            filter.Clear();
            filter.Property(EnRules.RuleActionConcept.Properties.RuleBaseId, typeof(EnRules.RuleActionConcept).Name).Equal().Constant(ruleBaseId);
            RuleActionConceptView actionConceptView = new RuleActionConceptView();
            ViewBuilder builderAction = new ViewBuilder("RuleActionConcept") { Filter = filter.GetPredicate() };
            DataFacadeManager.Instance.GetDataFacade().FillView(builderAction, actionConceptView);

            List<MRules._Concept> concepts = _ModelAssembler.CreateListConcepts(conditionConceptView.Concept.Cast<EnRules.Concept>().ToList());
            concepts.AddRange(_ModelAssembler.CreateListConcepts(actionConceptView.Concept.Cast<EnRules.Concept>().ToList()));

            filter.Clear();
            filter.Property(EnRules.Rule.Properties.RuleBaseId).Equal().Constant(ruleBaseId);
            List<EnRules.Rule> rules = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EnRules.Rule), filter.GetPredicate())).Cast<EnRules.Rule>().ToList();
            List<EnRules.RuleCondition> conditions = conditionConceptView.RuleCondition.Cast<EnRules.RuleCondition>().ToList();
            List<EnRules.RuleConditionConcept> conditionConcepts = conditionConceptView.RuleConditionConcept.Cast<EnRules.RuleConditionConcept>().OrderBy(x => x.OrderNum).ToList().ToList();
            List<EnRules.RuleAction> actions = actionConceptView.RuleAction.Cast<EnRules.RuleAction>().ToList();
            List<EnRules.RuleActionConcept> actionConcepts = actionConceptView.RuleActionConcept.Cast<EnRules.RuleActionConcept>().OrderBy(x => x.OrderNum).ToList();


            List<MRules._Concept> conceptsValues = new List<MRules._Concept>();
            foreach (MRules._Concept concept in concepts)
            {
                MRules._Concept conceptTmp = conceptDao.GetConceptByIdConceptIdEntity(concept.ConceptId, concept.Entity.EntityId);
                MRules._Concept conceptTmp2 = (MRules._Concept)conceptDao.GetSpecificConceptWithVales(concept.ConceptId, concept.Entity.EntityId, new string[0], concept.ConceptType);
                conceptTmp2.ConceptDependences = conceptTmp.ConceptDependences;

                conceptsValues.Add(conceptTmp2);
                if (!comparators.ContainsKey(concept.ConceptId + "-" + concept.Entity.EntityId))
                {
                    comparators.Add(concept.ConceptId + "-" + concept.Entity.EntityId, conceptDao.GetComparatorConcept(concept.ConceptId, concept.Entity.EntityId));
                }
            }

            Exception error = null;
            object _object = new object();
            int numProcess = Convert.ToInt32(ConfigurationManager.AppSettings["MaxProcessThreadRuleSet"]);
            int numThread = Math.Max(rules.Count, numProcess) / numProcess;
            List<Task> threads = new List<Task>();

            /*REGLAS*/
            for (int t = 0; t < numThread; t++)
            {
                List<MRules._Rule> listRules = new List<MRules._Rule>();
                int length = numProcess;
                int ii = t;

                Task thread = new Task(() =>
                {
                    if (ii == numThread - 1)
                    {
                        length = rules.Count - (numProcess * ii);
                    }

                    try
                    {
                        foreach (EnRules.Rule rule in rules.GetRange(numProcess * ii, length))
                        {
                            MRules._Rule modelRule = new MRules._Rule
                            {
                                RuleId = rule.RuleId,
                                Description = "r_" + rule.RuleId,
                                Actions = new List<MRules._Action>(),
                                Conditions = new List<MRules._Condition>()
                            };

                            #region condiciones
                            foreach (EnRules.RuleConditionConcept conditionConcept in conditionConcepts)
                            {
                                EnRules.RuleCondition valueConditionConcept =
                                     conditions.FirstOrDefault(x => x.ConceptId == conditionConcept.ConceptId &&
                                                                    x.EntityId == conditionConcept.EntityId &&
                                                                    x.RuleId == rule.RuleId);
                                List<MRules._Comparator> listComparatorTmp = comparators
                                     .First(x => x.Key == conditionConcept.ConceptId + "-" + conditionConcept.EntityId).Value;

                                MRules._Concept concept2Tmp =
                                     concepts.First(x => x.Entity.EntityId == conditionConcept.EntityId &&
                                                         x.ConceptId == conditionConcept.ConceptId);
                                MRules._Condition condition = new MRules._Condition
                                {
                                    ComparatorType = Enums.ComparatorType.ConstantValue,
                                    Concept = concept2Tmp,
                                    Comparator = valueConditionConcept?.ComparatorCode == null
                                         ? null
                                         : listComparatorTmp.First(x => (int)x.Operator == valueConditionConcept.ComparatorCode),
                                    Expression = valueConditionConcept?.CondValue
                                };

                                if (condition.Expression != null)
                                {
                                    concept2Tmp =
                                         conceptsValues.First(x => x.Entity.EntityId == conditionConcept.EntityId &&
                                                                   x.ConceptId == conditionConcept.ConceptId);

                                    try
                                    {
                                        if (concept2Tmp.ConceptType == Enums.ConceptType.Reference)
                                        {
                                            List<MRules._Condition> lastConditions = modelRule.Conditions
                                             .Where(x => ((MRules._Concept)x.Concept).ConceptType == Enums.ConceptType.Reference)
                                             .ToList();

                                            if (concept2Tmp.ConceptDependences.Count() != 0)
                                                condition.Expression = fillExpression(concept2Tmp, condition.Expression, lastConditions);
                                            else
                                                condition.Expression = fillExpression(concept2Tmp, condition.Expression, null);
                                        }
                                        else
                                        {
                                            condition.Expression = fillExpression(concept2Tmp, condition.Expression, null);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        condition.Expression = string.Format(Resources.Errors.ErrorData, condition.Expression);
                                        condition.ErrorDescription = e.Message + " " + e.InnerException?.Message;
                                        condition.HasError = true;
                                    }
                                }

                                modelRule.Conditions.Add(condition);
                            }
                            #endregion

                            #region Acciones

                            foreach (EnRules.RuleActionConcept actionConcept in actionConcepts)
                            {
                                EnRules.RuleAction valueActionConcept =
                                     actions.FirstOrDefault(x => x.ConceptId == actionConcept.ConceptId &&
                                                                 x.EntityId == actionConcept.EntityId &&
                                                                 x.RuleId == rule.RuleId);

                                MRules._Concept concept2Tmp =
                                     concepts.First(x => x.Entity.EntityId == actionConcept.EntityId &&
                                                         x.ConceptId == actionConcept.ConceptId);
                                MRules._ActionConcept action = new MRules._ActionConcept
                                {
                                    AssignType = Enums.AssignType.ConceptAssign,
                                    ComparatorType = Enums.ComparatorType.ConstantValue,
                                    Concept = concept2Tmp,
                                    ArithmeticOperator = valueActionConcept?.OperatorCode == null
                                         ? null
                                         : this.ConvertArithmeticOperatorDTRead((int)valueActionConcept.OperatorCode),
                                    Expression = valueActionConcept?.ActionValue
                                };

                                if (action.Expression != null)
                                {
                                    concept2Tmp = conceptsValues.First(x => x.Entity.EntityId == actionConcept.EntityId &&
                                     x.ConceptId == actionConcept.ConceptId);

                                    try
                                    {
                                        if (concept2Tmp.ConceptType == Enums.ConceptType.Reference)
                                        {
                                            List<MRules._Action> lastActions = modelRule.Actions.Where(x => x.AssignType == Enums.AssignType.ConceptAssign &&
                                                        ((MRules._Concept)((MRules._ActionConcept)x).Concept).ConceptType == Enums.ConceptType.Reference)
                                                        .ToList();

                                            if (concept2Tmp.ConceptDependences.Count() != 0)
                                                action.Expression = fillExpression(concept2Tmp, action.Expression, lastActions);
                                            else
                                                action.Expression = fillExpression(concept2Tmp, action.Expression, null);
                                        }
                                        else
                                        {
                                            action.Expression = fillExpression(concept2Tmp, action.Expression, null);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        action.Expression = string.Format(Resources.Errors.ErrorData, action.Expression);
                                        action.ErrorDescription = e.Message + " " + e.InnerException?.Message;
                                        action.HasError = true;
                                    }
                                }
                                modelRule.Actions.Add(action);
                            }
                            #endregion
                            rulesResult.Add(modelRule);
                            DataFacadeManager.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (_object)
                        {
                            if (error == null)
                            {
                                error = ex;
                            }
                        }
                    }

                });

                threads.Add(thread);
            }

            //Se procesan los hilos
            int threadRuleSet = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreadRuleSet"]);
            while (threads.Any(x => x.Status == TaskStatus.Created))
            {
                List<Task> threadsRun = threads.Where(x => x.Status == TaskStatus.Created).Skip(0).Take(threadRuleSet).ToList();
                threadsRun.ForEach(x => x.Start());
                Task.WaitAll(threadsRun.ToArray());
            }

            if (error != null)
            {
                throw new BusinessException(error.Message, error);
            }

            List<MRules._Concept> query = (from t1 in conditionConcepts
                                           join t2 in conceptsValues on new { t1.EntityId, t1.ConceptId } equals new
                                           {
                                               t2.Entity.EntityId,
                                               t2.ConceptId
                                           }
                                           orderby t1.OrderNum
                                           select t2).ToList();

            var listReturn = rulesResult.AsParallel().ToList();
            listReturn.Sort(new DecisionTableOrder(query));
            return listReturn;
        }

        private MRules._ArithmeticOperator ConvertArithmeticOperatorDTRead(int operatorType)
        {
            MRules._ArithmeticOperator result = new MRules._ArithmeticOperator();
            switch (operatorType)
            {

                case 1:  //  Assignment
                    result.ArithmeticOperatorType = Enums.ArithmeticOperatorType.Assign;
                    result.Description = "Assign";
                    result.Symbol = "=";
                    break;

                case 2:  //  Add
                    result.ArithmeticOperatorType = Enums.ArithmeticOperatorType.Add;
                    result.Description = "Add";
                    result.Symbol = "+";
                    break;

                case 3:  //  Subtract
                    result.ArithmeticOperatorType = Enums.ArithmeticOperatorType.Subtract;
                    result.Description = "Subtract";
                    result.Symbol = "-";
                    break;

                case 4:  //  Multiply
                    result.ArithmeticOperatorType = Enums.ArithmeticOperatorType.Multiply;
                    result.Description = "Multiply";
                    result.Symbol = "*";
                    break;

                case 5: // Divide
                    result.ArithmeticOperatorType = Enums.ArithmeticOperatorType.Divide;
                    result.Description = "Divide";
                    result.Symbol = "/";
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Operador no se pudo validar " + operatorType);

            }
            return result;
        }

        private int ConvertArithmeticOperatorDTWrite(Enums.ArithmeticOperatorType operatorType)
        {
            int rerult = 0;
            switch (operatorType)
            {
                case Enums.ArithmeticOperatorType.Add:
                    rerult = 2;
                    break;

                case Enums.ArithmeticOperatorType.Subtract:
                    rerult = 3;
                    break;
                case Enums.ArithmeticOperatorType.Multiply:
                    rerult = 4;
                    break;
                case Enums.ArithmeticOperatorType.Divide:
                    rerult = 5;
                    break;
                case Enums.ArithmeticOperatorType.Assign:
                    rerult = 1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Operador no se pudo validar " + operatorType);

            }
            return rerult;
        }

        private static object fillExpression(MRules._Concept concept, dynamic expression, dynamic lastConcepts)
        {
            _ConceptDao conceptDao = new _ConceptDao();

            try
            {
                if (string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression))
                {
                    return null;
                }

                switch (concept.ConceptType)
                {
                    case Enums.ConceptType.Basic:
                        MRules._BasicConcept conceptBasicTmp = concept as MRules._BasicConcept;
                        switch (conceptBasicTmp.BasicType)
                        {
                            case Enums.BasicType.Text:
                                return Convert.ToString(expression);

                            case Enums.BasicType.Numeric:
                                return (int)ConvertHelper.ConvertToDecimal(expression);

                            case Enums.BasicType.Decimal:
                                return ConvertHelper.ConvertToDecimal(expression);

                            case Enums.BasicType.Date:
                                string[] formats = new[] {
                                    "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy",
                                    "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy",
                                    "MM/dd/yyyy HH:mm:ss", "MM/dd/yyyy",
                                    "MM-dd-yyyy HH:mm:ss", "MM-dd-yyyy",
                                    "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd",
                                    "yyyy/MM/dd HH:mm:ss", "yyyy/MM/dd"};
                                DateTime returnDate;

                                if (DateTime.TryParseExact(expression, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out returnDate))
                                {
                                    return returnDate.ToString("dd/MM/yyyy");
                                }
                                throw new ArgumentOutOfRangeException("El valor " + expression + " de tipo " + conceptBasicTmp.BasicType + "no se pudo evaluar");

                            default:
                                throw new ArgumentOutOfRangeException("El valor " + expression + " de tipo " + conceptBasicTmp.BasicType + "no se pudo evaluar");
                        }
                    case Enums.ConceptType.Range:
                        return (concept as MRules._RangeConcept).RangeEntity.RangeEntityValues.First(
                            x => x.RangeValueCode == Convert.ToInt32(expression));

                    case Enums.ConceptType.List:

                        if ((concept as MRules._ListConcept).ListEntity.DescriptionList == "Booleano")
                        {
                            expression = expression == "False" || expression == "0" ? 0 : 1;
                            return (concept as MRules._ListConcept).ListEntity.ListEntityValues.First(x => x.ListValueCode == Convert.ToInt32(expression));
                        }
                        else
                        {
                            return (concept as MRules._ListConcept).ListEntity.ListEntityValues.First(x => x.ListValueCode == Convert.ToInt32(expression));
                        }

                    case Enums.ConceptType.Reference:

                        List<string> dependences = new List<string>();

                        if (lastConcepts is List<MRules._Condition> && concept.ConceptDependences.Count > 0)
                        {
                            List<MRules._Condition> lastConditions = (List<MRules._Condition>)lastConcepts;
                            foreach (MRules._ConceptDependence dependence in concept.ConceptDependences)
                            {
                                MRules._Condition condition = lastConditions.FirstOrDefault(x => ((MRules._Concept)x.Concept).ConceptId == dependence.DependsConcept.ConceptId && ((MRules._Concept)x.Concept).Entity.EntityId == dependence.DependsConcept.Entity.EntityId);
                                if (condition != null)
                                {
                                    if (condition.ComparatorType == Enums.ComparatorType.ConstantValue && condition.Comparator.Operator == Enums.OperatorConditionType.Equals)
                                    {
                                        dependences.Add(condition.Expression.Id);
                                    }
                                    else
                                    {
                                        throw new Exception($"El concepto {dependence.DependsConcept.Description} debe ser igual a un valor constante");
                                    }
                                }
                                else
                                {
                                    throw new Exception($"No se ha asignado el concepto {dependence.DependsConcept.Description}");
                                }
                            }
                        }
                        else if (lastConcepts is List<MRules._Action> && concept.ConceptDependences.Count > 0)
                        {
                            IEnumerable<MRules._ActionConcept> lastActions = ((List<MRules._Action>)lastConcepts).Cast<MRules._ActionConcept>();
                            foreach (MRules._ConceptDependence dependence in concept.ConceptDependences)
                            {
                                MRules._ActionConcept action = lastActions.FirstOrDefault(x => ((MRules._Concept)x.Concept).ConceptId == dependence.DependsConcept.ConceptId && ((MRules._Concept)x.Concept).Entity.EntityId == dependence.DependsConcept.Entity.EntityId);
                                if (action != null)
                                {
                                    if (action.ComparatorType == Enums.ComparatorType.ConstantValue && action.ArithmeticOperator.ArithmeticOperatorType == Enums.ArithmeticOperatorType.Assign)
                                    {
                                        dependences.Add(action.Expression.Id);
                                    }
                                    else
                                    {
                                        throw new Exception($"El concepto {dependence.DependsConcept.Description} debe de le debe asignar a un valor constante");
                                    }
                                }
                                else
                                {
                                    throw new Exception($"No se ha asignado el concepto {dependence.DependsConcept.Description}");
                                }
                            }
                        }

                        if (dependences.Count == concept.ConceptDependences.Count && concept.ConceptDependences.Count != 0)
                        {
                            concept = (MRules._Concept)conceptDao.GetSpecificConceptWithVales(concept.ConceptId, concept.Entity.EntityId, dependences.ToArray(), concept.ConceptType);
                        }
                        return (concept as MRules._ReferenceConcept).ReferenceValues.First(x => Convert.ToInt32(x.Id) == Convert.ToInt32(expression));

                    default:
                        throw new ArgumentOutOfRangeException("El valor " + expression + " de tipo " + concept.ConceptType + "no se pudo evaluar");

                }
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo evaluar la expresion " + expression + " para el concepto " + concept.Entity.EntityId + "-" + concept.ConceptId, e);
            }
        }

        /// <summary>
        /// Realiza la el guardado de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns></returns>
        public MRules._RuleBase CreateTableDecisionHead(MRules._RuleBase ruleBase, List<MRules._Concept> conceptCondition, List<MRules._Concept> conceptAction)
        {
            ruleBase.IsPublished = false;
            ruleBase.Version = 1;
            ruleBase.RuleEnumerator = 0;
            ruleBase.CurrentFrom = DateTime.Now.ToString("dd/MM/yyyy");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleBase.Properties.Description, typeof(EnRules.RuleBase).Name).Equal().Constant(ruleBase.Description);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EnRules.RuleBase), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                throw new ValidationException("El nombre ya esta en uso");
            }


            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    EnRules.RuleBase ruleBaseEntity = new EnRules.RuleBase
                    {
                        Description = ruleBase.Description,
                        PackageId = ruleBase.Package.PackageId,
                        LevelId = ruleBase.Level.LevelId,
                        IsEvent = ruleBase.IsEvent,
                        RuleBaseTypeCode = (int)ruleBase.RuleBaseType,
                        IsPublished = ruleBase.IsPublished,
                        RuleBaseVersion = ruleBase.Version,
                        RuleEnumerator = ruleBase.RuleEnumerator,
                        CurrentFrom = DateTime.Now
                    };

                    DataFacadeManager.Instance.GetDataFacade().InsertObject(ruleBaseEntity);
                    ruleBase.RuleBaseId = ruleBaseEntity.RuleBaseId;

                    for (int i = 0; i < conceptCondition.Count; i++)
                    {
                        MRules._Concept concept = conceptCondition[i];
                        EnRules.RuleConditionConcept ruleConditionConcept = new EnRules.RuleConditionConcept(ruleBase.RuleBaseId, concept.Entity.EntityId, concept.ConceptId)
                        {
                            OrderNum = i + 1
                        };
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(ruleConditionConcept);
                    }
                    for (int i = 0; i < conceptAction.Count; i++)
                    {
                        MRules._Concept concept = conceptAction[i];
                        EnRules.RuleActionConcept ruleActionConcept = new EnRules.RuleActionConcept(ruleBase.RuleBaseId, concept.Entity.EntityId, concept.ConceptId)
                        {
                            OrderNum = i + 1
                        };
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(ruleActionConcept);
                    }

                    transaction.Complete();

                    return ruleBase;
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new BusinessException("Error al guardar la cabecera de una tabla de decision", e);
                }
            }
        }

        /// <summary>
        /// Realiza la actualizacion de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns></returns>
        public void UpdateTableDecisionHead(MRules._RuleBase ruleBase, List<MRules._Concept> conceptCondition, List<MRules._Concept> conceptAction)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleBase.Properties.Description, typeof(EnRules.RuleBase).Name).Equal().Constant(ruleBase.Description);
            filter.And().Property(EnRules.RuleBase.Properties.RuleBaseId, typeof(EnRules.RuleBase).Name).Distinct().Constant(ruleBase.RuleBaseId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EnRules.RuleBase), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                throw new ValidationException("El nombre ya esta en uso");
            }


            using (Context.Current) using (Transaction transaction = new Transaction())
            {
                try
                {
                    filter.Clear();

                    PrimaryKey key = EnRules.RuleBase.CreatePrimaryKey(ruleBase.RuleBaseId);
                    EnRules.RuleBase ruleBaseEntity = (EnRules.RuleBase)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                    ruleBaseEntity.Description = ruleBase.Description;
                    ruleBaseEntity.RuleBaseTypeCode = (int)Enums.RuleBaseType.Option;
                    ruleBaseEntity.CurrentFrom = DateTime.Now;
                    ruleBaseEntity.RuleBaseVersion += 1;
                    ruleBaseEntity.IsPublished = false;
                    ruleBaseEntity.IsEvent = ruleBase.IsEvent;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ruleBaseEntity);


                    /*conceptos de la condicion*/
                    filter.Clear();
                    filter.Property(EnRules.RuleConditionConcept.Properties.RuleBaseId).Equal().Constant(ruleBase.RuleBaseId);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(EnRules.RuleConditionConcept), filter.GetPredicate());

                    filter.Clear();
                    filter.Property(EnRules.RuleCondition.Properties.RuleBaseId).Equal().Constant(ruleBase.RuleBaseId);
                    filter.And();
                    for (int i = 1; i <= conceptCondition.Count; i++)
                    {
                        MRules._Concept concept = conceptCondition[i - 1];
                        EnRules.RuleConditionConcept ruleConditionConcept = new EnRules.RuleConditionConcept(ruleBase.RuleBaseId, concept.Entity.EntityId, concept.ConceptId)
                        {
                            OrderNum = i
                        };
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(ruleConditionConcept);

                        filter.OpenParenthesis();
                        filter.Property(EnRules.RuleCondition.Properties.ConceptId).Distinct().Constant(concept.ConceptId);
                        filter.Or();
                        filter.Property(EnRules.RuleCondition.Properties.EntityId).Distinct().Constant(concept.Entity.EntityId);
                        filter.CloseParenthesis();
                        if (i != conceptCondition.Count)
                        {
                            filter.And();
                        }
                    }
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(EnRules.RuleCondition), filter.GetPredicate());


                    /*conceptos de la accion*/
                    filter.Clear();
                    filter.Property(EnRules.RuleActionConcept.Properties.RuleBaseId).Equal().Constant(ruleBase.RuleBaseId);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(EnRules.RuleActionConcept), filter.GetPredicate());

                    filter.Clear();
                    filter.Property(EnRules.RuleAction.Properties.RuleBaseId).Equal().Constant(ruleBase.RuleBaseId);
                    filter.And();
                    for (int i = 1; i <= conceptAction.Count; i++)
                    {
                        MRules._Concept concept = conceptAction[i - 1];
                        EnRules.RuleActionConcept ruleActionConcept = new EnRules.RuleActionConcept(ruleBase.RuleBaseId, concept.Entity.EntityId, concept.ConceptId)
                        {
                            OrderNum = i
                        };
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(ruleActionConcept);

                        filter.OpenParenthesis();
                        filter.Property(EnRules.RuleAction.Properties.ConceptId).Distinct().Constant(concept.ConceptId);
                        filter.Or();
                        filter.Property(EnRules.RuleAction.Properties.EntityId).Distinct().Constant(concept.Entity.EntityId);
                        filter.CloseParenthesis();
                        if (i != conceptAction.Count)
                        {
                            filter.And();
                        }
                    }
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(EnRules.RuleAction), filter.GetPredicate());

                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new BusinessException("Error al actualizar la cabecera de una tabla de decision", e);
                }
            }
        }

        /// <summary>
        /// Realiza el guardado de la DT
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <param name="rulesAdd">Reglas para agregar</param>
        /// <param name="rulesEdit">reglas para editar</param>
        /// <param name="rulesDelete">Reglas para eliminar</param>
        /// <returns></returns>
        public void SaveDecisionTable(int ruleBaseId, List<MRules._Rule> rulesAdd, List<MRules._Rule> rulesEdit, List<MRules._Rule> rulesDelete)
        {
            using (Context.Current) using (Transaction transaction = new Transaction())
            {
                try
                {
                    PrimaryKey key = EnRules.RuleBase.CreatePrimaryKey(ruleBaseId);
                    EnRules.RuleBase ruleBaseEntity = (EnRules.RuleBase)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                    ruleBaseEntity.CurrentFrom = DateTime.Now;
                    ruleBaseEntity.RuleBaseVersion += 1;
                    ruleBaseEntity.IsPublished = false;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ruleBaseEntity);

                    #region insertar
                    foreach (MRules._Rule rule in rulesAdd)
                    {
                        EnRules.Rule ruleEntity = new EnRules.Rule(ruleBaseId, rule.RuleId)
                        {
                            OrderNum = 0
                        };
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(ruleEntity);

                        for (int index = 0; index < rule.Conditions.Count; index++)
                        {
                            MRules._Condition condition = rule.Conditions[index];
                            EnRules.RuleCondition conditionEntity = new EnRules.RuleCondition(ruleBaseId, rule.RuleId, index + 1)
                            {
                                EntityId = condition.Concept.Entity.EntityId,
                                ConceptId = condition.Concept.ConceptId,
                                RuleValueTypeCode = 1,
                                OrderNum = 0,
                                ComparatorCode = null,
                                CondValue = null
                            };

                            if (condition.Comparator != null)
                            {
                                conditionEntity.ComparatorCode = (int)condition.Comparator.Operator;
                                if (condition.Expression != null)
                                {
                                    conditionEntity.CondValue = this.FillValueDT(condition);
                                }
                            }

                            DataFacadeManager.Instance.GetDataFacade().InsertObject(conditionEntity);
                        }


                        for (int index = 0; index < rule.Actions.Count; index++)
                        {
                            MRules._ActionConcept action = rule.Actions[index] as MRules._ActionConcept;
                            EnRules.RuleAction actionEntity = new EnRules.RuleAction(ruleBaseId, rule.RuleId, index + 1)
                            {
                                EntityId = action.Concept.Entity.EntityId,
                                ConceptId = action.Concept.ConceptId,
                                OrderNum = 0,
                                ActionTypeCode = 1,
                                ValueTypeCode = 1,
                                OperatorCode = null,
                                ActionValue = null
                            };

                            if (action.ArithmeticOperator != null)
                            {
                                actionEntity.OperatorCode = this.ConvertArithmeticOperatorDTWrite(action.ArithmeticOperator.ArithmeticOperatorType);
                                actionEntity.ActionValue = this.FillValueDT(action);
                            }

                            DataFacadeManager.Instance.GetDataFacade().InsertObject(actionEntity);
                        }
                    }
                    #endregion

                    #region editar
                    foreach (MRules._Rule rule in rulesEdit)
                    {
                        for (int index = 0; index < rule.Conditions.Count; index++)
                        {
                            MRules._Condition condition = rule.Conditions[index];

                            key = EnRules.RuleCondition.CreatePrimaryKey(ruleBaseId, rule.RuleId, index + 1);

                            EnRules.RuleCondition conditionEntity = (EnRules.RuleCondition)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (conditionEntity != null)
                            {
                                conditionEntity.ComparatorCode = null;
                                conditionEntity.CondValue = null;


                                if (condition.Comparator != null)
                                {
                                    conditionEntity.ComparatorCode = (int)condition.Comparator.Operator;
                                    if (condition.Expression != null)
                                    {
                                        conditionEntity.CondValue = this.FillValueDT(condition);
                                    }
                                }

                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(conditionEntity);
                            }
                            else
                            {
                                conditionEntity = new EnRules.RuleCondition(ruleBaseId, rule.RuleId, index + 1)
                                {
                                    EntityId = condition.Concept.Entity.EntityId,
                                    ConceptId = condition.Concept.ConceptId,
                                    RuleValueTypeCode = 1,
                                    OrderNum = 0,
                                    ComparatorCode = null,
                                    CondValue = null
                                };

                                if (condition.Comparator != null)
                                {
                                    conditionEntity.ComparatorCode = (int)condition.Comparator.Operator;
                                    if (condition.Expression != null)
                                    {
                                        conditionEntity.CondValue = this.FillValueDT(condition);
                                    }
                                }

                                DataFacadeManager.Instance.GetDataFacade().InsertObject(conditionEntity);
                            }
                        }


                        for (int index = 0; index < rule.Actions.Count; index++)
                        {
                            MRules._ActionConcept action = rule.Actions[index] as MRules._ActionConcept;

                            key = EnRules.RuleAction.CreatePrimaryKey(ruleBaseId, rule.RuleId, index + 1);

                            EnRules.RuleAction actionEntity = (EnRules.RuleAction)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (actionEntity != null)
                            {
                                actionEntity.OperatorCode = null;
                                actionEntity.ActionValue = null;


                                if (action.ArithmeticOperator != null)
                                {
                                    actionEntity.OperatorCode = this.ConvertArithmeticOperatorDTWrite(action.ArithmeticOperator
                                            .ArithmeticOperatorType);
                                    actionEntity.ActionValue = this.FillValueDT(action);
                                }

                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(actionEntity);
                            }
                            else
                            {
                                actionEntity = new EnRules.RuleAction(ruleBaseId, rule.RuleId, index + 1)
                                {
                                    EntityId = action.Concept.Entity.EntityId,
                                    ConceptId = action.Concept.ConceptId,
                                    OrderNum = 0,
                                    ActionTypeCode = 1,
                                    ValueTypeCode = 1,
                                    OperatorCode = null,
                                    ActionValue = null
                                };

                                if (action.ArithmeticOperator != null)
                                {
                                    actionEntity.OperatorCode = this.ConvertArithmeticOperatorDTWrite(action.ArithmeticOperator.ArithmeticOperatorType);
                                    actionEntity.ActionValue = this.FillValueDT(action);
                                }

                                DataFacadeManager.Instance.GetDataFacade().InsertObject(actionEntity);
                            }
                        }
                    }
                    #endregion

                    #region Eliminar
                    foreach (MRules._Rule rule in rulesDelete)
                    {
                        for (int index = 0; index < rule.Conditions.Count; index++)
                        {
                            key = EnRules.RuleCondition.CreatePrimaryKey(ruleBaseId, rule.RuleId, index + 1);

                            EnRules.RuleCondition conditionEntity = (EnRules.RuleCondition)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (conditionEntity != null)
                                DataFacadeManager.Instance.GetDataFacade().DeleteObject(conditionEntity);
                        }


                        for (int index = 0; index < rule.Actions.Count; index++)
                        {
                            key = EnRules.RuleAction.CreatePrimaryKey(ruleBaseId, rule.RuleId, index + 1);

                            EnRules.RuleAction actionEntity = (EnRules.RuleAction)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (actionEntity != null)
                                DataFacadeManager.Instance.GetDataFacade().DeleteObject(actionEntity);
                        }

                        key = EnRules.Rule.CreatePrimaryKey(ruleBaseId, rule.RuleId);
                        EnRules.Rule ruleEntity = (EnRules.Rule)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        if (ruleEntity != null)
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(ruleEntity);
                    }

                    #endregion

                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new BusinessException("Error al guardar la tabla de decisión", e);
                }
            }
        }

        private string FillValueDT(dynamic valueObject)
        {
            _ConceptDao conceptDao = new _ConceptDao();

            MRules._Concept concept = valueObject.Concept as MRules._Concept;

            switch (concept.ConceptType)
            {
                case Enums.ConceptType.Basic:
                    MRules._BasicConcept conceptBasic = conceptDao.GetBasicConceptByIdConceptIdEntity(concept.ConceptId, concept.Entity.EntityId);
                    switch (conceptBasic.BasicType)
                    {
                        case Enums.BasicType.Decimal:
                        case Enums.BasicType.Numeric:
                            return ConvertHelper.ConvertToDecimal(valueObject.Expression).ToString();

                        case Enums.BasicType.Text:
                            return valueObject.Expression.ToString();

                        case Enums.BasicType.Date:
                            return DateTime.ParseExact(valueObject.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case Enums.ConceptType.Range:
                    return valueObject.Expression.RangeValueCode.ToString();

                case Enums.ConceptType.List:
                    return valueObject.Expression.ListValueCode.ToString();

                case Enums.ConceptType.Reference:
                    return valueObject.Expression.Id.ToString();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// publica una TD
        /// </summary>
        /// <param name="ruleBaseId">id dT a publicar</param>
        /// <param name="isEvent">si la Dt es un evento</param>
        /// <returns></returns>
        public async Task<List<int>> PublishDecisionTable(int ruleBaseId, bool isEvent)
        {
            XmlHelperWriter xmlHelperWriter = new XmlHelperWriter();
            DecisionTableValidator validateDecisionTable = new DecisionTableValidator();
            bool isValid;

            List<MRules._Rule> rulesDt = this.GetTableDecisionBody(ruleBaseId);
            List<int> rulesInvalidList = validateDecisionTable.ValidatePublish(rulesDt, out isValid);

            if (rulesInvalidList.Count != 0)
            {
                return rulesInvalidList;
            }

            using (Context.Current)
            {
                _RuleSetDao ruleSetDao = new _RuleSetDao();

                PrimaryKey key = EnRules.RuleBase.CreatePrimaryKey(ruleBaseId);
                EnRules.RuleBase ruleBaseEntity = (EnRules.RuleBase)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                MRules._RuleSet newRuleSet = new MRules._RuleSet
                {
                    RuleSetId = ruleBaseEntity.RuleBaseId,
                    Description = "DT: " + ruleBaseEntity.Description,
                    Level = new MRules._Level { LevelId = ruleBaseEntity.LevelId },
                    Package = new MRules._Package { PackageId = ruleBaseEntity.PackageId },
                    IsEvent = isEvent,
                    CurrentFrom = DateTime.Now,
                    RuleSetVer = 1,
                    Rules = rulesDt,
                    Type = Enums.RuleBaseType.Option,
                };

                var xmlBytes = await xmlHelperWriter.GetXmlByRuleSet(newRuleSet);

                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        ruleBaseEntity.RuleEnumerator = rulesDt.Count() > 0 ? rulesDt.Max(b => b.RuleId) : 0;
                        ruleBaseEntity.CurrentFrom = DateTime.Now;
                        ruleBaseEntity.IsPublished = true;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(ruleBaseEntity);


                        key = EnRules.RuleSet.CreatePrimaryKey(ruleBaseId);
                        EnRules.RuleSet ruleSetEntity = (EnRules.RuleSet)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                        if (ruleSetEntity == null) //Crear regla de DT
                        {
                            ruleSetDao.CreateRuleSet(newRuleSet, xmlBytes);
                        }
                        else //actulizar
                        {
                            ruleSetDao.UpdateRuleSet(newRuleSet, xmlBytes);
                        }

                        transaction.Complete();
                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        throw new BusinessException("Error al publicar una tabla de decisión", e);
                    }
                }
            }
            return new List<int>();
        }
    }

    public class _FileDao
    {
        /// <summary>
        /// Exporta archivo excel de la tabla de desicion 
        /// </summary>
        /// <param name="ruleBaseId">id dT a exportar</param>        
        /// <returns>ruta del archivo para descargar</returns>
        public string ExportDecisionTable(int ruleBaseId)
        {
            string pathFile = null;
            //Obtener informacion tabla de decision 
            PrimaryKey key = EnRules.RuleBase.CreatePrimaryKey(ruleBaseId);
            EnRules.RuleBase ruleBaseEntity = (EnRules.RuleBase)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            //Obtengo reglas
            List<MRules._Rule> rules = new List<MRules._Rule>();
            _RuleBaseDao _ruleBaseDao = new _RuleBaseDao();
            rules = _ruleBaseDao.GetTableDecisionBody(ruleBaseId);

            //Template archivo
            List<Template> templates = new List<Template>();
            Template template = new Template();
            List<Row> rows = new List<Row>();

            #region Cabecera Archivo
            //Cabecera del archivo condiciones-acciones
            MRules._Rule ruleHead = new MRules._Rule();
            if (rules.Count == 0)
            {
                throw new BusinessException("Tabla sin datos, no se puede descargar");
            }
            ruleHead = rules[0];
            Row rowHead = new Row { Fields = new List<Field>() };

            foreach (MRules._Condition condition in ruleHead.Conditions)
            {
                Field fieldHead = new Field();
                fieldHead.ColumnSpan = 1;
                fieldHead.Description = String.Empty;
                fieldHead.FieldType = FieldType.String;
                fieldHead.RowPosition = 1;
                rowHead.Fields.Add(fieldHead);
                fieldHead = new Field();
                fieldHead.ColumnSpan = 1;
                fieldHead.Description = condition.Concept.Description;
                fieldHead.FieldType = FieldType.String;
                fieldHead.RowPosition = 1;
                rowHead.Fields.Add(fieldHead);
                if (!(condition.Concept.ConceptType == Enums.ConceptType.Basic))
                {
                    rowHead.Fields.Add(fieldHead);
                }
            }

            foreach (MRules._ActionConcept actionConcept in ruleHead.Actions)
            {
                Field fieldHead = new Field();
                fieldHead.ColumnSpan = 1;
                fieldHead.Description = String.Empty;
                fieldHead.FieldType = FieldType.String;
                fieldHead.RowPosition = 1;
                rowHead.Fields.Add(fieldHead);
                fieldHead = new Field();
                fieldHead.ColumnSpan = 1;
                fieldHead.Description = actionConcept.Concept.Description;
                fieldHead.FieldType = FieldType.String;
                fieldHead.RowPosition = 1;
                rowHead.Fields.Add(fieldHead);
                if (!(actionConcept.Concept.ConceptType == Enums.ConceptType.Basic))
                {
                    rowHead.Fields.Add(fieldHead);
                }
            }
            rows.Add(rowHead);
            #endregion

            #region Data Archivo
            foreach (MRules._Rule rule in rules)
            {
                Row row = new Row { Fields = new List<Field>() };
                foreach (MRules._Condition condition in rule.Conditions)
                {
                    if (condition.Concept.ConceptType == Enums.ConceptType.Basic)
                    {
                        if (condition.Comparator == null && condition.Expression == null)
                        {
                            row.Fields.Add(new Field { Value = " ", FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = String.Empty, FieldType = FieldType.String });
                        }
                        else
                        {
                            row.Fields.Add(new Field { Value = condition.Comparator.Symbol, FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = condition.Expression.ToString(), FieldType = FieldType.String });
                        }
                    }
                    else
                    {
                        if (condition.Comparator == null && condition.Expression == null)
                        {
                            row.Fields.Add(new Field { Value = " ", FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = " ", FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = String.Empty, FieldType = FieldType.String });
                        }
                        else
                        {
                            row.Fields.Add(new Field { Value = condition.Comparator.Symbol, FieldType = FieldType.String });
                            if (condition.Expression is MRules._ListEntityValue)
                            {
                                row.Fields.Add(new Field { Value = condition.Expression.ListValueCode.ToString(), FieldType = FieldType.String });
                                row.Fields.Add(new Field { Value = condition.Expression.ListValue.ToString(), FieldType = FieldType.String });
                            }
                            else if (condition.Expression is MRules._ReferenceValue)
                            {
                                row.Fields.Add(new Field { Value = condition.Expression.Id.ToString(), FieldType = FieldType.String });
                                row.Fields.Add(new Field { Value = condition.Expression.Description.ToString(), FieldType = FieldType.String });
                            }
                            else if (!string.IsNullOrEmpty(condition.ErrorDescription.ToString()))
                            {
                                row.Fields.Add(new Field { Value = condition.ErrorDescription.ToString(), FieldType = FieldType.String });
                                row.Fields.Add(new Field { Value = condition.ErrorDescription.ToString(), FieldType = FieldType.String });
                            }
                            else
                            {
                                row.Fields.Add(new Field { Value = condition.Comparator.Symbol, FieldType = FieldType.String });
                                if (condition.Expression is MRules._ListEntityValue)
                                {
                                    row.Fields.Add(new Field { Value = condition.Expression.ListValueCode.ToString(), FieldType = FieldType.String });
                                    row.Fields.Add(new Field { Value = condition.Expression.ListValue.ToString(), FieldType = FieldType.String });
                                }
                                else if (condition.Expression is MRules._ReferenceValue)
                                {
                                    row.Fields.Add(new Field { Value = condition.Expression.Id.ToString(), FieldType = FieldType.String });
                                    row.Fields.Add(new Field { Value = condition.Expression.Description.ToString(), FieldType = FieldType.String });
                                }
                                else if (!string.IsNullOrEmpty(condition.ErrorDescription.ToString()))
                                {
                                    string[] msjConditionExpression = condition.Expression.ToString().Split('(', ')');
                                    row.Fields.Add(new Field { Value = condition.ErrorDescription.ToString(), FieldType = FieldType.String });
                                    row.Fields.Add(new Field { Value = msjConditionExpression.Count() > 1 ? msjConditionExpression[1] : msjConditionExpression[0], FieldType = FieldType.String });
                                }
                                else
                                {
                                    row.Fields.Add(new Field { Value = condition.Expression.RangeValueCode.ToString(), FieldType = FieldType.String });
                                    row.Fields.Add(new Field { Value = condition.Expression.FromValue.ToString() + '-' + condition.Expression.ToValue.ToString(), FieldType = FieldType.String });
                                }
                            }
                        }
                    }
                }

                foreach (MRules._ActionConcept actionConcept in rule.Actions)
                {
                    if (actionConcept.Concept.ConceptType == Enums.ConceptType.Basic)
                    {
                        if (actionConcept.ArithmeticOperator == null && actionConcept.Expression == null)
                        {
                            row.Fields.Add(new Field { Value = " ", FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = "nada", FieldType = FieldType.String });
                        }
                        else
                        {
                            row.Fields.Add(new Field { Value = actionConcept.ArithmeticOperator.Symbol, FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = actionConcept.Expression.ToString(), FieldType = FieldType.String });
                        }
                    }
                    else
                    {
                        if (actionConcept.ArithmeticOperator == null && actionConcept.Expression == null)
                        {
                            row.Fields.Add(new Field { Value = " ", FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = " ", FieldType = FieldType.String });
                            row.Fields.Add(new Field { Value = "nada", FieldType = FieldType.String });
                        }
                        else
                        {
                            row.Fields.Add(new Field { Value = actionConcept.ArithmeticOperator.Symbol, FieldType = FieldType.String });
                            if (actionConcept.Expression is MRules._ListEntityValue)
                            {
                                row.Fields.Add(new Field { Value = actionConcept.Expression.ListValueCode.ToString(), FieldType = FieldType.String });
                                row.Fields.Add(new Field { Value = actionConcept.Expression.ListValue.ToString(), FieldType = FieldType.String });
                            }
                            else if (actionConcept.Expression is MRules._ReferenceValue)
                            {
                                row.Fields.Add(new Field { Value = actionConcept.Expression.Id.ToString(), FieldType = FieldType.String });
                                row.Fields.Add(new Field { Value = actionConcept.Expression.Description.ToString(), FieldType = FieldType.String });
                            }
                            else
                            {
                                row.Fields.Add(new Field { Value = actionConcept.Expression.RangeValueCode.ToString(), FieldType = FieldType.String });
                                row.Fields.Add(new Field { Value = actionConcept.Expression.FromValue.ToString() + '-' + actionConcept.Expression.ToValue.ToString(), FieldType = FieldType.String });
                            }
                        }
                    }
                }
                rows.Add(row);
            }
            #endregion

            template.Rows = rows;
            template.Description = ruleBaseEntity.Description;
            templates.Add(template);


            USMOD.File fileDecisionTable = new USMOD.File()
            {
                Id = 1,
                Description = ruleBaseEntity.Description,
                FileType = FileType.Excel,
                Templates = templates,
                Name = ruleBaseEntity.Description
            };
            pathFile = DelegateService.utilitiesService.CreateExcelFile(fileDecisionTable);
            return pathFile;
        }

        public string ExportDecisionTables()
        {
            SelectQuery select = new SelectQuery();
            //select.MaxRows = 50;
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleBaseId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.Description, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleBaseTypeCode, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.PackageId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.LevelId, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.CurrentFrom, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleBaseVersion, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.IsPublished, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.RuleEnumerator, "r")));
            select.AddSelectValue(new SelectValue(new Column(EnRules.RuleBase.Properties.IsEvent, "r")));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.PackageId, "p")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PDescription"));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.LevelId, "l")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Levels.Properties.Description, "l"), "LDescription"));

            Join join = new Join(new ClassNameTable(typeof(EnRules.RuleBase), "r"), new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleBase.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(EnParam.Levels), "l"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnRules.RuleBase.Properties.PackageId, "r")
                    .Equal().Property(EnParam.Levels.Properties.PackageId, "l")

                    .And().Property(EnRules.RuleBase.Properties.LevelId, "r")
                    .Equal().Property(EnParam.Levels.Properties.LevelId, "l")
                    .GetPredicate()
            };
            select.Table = join;
            select.AddSortValue(new SortValue(new Column(EnRules.RuleBase.Properties.Description, "r"), SortOrderType.Ascending));

            FileDAO commonFileDAO = new FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationDecisionTables;

            USMOD.File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                //file.Name = fileName;
                List<Row> rows = new List<Row>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
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
                        fields[0].Value = reader["RuleBaseId"].ToString();
                        fields[1].Value = reader["Description"].ToString();
                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                }
                file.Templates[0].Rows = rows;
                file.Name += file.Description + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return commonFileDAO.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }
    }

    public class _RangeEntityDao
    {
        public List<MRules._RangeEntity> GetRangeEntityByFilter(string filter)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.RangeEntity.Properties.RangeEntityCode, typeof(EnRules.RangeEntity).Name).IsNotNull();

            if (!string.IsNullOrEmpty(filter))
            {
                where.And().Property(EnRules.RangeEntity.Properties.Description, typeof(EnRules.RangeEntity).Name).Like().Constant($"%{filter}%");
            }

            List<EnRules.RangeEntity> rangeEntities = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnRules.RangeEntity), where.GetPredicate()))
                .Cast<EnRules.RangeEntity>().ToList();

            return _ModelAssembler.CreateRangeEntities(rangeEntities).OrderBy(x => x.DescriptionRange).ToList();
        }

        public MRules._RangeEntity GetRangeEntityById(int rangeEntityId)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.RangeEntity.Properties.RangeEntityCode, typeof(EnRules.RangeEntity).Name).Equal().Constant(rangeEntityId);

            List<EnRules.RangeEntity> rangeEntities = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(EnRules.RangeEntity), where.GetPredicate()))
                .Cast<EnRules.RangeEntity>().ToList();

            MRules._RangeEntity rangeEntity = _ModelAssembler.CreateRangeEntity(rangeEntities.First());


            where.Clear();
            where.Property(EnRules.RangeEntityValue.Properties.RangeEntityCode, typeof(EnRules.RangeEntity).Name).Equal().Constant(rangeEntityId);
            List<EnRules.RangeEntityValue> entityValues = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(EnRules.RangeEntityValue), where.GetPredicate()))
                .Cast<EnRules.RangeEntityValue>().ToList();

            rangeEntity.RangeEntityValues = _ModelAssembler.CreateRangeEntityValues(entityValues);

            return rangeEntity;
        }

        public MRules._RangeEntity CreateRangeEntity(MRules._RangeEntity rangeEntity)
        {
            EnRules.RangeEntity entity = _EntityAssembler.CreateRangeEntity(rangeEntity);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);

            rangeEntity.RangeEntityCode = entity.RangeEntityCode;

            for (int i = 0; i < rangeEntity.RangeEntityValues.Count; i++)
            {
                MRules._RangeEntityValue entityValue = rangeEntity.RangeEntityValues[i];

                EnRules.RangeEntityValue rangeEntityValue = _EntityAssembler.CreateRangeEntityValue(entityValue);
                rangeEntityValue.RangeValueCode = i + 1;
                rangeEntityValue.RangeEntityCode = rangeEntity.RangeEntityCode;

                DataFacadeManager.Instance.GetDataFacade().InsertObject(rangeEntityValue);

                rangeEntity.RangeEntityValues[i].RangeValueCode = i + 1;
            }

            return rangeEntity;
        }

        public MRules._RangeEntityValue CreateRangeEntityValue(int rangeEntityCode, MRules._RangeEntityValue entityValue)
        {
            EnRules.RangeEntityValue rangeEntityValue = _EntityAssembler.CreateRangeEntityValue(entityValue);
            rangeEntityValue.RangeEntityCode = rangeEntityCode;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(rangeEntityValue);
            return entityValue;
        }
    }

    public class _ListEntityDao
    {
        public List<MRules._ListEntity> GetListEntityByFilter(string filter)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.ListEntity.Properties.ListEntityCode, typeof(EnRules.ListEntity).Name).IsNotNull();

            if (!string.IsNullOrEmpty(filter))
            {
                where.And().Property(EnRules.ListEntity.Properties.Description, typeof(EnRules.ListEntity).Name).Like().Constant($"%{filter}%");
            }

            List<EnRules.ListEntity> listEntities = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnRules.ListEntity), where.GetPredicate()))
                .Cast<EnRules.ListEntity>().ToList();

            return _ModelAssembler.CreateListEntities(listEntities).OrderBy(x => x.DescriptionList).ToList();
        }

        public MRules._ListEntity GetListEntityById(int listEntityId)
        {
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnRules.ListEntity.Properties.ListEntityCode, typeof(EnRules.ListEntity).Name).Equal().Constant(listEntityId);

            List<EnRules.ListEntity> ListEntities = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(EnRules.ListEntity), where.GetPredicate()))
                .Cast<EnRules.ListEntity>().ToList();

            MRules._ListEntity listEntity = _ModelAssembler.CreateListEntity(ListEntities.First());


            where.Clear();
            where.Property(EnRules.ListEntityValue.Properties.ListEntityCode, typeof(EnRules.ListEntity).Name).Equal().Constant(listEntityId);
            List<EnRules.ListEntityValue> entityValues = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(EnRules.ListEntityValue), where.GetPredicate()))
                .Cast<EnRules.ListEntityValue>().ToList();

            listEntity.ListEntityValues = _ModelAssembler.CreateListEntityValues(entityValues);

            return listEntity;
        }

        public MRules._ListEntity CreateListEntity(MRules._ListEntity listEntity)
        {
            EnRules.ListEntity entity = _EntityAssembler.CreateListEntity(listEntity);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);

            listEntity.ListEntityCode = entity.ListEntityCode;

            for (int i = 0; i < listEntity.ListEntityValues.Count; i++)
            {
                MRules._ListEntityValue entityValue = listEntity.ListEntityValues[i];

                EnRules.ListEntityValue listEntityValue = _EntityAssembler.CreateListEntityValue(entityValue);
                listEntityValue.ListValueCode = i + 1;
                listEntityValue.ListEntityCode = listEntity.ListEntityCode;

                DataFacadeManager.Instance.GetDataFacade().InsertObject(listEntityValue);

                listEntity.ListEntityValues[i].ListValueCode = i + 1;
            }

            return listEntity;
        }

        public MRules._ListEntityValue CreateListEntityValue(int listEntityCode, MRules._ListEntityValue entityValue)
        {
            EnRules.ListEntityValue listEntityValue = _EntityAssembler.CreateListEntityValue(entityValue);
            listEntityValue.ListEntityCode = listEntityCode;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(listEntityValue);
            return entityValue;
        }

        /// <summary>
        /// Obtiene reglas incorrectas
        /// </summary>
        /// <param name="idRuleSet">id de la regla</param>
        /// <param name="deserializeXml">saber si se deserializa el XML</param>
        /// <returns></returns>
        public MRules._RuleSet GetRuleSetByIdRuleSet(int idRuleSet, bool deserializeXml)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EnRules.RuleSet.Properties.RuleSetId, typeof(EnRules.RuleSet).Name).Equal().Constant(idRuleSet);
            EnRules.RuleSet businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(EnRules.RuleSet), filter.GetPredicate()))
                .Cast<EnRules.RuleSet>().First();
            MRules._RuleSet modelRuleSet = _ModelAssembler.CreateRuleSet(businessCollection);
            if (deserializeXml)
            {
                modelRuleSet.Rules = XmlHelperReader.GetRuleSetByXml(businessCollection.RuleSetXml).Rules;
            }

            return modelRuleSet;
        }
    }
}
