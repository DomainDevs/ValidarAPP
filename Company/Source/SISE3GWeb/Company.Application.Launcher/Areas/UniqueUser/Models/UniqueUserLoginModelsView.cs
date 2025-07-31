using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class UniqueUserLoginModelsView
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int ExpirationsDays { get; set; }
        public bool MustChangePassword { get; set; }
    }
}