using Sistran.Core.Application.UnderwritingParamService.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    [DataContract]
    public class ParamQueryCoverage : BaseAllyCoverage
    {
        [DataMember]
        public BaseQueryAllyCoverage CoveragePrincipal { get; set; }
        [DataMember]
        public BaseQueryAllyCoverage AllyCoverage { get; set; }
        
        ///// <summary>
        ///// Inicializa una nueva instancia de la clase <see cref="ParamQueryCoverage"/>.
        ///// </summary>
        ///// <param name="Id">Identificador de la cobertura.</param>
        ///// <param name="CovergaPercent">Porcentaje cobertura.</param>
        ///// <param name="alliedCoverage">Cobertura aliada.</param>
        //public ParamQueryCoverage(int Id, decimal CoveragePercent) :
        //    base(Id, CoveragePercent) {}

        ///// <summary>
        ///// Inicializa una nueva instancia de la clase <see cref="ParamQueryCoverage"/>.
        ///// </summary>
        ///// <param name="Id">Identificador de la cobertura.</param>
        ///// <param name="CovergaPercent">Porcentaje cobertura.</param>
        ///// <param name="alliedCoverage">Cobertura aliada.</param>
        //public ParamQueryCoverage(int Id, decimal CoveragePercent, BaseQueryAllyCoverage coveragePrincipal,
        //    BaseQueryAllyCoverage allyCoverage) :
        //    base(Id, CoveragePercent)
        //{
        //    this.CoveragePrincipal = coveragePrincipal;
        //    this.AllyCoverage = allyCoverage;
        //}
    }
}
