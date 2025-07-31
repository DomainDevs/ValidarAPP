using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Events.Models
{
   public class EmailCriteriaModelView
   {
      public string        Subject     { get; set; }

      public string        Message     { get; set; }

      public List<string>  Addressed   { get; set; }

      public List<string>  Files       { get; set; }

      public string        userId      { get; set; }

      public string        Type        { get; set; }

      public int           count       { get; set; }
   }
}