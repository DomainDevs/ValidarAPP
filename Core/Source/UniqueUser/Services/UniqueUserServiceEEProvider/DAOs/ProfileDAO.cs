using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.UtilitiesServices.Enums;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao de Perfil
    /// </summary>
    public class ProfileDAO
    {
        /// <summary>
        /// obtiene el Listado de perfiles
        /// </summary>
        /// <returns>List of Profile</returns>
        public List<Profile> GetProfilesByDescription(string description, int idProfile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (description != "")
            {
                filter.Property(UniqueUser.Entities.Profiles.Properties.Description, typeof(UniqueUser.Entities.Profiles).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            if (idProfile != 0)
            {
                filter = new ObjectCriteriaBuilder();
                filter.Property(UniqueUser.Entities.Profiles.Properties.ProfileId, typeof(UniqueUser.Entities.Profiles).Name);
                filter.Equal();
                filter.Constant(idProfile);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.Profiles), filter.GetPredicate()));
            List<Profile> profiles = Assemblers.ModelAssembler.CreateProfiles(businessCollection);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetProfilesByDescription");
            return profiles;
        }


        /// <summary>
        /// Guarda el objeto Profile
        /// </summary>
        /// <param name="profile">profile</param>
        /// <returns>bool</returns>
        public int SaveProfile(Profile profile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AccessProfileDAO accessProfileDAO = new AccessProfileDAO();

            bool addAllAccess = false;
            PrimaryKey key = UniqueUser.Entities.Profiles.CreatePrimaryKey(profile.Id);
            UniqueUser.Entities.Profiles entity = (UniqueUser.Entities.Profiles)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (entity != null)
            {
                entity.Description = profile.Description;
                entity.Enabled = profile.IsEnabled;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
            }
            else
            {
                addAllAccess = true;
                entity = Assemblers.EntityAssembler.CreateProfile(profile);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
            }
            //Coprofile- Solicitud de Jhon Nuñez para compatibilidad con R1
            PrimaryKey keyCoProfile = UniqueUser.Entities.CoProfiles.CreatePrimaryKey(entity.ProfileId);
            UniqueUser.Entities.CoProfiles entityCoProfiles = (UniqueUser.Entities.CoProfiles)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCoProfile);

            if (entityCoProfiles == null) //Para regitros antiguos que no esten creados con AccessObjects
            {
                entityCoProfiles = new UniqueUser.Entities.CoProfiles(entity.ProfileId)
                {
                    Multibranch = false, //compatibilidad con R1
                    Intermediary = false,//compatibilidad con R1
                    Quoted = false //compatibilidad con R1    
                };
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCoProfiles);
            }
            this.InsertProfileGuaranteeStatus(profile.guaranteeProfileStatus);

            //accesos modificados
            List<AccessProfile> accessModified = profile.profileAccesses;
            //accesos Asignados
            List<UniqueUser.Entities.AccessProfiles> asignedAccess = AccessProfileDAO.GetAccessProfileByProfileIdAccessId(entity.ProfileId, 0);
            //todos los accesos
            List<UniqueUser.Entities.Accesses> activeAccess = accessProfileDAO.GetEntitiesAccessObject(false);



            foreach (AccessProfile modified in accessModified)
            {
                if (modified.AccessType == (int)AccessObjectType.PERMISION)
                {
                    AccessPermissionsDAO accessPermissionsDAO = new AccessPermissionsDAO();
                    accessPermissionsDAO.SaveAccesPermission(modified);

                }
                else
                {
                    UniqueUser.Entities.AccessProfiles active = asignedAccess.Where(x => x.AccessId == modified.AccessId).FirstOrDefault();

                    if (active != null) //Si el acceso está en base, se elimina
                    {
                        if (active != null && modified.Assigned != true)
                        {
                            AccessProfileDAO.DeleteAccessProfile(active.AccessId, entity.ProfileId);
                        }
                    }
                    else
                    {
                        if (modified.Assigned != false)
                        {
                            AccessProfile access = new AccessProfile();
                            access.AccessId = modified.AccessId;
                            access.ProfileId = entity.ProfileId;
                            access.Enabled = true;
                            access.DatabaseId = (int)Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes.DataBasesEnum.DataBase1;
                            UniqueUser.Entities.AccessProfiles entityAccess = Assemblers.EntityAssembler.CreateAccessProfile(access);
                            AccessProfileDAO.CreateAccessProfile(entityAccess);

                            int profileId = modified.ProfileId;
                            int accessId = modified.AccessId;

                            NameValue[] parameters = new NameValue[2];
                            parameters[0] = new NameValue("@PROFILE_CD", profileId);
                            parameters[1] = new NameValue("@ACCESS_ID", accessId);

                            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                            {
                                dynamicDataAccess.ExecuteSPNonQuery("UU.INSERT_PROFILE_ACCESS_PERMISSION_MASSIVE", parameters);
                            }
                        }
                    }


                }
            }



            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs.SaveProfile");
            return entity.ProfileId;
        }



        /// <summary>
        /// Copia un perfil y crea uno nuevo
        /// </summary>
        /// <param name="profile">profile</param>
        /// <returns>string</returns>
        public bool CopyProfile(Profile profile)
        {
            List<Profile> profiles = GetProfilesByDescription(profile.Description, 0);
            if (profiles.Count == 0)
            {
                PrimaryKey key = UniqueUser.Entities.Profiles.CreatePrimaryKey(profile.Id);
                UniqueUser.Entities.Profiles entity = (UniqueUser.Entities.Profiles)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (entity != null)
                {
                    int sourceId = entity.ProfileId;
                    entity.Description = profile.Description;
                    entity = Assemblers.EntityAssembler.CreateProfile(profile);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                    int profileId = entity.ProfileId;

                    //Llamado a SP para agregar los accesos del perfil                    
                    NameValue[] parameters = new NameValue[2];
                    parameters[0] = new NameValue("@ID", profileId);
                    parameters[1] = new NameValue("@SOURCE_ID", sourceId);

                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        dynamicDataAccess.ExecuteSPNonQuery("UU.ADD_ACCESS_PROFILES_BY_ID", parameters);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// consulta si existe perfil con el nombre
        /// </summary>
        /// <param name="description">description</param>
        /// <returns>bool</returns>
        public bool GetProfileByDescription(string description)
        {
            bool foundProfile = false;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.Profiles.Properties.Description, typeof(UniqueUser.Entities.Profiles).Name);
            filter.Like();
            filter.Constant("%" + description + "%");
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.Profiles), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                foundProfile = true;
            }
            return foundProfile;
        }


        /// <summary>
        /// Obtiene los estados de contragarantías
        /// </summary>
        /// <returns> Listado de contragarantías </returns>
        public List<ProfileGuaranteeStatus> GetProfileGuaranteeStatus(int profileId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.ProfileGuaranteeStatus> profGuaranStatus = new List<Models.ProfileGuaranteeStatus>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.ProfileGuaranteeStatus.Properties.ProfileId, typeof(UniqueUser.Entities.ProfileGuaranteeStatus).Name);
            filter.Equal();
            filter.Constant(profileId);

            ProfileGuaranteeStatusView profileGuaranteeStatusView = new ProfileGuaranteeStatusView();
            ViewBuilder builder = new ViewBuilder("ProfileGuaranteeStatusView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, profileGuaranteeStatusView);

            if (profileGuaranteeStatusView.GuaranteeStatus.Count > 0)
            {
                profGuaranStatus = Assemblers.ModelAssembler.CreateProfilesGuaranteesStatus(profileGuaranteeStatusView.ProfileGuaranteeStatus);
            }

            return profGuaranStatus;
        }
        
        /// <summary>
        /// guarda los estados de contragarantías
        /// </summary>
        /// <returns> Listado de contragarantías </returns>
        public bool InsertProfileGuaranteeStatus(List<ProfileGuaranteeStatus> statusGuaranteeProfile)
        {
            bool resul = false;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (ProfileGuaranteeStatus item in statusGuaranteeProfile)
            {

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UniqueUser.Entities.ProfileGuaranteeStatus.Properties.ProfileId, typeof(UniqueUser.Entities.ProfileGuaranteeStatus).Name);
                filter.Equal();
                filter.Constant(item.ProfileId);
                filter.And();
                filter.Property(UniqueUser.Entities.ProfileGuaranteeStatus.Properties.GuaranteeStatusCode, typeof(UniqueUser.Entities.ProfileGuaranteeStatus).Name);
                filter.Equal();
                filter.Constant(item.StatusId);


                BusinessCollection businessCollection = new BusinessCollection(
                    DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.ProfileGuaranteeStatus), filter.GetPredicate()));
                if (businessCollection.Count > 0 )
                {
                    foreach (UniqueUser.Entities.ProfileGuaranteeStatus currencyDifferenceEntity in businessCollection.OfType<UniqueUser.Entities.ProfileGuaranteeStatus>())
                    {
                        currencyDifferenceEntity.Enabled = item.Enabled;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(currencyDifferenceEntity);
                        resul = true;
                    }
                }
                else if(item.Enabled == true && item.Id == 0)
                {
                    UniqueUser.Entities.ProfileGuaranteeStatus entityGuaranteeStatus = Assemblers.EntityAssembler.CreateProfileGuaranteeStatus(item);

                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entityGuaranteeStatus);
                    resul = true;
                }

            }

            return resul;
        }


    }
}
