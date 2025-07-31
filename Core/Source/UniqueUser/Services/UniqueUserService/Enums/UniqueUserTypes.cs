using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Enums
{
    public class UniqueUserTypes
    {
        [Flags]
        public enum AuthenticationType
        {
            [EnumMember]
            Integrated = 1,
            [EnumMember]
            Standard = 2
        }
        [Flags]
        public enum StatusItem
        {
            [EnumMember]
            Deleted = 1,
            [EnumMember]
            Modified = 2
        }
        [Flags]
        public enum DataBasesEnum
        {
            [EnumMember]
            DataBase1 = 1,
        }

        public enum ChangePasswordErrors
        {
            OldPassswordDoesntMatch = 1,
            ContainsUserName = 2,
            minLenght = 3,
            FirstMustNonNumber = 4,
            LastMustNotNumber = 5,
            MustHaveLower = 6,
            MustHaveUpper = 7,
            MustHaveNumber = 8,
            MustHaveSpecial = 9,
            CantHaveSecuence = 10,
            SimilarTohistorical = 11
        }
    }
}
