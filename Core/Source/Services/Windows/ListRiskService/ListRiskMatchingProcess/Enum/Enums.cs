using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Enum
{
    public enum RiskListEventEnum
    {
        INCLUDED = 1,
        EXCLUDED = 2
    }
    public enum RiskListTypeEnum
    {
        OWN = 1,
        OFAC = 2,
        ONU = 3
    }

    public class RiskListConstants
    {
        public static readonly string Included = "INCLUDED";
        public static readonly string Excluded = "EXCLUDED";
        public static readonly string OWN = "PROPIA";
        public static readonly string ONU = "ONU";
        public static readonly string OFAC = "OFAC";
        public static readonly string SI = "SÍ";
        public static readonly string NO = "NO";
    }

    public enum ProcessStatusEnum
    {
        [Description("Cargando")]
        Cargando = 4,
        [Description("Cargado")]
        Cargado = 5,
        [Description("Procesando")]
        Procesando = 6,
        [Description("Procesado")]
        Procesado = 7,
        [Description("ConErrores")]
        ConErrores = 8,
        [Description("SinCoincidencias")]
        SinCoincidencias = 9
    }

    public enum PersonRoleEnum
    {
        [Description("TomadorPoliza")]
        PolicyHolder = 0,
        [Description("AseguradoPoliza")]
        PolicyInsured = 1,
        [Description("IntermediarioPoliza")]
        PolicyAgent = 2,
        [Description("BeneficiarioPoliza")]
        PolicyBeneficiary = 4,
        [Description("Asegurado")]
        Insured = 7,
        [Description("Beneficiario")]
        Beneficiary = 8,
        [Description("Tercero")]
        Third = 9,
        [Description("Proveedor")]
        Supplier = 10,
        [Description("Reasegurador")]
        Reinsured = 11,
        [Description("Creacion de persona")]
        PersonCreation = 12
    }
    public enum ProcessType
    {
        [Description("Poliza")]
        Policy = 1,
        [Description("Siniestros")]
        Claim = 2,
        [Description("Pago")]
        Payment = 3,
        [Description("Creacion de persona")]
        PersonCreation = 4
    }

    public static class EnumHelper
    {
        public static T GetValueMember<T, E>(System.Enum @enum)
        {
            Type type = typeof(E);
            MemberInfo[] memInfo = type.GetMember(@enum.ToString());
            object[] attributes = memInfo[memInfo.Length - 1].GetCustomAttributes(typeof(T), false);
            return (T)attributes[attributes.Length - 1];
        }
    }

}