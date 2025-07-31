using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnuListRisk.Models
{
    public class OnuList
    {
        public int ProcessId { get; set; }

        public string ShaCode { get; set; }

        public Guid Guid { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int StatusId { get; set; }

        public string Error { get; set; }

        public int PersonCount { get; set; }

        public int? RequestId { get; set; }
    }
}
