using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnuListRisk.Enum
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

    public enum OnuProcessStatusEnum
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
        [Description("ValidadoSinCambios")]
        ValidadoSinCambios = 9,

    }
}