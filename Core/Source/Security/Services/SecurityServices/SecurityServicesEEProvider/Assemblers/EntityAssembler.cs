using Sistran.Core.Application.UniqueUser.Entities;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        public static List<ProfileUniqueUser> CreateProfileUniqueUser (BusinessCollection businessCollection)
        {
            return businessCollection.Cast<ProfileUniqueUser>().ToList();
        }
        public static List<Profiles> CreateProfile(BusinessCollection businessCollection)
        {
            return businessCollection.Cast<Profiles>().ToList();
        }
    }
}
