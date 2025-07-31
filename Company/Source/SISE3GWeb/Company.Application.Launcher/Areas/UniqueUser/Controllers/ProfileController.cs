using Newtonsoft.Json;

using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UtilitiesServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Controllers
{
    public class ProfileController : Controller
    {
        private static List<ProfileModelsView> profiles = new List<ProfileModelsView>();
        private static List<Profile> profilesModel = new List<Profile>();

        public ActionResult Profile()
        {
            return View("Profile");
        }

        public ActionResult ProfileAdvancedSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateFileToExport()
        {
            try
            {
                GetListProfile();
                if (profilesModel.Count > 0)
                {
                    string urlFile = DelegateService.uniqueUserService.GenerateFileToProfiles(profilesModel, App_GlobalResources.Language.Profiles);

                    if (string.IsNullOrEmpty(urlFile))
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                    }
                    else
                    {
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        public ActionResult GetProfiles()
        {
            try
            {
                GetListProfile();
                return new UifJsonResult(true, profiles.Where(x => x.Enabled == true).OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProfiles);
            }
        }

        public ActionResult GetProfileByDescription(string description, int id)
        {
            try
            {
                List<ProfileModelsView> profilesByDescription = ModelAssembler.CreateProfiles(DelegateService.uniqueUserService.GetProfilesByDescription(description, id));
                if (profilesByDescription.Count > 0)
                {
                    return new UifJsonResult(true, profilesByDescription);
                }
                else
                {
                    return new UifJsonResult(false, null);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }


        public ActionResult SaveProfile(ProfileModelsView profileModel)
        {
            try
            {
                if (DelegateService.uniqueUserService.GetProfileByDescription(profileModel.Description) && profileModel.Id == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ExistProfile);
                }
                else
                {
                    Profile profile = ModelAssembler.CreateProfile(profileModel);
                    if (profileModel.profileAccesses != null)
                    {
                        profileModel.profileAccesses = profileModel.profileAccesses;
                    }                    
                    profile.profileAccesses = ModelAssembler.CreateProfiles(profileModel.profileAccesses, profile.Id);
                    if (profileModel.guaranteeStatus != null)
                    {
                        profileModel.guaranteeStatus = profileModel.guaranteeStatus;
                    }
                    profile.guaranteeProfileStatus = ModelAssembler.CreateGuaranteeProfiles(profileModel.guaranteeStatus, profile.Id);
                    int id = DelegateService.uniqueUserService.CreateProfile(profile);
                    profiles = new List<ProfileModelsView>();
                    GetListProfile();
                    return new UifJsonResult(true, id);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public ActionResult GetAccessType()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<AccessObjectType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAccessType);
            }
        }
        public ActionResult CopyProfile(ProfileModelsView profileModel)
        {
            try
            {
                if (DelegateService.uniqueUserService.GetProfileByDescription(profileModel.Description))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ExistProfile);
                }
                else
                {
                    Profile profile = ModelAssembler.CreateProfile(profileModel);
                    bool result = DelegateService.uniqueUserService.CopyProfile(profile);
                    if (result)
                    {
                        profiles = new List<ProfileModelsView>();
                        GetListProfile();
                        return new UifJsonResult(true, profiles.Where(x => x.Description == profileModel.Description).FirstOrDefault());
                    }
                    else
                    {
                        return new UifJsonResult(true, App_GlobalResources.Language.ExistProfile);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public ActionResult GetAccessProfile(int moduleId, int subModuleId, int typeId, int profileId, int parent)
        {
            try
            {
                List<AccessObject> currentAccesses = new List<AccessObject>();
                List<AccessObject> AccessProfiles = new List<AccessObject>();
                List<ProfileAccessView> profileAccess = new List<ProfileAccessView>();
                ProfileAccessView profile;
                currentAccesses = DelegateService.uniqueUserService.GetAccessObjectByModuleIdSubModuleId(moduleId, subModuleId, typeId, profileId, parent);

                foreach (AccessObject prof in currentAccesses)
                {
                    profile = new ProfileAccessView();
                    profile.Description = prof.Description;
                    profile.AccessId = prof.AccessId;
                    profile.Type = EnumsHelper.GetItemName<AccessObjectType>(typeId);
                    profile.Assigned = prof.Assigned;
                    if (prof.Assigned)
                    {
                        profile.Status = App_GlobalResources.Language.Enabled;
                    }
                    profile.AccessObjectId = prof.AccessObjectId;
                    profileAccess.Add(profile);
                }
                return new UifTableResult(profileAccess);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAccessType);
            }

        }
        public ActionResult GetJSONProfile(string query)
        {
            try
            {
                GetListProfile();
                return Json(profiles.Where(x => TextHelper.replaceAccentMarks(x.Description.ToLower()).Contains(TextHelper.replaceAccentMarks(query.ToLower()))).OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public ActionResult AssignAccess(int moduleId, int subModuleId, int profileId, bool active)
        {
            try
            {
                DelegateService.uniqueUserService.AssingAllAccess(moduleId, subModuleId, profileId, active);
                return new UifJsonResult(true, App_GlobalResources.Language.AccessesSuccessfullyModified);
            }
            catch (Exception)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.ErrorAssingAccess);
            }
        }
        public void GetListProfile()
        {
            if (profiles.Count <= 0)
            {
                profilesModel = DelegateService.uniqueUserService.GetProfilesByDescription("", 0);
                profilesModel.ForEach(x => x.EnabledDescription = x.IsEnabled == true ? App_GlobalResources.Language.LabelEnabled : App_GlobalResources.Language.Disabled);
                profiles = ModelAssembler.CreateProfiles(profilesModel);
            }

        }

    }
}