using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    public class AgentExecutiveViewV1: BusinessView
    {
        public BusinessCollection Employees
        {
            get
            {
                return this["Employee"];
            }
        }
        public BusinessCollection Persons
        {
            get
            {
                return this["Person"];
            }
        }
    }
}
