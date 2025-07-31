using System.Collections;

//namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs no borrar
namespace Sistran.Core.Application.Scripts.Entities
{
    /// <summary>
    /// Summary description for IDynamicConceptContainer.
    /// </summary>
    public interface IDynamicConceptContainer : IEnumerable
    {
        object GetDynamicConcept(int conceptId);
        void SetDynamicConcept(int conceptId, object value);
    }
}
