// -----------------------------------------------------------------------
// <copyright file="BranchDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.CommonParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Co.Application.Data;
    using CommonParamService.Assemblers;
    using CommonParamService.Models;
    using Sistran.Core.Application.CommonParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using utilitiesProvider =  Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using UnderwritingParamService.EEProvider.Business;
    using Utilities.Error;
    using COMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;    
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;

    /// <summary>
    /// DAO De Sucursal
    /// </summary>
    public class BranchDAO
    {
        /// <summary>
        /// se inicializa el BranchBusiness
        /// </summary>
        private BranchBusiness branchBusiness = new BranchBusiness();

        /// <summary>
        /// Lista sucursales
        /// </summary>       
        /// <returns>Objeto de ParamParameter</returns>
        public UTMO.Result<List<ParamBranch>, ErrorModel> GetBranch()
        {
            try
            {
                List<ParamBranch> companyBranchs = new List<ParamBranch>();
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMEN.Branch)));
                if (businessCollection.Count > 0)
                {
                    companyBranchs = ModelAssembler.CreateCompanyBranchs(businessCollection);
                }               

                return new UTMO.ResultValue<List<ParamBranch>, UTMO.ErrorModel>(companyBranchs);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(CommonParamService.Resources.Errors.ErrorGetParameter);
                return new UTMO.ResultError<List<ParamBranch>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Lista sucursales
        /// </summary>       
        /// <returns>Objeto de ParamParameter</returns>
        public UTMO.Result<List<ParamCoBranch>, ErrorModel> GetBranches()
        {
            try
            {
                List<ParamBranch> companyBranchs = new List<ParamBranch>();
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMEN.Branch)));
                if (businessCollection.Count > 0)
                {
                    companyBranchs = ModelAssembler.CreateCompanyBranchs(businessCollection);
                }

                List<ParamCoBranch> paramCoBranch = this.GetCobranches(companyBranchs);              

                return new UTMO.ResultValue<List<ParamCoBranch>, UTMO.ErrorModel>(paramCoBranch);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(CommonParamService.Resources.Errors.ErrorGetParameter);
                return new UTMO.ResultError<List<ParamCoBranch>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Lista las Co-sucursales
        /// </summary>
        /// <param name="description"> descripcion de la sucursal</param>
        /// <returns>Lista de Co-sucursales</returns>
        public UTMO.Result<List<ParamCoBranch>, ErrorModel> GetBranchesByDescription(string description)
        {
            try
            {
                List<ParamBranch> companyBranchs = new List<ParamBranch>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMEN.Branch.Properties.Description, typeof(COMEN.Branch).Name).Like().Constant("%" + description + "%");
                filter.Or();
                filter.Property(COMEN.Branch.Properties.SmallDescription, typeof(COMEN.Branch).Name).Like().Constant("%" + description + "%");

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMEN.Branch), filter.GetPredicate()));
              
                if (businessCollection.Count > 0)
                {
                    companyBranchs = ModelAssembler.CreateCompanyBranchs(businessCollection);
                }

                List<ParamCoBranch> paramCoBranch = this.GetCobranches(companyBranchs);

                return new UTMO.ResultValue<List<ParamCoBranch>, UTMO.ErrorModel>(paramCoBranch);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(CommonParamService.Resources.Errors.ErrorGetParameter);
                return new UTMO.ResultError<List<ParamCoBranch>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Une el modelo de CoBranch con el modelo de branch
        /// </summary>
        /// <param name="paramBranch">lista de modelo de negocio de sucursales</param>
        /// <returns>retorna lista de modelo de co-sucursales</returns>
        public List<ParamCoBranch> GetCobranches(List<ParamBranch> paramBranch)
        {
            List<ParamCoBranch> companyCoBranchs = new List<ParamCoBranch>();         

            foreach (var item in paramBranch)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMEN.CoBranch.Properties.BranchCode, typeof(COMEN.CoBranch).Name).Equal().Constant(item.Id);
                COMEN.CoBranch entityCoBranch = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMEN.CoBranch), filter.GetPredicate())).Cast<COMEN.CoBranch>().FirstOrDefault();
                if (entityCoBranch != null)
                {
                    ParamCoBranch paramCoBranch = new ParamCoBranch
                    {
                        Id = entityCoBranch.BranchCode,
                        Branch = new ParamBranch { Id = item.Id, Description = item.Description, SmallDescription = item.SmallDescription },
                        City = new City { Id = entityCoBranch.CityCode.Value },
                        Country = new Country { Id = entityCoBranch.CountryCode.Value },
                        PhoneType = new ParamPhoneType { Id = entityCoBranch.PhoneTypeCode.Value },
                        State = new State { Id = entityCoBranch.StateCode.Value },
                        AddressType = new ParamAddressType { Id = entityCoBranch.AddressTypeCode.Value },
                        Address = entityCoBranch.Address,
                        IsIssue = entityCoBranch.IsIssue,
                        PhoneNumber = entityCoBranch.PhoneNumber.Value
                    };

                    companyCoBranchs.Add(paramCoBranch);
                }
            }

            return companyCoBranchs.OrderBy(x => x.Branch.Description).ToList();
        }

        /// <summary>
        /// Obtener Sucursal Por Descripción
        /// </summary>
        /// <param name="description">descripcion de Sucursal</param>
        /// <returns>retorna Sucursal</returns>
        public UTMO.Result<List<ParamBranch>, UTMO.ErrorModel> GetBranchsByDescription(string description)
        {
            if (this.branchBusiness.ValidateLengthDescription(description))
            {
                List<ParamBranch> companyBranchs = new List<ParamBranch>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(COMEN.Branch.Properties.Description, typeof(COMEN.Branch).Name).Like().Constant("%" + description + "%");
                filter.Or();
                filter.Property(COMEN.Branch.Properties.SmallDescription, typeof(COMEN.Branch).Name).Like().Constant("%" + description + "%");

                List<ParamBranch> businessCollection = new List<ParamBranch>();
                var viewBuilder = new ViewBuilder("BranchView");

                BranchView branchView = new BranchView();
                viewBuilder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, branchView);

                foreach (COMEN.Branch branchc in branchView.Branch)
                {
                    COMEN.CoBranch coBranch = branchView.CoBranch.Cast<COMEN.CoBranch>().First(x => x.BranchCode == branchc.BranchCode);
                    ParamBranch resultParamBranch = ModelAssembler.CreateBranchss(branchc, coBranch);
                    businessCollection.Add(resultParamBranch);
                }                

                return new UTMO.ResultValue<List<ParamBranch>, UTMO.ErrorModel>(businessCollection);
            }
            else
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetParameter);
                return new UTMO.ResultError<List<ParamBranch>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.NotFound, null));
            }
        }

        /// <summary>
        /// Genera archivo excel de Sucursal
        /// </summary>
        /// <param name="branch">Listado de objectos de seguro</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToBranch(List<ParamBranch> branch, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationBranch
                };
                utilitiesProvider.FileDAO fileDAO = new utilitiesProvider.FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);
                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    file.Templates[0].Rows = this.AssignValues(branch, file);
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Descargando excel");
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Validación de sucursales
        /// </summary>
        /// <param name="branchId">Codigo de sucursales </param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateBranch(int branchId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@BRANCH_CD", branchId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("COMM.VALIDATE_BRANCH_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }

        /// <summary>
        /// Crea el co Branch
        /// </summary>
        /// <param name="branchParam"> modelo de negocio </param>
        /// <returns>retorna sucursal</returns>
        public Result<ParamCoBranch, ErrorModel> CreateCoBranch(ParamCoBranch branchParam)
        {
            try
            {
                ParamBranch createBranch = this.CreateBranch(branchParam.Branch);
                branchParam.Id = createBranch.Id;
                COMEN.CoBranch comBranch = this.CoBranch(branchParam);

                ParamCoBranch paramCoBranch = ModelAssembler.CreateCoBranches(comBranch);
                paramCoBranch.Branch = createBranch;
                return new UTMO.ResultValue<ParamCoBranch, UTMO.ErrorModel>(paramCoBranch);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingBranchErrorBD);
                return new UTMO.ResultError<ParamCoBranch, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// crea la sucursal
        /// </summary>
        /// <param name="branchParam">modelo de negocio de sucursal</param>
        /// <returns>retorna la sucursal </returns>
        public ParamBranch CreateBranch(ParamBranch branchParam)
        {
            branchParam.Id = this.GetIdBranch();
            COMEN.Branch branch = this.Branch(branchParam);

            ParamBranch paramBranch = ModelAssembler.CreateBranchs(branch);
            return paramBranch;
        }

        /// <summary>
        /// Lista las sucursales
        /// </summary>
        /// <returns> retorna los id</returns>
        public List<ParamBranch> GetIdBranches()
        {
            var businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMEN.Branch)));
            return businessCollection.Select(x => ModelAssembler.CreateBranchs((COMEN.Branch)x)).OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// Obtiene el Id de la sucursal
        /// </summary>
        /// <returns> retorna id </returns>
        public int GetIdBranch()
        {
            return this.GetIdBranches().Max(p => p.Id) + 1;
        }

        /// <summary>
        /// Actualiza la co sucursal
        /// </summary>
        /// <param name="branchParam">modelo de negocio de la co sucursal</param>
        /// <returns>retorna sucursal </returns>
        public Result<ParamCoBranch, ErrorModel> UpdateCoBranch(ParamCoBranch branchParam)
        {
            try
            {
                COMEN.CoBranch comBranch = this.UpdCoBranch(branchParam);
                COMEN.Branch branch = this.UpdateBranch(branchParam.Branch);             

                return new UTMO.ResultValue<ParamCoBranch, UTMO.ErrorModel>(branchParam);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingBranchErrorBD);
                return new UTMO.ResultError<ParamCoBranch, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// elimina la co sucursal 
        /// </summary>
        /// <param name="branchParam">modelo de negocio de la co sucursal</param>
        /// <returns>retorna al sucursal </returns>
        public Result<ParamCoBranch, ErrorModel> DeleteCoBranch(ParamCoBranch branchParam)
        {
            try
            {
                PrimaryKey key = COMEN.CoBranch.CreatePrimaryKey(branchParam.Id);
                var comBranchEntity = (COMEN.CoBranch)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMEN.CoBranch.Properties.BranchCode);
                filter.Equal();
                filter.Constant(comBranchEntity.BranchCode);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(COMEN.CoBranch), filter.GetPredicate());

                foreach (var itemDelete in businessCollection)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(itemDelete);
                }

                return new UTMO.ResultValue<ParamCoBranch, UTMO.ErrorModel>(branchParam);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingBranchErrorBD);
                return new UTMO.ResultError<ParamCoBranch, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// elimina la sucursal 
        /// </summary>
        /// <param name="branchParam">modelo de negocio de la co sucursal</param>
        /// <returns>retorna la sucursal </returns>
        public Result<ParamCoBranch, ErrorModel> DeleteBranch(ParamCoBranch branchParam)
        {
            List<string> listErrors = new List<string>();
            try
            {
                if (this.ValidateBranch(branchParam.Branch.Id) == 0)
                {
                    PrimaryKey key = COMEN.Branch.CreatePrimaryKey(branchParam.Branch.Id);
                    var branchEntity = (COMEN.Branch)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(COMEN.Branch.Properties.BranchCode);
                    filter.Equal();
                    filter.Constant(branchEntity.BranchCode);

                    BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(COMEN.Branch), filter.GetPredicate());

                    foreach (var itemDelete in businessCollection)
                    {
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(itemDelete);
                    }

                    return new UTMO.ResultValue<ParamCoBranch, UTMO.ErrorModel>(branchParam);
                }
                else
                {
                    listErrors.Add(string.Format(Resources.Errors.ErrorBranchUse, branchParam.Branch.Description +"("+branchParam.Branch.Id+")"));
                    return new UTMO.ResultError<ParamCoBranch, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, null));
                }
            }
            catch (System.Exception ex)
            {
                listErrors.Add(Resources.Errors.FailedUpdatingBranchErrorBD);
                return new UTMO.ResultError<ParamCoBranch, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Asigna los valores de las filas
        /// </summary>
        /// <param name="branch" >Listado de Sucursales</param>
        /// <param name="file">Configuración archivo</param>
        /// <returns>Listado filas</returns>
        private List<Row> AssignValues(List<ParamBranch> branch, File file)
        {

            List<Row> rows = new List<Row>();
            foreach (ParamBranch item in branch)
            {
                for (int i = 0; i < file.Templates[0].Rows.Count; i++)
                {
                    var fields = file.Templates[0].Rows[i].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    if (fields.Count > 1)
                    {
                        fields[0].Value = item.Id.ToString();
                        fields[1].Value = item.Description;
                        fields[2].Value = item.SmallDescription;
                        if(item.Is_issue == true)
                        {
                            fields[3].Value = Resources.Errors.IsEmisionBranch;
                        }
                        else
                        {
                            fields[3].Value = Resources.Errors.NotIsEmisionBranch;
                        }
                    }

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }
            }

            return rows;
        }

        /// <summary>
        /// Actualiza la sucursal
        /// </summary>
        /// <param name="paramBranch">modelo de negocio de la co sucursal</param>
        /// <returns>retorna la sucursal</returns>
        private COMEN.Branch UpdateBranch(ParamBranch paramBranch)
        {
            PrimaryKey key = COMEN.Branch.CreatePrimaryKey(paramBranch.Id);
            COMEN.Branch branchEntity = (COMEN.Branch)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            branchEntity.Description = paramBranch.Description;
            branchEntity.SmallDescription = paramBranch.SmallDescription;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(branchEntity);
            return branchEntity;
        }

        /// <summary>
        /// Actualiza la co sucursal
        /// </summary>
        /// <param name="branchParam">modelo de negocio de la co sucursal</param>
        /// <returns>retorna la sucursal</returns>
        private COMEN.CoBranch UpdCoBranch(ParamCoBranch branchParam)
        {
            PrimaryKey key = COMEN.CoBranch.CreatePrimaryKey(branchParam.Id);
            COMEN.CoBranch comBranchEntity = (COMEN.CoBranch)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            comBranchEntity.Address = branchParam.Address;
            comBranchEntity.AddressTypeCode = branchParam.AddressType?.Id;
            comBranchEntity.CountryCode = branchParam.Country?.Id;
            comBranchEntity.CityCode = branchParam.City?.Id;
            comBranchEntity.IsIssue = branchParam.IsIssue;
            comBranchEntity.PhoneNumber = branchParam.PhoneNumber;
            comBranchEntity.PhoneTypeCode = branchParam.PhoneType?.Id;
            comBranchEntity.StateCode = branchParam.State?.Id;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(comBranchEntity);

            return comBranchEntity;
        }

        /// <summary>
        /// convierte de modelo de negocio a entidad
        /// </summary>
        /// <param name="branchParam">modelo de negocio de la co sucursal</param>
        /// <returns>retorna la entidad de la sucursal </returns>
        private COMEN.CoBranch CoBranch(ParamCoBranch branchParam)
        {
            COMEN.CoBranch comBranchEntity = EntityAssembler.CreateCoBranch(branchParam);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(comBranchEntity);

            return comBranchEntity;
        }

        /// <summary>
        /// convierte de modelo de negocio a entidad
        /// </summary>
        /// <param name="branchParam">modelo de negocio de la sucursal</param>
        /// <returns>retorna la entidad de la sucursal </returns>
        private COMEN.Branch Branch(ParamBranch branchParam)
        {
            COMEN.Branch componentEntity = EntityAssembler.CreateBranch(branchParam);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(componentEntity);

            return componentEntity;
        }
    }
}
