using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonParamService.Models
{
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    public class ParamWorkerType
    {
        private readonly int id;
        private readonly string description;
        private readonly string smallDescription;
        private readonly bool isEnabled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="smallDescription"></param>
        /// <param name="isEnabled"></param>
        /// 

        private ParamWorkerType(int id,string description,string smallDescription,bool isEnable)
        {
            this.id = id;
            this.description = description;
            this.smallDescription = smallDescription;
            this.isEnabled = isEnable;
        }

        public int Id
        {
            get
            {
                return this.id;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="smallDescription"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        /// 
        public static Result<ParamWorkerType, ErrorModel> CreateParamWorkerType(int id, string description, string smallDescription, bool isEnabled)
        {
            List<string> error = new List<string>();

            if (string.IsNullOrEmpty(description))
            {
                error.Add(Resources.Errors.ErrorEmptyDescription);
               
            }
            if (description.Length > 50)
            {
                error.Add(Resources.Errors.ErrorLengthDescription);
            }
            if (string.IsNullOrEmpty(smallDescription))
            {
                error.Add(Resources.Errors.ErrorEmptySmallDescription);
            }
            if (smallDescription.Length > 10)
            {
                error.Add(Resources.Errors.ErrorLengthSmallDescription);
            }
            if (error.Count > 0)
            {
                return new ResultError<ParamWorkerType, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamWorkerType, ErrorModel>(new ParamWorkerType(id, description, smallDescription, isEnabled));
            }
        }

        public static ResultValue<ParamWorkerType,ErrorModel>GetParamWorkerType(int id,string description,string smallDescription, bool isEnabled)
        {
            return new ResultValue<ParamWorkerType, ErrorModel>(new ParamWorkerType(id, description, smallDescription, isEnabled));
        }


    }
}
