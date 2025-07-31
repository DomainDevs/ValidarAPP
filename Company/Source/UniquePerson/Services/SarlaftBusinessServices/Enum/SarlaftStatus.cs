using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Enum
{
    public enum SarlaftStatus
    {
        Original = 1,
        Create = 2,
        Update = 3,
        Delete = 4,
        Error = 5
    }

    public enum SarlaftValidationState
    {
        NOT_EXISTS = 0,
        EXPIRED = 1,
        OVERCOME = 2,
        ACCURATE = 3,
        PENDING = 4
    };
}
