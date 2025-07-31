using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.Enums
{
    [DataContract]
    public enum AuthenticationEnum
    {
        [EnumMember]
        isAuthenticated = 0,
        [EnumMember]
        isInvalidPassword = 1,
        [EnumMember]
        isUserBlocked = 2,
        [EnumMember]
        isPasswordExpired = 3,
        [EnumMember]
        isPasswordNearToExpire = 4,
        [EnumMember]
        isUserBlockedWithTime = 5,
        [EnumMember]
        isInvalidPasswordWithData = 6,
        [EnumMember]
        MustChangePasssword = 7,
        [EnumMember]
        IsUserExpired = 8,
        [EnumMember]
        IsUserWithoutProfiles = 9,
        [EnumMember]
        IsProfileDisabled = 10
    }
}
