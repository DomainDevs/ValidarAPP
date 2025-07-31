// -----------------------------------------------------------------------
// <copyright file="DelegationDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UNUEN = Sistran.Core.Application.UniqueUser.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Define la clase <see cref="DelegationDAO" />
    /// </summary>
    public class DelegationDAO
    {
        /// <summary>
        /// Metodo para crear delegacion
        /// </summary>
        /// <param name="parametrizationDelegation">Recibe nueva delegacion</param>
        /// <returns>Retorna delegaciones creadas</returns>
        public UTMO.Result<ParamHierarchyAssociation, UTMO.ErrorModel> CreateParametrizationDelegation(ParamHierarchyAssociation parametrizationDelegation)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                UNUEN.CoHierarchyAssociation delegationEntity = EntityAssembler.CreateDelegationParam(parametrizationDelegation);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(delegationEntity);
                ParamHierarchyAssociation parametrizationDelegationResult = ModelAssembler.CreateDelegation(delegationEntity);
                return new UTMO.ResultValue<ParamHierarchyAssociation, UTMO.ErrorModel>(parametrizationDelegationResult);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorCreateParametrizationDelegation");
                return new UTMO.ResultError<ParamHierarchyAssociation, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Genera archivo excel de delegaciones
        /// </summary>
        /// <param name="delegations">listado de delegaciones a exportar</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>lista de delegaciones - MOD-B</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToDelegation(List<ParamHierarchyAssociation> delegations, string fileName)
        {

        
            List<string> listErrors = new List<string>();
            string value;
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationDelegations
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();                    

                    foreach (ParamHierarchyAssociation item in delegations)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x=> x.Order).Select(x => new Field
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

                        fields[0].Value = item.Description.ToString();
                        fields[1].Value = item.ParamModulen.Description.ToString();
                        fields[2].Value = item.ParamSubModule.Description.ToString();
                        fields[3].Value = item.ParamHierarchy.Description.ToString();
                        if(item.IsExclusionary == true)
                        {
                            value = "SI";
                            fields[4].Value = value.ToString();
                        }
                        else
                        {
                            value = "NO";
                            fields[4].Value = value.ToString();
                        }
                        
                        if(item.IsEnabled == true)
                        {
                            value = "SI";
                            fields[5].Value = value.ToString();
                        }
                        else
                        {
                            value = "NO";
                            fields[5].Value = value.ToString();
                        }

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene consulta delegados, jerarquias, modulos y submodulos
        /// </summary>
        /// <returns>Retorna lista de delegados</returns>
        public UTMO.Result<List<ParamHierarchyAssociation>, UTMO.ErrorModel> GetDelegationAll()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                DelegationParametrizationView view = new DelegationParametrizationView();
                ViewBuilder builder = new ViewBuilder("DelegationParametrizationView");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamHierarchyAssociation> listParametrizationDelegation = new List<ParamHierarchyAssociation>();
                List<ParamModule> listParamModule = new List<ParamModule>();

                if (view.Hierarchy_Association.Count == 0)
                {
                    errorModelListDescription.Add("Resources.Errors.ErrorGetDelegation");
                    return new UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.NotFound, null));
                }
                else
                {
                    listParametrizationDelegation = ModelAssembler.CreateDelegations(view.Hierarchy_Association);
                    listParamModule = ModelAssembler.CreateModules(view.Modules);
                    List<ParamSubModule> listSubModule = ModelAssembler.CreateSubModules(view.SubModules);
                    List<ParamHierarchy> listHierarchy = ModelAssembler.CreateHierarchies(view.Hierarchy);
                    foreach (ParamHierarchyAssociation delegation in listParametrizationDelegation)
                    {
                        delegation.ParamModulen = listParamModule.First(x => x.Id == delegation.ParamModulen.Id);
                        delegation.ParamSubModule = listSubModule.First(x => x.Id == delegation.ParamSubModule.Id);
                        delegation.ParamHierarchy = listHierarchy.First(x => x.Id == delegation.ParamHierarchy.Id);
                    }
                }

                return new UTMO.ResultValue<List<ParamHierarchyAssociation>, UTMO.ErrorModel>(listParametrizationDelegation);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetDelegation");
                return new UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Obtiene consulta delegados, jerarquias, modulos y submodulos
        /// </summary>
        /// <param name="description">Descripcion de la delegacion</param>
        /// <returns>Retorna lista de delegados</returns>
        public UTMO.Result<List<ParamHierarchyAssociation>, UTMO.ErrorModel> GetDelegationByName(string description)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UNUEN.CoHierarchyAssociation.Properties.Description, "Hierarchy_Association");
                filter.Like();
                filter.Constant("%" + description + "%");

                DelegationParametrizationView view = new DelegationParametrizationView();
                ViewBuilder builder = new ViewBuilder("DelegationParametrizationView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamHierarchyAssociation> listParametrizationDelegation = new List<ParamHierarchyAssociation>();
                List<ParamModule> listParamModule = new List<ParamModule>();

                if (view.Hierarchy_Association.Count == 0)
                {
                    errorModelListDescription.Add("Resources.Errors.ErrorGetDelegationByName");
                    return new UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.NotFound, null));
                }
                else
                {
                    listParametrizationDelegation = ModelAssembler.CreateDelegations(view.Hierarchy_Association);
                    listParamModule = ModelAssembler.CreateModules(view.Modules);
                    List<ParamSubModule> listSubModule = ModelAssembler.CreateSubModules(view.SubModules);
                    List<ParamHierarchy> listHierarchy = ModelAssembler.CreateHierarchies(view.Hierarchy);
                    foreach (ParamHierarchyAssociation delegation in listParametrizationDelegation)
                    {
                        delegation.ParamModulen = listParamModule.First(x => x.Id == delegation.ParamModulen.Id);
                        delegation.ParamSubModule = listSubModule.First(x => x.Id == delegation.ParamSubModule.Id);
                        delegation.ParamHierarchy = listHierarchy.First(x => x.Id == delegation.ParamHierarchy.Id);
                    }
                }

                return new UTMO.ResultValue<List<ParamHierarchyAssociation>, UTMO.ErrorModel>(listParametrizationDelegation);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetDelegationByName");
                return new UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Metodo para actualizar delegaciones
        /// </summary>
        /// <param name="parametrizationDelegation">Recibe delegaciones</param>
        /// <returns>Retorna delegaciones actualizadas</returns>
        public UTMO.Result<ParamHierarchyAssociation, UTMO.ErrorModel> UpdateParametrizationDelegation(ParamHierarchyAssociation parametrizationDelegation)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                DataTable result;
                NameValue[] parameters = new NameValue[6];
                parameters[0] = new NameValue("@MODULE_ID", parametrizationDelegation.ParamModulen.Id);
                parameters[1] = new NameValue("@SUBMODULE_ID", parametrizationDelegation.ParamSubModule.Id);
                parameters[2] = new NameValue("@HIERARCHY_ID", parametrizationDelegation.ParamHierarchy.Id);
                parameters[3] = new NameValue("@DESCRIPTION", parametrizationDelegation.Description);
                parameters[4] = new NameValue("@ENABLED", parametrizationDelegation.IsEnabled);
                parameters[5] = new NameValue("@EXCLUSIONARY", parametrizationDelegation.IsExclusionary);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("[UU].[UPDATE_CO_HIERARCHY_ASSOCIATION_PARAMETRIZATION]", parameters);
                }

                return new UTMO.ResultValue<ParamHierarchyAssociation, UTMO.ErrorModel>(parametrizationDelegation);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorUpdateParametrizationDelegation");
                return new UTMO.ResultError<ParamHierarchyAssociation, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
