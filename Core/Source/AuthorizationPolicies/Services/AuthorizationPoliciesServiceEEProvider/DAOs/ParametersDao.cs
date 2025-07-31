using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using EnParam = Sistran.Core.Application.Parameters.Entities;
using RUModels = Sistran.Core.Application.RulesScriptsServices.Models;
using UUEntity = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class ParametersDao
    {
        /// <summary>
        /// Consulta la jeraquia de un usuario
        /// </summary>
        /// <param name="idPackage">Id del paquete</param>
        /// <param name="idUser">id del usuario que ejecuta</param>
        /// <returns></returns>
        public int GetHierarchyByIdUser(int idPackage, int idUser)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(APEntity.Parameters.Properties.PackageId, typeof(APEntity.Parameters).Name).Equal().Constant(idPackage);
            List<APEntity.Parameters> parameters = DataFacadeManager.Instance.GetDataFacade().List(typeof(APEntity.Parameters), filter.GetPredicate()).Cast<APEntity.Parameters>().ToList();

            APEntity.Parameters parameter = parameters.FirstOrDefault();

            filter.Clear();
            filter.Property(UUEntity.CoHierarchyAccesses.Properties.ModuleCode, typeof(UUEntity.CoHierarchyAccesses).Name).Equal().Constant(parameter.ModuleId);
            filter.And().Property(UUEntity.CoHierarchyAccesses.Properties.SubmoduleCode, typeof(UUEntity.CoHierarchyAccesses).Name).Equal().Constant(parameter.SubmoduleId);
            filter.And().Property(UUEntity.CoHierarchyAccesses.Properties.UserId, typeof(UUEntity.CoHierarchyAccesses).Name).Equal().Constant(idUser);


            List<UUEntity.CoHierarchyAccesses> entityCoHierarchyAccesses = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                   .SelectObjects(typeof(UUEntity.CoHierarchyAccesses), filter.GetPredicate()))
                   .Cast<UUEntity.CoHierarchyAccesses>().ToList();

            if (entityCoHierarchyAccesses == null || entityCoHierarchyAccesses.Count == 0)
            {
                filter.Clear();
                filter.Property(UUEntity.CoHierarchyAssociation.Properties.ModuleCode, typeof(UUEntity.CoHierarchyAssociation).Name).Equal().Constant(parameter.ModuleId);
                filter.And().Property(UUEntity.CoHierarchyAssociation.Properties.SubmoduleCode, typeof(UUEntity.CoHierarchyAssociation).Name).Equal().Constant(parameter.SubmoduleId);

                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(UUEntity.CoHierarchyAssociation), filter.GetPredicate()))
                .Cast<UUEntity.CoHierarchyAssociation>().Max(x => x.HierarchyCode);
            }
            else
            {
                return entityCoHierarchyAccesses.Max(x => x.HierarchyCode);
            }
        }

        /// <summary>
        /// consulta los paquetes asociados a politicas
        /// </summary>
        /// <returns></returns>
        public List<RUModels.Package> GetPackagePolicies()
        {
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.PackageId, "p")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Description, "p")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Namespace, "p")));
            select.AddSelectValue(new SelectValue(new Column(EnParam.Package.Properties.Disabled, "p")));

            Join join = new Join(new ClassNameTable(typeof(EnParam.Package), "p"), new ClassNameTable(typeof(APEntity.Parameters), "pr"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnParam.Package.Properties.PackageId, "p")
                    .Equal().Property(APEntity.Parameters.Properties.PackageId, "pr").GetPredicate()
            };

            select.Table = join;
            select.AddSortValue(new SortValue(new Column(EnParam.Package.Properties.Description, "p"), SortOrderType.Ascending));

            var packages = new List<RUModels.Package>();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    packages.Add(new RUModels.Package
                    {
                        PackageId = (int)reader["PackageId"],
                        Description = (string)reader["Description"],
                        NameSpace = (string)reader["Namespace"],
                        Disabled = (bool)reader["Disabled"]
                    });
                }
            }

            return packages;
        }
    }
}
