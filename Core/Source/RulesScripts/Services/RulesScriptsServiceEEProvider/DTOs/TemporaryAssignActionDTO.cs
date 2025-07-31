using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    [Serializable]
    public class TemporaryAssignActionDTO : AssignActionDTOBase
    {
        private string _temporaryName;

        public string TemporaryName
        {
            get
            {
                return _temporaryName;
            }
            set
            {
                _temporaryName = value;
            }
        }
    }
}
