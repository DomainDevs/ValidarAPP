namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.UnderwritingParamService.Resources;
    using Sistran.Core.Application.Extensions;

    public class BaseBmParamPaymentMethod: Extension
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
        private readonly string tinyDescription;

        /// <summary>
        /// 
        /// </summary>
        private readonly string smallDescription;

        

        protected BaseBmParamPaymentMethod(int id, string description, string tinyDescription, string smallDescription)
        {
            this.id = id;
            this.description = description;
            this.tinyDescription = tinyDescription;
            this.smallDescription = smallDescription;
            
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

        /// <summary>
        /// 
        /// </summary>
        public string TinyDescription
        {
            get
            {
                return this.tinyDescription;
            }
        }

        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }
        
    }
}
