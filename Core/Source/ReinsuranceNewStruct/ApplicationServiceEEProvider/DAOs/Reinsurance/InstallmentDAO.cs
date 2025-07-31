//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    public class InstallmentDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveInstallment
        /// </summary>
        /// <param name="installment"></param>
        public void SaveInstallment(Installment installment)
        {
            //TmpFacultativePayments
            if (installment.LevelCompany.LevelCompanyId > 0)
            {
                REINSURANCEEN.TempInstallmentPayment entityTmpFacultativePayments = EntityAssembler.CreateTmpFacultativePayments(installment);
                _dataFacadeManager.GetDataFacade().InsertObject(entityTmpFacultativePayments);
            }
            else
            {
                //TmpFacultativeCompany
                REINSURANCEEN.TempInstallmentCompany entityFacultative = EntityAssembler.CreateFacultativeCompany(installment);
                _dataFacadeManager.GetDataFacade().InsertObject(entityFacultative);
            }
        }

        /// <summary>
        /// UpdateInstallment
        /// </summary>
        /// <param name="installment"></param>
        public void UpdateInstallment(Installment installment)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.TempInstallmentCompany.CreatePrimaryKey(installment.Id);
            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.TempInstallmentCompany entityTempFacultativeCompany = (REINSURANCEEN.TempInstallmentCompany)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityTempFacultativeCompany.BrokerReinsuranceCode = installment.LevelCompany.Agent.IndividualId;
            entityTempFacultativeCompany.ReinsuranceCompanyCode = installment.LevelCompany.Company.IndividualId;
            entityTempFacultativeCompany.CommissionPercentage = Convert.ToDecimal(installment.LevelCompany.ComissionPercentage);
            entityTempFacultativeCompany.PremiumPercentage = Convert.ToDecimal(installment.LevelCompany.GivenPercentage);
            entityTempFacultativeCompany.ParticipationPercentage = Convert.ToDecimal(installment.LevelCompany.ContractLevel.AssignmentPercentage);
            entityTempFacultativeCompany.ReservePremiumPercentage = Convert.ToDecimal(installment.LevelCompany.DepositPercentage);
            entityTempFacultativeCompany.InterestReserveRelease = Convert.ToDecimal(installment.LevelCompany.InterestOnReserve);
            entityTempFacultativeCompany.DepositReleaseDate = installment.LevelCompany.DepositReleaseDate;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityTempFacultativeCompany);
        }

        /// <summary>
        /// GetInstallmentsByLevelCompanyId
        /// Trae los planes de pago de la companía de facultativo
        /// </summary>
        /// <param name="levelCompanyId"></param>
        /// <returns>List<Installment></returns>
        public List<Installment> GetInstallmentsByLevelCompanyId(int levelCompanyId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.TempInstallmentPayment.Properties.TempInstallementCompanyCode, levelCompanyId);
            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                                            typeof(REINSURANCEEN.TempInstallmentPayment), criteriaBuilder.GetPredicate()));
            return ModelAssembler.CreateInstallment(businessCollection);
        }

        /// <summary>
        /// DeleteInstallment
        /// </summary>
        /// <param name="installment"></param>
        public void DeleteInstallment(Installment installment)
        {
            //elimina un solo registro
            if (installment.LevelCompany.LevelCompanyId == 0)
            {
                PrimaryKey primaryKey = REINSURANCEEN.TempInstallmentPayment.CreatePrimaryKey(installment.Id);
                REINSURANCEEN.TempInstallmentPayment entityPmpFacultativePayments = (REINSURANCEEN.TempInstallmentPayment)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                _dataFacadeManager.GetDataFacade().DeleteObject(entityPmpFacultativePayments);
            }
            else
            {
                //elimina todos los registros
                List<Installment> facultativePayments = GetInstallmentsByLevelCompanyId(installment.LevelCompany.LevelCompanyId);
                foreach (Installment facultativePayment in facultativePayments)
                {
                    PrimaryKey primaryKey = REINSURANCEEN.TempInstallmentPayment.CreatePrimaryKey(facultativePayment.Id);
                    REINSURANCEEN.TempInstallmentPayment entityPmpFacultativePayments = (REINSURANCEEN.TempInstallmentPayment)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                    _dataFacadeManager.GetDataFacade().DeleteObject(entityPmpFacultativePayments);
                }
            }
        }
    }
}