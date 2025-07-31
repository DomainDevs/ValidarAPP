using System;
using System.Linq;
using Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.UtilitiesServicesEEProvider.Assemblers;
using MODEL = Sistran.Core.Services.UtilitiesServices.Models;
using TMP = Sistran.Core.Application.Temporary.Entities;

namespace Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs
{
    public class EndorsementControlDAO
    {
        /// <summary>
        /// Insert de control de endoso
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public void CreateEndorsementControl(int id, int userId)
        {
            try
            {
                EndorsementControl endorsementControlentity = new EndorsementControl();
                endorsementControlentity.Id = id;
                endorsementControlentity.UserId = userId;
                endorsementControlentity.CurrentFrom = DateTime.Now;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(endorsementControlentity);
                DataFacadeManager.Dispose();
                //return endorsementControlentity.Id;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Elimina registro de contorl de endoso
        public bool DeleteEndorsementControl(int id)
        {

            PrimaryKey key = EndorsementControl.CreatePrimaryKey(id);
            EndorsementControl endorsementControl = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementControl = (EndorsementControl)daf.GetObjectByPrimaryKey(key);
            }

            if (endorsementControl != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(endorsementControl);
                return true;
            }
            return false;
        }

        /// <summary>
        /// obtiene id de endoso 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetEndorsementControlById(int id, int userId)
        {
            PrimaryKey key = EndorsementControl.CreatePrimaryKey(id);
            EndorsementControl endorsementControl = null;
            bool result = false;
            MODEL.EndorsementControl endorsementResult = new MODEL.EndorsementControl();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementControl = (EndorsementControl)daf.GetObjectByPrimaryKey(key);
            }
            if (endorsementControl != null && ValidateDeleteTemporaryR1(endorsementControl))
            {
                //if (endorsementControl.UserId == userId)
                //{
                //    result = true;
                //}
                //else
                //    result = false;
                result = true; 
            }
            else
            {
                CreateEndorsementControl(id, userId);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// validar que por R1 no eliminen el temporal
        /// </summary>
        /// <param name="endorsementControl"></param>
        /// <returns></returns>
        public bool ValidateDeleteTemporaryR1(EndorsementControl endorsementControl)
        {

            System.Collections.Generic.List<TMP.TempSubscription> TempSubscription = null;

            Framework.Queries.ObjectCriteriaBuilder filter = new Framework.Queries.ObjectCriteriaBuilder();
            filter.Property(TMP.TempSubscription.Properties.EndorsementId, typeof(TMP.TempSubscription).Name);
            filter.Equal();
            filter.Constant(endorsementControl.Id);
            
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                TempSubscription = daf.List(typeof(TMP.TempSubscription), filter.GetPredicate()).Cast<TMP.TempSubscription>().ToList();
            }

            if (TempSubscription != null && TempSubscription.Count > 0)
                 return true;
            else
            {
                DeleteEndorsementControl(endorsementControl.Id);
                return false;
            }
        }
    }
}
