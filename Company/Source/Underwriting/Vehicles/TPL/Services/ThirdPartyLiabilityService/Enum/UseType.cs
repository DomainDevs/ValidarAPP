using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Enum
{
    public enum UseType
    {
        [Description("TPTE. PERSONAS")] Tpte_Personas = 1,
        [Description("TPTE. DE CARGA")] Tpte_De_Carga = 2,
        [Description("CUERPO DIPLOMAT")] Cuerpo_Diplomatico = 3,
        [Description("VEH.EN DEMOSTRA")] Veh_En_Demostra = 4,
        [Description("PARA ALQUILER")] Para_Alquiler = 5,
        [Description("PARA ENSEÑANZA")] Para_Enseñanza = 6,
        [Description("CUERPO DE BOMBE")] Cuerpo_De_Bombe = 7,
        [Description("AMBULANCIA")] Ambulancia = 8,
        [Description("FUERZAS MILITAR")] Fuerza_Militar = 9,
        [Description("RECOLECTORES DE")] Recolectores_De = 10,
        [Description("SERVICIO PUBLIC")] Servicio_Publico = 11,
        [Description("SERV.PUBLICO IN")] Servicio_Publico_In = 12,
        [Description("TRANPORTE PERSO")] Transporte_Personas = 13,
        [Description("FAMILIAR")] Familiar = 14,
        [Description("ESPECIAL")] Especial = 16,
        [Description("PUBLICO URBANO")] Publico_Urbano = 18,
        [Description("OTRO SERVICIO")] Otro_Servicio = 19,
        [Description("N/A")] NoAplica = 20

    }
}
