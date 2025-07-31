using Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using EnParam = Sistran.Core.Application.Parameters.Entities;
using RUEntity = Sistran.Core.Application.Script.Entities;
using RuEnum = Sistran.Core.Application.RulesScriptsServices.Enums;
using RUModels = Sistran.Core.Application.RulesScriptsServices.Models;
using UTEntity = Sistran.Core.Application.Script.Entities;
using UUEntity = Sistran.Core.Application.UniqueUser.Entities;
using UUModels = Sistran.Core.Application.UniqueUserServices.Models;


namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    using System.Threading.Tasks;
    using Entities.Views;
    using Services.UtilitiesServices.Enums;
    using Services.UtilitiesServices.Models;
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.Utilities.Enums;

    public class PoliciesDao
    {
        /// <summary>
        /// Obtiene las politicas asociadas a las reglas ejecutadas
        /// </summary>
        /// <param name="policieslist">lista de politicas con los id de las reglas que se ejecutaron</param>
        public List<Models.PoliciesAut> GetPoliciesByRules(int userId, List<Models.PoliciesAut> policieslist)
        {
            List<Models.PoliciesAut> modelPolicies = new List<Models.PoliciesAut>();

            if (policieslist.Count <= 0) return modelPolicies;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(APEntity.Policies.Properties.PoliciesId, typeof(APEntity.Policies).Name).In();
            filter.ListValue();
            policieslist.ForEach(x => filter.Constant(x.IdPolicies));
            filter.EndList();
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection<APEntity.Policies>(daf
                   .SelectObjects(typeof(APEntity.Policies), filter.GetPredicate()));
            }

            modelPolicies = ModelAssembler.CreateListPolicies(businessCollection.Cast<APEntity.Policies>().ToList());

            filter.Clear();
            filter.Property(UUEntity.UserGroup.Properties.UserId).Equal().Constant(userId);

            businessCollection.Clear();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection<UUEntity.UserGroup>(daf
                .SelectObjects(typeof(UUEntity.UserGroup), filter.GetPredicate()));
            }
            
            List<Models.UserGroupModel> usersGroup = businessCollection.Cast<UUEntity.UserGroup>().Select(x => new UserGroupModel { GroupId = x.GroupCode }).ToList();

            UserAuthorizationDao authorizationDao = new UserAuthorizationDao();
            foreach (PoliciesAut x in modelPolicies.Where(y => y.Type == Enums.TypePolicies.Authorization))
            {
                List<UserAuthorization> userAuthorizations = authorizationDao.GetUsersAutorizationByIdPoliciesIdHierarchy(x.IdPolicies, null, usersGroup);

                int maxHierarchy = -1;
                if (userAuthorizations.Any(z => z.Hierarchy.Id < x.IdHierarchyPolicy))
                {
                    maxHierarchy = userAuthorizations.Where(z => z.Hierarchy.Id < x.IdHierarchyPolicy).Max(z => z.Hierarchy.Id);
                }

                int hierarchyAut = policieslist.First(y => y.IdPolicies == x.IdPolicies).IdHierarchyAut;

                if (hierarchyAut >= x.IdHierarchyPolicy || hierarchyAut == -1)
                {
                    x.IdHierarchyAut = maxHierarchy;
                }
                else if (userAuthorizations.Count(z => z.Hierarchy.Id == hierarchyAut) != 0)
                {
                    x.IdHierarchyAut = hierarchyAut;
                }
                else
                {
                    x.IdHierarchyAut = maxHierarchy;
                }

                if (userAuthorizations.Exists(y => y.Default))
                {
                    PrimaryKey primaryKey = UUEntity.UniqueUsers.CreatePrimaryKey(userAuthorizations.First(y => y.Default).User.UserId);
                    UUEntity.UniqueUsers entityUniqueUser = (UUEntity.UniqueUsers)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                    x.UserAuthorization = new List<Models.UserAuthorization>();
                    x.UserAuthorization.Add(new Models.UserAuthorization
                    {
                        User = new UUModels.User
                        {
                            AccountName = entityUniqueUser.AccountName
                        }
                    });
                }
            }

            modelPolicies.ForEach(x => x.IdHierarchyPolicy = policieslist.First(y => y.IdPolicies == x.IdPolicies).IdHierarchyPolicy);
            modelPolicies.ForEach(x => x.ConceptsDescription = policieslist.First(y => y.IdPolicies == x.IdPolicies).ConceptsDescription);
            modelPolicies.ForEach(x => x.Message = this.BuildMessage(x));

            return modelPolicies;
        }


        private string BuildMessage(PoliciesAut policiesAut)
        {
            if (policiesAut.ConceptsDescription == null)
            {
                return policiesAut.Message;
            }

            try
            {
                List<string> values = new List<string> { string.Empty };
                values.AddRange(policiesAut.ConceptsDescription.Select(x => x.Value).ToList());
                return string.Format(policiesAut.Message, values.ToArray());
            }
            catch (Exception)
            {
                return policiesAut.Message;
            }
        }

        /// <summary>
        /// Obtiene la politica asociadas a la regla
        /// </summary>
        /// <param name="policies">politica con el id de las regla</param>
        public Models.PoliciesAut GetPoliciesByIdRules(Models.PoliciesAut policies)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(APEntity.Policies.Properties.PoliciesId, typeof(APEntity.Policies).Name).Equal().Constant(policies.IdPolicies);

            APEntity.Policies businessCollection = new BusinessCollection<APEntity.Policies>(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(APEntity.Policies), filter.GetPredicate()))
                .Cast<APEntity.Policies>().First();

            PoliciesAut modelPolicy = ModelAssembler.CreatePolicies(businessCollection);
            modelPolicy.IdHierarchyPolicy = policies.IdHierarchyPolicy;

            if (policies.IdHierarchyAut < modelPolicy.IdHierarchyAut)
            {
                modelPolicy.IdHierarchyAut = policies.IdHierarchyAut;
            }
            modelPolicy.Message = modelPolicy.Message + "  -Jerarquia:" + modelPolicy.IdHierarchyAut;
            modelPolicy.ConceptsDescription = policies.ConceptsDescription;

            return modelPolicy;
        }

        /// <summary>
        /// Obtiene los id de las reglas que se van a validar para politica
        /// </summary>
        /// <param name="idPackage">Id del paquete</param>
        /// <param name="hierarchy">Jeraquia del usuario que ejecuta</param>
        /// <param name="key">llave filtro</param>
        /// <param name="facadeType">nievel de la ejecucion</param>
        /// <returns>lista de id's de reglas</returns>
        public List<int> GetRulesToValidate(int idPackage, int hierarchy, string key, FacadeType facadeType)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.PoliciesId, "p")));
            select.AddSelectValue(new SelectValue(new Column(RUEntity.PositionEntity.Properties.EntityId, "pe")));
            select.AddSelectValue(new SelectValue(new Column(RUEntity.PositionEntity.Properties.Position, "pe")));

            Join join = new Join(new ClassNameTable(typeof(APEntity.Policies), "p"), new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.Policies.Properties.GroupPoliciesId, "p").Equal().Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(RUEntity.PositionEntity), "pe"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.GroupPolicies.Properties.PackageId, "gp").Equal().Property(RUEntity.PositionEntity.Properties.PackageId, "pe")
                .And().Property(APEntity.Policies.Properties.Position, "p").Equal().Property(RUEntity.PositionEntity.Properties.LevelId, "pe").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.GroupPolicies.Properties.PackageId, "gp").Equal().Constant(idPackage)
                .And().Property(APEntity.Policies.Properties.Enabled, "p").Equal().Constant(true)
                .And().Property(APEntity.GroupPolicies.Properties.Key, "gp").Equal().Constant(key)
                .And().Property(APEntity.Policies.Properties.HierarchyPoliciesId, "p").LessEqual().Constant(hierarchy);

            select.Table = join;
            select.Where = where.GetPredicate();
            select.AddSortValue(new SortValue(new Column(APEntity.Policies.Properties.PoliciesId, "p"), SortOrderType.Ascending));
            select.AddSortValue(new SortValue(new Column(RUEntity.PositionEntity.Properties.Position, "pe"), SortOrderType.Ascending));

            List<dynamic> rules = new List<dynamic>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    rules.Add(new
                    {
                        PoliciesId = (int)reader["PoliciesId"],
                        EntityId = (int)reader["EntityId"],
                        Position = (int)reader["Position"]
                    });
                }
            }

            int idEntity = int.Parse(Utilities.Helper.EnumHelper.GetEnumParameterValue(facadeType).ToString());
            return rules.GroupBy(x => x.PoliciesId).Select(x => x.ToList().Last()).Where(x => x.EntityId == idEntity).Select(x => (int)x.PoliciesId).ToList();
        }

        /// <summary>
        /// obtiene las de politicas del grupo 
        /// </summary>
        /// <param name="idGroup">id del grupo de politicas</param>
        /// <returns></returns>
        public List<Models.PoliciesAut> GetPoliciesByIdGroup(int idGroup)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(APEntity.Policies.Properties.GroupPoliciesId, typeof(APEntity.Policies).Name).Equal().Constant(idGroup);
            List<APEntity.Policies> businessCollection = new BusinessCollection<APEntity.Policies>(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(APEntity.Policies), filter.GetPredicate(), new[] { APEntity.Policies.Properties.Description }))
                .Cast<APEntity.Policies>().ToList();
            return ModelAssembler.CreateListPolicies(businessCollection);
        }

        /// <summary>
        /// Obtiene las politicas con su respectiva regla segun el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="idGroup">id del grupo</param>
        /// <param name="type">tipo de politica</param>
        /// <param name="position">posicion de la politica</param>
        /// <param name="filter">filtro tipo like</param>
        /// <returns></returns>
        public List<Models.PoliciesAut> GetRulesPoliciesByFilter(int? idPackage, int idGroup, int? type, int? position, string filter)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.PoliciesId, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.TypePoliciesId, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.HierarchyPoliciesId, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Description, "po"), "DescriptionPolicies"));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Position, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.NumberAut, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Message, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Enabled, "po")));

            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.ModuleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.SubmoduleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.PackageId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.Description, "gp"), "DescriptionGroup"));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.Key, "gp")));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "pa"), "DescriptionPackage"));

            select.AddSelectValue(new SelectValue(new Column(UUEntity.Modules.Properties.Description, "mo"), "DescriptionModule"));

            select.AddSelectValue(new SelectValue(new Column(UUEntity.Submodules.Properties.Description, "sm"), "DescriptionSubModule"));

            select.AddSelectValue(new SelectValue(new Column(UTEntity.RuleSet.Properties.Description, "ru"), "DescriptionRule"));

            select.AddSelectValue(new SelectValue(new Column(UTEntity.Entity.Properties.Description, "en"), "EntityDescription"));


            Join join = new Join(new ClassNameTable(typeof(APEntity.Policies), "po"), new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.Policies.Properties.GroupPoliciesId, "po")
                    .Equal().Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.Modules), "mo"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEntity.Modules.Properties.ModuleCode, "mo")
                    .Equal().Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.Submodules), "sm"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEntity.Submodules.Properties.SubmoduleCode, "sm")
                    .Equal().Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp").And()

                    .Property(UUEntity.Submodules.Properties.ModuleCode, "sm")
                    .Equal().Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "pa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnParam.Package.Properties.PackageId, "pa")
                    .Equal().Property(APEntity.GroupPolicies.Properties.PackageId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UTEntity.RuleSet), "ru"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.RuleSet.Properties.RuleSetId, "ru")
                    .Equal().Property(APEntity.Policies.Properties.PoliciesId, "po").And().

                    Property(UTEntity.RuleSet.Properties.LevelId, "ru")
                    .Equal().Property(APEntity.Policies.Properties.Position, "po").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UTEntity.PositionEntity), "pe"), JoinType.Inner)
            {

                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.PositionEntity.Properties.PackageId, "pe")
                    .Equal().Property(APEntity.GroupPolicies.Properties.PackageId, "gp").And().

                    Property(UTEntity.PositionEntity.Properties.LevelId, "pe")
                    .Equal().Property(APEntity.Policies.Properties.Position, "po").GetPredicate()
            };


            join = new Join(join, new ClassNameTable(typeof(UTEntity.Entity), "en"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.PositionEntity.Properties.EntityId, "pe")
                    .Equal().Property(UTEntity.Entity.Properties.EntityId, "en").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal().Constant(idGroup);

            if (idPackage.HasValue)
            {
                where.And().Property(APEntity.GroupPolicies.Properties.PackageId, "gp").Equal().Constant(idPackage);
            }
            if (type.HasValue)
            {
                where.And().Property(APEntity.Policies.Properties.TypePoliciesId, "po").Equal().Constant(type);
            }
            if (position.HasValue)
            {
                where.And().Property(APEntity.Policies.Properties.Position, "po").Equal().Constant(position);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                where.And().Property(APEntity.Policies.Properties.Description, "po").Like().Constant("%" + filter + "%");
            }

            select.Table = join;
            select.Where = where.GetPredicate();

            select.AddSortValue(new SortValue(new Column(APEntity.Policies.Properties.Description, "po"), SortOrderType.Ascending));
            select.AddSortValue(new SortValue(new Column(UTEntity.PositionEntity.Properties.Position, "pe"), SortOrderType.Ascending));

            List<PoliciesAut> policiesAuts = new List<Models.PoliciesAut>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    policiesAuts.Add(new Models.PoliciesAut
                    {
                        IdPolicies = (int)reader["PoliciesId"],
                        Type = (Enums.TypePolicies)(int)reader["TypePoliciesId"],
                        Description = (string)reader["DescriptionPolicies"],
                        IdHierarchyPolicy = (int)reader["HierarchyPoliciesId"],
                        Position = (int)reader["Position"],
                        NumberAut = (int)reader["NumberAut"],
                        Message = (string)reader["Message"],
                        Enabled = (bool)reader["Enabled"],
                        GroupPolicies = new Models.GroupPolicies
                        {
                            IdGroupPolicies = (int)reader["GroupPoliciesId"],
                            Module = new UUModels.Module
                            {
                                Id = (int)reader["ModuleId"],
                                Description = (string)reader["DescriptionModule"]
                            },
                            SubModule = new UUModels.SubModule
                            {
                                Id = (int)reader["SubmoduleId"],
                                Description = (string)reader["DescriptionSubModule"]
                            },
                            Package = new RUModels._Package
                            {
                                PackageId = (int)reader["PackageId"],
                                Description = (string)reader["DescriptionPackage"]
                            },
                            Description = (string)reader["DescriptionGroup"],
                            Key = (string)reader["Key"],
                            EntityDescription = (string)reader["EntityDescription"]
                        },
                        RuleSet = new RUModels._RuleSet
                        {
                            RuleSetId = (int)reader["PoliciesId"],
                            Description = (string)reader["DescriptionRule"],
                            Package = new RUModels._Package
                            {
                                PackageId = (int)reader["PackageId"],
                                Description = (string)reader["DescriptionPackage"]
                            },
                            Level = new RUModels._Level
                            {
                                LevelId = (int)reader["Position"]
                            },
                            Type = RuEnum.RuleBaseType.Sequence
                        }
                    });
                }
            }

            return policiesAuts.GroupBy(x => x.IdPolicies).Select(z => z.ToList().Last()).ToList();
        }

        /// <summary>
        /// Obtierne los tipos de politicas 
        /// </summary>
        /// <returns></returns>
        public List<Models.TypePolicies> GetTypePolicies()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                .SelectObjects(typeof(APEntity.TypePolicies), new[] { APEntity.TypePolicies.Properties.Description }));

            return ModelAssembler.CreateListTypePolicies(businessCollection.Cast<APEntity.TypePolicies>().ToList());
        }



        /// <summary>
        /// Obtiene los niveles asociados al grupo de politicas
        /// </summary>
        /// <returns></returns>
        public Dictionary<int[], string> GetLevelsByIdGroupPolicies(int idGroupPolicies, int? level)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UTEntity.PositionEntity.Properties.PackageId, "pe")));
            select.AddSelectValue(new SelectValue(new Column(UTEntity.PositionEntity.Properties.LevelId, "pe")));
            select.AddSelectValue(new SelectValue(new Column(UTEntity.Entity.Properties.Description, "e")));
            select.AddSelectValue(new SelectValue(new Column(UTEntity.Entity.Properties.EntityId, "e")));

            Join join = new Join(new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), new ClassNameTable(typeof(UTEntity.PositionEntity), "pe"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.PositionEntity.Properties.PackageId, "pe")
                    .Equal().Property(APEntity.GroupPolicies.Properties.PackageId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UTEntity.Entity), "e"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.PositionEntity.Properties.EntityId, "pe")
                    .Equal().Property(UTEntity.Entity.Properties.EntityId, "e").GetPredicate()
            };


            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal().Constant(idGroupPolicies);

            if (level.HasValue)
            {
                where.And().Property(UTEntity.PositionEntity.Properties.LevelId, "pe").Equal().Constant(level.Value);
            }

            select.Table = join;
            select.Where = where.GetPredicate();
            select.AddSortValue(new SortValue(new Column(UTEntity.PositionEntity.Properties.LevelId, "pe"), SortOrderType.Ascending));
            select.AddSortValue(new SortValue(new Column(UTEntity.PositionEntity.Properties.Position, "pe"), SortOrderType.Ascending));

            List<dynamic> position = new List<dynamic>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    position.Add(
                        new
                        {
                            EntityId = (int)reader["EntityId"],
                            LevelId = (int)reader["LevelId"],
                            PackageId = (int)reader["PackageId"],
                            Description = (string)reader["Description"]
                        });
                }
            }

            Dictionary<int[], string> result = new Dictionary<int[], string>();
            if (!level.HasValue)
            {
                result.Add(new[] { 0, (int)position.First().EntityId }, (string)position.First().Description);
                for (int i = 0; i < position.Max(x => x.LevelId); i++)
                {
                    dynamic pos = position.FindLast(x => x.LevelId == i + 1);
                    result.Add(new[] { (int)pos.LevelId, (int)pos.EntityId }, pos.Description);
                }
            }
            else
            {
                for (int i = 0; i < position.Count; i++)
                {
                    dynamic pos = position[i];
                    result.Add(new[] { i, (int)pos.EntityId }, pos.Description);
                }
            }

            return result;
        }


        /// <summary>
        /// Realiza la creacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a crear</param>
        /// <param name="ruleSet">regla a crear</param>
        public void CreateRulePolicies(Models.PoliciesAut policies, RUModels._RuleSet ruleSet)
        {
            /***SE CREA EL PAQUETE DE REGLAS*/
            ruleSet.Description = $"{policies.GroupPolicies.Description}: {policies.Description}";
            ruleSet.Level = new RUModels._Level { LevelId = policies.Position };
            ruleSet.Package = policies.GroupPolicies.Package;
            ruleSet.CurrentFrom = DateTime.Now;
            ruleSet.RuleSetVer = 1;
            ruleSet.IsEvent = true;
            ruleSet.Type = RuEnum.RuleBaseType.Sequence;

            Task<RUModels._RuleSet> rulesettask = DelegateService.RuleService._CreateRuleSet(ruleSet);
            rulesettask.Wait();

            ruleSet = rulesettask.Result;

            /****SE CREA LA POLITICA*/
            APEntity.Policies policiesEntity = new APEntity.Policies(ruleSet.RuleSetId)
            {
                GroupPoliciesId = policies.GroupPolicies.IdGroupPolicies,
                HierarchyPoliciesId = policies.IdHierarchyPolicy,
                TypePoliciesId = (int)policies.Type,
                Description = policies.Description,
                Position = policies.Position,
                NumberAut = policies.NumberAut,
                Message = policies.Message,
                Enabled = policies.Enabled
            };

            DataFacadeManager.Instance.GetDataFacade().InsertObject(policiesEntity);
        }

        /// <summary>
        /// Realiza la modificacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a modificar</param>
        /// <param name="ruleSet">regla a modificar</param>
        public void UpdateRulePolicies(Models.PoliciesAut policies, RUModels._RuleSet ruleSet)
        {
            /***SE ACTUALIZA EL PAQUETE DE REGLAS*/
            ruleSet.RuleSetId = policies.IdPolicies;
            ruleSet.Description = $"{policies.GroupPolicies.Description}: {policies.Description}";
            ruleSet.Level = new RUModels._Level { LevelId = policies.Position };
            ruleSet.Package = policies.GroupPolicies.Package;
            ruleSet.CurrentFrom = DateTime.Now;
            ruleSet.RuleSetVer = 1;
            ruleSet.IsEvent = true;
            ruleSet.Type = RuEnum.RuleBaseType.Sequence;
            DelegateService.RuleService.UpdateRuleSet(ruleSet);

            /****SE ACTUALIZA LA POLITICA*/
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(APEntity.Policies.Properties.PoliciesId, typeof(APEntity.Policies).Name).Equal().Constant(policies.IdPolicies);

            APEntity.Policies entityPolicies = new BusinessCollection<APEntity.Policies>(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(APEntity.Policies), filter.GetPredicate()))
                .Cast<APEntity.Policies>().First();

            entityPolicies.HierarchyPoliciesId = policies.IdHierarchyPolicy;
            entityPolicies.TypePoliciesId = (int)policies.Type;
            entityPolicies.Description = policies.Description;
            entityPolicies.Position = policies.Position;
            entityPolicies.Message = policies.Message;
            entityPolicies.Enabled = policies.Enabled;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityPolicies);
        }


        /// <summary>
        /// Realiza el proceso de importar la regla de la politica
        /// </summary>
        /// <param name="policies">politica a importar</param>
        /// <returns>Politica importada</returns>
        public Models.PoliciesAut ImportRulePolicies(Models.PoliciesAut policies)
        {
            try
            {
                GroupPoliciesDao groupPoliciesDao = new GroupPoliciesDao();
                GroupPolicies groupPolicies = groupPoliciesDao.GetGroupsPolicies().First(x => x.IdGroupPolicies == policies.GroupPolicies.IdGroupPolicies);

                policies.RuleSet.Package = groupPolicies.Package;
                policies.RuleSet.Level.LevelId = policies.Position;

                //// se debe crear la politica
                if (policies.IdPolicies == 0)
                {
                    policies.RuleSet.Description = policies.Description;

                    Task<RUModels._RuleSet> rulesettask = DelegateService.RuleService.ImportRuleSet(policies.RuleSet);
                    rulesettask.Wait();

                    policies.RuleSet = rulesettask.Result;

                    APEntity.Policies policiesEntity = new APEntity.Policies(policies.RuleSet.RuleSetId)
                    {
                        GroupPoliciesId = policies.GroupPolicies.IdGroupPolicies,
                        HierarchyPoliciesId = policies.IdHierarchyPolicy,
                        TypePoliciesId = (int)policies.Type,
                        Description = policies.Description,
                        Position = policies.Position,
                        NumberAut = policies.NumberAut,
                        Message = policies.Message,
                        Enabled = policies.Enabled
                    };
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(policiesEntity);
                }
                else
                {
                    Task<RUModels._RuleSet> rulesettask = DelegateService.RuleService.ImportRuleSet(policies.RuleSet);
                    rulesettask.Wait();
                    policies.RuleSet = rulesettask.Result;

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(APEntity.Policies.Properties.PoliciesId, typeof(APEntity.Policies).Name).Equal().Constant(policies.IdPolicies);

                    APEntity.Policies entityPolicies = new BusinessCollection<APEntity.Policies>(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(APEntity.Policies), filter.GetPredicate())).Cast<APEntity.Policies>().First();

                    entityPolicies.HierarchyPoliciesId = policies.IdHierarchyPolicy;
                    entityPolicies.TypePoliciesId = (int)policies.Type;
                    entityPolicies.Description = policies.Description;
                    entityPolicies.Position = policies.Position;
                    entityPolicies.Message = policies.Message;
                    entityPolicies.Enabled = policies.Enabled;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityPolicies);
                }

                policies.IdPolicies = policies.RuleSet.RuleSetId;
                return policies;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.InnerException.InnerException.Message.ToString(), ex);
            }
        }



        /// <summary>
        /// guarda la politica regla
        /// </summary>
        /// <param name="policies">politica a guardar</param>
        /// <param name="idHierarchyDt">id de la tabla de decision</param>
        /// <returns></returns>
        public void UpdateRulesPolicies(Models.PoliciesAut policies, int? idHierarchyDt)
        {
            RUModels._RuleSet ruleSet = DelegateService.RuleService.GetRuleSetByIdRuleSet(policies.IdPolicies, false);

            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ruleSet.Description = $"{policies.GroupPolicies.Description}: {policies.Description}";
                    ruleSet.Rules = null;
                    DelegateService.RuleService.UpdateRuleSet(ruleSet);

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(APEntity.Policies.Properties.PoliciesId, typeof(APEntity.Policies).Name).Equal().Constant(policies.IdPolicies);

                    APEntity.Policies entityPolicies = new BusinessCollection<APEntity.Policies>(DataFacadeManager.Instance.GetDataFacade()
                            .SelectObjects(typeof(APEntity.Policies), filter.GetPredicate()))
                        .Cast<APEntity.Policies>().First();

                    entityPolicies.HierarchyPoliciesId = policies.IdHierarchyPolicy;
                    entityPolicies.TypePoliciesId = (int)policies.Type;
                    entityPolicies.Description = policies.Description;
                    entityPolicies.Position = policies.Position;
                    entityPolicies.Message = policies.Message;
                    entityPolicies.Enabled = policies.Enabled;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityPolicies);

                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Elimina una politica y su respectiva regla
        /// </summary>
        /// <param name="idPolicies"></param>
        public void DeleteRulePolicies(int idPolicies)
        {
            IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
            IDataFacade df = dataFacadeManager.GetDataFacade();

            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(RUEntity.RuleSet.Properties.RuleSetId).Equal().Constant(idPolicies);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(RUEntity.RuleSet), filter.GetPredicate());

                    filter.Clear();
                    filter.Property(APEntity.ConceptDescription.Properties.PoliciesId).Equal().Constant(idPolicies);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(APEntity.ConceptDescription), filter.GetPredicate());

                    filter.Clear();
                    filter.Property(APEntity.UserAuthorization.Properties.PoliciesId).Equal().Constant(idPolicies);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(APEntity.UserAuthorization), filter.GetPredicate());

                    filter.Clear();
                    filter.Property(APEntity.UserNotification.Properties.PoliciesId).Equal().Constant(idPolicies);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(APEntity.UserNotification), filter.GetPredicate());

                    filter.Clear();
                    filter.Property(APEntity.Policies.Properties.PoliciesId).Equal().Constant(idPolicies);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(APEntity.Policies), filter.GetPredicate());

                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(Resources.Errors.ErrorPoliciesIsInUse);
                }
            }
        }

        public List<Models.PoliciesAut> GetPoliciesByFilter(int? groupPolicyId, int? typePolicyId, int? levelId, string name, string message, bool enabled)
        {
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.PoliciesId, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.TypePoliciesId, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.HierarchyPoliciesId, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Description, "po"), "DescriptionPolicies"));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Position, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.NumberAut, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Message, "po")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Policies.Properties.Enabled, "po")));

            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.ModuleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.SubmoduleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.PackageId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.Description, "gp"), "DescriptionGroup"));
            select.AddSelectValue(new SelectValue(new Column(APEntity.GroupPolicies.Properties.Key, "gp")));

            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "pa"), "DescriptionPackage"));

            select.AddSelectValue(new SelectValue(new Column(UUEntity.Modules.Properties.Description, "mo"), "DescriptionModule"));

            select.AddSelectValue(new SelectValue(new Column(UUEntity.Submodules.Properties.Description, "sm"), "DescriptionSubModule"));

            select.AddSelectValue(new SelectValue(new Column(UTEntity.RuleSet.Properties.Description, "ru"), "DescriptionRule"));

            select.AddSelectValue(new SelectValue(new Column(UTEntity.Entity.Properties.Description, "en"), "EntityDescription"));


            Join join = new Join(new ClassNameTable(typeof(APEntity.Policies), "po"), new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.Policies.Properties.GroupPoliciesId, "po")
                    .Equal().Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.Modules), "mo"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEntity.Modules.Properties.ModuleCode, "mo")
                    .Equal().Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.Submodules), "sm"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEntity.Submodules.Properties.SubmoduleCode, "sm")
                    .Equal().Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp").And()

                    .Property(UUEntity.Submodules.Properties.ModuleCode, "sm")
                    .Equal().Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "pa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnParam.Package.Properties.PackageId, "pa")
                    .Equal().Property(APEntity.GroupPolicies.Properties.PackageId, "gp").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UTEntity.RuleSet), "ru"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.RuleSet.Properties.RuleSetId, "ru")
                    .Equal().Property(APEntity.Policies.Properties.PoliciesId, "po").And().

                    Property(UTEntity.RuleSet.Properties.LevelId, "ru")
                    .Equal().Property(APEntity.Policies.Properties.Position, "po").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UTEntity.PositionEntity), "pe"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.PositionEntity.Properties.Position, "pe")
                    .Equal().Property(APEntity.Policies.Properties.Position, "po").And().

                    Property(UTEntity.PositionEntity.Properties.PackageId, "pe")
                    .Equal().Property(APEntity.GroupPolicies.Properties.PackageId, "gp").And().

                    Property(UTEntity.PositionEntity.Properties.LevelId, "pe")
                    .Equal().Property(APEntity.Policies.Properties.Position, "po").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UTEntity.Entity), "en"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UTEntity.PositionEntity.Properties.EntityId, "pe")
                    .Equal().Property(UTEntity.Entity.Properties.EntityId, "en").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.Policies.Properties.Enabled, "po").Equal().Constant(enabled);

            if (groupPolicyId.HasValue)
            {
                where.And().Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal().Constant(groupPolicyId.Value);
            }
            if (typePolicyId.HasValue)
            {
                where.And().Property(APEntity.Policies.Properties.TypePoliciesId, "po").Equal().Constant(typePolicyId.Value);
            }
            if (levelId.HasValue)
            {
                where.And().Property(UTEntity.PositionEntity.Properties.LevelId, "pe").Equal().Constant(levelId.Value);
            }
            if (!string.IsNullOrEmpty(name))
            {
                where.And().Property(APEntity.Policies.Properties.Description, "po").Like().Constant("%" + name + "%");
            }
            if (!string.IsNullOrEmpty(message))
            {
                where.And().Property(APEntity.Policies.Properties.Message, "po").Like().Constant("%" + message + "%");
            }

            select.Table = join;
            select.Where = where.GetPredicate();

            select.AddSortValue(new SortValue(new Column(APEntity.Policies.Properties.Description, "po"), SortOrderType.Ascending));

            List<PoliciesAut> policiesAuts = new List<Models.PoliciesAut>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    policiesAuts.Add(new Models.PoliciesAut
                    {
                        IdPolicies = (int)reader["PoliciesId"],
                        Type = (Enums.TypePolicies)(int)reader["TypePoliciesId"],
                        Description = (string)reader["DescriptionPolicies"],
                        IdHierarchyPolicy = (int)reader["HierarchyPoliciesId"],
                        Position = (int)reader["Position"],
                        NumberAut = (int)reader["NumberAut"],
                        Message = (string)reader["Message"],
                        Enabled = (bool)reader["Enabled"],
                        GroupPolicies = new Models.GroupPolicies
                        {
                            IdGroupPolicies = (int)reader["GroupPoliciesId"],
                            Module = new UUModels.Module
                            {
                                Id = (int)reader["ModuleId"],
                                Description = (string)reader["DescriptionModule"]
                            },
                            SubModule = new UUModels.SubModule
                            {
                                Id = (int)reader["SubmoduleId"],
                                Description = (string)reader["DescriptionSubModule"]
                            },
                            Package = new RUModels._Package
                            {
                                PackageId = (int)reader["PackageId"],
                                Description = (string)reader["DescriptionPackage"]
                            },
                            Description = (string)reader["DescriptionGroup"],
                            Key = (string)reader["Key"],
                            EntityDescription = (string)reader["EntityDescription"]
                        },
                        RuleSet = new RUModels._RuleSet
                        {
                            RuleSetId = (int)reader["PoliciesId"],
                            Description = (string)reader["DescriptionRule"],
                            Package = new RUModels._Package
                            {
                                PackageId = (int)reader["PackageId"],
                                Description = (string)reader["DescriptionPackage"]
                            },
                            Level = new RUModels._Level
                            {
                                LevelId = (int)reader["Position"]
                            },
                            Type = RuEnum.RuleBaseType.Sequence
                        }
                    });
                }
            }

            return policiesAuts;
        }

        /// <summary>
        /// Genera archivo de eventos
        /// </summary>
        /// <param name="policiesList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToPolicies(List<string> policiesList, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.PoliciesReport;
            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
                string key = Guid.NewGuid().ToString();
                file.FileType = FileType.CSV;
                int cont = 1;
                foreach (string item in policiesList)
                {
                    string[] str = item.Split('|');

                    List<Field> fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
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
                    fields[0].Value = cont.ToString();
                    fields[1].Value = item.Replace('|', ' ');

                    rows.Add(new Row
                    {
                        Fields = fields,
                    });

                    cont++;
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy") + key;
                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }
        public List<Models.PoliciesAut> ValidateInfringementPolicies(List<Models.PoliciesAut> infringementPolicies)
        {
            infringementPolicies = infringementPolicies.Where(x => x.Type == Enums.TypePolicies.Authorization || x.Type == Enums.TypePolicies.Restrictive).ToList();
            return infringementPolicies;
        }

        /// <summary>
        /// Elimina la información de las notificaciones por temporal 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteNotificationByTemporalId(int id, int functionId)
        {
            NameValue[] parameters = new NameValue[2];
            string temporalId = id.ToString();
            parameters[0] = new NameValue("TEMPORALID", temporalId);
            parameters[1] = new NameValue("FUNCTIONID", functionId);
            object result = null;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("AUTHO.DELETE_NOTIFICATION", parameters);
            }

            if (result != null)
            {
                return false;
            }
            return true;
        }
    }
}