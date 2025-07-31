using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Queries;
using System;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventReassignmentUserDAO
    {
        /// <summary>
        /// reasigna el evento a otro osuario
        /// </summary>
        /// <param name="Authorization">id de la autorizacion</param>
        /// <param name="IdHieriachy">id de la nueva delegacion</param>
        /// <param name="IdAuthUser">id del nuevo usuario</param>
        public void ReassignmentUserByAuthorizationIdHieriachyIdAuthUser(Models.EventAuthorization Authorization, int IdHieriachy, int IdAuthUser)
        {
            try
            {
                UpdateQuery update = new UpdateQuery();
                update.Table = new ClassNameTable(typeof(EVENTEN.CoEventReassignmentUser));
                update.ColumnValues.Add(new Column(EVENTEN.CoEventReassignmentUser.Properties.EnabledInd), new Constant(false, System.Data.DbType.Boolean));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventReassignmentUser.Properties.AuthorizationId).Equal().Constant(Authorization.AuthorizationId);

                update.Where = where.GetPredicate();

                var count = DataFacadeManager.Instance.GetDataFacade().Execute(update);

                var newEntity = new EVENTEN.CoEventReassignmentUser
                {
                    HierarEventUser = Authorization.HierachyCd,
                    EventUserId = Authorization.EventUserId,
                    AuthoUserId = Authorization.AuthUserID,
                    HierarReassUser = IdHieriachy,
                    ReassUserId = Authorization.AuthUserID,
                    ReassAuthoUserId = IdAuthUser,
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    EnabledInd = true,
                    AuthorizationId = Convert.ToInt32(Authorization.AuthorizationId),
                    EventInitial = count == 0 ? 1 : 0
                };

                DataFacadeManager.Instance.GetDataFacade().InsertObject(newEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: ReassignmentUserByAuthorizationIdHieriachyIdAuthUser", ex);
            }
        }
    }
}
