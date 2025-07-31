// -----------------------------------------------------------------------
// <copyright file="DelegationDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.DAOs
{
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using UNUEN = Sistran.Core.Application.AuthorizationPolicies.Entities;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.AuthorizationPolicies.Entities;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Entities.Views;
    using Sistran.Core.Framework.DAF.Engine;
    using System.Linq;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using System;

    public class RejectionCausesDAO
    {
        /// <summary>
        /// Obtiene consulta delegados, jerarquias, modulos y submodulos
        /// </summary>
        /// <returns>Retorna lista de delegados</returns>
        public UTMO.Result<List<ParamBaseEjectionCauses>, UTMO.ErrorModel> GetRejectionCausesAll()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBaseEjectionCauses> listParamBaseEjectionCauses = new List<ParamBaseEjectionCauses>();
                RejectionCausesParametrizationView view = new RejectionCausesParametrizationView();
                ViewBuilder builder = new ViewBuilder("RejectionCausesParametrizationView");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (RejectionCauses rejectionCauses in view.RejectionCauses)
                {
                    GroupPolicies groupPolicies = view.GroupPolicies.Cast<GroupPolicies>().First(x => x.GroupPoliciesId == rejectionCauses.GroupPoliciesId);


                    ParamBaseEjectionCauses resultParamBaseEjectionCauses = ModelAssembler.CreateRejectionCauses(rejectionCauses, groupPolicies);
                    listParamBaseEjectionCauses.Add(resultParamBaseEjectionCauses);

                }

                return new UTMO.ResultValue<List<ParamBaseEjectionCauses>, UTMO.ErrorModel>(listParamBaseEjectionCauses);
            }

            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetDelegation");
                return new UTMO.ResultError<List<ParamBaseEjectionCauses>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.AuthorizationParamPolicies.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Obtiene consulta delegados, jerarquias, modulos y submodulos
        /// </summary>
        /// <returns>Retorna lista de delegados</returns>
        public UTMO.Result<List<ParamBaseEjectionCauses>, UTMO.ErrorModel> GetBaseGroupPoliciesByDescription(string description, int  groupPolicie)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBaseEjectionCauses> listParamBaseEjectionCauses = new List<ParamBaseEjectionCauses>();
                if (description == "")
                {
                    RejectionCausesParametrizationView view = new RejectionCausesParametrizationView();
                    ViewBuilder builder = new ViewBuilder("RejectionCausesParametrizationView");
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(RejectionCauses.Properties.GroupPoliciesId, typeof(RejectionCauses).Name);
                    filter.Equal();
                    filter.Constant(groupPolicie);
                    builder.Filter = filter.GetPredicate();                   
                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                    foreach (RejectionCauses rejectionCauses in view.RejectionCauses)
                    {
                        GroupPolicies groupPolicies = view.GroupPolicies.Cast<GroupPolicies>().First(x => x.GroupPoliciesId == rejectionCauses.GroupPoliciesId);
                        ParamBaseEjectionCauses resultParamBaseEjectionCauses = ModelAssembler.CreateRejectionCauses(rejectionCauses, groupPolicies);
                        listParamBaseEjectionCauses.Add(resultParamBaseEjectionCauses);

                    }
                }else if(groupPolicie == 0)
                {
                    RejectionCausesParametrizationView view = new RejectionCausesParametrizationView();
                    ViewBuilder builder = new ViewBuilder("RejectionCausesParametrizationView");
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(RejectionCauses.Properties.Description, typeof(RejectionCauses).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                    builder.Filter = filter.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                    foreach (RejectionCauses rejectionCauses in view.RejectionCauses)
                    {
                        GroupPolicies groupPolicies = view.GroupPolicies.Cast<GroupPolicies>().First(x => x.GroupPoliciesId == rejectionCauses.GroupPoliciesId);
                        ParamBaseEjectionCauses resultParamBaseEjectionCauses = ModelAssembler.CreateRejectionCauses(rejectionCauses, groupPolicies);
                        listParamBaseEjectionCauses.Add(resultParamBaseEjectionCauses);

                    }
                }
                else
                {
                    RejectionCausesParametrizationView view = new RejectionCausesParametrizationView();
                    ViewBuilder builder = new ViewBuilder("RejectionCausesParametrizationView");
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(RejectionCauses.Properties.Description, typeof(RejectionCauses).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                    filter.And();
                    filter.Property(RejectionCauses.Properties.GroupPoliciesId, typeof(RejectionCauses).Name);
                    filter.Equal();
                    filter.Constant(groupPolicie);
                    builder.Filter = filter.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                    foreach (RejectionCauses rejectionCauses in view.RejectionCauses)
                    {
                        GroupPolicies groupPolicies = view.GroupPolicies.Cast<GroupPolicies>().First(x => x.GroupPoliciesId == rejectionCauses.GroupPoliciesId);
                        ParamBaseEjectionCauses resultParamBaseEjectionCauses = ModelAssembler.CreateRejectionCauses(rejectionCauses, groupPolicies);
                        listParamBaseEjectionCauses.Add(resultParamBaseEjectionCauses);

                    }
                }  

                return new UTMO.ResultValue<List<ParamBaseEjectionCauses>, UTMO.ErrorModel>(listParamBaseEjectionCauses);
            }

            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetDelegation");
                return new UTMO.ResultError<List<ParamBaseEjectionCauses>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.AuthorizationParamPolicies.EEProvider.DAOs");
            }
        }
       
        /// <summary>
        /// Obtiene listado de grupo de politicas
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public UTMO.Result<List<ParamBaseGroupPolicies>, UTMO.ErrorModel> GetBaseGroupPolicies()
        { 
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UNUEN.GroupPolicies)));
                List<ParamBaseGroupPolicies> paramBaseGroupPolicies = ModelAssembler.CreateBaseGroupPolicies(businessCollection);
                return new UTMO.ResultValue<List<ParamBaseGroupPolicies>, UTMO.ErrorModel>(paramBaseGroupPolicies);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetSubModules");
                return new UTMO.ResultError<List<ParamBaseGroupPolicies>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }  
            finally
            {
                 stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.AuthorizationParamPolicies.EEProvider.DAOs");
            }
        }
       
        /// <summary>
        /// Crea en base de datos motivo de rechazo
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public UTMO.Result<ParamBaseEjectionCauses, UTMO.ErrorModel> CreateRejectionCause(ParamBaseEjectionCauses paramBaseEjectionCauses)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                RejectionCauses rejectionCauses = EntityAssembler.CreateBaseRejection(paramBaseEjectionCauses);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(rejectionCauses);
                paramBaseEjectionCauses = ModelAssembler.CreateBaseRejectionCauses(rejectionCauses);
               return new UTMO.ResultValue<ParamBaseEjectionCauses, UTMO.ErrorModel>(paramBaseEjectionCauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetSubModules");
                return new UTMO.ResultError<ParamBaseEjectionCauses, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.AuthorizationParamPolicies.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// actualiza en base de datos motivo de rechazo
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public UTMO.Result<ParamBaseEjectionCauses, UTMO.ErrorModel> UpdateRejectionCause(ParamBaseEjectionCauses paramBaseEjectionCauses)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                PrimaryKey primaryKeyRejectionCauses = RejectionCauses.CreatePrimaryKey(paramBaseEjectionCauses.Id);
                RejectionCauses rejectionCauses = (RejectionCauses)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKeyRejectionCauses));
                rejectionCauses.Description = paramBaseEjectionCauses.Description;
                rejectionCauses.GroupPoliciesId = paramBaseEjectionCauses.paramBaseGroupPolicies.id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(rejectionCauses);

                PrimaryKey primarygroupPolicies = GroupPolicies.CreatePrimaryKey(paramBaseEjectionCauses.paramBaseGroupPolicies.id);
                GroupPolicies groupPolicie = (GroupPolicies)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primarygroupPolicies));
                paramBaseEjectionCauses = ModelAssembler.CreateRejectionCauses(rejectionCauses , groupPolicie);
                return new UTMO.ResultValue<ParamBaseEjectionCauses, UTMO.ErrorModel>(paramBaseEjectionCauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetSubModules");
                return new UTMO.ResultError<ParamBaseEjectionCauses, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.AuthorizationParamPolicies.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Elimina en base de datos motivo de rechazo
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public UTMO.Result<ParamBaseEjectionCauses, UTMO.ErrorModel> DeleteRejectionCause(ParamBaseEjectionCauses paramBaseEjectionCauses)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(RejectionCauses.Properties.RejectionCausesId, typeof(RejectionCauses).Name);
                filter.Equal();
                filter.Constant(paramBaseEjectionCauses.Id);

                BusinessCollection businessCollection1 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RejectionCauses), filter.GetPredicate()));
                RejectionCauses rejectionCauses = new RejectionCauses(paramBaseEjectionCauses.Id);
                foreach (RejectionCauses item in businessCollection1)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                }

                //RejectionCauses rejectionCauses = EntityAssembler.CreateBaseRejection(paramBaseEjectionCauses);
                //DataFacadeManager.Instance.GetDataFacade().DeleteObject(rejectionCauses);
                return new UTMO.ResultValue<ParamBaseEjectionCauses, UTMO.ErrorModel>(paramBaseEjectionCauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetSubModules");
                return new UTMO.ResultError<ParamBaseEjectionCauses, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.AuthorizationParamPolicies.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Genera archivo excel de Metodos de rechazo
        /// </summary>
        /// <param name="RejectionCauses">listado de delegaciones a exportar</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>lista de delegaciones - MOD-B</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToRejectionCauses(List<ParamBaseEjectionCauses> rejectionCauses, string fileName)
        {


            List<string> listErrors = new List<string>();           
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationRejectionCauses
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamBaseEjectionCauses item in rejectionCauses)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x=>x.Order).Select(x => new Field
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

                        fields[0].Value = item.Id.ToString();
                        fields[1].Value = item.Description.ToString();
                        fields[2].Value = item.paramBaseGroupPolicies.description.ToString();                   

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

    }
}
