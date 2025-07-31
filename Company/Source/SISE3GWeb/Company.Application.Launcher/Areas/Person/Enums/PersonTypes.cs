using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Enums
{
    public enum IndividualNew
    {
        New = -1,
        Prospect = 0
    }
    public enum GuaranteeType
    {
        Mortage = 1,
        Pledge = 2,
        Fixedtermdeposit = 3,
        Promissorynote = 4,
        Others = 5,
        Actions =6
    }
    public enum parameterType
    {
        HardRiskType = 10004,
        FutureSociety = 1009
    }

    public enum IndividualRol
    {
        //ASEGURADO
        Insured = 1,
        //INTERMEDIARIO
        Intermediary = 2,
        //EMPLEADO
        Employee = 3,
        //PROVEEDOR
        Provider = 4,
        //TERCERO
        Third = 5,
        //REASEGURADOR
        Reinsured = 6,
        //COASEGURADOR
        Coinsurant=7
    }
}