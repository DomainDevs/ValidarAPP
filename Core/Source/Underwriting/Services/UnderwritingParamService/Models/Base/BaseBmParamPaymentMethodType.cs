using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;
    public class BaseBmParamPaymentMethodType: Extension
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int id;

        /// <summary>
        /// 
        /// </summary>
        private readonly string description;

        /// <summary>
        /// 
        /// </summary>
        protected BaseBmParamPaymentMethodType (int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        

    }
}
