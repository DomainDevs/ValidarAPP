using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Enums
{
    public enum PersonNaturalBasic
    {
        EconomicActivityId = 1
    }

    public enum PersonLegalBasic
    {
        Id = -1,
        EconomicActivityId = 1,
        AssociationTypeId = 1,
        CompanyTypeId = 5
    }
    public enum PhoneType
    {
        OFICINA = 1,
        DIRECTO = 2,
        FAX = 3,
        DOMICILIO = 4,
        PBXCONMUTADOR = 5,
        EMAIL = 6,
        CELULAR = 7,
        OTRO = 8,
    }

    public enum AddressBasicPerson
    {
        AddressTypeId = 1,
        AddressCityId = 1,
        AddressStateId = 11,
        AddressCountryId = 1,
    }

    public enum PhoneBasicPerson
    {
        PhoneId = 0,  
        PhoneTypeId = 1
    }

    public enum EmailBasicPerson
    {
        EmailId = 0,
        EmailTypeId = 6
    }

    public enum BankTransferBasic
    {
        SUCURSAL
    }

    public enum TaxBasic
    {
        CategoryId = 1
    }
}
