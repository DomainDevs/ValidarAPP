//Sistran Core
using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.ReportingServices.Provider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.ReportingServices.Provider.DAOs
{
    public class MassiveReportDAO
    {
        #region Instance Variables

        /// <summary>
        /// Declaración del contexto y del DataFacadeManager
        /// </summary>
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance Variables

        #region Public Methods

        #region MassiveReport

        /// <summary>
        /// SaveMassiveReport
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns>MassiveReport</returns>
        public MassiveReport SaveMassiveReport(MassiveReport massiveReport)
        {
            try
            {
                // Convertir de model a entity
                Entities.MassiveReport massiveReportEntity = EntityAssembler.CreateMassiveReport(massiveReport);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(massiveReportEntity);

                // Return del model
                return ModelAssembler.CreateMassiveReport(massiveReportEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// UpdateMassiveReport
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns>MassiveReport</returns>
        public MassiveReport UpdateMassiveReport(MassiveReport massiveReport)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.MassiveReport.CreatePrimaryKey(massiveReport.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.MassiveReport massiveReportEntity = (Entities.MassiveReport)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Actualización en la generación de archivo
                if (massiveReport.RecordsNumber == -1)
                {
                    massiveReportEntity.UrlFile = massiveReport.UrlFile;
                }
                else if (massiveReport.RecordsNumber == -2)
                {
                    massiveReportEntity.RecordsProcessed = massiveReport.RecordsProcessed;
                    massiveReportEntity.UrlFile = massiveReport.UrlFile;
                }
                else if (massiveReport.RecordsNumber == -3)
                {
                    massiveReportEntity.RecordsProcessed = massiveReport.RecordsProcessed;
                    massiveReportEntity.EndDate = massiveReport.EndDate;
                }
                else
                {
                    massiveReportEntity.Status = Convert.ToInt16(massiveReport.Success);
                    massiveReportEntity.UrlFile = massiveReport.UrlFile;
                    massiveReportEntity.UserId = massiveReport.UserId;
                    massiveReportEntity.RecordsProcessed = massiveReport.RecordsProcessed;
                }


                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(massiveReportEntity);

                // Return del model
                return ModelAssembler.CreateMassiveReport(massiveReportEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteMassiveReport
        /// </summary>
        /// <param name="massiveReport"></param>
        public void DeleteMassiveReport(MassiveReport massiveReport)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.MassiveReport.CreatePrimaryKey(massiveReport.Id);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.MassiveReport massiveReportEntity = (Entities.MassiveReport)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(massiveReportEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetMassiveReport
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns>MassiveReport</returns>
        public MassiveReport GetMassiveReport(MassiveReport massiveReport)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.MassiveReport.CreatePrimaryKey(massiveReport.Id);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.MassiveReport massiveReportEntity = (Entities.MassiveReport)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateMassiveReport(massiveReportEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetMassiveReports
        /// </summary>
        /// <returns>List<MassiveReport></returns>
        public List<MassiveReport> GetMassiveReports()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(Entities.MassiveReport.Properties.MassiveReportCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(Entities.MassiveReport), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                List<MassiveReport> massiveReports = new List<MassiveReport>();
                foreach (Entities.MassiveReport massiveReportEntity in businessCollection.OfType<Entities.MassiveReport>())
                {
                    massiveReports.Add(new MassiveReport
                    {
                        Description = massiveReportEntity.Description,
                        EndDate = Convert.ToDateTime(massiveReportEntity.EndDate),
                        GenerationDate = massiveReportEntity.GenerationDate,
                        Id = massiveReportEntity.MassiveReportCode,
                        StartDate = massiveReportEntity.StartDate,
                        Success = Convert.ToBoolean(massiveReportEntity.Status),
                        UrlFile = massiveReportEntity.UrlFile,
                        UserId = massiveReportEntity.UserId
                    });
                }

                return massiveReports;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetMassiveReportsByUser
        /// </summary>
        /// <param name="massiveReport"></param>
        /// <returns>List<MassiveReport></returns>
        public List<MassiveReport> GetMassiveReportsByUser(MassiveReport massiveReport)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(Entities.MassiveReport.Properties.UserId, massiveReport.UserId);

                if (massiveReport.ModuleId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(Entities.MassiveReport.Properties.ModuleId, massiveReport.ModuleId);
                }
                if (massiveReport.StartDate != Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(Entities.MassiveReport.Properties.GenerationDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(massiveReport.StartDate);
                }

                if (massiveReport.EndDate != Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(Entities.MassiveReport.Properties.GenerationDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(massiveReport.EndDate);
                }

                if (massiveReport.Description != "0")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(Entities.MassiveReport.Properties.Description, massiveReport.Description);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().
                    SelectObjects(typeof(Entities.MassiveReport), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                List<MassiveReport> massiveReports = new List<MassiveReport>();
                foreach (Entities.MassiveReport massiveReportEntity in businessCollection.OfType<Entities.MassiveReport>())
                {
                    massiveReports.Add(new MassiveReport
                    {
                        Description = massiveReportEntity.Description,
                        EndDate = Convert.ToDateTime(massiveReportEntity.EndDate),
                        GenerationDate = massiveReportEntity.GenerationDate,
                        Id = massiveReportEntity.MassiveReportCode,
                        StartDate = massiveReportEntity.StartDate,
                        Success = Convert.ToBoolean(massiveReportEntity.Status),
                        UrlFile = massiveReportEntity.UrlFile,
                        UserId = massiveReportEntity.UserId,
                        RecordsNumber = Convert.ToInt32(massiveReportEntity.RecordsNumber),
                        RecordsProcessed = Convert.ToInt32(massiveReportEntity.RecordsProcessed)
                    });
                }

                return massiveReports;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion Public Methods

    }
}
