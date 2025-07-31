using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for AssignActionDTO.
    /// </summary>
    [Serializable]
    public class AssignActionDTO : AssignActionDTOBase
    {
        private CodeConceptExpression _concept;

        public CodeConceptExpression Concept
        {
            get { return _concept; }
            set { _concept = value; }
        }
    }
}
