using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao Profile
    /// </summary>
    public class ProfileUserDAO
    {
        /// <summary>
        /// Save Profiles
        /// </summary>
        /// <param name="profiles">List of profiles</param>
        /// <param name="userId">userId</param>
        public void CreateProfiles(List<Profile> profiles, int userId)
        {
            List<UniqueUser.Entities.ProfileUniqueUser> entities = GetProfilesByUserId(userId);
            foreach (UniqueUser.Entities.ProfileUniqueUser item in entities)
            {
                Profile profile = profiles.Where(x => x.Id == item.ProfileId).FirstOrDefault();
                if (profile == null)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                }
            }

            foreach (Profile item in profiles)
            {
                PrimaryKey key = UniqueUser.Entities.ProfileUniqueUser.CreatePrimaryKey(userId, item.Id);
                UniqueUser.Entities.ProfileUniqueUser entityProfile = (UniqueUser.Entities.ProfileUniqueUser)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                if (entityProfile == null)
                {
                    entityProfile = EntityAssembler.CreateProfileUniqueUser(item, userId);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entityProfile);
                }
            }
        }

        /// <summary>
        /// Get ProfileUniqueUser ByUserId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns> List of ProfileUniqueUser</returns>
        public List<UniqueUser.Entities.ProfileUniqueUser> GetProfilesByUserId(int userId)
        {
            List<UniqueUser.Entities.ProfileUniqueUser> entities = new List<UniqueUser.Entities.ProfileUniqueUser>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.ProfileUniqueUser), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                entities = businessCollection.Cast<UniqueUser.Entities.ProfileUniqueUser>().ToList();
            }
            return entities;
        }

        /// <summary>
        /// Valida que el usuario tenga al menos un perfil habilitado
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <returns>valor verdadero: perfil habilitado, falso: perfil deshabilitado</returns>
        public bool ValidateProfileByUserId(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(UniqueUser.Entities.Profiles.Properties.Enabled, typeof(UniqueUser.Entities.Profiles).Name);
            filter.Equal();
            filter.Constant(Convert.ToByte(true));

            UserProfileView view = new UserProfileView();
            ViewBuilder builder = new ViewBuilder("UserProfileView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<User> users = new List<User>();

            if (view.Profiles.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
