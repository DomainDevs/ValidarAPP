// -----------------------------------------------------------------------
// <copyright file="DocumentTypeParamDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UNENT = Sistran.Core.Application.UniquePerson.Entities;

    public class DocumentTypeParamDAO
    {
        public Result<List<ParamDocumentType>, ErrorModel> GetDocumentTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();          
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UNENT.DocumentType)));
                List<ParamDocumentType> lstDocumentType = ModelAssembler.GetDocumentTypes(businessCollection);               

                return new ResultValue<List<ParamDocumentType>, ErrorModel>(lstDocumentType);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.NoTypeOfBranchWasFound);
                return new ResultError<List<ParamDocumentType>, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
