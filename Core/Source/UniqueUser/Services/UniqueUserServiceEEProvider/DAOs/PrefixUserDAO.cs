using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.CommonService.Models;
using UUML = Sistran.Core.Application.UniqueUserServices.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class PrefixUserDAO
    {
        public void SavePrefixes(List<Prefix> prefixes, int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.PrefixUser.Properties.UserId,  userId);

            List<COMMEN.PrefixUser> entityPrefixUsers = DataFacadeManager.GetObjects(typeof(COMMEN.PrefixUser), filter.GetPredicate()).Cast<COMMEN.PrefixUser>().ToList();

            foreach (Prefix prefix in prefixes)
            {
                if (entityPrefixUsers.Exists(x => x.PrefixCode == prefix.Id))
                {
                    entityPrefixUsers.RemoveAll(x => x.PrefixCode == prefix.Id);
                }
                else
                {
                    DataFacadeManager.Insert(EntityAssembler.CreatePrefixUser(prefix, userId));
                }
            }

            if (entityPrefixUsers.Count > 0)
            {


                filter.And();
                filter.Property(COMMEN.PrefixUser.Properties.PrefixCode);
                filter.In();
                filter.ListValue();



                foreach (COMMEN.PrefixUser entityPrefixUser in entityPrefixUsers)
                {
                    filter.Constant(entityPrefixUser.PrefixCode);
                }
                filter.EndList();

                DataFacadeManager.Instance.GetDataFacade().Delete<COMMEN.PrefixUser>(filter.GetPredicate());
            }


        }
        /// <summary>
        /// Listado de Usuario Por Filtro
        /// </summary>
        /// <param name="user">Filtro</param>
        /// <returns>Users</returns>
        public List<string> GetPrefixesByUserId(int userId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //filter.Property(UniqueUserEntities.CptUniqueUserSalePointAlliance.Properties.UserId, typeof(UniqueUserEntities.CptUniqueUserSalePointAlliance).Name);
            //filter.Equal();
            //filter.Constant(userId);
            //BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUserEntities.CptUniqueUserSalePointAlliance), filter.GetPredicate()));
            //List<UniqueUserEntities.CptUniqueUserSalePointAlliance> uniqueUsersSalePointAlliance = new List<UniqueUserEntities.CptUniqueUserSalePointAlliance>();
            //List<PrefixUser> listUniqueUsersSalePointAllianceMod = new List<PrefixUser>();
            //foreach (UniqueUserEntities.CptUniqueUserSalePointAlliance item in businessCollection)
            //{
            //    if (item.IsAssign != null && item.IsAssign == true)
            //    {
            //        uniqueUsersSalePointAlliance.Add(item);
            //        //listUniqueUsersSalePointAllianceMod.Add(ModelAssembler.MappCptUniqueUserSalePointAlliance(item));
            //    }
            //}
            //if (uniqueUsersSalePointAlliance.Count >= 1)
            //{
            //    return listUniqueUsersSalePointAllianceMod;

            //}
            //else
            //{
            //    return null;
            //}

            return null;

        }

        /// <summary>
        /// SavePrefixUser
        /// </summary>
        /// <param name="estimationTypePrefix"></param>
        /// <returns>PrefixUser</returns>
        public UUML.PrefixUser SavePrefixUser(UUML.PrefixUser prefixUser)
        {
            //convertir de model a entity
            COMMEN.PrefixUser estimationTypePrefixEntity = EntityAssembler.CreatePrefixUser(prefixUser);

            //realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Instance.GetDataFacade().InsertObject(estimationTypePrefixEntity);

            //return del model
            return ModelAssembler.CreatePrefixUser(estimationTypePrefixEntity);
        }

        /// <summary>
        /// DeleteEstimationTypePrefix
        /// </summary>
        /// <param name="estimationTypePrefix"></param>
        /// <returns></returns>
        public void DeletePrefixUser(UUML.PrefixUser prefixUser)
        {
            //convertir de model a entity
            COMMEN.PrefixUser estimationTypePrefixEntity = EntityAssembler.CreatePrefixUser(prefixUser);

            //realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(estimationTypePrefixEntity);

        }

    }
}