using System;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.BAF;
using System.Diagnostics;
using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class BillingGroupDAO
    {
        /// <summary>
        /// Crea un grupo de facturación
        /// </summary>
        /// <param name="billingGroup">Modelo del grupo de facturación</param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public Model.BillingGroup CreateBillingGroup(Model.BillingGroup billingGroup)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int paramBillingId = 10002;

            try
            {
                Parameter paramBilling = DelegateService.commonServiceCore.GetParameterByParameterId(paramBillingId);

                if (paramBilling != null && paramBilling.NumberParameter.HasValue)
                {
                    paramBilling.NumberParameter = paramBilling.NumberParameter.Value + 1;
                    billingGroup.Id = paramBilling.NumberParameter.Value;
                }
                else
                {
                    throw new Exception();
                }

                BillingGroup billingGroupEntity = EntityAssembler.CreateBillingGroup(billingGroup);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(billingGroupEntity);
                DelegateService.commonServiceCore.UpdateParameter(paramBilling);
                Models.BillingGroup newBillingGroup = ModelAssembler.CreateBillingGroup(billingGroupEntity);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.CreateBillingGroup");

                return newBillingGroup;
            }
            catch (Exception exception)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.CreateBillingGroup");

                throw new BusinessException("CreateBillingGroup", exception);
            }
        }

        /// <summary>
        /// Obtener Grupos De Facturación
        /// </summary>
        /// <param name="description">Id o Descripción</param>
        /// <returns>Grupos De Facturación</returns>
        public List<Models.BillingGroup> GetBillingGroupsByDescription(string description)
        {
            List<Model.BillingGroup> billingGroups = new List<Model.BillingGroup>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int32 billingGroupId = 0;
            Int32.TryParse(description, out billingGroupId);

            if (billingGroupId > 0)
            {
                filter.Property(BillingGroup.Properties.BillingGroupCode, typeof(BillingGroup).Name);
                filter.Equal();
                filter.Constant(billingGroupId);
            }
            else
            {
                filter.Property(BillingGroup.Properties.Description, typeof(BillingGroup).Name);
                filter.Like();
                filter.Constant(description + "%");
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(BillingGroup), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                billingGroups = ModelAssembler.CreateBillingGroups(businessCollection);
            }

            return billingGroups;
        }

        public List<Models.BillingGroup> GetBillingGroup()
        {
            BusinessCollection billingGroup = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(BillingGroup)));
            List<Model.BillingGroup> billingGroupModel = ModelAssembler.CreateBillingGroups(billingGroup);
            return billingGroupModel;
        }
    }
}