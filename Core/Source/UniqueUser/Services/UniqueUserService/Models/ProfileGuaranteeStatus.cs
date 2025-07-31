using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    public class ProfileGuaranteeStatus
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int ProfileId { get; set; }
        public bool Enabled { get; set; }
    }
}
