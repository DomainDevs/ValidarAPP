using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class UserContextPermissionsModelsView
    {
      /// <summary>
      /// id 
      /// </summary>
      public int Id { get; set; }

      public int PermissionsId { get; set; }
      /// <summary>
      /// 
      /// </summary>
      public int ProfileId { get; set; }
        
      public string Description { get; set; }

     public bool Assigned { get; set; }

     public int securityContextId { get; set; }

    }
}