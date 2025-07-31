using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;
using PRODEN = Sistran.Core.Application.Product.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    /// <summary>
    /// 
    /// </summary>
    public class PolicyTypeDAO
    {
        /// <summary>
        ///Obtener Tipo de poliza por ramo y tipo de poliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="Id">Identificador del tipo de poliza</param>
        /// <returns></returns>
        public COMMML.PolicyType GetPolicyTypesByPrefixIdById(int prefixId, int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.CoPolicyType.CreatePrimaryKey(prefixId, id);
            COMMEN.CoPolicyType policyType = (COMMEN.CoPolicyType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            COMMML.PolicyType policyTypeModel = ModelAssembler.CreatePolicyType(policyType);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.PolicyTypeDAO.GetPolicyTypesByPrefixIdById");
            return policyTypeModel;
        }

        /// <summary>
        /// Obtener lista de tipos de póliza
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Lista de tipos de póliza</returns>
        public List<COMMML.PolicyType> GetPolicyTypesByProductId(int productId)
        {
            List<Task> task = new List<Task>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PolicyTypeView view = new PolicyTypeView();
            ViewBuilder builder = new ViewBuilder("PolicyTypeView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.CoProductPolicyType.Properties.ProductId, typeof(PRODEN.CoProductPolicyType).Name);
            filter.Equal();
            filter.Constant(productId);

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<COMMML.PolicyType> policyTypes = TP.Task.Run(() =>
            {
                return ModelAssembler.CreatePolicyTypes(view.CoPolicyTypes);
            }).Result;
            List<COMMML.ProductPolicyType> productPolicyTypes = TP.Task.Run(() =>
            {
                return ModelAssembler.CreateProductPolicyTypes(view.CoProductPolicyTypes);
            }).Result;
            TP.Parallel.ForEach(policyTypes, item =>
            {
                item.IsDefault = productPolicyTypes?.FirstOrDefault(x => x.Id == item.Id)?.IsDefault == null ? false : productPolicyTypes.FirstOrDefault(x => x.Id == item.Id).IsDefault;
            });
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetPolicyTypesByProductId");
            return policyTypes;
        }

        /// <summary>
        /// Obtener lista de tipos de póliza
        /// </summary>
        /// <param name="productId">Id Ramo Comercial</param>
        /// <returns>Lista de tipos de póliza</returns>
        public List<COMMML.PolicyType> GetPolicyTypesByPrefixId(int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.CoPolicyType.Properties.PrefixCode, typeof(COMMEN.CoPolicyType).Name);
            filter.Equal();
            filter.Constant(prefixId);
            BusinessCollection policyTypeList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoPolicyType), filter.GetPredicate()));
            List<COMMML.PolicyType> policyTypes = ModelAssembler.CreatePolicyTypes(policyTypeList);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetPolicyTypesByPrefixId");
            return policyTypes;
        }
    }
}
