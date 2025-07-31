using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using System;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.DataFacade;
using APentity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using EntParameter=Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class PackageDAO
    {
        /// <summary>
        /// crea un Package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static Package CreatePackage(Package package)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(package);
                return package;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptControl", ex);
            }            
        }

        /// <summary>
        /// actualiza un Package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static Package UpdatePackage(Package package)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(package);
                return package;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdatePackage", ex);
            }
            
        }

        /// <summary>
        /// elimina un Package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static void DeletePackage(Package package)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(package);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeletePackage", ex);
            }
            
        }

        /// <summary>
        /// obtiene un Package a partir del packageId
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static Package FindPackage(int packageId)
        {
            try
            {
                PrimaryKey key = Package.CreatePrimaryKey(packageId);
                Package package = (Package)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return package;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindPackage", ex);
            }
           
        }

        /// <summary>
        /// obtiene una lista de Package  a partir del filtro
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static BusinessCollection ListPackage(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Package), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListPackage", ex);
            }
            
        }

        /// <summary>
        /// obtiene una lista de Package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public List<Models.Package> ListPackage2()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Package)));
                return ModelAssembler.CreatePackages2(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListPackage2", ex);
            }
            
        }

        /// <summary>
        /// obtiene una lista de autho.parameters
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public List<Models.Package> GetPackagesPolicies()
        {
            try
            {
                List<Models.Package> packages = new List<Models.Package>();               
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(APentity.Parameters.Properties.PackageId, "r"), "PackageId"));              

                Join join = new Join(new ClassNameTable(typeof(APentity.Parameters), "r"), new ClassNameTable(typeof(EntParameter.Package), "p"), JoinType.Inner)
                {
                   Criteria = new ObjectCriteriaBuilder().Property(APentity.Parameters.Properties.PackageId, "r")
                  .Equal().Property(EntParameter.Package.Properties.PackageId, "p").GetPredicate()
                };

                select.Table = join;
                
                using (System.Data.IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        packages.Add(new Models.Package()
                        {
                                PackageId = (int)reader["PackageId"],
                        });
                    }
                }
                return packages;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetAuthoParameters", ex);
            }           

        }

    }
}
