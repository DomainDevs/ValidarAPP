using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CoLimitsRcDAO
    {
        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name=""></param>
        /// <returns>Lista Limit RC</returns>
        public List<Model.LimitRc> GetLimitsRc()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoLimitsRc)));

            return ModelAssembler.CreateLimitsRC(businessCollection).OrderBy(x => x.Description).ToList();
        }

        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="productId">Id producto</param>
        /// <param name="policyTypeId">Id tipo de poliza</param>
        /// <returns></returns>
        public List<Model.LimitRc> GetLimitsRcByPrefixIdProductIdPolicyTypeId(int prefixId, int productId, int policyTypeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(CoLimitsRcRel.Properties.PrefixCode, typeof(CoLimitsRcRel).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(CoLimitsRcRel.Properties.ProductId, typeof(CoLimitsRcRel).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(CoLimitsRcRel.Properties.PolicyTypeCode, typeof(CoLimitsRcRel).Name);
            filter.Equal();
            filter.Constant(policyTypeId);

            LimitRCView view = new LimitRCView();
            ViewBuilder builder = new ViewBuilder("LimitRCView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.LimitsRC.Count > 0)
            {
                List<Model.LimitRc> limitsRC = ModelAssembler.CreateLimitsRC(view.LimitsRC);
                List<Model.LimitRCRelation> limitRCRelations = ModelAssembler.CreateLimitRCRelations(view.LimitRCRelations);

                foreach (Model.LimitRc item in limitsRC)
                {
                    item.IsDefault = limitRCRelations.First(x => x.Id == item.Id).IsDefault;
                    item.LimitSum = item.Limit1 + item.Limit3;
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetLimitsRcByPrefixIdProductIdPolicyTypeId");
                return limitsRC;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetLimitsRcByPrefixIdProductIdPolicyTypeId");
                return null;
            }
        }

        /// <summary>
        /// Obtener limite RC por identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Limite RC</returns>
        public Model.LimitRc GetLimitRcById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = CoLimitsRc.CreatePrimaryKey(id);
            CoLimitsRc coLimitsRc = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                coLimitsRc = (CoLimitsRc)daf.GetObjectByPrimaryKey(key);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetLimitRcById");
            if (coLimitsRc != null)
                return ModelAssembler.CreateLimitRC(coLimitsRc);

            return null;
        }
    }
}