using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class InsuredObjectDAO
    {
        /// <summary>
        /// Finds the specified coverage identifier.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <returns></returns>
        public static InsuredObject GetInsuredObjectByInsuredObjectId(int insuredObjectId)
        {
            PrimaryKey key = InsuredObject.CreatePrimaryKey(insuredObjectId);
            return (InsuredObject)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }


        public Model.InsuredObject GetInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(GroupInsuredObject.Properties.ProductId, typeof(GroupInsuredObject).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(GroupInsuredObject.Properties.CoverageGroupCode, typeof(GroupInsuredObject).Name);
            filter.Equal();
            filter.Constant(groupCoverageId);
            filter.And();
            filter.Property(GroupInsuredObject.Properties.InsuredObject, typeof(GroupInsuredObject).Name);
            filter.Equal();
            filter.Constant(insuredObjectId);

            InsuredObjectView view = new InsuredObjectView();
            ViewBuilder builder = new ViewBuilder("InsuredObjectView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }



            List<Model.InsuredObject> insuredObjects = ModelAssembler.CreateInsuredObjectByGroupInsuredObjects(view.InsuredObjects, view.GroupInsuredObjects);

            return insuredObjects.First();
        }

        /// <summary>
        /// Obtener lista de coberturas por producto y grupo
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverage">Id grupo cobertura</param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.InsuredObject> GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Model.InsuredObject> insuredObjects = new List<Model.InsuredObject>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(GroupInsuredObject.Properties.ProductId, typeof(GroupInsuredObject).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(GroupInsuredObject.Properties.CoverageGroupCode, typeof(GroupInsuredObject).Name);
            filter.Equal();
            filter.Constant(groupCoverageId);

            InsuredObjectView view = new InsuredObjectView();
            ViewBuilder builder = new ViewBuilder("InsuredObjectView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.InsuredObjects.Count > 0)
            {
                insuredObjects = ModelAssembler.CreateInsuredObjects(view.InsuredObjects);
                List<PRODEN.GroupInsuredObject> groupInsuredObjects = view.GroupInsuredObjects.Cast<PRODEN.GroupInsuredObject>().ToList();

                TP.Parallel.ForEach(insuredObjects, insured =>
                {
                    insured.IsMandatory = groupInsuredObjects.First(x => x.InsuredObject.Equals(insured.Id)).IsMandatory;
                    insured.IsSelected = groupInsuredObjects.First(x => x.InsuredObject.Equals(insured.Id)).IsSelected;
                });
            }
            return insuredObjects;
        }

        /// <summary>
        /// Obtiene el deducible por Id
        /// </summary>
        /// <param name="deductibleId">Identificador del deducible</param>
        /// <returns>Model de deducible</returns>
        public static Model.Coverage SetDeductibleToCoverageByDeductibleId(Model.Coverage coverage, int deductibleId)
        {
            if (deductibleId > 0)
            {
                coverage.Deductible = DeductibleDAO.GetDeductibleByCoverageIdDeductibleId(coverage.Id, deductibleId);
            }
            return coverage;
        }

        /// <summary>
        /// obtiene una lista de Model.InsuredObject a partir de un riskId
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public List<Model.InsuredObject> GetInsuredObjectsIdByRiskId(int riskId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(RiskInsuredObject.Properties.RiskId, typeof(RiskInsuredObject).Name);
            filter.Equal();
            filter.Constant(riskId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RiskInsuredObject), filter.GetPredicate()));

            List<Model.InsuredObject> insuredObjects = ModelAssembler.CreateISSInsuredObjects(businessCollection);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetInsuredObjectByRiskId");

            return insuredObjects;
        }

        /// <summary>
        /// obtiene una lista de Model.InsuredObject a partir de un riskId
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public List<Model.InsuredObject> GetInsuredObjectByRiskId(int riskId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(RiskInsuredObject.Properties.RiskId, "ri");
            filter.Equal();
            filter.Constant(riskId);

            SelectQuery selectQuery = new SelectQuery();

            selectQuery.AddSelectValue(new SelectValue(new Column(RiskInsuredObject.Properties.RiskId, "ri")));
            selectQuery.AddSelectValue(new SelectValue(new Column(RiskInsuredObject.Properties.InsuredObjectId, "ri")));
            selectQuery.AddSelectValue(new SelectValue(new Column(RiskInsuredObject.Properties.InsuredValue, "ri")));

            selectQuery.AddSelectValue(new SelectValue(new Column(InsuredObject.Properties.InsuredObjectId, "io")));
            selectQuery.AddSelectValue(new SelectValue(new Column(InsuredObject.Properties.IsDeclarative, "io")));
            selectQuery.AddSelectValue(new SelectValue(new Column(InsuredObject.Properties.Description, "io")));

            Join join = new Join(new ClassNameTable(typeof(RiskInsuredObject), "ri"), new ClassNameTable(typeof(InsuredObject), "io"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(RiskInsuredObject.Properties.InsuredObjectId, "ri")
                .Equal()
                .Property(InsuredObject.Properties.InsuredObjectId, "io")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            List<Model.InsuredObject> insuredObjects = new List<Model.InsuredObject>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    Model.InsuredObject insuredObject = new Model.InsuredObject();
                    insuredObject.Id = Convert.ToInt32(reader["InsuredObjectId"]);
                    insuredObject.Description = reader["Description"].ToString();
                    insuredObject.Amount = Convert.ToInt64(reader["InsuredValue"]);
                    insuredObject.IsDeclarative = Convert.ToBoolean(reader["IsDeclarative"]);
                    insuredObjects.Add(insuredObject);
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetInsuredObjectByRiskId");

            return insuredObjects;
        }

        /// <summary>
        /// obtiene un BusinessCollection de tipo InsuredObject a partir de un  filter y un  sort
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListInsuredObject(Predicate filter, string[] sort)
        {
            return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(InsuredObject), filter, sort));
        }

        /// <summary>
        /// obtiene una lista de Models.InsuredObject a partir del prefixId
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public static List<Models.InsuredObject> GetInsuredObjectByPrefixIdList(int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(Prefix.Properties.PrefixCode, typeof(Prefix).Name);
            filter.Equal();
            filter.Constant(prefixId);

            InsuredObjectPrefixView view = new InsuredObjectPrefixView();
            ViewBuilder builder = new ViewBuilder("InsuredObjectPrefixView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Prefix.Count > 0)
            {
                List<Model.InsuredObject> insuredObject = ModelAssembler.CreateInsuredObjects(view.InsuredObjects);

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetInsuredObjectByPrefixIdList");

                return insuredObject;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetInsuredObjectByPrefixIdList");

                return null;
            }
        }
        public List<Models.InsuredObject> GetInsuredObjects()
        {
            BusinessCollection BusinessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(InsuredObject)));
            return ModelAssembler.CreateInsuredObjects(BusinessCollection);


        }
    }
}
