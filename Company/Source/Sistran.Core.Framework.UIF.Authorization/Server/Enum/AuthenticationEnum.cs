using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Authorization.Server.Enum
{
    [DataContract]
    public enum AuthenticationEnum
    {
        isAuthenticated = 0,
        isInvalidPassword = 1,
        isUserBlocked = 2,
        isPasswordExpired = 3,
        isPasswordNearToExpire = 4,
        isUserBlockedWithTime = 5,
        isInvalidPasswordWithData = 6,
        MustChangePasssword = 7,
        IsUserExpired = 8
    }
}