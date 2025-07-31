using Newtonsoft.Json;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.MassiveUnderwritingServices.Entities.Views;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
using MSVEN = Sistran.Core.Application.Massive.Entities;

namespace Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassiveEmissionDAO
    {
        /// <summary>
        /// Crear Cargue Masivo
        /// </summary>
        /// <param name="massiveLoad">Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        public MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.CreateMassiveLoad(massiveEmission);

            if (massiveLoad != null)
            {
                massiveEmission.Id = massiveLoad.Id;
                MSVEN.MassiveEmission entityMassiveEmission = EntityAssembler.CreateMassiveEmission(massiveEmission);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityMassiveEmission);

                if (entityMassiveEmission != null)
                {
                    return massiveEmission;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Actualizar Cargue Masivo
        /// </summary>
        /// <param name="massiveLoad"></param>
        /// <returns>Cargue Masivo</returns>
        public MassiveEmission UpdateMassiveEmission(MassiveEmission massiveEmission)
        {
            PrimaryKey primaryKeyMassiveEmision = MSVEN.MassiveEmission.CreatePrimaryKey(massiveEmission.Id);

            MSVEN.MassiveEmission entityMassiveEmission = (MSVEN.MassiveEmission)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKeyMassiveEmision);

            if (entityMassiveEmission != null)
            {
                entityMassiveEmission.AgentId = massiveEmission.Agency.Agent.IndividualId;
                entityMassiveEmission.AgencyId = massiveEmission.Agency.Id;
                entityMassiveEmission.PrefixId = massiveEmission.Prefix.Id;
                entityMassiveEmission.ProductId = massiveEmission.Product.Id;
                entityMassiveEmission.BranchId = massiveEmission.Branch.Id;

                if (massiveEmission.BillingGroupId > 0)
                {
                    entityMassiveEmission.BillingGroupId = massiveEmission.BillingGroupId;
                }

                if (massiveEmission.RequestId > 0)
                {
                    entityMassiveEmission.RequestId = massiveEmission.RequestId;
                    entityMassiveEmission.RequestNum = massiveEmission.RequestNumber;
                }

                if (massiveEmission.Branch.SalePoints != null && massiveEmission.Branch.SalePoints.Count > 0)
                {
                    entityMassiveEmission.SalePointId = massiveEmission.Branch.SalePoints[0].Id;
                }

                if (massiveEmission.BusinessType.HasValue)
                {
                    entityMassiveEmission.BusinessTypeId = (int)massiveEmission.BusinessType.Value;
                }

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityMassiveEmission);

                return massiveEmission;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Cargue por Id
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns>Cargue Masivo</returns>
        public MassiveEmission GetMassiveEmissionByMassiveLoadId(int massiveLoadId)
        {
            MassiveEmissionView view = new MassiveEmissionView();
            ViewBuilder builder = new ViewBuilder("MassiveEmissionView");

            ObjectCriteriaBuilder filterView = new ObjectCriteriaBuilder();
            filterView.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name);
            filterView.Equal();
            filterView.Constant(massiveLoadId);
            builder.Filter = filterView.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            MSVEN.MassiveLoad massiveLoadView = view.MassiveLoad.Cast<MSVEN.MassiveLoad>().FirstOrDefault();
            MSVEN.MassiveEmission massiveEmissionView = view.MassiveEmission.Cast<MSVEN.MassiveEmission>().FirstOrDefault();
            if (massiveEmissionView == null || massiveLoadView == null)
                return null;
            MassiveEmission massiveEmission = ModelAssembler.CreateMassiveEmission(massiveLoadView, massiveEmissionView);

            return massiveEmission;
        }

        #region Eventos




        /// <summary>
        /// Realiza el envio de las solicitudes de autorizacion de los eventos
        /// </summary>
        /// <param name="authorizationRequest"></param>
        /// <returns></returns>
        private Task SendAuthorizationPolicies(List<AuthorizationRequest> authorizationRequest)
        {
            return TP.Task.Run(() =>
            {
                try
                {
                    DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequest);
                }
                catch (Exception)
                {
                    //
                }
            });
        }

        
        #endregion
    }
}