using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ConsultaPoliza2018.Models
{
    public class ContextAxa : DbContext
    {
        public ContextAxa() : base("DBContext")
        {

        }
    }
}