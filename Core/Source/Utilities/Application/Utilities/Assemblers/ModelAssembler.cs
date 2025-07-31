using System.Collections.Generic;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.Utilities.Assemblers
{
    /// <summary>
    /// Convertir entidad en Modelo
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Creats the concepts.
        /// </summary>
        /// <param name="concepts">The concepts.</param>
        /// <returns></returns>
        public static List<Sistran.Core.Framework.Rules.Concept> CreatConcepts(List<SCREN.Concept> concepts)
        {
            if (concepts?.Count > 0)
            {
                var imapper = AutoMapperAssembler.CreateMapConcept();
                return imapper.Map<List<SCREN.Concept>, List<Sistran.Core.Framework.Rules.Concept>>(concepts);
            }
            else
            {
                return new List<Sistran.Core.Framework.Rules.Concept>();

            }

        }
    }
}
