using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EnParam = Sistran.Core.Application.Parameters.Entities;
using EnPolicies = Sistran.Core.Application.AuthorizationPolicies.Entities;
using EnUUser = Sistran.Core.Application.UniqueUser.Entities;
using Model = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using MUUser = Sistran.Core.Application.UniqueUserServices.Models;
using Encomm = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class GroupPoliciesDao
    {
        /// <summary>
        /// obtiene los grupos de politicas
        /// </summary>
        /// <returns></returns>
        public List<Models.GroupPolicies> GetGroupsPolicies()
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Description, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.PackageId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Key, "gp")));

            select.AddSelectValue(new SelectValue(new Column(EnUUser.Modules.Properties.Description, "m"), "ModuleDescription"));
            select.AddSelectValue(new SelectValue(new Column(EnUUser.Submodules.Properties.Description, "sm"), "SubModuleDescription"));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PackageDescription"));

            Join join = new Join(new ClassNameTable(typeof(EnPolicies.GroupPolicies), "gp"), new ClassNameTable(typeof(EnUUser.Modules), "m"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")
                    .Equal().Property(EnUUser.Modules.Properties.ModuleCode, "m").GetPredicate()
            };
            //REQ_483
            //purpose: Corrección script inner, sólo comparaba contra módulo, faltando validar contra submódulo también
            //author: Germán F. Grimaldi
            //date: 13/08/2018
            //inicio
            join = new Join(join, new ClassNameTable(typeof(EnUUser.Submodules), "sm"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")
                    .Equal().Property(EnUUser.Submodules.Properties.SubmoduleCode, "sm")
                    .And().Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")
                    .Equal().Property(EnUUser.Submodules.Properties.ModuleCode, "sm")
                    .GetPredicate()
            };
            //Fin
            join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.PackageId, "gp")
                    .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
            };

            select.Table = join;

            var result = new List<Models.GroupPolicies>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    result.Add(new Models.GroupPolicies
                    {
                        IdGroupPolicies = (int)reader["GroupPoliciesId"],
                        Module = new MUUser.Module { Id = (int)reader["ModuleId"], Description = (string)reader["ModuleDescription"] },
                        SubModule = new MUUser.SubModule { Id = (int)reader["SubmoduleId"], Description = (string)reader["SubModuleDescription"] },
                        Description = (string)reader["Description"],
                        Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PackageDescription"] },
                        Key = (string)reader["Key"]
                    });
                }
            }

            ///<summary>
            ///Se realiza el ordenamiento de datos por ID con el que se almaceno en la base de datos
            ///</summary>
            ///<author>Diego Leon</author>
            ///<date>24/08/2018</date>
            ///<purpose>REQ_#079</purpose>
            ///<returns></returns>
            return result.OrderBy(x => x.IdGroupPolicies).ToList();
        }

        /// <summary>
        /// obtiene los grupos de politicas
        /// </summary>
        /// <returns></returns>
        public List<Models.GroupPolicies> GetGroupsPoliciesByDescription(string description, int module, int subModule, string prefix)
        {
            var result = new List<Models.GroupPolicies>();
            if (description != "" && module != 0 && subModule != 0 && prefix != "")
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Description, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.PackageId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Key, "gp")));

                select.AddSelectValue(new SelectValue(new Column(EnUUser.Modules.Properties.Description, "m"), "ModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnUUser.Submodules.Properties.Description, "sm"), "SubModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PackageDescription"));

                Join join = new Join(new ClassNameTable(typeof(EnPolicies.GroupPolicies), "gp"), new ClassNameTable(typeof(EnUUser.Modules), "m"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")
                        .Equal().Property(EnUUser.Modules.Properties.ModuleCode, "m").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnUUser.Submodules), "sm"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")
                        .Equal().Property(EnUUser.Submodules.Properties.SubmoduleCode, "sm").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.PackageId, "gp")
                        .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
                };
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EnPolicies.GroupPolicies.Properties.Description, "gp").Like().Equals(description);
                where.And();
                where.Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp").Equal().Equals(module);
                where.And();
                where.Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp").Equal().Equals(subModule);
                where.And();
                where.Property(EnPolicies.GroupPolicies.Properties.Key, "gp").Like().Constant(prefix);

                select.Table = join;
                select.Where = where.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {

                    while (reader.Read())
                    {
                        result.Add(new Models.GroupPolicies
                        {
                            IdGroupPolicies = (int)reader["GroupPoliciesId"],
                            Module = new MUUser.Module { Id = (int)reader["ModuleId"], Description = (string)reader["ModuleDescription"] },
                            SubModule = new MUUser.SubModule { Id = (int)reader["SubmoduleId"], Description = (string)reader["SubModuleDescription"] },
                            Description = (string)reader["Description"],
                            Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PackageDescription"] },
                            Key = (string)reader["Key"]
                        });
                    }
                }
            }
            else if (description != "" && module == 0 && subModule == 0  && prefix == "")
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Description, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.PackageId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Key, "gp")));

                select.AddSelectValue(new SelectValue(new Column(EnUUser.Modules.Properties.Description, "m"), "ModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnUUser.Submodules.Properties.Description, "sm"), "SubModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PackageDescription"));

                Join join = new Join(new ClassNameTable(typeof(EnPolicies.GroupPolicies), "gp"), new ClassNameTable(typeof(EnUUser.Modules), "m"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")
                        .Equal().Property(EnUUser.Modules.Properties.ModuleCode, "m").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnUUser.Submodules), "sm"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")
                        .Equal().Property(EnUUser.Submodules.Properties.SubmoduleCode, "sm").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.PackageId, "gp")
                        .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
                };
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EnPolicies.GroupPolicies.Properties.Description, "gp").Like().Constant("%" + description + "%");

                select.Table = join;
                select.Where = where.GetPredicate();

               
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {

                    while (reader.Read())
                    {
                        result.Add(new Models.GroupPolicies
                        {
                            IdGroupPolicies = (int)reader["GroupPoliciesId"],
                            Module = new MUUser.Module { Id = (int)reader["ModuleId"], Description = (string)reader["ModuleDescription"] },
                            SubModule = new MUUser.SubModule { Id = (int)reader["SubmoduleId"], Description = (string)reader["SubModuleDescription"] },
                            Description = (string)reader["Description"],
                            Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PackageDescription"] },
                            Key = (string)reader["Key"]
                        });
                    }
                }
            }
            else if (description == "" && module != 0 && subModule == 0 && prefix == "")
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Description, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.PackageId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Key, "gp")));

                select.AddSelectValue(new SelectValue(new Column(EnUUser.Modules.Properties.Description, "m"), "ModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnUUser.Submodules.Properties.Description, "sm"), "SubModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PackageDescription"));

                Join join = new Join(new ClassNameTable(typeof(EnPolicies.GroupPolicies), "gp"), new ClassNameTable(typeof(EnUUser.Modules), "m"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")
                        .Equal().Property(EnUUser.Modules.Properties.ModuleCode, "m").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnUUser.Submodules), "sm"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")
                        .Equal().Property(EnUUser.Submodules.Properties.SubmoduleCode, "sm").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.PackageId, "gp")
                        .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
                };
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp").Equal().Constant(module);

                select.Table = join;
                select.Where = where.GetPredicate();
               
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {

                    while (reader.Read())
                    {
                        result.Add(new Models.GroupPolicies
                        {
                            IdGroupPolicies = (int)reader["GroupPoliciesId"],
                            Module = new MUUser.Module { Id = (int)reader["ModuleId"], Description = (string)reader["ModuleDescription"] },
                            SubModule = new MUUser.SubModule { Id = (int)reader["SubmoduleId"], Description = (string)reader["SubModuleDescription"] },
                            Description = (string)reader["Description"],
                            Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PackageDescription"] },
                            Key = (string)reader["Key"]
                        });
                    }
                }
            }            
            else if (description != "" && module != 0 && subModule != 0 && prefix != "")
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Description, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.PackageId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Key, "gp")));

                select.AddSelectValue(new SelectValue(new Column(EnUUser.Modules.Properties.Description, "m"), "ModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnUUser.Submodules.Properties.Description, "sm"), "SubModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PackageDescription"));

                Join join = new Join(new ClassNameTable(typeof(EnPolicies.GroupPolicies), "gp"), new ClassNameTable(typeof(EnUUser.Modules), "m"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")
                        .Equal().Property(EnUUser.Modules.Properties.ModuleCode, "m").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnUUser.Submodules), "sm"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")
                        .Equal().Property(EnUUser.Submodules.Properties.SubmoduleCode, "sm").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.PackageId, "gp")
                        .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
                };
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp").Equal().Constant(module);
                where.And();
                where.Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp").Equal().Constant(subModule);
                select.Table = join;
                select.Where = where.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {

                    while (reader.Read())
                    {
                        result.Add(new Models.GroupPolicies
                        {
                            IdGroupPolicies = (int)reader["GroupPoliciesId"],
                            Module = new MUUser.Module { Id = (int)reader["ModuleId"], Description = (string)reader["ModuleDescription"] },
                            SubModule = new MUUser.SubModule { Id = (int)reader["SubmoduleId"], Description = (string)reader["SubModuleDescription"] },
                            Description = (string)reader["Description"],
                            Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PackageDescription"] },
                            Key = (string)reader["Key"]
                        });
                    }
                }
            }
            else
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Description, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.PackageId, "gp")));
                select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Key, "gp")));

                select.AddSelectValue(new SelectValue(new Column(EnUUser.Modules.Properties.Description, "m"), "ModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnUUser.Submodules.Properties.Description, "sm"), "SubModuleDescription"));
                select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p"), "PackageDescription"));

                Join join = new Join(new ClassNameTable(typeof(EnPolicies.GroupPolicies), "gp"), new ClassNameTable(typeof(EnUUser.Modules), "m"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")
                        .Equal().Property(EnUUser.Modules.Properties.ModuleCode, "m").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnUUser.Submodules), "sm"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")
                        .Equal().Property(EnUUser.Submodules.Properties.SubmoduleCode, "sm").GetPredicate()
                };
                join = new Join(join, new ClassNameTable(typeof(EnParam.Package), "p"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.PackageId, "gp")
                        .Equal().Property(EnParam.Package.Properties.PackageId, "p").GetPredicate()
                };
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EnPolicies.GroupPolicies.Properties.Key, "gp").Like().Constant("%" + prefix +"%");

                select.Table = join;
                select.Where = where.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {

                    while (reader.Read())
                    {
                        result.Add(new Models.GroupPolicies
                        {
                            IdGroupPolicies = (int)reader["GroupPoliciesId"],
                            Module = new MUUser.Module { Id = (int)reader["ModuleId"], Description = (string)reader["ModuleDescription"] },
                            SubModule = new MUUser.SubModule { Id = (int)reader["SubmoduleId"], Description = (string)reader["SubModuleDescription"] },
                            Description = (string)reader["Description"],
                            Package = new MRules._Package { PackageId = (int)reader["PackageId"], Description = (string)reader["PackageDescription"] },
                            Key = (string)reader["Key"]
                        });
                    }
                }
            }

            return result.OrderBy(x => x.Description).ToList();
        }

        /// <summary>
        /// Obtiene el grupo de politica a partir de la politica id
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <returns></returns>
        public Models.GroupPolicies GetGroupPoliciesByidPolicies(int idPolicies)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Description, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.ModuleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.SubmoduleId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.PackageId, "gp")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.GroupPolicies.Properties.Key, "gp")));

            Join join = new Join(new ClassNameTable(typeof(EnPolicies.GroupPolicies), "gp"), new ClassNameTable(typeof(EnPolicies.Policies), "p"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(EnPolicies.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal().Property(EnPolicies.Policies.Properties.GroupPoliciesId, "p").GetPredicate());

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnPolicies.Policies.Properties.PoliciesId, "p").Equal().Constant(idPolicies);

            select.Table = join;
            select.Where = where.GetPredicate();

            var result = new List<Models.GroupPolicies>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    result.Add(new Models.GroupPolicies
                    {
                        IdGroupPolicies = (int)reader["GroupPoliciesId"],
                        Module = new MUUser.Module { Id = (int)reader["ModuleId"] },
                        SubModule = new MUUser.SubModule { Id = (int)reader["SubmoduleId"] },
                        Description = (string)reader["Description"],
                        Package = new MRules._Package { PackageId = (int)reader["PackageId"] },
                        Key = (string)reader["Key"]
                    });
                }
            }

            return result.First();
        }

        /// <summary>
        /// Elimina un grupo de Politicas
        /// </summary>
        /// <param name="GroupPoliciesId"></param>
        public List<Models.PoliciesAut> ValidGroupPolicies(int groupPoliciesId)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.Policies.Properties.PoliciesId, "lb")));
            select.AddSelectValue(new SelectValue(new Column(EnPolicies.Policies.Properties.Description, "lb")));
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnPolicies.Policies.Properties.GroupPoliciesId, "lb").Equal().Constant(groupPoliciesId);
            select.Table = new ClassNameTable(typeof(EnPolicies.Policies), "lb");
            select.Where = where.GetPredicate();
            List<Models.PoliciesAut> result = new List<Models.PoliciesAut>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    result.Add(new Models.PoliciesAut
                    {
                        IdPolicies = (int)reader["PoliciesId"],
                        Description = (string)reader["Description"]
                    });
                }
            }            
            return result;
            
            

        }

        /// <summary>
        /// Elimina un grupo de Politicas
        /// </summary>
        /// <param name="GroupPoliciesId"></param>
        public void DeleteGroupPolicies(int groupPoliciesId)
        {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(EnPolicies.GroupPolicies.Properties.GroupPoliciesId).Equal().Constant(groupPoliciesId);
                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(EnPolicies.GroupPolicies), filter.GetPredicate());                   
        }

        /// <summary>
        /// Crea un grupo de Politicas a partir del modelo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        public void CreateGroupPolicies(Model.GroupPolicies groupPolicy)
        {
            var groupPolicies = new EnPolicies.GroupPolicies();
            groupPolicies.ModuleId = groupPolicy.Module.Id;
            groupPolicies.SubmoduleId = groupPolicy.SubModule.Id;
            groupPolicies.Description = groupPolicy.Description;
            groupPolicies.PackageId = groupPolicy.Package.PackageId;
            groupPolicies.Key = groupPolicy.Key;

            DataFacadeManager.Instance.GetDataFacade().InsertObject(groupPolicies);
        }

        /// <summary>
        /// Actualiza un grupo de Politicas a partir del modelo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        public void UpdateGroupPolicies(Model.GroupPolicies groupPolicy)
        {
            PrimaryKey key = EnPolicies.GroupPolicies.CreatePrimaryKey(groupPolicy.IdGroupPolicies);
            EnPolicies.GroupPolicies groupPolicies = (EnPolicies.GroupPolicies)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            groupPolicies.ModuleId = groupPolicy.Module.Id;
            groupPolicies.SubmoduleId = groupPolicy.SubModule.Id;
            groupPolicies.Description = groupPolicy.Description;
            groupPolicies.PackageId = groupPolicy.Package.PackageId;
            groupPolicies.Key = groupPolicy.Key;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(groupPolicies);
        }

        /// <summary>
        /// Actualiza un grupo de Politicas a partir del modelo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        /// <summary>
        /// obtiene los grupos de politicas
        /// </summary>
        /// <returns></returns>
        public List<Models.CoveredRisk> GetCoveredRiskType(int Prefix)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(Encomm.LineBusinessCoveredRiskType.Properties.LineBusinessCode, "lb")));
            select.AddSelectValue(new SelectValue(new Column(Encomm.LineBusinessCoveredRiskType.Properties.CoveredRiskTypeCode, "lb")));
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(Encomm.LineBusinessCoveredRiskType.Properties.LineBusinessCode, "lb").Equal().Constant(Prefix);
            select.Table = new ClassNameTable(typeof(Encomm.LineBusinessCoveredRiskType), "lb");
            select.Where = where.GetPredicate();
            List<Models.LbCoveredRiskType> result = new List<Models.LbCoveredRiskType>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    result.Add(new Models.LbCoveredRiskType
                    {
                        LineBusiness = (int)reader["LineBusinessCode"],
                        CoveredRiskType = (int)reader["CoveredRiskTypeCode"]
                    });
                }
            }    
            List<Models.CoveredRisk> resultCoveredRiskType = new List<Models.CoveredRisk>();
            foreach (var CoveredRisk in result)
            {
                SelectQuery selectCovered = new SelectQuery();
                selectCovered.AddSelectValue(new SelectValue(new Column(EnParam.CoveredRiskType.Properties.CoveredRiskTypeCode, "cr")));
                selectCovered.AddSelectValue(new SelectValue(new Column(EnParam.CoveredRiskType.Properties.SmallDescription, "cr")));
                ObjectCriteriaBuilder whereCovered = new ObjectCriteriaBuilder();
                whereCovered.Property(EnParam.CoveredRiskType.Properties.CoveredRiskTypeCode, "cr").Equal().Constant(CoveredRisk.CoveredRiskType);
                selectCovered.Table = new ClassNameTable(typeof(EnParam.CoveredRiskType), "cr");
                selectCovered.Where = whereCovered.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectCovered))
                {
                    while (reader.Read())
                    {
                        resultCoveredRiskType.Add(new Models.CoveredRisk
                        {
                            Id = (int)reader["CoveredRiskTypeCode"],
                            Description = (string)reader["SmallDescription"]
                        });
                    }
                }

            }
            return resultCoveredRiskType;
        }
    }
}
